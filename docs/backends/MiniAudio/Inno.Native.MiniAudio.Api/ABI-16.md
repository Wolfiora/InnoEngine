# Inno.Native.MiniAudio ABI · MaResourceManagerDataStream → MaSpatializerListenerConfigPtr

[项目与生命周期](../Inno.Native.MiniAudio.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.MiniAudio.MaResourceManagerDataStream</code> · 24 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataStream 48369b97cb83704aa69fc96358ff0fee2a3e2f6d4132b91c0fd7e5466f14ee93 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.Ds 77a10e01dc9b0fa5c1e5b21eb50eaae88ef135163667222022ade1122471d5e4 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.PResourceManager 43a775a0eb1b9e1a606a11c32e1d9119d52e9778efc1eda14477c9996f373dbf -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.Flags 72644bc4a04c4c31d8fac1e43a74b28afa2bbc40a56d43b69f2e348b87dbc0ce -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.Decoder b0aa023c64a7be5066fef12c0edf0b8f2005a060008ae163b1ec6810e06062d1 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.IsDecoderInitialized a2bf71aed21dc981a52974f03d6d269fe29d6c50169e43bd4536946547db52a2 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.TotalLengthInPcmFrames 3407db5c50d68c7d07da0e9cc7eac406b97e4baf3c06b008b0f1a8307bd6af91 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.RelativeCursor f0ef86da657907d7634479f320ad6a5a812b0da005fa7cab23c4d52385bc3222 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.AbsoluteCursor 5d2fd0d4a1e5eac998dd22779aaf3d5e4cd2cba553c9afddee62495d410a9955 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.CurrentPageIndex a49f423ede7add7aaf63740077d037973c61ff00d08fc4cb067d4b7ec5e9d1d4 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.ExecutionCounter 71b222b3a0288074abe542a2f7d61de696fc843153b9e75d99461bdafa77452e -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.ExecutionPointer 43ab655d189288a0b655174e8ade06a57b7e9f0b0f9f267066104e850313c5fa -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.IsLooping bea6f35d6ab0d8b0588b7e51d0d6ae08b77419c4e747a24a708805cf41112c96 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.PPageData 80d53f3f43f4005e77aed3832ec434eef4df96125e45843aca22f035e0ba7ac9 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.PageFrameCount_0 445dceb2102e1a52fddc05032bafc204dcf8e17aa3dea5da7957a7f8bbe797f2 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.PageFrameCount_1 8443348fe02fce04597d503c694a0972a46214a715c183c0c6e707eccd0874e5 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStream.PageFrameCount 4d92fad55a85734b2e9efc4f883cb9d7d4c0456c99e999380be6c29dc6b2bd5d -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.Result b6f67159701de17754b0ebf25b375f36a33e149c578fc56238918b0c17cdce92 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.IsDecoderAtEnd 7dc3c1443a5208ea5908f1f0a8a34576385986b49b594a2c8f4565212b638df1 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.IsPageValid_0 63600e69045f06debd32adeec0d31b6f658d84ff71455708177ed7e11908671a -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.IsPageValid_1 987bcfdf653a2b675d78cdca8c25505ceb7966ad581a20b814ccf263c27d8eab -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStream.IsPageValid 1709e397a9cd90ad838b3f366ce13d2762f243ee67077a8cf5cfc1553e854a4c -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStream.SeekCounter 18e3fbde39aef5c612ebe74d649ce134156500de7b3ea01953082842f48d9eca -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerDataStream.#ctor(Inno.Native.MiniAudio.MaDataSourceBase,Inno.Native.MiniAudio.MaResourceManager*,System.UInt32,Inno.Native.MiniAudio.MaDecoder,System.UInt32,System.UInt64,System.UInt32,System.UInt64,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.Void*,System.UInt32*,Inno.Native.MiniAudio.MaResult,System.UInt32,System.UInt32*,System.UInt32) 2c7299a9cee43b842b004eb26c0170ed71602ac54008cfba1f4624e1cb5a05ce -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3245ee9f033367a3"></a>[`public ulong Inno.Native.MiniAudio.MaResourceManagerDataStream.AbsoluteCursor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12830) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0796d0532c747aba"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataStream.CurrentPageIndex`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12831) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d671651e2c949852"></a>[`public Inno.Native.MiniAudio.MaDecoder Inno.Native.MiniAudio.MaResourceManagerDataStream.Decoder`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12826) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d65cd51c61e91f06"></a>[`public Inno.Native.MiniAudio.MaDataSourceBase Inno.Native.MiniAudio.MaResourceManagerDataStream.Ds`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12823) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-484063a3ef254caa"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataStream.ExecutionCounter`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12832) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-853fb2d2ce7cc932"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataStream.ExecutionPointer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12833) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c82b1cdd47ffe74"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataStream.Flags`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12825) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8b685a42eb98f5c7"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataStream.IsDecoderAtEnd`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12841) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a9a7d55666a05f4"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataStream.IsDecoderInitialized`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12827) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1694a19223559ac"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataStream.IsLooping`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12834) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c112c92fe23f4ad8"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataStream.IsPageValid_0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12842) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8da9fb25f08c0243"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataStream.IsPageValid_1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12843) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8d2b5c77581e6cc4"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataStream.PageFrameCount_0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12836) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e19d60fec63be51"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataStream.PageFrameCount_1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12837) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe0774a51b289b8a"></a>[`public void* Inno.Native.MiniAudio.MaResourceManagerDataStream.PPageData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12835) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b425a86a3a0d7ab1"></a>[`public Inno.Native.MiniAudio.MaResourceManager* Inno.Native.MiniAudio.MaResourceManagerDataStream.PResourceManager`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12824) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0b72f6375817a62a"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataStream.RelativeCursor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12829) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-221f40d9eb9a29ac"></a>[`public Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaResourceManagerDataStream.Result`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12840) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e98ae2b5780f360"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataStream.SeekCounter`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12846) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d642301d9f711dfa"></a>[`public ulong Inno.Native.MiniAudio.MaResourceManagerDataStream.TotalLengthInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12828) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eb19d7ff9e96f790"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataStream.MaResourceManagerDataStream(Inno.Native.MiniAudio.MaDataSourceBase ds = default(Inno.Native.MiniAudio.MaDataSourceBase), Inno.Native.MiniAudio.MaResourceManager* pResourceManager = null, uint flags = 0, Inno.Native.MiniAudio.MaDecoder decoder = default(Inno.Native.MiniAudio.MaDecoder), uint isDecoderInitialized = 0, ulong totalLengthInPCMFrames = 0, uint relativeCursor = 0, ulong absoluteCursor = 0, uint currentPageIndex = 0, uint executionCounter = 0, uint executionPointer = 0, uint isLooping = 0, void* pPageData = null, uint* pageFrameCount = null, Inno.Native.MiniAudio.MaResult result = Inno.Native.MiniAudio.MaResult.Success, uint isDecoderAtEnd = 0, uint* isPageValid = null, uint seekCounter = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12847) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f86793e911643207"></a>[`public System.Span<uint> Inno.Native.MiniAudio.MaResourceManagerDataStream.IsPageValid { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12844) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f6faba1c665edde1"></a>[`public System.Span<uint> Inno.Native.MiniAudio.MaResourceManagerDataStream.PageFrameCount { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12838) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-af2236e33b3db092"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataStream`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12821) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr</code> · 31 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr b0ff6b34824ce587ff9c65831fc516bfc5159d885e05305949baaa1d6d21b2eb -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.#ctor(Inno.Native.MiniAudio.MaResourceManagerDataStream*) b49d110563bfd992fcd37c89131c5b4e7c530e94e23ebbf792c8ceb7f06b75a1 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Handle 19cb2d5d0393fb9aa695e1bb886161a93d520d7575846685b58dadf7cb0486cf -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.IsNull e24f8576c90655c3c5aff88d8fff37590cd044acda0fb90427bbccb89c4a39be -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Null 64d47e7458eb55efb477c2442fa9811ea421b7abd0526c83b7652454d7561d0b -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Item(System.Int32) 1186a6ba9ac99b1192ef2aba216ca9b94fcf3c74975f8c4f78d8b7e2be413bc3 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.op_Implicit(Inno.Native.MiniAudio.MaResourceManagerDataStream*)~Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr 176a84d2e2de85205218c69d9355ff7f1d1bc21afb32273f5650c6ab9463775e -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.op_Implicit(Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr)~Inno.Native.MiniAudio.MaResourceManagerDataStream* a7c8fc345589ffda57bf92e0e73eeb0e4a268267bec21362f6bd41116f640687 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.op_Equality(Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr,Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr) 3db7dc5700cf6cd3ce30ac2fca139d933438899cb65d508066b61e02b929da72 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.op_Inequality(Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr,Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr) 4f802a1eca2cd467951afc346240d9abe91117b4002723d8828c78c2cc7a164d -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Equals(Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr) f4a641b0ff0f9ff7e50d4c1509d5f50fd403c88407b6fba9d7ed91d88e6ca158 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Equals(System.Object) 916b08884bf9415f40684be0de561c468da66092922f6b45989c66f8ee1a124b -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.GetHashCode 27845162dc75f41f3174be2520508ad440e41793a728e3f9fbfc2a489338e2b4 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Ds 5596ddc196dd5d5fbb0eb93377c33632c95ccef6a102223f503e188880014e79 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.PResourceManager f63f643666b386463ddec7a7e44894044ae700c49a373f17253bc544cf0f6201 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Flags 2ebf456afa7cedb40c84b1ce2873ea359b4dfd41fdcd53dca5a3c994e7bc7b22 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Decoder 3c5a3f1db12f279360c2d2e6420355d926a59256b705cb0580f622caa385377a -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.IsDecoderInitialized bab31df895eb27059b804aaff1fb8a969f7353b3e1a5891b39781f5a78c9881a -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.TotalLengthInPcmFrames 4cc1bace05edf894b8c49f0324065b6940528e6def92ff759e886e9134af1faa -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.RelativeCursor f5c082d459425fde05efa4cb5c84b674519820c2a8cf69aa407c5bd2155a3cc7 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.AbsoluteCursor 7580c16c39ec681a564607bcb4d10ea13bef09a95902168d78eaded7d8fcde34 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.CurrentPageIndex ce31f7c5c1023bfd9391dfc076c26a49a7f594b6e2dd38960be4abf42c481ffa -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.ExecutionCounter 643bc59aeb7512be5f5e0f3a5e9e38b1c96b88ffd3aaf74d4ae967e3f45962c6 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.ExecutionPointer 071fd679a9100076fef6e67fb475e73321186b942fc77f54d2835252ec5cdfd2 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.IsLooping a4c9b68446adb777a248e4a088660235f25e89fa0282c7a15a5897b26e12d061 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.PPageData e0eb8df1b2c11f7d63c54c630e9bf322a758004484b6d08a921f97b6fab45d8e -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.PageFrameCount 3b19fae44f1e74943dcfbaa0e000fadfefb82876b5303dd44ac601ee0275f24f -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Result 636961cdd5cac3db22024d0ea6b3d07da6a923d2b5f6bdee3c499b1d2da71ce9 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.IsDecoderAtEnd 9b77d5bc4e3dfa682ad5c5bbc852e671bd87e3d11d092bb7d8bfabf51e623bfd -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.IsPageValid 211d6645ec6880e9da0ffc2aae44eee5662d02e9e8179a2cc1c1a324e592b6c9 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.SeekCounter 38739924a9f1f9a6016b625041c9b8bf37f34dc292db5216f5dc5c3cc2dcbd84 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e60328eb06c012da"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataStream* Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17980) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6a7d743008644d8b"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.MaResourceManagerDataStreamPtr(Inno.Native.MiniAudio.MaResourceManagerDataStream* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17975) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9d1ae7d39cc0d7d7"></a>[`public bool Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Equals(Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17990) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-e3e444944464f115"></a>[`public override bool Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17991) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-8ac10b5c7d1cd904"></a>[`public override int Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17992) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-a0bdc23649cdefbf"></a>[`public static bool Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.operator ==(Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr left, Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17988) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d0e932a3503781a8"></a>[`public static Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.implicit operator Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr(Inno.Native.MiniAudio.MaResourceManagerDataStream* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17986) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5f47749544fe3ba4"></a>[`public static Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.implicit operator Inno.Native.MiniAudio.MaResourceManagerDataStream*(Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17987) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-331b521978d32f82"></a>[`public static bool Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.operator !=(Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr left, Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17989) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-841e2e40fe87de94"></a>[`public ref ulong Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.AbsoluteCursor { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18001) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-20916d9045f77323"></a>[`public ref uint Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.CurrentPageIndex { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18002) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dc48cd276afeb03c"></a>[`public ref Inno.Native.MiniAudio.MaDecoder Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Decoder { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17997) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-44d5ca852e93f088"></a>[`public ref Inno.Native.MiniAudio.MaDataSourceBase Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Ds { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17994) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f79e8d50576659d2"></a>[`public ref uint Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.ExecutionCounter { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18003) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3dd0e6dee01eddfc"></a>[`public ref uint Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.ExecutionPointer { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18004) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8da43b7cd4eac594"></a>[`public ref uint Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Flags { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17996) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d5143a05c2b03960"></a>[`public ref uint Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.IsDecoderAtEnd { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18009) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-65043a79d0a2aed1"></a>[`public ref uint Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.IsDecoderInitialized { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17998) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-29493a0e3fe357d5"></a>[`public ref uint Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.IsLooping { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18005) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5dd134a9a3a768ef"></a>[`public bool Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17981) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e4eff276fdf92525"></a>[`public System.Span<uint> Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.IsPageValid { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18010) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a76edfa02b65182a"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataStream Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17984) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1b76c6075e1684c2"></a>[`public static Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17982) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ec519bd6a0af714"></a>[`public System.Span<uint> Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.PageFrameCount { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18007) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2b221dbd786f7272"></a>[`public void* Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.PPageData { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18006) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3941c6720b0a8b07"></a>[`public ref Inno.Native.MiniAudio.MaResourceManagerPtr Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.PResourceManager { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17995) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-79bef40bc0b74e3c"></a>[`public ref uint Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.RelativeCursor { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18000) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a962d8b842f59d9b"></a>[`public ref Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.Result { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18008) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-755fae088bd22540"></a>[`public ref uint Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.SeekCounter { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18011) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c59dffb53f0dd130"></a>[`public ref ulong Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr.TotalLengthInPcmFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17999) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fb5efb810260ff9d"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr : System.IEquatable<Inno.Native.MiniAudio.MaResourceManagerDataStreamPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L17973) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaResourceManagerDataSupply</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSupply 1f54fe7d831a473f13d7e8980986b580f0a9df0d705820f36ed2f94ddbffd09d -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.Type 966ea1f41dc52e153bf452dce05c0e521ad7604ee698c943d4c4d0493385f4e2 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.Backend d9c9d06cc88685e747784b709d8dbbf2d51ffeced8fae6ed7caceb1191bac478 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerDataSupply.#ctor(Inno.Native.MiniAudio.MaResourceManagerDataSupplyType,Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous) 86b325fc9cdf66875070fabeeabb9264353390bd4922848b59162a7e1c5f16b2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7e208aed0effc9a1"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous Inno.Native.MiniAudio.MaResourceManagerDataSupply.Backend`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12572) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e92dab82315d419c"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataSupplyType Inno.Native.MiniAudio.MaResourceManagerDataSupply.Type`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12571) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d3e0d75775c2bef"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataSupply.MaResourceManagerDataSupply(Inno.Native.MiniAudio.MaResourceManagerDataSupplyType type = Inno.Native.MiniAudio.MaResourceManagerDataSupplyType.Unknown, Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous backend = default(Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12573) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fc60f249f07b5e52"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataSupply`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12500) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous 0f84bc054a832487b9b38a5a45018b5ff485539c128c79858c880cad750b4159 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.Encoded 53b3ef3c5df35df444c507a3ad5bbd97392008240f5132a632493db45251f0c8 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.Decoded deea5616692de587eb99dff4fd2a7a94f4863bb59a00c816f63996911393bd7f -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPaged 47d3d07e221f17a36c7f8359304890e6c99babfdbb6648907dad2af259d86cd6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0f52d040febeebff"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.Decoded`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12566) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5baf7e8c4f3b48cb"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPagedAnonymous Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPaged`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12568) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad0943b470748ab5"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.EncodedAnonymous Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.Encoded`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12564) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5852c2412a090021"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12506) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous 0bc4ce3d61ff1c78b48c40f6e368989bfb0f6110c16f46437f30b4157aa5f6a8 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.PData ebc0d7f475eba879863f7a6bf06f1ee47e1ca238d2cd75dfdb1cf99e2d5b50d3 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.TotalFrameCount 5acab0fba219d50e95c01ef396044ebcbc6ea67c31e56bcccd1c55a1fc6d02fa -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.DecodedFrameCount 3ba0c6faaa276c3de4695c0485147dfbfc63c820b7614aacd0da5629c1287930 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.Format 8b376b1cdad420ece402821c6b2dfab71a69201a37c3af5ccac3099d62922463 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.Channels 228adf7be64b1026db929ec60bd630597120b5ba2c1ab7eba238985c8c8714fc -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.SampleRate 1dc1b75d56d825fbc498d82b30efc6aea59fd58ccff6c4c13b5a8a7e4e5c7e03 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.#ctor(System.Void*,System.UInt64,System.UInt64,Inno.Native.MiniAudio.MaFormat,System.UInt32,System.UInt32) 5ac2d697d39b6a0755ef91c8614dc195410be77231f7893115988e8ce8f32622 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9c5eedf276a1248c"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12533) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-475e0f89aa706c09"></a>[`public ulong Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.DecodedFrameCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12531) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-40b59c6cc5011778"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12532) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-68b811f9b1c866ef"></a>[`public void* Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.PData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12529) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b1493f0309f0a98"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12534) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b3fb2352b8fb1add"></a>[`public ulong Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.TotalFrameCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12530) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e28383fd8a5009dd"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous.DecodedAnonymous(void* pData = null, ulong totalFrameCount = 0, ulong decodedFrameCount = 0, Inno.Native.MiniAudio.MaFormat format = Inno.Native.MiniAudio.MaFormat.Unknown, uint channels = 0, uint sampleRate = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12535) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-58d99a63e45d7454"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12527) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPagedAnonymous</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPagedAnonymous 0b17b7bfa31456eb566ebf386c06fc90fcd6d3116ca0ba107ade968cfb2c845a -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPagedAnonymous.Data 1d3556c170235cfac77e3678c92ca2339a85d521ca6b771ff17de394ed6022a3 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPagedAnonymous.DecodedFrameCount 825d7241e37feb90c5df64e08da6857805242787b445c666e29a20231ff741e4 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPagedAnonymous.SampleRate da8760635128c6cd986565e8d6c4754e5443c8ecc0f38cc198e4e7a180f17e99 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPagedAnonymous.#ctor(Inno.Native.MiniAudio.MaPagedAudioBufferData,System.UInt64,System.UInt32) 371eee6f2447b57500d0ecf16cbe850ac3bfa689875b45214b3596bbc521c949 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f7a14c783fba9e51"></a>[`public Inno.Native.MiniAudio.MaPagedAudioBufferData Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPagedAnonymous.Data`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12552) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a021005dee86d41"></a>[`public ulong Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPagedAnonymous.DecodedFrameCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12553) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd9e22617611ac50"></a>[`public uint Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPagedAnonymous.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12554) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27d6cb18203bfe95"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPagedAnonymous.DecodedPagedAnonymous(Inno.Native.MiniAudio.MaPagedAudioBufferData data = default(Inno.Native.MiniAudio.MaPagedAudioBufferData), ulong decodedFrameCount = 0, uint sampleRate = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12555) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-168ba67a71572e66"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.DecodedPagedAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12550) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.EncodedAnonymous</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.EncodedAnonymous 3893de812733efbb4c55ae008d69cd5c7b70964ebd5fc1522f5624cc1489405b -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.EncodedAnonymous.PData 2a798151875550e185c347c5286733f06e6f993646390fa167e1e4656f82140f -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.EncodedAnonymous.SizeInBytes e0beb910e07689e80c20fceb09ac5bd760e1d578fe2bca1b946b8748e3b0431f -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.EncodedAnonymous.#ctor(System.Void*,System.UIntPtr) 86de566a3ebb9c760b2439500c89650f756a5dc2f21b0631243d48b188c3c4bf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-766b537f2df0c1d2"></a>[`public void* Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.EncodedAnonymous.PData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12514) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea1adfa64db1a135"></a>[`public nuint Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.EncodedAnonymous.SizeInBytes`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12515) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e206e9de7b61c2b"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.EncodedAnonymous.EncodedAnonymous(void* pData = null, nuint sizeInBytes = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12516) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e4cd769989111b6d"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerDataSupply.BackendAnonymous.EncodedAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12512) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaResourceManagerDataSupplyType</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerDataSupplyType 5dbc677d88ab4fd2890971453b5a45142e711c18a6c07491bcc0d96fb0f4e16b -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupplyType.Unknown e714545e35f58f92976eb8b1088a7a13d1469fa848dbc7f84df7848aac87270d -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupplyType.Encoded adf218a1c75418c0610f8636b97456bf737e4391da8bdecca00573b7a240cb41 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupplyType.Decoded e51d3ffe030413a7b2193aab7af79ed21d7bf3e01214eb5dced4f31951c83adb -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerDataSupplyType.DecodedPaged a4f286e2e6e79f5eae50204b88a2af5de734add9fad7e31a713afacae8737312 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e43c88209df394e2"></a>[`Inno.Native.MiniAudio.MaResourceManagerDataSupplyType.Decoded = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L682) | 当前常量或枚举值：`2`。 | — |
| <a id="api-a8ef9e04e49a1bad"></a>[`Inno.Native.MiniAudio.MaResourceManagerDataSupplyType.DecodedPaged = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L683) | 当前常量或枚举值：`3`。 | — |
| <a id="api-7af1dde77503cb60"></a>[`Inno.Native.MiniAudio.MaResourceManagerDataSupplyType.Encoded = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L681) | 当前常量或枚举值：`1`。 | — |
| <a id="api-f087dd59fceac724"></a>[`Inno.Native.MiniAudio.MaResourceManagerDataSupplyType.Unknown = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L680) | 当前常量或枚举值：`0`。 | — |
| <a id="api-09db401c062951c2"></a>[`public enum Inno.Native.MiniAudio.MaResourceManagerDataSupplyType : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L678) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaResourceManagerFlags</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerFlags 6afb3db39fd64cc99250d90d7440f9324640ef3f266fe089f8d5671b7fc28527 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerFlags.FlagNonBlocking f96322ae72d70f13ee37953f261f7902a17e9acaab8d324ca6503d9e0c362273 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerFlags.FlagNoThreading 125c2cf293ce387a7bd47265c20f39258bfd61e0fa5c0522dc4b7465e76c91af -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dc38404466c14617"></a>[`Inno.Native.MiniAudio.MaResourceManagerFlags.FlagNonBlocking = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L704) | 当前常量或枚举值：`1`。 | — |
| <a id="api-e4e437008c14f969"></a>[`Inno.Native.MiniAudio.MaResourceManagerFlags.FlagNoThreading = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L705) | 当前常量或枚举值：`2`。 | — |
| <a id="api-cada1ada996212c6"></a>[`public enum Inno.Native.MiniAudio.MaResourceManagerFlags : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L702) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications 1736821d7f8c06f7fc7f844fbfc1fcc54965ece6df13a83b8f0bf194242d2ec1 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications.Init 2cc8d2e35a2e6a147646c5b1b72d104e0256a3596a6cc024ef557f2eeff1f2e8 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications.Done 58990a684bdfa8b5ebf52c9bd0e359a7158e1d63302740fdac4c98b0674a08ff -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications.#ctor(Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification,Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification) 195a95f84a65cc316b7fd3bbb6bd4c5dffdd1588d0d69a95dda943a68ea7c9e2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-acf7dd0a93d021be"></a>[`public Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications.Done`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12933) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d5db9cd97e595a2a"></a>[`public Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications.Init`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12932) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc2c174243c0d31e"></a>[`public Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications.MaResourceManagerPipelineNotifications(Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification init = default(Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification), Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification done = default(Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12934) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6e585ae676f32eba"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12930) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr f4484bac5aa24520fe1f7d0a833b2bb438e9e57e3daae72c722bada11e28f274 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.#ctor(Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications*) 5b219169cc6e87c8fdf190728f7e74619f606db3f7bc8ce382daa7b3b4117824 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.Handle 9db703ad7bd63d0fcd544103a031215049201681b183609cb31cf5bb12f033b6 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.IsNull 835af5325545ef2924be0bbbb76bff160cb5afe4e2460d3d542082fc610bfb30 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.Null 5f9d2ae9cbadc735309abafc03db7bd297075d4228e29e2352278fd64bc729be -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.Item(System.Int32) ecb8424bd6b4709445ae8c9ba3bbb32fdad5890dc9ab206ed6beb394ef2b1785 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.op_Implicit(Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications*)~Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr a146ef514fc9242c2200830d79a074679806007d3755e7e1001d0e7e9a747278 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.op_Implicit(Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr)~Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications* 179f2b99994455333262ed9920d6ae33c621ba06b3619e23eaf77413f2c2b747 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.op_Equality(Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr,Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr) 81d8e61b63c806020d4db3b332c8cec3fd6e0566fb5761eca07bfb9518630d02 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.op_Inequality(Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr,Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr) bdaf1765d135c1f264dab0e0cd69b45953106fd27aa30a42cb0a3aaa3bde7470 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.Equals(Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr) a4611281cbb8134ba6caa2690db3ffaecbbae6b2e4aed87dbb00cb39eb43b3fd -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.Equals(System.Object) 98df2a10991bb6045f55de1de942e507fd7e94920e7a86a4f4943eb604e67ee6 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.GetHashCode 0d92bea084f622fea459288864bd7ca4817d4cb8760e605a639e24ae83e7578b -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.Init 7df2714457e3eae00ed65b0e28fffd80f02270983e30fa4ffbd761a164fc624f -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.Done d9a074e184415a0d8efe8c57fb0424b333ba5f692d41f5a788b1d084a89298ce -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a06d85be1bdd3b83"></a>[`public Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications* Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18022) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e27ff759469f7efe"></a>[`public Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.MaResourceManagerPipelineNotificationsPtr(Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18017) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8cc379c8fde0e4f2"></a>[`public bool Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.Equals(Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18032) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-3c1caeaf763745d4"></a>[`public override bool Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18033) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-25a4daf4b1e885b4"></a>[`public override int Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18034) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-940a51cbc6c5b3a4"></a>[`public static bool Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.operator ==(Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr left, Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18030) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f83295a9777bbf69"></a>[`public static Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.implicit operator Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr(Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18028) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8afaaa6a5428757a"></a>[`public static Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.implicit operator Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications*(Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18029) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0cff517c69420399"></a>[`public static bool Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.operator !=(Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr left, Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18031) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ce8f06b361fb83f6"></a>[`public ref Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.Done { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18037) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a98cfad276c33724"></a>[`public ref Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.Init { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18036) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f7e21a11751ccd10"></a>[`public bool Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18023) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ee1322ae6b95413e"></a>[`public Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18026) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-95db87d97413d95c"></a>[`public static Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18024) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7fdfd3c30330e736"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr : System.IEquatable<Inno.Native.MiniAudio.MaResourceManagerPipelineNotificationsPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18015) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification 8620c921a9b4be9b49850684d778ad0d502eca95f56170bca4d258799979b290 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification.PNotification 3ce94e98a31f37524de2e239b5fce45935f70aa0b230f2a21429726b8a10a874 -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification.PFence ae9ee163555a668e5f27d2460b71c3f7b96cba63e00d25f1d7e5cff7b815399a -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification.#ctor(Inno.Native.MiniAudio.MaAsyncNotification*,Inno.Native.MiniAudio.MaFence*) a60de253c3936f0a888db079c4eccd79bbc9db7d5328380258425d55e5fd6ac2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ce006468b21e8822"></a>[`public Inno.Native.MiniAudio.MaFence* Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification.PFence`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12918) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2dd08f4784284b0"></a>[`public Inno.Native.MiniAudio.MaAsyncNotification* Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification.PNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12917) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de268f4049f8b736"></a>[`public Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification.MaResourceManagerPipelineStageNotification(Inno.Native.MiniAudio.MaAsyncNotification* pNotification = null, Inno.Native.MiniAudio.MaFence* pFence = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12919) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2396bf7c553596ca"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerPipelineStageNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12915) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaResourceManagerPtr</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResourceManagerPtr e4120a3193649267809f8a669fdeb56088d3bf550fa28ad0142ec7a5bc9a2aef -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPtr.#ctor(Inno.Native.MiniAudio.MaResourceManager*) 91d1e03e7cd614eb6d43b0afd6af3c5d916d17b439e5351300bf38693132913a -->
<!-- api: F:Inno.Native.MiniAudio.MaResourceManagerPtr.Handle 6aa005fd71095f488641b15d8d8c0b51d88cad673670308f51ef7562b3e9136b -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPtr.IsNull c53a722238a1c94e63bb0f597d0fb19aadc8993d8c097dfc04c2bbd52f2f3200 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPtr.Null cb5068dcc3d3c4e7bfa266903c8417159df02412b525da8f125a93458cd8bd98 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPtr.Item(System.Int32) 2e388427d118a3ae11a6425175f8f3cc547b22e52c1a1c3ccaacb83c08844b35 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPtr.op_Implicit(Inno.Native.MiniAudio.MaResourceManager*)~Inno.Native.MiniAudio.MaResourceManagerPtr 492e41def1562f9a4923010579cd968630eaa36dfd2b3a5632ba9ff80e05c931 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPtr.op_Implicit(Inno.Native.MiniAudio.MaResourceManagerPtr)~Inno.Native.MiniAudio.MaResourceManager* b84a3c20db295c212ea1a0480f46c6a6d5410de9539fdf36e00c8705a723dc56 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPtr.op_Equality(Inno.Native.MiniAudio.MaResourceManagerPtr,Inno.Native.MiniAudio.MaResourceManagerPtr) 960ffaedc782ecc9d7567d588a3716e059c413cb51a07c1eeddc6527c5ffea72 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPtr.op_Inequality(Inno.Native.MiniAudio.MaResourceManagerPtr,Inno.Native.MiniAudio.MaResourceManagerPtr) ca3ab33525c7ecb6af82c454c6e5abe2b23037b3fad2734c61693737d124ead6 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPtr.Equals(Inno.Native.MiniAudio.MaResourceManagerPtr) c62ea398caf34f8e63d1d91702330072ab3bb1f9cae4fdc79c8d5f1c035924d9 -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPtr.Equals(System.Object) 96da8314415c226ae4bfbb754a405ac2730ef0ac2b862e2977bd3cdc37030adb -->
<!-- api: M:Inno.Native.MiniAudio.MaResourceManagerPtr.GetHashCode 9ddbdf312b1689916bacb2cf52f97edaf2703ea4a1d67796657b2d91625d6c6a -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPtr.Config e939a9b59b96843895506e7e3cf02ea5b905c233d2ac3b84334da6fda8b8e2c1 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPtr.PRootDataBufferNode f9506272ad5e4605b3264a912ea9e0927c554567690c62dee74bb643e58a8f82 -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPtr.DataBufferBstLock 217fd2b5765c798d67fcaa3532baf6e300dd01ee794a40b6c76eaf7d3ea4c0bb -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPtr.JobThreads d141cc85eee94b85583163e75db9eed54c0b338326708df6b9608872627f2b5e -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPtr.JobQueue a4b7fff578f224435743e021007e33d24732cdb32d92f390fe0936faaf32126b -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPtr.DefaultVfs 9a22c7432554485d5a1a0f0850f93ac2b9cad50e01fbe49de19097b31d48671a -->
<!-- api: P:Inno.Native.MiniAudio.MaResourceManagerPtr.Log 0fc68078ce8faa4ff0fa64d8db74f7f551651196fb6068938b07f7b59004489b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-41b8bc38180d8364"></a>[`public Inno.Native.MiniAudio.MaResourceManager* Inno.Native.MiniAudio.MaResourceManagerPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18048) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-84e9b41e8ad6da7c"></a>[`public Inno.Native.MiniAudio.MaResourceManagerPtr.MaResourceManagerPtr(Inno.Native.MiniAudio.MaResourceManager* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18043) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-229dfcfa91a5d29c"></a>[`public bool Inno.Native.MiniAudio.MaResourceManagerPtr.Equals(Inno.Native.MiniAudio.MaResourceManagerPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18058) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-16bcaa3a6c7b783a"></a>[`public override bool Inno.Native.MiniAudio.MaResourceManagerPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18059) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-8600fb0f2e581a59"></a>[`public override int Inno.Native.MiniAudio.MaResourceManagerPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18060) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-1561504716e1ea32"></a>[`public static bool Inno.Native.MiniAudio.MaResourceManagerPtr.operator ==(Inno.Native.MiniAudio.MaResourceManagerPtr left, Inno.Native.MiniAudio.MaResourceManagerPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18056) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4bc28c02b6f59751"></a>[`public static Inno.Native.MiniAudio.MaResourceManagerPtr.implicit operator Inno.Native.MiniAudio.MaResourceManagerPtr(Inno.Native.MiniAudio.MaResourceManager* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18054) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e7fa75dfcd6c768c"></a>[`public static Inno.Native.MiniAudio.MaResourceManagerPtr.implicit operator Inno.Native.MiniAudio.MaResourceManager*(Inno.Native.MiniAudio.MaResourceManagerPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18055) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7fa2e24c6025af11"></a>[`public static bool Inno.Native.MiniAudio.MaResourceManagerPtr.operator !=(Inno.Native.MiniAudio.MaResourceManagerPtr left, Inno.Native.MiniAudio.MaResourceManagerPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18057) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1ce984d2cc447255"></a>[`public ref Inno.Native.MiniAudio.MaResourceManagerConfig Inno.Native.MiniAudio.MaResourceManagerPtr.Config { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18062) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8c1b93c5239ebe9b"></a>[`public ref nint Inno.Native.MiniAudio.MaResourceManagerPtr.DataBufferBstLock { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18064) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-01525d399af95f55"></a>[`public ref Inno.Native.MiniAudio.MaDefaultVfs Inno.Native.MiniAudio.MaResourceManagerPtr.DefaultVfs { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18067) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cc91181a314a8270"></a>[`public bool Inno.Native.MiniAudio.MaResourceManagerPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18049) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-410bf23c966d08b9"></a>[`public Inno.Native.MiniAudio.MaResourceManager Inno.Native.MiniAudio.MaResourceManagerPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18052) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f7110999b867ea77"></a>[`public ref Inno.Native.MiniAudio.MaJobQueue Inno.Native.MiniAudio.MaResourceManagerPtr.JobQueue { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18066) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-45bf09f868bcb088"></a>[`public System.Span<nint> Inno.Native.MiniAudio.MaResourceManagerPtr.JobThreads { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18065) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c5916772c5af1fbf"></a>[`public ref Inno.Native.MiniAudio.MaLog Inno.Native.MiniAudio.MaResourceManagerPtr.Log { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18068) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6fe4610d0a2f0c20"></a>[`public static Inno.Native.MiniAudio.MaResourceManagerPtr Inno.Native.MiniAudio.MaResourceManagerPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18050) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-71a31ce2b0ba68c2"></a>[`public ref Inno.Native.MiniAudio.MaResourceManagerDataBufferNodePtr Inno.Native.MiniAudio.MaResourceManagerPtr.PRootDataBufferNode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18063) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ee4a404bd5dad78e"></a>[`public struct Inno.Native.MiniAudio.MaResourceManagerPtr : System.IEquatable<Inno.Native.MiniAudio.MaResourceManagerPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18041) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaResult</code> · 72 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaResult e6a336898190005bc691cb82bcc705caec40111be67aba472d711cd13b300b71 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.Success 299d628f13b5a724b0e32f0a6a7b4d437eb1c00ebf6944b30ff93d3c2046cb51 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.Error 5b40e0cf07045782dc0d2cbc7452621b3081e2810b584c586e3d6ed62394c8cf -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.InvalidArgs 9a6a84283d7dae0d848f54ef9001df0203f468577fcad00b362273c5aca79a80 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.InvalidOperation 7a9a0da1767057449e21c602660d9a51b4f67ea12e3e5673f9db60dbbe63fd9e -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.OutOfMemory 494262e16866d986f84840edd502a5414e8b71fa9152d5bbe0815d8d45dc39f6 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.OutOfRange 122710b7bb57f6078253e867f015a7b0ac174a54dcaebab86d552e24cda5591a -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.AccessDenied ab90a3256696cbe94b1c19b110c951bad60ffcf570df5d412ddba344b0509df2 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.DoesNotExist 6f5e0bcf465d090c6cf7fb86547ce1db4c81e204fb08a0078d370eebf31f51aa -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.AlreadyExists be89f63c1d8325521f428b7dd70ad733e858caf45297669c9640eddd7e83cffb -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.TooManyOpenFiles 141618fcbb0cb54e67e7d970b71a15734be37b4e916e9c38dab05eea013b2e3d -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.InvalidFile 73d04cc091bf7c4f2505d8d1672d1190327f13e8eaaa8872da1e4c4ed8d85453 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.TooBig 273dd1f01eef8e9e40261584e90d9043b892137a4023023a937c0e297b4a0761 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.PathTooLong cf6f9d18c6f6ba2d442749ed1ddac4eb8ef1a30c6490e8e6b8823341e0478087 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NameTooLong 30118c3de7236996cf1d915c42c697e8d2279c278d4a9687e8567371c87dc05f -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NotDirectory 2370e111cdc575a4bdfedc0f81bcaeff9f5506f6e5476a5db93b5c3057e8259f -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.IsDirectory b29889ac352bab880d5a47491da31647bef72c739f90b8ba53180cc4cffa57b5 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.DirectoryNotEmpty be5e62fbe348631b6177a28b72260403237c594ee102f61e87beb42754bce1c9 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.AtEnd 85436201ab7490b8f48ce89b85b5101626b12442b544f885951e54a9a8f52ff8 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NoSpace 07bb6e62229f3a23ff8de0d21cdb0698e109a3f20aadfb7bf5e1a31199e66027 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.Busy 3bfd5f2ac8221963f8efe54cef6f585306bc2cbcce0c6d517d5ad3afe0908f78 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.IoError 9c07dbe1f787382649bf74dd2d2c09ddc34085f9369c1538c513c63bae2ce898 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.Interrupt 739ca9c7112f51a2ad62638a1dd8f367273e856a3f6b675948f3c566b8d919c9 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.Unavailable 2bd59639069ac231808e9ce22ed876bc83ce8a86cee170c80a6ca63410578be8 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.AlreadyInUse 05d3686f4e9b3a78916af74492183d4158ca67863ae892882959203e42f53546 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.BadAddress 4b9ad2f47b6931ed97e207c947390768b08a6fa1b989c677936e9c9798cf70e0 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.BadSeek 44bfeafd058aa25cc3088606503397c58536212ae973a722f704b55cb95f87de -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.BadPipe 3f02091791274ae2c31310f2a35779404cbd636f3104242617ce9f36ff350397 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.Deadlock 758a55674828ce88f6ee78f7bf352db4fa6cd5483a6bca4ca8d251c3a326e090 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.TooManyLinks b1476587be2e8481e326bdfbc2f1e3655b37260ae36be7ff04bc5c3bd52d511b -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NotImplemented 8d6efe905e185ba5cec069447f8767084c7802a7b13116111c15ee8319d2b54b -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NoMessage 253d9a57d70216c49d2ed930358b7625fa485a34b71b90cb3db6cb9cdf53c20d -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.BadMessage 3ab36530b838f65ea8f63daf058e2d65f33dc17ebc5a910cbf6902fdc389b04e -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NoDataAvailable ca8e220de9dfbbf255f1052e0b455d4822e00e88604d78cfeccdf60c1895559c -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.InvalidData da8efc92afbba73a402fd1a369fc04eff3e128e10aa90e575ee94141d79ce67e -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.Timeout 619dd3f1e8f34017711f5810e219a89aa8eb9d040e4f35b3d4437d89b16d8493 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NoNetwork 30f4cbb78770bdb4880efbd5ce15cc8f83ed01eabb27d8c64633a772d0d5b5d0 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NotUnique 3adc857fbdd570747f2391f8302b1d9f4c3a75d6334aa6c615a3a27bb0426538 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NotSocket 7bccb4b9257c05d235a12125b9a9994a19abc70d0171013f692c3e2a2ebf7ea1 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NoAddress 53aad2c62d9d7ac04976f6cf1d0b2f65fd412ab761a471238d2b9546cdd8fe12 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.BadProtocol 4ce055e18567bca2a1baa3699b6e5f929eafbda242e2c39452a1b59d728b4440 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.ProtocolUnavailable daa946b7f564a0d110590d506572d313ebb71c1ad79704a14f3b8f2ba2f14c72 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.ProtocolNotSupported 9ddf2316b73bd9b6574ef278fca6ba31fd848c2dd880e93c08b85c106aa18090 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.ProtocolFamilyNotSupported 6ed96b272a5962774636517acb346d62b222818c15be11f70bf5c4026c558def -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.AddressFamilyNotSupported 8c4ce8e9dc2c5a6d57bfd60df1f4e656240238ef89d033c82f923ec6928e52f2 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.SocketNotSupported 2ad647072099e049c29a576e0aef36a3c5488448723ee2ab8c82847f75c97444 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.ConnectionReset f559b7447fa9e796548abd60700da70ed442bd165b220cc77bebd4e7455d629e -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.AlreadyConnected cead23daeb0c46706b916caaaafdb2247ca1ae24b9322c4d271b726dae60b19c -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NotConnected faf316f145dfcf1f2e5503cb3b4b128e5a08b589f5a2d6bbb7bbd5187a11ea09 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.ConnectionRefused c4ba2c9dda988a8e3e42896c9a7e21c4bae6aa2f41aff7bf6cf97a35c514ebad -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NoHost e514c3d0d40a5b2871a2c14814238be279c6fb4524c353fed8cf22526bfb388a -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.InProgress 55154f05170ed78c8708e2298a670a57dd069722783d1301275f9863855cb4a4 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.Cancelled 22daf6dd9e4f35f40f02392945b4e5f961bd006b90548bb30ab8691055101a3e -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.MemoryAlreadyMapped eba64b1aff27ccd88648a73b300eac977dcba2993faabde9195bbead9ef0b0cb -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.CrcMismatch 3fd5972e873663f25a9a55948cdf064010f8028d72542881a0159dadc1690f67 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.FormatNotSupported c1271cabaa207c6f4addb0aa17ec0e56cb6dde360ec56e6e1e066201bbc0f67b -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.DeviceTypeNotSupported 4b25e1a26ee0bb4667261c5a08ac77fd9c2e7f16ef47fb9aa6a044b51e927ded -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.ShareModeNotSupported d2ad70ca777e583cbea40fee08d4b60aaf29fd7f91d3798e0cb1349e7eba8ca0 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NoBackend 9031b98e81e6747fa0c6a423135ef9f28505977695c9e2972fb00600d23514cd -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.NoDevice a27c518c21c72c4971f10aeb3e8d0d8d11c4de5cd3e080d386fefae54b4fd2e6 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.ApiNotFound 8d6e54d40cd3b9c8fda22c5fc012796c49c1f35be3f5e72f872141f66be123c6 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.InvalidDeviceConfig b530879e57a7df638f3019a293963fd3ecb0c48c89b999be97c65f1ab0a72e6e -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.Loop 8713393bf9b3daa69c2c6de116111eb96506e08cf31ebad67dc3ff8c5abecd1b -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.BackendNotEnabled fab378608083b9eb902dd2ac4aad5694b0aa28b66a358cf8050af7d51d4c5568 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.DeviceNotInitialized ec07cb4cea24bfd872b69e3a3d55ad4d039b3b5e23d58b5a1043dd1bba3b6651 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.DeviceAlreadyInitialized 6b0e0d381363691beec7cfbd261279319bfadd91de0e26528845fdcde064f24e -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.DeviceNotStarted 8138236cbfeae771a5b923315fdac20986793a192c9d5c210b4d59d098556781 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.DeviceNotStopped 7460b347c4b756a0a69bf26fe199b80b6e33440c73fa668ec64e5394b2f07c17 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.FailedToInitBackend ccc8aa7a9a78b4d44eb4c385502725f43af747387d4a0a4cb32720d35dfa20e2 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.FailedToOpenBackendDevice 35dc82877eedced4b5937ac0faa9d48b69ab7ecb2ad4d78e0a2af755d2003a31 -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.FailedToStartBackendDevice a0ebad8e75d74b20386fa03ed6ca70598e5d0b628333e00024341bec1bb3dbca -->
<!-- api: F:Inno.Native.MiniAudio.MaResult.FailedToStopBackendDevice cae88942218b35437df7cb39546a3a1a680a06e5700d018b3a801fbf227ecef5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5032b04deedb3ef3"></a>[`Inno.Native.MiniAudio.MaResult.AccessDenied = -6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L72) | 当前常量或枚举值：`-6`。 | — |
| <a id="api-2c70b945f5a44869"></a>[`Inno.Native.MiniAudio.MaResult.AddressFamilyNotSupported = -43`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L109) | 当前常量或枚举值：`-43`。 | — |
| <a id="api-4235d0d8ac7bb6c2"></a>[`Inno.Native.MiniAudio.MaResult.AlreadyConnected = -46`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L112) | 当前常量或枚举值：`-46`。 | — |
| <a id="api-03a4de84074557ab"></a>[`Inno.Native.MiniAudio.MaResult.AlreadyExists = -8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L74) | 当前常量或枚举值：`-8`。 | — |
| <a id="api-0a70886a05192d67"></a>[`Inno.Native.MiniAudio.MaResult.AlreadyInUse = -23`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L89) | 当前常量或枚举值：`-23`。 | — |
| <a id="api-06021a4cdd2925cc"></a>[`Inno.Native.MiniAudio.MaResult.ApiNotFound = -205`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L125) | 当前常量或枚举值：`-205`。 | — |
| <a id="api-ae72802fee2f3a4d"></a>[`Inno.Native.MiniAudio.MaResult.AtEnd = -17`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L83) | 当前常量或枚举值：`-17`。 | — |
| <a id="api-c9a81eda7d0a2da2"></a>[`Inno.Native.MiniAudio.MaResult.BackendNotEnabled = -208`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L128) | 当前常量或枚举值：`-208`。 | — |
| <a id="api-5560d7eb7abd5c6e"></a>[`Inno.Native.MiniAudio.MaResult.BadAddress = -24`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L90) | 当前常量或枚举值：`-24`。 | — |
| <a id="api-9d4c247fd3e94d09"></a>[`Inno.Native.MiniAudio.MaResult.BadMessage = -31`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L97) | 当前常量或枚举值：`-31`。 | — |
| <a id="api-9b2ede0ce163dc55"></a>[`Inno.Native.MiniAudio.MaResult.BadPipe = -26`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L92) | 当前常量或枚举值：`-26`。 | — |
| <a id="api-0ce5514c297560aa"></a>[`Inno.Native.MiniAudio.MaResult.BadProtocol = -39`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L105) | 当前常量或枚举值：`-39`。 | — |
| <a id="api-6023293255e9cbf6"></a>[`Inno.Native.MiniAudio.MaResult.BadSeek = -25`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L91) | 当前常量或枚举值：`-25`。 | — |
| <a id="api-f322c00b46509e6b"></a>[`Inno.Native.MiniAudio.MaResult.Busy = -19`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L85) | 当前常量或枚举值：`-19`。 | — |
| <a id="api-1ff7ed839dddc97d"></a>[`Inno.Native.MiniAudio.MaResult.Cancelled = -51`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L117) | 当前常量或枚举值：`-51`。 | — |
| <a id="api-5a974cbb729d1095"></a>[`Inno.Native.MiniAudio.MaResult.ConnectionRefused = -48`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L114) | 当前常量或枚举值：`-48`。 | — |
| <a id="api-3047c1dadf6ec96a"></a>[`Inno.Native.MiniAudio.MaResult.ConnectionReset = -45`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L111) | 当前常量或枚举值：`-45`。 | — |
| <a id="api-656a6a321a95db0a"></a>[`Inno.Native.MiniAudio.MaResult.CrcMismatch = -100`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L119) | 当前常量或枚举值：`-100`。 | — |
| <a id="api-001b53f8310ed5e6"></a>[`Inno.Native.MiniAudio.MaResult.Deadlock = -27`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L93) | 当前常量或枚举值：`-27`。 | — |
| <a id="api-895724be185090d7"></a>[`Inno.Native.MiniAudio.MaResult.DeviceAlreadyInitialized = -301`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L130) | 当前常量或枚举值：`-301`。 | — |
| <a id="api-8e5a2bc147dc9df4"></a>[`Inno.Native.MiniAudio.MaResult.DeviceNotInitialized = -300`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L129) | 当前常量或枚举值：`-300`。 | — |
| <a id="api-8fc68a75134b4366"></a>[`Inno.Native.MiniAudio.MaResult.DeviceNotStarted = -302`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L131) | 当前常量或枚举值：`-302`。 | — |
| <a id="api-74b75cd694999281"></a>[`Inno.Native.MiniAudio.MaResult.DeviceNotStopped = -303`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L132) | 当前常量或枚举值：`-303`。 | — |
| <a id="api-05072d5affe9f2c9"></a>[`Inno.Native.MiniAudio.MaResult.DeviceTypeNotSupported = -201`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L121) | 当前常量或枚举值：`-201`。 | — |
| <a id="api-a07398501adc79b8"></a>[`Inno.Native.MiniAudio.MaResult.DirectoryNotEmpty = -16`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L82) | 当前常量或枚举值：`-16`。 | — |
| <a id="api-1e730ef691b07852"></a>[`Inno.Native.MiniAudio.MaResult.DoesNotExist = -7`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L73) | 当前常量或枚举值：`-7`。 | — |
| <a id="api-b24e386650585345"></a>[`Inno.Native.MiniAudio.MaResult.Error = -1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L67) | 当前常量或枚举值：`-1`。 | — |
| <a id="api-a60b3eb636ba951e"></a>[`Inno.Native.MiniAudio.MaResult.FailedToInitBackend = -400`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L133) | 当前常量或枚举值：`-400`。 | — |
| <a id="api-0ec44d7731ad8276"></a>[`Inno.Native.MiniAudio.MaResult.FailedToOpenBackendDevice = -401`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L134) | 当前常量或枚举值：`-401`。 | — |
| <a id="api-aa405796cf34cb46"></a>[`Inno.Native.MiniAudio.MaResult.FailedToStartBackendDevice = -402`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L135) | 当前常量或枚举值：`-402`。 | — |
| <a id="api-45f942eca52f1170"></a>[`Inno.Native.MiniAudio.MaResult.FailedToStopBackendDevice = -403`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L136) | 当前常量或枚举值：`-403`。 | — |
| <a id="api-fd328ad96472e0a3"></a>[`Inno.Native.MiniAudio.MaResult.FormatNotSupported = -200`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L120) | 当前常量或枚举值：`-200`。 | — |
| <a id="api-8992d367f34315d0"></a>[`Inno.Native.MiniAudio.MaResult.InProgress = -50`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L116) | 当前常量或枚举值：`-50`。 | — |
| <a id="api-f64f7cacf3ce1d06"></a>[`Inno.Native.MiniAudio.MaResult.Interrupt = -21`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L87) | 当前常量或枚举值：`-21`。 | — |
| <a id="api-896dd967818a6fd8"></a>[`Inno.Native.MiniAudio.MaResult.InvalidArgs = -2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L68) | 当前常量或枚举值：`-2`。 | — |
| <a id="api-ebe8e7389fc3c85d"></a>[`Inno.Native.MiniAudio.MaResult.InvalidData = -33`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L99) | 当前常量或枚举值：`-33`。 | — |
| <a id="api-2f5896cd74d7ebe6"></a>[`Inno.Native.MiniAudio.MaResult.InvalidDeviceConfig = -206`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L126) | 当前常量或枚举值：`-206`。 | — |
| <a id="api-a9359127520f5d00"></a>[`Inno.Native.MiniAudio.MaResult.InvalidFile = -10`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L76) | 当前常量或枚举值：`-10`。 | — |
| <a id="api-5b4ba2d53ced9ba5"></a>[`Inno.Native.MiniAudio.MaResult.InvalidOperation = -3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L69) | 当前常量或枚举值：`-3`。 | — |
| <a id="api-84b554c7c2f10b74"></a>[`Inno.Native.MiniAudio.MaResult.IoError = -20`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L86) | 当前常量或枚举值：`-20`。 | — |
| <a id="api-71ffa61fa646990b"></a>[`Inno.Native.MiniAudio.MaResult.IsDirectory = -15`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L81) | 当前常量或枚举值：`-15`。 | — |
| <a id="api-d5afe2d9b1a394aa"></a>[`Inno.Native.MiniAudio.MaResult.Loop = -207`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L127) | 当前常量或枚举值：`-207`。 | — |
| <a id="api-03aa1246299df26b"></a>[`Inno.Native.MiniAudio.MaResult.MemoryAlreadyMapped = -52`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L118) | 当前常量或枚举值：`-52`。 | — |
| <a id="api-6bfc885e15b14d97"></a>[`Inno.Native.MiniAudio.MaResult.NameTooLong = -13`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L79) | 当前常量或枚举值：`-13`。 | — |
| <a id="api-2dba1ae5b6231a76"></a>[`Inno.Native.MiniAudio.MaResult.NoAddress = -38`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L104) | 当前常量或枚举值：`-38`。 | — |
| <a id="api-9ac10d028d4af46e"></a>[`Inno.Native.MiniAudio.MaResult.NoBackend = -203`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L123) | 当前常量或枚举值：`-203`。 | — |
| <a id="api-65692a76eff32754"></a>[`Inno.Native.MiniAudio.MaResult.NoDataAvailable = -32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L98) | 当前常量或枚举值：`-32`。 | — |
| <a id="api-5daf429b879c14bd"></a>[`Inno.Native.MiniAudio.MaResult.NoDevice = -204`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L124) | 当前常量或枚举值：`-204`。 | — |
| <a id="api-03588ced5e385663"></a>[`Inno.Native.MiniAudio.MaResult.NoHost = -49`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L115) | 当前常量或枚举值：`-49`。 | — |
| <a id="api-30d96c0e143e3f99"></a>[`Inno.Native.MiniAudio.MaResult.NoMessage = -30`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L96) | 当前常量或枚举值：`-30`。 | — |
| <a id="api-408437a407a72e04"></a>[`Inno.Native.MiniAudio.MaResult.NoNetwork = -35`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L101) | 当前常量或枚举值：`-35`。 | — |
| <a id="api-1df14a8a4a198f6a"></a>[`Inno.Native.MiniAudio.MaResult.NoSpace = -18`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L84) | 当前常量或枚举值：`-18`。 | — |
| <a id="api-c20911640ea7899e"></a>[`Inno.Native.MiniAudio.MaResult.NotConnected = -47`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L113) | 当前常量或枚举值：`-47`。 | — |
| <a id="api-ca9ddff33949f0c3"></a>[`Inno.Native.MiniAudio.MaResult.NotDirectory = -14`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L80) | 当前常量或枚举值：`-14`。 | — |
| <a id="api-ff2d11dbb2f28437"></a>[`Inno.Native.MiniAudio.MaResult.NotImplemented = -29`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L95) | 当前常量或枚举值：`-29`。 | — |
| <a id="api-cd5f278cfef16bd2"></a>[`Inno.Native.MiniAudio.MaResult.NotSocket = -37`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L103) | 当前常量或枚举值：`-37`。 | — |
| <a id="api-fe291bf3f0e2950c"></a>[`Inno.Native.MiniAudio.MaResult.NotUnique = -36`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L102) | 当前常量或枚举值：`-36`。 | — |
| <a id="api-1368c02b4fb4162b"></a>[`Inno.Native.MiniAudio.MaResult.OutOfMemory = -4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L70) | 当前常量或枚举值：`-4`。 | — |
| <a id="api-c671df4c0fd5e8c0"></a>[`Inno.Native.MiniAudio.MaResult.OutOfRange = -5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L71) | 当前常量或枚举值：`-5`。 | — |
| <a id="api-a3a2782e1b739934"></a>[`Inno.Native.MiniAudio.MaResult.PathTooLong = -12`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L78) | 当前常量或枚举值：`-12`。 | — |
| <a id="api-a2543ebb40af4c99"></a>[`Inno.Native.MiniAudio.MaResult.ProtocolFamilyNotSupported = -42`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L108) | 当前常量或枚举值：`-42`。 | — |
| <a id="api-cdefe29f2ca2b987"></a>[`Inno.Native.MiniAudio.MaResult.ProtocolNotSupported = -41`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L107) | 当前常量或枚举值：`-41`。 | — |
| <a id="api-38fecd2381a5d63e"></a>[`Inno.Native.MiniAudio.MaResult.ProtocolUnavailable = -40`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L106) | 当前常量或枚举值：`-40`。 | — |
| <a id="api-5e50b367581b22e2"></a>[`Inno.Native.MiniAudio.MaResult.ShareModeNotSupported = -202`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L122) | 当前常量或枚举值：`-202`。 | — |
| <a id="api-87b591571fd4187a"></a>[`Inno.Native.MiniAudio.MaResult.SocketNotSupported = -44`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L110) | 当前常量或枚举值：`-44`。 | — |
| <a id="api-9663bdae4f8cc7fb"></a>[`Inno.Native.MiniAudio.MaResult.Success = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L66) | 当前常量或枚举值：`0`。 | — |
| <a id="api-49072804c27b6ea1"></a>[`Inno.Native.MiniAudio.MaResult.Timeout = -34`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L100) | 当前常量或枚举值：`-34`。 | — |
| <a id="api-5c79c4f6550ba3c9"></a>[`Inno.Native.MiniAudio.MaResult.TooBig = -11`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L77) | 当前常量或枚举值：`-11`。 | — |
| <a id="api-a5079b6222bba2ab"></a>[`Inno.Native.MiniAudio.MaResult.TooManyLinks = -28`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L94) | 当前常量或枚举值：`-28`。 | — |
| <a id="api-1d095fcc84f6a79b"></a>[`Inno.Native.MiniAudio.MaResult.TooManyOpenFiles = -9`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L75) | 当前常量或枚举值：`-9`。 | — |
| <a id="api-ceb72a9d4c594dbb"></a>[`Inno.Native.MiniAudio.MaResult.Unavailable = -22`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L88) | 当前常量或枚举值：`-22`。 | — |
| <a id="api-f650295e33f003aa"></a>[`public enum Inno.Native.MiniAudio.MaResult : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L64) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSeekOrigin</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSeekOrigin 5301b9d072afc6ed94c304386e6beb3944cfd7d15d597fe145211b24d4290589 -->
<!-- api: F:Inno.Native.MiniAudio.MaSeekOrigin.Start a59a87ed88b8b724c43304cb4118d3857b121311c4d2fececec459b577220759 -->
<!-- api: F:Inno.Native.MiniAudio.MaSeekOrigin.Current 05edfe650b1d177c55ae415fcd432266637f084f3556418bbb2743188f6dcafe -->
<!-- api: F:Inno.Native.MiniAudio.MaSeekOrigin.End 30f08bf531fbd135a0389e790c4ab611215594778a02f724c441d6658ded956c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-37e77201db847d9b"></a>[`Inno.Native.MiniAudio.MaSeekOrigin.Current = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L638) | 当前常量或枚举值：`1`。 | — |
| <a id="api-63693bb81daa8bfb"></a>[`Inno.Native.MiniAudio.MaSeekOrigin.End = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L639) | 当前常量或枚举值：`2`。 | — |
| <a id="api-d30d05d95fd7b4f4"></a>[`Inno.Native.MiniAudio.MaSeekOrigin.Start = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L637) | 当前常量或枚举值：`0`。 | — |
| <a id="api-8b4e70bec707ef0d"></a>[`public enum Inno.Native.MiniAudio.MaSeekOrigin : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L635) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSeekProc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSeekProc 3efa5f6b32f6063f77a4b2a01d4d3f4a63d854b4c0af998b49e80846ce550d27 -->
<!-- api: M:Inno.Native.MiniAudio.MaSeekProc.#ctor(System.Object,System.IntPtr) fe2ec1c022cd3612e3621724109fd0afd9b91be6a07b56050bc2269104b8eb92 -->
<!-- api: M:Inno.Native.MiniAudio.MaSeekProc.Invoke(System.Void*,System.Int64,Inno.Native.MiniAudio.MaSeekOrigin) 4513eab6d2893b4e83cd3fa71d00cb15e0d1f22e5050901ccb3887438c931fe0 -->
<!-- api: M:Inno.Native.MiniAudio.MaSeekProc.BeginInvoke(System.Void*,System.Int64,Inno.Native.MiniAudio.MaSeekOrigin,System.AsyncCallback,System.Object) fe1ac1ceeb0f8cd4f3ba838aee16d07f4a45ff2b796fed73eed91a34c357161d -->
<!-- api: M:Inno.Native.MiniAudio.MaSeekProc.EndInvoke(System.IAsyncResult) aa758edf0540ca8ead97a6fb8fe8c99e86dd2ad071496e4786e578aefcd611f8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8d9ffddfe00c2c06"></a>[`public Inno.Native.MiniAudio.MaSeekProc.MaSeekProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14161) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-875a0091c39444f0"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaSeekProc.BeginInvoke(void* pUserData, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14161) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ee0da95882174a5b"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaSeekProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14161) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bd771c7379db2ffa"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaSeekProc.Invoke(void* pUserData, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14161) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f1cd8abbb7f327d2"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaSeekProc(void* pUserData, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14161) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaShareMode</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaShareMode c1fb0e17d082ca6c97ed866d54c5e5142f28f1d257b42f44faf15739d91b161d -->
<!-- api: F:Inno.Native.MiniAudio.MaShareMode.Shared 6677a7b21e98c86756b4fc3d44e4e8ae6fd579ea138c497475021a956f3f02da -->
<!-- api: F:Inno.Native.MiniAudio.MaShareMode.Exclusive 3679dcc0156c7c080a2c55b5364c14e791e049b6e9144af9c2c042819928a9a5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dde9156ee4da6908"></a>[`Inno.Native.MiniAudio.MaShareMode.Exclusive = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L234) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d584bb647eb3438a"></a>[`Inno.Native.MiniAudio.MaShareMode.Shared = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L233) | 当前常量或枚举值：`0`。 | — |
| <a id="api-1a6fefee69bc1634"></a>[`public enum Inno.Native.MiniAudio.MaShareMode : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L231) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSlotAllocator</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSlotAllocator ef7f60dfc2064cfdcd6765b97fb15b5de127c42f21dd9b933874912eb01d1883 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocator.PGroups 1dbb57d6ee0cc1dbad63d43ac858b97d29a4aecc79cd9be1be14699b58f90f08 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocator.PSlots 24b6d1a8631cbe76c111a606f983eeaa63c8620bb65890d60af986f94b1be482 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocator.Count afe311c497ad5fb004ec54359eedda411b62947e1ae428671a4a9391bd01b715 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocator.Capacity 3c9a1df30431d566bca600bcc3375d6c21ad28121ad588138d64232f723b755a -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocator.Ownsheap e23b84d012f4d94002d3e06707c3b525602440452a32b868bd9add344a970205 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocator.Pheap 104e683d63cab84a26a193c2983e4cf873166709daa0d4aaadd6aa4ba3be8209 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocator.#ctor(Inno.Native.MiniAudio.MaSlotAllocatorGroup*,System.UInt32*,System.UInt32,System.UInt32,System.UInt32,System.Void*) c0851ca53565921c848132f62181b31db2cf08295f24ea0df06452bbd25d555b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-bc548c2258adb49a"></a>[`public uint Inno.Native.MiniAudio.MaSlotAllocator.Capacity`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11550) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c6283ceb33fd832a"></a>[`public uint Inno.Native.MiniAudio.MaSlotAllocator.Count`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11549) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-16e122112de4a500"></a>[`public uint Inno.Native.MiniAudio.MaSlotAllocator.Ownsheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11551) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed0ab0f32cbd54c6"></a>[`public Inno.Native.MiniAudio.MaSlotAllocatorGroup* Inno.Native.MiniAudio.MaSlotAllocator.PGroups`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11547) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a121f443813d8a81"></a>[`public void* Inno.Native.MiniAudio.MaSlotAllocator.Pheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11552) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d0b3915562d799bd"></a>[`public uint* Inno.Native.MiniAudio.MaSlotAllocator.PSlots`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11548) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07147426454bfaee"></a>[`public Inno.Native.MiniAudio.MaSlotAllocator.MaSlotAllocator(Inno.Native.MiniAudio.MaSlotAllocatorGroup* pGroups = null, uint* pSlots = null, uint count = 0, uint capacity = 0, uint ownsHeap = 0, void* pHeap = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11553) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f9ee0b16ac433536"></a>[`public struct Inno.Native.MiniAudio.MaSlotAllocator`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11545) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSlotAllocatorConfig</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSlotAllocatorConfig 38a4e32e6ee5c9f6d891f7c6046dd87c3a6797c86888a8bc239b3fc1664e02b3 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocatorConfig.Capacity d40d3efd8bc5698aafa34e59c4ae921fc15bd3a0213d661c7f20506b868b3613 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorConfig.#ctor(System.UInt32) 626008d452d6891816f4f9a1a9ce2b6e1761112b7089e03853943a447dc6ad16 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-635bb79a779e7a43"></a>[`public uint Inno.Native.MiniAudio.MaSlotAllocatorConfig.Capacity`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11521) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af247d3cb3d36e05"></a>[`public Inno.Native.MiniAudio.MaSlotAllocatorConfig.MaSlotAllocatorConfig(uint capacity = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11522) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7173093e8eff7a7f"></a>[`public struct Inno.Native.MiniAudio.MaSlotAllocatorConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11519) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr b423354e6518e47c2395ecbcd50d8b08f11470813abaa6316c567bdcf4847bf5 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.#ctor(Inno.Native.MiniAudio.MaSlotAllocatorConfig*) 748840f49af54207f826e376c415f0720b50c78bb40b10c47b87e767fb27b781 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.Handle 8caa7595b14efae1e08a6cf7f2d889a3af05c0e78f9f30df0f4a606aee414ffc -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.IsNull 62a9e562be28972964120784b088e46ae4dd66048bd664c81a828b099f2c63ac -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.Null 63ce638e79ebdfc3bfca4ce996bf9651e168b41c2dfd5731ec77b90976a06d3a -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.Item(System.Int32) cda42182b6793f10d78b3777c57d488cbe9f2751a0e4e54dd94caada4d85922e -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaSlotAllocatorConfig*)~Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr 7c9d688711ea6a8c708efdbab8bb611578041a4a9664c70b41553db7e7067a68 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr)~Inno.Native.MiniAudio.MaSlotAllocatorConfig* fadf9cfccd83c5d3dbbc70a7abbe30d7630a7d49f0d54cc006cde43041d1ba88 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.op_Equality(Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr,Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr) 1f33329e174b8e8ce52cd5238bedc8e8e03e5d0d5b860eb7e2e47717bed45288 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr,Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr) cbb17f6ccf83c602dfbef162dd71c0f660b1f76d25aba136f7df8e468892df5b -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.Equals(Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr) 5136513ed5d5b2ecbd5d0957ea338ed1f89b791da33bcdeb9239d903ba2d8f70 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.Equals(System.Object) a5ed55d8328824c2497dddcbc74320e102c1440e6e89c6d05fc419670a769aee -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.GetHashCode aa14af6f49dfba640aa97091cd80d50905dfb834a24c18fa9be9c27e61a4756b -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.Capacity 22a5c54f43cbcac758f1feaac2921efcf08c96888f0bf808462cf84911b3d4aa -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-96b4561723778f94"></a>[`public Inno.Native.MiniAudio.MaSlotAllocatorConfig* Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18079) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-111446c58c0c1f48"></a>[`public Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.MaSlotAllocatorConfigPtr(Inno.Native.MiniAudio.MaSlotAllocatorConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18074) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-848170223bee7357"></a>[`public bool Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.Equals(Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18089) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-6bf840d84de49475"></a>[`public override bool Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18090) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-142c5e2efac51471"></a>[`public override int Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18091) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-6529973ef55775a5"></a>[`public static bool Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.operator ==(Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr left, Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18087) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9f2429e554d35c1f"></a>[`public static Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.implicit operator Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr(Inno.Native.MiniAudio.MaSlotAllocatorConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18085) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-69f6fdcc26e72a91"></a>[`public static Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.implicit operator Inno.Native.MiniAudio.MaSlotAllocatorConfig*(Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18086) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e6d96740ccc18274"></a>[`public static bool Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.operator !=(Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr left, Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18088) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dd1c0aa39a6b1ff5"></a>[`public ref uint Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.Capacity { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18093) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0fce776558abd545"></a>[`public bool Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18080) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-65ba61464cfe382d"></a>[`public Inno.Native.MiniAudio.MaSlotAllocatorConfig Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18083) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ffbcb222320a1a76"></a>[`public static Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18081) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e9e2686cb83d4ab9"></a>[`public struct Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaSlotAllocatorConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18072) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSlotAllocatorGroup</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSlotAllocatorGroup a7a267d788de15ceb3402b858810802c618c709053891a35f4ac114b350e93b1 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocatorGroup.Bitfield 01644108b93c99e4ec6bfd028523108c87bff34e6d52e5830cd66c8ca5db3864 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorGroup.#ctor(System.UInt32) ff7c784a412f2a433a053ef8ff8d0d266ad7bd69a26cced870db8f29e84900ab -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1280e7ebdca0138e"></a>[`public uint Inno.Native.MiniAudio.MaSlotAllocatorGroup.Bitfield`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11534) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-008aa1ef73313521"></a>[`public Inno.Native.MiniAudio.MaSlotAllocatorGroup.MaSlotAllocatorGroup(uint bitfield = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11535) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d328bf7b31c41dfe"></a>[`public struct Inno.Native.MiniAudio.MaSlotAllocatorGroup`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11532) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr aba02f6bb5300d01c67b43c218198dab9795c36ae548af4ba1632f4810c12595 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.#ctor(Inno.Native.MiniAudio.MaSlotAllocatorGroup*) 089ad3c89dfe571b5f5dfc81f86a362c571d0658cda099aa6829d5e2aa36e97e -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.Handle 65eb6e5a49368d8fe67311a544f8645c9d74cb7d6b13232ee11dea93e2615ff2 -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.IsNull 833db2d9c8008cf0ac76f1056d15332f6e1946be5fdd68bf21a9353c323c5a3a -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.Null 9e5049a02354eddbe7ce3acade270ff32d2531fcb18528dbd1cccb5dc12f3e6e -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.Item(System.Int32) 8e7d80e804da654bc1d10dd833a8bdb59b0829c38ff8b455a503c3a5f810cbc2 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.op_Implicit(Inno.Native.MiniAudio.MaSlotAllocatorGroup*)~Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr 1d19d4db151c0cdf77b429822eb8f1ef4c372f0abc85f6793094be6c0be3e0e7 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.op_Implicit(Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr)~Inno.Native.MiniAudio.MaSlotAllocatorGroup* 2ce1dcbe4ae22c781b116979cddad8e801906a1778ad199bfb9c2d5e5c24e9cf -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.op_Equality(Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr,Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr) 20bcb3b2ceb3f193ae9ea42b74e226204b31721454ac2220fc7e44e1419fdfb1 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.op_Inequality(Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr,Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr) 82a77c5ea42099be0e720d5a07791036d980f219fd2fa9202374417859362bc0 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.Equals(Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr) 51442418dfdc0d7240bf5ccdfa033d1b3975fb38aa1da2b097701788a4584917 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.Equals(System.Object) b8ed4558401dde09d55d0204ec52b6a41f19b99a26aa2a8f821a9af0212d17f7 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.GetHashCode bbc318a4bbba3a36044ff0830832829532f99c623e8c35bc5a6c017723c50e00 -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.Bitfield d76edcb1518d06bbad9ea2f92ecc947002b030f5cd11ac991e6e29549ae93624 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-29a1611d995d9bf7"></a>[`public Inno.Native.MiniAudio.MaSlotAllocatorGroup* Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18104) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64c0e6ad30ceba81"></a>[`public Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.MaSlotAllocatorGroupPtr(Inno.Native.MiniAudio.MaSlotAllocatorGroup* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18099) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4f79249500992642"></a>[`public bool Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.Equals(Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18114) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-73f78af63d2c0597"></a>[`public override bool Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18115) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-654347c10f7f21a7"></a>[`public override int Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18116) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-295f005da5d6366d"></a>[`public static bool Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.operator ==(Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr left, Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18112) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0cbe96cd9300896b"></a>[`public static Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.implicit operator Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr(Inno.Native.MiniAudio.MaSlotAllocatorGroup* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18110) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d591ab5c34a3810d"></a>[`public static Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.implicit operator Inno.Native.MiniAudio.MaSlotAllocatorGroup*(Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18111) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ab9b55d5b82b8eac"></a>[`public static bool Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.operator !=(Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr left, Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18113) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-41c8c0075c747de8"></a>[`public ref uint Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.Bitfield { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18118) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ec99106003ffef5d"></a>[`public bool Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18105) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-63622ab271428a5c"></a>[`public Inno.Native.MiniAudio.MaSlotAllocatorGroup Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18108) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d2a9bc16810ee37f"></a>[`public static Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18106) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-635df32055ebbbb1"></a>[`public struct Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr : System.IEquatable<Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18097) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSlotAllocatorPtr</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSlotAllocatorPtr b2fff61af2fdbbd2d2720242e40b5083df6b31575f24d885862a0141cfd15117 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorPtr.#ctor(Inno.Native.MiniAudio.MaSlotAllocator*) 2c745495b92f0ceabd99c29fc1b07fb15d04815468ba2e896bd8fc63337a4fe6 -->
<!-- api: F:Inno.Native.MiniAudio.MaSlotAllocatorPtr.Handle bdf64ff5a718b71fbf0ef8057269b165ea61be4c97f8ccdc175819424e6fb2f2 -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorPtr.IsNull 8f8b0804529cdde57fa0d555d92a0bc7e0722e18a308971e75b61e2a00e0e528 -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorPtr.Null 68f0c06eb98d2dd967716af8231729e5c0bfc48d431009a1b4c34b4f99422758 -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorPtr.Item(System.Int32) 441fe9ce7d03daea1dd2163c80a54a457bd9036610a8f717e604e11776c53deb -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorPtr.op_Implicit(Inno.Native.MiniAudio.MaSlotAllocator*)~Inno.Native.MiniAudio.MaSlotAllocatorPtr d578cf766c1d743385e07c6130bf2075bd791f96f3eb79c1ceb9ca36155869af -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorPtr.op_Implicit(Inno.Native.MiniAudio.MaSlotAllocatorPtr)~Inno.Native.MiniAudio.MaSlotAllocator* 43c0dcd2dcaf1872299e025cf22e13571ed4883b4941476c0f3bef2bb96c7104 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorPtr.op_Equality(Inno.Native.MiniAudio.MaSlotAllocatorPtr,Inno.Native.MiniAudio.MaSlotAllocatorPtr) ebd18b791b2e02429ef8e8bb2c99dc750b822028fa1d48d908ccf6fc574dee4d -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorPtr.op_Inequality(Inno.Native.MiniAudio.MaSlotAllocatorPtr,Inno.Native.MiniAudio.MaSlotAllocatorPtr) fd0353e466316e95954ee235a619f549f280376b05702f5b2e7ca1ff86b6aee4 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorPtr.Equals(Inno.Native.MiniAudio.MaSlotAllocatorPtr) 1c24a5ead133ba5e8db79fd07286d4b193bab0b1ecb644568d432903c7c58600 -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorPtr.Equals(System.Object) 3cf4221647dc8e9be17feefc8a892c27513ea7f60e1f11f1556bf45570fc1c1c -->
<!-- api: M:Inno.Native.MiniAudio.MaSlotAllocatorPtr.GetHashCode 0551b54a90757a361b445c631bcc3778d6533b2aab031c67d37b78556efb86e4 -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorPtr.PGroups 56f77e909eee4d6cb69907401ed2f4848a180aad1eee6589f6515af4f06d7785 -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorPtr.PSlots 414d0890c1c0b8bf667ea84e101b5d6098b27f64865caadc22755b2196d731ef -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorPtr.Count 8a4b1bdbec590c712886c04abdc0b4bddb865fa87c52f099c3baa307c50a66da -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorPtr.Capacity 8538c08a1761333de6ec3e885018b6af52ff03ce2c10ed5ba30671ec424fd1f5 -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorPtr.Ownsheap a434e4d547c6f53aa9b545022486ac6835a3cbea03cd73459bec9de92d9e3df1 -->
<!-- api: P:Inno.Native.MiniAudio.MaSlotAllocatorPtr.Pheap 0fc7a171a08fa72bb0deb8a17e000818ea8b7d5e7de3efaf7b006b4b0a5d61f2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4ac67e82987e7ebc"></a>[`public Inno.Native.MiniAudio.MaSlotAllocator* Inno.Native.MiniAudio.MaSlotAllocatorPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18129) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2bde6764935af080"></a>[`public Inno.Native.MiniAudio.MaSlotAllocatorPtr.MaSlotAllocatorPtr(Inno.Native.MiniAudio.MaSlotAllocator* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18124) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-585e73c55f8c5c3f"></a>[`public bool Inno.Native.MiniAudio.MaSlotAllocatorPtr.Equals(Inno.Native.MiniAudio.MaSlotAllocatorPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18139) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-ce62d9bfe0d9eb8b"></a>[`public override bool Inno.Native.MiniAudio.MaSlotAllocatorPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18140) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-80f429879fa332d0"></a>[`public override int Inno.Native.MiniAudio.MaSlotAllocatorPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18141) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-c161d42a7c0feee6"></a>[`public static bool Inno.Native.MiniAudio.MaSlotAllocatorPtr.operator ==(Inno.Native.MiniAudio.MaSlotAllocatorPtr left, Inno.Native.MiniAudio.MaSlotAllocatorPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18137) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1d74901019154bd5"></a>[`public static Inno.Native.MiniAudio.MaSlotAllocatorPtr.implicit operator Inno.Native.MiniAudio.MaSlotAllocatorPtr(Inno.Native.MiniAudio.MaSlotAllocator* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18135) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5f739bc737740e7d"></a>[`public static Inno.Native.MiniAudio.MaSlotAllocatorPtr.implicit operator Inno.Native.MiniAudio.MaSlotAllocator*(Inno.Native.MiniAudio.MaSlotAllocatorPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18136) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-536441a037025dfa"></a>[`public static bool Inno.Native.MiniAudio.MaSlotAllocatorPtr.operator !=(Inno.Native.MiniAudio.MaSlotAllocatorPtr left, Inno.Native.MiniAudio.MaSlotAllocatorPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18138) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-985f000ff66cc43b"></a>[`public ref uint Inno.Native.MiniAudio.MaSlotAllocatorPtr.Capacity { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18146) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-56991fbf19b7a93a"></a>[`public ref uint Inno.Native.MiniAudio.MaSlotAllocatorPtr.Count { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18145) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-60e934357fa30bf1"></a>[`public bool Inno.Native.MiniAudio.MaSlotAllocatorPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18130) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-52275d0e8f16954c"></a>[`public Inno.Native.MiniAudio.MaSlotAllocator Inno.Native.MiniAudio.MaSlotAllocatorPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18133) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5f7f58b5c4407e01"></a>[`public static Inno.Native.MiniAudio.MaSlotAllocatorPtr Inno.Native.MiniAudio.MaSlotAllocatorPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18131) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9a8c533638874b99"></a>[`public ref uint Inno.Native.MiniAudio.MaSlotAllocatorPtr.Ownsheap { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18147) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f4f52a51173532ed"></a>[`public ref Inno.Native.MiniAudio.MaSlotAllocatorGroupPtr Inno.Native.MiniAudio.MaSlotAllocatorPtr.PGroups { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18143) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a2f04ee78ffde7cc"></a>[`public void* Inno.Native.MiniAudio.MaSlotAllocatorPtr.Pheap { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18148) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ef0edfc306d34603"></a>[`public uint* Inno.Native.MiniAudio.MaSlotAllocatorPtr.PSlots { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18144) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-55d231046d09efe8"></a>[`public struct Inno.Native.MiniAudio.MaSlotAllocatorPtr : System.IEquatable<Inno.Native.MiniAudio.MaSlotAllocatorPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18122) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSound</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSound 1b902aca94b5fa85b887e68a1531a307d11d621aef8013dc4145adfe9c112a94 -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.EngineNode 2b3d95297876acbb077455ec4f38e0865eef949c44e6c84499a4c45ebceba0df -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.PDataSource 3ee7861a8071d443a651e170a6642c7b77e6b02934c775afa485fe99af24cb3b -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.SeekTarget d3bfaf40fdd8df7a2d7258beef117d0171fa1ec8aaa329564a8d4735a912bc5c -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.AtEnd 6bb4480261c05cb596a931a5a17a3d7ad9f4d9c4c58c7d8206eb1998cccbce7c -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.EndCallback baf990622d952c98446ac60f2607dd42c8ff2ecd20d522daa31bc2c56830b378 -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.PEndCallbackUserData dfbeeada94f260f89ec9e3c398730080bd921374cd0945966a5ca286890437a7 -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.PProcessingCache 392388d21fb49622d30ed29d9fd0c9de93cbdff5873901f18e41af3a38339193 -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.ProcessingCacheFramesRemaining 8b8f3bf8431318065d3c1df22700d7da1ff590d0760812a2030fc978363bce3c -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.ProcessingCacheCap fc201c22b91178f520b8f27e21198145dc1e80f45c4e1c397cf076b0429f00d8 -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.OwnsDataSource ca39ac4bec562b3dd45791271f126677efe253e4f6a90684e452574549b4b3e6 -->
<!-- api: F:Inno.Native.MiniAudio.MaSound.PResourceManagerDataSource 4441aba8109c461c137e1056cd4f9b439ec53bd3d065485b4a5dc47263a43461 -->
<!-- api: M:Inno.Native.MiniAudio.MaSound.#ctor(Inno.Native.MiniAudio.MaEngineNode,Inno.Native.MiniAudio.MaDataSource*,System.UInt64,System.UInt32,System.Void*,System.Void*,System.Single*,System.UInt32,System.UInt32,System.Byte,Inno.Native.MiniAudio.MaResourceManagerDataSource*) fc69404ac2aa0043ce3020970c8a759bd87931cb6be51982478c0b5250da67c6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-118332f5e0575021"></a>[`public uint Inno.Native.MiniAudio.MaSound.AtEnd`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13612) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-05ae06c6c9af7229"></a>[`public void* Inno.Native.MiniAudio.MaSound.EndCallback`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13613) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-535b5b2b0dddbfa8"></a>[`public Inno.Native.MiniAudio.MaEngineNode Inno.Native.MiniAudio.MaSound.EngineNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13609) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0751cb39207623fa"></a>[`public byte Inno.Native.MiniAudio.MaSound.OwnsDataSource`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13618) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f62ae2c23253fe8b"></a>[`public Inno.Native.MiniAudio.MaDataSource* Inno.Native.MiniAudio.MaSound.PDataSource`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13610) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a30d07942bda1d46"></a>[`public void* Inno.Native.MiniAudio.MaSound.PEndCallbackUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13614) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-056a721044f20c0e"></a>[`public float* Inno.Native.MiniAudio.MaSound.PProcessingCache`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13615) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a961e6f04c3b4ec"></a>[`public Inno.Native.MiniAudio.MaResourceManagerDataSource* Inno.Native.MiniAudio.MaSound.PResourceManagerDataSource`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13619) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c1bce2347f7ec385"></a>[`public uint Inno.Native.MiniAudio.MaSound.ProcessingCacheCap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13617) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-627aa8bad44b07bb"></a>[`public uint Inno.Native.MiniAudio.MaSound.ProcessingCacheFramesRemaining`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13616) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-22ea76993677e163"></a>[`public ulong Inno.Native.MiniAudio.MaSound.SeekTarget`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13611) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-330510b4a2db08fe"></a>[`public Inno.Native.MiniAudio.MaSound.MaSound(Inno.Native.MiniAudio.MaEngineNode engineNode = default(Inno.Native.MiniAudio.MaEngineNode), Inno.Native.MiniAudio.MaDataSource* pDataSource = null, ulong seekTarget = 0, uint atEnd = 0, void* endCallback = null, void* pEndCallbackUserData = null, float* pProcessingCache = null, uint processingCacheFramesRemaining = 0, uint processingCacheCap = 0, byte ownsDataSource = 0, Inno.Native.MiniAudio.MaResourceManagerDataSource* pResourceManagerDataSource = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13620) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ffde3f0787e42290"></a>[`public struct Inno.Native.MiniAudio.MaSound`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13607) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSoundConfig</code> · 23 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSoundConfig 8efbc3d0ed4c4254ceb5d90cfb9669029f5c91405db18ee46eaf5a5b3679914b -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PFilePath f1adf339d13c458ed2116ccb6149c838376637bd1aa8c0b9b16141f4522eee2e -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PFilePathW 69b050ef30bebf9517717995e048b09da174fc1a02175508972b2580340e4e3f -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PDataSource 0e3e2652f2923f8a46ab575d5f83a63d17867eccbd5ce2b12b7672c7e8245f09 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PInitialAttachment a4672ed3dde31deb1fa22af704ff5bafbf8a1769380d8ea4dac078c00dd4aefb -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.InitialAttachmentInputBusIndex 51e00693da7f2e7ea7b358fa554f72f7660c1c1079b2ac1f5485bdfb53ebc00d -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.ChannelsIn 4da5e6088fec25fbfd0260dfa31bf2bab561907efc68b3ba8e39a1229c7eef3f -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.ChannelsOut 88358e176767ba1114cee7668f75d403781a94fa87658f9c1c1ed3386aa1bbc4 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.MonoExpansionMode 8b6ca813f2a8278994da17f6cbd99e50ff5f5f975b60935d215b1e7bf75156b6 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.Flags 8a5bedb58fdafbe58b84cc17322ab07b5737995b6e917a58489edf9ada8dd35d -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.VolumeSmoothTimeInPcmFrames db29eee5d830ff4fa34690907282ac384eec85b1ab091f55ffae96d5ba8a8e35 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.InitialSeekPointInPcmFrames 38ddc60b309b92347bc8e35cea62514bf677b6e11a5a4e7b2acc32f72ec65b38 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.RangeBegInPcmFrames e520e22d59cedc0eb4c75addc7ebd1243a0738f19dfb97995254f21490e3b232 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.RangeEndInPcmFrames 17be88fae2f39dd0425211360716e3734b0be89368d4626c20d6d6f2d9056275 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.LoopPointBegInPcmFrames badd30f43c6da396e4a0ec657a2116fec34e09c5249f8fe1da9b17c83ea39f11 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.LoopPointEndInPcmFrames 122eb0abba34c30b7d056e4c2f2528b8554afdd2d2de0e7cd16c023923efb95f -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.EndCallback 3c6d928e9fd8b15c1f1b753876f83aaaa0c56053cd9c9f1861c0605c8c25990d -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PEndCallbackUserData ecca5bf999e0400b9c9f5e1973b615023381967d8d1b8c56228e4eb6cd5a2306 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PitchResampling d573019e98df8cb5f38ffb1c80ae857c51eaeed486f57852d723eada23ddf265 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.InitNotifications 82d9f070e99c77baf4cf152767de5b86e6c10fa201528886a9dfeeb3d398dbe0 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.PDoneFence f075851e9db60f2bded2f97662bd6346e51517df5055f1fec70b0112b42a93c5 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfig.IsLooping b5331bad83393d934fe5ac1823f7372a4f0beacfeda6bc5428d0276316c0e444 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundConfig.#ctor(System.Byte*,System.UInt16*,Inno.Native.MiniAudio.MaDataSource*,System.Void*,System.UInt32,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaMonoExpansionMode,System.UInt32,System.UInt32,System.UInt64,System.UInt64,System.UInt64,System.UInt64,System.UInt64,System.Void*,System.Void*,Inno.Native.MiniAudio.MaResamplerConfig,Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications,Inno.Native.MiniAudio.MaFence*,System.UInt32) 52449ebca0a70f4eb15a05ad9c3cc51471a495b8aafad29a13c72401d1142ce4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0d4513afc4b6d766"></a>[`public uint Inno.Native.MiniAudio.MaSoundConfig.ChannelsIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13758) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ecd5c2a47015efb"></a>[`public uint Inno.Native.MiniAudio.MaSoundConfig.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13759) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7e415a42c4fddecd"></a>[`public void* Inno.Native.MiniAudio.MaSoundConfig.EndCallback`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13768) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-967ea5e5865cd5ce"></a>[`public uint Inno.Native.MiniAudio.MaSoundConfig.Flags`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13761) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b6ffc8f4e6e4ae7a"></a>[`public uint Inno.Native.MiniAudio.MaSoundConfig.InitialAttachmentInputBusIndex`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13757) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0232ff91ccfe5230"></a>[`public ulong Inno.Native.MiniAudio.MaSoundConfig.InitialSeekPointInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13763) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a737fe62308fe3b6"></a>[`public Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications Inno.Native.MiniAudio.MaSoundConfig.InitNotifications`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13771) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-728bb873b9a5b071"></a>[`public uint Inno.Native.MiniAudio.MaSoundConfig.IsLooping`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13773) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58c6eb93d24f3798"></a>[`public ulong Inno.Native.MiniAudio.MaSoundConfig.LoopPointBegInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13766) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be25fb476825ce2f"></a>[`public ulong Inno.Native.MiniAudio.MaSoundConfig.LoopPointEndInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13767) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5aa71b910d7dbf0a"></a>[`public Inno.Native.MiniAudio.MaMonoExpansionMode Inno.Native.MiniAudio.MaSoundConfig.MonoExpansionMode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13760) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-91e0c560f14f087e"></a>[`public Inno.Native.MiniAudio.MaDataSource* Inno.Native.MiniAudio.MaSoundConfig.PDataSource`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13755) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85865f40cf81473c"></a>[`public Inno.Native.MiniAudio.MaFence* Inno.Native.MiniAudio.MaSoundConfig.PDoneFence`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13772) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1671c65897be2efc"></a>[`public void* Inno.Native.MiniAudio.MaSoundConfig.PEndCallbackUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13769) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9ebabb33e000ddf1"></a>[`public byte* Inno.Native.MiniAudio.MaSoundConfig.PFilePath`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13753) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f9d147730c10f96"></a>[`public ushort* Inno.Native.MiniAudio.MaSoundConfig.PFilePathW`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13754) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-95ee0bfb3b87c1da"></a>[`public void* Inno.Native.MiniAudio.MaSoundConfig.PInitialAttachment`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13756) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39f51c64ea8914d2"></a>[`public Inno.Native.MiniAudio.MaResamplerConfig Inno.Native.MiniAudio.MaSoundConfig.PitchResampling`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13770) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d953433f1c420e69"></a>[`public ulong Inno.Native.MiniAudio.MaSoundConfig.RangeBegInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13764) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fbdd4c6626cc5b07"></a>[`public ulong Inno.Native.MiniAudio.MaSoundConfig.RangeEndInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13765) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ea1ea58bfc72b2e"></a>[`public uint Inno.Native.MiniAudio.MaSoundConfig.VolumeSmoothTimeInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13762) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-95221e14cb8bd650"></a>[`public Inno.Native.MiniAudio.MaSoundConfig.MaSoundConfig(byte* pFilePath = null, ushort* pFilePathW = null, Inno.Native.MiniAudio.MaDataSource* pDataSource = null, void* pInitialAttachment = null, uint initialAttachmentInputBusIndex = 0, uint channelsIn = 0, uint channelsOut = 0, Inno.Native.MiniAudio.MaMonoExpansionMode monoExpansionMode = Inno.Native.MiniAudio.MaMonoExpansionMode.Default, uint flags = 0, uint volumeSmoothTimeInPCMFrames = 0, ulong initialSeekPointInPCMFrames = 0, ulong rangeBegInPCMFrames = 0, ulong rangeEndInPCMFrames = 0, ulong loopPointBegInPCMFrames = 0, ulong loopPointEndInPCMFrames = 0, void* endCallback = null, void* pEndCallbackUserData = null, Inno.Native.MiniAudio.MaResamplerConfig pitchResampling = default(Inno.Native.MiniAudio.MaResamplerConfig), Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications initNotifications = default(Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications), Inno.Native.MiniAudio.MaFence* pDoneFence = null, uint isLooping = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13774) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-69e44e1ca454f6f0"></a>[`public struct Inno.Native.MiniAudio.MaSoundConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13751) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSoundConfigPtr</code> · 34 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSoundConfigPtr 0431f3964bc3cb20c800f6596bff1982eb498d947a441e704a4049e0c5bdd974 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundConfigPtr.#ctor(Inno.Native.MiniAudio.MaSoundConfig*) 850a6e57cc77005e7cdeff8f31b4b42876f90f15d8e68d91f37b4f90b34b0304 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundConfigPtr.Handle e635b7105d4b9e9fe04624b86ae43189026f3e4668e1cdd06b52343dd6a826ee -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.IsNull d8533cc89cec485a99d69c742e8e79b44afae76a2b4d39f8d25adec5cbd89d75 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.Null 519375eb34d07d4070264902ded381808f9ba10ed19d1ee551294a9d4eacdb2c -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.Item(System.Int32) f356196730115cd6ef0675141d3dce8a8d5121f2750cad774f08b6129cc65206 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaSoundConfig*)~Inno.Native.MiniAudio.MaSoundConfigPtr a7d1587801ca426a53024683a42c07d331e0b07a340457966ef825b344f0dff7 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaSoundConfigPtr)~Inno.Native.MiniAudio.MaSoundConfig* ca5adffecc2be0ae7c7c17f076039575fabfc7d2c973b83e90762b5dafd8851c -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundConfigPtr.op_Equality(Inno.Native.MiniAudio.MaSoundConfigPtr,Inno.Native.MiniAudio.MaSoundConfigPtr) 16194ef6b268a6fd496da65be4327cd5e278264c6ce637142752ebb59f359c58 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaSoundConfigPtr,Inno.Native.MiniAudio.MaSoundConfigPtr) 609135a4ca54eba38d4525cb7b2e8c4d87e813e4befa79c97d4f1da45bf96e0b -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundConfigPtr.Equals(Inno.Native.MiniAudio.MaSoundConfigPtr) f9ef4475d09205dfb6b35dd2af905abd752050cf9558e6a84fab6976725a17f2 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundConfigPtr.Equals(System.Object) b4b6a25a650595f7f4696d9b73b38541b945983c463bd1bad57fe933698eab67 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundConfigPtr.GetHashCode 3cd0ba4c53289953572684f9a91350c1a0172780f9e1b8ab9fc1a0a9b474d1a7 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.PFilePath 0047df8033e83fb921b088abac7599754688d577cc4e9008a9310ef4e62c1540 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.PFilePathW ce09fcd375333599f2653b1e43a18510a0df6aa3fc4883fdc510a82603a2c146 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.PDataSource 4af0ea5f5e7df2867ba127c06f26b0b0fb0a1b4c7f454758565d7ca7d81aa3f5 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.PInitialAttachment c3535feebf01cc97dd17a006975cec28541b6db07153ffe73a875a52b07715dc -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.InitialAttachmentInputBusIndex 21ca17ace1a7ad9c38ca5b9bb28f4512907de7b86fb3e5d46fd2cd511b5a8b1c -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.ChannelsIn 88e4f7267c78c1cdaf904016c198ec116975e8481fccffd8d5ff1c6e7f4cd262 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.ChannelsOut f1a466b12a5889b4f3bc7118d9d428ba0d96fbb934e77c1db32e98d11db9cd92 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.MonoExpansionMode 81128d6d0514bc0206d6eb721be61593eeaa291bb2629e275d2dad551f3c2ad7 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.Flags cb4d7106005f2c02355f3824933e7a05a368b4ff65780b22b8f6b543d29b1e94 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.VolumeSmoothTimeInPcmFrames 3cd4f90d92833fffb6b3f37632c75b8ce731a448838ac180b0d2bf8b458b3161 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.InitialSeekPointInPcmFrames a8a9b5c032a7938fe4fc9d2c5c1ca83a826a5829e127baef746cbe5693ec164c -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.RangeBegInPcmFrames 82b234c60f508e9de24151b693f76142d6e08347465eb9f57b909cfe826b91f5 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.RangeEndInPcmFrames 5684e50df6d02fa8a03d2d9ef34137a7385e127558179cb34b9ad48c963d94a9 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.LoopPointBegInPcmFrames fe43ee95f1a0c0092370b22333b28a853ac1a19a6f6744f29a1788465c02a1d7 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.LoopPointEndInPcmFrames f9e24ead5eade765489869860e37b1842996e98ba090a8bdf751384d57898103 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.EndCallback 1973bd6d1ddffbdd2c3719bdebc0a3f5dcb6646dd6885e6032f4ce2abb974ce0 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.PEndCallbackUserData 50131d67c34ed5fcfe46b1c8a62a67dc90ebecb37d60c5854bd31bc9728436be -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.PitchResampling 3d95f17d166b3873b9b8fc71f8e02288823e0e178f392b1016a8b62f621ed2a8 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.InitNotifications 914881cc1fb90a028d1073125b2512c794810dc4d92a4ed1787aa40d259aa2bf -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.PDoneFence e60506b5e6235c3636c8f49a8d5414b7a2e1999e10655abf8a8036bab4457ec2 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundConfigPtr.IsLooping f2962e77766758175f1bf4a50856fd102601cf431fc3fe9e7b55e5f674718482 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-cc14e12a989a2588"></a>[`public Inno.Native.MiniAudio.MaSoundConfig* Inno.Native.MiniAudio.MaSoundConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18159) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a787d97532a660a"></a>[`public Inno.Native.MiniAudio.MaSoundConfigPtr.MaSoundConfigPtr(Inno.Native.MiniAudio.MaSoundConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18154) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a2557c2f764b1fa4"></a>[`public bool Inno.Native.MiniAudio.MaSoundConfigPtr.Equals(Inno.Native.MiniAudio.MaSoundConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18169) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-02200c84a0c97738"></a>[`public override bool Inno.Native.MiniAudio.MaSoundConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18170) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-c40af58cb7b5afd2"></a>[`public override int Inno.Native.MiniAudio.MaSoundConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18171) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-f8f30d4951b19f19"></a>[`public static bool Inno.Native.MiniAudio.MaSoundConfigPtr.operator ==(Inno.Native.MiniAudio.MaSoundConfigPtr left, Inno.Native.MiniAudio.MaSoundConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18167) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-83728ddec41dde4d"></a>[`public static Inno.Native.MiniAudio.MaSoundConfigPtr.implicit operator Inno.Native.MiniAudio.MaSoundConfigPtr(Inno.Native.MiniAudio.MaSoundConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18165) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9eb26c2ba69386e5"></a>[`public static Inno.Native.MiniAudio.MaSoundConfigPtr.implicit operator Inno.Native.MiniAudio.MaSoundConfig*(Inno.Native.MiniAudio.MaSoundConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18166) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-423fb9590a3bbc67"></a>[`public static bool Inno.Native.MiniAudio.MaSoundConfigPtr.operator !=(Inno.Native.MiniAudio.MaSoundConfigPtr left, Inno.Native.MiniAudio.MaSoundConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18168) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1b4eb0d0713190dd"></a>[`public ref uint Inno.Native.MiniAudio.MaSoundConfigPtr.ChannelsIn { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18178) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7e119ea0284c3ed1"></a>[`public ref uint Inno.Native.MiniAudio.MaSoundConfigPtr.ChannelsOut { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18179) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2482962d9f80e891"></a>[`public void* Inno.Native.MiniAudio.MaSoundConfigPtr.EndCallback { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18188) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0e85d39f7c1ee6b7"></a>[`public ref uint Inno.Native.MiniAudio.MaSoundConfigPtr.Flags { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18181) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a2389ede14a389fc"></a>[`public ref uint Inno.Native.MiniAudio.MaSoundConfigPtr.InitialAttachmentInputBusIndex { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18177) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-850b09ee7a69a5d3"></a>[`public ref ulong Inno.Native.MiniAudio.MaSoundConfigPtr.InitialSeekPointInPcmFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18183) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bffa192442f16852"></a>[`public ref Inno.Native.MiniAudio.MaResourceManagerPipelineNotifications Inno.Native.MiniAudio.MaSoundConfigPtr.InitNotifications { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18191) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-94fa278374139279"></a>[`public ref uint Inno.Native.MiniAudio.MaSoundConfigPtr.IsLooping { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18193) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ed420f1368e2364a"></a>[`public bool Inno.Native.MiniAudio.MaSoundConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18160) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-22740cee50c31cd7"></a>[`public Inno.Native.MiniAudio.MaSoundConfig Inno.Native.MiniAudio.MaSoundConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18163) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8d3f4f6406773af1"></a>[`public ref ulong Inno.Native.MiniAudio.MaSoundConfigPtr.LoopPointBegInPcmFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18186) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-80725f85d7af8b87"></a>[`public ref ulong Inno.Native.MiniAudio.MaSoundConfigPtr.LoopPointEndInPcmFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18187) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a716a6152fbf88ee"></a>[`public ref Inno.Native.MiniAudio.MaMonoExpansionMode Inno.Native.MiniAudio.MaSoundConfigPtr.MonoExpansionMode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18180) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9fb4205cf3c63de3"></a>[`public static Inno.Native.MiniAudio.MaSoundConfigPtr Inno.Native.MiniAudio.MaSoundConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18161) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-03d20b7eb84e93c0"></a>[`public Inno.Native.MiniAudio.MaDataSource* Inno.Native.MiniAudio.MaSoundConfigPtr.PDataSource { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18175) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f427a025a3848c95"></a>[`public ref Inno.Native.MiniAudio.MaFencePtr Inno.Native.MiniAudio.MaSoundConfigPtr.PDoneFence { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18192) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-241ca56147711b0c"></a>[`public void* Inno.Native.MiniAudio.MaSoundConfigPtr.PEndCallbackUserData { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18189) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-be712dc09ce8c96f"></a>[`public byte* Inno.Native.MiniAudio.MaSoundConfigPtr.PFilePath { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18173) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c941d5405d9ba780"></a>[`public ushort* Inno.Native.MiniAudio.MaSoundConfigPtr.PFilePathW { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18174) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ad5d11579f2c52f0"></a>[`public void* Inno.Native.MiniAudio.MaSoundConfigPtr.PInitialAttachment { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18176) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b125ed8f3a4b4aa3"></a>[`public ref Inno.Native.MiniAudio.MaResamplerConfig Inno.Native.MiniAudio.MaSoundConfigPtr.PitchResampling { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18190) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c0563f16795a9558"></a>[`public ref ulong Inno.Native.MiniAudio.MaSoundConfigPtr.RangeBegInPcmFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18184) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-487a32ec62e732c1"></a>[`public ref ulong Inno.Native.MiniAudio.MaSoundConfigPtr.RangeEndInPcmFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18185) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-569ac174574b0b0e"></a>[`public ref uint Inno.Native.MiniAudio.MaSoundConfigPtr.VolumeSmoothTimeInPcmFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18182) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b787b0707c811cfb"></a>[`public struct Inno.Native.MiniAudio.MaSoundConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaSoundConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18152) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSoundEndProc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSoundEndProc 9220b3e481e2f351a73a5fff779f55195777e9d59ce867c1b183142139216c01 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundEndProc.#ctor(System.Object,System.IntPtr) 3d24272f2a538bb0022bfe7d2fcfd337fe2e9b1738153753b3dd4d9f60cf9632 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundEndProc.Invoke(System.Void*,Inno.Native.MiniAudio.MaSound*) c2a91cf02cce9029084c39fbbb0677cbf6d0576ae61dce7309e356dddce3de6a -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundEndProc.BeginInvoke(System.Void*,Inno.Native.MiniAudio.MaSound*,System.AsyncCallback,System.Object) ba421f8bbcc2bfc14a868b8c5c777e0ddd3428fe54b6ba6e0bfc3c42b0b0b662 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundEndProc.EndInvoke(System.IAsyncResult) 3c734877fc1f742c5142db2b454a371198b3da1d432beab224e6f61bf0283a8f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dede0911a7c4c571"></a>[`public Inno.Native.MiniAudio.MaSoundEndProc.MaSoundEndProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14286) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-92946da04d221570"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaSoundEndProc.BeginInvoke(void* pUserData, Inno.Native.MiniAudio.MaSound* pSound, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14286) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-03af4a3dbdd9dce7"></a>[`public virtual void Inno.Native.MiniAudio.MaSoundEndProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14286) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c11d049057bd9401"></a>[`public virtual void Inno.Native.MiniAudio.MaSoundEndProc.Invoke(void* pUserData, Inno.Native.MiniAudio.MaSound* pSound)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14286) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3fee673fd3a1769a"></a>[`public delegate void Inno.Native.MiniAudio.MaSoundEndProc(void* pUserData, Inno.Native.MiniAudio.MaSound* pSound)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14286) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSoundFlags</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSoundFlags 6f1377640a9dc74afeef258e7d76cba337cc72886e6cadcc73c87666bbcfc305 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.FlagStream a92b69db9c480935b4d8e5805f42a623d9bce2aae63079d2a450ee156aaf0a61 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.FlagDecode 2ca7e2a731a031e308f4dbb385e89a47d50d5e681b1f92a3c107c767f7268931 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.FlagAsync b4fcf95a92e1d39017cf65546e0c58cae51ef54014876e3ebd88a44290e53dc8 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.FlagWaitInit 16ce6ad25b751c7c15808a5ab546a78d613362e4999d597421e9cc3f5b86243b -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.FlagUnknownLength d4166cea6ddf20dc276cc20fae20936b8ec59323ba56471ab55b8a64522dbaa7 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.FlagLooping 5ac46cc39517f3bd4bde3d79a456306db616c95176b8b1e1e8a45e0c7eb60905 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.FlagNoDefaultAttachment 0f0fe0ccf0ecae43e7481a786e99f1ce0e5aed47cbbe8da71ccc3ee14114f277 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.FlagNoPitch 755389dd143975c9dc82c54c9ef978dd9027f2abd9225859d3562ba6a126d100 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundFlags.FlagNoSpatialization 75e54034f98f410c3791300b63e66580d820d3ce1fa67b7681c963b6b5af0c04 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ab516d86a7138a16"></a>[`Inno.Native.MiniAudio.MaSoundFlags.FlagAsync = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L736) | 当前常量或枚举值：`4`。 | — |
| <a id="api-ad9026802fd17cc3"></a>[`Inno.Native.MiniAudio.MaSoundFlags.FlagDecode = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L735) | 当前常量或枚举值：`2`。 | — |
| <a id="api-2d21bdf6b2dbe322"></a>[`Inno.Native.MiniAudio.MaSoundFlags.FlagLooping = 32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L739) | 当前常量或枚举值：`32`。 | — |
| <a id="api-d2797e397f5ef85e"></a>[`Inno.Native.MiniAudio.MaSoundFlags.FlagNoDefaultAttachment = 4096`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L740) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-7c5d7a7d6dd7e27f"></a>[`Inno.Native.MiniAudio.MaSoundFlags.FlagNoPitch = 8192`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L741) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-0170782491a21459"></a>[`Inno.Native.MiniAudio.MaSoundFlags.FlagNoSpatialization = 16384`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L742) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-ba3244f7b17da1bd"></a>[`Inno.Native.MiniAudio.MaSoundFlags.FlagStream = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L734) | 当前常量或枚举值：`1`。 | — |
| <a id="api-b03ea5c52df940a9"></a>[`Inno.Native.MiniAudio.MaSoundFlags.FlagUnknownLength = 16`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L738) | 当前常量或枚举值：`16`。 | — |
| <a id="api-b1463dc9fe212c53"></a>[`Inno.Native.MiniAudio.MaSoundFlags.FlagWaitInit = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L737) | 当前常量或枚举值：`8`。 | — |
| <a id="api-91c7940a5da3831c"></a>[`public enum Inno.Native.MiniAudio.MaSoundFlags : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L732) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSoundInlined</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSoundInlined a59cc404cbea88211afb4845b438abc3f511e3e6b2320536cbdea1cba70f5b8c -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundInlined.Sound 7b0c3df2aaaa6880aa040d258cd58195bf06d08ad5e6d503e598fc59f63731a3 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundInlined.PNext 21b924a96c28fff11c5704e9a87b50f5051c01ced040e0cad15f0c9a38855263 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundInlined.PPrev d0d4828b3c4c58e4b628ac45b311a7a85616dea7adfac735b3fdcb45cedcb1c2 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundInlined.#ctor(Inno.Native.MiniAudio.MaSound,Inno.Native.MiniAudio.MaSoundInlined*,Inno.Native.MiniAudio.MaSoundInlined*) ba4bf0354fcb78218e3b76d7483bdc6dcd5cd64a673b412d6d2881c82a95f982 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5dc8cfb5854878db"></a>[`public Inno.Native.MiniAudio.MaSoundInlined* Inno.Native.MiniAudio.MaSoundInlined.PNext`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13643) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df5b480286c98190"></a>[`public Inno.Native.MiniAudio.MaSoundInlined* Inno.Native.MiniAudio.MaSoundInlined.PPrev`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13644) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c21a86d03c3073d5"></a>[`public Inno.Native.MiniAudio.MaSound Inno.Native.MiniAudio.MaSoundInlined.Sound`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13642) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f4a713dc225b3919"></a>[`public Inno.Native.MiniAudio.MaSoundInlined.MaSoundInlined(Inno.Native.MiniAudio.MaSound sound = default(Inno.Native.MiniAudio.MaSound), Inno.Native.MiniAudio.MaSoundInlined* pNext = null, Inno.Native.MiniAudio.MaSoundInlined* pPrev = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13645) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b78f298f9787b5bc"></a>[`public struct Inno.Native.MiniAudio.MaSoundInlined`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13640) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSoundInlinedPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSoundInlinedPtr d90f80197deb8d17f39c7977b30120a3868b03128925db2d7477b9e0bbce08f9 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundInlinedPtr.#ctor(Inno.Native.MiniAudio.MaSoundInlined*) 78678305aea00365f671cf7ac7922fb55d5175f77b6d6f9acae7acb05ff8a562 -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundInlinedPtr.Handle e3bb1be9b1b450ba769793a819e606f3b90a0a76cd99f0d74723c08873505c8c -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundInlinedPtr.IsNull 7cff11772e7f305e7eb2ac5b694496bf49eeabac8a30c1c312b77370559925c1 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundInlinedPtr.Null bccc9df9fb0731a487163338903cae01c5a528c83f49a88b28d8d6eb50ec0606 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundInlinedPtr.Item(System.Int32) 7bcca2371d088ca48160fc9ad134a02b30f64a7014e2a9817311c93a9c9f0b8e -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundInlinedPtr.op_Implicit(Inno.Native.MiniAudio.MaSoundInlined*)~Inno.Native.MiniAudio.MaSoundInlinedPtr 0e8830fd291a0798603670cc899bd6db98c533dae613a1be994acb8288d453eb -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundInlinedPtr.op_Implicit(Inno.Native.MiniAudio.MaSoundInlinedPtr)~Inno.Native.MiniAudio.MaSoundInlined* ae2e4d6bf930323cf81dcc33a329416d625da788ee111ae09391d524341f5d41 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundInlinedPtr.op_Equality(Inno.Native.MiniAudio.MaSoundInlinedPtr,Inno.Native.MiniAudio.MaSoundInlinedPtr) 5137e22a76ebc02a540477cf18fb40de9d081a348cf8604f3ba1317cded2e6f8 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundInlinedPtr.op_Inequality(Inno.Native.MiniAudio.MaSoundInlinedPtr,Inno.Native.MiniAudio.MaSoundInlinedPtr) 5d9036acc6f01bd9519345b0e9b158d567f5ad5c87e7743f388b093662f40eac -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundInlinedPtr.Equals(Inno.Native.MiniAudio.MaSoundInlinedPtr) 15db1fd5a91aa56e021bbd3eeed2f49d111989d327f3d60bf7ad322dd93fe2dd -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundInlinedPtr.Equals(System.Object) ea21aee9053d69e8c24c6c84877df3ad83a152e5a3c177c678650b9d77251e40 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundInlinedPtr.GetHashCode c8f35c2bd6b8c04ec50d6d7ab3f9af9410bdd957f5801af3b2fd9adf09fe3129 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundInlinedPtr.Sound 7df177bc326fbac854704d03b7bb3bdf00656209449ce630c25e3fffa4b1f64a -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundInlinedPtr.PNext 537d7446171564bce24abfa20db6400953119d781c76214d94191d1f6267d1da -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundInlinedPtr.PPrev 37846267f1ce644193c3703248fba9770c817c5c625f1176860efdb57d197e34 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-31fe93c231b27244"></a>[`public Inno.Native.MiniAudio.MaSoundInlined* Inno.Native.MiniAudio.MaSoundInlinedPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18204) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6b227edc3c621f14"></a>[`public Inno.Native.MiniAudio.MaSoundInlinedPtr.MaSoundInlinedPtr(Inno.Native.MiniAudio.MaSoundInlined* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18199) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1d6c28f0d7943a8f"></a>[`public bool Inno.Native.MiniAudio.MaSoundInlinedPtr.Equals(Inno.Native.MiniAudio.MaSoundInlinedPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18214) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-058846951642dde3"></a>[`public override bool Inno.Native.MiniAudio.MaSoundInlinedPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18215) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-c7343e2e8f69fea7"></a>[`public override int Inno.Native.MiniAudio.MaSoundInlinedPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18216) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-207f51397041eb0d"></a>[`public static bool Inno.Native.MiniAudio.MaSoundInlinedPtr.operator ==(Inno.Native.MiniAudio.MaSoundInlinedPtr left, Inno.Native.MiniAudio.MaSoundInlinedPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18212) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e8f29da9f3984abe"></a>[`public static Inno.Native.MiniAudio.MaSoundInlinedPtr.implicit operator Inno.Native.MiniAudio.MaSoundInlinedPtr(Inno.Native.MiniAudio.MaSoundInlined* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18210) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-09f92b2940f8e566"></a>[`public static Inno.Native.MiniAudio.MaSoundInlinedPtr.implicit operator Inno.Native.MiniAudio.MaSoundInlined*(Inno.Native.MiniAudio.MaSoundInlinedPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18211) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-59f02865a4820ae7"></a>[`public static bool Inno.Native.MiniAudio.MaSoundInlinedPtr.operator !=(Inno.Native.MiniAudio.MaSoundInlinedPtr left, Inno.Native.MiniAudio.MaSoundInlinedPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18213) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-365175ad9bf4c8ad"></a>[`public bool Inno.Native.MiniAudio.MaSoundInlinedPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18205) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0e4a3ff81e0790de"></a>[`public Inno.Native.MiniAudio.MaSoundInlined Inno.Native.MiniAudio.MaSoundInlinedPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18208) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-488197e8842cae4c"></a>[`public static Inno.Native.MiniAudio.MaSoundInlinedPtr Inno.Native.MiniAudio.MaSoundInlinedPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18206) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4feda575e96ca9fd"></a>[`public ref Inno.Native.MiniAudio.MaSoundInlinedPtr Inno.Native.MiniAudio.MaSoundInlinedPtr.PNext { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18219) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8b800c2c1e5eb3ae"></a>[`public ref Inno.Native.MiniAudio.MaSoundInlinedPtr Inno.Native.MiniAudio.MaSoundInlinedPtr.PPrev { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18220) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-150ae90e254a3fe9"></a>[`public ref Inno.Native.MiniAudio.MaSound Inno.Native.MiniAudio.MaSoundInlinedPtr.Sound { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18218) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aacd3f0b68bc624d"></a>[`public struct Inno.Native.MiniAudio.MaSoundInlinedPtr : System.IEquatable<Inno.Native.MiniAudio.MaSoundInlinedPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18197) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSoundPtr</code> · 24 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSoundPtr b3b520c4c7c4a3c1b54147f371f1045a55c764aa1707df1e43b90a527aaa2a50 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundPtr.#ctor(Inno.Native.MiniAudio.MaSound*) 14899f1e7797788903f528124e49811700a433277f743a06f6ca3c38bc477c8e -->
<!-- api: F:Inno.Native.MiniAudio.MaSoundPtr.Handle 47fd3ed75a5748409b9a5642ab94920a73f20c6666d703993b477bdee0cb45a8 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.IsNull b4df75ec61fee06a5ed0caab0d6266baadf60b6a0e8953a39c1651487ab4cad7 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.Null e8a926cb386b96ea6b7a2a080705cf484d342055a980937061a62c10653105a7 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.Item(System.Int32) 4cdc332155b8a92473f637a24b150c783c63d4f8f30ee93c97a27da18de9a4a6 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundPtr.op_Implicit(Inno.Native.MiniAudio.MaSound*)~Inno.Native.MiniAudio.MaSoundPtr 4bb3f493c0b9fbf434c851c8ff1b0efd967eef7ec7e6e0eb6c414dcb9fff20ed -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundPtr.op_Implicit(Inno.Native.MiniAudio.MaSoundPtr)~Inno.Native.MiniAudio.MaSound* f40db964865cccaf78d201d975505a7792a4bcae75e3a84703f7504025c78264 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundPtr.op_Equality(Inno.Native.MiniAudio.MaSoundPtr,Inno.Native.MiniAudio.MaSoundPtr) 52d251dfea786c4cd56041768738858babdd041775b2315b25edf175b68e4881 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundPtr.op_Inequality(Inno.Native.MiniAudio.MaSoundPtr,Inno.Native.MiniAudio.MaSoundPtr) 4e1636ec1edaa7ef4df1d1d1ee600b48cdb777f019a6ef766bd198b5d4983737 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundPtr.Equals(Inno.Native.MiniAudio.MaSoundPtr) 23fdf88ee8c05ea09d069d44f7d8ee7270b188151b472415701f39c0d9e6b5d0 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundPtr.Equals(System.Object) 7c960a25ea6b75b6cb5ccce2117ee73df07ae496f300f7bfae55aea84f6937c0 -->
<!-- api: M:Inno.Native.MiniAudio.MaSoundPtr.GetHashCode 1f8a413ccf8855dcda3cf5494d81e8d469b6a0fa4c88adfe4d91267e3dce6dd5 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.EngineNode ee47fc2ee60cb022eeac8ac0f1a3d21bd20ab82a975889b21178f45ebaeccf14 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.PDataSource d87c8f3657c32d3fd4d6b902a717a9821c782ca9645a086ba7a73b7ba7a43422 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.SeekTarget 4cfdf06a9ac2adb885290ee7163ed7c9df72c3ff9293ed23ee1b3e1cde197c51 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.AtEnd aa2bd46e354cc3ff6a882f450a05b87a12f2bff99fc74521f81a47574f078ee6 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.EndCallback df7064ba4810cfcc2c55c78dec8443de741615b457c67e53d76484e300f3b30a -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.PEndCallbackUserData 453fcb5b78e45d6f7157adf3ef1b8c224148fc7c6c1a7250e9709eeaefb3359c -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.PProcessingCache 451ff9521450159a80a7d5c4a27d82f00b56be2ee9e6c14ebd043f1e7c67e305 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.ProcessingCacheFramesRemaining 7857eec19e761b3c6fc18f36a7fc6a49b2005d005b10648ecf616b087d80286a -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.ProcessingCacheCap 7dc95a65b172855698be4baeb029dcb0889308e0afc09efe51e1bbe565d122e1 -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.OwnsDataSource e8b09557051733f8e75288a121ab5bf4cc178d72ee3551fb4a31f76ca9a6791c -->
<!-- api: P:Inno.Native.MiniAudio.MaSoundPtr.PResourceManagerDataSource efbcbebe697961cea4a1aa32c27028d3662248f2698c6d55c6b4a7025c41a2e3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e32b5ae120ed742d"></a>[`public Inno.Native.MiniAudio.MaSound* Inno.Native.MiniAudio.MaSoundPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18231) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2b305954cf6045cd"></a>[`public Inno.Native.MiniAudio.MaSoundPtr.MaSoundPtr(Inno.Native.MiniAudio.MaSound* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18226) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6fed72dba12d9e9e"></a>[`public bool Inno.Native.MiniAudio.MaSoundPtr.Equals(Inno.Native.MiniAudio.MaSoundPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18241) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-9d4c45a6ebf6a6cc"></a>[`public override bool Inno.Native.MiniAudio.MaSoundPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18242) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-66e43261d9652de4"></a>[`public override int Inno.Native.MiniAudio.MaSoundPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18243) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-263f51fe95f22aba"></a>[`public static bool Inno.Native.MiniAudio.MaSoundPtr.operator ==(Inno.Native.MiniAudio.MaSoundPtr left, Inno.Native.MiniAudio.MaSoundPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18239) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e5eea4efc806c91"></a>[`public static Inno.Native.MiniAudio.MaSoundPtr.implicit operator Inno.Native.MiniAudio.MaSoundPtr(Inno.Native.MiniAudio.MaSound* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18237) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b4851c86e4d4d929"></a>[`public static Inno.Native.MiniAudio.MaSoundPtr.implicit operator Inno.Native.MiniAudio.MaSound*(Inno.Native.MiniAudio.MaSoundPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18238) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-05c2072520ae5fc2"></a>[`public static bool Inno.Native.MiniAudio.MaSoundPtr.operator !=(Inno.Native.MiniAudio.MaSoundPtr left, Inno.Native.MiniAudio.MaSoundPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18240) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b8f60a6c36a9227c"></a>[`public ref uint Inno.Native.MiniAudio.MaSoundPtr.AtEnd { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18248) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f03b66ee48fb29c4"></a>[`public void* Inno.Native.MiniAudio.MaSoundPtr.EndCallback { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18249) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6d9be2f57f76ae57"></a>[`public ref Inno.Native.MiniAudio.MaEngineNode Inno.Native.MiniAudio.MaSoundPtr.EngineNode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18245) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0eeaa9cf3f9863da"></a>[`public bool Inno.Native.MiniAudio.MaSoundPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18232) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f6ccbbb38d6b5b07"></a>[`public Inno.Native.MiniAudio.MaSound Inno.Native.MiniAudio.MaSoundPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18235) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4aa318a1225578f5"></a>[`public static Inno.Native.MiniAudio.MaSoundPtr Inno.Native.MiniAudio.MaSoundPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18233) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2af2d3735aed11e8"></a>[`public ref byte Inno.Native.MiniAudio.MaSoundPtr.OwnsDataSource { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18254) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5fdbb84fa71fc3c5"></a>[`public Inno.Native.MiniAudio.MaDataSource* Inno.Native.MiniAudio.MaSoundPtr.PDataSource { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18246) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6c8eb211bafba71c"></a>[`public void* Inno.Native.MiniAudio.MaSoundPtr.PEndCallbackUserData { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18250) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4232220faa4719af"></a>[`public float* Inno.Native.MiniAudio.MaSoundPtr.PProcessingCache { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18251) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b10f0ebdbcfcbedc"></a>[`public ref Inno.Native.MiniAudio.MaResourceManagerDataSourcePtr Inno.Native.MiniAudio.MaSoundPtr.PResourceManagerDataSource { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18255) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-377d9e2486298f9f"></a>[`public ref uint Inno.Native.MiniAudio.MaSoundPtr.ProcessingCacheCap { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18253) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-917e0ad5821aa90c"></a>[`public ref uint Inno.Native.MiniAudio.MaSoundPtr.ProcessingCacheFramesRemaining { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18252) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fead19766dff30f8"></a>[`public ref ulong Inno.Native.MiniAudio.MaSoundPtr.SeekTarget { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18247) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4d6af383a22a6773"></a>[`public struct Inno.Native.MiniAudio.MaSoundPtr : System.IEquatable<Inno.Native.MiniAudio.MaSoundPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18224) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSpatializer</code> · 28 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSpatializer 30ea53be26f0a8b269853e28d3810647c32d9c4c636aa6d5e38674d4814d3caa -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.ChannelsIn bb4fdb2f8c102706cfbbe36eb8b83c999b8085b392fd57168c422ec3558f12ae -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.ChannelsOut 6e7badde1bec7e72134036ab523e7b479d1dd56e5174fd3582b2591ef5db520f -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.PChannelMapIn 82e76768d32246b5914041277c5dd205761be687cec8fcb5f42b61e6311c2c04 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.AttenuationModel 41bfa9fbdbc76aa7565b7931d7bb9a6258c169593170e5435bba297245ce40f5 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Positioning 313c42a04956e9880e895ced3ab7e733d8bfea280fec4fe542fcee5368759457 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Handedness 19ff37e80a4e3ec0304a75c92475e21e0f75649d732cd7c6d93e2f1780e7a6be -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.MinGain 39591723530d1245da6fd58906c30c92ebf89707cfe097479275d61a245d97da -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.MaxGain ffed079054d7ad00943c0bd3fa6dffa9d89ff6546e39f02f38d3a6e72b68dccc -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.MinDistance 41c5a2c4b3234461f4a833047b74fd7470f0a30ed6cb0a8c5141634bb2c9d698 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.MaxDistance 994865c6cb47057a3f212b7e297e74f38fbb62c6ea2714af15b5cf59e418391a -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Rolloff b96765360ace5403d617c76ad8f26069011b1769a59c5304608ff612c8fa8ec4 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.ConeInnerAngleInRadians 7f8ec8b83288924d2a8e87705d7168a6f5e9c7ab80684b8694fbcaffce95d476 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.ConeOuterAngleInRadians ee9c26fbaa5e40024b6367fc45015142d24897901e7deaed67967dfa97a41295 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.ConeOuterGain f6ca22a6cc192fa1df9dfb20afdbb7f037c5880dd1504bf0a64b229fa59bdb90 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.DopplerFactor 523283393d8e040837e35de4ff7206652a146c2a947d29b832482282a965b395 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.DirectionalAttenuationFactor c59a674c085770e57811e24cdeaeb702681920692b6c58e0c810e09c4777d81c -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.GainSmoothTimeInFrames 940b3759e22060a843a12b7dbb8e1dee8aaef088f77d3ac753d5a1bf49ee26ea -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Position e8a0bf5c6794046420b0f80615f01c5653fa62542ff5f4ddb64ec19d1156a8ff -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Direction 9bf90f6f83fde2019b22013128a8f6f605d435ce078bd64f859ce027f49c7120 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Velocity 19dee0fa0774d59dbb1c8545bc2a335db0bb56c0908d24fac8539d5bce2d5969 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.DopplerPitch 260246735f5f4240b8f2175c92978a53779356c57ef24932309f07214584af5a -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.MinSpatializationChannelGain 0cef4bf613b55ce73d877d170f313895630073b63ffc742fc32187dbd5286c13 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Gainer e15d509af13b1f02a94eb6208153dd44d848cf7621b7cee95726eb7a25514e24 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.PNewChannelGainsOut b7f2fad3a36b073f6bd1eb4054463965ec0139d8728a9e1c74013fdf1880459f -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Pheap 0dec9290d5fc28e5681154639e65c1db9dce5925be358a097c68f0950d846b70 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializer.Ownsheap 01ae82203a0eda4c31e3f2b9839f0c6bd9c8e465dfb0c1858c1482be699c3618 -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializer.#ctor(System.UInt32,System.UInt32,System.Byte*,Inno.Native.MiniAudio.MaAttenuationModel,Inno.Native.MiniAudio.MaPositioning,Inno.Native.MiniAudio.MaHandedness,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.UInt32,Inno.Native.MiniAudio.MaAtomicVec3F,Inno.Native.MiniAudio.MaAtomicVec3F,Inno.Native.MiniAudio.MaAtomicVec3F,System.Single,System.Single,Inno.Native.MiniAudio.MaGainer,System.Single*,System.Void*,System.UInt32) 6113f032b9dbe739c7d97003cae1bda538467e71d1ea22a9bf2796c9babc258a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0d74718f7b8d4a19"></a>[`public Inno.Native.MiniAudio.MaAttenuationModel Inno.Native.MiniAudio.MaSpatializer.AttenuationModel`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11170) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c9e0b4d8e036b70d"></a>[`public uint Inno.Native.MiniAudio.MaSpatializer.ChannelsIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11167) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eb46a4dbf3689d4b"></a>[`public uint Inno.Native.MiniAudio.MaSpatializer.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11168) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c618d42947a4077"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.ConeInnerAngleInRadians`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11178) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-02b02a210b4321f6"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.ConeOuterAngleInRadians`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11179) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-08a34eaaba8ead5b"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.ConeOuterGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11180) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-76a21b6b3ac22ed1"></a>[`public Inno.Native.MiniAudio.MaAtomicVec3F Inno.Native.MiniAudio.MaSpatializer.Direction`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11185) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-38ff734e67c5dd13"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.DirectionalAttenuationFactor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11182) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-653fb6193cabf131"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.DopplerFactor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11181) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc6d280aaee13191"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.DopplerPitch`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11187) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53d82e6452e16183"></a>[`public Inno.Native.MiniAudio.MaGainer Inno.Native.MiniAudio.MaSpatializer.Gainer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11189) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d4e52cb91c692cb"></a>[`public uint Inno.Native.MiniAudio.MaSpatializer.GainSmoothTimeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11183) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e524ccceb71ad05e"></a>[`public Inno.Native.MiniAudio.MaHandedness Inno.Native.MiniAudio.MaSpatializer.Handedness`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11172) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ad4f34909216ebb"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.MaxDistance`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11176) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5fc86c720ffc25b7"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.MaxGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11174) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0b2586e12e812e07"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.MinDistance`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11175) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d053ac2a8106f3c"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.MinGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11173) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f995192538e13e6"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.MinSpatializationChannelGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11188) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e01b55a4f541977"></a>[`public uint Inno.Native.MiniAudio.MaSpatializer.Ownsheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11192) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9c3df018ed68785"></a>[`public byte* Inno.Native.MiniAudio.MaSpatializer.PChannelMapIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11169) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3f3464981aebeada"></a>[`public void* Inno.Native.MiniAudio.MaSpatializer.Pheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11191) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da47297377f2924b"></a>[`public float* Inno.Native.MiniAudio.MaSpatializer.PNewChannelGainsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11190) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bec7fb9db3dfeedd"></a>[`public Inno.Native.MiniAudio.MaAtomicVec3F Inno.Native.MiniAudio.MaSpatializer.Position`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11184) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba95edeeea8de436"></a>[`public Inno.Native.MiniAudio.MaPositioning Inno.Native.MiniAudio.MaSpatializer.Positioning`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11171) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a9e07e253a615479"></a>[`public float Inno.Native.MiniAudio.MaSpatializer.Rolloff`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11177) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13555afe3d758ea0"></a>[`public Inno.Native.MiniAudio.MaAtomicVec3F Inno.Native.MiniAudio.MaSpatializer.Velocity`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11186) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-95cdcb6c02b0c929"></a>[`public Inno.Native.MiniAudio.MaSpatializer.MaSpatializer(uint channelsIn = 0, uint channelsOut = 0, byte* pChannelMapIn = null, Inno.Native.MiniAudio.MaAttenuationModel attenuationModel = Inno.Native.MiniAudio.MaAttenuationModel.None, Inno.Native.MiniAudio.MaPositioning positioning = Inno.Native.MiniAudio.MaPositioning.Absolute, Inno.Native.MiniAudio.MaHandedness handedness = Inno.Native.MiniAudio.MaHandedness.Right, float minGain = 0, float maxGain = 0, float minDistance = 0, float maxDistance = 0, float rolloff = 0, float coneInnerAngleInRadians = 0, float coneOuterAngleInRadians = 0, float coneOuterGain = 0, float dopplerFactor = 0, float directionalAttenuationFactor = 0, uint gainSmoothTimeInFrames = 0, Inno.Native.MiniAudio.MaAtomicVec3F position = default(Inno.Native.MiniAudio.MaAtomicVec3F), Inno.Native.MiniAudio.MaAtomicVec3F direction = default(Inno.Native.MiniAudio.MaAtomicVec3F), Inno.Native.MiniAudio.MaAtomicVec3F velocity = default(Inno.Native.MiniAudio.MaAtomicVec3F), float dopplerPitch = 0, float minSpatializationChannelGain = 0, Inno.Native.MiniAudio.MaGainer gainer = default(Inno.Native.MiniAudio.MaGainer), float* pNewChannelGainsOut = null, void* pHeap = null, uint ownsHeap = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11193) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2dcc070e580b2b00"></a>[`public struct Inno.Native.MiniAudio.MaSpatializer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11165) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSpatializerConfig</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSpatializerConfig 223af95a01db556658034b1ce5bd785640eb944339700f5a348ce6bc5fe4acdb -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.ChannelsIn 3d368a2b89a8a6cdf5d2f72a1a4c6f5d6f9d47ecf044ea0ccccbd00076f359f5 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.ChannelsOut 4db469abeb909ea5733dc2781e25c44bb2c58328bc7df9b25843c406e107b1c4 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.PChannelMapIn 540d5d4d3a8b34c133eac1355e2cf27b414e0b2e9aabcd6a470bba9c3caacb9f -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.AttenuationModel 7b8dae6595b407c2807a6142989f4478bf0b7e07a5e35142ca5f5be52650820a -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.Positioning 4e5a366e9da547f801c02337d04467ed9260941533b372319285c173b124e9a4 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.Handedness 7672a49555482acd4a1d5acce14bfe4bbbe97d400443b8652f11ab95d3c96487 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.MinGain 1dc6c773ab07bb2b4038557d4398e5c9d2c0637cfa5b74261b815fe7918a5835 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.MaxGain 69ff069ba819501d7d5cb174ad0a68c393d61ddd5cdd8f68998aa61f2c3d9e93 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.MinDistance ac99ed1814c09004b4fe20a3839a5e133229bcf76142641fb18e6eba9cf673fa -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.MaxDistance 8561ee6127c832d65e6a3c428bfc8ab8fd0dc0b7ce4a8ab04f09686ee69ee68b -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.Rolloff f91759d6c59aa8202d80f3a883e16d8a70c8031f660a925a8b6f9baa1dc0e3ba -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.ConeInnerAngleInRadians e125b999f348a029d14b1005ab2de879c2699bdb89e76e804cd8891cddc48bc8 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.ConeOuterAngleInRadians 8fa46327101240b0be8c5ef38c91190e3989f57cf11afe319612a442bec2bc08 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.ConeOuterGain c4eab956933dcaf8338daf32153a93037ca9f49f149330f1f7a5ede2bbc6bb8e -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.DopplerFactor d768a5201656492fa2680a8afec61b34b3ea0bdead876b2b32fa9f89df5df99e -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.DirectionalAttenuationFactor 6736770a2ad698799677feaccba912d1af3b0fb76eceba823374887ca90b2b14 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.MinSpatializationChannelGain d37ae1231a04131e48553f3b8f1c7b196c972025183ef7f7de2bbc5a2a260d0e -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfig.GainSmoothTimeInFrames ee102b9bfa4ce607bc76c788fe1de86a6ac3d521f0d6472a4d6ebf333dd49c83 -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerConfig.#ctor(System.UInt32,System.UInt32,System.Byte*,Inno.Native.MiniAudio.MaAttenuationModel,Inno.Native.MiniAudio.MaPositioning,Inno.Native.MiniAudio.MaHandedness,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.UInt32) dcbc0d4e2ba25c378872341e1a1f18b520725f5ac761557b14b9364c5385e777 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c737a162ca4119d6"></a>[`public Inno.Native.MiniAudio.MaAttenuationModel Inno.Native.MiniAudio.MaSpatializerConfig.AttenuationModel`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11123) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae05b1b2ad9f026a"></a>[`public uint Inno.Native.MiniAudio.MaSpatializerConfig.ChannelsIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11120) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-559ac3ed16fb4494"></a>[`public uint Inno.Native.MiniAudio.MaSpatializerConfig.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11121) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-082a8b9e2ecbc70b"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.ConeInnerAngleInRadians`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11131) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07fd88c28431d01c"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.ConeOuterAngleInRadians`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11132) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bed9d2c02da7426b"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.ConeOuterGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11133) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-82fa6d998250622f"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.DirectionalAttenuationFactor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11135) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6bb27c99e1679bae"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.DopplerFactor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11134) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ecc626f535ccbf58"></a>[`public uint Inno.Native.MiniAudio.MaSpatializerConfig.GainSmoothTimeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11137) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1df46fe2a4c2abf9"></a>[`public Inno.Native.MiniAudio.MaHandedness Inno.Native.MiniAudio.MaSpatializerConfig.Handedness`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11125) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b83625040edd3efd"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.MaxDistance`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11129) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-850f3d0b587a2e55"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.MaxGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11127) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bea6f2884b1a20ac"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.MinDistance`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11128) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-402713492925a7d1"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.MinGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11126) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-232b31974267e29a"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.MinSpatializationChannelGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11136) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c243919e0f71dd7f"></a>[`public byte* Inno.Native.MiniAudio.MaSpatializerConfig.PChannelMapIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11122) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-78ae6f10da6f2bac"></a>[`public Inno.Native.MiniAudio.MaPositioning Inno.Native.MiniAudio.MaSpatializerConfig.Positioning`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11124) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa929108b2a53dc4"></a>[`public float Inno.Native.MiniAudio.MaSpatializerConfig.Rolloff`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11130) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ee36f70ba859f043"></a>[`public Inno.Native.MiniAudio.MaSpatializerConfig.MaSpatializerConfig(uint channelsIn = 0, uint channelsOut = 0, byte* pChannelMapIn = null, Inno.Native.MiniAudio.MaAttenuationModel attenuationModel = Inno.Native.MiniAudio.MaAttenuationModel.None, Inno.Native.MiniAudio.MaPositioning positioning = Inno.Native.MiniAudio.MaPositioning.Absolute, Inno.Native.MiniAudio.MaHandedness handedness = Inno.Native.MiniAudio.MaHandedness.Right, float minGain = 0, float maxGain = 0, float minDistance = 0, float maxDistance = 0, float rolloff = 0, float coneInnerAngleInRadians = 0, float coneOuterAngleInRadians = 0, float coneOuterGain = 0, float dopplerFactor = 0, float directionalAttenuationFactor = 0, float minSpatializationChannelGain = 0, uint gainSmoothTimeInFrames = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11138) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-cf5b7d073799650a"></a>[`public struct Inno.Native.MiniAudio.MaSpatializerConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11118) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSpatializerConfigPtr</code> · 31 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSpatializerConfigPtr e0e676611c6efda9727b3cb297b6426fa71a94651368ae6cfcca0bb4d5758f55 -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerConfigPtr.#ctor(Inno.Native.MiniAudio.MaSpatializerConfig*) 6ef2662ee64fea98691fd531b02bd18204a8f45f9a38fa98704cd17402c89040 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerConfigPtr.Handle a74ae7a9265de7925bcfe54951b85ef56fd7243f967477c52265e236d88ae4a2 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.IsNull d22bc3f37e865698637c68e32ca64119158fb743204bace40bee40af7d7abb26 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.Null b8f70996d6730f3344af890eeef2f995706c0666b90befa4e82fa871e6980bd8 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.Item(System.Int32) 6faa9394e4b24ce940dde59a558e2d5ab6528955a8911fbce5bf5abe85d87c31 -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaSpatializerConfig*)~Inno.Native.MiniAudio.MaSpatializerConfigPtr cca7efd94c6615ce0bd128cc325a628179a351090e0a0de8e5ba541e7dfdf8a2 -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaSpatializerConfigPtr)~Inno.Native.MiniAudio.MaSpatializerConfig* aa2b9f58f0a875c1c0b2c5639b8b813dc94b6e4ecb3a3ce555e6dabffa7c2fc2 -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerConfigPtr.op_Equality(Inno.Native.MiniAudio.MaSpatializerConfigPtr,Inno.Native.MiniAudio.MaSpatializerConfigPtr) 1db69aef209e470d1e98261d6cf886fafe3dcc2ce3dc947c5c647deb1e45deeb -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaSpatializerConfigPtr,Inno.Native.MiniAudio.MaSpatializerConfigPtr) e7de2da5ad5979d046b6239efe9c599daa94c4c18dbb2881b61b3b07ceef995a -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerConfigPtr.Equals(Inno.Native.MiniAudio.MaSpatializerConfigPtr) a6da68a35fba0b45a81dfe80e29b115629861298a5020b9978c66c2665e6cb5a -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerConfigPtr.Equals(System.Object) 111a216c66afb03c73d644142061b1d074f0fd69a49871541086fd3215a26ee9 -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerConfigPtr.GetHashCode a3e9b4044c61570fb7173f5f44907f660372b4d7de036734d4c20b253b43d3e2 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.ChannelsIn 3c8a548c558d78d79150322229c08f9ef9eebed818cdcaca9375025eb919520a -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.ChannelsOut 987c68857a51179cc0232330453c7fbfb777456dcfe348e596a0cf2f008b5aa2 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.PChannelMapIn 6133f4d388e1d523802b6281bf2472d194e627c48fb314bc82c9c70c5bde42b4 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.AttenuationModel b3b7267aa8e32f6528662307127326777928efc979754938784e28fed9eccbd8 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.Positioning 08ec48d6a8b7091a6ed64fbae2eb0bb3253acc3b40229302f67f92c272d9847e -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.Handedness 190a1cc99fedf348ca77ffdca87b00706bb35f2f6847e27c512e44c07c02ff74 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.MinGain ff58092cee1c4f52ee08ac7efa86c5b0000a99ec3d73f04e3668e05fef14e0a7 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.MaxGain 654222d91b08a797165bf5df8dd3d4468753ca1a878e2131ca7dec8e419fb94c -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.MinDistance 404080df068893dcf333bd6ea5956df085b5806a69d7ef45e612234d9e43de87 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.MaxDistance 2235b1c8201f987b4899dba9049130e257d06052c22e2b771794c5191f66c56d -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.Rolloff 291e965ffa4875ceb8eb4bd5f11cb360e1fae3af22ed99d1efde4ee439970cf7 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.ConeInnerAngleInRadians 46b62f62e46991bf14d950d69cc25450f26f8fa0e52a1030665b1a0a6ed12d99 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.ConeOuterAngleInRadians 5c75a87ae4a265297090b5aff342864c30301efc35f680ea6d9a7012e97b4e1e -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.ConeOuterGain 0b47c52bde24170d003633dffe4dd34d897df783d8ec067633982d958a35feca -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.DopplerFactor f4a3b0a5276da2eacba63f087ab89ff5f7dca2519bdbb7323e333c331c426596 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.DirectionalAttenuationFactor 61a25da3272fde57bd20dfe9b7278f3cc62c424efa0f6fc075b08db4b7570e4a -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.MinSpatializationChannelGain 3b76ae915482c76d013f690042ba887cc2d32800645913f2533753dfca29f22a -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerConfigPtr.GainSmoothTimeInFrames ca1d20d5f37d863bd18fc225196d6537f76f40e39c639de5342370bdecb5689d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-21536163b13a8f44"></a>[`public Inno.Native.MiniAudio.MaSpatializerConfig* Inno.Native.MiniAudio.MaSpatializerConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18266) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4378054d0225bc59"></a>[`public Inno.Native.MiniAudio.MaSpatializerConfigPtr.MaSpatializerConfigPtr(Inno.Native.MiniAudio.MaSpatializerConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18261) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-edd874c0283a37a7"></a>[`public bool Inno.Native.MiniAudio.MaSpatializerConfigPtr.Equals(Inno.Native.MiniAudio.MaSpatializerConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18276) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-23c50eeda9e73b94"></a>[`public override bool Inno.Native.MiniAudio.MaSpatializerConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18277) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-85b686065952869f"></a>[`public override int Inno.Native.MiniAudio.MaSpatializerConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18278) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-1e16d9281c8b45a2"></a>[`public static bool Inno.Native.MiniAudio.MaSpatializerConfigPtr.operator ==(Inno.Native.MiniAudio.MaSpatializerConfigPtr left, Inno.Native.MiniAudio.MaSpatializerConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18274) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-350a63f8f248895d"></a>[`public static Inno.Native.MiniAudio.MaSpatializerConfigPtr.implicit operator Inno.Native.MiniAudio.MaSpatializerConfigPtr(Inno.Native.MiniAudio.MaSpatializerConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18272) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-63d81db2d5074db2"></a>[`public static Inno.Native.MiniAudio.MaSpatializerConfigPtr.implicit operator Inno.Native.MiniAudio.MaSpatializerConfig*(Inno.Native.MiniAudio.MaSpatializerConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18273) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-597b9cb16381950e"></a>[`public static bool Inno.Native.MiniAudio.MaSpatializerConfigPtr.operator !=(Inno.Native.MiniAudio.MaSpatializerConfigPtr left, Inno.Native.MiniAudio.MaSpatializerConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18275) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-31f519974255b00a"></a>[`public ref Inno.Native.MiniAudio.MaAttenuationModel Inno.Native.MiniAudio.MaSpatializerConfigPtr.AttenuationModel { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18283) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-128f8ac0ead17c87"></a>[`public ref uint Inno.Native.MiniAudio.MaSpatializerConfigPtr.ChannelsIn { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18280) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-99e49c85d0e9c08f"></a>[`public ref uint Inno.Native.MiniAudio.MaSpatializerConfigPtr.ChannelsOut { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18281) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-659ab564ea2ba6ce"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerConfigPtr.ConeInnerAngleInRadians { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18291) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bca55493da1e372f"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerConfigPtr.ConeOuterAngleInRadians { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18292) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3e59e13648afaacb"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerConfigPtr.ConeOuterGain { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18293) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba5bbbc773ce834c"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerConfigPtr.DirectionalAttenuationFactor { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18295) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-48325f644118ae11"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerConfigPtr.DopplerFactor { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18294) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5a76db1ba3b63030"></a>[`public ref uint Inno.Native.MiniAudio.MaSpatializerConfigPtr.GainSmoothTimeInFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18297) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1fed4e1da847ea4c"></a>[`public ref Inno.Native.MiniAudio.MaHandedness Inno.Native.MiniAudio.MaSpatializerConfigPtr.Handedness { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18285) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fd21ef71cff4b30b"></a>[`public bool Inno.Native.MiniAudio.MaSpatializerConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18267) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2cee0686996da25c"></a>[`public Inno.Native.MiniAudio.MaSpatializerConfig Inno.Native.MiniAudio.MaSpatializerConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18270) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b8b40b5fafc22f6e"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerConfigPtr.MaxDistance { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18289) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8db5dfe817641c28"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerConfigPtr.MaxGain { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18287) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2d67938054a5acf0"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerConfigPtr.MinDistance { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18288) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0435cc3dd1ce04e2"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerConfigPtr.MinGain { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18286) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fb41547a42f04af6"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerConfigPtr.MinSpatializationChannelGain { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18296) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-31f8fbbb3c8fc8c5"></a>[`public static Inno.Native.MiniAudio.MaSpatializerConfigPtr Inno.Native.MiniAudio.MaSpatializerConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18268) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bb3aeda85fd5b334"></a>[`public byte* Inno.Native.MiniAudio.MaSpatializerConfigPtr.PChannelMapIn { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18282) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d7d86d67612accbd"></a>[`public ref Inno.Native.MiniAudio.MaPositioning Inno.Native.MiniAudio.MaSpatializerConfigPtr.Positioning { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18284) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-278ea7947e20ba90"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerConfigPtr.Rolloff { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18290) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f0d0cdd54d3b0582"></a>[`public struct Inno.Native.MiniAudio.MaSpatializerConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaSpatializerConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18259) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSpatializerListener</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSpatializerListener 7c8e78c77cc919855f3a4c756faefe199026526d70218a0bf1cbf8da28dfd6d3 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.Config 06d9fe80517b1ddc0d60fa4c4dc582cc3ec85e063d47b816c4a4598c4f14e4eb -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.Position 7d42f05e1f6d83a35d0f0f90f207c0cd24e8b410524700769394acd6fda99490 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.Direction 01ffe4476eb5e38def33dd0bd82b381e21e3e8d0d1ebc90d914ba8ad295b7af8 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.Velocity ccb21d896835463faca58fa1bff65b2e27800861cf954664028f7197d0694389 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.IsEnabled 50fb682c0183d142c874836110e92d1e0b64af07f46ae0ac3c3cb9e71ffb618d -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.Ownsheap 5ebbcbabadd9520a07747e0f2ae3f63da068bb1192ffee1243e801cfe77c7142 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListener.Pheap fe81aabc3e6f87bb69189a3ee8c40420d08b798148fe569efe0ffa90f867d4fd -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerListener.#ctor(Inno.Native.MiniAudio.MaSpatializerListenerConfig,Inno.Native.MiniAudio.MaAtomicVec3F,Inno.Native.MiniAudio.MaAtomicVec3F,Inno.Native.MiniAudio.MaAtomicVec3F,System.UInt32,System.UInt32,System.Void*) f67a4a3731d47b8e142ef9d49400c05e066428815a1c9b66416089ffb6d570cf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6dcb3bd57d9edc2f"></a>[`public Inno.Native.MiniAudio.MaSpatializerListenerConfig Inno.Native.MiniAudio.MaSpatializerListener.Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11095) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-783a161c467354ae"></a>[`public Inno.Native.MiniAudio.MaAtomicVec3F Inno.Native.MiniAudio.MaSpatializerListener.Direction`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11097) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-443e23fb1ae21611"></a>[`public uint Inno.Native.MiniAudio.MaSpatializerListener.IsEnabled`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11099) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e1d88903a792d386"></a>[`public uint Inno.Native.MiniAudio.MaSpatializerListener.Ownsheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11100) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f4451f5a50cfe4a0"></a>[`public void* Inno.Native.MiniAudio.MaSpatializerListener.Pheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11101) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-94c03972c143be1d"></a>[`public Inno.Native.MiniAudio.MaAtomicVec3F Inno.Native.MiniAudio.MaSpatializerListener.Position`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11096) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-97c755ef67bdc92c"></a>[`public Inno.Native.MiniAudio.MaAtomicVec3F Inno.Native.MiniAudio.MaSpatializerListener.Velocity`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11098) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-83fac287f9b1c06b"></a>[`public Inno.Native.MiniAudio.MaSpatializerListener.MaSpatializerListener(Inno.Native.MiniAudio.MaSpatializerListenerConfig config = default(Inno.Native.MiniAudio.MaSpatializerListenerConfig), Inno.Native.MiniAudio.MaAtomicVec3F position = default(Inno.Native.MiniAudio.MaAtomicVec3F), Inno.Native.MiniAudio.MaAtomicVec3F direction = default(Inno.Native.MiniAudio.MaAtomicVec3F), Inno.Native.MiniAudio.MaAtomicVec3F velocity = default(Inno.Native.MiniAudio.MaAtomicVec3F), uint isEnabled = 0, uint ownsHeap = 0, void* pHeap = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11102) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-98042f57a705f4a4"></a>[`public struct Inno.Native.MiniAudio.MaSpatializerListener`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11093) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSpatializerListenerConfig</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSpatializerListenerConfig b1d09d488873c9866ec446a8bb99841612a8dbb84a24c84d71e8dae4004689da -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.ChannelsOut d3c2b221a6a87d428cf5eb8ccca930621a43a4f6f226e861f3ce559223594beb -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.PChannelMapOut 6b95f8b7913bdeb8aa8deb33b161e86361d01c68346a7164cb54c24468731ab6 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.Handedness 2a6ccc5c4db25800d468c42ab9aa418af0d4556e0d0f48a73b6bed0bae680f51 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.ConeInnerAngleInRadians b64d8dd34ca54886dd33275b5f36b3b7704fd3fef1755493ca595582817dbfed -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.ConeOuterAngleInRadians 15bfe2c2ad68cf29b6085db7c36be0a45e62de4e2d11d971d6bea41b735b5c1a -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.ConeOuterGain 6080d8b1d74db971eccb68110e699217573158f7485772b5f9b6ae782538ade3 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.SpeedOfSound d38e018eec959c039aab791ad9a99d3bb6706c5d7e57a744de289fb2f6a10a53 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfig.WorldUp 0143c12c3a532150345b2873f92ebe11879b96c15c897dee6faf14d252daf4d6 -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerListenerConfig.#ctor(System.UInt32,System.Byte*,Inno.Native.MiniAudio.MaHandedness,System.Single,System.Single,System.Single,System.Single,Inno.Native.MiniAudio.MaVec3F) 74bf2ef7e6d5518c2d4ce26eaeb411a5459e4b22fb10e5aca90093e8266e554b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-72911cac7baaa9fa"></a>[`public uint Inno.Native.MiniAudio.MaSpatializerListenerConfig.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11068) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8f48319dfd7b6555"></a>[`public float Inno.Native.MiniAudio.MaSpatializerListenerConfig.ConeInnerAngleInRadians`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11071) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cb68ae19a7bc1517"></a>[`public float Inno.Native.MiniAudio.MaSpatializerListenerConfig.ConeOuterAngleInRadians`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11072) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4d51d40f6930d3f2"></a>[`public float Inno.Native.MiniAudio.MaSpatializerListenerConfig.ConeOuterGain`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11073) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a076f8748f805621"></a>[`public Inno.Native.MiniAudio.MaHandedness Inno.Native.MiniAudio.MaSpatializerListenerConfig.Handedness`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11070) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e3f753c60cdabf4"></a>[`public byte* Inno.Native.MiniAudio.MaSpatializerListenerConfig.PChannelMapOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11069) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-99b0c9b72d817e68"></a>[`public float Inno.Native.MiniAudio.MaSpatializerListenerConfig.SpeedOfSound`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11074) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d754bc1144989a2"></a>[`public Inno.Native.MiniAudio.MaVec3F Inno.Native.MiniAudio.MaSpatializerListenerConfig.WorldUp`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11075) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-039174ba164010f4"></a>[`public Inno.Native.MiniAudio.MaSpatializerListenerConfig.MaSpatializerListenerConfig(uint channelsOut = 0, byte* pChannelMapOut = null, Inno.Native.MiniAudio.MaHandedness handedness = Inno.Native.MiniAudio.MaHandedness.Right, float coneInnerAngleInRadians = 0, float coneOuterAngleInRadians = 0, float coneOuterGain = 0, float speedOfSound = 0, Inno.Native.MiniAudio.MaVec3F worldUp = default(Inno.Native.MiniAudio.MaVec3F))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11076) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2ee37cb3454b1f68"></a>[`public struct Inno.Native.MiniAudio.MaSpatializerListenerConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11066) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr</code> · 21 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr 9bd11430820871caa600aa1a360c982d14c7e96a6a8f0d83d2d09696cc125111 -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.#ctor(Inno.Native.MiniAudio.MaSpatializerListenerConfig*) b50b2baf2616c589a29df866f744c91a85bee7d6273c72963b4c789f58687819 -->
<!-- api: F:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.Handle 390d07211f306b84d00029bf85c5c6494328a9201ae24a53eef12be4f2e71cd8 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.IsNull a0b0fc3a12f29d16955f6892d159c8cb417887a4f78d7def5706e47c4bd93e4d -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.Null fb8c6d9d97d0af9c50b9ed8d1fcb61dc1e9666c1142087013570c2dafdfede6b -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.Item(System.Int32) e6b6333a5dc462d2478dcc2de3193fe24a42b430eaa0428a5046686ab9ca4b26 -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaSpatializerListenerConfig*)~Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr abec6163fadc66dd65061b0d08891495a76069b89f9430c61e9980dc4cc9858e -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr)~Inno.Native.MiniAudio.MaSpatializerListenerConfig* 1774c2907bcce3e1b066d1fced64a064ae682e414d9bc157242816b8967e4cad -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.op_Equality(Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr,Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr) b30934c2d795c288ae8685647a4614046402a7a418a82466b0026c523991ec8d -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr,Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr) 4c144780a98b0cefc332f27339be21a20d4d7240e9afc4e80f4275a017a9c825 -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.Equals(Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr) 9e85bde601261ee35f8d2d13692652fa4a696c5efa90529e5a92f2acf7f29e33 -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.Equals(System.Object) 2b2782b54fccf1da1e0ad71187b03ccecebda607bc79e1da80657f4f17cbd7e5 -->
<!-- api: M:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.GetHashCode 32b74e8331fc09f5af6e8398c6a9eb831f3b789e2077301791b168b3649dd5f5 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.ChannelsOut a0888d85d05c4b32c0aea26d5350db17b44ebc30d3a8fae0ee368229b9a6f48a -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.PChannelMapOut fbf1e44a18f60a8d7e703509463ab30b7df4c2a32a1006231472cf61c2eabbd3 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.Handedness 8bfff03cf5da5975daec6b0e540709c8aaedcc69ad2637a60bb58afe0c8eea43 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.ConeInnerAngleInRadians 7b9f1215bd140c5d0ea91cbff2dfe0f788e77a627d474dc2765d67c473462a2e -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.ConeOuterAngleInRadians fbe884a857fe445e5ce3975ea1acf8325d936d6f3041cfdaa6971cd04280235c -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.ConeOuterGain f54520e9fa79d75ac082642f3da7c4bc0c74fc5a88d24c7e1f572012adc3ed44 -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.SpeedOfSound 517bce1fbac88c28c8d87b63fc7a19acda2b760b25159b7aa32dbf3029928a5c -->
<!-- api: P:Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.WorldUp 4b6ed049377b03a1f1845dfe15704a0f4fc98bafa9b3f20ec3c75d8380297afc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5481e0af8d4332d9"></a>[`public Inno.Native.MiniAudio.MaSpatializerListenerConfig* Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18308) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-743eb194f8601250"></a>[`public Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.MaSpatializerListenerConfigPtr(Inno.Native.MiniAudio.MaSpatializerListenerConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18303) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-c4979860348abaf5"></a>[`public bool Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.Equals(Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18318) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-778d6b536a3308de"></a>[`public override bool Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18319) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-a2cff62b79c8da2c"></a>[`public override int Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18320) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-93aadf7954ebbc36"></a>[`public static bool Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.operator ==(Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr left, Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18316) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-86f7ac82a1d62bf5"></a>[`public static Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.implicit operator Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr(Inno.Native.MiniAudio.MaSpatializerListenerConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18314) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e32a7e969bb469a4"></a>[`public static Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.implicit operator Inno.Native.MiniAudio.MaSpatializerListenerConfig*(Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18315) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ac41ae14b51f45fb"></a>[`public static bool Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.operator !=(Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr left, Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18317) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-307ce3345cbffdd2"></a>[`public ref uint Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.ChannelsOut { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18322) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-144ee62abcf2c46a"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.ConeInnerAngleInRadians { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18325) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bd7f4c4638a8ecf0"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.ConeOuterAngleInRadians { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18326) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5ef165fbfa8ce921"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.ConeOuterGain { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18327) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0b627e962a22ae33"></a>[`public ref Inno.Native.MiniAudio.MaHandedness Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.Handedness { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18324) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5c0cbdcf556c5345"></a>[`public bool Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18309) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1ec2aef876d3e590"></a>[`public Inno.Native.MiniAudio.MaSpatializerListenerConfig Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18312) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eab47701420f7310"></a>[`public static Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18310) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-93dc8c2cf8c4cdb5"></a>[`public byte* Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.PChannelMapOut { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18323) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-388ad35f1ba99bef"></a>[`public ref float Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.SpeedOfSound { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18328) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ae9f5857078a50ee"></a>[`public ref Inno.Native.MiniAudio.MaVec3F Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr.WorldUp { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18329) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ad24fd79dec78121"></a>[`public struct Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaSpatializerListenerConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L18301) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
