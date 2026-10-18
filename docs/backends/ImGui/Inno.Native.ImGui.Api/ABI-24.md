# Inno.Native.ImGui ABI · ImGuiTableHeaderDataPtr → ImGuiTextFilterPtr

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableHeaderDataPtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableHeaderDataPtr 5fa3b0e13983e34fc148a0727857303de5b2f8b9c64337334c95717d808da38a -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.#ctor(Inno.Native.ImGui.ImGuiTableHeaderData*) 4f089797ca751305b6a05c5e14f1c825fd29b7bccabd19c89143d1eaa4ee86e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.Handle 82ba02e742d7c03cfc10b273e87b6f08dd878b5692fdd7907e5079c97b1abbc2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.IsNull 1d4d7abddd44009b8332349c38a2ca10bc6a127c7d2f7b76c4983ebe7a71a923 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.Null 486d7134dc5ba026a852e312b6c3f23917212821aa154180ff35213421dc61f9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.Item(System.Int32) 0d8d461daaf4baa2348f8d834c91ceb443fa6b18aa42421db1cb885e92285f04 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableHeaderData*)~Inno.Native.ImGui.ImGuiTableHeaderDataPtr f0c94f1d96465706b2320ea5129214a859d1ad8922d491e224b18ac62cd23ab3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableHeaderDataPtr)~Inno.Native.ImGui.ImGuiTableHeaderData* 5fa0bb491ca36d70a188a674cda754365d7d734c08daf4f14406b21787ec470c -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.op_Equality(Inno.Native.ImGui.ImGuiTableHeaderDataPtr,Inno.Native.ImGui.ImGuiTableHeaderDataPtr) fcf5d789a82c0062824fad3701b1dfc884e4bb473967fd108e1e9d202c1e236c -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.op_Inequality(Inno.Native.ImGui.ImGuiTableHeaderDataPtr,Inno.Native.ImGui.ImGuiTableHeaderDataPtr) 3b2551aa69adedb59ea9aae29a3bf860b5b7cb34a3bec05c32870ef2f08ec375 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.Equals(Inno.Native.ImGui.ImGuiTableHeaderDataPtr) e425741390e65be048812fe86426f5a0ed72d65abaac65209566112b4ef2a4fa -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.Equals(System.Object) ffdfc951a20c9ec114d268bb5cc81fe5027cfc317de2353698ead65d23282436 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.GetHashCode 234395d51cad7aa45239fb427bc552861db170d8afc54c0c6deae349245321ac -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.Index 2ba5a4c3fe1ad3da485c390572873914ea82fa3ebf8215eff21e02d86dddaf6b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.TextColor 93d541abd8a3bd86e735d9500bd8a5b527be8b711cd214108e3d9d67dcfb6611 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.BgColor0 14c0d28f8517e377e7ec65fab5e15e21d532a591ea457090771c597ba323da37 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableHeaderDataPtr.BgColor1 9e0115666beab9676d8085d447812abf11a822ddff41452ac46f51d123fd9392 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-02da68fdf9cb1b07"></a>[`public Inno.Native.ImGui.ImGuiTableHeaderData* Inno.Native.ImGui.ImGuiTableHeaderDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16914) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21a0f02a99fe3511"></a>[`public Inno.Native.ImGui.ImGuiTableHeaderDataPtr.ImGuiTableHeaderDataPtr(Inno.Native.ImGui.ImGuiTableHeaderData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16909) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-116f578d391459e5"></a>[`public bool Inno.Native.ImGui.ImGuiTableHeaderDataPtr.Equals(Inno.Native.ImGui.ImGuiTableHeaderDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16924) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-5513caed5bea62eb"></a>[`public override bool Inno.Native.ImGui.ImGuiTableHeaderDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16925) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-07751027aea2b092"></a>[`public override int Inno.Native.ImGui.ImGuiTableHeaderDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16926) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-0ffe42942f817c68"></a>[`public static bool Inno.Native.ImGui.ImGuiTableHeaderDataPtr.operator ==(Inno.Native.ImGui.ImGuiTableHeaderDataPtr left, Inno.Native.ImGui.ImGuiTableHeaderDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16922) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-28a0c0342096e1d5"></a>[`public static Inno.Native.ImGui.ImGuiTableHeaderDataPtr.implicit operator Inno.Native.ImGui.ImGuiTableHeaderDataPtr(Inno.Native.ImGui.ImGuiTableHeaderData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16920) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7febca36cf576aed"></a>[`public static Inno.Native.ImGui.ImGuiTableHeaderDataPtr.implicit operator Inno.Native.ImGui.ImGuiTableHeaderData*(Inno.Native.ImGui.ImGuiTableHeaderDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16921) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-176ed39cf1e3d9b8"></a>[`public static bool Inno.Native.ImGui.ImGuiTableHeaderDataPtr.operator !=(Inno.Native.ImGui.ImGuiTableHeaderDataPtr left, Inno.Native.ImGui.ImGuiTableHeaderDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16923) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d3f6f649fa7d52ad"></a>[`public ref uint Inno.Native.ImGui.ImGuiTableHeaderDataPtr.BgColor0 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16930) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5f5e59e9cd5b267a"></a>[`public ref uint Inno.Native.ImGui.ImGuiTableHeaderDataPtr.BgColor1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16931) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-44eab32182a1dd6f"></a>[`public ref short Inno.Native.ImGui.ImGuiTableHeaderDataPtr.Index { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16928) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b9f00f739458323b"></a>[`public bool Inno.Native.ImGui.ImGuiTableHeaderDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16915) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-131432fe915cfe83"></a>[`public Inno.Native.ImGui.ImGuiTableHeaderData Inno.Native.ImGui.ImGuiTableHeaderDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16918) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-84b3c225e2481ad9"></a>[`public static Inno.Native.ImGui.ImGuiTableHeaderDataPtr Inno.Native.ImGui.ImGuiTableHeaderDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16916) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b773e812224fc36b"></a>[`public ref uint Inno.Native.ImGui.ImGuiTableHeaderDataPtr.TextColor { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16929) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b7a08771725f669a"></a>[`public struct Inno.Native.ImGui.ImGuiTableHeaderDataPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTableHeaderDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16907) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableInstanceData</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableInstanceData 1a7cf710f9dd2b9cda65d2f4e926d552b51a2c6388eae745c98bbeedaebf28f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableInstanceData.TableInstanceID ca1dd1df7468d9ae4d5f282bd55edf08d947bc329a5efd50d0d6a36b83677fdf -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableInstanceData.LastOuterHeight 0f88b19afb2753f07e28cb46a0ba5e8cfac0176504b79c4d9e605fc7d6a2d393 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableInstanceData.LastTopHeadersRowHeight b246847cb401fd81f1b19d7e1d1a9667e4dbe5e39f1255d9994a7ed93340b93d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableInstanceData.LastFrozenHeight 94d31d7b6885fc05e822026c35276aac65696fd856ac112699d8902546e7f2bb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableInstanceData.HoveredRowLast 0f5d61c99c8edc149503c522bc46574c991b1a0563b0a23b40be0ca37e24a3d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableInstanceData.HoveredRowNext 7a2b6c4b22d18963527c68c86075e006fa2556615f03797bd167fa5d5de8e15a -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableInstanceData.#ctor(System.UInt32,System.Single,System.Single,System.Single,System.Int32,System.Int32) 7205ec0d7047dce21a807114766162a72b9cd23d42c2c04eea3655948c420e13 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableInstanceData.Destroy 99341fb30146cd2f808d62215d56ad8451888a6061e045d0ad51e0a361e4b100 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f4581be651344dbf"></a>[`public int Inno.Native.ImGui.ImGuiTableInstanceData.HoveredRowLast`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6305) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e52522c49020c22"></a>[`public int Inno.Native.ImGui.ImGuiTableInstanceData.HoveredRowNext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6306) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0d53b154cdc07dfe"></a>[`public float Inno.Native.ImGui.ImGuiTableInstanceData.LastFrozenHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6304) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f9a737440f5961f"></a>[`public float Inno.Native.ImGui.ImGuiTableInstanceData.LastOuterHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6302) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e61eab8364e27e86"></a>[`public float Inno.Native.ImGui.ImGuiTableInstanceData.LastTopHeadersRowHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6303) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0ed3c6aff43f0869"></a>[`public uint Inno.Native.ImGui.ImGuiTableInstanceData.TableInstanceID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6301) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c113f49e058b8bcd"></a>[`public Inno.Native.ImGui.ImGuiTableInstanceData.ImGuiTableInstanceData(uint tableInstanceID = 0, float lastOuterHeight = 0, float lastTopHeadersRowHeight = 0, float lastFrozenHeight = 0, int hoveredRowLast = 0, int hoveredRowNext = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6307) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-52f695cef7945b85"></a>[`public void Inno.Native.ImGui.ImGuiTableInstanceData.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67589) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5b9f495e51a20b6f"></a>[`public struct Inno.Native.ImGui.ImGuiTableInstanceData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6299) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableInstanceDataPtr</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableInstanceDataPtr f45f6f96165fc19a19f6c896c7f33d0b8b0972ad616661ca92d2dd6ac5c6212c -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.#ctor(Inno.Native.ImGui.ImGuiTableInstanceData*) 38c9380b596550d6ca99ce96216b9f349433ee5b145d6bc2e107ff10d975bab4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.Handle 296e4706105b88197d6d429d9fec5626cfee65a09d82b2671420c357fd21cd10 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.IsNull c00d94e98b1b18dd62323410e09c098e55d58e6144da4fc371cd7e8cb58fb0f2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.Null 2d84d1b53ffcf30c8b05ae8fb4ff492dc1fab80f92aeec384cd45f1639996fd0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.Item(System.Int32) bc8d8c4bed734c0c4480904c3c5fda3f18f8cad2bf38dbd3d29a5cabeda2fcd2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableInstanceData*)~Inno.Native.ImGui.ImGuiTableInstanceDataPtr 476ee109e17433f8c88f5a7537bb8b29b308cd6ca028b1c1d02ca7bcc286c015 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableInstanceDataPtr)~Inno.Native.ImGui.ImGuiTableInstanceData* 9134c63b68ee8b21065073b0b69de00bde77a90443a0b2b56e86d7ff777b9b52 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.op_Equality(Inno.Native.ImGui.ImGuiTableInstanceDataPtr,Inno.Native.ImGui.ImGuiTableInstanceDataPtr) cf5774bb9e2656c62857093959a73851af58e8e95bb48a1ca6db51f71b300d86 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.op_Inequality(Inno.Native.ImGui.ImGuiTableInstanceDataPtr,Inno.Native.ImGui.ImGuiTableInstanceDataPtr) 3b6cd79f43fada9ccf8852aa3d94b9464199f2960709cf771e95d7b7705fdaeb -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.Equals(Inno.Native.ImGui.ImGuiTableInstanceDataPtr) 3e32b4f6849c366f51094f21432878d4a165fd84acfb75828c221e4f8843f75a -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.Equals(System.Object) 3b6950f7a7e522717fbaa595c35d4184b9503ec2ba956d0769e8aa1bd8e108c4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.GetHashCode f0ef83a1d7c5070a21cb664bc6df1d98c224b9dca739dfc771f9af6af8f3ea80 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.TableInstanceID 66644ef0423ed1bcd820bf1fa537ee7ba11bc461b596417a61becfaa04dd2051 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.LastOuterHeight 77c648a4c8ccccec90267502ca627757c400dba463a20e5bb3e5d1b4b24a37c5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.LastTopHeadersRowHeight 3aa0b1958a99507d2db35d4e50d89bf5037eb7dfe0f25caa2104af490ca28c68 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.LastFrozenHeight ebe6422850b33db1a49772201632528444cd77f1978fe37a29dfe236091055cb -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.HoveredRowLast 060110bae1f753bed4c91c01aa0763613a97f12f142694fb5366764a3514ca79 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.HoveredRowNext cfe7d57dc61e1a6271c7df074029d700f412d67706981a77c6e7f9cb489abed8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableInstanceDataPtr.Destroy 38c4ddc200ff46c28a4948f99509a8084d6afdf9721c8431841fee2dfb594cde -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b5242260d390f53d"></a>[`public Inno.Native.ImGui.ImGuiTableInstanceData* Inno.Native.ImGui.ImGuiTableInstanceDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16942) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8eb8c30fdd470012"></a>[`public Inno.Native.ImGui.ImGuiTableInstanceDataPtr.ImGuiTableInstanceDataPtr(Inno.Native.ImGui.ImGuiTableInstanceData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16937) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a0d2abccc36f16ae"></a>[`public void Inno.Native.ImGui.ImGuiTableInstanceDataPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16966) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-80f8257d6df54d83"></a>[`public bool Inno.Native.ImGui.ImGuiTableInstanceDataPtr.Equals(Inno.Native.ImGui.ImGuiTableInstanceDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16952) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-32c611e53ec284a2"></a>[`public override bool Inno.Native.ImGui.ImGuiTableInstanceDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16953) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-f00a1f7d80050cf1"></a>[`public override int Inno.Native.ImGui.ImGuiTableInstanceDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16954) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-cb8855290e31d087"></a>[`public static bool Inno.Native.ImGui.ImGuiTableInstanceDataPtr.operator ==(Inno.Native.ImGui.ImGuiTableInstanceDataPtr left, Inno.Native.ImGui.ImGuiTableInstanceDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16950) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-96f994c99d54d15e"></a>[`public static Inno.Native.ImGui.ImGuiTableInstanceDataPtr.implicit operator Inno.Native.ImGui.ImGuiTableInstanceDataPtr(Inno.Native.ImGui.ImGuiTableInstanceData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16948) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2b00339850c1312f"></a>[`public static Inno.Native.ImGui.ImGuiTableInstanceDataPtr.implicit operator Inno.Native.ImGui.ImGuiTableInstanceData*(Inno.Native.ImGui.ImGuiTableInstanceDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16949) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5b11e74d4bc9e676"></a>[`public static bool Inno.Native.ImGui.ImGuiTableInstanceDataPtr.operator !=(Inno.Native.ImGui.ImGuiTableInstanceDataPtr left, Inno.Native.ImGui.ImGuiTableInstanceDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16951) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3a1dbd6132302340"></a>[`public ref int Inno.Native.ImGui.ImGuiTableInstanceDataPtr.HoveredRowLast { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16960) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-320fccdcf54cf568"></a>[`public ref int Inno.Native.ImGui.ImGuiTableInstanceDataPtr.HoveredRowNext { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16961) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0e1489e04fc1bc1c"></a>[`public bool Inno.Native.ImGui.ImGuiTableInstanceDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16943) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e45a33dab2549422"></a>[`public Inno.Native.ImGui.ImGuiTableInstanceData Inno.Native.ImGui.ImGuiTableInstanceDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16946) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dbab97f170efe26a"></a>[`public ref float Inno.Native.ImGui.ImGuiTableInstanceDataPtr.LastFrozenHeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16959) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2132989a47c22116"></a>[`public ref float Inno.Native.ImGui.ImGuiTableInstanceDataPtr.LastOuterHeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16957) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-04a272fc3491b0c7"></a>[`public ref float Inno.Native.ImGui.ImGuiTableInstanceDataPtr.LastTopHeadersRowHeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16958) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4e4b5e11f5fc38af"></a>[`public static Inno.Native.ImGui.ImGuiTableInstanceDataPtr Inno.Native.ImGui.ImGuiTableInstanceDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16944) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f68f707d42f44c17"></a>[`public ref uint Inno.Native.ImGui.ImGuiTableInstanceDataPtr.TableInstanceID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16956) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f5acc1ee74e5ec3a"></a>[`public struct Inno.Native.ImGui.ImGuiTableInstanceDataPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTableInstanceDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16935) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTablePtr</code> · 127 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTablePtr 9936775dea6cf3c5cf5c5fe04f2e7fe595d9399d2586e592a855a4f8a52f0e8d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTablePtr.#ctor(Inno.Native.ImGui.ImGuiTable*) 62c4a5b5ab32f3ece372d481260f37af885baac8833ee37f340652f8320aaab1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTablePtr.Handle cba51beb8d743989a44491771d1dd8152dd290e3b4ac7cdcc9bff725f46d0c48 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsNull 44667a954180cc07d245b7971e81dc3a34b5b58a532f4860d96970e349174b9b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.Null 1a90a6a49e40b367a7f8aaae1da154497f34a9d9c4c0004a50eb41124dd99f90 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.Item(System.Int32) d07d9bb13b03f64a620db3a2855daad23721562044e75c7ac91ebd3994f0c014 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTablePtr.op_Implicit(Inno.Native.ImGui.ImGuiTable*)~Inno.Native.ImGui.ImGuiTablePtr 56187621117497b26cfe98f977c25009d006e49bf96c1fa0387776d724e42c75 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTablePtr.op_Implicit(Inno.Native.ImGui.ImGuiTablePtr)~Inno.Native.ImGui.ImGuiTable* b0a5bf388465fecfe282df6327408451bed0a18ef499dc029575549f6963241a -->
<!-- api: M:Inno.Native.ImGui.ImGuiTablePtr.op_Equality(Inno.Native.ImGui.ImGuiTablePtr,Inno.Native.ImGui.ImGuiTablePtr) 1b641fba91368256d0366d51b5f73d8e7dd334f4542d6fbbb97197a9e9d43e96 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTablePtr.op_Inequality(Inno.Native.ImGui.ImGuiTablePtr,Inno.Native.ImGui.ImGuiTablePtr) 4123213ac6d6915c7ea145d75d71868b206bf056095822c20bff4a4eb4b69fa8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTablePtr.Equals(Inno.Native.ImGui.ImGuiTablePtr) d6e75a494647dcc9c011a95b780399bacefcbc879f644df59c1f9aa8a09d9156 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTablePtr.Equals(System.Object) 13bfb1cbc45b52ea38a91e64b538c307a74c0ee76c094d6c032b98a256509bcb -->
<!-- api: M:Inno.Native.ImGui.ImGuiTablePtr.GetHashCode 811947787cd9834b1be63d9964dfe20f7959652f93e7b69ccd9f2cb727675e1d -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ID f9fa49d34d05eec08acedc52d43f23d50fe914c9e31a7d70260dc63b9ab90c61 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.Flags cece507bed2fba0cb8064bed0edcc39b0bb44e23ae52295fed6e565fe93e3864 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RawData 53efe82ac9ea531a9640e133c60266f5427892457e752fc20f2f6de79fa43347 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.TempData 8639ca9f5659262eac2664afbb4f90726c4d2ad3e420a7cd6694b6b874673164 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.Columns 98273cddc5ab72a45d8bd4a66f96808db75231c807838adc9581964d52aeb0d9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.DisplayOrderToIndex 87320756722630baabe44d0df08669426d7fb7c713a830fd719f1525a11a914e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RowCellData 32919d17af76de6c2f64a3bfa39b9bef42dc3e5143ac79f872e6c836c98f578c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.EnabledMaskByDisplayOrder c680e72996cd9c952cb6034200008cbe89c5b7376097031a42452268a3aea871 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.EnabledMaskByIndex 4e412e25a9dfcd0aa206def86148b4a3e39713e7f1de66b70d1a35266fe0cfa9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.VisibleMaskByIndex 4a5537dfd2ce5bb8a5672bc05ef30727cecc21fb499f2e6c41a49f154d066de9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.SettingsLoadedFlags eea9dbf6c77407d5501d5378e2b31292f8ae5a27cacd44779f9824232573d90a -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.SettingsOffset 64569e347cda1224a653bd487099f73061689444d944d28e82bfda0ec2a75e6b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.LastFrameActive f5f8a756c8808d5d1a905045e986c5bce4b2e6417595fe35c73985360d69dc79 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ColumnsCount b1707aecbf3ba3d530a439a1173ede932126c775f02073a4c3437f673d3f4804 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.CurrentRow 15835da5a3bd1b8334ecd633032dc69d4ec6b55d953083ca3d4a677fe3adf9c4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.CurrentColumn 86b48284c815c8c637267815fd51276100ea765571e32eb0a3caec5d03879315 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.InstanceCurrent afebe31cdf4637d369afd14441bb120eaca5ee23243e6f4650055f74f8c7951b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.InstanceInteracted 741fde7f0a8d21aaae3047e0f0e363781d939dca7c86000d134ac1e5ec5c40b9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RowPosY1 736cfa9a60c24fb78e37cae3bd746dd536b6848b95b1bb8931002458fc1ce0b2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RowPosY2 60437ca8a6366efd80816f6f6269636ba9829c829b584b4193266f221c48a942 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RowMinHeight 231141aed85dd88a02bfee9a686584204adfbb5a42d1366cd38125954d2ca3a7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RowCellPaddingY a727b479788a6ebfc8fee1ca188d2738ff717d59df0dfa6133fb8fa9e4ccbb33 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RowTextBaseline 6925cff8f73736b96a850d19e09e407a3614064fd76a24b14b3b2a8088e054bb -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RowIndentOffsetX 38d8e1935e33a563a0226c488873f04a4c051c63c2cedf8f77f8e93adc36cad4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RowFlags 9c4309a5114fd6fd49a13161339046cc242e806001e83303d3f331188a40f576 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.LastRowFlags 757316118cf04422c7efeb6900af83d67442ec09db92bce0bd439a61a9d28a47 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RowBgColorCounter 37c95e5c14598865d82287a2e892861f63d747f55cf9e768b2c31b7f8d5477db -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RowBgColor 2e5bcf20ee9d9da08aa446226862c7fb31a11b09ed9ad297273fdd08fade26dd -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.BorderColorStrong 920777c7890058372b009ade5fe34f28d7c3373e544f9a33bb8168046fffb183 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.BorderColorLight c08f25075e1b4a61b4d1ecd4bc82b1804a37fe02420230e52e3488d68dbcbd58 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.BorderX1 3dbaed72e0d70fee61e16f2e5dd7f0decd2957e0b852e7991146c78fc38d661c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.BorderX2 b850748dc82f6c63d6ec679d85f914529e0e05f60e7ecf78ffa4017d08c69e6b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.HostIndentX c5c7b9ca96d632ee560292874f586560c5e9b485204c8277f473a90ec724ce6d -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.MinColumnWidth 97ff4f780c068ed5933a18f7df037f926fb50cb8a454d2d590513084d63a0cc7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.OuterPaddingX cc2073846d08708b3aa726bac0f625e767fff9be21a54b7412f72af1f791f183 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.CellPaddingX f5df4774e969f21554004903cbb7e22b15ecb541855a49cf7d9a06c6be2bd59e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.CellSpacingX1 586843c80354ac59a1b2743a58553ba2aa39dc3fa847bbd2cdc4f6e7afa1d0a2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.CellSpacingX2 d372c22b50fe7872f12bdccf6a622f6fd9dadb84b7521540e26af459933f5a15 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.InnerWidth ac3323637aa083bba4c16f12842624b941b2b6711e33d7afc22b5bd5269e7214 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ColumnsGivenWidth 6f8be3f418b91c4d2e3f0735b93a8ca8d96faac37b0952e4281e59077667e960 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ColumnsAutoFitWidth 43aacf050f9c0805ceb26bba46ecbab50d98ec36bb6409b260f8fb9258694fe2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ColumnsStretchSumWeights cf9002ae008db29a7ec124d08bfb6d230d5a8be357df5ad00d7e9d85b6a7a3de -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ResizedColumnNextWidth 79363bdccc95e94ec80cd1d6bfbed4000ad2d0f084017bdcbf38dddf09b8bd0b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ResizeLockMinContentsX2 ed368954fe9e143df206d3c3b9c9ede29b514a6f5c3dac98cc553dd60bae036a -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RefScale c6246606ff91b9f7e91f0c9d5b23b90e7cfee541897c5c78f345cada73964d7f -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.AngledHeadersHeight 45192388de2afb2ed0178be7ee0b151dd14b610619028d8efe3afc2b59efa0ed -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.AngledHeadersSlope f15e233b09d5f48543a530eee5bb64db2d918ae3a0cd27d7d5b9f5482ced81a4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.OuterRect a59506b41c4dd99bb2f1cc2cc6ac57bb214cfd8c2b3484e241813f049fc6f62c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.InnerRect ad0503fe92af3038727d3f7670e2db18177ec9a47a90af936bdefa2aa77ad3ec -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.WorkRect 8a77226486ec0f8e693a92c94bc8dbbec5ddfbbc90c27e228bcfea63c35876f1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.InnerClipRect a640ccf0db38da3abe1207757cc41ae44c521a0b5aac1ac581607adf64edd33e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.BgClipRect 67f877fa8e9da769c8cabc0636c6c028f2b7365a08c2564e4ed2da98366a28ec -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.Bg0ClipRectForDrawCmd c5d69b6483ccd9222e21c29f3a501da072640df7c7e2803f0c5adaabd5f6a845 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.Bg2ClipRectForDrawCmd 713275fdc179a40382126e0dc1002fa01ee438dd4098f8ec7595ffb378118736 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.HostClipRect 499055c9914fb3009faccb0544f916026f63ad587c3874d31f2267a8c8e32838 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.HostBackupInnerClipRect e921648a3ce8e6ae3d78a136e03e85b0e7c054950cabe607ff9990f8b890b703 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.OuterWindow 8be7df17832631c2bdeb37f5d9bd26b649813b43dde61d5c83d40982addd2ee7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.InnerWindow b8da68a00aaf2c94e29b75278d4912701d21013a5d97aca98e9ec9e4b9b21920 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ColumnsNames cd42093fc10ffcfaf62bbf5a968ece60adb66a91cf43236c020701599dd8a77b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.DrawSplitter 96070cedaa4ce11d0394b9616d476f1db161e01ebdb66323ac854ffbbe6c96ef -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.InstanceDataFirst 71171074c36665b290876bbbf7c14557b5c4223a66f0ad8d9480f330e803b2bf -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.InstanceDataExtra 84d97bb017d3d16598e7e0b945e1a9b54a68b7e1c09fc66e62a4214efa90e745 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.SortSpecsSingle b9b6845b3f94806d678eee31503296b2635ee9b888d8dca0a9e7b4d5eb73c267 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.SortSpecsMulti dc8ab54f5c50a676cfdce51c9601be5d4fb407c710fcf7bad90c447bf9320424 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.SortSpecs 6fb4227ba9994c1c286259834d24def9ebaf5faeee935d0867b0208302f21578 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.SortSpecsCount 03214b10252a1195a483c19aea3858f9e4894c412c86b5fffd459221a39adb9e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ColumnsEnabledCount cefb6fb128435487634d0561b2bece15a9e7375580cdd31d353295defac666e7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ColumnsEnabledFixedCount 57984b4cc0ec3d72b4b75d99081246981b0dacd372341fb2c840b0a020f64e0a -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.DeclColumnsCount 587a5f6845ce3b40cc7889b6b9cbe29a2ea1ed8f84af8b0c93f631696df9d43d -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.AngledHeadersCount 5d80a2e1aedc7489ad21b7f98400e641332f4025bdaf6d2c434f9d67e2132301 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.HoveredColumnBody 4f2e8f0fe5d4274bdfa451e1dd36a3be5f4f28ffbfe39ca2650a81a73db7615d -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.HoveredColumnBorder aad2dae369268084d05982ab0a3d3bc4c2db4251bb03cb4859ae759756b406e1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.HighlightColumnHeader 6d817223800718ae6379bb84bb33c16d0c964df9bee177efd53df3a846b8579c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.AutoFitSingleColumn 4915c823b3e5c1282b18b663c3271dd677a6ca5180e88847e6162de248381a3a -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ResizedColumn ae741f28c78c3d0831c9ad5d78fe08dc50f47c9a1630d182453ee494a77e9b27 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.LastResizedColumn 9ff621524d85d3f4ae08a542ec79cd8f123e9709320993009d9d98c23f89442e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.HeldHeaderColumn ec9487d83c439361a6d79d4431b66299917e3128b7951d3e7cb4051920648642 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ReorderColumn 0ba20366ee04f00a07d1773afae62386e02e3b0ac20898987af9ba35b58e51af -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ReorderColumnDir 48268daf5a1a7a6b7219144c775e0f33ff7f2de6346d5a2f8198894624035a7c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.LeftMostEnabledColumn d7d4bebf10310f2b35742a6924cdb3fd4a8ed477d610d93c8870ced7259be03a -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RightMostEnabledColumn 23c81a0f3d7c8f5a8175dffd48b522f47142a0085ce51d669e44f30eec79e953 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.LeftMostStretchedColumn b6d990b8a72c573c52b9a4371822b44beb761601417ae0d44bc29ad6d52587a1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RightMostStretchedColumn 7bf5a247a8eabdd762f3a6721447c7422274b96a54915eed80463cb8f3bd4c53 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.ContextPopupColumn fa73fa3d1ba39115467b5fc17e987b55c7c1bbd1f5895af2c520bb7b13884a44 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.FreezeRowsRequest a8305031880d369c95cbfe836d3cdd42e499d45d320f22ccb7260fa977bfc2ff -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.FreezeRowsCount cdd7f6eb56813393620176d7ecff7393d0603ddea7783036ba3da38d54a1fd05 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.FreezeColumnsRequest 0ee9af456258aa478e0c149cccae67ebc91c885b6248b958c88e750c5dfcf8b8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.FreezeColumnsCount f1723b51c67b548455218816e65374645a92812c1c10313fa44e29b065dc3663 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.RowCellDataCurrent 48a4dfe1b1d060b77d2b50bcd1c80e643a354bd34f6938f8a424e56aac656394 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.DummyDrawChannel 485f1a8d963b999f880331b98b79002f759da3b058165d11bedaa5bf10883315 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.Bg2DrawChannelCurrent ac0808508a106f5174d40fca5cdf334749fac626ef22edd3adf6fe0ac1aa93ba -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.Bg2DrawChannelUnfrozen e90592f1f5f4d98b584f0d6a2db95b3d43ba112d096bbe5c6926258cb778a0e6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.NavLayer b70b686f7e5e25aa7e81bcf192d763bf9d68f09926eb2fda42400819ce2edef6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsLayoutLocked 6dfef180894637fde01f3200f138e85b7e5b2a35d90e2f98a07d4fdcaa7cf375 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsInsideRow 14f71c879e113971ee1240a34d262ce91c6733eb779f78b57f50ead26da69eeb -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsInitializing c477117e76e8dab834eeb46495d5027e7a151e88f3930c41e621b306fd027fbc -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsSortSpecsDirty e32a1d6fac982b491db38f376133790c06c815a92647138233f9faf9a833da84 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsUsingHeaders c4e6247930164de3a7b458c8f0d7c9b42f13279b93a022c158ae981effb3cbc5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsContextPopupOpen 838a2894d738f7eaf871ca345740c18fdf5f9d8769437479b47b7f21b528788e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.DisableDefaultContextMenu 4d1930e76d33dbda9b19214b02549578057a539ce6b38a5f2ba697dd3205acc7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsSettingsRequestLoad 913becf785093c2404f03f3aa2761b1ee188d469424506ecabab923f549868b7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsSettingsDirty 01fb5dec1e22372333c735f48dc1fa5ff9d92b896e98a9e44b68562644ad8976 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsDefaultDisplayOrder 4226dad9f97768252f38f8341a680b90f55319f56d4f1543f65392d19d500b14 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsResetAllRequest d2b9df82a93db20d9fe62b5844b720fa342684a4e8280f1f7ee769aa2b885575 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsResetDisplayOrderRequest 59339107ebd28d707ad9acae2496bd5207ccfd6ec2235d1b7567d8e184ace889 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsUnfrozenRows acd12bcb6c43f99d27e6f9be1288f9ab967e9f0b926cb2d4d81b0a82db442936 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsDefaultSizingPolicy e1437abcfba38d6f887d44af12324d66ed0426cc29f07e7b28208ab75f11335c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsActiveIdAliveBeforeTable c2ef9f31423e27cf80014afa5fcfeba6ab4f13ec9ec64861300720981121b729 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.IsActiveIdInTable 340b73c030943c0b402aa417d5bb2380e284aaeca0489aaf44a31716826d28ac -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.HasScrollbarYCurr 32209b0f3cc8a0711aa426f548345c08c7aba91d0197953aea1a395bb8d2045e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.HasScrollbarYPrev 865cfac97711d3fbbe63c1a481cf0606adfd0b7389160988332f6e5096ef9668 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.MemoryCompacted adcaf2405e1d6fa56a77aedacdefb0dc973cb8dd7bc68a5b01584f62b24c136d -->
<!-- api: P:Inno.Native.ImGui.ImGuiTablePtr.HostSkipItems 771be7d3f87de4c8edb9c8f911569b0ffc604b5e3e0b34a31288e868dc259ea2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTablePtr.Destroy 291b282f183f7ce546185845ee7c01a6604f429c3ecc803761b963757dfbcc5e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d8b7ecb196b33670"></a>[`public Inno.Native.ImGui.ImGuiTable* Inno.Native.ImGui.ImGuiTablePtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16980) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-38b886e3b09fdc69"></a>[`public Inno.Native.ImGui.ImGuiTablePtr.ImGuiTablePtr(Inno.Native.ImGui.ImGuiTable* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16975) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e3a978ac56b9a7ca"></a>[`public void Inno.Native.ImGui.ImGuiTablePtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17111) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8b814d2fac3efece"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.Equals(Inno.Native.ImGui.ImGuiTablePtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16990) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-2922dbed9223f5eb"></a>[`public override bool Inno.Native.ImGui.ImGuiTablePtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16991) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-4705fbfe2f2407c7"></a>[`public override int Inno.Native.ImGui.ImGuiTablePtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16992) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-8a396e12ff3f089b"></a>[`public static bool Inno.Native.ImGui.ImGuiTablePtr.operator ==(Inno.Native.ImGui.ImGuiTablePtr left, Inno.Native.ImGui.ImGuiTablePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16988) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bf0bb8127e2dff51"></a>[`public static Inno.Native.ImGui.ImGuiTablePtr.implicit operator Inno.Native.ImGui.ImGuiTablePtr(Inno.Native.ImGui.ImGuiTable* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16986) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0b6e61b25567047e"></a>[`public static Inno.Native.ImGui.ImGuiTablePtr.implicit operator Inno.Native.ImGui.ImGuiTable*(Inno.Native.ImGui.ImGuiTablePtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16987) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fbf11bb7ecc36f27"></a>[`public static bool Inno.Native.ImGui.ImGuiTablePtr.operator !=(Inno.Native.ImGui.ImGuiTablePtr left, Inno.Native.ImGui.ImGuiTablePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16989) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d8f1e2bc3e88176f"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.AngledHeadersCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17063) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bae2376cbc339462"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.AngledHeadersHeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17039) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-79b82b757432ee9e"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.AngledHeadersSlope { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17040) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f6adc11950d24a3a"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.AutoFitSingleColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17067) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-34f2e2c575879450"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTablePtr.Bg0ClipRectForDrawCmd { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17046) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-771de758a578065e"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTablePtr.Bg2ClipRectForDrawCmd { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17047) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1fad17e3fce7eca7"></a>[`public ref ushort Inno.Native.ImGui.ImGuiTablePtr.Bg2DrawChannelCurrent { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17084) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4c9a3350ecebe459"></a>[`public ref ushort Inno.Native.ImGui.ImGuiTablePtr.Bg2DrawChannelUnfrozen { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17085) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6218e10f9870d155"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTablePtr.BgClipRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17045) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-34525b20e06e8a9a"></a>[`public ref uint Inno.Native.ImGui.ImGuiTablePtr.BorderColorLight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17023) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a6a3b22cc32fc2e5"></a>[`public ref uint Inno.Native.ImGui.ImGuiTablePtr.BorderColorStrong { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17022) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cbf65b5b118d439c"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.BorderX1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17024) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6ecfd92baf2a04cc"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.BorderX2 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17025) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-55bebb50f16f24c9"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.CellPaddingX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17029) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-77c0a1a830bf1f5b"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.CellSpacingX1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17030) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e771caca71ee49ee"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.CellSpacingX2 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17031) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9ecd09242f497cd3"></a>[`public ref Inno.Native.ImGui.ImSpanImGuiTableColumn Inno.Native.ImGui.ImGuiTablePtr.Columns { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16998) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c7e7438c3873c2be"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.ColumnsAutoFitWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17034) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-05a75a35d29ef9bb"></a>[`public ref int Inno.Native.ImGui.ImGuiTablePtr.ColumnsCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17007) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-19a5d91a5ce80516"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.ColumnsEnabledCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17060) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-54defa01230c38d8"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.ColumnsEnabledFixedCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17061) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-39a789ef60ff45f1"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.ColumnsGivenWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17033) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d79a1aa5192bb078"></a>[`public ref Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiTablePtr.ColumnsNames { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17052) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a74ad6d9051d4da6"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.ColumnsStretchSumWeights { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17035) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-05bb3115a93993b5"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.ContextPopupColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17077) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4585cb3e2406703d"></a>[`public ref int Inno.Native.ImGui.ImGuiTablePtr.CurrentColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17009) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2906c39f50d8a75d"></a>[`public ref int Inno.Native.ImGui.ImGuiTablePtr.CurrentRow { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17008) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0d9b1597fe1681c3"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.DeclColumnsCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17062) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b98d9a859577f75a"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.DisableDefaultContextMenu { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17093) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8322bf08ed40a2ab"></a>[`public ref Inno.Native.ImGui.ImSpanImGuiTableColumnIdx Inno.Native.ImGui.ImGuiTablePtr.DisplayOrderToIndex { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16999) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2804cf2877ee9937"></a>[`public ref Inno.Native.ImGui.ImDrawListSplitterPtr Inno.Native.ImGui.ImGuiTablePtr.DrawSplitter { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17053) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f105b102f46682cc"></a>[`public ref ushort Inno.Native.ImGui.ImGuiTablePtr.DummyDrawChannel { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17083) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-34ba7cf1bc29ea03"></a>[`public uint* Inno.Native.ImGui.ImGuiTablePtr.EnabledMaskByDisplayOrder { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17001) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5bc1ea1c9f97c867"></a>[`public uint* Inno.Native.ImGui.ImGuiTablePtr.EnabledMaskByIndex { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17002) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-25750c85277000f2"></a>[`public ref Inno.Native.ImGui.ImGuiTableFlags Inno.Native.ImGui.ImGuiTablePtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16995) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-70855e295928d718"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.FreezeColumnsCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17081) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a9b2f83073fdc570"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.FreezeColumnsRequest { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17080) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-06f1290d4a4342c7"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.FreezeRowsCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17079) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3cde1b13e81e2fd5"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.FreezeRowsRequest { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17078) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7048e95295c092f8"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.HasScrollbarYCurr { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17103) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8193b71f4dce47d8"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.HasScrollbarYPrev { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17104) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-19e4e1eeb110235d"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.HeldHeaderColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17070) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-88aa9599724681b9"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.HighlightColumnHeader { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17066) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-39d7bfdfbb63a9bc"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTablePtr.HostBackupInnerClipRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17049) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6687e9b3641f886d"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTablePtr.HostClipRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17048) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bf36a11b7aab1269"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.HostIndentX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17026) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9ed58690d9a8d5db"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.HostSkipItems { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17106) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-07874b2c7d0d27fe"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.HoveredColumnBody { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17064) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1e9574b3cc6ade9a"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.HoveredColumnBorder { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17065) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9d0848db5ed1f7fe"></a>[`public ref uint Inno.Native.ImGui.ImGuiTablePtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16994) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d53cf9a8838a5ad2"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTablePtr.InnerClipRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17044) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0e6eefbaa32cf83d"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTablePtr.InnerRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17042) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0c2f970b83cc43b5"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.InnerWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17032) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c47e86194b74127e"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiTablePtr.InnerWindow { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17051) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-90108e4626f7c93a"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.InstanceCurrent { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17010) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-18c0c3b3c13d08bc"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTableInstanceData> Inno.Native.ImGui.ImGuiTablePtr.InstanceDataExtra { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17055) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-594d2b00d3573144"></a>[`public ref Inno.Native.ImGui.ImGuiTableInstanceData Inno.Native.ImGui.ImGuiTablePtr.InstanceDataFirst { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17054) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-820f7270cdb381d9"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.InstanceInteracted { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17011) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-38e3a90c6d6faf78"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsActiveIdAliveBeforeTable { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17101) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a18d3f69ed0cbfbe"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsActiveIdInTable { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17102) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-61cacbd224f2bae6"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsContextPopupOpen { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17092) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f672bdc4a85ca665"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsDefaultDisplayOrder { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17096) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fdd67b772ba41428"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsDefaultSizingPolicy { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17100) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7b99e7381b291c63"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsInitializing { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17089) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-56efdc1339f92f07"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsInsideRow { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17088) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-09e01f8fbec01951"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsLayoutLocked { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17087) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ca413cc469ba7e4e"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16981) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-721094ecbcf713ce"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsResetAllRequest { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17097) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-24b20cc289aa701d"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsResetDisplayOrderRequest { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17098) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-00546ecedde0abee"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsSettingsDirty { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17095) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-182e9c2c90a2ba92"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsSettingsRequestLoad { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17094) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-53101057d5fcbb34"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsSortSpecsDirty { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17090) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ee70ed232ec24978"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsUnfrozenRows { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17099) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7217f2dda447968c"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.IsUsingHeaders { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17091) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-98f283ed5635faa1"></a>[`public Inno.Native.ImGui.ImGuiTable Inno.Native.ImGui.ImGuiTablePtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16984) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ac1c85501c83cce"></a>[`public ref int Inno.Native.ImGui.ImGuiTablePtr.LastFrameActive { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17006) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-343b7a0042cc5be2"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.LastResizedColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17069) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e9d8920022180c77"></a>[`public Inno.Native.ImGui.ImGuiTableRowFlags Inno.Native.ImGui.ImGuiTablePtr.LastRowFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17019) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d773afd05e51f501"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.LeftMostEnabledColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17073) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8263637c9865401a"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.LeftMostStretchedColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17075) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a60813a645eadcfc"></a>[`public bool Inno.Native.ImGui.ImGuiTablePtr.MemoryCompacted { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17105) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1cc4598e34aa093c"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.MinColumnWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17027) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-460c9a56d30e0503"></a>[`public ref byte Inno.Native.ImGui.ImGuiTablePtr.NavLayer { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17086) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a623f1e04ecc3cf2"></a>[`public static Inno.Native.ImGui.ImGuiTablePtr Inno.Native.ImGui.ImGuiTablePtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16982) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8d4a0f3c347125a4"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.OuterPaddingX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17028) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7f3a698e98d0a6aa"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTablePtr.OuterRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17041) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5e0f2d5eeeb47eb8"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiTablePtr.OuterWindow { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17050) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7b4f01a205eef4c4"></a>[`public void* Inno.Native.ImGui.ImGuiTablePtr.RawData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16996) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-38991f89dd204cf8"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.RefScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17038) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2490a4d7bf023b97"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.ReorderColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17071) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6c441dc8a6cb5df4"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.ReorderColumnDir { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17072) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dc6d231db00f1bfa"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.ResizedColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17068) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-024b68cb57b1706d"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.ResizedColumnNextWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17036) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0c1ddf119971083d"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.ResizeLockMinContentsX2 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17037) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-293ccc145748ff65"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.RightMostEnabledColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17074) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b35b19c2c9004bbf"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.RightMostStretchedColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17076) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d5ddffeafc2b047c"></a>[`public System.Span<uint> Inno.Native.ImGui.ImGuiTablePtr.RowBgColor { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17021) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5fd0aff3a28c28b1"></a>[`public ref int Inno.Native.ImGui.ImGuiTablePtr.RowBgColorCounter { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17020) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0e69f7e67c8b8768"></a>[`public ref Inno.Native.ImGui.ImSpanImGuiTableCellData Inno.Native.ImGui.ImGuiTablePtr.RowCellData { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17000) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-780cd81f6401925d"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.RowCellDataCurrent { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17082) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9cbb96d542582593"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.RowCellPaddingY { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17015) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0e3f5d72d3bf3e42"></a>[`public Inno.Native.ImGui.ImGuiTableRowFlags Inno.Native.ImGui.ImGuiTablePtr.RowFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17018) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-782025a1903bc82c"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.RowIndentOffsetX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17017) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-11c8e1f518c23713"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.RowMinHeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17014) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8a2a1d0ce771d491"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.RowPosY1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17012) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-91fe168ddf36132f"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.RowPosY2 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17013) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9630c96670dfd194"></a>[`public ref float Inno.Native.ImGui.ImGuiTablePtr.RowTextBaseline { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17016) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f09dc660706b9f90"></a>[`public ref Inno.Native.ImGui.ImGuiTableFlags Inno.Native.ImGui.ImGuiTablePtr.SettingsLoadedFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17004) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6ed271e323721346"></a>[`public ref int Inno.Native.ImGui.ImGuiTablePtr.SettingsOffset { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17005) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e10e32c444e1bf2b"></a>[`public ref Inno.Native.ImGui.ImGuiTableSortSpecs Inno.Native.ImGui.ImGuiTablePtr.SortSpecs { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17058) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-760df13fa6451922"></a>[`public ref short Inno.Native.ImGui.ImGuiTablePtr.SortSpecsCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17059) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-885604f336d3e3c2"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTableColumnSortSpecs> Inno.Native.ImGui.ImGuiTablePtr.SortSpecsMulti { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17057) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0aafc2ecb6893427"></a>[`public ref Inno.Native.ImGui.ImGuiTableColumnSortSpecs Inno.Native.ImGui.ImGuiTablePtr.SortSpecsSingle { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17056) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0bde8e1added6aad"></a>[`public ref Inno.Native.ImGui.ImGuiTableTempDataPtr Inno.Native.ImGui.ImGuiTablePtr.TempData { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16997) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fdf426dddf31409e"></a>[`public uint* Inno.Native.ImGui.ImGuiTablePtr.VisibleMaskByIndex { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17003) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1db7a4d77b85e087"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTablePtr.WorkRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17043) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8cf0dd0990d0ea81"></a>[`public struct Inno.Native.ImGui.ImGuiTablePtr : System.IEquatable<Inno.Native.ImGui.ImGuiTablePtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16973) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableRowFlags</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableRowFlags 87d9068c8a279b0645830b8dbe006d45021671d5c22f9a774d7daf7a9cf07f8d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableRowFlags.None 63227798f2d4f49821e27d1a9cf419ad20e1767dc95fe7ea3cfa5f129a20ac41 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableRowFlags.Headers 3659dd34302842288137e0c18cb6ff00a1ee850bf27a3ed71214d221c4c53be8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-209b51ae48a7a728"></a>[`Inno.Native.ImGui.ImGuiTableRowFlags.Headers = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1063) | 当前常量或枚举值：`1`。 | — |
| <a id="api-21a5b41c3890513b"></a>[`Inno.Native.ImGui.ImGuiTableRowFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1062) | 当前常量或枚举值：`0`。 | — |
| <a id="api-760b470e59180c41"></a>[`public enum Inno.Native.ImGui.ImGuiTableRowFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1060) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableSettings</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableSettings 48a443153e82eb3bc558d54ccf7079bc189ccbf3364cede5c1da05a00bb23269 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSettings.ID 32fa19346b05650550df121fca8df3ba4be81ea387654ca7422e92d8cbb617f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSettings.SaveFlags aba7aa8e55e7dc896da852159ca03a8477800cbb171da464a1543cce85385885 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSettings.RefScale 78e4d074874313d80b3a68836c2b1ff98fe0eadcf8bd4c0d7a1b043311d653f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSettings.ColumnsCount 9212b5eb37a97dfa294c93545088e159a9a1e6edae62423de3b14f2b94610c2a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSettings.ColumnsCountMax ee0ad85ca046889551bc6be0b4ecf55785a8161ff229b1d2df465f535c222fca -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSettings.WantApply 213b88510c44c1d63f5b005fe7ef0df876c8ffabb154f6ff9544d34e8666acd7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSettings.#ctor(System.UInt32,Inno.Native.ImGui.ImGuiTableFlags,System.Single,System.Int16,System.Int16,System.Byte) 52640f2e6e45ff44c551ec9b071a1e0d68476b3817436e896c4facb6e8b6e886 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSettings.Destroy e9f76b35c9d09e7bd5ed97354f55562689a572d5df61f4b416538811fde4685d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSettings.GetColumnSettings 074a22c38bd46db2c1deff6dcb58c54b12c52437186a87218cc2a2add268d99e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-532e0e458d30c59f"></a>[`public short Inno.Native.ImGui.ImGuiTableSettings.ColumnsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10270) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-91c2a3a3c6274212"></a>[`public short Inno.Native.ImGui.ImGuiTableSettings.ColumnsCountMax`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10271) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9e6ad75d20981b7c"></a>[`public uint Inno.Native.ImGui.ImGuiTableSettings.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10267) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-283b506092e40c19"></a>[`public float Inno.Native.ImGui.ImGuiTableSettings.RefScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10269) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c34c36d1535b21e0"></a>[`public Inno.Native.ImGui.ImGuiTableFlags Inno.Native.ImGui.ImGuiTableSettings.SaveFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10268) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-255e97a3e36ccff7"></a>[`public byte Inno.Native.ImGui.ImGuiTableSettings.WantApply`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10272) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f0ebc630d30313ae"></a>[`public Inno.Native.ImGui.ImGuiTableSettings.ImGuiTableSettings(uint iD = 0, Inno.Native.ImGui.ImGuiTableFlags saveFlags = Inno.Native.ImGui.ImGuiTableFlags.None, float refScale = 0, short columnsCount = 0, short columnsCountMax = 0, byte wantApply = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10273) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1facae1459f4a66e"></a>[`public void Inno.Native.ImGui.ImGuiTableSettings.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67645) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f1e73055195e33f5"></a>[`public Inno.Native.ImGui.ImGuiTableColumnSettings* Inno.Native.ImGui.ImGuiTableSettings.GetColumnSettings()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67656) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7f30e4c75b7603de"></a>[`public struct Inno.Native.ImGui.ImGuiTableSettings`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10265) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableSettingsPtr</code> · 21 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableSettingsPtr 73e56eb88e536d34b835c2a57660684e067091a7f6f4708fa441941cef766a06 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSettingsPtr.#ctor(Inno.Native.ImGui.ImGuiTableSettings*) d982153e8cbf390c4589ec394eba52cb756950f4b560fd009cfd21bd03b0908b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSettingsPtr.Handle 1baf43d349a7274161cccdf27a7a4ae6bfa3abba5af43d582db68e12d016b059 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSettingsPtr.IsNull 72c7f6a0f67b7fbb9edfd2cf7148bd786e184f5b3eba0968b2162ed829470030 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSettingsPtr.Null 95f763de077e5b515c0165522e7bac0724b4ed9e2458d21f6f54bc1b80f56610 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSettingsPtr.Item(System.Int32) 00244d0cbef92073bc41c116e1a7fc629da816376616cd90d3bd08bc59acd3f8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSettingsPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableSettings*)~Inno.Native.ImGui.ImGuiTableSettingsPtr 0227012f67d66421b2a580d84b0dd5a3ec16db938474fa8e546f310928aace9a -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSettingsPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableSettingsPtr)~Inno.Native.ImGui.ImGuiTableSettings* 5c9479d4ab5b66b2cf85fb115600b56de9051bfd555b26faf0666665a34d1922 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSettingsPtr.op_Equality(Inno.Native.ImGui.ImGuiTableSettingsPtr,Inno.Native.ImGui.ImGuiTableSettingsPtr) b668e300e912b199c2ae768fe882a834a763d88c759a414041545382fd822793 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSettingsPtr.op_Inequality(Inno.Native.ImGui.ImGuiTableSettingsPtr,Inno.Native.ImGui.ImGuiTableSettingsPtr) 7115bc402a74bb622a674e1c9e26c68e128b1ae72c2fdc1b1c9033b05996e370 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSettingsPtr.Equals(Inno.Native.ImGui.ImGuiTableSettingsPtr) 65c9e430d717e9b32c1ef2d64215f79c808cc3959e39237bdbb5f246b2df75c9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSettingsPtr.Equals(System.Object) b42b2a4dd89627f6b5873550cab51795acb8aa231f750a3efb886427c16b3403 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSettingsPtr.GetHashCode 4c912b9356f58f43fec0a6626cba152cd1b79cd38b866c9467077088eb711dba -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSettingsPtr.ID 8e0de61ccbd48ab4dd3aa4b367cde9318abcb2aac860699e28a23e47e615bc22 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSettingsPtr.SaveFlags 3ae68f614850abbc42db43b5e1de4f3f96424f16440c5fba9ad160c3e8e11fc5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSettingsPtr.RefScale cd415ab8c0791ace841bb8b5ba2c4a772982d86254beec66373c2661e3a13d4b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSettingsPtr.ColumnsCount cd8ee45856a6098a2e83b7f40a24b8ac695e22aa3f3a406a112ec3c7f0ffd918 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSettingsPtr.ColumnsCountMax a838b829e884f938430adc1ca95d84409e565d5f1453fe1f037430f5cf7abb7a -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSettingsPtr.WantApply 54898834217cd804dbc5a1df31cd54ea69e96576010f76bb9ef43a05cfc29b43 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSettingsPtr.Destroy 7971f17a476625f63740ea3723e421c59dc06c2b6a5403b633bf17d3fb274246 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSettingsPtr.GetColumnSettings a07176d324009f3ad81ea51339352e07f1a782d4378cf5ce585fecd46f780ad8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1f09a804df17ee7f"></a>[`public Inno.Native.ImGui.ImGuiTableSettings* Inno.Native.ImGui.ImGuiTableSettingsPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17125) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e102e9228d410a7c"></a>[`public Inno.Native.ImGui.ImGuiTableSettingsPtr.ImGuiTableSettingsPtr(Inno.Native.ImGui.ImGuiTableSettings* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17120) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6b1a662c64743d2a"></a>[`public void Inno.Native.ImGui.ImGuiTableSettingsPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17149) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-090c0811b6f62594"></a>[`public bool Inno.Native.ImGui.ImGuiTableSettingsPtr.Equals(Inno.Native.ImGui.ImGuiTableSettingsPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17135) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-f91825a202eb7d9b"></a>[`public override bool Inno.Native.ImGui.ImGuiTableSettingsPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17136) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-70881ce50a728185"></a>[`public Inno.Native.ImGui.ImGuiTableColumnSettingsPtr Inno.Native.ImGui.ImGuiTableSettingsPtr.GetColumnSettings()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17157) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3036acf30bc35ee9"></a>[`public override int Inno.Native.ImGui.ImGuiTableSettingsPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17137) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-efaa01a763ccc376"></a>[`public static bool Inno.Native.ImGui.ImGuiTableSettingsPtr.operator ==(Inno.Native.ImGui.ImGuiTableSettingsPtr left, Inno.Native.ImGui.ImGuiTableSettingsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17133) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0e3de2c09de4ebc8"></a>[`public static Inno.Native.ImGui.ImGuiTableSettingsPtr.implicit operator Inno.Native.ImGui.ImGuiTableSettingsPtr(Inno.Native.ImGui.ImGuiTableSettings* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17131) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b98406760f672542"></a>[`public static Inno.Native.ImGui.ImGuiTableSettingsPtr.implicit operator Inno.Native.ImGui.ImGuiTableSettings*(Inno.Native.ImGui.ImGuiTableSettingsPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17132) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f86dfeae9ab9dcba"></a>[`public static bool Inno.Native.ImGui.ImGuiTableSettingsPtr.operator !=(Inno.Native.ImGui.ImGuiTableSettingsPtr left, Inno.Native.ImGui.ImGuiTableSettingsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17134) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8fcc234ad4533034"></a>[`public ref short Inno.Native.ImGui.ImGuiTableSettingsPtr.ColumnsCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17142) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e36920823d0b10c5"></a>[`public ref short Inno.Native.ImGui.ImGuiTableSettingsPtr.ColumnsCountMax { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17143) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ef23c228efd03067"></a>[`public ref uint Inno.Native.ImGui.ImGuiTableSettingsPtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17139) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-39355122c1998079"></a>[`public bool Inno.Native.ImGui.ImGuiTableSettingsPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17126) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-432d9cc56daab07b"></a>[`public Inno.Native.ImGui.ImGuiTableSettings Inno.Native.ImGui.ImGuiTableSettingsPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17129) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a4a5510b57a64a48"></a>[`public static Inno.Native.ImGui.ImGuiTableSettingsPtr Inno.Native.ImGui.ImGuiTableSettingsPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17127) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0847d235e710eac4"></a>[`public ref float Inno.Native.ImGui.ImGuiTableSettingsPtr.RefScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17141) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4968f806301cc868"></a>[`public ref Inno.Native.ImGui.ImGuiTableFlags Inno.Native.ImGui.ImGuiTableSettingsPtr.SaveFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17140) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-589312568f7d43fc"></a>[`public bool Inno.Native.ImGui.ImGuiTableSettingsPtr.WantApply { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17144) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d57c9c462b8d53ee"></a>[`public struct Inno.Native.ImGui.ImGuiTableSettingsPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTableSettingsPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17118) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableSortSpecs</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableSortSpecs 21e679a03b64d00ec8c969a30542ee161b80f99e863228b2311fde478469d6ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSortSpecs.Specs 4f0083b2afb17c7a5cc2dbcaa6638453b1a6d20ff63f9cf8d5c387f23b2c746e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSortSpecs.SpecsCount 7903cdebda5352999dfd7c2c3413a2e707fd0b79a7eb4287f3e8f9b7e38f4e2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSortSpecs.SpecsDirty 9732936ab691eb8cc1ffe0ee5020c3e11ce33b6d65a0bff7de23362db487356e -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSortSpecs.#ctor(Inno.Native.ImGui.ImGuiTableColumnSortSpecs*,System.Int32,System.Byte) 69abe0f737346e266e69d52702c3d852ec7d7bb5672b247fe5dd1d7b87fdc27c -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSortSpecs.Destroy 8400ecdcf3aad7b8544fd1b97b1a15d7ca0d862408d91897d1a585f5bc8f53f8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fa51e488c27090b4"></a>[`public Inno.Native.ImGui.ImGuiTableColumnSortSpecs* Inno.Native.ImGui.ImGuiTableSortSpecs.Specs`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6343) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9066addadd4ba39e"></a>[`public int Inno.Native.ImGui.ImGuiTableSortSpecs.SpecsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6344) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea4677916ce82a71"></a>[`public byte Inno.Native.ImGui.ImGuiTableSortSpecs.SpecsDirty`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6345) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9838aef9711fa2fe"></a>[`public Inno.Native.ImGui.ImGuiTableSortSpecs.ImGuiTableSortSpecs(Inno.Native.ImGui.ImGuiTableColumnSortSpecs* specs = null, int specsCount = 0, byte specsDirty = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6346) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bae907d4b0d0cc72"></a>[`public void Inno.Native.ImGui.ImGuiTableSortSpecs.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63194) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e6574de4217d91c1"></a>[`public struct Inno.Native.ImGui.ImGuiTableSortSpecs`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6341) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableSortSpecsPtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableSortSpecsPtr 4dc3993d08b3b25c64ea06bcb472eeae75d2c740f8a420979f768e836fa691ea -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.#ctor(Inno.Native.ImGui.ImGuiTableSortSpecs*) db8c2e79ca5394bc9542541e5e11f240590293d46e8b419b6e5cf084dc2970c6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.Handle b0474d2575b5fbcdcc40e1781822889d63a8fcbd48627ec0eff59b9d2b59dff3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.IsNull d02913947154b34d17b0bda39f05315651444492e291a64bc979d79b2cdf9fc6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.Null 1d827b1199a5b491c69d46264d969a039debc255428edf9dbb40559cba3aa0d1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.Item(System.Int32) 72b0787bd6f489d58b822b4eb16dd61b30593b6ba7b93b3e3ce1321d0352cb96 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableSortSpecs*)~Inno.Native.ImGui.ImGuiTableSortSpecsPtr fa4e81cdaf8dc091171c5ccfd69f687b129f213add6e0376687ae543b7cf36e5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableSortSpecsPtr)~Inno.Native.ImGui.ImGuiTableSortSpecs* eb761618c403af30899093925d99a7908089dc9e0e71a6e142e8a3417fd315c9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.op_Equality(Inno.Native.ImGui.ImGuiTableSortSpecsPtr,Inno.Native.ImGui.ImGuiTableSortSpecsPtr) b419da632851c85562c5e1133310dc3021a5dea55a44320a26dacc8df75da7e0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.op_Inequality(Inno.Native.ImGui.ImGuiTableSortSpecsPtr,Inno.Native.ImGui.ImGuiTableSortSpecsPtr) 069f2c7aabe1b6f0353e385e6201c84a032d2676331381b32ad0b2a2b2c8c87f -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.Equals(Inno.Native.ImGui.ImGuiTableSortSpecsPtr) 3143ea8f5552e4ee4facf8fb860935841789c22c82fc3ceea57551af0553461d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.Equals(System.Object) d98af727b30eee356f85da06eb2674c17ef97fb39e9ab3e0c5fef708c53c8adc -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.GetHashCode cbb6ca6960da1598ed09139fb2b9e105a97991c8bfe11d7c4996a82b7d6ac3b0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.Specs e51614cd24705705f0f0723213069ba15b4950f45c26c00bb3421af76ca1676e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.SpecsCount 5eaf44eead5ae325550e30d064168b00c0659c361c137d69a3b3cf39d51671c9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.SpecsDirty c4b84ba59a70dfb1a24a08f621471ea745fec252e61486a45491f6b921bd86eb -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableSortSpecsPtr.Destroy 69cac0e41236e056b500aec8a64c892d700e6283f992da7f37bab4bfe719cf69 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-be59311d85b474a9"></a>[`public Inno.Native.ImGui.ImGuiTableSortSpecs* Inno.Native.ImGui.ImGuiTableSortSpecsPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17171) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2bc4f44b97eca1f4"></a>[`public Inno.Native.ImGui.ImGuiTableSortSpecsPtr.ImGuiTableSortSpecsPtr(Inno.Native.ImGui.ImGuiTableSortSpecs* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17166) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a7622d25fdf3f073"></a>[`public void Inno.Native.ImGui.ImGuiTableSortSpecsPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17192) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-12c0c2fe5b4a014b"></a>[`public bool Inno.Native.ImGui.ImGuiTableSortSpecsPtr.Equals(Inno.Native.ImGui.ImGuiTableSortSpecsPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17181) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-64a0d62181a9335a"></a>[`public override bool Inno.Native.ImGui.ImGuiTableSortSpecsPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17182) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-89b3ba111f92f2d6"></a>[`public override int Inno.Native.ImGui.ImGuiTableSortSpecsPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17183) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-a4f248d7daae8df3"></a>[`public static bool Inno.Native.ImGui.ImGuiTableSortSpecsPtr.operator ==(Inno.Native.ImGui.ImGuiTableSortSpecsPtr left, Inno.Native.ImGui.ImGuiTableSortSpecsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17179) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-440c1f0299be80e8"></a>[`public static Inno.Native.ImGui.ImGuiTableSortSpecsPtr.implicit operator Inno.Native.ImGui.ImGuiTableSortSpecsPtr(Inno.Native.ImGui.ImGuiTableSortSpecs* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17177) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8ec2dbbe546db365"></a>[`public static Inno.Native.ImGui.ImGuiTableSortSpecsPtr.implicit operator Inno.Native.ImGui.ImGuiTableSortSpecs*(Inno.Native.ImGui.ImGuiTableSortSpecsPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17178) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f64a85188577403b"></a>[`public static bool Inno.Native.ImGui.ImGuiTableSortSpecsPtr.operator !=(Inno.Native.ImGui.ImGuiTableSortSpecsPtr left, Inno.Native.ImGui.ImGuiTableSortSpecsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17180) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-19417f99add63914"></a>[`public bool Inno.Native.ImGui.ImGuiTableSortSpecsPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17172) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-444210867b1272a1"></a>[`public Inno.Native.ImGui.ImGuiTableSortSpecs Inno.Native.ImGui.ImGuiTableSortSpecsPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17175) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-094b1a8273e051fc"></a>[`public static Inno.Native.ImGui.ImGuiTableSortSpecsPtr Inno.Native.ImGui.ImGuiTableSortSpecsPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17173) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-29acf636d20b4932"></a>[`public ref Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr Inno.Native.ImGui.ImGuiTableSortSpecsPtr.Specs { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17185) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-29f25d700ccb0aee"></a>[`public ref int Inno.Native.ImGui.ImGuiTableSortSpecsPtr.SpecsCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17186) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e8981cf2c12175fb"></a>[`public bool Inno.Native.ImGui.ImGuiTableSortSpecsPtr.SpecsDirty { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17187) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-10635e9086f4c7a0"></a>[`public struct Inno.Native.ImGui.ImGuiTableSortSpecsPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTableSortSpecsPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17164) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableTempData</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableTempData 923288f001cf4150644c0196bb59f1217f474529c556da6d68b93b0e1241ce27 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.WindowID 2d4a398d94d3673f68969b1f90f84a986e17f8ad0fcc2ed8395eac1074910d7c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.TableIndex 1e63d33c8ef8acc950c5be35bad0e518b61cb2853d05ed4e2746d8dd23f24f25 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.LastTimeActive 0afeeca4be881506632b011d4aa8593b79d1de6d733723fee09a11cfec6ec14d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.AngledHeadersExtraWidth e4651b2d4ad1e18bc88bb5239d1cb5e4c334916cfe5b3bada7a040cc6df5842a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.AngledHeadersRequests 68b9dc33935f023a292754243345dfbbd77884b40bbbee6c0868d00dfa8a1eb0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.UserOuterSize 3990957b31ac09fac567924442661572eaeaaf4dd4319126017b41b56d497440 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.DrawSplitter a36024977a10a640aa4dc9e94a8c3725740025c2264c0a3fec0393af171e0068 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupWorkRect 27f89e6e816bacbac4c5c3e6cc786a0ea9d0d73b3f49efc2eb600c8c60de09a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupParentWorkRect 952eb8266e8623b56346c4e7472cb8e32254c2949091635169f321755942ace6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupPrevLineSize b4b1a79b4453576656b1bdebf0d19632efba2d416cec87abb3246908ba756405 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupCurrLineSize ff40edcde7513b90907dd9267bc25b0569d7b14c934770664e7a6842fb6a2c7a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupCursorMaxPos 3ddb43c9ea67d5b6aa3b416278625594e07f001be2303015c279566048d9c8b3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupColumnsOffset a85e2428cefa424ad2c4129e68c2e53a9a7ca33d9a6a3079d2039a818e4729d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupItemWidth 082650763a20ac215808b5d699acc0d930c475240b8707d6d66c94623bcf6cef -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempData.HostBackupItemWidthStackSize b844b86d030c31d6b11963dad6dee07700ca89426a1ad23a36de32e439fd8da4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableTempData.#ctor(System.UInt32,System.Int32,System.Single,System.Single,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiTableHeaderData},System.Numerics.Vector2,Inno.Native.ImGui.ImDrawListSplitter,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,Inno.Native.ImGui.ImVec1,System.Single,System.Int32) cfc490ad192f9c03b97b868788b9b0232078c84d7831f7062cd5c27d46e8a612 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableTempData.Destroy 3c0e569bee474fcf9b3c8ee1ed2900b5001dc3acfc66219b6d3553edf57a22e2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0b60a03b5fb6d87c"></a>[`public float Inno.Native.ImGui.ImGuiTableTempData.AngledHeadersExtraWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6066) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-596eeab6cb1138ba"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTableHeaderData> Inno.Native.ImGui.ImGuiTableTempData.AngledHeadersRequests`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6067) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f77d03c174b1acc3"></a>[`public Inno.Native.ImGui.ImDrawListSplitter Inno.Native.ImGui.ImGuiTableTempData.DrawSplitter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6069) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-161eb7a478a8224f"></a>[`public Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImGuiTableTempData.HostBackupColumnsOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6075) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f28721a2f1b95854"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiTableTempData.HostBackupCurrLineSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6073) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39cb41710c82a75c"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiTableTempData.HostBackupCursorMaxPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6074) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bdb44b74d758235b"></a>[`public float Inno.Native.ImGui.ImGuiTableTempData.HostBackupItemWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6076) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6e48200d7c8d9070"></a>[`public int Inno.Native.ImGui.ImGuiTableTempData.HostBackupItemWidthStackSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6077) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3829a5a53ba77b3f"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTableTempData.HostBackupParentWorkRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6071) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ddc9e0513e991511"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiTableTempData.HostBackupPrevLineSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6072) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5605b44d32221a4d"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTableTempData.HostBackupWorkRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6070) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f53c7d76fc47760d"></a>[`public float Inno.Native.ImGui.ImGuiTableTempData.LastTimeActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6065) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7333ee044a440ab2"></a>[`public int Inno.Native.ImGui.ImGuiTableTempData.TableIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6064) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4d9457cbad88016d"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiTableTempData.UserOuterSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6068) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-082f39e2343f9178"></a>[`public uint Inno.Native.ImGui.ImGuiTableTempData.WindowID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6063) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-683cbd941bbd8ef6"></a>[`public Inno.Native.ImGui.ImGuiTableTempData.ImGuiTableTempData(uint windowID = 0, int tableIndex = 0, float lastTimeActive = 0, float angledHeadersExtraWidth = 0, Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTableHeaderData> angledHeadersRequests = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTableHeaderData>), System.Numerics.Vector2 userOuterSize = default(System.Numerics.Vector2), Inno.Native.ImGui.ImDrawListSplitter drawSplitter = default(Inno.Native.ImGui.ImDrawListSplitter), Inno.Native.ImGui.ImRect hostBackupWorkRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect hostBackupParentWorkRect = default(Inno.Native.ImGui.ImRect), System.Numerics.Vector2 hostBackupPrevLineSize = default(System.Numerics.Vector2), System.Numerics.Vector2 hostBackupCurrLineSize = default(System.Numerics.Vector2), System.Numerics.Vector2 hostBackupCursorMaxPos = default(System.Numerics.Vector2), Inno.Native.ImGui.ImVec1 hostBackupColumnsOffset = default(Inno.Native.ImGui.ImVec1), float hostBackupItemWidth = 0, int hostBackupItemWidthStackSize = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6078) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2e1437c2d7000b1d"></a>[`public void Inno.Native.ImGui.ImGuiTableTempData.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67617) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9f81426ba8ab1f91"></a>[`public struct Inno.Native.ImGui.ImGuiTableTempData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6061) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableTempDataPtr</code> · 29 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableTempDataPtr 1789c26821165d7c442ba5fdfd66cfa5589db33bd0f290f1a2e857bfc1712226 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableTempDataPtr.#ctor(Inno.Native.ImGui.ImGuiTableTempData*) 9c41c1fb97ebb3c20ae08e84675afbab3ff76935ac4a17ebbe037252f418ecd3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableTempDataPtr.Handle 02d0da1ea3a52e28297b6f22b4c94a34728e231a2fe3e1dfc5fa36483e1475c4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.IsNull a11b1e3b32a44a398dfc1535e3c16d18c2c7d4f65b5969183a85ac801a8c7056 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.Null 449612401ac6f8ea0811fa5fb214697a37ac55040294fa06e260257cafe9ae00 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.Item(System.Int32) 3774e229081fbf3e812400df54e6af4c41858a51960d8614558a61df2551910d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableTempDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableTempData*)~Inno.Native.ImGui.ImGuiTableTempDataPtr 60c4bf979650016b7fc4c5c79270bc3008a5901b42f83dbb73606164b98309b8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableTempDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableTempDataPtr)~Inno.Native.ImGui.ImGuiTableTempData* 73c14b1707deed089bc6d23b4e52258ec2eceda2c3be01ff7d92338ddb47cf9d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableTempDataPtr.op_Equality(Inno.Native.ImGui.ImGuiTableTempDataPtr,Inno.Native.ImGui.ImGuiTableTempDataPtr) 2b695b85377158e578c1ba8ac92a8c4925d204fbd70d001b6f8355d32670c63a -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableTempDataPtr.op_Inequality(Inno.Native.ImGui.ImGuiTableTempDataPtr,Inno.Native.ImGui.ImGuiTableTempDataPtr) d54ee76cca4420d5053785932ddc39b0ba6be9e5dcd7f9eb4b536f3f7e88706a -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableTempDataPtr.Equals(Inno.Native.ImGui.ImGuiTableTempDataPtr) d55f7fa1dfd31d478bf509adfc0834e5fcafba660ae6381a1a1da25895bea494 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableTempDataPtr.Equals(System.Object) 067ed7f1ca38c87cca3c1ea08884cd6f05fded98ec29b2f14c0649200891b822 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableTempDataPtr.GetHashCode 7e55571b5f1679853d849c4cb45af1fbecc8d01223bdcaf53289481c8c13e4a6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.WindowID 9d0357bea9ed69bfd3c178632702017e3b9188da26e9e274ea9168096cfc9012 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.TableIndex 074e9d0d432e63bdb424c3278d04076976776038a8dc0cbf37c6b5cef842d412 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.LastTimeActive 890ac35e9e1be364d888ba50e2eefada9ad96cfa4551d10c00a5cb644d480127 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.AngledHeadersExtraWidth d9048a354382536c46f8f7d25de90c16a3483a84a8b568ee727388cd349cc8d5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.AngledHeadersRequests e9963bb67888c331782ba5d27b5b10c42e543edc17b7fcbd2a534a47b0ac9c03 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.UserOuterSize 584ff64e3e855e4879cede08a2f5cd29dd1bf6eaa1ef41ccbe314344b5c9ec31 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.DrawSplitter e127c694f682f1c71df727f0f8c2d8a8ccb1c6efa96c5653ab75f386303ed8d0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupWorkRect 75d30819b9a5c18cef64d43820ac9133edcb408bfd1e40593f86e5cfed1a21a6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupParentWorkRect 442ffb90b9912c511b7b609e3efd270cf0240111ecddfbbf3196556f0a1c80c2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupPrevLineSize d49b78847e1c3463863fa562b5ec0c8474fa59eed75820ddc7752aa22de8c788 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupCurrLineSize 94661cbb09278f40aca38c9b4bdb3ded904968ede31360f595795e1aacd2640f -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupCursorMaxPos 54772191e20de4e4f31bc950cbeb3984404163b498e539cf651adfa62295dd4a -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupColumnsOffset 8a667d7be3a27a1e713be3c68e3d985f7e761e725cc023d45db32b147592d0a4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupItemWidth 2b84b1fce229fcf6085ea6ac6a42b4dd43d91ac7180e4beaf5fb81266301a545 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupItemWidthStackSize c277b5f12c0100748b55a492a2ec8a76efd627bdc986067d13a66edf5215c637 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableTempDataPtr.Destroy ac3b83f51037bba5b57ba37dbe98aed590063d65c8218cc6f254c9458c3df9cd -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-84333c62c08d1c01"></a>[`public Inno.Native.ImGui.ImGuiTableTempData* Inno.Native.ImGui.ImGuiTableTempDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17206) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a0e319022e26a22d"></a>[`public Inno.Native.ImGui.ImGuiTableTempDataPtr.ImGuiTableTempDataPtr(Inno.Native.ImGui.ImGuiTableTempData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17201) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bf6712f1707656ae"></a>[`public void Inno.Native.ImGui.ImGuiTableTempDataPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17239) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f53d69671b8fd528"></a>[`public bool Inno.Native.ImGui.ImGuiTableTempDataPtr.Equals(Inno.Native.ImGui.ImGuiTableTempDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17216) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-73b06af4e16e9f5a"></a>[`public override bool Inno.Native.ImGui.ImGuiTableTempDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17217) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-9b707da1338a5981"></a>[`public override int Inno.Native.ImGui.ImGuiTableTempDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17218) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-c433f9436810fa8e"></a>[`public static bool Inno.Native.ImGui.ImGuiTableTempDataPtr.operator ==(Inno.Native.ImGui.ImGuiTableTempDataPtr left, Inno.Native.ImGui.ImGuiTableTempDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17214) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-66ca9487ea56aef8"></a>[`public static Inno.Native.ImGui.ImGuiTableTempDataPtr.implicit operator Inno.Native.ImGui.ImGuiTableTempDataPtr(Inno.Native.ImGui.ImGuiTableTempData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17212) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9ec25eb7d5fa1ada"></a>[`public static Inno.Native.ImGui.ImGuiTableTempDataPtr.implicit operator Inno.Native.ImGui.ImGuiTableTempData*(Inno.Native.ImGui.ImGuiTableTempDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17213) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d0f9a0f6a449a08d"></a>[`public static bool Inno.Native.ImGui.ImGuiTableTempDataPtr.operator !=(Inno.Native.ImGui.ImGuiTableTempDataPtr left, Inno.Native.ImGui.ImGuiTableTempDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17215) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f5d7db2b37e3a03e"></a>[`public ref float Inno.Native.ImGui.ImGuiTableTempDataPtr.AngledHeadersExtraWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17223) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7796c6a3a04f8601"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTableHeaderData> Inno.Native.ImGui.ImGuiTableTempDataPtr.AngledHeadersRequests { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17224) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-caed30be80c722ae"></a>[`public ref Inno.Native.ImGui.ImDrawListSplitter Inno.Native.ImGui.ImGuiTableTempDataPtr.DrawSplitter { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17226) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-93f44445048a554e"></a>[`public ref Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupColumnsOffset { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17232) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c2bc37e3cb3d4fda"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupCurrLineSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17230) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-941b239d60a3d3f2"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupCursorMaxPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17231) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-79ffc464734db815"></a>[`public ref float Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupItemWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17233) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7c9f2d5a13b0904a"></a>[`public ref int Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupItemWidthStackSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17234) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e423bcb7b7072e1b"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupParentWorkRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17228) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2f5febb2852998b5"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupPrevLineSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17229) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-18cc45ad03102e47"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTableTempDataPtr.HostBackupWorkRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17227) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5b00a21acff48d73"></a>[`public bool Inno.Native.ImGui.ImGuiTableTempDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17207) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-910ed4a463937da3"></a>[`public Inno.Native.ImGui.ImGuiTableTempData Inno.Native.ImGui.ImGuiTableTempDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17210) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c7375152c830f324"></a>[`public ref float Inno.Native.ImGui.ImGuiTableTempDataPtr.LastTimeActive { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17222) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2498b57604dab0a3"></a>[`public static Inno.Native.ImGui.ImGuiTableTempDataPtr Inno.Native.ImGui.ImGuiTableTempDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17208) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-078e10dece15ae94"></a>[`public ref int Inno.Native.ImGui.ImGuiTableTempDataPtr.TableIndex { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17221) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-24b9325175e2dadb"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiTableTempDataPtr.UserOuterSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17225) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4446739bdfd13b2d"></a>[`public ref uint Inno.Native.ImGui.ImGuiTableTempDataPtr.WindowID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17220) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-15f34be39c19af85"></a>[`public struct Inno.Native.ImGui.ImGuiTableTempDataPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTableTempDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17199) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTextBuffer</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTextBuffer 7187dd5950d0da36c9061abfe023ea1ecb6c64b4d1a06436816bd37302e2e6cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextBuffer.Buf 9144fb35aafe6c1aa4718ac5938a2a52228e6a8160e055f669ba8f459f0a18c1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBuffer.#ctor(Inno.Native.ImGui.ImVector{System.Byte}) 5408663ed7f0e47a2ca0242458e7bdd3015fa7c0e8a67f7565330ec3fc72233f -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBuffer.Destroy bb630711a55b568e4c70b754458eed3ff58e6fd7e4a7d7fa834277ac04f8aa83 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBuffer.begin e3298317d8d8a36e6024778854fa76cf8dd795d623c75e65ecf1b3156560028e -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBuffer.end 915a070a5f6edffb3328808fe62bc71877d844f4a60e0ffea6d21e9c653ff380 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBuffer.size ecbc1bc7b9de192fb10f17248db4558c430e83049b2040555d64f2007157503e -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBuffer.empty e64f648beab0db292f23a1b7af6b14efaa45f78e17f984586b363d03fee5b965 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBuffer.clear b0c4c6053c4a2b7b5af9c4b006348da1644d80b108108d5a412091b0e539f4a6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBuffer.resize(System.Int32) 074cf9581a1cb83ac296ad280bcd77c91204b583e49bcd1498a737b548df79ff -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBuffer.reserve(System.Int32) fef558bc8c39df3a13cde13bf9aea24417f5aba8b7a38d35042564b407c54bcd -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBuffer.c_str 08f006b6457e993025f4449cfa54039f1f37b3081a82a33809aae335a98792c9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBuffer.append(System.Byte*,System.Byte*) 0640585d254f2b534d8bc69173f4cf26b6c999669da2094f979a86dd55685749 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBuffer.appendfv(System.Byte*,System.Byte*) 23e767893143aa4dcc6dfa4ad6c97f1ccb2afd9587583036f45cfb088cb0abdf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1c6a3dcdb0b9dc72"></a>[`public Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImGuiTextBuffer.Buf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4372) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bbb30ad6fcac0ad0"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer.ImGuiTextBuffer(Inno.Native.ImGui.ImVector<byte> buf = default(Inno.Native.ImGui.ImVector<byte>))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4373) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-50e6019188910328"></a>[`public void Inno.Native.ImGui.ImGuiTextBuffer.append(byte* str, byte* strEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63807) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ee317ffb13dc285f"></a>[`public void Inno.Native.ImGui.ImGuiTextBuffer.appendfv(byte* fmt, byte* args)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63818) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-27e124116219f616"></a>[`public byte* Inno.Native.ImGui.ImGuiTextBuffer.begin()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63719) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3ba5bcbb8b49ac57"></a>[`public byte* Inno.Native.ImGui.ImGuiTextBuffer.c_str()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63796) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6860fa3c04b8a804"></a>[`public void Inno.Native.ImGui.ImGuiTextBuffer.clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63763) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-55e12a63556bde12"></a>[`public void Inno.Native.ImGui.ImGuiTextBuffer.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63708) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-866f814067f32536"></a>[`public byte Inno.Native.ImGui.ImGuiTextBuffer.empty()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63752) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d0939215caa8532a"></a>[`public byte* Inno.Native.ImGui.ImGuiTextBuffer.end()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63730) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-631fabccbd69ae11"></a>[`public void Inno.Native.ImGui.ImGuiTextBuffer.reserve(int capacity)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63785) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-96043e46fa8287d0"></a>[`public void Inno.Native.ImGui.ImGuiTextBuffer.resize(int size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63774) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-806b0fb3772f70f9"></a>[`public int Inno.Native.ImGui.ImGuiTextBuffer.size()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63741) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d167127fbeb47b7e"></a>[`public struct Inno.Native.ImGui.ImGuiTextBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4370) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTextBufferPtr</code> · 26 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTextBufferPtr 9e399249650497a39594350464b15c3f0ca5a0d4d945aef029f0ea034c414581 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.#ctor(Inno.Native.ImGui.ImGuiTextBuffer*) b09a92c268f307f9756ea6b23c5f8ee4640943c2d6c2f3ad1d4f6e3189ab6ca2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextBufferPtr.Handle 48117ae28bca8772da6e24e53c0015723ada327eb3eca8601946a7cc359a8af6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextBufferPtr.IsNull 9606ebd2ba4802a303871a5cf6eb39162b61a7d3b6fd5ade3765c110b5917ffc -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextBufferPtr.Null 10ddf2f5cfd10c3d94a65f923dd710efa1c1941489a8c8f0721b5e493fe37490 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextBufferPtr.Item(System.Int32) 32fa9f240a0aa76394e0f181726521b847b0d1ab89ccc349927bf7e70ba596f0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.op_Implicit(Inno.Native.ImGui.ImGuiTextBuffer*)~Inno.Native.ImGui.ImGuiTextBufferPtr 3da7f14d654a10f1e534581d18137026b8c3e150de281c0928be069d2781871f -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.op_Implicit(Inno.Native.ImGui.ImGuiTextBufferPtr)~Inno.Native.ImGui.ImGuiTextBuffer* 8d50b27d43218af3cb9a5ea0749520e553883b16a0b632f7ba74c70a2c1796ee -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.op_Equality(Inno.Native.ImGui.ImGuiTextBufferPtr,Inno.Native.ImGui.ImGuiTextBufferPtr) 643134372c8e49d5a36eab1530229bea67da626970c969a7ed9fa1679fd00375 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.op_Inequality(Inno.Native.ImGui.ImGuiTextBufferPtr,Inno.Native.ImGui.ImGuiTextBufferPtr) 744bea5d142d80b697892d30e8bb5f9964535955501140a03e97610db377dff7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.Equals(Inno.Native.ImGui.ImGuiTextBufferPtr) 2dd07a645a2bfb5941e319e08b9e7341929bdf9d2cb76c117678d21f6283e91d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.Equals(System.Object) 295041226bfdf19961a018cb86d6f8e78abf345515755d9e2aa2e14743cf2ad3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.GetHashCode 255b3e1cb4b9b0889d29391c117fc31d9793736d107e70c224531398bbd7f2a9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextBufferPtr.Buf 0e031e17b91b229e009b0afa5ffd64587631762f2d29daa4956efbf94c4b1727 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.Destroy dfe07ae8b6f31487e9837f1d5bdcd4a598f908353042681909a4b3676522418a -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.begin 630cee4397cde0a01b7b5570593d6508b840819321efa27ef4391440400a2489 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.end 8c7bdff80b81bffabc92c859bbd0409ed8084af42d6067b8a53b812840fe24c2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.size a73ce3f6d8dea70f8ceb8fd0a2880ab12238c07f9ff3bb5e1702c1064464a25c -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.empty 22f5e9b0e4d2e7f49ecad921719c7e4b93c053a70c98168cc426714e22992428 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.clear 43df569d8ddfe129233f27466fec0eae0a94be193c8e1d5615e6d533819b5375 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.resize(System.Int32) 19138ae1fb79bc8e9853d333db8d63cbcfcdb579bf33bb3f6e0d7f81672f01d9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.reserve(System.Int32) 92542706238bca9aa2df489e9f3bfc998cfc94a2a010327d2a8b77da784846f6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.c_str 7fb9e8f043542f566ec914909c43d55cf09f633a0f70ca05693924192eb497b6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.append(System.String,System.String) 585142bdd3a828037af4653defec1dcd2a951f6ae2f281bc986e9958a17cda4f -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.append(System.String) 286f505ae4bc32974c4980c91a82cd95e00e03c32052f69c941773e5cf53385e -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextBufferPtr.appendfv(System.String,System.Byte*) 30a75bfd564311f9217d6ad60bdade323b497f07d126c2a25a6c52a45d129ffc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5e6d63c625855498"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer* Inno.Native.ImGui.ImGuiTextBufferPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17253) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a683e1bfdf453d5"></a>[`public Inno.Native.ImGui.ImGuiTextBufferPtr.ImGuiTextBufferPtr(Inno.Native.ImGui.ImGuiTextBuffer* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17248) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ce6d1d60adcaf3d0"></a>[`public void Inno.Native.ImGui.ImGuiTextBufferPtr.append(string str, string? strEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17344) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fb92afb5183cba93"></a>[`public void Inno.Native.ImGui.ImGuiTextBufferPtr.append(string str)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17352) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-faf23a29a494c69b"></a>[`public void Inno.Native.ImGui.ImGuiTextBufferPtr.appendfv(string fmt, byte* args)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17360) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e1b2b5f91809e920"></a>[`public string? Inno.Native.ImGui.ImGuiTextBufferPtr.begin()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17280) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d0334b1b1f0dcd5a"></a>[`public string? Inno.Native.ImGui.ImGuiTextBufferPtr.c_str()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17336) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b711fde6f6e3cd41"></a>[`public void Inno.Native.ImGui.ImGuiTextBufferPtr.clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17312) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aa6035c94e7c16b1"></a>[`public void Inno.Native.ImGui.ImGuiTextBufferPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17272) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-498bcfd6d629a5c9"></a>[`public bool Inno.Native.ImGui.ImGuiTextBufferPtr.empty()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17304) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fea571ff34242e4b"></a>[`public string? Inno.Native.ImGui.ImGuiTextBufferPtr.end()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17288) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c7f1f1a7d6f1493a"></a>[`public bool Inno.Native.ImGui.ImGuiTextBufferPtr.Equals(Inno.Native.ImGui.ImGuiTextBufferPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17263) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-9cca1d8278172c51"></a>[`public override bool Inno.Native.ImGui.ImGuiTextBufferPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17264) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-286eaf82b8d6678e"></a>[`public override int Inno.Native.ImGui.ImGuiTextBufferPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17265) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-e92fd4b418630669"></a>[`public static bool Inno.Native.ImGui.ImGuiTextBufferPtr.operator ==(Inno.Native.ImGui.ImGuiTextBufferPtr left, Inno.Native.ImGui.ImGuiTextBufferPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17261) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5ec6bd78c8d345a8"></a>[`public static Inno.Native.ImGui.ImGuiTextBufferPtr.implicit operator Inno.Native.ImGui.ImGuiTextBufferPtr(Inno.Native.ImGui.ImGuiTextBuffer* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17259) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-82871a9842b8fbab"></a>[`public static Inno.Native.ImGui.ImGuiTextBufferPtr.implicit operator Inno.Native.ImGui.ImGuiTextBuffer*(Inno.Native.ImGui.ImGuiTextBufferPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17260) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-50a2b19ae21f03aa"></a>[`public static bool Inno.Native.ImGui.ImGuiTextBufferPtr.operator !=(Inno.Native.ImGui.ImGuiTextBufferPtr left, Inno.Native.ImGui.ImGuiTextBufferPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17262) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3d5dd5ee4bd80097"></a>[`public void Inno.Native.ImGui.ImGuiTextBufferPtr.reserve(int capacity)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17328) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bf6fe2256918dbcc"></a>[`public void Inno.Native.ImGui.ImGuiTextBufferPtr.resize(int size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17320) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0e30ab66aab99d41"></a>[`public int Inno.Native.ImGui.ImGuiTextBufferPtr.size()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17296) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-98c47f65c74b264d"></a>[`public ref Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImGuiTextBufferPtr.Buf { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17267) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba3c2431c282916b"></a>[`public bool Inno.Native.ImGui.ImGuiTextBufferPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17254) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7430672a54780f6c"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiTextBufferPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17257) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c52415c2af419eaf"></a>[`public static Inno.Native.ImGui.ImGuiTextBufferPtr Inno.Native.ImGui.ImGuiTextBufferPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17255) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-82df34eecc25c077"></a>[`public struct Inno.Native.ImGui.ImGuiTextBufferPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTextBufferPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17246) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTextFilter</code> · 267 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTextFilter b5651d927738ef825c24741297796b42c95a12da852a8bc75dedd8683dd982d6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_0 548fbaa03a5e689259a1ccc36d484fb60d3644dca0622474bcc2a4ec41402d1b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_1 c09eea19c05e19861814102c1fa6b9c5cfcd876c181a8eb2d8c3db5e938de93f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_2 b54c0760b71551322ac810fbff474f97f2ba73debbcbdc05da6ef962674d2738 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_3 1e920c64485c7f78b507842b88c8fb8a00b0047a1d30fa6b249b443c5e64870d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_4 edff2c627a8b1f75fe2fab2126f92fa5ab9e39605815927ce49740b35048b913 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_5 163cf820821dc60bf9814730a472c54226a1933674c7322efabea13701d4ebe0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_6 fa49ee21871351666337625fb47f9db2e0dbb7d183a59eb2900559c5de0c119e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_7 e99a3de9ff60d4067cfa2cefaf3256b20f8c4a80acf575926955014b5e02fcdb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_8 d93b1cd9d348b37a9c8ae1d4a5eb8bfaf9e2c8686bd9e921ad5f5b0a24d978e6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_9 b838a422fb88bbcaf224d273c892e8e182869be21002cb3844620194dc44dce6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_10 715cc322b52596b9afafc41819e77cf9297e19949f41eb2ca720e88e5bba255f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_11 52b1477930d8dbac598fc4379ba672e3fe801e1567cc820e019af4f0ddb75cb2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_12 36a065fa5db3e791ea26c80bb76c7d9f214df462bcb1556a0efabe918b676d40 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_13 cf7f9a9add04276dbd7edb36f305d604c825a4cf23b74ca9f8c8789f553b11d8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_14 bfe8355086969505f1174c7a00a703e3dea6a0a0f5e38be735ac37e660eaceb6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_15 24e7f3d7a5e71a05f5f407440d980c816b965023bc8e6c71613bbcbbb693e613 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_16 9b43a120f57349d10f0eeb8fe0dd448517c3f92cdc4dfcc61787bd575a9c61c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_17 1e8fedeb857690c6f19930d181a8bcc5b54e04682e29341cb01abaff4e416ebc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_18 56a7ade0b991c82e7770dff291d3d5d0c7efb0a65541c10477bf2b63e7a18993 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_19 9061f3cf083584f8be7c63c123f3622a5d83a51fbd429f1d9ff1947fcb909bce -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_20 c2a4f682f5c2e29c2daf6800492f7b97ff3c6a613173e05815263a77232b257e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_21 ceb5a6e57b138ecd0ae9edd896936e8601c125b02d8a4c3541f4aa371939b57f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_22 c3a2a7f89d5c6bc8cfa7cbf35f7ee2713aa102b46148d5a5d836a3ed7a0bc01b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_23 379fb215a47d68ea6254568eaee25ae0ce628f3fef343ee680fc5f105c60b7de -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_24 5be886f54a7d32387c64a6cbc1e8c6dfc7b47ad133518e573c7e65ab2ba10754 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_25 9516adaa410caf211474a69cb89f23b42c39b018f83a9abce55b72c43f4dc305 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_26 9193fd2e5be02aa171f5667cb9b23e8f95d3cb0eb11a1166933ecd20aa03361c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_27 6c683552d4699b4c03182b0d6ef097307ceb359d4f6f871de54b23b24bb43851 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_28 2ff05c5f66c46ce5b6204f0ba5610c0d171a182ceda84bf38527668f4e53f210 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_29 578e8dc2128bda9be733ba455e79889322577cedd3457ee6c39ca1a640079020 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_30 b4bdd597aaed49315844c661ff2feb645816468e1358630b466d3bc43d8d9e2e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_31 009c55d632d98e19027c667be2b84c37978447c35f7c4db0cb16d4cc29190555 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_32 c9fd0aba7cc2843701fd4a1f8bca1a2493f1e9dd9fa8795d05173b2572a2f5e7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_33 2975a01bfb7965c70b8a728ed9adbd3456e492c10a0b1afa9c76224c8ff8f807 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_34 6bdd0ab2259cd837cd322d96ee956339ee593af9e40b0ed26b4dfaa9f60eecd2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_35 b14630bb748d71e8fb1f88205a0fa6d987c8cfce5297a56d940f50e000ec7327 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_36 3e18633b555a79119fca1173ad389a4df24900889b1ee162830788ab41dbdd05 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_37 fa17572fad0e78ec60b9cc02775b08daceaa23a2fa7a8c11e4b1e43059af49a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_38 c58d3a7efa3adbe10aaa3d3160cd3458b541e34bdf8d2858454664837b635711 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_39 be61cd87b2398d1e691ea901f4b3e0e3fac5a176340d607615b523e8e23b89a9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_40 7434ff4f9b2e1da940fd9a874e6f42c499aaa728ca8de73dd9462083af576286 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_41 cc6cd3f0c910c075cb76e1809247fade788f7a8d247f8bf349f2bef0cfa65853 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_42 22017e3e4357e62cb2349ac7c8357b24e715aaa6ac97192a9d433187fe691283 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_43 9047f7b51efde7c87f75e3623cd9e24789ce4a752c820e97d0849f84ad92be46 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_44 e424384c9b5d492c64fe010308d4b172b8244e1a2051dc7e3ee498df4ae40937 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_45 3286db83586b5e7b5ad6578100a16c2ca270cdf3ce9cb52e277c5d04f9839073 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_46 a198318e1cc9205342b292eb653ce71012ba63a9678db9c3edef440d034fe8fb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_47 e3b410091f38810a0128864d26e6b67097c84fa2207f28d23e97d0d471a171b6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_48 fbfb8c6d1d97be7ab6bceedfe8230d8aa0a8d99878a9f4a4b8152011f90080e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_49 19dc4234740c8bc718214082f539479dfeaf70494dfb65620e5cb1d0bf91820f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_50 31c4b5375ada94c6603e5e18a70a351995a931fcee34098106d9358c9444f37c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_51 6b33c3114a799e7a4a06924bf52572e7f6326e097b7cadad6fc6abd7683ea6cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_52 6e5982d640c8227ff4fd70852e94053b2ca23bdc8bb9807499a0c9ea64da1f8f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_53 759f441a26d57ef6c8db8e3cd9b1ecebd92a9ef057438c41b3fd1dab991de5d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_54 1eac0412258280914283c223d45b1a66c3b846f5bfebf4a6f5f1b064ef151c0a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_55 85063aa3045c376631673e54b053f1c84c0ea40a8674ed35f57196b56b5895d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_56 964b8a6ea179a10a6ad189ba17f6541d34d7f56a7642517f0fce2ed37d00f5a1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_57 8dc6f2a40f36742d0af5ab83400e506886834b6ddeb1adb6a9eab3e7d9f8fc3a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_58 7aa77679496e839e8a13d6ca313e3900b6efc3d009ca81ed9077318fae9ad3ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_59 05705f07a8f4a1bc5d0ab8f7c5e6185b29274d606c3faf4ded53f950bd6c9e94 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_60 c59b913595a91c828541a1ac4bfdffec4395eeea7ef7f8092c1020a7956fd7e1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_61 144632cb1bfbde253e8820d51ce8e6c2e9ac903a4bf569deba687dca2d4b3391 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_62 76ba2306356b1c3a0ac6f11459810807c6adccd76497ef21ed44b515c9a53f02 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_63 9ab2e256ab9695a094e8882c8be529780c1ff2292dae11d118f3045526264c92 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_64 b5ee22bf23791bc9b4bd26d9222450eaaa6504d6ec787078a231dc605e1117d2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_65 9adde56bc5710eb79d8c899844fb675715280e0d4a4b1ff513cff266a79860ae -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_66 cf19a86732414ecf6852141db45e9d7f4dc26adba683f3bb205c750610141148 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_67 3a02b7add99edd4521933d3176bd0b136a1c03f355d66c7d80eb64d802c80ae6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_68 e28c53c9c7a3db318135da22b9f2a2243630f8cb0c38528d6ff957c974394baf -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_69 a518ba72e7c04c45c1ba433880a30aea4a07e9a63d0384dd6d5df313f5c49ffb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_70 be6396b9f737a6fe6d3cfa909d727b45985248947261b54a74d2867b02da2644 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_71 d7dd3b633d72780116793e1a01acbe804443c2606b76f4cfe9778ad8ad6b2794 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_72 c769eb5aa10c6b2ae6f76f7705fa451f07793ca4f1dc3d292b8ff2d0b112bb86 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_73 adba97ec7e9ed3a36c1fd940301ec289aa3a17cb1032462e5fd3c7fd2ea77bd8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_74 c996fe769967c650076ff659c838fc1065fe71b28e93ea0b98c92a99e2be280f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_75 1f4db582b3f78fe97be5432beb4cf750f863fd4356307e42c7958900df1ccbe7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_76 c84bc878e1388a5cd5002cad9909095fcb852a386f1b8b0f94d7a69b811a8b49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_77 a207b554b56f46c90745e2f5b0b73f8a11bce4932feb25e30502d11ff90fc6b8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_78 4d01e25c612c6691881d855102db5bda962ac77e745055db15091daf961f170f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_79 ff9568be248f869a12fd06aac4ab4c13f18984048d25bc1edf3f930f12c32f41 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_80 a021f78bb842d78db48536dad9e4abc2af667d70a50263aceb04fd0fadecfda0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_81 f6f9f50f3e3db4a0d0d4e99e80d1f9db35c397c834865948e313e80702c1296e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_82 c2109503f826939fa2ea70623179cbc42d5c82bdb40de04ac37503bc0c6fc69f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_83 4cc16f6ec1b2e83f621bdf486a2c5181ac69538c388b662d7d8b41cd2b6ebaa5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_84 bbe7eed10bb53d7ab782bb6c6a2eb9c270ffd09a5cccbfeaf89e3030326a77ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_85 fbf556fd1e2aac034faa80a2028d658e9e06062d13cd4620db6de865ddacc45b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_86 0c66176c39511f0495577babf55a6f9f225cce65f2ef9034b845f780d3ad5ba8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_87 112c3ff53a6177472b65cc424ed2ded06535e2241f176c50ba110ce82aa86763 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_88 6324c6c49fbb43f61a65a5819b8ee4c924876699cfe41fa2c6e4f8690d31a1c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_89 aad095d2c8245e7115d5a1e0c225805ee63a3e970f3da123123f2fc611bc3fde -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_90 c1fc56acfb57fe6083a5514f2625da2d74a7e4c153448c81984cda06e5cf1536 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_91 9f604cf2f071ad0d5716e98183629d04997eff46b6e6a4308aa49481dde7f26a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_92 d75f4279e6a86a383c7e4bcb19222221c953f976082588ee47492510dd6019b4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_93 7a17393b8320b9c8db19400dc0d3a8615a7a5f1ab14ac9a49fef3de3f11a8b24 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_94 a3631d0310ec5e09c5847f13e57f7613274ddaed32fcb45950a7d450a61c18f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_95 45e6516077bc9c94bfc98e87d65c949a4aa1a723c12100d8dea272f85740fc2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_96 a032dd425fb7f3b452d6ddaeb88eab3888b2ef45436b587add71be5bf45170f9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_97 d0db8bba39e85c944af203548cbf89d8e8cf676784aeaf8d67b7e44a69e3ee01 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_98 8df8f71d124684c5853fc4201a98283d33802b20fcef8f2cdeb5f29f7a70b85c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_99 c65d37a0f4539870b72b1f5dde6773a867b606cdda3169672530aa057d982760 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_100 cc5abbac4c2d21a09f5a93d64501f74b53adc891f14b0e35c65bc7f58d9c9303 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_101 9ec82e9b1d638dc36f79e5c06c8de84c029e1d5611f5bded7cfc6efad7147d03 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_102 84d77add8272caba38c63598b745d3e5bbc6becc605a6df964b81c022b5efe11 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_103 ed5ad8f336618235e388781441c5a5c3abd52c4e0632d96b6b8a72c411860586 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_104 417bc17bbcbe8385c0464f518168ca33de92cbfbdf25b73867a45267e7c9869d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_105 253a9cee599c8a82b30ce361bfd16d0fb9137dc0686dfb4113c3d746359312fa -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_106 e09142483f4ca018fe4754fd698f9eac476aadf6c13d769d0871afab3e05e1cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_107 bf36c868430e664fb337fa9375ffb45425b7246b1df515e888d527b6b3f90f24 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_108 e99a7d6b1e869c81e9907c92fbe16877684f1df66e191576608ce3ba5e6b1722 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_109 bce803a44e36ea638694d0d5f9cc0f911c37ec1c93d2c27ee55273ef6a9d5efd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_110 7809fb9a562db52a0d35bd6ba26478ea22b67e89508fc1264c640a5b2d00ac81 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_111 27c2876af9c8fd25b8c589f525d355b0a1192b3178ec2610876278c64b70d248 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_112 930be1dae5683f8b4a542b4caa75cd144cb60093c71d9c622a3b836096b311f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_113 f06ab5dd462e320e03bfd1e547bee0ec75887160e09e32daed0da76e8b1deb62 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_114 f2ca48f4eb2482aaaddee893f7078b347ed688cc454964f8709fb09743979671 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_115 401cf27b18b926dc16104bb41881e93b971b092e0bb4baa7d8c1dc471ddf3d83 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_116 501ad5fa1f25c689232ca9e59fbdeafc92ff3c47b9947aba28adb1ec7cb2bb0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_117 2a1bd160e17881e6c7192eeb1518c0a737d62402d5c53d05578bbed428fd46de -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_118 86915c7d7ae3d4b589bff1d6beee93185b7acbbcdc49f0f0c8eac1a5ad88856b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_119 09a20cc662f24c63331896eebe7b662941fecbecbbbf188e0fd8acf8784a8f38 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_120 dcfb66bcf5577df3c9a4cfd358d79f8836e1c3db0b1c8e5755e94f14f8f5ada7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_121 7df9cf5e79d6493699d9906d7a397bb502182330bcc19eb68359ecf1d1253a33 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_122 656e31a321bdf2ae28da5799a69ab376d4aedaf246310f1dfb1665a0623b61c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_123 7c5949fb21413cf3b4e3b729a6f0f0181ef229342ad82158039265ebcf9b400e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_124 1296c164f26e598062047c242dda0a0c83e012f3ae8ffa919546ee21300cf5b8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_125 2606cc61daaab26de78cbf1439ef49bb6e509925fb1b42aa5095b1123aa7dc8b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_126 77d652b76c13201f2b44c3d801dd24885c085bfb9e0ffb5e045e8ef79ea58009 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_127 4b76f2029e702c2c9afa9379b7717ca1235cdcbfe60a47b22d041cc56380d1c0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_128 fa2dd1092613f3c89895ce4c647b5b38771ac6a498251c6d2c6c7cbb6e01e236 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_129 70bc5db7790f973eb091e1716641f6854b73cb6d46074b95455f9592192ff354 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_130 f9b0d25f9337994881c96965485064c79132e64012628c3baa4a2311ddb5f5fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_131 1dbbd6a99babcd44b460f467c1bb8ecde337b189dbf5cb048eab76852b207285 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_132 31af7a6857bce8745c638026ecfd5774e2499c7ff43c2e65b2f70953b7dd0119 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_133 73356723e8ae4922f3a9d33b1abcf835d4443d19b269e3e843d36a49adbd4326 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_134 5d43fd801501458532f2a25a2108d517c1f3ab95c78f8b6808d30632191a82af -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_135 0b9f5afdb89334bb0a9a8fed31d38d72f242a88b3a4c039dfc3792f121ccc399 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_136 8721c9738217cfbd2d0ea35a73f389b8fbd5b516c6b5b16c72265aaa436cc6d8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_137 301220b00438516d3f4bf8977c01ea8a791630a3121f149d9fd5aade0af53bb5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_138 4aa57287925666b42e04a6f93ca8919ff22a88c5adcd99e0e9b183191d3a1503 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_139 882c475e586837c82a45e8e5dec6d13a5f1d62a89e1ab71861a5b60ae0354962 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_140 7dad0daf8207fa2dc867e17c9686de61f9f99df2831362e098f117ab940b6606 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_141 59861141143274047b0c48b2ec9ccd791795d40d1295252eb70c4ad36f264ffb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_142 02de6606dff32f6e763a39861139aecf35eb5abba342ddc27a7a79073ba30de0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_143 3847e83b133e1e1ca36ad86af0b6eccca57595249dabd8411d5760931a20fd42 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_144 f18e276bad6ad3cb94105c676ceee6c184b0e507d87a6294d7d60fbd23a2aef2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_145 35ddafccea86a4e333cda9e7cc56b781ecab73be661c66688d60edea188b8525 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_146 27eaed811846f8c018cb393b048c2c6131c46fba4f2577a9ef8a6e72143d88a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_147 099d4aae407a30151c97cf54cb1dfbfe29cd64917ed59363bca5c29502df8234 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_148 ff92f3bbfcb25b07502bf4db32404d1f5428c147093399f2e9e3ed3bb0678a68 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_149 2c80bd3b0a78e2f2ed5c06b1545bbb42efbe20601230b899292bdffd42d84352 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_150 78f809512c58e3aea83060ddb727f5d985b318e5bf86764cc2988c8e3882d8e6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_151 be7ee4cdd8372de68902afd8560e3d04a65cbb9b6b5b45431b9e422b10a39be6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_152 7817e8be82eacfaa212fe0f3563a08de67f8f2d9fd930372878db95171dc3918 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_153 ac5ceb9e0a97bc3a3a1dc39cd28d90407bf82e0a95d6204e2ccfb06c6092d1ff -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_154 d4b909431898f666fa9b2b488bcd6a9341ebfbd3935c440be039094a67e79d95 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_155 9dbde6818d77d0cb86badfed17525f8d3b512bb192ba1f8894ec45720ef96038 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_156 bbe08466b3edfd8ff9f837fdb7c97e20084c4713b528c847ec2a64357b644c42 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_157 9789f2809cc75c35a6bb72544e743262b4653c2514d950f8f4d2ad306af61da7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_158 dff26c762d43eac16d921f19e2c6a915bc79ae2e915a5fc09cbdf9122d84a569 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_159 34160dd0a1e5576aa5c737f12b3c7d9e8f39cbbf1a660b10829a6f97bcd9f6b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_160 4447bad46498e53f9e7a3334227447ae8fdcd9ba455895523bd4ae3889de74a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_161 17763796b95d67ef1a0c68c4fb20e66da6fd2add9f49c43bc2465ec741a217a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_162 7e4fbe412f30b00b1cdcbbdb4b7f47be95f683998d17d409b19835354e23577d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_163 f026639e0f4d535e347b792e997af9a5651d014fc421104013247687648f4161 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_164 14dbc4989263de9d9cd49c68763accd6b6ebb89d85b460e3cb6e4d30ecf30379 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_165 39ab3879a44fa4c92901b79fcc5724b5bea23b278c9b106a287186ed500fe375 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_166 fa1468104c360cfe025536d79650d8d131b36a005633d10e7d5fe1ed888abc1c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_167 875ca4c861269f872ee5c90d88df19f83e5c6d29ecbb51563bfbc06a083c3f01 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_168 ce97c61f273da7f3eabfde8871c5363054f2860846648328a4e7b246968d5fdf -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_169 08bd870e56b85996b31a569f7429e1c89af81f616727b0d33f7235e0afeb4e09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_170 28d65ad9a3a31d04a37d844be84e5bc56f2746d4de60bdc82a61ac539663ac90 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_171 54a48a1c849b248516396606e10b90a301ac958b37ab1366f09f4ffb7a6f4bf0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_172 62061b688ba7a83408018fd44dc5c1cf9f8fb546123ebf2c562af53c4312f9b4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_173 1a91ad0bc4fc4a8fc3612408a12d3c77a093243a9cf02af6e85294f636da0eaa -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_174 206657574f6be66093d26413db0854f0fc43f485a29a08f81101679dfa29dc0b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_175 5ef81d4e2e36a400ec9697345e2516e5c658cedf68156b9983b4c1401bd904b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_176 4327ca25362111eec108470a9035a8d515d802dafbb72a6217078c7805d566e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_177 d8785126bb249316506e4e204a10c26fdf672ed6baf77961a37746763e7a1add -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_178 4cc1be9ee6e535d2e895f1aab7fec99938cf1fb7251250c09cda7cbb77ac37da -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_179 018c4784f6ff01a4bd34fd96932cda423a1fcc15b9386bb37acd1c9292ed4fff -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_180 abb3cc470fd27787fee7fd30269cec04c30844748b49f7903241b28e79b0f423 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_181 8d0c856f5d77c7c810c39f5db15e761cbffdeb317690e2f7b91a257af788e6b4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_182 92f2364a5b4b36b304bf65402fc93bd48b594dcbd8be2f455f4ba072cedc3c2c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_183 6a8cccd93abe6f60f5a7df2349dce16e97c8570a7bec2f4289e7150957861ba8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_184 1e13b8f23301c941348897e692106eb347c4ae36bab3e3ad7b84e37ae9aceead -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_185 863247d76fde3baeea83c6ac3c456baefb8414bcb4ff91f5602d365542268c6b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_186 0e0c38460068a71a5a055d42cc678fd204f53082d9a83db3a70c2d4947a31be5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_187 5f6d01fcc49ec02d5bb4c9e0e9baa3eb72e59e8484fa70a753bdb6c5fb837dc4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_188 6280959b7f1c468921decc2f9be20dec8918acbe741529851060c10d592ce3eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_189 cc72e20f7654ef8c75c885e81e6bd46e151b4edb671da8f201a005b1fa709c7e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_190 a94126c60c2627c44520471836d281539b5e71431e3889e78ef43af55764a331 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_191 b96bbbf1038435fd998aad9fef2022f0d002d3dc02b0c793efb1afc2da58b1e3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_192 4cf4a93d9d59ed8242c951c51bfb0b4d6cec96dbaa0e23e83ece46ad8f13cbe0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_193 83ff9b2674de4f4fa4448773b34482fcc3b3ef37e1d9f498320e1d10bd61b431 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_194 89528a01d7d3955924392d652150c983398efcf0a3588dc69ace8922e37baa99 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_195 aa44822586e4daa75538865ea69bd4a7f3e40fc242fa086b6fc730a7d7550088 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_196 da1144d0920c3df09784e9d728d151b0da23b5b4430ac190aec67d97aaf1d049 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_197 a77bce049b55f55b2a50f0daa7040fd182e0e05c65279d6de7e064a8187fec31 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_198 1170c0882b3d28fb96e914cff58d0d467e5b201ec4e31ea28c97665c503fd150 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_199 3edff191e5787dadd82dfd1d8395b79c45721455454a3918ac1031306d7d3c0e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_200 a495f86be3a48fd82d30fabc2897bb2ddb8b83326837ede20ae61163ddfb02bd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_201 36f5725050f0ab217c4d586592a5c52fcb8100a16071a5e41de7abd604bc3989 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_202 164851425f96f0c63628a6f42155b2b243045d28a364907268c24a50b7402cd3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_203 fe6155e192d0fc15112a3cc7f9a6fb058ce086dfbf4e249ce995e91aa44c8e3e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_204 e85f6829d9e5c03157f356617ca9e7432eb2a3ed9971f322cd280c9adee1ca60 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_205 0dc7aa8212fcaaeab91579a02ef050cbf4d6a1c203dc02af03f49b8fdb28047a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_206 5bde5f45ef6e40890894f9be364b197202610da988f2d2e090619846bf436e6d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_207 99e7180979c4c6391bec1190e6b9193b0fbd04a4fa973a42f529d96c44d13479 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_208 19a280b1129b9c9f767bd8dec502c156ef9e6fdfa290611b3404d448ddc99512 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_209 41ae6802281d30e410862e0a04e2733062919511fa2e590aa747432653744028 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_210 9be7b669721f758e9185479d125e0365c99058851c718965626b68f1562a9b4c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_211 20ed824e285aed322c44c40c8d11a271e83233cb43b35917afb9b1a610e66f09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_212 6035dd1f2e70787730c8108165ea0127df8e9df7262b5e3874823b33d8d668be -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_213 2ef364e71dc008f3ef5f71858aced85e4851ddf73ed3d49140bd86434e0d4a67 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_214 e385bf9cc6702562a9dbaa92e4d5066b56560fe238fc76bdcb17e560e5ea57c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_215 8f44cb4bf7080f7f6703d76efbf322f61a224515415ff71d86611c5b39e38bc4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_216 23c8588f8430ccfda21327d2ea8535b89f0005e74f5b3e8fb1528aaa038da2c5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_217 0c2bae9e71df4b7571bdce544885183f8b4e177e38760b907f58bdc233e125f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_218 1ea22574a8e0f1000ec6fb888e630fdb2c8bcfe4fa5dae68fb7ac7aee69a2746 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_219 c2be17c05ef4709177b0d746e8f75ec478d3767ab4fcd56c7b300664a1e0da0a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_220 53247ab6e8b711cb508c5d4dae2ba50116b2275e7dcfe6e5a27fa31cbdcc1919 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_221 34fd3fd69a71ed529bcf5f1ffbbbceac2b22233023113df34e3bbe2346419ab4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_222 0751e125947b5150a9bb8492e99e29398a5a9070e2a1b50a83d4398b99dbb4f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_223 03b61919e14cfd736f2d18f9b2b45b71eb8a0c0e015a4285bf9238b9f421e86a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_224 509a993aefc72cd24edba346e978550b159be4a0b57b796a59485284535d3d47 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_225 15a01ce9eea6e8d48d5cb94cad93f16e910ed804afcd4ff52b2579fc70ff8736 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_226 8a0ce36dba89c258818e967b0b798478efc7756767df731c550e49147d88cc3a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_227 a6df7522b02ad4108d7b89ef3780de9977e3e98ad2499929e3091334ccca0c26 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_228 288bfaf9dda3b2c6f20d00f035f99fffd98101bed990dda49422d3d8b716c031 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_229 d83bb5ce3ddbe3e13fd37b5bd68d2b550476546a65ff4f94bb2969feba30f763 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_230 c21c949fde9ceb48880dd81abcc8b31f4ba83893e87003c052d5a058d2d71e9d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_231 2787700caedf16859edcc193ed55fd2e8d252c26ed6a08b3315778afb01d2876 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_232 3aef73a2b75712e94a1ec5f2dc83d410792595fb47d6a203331fefe066f7a0be -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_233 3a7c11cc84fdbcf8eaf70d6f51bc31dce1fd20730ff1957e10effc822bb90c5a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_234 e05809b99d19a751bed2f19b6a7c3f0d7ee6255d3b59b24f9aa817033fbec4da -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_235 46f40edb5206fc53fac800c9ee2b59a4cdec794550797547b034b453417c3292 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_236 fcce273403d6a359aa827fc5345a9641e7e98ca447c8be0cd4f6f8d4434d88d5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_237 bd79f9e0ef40f3ae7b00a9a1d17d9ff4f8d41815dc0d136adace638bf9bfaaa5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_238 b1a745927a9abac80a7a3ff60f3f6f29d1745bd046ba14d68ab9b9b9cc68ec61 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_239 b532367f893a26e8b93c3f0cfcbcc1a3cec74dbfd80580d0ae8e4380f0441040 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_240 94ae986ec4f06ac7fec9d1a9616550441a0fcb9cc692c915352b103305fbee33 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_241 cffc0613610f8c10847b22c3c3d592adfd2c31cdf62f866a81a5bbecb2c12d67 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_242 0b76a22a94ee7e033331b31cfc610ccec875d65d2585766936d8971ae62f4faf -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_243 ff536d065c96bcbea75aa8559ac11810de96d7995714df90306e753a0c8c0782 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_244 be61056f362850bc0ea31ce20a247cc7372c24653094b777046a0813e41501fd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_245 b10d19873c26c2921de90e808818a9bd179dc707036d9f37b6c8aac748995db7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_246 701e107f018fbbf74cb3bfca702d048563eca0c0ff0711b330b6e4e7192a644d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_247 6b82cebb5f6b9ff0df19f676e5e26b84ec88f26ca1ecfbbf1f61e946dd11646d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_248 b21745341928c5a600c4437471dbdaca779102c3a14df5d99dcd089b998ea624 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_249 0f29d387f11678d5703cddd5bc5db43e23e80741802ecf15fdcc5e5f43144dcc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_250 e21f85560a4ada6608076bb787e059eefce9f37cb2af46bd9c0c9afc8b997dc8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_251 2e6ad9008e6abe1aaa0ddd28bcd043e8db0d9cc0bdfc4bbab8cb021e088ab0f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_252 2a9cc7524f2d50da1f842d05ce98d66f9620b1891dcc2d7d76709792ca99d472 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_253 8c756a0e36f7c959169de10a1623778866f6ea2b3955d56cdf2b461ee3d433fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_254 0b9fa36d4f9c2083a9206d45b1e7bd71dafa1e0dcf7bdf8183cb56b23ede18b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.InputBuf_255 00f8bf245f13cf18b9c25333cc930618b6a402514f975916ab31381e288de98e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextFilter.InputBuf 64e8cf22b53faadf7ca0b40975d9147ead4498ccbffde0c1bd34caed0c64ec86 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.Filters 6d1cf45fdc63c1d53195a33b44800ce471ab06635579a05b2868b26e9fc487a9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilter.CountGrep 5156ec524b3048ebb151b0c535f8c6bc8fd11257f5f79241978286cd8b4b0754 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilter.#ctor(System.Byte*,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiTextRange},System.Int32) a6dc3876518d90913d877695849a5de9f4ba3ba8a36c350c07afff78f80eebf3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilter.Destroy 10ea73d42f40280e977771ce40ecf5662c77beeb02d9771252f6c923234391d8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilter.Draw(System.Byte*,System.Single) 481ad2eb70ecb760e42ee3b6daf04b1a10c6d70f1df7774c145a2df3d50206e2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilter.PassFilter(System.Byte*,System.Byte*) af7e9bfcea62f355d29986830a60c513622a26118b093a85a3baf00fadc4679b -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilter.Build 4ef26313e161ca6433195487e672d16e34355855acba98443694289ace166943 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilter.Clear a37623e580d2e547870685923c9aa7f2604686b481b486fc2010f5bd4c53e8a3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilter.IsActive 6ee1a73f635a07bd1a8597e5e9f8450281c2e558ced04bb46e7a67550284260c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7bf43086f84a6342"></a>[`public int Inno.Native.ImGui.ImGuiTextFilter.CountGrep`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9865) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d619f698ce1b9fdc"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTextRange> Inno.Native.ImGui.ImGuiTextFilter.Filters`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9864) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2018b7992ffd2681"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9606) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c61efc18d5f25b70"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9607) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-461a558a7d7610b0"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9616) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e0920a115626718d"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_100`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9706) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-82495f61af65b042"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_101`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9707) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c37a629528ca097"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_102`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9708) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-36608535bf2b97a0"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_103`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9709) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e879667238b3d877"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_104`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9710) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f4757bda3d63f827"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_105`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9711) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a896674c0a06f9a6"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_106`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9712) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6376e017ab606be0"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_107`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9713) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d18541d842358b25"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_108`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9714) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-79d2bfb0fd9b57ec"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_109`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9715) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-79bb57e115018021"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9617) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c6ff5c796afdf265"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_110`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9716) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ff6123a4d560bbb"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_111`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9717) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c622e8d15d41c68b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_112`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9718) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ff68ccf5d60fdab9"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_113`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9719) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-72056db461e0204a"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_114`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9720) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-96334b0e384e11e0"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_115`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9721) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6628a46aaa9f5a2b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_116`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9722) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-98a0f1c1f6bde5bb"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_117`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9723) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d3c4736c284a630e"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_118`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9724) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0360375446a8bdde"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_119`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9725) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d894dac4492c6346"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9618) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-92bb988d6f3d8936"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_120`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9726) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ee8d51d23d557bd"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_121`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9727) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5f2376e77a8b0a9a"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_122`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9728) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35203351e46a5f78"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_123`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9729) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e1fb25ae9d7bbcac"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_124`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9730) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1da7b51b4e594511"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_125`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9731) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3c821359a685667a"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_126`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9732) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-361f3b5f7420efdf"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_127`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9733) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9950253d33892d04"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9734) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce901771f581ee32"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_129`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9735) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6dd436c7b02a4819"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9619) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f73c9d555d59f762"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_130`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9736) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6ba513673b6d7ef2"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_131`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9737) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-786505137e771a3c"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_132`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9738) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fba49b892cf88cd0"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_133`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9739) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-98fb2824975772bf"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_134`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9740) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e5b38f0d87858d12"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_135`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9741) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9cc54ff2748cd75"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_136`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9742) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53e715685e47e29b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_137`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9743) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a3a02fb581d4f67f"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_138`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9744) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-660a318dbda4cd0d"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_139`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9745) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ab66a2b39c649e5"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9620) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-488722a6019497a9"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_140`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9746) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7cdee0ae032b71d3"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_141`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9747) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fbc93b0f19708054"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_142`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9748) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13cc01c30641e653"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_143`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9749) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a2f27d57ed9a08a0"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9750) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1422823ff4de0ade"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_145`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9751) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-281f5dd9356f1aec"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_146`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9752) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7e00b5fc4082ce08"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_147`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9753) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6d9d7ce9c09bf92e"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_148`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9754) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-31989f84696bcbac"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_149`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9755) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-15f57495629499fc"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9621) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a0085e5e87a424cc"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_150`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9756) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f26bf5782c484aea"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_151`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9757) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85a76770226d5ceb"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9758) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-46e4a54dac0f006c"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_153`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9759) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-169434df33313f32"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_154`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9760) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7e6a92a3f4706b12"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_155`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9761) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eae7077ff2f39b72"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_156`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9762) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-262b8f5d4ec1720b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_157`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9763) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfdb1a0cfb312165"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_158`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9764) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6915ce176532544f"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_159`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9765) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9532f69c7780ab8c"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9622) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5af3556f0306839b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_160`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9766) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-782c2f0fafc8c7ed"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_161`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9767) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-090875ffed4d1dac"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_162`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9768) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c36cefef7b1935c6"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_163`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9769) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-893330f51f9dda54"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_164`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9770) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01f5f43276c73453"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_165`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9771) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5445d72f6d0490b7"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_166`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9772) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0fd913b30fc47508"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_167`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9773) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-823f904559d7d00c"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_168`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9774) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc49dfb436107c46"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_169`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9775) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54659ebdd53e7bfe"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9623) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-48019dc1e8a58751"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_170`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9776) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-05add49e630f8984"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_171`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9777) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2726b208082da084"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_172`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9778) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8393ea08cb707c3f"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_173`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9779) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-610c80ea24af9b51"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_174`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9780) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f33329ceaef36a64"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_175`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9781) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fed765eb0a67db94"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_176`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9782) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c7f62b66453102e"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_177`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9783) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a1b74bccf140b70b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_178`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9784) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f35987d1f15f9a29"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_179`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9785) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-212b7989735b28f4"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9624) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae3936c8e796310b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_180`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9786) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a0594cd6ff96721"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_181`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9787) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e435b7a153811984"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_182`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9788) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-804c3bda46322366"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_183`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9789) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85833707277c1288"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_184`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9790) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-735afab3b7eae435"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_185`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9791) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eaa268e122f66dbe"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_186`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9792) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5af6a1031d9cdf5b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_187`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9793) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-72c05dc81af93ec4"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_188`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9794) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-09b6a09bde36bea4"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_189`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9795) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d19478c23247aaba"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9625) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0871138f30461e17"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_190`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9796) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c25668b65b040e27"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_191`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9797) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8a0a415be3af65f1"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9798) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a02fb82a8a390620"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_193`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9799) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-71c54a90194549e1"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_194`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9800) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b465a6ef375a3969"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_195`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9801) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6e2826ba7ea238fa"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_196`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9802) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dec5c2a56bcd524b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_197`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9803) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b0638758f4a7a882"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_198`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9804) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-729d79c07147e3a2"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_199`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9805) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe6fb5a1fef90597"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9608) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9ecc7854f2d9df29"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9626) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a0cb7c3126950441"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_200`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9806) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-84704b6417adc4f8"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_201`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9807) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-326a71bb68320205"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_202`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9808) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-935ff1310789ba47"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_203`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9809) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-132eec95ad4b65a5"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_204`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9810) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4b01086ab5a99723"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_205`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9811) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bdfd7b753add0ad6"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_206`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9812) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-71ac97d43378981b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_207`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9813) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3bc9d240782726db"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_208`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9814) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5df69c68ea027858"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_209`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9815) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13100899903d491e"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9627) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1a3441054334a890"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_210`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9816) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d69e1cdb87e3251d"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_211`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9817) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eaf8ae0501750013"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_212`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9818) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ed376bbd36f7879"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_213`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9819) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-96207d3174932458"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_214`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9820) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-22598f4226b425a7"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_215`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9821) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-02ebfdd9ae2faa82"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_216`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9822) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6948a46796adc40c"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_217`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9823) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b0e8ceebf5d8408f"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_218`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9824) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d0b16393ffe0cd71"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_219`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9825) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-670138bfa454e84b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9628) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fc4b8db5369ccf4a"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_220`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9826) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a14521dc07e2171"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_221`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9827) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-45d472e821a5e2b5"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_222`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9828) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2611a53e0550ba2c"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_223`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9829) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b7c3956e7233bcff"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_224`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9830) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b2e9f11710a4c664"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_225`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9831) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21ab93d174119bc4"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_226`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9832) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-91f7acfd4a64b798"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_227`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9833) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f38a7952d482c1a7"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_228`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9834) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-15469702b3d647ac"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_229`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9835) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1fb71b9801baeed6"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9629) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ca0306f06c6658f2"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_230`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9836) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9506a1a0fc4ad6b5"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_231`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9837) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f70eef58fc19a499"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_232`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9838) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aaddbd7bb788cc84"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_233`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9839) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de93fff1900fac49"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_234`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9840) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb593e653ca0a3f1"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_235`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9841) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8d7e4f1ef1e43ecd"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_236`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9842) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-977073fd3ad8731e"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_237`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9843) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-18b7d3efe1be28a7"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_238`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9844) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5202afc980fb2acf"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_239`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9845) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a762faa3d3f28b3f"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9630) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8abe3c602b857a4e"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_240`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9846) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8a8093ecfefa7d15"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_241`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9847) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d91f70c543a643b5"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_242`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9848) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e17011ffacc643a0"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_243`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9849) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-91a3e7bf200dfcc1"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_244`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9850) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f3066d3da112e68"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_245`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9851) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-251199add5a1d0ff"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_246`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9852) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b80940b8aa4e3bc3"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_247`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9853) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03ea55465f9fac73"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_248`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9854) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-689d17856e11c30b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_249`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9855) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a289dcd8bd3727a"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9631) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-84dada8edce476a2"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_250`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9856) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2996cd59c6e14d47"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_251`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9857) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-327755abf38ccf70"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_252`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9858) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6edb48e340b93825"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_253`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9859) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7c316ff36300af3"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_254`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9860) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d63a911910e4192f"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_255`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9861) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e09a8af85f7ccd4e"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9632) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fd23782b483a68ff"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9633) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aed37b9b6928c2ac"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9634) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03bae213c122c321"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9635) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7f757c0c80f08d7"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9609) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5bb4726d5f300fe3"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9636) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec1ba02720dd0ac8"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9637) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1da24a5c57db88da"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9638) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-65468a77d932480c"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_33`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9639) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-818b31d80b5e15a3"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_34`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9640) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-02b82c048de6c8c4"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_35`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9641) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c71f1a22732372ba"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_36`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9642) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de1ffc27b3dcfa99"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_37`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9643) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6fb6f6bf1ff80230"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_38`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9644) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3fd5c4f95c911443"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_39`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9645) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-435429e040d9d1d8"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9610) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed376f249d0d353a"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_40`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9646) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d068aaf68732f778"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_41`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9647) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-26ec48d5b4f06a88"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_42`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9648) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c388294ae637ff0"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_43`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9649) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fff1dcd3591f2afc"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_44`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9650) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-18ec1629357bf2d3"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_45`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9651) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-84d499f8b864721a"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_46`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9652) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ffbd30fe54f86dc"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_47`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9653) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7239c559db10d733"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_48`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9654) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cb2bd4d4aa0cfd30"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_49`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9655) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f87247682edcc237"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9611) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dffe8ae1bea5a580"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_50`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9656) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ba023ed33e3f609"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_51`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9657) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-93339a8813f41eff"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_52`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9658) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c6b77a3a9b25c8ae"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_53`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9659) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f41f97ddae67e37b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_54`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9660) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69cf83d73d99f4e6"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_55`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9661) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1cae40750fb74f1b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_56`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9662) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4b9685ce9c60491e"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_57`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9663) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3c34d9e63101904f"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_58`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9664) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-713de1fac061f350"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_59`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9665) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d1d9516261ca334"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9612) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5f48f0afeb51c4e1"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_60`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9666) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af6dbf5e36aae52d"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_61`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9667) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-847dc0419cd9f7a7"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_62`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9668) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2279a914252bf28d"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_63`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9669) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e1efab4125140a0"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9670) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2763eec31ca33c0a"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_65`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9671) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f73124e25f1c4d5"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_66`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9672) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-afab1d2421624180"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_67`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9673) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c4752481c73a2b55"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_68`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9674) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d609f007df55da0f"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_69`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9675) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b6f11fb72074a47"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9613) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a2b5475005568ed2"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_70`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9676) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54d20c6a3a95ba20"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_71`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9677) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc783e7e148f5f26"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_72`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9678) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d1f219e84e9e07c"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_73`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9679) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea63a5b450505b4d"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_74`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9680) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ffdb1c8b0675180"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_75`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9681) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27f3d25ae9ea84ec"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_76`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9682) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6d75f0d193fe82c5"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_77`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9683) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13e54f04e1c06c15"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_78`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9684) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6d5d611a2d8c8276"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_79`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9685) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a3d87ba2bdf1b66f"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9614) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a83c39601dfb63cb"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_80`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9686) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-32390f6cd8c07867"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_81`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9687) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69ebbc6c7f3c1955"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_82`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9688) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b03e540bdaadcb53"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_83`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9689) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d8a25fff5398472e"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_84`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9690) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-686863f73e2af0cd"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_85`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9691) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9751c6fcc2cffcdc"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_86`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9692) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c41403b1d0fe3214"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_87`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9693) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-269665dd50fda607"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_88`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9694) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4ab7eed3e930855"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_89`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9695) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6ca026665674b43d"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9615) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0b847173c6d0de91"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_90`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9696) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-51856c46f5179cac"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_91`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9697) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9ca9789dcb1ae5c9"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_92`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9698) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2c332fef3caea78"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_93`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9699) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-94e384afe766aaaa"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_94`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9700) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c656c007d4ccd137"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_95`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9701) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-75678fa771e5cd9a"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_96`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9702) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fd3f92ab4d8057e2"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_97`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9703) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd0c6ae6170f7de3"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_98`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9704) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f13a8a53fe83892b"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.InputBuf_99`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9705) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be82b77b32030db2"></a>[`public Inno.Native.ImGui.ImGuiTextFilter.ImGuiTextFilter(byte* inputBuf = null, Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTextRange> filters = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTextRange>), int countGrep = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9866) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1a89ac559eccecbc"></a>[`public void Inno.Native.ImGui.ImGuiTextFilter.Build()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63636) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b64711ceb78859a2"></a>[`public void Inno.Native.ImGui.ImGuiTextFilter.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63647) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7bcb382d9dda7ad6"></a>[`public void Inno.Native.ImGui.ImGuiTextFilter.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63603) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ad1152f6a4e13464"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.Draw(byte* label, float width)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63614) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1c4b52e710f8e8d4"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.IsActive()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63658) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a17b682881181cb9"></a>[`public byte Inno.Native.ImGui.ImGuiTextFilter.PassFilter(byte* text, byte* textEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63625) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-69c8950132ef51dd"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiTextFilter.InputBuf { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9862) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e0b6d9094d99df56"></a>[`public struct Inno.Native.ImGui.ImGuiTextFilter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9604) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTextFilterPtr</code> · 26 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTextFilterPtr 64d1b67a21844e3292392e68bea1d28819a9b4c9eaf85aafbccf70596e9058ef -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.#ctor(Inno.Native.ImGui.ImGuiTextFilter*) bb3655b4eaf37c1d8fcf176d8882f4f48d054c8d1ae0ab20d00ffd1b6da90a4d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFilterPtr.Handle e0eaaf088fa6a2361bb8aa7cc615042567bc2eeba47c3b789d123db02ed51419 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextFilterPtr.IsNull 5718e0f08186e550a9a2153df98fe50f78d4858b51fbdf2ba2539f3006249eb0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextFilterPtr.Null 03ccb682dadcb03f49fc638cd70353c7b5bb0b2f6bd1ed4136ca01cdb01e76ad -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextFilterPtr.Item(System.Int32) 47d06193b0d430000bd29282845b51044f4ca2a5fb11c70e3aa114a4e064cb8c -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.op_Implicit(Inno.Native.ImGui.ImGuiTextFilter*)~Inno.Native.ImGui.ImGuiTextFilterPtr 867d355e0bcc0dff600faffd61ed0897d36575ef7e916a445a9aafb899f42be6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.op_Implicit(Inno.Native.ImGui.ImGuiTextFilterPtr)~Inno.Native.ImGui.ImGuiTextFilter* 78a01a12ddb50b35966ccf000e713727038e1bd1ced9074b40640ab6dabb28b1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.op_Equality(Inno.Native.ImGui.ImGuiTextFilterPtr,Inno.Native.ImGui.ImGuiTextFilterPtr) 41db7ef41bde98f46710aabaf1b3b33ee2d1d3855249d8f0a2637389f2e07bc8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.op_Inequality(Inno.Native.ImGui.ImGuiTextFilterPtr,Inno.Native.ImGui.ImGuiTextFilterPtr) 29d4b867c35b690b857586d1e270ed34e2b255b86fd6524bf38745dee78b8d2b -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.Equals(Inno.Native.ImGui.ImGuiTextFilterPtr) 2b5c4eb0e8519c5d422cecd22335096a483023d40ac32f9ccc916db18acc6984 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.Equals(System.Object) eb63857071435edcc3814f3939b15fa8b71d6343cfc458948a6687d4ad2b03dc -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.GetHashCode 924cae2ce815106e315703515e1e275e01fe5087d724249cc99856a097794b16 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextFilterPtr.InputBuf 812e3acde58c8fe1aed4be2a17282f17426611363684795cd1a1d8379dd48b42 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextFilterPtr.Filters 2bd6f5b802fe40300976ffeb87e6839e8a962995c132c87488461099e67d7ddd -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextFilterPtr.CountGrep 3e46fdded2a11ea4a6299a77d8519fc9e8d3190ccf65a584635f7be9a43154c4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.Destroy 5c7997dbf8680b4e9e12efcbca850e281e90de67702ec8b2c4b55b96d7769713 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.Draw(System.String,System.Single) 645d5cbf55bdbb0a2e4e9e6469aad2244101147cdc7a35cae65e32816e029e49 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.Draw 1e74dfca46145ffcacea0a25e15103f01b5bdffead3c8402a4d5d32b6dfc8de3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.Draw(System.String) a66c4d1896eb0b9c646859371b024aad556d09bb20893a77a9221acc1a6d8b60 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.Draw(System.Single) 6213f2bd14f49125e410c1f10915545e00881dfbd3dc2e2eccf76a62badca0be -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.PassFilter(System.String,System.String) ec57dda68e1564536a7d1cf62c13fd0a03fe5ae2748226e334e35e9f6da4d21b -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.PassFilter(System.String) ccf3222c3ce583db07ed22c6738d8a6aed6c5a099b8c3fccb21e9c1681e73fc1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.Build 92a085ddeb8e457d0ee3c10c92f9ff69a1d8aa982c67a23aa631f475228da05a -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.Clear a0b07b4a2801689d3ebd8d5f0b095e2db11a51f8a26f3e6b792e829fdfe442eb -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextFilterPtr.IsActive 9127d9d532ad65597b034d159cf05030a1a44bd054b0f585ebe655062fd4dd28 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ab9cb21b4db728c4"></a>[`public Inno.Native.ImGui.ImGuiTextFilter* Inno.Native.ImGui.ImGuiTextFilterPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17374) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3e18ab7fc8de3e65"></a>[`public Inno.Native.ImGui.ImGuiTextFilterPtr.ImGuiTextFilterPtr(Inno.Native.ImGui.ImGuiTextFilter* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17369) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0813150c3ac697b7"></a>[`public void Inno.Native.ImGui.ImGuiTextFilterPtr.Build()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17451) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ac50ab58405523b4"></a>[`public void Inno.Native.ImGui.ImGuiTextFilterPtr.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17459) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8fe113b6270f9fac"></a>[`public void Inno.Native.ImGui.ImGuiTextFilterPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17395) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e060d3f2e5a18b1f"></a>[`public bool Inno.Native.ImGui.ImGuiTextFilterPtr.Draw()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17411) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-67371b1ce9f679bd"></a>[`public bool Inno.Native.ImGui.ImGuiTextFilterPtr.Draw(float width)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17427) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7cc61fd577e2f49d"></a>[`public bool Inno.Native.ImGui.ImGuiTextFilterPtr.Draw(string label, float width)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17403) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cc0e20b74b1a3c2e"></a>[`public bool Inno.Native.ImGui.ImGuiTextFilterPtr.Draw(string label)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17419) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5e4c5520322c9017"></a>[`public bool Inno.Native.ImGui.ImGuiTextFilterPtr.Equals(Inno.Native.ImGui.ImGuiTextFilterPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17384) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-2d6e7587ea162ff6"></a>[`public override bool Inno.Native.ImGui.ImGuiTextFilterPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17385) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-ae92bf81f2d03f3d"></a>[`public override int Inno.Native.ImGui.ImGuiTextFilterPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17386) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-7bff069beba8a0e4"></a>[`public bool Inno.Native.ImGui.ImGuiTextFilterPtr.IsActive()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17467) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c8dbc6a4f0bf8b3f"></a>[`public static bool Inno.Native.ImGui.ImGuiTextFilterPtr.operator ==(Inno.Native.ImGui.ImGuiTextFilterPtr left, Inno.Native.ImGui.ImGuiTextFilterPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17382) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e72f1fe912ab7bed"></a>[`public static Inno.Native.ImGui.ImGuiTextFilterPtr.implicit operator Inno.Native.ImGui.ImGuiTextFilterPtr(Inno.Native.ImGui.ImGuiTextFilter* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17380) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9ac38dc92ce8176f"></a>[`public static Inno.Native.ImGui.ImGuiTextFilterPtr.implicit operator Inno.Native.ImGui.ImGuiTextFilter*(Inno.Native.ImGui.ImGuiTextFilterPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17381) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5682551ac360917e"></a>[`public static bool Inno.Native.ImGui.ImGuiTextFilterPtr.operator !=(Inno.Native.ImGui.ImGuiTextFilterPtr left, Inno.Native.ImGui.ImGuiTextFilterPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17383) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-93b201996c689b4e"></a>[`public bool Inno.Native.ImGui.ImGuiTextFilterPtr.PassFilter(string text, string? textEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17435) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fa6404ec06f9a954"></a>[`public bool Inno.Native.ImGui.ImGuiTextFilterPtr.PassFilter(string text)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17443) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0e5d90cff9e0cd11"></a>[`public ref int Inno.Native.ImGui.ImGuiTextFilterPtr.CountGrep { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17390) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-34c4e22414b7e580"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTextRange> Inno.Native.ImGui.ImGuiTextFilterPtr.Filters { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17389) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-253f72329027b98a"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiTextFilterPtr.InputBuf { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17388) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-779db4688b85ad71"></a>[`public bool Inno.Native.ImGui.ImGuiTextFilterPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17375) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-42ede98f01698403"></a>[`public Inno.Native.ImGui.ImGuiTextFilter Inno.Native.ImGui.ImGuiTextFilterPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17378) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c63f5d094dae5fc6"></a>[`public static Inno.Native.ImGui.ImGuiTextFilterPtr Inno.Native.ImGui.ImGuiTextFilterPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17376) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-40e2106975660a97"></a>[`public struct Inno.Native.ImGui.ImGuiTextFilterPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTextFilterPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17367) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
