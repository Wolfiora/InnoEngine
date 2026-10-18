# Inno.Native.ImGui ABI · ImGuiMenuColumnsPtr → ImGuiOnceUponAFramePtr

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImGuiMenuColumnsPtr</code> · 25 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiMenuColumnsPtr 05c92ba1d6320caba2e7dad8462aeac293aff1cf84f093748552c9018f01a33f -->
<!-- api: M:Inno.Native.ImGui.ImGuiMenuColumnsPtr.#ctor(Inno.Native.ImGui.ImGuiMenuColumns*) c9ff81383cedad6fc9936961118fbd9c88b5d4302b8474bbbc0813442ad9f456 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMenuColumnsPtr.Handle 8e3069a371f8d2a0252ecd8262412f6931ce69918b7ec0fcd86e9471025146a8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMenuColumnsPtr.IsNull 2ede5888fd02517be9138e54a97ce1adb89d11b84f11ac1e690140a9f2d77fb4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMenuColumnsPtr.Null d206482beef248e97d005377eb6f60402b9d5fee5e1634decbb64e8e8041a88a -->
<!-- api: P:Inno.Native.ImGui.ImGuiMenuColumnsPtr.Item(System.Int32) 290afe21db02f0fb1e5d95197f372fa8d8c47c821c10dfe3714b3f7ca56bf35a -->
<!-- api: M:Inno.Native.ImGui.ImGuiMenuColumnsPtr.op_Implicit(Inno.Native.ImGui.ImGuiMenuColumns*)~Inno.Native.ImGui.ImGuiMenuColumnsPtr 39c56ed26bd7b5d7f24e818ca845580e122c43f04019ae545c3bced65bd6f1b8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMenuColumnsPtr.op_Implicit(Inno.Native.ImGui.ImGuiMenuColumnsPtr)~Inno.Native.ImGui.ImGuiMenuColumns* ad5bedcec0d7eb3d2d8e6eba338e9b3e4b8fff320bde87f48bc1623f8974f977 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMenuColumnsPtr.op_Equality(Inno.Native.ImGui.ImGuiMenuColumnsPtr,Inno.Native.ImGui.ImGuiMenuColumnsPtr) dd769e6e3c9a9d9d4865d3d16ff8d57e576382756764224828b0d618d6aec90e -->
<!-- api: M:Inno.Native.ImGui.ImGuiMenuColumnsPtr.op_Inequality(Inno.Native.ImGui.ImGuiMenuColumnsPtr,Inno.Native.ImGui.ImGuiMenuColumnsPtr) 899a2556e6e2df279b10eef304db2b566979eb815957c520e61aba4615cbb78b -->
<!-- api: M:Inno.Native.ImGui.ImGuiMenuColumnsPtr.Equals(Inno.Native.ImGui.ImGuiMenuColumnsPtr) 4fcb4416cc14adfd0779183a6b31b05d701ebd3fe6744dcc5e98b3d04d809012 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMenuColumnsPtr.Equals(System.Object) e2e082dddd9380cfaa140bb0236dc18dd14f9ec18bb0c212edab6475ed68c0dd -->
<!-- api: M:Inno.Native.ImGui.ImGuiMenuColumnsPtr.GetHashCode 7ee3f3804bc25d05db11a74fcc7495178015f3f7dcea64dcad56257c3507ae79 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMenuColumnsPtr.TotalWidth d66ed92fca533969712cd67837a999d37bc1ac8ba42dcc6e3781de2ca06fd80c -->
<!-- api: P:Inno.Native.ImGui.ImGuiMenuColumnsPtr.NextTotalWidth 72e218a405e73f0623cbae2fcd89e3454e6af28d1ca4d30c34ceb207d07195a0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMenuColumnsPtr.Spacing 1b5470449fdfa34431ed158b3b0577d24261e794f5d7b7bc8990ff3154aa13e5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMenuColumnsPtr.OffsetIcon e7f74a84a8f0eea0f9d3dd24dfffce17b87f826b7b1c923bb526ceb16dd7e7a0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMenuColumnsPtr.OffsetLabel 2a13bda087ddc5e1edcd21e2afefb2ef79337528a97f191f2dbf2589bcfd49d3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMenuColumnsPtr.OffsetShortcut 89760cffececbd31ca226c6592f2a737e6e1179a6eadd6b63593c650aa017753 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMenuColumnsPtr.OffsetMark e411979f51b4df6d9c9db4b0dcadfd987e2b24f34bba2691eb2e31696cf9a4b5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMenuColumnsPtr.Widths 39ba5a93b5278839fddc3efa70a0e36832818a733d7859aede5286e6dc356435 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMenuColumnsPtr.Destroy 86b4d870db490416e785ad2b081571fbebc331652cbc6d31db012957fff2436a -->
<!-- api: M:Inno.Native.ImGui.ImGuiMenuColumnsPtr.Update(System.Single,System.Boolean) 13eea94c09fbaf648829a4552ce6c9fd6c9c2cb6289bbb0485f2d0c29c3b2a7e -->
<!-- api: M:Inno.Native.ImGui.ImGuiMenuColumnsPtr.DeclColumns(System.Single,System.Single,System.Single,System.Single) 7c854de4f5d9cc48ba9eb96bd5da3af07891cb2a09b4dd277f383b2ebc1104e2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMenuColumnsPtr.CalcNextTotalWidth(System.Boolean) 092b3974ef8e5c0797352c84e21a79902f9b7642cd048d20c55e45747f74e60f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-82e231db3fe918ce"></a>[`public Inno.Native.ImGui.ImGuiMenuColumns* Inno.Native.ImGui.ImGuiMenuColumnsPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15177) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35f3e0ec0cd9a84e"></a>[`public Inno.Native.ImGui.ImGuiMenuColumnsPtr.ImGuiMenuColumnsPtr(Inno.Native.ImGui.ImGuiMenuColumns* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15172) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fce1705758d65607"></a>[`public void Inno.Native.ImGui.ImGuiMenuColumnsPtr.CalcNextTotalWidth(bool updateOffsets)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15227) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3cd2cc33891ddd02"></a>[`public float Inno.Native.ImGui.ImGuiMenuColumnsPtr.DeclColumns(float wIcon, float wLabel, float wShortcut, float wMark)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15219) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-80089274aa8bcee1"></a>[`public void Inno.Native.ImGui.ImGuiMenuColumnsPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15203) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c5edb406b30cc5a7"></a>[`public bool Inno.Native.ImGui.ImGuiMenuColumnsPtr.Equals(Inno.Native.ImGui.ImGuiMenuColumnsPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15187) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-a1288c4561eda440"></a>[`public override bool Inno.Native.ImGui.ImGuiMenuColumnsPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15188) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-bf8be4d15c41e2f8"></a>[`public override int Inno.Native.ImGui.ImGuiMenuColumnsPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15189) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-be43167c0010863b"></a>[`public static bool Inno.Native.ImGui.ImGuiMenuColumnsPtr.operator ==(Inno.Native.ImGui.ImGuiMenuColumnsPtr left, Inno.Native.ImGui.ImGuiMenuColumnsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15185) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c00585a14e56274c"></a>[`public static Inno.Native.ImGui.ImGuiMenuColumnsPtr.implicit operator Inno.Native.ImGui.ImGuiMenuColumnsPtr(Inno.Native.ImGui.ImGuiMenuColumns* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15183) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e5a3e63671358a01"></a>[`public static Inno.Native.ImGui.ImGuiMenuColumnsPtr.implicit operator Inno.Native.ImGui.ImGuiMenuColumns*(Inno.Native.ImGui.ImGuiMenuColumnsPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15184) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-47a710d4a2cf7a7d"></a>[`public static bool Inno.Native.ImGui.ImGuiMenuColumnsPtr.operator !=(Inno.Native.ImGui.ImGuiMenuColumnsPtr left, Inno.Native.ImGui.ImGuiMenuColumnsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15186) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3ed601625c0e94b5"></a>[`public void Inno.Native.ImGui.ImGuiMenuColumnsPtr.Update(float spacing, bool windowReappearing)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15211) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-13009eb748284221"></a>[`public bool Inno.Native.ImGui.ImGuiMenuColumnsPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15178) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f142a158e14505e1"></a>[`public Inno.Native.ImGui.ImGuiMenuColumns Inno.Native.ImGui.ImGuiMenuColumnsPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15181) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-64f9e8ed9a7c510a"></a>[`public ref uint Inno.Native.ImGui.ImGuiMenuColumnsPtr.NextTotalWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15192) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-010f3d0210095bf9"></a>[`public static Inno.Native.ImGui.ImGuiMenuColumnsPtr Inno.Native.ImGui.ImGuiMenuColumnsPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15179) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b5e3db602ea96e5e"></a>[`public ref ushort Inno.Native.ImGui.ImGuiMenuColumnsPtr.OffsetIcon { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15194) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d2ad3e5b838a8d3c"></a>[`public ref ushort Inno.Native.ImGui.ImGuiMenuColumnsPtr.OffsetLabel { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15195) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-536da36382e5891f"></a>[`public ref ushort Inno.Native.ImGui.ImGuiMenuColumnsPtr.OffsetMark { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15197) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-40833ed047ae3b6a"></a>[`public ref ushort Inno.Native.ImGui.ImGuiMenuColumnsPtr.OffsetShortcut { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15196) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4b299088e014e1b6"></a>[`public ref ushort Inno.Native.ImGui.ImGuiMenuColumnsPtr.Spacing { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15193) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e1935eba6310a37a"></a>[`public ref uint Inno.Native.ImGui.ImGuiMenuColumnsPtr.TotalWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15191) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-227e1bae8d2dec87"></a>[`public System.Span<ushort> Inno.Native.ImGui.ImGuiMenuColumnsPtr.Widths { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15198) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f15cf0858b8349ea"></a>[`public struct Inno.Native.ImGui.ImGuiMenuColumnsPtr : System.IEquatable<Inno.Native.ImGui.ImGuiMenuColumnsPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15170) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiMetricsConfig</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiMetricsConfig ef90d434241902031a7574cf8d07d7075d9b626616aa1a27992db8408cfab895 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowDebugLog 9daf376cf91f3b2c84b2fb06d8d6089b85e3679732a117cf35d96e26cf531716 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowIDStackTool 63a486d7b6620824296e389f4f0e043e0d6bbe56635e9dc98526c55c647fa497 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowWindowsRects 607fa9073657cd3992f7bfa8360c526b42741be9a690c26bc8a6e7b0459ba7bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowWindowsBeginOrder 71950b681ef04d183cd3cb82388784809806ffff0a581cb2db2d2d9f4f87d22c -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowTablesRects 779557f697da954133f3d93e68d74af1b2a466b58a63dfaba2ef272c29e7985d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowDrawCmdMesh aa8bfb6aa36ba7f7ad66c880c3a04d7479a2798ea8a37afae526f8a92b30f6e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowDrawCmdBoundingBoxes 59f3b07f3216e1958dd1cf06bcb453be038042273a3cc93873bcf7f4c30e95a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowTextEncodingViewer 99bbfea36151148e9c042e24f0530b085f40a995e200acc80c5df921d9142abe -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowTextureUsedRect 1c1ca2b714ba6de39150bb71f4183277e43ce58bf6fb705b78f9c2160360ec14 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowDockingNodes fb71b097d1fcb51223c38c0f0cbf884e80bd0bbc7c298292ad7f3e48774fc52a -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowWindowsRectsType 63c1acee7356841e328c6fda5e0d97a82134d6bab9d77014847e381c3359e86d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowTablesRectsType 25ce956b73e904a18b151c0228b6de3d584cc17a10d9488e08eec53eb0ac4fe5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.HighlightMonitorIdx a757c4b8494852460a7f6f65583d2f06b2bcff2e2a4e358739c44ad2f8089977 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.HighlightViewportID 458e649472ddc6feab11e3c9975c7185682360a779edb865a1a82f419d6b24da -->
<!-- api: F:Inno.Native.ImGui.ImGuiMetricsConfig.ShowFontPreview 48a7abc4630d86ed5b29bbdeae96c2ff2cb7ef9a21207b9c533e928d4e2c2c2b -->
<!-- api: M:Inno.Native.ImGui.ImGuiMetricsConfig.#ctor(System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Int32,System.Int32,System.Int32,System.UInt32,System.Byte) 9b939046beb47c85e3459be564a9d78fae78b9da4b824651673107ce3a936d90 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f848cae886d6a972"></a>[`public int Inno.Native.ImGui.ImGuiMetricsConfig.HighlightMonitorIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7454) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d6c356cb828890c9"></a>[`public uint Inno.Native.ImGui.ImGuiMetricsConfig.HighlightViewportID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7455) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc317db5dffccde8"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowDebugLog`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7442) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f0e326f99b40b6d4"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowDockingNodes`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7451) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-96d59e096dccb0bc"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowDrawCmdBoundingBoxes`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7448) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c699d9c14f89dcf"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowDrawCmdMesh`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7447) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69ebd3e7cd4c9b52"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowFontPreview`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7456) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9dd5f2513dc0bd30"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowIDStackTool`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7443) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b667a21fcc397e0"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowTablesRects`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7446) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eb5d2799cf4f604a"></a>[`public int Inno.Native.ImGui.ImGuiMetricsConfig.ShowTablesRectsType`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7453) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7383752e796fa5e8"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowTextEncodingViewer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7449) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-775aafeaab9b5bb5"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowTextureUsedRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7450) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c55f18ecc2bada1"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowWindowsBeginOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7445) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8807475d77221c7f"></a>[`public byte Inno.Native.ImGui.ImGuiMetricsConfig.ShowWindowsRects`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7444) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03fc20285be5765b"></a>[`public int Inno.Native.ImGui.ImGuiMetricsConfig.ShowWindowsRectsType`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7452) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-82c196dcaef31272"></a>[`public Inno.Native.ImGui.ImGuiMetricsConfig.ImGuiMetricsConfig(byte showDebugLog = 0, byte showIDStackTool = 0, byte showWindowsRects = 0, byte showWindowsBeginOrder = 0, byte showTablesRects = 0, byte showDrawCmdMesh = 0, byte showDrawCmdBoundingBoxes = 0, byte showTextEncodingViewer = 0, byte showTextureUsedRect = 0, byte showDockingNodes = 0, int showWindowsRectsType = 0, int showTablesRectsType = 0, int highlightMonitorIdx = 0, uint highlightViewportID = 0, byte showFontPreview = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7457) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-68eb025f29cf7c8c"></a>[`public struct Inno.Native.ImGui.ImGuiMetricsConfig`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7440) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiMouseButton</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiMouseButton 9b09bdeff73013673ed26712859ae7e5ccc636b2d52d840cbd0be24a0b6e4d54 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseButton.Left e7145fd4085c2462dff441b45d4121a6b62d22dc243f66ebf3a588b12218868d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseButton.Right 0aaabee2694d6913620129458eb5514c4fa1d0e5b6650d6a4ad373e8480769a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseButton.Middle 47041045a7ed4a9291fcb552d7c769014a175ae58cd98a08ae5e6082dac55101 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseButton.Count 334d262d3d79bead188d2de21360e669cee0a68df8470be1038bdb3f2b9d0066 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a6fcfda0bb4df2c0"></a>[`Inno.Native.ImGui.ImGuiMouseButton.Count = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L943) | 当前常量或枚举值：`5`。 | — |
| <a id="api-d59f238c717deb34"></a>[`Inno.Native.ImGui.ImGuiMouseButton.Left = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L940) | 当前常量或枚举值：`0`。 | — |
| <a id="api-311b2bccee63e80f"></a>[`Inno.Native.ImGui.ImGuiMouseButton.Middle = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L942) | 当前常量或枚举值：`2`。 | — |
| <a id="api-318fde4336096fab"></a>[`Inno.Native.ImGui.ImGuiMouseButton.Right = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L941) | 当前常量或枚举值：`1`。 | — |
| <a id="api-6cf395bb2132c365"></a>[`public enum Inno.Native.ImGui.ImGuiMouseButton : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L938) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiMouseCursor</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiMouseCursor 6beb962e99c55bc21312c9cea0b07f9430cf7ad4d4da87a5c179db688dc68bf2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.None 5a67abb7a545ef26a5c8570b601ce362af73c1af7dc549c2a033ee496853ee00 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.Arrow 487a020c8a5dce36302640a59b04e386d9e087927e3e23c8065ab2883883a2db -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.TextInput a6972dd102617a954725a4b2d3a1a6c78d3f4ca12562162cf49d6e79f6d1524d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.ResizeAll ff0dce0352afa515ce58ef9ea6ee3ae90701ba3dc3b3d07383683327c96f9b05 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.ResizeNs b39712f1191571cc507d0e891b66704c27d65f88547cfeef390924c1604aa2e4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.ResizeEw 69d949633184a89b98200863f98210b39867ec01d3fed940aae7aba96d428105 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.ResizeNesw 1874c76bedc454acdb3982fda4a52d07cbee84e05f28cbaddbee0aad31862afc -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.ResizeNwse 8c4d882e445c6d2984a862ccd0e0219f107ef24e5e46e64cf4b87a5da2d3758b -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.Hand fe2636d660f2dc87c03e5b101e0f0501377ad0a0b2b041b046b45a150d9580e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.Wait da83f7dc43ba4f9b18ccfeba8467a6302c75258e6b4b5574102a0c85969650ca -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.Progress 33efb98ccdf07d18c76919a31f3e4440a413301190bb74546e4012af6af3b05e -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.NotAllowed c0fd5e82fc7b404a00ffa0afed584519152d4d0cb1815e1d3d2a2819e615607a -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseCursor.Count 413fe82064f84ed9b7eb4f3974d5c1748cddc61eafd7a91270c686bccee9a3f3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d77e9652c31f9eb7"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.Arrow = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L952) | 当前常量或枚举值：`0`。 | — |
| <a id="api-79d62e2a04a6d62e"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.Count = 11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L963) | 当前常量或枚举值：`11`。 | — |
| <a id="api-ec310ddc0bd503c3"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.Hand = 7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L959) | 当前常量或枚举值：`7`。 | — |
| <a id="api-66f308d07ca94cf6"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.None = -1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L951) | 当前常量或枚举值：`-1`。 | — |
| <a id="api-56233c865606d534"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.NotAllowed = 10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L962) | 当前常量或枚举值：`10`。 | — |
| <a id="api-72c7fdbea9c9e631"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.Progress = 9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L961) | 当前常量或枚举值：`9`。 | — |
| <a id="api-a50483949ad23016"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.ResizeAll = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L954) | 当前常量或枚举值：`2`。 | — |
| <a id="api-3fda170757875622"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.ResizeEw = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L956) | 当前常量或枚举值：`4`。 | — |
| <a id="api-6f771666cd035f69"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.ResizeNesw = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L957) | 当前常量或枚举值：`5`。 | — |
| <a id="api-7d16ea01f5501b4c"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.ResizeNs = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L955) | 当前常量或枚举值：`3`。 | — |
| <a id="api-2a2d09dbaa5137df"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.ResizeNwse = 6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L958) | 当前常量或枚举值：`6`。 | — |
| <a id="api-842b96d2a419b7b7"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.TextInput = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L953) | 当前常量或枚举值：`1`。 | — |
| <a id="api-a4f89f0dff919d0e"></a>[`Inno.Native.ImGui.ImGuiMouseCursor.Wait = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L960) | 当前常量或枚举值：`8`。 | — |
| <a id="api-a1127c4d004c8226"></a>[`public enum Inno.Native.ImGui.ImGuiMouseCursor : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L949) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiMouseSource</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiMouseSource 73dd0ae9257dac5329473451d81881ab5f5720659753bdcb48df09a1eb405e92 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseSource.Mouse 6b277eca9b3b9056c74618b3a57b228cfbe956a5a0633f999696dc33dcc5b205 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseSource.TouchScreen d6f0e1f7df07a57b8803fa351c69710b75a00008ca2ba5bd35f3e3b7c385a573 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseSource.Pen 59971b676438767ccd57a68d0ce6eca98c0fb9876746f66e81ba9b9d834aa4c9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMouseSource.Count 580d44c508e26e556cc52461f4f02539071bbe5ed1e4e5d98f5b605352f0d5b7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-eea81617c663ff3f"></a>[`Inno.Native.ImGui.ImGuiMouseSource.Count = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L73) | 当前常量或枚举值：`3`。 | — |
| <a id="api-12a85c922e6d29f7"></a>[`Inno.Native.ImGui.ImGuiMouseSource.Mouse = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L70) | 当前常量或枚举值：`0`。 | — |
| <a id="api-de0c1d25789e6199"></a>[`Inno.Native.ImGui.ImGuiMouseSource.Pen = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L72) | 当前常量或枚举值：`2`。 | — |
| <a id="api-657a5135de0971ab"></a>[`Inno.Native.ImGui.ImGuiMouseSource.TouchScreen = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L71) | 当前常量或枚举值：`1`。 | — |
| <a id="api-fad0ba2ec619f327"></a>[`public enum Inno.Native.ImGui.ImGuiMouseSource : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L68) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiMultiSelectFlags</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiMultiSelectFlags 312f07142d060fd66d8d96a82e98c503d3e9c35c9a7b49531c10d0403864ceb8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.None 7bb419e19e16d32ac960bd1eb8b72c6389f40f7aa5f9ae74f1e3cfcf01ab17f7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.SingleSelect 04bfb905e1062d84bb47e4f16b87a289e4dc8ee22f701bf50ed28b7cfed569d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NoSelectAll 1f56d6e8577c15424297fe0fe3c7730184e354a45d844cbd0c6198e706da1d8b -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NoRangeSelect 737ac369d8ccb78fac3ea6d9ba82a582e7254588d84a798a4ff2f9613b01f938 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NoAutoSelect 8402e75fa39f21c4622d68a1250fbc78f4f4ea603cb35e81d66824190e5cb979 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NoAutoClear 8333586e4fce18753ece9ca966ed990f91897049fc57fc6bc4ac76aec7d83c73 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NoAutoClearOnReselect e012491363ca67eca1fb6b4dafe7694da6497742c413414212860481f57f429d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.BoxSelect1D 6977a7c6a00008b8f964f869b3cf168a9d726ac2014c3aba4dfd8cd37311fa37 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.BoxSelect2D cf1321041e9b7c830f98dd8a7ca5d91eedfe888e1b64e233184be245aacda472 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.BoxSelectNoScroll d1e4879919f7548e59b26ba33c6c8544af6c1a9d0e83ef1e5685f8939f77364a -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.ClearOnEscape b9c0a9fd289c668dca31b39937d01608d834cd36a09c65be9fb1519287dad0f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.ClearOnClickVoid cae82052dc96373492446e8269bc563e4b8aa832c28000c8764e6ebdccca0107 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.ScopeWindow ed4cd90f68be8347a33db41c8a3b3e83c224b852b2d28ae7364bad6db5c2236d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.ScopeRect 04d377208b3356b9deae45b0c234cfca8f24952627af654fcf4d7893e3e01d7f -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.OnClick 8c69010069d1ff0ed00e317f1ae00cd2ba600d84183d06a24820ec217489f395 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.OnClickRelease 588ab473bf9485ad957f063e1d542a075a4a686c80bed42d9910b0608bebdcb8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NavWrapX 8e8b269df467923641818ffbf602e742e44ba38a8256d3a21a831d3e2c660e69 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectFlags.NoSelectOnRightClick 1697112e2cec5e347aa89c3ab27f4989488fa52de48b7d0afd429d12c9efccb5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b5edb17496801fa3"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.BoxSelect1D = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1098) | 当前常量或枚举值：`64`。 | — |
| <a id="api-2a8437d40d93ce41"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.BoxSelect2D = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1099) | 当前常量或枚举值：`128`。 | — |
| <a id="api-de6d851a2362e683"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.BoxSelectNoScroll = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1100) | 当前常量或枚举值：`256`。 | — |
| <a id="api-c55de8c772f6932c"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.ClearOnClickVoid = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1102) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-6e06cd554e1ab32e"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.ClearOnEscape = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1101) | 当前常量或枚举值：`512`。 | — |
| <a id="api-ed893505f5526d24"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NavWrapX = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1107) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-5923897871255ba5"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NoAutoClear = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1096) | 当前常量或枚举值：`16`。 | — |
| <a id="api-2973100e14d43fdf"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NoAutoClearOnReselect = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1097) | 当前常量或枚举值：`32`。 | — |
| <a id="api-033d4e00ed672469"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NoAutoSelect = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1095) | 当前常量或枚举值：`8`。 | — |
| <a id="api-81c213885c8d8758"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1091) | 当前常量或枚举值：`0`。 | — |
| <a id="api-94b313a4e4a603ed"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NoRangeSelect = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1094) | 当前常量或枚举值：`4`。 | — |
| <a id="api-66e4f5f6909538e4"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NoSelectAll = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1093) | 当前常量或枚举值：`2`。 | — |
| <a id="api-799ef1b498d5e371"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.NoSelectOnRightClick = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1108) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-87795e834a48a0e0"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.OnClick = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1105) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-245c9f86f9d410ef"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.OnClickRelease = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1106) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-a8941a6147084e3f"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.ScopeRect = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1104) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-0f26c8441d9b24d0"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.ScopeWindow = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1103) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-45508217565d21c1"></a>[`Inno.Native.ImGui.ImGuiMultiSelectFlags.SingleSelect = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1092) | 当前常量或枚举值：`1`。 | — |
| <a id="api-9a533919dcc75555"></a>[`public enum Inno.Native.ImGui.ImGuiMultiSelectFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1089) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiMultiSelectIO</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiMultiSelectIO 7fee44e0db6400e03b431ab51090a91d1c54d5da6d2570d7b1a683e50cdd28c5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectIO.Requests ae660fea42530fcc6f8e31c8f49f712038dbe90838c86584c0198291c2a18ec3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectIO.RangeSrcItem bb75232b8bdcc6843c086db7ea3479145a1d3f6f7c44c8cddf7ca1ffa333131c -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectIO.NavIdItem 37906babb126bd1b1f3012558cb6853b3873f30cb9b9b8a87752a3444f3b642e -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectIO.NavIdSelected e27bc07a262c587c0faba6b91481dbd33bf4cb8a3b6d80e8622170abaa2b9d79 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectIO.RangeSrcReset 8832fc936071bf374d9a3b916e989d272a17d3f32e14822d619cd03971b94ce0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectIO.ItemsCount 107876833ba6a0d177fe379ca355125e88970e95c0db46a62a83a833c3b8b2bd -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectIO.#ctor(Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiSelectionRequest},System.Int64,System.Int64,System.Byte,System.Byte,System.Int32) 96d9ed4ed0e4bf0ac832ab07b9f817e8c4068fe95b1701ea363626eb33cea19c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c3088fe023f7bd75"></a>[`public int Inno.Native.ImGui.ImGuiMultiSelectIO.ItemsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6882) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-475d987b106085e4"></a>[`public long Inno.Native.ImGui.ImGuiMultiSelectIO.NavIdItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6879) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b3ec9f40ce3b42b0"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectIO.NavIdSelected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6880) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d8616b6c60c52a29"></a>[`public long Inno.Native.ImGui.ImGuiMultiSelectIO.RangeSrcItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6878) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb20130ca55cc1ad"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectIO.RangeSrcReset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6881) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea8a2fb04f9cf645"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiSelectionRequest> Inno.Native.ImGui.ImGuiMultiSelectIO.Requests`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6877) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a97010b6bed592e"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectIO.ImGuiMultiSelectIO(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiSelectionRequest> requests = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiSelectionRequest>), long rangeSrcItem = 0, long navIdItem = 0, byte navIdSelected = 0, byte rangeSrcReset = 0, int itemsCount = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6883) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2c77d593d88f17f6"></a>[`public struct Inno.Native.ImGui.ImGuiMultiSelectIO`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6875) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiMultiSelectIOPtr</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiMultiSelectIOPtr 3c1bfc1fdc6b2f8e0301c08d544acb933bb0f11c15a842abfee4635e311b345c -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.#ctor(Inno.Native.ImGui.ImGuiMultiSelectIO*) 3d57f2f489be8cffe1ac47a449a515eba8e455b91920f99e2d011969db2f564d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.Handle 81e0a8acd1046eda63e8c318da59c9cb03e90bee65247cc77de9c4dbf76a958a -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.IsNull 888a24d4716227477188bc5aeed42495403c4cb54c6f632cd9de55abb2c36336 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.Null 0c1c9d23d669fc2877b5623fbeab17950e06f7fa305181c610298354230fd1f9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.Item(System.Int32) 5d9d3352a35b6d641e4a276114a5951e5e2b9b1c55a03050ad5bb756b18b814d -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.op_Implicit(Inno.Native.ImGui.ImGuiMultiSelectIO*)~Inno.Native.ImGui.ImGuiMultiSelectIOPtr 0a8698f9f3695e6d49c3a36cca996dc0f5c478a10d5c79d49b68f550d0b407bd -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.op_Implicit(Inno.Native.ImGui.ImGuiMultiSelectIOPtr)~Inno.Native.ImGui.ImGuiMultiSelectIO* c22824dc7e629859dcd1cf386a5cd92ef0210e9b3c91789b7b85f3ebbabaafe0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.op_Equality(Inno.Native.ImGui.ImGuiMultiSelectIOPtr,Inno.Native.ImGui.ImGuiMultiSelectIOPtr) 3de43d03cbb0d212faa093e43f36bbf742a3e1d07381ecbc6f30da4c9391dabb -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.op_Inequality(Inno.Native.ImGui.ImGuiMultiSelectIOPtr,Inno.Native.ImGui.ImGuiMultiSelectIOPtr) e4b8bf4ee3c24e20f8fa555adca31acd5846b97be8b996799fb26922406b0aa6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.Equals(Inno.Native.ImGui.ImGuiMultiSelectIOPtr) 48ece4284f20b06528dfee64fda87ee736bd38d8ed7839a878fc6fc8bc9d1ef5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.Equals(System.Object) edc4c63118c68946384e726b8fbab9f1d491d26930c64c3fdca7c1c62f129722 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.GetHashCode c108d471271e3d92a11d978ac8196fad542704e67cda82e567881aded281a4d1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.Requests 68cbe9e934079d76a8d3c503aad9135b18a89a67429683a8db7d130f77223714 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.RangeSrcItem 4c98e55d3e85c9ec88caeb767bcccb630ebd6ba80b141f7921e4db943a398cd3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.NavIdItem 3352aa0fb7763dfcb76a534a40a00ea70db03736bb6132f4fdd6f3af4c191929 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.NavIdSelected 25dc486c9d2d09702f76b151c95ef33bf04739a62826b51823b4104ed2a14ef2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.RangeSrcReset b2d417095c3b4e1da70de14af74a4b9c6292c1ade2cea90c6417d3fe1eff0463 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectIOPtr.ItemsCount c41859536e9af7cc5d044fe62fd4efac62e9f8331e174a5e4686d32e15739714 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0cbcb5c6ef4a9cdb"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectIO* Inno.Native.ImGui.ImGuiMultiSelectIOPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15241) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba03d4176bb72fd4"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectIOPtr.ImGuiMultiSelectIOPtr(Inno.Native.ImGui.ImGuiMultiSelectIO* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15236) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1c480943179b7846"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectIOPtr.Equals(Inno.Native.ImGui.ImGuiMultiSelectIOPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15251) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-205e959098fb3f8a"></a>[`public override bool Inno.Native.ImGui.ImGuiMultiSelectIOPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15252) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-b1923b9628429784"></a>[`public override int Inno.Native.ImGui.ImGuiMultiSelectIOPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15253) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-9c1e683abdaf48af"></a>[`public static bool Inno.Native.ImGui.ImGuiMultiSelectIOPtr.operator ==(Inno.Native.ImGui.ImGuiMultiSelectIOPtr left, Inno.Native.ImGui.ImGuiMultiSelectIOPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15249) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1d730d4a92ac238a"></a>[`public static Inno.Native.ImGui.ImGuiMultiSelectIOPtr.implicit operator Inno.Native.ImGui.ImGuiMultiSelectIOPtr(Inno.Native.ImGui.ImGuiMultiSelectIO* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15247) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-88bd80effb32e6d7"></a>[`public static Inno.Native.ImGui.ImGuiMultiSelectIOPtr.implicit operator Inno.Native.ImGui.ImGuiMultiSelectIO*(Inno.Native.ImGui.ImGuiMultiSelectIOPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15248) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-87bd0fa34cea4a62"></a>[`public static bool Inno.Native.ImGui.ImGuiMultiSelectIOPtr.operator !=(Inno.Native.ImGui.ImGuiMultiSelectIOPtr left, Inno.Native.ImGui.ImGuiMultiSelectIOPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15250) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-67c0cf6c1be2370b"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectIOPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15242) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a9765376d4e01934"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectIO Inno.Native.ImGui.ImGuiMultiSelectIOPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15245) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6faa160c4a7c9ffd"></a>[`public ref int Inno.Native.ImGui.ImGuiMultiSelectIOPtr.ItemsCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15260) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c6364e5ae4d963a9"></a>[`public ref long Inno.Native.ImGui.ImGuiMultiSelectIOPtr.NavIdItem { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15257) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4d4bf7c7b0beb2ca"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectIOPtr.NavIdSelected { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15258) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c19875e923fcb533"></a>[`public static Inno.Native.ImGui.ImGuiMultiSelectIOPtr Inno.Native.ImGui.ImGuiMultiSelectIOPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15243) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-12752243160285ef"></a>[`public ref long Inno.Native.ImGui.ImGuiMultiSelectIOPtr.RangeSrcItem { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15256) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-35c3ddb56fbfe676"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectIOPtr.RangeSrcReset { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15259) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-91711e5e94ef6823"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiSelectionRequest> Inno.Native.ImGui.ImGuiMultiSelectIOPtr.Requests { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15255) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ee559629f8e4f4c1"></a>[`public struct Inno.Native.ImGui.ImGuiMultiSelectIOPtr : System.IEquatable<Inno.Native.ImGui.ImGuiMultiSelectIOPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15234) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiMultiSelectState</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiMultiSelectState ee104e7fa80ef11eb9b0b985a68796b59ce6ef55c48bddc6c0d2fabd7af385f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.Window 5359132edb2a5d8ff2ba169b5497b417835a2c7dd680a6a7aa8d2737a38274f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.ID 451413ad7d020ceb29baca8205e061fc38fc80c09d08201c95da69dce25f4ef6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.LastFrameActive 98ee4298ccbd5989627e14207918c9c03aa5e185dc3ea8c95d81e9eddc203296 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.LastSelectionSize 6dcdea3e80d45ebdf4c4b163c86ab8259bc2a2412389d8cd1e4e57eac8e7998a -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.RangeSelected 135317e67b73e0f6a5b408a6506e3b6ba97ded478e61ebdf7f261bd66122ad0d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.NavIdSelected e3e8c58b65fea61ba697ceddb6623685ff0f8f698546725b0a4507b5037e1e96 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.RangeSrcItem 5ef5916e720ebb3ab4dd69c40eb07c93e1388de2fc7a5fff374dc10fa68414f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectState.NavIdItem 186bc1367b6088295867894a58281478bf3041d46131cab96deb2ac0716b4de5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectState.#ctor(Inno.Native.ImGui.ImGuiWindow*,System.UInt32,System.Int32,System.Int32,System.Byte,System.Byte,System.Int64,System.Int64) 0f2b07cc2ff259e9ada201bbcca50eaf96347d438ac41abdda249d59c85f886d -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectState.Destroy a6da72481b4fe81f4b5a93ce3d65fea2fc1cdba914397a8e8fc862001f5e59b5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4890ed19d0130f88"></a>[`public uint Inno.Native.ImGui.ImGuiMultiSelectState.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6901) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-32d752a03fc7525a"></a>[`public int Inno.Native.ImGui.ImGuiMultiSelectState.LastFrameActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6902) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-65b47993faa656c4"></a>[`public int Inno.Native.ImGui.ImGuiMultiSelectState.LastSelectionSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6903) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec5aee6e7546395b"></a>[`public long Inno.Native.ImGui.ImGuiMultiSelectState.NavIdItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6907) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8a0a0b04c4024493"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectState.NavIdSelected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6905) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9be9fa4d50e13c85"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectState.RangeSelected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6904) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8cf191b13f087024"></a>[`public long Inno.Native.ImGui.ImGuiMultiSelectState.RangeSrcItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6906) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c3daf8bc437ca192"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiMultiSelectState.Window`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6900) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-66a0a1643a71d4c4"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectState.ImGuiMultiSelectState(Inno.Native.ImGui.ImGuiWindow* window = null, uint iD = 0, int lastFrameActive = 0, int lastSelectionSize = 0, byte rangeSelected = 0, byte navIdSelected = 0, long rangeSrcItem = 0, long navIdItem = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6908) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1f8574685a63abad"></a>[`public void Inno.Native.ImGui.ImGuiMultiSelectState.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67057) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f73107edf67129b1"></a>[`public struct Inno.Native.ImGui.ImGuiMultiSelectState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6898) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiMultiSelectStatePtr</code> · 22 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiMultiSelectStatePtr 86a27479316ef12d94a94f0af05a3943a8728ccc2e3862f0b1baf4c73eb99696 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.#ctor(Inno.Native.ImGui.ImGuiMultiSelectState*) d00f0604f8256aa40146f2ae03717dbc2678f072cc2ef46538d2b415cdb28a55 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.Handle 18e1603e4866d0ef479dbba466d2b4f7136aa76f7dea9d6bd87ec11fdc74232e -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.IsNull 85489f38e187576ddcae3ad53538080faec45cf1b92d5e2ea7dac9cf3aac36c5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.Null 7dfaf424736ba70a9d6b002b527421aff6404afb86a6f6a45cca0958e074f7b3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.Item(System.Int32) 3a8718dca52714e2490426698f317c993ae93979264a8e8b4eed5125a643fa57 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.op_Implicit(Inno.Native.ImGui.ImGuiMultiSelectState*)~Inno.Native.ImGui.ImGuiMultiSelectStatePtr fdc840fc0ce823e939b71388b2701d8c71a7d2e170590296a6c4e02337fa7962 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.op_Implicit(Inno.Native.ImGui.ImGuiMultiSelectStatePtr)~Inno.Native.ImGui.ImGuiMultiSelectState* 87c803353f103648a231dd4f31435895c137ca5f70687d3d62c023cf6bad8e5c -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.op_Equality(Inno.Native.ImGui.ImGuiMultiSelectStatePtr,Inno.Native.ImGui.ImGuiMultiSelectStatePtr) 2bd672295b559f97124cffe83728226b8c5f2c86a44f74b31d157273f5fe5c21 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.op_Inequality(Inno.Native.ImGui.ImGuiMultiSelectStatePtr,Inno.Native.ImGui.ImGuiMultiSelectStatePtr) 57a0b8f6434068ba429c27d752161790c0946a809babd79ffbc33d8d3b42abc7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.Equals(Inno.Native.ImGui.ImGuiMultiSelectStatePtr) eb487518871085528b90cd3c49e96f7553553870cbb9c9fa8143eb7e19040c9a -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.Equals(System.Object) 9bc9907e22dd255c6cffcb333d03102b1a26c329a5fc5677be31b9dfc60ba631 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.GetHashCode dd3a68c20d66fc4f4e95b9dc0353a93275f14d4413c5fc91006c1c396f46a5e8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.Window 10c48c1a06a020128427baed4c84bedfbcf04efa7365128e3a87a11476a9c05d -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.ID 23551b179f1618d26ba0e12711e55ba9189c7a9e155040cbe2deb86d85aa35bd -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.LastFrameActive d3d3334e171a5027663a655dc3cfb81b62f2936ef7bd196e329956601cd824bb -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.LastSelectionSize a461378b8bf95e07ccc3527a8358ec42ebab722dbeb899b46fa4c1cdac3ee6d5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.RangeSelected 435cfa50be97ce4f5b63ffa9b83f90483d91393098834c9be930ff9d71eb9210 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.NavIdSelected f3132a299a26eee0de1ef2af3683e3d84ba4aaa3fc9aed4f15899bc81314e1a8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.RangeSrcItem d4d5678480560406637a4feb1d600a58f329c185e0910e1d428535119a2487a4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.NavIdItem 3e57a8902ec0f23f98b83dd1967007dc5bec5f01b82cac13ad7ad0e011e2c431 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectStatePtr.Destroy f348373cfc8aaa7558b9a0df6f4f0315173ec4b20ffa6a1039dc9bba408a981b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-34a3d6053a73b5f5"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectState* Inno.Native.ImGui.ImGuiMultiSelectStatePtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15271) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6d66a7ba65bf2ef4"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectStatePtr.ImGuiMultiSelectStatePtr(Inno.Native.ImGui.ImGuiMultiSelectState* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15266) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d907e70beb642d4e"></a>[`public void Inno.Native.ImGui.ImGuiMultiSelectStatePtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15297) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e5ca4cbe3558d437"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectStatePtr.Equals(Inno.Native.ImGui.ImGuiMultiSelectStatePtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15281) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-e15aac08f7793dbc"></a>[`public override bool Inno.Native.ImGui.ImGuiMultiSelectStatePtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15282) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-bc9962b6db9839c5"></a>[`public override int Inno.Native.ImGui.ImGuiMultiSelectStatePtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15283) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-70d04e98d3a3a448"></a>[`public static bool Inno.Native.ImGui.ImGuiMultiSelectStatePtr.operator ==(Inno.Native.ImGui.ImGuiMultiSelectStatePtr left, Inno.Native.ImGui.ImGuiMultiSelectStatePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15279) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d760a2d10d5f1ffc"></a>[`public static Inno.Native.ImGui.ImGuiMultiSelectStatePtr.implicit operator Inno.Native.ImGui.ImGuiMultiSelectStatePtr(Inno.Native.ImGui.ImGuiMultiSelectState* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15277) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-66b11684e118f103"></a>[`public static Inno.Native.ImGui.ImGuiMultiSelectStatePtr.implicit operator Inno.Native.ImGui.ImGuiMultiSelectState*(Inno.Native.ImGui.ImGuiMultiSelectStatePtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15278) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-43f6ca67e9a08f09"></a>[`public static bool Inno.Native.ImGui.ImGuiMultiSelectStatePtr.operator !=(Inno.Native.ImGui.ImGuiMultiSelectStatePtr left, Inno.Native.ImGui.ImGuiMultiSelectStatePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15280) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b23aa7a2a87b03e9"></a>[`public ref uint Inno.Native.ImGui.ImGuiMultiSelectStatePtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15286) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-60976f4ee6f756fc"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectStatePtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15272) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-54dd2dd75ba23d4b"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectState Inno.Native.ImGui.ImGuiMultiSelectStatePtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15275) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-31e3eeb7084a41c6"></a>[`public ref int Inno.Native.ImGui.ImGuiMultiSelectStatePtr.LastFrameActive { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15287) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fbc02b650ff9609b"></a>[`public ref int Inno.Native.ImGui.ImGuiMultiSelectStatePtr.LastSelectionSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15288) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-947c74522ac77ad9"></a>[`public ref long Inno.Native.ImGui.ImGuiMultiSelectStatePtr.NavIdItem { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15292) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9cce5995e4c0fda1"></a>[`public ref byte Inno.Native.ImGui.ImGuiMultiSelectStatePtr.NavIdSelected { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15290) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-927e285222263320"></a>[`public static Inno.Native.ImGui.ImGuiMultiSelectStatePtr Inno.Native.ImGui.ImGuiMultiSelectStatePtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15273) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3cb72c0bb3c71211"></a>[`public ref byte Inno.Native.ImGui.ImGuiMultiSelectStatePtr.RangeSelected { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15289) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4ca1c573301e694e"></a>[`public ref long Inno.Native.ImGui.ImGuiMultiSelectStatePtr.RangeSrcItem { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15291) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6b49c4dc71ad5b89"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiMultiSelectStatePtr.Window { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15285) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bb8b8743c6e42c30"></a>[`public struct Inno.Native.ImGui.ImGuiMultiSelectStatePtr : System.IEquatable<Inno.Native.ImGui.ImGuiMultiSelectStatePtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15264) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiMultiSelectTempData</code> · 21 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiMultiSelectTempData a2837a55d7e93be2797873df5ee1e0bd4e8a3fe5be49c32ab60b7c94d3bf59f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.IO f8da4e856b44d95c1e8b0b958bba1642996205743ca8bb3c3a3fab5a132b4b7e -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.Storage 5c73198a8ece93bf4166c9bae6c5af8be8ac958f36c9dafb5ecfe5522ead9a18 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.FocusScopeId fd4833f1e1e872fa631653c52338b67fb574616e115eb40eafa6cb1edb8cc776 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.Flags f44dace06133d8a5df1af2bd45eec4eab99e7e2f2cb3e45de928ac42587bd2ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.ScopeRectMin 25e8a4dec0d02aa38f5ab4abcacb7f521cca8845d77907abf9017c6e767d5afb -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.BackupCursorMaxPos f8c0abe6b609e3537e91db4109570ac6d1390300645f5102075e363782c3afff -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.LastSubmittedItem 2c903c26892c3004a4b6e76cc072a5420a71e1e65e89350438da26ed2110287c -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.BoxSelectId 04ba5d4bd75ff795c7c8a1f0f772e7a442a9dc3d96661f89e519ed261e4db7df -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.KeyMods 4daaa61673c993c49b9aeb3f82cfa8fdb9e54f824e923eac4b1ddb5074321b08 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.LoopRequestSetAll 9a6d9586f0200393b47ff53f53644dca7c97cb8c0eda204f4f51aebeb63b2a9c -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.IsEndIO 56b337f032c5215ee04c4f739c7674e0fad3822d1438b59771d13ff707da270a -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.IsFocused 5f58388923323dccc3f645a08dd5b06bf814b971353ae18e510e2bc9ffda18c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.IsKeyboardSetRange dc910b83dfcb5a907835bf1ed39c0c2030b4a417b820752dcb53948919abd00c -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.NavIdPassedBy 91b88d02f31ece400bfdb3f838a5e4215259708115508a9a0d5ba1c35b91b288 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.RangeSrcPassedBy b24d8cdee7688bc20d40c634a17196bd7b8d4972ad013c70365087230ce4d2d2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempData.RangeDstPassedBy 35e4495abccd76fd50a1cf15a7d644d66679165cf234b075d64f2b635ce8c911 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempData.#ctor(Inno.Native.ImGui.ImGuiMultiSelectIO,Inno.Native.ImGui.ImGuiMultiSelectState*,System.UInt32,Inno.Native.ImGui.ImGuiMultiSelectFlags,System.Numerics.Vector2,System.Numerics.Vector2,System.Int64,System.UInt32,System.Int32,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte) be493ed8409e2527fcf7f09694d57a226db8d1f9aa173cf9d7b3f37d3c351d54 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempData.Destroy a7274db65a076858360a02fa3b62d0fe733cdd76b2d1cd4d182ecd0f00ca6ca5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempData.Clear 9f314df619436ea04f72e7bfa29df8558b982b42cb79711b7f11747e4ac2f55b -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempData.ClearIO a3a463b45753ef4cefd3c40b4728364d78c464d7fd20508395e95163783b7211 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2de59547d03817a7"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiMultiSelectTempData.BackupCursorMaxPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6932) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e0dedee8e029ffd4"></a>[`public uint Inno.Native.ImGui.ImGuiMultiSelectTempData.BoxSelectId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6934) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8eced293ef8d0d3c"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectFlags Inno.Native.ImGui.ImGuiMultiSelectTempData.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6930) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-70f4b0a4c576ae18"></a>[`public uint Inno.Native.ImGui.ImGuiMultiSelectTempData.FocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6929) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f5389a11744303c"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectIO Inno.Native.ImGui.ImGuiMultiSelectTempData.IO`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6927) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a2cc3a1ecb54a682"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectTempData.IsEndIO`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6937) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c34374ac4ff3602c"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectTempData.IsFocused`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6938) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3db8867b3c06588e"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectTempData.IsKeyboardSetRange`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6939) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6afe04a3f737912f"></a>[`public int Inno.Native.ImGui.ImGuiMultiSelectTempData.KeyMods`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6935) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e28d578ced6ca32"></a>[`public long Inno.Native.ImGui.ImGuiMultiSelectTempData.LastSubmittedItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6933) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-49eabb21a2b456c7"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectTempData.LoopRequestSetAll`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6936) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54f10bb97aacdb48"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectTempData.NavIdPassedBy`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6940) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-897ddff5514e03d8"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectTempData.RangeDstPassedBy`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6942) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2633832377592dd0"></a>[`public byte Inno.Native.ImGui.ImGuiMultiSelectTempData.RangeSrcPassedBy`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6941) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64a7650d18092265"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiMultiSelectTempData.ScopeRectMin`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6931) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-87a0e63101f7dfd2"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectState* Inno.Native.ImGui.ImGuiMultiSelectTempData.Storage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6928) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-704e72f1e8327b3e"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectTempData.ImGuiMultiSelectTempData(Inno.Native.ImGui.ImGuiMultiSelectIO iO = default(Inno.Native.ImGui.ImGuiMultiSelectIO), Inno.Native.ImGui.ImGuiMultiSelectState* storage = null, uint focusScopeId = 0, Inno.Native.ImGui.ImGuiMultiSelectFlags flags = Inno.Native.ImGui.ImGuiMultiSelectFlags.None, System.Numerics.Vector2 scopeRectMin = default(System.Numerics.Vector2), System.Numerics.Vector2 backupCursorMaxPos = default(System.Numerics.Vector2), long lastSubmittedItem = 0, uint boxSelectId = 0, int keyMods = 0, byte loopRequestSetAll = 0, byte isEndIO = 0, byte isFocused = 0, byte isKeyboardSetRange = 0, byte navIdPassedBy = 0, byte rangeSrcPassedBy = 0, byte rangeDstPassedBy = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6943) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-da9fc268a37ce6fa"></a>[`public void Inno.Native.ImGui.ImGuiMultiSelectTempData.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67032) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-49300a1d7ec3f9e4"></a>[`public void Inno.Native.ImGui.ImGuiMultiSelectTempData.ClearIO()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67043) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-83c748a04eb1e2be"></a>[`public void Inno.Native.ImGui.ImGuiMultiSelectTempData.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67021) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9d8fdeca71231991"></a>[`public struct Inno.Native.ImGui.ImGuiMultiSelectTempData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6925) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr</code> · 32 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr 9525eaddcde42311a38f2b7f16f7042efa5c2a7f221711508fe2dd9c3a213d93 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.#ctor(Inno.Native.ImGui.ImGuiMultiSelectTempData*) adde698ec0aa4048e92dd2fa68983418c99d9102ef054f4c3bc520465bca090d -->
<!-- api: F:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Handle 7d974beef44c42f5049899938f8a9302f414e971eb7b122ea8481bcc5fdccd79 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.IsNull 45ee0579bfca41b1f6e737bc71db55378c0be265e12fbc97f77ea704b6799073 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Null 878c60b6c481da04da942db64acb0c31068ffdfebf0e76fb3b51d92770d467ab -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Item(System.Int32) df11f7359181c9020af7f23474f3fae6f1eff19bdc664bb9b25d24985f9eb058 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiMultiSelectTempData*)~Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr 640840f8bd3d622c33517d6915a780fbc5743ce5413c389ae30701e90d7762cb -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr)~Inno.Native.ImGui.ImGuiMultiSelectTempData* 53cfc1e219c9c3158b4ab5b6885ce39b0eee26689ab4ef6a7a61617d9bd2cf96 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.op_Equality(Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr,Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr) d14a237a3fcb983ed3566dbefe6f5727404904fba8efa7afc6e524a7236e23d1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.op_Inequality(Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr,Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr) dcce4ab36bcf2ee49155e8670a1e827c38518d084b6cc775008de010771a4b39 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Equals(Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr) 0778ebc36c9b2b439e3dcbdc95803cac045648dd77ccb68b95594d4c4a20ae99 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Equals(System.Object) d16820b206945eddea1260c526b95e93b630ba8d4875feae197babea7bf5ed56 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.GetHashCode 906f900e06d7fefcdba7295cd98069eb64ffb248df2f4172293228e42b932df8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.IO 476efdb83c2e1b81bf896ae77082b0064b43abe13c0da4a0c2ebb8321d27bf9f -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Storage 339231cc1beb7008db530676bb8b4d0deeefc2846a55642cac7bd1618b528551 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.FocusScopeId 0d3f23eb08ea057f46b2c4b911ffc2e25bfd915abf461a44dd1dbd7a7bfdb15b -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Flags cfb066af11e505bb10db769e37dfa4eba26b388b852456ba77cc8331ac3b683b -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.ScopeRectMin 5cb634748463a05bc56c4cd66ada7d07a73b9bd183e4ddef91b7554af7d5949a -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.BackupCursorMaxPos af48ee39adb7a6399987f8628255d94f0f99eda48187d751f85f7b4bca6e57cd -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.LastSubmittedItem 05feb08842d1011c6a649b81acf8ddedd6106a7212ea75d099e7204051dd750b -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.BoxSelectId 8fd85e812bc7d397c43fd1ec381109dd45855b62e0009576d4fd8f78ed9cb242 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.KeyMods 2a0b1f01b3126b557c0762b1efd35fbbfc8a941e0dd53ccc314e771b117adbfb -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.LoopRequestSetAll fa488cf972a652d1963493bf5915f3a06800a655e8441433460c82bb5994e6df -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.IsEndIO 73eea991dfeae645b707e5ff6eb156ce2245c47e48a54fb834111f8cb306d839 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.IsFocused 30504981f080f00a51b24f9f55238d6d11901197605d46c640548e20589fcf92 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.IsKeyboardSetRange 6a637f681ff4fe2806f77e836fc5ea8909cabf41af56ba6651a94d635a261ac6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.NavIdPassedBy 3318f7345d0678971d49f2e85be763b5809a3eecf5f8e099e59648f5ee5857b4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.RangeSrcPassedBy df4d7e23892a20b878741b9cdb8bbe48adf29ae0742c3e5608eba4fb0159e9e9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.RangeDstPassedBy f496d551d4aad48ec836705831d511a0457bd3603ff0f2559448a86fe8357214 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Destroy 289dfbd5c24a0d0cac26388531d698efef2120b89bdde41462bbe0968c932cc1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Clear cad89fcd2465c54c615400887c3ce5ba0f83e107dbdf42c03a22c9b7ee752fb7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.ClearIO 80532b2468032fd93d13ffb7f63a9dad16a95ad4aaa3120073219ccd1ca58570 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ff4491ae67b78703"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectTempData* Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15311) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f0892baf3a4cf825"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.ImGuiMultiSelectTempDataPtr(Inno.Native.ImGui.ImGuiMultiSelectTempData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15306) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-eb2e7309e32341c6"></a>[`public void Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15353) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d29d1b0080da9a27"></a>[`public void Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.ClearIO()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15361) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-34010f4c9c3efef6"></a>[`public void Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15345) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-16be18e79f6db5c2"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Equals(Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15321) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-75a1e0a31efa307d"></a>[`public override bool Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15322) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-7da11257243cdf5b"></a>[`public override int Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15323) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-4588f80eeb59cc0c"></a>[`public static bool Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.operator ==(Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr left, Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15319) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7d9642a7deb3a04c"></a>[`public static Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.implicit operator Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr(Inno.Native.ImGui.ImGuiMultiSelectTempData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15317) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e8642f230a7b4150"></a>[`public static Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.implicit operator Inno.Native.ImGui.ImGuiMultiSelectTempData*(Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15318) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eda461e1154084c2"></a>[`public static bool Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.operator !=(Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr left, Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15320) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9862110fdde9f743"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.BackupCursorMaxPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15330) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e9c4a1396ed15c26"></a>[`public ref uint Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.BoxSelectId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15332) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dbf65a44afc0eb72"></a>[`public ref Inno.Native.ImGui.ImGuiMultiSelectFlags Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15328) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9fafc77d48dd0182"></a>[`public ref uint Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.FocusScopeId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15327) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-21a33ebf7df3c215"></a>[`public ref Inno.Native.ImGui.ImGuiMultiSelectIO Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.IO { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15325) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e0a498a2d1dc886b"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.IsEndIO { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15335) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-019bd64e6dfc708b"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.IsFocused { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15336) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bc575ded5c707f42"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.IsKeyboardSetRange { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15337) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9206f03e50ca4447"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15312) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6fc167e761062d15"></a>[`public Inno.Native.ImGui.ImGuiMultiSelectTempData Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15315) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1dc30fbbe4d5389a"></a>[`public ref int Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.KeyMods { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15333) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8a2530e669cee69a"></a>[`public ref long Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.LastSubmittedItem { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15331) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d65a865e7d6041b4"></a>[`public ref byte Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.LoopRequestSetAll { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15334) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-08d16a01a056dc57"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.NavIdPassedBy { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15338) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9edd8cdc04b7a40b"></a>[`public static Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15313) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9199ee8519f2b940"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.RangeDstPassedBy { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15340) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7626280f05449e63"></a>[`public bool Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.RangeSrcPassedBy { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15339) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8a163e5426330205"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.ScopeRectMin { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15329) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba93bd01029baabf"></a>[`public ref Inno.Native.ImGui.ImGuiMultiSelectStatePtr Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr.Storage { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15326) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6ae3f666711ab304"></a>[`public struct Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr : System.IEquatable<Inno.Native.ImGui.ImGuiMultiSelectTempDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15304) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiNavItemData</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiNavItemData 5a914eab0168a0cadbfe1ccd7defa8c0ca7ad96c6fb5fd9df044c938934ce080 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.Window 6ed7420b009ef6db21097b18be205c2ab0bfb4f59002b89c24e09237d880e141 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.ID f8c6687a4ca42fd5bde17c492b0ae5f54340c44b07464668b38e68ebfa210d48 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.FocusScopeId 6dfd2c29dced5b8eabe6ed9b2564d73cdefcde5e8bc2b124003731e7424347de -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.RectRel 16e848eb119e9e9ec631fdf4c0e2d0c5ab37cbb69731210d831378136347a3c7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.ItemFlags 4c8de7eb83ed0f9e8ea2602d29bea94ed716b39d6953b0767cf09aceebd09e93 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.DistBox 9c6a70fb9334867804d0926d07a7ecf332245d7f3b58ba3746f7e724b1c8c3b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.DistCenter 85be80201fdda080d39cf564b27fd9899a45306480ded14307456cddca6fd0f9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.DistAxial 3a60b0936e924028d5bf89c15ecedf3b87f997758c9c88d8e06942b433e81f66 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemData.SelectionUserData a53e06895e9916c29c6dc1462981f6b45a217c19be5738d4c16d77fed67a7ef2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNavItemData.#ctor(Inno.Native.ImGui.ImGuiWindow*,System.UInt32,System.UInt32,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImGuiItemFlags,System.Single,System.Single,System.Single,System.Int64) 2f7267e240c49e7a9de758dcd90ebdcf63e049a5eb56f76f30defcb0b6d552d4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNavItemData.Destroy 319ab60b0bc8275e142b7ac26f4bd8e521cf121c7127e8e1a54ee2e8384f19bc -->
<!-- api: M:Inno.Native.ImGui.ImGuiNavItemData.Clear b8efb7628d5a1698a0e88001fd862981e07f18917571886b53bedaaa2f4ae6b7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-53b78d87497cb8b2"></a>[`public float Inno.Native.ImGui.ImGuiNavItemData.DistAxial`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5854) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a4c6ef8eb2f7d43"></a>[`public float Inno.Native.ImGui.ImGuiNavItemData.DistBox`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5852) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c634f552d86b806"></a>[`public float Inno.Native.ImGui.ImGuiNavItemData.DistCenter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5853) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a1534403ec635eec"></a>[`public uint Inno.Native.ImGui.ImGuiNavItemData.FocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5849) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-06d735254c72d11d"></a>[`public uint Inno.Native.ImGui.ImGuiNavItemData.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5848) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f0be9f19764ae174"></a>[`public Inno.Native.ImGui.ImGuiItemFlags Inno.Native.ImGui.ImGuiNavItemData.ItemFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5851) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c7c8ff14c77b618e"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiNavItemData.RectRel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5850) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d0636faa75804cdf"></a>[`public long Inno.Native.ImGui.ImGuiNavItemData.SelectionUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5855) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-36fa467fe5046024"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiNavItemData.Window`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5847) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-48ee4041d310f510"></a>[`public Inno.Native.ImGui.ImGuiNavItemData.ImGuiNavItemData(Inno.Native.ImGui.ImGuiWindow* window = null, uint iD = 0, uint focusScopeId = 0, Inno.Native.ImGui.ImRect rectRel = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImGuiItemFlags itemFlags = Inno.Native.ImGui.ImGuiItemFlags.None, float distBox = 0, float distCenter = 0, float distAxial = 0, long selectionUserData = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5856) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-aca9ac96f0526499"></a>[`public void Inno.Native.ImGui.ImGuiNavItemData.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66940) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8fc4909796d53955"></a>[`public void Inno.Native.ImGui.ImGuiNavItemData.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66929) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a111af9614084be2"></a>[`public struct Inno.Native.ImGui.ImGuiNavItemData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5845) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiNavItemDataPtr</code> · 24 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiNavItemDataPtr b699c079a8dc79f0b36962ce79186a11fc0905c16a1462ec7cd18b1c6596530b -->
<!-- api: M:Inno.Native.ImGui.ImGuiNavItemDataPtr.#ctor(Inno.Native.ImGui.ImGuiNavItemData*) 31dd04794d992c0fd4e78d66db4a9ac0edac8d600bc1fc536c7a0bbc4d0fe5ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavItemDataPtr.Handle 648e0ce104cdd5981b79fbcd929778a5604ce0b61e61832b9b2f2989f6f090a0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNavItemDataPtr.IsNull da8dbe344729bce96eb56c6e1c7318c3a2dd985f1a52fd5a97237949b3c2d13f -->
<!-- api: P:Inno.Native.ImGui.ImGuiNavItemDataPtr.Null c395ebe0cc596b4953920aebdfc48599ebebd4242e219739ccabcef113001747 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNavItemDataPtr.Item(System.Int32) 4379bd5d96dc6658b1bfd35a8014f4e1575aa3a03d0a47fb3e1315b99ea1342d -->
<!-- api: M:Inno.Native.ImGui.ImGuiNavItemDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiNavItemData*)~Inno.Native.ImGui.ImGuiNavItemDataPtr e38e1ec4430684c1edc2af61cf6d9d06ae636870a8cc286efa480e78deb9c11a -->
<!-- api: M:Inno.Native.ImGui.ImGuiNavItemDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiNavItemDataPtr)~Inno.Native.ImGui.ImGuiNavItemData* fc8e2e457635ded49feee75acd12188b82663ad3568dfe692ff116c2684a25ec -->
<!-- api: M:Inno.Native.ImGui.ImGuiNavItemDataPtr.op_Equality(Inno.Native.ImGui.ImGuiNavItemDataPtr,Inno.Native.ImGui.ImGuiNavItemDataPtr) 8145a894e70d8bf2209b8f3315b2ddd6578145f18a8b7e9addcf3a6e8f587be8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNavItemDataPtr.op_Inequality(Inno.Native.ImGui.ImGuiNavItemDataPtr,Inno.Native.ImGui.ImGuiNavItemDataPtr) a78e15d5832cde8e8aa6532596c23f180a1c44433461fa85693c9eec4966e066 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNavItemDataPtr.Equals(Inno.Native.ImGui.ImGuiNavItemDataPtr) 6c696b3ea37252aeeb2594e18181962e8c25be02114a6d759eda2ba46f005300 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNavItemDataPtr.Equals(System.Object) d0d33d936fb7508b87b5e11fb51619297b72ef39922b69e4162214fc724d9c57 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNavItemDataPtr.GetHashCode b6412d285116efab6077b2d32ca743a4e1445a1177f3a91b214a9c67146c2927 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNavItemDataPtr.Window 9b94c5b0beb8f19894c72e4fb7e6d4d2a308a6c32323e1e95cb3fa215dd82f0e -->
<!-- api: P:Inno.Native.ImGui.ImGuiNavItemDataPtr.ID 7551da0e92564370534324ce3d29b8aa2afa3db8daa9fde627864e86af3a7a19 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNavItemDataPtr.FocusScopeId 40ebed9ff26f5fc3d8d0d01add1faae30904f168ab879fa4642a0637a0e33000 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNavItemDataPtr.RectRel 73fa86f67528c33caac1803865003319e38b2fa353479db975e64be91fbfbbe7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNavItemDataPtr.ItemFlags e508b349b8746cc51200cdc0fa4ecea4a3d730aecb605099dbe54924e573df94 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNavItemDataPtr.DistBox d2e2fb2ff5c6dc654eda939e58cdbf802b26cf5981c3a0c0a33a1676a77afe98 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNavItemDataPtr.DistCenter 9e7846fba22f3a618c4c4076ef68f256f0e5f37f5ec81d032bdfd6ed65339583 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNavItemDataPtr.DistAxial b0741cead02c84a3ef2fe95d0e958a52406ea68ff9788b8786fac2de60b8428f -->
<!-- api: P:Inno.Native.ImGui.ImGuiNavItemDataPtr.SelectionUserData c279d1782874247e0da0eeac79b47dff242ecb91f9f18d3f4a72dfe849796c1b -->
<!-- api: M:Inno.Native.ImGui.ImGuiNavItemDataPtr.Destroy 92bb79d9655eba533b93205b9bc4f292e716a7ec58cf190bd5b6d72de317b878 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNavItemDataPtr.Clear ae5170153bbb179548d25c6a1f8484f51a3ddab520b6d4e7546077e5ebab3079 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f53f19bc515ced84"></a>[`public Inno.Native.ImGui.ImGuiNavItemData* Inno.Native.ImGui.ImGuiNavItemDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15375) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-36ff338ca5863d9b"></a>[`public Inno.Native.ImGui.ImGuiNavItemDataPtr.ImGuiNavItemDataPtr(Inno.Native.ImGui.ImGuiNavItemData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15370) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a2a9a2eb4ce7a054"></a>[`public void Inno.Native.ImGui.ImGuiNavItemDataPtr.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15410) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-63d21f3445ae6f6a"></a>[`public void Inno.Native.ImGui.ImGuiNavItemDataPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15402) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a962e8b6eefad041"></a>[`public bool Inno.Native.ImGui.ImGuiNavItemDataPtr.Equals(Inno.Native.ImGui.ImGuiNavItemDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15385) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-f3a136394c3ac61b"></a>[`public override bool Inno.Native.ImGui.ImGuiNavItemDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15386) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-9736272b1b49e748"></a>[`public override int Inno.Native.ImGui.ImGuiNavItemDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15387) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-ef33119279454c7f"></a>[`public static bool Inno.Native.ImGui.ImGuiNavItemDataPtr.operator ==(Inno.Native.ImGui.ImGuiNavItemDataPtr left, Inno.Native.ImGui.ImGuiNavItemDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15383) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-51275d7c0a4e8a3c"></a>[`public static Inno.Native.ImGui.ImGuiNavItemDataPtr.implicit operator Inno.Native.ImGui.ImGuiNavItemDataPtr(Inno.Native.ImGui.ImGuiNavItemData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15381) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-74f87d2bfdad5057"></a>[`public static Inno.Native.ImGui.ImGuiNavItemDataPtr.implicit operator Inno.Native.ImGui.ImGuiNavItemData*(Inno.Native.ImGui.ImGuiNavItemDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15382) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-52e3a7113aca8f1f"></a>[`public static bool Inno.Native.ImGui.ImGuiNavItemDataPtr.operator !=(Inno.Native.ImGui.ImGuiNavItemDataPtr left, Inno.Native.ImGui.ImGuiNavItemDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15384) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a5be4f89128e9605"></a>[`public ref float Inno.Native.ImGui.ImGuiNavItemDataPtr.DistAxial { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15396) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1498be4de113e79b"></a>[`public ref float Inno.Native.ImGui.ImGuiNavItemDataPtr.DistBox { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15394) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-952a0a46c4ab0897"></a>[`public ref float Inno.Native.ImGui.ImGuiNavItemDataPtr.DistCenter { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15395) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a9b0d3365e8b6470"></a>[`public ref uint Inno.Native.ImGui.ImGuiNavItemDataPtr.FocusScopeId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15391) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dbe9127f3dfbb8e1"></a>[`public ref uint Inno.Native.ImGui.ImGuiNavItemDataPtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15390) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4d556e24d5aa5658"></a>[`public bool Inno.Native.ImGui.ImGuiNavItemDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15376) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2b7f66b532da9d6d"></a>[`public Inno.Native.ImGui.ImGuiNavItemData Inno.Native.ImGui.ImGuiNavItemDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15379) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-607b25c656cd4c8d"></a>[`public ref Inno.Native.ImGui.ImGuiItemFlags Inno.Native.ImGui.ImGuiNavItemDataPtr.ItemFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15393) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-775e319c3b2c511f"></a>[`public static Inno.Native.ImGui.ImGuiNavItemDataPtr Inno.Native.ImGui.ImGuiNavItemDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15377) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fe002663cb44cc3c"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiNavItemDataPtr.RectRel { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15392) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ee55f6622e9fcc88"></a>[`public ref long Inno.Native.ImGui.ImGuiNavItemDataPtr.SelectionUserData { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15397) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-90161e2e76faa155"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiNavItemDataPtr.Window { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15389) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-93520abe0fbaf00a"></a>[`public struct Inno.Native.ImGui.ImGuiNavItemDataPtr : System.IEquatable<Inno.Native.ImGui.ImGuiNavItemDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15368) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiNavLayer</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiNavLayer 3b5d81713654589c2ba0472adb13d8ee2e80aa0d20bd845c8963f3935db264aa -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavLayer.Main 76d8459515dd895c0ebb881b5a52a62bcbf0ada531aa9e3666944281f4425ec1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavLayer.Menu 61f544dfb3478216cca3dbeda1bc3e514f1dba8fd5091f6f102ad38a0224cb4c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavLayer.Count 6111c017a0e55d12548e98855365027d16824607a7cf61b8bf6d0ad0f4ec7e12 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b87cc314141bd7cd"></a>[`Inno.Native.ImGui.ImGuiNavLayer.Count = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L296) | 当前常量或枚举值：`2`。 | — |
| <a id="api-8aa50785334ce37d"></a>[`Inno.Native.ImGui.ImGuiNavLayer.Main = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L294) | 当前常量或枚举值：`0`。 | — |
| <a id="api-0bab7dbeb34f9076"></a>[`Inno.Native.ImGui.ImGuiNavLayer.Menu = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L295) | 当前常量或枚举值：`1`。 | — |
| <a id="api-c873f85cf9e9decb"></a>[`public enum Inno.Native.ImGui.ImGuiNavLayer : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L292) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiNavMoveFlags</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiNavMoveFlags 3183bd46c32964d257869727561c868ecdaa35d2166bb670e7a929cd8204c37a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.None 329cc1707162925860092c9bdc9e83d7010a899958586eb1c1903ffaec0eb497 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.LoopX 1c2f01ccab08ddc4ab228b876b4074639531d6aace6d8f159574a02148d2b4f5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.LoopY ad873d85e0142207c7c8b49836fa7dbe87644889ed4537eb942ffb683e57003d -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.WrapX 90dc4d316f43df6ecd1e71e96f533537b0e40cb2bbe140db118950ca743f7108 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.WrapY 7a22d18fd31b041294e561269665db8b625750d9213ba20278fa96d07c5577cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.WrapMask 8e5b5e558bd36d85431199ce8a834d0f8d6b531922e05c2ac9600c58c3a4db55 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.AllowCurrentNavId e6622681e2b94a8f0eba2890ec2eb35fc7f1062400499ec279a27eff627d4438 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.AlsoScoreVisibleSet 61ea041aee33cd743d27447bfb41c0fdd8b4a3ab2c93892cff6fcbbe808fddbe -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.ScrollToEdgeY 4796a966da1a7a8d822a0089dadbad3d7fc5b72ab239939202c96ae97b981493 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.Forwarded 4b983a4cb36d25bb1d24c7dc660dcaf73b90089d9ff0d2c249d36595531c2e7c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.DebugNoResult ad08fe0ac93944b96766e79c9e99477ef9a0f8f36acb70558f968e73f1457faa -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.FocusApi 89d5f4a131bc5f26af5f8f01531e78b3f5a4cd02de9b4825dcf51bebe5ccfcc7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.IsTabbing ee69fe9621bf38f200e7087a49bab98f174dc3e4959ea22550170436f9a81e4f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.IsPageMove 75d5366e18f998d15af5ae018a0f52e44fd13684fa03e29ea8cacc9060724856 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.Activate a7f7084fc836ed73e69905bb7fad735b347739ddf9f6e74e53d94c3530adbfba -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.NoSelect caef67a2d6da648a305fceb61ef8e4503e7d78a7d0a50e5f3fad568fdad2e199 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.NoSetNavCursorVisible e24c1254da86b7fd28b27e27161e084bc6a347aa4490b2665d697caae3b33003 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavMoveFlags.NoClearActiveId db30896343c0d28904d9b98e0b7fb5d27e55813448e86c16be3f0106598764da -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9c690579cc6dc617"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.Activate = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1571) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-4b4cb8cff2ae706f"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.AllowCurrentNavId = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1563) | 当前常量或枚举值：`16`。 | — |
| <a id="api-1353f3d0cfe56c75"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.AlsoScoreVisibleSet = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1564) | 当前常量或枚举值：`32`。 | — |
| <a id="api-f61f67bf8a01b819"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.DebugNoResult = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1567) | 当前常量或枚举值：`256`。 | — |
| <a id="api-9b411af17ef17725"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.FocusApi = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1568) | 当前常量或枚举值：`512`。 | — |
| <a id="api-b228e9666470c0c3"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.Forwarded = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1566) | 当前常量或枚举值：`128`。 | — |
| <a id="api-513d21788bf7fd0d"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.IsPageMove = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1570) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-f20d2babd3394113"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.IsTabbing = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1569) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-2b9ae580865ca332"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.LoopX = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1558) | 当前常量或枚举值：`1`。 | — |
| <a id="api-8c77478b0672c479"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.LoopY = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1559) | 当前常量或枚举值：`2`。 | — |
| <a id="api-6b9d6b6a38d62a77"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.NoClearActiveId = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1574) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-34e19d3683cef603"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1557) | 当前常量或枚举值：`0`。 | — |
| <a id="api-5d2eaaaad4ddf388"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.NoSelect = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1572) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-6c43069ef61058a3"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.NoSetNavCursorVisible = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1573) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-abbd985c145a8b4a"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.ScrollToEdgeY = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1565) | 当前常量或枚举值：`64`。 | — |
| <a id="api-09370d3795ae50cf"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.WrapMask = 15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1562) | 当前常量或枚举值：`15`。 | — |
| <a id="api-24d1f509a5d61960"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.WrapX = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1560) | 当前常量或枚举值：`4`。 | — |
| <a id="api-48c1ee04b948c13a"></a>[`Inno.Native.ImGui.ImGuiNavMoveFlags.WrapY = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1561) | 当前常量或枚举值：`8`。 | — |
| <a id="api-9dfe5aaf41ca9f25"></a>[`public enum Inno.Native.ImGui.ImGuiNavMoveFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1555) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiNavRenderCursorFlags</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiNavRenderCursorFlags bcf7e397c511c64cf04421eb08aa7f969a0f489876381715359ab86b47e2897d -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavRenderCursorFlags.None f904211ef9396a34af978a912163c455a5b3c7bf10ddf9b5cf739ccede6a1990 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavRenderCursorFlags.Compact 0201831572db08c20d9013c5d7a1f4f0bf77f6c134be3e53bf4313901884c264 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavRenderCursorFlags.AlwaysDraw 6d3ed4b73c089bedcb509c0eb840f4af64c63c61f93a0027455d09376413cfe9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNavRenderCursorFlags.NoRounding 513a0bb2ccacd3fc449fadbdbb9c9b26193daa97fe85f6b56689796fceef5df5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-16d88d64ea25a5f2"></a>[`Inno.Native.ImGui.ImGuiNavRenderCursorFlags.AlwaysDraw = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1548) | 当前常量或枚举值：`4`。 | — |
| <a id="api-a40da3a1cbe06c18"></a>[`Inno.Native.ImGui.ImGuiNavRenderCursorFlags.Compact = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1547) | 当前常量或枚举值：`2`。 | — |
| <a id="api-cc4f9ac7d13f5a8a"></a>[`Inno.Native.ImGui.ImGuiNavRenderCursorFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1546) | 当前常量或枚举值：`0`。 | — |
| <a id="api-41a4011dd6008b53"></a>[`Inno.Native.ImGui.ImGuiNavRenderCursorFlags.NoRounding = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1549) | 当前常量或枚举值：`8`。 | — |
| <a id="api-4784c9a02d4c0d9b"></a>[`public enum Inno.Native.ImGui.ImGuiNavRenderCursorFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1544) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiNextItemData</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiNextItemData 494a02ddd20ff5ad0d5bdfe1dcaa3fbb34880f498ecfd6f0781fc2c8c2c89a49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.HasFlags 1cb67f02b2e72b87571daf2e06b944913f51921a18bccc15e0699297866721ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.ItemFlags 93c0b1090bd3605206dfb2b924b9eb9f838f9628dbd33e967441443a36d8a027 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.FocusScopeId 3bbc25b999b5fe20436e202e8e7ccc58f29f6a543fab1c4d314ca8af6e3a56f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.SelectionUserData 102887142e116387733e0ab9eb738c28b707ee2a0963a0e9bb488f5b76ab1be6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.Width 8a59fbd9a81ecd3c592b70749db5ceb5c0e7c869028e03a9e0c195b71ad0f2b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.Shortcut 5d154e5da63b5829bb287d969cc0866c7e1f3d1b7a5c1a18fad1cc6308d9d0cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.ShortcutFlags 146e8b86ffcb74cdc28bf4e3e0daccb4ab97af8f14fd713976defc539f991c8f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.OpenVal 8fa21e7f09b30177ff787ccdfde301dc8cbdc2e420a676a98e9f5e71d7a4d72a -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.OpenCond 68b27ac107da2f002d6917751413629817185854358fc0508c55b401290a0c88 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.RefVal e724cc731638321927f444c7761ab72c77e84911570bf0d10ee65420dbaf4fac -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.StorageId 95525cfe6b7c26b69df59c034758338a624cc4f621b85cd069e5368078f68a9f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemData.ColorMarker c921e559d5c3373f4b08d165339e28a73ec9058d38e64be62355ea19b765ead4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextItemData.#ctor(Inno.Native.ImGui.ImGuiNextItemDataFlags,Inno.Native.ImGui.ImGuiItemFlags,System.UInt32,System.Int64,System.Single,System.Int32,Inno.Native.ImGui.ImGuiInputFlags,System.Byte,System.Byte,Inno.Native.ImGui.ImGuiDataTypeStorage,System.UInt32,System.UInt32) aee619d44eb1f57f52b33570f57d880b9992d8b8a5a5487afddd99a3c926cd10 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextItemData.Destroy 2a4cc906814f75271c919020ba1a28ea3b9464591138fe5a3d7d012c5df9a806 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextItemData.ClearFlags dcc5b567abf70dec7eba39481788d39fa54714d3b18a47a80426b53826703e3c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d491bf67bcef932e"></a>[`public uint Inno.Native.ImGui.ImGuiNextItemData.ColorMarker`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5575) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc57b839fe5c7c91"></a>[`public uint Inno.Native.ImGui.ImGuiNextItemData.FocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5566) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-33471d71c1ef43ef"></a>[`public Inno.Native.ImGui.ImGuiNextItemDataFlags Inno.Native.ImGui.ImGuiNextItemData.HasFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5564) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1175cc7595b45a57"></a>[`public Inno.Native.ImGui.ImGuiItemFlags Inno.Native.ImGui.ImGuiNextItemData.ItemFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5565) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7d7c09f455703345"></a>[`public byte Inno.Native.ImGui.ImGuiNextItemData.OpenCond`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5572) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ff785e33e7fdc232"></a>[`public byte Inno.Native.ImGui.ImGuiNextItemData.OpenVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5571) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b20e517f6aca3d07"></a>[`public Inno.Native.ImGui.ImGuiDataTypeStorage Inno.Native.ImGui.ImGuiNextItemData.RefVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5573) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6fc9eeb650c5352"></a>[`public long Inno.Native.ImGui.ImGuiNextItemData.SelectionUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5567) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb304c1176025c44"></a>[`public int Inno.Native.ImGui.ImGuiNextItemData.Shortcut`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5569) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1bde502810227a8b"></a>[`public Inno.Native.ImGui.ImGuiInputFlags Inno.Native.ImGui.ImGuiNextItemData.ShortcutFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5570) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-40f80813c47c83f0"></a>[`public uint Inno.Native.ImGui.ImGuiNextItemData.StorageId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5574) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-00c383a21c1cecbe"></a>[`public float Inno.Native.ImGui.ImGuiNextItemData.Width`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5568) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b0829370c5ed31f5"></a>[`public Inno.Native.ImGui.ImGuiNextItemData.ImGuiNextItemData(Inno.Native.ImGui.ImGuiNextItemDataFlags hasFlags = Inno.Native.ImGui.ImGuiNextItemDataFlags.None, Inno.Native.ImGui.ImGuiItemFlags itemFlags = Inno.Native.ImGui.ImGuiItemFlags.None, uint focusScopeId = 0, long selectionUserData = 0, float width = 0, int shortcut = 0, Inno.Native.ImGui.ImGuiInputFlags shortcutFlags = Inno.Native.ImGui.ImGuiInputFlags.None, byte openVal = 0, byte openCond = 0, Inno.Native.ImGui.ImGuiDataTypeStorage refVal = default(Inno.Native.ImGui.ImGuiDataTypeStorage), uint storageId = 0, uint colorMarker = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5576) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-16f616fbb2148af6"></a>[`public void Inno.Native.ImGui.ImGuiNextItemData.ClearFlags()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66767) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f40f2fc6b217bfd3"></a>[`public void Inno.Native.ImGui.ImGuiNextItemData.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66756) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8b32205ebdb18490"></a>[`public struct Inno.Native.ImGui.ImGuiNextItemData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5562) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiNextItemDataFlags</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiNextItemDataFlags 7e37b5ab85632d22801a87f2bc7092a44da2a92c689a5f463e177fc1c5345ce3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.None a35cb6681aa33de2b3b97f88d3979a0bb620f922a5d7c89019d1142fd3519cae -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.HasWidth ccef2d657540610ea3ee9b7d482ab39ceacc4a062c76be5716a5c050a21d67b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.HasOpen 9b4488041734bde5c07920e22a5a47b44d14db491deb47bd79e087d8671b3958 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.HasShortcut 88fd8018f54b2d1f54be5914b2bb3a35355ea86c24b7485bda21eded64ae5cf5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.HasRefVal b1e2b3d39cb3aac9ba6e1b69e79c8be03d0bc8a618f65af2960767be68499ed5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.HasStorageId fb2a494e0093bbd852078e3ecdd127e9e958d8eab68022b6a7e80e8188c511cf -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataFlags.HasColorMarker 0fb2bbbbcacaf94890863f5876834e18b86ca255a6459d6c22dd3f3d4c375067 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fd03df99c64b1312"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.HasColorMarker = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1466) | 当前常量或枚举值：`32`。 | — |
| <a id="api-6acf87a92133940e"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.HasOpen = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1462) | 当前常量或枚举值：`2`。 | — |
| <a id="api-f55e0942bcba1bbb"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.HasRefVal = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1464) | 当前常量或枚举值：`8`。 | — |
| <a id="api-9292c49b71a9068b"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.HasShortcut = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1463) | 当前常量或枚举值：`4`。 | — |
| <a id="api-7f0483168eae586b"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.HasStorageId = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1465) | 当前常量或枚举值：`16`。 | — |
| <a id="api-00e6883ff1b956d9"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.HasWidth = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1461) | 当前常量或枚举值：`1`。 | — |
| <a id="api-4735e874c3e4c2c3"></a>[`Inno.Native.ImGui.ImGuiNextItemDataFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1460) | 当前常量或枚举值：`0`。 | — |
| <a id="api-9b5ba0848d7511b5"></a>[`public enum Inno.Native.ImGui.ImGuiNextItemDataFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1458) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiNextItemDataPtr</code> · 27 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiNextItemDataPtr 9d964c817d18a86a042b1e2d6f80e5134d8d8fa770460b9f1da4afc57547dd77 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextItemDataPtr.#ctor(Inno.Native.ImGui.ImGuiNextItemData*) fcff4307557cb7dec1f34f296f9469dac299486025bf29d6a2f01d679c10381d -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextItemDataPtr.Handle d49cb851d6d97639793855e68e365c325b93df0de1b56bac08027bc23554cd10 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.IsNull 157b37b0de66407a02c1d51e70592f47907da0315d8325185bcd919c58e07434 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.Null 78ff33ff88dd2757cb6f1caf568b1466473c31825370617fb2f1ead00eae11c3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.Item(System.Int32) 425cd876ad579e4a745d95b7854fa313ae7d0d9a6c1faaad83f26a7062e2cecb -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextItemDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiNextItemData*)~Inno.Native.ImGui.ImGuiNextItemDataPtr ae9e6d5682962da255c083263662e3e9b54d1deca8ac132abba81aeb6b3ea686 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextItemDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiNextItemDataPtr)~Inno.Native.ImGui.ImGuiNextItemData* 72b84d222cea71d9deec0557bca64dc319c13a06d0efeb0cac8698847604b791 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextItemDataPtr.op_Equality(Inno.Native.ImGui.ImGuiNextItemDataPtr,Inno.Native.ImGui.ImGuiNextItemDataPtr) 638307377c4e0bc96028442618dcb3a014b4cefe924ff56022161296ed3d4f82 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextItemDataPtr.op_Inequality(Inno.Native.ImGui.ImGuiNextItemDataPtr,Inno.Native.ImGui.ImGuiNextItemDataPtr) a84b4accc10a02b6a7e952de2765c7aff8270f03ef9f4bcc4a99f47afb5757ae -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextItemDataPtr.Equals(Inno.Native.ImGui.ImGuiNextItemDataPtr) f2dda186f4afafbb92cbeaeecb34e49d7f01a751071664ae359891437ab29e6d -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextItemDataPtr.Equals(System.Object) c9f6297b3f480bbc367d7735709e21ff3da65aefc56e6c6d5f1a19e9840a3432 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextItemDataPtr.GetHashCode 0b8c808d2b4d3ed4a9595f4915f71fd1b31eb0b13c2dc67e3f924abfd165c38e -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.HasFlags 32c9aca269e22675c05b7bfca626e47e2c95adecf09d3bf48bab6acb3aca79f7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.ItemFlags 5a15c1e0a8bc130f19415ceb28cfa1d03d3f2f66a26ef6d8a36554223c66f715 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.FocusScopeId 41467ea74a06df0c5c4dda8fffa56466aa31be8d3adc22f119705855c95167e4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.SelectionUserData 6676046cb82856124d283f39ee45e2ecd2d9e7c23d16c8cb5740e300d60ca006 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.Width a0fb5cf5e91945c2446c82c988a2cfb7fb396995691eeb1376abe28d9888f1ea -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.Shortcut a2eb51253a67d3d9c87bbceea94e4b5406696d680d1fe218c9c9f5932e5d483c -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.ShortcutFlags 2dac43d689b8370463552af79a4103aa4b7ae579add24bbbf20f4eb511b6c240 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.OpenVal 977efca015a5dad22a3175cb9b5fdd98a904df4b5bcc6d78b73278238fdcdb30 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.OpenCond dbff9fc42da073720e642521cd3316c31c782fd23afd528bb2ecc7f2191a4779 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.RefVal 26dee1070683d0dba79b1ed8cf175110ff97d4a9ce54ab4f9b4eca6fe5154da0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.StorageId d4baad8902c1b55e4d6173854f822dc36934474f10115f05b236049c6bc97c10 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextItemDataPtr.ColorMarker 238228cd4a65c97207ac70fefbbe8f06b24a4c45df3bf0207090d7743c585cfe -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextItemDataPtr.Destroy e2cc0dc0d9615b53385471dc6bb2fabf1a7815431235634ab39e6ccfc2830dfd -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextItemDataPtr.ClearFlags 258c98f4ea55cc688ede756b6d8bb8d14b9776e4b1f48c11a30dd6415c68615c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-71642ae7dfd0f666"></a>[`public Inno.Native.ImGui.ImGuiNextItemData* Inno.Native.ImGui.ImGuiNextItemDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15424) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5d3014a657d38db3"></a>[`public Inno.Native.ImGui.ImGuiNextItemDataPtr.ImGuiNextItemDataPtr(Inno.Native.ImGui.ImGuiNextItemData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15419) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a960ed99e137c630"></a>[`public void Inno.Native.ImGui.ImGuiNextItemDataPtr.ClearFlags()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15462) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-633166d59eebc9e9"></a>[`public void Inno.Native.ImGui.ImGuiNextItemDataPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15454) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2d1ad5bf673bc9d3"></a>[`public bool Inno.Native.ImGui.ImGuiNextItemDataPtr.Equals(Inno.Native.ImGui.ImGuiNextItemDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15434) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-71a24669deb9ea41"></a>[`public override bool Inno.Native.ImGui.ImGuiNextItemDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15435) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-c5e279d07d8d8a0b"></a>[`public override int Inno.Native.ImGui.ImGuiNextItemDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15436) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-66467b923d91f8fe"></a>[`public static bool Inno.Native.ImGui.ImGuiNextItemDataPtr.operator ==(Inno.Native.ImGui.ImGuiNextItemDataPtr left, Inno.Native.ImGui.ImGuiNextItemDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15432) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c67541f15c8f8bf0"></a>[`public static Inno.Native.ImGui.ImGuiNextItemDataPtr.implicit operator Inno.Native.ImGui.ImGuiNextItemDataPtr(Inno.Native.ImGui.ImGuiNextItemData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15430) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8385bdf497615d44"></a>[`public static Inno.Native.ImGui.ImGuiNextItemDataPtr.implicit operator Inno.Native.ImGui.ImGuiNextItemData*(Inno.Native.ImGui.ImGuiNextItemDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15431) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d1ae60eb78882501"></a>[`public static bool Inno.Native.ImGui.ImGuiNextItemDataPtr.operator !=(Inno.Native.ImGui.ImGuiNextItemDataPtr left, Inno.Native.ImGui.ImGuiNextItemDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15433) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5a20b709d111192c"></a>[`public ref uint Inno.Native.ImGui.ImGuiNextItemDataPtr.ColorMarker { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15449) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-df5303b839bc61cd"></a>[`public ref uint Inno.Native.ImGui.ImGuiNextItemDataPtr.FocusScopeId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15440) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b2c8af43af39c083"></a>[`public ref Inno.Native.ImGui.ImGuiNextItemDataFlags Inno.Native.ImGui.ImGuiNextItemDataPtr.HasFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15438) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7dfef9f39451d1ee"></a>[`public bool Inno.Native.ImGui.ImGuiNextItemDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15425) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d005ff9f9203fbae"></a>[`public Inno.Native.ImGui.ImGuiNextItemData Inno.Native.ImGui.ImGuiNextItemDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15428) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-19f29d2f778bd58c"></a>[`public ref Inno.Native.ImGui.ImGuiItemFlags Inno.Native.ImGui.ImGuiNextItemDataPtr.ItemFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15439) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9435a099000b2738"></a>[`public static Inno.Native.ImGui.ImGuiNextItemDataPtr Inno.Native.ImGui.ImGuiNextItemDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15426) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9b0dc059afcff302"></a>[`public ref byte Inno.Native.ImGui.ImGuiNextItemDataPtr.OpenCond { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15446) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0b3377947efee387"></a>[`public bool Inno.Native.ImGui.ImGuiNextItemDataPtr.OpenVal { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15445) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c550ffcf32697963"></a>[`public ref Inno.Native.ImGui.ImGuiDataTypeStorage Inno.Native.ImGui.ImGuiNextItemDataPtr.RefVal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15447) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c168c6ec1e6ec960"></a>[`public ref long Inno.Native.ImGui.ImGuiNextItemDataPtr.SelectionUserData { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15441) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c68fcdd23c7abf63"></a>[`public ref int Inno.Native.ImGui.ImGuiNextItemDataPtr.Shortcut { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15443) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bd7f0558d5d379d2"></a>[`public ref Inno.Native.ImGui.ImGuiInputFlags Inno.Native.ImGui.ImGuiNextItemDataPtr.ShortcutFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15444) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ed1bd5464e60006d"></a>[`public ref uint Inno.Native.ImGui.ImGuiNextItemDataPtr.StorageId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15448) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5355e74c6e1cc955"></a>[`public ref float Inno.Native.ImGui.ImGuiNextItemDataPtr.Width { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15442) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0d785a67836a55cb"></a>[`public struct Inno.Native.ImGui.ImGuiNextItemDataPtr : System.IEquatable<Inno.Native.ImGui.ImGuiNextItemDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15417) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiNextWindowData</code> · 27 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiNextWindowData 86d92a40b8ec85de42a8a16632065fb4afe79d09fb83c3539df37fae3507229d -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.HasFlags 7c73ad21ecbc69010b2e4d551129e9ca77d26d78d528abe27f42d6cb7dddcede -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.PosCond 6c056fc0fa6eba8e007a0591f1860d286dbfda7f75e1eff600fe3ea48bca90d8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.SizeCond 049068dabac03c1c7cc2a94e81a833729b0bc8cd3ef70da4a2e96ea99f9edee4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.CollapsedCond baa486c5579812b0ddc30db695ab821e2c6099a504d80a34a6c0df3f8e59b253 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.DockCond 472e96edad1d707f2ad1d8308ed8d8f360944c831c918c0e5b0a7d86c9f9e088 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.PosVal 58b1805b114fb414f26826a294e38515d5f55fb24fa4f8db508edd22c30fde64 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.PosPivotVal 5da873c6c3bcff50451a9c862883c1c7dbdbce9aab6904298ab9762edc053123 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.SizeVal 744c3a304d1ee9e5e7ed68159363d4510060c946d14ee914a8d38a41cb60377e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.ContentSizeVal 16fcd99e899d7e26d2253b6f6a3e790a11593a246d8eb65be42c84854b32208d -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.ScrollVal 3aa70aa360f387b9ffef3ed201ec1002319977282c5bb5d272a60f2eda1d2b20 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.WindowFlags 4980cef179089bbdc6958a0e82209f1dbeda300fa7df0232e72602af7a5d1edb -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.ChildFlags 1ef6cceb946b120c2f5377827bf70166c338d8949de86f5a9e9e90c15e3cd755 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.PosUndock dc5def497fc22e9a8758c62115080945e3be831996150c0b5548427aa36e89ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.CollapsedVal 6c89d022d814a93f3e70e631cfb0009899ef46c74ef2ec4617a78a3c66473880 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.SizeConstraintRect b5cf571851d0c14dfa813e68ee5f7a65b6cdd5ea0f74b2fc77adcfc204b5253b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.SizeCallback 307377675d8e0dfbaa024c462356847c98854181e82d4b4c22786728dd59ed3c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.SizeCallbackUserData a4335d3de151b91722e2885979dbd4ece2191ac317734aa2ab61ab3a7da7d9a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.BgAlphaVal 61a8e877468692b8755b702ecba0874382b8da209ab1d55494df404f6d79a51c -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.ViewportId aa91b4a368aaf76d6a21b396c97716c7ce4a2f26f9c868b658361ceca306308f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.DockId 26b82e00f340ae0b41f083f370dbbbc21db76dea7d4de90039ae246fa4092553 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.WindowClass 808da3c9ef05c0f9f29b17ed215aab5ec75c475d86711e7eb263d172591e22ee -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.MenuBarOffsetMinVal f24199279f67c82673a6e63cb2b42f62174f92ffdec200cce7c51e90ac5a37fd -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowData.RefreshFlagsVal 45c8f0fb0a2d4c987d693f36c658887fc3b706b5066671e1e80b0ecd38fe02f1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextWindowData.#ctor(Inno.Native.ImGui.ImGuiNextWindowDataFlags,Inno.Native.ImGui.ImGuiCond,Inno.Native.ImGui.ImGuiCond,Inno.Native.ImGui.ImGuiCond,Inno.Native.ImGui.ImGuiCond,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,Inno.Native.ImGui.ImGuiWindowFlags,Inno.Native.ImGui.ImGuiChildFlags,System.Byte,System.Byte,Inno.Native.ImGui.ImRect,,System.Void*,System.Single,System.UInt32,System.UInt32,Inno.Native.ImGui.ImGuiWindowClass,System.Numerics.Vector2,Inno.Native.ImGui.ImGuiWindowRefreshFlags) 224faaeb3c9474f3b2b1847964f527307475b6cd9a0fe83d7f8e62a33514df00 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextWindowData.Destroy 48865b4535f464bed2c6a868ef5564b0a0110c95be098d2871e1962fd78d8665 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextWindowData.ClearFlags 3cb6cab69478ee6e9e9e38afb98f4e370766ef0d8c1c01e8122be899d27b0f66 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-86d29d402f4e5f60"></a>[`public float Inno.Native.ImGui.ImGuiNextWindowData.BgAlphaVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5635) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0fbdd803c1ba8087"></a>[`public Inno.Native.ImGui.ImGuiChildFlags Inno.Native.ImGui.ImGuiNextWindowData.ChildFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5629) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c79505681c34b29"></a>[`public Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiNextWindowData.CollapsedCond`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5621) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20dd4d8f1f2bcff7"></a>[`public byte Inno.Native.ImGui.ImGuiNextWindowData.CollapsedVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5631) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8eaa2a72fa57f436"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiNextWindowData.ContentSizeVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5626) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2ef69a4e19ec6f8"></a>[`public Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiNextWindowData.DockCond`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5622) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7aadfda3b2aac1f3"></a>[`public uint Inno.Native.ImGui.ImGuiNextWindowData.DockId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5637) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-29a625e3b066b3da"></a>[`public Inno.Native.ImGui.ImGuiNextWindowDataFlags Inno.Native.ImGui.ImGuiNextWindowData.HasFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5618) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4553a823ed5d9010"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiNextWindowData.MenuBarOffsetMinVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5639) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a3703461e6a11d51"></a>[`public Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiNextWindowData.PosCond`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5619) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-979020b50da0714b"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiNextWindowData.PosPivotVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5624) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21c2a7d25d49d195"></a>[`public byte Inno.Native.ImGui.ImGuiNextWindowData.PosUndock`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5630) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d08f153db4466a5c"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiNextWindowData.PosVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5623) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8df1bd51096b796a"></a>[`public Inno.Native.ImGui.ImGuiWindowRefreshFlags Inno.Native.ImGui.ImGuiNextWindowData.RefreshFlagsVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5640) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ade3c9f8b87e5222"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiNextWindowData.ScrollVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5627) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fdde0aeda027e16b"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSizeCallbackData*, void> Inno.Native.ImGui.ImGuiNextWindowData.SizeCallback`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5633) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9e4bacb1ba9cc426"></a>[`public void* Inno.Native.ImGui.ImGuiNextWindowData.SizeCallbackUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5634) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7dc342259498e60b"></a>[`public Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiNextWindowData.SizeCond`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5620) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ef9dbbb24ebab29"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiNextWindowData.SizeConstraintRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5632) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1880d9064d79e547"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiNextWindowData.SizeVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5625) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df94efd4a397f0cd"></a>[`public uint Inno.Native.ImGui.ImGuiNextWindowData.ViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5636) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b794191bc86cf6f0"></a>[`public Inno.Native.ImGui.ImGuiWindowClass Inno.Native.ImGui.ImGuiNextWindowData.WindowClass`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5638) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-90a53b200c7488a1"></a>[`public Inno.Native.ImGui.ImGuiWindowFlags Inno.Native.ImGui.ImGuiNextWindowData.WindowFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5628) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8e1ddac7b9c3a4dc"></a>[`public Inno.Native.ImGui.ImGuiNextWindowData.ImGuiNextWindowData(Inno.Native.ImGui.ImGuiNextWindowDataFlags hasFlags = Inno.Native.ImGui.ImGuiNextWindowDataFlags.None, Inno.Native.ImGui.ImGuiCond posCond = Inno.Native.ImGui.ImGuiCond.None, Inno.Native.ImGui.ImGuiCond sizeCond = Inno.Native.ImGui.ImGuiCond.None, Inno.Native.ImGui.ImGuiCond collapsedCond = Inno.Native.ImGui.ImGuiCond.None, Inno.Native.ImGui.ImGuiCond dockCond = Inno.Native.ImGui.ImGuiCond.None, System.Numerics.Vector2 posVal = default(System.Numerics.Vector2), System.Numerics.Vector2 posPivotVal = default(System.Numerics.Vector2), System.Numerics.Vector2 sizeVal = default(System.Numerics.Vector2), System.Numerics.Vector2 contentSizeVal = default(System.Numerics.Vector2), System.Numerics.Vector2 scrollVal = default(System.Numerics.Vector2), Inno.Native.ImGui.ImGuiWindowFlags windowFlags = Inno.Native.ImGui.ImGuiWindowFlags.None, Inno.Native.ImGui.ImGuiChildFlags childFlags = Inno.Native.ImGui.ImGuiChildFlags.None, byte posUndock = 0, byte collapsedVal = 0, Inno.Native.ImGui.ImRect sizeConstraintRect = default(Inno.Native.ImGui.ImRect), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSizeCallbackData*, void> sizeCallback = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSizeCallbackData*, void>), void* sizeCallbackUserData = null, float bgAlphaVal = 0, uint viewportId = 0, uint dockId = 0, Inno.Native.ImGui.ImGuiWindowClass windowClass = default(Inno.Native.ImGui.ImGuiWindowClass), System.Numerics.Vector2 menuBarOffsetMinVal = default(System.Numerics.Vector2), Inno.Native.ImGui.ImGuiWindowRefreshFlags refreshFlagsVal = Inno.Native.ImGui.ImGuiWindowRefreshFlags.None)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5641) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9d5287c39a75f096"></a>[`public void Inno.Native.ImGui.ImGuiNextWindowData.ClearFlags()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66742) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-38fa5b2ba86a151a"></a>[`public void Inno.Native.ImGui.ImGuiNextWindowData.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66731) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-75c0e261302dd442"></a>[`public struct Inno.Native.ImGui.ImGuiNextWindowData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5616) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiNextWindowDataFlags</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiNextWindowDataFlags 0acdebe373889d66fc17f2dc9088684cf2998b384c9c7217b78153efd8615bfc -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.None 73e048ef8d7a1337b6064529571aa084b21e01d4f1304768bf7d34fbc3142a87 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasPos 9d879e7dba9413f1c0a7460304c61ce1258500be5f92476ded18a5dbfde3a370 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasSize 573fcf85da4cb5e1947dcaa764d4ac07d0e469b19438c2b6d3ca80dfb4fd56e3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasContentSize bfc1c67cdb016dec93c77b6c99dae90a02406af7e67b601b7e18629f46a3c2ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasCollapsed 413582f02a1c3effdef144609d2307de889f48cde66272a4531b10452369182e -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasSizeConstraint 9b2d87a30c211b6e1acad3b62c06497d395c45fa59cac8669042317e78c5e43f -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasFocus 77369f098e2d5bf0c8a0c4fdc4ce846c4295d48be52fc3e10c46b552bf662fe5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasBgAlpha 444cdb211fcf550173269833490f1fe91e801f11471ec60fd25f47e2f76968e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasScroll 310fff112b9c0f0f42f2604f85e61c29be3e3f2675025da92e5ab09bff3b4cbe -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasWindowFlags 846b7227a6a7c0291fa0cf59b0402acd767bb0fb9e1b51a9a88d45b6c7d8ffbd -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasChildFlags 997e4aa2e36e45f260488af67cb9c295f03e743855506040bcb34d69c8040538 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasRefreshPolicy 0b84136afba323d84149e487f026d1576ad954b5c7514db2b6f7e363e0592de7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasViewport b295d2f703068fd3109e7af324c9b9e52ec412749d2d395b26ffb97b99e2845b -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasDock 3f628a3af9b85e7098a918ac423cba637910c2b698f2ba8d94622d9f7ac47185 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasWindowClass d3d8824c229d749c7ed50d81bb5342689f008f93c94d50058bd91edfe206c6b7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b4e8214aa6c4e50d"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasBgAlpha = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1445) | 当前常量或枚举值：`64`。 | — |
| <a id="api-b3bc68d7755d69b2"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasChildFlags = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1448) | 当前常量或枚举值：`512`。 | — |
| <a id="api-db48cd40ee3faae7"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasCollapsed = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1442) | 当前常量或枚举值：`8`。 | — |
| <a id="api-6c216fc568a09e5e"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasContentSize = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1441) | 当前常量或枚举值：`4`。 | — |
| <a id="api-54e1d79f007edd2e"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasDock = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1451) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-b3f19b666803679e"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasFocus = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1444) | 当前常量或枚举值：`32`。 | — |
| <a id="api-2056be70590de589"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasPos = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1439) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d50bf76c02a575a5"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasRefreshPolicy = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1449) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-f4885673bbb9a161"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasScroll = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1446) | 当前常量或枚举值：`128`。 | — |
| <a id="api-a3338413d6b93105"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasSize = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1440) | 当前常量或枚举值：`2`。 | — |
| <a id="api-aec4b4a5be689c54"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasSizeConstraint = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1443) | 当前常量或枚举值：`16`。 | — |
| <a id="api-295a7987927b854c"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasViewport = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1450) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-5851856dfd6e804a"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasWindowClass = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1452) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-0dd70be3e78252c0"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.HasWindowFlags = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1447) | 当前常量或枚举值：`256`。 | — |
| <a id="api-e45cf9c0b471ac47"></a>[`Inno.Native.ImGui.ImGuiNextWindowDataFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1438) | 当前常量或枚举值：`0`。 | — |
| <a id="api-f0f93b938fa99d7b"></a>[`public enum Inno.Native.ImGui.ImGuiNextWindowDataFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1436) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiNextWindowDataPtr</code> · 38 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiNextWindowDataPtr d6cac0b874d0f67aa81444f86df49ed1968020e7928326cea775e267af2e9d2c -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextWindowDataPtr.#ctor(Inno.Native.ImGui.ImGuiNextWindowData*) c6c9f90eba427f68510b9f0fea3b9413945c8a8122863efe900ab6d9ee400322 -->
<!-- api: F:Inno.Native.ImGui.ImGuiNextWindowDataPtr.Handle 6df947b81e8bd0572f513e0b265790615e6b1923166c308d7e85fbd931614f8d -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.IsNull 61c38f2895ce18ed06b76d45e40857864791b78d789abf2663723814fdda627f -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.Null 46b9c01571311c60d88d9ad92e55c2590934811a7b257d75f50686c4e5c8ebd3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.Item(System.Int32) be1256afd3c546ab791b52542bc7b65832f786f6aed2500a57283142c4e3a4cf -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextWindowDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiNextWindowData*)~Inno.Native.ImGui.ImGuiNextWindowDataPtr c9ab8fa3bdd29dfe331dceec7ae4fea222c59c641b5d2fa8de2c0ad4af620610 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextWindowDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiNextWindowDataPtr)~Inno.Native.ImGui.ImGuiNextWindowData* 3e96598d68b8226260fe4b9ff171fcf3b7f1bf321b03d1fd6561efa92488be6e -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextWindowDataPtr.op_Equality(Inno.Native.ImGui.ImGuiNextWindowDataPtr,Inno.Native.ImGui.ImGuiNextWindowDataPtr) 29dd82b1b93ab4631a8532a6cbf2eaa12e5278b5b4bed7ee73b04e51c1e7bf1d -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextWindowDataPtr.op_Inequality(Inno.Native.ImGui.ImGuiNextWindowDataPtr,Inno.Native.ImGui.ImGuiNextWindowDataPtr) 6f6217240af67db08a1fac556f43c6d5d833735ee703c8933065c394fa94235b -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextWindowDataPtr.Equals(Inno.Native.ImGui.ImGuiNextWindowDataPtr) 5941a9312781fcab48f79a2b7c19d3efab3de5786c3bd6f31f87ecbb585b2d76 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextWindowDataPtr.Equals(System.Object) 5237beb9aaf753e28cd82b6ebdc4d6a86d461e8eccb49ed78e1b6904bf170b82 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextWindowDataPtr.GetHashCode 8e95b8ffed7dd627d2fa574cede39382fd07069b9580c8e6975d135c621c0363 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.HasFlags bae29a5a55d301435acc6d9611a183520222e08b120baf4cb7be72e76d32cada -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.PosCond 0daa4b8b798b93c643a24db79bfdc2afa90c695bbecc13bbccb693cf80420e4e -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.SizeCond fa938be0d96d72072ddd912caa48ff12aee61a56af1f87df0b4c7f865906ad98 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.CollapsedCond f8026bdbea0166fad823329a34c863492160a2ee67256ea50ef59bbfa08b7599 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.DockCond d8279cf37b16d400ec92c8482dad44d1d16c72a2c27fb6a6b7fad6f2000deab9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.PosVal 0d3289e1f00615fcf64454d88c27bc9eee603c20172c669e0444f02d2ad39e2c -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.PosPivotVal c0a9c88003b6d943c64f1ee8a8fa7469f7d9c8405cc6f139a36b2e8919c7f715 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.SizeVal c1bba4342706223168e57c23dad1733c9ed263db769bbdf6e0e793bf5205ef8f -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.ContentSizeVal aa2162ac37241e21414376e1d322a8050d4dd6f9cc78fd99883e0e238572488b -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.ScrollVal d65fa38691510ea9d538213daf3be9f5339300802e6d6a551ac278b2acd67502 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.WindowFlags 30d3ed2c45e1cf7c8301c3b76654c4f8597d9b9a138f03576dde92aa37e0c47c -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.ChildFlags 21ce3e408d4ab083464693ffe152016a9d1a198ef40be7b6f6d211955d4ae1fd -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.PosUndock 2430c5081cc19573051e1996fdb178ccd32e89e2593325f56c90fd26d252b49a -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.CollapsedVal c983f340696b61193b328ed78b49f42fbafcbdf3574766d5a9add7cfdcf6774d -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.SizeConstraintRect c686300429f6414477770462c03bc5567f9864cc8784ffcf5e0781af94654042 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.SizeCallback 49760da7a0397705046a114370eb9123187fb50245961965336dd697c45598c5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.SizeCallbackUserData 3f3b70146e633a42537d2efe3b0e9fde353a3b06f58fd877c4f88d2a60967f24 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.BgAlphaVal 4e2dfad488dba3d29e40da9640850dadac0934eca308b6fc3ebe5c15060bd664 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.ViewportId 3cc43d637fc728b4ceee77cda5058333ce5f4db3ee46eb89b842ecff6adba9c6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.DockId 028b654c653867891d860bc6b04c9ccea6667e29490c4d4977283f73ecc681c0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.WindowClass d0ebadf4e4a3430a16e9f2a963e847c0b8fc78a1cfcbd6a08419050e6b04c7d9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.MenuBarOffsetMinVal e2e68016bff54902305d28e59f752949f2dd3ba134fb74a98442569750703207 -->
<!-- api: P:Inno.Native.ImGui.ImGuiNextWindowDataPtr.RefreshFlagsVal 6aafc0f7b998feec2542247c5af06ee42874bbe92e9bb6c55ca6431c6bc58509 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextWindowDataPtr.Destroy 0df2de3b8f7470427773dac0f150e18a9d9627ce38c23ef24af882813990fad5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiNextWindowDataPtr.ClearFlags bb936f99e5cb19bf04232a82b9f4055d96aac8e8fb6b186ff36e31982d477cd1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ff88fc9fd135a316"></a>[`public Inno.Native.ImGui.ImGuiNextWindowData* Inno.Native.ImGui.ImGuiNextWindowDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15476) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c9cf0c70f436c7c2"></a>[`public Inno.Native.ImGui.ImGuiNextWindowDataPtr.ImGuiNextWindowDataPtr(Inno.Native.ImGui.ImGuiNextWindowData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15471) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6771e362e40ced79"></a>[`public void Inno.Native.ImGui.ImGuiNextWindowDataPtr.ClearFlags()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15525) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3e8ad14fed4c8a37"></a>[`public void Inno.Native.ImGui.ImGuiNextWindowDataPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15517) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-57c138ea7d062eff"></a>[`public bool Inno.Native.ImGui.ImGuiNextWindowDataPtr.Equals(Inno.Native.ImGui.ImGuiNextWindowDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15486) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-a412366ff3ae6f08"></a>[`public override bool Inno.Native.ImGui.ImGuiNextWindowDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15487) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-8b5b698a7aaf4db7"></a>[`public override int Inno.Native.ImGui.ImGuiNextWindowDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15488) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-86d1e33af0b1b3ef"></a>[`public static bool Inno.Native.ImGui.ImGuiNextWindowDataPtr.operator ==(Inno.Native.ImGui.ImGuiNextWindowDataPtr left, Inno.Native.ImGui.ImGuiNextWindowDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15484) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2d1916ebaf2d0edf"></a>[`public static Inno.Native.ImGui.ImGuiNextWindowDataPtr.implicit operator Inno.Native.ImGui.ImGuiNextWindowDataPtr(Inno.Native.ImGui.ImGuiNextWindowData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15482) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a435de4d0e0e9bd0"></a>[`public static Inno.Native.ImGui.ImGuiNextWindowDataPtr.implicit operator Inno.Native.ImGui.ImGuiNextWindowData*(Inno.Native.ImGui.ImGuiNextWindowDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15483) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-df2b3ad118441a6e"></a>[`public static bool Inno.Native.ImGui.ImGuiNextWindowDataPtr.operator !=(Inno.Native.ImGui.ImGuiNextWindowDataPtr left, Inno.Native.ImGui.ImGuiNextWindowDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15485) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4c91b410a07f61ab"></a>[`public ref float Inno.Native.ImGui.ImGuiNextWindowDataPtr.BgAlphaVal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15507) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4bfe64b9f7e9b6b4"></a>[`public ref Inno.Native.ImGui.ImGuiChildFlags Inno.Native.ImGui.ImGuiNextWindowDataPtr.ChildFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15501) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c94d2b39ad5232de"></a>[`public ref Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiNextWindowDataPtr.CollapsedCond { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15493) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f90159c9c0e163c7"></a>[`public bool Inno.Native.ImGui.ImGuiNextWindowDataPtr.CollapsedVal { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15503) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2aa1ac559371e28e"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiNextWindowDataPtr.ContentSizeVal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15498) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0b4f7ee9d961191d"></a>[`public ref Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiNextWindowDataPtr.DockCond { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15494) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-987f9e1eb505c4f2"></a>[`public ref uint Inno.Native.ImGui.ImGuiNextWindowDataPtr.DockId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15509) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c41de3adfa6e845a"></a>[`public ref Inno.Native.ImGui.ImGuiNextWindowDataFlags Inno.Native.ImGui.ImGuiNextWindowDataPtr.HasFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15490) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8001143036344934"></a>[`public bool Inno.Native.ImGui.ImGuiNextWindowDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15477) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4bada7f58497b3f7"></a>[`public Inno.Native.ImGui.ImGuiNextWindowData Inno.Native.ImGui.ImGuiNextWindowDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15480) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-907b77012d9853ec"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiNextWindowDataPtr.MenuBarOffsetMinVal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15511) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ad1b5629c2f47713"></a>[`public static Inno.Native.ImGui.ImGuiNextWindowDataPtr Inno.Native.ImGui.ImGuiNextWindowDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15478) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b6ee94152ab412eb"></a>[`public ref Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiNextWindowDataPtr.PosCond { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15491) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9f131b3b3f52a5a0"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiNextWindowDataPtr.PosPivotVal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15496) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-777b8bc028dd3490"></a>[`public bool Inno.Native.ImGui.ImGuiNextWindowDataPtr.PosUndock { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15502) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b7089d0c4590e5e2"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiNextWindowDataPtr.PosVal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15495) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b54345a2b8e52c2a"></a>[`public ref Inno.Native.ImGui.ImGuiWindowRefreshFlags Inno.Native.ImGui.ImGuiNextWindowDataPtr.RefreshFlagsVal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15512) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fb6cee0eb4e292fc"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiNextWindowDataPtr.ScrollVal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15499) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e3787456264fe73a"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSizeCallbackData*, void> Inno.Native.ImGui.ImGuiNextWindowDataPtr.SizeCallback { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15505) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6e16560a025f1ea3"></a>[`public void* Inno.Native.ImGui.ImGuiNextWindowDataPtr.SizeCallbackUserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15506) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fd4a988742a8af19"></a>[`public ref Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiNextWindowDataPtr.SizeCond { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15492) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6f9bcb11aff252f3"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiNextWindowDataPtr.SizeConstraintRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15504) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5438a832eb85a4ff"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiNextWindowDataPtr.SizeVal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15497) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6633c105f5f89123"></a>[`public ref uint Inno.Native.ImGui.ImGuiNextWindowDataPtr.ViewportId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15508) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-71af77138616fed9"></a>[`public ref Inno.Native.ImGui.ImGuiWindowClass Inno.Native.ImGui.ImGuiNextWindowDataPtr.WindowClass { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15510) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-01625adccd4caf43"></a>[`public ref Inno.Native.ImGui.ImGuiWindowFlags Inno.Native.ImGui.ImGuiNextWindowDataPtr.WindowFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15500) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a007e8926e11efd9"></a>[`public struct Inno.Native.ImGui.ImGuiNextWindowDataPtr : System.IEquatable<Inno.Native.ImGui.ImGuiNextWindowDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15469) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiOldColumnData</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiOldColumnData a59a9d1939749b29a97901426c2192da8c36986f4cc65b8c50da612f3bbdc2eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnData.OffsetNorm 9375bf111997171b8dd9dc00a3d2f775102c037d3df0d251560c0985d4c7dd17 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnData.OffsetNormBeforeResize 03c1a1a433a299028b0908d876869faa3d4246699f2747715187e93c0fe7ecd7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnData.Flags 79d73373643d0cbf5f2b98ea10d84c3dfe90a428fbc053b07f4148431140c27a -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnData.ClipRect b897ce0d413e02aaafd9ec5b3b4ddd55e519bb31ec44d40f9be0385841380686 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnData.#ctor(System.Single,System.Single,Inno.Native.ImGui.ImGuiOldColumnFlags,Inno.Native.ImGui.ImRect) 6b2ea8836994f9eabda4eae42c6377d2ba48f69e709c707f8d2166aaf25906df -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnData.Destroy 129d7d3b0ea55b15333b11296573c0a6b2b6ceb0de6e5707bdb26d7d5907e91d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d36647322e6452a3"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiOldColumnData.ClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4111) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-99e68e8c6b122274"></a>[`public Inno.Native.ImGui.ImGuiOldColumnFlags Inno.Native.ImGui.ImGuiOldColumnData.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4110) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e1b28976a85209cd"></a>[`public float Inno.Native.ImGui.ImGuiOldColumnData.OffsetNorm`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4108) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-32fbd4dd030d8c17"></a>[`public float Inno.Native.ImGui.ImGuiOldColumnData.OffsetNormBeforeResize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4109) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4393693e420e058"></a>[`public Inno.Native.ImGui.ImGuiOldColumnData.ImGuiOldColumnData(float offsetNorm = 0, float offsetNormBeforeResize = 0, Inno.Native.ImGui.ImGuiOldColumnFlags flags = Inno.Native.ImGui.ImGuiOldColumnFlags.None, Inno.Native.ImGui.ImRect clipRect = default(Inno.Native.ImGui.ImRect))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4112) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-c762e7df4232b008"></a>[`public void Inno.Native.ImGui.ImGuiOldColumnData.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66979) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b9d3ba848c1f9833"></a>[`public struct Inno.Native.ImGui.ImGuiOldColumnData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4106) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiOldColumnDataPtr</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiOldColumnDataPtr 0671b0de99cbd18d8fb9ce10138c72cdfe81f6c27d2f5032bfd698ecf15aea45 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnDataPtr.#ctor(Inno.Native.ImGui.ImGuiOldColumnData*) 0fda51fbb4e34619ca0bbd42aa6496387246254f0eefa7bf484737fa0c20ecd7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnDataPtr.Handle 67891bcb089fe7a5bfb3116cf20f83596ba91479219ffe69407cc9980bc937e2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnDataPtr.IsNull 9309e09ffbb1b5c2b1b3f7cd50d7be38e3979f7ee85eb0732ad209c95b6ea0cd -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnDataPtr.Null c0b0b1b0e5320207a6c63d895485bdbe85e4904faa147c7b2b0e364da1c47a0e -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnDataPtr.Item(System.Int32) 189125713e3816405b4d8289c11e65211e9ae4f48a39c63ea0d90a20b66dbed0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiOldColumnData*)~Inno.Native.ImGui.ImGuiOldColumnDataPtr 5dbbff8bfdaaf21710b4ef53b3ebe8c0e0a12fc58f0f8bedd500777f284f673c -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiOldColumnDataPtr)~Inno.Native.ImGui.ImGuiOldColumnData* f7fe263a0321240785b76374be82e4901b7cc2f63256dd66eec2ae44b3de152e -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnDataPtr.op_Equality(Inno.Native.ImGui.ImGuiOldColumnDataPtr,Inno.Native.ImGui.ImGuiOldColumnDataPtr) 94b5246599aa47154ddde863a9c55404769bcbd7737a9846fef58c385031dba4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnDataPtr.op_Inequality(Inno.Native.ImGui.ImGuiOldColumnDataPtr,Inno.Native.ImGui.ImGuiOldColumnDataPtr) db7e73cfce5e8b6c1a33bdcdf343481e9760ce3f158aab47a852fe8296f2854f -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnDataPtr.Equals(Inno.Native.ImGui.ImGuiOldColumnDataPtr) 5f0d53d5b7eddd7dd6e7b224f4b223ad94b993a626d4aa10df2f6697126249a8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnDataPtr.Equals(System.Object) 3502b82d4244ebfcd978000eac542bedf6ebc0db09c491ae92f12c65cf518633 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnDataPtr.GetHashCode 29dfbefdbe3eb2af4770f90cf231f1f1943cc03a5fd9812a267c717abdc0273e -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnDataPtr.OffsetNorm a7495c0666c898af3f9aaee15b63190efac5bb5a87ab4c2a5e33c6e25c301256 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnDataPtr.OffsetNormBeforeResize 9068610eb7da9b3facc1f7c604b67a28bf73c947cbfecb992c35d64fb72a5ea4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnDataPtr.Flags 2cad1f78d30b3ba4e0ff9f4a3a517054bc38d01abf3062ec6761c9b61eb6958e -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnDataPtr.ClipRect db3a562a79a33e3dfa86093ac7862c2f176f78216fb590367298e9466fd49689 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnDataPtr.Destroy 6826f54d687c9f440d9aec0451c14cd398d36cd0fbd5d307a3020d48556849b2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-911c790f53f2df4c"></a>[`public Inno.Native.ImGui.ImGuiOldColumnData* Inno.Native.ImGui.ImGuiOldColumnDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15539) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3dc480126c25190a"></a>[`public Inno.Native.ImGui.ImGuiOldColumnDataPtr.ImGuiOldColumnDataPtr(Inno.Native.ImGui.ImGuiOldColumnData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15534) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0bcc1fd8642b135f"></a>[`public void Inno.Native.ImGui.ImGuiOldColumnDataPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15561) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9ef0c390ab8d462a"></a>[`public bool Inno.Native.ImGui.ImGuiOldColumnDataPtr.Equals(Inno.Native.ImGui.ImGuiOldColumnDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15549) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-653cdd47065d44f9"></a>[`public override bool Inno.Native.ImGui.ImGuiOldColumnDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15550) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-58f1710b2527ea2c"></a>[`public override int Inno.Native.ImGui.ImGuiOldColumnDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15551) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-dfa8e460d9d74989"></a>[`public static bool Inno.Native.ImGui.ImGuiOldColumnDataPtr.operator ==(Inno.Native.ImGui.ImGuiOldColumnDataPtr left, Inno.Native.ImGui.ImGuiOldColumnDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15547) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-930913564b96b3d0"></a>[`public static Inno.Native.ImGui.ImGuiOldColumnDataPtr.implicit operator Inno.Native.ImGui.ImGuiOldColumnDataPtr(Inno.Native.ImGui.ImGuiOldColumnData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15545) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ac278b5888f91364"></a>[`public static Inno.Native.ImGui.ImGuiOldColumnDataPtr.implicit operator Inno.Native.ImGui.ImGuiOldColumnData*(Inno.Native.ImGui.ImGuiOldColumnDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15546) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a7448d0d734e529b"></a>[`public static bool Inno.Native.ImGui.ImGuiOldColumnDataPtr.operator !=(Inno.Native.ImGui.ImGuiOldColumnDataPtr left, Inno.Native.ImGui.ImGuiOldColumnDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15548) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-31284e34cab29fac"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiOldColumnDataPtr.ClipRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15556) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cacb6fa3e5a5f6b7"></a>[`public ref Inno.Native.ImGui.ImGuiOldColumnFlags Inno.Native.ImGui.ImGuiOldColumnDataPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15555) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7ee153cbc2c70ebd"></a>[`public bool Inno.Native.ImGui.ImGuiOldColumnDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15540) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bf5f1d4cd5660ca0"></a>[`public Inno.Native.ImGui.ImGuiOldColumnData Inno.Native.ImGui.ImGuiOldColumnDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15543) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b380328e7c6abae6"></a>[`public static Inno.Native.ImGui.ImGuiOldColumnDataPtr Inno.Native.ImGui.ImGuiOldColumnDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15541) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-745049e91399356b"></a>[`public ref float Inno.Native.ImGui.ImGuiOldColumnDataPtr.OffsetNorm { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15553) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d81829ab1df59a51"></a>[`public ref float Inno.Native.ImGui.ImGuiOldColumnDataPtr.OffsetNormBeforeResize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15554) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2772787fd0b62534"></a>[`public struct Inno.Native.ImGui.ImGuiOldColumnDataPtr : System.IEquatable<Inno.Native.ImGui.ImGuiOldColumnDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15532) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiOldColumnFlags</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiOldColumnFlags ff9d6a28c01d9bdd8b2089efb052053e6be4d9cf1e782085dff549bdc51d8c95 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnFlags.None 02126e7a33654b2b46380fb67427ebc303a2380871cedd6b619e761caa280c59 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnFlags.NoBorder 3b3338852e3db2aeacb8e160dcd0229afa239efe1dadc2892aa95bb249484be2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnFlags.NoResize 267864d7b88b130a0983f2d781ec9917bcc97128551b1e7be1da238791fc9ab9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnFlags.NoPreserveWidths 488ed5d84b8d87ac42e8dee5e2e40d792ed8dc2546c6d98b4eece60f8059e94b -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnFlags.NoForceWithinWindow 70cda2b9c15ef52188ce82ae854bd429c946bc71267e7dc31f43d0d9b5431e63 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnFlags.GrowParentContentsSize 2856b7f6c7081232a5e85c398ad3c45624fba4f8599a26a162c47861e2b8c806 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-08eda66ba26cfc3b"></a>[`Inno.Native.ImGui.ImGuiOldColumnFlags.GrowParentContentsSize = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1597) | 当前常量或枚举值：`16`。 | — |
| <a id="api-0d7707738a2c7c5a"></a>[`Inno.Native.ImGui.ImGuiOldColumnFlags.NoBorder = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1593) | 当前常量或枚举值：`1`。 | — |
| <a id="api-2af499f1a29e6773"></a>[`Inno.Native.ImGui.ImGuiOldColumnFlags.NoForceWithinWindow = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1596) | 当前常量或枚举值：`8`。 | — |
| <a id="api-aed5e11716fb7ae8"></a>[`Inno.Native.ImGui.ImGuiOldColumnFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1592) | 当前常量或枚举值：`0`。 | — |
| <a id="api-30763187d0ba791e"></a>[`Inno.Native.ImGui.ImGuiOldColumnFlags.NoPreserveWidths = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1595) | 当前常量或枚举值：`4`。 | — |
| <a id="api-966e707832f0e70a"></a>[`Inno.Native.ImGui.ImGuiOldColumnFlags.NoResize = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1594) | 当前常量或枚举值：`2`。 | — |
| <a id="api-650bdd034e636f68"></a>[`public enum Inno.Native.ImGui.ImGuiOldColumnFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1590) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiOldColumns</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiOldColumns 53d0dac249f7b49b55fcb7b24f714fab8666c7d7bc7e88f849cc6894f91a6779 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.ID 413c81d4310e2ca2f7f9d3623c1f2b7464097a3ec41ded4b9f49fcd48e4ad393 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.Flags 18e7bc6b6c7cef921523063b05b07e1a5d85e9defcf22aa76e35c0f826318aa3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.IsFirstFrame a9e768440dd20fbc556fb5f3ea88cbfdc3ed43b9720c4b188ca6651283fb3dd4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.IsBeingResized 81b777a13ec737792b5d28e48b1637da2e1ba2b3723108f39752320699664d2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.Current b7e7d1033a208ab4c6b7050b8d3e8f5b4bcce38f068ebb7fee8d0a6d520bb133 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.Count 29b51c036c10a4c316c242e60dd1e40b597a3e82aeb6168475b4144002c4ddf0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.OffMinX 0d0b56ee016b593c1766ce248dc490af016d161d345383226fe282dbb4b87c63 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.OffMaxX 1c9a0f692d00e6bb07c5640f8a885928ca1aedad3871c2e08cd9e195e12b790a -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.LineMinY 20f9875695d1b9f6987b0a056ebae8530542f5fb3eb0099d08dd3d2232f936b8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.LineMaxY b2f3fa6504a02a2e57bda7b1cc1174b3b58d689a180e413398d6ecf34bd7bbec -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.HostCursorPosY e2ffe76649ac5507e4b144bc13cae8d6fea19c0f05a471930fc92750cd41d8c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.HostCursorMaxPosX 8790a2f9e518161b8b5fcbd9bbb1de093b09aed930612dfe67849e676ee2872f -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.HostInitialClipRect 6c792997167573473f7326ca8b778581d3eae54d1efd5f2e1e7f5c4a493d6e0d -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.HostBackupClipRect 8d557116b7a6ab678bb798b1b495c2a30c0629380b09658bd8c89175aa606eb0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.HostBackupParentWorkRect 5c8c7b2d7b208beaf44390b41f0b8762a3f8e0a1dafaffbf37d6f0b937d29ff8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.Columns d74b6600437563b40babb6720995313479f80e513ecee0370fc119b22d12d2d5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumns.Splitter a8b1ec2465cd31ffec2405913c1e9eded1a6bc2d6e38c564cb85f55b9e2140b5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumns.#ctor(System.UInt32,Inno.Native.ImGui.ImGuiOldColumnFlags,System.Byte,System.Byte,System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiOldColumnData},Inno.Native.ImGui.ImDrawListSplitter) d10a7a3c7ee0fa20c67958928836dce59f63f40bf7a9949f962faffd1b7d3bf5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumns.Destroy 33e7e96f849de5fe16826dfce6fe62cf36041133870fc611615920fd1756c90f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-cbe281f349601174"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiOldColumnData> Inno.Native.ImGui.ImGuiOldColumns.Columns`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4159) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-28bc4b4312140ac8"></a>[`public int Inno.Native.ImGui.ImGuiOldColumns.Count`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4149) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b0a0c6f94296121"></a>[`public int Inno.Native.ImGui.ImGuiOldColumns.Current`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4148) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-170104eb4d65ba58"></a>[`public Inno.Native.ImGui.ImGuiOldColumnFlags Inno.Native.ImGui.ImGuiOldColumns.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4145) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ebe47f94b5445e7a"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiOldColumns.HostBackupClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4157) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e42faf806ed2ae6a"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiOldColumns.HostBackupParentWorkRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4158) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-caeaca75902dfd21"></a>[`public float Inno.Native.ImGui.ImGuiOldColumns.HostCursorMaxPosX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4155) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-348f1bd981c2af11"></a>[`public float Inno.Native.ImGui.ImGuiOldColumns.HostCursorPosY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4154) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4eef989dd15ea579"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiOldColumns.HostInitialClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4156) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf7dc4294ec18818"></a>[`public uint Inno.Native.ImGui.ImGuiOldColumns.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4144) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6309a2f21835e8bc"></a>[`public byte Inno.Native.ImGui.ImGuiOldColumns.IsBeingResized`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4147) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-939a1a21dd9eb8af"></a>[`public byte Inno.Native.ImGui.ImGuiOldColumns.IsFirstFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4146) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-91f81c03bf697294"></a>[`public float Inno.Native.ImGui.ImGuiOldColumns.LineMaxY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4153) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-489ea63b3292070e"></a>[`public float Inno.Native.ImGui.ImGuiOldColumns.LineMinY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4152) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6bcf2a2225f6db85"></a>[`public float Inno.Native.ImGui.ImGuiOldColumns.OffMaxX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4151) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9331a80d9ba5844d"></a>[`public float Inno.Native.ImGui.ImGuiOldColumns.OffMinX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4150) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a3df4e7fa250a34"></a>[`public Inno.Native.ImGui.ImDrawListSplitter Inno.Native.ImGui.ImGuiOldColumns.Splitter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4160) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d99245ba890e7ed0"></a>[`public Inno.Native.ImGui.ImGuiOldColumns.ImGuiOldColumns(uint iD = 0, Inno.Native.ImGui.ImGuiOldColumnFlags flags = Inno.Native.ImGui.ImGuiOldColumnFlags.None, byte isFirstFrame = 0, byte isBeingResized = 0, int current = 0, int count = 0, float offMinX = 0, float offMaxX = 0, float lineMinY = 0, float lineMaxY = 0, float hostCursorPosY = 0, float hostCursorMaxPosX = 0, Inno.Native.ImGui.ImRect hostInitialClipRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect hostBackupClipRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect hostBackupParentWorkRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiOldColumnData> columns = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiOldColumnData>), Inno.Native.ImGui.ImDrawListSplitter splitter = default(Inno.Native.ImGui.ImDrawListSplitter))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4161) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1f51ec7e2d009d03"></a>[`public void Inno.Native.ImGui.ImGuiOldColumns.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66993) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-060a3391647e5780"></a>[`public struct Inno.Native.ImGui.ImGuiOldColumns`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4142) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiOldColumnsPtr</code> · 31 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiOldColumnsPtr ec8b00130d5b3a400165839afe59dcc70bcaf6d9fbf5f476c02a414118709e3b -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnsPtr.#ctor(Inno.Native.ImGui.ImGuiOldColumns*) f1deae80cb25ecc00eceae30cb19c6d1bf96cfefaaab727234a566371fd45fdc -->
<!-- api: F:Inno.Native.ImGui.ImGuiOldColumnsPtr.Handle 76901418aa61f90a65d20bdc935d6fbf250464ce8ca16d0763a8860666b488e4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.IsNull 0c73e9337bac300e81a3944c82ce87ac4c78aa4dd519f4b5a001c23bb703e6b1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.Null fd43ecfbb36c16c224fba1079f6d128446907423306b02e5ed16e09e65c48d8e -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.Item(System.Int32) cac083b0fd42fcfca7d5c7324f66f9135f7e3ff93977e8e1cae9b44bc0d7d725 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnsPtr.op_Implicit(Inno.Native.ImGui.ImGuiOldColumns*)~Inno.Native.ImGui.ImGuiOldColumnsPtr 6ff1f434200c1ac172fdd43aeeebdcdcec87cfbce8415083fdd4ca78aac4661a -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnsPtr.op_Implicit(Inno.Native.ImGui.ImGuiOldColumnsPtr)~Inno.Native.ImGui.ImGuiOldColumns* 781c482492cd940f2b69331f698f33d5bf28fa39e5cf6da5ac51aa03ed72a65e -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnsPtr.op_Equality(Inno.Native.ImGui.ImGuiOldColumnsPtr,Inno.Native.ImGui.ImGuiOldColumnsPtr) 27302dfa74cbadfcaa4c07867189c3f9c83fa2c5183e4580aeebff07d6612f9c -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnsPtr.op_Inequality(Inno.Native.ImGui.ImGuiOldColumnsPtr,Inno.Native.ImGui.ImGuiOldColumnsPtr) 058f15271e6e68b93005bad249339d7c6de7aa1cd8926ad8a46c05218dad4d66 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnsPtr.Equals(Inno.Native.ImGui.ImGuiOldColumnsPtr) fe8993c18a5d6d9f2fde6437daf57e28013b4e4bc5bf97de2a3bd98a434aa84c -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnsPtr.Equals(System.Object) e6bb639e445b3e619a772f371a9932b931720bf05ff2e272ea7264574a1c9542 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnsPtr.GetHashCode e51430516593999baa9ac2f52c76e81bad03fdbce8091ebc3fb9bd4de7985272 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.ID cfc9b891c9eb7b356a0d9ca1562d5d9d87eaee1fcbcdb12b9bcb0ba687d37cbe -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.Flags 5de7b2f4a0c5c7559f9cb3e1afd77b0c10a0fd11210462c34d2cc92f87ff8433 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.IsFirstFrame 673e839fc18e4712a238910634b0e7f3d1a1d896c3fbe8f9f2cc15ca8ee9c59d -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.IsBeingResized 0add447a88edd53eb81e64d103b2f2328c10e376380c10be6c263ba1baaf4343 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.Current e80371223b22dc2c276df08b3986418cad0d38bf49dcd6ab1d6b03be22f99fcc -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.Count 86e523421cfdbbf4864133cbfe0d7cfed84248c51d2799478aa50f0128c9c0c8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.OffMinX 03200e36eedbe0dc9e8dc181a15faee1952d373e518da184021bde221cf1a5b2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.OffMaxX 114ce16f1b95569a3c42b816bee66c01b9bc2192fcaf82422e6c3efc44fc9fef -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.LineMinY 137d8dfdbfd2447896b522bcf814485b75bd871170e769dfb2bce0876dd97469 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.LineMaxY 682b4d5671f88f6f8d7604cbf10b6c54e39cc74ee966fb92584e28d7e86f58f1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.HostCursorPosY 5e17ae8223544d84df12fa49bea0d2094abef89eb6e736693dc1f7334f605103 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.HostCursorMaxPosX 8e04b00ab77841d3a2479905086470d87abf22c2e0f4572fbf204e5feff8ce97 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.HostInitialClipRect 091ef7e0f3ee25ed2e411b81ca82cd43f40ddfdd122da22f02b4542a67141fa9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.HostBackupClipRect d42a641968152257166fc437925e822ece0f8f2c64a77363d0f87a245855bfc7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.HostBackupParentWorkRect 4112f2e1bb1782b9b1007f6a7b694b046a49d41680f39329c737a9e673c245d3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.Columns b8cfd9603ce545458f8ae8fb5ea1f693ec4460bf31d5e68639f79c46b7926716 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOldColumnsPtr.Splitter d56de9675425b40281098927e7ea638e448cb7cfeda592c38273f6a2cdefbce8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOldColumnsPtr.Destroy ca72d3d41de944dac3d7e410e5fedb4bf5a6cee01c0b44a13f805d80cbf34c23 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ccc409f79206eadf"></a>[`public Inno.Native.ImGui.ImGuiOldColumns* Inno.Native.ImGui.ImGuiOldColumnsPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15575) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27de71e045686ad9"></a>[`public Inno.Native.ImGui.ImGuiOldColumnsPtr.ImGuiOldColumnsPtr(Inno.Native.ImGui.ImGuiOldColumns* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15570) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-03d7d21e69922c04"></a>[`public void Inno.Native.ImGui.ImGuiOldColumnsPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15610) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5375306f97143db4"></a>[`public bool Inno.Native.ImGui.ImGuiOldColumnsPtr.Equals(Inno.Native.ImGui.ImGuiOldColumnsPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15585) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-bd20835faf39c73f"></a>[`public override bool Inno.Native.ImGui.ImGuiOldColumnsPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15586) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-95ec5d85a3be2305"></a>[`public override int Inno.Native.ImGui.ImGuiOldColumnsPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15587) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-9c6f2b109a8827ae"></a>[`public static bool Inno.Native.ImGui.ImGuiOldColumnsPtr.operator ==(Inno.Native.ImGui.ImGuiOldColumnsPtr left, Inno.Native.ImGui.ImGuiOldColumnsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15583) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0cf190751488ce92"></a>[`public static Inno.Native.ImGui.ImGuiOldColumnsPtr.implicit operator Inno.Native.ImGui.ImGuiOldColumnsPtr(Inno.Native.ImGui.ImGuiOldColumns* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15581) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0de6b6a5fe1a42eb"></a>[`public static Inno.Native.ImGui.ImGuiOldColumnsPtr.implicit operator Inno.Native.ImGui.ImGuiOldColumns*(Inno.Native.ImGui.ImGuiOldColumnsPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15582) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1eb08e442a5e171b"></a>[`public static bool Inno.Native.ImGui.ImGuiOldColumnsPtr.operator !=(Inno.Native.ImGui.ImGuiOldColumnsPtr left, Inno.Native.ImGui.ImGuiOldColumnsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15584) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ec40f9a9c6dc976f"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiOldColumnData> Inno.Native.ImGui.ImGuiOldColumnsPtr.Columns { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15604) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8e9ea75c9c39b478"></a>[`public ref int Inno.Native.ImGui.ImGuiOldColumnsPtr.Count { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15594) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eaa17cbb2b91a59e"></a>[`public ref int Inno.Native.ImGui.ImGuiOldColumnsPtr.Current { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15593) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-963e1a66af93ae97"></a>[`public ref Inno.Native.ImGui.ImGuiOldColumnFlags Inno.Native.ImGui.ImGuiOldColumnsPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15590) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4d357e99a16fdad5"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiOldColumnsPtr.HostBackupClipRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15602) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-18809433bee0fae0"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiOldColumnsPtr.HostBackupParentWorkRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15603) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-22be585f2dd938e6"></a>[`public ref float Inno.Native.ImGui.ImGuiOldColumnsPtr.HostCursorMaxPosX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15600) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4cd424ec0aac6246"></a>[`public ref float Inno.Native.ImGui.ImGuiOldColumnsPtr.HostCursorPosY { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15599) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ac936352bcdd9409"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiOldColumnsPtr.HostInitialClipRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15601) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a09fe532056cc85a"></a>[`public ref uint Inno.Native.ImGui.ImGuiOldColumnsPtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15589) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e5f5bbe06b7cdb8c"></a>[`public bool Inno.Native.ImGui.ImGuiOldColumnsPtr.IsBeingResized { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15592) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-39a6aace861a4669"></a>[`public bool Inno.Native.ImGui.ImGuiOldColumnsPtr.IsFirstFrame { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15591) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4d6ea5659a3fe0ca"></a>[`public bool Inno.Native.ImGui.ImGuiOldColumnsPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15576) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3fd04ad88ad8d56f"></a>[`public Inno.Native.ImGui.ImGuiOldColumns Inno.Native.ImGui.ImGuiOldColumnsPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15579) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-39f6d96dc2427867"></a>[`public ref float Inno.Native.ImGui.ImGuiOldColumnsPtr.LineMaxY { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15598) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aba7acd9bd099917"></a>[`public ref float Inno.Native.ImGui.ImGuiOldColumnsPtr.LineMinY { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15597) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f9f7522bb8468323"></a>[`public static Inno.Native.ImGui.ImGuiOldColumnsPtr Inno.Native.ImGui.ImGuiOldColumnsPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15577) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e2781d3dbee819ca"></a>[`public ref float Inno.Native.ImGui.ImGuiOldColumnsPtr.OffMaxX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15596) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-07e013bd377543df"></a>[`public ref float Inno.Native.ImGui.ImGuiOldColumnsPtr.OffMinX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15595) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d2e4e3ffc28832e1"></a>[`public ref Inno.Native.ImGui.ImDrawListSplitter Inno.Native.ImGui.ImGuiOldColumnsPtr.Splitter { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15605) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-180589ff1c219a4d"></a>[`public struct Inno.Native.ImGui.ImGuiOldColumnsPtr : System.IEquatable<Inno.Native.ImGui.ImGuiOldColumnsPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15568) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiOnceUponAFrame</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiOnceUponAFrame a922cf31e48e3a2ef32948d2e6a8d47b03cf74321c7523d4e34eaea65c661673 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOnceUponAFrame.RefFrame 483453aa19fdcd75f59733ae5b6beb090f7e813cc9c16914ec99cf62d94cc206 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOnceUponAFrame.#ctor(System.Int32) c8884e249f86961a56a4b1e23c13b4a99e15963414f862340431b7291e99a4a6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOnceUponAFrame.Destroy fccaccc12e40c2620f7cfa1e6ca7c05424cac9b6c15a22ddd86012f91939dbd7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6160ce3a78b2836c"></a>[`public int Inno.Native.ImGui.ImGuiOnceUponAFrame.RefFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9540) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-77f15bdabe14d194"></a>[`public Inno.Native.ImGui.ImGuiOnceUponAFrame.ImGuiOnceUponAFrame(int refFrame = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9541) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5fb5c467746a22c2"></a>[`public void Inno.Native.ImGui.ImGuiOnceUponAFrame.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63589) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d87f3e0721387652"></a>[`public struct Inno.Native.ImGui.ImGuiOnceUponAFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9538) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiOnceUponAFramePtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiOnceUponAFramePtr 3a2f7805147d2ad1aa2d07e53a7a7ca316f8f6c6600f6a6509ae5172d1aa8fa3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.#ctor(Inno.Native.ImGui.ImGuiOnceUponAFrame*) 1a4a1eb6e06ad6f0138d25b162607deb52927a41f8296be2b2557094a6729c15 -->
<!-- api: F:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.Handle 00daf683f69875d5eb1723b00123b56daa15e500a790a04aa8c93e054aa1ed53 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.IsNull 8e033329f039087486f63b50eb8ad4b564ce5fc43ed892472a842926e0bfa441 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.Null 358b0831d3ba24af79232289b126a76a1c02eb44ccfa39a629bb8b48dd8671e6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.Item(System.Int32) 4261842cdcdcae7fdd3ad76fbf801002e1eabf2031b49fefbb9458b57929770e -->
<!-- api: M:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.op_Implicit(Inno.Native.ImGui.ImGuiOnceUponAFrame*)~Inno.Native.ImGui.ImGuiOnceUponAFramePtr ef7f516cf012a32ffdfadbca94754369573d3d766a60cff7e7c45e42819bc3fd -->
<!-- api: M:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.op_Implicit(Inno.Native.ImGui.ImGuiOnceUponAFramePtr)~Inno.Native.ImGui.ImGuiOnceUponAFrame* 1e6d3f5d321353cc9599a30e5ceb44df5fcdc5b3211f273bfcbf434071697695 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.op_Equality(Inno.Native.ImGui.ImGuiOnceUponAFramePtr,Inno.Native.ImGui.ImGuiOnceUponAFramePtr) 2fbf9247329abc8d508b74b82c7b38d16317f0fd755cb69eeeb407075b92a6f0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.op_Inequality(Inno.Native.ImGui.ImGuiOnceUponAFramePtr,Inno.Native.ImGui.ImGuiOnceUponAFramePtr) fb0c1f0a8694dc52061fc5bafe83ef470e1ed0012d229307e729df4ece872341 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.Equals(Inno.Native.ImGui.ImGuiOnceUponAFramePtr) 0be1a75ba02ab324a7da674c9dc7930afcc14a9097fc8a17d5d2d8bb20c6aaad -->
<!-- api: M:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.Equals(System.Object) 10e3815931fad7df8d3eaa75f5f16b9ea6e4cc33c81eb8071257b09c6fafb63a -->
<!-- api: M:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.GetHashCode 75827785294bd74947955a48dc6fc378994caad75e4eaba700c573a80b430b97 -->
<!-- api: P:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.RefFrame 2e3833da076c20730acca0eb2d1f478461f90c72c85e8de1aadfb5c134509dc2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiOnceUponAFramePtr.Destroy 98a5bfb75f2bac9063b37a935b96b0a447e05077b8c05dc6a790e0e4a5bf1352 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5fc58c13af62412f"></a>[`public Inno.Native.ImGui.ImGuiOnceUponAFrame* Inno.Native.ImGui.ImGuiOnceUponAFramePtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15624) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c2bc26c1a132023"></a>[`public Inno.Native.ImGui.ImGuiOnceUponAFramePtr.ImGuiOnceUponAFramePtr(Inno.Native.ImGui.ImGuiOnceUponAFrame* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15619) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9ef5b7e715b747fe"></a>[`public void Inno.Native.ImGui.ImGuiOnceUponAFramePtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15643) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-00e3b565b2cf12da"></a>[`public bool Inno.Native.ImGui.ImGuiOnceUponAFramePtr.Equals(Inno.Native.ImGui.ImGuiOnceUponAFramePtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15634) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-32302393fa750a7f"></a>[`public override bool Inno.Native.ImGui.ImGuiOnceUponAFramePtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15635) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-092e3660a5fec98c"></a>[`public override int Inno.Native.ImGui.ImGuiOnceUponAFramePtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15636) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-f02bd1312810b0e9"></a>[`public static bool Inno.Native.ImGui.ImGuiOnceUponAFramePtr.operator ==(Inno.Native.ImGui.ImGuiOnceUponAFramePtr left, Inno.Native.ImGui.ImGuiOnceUponAFramePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15632) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cc5b984de02cadef"></a>[`public static Inno.Native.ImGui.ImGuiOnceUponAFramePtr.implicit operator Inno.Native.ImGui.ImGuiOnceUponAFramePtr(Inno.Native.ImGui.ImGuiOnceUponAFrame* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15630) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b58a38cbea0f4b65"></a>[`public static Inno.Native.ImGui.ImGuiOnceUponAFramePtr.implicit operator Inno.Native.ImGui.ImGuiOnceUponAFrame*(Inno.Native.ImGui.ImGuiOnceUponAFramePtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15631) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-47eaf117aab3e1be"></a>[`public static bool Inno.Native.ImGui.ImGuiOnceUponAFramePtr.operator !=(Inno.Native.ImGui.ImGuiOnceUponAFramePtr left, Inno.Native.ImGui.ImGuiOnceUponAFramePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15633) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9ff88330c7920a82"></a>[`public bool Inno.Native.ImGui.ImGuiOnceUponAFramePtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15625) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-65605408697e7d29"></a>[`public Inno.Native.ImGui.ImGuiOnceUponAFrame Inno.Native.ImGui.ImGuiOnceUponAFramePtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15628) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7d988bbb105d9ab3"></a>[`public static Inno.Native.ImGui.ImGuiOnceUponAFramePtr Inno.Native.ImGui.ImGuiOnceUponAFramePtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15626) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-210c21e7b9d182cd"></a>[`public ref int Inno.Native.ImGui.ImGuiOnceUponAFramePtr.RefFrame { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15638) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9c2e9e7df946b61a"></a>[`public struct Inno.Native.ImGui.ImGuiOnceUponAFramePtr : System.IEquatable<Inno.Native.ImGui.ImGuiOnceUponAFramePtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15617) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
