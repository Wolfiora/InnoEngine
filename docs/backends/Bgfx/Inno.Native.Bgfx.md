# Inno.Native.Bgfx

[分类索引](README.md) · [Rendering BGFX](Inno.Adapter.Rendering.Bgfx.md) · [Wiki 首页](../../README.md)

该项目提供 BGFX C API 的 generated binding surface。`Generated/Bindings.cs` 由 BGCS 生成 handle、enum、descriptor、callback 和 API；`bgfx.cs` 只负责原生库加载。完整声明见本页生成 ABI 索引；这些声明保持 backend 边界，不成为中立领域 API。

离线工具执行器 `BgfxTool`、`ToolRunner` 和 `ToolRunResult` 属于
[BGFX build toolchain](Inno.Build.Toolchains.Bgfx.Tools.md) 的 `Execution/`。
Native 项目只持有绑定和库加载边界，不引用离线进程执行、Shader 创作或构建策略。
宿主明确部署本次工具产物，运行时加载器不扫描仓库或复制旧 `.lib` 输出。

只有 `Inno.Adapter.Rendering.Bgfx`、BGFX toolchain 和 Native tests 可以引用本项目。上层 public/protected API 不得泄漏任一 BGFX 类型或原生指针。

## 静态目标

同一项目通过 bindgen.browser-wasm.json 生成 wasm32 的静态符号绑定，继承 common.json 的声明；目标编译使用 INNO_STATIC_NATIVE，不初始化动态 loader。Host profile 仍使用原动态库命名与显式初始化。生成输出分别属于宿主 Generated/Bindings.cs 和目标 obj/browser-wasm/<generationFingerprint>/Generated/Bindings.cs，没有 Native Browser 副本。

## 源码归属

当前唯一源码 owner：`backends/Bgfx/native/Inno.Native.Bgfx/Inno.Native.Bgfx.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Native.Bgfx.bgfx`

<!-- api: T:Inno.Native.Bgfx.bgfx b565d3fb16c45cb493209a0c0900ec59bee9b046223c4cc8aaaa14e084abf33e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6653f12dad3194b1"></a>[`public static class Inno.Native.Bgfx.bgfx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/bgfx.cs#L8) | bgfx native bindings loader. | — | — |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `$(BGCSRuntimeProject)`：公开引用边界由实际签名核对。







<!-- generated-abi-index:start -->

## 生成 ABI 成员索引

以下索引取自当前 Debug 宿主配置的实际语义符号，覆盖生成类型、函数、字段、枚举值、委托及包装视图。它属于 backend 的低层接入参考，不是游戏脚本或中立服务的扩展 API。不同目标必须重新选择绑定定义并匹配 Native 产物；不能用宿主表格证明另一 ABI 的正确性。

按类型折叠成员，保留准确签名、参数、返回类型、常量值和源码位置。已有生成说明直接列出；未提供说明的声明只记录 ABI 形状，不推断原生函数的副作用、线程安全或销毁权。具体使用流程见本页生命周期与对应 Adapter 页面。原生地址、视图和 callback 只在所属 Native owner 有效期间使用；未注明的额外约束以“—”表示，不意味着线程安全或自动取得销毁权。

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
| <a id="api-23db435917e7bdf2"></a>[`public const int Inno.Native.Bgfx.bgfx._BUFFER_COMPUTE_FORMAT_SHIFT = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3819) | 当前常量或枚举值：`0`。 | — |
| <a id="api-f9dcf9cf4331573b"></a>[`public const int Inno.Native.Bgfx.bgfx._BUFFER_COMPUTE_TYPE_SHIFT = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3820) | 当前常量或枚举值：`4`。 | — |
| <a id="api-6d22ecae24b8f3bb"></a>[`public const int Inno.Native.Bgfx.bgfx._RESET_FULLSCREEN_SHIFT = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3833) | 当前常量或枚举值：`0`。 | — |
| <a id="api-49c5fa3575807782"></a>[`public const int Inno.Native.Bgfx.bgfx._RESET_MSAA_SHIFT = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3832) | 当前常量或枚举值：`4`。 | — |
| <a id="api-5dce4a5b092ad3cd"></a>[`public const int Inno.Native.Bgfx.bgfx._RESET_RESERVED_SHIFT = 31`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3834) | 当前常量或枚举值：`31`。 | — |
| <a id="api-5d2aec46e79ebeb2"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_BORDER_COLOR_SHIFT = 24`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3830) | 当前常量或枚举值：`24`。 | — |
| <a id="api-4de3da5a238f075e"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_COMPARE_SHIFT = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3829) | 当前常量或枚举值：`16`。 | — |
| <a id="api-47db570615b4bc8e"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_MAG_SHIFT = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3827) | 当前常量或枚举值：`8`。 | — |
| <a id="api-3326c3b119e08b7f"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_MIN_SHIFT = 6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3826) | 当前常量或枚举值：`6`。 | — |
| <a id="api-b4066c407f53f77a"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_MIP_SHIFT = 10`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3828) | 当前常量或枚举值：`10`。 | — |
| <a id="api-0006a57781a3581d"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_RESERVED_SHIFT = 28`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3831) | 当前常量或枚举值：`28`。 | — |
| <a id="api-250d4bf7a47bc8ea"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_U_SHIFT = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3823) | 当前常量或枚举值：`0`。 | — |
| <a id="api-46ffe328edff07be"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_V_SHIFT = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3824) | 当前常量或枚举值：`2`。 | — |
| <a id="api-4cdd144afac3c084"></a>[`public const int Inno.Native.Bgfx.bgfx._SAMPLER_W_SHIFT = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3825) | 当前常量或枚举值：`4`。 | — |
| <a id="api-203f36d3179c05ed"></a>[`public const int Inno.Native.Bgfx.bgfx._SHARED_LIB_BUILD = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3803) | 当前常量或枚举值：`0`。 | — |
| <a id="api-9286f8ed7f9f0c2f"></a>[`public const int Inno.Native.Bgfx.bgfx._SHARED_LIB_USE = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3804) | 当前常量或枚举值：`0`。 | — |
| <a id="api-9be693aab02c2f72"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_ALPHA_REF_SHIFT = 40`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3809) | 当前常量或枚举值：`40`。 | — |
| <a id="api-a1e2edd38318c9c0"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_BLEND_EQUATION_SHIFT = 28`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3807) | 当前常量或枚举值：`28`。 | — |
| <a id="api-4baadc4c0f5b68b3"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_BLEND_SHIFT = 12`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3806) | 当前常量或枚举值：`12`。 | — |
| <a id="api-e67779b101fb0079"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_CULL_SHIFT = 36`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3808) | 当前常量或枚举值：`36`。 | — |
| <a id="api-1dfb97525b6af8c7"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_DEPTH_TEST_SHIFT = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3805) | 当前常量或枚举值：`4`。 | — |
| <a id="api-6956a8ad4f36c2ad"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_POINT_SIZE_SHIFT = 52`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3811) | 当前常量或枚举值：`52`。 | — |
| <a id="api-1bffaa48fa262852"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_PT_SHIFT = 48`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3810) | 当前常量或枚举值：`48`。 | — |
| <a id="api-cbbb10afa3ed84c1"></a>[`public const int Inno.Native.Bgfx.bgfx._STATE_RESERVED_SHIFT = 61`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3812) | 当前常量或枚举值：`61`。 | — |
| <a id="api-d96634bff573db39"></a>[`public const int Inno.Native.Bgfx.bgfx._STENCIL_FUNC_REF_SHIFT = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3813) | 当前常量或枚举值：`0`。 | — |
| <a id="api-aa13261ba9ce80dd"></a>[`public const int Inno.Native.Bgfx.bgfx._STENCIL_FUNC_RMASK_SHIFT = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3814) | 当前常量或枚举值：`8`。 | — |
| <a id="api-a564ce60e14b8097"></a>[`public const int Inno.Native.Bgfx.bgfx._STENCIL_OP_FAIL_S_SHIFT = 20`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3816) | 当前常量或枚举值：`20`。 | — |
| <a id="api-5bc7a542615677cc"></a>[`public const int Inno.Native.Bgfx.bgfx._STENCIL_OP_FAIL_Z_SHIFT = 24`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3817) | 当前常量或枚举值：`24`。 | — |
| <a id="api-0f3180878aa6a088"></a>[`public const int Inno.Native.Bgfx.bgfx._STENCIL_OP_PASS_Z_SHIFT = 28`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3818) | 当前常量或枚举值：`28`。 | — |
| <a id="api-844dec506124d7a3"></a>[`public const int Inno.Native.Bgfx.bgfx._STENCIL_TEST_SHIFT = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3815) | 当前常量或枚举值：`16`。 | — |
| <a id="api-25e7390e0e22c9d0"></a>[`public const int Inno.Native.Bgfx.bgfx._TEXTURE_RT_MSAA_SHIFT = 36`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3821) | 当前常量或枚举值：`36`。 | — |
| <a id="api-2bd84794b613cabe"></a>[`public const int Inno.Native.Bgfx.bgfx._TEXTURE_RT_SHIFT = 36`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3822) | 当前常量或枚举值：`36`。 | — |
| <a id="api-e4c50d79e34a1a75"></a>[`public static void Inno.Native.Bgfx.bgfx.alloc_instance_data_buffer(ref Inno.Native.Bgfx.bgfx.InstanceDataBuffer idb, uint num, ushort stride)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5767) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fa189a5a078a2f87"></a>[`public static void Inno.Native.Bgfx.bgfx.alloc_instance_data_buffer(Inno.Native.Bgfx.bgfx.InstanceDataBuffer* idb, uint num, ushort stride)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5761) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe6b9285ed125cd9"></a>[`public static uint Inno.Native.Bgfx.bgfx.alloc_transform(ref Inno.Native.Bgfx.bgfx.Transform transform, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6760) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1108a3763e9ff12e"></a>[`public static uint Inno.Native.Bgfx.bgfx.alloc_transform(Inno.Native.Bgfx.bgfx.Transform* transform, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6753) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7fde83b5910217b8"></a>[`public static bool Inno.Native.Bgfx.bgfx.alloc_transient_buffers(ref Inno.Native.Bgfx.bgfx.TransientVertexBuffer tvb, Inno.Native.Bgfx.bgfx.VertexLayout* layout, uint numVertices, ref Inno.Native.Bgfx.bgfx.TransientIndexBuffer tib, uint numIndices, bool index32)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5748) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d7780fd7f3bb7d34"></a>[`public static byte Inno.Native.Bgfx.bgfx.alloc_transient_buffers(Inno.Native.Bgfx.bgfx.TransientVertexBuffer* tvb, Inno.Native.Bgfx.bgfx.VertexLayout* layout, uint numVertices, Inno.Native.Bgfx.bgfx.TransientIndexBuffer* tib, uint numIndices, byte index32)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5741) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4936c6f68c0950f8"></a>[`public static void Inno.Native.Bgfx.bgfx.alloc_transient_index_buffer(ref Inno.Native.Bgfx.bgfx.TransientIndexBuffer tib, uint num, bool index32)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5717) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-961ff2be02230e7c"></a>[`public static void Inno.Native.Bgfx.bgfx.alloc_transient_index_buffer(Inno.Native.Bgfx.bgfx.TransientIndexBuffer* tib, uint num, byte index32)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5711) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-489b1e2b4ec7b180"></a>[`public static void Inno.Native.Bgfx.bgfx.alloc_transient_vertex_buffer(ref Inno.Native.Bgfx.bgfx.TransientVertexBuffer tvb, uint num, Inno.Native.Bgfx.bgfx.VertexLayout* layout)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5732) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-63ad27f8fac9ddfc"></a>[`public static void Inno.Native.Bgfx.bgfx.alloc_transient_vertex_buffer(Inno.Native.Bgfx.bgfx.TransientVertexBuffer* tvb, uint num, Inno.Native.Bgfx.bgfx.VertexLayout* layout)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5726) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8868171a9c4f5db9"></a>[`public static Inno.Native.Bgfx.bgfx.Memory* Inno.Native.Bgfx.bgfx.alloc(uint size)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5491) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-17db06a0bb24830a"></a>[`public static void Inno.Native.Bgfx.bgfx.attachment_init(ref Inno.Native.Bgfx.bgfx.Attachment @this, Inno.Native.Bgfx.bgfx.TextureHandle handle, Inno.Native.Bgfx.bgfx.Access access, ushort layer, ushort numLayers, ushort mip, byte resolve)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5250) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-092d3dc9fe1325fc"></a>[`public static void Inno.Native.Bgfx.bgfx.attachment_init(Inno.Native.Bgfx.bgfx.Attachment* @this, Inno.Native.Bgfx.bgfx.TextureHandle handle, Inno.Native.Bgfx.bgfx.Access access, ushort layer, ushort numLayers, ushort mip, byte resolve)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5244) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7138968811491c0e"></a>[`public static void Inno.Native.Bgfx.bgfx.blit(ushort id, Inno.Native.Bgfx.bgfx.TextureHandle dst, byte dstMip, ushort dstX, ushort dstY, ushort dstZ, Inno.Native.Bgfx.bgfx.TextureHandle src, byte srcMip, ushort srcX, ushort srcY, ushort srcZ, ushort width, ushort height, ushort depth)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6949) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-106c1a0fe1da2358"></a>[`public static void Inno.Native.Bgfx.bgfx.calc_texture_size(ref Inno.Native.Bgfx.bgfx.TextureInfo info, ushort width, ushort height, ushort depth, bool cubeMap, bool hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5899) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cbc4c10b3193e5b3"></a>[`public static void Inno.Native.Bgfx.bgfx.calc_texture_size(Inno.Native.Bgfx.bgfx.TextureInfo* info, ushort width, ushort height, ushort depth, byte cubeMap, byte hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5893) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c5911a5a9c118f1c"></a>[`public static Inno.Native.Bgfx.bgfx.Memory* Inno.Native.Bgfx.bgfx.copy(void* data, uint size)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5498) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fc7023daa6889073"></a>[`public static Inno.Native.Bgfx.bgfx.ProgramHandle Inno.Native.Bgfx.bgfx.create_compute_program(Inno.Native.Bgfx.bgfx.ShaderHandle csh, bool destroyShaders)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5860) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-39fbdd32e6cd876d"></a>[`public static Inno.Native.Bgfx.bgfx.ProgramHandle Inno.Native.Bgfx.bgfx.create_compute_program(Inno.Native.Bgfx.bgfx.ShaderHandle csh, byte destroyShaders)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5853) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-af76ed7b6f0f2cf0"></a>[`public static Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle Inno.Native.Bgfx.bgfx.create_dynamic_index_buffer_mem(Inno.Native.Bgfx.bgfx.Memory* mem, ushort flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5639) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c0c010ce45bfe468"></a>[`public static Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle Inno.Native.Bgfx.bgfx.create_dynamic_index_buffer(uint num, ushort flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5632) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-236d519af3820295"></a>[`public static Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle Inno.Native.Bgfx.bgfx.create_dynamic_vertex_buffer_mem(Inno.Native.Bgfx.bgfx.Memory* mem, Inno.Native.Bgfx.bgfx.VertexLayout* layout, ushort flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5665) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d0978bdb2520508c"></a>[`public static Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle Inno.Native.Bgfx.bgfx.create_dynamic_vertex_buffer(uint num, Inno.Native.Bgfx.bgfx.VertexLayout* layout, ushort flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5658) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9d0ba29bf53b5d09"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer_from_attachment(byte num, Inno.Native.Bgfx.bgfx.Attachment* attachment, bool destroyTexture)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6070) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4dbf067cc16296ff"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer_from_attachment(byte num, Inno.Native.Bgfx.bgfx.Attachment* attachment, byte destroyTexture)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6063) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5599e8fa4dd5d8bf"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer_from_handles(byte num, Inno.Native.Bgfx.bgfx.TextureHandle* handles, bool destroyTexture)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6057) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1d5bf53d472b353b"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer_from_handles(byte num, Inno.Native.Bgfx.bgfx.TextureHandle* handles, byte destroyTexture)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6050) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-82b8771d0dc14760"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer_from_nwh(void* nwh, ushort width, ushort height, Inno.Native.Bgfx.bgfx.TextureFormat format, Inno.Native.Bgfx.bgfx.TextureFormat depthFormat)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6076) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f1bf830a53c2931c"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer_scaled(Inno.Native.Bgfx.bgfx.BackbufferRatio ratio, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong textureFlags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6043) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0efccb3b8c51ac80"></a>[`public static Inno.Native.Bgfx.bgfx.FrameBufferHandle Inno.Native.Bgfx.bgfx.create_frame_buffer(ushort width, ushort height, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong textureFlags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6036) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ed04e2bbe9c40763"></a>[`public static Inno.Native.Bgfx.bgfx.IndexBufferHandle Inno.Native.Bgfx.bgfx.create_index_buffer(Inno.Native.Bgfx.bgfx.Memory* mem, ushort flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5549) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ebd3d4c69f10bba4"></a>[`public static Inno.Native.Bgfx.bgfx.IndirectBufferHandle Inno.Native.Bgfx.bgfx.create_indirect_buffer(uint num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5776) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6411b05decde4172"></a>[`public static Inno.Native.Bgfx.bgfx.OcclusionQueryHandle Inno.Native.Bgfx.bgfx.create_occlusion_query()`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6185) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7b02360fb955b6f6"></a>[`public static Inno.Native.Bgfx.bgfx.ProgramHandle Inno.Native.Bgfx.bgfx.create_program(Inno.Native.Bgfx.bgfx.ShaderHandle vsh, Inno.Native.Bgfx.bgfx.ShaderHandle fsh, bool destroyShaders)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5847) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b922495f96e29193"></a>[`public static Inno.Native.Bgfx.bgfx.ProgramHandle Inno.Native.Bgfx.bgfx.create_program(Inno.Native.Bgfx.bgfx.ShaderHandle vsh, Inno.Native.Bgfx.bgfx.ShaderHandle fsh, byte destroyShaders)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5840) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-436e7e79c40a1426"></a>[`public static Inno.Native.Bgfx.bgfx.ShaderHandle Inno.Native.Bgfx.bgfx.create_shader(Inno.Native.Bgfx.bgfx.Memory* mem)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5789) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8f19f760375d719f"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_2d_scaled(Inno.Native.Bgfx.bgfx.BackbufferRatio ratio, bool hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5944) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dd52dfb9c94263fc"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_2d_scaled(Inno.Native.Bgfx.bgfx.BackbufferRatio ratio, byte hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5937) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b3d5c5aea0ab41df"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_2d(ushort width, ushort height, bool hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags, Inno.Native.Bgfx.bgfx.Memory* mem, ulong external)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5931) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f1a34cdc007e078e"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_2d(ushort width, ushort height, byte hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags, Inno.Native.Bgfx.bgfx.Memory* mem, ulong external)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5924) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-54bc803918460306"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_3d(ushort width, ushort height, ushort depth, bool hasMips, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags, Inno.Native.Bgfx.bgfx.Memory* mem, ulong external)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5957) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-33e37475e0cb0f4c"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_3d(ushort width, ushort height, ushort depth, byte hasMips, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags, Inno.Native.Bgfx.bgfx.Memory* mem, ulong external)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5950) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-607a96a3890b7b50"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_cube(ushort size, bool hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags, Inno.Native.Bgfx.bgfx.Memory* mem, ulong external)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5970) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-83a1303f61bcdefb"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture_cube(ushort size, byte hasMips, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags, Inno.Native.Bgfx.bgfx.Memory* mem, ulong external)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5963) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-679d3226c2a35a09"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture(Inno.Native.Bgfx.bgfx.Memory* mem, ulong flags, byte skip, ref Inno.Native.Bgfx.bgfx.TextureInfo info)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5915) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ea0db0281cf0dbea"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.create_texture(Inno.Native.Bgfx.bgfx.Memory* mem, ulong flags, byte skip, Inno.Native.Bgfx.bgfx.TextureInfo* info)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5908) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6f759bb83f6219b1"></a>[`public static Inno.Native.Bgfx.bgfx.UniformHandle Inno.Native.Bgfx.bgfx.create_uniform_with_freq(byte* name, Inno.Native.Bgfx.bgfx.UniformFreq freq, Inno.Native.Bgfx.bgfx.UniformType type, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6141) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d36725a96e924aa8"></a>[`public static Inno.Native.Bgfx.bgfx.UniformHandle Inno.Native.Bgfx.bgfx.create_uniform_with_freq(string name, Inno.Native.Bgfx.bgfx.UniformFreq freq, Inno.Native.Bgfx.bgfx.UniformType type, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6148) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3e0ead37f9c917de"></a>[`public static Inno.Native.Bgfx.bgfx.UniformHandle Inno.Native.Bgfx.bgfx.create_uniform(byte* name, Inno.Native.Bgfx.bgfx.UniformType type, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6118) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-02a6730563f1e5c2"></a>[`public static Inno.Native.Bgfx.bgfx.UniformHandle Inno.Native.Bgfx.bgfx.create_uniform(string name, Inno.Native.Bgfx.bgfx.UniformType type, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6125) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-986048fd87e6d6a9"></a>[`public static Inno.Native.Bgfx.bgfx.VertexBufferHandle Inno.Native.Bgfx.bgfx.create_vertex_buffer(Inno.Native.Bgfx.bgfx.Memory* mem, Inno.Native.Bgfx.bgfx.VertexLayout* layout, ushort flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5597) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7b64de9eb17299de"></a>[`public static Inno.Native.Bgfx.bgfx.VertexLayoutHandle Inno.Native.Bgfx.bgfx.create_vertex_layout(Inno.Native.Bgfx.bgfx.VertexLayout* layout)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5584) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4fbbcbe3ff6f7002"></a>[`public static void Inno.Native.Bgfx.bgfx.dbg_text_clear(byte attr, bool small)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5531) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cc6c59370639f89f"></a>[`public static void Inno.Native.Bgfx.bgfx.dbg_text_clear(byte attr, byte small)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5525) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c269d6141643691f"></a>[`public static void Inno.Native.Bgfx.bgfx.dbg_text_image(ushort x, ushort y, ushort width, ushort height, void* data, ushort pitch)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5543) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6c330cb276c3eb14"></a>[`public static void Inno.Native.Bgfx.bgfx.dbg_text_vprintf(ushort x, ushort y, byte attr, byte* format, byte* argList)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5537) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-caa5c6c345c932a5"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5652) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e74b16731ea348ed"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5678) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-49cd3a162167659c"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_frame_buffer(Inno.Native.Bgfx.bgfx.FrameBufferHandle handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6112) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f87790dd91f7ec23"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_index_buffer(Inno.Native.Bgfx.bgfx.IndexBufferHandle handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5578) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9a15fbfa842197a9"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_indirect_buffer(Inno.Native.Bgfx.bgfx.IndirectBufferHandle handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5783) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-773517a02bd9b550"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_occlusion_query(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6208) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-75797b2be91f6dae"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_program(Inno.Native.Bgfx.bgfx.ProgramHandle handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5866) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d632036991dbb012"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_shader(Inno.Native.Bgfx.bgfx.ShaderHandle handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5834) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d090ab333494f9fe"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_texture(Inno.Native.Bgfx.bgfx.TextureHandle handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6030) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-39bb8218db68567a"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_uniform(Inno.Native.Bgfx.bgfx.UniformHandle handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6179) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e66e8d93bc62df56"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_vertex_buffer(Inno.Native.Bgfx.bgfx.VertexBufferHandle handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5626) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4dc9f7af8f800318"></a>[`public static void Inno.Native.Bgfx.bgfx.destroy_vertex_layout(Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5591) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-25d2eba666696fc8"></a>[`public static void Inno.Native.Bgfx.bgfx.discard(byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6943) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c8663e0a11bdb2b6"></a>[`public static void Inno.Native.Bgfx.bgfx.dispatch_indirect(ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.IndirectBufferHandle indirectHandle, uint start, uint num, byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6937) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-33f6314e69393327"></a>[`public static void Inno.Native.Bgfx.bgfx.dispatch(ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, uint numX, uint numY, uint numZ, byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6931) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-010166f084f040d5"></a>[`public static uint Inno.Native.Bgfx.bgfx.encoder_alloc_transform(Inno.Native.Bgfx.bgfx.Encoder @this, ref Inno.Native.Bgfx.bgfx.Transform transform, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6418) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e3ec687bc065300"></a>[`public static uint Inno.Native.Bgfx.bgfx.encoder_alloc_transform(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.Transform* transform, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6411) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4969ee82fcbd511d"></a>[`public static Inno.Native.Bgfx.bgfx.Encoder Inno.Native.Bgfx.bgfx.encoder_begin(bool forceNewEncoder)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6327) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-499d76343dba5fc1"></a>[`public static Inno.Native.Bgfx.bgfx.Encoder Inno.Native.Bgfx.bgfx.encoder_begin(byte forceNewEncoder)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6320) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-13349a233d96f685"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_blit(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.TextureHandle dst, byte dstMip, ushort dstX, ushort dstY, ushort dstZ, Inno.Native.Bgfx.bgfx.TextureHandle src, byte srcMip, ushort srcX, ushort srcY, ushort srcZ, ushort width, ushort height, ushort depth)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6619) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8c07fb6571af5639"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_discard(Inno.Native.Bgfx.bgfx.Encoder @this, byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6613) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-76500d8e39a173da"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_dispatch_indirect(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.IndirectBufferHandle indirectHandle, uint start, uint num, byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6607) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9450d699b6eb623b"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_dispatch(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, uint numX, uint numY, uint numZ, byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6601) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d18eb4d580d70db8"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_end(Inno.Native.Bgfx.bgfx.Encoder encoder)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6333) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-271f96912d94e703"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_compute_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6577) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-463505e0fbe32da2"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_compute_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6583) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-745f2a55fa71ed53"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_compute_index_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.IndexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6565) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e30e91f95e8e9e2"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_compute_indirect_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.IndirectBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6589) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a5dd088176dc20fa"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_compute_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6571) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-89177791febfc42a"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_condition(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle, bool visible)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6373) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-76ef72340ec69077"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_condition(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle, byte visible)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6367) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bc084b644481c619"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle handle, uint firstIndex, uint numIndices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6451) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-368f9646f655023c"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_dynamic_vertex_buffer_with_layout(Inno.Native.Bgfx.bgfx.Encoder @this, byte stream, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, uint numVertices, Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6481) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bad8ca1eca175265"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stream, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, uint numVertices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6475) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bb88528405f2d8ac"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_image(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.TextureHandle handle, byte mip, Inno.Native.Bgfx.bgfx.Access access, Inno.Native.Bgfx.bgfx.TextureFormat format)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6595) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-133154351cc50d7f"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_index_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.IndexBufferHandle handle, uint firstIndex, uint numIndices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6445) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-500a796291b281ec"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_instance_count(Inno.Native.Bgfx.bgfx.Encoder @this, uint numInstances)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6523) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e8ace63d23de5fb7"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_instance_data_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.InstanceDataBuffer* idb, uint start, uint num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6505) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-493ef198fd096a42"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_instance_data_from_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, uint num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6517) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5ac8dcf05b05617c"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_instance_data_from_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, uint startVertex, uint num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6511) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c8721835a30804b7"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_marker(Inno.Native.Bgfx.bgfx.Encoder @this, byte* name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6339) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-36d1025ed4e67886"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_marker(Inno.Native.Bgfx.bgfx.Encoder @this, string name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6345) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cbcb1dcbaa6085d6"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_scissor_cached(Inno.Native.Bgfx.bgfx.Encoder @this, ushort cache)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6392) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b9e8cab1a32cf07e"></a>[`public static ushort Inno.Native.Bgfx.bgfx.encoder_set_scissor(Inno.Native.Bgfx.bgfx.Encoder @this, ushort x, ushort y, ushort width, ushort height)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6385) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f4cf8a2f1116a60a"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_state(Inno.Native.Bgfx.bgfx.Encoder @this, ulong state, uint rgba)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6361) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-962d655aa954859a"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_stencil(Inno.Native.Bgfx.bgfx.Encoder @this, uint fstencil, uint bstencil)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6379) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-33bdeaccb55319d9"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_texture(Inno.Native.Bgfx.bgfx.Encoder @this, byte stage, Inno.Native.Bgfx.bgfx.UniformHandle sampler, Inno.Native.Bgfx.bgfx.TextureHandle handle, uint flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6529) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e86092ebbd81ddd3"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_transform_cached(Inno.Native.Bgfx.bgfx.Encoder @this, uint cache, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6405) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-336f76db9e11bb93"></a>[`public static uint Inno.Native.Bgfx.bgfx.encoder_set_transform(Inno.Native.Bgfx.bgfx.Encoder @this, void* mtx, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6398) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-74697adec1f59bae"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_transient_index_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.TransientIndexBuffer* tib, uint firstIndex, uint numIndices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6457) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2a56449ef249e45d"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_transient_vertex_buffer_with_layout(Inno.Native.Bgfx.bgfx.Encoder @this, byte stream, Inno.Native.Bgfx.bgfx.TransientVertexBuffer* tvb, uint startVertex, uint numVertices, Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6493) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e18da5585963bd3"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_transient_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stream, Inno.Native.Bgfx.bgfx.TransientVertexBuffer* tvb, uint startVertex, uint numVertices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6487) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9c93123f051e73d2"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_uniform(Inno.Native.Bgfx.bgfx.Encoder @this, Inno.Native.Bgfx.bgfx.UniformHandle handle, void* value, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6427) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ef4b4d0a9425c856"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_vertex_buffer_with_layout(Inno.Native.Bgfx.bgfx.Encoder @this, byte stream, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, uint startVertex, uint numVertices, Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6469) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2f0f23f7d9d2b294"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_vertex_buffer(Inno.Native.Bgfx.bgfx.Encoder @this, byte stream, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, uint startVertex, uint numVertices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6463) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e6cd226cadc92d6f"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_set_vertex_count(Inno.Native.Bgfx.bgfx.Encoder @this, uint numVertices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6499) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6ef0d1fea0f5c455"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_submit_indirect_count(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.IndirectBufferHandle indirectHandle, uint start, Inno.Native.Bgfx.bgfx.IndexBufferHandle numHandle, uint numIndex, uint numMax, uint depth, byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6559) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4b63559dfb9a0699"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_submit_indirect(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.IndirectBufferHandle indirectHandle, uint start, uint num, uint depth, byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6553) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1c61a002fdf29823"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_submit_occlusion_query(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.OcclusionQueryHandle occlusionQuery, uint depth, byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6547) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ff5f55912860b4b"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_submit(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, uint depth, byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6541) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a28377be1323788d"></a>[`public static void Inno.Native.Bgfx.bgfx.encoder_touch(Inno.Native.Bgfx.bgfx.Encoder @this, ushort id)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6535) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ac10fbd4f920963"></a>[`public static uint Inno.Native.Bgfx.bgfx.frame(byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5463) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-223bec0ded89112c"></a>[`public static uint Inno.Native.Bgfx.bgfx.get_avail_instance_data_buffer(uint num, ushort stride)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5704) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ffa7fa3247b466f"></a>[`public static uint Inno.Native.Bgfx.bgfx.get_avail_transient_index_buffer(uint num, bool index32)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5691) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bed4a004003ebe0e"></a>[`public static uint Inno.Native.Bgfx.bgfx.get_avail_transient_index_buffer(uint num, byte index32)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5684) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-13a7ceca503281b5"></a>[`public static uint Inno.Native.Bgfx.bgfx.get_avail_transient_vertex_buffer(uint num, Inno.Native.Bgfx.bgfx.VertexLayout* layout)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5697) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6c4bc26a5f3c1fc7"></a>[`public static Inno.Native.Bgfx.bgfx.Caps* Inno.Native.Bgfx.bgfx.get_caps()`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5477) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-64800a88c23328f1"></a>[`public static void* Inno.Native.Bgfx.bgfx.get_direct_access_ptr(Inno.Native.Bgfx.bgfx.TextureHandle handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6023) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4cb8cbf0649bf7de"></a>[`public static Inno.Native.Bgfx.bgfx.InterfaceVtbl* Inno.Native.Bgfx.bgfx.get_interface(uint version)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6955) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-afd273a36592e54c"></a>[`public static Inno.Native.Bgfx.bgfx.InternalData* Inno.Native.Bgfx.bgfx.get_internal_data()`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6660) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-30a599b1f2d9a37d"></a>[`public static byte* Inno.Native.Bgfx.bgfx.get_renderer_name(Inno.Native.Bgfx.bgfx.RendererType type)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5422) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b276d01ffb294f97"></a>[`public static Inno.Native.Bgfx.bgfx.RendererType Inno.Native.Bgfx.bgfx.get_renderer_type()`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5470) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-92f375261670e3f6"></a>[`public static Inno.Native.Bgfx.bgfx.OcclusionQueryResult Inno.Native.Bgfx.bgfx.get_result(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle, ref int result)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6199) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e84a1f51854d412a"></a>[`public static Inno.Native.Bgfx.bgfx.OcclusionQueryResult Inno.Native.Bgfx.bgfx.get_result(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle, int* result)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6192) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8bb31878a9dcc228"></a>[`public static ushort Inno.Native.Bgfx.bgfx.get_shader_uniforms(Inno.Native.Bgfx.bgfx.ShaderHandle handle, ref Inno.Native.Bgfx.bgfx.UniformHandle uniforms, ushort max)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5803) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bcfd39e2dd527a5d"></a>[`public static ushort Inno.Native.Bgfx.bgfx.get_shader_uniforms(Inno.Native.Bgfx.bgfx.ShaderHandle handle, Inno.Native.Bgfx.bgfx.UniformHandle* uniforms, ushort max)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5796) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f9dcf2ffe09fcade"></a>[`public static Inno.Native.Bgfx.bgfx.Stats* Inno.Native.Bgfx.bgfx.get_stats()`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5484) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9a13dda870c8775b"></a>[`public static byte Inno.Native.Bgfx.bgfx.get_supported_renderers(byte max, ref Inno.Native.Bgfx.bgfx.RendererType @enum)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5413) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d6c6a8e4716e16bf"></a>[`public static byte Inno.Native.Bgfx.bgfx.get_supported_renderers(byte max, Inno.Native.Bgfx.bgfx.RendererType* @enum)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5406) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-63580e06cdda7fd5"></a>[`public static Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.get_texture(Inno.Native.Bgfx.bgfx.FrameBufferHandle handle, byte attachment)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6105) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ca1bc73c3f34e38d"></a>[`public static void Inno.Native.Bgfx.bgfx.get_uniform_info(Inno.Native.Bgfx.bgfx.UniformHandle handle, ref Inno.Native.Bgfx.bgfx.UniformInfo info)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6170) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2e2f8160fef18336"></a>[`public static void Inno.Native.Bgfx.bgfx.get_uniform_info(Inno.Native.Bgfx.bgfx.UniformHandle handle, Inno.Native.Bgfx.bgfx.UniformInfo* info)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6164) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8606ba3efde35831"></a>[`public static void Inno.Native.Bgfx.bgfx.init_ctor(ref Inno.Native.Bgfx.bgfx.Init init)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5435) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-982d6938f60dae60"></a>[`public static void Inno.Native.Bgfx.bgfx.init_ctor(Inno.Native.Bgfx.bgfx.Init* init)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5429) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3c73b6ecbd456086"></a>[`public static bool Inno.Native.Bgfx.bgfx.init(Inno.Native.Bgfx.bgfx.Init* init)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5444) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dbbb40570568e998"></a>[`public static bool Inno.Native.Bgfx.bgfx.is_frame_buffer_valid(byte num, Inno.Native.Bgfx.bgfx.Attachment* attachment)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5886) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e714c0c65a4eb1c2"></a>[`public static bool Inno.Native.Bgfx.bgfx.is_texture_valid(ushort depth, bool cubeMap, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5879) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-de4f9af2a0f96a36"></a>[`public static byte Inno.Native.Bgfx.bgfx.is_texture_valid(ushort depth, byte cubeMap, ushort numLayers, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5872) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2daf849959e1dd21"></a>[`public static Inno.Native.Bgfx.bgfx.Memory* Inno.Native.Bgfx.bgfx.make_ref_release(void* data, uint size, void* releaseFn, void* userData)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5512) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3fcd801d438711fd"></a>[`public static Inno.Native.Bgfx.bgfx.Memory* Inno.Native.Bgfx.bgfx.make_ref(void* data, uint size)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5505) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-491db3c91d92d1f6"></a>[`public static ulong Inno.Native.Bgfx.bgfx.override_internal_texture_ptr(Inno.Native.Bgfx.bgfx.TextureHandle handle, ulong ptr, ushort layerIndex)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6667) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-53aac9599cd7d0d6"></a>[`public static ulong Inno.Native.Bgfx.bgfx.override_internal_texture(Inno.Native.Bgfx.bgfx.TextureHandle handle, ushort width, ushort height, byte numMips, Inno.Native.Bgfx.bgfx.TextureFormat format, ulong flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6674) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2e9735dec716185d"></a>[`public static uint Inno.Native.Bgfx.bgfx.read_texture(Inno.Native.Bgfx.bgfx.TextureHandle handle, void* data, byte mip)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5994) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2fdd3bce9915a7af"></a>[`public static Inno.Native.Bgfx.bgfx.RenderFrame Inno.Native.Bgfx.bgfx.render_frame(int msecs)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6647) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e2eb0b0a46862a2"></a>[`public static void Inno.Native.Bgfx.bgfx.request_screen_shot(Inno.Native.Bgfx.bgfx.FrameBufferHandle handle, byte* filePath)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6625) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4e2872f8bd5381cb"></a>[`public static void Inno.Native.Bgfx.bgfx.request_screen_shot(Inno.Native.Bgfx.bgfx.FrameBufferHandle handle, string filePath)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6631) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f350c90c5907aefe"></a>[`public static void Inno.Native.Bgfx.bgfx.reset_view(ushort id)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6314) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8581dc7fd036db18"></a>[`public static void Inno.Native.Bgfx.bgfx.reset(uint width, uint height, uint flags, Inno.Native.Bgfx.bgfx.TextureFormat format)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5457) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-646ed76981f18a6b"></a>[`public static void Inno.Native.Bgfx.bgfx.set_compute_dynamic_index_buffer(byte stage, Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6907) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e74df60e6a48716b"></a>[`public static void Inno.Native.Bgfx.bgfx.set_compute_dynamic_vertex_buffer(byte stage, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6913) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-753a07448c149bcc"></a>[`public static void Inno.Native.Bgfx.bgfx.set_compute_index_buffer(byte stage, Inno.Native.Bgfx.bgfx.IndexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6895) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-daa16e2b12448b71"></a>[`public static void Inno.Native.Bgfx.bgfx.set_compute_indirect_buffer(byte stage, Inno.Native.Bgfx.bgfx.IndirectBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6919) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5e1920c1dbda625d"></a>[`public static void Inno.Native.Bgfx.bgfx.set_compute_vertex_buffer(byte stage, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, Inno.Native.Bgfx.bgfx.Access access)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6901) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-51ff163428f08c26"></a>[`public static void Inno.Native.Bgfx.bgfx.set_condition(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle, bool visible)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6715) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-96bab17bce8b6e08"></a>[`public static void Inno.Native.Bgfx.bgfx.set_condition(Inno.Native.Bgfx.bgfx.OcclusionQueryHandle handle, byte visible)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6709) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3c0f204f1bd3b3b6"></a>[`public static void Inno.Native.Bgfx.bgfx.set_debug(uint debug)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5519) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ca2aefd308b07964"></a>[`public static void Inno.Native.Bgfx.bgfx.set_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle handle, uint firstIndex, uint numIndices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6781) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9622812d37952e94"></a>[`public static void Inno.Native.Bgfx.bgfx.set_dynamic_vertex_buffer_with_layout(byte stream, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, uint numVertices, Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6811) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3b01cdd54730470c"></a>[`public static void Inno.Native.Bgfx.bgfx.set_dynamic_vertex_buffer(byte stream, Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, uint numVertices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6805) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5e84f294a31f7f7f"></a>[`public static void Inno.Native.Bgfx.bgfx.set_frame_buffer_name(Inno.Native.Bgfx.bgfx.FrameBufferHandle handle, byte* name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6083) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4f2ddc9a3486d92c"></a>[`public static void Inno.Native.Bgfx.bgfx.set_frame_buffer_name(Inno.Native.Bgfx.bgfx.FrameBufferHandle handle, string name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6089) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-95bfd79789c5b51a"></a>[`public static void Inno.Native.Bgfx.bgfx.set_frame_uniform(Inno.Native.Bgfx.bgfx.UniformHandle handle, void* value, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6439) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-89949b891bba47fc"></a>[`public static void Inno.Native.Bgfx.bgfx.set_image(byte stage, Inno.Native.Bgfx.bgfx.TextureHandle handle, byte mip, Inno.Native.Bgfx.bgfx.Access access, Inno.Native.Bgfx.bgfx.TextureFormat format)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6925) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f1accec77f97d79b"></a>[`public static void Inno.Native.Bgfx.bgfx.set_index_buffer_name(Inno.Native.Bgfx.bgfx.IndexBufferHandle handle, byte* name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5556) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-597f2f70d8dd4265"></a>[`public static void Inno.Native.Bgfx.bgfx.set_index_buffer_name(Inno.Native.Bgfx.bgfx.IndexBufferHandle handle, string name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5562) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c55fcc8ca0c06a5a"></a>[`public static void Inno.Native.Bgfx.bgfx.set_index_buffer(Inno.Native.Bgfx.bgfx.IndexBufferHandle handle, uint firstIndex, uint numIndices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6775) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fd1f21c1a61913dd"></a>[`public static void Inno.Native.Bgfx.bgfx.set_instance_count(uint numInstances)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6853) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e09a7be538fa71ad"></a>[`public static void Inno.Native.Bgfx.bgfx.set_instance_data_buffer(Inno.Native.Bgfx.bgfx.InstanceDataBuffer* idb, uint start, uint num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6835) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7a4ae6d3e06730f9"></a>[`public static void Inno.Native.Bgfx.bgfx.set_instance_data_from_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, uint num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6847) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1435bc3f813ad994"></a>[`public static void Inno.Native.Bgfx.bgfx.set_instance_data_from_vertex_buffer(Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, uint startVertex, uint num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6841) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-59a96dbdccc61080"></a>[`public static void Inno.Native.Bgfx.bgfx.set_marker(byte* name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6681) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c4d3ef39b5f09f47"></a>[`public static void Inno.Native.Bgfx.bgfx.set_marker(string name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6687) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ddb575840923ccb7"></a>[`public static void Inno.Native.Bgfx.bgfx.set_palette_color_rgba32f(byte index, float r, float g, float b, float a)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6220) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b6eaa98915748952"></a>[`public static void Inno.Native.Bgfx.bgfx.set_palette_color_rgba8(byte index, uint rgba)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6226) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-abc577c13884a5dd"></a>[`public static void Inno.Native.Bgfx.bgfx.set_palette_color(byte index, float* rgba)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6214) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c2b964e3fd9cc7ee"></a>[`public static void Inno.Native.Bgfx.bgfx.set_platform_data(Inno.Native.Bgfx.bgfx.PlatformData* data)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6654) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6b23a412fbf0d0a8"></a>[`public static void Inno.Native.Bgfx.bgfx.set_scissor_cached(ushort cache)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6734) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9ee75e85b27be4b3"></a>[`public static ushort Inno.Native.Bgfx.bgfx.set_scissor(ushort x, ushort y, ushort width, ushort height)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6727) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-81d50f55ea1cfc83"></a>[`public static void Inno.Native.Bgfx.bgfx.set_shader_name(Inno.Native.Bgfx.bgfx.ShaderHandle handle, byte* name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5812) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d1e3ab90c44528b9"></a>[`public static void Inno.Native.Bgfx.bgfx.set_shader_name(Inno.Native.Bgfx.bgfx.ShaderHandle handle, string name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5818) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-87b10d1dcaa305fb"></a>[`public static void Inno.Native.Bgfx.bgfx.set_state(ulong state, uint rgba)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6703) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9b03d1fa939d42a4"></a>[`public static void Inno.Native.Bgfx.bgfx.set_stencil(uint fstencil, uint bstencil)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6721) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ece3f9f8a5531ba8"></a>[`public static void Inno.Native.Bgfx.bgfx.set_texture_name(Inno.Native.Bgfx.bgfx.TextureHandle handle, byte* name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6001) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e4e3a0394f9215a6"></a>[`public static void Inno.Native.Bgfx.bgfx.set_texture_name(Inno.Native.Bgfx.bgfx.TextureHandle handle, string name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6007) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6f6307965c7263fd"></a>[`public static void Inno.Native.Bgfx.bgfx.set_texture(byte stage, Inno.Native.Bgfx.bgfx.UniformHandle sampler, Inno.Native.Bgfx.bgfx.TextureHandle handle, uint flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6859) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-db074d2f292cdf10"></a>[`public static void Inno.Native.Bgfx.bgfx.set_transform_cached(uint cache, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6747) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ec7c9af703b01e07"></a>[`public static uint Inno.Native.Bgfx.bgfx.set_transform(void* mtx, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6740) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2d0530019946876d"></a>[`public static void Inno.Native.Bgfx.bgfx.set_transient_index_buffer(Inno.Native.Bgfx.bgfx.TransientIndexBuffer* tib, uint firstIndex, uint numIndices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6787) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e22459a4209f6ec7"></a>[`public static void Inno.Native.Bgfx.bgfx.set_transient_vertex_buffer_with_layout(byte stream, Inno.Native.Bgfx.bgfx.TransientVertexBuffer* tvb, uint startVertex, uint numVertices, Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6823) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2e99ead3e822e593"></a>[`public static void Inno.Native.Bgfx.bgfx.set_transient_vertex_buffer(byte stream, Inno.Native.Bgfx.bgfx.TransientVertexBuffer* tvb, uint startVertex, uint numVertices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6817) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b63390901df5d69e"></a>[`public static void Inno.Native.Bgfx.bgfx.set_uniform(Inno.Native.Bgfx.bgfx.UniformHandle handle, void* value, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6769) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f105ed9190aea811"></a>[`public static void Inno.Native.Bgfx.bgfx.set_vertex_buffer_name(Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, byte* name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5604) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5dda862139013950"></a>[`public static void Inno.Native.Bgfx.bgfx.set_vertex_buffer_name(Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, string name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5610) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-054b90c51db42b41"></a>[`public static void Inno.Native.Bgfx.bgfx.set_vertex_buffer_with_layout(byte stream, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, uint startVertex, uint numVertices, Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6799) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-df9ff2bc9ec227d3"></a>[`public static void Inno.Native.Bgfx.bgfx.set_vertex_buffer(byte stream, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle, uint startVertex, uint numVertices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6793) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-10727debc3d2e21d"></a>[`public static void Inno.Native.Bgfx.bgfx.set_vertex_count(uint numVertices)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6829) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7c27c6f3945d8f03"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_clear_mrt(ushort id, ushort flags, float depth, byte stencil, byte c0, byte c1, byte c2, byte c3, byte c4, byte c5, byte c6, byte c7)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6278) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6393b51ff5851146"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_clear(ushort id, ushort flags, uint rgba, float depth, byte stencil)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6272) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3f1d3ac33def335d"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_frame_buffer(ushort id, Inno.Native.Bgfx.bgfx.FrameBufferHandle handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6290) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b288e0f55166dd94"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_mode(ushort id, Inno.Native.Bgfx.bgfx.ViewMode mode)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6284) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-198c9ed3e49c5806"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_name(ushort id, byte* name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6232) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6d3cc80f7013d4a6"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_name(ushort id, string name, int len)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6238) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-82c3fe64cbcf6dca"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_order(ushort id, ushort num, ushort* order)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6302) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7868ede7d7ddd518"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_rect_ratio(ushort id, ushort x, ushort y, Inno.Native.Bgfx.bgfx.BackbufferRatio ratio)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6260) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e12fb6f91ec2477a"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_rect(ushort id, ushort x, ushort y, ushort width, ushort height)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6254) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7f1fd6c953ddcfb5"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_scissor(ushort id, ushort x, ushort y, ushort width, ushort height)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6266) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f95a8a5f01658298"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_shading_rate(ushort id, Inno.Native.Bgfx.bgfx.ShadingRate shadingRate)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6308) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8aa4bf32657d9084"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_transform(ushort id, void* view, void* proj)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6296) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8e1da1ccf7636137"></a>[`public static void Inno.Native.Bgfx.bgfx.set_view_uniform(ushort id, Inno.Native.Bgfx.bgfx.UniformHandle handle, void* value, ushort num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6433) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-57c7ec2cf8cb7dd5"></a>[`public static void Inno.Native.Bgfx.bgfx.shutdown()`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5451) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ae82a91e40e0daa7"></a>[`public static void Inno.Native.Bgfx.bgfx.submit_indirect_count(ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.IndirectBufferHandle indirectHandle, uint start, Inno.Native.Bgfx.bgfx.IndexBufferHandle numHandle, uint numIndex, uint numMax, uint depth, byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6889) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9bd3e7a1e766f8ed"></a>[`public static void Inno.Native.Bgfx.bgfx.submit_indirect(ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.IndirectBufferHandle indirectHandle, uint start, uint num, uint depth, byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6883) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3f41f1626f8380cd"></a>[`public static void Inno.Native.Bgfx.bgfx.submit_occlusion_query(ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, Inno.Native.Bgfx.bgfx.OcclusionQueryHandle occlusionQuery, uint depth, byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6877) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d8fb2f93d0daec9b"></a>[`public static void Inno.Native.Bgfx.bgfx.submit(ushort id, Inno.Native.Bgfx.bgfx.ProgramHandle program, uint depth, byte flags)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6871) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5dc2d411ff512a29"></a>[`public static uint Inno.Native.Bgfx.bgfx.topology_convert(Inno.Native.Bgfx.bgfx.TopologyConvert conversion, void* dst, uint dstSize, void* indices, uint numIndices, bool index32)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5388) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-29dc825998a36386"></a>[`public static uint Inno.Native.Bgfx.bgfx.topology_convert(Inno.Native.Bgfx.bgfx.TopologyConvert conversion, void* dst, uint dstSize, void* indices, uint numIndices, byte index32)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5381) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a43450fa60b810a9"></a>[`public static void Inno.Native.Bgfx.bgfx.topology_sort_tri_list(Inno.Native.Bgfx.bgfx.TopologySort sort, void* dst, uint dstSize, float* dir, float* pos, void* vertices, uint stride, void* indices, uint numIndices, bool index32)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5400) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e13b17307c4810ed"></a>[`public static void Inno.Native.Bgfx.bgfx.topology_sort_tri_list(Inno.Native.Bgfx.bgfx.TopologySort sort, void* dst, uint dstSize, float* dir, float* pos, void* vertices, uint stride, void* indices, uint numIndices, byte index32)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5394) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aa8e4fcb10ea14f4"></a>[`public static void Inno.Native.Bgfx.bgfx.touch(ushort id)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L6865) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-77b8f7ad043000d6"></a>[`public static void Inno.Native.Bgfx.bgfx.update_dynamic_index_buffer(Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle handle, uint startIndex, Inno.Native.Bgfx.bgfx.Memory* mem)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5646) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-559b9ccfb5760e98"></a>[`public static void Inno.Native.Bgfx.bgfx.update_dynamic_vertex_buffer(Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle handle, uint startVertex, Inno.Native.Bgfx.bgfx.Memory* mem)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5672) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b30b41af41ae4f26"></a>[`public static void Inno.Native.Bgfx.bgfx.update_texture_2d(Inno.Native.Bgfx.bgfx.TextureHandle handle, ushort layer, byte mip, ushort x, ushort y, ushort width, ushort height, Inno.Native.Bgfx.bgfx.Memory* mem, ushort pitch)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5976) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a25d1d2a08994d9d"></a>[`public static void Inno.Native.Bgfx.bgfx.update_texture_3d(Inno.Native.Bgfx.bgfx.TextureHandle handle, byte mip, ushort x, ushort y, ushort z, ushort width, ushort height, ushort depth, Inno.Native.Bgfx.bgfx.Memory* mem)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5982) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a8dffebd73f20cb7"></a>[`public static void Inno.Native.Bgfx.bgfx.update_texture_cube(Inno.Native.Bgfx.bgfx.TextureHandle handle, ushort layer, byte side, byte mip, ushort x, ushort y, ushort width, ushort height, Inno.Native.Bgfx.bgfx.Memory* mem, ushort pitch)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5988) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aa578c50cb6e3d74"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_convert(Inno.Native.Bgfx.bgfx.VertexLayout* dstLayout, void* dstData, Inno.Native.Bgfx.bgfx.VertexLayout* srcLayout, void* srcData, uint num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5368) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3291bb14a9395f47"></a>[`public static Inno.Native.Bgfx.bgfx.VertexLayout* Inno.Native.Bgfx.bgfx.vertex_layout_add(ref Inno.Native.Bgfx.bgfx.VertexLayout @this, Inno.Native.Bgfx.bgfx.Attrib attrib, byte num, Inno.Native.Bgfx.bgfx.AttribType type, bool normalized, bool asInt)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5273) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0dfc5bdb6f86709f"></a>[`public static Inno.Native.Bgfx.bgfx.VertexLayout* Inno.Native.Bgfx.bgfx.vertex_layout_add(Inno.Native.Bgfx.bgfx.VertexLayout* @this, Inno.Native.Bgfx.bgfx.Attrib attrib, byte num, Inno.Native.Bgfx.bgfx.AttribType type, byte normalized, byte asInt)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5266) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3be12eb7901957c1"></a>[`public static Inno.Native.Bgfx.bgfx.VertexLayout* Inno.Native.Bgfx.bgfx.vertex_layout_begin(Inno.Native.Bgfx.bgfx.VertexLayout* @this, Inno.Native.Bgfx.bgfx.RendererType rendererType)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5259) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-296db424e5446a72"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_layout_decode(Inno.Native.Bgfx.bgfx.VertexLayout* @this, Inno.Native.Bgfx.bgfx.Attrib attrib, ref byte num, ref Inno.Native.Bgfx.bgfx.AttribType type, ref bool normalized, ref bool asInt)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5288) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-23d30dd54644177a"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_layout_decode(Inno.Native.Bgfx.bgfx.VertexLayout* @this, Inno.Native.Bgfx.bgfx.Attrib attrib, byte* num, Inno.Native.Bgfx.bgfx.AttribType* type, byte* normalized, byte* asInt)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5282) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4e4033b34404745a"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_layout_end(ref Inno.Native.Bgfx.bgfx.VertexLayout @this)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5326) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c552a69ddfce242e"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_layout_end(Inno.Native.Bgfx.bgfx.VertexLayout* @this)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5320) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bd3607802f1d07ec"></a>[`public static ushort Inno.Native.Bgfx.bgfx.vertex_layout_get_offset(Inno.Native.Bgfx.bgfx.VertexLayout* @this, Inno.Native.Bgfx.bgfx.Attrib attrib)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5335) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9578cc1678ad4d69"></a>[`public static uint Inno.Native.Bgfx.bgfx.vertex_layout_get_size(Inno.Native.Bgfx.bgfx.VertexLayout* @this, uint num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5349) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ddd18700d08b624a"></a>[`public static ushort Inno.Native.Bgfx.bgfx.vertex_layout_get_stride(Inno.Native.Bgfx.bgfx.VertexLayout* @this)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5342) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ba0857acdaa95727"></a>[`public static bool Inno.Native.Bgfx.bgfx.vertex_layout_has(Inno.Native.Bgfx.bgfx.VertexLayout* @this, Inno.Native.Bgfx.bgfx.Attrib attrib)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5306) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a1977b9ee0a34a63"></a>[`public static Inno.Native.Bgfx.bgfx.VertexLayout* Inno.Native.Bgfx.bgfx.vertex_layout_skip(Inno.Native.Bgfx.bgfx.VertexLayout* @this, byte num)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5313) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4d8aa907653b20db"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_pack(float* input, byte inputNormalized, Inno.Native.Bgfx.bgfx.Attrib attr, Inno.Native.Bgfx.bgfx.VertexLayout* layout, void* data, uint index)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5356) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aaafb8602c478b9e"></a>[`public static void Inno.Native.Bgfx.bgfx.vertex_unpack(float* output, Inno.Native.Bgfx.bgfx.Attrib attr, Inno.Native.Bgfx.bgfx.VertexLayout* layout, void* data, uint index)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5362) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ea090d86c0e9b5f0"></a>[`public static uint Inno.Native.Bgfx.bgfx.weld_vertices(void* output, Inno.Native.Bgfx.bgfx.VertexLayout* layout, void* data, uint num, byte index32, float epsilon)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L5374) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |

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
| <a id="api-b62c66249e25b0dd"></a>[`Inno.Native.Bgfx.bgfx.Access.Count = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L67) | 当前常量或枚举值：`3`。 | — |
| <a id="api-978c23ff4e03f669"></a>[`Inno.Native.Bgfx.bgfx.Access.Read = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L64) | 当前常量或枚举值：`0`。 | — |
| <a id="api-93970281ade43b49"></a>[`Inno.Native.Bgfx.bgfx.Access.ReadWrite = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L66) | 当前常量或枚举值：`2`。 | — |
| <a id="api-0e654b97e9f88a0d"></a>[`Inno.Native.Bgfx.bgfx.Access.Write = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L65) | 当前常量或枚举值：`1`。 | — |
| <a id="api-3ea31ac55ae50695"></a>[`public enum Inno.Native.Bgfx.bgfx.Access : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L62) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.AllocatorInterface</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.AllocatorInterface 2f6990b229fac9c223cc41e52e7b90b0fe5e0d4cb4a03995f92d1583f8dd0ab1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AllocatorInterface.vtbl ca2734dbbc1049d2b0a1a8d267b82ac1368740abc74c8a03144ac5b4b5806f36 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.AllocatorInterface.#ctor(Inno.Native.Bgfx.bgfx.AllocatorVtbl*) c1f6573176fd7fffefedc5dc475196fb42b6d2b6f5726eec73c890ffcc839918 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9af84d37db2c4edd"></a>[`public Inno.Native.Bgfx.bgfx.AllocatorVtbl* Inno.Native.Bgfx.bgfx.AllocatorInterface.vtbl`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L643) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f21e94d43a468a4"></a>[`public Inno.Native.Bgfx.bgfx.AllocatorInterface.AllocatorInterface(Inno.Native.Bgfx.bgfx.AllocatorVtbl* vtbl = null)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L644) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-c860e42f4f8fd2cf"></a>[`public struct Inno.Native.Bgfx.bgfx.AllocatorInterface`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L641) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.AllocatorVtbl</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.AllocatorVtbl bb1c39160c1ae3af1b44e3156ebab8888021966056b07dfb60a7bd2db7e11b3c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.AllocatorVtbl.realloc e3417ea53e7daf30ad9ad53738b8ba0d570a5eac9895d31452fede2831fab9ce -->
<!-- api: M:Inno.Native.Bgfx.bgfx.AllocatorVtbl.#ctor(System.Void*) 0c30dcf9ff5b2df3a94593d7d3e9e07f5fb155b46bd6996c525fba2dfd5a523a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b9c20ec37d6c5041"></a>[`public void* Inno.Native.Bgfx.bgfx.AllocatorVtbl.realloc`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L627) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7aeb407d0d95c0ca"></a>[`public Inno.Native.Bgfx.bgfx.AllocatorVtbl.AllocatorVtbl(void* realloc = null)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L628) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-91c24f218ac3d147"></a>[`public struct Inno.Native.Bgfx.bgfx.AllocatorVtbl`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L625) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-ca56734cc6dcac55"></a>[`public Inno.Native.Bgfx.bgfx.Access Inno.Native.Bgfx.bgfx.Attachment.access`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L677) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1148b21562ec97aa"></a>[`public Inno.Native.Bgfx.bgfx.TextureHandle Inno.Native.Bgfx.bgfx.Attachment.handle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L678) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad14c64cd6fac96c"></a>[`public ushort Inno.Native.Bgfx.bgfx.Attachment.layer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L680) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a11dbfc3fcbcb23"></a>[`public ushort Inno.Native.Bgfx.bgfx.Attachment.mip`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L679) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d873aeec6fa9a569"></a>[`public ushort Inno.Native.Bgfx.bgfx.Attachment.numLayers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L681) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f175347be0a50ec2"></a>[`public byte Inno.Native.Bgfx.bgfx.Attachment.resolve`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L682) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3dcd3b7d31a0695d"></a>[`public Inno.Native.Bgfx.bgfx.Attachment.Attachment(Inno.Native.Bgfx.bgfx.Access access = Inno.Native.Bgfx.bgfx.Access.Read, Inno.Native.Bgfx.bgfx.TextureHandle handle = default(Inno.Native.Bgfx.bgfx.TextureHandle), ushort mip = 0, ushort layer = 0, ushort numLayers = 0, byte resolve = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L683) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-48cb5b66777519a0"></a>[`public struct Inno.Native.Bgfx.bgfx.Attachment`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L675) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-7f3b58444579011b"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Bitangent = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L81) | 当前常量或枚举值：`3`。 | — |
| <a id="api-83cc69b64191f061"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Color0 = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L82) | 当前常量或枚举值：`4`。 | — |
| <a id="api-ef1b9b473978fc66"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Color1 = 5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L83) | 当前常量或枚举值：`5`。 | — |
| <a id="api-71914f44c802e31d"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Color2 = 6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L84) | 当前常量或枚举值：`6`。 | — |
| <a id="api-c19f9c0d38668589"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Color3 = 7`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L85) | 当前常量或枚举值：`7`。 | — |
| <a id="api-f78ad44d0d995c5f"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Count = 18`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L96) | 当前常量或枚举值：`18`。 | — |
| <a id="api-dedda80f71a7c53c"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Indices = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L86) | 当前常量或枚举值：`8`。 | — |
| <a id="api-3050f8cea02ab541"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Normal = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L79) | 当前常量或枚举值：`1`。 | — |
| <a id="api-25edc5225dd95950"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Position = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L78) | 当前常量或枚举值：`0`。 | — |
| <a id="api-1b95ff9cd2601e91"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Tangent = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L80) | 当前常量或枚举值：`2`。 | — |
| <a id="api-9be37662c648364b"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord0 = 10`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L88) | 当前常量或枚举值：`10`。 | — |
| <a id="api-b5248d0062dd4fda"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord1 = 11`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L89) | 当前常量或枚举值：`11`。 | — |
| <a id="api-f2e0dd75b746e2bf"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord2 = 12`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L90) | 当前常量或枚举值：`12`。 | — |
| <a id="api-80c6003126e86973"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord3 = 13`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L91) | 当前常量或枚举值：`13`。 | — |
| <a id="api-6404af589ea696cd"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord4 = 14`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L92) | 当前常量或枚举值：`14`。 | — |
| <a id="api-234ad6f63cbe637d"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord5 = 15`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L93) | 当前常量或枚举值：`15`。 | — |
| <a id="api-231e702c881a9a3b"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord6 = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L94) | 当前常量或枚举值：`16`。 | — |
| <a id="api-b45dd98d310d6fdc"></a>[`Inno.Native.Bgfx.bgfx.Attrib.TexCoord7 = 17`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L95) | 当前常量或枚举值：`17`。 | — |
| <a id="api-152e091f7bde2453"></a>[`Inno.Native.Bgfx.bgfx.Attrib.Weight = 9`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L87) | 当前常量或枚举值：`9`。 | — |
| <a id="api-f6b86a9ed3b8ddfa"></a>[`public enum Inno.Native.Bgfx.bgfx.Attrib : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L76) | Native 取值集合；当前取值见本类型成员。 | — |

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
| <a id="api-f3c271624c59b3c0"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Count = 7`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L114) | 当前常量或枚举值：`7`。 | — |
| <a id="api-3955ddb4efb4fd83"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Float = 6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L113) | 当前常量或枚举值：`6`。 | — |
| <a id="api-3999bfa16c8207e6"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Half = 5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L112) | 当前常量或枚举值：`5`。 | — |
| <a id="api-180b8c2651880ab5"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Int16 = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L110) | 当前常量或枚举值：`3`。 | — |
| <a id="api-585bd9a6f2b531a2"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Int8 = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L107) | 当前常量或枚举值：`0`。 | — |
| <a id="api-5c80a9880f92726f"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Uint10 = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L109) | 当前常量或枚举值：`2`。 | — |
| <a id="api-7efed06fad7f93f9"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Uint16 = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L111) | 当前常量或枚举值：`4`。 | — |
| <a id="api-322e43a871605c2f"></a>[`Inno.Native.Bgfx.bgfx.AttribType.Uint8 = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L108) | 当前常量或枚举值：`1`。 | — |
| <a id="api-efe291254e8ede9c"></a>[`public enum Inno.Native.Bgfx.bgfx.AttribType : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L105) | Native 取值集合；当前取值见本类型成员。 | — |

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
| <a id="api-5c8f4fcfa05a9c76"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Count = 6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L272) | 当前常量或枚举值：`6`。 | — |
| <a id="api-a948e60552e1491e"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Double = 5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L271) | 当前常量或枚举值：`5`。 | — |
| <a id="api-70efe07325dfaba4"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Eighth = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L269) | 当前常量或枚举值：`3`。 | — |
| <a id="api-0ecef3ce8de1fb1b"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Equal = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L266) | 当前常量或枚举值：`0`。 | — |
| <a id="api-cf74ebfa416732e9"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Half = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L267) | 当前常量或枚举值：`1`。 | — |
| <a id="api-e181eef723c57c64"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Quarter = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L268) | 当前常量或枚举值：`2`。 | — |
| <a id="api-fad8b69008226e79"></a>[`Inno.Native.Bgfx.bgfx.BackbufferRatio.Sixteenth = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L270) | 当前常量或枚举值：`4`。 | — |
| <a id="api-d554ee7a6af87716"></a>[`public enum Inno.Native.Bgfx.bgfx.BackbufferRatio : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L264) | Native 取值集合；当前取值见本类型成员。 | — |

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
| <a id="api-d16ff06e47e6bfb0"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.AllowResize = 2048`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3340) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-35524ceb14ef725c"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1 = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3323) | 当前常量或枚举值：`4`。 | — |
| <a id="api-3f100aa4a0a2cf2a"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x2 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3324) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1`。 | — |
| <a id="api-1f02876e33b59889"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3325) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x1`。 | — |
| <a id="api-0dee05c3761245cc"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x1 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3326) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4`。 | — |
| <a id="api-6f89f2d378e029c1"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x2 = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3327) | 当前常量或枚举值：`8`。 | — |
| <a id="api-a558dcea60f402b5"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x4 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3328) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x2`。 | — |
| <a id="api-e531f64700675d48"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3320) | 当前常量或枚举值：`1`。 | — |
| <a id="api-9a84a0c7acddfd5d"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2 = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3321) | 当前常量或枚举值：`2`。 | — |
| <a id="api-fab09fc8a824a9c9"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x4 = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3322) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x1 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat8x2`。 | — |
| <a id="api-ad55712fc49fd9a6"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormatMask = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3330) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat16x4 &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormat32x4`。 | — |
| <a id="api-72d65506ec1bf622"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeFormatShift = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3329) | 当前常量或枚举值：`0`。 | — |
| <a id="api-8644fe2002d3bb60"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeRead = 256`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3337) | 当前常量或枚举值：`256`。 | — |
| <a id="api-13b20fa6c0a26d52"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeReadWrite = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeRead &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeWrite`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3342) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeRead &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeWrite`。 | — |
| <a id="api-88ca145caba0ef2d"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeFloat = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3333) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint`。 | — |
| <a id="api-cc887928149ca9a9"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3331) | 当前常量或枚举值：`16`。 | — |
| <a id="api-f414028f82735d2f"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeMask = Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3335) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeInt &#124; Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint`。 | — |
| <a id="api-ad82f9f91b4f00c4"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeShift = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3334) | 当前常量或枚举值：`4`。 | — |
| <a id="api-3cda6dc3c68cb37b"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeTypeUint = 32`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3332) | 当前常量或枚举值：`32`。 | — |
| <a id="api-95324ba48e71a2a6"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.ComputeWrite = 512`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3338) | 当前常量或枚举值：`512`。 | — |
| <a id="api-ef5ee455c7d332f9"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.DrawIndirect = 1024`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3339) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-4e4675979e6df7cf"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.Index32 = 4096`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3341) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-d743ad3cdd347b86"></a>[`Inno.Native.Bgfx.bgfx.BufferFlags.None = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3336) | 当前常量或枚举值：`0`。 | — |
| <a id="api-ad7d0c1ee74cff78"></a>[`public enum Inno.Native.Bgfx.bgfx.BufferFlags : ushort`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3318) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.CallbackInterface</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.CallbackInterface 3e5ee07ecd943a967b1052357fbbba992e0254201f248479e6c1c0b498753e95 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CallbackInterface.vtbl fa98a95c9ccedd70a7753910f9f1b33aeca9e3bafd64a7156fbc9ba3ede00fcf -->
<!-- api: M:Inno.Native.Bgfx.bgfx.CallbackInterface.#ctor(Inno.Native.Bgfx.bgfx.CallbackVtbl*) 1be9a79dc12f23e745351afe3b9da298e585e9d2166770acd442e51e57c05d11 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5ff34dd77421c952"></a>[`public Inno.Native.Bgfx.bgfx.CallbackVtbl* Inno.Native.Bgfx.bgfx.CallbackInterface.vtbl`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L924) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f99b560205f0e99"></a>[`public Inno.Native.Bgfx.bgfx.CallbackInterface.CallbackInterface(Inno.Native.Bgfx.bgfx.CallbackVtbl* vtbl = null)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L925) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d99b0c4ef9192227"></a>[`public struct Inno.Native.Bgfx.bgfx.CallbackInterface`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L922) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-d73e6fc6d2063631"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_read`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L892) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0fe3bca2f9e213d5"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_read_size`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L891) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f330c2e702cfe82"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.cache_write`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L893) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-758c86e5a291947e"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_begin`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L895) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ec5437498d66d56"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_end`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L896) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-94d832cfdc5c7b31"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.capture_frame`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L897) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-052aa9071b6842ec"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.fatal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L886) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc4608b49c2581d2"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_begin`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L888) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2217aab42a703df4"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_begin_literal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L889) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c9c3c692326d4c98"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.profiler_end`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L890) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-63e6d7ec64a626b7"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.screen_shot`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L894) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9780e0e42697f23f"></a>[`public void* Inno.Native.Bgfx.bgfx.CallbackVtbl.trace_vargs`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L887) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-752446baa2cc4539"></a>[`public Inno.Native.Bgfx.bgfx.CallbackVtbl.CallbackVtbl(void* fatal = null, void* trace_vargs = null, void* profiler_begin = null, void* profiler_begin_literal = null, void* profiler_end = null, void* cache_read_size = null, void* cache_read = null, void* cache_write = null, void* screen_shot = null, void* capture_begin = null, void* capture_end = null, void* capture_frame = null)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L898) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1bbdee6bf23a07b5"></a>[`public struct Inno.Native.Bgfx.bgfx.CallbackVtbl`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L884) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-c296b29cbd5cc2ac"></a>[`public ushort Inno.Native.Bgfx.bgfx.Caps.deviceId`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1063) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2eeab95923de628a"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1074) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1efe3550af516b95"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1075) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-74b0acfb8f0690d5"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_10`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1084) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6ef2008b8a96ee3b"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_11`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1085) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-99722a32e02f0542"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_12`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1086) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39f6dcbb9a9d649b"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_13`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1087) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf067affe7341152"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_14`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1088) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5dafd73f90ed15ea"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_15`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1089) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-77980cd72ac45bd0"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1090) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e3598064e3a97eb5"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_17`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1091) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3751b7aac59d094e"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_18`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1092) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-abb9f8596d8ee67a"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_19`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1093) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-49af88ce9c70041f"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1076) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ddb5bc958e2d2e0"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_20`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1094) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6308e658ce7b1023"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_21`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1095) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-25937e866997b711"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_22`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1096) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1e42acd657e4db7"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_23`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1097) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f75a130102855728"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_24`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1098) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1539e7d1d8570ef5"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_25`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1099) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a58e46d567074a9e"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_26`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1100) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-356a82f20a0bc166"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_27`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1101) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e9037cd677f05f5"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_28`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1102) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a56e4fd87fb786a7"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_29`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1103) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e698b55c944841a6"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1077) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f3174868e1134274"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_30`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1104) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9421b720700a1abe"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_31`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1105) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-79e4ff38365491b2"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_32`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1106) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4c8665f5b70bc893"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_33`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1107) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-160d298ae22b00b3"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_34`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1108) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9fd549a6a4b7f10d"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_35`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1109) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ee45fac30105876"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_36`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1110) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa1a1d4c2f4e5411"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_37`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1111) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-deecb5972ffbeb3f"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_38`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1112) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fd5351ee27642958"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_39`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1113) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-479bee5285805258"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1078) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-90ba574055eabd2a"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_40`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1114) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c543b307541c6e8"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_41`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1115) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-decbcce91ae5b7cc"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_42`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1116) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b12fda68e9084700"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_43`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1117) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7acc08e58178d0b4"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_44`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1118) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-691111d301f32f77"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_45`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1119) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8677b1d4786f413d"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_46`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1120) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4be756d9f97f29f9"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_47`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1121) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0ab7cf0a952b1290"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_48`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1122) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c15f55f4d53f75ff"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_49`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1123) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f5b0eb9042d02e3"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1079) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-62c8d649dee52e46"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_50`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1124) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3c36c198e2e957af"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_51`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1125) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed9176c7dec10c23"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_52`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1126) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e509b4239435f593"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_53`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1127) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-88631bd486c756b9"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_54`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1128) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be5639e5af1a61b0"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_55`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1129) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4ab60cc337ae6f03"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_56`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1130) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-306e631082dab874"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_57`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1131) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-52dea80d554fd363"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_58`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1132) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-52bb436affb51088"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_59`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1133) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ef99a88fc390c75"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1080) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e67764276e0fc963"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_60`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1134) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-78363d2c7469fa0c"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_61`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1135) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-368e368f65954b38"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_62`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1136) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c2a0c64d507f463"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_63`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1137) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f0b2f529d479e40f"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_64`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1138) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-991e8ae26c0df666"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_65`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1139) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a3c4dc7ccbd34d06"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_66`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1140) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a756ab8cafaec08"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_67`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1141) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-46a8cc28b08d5899"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_68`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1142) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6a474da508716913"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_69`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1143) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6699ab98ca884eb"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_7`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1081) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f27061ed2e9b96f"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_70`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1144) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eaf93e654f8f9a3f"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_71`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1145) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6e8bcaf2454a465"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_72`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1146) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d0587bbcd99596a"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_73`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1147) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8888fca2cefad658"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_74`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1148) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0359cd04893b87c2"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_75`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1149) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2eb583e070dae3b"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_76`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1150) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0849e50af22757d0"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_77`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1151) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab034a9ef82a5cc5"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_78`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1152) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4c6eb9176a69e821"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_79`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1153) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2342dffe76fdea79"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1082) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5fb5034940a8390"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_80`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1154) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-42d286cf11caf5d8"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_81`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1155) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b7d35a66bc5baca"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_82`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1156) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e9350927fa4358b"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_83`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1157) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85dcff78dcc55e73"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_84`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1158) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-22654da813917783"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_85`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1159) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3def6419cb4d6f6c"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_86`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1160) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-591e6287866da913"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_87`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1161) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dbdd5c2a0767775c"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_88`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1162) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c18f8d8c5b97b59f"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_89`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1163) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20123a88c6741b69"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_9`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1083) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9bac3688446edd81"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_90`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1164) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad478cf46d79e05e"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_91`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1165) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e69e09b025d651fd"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_92`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1166) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c589da2acc14971d"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_93`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1167) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-87e6005e795a21d6"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_94`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1168) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0da3eba203712b6b"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_95`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1169) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b5be01da7da403d5"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_96`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1170) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a95d2d8a4e19dc06"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_97`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1171) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3440c684eabd5bed"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_98`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1172) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01f4c8029ef22a17"></a>[`public uint Inno.Native.Bgfx.bgfx.Caps.formats_99`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1173) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-668c5b0695756329"></a>[`public Inno.Native.Bgfx.bgfx.CapsGpu Inno.Native.Bgfx.bgfx.Caps.gpu_0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1067) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61d1b951236b1506"></a>[`public Inno.Native.Bgfx.bgfx.CapsGpu Inno.Native.Bgfx.bgfx.Caps.gpu_1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1068) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8920ed901e32fed1"></a>[`public Inno.Native.Bgfx.bgfx.CapsGpu Inno.Native.Bgfx.bgfx.Caps.gpu_2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1069) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d3f55534107beb5f"></a>[`public Inno.Native.Bgfx.bgfx.CapsGpu Inno.Native.Bgfx.bgfx.Caps.gpu_3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1070) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27ddcc19c30a412e"></a>[`public byte Inno.Native.Bgfx.bgfx.Caps.homogeneousDepth`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1064) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01ba6930ccaec984"></a>[`public Inno.Native.Bgfx.bgfx.CapsLimits Inno.Native.Bgfx.bgfx.Caps.limits`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1073) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e82b89f0a6c5f5c"></a>[`public byte Inno.Native.Bgfx.bgfx.Caps.numGPUs`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1066) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07c40e3168520a17"></a>[`public byte Inno.Native.Bgfx.bgfx.Caps.originBottomLeft`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1065) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-38984fcead5c385f"></a>[`public Inno.Native.Bgfx.bgfx.RendererType Inno.Native.Bgfx.bgfx.Caps.rendererType`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1060) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da3f042458bd7b2d"></a>[`public ulong Inno.Native.Bgfx.bgfx.Caps.supported`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1061) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8bd06ce1833cfa0c"></a>[`public ushort Inno.Native.Bgfx.bgfx.Caps.vendorId`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1062) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2527c3aaf1b62559"></a>[`public Inno.Native.Bgfx.bgfx.Caps.Caps(Inno.Native.Bgfx.bgfx.RendererType rendererType = Inno.Native.Bgfx.bgfx.RendererType.Noop, ulong supported = 0, ushort vendorId = 0, ushort deviceId = 0, byte homogeneousDepth = 0, byte originBottomLeft = 0, byte numGPUs = 0, Inno.Native.Bgfx.bgfx.CapsGpu* gpu = null, Inno.Native.Bgfx.bgfx.CapsLimits limits = default(Inno.Native.Bgfx.bgfx.CapsLimits), uint* formats = null)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1176) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8c2dbabaca5dd55a"></a>[`public System.Span<uint> Inno.Native.Bgfx.bgfx.Caps.formats { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1174) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cf1d2a4953a6f851"></a>[`public System.Span<Inno.Native.Bgfx.bgfx.CapsGpu> Inno.Native.Bgfx.bgfx.Caps.gpu { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1071) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6306e998434feb48"></a>[`public struct Inno.Native.Bgfx.bgfx.Caps`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1058) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-e63ea4ef16117617"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.AlphaToCoverage = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3354) | 当前常量或枚举值：`1`。 | — |
| <a id="api-401a8755f10cbe93"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.BlendIndependent = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3355) | 当前常量或枚举值：`2`。 | — |
| <a id="api-2de09d22b20d1f98"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Compute = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3356) | 当前常量或枚举值：`4`。 | — |
| <a id="api-5a0a336438a36f6a"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.ConservativeRaster = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3357) | 当前常量或枚举值：`8`。 | — |
| <a id="api-b4055a1d16076214"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.DrawIndirect = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3358) | 当前常量或枚举值：`16`。 | — |
| <a id="api-e57030e7f118eb21"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.DrawIndirectCount = 32`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3359) | 当前常量或枚举值：`32`。 | — |
| <a id="api-312245001b7e1e69"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.FragmentDepth = 64`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3360) | 当前常量或枚举值：`64`。 | — |
| <a id="api-ef3ede883dd70a50"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.FragmentOrdering = 128`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3361) | 当前常量或枚举值：`128`。 | — |
| <a id="api-538e864d124ff618"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.GraphicsDebugger = 256`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3362) | 当前常量或枚举值：`256`。 | — |
| <a id="api-9c92174cf69172cf"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Hdr10 = 512`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3363) | 当前常量或枚举值：`512`。 | — |
| <a id="api-2b5801bf1936d4ac"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Hidpi = 1024`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3364) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-35516eaa769e8cce"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.ImageRw = 2048`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3365) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-bb16ca8edd5052a1"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Index32 = 4096`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3366) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-e38858d52d69835a"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Instancing = 8192`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3367) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-9367d7bd208d7b1e"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.OcclusionQuery = 16384`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3368) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-5247949a958e3906"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.PrimitiveId = 32768`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3369) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-2f7bfd3aebf66c43"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.RendererMultithreaded = 65536`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3370) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-bf73b1f637ab3738"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.SwapChain = 131072`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3371) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-706d2f8ec1db53d1"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Texture2dArray = 67108864`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3380) | 当前常量或枚举值：`67108864`。 | — |
| <a id="api-8f62f47327b0354a"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.Texture3d = 134217728`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3381) | 当前常量或枚举值：`134217728`。 | — |
| <a id="api-196fa754a77a7caa"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureBlit = 262144`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3372) | 当前常量或枚举值：`262144`。 | — |
| <a id="api-294ebbd947b1b25d"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareAll = Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareLequal &#124; Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareReserved`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3388) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareLequal &#124; Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareReserved`。 | — |
| <a id="api-e1e82e7b3fb41cc4"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareLequal = 524288`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3373) | 当前常量或枚举值：`524288`。 | — |
| <a id="api-e2160773f01ecae6"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCompareReserved = 1048576`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3374) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-a82e77236bb3e5d0"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureCubeArray = 2097152`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3375) | 当前常量或枚举值：`2097152`。 | — |
| <a id="api-bb72b041fd9e2557"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureDirectAccess = 4194304`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3376) | 当前常量或枚举值：`4194304`。 | — |
| <a id="api-73b506e4f66be42b"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureExternal = 8388608`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3377) | 当前常量或枚举值：`8388608`。 | — |
| <a id="api-9b7136d5e848189b"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureExternalShared = 16777216`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3378) | 当前常量或枚举值：`16777216`。 | — |
| <a id="api-9446f753d8219dc7"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TextureReadBack = 33554432`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3379) | 当前常量或枚举值：`33554432`。 | — |
| <a id="api-2f3aea4aec88d6d9"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.TransparentBackbuffer = 268435456`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3382) | 当前常量或枚举值：`268435456`。 | — |
| <a id="api-5de1c36a13800854"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.VariableRateShading = 536870912`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3383) | 当前常量或枚举值：`536870912`。 | — |
| <a id="api-bcf276f753113ded"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.VertexAttribHalf = 1073741824`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3384) | 当前常量或枚举值：`1073741824`。 | — |
| <a id="api-e8e49bb12536d63e"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.VertexAttribUint10 = 2147483648`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3385) | 当前常量或枚举值：`2147483648`。 | — |
| <a id="api-cf2e01443711a55b"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.VertexId = 4294967296`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3386) | 当前常量或枚举值：`4294967296`。 | — |
| <a id="api-1c2c500bda81fff4"></a>[`Inno.Native.Bgfx.bgfx.CapsFlags.ViewportLayerArray = 8589934592`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3387) | 当前常量或枚举值：`8589934592`。 | — |
| <a id="api-baf9c42ad60713b6"></a>[`public enum Inno.Native.Bgfx.bgfx.CapsFlags : ulong`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3352) | Native 取值集合；当前取值见本类型成员。 | — |

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
| <a id="api-3da49be5c5a306b2"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2d = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3401) | 当前常量或枚举值：`1`。 | — |
| <a id="api-f772621033f14a12"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2dEmulated = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3403) | 当前常量或枚举值：`4`。 | — |
| <a id="api-4e6165e90c161f2b"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture2dSrgb = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3402) | 当前常量或枚举值：`2`。 | — |
| <a id="api-1ed19df705f4336e"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3d = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3404) | 当前常量或枚举值：`8`。 | — |
| <a id="api-15194169bbdfc439"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3dEmulated = 32`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3406) | 当前常量或枚举值：`32`。 | — |
| <a id="api-9e3e338aa0f2ae46"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.Texture3dSrgb = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3405) | 当前常量或枚举值：`16`。 | — |
| <a id="api-931a471c01b6dc6a"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureBackbuffer = 65536`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3417) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-75eb501f1d6d17be"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCube = 64`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3407) | 当前常量或枚举值：`64`。 | — |
| <a id="api-0ebbbb76dfced243"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCubeEmulated = 256`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3409) | 当前常量或枚举值：`256`。 | — |
| <a id="api-827599bfc1d2077f"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureCubeSrgb = 128`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3408) | 当前常量或枚举值：`128`。 | — |
| <a id="api-6c2b24998e3199ba"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureFramebuffer = 4096`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3413) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-1bf8e8ce118c408a"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureFramebufferMsaa = 8192`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3414) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-b2f30408ed6c8167"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureImageRead = 1024`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3411) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-e4f05f9771829f0c"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureImageWrite = 2048`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3412) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-68b0c661a4685a34"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureMipAutogen = 32768`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3416) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-05cbfd2ae7bd783b"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureMsaa = 16384`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3415) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-ba57c1f03f49bedd"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureNone = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3400) | 当前常量或枚举值：`0`。 | — |
| <a id="api-0551948e143f5905"></a>[`Inno.Native.Bgfx.bgfx.CapsFormatFlags.TextureVertex = 512`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3410) | 当前常量或枚举值：`512`。 | — |
| <a id="api-55d759994eddb6a1"></a>[`public enum Inno.Native.Bgfx.bgfx.CapsFormatFlags : uint`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3398) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.CapsGpu</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.CapsGpu 9d9aec56d8519ee8a7195c76a30ded3c99730b612d87b49c1c0ff69946a46199 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsGpu.vendorId ea3d2264ea5a649a38b287fc3cbaa2f2fdff873e4915e717b23a9f156b8d0a4e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsGpu.deviceId 210383e7159d5179187639ce8f105bf03f02937a684507925f33cfcd2e25bedd -->
<!-- api: M:Inno.Native.Bgfx.bgfx.CapsGpu.#ctor(System.UInt16,System.UInt16) c6b776360fd09fe9801dce3f29a910bd123a47d356be5445f321dbb3b1f95e67 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5837532d8759bdaa"></a>[`public ushort Inno.Native.Bgfx.bgfx.CapsGpu.deviceId`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L979) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-969d9f770cf15076"></a>[`public ushort Inno.Native.Bgfx.bgfx.CapsGpu.vendorId`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L978) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4eaf49f0cac58f3c"></a>[`public Inno.Native.Bgfx.bgfx.CapsGpu.CapsGpu(ushort vendorId = 0, ushort deviceId = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L980) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a6aa1f9f1582c7df"></a>[`public struct Inno.Native.Bgfx.bgfx.CapsGpu`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L976) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.CapsLimits</code> · 27 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.CapsLimits dff01c951e0dfb765a3fa65f361c0879afa2542f7479d7e273129be25c2d431e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxDrawCalls 5404e8f540caf9a6895ce07101d5899d6d2a795d95890e9bc44a09cbd9609ec6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxBlits 4bb03a9deb5a008f839fef657040c174ba60d6dec4ca61dece06b6b6cebcc8e3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureSize a86e33659d1f78b9da0ebce4570989eff8dcd8b72e882c3ea0b40b55cda73337 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureLayers 6850f9e0d4b8f112ec543296e4140b4cc6fe17274014013c56301cb1f71cd0ac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxViews 7e0a273160b38a192ed598137df585833c4a2180a805484a1cc161d31d4a0f50 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxFrameBuffers b6135b745caaa1e0e5845f74d9759f99f47d0776aa54097ed8e8174531d3dd14 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxFBAttachments d82222e2628148d1b87f6e4ea00a24639e4d64533d367d4ff84f788ff27868ff -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxPrograms 1f5b13fd9fef992a11c49064c04fc118be39be6a075eb953cc7beb9c5e143372 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxShaders 237a6e7dfe6bb8bcef0f5f966be1230667892189ebcc48489c40a4f94ef7353e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxTextures c5fecbdb145e60a007136d06ef429a9a5f6784a10cd08fc00d12a5e85a4cbe19 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureSamplers 0b72a03a9d74b240d433e8af46306685be4c40cda7dda712feadc15d0501a17f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxComputeBindings ff095532263ed255c1a735d960a091ce1b48cefd2c7cf1d4be60fe40b645ede7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexLayouts 9f6275caf8d2aad3cdde495b8de59a1ab2f268bf3e09ebb9f5338cfc75616d24 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexStreams 37a2aa9e94beeb868646a8f5fc851d574dc5a83a66e6bffb5df5ea6e4b592b74 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxIndexBuffers 0c2aae4b18275ce796761b3ac888a8002ae2a5ae515c49a57a98b8d75220bd78 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexBuffers be25de997e1534693de0a65366cdcdcf66a5fe68c5fad0c51e620b0232dcf34e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxDynamicIndexBuffers 7bca156c069de95ac05003fd51084b790ac28c9f7a032feb8a455a6c03526f17 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxDynamicVertexBuffers 0424321ec2aff930754888e85f1b421d2830764298cc4e5393a4fc9ac4019009 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxUniforms 21c1ac1fa486528fcbf3058ce1795abc1eb4586cc02d251a2c5235e65d09e744 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxOcclusionQueries 07af44b66e47aeb757de39ddaf4b303fb5cca01fc629a764e652bfa377d52bce -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxEncoders 82d0460e19755e2f47affdcd9e89a2e50240a205c2b6d81f50a49034928e81e5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.minResourceCbSize 1dd1eec804eba223c0772ae208a19334e459dee24c9c139ca832598a5109bed5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxTransientVbSize b8c86da82f8e759710064242a75060ac64233c7b912a206524714700c96cd8d4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.maxTransientIbSize 0655d7410f1ba44ae5588777e3f2a959be9bc5182f58c2f479b32ac975551e2a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CapsLimits.minUniformBufferSize 706591535f20fa88dd227a63efc289bb84666adc251403d514eecb0dc804cc28 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.CapsLimits.#ctor(System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32) 7796f6a09820505556060e299a2a889556d3d8d4023fce78c3792f7b5b84852f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-65e3f1bf15576517"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxBlits`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L997) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39eb0aaa9466a700"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxComputeBindings`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1007) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-70cc05a83cc666ea"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxDrawCalls`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L996) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-933783d3bfe9bfb5"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxDynamicIndexBuffers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1012) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-96297dad7347d9a4"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxDynamicVertexBuffers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1013) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fa950218380585c9"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxEncoders`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1016) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5db62de07f47d7ad"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxFBAttachments`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1002) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-93aa81e57e218929"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxFrameBuffers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1001) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cacce2bcfaa1ed36"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxIndexBuffers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1010) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-51ef725397f6caa3"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxOcclusionQueries`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1015) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af03219246ee4a5c"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxPrograms`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1003) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a86d75eeafa0711"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxShaders`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1004) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-922e67fb588c721c"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureLayers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L999) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-44b8bfc0aad4819b"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTextures`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1005) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-82239e7760c0e4ba"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureSamplers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1006) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc102356bb523bab"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureSize`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L998) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a960d6261e8a09be"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTransientIbSize`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1019) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-52b394fe124f3846"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTransientVbSize`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1018) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-973474f577357a8d"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxUniforms`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1014) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-736621a2335a3f40"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexBuffers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1011) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9cbee65c09c6e4d8"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexLayouts`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1008) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c048618e05b00c1"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexStreams`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1009) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-194931c571755652"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxViews`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1000) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-90dacdb5313092ff"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.minResourceCbSize`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1017) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c77e99054396b1a0"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.minUniformBufferSize`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1020) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfcc2585f10dd6ae"></a>[`public Inno.Native.Bgfx.bgfx.CapsLimits.CapsLimits(uint maxDrawCalls = 0, uint maxBlits = 0, uint maxTextureSize = 0, uint maxTextureLayers = 0, uint maxViews = 0, uint maxFrameBuffers = 0, uint maxFBAttachments = 0, uint maxPrograms = 0, uint maxShaders = 0, uint maxTextures = 0, uint maxTextureSamplers = 0, uint maxComputeBindings = 0, uint maxVertexLayouts = 0, uint maxVertexStreams = 0, uint maxIndexBuffers = 0, uint maxVertexBuffers = 0, uint maxDynamicIndexBuffers = 0, uint maxDynamicVertexBuffers = 0, uint maxUniforms = 0, uint maxOcclusionQueries = 0, uint maxEncoders = 0, uint minResourceCbSize = 0, uint maxTransientVbSize = 0, uint maxTransientIbSize = 0, uint minUniformBufferSize = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1021) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d3d1124616a0db65"></a>[`public struct Inno.Native.Bgfx.bgfx.CapsLimits`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L994) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.ClearFlags</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.ClearFlags f85cd12b94278e6d16e1290e0cd882f5d59f22e44d35671d469e4a250c4a603c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.None 242bb572e3915a1adf262e866c6ba2fb7172c792c0037fc323f1d1ab5ff9dcb5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.Color 46b30310b7235e42ff549b7ee3c480c6ca6544c8bb4bca3eacf884161b21388c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.Depth 8d345b15eeb112a70d64d07561b42baf4fcb9400bab991071fe5f59bb1b9ff10 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.Stencil d835d99998e725e9b1066f2736501b69c8075920df99cc3ae8cfa5b08a0e6d73 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor0 77b62c2635b3e6dbc423804b47097e74326a9cd53f5841a714df13ed86996d3e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor1 fffa5d57a2d0ceb4c8ba8ca07223abd52b059cdc5f6aa45eab11d5286e8132a6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor2 010e0647c7d10affaf52729bbf5b31e3e47fe1ce522718d219b9a97fc5258dc0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor3 92779379ed46af0b20a1e2d56965ce4e47206f6cad0a290fe5248c9488ea50c2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor4 4d22cf8af73c26a0249973a56759e919567b9320f6231b061fc49e0197d90026 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor5 ba0e01161f6af9c58c98177831f1a6d8195ce7cf034e27333c22ed8bcce28f40 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor6 137f4c2e90d991941209f179e6db549ddd08bd719e1c0c3689502ca9624a24de -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor7 aa064e164262fbc22e2034b70a857d78325bfb1ce01435db1b5ce298ba4f2154 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardDepth 7d8f3e31fa112e524f1afa820c7525d8f3ff2c412bd0c87b02e7ac5e4518ea61 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardStencil 56f6423dd0d8746c797fefc9df0c148720c93dfdbbcaccb8c7442ef9fbf7c2f1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColorMask 5b2481f45c1544629e81e6d8f400af7cb66573fe9277b757d916ef38bbb9330a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ClearFlags.DiscardMask 3e2c50a8634e383a5acd52bf67b65c655a6e60b9b1932d046b2851596e179501 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-55683955805566e2"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.Color = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3430) | 当前常量或枚举值：`1`。 | — |
| <a id="api-81fa711b2c7eda24"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.Depth = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3431) | 当前常量或枚举值：`2`。 | — |
| <a id="api-5c0a3651e5629d25"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor0 = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3433) | 当前常量或枚举值：`8`。 | — |
| <a id="api-5cea43178a812f5c"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor1 = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3434) | 当前常量或枚举值：`16`。 | — |
| <a id="api-5bd465ec83a0df99"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor2 = 32`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3435) | 当前常量或枚举值：`32`。 | — |
| <a id="api-2c78911dccf14ef2"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor3 = 64`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3436) | 当前常量或枚举值：`64`。 | — |
| <a id="api-74bd2f698cca607f"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor4 = 128`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3437) | 当前常量或枚举值：`128`。 | — |
| <a id="api-77d52fc8b49e1853"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor5 = 256`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3438) | 当前常量或枚举值：`256`。 | — |
| <a id="api-23c202c7cc85e35a"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor6 = 512`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3439) | 当前常量或枚举值：`512`。 | — |
| <a id="api-7df2160d2166599a"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor7 = 1024`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3440) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-71a273235ece6fd0"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColorMask = Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor0 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor1 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor2 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor3 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor4 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor5 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor6 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor7`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3443) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor0 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor1 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor2 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor3 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor4 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor5 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor6 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor7`。 | — |
| <a id="api-fda9a32fe3f8ccb4"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardDepth = 2048`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3441) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-858ce7d5b3105b6d"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardMask = Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColorMask &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardDepth &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardStencil`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3444) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColorMask &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardDepth &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardStencil`。 | — |
| <a id="api-a7b4b210e6c342bf"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardStencil = 4096`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3442) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-f489cdc9c03d2a86"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.None = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3429) | 当前常量或枚举值：`0`。 | — |
| <a id="api-df13a9926895b371"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.Stencil = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3432) | 当前常量或枚举值：`4`。 | — |
| <a id="api-36f5a142348f937a"></a>[`public enum Inno.Native.Bgfx.bgfx.ClearFlags : ushort`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3427) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.CubeMapFlags</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.CubeMapFlags 051f8c5f1168d29cc60e5b113be5b67565fb8ad1302cd21d5fb701815d389256 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveX ef91aa7b46656652da50ffa948c351f5ea17451232f857b05d69e50850170bb5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX 781661cb869a5a35b953fa1006d3fca9c05f3e94a496e0856598c27cd55eb567 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveY 303f26f23186972768972da5fb7b7479533cd73c1f8d963fdead6a4a0e53f730 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeY 8b9242403aaae05efab35f30fc7162dc32ceb9fdf175f02ddecba08f40879ae8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveZ fd231bb359d3dc4e0fe9b90fc87fe84a5eb2543a4a6d3d2d6806dc2ba07f82d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeZ d621a5e5ab6152a00ac2ab9bdc0193149223383e4e1fde6734cd50b6d131c4ba -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0bc5ae37abaadda2"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3457) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d348511dce75e85f"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeY = Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX &#124; Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveY`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3459) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX &#124; Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveY`。 | — |
| <a id="api-bec211413355f27b"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeZ = Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX &#124; Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveZ`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3461) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX &#124; Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveZ`。 | — |
| <a id="api-8f74dc88979c48c4"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveX = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3456) | 当前常量或枚举值：`0`。 | — |
| <a id="api-d23c2f90aa612228"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveY = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3458) | 当前常量或枚举值：`2`。 | — |
| <a id="api-5cefec28499bd4ff"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveZ = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3460) | 当前常量或枚举值：`4`。 | — |
| <a id="api-2c2bac8b06dbdf27"></a>[`public enum Inno.Native.Bgfx.bgfx.CubeMapFlags : uint`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3454) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.DebugFlags</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.DebugFlags a3b3e0cea570dd9addd0117d4c5fff35ed9b07bf6b727fb013ff81cc356e159e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DebugFlags.None 6bd3227881fb6a7e2d3e08a306852d0543a5b98cf9818beba1da4adab7ab898c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DebugFlags.Wireframe 0b8da2aa4d32de5b6b5edccaa47e0b7dbee092960f6f40067e915b2c289df20f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DebugFlags.Ifh b0fdcc46cbf0d18f163248bdd5959197f2e6fb7d8e768e340a84bbb8505187d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DebugFlags.Stats 80f1a6c5e652871d09d7df6c916d62bb685cc1e451daebba6107dcbed94e2571 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DebugFlags.Text 303775f867c32e5791cffdcd212f23fb654fd8034d1d174a5cb087347192d186 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DebugFlags.Profiler ab1f415cb698fe95d32685db79888e8641b93caf02cb6bf06ef319ba6549510d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-685f04e0445b1dd6"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Ifh = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3475) | 当前常量或枚举值：`2`。 | — |
| <a id="api-d8ae087a15dfcb99"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.None = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3473) | 当前常量或枚举值：`0`。 | — |
| <a id="api-50916d8add82e4d0"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Profiler = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3478) | 当前常量或枚举值：`16`。 | — |
| <a id="api-6d432868431fc190"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Stats = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3476) | 当前常量或枚举值：`4`。 | — |
| <a id="api-88d1da0621d16dfe"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Text = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3477) | 当前常量或枚举值：`8`。 | — |
| <a id="api-cd9178b52165a1b6"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Wireframe = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3474) | 当前常量或枚举值：`1`。 | — |
| <a id="api-63de25306cd42b14"></a>[`public enum Inno.Native.Bgfx.bgfx.DebugFlags : uint`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3471) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.DiscardFlags</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.DiscardFlags 463adad1d0b5ebcd6bf5e62229ec0c496d03d081ea26b74c71a5d02e6ca07bd9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.None 16f60a85e9ae8b3dce3478f46f898f493d177a170d3c3ea7ff4e16cf5684937f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.Bindings 316df6c50949739e3f3d985f8a534ffe2788f703c62ee6d6beeae942cc0cba59 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.IndexBuffer 8bdadcae635fbdcced973362b6536c73463b5966ed0039805807efbbc21ad456 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.InstanceData e33cc55604fb60c6248c309bce5b4a3725666a4bb4648799be0c849703ef4cac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.State b6befc34ed9f16b2ef5e5501d2679f7ff311e25c133b3674ed220072be619ee4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.Transform 3f68b21afd7fd99ac6ac9ca418c4d5729b085f5e36d8b81306437ff644f15166 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.VertexStreams 47a8118f123f57db5d199e8c3dc1f3051763acb649050c024fc11ccc5251a7d3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DiscardFlags.All af305deb2c916b737272d518ba52357b0bcf0647cae193eaf6f094d44022fa7e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a62d451b83c8860a"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.All = 255`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3497) | 当前常量或枚举值：`255`。 | — |
| <a id="api-8841f49063f259c3"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.Bindings = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3491) | 当前常量或枚举值：`1`。 | — |
| <a id="api-f918abeac53be327"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.IndexBuffer = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3492) | 当前常量或枚举值：`2`。 | — |
| <a id="api-f26d559644d72567"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.InstanceData = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3493) | 当前常量或枚举值：`4`。 | — |
| <a id="api-10f2e3ab39580016"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.None = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3490) | 当前常量或枚举值：`0`。 | — |
| <a id="api-091a1a75aa26214b"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.State = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3494) | 当前常量或枚举值：`8`。 | — |
| <a id="api-4e8db6bdaf7b3511"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.Transform = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3495) | 当前常量或枚举值：`16`。 | — |
| <a id="api-ab5bf5c555e4d16c"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.VertexStreams = 32`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3496) | 当前常量或枚举值：`32`。 | — |
| <a id="api-256da9f62a11c03e"></a>[`public enum Inno.Native.Bgfx.bgfx.DiscardFlags : uint`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3488) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle 4b32340364328c0af43affec0db655738ced8926e14f2ed7e2196551ca68ca37 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.idx 96546596c287b1ace30c93454b55f9ef7b339270da9c1984d461c8b4647392e0 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.Valid 18fa3a030c8c7e32807638ef011edb4020bf9bda14ffe49052d34057ed4f4ef9 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.#ctor(System.UInt16) 9b03ab3fc3d13c8efd0e0900e04b0341fd02cd54c79cdc489a33f7b395182ec0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6c89292187534ca3"></a>[`public ushort Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.idx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2051) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2f15c3d7d0b6736"></a>[`public Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.DynamicIndexBufferHandle(ushort idx = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2054) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1c84e9aeba2e96ad"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.Valid { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2052) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-91a36af391c9f8af"></a>[`public struct Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2049) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle 756c3298c362b4a2a71c9059d3f25f46a42ee62dae513da697c256cd1698fd4e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.idx 530fe00296e11bb9cff2e355c64c806812c8879cdfa2598611725b239f7330d6 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.Valid 2cb98500b61ffc741072501d4b5b65696b5bf88a778c0930441cd57a6da15393 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.#ctor(System.UInt16) edd8b9450426c5329028deeddfe7dc6280d8f5e34257a36ea7c76cf10f375bc3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9ff345f95247caf5"></a>[`public ushort Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.idx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2069) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0ca2621934475c0c"></a>[`public Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.DynamicVertexBufferHandle(ushort idx = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2072) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4e1792860450d18e"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.Valid { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2070) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b89f5d4be9bbc624"></a>[`public struct Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2067) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.Encoder</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.Encoder d5bfbe70bedeb33a62a08d16fc6328a203ed99ce4cba8f9442d8a121503a01b8 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Encoder.#ctor(System.IntPtr) d84fe3565a2fc91c14131e8dadd246868095d6d3801a3edc11bf69f9147e12aa -->
<!-- api: P:Inno.Native.Bgfx.bgfx.Encoder.Handle f27f97f8a0857557a0ecdb5f40e0f5d0cd35142a8052ba72b591eec25268c73c -->
<!-- api: P:Inno.Native.Bgfx.bgfx.Encoder.IsNull 5b76baa7efee9bef45359e1a410d4712534cd5008e7dc9141448be16fb75f602 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.Encoder.Null aa66255983e92c35b174b7b7553f4a6af7665c8999d79c9667a17671928c16db -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Encoder.op_Implicit(System.IntPtr)~Inno.Native.Bgfx.bgfx.Encoder a67e33030317bb6a6a6112695a0c14c8a3d991de14bf71ff811c43f058c20ad0 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Encoder.op_Equality(Inno.Native.Bgfx.bgfx.Encoder,Inno.Native.Bgfx.bgfx.Encoder) fb43ac9b3e63566e26a8c59f258bedadf60a420d6fd78136baad57cb8fefb043 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Encoder.op_Inequality(Inno.Native.Bgfx.bgfx.Encoder,Inno.Native.Bgfx.bgfx.Encoder) 1156627d5e2fdd415c7ca4846c5232bdc5b3bce212a58313751b7a60495e79e9 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Encoder.Equals(Inno.Native.Bgfx.bgfx.Encoder) 1f26afd18df10e116dd838cd176fc5dd9923222841992ebff62edc310478ce56 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Encoder.Equals(System.Object) 9e9db0f5ea7ef23557a3bb04ac328281cd79a3b1c093a583987e44a5a4eecc0f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Encoder.GetHashCode f3585856da54f514bd08a747b488e15d02e8033ebafe582042057b66e1823556 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-88635e27add577de"></a>[`public Inno.Native.Bgfx.bgfx.Encoder.Encoder(nint handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2839) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-94885fb9637f6b5a"></a>[`public bool Inno.Native.Bgfx.bgfx.Encoder.Equals(Inno.Native.Bgfx.bgfx.Encoder other)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2851) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-6e0145c8df2a2faf"></a>[`public override bool Inno.Native.Bgfx.bgfx.Encoder.Equals(object? obj)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2852) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-357b915f970f9786"></a>[`public override int Inno.Native.Bgfx.bgfx.Encoder.GetHashCode()`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2853) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-84d9784b70270195"></a>[`public static bool Inno.Native.Bgfx.bgfx.Encoder.operator ==(Inno.Native.Bgfx.bgfx.Encoder left, Inno.Native.Bgfx.bgfx.Encoder right)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2849) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7ea1377692c5083e"></a>[`public static Inno.Native.Bgfx.bgfx.Encoder.implicit operator Inno.Native.Bgfx.bgfx.Encoder(nint handle)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2848) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-16aea6701e0c9220"></a>[`public static bool Inno.Native.Bgfx.bgfx.Encoder.operator !=(Inno.Native.Bgfx.bgfx.Encoder left, Inno.Native.Bgfx.bgfx.Encoder right)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2850) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c8501d0be95fb65a"></a>[`public nint Inno.Native.Bgfx.bgfx.Encoder.Handle { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2844) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c1a2564c85b28c14"></a>[`public bool Inno.Native.Bgfx.bgfx.Encoder.IsNull { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2845) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ac9ebc0c3fc8a2e1"></a>[`public static Inno.Native.Bgfx.bgfx.Encoder Inno.Native.Bgfx.bgfx.Encoder.Null { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2846) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7ea3cbdebc3e14fb"></a>[`public readonly struct Inno.Native.Bgfx.bgfx.Encoder : System.IEquatable<Inno.Native.Bgfx.bgfx.Encoder>`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2837) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.EncoderStats</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.EncoderStats 906556f0a1d9a7ff73abc5c8b4caa46381d8d549637cbe85af5a6a0cd595d889 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.EncoderStats.cpuTimeBegin 51e2efaa9afb68e2872a57268a6273982d65c6fb86fe5c6d4c72e2332959de45 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.EncoderStats.cpuTimeEnd 772bdf9572237b6536f8ec05d8f139d1093293acd41861fd44a66121e8240686 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.EncoderStats.#ctor(System.Int64,System.Int64) d726e90ca0800dfd09c2c9117031ae5eca8ec8769772e5ffafd0e7d7eaaaede9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0cc480539714c685"></a>[`public long Inno.Native.Bgfx.bgfx.EncoderStats.cpuTimeBegin`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1853) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c290f76248752c9a"></a>[`public long Inno.Native.Bgfx.bgfx.EncoderStats.cpuTimeEnd`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1854) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3c7c1d63a0821896"></a>[`public Inno.Native.Bgfx.bgfx.EncoderStats.EncoderStats(long cpuTimeBegin = 0, long cpuTimeEnd = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1855) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f82d561287f6f4b2"></a>[`public struct Inno.Native.Bgfx.bgfx.EncoderStats`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1851) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.Fatal</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.Fatal 19bf5fa24abdad059fad9a4ea4ae2f9b85ba24c5daf7e95d8444f4d0b3994c75 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Fatal.DebugCheck 40f56a28fad72595e91d81a2dff0ec30b80da83743b2c9035f80e7571a8d9121 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Fatal.InvalidShader 73b8d24d442d26c61e0f19afd8bab0d7bc860eafe46b814332497da59d7c9a50 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Fatal.UnableToInitialize eb71be931fdb4998748e09ccd69552e73e6c1bea30f2cf25bd0421d747f867ca -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Fatal.UnableToCreateTexture c72d0df5df1afb0586e49aee5bf75aa5767abac05a43cddd6fda06a36d9c4ef8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Fatal.DeviceLost 907ca1a9b2e9379ad5134a6086dc9cfaa3a20a897e541040b20ff81198c8eac8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Fatal.Count 9333bec29cb3cfdcb30b236eaa7be070682d7ffd52185823f73c5bbc80aef1de -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e1c7be5f766534e2"></a>[`Inno.Native.Bgfx.bgfx.Fatal.Count = 5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L31) | 当前常量或枚举值：`5`。 | — |
| <a id="api-d86286188a268fa5"></a>[`Inno.Native.Bgfx.bgfx.Fatal.DebugCheck = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L26) | 当前常量或枚举值：`0`。 | — |
| <a id="api-c8d5e4f834222239"></a>[`Inno.Native.Bgfx.bgfx.Fatal.DeviceLost = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L30) | 当前常量或枚举值：`4`。 | — |
| <a id="api-c3cf46b00327cda2"></a>[`Inno.Native.Bgfx.bgfx.Fatal.InvalidShader = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L27) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d1d19e1f870ff975"></a>[`Inno.Native.Bgfx.bgfx.Fatal.UnableToCreateTexture = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L29) | 当前常量或枚举值：`3`。 | — |
| <a id="api-ec62e4affe72ad73"></a>[`Inno.Native.Bgfx.bgfx.Fatal.UnableToInitialize = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L28) | 当前常量或枚举值：`2`。 | — |
| <a id="api-f1e7eab52cb66409"></a>[`public enum Inno.Native.Bgfx.bgfx.Fatal : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L24) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.FrameBufferHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.FrameBufferHandle b0ee56f2e36d2c07e534c0482c64778e90be27c0ef1f1396c27144b452944a1f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FrameBufferHandle.idx 5bae03d21976dd700e1ecab1b2da33b72dc4b9260178dc360eafc6aadb8a5fd2 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.FrameBufferHandle.Valid fd4705399bd43a9984b165a030ad87a361343fd1e0d63d70393c8d5b1313863d -->
<!-- api: M:Inno.Native.Bgfx.bgfx.FrameBufferHandle.#ctor(System.UInt16) 51131143b8df9f8a35520b9ee1ac7a2f3b7e30d62549c294583dacb3a0630cd9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1b152bc971c6863b"></a>[`public ushort Inno.Native.Bgfx.bgfx.FrameBufferHandle.idx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2267) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-44e9aecaaf760a5c"></a>[`public Inno.Native.Bgfx.bgfx.FrameBufferHandle.FrameBufferHandle(ushort idx = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2270) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1c560a8c46664762"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.FrameBufferHandle.Valid { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2268) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-07501cc8d93490b2"></a>[`public struct Inno.Native.Bgfx.bgfx.FrameBufferHandle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2265) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.FrameFlags</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.FrameFlags e2c549519bef895916cb1ad6530711522ae7ffce5b6c58bbce2f8080350c92d2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FrameFlags.None e19600a4a626c216a5bec920e0fbc0868b1de9bf5a54b5de0719c8d1c8883ae5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FrameFlags.DebugCapture 8909f6cec9ec64430b11feaf2a01689d8fd471d7121d8d8fc4ed5b206f6b96d0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FrameFlags.Discard 78a1e3e57040ff0fc29cb0e9283a3c28fd02c7f74ccf224bf30638aa094c2f6c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FrameFlags.Flush 6bead72177dc7a457dd687e8b9db95e8b2580dba67b1796000c55565b031e50e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c43e6f1ea597cbe0"></a>[`Inno.Native.Bgfx.bgfx.FrameFlags.DebugCapture = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3510) | 当前常量或枚举值：`1`。 | — |
| <a id="api-5a6922758581a816"></a>[`Inno.Native.Bgfx.bgfx.FrameFlags.Discard = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3511) | 当前常量或枚举值：`2`。 | — |
| <a id="api-ce5dd3173913b278"></a>[`Inno.Native.Bgfx.bgfx.FrameFlags.Flush = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3512) | 当前常量或枚举值：`4`。 | — |
| <a id="api-7791472875fccd60"></a>[`Inno.Native.Bgfx.bgfx.FrameFlags.None = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3509) | 当前常量或枚举值：`0`。 | — |
| <a id="api-fb7fdb29c3b233fc"></a>[`public enum Inno.Native.Bgfx.bgfx.FrameFlags : uint`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3507) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.FunctionId</code> · 204 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.FunctionId 46602187dc43f5e94b4285a46bc72ac6cb24a818cbd46249e4f3c1936b6e61d3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.AttachmentInit 048c4975dc2fc7337e48322e9a82c28e8da8303fe46815df45270fdfe463c3a5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutBegin 5f4021bf1909650a4945f40c6399e91338a6b60103bd7a2b09075e1be97f9eec -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutAdd a2ef326a687e7db0a7dda80932465f8867d88cb14ccd5c00634406d9703cba54 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutDecode 252be8e03142307ee2af0efb44b1f0b5128155201d87f62eb0354fb0f60d48fe -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutHas 3194d81f702248f83aaf3aacc53f1daf8c54e5ae489c32b22a2f4136dce26bd8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutSkip 2668230378f7015db7b43864205236f7d892e5d9102e70c1f9f2555cbf9356c7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutEnd b3021983243ff009279522d2d42b4c83b1b954fa116287802fa110a6130ced20 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutGetOffset a0a038455cbc4924a502830f10eac0ac91b07f7baac2e0ca66246c2a6accc042 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutGetStride 7b5b3deb04b9bef31756f4b08ee4809494bee8dc6116341ff456651e35173bce -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutGetSize 24156ddd4540268c6cf4f88de35680baa24ee5217013fa6fe4c67ddc0f79f17d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.VertexPack 7601e085ef452812f056f8c5e18528c918cc87224c2ad01b6eda3db56b6d33fe -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.VertexUnpack 2d8e5fa53ba0b6fd9ac37ae8584cf60528146e61f274e0a675a2bd43eeb850ad -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.VertexConvert 5d6c4db2aefa83f85377f8c5953db4a451476c78650ad4da6b11608ae2263238 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.WeldVertices 6de7cf4896b8622207c11482d35becdd7c911fff4af00be173ee7b1740802474 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.TopologyConvert b01fb5f6f77e82a66f46aa7e59f45bb55fd6eba3d60331903be1ec81aa9c5b80 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.TopologySortTriList 5b0c504b241e861cb18cde37b7dc4a9be150799f7519e4dcfde50a8de634636f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetSupportedRenderers 250c000d66ae978bdb030c82b010a2b24e9582f67f409b9f15b0f815f89db971 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetRendererName 7a4ad97ad0d9996d55d4fd5975bfc858efaaf5f081d8c8c80a36fa1cab3e19cb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.InitCtor 848a3e3a575303824f9242eba014a07319cc6e421627843a6cd88e99cb38f4dd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Init ba1e186ed307bca5d77a593bcaf758057e80eb04c2b7c658ee5b2ebba6ccea67 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Shutdown 30ce2e1f7d3eed26feb85b4cdfd15f48a50c5c961064aa01cc780009b5f0361a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Reset ef1c520c29279a7e708c9d3caff920ba29248d1a2e529ebd187ff2d5d478ef45 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Frame d4a6524fbfbf33174aa0802ed9aaf63647e127244fdac4831af3adda787120cf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetRendererType caf617c5a597083a65d6d5832caf45587859fcbff0edfd586b948c8e1e62e863 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetCaps 26a7b591a35f067db3666f3ec2e63c250d0ba827a9ee78c128b398bee54d6df9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetStats 740c2dd559d39adb22853c96f7af3f6e88d8e028399f59e50e054edbdf2f31ce -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Alloc 1b98defaabae4cb48a8170657cd99fe3f05ce6477450f4fa2f25aae85def01ba -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Copy ae6c351bb78763238a17d8046b2774bfccdefed6d0e0d003317c9609aeb4ef49 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.MakeRef c787b008c06a5a1285dfe816f2c4f39d23cee42819f7fed92ea2cd76c72e1081 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.MakeRefRelease 9f1f04f00f153fa33a54f75505dc75bbe0351396ddb3fb475233f4b4dcf41616 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetDebug b94ff96cd125ed0ea11fa105a9ac3a87e80d2d96ba4a9a60a242a4e3f8cb0f19 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DbgTextClear 1f1d6bf88b04d32781e52e7a4dc38425458e8e5f6796ca28b284c3b3722a1c73 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DbgTextPrintf 8ab3d1cd851895c7a416b108771490d93c6aec43ea2cf3bee9f6fd7bb35098a8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DbgTextVprintf 7dc3743b17201dbdd8315383af2c7c876e825b9a4c52b761b52f5d802400c486 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DbgTextImage a5f9209de7b12b445bd032e79f6e7dd184d42b53ce72394f3cf84c511a8f6eca -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateIndexBuffer 16119b5f64396aa43f639eb3c6dfee9e57e45e3354f4560aed0e74a4cc4a7d94 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetIndexBufferName 7c05a11af176a9f40fb57334a2349cbaaedf5eb06757f3f011f0b5f108338260 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyIndexBuffer 4fc19fa4976003ef416ddd4aa069f30319e132d9914121f42f4e37cf8b168087 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateVertexLayout 2eb348c8a3326407a87017fc8ca43bf49ff50108236481ef2c41e6ed42349ce2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyVertexLayout 593d4d79f80d3d98ebe3d7af53f2e3f89fa9e36519981e38bd395570135a9201 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateVertexBuffer fa156f943b3b20acf222e9e61ebcedcff8031e0c0911caca74eb77529e99b19c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetVertexBufferName 351fd945487d73de7c50495c80b452761e9024d141554c61bec9c8040d9b14f8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyVertexBuffer eb1e7a391dd10b1ff35666e9628e3818ad7412ec7cea36ebe4b2df5cb4c1637c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicIndexBuffer fd2f52dd2db68c29b22e2e56f15b1eac32a47dd6f7260e6d9e28b39778b33420 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicIndexBufferMem f815149972c2a9ac3cdaf3184960beae8ce6c35cda493691886320f6fcfdd3a8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.UpdateDynamicIndexBuffer 518b537273ba8e9a6b1c02c807eef345d5b542821bcc32d567e89f01cfcbcee5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyDynamicIndexBuffer 40187af551a8843b9c4640dbc77dfb00f13e07d50cf405af20b0067691188f28 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicVertexBuffer 73610e15d3ddf6938ab7a12cda8ce4f57ceb5ef32334c7fb322b1bbbba94ce0d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicVertexBufferMem 6bae9fc4fd116a3ebb0d8830858b24a2559974e82e3b94d287a1cc4cc6116a67 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.UpdateDynamicVertexBuffer 66dfa2ac1034a306ac3c438f71974173979685e57060dbd02925cafb19fd5713 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyDynamicVertexBuffer 7348961ac032ebd317507afab66f0ab761eee042961fa9e4276ca96e4b91ec7d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetAvailTransientIndexBuffer 9958c32665e3993ca8d7ef2d6430ddc6c9bd6a5327c6095127c8cffbdc6f88c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetAvailTransientVertexBuffer 5e462dfa2fc1f0bd36f437c299c242001154d63e5f5a3b9e2d35e03c69e0e3c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetAvailInstanceDataBuffer 9ead60a9830d0ba1799c545573556c624c4f09ad8eb6b05816174c6bb6647485 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientIndexBuffer 63f4b65bfe2c9f9308797e812c66777a7133ccb2e27680dd3d31546770d39287 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientVertexBuffer bbec0776f74323d2b2d0fe30be97917686abda90eb38558d6867021f24dc72b5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientBuffers 49d654f19b55a8a7b0da32de5627d7e0bee42a650c1990f664c77313dd9632ef -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.AllocInstanceDataBuffer 0d80f9662e2e57620ee7ea0e1cd9fbe3a0e82e1e2be1c06a145f7ef7a3a3d2dd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateIndirectBuffer 109df236c503bff4d6f83d6aef9442d3fc4fdee0f25705ee8076c4e80eeae424 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyIndirectBuffer 6ee6b123b29f6fa398da44056968e8ba750a3dab9f5a9bb1fc8bee08e5b10dbd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateShader 09184394f3e6cc381f9e724269137a5b4f139177534caf7f094e08361c38865b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetShaderUniforms 2a98738722ff461c5895377b49f850c29debbe9f4946aafe76256543c9c68223 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetShaderName 7db7ef15670691135d8b4f87ab1f39773286237b16863cf5823190fdf7f2c69a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyShader bbfb69a1a8539ad150faae6e922c19fb5e86bfe0bbc3a1f60666e2b002a2111b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateProgram 302306beac5e94263bc1a9a586e87272d9cbb6960d31543e9d10053f2b81433e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateComputeProgram f7e65cb98bb010fcb4ad43a517548aba92fa68cbf2be68038b66e9190654d2a6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyProgram d1f2a896d2eb55c5399f7d5f4f14af83a58fa3e704b7e152dd220d471143a10a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.IsTextureValid 80b8a620847bccf07ee6666d1224a37dc1d3ef49ade985b7e0b7aa7e534fd80c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.IsFrameBufferValid 111a23fade94e0eb807b065d8fa61f5fd3685bff77dcf2781452f9c0786d92e1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CalcTextureSize 4da46723055ae9b4c2b270ed7c185411d7454b0f969cc1ab734428a7fba72978 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture 423685972e236e7b179ae4c4106995df4bcb010c4c94cdd4769b59b297404ae7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture2D 13b7a0ca62aa370f617f6483856a61f613f2ea171161662d6eefff74918c2cc8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture2DScaled f22ab35eda1deae0c46ec4246152f79ddf11156464c052a42f04039de4b8ce77 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture3D 09561ee8073f3adfff65026cd7d454cc0e385888ee6df97c4d461a01bb68de9e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateTextureCube 9abfa76a4b5171653cab76ab49bdd601f3d7c79f319b2a8283742ed921689424 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.UpdateTexture2D 9c874d0443395114ee9a62ab6f7d2a3ac0ed60c1ebff553692fc00fa29b5a1c0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.UpdateTexture3D 34533ed0094dd7b74b4d874d3db58202b89e6b37474e26935f919e995b059237 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.UpdateTextureCube f61e04a8f6a95798bc81d87475072bcb57c40e924578b7c42819ab6b09f097e8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.ReadTexture 7b16ac715ed651afd7202d56ce9e383c8cc92c2164fe6043809dc8fd902b1c54 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetTextureName 75bf768f90c5eac78794ad03b444b0e2f1a7a5f50b8506aa1ebdb9310545e05d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetDirectAccessPtr 1039fdbec557d90c0d91ddec098eb1880881b9231a216c6a9274d21aa181d107 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyTexture 79c78aa41445086b509dd83a8972f872f01159b3204af9ac38c889a97a271b72 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBuffer 6380e82944bf2b99a2aabc1b53d8c52f32371904932033ad88d935d6301ba9df -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferScaled b3995adb7d797dc91d43e71e4bf149d6ae06abe2df6acf409e605b3d9dc2a657 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromHandles 99b2d27b4ec79fa5d190f431be3f3ae13e30d70e8c284b0c59406f5af7e93f5a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromAttachment c811b8abda7c89a8f5546de2c803b5b279041a1df884bac9ee9c2bfa56cd91f4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromNwh c29bb93c37ef3235a859ce5cdfd17ba5c5f2fd0349b2ba154b27ed7aa435fe98 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetFrameBufferName 2627b1e03489bafa1c61cd3028c562523b19dba4816c76e7dbc471732c5d3fd1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetTexture 50da90495c674449b1780ca5f0aa37bfdec5b6b10afffb4a5f1bbba72b639ca0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyFrameBuffer 7e9dc273f245d5057e0c2f00d53b686527838d2f153238efe4674f6c64b0e716 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateUniform bc43bd050044fda3ff6a00514b9c8c8255fdd8f90c914eef8edfe39598c272e3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateUniformWithFreq 175dce3315553ccee7f21093e57712f805e67f1283609fb5c644173369586b84 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetUniformInfo 60fdb5f0a1fc03218f41656598bceb5d7dd533b1dadfe1f133436b2f9aa05431 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyUniform 0ec9febfdb38202482790088dd14fef7135a0c316508893df3edfb0e2ee4160f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.CreateOcclusionQuery 295d18b89ece23e3de16658d5e6dacbcea89207c724c3221af4bf8e84a538e16 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetResult fff9dcb2c33e16ff52085d994231dc677e919a5ba15f95c90383483fcd30bd5c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DestroyOcclusionQuery 36a696d2327aa8c98297ca26b3a1fb90142b7ec4844e6623b20946bf33242bb3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetPaletteColor 0beeccc2d920ea6bb233da0f3f7efae8eeed80dbf2923036c9eb86539ffdad92 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetPaletteColorRgba32F f677384c4c1ce32e622ddada63de161d993aab60f6acf4a19b9bfa80b8f68b3e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetPaletteColorRgba8 bd812b7a9ce5f77dfe32854e0913f649367d05ab019cdab31015808cc292c64b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetViewName aea2441c0c474ce13cd220e882f487ba414376c009aa85a0474c6cfe0b8391ea -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetViewRect 630abdd59aae7c9788093717cc39181ffd040cdc5dba4ab1fdbd40fcbfda2055 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetViewRectRatio 28c898aa29c4b7b707a92af755f172a532e4fdcb7d0bd4c86d0a2fc82f06e23a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetViewScissor 5d50f4f5315a7be630a5d1483216963a3fd5bc21d84606af55605c654e7deb73 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetViewClear 2eb7c67c81543e2d21ae1fa822bf34e5ed5333c7069b1b52972aaa4af8b982ee -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetViewClearMrt d209c528a64baa943ce9e14065eff4bdb2340b87d382fe02571ab9587d93b574 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetViewMode 9b373ddd21ad37770a40b5e9c47313e52b6407fdf2293d112349a2af0c3e4e4a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetViewFrameBuffer 171eeb7ff5b8b18421de68f8ad4a5b94ac6a458dbfc81941bea616f7b3581fdf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetViewTransform 192a2bbca0a31b08889de9efa2f5e2ea8311d5f7f25c1ad98b5e27dcdac75a41 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetViewOrder 7ea04be90d6c96d0a82e639000e5707a4308d8e247bc12419c0a1b1e3fe0b9e4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetViewShadingRate e4960ff83fafbaa88a19830e5011fc94f0a79233e7fe9b45942b8db9d7f9e59b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.ResetView ba5cbdf2bfbc78b082f4f6e038e521df752ba26370003a8584a48e6fe04e0b10 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderBegin 7ced6d64890bfe94d00f3fd69e8afd69f4773736cf337536909f7c4b3f81a716 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderEnd 35b551d7ddb3c24f334a13f462fa95b26250ba82edd5d793935d0bf890a05a98 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetMarker ac856f0b6c69436bfa9ccfa31e86f65ecca6b67beb04bbee8054b76e15947a31 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetState 98836579c8ff44cc7ed773793e3e86d218dda085acb657faaf45081d6afc77c9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetCondition f808818835532e385e97ee64f6b2673652fd49bdae90a86cbe2aa5c5db579409 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetStencil e17da68488c73b45cd17ba79f4a6cdc769bda31544c397a34bb59f2f6788e165 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetScissor 5f3c3b0554b5ad4f3a2abf7d5609a80c21e77ec1381ae62ca18de9e5007494ed -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetScissorCached c7b39b4fc1267329a0c4b6d9e34892621a462ac816bcaa1c1f8feebf13691dc3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransform cc591bafbdc6a0235bc09385dd8d2dd45b63575df67834c9a53d0dfc39bbf076 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransformCached 2d8abe865332c47dc2c8ae50497ee355e273d9640f32d5ff0398e14081be2f66 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderAllocTransform 838279d53039732f580be9b8069a3216b367832f8ea40612eb4be5e14cd18696 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetUniform 2eb5cef0809f5f943f7bace9822464cdbeefb436ffd877e8fa36b33b53fa1728 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetViewUniform 4fb32f506afb20efba319b2264ffbc7b0b2d6791035351e6a59086367d6bf1f8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetFrameUniform e955ed8673efcbe939fcdf66a8ea40c245bf874137b523ea317a274089e8a53f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetIndexBuffer 5ae6666aafea2844fd80a44a34f1789cf480768c59a0b10d7e0b13419e999f7d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicIndexBuffer 418710afbda728083cdc6fea5c948fb276ed71d071dd60fab3ac569d529d9f7d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientIndexBuffer 337ac10d65601d14379a77ed48cf2825cd346e725eee485e26d07eb5f7e9fbec -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexBuffer 80fdbf61ffa83ffc632906ecc2b705d55df6b5582009c0dda6a0a915408fe91e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexBufferWithLayout a5152f8e4351ddc86a2fc31bd74ae485e6960495fecd557787c0d14fd6f95a67 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicVertexBuffer 5d763d8818464a603aabd7ca259af3e270d85c04d769f0eaaacb242fafcb1779 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicVertexBufferWithLayout 576d85cabd7f61dd5966b0787aab75b7b1beba6d3a2d9e30e069812041b68c06 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientVertexBuffer e546888f09dd17bc17d1ef9e072142adf0306326e355a2ffd10caf03eb7154a9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientVertexBufferWithLayout 5b533b5b57cf46012508485b87b58cffbd2c51d6d79c7957ca4164d75b35048f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexCount 9ce855ad84219817f85f7cc9d6148fd2fcf13d678d0a9086cacbed10eb4e3444 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataBuffer 03215ae6721304a99bca300bad0aff5c9d44ef0daa5f3aef19ace5daecffdd56 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataFromVertexBuffer 626783fbaf2e8b2eba987ba8d4cbe61f447c0e0d7524f5222db96c7d4fdb92e0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataFromDynamicVertexBuffer 991883543ba1408de2f311a30768bd7638363716a276047027bd0b77709acf1a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceCount a470105e22a7919bdd8633a5ec75f9c2f93e4e23eacc46bc857287be2d67c709 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTexture 759374fc99df5f15efed192ff21ed6c31d5c7bf13bf29b39160988c35615fe52 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderTouch ade34a7f3e714bc4f02b2d9041e0f423dd58892c2d49f656121a84c268fe3d78 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmit c39c5cdaa0963284e627ae0c20c92f420c5e5ed6dccc1a5a6c83a89f3b76bc63 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitOcclusionQuery 9b225e89ec1d2d0b74e640f3d3b7d32c07bda41ef656ce07b65eeeeb2cfcd51c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitIndirect 89fe6af1577337751ebffb53f7d8dc2c4b3e803b95c82a8193a54ce7fc2bede6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitIndirectCount 9625a47bcb590bb685c3d560ed0018eee6e0f6058b5a5dbc3a26a9a0d8d3a2c3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeIndexBuffer 711ff52050139876971192dbdc6b03cb81d0ba6b0100a05258a517fd0f12459c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeVertexBuffer deeb50ca76ca411c3d8d153226c49ed603c72f1283f2299b6517f18d1434dc15 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeDynamicIndexBuffer a373e2cbdfc7e5b390dfcfb39e4aee086835ecdc29398a8033822509ad41ecb7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeDynamicVertexBuffer 0563f12ea6d28bb0859dae05480c77b03be59c3806044f5eec1a707b36d51350 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeIndirectBuffer 4d664e112a778d389d63db642d5c17c456b8b11d2aa29ffe7fef0a9f3689454c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetImage 5bbb33c7946e24b39d9a6459e3e62f628ff842591eff098d29e168fca5fd645e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderDispatch 913b4f25250a9cfc13bb5ade3c5ea67c1c8ffed96a8872cbbf3140e332f4cbd3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderDispatchIndirect fcd0a147785d9a13033e0269eb7a24b2f3a95f5324eaebbf847a1f420e1770ee -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderDiscard 3157b8749e754900113b35b1dc8a9d0817136bef6fa6b3dc54bdfb7893e3af9c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.EncoderBlit 5d465fa76fe896e9f352e57307f2dafbca11862ffeaddcc8b58417d20cae4320 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.RequestScreenShot 75c9f797b0de0407f2d948635d5fe04f12b7e5fdcfc0202c791d664797d87595 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.RenderFrame 1733e6bc7994f2d584a65fc37970432d89d7a04842a43a7cc461df6ab5b47739 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetPlatformData b8267538f909cb2ba5d9c748af5bd6057a5ad1b22db52b44e6d891a6ac618e96 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.GetInternalData c8a790cd219b73c009707e2fbdcf97e9b993a71fe94075ab445b471a37fa95e4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.OverrideInternalTexturePtr 926bd87fcfd397bc1aeca1fc8bdd547b862fbfbca9ce0e301cd8e5c92bf19831 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.OverrideInternalTexture aaed258dd8ab340215bafaec006079a165f1cadd9c954fe6ca54cfeb7d6a11c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetMarker d4a50bd5f9d1074fa48a84786b2a6b0f331a0c819635ecb6254d1544bc72a987 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetState d604f5a269a5aca18e08c9af7909c93ba458919634a51a535c56aa885c376aa6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetCondition 14422c640b37688664c33d30fb9775b514a1fa35543e01ccad5f95bfef564a6c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetStencil c55173ef5fa6793284f34c147243cc04e8147a3d089d98838b0429808a067e75 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetScissor e7ede961f0e1620fa02d53361cf250ee88171886e1512a40f5b5c79815b52ea2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetScissorCached decffc57a103d57af486eb8e2933fef29953a0355079baab7e5441e2244b07e6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetTransform d2b413ebe96f555e096eff4551701100591e4d9c2dddd436982f2f199e76cac5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetTransformCached 3c8446149f84f8127dacaa0585be807ccd3331770d9d43827eb20b5b8c879f68 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.AllocTransform d4e99b631fda8409a9e1236a17355882fadd82b1aa3a59f1c563525a0cb8e75b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetUniform bf50a479fbc2fc59505918cbd6ed169bb3bc5db1b8e313e9421eaa3da7b1a49e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetIndexBuffer 386bca8ebac25927d72a22ae958cba878b782c7477ad90e8092ff4cc9545a03a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetDynamicIndexBuffer 66ab1e0414449ec39503fb429737e30b14dcb63ab7d729f4a573ba4635e6dfe1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetTransientIndexBuffer 895bf00d84cfcdc7b16d6d976a589496a29e2f5062c1cb5efb72989f297fa1cf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetVertexBuffer 3d7e3f8af60dd4dc719411fc3ea68054d8431c8b37b6a887033e4d7ec4403dc3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetVertexBufferWithLayout a690c71d9455cf651e182431d9a80369b069133f80e5211bda98fcd16ef1a6fc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetDynamicVertexBuffer 1757cd513503cae8080fc7a526014b4c34e810af3cbee03e4bf683360a1c872c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetDynamicVertexBufferWithLayout f9de12216fbdbf0824de2f227ac51115e8337c1750e4b4146775bccb9a4d7fd2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetTransientVertexBuffer 903458f76a00b822a10598f8e19845562e5cb5162160751c74855941e1b877f0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetTransientVertexBufferWithLayout 45af03456f73e8247c865dc41c893f1d0b987fbecc7ff3fbf24a72c94f043d07 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetVertexCount d3913f391992f8824db29c3b997cf214d22729365fd1e3ed1ebe6a00173e58b2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetInstanceDataBuffer bc45bd1bc9f77eae443364ba86ef84908be596096665fadf73bcc7e60453921c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetInstanceDataFromVertexBuffer d16ebf683121425811878f165e9d8370c53ac169c9b330916a54fd2ac7567946 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetInstanceDataFromDynamicVertexBuffer 4abb7af47f389ab9e57e323a783afd59d825a367c5125ef7c90b7aa38eea620a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetInstanceCount 80ef0d43f7e81cb6afff54e11648e28349b4b6189931d7f9f6dd1507ea6ee22e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetTexture 3e3b639a00cf75d0d7a8acbc4e08373eac6d9fbd88bd685712748ebb293c69f3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Touch 4584132d02b79675f2a9574ffc38bfd76918ddf2cd6c90ea24446c328bb64dbb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Submit 62d11a62f159d671a29e7433abc8a323aee692b8f0091aae05f52f740237b970 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SubmitOcclusionQuery 645d5a0098fc2eeac2272ac581aedb0a7606ea50a6086cd4ad7a19fd33bd5428 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SubmitIndirect bff36ed7366cb922233bf6d87bebaca4b332cb983d6e25ab537243e8664b3263 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SubmitIndirectCount cba160aee79918f16a2a2d2c2ba9b0e14a29b2d38add9b185c213919195ca92d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetComputeIndexBuffer 63dece3bd700a00eeb7e590b25215182718b69acfb9f8ddda73501f2151c9f9b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetComputeVertexBuffer 50293cea470eab1cf362dcd0816a5ef7c1ff6f45b0fa73cdd9d0a1d5acd3b845 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetComputeDynamicIndexBuffer bb0a5af9f7b81ba00e7a1ebd4533d8e423816547ca0ddaff5b2d388b607eafa6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetComputeDynamicVertexBuffer 562791e1ba62cb9b3c4bbf50722e96da7baf8a94f755731e9f012dfbf43af807 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetComputeIndirectBuffer dcb7d120aec62d93705b480b19bc4f1ab360119a8ed02aa1da4b566df276e53f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.SetImage 9a7cec6eb2fcf080a101b7cabda2f128158a1db949740be887f7a8c70fdaa25e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Dispatch dd113c3c56aea26cc6583d456db9dda1f65967369f1d5a9e4c6adc82104fcf2b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.DispatchIndirect c99ed018f8976dfdc2661d6494655802f3a45ceabc9967a7c9cf7751d5090317 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Discard 561f7f70eaee90d6bfe11a29b5ae5b8f8d757dea5825061035f3d5828c93b6bd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Blit f076b24c97eedd5edda262013b1888d468c57def867b55e56f50ad7047fd5bac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FunctionId.Count 86fc5502bf30f6bbbdf8ceb1c339e8e8701715498a0a54710fb156b2faf2c46a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-85a895ffa6a5093f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Alloc = 26`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L439) | 当前常量或枚举值：`26`。 | — |
| <a id="api-bb969015e6940eb4"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocInstanceDataBuffer = 57`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L470) | 当前常量或枚举值：`57`。 | — |
| <a id="api-0d3f590f8a4c4b49"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocTransform = 170`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L583) | 当前常量或枚举值：`170`。 | — |
| <a id="api-507c30efc94f5dec"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientBuffers = 56`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L469) | 当前常量或枚举值：`56`。 | — |
| <a id="api-a709bc8bbbd7544c"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientIndexBuffer = 54`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L467) | 当前常量或枚举值：`54`。 | — |
| <a id="api-f05db85ee0df0cb3"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientVertexBuffer = 55`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L468) | 当前常量或枚举值：`55`。 | — |
| <a id="api-67750b9743fc7927"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AttachmentInit = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L413) | 当前常量或枚举值：`0`。 | — |
| <a id="api-5883e5d81ca902b6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Blit = 201`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L614) | 当前常量或枚举值：`201`。 | — |
| <a id="api-033a8e815148ef7a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CalcTextureSize = 69`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L482) | 当前常量或枚举值：`69`。 | — |
| <a id="api-c9d9dac5946fa719"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Copy = 27`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L440) | 当前常量或枚举值：`27`。 | — |
| <a id="api-a59a35dafff487ae"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Count = 202`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L615) | 当前常量或枚举值：`202`。 | — |
| <a id="api-deb35b30de1ffb76"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateComputeProgram = 65`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L478) | 当前常量或枚举值：`65`。 | — |
| <a id="api-e38ef9ea4ac56565"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicIndexBuffer = 43`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L456) | 当前常量或枚举值：`43`。 | — |
| <a id="api-2cf9e85c14fa7395"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicIndexBufferMem = 44`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L457) | 当前常量或枚举值：`44`。 | — |
| <a id="api-7378243eb84a1ca6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicVertexBuffer = 47`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L460) | 当前常量或枚举值：`47`。 | — |
| <a id="api-4037f3a0e69ef8ab"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicVertexBufferMem = 48`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L461) | 当前常量或枚举值：`48`。 | — |
| <a id="api-9141babfb0a48504"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBuffer = 82`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L495) | 当前常量或枚举值：`82`。 | — |
| <a id="api-5ed772833357b885"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromAttachment = 85`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L498) | 当前常量或枚举值：`85`。 | — |
| <a id="api-55736e389a8543de"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromHandles = 84`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L497) | 当前常量或枚举值：`84`。 | — |
| <a id="api-047b35b246803fe0"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromNwh = 86`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L499) | 当前常量或枚举值：`86`。 | — |
| <a id="api-f7a24a9c48cc3e0b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferScaled = 83`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L496) | 当前常量或枚举值：`83`。 | — |
| <a id="api-b5e2e51d8993426f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateIndexBuffer = 35`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L448) | 当前常量或枚举值：`35`。 | — |
| <a id="api-4a0d5070e2aef459"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateIndirectBuffer = 58`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L471) | 当前常量或枚举值：`58`。 | — |
| <a id="api-e5353f5a182bd08d"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateOcclusionQuery = 94`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L507) | 当前常量或枚举值：`94`。 | — |
| <a id="api-67432dbb454409e4"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateProgram = 64`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L477) | 当前常量或枚举值：`64`。 | — |
| <a id="api-642ef1fd65fc4ff5"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateShader = 60`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L473) | 当前常量或枚举值：`60`。 | — |
| <a id="api-815b5dfcf30d5172"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture = 70`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L483) | 当前常量或枚举值：`70`。 | — |
| <a id="api-d46806e272de45cc"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture2D = 71`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L484) | 当前常量或枚举值：`71`。 | — |
| <a id="api-1683f804c203dc6d"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture2DScaled = 72`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L485) | 当前常量或枚举值：`72`。 | — |
| <a id="api-0a7281c761f6f746"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture3D = 73`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L486) | 当前常量或枚举值：`73`。 | — |
| <a id="api-14420387b9f7fa09"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTextureCube = 74`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L487) | 当前常量或枚举值：`74`。 | — |
| <a id="api-9b9f95c9d8a52e13"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateUniform = 90`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L503) | 当前常量或枚举值：`90`。 | — |
| <a id="api-99cef14808b7a8d6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateUniformWithFreq = 91`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L504) | 当前常量或枚举值：`91`。 | — |
| <a id="api-6f2e1baf2f4b01ab"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateVertexBuffer = 40`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L453) | 当前常量或枚举值：`40`。 | — |
| <a id="api-0be9d301ccf88065"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateVertexLayout = 38`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L451) | 当前常量或枚举值：`38`。 | — |
| <a id="api-7280f13dff5a97aa"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DbgTextClear = 31`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L444) | 当前常量或枚举值：`31`。 | — |
| <a id="api-36258c40c9c1a806"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DbgTextImage = 34`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L447) | 当前常量或枚举值：`34`。 | — |
| <a id="api-65d3b625fd3ea013"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DbgTextPrintf = 32`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L445) | 当前常量或枚举值：`32`。 | — |
| <a id="api-c6971919bf077fa2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DbgTextVprintf = 33`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L446) | 当前常量或枚举值：`33`。 | — |
| <a id="api-749f24806ff8d30a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyDynamicIndexBuffer = 46`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L459) | 当前常量或枚举值：`46`。 | — |
| <a id="api-13df7fb7a87febdc"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyDynamicVertexBuffer = 50`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L463) | 当前常量或枚举值：`50`。 | — |
| <a id="api-03f738743c0c5cfe"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyFrameBuffer = 89`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L502) | 当前常量或枚举值：`89`。 | — |
| <a id="api-85e9ce86ac819890"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyIndexBuffer = 37`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L450) | 当前常量或枚举值：`37`。 | — |
| <a id="api-6685182d0c0ee5e8"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyIndirectBuffer = 59`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L472) | 当前常量或枚举值：`59`。 | — |
| <a id="api-9879c983dbc1a970"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyOcclusionQuery = 96`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L509) | 当前常量或枚举值：`96`。 | — |
| <a id="api-e50f4b643561b3da"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyProgram = 66`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L479) | 当前常量或枚举值：`66`。 | — |
| <a id="api-7f24fe09a08d57ad"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyShader = 63`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L476) | 当前常量或枚举值：`63`。 | — |
| <a id="api-e368b60ab5e08171"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyTexture = 81`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L494) | 当前常量或枚举值：`81`。 | — |
| <a id="api-09ecd371004e5f47"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyUniform = 93`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L506) | 当前常量或枚举值：`93`。 | — |
| <a id="api-2a2c96c292a3259a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyVertexBuffer = 42`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L455) | 当前常量或枚举值：`42`。 | — |
| <a id="api-34a899dd698472ca"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyVertexLayout = 39`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L452) | 当前常量或枚举值：`39`。 | — |
| <a id="api-eb05801310aaae45"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Discard = 200`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L613) | 当前常量或枚举值：`200`。 | — |
| <a id="api-7ee9e8cd8e18bb14"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Dispatch = 198`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L611) | 当前常量或枚举值：`198`。 | — |
| <a id="api-780eb4f1274a8e18"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DispatchIndirect = 199`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L612) | 当前常量或枚举值：`199`。 | — |
| <a id="api-71c0cf0ddf10e8c7"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderAllocTransform = 122`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L535) | 当前常量或枚举值：`122`。 | — |
| <a id="api-5c7371c9a38b0a63"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderBegin = 112`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L525) | 当前常量或枚举值：`112`。 | — |
| <a id="api-a300cd97efd22d2a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderBlit = 155`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L568) | 当前常量或枚举值：`155`。 | — |
| <a id="api-7cf672d21f42f32e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderDiscard = 154`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L567) | 当前常量或枚举值：`154`。 | — |
| <a id="api-966e173e32ca68be"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderDispatch = 152`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L565) | 当前常量或枚举值：`152`。 | — |
| <a id="api-25888a212ca46f53"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderDispatchIndirect = 153`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L566) | 当前常量或枚举值：`153`。 | — |
| <a id="api-5750c5f3cd4657fc"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderEnd = 113`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L526) | 当前常量或枚举值：`113`。 | — |
| <a id="api-7b61fd7dda94a022"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeDynamicIndexBuffer = 148`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L561) | 当前常量或枚举值：`148`。 | — |
| <a id="api-0d6fac852d8101a7"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeDynamicVertexBuffer = 149`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L562) | 当前常量或枚举值：`149`。 | — |
| <a id="api-db68b460687f7c6e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeIndexBuffer = 146`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L559) | 当前常量或枚举值：`146`。 | — |
| <a id="api-4bf12d88c3532b6c"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeIndirectBuffer = 150`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L563) | 当前常量或枚举值：`150`。 | — |
| <a id="api-1acbd608d5435008"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeVertexBuffer = 147`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L560) | 当前常量或枚举值：`147`。 | — |
| <a id="api-4c07b7fc8bb871e6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetCondition = 116`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L529) | 当前常量或枚举值：`116`。 | — |
| <a id="api-bfda36cdd2d2b3b8"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicIndexBuffer = 127`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L540) | 当前常量或枚举值：`127`。 | — |
| <a id="api-1b5ff9bd1f77f286"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicVertexBuffer = 131`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L544) | 当前常量或枚举值：`131`。 | — |
| <a id="api-713367ef8a81ce14"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicVertexBufferWithLayout = 132`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L545) | 当前常量或枚举值：`132`。 | — |
| <a id="api-1458c391029bffcf"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetImage = 151`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L564) | 当前常量或枚举值：`151`。 | — |
| <a id="api-2360fdadbfeda9d3"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetIndexBuffer = 126`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L539) | 当前常量或枚举值：`126`。 | — |
| <a id="api-1571c750bcefcf24"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceCount = 139`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L552) | 当前常量或枚举值：`139`。 | — |
| <a id="api-d1c77e6ad2829c20"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataBuffer = 136`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L549) | 当前常量或枚举值：`136`。 | — |
| <a id="api-ec6c827052145565"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataFromDynamicVertexBuffer = 138`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L551) | 当前常量或枚举值：`138`。 | — |
| <a id="api-bf337dd0e43b7807"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataFromVertexBuffer = 137`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L550) | 当前常量或枚举值：`137`。 | — |
| <a id="api-d57f7926986d7b7e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetMarker = 114`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L527) | 当前常量或枚举值：`114`。 | — |
| <a id="api-7b151ea647630f9e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetScissor = 118`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L531) | 当前常量或枚举值：`118`。 | — |
| <a id="api-7eb6dc68ac2e5e61"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetScissorCached = 119`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L532) | 当前常量或枚举值：`119`。 | — |
| <a id="api-83949a8392834e56"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetState = 115`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L528) | 当前常量或枚举值：`115`。 | — |
| <a id="api-a824258758a052d1"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetStencil = 117`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L530) | 当前常量或枚举值：`117`。 | — |
| <a id="api-7ea79ddb2b509922"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTexture = 140`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L553) | 当前常量或枚举值：`140`。 | — |
| <a id="api-df711de2e60a1d3e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransform = 120`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L533) | 当前常量或枚举值：`120`。 | — |
| <a id="api-b230614d454ef471"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransformCached = 121`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L534) | 当前常量或枚举值：`121`。 | — |
| <a id="api-77300260d257b46c"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientIndexBuffer = 128`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L541) | 当前常量或枚举值：`128`。 | — |
| <a id="api-af5fa62a951ea327"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientVertexBuffer = 133`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L546) | 当前常量或枚举值：`133`。 | — |
| <a id="api-744ecb59bf36dc6f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientVertexBufferWithLayout = 134`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L547) | 当前常量或枚举值：`134`。 | — |
| <a id="api-dbdeb64aeae63635"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetUniform = 123`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L536) | 当前常量或枚举值：`123`。 | — |
| <a id="api-4490644ef94ca750"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexBuffer = 129`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L542) | 当前常量或枚举值：`129`。 | — |
| <a id="api-da79919fe8e389a7"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexBufferWithLayout = 130`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L543) | 当前常量或枚举值：`130`。 | — |
| <a id="api-6cbf73d7c97381ef"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexCount = 135`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L548) | 当前常量或枚举值：`135`。 | — |
| <a id="api-5e2967fbb80ec47b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmit = 142`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L555) | 当前常量或枚举值：`142`。 | — |
| <a id="api-3f4673865046f4fe"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitIndirect = 144`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L557) | 当前常量或枚举值：`144`。 | — |
| <a id="api-a442c6f19f628d82"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitIndirectCount = 145`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L558) | 当前常量或枚举值：`145`。 | — |
| <a id="api-bd699dffd96d210b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitOcclusionQuery = 143`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L556) | 当前常量或枚举值：`143`。 | — |
| <a id="api-f8d0e137480340a2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderTouch = 141`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L554) | 当前常量或枚举值：`141`。 | — |
| <a id="api-219c90dd8a9e665b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Frame = 22`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L435) | 当前常量或枚举值：`22`。 | — |
| <a id="api-0fa144662ec87291"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetAvailInstanceDataBuffer = 53`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L466) | 当前常量或枚举值：`53`。 | — |
| <a id="api-f1b2347f877369d6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetAvailTransientIndexBuffer = 51`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L464) | 当前常量或枚举值：`51`。 | — |
| <a id="api-37596f0bccef83cd"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetAvailTransientVertexBuffer = 52`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L465) | 当前常量或枚举值：`52`。 | — |
| <a id="api-5ed8d7007cb040b4"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetCaps = 24`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L437) | 当前常量或枚举值：`24`。 | — |
| <a id="api-0331963a4cc501c2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetDirectAccessPtr = 80`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L493) | 当前常量或枚举值：`80`。 | — |
| <a id="api-e50eff5f98f100a2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetInternalData = 159`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L572) | 当前常量或枚举值：`159`。 | — |
| <a id="api-e9d8644167d62655"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetRendererName = 17`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L430) | 当前常量或枚举值：`17`。 | — |
| <a id="api-a334b4fd24fadd3b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetRendererType = 23`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L436) | 当前常量或枚举值：`23`。 | — |
| <a id="api-56d3524161975d83"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetResult = 95`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L508) | 当前常量或枚举值：`95`。 | — |
| <a id="api-cb5f068742771e62"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetShaderUniforms = 61`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L474) | 当前常量或枚举值：`61`。 | — |
| <a id="api-6c426f93ba8a59b1"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetStats = 25`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L438) | 当前常量或枚举值：`25`。 | — |
| <a id="api-5d6091c2743280f1"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetSupportedRenderers = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L429) | 当前常量或枚举值：`16`。 | — |
| <a id="api-80f97ddd94b88472"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetTexture = 88`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L501) | 当前常量或枚举值：`88`。 | — |
| <a id="api-2420b5b51305f3fa"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetUniformInfo = 92`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L505) | 当前常量或枚举值：`92`。 | — |
| <a id="api-81209b1c071d2aa7"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Init = 19`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L432) | 当前常量或枚举值：`19`。 | — |
| <a id="api-1f0d4ea55c3e7716"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.InitCtor = 18`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L431) | 当前常量或枚举值：`18`。 | — |
| <a id="api-e93f8a21c7547a4b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.IsFrameBufferValid = 68`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L481) | 当前常量或枚举值：`68`。 | — |
| <a id="api-2e1d7ed7a7450cda"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.IsTextureValid = 67`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L480) | 当前常量或枚举值：`67`。 | — |
| <a id="api-7d84423f94602183"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.MakeRef = 28`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L441) | 当前常量或枚举值：`28`。 | — |
| <a id="api-262371678afd4d84"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.MakeRefRelease = 29`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L442) | 当前常量或枚举值：`29`。 | — |
| <a id="api-fa8afd6f88d7c6bc"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.OverrideInternalTexture = 161`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L574) | 当前常量或枚举值：`161`。 | — |
| <a id="api-012f8234d7ad82d2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.OverrideInternalTexturePtr = 160`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L573) | 当前常量或枚举值：`160`。 | — |
| <a id="api-c1f92897042ea64e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.ReadTexture = 78`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L491) | 当前常量或枚举值：`78`。 | — |
| <a id="api-1556e8a28c320faa"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.RenderFrame = 157`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L570) | 当前常量或枚举值：`157`。 | — |
| <a id="api-0831b474e73a2601"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.RequestScreenShot = 156`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L569) | 当前常量或枚举值：`156`。 | — |
| <a id="api-3792a6514dd32b86"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Reset = 21`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L434) | 当前常量或枚举值：`21`。 | — |
| <a id="api-0a4190fda702185a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.ResetView = 111`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L524) | 当前常量或枚举值：`111`。 | — |
| <a id="api-1118fd9eef9d9c57"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeDynamicIndexBuffer = 194`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L607) | 当前常量或枚举值：`194`。 | — |
| <a id="api-53169b881589ae6a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeDynamicVertexBuffer = 195`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L608) | 当前常量或枚举值：`195`。 | — |
| <a id="api-ed38c5b76e5563b5"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeIndexBuffer = 192`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L605) | 当前常量或枚举值：`192`。 | — |
| <a id="api-7c52b4218158594b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeIndirectBuffer = 196`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L609) | 当前常量或枚举值：`196`。 | — |
| <a id="api-658d89676ba9d858"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeVertexBuffer = 193`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L606) | 当前常量或枚举值：`193`。 | — |
| <a id="api-eef3c277c746764f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetCondition = 164`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L577) | 当前常量或枚举值：`164`。 | — |
| <a id="api-6e8c62ac326ba8b6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetDebug = 30`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L443) | 当前常量或枚举值：`30`。 | — |
| <a id="api-88cde696c2e453a0"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetDynamicIndexBuffer = 173`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L586) | 当前常量或枚举值：`173`。 | — |
| <a id="api-d29f969bb8104089"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetDynamicVertexBuffer = 177`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L590) | 当前常量或枚举值：`177`。 | — |
| <a id="api-a1f477396d0c06d8"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetDynamicVertexBufferWithLayout = 178`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L591) | 当前常量或枚举值：`178`。 | — |
| <a id="api-1f4764531827b0d3"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetFrameBufferName = 87`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L500) | 当前常量或枚举值：`87`。 | — |
| <a id="api-7c5d29d2019d06eb"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetFrameUniform = 125`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L538) | 当前常量或枚举值：`125`。 | — |
| <a id="api-c8a21a16346bb2bb"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetImage = 197`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L610) | 当前常量或枚举值：`197`。 | — |
| <a id="api-1c51c8013fb3b35d"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetIndexBuffer = 172`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L585) | 当前常量或枚举值：`172`。 | — |
| <a id="api-ef7a2d5c7f493322"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetIndexBufferName = 36`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L449) | 当前常量或枚举值：`36`。 | — |
| <a id="api-1b3294bd31cad15e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetInstanceCount = 185`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L598) | 当前常量或枚举值：`185`。 | — |
| <a id="api-90836fb9f3933651"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetInstanceDataBuffer = 182`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L595) | 当前常量或枚举值：`182`。 | — |
| <a id="api-a3c3d3ce9dd5c7ae"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetInstanceDataFromDynamicVertexBuffer = 184`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L597) | 当前常量或枚举值：`184`。 | — |
| <a id="api-790cdf0bdedd3705"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetInstanceDataFromVertexBuffer = 183`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L596) | 当前常量或枚举值：`183`。 | — |
| <a id="api-85378458f74fcc4a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetMarker = 162`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L575) | 当前常量或枚举值：`162`。 | — |
| <a id="api-da2734039b4fa551"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetPaletteColor = 97`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L510) | 当前常量或枚举值：`97`。 | — |
| <a id="api-464da20f1fdc7a77"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetPaletteColorRgba32F = 98`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L511) | 当前常量或枚举值：`98`。 | — |
| <a id="api-c0b4cfa24817e1b1"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetPaletteColorRgba8 = 99`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L512) | 当前常量或枚举值：`99`。 | — |
| <a id="api-de6b71a7a02481ab"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetPlatformData = 158`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L571) | 当前常量或枚举值：`158`。 | — |
| <a id="api-5124012ed76aef2a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetScissor = 166`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L579) | 当前常量或枚举值：`166`。 | — |
| <a id="api-4a2450eaba30a415"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetScissorCached = 167`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L580) | 当前常量或枚举值：`167`。 | — |
| <a id="api-1c520b55b90ce365"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetShaderName = 62`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L475) | 当前常量或枚举值：`62`。 | — |
| <a id="api-0bf8f7a50cdaa7b0"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetState = 163`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L576) | 当前常量或枚举值：`163`。 | — |
| <a id="api-ca027328bae24658"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetStencil = 165`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L578) | 当前常量或枚举值：`165`。 | — |
| <a id="api-01bd12533d0a0431"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTexture = 186`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L599) | 当前常量或枚举值：`186`。 | — |
| <a id="api-f65962b4c262a9bf"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTextureName = 79`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L492) | 当前常量或枚举值：`79`。 | — |
| <a id="api-38b337ae07dc15c8"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTransform = 168`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L581) | 当前常量或枚举值：`168`。 | — |
| <a id="api-d922513373e51c5e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTransformCached = 169`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L582) | 当前常量或枚举值：`169`。 | — |
| <a id="api-888cb763a2b98a4f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTransientIndexBuffer = 174`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L587) | 当前常量或枚举值：`174`。 | — |
| <a id="api-75824264f5fc0636"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTransientVertexBuffer = 179`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L592) | 当前常量或枚举值：`179`。 | — |
| <a id="api-eaf838260327a169"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTransientVertexBufferWithLayout = 180`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L593) | 当前常量或枚举值：`180`。 | — |
| <a id="api-7ef6d59d9f62d24b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetUniform = 171`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L584) | 当前常量或枚举值：`171`。 | — |
| <a id="api-0a300170d815f8d1"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetVertexBuffer = 175`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L588) | 当前常量或枚举值：`175`。 | — |
| <a id="api-fa227336d7d7d031"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetVertexBufferName = 41`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L454) | 当前常量或枚举值：`41`。 | — |
| <a id="api-9d8ad661efdea099"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetVertexBufferWithLayout = 176`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L589) | 当前常量或枚举值：`176`。 | — |
| <a id="api-eece679bcc30c36d"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetVertexCount = 181`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L594) | 当前常量或枚举值：`181`。 | — |
| <a id="api-80a8c187c29e75b3"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewClear = 104`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L517) | 当前常量或枚举值：`104`。 | — |
| <a id="api-0e6588e56cdf95e3"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewClearMrt = 105`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L518) | 当前常量或枚举值：`105`。 | — |
| <a id="api-5f145ee612f659db"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewFrameBuffer = 107`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L520) | 当前常量或枚举值：`107`。 | — |
| <a id="api-bb54acee91e4e59a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewMode = 106`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L519) | 当前常量或枚举值：`106`。 | — |
| <a id="api-e6f96cccf7249700"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewName = 100`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L513) | 当前常量或枚举值：`100`。 | — |
| <a id="api-e2293b9364931688"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewOrder = 109`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L522) | 当前常量或枚举值：`109`。 | — |
| <a id="api-660c40d80993549a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewRect = 101`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L514) | 当前常量或枚举值：`101`。 | — |
| <a id="api-07b32971c6115753"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewRectRatio = 102`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L515) | 当前常量或枚举值：`102`。 | — |
| <a id="api-61a1866ed6c0eafb"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewScissor = 103`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L516) | 当前常量或枚举值：`103`。 | — |
| <a id="api-b8c6310e03427e7f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewShadingRate = 110`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L523) | 当前常量或枚举值：`110`。 | — |
| <a id="api-e43d4b121851d24f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewTransform = 108`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L521) | 当前常量或枚举值：`108`。 | — |
| <a id="api-a82732e0b57394bd"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewUniform = 124`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L537) | 当前常量或枚举值：`124`。 | — |
| <a id="api-06383e54f82e5248"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Shutdown = 20`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L433) | 当前常量或枚举值：`20`。 | — |
| <a id="api-ded6a79dcefe6394"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Submit = 188`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L601) | 当前常量或枚举值：`188`。 | — |
| <a id="api-436320ab1fda6c8a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SubmitIndirect = 190`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L603) | 当前常量或枚举值：`190`。 | — |
| <a id="api-ab10afa5ddc5b351"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SubmitIndirectCount = 191`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L604) | 当前常量或枚举值：`191`。 | — |
| <a id="api-1c9aa9eff538352c"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SubmitOcclusionQuery = 189`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L602) | 当前常量或枚举值：`189`。 | — |
| <a id="api-cbe38f953a614c87"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.TopologyConvert = 14`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L427) | 当前常量或枚举值：`14`。 | — |
| <a id="api-288b408ced25b261"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.TopologySortTriList = 15`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L428) | 当前常量或枚举值：`15`。 | — |
| <a id="api-070a096e0c8a1ec0"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Touch = 187`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L600) | 当前常量或枚举值：`187`。 | — |
| <a id="api-746f993da1a37217"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.UpdateDynamicIndexBuffer = 45`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L458) | 当前常量或枚举值：`45`。 | — |
| <a id="api-5c9c5be492e3e825"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.UpdateDynamicVertexBuffer = 49`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L462) | 当前常量或枚举值：`49`。 | — |
| <a id="api-8ca2bc9a14edee0d"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.UpdateTexture2D = 75`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L488) | 当前常量或枚举值：`75`。 | — |
| <a id="api-fc4a872113294371"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.UpdateTexture3D = 76`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L489) | 当前常量或枚举值：`76`。 | — |
| <a id="api-ccd39435be05a829"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.UpdateTextureCube = 77`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L490) | 当前常量或枚举值：`77`。 | — |
| <a id="api-39decbaed222b641"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexConvert = 12`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L425) | 当前常量或枚举值：`12`。 | — |
| <a id="api-0d5c469773f95849"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutAdd = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L415) | 当前常量或枚举值：`2`。 | — |
| <a id="api-5b17a763e9193c7f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutBegin = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L414) | 当前常量或枚举值：`1`。 | — |
| <a id="api-3290fb29e300e837"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutDecode = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L416) | 当前常量或枚举值：`3`。 | — |
| <a id="api-10a8e4b4437acc92"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutEnd = 6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L419) | 当前常量或枚举值：`6`。 | — |
| <a id="api-f423df1951189609"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutGetOffset = 7`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L420) | 当前常量或枚举值：`7`。 | — |
| <a id="api-11baf07b0373429b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutGetSize = 9`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L422) | 当前常量或枚举值：`9`。 | — |
| <a id="api-0a90ff39579a7166"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutGetStride = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L421) | 当前常量或枚举值：`8`。 | — |
| <a id="api-3e37befe658287e2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutHas = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L417) | 当前常量或枚举值：`4`。 | — |
| <a id="api-76f8645c2ccfd28c"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutSkip = 5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L418) | 当前常量或枚举值：`5`。 | — |
| <a id="api-0d6acecfdd1739cd"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexPack = 10`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L423) | 当前常量或枚举值：`10`。 | — |
| <a id="api-aa4a0b487525f41d"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexUnpack = 11`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L424) | 当前常量或枚举值：`11`。 | — |
| <a id="api-a75605a761a32659"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.WeldVertices = 13`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L426) | 当前常量或枚举值：`13`。 | — |
| <a id="api-a8b1c47523c4a54a"></a>[`public enum Inno.Native.Bgfx.bgfx.FunctionId : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L411) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.IndexBufferHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.IndexBufferHandle c3e33a081d5cf3c18ad683c60787085a6d1ac1f3985487b6ae55c87f0e73ecc5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.IndexBufferHandle.idx 440f6f92422dcf709dcb952ee2874e9411e732992410b30aed1b9b4afaf6abec -->
<!-- api: P:Inno.Native.Bgfx.bgfx.IndexBufferHandle.Valid 5cc01bda6095bc00aafec850318dd894052090be2cec90edbab9c96ea608708e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.IndexBufferHandle.#ctor(System.UInt16) eab9d2838fb422589641b6e820935f590ce782423e897f9102f2a9ee50d1df90 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-603eab0ed1fa46f8"></a>[`public ushort Inno.Native.Bgfx.bgfx.IndexBufferHandle.idx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1997) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01f3e20f2406d2be"></a>[`public Inno.Native.Bgfx.bgfx.IndexBufferHandle.IndexBufferHandle(ushort idx = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2000) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d93ab701920ae97c"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.IndexBufferHandle.Valid { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1998) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a3cc4004437eb6db"></a>[`public struct Inno.Native.Bgfx.bgfx.IndexBufferHandle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1995) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.IndirectBufferHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.IndirectBufferHandle 6cb5fce22bfad50e76586fd0a92c9b9e10bbc5bc68936be598cd019d6f3f0310 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.IndirectBufferHandle.idx 508964712f07c3c666ee4903b9421c0911af444cf62fafacee1bd75b275e17d8 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.IndirectBufferHandle.Valid 14de3fc38aed5e551c172bde0a5776b1b67fa30a960b02b0b3e8017794139899 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.IndirectBufferHandle.#ctor(System.UInt16) d6845fbdb942594f6be4452d54f3bec4f68be8a0fcf7524d91b45dfd107f669f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-35973d0af3d20a2b"></a>[`public ushort Inno.Native.Bgfx.bgfx.IndirectBufferHandle.idx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2163) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-67a729f8d3be6d2b"></a>[`public Inno.Native.Bgfx.bgfx.IndirectBufferHandle.IndirectBufferHandle(ushort idx = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2166) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-32f6878a66e38162"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.IndirectBufferHandle.Valid { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2164) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c560030cfd307da9"></a>[`public struct Inno.Native.Bgfx.bgfx.IndirectBufferHandle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2161) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.Init</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.Init 019a361dac17cef6f662b117acb1d4a1ecabe0edaa135660431dfb9d1c0771a0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Init.type d70fb2a66065cf9c2ee19206dd7cb1601cbb484f82ccccb5228e601b14a5dbe3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Init.vendorId fdb4ec3d0b321d326d2eb4fa68b79acd67e0895e37684c8e2fe38d46f893c680 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Init.deviceId 02f086e06f93c6f2cc5db382b72f0eaa822c0c3de6237cef01445d99d5c722f2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Init.capabilities ebe22a8bbc9a65ca237ecb4f180c7550da5f523043de8b861c941b1d3dc51b30 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Init.debug c153eefd8c32086fe9fb065684b851d0af349a5ee7d9f0739d8cd076b57bc1a8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Init.profile 1a041511389003b73110640d9f6225085aa5171cd2be5c8a690a1034131e2be2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Init.fallback 155b4a911ffecc500fca75da4a0c2d00edd6a6015eb6dc0e4c08ba5ea88452d8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Init.platformData ea635d08fb4aa902904305ca2b7081afd9713afcb962780408e5cf24690da9ab -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Init.resolution e3cb9dbc7e735c8b8a43c0ed64920fcdf65ba7b7049e1554d2fe73a0d812245b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Init.limits a5569bd04aa3c932e3a1a9020a2e4034122da13096c144d90124a9b541ba1bf2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Init.callback 17abf02f2bbc07be9b451b7da765327b8d13a113578455add7d2ed7ed596a6be -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Init.allocator 657ceb96db25650812c68b2fc5df14b41eafa22d1055aa81636a85fc39151bd5 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Init.#ctor(Inno.Native.Bgfx.bgfx.RendererType,System.UInt16,System.UInt16,System.UInt64,System.Byte,System.Byte,System.Byte,Inno.Native.Bgfx.bgfx.PlatformData,Inno.Native.Bgfx.bgfx.Resolution,Inno.Native.Bgfx.bgfx.InitLimits,Inno.Native.Bgfx.bgfx.CallbackInterface*,Inno.Native.Bgfx.bgfx.AllocatorInterface*) 03ea5d4a7862acc6910934662d5506ad5ceb03580421d12c89669161c1f67397 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dd3dacb1c322891a"></a>[`public Inno.Native.Bgfx.bgfx.AllocatorInterface* Inno.Native.Bgfx.bgfx.Init.allocator`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L951) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c64fad071aeeeb98"></a>[`public Inno.Native.Bgfx.bgfx.CallbackInterface* Inno.Native.Bgfx.bgfx.Init.callback`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L950) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7d31d607799bb3d5"></a>[`public ulong Inno.Native.Bgfx.bgfx.Init.capabilities`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L943) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ebaa67df12b37ddc"></a>[`public byte Inno.Native.Bgfx.bgfx.Init.debug`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L944) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b99b56425c4feba0"></a>[`public ushort Inno.Native.Bgfx.bgfx.Init.deviceId`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L942) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-849b428e7b035f83"></a>[`public byte Inno.Native.Bgfx.bgfx.Init.fallback`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L946) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39e03e44e9685db1"></a>[`public Inno.Native.Bgfx.bgfx.InitLimits Inno.Native.Bgfx.bgfx.Init.limits`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L949) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b0e30343f00255a"></a>[`public Inno.Native.Bgfx.bgfx.PlatformData Inno.Native.Bgfx.bgfx.Init.platformData`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L947) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9af81f0e727658cb"></a>[`public byte Inno.Native.Bgfx.bgfx.Init.profile`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L945) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9ad49dbc3d7b351"></a>[`public Inno.Native.Bgfx.bgfx.Resolution Inno.Native.Bgfx.bgfx.Init.resolution`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L948) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fa47bc0d47cbeec3"></a>[`public Inno.Native.Bgfx.bgfx.RendererType Inno.Native.Bgfx.bgfx.Init.type`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L940) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9a67b55da7812be7"></a>[`public ushort Inno.Native.Bgfx.bgfx.Init.vendorId`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L941) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a1e7e84304b029c6"></a>[`public Inno.Native.Bgfx.bgfx.Init.Init(Inno.Native.Bgfx.bgfx.RendererType type = Inno.Native.Bgfx.bgfx.RendererType.Noop, ushort vendorId = 0, ushort deviceId = 0, ulong capabilities = 0, byte debug = 0, byte profile = 0, byte fallback = 0, Inno.Native.Bgfx.bgfx.PlatformData platformData = default(Inno.Native.Bgfx.bgfx.PlatformData), Inno.Native.Bgfx.bgfx.Resolution resolution = default(Inno.Native.Bgfx.bgfx.Resolution), Inno.Native.Bgfx.bgfx.InitLimits limits = default(Inno.Native.Bgfx.bgfx.InitLimits), Inno.Native.Bgfx.bgfx.CallbackInterface* callback = null, Inno.Native.Bgfx.bgfx.AllocatorInterface* allocator = null)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L952) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-632093c41c40396e"></a>[`public struct Inno.Native.Bgfx.bgfx.Init`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L938) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.InitLimits</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.InitLimits 7ac5c4b02dc9e70c7ca8d986535c1cba3db18f499a37801d0e5b3238308be9da -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InitLimits.maxEncoders 7a80d50aa9b81ab039e16389185d065820f0b0aaa5033e663c00a26750cc6bab -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InitLimits.minResourceCbSize 13b551fb298b24c3ede01e6d9d5880981bbed67a5699a8e483aeed4268f1be5a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InitLimits.maxTransientVbSize 032ea8c29b55ace1fbaebb37420627fc46e5f9a40eaf0344a0095196c13c5df4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InitLimits.maxTransientIbSize c76a8c5d33606626a0c76e8f5a440e5ea26400d73b501b0c667477997696cebe -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InitLimits.minUniformBufferSize 95a2fc8138e679420ba631bd8ea9b538a4921737d0f3818c21fd69ad3049f520 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.InitLimits.#ctor(System.UInt16,System.UInt32,System.UInt32,System.UInt32,System.UInt32) b4013b955e888bf222770a5f72686d763d82dbf0046ecf57e1de41566a761b3d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d709cd870c174ae1"></a>[`public ushort Inno.Native.Bgfx.bgfx.InitLimits.maxEncoders`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L862) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7bd09dfc5a8e7609"></a>[`public uint Inno.Native.Bgfx.bgfx.InitLimits.maxTransientIbSize`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L865) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f4b43fd3755d184"></a>[`public uint Inno.Native.Bgfx.bgfx.InitLimits.maxTransientVbSize`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L864) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-04db8614f4c5f4b7"></a>[`public uint Inno.Native.Bgfx.bgfx.InitLimits.minResourceCbSize`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L863) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-92e69abfdfe586f0"></a>[`public uint Inno.Native.Bgfx.bgfx.InitLimits.minUniformBufferSize`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L866) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1606eb7990fb19af"></a>[`public Inno.Native.Bgfx.bgfx.InitLimits.InitLimits(ushort maxEncoders = 0, uint minResourceCbSize = 0, uint maxTransientVbSize = 0, uint maxTransientIbSize = 0, uint minUniformBufferSize = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L867) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1f55c8d419b749bb"></a>[`public struct Inno.Native.Bgfx.bgfx.InitLimits`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L860) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.InstanceDataBuffer</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.InstanceDataBuffer 13e1a97e5b3524c9a42625860c4186bfbf22ddbcb7559efeb6c3dc677735025b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InstanceDataBuffer.data 33f76e16075806d07097cd2f5a8d6a5c1ac17a26c3430c3f477ed3ba6fc0e286 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InstanceDataBuffer.size e9b4efa0746a3c04224a001a3b3c2c3f6f4c4479cf2fd43f1946df5166de72c5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InstanceDataBuffer.offset e05a5c8813b0b7f3bda3fc4c9c8dd6dd4271ce3d7761e0b65ecc891bbf3ac23c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InstanceDataBuffer.num e2cc14ab3fff6f35d9e2f4aa7323e40ba904d8087250f2e05dd30c6c2146abd0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InstanceDataBuffer.stride d63bd4003e3fc5534e75d3d78bf0a64348a3f95a27e518800efe9deb48b2c4f9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InstanceDataBuffer.handle e5771c2f0eaae802edacff0b6fa25173634aaaf05dc7ccf96619d66761b6e4a4 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.InstanceDataBuffer.#ctor(System.Byte*,System.UInt32,System.UInt32,System.UInt32,System.UInt16,Inno.Native.Bgfx.bgfx.VertexBufferHandle) d60dde23b7e1695e9629e4f917b30132039cecfa79ce3ece58d34690964c8422 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ff930d619fc89e67"></a>[`public byte* Inno.Native.Bgfx.bgfx.InstanceDataBuffer.data`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2137) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-22f6284562072f56"></a>[`public Inno.Native.Bgfx.bgfx.VertexBufferHandle Inno.Native.Bgfx.bgfx.InstanceDataBuffer.handle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2142) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d550c33cf98efca9"></a>[`public uint Inno.Native.Bgfx.bgfx.InstanceDataBuffer.num`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2140) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5b76783d90bac44b"></a>[`public uint Inno.Native.Bgfx.bgfx.InstanceDataBuffer.offset`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2139) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61cf0ea6e6d021f0"></a>[`public uint Inno.Native.Bgfx.bgfx.InstanceDataBuffer.size`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2138) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-afff9a9e3196f5c6"></a>[`public ushort Inno.Native.Bgfx.bgfx.InstanceDataBuffer.stride`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2141) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed4a62e89f1ec604"></a>[`public Inno.Native.Bgfx.bgfx.InstanceDataBuffer.InstanceDataBuffer(byte* data = null, uint size = 0, uint offset = 0, uint num = 0, ushort stride = 0, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle = default(Inno.Native.Bgfx.bgfx.VertexBufferHandle))`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2143) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0ede8a296b59edff"></a>[`public struct Inno.Native.Bgfx.bgfx.InstanceDataBuffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2135) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.InterfaceVtbl</code> · 204 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.InterfaceVtbl 4162c38b5bc3ff8e049fbd31b1c19c84ca8b495f456dfc093a15c543f39272da -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.attachment_init f1121106847e09d296c77f98c2b1f2478a75b28bcf2e0678598ff6e0b8651f36 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_begin c7c49f1a42cee66701fe3071a7da747abe8dbe8090abd173ccaa3c258a5923da -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_add 28307b324ef9c823eef1a0898f7e21424ecb5eddb81b8fba967614ab36bcb7d4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_decode 21122a0599001ab78afa0adeb73563e58961b4732eb3a19d6e279c38d56305a5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_has 654e68c79575f7579990b741692b5de348e9f39949426a010541d5f367f50b80 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_skip 9c74743d19fea698c5c8494d0e8eef7b9398c0ba271083b7f47e00ca0851c9b0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_end 77623f6b8b5fd01047c27ab21da2bfadbd99d106f6b186a0f5db8d57402816c4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_get_offset 0dee68b3521fc087a63caad085b10bc3a27c2521fd274ed75645a8da9d23432b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_get_stride 93da96a2c0a5aa77d2302642f117ae96e17bdbfa762df82ad6ca7dc306a77e63 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_get_size 878ec80fb329e428afa295992b38a2ba1864726d0b9867dd50eb7355b6ee2160 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_pack 334801c18a2fef9f5edaf588214b4a9f46f4cdab7db44805071c6ab4d47c5118 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_unpack fef80be6540a66bac346966333017a0893cf621bc71dec660edb1e1250ecae6b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_convert 5309bb83e12c78998afa2d3af890b84e043385d757016071e649e9ebdf19e42a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.weld_vertices 9a0cbd13f9da32e09ef0ab49e1409e5c0326c0c77f903fee3e0202e3e78139d0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.topology_convert 6e386e66e8dc75c17b98fc4173c4def6c487e2b2193ff7ff7303edd073bd7f05 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.topology_sort_tri_list 2f571149c2474d46a79306d08316f2a857d7fd279f92d3a2964173aa8f22d8d9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_supported_renderers d66a00f9ca7f9a727825b90aa0400f8d5979ed8cddc1c1dc52ff8262b02eeee8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_renderer_name 0d109c2327a69410cbed8d3e19d972215ac8e8edace4b06a144de05b022de8a3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.init_ctor 6ab137f55d7ba52042089d6956d9b7e2b8b18aa4028a00ba8323a6b2c3c30f6d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.init a514c8fea439f0c3759da09961cb51af9692889de2829aaa3c4c56fe2f033469 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.shutdown 334189512bb433b20a66a192879f283389d6c786a09328e648f03c2fa20cfd92 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.reset 4ca2a46387254c790bd41cffe73208768808fdbc571a232060c8e48a4b1f5849 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.frame 26b8932e0edef358458a1358cb817b27165645e85e2a350a7d7cadb8262d3ada -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_renderer_type 772608cc07eb8f3046fb0ac39ff711523612172d55cc0d09431e52b32557e20f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_caps 1ad3408acf4ec7bd534902122884250b6cf2c979cfd3209832f6518bbad7abd7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_stats 9dbc4889dfbb05effccc49e9be2f7d8eff6db28be9ec618b2b944cc2ff40defb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc 122dd1ac4259c21c82dbd57cd9e569c72422d4d7079aa27a9facb25e05409288 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.copy 26cf257d75fdafddacca6227b9ccfa7f727deb6c34e9d33f8d58681348747d1a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.make_ref 3d6dc8310291ec9d0adb68e5f7e9dedf8ececcd7cee9813e60a46497b8dffef6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.make_ref_release b5b8179b4a60411d2661c71992b35a9939e63232db92a4f6242cd8fc835074de -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_debug 4233bc219ee02b8d92c84905b11c3f25930d04702691a77b87e9fdfcd51d31ca -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.dbg_text_clear ae95e597aad921c798d3ea8da52d7033a082bac18e34f7d9501df6ce98266ad2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.dbg_text_printf bafb86b46c61464f2c54c8c74cacb62d1a783f6858c052276e88ce509ff8574a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.dbg_text_vprintf 77ad50a16a371e2b541d35dce685e3b09310830a99ebbc6b503773b4142c6ebf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.dbg_text_image 7556a962c00a485a5e8f8a6d6704958a9f6cb36608e218127dcb0ad3ea4ce514 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_index_buffer 3e59a9461e14bc11a83b6ca99abd0cee096d09a67d7dc93f5ff33541a7d8e87e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_index_buffer_name e0ad0e8b7c09e0476f5d574e023505d8ab5ceeace4ab471705d1a7316897a7a7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_index_buffer e825d901a396ce6c03c0cca69c31278bfeb64586bbc90adeffe20f41daac6f50 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_vertex_layout fba715c3389f89869adb747daa4f5cf5a4c16bb1567d219fd4dbcd4e53296778 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_vertex_layout 9f88698c8c7c515a8e136da8b79cad39621c6141a7fa7ee03ee0ba2f212433a7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_vertex_buffer 965f45dfa66e643db39935b1d5dfabec8ddb505bc7e497c7629f7b341c331cd9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_vertex_buffer_name 176e95a896c5799d544ea9ab70874e4d04a0283cf96a4b16c3bf43c5d975f845 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_vertex_buffer 964206c93bad80a56285ca98bb5808ee0e3c62682b9274956053167449d181ce -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_dynamic_index_buffer 32e5d2ed320bc010229849185f19189bdd2ce70a8e01c14538bb2f8083d0a7a3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_dynamic_index_buffer_mem 7dbb9c825c31f34b6c0513ec0da384b66736fcd9270656abda05700b71c869e8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_dynamic_index_buffer 2651dbdfb60c1cc19fc788baa5971dcca76c801881f58df829b305f19f906cbd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_dynamic_index_buffer 1298ee24da2d41363ab0afc49d04f815b303013a73e8b97af0f0267ca8eca40d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_dynamic_vertex_buffer e7e5dbcb3c99ba1afc930f3cc175848ffbdc97ba37694a79573db8e25aa8dbcf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_dynamic_vertex_buffer_mem 05a16d31b37db7c87099a26c850186dcb16288bec3e898f3c2f294a18d16ec26 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_dynamic_vertex_buffer 967703f013725b596e72754338ab59bb2e01db247c41fb028b4b5741dd52e7b3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_dynamic_vertex_buffer d8446d82711b3792c2470e4a3e3bb9adb12456e46507f76af80b83302f60ecb8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_avail_transient_index_buffer 92765ad97dad714c477c7042f9711230a9b7a1a6041d0aa4c3a173c5f14d027e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_avail_transient_vertex_buffer 3ae58f19aefb19a4f632f598d83794c74186a424eabd2b996e2cd1caec4dfe65 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_avail_instance_data_buffer d5d823e012073b70348eb55c35310e7896c7e76b8c84f1a2ec5e55946f25c20a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_transient_index_buffer 4383dca56a36e4904a1b063225ac9dae0f5a7b799fab030f0a4633bdb5b258dd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_transient_vertex_buffer 4dc8fe729e133a3f4f79390035e39f54f376b02b7e4a3e41750a9b02c948f2f2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_transient_buffers 81b0beebe51dd46d377935502cbaf266f72f78b02829a0b33d395ccfbece2530 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_instance_data_buffer d84162719b9ffb87dd8217124a6a3e9749ab83f3e9655b9716106d47bbdc8d17 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_indirect_buffer bab928363a62bb2adeaf6208b42789411ae2c79c0fe9e8daee803b63242bf129 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_indirect_buffer 312550556673150e60bad5760146f3e585bf3697fc9876a4fa93d462bf64fbad -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_shader 0e0e495b580a03bed78872039122c5ab8ad3b65f61192a976d1cf0c21d5c5306 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_shader_uniforms a3a21b8e858e69fb73713510e52dbe6b628fc5986dec61132c49276f7dd5caad -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_shader_name c543887b6c1f67e28e2c9359acf10a2cda1db751ceb7df152cee907f21244397 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_shader dff591ba1a7d620a960da3acf7442172cc31d970fa3c18fd598ba7fe496f30eb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_program 407daa18215593c3b87b48b9af1eef92786987ce079ca566b5fc9f69d3447c7e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_compute_program 487582a3341b90bb0edd03f73328d86a539a712285512206b938a2a32f69f451 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_program 92bb03748bc1db8834013bf9c5d820c674542702066478d81cf2647bd3505eba -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.is_texture_valid bfb9653ec641cd27d68bfc39ef2e924a7beb1c5195a9ac7c83b6cd7926a7ba05 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.is_frame_buffer_valid bee45e3384f8aed60f5a8aa83f5dacccdc32163b1f68ec140d1fa4b183874843 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.calc_texture_size 82552b5cd4503ee22f7e997bb988c5b882ee8338888a5a61c0369f688410adf2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture 3c11133a3f576ba1a362fef702b11429978f70ee30578d2a927894bb71d1e870 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture_2d 24d7b415c58b24434561d059c24add0a735bbd9918e8df9038b984d8612af195 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture_2d_scaled 0ba52ddc35b913e16b0db2c8de85d1be313dcfd53977ffba097bedc7aed9048d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture_3d d2950d442865a4997d60fe5de4d6f814ae0c649d676fa2caa4e866e771ed6526 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture_cube a12b99be37d4d798a39938801d28a90bf3c5accc06832b31b70a228af4ab8d9e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_texture_2d 5ae278aa1bc10a1ed3a1dd916ab3e6aeeffc46d7e53cf735bba6227dc3660bc0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_texture_3d 1b3bc64bfc4e0f08b093461bff41317dd1d9b630b53a18450ebeead702b52ec1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_texture_cube e512eb1845aeb2fc0ca7af22f93d8857651cd35e9ef33667b57601223c0f3b09 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.read_texture 01ebf4221b40201cc4c98729a63a75064add394e94717186d7a840fcc8009d24 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_texture_name 905cd86b957aae31b639fcaff4a67413e868fafdc40965d7b2ac0dcb039b314d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_direct_access_ptr d435053c9ae10e6f7f141b95e085de2e10907862f039d6497196c19ab41229e8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_texture 980396226485f32cb709b1b51a71c2f8ad170e36129d39e647252f4b1162851c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer a09f07c4598529e9336dcfb3917c14592dec25e2cf23c98c9f516144722f4bde -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer_scaled f30ab44312ef1a0e5bcc9c3f911af2dd2b6dae6fad7d81d1878fc8eba29109a8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer_from_handles d8d4401c93b54ee52b5a4712d40ca859f549ecf12de03f2758412f6632833ba9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer_from_attachment be83d7027747516dabbc2a47e8cf64dcd817ea26d1089d8040ec49ae91e8236d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer_from_nwh c9d174db1468e2a7a1cd4aa463545a7c21b4713aad2b551ec189ad77bd0d5d32 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_frame_buffer_name 5f4a32d33052ca4bb97e1254eb7d83398979da07cb36902d4b8675dc7c9336e3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_texture 241febc7a2ba5ac8e8d8dbe1b4faac96933065de12dabfd89a918d83f6914b6b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_frame_buffer d818fa49ac61a48d3313417b1225521e0845bbdb05173a9ebfab8dd30756328c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_uniform f2fa44caf85a97a165a38c1fd372457f9db41fd597c28b8039a8e7ab59e278da -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_uniform_with_freq aab02663d1e148d79338a374b754b991a7becba9dad11674a2154fa4daf16165 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_uniform_info d8a4db6f01ee4055c22bedb05ea7515992e10259339d77e20e9b37a936729c90 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_uniform 32c673c3ecb2dbb6faa733a3755f0fa221c7147c4b20833054e6cbe2b37e8b95 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_occlusion_query b12c99caaeb901d12b028cf037671c73c7fa524b896f157776753341d6cfddbe -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_result f3e48507933ca666cf69ce5da9501a095b27741702c528534e56d190fd232d6b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_occlusion_query 56de2e43209e976a5708ea010a55a370d9e4779c7ac0379d8823d13b2e32ed95 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_palette_color d9b5b8651b41afa82df2dd4c7aa6fc7e4c6f9dea72004190d11bfa646993a181 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_palette_color_rgba32f 7ac94fb515f412f8685c475f7dc6378c2c536b0b3a6b9c81fcdd85e028e2ce3f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_palette_color_rgba8 c8981487dfc438f57d3e8ba59a097144b8370f51df23984dd9e3bc8461566008 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_name 823caccdcd665df762dbb543f32faa2c0f0550a601accc3cf70f4e3cc61bee0e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_rect f12012d066156378563c1cac5abf17764b2e884ab1d26f8bb67e7855c2709aab -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_rect_ratio c1382856d7d7584ba0fdcca5b96bbc2e287ce9d45a83fb55e9e4555517e3c895 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_scissor e7f2253d5adfd6b629e3854fa5d2f6ce1d14493f304aa20481c128398e4d39f1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_clear e76cf2193fe4bc05d3ae0dd82a0592c3cb8613053a4fb44173b28b0a5175263b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_clear_mrt 4e95f144ba796e40c9497a67db4e78efbe1a57fc3f64e239f23ea6a48197ebe4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_mode b3072252e736ce1669b5620c62595e345b3068eac54a14b8e8e8da16240a5dd4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_frame_buffer 10ee51d49e7e30e278ebf9d6e6456a5cdc01cb381aa9b749f739f3374e7c7006 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_transform aa9529e25345b42c2c2d309be7fd7d948b066b67c02dc9d72d6f1c067ea83b31 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_order 395cba35f18a498298dc4732028f70b57516cf0a0d242465414a40c1811b4e2a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_shading_rate 23fcc2cb40a9d2f9cef30488aff1b1bfcc498684372c6b1c803714692e294aa5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.reset_view 2f1b37df19cf3aeabda3972d3b994e091ec528d5794439ad234d200a81d1bcb0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_begin e5f41eaadf7763f2eddaabbcc84a6321e2e9aa183e8600bdbfa0fbcf9187941a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_end 9f005be241d40d5d5bc3024a4fda4454fbae789196fd3c25562ca3d39d905c85 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_marker 94f2b0b702e53dda0977a94bf5af5b672a91218ddc826a04727cf08ce6d77a08 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_state 0194e16419b0614cf6e357fade84701032a00036697c612534bce2547981421c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_condition 2a39eb751a240434b2940e8cfa01f7378e54316fc7624a13811433d355caa334 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_stencil 28c1bc9b32e48bad4eefd62128b5762795b19f98439e7539c90b440ea2622f57 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_scissor 08b5cbff79758d706d83b823dfa34e134824939361539f100fd8046a56734851 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_scissor_cached 9d819b247f3dd76cd2148dcc218fcb8bde142cc39fe8be0be79873c6ae5b8eb5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transform 76a8a58dd359ae5d3766e1d6373b2975e92a6636f6443a04906700deb4169d86 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transform_cached 50c3aaa5007c700d58b280ff710fc4877289b903ba65d5c9eeb8b8b1092dd37c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_alloc_transform 68c99ee02eaa8ec2cdd80367d3bcfbe7c44daec72a38b594ad9d366743c4a19f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_uniform cf88bce79992b13ea5a9fe12c9ff6d74b6e46b6baedc42f7392c7e1c60f0c470 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_uniform f282a87a8bc546cfb7599d5f9d23e44fc25831c9b3b4df79331359036d68fee7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_frame_uniform ca57da9812a87b483ec2d8ed04395da9cd01e6b90df2057fbcb30c42538225db -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_index_buffer e22d062323ae4ba8083676a4429712a1fbe5e475dc447fea8b0528122759e71d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_dynamic_index_buffer 258c131ad5245aa0338f08142b14228aae7a2c86d6a294d121c0835b8e28c062 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transient_index_buffer e09f726864319993f56676b083f6cf77a3b8159c3542925865b09a2430747d86 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_vertex_buffer 3ff77ac1434633448c1324b46129abc5f680491251919e212e9d2cec1d13d5db -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_vertex_buffer_with_layout 623025c0b75a28037099be5e0ed1d9bb0e372c8d56445102c2b5da56deeb5424 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_dynamic_vertex_buffer b5637680e16b205c82d6807247d873d11194acfb6049634e40987b367ac6751b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_dynamic_vertex_buffer_with_layout e6d47873e2caedab23b2ee02cf4b2dc5f41b4323cd26be867f0a6b4544c87c98 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transient_vertex_buffer 549006986cd1677b81d8dc1c10dac086dcf8b880a74604281d9a4db7441e20f1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transient_vertex_buffer_with_layout 6b6dcbbc109b0cabfd6bfb9e2d766bbb0299789430f6ed341e598135551fecb3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_vertex_count 5722c045b719dae20720e43efb9791da2250049fa4f4be3bb6356339b418596e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_instance_data_buffer f5b608386dffc55630bcea3b1f06662814ad6545533c67bd77f6d7474ad9eb53 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_instance_data_from_vertex_buffer 5fa0ba31083184095ccc218b77f4fb6ee145bd760c4d9f3d4aa943d3ec1d22ce -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_instance_data_from_dynamic_vertex_buffer 7fc1cf7697305af6a12a77c1fcd2b4d34e1af70d7ccff2c2706642cf40c05f98 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_instance_count b4127a0fca46481b58b3a5d902b6868b515a1c10cd276197025a06c3f70c989b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_texture 49c79f1035e17e201e4f67abdea64ed66acc9b32b434b5dd97c4ea3b9aaa8233 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_touch a538a8ec7387acbdba9e7b8f01ddf72525ba01dea39672701f5ca6cf3d8fbd33 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_submit 070925bf599168f8aadccd53a9c3479c487f079a8b0e326eee7a81ec8aee221e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_submit_occlusion_query dd7c4c0ec39e2aa60683629bcbd582a0feecd947a93d802701e5c4d399de98f1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_submit_indirect 66c1ef09840311d370b8ce5fa567c4e8bf1d8c6be35541b18bfe3bdcee33709b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_submit_indirect_count 1e4ceb210b24cbebfcff155623b6bb0d02ab3e00f67c3610486a7c636223b79b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_index_buffer 4802c4b394244cdfb492697eab1590810192997cbab6cf745baa3b674b510251 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_vertex_buffer 0c48282adb0a89d5cacb9681b068716b1cd7ad5f3ee4d511d6199f26c04783b6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_dynamic_index_buffer 44a84e2f25f44f944cfb4797749b6b79c9912699c1aabdb4d1e952c65c5bed8f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_dynamic_vertex_buffer 8814478d76d15f936fc298288a4bc6cbd80999d727a47773219fa8f550bbe413 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_indirect_buffer 3581ac5584923a123cf7c75efef48244c85d0e81d46838d2c3b5ebf2efec95c9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_image ef174e2b25a7e1503166e345a9a8576381f75b521207e10ad0e40a97ea2ed6ee -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_dispatch 0b2a3736798d7e9baf49c2567ba79a3414de8d625dace858cb87e2e94d3acc8f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_dispatch_indirect 3e3d5635b4ba21113f9224f9d36fcd2cdee65482da44ba60b2b050e5b99ac69b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_discard 79f3b428a3528948683a659d191386414b24c334582f30a927b50a107074682e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_blit 2bf33975289b893ec2cc3fe48661c55ae49772dca9840fe719bd8291b5d3a9ed -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.request_screen_shot 84238af104eb40b7268895b4cecec3ab9b6c4505c4ed6b02dab4f7adefb72aac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.render_frame d4b9f4efa3b06cb4ce6232ea93c04d11c97616b7ca80a93973d8376e75afb95d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_platform_data 78f0cbf424ab6beae4b486745bcc8f4a036945a49db945e8c192f81c92a2fe1d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_internal_data 5b67a9b10da1d0afb07a82fcbad42af9517ecee356e7a5d3dbe4a45973eae605 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.override_internal_texture_ptr d5d54e9093e7e456e600bfc3acf0dc94c1e8e5a4a226b1f6cd560dc647cae051 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.override_internal_texture 583909dad5c5bfb559358e018b5f133a221f41f8798ddba2d60caf0aa3207e83 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_marker 175005d6df05036d51ecf223bc34b467606178c017e9e2415b41bf2d0258f3c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_state ee01e7db5a83c0a8e93b5190a6d40b240f1df27793787bc2ac4f72d5ba2a6158 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_condition 6272eaaf82c8b907a492a3731b8425e938daf49ee843744c62dfa4187389f723 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_stencil c50dccbd7ad0168780c0d6efb5514b712bcd02839339531576059f45c937fd05 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_scissor b8aea66f74fe71550494b691f54be000871b93538f121e059bcd16be48a79d83 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_scissor_cached 119fd7f6e5d68f4c4ca0eb121fa37b91a49330aa0724a97a6c2883ef86246dba -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transform c684a73ff48fd855ecbca14608320cff7e70e386c64e3c0f3192b15e7e9250ea -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transform_cached b318cc31d2992bbd2798e1895b1a7e611f825b753feffa4a933c370e6ae2cbba -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_transform b01509d265e63cadb2e3f21036267d3e88e196d0ef6d93046cc695875c2d0891 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_uniform 1d8affc403114956b5fec00d95f7b0a2b726e6c44c0d2c52a010e3effee8b98c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_index_buffer fed73558d742f9246051d8b8911840adb9b423f407515248887f40603fecbcb4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_dynamic_index_buffer 3a5cc2016cf4be26ae91e5f344a689e5c254c8159dd64c211aad5965dfae9b61 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transient_index_buffer 091161bbbdcfa6b77b4dffb919f2019b90e513207518f4ca40b608f929965722 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_vertex_buffer 082d0624e4d4bdfa3f15e0af0eed062841033486de40d8073cf5d57846b83cdb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_vertex_buffer_with_layout 208c4cf672a443e6ba8ba98102d44f1e86da131a4cc993d63ecbc4796506e7ac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_dynamic_vertex_buffer 6164f33066bbcca01bcb1c3d36dc6aa69eaee7abd4aefb9a747f8be4a31939ce -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_dynamic_vertex_buffer_with_layout b032858b100be1aea147a1beb9b5f21a241cb311e6fff94e72db6d828be798b3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transient_vertex_buffer ebfa735a40c9aef7a5de206d6e1bafb9fdeb9d6ce69400012f33fb881c818453 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transient_vertex_buffer_with_layout dda24ac2859a9b3bcc54297ba5cbf0d8717c1b50d9e013ad7b626edd1dd94164 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_vertex_count 25e2433daacfc12d9240d1a0dff14bf09110a09cad418f96ac567acaa21a829c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_instance_data_buffer 6d4656950d42c8321905e0776039e74f68e249a0a56fd157f8fcedd7a24ef485 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_instance_data_from_vertex_buffer c04cdebcdce77b6c23aef0b18f0f6c25c4758780b38a4b8e5d543ff90f56b18f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_instance_data_from_dynamic_vertex_buffer b31045f6cc4b391667af8c40739a34d856c5100ff1c551e5dac7e94bd1593c0c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_instance_count 546cab4b8b23e98bad8be3bc080a9bec6629c1fd34dfbb461f34bc0498922add -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_texture aec86c11ce45333a2766f3cd5333b5424796c450810484d6f7d55b3728fb2a30 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.touch 2a4949fa2a046130d3c5c0334a3a1c27e891f26cc1c05e38d24edd5a10fcb830 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.submit 4019b00ed3fcc1b79dfc1700f7470dc50cff2a055617dbfcf350961a01f1bf29 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.submit_occlusion_query be11eb2366c3a6855b3304891a8e1deb4421e5da173c913b2e9934d61b89e19b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.submit_indirect 4692e7728431fca0502fefd443b1ff5027d19af206f1831c0fafda476b925e39 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.submit_indirect_count e8ce242908949db8127a2f5831358bca79a791a558f6fc5276398f26acf2bcfd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_index_buffer 224cef813cdee57b0a242222f821f4b84251640c511b81f45ff14d837ab4aa13 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_vertex_buffer 0d50512d9e64a9d257eaf9dc74a94c7a194c997aafb5a5bf300888438a9952cd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_dynamic_index_buffer ef94ac0bd8c0c84da913e6ed0d9df97517ec3d64ca710bba3d8a062e5f3583a9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_dynamic_vertex_buffer 2d86dcbf3396f44faf1a147a7bbb8b4d6733028c64ce361b25821921bb89e1f6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_indirect_buffer 90d066f99fa126691a365136eda33a1f80c75c29ca5a3a272b5d34beb141bbda -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_image 0e095218594a96ab6c5a4c8cfbdd33289496e2ad4cad39f32f4ccab1d8c2617d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.dispatch 5ca8112ddf955c0e97d62954febb706fd2967b5c881f295aba2899d4946f6b66 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.dispatch_indirect 0b8c8efeb27501e5b143413a5b7771cba2582ec435a7017d480d1b948a05dbf6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.discard 41ca712b703c16091c34b67ceddd7f47bde3c6e903d2e53d1b5db3b2d42374f3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InterfaceVtbl.blit 5a340ee61d6d23e7b5d1b822b89c42b501af3793daa551a426bf3c5817efde05 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.InterfaceVtbl.#ctor(System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*) 3fea97983a9e7a38a674c87ae79301ba0931c1801995e7b57aa92927d44584c2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-881c6f26cc72e1eb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2928) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58f1ac2ca82d8b65"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_instance_data_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2959) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-789126292a15b0e0"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_transform`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3072) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64ad13127c7ca4fb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_transient_buffers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2958) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e6249f16bd13757"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_transient_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2956) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-52e0f8343df2b641"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_transient_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2957) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-434dab6fc839255d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.attachment_init`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2902) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e038b8771ef7ad49"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.blit`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3103) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-70a0c96470117853"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.calc_texture_size`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2971) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8504e54ff090169a"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.copy`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2929) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e8686ca8bed6ade6"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_compute_program`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2967) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f87f9b73c4023d06"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_dynamic_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2945) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8faf2f1cf6336552"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_dynamic_index_buffer_mem`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2946) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-973bcb655e0c0329"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_dynamic_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2949) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8cb8bf652587e2dc"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_dynamic_vertex_buffer_mem`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2950) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6173183e3c49f1f5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2984) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2fb4e990ba613058"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer_from_attachment`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2987) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c00b1f947ff5ede"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer_from_handles`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2986) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f66716f38ced76f1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer_from_nwh`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2988) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d291597b61fc324c"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer_scaled`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2985) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b11a080ef297838"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2937) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba62dfed169bd832"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_indirect_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2960) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5342c5638f5a7fb0"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_occlusion_query`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2996) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-96913219ded3e82b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_program`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2966) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7406ea56d1ea9ae"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_shader`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2962) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a23de77e8878367f"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2972) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9aebb1744a38b11"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture_2d`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2973) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-520ebaffc0ce2024"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture_2d_scaled`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2974) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a8edc65b2382c18f"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture_3d`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2975) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e550ef9e85546fd"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture_cube`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2976) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a620c9f62db6a6f8"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_uniform`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2992) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae4d61d7bb96f639"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_uniform_with_freq`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2993) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f8455c233f364a18"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2942) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58762a2b2e52e4e7"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_vertex_layout`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2940) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f175a1238f09361"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.dbg_text_clear`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2933) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-421729a56a26c1c2"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.dbg_text_image`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2936) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b170887f14632a5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.dbg_text_printf`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2934) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-442e982dddb3988d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.dbg_text_vprintf`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2935) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-049bc815fb7310ad"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_dynamic_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2948) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-00e83b078ad411f1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_dynamic_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2952) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b30ba1781278048"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_frame_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2991) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d5519d8175e9dd8"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2939) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0231cf231e2272af"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_indirect_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2961) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5a123d67bf03dc8"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_occlusion_query`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2998) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f83ee5ef29168cd9"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_program`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2968) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-776b77dc5e19bedb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_shader`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2965) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-406b81c518245a96"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_texture`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2983) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-50990446025978b2"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_uniform`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2995) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01ffd0e01f86f583"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2944) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-79cc79d80b26f2a5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_vertex_layout`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2941) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7e7642431d2e46e6"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.discard`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3102) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb7b5fdae5cfa050"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.dispatch`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3100) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-616ccec11a841c7e"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.dispatch_indirect`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3101) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b6f70a92a6437f37"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_alloc_transform`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3024) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d7d7705bc1c944cb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_begin`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3014) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a46fdf260b93ba2"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_blit`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3057) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-50a5292e993953da"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_discard`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3056) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5b756b1b231e269b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_dispatch`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3054) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4b54330dd722eb3"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_dispatch_indirect`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3055) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3fc14de414c337ff"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_end`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3015) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-57c61a49b799c3ec"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_dynamic_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3050) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d705532a5fcf6bc"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_dynamic_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3051) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e0091b3f4a0d66c"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3048) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-55887db067644230"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_indirect_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3052) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a77cf57d7e7eb8bb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3049) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a56c4f5c656b31a1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_condition`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3018) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b357dd68c8309be"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_dynamic_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3029) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ebd1c22712ed87aa"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_dynamic_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3033) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-06903d3681c68269"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_dynamic_vertex_buffer_with_layout`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3034) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e29ab0c93a48e8a"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_image`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3053) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf7f18ba407962d9"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3028) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b017ffefa6a6dcd3"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_instance_count`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3041) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e8b2853bb175b42"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_instance_data_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3038) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-327a74b4e0fcc730"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_instance_data_from_dynamic_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3040) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85ab5d113dbd6482"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_instance_data_from_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3039) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-552c3e0cb41710b5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_marker`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3016) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-427316c28085176d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_scissor`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3020) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3531d44a55adbe12"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_scissor_cached`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3021) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9298a248116887cb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_state`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3017) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-504457343ce31b04"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_stencil`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3019) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-41e767a44c032a53"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_texture`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3042) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f95c7c1db973cee"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transform`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3022) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da602210e18c6138"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transform_cached`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3023) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c03a1825e742744b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transient_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3030) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69395c3615aca71b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transient_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3035) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-30b44c26ed8c65f8"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transient_vertex_buffer_with_layout`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3036) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-04001b9df3dbf3cc"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_uniform`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3025) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-50144fd2154c3f57"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3031) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c591524cf38ecea"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_vertex_buffer_with_layout`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3032) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-67441a0958eccc84"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_vertex_count`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3037) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-898725393bc88f5b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_submit`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3044) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7c2375d7d85816e4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_submit_indirect`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3046) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c17ec3f482d4fdce"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_submit_indirect_count`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3047) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4851ad9f86e2dc79"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_submit_occlusion_query`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3045) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df815ab5885df03e"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_touch`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3043) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d8c0105f7f667d54"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.frame`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2924) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c65835caca517de"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_avail_instance_data_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2955) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-37cefe8670ee513a"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_avail_transient_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2953) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20579853e9a8e93d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_avail_transient_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2954) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-845ae89789b64c28"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_caps`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2926) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53dec3120fe355cd"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_direct_access_ptr`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2982) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5fa16bd691d0aa6"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_internal_data`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3061) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-277d311cf1bc1dff"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_renderer_name`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2919) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-062995210c629b7b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_renderer_type`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2925) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27a2ec85c696ee35"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_result`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2997) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cdcd6b313c40ccfe"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_shader_uniforms`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2963) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-180f752a3a1e0980"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_stats`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2927) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f7642dd2570fec58"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_supported_renderers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2918) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f8fef024df82db3"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_texture`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2990) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a7374b2b9da8775"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_uniform_info`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2994) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ebc69e82de4b5f5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.init`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2921) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-77497b6d42434e48"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.init_ctor`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2920) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c3b9c329cbb3eea5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.is_frame_buffer_valid`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2970) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f00fe0c9e383872"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.is_texture_valid`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2969) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f96d94c6177a339"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.make_ref`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2930) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f51811918ae8e867"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.make_ref_release`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2931) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed338450f9448eac"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.override_internal_texture`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3063) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d32ede7f357ad2b6"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.override_internal_texture_ptr`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3062) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b8e50e37bd2e2e12"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.read_texture`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2980) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f2f3302867c0f62"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.render_frame`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3059) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11276a3d6addcc03"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.request_screen_shot`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3058) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-269e5c0b3f7fff61"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.reset`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2923) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-45c08ee8110af0ce"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.reset_view`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3013) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b472521fa5e7076"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_dynamic_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3096) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d881b1e0b15d4a71"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_dynamic_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3097) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb9960ca51149435"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3094) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5b75577cfcca277"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_indirect_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3098) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0b436b066a2e4a18"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3095) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a4de35efc5e3568"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_condition`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3066) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb2f364c104d7496"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_debug`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2932) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2348dca51b797a6d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_dynamic_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3075) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cdfce4c31dc8cfda"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_dynamic_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3079) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-83492b897ebf5868"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_dynamic_vertex_buffer_with_layout`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3080) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6086c7c6f353839"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_frame_buffer_name`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2989) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-42f2956697d850e1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_frame_uniform`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3027) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e1a4233b943d396"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_image`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3099) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a50932e7e93a76ec"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3074) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-05432a0f227969c7"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_index_buffer_name`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2938) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3853506c3a4d179d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_instance_count`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3087) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2bba29719d77ce2e"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_instance_data_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3084) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc5986c7106d4e0a"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_instance_data_from_dynamic_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3086) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0329319b114a403a"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_instance_data_from_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3085) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a425767d994f9675"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_marker`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3064) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7998e668006c8c6b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_palette_color`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2999) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d62afd19b7655048"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_palette_color_rgba32f`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3000) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7027e94163396393"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_palette_color_rgba8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3001) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-135093968574f4c8"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_platform_data`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3060) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-024c4facbde7e27f"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_scissor`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3068) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f5c72fcf1d6f70b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_scissor_cached`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3069) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-187e827abd5c0dd6"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_shader_name`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2964) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e79ed7b5f89f5cc7"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_state`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3065) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-66e01242dee3b2f4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_stencil`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3067) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfa201d3c19bcaab"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_texture`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3088) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-307decabe5cb1889"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_texture_name`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2981) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2db309948d7836b4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transform`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3070) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3f7da7783b5e2529"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transform_cached`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3071) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53aeecdda9f1ded6"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transient_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3076) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf863f3e44307ccb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transient_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3081) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bdeb741049eeb49b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transient_vertex_buffer_with_layout`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3082) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07ae58a61018caf3"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_uniform`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3073) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-efbe8db61e8356e7"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3077) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8f08923e080c01e1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_vertex_buffer_name`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2943) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f749252a9bb1734"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_vertex_buffer_with_layout`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3078) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea65116e5d183c0d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_vertex_count`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3083) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-430884746d71c699"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_clear`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3006) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c999702df82368ee"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_clear_mrt`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3007) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1ae444a57dd38e7c"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_frame_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3009) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9836eb16dcf4b5bd"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_mode`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3008) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2bb6f310d728d460"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_name`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3002) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7c828b6af85507c4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_order`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3011) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-92b45c70c2e10d69"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_rect`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3003) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae57395a1633040f"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_rect_ratio`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3004) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ebb06b099b9575f2"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_scissor`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3005) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3c82283425b948da"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_shading_rate`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3012) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b4ead3b935434133"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_transform`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3010) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b2bf807b39b4c897"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_uniform`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3026) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-83d859bc091531f1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.shutdown`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2922) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a58fb85fe97647ec"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.submit`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3090) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab0f294696d51a9b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.submit_indirect`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3092) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-02c6b7c169f999a3"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.submit_indirect_count`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3093) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d8d7a63dec6a83cc"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.submit_occlusion_query`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3091) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-425b0d8334b1cfa2"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.topology_convert`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2916) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e7f2bbbeb9a6de5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.topology_sort_tri_list`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2917) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6663a631918ad948"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.touch`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3089) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5139737cfb6198e1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_dynamic_index_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2947) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0645f287acb5c5cb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_dynamic_vertex_buffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2951) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11c166f36960b391"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_texture_2d`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2977) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a99544e3d27f3259"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_texture_3d`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2978) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-98ad0194c5752123"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_texture_cube`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2979) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ff9f6eb75212c7e4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_convert`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2914) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58312c87f647e67a"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_add`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2904) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-16e06d0176d40a38"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_begin`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2903) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1db449252223287e"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_decode`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2905) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6de59338c15e60f"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_end`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2908) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-467cefaf193b8dc4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_get_offset`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2909) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c5d709ed56cbbdc2"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_get_size`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2911) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-65830f61d31587e4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_get_stride`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2910) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c2dae36b754dca69"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_has`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2906) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f8abb08eac804685"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_skip`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2907) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-12bc6a37f4e78faf"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_pack`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2912) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f3f313969a0bb88"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_unpack`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2913) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9fafcd350f6ce550"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.weld_vertices`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2915) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-395ed68db05a6780"></a>[`public Inno.Native.Bgfx.bgfx.InterfaceVtbl.InterfaceVtbl(void* attachment_init = null, void* vertex_layout_begin = null, void* vertex_layout_add = null, void* vertex_layout_decode = null, void* vertex_layout_has = null, void* vertex_layout_skip = null, void* vertex_layout_end = null, void* vertex_layout_get_offset = null, void* vertex_layout_get_stride = null, void* vertex_layout_get_size = null, void* vertex_pack = null, void* vertex_unpack = null, void* vertex_convert = null, void* weld_vertices = null, void* topology_convert = null, void* topology_sort_tri_list = null, void* get_supported_renderers = null, void* get_renderer_name = null, void* init_ctor = null, void* init = null, void* shutdown = null, void* reset = null, void* frame = null, void* get_renderer_type = null, void* get_caps = null, void* get_stats = null, void* alloc = null, void* copy = null, void* make_ref = null, void* make_ref_release = null, void* set_debug = null, void* dbg_text_clear = null, void* dbg_text_printf = null, void* dbg_text_vprintf = null, void* dbg_text_image = null, void* create_index_buffer = null, void* set_index_buffer_name = null, void* destroy_index_buffer = null, void* create_vertex_layout = null, void* destroy_vertex_layout = null, void* create_vertex_buffer = null, void* set_vertex_buffer_name = null, void* destroy_vertex_buffer = null, void* create_dynamic_index_buffer = null, void* create_dynamic_index_buffer_mem = null, void* update_dynamic_index_buffer = null, void* destroy_dynamic_index_buffer = null, void* create_dynamic_vertex_buffer = null, void* create_dynamic_vertex_buffer_mem = null, void* update_dynamic_vertex_buffer = null, void* destroy_dynamic_vertex_buffer = null, void* get_avail_transient_index_buffer = null, void* get_avail_transient_vertex_buffer = null, void* get_avail_instance_data_buffer = null, void* alloc_transient_index_buffer = null, void* alloc_transient_vertex_buffer = null, void* alloc_transient_buffers = null, void* alloc_instance_data_buffer = null, void* create_indirect_buffer = null, void* destroy_indirect_buffer = null, void* create_shader = null, void* get_shader_uniforms = null, void* set_shader_name = null, void* destroy_shader = null, void* create_program = null, void* create_compute_program = null, void* destroy_program = null, void* is_texture_valid = null, void* is_frame_buffer_valid = null, void* calc_texture_size = null, void* create_texture = null, void* create_texture_2d = null, void* create_texture_2d_scaled = null, void* create_texture_3d = null, void* create_texture_cube = null, void* update_texture_2d = null, void* update_texture_3d = null, void* update_texture_cube = null, void* read_texture = null, void* set_texture_name = null, void* get_direct_access_ptr = null, void* destroy_texture = null, void* create_frame_buffer = null, void* create_frame_buffer_scaled = null, void* create_frame_buffer_from_handles = null, void* create_frame_buffer_from_attachment = null, void* create_frame_buffer_from_nwh = null, void* set_frame_buffer_name = null, void* get_texture = null, void* destroy_frame_buffer = null, void* create_uniform = null, void* create_uniform_with_freq = null, void* get_uniform_info = null, void* destroy_uniform = null, void* create_occlusion_query = null, void* get_result = null, void* destroy_occlusion_query = null, void* set_palette_color = null, void* set_palette_color_rgba32f = null, void* set_palette_color_rgba8 = null, void* set_view_name = null, void* set_view_rect = null, void* set_view_rect_ratio = null, void* set_view_scissor = null, void* set_view_clear = null, void* set_view_clear_mrt = null, void* set_view_mode = null, void* set_view_frame_buffer = null, void* set_view_transform = null, void* set_view_order = null, void* set_view_shading_rate = null, void* reset_view = null, void* encoder_begin = null, void* encoder_end = null, void* encoder_set_marker = null, void* encoder_set_state = null, void* encoder_set_condition = null, void* encoder_set_stencil = null, void* encoder_set_scissor = null, void* encoder_set_scissor_cached = null, void* encoder_set_transform = null, void* encoder_set_transform_cached = null, void* encoder_alloc_transform = null, void* encoder_set_uniform = null, void* set_view_uniform = null, void* set_frame_uniform = null, void* encoder_set_index_buffer = null, void* encoder_set_dynamic_index_buffer = null, void* encoder_set_transient_index_buffer = null, void* encoder_set_vertex_buffer = null, void* encoder_set_vertex_buffer_with_layout = null, void* encoder_set_dynamic_vertex_buffer = null, void* encoder_set_dynamic_vertex_buffer_with_layout = null, void* encoder_set_transient_vertex_buffer = null, void* encoder_set_transient_vertex_buffer_with_layout = null, void* encoder_set_vertex_count = null, void* encoder_set_instance_data_buffer = null, void* encoder_set_instance_data_from_vertex_buffer = null, void* encoder_set_instance_data_from_dynamic_vertex_buffer = null, void* encoder_set_instance_count = null, void* encoder_set_texture = null, void* encoder_touch = null, void* encoder_submit = null, void* encoder_submit_occlusion_query = null, void* encoder_submit_indirect = null, void* encoder_submit_indirect_count = null, void* encoder_set_compute_index_buffer = null, void* encoder_set_compute_vertex_buffer = null, void* encoder_set_compute_dynamic_index_buffer = null, void* encoder_set_compute_dynamic_vertex_buffer = null, void* encoder_set_compute_indirect_buffer = null, void* encoder_set_image = null, void* encoder_dispatch = null, void* encoder_dispatch_indirect = null, void* encoder_discard = null, void* encoder_blit = null, void* request_screen_shot = null, void* render_frame = null, void* set_platform_data = null, void* get_internal_data = null, void* override_internal_texture_ptr = null, void* override_internal_texture = null, void* set_marker = null, void* set_state = null, void* set_condition = null, void* set_stencil = null, void* set_scissor = null, void* set_scissor_cached = null, void* set_transform = null, void* set_transform_cached = null, void* alloc_transform = null, void* set_uniform = null, void* set_index_buffer = null, void* set_dynamic_index_buffer = null, void* set_transient_index_buffer = null, void* set_vertex_buffer = null, void* set_vertex_buffer_with_layout = null, void* set_dynamic_vertex_buffer = null, void* set_dynamic_vertex_buffer_with_layout = null, void* set_transient_vertex_buffer = null, void* set_transient_vertex_buffer_with_layout = null, void* set_vertex_count = null, void* set_instance_data_buffer = null, void* set_instance_data_from_vertex_buffer = null, void* set_instance_data_from_dynamic_vertex_buffer = null, void* set_instance_count = null, void* set_texture = null, void* touch = null, void* submit = null, void* submit_occlusion_query = null, void* submit_indirect = null, void* submit_indirect_count = null, void* set_compute_index_buffer = null, void* set_compute_vertex_buffer = null, void* set_compute_dynamic_index_buffer = null, void* set_compute_dynamic_vertex_buffer = null, void* set_compute_indirect_buffer = null, void* set_image = null, void* dispatch = null, void* dispatch_indirect = null, void* discard = null, void* blit = null)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3104) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bc92182e4d6434fc"></a>[`public struct Inno.Native.Bgfx.bgfx.InterfaceVtbl`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2900) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.InternalData</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.InternalData 51ccdb94bad68e67827e0950ac2649bd36a60cfd21a506fe3e4432c40ec87e2f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InternalData.caps 0545fde4fc63b336693a0e47e7fc2435fa187255ad24d6696f1ee4468a7d52f2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InternalData.context 03ed0a2a983e550e0d31672cf6ea4d396053e1e4e52a0240ed4784b70bb784e5 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.InternalData.#ctor(Inno.Native.Bgfx.bgfx.Caps*,System.Void*) 3ab273e5cea246380c7960fe68f93d02e78d8bbcf6bd336acba4138b3b064a02 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4651d7163034cbdd"></a>[`public Inno.Native.Bgfx.bgfx.Caps* Inno.Native.Bgfx.bgfx.InternalData.caps`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2884) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7495af0be1dcc1f8"></a>[`public void* Inno.Native.Bgfx.bgfx.InternalData.context`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2885) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4b8126b92bb5241"></a>[`public Inno.Native.Bgfx.bgfx.InternalData.InternalData(Inno.Native.Bgfx.bgfx.Caps* caps = null, void* context = null)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2886) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1bb26ab0472f9567"></a>[`public struct Inno.Native.Bgfx.bgfx.InternalData`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2882) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.Memory</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.Memory eb3f14984391f46b51fe5b6f5f875745b7a038c592c5102851b09eb2f4f0503e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Memory.data 8762923598208ab13efdb485fa2424f1af9e3ae746b769f63f7463876b5cd9f2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Memory.size b56a4666ce0cd798ce0f65e2421befed0cc4ba876313a6f742840b399ef0f69f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Memory.#ctor(System.Byte*,System.UInt32) 9e39a98131feb5b94b85fd6aa4b769cc6e5cc1cabca6519e588e90fb09dc339a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-732a17b2d093ec4e"></a>[`public byte* Inno.Native.Bgfx.bgfx.Memory.data`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1979) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2ed4c50568eb285d"></a>[`public uint Inno.Native.Bgfx.bgfx.Memory.size`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1980) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e9be5c17485d673"></a>[`public Inno.Native.Bgfx.bgfx.Memory.Memory(byte* data = null, uint size = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1981) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5a09c577c0da4b2e"></a>[`public struct Inno.Native.Bgfx.bgfx.Memory`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1977) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.NativeWindowHandleType</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.NativeWindowHandleType 6b765d1d901519a739eadd353100bb201e4626695f4430029ea98f14f2177a6b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Default 5268c56ed418004ba00d1b37676ed5b376cb2d087c2dd7df2bbc5c02de7c636f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Wayland 1a51db22f0015ee17d0986ed345eff90f3f6389c1a1b91be7ae41f6a082cd73d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Count 478bea69367b9a0efa12df925e48c7e541f5ddc0106da3106e374937fa4a021e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-932328d246cb378c"></a>[`Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Count = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L387) | 当前常量或枚举值：`2`。 | — |
| <a id="api-6f7779b525c56053"></a>[`Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Default = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L385) | 当前常量或枚举值：`0`。 | — |
| <a id="api-8bc6888fd038a6bd"></a>[`Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Wayland = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L386) | 当前常量或枚举值：`1`。 | — |
| <a id="api-5275ffa9e848eb83"></a>[`public enum Inno.Native.Bgfx.bgfx.NativeWindowHandleType : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L383) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.OcclusionQueryHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.OcclusionQueryHandle 0d7e78c029985e0b7305ffdfab599ee2e50516314bd58c5d8aa9ca3748190ba4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.idx ff4d1ca44bd26d743b77b18c7082383fa2e966707f9ef43e7b3eee79759842bb -->
<!-- api: P:Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.Valid 52e892c897db39f7ded4fcfad93e2f643214d551b3d17e9cd0374a5dc0a8838a -->
<!-- api: M:Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.#ctor(System.UInt16) 9c2f74e37478783755c8b5ae48605b4260d0edcbfd31e23f233bef75b4466c2c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dd01e85403de3928"></a>[`public ushort Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.idx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2821) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a8557a94ced2aff8"></a>[`public Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.OcclusionQueryHandle(ushort idx = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2824) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0797eabd0d3711e0"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.Valid { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2822) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d68beac0a0730ed6"></a>[`public struct Inno.Native.Bgfx.bgfx.OcclusionQueryHandle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2819) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.OcclusionQueryResult</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.OcclusionQueryResult 8b1c068502d0a95f9c41c488e3eeacbfa6d94ee1793bf48e0d47d888d01cd176 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.OcclusionQueryResult.Invisible fb5b845f382e78b84ae0f63eb99ac9395e25b5454faa2f68652aadf61278448f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.OcclusionQueryResult.Visible a35fb50ef05ad9c958f8ae493ed5e37f6b8aecfb19a6e1fb8d8a2b688f6907a1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.OcclusionQueryResult.NoResult 2885d8860e048d2349769420450763f8765ea41c554964f9f9ee25e6d56169fe -->
<!-- api: F:Inno.Native.Bgfx.bgfx.OcclusionQueryResult.Count d659a841360ff6ca7fedb1d862ea56a227ac2f3dd86fddc4f4cda328f89dc258 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f6def1baaaa3f26a"></a>[`Inno.Native.Bgfx.bgfx.OcclusionQueryResult.Count = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L286) | 当前常量或枚举值：`3`。 | — |
| <a id="api-a3ba883e0c252db8"></a>[`Inno.Native.Bgfx.bgfx.OcclusionQueryResult.Invisible = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L283) | 当前常量或枚举值：`0`。 | — |
| <a id="api-03cb06ac339775e8"></a>[`Inno.Native.Bgfx.bgfx.OcclusionQueryResult.NoResult = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L285) | 当前常量或枚举值：`2`。 | — |
| <a id="api-72f21c9585dd794e"></a>[`Inno.Native.Bgfx.bgfx.OcclusionQueryResult.Visible = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L284) | 当前常量或枚举值：`1`。 | — |
| <a id="api-40203bc73f9b0d8f"></a>[`public enum Inno.Native.Bgfx.bgfx.OcclusionQueryResult : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L281) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.PciIdFlags</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.PciIdFlags 72800368a820c6b112f78ecbcfd00128369745a61e6d8f002116d6e5260957b4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PciIdFlags.None cb7fdd433fdad674b751097d9ad2fe745db78d9cbf2946daf01eaab1aa17481e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PciIdFlags.SoftwareRasterizer aa6c1cc911b9a225b2996195d85507f32c7a106c5f6c917ea7e99b0b08c1086e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PciIdFlags.Amd a056941b861c68a3bcc3a3f1cd5e06ab3719176e2645089b8aa4def262921bd2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PciIdFlags.Apple 423ba2200f43416cc2c0552b6e198f1e460a8ef9062900503601d87d98ab5c56 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PciIdFlags.Intel 620de43037d51ecace65e501a0002d1ae9cca9161e05948b697450a071be0dac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PciIdFlags.Nvidia fe486a917ff92fa753aa1b4a9d47f5e5e93f63c068b54d8fc7c6d4b38606a93e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PciIdFlags.Microsoft 21bd2705efe7f148f535bd3156445c57500e1cf9cdfa575a72e11085df1857c0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PciIdFlags.Arm a8f443c0f4328fe7d8e393810454c9abf90569c071eeb4c15c5ee61f7ced6685 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-699278333e5a49af"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.Amd = 4098`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3526) | 当前常量或枚举值：`4098`。 | — |
| <a id="api-c45c2318260247a2"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.Apple = 4203`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3527) | 当前常量或枚举值：`4203`。 | — |
| <a id="api-3935477ab99cddfd"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.Arm = 5045`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3531) | 当前常量或枚举值：`5045`。 | — |
| <a id="api-b471e9f381bab53d"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.Intel = 32902`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3528) | 当前常量或枚举值：`32902`。 | — |
| <a id="api-ffb6d0838ae1426a"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.Microsoft = 5140`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3530) | 当前常量或枚举值：`5140`。 | — |
| <a id="api-0ed54416f8fa12a6"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.None = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3524) | 当前常量或枚举值：`0`。 | — |
| <a id="api-694c3a85555c0faa"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.Nvidia = 4318`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3529) | 当前常量或枚举值：`4318`。 | — |
| <a id="api-8a0c8a6349c735f2"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.SoftwareRasterizer = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3525) | 当前常量或枚举值：`1`。 | — |
| <a id="api-ae56e94d4ac285e6"></a>[`public enum Inno.Native.Bgfx.bgfx.PciIdFlags : ushort`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3522) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.PlatformData</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.PlatformData 5746f18265f5c0b498ee35cc025c7f85d0c0a614ad3599c7a83222127b5ae98d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PlatformData.ndt 8ec4e529d1a878984f292da8ddf70a63f6833df06a77bb3eed35ea1f8771f4ba -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PlatformData.nwh 33f164d56a64056b72d157f1664e3397bb24b6dfa75315504a38f0bb81586d55 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PlatformData.context 3397c91c153b7877772a4db6298407b87da2cd0ae34f31b1bc861930dda1b955 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PlatformData.queue bbb1a51be8713a045c3fb8ec1fb49d355be983062f7515ec206e214680b15ff5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PlatformData.backBuffer db1ae91e63f03884f94805641fe5f9f97b28c8a4f40ee3115cf86b494d762e09 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PlatformData.backBufferDS 421c6e8d70e650b8ba26f178e81f776bf3878667555e50b8fc39a2e712205771 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.PlatformData.type 0ab6374b693fe9449b66353eee857919f7787a8ce14a2a03cb4d1606eace7db3 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.PlatformData.#ctor(System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,Inno.Native.Bgfx.bgfx.NativeWindowHandleType) d5492b8ffa6a7bf9c8e24a6becd03ac07686b11818032cca6f251c77795d9402 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-27767ee1d302639a"></a>[`public void* Inno.Native.Bgfx.bgfx.PlatformData.backBuffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L808) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-76e9f38e3e550404"></a>[`public void* Inno.Native.Bgfx.bgfx.PlatformData.backBufferDS`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L809) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b20c3bc99563b6b5"></a>[`public void* Inno.Native.Bgfx.bgfx.PlatformData.context`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L806) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-342f1ab746eab45a"></a>[`public void* Inno.Native.Bgfx.bgfx.PlatformData.ndt`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L804) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6e5c92f2e316902e"></a>[`public void* Inno.Native.Bgfx.bgfx.PlatformData.nwh`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L805) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce34b743265a7bc8"></a>[`public void* Inno.Native.Bgfx.bgfx.PlatformData.queue`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L807) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-faedf77ff21f7cd4"></a>[`public Inno.Native.Bgfx.bgfx.NativeWindowHandleType Inno.Native.Bgfx.bgfx.PlatformData.type`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L810) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce1088c1baa6e163"></a>[`public Inno.Native.Bgfx.bgfx.PlatformData.PlatformData(void* ndt = null, void* nwh = null, void* context = null, void* queue = null, void* backBuffer = null, void* backBufferDS = null, Inno.Native.Bgfx.bgfx.NativeWindowHandleType type = Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Default)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L811) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d1c9dd22355b339c"></a>[`public struct Inno.Native.Bgfx.bgfx.PlatformData`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L802) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.ProgramHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.ProgramHandle c4d0a12bfd17a7552cb8b40833db4ecc2c319a52d636bb011455188915965138 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ProgramHandle.idx 3e096c6a797a741a5e61cc4c7ac15a3cbb4045594a04b220653d17f5dd155fa6 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.ProgramHandle.Valid 1b99a000647dc6cbf496fd451007a89f39fec4f15fb36293ca3cb83fc2a60dec -->
<!-- api: M:Inno.Native.Bgfx.bgfx.ProgramHandle.#ctor(System.UInt16) 0a1799b43db3c09f83be6d53f48c3f898c2b0722c133c03870dc05e0258e08ce -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6fd379784de5f0aa"></a>[`public ushort Inno.Native.Bgfx.bgfx.ProgramHandle.idx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2217) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf7ccaab95ba7740"></a>[`public Inno.Native.Bgfx.bgfx.ProgramHandle.ProgramHandle(ushort idx = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2220) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-af7d7bff3cc587d9"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.ProgramHandle.Valid { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2218) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-06cdb68f566eef5d"></a>[`public struct Inno.Native.Bgfx.bgfx.ProgramHandle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2215) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.RendererType</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.RendererType 88f166ec74e9a0b087ad24491cba5ec3b48347a1004c7f9db86b2eb8450894d0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RendererType.Noop f5333f7946fb963e34a7e6af5e1816565593c0bff37e5a552a24eadf87d285bb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RendererType.Agc 1cd3f56671b479b0eef1d82da790c66f0f2f5c71bffa5146597b95c39523c146 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RendererType.Direct3D11 a671d8741566381579b378cd94ac9e1b803179278e64b9f7cd7838d8888b4a16 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RendererType.Direct3D12 4b9d13b8e04ed427768a486fcfbfe2d2722cabc0dabd06fdcf2219bceb8c4bf4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RendererType.Gnm 285fcc82b7befda63ed60cb9c79245e4388a2ad0cb8ca2d599fc3bd7fed46be1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RendererType.Metal 88e69d8b9f06161e97e607d912d05c810d586974e67f9d46c807edfea24e9cc3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RendererType.Nvn b592ceb70a8794b2fd9729e2adfe9988f74e6cf89752a49ba03f6f0fc3a628f3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RendererType.OpenGLES b47f23928db5f3a8aa0a078b310c75f2a19e71cb59983bcf1e166211de4f8938 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RendererType.OpenGL 8e6f79905ff7a6e614c3a35d2a1360a044587e55ffb758666b8e9824a7bb8a06 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RendererType.Vulkan 12fa32fc670142e3ff1277c2017a328f5e48ccce13fa57f087f0ddc5d11a725a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RendererType.WebGPU 28bb9a0e3c8de95c06e0a8dff9c75ece8f679208ad5858621cdec167814d5cc2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RendererType.Count 78ce449d1e795d734af975f1cb66c03cedbd09ea18dce6d947c8c5d83570b69d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1de729c27a5f8dd3"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Agc = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L43) | 当前常量或枚举值：`1`。 | — |
| <a id="api-a35e26cb28d293e0"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Count = 11`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L53) | 当前常量或枚举值：`11`。 | — |
| <a id="api-3e73f602acb7a713"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Direct3D11 = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L44) | 当前常量或枚举值：`2`。 | — |
| <a id="api-a15a4435b3700d1e"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Direct3D12 = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L45) | 当前常量或枚举值：`3`。 | — |
| <a id="api-1df40549495fb9ca"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Gnm = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L46) | 当前常量或枚举值：`4`。 | — |
| <a id="api-025df2e10549d2ac"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Metal = 5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L47) | 当前常量或枚举值：`5`。 | — |
| <a id="api-e3de132f44bfd39d"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Noop = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L42) | 当前常量或枚举值：`0`。 | — |
| <a id="api-81dcd5fc0e1cc749"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Nvn = 6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L48) | 当前常量或枚举值：`6`。 | — |
| <a id="api-6c80632ec4d6c8ec"></a>[`Inno.Native.Bgfx.bgfx.RendererType.OpenGL = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L50) | 当前常量或枚举值：`8`。 | — |
| <a id="api-463e46f1b6893709"></a>[`Inno.Native.Bgfx.bgfx.RendererType.OpenGLES = 7`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L49) | 当前常量或枚举值：`7`。 | — |
| <a id="api-d7ba0a4ac7813e11"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Vulkan = 9`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L51) | 当前常量或枚举值：`9`。 | — |
| <a id="api-52fa80d010841106"></a>[`Inno.Native.Bgfx.bgfx.RendererType.WebGPU = 10`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L52) | 当前常量或枚举值：`10`。 | — |
| <a id="api-1b122a12c7e55e6b"></a>[`public enum Inno.Native.Bgfx.bgfx.RendererType : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L40) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.RenderFrame</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.RenderFrame aac09026d261307e616954012545a5868377db8bba7c13e254f085bb42337d0e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RenderFrame.NoContext 28ad9186dff850231108792588130a145fa88bd8abc772a2b9835ef21c6e995d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RenderFrame.Render 2f203352c6d20fd130b92adba89e59daa541a17d149465829fbee0ed13a1698e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RenderFrame.Timeout 45aebbdd5b55d33f0ecdce81e06149615cccfdb9186bc51d0654862ab5efc078 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RenderFrame.Exiting c9f1e0eebc65a781d6d28a963e739b708ee3f149b289af82bb40b6a1a0b507a5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.RenderFrame.Count 1bdd5a2a8d4a80f2c33319a2990029a4529c5a4378071a11bc43efcd884839d9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b107d7603ddcecf7"></a>[`Inno.Native.Bgfx.bgfx.RenderFrame.Count = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L402) | 当前常量或枚举值：`4`。 | — |
| <a id="api-5b34643efb143ca3"></a>[`Inno.Native.Bgfx.bgfx.RenderFrame.Exiting = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L401) | 当前常量或枚举值：`3`。 | — |
| <a id="api-74a11fb74fe18f67"></a>[`Inno.Native.Bgfx.bgfx.RenderFrame.NoContext = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L398) | 当前常量或枚举值：`0`。 | — |
| <a id="api-b46637be3a7de1dc"></a>[`Inno.Native.Bgfx.bgfx.RenderFrame.Render = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L399) | 当前常量或枚举值：`1`。 | — |
| <a id="api-336115a0a436ce7f"></a>[`Inno.Native.Bgfx.bgfx.RenderFrame.Timeout = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L400) | 当前常量或枚举值：`2`。 | — |
| <a id="api-54169e5038d2f4a1"></a>[`public enum Inno.Native.Bgfx.bgfx.RenderFrame : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L396) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.ResetFlags</code> · 24 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.ResetFlags c9a1f696636d654607a2492b796c32f2417b67b9b17631713db8350264759f59 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX2 5f8d7f0229196323435f00db7483945b63201f531a8f4495ecd21f71bd2cde28 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX4 2f17d11adc89dfcdcb3e14f7e613156f4c0304fcffa4cdfc1224cf419ff8a9f5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX8 76a1392a4ddcb3e849d8d02f39b66075ede2c586c1ade0ca78e3037f34c55b6d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX16 d75c52b412cec968b6aaadfe16f7074661c3eb6b94626618bd053bf3abf9a7fc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.MsaaShift ce3056d52cd132d320dbb717de1f29ad5a7b9861ab028cdb6ed582311c3de53f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.MsaaMask a663f6d9d24188e2d30a09f1539bb1fffd2b303ea5e5df03fcc6ab809a6d5d51 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.None 32100dc9442df212bb7359d42dc9fa70d80c7f6e2cc6502c48f18fe36bc3e409 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.Fullscreen cd12822d49d5a3b14ec2627d389be4bcb93e1eb34069354371f09aa895105362 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.Vsync c7ad2eab89bc1f13ad06055425cd4717558c9b6abc85fc64151842d51118b8e8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.Maxanisotropy 281c1ee75a3ff5580bbd3497f273231b406355fc3cf7ab072553650dcb0715b1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.Capture 804d2b375d5fcee61fbdeb3a9f82811184f17beb3f025ff14b49380393db61d0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.FlushAfterRender 248bf1dbe418a406a1b5e018e9d8334ac3729216446b215b9179ea24c56a74e8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.FlipAfterRender dbac625ae0bb6867f763357f0f6f2c7433b72ec04c86d4f5f9153bf7bce9fef3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.SrgbBackbuffer b224fbdd171a293faabdfa78f41b014ff89ff8f46e51f0d37cbfa7ab7d68da4a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.Hdr10 5799515b786175b568010dda4476973d236e3741959beaa2325ba6754d3a2991 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.Hidpi 0d9f13517e7a6e413b95865c62c48afe29a819543ee86c7ca132d447a13d146f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.DepthClamp c472d20c5b45fd033b37fa0713884ad423a6e21866bc3d5061a64883b0fe73e7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.Suspend bc7431efc853e62b4bf8b04d8c17f77f9298b60f7902bbe80cd6a4b88066353e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.TransparentBackbuffer 1ecd0f6ec59a66495c36e334268470490b2220e20502b5ec46ba8348231496e7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.FullscreenShift b460a88c39d6a0347fc9f8de0f4d395a804878d566edc97a8496ccce354776ab -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.FullscreenMask 68bfd4b8637f9ad60cee6cf41e6726655a6c47a03ac4e85f33cd81f7a6b7ee94 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.ReservedShift dcc2c8fa647944339dd15a0377655123b0df2b1880038dcc6b082cccba44ebed -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResetFlags.ReservedMask 67963e5d041f90718d230e8349cc3e2ea3f41af25497c2492eaa9b717f55d002 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6532da9f9b7584e1"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.Capture = 512`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3553) | 当前常量或枚举值：`512`。 | — |
| <a id="api-19bed0ae2a40a037"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.DepthClamp = 262144`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3559) | 当前常量或枚举值：`262144`。 | — |
| <a id="api-bf7673eac4550aaa"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.FlipAfterRender = 16384`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3555) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-3111e7e5f386c079"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.FlushAfterRender = 8192`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3554) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-d5cadb3e18a9fe6e"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.Fullscreen = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3550) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d58282e17217f336"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.FullscreenMask = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3563) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d7ce6f91143f0062"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.FullscreenShift = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3562) | 当前常量或枚举值：`0`。 | — |
| <a id="api-67f6474d3311b38d"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.Hdr10 = 65536`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3557) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-16981ff4b0fb91a8"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.Hidpi = 131072`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3558) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-1668f798cb931d5f"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.Maxanisotropy = 256`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3552) | 当前常量或枚举值：`256`。 | — |
| <a id="api-e037723dec897515"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.MsaaMask = Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX8 &#124; Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3548) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX8 &#124; Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX16`。 | — |
| <a id="api-1c3acd942edb4934"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.MsaaShift = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3547) | 当前常量或枚举值：`4`。 | — |
| <a id="api-d8a666e8d590e73f"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX16 = 64`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3546) | 当前常量或枚举值：`64`。 | — |
| <a id="api-6ff99c43598f195a"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX2 = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3543) | 当前常量或枚举值：`16`。 | — |
| <a id="api-631cfedff1c4e907"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX4 = 32`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3544) | 当前常量或枚举值：`32`。 | — |
| <a id="api-6e07414a3853c0a5"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX8 = Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX2 &#124; Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3545) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX2 &#124; Inno.Native.Bgfx.bgfx.ResetFlags.MsaaX4`。 | — |
| <a id="api-70054fad52b5171b"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.None = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3549) | 当前常量或枚举值：`0`。 | — |
| <a id="api-f6fd89d482f3587f"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.ReservedMask = 2147483648`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3565) | 当前常量或枚举值：`2147483648`。 | — |
| <a id="api-f5f7bf3e0fd74233"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.ReservedShift = 31`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3564) | 当前常量或枚举值：`31`。 | — |
| <a id="api-72aa9841992d597c"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.SrgbBackbuffer = 32768`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3556) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-f9818f8c9f6f4f69"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.Suspend = 524288`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3560) | 当前常量或枚举值：`524288`。 | — |
| <a id="api-eda3245f1e5c36fa"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.TransparentBackbuffer = 1048576`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3561) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-9f920b49d2cae1e6"></a>[`Inno.Native.Bgfx.bgfx.ResetFlags.Vsync = 128`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3551) | 当前常量或枚举值：`128`。 | — |
| <a id="api-72666e5ee8f47746"></a>[`public enum Inno.Native.Bgfx.bgfx.ResetFlags : uint`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3541) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.Resolution</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.Resolution 403c64b58bfa02968828fe99a66de59caaca46d4dc1e9f845fa223f001a2c517 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Resolution.formatColor 9d3cc74b48b658fdf9f366c595979f04eb10b0794d3cbaeb106c595e05230deb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Resolution.formatDepthStencil f4ee6cac9d31bd3ea7ae12993b334145ca89186c3833a28257b96270513e2fa4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Resolution.width 2c21737a35bd99e58c43acbb815475653a3909ef96052b065c74b03299294f64 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Resolution.height 125f07547ef36fe28ed97b2dbbdf53bfeb3caaf0b422b72ab404b2efe2a6085e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Resolution.reset eef1410911491c92fd81677a578a7b13e0cd6d919b1fa8a2e7c13dc1740c5552 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Resolution.numBackBuffers a900ecf38bcf625fd7cfaab9dc1a5bfd250737cc398f9610b0b9f42d23a78cd4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Resolution.maxFrameLatency d360739d676af899fbf833a0bf9e5d5e7de911bddd437a34a5707ce68576713b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Resolution.debugTextScale c710e6b90e14c14e87954d7b494f24121c08235f8cf79f7764a8ee397b0668f3 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Resolution.#ctor(Inno.Native.Bgfx.bgfx.TextureFormat,Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt32,System.UInt32,System.UInt32,System.Byte,System.Byte,System.Byte) 0e97d8c6657f7bd38bc50d63001e7a2e598389702c7f27d002742b29ed6b6b39 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e5e60e13e3e9e459"></a>[`public byte Inno.Native.Bgfx.bgfx.Resolution.debugTextScale`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L839) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-15a53c74ff69223c"></a>[`public Inno.Native.Bgfx.bgfx.TextureFormat Inno.Native.Bgfx.bgfx.Resolution.formatColor`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L832) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-170ac774416e26e2"></a>[`public Inno.Native.Bgfx.bgfx.TextureFormat Inno.Native.Bgfx.bgfx.Resolution.formatDepthStencil`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L833) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6f6ed205e93ac49"></a>[`public uint Inno.Native.Bgfx.bgfx.Resolution.height`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L835) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8e64949cc48ffee1"></a>[`public byte Inno.Native.Bgfx.bgfx.Resolution.maxFrameLatency`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L838) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e17de95fea02960"></a>[`public byte Inno.Native.Bgfx.bgfx.Resolution.numBackBuffers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L837) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e00aa16e745e8962"></a>[`public uint Inno.Native.Bgfx.bgfx.Resolution.reset`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L836) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c5d85b3195fb97c"></a>[`public uint Inno.Native.Bgfx.bgfx.Resolution.width`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L834) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b23c7eebb0d4a67"></a>[`public Inno.Native.Bgfx.bgfx.Resolution.Resolution(Inno.Native.Bgfx.bgfx.TextureFormat formatColor = Inno.Native.Bgfx.bgfx.TextureFormat.BC1, Inno.Native.Bgfx.bgfx.TextureFormat formatDepthStencil = Inno.Native.Bgfx.bgfx.TextureFormat.BC1, uint width = 0, uint height = 0, uint reset = 0, byte numBackBuffers = 0, byte maxFrameLatency = 0, byte debugTextScale = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L840) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-05b0a1ec9647502a"></a>[`public struct Inno.Native.Bgfx.bgfx.Resolution`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L830) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.ResolveFlags</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.ResolveFlags 8e1f1ee3ebeb586482039b07f2e7e4c4dc1ec4649b3dcf65f9ff99933aac0280 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResolveFlags.None 6a7c2e01befcc2fa475cbd7532dcc1a37d8aeafaf7b57c96e07a92e48c443891 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ResolveFlags.AutoGenMips 84fb663a1e85509ad6ed8110a1776ff78d41abe6357607922921c42648e2f4e8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0ad68a4f93167c8d"></a>[`Inno.Native.Bgfx.bgfx.ResolveFlags.AutoGenMips = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3578) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d126f167040d0361"></a>[`Inno.Native.Bgfx.bgfx.ResolveFlags.None = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3577) | 当前常量或枚举值：`0`。 | — |
| <a id="api-75cfd03a4e5c6f76"></a>[`public enum Inno.Native.Bgfx.bgfx.ResolveFlags : uint`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3575) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.SamplerFlags</code> · 48 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.SamplerFlags 3a05819da2782f53e6e64f0e7bee65459995f08df3b442e4b823b3ce0fcea2d2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.UMirror f7056bda49b9651346863f8ece10bf4d46dd436cefc1e90479bdf4cfa058aed4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.UClamp 98e5d9a9a4bca880680ef36a5b8831a9bb4613b46619392883afe8d4213f5d4f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.UBorder 6964ecb5df5180d8c10b05c9f4b4b5675a6af9e1b95f01c8a4267ed8fc99b07d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.UShift 3a1a3a9291df46cdc1b730c9bf7d97898011de39433cecbcf7e2204d429ec041 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.UMask dd5b726c93f1cddf75e021f1583c4e5d6290876009387881a8d19104a7029545 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.VMirror d856be36f818ffccc6f323e739cb9462641dd8dca484323b5e300afd239cfecb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.VClamp faff15c2543b87eaeabd0e5122b097cb4fea02424257e37a8cfae1e01e58ad09 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.VBorder 807c4f6f760e57c38983200dd164800b441da09655762adf9f0b5d48aa37ad30 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.VShift 54c75d1e5fb5ba16e3e1dba2e3c86629a85dca95f29a9eac1a98c26ed10e30b5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.VMask 4aa12d7e394df8a4b09333ea2cb20db000fdcbcbfa35d648e537e45bbefe1990 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.WMirror ad1fcf1437d832303341b93a0ccc2830a1ceb83058195284e52d532c580f1bbc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.WClamp b743b427a787b90500e7e19b99fd1936cb5e2c54d42217bab6e1a7c7ea624f8b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.WBorder fe3d8ae8f0d87f643ea54a4367e3e3b36cefd8542e25ca7d6623d9006265d17e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.WShift 4a58fc00f6e2bfce887fe4437a5fbe37f68e60062b1e5983957a24605a05135d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.WMask 341d5c306fb1cba216f2d57f18e79efe737a075a71ecf434dbec9c00983e24ee -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.MinPoint 87313330cbe3f2a78c2607b95217a490047233b685d6b118640003e508ddad8c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.MinAnisotropic c3d2a2e00cc56f6b36c97479c288df4ed3d6770410b28c5ce67b92eff063abfb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.MinShift b096b55e76e2edbce3ac2bba368f7ee4e45674d25ebbb50e81569334ba759601 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.MinMask 9ac1873739aa2bd6a81aee6b74498bb174499af4b36cf82a4aaa8052d23535be -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.MagPoint 7993d5da90352bff6c9e5cb8e94ecb06bab210e195de8b725b7d9d1067d2cd42 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.MagAnisotropic 14f168c7f60f07eccfdc234040d92ed3ff4ab2e1cbbfa3d92346ff0e2856b54b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.MagShift 425d81fbc30a99fd1407a072b92d431f109942c0034aafd69422c9f4ebe0526a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.MagMask 206f3e7a4d25e6af9ff7809f792ed4f8ed58a4eca6aa6e8457ac963055d5718a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.MipPoint 1a3920f602e889889fe4ba37520d7f81cc6dea54f0c7b7a6951b1cdf2106c4c5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.MipShift 805a620ff9194ea8bb544b5323b6c55990ea0cab66e1413726252a052e659d26 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.MipMask d9b0ebb58065358f45fbb9bd2ac0af1557ef690512d59890ecfc68ef1eed9336 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLess 9304d3d9bab3b7ef0d700ba8b5ffa15693227a1e175349e76280db9b6b8f1d9e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLequal f60466dddfc6ab3a529f9678a9942835c665c365c6b4eb4b2d5394e2c569c044 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.CompareEqual 117db36cbbc112018d72347ce2d669f069f8199490b500f490817951d0d65e78 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.CompareGequal f61725638e23295a2a10a956fbc22b51df77c5bd03d53b74dca5f12c615691f4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.CompareGreater f9b576a856558752fcaa24dd4dc9c9295c6aa599a862de3b6645959c0edfda3f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.CompareNotequal 9745e17fcb7969181e0e6d8663cf8381b5d681dc688133826b944f6cff3c985e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.CompareNever 2b4e8cbf2779865b474b6cd356fd39111389cfde4dbbcebb9c7b7700ecf24373 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.CompareAlways 81297572e8db9a48aab1164ef6b682b0188d486273cb87c5ecaddc1cb4d051a1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.CompareShift 2f8033a081a1d8b31ed995abeaf6f93cfa44c950e9bd5a577c59173e8abd68cb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.CompareMask baf378af2e9d09be8f68f868ee2607c595e3d160d6d03f4e085e4ef70d5c0fdc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.BorderColorShift 00ce0ee78e73d110e6080d76a8e529c0bc599b90b6a4c21a162ae006ed3fb708 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.BorderColorMask da8001c0f5fe57dc56c28f5259abb4f2ae76a26fed30769b3b74fd8ea572be51 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.ReservedShift 2659741aa5703d6fee396bb4ffa046b4224335226ce714098d8203d0a58a1d72 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.ReservedMask 46c2b24a8ca4e772d52cbcb3ad301c1ee32cfbb0b6a19c222fddd7bffe6cc79d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.None 1c77b7bd749b276941b7ae172e252f8d991fda3be572655241f736988e3cef57 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.SampleStencil 1dc8cf3ec3875869d36006b980406a4dc0a9a4c5faa8f9f30417367b6d4ebc59 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.Point 77dafda71fa20608695e0aa59576653c2736101dabb28a068ade482aa57f0c11 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.UvwMirror 6e055f69d6cad8f6c6e1b7b2f7d78e4e15dcfc50868a9eed183de9a48a7b70ed -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.UvwClamp eef0905088c6a72f360ff50cbce7dd2ceddce54fa364126023b3c9d0ee5d64ec -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.UvwBorder f91f306b1ed5d5fc2a1b900da63600eb14bc6b4dcddd78d8ad5e9c233837da5e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.SamplerFlags.BitsMask ae796c7dcd40c6ff58d1bdfd2ea82c021140ba260e569b96dbab6814ac02f3bf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b07b1cb4dc2b1e58"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.BitsMask = Inno.Native.Bgfx.bgfx.SamplerFlags.UvwBorder &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MinAnisotropic &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MagAnisotropic &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.Point &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareMask`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3636) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.UvwBorder &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MinAnisotropic &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MagAnisotropic &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.Point &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareMask`。 | — |
| <a id="api-6190fcc4f1bcb171"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.BorderColorMask = 251658240`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3627) | 当前常量或枚举值：`251658240`。 | — |
| <a id="api-fabe60b8d2d895e1"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.BorderColorShift = Inno.Native.Bgfx.bgfx.SamplerFlags.MagShift &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareShift`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3626) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.MagShift &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareShift`。 | — |
| <a id="api-17be061043634c88"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareAlways = 524288`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3623) | 当前常量或枚举值：`524288`。 | — |
| <a id="api-0a3eb303cb6b85df"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareEqual = Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLess &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3618) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLess &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLequal`。 | — |
| <a id="api-3ebce05b0cf04780"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareGequal = 262144`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3619) | 当前常量或枚举值：`262144`。 | — |
| <a id="api-65d715c90dd66b2c"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareGreater = Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLess &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareGequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3620) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLess &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareGequal`。 | — |
| <a id="api-14068274461cff8f"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLequal = 131072`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3617) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-0de4f76288bf2990"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLess = 65536`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3616) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-2fc53a5a1736f685"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareMask = Inno.Native.Bgfx.bgfx.SamplerFlags.CompareNever &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareAlways`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3625) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareNever &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareAlways`。 | — |
| <a id="api-8fbc7d2f882748d4"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareNever = Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLess &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareNotequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3622) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLess &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareNotequal`。 | — |
| <a id="api-43b63747a853ec6a"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareNotequal = Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLequal &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareGequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3621) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareLequal &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareGequal`。 | — |
| <a id="api-730c5e9f0d504208"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareShift = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3624) | 当前常量或枚举值：`16`。 | — |
| <a id="api-b3030e1dd9d3cbc5"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.MagAnisotropic = 512`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3610) | 当前常量或枚举值：`512`。 | — |
| <a id="api-69feb6974e48f548"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.MagMask = Inno.Native.Bgfx.bgfx.SamplerFlags.MagPoint &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MagAnisotropic`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3612) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.MagPoint &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MagAnisotropic`。 | — |
| <a id="api-a96715f2d69ac1eb"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.MagPoint = 256`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3609) | 当前常量或枚举值：`256`。 | — |
| <a id="api-c61d23fb3c1c6d04"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.MagShift = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3611) | 当前常量或枚举值：`8`。 | — |
| <a id="api-271a05ebe5d63bce"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.MinAnisotropic = 128`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3606) | 当前常量或枚举值：`128`。 | — |
| <a id="api-14106e5032053f78"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.MinMask = Inno.Native.Bgfx.bgfx.SamplerFlags.MinPoint &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MinAnisotropic`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3608) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.MinPoint &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MinAnisotropic`。 | — |
| <a id="api-fd964ae059b60d05"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.MinPoint = 64`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3605) | 当前常量或枚举值：`64`。 | — |
| <a id="api-0bae1c04ac4381ca"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.MinShift = Inno.Native.Bgfx.bgfx.SamplerFlags.UClamp &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.VMirror`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3607) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.UClamp &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.VMirror`。 | — |
| <a id="api-9dde20b03caa2e40"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.MipMask = 1024`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3615) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-05d469af2a4bde94"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.MipPoint = 1024`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3613) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-a4d60014699e09a5"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.MipShift = Inno.Native.Bgfx.bgfx.SamplerFlags.UClamp &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MagShift`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3614) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.UClamp &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MagShift`。 | — |
| <a id="api-5139ae31a81a34cf"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.None = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3630) | 当前常量或枚举值：`0`。 | — |
| <a id="api-a333559756fc8f68"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.Point = Inno.Native.Bgfx.bgfx.SamplerFlags.MinPoint &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MagPoint &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MipMask`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3632) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.MinPoint &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MagPoint &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MipMask`。 | — |
| <a id="api-666cdc718ab2cabe"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.ReservedMask = 4026531840`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3629) | 当前常量或枚举值：`4026531840`。 | — |
| <a id="api-68a91e8dda44667e"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.ReservedShift = Inno.Native.Bgfx.bgfx.SamplerFlags.VMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.BorderColorShift`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3628) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.VMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.BorderColorShift`。 | — |
| <a id="api-fc08643fafe8f0f1"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.SampleStencil = 1048576`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3631) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-f74d13935030fddc"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.UBorder = Inno.Native.Bgfx.bgfx.SamplerFlags.UMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.UClamp`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3592) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.UMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.UClamp`。 | — |
| <a id="api-a8f6817ff8d66af8"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.UClamp = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3591) | 当前常量或枚举值：`2`。 | — |
| <a id="api-f89e196372dbbd2b"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.UMask = Inno.Native.Bgfx.bgfx.SamplerFlags.UMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.UClamp`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3594) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.UMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.UClamp`。 | — |
| <a id="api-1a86ef89ec72def3"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.UMirror = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3590) | 当前常量或枚举值：`1`。 | — |
| <a id="api-0b2f50c2ebc84e18"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.UShift = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3593) | 当前常量或枚举值：`0`。 | — |
| <a id="api-092620e07f693629"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.UvwBorder = Inno.Native.Bgfx.bgfx.SamplerFlags.UBorder &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.VBorder &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.WBorder`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3635) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.UBorder &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.VBorder &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.WBorder`。 | — |
| <a id="api-e7ab087ecdc4e411"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.UvwClamp = Inno.Native.Bgfx.bgfx.SamplerFlags.MipShift &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.WClamp`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3634) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.MipShift &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.WClamp`。 | — |
| <a id="api-2c64e370df169041"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.UvwMirror = Inno.Native.Bgfx.bgfx.SamplerFlags.UMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.VMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareShift`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3633) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.UMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.VMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.CompareShift`。 | — |
| <a id="api-205cf9b066f550b6"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.VBorder = Inno.Native.Bgfx.bgfx.SamplerFlags.VMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MagShift`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3597) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.VMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MagShift`。 | — |
| <a id="api-c21e3a35453f74de"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.VClamp = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3596) | 当前常量或枚举值：`8`。 | — |
| <a id="api-742993428a04d817"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.VMask = Inno.Native.Bgfx.bgfx.SamplerFlags.VMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MagShift`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3599) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.VMirror &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.MagShift`。 | — |
| <a id="api-203626db1b0680fc"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.VMirror = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3595) | 当前常量或枚举值：`4`。 | — |
| <a id="api-dbc623b5d4a9ed64"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.VShift = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3598) | 当前常量或枚举值：`2`。 | — |
| <a id="api-1db843e59dde5b9c"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.WBorder = Inno.Native.Bgfx.bgfx.SamplerFlags.CompareShift &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.WClamp`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3602) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareShift &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.WClamp`。 | — |
| <a id="api-5bc3a747f0e05304"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.WClamp = 32`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3601) | 当前常量或枚举值：`32`。 | — |
| <a id="api-d8cfeda43c59ddc9"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.WMask = Inno.Native.Bgfx.bgfx.SamplerFlags.CompareShift &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.WClamp`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3604) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.SamplerFlags.CompareShift &#124; Inno.Native.Bgfx.bgfx.SamplerFlags.WClamp`。 | — |
| <a id="api-192b763068df6c93"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.WMirror = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3600) | 当前常量或枚举值：`16`。 | — |
| <a id="api-afceb395097304a2"></a>[`Inno.Native.Bgfx.bgfx.SamplerFlags.WShift = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3603) | 当前常量或枚举值：`4`。 | — |
| <a id="api-f778625742870c80"></a>[`public enum Inno.Native.Bgfx.bgfx.SamplerFlags : uint`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3588) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.ShaderHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.ShaderHandle 08383e5c8d793b6a682e334999b4ffb4199d7adf232754832493776c42ca734b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ShaderHandle.idx 8c1179093d7229c6fef55bfaa34e56b84bc3fbe830032ebf3354812364cf4901 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.ShaderHandle.Valid 195a298b1e064f712d410348679fe868bf73d8c7b257623740a874b26c2e6e15 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.ShaderHandle.#ctor(System.UInt16) cd6a46196ccc15760133373bab2a61dc94610850f89b58985d6e4339779af977 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4169348c2c576b03"></a>[`public ushort Inno.Native.Bgfx.bgfx.ShaderHandle.idx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2181) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6bc8254f501e0c3d"></a>[`public Inno.Native.Bgfx.bgfx.ShaderHandle.ShaderHandle(ushort idx = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2184) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a003321ba2073665"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.ShaderHandle.Valid { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2182) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2f69152221706e17"></a>[`public struct Inno.Native.Bgfx.bgfx.ShaderHandle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2179) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.ShadingRate</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.ShadingRate 30b733d82e7b0f4c6730408ba75a19d92524e76194cfc4847cb58d71ae4bc876 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ShadingRate.Rate1x1 4b09a85f7be226f46f29a23b429a113b5b737916963ad924cea14f39ef6d9b57 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ShadingRate.Rate1x2 ddfe6cedf82291aba1c77be8b9d5cc3d3d5a14d68299387ffb9003dd5589ad8d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ShadingRate.Rate2x1 779205bd42a71a2e36a8036a73f748c18ef3ae7b3974729634a50c97b4237ab7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ShadingRate.Rate2x2 22d83c22690f404fbee0ab686dcf86f55af679a4fd95e30d13f45930261ca1c2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ShadingRate.Rate2x4 66f300adb9a74971a5d75dbf046da96b136f10907c446618daf8f6a128d91e5b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ShadingRate.Rate4x2 456cc0d20ce6fa50e371e80582e4b2079a2155db5f0862ef100ddd79b13bd83d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ShadingRate.Rate4x4 26665ed0cfaaab72d1ae6fd7f1e07386134b893d4caba9af94217d78b832d89d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ShadingRate.Count aa1cc6bdb857b497f2ac7805886c47d6e8204ebc675b2f1c5ae62b9e7e77664e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e9835d6c03ac9285"></a>[`Inno.Native.Bgfx.bgfx.ShadingRate.Count = 7`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L374) | 当前常量或枚举值：`7`。 | — |
| <a id="api-1621d140603c5c7e"></a>[`Inno.Native.Bgfx.bgfx.ShadingRate.Rate1x1 = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L367) | 当前常量或枚举值：`0`。 | — |
| <a id="api-c2869bc4cdef7b09"></a>[`Inno.Native.Bgfx.bgfx.ShadingRate.Rate1x2 = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L368) | 当前常量或枚举值：`1`。 | — |
| <a id="api-b6295e60eac9680f"></a>[`Inno.Native.Bgfx.bgfx.ShadingRate.Rate2x1 = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L369) | 当前常量或枚举值：`2`。 | — |
| <a id="api-a06320676f25d999"></a>[`Inno.Native.Bgfx.bgfx.ShadingRate.Rate2x2 = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L370) | 当前常量或枚举值：`3`。 | — |
| <a id="api-6b4ec701e06e87f8"></a>[`Inno.Native.Bgfx.bgfx.ShadingRate.Rate2x4 = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L371) | 当前常量或枚举值：`4`。 | — |
| <a id="api-955af9b4f6b855de"></a>[`Inno.Native.Bgfx.bgfx.ShadingRate.Rate4x2 = 5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L372) | 当前常量或枚举值：`5`。 | — |
| <a id="api-c0bdee21093f8227"></a>[`Inno.Native.Bgfx.bgfx.ShadingRate.Rate4x4 = 6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L373) | 当前常量或枚举值：`6`。 | — |
| <a id="api-070dd0e27384a29b"></a>[`public enum Inno.Native.Bgfx.bgfx.ShadingRate : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L365) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.StateFlags</code> · 65 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.StateFlags 722eeffdc868d8759f2e8122400c84c4ded5261f745e2198947572a43f75d1db -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteR 5f1c4910766e97114984aaf1345337e7e09f705d2ee57e19e896f257f0ebd337 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteG d369c91355f084832fdfe26922080aa6ea9f3d1b66eff635ac6d92b6b2577be3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteB bbcd49c9db451b0a78204e1524fbf6c080b081970ef3084ad8cc5f32f037d954 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteA a2e3842c5ae616f7817e0f3bdf43d8ae6de07a5ab764a7266a07d7f7a41ea0c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteZ cc1e18e008013193e148d76cc8ca4adfff54a416c7198608e13ed4f783fff3d1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteRgb 4b37ae4ad753767e86931858a416920e3c2cd540e85d398b5bc25c6e60a8f38f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.WriteMask ff57585da60fb5b499f4ca58554bbed8f300b03f06f0b8f9e01f0a5cbe1700ee -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess 7c1ad9b1a47aaa339a7099c04bb5598a3b8051021fa1876efb361dca0dba2b91 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal 4bc72141d54bcbf04883fe064b57864b901939bcfc429358a9958ad6e251f19c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestEqual 91e350c21716d1b13cf02e897c240609ef249739bb550a1728d651f5a1c5d940 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGequal e0cb4f92448c65de89841198567f4942b126172e2c6aa6db54256b8e972b21d9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGreater 574e284d36ec9a81b5bfbef7c070858a2a2fd9f07f11d8836a0fd2108c255df8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNotequal c16f79ddfef1ce67232c60235adad5215619e16b77ba398b027923e899605f5f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNever 27ec0f3e6552f0747eece2454efdd1b91545960f28bc16241281a2a9309398c1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestAlways 4fbe7d219d56415222e164371dda4c904c6e90075af235d6cd864db14814d98c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift 3bbc59618e9d0b86d3bc422e30c466e61941a792f4e534a1ac5ffd5945666c19 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.DepthTestMask a1832dbb39b61f6a0a455d739578bff58988f762e25ef30d26ebb2a7a5488d03 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendZero 7cdd2be0e145863f59e69fb3b7257a0b88e6b7a0e3a7d5f4739cf89d373d817f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendOne 863a27f1cf65e8dbff01d805003c06d626e57a9e304260dfa014245127da5e0f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendSrcColor 3752e140b78ce9e3cd3a0c13f52176a54867462ce9a42c25b6fbe167e4a373eb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcColor 89b995f535bdbbca09886e5012a6002285a999718d1ce924391e6f9ffda0cf41 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendSrcAlpha 828918700e97bbd274c335c95f0905114d3329097da73691a0d8dcbe1303b568 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcAlpha 0127d7d285b7502adadc7c25595e8a17ecd2b1311b2f58888a063ebbe75c5c0e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendDstAlpha 70276e7f033fc52f1e1b7b22deefe29576d0c04e7f46f43f096990d1130e779d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendInvDstAlpha da2e05b4e452dc4233b617f16794bdff0b9f60911701661267b8a94a60c670de -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendDstColor bc5a84ff8c1a1a926d933f37a3bb00f383697c5acb6f36d6c339d7240be86df1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendInvDstColor a4698268347262eeddbf42c57b2d72df0002524133e7ab8e9de28a925e3ababd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendSrcAlphaSat ac3278556ebe68e2c5e6c1a08fc45085eafce49fa9cd0d141c3a4a40b3458774 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendFactor 43cae11397adeed70a02843c4bd5f0bc443f09aa13663bf63bd0a83b812e7e9a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendInvFactor 10c089020a3ee42a7cbf1c75bb9e5b9f77d96213a19768c383e640c802e4dc98 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendShift 3c80afe6a9855bac76acf453809ae10d355138e7c8669f0f1a9feb754ad3debf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendMask dc11506694303c38a7ba221bd451ae91c474c802c5c066611cb9be84acb9f5c5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationAdd d8e56f049fa3e3bc89bc0c7c94dd27b50a2f1cb78f5cb0ee412b916be0705d33 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationSub 95b49884019b3bc6723c3af52c135174f17dd722bb43ce6c8d9e0fe947fcef0b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationRevsub e19892860a1d2a2e8969d16db7aaa8c796c7096e70a670bb5fba4fa40656d24e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationMin 1c0b992c2c03f1ef83b2cc24b9606b5cfe9a4209b2ced0792a35e2c8ed468575 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationMax 8a769898feb3805b2d0fa185fc85ac40c4777c273fdc48a3baff48d33c6471c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationShift a60e13be10c7e4d31e2a0d4f3b588a461ee1106466a04a0e0157e3df70a54b0f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationMask 15e1cf7d67e09f657da883b77f455451e76c75b6522d513b3cf7119a2cb29847 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.CullCw f4e2e8684f0ef53fdd77a5cdf5580a078351b6a6ffc72efe08505233e821de4b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.CullCcw e551c9486973c2cd94ac3f034190530b4fd990611e4da63d47b5bdf82783ddae -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.CullShift 8fe835e2aef7b4253a12fa40734322b275c8e2390a15923f1c90720fd5e4cf8e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.CullMask 0c6a87cc37e8366a395cd063f985f2fda45533caa7f962a03f52c8b73ddbdc76 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.AlphaRefShift 49ff76e55ade61b37923aeab799654c15a3ae373aaaa9c1c608267496b775c11 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.AlphaRefMask d8c9c76b76d3b97e8d0480ac0477d70e03d827d2f444fcdbf95480718deaa600 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PtTristrip 9c79671d2eead2f45563637482f9a2c85c0b23bb6bcad35e5095364c6b33e8f8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PtLines 1195c30df7ec757cf31952df6fbabf9eb7a593eb0c091946f12a52c7456146a8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PtLinestrip b9944dfeff0bc11f8208e3b16aa53f03f0e10a9954e176c6ee45606075da911a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PtPoints a9a077e88bd1d756131d00c54e61546a15bae3f0d022ed7d5a37e71ba8318dcc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PtShift 0441077150279155a1906bb58144d5886640040597ad9ddce62441cee99e8a3a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PtMask 4452875cc205eca1ec8bc961ec4056a3fed4741c1fc179178e81be6695744412 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PointSizeShift 71057d5d2a770ea2619273aecc7b6c9863c319ae92b9d5062076c7bc4fb6af8d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.PointSizeMask e3ee99db75a07a837be6105f237d1e7a42e654f4a2925f98c0173803a2238b78 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.Msaa 73857ae5e7393d1f80994bff68ded3cc89f85de0ac33dd8e68f0de40ec1eaade -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.Lineaa e93f9cd3eafaf4a333ea46291607a9c827d3bc37d3510f552e2bcbceb4194994 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.ConservativeRaster e51f7506e8e2cd6142ad84fda6bd6ccb4666d9e4de37b36977a381aa412b3e2d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.None 08c0808e038bb200666208e95c651c2cd0a5c1df06120098550becd2569475e7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.FrontCcw 19d3cf42fda8c109b72a7c8f7de35b88ace855d92f18800d1aab96901a135391 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendIndependent 4f4c8026e9b6e284a4da385c5b212616ea40608f3bc777436684d658c066bc32 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.BlendAlphaToCoverage e6ad860adb3c70592477db10cf5898d3a8c38fb3d897471a1df8b946ce9edd76 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.Default 079a757c4fbde83f8b23673c57d9103d0b814f64f2fe9029a5d1184c670488ac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.Mask eb87da757e445a55866ed15c17d066e11efb9e5d6a824f738d3c54d9dddb5bd8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.ReservedShift 5b994ab97c27fd9c75ea807a217d2e716cb9a356458cde7aa40560ffe5856ab1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StateFlags.ReservedMask 23bd59ec6982e784e7ed0ba8088a509786871f6f5b6c53d676ca72edb03bbb2c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2895e45285af4362"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.AlphaRefMask = 280375465082880`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3692) | 当前常量或枚举值：`280375465082880`。 | — |
| <a id="api-4d09038f5ef28eba"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.AlphaRefShift = Inno.Native.Bgfx.bgfx.StateFlags.WriteA &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3691) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.WriteA &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal`。 | — |
| <a id="api-38fefe7b10b77802"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendAlphaToCoverage = 34359738368`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3707) | 当前常量或枚举值：`34359738368`。 | — |
| <a id="api-d3e26a63f5588294"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendDstAlpha = Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcAlpha`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3671) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcAlpha`。 | — |
| <a id="api-14f630e2af32aadb"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendDstColor = Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvDstAlpha`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3673) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvDstAlpha`。 | — |
| <a id="api-e77bef5a1969d3b3"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationAdd = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3680) | 当前常量或枚举值：`0`。 | — |
| <a id="api-f9b32d540273132b"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationMask = 16911433728`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3686) | 当前常量或枚举值：`16911433728`。 | — |
| <a id="api-afdc49a58c72e039"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationMax = 1073741824`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3684) | 当前常量或枚举值：`1073741824`。 | — |
| <a id="api-5c703bbb1ade51fa"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationMin = Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationSub &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationRevsub`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3683) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationSub &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationRevsub`。 | — |
| <a id="api-57008da2d9a8e198"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationRevsub = 536870912`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3682) | 当前常量或枚举值：`536870912`。 | — |
| <a id="api-8e777868b226dca7"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationShift = Inno.Native.Bgfx.bgfx.StateFlags.BlendShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3685) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.BlendShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess`。 | — |
| <a id="api-95ef2578742b6300"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendEquationSub = 268435456`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3681) | 当前常量或枚举值：`268435456`。 | — |
| <a id="api-00815a6b842d5f04"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendFactor = Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcColor &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvDstAlpha`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3676) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcColor &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvDstAlpha`。 | — |
| <a id="api-e0d697b9b48d9232"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendIndependent = 17179869184`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3706) | 当前常量或枚举值：`17179869184`。 | — |
| <a id="api-60f37060f49b2593"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendInvDstAlpha = 32768`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3672) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-8db853ac73de55d9"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendInvDstColor = Inno.Native.Bgfx.bgfx.StateFlags.BlendOne &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvDstAlpha`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3674) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.BlendOne &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvDstAlpha`。 | — |
| <a id="api-56aef5508110e2bc"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendInvFactor = Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendFactor`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3677) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendFactor`。 | — |
| <a id="api-234b1b04931f45d4"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcAlpha = Inno.Native.Bgfx.bgfx.StateFlags.BlendOne &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcColor`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3670) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.BlendOne &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcColor`。 | — |
| <a id="api-c61a5298819224b7"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcColor = 16384`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3668) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-4b684427bf8e7f76"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendMask = 268431360`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3679) | 当前常量或枚举值：`268431360`。 | — |
| <a id="api-0bcc8d24afc3fc7b"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendOne = 8192`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3666) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-a8b00db65fb0fdf6"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendShift = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteA`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3678) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteA`。 | — |
| <a id="api-56c3baa2b6d7e8b0"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendSrcAlpha = Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcColor`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3669) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvSrcColor`。 | — |
| <a id="api-77cf3ddbfc6b7d6b"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendSrcAlphaSat = Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvDstColor`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3675) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendInvDstColor`。 | — |
| <a id="api-cec26b3c0f344950"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendSrcColor = Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendOne`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3667) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.BlendZero &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendOne`。 | — |
| <a id="api-204792f493e2eaf3"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.BlendZero = 4096`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3665) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-df5d3d434a493523"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.ConservativeRaster = 288230376151711744`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3703) | 当前常量或枚举值：`288230376151711744`。 | — |
| <a id="api-c24f1d378a5a62a3"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.CullCcw = 137438953472`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3688) | 当前常量或枚举值：`137438953472`。 | — |
| <a id="api-c50985eae6077a7f"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.CullCw = 68719476736`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3687) | 当前常量或枚举值：`68719476736`。 | — |
| <a id="api-778e027c8d25b8fb"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.CullMask = Inno.Native.Bgfx.bgfx.StateFlags.CullCw &#124; Inno.Native.Bgfx.bgfx.StateFlags.CullCcw`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3690) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.CullCw &#124; Inno.Native.Bgfx.bgfx.StateFlags.CullCcw`。 | — |
| <a id="api-4105a10e1aca43d3"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.CullShift = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3689) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal`。 | — |
| <a id="api-7c3be8b39041cd86"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.Default = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.CullCw &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteMask &#124; Inno.Native.Bgfx.bgfx.StateFlags.Msaa`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3708) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.CullCw &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteMask &#124; Inno.Native.Bgfx.bgfx.StateFlags.Msaa`。 | — |
| <a id="api-17ba048027a41408"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestAlways = 128`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3662) | 当前常量或枚举值：`128`。 | — |
| <a id="api-a5b676318437fe06"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestEqual = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3657) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal`。 | — |
| <a id="api-5649a4c4c8bfb1ba"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGequal = 64`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3658) | 当前常量或枚举值：`64`。 | — |
| <a id="api-e5d945fd06a9cd4b"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGreater = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3659) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGequal`。 | — |
| <a id="api-802e4ebfd85f3ee9"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal = 32`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3656) | 当前常量或枚举值：`32`。 | — |
| <a id="api-9ad6bf9fede4599d"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3655) | 当前常量或枚举值：`16`。 | — |
| <a id="api-9fa75edcb7b899ef"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestMask = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNever &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestAlways`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3664) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNever &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestAlways`。 | — |
| <a id="api-fdf2bca17c7bd753"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNever = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNotequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3661) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNotequal`。 | — |
| <a id="api-9851b42f8e395328"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestNotequal = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3660) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestGequal`。 | — |
| <a id="api-c8de688f1e89340f"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3663) | 当前常量或枚举值：`4`。 | — |
| <a id="api-cd55a03141ebd1b1"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.FrontCcw = 549755813888`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3705) | 当前常量或枚举值：`549755813888`。 | — |
| <a id="api-22b2eee58b334496"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.Lineaa = 144115188075855872`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3702) | 当前常量或枚举值：`144115188075855872`。 | — |
| <a id="api-a5748239768e7575"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.Mask = 18446744073709551615`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3709) | 当前常量或枚举值：`18446744073709551615`。 | — |
| <a id="api-65fefcab49295ddf"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.Msaa = 72057594037927936`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3701) | 当前常量或枚举值：`72057594037927936`。 | — |
| <a id="api-dee3bc3c4e708c19"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.None = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3704) | 当前常量或枚举值：`0`。 | — |
| <a id="api-1cf5a76f0650a51d"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PointSizeMask = 67553994410557440`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3700) | 当前常量或枚举值：`67553994410557440`。 | — |
| <a id="api-d07fce8da8a62579"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PointSizeShift = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestEqual`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3699) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestEqual`。 | — |
| <a id="api-62212de36a8dee7b"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PtLines = 562949953421312`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3694) | 当前常量或枚举值：`562949953421312`。 | — |
| <a id="api-df22e9a9a82abfcd"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PtLinestrip = Inno.Native.Bgfx.bgfx.StateFlags.PtTristrip &#124; Inno.Native.Bgfx.bgfx.StateFlags.PtLines`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3695) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.PtTristrip &#124; Inno.Native.Bgfx.bgfx.StateFlags.PtLines`。 | — |
| <a id="api-d28bfd8d10302c3f"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PtMask = Inno.Native.Bgfx.bgfx.StateFlags.PtLinestrip &#124; Inno.Native.Bgfx.bgfx.StateFlags.PtPoints`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3698) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.PtLinestrip &#124; Inno.Native.Bgfx.bgfx.StateFlags.PtPoints`。 | — |
| <a id="api-dded7568e697c7d6"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PtPoints = 1125899906842624`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3696) | 当前常量或枚举值：`1125899906842624`。 | — |
| <a id="api-07728a0b8bd92ce4"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PtShift = Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3697) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLess &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestLequal`。 | — |
| <a id="api-861bd87f009fcd42"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.PtTristrip = 281474976710656`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3693) | 当前常量或枚举值：`281474976710656`。 | — |
| <a id="api-9d2ffaab99116fb8"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.ReservedMask = 16140901064495857664`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3711) | 当前常量或枚举值：`16140901064495857664`。 | — |
| <a id="api-d9cf56c6eff1d46e"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.ReservedShift = Inno.Native.Bgfx.bgfx.StateFlags.WriteR &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteA &#124; Inno.Native.Bgfx.bgfx.StateFlags.PointSizeShift`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3710) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.WriteR &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteA &#124; Inno.Native.Bgfx.bgfx.StateFlags.PointSizeShift`。 | — |
| <a id="api-fafc0b62e00d93c5"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteA = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3651) | 当前常量或枚举值：`8`。 | — |
| <a id="api-8f5195dac13685fd"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteB = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3650) | 当前常量或枚举值：`4`。 | — |
| <a id="api-1dd82952afbbe45f"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteG = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3649) | 当前常量或枚举值：`2`。 | — |
| <a id="api-48a7d3b0dd2e48df"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteMask = Inno.Native.Bgfx.bgfx.StateFlags.WriteR &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteG &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteZ`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3654) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.WriteR &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteG &#124; Inno.Native.Bgfx.bgfx.StateFlags.BlendShift &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteZ`。 | — |
| <a id="api-2c876eb49303b4a4"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteR = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3648) | 当前常量或枚举值：`1`。 | — |
| <a id="api-0d18361b966edb4d"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteRgb = Inno.Native.Bgfx.bgfx.StateFlags.WriteR &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteG &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3653) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StateFlags.WriteR &#124; Inno.Native.Bgfx.bgfx.StateFlags.WriteG &#124; Inno.Native.Bgfx.bgfx.StateFlags.DepthTestShift`。 | — |
| <a id="api-074cfed13df902d2"></a>[`Inno.Native.Bgfx.bgfx.StateFlags.WriteZ = 274877906944`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3652) | 当前常量或枚举值：`274877906944`。 | — |
| <a id="api-38009012f74005dd"></a>[`public enum Inno.Native.Bgfx.bgfx.StateFlags : ulong`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3646) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.Stats</code> · 47 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.Stats 17f47c718c4be6ab6c45f3d19e1862d158a39aa99e8119227b749897680fe759 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.cpuTimeFrame 6ee1fc2ea4dff1113181008fa7ba188b6dd43739227c9a8c1cb0ddb42c94d26d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.cpuTimeBegin 9419728bda5960691b14c2662e504627a974a065618cc3ec014c5f01b0c621e1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.cpuTimeEnd ffabfb887c33f2b322396bb5b1916371b1ead108cc59cff2ef26818ca07779e8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.cpuTimerFreq 54644f580f2657c078571db74cbbf908b850cb30b41dfe59d06dfa0cf78bf4ed -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.gpuTimeBegin 3c0b51e5383cec8e3aaec6053160adfd08142e4423348628cc4bc195ccdadfc2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.gpuTimeEnd 840459a5c99552c4fbc7bc728d90bb72d4cd1ddf9e8a3b132e62ed31920d06dc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.gpuTimerFreq a928763f79848ad81d54b4c86d46f2a34ff7d525a5830bdda86edb468d40d54e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.waitRender b9fb3b1a118d63d63fefd9237233645848830b7647eec08be6cd7c71dae1687d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.waitSubmit 428266567c38f0267a8643b4636e0f7195f4331d1aba302f48187060fbeaa7d9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numDraw 0c42df5f18a94f815eaeacfe54b4e27559e20b0301ae609c0ddb5de7cad2681a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numCompute 68b1bc6d3998cc648d3ae826be64f1794f5799b6ffe353fe9448d9be833d1400 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numBlit 615512f13c1afc0fba7ba88ccf916a6c228d90e29d177c56161a082539c03b97 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.maxGpuLatency 566673abf94af68ad1d971476fbe503ebfa4894fba9951c8d248c40bf2b24805 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.gpuFrameNum 176ce586f947b1e5c82809aa0da40a7bafb5dbad13c2fdd4550486158bfa5ea0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numDynamicIndexBuffers 8b2aa6fbf4949288b70fea8140e7411502dbc233f0e6bef7762aafda8140c475 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numDynamicVertexBuffers 4e89e696d04093b02cf37aef349ff37a327f921e6db7211618ec99535c9636a5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numFrameBuffers c0e947a58a029945c9029e9d6d52eccbd9db88b31c05ca8b42e40f47cfeb28f4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numIndexBuffers 622518ff073bf8e9b0b921a58acc986d5754194069225f72959b9224029bd649 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numOcclusionQueries 12ad12f24b0e9c15a5ac358f922a2d9e8a91cfa47a5e07a7e3a6417025a8eb55 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numPrograms b2506791b3f4d04d171ff9e644b92b99b4e8b6c638fd7a04cfce0ae77abb8095 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numShaders b506a89538356b2b1712b497c7226accaf95d4db1d40ca6861587590ebdb5063 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numTextures 913d381e8307d0149c455cd7e2a73508a8296bd2602f64a295ef5134cafd8a90 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numUniforms 19a89945b54f5c2589a63982a5d1897d6d6b7aa0b9eb7799537d9378b2f9f5da -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numVertexBuffers 299ba79e91f37a9bd8941a199c9debf0a60c7af3d723ca34349cb41a6295028b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numVertexLayouts 54b236d6bcb98f2960e0b4c423fc0bcfd67c5f494f4afcdcf15cb17e306cd057 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.textureMemoryUsed dba41c5ac02219c48172c38c254b7e1c1476d2e34c466b7ead9a73ce50c17de4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.rtMemoryUsed 6c4d8ab608c6ad8402a5146e5a247d58b0ccb03a0e5f3d11f723578970963b91 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.transientVbUsed bd3daa5ff0a46391646ba4b5d2bd8a420a4b39807ac16cd542edb3ce1fa4a118 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.transientIbUsed 4001af8426652c5cc8b754bde18a8ffb0a25adbc8df5dc0fd20d013c4f30c609 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numPrims_0 0ff8b3e8963624b883acaa130cdbc3fba798dd3fb9f9a55c608eae209389cbc0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numPrims_1 419778f66c2bb989078e7366d0c9515ec8b9f889a15d260be019a6ea29bfa5b4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numPrims_2 4d79a741e7b2c77f388ee689f3e915b720b6336f785dfb3493a7cfc79ef8c319 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numPrims_3 907ab82b4b835c1cc17bd99851efdd6c17f3444338a38b996dbcf8680f2b132b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numPrims_4 d0581fb6466b970bcdb5ab564c33c076d9b135c47b8f0e8d2dee3e77e780bb98 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.Stats.numPrims 2827819aac6b989bb62daa0629e268e2aa0460282a3689045e0734c3110a6f03 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.gpuMemoryMax a6b534f797ecb0a2d3185e45996b70e786e250cf9978e4db98bff09438383bc0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.gpuMemoryUsed a5dcda14ded3728f7aa91625c6be0095d2aad02ca9e305d428ffc7367acbc175 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.width 2911764368b3f8efe9a2f432b06986ba2ff350e00fb9fc4f89fd88d0234c7d9a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.height 42c93fd00c8ce91f5dbe2c1c4f7e8fe67f34a14f1c4684abda76bab88fce75d0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.textWidth 4f89e41f31fe7fe17d7805c8310ebb718d56f4774e545705f072e9851e2726d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.textHeight 0bb7a2e0b6f051636c1a3363e21495291414c42086217318ca8929a3573a8e85 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numViews 43598473298b21c58d973f1614f07abbf7bc9a39e7e925074f73375df7986307 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.viewStats 6a3c39226c6cee4612ec283f12c9eb5d15c720d54041a01018d917f894f953ff -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.numEncoders 60c1df37901f5a216f58a53e60010a305326886ae5009eb5eb0ad5073c1bd1d8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Stats.encoderStats 0c70477a85dd40a94d5b63fc59b9d5b8d3bacd0fa618df39e464bc978b5b4561 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Stats.#ctor(System.Int64,System.Int64,System.Int64,System.Int64,System.Int64,System.Int64,System.Int64,System.Int64,System.Int64,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.Int64,System.Int64,System.Int32,System.Int32,System.UInt32*,System.Int64,System.Int64,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,Inno.Native.Bgfx.bgfx.ViewStats*,System.Byte,Inno.Native.Bgfx.bgfx.EncoderStats*) 1100a2fc9e5477d2de7ef0fa0d9429d77e08a4055080c73e60a4708d7ba19a63 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5fcdd7d2c11538a8"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.cpuTimeBegin`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1872) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe353d14755f3ce1"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.cpuTimeEnd`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1873) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7ba1e78542ccefe"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.cpuTimeFrame`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1871) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8a5fec1f6e0e93c7"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.cpuTimerFreq`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1874) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-91fb4c7d625a4113"></a>[`public Inno.Native.Bgfx.bgfx.EncoderStats* Inno.Native.Bgfx.bgfx.Stats.encoderStats`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1916) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7614848d7bcb2b2c"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.gpuFrameNum`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1884) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b57b25fd5696e495"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.gpuMemoryMax`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1907) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89aba356656c240c"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.gpuMemoryUsed`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1908) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8779b033246b4c7d"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.gpuTimeBegin`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1875) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a4faf3e02ddf95c"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.gpuTimeEnd`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1876) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-45f90891c8ddff5b"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.gpuTimerFreq`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1877) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fd41e623a514574d"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.height`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1910) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc6172bb484a5604"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.maxGpuLatency`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1883) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7363a5dd289fef12"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.numBlit`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1882) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb69ed3f123a3867"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.numCompute`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1881) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e14c7a287da9c7da"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.numDraw`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1880) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-409464e0a4b6cf43"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numDynamicIndexBuffers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1885) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-116fc8144be5e9aa"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numDynamicVertexBuffers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1886) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-40300efee382e725"></a>[`public byte Inno.Native.Bgfx.bgfx.Stats.numEncoders`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1915) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c46ac6e847d2b8f6"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numFrameBuffers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1887) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f4b2387d558ea89"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numIndexBuffers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1888) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1ec07f8bea273da9"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numOcclusionQueries`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1889) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9a3755a133b6170f"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.numPrims_0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1900) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-268169c697a9dac0"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.numPrims_1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1901) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c02ca994ad06ab1"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.numPrims_2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1902) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6e7eab0565ac4e6b"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.numPrims_3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1903) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1ea1b56933e5adc"></a>[`public uint Inno.Native.Bgfx.bgfx.Stats.numPrims_4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1904) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a99733bbe2f13e5b"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numPrograms`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1890) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-50f4d7e6209f3705"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numShaders`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1891) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-750d38e84ed6458d"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numTextures`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1892) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d61bad017670508"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numUniforms`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1893) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a599e8b762610dd"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numVertexBuffers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1894) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c740d7409062c9e3"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numVertexLayouts`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1895) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7bd2b9f586951085"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.numViews`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1913) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb0d01acb4407d9e"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.rtMemoryUsed`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1897) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-645a240cbd836fc8"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.textHeight`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1912) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c8c1f3f510ec591"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.textureMemoryUsed`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1896) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b8d54214b9916f70"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.textWidth`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1911) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2a690896d439919"></a>[`public int Inno.Native.Bgfx.bgfx.Stats.transientIbUsed`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1899) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3116278f02b80cdf"></a>[`public int Inno.Native.Bgfx.bgfx.Stats.transientVbUsed`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1898) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0fda1fade0373904"></a>[`public Inno.Native.Bgfx.bgfx.ViewStats* Inno.Native.Bgfx.bgfx.Stats.viewStats`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1914) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b2ef17c97eabe8af"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.waitRender`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1878) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b109d8e0c2dc2e1a"></a>[`public long Inno.Native.Bgfx.bgfx.Stats.waitSubmit`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1879) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53dca2c06ef86ee3"></a>[`public ushort Inno.Native.Bgfx.bgfx.Stats.width`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1909) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5177bf8ff4a39ae2"></a>[`public Inno.Native.Bgfx.bgfx.Stats.Stats(long cpuTimeFrame = 0, long cpuTimeBegin = 0, long cpuTimeEnd = 0, long cpuTimerFreq = 0, long gpuTimeBegin = 0, long gpuTimeEnd = 0, long gpuTimerFreq = 0, long waitRender = 0, long waitSubmit = 0, uint numDraw = 0, uint numCompute = 0, uint numBlit = 0, uint maxGpuLatency = 0, uint gpuFrameNum = 0, ushort numDynamicIndexBuffers = 0, ushort numDynamicVertexBuffers = 0, ushort numFrameBuffers = 0, ushort numIndexBuffers = 0, ushort numOcclusionQueries = 0, ushort numPrograms = 0, ushort numShaders = 0, ushort numTextures = 0, ushort numUniforms = 0, ushort numVertexBuffers = 0, ushort numVertexLayouts = 0, long textureMemoryUsed = 0, long rtMemoryUsed = 0, int transientVbUsed = 0, int transientIbUsed = 0, uint* numPrims = null, long gpuMemoryMax = 0, long gpuMemoryUsed = 0, ushort width = 0, ushort height = 0, ushort textWidth = 0, ushort textHeight = 0, ushort numViews = 0, Inno.Native.Bgfx.bgfx.ViewStats* viewStats = null, byte numEncoders = 0, Inno.Native.Bgfx.bgfx.EncoderStats* encoderStats = null)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1917) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-36ba3ab3af7582ed"></a>[`public System.Span<uint> Inno.Native.Bgfx.bgfx.Stats.numPrims { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1905) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8fdb211b9aa92fd7"></a>[`public struct Inno.Native.Bgfx.bgfx.Stats`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1869) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.StencilFlags</code> · 48 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.StencilFlags ca96fefbfd3a7eb7a83d72659b8bccd8ee942afc65aaec7da0430abcdc730bdb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.FuncRefShift c30b923d444f4647a4aeba77387d905a8f355150feaa529b8bf149c378326c66 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.FuncRefMask 2a46c717a08353db549320d44e5d0c74e793ae7ea98b385f6a9a080c7cbc06be -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskShift 618edc9be6f6635e525cb461c50819e22e31876f055576d56f4cb6013000a708 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskMask 5cab34795b39a5ef776f31b91544b043b8f824a934a0fc30fb963bd5ebb18fbc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.None 6cc0305ba49150276a210b93ba0d9da859ee1508b1e6f437cb470852da2579ab -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.Mask dd4d52d0b67416483e2c7a1483f79da0ee520dc82a2f2618c39939be00d41290 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.Default 877f25ca3ed70bc1db52ae518386210ea5ed0f22a16aa547fbf3358adfe24f63 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestLess f0fb6a34eb16e3982f1ac22717334f37980acf613f5fdebbe6924785c6b06945 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestLequal acd0892745c4a99bdb7ec15260f4c957776078a12d1340cab6855327e706dbd1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestEqual ddb24384d2a968a6fe77457d90a87652768b250db3241419dd05ea25861c4fa3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestGequal 861f146817c9fb2b01bdb76ef57034210963d1d1105bf8ccfa4465d2ba52bde9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestGreater 70c56cfb3a6330704a07e808f9139fce9d567057e97c42c6683842da32120ad4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestNotequal 62dcb476848c08185322968200c37bb8da0390eac9a7fab97bdb16e2e5a178f2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestNever f4565d33acb23d70b7fa3c4b9de693730e4fa91624be58ef42c18c9719f49515 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestAlways 4d9bf031a93c0d77445035620ad59386971fa3ecd5359f07346a4c9831035cb0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestShift d180d3e3fab5424fde6efc91c5a6c9dfcfb78c29947a435890d9a5d85843a508 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.TestMask 8b75570e059b96c99fbbf9212bc096df849c4caa6a3195a5258a3c8dfa995e3d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSZero df49178c12a5f8b04ce4d85c0a923519a1f3cfb81d8aef4014bdb29adc5a8383 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSKeep 261b82db536ac9f303f2ff78e9ec75bfa432c7c05239c811731dce46be859b11 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSReplace 1cd8a4898eb19bd351e373b0d091363351755a8c25413d78730dbac6098ec400 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncr afe425a94f499b733a8bd47c79189cfa2c27444dd33a09717240506a2934dd10 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncrsat 207f2d9b1b53ba554057800982a084560e9604f3c45d4b85982c40c047a5ecf6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSDecr c3073345235e373133c33185a31a7d18a8917eb6e2a473abdf8dfa24f8d7c3db -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSDecrsat 87b10ff12acfea28f064d38089540813c0758b3f7625d4cb359dee8b39af5a23 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSInvert f47013483ceb8e0fa0de448b55e004f60785acf4ed57d310434f9350b6e4190e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSShift a5e3d6c6f178fa7b8f0ec3b2f6e370447075429196c50c2d60b83eab7b12ccea -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSMask 16a1fde196996c3aecf34b9039891bf663a556c64a8d99bef3be5ce40b169d31 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZZero 286e04506dd1d21467c4549e436c0996fd9d7cc622c162bff23d0094d1b4df08 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZKeep 3671b401a587069847d28434ee11a235f495b30425a483f8ef698b82977050bd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZReplace 591873d6ad4237358d181276918224c1e014e7dfd78f12d511f0ae6de0766e57 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncr c4e6e02115b165994ae011cbc17bd44b73ce06314d1c4b610b00626d6782fc51 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncrsat 0812a4f589b08930e9b58f7102c58e6537fd8b11185359b0810ce1feb0d21ddd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZDecr f628abb3dddb5b555676b90c5fb31e2d6a2b0edac77a3241be586779f3016b82 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZDecrsat 8399da8afee1bcf3542e62e79266acc6be80b007fdda451ad9210ec3f6ebae89 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZInvert 77990b8d4b55496d0356768708b90b3a102f81f48086e841aa2d74a922970c55 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZShift e916d284c1e17925ab946ec7126a91fc8a3ddda85fde3974390f47d51324a51f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZMask 7fce6ebdc0618521fae336c8491f6908fdeae91a4447fbbfa3475996241115e3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZZero 04712a9b6c7b947b20fb84146516753a3bf7af7f3de96e98ddc1dfd359cb131e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZKeep 67651f79ff29f7588bf4caa7d8dc1e94f78df068f4f5769e3406232590aebacc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZReplace 60dcbaf5f847698d3145d8c98b9d7e69237025ad6c15d3b43f396832071c7fa7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncr f06134d07ae90fa7df6e5770048c683f2c2be405d9c125ca066e9e9a4b98af7e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncrsat 0bb66b3e2280d3f9dbe521c90a8ebb81539d2cae832e021a6b8e6adfecfa5b4d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZDecr 3c74b00609f9e17959406ad7712500f66e1fe1bd9cd224500f3426ebb6a733d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZDecrsat 4048cc7e9b4895d550d6bc839991e9f843516b91ce6419431faea3df92829bb5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZInvert 4e55a065743ee2b60d198eb3bcf06d8cc042328b89be2635804c33a2dbf046e5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZShift 7e4444651a575b4d5220049b2138234565feb8891236a0d80926686062834350 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZMask 9b6d290376b8935c50afa7cceae4acf722d4c9b3e873ae55d8216ac2afc1929f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d8440918e4c65733"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.Default = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3729) | 当前常量或枚举值：`0`。 | — |
| <a id="api-13642a6ef512e4ee"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.FuncRefMask = 255`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3724) | 当前常量或枚举值：`255`。 | — |
| <a id="api-b28230ebbb893ccc"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.FuncRefShift = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3723) | 当前常量或枚举值：`0`。 | — |
| <a id="api-dd5ca704bea02827"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskMask = 65280`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3726) | 当前常量或枚举值：`65280`。 | — |
| <a id="api-2588a91b6451d9f9"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskShift = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3725) | 当前常量或枚举值：`8`。 | — |
| <a id="api-c91d707f0dd0177e"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.Mask = Inno.Native.Bgfx.bgfx.StencilFlags.FuncRefMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZMask`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3728) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.FuncRefMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZMask &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZMask`。 | — |
| <a id="api-74dc1f4a807841fb"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.None = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3727) | 当前常量或枚举值：`0`。 | — |
| <a id="api-aaa98bacf6e6f0a2"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSDecr = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncrsat`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3745) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncrsat`。 | — |
| <a id="api-468ccd7b4992175a"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSDecrsat = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSReplace &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncrsat`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3746) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSReplace &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncrsat`。 | — |
| <a id="api-81e0930aa900d2d1"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncr = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSReplace`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3743) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSReplace`。 | — |
| <a id="api-cdf26e804343f77b"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSIncrsat = 4194304`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3744) | 当前常量或枚举值：`4194304`。 | — |
| <a id="api-1d6755cd45096c38"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSInvert = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSDecrsat`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3747) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSDecrsat`。 | — |
| <a id="api-27dffd9f1c0e1e14"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSKeep = 1048576`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3741) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-c01b033ab18e154c"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSMask = 15728640`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3749) | 当前常量或枚举值：`15728640`。 | — |
| <a id="api-5f79e0dfa70b538d"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSReplace = 2097152`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3742) | 当前常量或枚举值：`2097152`。 | — |
| <a id="api-48c73c18e7df1a68"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSShift = 20`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3748) | 当前常量或枚举值：`20`。 | — |
| <a id="api-1a02bdad5b90eaba"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailSZero = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3740) | 当前常量或枚举值：`0`。 | — |
| <a id="api-69d7b4503196909a"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZDecr = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncrsat`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3755) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncrsat`。 | — |
| <a id="api-533cc1e26a7994dc"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZDecrsat = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZReplace &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncrsat`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3756) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZReplace &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncrsat`。 | — |
| <a id="api-725d9c6365cb2a21"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncr = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZReplace`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3753) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZReplace`。 | — |
| <a id="api-e654d82f451aff12"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZIncrsat = 67108864`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3754) | 当前常量或枚举值：`67108864`。 | — |
| <a id="api-ce75d3f635016e89"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZInvert = Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZDecrsat`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3757) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZDecrsat`。 | — |
| <a id="api-c3a9d22aa0740637"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZKeep = 16777216`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3751) | 当前常量或枚举值：`16777216`。 | — |
| <a id="api-7aec9acc9aaddea4"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZMask = 251658240`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3759) | 当前常量或枚举值：`251658240`。 | — |
| <a id="api-b1eb9e66fab4215a"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZReplace = 33554432`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3752) | 当前常量或枚举值：`33554432`。 | — |
| <a id="api-92dd83c4a81e256d"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZShift = Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskShift &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestShift`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3758) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.FuncRmaskShift &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestShift`。 | — |
| <a id="api-e591ae5c0e343b0a"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpFailZZero = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3750) | 当前常量或枚举值：`0`。 | — |
| <a id="api-12ef45a26972a719"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZDecr = Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncrsat`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3765) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncrsat`。 | — |
| <a id="api-863efe1366ebb957"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZDecrsat = Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZReplace &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncrsat`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3766) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZReplace &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncrsat`。 | — |
| <a id="api-93dd7b34d78f6f90"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncr = Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZReplace`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3763) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZReplace`。 | — |
| <a id="api-396bb4fdfb9002db"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZIncrsat = 1073741824`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3764) | 当前常量或枚举值：`1073741824`。 | — |
| <a id="api-8c6c71861dc13ce9"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZInvert = Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZDecrsat`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3767) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZKeep &#124; Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZDecrsat`。 | — |
| <a id="api-dff5777e41d12475"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZKeep = 268435456`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3761) | 当前常量或枚举值：`268435456`。 | — |
| <a id="api-57e9ae87de2ed853"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZMask = 4026531840`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3769) | 当前常量或枚举值：`4026531840`。 | — |
| <a id="api-9375700744fc8fcf"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZReplace = 536870912`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3762) | 当前常量或枚举值：`536870912`。 | — |
| <a id="api-bd91f583986e119a"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZShift = 28`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3768) | 当前常量或枚举值：`28`。 | — |
| <a id="api-ebc28e94861361f8"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.OpPassZZero = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3760) | 当前常量或枚举值：`0`。 | — |
| <a id="api-77b1d5c40c1736ce"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestAlways = 524288`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3737) | 当前常量或枚举值：`524288`。 | — |
| <a id="api-cf0a8e9670dcbcf5"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestEqual = Inno.Native.Bgfx.bgfx.StencilFlags.TestLess &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestLequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3732) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.TestLess &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestLequal`。 | — |
| <a id="api-979d7c90bedf27fc"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestGequal = 262144`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3733) | 当前常量或枚举值：`262144`。 | — |
| <a id="api-ea1ebcccad501a34"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestGreater = Inno.Native.Bgfx.bgfx.StencilFlags.TestLess &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestGequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3734) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.TestLess &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestGequal`。 | — |
| <a id="api-82584b775422a4d8"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestLequal = 131072`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3731) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-fb192e7e9be88646"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestLess = 65536`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3730) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-89b102d899dac77e"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestMask = Inno.Native.Bgfx.bgfx.StencilFlags.TestNever &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestAlways`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3739) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.TestNever &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestAlways`。 | — |
| <a id="api-c5c43d16765dd59a"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestNever = Inno.Native.Bgfx.bgfx.StencilFlags.TestLess &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestNotequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3736) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.TestLess &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestNotequal`。 | — |
| <a id="api-db766617b0a6c1f9"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestNotequal = Inno.Native.Bgfx.bgfx.StencilFlags.TestLequal &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestGequal`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3735) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.StencilFlags.TestLequal &#124; Inno.Native.Bgfx.bgfx.StencilFlags.TestGequal`。 | — |
| <a id="api-5f6f9db1c92dcdce"></a>[`Inno.Native.Bgfx.bgfx.StencilFlags.TestShift = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3738) | 当前常量或枚举值：`16`。 | — |
| <a id="api-b99c5deacc6b5fb3"></a>[`public enum Inno.Native.Bgfx.bgfx.StencilFlags : uint`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3721) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.TextureFlags</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.TextureFlags 63e38b1ecb4e05d8d156ae444da5bc803fe26d958d78ff5db7b096d200148138 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.None 4107b87f05d0d46f65e581d65c2010b918b8dc0af02da401c10becffcae5fc61 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.MsaaSample dd103f1ef08809960486d311bebef2f20eb6176f4275f48bb4ac06d88b39a21a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.Rt f08366f05dfb9877e106c26acde993d25e438cc7f174c40abd0c211e880d531c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.ComputeWrite 490f60bc1394a6ebb57fe9caa43d22272c25176d70321bf899b0e5b8b9036c9a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.Srgb 5bea9d5925480eefa65b4b45cc6717ca2ec5767086e1f5f0e7ae9536d845c03e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.BlitDst 63c5810733f5326259d95e014ac00527b05d8cb7ac67cee6c14c0553f80d34ec -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.ReadBack b4159c91c331d0407d9390548c07cba5eeefe23e8e24c50b18951a945e71858c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.ExternalShared 0b2a4bf061f4de587890c1eaaf63c88b48fce352a8cad72bb4ddb61a62248522 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX2 4a005981b1c08067d4ca91e17cbf8bdde3688479e2d3d023739c7a2dd8eafbd3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX4 7c7bfd994a0e8dd09e33bcae9c67098b0ab906aeec088acde3bdd7442ff5c5c4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX8 304e2e67ad427f8a6665f93595c87b2cf6365f19118b55cf53bfa86d07b29d9b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX16 dde3f009f8fb165a3365a16b5a9e48c56749365509fd287d2aee92dec70f2ab0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaShift 3c7b696486232d1ced5437814f8151046d1fc31a9b398dd36c18e148d70bee74 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaMask dfdb8c2a7c3604e6718bb0a38509901dcaa2b1949ea107ce058eb0d5da4bd47c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtWriteOnly 349a50e78bd173d850a4b0aed2b9da4fcecd44c59b5b1ca6dd6dda82b31fd027 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtShift 14ab0bd1cadbc663b4913188765873f57fa029278d7711f851162c365f7b0cc8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFlags.RtMask a57290930910f3536c7c7ff34bd61268e50646cb6ed26b2a4b604c1005519ae8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9a9bba16267d642c"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.BlitDst = 70368744177664`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3786) | 当前常量或枚举值：`70368744177664`。 | — |
| <a id="api-6390c8f76ef36055"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.ComputeWrite = 17592186044416`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3784) | 当前常量或枚举值：`17592186044416`。 | — |
| <a id="api-5109bc630a203166"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.ExternalShared = 281474976710656`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3788) | 当前常量或枚举值：`281474976710656`。 | — |
| <a id="api-73996bf41fc1f20b"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.MsaaSample = 34359738368`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3782) | 当前常量或枚举值：`34359738368`。 | — |
| <a id="api-c97e50d5ac0fdf30"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.None = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3781) | 当前常量或枚举值：`0`。 | — |
| <a id="api-3f6a45f532b94662"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.ReadBack = 140737488355328`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3787) | 当前常量或枚举值：`140737488355328`。 | — |
| <a id="api-b636f5e75662dd32"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.Rt = 68719476736`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3783) | 当前常量或枚举值：`68719476736`。 | — |
| <a id="api-8f5bd9b70ba2ac0b"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMask = Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaMask &#124; Inno.Native.Bgfx.bgfx.TextureFlags.RtWriteOnly`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3797) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaMask &#124; Inno.Native.Bgfx.bgfx.TextureFlags.RtWriteOnly`。 | — |
| <a id="api-a8ea9017ddb4d86c"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaMask = Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX2 &#124; Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3794) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX2 &#124; Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX16`。 | — |
| <a id="api-8038f0a0fdd05c32"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaShift = 36`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3793) | 当前常量或枚举值：`36`。 | — |
| <a id="api-70c30a801186feee"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX16 = Inno.Native.Bgfx.bgfx.TextureFlags.Rt &#124; Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3792) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.TextureFlags.Rt &#124; Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX8`。 | — |
| <a id="api-3dcaa1ae4bc8320b"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX2 = 137438953472`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3789) | 当前常量或枚举值：`137438953472`。 | — |
| <a id="api-f17095603b4f1214"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX4 = Inno.Native.Bgfx.bgfx.TextureFlags.Rt &#124; Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3790) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.TextureFlags.Rt &#124; Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX2`。 | — |
| <a id="api-5dadf6d31502e705"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtMsaaX8 = 274877906944`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3791) | 当前常量或枚举值：`274877906944`。 | — |
| <a id="api-59d71e39559d31de"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtShift = 36`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3796) | 当前常量或枚举值：`36`。 | — |
| <a id="api-977d6877894c242e"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.RtWriteOnly = 549755813888`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3795) | 当前常量或枚举值：`549755813888`。 | — |
| <a id="api-33071d235f96c6b8"></a>[`Inno.Native.Bgfx.bgfx.TextureFlags.Srgb = 35184372088832`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3785) | 当前常量或枚举值：`35184372088832`。 | — |
| <a id="api-6a241f90aa94ab63"></a>[`public enum Inno.Native.Bgfx.bgfx.TextureFlags : ulong`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3779) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.TextureFormat</code> · 102 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.TextureFormat 19fa6be904fd5c38fd2b42eba8758f79420ed093d5ee6da22a35c345d7d53dc5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC1 8fe4def5320b12bce41ff21a83589683d9d43e161120f39de7918da5f050fbd1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC2 66ce4f9f97df710eaaee3d052fa950d83ef6a804ab89f6b29d97457d4f054aa6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC3 4258a4b819a47683f194f6e9e772be8dd5b56857a4ba1e667931c518cce665c1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC4 da4748037f147659011f057f959e3e4acb6d7096bf885d13aadd8a4f2f10a2e8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC5 1b751077b4b4b65c3ae4b3089c688b7f7b56d0392efb7db3c36c7cd917ab2614 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC6H af5ea98f9e349b3987089e635c69b650a5d90a3c97f3247cd09adb03e83e8922 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BC7 844355fc08eb069678dfffe3385b55decf99425524c5d93f2235b44b3ef2f58d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ETC1 e37a2d15f9c2049b8ab5a43fbf66cec6d6e86a0323c4474d35263f4451de5f2b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ETC2 d04e58a4a4a6dd5d1d3e77ca92fab78f4d4b1528363e9b581337a27ad156dcf9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ETC2A 422b9c3732fe82362c45753b112ee2e6db4bfa3424471539337f14bfb92ca39d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ETC2A1 73d7e23953d9e735377831c96164f0e50f4e35c19723e29ccba0c55fa34dac5e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.EACR11 ddae45c11558eddfa41b1d3182d58eed063cc2b5aa5c2b21d493d5c677ecc9a0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.EACR11S d90b14433a31b0f0204162e8daace0d683bf8b423e940106db60356953dae4bd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.EACRG11 efff49e97c1e28bfbb714cfd49d47f765c3fe1127a3a3ca6a3d1c3b0fc6ecfda -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.EACRG11S 30cf23433b94d5568439a2de2da0177468d42173a3683f473a7c3b61743b79df -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.PTC12 7620079cb68763fb1e319159bcf20aa12db7f7cde38f0910a2841e50b31ffadb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.PTC14 cb36cbcf0b728f606f8e9238fd7b249697f42377f80263b69872a51c038ed0f5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.PTC12A 02ed8dc90952adad05bb627c2f4d1737adfae8ab57b571a968c0a7fb1b5d93ce -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.PTC14A 4352274da2c7de27ce9f5d3e3d9d41d66f18bb51e59a2319cb5113c758a228c7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.PTC22 a886ca86449a122e8a70c9aaf589d70dfe05cc9a8eca37273ef5ab5807d3179c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.PTC24 8006be7c0dccfd240a9592ab6a01ce7972fe26a620e3e331d2e93c6b8cd3fafa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ATC adeba935fa40dab5e66990263bb6f5b55759fc466a800cd0f6ae228ab268a4da -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ATCE aa2079c4ed6679d414d7f0e625b70b77a3148d4b4431469a769ffdd9ebd432ed -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ATCI 4a5e17d58f848a896ffaf1c88618252c555cdf932132e2b5ab8330f505acceb5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC4x4 f54e35e13907099397aa7e5afc33d3c08f96d5fab21d25b0c58f3e7d68af7000 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC5x4 bdc3272b019f97b1ed04124e812ea8b719795f714e1de542a6506435465148a8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC5x5 d62e48681d26c80710e8a479d9ca029d4f2805e19b4ac38cefefcf29d0b9cc25 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC6x5 6bc26c5182280ec429db798b49484eb652200bac439ae7f283d72b78bc056869 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC6x6 221fa7f8149870c1b1a71afbdf52dbdd2ee8f62c2e03db424c1fb91c3eb00471 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC8x5 55f3541294ccbbe9b51fb453fba0ac75033b0647b6172153ced789a216620cf9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC8x6 cd96c303e5137781dac3345d8054344491256e004e28ca36b99578bc79b7bdf1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC8x8 ceabe3adf09cad5db0c1dec7fd7d11213f1bba2d62c23fd0a96ca0eed5a9bac1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x5 2c2e26e9a4fec0e173b68812182837f3e0a2008c12061329241286670106d5b1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x6 0bb20b2714ff8af63424b9e2179c6fbacd3f2ceeab6032e50f860a7d2b2ac687 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x8 61ac5f20b25a7f19f219b21fb1463b152df96dd14d7a77939dd3cd3d2cba903f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x10 20e8b592b09ef3f72d826c031dc94654e3c11afd9c407283ea97b07c2e62d63d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC12x10 c6c53e2ddb494c3e287d160fa56ca99b48bae80924923e583f1e220ec432aeb3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.ASTC12x12 751d1ae15e24d8636f6084072c3f641dab2657be27e028c1805d2fc12e7435f7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.Unknown c5e9f57a695070d821262224d9f485379aa43698e4b07f3352f97c3384ec15ad -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R1 6672201a2ab8199f93914ee6c505eb43816b21a0890ca87ce8bcaa03bdbfa535 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.A8 a0ef817c8026cf2c31176959d543faa90cada448ebddff9071c19b376b6bd917 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R8 caf8f31ca66b52194790add1f0b255f5118956a5af8fe2a9e4bdb68234208c82 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R8I fb57052c885c07360f975db3d7679b71aaa69ab380634952573e60bdbf9d01fc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R8U aa5b32da696c410ec8aa6dd5b5559db36357bef36b5fa1acc453629fca3450ea -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R8S a9643e334e14f9fa0547989b955debbe8305b3558165e2ecc3383b49b41807f5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R16 6754d15523cb8f3d1d499f2c2435bbdc18c1f59fe73f06cbe823b4f5a40252e9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R16I b7ed72e3ced550ba55b5cf53d5ae35ea1592bd7dcac4a1b6673136d820b09826 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R16U 6a50774e64c93fc86b3829693bf7bfeb9e66408ca0036efdaa7a0ac810d73613 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R16F 971268d2bdd668cdcc36ed775d0c0865dbd271182b2a5b62cfe27dff66021dfc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R16S 29b6119acdf30918e5605aaaa9cb3c7b6b27960507b4bd98d422227f1b2e40e9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R32I 33fc8f890b30d90f96afe19b70674c5436ab8c74da64848e4ae3b02e21bd642d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R32U 710423b32450ea1b35cb7b41884853c99ca960312f65a297bbdb778e70397b99 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R32F 2b573773b1081ca01e0afe2a0b0671d52758f407faaf7a13cb32e26a5724c08e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG8 a358ac22038e3bc3cf387bd76ef00fa50a2a4c9a95a30ebd6d2c716e183e8484 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG8I 219bcf5178b9114ac8151c60720a0961518612092fb5d0c5677b3002a37e2c10 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG8U 633775ea5d443c233a3948f003ad0898afa430595aa15f973a43c79c14c7eb4d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG8S 294498c477441a05c01081f303e14f5005270e0b71d276f2c9cc27a4c7d9d7d1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG16 9145c363699af6b64e1f703bcef067b8c3a565e611e84fc27314f56450fb25c9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG16I 5519f3ed7bc256f90d8308826a3738ef4926f93cb0934a01fa486c5e60b3c836 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG16U 176d70076a54f73745b807929d755de3e5b44497dbd66a955d4d3e50729c08f3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG16F 4bcfa38c36c649b47bfdf0641e4b0ae366df8b96a25b296931628f1f371bcdad -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG16S 9744a5dbdc363b5648c18232fe66fe321c54eff08ed2362ae11a7f31afcc0e51 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG32I a97da419b8bd9cc74f9291b65414dd56bd824274520572da9c045eefe80dc2e2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG32U 001b300902fba4726498ea36c82064885a5b4818848e174b358c2a93d4117aec -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG32F 954ba6d6a9a1c0bc614728f7661f151bfcb558d323cdf3e8007a6081ea9d4fbd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB8 3c1e4f3999ac69325181b92d0fceb0cc1a891f1921aa19d09954344849586f90 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB8I 6f02d6b62b470a71177ac3d0e9e3d328cb65856d4bdaec81b4e8de46feb10643 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB8U 9c30be68958b219332518d714ff104c1807680e2d23eac3ef7481f5b6f26d846 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB8S 77a4d1876529b2077dbbe3b0e864ab901d5ac1025997ca0cd571befc02f32f0a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB9E5F 3fcb6270061bdf82d0969c8677e93889c25d968cab5cfaf04acfbed21641a955 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BGRA8 60ab9c0dd2b88d541d958e8a9819d83eeb3f154f993fa15075901679f890cce4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8 a1a9ff7204586623956ffc6d5ef7ada9216822e5dc8e548050b2d6e4c13bd156 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8I 230110346b2c7d0aefb4d7b3b81bc5138c86ad92c1af734254dd1c75ca967db1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8U 7c5b4a92ea7fd6a4bbc7c1a91066cc29d391ddbe642822ba4c9762a53e94c67a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8S 8d617d5b5e3d2f15e654ebe2b3af03dbda792c94bd4a26e67ae4652fcb8e9ebd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16 f6d1ce70c9fc9897c5b048d52a956cb51cfa49186c822a62c5aa39ba952d277c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16I 96c485e3253542e63369b2f53b9b9d42e54fe006f1ca7a7a8495f4e8ac1ec478 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16U a810ff3baa3215625cd2790e5d67109a4ac3f1dfd0b424335414d7d3c1265e05 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16F 2d705086b57b0a3e5e425c042188849dce9be69b1a9234a7d2240d79d64446a7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16S c9b0e07112e4d23dc012839075692e45f7a9c17fe92002b558dda1de4b323c9d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA32I 1d3d13e8e1151bd475dd502337b8407088c2d49ece00f0d4d829d4e0a9967b48 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA32U 3873c48382495c96b76bf8fa165bb08941966ee66d17dd8749ec1c5676795246 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA32F 951acbe8ca8feaf879d3331a9bc524af3c806ce434324831e66e980ccb389ceb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.B5G6R5 7d6163710e3fc0cd60318685a89c90b57d65438ed1c4fee1221768d95e8f2c3b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.R5G6B5 97db7994aa7921fbce734e95057bd48b7a3848a9a03bfd2f71181d78bf335184 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BGRA4 5c000822ece5d93944db183e8f77ae13688860b8eeda1f7ad0dec9aa1f9b4f73 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGBA4 21a86132d281ca3cae3e85c74ee97bcbbac67742dc2cfe6fe47bb3cc7838a378 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.BGR5A1 3c7b9eea714dc3c8ca41d4d6558dc35879605e8bec0bcfef86336e5ef0a9afa5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB5A1 9861c443b16c49efd0f5356525439ea7f1448a223353802db63dd231fc81d503 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RGB10A2 9d6e29047a8bae09920438c0dc50840b18096f02634a659db6e364e0965f1c9e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.RG11B10F 111c8de19a322a3d8315478a43d61720c6bae039e0d46fe94182b0f224e0b2c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.UnknownDepth a09eba3f0e66c8bc1141dd98452a536c7b0ccd150e586f2f52a85bd01758560e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D16 ddd3f0678b9319afb23de0c4a35e8e6f161e5d9e91203bae28143c914ad8316e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D24 eff7318a794ca4dfeae0600de14946f019f24e8144f4f99f3c470c186c998791 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D24S8 df579f13c44b8ab9abffce7d8ee67f919f931c58939722a2558ad443573c52cf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D32 24beae21e3d59fb0940df9b035966436ce475805dcecdff94b972bac728cae63 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D16F 66c27c16454c403b57640ac49e5c4721998713a113445b5bea4bb38ec5232af9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D24F f4c2c35ceab75f589f5aa6aeb35d650a83f0a7eb323854dd052446b91ada85b8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D32F da846b1adcca6ba2bbee4d975854c03fb732bb795841c19bb16864587ad1e3fe -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.D0S8 5f839bf18fcfb67fc28ca877e23c9765d4d0f3e84b3aa543ecb4ac4aef3e871d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureFormat.Count deef704398cc1c036ae3447fc98c82b270038065e998cebab449686370c2233c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-23c0f04e2eb98716"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.A8 = 40`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L165) | 当前常量或枚举值：`40`。 | — |
| <a id="api-5705a14dc0dc7d33"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x10 = 35`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L160) | 当前常量或枚举值：`35`。 | — |
| <a id="api-8875015509d61e93"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x5 = 32`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L157) | 当前常量或枚举值：`32`。 | — |
| <a id="api-548de5cf143f71d0"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x6 = 33`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L158) | 当前常量或枚举值：`33`。 | — |
| <a id="api-02f1d2f91889ecc0"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC10x8 = 34`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L159) | 当前常量或枚举值：`34`。 | — |
| <a id="api-c35ea18fe18cdc4d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC12x10 = 36`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L161) | 当前常量或枚举值：`36`。 | — |
| <a id="api-ff521b55f359e497"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC12x12 = 37`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L162) | 当前常量或枚举值：`37`。 | — |
| <a id="api-f42959c467b192c8"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC4x4 = 24`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L149) | 当前常量或枚举值：`24`。 | — |
| <a id="api-9d743ae641a22aab"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC5x4 = 25`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L150) | 当前常量或枚举值：`25`。 | — |
| <a id="api-512caa8bd702532d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC5x5 = 26`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L151) | 当前常量或枚举值：`26`。 | — |
| <a id="api-c6ebe57923126cd0"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC6x5 = 27`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L152) | 当前常量或枚举值：`27`。 | — |
| <a id="api-0c8de7dfa46c76f2"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC6x6 = 28`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L153) | 当前常量或枚举值：`28`。 | — |
| <a id="api-aebfce2697a31e5d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC8x5 = 29`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L154) | 当前常量或枚举值：`29`。 | — |
| <a id="api-f0eefceb1f45c82a"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC8x6 = 30`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L155) | 当前常量或枚举值：`30`。 | — |
| <a id="api-4c5907a89d8e76c9"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ASTC8x8 = 31`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L156) | 当前常量或枚举值：`31`。 | — |
| <a id="api-b38598a970a73120"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ATC = 21`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L146) | 当前常量或枚举值：`21`。 | — |
| <a id="api-54b77ba4a5fa4df7"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ATCE = 22`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L147) | 当前常量或枚举值：`22`。 | — |
| <a id="api-d1dbb6cff587c4ab"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ATCI = 23`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L148) | 当前常量或枚举值：`23`。 | — |
| <a id="api-d0fbd635f7b3ae5e"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.B5G6R5 = 83`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L208) | 当前常量或枚举值：`83`。 | — |
| <a id="api-c33604913c26fa6b"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC1 = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L125) | 当前常量或枚举值：`0`。 | — |
| <a id="api-7014f16361d37d78"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC2 = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L126) | 当前常量或枚举值：`1`。 | — |
| <a id="api-5e7953e6561ae80d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC3 = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L127) | 当前常量或枚举值：`2`。 | — |
| <a id="api-413b1bd095bbd798"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC4 = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L128) | 当前常量或枚举值：`3`。 | — |
| <a id="api-3526ad3ef151117c"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC5 = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L129) | 当前常量或枚举值：`4`。 | — |
| <a id="api-518558f9a730eb55"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC6H = 5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L130) | 当前常量或枚举值：`5`。 | — |
| <a id="api-cb9099333a6cf2d4"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BC7 = 6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L131) | 当前常量或枚举值：`6`。 | — |
| <a id="api-0954fb0f0f403804"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BGR5A1 = 87`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L212) | 当前常量或枚举值：`87`。 | — |
| <a id="api-d1779c5b9a7f8256"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BGRA4 = 85`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L210) | 当前常量或枚举值：`85`。 | — |
| <a id="api-148ec8528a2f33fd"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.BGRA8 = 70`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L195) | 当前常量或枚举值：`70`。 | — |
| <a id="api-1bbc5155f48eec9a"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.Count = 100`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L225) | 当前常量或枚举值：`100`。 | — |
| <a id="api-9e3c2f11792061aa"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D0S8 = 99`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L224) | 当前常量或枚举值：`99`。 | — |
| <a id="api-4f3981d5417cc34b"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D16 = 92`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L217) | 当前常量或枚举值：`92`。 | — |
| <a id="api-329c9e9642525470"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D16F = 96`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L221) | 当前常量或枚举值：`96`。 | — |
| <a id="api-fe78f2ac7a89e54d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D24 = 93`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L218) | 当前常量或枚举值：`93`。 | — |
| <a id="api-9aa99b5d0ddc5460"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D24F = 97`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L222) | 当前常量或枚举值：`97`。 | — |
| <a id="api-5c0a303dddff9c3d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D24S8 = 94`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L219) | 当前常量或枚举值：`94`。 | — |
| <a id="api-0afda02d5519ab64"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D32 = 95`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L220) | 当前常量或枚举值：`95`。 | — |
| <a id="api-9fa82c2aebfc15a3"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.D32F = 98`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L223) | 当前常量或枚举值：`98`。 | — |
| <a id="api-f1daacf030029d2c"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.EACR11 = 11`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L136) | 当前常量或枚举值：`11`。 | — |
| <a id="api-b98c2cc8ff8cfec6"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.EACR11S = 12`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L137) | 当前常量或枚举值：`12`。 | — |
| <a id="api-add7986230fdcc26"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.EACRG11 = 13`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L138) | 当前常量或枚举值：`13`。 | — |
| <a id="api-dda5edbff906b910"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.EACRG11S = 14`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L139) | 当前常量或枚举值：`14`。 | — |
| <a id="api-96129d43519837a1"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ETC1 = 7`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L132) | 当前常量或枚举值：`7`。 | — |
| <a id="api-cade0ea92b1af805"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ETC2 = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L133) | 当前常量或枚举值：`8`。 | — |
| <a id="api-0147a1351d89f869"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ETC2A = 9`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L134) | 当前常量或枚举值：`9`。 | — |
| <a id="api-0e4d0069af20360d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.ETC2A1 = 10`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L135) | 当前常量或枚举值：`10`。 | — |
| <a id="api-18c33aaaeee2c510"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.PTC12 = 15`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L140) | 当前常量或枚举值：`15`。 | — |
| <a id="api-77c30ee4c40567c3"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.PTC12A = 17`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L142) | 当前常量或枚举值：`17`。 | — |
| <a id="api-582eff537e3dd93e"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.PTC14 = 16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L141) | 当前常量或枚举值：`16`。 | — |
| <a id="api-cea8851350e380ae"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.PTC14A = 18`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L143) | 当前常量或枚举值：`18`。 | — |
| <a id="api-b22f0ef5d6ebb633"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.PTC22 = 19`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L144) | 当前常量或枚举值：`19`。 | — |
| <a id="api-4e9cc2b63c5a3e1c"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.PTC24 = 20`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L145) | 当前常量或枚举值：`20`。 | — |
| <a id="api-280afce2f48c1f67"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R1 = 39`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L164) | 当前常量或枚举值：`39`。 | — |
| <a id="api-6336b507066b597e"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R16 = 45`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L170) | 当前常量或枚举值：`45`。 | — |
| <a id="api-e57df33e330ef26e"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R16F = 48`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L173) | 当前常量或枚举值：`48`。 | — |
| <a id="api-e3145480222f906d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R16I = 46`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L171) | 当前常量或枚举值：`46`。 | — |
| <a id="api-342aae3ea687c539"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R16S = 49`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L174) | 当前常量或枚举值：`49`。 | — |
| <a id="api-b2fc225c6c1310e5"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R16U = 47`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L172) | 当前常量或枚举值：`47`。 | — |
| <a id="api-7ee26f5e6b3b310b"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R32F = 52`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L177) | 当前常量或枚举值：`52`。 | — |
| <a id="api-f6e45766f479575b"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R32I = 50`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L175) | 当前常量或枚举值：`50`。 | — |
| <a id="api-5e14ba0caf4357fa"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R32U = 51`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L176) | 当前常量或枚举值：`51`。 | — |
| <a id="api-669893a3357fa396"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R5G6B5 = 84`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L209) | 当前常量或枚举值：`84`。 | — |
| <a id="api-230fc6149ce10428"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R8 = 41`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L166) | 当前常量或枚举值：`41`。 | — |
| <a id="api-e8df4ac26fbc5316"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R8I = 42`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L167) | 当前常量或枚举值：`42`。 | — |
| <a id="api-6f2f92f774ec32a9"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R8S = 44`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L169) | 当前常量或枚举值：`44`。 | — |
| <a id="api-b70c8c18af80e8d7"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.R8U = 43`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L168) | 当前常量或枚举值：`43`。 | — |
| <a id="api-3d8afb7471596782"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG11B10F = 90`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L215) | 当前常量或枚举值：`90`。 | — |
| <a id="api-fc9e262efb44de9c"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG16 = 57`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L182) | 当前常量或枚举值：`57`。 | — |
| <a id="api-12010e567c525ba6"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG16F = 60`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L185) | 当前常量或枚举值：`60`。 | — |
| <a id="api-612f74a3ee8f8fab"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG16I = 58`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L183) | 当前常量或枚举值：`58`。 | — |
| <a id="api-808e3622e87b9aa4"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG16S = 61`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L186) | 当前常量或枚举值：`61`。 | — |
| <a id="api-cb083dcdd7b020ed"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG16U = 59`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L184) | 当前常量或枚举值：`59`。 | — |
| <a id="api-0ae10eceba4e85f1"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG32F = 64`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L189) | 当前常量或枚举值：`64`。 | — |
| <a id="api-3e58bb36e10e06d0"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG32I = 62`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L187) | 当前常量或枚举值：`62`。 | — |
| <a id="api-73eed92110705a40"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG32U = 63`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L188) | 当前常量或枚举值：`63`。 | — |
| <a id="api-7f77a07bc69e5791"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG8 = 53`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L178) | 当前常量或枚举值：`53`。 | — |
| <a id="api-5a10db0d14d1ca86"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG8I = 54`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L179) | 当前常量或枚举值：`54`。 | — |
| <a id="api-b55ceaf16bcc7b22"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG8S = 56`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L181) | 当前常量或枚举值：`56`。 | — |
| <a id="api-74f3887995eb21c8"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RG8U = 55`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L180) | 当前常量或枚举值：`55`。 | — |
| <a id="api-17948d24fecea324"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB10A2 = 89`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L214) | 当前常量或枚举值：`89`。 | — |
| <a id="api-9e0755bf9d645659"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB5A1 = 88`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L213) | 当前常量或枚举值：`88`。 | — |
| <a id="api-1e4edff53731ca96"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB8 = 65`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L190) | 当前常量或枚举值：`65`。 | — |
| <a id="api-db00fdf8245e35e0"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB8I = 66`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L191) | 当前常量或枚举值：`66`。 | — |
| <a id="api-b67ce1dc7bada87d"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB8S = 68`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L193) | 当前常量或枚举值：`68`。 | — |
| <a id="api-78eda65f908e39af"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB8U = 67`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L192) | 当前常量或枚举值：`67`。 | — |
| <a id="api-80baaa71b427b580"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGB9E5F = 69`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L194) | 当前常量或枚举值：`69`。 | — |
| <a id="api-e84416e650d14f55"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16 = 75`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L200) | 当前常量或枚举值：`75`。 | — |
| <a id="api-e65a72da10ff0d68"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16F = 78`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L203) | 当前常量或枚举值：`78`。 | — |
| <a id="api-64adc43131d6f265"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16I = 76`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L201) | 当前常量或枚举值：`76`。 | — |
| <a id="api-a01556ae4098e8d9"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16S = 79`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L204) | 当前常量或枚举值：`79`。 | — |
| <a id="api-21c98ee0e8b9864b"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA16U = 77`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L202) | 当前常量或枚举值：`77`。 | — |
| <a id="api-18e398824526999a"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA32F = 82`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L207) | 当前常量或枚举值：`82`。 | — |
| <a id="api-3481aa9d017ec7a5"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA32I = 80`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L205) | 当前常量或枚举值：`80`。 | — |
| <a id="api-2a426d890d4e63b5"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA32U = 81`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L206) | 当前常量或枚举值：`81`。 | — |
| <a id="api-d5942a366d02e029"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA4 = 86`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L211) | 当前常量或枚举值：`86`。 | — |
| <a id="api-3ddd421494ecdeb5"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8 = 71`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L196) | 当前常量或枚举值：`71`。 | — |
| <a id="api-c42d6cc9d9631903"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8I = 72`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L197) | 当前常量或枚举值：`72`。 | — |
| <a id="api-bf5eb58e38baf92f"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8S = 74`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L199) | 当前常量或枚举值：`74`。 | — |
| <a id="api-c816e91728db8333"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.RGBA8U = 73`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L198) | 当前常量或枚举值：`73`。 | — |
| <a id="api-5ac6c89ebe6f72c6"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.Unknown = 38`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L163) | 当前常量或枚举值：`38`。 | — |
| <a id="api-ecab39411bdb0ac8"></a>[`Inno.Native.Bgfx.bgfx.TextureFormat.UnknownDepth = 91`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L216) | 当前常量或枚举值：`91`。 | — |
| <a id="api-b5d1009fa4f0ad7f"></a>[`public enum Inno.Native.Bgfx.bgfx.TextureFormat : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L123) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.TextureHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.TextureHandle e7a28ba38a8557dace9f6dbfc9021673d82063706358b986ba9d3f4ecd968662 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureHandle.idx 937a8eea10c68d76e882526160582e2fb784b8f4fb62e88f43f20a59dd355bd1 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.TextureHandle.Valid e8f4734520d0e5da60f7ea656c84ba8df01d4a85835266130cc41b670133ec67 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.TextureHandle.#ctor(System.UInt16) cf192fb6295d9cb8a88251abe15485339517e4ae63f4ab9b15e74e32637a31a4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a12af8e5fbaa248e"></a>[`public ushort Inno.Native.Bgfx.bgfx.TextureHandle.idx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L659) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-340617ce25eecb8f"></a>[`public Inno.Native.Bgfx.bgfx.TextureHandle.TextureHandle(ushort idx = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L662) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0007cdad87b1f9cb"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.TextureHandle.Valid { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L660) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f339ce08e39cf6ae"></a>[`public struct Inno.Native.Bgfx.bgfx.TextureHandle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L657) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.TextureInfo</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.TextureInfo 4ca9759bf20c925319e082943f3c67c4d9c3e7ce226394abc83562e1b83b1094 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.format 5e7f28c28b9b5112f6d1d2580ca0271d85d68e8d02c8568bd73a5d8e406b3dfd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.storageSize 1c06cee4dd0eac2517b815d49a21fa5a6dc904e7c84201800af0c49c0c6fe4ee -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.width 30f68db008b42ebe526d39295291fbfef4b16e2b9245d47026d3bd007b660556 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.height 3ba96b85f8722fa122b18f7eadf23fbde0804abbe930be628dace39d07fe9df5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.depth da619003d4720c6311eb7fd0fae7f0f70fb3ef2ce75dea6047e8b7fa5fb33a94 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.numLayers 548012a6125fd3080453e16a8ccad5fbd4556ca902276ba263e3a353ed46f744 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.numMips 79cf391346bcedac70a62b36fb8ce2bfcbb8214ffac3c3a9205db6986201e443 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.bitsPerPixel 3f6f6fa76254f1804d28efe9fb4078524c19543ac0af85a205f8ddb831008fa6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TextureInfo.cubeMap 23d02f03f2e1a458d77f7884ee45321a735db3349aaf9336e73b769a212e7e97 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.TextureInfo.#ctor(Inno.Native.Bgfx.bgfx.TextureFormat,System.UInt32,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.Byte,System.Byte,System.Byte) cfd43a449fc9cd490839aae27fc4939e260e8dc7dfa7e117b61b79161a813cd3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d12f93a0845ce29f"></a>[`public byte Inno.Native.Bgfx.bgfx.TextureInfo.bitsPerPixel`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2242) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-049d76765e5a4335"></a>[`public byte Inno.Native.Bgfx.bgfx.TextureInfo.cubeMap`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2243) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-47a5a60ca146aa86"></a>[`public ushort Inno.Native.Bgfx.bgfx.TextureInfo.depth`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2239) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-41c4a030d5e261d0"></a>[`public Inno.Native.Bgfx.bgfx.TextureFormat Inno.Native.Bgfx.bgfx.TextureInfo.format`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2235) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3481da1eaae05b08"></a>[`public ushort Inno.Native.Bgfx.bgfx.TextureInfo.height`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2238) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ff645d0ce20d3b0f"></a>[`public ushort Inno.Native.Bgfx.bgfx.TextureInfo.numLayers`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2240) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-655a88cd816f4431"></a>[`public byte Inno.Native.Bgfx.bgfx.TextureInfo.numMips`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2241) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc4dd21b85338e23"></a>[`public uint Inno.Native.Bgfx.bgfx.TextureInfo.storageSize`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2236) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8172e2bf94f11563"></a>[`public ushort Inno.Native.Bgfx.bgfx.TextureInfo.width`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2237) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf55f08dd34cef35"></a>[`public Inno.Native.Bgfx.bgfx.TextureInfo.TextureInfo(Inno.Native.Bgfx.bgfx.TextureFormat format = Inno.Native.Bgfx.bgfx.TextureFormat.BC1, uint storageSize = 0, ushort width = 0, ushort height = 0, ushort depth = 0, ushort numLayers = 0, byte numMips = 0, byte bitsPerPixel = 0, byte cubeMap = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2244) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-55e98959afb11b4c"></a>[`public struct Inno.Native.Bgfx.bgfx.TextureInfo`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2233) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.Topology</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.Topology 06625fd2ee63e05f944ccf2f96eb2cce3f9866896906b184f6181f828aaad879 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Topology.TriList 4e2735f6d6dfade9a5dfc66ad237258ff3ae118e4f827b7b59d380c260cec6b0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Topology.TriStrip 8dd869ce426943543c0a5351ec874c7e1bddeeedc282e54a8aff4a114ec8fa2d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Topology.LineList 8cf597971d0126b545a494080d27d302c680003b348d44e2f6f69ae9f4847884 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Topology.LineStrip 8cd0e2708663f728863216369f691e76f549cd14175e0bef6e7a37138b9e138e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Topology.PointList 679b633f9aa1aa286165bed4331e892987ca5540d1359750e371b055a02c8a15 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Topology.Count 7dc9ba43d05955519f765604b377f084fc074bc2df453ee4abcf02f535ceec65 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8b6910553ada3769"></a>[`Inno.Native.Bgfx.bgfx.Topology.Count = 5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L302) | 当前常量或枚举值：`5`。 | — |
| <a id="api-db541e2be67dae9d"></a>[`Inno.Native.Bgfx.bgfx.Topology.LineList = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L299) | 当前常量或枚举值：`2`。 | — |
| <a id="api-14cd62e056e92d29"></a>[`Inno.Native.Bgfx.bgfx.Topology.LineStrip = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L300) | 当前常量或枚举值：`3`。 | — |
| <a id="api-0cf43eeb8dbb7488"></a>[`Inno.Native.Bgfx.bgfx.Topology.PointList = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L301) | 当前常量或枚举值：`4`。 | — |
| <a id="api-d1c5e54184405351"></a>[`Inno.Native.Bgfx.bgfx.Topology.TriList = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L297) | 当前常量或枚举值：`0`。 | — |
| <a id="api-d8bd3bd86e8e5c95"></a>[`Inno.Native.Bgfx.bgfx.Topology.TriStrip = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L298) | 当前常量或枚举值：`1`。 | — |
| <a id="api-dd4215fcecb7e364"></a>[`public enum Inno.Native.Bgfx.bgfx.Topology : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L295) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.TopologyConvert</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.TopologyConvert 864d03c1bbbe6323f91f97def8caaa4aa27e9b11c3a3417e812a581e79a6f230 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologyConvert.TriListFlipWinding 407af5fca2d68a4794211efeba339583c97f4bf1eefa1d3fdf884987143f3532 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologyConvert.TriStripFlipWinding dd73b2c49ea3cb46e387df9c89f7de7560e644a514d0560d79d11f41ab039ca1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologyConvert.TriListToLineList f4b6619f31a0b0994bf5d8e666a690298190e19725ce286361ced74a6e138743 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologyConvert.TriStripToTriList f87790d49bebcc18c029a1a8464116741a738d408fb4457113b6b956e9d24f48 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologyConvert.LineStripToLineList 5d889e600568be448aaf19f0baa7938663c3316bdf5f51b9eb4b802e449456c7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologyConvert.Count 9b718b881f0894339663d62f133b1ccf13b06dd303f266a3e5be149ee33b2d35 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-078a504d709ca7dc"></a>[`Inno.Native.Bgfx.bgfx.TopologyConvert.Count = 5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L318) | 当前常量或枚举值：`5`。 | — |
| <a id="api-f98bd84126ccbf84"></a>[`Inno.Native.Bgfx.bgfx.TopologyConvert.LineStripToLineList = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L317) | 当前常量或枚举值：`4`。 | — |
| <a id="api-649541ffef6496af"></a>[`Inno.Native.Bgfx.bgfx.TopologyConvert.TriListFlipWinding = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L313) | 当前常量或枚举值：`0`。 | — |
| <a id="api-e9be8fb3cdf253a7"></a>[`Inno.Native.Bgfx.bgfx.TopologyConvert.TriListToLineList = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L315) | 当前常量或枚举值：`2`。 | — |
| <a id="api-35cfc4d2a15d09f2"></a>[`Inno.Native.Bgfx.bgfx.TopologyConvert.TriStripFlipWinding = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L314) | 当前常量或枚举值：`1`。 | — |
| <a id="api-6195a9a3e112a374"></a>[`Inno.Native.Bgfx.bgfx.TopologyConvert.TriStripToTriList = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L316) | 当前常量或枚举值：`3`。 | — |
| <a id="api-e729b6d09a63ffbc"></a>[`public enum Inno.Native.Bgfx.bgfx.TopologyConvert : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L311) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.TopologySort</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.TopologySort d64ed77d22f8588d9d59394517c699909e7c45175d43ac64e89c28346e192505 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DirectionFrontToBackMin fa755080e850123314f2e5ae7751fb472e250d57c1f2d75bcb6eb67eb472b5eb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DirectionFrontToBackAvg 9dbbf36154e5dede514cf178b52b764fd089089bf89467029fb14ebb4798c709 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DirectionFrontToBackMax 7ea35327f76933f89c73946d35cf44441c4beb5945942a1d2618c6b41188c7fb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DirectionBackToFrontMin 21d968613b62e356209756f755b6d6d1883252a6d764cd09a0bbca8ad5b495ef -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DirectionBackToFrontAvg a07572c7be4b58061558c6d295539fe57759e5b3ff48b7bd0934fae3547cd03c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DirectionBackToFrontMax 3ade8b18fb6bd03ee8ddf05a8c0044a2c8f7c700c203fde95cc79d8bf5d14e6b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DistanceFrontToBackMin dfd91e4acbabe6e2fc5b87bbec48447862a52ef8e0803da5315c5b6a9a8e561c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DistanceFrontToBackAvg 0ddaff404b7aaf8dd767b4f50f1d650f4be572134e5d30c95f0dee886b2a5bb5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DistanceFrontToBackMax 6398682b7e64c994552456f9f93cd9f4b87f70ef848ff0876b40de44b390cd75 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DistanceBackToFrontMin 2058af260d82c2d626cbf56588b947c07af3de916a0c575b912e8a5a65320fde -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DistanceBackToFrontAvg 4572d942a9bf3cdd6527bda329e1399451a0f7446d424d4b77224c7d192637a0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.DistanceBackToFrontMax 92a2f22b4e9644c704de570821d89c8b91ed2be746bc89c9962314102fff594b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TopologySort.Count bc15ad655c5bb25793c8e0182d3bcbe25f114660daa71de243163698aebe6a19 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8efbddabd9c57232"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.Count = 12`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L341) | 当前常量或枚举值：`12`。 | — |
| <a id="api-2eeb987b42f57866"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DirectionBackToFrontAvg = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L333) | 当前常量或枚举值：`4`。 | — |
| <a id="api-497f27adb1e28d74"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DirectionBackToFrontMax = 5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L334) | 当前常量或枚举值：`5`。 | — |
| <a id="api-a4c37f17a618963d"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DirectionBackToFrontMin = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L332) | 当前常量或枚举值：`3`。 | — |
| <a id="api-b89e9170242435d6"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DirectionFrontToBackAvg = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L330) | 当前常量或枚举值：`1`。 | — |
| <a id="api-1ff53c9c42b9b8b7"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DirectionFrontToBackMax = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L331) | 当前常量或枚举值：`2`。 | — |
| <a id="api-b4b10834784764a9"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DirectionFrontToBackMin = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L329) | 当前常量或枚举值：`0`。 | — |
| <a id="api-5c35f2625aa1d7c1"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DistanceBackToFrontAvg = 10`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L339) | 当前常量或枚举值：`10`。 | — |
| <a id="api-3408d5e9a35f1014"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DistanceBackToFrontMax = 11`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L340) | 当前常量或枚举值：`11`。 | — |
| <a id="api-33d29641e9b02924"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DistanceBackToFrontMin = 9`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L338) | 当前常量或枚举值：`9`。 | — |
| <a id="api-ad07e5ba5b250f24"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DistanceFrontToBackAvg = 7`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L336) | 当前常量或枚举值：`7`。 | — |
| <a id="api-4ba5af0aea45ba0e"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DistanceFrontToBackMax = 8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L337) | 当前常量或枚举值：`8`。 | — |
| <a id="api-cf4daca8ba2cf2ca"></a>[`Inno.Native.Bgfx.bgfx.TopologySort.DistanceFrontToBackMin = 6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L335) | 当前常量或枚举值：`6`。 | — |
| <a id="api-162b3387b2649ac6"></a>[`public enum Inno.Native.Bgfx.bgfx.TopologySort : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L327) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.Transform</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.Transform e12df84d64fab6a43456aaf4e086735bc523f9c920a2c1b2ea6d5fbd4577d0f3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Transform.data c06a6226a27c4cf76802b790c233cf62c99d3ae8ec11e2b4c55eab1906c59dc4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Transform.num 18bec64d93169f71a8643ab76a6e716ca005ffbd852e7dbe9f964b7ff37427e1 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Transform.#ctor(System.Single*,System.UInt16) debcf5ffecc7a6cc8d25c1e8458a5165ad3eb68177f67d590a811cf6bf2b9a44 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d84e99e0a5b3dc59"></a>[`public float* Inno.Native.Bgfx.bgfx.Transform.data`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2866) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-263a31c9949380f2"></a>[`public ushort Inno.Native.Bgfx.bgfx.Transform.num`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2867) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5fc50c8d912c3e6b"></a>[`public Inno.Native.Bgfx.bgfx.Transform.Transform(float* data = null, ushort num = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2868) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-26ff34957cde3a10"></a>[`public struct Inno.Native.Bgfx.bgfx.Transform`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2864) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.TransientIndexBuffer</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.TransientIndexBuffer e282a4fd8a3079b67c73145632f78737ca47dca1075e9ab6980b82f37d40af57 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientIndexBuffer.data b5757e0b52182b3207867dca9498713d8df6df4d54458f4e5dbdaf65fe8b8fc1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientIndexBuffer.size 83b022907135455d5c06fc01f428ca39a3d9d266aea5dc0075c75caea8ec9b9d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientIndexBuffer.startIndex b8d7a0c813782e8f0da1f9fdf3b3b8cd5a74108740548dc9b9629b90ad322efa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientIndexBuffer.handle 705989c2ad12ea5f266b39e10c471c57abb7e9df461db5d8d10c71712adb5347 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientIndexBuffer.isIndex16 07549f62cdb4c7c3d837381d9ac4ed7d7a9f0fb45b29f748e7151fce1d07b309 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.TransientIndexBuffer.#ctor(System.Byte*,System.UInt32,System.UInt32,Inno.Native.Bgfx.bgfx.IndexBufferHandle,System.Byte) b0af8c3e84c5bc8638711bcb51ba16f04186c002ac26d2022b0f96f0b928a29a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-18f1802cd4397c6f"></a>[`public byte* Inno.Native.Bgfx.bgfx.TransientIndexBuffer.data`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2087) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6980ccbf4534b354"></a>[`public Inno.Native.Bgfx.bgfx.IndexBufferHandle Inno.Native.Bgfx.bgfx.TransientIndexBuffer.handle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2090) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d13cb18885a3e03b"></a>[`public byte Inno.Native.Bgfx.bgfx.TransientIndexBuffer.isIndex16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2091) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc30f972af108b87"></a>[`public uint Inno.Native.Bgfx.bgfx.TransientIndexBuffer.size`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2088) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e764b950509f125"></a>[`public uint Inno.Native.Bgfx.bgfx.TransientIndexBuffer.startIndex`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2089) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64085934f13b7e6f"></a>[`public Inno.Native.Bgfx.bgfx.TransientIndexBuffer.TransientIndexBuffer(byte* data = null, uint size = 0, uint startIndex = 0, Inno.Native.Bgfx.bgfx.IndexBufferHandle handle = default(Inno.Native.Bgfx.bgfx.IndexBufferHandle), byte isIndex16 = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2092) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a0ba6455f8045230"></a>[`public struct Inno.Native.Bgfx.bgfx.TransientIndexBuffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2085) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.TransientVertexBuffer</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.TransientVertexBuffer 90d89e91159347d5df35248da02438eafcfc570cae08d9de93cd41a53f4631c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientVertexBuffer.data 8c9705f06756025e4bbdbc41fa6a9286b2e73c2af23b9546827292ffc59c689b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientVertexBuffer.size 7dcb8245b540e2e0682beb772d9e330652e540317c0ca3dd329ae2eabf472791 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientVertexBuffer.startVertex 82331946ba9cf6db26ea1b23350fc2790c35d7234ac758458fe44eac3a64fb5f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientVertexBuffer.stride 5788db19cdd0c53a1a10f4aca847aab0b0fd4aab48441146962f8cd9437078fb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientVertexBuffer.handle 17c8c26c7f3a658ba7ef23d3286c4ecf89cb208493e1cef92a125ac6a350eac1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.TransientVertexBuffer.layoutHandle 2cb8d23c46aaabdf8733d718c89faa58880f7ecfacbb04899034eeae279c0647 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.TransientVertexBuffer.#ctor(System.Byte*,System.UInt32,System.UInt32,System.UInt16,Inno.Native.Bgfx.bgfx.VertexBufferHandle,Inno.Native.Bgfx.bgfx.VertexLayoutHandle) 975876df74053a1d55c90f4162d2c27d946f78e2b7717b0d8700bb270039b94b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e120c0c6d8fc71dc"></a>[`public byte* Inno.Native.Bgfx.bgfx.TransientVertexBuffer.data`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2111) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4146223f8a0b1ec6"></a>[`public Inno.Native.Bgfx.bgfx.VertexBufferHandle Inno.Native.Bgfx.bgfx.TransientVertexBuffer.handle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2115) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ef5a5e97ad61e13e"></a>[`public Inno.Native.Bgfx.bgfx.VertexLayoutHandle Inno.Native.Bgfx.bgfx.TransientVertexBuffer.layoutHandle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2116) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe83589e28da09b2"></a>[`public uint Inno.Native.Bgfx.bgfx.TransientVertexBuffer.size`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2112) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0581d218ae71def8"></a>[`public uint Inno.Native.Bgfx.bgfx.TransientVertexBuffer.startVertex`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2113) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2aa415839743c25"></a>[`public ushort Inno.Native.Bgfx.bgfx.TransientVertexBuffer.stride`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2114) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2aeb607914fcfaa1"></a>[`public Inno.Native.Bgfx.bgfx.TransientVertexBuffer.TransientVertexBuffer(byte* data = null, uint size = 0, uint startVertex = 0, ushort stride = 0, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle = default(Inno.Native.Bgfx.bgfx.VertexBufferHandle), Inno.Native.Bgfx.bgfx.VertexLayoutHandle layoutHandle = default(Inno.Native.Bgfx.bgfx.VertexLayoutHandle))`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2117) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f234c99752c35ba0"></a>[`public struct Inno.Native.Bgfx.bgfx.TransientVertexBuffer`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2109) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.UniformFreq</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.UniformFreq 80b531f12acd02ea113a0455d26bf827ff1fcf8dbebb73f22c9978db86104712 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformFreq.Draw 6a7bbf0faf3dec1612b5db77ddc56f28885aac3c187b0c580f9072dd087a34a1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformFreq.View bbc4ec5ac3a55dc2a6917dd1793d16e8d115224a904cf60af8dd2337e9d3c365 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformFreq.Frame d32b29cc1eb45acd8b8b8ee042d0797ba92e96285f17e1464ca49a9d8a050635 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformFreq.Count 03b9bfccafe686e2f2fb9ccd2d0e55c9b7d8286f6d35439ea88172e7a33b038c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9522fe206af8d3a9"></a>[`Inno.Native.Bgfx.bgfx.UniformFreq.Count = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L255) | 当前常量或枚举值：`3`。 | — |
| <a id="api-39d05e005bc7c1c0"></a>[`Inno.Native.Bgfx.bgfx.UniformFreq.Draw = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L252) | 当前常量或枚举值：`0`。 | — |
| <a id="api-6a4463d1c89bb01d"></a>[`Inno.Native.Bgfx.bgfx.UniformFreq.Frame = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L254) | 当前常量或枚举值：`2`。 | — |
| <a id="api-4edcc4f23d94d8bb"></a>[`Inno.Native.Bgfx.bgfx.UniformFreq.View = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L253) | 当前常量或枚举值：`1`。 | — |
| <a id="api-0bc9b2ed802ed768"></a>[`public enum Inno.Native.Bgfx.bgfx.UniformFreq : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L250) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.UniformHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.UniformHandle 69eebfebb3b14bd64934a9ffa44c414104635fbe3b5387968fd8ec8b6d03897f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformHandle.idx 13bfbfa32beb43dbf3477dbcaeca3d1ecd2f7368df883a01da9a645799f511ea -->
<!-- api: P:Inno.Native.Bgfx.bgfx.UniformHandle.Valid 8bec9142ee773917c506196527301cfb5ef12bba0e8cbc1764cffe6a95ef5967 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.UniformHandle.#ctor(System.UInt16) 3ff25aa4868b45c0b938788ca602cddacf6f0d2bc7b15d8501088c984244140b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c713a911e39ab8b4"></a>[`public ushort Inno.Native.Bgfx.bgfx.UniformHandle.idx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2199) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8cb37bd3e69f347b"></a>[`public Inno.Native.Bgfx.bgfx.UniformHandle.UniformHandle(ushort idx = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2202) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-634c6f615557c5a0"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.UniformHandle.Valid { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2200) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8d20a02763d90bde"></a>[`public struct Inno.Native.Bgfx.bgfx.UniformHandle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2197) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.UniformInfo</code> · 261 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.UniformInfo efa8f8b5463984b7ac459e3be6bd3c5eaf1916709418e96d92e96a5b6592ddd3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_0 f7483dbc5570ba78dc35aaf86fb7db1ab91a3117f7fd0cfb2af2472f3d711764 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_1 51f87342215f8e467d563dd64d10d7e69cb6eec5d9bca831281c438254487c79 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_2 f6449ad8dff925a09b981842d749c11f887c93064dd011a204be33e389bb990b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_3 713fcd93b42a303dbec1c4658912e2265e0e56ecc9d3c67ec2274282fe1d6c0f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_4 e2935bef52436871d3bb232092b4ea914c7eb7431a143cf8313e49b21669a255 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_5 f6a3a1da42ae76b2f915db6824db15a03d661d76cf1e0ac9867789b9ffe2cbda -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_6 ff210a80944a7da7d27b4b3b25d48262054389e6e9237f00964cf153ced1b45e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_7 f440849a9563e0b38d08f050b6d7a02ae17c4ebfc73a94f00993012c99cd77fa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_8 a9c1b871b60c1a102aecf57a9e3ca684ebfd0d2b9839f7ff0584da57c5675bca -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_9 fa3de7bf607c57370a62f45b1ca804bab28d26c51ff7693e3f74e4dd2ba8f29c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_10 d0d940885e6abf535f3a083fbad8ce5e77f919468f398e03a58011f16776f732 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_11 e7fad0f41e6bf69e7745f2720d17a0d45da9ef202d6e575875c1c834c594a394 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_12 9e74a52a1b59d22c995025923b1972ca1cb838c6c015bd92aac32a2c30069f47 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_13 4a1baad9603ed12c5fe3c54c32fe8d763f8d74f05990f624f5fd6096ec0dc2dc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_14 4979484e8d8fb6906438954e72bec193c246d3218386fcfceae78a85a0d4783a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_15 d1d92708a43e4182cc2b889da8cb90c1021202b644a8365927303216d675ee1e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_16 6079ebb748ad1b7da6939ffc0b617ba9178966b5ba4c20f2f98007475674b3a3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_17 6ac2634fb1b9a54644407506a0790b92a31b3217d174700fb09e217fc4be73f4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_18 63b0747fa85454c4197250456146e794383b1449965929870bae271efa071c1d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_19 437d709843b987478f0546ade9f5f507283a2d4ccefc1212085a8dc0c77deca4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_20 c083ad0978c4aa4b58fae8ffe21a35bf8cf1d3626173c73d921eaacbd666f735 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_21 1dd3a11cc08146aecd9c039a4337bf44948f53b07981a9f98ae30c2010c0b1b1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_22 d89094a50ee1601e09efcf3477870609b388bcae64d41ab7df1968c0ddaa2318 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_23 2046673b7c0b7d1cf473ec75b5a1fd6156bee9522824ca1d1929fca8034dfe0e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_24 a733e71cf4c1a5e452fae8c6eeec7e6e31696bb59af6e21106f61b596b33e8b5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_25 fb2b469c4acabe2dd190c75cf65ddddc0e31dbe129f46bbf766f05ee21617be2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_26 e00ec0d97c3028aa9c45d6fcdc8ed5dd01344d74aa61d9ad93ce6152c3010ca6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_27 0d506883d8461190dbd8a83c071811232bb8a0e252287fa0d4fe1c0b85c6448b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_28 2147af4b0d5f00002ca0db73663298e49dd034a0263c02cb3946aaedc24fdb9c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_29 2190446e18715ea2a9102165783a096b21eb3b7a534b6b1ce5a2a4cd1f51dd50 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_30 c7177765c492615454a017d958959a5a3f50213737a13468dff84c87af02063d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_31 89f66df97c357b050813b595cc67c7485ae7e984539fac98c5d44bbef5b4cdf6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_32 aa6d60ede6b45b815971df58899643376b30f9de183ccc5fb2436c9aebcdcf8b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_33 e10b46c268b9228b8e7cc32b5e035d7a7553bcefa5abeb68e12cf8bd18d27149 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_34 b723114f62ba8deb2d8eea44d04046249be999d92f0387bbe489a695979c4f35 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_35 9d422c6277f67979ae454390519ec955ae95bd2a29e26d7027dcec21cf04858d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_36 92cc48d3d8dd56e2eb782311c8d54bee69bca4bd46c163ad3c3a1a873f7ceb0b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_37 def7a75c299996037f0e1f54ce96c1af9e0720697ae32a6f10927ea274264ba2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_38 0019772c18395cc96fc82bd152f7839ceeaa9c8d1c428e9804f03433dd2f9e70 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_39 c28beab478bc8666f5b4693ee3aab19cca03593e273691f9854caf705c5acf1c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_40 fa088adfd74a6fbff0ed10ba1d085249397ef9b6e7224c5506041f406686029d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_41 696d6a99fe565709f111f3e8ef8d77cb951dcd33220cba95d44fcd5c46000969 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_42 f5655d0672df6c2c4ff9c707e28841775765fec20fe6cd1b3ec4b9cf7f16edde -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_43 5e96ae3277dc3902dfaee5aa73f1b06da7fa5dfa92ab3dc7562b90630c712560 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_44 655b9fc705eac675009eb745f78f99167bfd3a5757ecf56aa202734c9d18fac9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_45 7ca436f8f0da2f0508d83c82391cedf82bf3f4dbab268dcdd96478ea9c05f04c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_46 9970be7a71696afe6fb10b237488f74a2c370bae55fada56a3765ab60a3fdc5c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_47 865d0a0a13ffeac960bbad8380ccf94b5ab475a883d85501326d18a1c5d1bd5b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_48 cdc7f4dd1638a1a22152bb4df4cd97abe1f4f1aa82ba30395dcd632f3516adc5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_49 5861bb8a620cda1d9637e64a6a13f876745d37493eddd46f348ba7157199a374 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_50 cff6dbc805f6aeabaaafdd720722e144232e2faf265420667bb89976a5730806 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_51 ce715affa2a9050bc44c00bafed12b0ebf216538d6b81eb5c8a43b3435967a62 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_52 068320289af882b4e728b76a0b5b3a7daa497f775a15127fc5051827634bc35c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_53 e456140ce2efdd040daa7124b3d9a0c9cf8ec4a9cbebf84f0d0f788c25f770c8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_54 ea51035b4093174137b0630e7d942794e6cbfa11d216e8b131ea595d3082805a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_55 8aa21d3208bc885c30cbeab2508cf0c92baf92a2814cfd917515783552fd1b8b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_56 e57b065411c4cb7bc659df9f87fe9f47d92f83e8af3c6a65303451c402b3a1f9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_57 65f952b33ac99f3e421b3b90bd1e379f083ae4a757e48d318922224c7544a346 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_58 7318d756687e15b6dca7b5735ef9ef1aefca8903b2f6be9936e8288309c280bf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_59 59e3bad3593861da9bc0694e033c597fee77700bc37d96b85771d9c90a028a7a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_60 bef5831a7481ac94bc7ddb169ccfe69e66f819e9b94d5cbd6c560981b9eb7f12 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_61 07c45bf51edebd6abc0be659000e8e889fa845a91cc58e679a3d9d7132d8cd02 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_62 c16079a0484ab7fc48a09ffed8212b1e253471c77c54c0cd0cf25794bd647730 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_63 1ad7c5f7d7fd549b94c645c5a17576edad233683885da7882ee9e98907c1163b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_64 243a37a0c24b87d816d68cf5caf50edb84c764af9a5ab96116c2cc1c743c8cf4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_65 ed8e21b51664a8755b3859226d3e55c74e56cdd4e19c9279917256d6fd139def -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_66 02ba461018b71ee1c8660f47dcf38d74b11e6ad1c5af74604a4f1277df8b7d01 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_67 0a29897921060a113039d93821a13ccb4f71eab346289cac8159088efb95d7b6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_68 2868a9f5b37a220c42ede85d0c4c3326b4bead69cd28cecbaac388ee8ff4f408 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_69 189e8dcf21fadf2cc77018d017f217a2473ac79e9c1bea446f67537c4b7968a6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_70 51fbc885b2e9efd737f33ec721e3319c8745f49111f6edc550f368475cab29b5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_71 9d8e8d3ea5010905d95d8fa156b6a9b2ab3613c3f4ecdeaa0a0db64b7514b41a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_72 71f59eca82628447eb854de1ef2c7787b29a66d5a606139dea59b6bc9113e4b2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_73 cd65ae6f50cb953b4c0ee059390da8dba9e88c93067b34fc6704ea048ac84297 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_74 d680e0caacb30391fd10d264ef43141f8dc0dfb94fdb8236d92a468a8dda481a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_75 89b360cca1fcd3a685d43e59628f30a643159537edab2062fc97bf8f9a9365be -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_76 795d04987d9a83920672f86552388230d08923859b9abd77d3b7838276ba1d7d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_77 5061d3617bacae7ce5014ce491e58af3f0eed31642db6892d17853d6383b36d8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_78 23f7fbb0d4d41bdccfb9b87f75159a1b935605ec2c037f38866b750330643943 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_79 fd301c1a179bc7d040cb23b8fb9ccf83ac5fff4dd7460510b99ec2ab9a02dd3a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_80 d6fdb9945133c6428a477758315304b2251a75d463c642f6f56f533c372b70fb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_81 6cd138aa5426ba6c99f38f9e70e7b4248a1c70cf7eceb45a7e532eee9f300342 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_82 c7d208701b173f7ef387d7716bdd10e6298a4b2833f313e1a2a641448b4ad8d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_83 b8fa54710b25d913a938bf96dd243b6290b981e3bf4a0d94f3ff2d809b6dde29 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_84 519c78f7100e73d96cf63730f396b398f1c2948f2ab74064f11c1d883e2e2ac0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_85 9818f61581afa370cac801bb14b8f0ead53a634f8d964b44f7f7fba1af795f2c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_86 a2b7a81179bb4db808fa50b7fc9739e025832d41f1101d4182437a8796ea4c50 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_87 1d0640e348fa2f3158003b56c7fdee1928a6bac0f57dc7d8ce0864985ff121cc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_88 37c81de0e64fe214af10b4427ef864cfc8250cdaf824f8693588e81d44111086 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_89 ce574dc79c928464c31996c9c428c6beb5982364a1269f469ad178d5f2802e79 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_90 6409b17e3cbd0740a544789b0870112b46b366daa4f77f7eafaa6bf389e341c9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_91 86c1111be9355c8ab8c6c361538d9712ea690a048320c869c0b098009ea62bf1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_92 b5d4ed9a9b8a9a013b62f8b76eedb087375d9e3ac854eebb8a84d75bf491af2a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_93 3f795dd8d2081f41800ad7c346c32befe96481d83e02eb433a54c26face6f08f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_94 42c9a55a3ad193953dfc34944d0a6a9f9df5c14e266dae2d4770754b50594a0f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_95 65ce7f44626c2f4a6bcb973d6beb03463383a8809660aa06ea68f34c18776837 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_96 ce65428cb0534321d24fa15d85a35d603875c4d53ca92a42502aca4c4df86595 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_97 cc4195eceb902262ad5fcf152c254c49bb1b461e9832f5a0399b2a8f09dabd91 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_98 eb018d6e0a605b0009aa999e92dee24435a4693e19f8b77baf70c32e793f276d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_99 872fbcbd83ee2033df2ac33911193299d1036bef0769f00292fa3f4d7c156b47 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_100 fd425a724663b0ecb0a83e13118f8e0652daa4db4742943a8cdc15bf39dccade -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_101 4315d6a946660d4401646aa2a012a7f127cb50ad1382e522c7d129d4d9b64c61 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_102 db1f1bc91bd79464bdcc6faad8f889aefd31ba85fbfc5ceb3285c0587bd37dfc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_103 b478f25891415ff006b08a4b001d7050cb76be289cd0185ec10de6cff2cbe61a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_104 e033795fbe1a5b2d151de3fe9daf3b48b8801aa8f4d8cea770e24bf18a07e751 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_105 fc9843690a5a231e3cacce8c36ed1da9db53d7ed6802565414f57b8412ba11b4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_106 27736b6943493f14db638087d57841d5dd3788fd23e1afbc6c25cdb2cff943c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_107 6c71f5259b16df17727d3ab2bf34f74c807a7d5d21571047fca0d1b0831a3185 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_108 a4d407fba2a74e6209eca3562c01828b1ef8cfd2ec81474cf8890149d007e1c0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_109 5396e6a8b9e472e4e9a23f3fead311ee914fbca3af9568a14cef294030ca1817 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_110 4e9a9d3d87fbd4bf86a2a40bf5f9166468a52e0b6eaa73ec131bce5ce428b3ff -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_111 659741c6447d976047b688c778e35a373c239c4b3c771868aa80a9241523fd27 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_112 1d9ec60a9cc972b7d821299f7dae794c620e7021a0abc56405349e0c6798fd89 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_113 39fe69d5b350aae1561201a6a2fe330f3628f9cea796f33f5724cd5fcf8263f8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_114 01822b3b58d565c6cbe5c8a72c5c7872613ccf0faf5a7929637129abe2a49d4b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_115 c673329bd89cb4bd0c30217304198e3f1d38e5d04e8bb9a645e9a034c3042591 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_116 8a7196b74e23007ff6f47c3e5e8a9219f531dc41132f529f908be471f85c06a5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_117 bf4178a627b0a873b85604b52cb2dfb98d6383be3ef624fb5ee92318eef6f3a8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_118 ccdc9f8054b25f5feb36b3cf9cc3bd22ade31d9b744ca5c4313dd0ca1bbae862 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_119 911038311edb82f30b83a0a4055fd4ae24aa462ca5cf6084d4db7309a9dfe6ec -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_120 5679b55feebc6efb585e7f971049dd798a3337e0a5b62bab8b107c0a6b8456d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_121 66b508ed7797c36f60893c431815355caf705fcaa9fc10bb6e0f53ac9e95cb14 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_122 20c304c9147d066e065cfff59f2333bc3fe9b9d31d84ac7983d24c0b3bb3e30d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_123 2aad7d31e552e9621446ac46a6581d9ec54fd42574b181f35258d1b80117071b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_124 6dc2902b591a3880011688f9c1d7a83be6ef83711c7b1770b0df7e869f8934ea -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_125 e0e2aba6efca60353992dd8a137fcc6eb2d3d58cccedab94ed253e9d77e196e6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_126 f59582552cc95b5926d39300990ede59d239345f0282aa399fce0e985253d437 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_127 2d2b47643bc8f2a1b5605f6e965b285752dd499358e575669e2ee488a69dbf8c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_128 56e36be78c538e43087b4552814463e2301fd6157e4d3bbc8c2abbb01c6645c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_129 436a90086700b7e75771f7faae4471554cb61558af71a54425ddedb736edcf7b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_130 186cf9ed3e45d9671758404d362b64da69377ba235d073a31d9108e21f43a9ab -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_131 78f34efec9dba6c018c3b1c02d6140623a75f18e07cfa03ba90c0dbce30a5834 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_132 d3a33c42a58a2f6ce2542120490d069ed09e2394fa863c94cfde5d4db047ba01 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_133 d9b537c8fd0be94e062720f12dc8bf212c32aa4bf5cf0f94e1f437fa70d890ae -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_134 22b87bab6ea112d04a4cbd0f8f097208f81927ffd4379563938cfe65b4d11104 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_135 5f414755a95d072e66c25295291b3d3c8aae776e82a076abe5355a52d3c00978 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_136 825872d139d0148e611037ae9a3a0ac92206f3724212aebec1eb99256a127eee -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_137 d89e1503cb9e6d593336936eb8334d6dfefbf2f93030f0867805adefbcb20597 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_138 45cf07f451a0f00dc5ab8384a91954dce2a7b742e2004332cc3e8d0012f512f4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_139 deccc8c3402602fbc366c9d2343412d6d67091946a1f3834cb8ef594bba62eb0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_140 cd8744af4916ddc74e2a158683593afb0859af0e70ed852f09854731b2119c34 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_141 66b2132afe9fc1e8dbd344d01c2c994602c51168dd63653d193ea224cc28e25b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_142 c9f3c1f99516a645ea1724346b1ae30d5d8ba211e8a27c09804509948aef8e5a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_143 792ac2b71adf0658deb41bfbe8eddd7073aeb8891bdef0d297a15909d7e74717 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_144 146e403f0aceee856474c65ecdf0e135080139ffbf3349c94c0880870b87f2e1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_145 c6cf2bc30e94c5939fcefbc8b872784e4828477a422949a5cf6a452a523793c1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_146 790c22b0d1da6163402e93dd1648f5accbea8360a695dcfef9e8c1aaa787ffd1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_147 87cc6ba3d5b8438ba1e7ea3b0952e82573bd4f16012fc9ad817ca91901edbfab -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_148 9fb8e0992af509938e9603f953f8b8c0e6a8b099252a9dfdbebe6f944dd65ccd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_149 acb984fbec0d92174f1b14a0b65fecc69fa6c9c9bb6176e4be834b003d0c1dff -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_150 8d6d221185929090c0b7bcee2616872d5f80cf52fb3ddfdb60258019c2649b70 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_151 db5e1c6d4e488c27c683cef672813c0af0b06d39aa0aa4cdcb2cc3d8278a9fd0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_152 b26751311e344efc1d73ea01e8b3e5cc75e045e1c2e50d5315e7d72891d10f35 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_153 ddd2fafc5e1591407a3feba0d7b55d714a5af0ee31e04d2ca4eb8e24e5a88779 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_154 7fe9d797eeec052af54c6408865eace4d6671ff3367bf7b0b2b7b73de7efa369 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_155 29f2b1edf119f85279f43857c1fe64eaea2e945e0577fb793bed3869624b06f9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_156 377cecf41412617fca659222b298174f08a0c4d397011ab948b0843eb057d657 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_157 fb5ca9c3b17a99185b7fe90738bc65863c13c1bdd47b71f74d86294e04bde971 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_158 bbb566f0dbe38c11d1e66081544d41559c60c780c8955ffc4f444b456823e2a4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_159 d2bf623f77f676f73060f7b449ebd73d2f8ab44f87a278489bf4fb3967aa85e4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_160 f590b517a4c690bc78afce7ce2892e1da298ffe9f173c19981d61b890f30e1ef -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_161 868e40530d8dcde1d8122a525c9481c370f9fdaf21dfc85f872af57f49256bba -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_162 fd583b2322a76c130538d0d518e828e5af5110e0b8df235a20c40c3e81bb0c8f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_163 b2d7ba4ca4823c78a975c066ed27fc93efd6b1e583b057767b8e59a00817fe0e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_164 c913384f22733553465080bede1edd56f0f7d7fdd653be012b12c07f1d1dc271 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_165 b95c8f554cf029f681513dbf807b4d1555f6ac6782485197a2c6f535e67660e1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_166 4044e8e51549faedfa9270e2658b59a4c75d594d54657cb67cd5ed32840dfe0f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_167 2900c901a69013bbaf397cf5fdbad7606276b2fe73a5d695c5f0e28327deae06 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_168 aaec660e55fbec4e0646ebd257c6cdcf57e53ffa4be8a04ced372b915fd8f5f8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_169 4ea3dc0619958f2836c8433649516acc8b2671bef578d7acd06681233539e1cb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_170 7521a3da9a52d5194b17dcbd5a44cc4e503a6000cb43432234d0186554b8f9a2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_171 87f405c4c3bc89acadf45fec9147bc02f1d5581fce81377bb3e0280eec98e5e8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_172 ffd261e1e7019ca4d8d17d2d686284deb0e9e1d0ed9cd166e6cbb22de5c14954 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_173 5c8f14f934ce932831b8cf9261b754a5930c7fc802ec21e7164dc9b9dc01ac03 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_174 1409a34274882454f88115415001e9cc37d253ebbf63354703526f7db9ae9335 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_175 9f5d701461fb6e65d15c916c9b6b0a7d365c4fbc6d5f30cd2147e6ca76f79455 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_176 b28eb37c44cc0b815e8484530d61d61ce65a3914e7ab19c9f4b31e9a60093a03 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_177 12222b8078ff82a89dc47336563eb6c8c4347a8782f9df5e5a71165af840c9ed -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_178 c78fe459bb4d82a5c323519434f59b517b4e3f49f377322760082575a32c7091 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_179 d9c130d6a883c16b5a89ae8d52d51436102c20501e94e63a4c9446b631f3f143 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_180 40d95df48be241a372b8fd4fb361471c146da11464702219ef674b68770ca612 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_181 b2d83fb07cdec57586f47bcd2b143a558984b272fbf33ef98af0b95fb9985997 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_182 7f071f407c5002dc4ee3b29749190604d1b0df3bc2797b31f0390ccae0a6a18d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_183 89552bd5d22974f3503551c51be661b46e8ad1a8f62ea906ddf702f68f598c89 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_184 81cdeb84406459b71c41f9a0b2c83c4d7bf430752a479bcb4a39776585f22abb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_185 694da60a1f3ebec403e059b149c5c90171834b9e0a4eb915789bee1ddac636f9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_186 f0953cf0ec7d724da66693f215c9d0836c072f2f3c0bb1cd42f8eedd832634df -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_187 1bb62383ab54ca7014d601b3b213a32c09c50e8ea00e224c846da45674b82cfa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_188 dbab62708446647f58c681fb15adeabf75548ba5f6add544d854ea8406567d9f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_189 53db30a68bd195bd4bd60570215a2fb1b2f82802ee2b06759a60f54c3975621c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_190 24124e60e659371bc903790d910858bc64b4fb88b7b90cdfd5082fdcc8ae92eb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_191 feb6e15ef95f2e0568b74b57ee1fa0e5f8f8f175267010a7dfb8ecaf28afca8e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_192 1480f13e99e9fb65ba5d7170fccaea10a14bc674c88af35767294f31565d5d18 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_193 d3cfcc3035ef6835e1ba7d3ebb956e756f10649e48927205bec42d6325af24be -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_194 c0da32f986990d70675bd2f890af23341f8fa2c511c1f59eb89e2c3135f2ab08 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_195 c6e8831acc790aa281b093d4bbc0253ef7c235ed052828db653ae56a05859b67 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_196 7edd6c72d7bdd3c3eff786df46b7007b6c11f644a45f66151a0debc700a9e7b3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_197 86f41202d63244baf93a64d23576585b0cb39cfc4c2678a84feb5f3fb917635b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_198 ffc1a2ed5245e43486f2a1d56cceb64815de0e25d7da269640d2bcfa32e7fb3d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_199 b4ede0ee10e2c6381b7d3018ebaca306c2cc53693f5a95426a2c733874ac2e99 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_200 b22dc35b9ae180ae24d20bb88209eac24d8a6406b8be833b62202b64121f01dc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_201 6c5e6d1e6538153aa1a19ae543f0f5bb0cae15388647851837a00973ce8c1449 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_202 9feff8df4f7dd94960d50bb2905323a7d98f611dd900ac116f7a8a926fdeb0d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_203 7cd06c12c4baca67e182d24374658678781a94abe685012b9c79a167d8ca0f40 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_204 48154a68075aede069c51df88ada207f24a297e0d39e12bc3c7a48ffe4d1ab6f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_205 3a66156f16bcf600a7196e1000230b649dd001f95d50de818b463917c85d977f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_206 d83ded5098172d5032cfcd78522eff4811c4492279d9a1727a061559ddfe632f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_207 d0d9ebfc561e99fdafc8acef1ea15e87a57b73340ee12bbaf2abb53a39b8540f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_208 403299839bb953d3ae36f94ae1bcbd17c16560a027220f220092dec55d937b38 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_209 c1db0554ff3907a337ba15cacbe4a6fe418db788c0be05cd89c278fb76caadec -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_210 cc2e984acaec8610748a6ad1309e71df37df73cdf561f01f78c0bd5addbbc800 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_211 e5865f3cc3f096fd0207fb3b51388b4cad5fbd2dcbe508ed3cad2f6109cafb0d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_212 07c73231f2ebebb958f5764fc7f0e169ba1ead33f8a0f84e6521860860e1bcb9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_213 5a50e94ac6a224349102886f2177b3b77001cf07ca7366fe304acbcc106aee4c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_214 968cdaa5c352cb0a3bdbe6eb8352030685e9213ef16c8003fb59b287a93ef20a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_215 ba53c723480bcf86947e254eae1d7def9aa2fbe55fce9e2b74624db92a70ce1c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_216 d6d9536f4991c7b81b77153864149a0c7f646c852186971424c3a0326e088e85 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_217 d5ac84b228c95af09ec0080422f107197e9abdf61d945ffd12651206e18d7c62 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_218 cd72f83c5559f5d633354ce5bb3c33e0043de058fe2bcdf9f7ad1a42c4db6a16 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_219 de303385bf65c817abc87e1fcdbc677ec88f1ae1f323a726d7ccbd8d64bf83f4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_220 3ac5c14126cfc357cafe955bbab00c8b60bc159fc95f9b66439ec7fe1c797002 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_221 fc2520a0ee92ed35fa379a24ac3b789d37b993adda8f0071eb113f8446d787df -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_222 1c1b86eb72bd8b43a5cf7e011a9b2abf0b8b6186681ea655d907c72d5a50343e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_223 87dbfbf729f36ebc0ca54e712d5a236dbba5b138bb2d5b03d29c4a857db4d762 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_224 d41f3a15ac06b0f5b609834cd0113f2f7b2a786ce656ee9949290dde9e28c535 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_225 c38d7ac66205f79a49b5b2a44a97eec40004828d9ceb22d3deeba31069f37cca -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_226 8b08c46330682f76440cd0da0c0332ae70845fcc7ce0eb7e44e4140a532601b9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_227 8ea9a4278661bb55437a52133e3238fcd4b370e5c1e0cc49d5a69f7e083e952c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_228 ab921066aeed7eb6825ef699e855a046320e174174429990696d1bf7e3a2d330 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_229 98917d4e901a562104e39a8975b26a8c16bfa0bf9be2f896a5eb4c278f3135fe -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_230 926f8d7a4189bcd6d9faeb283a0b854c5187b9688a79b1d80e835ce82575670b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_231 3e06b33d07fd4fa64f338455e39a7e77be8853030b89783642c22a9e8ce3bc2d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_232 b2de35d500f23b0a58d4ed223c8d9f0127eda77b8e5ae61c4c727d8fdfddb528 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_233 339bc81459d6f293c59b17c688d8028ae7080807ac8652324786b56758f378c9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_234 10f13e0bffe45a18172af890ad2b36a712a6a1233dcc34ff62dbabdde5af2bee -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_235 353e57adc0c558ade57de92f48a41c636355ddc6d34335b89d55f6c788bd3a91 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_236 f4ceed7ff82472b50ad06d6a3907e216c067923a94712f8383efacfe9961fb79 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_237 77451755f91a4951fc90f0f82ce7d9a0e05973ef9ef842f9a8d4ad7228371ecd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_238 e64eaf1a67889a228eaaf990489cddbfe0be8103335dd53454f7f3502fa4bceb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_239 86cf474197b3c815d44dee61857f570f126ccc4bb8668c418f2311f266515cea -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_240 54ddbdef4b0485899dbf9282cd1f81326860967adb7ecc6aeaa9b91fc6b0c21e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_241 8a3a0ff562ec5db3f76c860342a0c61631174ab487d752dc02ce7602e880113a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_242 4f25e4642f0008daf099d72fb3a18b7474128a449225cf49fcf1204a6fac0e50 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_243 4fde9477cc7c315ac8ded2039eeb32f9d5c5612b2a231cecc0a4f85fcfc4915d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_244 65b789b535830ded4b65747aed27c86dbf41a111b7d1a88e338d4c3a759301c1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_245 abcbd2a72df1cf3779e98999a7c985df69df285cb20021ccff154dfee3878e2f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_246 0bcd654d0d4039ea518bb37e0f204a7e0628ab5c4a3bd1a4e1597375959b2216 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_247 3a925ffa6fc43039cacb88f3b690f46e1ca9e4060be8a50c258df8416ad27cd1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_248 1c16d3f9f0496be1838acb3281c6eb2c3810b79a23e25c5f83aaabda319824d4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_249 a06f7ed48ae3b2f77e6d6e76f65fe5fd19609c1dd8eb56a0b708b8f457071003 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_250 cf453d395b7373c226ace23ffa983595ba5f6f81db4ee4f32939a3ee63d6b417 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_251 f529718522bf650210b7b9d3be7b87f10fd9940886994029f514b25664bb910a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_252 3816376634921c9892f3164c5b33efa881d74870f4bc4f442fe9a9816732e436 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_253 a80ac08884ef5751b05066ead55a01c62ea22fe9f7c91ec4e7656aa62c32c108 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_254 c67cc898237298f285728024dc09fee5848df49a199e586ba785a05f2809e295 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.name_255 4aad9d820a7f2dc4ca960c9d000afb752bf9b1ec96b5e4c7a1ea9b1aa6f337c7 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.UniformInfo.name fa3507300c35faefe21604b40b968380e2f308ffe63c77329a4badda5fa94394 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.type a84538e8bac70980f4720836b0abbe55ddab9394f38bc24ad5d270536d8b63da -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformInfo.num faeb19aec0a1f642c3c62a778f86744a2363c029aa410416f30fc082a0573d13 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.UniformInfo.#ctor(System.Byte*,Inno.Native.Bgfx.bgfx.UniformType,System.UInt16) c2098de391e9e8488915ad14035dee0867fe883fe1459979a7f9e0d0ed24aade -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-924397dc32616725"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2285) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f25cc6d9b630d68"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2286) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e124e28b1d3c3cc"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_10`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2295) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-71e9ce33c58f80c9"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_100`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2385) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d48a4b9ceb3ac825"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_101`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2386) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b14fe658dcf1bfad"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_102`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2387) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f55726767ffa732"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_103`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2388) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d62ecba1f36dc6f"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_104`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2389) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a436735e09ad491"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_105`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2390) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-968ac9b7e7e8fd24"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_106`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2391) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc722cead30c120d"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_107`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2392) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e97ec24e2178beea"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_108`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2393) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-48e0554e0e083c24"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_109`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2394) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6655ec40587175b"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_11`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2296) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a0de728af85d3fb"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_110`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2395) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01002f3833a15a2f"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_111`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2396) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-97de74653a0daad4"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_112`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2397) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b68e3ea7e78dd506"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_113`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2398) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa7861425b596092"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_114`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2399) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c529ee83327d80f"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_115`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2400) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1365705699e65eaf"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_116`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2401) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-32817015eb1a5ff5"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_117`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2402) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5b478801ea93389f"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_118`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2403) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-79576249b4f00e26"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_119`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2404) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3dfc40be4635af1e"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_12`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2297) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bbf3b455be79b722"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_120`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2405) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-916747c4e8e40e05"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_121`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2406) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d229047fc2787ea2"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_122`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2407) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d6064b91be16f83c"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_123`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2408) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-286d8421f6b86037"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_124`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2409) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c0f4c4c52ad67b3e"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_125`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2410) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6a4a7c63786c0048"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_126`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2411) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ebf76635d497c103"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_127`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2412) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b52fa4d2c52ddab4"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_128`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2413) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8425ac93bbe6b20c"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_129`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2414) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-18b8610a4d813dad"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_13`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2298) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b28cde535f001e79"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_130`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2415) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc89e0de8ecd8c9d"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_131`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2416) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f0ef5736065043f"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_132`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2417) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df655063b6525c6c"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_133`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2418) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8b80d1ee695cbfbe"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_134`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2419) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5a95364f0179115"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_135`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2420) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eabf428b7a765bb8"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_136`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2421) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-12631e83dc627a49"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_137`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2422) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2bdc68bf289db245"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_138`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2423) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-924ff301078df8e5"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_139`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2424) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7cfe150301f69992"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_14`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2299) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-191282f1ecc083c9"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_140`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2425) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5321405bc832b6f6"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_141`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2426) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c33de3e99565e657"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_142`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2427) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe5a24f6d3371030"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_143`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2428) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d11de9d2254f599b"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_144`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2429) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9a33cdded5f11c7a"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_145`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2430) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-57b5762a13c78969"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_146`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2431) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f9ab597e022d5454"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_147`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2432) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8846ce36dcdbb3bc"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_148`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2433) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab8f1a92d73ce9d1"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_149`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2434) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9dd24bb632f2a220"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_15`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2300) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4856ee8330acc17c"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_150`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2435) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d6441b1a460e5730"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_151`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2436) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e17c79f410419359"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_152`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2437) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-670931848efbb261"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_153`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2438) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fa1a5ad6cf943076"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_154`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2439) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc120ba24115b781"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_155`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2440) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13fb4f8799e4bfb5"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_156`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2441) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cdf3810e75d121f7"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_157`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2442) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21b974acae65b160"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_158`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2443) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-154762d8633fee62"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_159`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2444) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e130b0d39b2ca94a"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2301) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc81acec16789bce"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_160`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2445) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-23b0159d9470b2e0"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_161`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2446) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a62ac1fb78fddf3"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_162`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2447) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-764e015d3911db62"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_163`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2448) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-09ae71f60277a07b"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_164`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2449) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-251e3c9249771544"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_165`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2450) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b09442db5b290de"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_166`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2451) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1a7b1f055f12bfe2"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_167`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2452) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de427d5ff94bb8e7"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_168`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2453) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-06fd19ef057a0d70"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_169`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2454) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e9cf251b9386d185"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_17`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2302) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0fe543c099c1b187"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_170`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2455) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-421db5baed9a6e3b"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_171`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2456) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c2628357a6cfe1bd"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_172`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2457) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5451479cc7c0d63e"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_173`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2458) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-67e8f31e4197b81c"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_174`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2459) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fc1346eb4600aad3"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_175`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2460) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0b259e7834421b36"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_176`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2461) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b79c42c8809a79d3"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_177`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2462) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f0389c0091d8322f"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_178`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2463) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c840a47e2edc8f4"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_179`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2464) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ee5b3be3f3cbc897"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_18`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2303) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b7400c62a28343a9"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_180`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2465) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1cacf49f54177f3e"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_181`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2466) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-28e09534f31b641e"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_182`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2467) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8948df464a27d12d"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_183`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2468) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d12ca513344bfb5d"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_184`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2469) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d13e01d901a4be65"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_185`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2470) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b3cf8a644dd81e90"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_186`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2471) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c951f5e97de9d14a"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_187`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2472) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c1798ce1cb221789"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_188`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2473) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2ace9be7b3105bc4"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_189`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2474) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-831080e78bad7f3e"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_19`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2304) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ef77227d0e1d0e5"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_190`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2475) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4577289a15791534"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_191`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2476) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-abba674aa161bbfd"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_192`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2477) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce6142c2eb6a6775"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_193`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2478) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-10521bc85291409c"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_194`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2479) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ef25c2d8fdaf9382"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_195`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2480) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-14edc10a52d98c47"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_196`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2481) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-66219097fdc006e3"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_197`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2482) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d7e335399218775f"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_198`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2483) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-40c9774df7356ec7"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_199`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2484) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d47ffd23e549c198"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2287) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-665b946586b39f9d"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_20`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2305) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-370929c4fb7810c6"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_200`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2485) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ddeac6b666a388f5"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_201`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2486) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a4196110e5d23dae"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_202`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2487) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e34543fae1d5dc1"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_203`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2488) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c7316f97b7ad5df"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_204`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2489) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3982a03538414595"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_205`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2490) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e76d3992bc74dfbe"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_206`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2491) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4ecd7786cab4163"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_207`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2492) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-04e7a43cd469b2f8"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_208`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2493) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c38622e179ca19a"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_209`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2494) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-26d783103cd1369f"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_21`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2306) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6bea043f25546e87"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_210`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2495) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-389aa67f88d8dd99"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_211`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2496) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb677b251a7cabf8"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_212`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2497) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c1f389b95035487"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_213`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2498) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-14765b6921fe6536"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_214`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2499) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2038b4dfda4c5809"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_215`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2500) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39b24c0fd6472429"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_216`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2501) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-06ed1345e65a3dc0"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_217`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2502) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c92372bc11995d7"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_218`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2503) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfb150326f7dadee"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_219`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2504) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-805f3ee17cda0b55"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_22`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2307) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fa78717e7ab4ec78"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_220`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2505) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60d0bfa68663684e"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_221`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2506) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f8287358a9be7c6"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_222`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2507) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8302c92a50402da7"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_223`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2508) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-34623ba3eabe806b"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_224`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2509) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-17958e2ba0e287eb"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_225`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2510) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-627ca902dfb93220"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_226`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2511) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c4568c54b7e0347"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_227`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2512) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eadf9272497e1f0e"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_228`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2513) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03a08eac539c44ef"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_229`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2514) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6e2c789564d4ea6"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_23`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2308) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd0ecfad5426851f"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_230`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2515) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-70cfbd168e6b5774"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_231`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2516) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60d8dc93f66ed20d"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_232`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2517) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1842482ce65c438e"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_233`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2518) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-176975e870fb4b8b"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_234`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2519) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-efd6f166576a4a0d"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_235`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2520) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1c6601e4e329532"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_236`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2521) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-274005ad7d7bb68a"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_237`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2522) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-749e463fbc4962a2"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_238`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2523) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-750838bd3e4c3520"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_239`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2524) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-924204096a640fa6"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_24`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2309) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b100c98fff752e8"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_240`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2525) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-49b7c66e3158f0a9"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_241`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2526) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ff5a8ea15c424b7"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_242`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2527) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1a83cc29d9fb4b9e"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_243`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2528) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-776dc1bdfeda7e03"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_244`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2529) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9a44b55441f65812"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_245`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2530) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-68c216a651e4d6a3"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_246`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2531) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b160c5df06d3f95f"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_247`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2532) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7bd870bcdb7cbcbc"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_248`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2533) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3e01fb50edb22b53"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_249`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2534) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-48110fb7b9ff79a6"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_25`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2310) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d11a35f1566f8ce7"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_250`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2535) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7cdc73f82ac9a41d"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_251`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2536) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f323a2a1f7b30c2c"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_252`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2537) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64c124193fd121ec"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_253`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2538) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54b1be81617d8683"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_254`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2539) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae4c56c3be1c911c"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_255`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2540) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89181e7885b1a297"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_26`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2311) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-72a8a2fb9c5d40f1"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_27`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2312) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c0b053b71c58b8ee"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_28`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2313) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e9a05df95e4905eb"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_29`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2314) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7e2a886848c880c"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2288) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-88c157e85a792b14"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_30`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2315) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e09ab48c43270cd"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_31`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2316) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e956e6f74d649bf8"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_32`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2317) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e658966f7bce7ab"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_33`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2318) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d2803eece64484f"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_34`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2319) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-23e1313c2dc6c344"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_35`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2320) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1a1d3b92d8175a65"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_36`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2321) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c3fecaffea440ca4"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_37`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2322) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b0b2ac03240a862"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_38`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2323) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c025a5866c5cf39a"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_39`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2324) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-442045a06a1c5168"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2289) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e4c1034b948ee7b1"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_40`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2325) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-93691ed9f2bda1e4"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_41`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2326) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-beb1a5ea49888541"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_42`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2327) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dbd364b0534c7162"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_43`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2328) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a536f3b6e00c735"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_44`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2329) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e4226f400a052321"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_45`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2330) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-837cd717dbf18bda"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_46`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2331) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-86b981c1d469a2dc"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_47`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2332) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-801d8e34bdcf0a19"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_48`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2333) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eaec05b6af440dc2"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_49`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2334) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a690ac8efda18329"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2290) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-15077067c4d8cafd"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_50`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2335) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d812f4888230cea"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_51`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2336) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-23fa3a781f8405f9"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_52`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2337) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-638771376a10055a"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_53`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2338) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a4cd4d52a575cf77"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_54`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2339) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fff8b7543e76e973"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_55`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2340) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4bd120e357c06694"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_56`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2341) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2f9ac0706470ed9"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_57`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2342) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2c41bf5e00f54b72"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_58`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2343) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c8ef9f6d7dbdae2"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_59`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2344) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1ed30f2211df0f0"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2291) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b17d66612a7332c1"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_60`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2345) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-700b0173d28646e6"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_61`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2346) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e5d3180a5740651c"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_62`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2347) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-46f1b2467174d8c4"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_63`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2348) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0429a811ffa549f5"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_64`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2349) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d6d914c748bffbd6"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_65`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2350) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-00d3ce95de66016f"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_66`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2351) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-449891a9a9edf068"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_67`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2352) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-abe382e7d968e2c1"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_68`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2353) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e54a5dfa7f62d8c6"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_69`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2354) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b602fb7e2b2832d2"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_7`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2292) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b88bbd55ae8c8b3a"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_70`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2355) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-116ada5360e3c8e3"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_71`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2356) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e720ced7c4729f8d"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_72`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2357) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6edf97fcb914e450"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_73`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2358) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7a0d469354969801"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_74`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2359) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-132e3121ef8981d2"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_75`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2360) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c9e689c0ca4d369"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_76`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2361) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13a656c36ec0be81"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_77`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2362) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ade1e8453ef55d4"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_78`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2363) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4072fb356bd2ca94"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_79`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2364) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5126091f96ed9b3"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2293) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b46cf3e6fc65cdf5"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_80`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2365) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c36852952422c352"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_81`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2366) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-08fe1e6a32d300d3"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_82`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2367) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad50abe6ffc65304"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_83`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2368) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea30623a4397d8a5"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_84`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2369) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6fbadd2ac42ad83f"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_85`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2370) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3021e89341a58703"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_86`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2371) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0640b6d965f84ac3"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_87`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2372) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-499a8cb628e2a120"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_88`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2373) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a4fbeaf72c27d16d"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_89`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2374) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f95b38f8ba71a685"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_9`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2294) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec526fc67a658960"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_90`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2375) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b8c48e6290fd991"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_91`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2376) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5806071b0c09fec9"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_92`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2377) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9f7778fc6aaa8c6"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_93`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2378) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c74d384f55e98343"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_94`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2379) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3cc13464f1865d79"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_95`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2380) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec786f461233740d"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_96`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2381) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd80b51dad2afe46"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_97`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2382) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b9b472d28cb30b5"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_98`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2383) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2377eecf78e6b6a"></a>[`public byte Inno.Native.Bgfx.bgfx.UniformInfo.name_99`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2384) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7549e017e6a1d7db"></a>[`public ushort Inno.Native.Bgfx.bgfx.UniformInfo.num`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2544) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-46b62d3bc8ef6d2e"></a>[`public Inno.Native.Bgfx.bgfx.UniformType Inno.Native.Bgfx.bgfx.UniformInfo.type`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2543) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f7b62c17cfc1fffc"></a>[`public Inno.Native.Bgfx.bgfx.UniformInfo.UniformInfo(byte* name = null, Inno.Native.Bgfx.bgfx.UniformType type = Inno.Native.Bgfx.bgfx.UniformType.Sampler, ushort num = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2545) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4b6d99aed9056d28"></a>[`public System.Span<byte> Inno.Native.Bgfx.bgfx.UniformInfo.name { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2541) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-25cd14092e391ec3"></a>[`public struct Inno.Native.Bgfx.bgfx.UniformInfo`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2283) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.UniformType</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.UniformType 2aad6518d90257f7ba66655e2c2796d3aec44399b8e59d93cd783eccb23abbb5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformType.Sampler 2ba9a7abced5b6c21c6d49f4367f02d6404316959cb94910ae798732bf8054b5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformType.End 7fdddc2f063427c1210b9a2b50ba69747bc892ad939f72f2353c4fc200829cd0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformType.Vec4 c57e95ae6e312249865eda8a6cf85d6d13f86ee339f1b18665949326e81b105e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformType.Mat3 a0d0c5bf74e7c0dd6f025986f460576bf74d7fd3f8f37ded984b550b9be042e1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformType.Mat4 3d740d34720daf448d1d8d73bca7cccd4dc7d71e28205b6b6d75b799ba21195d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.UniformType.Count f3afc1ced5e0ecfd74248d65b20878934edb1acdb1a1c663660fe85b8b3d35c5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-951586710cbf6546"></a>[`Inno.Native.Bgfx.bgfx.UniformType.Count = 5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L241) | 当前常量或枚举值：`5`。 | — |
| <a id="api-cc13d102899845a9"></a>[`Inno.Native.Bgfx.bgfx.UniformType.End = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L237) | 当前常量或枚举值：`1`。 | — |
| <a id="api-959dbc3400f1e207"></a>[`Inno.Native.Bgfx.bgfx.UniformType.Mat3 = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L239) | 当前常量或枚举值：`3`。 | — |
| <a id="api-fc2f49de1672cbca"></a>[`Inno.Native.Bgfx.bgfx.UniformType.Mat4 = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L240) | 当前常量或枚举值：`4`。 | — |
| <a id="api-451b5b54562fb8b3"></a>[`Inno.Native.Bgfx.bgfx.UniformType.Sampler = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L236) | 当前常量或枚举值：`0`。 | — |
| <a id="api-c19759fb08ca8703"></a>[`Inno.Native.Bgfx.bgfx.UniformType.Vec4 = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L238) | 当前常量或枚举值：`2`。 | — |
| <a id="api-06eccfb88f8cb8b1"></a>[`public enum Inno.Native.Bgfx.bgfx.UniformType : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L234) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.VertexBufferHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.VertexBufferHandle 19efd50833bc2ad5484485e82a1a4a9a59eca4aa393502c58c7c4704986f84ba -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexBufferHandle.idx 37bd56093dfd2f25596b2a108365fdce4cfe2c862db2c34cd45d4dc37f60e5eb -->
<!-- api: P:Inno.Native.Bgfx.bgfx.VertexBufferHandle.Valid 782af0cacec362a2dc4ffa3d64a82da9f4f46cb0f29d9e9c71e1ecf06075577e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.VertexBufferHandle.#ctor(System.UInt16) 1ce0599897ecd4ba59002e7d5be1f4e77b01d826e5082a6befc6b16d7d404ed7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-be865c84d8b881c7"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexBufferHandle.idx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2033) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e11094bd2440488c"></a>[`public Inno.Native.Bgfx.bgfx.VertexBufferHandle.VertexBufferHandle(ushort idx = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2036) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-af30d8c641569c23"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.VertexBufferHandle.Valid { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2034) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-21c6cde2dd80ba2a"></a>[`public struct Inno.Native.Bgfx.bgfx.VertexBufferHandle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2031) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.VertexLayout</code> · 42 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.VertexLayout d3a5a0ae9895c8372f2a0dea3459097485b06dc002a299942d07d28618be1079 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.hash 018e90fd8889a7d3ab0eec921d262a9d69415ae5273f8ad8414a6b1cc94a3d25 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.stride 552a65b00ebca9d0e3f048c5d0aadc8d410af75ba6fae97a6569dfd5692ef788 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_0 5e2c26fe8467786142433269b2d04bdb0f9f0f60075a66351e3541f697f5fdbe -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_1 26db0f2ed5273cd8f5786f917d33f81530104aed7f67bac50f9b44b90f3742ba -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_2 1b68ce7156bd4cd7bd341b055c08ec0f553a1642304bf403aa8e8375bf82735b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_3 b88add19e8829cb73bd0e6c2631f592a0af51e4e96d7545ea87f8b3cd07ac4bd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_4 77060803fbef9de89606e97a0ccb8df3a558dc74988b76a601666d264ff1de08 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_5 e31b977b9bf4329218229bf628a4753a9a78126b31ad25558bda86659b641fa4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_6 5e57b4612e9f0d0fa1d5b98e9e6c6046ef2d2cdb979190a563880dfcaa88d0c8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_7 2cbc7832c522957e70bac3a04ffacb3fca9fd0738f1f3b1d15a4a62beeda7541 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_8 eb126d6baa258fb513ed4c4dd9806b674f4b2c38ad929ccbd9b0b037481ceff8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_9 d32d0dd99609ae34a594c0d3e5d52ddab6e92610383b69cb19c1e778fe7e1964 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_10 3f3a9723faed09174f0d5f766a338ab29291c5a49047a7979b5f1b077d080d75 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_11 4f83f53978015705bf9885223756a438be764082843471f04ee72abb18ab99fe -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_12 5422cd276739415bac645ec6d4d7e673ca1b7823b6baaf1952f14e01318a62c9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_13 62d2051160adf8b57ee69d349d7dfbfdb03a3fc0dbd26ad14b41a1fb7855ddf4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_14 f84f18f6cf7a8650dc2a7707196a5672205105ef98b9a0881bfea43dd177e01d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_15 08b4015d5bcb0f66d57f78db329897a5a7d1a2ae6df3dcddfbb433603d9b6fe1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_16 04491024590a1ff3e46b9821a4a22e74344ddaadf80f3e34385f01b8b5b9093a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.offset_17 6f933c1f416d1114b533b7e3863b77daf2d89423a6980172a5f1d74ae2d79597 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.VertexLayout.offset caf452d89b3155f2e2422864ee9d60bd633c2e5dd825044c579bc9ed43f0b0c7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_0 aaa2caf470d8046828b552376351efd44fe996b97beaf0ec33d882cb7c0e35cb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_1 f13da704a9b2681991f603cfb9d5449289e44ac2684b799cc0888cee27fe60c9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_2 baae2f5e3567c3876e1dc0e00394dae49a9203d95adff3621f92a160da7bc1fa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_3 f2da4b33d29a86f1c0bd260c95385861383b6ae69de1fbf9ee55b8f74a7f2694 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_4 bfa316e852c72d1de2c41fee27abe28352b4db727ed3d2b514731dd6c33aba72 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_5 6eabbc979cbec8347c9e16ae27b5d1035c1cc78da0d6156f0e248763b694161d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_6 13c60d28aafb43f27f9ba719560fea0bc1930735216d1e9356fbbf4f9de6c282 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_7 bcf113568c9470bcdee1a877e79af493fab0fddbc45b22ea5eb19575cdcb58b2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_8 1461a0e9fe29d50555803e98520ce8ca28d9a596beec7beae3ebc1c5ed949ebc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_9 0c0e81f988a306e60ab8c5770ef90583ff91838f39ad02ab14055d04ef27580e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_10 44bce9d245faca4b392858d44452613fa63220348f682a039df7481b4f0fc707 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_11 3b0b8b19d7dd6087934ca06c12fe97a4c1d37130bbf7f1a241536a2c6229bd2d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_12 b42e196225a34074eb76b3a863233ed55efb4c47c747633cf0c7dd19af2f4f04 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_13 70b73c002ab53283d263a893ea7aee2ee7fbd9cecf3ba727f0cfc68734a8e058 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_14 85f394fa7f52438c998ee003f48807e72d3bf8cae873b03427e7fe0e60a7bed7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_15 a22a7065cceb7aacd9cb721fcddf7292248cab315828239eb357d0a554ee21d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_16 d9ce4b3ff3e8596e88081a5e568827e39b4d610924624425c0db7cbd03cc36e5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayout.attributes_17 a45afdd2261feb90afc7f04b5aa4e1d0d4af5c03b01586e564511e8d2c39364e -->
<!-- api: P:Inno.Native.Bgfx.bgfx.VertexLayout.attributes e8efc8b872adc14898cf45ce701d6905894d3dd44c60834f21b06d7e8ef2a163 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.VertexLayout.#ctor(System.UInt32,System.UInt16,System.UInt16*,System.UInt16*) 678ef9531cf3e54840a8f70bf87966c41932273f847177c69024df10ceae6858 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4efa1adaa406ab61"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L725) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc0749544b2cad24"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L726) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5924087a0163ec4a"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_10`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L735) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a05664c4f71218fc"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_11`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L736) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb5f3778e1ab8d4e"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_12`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L737) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd40c260d215c2be"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_13`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L738) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a97c59e5b4d2000d"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_14`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L739) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be2d8ccff04d0683"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_15`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L740) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a5e915abdd567c51"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L741) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03c1fc6c770cae2c"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_17`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L742) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec0f296cdf4fbbb7"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L727) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c192b9a5bd48dd9"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L728) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f96b26bac25144c8"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L729) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b2d8c37f6d42b7d3"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L730) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69ec35412b191ab9"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L731) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f1301a4913a0ce0"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_7`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L732) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d8a823073b15ec2f"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L733) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-177d113c53bb07da"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.attributes_9`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L734) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c582349572c5d085"></a>[`public uint Inno.Native.Bgfx.bgfx.VertexLayout.hash`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L703) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb8dc4b4d4e178c8"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L705) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2ccf65db85262389"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L706) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-332f711c0637e27b"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_10`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L715) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9ef6cd2e3ba07faf"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_11`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L716) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fc04c3525c6d70a5"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_12`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L717) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e214f138f102bd17"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_13`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L718) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-daea74a9464622ca"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_14`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L719) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-001a820054f9c2e4"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_15`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L720) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-158be47070efa035"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L721) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-25c538e85c726197"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_17`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L722) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d8ff3e5af842965"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L707) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9fbfd950e49e282a"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L708) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0dd54e1f7d7c45fe"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L709) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c9fcf96fa76bda4a"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L710) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f2b79275ebf43d4"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L711) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7696faf103f5da27"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_7`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L712) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b217d22629d263db"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L713) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-47b77eb53e9f3dc2"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.offset_9`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L714) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89b688b0dd29a945"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayout.stride`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L704) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e23a577373f8b821"></a>[`public Inno.Native.Bgfx.bgfx.VertexLayout.VertexLayout(uint hash = 0, ushort stride = 0, ushort* offset = null, ushort* attributes = null)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L745) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-002ee6ec49c77521"></a>[`public System.Span<ushort> Inno.Native.Bgfx.bgfx.VertexLayout.attributes { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L743) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-394260690bced916"></a>[`public System.Span<ushort> Inno.Native.Bgfx.bgfx.VertexLayout.offset { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L723) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aea5695dd413fae5"></a>[`public struct Inno.Native.Bgfx.bgfx.VertexLayout`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L701) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.VertexLayoutHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.VertexLayoutHandle 9e0119ef1ceb1c556fe6acb0b742c22872918355e8223a04a1adcfc8ef46c1e9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.VertexLayoutHandle.idx d83a913d08177c910577673303945de1948068c77fe5f9f3e77f4747b06bc748 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.VertexLayoutHandle.Valid 9a9298e63715c41cbfe2f7e0864af70bb5bbe45a08849a5c4a35596b91607caa -->
<!-- api: M:Inno.Native.Bgfx.bgfx.VertexLayoutHandle.#ctor(System.UInt16) 26ef4484235488419b44461f11db66619336d3d889479acadb9f12639b1d8f11 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d14c26c111ac26ec"></a>[`public ushort Inno.Native.Bgfx.bgfx.VertexLayoutHandle.idx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2015) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f0cbfe7770f30dcf"></a>[`public Inno.Native.Bgfx.bgfx.VertexLayoutHandle.VertexLayoutHandle(ushort idx = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2018) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-30b77e672a6aca9f"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.VertexLayoutHandle.Valid { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2016) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-20d058becb3276a5"></a>[`public struct Inno.Native.Bgfx.bgfx.VertexLayoutHandle`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2013) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.ViewMode</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.ViewMode fa1f52b20c56bf207493648d5fa616c22ec1478bdc7b05cc6089d869d7650f7b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewMode.Default e3b372a50ce840e1cf0d20abd44884c51954898dd51302cee393218e14753f3f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewMode.Sequential 7718a7b104aeaec0ee47b8abe4fda7fe4b6349b7590cf0c1f62ec13e20cee936 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewMode.DepthAscending 2ff3c1e7eb758216eb927edf1d08b6f8c98dda790b8bc9dd84fc7963c0034268 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewMode.DepthDescending d9b4f29254a27c5ed18e055bd2e3fb7f31b02f46ee856277fe705749808417d3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewMode.Count 826573109cf2c7ed45f5908a7338f22a2ff874bb7e5e1f22945cc0e0668c6b1a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-666bfad0df5c5379"></a>[`Inno.Native.Bgfx.bgfx.ViewMode.Count = 4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L356) | 当前常量或枚举值：`4`。 | — |
| <a id="api-3f8ecd9b2b95bf17"></a>[`Inno.Native.Bgfx.bgfx.ViewMode.Default = 0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L352) | 当前常量或枚举值：`0`。 | — |
| <a id="api-a56ff3552d2c5157"></a>[`Inno.Native.Bgfx.bgfx.ViewMode.DepthAscending = 2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L354) | 当前常量或枚举值：`2`。 | — |
| <a id="api-5a04133c20bd372e"></a>[`Inno.Native.Bgfx.bgfx.ViewMode.DepthDescending = 3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L355) | 当前常量或枚举值：`3`。 | — |
| <a id="api-b50843a78e5c7772"></a>[`Inno.Native.Bgfx.bgfx.ViewMode.Sequential = 1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L353) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d18876d312dd3727"></a>[`public enum Inno.Native.Bgfx.bgfx.ViewMode : int`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L350) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.ViewStats</code> · 265 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.ViewStats 2ab5a0efe1be98ba61ec02e27e5128a3479c2bb6ea80bfb6cb30271124f7cb9b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_0 d5b92aeb91ac81e65bc306e743b9c5386c1320d4323d8ccbdc83e2c03d93e420 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_1 31e43a888454b62ba60d8fc70535b56ae14e79a4458ed8bcac6537c6c22d2d0c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_2 ac1208cded68b4a2929c9c488aeabe98fa491f231edd2e719424555e8f294a75 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_3 eb5d3493a9508cac37cfba206b52a8298ba8cfb606c0679c75f516ff6153d146 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_4 3fe8c9cfda7a854cb60e8054740fa6883ea288c05a21a50b44821c3e66602849 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_5 240aeabb3c3714acefaa50203c1edec7afa558b27b342ad4318a47aa1ada15df -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_6 178b99e9c138e9e14881bcbe2b0e017f2ccbcb53ccb58026cc9f6f3968524856 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_7 14d92b813cc897a34d8fd94c321e16d72afb0029d311a2119769cfd521b8da08 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_8 22882ca0083987aa8673c74d97ba3d05aa241cc578d81f4affbf815837db9b82 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_9 29e91c9c884f98d6fd2e302c34d869a674ac97b0a89b816f87f56f8ee97be7fa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_10 770a7f7e4a15182cd627d4edb500799f33cb9448c21442987e636bf5f8148b58 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_11 7e50860e551032b137749175f6855f720a9ecc9c63e8ca17a6bb23f7e703f996 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_12 a95066ec672333f188ed78ee98e1faed7abc0817cd9b84fe26793c328db2f6e1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_13 e4f76b5e4f7ac54ca4579be08188aa5041e3e0554b1cac67306639a1cd900959 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_14 f01c6a0865ae5d5b6172455c47c5c6fecb71589dd01663e26094cab15551d5d2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_15 bae6e160e2303cfb0aa02d46935a86341279260e4486a93dc593b9d06ece91e9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_16 a57fa22e4ef1c0a8eead452cd8cd455254e4e7a49a20720d080980352b5222c6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_17 b5f69339200cb4f3112b889fffd66a3332dbd00b3c10346fdc0efb4fc06603fe -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_18 2ad6bb0b1d8845274d28685b4d9b37d4dce15a1538f9840be521743a1e1987bd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_19 48ab1e7040600e7a1ef0d8e713c8ca754e332333384e7a8a0f5c91081ca08015 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_20 149197cef6fe5000f0a31b126c8f3927907eb84ae12914c6fc76b6cd96533fcd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_21 2bc9b7ce1489c603fcd72aa213a77ace314e7a223332a0fe3b191956f62aac94 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_22 fd72df59d14d702e1d1654f2b513b8ee0839b62317f69ad3d95035d92756a55a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_23 1d020b4b6017226bb8c3be65c29456690a1316b241a0859798238e8c8db71b59 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_24 7ff0cc3afb9887092aa2988871cbf1642c2fb71015d59dd65ea2c66b890aa11a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_25 d60ddf4f16c0d584bbcea39fbd5b2b6147adfa5bb287324e156c6f7f52b380b8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_26 3daf0073b7b02826046eb6471c63b0117b94c40aa405583466e27b944eb3f048 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_27 38675ea2539910eef72d3faf16348dec39c61cba1ce11b1008bc6639e633548e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_28 1e3fb3b456597193038577e25bcba2b953d487f818e5329a118225ce18cf661b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_29 3c2160104228b44be9027c969fac844b37e92784dff55187d523ecaf3405c8fb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_30 96d7b2b5fee5260963ce60264a57bb6ff9a2cdfc084d862c51f345fe2a2b8ff4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_31 d773ce75162a3237eab2360f77616ad44c75c6617c24b9365622dc466fdca7b3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_32 f4df6685c5149bd243fbe25cdb576dc857dfde3512856346e6fed17ab0f413f6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_33 fdc0f4b060b0aa51c9c2af7c920780d7915ed8c7e9941540d7614848fac2ef3b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_34 c1da06a171c00d3ed8db4577fcac9970ea4e365823922e2df97c5a178e6d1725 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_35 2172e9be1268fab1ecc0a46cc51d97cd0a9176fb5058769338a17df0d0321dd3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_36 f1a0cf845d62ce0440cb060097651bc64cee84c4b5e1ba7021cf0a395d312222 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_37 1f1409af297b20c20a864d97781d9d3598cd2b1a155cd0a9178cc4b25de20342 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_38 59d9e447570a1770c1f87c44167f1c3f5c70296f6dc3ac93ab20fc9039346d2c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_39 9a12a1672bece99df16203fca909f95df9b35865ee757cd181831915a1e82c25 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_40 a5d7fbda9470a963e99f02d9e53a696186cd8b7498a9e94b157e49001781b7b9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_41 11b2bd4a45d1bc8b6ebdd5f0b83967d3bf50403ac30ee734ef9c524409e76ff7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_42 b99ab15947a7e99cde7701031380b5d73ac2b26ed1b173199e1a93106b53b268 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_43 1cdcc05460decc4a3319b7b35bf3efb29df4049184ec2003100ba82b4da0dbb6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_44 aaa5e0bd47880f3bcf96f718eaa3612c5ec8b96d892de8fc79b0e437fabe3de8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_45 85592e95dc0b3541dea2257df5d1b61751988dc879378c859d49e061fad37cf1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_46 3288542b432a880eecbfeb355d845cb868f7359532a34f7215a2720ea609353c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_47 508d32ee50e9f4b63bbb6239797c0487e11747f529db27c9137be617e57783c7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_48 8f4a45dacd74f990b51a61619d79f14054d0c2d8fe5a78d92b9e6f15122edd76 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_49 76236cef6ba644d6341ce00630ddeef5f1b94f8d05bf12d5e82011b6d16f7d1f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_50 662bbb7aa9741cd0b8c1358424fb6e795ec684c9eac104fdb5e2d8c62dac0b2a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_51 b718feba7ab7dabb2db109482605b564dc2f9e59b2bae07be92700eb9daa3b8b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_52 19bd3c87b3231da18ed17168e8fec94e38de83053a270e1b4f89a8570158a832 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_53 d8fdadedb5cf3b2d5c92ad95bd51495a8372c67d2bb2e6f80e13aecda9317d65 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_54 8e206412164874bb87226575cf8016255801cc9c35ac09477fa7b0892ba79fd8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_55 a51a61e7350353bc0247524b97da47601b9884b39dbdf9cf4fe097840523f025 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_56 e1bdd0c21455767b0830133bef1fbe67a8d972078f687e24d29a05d675227b48 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_57 0f7894a4be4d172f5ca88ca77f102321d3553ee8790ed8758ad51399f32d9193 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_58 8d091414691bcc62cfc1b04b4fc503ae6e7ce8a9292ccefc1facbcccba583727 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_59 2b3e0a10c21e4ed19a2af212d08cddb136d60e35e4c7acada3df944da3f27fb2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_60 80e11cbd28aa90733f55c5e112d10ea1d2915e0a2af3c33a45c3eb22c1dbacf7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_61 977ffcc11f1aa8ca4d1adf66cf629161e0e58df7aae8c4ceba2b5e6dfb3cd4d1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_62 0b62bc0587ef6ef6e6ed61d8bfcf12b2db30ee7076a71fefd36554147cb8c568 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_63 14254eecbc7db9be1cc04ec95c179bc90d006158823770b046c0d3c356dad002 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_64 eeac27c3b5eec0e6a0efb5cffd25ca1127850bb149945960d6123da1b7373be2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_65 619ec4b38d5462bfd520cd2709c97d56e3d0e101b3fd75e06f79c69876dcd6bc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_66 7b3279a291dfb052b0c62ece7bbb29ab7b2edab8d8c916c440fc8668fabbb40a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_67 e7f49978b8c561a1d727cf445dfdc85249a5c316f9e7b33f2c1e0990dabb0464 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_68 0ae29daefa9d1cdaf5b97021fe304936e73efcc68db44205bedbe11865595a5f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_69 87341c8cf5617658ab54bb9864f7492d2544e2ddf34172dd8f094653d779a5dc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_70 2c7d970f80d46e0ae95ab819e6e4367d03bf055812694683eb7517e335bc6e6b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_71 4a2ad6d92711cc66e0f1b70742ac34d1ee88fd04313631de6aad27dbab65f966 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_72 80b5297f6b1433124e31d63f6ee13c35b96473a79070f83e44fba365ffb653f5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_73 91adee678018b42bf590e1ce5f3886e89dba67268ed8d719af3c6e13bac2e25a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_74 ed9cf5324510fd31229d33be409daf15ffc0be7f8ede74ae9f7469dc1b76d96e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_75 9398c52cc2bba935648a12b740dea672f811535608b2525b6d389b115533beab -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_76 e26b56efa598a7007cfabc5bf4b48a92d2130ae33afba859a4c84d804320e40b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_77 51a7ca1c6044688078ce703639800c93ac5a57c59c66519fda0ec632823a4a77 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_78 3ec9a604f6757a9f01486f52525cdb0061c8db0006c4fe151c82b7f7770ea0ec -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_79 dccea7d06ce2dfd63b67f0fb08c75cab47d94aff8612b6e978252bed91b06688 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_80 17fe14d1320d870fb7634fec29ab698b5d76278c0c632e4be4955fcb44daef76 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_81 3eec78393a33f58109f4396e27382aaffafbf561e0903fffad852c44e2c48202 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_82 712a53c3469d84a78d58fd1c0281ba5cd7cd1ab43c94192756b4db4f877f23bf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_83 e90c10f921209cf42b21945e72b221fa4dcb15fe8543761c478ce574eaa4ea3b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_84 2d4aededb62703d640e41f95d6148b79a47d72c308d9415d942aac676c3fb737 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_85 33f63d66b104b909e7911ab76aa63b51fe4bbbfea954bc2db67c8431c8275254 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_86 cb18dd3017d94fc5f6c3913d3b0bd239c091c41c698ca3af74669a583c482621 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_87 3160dc8b29a604e8817fd3c491ce805db171ad8f811168a62a10e2012475a7fe -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_88 2379f34fb63633c5ba8b244e73d43bc63698f9034fc1c90b4f2d2581687d35d1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_89 3e73468e5e52536a4a9831290bacb8bb123435994082009446867db2add11c38 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_90 517a76ea7f32ebbc29235aed4b20cd107a02806acd0f00535425070bba22036e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_91 66abe194de7db937dff0bc89de974b56b28a2f5a8d49b5f7eab05d8a4dc0e53d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_92 2cacbeb635f334af4c151db18002de5169a86a7af20b6f35c3e36747dbd8c9ac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_93 3f26923833ff7adf2fff684ee5ff35e25d5785657b4a78a39b35ef71f04f6269 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_94 b6a9fc9bb150e46207095e5292c6322428653e5be34541eef50e3b1b2ef3097b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_95 f158e750fecce275bf7360664e80da6c15dfd4aa67188d6f57a128c236f1d45a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_96 83d167e3cfe3c81b8b6eaaecf281f7c12ed309284cc4d8255ca61b4db9600f68 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_97 32198dff5859fcad9440cfa876f4d6da52ca1c7f679a8ec8c943a25d2896c27e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_98 0ffc1fc5fa94ba96fa6fa3bbcd21b53ba0095ea59c3e925b6f9824b8c018fabd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_99 9fe0bb5c55818f7888b2292f4cf2d71863afff192f489ff62446c156599777cf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_100 2aa71b9b7c43a783230ca2005e1be2c2feb3b835e9992bae4f04131c6ef45c0b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_101 e9c11d925d5751ab2e46d657ad67614710ba8058d2c95ca21c738ad0e6787944 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_102 65d7297a5e4aebb6098f09cdd5e1c6e63a0f329d6f1a48a89d1858fe21b2e32a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_103 9faa448b3a565169fd7efc31e0f168e2294f3481a2c19d991e9052cf93bfa7e2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_104 48e7b0cda389eb40765e235afd4303bcab0bc8237364e0d7159209de14d879a9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_105 11518cdc4b23543a7caadf174ff631689bebbfabcf825dafd62ed595f922adf6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_106 f3ade338447b9ae309bc4a98051c55a9df8def944a1450ab0fa345d8ba0c3a01 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_107 6694c176bb8702ada54a9651428b48899d893102f8feaf11619102ba6c62ab2d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_108 9cfeb5dc4b330e81c34029e26236d2f8b0ffdc6c69f2c7b9df2ae6b999e68141 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_109 9d33db044c78584df71caf80cdeb1470d5bede90136c010e9b8ca269f5e2bf99 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_110 6b1776bab64e12f04eb2acbaa5b7e39687b0ea9d4176f38a4128397bc3e83bf5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_111 c7378ad183feb8c2a7a4cee61e6959d41e7052523a4cb1940cc5c66261598761 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_112 7bbfa1d9693748343fab2c3a0490ce4cf65895861fc92b3ad83fcacd6fbe516a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_113 b592127df5ad1dbb3b610dc3428c9046bb4e2f2ee657f22c027bba396237cbd9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_114 8923fe42d048b785e5ef4899a103db6d2fe922c096167da7fbc47eff934c79bd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_115 1b0a7f8a4e607f71d1ef0706f392aa405ae215837d588d260f7275872078c6db -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_116 bc7a1b2db5814a193b88747356afc4b17192ee4ae364775e9d6a7eaf322101c1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_117 6e0b6d2e81d7fc69003d3920d3f9331b17df37167e2a776e4af5907872ec3ff7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_118 9f58abdf8d5457a3037c45630a10cb38c40e82382ada66c41dca14f4cba654ff -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_119 5208d6d15675c28b7a5eafad78fa0cfd422b568c32c6897b265e35787937730f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_120 8cef3e387e971289e6dd750d5e44623febf9e320ca04eb484051679fbc862036 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_121 fb27de248a1c8b59216e2108c5745202b57c89dc8c724ea60ba45e1fd13ae6a6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_122 a9ac26f968c0e30e1c6cd132da28ebbb9bc4494b4a0b622aecd6a62e926def61 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_123 f988458607c25db43ade5deddaa72c72ae6d4e4b88a7a8625baa8514d96ef767 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_124 44563e92f8670ea1cc3cffe045e4de6ac54df99346f8385163aa6859bf1140d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_125 c9dbe23a456d915a43f73788e19a884fe03a36db19b55cffe26981c81ce089b5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_126 8223aa2ab598d27bf324616fe955eab05262c7e2c4612216305a95aef5681d96 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_127 247b7d20e32813abf2069c9355ffceac7633de2b012adc595dfc5ff7840a592c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_128 b91c0d479ad8337c9957e3cf6175abb6e53cbfcd0fee21cab0723bba0d3723c2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_129 e14c7e9a56017fae7798bbfeedad976ae30b489b5f293f02ccd8a85b6786b521 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_130 51c5e035a5e723011df3265bbc90f37acfb08a765addea7e57df1f94757d46ed -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_131 0f675438ae2c00f0db3eff85e40be2e92d69e51316fc41efead3ff5cb342b526 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_132 8f53e70170e72002d2ac927c8ce79dd2ec57f2eadf93e4f5d7a8c4d3cd8bfcd7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_133 379e9de3c5b66188f6b656866933576241ec85e4f011c4242590f06f712607e9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_134 79cc3bf6dc2b8645437ec7978eeeb4cd8b554ec13e1fc3435027f069e7129df8 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_135 db3e83c8bce05f4c42b17f4ea8d9bb8f205cba577255a6a35ca4980f0e1213b9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_136 d7186d6fa44339e6b6504c9fc6a1eabaea9d35cfc19e0ace812e0b38ffa6e762 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_137 681fcf0519b6900d4e6e8ae02f31249dcba14eee1b5f33deccf518837c243154 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_138 914100ca567e1edcb5d5b09e9ed00cf0a1fd0b95abb77c56c1fcddfd2c7a22db -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_139 cb3e11fe36737aae7d0ea1a9649b0c497e21ad9aa16e3f50b1012b513da2d22f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_140 c73c22156babaa376b80029ba1b1b9db96916d3b6ee859969970b52d18f6011e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_141 77f75d3f32d73941c88abfb4fa20fa136edcca2c5377eb2d7b4825a23495d385 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_142 2c757523b38706fbb7069ee1285008a6362a5f245f74e0afcf3f292298842f7e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_143 7374bdf244d96c72ee3a2c8493aa50fc107132363e0de55516bde9a9012c34eb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_144 ca4299b4329e7131898c95859c92f39dd311804a36465340f33e5aeb1776d25f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_145 31499c80cc512484220fd35930f8188f8690da2e7ae7cfb3269da952de75acb5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_146 46182c81dbe4912efc56bd0ad24203fad466c85cfbdcd7d59b9b6ee0dfb4094a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_147 3bc4476afd8802359bfc8cec302b78e30003032e417d2bf84bf71cea53dc75ac -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_148 1e65aa508345417549203fca7e66a72c1ae4b49ecf5df20755611a37db2a5fff -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_149 571622eaa336944c0987352c01d96cc53996fc3899e53c618170203079931635 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_150 7c40a2630146cb4fb1343b8545644f94338781f73b00a2386fdd58b918e44bf0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_151 9c475c289990d61f69607fe6f42a6a3b25bdbde1cf62bb542ba8a6232e590a5c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_152 f49cd31360da39cd8f826c45f6fd24a3193d23bf672a957158600ee0b1ca19bd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_153 64daab26178a6b2020e644f379c02d36598d0f47b54de87d670cc23a0587d52c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_154 99a940ebf6b8cb643b3efe939423ef1fd2938dee7a53f8fd74c9c06753498942 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_155 aea5908329a2fc8fbf552c81d72a67475a0403345e977fe50f5b6c61a7458597 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_156 16805f91b48f334170ec2027bfc148638db2fdd61333f82f4110c728a2a0396e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_157 7377ed8f523ea581473a105bc91348e9bb09ef5bea19a3f6bbb78c46cef5fe1a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_158 f2c752b8b9fc84969741d10bdb599ba86faee6cc9e634029db5feb56cac7485a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_159 ac2443c83261056e35ba87c547a1a1d824bb65b77f40c46cbe3df5301ebb319d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_160 a25691a1e01656a15025f4c6989ca5b9a4c84fa6e4c213cbb8bb6ab508d08d3d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_161 0359a869ad9a20a84a14b2c81a8952cad77a4b991ff387eb9b15cd842d6cbc24 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_162 f6345150ad139aa3a8e9be7dcf4bbaea7fe8384354ed2d7f9e798e3832f846c3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_163 60d2ce7b975b8bac010228c87405529c5853f49cf855a8bb3bbee2362b6dc93c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_164 b7bdd9ad633a32bb20a73a1c19e6cf08c20454129ccfcb2feb1603d526b69152 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_165 09637cbcb84cd6fad17ec6b65e169f58b94345c89bd0f67a8d9fd9fb0310b7c0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_166 5d316f55326d53689d47b7a9bd1757d3ef0134d7cdbafb2a5142ed611e25116a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_167 daa89d9316981f03ae8ea4cf51fda24ca492fe7f1931cfe21b6d75b64546df09 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_168 d74c02970b150603e9840d8a4ce234ee92568f82f6c6308e87830ccbe4c9909a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_169 39320da8871d3815262746b3236596440c555280b97bdb74371486e79a0efea0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_170 453a891358557f0adb1f197544f41678bde31c5627107ee9a2aef63b92a8605a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_171 edd7d9f51899e1393542aa72f1fe19639f8ccfafa29b6ac88dccb72d511dc13d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_172 9be0c081f4f4d6449321e5646f1441e0d0fc5c0d42d27a356c544cf63c6fec6c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_173 8fca48a4650e8a1f85a5c623ad2713ee2b214daefd1e7c973258b34f5699932a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_174 02801136c1096144c8da8e06172fedee1ba0c225444a442755ed521eca6e7fa6 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_175 4ae4d6543a8100c05dc1669fef49c23224913eebb341fb32189844f82e3d7e49 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_176 86ebd9e13c0c63d3b072ae995930e6c4ad01d1584653bdb6573a4878ec704672 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_177 3d8418a74c4568ed7587a8f98dea8c31d1ea1fa72ecfb1731cb5227698ef7756 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_178 46ac6537df0bb5aa3c562582aa394e58b1e6fcbbeedc3cc9eee265651158e04e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_179 4bcf454e0ed10dfebb3a28d47de12eda2aab882cea67ec4ac89df9b11b48d355 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_180 086199af5194b1f19ddc20f6cd1b7fb64aea8f56de8e5365c0303de5d29c8987 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_181 e6c0dec159d6b88af371a73d7c80f57bc5e92793d5608961c0426e330bce31b1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_182 195dd472c9956c4a42f7511d0456a3b583f9e248fe1d185e7fd9318367c7f4b1 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_183 9e98e35f076ded62408b81f09edafbb5965b29ef6dd6277c72702458bef716c7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_184 9f307e0c13641c733c48cee0994ee11fb7c6780cda681727efbb510c9b641210 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_185 d0849b33a1b0136bd74b4cc81d4794146e2591e2d07848d5246d47ed31e4cce4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_186 f6398aa49d53f285152114cadc14e9c526211384b6e4a5c72193e00121dd2983 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_187 47e16419c6d6f5ba66b18723df07d8149d5c0cdda771f3b4663dd104f6d7a637 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_188 819a10443c67ce835577365d3ff9055d39a82149a55e99825ae171d53a2b801b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_189 ce8bad7207c754d7a530ee850a81dc71abe50e9216c21b01983777b5a46405ec -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_190 6b8497ba9b5681dd900345e56d56d3bc630980b0f6540d2307b4e565b8cd2548 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_191 dc4ecd878703c5ddf7c712e91eef048c36535a0286ada9bc50eef10f790d321c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_192 87e3f3a4028714692bac3744f577b0f485182a1747ddc66dfc26e95525df6924 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_193 503afeeb9791bcb7fa2952a4522ca256d879c34fafdd35c45b3c257abcad4d48 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_194 62f0f98f6b90ce8bd1cca96da3b53a89e004e084fe240b8e7205d6fa755d67b4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_195 a66c50735deffb4399a0e203f815be924e7bb90f8cb30e8e262228df9b815cec -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_196 e37205466577620555683f2116cacbb46e07821cd591f7062a68300fcc5a5c7f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_197 6bf455a7998ba63318c8c01a697a27289e32495880adfcc3af5d1a3894105468 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_198 287b2c0543a16e7314fc8ad25ee8bf033638b140f9e391b8c400b98f8f8e004e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_199 0a6f34db28b50711170009da9a0cf46c6c578018b505c84e84a740aad2a01ac9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_200 1db5ee83fc40b1ec21e6a6509a611b3f89e0c6af9d7bd70633253ae8c7ce45d5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_201 de23432e27d2703d0b5cabddceeea096dcb1637d9369667b1d045b50eaf92fc7 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_202 3298605d05984e22770531659c3f8a50a38f5a02d4e28162b0be2635ff870c07 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_203 c108a01cb3bc204baf18083f246ce055a1afb20c345b5ebe19808b15f7362b5c -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_204 77cb79e3818adf67b6bce935381d5f17ece30db97f270ad5f8bd7d31c65eecaf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_205 fdd893fa9516aea47147690d899839e59766e3081077b1d89600fae134d804e9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_206 190eee58bd9f80ff6694b4f92becefe87f2b7ea95c8b696b216af20956655790 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_207 1431415845317b2f52f39d7b845882789f5034afeed67b1ea731bb774f914a22 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_208 a18c57f7d1ee153a010f84da5102f0fc362998106186e7969b7751d5e211c652 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_209 31a0bee4a9858e9538f909b270f790ec3cf08e53e04047f80c843d1636f4ca37 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_210 27387e3d7246afcd892edf27d81b1ccef6395f5265e9851e99bb27fb67b3e34a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_211 ff40851d04888f1515a6c4965f07ee502c22cc3dcb2eb70904a7c0f36d15b8be -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_212 4c67ca1b8f4a5a6ee8dd7107b4c0c5a14626d8c6950ed310c1e54fc9b0ae7171 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_213 693ad955b0d295debd8e4e4638eed650da28209448526fc6b17ddae5217743eb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_214 cd9881a8fcbaaa1a4bfd68695c59ba8be0daf34e6f28ab05ea09cabcb050123e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_215 115f0cc6e98d9b18a4be8e3d556247ca98abe6c58319e1a2e309c5efdf7d0917 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_216 bf4a3fc2e319e52629546c30879775309718512dbc8dd593d7338727e30e1f30 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_217 f1c9871daa0cd26929a9355943d65a82ecc2f0ad91dd944c2e017fde3dde33bc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_218 509e2286dc12a92632e8734712449f7ac4a259a2de4c647fc768ef21750fb516 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_219 6bcd01523c8ae41adfe05bb4ddf04db142a9e9240aa5706de79e97635586cfa0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_220 32ad3939ef26b02c9af5a31bb1336842ebf79350438ed5ebd135c3ee3e889450 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_221 d485bc01ae2274e46dd96b1e4aa351f021222779b6eadead036aff63fefbae59 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_222 5f71f0090e465c8663998c8d1196b9e1921fbf6dfa015f9186f9decef8733ffa -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_223 d2b18b0037c5caf5137e8a4123b9628aeb5f9092d4956affef3b18b47f9f99dd -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_224 c678826293ac832f895f55ce3cb17207d1aee16764cdfd9a561018aa60f4e51b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_225 ff167b3c7192ee099dfa1e931943f5e47b7061ab4ca81de6aa18d680e939fd8e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_226 b912538b987e0069a523e99b4d15d71b25efe7ccd5c0454589c0a9fa68c22f80 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_227 80add73d30828d0b441e150f8c13ac4d8afd2360d2cc48deee1b9307e2f06f2f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_228 70555331420b36e76a5bb6bc7dd30a11ad592356da06e5b4a1644c4a903b8017 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_229 e0e8833d114e85f6282957f96da2097d7ad5219a47ff57b3b10e19ea85dac6e2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_230 24177ca6fe4474ddaa101dbb7b300e007ff53a577b7f5aa0ad42e042c8e2c231 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_231 d675d436eada800c5f281c1968799dfd821976a570a926446b81fe5a151c831a -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_232 eabdb6e833b4ac2b004172cadc918adc2b9cdee75342eae21abbc6ca5348628f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_233 c708d711787a7f153685a5e39bdf8689c3fa8c9d7ed58ec912083fc9f2a5aa66 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_234 558ff42cb4cac087e1cc48262850dccf54ca438e7a0db63c3d16433c94a8ddda -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_235 11e1c06fd52eb4ca10b366f0c6e8de1cce9c17d9195369c63ce0feb670a8f5ed -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_236 0f41736654a8188a5578cbebfcb46908e4c6a2275422b968784db247599334c3 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_237 a640567f623fe52cf8d5eed693c5c8523f78adb00111943da7094c8ab2158d38 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_238 0ea42db865799a404164085e34ee81069d01f1c8a5651821b7c35d3db0ed3270 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_239 badda7224445817e383746bce92c11d09afd1baf38b64f3f3328640cada61944 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_240 2996a060cc7f94808170dc89939633ca6c4e5396f488bde058527feda6d741bf -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_241 3f16125c517f4344cd5ddcf8485ef77f5de70918297ef680b01da6b5f43c88bb -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_242 57edd8f683ccfc9cfc290448b6b65e797442b8ef7d45b7f4913c032ea6bb5f46 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_243 509695cac100d57ab40b767444f982069e79362e83f8d3bee6ea266be568a8e0 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_244 06ca40de4e2e2ea676bf2afcb7bf30dea3550005c824309ae16c64b1b5ad191d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_245 0177311c581fb4ebc9ebde8622f61d1086fc316921e7ef11d5dcd9814578fc3d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_246 d9856927464a5cce83005643263c1b7be09f907758633a53d363c489d9fadb5f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_247 98178d261681d1593300a785e83d416dd14353b55d035a14e62fbf5bfef28ffe -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_248 af1138365ee98084c8dce078c609e914d1ca3c5d4e25280c5c57e7d149c0a8bc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_249 4a27e814eb58514cf6aa66cf799a4def9dde993a2e1d26e5b3bd2d5ab93b6a89 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_250 479701387c7b76f4837aa630a8d4e743fd6ccb2a544d8f444fbdcb2ae11723e5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_251 744d4dbc990a3c8f1b39651933cf1bef2baa8df886e68c322542818b10ae5c69 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_252 66eca0f51712f44420c6baf514741fb9c2eeb356535ad8cc9c07bbc5ebf36724 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_253 73e59d63d79956db0493cdb9f57436529759483ac27b132023f2a1c54a07b59f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_254 87d35826be52ac02acdc20fb2e5600c2814b90949274925b81d0e1f4f7f47c68 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.name_255 7623f19dfc0eef72103a280f0de42b8450c2f6d761f157af1c57e38d61888498 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.ViewStats.name 0d2273510fc39a14898fbd1347264a42f1fcda3f4bbeb2db72ebd282ba948615 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.view cbbab5895bbc98a7d1b79d1ad5c1083e08e88b12ef231e29efa9e976c158b0b9 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.cpuTimeBegin 8f5f2be366e431b2b4eb3907123708ac87468a3cf2a61c5b350d26eb22376a02 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.cpuTimeEnd 7a50169b8e1992a8bb30b991ebffea3a9b4fba7028e82f9eb247429c0f999085 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.gpuTimeBegin 2dcb52cc484c0f20ffa2ceb2c38f4c68d0d6434708bc394dfc05b282379020dc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.gpuTimeEnd 64fa70cfccb0e171843c5a2afe3ea3746abdf9d65680096db1d2a1144d5271cc -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ViewStats.gpuFrameNum 28c38d1c3d428cd0dd263116464926784a1a07b979b5b2914c6debce5bf6efff -->
<!-- api: M:Inno.Native.Bgfx.bgfx.ViewStats.#ctor(System.Byte*,System.UInt16,System.Int64,System.Int64,System.Int64,System.Int64,System.UInt32) 237ac8b6a9d14b04ae57d57d005e24b00325feae65a42034ad594ecc7fd8ac2d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a49c2dfdf36902da"></a>[`public long Inno.Native.Bgfx.bgfx.ViewStats.cpuTimeBegin`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1568) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a78a7b13c029eb9b"></a>[`public long Inno.Native.Bgfx.bgfx.ViewStats.cpuTimeEnd`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1569) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0d1c68858009d403"></a>[`public uint Inno.Native.Bgfx.bgfx.ViewStats.gpuFrameNum`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1572) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6113f2941369cd4e"></a>[`public long Inno.Native.Bgfx.bgfx.ViewStats.gpuTimeBegin`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1570) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-626198fc6d5610aa"></a>[`public long Inno.Native.Bgfx.bgfx.ViewStats.gpuTimeEnd`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1571) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3694d06194597093"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_0`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1309) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4ea63680a5df7035"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_1`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1310) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c565a3de434b527"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_10`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1319) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f03420f22a77120c"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_100`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1409) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-584041502afed6c3"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_101`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1410) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8be4fe076fba73fb"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_102`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1411) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89125c2370f30d95"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_103`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1412) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-30c0e2d10df98b5a"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_104`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1413) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2225ecd306c481e3"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_105`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1414) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-496ef0cd709f6a96"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_106`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1415) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aaca926cfa54e199"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_107`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1416) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c238e7937c69ed99"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_108`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1417) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e42ddb506899bcdf"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_109`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1418) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-48069356567d1f4e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_11`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1320) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2546bedd2891449a"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_110`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1419) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a1876c66954b7ac4"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_111`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1420) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-509b405bc9916975"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_112`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1421) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-77a21e9b2097ccfe"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_113`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1422) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-86ac69514a5dd0ca"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_114`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1423) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-045f62e76d5897e1"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_115`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1424) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13888b611baced98"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_116`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1425) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-151238bcf37eea6e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_117`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1426) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1d1f531dbf4739e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_118`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1427) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-37c4a956fbea8d6d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_119`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1428) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2df027c5e81b67e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_12`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1321) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-171bb8df2633e6a4"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_120`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1429) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-14a846ad2f1df91d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_121`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1430) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5984a292df63bcd0"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_122`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1431) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-37ed452c8fb1a94c"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_123`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1432) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-14c5fa43b49f8da9"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_124`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1433) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba37914ffa620bf3"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_125`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1434) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfcda4a59d632a43"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_126`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1435) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-038129d73465af82"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_127`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1436) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a9d070231ef9ae43"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_128`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1437) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be1e7d652322e080"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_129`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1438) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5fea41fcfdfa7d47"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_13`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1322) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cef603637afd3150"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_130`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1439) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1281bec61330e827"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_131`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1440) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d9eaa8b61bc7816c"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_132`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1441) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c20f8b6abe4df446"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_133`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1442) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-178da0bd8b36b7ac"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_134`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1443) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e81fc71f694513b"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_135`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1444) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-728041144545e167"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_136`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1445) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1bc4298c35f750e7"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_137`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1446) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b24f9cd49d06a18d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_138`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1447) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ef900e6e7611ba0a"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_139`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1448) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-00196e57abba29c0"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_14`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1323) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6cd3a3963439dc28"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_140`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1449) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6451c7f7795f273"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_141`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1450) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c4db233957166f6"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_142`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1451) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b1f311f15d0a544e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_143`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1452) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5b3a9d9c7c6fade0"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_144`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1453) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d60f10a9d60eaf35"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_145`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1454) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-195be3895d7fda6a"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_146`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1455) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8a927b36f99d49b9"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_147`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1456) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc28f2913dd8abc7"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_148`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1457) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f157b65af74ecd09"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_149`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1458) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07a3ea457ca1f67e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_15`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1324) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d38a3e81a8e4df93"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_150`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1459) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d733e71919a3345"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_151`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1460) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d48e91a52c092b96"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_152`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1461) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab3ff67e20cc3c3c"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_153`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1462) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eab94300467e2654"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_154`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1463) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c3da86095ebf0fbc"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_155`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1464) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5ff9e091e1317fa3"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_156`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1465) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a41e9dcb599293c"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_157`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1466) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1751118065781ee5"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_158`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1467) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba994c5deadbb3ba"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_159`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1468) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9203bbf00b25ab72"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_16`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1325) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a44228c1fd8a6e03"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_160`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1469) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0d9219fdc14e37cf"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_161`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1470) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e60b9bc79715574c"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_162`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1471) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0061176d1fc33210"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_163`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1472) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-31df21c1cfefb1c3"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_164`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1473) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b182c85d19d692ea"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_165`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1474) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c973206e50f9e6e1"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_166`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1475) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b3daf17720bf0951"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_167`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1476) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e1d1a98f8b7b063"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_168`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1477) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dbef74948bd46b62"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_169`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1478) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b27da518f8908ac9"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_17`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1326) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-51b9ba5ba1e17700"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_170`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1479) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d5411fb1bf9d016"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_171`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1480) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8f01f6b477c6b633"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_172`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1481) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-42cb4c9913fb427a"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_173`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1482) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-951aea023dfeed96"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_174`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1483) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c93afe3d80ed55dc"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_175`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1484) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58c44765964a2854"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_176`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1485) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f3f2d8eff11eb95"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_177`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1486) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-16c4027e7b03ac42"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_178`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1487) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b2c63484a67d6a32"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_179`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1488) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-091dc8dad3e959c0"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_18`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1327) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6b0a04a60ff50967"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_180`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1489) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9892f9e652d01269"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_181`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1490) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1af81c02a1ad271"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_182`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1491) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7f9acd113b2dff0"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_183`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1492) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9e1f3f7a8815ffeb"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_184`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1493) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6298d0ad8d3b7edb"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_185`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1494) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c29727e88dc6cb9d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_186`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1495) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2818e598265be05d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_187`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1496) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-62144150e021fd55"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_188`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1497) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e0e1464754290b2e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_189`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1498) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-99b6ff392e9517c2"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_19`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1328) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8632a709840a198f"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_190`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1499) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0df37976284369d1"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_191`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1500) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a81166016205f674"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_192`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1501) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2166d58f8271fd84"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_193`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1502) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd7511bf459913af"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_194`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1503) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6c16db2eb05c88d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_195`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1504) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-885063e225065548"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_196`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1505) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a5bdaea4ad43e019"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_197`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1506) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d33690a2e5d4df93"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_198`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1507) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1989d94486dc44ea"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_199`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1508) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a515c2436407ae09"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_2`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1311) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-051249cbcd777bce"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_20`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1329) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-879008e6cdcee612"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_200`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1509) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb770b7eea5a0d97"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_201`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1510) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cdd9a4fbafb6e150"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_202`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1511) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7980b73bc69723e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_203`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1512) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-93ce8172145d72b2"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_204`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1513) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c9b3c916ef14e2f"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_205`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1514) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-31bd16a675a826d6"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_206`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1515) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe2baf42b5aae831"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_207`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1516) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-982236bbc6c0048d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_208`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1517) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b000534462c6b2a"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_209`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1518) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c38c13de7fe54382"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_21`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1330) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35752fa4b160270d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_210`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1519) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-56054d0b3f802469"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_211`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1520) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ddcfb0afaa1c8377"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_212`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1521) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f9568befdc73ab34"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_213`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1522) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-68a28755a7020432"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_214`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1523) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9268d0ad7d9f4652"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_215`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1524) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6aaa3761d80946b"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_216`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1525) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de01a82628890e89"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_217`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1526) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dab217a0bb09ce0d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_218`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1527) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0cb827b8b9163f25"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_219`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1528) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a088016b7b66f6b5"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_22`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1331) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f006a81be70abf3d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_220`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1529) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-037f01dfde357048"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_221`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1530) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fec30d7c739c85a0"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_222`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1531) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-acdebda677a4b5c3"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_223`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1532) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d11d06ff7d0e6da7"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_224`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1533) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c971262f4bffef37"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_225`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1534) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7a7945fc03925049"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_226`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1535) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b763a7ed1315b27c"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_227`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1536) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c6bde98467b9b887"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_228`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1537) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2fb459c44a57e5d6"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_229`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1538) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-575f51fe434ca9fa"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_23`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1332) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-00167bea093ff3dc"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_230`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1539) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5ce8c6081b47eb26"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_231`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1540) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8899f47c2a3dc0d1"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_232`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1541) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2758b39e2a671b6e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_233`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1542) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6e6e1b747ec2aba"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_234`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1543) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b7c503464c4c0da4"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_235`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1544) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab43d42ce869a406"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_236`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1545) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-34b0f87a6951c828"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_237`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1546) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85bb56f5a7a22034"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_238`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1547) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-46bfea65a2d04111"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_239`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1548) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f725df8317c6aa4"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_24`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1333) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c0e771c9b01d2c3"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_240`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1549) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4627b347e831ec42"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_241`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1550) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e77e98cc5216b3ab"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_242`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1551) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c54763e6eaf39e3"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_243`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1552) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-25fcbad8d0e52569"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_244`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1553) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-10464704b8030dd9"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_245`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1554) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-25ffce35c4bd0a7f"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_246`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1555) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5be69518cc2a332d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_247`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1556) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58f8cb3c5b97c185"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_248`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1557) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b9e0ce75017e726"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_249`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1558) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c461046e0294ef24"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_25`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1334) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fc9d1697d7ffb356"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_250`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1559) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2c76aed4dd357e69"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_251`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1560) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13d9f2df6578895e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_252`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1561) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ffa8a9186f86e85f"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_253`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1562) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89469e705ec8713e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_254`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1563) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-15aa63a35a591630"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_255`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1564) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d88301c8e91ea6ef"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_26`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1335) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35c52a374e355160"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_27`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1336) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb026e60b589f1bb"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_28`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1337) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-95e619babd503036"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_29`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1338) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f65db786005631e8"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_3`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1312) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fdcc9cfcc40f3564"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_30`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1339) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-18eac75285a6adc5"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_31`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1340) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a7e07a73f3ffde6"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_32`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1341) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-623e07a637576f1a"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_33`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1342) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c1888a63ef9b1dc"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_34`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1343) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-beda89e0feb33c7d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_35`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1344) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2319415eca4c2867"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_36`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1345) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0b4e1867611c317f"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_37`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1346) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-15c84303f437d4c4"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_38`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1347) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-56a9ed2c174c0b3e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_39`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1348) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60664933c4a6b14b"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_4`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1313) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c1541fc7607d831"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_40`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1349) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3f1440581201700f"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_41`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1350) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b5702bf60f47c064"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_42`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1351) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c3b9759480a5d026"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_43`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1352) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-126d2aadd3cb6cd6"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_44`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1353) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-449f0f8359693ae5"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_45`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1354) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fd00ce5a37325822"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_46`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1355) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8a84ef69a7da44e4"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_47`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1356) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-70f9cbaddc1e71db"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_48`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1357) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e814183d8a1b28a"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_49`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1358) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe069a1582e2f99e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_5`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1314) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-95a48f286cc3e065"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_50`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1359) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9599a04aae2b4fd7"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_51`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1360) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b69b757f66ff22a0"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_52`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1361) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-52196e1ebb78dbe1"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_53`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1362) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-498d192a47b0e73d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_54`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1363) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21ee919ac95c9e20"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_55`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1364) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-011c6dfca734d4e4"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_56`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1365) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61c805a1d0bf0a82"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_57`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1366) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01d35f1759706b83"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_58`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1367) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-103c2e52c41ab58a"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_59`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1368) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-774c1c0e7271b37e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_6`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1315) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-599af8b58b6b1904"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_60`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1369) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e9702a45b34abf7f"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_61`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1370) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-46607835a832bc50"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_62`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1371) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6131c94db271c7a5"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_63`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1372) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f30309bb08b5621"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_64`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1373) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-571fd2e4a6599938"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_65`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1374) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13d5ed75ac27c78f"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_66`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1375) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a045e9d997fc587"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_67`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1376) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53c455d01f8e5102"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_68`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1377) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3cadf4c27c2bb082"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_69`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1378) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-96727f1340529886"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_7`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1316) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eb557821c234ae00"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_70`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1379) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-73a260a03e3a1dc6"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_71`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1380) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea65133209874755"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_72`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1381) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c0bdee18263ca62"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_73`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1382) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d82008c71ce461db"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_74`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1383) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c1b27e38234600ca"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_75`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1384) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ffb8f347f0ad0f0c"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_76`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1385) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8df2defb47a398a0"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_77`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1386) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2645cc76c0ae233"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_78`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1387) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e078c62fcc5d7b71"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_79`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1388) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a004e28d2f357d45"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_8`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1317) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f5321303f9df2a5"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_80`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1389) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8bae53d46a8d2f4e"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_81`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1390) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd20c97e33f16143"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_82`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1391) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-32ece3e470c32abd"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_83`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1392) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-076d562952be1a0b"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_84`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1393) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8d98301dc59bbbb2"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_85`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1394) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be1623a60bd04289"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_86`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1395) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b2332161c7c55716"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_87`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1396) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc496bd81b50d5f8"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_88`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1397) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0576da8d03142a67"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_89`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1398) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7cb5a09f79c0d198"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_9`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1318) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b828399fe16214d"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_90`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1399) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a46b66c361c9de12"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_91`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1400) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7d1cf778b875e863"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_92`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1401) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c5a44e82cd3bebb1"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_93`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1402) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60fb0d3aeb7d5eb4"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_94`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1403) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f49919b5cff080ec"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_95`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1404) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eb0262001335d50f"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_96`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1405) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a3af4e2922b629a"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_97`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1406) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-93d4d9b65cd99b17"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_98`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1407) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e8c158b7c1223196"></a>[`public byte Inno.Native.Bgfx.bgfx.ViewStats.name_99`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1408) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce00f92e508930ec"></a>[`public ushort Inno.Native.Bgfx.bgfx.ViewStats.view`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1567) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1464a76771115e66"></a>[`public Inno.Native.Bgfx.bgfx.ViewStats.ViewStats(byte* name = null, ushort view = 0, long cpuTimeBegin = 0, long cpuTimeEnd = 0, long gpuTimeBegin = 0, long gpuTimeEnd = 0, uint gpuFrameNum = 0)`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1573) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fa487504829ad32b"></a>[`public System.Span<byte> Inno.Native.Bgfx.bgfx.ViewStats.name { get; }`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1565) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-796b4f9fd30f8a5d"></a>[`public struct Inno.Native.Bgfx.bgfx.ViewStats`](../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1307) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<!-- generated-abi-index:end -->
