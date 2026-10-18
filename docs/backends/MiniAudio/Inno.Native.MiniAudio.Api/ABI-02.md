# Inno.Native.MiniAudio ABI · MaBpfPtr → MaDataSourceNodeConfig

[项目与生命周期](../Inno.Native.MiniAudio.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.MiniAudio.MaBpfPtr</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBpfPtr 60faf8dfa335a181f1ca0a7ff205f2e08082b2f7f3a78566ae567d9ce8daea99 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfPtr.#ctor(Inno.Native.MiniAudio.MaBpf*) 65da3872461f2129005027171c57d089935cdb899e9b270451c70a146d84362b -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfPtr.Handle 032789416db640cfde7fdf610fcfe29283f2660f32bd3fdd03536ab6a49ee0ae -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfPtr.IsNull af724337baf59929e036ea98a7a39d32438e150d9398e20e8de34f25cd102d65 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfPtr.Null 1a16369b636ed38c8f05e09900999b0a967f733cb22defd6f154767f00f60aa0 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfPtr.Item(System.Int32) 80399aae44a6c7862b6431f9e3f49205f6a43073cdd9266948ddd09eaa3fdc2d -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfPtr.op_Implicit(Inno.Native.MiniAudio.MaBpf*)~Inno.Native.MiniAudio.MaBpfPtr 0c0236685125d3510f0ac7d44271db0e252a09faab7f9a81a394cfbfa97eb4b4 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfPtr.op_Implicit(Inno.Native.MiniAudio.MaBpfPtr)~Inno.Native.MiniAudio.MaBpf* f84987158e013baa62500ed2d250f411c8b1663b6ce21017a9df0b3cfcc212f4 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfPtr.op_Equality(Inno.Native.MiniAudio.MaBpfPtr,Inno.Native.MiniAudio.MaBpfPtr) 75e4281c2cd509645cc52da1d21421ba51c99453402f8f2b648fc3e48a18cc84 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfPtr.op_Inequality(Inno.Native.MiniAudio.MaBpfPtr,Inno.Native.MiniAudio.MaBpfPtr) 43e68833ac5c2ec3f8b4e942e414684bab7d603de4e1e8e9356bb7c51fc0c322 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfPtr.Equals(Inno.Native.MiniAudio.MaBpfPtr) 2c9c86ff6a894912681efbfadc6710c498260569677526d5d732c3274e1ac03f -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfPtr.Equals(System.Object) fc4a9c1c3fefb840a196a4dbbaca93afb599aeff08d1585b2bfe4b76f6c8ec70 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfPtr.GetHashCode 22b7e7e82ba7da42e579c1c4c91906b599af72eb92426112ff834a55331f9502 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfPtr.Format ec229685f5a2e0258c01f29914c7101c9101fb2affa4f15eacc56df35be9abb7 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfPtr.Channels 0806038e1a57d87658b6a83d2f3f61db190308c697eca009324f7a8abb6524f4 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfPtr.Bpf2Count 98de003f37db4f1f34e1469b340805a9e017905a351f5479c80ad54f2a6089ff -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfPtr.PBpf2 0e2bacb7da8ac293387435a8a18fddf2cfd1f935b0cbb5965feade1a54e36f54 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfPtr.Pheap 8b5994d1357c735ccc5d93c02496887c9e8b836e17953ae68f084c7d0b390c63 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfPtr.Ownsheap b2e8214a6abb2add6fdbdeafd85c3ca97cc951d2c390740084a442263c4da3d1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d9267e664025f4f0"></a>[`public Inno.Native.MiniAudio.MaBpf* Inno.Native.MiniAudio.MaBpfPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14833) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7eb4b4a8970865b8"></a>[`public Inno.Native.MiniAudio.MaBpfPtr.MaBpfPtr(Inno.Native.MiniAudio.MaBpf* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14828) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-df8353e14dbe802f"></a>[`public bool Inno.Native.MiniAudio.MaBpfPtr.Equals(Inno.Native.MiniAudio.MaBpfPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14843) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-988bb16a55282d22"></a>[`public override bool Inno.Native.MiniAudio.MaBpfPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14844) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-a797944ff4ab3d90"></a>[`public override int Inno.Native.MiniAudio.MaBpfPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14845) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-5487ef444a182af7"></a>[`public static bool Inno.Native.MiniAudio.MaBpfPtr.operator ==(Inno.Native.MiniAudio.MaBpfPtr left, Inno.Native.MiniAudio.MaBpfPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14841) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5f6ecc1c9311dcec"></a>[`public static Inno.Native.MiniAudio.MaBpfPtr.implicit operator Inno.Native.MiniAudio.MaBpfPtr(Inno.Native.MiniAudio.MaBpf* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14839) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f537d8c46c8efcaf"></a>[`public static Inno.Native.MiniAudio.MaBpfPtr.implicit operator Inno.Native.MiniAudio.MaBpf*(Inno.Native.MiniAudio.MaBpfPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14840) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dbf8443d983cfdb1"></a>[`public static bool Inno.Native.MiniAudio.MaBpfPtr.operator !=(Inno.Native.MiniAudio.MaBpfPtr left, Inno.Native.MiniAudio.MaBpfPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14842) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ce98662e07e50dd2"></a>[`public ref uint Inno.Native.MiniAudio.MaBpfPtr.Bpf2Count { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14849) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eb56666c10dcd33a"></a>[`public ref uint Inno.Native.MiniAudio.MaBpfPtr.Channels { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14848) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9ae6826792220a52"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBpfPtr.Format { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14847) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bb2bb5b8961e1981"></a>[`public bool Inno.Native.MiniAudio.MaBpfPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14834) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-38e7850884aeed51"></a>[`public Inno.Native.MiniAudio.MaBpf Inno.Native.MiniAudio.MaBpfPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14837) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-df1569b0bdfe4282"></a>[`public static Inno.Native.MiniAudio.MaBpfPtr Inno.Native.MiniAudio.MaBpfPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14835) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f292063845e85ac3"></a>[`public ref uint Inno.Native.MiniAudio.MaBpfPtr.Ownsheap { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14852) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4bfd14b561bcdb4b"></a>[`public ref Inno.Native.MiniAudio.MaBpf2Ptr Inno.Native.MiniAudio.MaBpfPtr.PBpf2 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14850) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-abf054925f1849ca"></a>[`public void* Inno.Native.MiniAudio.MaBpfPtr.Pheap { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14851) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c3a717eb0d25f322"></a>[`public struct Inno.Native.MiniAudio.MaBpfPtr : System.IEquatable<Inno.Native.MiniAudio.MaBpfPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14826) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaChannelConversionPath</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaChannelConversionPath 0c7630d4378e63bdee14ec97c174a0f72bdb3ce16147f6e718c4272d1527675a -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConversionPath.Unknown 93869e2c0c18a5cc9ffe8aa427f53a666f9a9da7a5b133abe76bdf331b73c2ab -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConversionPath.Passthrough f7540b77b899b4e8e0b1d1d28c3928be8a5c01232bb451a2858e430f37a9b9da -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConversionPath.MonoOut aaf2427b325db1181c5cd4f07f9cba6d4dc0d35e79f3de39f784286fdf8652a1 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConversionPath.MonoIn a71c94c259da71cafce036aabac83049183785a66fee4b6fbceb4067eda1c149 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConversionPath.Shuffle b45443cc5528b645b29c36bab0e9bdb1d4d152a7fe16dab985f94de161893ab7 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConversionPath.Weights af112ccd7138af35206f37000360fc3e3563e20266551bacc2438b5337a137e3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-75b02d83c357592d"></a>[`Inno.Native.MiniAudio.MaChannelConversionPath.MonoIn = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L279) | 当前常量或枚举值：`3`。 | — |
| <a id="api-3f4b8d4228effa45"></a>[`Inno.Native.MiniAudio.MaChannelConversionPath.MonoOut = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L278) | 当前常量或枚举值：`2`。 | — |
| <a id="api-3019406f6081f7a0"></a>[`Inno.Native.MiniAudio.MaChannelConversionPath.Passthrough = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L277) | 当前常量或枚举值：`1`。 | — |
| <a id="api-6df23939438a97b4"></a>[`Inno.Native.MiniAudio.MaChannelConversionPath.Shuffle = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L280) | 当前常量或枚举值：`4`。 | — |
| <a id="api-a9bb4a1931390e66"></a>[`Inno.Native.MiniAudio.MaChannelConversionPath.Unknown = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L276) | 当前常量或枚举值：`0`。 | — |
| <a id="api-4bfc0dff54adb224"></a>[`Inno.Native.MiniAudio.MaChannelConversionPath.Weights = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L281) | 当前常量或枚举值：`5`。 | — |
| <a id="api-9225a94bb24abfe5"></a>[`public enum Inno.Native.MiniAudio.MaChannelConversionPath : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L274) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaChannelConverter</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaChannelConverter 55db3ace338f9458ab3e21e4b9a3c2d25e5092075b64a3a5b340f76f6202295f -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.Format 73021555e88b2fda7e96c34157813f1b332eda9198b4f694c3d6bd843d0addbc -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.ChannelsIn 82872fb29be4de352199fd7082a86d708a5559affdb1e8ab44125e3a913436c7 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.ChannelsOut d7d2ffdc4a4de62748748d58e1ee9b1ea90c9a72168fd99a8e7b923154673619 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.MixingMode 2b3a8674eeaa83b17c34b937027d318b07c5ec8bba8b4e919cc37d2b4738c63c -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.ConversionPath ae20a940d2f340ba71c54cfb6e75fff4f70d31da107f5070d52ea52d438e043d -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.PChannelMapIn 8c2993c2f82601804d88c24b2a959a011f842cfee160653b952eb172f9cd9d12 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.PChannelMapOut f65a3ab0d9992da5412af307f3cd7286e308633b8b503d739d52bbb6a0335b36 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.PShuffleTable 8d9ea99c98d0e5caab5207086532b12226f2fccbc2c8fd3209534576f871b6db -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.Weights a8bf077bed090c0a6156f4c3c3b97b99e8bf3494d937312d8cd2d9dcded0b7dd -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.Pheap f4d1d4a27d85a1fce2110b35828edce641d18ec750b19aa6c55d0ff81ace7860 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.Ownsheap 5e5933e9e163692dc34799251e2cc2957a28b28c3273285710683e684a46406c -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverter.#ctor(Inno.Native.MiniAudio.MaFormat,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaChannelMixMode,Inno.Native.MiniAudio.MaChannelConversionPath,System.Byte*,System.Byte*,System.Byte*,Inno.Native.MiniAudio.MaChannelConverter.WeightsAnonymous,System.Void*,System.UInt32) b39614144a9ea69296bc34e3cd8cfb41b610526cdc7df355e2a212086cd42b09 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e2cba1118021b816"></a>[`public uint Inno.Native.MiniAudio.MaChannelConverter.ChannelsIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5410) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-403167225e6c400a"></a>[`public uint Inno.Native.MiniAudio.MaChannelConverter.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5411) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-053d9c87782fc877"></a>[`public Inno.Native.MiniAudio.MaChannelConversionPath Inno.Native.MiniAudio.MaChannelConverter.ConversionPath`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5413) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3897f31ce2c91ac8"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaChannelConverter.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5409) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-181b3fe145a33673"></a>[`public Inno.Native.MiniAudio.MaChannelMixMode Inno.Native.MiniAudio.MaChannelConverter.MixingMode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5412) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-657db77e23a4b5a6"></a>[`public uint Inno.Native.MiniAudio.MaChannelConverter.Ownsheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5419) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-304cc3054e1d1319"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverter.PChannelMapIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5414) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e72b3dd4f47d27ec"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverter.PChannelMapOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5415) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f2523a293e6c4e8"></a>[`public void* Inno.Native.MiniAudio.MaChannelConverter.Pheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5418) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e72a5d133444a90"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverter.PShuffleTable`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5416) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e50acfa353b8964e"></a>[`public Inno.Native.MiniAudio.MaChannelConverter.WeightsAnonymous Inno.Native.MiniAudio.MaChannelConverter.Weights`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5417) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-495e6ebc88243e14"></a>[`public Inno.Native.MiniAudio.MaChannelConverter.MaChannelConverter(Inno.Native.MiniAudio.MaFormat format = Inno.Native.MiniAudio.MaFormat.Unknown, uint channelsIn = 0, uint channelsOut = 0, Inno.Native.MiniAudio.MaChannelMixMode mixingMode = Inno.Native.MiniAudio.MaChannelMixMode.Default, Inno.Native.MiniAudio.MaChannelConversionPath conversionPath = Inno.Native.MiniAudio.MaChannelConversionPath.Unknown, byte* pChannelMapIn = null, byte* pChannelMapOut = null, byte* pShuffleTable = null, Inno.Native.MiniAudio.MaChannelConverter.WeightsAnonymous weights = default(Inno.Native.MiniAudio.MaChannelConverter.WeightsAnonymous), void* pHeap = null, uint ownsHeap = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5420) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f230c0381d751199"></a>[`public struct Inno.Native.MiniAudio.MaChannelConverter`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5395) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaChannelConverter.WeightsAnonymous</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaChannelConverter.WeightsAnonymous 32827995b893e2623272665590cbfb3ff955a9233eab527f68b8ab6a9d72e5cb -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.WeightsAnonymous.F32 f58eb61bef15c3459adee3094ca6178eda2e295f389a37d160dc1c4a6dea49fd -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverter.WeightsAnonymous.S16 a8dc3418b402b4b50497d82a2de7dbafae6b569368e921e942bea56402f83e2f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e062f75b134751ab"></a>[`public float** Inno.Native.MiniAudio.MaChannelConverter.WeightsAnonymous.F32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5404) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc04e090a47a96c7"></a>[`public int** Inno.Native.MiniAudio.MaChannelConverter.WeightsAnonymous.S16`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5406) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1896c05ce5d55c26"></a>[`public struct Inno.Native.MiniAudio.MaChannelConverter.WeightsAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5401) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaChannelConverterConfig</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaChannelConverterConfig 8fadc5783387ea50b8391cd53067f34c75cfb327215baca94bc52e51838adbcd -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.Format b4a952498fa582c711fe8a11fd299aa6e47809815e098c14e4b3279f1f04b6d7 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.ChannelsIn 5b7a2ca6bf5c96adc3b8ac3a4a2c4c266aed638d63aeeb5da4dd50928fd8b698 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.ChannelsOut 2e5f5806139f61e6fb91f9d7f85dfd5f6e53a3355b26231b5d48e8d692c3f0c5 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.PChannelMapIn 2191d1bf4315a97278f6f4b6b07b47f59e496f256684f4baa8ff8a5dbd5ecf0a -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.PChannelMapOut 076ab55238f05d66a5d7fcb3817f9e692d7222a2f9cd00fbbbd1127e0671db2a -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.MixingMode 88482c2d09642602d158e6f5b63ccfb15d2f0a85ff2803ac7c5b50fca4bdcb68 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.CalculateLfeFromSpatialChannels 9d27071351cc2b5038989f4d0fe21a81eba423c4589e043e1d339a8471ac011f -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfig.PpWeights db755ab9d6b4656d9d2eb46c37ff3fcd1528ea56226909a43d7914849bc9bbe7 -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterConfig.#ctor(Inno.Native.MiniAudio.MaFormat,System.UInt32,System.UInt32,System.Byte*,System.Byte*,Inno.Native.MiniAudio.MaChannelMixMode,System.UInt32,System.Single**) f69cb36d7ac5d951f5712cf07844942923c19e44c5e3dc4420c9257eb504147e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e5349bfbedd340b7"></a>[`public uint Inno.Native.MiniAudio.MaChannelConverterConfig.CalculateLfeFromSpatialChannels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11236) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4fd9bac4d7dbb4c5"></a>[`public uint Inno.Native.MiniAudio.MaChannelConverterConfig.ChannelsIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11231) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc6f7c20c17e32a6"></a>[`public uint Inno.Native.MiniAudio.MaChannelConverterConfig.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11232) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa83b1808418278a"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaChannelConverterConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11230) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f8a527725bcfe910"></a>[`public Inno.Native.MiniAudio.MaChannelMixMode Inno.Native.MiniAudio.MaChannelConverterConfig.MixingMode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11235) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1433e65a57125d7"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverterConfig.PChannelMapIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11233) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b78a222875a06125"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverterConfig.PChannelMapOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11234) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d52831761392d25a"></a>[`public float** Inno.Native.MiniAudio.MaChannelConverterConfig.PpWeights`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11237) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58a4366f8b267777"></a>[`public Inno.Native.MiniAudio.MaChannelConverterConfig.MaChannelConverterConfig(Inno.Native.MiniAudio.MaFormat format = Inno.Native.MiniAudio.MaFormat.Unknown, uint channelsIn = 0, uint channelsOut = 0, byte* pChannelMapIn = null, byte* pChannelMapOut = null, Inno.Native.MiniAudio.MaChannelMixMode mixingMode = Inno.Native.MiniAudio.MaChannelMixMode.Default, uint calculateLFEFromSpatialChannels = 0, float** ppWeights = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11238) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e560e68248295965"></a>[`public struct Inno.Native.MiniAudio.MaChannelConverterConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11228) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaChannelConverterConfigPtr</code> · 21 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaChannelConverterConfigPtr a373808097658d32a7ad02ff05e47dbf2e334d112e0d5c324a3f1a6f70db2b2e -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.#ctor(Inno.Native.MiniAudio.MaChannelConverterConfig*) 66785127feb3e059158aa1f36469f06e4fdd30d30ad99df158de4bff827030cb -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.Handle bb2e2d5ffd14c64fa6c5c7ec2413da1c3e937e2b2ed532418f14a4a98cc2f63a -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.IsNull 2b6c2a05a9b94d3bd219912dc66b685ac0e19551a89f12df98691a1d0ce22daf -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.Null 3709d3d1b52574e3d0ae26cbe5eee151e674c2957e2b7f8d8bfefa895fd877aa -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.Item(System.Int32) 97ab461e49208eb2dcc4aaa1275574ee44486c5596fd01883ff7f5e2fda0fab4 -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaChannelConverterConfig*)~Inno.Native.MiniAudio.MaChannelConverterConfigPtr 33663c68173a5e435e39edd80325f4f00c46ecefc96e49fb6e4ce9fe24598f32 -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaChannelConverterConfigPtr)~Inno.Native.MiniAudio.MaChannelConverterConfig* bdc6cbeefdf922bc5fdae90e2bdd20dec5600a1f37c6d88349342495e7923998 -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.op_Equality(Inno.Native.MiniAudio.MaChannelConverterConfigPtr,Inno.Native.MiniAudio.MaChannelConverterConfigPtr) 6e9db6a446a3ec45189e23a3a25e9ad74236cf34ee87b324b3128d3a040c57fd -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaChannelConverterConfigPtr,Inno.Native.MiniAudio.MaChannelConverterConfigPtr) e27773a3306cb5da98d9f157a92e28361ea0ed02ed5659db9a780189c916606f -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.Equals(Inno.Native.MiniAudio.MaChannelConverterConfigPtr) 99bbe679c0be06d53fa2904db3dd59603a866ab498fcf35d043c1fcd7a18c879 -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.Equals(System.Object) 6a5ad1f4d1fa918b67167d97bc6db10ddab85cdf588bb290bef944e1305a8722 -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.GetHashCode cacffd70f055385256cbde7dc6c9e841921f0963d0284416892a5b108385207c -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.Format 69d74a9cb585b767a45a3564857b36e1546cfe7292644801ceba649ad8c72a46 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.ChannelsIn bb04b8a00abd6a70ba38dab7f001ad68c6526cddc64e73acbe3c3fb04836a459 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.ChannelsOut 5643e56ba4312520257761c17c71eca92e32ecb2fd0c5e86ed896a0bc452ba38 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.PChannelMapIn e8741de46784ad3c2a083aff8003d24dc358fe033da369351d8e721f5fa444a5 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.PChannelMapOut 2e67016e00ef74dc34842143292bacd0b896706f96f23f986804aea34934d886 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.MixingMode 841954f955f8a6091ca9bf60bb3312203d075dd3d081a77f24ec9e25bd7247a9 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.CalculateLfeFromSpatialChannels d0ced1d113e59cbea74dafb5e33b54c32e6889da5484de1d38000e0ed4840e47 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterConfigPtr.PpWeights e36d24fb4c975f651fa744c4502b51300897186ee62af9496688c9d9dd6431d8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-24a9c8e7bf80df85"></a>[`public Inno.Native.MiniAudio.MaChannelConverterConfig* Inno.Native.MiniAudio.MaChannelConverterConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14863) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3baa7318162bc099"></a>[`public Inno.Native.MiniAudio.MaChannelConverterConfigPtr.MaChannelConverterConfigPtr(Inno.Native.MiniAudio.MaChannelConverterConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14858) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e1a371ae86c7829b"></a>[`public bool Inno.Native.MiniAudio.MaChannelConverterConfigPtr.Equals(Inno.Native.MiniAudio.MaChannelConverterConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14873) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-fbfa7f249e04a6cf"></a>[`public override bool Inno.Native.MiniAudio.MaChannelConverterConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14874) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-090061d7fd69b4e2"></a>[`public override int Inno.Native.MiniAudio.MaChannelConverterConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14875) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-6ff0828f4297352f"></a>[`public static bool Inno.Native.MiniAudio.MaChannelConverterConfigPtr.operator ==(Inno.Native.MiniAudio.MaChannelConverterConfigPtr left, Inno.Native.MiniAudio.MaChannelConverterConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14871) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fcbac2f356e4cbc0"></a>[`public static Inno.Native.MiniAudio.MaChannelConverterConfigPtr.implicit operator Inno.Native.MiniAudio.MaChannelConverterConfigPtr(Inno.Native.MiniAudio.MaChannelConverterConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14869) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2ed140ca7dd502ec"></a>[`public static Inno.Native.MiniAudio.MaChannelConverterConfigPtr.implicit operator Inno.Native.MiniAudio.MaChannelConverterConfig*(Inno.Native.MiniAudio.MaChannelConverterConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14870) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a9480df4a85fb5c9"></a>[`public static bool Inno.Native.MiniAudio.MaChannelConverterConfigPtr.operator !=(Inno.Native.MiniAudio.MaChannelConverterConfigPtr left, Inno.Native.MiniAudio.MaChannelConverterConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14872) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d925ea2eddf52a34"></a>[`public ref uint Inno.Native.MiniAudio.MaChannelConverterConfigPtr.CalculateLfeFromSpatialChannels { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14883) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b69204d79d6af258"></a>[`public ref uint Inno.Native.MiniAudio.MaChannelConverterConfigPtr.ChannelsIn { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14878) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cc6f2cc0ee2e45dc"></a>[`public ref uint Inno.Native.MiniAudio.MaChannelConverterConfigPtr.ChannelsOut { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14879) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1bbb7e65a60b782f"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaChannelConverterConfigPtr.Format { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14877) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-54f417e3265e8c4d"></a>[`public bool Inno.Native.MiniAudio.MaChannelConverterConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14864) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ab9faaefd34c5914"></a>[`public Inno.Native.MiniAudio.MaChannelConverterConfig Inno.Native.MiniAudio.MaChannelConverterConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14867) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-66ecf94e5e5cd0d1"></a>[`public ref Inno.Native.MiniAudio.MaChannelMixMode Inno.Native.MiniAudio.MaChannelConverterConfigPtr.MixingMode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14882) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aaed0e6591f14a43"></a>[`public static Inno.Native.MiniAudio.MaChannelConverterConfigPtr Inno.Native.MiniAudio.MaChannelConverterConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14865) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c2c4c784af506ac0"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverterConfigPtr.PChannelMapIn { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14880) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a8bd122591a831c9"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverterConfigPtr.PChannelMapOut { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14881) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-78e9c686ba398c54"></a>[`public float** Inno.Native.MiniAudio.MaChannelConverterConfigPtr.PpWeights { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14884) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0f9e9d1df20ba7b0"></a>[`public struct Inno.Native.MiniAudio.MaChannelConverterConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaChannelConverterConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14856) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaChannelConverterPtr</code> · 24 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaChannelConverterPtr 1bdf8b823b5a354cdf7a4e0fda2bfecd1c9cd05ab1129fe62c4d68f9db69aa74 -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterPtr.#ctor(Inno.Native.MiniAudio.MaChannelConverter*) 789e9842fefd4498fffcd6c3bfc336146a712a39758b02fde596dbbe9b1ec7b3 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelConverterPtr.Handle 3c45153b83c0d75ce0ed1c5fe439bd92c5e8767e0d0c74e827ff89c19ae2d9ff -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.IsNull 600ab18a4a29dacdbd8cd01f1d22a3b0b6afea1a8e70ae40a98ffdfb760576e7 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.Null 0658c713fc7f75b2a45061b6a9d4c3272d0bc3803cc527841eb3d30c75d830c5 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.Item(System.Int32) 0fa6f6aec92848bd89f14d3a475490bfc9c7b357fa79f6a43b980c5f833a44f0 -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterPtr.op_Implicit(Inno.Native.MiniAudio.MaChannelConverter*)~Inno.Native.MiniAudio.MaChannelConverterPtr c21cae146e44ac64dccd407446e54ac331065e87a547283b06e984ca600c67ca -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterPtr.op_Implicit(Inno.Native.MiniAudio.MaChannelConverterPtr)~Inno.Native.MiniAudio.MaChannelConverter* f3cd50b8dec834253eb362a3426b0c6ad3a9c3d4406b9121d17e4acd67f69dba -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterPtr.op_Equality(Inno.Native.MiniAudio.MaChannelConverterPtr,Inno.Native.MiniAudio.MaChannelConverterPtr) c31d5e9bd25c798bc44f03eb8cf89e0a186d10e76558230a6279dcaef40dc120 -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterPtr.op_Inequality(Inno.Native.MiniAudio.MaChannelConverterPtr,Inno.Native.MiniAudio.MaChannelConverterPtr) 466efdd851085e7e97f4b30e93f36903744df6edb208bbcb1c370251e10e72d1 -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterPtr.Equals(Inno.Native.MiniAudio.MaChannelConverterPtr) 5c5ac722c00d24e216431f73301c21d9b6863a2db8ab727d9c2f8f2f4195f964 -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterPtr.Equals(System.Object) d708ed5a2fa78bd4ffe70ad6658908b6a6c843d5cdc93da75f0dd37f335fc92b -->
<!-- api: M:Inno.Native.MiniAudio.MaChannelConverterPtr.GetHashCode 9affdef9a80f982b0b28286f9ca4c73fadf1356234117a53ae242740cf4e49b2 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.Format 52fd60c5a016b50dfada35553b02b8be9b0b473a671773ad560515c7c89bd540 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.ChannelsIn 0c2002d3b7cc3e6808db0229d7c88fbc5b78f40e68cc9193663cf1be23d5ea1b -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.ChannelsOut 80468607a5b251fd8fc367bbd30628ab34982898c1e540e4ea154379edf3ad98 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.MixingMode 61a1490c794427c23de71b8b1224368a6b50f0cf322a24eca1a93ec857efa147 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.ConversionPath c4667d641f15e7f36d65e5f4ccdde55bf852cc782ae831db02b45874fd10d7a6 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.PChannelMapIn b694acc0e6d00b14bb80cbcdf08024026956744cef6362cde377313336e386dc -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.PChannelMapOut 0b41ecda0dac2a76914c38cf4cf4dc7af1f835016d9b7c67b693544933a75cd2 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.PShuffleTable 6810d99385c0bf5dd66059382e4e69cc6616539f97cb20922333257d3bfe2bc7 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.Weights 88f0d7db276e23b366fe25647d03a5ac9312c1298aaf6bf7f7a29f4bcb8753b8 -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.Pheap 4739c31dc159bb8ce5869c1c4446f518f89b06a4a9884cf8627957329716d2cb -->
<!-- api: P:Inno.Native.MiniAudio.MaChannelConverterPtr.Ownsheap 0ee964299dba1f5570b8ba4f63ced114ad58775405308bb683e73d65c4dc58fc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f3b97130fb954043"></a>[`public Inno.Native.MiniAudio.MaChannelConverter* Inno.Native.MiniAudio.MaChannelConverterPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14895) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-721b5462fe61fb6f"></a>[`public Inno.Native.MiniAudio.MaChannelConverterPtr.MaChannelConverterPtr(Inno.Native.MiniAudio.MaChannelConverter* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14890) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-48a272e09fd5db0d"></a>[`public bool Inno.Native.MiniAudio.MaChannelConverterPtr.Equals(Inno.Native.MiniAudio.MaChannelConverterPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14905) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-b0505fa7cebfdbed"></a>[`public override bool Inno.Native.MiniAudio.MaChannelConverterPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14906) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-c40eb3856166d095"></a>[`public override int Inno.Native.MiniAudio.MaChannelConverterPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14907) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-83601fef9daee1e5"></a>[`public static bool Inno.Native.MiniAudio.MaChannelConverterPtr.operator ==(Inno.Native.MiniAudio.MaChannelConverterPtr left, Inno.Native.MiniAudio.MaChannelConverterPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14903) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2adf39aebd0e6cdb"></a>[`public static Inno.Native.MiniAudio.MaChannelConverterPtr.implicit operator Inno.Native.MiniAudio.MaChannelConverterPtr(Inno.Native.MiniAudio.MaChannelConverter* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14901) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-635412dc906dd9a2"></a>[`public static Inno.Native.MiniAudio.MaChannelConverterPtr.implicit operator Inno.Native.MiniAudio.MaChannelConverter*(Inno.Native.MiniAudio.MaChannelConverterPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14902) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9dc9a863f5771aaa"></a>[`public static bool Inno.Native.MiniAudio.MaChannelConverterPtr.operator !=(Inno.Native.MiniAudio.MaChannelConverterPtr left, Inno.Native.MiniAudio.MaChannelConverterPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14904) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-466643d87cc9aa83"></a>[`public ref uint Inno.Native.MiniAudio.MaChannelConverterPtr.ChannelsIn { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14910) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bb03d5c0d5e4c20d"></a>[`public ref uint Inno.Native.MiniAudio.MaChannelConverterPtr.ChannelsOut { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14911) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-96baf34d7fca72e7"></a>[`public ref Inno.Native.MiniAudio.MaChannelConversionPath Inno.Native.MiniAudio.MaChannelConverterPtr.ConversionPath { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14913) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-06d442974658eb81"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaChannelConverterPtr.Format { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14909) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d4e1b67e025d4552"></a>[`public bool Inno.Native.MiniAudio.MaChannelConverterPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14896) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f70eaaa58e060484"></a>[`public Inno.Native.MiniAudio.MaChannelConverter Inno.Native.MiniAudio.MaChannelConverterPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14899) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8a16572aa4749fcd"></a>[`public ref Inno.Native.MiniAudio.MaChannelMixMode Inno.Native.MiniAudio.MaChannelConverterPtr.MixingMode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14912) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a285f25dfed7f992"></a>[`public static Inno.Native.MiniAudio.MaChannelConverterPtr Inno.Native.MiniAudio.MaChannelConverterPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14897) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1ddbcc7e8f967580"></a>[`public ref uint Inno.Native.MiniAudio.MaChannelConverterPtr.Ownsheap { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14919) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4d03fe9229010941"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverterPtr.PChannelMapIn { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14914) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b1a9e3ee51cf6e90"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverterPtr.PChannelMapOut { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14915) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-59accc969483abbc"></a>[`public void* Inno.Native.MiniAudio.MaChannelConverterPtr.Pheap { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14918) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-13be4e87cdcc3433"></a>[`public byte* Inno.Native.MiniAudio.MaChannelConverterPtr.PShuffleTable { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14916) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f7ca173dd59d78e3"></a>[`public ref Inno.Native.MiniAudio.MaChannelConverter.WeightsAnonymous Inno.Native.MiniAudio.MaChannelConverterPtr.Weights { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14917) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba70c126a04e0035"></a>[`public struct Inno.Native.MiniAudio.MaChannelConverterPtr : System.IEquatable<Inno.Native.MiniAudio.MaChannelConverterPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14888) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaChannelMixMode</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaChannelMixMode 6c93e1b8f63ddd80eed2f72473871606bc66d35ee93e193c0f45af0d1540c1ef -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelMixMode.Rectangular ca21d36c1d2fa17bd8f6751c7ba7270bab61c3a9272bff261d4cbe740f85bc9f -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelMixMode.Simple 4343c04af54cdd8f3fe391b579beb8836e1c6c1bc65378e02716589b9b748a71 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelMixMode.CustomWeights 0128711421b9f40b46e1019ef162961d68cefb8ee60a5918e305310cecaa0b9d -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelMixMode.Default 3974944a254257cd9cc8f8ac979ca1c32b4e51d5084cd5650ca53499fb4f17b8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b214ed4d53027cac"></a>[`Inno.Native.MiniAudio.MaChannelMixMode.CustomWeights = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L244) | 当前常量或枚举值：`2`。 | — |
| <a id="api-d5e0873d5f2d7c8d"></a>[`Inno.Native.MiniAudio.MaChannelMixMode.Default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L245) | 当前常量或枚举值：`0`。 | — |
| <a id="api-d69f5ae58f03cf2d"></a>[`Inno.Native.MiniAudio.MaChannelMixMode.Rectangular = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L242) | 当前常量或枚举值：`0`。 | — |
| <a id="api-0d9ba97d89a0c5c4"></a>[`Inno.Native.MiniAudio.MaChannelMixMode.Simple = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L243) | 当前常量或枚举值：`1`。 | — |
| <a id="api-6369da6c6df7b56b"></a>[`public enum Inno.Native.MiniAudio.MaChannelMixMode : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L240) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaChannelPosition</code> · 56 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaChannelPosition 50273b45fe24ecae4c28ba503bf680ab70cde0ee9443bac340bb2d13c60d4a9d -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.None ba0074cf7fac85e705b2b2a8fec9b2506224602391d771e89ebd405dcc22b172 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Mono 30e26550a1b88a45d62fdf91e4927d60df6294175d6e683a2842fd252f6a3e69 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.FrontLeft 3f4e52623ecac7d76f95d0ed480c900b57f4adebe285a6f1b5806fbeedd73825 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.FrontRight 9a69173a2b595a48d1a7814f8d654b0563fa7599f4ff0feeee58347a7cfb3608 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.FrontCenter a77d32817ab896af1622e14a0ad7dbd0fc0c9c3b23417426d94eadb5f589069d -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Lfe 2ba7a3d7864f086d73b127eec5c9954006c680aa0620570561d5cbaaca7a959b -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.BackLeft 043255f2d451c1330c6911f42f08b8c0ddbe22a288e846d8bdcd02e61a9ca564 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.BackRight 6192f06853a8faf228622765afa9499d3ec1381a0831157cf2cc98b0f8f0cc14 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.FrontLeftCenter dd288d225d03658b524eb93777b83455c4a7eee1cad636494a95f0262fa79c80 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.FrontRightCenter 73688d7f7371bce48a5ee019d6952b378688fb1f4ed2f0d3ff3ad1d771ed2aef -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.BackCenter 3c5155f61adbabcfeb4498cd6d992723b48794574a40c41696a049bcf6ffaea5 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.SideLeft 846ad7864300d1b28212ada6fba17488357753041bd8a19aa1bb40efb0b196ae -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.SideRight 0080290ec2f37870ebf43e8cf53142887a6bcad67cac40a12e440b2422c47417 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.TopCenter 500407444e4223a625556a1168a5a7513a73c0bee1bf5eb26b4501c2371c2a61 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.TopFrontLeft 35c1ba7d59b82ea540457ce5a6dbcd2c6e0372a1ada1b2a88bb90ff4c995acdd -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.TopFrontCenter 6c98619538599a77636c9a232a81295892ba2cf8ab61df2dacc59de7889929a0 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.TopFrontRight 29d241bc811540d51696891d349daa991f0b812f09217f5ade719ec4410c0bb0 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.TopBackLeft aa21697c5485b5a23c71f2840a813afb14bd7a9c25d7ca0b3c80a6c1ad370231 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.TopBackCenter 5fc2a022c6f2af3a963cb337dc2ffd7270fe7604463c3148081e01d616e0b6b9 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.TopBackRight a5ad845a5f7a3700635364e9f2b76ae66276b29600a06e8ba96fa13ca8fa26ac -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux0 9def2c65864f6807293720678620ba3d097cbda4334591a2b8535c43a1059d0a -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux1 298486e67ab359c5326b2fb16097b1753edf7da348e407ad5bae9905fadceab5 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux2 e5d8bf55860e4ac977a6cab0393f57cddda4183cf5d6ee7419d492c0ac0f1a65 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux3 5a65540cc0d6fdf0e8d32646b015362cc7973fbc69a0e1e9187d64f0bbf3e799 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux4 c31113672be67241ab850958f0d273539dbcd46690ccca92d1e2dbd7ee247239 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux5 9adbbb7635f6a9451ab6fd078f1ea1df8bfb3573b2d44f32926ae15361ae84a9 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux6 044cd97ba9922a278ec218d3407f1a61cdf79b45c5b8e9fcf60b4723170d6867 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux7 a22959a133551197ee525125dab04e42c65bdb398802ff36e6d0dbb1aea17ea4 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux8 cb07ac01f59f39f584f74fc3569b4530bd9c0440ad64efd4d2c78cbbd843f20a -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux9 e60b14ac8d4b52aaef4b8a9cbaa202d2684e980d4eb1d079f903cbc5b0246007 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux10 828996a3551df48d148eec48f07b74bfae6de7c67f4061da3d78e1b868882fb9 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux11 3c24acc7f7513507912ec7a135e4bebdaead40233c7289d991fece585a6876fe -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux12 44ab75d0b079fdf2dc01a39e11ba693a4c8c072639376757ba57a39ddc2062e5 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux13 c64fb5a8cf147ad1954658a493519ea14dcad50043606cfc66b90d69b2a662f1 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux14 a088add44ecfe599abeb8c1ba7a5289492af2b553ba4130b3bc51246cb399979 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux15 8cb038cf6fb7062b22f20507ec4e32e29270b095b6a167727de043e80f423097 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux16 699dfb0fd9d5576c325d19e92f6d5a7d12df296b65517337d8029c0beeafb990 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux17 5704509b67eb7dde633164dbb6a32422b63a86afb963c4abc3b534a6ac99e811 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux18 5ed84259c90640b9b5c404f53244f5e2e184a737ec4d00d430ce8983fa4dfe74 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux19 c77f4ff57157699a268f2895e55f3ef4229191daaace1f7d64c141357f26faa0 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux20 05f57c73513730243962fd317bda0364dbce7493816a8d7c28a9c5b0dfb405c6 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux21 f0ba554ab28ca3975959f874503f211c0a34e6f011d3b0bfabd3ab9bdd5cc627 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux22 3297219b47557ddeca5037d85365434696d3559859b16b6665462a259215500b -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux23 c41b85994abd2a0cd1c619eca07995c733b7842ded9f89479d3b6cdb2ccc8e21 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux24 7eb42d75c49a7450bbb5e170720bfbb6129824d919158629afda9d4d02e20156 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux25 665cd468893d7c38febf4c6d9a5a1a7180b1483fb4fb46d4d1a49bc275d7042b -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux26 b728b50597c846335c3e5941ac6da1975cd25c884489b32c48ab461b39d126c1 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux27 a43974d4b450bba19366c9c621fc454a4af8ff1b421f86d10aed6374612faeb5 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux28 7dd995ba618f0efa87e240cd9abc2a16a04aab204498fe64cadceee50595da1a -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux29 aeac994bf08e858811cff19d609d1b2e4bd4967c8fe019dbffd7650b06132807 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux30 c1d5e19c2269d8034b54fa0e8d6f6bae9e72a8ab8bd4b92cc43eb3228d2d439a -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Aux31 1528e8b9add6620f7c12f1cef803eca4c9e822a919a4d8b584b2e7319a1f8d56 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Count d36360f4fa5618174f1d5405e3dc3bfc5f15541fca2226beb13ad1b30011ba2b -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Left 27fb4b81bbe64175b5695bb19c184d56fa5f596a266d4997cb48ead0b9b345c3 -->
<!-- api: F:Inno.Native.MiniAudio.MaChannelPosition.Right 23ffde560e1c4335b97ba1ebf0701e0e8b345bdce1ac1d164b455aaa19a34ed4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9280e8c387c41e66"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux0 = 20`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L438) | 当前常量或枚举值：`20`。 | — |
| <a id="api-245c6654bed9631d"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux1 = 21`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L439) | 当前常量或枚举值：`21`。 | — |
| <a id="api-089f4c0397114e2d"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux10 = 30`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L448) | 当前常量或枚举值：`30`。 | — |
| <a id="api-5535d2c2dd3ab7bb"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux11 = 31`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L449) | 当前常量或枚举值：`31`。 | — |
| <a id="api-a73097b7c30ca498"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux12 = 32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L450) | 当前常量或枚举值：`32`。 | — |
| <a id="api-7afc14b9de61058a"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux13 = 33`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L451) | 当前常量或枚举值：`33`。 | — |
| <a id="api-f94c44b65bcb7e1a"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux14 = 34`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L452) | 当前常量或枚举值：`34`。 | — |
| <a id="api-4aff2097de0f3db0"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux15 = 35`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L453) | 当前常量或枚举值：`35`。 | — |
| <a id="api-a17c3b876e45aeda"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux16 = 36`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L454) | 当前常量或枚举值：`36`。 | — |
| <a id="api-68192006dfa629f9"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux17 = 37`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L455) | 当前常量或枚举值：`37`。 | — |
| <a id="api-6ddb8d48546ad13f"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux18 = 38`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L456) | 当前常量或枚举值：`38`。 | — |
| <a id="api-2b82fa819aff3a70"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux19 = 39`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L457) | 当前常量或枚举值：`39`。 | — |
| <a id="api-b524a119557342c7"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux2 = 22`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L440) | 当前常量或枚举值：`22`。 | — |
| <a id="api-e1f5be97542600ca"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux20 = 40`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L458) | 当前常量或枚举值：`40`。 | — |
| <a id="api-e5984179897f4955"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux21 = 41`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L459) | 当前常量或枚举值：`41`。 | — |
| <a id="api-12fa7b6465ba6ca7"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux22 = 42`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L460) | 当前常量或枚举值：`42`。 | — |
| <a id="api-4dcde683a4305242"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux23 = 43`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L461) | 当前常量或枚举值：`43`。 | — |
| <a id="api-096c9a456994df10"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux24 = 44`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L462) | 当前常量或枚举值：`44`。 | — |
| <a id="api-e5a49cd94f0283cb"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux25 = 45`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L463) | 当前常量或枚举值：`45`。 | — |
| <a id="api-e683aba183df673d"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux26 = 46`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L464) | 当前常量或枚举值：`46`。 | — |
| <a id="api-1cc70a011e77d86c"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux27 = 47`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L465) | 当前常量或枚举值：`47`。 | — |
| <a id="api-e9315be1c2d9a209"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux28 = 48`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L466) | 当前常量或枚举值：`48`。 | — |
| <a id="api-85fe9c1c565df5ae"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux29 = 49`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L467) | 当前常量或枚举值：`49`。 | — |
| <a id="api-80ae1878547c81c1"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux3 = 23`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L441) | 当前常量或枚举值：`23`。 | — |
| <a id="api-d7df9db3ecfafb17"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux30 = 50`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L468) | 当前常量或枚举值：`50`。 | — |
| <a id="api-4c4bafacae7cb0af"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux31 = 51`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L469) | 当前常量或枚举值：`51`。 | — |
| <a id="api-c69ca8fec2e3a2f8"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux4 = 24`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L442) | 当前常量或枚举值：`24`。 | — |
| <a id="api-7d4cfcdfe796dc0e"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux5 = 25`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L443) | 当前常量或枚举值：`25`。 | — |
| <a id="api-1e8539ee3b437f1d"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux6 = 26`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L444) | 当前常量或枚举值：`26`。 | — |
| <a id="api-0af410da1ff5d853"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux7 = 27`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L445) | 当前常量或枚举值：`27`。 | — |
| <a id="api-08ba945e1437c552"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux8 = 28`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L446) | 当前常量或枚举值：`28`。 | — |
| <a id="api-d2d0f54108957935"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Aux9 = 29`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L447) | 当前常量或枚举值：`29`。 | — |
| <a id="api-8854807809d6038c"></a>[`Inno.Native.MiniAudio.MaChannelPosition.BackCenter = 10`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L428) | 当前常量或枚举值：`10`。 | — |
| <a id="api-015fcb2ddf15efdd"></a>[`Inno.Native.MiniAudio.MaChannelPosition.BackLeft = 6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L424) | 当前常量或枚举值：`6`。 | — |
| <a id="api-49e48a262387e72f"></a>[`Inno.Native.MiniAudio.MaChannelPosition.BackRight = 7`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L425) | 当前常量或枚举值：`7`。 | — |
| <a id="api-5a462599db5edb40"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Count = 52`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L470) | 当前常量或枚举值：`52`。 | — |
| <a id="api-20242ae956a3d868"></a>[`Inno.Native.MiniAudio.MaChannelPosition.FrontCenter = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L422) | 当前常量或枚举值：`4`。 | — |
| <a id="api-a0aa0364bd17c2df"></a>[`Inno.Native.MiniAudio.MaChannelPosition.FrontLeft = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L420) | 当前常量或枚举值：`2`。 | — |
| <a id="api-bfcb881a415e3fe9"></a>[`Inno.Native.MiniAudio.MaChannelPosition.FrontLeftCenter = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L426) | 当前常量或枚举值：`8`。 | — |
| <a id="api-2b3df2a608eaaae2"></a>[`Inno.Native.MiniAudio.MaChannelPosition.FrontRight = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L421) | 当前常量或枚举值：`3`。 | — |
| <a id="api-d5d547894d9a6602"></a>[`Inno.Native.MiniAudio.MaChannelPosition.FrontRightCenter = 9`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L427) | 当前常量或枚举值：`9`。 | — |
| <a id="api-d1a6160d21fce7dd"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Left = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L471) | 当前常量或枚举值：`2`。 | — |
| <a id="api-c9b08e0458fb34e2"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Lfe = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L423) | 当前常量或枚举值：`5`。 | — |
| <a id="api-16cb1914fb06d189"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Mono = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L419) | 当前常量或枚举值：`1`。 | — |
| <a id="api-75a603a0d9774d65"></a>[`Inno.Native.MiniAudio.MaChannelPosition.None = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L418) | 当前常量或枚举值：`0`。 | — |
| <a id="api-2b5863e56249dde0"></a>[`Inno.Native.MiniAudio.MaChannelPosition.Right = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L472) | 当前常量或枚举值：`3`。 | — |
| <a id="api-01907e84b273b4c3"></a>[`Inno.Native.MiniAudio.MaChannelPosition.SideLeft = 11`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L429) | 当前常量或枚举值：`11`。 | — |
| <a id="api-e145a10bfe67cf93"></a>[`Inno.Native.MiniAudio.MaChannelPosition.SideRight = 12`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L430) | 当前常量或枚举值：`12`。 | — |
| <a id="api-f0241b7219714e88"></a>[`Inno.Native.MiniAudio.MaChannelPosition.TopBackCenter = 18`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L436) | 当前常量或枚举值：`18`。 | — |
| <a id="api-d7e6b078d026805f"></a>[`Inno.Native.MiniAudio.MaChannelPosition.TopBackLeft = 17`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L435) | 当前常量或枚举值：`17`。 | — |
| <a id="api-bb431426cf09e7ac"></a>[`Inno.Native.MiniAudio.MaChannelPosition.TopBackRight = 19`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L437) | 当前常量或枚举值：`19`。 | — |
| <a id="api-b242e3dad1c62a02"></a>[`Inno.Native.MiniAudio.MaChannelPosition.TopCenter = 13`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L431) | 当前常量或枚举值：`13`。 | — |
| <a id="api-a1962f84b7a60f47"></a>[`Inno.Native.MiniAudio.MaChannelPosition.TopFrontCenter = 15`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L433) | 当前常量或枚举值：`15`。 | — |
| <a id="api-2b2779f1d494727d"></a>[`Inno.Native.MiniAudio.MaChannelPosition.TopFrontLeft = 14`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L432) | 当前常量或枚举值：`14`。 | — |
| <a id="api-0d705c274ea50c4d"></a>[`Inno.Native.MiniAudio.MaChannelPosition.TopFrontRight = 16`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L434) | 当前常量或枚举值：`16`。 | — |
| <a id="api-0e2707f35e407653"></a>[`public enum Inno.Native.MiniAudio.MaChannelPosition : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L416) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContext</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContext 74b7682deb2d7126367235828d4e4dc416c4d2d1a5054367ef268362019bf5b9 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.Callbacks 28b509fcaab92616a9d6077466007adf3c0154e0288676bfe5e37d930202aeb3 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.Backend 18b540034cdd2268a54aff1288ad7152ae06f464f05c005f33a11f557fcb7fa5 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.PLog 3875f128f5f53d12ed6c2b6b402a802bcc293646825944d772c6c9be13d50139 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.Log 6eae8db7fee112396418f07b8106308ca1a8915531b3d9818ed3be16908c049d -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.ThreadPriority be41aae1c7bd271fef8cbc14d3ac1fca57294be62546f252a8b58fd165a974c0 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.ThreadStackSize 23ae9234af5f8345a62d1b34d591e61d942d9d01e47a78a43a4b3c9a7843292f -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.PUserData 3bf7a5ed163d8e02efd477d7264651f969f3b6844392500c05e827211f6ceca4 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.AllocationCallbacks 0c285d309480063849621f998273bfdc375a595e2ba23c166a7b3750d0ee2296 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.DeviceEnumLock 3bfde45f96cee5b33a9d0ec27a5463d6e1d491636562066a74268b1d19da823b -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.DeviceInfoLock a02f5942bfcda625e228253b5a7622b41c628aaa97d24255dcd863093439b9ae -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.DeviceInfoCapacity 3e2586af0852863b9ecaeb732e4dc6771bf7fd6dfb281664a7a8db97dea47330 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.PlaybackDeviceInfoCount 63b2c34d046d82ce2d32ab6b46234a15dc9b70e4c221c979c2668c16ad3acb70 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.CaptureDeviceInfoCount 4f8adcdc3fd692159788bf17b6edcd611441424aecc9341623790a3d729fc666 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.PDeviceInfos 406b269548a05bda49b52a1c71e68c9d6fcae28969a48d73dd59b419619e90e8 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.AnonymousField14 be632aa9b866691a3cdd07ade234328251f04682158e1d34c1caef373a610ae6 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.AnonymousField15 41376d198ea76a63c43ae176898e73c9eb6003e6c47ba9aad3aa22a161a1990e -->
<!-- api: M:Inno.Native.MiniAudio.MaContext.#ctor(Inno.Native.MiniAudio.MaBackendCallbacks,Inno.Native.MiniAudio.MaBackend,Inno.Native.MiniAudio.MaLog*,Inno.Native.MiniAudio.MaLog,Inno.Native.MiniAudio.MaThreadPriority,System.UIntPtr,System.Void*,Inno.Native.MiniAudio.MaAllocationCallbacks,System.IntPtr,System.IntPtr,System.UInt32,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaDeviceInfo*,Inno.Native.MiniAudio.MaContext.MaContextAnonymous14,Inno.Native.MiniAudio.MaContext.MaContextAnonymous15) 559a0cdbba15a9dbe1c19037349f8a59d16513e4cfb91b758f29e04e1c108269 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-153d7ae4a2fe9b53"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaContext.AllocationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10392) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0ef189974e26a81f"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous14 Inno.Native.MiniAudio.MaContext.AnonymousField14`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10399) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1aa7e3abc776daf"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous15 Inno.Native.MiniAudio.MaContext.AnonymousField15`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10400) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a34e1c7f249fe3d0"></a>[`public Inno.Native.MiniAudio.MaBackend Inno.Native.MiniAudio.MaContext.Backend`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10386) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8add20e77b255fcc"></a>[`public Inno.Native.MiniAudio.MaBackendCallbacks Inno.Native.MiniAudio.MaContext.Callbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10385) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5ebe31d572abc28c"></a>[`public uint Inno.Native.MiniAudio.MaContext.CaptureDeviceInfoCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10397) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2cadc9f9ca4f2efc"></a>[`public nint Inno.Native.MiniAudio.MaContext.DeviceEnumLock`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10393) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cdd827aa112763ce"></a>[`public uint Inno.Native.MiniAudio.MaContext.DeviceInfoCapacity`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10395) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-422b8b209a649872"></a>[`public nint Inno.Native.MiniAudio.MaContext.DeviceInfoLock`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10394) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5624a544f17921be"></a>[`public Inno.Native.MiniAudio.MaLog Inno.Native.MiniAudio.MaContext.Log`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10388) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a86f8f5a69f67a63"></a>[`public Inno.Native.MiniAudio.MaDeviceInfo* Inno.Native.MiniAudio.MaContext.PDeviceInfos`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10398) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e52180873f637195"></a>[`public uint Inno.Native.MiniAudio.MaContext.PlaybackDeviceInfoCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10396) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d5d303d279b0be10"></a>[`public Inno.Native.MiniAudio.MaLog* Inno.Native.MiniAudio.MaContext.PLog`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10387) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-44f4a4e060cfb88c"></a>[`public void* Inno.Native.MiniAudio.MaContext.PUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10391) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e818be270ae0a1c6"></a>[`public Inno.Native.MiniAudio.MaThreadPriority Inno.Native.MiniAudio.MaContext.ThreadPriority`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10389) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-233e9ce8167b1368"></a>[`public nuint Inno.Native.MiniAudio.MaContext.ThreadStackSize`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10390) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-afce64ce7eb7a9ad"></a>[`public Inno.Native.MiniAudio.MaContext.MaContext(Inno.Native.MiniAudio.MaBackendCallbacks callbacks = default(Inno.Native.MiniAudio.MaBackendCallbacks), Inno.Native.MiniAudio.MaBackend backend = Inno.Native.MiniAudio.MaBackend.Wasapi, Inno.Native.MiniAudio.MaLog* pLog = null, Inno.Native.MiniAudio.MaLog log = default(Inno.Native.MiniAudio.MaLog), Inno.Native.MiniAudio.MaThreadPriority threadPriority = Inno.Native.MiniAudio.MaThreadPriority.Default, nuint threadStackSize = 0, void* pUserData = null, Inno.Native.MiniAudio.MaAllocationCallbacks allocationCallbacks = default(Inno.Native.MiniAudio.MaAllocationCallbacks), nint deviceEnumLock = 0, nint deviceInfoLock = 0, uint deviceInfoCapacity = 0, uint playbackDeviceInfoCount = 0, uint captureDeviceInfoCount = 0, Inno.Native.MiniAudio.MaDeviceInfo* pDeviceInfos = null, Inno.Native.MiniAudio.MaContext.MaContextAnonymous14 value14 = default(Inno.Native.MiniAudio.MaContext.MaContextAnonymous14), Inno.Native.MiniAudio.MaContext.MaContextAnonymous15 value15 = default(Inno.Native.MiniAudio.MaContext.MaContextAnonymous15))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10401) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-75b55bbd2fa0d353"></a>[`public struct Inno.Native.MiniAudio.MaContext`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10133) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContext.MaContextAnonymous14</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14 0423317de9fa90d4a58d2b47b87aaca29d5b87925820a2b2968116312c2af50d -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.Wasapi 33a70a8e6e5dab899dbe2e56a287d21918af53a3ea669d9745933764153d7c9f -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.Dsound ffb0ddc040d47667e29193bb85e8d1b997d253d552538a486be421f971838dbf -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.Winmm f94c537abd16ec0adc8d546ada6847e4e4f693f03a7ac25c936d23c3c1b78b97 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.Jack 0ae9d78204220557f3d88d26a4e665ed33b796af1fa349988695980ab9f407d1 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.NullBackend 7c471973b983fc4b3fcf438ee6e39806af4c5e6d7d4dab8430e3e0378ada79ba -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f9126db11897eeee"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.Dsound`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10321) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e19e93b8fedf191a"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.Jack`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10325) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9fb3177c33936dfc"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.NullBackendAnonymous Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.NullBackend`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10327) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7a1ebac99c9673ff"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.Wasapi`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10319) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd7feaf69480e936"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.Winmm`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10323) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ef863eda6e0e15c3"></a>[`public struct Inno.Native.MiniAudio.MaContext.MaContextAnonymous14`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10139) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous bf9a7a6e862dec80d274d0207658596338600de0c64454a14db4da0b8f720718 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.HWnd a795e0bdd2530c342fd56de92d7099e94406ef59ea629bb80e4615d9973b2252 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.HDSoundDll 7deccbddb583cf273d93d9f90691c2c7842d70323815df63d6a9e01975ca6db5 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.DirectSoundCreate 70b1712ba5d1157144b3f00da3c8827e01d59326a13e3b3c378e71c2552e2336 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.DirectSoundEnumerateA 853a615e6d02d7392c5ca16d821d54c01d6c8acbff3768c34371ac00e007b288 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.DirectSoundCaptureCreate 55dc095e3f6885d054a0f29a857ad45628d3019a89db8121874ce0012bfeb94b -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.DirectSoundCaptureEnumerateA 1b52964d04ab61790122df4965c4468636762378b9abd1393826c3bc053bc989 -->
<!-- api: M:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.#ctor(System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr) 33dcfa2f86b542748bff5e4ee772f75949d597ee24f42405853aa48d774e3141 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3321d528b07bba64"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.DirectSoundCaptureCreate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10196) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a33ee4c7ddffc45e"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.DirectSoundCaptureEnumerateA`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10197) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-316024509f7c31c5"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.DirectSoundCreate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10194) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f9556b96ee67ad3"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.DirectSoundEnumerateA`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10195) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5cf73bd499aa488f"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.HDSoundDll`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10193) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e53fbdb4a8a7cc97"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.HWnd`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10192) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-880db382c7b15f44"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous.DsoundAnonymous(nint hWnd = 0, nint hDSoundDLL = 0, nint directSoundCreate = 0, nint directSoundEnumerateA = 0, nint directSoundCaptureCreate = 0, nint directSoundCaptureEnumerateA = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10198) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7841a4b6e5fb203f"></a>[`public struct Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.DsoundAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10190) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous</code> · 21 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous bda77467997195052835a3b44a2a907dc1c6e11b9ef53fa4af0516ab66932892 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackSo 653f5ecc5ac3c08ed0fe57c25837ab70e4c1f1b65495777e30f5f7db564ea1c7 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackClientOpen 9369de622f2387897902c9dfa6924e447d09cfac85431d9b4f713e0d559914d1 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackClientClose c9e7e23beb7f93ca7f646309bc5e98c5ebc5e838abecf7932041959c32e8a47f -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackClientNameSize 15fc77c2be0998a09d9dbd7b902a4d620624578a933de534519276cb9bf929e7 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackSetProcessCallback 663547374e9620918dccff4503c7c7cf705580601df6a51b85765a65d22beb61 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackSetBufferSizeCallback 01c66532ab7fc944691740a78afdc1b54731c4e00cd25a94ea1d0c79f03e6262 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackOnShutdown b2e9d7677fbd16e9b73317ff8daff237405efff712cc6cb5add5fecde2e260ff -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackGetSampleRate 361c13782bb9663de92a7aa9c358cc5a12023c7f568d090c2d84f0e62060b51a -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackGetBufferSize 15e56ec7b9b7a5e5e7529ae5e3165b0ae08ec9546145a0c9afaa142bbcb44e11 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackGetPorts 01485c1155431dddc622ad2fb136bb1be81d0e916332ba76a0f1f14ca3a0b3b6 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackActivate 8c4257982ee28baaabf2643503ea32cc6ffd41ecc3d0412c691bf618f30bf935 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackDeactivate 34edf7a0b90af3d155a8722712dcfa6a7e2caa784b311df48ba12b99bca1d83d -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackConnect 72c1b61af9c3ed84e04db42b3e9d0fb8b88d6237ff35ede28b69ed5e210fa4b1 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackPortRegister ca54a1756637639e5530ab725d204d90b0ee4bca321ca3be3ec039109928d85b -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackPortName e6f1ab6413d3ec02bcdba62cffda09cfe9ba375c94976b6433be67e0368de0b7 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackPortGetBuffer beaeff35f67fc0fa7ea0d7b86dc01c8bf851c22342d96707b9fc21e5e86385e4 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackFree f34cd102fb9aa7f4e913bdf4090545b2f1a91c99d3b003013851961dafbd5a3f -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.PClientName 6079fc26bf734fdc3f40840e52c51fac517e90004c8edb0239fa591c64ef2d2d -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.TryStartServer e9a78620da20b4b11fff2ef27f693d17bc2dd4e7fe14734bf5cf018b38ad6472 -->
<!-- api: M:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.#ctor(System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.Byte*,System.UInt32) ec00b7e6f0c9b90de33c7ab9d0c5a6a2e47f0fefbd1d8c6a1824cafd041f73c1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8cdabf2af212a8b3"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackActivate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10272) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-209a582b438a9c46"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackClientClose`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10264) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-264928eed2f98592"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackClientNameSize`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10265) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec15e63a34911032"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackClientOpen`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10263) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-77357d2fdd00f2aa"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackConnect`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10274) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d8eb208de955e01a"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackDeactivate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10273) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c3f656258ecd2040"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackFree`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10278) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd147301983d29dc"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackGetBufferSize`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10270) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b060d53f227c2738"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackGetPorts`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10271) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ca70f019b20ecfab"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackGetSampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10269) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e566589f561a3316"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackOnShutdown`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10268) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-468175646dd1ac11"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackPortGetBuffer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10277) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e5f055f05de923e"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackPortName`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10276) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c2aca236d7cc8e81"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackPortRegister`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10275) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a848492a2bf09547"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackSetBufferSizeCallback`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10267) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-16d19942dbcd48b6"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackSetProcessCallback`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10266) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b1814bcdc49a2f7d"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackSo`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10262) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-29480bd20033d0d5"></a>[`public byte* Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.PClientName`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10279) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b98d60e19b273f0"></a>[`public uint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.TryStartServer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10280) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d04083c0805cbd8"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous.JackAnonymous(nint jackSO = 0, nint jack_client_open = 0, nint jack_client_close = 0, nint jack_client_name_size = 0, nint jack_set_process_callback = 0, nint jack_set_buffer_size_callback = 0, nint jack_on_shutdown = 0, nint jack_get_sample_rate = 0, nint jack_get_buffer_size = 0, nint jack_get_ports = 0, nint jack_activate = 0, nint jack_deactivate = 0, nint jack_connect = 0, nint jack_port_register = 0, nint jack_port_name = 0, nint jack_port_get_buffer = 0, nint jack_free = 0, byte* pClientName = null, uint tryStartServer = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10281) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bba0b8f72001c0fb"></a>[`public struct Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.JackAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10260) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.NullBackendAnonymous</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.NullBackendAnonymous 592b63123971e965f888d886704fe3bc31455d4b2868f6d502079604e1ffafa6 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.NullBackendAnonymous.Unused 4871bc3f932c91e86b36308c8bda771ff814cf2cf672afc1e331a684dff7b64b -->
<!-- api: M:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.NullBackendAnonymous.#ctor(System.Int32) 4c3bcad3aaf7f87235f14c54639a9eacdf6f753b31712c1b7a0136d82f404335 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2b6448c456ec6baa"></a>[`public int Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.NullBackendAnonymous.Unused`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10311) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f7957bdda8436d78"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.NullBackendAnonymous.NullBackendAnonymous(int unused = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10312) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b9228aeaa9af1846"></a>[`public struct Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.NullBackendAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10309) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous 2d1fb3e874e859fb80e1ec940d7bd4bd49f38cceb31eda0f8fd229162b75c69f -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.CommandThread 09ad84e4eadc759878e8e6952c8128842a3d9a651a23b9fa3d3f1436b1240e06 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.CommandLock 3e90494a05793cbb24f2f7d2f925949560124383f3fd171e3bd8e2cd570bff0e -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.CommandSem 279244159be393175fc7d1f97e58575cdd0ff8fcba36a39474785f6992e7f1f2 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.CommandIndex 347fe3f3c1b39718f077a52b95a78e3779889181ddb9aa1e7a234184392faf29 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.CommandCount dee38ebf61d22d6e72c1c61c7f034be82c36947ef7e77ea693f6168cafcc0384 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.Commands_0 7eb5b438de3944d73cfa06bb971fb9af971a9baa9b4109688bd9182ee78f94e8 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.Commands_1 5f62d621b0f7a78213f927d9cfed0877162239346ea02d6e5328ba2628afa373 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.Commands_2 80ace0b6dae7c23324dc5d0c483eda77dc91d0d8d9fee79e0f171c7c823195b9 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.Commands_3 a80407ab0c9382479f95831b717b7349c5ef1f031d17b9da6fb2ff64ebc4324a -->
<!-- api: P:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.Commands 05d5ec09f050611d633eaf7d24712407a830baa2ead357a9aa84bd58346a81f2 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.HAvrt 6e187089b9df7d7d4dfc54bb16d08744d9dbb809372caad2f92aac74d185907e -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.AvSetMmThreadCharacteristicsA 038f7ee77cfd4c12814cd6fec6debf4b12c9b13c832a482aac1e6fcefaab1e99 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.AvRevertMmThreadcharacteristics 352397bc7a019617d4e5b0e4437b0346fbe29a56176f866fa4cc4720f13dd4b0 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.HMmDevapi 5f345749c5a438f2023d7871c57ab6a532252eba6f375b9fd008580c2c363590 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.ActivateAudioInterfaceAsync b9b2528266f78db1c7d1303949b2c1ff2d323f1567279593a7f25282ea254dc9 -->
<!-- api: M:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.#ctor(System.IntPtr,System.IntPtr,System.IntPtr,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaContextCommandWasapi*,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr) 560dc01a4ab7ab4b6921430d9490fbb5afb1f8d375c7bab52d3925e7968abf3f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a915d213dac23b90"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.ActivateAudioInterfaceAsync`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10162) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c54748e2efc117ce"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.AvRevertMmThreadcharacteristics`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10160) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b2d8239423eb630f"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.AvSetMmThreadCharacteristicsA`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10159) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4340dd7c536d41f4"></a>[`public uint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.CommandCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10151) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b5067a8518d6c18a"></a>[`public uint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.CommandIndex`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10150) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-92edbd83cb551cbe"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.CommandLock`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10148) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccd2ad70e9f36313"></a>[`public Inno.Native.MiniAudio.MaContextCommandWasapi Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.Commands_0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10152) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4362b27396796c10"></a>[`public Inno.Native.MiniAudio.MaContextCommandWasapi Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.Commands_1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10153) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc49ba04a2b4b243"></a>[`public Inno.Native.MiniAudio.MaContextCommandWasapi Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.Commands_2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10154) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f0ab5adf79acb694"></a>[`public Inno.Native.MiniAudio.MaContextCommandWasapi Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.Commands_3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10155) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e9fad436468386ca"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.CommandSem`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10149) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-82743d6c71ad69ca"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.CommandThread`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10147) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b62456b939e48bd"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.HAvrt`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10158) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cab31942dbeba6ea"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.HMmDevapi`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10161) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-28c24d4a60cbae55"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.WasapiAnonymous(nint commandThread = 0, nint commandLock = 0, nint commandSem = 0, uint commandIndex = 0, uint commandCount = 0, Inno.Native.MiniAudio.MaContextCommandWasapi* commands = null, nint hAvrt = 0, nint avSetMmThreadCharacteristicsA = 0, nint avRevertMmThreadcharacteristics = 0, nint hMMDevapi = 0, nint activateAudioInterfaceAsync = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10163) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ab70bbc80ae5ad9c"></a>[`public System.Span<Inno.Native.MiniAudio.MaContextCommandWasapi> Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous.Commands { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10156) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eadac7b447af6dcf"></a>[`public struct Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WasapiAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10145) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous 8e56f3ddfdf84b3445e253e8bf550e919926e5a82bf95da0d747fb0857545a48 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.HWinMm 16ee3a071af6c9fca6d13d8fcbcf5bbdd9ec53cc8834b1c077ee6231f7353601 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutGetNumDevs 8ed7226392e786d059cfb8f7618bc2a74d135aebaf048ff09aebaae145f9131f -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutGetDevCapsA fcb2fbdf0e3715ab9c7c6084bc4fafbb5dfdd1bca4be147acd9d665d27bd3857 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutOpen 40ba3462dcaf457c8121ad7d0fd038f89c333f8cbb8df3559b8984b0a077dc39 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutClose e6edd4d369ce7a722e5ceb2545440f68322d925fc960cc1a4b306fb7c44e1164 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutPrepareHeader 7425de3922df9e0a2e886e0e8eb3ebda15a6cefd331928de99d331f28f7188a2 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutUnprepareHeader e14b487c62f3f6145820e1bd7984a3d0c65d836bcd8fc579642b0950624b2844 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutWrite b5e3330f53cf8f2476924dda08a7957af5c895681b116d0e49aec872f8d541a1 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutReset 183672828b82239e27419635ca3c7619761cff43f86184ab200d37039f65b324 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInGetNumDevs fbb4d75db6f9ce3fe88d2cfe3173036c0360dfc545f90a70f12cb980b2819c27 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInGetDevCapsA 29a160b672eb9edf9e1555b6dd14bad29188faea9ed0c2af170bbc7da10026ec -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInOpen 0fea69bdeb1225e2b61f6946687503380f681685c34881f6021ad70372131570 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInClose dfd1be02a5f1ae762e950cdfd68e880c87839fc90d63d12876f9f6058fb60c93 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInPrepareHeader a480d155c9acccc651563086ce88781e6113f2a1fb775e2ec5081ca5f69e95b1 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInUnprepareHeader c66a7a16ef5151a82a6f4625f4ca00568d014ba9f1b804a9a4f8ab9c5b1414e7 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInAddBuffer b3e0e6aadbe5df9bd37f1b1d6f08f8135c2628dd66a80742ac185fa49561f1a8 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInStart 10b25a34709307931d65d06999416893cb5fd6c9eca596855a4ff72cd3ee11a4 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInReset f48d712c198578aca2b006f34d34d7f528cb0eaed4ddc456ed041e90a5d4c005 -->
<!-- api: M:Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.#ctor(System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr) 6048a9af503563e64393bcbe86add0eaf35a09d63e0e5bd8b94830dee2288562 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0e3a77c30a1079bf"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.HWinMm`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10215) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e3f92b736e54946f"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInAddBuffer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10230) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ff729a4227fcd6bc"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInClose`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10227) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4106173d53e8ce86"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInGetDevCapsA`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10225) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c79632e58accb5c"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInGetNumDevs`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10224) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8aec2744fcd90d41"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInOpen`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10226) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-301cf83ad97600cc"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInPrepareHeader`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10228) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fd9e10e04f1b2b3f"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInReset`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10232) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2668647d1956848e"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInStart`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10231) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cafbac31939918e5"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveInUnprepareHeader`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10229) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1bc6e9c9ef96766f"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutClose`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10219) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de06c99429cec52d"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutGetDevCapsA`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10217) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5567738426524410"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutGetNumDevs`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10216) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e4954b19436ac526"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutOpen`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10218) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a389ac59db92b326"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutPrepareHeader`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10220) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-97544a93e03873a2"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutReset`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10223) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3e7e1471f9f11bff"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutUnprepareHeader`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10221) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1198d81cf772943"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WaveOutWrite`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10222) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e74b46c5724ff79f"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous.WinmmAnonymous(nint hWinMM = 0, nint waveOutGetNumDevs = 0, nint waveOutGetDevCapsA = 0, nint waveOutOpen = 0, nint waveOutClose = 0, nint waveOutPrepareHeader = 0, nint waveOutUnprepareHeader = 0, nint waveOutWrite = 0, nint waveOutReset = 0, nint waveInGetNumDevs = 0, nint waveInGetDevCapsA = 0, nint waveInOpen = 0, nint waveInClose = 0, nint waveInPrepareHeader = 0, nint waveInUnprepareHeader = 0, nint waveInAddBuffer = 0, nint waveInStart = 0, nint waveInReset = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10233) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6689812556eee13b"></a>[`public struct Inno.Native.MiniAudio.MaContext.MaContextAnonymous14.WinmmAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10213) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContext.MaContextAnonymous15</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15 0b1268392b0bbaf05dd51f75eea103ac8f30117eb71e9fe5be01c78be1e707d2 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32 671744b715f886f84778023e216f0c398a3993ea1272d532a43bbecc5f27fa28 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Unused f5b79799b960b009fca8397ed718d988e05118b22f1f15c17bbb42dac4c6eaf5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4744c07a7b8aebe8"></a>[`public int Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Unused`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10382) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a270a6b9e6498c02"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10380) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d98bfd2953867e23"></a>[`public struct Inno.Native.MiniAudio.MaContext.MaContextAnonymous15`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10334) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous 2f96a72ebe6235902348bb5325c6541f68fb32d887178f2839e9970c8f7b570e -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.HOle32Dll 982a8e92c78239116318c242bca16fcb3a951da75610821cf1e4f3336b8ece37 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.CoInitialize 49bf400cb68bf6ba671d612d7aea72bbb15a1ede113808cfd954f69ddfe5b23a -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.CoInitializeEx a163b000b39e1a5bdd1ced4cd2d90a6c2af49d872ec92cb4f7ab5eabcd78bac0 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.CoUninitialize ce64edbcb8952b15b45682a4fd3cc31ed96f1f59d33748ba8c22be1130002e87 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.CoCreateInstance e22ba08afa9e363954d917682bd990db5b03370c426a8d76728bcfb6517bdc5e -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.CoTaskMemFree 12b698e1166a50424b64fa2b219df83bea8589b49a053c09e49a23f1cc0fa4a7 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.PropVariantClear 5a827b219060cd0de6446e5acf62f35391bb342f9135a4a1d9b6c001ffb777dd -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.StringFromGUID2 a342ac77a9f3d1fbac9eb3399fc3d46c071aba454df7914a68616915ca711f6a -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.HUser32Dll 9662ef9eaa585e1a9daa7267dbc26efd6a7357184ce16725aa98f285c39b364f -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.GetForegroundWindow bf5431caa1674fd59010964103c439e691d7185d315de758a2b5ed00bcaf4e56 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.GetDesktopWindow 7d794909bed67a11064d23a03b3eb7c28f2c5f8259876b80f9daee2850cad0b0 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.HAdvapi32Dll b6e0381ee73feb6e0081e85a19aa1f7b77ed4a404c825369bc68548d6397ba58 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.RegOpenKeyExA 66d77f8e8a1d29d21a8d8c4a7bc66f5af0d94435c84b391ba7a901eae573f88b -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.RegCloseKey bab010ce2d9e73746c82d2e9dced0b0a40ebddc7dd6a4e59c5289df9881cbf90 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.RegQueryValueExA 8a4e6a73137a8b00cc8d22a7527a46be2e1b2cedfea0a9d4a5a85a538b520f67 -->
<!-- api: F:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.CoInitializeResult a6a51543d0db31c990615f9157165f77890cdd4542bfcc2bde15ee957b1fd1ec -->
<!-- api: M:Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.#ctor(System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr,System.Int32) f1edd2438fa891294057c2f1e6a15a0ed91776c277099f6b5677a22aab721e72 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d2c91b1af833fd0a"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.CoCreateInstance`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10346) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c8eead12d2ae76ec"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.CoInitialize`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10343) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85720b506122e6f2"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.CoInitializeEx`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10344) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ac087aa86af509a1"></a>[`public int Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.CoInitializeResult`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10357) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58e6d5c3a5de2c2c"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.CoTaskMemFree`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10347) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9cd0f8a3b4a9768"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.CoUninitialize`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10345) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-88a995cbe249e94d"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.GetDesktopWindow`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10352) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfdca224d11be77e"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.GetForegroundWindow`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10351) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de58740e45c722cb"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.HAdvapi32Dll`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10353) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0dd6f887be82fadd"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.HOle32Dll`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10342) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-50dbb43c8a27b773"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.HUser32Dll`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10350) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ce4ceead70d9627"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.PropVariantClear`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10348) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-18442b9f146e7fb2"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.RegCloseKey`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10355) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b0f808fb8001ff58"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.RegOpenKeyExA`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10354) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07f0b96926a5b886"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.RegQueryValueExA`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10356) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0cc3f60e064f69e2"></a>[`public nint Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.StringFromGUID2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10349) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e727af513d74ce8"></a>[`public Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous.Win32Anonymous(nint hOle32DLL = 0, nint coInitialize = 0, nint coInitializeEx = 0, nint coUninitialize = 0, nint coCreateInstance = 0, nint coTaskMemFree = 0, nint propVariantClear = 0, nint stringFromGUID2 = 0, nint hUser32DLL = 0, nint getForegroundWindow = 0, nint getDesktopWindow = 0, nint hAdvapi32DLL = 0, nint regOpenKeyExA = 0, nint regCloseKey = 0, nint regQueryValueExA = 0, int coInitializeResult = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10358) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bf5e1f17ffa6ef19"></a>[`public struct Inno.Native.MiniAudio.MaContext.MaContextAnonymous15.Win32Anonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10340) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContextCommandWasapi</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContextCommandWasapi 193d7dc8f879af4387650fa2d86b83c3acb47f8d10ca014b46d193f0fbbead97 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextCommandWasapi.Code fb1a1ba78ae4354945c3df91d181b6fca00553517291672202c5b76d269cbf86 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextCommandWasapi.PEvent 334f55511655e222939e70c16a4872442ce376e6d9296c94f5775e94ba49b150 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextCommandWasapi.Data cc3f81c979edc206bf335bbb4caae8c5dbe2fc8cf30123206759bd635728d33d -->
<!-- api: M:Inno.Native.MiniAudio.MaContextCommandWasapi.#ctor(System.Int32,System.IntPtr*,Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous) 0ae7e723b0d9ad4c2de74630e89e9fc6fdcd2bc00b8ac3ed0481f881935f361f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6e48697827d01705"></a>[`public int Inno.Native.MiniAudio.MaContextCommandWasapi.Code`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10118) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa9c5f7d398ba665"></a>[`public Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous Inno.Native.MiniAudio.MaContextCommandWasapi.Data`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10120) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a149ab03e7e5e0e3"></a>[`public nint* Inno.Native.MiniAudio.MaContextCommandWasapi.PEvent`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10119) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4664254d885b4833"></a>[`public Inno.Native.MiniAudio.MaContextCommandWasapi.MaContextCommandWasapi(int code = 0, nint* pEvent = null, Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous data = default(Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10121) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a6ee5f575a23baae"></a>[`public struct Inno.Native.MiniAudio.MaContextCommandWasapi`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10055) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous 3e98da3f676152fa05da0d0da53bd297064e15aeefe0dea3fd8b6022a60eebea -->
<!-- api: F:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.Quit de3a1604ac15a8cfe25a77d28b426140c0cdfa0dd2f54918c06e5643827d66f0 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClient d459501f6137da0ada57e9b7508cc766409ad5a7f54b8eee4c7f3247e3d31d33 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.ReleaseAudioClient 583030c36814c451ebf330d7355ab5aa1d47d282583530ba84f6704772b21c54 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-42fd61f7544a132a"></a>[`public Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClient`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10113) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c408e7db8c4005b3"></a>[`public Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.QuitAnonymous Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.Quit`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10111) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c460b8099c36935"></a>[`public Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.ReleaseAudioClientAnonymous Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.ReleaseAudioClient`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10115) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e45b7b3a495dbbc7"></a>[`public struct Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10061) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous 2f3c62164b8f7c972b4bcc509f8422624b7827f8dddfea6162d75cec1ffbd6fa -->
<!-- api: F:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous.DeviceType a4ae18671f0b68887ee3d6ab72310fde5bc2dba2333eb50bbaa2f0bf6c298d71 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous.PAudioClient cf57502a01ae80077686ea6de72ab8533ff3217382209030804fc41d45e9143c -->
<!-- api: F:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous.PpAudioClientService cfe1eb33bc753697905e439b0724d42f56201feba54ed947a38d0ec0c7bfe00c -->
<!-- api: F:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous.PResult 1d817ce9bd2e1db02cd64e588bdb1e38d6be0a5a07d16a1e90bc0f568dcb6999 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous.#ctor(Inno.Native.MiniAudio.MaDeviceType,System.Void*,System.Void**,Inno.Native.MiniAudio.MaResult*) a7ad798f98377630378ad92510babec88ed1f22c55bf3f23ddc68c68cdcba24f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8a83fdcfdacbed88"></a>[`public Inno.Native.MiniAudio.MaDeviceType Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous.DeviceType`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10082) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-28e97c85a4f33ded"></a>[`public void* Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous.PAudioClient`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10083) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9994ee3cd4be3284"></a>[`public void** Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous.PpAudioClientService`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10084) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9186b457ce221cab"></a>[`public Inno.Native.MiniAudio.MaResult* Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous.PResult`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10085) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aedd96793781ba5b"></a>[`public Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous.CreateAudioClientAnonymous(Inno.Native.MiniAudio.MaDeviceType deviceType = (Inno.Native.MiniAudio.MaDeviceType)0, void* pAudioClient = null, void** ppAudioClientService = null, Inno.Native.MiniAudio.MaResult* pResult = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10086) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ecc6fab4ad2cdf7d"></a>[`public struct Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.CreateAudioClientAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10080) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.QuitAnonymous</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.QuitAnonymous 7a0752d837fb016a094951ed5df46366b14cf894c1d43c004a9bb5d505c75027 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.QuitAnonymous.Unused f11a9919d09aa7eae2cc0d7f2383fbc75c838d1a55a80ebb8b4cfa0c8b045334 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.QuitAnonymous.#ctor(System.Int32) 30c03d0d557f7296f84460d5a9fd8c77c57750253417dcb6d860c862df15cc23 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f62cfadc31829175"></a>[`public int Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.QuitAnonymous.Unused`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10069) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9bf5e427a28a3cbc"></a>[`public Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.QuitAnonymous.QuitAnonymous(int unused = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10070) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8721184f43a7f9d1"></a>[`public struct Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.QuitAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10067) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.ReleaseAudioClientAnonymous</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.ReleaseAudioClientAnonymous 3fe6451593d71db285bd97198c471ff3a0b1e1e54b1684bdfe90afbc1cabc5b9 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.ReleaseAudioClientAnonymous.PDevice ec65d4eeecea0cd1043c54ba90267afa860991138f91439dafc7490d0e764596 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.ReleaseAudioClientAnonymous.DeviceType db50a40582ba589f64682970f7459fca7b6fdc4fb49d030f3d5ba577842054fd -->
<!-- api: M:Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.ReleaseAudioClientAnonymous.#ctor(Inno.Native.MiniAudio.MaDevice*,Inno.Native.MiniAudio.MaDeviceType) e17c6dfacb5151b75148b6ae3f0db7f4b14ce524b7b7dab5f477b1d2cbdb500b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5dc775ccfd714451"></a>[`public Inno.Native.MiniAudio.MaDeviceType Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.ReleaseAudioClientAnonymous.DeviceType`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10102) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e0f15b2a1fd4001f"></a>[`public Inno.Native.MiniAudio.MaDevice* Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.ReleaseAudioClientAnonymous.PDevice`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10101) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-59895d2c69d852ee"></a>[`public Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.ReleaseAudioClientAnonymous.ReleaseAudioClientAnonymous(Inno.Native.MiniAudio.MaDevice* pDevice = null, Inno.Native.MiniAudio.MaDeviceType deviceType = (Inno.Native.MiniAudio.MaDeviceType)0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10103) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3e34553e09a20466"></a>[`public struct Inno.Native.MiniAudio.MaContextCommandWasapi.DataAnonymous.ReleaseAudioClientAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10099) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContextConfig</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContextConfig cf0419d1f092d2697a7c4a5b630aaaabc9fdb370f64b2ed0b4cdfeed3219cd97 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.PLog f40f527d8b622d7263668855357180921fc0d4364703b6a119f065dd717de4b5 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.ThreadPriority 34891c65026915798957f268f78221bcd59839d68b706112905ed15cf6d10654 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.ThreadStackSize ee80dd90b74b0ebaf6ceb2f95c0ccbb0d1e2a42fd9409278a852d593c5b4c267 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.PUserData 2372abc6b952f500ec9a43fd3c71872aa24c818029a87d133ba4f7670548b6e5 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.AllocationCallbacks cd999d35e14696f65628e530663d80a25acbfd63ccda7454cdade09d49a52661 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.Dsound cc518d2c202962368c9a1740bf4cb5fdd74fb147fb1da1dd9c7edb217de03115 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.Alsa b72baa011d776a67135498625278ecabdf20e89def81d94c7552880c6d31ae9b -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.Pulse 47d542e7d5c60fe705ba75b228f8130cbdeee9cdc4f108753b304ea83c99892a -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.Coreaudio af05e656d37301ce388f72d68de752742153e319975152e9fdde25dcf2ccd4aa -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.Jack b8eb1aa011eaa71d85b5582537ad2a60e1e9cb4f47d917aaca50b95a8d8c0117 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.Custom 427d5e78740b36179a486d7439fc087e1065b0ae0c4b0cbcb37f7e839cc2904e -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfig.#ctor(Inno.Native.MiniAudio.MaLog*,Inno.Native.MiniAudio.MaThreadPriority,System.UIntPtr,System.Void*,Inno.Native.MiniAudio.MaAllocationCallbacks,Inno.Native.MiniAudio.MaContextConfig.DsoundAnonymous,Inno.Native.MiniAudio.MaContextConfig.AlsaAnonymous,Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous,Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous,Inno.Native.MiniAudio.MaContextConfig.JackAnonymous,Inno.Native.MiniAudio.MaBackendCallbacks) 77e1f2879de19d38296f3268db4f43de7334faa6d3de06880e697a73c0b09b2a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b5bf9860d8f9964d"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaContextConfig.AllocationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L906) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b40e5174d61d6aa2"></a>[`public Inno.Native.MiniAudio.MaContextConfig.AlsaAnonymous Inno.Native.MiniAudio.MaContextConfig.Alsa`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L908) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2950b54dcb554991"></a>[`public Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous Inno.Native.MiniAudio.MaContextConfig.Coreaudio`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L910) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-16b90042aa60d0d5"></a>[`public Inno.Native.MiniAudio.MaBackendCallbacks Inno.Native.MiniAudio.MaContextConfig.Custom`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L912) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-67841793dd037793"></a>[`public Inno.Native.MiniAudio.MaContextConfig.DsoundAnonymous Inno.Native.MiniAudio.MaContextConfig.Dsound`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L907) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eb819b5636b6d806"></a>[`public Inno.Native.MiniAudio.MaContextConfig.JackAnonymous Inno.Native.MiniAudio.MaContextConfig.Jack`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L911) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4c3e576e67e97c44"></a>[`public Inno.Native.MiniAudio.MaLog* Inno.Native.MiniAudio.MaContextConfig.PLog`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L902) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7d1ef842a3c7fc34"></a>[`public Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous Inno.Native.MiniAudio.MaContextConfig.Pulse`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L909) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6bd9d636a81927fc"></a>[`public void* Inno.Native.MiniAudio.MaContextConfig.PUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L905) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9794028f2816f9be"></a>[`public Inno.Native.MiniAudio.MaThreadPriority Inno.Native.MiniAudio.MaContextConfig.ThreadPriority`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L903) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2aaddcd83b3d219"></a>[`public nuint Inno.Native.MiniAudio.MaContextConfig.ThreadStackSize`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L904) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea6fdefdff7d60e5"></a>[`public Inno.Native.MiniAudio.MaContextConfig.MaContextConfig(Inno.Native.MiniAudio.MaLog* pLog = null, Inno.Native.MiniAudio.MaThreadPriority threadPriority = Inno.Native.MiniAudio.MaThreadPriority.Default, nuint threadStackSize = 0, void* pUserData = null, Inno.Native.MiniAudio.MaAllocationCallbacks allocationCallbacks = default(Inno.Native.MiniAudio.MaAllocationCallbacks), Inno.Native.MiniAudio.MaContextConfig.DsoundAnonymous dsound = default(Inno.Native.MiniAudio.MaContextConfig.DsoundAnonymous), Inno.Native.MiniAudio.MaContextConfig.AlsaAnonymous alsa = default(Inno.Native.MiniAudio.MaContextConfig.AlsaAnonymous), Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous pulse = default(Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous), Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous coreaudio = default(Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous), Inno.Native.MiniAudio.MaContextConfig.JackAnonymous jack = default(Inno.Native.MiniAudio.MaContextConfig.JackAnonymous), Inno.Native.MiniAudio.MaBackendCallbacks custom = default(Inno.Native.MiniAudio.MaBackendCallbacks))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L913) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a7d8513cb95e5b6d"></a>[`public struct Inno.Native.MiniAudio.MaContextConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L823) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContextConfig.AlsaAnonymous</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContextConfig.AlsaAnonymous cef009cb9dcc6a4bd57ee1785b8c9ccfd5761b2e92d804c7933e1f9a9dd6b114 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.AlsaAnonymous.UseVerboseDeviceEnumeration fcfca98031d05c45b899a373241efd718d0b7532f3d925ff0ffe8140160b3305 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfig.AlsaAnonymous.#ctor(System.UInt32) 3a7b84416c30bf6e6ec3aaeec5f3dea56cd14656719cfa5d1457c66751bfabfd -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2523f9e941cdbeba"></a>[`public uint Inno.Native.MiniAudio.MaContextConfig.AlsaAnonymous.UseVerboseDeviceEnumeration`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L844) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-04fcffc5d1d7b08b"></a>[`public Inno.Native.MiniAudio.MaContextConfig.AlsaAnonymous.AlsaAnonymous(uint useVerboseDeviceEnumeration = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L845) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-efd1612272082efd"></a>[`public struct Inno.Native.MiniAudio.MaContextConfig.AlsaAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L842) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous c12363b70e4f9f0d725e375d8d337c157dcedd324bbf0cd5f8ac8a5d51360803 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous.SessionCategory 6ce3486ac9ae8ed26b8e5ab17a3c7548b261ad386355c0c6b20790b6f7ff9984 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous.SessionCategoryOptions 0f5ae9c887127ba1bfb969fd76ab14f19a90878b8eca4c4a413eafd930691bf2 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous.NoAudioSessionActivate f7fbe597bd7261f78b73d93ed863d2e99a36d8133c63e357de8e5637386a143e -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous.NoAudioSessionDeactivate f5958864b94fce2341c2a4a26973671807cc0967373426330a516769de67b580 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous.#ctor(Inno.Native.MiniAudio.MaIosSessionCategory,System.UInt32,System.UInt32,System.UInt32) 788669c0e4efec6efe0f44a67e7513c9d281e98f809edefbceccf9b4f14461dd -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b0d6678edce61cf5"></a>[`public uint Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous.NoAudioSessionActivate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L876) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2303df770a6dd369"></a>[`public uint Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous.NoAudioSessionDeactivate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L877) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-37f135aa71897819"></a>[`public Inno.Native.MiniAudio.MaIosSessionCategory Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous.SessionCategory`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L874) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7dac1923ad433a8b"></a>[`public uint Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous.SessionCategoryOptions`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L875) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9cbd3804d5d73fd9"></a>[`public Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous.CoreaudioAnonymous(Inno.Native.MiniAudio.MaIosSessionCategory sessionCategory = Inno.Native.MiniAudio.MaIosSessionCategory.Default, uint sessionCategoryOptions = 0, uint noAudioSessionActivate = 0, uint noAudioSessionDeactivate = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L878) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b84f8de63443a1b2"></a>[`public struct Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L872) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContextConfig.DsoundAnonymous</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContextConfig.DsoundAnonymous 5a9a5a21e8ba9a1ab9ff2c756d969b8a79092a7975bcf908f51a9ba125542de7 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.DsoundAnonymous.HWnd e00a64f78363c530894f96e26fa790931b27b02c2b14b998e23c194030d3d85a -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfig.DsoundAnonymous.#ctor(System.IntPtr) b3d2ba4dff2532fdfb58539cddfc7c9ad1f139692eb2a9e311f81c17baebf1db -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8485ca7a845c7e81"></a>[`public nint Inno.Native.MiniAudio.MaContextConfig.DsoundAnonymous.HWnd`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L831) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1dc6d95b304dad0"></a>[`public Inno.Native.MiniAudio.MaContextConfig.DsoundAnonymous.DsoundAnonymous(nint hWnd = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L832) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-57280a8246dc076f"></a>[`public struct Inno.Native.MiniAudio.MaContextConfig.DsoundAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L829) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContextConfig.JackAnonymous</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContextConfig.JackAnonymous f92e2e12f43ba08731402de9fcf5e86630ebfa4025ac9521f1c31800eb8d75e5 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.JackAnonymous.PClientName d1e77a15cbb4f099c11d622d24c842e679e48225ecd5c56e6f61124120c414f6 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.JackAnonymous.TryStartServer 32e83701501eddc32a19c3a81d2b81bf68024661e81715ac4d22776c49737a89 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfig.JackAnonymous.#ctor(System.Byte*,System.UInt32) 357ebed4fd9b0c3a16aa65c5a18730b9a5c6f1f97ce6340c8942cf9bda47c31b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-bac2f78f5e2d15e9"></a>[`public byte* Inno.Native.MiniAudio.MaContextConfig.JackAnonymous.PClientName`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L893) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6d6e99d980c26f76"></a>[`public uint Inno.Native.MiniAudio.MaContextConfig.JackAnonymous.TryStartServer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L894) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-656d960fb2d73eaa"></a>[`public Inno.Native.MiniAudio.MaContextConfig.JackAnonymous.JackAnonymous(byte* pClientName = null, uint tryStartServer = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L895) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f1e382af1b5e7949"></a>[`public struct Inno.Native.MiniAudio.MaContextConfig.JackAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L891) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous 0a0b8a5d25d82be01994fbed8992c7957f837989256b56fba432a690fbc633e4 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous.PApplicationName e948cbcccf7905536c55cc13780ae646c7383b26ee337ec5152ecf849e349352 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous.PServerName 0aac98d7513385d36590a0b7f68a5572894c8ed0b6e1024fff77e1014ee1a911 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous.TryAutoSpawn 7de307e91affa5ff0a0398a7f8c30e800966f026cba5d9b58a975fde742e1f59 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous.#ctor(System.Byte*,System.Byte*,System.UInt32) c7ab1efaf922180400145c5d9bf0134497507de312bc355a6684dec989253ce4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ad5d18ae16d1df41"></a>[`public byte* Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous.PApplicationName`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L857) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-158c72c1cb0a750d"></a>[`public byte* Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous.PServerName`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L858) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-797c95a2e2370a17"></a>[`public uint Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous.TryAutoSpawn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L859) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cb4e12709bb45761"></a>[`public Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous.PulseAnonymous(byte* pApplicationName = null, byte* pServerName = null, uint tryAutoSpawn = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L860) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ece1ea61eb8bfe14"></a>[`public struct Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L855) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContextConfigPtr</code> · 24 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContextConfigPtr 9b5bc147587ef4321dacfa875669285065d043187e70b59a70113e8dc98f6984 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfigPtr.#ctor(Inno.Native.MiniAudio.MaContextConfig*) 5b14d41efaccd6cfd8ba168cabb9bf2e58cac0c9c101e3f924cd9e685af606e1 -->
<!-- api: F:Inno.Native.MiniAudio.MaContextConfigPtr.Handle 362ef7af1e97d5c62d87e3bd12716ebeaa34fb5256d494f29bf058228c5f92f6 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.IsNull 12befa4af166322d4d720af325da0ab56bebe2fb1b37d87538be3beb96b1e785 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.Null 618cebe74cd508fef1ee6910f2bf56a9159eafd7406bedad10355362692ce34c -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.Item(System.Int32) 596b3a453457d96486d44d8e1fe74928614cf182007047dfdf0d527e6b6fc5c8 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaContextConfig*)~Inno.Native.MiniAudio.MaContextConfigPtr c9c0061d5af72b9029e4bc4037fa60490410a2f62de5c87dd7b66740d652312b -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaContextConfigPtr)~Inno.Native.MiniAudio.MaContextConfig* 17c3c4e0615d48aa980b39f70f729369991f491d60dc896f87f9e49d9aceea80 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfigPtr.op_Equality(Inno.Native.MiniAudio.MaContextConfigPtr,Inno.Native.MiniAudio.MaContextConfigPtr) b30e48c038f2ae6eea71d6daf9971696416f8cd23c6b3d6b1c8ed2faf1f567e2 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaContextConfigPtr,Inno.Native.MiniAudio.MaContextConfigPtr) 22f69849c969f44712991a1f0e05de9a0e81739f77e56ff193c33d209d902ab3 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfigPtr.Equals(Inno.Native.MiniAudio.MaContextConfigPtr) 207db850d716715ddba0a235b6c86e3a47f03ca9db0c37f1bd73c6ea5cc0c5d1 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfigPtr.Equals(System.Object) 06fe454853c7f0f2331153948d45f021285bdd75fb1e2a5e72e05fcf96f1670d -->
<!-- api: M:Inno.Native.MiniAudio.MaContextConfigPtr.GetHashCode df9051e7af8822eb0902e9d9ffac13e90d1f852c9e51ab1623a63740e2216092 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.PLog b07ef1e79d7cbba0048e200cc21bd97ac08c15cfc3648b959718c0916dbab411 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.ThreadPriority 1a7acb80d5c392342799a9ba5573eb4fc5435abcdfb43070273c2c9c54317210 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.ThreadStackSize 890c947007b2e4ffacc7662d4cbde3655f692bce8a714908c6aed3eaa2142cee -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.PUserData e53d6ecb2b3269cfdc0f123ba336799d7a5e876c549b3f5c4ae45f3ae8465b1d -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.AllocationCallbacks 8fe9c6ab78c43930a7a28fc8975a12866b35122fd83a8926dc507db8866542f8 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.Dsound 2c7cd670ddc043a9531285b1c0a32d581dfea3bf5cf63952e10c16f8fa6879ce -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.Alsa a0c76a14b990994f19fea01280ecd88be830f9923e059359ffe32ada13c662b5 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.Pulse ad92ca1d79227f717e11484f51530b57cd38f3fd6d92d3218ce93ace1cb97af4 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.Coreaudio d1d287352a4de48ff2329acf933c0983ff9a7306117713ad882836aa803b60d6 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.Jack 081d1ac0da2315b2b9916138d385e73e2dfc3b135543c50ecdb4e27aa6c7b852 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextConfigPtr.Custom 00ccdfd2b66d8a4d58d4872f9cca12d9cf4f53e7f5bcfec8d255d51e0d6a07b7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4b66b4266ed2977a"></a>[`public Inno.Native.MiniAudio.MaContextConfig* Inno.Native.MiniAudio.MaContextConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14930) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a2ca08cf4e07f427"></a>[`public Inno.Native.MiniAudio.MaContextConfigPtr.MaContextConfigPtr(Inno.Native.MiniAudio.MaContextConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14925) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e3eda17ab67b9e2f"></a>[`public bool Inno.Native.MiniAudio.MaContextConfigPtr.Equals(Inno.Native.MiniAudio.MaContextConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14940) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-97bdebed047fd3bb"></a>[`public override bool Inno.Native.MiniAudio.MaContextConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14941) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-0b5b6a61560341e6"></a>[`public override int Inno.Native.MiniAudio.MaContextConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14942) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-fad524d37bd8e35e"></a>[`public static bool Inno.Native.MiniAudio.MaContextConfigPtr.operator ==(Inno.Native.MiniAudio.MaContextConfigPtr left, Inno.Native.MiniAudio.MaContextConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14938) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4ce6036375a94969"></a>[`public static Inno.Native.MiniAudio.MaContextConfigPtr.implicit operator Inno.Native.MiniAudio.MaContextConfigPtr(Inno.Native.MiniAudio.MaContextConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14936) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a52f69c5596b1855"></a>[`public static Inno.Native.MiniAudio.MaContextConfigPtr.implicit operator Inno.Native.MiniAudio.MaContextConfig*(Inno.Native.MiniAudio.MaContextConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14937) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2fb789e40dc8ccdd"></a>[`public static bool Inno.Native.MiniAudio.MaContextConfigPtr.operator !=(Inno.Native.MiniAudio.MaContextConfigPtr left, Inno.Native.MiniAudio.MaContextConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14939) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-574b0edc9412a103"></a>[`public ref Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaContextConfigPtr.AllocationCallbacks { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14948) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a48b72f0142deb6f"></a>[`public ref Inno.Native.MiniAudio.MaContextConfig.AlsaAnonymous Inno.Native.MiniAudio.MaContextConfigPtr.Alsa { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14950) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d7f4d0eb0c1474ee"></a>[`public ref Inno.Native.MiniAudio.MaContextConfig.CoreaudioAnonymous Inno.Native.MiniAudio.MaContextConfigPtr.Coreaudio { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14952) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4843f87021220c00"></a>[`public ref Inno.Native.MiniAudio.MaBackendCallbacks Inno.Native.MiniAudio.MaContextConfigPtr.Custom { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14954) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-48a4bfb447663270"></a>[`public ref Inno.Native.MiniAudio.MaContextConfig.DsoundAnonymous Inno.Native.MiniAudio.MaContextConfigPtr.Dsound { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14949) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8c223486f2a10bfb"></a>[`public bool Inno.Native.MiniAudio.MaContextConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14931) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-15c30741a9bdd483"></a>[`public Inno.Native.MiniAudio.MaContextConfig Inno.Native.MiniAudio.MaContextConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14934) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-23ec9160c04fe2cd"></a>[`public ref Inno.Native.MiniAudio.MaContextConfig.JackAnonymous Inno.Native.MiniAudio.MaContextConfigPtr.Jack { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14953) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fdf36e462aca533c"></a>[`public static Inno.Native.MiniAudio.MaContextConfigPtr Inno.Native.MiniAudio.MaContextConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14932) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6feb2278285a10c6"></a>[`public ref Inno.Native.MiniAudio.MaLogPtr Inno.Native.MiniAudio.MaContextConfigPtr.PLog { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14944) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8edc624cece23d53"></a>[`public ref Inno.Native.MiniAudio.MaContextConfig.PulseAnonymous Inno.Native.MiniAudio.MaContextConfigPtr.Pulse { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14951) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-da1e7e3706b8aee5"></a>[`public void* Inno.Native.MiniAudio.MaContextConfigPtr.PUserData { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14947) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-99913229f2df8f3e"></a>[`public ref Inno.Native.MiniAudio.MaThreadPriority Inno.Native.MiniAudio.MaContextConfigPtr.ThreadPriority { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14945) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-02220a5adc6b5f94"></a>[`public ref nuint Inno.Native.MiniAudio.MaContextConfigPtr.ThreadStackSize { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14946) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dcdf4fe9bbdbfe20"></a>[`public struct Inno.Native.MiniAudio.MaContextConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaContextConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14923) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaContextPtr</code> · 29 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaContextPtr 15a4a380cf3a5b506d5d4a8e45a7cdc1adc01e6817e22e85685fd13bc40f3f9e -->
<!-- api: M:Inno.Native.MiniAudio.MaContextPtr.#ctor(Inno.Native.MiniAudio.MaContext*) d6442773a029df261777244a5ed013c3eea0cb8c40d845d033c57595eba9976d -->
<!-- api: F:Inno.Native.MiniAudio.MaContextPtr.Handle 7a635ab9607e71dd1b4236662001ac909bdb4364db64a8d6920b39a5cc7b6816 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.IsNull ff37b5b2da9271d12b9ed923d1177d380c6e103045a57a0c030ee8f758b19052 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.Null 2ea87d84d4635bd67744f5a3a66c846275a3b03741838a16e8dccf298d2ff031 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.Item(System.Int32) ca1e2a2bd56873c1a489523ac51515949cd9e53219278fae63b3aa65c3bfdb39 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextPtr.op_Implicit(Inno.Native.MiniAudio.MaContext*)~Inno.Native.MiniAudio.MaContextPtr 0069409b8bc72f30f7d14f9e61ff921f240564806aabe836cb6825cfc652d30c -->
<!-- api: M:Inno.Native.MiniAudio.MaContextPtr.op_Implicit(Inno.Native.MiniAudio.MaContextPtr)~Inno.Native.MiniAudio.MaContext* cbec8aadc3075ed4c92b1ec2dc5bef0cc03a7d3650d9ac481198298c5f1fcd89 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextPtr.op_Equality(Inno.Native.MiniAudio.MaContextPtr,Inno.Native.MiniAudio.MaContextPtr) 6ed6f868d52e404e2012904860edec33ca8b2c99e471b19c43fe07a0262ae1a5 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextPtr.op_Inequality(Inno.Native.MiniAudio.MaContextPtr,Inno.Native.MiniAudio.MaContextPtr) a879203015c8146eb3e967e014c32be313f9e375ec5822990e5ae1461c475ba8 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextPtr.Equals(Inno.Native.MiniAudio.MaContextPtr) f480ff231b9b806dd7d81669dcfcbe4d99f1d6e2b49f4a9d891f7b79b8b480ec -->
<!-- api: M:Inno.Native.MiniAudio.MaContextPtr.Equals(System.Object) 61eece107109c5360bba0354cd6166124849d7aa19aaf5ddcf1b07ddd3e5f483 -->
<!-- api: M:Inno.Native.MiniAudio.MaContextPtr.GetHashCode 1bb45630f53aa7a818b8711526c219837b4d04f82cc17831869d81c4edd03e79 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.Callbacks 8a3f722224b93c9f3849db9d923aa0971b4e021f2f3a12827b943416031cb916 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.Backend f7f65176ca1530af3c87529fd0f62e87abc78c17c611a1db9b3b2a4c4a42f780 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.PLog d1e010f8d6679952b64d84b36ca77f45e755295e9726d24c7e322cf5f4069047 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.Log d1a6e8c03659c8409f3d9e7bd0027f03a3d1d6026ccadde958dc1c8d05c42bd3 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.ThreadPriority 40f31ddc06e78a2985b259367e6e01b3a5f20b4bc2e04b097ea8afe4d0dbfbca -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.ThreadStackSize 364e6e4cab862d6c0281ca59a08b2d69a34b25fc1520846cb13be25926f52791 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.PUserData 09fc09edbaa605d71836ce06877768b1c23611f56d1e029ef2b37e0cbf9368ac -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.AllocationCallbacks cfba0560d6aabeb4367746ef48526bcbb68fe7bb3c1ade17d21eb1ef1f647aaa -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.DeviceEnumLock 1aa94852469156ec7968937ae66ed2d8949e43705d8ee4a38bd5754e38780af0 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.DeviceInfoLock dbd406092771ff29f5bb59c3e9d5a50d4ba7512ba23eae22e07cd4696bd77138 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.DeviceInfoCapacity a9e5e4d62d0c5f5ba81f5478c45a1d0a16e89183f7a8b9d09acec323dfdbe66a -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.PlaybackDeviceInfoCount e8930179385a22303e8301ed54200d96ee361e0c0b411386327aa21c536616ff -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.CaptureDeviceInfoCount 97027417c96728285072fa2269d3287ad7d78b52d5965c3cbe827b4369ca7272 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.PDeviceInfos 5d57cf30f7a6f397ef6280b3cddcb4b51d17ef3570c76a360966254917b95c30 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.AnonymousField14 d73fb25b081088946cc1e82040a769775da177b4360d16ece3b9d3cca1a2fb42 -->
<!-- api: P:Inno.Native.MiniAudio.MaContextPtr.AnonymousField15 7535a17074863c761b82a04fa0acbdbc314660f076889a2c824e11c263e2abc1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1ce84a6e71cdafa5"></a>[`public Inno.Native.MiniAudio.MaContext* Inno.Native.MiniAudio.MaContextPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14965) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-008aff0df51ba980"></a>[`public Inno.Native.MiniAudio.MaContextPtr.MaContextPtr(Inno.Native.MiniAudio.MaContext* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14960) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2d59776c089524c8"></a>[`public bool Inno.Native.MiniAudio.MaContextPtr.Equals(Inno.Native.MiniAudio.MaContextPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14975) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-b4820cf40fb9131a"></a>[`public override bool Inno.Native.MiniAudio.MaContextPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14976) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-936ab32ec4c8980f"></a>[`public override int Inno.Native.MiniAudio.MaContextPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14977) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-474caf6676f956c9"></a>[`public static bool Inno.Native.MiniAudio.MaContextPtr.operator ==(Inno.Native.MiniAudio.MaContextPtr left, Inno.Native.MiniAudio.MaContextPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14973) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3a459224ab78fa5e"></a>[`public static Inno.Native.MiniAudio.MaContextPtr.implicit operator Inno.Native.MiniAudio.MaContextPtr(Inno.Native.MiniAudio.MaContext* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14971) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-adbd4479ebf3b488"></a>[`public static Inno.Native.MiniAudio.MaContextPtr.implicit operator Inno.Native.MiniAudio.MaContext*(Inno.Native.MiniAudio.MaContextPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14972) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-964aaa8c7b7582c6"></a>[`public static bool Inno.Native.MiniAudio.MaContextPtr.operator !=(Inno.Native.MiniAudio.MaContextPtr left, Inno.Native.MiniAudio.MaContextPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14974) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2348dc3c6cfd99c3"></a>[`public ref Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaContextPtr.AllocationCallbacks { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14986) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fe7ad9867ac75f37"></a>[`public ref Inno.Native.MiniAudio.MaContext.MaContextAnonymous14 Inno.Native.MiniAudio.MaContextPtr.AnonymousField14 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14993) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0c34ab70807861ad"></a>[`public ref Inno.Native.MiniAudio.MaContext.MaContextAnonymous15 Inno.Native.MiniAudio.MaContextPtr.AnonymousField15 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14994) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2b9f5dd994d0f852"></a>[`public ref Inno.Native.MiniAudio.MaBackend Inno.Native.MiniAudio.MaContextPtr.Backend { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14980) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2a8d54d3fe213392"></a>[`public ref Inno.Native.MiniAudio.MaBackendCallbacks Inno.Native.MiniAudio.MaContextPtr.Callbacks { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14979) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a1e840f79fb7631a"></a>[`public ref uint Inno.Native.MiniAudio.MaContextPtr.CaptureDeviceInfoCount { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14991) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-791d237bde74892f"></a>[`public ref nint Inno.Native.MiniAudio.MaContextPtr.DeviceEnumLock { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14987) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-444e3b02363723eb"></a>[`public ref uint Inno.Native.MiniAudio.MaContextPtr.DeviceInfoCapacity { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14989) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0e9662a0a7b39fed"></a>[`public ref nint Inno.Native.MiniAudio.MaContextPtr.DeviceInfoLock { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14988) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dbe4446ad5e83c33"></a>[`public bool Inno.Native.MiniAudio.MaContextPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14966) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-19b71081b07663f6"></a>[`public Inno.Native.MiniAudio.MaContext Inno.Native.MiniAudio.MaContextPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14969) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-45616b71feac52c1"></a>[`public ref Inno.Native.MiniAudio.MaLog Inno.Native.MiniAudio.MaContextPtr.Log { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14982) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b7fbe315047baf9a"></a>[`public static Inno.Native.MiniAudio.MaContextPtr Inno.Native.MiniAudio.MaContextPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14967) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-72269c9e99e78d26"></a>[`public ref Inno.Native.MiniAudio.MaDeviceInfoPtr Inno.Native.MiniAudio.MaContextPtr.PDeviceInfos { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14992) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-674de8f17918209d"></a>[`public ref uint Inno.Native.MiniAudio.MaContextPtr.PlaybackDeviceInfoCount { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14990) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e5517f809a506340"></a>[`public ref Inno.Native.MiniAudio.MaLogPtr Inno.Native.MiniAudio.MaContextPtr.PLog { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14981) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2462234a39d869e4"></a>[`public void* Inno.Native.MiniAudio.MaContextPtr.PUserData { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14985) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7dfe4ca349e3e63a"></a>[`public ref Inno.Native.MiniAudio.MaThreadPriority Inno.Native.MiniAudio.MaContextPtr.ThreadPriority { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14983) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0b7829b856bc0a71"></a>[`public ref nuint Inno.Native.MiniAudio.MaContextPtr.ThreadStackSize { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14984) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d26b8df282c21cd8"></a>[`public struct Inno.Native.MiniAudio.MaContextPtr : System.IEquatable<Inno.Native.MiniAudio.MaContextPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14958) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDataConverter</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDataConverter af49af77b81c7b1ee1f090ab0ff6e0361447726bc59d2f99d7a9941f9ff70931 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.FormatIn 50893cab2b62526be1f2c4f37e9360ed694c066b974ccd0a1dff839c448a3292 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.FormatOut 07339c7218c5f41306e8f4e18f12e04b0326e8fce2c210db59b88866dcb6f340 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.ChannelsIn a454de4e018b5829bb5429b936f564ece12e42aaec65906dde65be1fef98b1e4 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.ChannelsOut 555d2dedbe409eeadf4facf9f2af76be5e9277ab594f235c8300a43dcef1fa4b -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.SampleRateIn 4146c906663cd79eb0754bf78ca2aa0b9490b6e5d12af37084dfc6d8a3192ea8 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.SampleRateOut e57ff02b1535148876884aca00b0ee9081b531537679b27c5b6c61ebbe967fe7 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.DitherMode 78928a5c5bebec1294195ca5caac0424005fbb54dd182ef3f9c43b9b0df0362f -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.ExecutionPath 99a20c31540e0d279046391399b3c959d253a211cf5dedab5078b622829f058a -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.ChannelConverter edf192a3a6bca79ec9fbe5751ad877f88dca77cb568d009fa7b7337774c38065 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.Resampler 037d340b22d13b405e447f524f547cba0b30df90b0365db4adbd0a6ab12af430 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.HasPreFormatConversion 7c6b58a362384ba265a45a41a710b7da29d6c6ea89a537e703b66ad62af6dba5 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.HasPostFormatConversion 4ab5d6170a5c3e2bceef73885e2793a05dd0ef8727cc6357334a0f9aede8afb2 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.HasChannelConverter c05a31e8e2500206a3166f4a42d68b61750e443582f66aa9b00418764c1fd434 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.HasResampler 64c67ed0b0d92121139aab9f50aee0645ef1bf6442d5903728a8af02ac1e79fd -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.IsPassthrough 4c6cd73b6c9a5fcb8babec9489e75e16f07b03e54f7a51152e2e2d5b4bb2b8ea -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.Ownsheap 53bb6efe0bfc1e53df66491fc6a231bd2ab5c7109ed45c8a42b933fbee5f6272 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverter.Pheap 6f9ffe10f02e4734aba4fd757d6c2e4dceb6c37cfca5a0cd415a09500eafa49b -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverter.#ctor(Inno.Native.MiniAudio.MaFormat,Inno.Native.MiniAudio.MaFormat,System.UInt32,System.UInt32,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaDitherMode,Inno.Native.MiniAudio.MaDataConverterExecutionPath,Inno.Native.MiniAudio.MaChannelConverter,Inno.Native.MiniAudio.MaResampler,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Void*) e7148310f4f6fab596635c5152836f790f4748859d2c0e89f8c77bb581466770 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-271f4caa6d0d5275"></a>[`public Inno.Native.MiniAudio.MaChannelConverter Inno.Native.MiniAudio.MaDataConverter.ChannelConverter`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5679) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6a9be19187d2c21"></a>[`public uint Inno.Native.MiniAudio.MaDataConverter.ChannelsIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5673) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eafcf79ac7851026"></a>[`public uint Inno.Native.MiniAudio.MaDataConverter.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5674) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd06d030d58e24ff"></a>[`public Inno.Native.MiniAudio.MaDitherMode Inno.Native.MiniAudio.MaDataConverter.DitherMode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5677) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d4c49546b91dc16"></a>[`public Inno.Native.MiniAudio.MaDataConverterExecutionPath Inno.Native.MiniAudio.MaDataConverter.ExecutionPath`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5678) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-72cb540942adf6fb"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaDataConverter.FormatIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5671) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f7b21340771bae0e"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaDataConverter.FormatOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5672) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a29c9c8bb9834257"></a>[`public byte Inno.Native.MiniAudio.MaDataConverter.HasChannelConverter`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5683) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc6fee32fed9b586"></a>[`public byte Inno.Native.MiniAudio.MaDataConverter.HasPostFormatConversion`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5682) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7567f5a883f51b20"></a>[`public byte Inno.Native.MiniAudio.MaDataConverter.HasPreFormatConversion`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5681) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c618c5814c7d2b5c"></a>[`public byte Inno.Native.MiniAudio.MaDataConverter.HasResampler`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5684) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3cfe201a5cede70d"></a>[`public byte Inno.Native.MiniAudio.MaDataConverter.IsPassthrough`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5685) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d65c7bb4334f07fa"></a>[`public byte Inno.Native.MiniAudio.MaDataConverter.Ownsheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5686) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de777a3a0eb1e510"></a>[`public void* Inno.Native.MiniAudio.MaDataConverter.Pheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5687) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-999265ebefe072da"></a>[`public Inno.Native.MiniAudio.MaResampler Inno.Native.MiniAudio.MaDataConverter.Resampler`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5680) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-434ab0384892d6ab"></a>[`public uint Inno.Native.MiniAudio.MaDataConverter.SampleRateIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5675) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b722cd0d32f33452"></a>[`public uint Inno.Native.MiniAudio.MaDataConverter.SampleRateOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5676) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20e59a2f67256064"></a>[`public Inno.Native.MiniAudio.MaDataConverter.MaDataConverter(Inno.Native.MiniAudio.MaFormat formatIn = Inno.Native.MiniAudio.MaFormat.Unknown, Inno.Native.MiniAudio.MaFormat formatOut = Inno.Native.MiniAudio.MaFormat.Unknown, uint channelsIn = 0, uint channelsOut = 0, uint sampleRateIn = 0, uint sampleRateOut = 0, Inno.Native.MiniAudio.MaDitherMode ditherMode = Inno.Native.MiniAudio.MaDitherMode.None, Inno.Native.MiniAudio.MaDataConverterExecutionPath executionPath = Inno.Native.MiniAudio.MaDataConverterExecutionPath.Passthrough, Inno.Native.MiniAudio.MaChannelConverter channelConverter = default(Inno.Native.MiniAudio.MaChannelConverter), Inno.Native.MiniAudio.MaResampler resampler = default(Inno.Native.MiniAudio.MaResampler), byte hasPreFormatConversion = 0, byte hasPostFormatConversion = 0, byte hasChannelConverter = 0, byte hasResampler = 0, byte isPassthrough = 0, byte ownsHeap = 0, void* pHeap = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5688) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-04aba6f777e7d1ef"></a>[`public struct Inno.Native.MiniAudio.MaDataConverter`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5669) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDataConverterConfig</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDataConverterConfig c3db6b64018c98a131ee86abb0f6c5dcadccc39e1a1c40be7378d69dc1d79aff -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.FormatIn 64ae10d823fe816d1cf0e13900772887d15c609a9ebe69e1683b7f9bdf3e2b70 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.FormatOut f193d9896df56323ca866e7c8a097b09c7cdb87b68d1f7f97ae18395a010939c -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.ChannelsIn f777d9756118519dc6ed1d5545113b760cbf901a2e2a3c12e16082ecc74d962a -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.ChannelsOut d5678bbbe96939f2a2e2dd97da0837f05ac8325ee7f75c60c04e5844630ffa31 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.SampleRateIn 6b19e0817a5dfe4c67dfd9b39bb3c41bb7bf479123a87c1d170c5db543aaf89e -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.SampleRateOut b57f94e05a17125ab11fc3061f277bcffb770c21943666740f4d0aa9eb2ee3e5 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.PChannelMapIn bcb6e2785a27ebace14f178852220497bc445980f6aaa2562c08cf45da15f2c7 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.PChannelMapOut bb52643041c2b3eb15b8bffdbfac0cbceeb874edb8a6ab796db818848f222f87 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.DitherMode a25e50403d656d308dee90efac0010d223a0ba1572e9e0fc243d272f03faa1ea -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.ChannelMixMode d12f388cff96a37011820e90c28cb6f57d2075162f5d8eb1e305aa280cc3f0ac -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.CalculateLfeFromSpatialChannels c7fb55b0f1fc04463c06856faa3d2490d44025bb79a1f1db17e49d265749c125 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.PpChannelWeights 1130bdbb8a4ad05316133ffa5f44a8029ce668aaf029736dd857de64b31e7c03 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.AllowDynamicSampleRate d1cd103773225528618398f6321a752826cec9022414b5d1cd3fd3c17ace340c -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfig.Resampling 215a1f8dd352b0b0160aa807663cb12aa0e781080a0736c247763737a62ad74a -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterConfig.#ctor(Inno.Native.MiniAudio.MaFormat,Inno.Native.MiniAudio.MaFormat,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.Byte*,System.Byte*,Inno.Native.MiniAudio.MaDitherMode,Inno.Native.MiniAudio.MaChannelMixMode,System.UInt32,System.Single**,System.UInt32,Inno.Native.MiniAudio.MaResamplerConfig) b7a8cbf84d4b527b96fe12e84ee99a633d3c7f3ec31ea2458d27c630b72ef674 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1db6445c15ae0651"></a>[`public uint Inno.Native.MiniAudio.MaDataConverterConfig.AllowDynamicSampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11269) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e8e0c1992029bf07"></a>[`public uint Inno.Native.MiniAudio.MaDataConverterConfig.CalculateLfeFromSpatialChannels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11267) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61bd16077792bf85"></a>[`public Inno.Native.MiniAudio.MaChannelMixMode Inno.Native.MiniAudio.MaDataConverterConfig.ChannelMixMode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11266) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f3f55a4334e2812"></a>[`public uint Inno.Native.MiniAudio.MaDataConverterConfig.ChannelsIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11259) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-685074a47b4dfebb"></a>[`public uint Inno.Native.MiniAudio.MaDataConverterConfig.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11260) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-08097d70c6995428"></a>[`public Inno.Native.MiniAudio.MaDitherMode Inno.Native.MiniAudio.MaDataConverterConfig.DitherMode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11265) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af95cf982ece2bac"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaDataConverterConfig.FormatIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11257) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4d2ee36a9abb9df0"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaDataConverterConfig.FormatOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11258) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e019e8f75bee9617"></a>[`public byte* Inno.Native.MiniAudio.MaDataConverterConfig.PChannelMapIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11263) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-23238f7334ece5f2"></a>[`public byte* Inno.Native.MiniAudio.MaDataConverterConfig.PChannelMapOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11264) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce38fdfa1a4161b9"></a>[`public float** Inno.Native.MiniAudio.MaDataConverterConfig.PpChannelWeights`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11268) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-88d6710db1129518"></a>[`public Inno.Native.MiniAudio.MaResamplerConfig Inno.Native.MiniAudio.MaDataConverterConfig.Resampling`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11270) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0eef20b9773c0eee"></a>[`public uint Inno.Native.MiniAudio.MaDataConverterConfig.SampleRateIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11261) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc4c179d135155d2"></a>[`public uint Inno.Native.MiniAudio.MaDataConverterConfig.SampleRateOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11262) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-932b5aec1d764147"></a>[`public Inno.Native.MiniAudio.MaDataConverterConfig.MaDataConverterConfig(Inno.Native.MiniAudio.MaFormat formatIn = Inno.Native.MiniAudio.MaFormat.Unknown, Inno.Native.MiniAudio.MaFormat formatOut = Inno.Native.MiniAudio.MaFormat.Unknown, uint channelsIn = 0, uint channelsOut = 0, uint sampleRateIn = 0, uint sampleRateOut = 0, byte* pChannelMapIn = null, byte* pChannelMapOut = null, Inno.Native.MiniAudio.MaDitherMode ditherMode = Inno.Native.MiniAudio.MaDitherMode.None, Inno.Native.MiniAudio.MaChannelMixMode channelMixMode = Inno.Native.MiniAudio.MaChannelMixMode.Default, uint calculateLFEFromSpatialChannels = 0, float** ppChannelWeights = null, uint allowDynamicSampleRate = 0, Inno.Native.MiniAudio.MaResamplerConfig resampling = default(Inno.Native.MiniAudio.MaResamplerConfig))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11271) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-08e241a8145cc315"></a>[`public struct Inno.Native.MiniAudio.MaDataConverterConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11255) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDataConverterConfigPtr</code> · 27 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDataConverterConfigPtr b7e2c289aaa5154775c134a59d301ef7bb3cadbea2160bf479cef68942387307 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterConfigPtr.#ctor(Inno.Native.MiniAudio.MaDataConverterConfig*) 50ba75e4f0b40c7e277330f954926656bcdad9a9d82e7bde6c8de1ffd2d4cd49 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterConfigPtr.Handle a74bdf4028ea7f6aee207f7d73ea084a10e8f3899131b3a0602c3abdf6123db4 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.IsNull 998056887a9d3121cdaf55cdedac6d5475cb57807eac45ea17feec84a1579c88 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.Null 638477f0c5aa898eadd6ea77961cf2b151acd949d4aae1dadedceb12e14b110e -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.Item(System.Int32) 5189dd71674056e9459012f76c35cf76f081202cd99cfe0939fc4aa5378a8cc6 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaDataConverterConfig*)~Inno.Native.MiniAudio.MaDataConverterConfigPtr 6f30083fa01ed93799de36068562c34d7418f7ca2b1af2765a75662415adeb39 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaDataConverterConfigPtr)~Inno.Native.MiniAudio.MaDataConverterConfig* a5dbc86e5e61af337a4337f8d418132db79333abcd91e68c07531772402372e5 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterConfigPtr.op_Equality(Inno.Native.MiniAudio.MaDataConverterConfigPtr,Inno.Native.MiniAudio.MaDataConverterConfigPtr) e75f0306b2d551c18353dd6a2fc39968d125a5c7cfea1c812c8cac6b74b4f05a -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaDataConverterConfigPtr,Inno.Native.MiniAudio.MaDataConverterConfigPtr) 3b1743ee2d935d6991d0d10f4c290c4b774c5f18aaa80a3800aeed310d3b5d2a -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterConfigPtr.Equals(Inno.Native.MiniAudio.MaDataConverterConfigPtr) b2a81baa74965d0a8ef8140b515422d2d4d7933fdbf41836ebe7d61ffe36aeb9 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterConfigPtr.Equals(System.Object) e7ac790bf94d7085a25993488fb7b4f8b69ff2b5ec77a593ef1d2d98999a4759 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterConfigPtr.GetHashCode fb7c6ac4c148e2567af4c177435ff3957deb3a33881389ba1f4177dbc916d7aa -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.FormatIn 94073bba0c985c1f0f1a6e691b5988d1c4ccec9232525900ae4c98c9106175d2 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.FormatOut 7311dba1f90b0a8a70552b4efd5c75029937fe067f789581785d3eb4f39bef0b -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.ChannelsIn f82ffca64bf11a9a0538fc00c2e8dd020e3828c4f20e3629ea804be0c4128cec -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.ChannelsOut 8f943436aeea3260edf1c00328edb4cafdff785310084ba2e6dc76a9af98be23 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.SampleRateIn f60a6e65c9a2d55030fe786b0a2c0fca063f74fa1d0a40195baafa6b7c325e70 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.SampleRateOut c35917f5651cbb6f3392b6c30e17b0293f5c8c1458187e0565c67d4b1342c78c -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.PChannelMapIn 49134483d62ee55ffe9a2ced350337b5a72075890f522fcf8c930612979f3f53 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.PChannelMapOut a37a5a0b6b80b89fa20c924b353598dfb473e28d4db12725f5938f0ba920efa2 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.DitherMode 58a2ad305d60c10a64b97bdd6aafcca4e3a7cbf051439570ab576e9e47c329b7 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.ChannelMixMode 604336f382b726efac6cedd1163f684b5a424873832cfd07359ab8b7ba62d327 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.CalculateLfeFromSpatialChannels abc3c54afb33e78f19f542d004e34de6bc21a0e2ef692b8a301fbd4212d1b51f -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.PpChannelWeights d1426064f96d390164fa959846c741435e40e5ef37966fc56074819e6fd4f003 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.AllowDynamicSampleRate cd67643074a78d3d42663fa597f7a30f8fd66d5312dee25034da87f8f253fdde -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterConfigPtr.Resampling 1f06bccc9b0cec02312dfd13e924bfc10cdeb846e65da9a7d25bb88dca3994f2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-beec86ec2741e346"></a>[`public Inno.Native.MiniAudio.MaDataConverterConfig* Inno.Native.MiniAudio.MaDataConverterConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15005) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4cbca25b3ada9ffa"></a>[`public Inno.Native.MiniAudio.MaDataConverterConfigPtr.MaDataConverterConfigPtr(Inno.Native.MiniAudio.MaDataConverterConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15000) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3482d3189f13464d"></a>[`public bool Inno.Native.MiniAudio.MaDataConverterConfigPtr.Equals(Inno.Native.MiniAudio.MaDataConverterConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15015) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-303ae40a0cf0f0b0"></a>[`public override bool Inno.Native.MiniAudio.MaDataConverterConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15016) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-178c5e357fdea186"></a>[`public override int Inno.Native.MiniAudio.MaDataConverterConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15017) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-e5fd53ec3d3c17ed"></a>[`public static bool Inno.Native.MiniAudio.MaDataConverterConfigPtr.operator ==(Inno.Native.MiniAudio.MaDataConverterConfigPtr left, Inno.Native.MiniAudio.MaDataConverterConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15013) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-189412f104fd5230"></a>[`public static Inno.Native.MiniAudio.MaDataConverterConfigPtr.implicit operator Inno.Native.MiniAudio.MaDataConverterConfigPtr(Inno.Native.MiniAudio.MaDataConverterConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15011) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c710e55fc4af74c8"></a>[`public static Inno.Native.MiniAudio.MaDataConverterConfigPtr.implicit operator Inno.Native.MiniAudio.MaDataConverterConfig*(Inno.Native.MiniAudio.MaDataConverterConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15012) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-39a19ae0d4bcff31"></a>[`public static bool Inno.Native.MiniAudio.MaDataConverterConfigPtr.operator !=(Inno.Native.MiniAudio.MaDataConverterConfigPtr left, Inno.Native.MiniAudio.MaDataConverterConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15014) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-542d30ac5da139cf"></a>[`public ref uint Inno.Native.MiniAudio.MaDataConverterConfigPtr.AllowDynamicSampleRate { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15031) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-de3b694b7c690b18"></a>[`public ref uint Inno.Native.MiniAudio.MaDataConverterConfigPtr.CalculateLfeFromSpatialChannels { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15029) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d665f3a9c0bbee9c"></a>[`public ref Inno.Native.MiniAudio.MaChannelMixMode Inno.Native.MiniAudio.MaDataConverterConfigPtr.ChannelMixMode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15028) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d6f284251e9e6970"></a>[`public ref uint Inno.Native.MiniAudio.MaDataConverterConfigPtr.ChannelsIn { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15021) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-88e0b7178cf21cb0"></a>[`public ref uint Inno.Native.MiniAudio.MaDataConverterConfigPtr.ChannelsOut { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15022) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ece6623dafdf3c18"></a>[`public ref Inno.Native.MiniAudio.MaDitherMode Inno.Native.MiniAudio.MaDataConverterConfigPtr.DitherMode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15027) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d1081c6148e7dee9"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaDataConverterConfigPtr.FormatIn { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15019) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-968004c1bca4cf27"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaDataConverterConfigPtr.FormatOut { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15020) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0a946363c47f7758"></a>[`public bool Inno.Native.MiniAudio.MaDataConverterConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15006) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b4fed31d75a7fc7b"></a>[`public Inno.Native.MiniAudio.MaDataConverterConfig Inno.Native.MiniAudio.MaDataConverterConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15009) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ed20f23e856ed28e"></a>[`public static Inno.Native.MiniAudio.MaDataConverterConfigPtr Inno.Native.MiniAudio.MaDataConverterConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15007) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-35ba1ce44dfccbba"></a>[`public byte* Inno.Native.MiniAudio.MaDataConverterConfigPtr.PChannelMapIn { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15025) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ff18b4238339b387"></a>[`public byte* Inno.Native.MiniAudio.MaDataConverterConfigPtr.PChannelMapOut { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15026) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-48a60888a1453688"></a>[`public float** Inno.Native.MiniAudio.MaDataConverterConfigPtr.PpChannelWeights { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15030) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6b47985e3547d856"></a>[`public ref Inno.Native.MiniAudio.MaResamplerConfig Inno.Native.MiniAudio.MaDataConverterConfigPtr.Resampling { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15032) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1bcbd337673455da"></a>[`public ref uint Inno.Native.MiniAudio.MaDataConverterConfigPtr.SampleRateIn { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15023) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b5694af072df2b51"></a>[`public ref uint Inno.Native.MiniAudio.MaDataConverterConfigPtr.SampleRateOut { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15024) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-60aef515d9c57e83"></a>[`public struct Inno.Native.MiniAudio.MaDataConverterConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaDataConverterConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14998) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDataConverterExecutionPath</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDataConverterExecutionPath b07792344b3582044581f494f87fbf66335535157dd996fee407ed8539de1bc6 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterExecutionPath.Passthrough 33602f301ec3e657787d0f03033b48754988cc27a6ed7a374d70ca2f4672bb11 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterExecutionPath.FormatOnly 606ce9be90023bb92cbe048ba05839cece8a80a9693d2d6340b4362e88a2327e -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterExecutionPath.ChannelsOnly 577b839be9e551ab401c70cd8378296e7abd6a69cd3a1b51f2573ee568e8fc24 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterExecutionPath.ResampleOnly ca5ee8f64dcfe1c471fc675511519ca7e48bb9001440ee17f9804d7f86982912 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterExecutionPath.ResampleFirst 4ca61aa1df6406ef8ea9ca7bc87a7c975cc60b25a80c4cc5261911fc8218c752 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterExecutionPath.ChannelsFirst e9921c0fd732c5f49d6bb37535d9810262bc763021f824f2eb846357531527fc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c0cd452fda98fbc1"></a>[`Inno.Native.MiniAudio.MaDataConverterExecutionPath.ChannelsFirst = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L268) | 当前常量或枚举值：`5`。 | — |
| <a id="api-97cd5b59164b4d04"></a>[`Inno.Native.MiniAudio.MaDataConverterExecutionPath.ChannelsOnly = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L265) | 当前常量或枚举值：`2`。 | — |
| <a id="api-e4e53a3fcec7f3bf"></a>[`Inno.Native.MiniAudio.MaDataConverterExecutionPath.FormatOnly = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L264) | 当前常量或枚举值：`1`。 | — |
| <a id="api-38ab382c784a4ccf"></a>[`Inno.Native.MiniAudio.MaDataConverterExecutionPath.Passthrough = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L263) | 当前常量或枚举值：`0`。 | — |
| <a id="api-c92def3e9579f65a"></a>[`Inno.Native.MiniAudio.MaDataConverterExecutionPath.ResampleFirst = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L267) | 当前常量或枚举值：`4`。 | — |
| <a id="api-5c62900eb3da1633"></a>[`Inno.Native.MiniAudio.MaDataConverterExecutionPath.ResampleOnly = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L266) | 当前常量或枚举值：`3`。 | — |
| <a id="api-9a3e168807e5d3f8"></a>[`public enum Inno.Native.MiniAudio.MaDataConverterExecutionPath : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L261) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDataConverterPtr</code> · 30 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDataConverterPtr 73cabbd0a40b82f9944aaa57d904ef2c01660e479d27adbd996d4ca2200c58f8 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterPtr.#ctor(Inno.Native.MiniAudio.MaDataConverter*) ecf22163d59c4441e78a18ac7faaa3c30c978213f9d82adaa5259ef08c338f01 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataConverterPtr.Handle ef28974aaa1c8f6205b084da16a288a4b128676b517f56c42507f610e0829643 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.IsNull 9174d60c67b1185c381fc647a2e02e5d7ec9d7952cbe6fbc78806c1fc8e6fa7d -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.Null 6b95a347a4a792ae3b9ad5005043bdadc0047a5ecc3e0b1edf782244127af3b2 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.Item(System.Int32) 2028ac7f0d42c3587abdac4020c4044351e854a6c6cb04f173056590050266d3 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterPtr.op_Implicit(Inno.Native.MiniAudio.MaDataConverter*)~Inno.Native.MiniAudio.MaDataConverterPtr ad884ad9f39652ec03b0d94930309e4735b77f1bab58caba8052fa568a5d2db2 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterPtr.op_Implicit(Inno.Native.MiniAudio.MaDataConverterPtr)~Inno.Native.MiniAudio.MaDataConverter* 5eb26100ba0ff67199adb64c6d1a703877c5c80def68dac33a91c4188c945a5d -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterPtr.op_Equality(Inno.Native.MiniAudio.MaDataConverterPtr,Inno.Native.MiniAudio.MaDataConverterPtr) 8552cfad679f97e3f1a3fc6b9b77efd00a28b9d5d267f57b0cc6bc6bb9f26e6f -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterPtr.op_Inequality(Inno.Native.MiniAudio.MaDataConverterPtr,Inno.Native.MiniAudio.MaDataConverterPtr) f579d9da248c196c82b0e1bb4ce5fc416d2b1aa386e4a1eccdec65ed4d75c3ee -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterPtr.Equals(Inno.Native.MiniAudio.MaDataConverterPtr) d84bd7eda9ad74ce85a64cafd599a552888130028764147bc1d6c4b211e66884 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterPtr.Equals(System.Object) d4000ee343a25926be7691f497d5bc7ae06952c3aa0782a750411eb384e9cb9d -->
<!-- api: M:Inno.Native.MiniAudio.MaDataConverterPtr.GetHashCode af04da1406bb898d12b673f96eae48c9e3c4f59eadd963919d09f02d4c7d7cf2 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.FormatIn 96d9c8e70b9b4acfd53b7e4aeda046c549e4b7d99e3b9f672e62bcd3b3cbbdce -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.FormatOut 4b3b9d597ea8be197657d00cf1ad4ac42c821e320070b3557a5cf9b3054a193a -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.ChannelsIn 4c5ba4e2b34ea9b3810f47927416650950476fe99c7f2fffbe695d40ee03a491 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.ChannelsOut 1b534075f8d6108adc5e28ec2ed09088a3fa71d22e8f371e7ca5c84061b58c57 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.SampleRateIn 816d9163ea8bd3442d18f534ad5cbfc17e2a0acf7c44d1016d6dc3823790e878 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.SampleRateOut 72ff689b578fac16efbe09a970c811034d777da076bf9941b826185997bc770c -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.DitherMode 055f891c512411526c6f208b1b3fffa486a04ee9400b37511f8c05bae858055e -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.ExecutionPath 8d7f457d521c3c7ba41b7e12b0bf63061d2cdc2e46158e4b79386ba9031dd6b7 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.ChannelConverter af13b3a8d30b3e6635c80637d565ada94d922d247fbccded32288051fcabe644 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.Resampler eac10f883d01684cfc8bbc4b3555489dfae253734c64fa824386e36aaa0347ee -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.HasPreFormatConversion 76a98bc54cef80cacad1a34cbd8418468a8ec8892389798139867396e0052e28 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.HasPostFormatConversion d3745728dbcd212fb0a6b7c49f3139469b63ddf266014adba4c1851afebc372e -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.HasChannelConverter dad7de3c7ff20d7e166e85a1cb57e42d3152c4b302a5c538e62bbaffecbfbf7d -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.HasResampler 528be1aeec552ac3eadb1a406be9bea319c1c1171dc29f62ba627c30a8977af6 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.IsPassthrough 39e434870c101dc1da4dc45585f94ced3f951b6e045372b51eda534bf274f1b3 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.Ownsheap f4fdcbf86244a64749187aaaaa4b6e0b6f52ffa9f83fb7658f5be4d41317912b -->
<!-- api: P:Inno.Native.MiniAudio.MaDataConverterPtr.Pheap 68bb50ac3e209deef23eaae52b3894c8af856556565ad5e95644f057b71ff017 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fb7b4e832d73591d"></a>[`public Inno.Native.MiniAudio.MaDataConverter* Inno.Native.MiniAudio.MaDataConverterPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15043) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e05a442a9ccb5436"></a>[`public Inno.Native.MiniAudio.MaDataConverterPtr.MaDataConverterPtr(Inno.Native.MiniAudio.MaDataConverter* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15038) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-94e8f72bc4ffc9f1"></a>[`public bool Inno.Native.MiniAudio.MaDataConverterPtr.Equals(Inno.Native.MiniAudio.MaDataConverterPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15053) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-8bec57a001c2e029"></a>[`public override bool Inno.Native.MiniAudio.MaDataConverterPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15054) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-703f64d22bd4278c"></a>[`public override int Inno.Native.MiniAudio.MaDataConverterPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15055) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-c355c3bc6163115b"></a>[`public static bool Inno.Native.MiniAudio.MaDataConverterPtr.operator ==(Inno.Native.MiniAudio.MaDataConverterPtr left, Inno.Native.MiniAudio.MaDataConverterPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15051) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9eb38a4e0000c87c"></a>[`public static Inno.Native.MiniAudio.MaDataConverterPtr.implicit operator Inno.Native.MiniAudio.MaDataConverterPtr(Inno.Native.MiniAudio.MaDataConverter* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15049) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ebe9fe9662abc02f"></a>[`public static Inno.Native.MiniAudio.MaDataConverterPtr.implicit operator Inno.Native.MiniAudio.MaDataConverter*(Inno.Native.MiniAudio.MaDataConverterPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15050) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-97476ff6dee95cf1"></a>[`public static bool Inno.Native.MiniAudio.MaDataConverterPtr.operator !=(Inno.Native.MiniAudio.MaDataConverterPtr left, Inno.Native.MiniAudio.MaDataConverterPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15052) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e34850c32072e53"></a>[`public ref Inno.Native.MiniAudio.MaChannelConverter Inno.Native.MiniAudio.MaDataConverterPtr.ChannelConverter { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15065) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-431e292b5062929c"></a>[`public ref uint Inno.Native.MiniAudio.MaDataConverterPtr.ChannelsIn { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15059) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0594d46e746c4a35"></a>[`public ref uint Inno.Native.MiniAudio.MaDataConverterPtr.ChannelsOut { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15060) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-240566d8b9cd989f"></a>[`public ref Inno.Native.MiniAudio.MaDitherMode Inno.Native.MiniAudio.MaDataConverterPtr.DitherMode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15063) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6c7cd6012ae1aae5"></a>[`public ref Inno.Native.MiniAudio.MaDataConverterExecutionPath Inno.Native.MiniAudio.MaDataConverterPtr.ExecutionPath { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15064) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e6def8ead648c7c0"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaDataConverterPtr.FormatIn { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15057) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-037744257e01e249"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaDataConverterPtr.FormatOut { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15058) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-815261a3057f2030"></a>[`public ref byte Inno.Native.MiniAudio.MaDataConverterPtr.HasChannelConverter { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15069) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ec71504c7420589f"></a>[`public ref byte Inno.Native.MiniAudio.MaDataConverterPtr.HasPostFormatConversion { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15068) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a6298cfe83d386ec"></a>[`public ref byte Inno.Native.MiniAudio.MaDataConverterPtr.HasPreFormatConversion { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15067) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f1207f7219b26447"></a>[`public ref byte Inno.Native.MiniAudio.MaDataConverterPtr.HasResampler { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15070) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a843c978b5738d5b"></a>[`public bool Inno.Native.MiniAudio.MaDataConverterPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15044) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-342e088b6353bbe3"></a>[`public ref byte Inno.Native.MiniAudio.MaDataConverterPtr.IsPassthrough { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15071) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5bb3291523744ea3"></a>[`public Inno.Native.MiniAudio.MaDataConverter Inno.Native.MiniAudio.MaDataConverterPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15047) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ee2153d42314b8f2"></a>[`public static Inno.Native.MiniAudio.MaDataConverterPtr Inno.Native.MiniAudio.MaDataConverterPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15045) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b182990229d56577"></a>[`public ref byte Inno.Native.MiniAudio.MaDataConverterPtr.Ownsheap { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15072) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-28d1644d645e39cc"></a>[`public void* Inno.Native.MiniAudio.MaDataConverterPtr.Pheap { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15073) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ac45bc9495bd0fd6"></a>[`public ref Inno.Native.MiniAudio.MaResampler Inno.Native.MiniAudio.MaDataConverterPtr.Resampler { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15066) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9a776255a963976a"></a>[`public ref uint Inno.Native.MiniAudio.MaDataConverterPtr.SampleRateIn { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15061) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5f4cb96b0e03f178"></a>[`public ref uint Inno.Native.MiniAudio.MaDataConverterPtr.SampleRateOut { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15062) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e4c096ce5be35e9d"></a>[`public struct Inno.Native.MiniAudio.MaDataConverterPtr : System.IEquatable<Inno.Native.MiniAudio.MaDataConverterPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15036) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDataSource</code> · 1 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDataSource 361f3d0765815a3cafafa1db0f27d3a1cf5581e348bdb4427a5012c2fcf650b5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-969b94cbd94cd977"></a>[`public struct Inno.Native.MiniAudio.MaDataSource`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12041) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDataSourceBase</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDataSourceBase 7310e43fa8cc2c5b1dc4ff243dc37a9b43d33cb06acd1355f6971c3c4507ee2b -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceBase.Vtable a61753d4784fb18c1b23f6a10058ce9e0ff55b3a47c0a42b86b276a536b67f6d -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceBase.RangeBegInFrames 5c1a405847d9fc3fe7d6de54a65b0e7547b745129e6f0d94c564d8f0c1932726 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceBase.RangeEndInFrames fb3f9255c25383a89bf7f7c5cc8201602c5073c55f884da0af1028029ec0ad23 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceBase.LoopBegInFrames 724075bff119ee18e844121f079d4a734f86e48e22f972e6a5d2dc87217211e3 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceBase.LoopEndInFrames 7dcbecd7df375e9ecf8eb8c9de10665bfb5b710c0ab4ba66501e789709beaac8 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceBase.PCurrent 566770d29c6cb31bd611c457598e51c905cb40a52f27c0280d04a777c1f8121f -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceBase.PNext 9bad8b6c8fd14e795a528bafa899204a1fa5841a406007e43aeb7d3c8dc14f64 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceBase.OnGetNext 99383e836d3b1737ceed61bc7473ccb76ab15196ae87474a029c1af5416c7f2f -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceBase.IsLooping ee54aef062cf5c326b12b9bf907def1ce53a942be974b4ab4b9bfc5be65ce86a -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceBase.#ctor(Inno.Native.MiniAudio.MaDataSourceVtable*,System.UInt64,System.UInt64,System.UInt64,System.UInt64,System.Void*,System.Void*,System.Void*,System.UInt32) 5fa318c0762495635c8b801f25bcf40d564cf56e22153d06b6c8a227a06f0452 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-476a756bb01f0c1d"></a>[`public uint Inno.Native.MiniAudio.MaDataSourceBase.IsLooping`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5242) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1655287339509889"></a>[`public ulong Inno.Native.MiniAudio.MaDataSourceBase.LoopBegInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5237) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f02727ca0fbf0b0d"></a>[`public ulong Inno.Native.MiniAudio.MaDataSourceBase.LoopEndInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5238) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa76dbe8df5a51af"></a>[`public void* Inno.Native.MiniAudio.MaDataSourceBase.OnGetNext`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5241) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b02c4c95a6d48bcf"></a>[`public void* Inno.Native.MiniAudio.MaDataSourceBase.PCurrent`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5239) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad9d1662e2136e42"></a>[`public void* Inno.Native.MiniAudio.MaDataSourceBase.PNext`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5240) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0d28a2270936da6d"></a>[`public ulong Inno.Native.MiniAudio.MaDataSourceBase.RangeBegInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5235) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea2f7a77a646b9b1"></a>[`public ulong Inno.Native.MiniAudio.MaDataSourceBase.RangeEndInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5236) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54011cd4917b0e63"></a>[`public Inno.Native.MiniAudio.MaDataSourceVtable* Inno.Native.MiniAudio.MaDataSourceBase.Vtable`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5234) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-23bfd0b55f7e4a6c"></a>[`public Inno.Native.MiniAudio.MaDataSourceBase.MaDataSourceBase(Inno.Native.MiniAudio.MaDataSourceVtable* vtable = null, ulong rangeBegInFrames = 0, ulong rangeEndInFrames = 0, ulong loopBegInFrames = 0, ulong loopEndInFrames = 0, void* pCurrent = null, void* pNext = null, void* onGetNext = null, uint isLooping = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5243) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e639c7464d4312da"></a>[`public struct Inno.Native.MiniAudio.MaDataSourceBase`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5232) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDataSourceConfig</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDataSourceConfig 3959df3a2d14efb74b2f6390fd93b4484048401d099de3e4b33de6af818ed0e6 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceConfig.Vtable a31246a3e4b15695b11032d3efbd14fde700f225edc9d9c4af7777821b9574d6 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceConfig.#ctor(Inno.Native.MiniAudio.MaDataSourceVtable*) 148a8757fa83284261b4ff1def04584539fb2afc67625387585e0a8cec5c988d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-43f2ab00d89fba23"></a>[`public Inno.Native.MiniAudio.MaDataSourceVtable* Inno.Native.MiniAudio.MaDataSourceConfig.Vtable`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11296) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6a20cc6f07dcb85b"></a>[`public Inno.Native.MiniAudio.MaDataSourceConfig.MaDataSourceConfig(Inno.Native.MiniAudio.MaDataSourceVtable* vtable = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11297) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f3ab77ef1f67a023"></a>[`public struct Inno.Native.MiniAudio.MaDataSourceConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11294) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDataSourceConfigPtr</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDataSourceConfigPtr 24ef0552432945ccbe372046f5e9b7d0268603c948f53fce7e8c62ffd7e0f5f4 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceConfigPtr.#ctor(Inno.Native.MiniAudio.MaDataSourceConfig*) 0fd01e08c32c63b7eba8a904b6ad2ab15f893406b18522f7f8bc9a7db414476a -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceConfigPtr.Handle 856672e2ffa70a1ee051518cba2d202510d545eb429f59637be508b01279f927 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataSourceConfigPtr.IsNull da011f2675bbf47d5c902eb0781a4524ba6a61596750c8a1a3ab202374c30e69 -->
<!-- api: P:Inno.Native.MiniAudio.MaDataSourceConfigPtr.Null 02f12b482c97b2cc25256f0874dda2da9dc2181bf34b0d7ce843e6442487d7bf -->
<!-- api: P:Inno.Native.MiniAudio.MaDataSourceConfigPtr.Item(System.Int32) 42b17d63d36b5345dbb64ed81a8e0644dd66fb70f918aedb0c97129ddd417a28 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaDataSourceConfig*)~Inno.Native.MiniAudio.MaDataSourceConfigPtr 03d2ee13542d78ca40dc96e9900b5d152109961e3abf86b08570a9267c02addf -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaDataSourceConfigPtr)~Inno.Native.MiniAudio.MaDataSourceConfig* 0ed63bf8b64d010d711570e8e1eb83e427305c71535cc4458d7f050c30780eed -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceConfigPtr.op_Equality(Inno.Native.MiniAudio.MaDataSourceConfigPtr,Inno.Native.MiniAudio.MaDataSourceConfigPtr) c9772f80ede5cdcc3955d8893fb1d34b388647e157a243581387bf9ac2b433be -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaDataSourceConfigPtr,Inno.Native.MiniAudio.MaDataSourceConfigPtr) 2226fd6fe3d14139181c02e0be4e7bf8868046c6b9683fd41106b64ee55befa9 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceConfigPtr.Equals(Inno.Native.MiniAudio.MaDataSourceConfigPtr) eff9d9d56beac71a1eb4d02b0bb25328d70ef1bdca8658b56cd4da2c44b82d41 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceConfigPtr.Equals(System.Object) c36055a96e4dd53445a85cfdcae3d0ebd71ff6bc2ede32fecad9bf03daffcecd -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceConfigPtr.GetHashCode 6c212a16869fde687db14a1b14bdd298d57761a8e4cff1fde9c5af1b6e013d7a -->
<!-- api: P:Inno.Native.MiniAudio.MaDataSourceConfigPtr.Vtable 0bdff392c572977fe45d056c6c0d48f4f10960c049306a79cd2dd857f66cd363 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ffd80f4845c72fbd"></a>[`public Inno.Native.MiniAudio.MaDataSourceConfig* Inno.Native.MiniAudio.MaDataSourceConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15084) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0b8c106a2a437246"></a>[`public Inno.Native.MiniAudio.MaDataSourceConfigPtr.MaDataSourceConfigPtr(Inno.Native.MiniAudio.MaDataSourceConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15079) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-910ba83aa0ddb5ec"></a>[`public bool Inno.Native.MiniAudio.MaDataSourceConfigPtr.Equals(Inno.Native.MiniAudio.MaDataSourceConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15094) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-bcc6b976ea90bbd5"></a>[`public override bool Inno.Native.MiniAudio.MaDataSourceConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15095) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-5e3f734ffaa1dffe"></a>[`public override int Inno.Native.MiniAudio.MaDataSourceConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15096) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-098e42c85b698b79"></a>[`public static bool Inno.Native.MiniAudio.MaDataSourceConfigPtr.operator ==(Inno.Native.MiniAudio.MaDataSourceConfigPtr left, Inno.Native.MiniAudio.MaDataSourceConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15092) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9a6a105b8574c5be"></a>[`public static Inno.Native.MiniAudio.MaDataSourceConfigPtr.implicit operator Inno.Native.MiniAudio.MaDataSourceConfigPtr(Inno.Native.MiniAudio.MaDataSourceConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15090) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-465cca0e1a689792"></a>[`public static Inno.Native.MiniAudio.MaDataSourceConfigPtr.implicit operator Inno.Native.MiniAudio.MaDataSourceConfig*(Inno.Native.MiniAudio.MaDataSourceConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15091) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-466414197fa9b9ac"></a>[`public static bool Inno.Native.MiniAudio.MaDataSourceConfigPtr.operator !=(Inno.Native.MiniAudio.MaDataSourceConfigPtr left, Inno.Native.MiniAudio.MaDataSourceConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15093) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4212503d919ed1eb"></a>[`public bool Inno.Native.MiniAudio.MaDataSourceConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15085) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4ea2e792aec843b7"></a>[`public Inno.Native.MiniAudio.MaDataSourceConfig Inno.Native.MiniAudio.MaDataSourceConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15088) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7a48cb3c89c66c48"></a>[`public static Inno.Native.MiniAudio.MaDataSourceConfigPtr Inno.Native.MiniAudio.MaDataSourceConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15086) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0674824e8a5eec3e"></a>[`public ref Inno.Native.MiniAudio.MaDataSourceVtablePtr Inno.Native.MiniAudio.MaDataSourceConfigPtr.Vtable { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15098) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f6dbdba810e91e3c"></a>[`public struct Inno.Native.MiniAudio.MaDataSourceConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaDataSourceConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15077) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDataSourceGetNextProc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDataSourceGetNextProc 9110dea6c670542193afde8b1ef66398653261ce24b4c76cb8b8fa33716e0aa2 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceGetNextProc.#ctor(System.Object,System.IntPtr) 9f15362024cc2a81f63c4a7152b2a17fee27f87dfc2e5d151ece6be13b765ca2 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceGetNextProc.Invoke(System.Void*) 50061c663e9cd9602edc8b2dc6be41cc3097b00b7299d6418475e157c93d71f4 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceGetNextProc.BeginInvoke(System.Void*,System.AsyncCallback,System.Object) cb4a9405455a6aeb72fab5b40faca6f5817cbc8ea7ff71967e6622e5172c9aaa -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceGetNextProc.EndInvoke(System.IAsyncResult) 1d1183c8983fa33fa80eaf98db80ee5ef5ea81bff846f8b8d02fb2bc112e839d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-320e7aecda002499"></a>[`public Inno.Native.MiniAudio.MaDataSourceGetNextProc.MaDataSourceGetNextProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13946) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-711df65b82a37f16"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaDataSourceGetNextProc.BeginInvoke(void* pDataSource, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13946) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f05b2a1932ec6c14"></a>[`public virtual void* Inno.Native.MiniAudio.MaDataSourceGetNextProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13946) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c30143cb200a0e37"></a>[`public virtual void* Inno.Native.MiniAudio.MaDataSourceGetNextProc.Invoke(void* pDataSource)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13946) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-76a6ae5292a7b9a4"></a>[`public delegate void* Inno.Native.MiniAudio.MaDataSourceGetNextProc(void* pDataSource)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13946) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDataSourceNode</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDataSourceNode 44856eb81219434fc9b50dff336d937e3e533b3ad30b3951e1025784147cfcfa -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceNode.Base 7f6dc4066da4c5b407b62eba72af5409b3d36fa4569900d6cdd163a7c83ce7ad -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceNode.PDataSource 5e063e0fad696cfe8379df405f0c7f6138923fb3fd51a9c80706c5d020616103 -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceNode.#ctor(Inno.Native.MiniAudio.MaNodeBase,Inno.Native.MiniAudio.MaDataSource*) 5d198f92a375428c9ec045a70d7b134c7be63df6b0a7e157298245957a9c91b5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-794eacffcfe55521"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaDataSourceNode.Base`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13224) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-25fb3bb770e64981"></a>[`public Inno.Native.MiniAudio.MaDataSource* Inno.Native.MiniAudio.MaDataSourceNode.PDataSource`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13225) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c9914d0ecabe3cf6"></a>[`public Inno.Native.MiniAudio.MaDataSourceNode.MaDataSourceNode(Inno.Native.MiniAudio.MaNodeBase @base = default(Inno.Native.MiniAudio.MaNodeBase), Inno.Native.MiniAudio.MaDataSource* pDataSource = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13226) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-16f1e24a14339100"></a>[`public struct Inno.Native.MiniAudio.MaDataSourceNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13222) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDataSourceNodeConfig</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDataSourceNodeConfig e988394e37ce7217212c1501910fc902b612269ef1d6791257d50536026fd84e -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceNodeConfig.NodeConfig ca210404364526c622a73b83da8a05d59ef98342fdec0288b022b9eae526f836 -->
<!-- api: F:Inno.Native.MiniAudio.MaDataSourceNodeConfig.PDataSource 878bf10daefa84561a6267fce7d572a70ae2e2a1c8cb69bcd77baefedc1500ec -->
<!-- api: M:Inno.Native.MiniAudio.MaDataSourceNodeConfig.#ctor(Inno.Native.MiniAudio.MaNodeConfig,Inno.Native.MiniAudio.MaDataSource*) 1a171e64960271c335e4cd4d538750e9cd4b568abf06e4b78145b383658d14d3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dcdc3a3030f195f4"></a>[`public Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaDataSourceNodeConfig.NodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13209) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-41b9262a785acccf"></a>[`public Inno.Native.MiniAudio.MaDataSource* Inno.Native.MiniAudio.MaDataSourceNodeConfig.PDataSource`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13210) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20b1810239fb5a06"></a>[`public Inno.Native.MiniAudio.MaDataSourceNodeConfig.MaDataSourceNodeConfig(Inno.Native.MiniAudio.MaNodeConfig nodeConfig = default(Inno.Native.MiniAudio.MaNodeConfig), Inno.Native.MiniAudio.MaDataSource* pDataSource = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13211) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5b499d02d60daa0c"></a>[`public struct Inno.Native.MiniAudio.MaDataSourceNodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13207) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
