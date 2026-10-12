# Inno.Native.Bgfx ABI · CapsLimits → RenderFrame

[项目与生命周期](../Inno.Native.Bgfx.md) · [ABI 索引](README.md)

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
| <a id="api-65e3f1bf15576517"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxBlits`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L997) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39eb0aaa9466a700"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxComputeBindings`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1007) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-70cc05a83cc666ea"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxDrawCalls`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L996) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-933783d3bfe9bfb5"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxDynamicIndexBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1012) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-96297dad7347d9a4"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxDynamicVertexBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1013) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fa950218380585c9"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxEncoders`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1016) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5db62de07f47d7ad"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxFBAttachments`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1002) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-93aa81e57e218929"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxFrameBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1001) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cacce2bcfaa1ed36"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxIndexBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1010) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-51ef725397f6caa3"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxOcclusionQueries`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1015) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af03219246ee4a5c"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxPrograms`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1003) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a86d75eeafa0711"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxShaders`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1004) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-922e67fb588c721c"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureLayers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L999) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-44b8bfc0aad4819b"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTextures`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1005) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-82239e7760c0e4ba"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureSamplers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1006) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc102356bb523bab"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTextureSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L998) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a960d6261e8a09be"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTransientIbSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1019) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-52b394fe124f3846"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxTransientVbSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1018) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-973474f577357a8d"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxUniforms`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1014) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-736621a2335a3f40"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexBuffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1011) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9cbee65c09c6e4d8"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexLayouts`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1008) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c048618e05b00c1"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxVertexStreams`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1009) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-194931c571755652"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.maxViews`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1000) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-90dacdb5313092ff"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.minResourceCbSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1017) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c77e99054396b1a0"></a>[`public uint Inno.Native.Bgfx.bgfx.CapsLimits.minUniformBufferSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1020) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfcc2585f10dd6ae"></a>[`public Inno.Native.Bgfx.bgfx.CapsLimits.CapsLimits(uint maxDrawCalls = 0, uint maxBlits = 0, uint maxTextureSize = 0, uint maxTextureLayers = 0, uint maxViews = 0, uint maxFrameBuffers = 0, uint maxFBAttachments = 0, uint maxPrograms = 0, uint maxShaders = 0, uint maxTextures = 0, uint maxTextureSamplers = 0, uint maxComputeBindings = 0, uint maxVertexLayouts = 0, uint maxVertexStreams = 0, uint maxIndexBuffers = 0, uint maxVertexBuffers = 0, uint maxDynamicIndexBuffers = 0, uint maxDynamicVertexBuffers = 0, uint maxUniforms = 0, uint maxOcclusionQueries = 0, uint maxEncoders = 0, uint minResourceCbSize = 0, uint maxTransientVbSize = 0, uint maxTransientIbSize = 0, uint minUniformBufferSize = 0)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1021) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d3d1124616a0db65"></a>[`public struct Inno.Native.Bgfx.bgfx.CapsLimits`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L994) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-55683955805566e2"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.Color = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3430) | 当前常量或枚举值：`1`。 | — |
| <a id="api-81fa711b2c7eda24"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.Depth = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3431) | 当前常量或枚举值：`2`。 | — |
| <a id="api-5c0a3651e5629d25"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor0 = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3433) | 当前常量或枚举值：`8`。 | — |
| <a id="api-5cea43178a812f5c"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor1 = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3434) | 当前常量或枚举值：`16`。 | — |
| <a id="api-5bd465ec83a0df99"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor2 = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3435) | 当前常量或枚举值：`32`。 | — |
| <a id="api-2c78911dccf14ef2"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor3 = 64`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3436) | 当前常量或枚举值：`64`。 | — |
| <a id="api-74bd2f698cca607f"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor4 = 128`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3437) | 当前常量或枚举值：`128`。 | — |
| <a id="api-77d52fc8b49e1853"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor5 = 256`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3438) | 当前常量或枚举值：`256`。 | — |
| <a id="api-23c202c7cc85e35a"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor6 = 512`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3439) | 当前常量或枚举值：`512`。 | — |
| <a id="api-7df2160d2166599a"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor7 = 1024`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3440) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-71a273235ece6fd0"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColorMask = Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor0 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor1 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor2 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor3 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor4 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor5 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor6 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor7`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3443) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor0 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor1 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor2 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor3 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor4 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor5 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor6 &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColor7`。 | — |
| <a id="api-fda9a32fe3f8ccb4"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardDepth = 2048`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3441) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-858ce7d5b3105b6d"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardMask = Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColorMask &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardDepth &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardStencil`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3444) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardColorMask &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardDepth &#124; Inno.Native.Bgfx.bgfx.ClearFlags.DiscardStencil`。 | — |
| <a id="api-a7b4b210e6c342bf"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.DiscardStencil = 4096`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3442) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-f489cdc9c03d2a86"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3429) | 当前常量或枚举值：`0`。 | — |
| <a id="api-df13a9926895b371"></a>[`Inno.Native.Bgfx.bgfx.ClearFlags.Stencil = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3432) | 当前常量或枚举值：`4`。 | — |
| <a id="api-36f5a142348f937a"></a>[`public enum Inno.Native.Bgfx.bgfx.ClearFlags : ushort`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3427) | Native 取值集合；当前取值见本类型成员。 | — |

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
| <a id="api-0bc5ae37abaadda2"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3457) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d348511dce75e85f"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeY = Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX &#124; Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveY`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3459) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX &#124; Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveY`。 | — |
| <a id="api-bec211413355f27b"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeZ = Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX &#124; Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveZ`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3461) | 当前常量或枚举值：`Inno.Native.Bgfx.bgfx.CubeMapFlags.NegativeX &#124; Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveZ`。 | — |
| <a id="api-8f74dc88979c48c4"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveX = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3456) | 当前常量或枚举值：`0`。 | — |
| <a id="api-d23c2f90aa612228"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveY = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3458) | 当前常量或枚举值：`2`。 | — |
| <a id="api-5cefec28499bd4ff"></a>[`Inno.Native.Bgfx.bgfx.CubeMapFlags.PositiveZ = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3460) | 当前常量或枚举值：`4`。 | — |
| <a id="api-2c2bac8b06dbdf27"></a>[`public enum Inno.Native.Bgfx.bgfx.CubeMapFlags : uint`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3454) | Native 取值集合；当前取值见本类型成员。 | — |

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
| <a id="api-685f04e0445b1dd6"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Ifh = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3475) | 当前常量或枚举值：`2`。 | — |
| <a id="api-d8ae087a15dfcb99"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3473) | 当前常量或枚举值：`0`。 | — |
| <a id="api-50916d8add82e4d0"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Profiler = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3478) | 当前常量或枚举值：`16`。 | — |
| <a id="api-6d432868431fc190"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Stats = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3476) | 当前常量或枚举值：`4`。 | — |
| <a id="api-88d1da0621d16dfe"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Text = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3477) | 当前常量或枚举值：`8`。 | — |
| <a id="api-cd9178b52165a1b6"></a>[`Inno.Native.Bgfx.bgfx.DebugFlags.Wireframe = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3474) | 当前常量或枚举值：`1`。 | — |
| <a id="api-63de25306cd42b14"></a>[`public enum Inno.Native.Bgfx.bgfx.DebugFlags : uint`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3471) | Native 取值集合；当前取值见本类型成员。 | — |

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
| <a id="api-a62d451b83c8860a"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.All = 255`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3497) | 当前常量或枚举值：`255`。 | — |
| <a id="api-8841f49063f259c3"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.Bindings = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3491) | 当前常量或枚举值：`1`。 | — |
| <a id="api-f918abeac53be327"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.IndexBuffer = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3492) | 当前常量或枚举值：`2`。 | — |
| <a id="api-f26d559644d72567"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.InstanceData = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3493) | 当前常量或枚举值：`4`。 | — |
| <a id="api-10f2e3ab39580016"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3490) | 当前常量或枚举值：`0`。 | — |
| <a id="api-091a1a75aa26214b"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.State = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3494) | 当前常量或枚举值：`8`。 | — |
| <a id="api-4e8db6bdaf7b3511"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.Transform = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3495) | 当前常量或枚举值：`16`。 | — |
| <a id="api-ab5bf5c555e4d16c"></a>[`Inno.Native.Bgfx.bgfx.DiscardFlags.VertexStreams = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3496) | 当前常量或枚举值：`32`。 | — |
| <a id="api-256da9f62a11c03e"></a>[`public enum Inno.Native.Bgfx.bgfx.DiscardFlags : uint`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3488) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle 4b32340364328c0af43affec0db655738ced8926e14f2ed7e2196551ca68ca37 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.idx 96546596c287b1ace30c93454b55f9ef7b339270da9c1984d461c8b4647392e0 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.Valid 18fa3a030c8c7e32807638ef011edb4020bf9bda14ffe49052d34057ed4f4ef9 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.#ctor(System.UInt16) 9b03ab3fc3d13c8efd0e0900e04b0341fd02cd54c79cdc489a33f7b395182ec0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6c89292187534ca3"></a>[`public ushort Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2051) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2f15c3d7d0b6736"></a>[`public Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.DynamicIndexBufferHandle(ushort idx = 0)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2054) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1c84e9aeba2e96ad"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle.Valid { get; }`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2052) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-91a36af391c9f8af"></a>[`public struct Inno.Native.Bgfx.bgfx.DynamicIndexBufferHandle`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2049) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle 756c3298c362b4a2a71c9059d3f25f46a42ee62dae513da697c256cd1698fd4e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.idx 530fe00296e11bb9cff2e355c64c806812c8879cdfa2598611725b239f7330d6 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.Valid 2cb98500b61ffc741072501d4b5b65696b5bf88a778c0930441cd57a6da15393 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.#ctor(System.UInt16) edd8b9450426c5329028deeddfe7dc6280d8f5e34257a36ea7c76cf10f375bc3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9ff345f95247caf5"></a>[`public ushort Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2069) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0ca2621934475c0c"></a>[`public Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.DynamicVertexBufferHandle(ushort idx = 0)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2072) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4e1792860450d18e"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle.Valid { get; }`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2070) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b89f5d4be9bbc624"></a>[`public struct Inno.Native.Bgfx.bgfx.DynamicVertexBufferHandle`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2067) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-88635e27add577de"></a>[`public Inno.Native.Bgfx.bgfx.Encoder.Encoder(nint handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2839) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-94885fb9637f6b5a"></a>[`public bool Inno.Native.Bgfx.bgfx.Encoder.Equals(Inno.Native.Bgfx.bgfx.Encoder other)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2851) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-6e0145c8df2a2faf"></a>[`public override bool Inno.Native.Bgfx.bgfx.Encoder.Equals(object? obj)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2852) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-357b915f970f9786"></a>[`public override int Inno.Native.Bgfx.bgfx.Encoder.GetHashCode()`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2853) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-84d9784b70270195"></a>[`public static bool Inno.Native.Bgfx.bgfx.Encoder.operator ==(Inno.Native.Bgfx.bgfx.Encoder left, Inno.Native.Bgfx.bgfx.Encoder right)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2849) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7ea1377692c5083e"></a>[`public static Inno.Native.Bgfx.bgfx.Encoder.implicit operator Inno.Native.Bgfx.bgfx.Encoder(nint handle)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2848) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-16aea6701e0c9220"></a>[`public static bool Inno.Native.Bgfx.bgfx.Encoder.operator !=(Inno.Native.Bgfx.bgfx.Encoder left, Inno.Native.Bgfx.bgfx.Encoder right)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2850) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c8501d0be95fb65a"></a>[`public nint Inno.Native.Bgfx.bgfx.Encoder.Handle { get; }`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2844) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c1a2564c85b28c14"></a>[`public bool Inno.Native.Bgfx.bgfx.Encoder.IsNull { get; }`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2845) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ac9ebc0c3fc8a2e1"></a>[`public static Inno.Native.Bgfx.bgfx.Encoder Inno.Native.Bgfx.bgfx.Encoder.Null { get; }`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2846) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7ea3cbdebc3e14fb"></a>[`public readonly struct Inno.Native.Bgfx.bgfx.Encoder : System.IEquatable<Inno.Native.Bgfx.bgfx.Encoder>`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2837) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.EncoderStats</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.EncoderStats 906556f0a1d9a7ff73abc5c8b4caa46381d8d549637cbe85af5a6a0cd595d889 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.EncoderStats.cpuTimeBegin 51e2efaa9afb68e2872a57268a6273982d65c6fb86fe5c6d4c72e2332959de45 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.EncoderStats.cpuTimeEnd 772bdf9572237b6536f8ec05d8f139d1093293acd41861fd44a66121e8240686 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.EncoderStats.#ctor(System.Int64,System.Int64) d726e90ca0800dfd09c2c9117031ae5eca8ec8769772e5ffafd0e7d7eaaaede9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0cc480539714c685"></a>[`public long Inno.Native.Bgfx.bgfx.EncoderStats.cpuTimeBegin`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1853) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c290f76248752c9a"></a>[`public long Inno.Native.Bgfx.bgfx.EncoderStats.cpuTimeEnd`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1854) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3c7c1d63a0821896"></a>[`public Inno.Native.Bgfx.bgfx.EncoderStats.EncoderStats(long cpuTimeBegin = 0, long cpuTimeEnd = 0)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1855) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f82d561287f6f4b2"></a>[`public struct Inno.Native.Bgfx.bgfx.EncoderStats`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1851) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-e1c7be5f766534e2"></a>[`Inno.Native.Bgfx.bgfx.Fatal.Count = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L31) | 当前常量或枚举值：`5`。 | — |
| <a id="api-d86286188a268fa5"></a>[`Inno.Native.Bgfx.bgfx.Fatal.DebugCheck = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L26) | 当前常量或枚举值：`0`。 | — |
| <a id="api-c8d5e4f834222239"></a>[`Inno.Native.Bgfx.bgfx.Fatal.DeviceLost = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L30) | 当前常量或枚举值：`4`。 | — |
| <a id="api-c3cf46b00327cda2"></a>[`Inno.Native.Bgfx.bgfx.Fatal.InvalidShader = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L27) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d1d19e1f870ff975"></a>[`Inno.Native.Bgfx.bgfx.Fatal.UnableToCreateTexture = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L29) | 当前常量或枚举值：`3`。 | — |
| <a id="api-ec62e4affe72ad73"></a>[`Inno.Native.Bgfx.bgfx.Fatal.UnableToInitialize = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L28) | 当前常量或枚举值：`2`。 | — |
| <a id="api-f1e7eab52cb66409"></a>[`public enum Inno.Native.Bgfx.bgfx.Fatal : int`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L24) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.FrameBufferHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.FrameBufferHandle b0ee56f2e36d2c07e534c0482c64778e90be27c0ef1f1396c27144b452944a1f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.FrameBufferHandle.idx 5bae03d21976dd700e1ecab1b2da33b72dc4b9260178dc360eafc6aadb8a5fd2 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.FrameBufferHandle.Valid fd4705399bd43a9984b165a030ad87a361343fd1e0d63d70393c8d5b1313863d -->
<!-- api: M:Inno.Native.Bgfx.bgfx.FrameBufferHandle.#ctor(System.UInt16) 51131143b8df9f8a35520b9ee1ac7a2f3b7e30d62549c294583dacb3a0630cd9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1b152bc971c6863b"></a>[`public ushort Inno.Native.Bgfx.bgfx.FrameBufferHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2267) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-44e9aecaaf760a5c"></a>[`public Inno.Native.Bgfx.bgfx.FrameBufferHandle.FrameBufferHandle(ushort idx = 0)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2270) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1c560a8c46664762"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.FrameBufferHandle.Valid { get; }`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2268) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-07501cc8d93490b2"></a>[`public struct Inno.Native.Bgfx.bgfx.FrameBufferHandle`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2265) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-c43e6f1ea597cbe0"></a>[`Inno.Native.Bgfx.bgfx.FrameFlags.DebugCapture = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3510) | 当前常量或枚举值：`1`。 | — |
| <a id="api-5a6922758581a816"></a>[`Inno.Native.Bgfx.bgfx.FrameFlags.Discard = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3511) | 当前常量或枚举值：`2`。 | — |
| <a id="api-ce5dd3173913b278"></a>[`Inno.Native.Bgfx.bgfx.FrameFlags.Flush = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3512) | 当前常量或枚举值：`4`。 | — |
| <a id="api-7791472875fccd60"></a>[`Inno.Native.Bgfx.bgfx.FrameFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3509) | 当前常量或枚举值：`0`。 | — |
| <a id="api-fb7fdb29c3b233fc"></a>[`public enum Inno.Native.Bgfx.bgfx.FrameFlags : uint`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3507) | Native 取值集合；当前取值见本类型成员。 | — |

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
| <a id="api-85a895ffa6a5093f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Alloc = 26`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L439) | 当前常量或枚举值：`26`。 | — |
| <a id="api-bb969015e6940eb4"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocInstanceDataBuffer = 57`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L470) | 当前常量或枚举值：`57`。 | — |
| <a id="api-0d3f590f8a4c4b49"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocTransform = 170`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L583) | 当前常量或枚举值：`170`。 | — |
| <a id="api-507c30efc94f5dec"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientBuffers = 56`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L469) | 当前常量或枚举值：`56`。 | — |
| <a id="api-a709bc8bbbd7544c"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientIndexBuffer = 54`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L467) | 当前常量或枚举值：`54`。 | — |
| <a id="api-f05db85ee0df0cb3"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AllocTransientVertexBuffer = 55`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L468) | 当前常量或枚举值：`55`。 | — |
| <a id="api-67750b9743fc7927"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.AttachmentInit = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L413) | 当前常量或枚举值：`0`。 | — |
| <a id="api-5883e5d81ca902b6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Blit = 201`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L614) | 当前常量或枚举值：`201`。 | — |
| <a id="api-033a8e815148ef7a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CalcTextureSize = 69`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L482) | 当前常量或枚举值：`69`。 | — |
| <a id="api-c9d9dac5946fa719"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Copy = 27`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L440) | 当前常量或枚举值：`27`。 | — |
| <a id="api-a59a35dafff487ae"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Count = 202`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L615) | 当前常量或枚举值：`202`。 | — |
| <a id="api-deb35b30de1ffb76"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateComputeProgram = 65`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L478) | 当前常量或枚举值：`65`。 | — |
| <a id="api-e38ef9ea4ac56565"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicIndexBuffer = 43`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L456) | 当前常量或枚举值：`43`。 | — |
| <a id="api-2cf9e85c14fa7395"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicIndexBufferMem = 44`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L457) | 当前常量或枚举值：`44`。 | — |
| <a id="api-7378243eb84a1ca6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicVertexBuffer = 47`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L460) | 当前常量或枚举值：`47`。 | — |
| <a id="api-4037f3a0e69ef8ab"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateDynamicVertexBufferMem = 48`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L461) | 当前常量或枚举值：`48`。 | — |
| <a id="api-9141babfb0a48504"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBuffer = 82`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L495) | 当前常量或枚举值：`82`。 | — |
| <a id="api-5ed772833357b885"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromAttachment = 85`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L498) | 当前常量或枚举值：`85`。 | — |
| <a id="api-55736e389a8543de"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromHandles = 84`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L497) | 当前常量或枚举值：`84`。 | — |
| <a id="api-047b35b246803fe0"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferFromNwh = 86`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L499) | 当前常量或枚举值：`86`。 | — |
| <a id="api-f7a24a9c48cc3e0b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateFrameBufferScaled = 83`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L496) | 当前常量或枚举值：`83`。 | — |
| <a id="api-b5e2e51d8993426f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateIndexBuffer = 35`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L448) | 当前常量或枚举值：`35`。 | — |
| <a id="api-4a0d5070e2aef459"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateIndirectBuffer = 58`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L471) | 当前常量或枚举值：`58`。 | — |
| <a id="api-e5353f5a182bd08d"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateOcclusionQuery = 94`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L507) | 当前常量或枚举值：`94`。 | — |
| <a id="api-67432dbb454409e4"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateProgram = 64`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L477) | 当前常量或枚举值：`64`。 | — |
| <a id="api-642ef1fd65fc4ff5"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateShader = 60`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L473) | 当前常量或枚举值：`60`。 | — |
| <a id="api-815b5dfcf30d5172"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture = 70`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L483) | 当前常量或枚举值：`70`。 | — |
| <a id="api-d46806e272de45cc"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture2D = 71`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L484) | 当前常量或枚举值：`71`。 | — |
| <a id="api-1683f804c203dc6d"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture2DScaled = 72`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L485) | 当前常量或枚举值：`72`。 | — |
| <a id="api-0a7281c761f6f746"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTexture3D = 73`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L486) | 当前常量或枚举值：`73`。 | — |
| <a id="api-14420387b9f7fa09"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateTextureCube = 74`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L487) | 当前常量或枚举值：`74`。 | — |
| <a id="api-9b9f95c9d8a52e13"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateUniform = 90`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L503) | 当前常量或枚举值：`90`。 | — |
| <a id="api-99cef14808b7a8d6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateUniformWithFreq = 91`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L504) | 当前常量或枚举值：`91`。 | — |
| <a id="api-6f2e1baf2f4b01ab"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateVertexBuffer = 40`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L453) | 当前常量或枚举值：`40`。 | — |
| <a id="api-0be9d301ccf88065"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.CreateVertexLayout = 38`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L451) | 当前常量或枚举值：`38`。 | — |
| <a id="api-7280f13dff5a97aa"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DbgTextClear = 31`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L444) | 当前常量或枚举值：`31`。 | — |
| <a id="api-36258c40c9c1a806"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DbgTextImage = 34`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L447) | 当前常量或枚举值：`34`。 | — |
| <a id="api-65d3b625fd3ea013"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DbgTextPrintf = 32`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L445) | 当前常量或枚举值：`32`。 | — |
| <a id="api-c6971919bf077fa2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DbgTextVprintf = 33`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L446) | 当前常量或枚举值：`33`。 | — |
| <a id="api-749f24806ff8d30a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyDynamicIndexBuffer = 46`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L459) | 当前常量或枚举值：`46`。 | — |
| <a id="api-13df7fb7a87febdc"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyDynamicVertexBuffer = 50`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L463) | 当前常量或枚举值：`50`。 | — |
| <a id="api-03f738743c0c5cfe"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyFrameBuffer = 89`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L502) | 当前常量或枚举值：`89`。 | — |
| <a id="api-85e9ce86ac819890"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyIndexBuffer = 37`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L450) | 当前常量或枚举值：`37`。 | — |
| <a id="api-6685182d0c0ee5e8"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyIndirectBuffer = 59`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L472) | 当前常量或枚举值：`59`。 | — |
| <a id="api-9879c983dbc1a970"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyOcclusionQuery = 96`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L509) | 当前常量或枚举值：`96`。 | — |
| <a id="api-e50f4b643561b3da"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyProgram = 66`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L479) | 当前常量或枚举值：`66`。 | — |
| <a id="api-7f24fe09a08d57ad"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyShader = 63`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L476) | 当前常量或枚举值：`63`。 | — |
| <a id="api-e368b60ab5e08171"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyTexture = 81`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L494) | 当前常量或枚举值：`81`。 | — |
| <a id="api-09ecd371004e5f47"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyUniform = 93`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L506) | 当前常量或枚举值：`93`。 | — |
| <a id="api-2a2c96c292a3259a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyVertexBuffer = 42`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L455) | 当前常量或枚举值：`42`。 | — |
| <a id="api-34a899dd698472ca"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DestroyVertexLayout = 39`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L452) | 当前常量或枚举值：`39`。 | — |
| <a id="api-eb05801310aaae45"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Discard = 200`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L613) | 当前常量或枚举值：`200`。 | — |
| <a id="api-7ee9e8cd8e18bb14"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Dispatch = 198`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L611) | 当前常量或枚举值：`198`。 | — |
| <a id="api-780eb4f1274a8e18"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.DispatchIndirect = 199`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L612) | 当前常量或枚举值：`199`。 | — |
| <a id="api-71c0cf0ddf10e8c7"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderAllocTransform = 122`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L535) | 当前常量或枚举值：`122`。 | — |
| <a id="api-5c7371c9a38b0a63"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderBegin = 112`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L525) | 当前常量或枚举值：`112`。 | — |
| <a id="api-a300cd97efd22d2a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderBlit = 155`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L568) | 当前常量或枚举值：`155`。 | — |
| <a id="api-7cf672d21f42f32e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderDiscard = 154`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L567) | 当前常量或枚举值：`154`。 | — |
| <a id="api-966e173e32ca68be"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderDispatch = 152`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L565) | 当前常量或枚举值：`152`。 | — |
| <a id="api-25888a212ca46f53"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderDispatchIndirect = 153`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L566) | 当前常量或枚举值：`153`。 | — |
| <a id="api-5750c5f3cd4657fc"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderEnd = 113`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L526) | 当前常量或枚举值：`113`。 | — |
| <a id="api-7b61fd7dda94a022"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeDynamicIndexBuffer = 148`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L561) | 当前常量或枚举值：`148`。 | — |
| <a id="api-0d6fac852d8101a7"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeDynamicVertexBuffer = 149`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L562) | 当前常量或枚举值：`149`。 | — |
| <a id="api-db68b460687f7c6e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeIndexBuffer = 146`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L559) | 当前常量或枚举值：`146`。 | — |
| <a id="api-4bf12d88c3532b6c"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeIndirectBuffer = 150`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L563) | 当前常量或枚举值：`150`。 | — |
| <a id="api-1acbd608d5435008"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetComputeVertexBuffer = 147`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L560) | 当前常量或枚举值：`147`。 | — |
| <a id="api-4c07b7fc8bb871e6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetCondition = 116`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L529) | 当前常量或枚举值：`116`。 | — |
| <a id="api-bfda36cdd2d2b3b8"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicIndexBuffer = 127`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L540) | 当前常量或枚举值：`127`。 | — |
| <a id="api-1b5ff9bd1f77f286"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicVertexBuffer = 131`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L544) | 当前常量或枚举值：`131`。 | — |
| <a id="api-713367ef8a81ce14"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetDynamicVertexBufferWithLayout = 132`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L545) | 当前常量或枚举值：`132`。 | — |
| <a id="api-1458c391029bffcf"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetImage = 151`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L564) | 当前常量或枚举值：`151`。 | — |
| <a id="api-2360fdadbfeda9d3"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetIndexBuffer = 126`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L539) | 当前常量或枚举值：`126`。 | — |
| <a id="api-1571c750bcefcf24"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceCount = 139`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L552) | 当前常量或枚举值：`139`。 | — |
| <a id="api-d1c77e6ad2829c20"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataBuffer = 136`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L549) | 当前常量或枚举值：`136`。 | — |
| <a id="api-ec6c827052145565"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataFromDynamicVertexBuffer = 138`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L551) | 当前常量或枚举值：`138`。 | — |
| <a id="api-bf337dd0e43b7807"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetInstanceDataFromVertexBuffer = 137`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L550) | 当前常量或枚举值：`137`。 | — |
| <a id="api-d57f7926986d7b7e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetMarker = 114`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L527) | 当前常量或枚举值：`114`。 | — |
| <a id="api-7b151ea647630f9e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetScissor = 118`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L531) | 当前常量或枚举值：`118`。 | — |
| <a id="api-7eb6dc68ac2e5e61"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetScissorCached = 119`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L532) | 当前常量或枚举值：`119`。 | — |
| <a id="api-83949a8392834e56"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetState = 115`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L528) | 当前常量或枚举值：`115`。 | — |
| <a id="api-a824258758a052d1"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetStencil = 117`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L530) | 当前常量或枚举值：`117`。 | — |
| <a id="api-7ea79ddb2b509922"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTexture = 140`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L553) | 当前常量或枚举值：`140`。 | — |
| <a id="api-df711de2e60a1d3e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransform = 120`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L533) | 当前常量或枚举值：`120`。 | — |
| <a id="api-b230614d454ef471"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransformCached = 121`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L534) | 当前常量或枚举值：`121`。 | — |
| <a id="api-77300260d257b46c"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientIndexBuffer = 128`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L541) | 当前常量或枚举值：`128`。 | — |
| <a id="api-af5fa62a951ea327"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientVertexBuffer = 133`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L546) | 当前常量或枚举值：`133`。 | — |
| <a id="api-744ecb59bf36dc6f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetTransientVertexBufferWithLayout = 134`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L547) | 当前常量或枚举值：`134`。 | — |
| <a id="api-dbdeb64aeae63635"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetUniform = 123`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L536) | 当前常量或枚举值：`123`。 | — |
| <a id="api-4490644ef94ca750"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexBuffer = 129`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L542) | 当前常量或枚举值：`129`。 | — |
| <a id="api-da79919fe8e389a7"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexBufferWithLayout = 130`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L543) | 当前常量或枚举值：`130`。 | — |
| <a id="api-6cbf73d7c97381ef"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSetVertexCount = 135`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L548) | 当前常量或枚举值：`135`。 | — |
| <a id="api-5e2967fbb80ec47b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmit = 142`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L555) | 当前常量或枚举值：`142`。 | — |
| <a id="api-3f4673865046f4fe"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitIndirect = 144`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L557) | 当前常量或枚举值：`144`。 | — |
| <a id="api-a442c6f19f628d82"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitIndirectCount = 145`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L558) | 当前常量或枚举值：`145`。 | — |
| <a id="api-bd699dffd96d210b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderSubmitOcclusionQuery = 143`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L556) | 当前常量或枚举值：`143`。 | — |
| <a id="api-f8d0e137480340a2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.EncoderTouch = 141`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L554) | 当前常量或枚举值：`141`。 | — |
| <a id="api-219c90dd8a9e665b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Frame = 22`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L435) | 当前常量或枚举值：`22`。 | — |
| <a id="api-0fa144662ec87291"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetAvailInstanceDataBuffer = 53`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L466) | 当前常量或枚举值：`53`。 | — |
| <a id="api-f1b2347f877369d6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetAvailTransientIndexBuffer = 51`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L464) | 当前常量或枚举值：`51`。 | — |
| <a id="api-37596f0bccef83cd"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetAvailTransientVertexBuffer = 52`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L465) | 当前常量或枚举值：`52`。 | — |
| <a id="api-5ed8d7007cb040b4"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetCaps = 24`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L437) | 当前常量或枚举值：`24`。 | — |
| <a id="api-0331963a4cc501c2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetDirectAccessPtr = 80`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L493) | 当前常量或枚举值：`80`。 | — |
| <a id="api-e50eff5f98f100a2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetInternalData = 159`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L572) | 当前常量或枚举值：`159`。 | — |
| <a id="api-e9d8644167d62655"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetRendererName = 17`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L430) | 当前常量或枚举值：`17`。 | — |
| <a id="api-a334b4fd24fadd3b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetRendererType = 23`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L436) | 当前常量或枚举值：`23`。 | — |
| <a id="api-56d3524161975d83"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetResult = 95`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L508) | 当前常量或枚举值：`95`。 | — |
| <a id="api-cb5f068742771e62"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetShaderUniforms = 61`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L474) | 当前常量或枚举值：`61`。 | — |
| <a id="api-6c426f93ba8a59b1"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetStats = 25`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L438) | 当前常量或枚举值：`25`。 | — |
| <a id="api-5d6091c2743280f1"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetSupportedRenderers = 16`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L429) | 当前常量或枚举值：`16`。 | — |
| <a id="api-80f97ddd94b88472"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetTexture = 88`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L501) | 当前常量或枚举值：`88`。 | — |
| <a id="api-2420b5b51305f3fa"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.GetUniformInfo = 92`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L505) | 当前常量或枚举值：`92`。 | — |
| <a id="api-81209b1c071d2aa7"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Init = 19`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L432) | 当前常量或枚举值：`19`。 | — |
| <a id="api-1f0d4ea55c3e7716"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.InitCtor = 18`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L431) | 当前常量或枚举值：`18`。 | — |
| <a id="api-e93f8a21c7547a4b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.IsFrameBufferValid = 68`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L481) | 当前常量或枚举值：`68`。 | — |
| <a id="api-2e1d7ed7a7450cda"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.IsTextureValid = 67`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L480) | 当前常量或枚举值：`67`。 | — |
| <a id="api-7d84423f94602183"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.MakeRef = 28`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L441) | 当前常量或枚举值：`28`。 | — |
| <a id="api-262371678afd4d84"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.MakeRefRelease = 29`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L442) | 当前常量或枚举值：`29`。 | — |
| <a id="api-fa8afd6f88d7c6bc"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.OverrideInternalTexture = 161`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L574) | 当前常量或枚举值：`161`。 | — |
| <a id="api-012f8234d7ad82d2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.OverrideInternalTexturePtr = 160`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L573) | 当前常量或枚举值：`160`。 | — |
| <a id="api-c1f92897042ea64e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.ReadTexture = 78`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L491) | 当前常量或枚举值：`78`。 | — |
| <a id="api-1556e8a28c320faa"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.RenderFrame = 157`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L570) | 当前常量或枚举值：`157`。 | — |
| <a id="api-0831b474e73a2601"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.RequestScreenShot = 156`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L569) | 当前常量或枚举值：`156`。 | — |
| <a id="api-3792a6514dd32b86"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Reset = 21`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L434) | 当前常量或枚举值：`21`。 | — |
| <a id="api-0a4190fda702185a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.ResetView = 111`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L524) | 当前常量或枚举值：`111`。 | — |
| <a id="api-1118fd9eef9d9c57"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeDynamicIndexBuffer = 194`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L607) | 当前常量或枚举值：`194`。 | — |
| <a id="api-53169b881589ae6a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeDynamicVertexBuffer = 195`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L608) | 当前常量或枚举值：`195`。 | — |
| <a id="api-ed38c5b76e5563b5"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeIndexBuffer = 192`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L605) | 当前常量或枚举值：`192`。 | — |
| <a id="api-7c52b4218158594b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeIndirectBuffer = 196`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L609) | 当前常量或枚举值：`196`。 | — |
| <a id="api-658d89676ba9d858"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetComputeVertexBuffer = 193`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L606) | 当前常量或枚举值：`193`。 | — |
| <a id="api-eef3c277c746764f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetCondition = 164`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L577) | 当前常量或枚举值：`164`。 | — |
| <a id="api-6e8c62ac326ba8b6"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetDebug = 30`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L443) | 当前常量或枚举值：`30`。 | — |
| <a id="api-88cde696c2e453a0"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetDynamicIndexBuffer = 173`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L586) | 当前常量或枚举值：`173`。 | — |
| <a id="api-d29f969bb8104089"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetDynamicVertexBuffer = 177`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L590) | 当前常量或枚举值：`177`。 | — |
| <a id="api-a1f477396d0c06d8"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetDynamicVertexBufferWithLayout = 178`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L591) | 当前常量或枚举值：`178`。 | — |
| <a id="api-1f4764531827b0d3"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetFrameBufferName = 87`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L500) | 当前常量或枚举值：`87`。 | — |
| <a id="api-7c5d29d2019d06eb"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetFrameUniform = 125`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L538) | 当前常量或枚举值：`125`。 | — |
| <a id="api-c8a21a16346bb2bb"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetImage = 197`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L610) | 当前常量或枚举值：`197`。 | — |
| <a id="api-1c51c8013fb3b35d"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetIndexBuffer = 172`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L585) | 当前常量或枚举值：`172`。 | — |
| <a id="api-ef7a2d5c7f493322"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetIndexBufferName = 36`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L449) | 当前常量或枚举值：`36`。 | — |
| <a id="api-1b3294bd31cad15e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetInstanceCount = 185`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L598) | 当前常量或枚举值：`185`。 | — |
| <a id="api-90836fb9f3933651"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetInstanceDataBuffer = 182`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L595) | 当前常量或枚举值：`182`。 | — |
| <a id="api-a3c3d3ce9dd5c7ae"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetInstanceDataFromDynamicVertexBuffer = 184`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L597) | 当前常量或枚举值：`184`。 | — |
| <a id="api-790cdf0bdedd3705"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetInstanceDataFromVertexBuffer = 183`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L596) | 当前常量或枚举值：`183`。 | — |
| <a id="api-85378458f74fcc4a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetMarker = 162`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L575) | 当前常量或枚举值：`162`。 | — |
| <a id="api-da2734039b4fa551"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetPaletteColor = 97`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L510) | 当前常量或枚举值：`97`。 | — |
| <a id="api-464da20f1fdc7a77"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetPaletteColorRgba32F = 98`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L511) | 当前常量或枚举值：`98`。 | — |
| <a id="api-c0b4cfa24817e1b1"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetPaletteColorRgba8 = 99`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L512) | 当前常量或枚举值：`99`。 | — |
| <a id="api-de6b71a7a02481ab"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetPlatformData = 158`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L571) | 当前常量或枚举值：`158`。 | — |
| <a id="api-5124012ed76aef2a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetScissor = 166`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L579) | 当前常量或枚举值：`166`。 | — |
| <a id="api-4a2450eaba30a415"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetScissorCached = 167`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L580) | 当前常量或枚举值：`167`。 | — |
| <a id="api-1c520b55b90ce365"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetShaderName = 62`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L475) | 当前常量或枚举值：`62`。 | — |
| <a id="api-0bf8f7a50cdaa7b0"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetState = 163`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L576) | 当前常量或枚举值：`163`。 | — |
| <a id="api-ca027328bae24658"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetStencil = 165`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L578) | 当前常量或枚举值：`165`。 | — |
| <a id="api-01bd12533d0a0431"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTexture = 186`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L599) | 当前常量或枚举值：`186`。 | — |
| <a id="api-f65962b4c262a9bf"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTextureName = 79`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L492) | 当前常量或枚举值：`79`。 | — |
| <a id="api-38b337ae07dc15c8"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTransform = 168`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L581) | 当前常量或枚举值：`168`。 | — |
| <a id="api-d922513373e51c5e"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTransformCached = 169`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L582) | 当前常量或枚举值：`169`。 | — |
| <a id="api-888cb763a2b98a4f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTransientIndexBuffer = 174`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L587) | 当前常量或枚举值：`174`。 | — |
| <a id="api-75824264f5fc0636"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTransientVertexBuffer = 179`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L592) | 当前常量或枚举值：`179`。 | — |
| <a id="api-eaf838260327a169"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetTransientVertexBufferWithLayout = 180`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L593) | 当前常量或枚举值：`180`。 | — |
| <a id="api-7ef6d59d9f62d24b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetUniform = 171`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L584) | 当前常量或枚举值：`171`。 | — |
| <a id="api-0a300170d815f8d1"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetVertexBuffer = 175`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L588) | 当前常量或枚举值：`175`。 | — |
| <a id="api-fa227336d7d7d031"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetVertexBufferName = 41`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L454) | 当前常量或枚举值：`41`。 | — |
| <a id="api-9d8ad661efdea099"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetVertexBufferWithLayout = 176`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L589) | 当前常量或枚举值：`176`。 | — |
| <a id="api-eece679bcc30c36d"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetVertexCount = 181`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L594) | 当前常量或枚举值：`181`。 | — |
| <a id="api-80a8c187c29e75b3"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewClear = 104`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L517) | 当前常量或枚举值：`104`。 | — |
| <a id="api-0e6588e56cdf95e3"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewClearMrt = 105`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L518) | 当前常量或枚举值：`105`。 | — |
| <a id="api-5f145ee612f659db"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewFrameBuffer = 107`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L520) | 当前常量或枚举值：`107`。 | — |
| <a id="api-bb54acee91e4e59a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewMode = 106`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L519) | 当前常量或枚举值：`106`。 | — |
| <a id="api-e6f96cccf7249700"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewName = 100`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L513) | 当前常量或枚举值：`100`。 | — |
| <a id="api-e2293b9364931688"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewOrder = 109`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L522) | 当前常量或枚举值：`109`。 | — |
| <a id="api-660c40d80993549a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewRect = 101`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L514) | 当前常量或枚举值：`101`。 | — |
| <a id="api-07b32971c6115753"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewRectRatio = 102`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L515) | 当前常量或枚举值：`102`。 | — |
| <a id="api-61a1866ed6c0eafb"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewScissor = 103`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L516) | 当前常量或枚举值：`103`。 | — |
| <a id="api-b8c6310e03427e7f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewShadingRate = 110`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L523) | 当前常量或枚举值：`110`。 | — |
| <a id="api-e43d4b121851d24f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewTransform = 108`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L521) | 当前常量或枚举值：`108`。 | — |
| <a id="api-a82732e0b57394bd"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SetViewUniform = 124`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L537) | 当前常量或枚举值：`124`。 | — |
| <a id="api-06383e54f82e5248"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Shutdown = 20`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L433) | 当前常量或枚举值：`20`。 | — |
| <a id="api-ded6a79dcefe6394"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Submit = 188`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L601) | 当前常量或枚举值：`188`。 | — |
| <a id="api-436320ab1fda6c8a"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SubmitIndirect = 190`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L603) | 当前常量或枚举值：`190`。 | — |
| <a id="api-ab10afa5ddc5b351"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SubmitIndirectCount = 191`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L604) | 当前常量或枚举值：`191`。 | — |
| <a id="api-1c9aa9eff538352c"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.SubmitOcclusionQuery = 189`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L602) | 当前常量或枚举值：`189`。 | — |
| <a id="api-cbe38f953a614c87"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.TopologyConvert = 14`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L427) | 当前常量或枚举值：`14`。 | — |
| <a id="api-288b408ced25b261"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.TopologySortTriList = 15`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L428) | 当前常量或枚举值：`15`。 | — |
| <a id="api-070a096e0c8a1ec0"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.Touch = 187`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L600) | 当前常量或枚举值：`187`。 | — |
| <a id="api-746f993da1a37217"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.UpdateDynamicIndexBuffer = 45`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L458) | 当前常量或枚举值：`45`。 | — |
| <a id="api-5c9c5be492e3e825"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.UpdateDynamicVertexBuffer = 49`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L462) | 当前常量或枚举值：`49`。 | — |
| <a id="api-8ca2bc9a14edee0d"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.UpdateTexture2D = 75`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L488) | 当前常量或枚举值：`75`。 | — |
| <a id="api-fc4a872113294371"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.UpdateTexture3D = 76`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L489) | 当前常量或枚举值：`76`。 | — |
| <a id="api-ccd39435be05a829"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.UpdateTextureCube = 77`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L490) | 当前常量或枚举值：`77`。 | — |
| <a id="api-39decbaed222b641"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexConvert = 12`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L425) | 当前常量或枚举值：`12`。 | — |
| <a id="api-0d5c469773f95849"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutAdd = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L415) | 当前常量或枚举值：`2`。 | — |
| <a id="api-5b17a763e9193c7f"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutBegin = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L414) | 当前常量或枚举值：`1`。 | — |
| <a id="api-3290fb29e300e837"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutDecode = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L416) | 当前常量或枚举值：`3`。 | — |
| <a id="api-10a8e4b4437acc92"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutEnd = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L419) | 当前常量或枚举值：`6`。 | — |
| <a id="api-f423df1951189609"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutGetOffset = 7`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L420) | 当前常量或枚举值：`7`。 | — |
| <a id="api-11baf07b0373429b"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutGetSize = 9`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L422) | 当前常量或枚举值：`9`。 | — |
| <a id="api-0a90ff39579a7166"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutGetStride = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L421) | 当前常量或枚举值：`8`。 | — |
| <a id="api-3e37befe658287e2"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutHas = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L417) | 当前常量或枚举值：`4`。 | — |
| <a id="api-76f8645c2ccfd28c"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexLayoutSkip = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L418) | 当前常量或枚举值：`5`。 | — |
| <a id="api-0d6acecfdd1739cd"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexPack = 10`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L423) | 当前常量或枚举值：`10`。 | — |
| <a id="api-aa4a0b487525f41d"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.VertexUnpack = 11`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L424) | 当前常量或枚举值：`11`。 | — |
| <a id="api-a75605a761a32659"></a>[`Inno.Native.Bgfx.bgfx.FunctionId.WeldVertices = 13`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L426) | 当前常量或枚举值：`13`。 | — |
| <a id="api-a8b1c47523c4a54a"></a>[`public enum Inno.Native.Bgfx.bgfx.FunctionId : int`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L411) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.IndexBufferHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.IndexBufferHandle c3e33a081d5cf3c18ad683c60787085a6d1ac1f3985487b6ae55c87f0e73ecc5 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.IndexBufferHandle.idx 440f6f92422dcf709dcb952ee2874e9411e732992410b30aed1b9b4afaf6abec -->
<!-- api: P:Inno.Native.Bgfx.bgfx.IndexBufferHandle.Valid 5cc01bda6095bc00aafec850318dd894052090be2cec90edbab9c96ea608708e -->
<!-- api: M:Inno.Native.Bgfx.bgfx.IndexBufferHandle.#ctor(System.UInt16) eab9d2838fb422589641b6e820935f590ce782423e897f9102f2a9ee50d1df90 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-603eab0ed1fa46f8"></a>[`public ushort Inno.Native.Bgfx.bgfx.IndexBufferHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1997) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01f3e20f2406d2be"></a>[`public Inno.Native.Bgfx.bgfx.IndexBufferHandle.IndexBufferHandle(ushort idx = 0)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2000) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d93ab701920ae97c"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.IndexBufferHandle.Valid { get; }`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1998) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a3cc4004437eb6db"></a>[`public struct Inno.Native.Bgfx.bgfx.IndexBufferHandle`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1995) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.IndirectBufferHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.IndirectBufferHandle 6cb5fce22bfad50e76586fd0a92c9b9e10bbc5bc68936be598cd019d6f3f0310 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.IndirectBufferHandle.idx 508964712f07c3c666ee4903b9421c0911af444cf62fafacee1bd75b275e17d8 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.IndirectBufferHandle.Valid 14de3fc38aed5e551c172bde0a5776b1b67fa30a960b02b0b3e8017794139899 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.IndirectBufferHandle.#ctor(System.UInt16) d6845fbdb942594f6be4452d54f3bec4f68be8a0fcf7524d91b45dfd107f669f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-35973d0af3d20a2b"></a>[`public ushort Inno.Native.Bgfx.bgfx.IndirectBufferHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2163) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-67a729f8d3be6d2b"></a>[`public Inno.Native.Bgfx.bgfx.IndirectBufferHandle.IndirectBufferHandle(ushort idx = 0)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2166) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-32f6878a66e38162"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.IndirectBufferHandle.Valid { get; }`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2164) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c560030cfd307da9"></a>[`public struct Inno.Native.Bgfx.bgfx.IndirectBufferHandle`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2161) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-dd3dacb1c322891a"></a>[`public Inno.Native.Bgfx.bgfx.AllocatorInterface* Inno.Native.Bgfx.bgfx.Init.allocator`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L951) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c64fad071aeeeb98"></a>[`public Inno.Native.Bgfx.bgfx.CallbackInterface* Inno.Native.Bgfx.bgfx.Init.callback`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L950) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7d31d607799bb3d5"></a>[`public ulong Inno.Native.Bgfx.bgfx.Init.capabilities`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L943) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ebaa67df12b37ddc"></a>[`public byte Inno.Native.Bgfx.bgfx.Init.debug`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L944) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b99b56425c4feba0"></a>[`public ushort Inno.Native.Bgfx.bgfx.Init.deviceId`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L942) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-849b428e7b035f83"></a>[`public byte Inno.Native.Bgfx.bgfx.Init.fallback`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L946) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39e03e44e9685db1"></a>[`public Inno.Native.Bgfx.bgfx.InitLimits Inno.Native.Bgfx.bgfx.Init.limits`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L949) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b0e30343f00255a"></a>[`public Inno.Native.Bgfx.bgfx.PlatformData Inno.Native.Bgfx.bgfx.Init.platformData`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L947) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9af81f0e727658cb"></a>[`public byte Inno.Native.Bgfx.bgfx.Init.profile`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L945) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9ad49dbc3d7b351"></a>[`public Inno.Native.Bgfx.bgfx.Resolution Inno.Native.Bgfx.bgfx.Init.resolution`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L948) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fa47bc0d47cbeec3"></a>[`public Inno.Native.Bgfx.bgfx.RendererType Inno.Native.Bgfx.bgfx.Init.type`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L940) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9a67b55da7812be7"></a>[`public ushort Inno.Native.Bgfx.bgfx.Init.vendorId`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L941) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a1e7e84304b029c6"></a>[`public Inno.Native.Bgfx.bgfx.Init.Init(Inno.Native.Bgfx.bgfx.RendererType type = Inno.Native.Bgfx.bgfx.RendererType.Noop, ushort vendorId = 0, ushort deviceId = 0, ulong capabilities = 0, byte debug = 0, byte profile = 0, byte fallback = 0, Inno.Native.Bgfx.bgfx.PlatformData platformData = default(Inno.Native.Bgfx.bgfx.PlatformData), Inno.Native.Bgfx.bgfx.Resolution resolution = default(Inno.Native.Bgfx.bgfx.Resolution), Inno.Native.Bgfx.bgfx.InitLimits limits = default(Inno.Native.Bgfx.bgfx.InitLimits), Inno.Native.Bgfx.bgfx.CallbackInterface* callback = null, Inno.Native.Bgfx.bgfx.AllocatorInterface* allocator = null)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L952) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-632093c41c40396e"></a>[`public struct Inno.Native.Bgfx.bgfx.Init`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L938) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-d709cd870c174ae1"></a>[`public ushort Inno.Native.Bgfx.bgfx.InitLimits.maxEncoders`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L862) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7bd09dfc5a8e7609"></a>[`public uint Inno.Native.Bgfx.bgfx.InitLimits.maxTransientIbSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L865) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f4b43fd3755d184"></a>[`public uint Inno.Native.Bgfx.bgfx.InitLimits.maxTransientVbSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L864) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-04db8614f4c5f4b7"></a>[`public uint Inno.Native.Bgfx.bgfx.InitLimits.minResourceCbSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L863) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-92e69abfdfe586f0"></a>[`public uint Inno.Native.Bgfx.bgfx.InitLimits.minUniformBufferSize`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L866) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1606eb7990fb19af"></a>[`public Inno.Native.Bgfx.bgfx.InitLimits.InitLimits(ushort maxEncoders = 0, uint minResourceCbSize = 0, uint maxTransientVbSize = 0, uint maxTransientIbSize = 0, uint minUniformBufferSize = 0)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L867) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1f55c8d419b749bb"></a>[`public struct Inno.Native.Bgfx.bgfx.InitLimits`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L860) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-ff930d619fc89e67"></a>[`public byte* Inno.Native.Bgfx.bgfx.InstanceDataBuffer.data`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2137) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-22f6284562072f56"></a>[`public Inno.Native.Bgfx.bgfx.VertexBufferHandle Inno.Native.Bgfx.bgfx.InstanceDataBuffer.handle`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2142) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d550c33cf98efca9"></a>[`public uint Inno.Native.Bgfx.bgfx.InstanceDataBuffer.num`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2140) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5b76783d90bac44b"></a>[`public uint Inno.Native.Bgfx.bgfx.InstanceDataBuffer.offset`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2139) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61cf0ea6e6d021f0"></a>[`public uint Inno.Native.Bgfx.bgfx.InstanceDataBuffer.size`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2138) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-afff9a9e3196f5c6"></a>[`public ushort Inno.Native.Bgfx.bgfx.InstanceDataBuffer.stride`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2141) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed4a62e89f1ec604"></a>[`public Inno.Native.Bgfx.bgfx.InstanceDataBuffer.InstanceDataBuffer(byte* data = null, uint size = 0, uint offset = 0, uint num = 0, ushort stride = 0, Inno.Native.Bgfx.bgfx.VertexBufferHandle handle = default(Inno.Native.Bgfx.bgfx.VertexBufferHandle))`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2143) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0ede8a296b59edff"></a>[`public struct Inno.Native.Bgfx.bgfx.InstanceDataBuffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2135) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-881c6f26cc72e1eb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2928) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58f1ac2ca82d8b65"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_instance_data_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2959) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-789126292a15b0e0"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_transform`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3072) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64ad13127c7ca4fb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_transient_buffers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2958) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e6249f16bd13757"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_transient_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2956) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-52e0f8343df2b641"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.alloc_transient_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2957) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-434dab6fc839255d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.attachment_init`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2902) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e038b8771ef7ad49"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.blit`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3103) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-70a0c96470117853"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.calc_texture_size`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2971) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8504e54ff090169a"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.copy`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2929) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e8686ca8bed6ade6"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_compute_program`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2967) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f87f9b73c4023d06"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_dynamic_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2945) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8faf2f1cf6336552"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_dynamic_index_buffer_mem`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2946) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-973bcb655e0c0329"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_dynamic_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2949) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8cb8bf652587e2dc"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_dynamic_vertex_buffer_mem`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2950) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6173183e3c49f1f5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2984) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2fb4e990ba613058"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer_from_attachment`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2987) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c00b1f947ff5ede"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer_from_handles`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2986) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f66716f38ced76f1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer_from_nwh`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2988) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d291597b61fc324c"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_frame_buffer_scaled`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2985) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b11a080ef297838"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2937) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba62dfed169bd832"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_indirect_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2960) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5342c5638f5a7fb0"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_occlusion_query`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2996) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-96913219ded3e82b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_program`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2966) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7406ea56d1ea9ae"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_shader`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2962) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a23de77e8878367f"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2972) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9aebb1744a38b11"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture_2d`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2973) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-520ebaffc0ce2024"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture_2d_scaled`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2974) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a8edc65b2382c18f"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture_3d`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2975) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e550ef9e85546fd"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_texture_cube`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2976) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a620c9f62db6a6f8"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_uniform`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2992) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae4d61d7bb96f639"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_uniform_with_freq`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2993) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f8455c233f364a18"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2942) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58762a2b2e52e4e7"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.create_vertex_layout`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2940) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f175a1238f09361"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.dbg_text_clear`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2933) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-421729a56a26c1c2"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.dbg_text_image`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2936) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b170887f14632a5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.dbg_text_printf`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2934) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-442e982dddb3988d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.dbg_text_vprintf`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2935) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-049bc815fb7310ad"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_dynamic_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2948) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-00e83b078ad411f1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_dynamic_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2952) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b30ba1781278048"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_frame_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2991) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d5519d8175e9dd8"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2939) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0231cf231e2272af"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_indirect_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2961) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5a123d67bf03dc8"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_occlusion_query`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2998) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f83ee5ef29168cd9"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_program`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2968) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-776b77dc5e19bedb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_shader`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2965) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-406b81c518245a96"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_texture`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2983) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-50990446025978b2"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_uniform`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2995) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01ffd0e01f86f583"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2944) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-79cc79d80b26f2a5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.destroy_vertex_layout`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2941) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7e7642431d2e46e6"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.discard`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3102) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb7b5fdae5cfa050"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.dispatch`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3100) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-616ccec11a841c7e"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.dispatch_indirect`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3101) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b6f70a92a6437f37"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_alloc_transform`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3024) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d7d7705bc1c944cb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_begin`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3014) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a46fdf260b93ba2"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_blit`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3057) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-50a5292e993953da"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_discard`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3056) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5b756b1b231e269b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_dispatch`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3054) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4b54330dd722eb3"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_dispatch_indirect`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3055) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3fc14de414c337ff"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_end`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3015) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-57c61a49b799c3ec"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_dynamic_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3050) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d705532a5fcf6bc"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_dynamic_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3051) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e0091b3f4a0d66c"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3048) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-55887db067644230"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_indirect_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3052) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a77cf57d7e7eb8bb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_compute_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3049) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a56c4f5c656b31a1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_condition`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3018) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b357dd68c8309be"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_dynamic_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3029) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ebd1c22712ed87aa"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_dynamic_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3033) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-06903d3681c68269"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_dynamic_vertex_buffer_with_layout`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3034) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e29ab0c93a48e8a"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_image`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3053) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf7f18ba407962d9"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3028) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b017ffefa6a6dcd3"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_instance_count`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3041) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e8b2853bb175b42"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_instance_data_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3038) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-327a74b4e0fcc730"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_instance_data_from_dynamic_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3040) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85ab5d113dbd6482"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_instance_data_from_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3039) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-552c3e0cb41710b5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_marker`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3016) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-427316c28085176d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_scissor`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3020) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3531d44a55adbe12"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_scissor_cached`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3021) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9298a248116887cb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_state`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3017) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-504457343ce31b04"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_stencil`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3019) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-41e767a44c032a53"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_texture`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3042) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f95c7c1db973cee"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transform`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3022) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da602210e18c6138"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transform_cached`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3023) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c03a1825e742744b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transient_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3030) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69395c3615aca71b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transient_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3035) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-30b44c26ed8c65f8"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_transient_vertex_buffer_with_layout`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3036) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-04001b9df3dbf3cc"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_uniform`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3025) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-50144fd2154c3f57"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3031) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c591524cf38ecea"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_vertex_buffer_with_layout`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3032) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-67441a0958eccc84"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_set_vertex_count`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3037) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-898725393bc88f5b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_submit`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3044) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7c2375d7d85816e4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_submit_indirect`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3046) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c17ec3f482d4fdce"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_submit_indirect_count`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3047) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4851ad9f86e2dc79"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_submit_occlusion_query`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3045) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df815ab5885df03e"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.encoder_touch`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3043) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d8c0105f7f667d54"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.frame`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2924) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c65835caca517de"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_avail_instance_data_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2955) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-37cefe8670ee513a"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_avail_transient_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2953) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20579853e9a8e93d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_avail_transient_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2954) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-845ae89789b64c28"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_caps`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2926) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53dec3120fe355cd"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_direct_access_ptr`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2982) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5fa16bd691d0aa6"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_internal_data`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3061) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-277d311cf1bc1dff"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_renderer_name`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2919) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-062995210c629b7b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_renderer_type`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2925) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27a2ec85c696ee35"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_result`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2997) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cdcd6b313c40ccfe"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_shader_uniforms`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2963) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-180f752a3a1e0980"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_stats`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2927) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f7642dd2570fec58"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_supported_renderers`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2918) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f8fef024df82db3"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_texture`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2990) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a7374b2b9da8775"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.get_uniform_info`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2994) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ebc69e82de4b5f5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.init`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2921) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-77497b6d42434e48"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.init_ctor`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2920) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c3b9c329cbb3eea5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.is_frame_buffer_valid`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2970) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f00fe0c9e383872"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.is_texture_valid`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2969) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f96d94c6177a339"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.make_ref`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2930) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f51811918ae8e867"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.make_ref_release`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2931) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed338450f9448eac"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.override_internal_texture`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3063) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d32ede7f357ad2b6"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.override_internal_texture_ptr`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3062) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b8e50e37bd2e2e12"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.read_texture`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2980) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f2f3302867c0f62"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.render_frame`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3059) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11276a3d6addcc03"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.request_screen_shot`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3058) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-269e5c0b3f7fff61"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.reset`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2923) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-45c08ee8110af0ce"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.reset_view`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3013) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b472521fa5e7076"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_dynamic_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3096) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d881b1e0b15d4a71"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_dynamic_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3097) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb9960ca51149435"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3094) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5b75577cfcca277"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_indirect_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3098) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0b436b066a2e4a18"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_compute_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3095) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a4de35efc5e3568"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_condition`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3066) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb2f364c104d7496"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_debug`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2932) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2348dca51b797a6d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_dynamic_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3075) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cdfce4c31dc8cfda"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_dynamic_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3079) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-83492b897ebf5868"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_dynamic_vertex_buffer_with_layout`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3080) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6086c7c6f353839"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_frame_buffer_name`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2989) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-42f2956697d850e1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_frame_uniform`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3027) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e1a4233b943d396"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_image`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3099) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a50932e7e93a76ec"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3074) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-05432a0f227969c7"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_index_buffer_name`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2938) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3853506c3a4d179d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_instance_count`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3087) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2bba29719d77ce2e"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_instance_data_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3084) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc5986c7106d4e0a"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_instance_data_from_dynamic_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3086) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0329319b114a403a"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_instance_data_from_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3085) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a425767d994f9675"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_marker`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3064) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7998e668006c8c6b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_palette_color`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2999) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d62afd19b7655048"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_palette_color_rgba32f`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3000) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7027e94163396393"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_palette_color_rgba8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3001) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-135093968574f4c8"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_platform_data`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3060) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-024c4facbde7e27f"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_scissor`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3068) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f5c72fcf1d6f70b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_scissor_cached`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3069) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-187e827abd5c0dd6"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_shader_name`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2964) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e79ed7b5f89f5cc7"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_state`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3065) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-66e01242dee3b2f4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_stencil`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3067) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfa201d3c19bcaab"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_texture`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3088) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-307decabe5cb1889"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_texture_name`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2981) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2db309948d7836b4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transform`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3070) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3f7da7783b5e2529"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transform_cached`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3071) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53aeecdda9f1ded6"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transient_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3076) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf863f3e44307ccb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transient_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3081) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bdeb741049eeb49b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_transient_vertex_buffer_with_layout`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3082) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07ae58a61018caf3"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_uniform`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3073) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-efbe8db61e8356e7"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3077) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8f08923e080c01e1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_vertex_buffer_name`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2943) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f749252a9bb1734"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_vertex_buffer_with_layout`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3078) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea65116e5d183c0d"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_vertex_count`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3083) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-430884746d71c699"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_clear`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3006) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c999702df82368ee"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_clear_mrt`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3007) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1ae444a57dd38e7c"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_frame_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3009) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9836eb16dcf4b5bd"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_mode`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3008) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2bb6f310d728d460"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_name`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3002) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7c828b6af85507c4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_order`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3011) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-92b45c70c2e10d69"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_rect`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3003) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae57395a1633040f"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_rect_ratio`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3004) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ebb06b099b9575f2"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_scissor`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3005) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3c82283425b948da"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_shading_rate`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3012) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b4ead3b935434133"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_transform`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3010) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b2bf807b39b4c897"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.set_view_uniform`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3026) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-83d859bc091531f1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.shutdown`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2922) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a58fb85fe97647ec"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.submit`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3090) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab0f294696d51a9b"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.submit_indirect`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3092) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-02c6b7c169f999a3"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.submit_indirect_count`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3093) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d8d7a63dec6a83cc"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.submit_occlusion_query`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3091) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-425b0d8334b1cfa2"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.topology_convert`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2916) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e7f2bbbeb9a6de5"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.topology_sort_tri_list`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2917) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6663a631918ad948"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.touch`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3089) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5139737cfb6198e1"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_dynamic_index_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2947) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0645f287acb5c5cb"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_dynamic_vertex_buffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2951) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11c166f36960b391"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_texture_2d`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2977) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a99544e3d27f3259"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_texture_3d`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2978) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-98ad0194c5752123"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.update_texture_cube`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2979) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ff9f6eb75212c7e4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_convert`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2914) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58312c87f647e67a"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_add`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2904) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-16e06d0176d40a38"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_begin`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2903) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1db449252223287e"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_decode`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2905) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6de59338c15e60f"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_end`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2908) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-467cefaf193b8dc4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_get_offset`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2909) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c5d709ed56cbbdc2"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_get_size`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2911) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-65830f61d31587e4"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_get_stride`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2910) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c2dae36b754dca69"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_has`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2906) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f8abb08eac804685"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_layout_skip`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2907) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-12bc6a37f4e78faf"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_pack`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2912) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f3f313969a0bb88"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.vertex_unpack`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2913) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9fafcd350f6ce550"></a>[`public void* Inno.Native.Bgfx.bgfx.InterfaceVtbl.weld_vertices`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2915) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-395ed68db05a6780"></a>[`public Inno.Native.Bgfx.bgfx.InterfaceVtbl.InterfaceVtbl(void* attachment_init = null, void* vertex_layout_begin = null, void* vertex_layout_add = null, void* vertex_layout_decode = null, void* vertex_layout_has = null, void* vertex_layout_skip = null, void* vertex_layout_end = null, void* vertex_layout_get_offset = null, void* vertex_layout_get_stride = null, void* vertex_layout_get_size = null, void* vertex_pack = null, void* vertex_unpack = null, void* vertex_convert = null, void* weld_vertices = null, void* topology_convert = null, void* topology_sort_tri_list = null, void* get_supported_renderers = null, void* get_renderer_name = null, void* init_ctor = null, void* init = null, void* shutdown = null, void* reset = null, void* frame = null, void* get_renderer_type = null, void* get_caps = null, void* get_stats = null, void* alloc = null, void* copy = null, void* make_ref = null, void* make_ref_release = null, void* set_debug = null, void* dbg_text_clear = null, void* dbg_text_printf = null, void* dbg_text_vprintf = null, void* dbg_text_image = null, void* create_index_buffer = null, void* set_index_buffer_name = null, void* destroy_index_buffer = null, void* create_vertex_layout = null, void* destroy_vertex_layout = null, void* create_vertex_buffer = null, void* set_vertex_buffer_name = null, void* destroy_vertex_buffer = null, void* create_dynamic_index_buffer = null, void* create_dynamic_index_buffer_mem = null, void* update_dynamic_index_buffer = null, void* destroy_dynamic_index_buffer = null, void* create_dynamic_vertex_buffer = null, void* create_dynamic_vertex_buffer_mem = null, void* update_dynamic_vertex_buffer = null, void* destroy_dynamic_vertex_buffer = null, void* get_avail_transient_index_buffer = null, void* get_avail_transient_vertex_buffer = null, void* get_avail_instance_data_buffer = null, void* alloc_transient_index_buffer = null, void* alloc_transient_vertex_buffer = null, void* alloc_transient_buffers = null, void* alloc_instance_data_buffer = null, void* create_indirect_buffer = null, void* destroy_indirect_buffer = null, void* create_shader = null, void* get_shader_uniforms = null, void* set_shader_name = null, void* destroy_shader = null, void* create_program = null, void* create_compute_program = null, void* destroy_program = null, void* is_texture_valid = null, void* is_frame_buffer_valid = null, void* calc_texture_size = null, void* create_texture = null, void* create_texture_2d = null, void* create_texture_2d_scaled = null, void* create_texture_3d = null, void* create_texture_cube = null, void* update_texture_2d = null, void* update_texture_3d = null, void* update_texture_cube = null, void* read_texture = null, void* set_texture_name = null, void* get_direct_access_ptr = null, void* destroy_texture = null, void* create_frame_buffer = null, void* create_frame_buffer_scaled = null, void* create_frame_buffer_from_handles = null, void* create_frame_buffer_from_attachment = null, void* create_frame_buffer_from_nwh = null, void* set_frame_buffer_name = null, void* get_texture = null, void* destroy_frame_buffer = null, void* create_uniform = null, void* create_uniform_with_freq = null, void* get_uniform_info = null, void* destroy_uniform = null, void* create_occlusion_query = null, void* get_result = null, void* destroy_occlusion_query = null, void* set_palette_color = null, void* set_palette_color_rgba32f = null, void* set_palette_color_rgba8 = null, void* set_view_name = null, void* set_view_rect = null, void* set_view_rect_ratio = null, void* set_view_scissor = null, void* set_view_clear = null, void* set_view_clear_mrt = null, void* set_view_mode = null, void* set_view_frame_buffer = null, void* set_view_transform = null, void* set_view_order = null, void* set_view_shading_rate = null, void* reset_view = null, void* encoder_begin = null, void* encoder_end = null, void* encoder_set_marker = null, void* encoder_set_state = null, void* encoder_set_condition = null, void* encoder_set_stencil = null, void* encoder_set_scissor = null, void* encoder_set_scissor_cached = null, void* encoder_set_transform = null, void* encoder_set_transform_cached = null, void* encoder_alloc_transform = null, void* encoder_set_uniform = null, void* set_view_uniform = null, void* set_frame_uniform = null, void* encoder_set_index_buffer = null, void* encoder_set_dynamic_index_buffer = null, void* encoder_set_transient_index_buffer = null, void* encoder_set_vertex_buffer = null, void* encoder_set_vertex_buffer_with_layout = null, void* encoder_set_dynamic_vertex_buffer = null, void* encoder_set_dynamic_vertex_buffer_with_layout = null, void* encoder_set_transient_vertex_buffer = null, void* encoder_set_transient_vertex_buffer_with_layout = null, void* encoder_set_vertex_count = null, void* encoder_set_instance_data_buffer = null, void* encoder_set_instance_data_from_vertex_buffer = null, void* encoder_set_instance_data_from_dynamic_vertex_buffer = null, void* encoder_set_instance_count = null, void* encoder_set_texture = null, void* encoder_touch = null, void* encoder_submit = null, void* encoder_submit_occlusion_query = null, void* encoder_submit_indirect = null, void* encoder_submit_indirect_count = null, void* encoder_set_compute_index_buffer = null, void* encoder_set_compute_vertex_buffer = null, void* encoder_set_compute_dynamic_index_buffer = null, void* encoder_set_compute_dynamic_vertex_buffer = null, void* encoder_set_compute_indirect_buffer = null, void* encoder_set_image = null, void* encoder_dispatch = null, void* encoder_dispatch_indirect = null, void* encoder_discard = null, void* encoder_blit = null, void* request_screen_shot = null, void* render_frame = null, void* set_platform_data = null, void* get_internal_data = null, void* override_internal_texture_ptr = null, void* override_internal_texture = null, void* set_marker = null, void* set_state = null, void* set_condition = null, void* set_stencil = null, void* set_scissor = null, void* set_scissor_cached = null, void* set_transform = null, void* set_transform_cached = null, void* alloc_transform = null, void* set_uniform = null, void* set_index_buffer = null, void* set_dynamic_index_buffer = null, void* set_transient_index_buffer = null, void* set_vertex_buffer = null, void* set_vertex_buffer_with_layout = null, void* set_dynamic_vertex_buffer = null, void* set_dynamic_vertex_buffer_with_layout = null, void* set_transient_vertex_buffer = null, void* set_transient_vertex_buffer_with_layout = null, void* set_vertex_count = null, void* set_instance_data_buffer = null, void* set_instance_data_from_vertex_buffer = null, void* set_instance_data_from_dynamic_vertex_buffer = null, void* set_instance_count = null, void* set_texture = null, void* touch = null, void* submit = null, void* submit_occlusion_query = null, void* submit_indirect = null, void* submit_indirect_count = null, void* set_compute_index_buffer = null, void* set_compute_vertex_buffer = null, void* set_compute_dynamic_index_buffer = null, void* set_compute_dynamic_vertex_buffer = null, void* set_compute_indirect_buffer = null, void* set_image = null, void* dispatch = null, void* dispatch_indirect = null, void* discard = null, void* blit = null)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3104) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bc92182e4d6434fc"></a>[`public struct Inno.Native.Bgfx.bgfx.InterfaceVtbl`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2900) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.InternalData</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.InternalData 51ccdb94bad68e67827e0950ac2649bd36a60cfd21a506fe3e4432c40ec87e2f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InternalData.caps 0545fde4fc63b336693a0e47e7fc2435fa187255ad24d6696f1ee4468a7d52f2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.InternalData.context 03ed0a2a983e550e0d31672cf6ea4d396053e1e4e52a0240ed4784b70bb784e5 -->
<!-- api: M:Inno.Native.Bgfx.bgfx.InternalData.#ctor(Inno.Native.Bgfx.bgfx.Caps*,System.Void*) 3ab273e5cea246380c7960fe68f93d02e78d8bbcf6bd336acba4138b3b064a02 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4651d7163034cbdd"></a>[`public Inno.Native.Bgfx.bgfx.Caps* Inno.Native.Bgfx.bgfx.InternalData.caps`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2884) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7495af0be1dcc1f8"></a>[`public void* Inno.Native.Bgfx.bgfx.InternalData.context`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2885) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4b8126b92bb5241"></a>[`public Inno.Native.Bgfx.bgfx.InternalData.InternalData(Inno.Native.Bgfx.bgfx.Caps* caps = null, void* context = null)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2886) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1bb26ab0472f9567"></a>[`public struct Inno.Native.Bgfx.bgfx.InternalData`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2882) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.Memory</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.Memory eb3f14984391f46b51fe5b6f5f875745b7a038c592c5102851b09eb2f4f0503e -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Memory.data 8762923598208ab13efdb485fa2424f1af9e3ae746b769f63f7463876b5cd9f2 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.Memory.size b56a4666ce0cd798ce0f65e2421befed0cc4ba876313a6f742840b399ef0f69f -->
<!-- api: M:Inno.Native.Bgfx.bgfx.Memory.#ctor(System.Byte*,System.UInt32) 9e39a98131feb5b94b85fd6aa4b769cc6e5cc1cabca6519e588e90fb09dc339a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-732a17b2d093ec4e"></a>[`public byte* Inno.Native.Bgfx.bgfx.Memory.data`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1979) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2ed4c50568eb285d"></a>[`public uint Inno.Native.Bgfx.bgfx.Memory.size`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1980) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e9be5c17485d673"></a>[`public Inno.Native.Bgfx.bgfx.Memory.Memory(byte* data = null, uint size = 0)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1981) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5a09c577c0da4b2e"></a>[`public struct Inno.Native.Bgfx.bgfx.Memory`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L1977) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.NativeWindowHandleType</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.NativeWindowHandleType 6b765d1d901519a739eadd353100bb201e4626695f4430029ea98f14f2177a6b -->
<!-- api: F:Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Default 5268c56ed418004ba00d1b37676ed5b376cb2d087c2dd7df2bbc5c02de7c636f -->
<!-- api: F:Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Wayland 1a51db22f0015ee17d0986ed345eff90f3f6389c1a1b91be7ae41f6a082cd73d -->
<!-- api: F:Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Count 478bea69367b9a0efa12df925e48c7e541f5ddc0106da3106e374937fa4a021e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-932328d246cb378c"></a>[`Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Count = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L387) | 当前常量或枚举值：`2`。 | — |
| <a id="api-6f7779b525c56053"></a>[`Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Default = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L385) | 当前常量或枚举值：`0`。 | — |
| <a id="api-8bc6888fd038a6bd"></a>[`Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Wayland = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L386) | 当前常量或枚举值：`1`。 | — |
| <a id="api-5275ffa9e848eb83"></a>[`public enum Inno.Native.Bgfx.bgfx.NativeWindowHandleType : int`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L383) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.OcclusionQueryHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.OcclusionQueryHandle 0d7e78c029985e0b7305ffdfab599ee2e50516314bd58c5d8aa9ca3748190ba4 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.idx ff4d1ca44bd26d743b77b18c7082383fa2e966707f9ef43e7b3eee79759842bb -->
<!-- api: P:Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.Valid 52e892c897db39f7ded4fcfad93e2f643214d551b3d17e9cd0374a5dc0a8838a -->
<!-- api: M:Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.#ctor(System.UInt16) 9c2f74e37478783755c8b5ae48605b4260d0edcbfd31e23f233bef75b4466c2c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dd01e85403de3928"></a>[`public ushort Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2821) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a8557a94ced2aff8"></a>[`public Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.OcclusionQueryHandle(ushort idx = 0)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2824) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0797eabd0d3711e0"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.OcclusionQueryHandle.Valid { get; }`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2822) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d68beac0a0730ed6"></a>[`public struct Inno.Native.Bgfx.bgfx.OcclusionQueryHandle`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2819) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-f6def1baaaa3f26a"></a>[`Inno.Native.Bgfx.bgfx.OcclusionQueryResult.Count = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L286) | 当前常量或枚举值：`3`。 | — |
| <a id="api-a3ba883e0c252db8"></a>[`Inno.Native.Bgfx.bgfx.OcclusionQueryResult.Invisible = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L283) | 当前常量或枚举值：`0`。 | — |
| <a id="api-03cb06ac339775e8"></a>[`Inno.Native.Bgfx.bgfx.OcclusionQueryResult.NoResult = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L285) | 当前常量或枚举值：`2`。 | — |
| <a id="api-72f21c9585dd794e"></a>[`Inno.Native.Bgfx.bgfx.OcclusionQueryResult.Visible = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L284) | 当前常量或枚举值：`1`。 | — |
| <a id="api-40203bc73f9b0d8f"></a>[`public enum Inno.Native.Bgfx.bgfx.OcclusionQueryResult : int`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L281) | Native 取值集合；当前取值见本类型成员。 | — |

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
| <a id="api-699278333e5a49af"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.Amd = 4098`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3526) | 当前常量或枚举值：`4098`。 | — |
| <a id="api-c45c2318260247a2"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.Apple = 4203`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3527) | 当前常量或枚举值：`4203`。 | — |
| <a id="api-3935477ab99cddfd"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.Arm = 5045`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3531) | 当前常量或枚举值：`5045`。 | — |
| <a id="api-b471e9f381bab53d"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.Intel = 32902`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3528) | 当前常量或枚举值：`32902`。 | — |
| <a id="api-ffb6d0838ae1426a"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.Microsoft = 5140`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3530) | 当前常量或枚举值：`5140`。 | — |
| <a id="api-0ed54416f8fa12a6"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.None = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3524) | 当前常量或枚举值：`0`。 | — |
| <a id="api-694c3a85555c0faa"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.Nvidia = 4318`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3529) | 当前常量或枚举值：`4318`。 | — |
| <a id="api-8a0c8a6349c735f2"></a>[`Inno.Native.Bgfx.bgfx.PciIdFlags.SoftwareRasterizer = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3525) | 当前常量或枚举值：`1`。 | — |
| <a id="api-ae56e94d4ac285e6"></a>[`public enum Inno.Native.Bgfx.bgfx.PciIdFlags : ushort`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L3522) | Native 取值集合；当前取值见本类型成员。 | — |

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
| <a id="api-27767ee1d302639a"></a>[`public void* Inno.Native.Bgfx.bgfx.PlatformData.backBuffer`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L808) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-76e9f38e3e550404"></a>[`public void* Inno.Native.Bgfx.bgfx.PlatformData.backBufferDS`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L809) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b20c3bc99563b6b5"></a>[`public void* Inno.Native.Bgfx.bgfx.PlatformData.context`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L806) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-342f1ab746eab45a"></a>[`public void* Inno.Native.Bgfx.bgfx.PlatformData.ndt`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L804) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6e5c92f2e316902e"></a>[`public void* Inno.Native.Bgfx.bgfx.PlatformData.nwh`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L805) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce34b743265a7bc8"></a>[`public void* Inno.Native.Bgfx.bgfx.PlatformData.queue`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L807) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-faedf77ff21f7cd4"></a>[`public Inno.Native.Bgfx.bgfx.NativeWindowHandleType Inno.Native.Bgfx.bgfx.PlatformData.type`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L810) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce1088c1baa6e163"></a>[`public Inno.Native.Bgfx.bgfx.PlatformData.PlatformData(void* ndt = null, void* nwh = null, void* context = null, void* queue = null, void* backBuffer = null, void* backBufferDS = null, Inno.Native.Bgfx.bgfx.NativeWindowHandleType type = Inno.Native.Bgfx.bgfx.NativeWindowHandleType.Default)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L811) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d1c9dd22355b339c"></a>[`public struct Inno.Native.Bgfx.bgfx.PlatformData`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L802) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Bgfx.bgfx.ProgramHandle</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Bgfx.bgfx.ProgramHandle c4d0a12bfd17a7552cb8b40833db4ecc2c319a52d636bb011455188915965138 -->
<!-- api: F:Inno.Native.Bgfx.bgfx.ProgramHandle.idx 3e096c6a797a741a5e61cc4c7ac15a3cbb4045594a04b220653d17f5dd155fa6 -->
<!-- api: P:Inno.Native.Bgfx.bgfx.ProgramHandle.Valid 1b99a000647dc6cbf496fd451007a89f39fec4f15fb36293ca3cb83fc2a60dec -->
<!-- api: M:Inno.Native.Bgfx.bgfx.ProgramHandle.#ctor(System.UInt16) 0a1799b43db3c09f83be6d53f48c3f898c2b0722c133c03870dc05e0258e08ce -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6fd379784de5f0aa"></a>[`public ushort Inno.Native.Bgfx.bgfx.ProgramHandle.idx`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2217) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf7ccaab95ba7740"></a>[`public Inno.Native.Bgfx.bgfx.ProgramHandle.ProgramHandle(ushort idx = 0)`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2220) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-af7d7bff3cc587d9"></a>[`public readonly bool Inno.Native.Bgfx.bgfx.ProgramHandle.Valid { get; }`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2218) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-06cdb68f566eef5d"></a>[`public struct Inno.Native.Bgfx.bgfx.ProgramHandle`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L2215) | 当前 ABI 的托管类型或借用视图声明。 | — |

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
| <a id="api-1de729c27a5f8dd3"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Agc = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L43) | 当前常量或枚举值：`1`。 | — |
| <a id="api-a35e26cb28d293e0"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Count = 11`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L53) | 当前常量或枚举值：`11`。 | — |
| <a id="api-3e73f602acb7a713"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Direct3D11 = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L44) | 当前常量或枚举值：`2`。 | — |
| <a id="api-a15a4435b3700d1e"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Direct3D12 = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L45) | 当前常量或枚举值：`3`。 | — |
| <a id="api-1df40549495fb9ca"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Gnm = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L46) | 当前常量或枚举值：`4`。 | — |
| <a id="api-025df2e10549d2ac"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Metal = 5`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L47) | 当前常量或枚举值：`5`。 | — |
| <a id="api-e3de132f44bfd39d"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Noop = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L42) | 当前常量或枚举值：`0`。 | — |
| <a id="api-81dcd5fc0e1cc749"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Nvn = 6`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L48) | 当前常量或枚举值：`6`。 | — |
| <a id="api-6c80632ec4d6c8ec"></a>[`Inno.Native.Bgfx.bgfx.RendererType.OpenGL = 8`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L50) | 当前常量或枚举值：`8`。 | — |
| <a id="api-463e46f1b6893709"></a>[`Inno.Native.Bgfx.bgfx.RendererType.OpenGLES = 7`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L49) | 当前常量或枚举值：`7`。 | — |
| <a id="api-d7ba0a4ac7813e11"></a>[`Inno.Native.Bgfx.bgfx.RendererType.Vulkan = 9`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L51) | 当前常量或枚举值：`9`。 | — |
| <a id="api-52fa80d010841106"></a>[`Inno.Native.Bgfx.bgfx.RendererType.WebGPU = 10`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L52) | 当前常量或枚举值：`10`。 | — |
| <a id="api-1b122a12c7e55e6b"></a>[`public enum Inno.Native.Bgfx.bgfx.RendererType : int`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L40) | Native 取值集合；当前取值见本类型成员。 | — |

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
| <a id="api-b107d7603ddcecf7"></a>[`Inno.Native.Bgfx.bgfx.RenderFrame.Count = 4`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L402) | 当前常量或枚举值：`4`。 | — |
| <a id="api-5b34643efb143ca3"></a>[`Inno.Native.Bgfx.bgfx.RenderFrame.Exiting = 3`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L401) | 当前常量或枚举值：`3`。 | — |
| <a id="api-74a11fb74fe18f67"></a>[`Inno.Native.Bgfx.bgfx.RenderFrame.NoContext = 0`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L398) | 当前常量或枚举值：`0`。 | — |
| <a id="api-b46637be3a7de1dc"></a>[`Inno.Native.Bgfx.bgfx.RenderFrame.Render = 1`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L399) | 当前常量或枚举值：`1`。 | — |
| <a id="api-336115a0a436ce7f"></a>[`Inno.Native.Bgfx.bgfx.RenderFrame.Timeout = 2`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L400) | 当前常量或枚举值：`2`。 | — |
| <a id="api-54169e5038d2f4a1"></a>[`public enum Inno.Native.Bgfx.bgfx.RenderFrame : int`](../../../../backends/Bgfx/native/Inno.Native.Bgfx/Generated/Bindings.cs#L396) | Native 取值集合；当前取值见本类型成员。 | — |

</details>
