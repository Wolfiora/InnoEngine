# Inno.Native.ImGui ABI · ImFontAtlasBuilderPtr → ImFontStackData

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImFontAtlasBuilderPtr</code> · 34 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontAtlasBuilderPtr dd24396dd4051557691c2d3aad0b15c4adc37d198ae56d09c02e55f22c12e3df -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasBuilderPtr.#ctor(Inno.Native.ImGui.ImFontAtlasBuilder*) 0589a62223a3b6bd160613ba4a4199b26900bcbe7f5c67842f32ab032c6eadca -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasBuilderPtr.Handle 7110324dc2a8c832fff32f9a081b6f1a33f58364caf33a23359ee7410a4736bf -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.IsNull f24817c201f6592ef76788f8847132477339383237cbe64b0ba255f2cfa7fd24 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.Null ba2cf8f4c4aaaffd28fcefd5207ce0ac0b845ec7330a5946958c85facfcf3fca -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.Item(System.Int32) e691ddb2172cc34a5ce423570331eae1fb09ce46eb1d3e100986f4253ff5f492 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasBuilderPtr.op_Implicit(Inno.Native.ImGui.ImFontAtlasBuilder*)~Inno.Native.ImGui.ImFontAtlasBuilderPtr ee578dcce4b91e59c113a4e0765d6330fcd1d809f1ca868344a395a1f45bad67 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasBuilderPtr.op_Implicit(Inno.Native.ImGui.ImFontAtlasBuilderPtr)~Inno.Native.ImGui.ImFontAtlasBuilder* 2b81fced22fdc68fbdce611d0d26044bf1b120df1a5c1878054747f1e4b95e1a -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasBuilderPtr.op_Equality(Inno.Native.ImGui.ImFontAtlasBuilderPtr,Inno.Native.ImGui.ImFontAtlasBuilderPtr) 4b34461532d75cb3bd09d276df5b3b51151efc9833423387154ba2eca7821753 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasBuilderPtr.op_Inequality(Inno.Native.ImGui.ImFontAtlasBuilderPtr,Inno.Native.ImGui.ImFontAtlasBuilderPtr) e51626ca695cfad17f0db63beb9c8017e878ff6e6c3a0ededd7a5f7124a0bf76 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasBuilderPtr.Equals(Inno.Native.ImGui.ImFontAtlasBuilderPtr) 02cef6f1bcd57bb53672bec0102ad143b503be42f27e80b959244c011e065995 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasBuilderPtr.Equals(System.Object) 5233b9c7f35cdf3e078cd12d0a8f73725a63d0ee4c260c061990378ba5bcca6d -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasBuilderPtr.GetHashCode ede0244bfe503322a7560252c5c957e5a1677ea39293ddac8a27b038ee84e3a2 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.PackContext 238115a30e3e1cc7f97a97648e21947abf3258627cb1b4c650c96bda6f3add82 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.PackNodes 2804b5ca1337f840cbc43bd15cec2a420cfdae873878e41a389937da881823b3 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.Rects e7d84083378634ee314905fd93537947f2cb98ef0542a8e83121143fd3733d4d -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.RectsIndex f5691281f7830b1f7ead7a38c69dbc1d3f265648f68a54a5717b8f63414c14ea -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.TempBuffer 4e6972c5968116eb4cf226dc844e78d10710daaed1b1c4a11aabe9a8ed00c06b -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.RectsIndexFreeListStart dd95c48e4242c650b6f46cc654dacf9ce32ee54ac103822e31f09ce6e1ff5275 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.RectsPackedCount 3b92f0136f9b6b4adbe5993a65b6f2444cf603ce6681e821db4944449de14300 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.RectsPackedSurface 109bc4ae57efcc4fc0fff7f92f59ff63b0a0ebdee80dadb376cded0b84935df6 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.RectsDiscardedCount 950f0bc9b2587688f0c3c33f671bbeaadec7fd3b94ae8cc582d45e94a4c7fe94 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.RectsDiscardedSurface 72e39c3033966648c503d7c5158c8419fa4db5e5e3ad03bcf4201b15fe7f7d3e -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.FrameCount e98e91efdb4c98770867e3dbd8156e16af72ad67f0e452b3cdd8575a4a5d802f -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.MaxRectSize 84acfa2dd6be216f0479dee9fbf8ab840ba0ac8e9f703fa261654c079ccca0e0 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.MaxRectBounds c41c7bcdef81b72e90e1c77f7cab3f1690dd9c7b34828182e5963c88b400826c -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.LockDisableResize 114c6d32c860aa76d83bbf664db88ecb07bf99be73f3f9c1a7fa063ba09cbeb0 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.PreloadedAllGlyphsRanges 4bfa6c381708511a2d56a57281b3bed23fa7b45c24c66c7057480b8fb5103de7 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.BakedPool eaee3bf98b4009f24f1b380e5dc8d2905d1bf312a4748c9b0c15075b80699fa0 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.BakedMap b08e5d33cf6673383c07baab067fde887e45dc2b18f483d6e53a6a69b7a5dc02 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.BakedDiscardedCount d31b5f536e1d6df36a050deccec601a8945b4cf024c4db35e04e6a3a2495d641 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.PackIdMouseCursors ca14501aece7a38d4dcb76fb64d798f217b273a4c983ba77c7b6872409ad4159 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasBuilderPtr.PackIdLinesTexData 374984d06bb8be719dafa85fb69404d4856e755dfac9c135c496b096db59094a -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasBuilderPtr.Destroy 60b7fb3df1b24119c47648b460025f4a1efcba73da93b63306900357c5064619 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-36349bfafefd3262"></a>[`public Inno.Native.ImGui.ImFontAtlasBuilder* Inno.Native.ImGui.ImFontAtlasBuilderPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12222) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-48c6fa75af4f31a5"></a>[`public Inno.Native.ImGui.ImFontAtlasBuilderPtr.ImFontAtlasBuilderPtr(Inno.Native.ImGui.ImFontAtlasBuilder* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12217) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-42ec160459f929c0"></a>[`public void Inno.Native.ImGui.ImFontAtlasBuilderPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12260) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1252eb6d8f5c30c7"></a>[`public bool Inno.Native.ImGui.ImFontAtlasBuilderPtr.Equals(Inno.Native.ImGui.ImFontAtlasBuilderPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12232) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-596436e8d0e3c716"></a>[`public override bool Inno.Native.ImGui.ImFontAtlasBuilderPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12233) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-3d435dd0893cfd34"></a>[`public override int Inno.Native.ImGui.ImFontAtlasBuilderPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12234) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-e67b801da701b611"></a>[`public static bool Inno.Native.ImGui.ImFontAtlasBuilderPtr.operator ==(Inno.Native.ImGui.ImFontAtlasBuilderPtr left, Inno.Native.ImGui.ImFontAtlasBuilderPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12230) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dcb22a7f8604d03d"></a>[`public static Inno.Native.ImGui.ImFontAtlasBuilderPtr.implicit operator Inno.Native.ImGui.ImFontAtlasBuilderPtr(Inno.Native.ImGui.ImFontAtlasBuilder* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12228) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f32a6b37ed045163"></a>[`public static Inno.Native.ImGui.ImFontAtlasBuilderPtr.implicit operator Inno.Native.ImGui.ImFontAtlasBuilder*(Inno.Native.ImGui.ImFontAtlasBuilderPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12229) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4b62770ae87a4606"></a>[`public static bool Inno.Native.ImGui.ImFontAtlasBuilderPtr.operator !=(Inno.Native.ImGui.ImFontAtlasBuilderPtr left, Inno.Native.ImGui.ImFontAtlasBuilderPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12231) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8ddec5478b9441c0"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasBuilderPtr.BakedDiscardedCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12253) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-37924647bb508a50"></a>[`public ref Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImFontAtlasBuilderPtr.BakedMap { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12252) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f1ac65b5305650ba"></a>[`public ref Inno.Native.ImGui.ImStableVectorImFontBaked32 Inno.Native.ImGui.ImFontAtlasBuilderPtr.BakedPool { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12251) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-702930f5605c18cb"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasBuilderPtr.FrameCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12246) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-27bcb807334f734d"></a>[`public bool Inno.Native.ImGui.ImFontAtlasBuilderPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12223) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9f474bf685878897"></a>[`public Inno.Native.ImGui.ImFontAtlasBuilder Inno.Native.ImGui.ImFontAtlasBuilderPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12226) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a8cae870ab0e82b3"></a>[`public bool Inno.Native.ImGui.ImFontAtlasBuilderPtr.LockDisableResize { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12249) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5bbce53efb6d9f49"></a>[`public ref Inno.Native.ImGui.ImVec2IC Inno.Native.ImGui.ImFontAtlasBuilderPtr.MaxRectBounds { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12248) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2d1143da04f60077"></a>[`public ref Inno.Native.ImGui.ImVec2IC Inno.Native.ImGui.ImFontAtlasBuilderPtr.MaxRectSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12247) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dab730a4d5d50a75"></a>[`public static Inno.Native.ImGui.ImFontAtlasBuilderPtr Inno.Native.ImGui.ImFontAtlasBuilderPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12224) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-65958f6df129883b"></a>[`public ref Inno.Native.ImGui.StbrpContextOpaque Inno.Native.ImGui.ImFontAtlasBuilderPtr.PackContext { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12236) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0980fc37a8717c8a"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasBuilderPtr.PackIdLinesTexData { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12255) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c0ac79209fd10fb2"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasBuilderPtr.PackIdMouseCursors { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12254) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8de8d01ae9eaa106"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.StbrpNode> Inno.Native.ImGui.ImFontAtlasBuilderPtr.PackNodes { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12237) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e3b2c56c3d496c3f"></a>[`public bool Inno.Native.ImGui.ImFontAtlasBuilderPtr.PreloadedAllGlyphsRanges { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12250) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5c3875c542aafcf2"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureRect> Inno.Native.ImGui.ImFontAtlasBuilderPtr.Rects { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12238) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e8229bc7fa03ccab"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasBuilderPtr.RectsDiscardedCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12244) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-25d4dca7bec08470"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasBuilderPtr.RectsDiscardedSurface { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12245) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c8c18480f92bd0dd"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontAtlasRectEntry> Inno.Native.ImGui.ImFontAtlasBuilderPtr.RectsIndex { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12239) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6d36f1d6c9c46dc4"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasBuilderPtr.RectsIndexFreeListStart { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12241) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-725e1ee9005de81c"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasBuilderPtr.RectsPackedCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12242) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ca1067ecc20f6de0"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasBuilderPtr.RectsPackedSurface { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12243) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3cc00a347b4a73aa"></a>[`public ref Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImFontAtlasBuilderPtr.TempBuffer { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12240) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bb8a374a80daa1e8"></a>[`public struct Inno.Native.ImGui.ImFontAtlasBuilderPtr : System.IEquatable<Inno.Native.ImGui.ImFontAtlasBuilderPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12215) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontAtlasFlags</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontAtlasFlags c941670ad899a97532f8b06317cb4ee123552adab05a88157418c1a15357aa09 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasFlags.None dedfa9fd8f313d4fed97e2fe306cb1a25409b7558daf81f86f258889ffff23a6 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasFlags.NoPowerOfTwoHeight 25ae178c01c9a72b19ac606b9155b04b82afd7e9991d741291560a32cc0651aa -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasFlags.NoMouseCursors 96e35b0a2d640a6aee0eca8c0393545c3a96711913a0a410d3edd99dd8297e84 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasFlags.NoBakedLines 197db3f8b9ada349175ae73eb8783dda4c43044a7631ca6f03b264775bc86348 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-660e24a9a7b2356f"></a>[`Inno.Native.ImGui.ImFontAtlasFlags.NoBakedLines = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1152) | 当前常量或枚举值：`4`。 | — |
| <a id="api-a58f023553ef7d1c"></a>[`Inno.Native.ImGui.ImFontAtlasFlags.NoMouseCursors = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1151) | 当前常量或枚举值：`2`。 | — |
| <a id="api-34206012bf420d9e"></a>[`Inno.Native.ImGui.ImFontAtlasFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1149) | 当前常量或枚举值：`0`。 | — |
| <a id="api-ca7436ae0a1085cc"></a>[`Inno.Native.ImGui.ImFontAtlasFlags.NoPowerOfTwoHeight = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1150) | 当前常量或枚举值：`1`。 | — |
| <a id="api-a137e7fd9c0af86b"></a>[`public enum Inno.Native.ImGui.ImFontAtlasFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1147) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontAtlasPostProcessData</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontAtlasPostProcessData 9f5898fd0edaa2aa041dd5b3dd155f4dcdfdc2e11c7d6f033da3563d5cf20352 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.FontAtlas ad9c8db07c92c840a4dbc1a3b0f6a2849ea8964bb9544b9d9b6702fa4872c2f1 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Font 728b9b37d2974dda005600828b81a6bff3b83648561a75c3bfbee93aeb9c447b -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.FontSrc fe5c44181c057294c4d0ea1c4bdbc6b4c6f9f1ff74a708d173328c9c82578779 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.FontBaked 83b78ac7a020256faf2a8991032ca364fc6ac18699f29b18453164e71de465f9 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Glyph 038e439d55efa2ec333466fbb1a2797298a6102c502c715c8df9d227e6c0a0a9 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Pixels 638c5c9179278faf2f7ab2548a07c4a13f273a0ef39084fd2dbea86b66297c43 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Format 27c1db2d618b6992dced2f49de9077518de2726ad1b551b7984d4627bdc25978 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Pitch 7ed87da8bca8803823b127d00358116aeebebaceae5c9300f6efff0408161789 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Width b53d3ec3be57cd8fb74dd87c709ae19f397967ccaba98d0c76d057bd1ae947c1 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessData.Height 1445ead71e4576203cc59627ac73f6f0b0e0d3399a377a14648f7702b38f1484 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPostProcessData.#ctor(Inno.Native.ImGui.ImFontAtlas*,Inno.Native.ImGui.ImFont*,Inno.Native.ImGui.ImFontConfig*,Inno.Native.ImGui.ImFontBaked*,Inno.Native.ImGui.ImFontGlyph*,System.Void*,Inno.Native.ImGui.ImTextureFormat,System.Int32,System.Int32,System.Int32) 9d0626bccb337107ca4d3764035ccfc7824c4469b89f4fa03a56afceb9f2a33f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-37d2812778aa7b14"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontAtlasPostProcessData.Font`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10153) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3118f15b10196051"></a>[`public Inno.Native.ImGui.ImFontAtlas* Inno.Native.ImGui.ImFontAtlasPostProcessData.FontAtlas`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10152) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5fe019e031713259"></a>[`public Inno.Native.ImGui.ImFontBaked* Inno.Native.ImGui.ImFontAtlasPostProcessData.FontBaked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10155) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f55a3b944cd6aec1"></a>[`public Inno.Native.ImGui.ImFontConfig* Inno.Native.ImGui.ImFontAtlasPostProcessData.FontSrc`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10154) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-abe4c82f8f0dc97b"></a>[`public Inno.Native.ImGui.ImTextureFormat Inno.Native.ImGui.ImFontAtlasPostProcessData.Format`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10158) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ef7754b3608f3925"></a>[`public Inno.Native.ImGui.ImFontGlyph* Inno.Native.ImGui.ImFontAtlasPostProcessData.Glyph`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10156) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-48146103cae9f161"></a>[`public int Inno.Native.ImGui.ImFontAtlasPostProcessData.Height`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10161) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e63da786d675ef6"></a>[`public int Inno.Native.ImGui.ImFontAtlasPostProcessData.Pitch`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10159) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7c27accfebfb5da2"></a>[`public void* Inno.Native.ImGui.ImFontAtlasPostProcessData.Pixels`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10157) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2b35e74a5b3095c6"></a>[`public int Inno.Native.ImGui.ImFontAtlasPostProcessData.Width`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10160) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5aec1f98ba93665e"></a>[`public Inno.Native.ImGui.ImFontAtlasPostProcessData.ImFontAtlasPostProcessData(Inno.Native.ImGui.ImFontAtlas* fontAtlas = null, Inno.Native.ImGui.ImFont* font = null, Inno.Native.ImGui.ImFontConfig* fontSrc = null, Inno.Native.ImGui.ImFontBaked* fontBaked = null, Inno.Native.ImGui.ImFontGlyph* glyph = null, void* pixels = null, Inno.Native.ImGui.ImTextureFormat format = Inno.Native.ImGui.ImTextureFormat.Rgba32, int pitch = 0, int width = 0, int height = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10162) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6da478e730929c91"></a>[`public struct Inno.Native.ImGui.ImFontAtlasPostProcessData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10150) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr</code> · 23 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr f22727214b5aefb2a95da3fa9e396c693de09b1fcd1e9f7b01b97b1fd916de10 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.#ctor(Inno.Native.ImGui.ImFontAtlasPostProcessData*) 62c71d8b6861ed8b1b91e4f0ec8dad6671d62b72783c57ab7eb1ea330ab9d312 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Handle da117178177f4c926aac642f8833d063298a8a14330ded50d55a07af7d76d8d5 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.IsNull a7a23ffe8929c8ccb29ff7a85c7c6e90cf8e3d9370dc4616bd7dc457265daac6 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Null 129d0df3d4c65b32d7c40fb0077960e0b9bbe43cf46808e8e378489161f44b2c -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Item(System.Int32) 9ab058ded3b80a509f71d3067d56780acf633113f0b49c59758371ebd5ec49e4 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.op_Implicit(Inno.Native.ImGui.ImFontAtlasPostProcessData*)~Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr 30b1ffcc9142c5afea75486365d2f307805cd31ce2332a07a50eaef3c34d96ec -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.op_Implicit(Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr)~Inno.Native.ImGui.ImFontAtlasPostProcessData* 3fef052cf686d8b58ebf76d2753b9b0a62135c2df67773e39a56f7dee05f23d6 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.op_Equality(Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr,Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr) 347e1bd189053c99271336c37eb8701788471ac00032f815770bb80876fa0b8a -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.op_Inequality(Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr,Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr) 5817681d3607dfb8d1f8400900efb131cab8ceefdb2e7bf33cadb9096648cf31 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Equals(Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr) dfb595e9cc774d51c02d51c1721faaf5083fd420b68f43f2ef77aa438afdc6e8 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Equals(System.Object) ceca4a7e106463c16dd137b2ee1d2592bed66fe3c0862539319365a42499df19 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.GetHashCode 703d77aa9f50b292ebb5c1d710ff8527f9a6258560f6a9db81f28dc585cfe4ff -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.FontAtlas 1030ed5c0873bd100f709e7b4ec185b1bec760578df8e382d693d8e558ccd1cf -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Font 46807ae4b4403db665b39b655428648dbc5e66d21115f17b83319939134a4c7c -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.FontSrc c09a7ae2d85145ef40a286a1bd7eb2f08cd83549f25066b6dceba336b3a8eacc -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.FontBaked 7fdbd6379eb4d51355ccdc318716d1cf4b2109bc7cd49edb6a42e615ba7cdede -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Glyph fd49bf7a3aec312f6e86f529d35123c20074934ec25653d8962610ed73c2fef9 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Pixels 566dbd0a41f1b10e64d05c03cbf7079a4bd4415c655dc512fa9f6c143222fdb0 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Format a5aa079741157aa4eeffc8f73390016965e66aa2b968240784e38c436c9d70cd -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Pitch e30c763bb5b7bb14533da1495eb1e4be9e45a98823e88e80d0c71adb002b2f71 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Width 1c6434a2613a9c5749095f729acbd5e7f1837304bcee76382f14b270f3306bbb -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Height 138102f00f9c683976e53d9bd04e2387f5b45808d8dd9b55590bb2d2fe97c2a9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8c1b8470e3dc9527"></a>[`public Inno.Native.ImGui.ImFontAtlasPostProcessData* Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12274) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0adbacc76bc90bd7"></a>[`public Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.ImFontAtlasPostProcessDataPtr(Inno.Native.ImGui.ImFontAtlasPostProcessData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12269) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b2c6e3b2489432ab"></a>[`public bool Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Equals(Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12284) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-f5432827aff9a2d1"></a>[`public override bool Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12285) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-68582253c870dc81"></a>[`public override int Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12286) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-df74effa29e362c5"></a>[`public static bool Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.operator ==(Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr left, Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12282) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2e5bc02589628d96"></a>[`public static Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.implicit operator Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr(Inno.Native.ImGui.ImFontAtlasPostProcessData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12280) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9131bc9b78b0a8f5"></a>[`public static Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.implicit operator Inno.Native.ImGui.ImFontAtlasPostProcessData*(Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12281) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0b791080cc9eeaf3"></a>[`public static bool Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.operator !=(Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr left, Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12283) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-80cbfcff8393e814"></a>[`public ref Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Font { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12289) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-723f47f20989633d"></a>[`public ref Inno.Native.ImGui.ImFontAtlasPtr Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.FontAtlas { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12288) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2cb4a52a2153210a"></a>[`public ref Inno.Native.ImGui.ImFontBakedPtr Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.FontBaked { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12291) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a1092b781aa020bc"></a>[`public ref Inno.Native.ImGui.ImFontConfigPtr Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.FontSrc { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12290) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-63294d83b7b3974a"></a>[`public ref Inno.Native.ImGui.ImTextureFormat Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Format { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12294) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-12fccfa025f48572"></a>[`public ref Inno.Native.ImGui.ImFontGlyphPtr Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Glyph { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12292) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3a251183bc2c927b"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Height { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12297) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-886debbea32308d0"></a>[`public bool Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12275) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-30eda1e2f12706a8"></a>[`public Inno.Native.ImGui.ImFontAtlasPostProcessData Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12278) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-024f46e816f6bbca"></a>[`public static Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12276) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-632fa354690fcd9c"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Pitch { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12295) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c67b33343ca95553"></a>[`public void* Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Pixels { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12293) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e975b1cd2e140d41"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr.Width { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12296) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c87de4c204de3d61"></a>[`public struct Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr : System.IEquatable<Inno.Native.ImGui.ImFontAtlasPostProcessDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12267) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontAtlasPtr</code> · 87 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontAtlasPtr 48d4420b842fb59c8e23b0376afe4d379a877d9dc896e5ba7973569f1d094478 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.#ctor(Inno.Native.ImGui.ImFontAtlas*) 52490a72142a665d0fb7a3552f01c47cafda11fb848c6a0290d9077eb08a3a95 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasPtr.Handle bf968b6934f86d27d4f97a44923436f4641499e06bf4cbe2a6402502b799adf3 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.IsNull 7ad2876610205611efbede8837e977f36af918b9996b031a7accf32e2d1fa76c -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.Null 32a83ffaefab61572b197a2b8354ca69c97241183afde371ff597202ef3f7df1 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.Item(System.Int32) d33b330e5af6a5d5e9cb3b243079da385fc75cdff01b8ab1f3b5352f2a43f527 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.op_Implicit(Inno.Native.ImGui.ImFontAtlas*)~Inno.Native.ImGui.ImFontAtlasPtr 92e8be3de58ca8cc4eeb5b93c21582da645dff042a33034eada6944679082e74 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.op_Implicit(Inno.Native.ImGui.ImFontAtlasPtr)~Inno.Native.ImGui.ImFontAtlas* 9c04265fdaf01e87c011f0a80d14147b85a8530f75a2555ee96b24fdfbcc93a4 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.op_Equality(Inno.Native.ImGui.ImFontAtlasPtr,Inno.Native.ImGui.ImFontAtlasPtr) 6ab9727300083aea2fb225bf8e10403bb9fef224ad286c28989cbda74ea953b6 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.op_Inequality(Inno.Native.ImGui.ImFontAtlasPtr,Inno.Native.ImGui.ImFontAtlasPtr) 75399c0662ca64599386d04d90a1c96485dac319809fe91d76c2cbc00157eedb -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.Equals(Inno.Native.ImGui.ImFontAtlasPtr) 91dfe1e8274c9f6b283890a5542f186e62819a9c6f01f6d32016b02736a9171a -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.Equals(System.Object) 8f8a971062628501b630df4ef8e790ca472ad9432a190d34e5d84f485a05d21d -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.GetHashCode ea472f43d81f30eceddeb048d096fcb6a78cacc853bdf24b3f7eafe43705fe44 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.Flags 871d922f9297d0940abfa6af99bfed9257efd6cd0964d3f1bb32f00622adec42 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexDesiredFormat 872454b693cb6e7842b71a58cdaa455da1d9bbb47ec98af1c9e0653ace3b9263 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexGlyphPadding adb70d13d2825d6f7a1eb17fd8865c677066a7b9de3874c7a120eb2af2804152 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexMinWidth 3942e664ea65b380f8ecf41556ab514fee367caa053a7b031c8278fe5e872f34 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexMinHeight 5990af7122edd5005ed6fae06b18dc2814988e12ca12cfe1d0d255a74e411ad2 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexMaxWidth 1d4a533910628b5265b426b8bca783e265c5673f4dfec7380176eea9c6f3904e -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexMaxHeight 9c851417672360f5f9750e13884a5202b85178efbd7e90bbe397e850f9d747ec -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.UserData 3cd9304eafdb193e15e08963da08192d7885fcd159e909afb234a9b5fd94f0f5 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexRef 17c47e3f8293a4bdc91dce90d7ef84a4f106ac27e045309efe8e897af0969432 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexData a93aeff0e21e33e3b9d2cfd644a022b43a9d81193865401c0d2138554c9a65ca -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexList aac11b0ada9e4792cbbafb3193881e8ad7bc93fac45ab022cf1258dc9dcfdebc -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.Locked cab7f372fb3cd9705c5dcba4b68f135ca2def2ea860f7c218629e00caf75d872 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.RendererHasTextures 3c96115e564b62fff708e246b5300cf4d1c70896d18aa35c3ea330ba6ddd57f9 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexIsBuilt 5d5ed63a1e5714903c67301d250db5b1de355be1e897dc37597ac999e7ade3c4 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexPixelsUseColors 4e28f15f8f909173c1867c9980cf2be0b3778e853cc42dc475dfd56a05d9246b -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexUvScale 91fada0d87edaf1d8eaae39d7ef8e040724bccab9b121c599dc98f4f2e72ea4b -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexUvWhitePixel 3138b521e7965cafbd81dc7919f51e0ffb63a255a2fe69f7d274c54080e15361 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.Fonts 75e0428a7e93c2aef1c84532efa22f15a469081f0e5c42a6ed0cb74db1392c1a -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.Sources a19ae51a2df1f03dce23d8c8e0dcb28f56cad8db30fe92ebb4980f619785b3a7 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexUvLines ed2676644edaeefa5da76596a13f339cc30cf773e5815829bc43e3fc5faa7ff2 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.TexNextUniqueID 1852a9afdcc40c14caecd63c72ac93ded65fbee1e29107dbfe3ea44441bbecf5 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.FontNextUniqueID cad7c4d697a55db1fa87e635536a34834709195039104a874e2b64f6c9aefdc3 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.DrawListSharedDatas d63e8b04fed84a422a0d15637cce45473b48a77cebefb1104e15b9d6bc12b4c3 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.Builder cdd0d376840dabaed7277d984274d824818341f609c154d2b32444f950d89382 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.FontLoader ffd2713839999c44a7761e9d081f1960ac56620635c213b03493ea939669022e -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.FontLoaderName 4a8f8739a33cfa2734b687eecb53ce0c59373f3fd609e0fe78a3417f4946876f -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.FontLoaderData 960722b570fd1117f64011f77c2f2fd18da5977ad706e2aa3d369d620468e6ef -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.FontLoaderFlags 92581644bb7847c59f844b1a31be6e882c0a078ff3616946948f76c3404774f0 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.RefCount 351b9d6828e62611eaf11551fb0b4a9e2621dd39a6ccccdd045b5a4d2db37f15 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasPtr.OwnerContext 641470c4850ddfc9eff9444f23a539c1e4228c82e8840219005a949dfa1c2910 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.Destroy 540dea963451fc80439f07d85782b66313782a97c72fc7ee00fdada3da22cf47 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFont(Inno.Native.ImGui.ImFontConfigPtr) 359d1640ad8f4b8a0222f360e553ce7505de2ba06b4fe5db64b93ce107055b65 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontDefault(Inno.Native.ImGui.ImFontConfigPtr) 3ff14b69a2460ca2a79b7adee1346791b83e3d6e104c9c7ece92be7d7aa5ccf8 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontDefault a327519e79d9a8b7f3cf12d50053094af1b15e0b2e6752fa493aee29434c0cd0 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontDefaultVector(Inno.Native.ImGui.ImFontConfigPtr) a852982eb8b2dd478af3c4ea8f23b15a0faafe6c500409090921254e78204e73 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontDefaultVector 6e930db002a7b22f08b74b54477ebc0462cb8dbe2ea1eab2cc91cb4d0a4a6b0b -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontDefaultBitmap(Inno.Native.ImGui.ImFontConfigPtr) d9d20da71a00ba1a66d22db2fe76a0f9a185360c6e4c24b145414abf6b849263 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontDefaultBitmap 9fcfbaa02bf2a0e73cf4be3a3a5d9e6fa1cf93733fdafbabe0086efce31432c7 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(System.String,System.Single,Inno.Native.ImGui.ImFontConfigPtr,System.UInt16*) ddf6c61368ab674b0c0034d2b2623817da86aff7079c040fc5a8cbc5d4c9a462 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(System.String) 8977bb9253b33e42e5fb5371a8c7fed9a2663e86c547abbb2f50286dae9c46dc -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(System.String,System.Single) b80b9fea213771e414b0b1126f91b2552f33b52f58b84db40ed3eddb088c334b -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(System.String,System.Single,Inno.Native.ImGui.ImFontConfigPtr) 85bc691e89eaab17ae7e4ac752a18b7561ee62b083fd3be78816c559002f6878 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(System.String,Inno.Native.ImGui.ImFontConfigPtr) cb602f955e7a2b0c4125aab4df502d7330efc449f1af37ac9d8fda68b0597ac4 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(System.String,System.UInt16*) 49e771f0443eea76735817e12bb62b64f6dcd7d859e198192f9119540a2e74d4 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(System.String,System.Single,System.UInt16*) 2cc29e9528609442f78025d0955ff5082ad0b64d9202903a366214d737b99fa2 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(System.String,Inno.Native.ImGui.ImFontConfigPtr,System.UInt16*) 81a29d849f701e4f93ec7f17294f6a97b8c049b0fe1ee3df6e521da14bf52872 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryTTF(System.Void*,System.Single,System.ReadOnlySpan{Inno.Native.ImGui.ImFontConfig},System.UInt16*) 98328173d1d516172cf877bb49894ad342e2f4e1c0618258a6fa5e41c91800bd -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryTTF(System.Void*,System.ReadOnlySpan{Inno.Native.ImGui.ImFontConfig}) 60dc5edf5344fb16a4d3a592eb50421bf70d2982dcbb77ed92581d4817fc445a -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryTTF(System.Void*,System.Single,System.ReadOnlySpan{Inno.Native.ImGui.ImFontConfig}) ac084bf77d33fc55a400be0edbc511df15985eb9c8811345dfeae8dabc694827 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryTTF(System.Void*,System.ReadOnlySpan{Inno.Native.ImGui.ImFontConfig},System.UInt16*) 329030b4730cb64f504f1dbcb1e316c380494f63ba7bfc629dd34a45f2e154d1 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedTTF(System.Void*,System.Single,System.ReadOnlySpan{Inno.Native.ImGui.ImFontConfig},System.UInt16*) b705c0dc9f4d0f3e280aaf694ad2ca5ee185fb77d72f653bb5cb81f3a581f4bb -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedTTF(System.Void*,System.ReadOnlySpan{Inno.Native.ImGui.ImFontConfig}) b0f9586e71738631fb2f289cde1c2bd16234eaa7f0823275a20df03de73a50d5 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedTTF(System.Void*,System.Single,System.ReadOnlySpan{Inno.Native.ImGui.ImFontConfig}) 2d2f80a01ae75b41011a0fa19d89cf02e2128553061c619f3ab56a8f11d61149 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedTTF(System.Void*,System.ReadOnlySpan{Inno.Native.ImGui.ImFontConfig},System.UInt16*) 67eb5f0e89b024b1704e4bf8bc87f215d766c4347e9d68dabc9eb736c2f07848 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(System.String,System.Single,Inno.Native.ImGui.ImFontConfigPtr,System.UInt16*) cd9046eba675c5be20d4ad462055898490423491d0f5c3db6fa5de0c1c27b6a2 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(System.String) dfdf3b3bf837fb7231a3060aabb3ff82311a25b178d48b4a96071200d04ee42b -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(System.String,System.Single) a9be0007dea0540384dc98d28bba627efc80ef75a1448de4451cee7c1fe20e10 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(System.String,System.Single,Inno.Native.ImGui.ImFontConfigPtr) 364ee17b34c5bfbaf0d727ef8cd844d2e87c712df3ac40385dddb8920022aca2 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(System.String,Inno.Native.ImGui.ImFontConfigPtr) 6caa0f319d89aee4fa39c34b1ac0f12261ce010e61c52b073c16c9f08c30a17f -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(System.String,System.UInt16*) d2325e4ec791c16e66ae11725b9202fe88eee52c6d90dd4bc1412251b64c6f6f -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(System.String,System.Single,System.UInt16*) af53ee49fd34df3e65a3841c17420df7b7c9f76001e10b82e6d78b8f536ef732 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(System.String,Inno.Native.ImGui.ImFontConfigPtr,System.UInt16*) 1dea6443df9855333ad48fe8723e2ff9159c39ca59bfa148e5d5f774c4af032a -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.RemoveFont(Inno.Native.ImGui.ImFontPtr) 81db250b7eeb8ecb78b9efb70243760c594c51a4577dc944bf92ca094ca38dd4 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.Clear fbdbfde870ac83b3cd836dd45bd6b29e91e59ae06fef305d615c9270489a4c26 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.CompactCache fbb965f8bf0b1c04c4423d83bdbdbd921d916644fc2f65eaed5ea645a99998fc -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.SetFontLoader(Inno.Native.ImGui.ImFontLoaderPtr) 0008360f34d6d95806a5da62aa378044e306a10b00318d15ed9525c21d73ae07 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.ClearInputData 17fde659708794f7400a5bff8f2d8b09107f7da8210e7c1c385bbdcef4f20d16 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.ClearFonts f6bb82271af68b6a202da49af1a434e4d0cf199b17d9f5bc6287348694dda124 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.ClearTexData c03d21f6023cadfed59b243f45592b2c03080a58fcd0e66c7be7228d04fe6d14 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.GetGlyphRangesDefault 8c677e86f68778cdfb10859ef9c4009280833b1916da384436c1d2ce3dfe50ad -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddCustomRect(System.Int32,System.Int32,Inno.Native.ImGui.ImFontAtlasRectPtr) 35f107b6dc98af61eb6873582fcf8fe6c975467e601235ce8e9bbf2aa90102a3 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.AddCustomRect(System.Int32,System.Int32) f72f99f2ebf92d5196aa001a1a894ebc3071ffae5a1afdb50119f106dc82849c -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.RemoveCustomRect(System.Int32) f02e036efe2be8b108949920defce7044039f1359987b344f415081742b5d064 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasPtr.GetCustomRect(System.Int32,Inno.Native.ImGui.ImFontAtlasRectPtr) dc4b772a65abbd69a677f1aebbfc05f2c537cdcc07c68f67dd21882fa0631526 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-22242b1fbbbe0296"></a>[`public Inno.Native.ImGui.ImFontAtlas* Inno.Native.ImGui.ImFontAtlasPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12308) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6865e9db2eaddc65"></a>[`public Inno.Native.ImGui.ImFontAtlasPtr.ImFontAtlasPtr(Inno.Native.ImGui.ImFontAtlas* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12303) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-80fc4b2b61ccb659"></a>[`public int Inno.Native.ImGui.ImFontAtlasPtr.AddCustomRect(int width, int height, Inno.Native.ImGui.ImFontAtlasRectPtr outR)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12676) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9361cac4974d61a7"></a>[`public int Inno.Native.ImGui.ImFontAtlasPtr.AddCustomRect(int width, int height)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12684) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c38655208a94e88c"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFont(Inno.Native.ImGui.ImFontConfigPtr fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12364) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-550a584940533fd5"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontDefault()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12380) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7824e9323eadcab9"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontDefault(Inno.Native.ImGui.ImFontConfigPtr fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12372) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6eb709c0de24b360"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontDefaultBitmap()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12412) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2fff8d0f920846ce"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontDefaultBitmap(Inno.Native.ImGui.ImFontConfigPtr fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12404) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-20a1a5c8101c1091"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontDefaultVector()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12396) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f381db20466cf7a2"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontDefaultVector(Inno.Native.ImGui.ImFontConfigPtr fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12388) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0a80becf0f2b1ed2"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(string filename, Inno.Native.ImGui.ImFontConfigPtr fontCfg, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12476) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b317d578aa792daf"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(string filename, Inno.Native.ImGui.ImFontConfigPtr fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12452) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a74299ac16d0937e"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(string filename, float sizePixels, Inno.Native.ImGui.ImFontConfigPtr fontCfg, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12420) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bd130251d941cdf3"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(string filename, float sizePixels, Inno.Native.ImGui.ImFontConfigPtr fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12444) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f75d5d7c1d747d06"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(string filename, float sizePixels, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12468) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b5af057bb8bdd63a"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(string filename, float sizePixels)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12436) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c27cf0aee8f5ca62"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(string filename, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12460) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cc4cbbf56f28c5ca"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromFileTTF(string filename)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12428) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-def74a44cb3f6eb9"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(string compressedFontDatabase85, Inno.Native.ImGui.ImFontConfigPtr fontCfg, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12604) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8a507db0544d094a"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(string compressedFontDatabase85, Inno.Native.ImGui.ImFontConfigPtr fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12580) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ded84960d889812d"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(string compressedFontDatabase85, float sizePixels, Inno.Native.ImGui.ImFontConfigPtr fontCfg, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12548) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-975e3f6a9c37d50f"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(string compressedFontDatabase85, float sizePixels, Inno.Native.ImGui.ImFontConfigPtr fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12572) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-57c26b5b1a041f7f"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(string compressedFontDatabase85, float sizePixels, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12596) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6bbb052035815e13"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(string compressedFontDatabase85, float sizePixels)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12564) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-71035dcadaf0cf40"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(string compressedFontDatabase85, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12588) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-434ab47689fed9eb"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedBase85TTF(string compressedFontDatabase85)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12556) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-025dc0d67eb57360"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedTTF(void* compressedFontData, System.ReadOnlySpan<Inno.Native.ImGui.ImFontConfig> fontCfg, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12540) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-869deff8cd3aa41b"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedTTF(void* compressedFontData, System.ReadOnlySpan<Inno.Native.ImGui.ImFontConfig> fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12524) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-86651c53164374ee"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedTTF(void* compressedFontData, float sizePixels, System.ReadOnlySpan<Inno.Native.ImGui.ImFontConfig> fontCfg, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12516) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-32262a85230e216c"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryCompressedTTF(void* compressedFontData, float sizePixels, System.ReadOnlySpan<Inno.Native.ImGui.ImFontConfig> fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12532) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e76efc502f8fadd3"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryTTF(void* fontData, System.ReadOnlySpan<Inno.Native.ImGui.ImFontConfig> fontCfg, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12508) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9e81467c51e35883"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryTTF(void* fontData, System.ReadOnlySpan<Inno.Native.ImGui.ImFontConfig> fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12492) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ee7d38b7b94c92ae"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryTTF(void* fontData, float sizePixels, System.ReadOnlySpan<Inno.Native.ImGui.ImFontConfig> fontCfg, ushort* glyphRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12484) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7278468711d4430c"></a>[`public Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontAtlasPtr.AddFontFromMemoryTTF(void* fontData, float sizePixels, System.ReadOnlySpan<Inno.Native.ImGui.ImFontConfig> fontCfg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12500) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-078a6f36f11cb905"></a>[`public void Inno.Native.ImGui.ImFontAtlasPtr.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12620) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-633de095b9973fac"></a>[`public void Inno.Native.ImGui.ImFontAtlasPtr.ClearFonts()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12652) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bdca7751dc6b811b"></a>[`public void Inno.Native.ImGui.ImFontAtlasPtr.ClearInputData()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12644) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ccad3abfeaffae7a"></a>[`public void Inno.Native.ImGui.ImFontAtlasPtr.ClearTexData()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12660) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-04984ccd27c2a20f"></a>[`public void Inno.Native.ImGui.ImFontAtlasPtr.CompactCache()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12628) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1a849a819a6fff20"></a>[`public void Inno.Native.ImGui.ImFontAtlasPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12356) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a7e81dabc3fbbbe3"></a>[`public bool Inno.Native.ImGui.ImFontAtlasPtr.Equals(Inno.Native.ImGui.ImFontAtlasPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12318) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-9de68554a1825135"></a>[`public override bool Inno.Native.ImGui.ImFontAtlasPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12319) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-997b4cc20cb4e9eb"></a>[`public bool Inno.Native.ImGui.ImFontAtlasPtr.GetCustomRect(int id, Inno.Native.ImGui.ImFontAtlasRectPtr outR)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12700) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f659ff70d07ce9b5"></a>[`public ushort* Inno.Native.ImGui.ImFontAtlasPtr.GetGlyphRangesDefault()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12668) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f02c492889dd8f80"></a>[`public override int Inno.Native.ImGui.ImFontAtlasPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12320) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-9df38cb81bb0103c"></a>[`public static bool Inno.Native.ImGui.ImFontAtlasPtr.operator ==(Inno.Native.ImGui.ImFontAtlasPtr left, Inno.Native.ImGui.ImFontAtlasPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12316) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f7acc6de03ddc448"></a>[`public static Inno.Native.ImGui.ImFontAtlasPtr.implicit operator Inno.Native.ImGui.ImFontAtlasPtr(Inno.Native.ImGui.ImFontAtlas* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12314) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e73a2680c4da6948"></a>[`public static Inno.Native.ImGui.ImFontAtlasPtr.implicit operator Inno.Native.ImGui.ImFontAtlas*(Inno.Native.ImGui.ImFontAtlasPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12315) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-75c0b3be4075023e"></a>[`public static bool Inno.Native.ImGui.ImFontAtlasPtr.operator !=(Inno.Native.ImGui.ImFontAtlasPtr left, Inno.Native.ImGui.ImFontAtlasPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12317) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ee28dd03347145c2"></a>[`public void Inno.Native.ImGui.ImFontAtlasPtr.RemoveCustomRect(int id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12692) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-89052e0774d26e5a"></a>[`public void Inno.Native.ImGui.ImFontAtlasPtr.RemoveFont(Inno.Native.ImGui.ImFontPtr font)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12612) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-98aa183ddb2e82dd"></a>[`public void Inno.Native.ImGui.ImFontAtlasPtr.SetFontLoader(Inno.Native.ImGui.ImFontLoaderPtr fontLoader)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12636) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-da269f79cd10bad4"></a>[`public ref Inno.Native.ImGui.ImFontAtlasBuilderPtr Inno.Native.ImGui.ImFontAtlasPtr.Builder { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12345) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-42ff3966b98ec6df"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImDrawListSharedDataPtr> Inno.Native.ImGui.ImFontAtlasPtr.DrawListSharedDatas { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12344) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e193c00f006e14cf"></a>[`public ref Inno.Native.ImGui.ImFontAtlasFlags Inno.Native.ImGui.ImFontAtlasPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12322) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-85b286b9fe151b80"></a>[`public ref Inno.Native.ImGui.ImFontLoaderPtr Inno.Native.ImGui.ImFontAtlasPtr.FontLoader { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12346) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cd67f23cc5cd9fa2"></a>[`public void* Inno.Native.ImGui.ImFontAtlasPtr.FontLoaderData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12348) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c0030f65c0738784"></a>[`public ref uint Inno.Native.ImGui.ImFontAtlasPtr.FontLoaderFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12349) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0eddb00124ccf91f"></a>[`public byte* Inno.Native.ImGui.ImFontAtlasPtr.FontLoaderName { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12347) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-20ccbd4b7ca28779"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasPtr.FontNextUniqueID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12343) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-92e51f13c411960d"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontPtr> Inno.Native.ImGui.ImFontAtlasPtr.Fonts { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12339) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f3e3b983a078f8b0"></a>[`public bool Inno.Native.ImGui.ImFontAtlasPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12309) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ed2206200a00dc82"></a>[`public Inno.Native.ImGui.ImFontAtlas Inno.Native.ImGui.ImFontAtlasPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12312) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-111e0c133f649d4b"></a>[`public bool Inno.Native.ImGui.ImFontAtlasPtr.Locked { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12333) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9f0412622f3932c8"></a>[`public static Inno.Native.ImGui.ImFontAtlasPtr Inno.Native.ImGui.ImFontAtlasPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12310) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-45afbb180ddb125d"></a>[`public ref Inno.Native.ImGui.ImGuiContextPtr Inno.Native.ImGui.ImFontAtlasPtr.OwnerContext { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12351) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-86cd6d9097f33a0b"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasPtr.RefCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12350) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-017ef91411ba1d19"></a>[`public bool Inno.Native.ImGui.ImFontAtlasPtr.RendererHasTextures { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12334) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bf0920771082fdc3"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontConfig> Inno.Native.ImGui.ImFontAtlasPtr.Sources { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12340) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2fbb0420da7c8a4c"></a>[`public ref Inno.Native.ImGui.ImTextureDataPtr Inno.Native.ImGui.ImFontAtlasPtr.TexData { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12331) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b312721d669f8d90"></a>[`public ref Inno.Native.ImGui.ImTextureFormat Inno.Native.ImGui.ImFontAtlasPtr.TexDesiredFormat { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12323) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-54f2ae2e3f6d5b39"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasPtr.TexGlyphPadding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12324) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eb80cc8cf9dd81c6"></a>[`public bool Inno.Native.ImGui.ImFontAtlasPtr.TexIsBuilt { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12335) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f6485b2f2afd1079"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureDataPtr> Inno.Native.ImGui.ImFontAtlasPtr.TexList { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12332) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e5552828555eb0a8"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasPtr.TexMaxHeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12328) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4ed71063c3afbed6"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasPtr.TexMaxWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12327) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-16a2ee6b94779224"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasPtr.TexMinHeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12326) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8cf1243f2bc92546"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasPtr.TexMinWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12325) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-84e073a1e4dc2a8a"></a>[`public ref int Inno.Native.ImGui.ImFontAtlasPtr.TexNextUniqueID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12342) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-17af1b490293d82a"></a>[`public bool Inno.Native.ImGui.ImFontAtlasPtr.TexPixelsUseColors { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12336) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0756a87872f2630f"></a>[`public ref Inno.Native.ImGui.ImTextureRef Inno.Native.ImGui.ImFontAtlasPtr.TexRef { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12330) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dac23fcda4337c6e"></a>[`public System.Span<System.Numerics.Vector4> Inno.Native.ImGui.ImFontAtlasPtr.TexUvLines { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12341) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-29b90cb5bce2d752"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImFontAtlasPtr.TexUvScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12337) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b842c6695fe822d3"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImFontAtlasPtr.TexUvWhitePixel { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12338) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8322997b8cea45e7"></a>[`public void* Inno.Native.ImGui.ImFontAtlasPtr.UserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12329) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e83e36786dc625a7"></a>[`public struct Inno.Native.ImGui.ImFontAtlasPtr : System.IEquatable<Inno.Native.ImGui.ImFontAtlasPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12301) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontAtlasRect</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontAtlasRect 1996dd72e00b5c21b0ae1fc511f47eba5a069e3cee3be5e89d9fad55d57f2bbb -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRect.X c3945ea410feb779a86402599b62004e558d2db2179975449d8fbfff795c82fc -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRect.Y 2fd419020c7fd41daecaced157cdd4e5e8e900b23fedd0383274f5031d011038 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRect.W 87e1a50610702c95be8dfdb5dcb37a5538ae58fddd8ff9b1b84fb23bde757ea2 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRect.H 8f68f2babcb5cbf7680092eb896ffd753b55dc00312feb217743e741e7a2b300 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRect.Uv0 15277baee3028cbc3cb3b36ad51184f2de32e155352d8a4bce3e95b18e0f8bbb -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRect.Uv1 6a8d7591845ecec6b9a93aee35929864ec47c0940d94f57590b544932d1a8a73 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRect.#ctor(System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.Numerics.Vector2,System.Numerics.Vector2) f3bea4c86fde195d1a8e43a61f16ac1d156a8636935d28b023341ba30b85bd65 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRect.Destroy 004af074831558e4bb50ef423ea784faf28cde4f6bb5c34b29b36c9a5fe15f70 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5356290a16640ef2"></a>[`public ushort Inno.Native.ImGui.ImFontAtlasRect.H`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9453) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b7a084e068c0bf9e"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImFontAtlasRect.Uv0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9454) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-520270eaf53dee74"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImFontAtlasRect.Uv1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9455) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-399006c4063b7052"></a>[`public ushort Inno.Native.ImGui.ImFontAtlasRect.W`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9452) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-72dfe8419eef5ca1"></a>[`public ushort Inno.Native.ImGui.ImFontAtlasRect.X`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9450) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-49efae5970b2f601"></a>[`public ushort Inno.Native.ImGui.ImFontAtlasRect.Y`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9451) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c9c30f5033999ba"></a>[`public Inno.Native.ImGui.ImFontAtlasRect.ImFontAtlasRect(ushort x = 0, ushort y = 0, ushort w = 0, ushort h = 0, System.Numerics.Vector2 uv0 = default(System.Numerics.Vector2), System.Numerics.Vector2 uv1 = default(System.Numerics.Vector2))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9456) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fa8f8d81fdf9fd9d"></a>[`public void Inno.Native.ImGui.ImFontAtlasRect.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65403) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3d3de4e3e6bf91d1"></a>[`public struct Inno.Native.ImGui.ImFontAtlasRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9448) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontAtlasRectEntry</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontAtlasRectEntry e22c6a84919b2dcaa2117a623d6ce9bc4c74b930698241cc3078fc0b77c8e152 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRectEntry.RawBits0_0 028f6446b0ef9029f432aa58a864f52705a7203b9dc373b8a18cf5fcef7c2a5f -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRectEntry.RawBits0_1 b4e45f0c90978118d44d7743249d207ae6bd0766364c99ada08e3b0ef1383e6e -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRectEntry.RawBits0_2 8445aa0a8cb30dca9a033f33a722887563545d8e802e5104888cd4851256b53d -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRectEntry.RawBits0_3 224851a9b03f40ebd5e27e42485d6e9cbb57a16c6b9ba7c086ad5fdaafb696d9 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectEntry.TargetIndex 3a8cc05ef8c426a6e2a7987cf72e3177f70f2b733a3084981106fb18fa49bca5 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectEntry.Generation bf0ea4cdf2d0f31a4c14a2fea1fce60ec0ca60c4323fb128af5e6df245e233b3 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectEntry.IsUsed 1cbd8ce4cb068e33875fd207b93736c8c2a3c71f4b1e679ed1d6bc9ca9c7ac58 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectEntry.#ctor(System.Int32,System.UInt32,System.UInt32) 9f25302446beae7be04f4800c12e7a368e84d6ead96aca2429a4420051194089 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7375412fc2ce0abb"></a>[`public byte Inno.Native.ImGui.ImFontAtlasRectEntry.RawBits0_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2381) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-daee5454b8da13ed"></a>[`public byte Inno.Native.ImGui.ImFontAtlasRectEntry.RawBits0_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2383) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a3e92861aab5fb3"></a>[`public byte Inno.Native.ImGui.ImFontAtlasRectEntry.RawBits0_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2385) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-88c60f3b0fe3568d"></a>[`public byte Inno.Native.ImGui.ImFontAtlasRectEntry.RawBits0_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2387) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4fb6891316ed580f"></a>[`public Inno.Native.ImGui.ImFontAtlasRectEntry.ImFontAtlasRectEntry(int targetIndex = 0, uint generation = 0, uint isUsed = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2392) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-51b0f2b03660dc5d"></a>[`public uint Inno.Native.ImGui.ImFontAtlasRectEntry.Generation { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2389) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-52f30b0f3b92f9d4"></a>[`public uint Inno.Native.ImGui.ImFontAtlasRectEntry.IsUsed { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2390) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1a2a82b2e16ee292"></a>[`public int Inno.Native.ImGui.ImFontAtlasRectEntry.TargetIndex { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2388) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5517e2e0003327b4"></a>[`public struct Inno.Native.ImGui.ImFontAtlasRectEntry`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2378) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontAtlasRectEntryPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontAtlasRectEntryPtr 888689164f64624b8ff4fb025e0cce4cb07a8b2462140b5df9b048bc48785c7d -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.#ctor(Inno.Native.ImGui.ImFontAtlasRectEntry*) 4d1fd4aa788247ae81ade8b8629633cf14a310af8a5253d20433eccfbe00153d -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.Handle 8e10e06592112e9f9edfda803784c57479070f84be3abc0a006ad71042a588a8 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.IsNull 63919494d336e4a3f784026882a80cbbdf9066bfd7ec678385d5b0c6cbea52ca -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.Null 3cde50d69b3aa652013b09464973782a64532dba1c30bc672ed0672ce1655214 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.Item(System.Int32) 2cde7823480ed50716d822f9c733007009c4fc723cf6b1f2244a82b504be593a -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.op_Implicit(Inno.Native.ImGui.ImFontAtlasRectEntry*)~Inno.Native.ImGui.ImFontAtlasRectEntryPtr c979c59411173fe8be420dd84c086cf1466aada1bf986ec4e9c98e7d5161e0fd -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.op_Implicit(Inno.Native.ImGui.ImFontAtlasRectEntryPtr)~Inno.Native.ImGui.ImFontAtlasRectEntry* 2ee9d63ca8c342e1bd7ca8a20a90855f07ecf7648993b2cbe4c9b671bed9f035 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.op_Equality(Inno.Native.ImGui.ImFontAtlasRectEntryPtr,Inno.Native.ImGui.ImFontAtlasRectEntryPtr) 2c77b0d21db6e660f656de2a5d897440f17293b717fcc6092928c1bf411c5428 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.op_Inequality(Inno.Native.ImGui.ImFontAtlasRectEntryPtr,Inno.Native.ImGui.ImFontAtlasRectEntryPtr) 32d0651bd8c950634132ec25213d4bae6a430df5fd8fafeec8fb85a04547b1ee -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.Equals(Inno.Native.ImGui.ImFontAtlasRectEntryPtr) 20f45d46b0684a8986d605b1dd37f7247ba2dbb9eca251c9e78eda35b5dc8a5d -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.Equals(System.Object) 5040b66f6ebeb408dddfedda03346e04503d1684caa012b73ea5f6380c238e4b -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.GetHashCode 9f361657e5f388407a21058c2ac23cddfbd75b556d0c9d12f486993545f560d1 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.TargetIndex 449f1d4610ecdd6c308e0a322acd2d33cfb5ad8ede449d706ffcffc921243f8e -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.Generation a83593443af7426033af758ed367b1e580fb77afd8dbc6822044361080152807 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectEntryPtr.IsUsed ed222bbb0255ff8887552acf09a3549965b7fe9c49df985196d8be276bde7f6c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4cd259705ea2bce8"></a>[`public Inno.Native.ImGui.ImFontAtlasRectEntry* Inno.Native.ImGui.ImFontAtlasRectEntryPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12714) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1078db298563c612"></a>[`public Inno.Native.ImGui.ImFontAtlasRectEntryPtr.ImFontAtlasRectEntryPtr(Inno.Native.ImGui.ImFontAtlasRectEntry* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12709) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3051673f6ec8b7fe"></a>[`public bool Inno.Native.ImGui.ImFontAtlasRectEntryPtr.Equals(Inno.Native.ImGui.ImFontAtlasRectEntryPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12724) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-12464bd98188d1d9"></a>[`public override bool Inno.Native.ImGui.ImFontAtlasRectEntryPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12725) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-18932351f85266b2"></a>[`public override int Inno.Native.ImGui.ImFontAtlasRectEntryPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12726) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-ad54f5f7def059d9"></a>[`public static bool Inno.Native.ImGui.ImFontAtlasRectEntryPtr.operator ==(Inno.Native.ImGui.ImFontAtlasRectEntryPtr left, Inno.Native.ImGui.ImFontAtlasRectEntryPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12722) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a85f87ccf5fbfb23"></a>[`public static Inno.Native.ImGui.ImFontAtlasRectEntryPtr.implicit operator Inno.Native.ImGui.ImFontAtlasRectEntryPtr(Inno.Native.ImGui.ImFontAtlasRectEntry* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12720) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-243e69f685ceaced"></a>[`public static Inno.Native.ImGui.ImFontAtlasRectEntryPtr.implicit operator Inno.Native.ImGui.ImFontAtlasRectEntry*(Inno.Native.ImGui.ImFontAtlasRectEntryPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12721) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b1bb04abb7296131"></a>[`public static bool Inno.Native.ImGui.ImFontAtlasRectEntryPtr.operator !=(Inno.Native.ImGui.ImFontAtlasRectEntryPtr left, Inno.Native.ImGui.ImFontAtlasRectEntryPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12723) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a0f85761db316af6"></a>[`public uint Inno.Native.ImGui.ImFontAtlasRectEntryPtr.Generation { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12729) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b464928f147ad47f"></a>[`public bool Inno.Native.ImGui.ImFontAtlasRectEntryPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12715) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-00de6818d91373bc"></a>[`public uint Inno.Native.ImGui.ImFontAtlasRectEntryPtr.IsUsed { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12730) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1b31a95b0a1ce506"></a>[`public Inno.Native.ImGui.ImFontAtlasRectEntry Inno.Native.ImGui.ImFontAtlasRectEntryPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12718) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3cbf776a0bac8c0b"></a>[`public static Inno.Native.ImGui.ImFontAtlasRectEntryPtr Inno.Native.ImGui.ImFontAtlasRectEntryPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12716) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aac9bba6b03f332c"></a>[`public int Inno.Native.ImGui.ImFontAtlasRectEntryPtr.TargetIndex { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12728) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c377dbeb1e52053f"></a>[`public struct Inno.Native.ImGui.ImFontAtlasRectEntryPtr : System.IEquatable<Inno.Native.ImGui.ImFontAtlasRectEntryPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12707) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontAtlasRectPtr</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontAtlasRectPtr d322477fb5b440bc870de652886a9d70a6ad3e499153a1fd5fdf79961fcd7cc4 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectPtr.#ctor(Inno.Native.ImGui.ImFontAtlasRect*) 74c13e88a7381c93cd1939365aedf7d3ae49bad46ccf57257348ec9617602314 -->
<!-- api: F:Inno.Native.ImGui.ImFontAtlasRectPtr.Handle 2e39dc0f937440570cbaafb43c20082094aad5f75bf18f778c4cea74f63c80bc -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectPtr.IsNull 2932c9ce4aa387d82a79ede43409da2dfcf8e5d41e9716d948fb7a6cc258fb0c -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectPtr.Null 360163dcd50d8834865f68a5ca4fe9499d304668b33299b307513b21607b82d7 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectPtr.Item(System.Int32) 08a4888758239f48801fdb38d1982996826f48c28691492a340151c2d5acdd43 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectPtr.op_Implicit(Inno.Native.ImGui.ImFontAtlasRect*)~Inno.Native.ImGui.ImFontAtlasRectPtr a12dee82e5235290a3ffa3deef919baeb2d9f0abed2147a5fe6e3f5258c9025e -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectPtr.op_Implicit(Inno.Native.ImGui.ImFontAtlasRectPtr)~Inno.Native.ImGui.ImFontAtlasRect* 23e48c645a6cca89551426fbe121aec969ae8aad56888ce543db2450d3bec419 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectPtr.op_Equality(Inno.Native.ImGui.ImFontAtlasRectPtr,Inno.Native.ImGui.ImFontAtlasRectPtr) f11665c992d01cdcf610799d4885cf14e1ed8a55237883d84d5ae66465ffc52c -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectPtr.op_Inequality(Inno.Native.ImGui.ImFontAtlasRectPtr,Inno.Native.ImGui.ImFontAtlasRectPtr) 7a3cdb1a8632ba2c0e45bda555530f83333c6090c60237f68c51d0304916478a -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectPtr.Equals(Inno.Native.ImGui.ImFontAtlasRectPtr) 23a74b0f5eeacd29818cf84de323b7d3eeec5166823bad551e6a0d67f99d5b3b -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectPtr.Equals(System.Object) 727351a607d7b7f7deee30f8b6e47a2beeb51ce5266f8492ddb8b107733131e4 -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectPtr.GetHashCode c1efe28913de7a8011859debde29ec855b641fd77a4c117ce69ed4213bd6d026 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectPtr.X 27833d36de8d2a0683c3611ed13e510054a873a2630025a0002a4aa0ce1d554f -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectPtr.Y 2f0af100b0334d950b616e08de33db4931bc4d791729199c1d83dc730df3518d -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectPtr.W a56eb17aab61708385e8b446f43cf124770f5534b658be16cc3098d2f31169c3 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectPtr.H 715c106ecb0da3766560240e17b78f2b9a0ee5e912261bcb1e0e9090640acee2 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectPtr.Uv0 49ad1e61b6baeb63d6ceb78b056af625134145b9c53334bad66b19fe345b7614 -->
<!-- api: P:Inno.Native.ImGui.ImFontAtlasRectPtr.Uv1 89ab70d6f77da57e60ef948e3f75cc9318d63135386ae3bc8a36ce157cd6688d -->
<!-- api: M:Inno.Native.ImGui.ImFontAtlasRectPtr.Destroy bfdbbdcea0f652a603079d2e67bbddedc61159fa13f03c45aed3a14a5f17d699 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-597c879421af7734"></a>[`public Inno.Native.ImGui.ImFontAtlasRect* Inno.Native.ImGui.ImFontAtlasRectPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12741) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64635575857460d2"></a>[`public Inno.Native.ImGui.ImFontAtlasRectPtr.ImFontAtlasRectPtr(Inno.Native.ImGui.ImFontAtlasRect* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12736) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6f3f7f03f53d0e43"></a>[`public void Inno.Native.ImGui.ImFontAtlasRectPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12765) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f7a58bfc5a6a7a07"></a>[`public bool Inno.Native.ImGui.ImFontAtlasRectPtr.Equals(Inno.Native.ImGui.ImFontAtlasRectPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12751) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-652afe028e21cde1"></a>[`public override bool Inno.Native.ImGui.ImFontAtlasRectPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12752) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-40ad99fdc1969d6f"></a>[`public override int Inno.Native.ImGui.ImFontAtlasRectPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12753) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-95cc5df45f01d967"></a>[`public static bool Inno.Native.ImGui.ImFontAtlasRectPtr.operator ==(Inno.Native.ImGui.ImFontAtlasRectPtr left, Inno.Native.ImGui.ImFontAtlasRectPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12749) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-380ae8933c9fbde4"></a>[`public static Inno.Native.ImGui.ImFontAtlasRectPtr.implicit operator Inno.Native.ImGui.ImFontAtlasRectPtr(Inno.Native.ImGui.ImFontAtlasRect* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12747) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-048e7fb12a7ed9f0"></a>[`public static Inno.Native.ImGui.ImFontAtlasRectPtr.implicit operator Inno.Native.ImGui.ImFontAtlasRect*(Inno.Native.ImGui.ImFontAtlasRectPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12748) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d7398e9a45915ca4"></a>[`public static bool Inno.Native.ImGui.ImFontAtlasRectPtr.operator !=(Inno.Native.ImGui.ImFontAtlasRectPtr left, Inno.Native.ImGui.ImFontAtlasRectPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12750) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c182c747083ca769"></a>[`public ref ushort Inno.Native.ImGui.ImFontAtlasRectPtr.H { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12758) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e227235972740ffc"></a>[`public bool Inno.Native.ImGui.ImFontAtlasRectPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12742) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-483a3cb7e3385fed"></a>[`public Inno.Native.ImGui.ImFontAtlasRect Inno.Native.ImGui.ImFontAtlasRectPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12745) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a24860a7e0151858"></a>[`public static Inno.Native.ImGui.ImFontAtlasRectPtr Inno.Native.ImGui.ImFontAtlasRectPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12743) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c7acf037d52b32c4"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImFontAtlasRectPtr.Uv0 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12759) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0becae915f45a631"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImFontAtlasRectPtr.Uv1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12760) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-80cca3d448245ff7"></a>[`public ref ushort Inno.Native.ImGui.ImFontAtlasRectPtr.W { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12757) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bb48af74897eb19c"></a>[`public ref ushort Inno.Native.ImGui.ImFontAtlasRectPtr.X { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12755) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ab80731622ccce60"></a>[`public ref ushort Inno.Native.ImGui.ImFontAtlasRectPtr.Y { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12756) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9a82c7e96176df3e"></a>[`public struct Inno.Native.ImGui.ImFontAtlasRectPtr : System.IEquatable<Inno.Native.ImGui.ImFontAtlasRectPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12734) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontBaked</code> · 29 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontBaked 4bd01dfa54b33ddd1601f5c95e409f9436261c034484bd4942a3478dc9297c07 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.IndexAdvanceX 3201f5a6b15ea9c0b1a00c9a6442aa2872de29aa97aac80af1ceaa6486e7f513 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.FallbackAdvanceX 5516f07415ebc4fb2ffd629593fb83ceef86e21ed3b068bbcbdab074ab78559f -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.Size 9a72ef1a0a7cf90d90166d2016503ed6e02f26df26210e91b2270a5087bd8d99 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.RasterizerDensity 1c721fec00f19a735a31dc41ebfa40d86978fa40a7f039ac4e62556373784e05 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.IndexLookup e72255c8ba8d862daa358fdbb39564b5812889f6507b13933f115f96a2bf5b8d -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.Glyphs 323696f106c78db6bbbfe8503505ac054cc7475c668c9ae16b379de5f6a833c9 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.FallbackGlyphIndex e55e16abf07fdf2d53dc361de6e5ac5458bd1eb82a0f11ac65eedf693307aa87 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.Ascent 8d5b516dae8ec7321b1606deb4c0e404c86c222f810cdf927fb79fcd358204ae -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.Descent 1eac196988aeb7f4f49be9da36e2e844e149639504c6468eb8c49377ac2abd0b -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.RawBits0_0 e23b0aa7acd56bec26a15c31a77cea47384c1e6dfc7af394e4b817c1b4fb12d6 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.RawBits0_1 4168796fa2c2f9e173b9c42a524076ae23d2ec72a7f1b7810a4e21ad79b6c858 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.RawBits0_2 315e41437f1de20b000de8b75baec21dde2de1b50af7d42fd1c4f91930cb565b -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.RawBits0_3 27344a72ec4ab8f1fe159f17ec0f38a30d99674b54ca60b912a6c3c2c62c4134 -->
<!-- api: P:Inno.Native.ImGui.ImFontBaked.MetricsTotalSurface 20657f22b96a47bb72b55d3aea22d56e7599ad35e95c445f4a7032efd3e27d35 -->
<!-- api: P:Inno.Native.ImGui.ImFontBaked.WantDestroy 2f4baa8086d33163b6d7a2876ea8d7085ea6a42e2c1a8a0f140cecfbd2db160c -->
<!-- api: P:Inno.Native.ImGui.ImFontBaked.LoadNoFallback ccf4cfe1ca85682c98892595be38125df5e0f65229512e0297aafd34c712398f -->
<!-- api: P:Inno.Native.ImGui.ImFontBaked.LoadNoRenderOnLayout d4b441385522eb933d634aa23d91d75ac4b7651b8591b11885e506d367a2943c -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.LastUsedFrame ef74237cf67a9f0bc99e52e841f6bd0e1e12a2aa0e0ae61010769e8a6b242bc6 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.BakedId f45afa01747d2e4a48dbe823aff1826279a62a735af4f32ab5d33579a2e7cd3b -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.OwnerFont cb1ea5a8cb17c75ebfb3eb2fc5a48d62df6f363b9ad3cb41203f02ae9cc04f02 -->
<!-- api: F:Inno.Native.ImGui.ImFontBaked.FontLoaderDatas c115110510dbd155b7eb6826e27ecf06238a23a2c2700b98c905f25b4805675e -->
<!-- api: M:Inno.Native.ImGui.ImFontBaked.#ctor(Inno.Native.ImGui.ImVector{System.Single},System.Single,System.Single,System.Single,Inno.Native.ImGui.ImVector{System.UInt16},Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImFontGlyph},System.Int32,System.Single,System.Single,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.Int32,System.UInt32,Inno.Native.ImGui.ImFont*,System.Void*) 2c8f53e18de5a637ea2fef153daca14371d4f66ee530af3ee096ae3a38d658d4 -->
<!-- api: M:Inno.Native.ImGui.ImFontBaked.Destroy 88eb215d2a4ac5d03a32937a6a9fdd835f4b6e366177b2196696604013627361 -->
<!-- api: M:Inno.Native.ImGui.ImFontBaked.ClearOutputData 2c1192dddd5c4affee92ae4f8df21cab7402bec19d5bb8abf4c95ad289ecc65c -->
<!-- api: M:Inno.Native.ImGui.ImFontBaked.FindGlyph(System.UInt16) c6cd01137ebb336a51e5b1ea56e67af495463261ad32f5fa3bebfb5fbab313c0 -->
<!-- api: M:Inno.Native.ImGui.ImFontBaked.FindGlyphNoFallback(System.UInt16) d705618450de8bd5583afc811183a836eceeb62fc2562c8fcaa9078482bea81b -->
<!-- api: M:Inno.Native.ImGui.ImFontBaked.GetCharAdvance(System.UInt16) cba709d3f5555d4988b33b73c438e5c99e7336fb4e3964ee5d5a8816f4d7f7ae -->
<!-- api: M:Inno.Native.ImGui.ImFontBaked.IsGlyphLoaded(System.UInt16) d49b8b0fa94675fa9c60ba7676b81fa863c1feccc23287ac3a43f610ba978e57 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7555e23a9d2c8947"></a>[`public float Inno.Native.ImGui.ImFontBaked.Ascent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1871) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-76ddcfbd5cb2c275"></a>[`public uint Inno.Native.ImGui.ImFontBaked.BakedId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1890) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f8b6beca2090bcc1"></a>[`public float Inno.Native.ImGui.ImFontBaked.Descent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1873) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-160255c6ea182c2d"></a>[`public float Inno.Native.ImGui.ImFontBaked.FallbackAdvanceX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1859) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aba9ee0759107ff9"></a>[`public int Inno.Native.ImGui.ImFontBaked.FallbackGlyphIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1869) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-94ae5d2351abd3cd"></a>[`public void* Inno.Native.ImGui.ImFontBaked.FontLoaderDatas`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1894) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c2841290088bf018"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontGlyph> Inno.Native.ImGui.ImFontBaked.Glyphs`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1867) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-258446819a73c411"></a>[`public Inno.Native.ImGui.ImVector<float> Inno.Native.ImGui.ImFontBaked.IndexAdvanceX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1857) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-945db06e5cd7d929"></a>[`public Inno.Native.ImGui.ImVector<ushort> Inno.Native.ImGui.ImFontBaked.IndexLookup`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1865) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b6c5393effa41fc0"></a>[`public int Inno.Native.ImGui.ImFontBaked.LastUsedFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1888) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ac0cf908b904f0ae"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontBaked.OwnerFont`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1892) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6e11fd1e9c5e12d4"></a>[`public float Inno.Native.ImGui.ImFontBaked.RasterizerDensity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1863) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe17bac1e21bc2cf"></a>[`public byte Inno.Native.ImGui.ImFontBaked.RawBits0_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1875) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8b126459d21be010"></a>[`public byte Inno.Native.ImGui.ImFontBaked.RawBits0_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1877) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-95de6cbe2f1e1d97"></a>[`public byte Inno.Native.ImGui.ImFontBaked.RawBits0_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1879) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8dfe1eb5f7255cb9"></a>[`public byte Inno.Native.ImGui.ImFontBaked.RawBits0_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1881) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e4e834592728a496"></a>[`public float Inno.Native.ImGui.ImFontBaked.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1861) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6de3adccc0b70a95"></a>[`public Inno.Native.ImGui.ImFontBaked.ImFontBaked(Inno.Native.ImGui.ImVector<float> indexAdvanceX = default(Inno.Native.ImGui.ImVector<float>), float fallbackAdvanceX = 0, float size = 0, float rasterizerDensity = 0, Inno.Native.ImGui.ImVector<ushort> indexLookup = default(Inno.Native.ImGui.ImVector<ushort>), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontGlyph> glyphs = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontGlyph>), int fallbackGlyphIndex = 0, float ascent = 0, float descent = 0, uint metricsTotalSurface = 0, uint wantDestroy = 0, uint loadNoFallback = 0, uint loadNoRenderOnLayout = 0, int lastUsedFrame = 0, uint bakedId = 0, Inno.Native.ImGui.ImFont* ownerFont = null, void* fontLoaderDatas = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1895) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-90fd475ae0cb2542"></a>[`public void Inno.Native.ImGui.ImFontBaked.ClearOutputData()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65651) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-09a744e6f3d2d889"></a>[`public void Inno.Native.ImGui.ImFontBaked.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65640) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe887f1dd5eb57b0"></a>[`public Inno.Native.ImGui.ImFontGlyph* Inno.Native.ImGui.ImFontBaked.FindGlyph(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65662) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-082a307412b3b232"></a>[`public Inno.Native.ImGui.ImFontGlyph* Inno.Native.ImGui.ImFontBaked.FindGlyphNoFallback(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65673) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-28569ae472d287a1"></a>[`public float Inno.Native.ImGui.ImFontBaked.GetCharAdvance(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65684) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0d9afc39ae9482ec"></a>[`public byte Inno.Native.ImGui.ImFontBaked.IsGlyphLoaded(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65695) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b7891afc5c2d6032"></a>[`public uint Inno.Native.ImGui.ImFontBaked.LoadNoFallback { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1884) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-226cb2ea97f32869"></a>[`public uint Inno.Native.ImGui.ImFontBaked.LoadNoRenderOnLayout { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1885) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f2403ceb1b9ee8ad"></a>[`public uint Inno.Native.ImGui.ImFontBaked.MetricsTotalSurface { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1882) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-731ff2bf2ada3648"></a>[`public uint Inno.Native.ImGui.ImFontBaked.WantDestroy { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1883) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6e22bd2d5469c900"></a>[`public struct Inno.Native.ImGui.ImFontBaked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1854) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontBakedPtr</code> · 36 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontBakedPtr 740b1efba1a03031225ba389f975d57e1924ca876d595e3bc63d42a364c6ac4b -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.#ctor(Inno.Native.ImGui.ImFontBaked*) c98a10ae232fd8422d28e67b8ccd38ed8444e0a3f72d8a08062fcb115a78c8da -->
<!-- api: F:Inno.Native.ImGui.ImFontBakedPtr.Handle f13b8f402839e0d9c3afb6a4a7bbbacddcd0c083c6c77b9c66d6ed398753d44b -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.IsNull 5f8bd39ee66a306ef85349157b0505e9a8b3119f396aa6d58c53d2838c3f6dfc -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.Null 9b1ecd9213ed0acd9e4cdf2dfa123b9288707c27a9f43e92e43c16b47076ffcd -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.Item(System.Int32) e38311b054b76353392db7d9b887d891b3b86072d711d616c7b6386d60dc8e1b -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.op_Implicit(Inno.Native.ImGui.ImFontBaked*)~Inno.Native.ImGui.ImFontBakedPtr 13f46773b7e627f0fbebcc58c08f1f4969695cf217544aae49c1acf9d27199ba -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.op_Implicit(Inno.Native.ImGui.ImFontBakedPtr)~Inno.Native.ImGui.ImFontBaked* ac4c6cff996c6fef26c0dd04d07e28ce13bbd75492a419efbd2515d6d2f1cdd9 -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.op_Equality(Inno.Native.ImGui.ImFontBakedPtr,Inno.Native.ImGui.ImFontBakedPtr) f807b06313af0c07f4b6383bad1abe3f45c0d01d322ae7774ce2546869026c3a -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.op_Inequality(Inno.Native.ImGui.ImFontBakedPtr,Inno.Native.ImGui.ImFontBakedPtr) 62953b0818b98638ae0d390e152b2d4349e8bcc11d1aa4cdd51e49f83fafc9ab -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.Equals(Inno.Native.ImGui.ImFontBakedPtr) cd21c92a7c324433c338eaaa94862d6a0c30bca9149bb2dd03046ce7b3ff27e0 -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.Equals(System.Object) d25283258479be71d852cc100f57273055ee6dc9e0aec21e55c36a23f0b2c4d7 -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.GetHashCode a24114a1abadca06dd985f87c451be0c4284f62827f9bde13164d52e1da0db63 -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.IndexAdvanceX 9f0c84577111e57da928c46edd30eb8caafb98d1488bea5826f9e6583af8c803 -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.FallbackAdvanceX f47739eb1bdc48de6bb3408ef02b47b976ea47a8bd3f3c4d3378769b43e90f9d -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.Size f017f7aadae9ee7514b70ed3a3908239d416796c894b49744d8772af3dd8a879 -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.RasterizerDensity a9ebf545a3c9488480246b32e0b421aa5259e24de241ac3ca58a58e71c74f58b -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.IndexLookup 1a9a6dfc696828f31c47e358cb347200da81fb5f9ab0ad3ff70646ca992bc70b -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.Glyphs 67a66c08c07b214cba125d0dbe8847bec5da022d9de78cbaca0b08037a9e5315 -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.FallbackGlyphIndex 5f689f1d3fff01ec9669f958fc9914e68e517e2285a9484974136f9bd35b6e71 -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.Ascent f394da8c4ae985d9fefb3a15e2b1a33f546443d937e859a417bfdc0a5db12118 -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.Descent 460fb686156d7d3cde219bd3f4c8a557022903c8038cc078a181d5ec13e679a8 -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.MetricsTotalSurface 9ac499ab09752f5105eac956dc6e8c72864afccc64d5eb4fdf047dc6755fc573 -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.WantDestroy 8b107491d892508ef0eed11d278b4ece39301f0472bc0078c15e78215a987a9b -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.LoadNoFallback 82027cda6e1d652f41fe56865281753b29fd60c7b692e46f2864ce36fa6f8f2a -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.LoadNoRenderOnLayout cd23adb8c8de3c9064ae7df31a9f3dabdb839178a7fc61f25a7782a59fddbee5 -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.LastUsedFrame 68eb97918227d49b8c98281c78129b1c3f35a1ab8faf1843f77172bc2dcd8066 -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.BakedId b1a6650a19bb654400e3ffff6b2824662afdee85548731e59c8348c40e4de018 -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.OwnerFont 7d26dc6a0c50a9aa207ba465cf103fa3f75b97a4b084387ff239580ee53d685c -->
<!-- api: P:Inno.Native.ImGui.ImFontBakedPtr.FontLoaderDatas d675c2b58f44b5f03facfba82f43247e672cf066fa93cea84ba00ff4d3269802 -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.Destroy 4958dfe7e963de2242237739847a6302b6379e424d55bd4e7e474f4e392e8f2c -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.ClearOutputData 38f3dbe735b978655f561d315db48203bb35e2c3edb8fc2d1620746abf68d426 -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.FindGlyph(System.UInt16) 97fc12d9722a48628050ceef8d439b197861a915ee2c32e43fb4ba2a256a8eaf -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.FindGlyphNoFallback(System.UInt16) 4da104888d6352655fea45515032e43b6129b9b732372eb00379d89b7883822e -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.GetCharAdvance(System.UInt16) ab6a7f69041f0039c9438f8e437b26ab7bb9419809bc134e37b8a78a90bef545 -->
<!-- api: M:Inno.Native.ImGui.ImFontBakedPtr.IsGlyphLoaded(System.UInt16) 1bfd3b8375438a9f66c259e68e47b9386b7f34ccba494544697ee8ba842586fe -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-05434be0ca33dda5"></a>[`public Inno.Native.ImGui.ImFontBaked* Inno.Native.ImGui.ImFontBakedPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12779) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2896364e7cb84f82"></a>[`public Inno.Native.ImGui.ImFontBakedPtr.ImFontBakedPtr(Inno.Native.ImGui.ImFontBaked* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12774) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5cbb52cbf3d6713f"></a>[`public void Inno.Native.ImGui.ImFontBakedPtr.ClearOutputData()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12822) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6cd9c9e32ab0bb09"></a>[`public void Inno.Native.ImGui.ImFontBakedPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12814) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dcb451b89a69dfb7"></a>[`public bool Inno.Native.ImGui.ImFontBakedPtr.Equals(Inno.Native.ImGui.ImFontBakedPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12789) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-106e62ac7a050179"></a>[`public override bool Inno.Native.ImGui.ImFontBakedPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12790) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-c3a9e2ad5ce7e6c7"></a>[`public Inno.Native.ImGui.ImFontGlyphPtr Inno.Native.ImGui.ImFontBakedPtr.FindGlyph(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12830) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d83b816607c4ef12"></a>[`public Inno.Native.ImGui.ImFontGlyphPtr Inno.Native.ImGui.ImFontBakedPtr.FindGlyphNoFallback(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12838) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-55bd17ce563cd2e0"></a>[`public float Inno.Native.ImGui.ImFontBakedPtr.GetCharAdvance(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12846) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a0e90b1cb2995bf9"></a>[`public override int Inno.Native.ImGui.ImFontBakedPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12791) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-781c67d127493371"></a>[`public bool Inno.Native.ImGui.ImFontBakedPtr.IsGlyphLoaded(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12854) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e6f020323e9c575f"></a>[`public static bool Inno.Native.ImGui.ImFontBakedPtr.operator ==(Inno.Native.ImGui.ImFontBakedPtr left, Inno.Native.ImGui.ImFontBakedPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12787) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-81777ef801796334"></a>[`public static Inno.Native.ImGui.ImFontBakedPtr.implicit operator Inno.Native.ImGui.ImFontBakedPtr(Inno.Native.ImGui.ImFontBaked* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12785) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b69b7aadb3e27a3d"></a>[`public static Inno.Native.ImGui.ImFontBakedPtr.implicit operator Inno.Native.ImGui.ImFontBaked*(Inno.Native.ImGui.ImFontBakedPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12786) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dbe9c8a0eda158b1"></a>[`public static bool Inno.Native.ImGui.ImFontBakedPtr.operator !=(Inno.Native.ImGui.ImFontBakedPtr left, Inno.Native.ImGui.ImFontBakedPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12788) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-627624d5199d9072"></a>[`public ref float Inno.Native.ImGui.ImFontBakedPtr.Ascent { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12800) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-00335079507a8965"></a>[`public ref uint Inno.Native.ImGui.ImFontBakedPtr.BakedId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12807) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-31c98a6171174612"></a>[`public ref float Inno.Native.ImGui.ImFontBakedPtr.Descent { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12801) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-639288678d3cefe9"></a>[`public ref float Inno.Native.ImGui.ImFontBakedPtr.FallbackAdvanceX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12794) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6ca5b4937aab00f5"></a>[`public ref int Inno.Native.ImGui.ImFontBakedPtr.FallbackGlyphIndex { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12799) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-69d9cfd6b7fd1c74"></a>[`public void* Inno.Native.ImGui.ImFontBakedPtr.FontLoaderDatas { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12809) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-33df0ebf7487c08d"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontGlyph> Inno.Native.ImGui.ImFontBakedPtr.Glyphs { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12798) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-78b338520801522e"></a>[`public ref Inno.Native.ImGui.ImVector<float> Inno.Native.ImGui.ImFontBakedPtr.IndexAdvanceX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12793) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c8a4765374c1d360"></a>[`public ref Inno.Native.ImGui.ImVector<ushort> Inno.Native.ImGui.ImFontBakedPtr.IndexLookup { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12797) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bbd2bafb4a069054"></a>[`public bool Inno.Native.ImGui.ImFontBakedPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12780) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d380a1824687988f"></a>[`public Inno.Native.ImGui.ImFontBaked Inno.Native.ImGui.ImFontBakedPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12783) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f0a0343481b8ad21"></a>[`public ref int Inno.Native.ImGui.ImFontBakedPtr.LastUsedFrame { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12806) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-68be5db5625eb442"></a>[`public uint Inno.Native.ImGui.ImFontBakedPtr.LoadNoFallback { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12804) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5f89021bd54c370f"></a>[`public uint Inno.Native.ImGui.ImFontBakedPtr.LoadNoRenderOnLayout { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12805) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e0462e13a12bbe4a"></a>[`public uint Inno.Native.ImGui.ImFontBakedPtr.MetricsTotalSurface { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12802) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dbd6815810b771fd"></a>[`public static Inno.Native.ImGui.ImFontBakedPtr Inno.Native.ImGui.ImFontBakedPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12781) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7bfae6c6059e2bf9"></a>[`public ref Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontBakedPtr.OwnerFont { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12808) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cbb7bfd4373587c2"></a>[`public ref float Inno.Native.ImGui.ImFontBakedPtr.RasterizerDensity { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12796) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5c3339fe73f43dfd"></a>[`public ref float Inno.Native.ImGui.ImFontBakedPtr.Size { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12795) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3e570c4ff82a0916"></a>[`public uint Inno.Native.ImGui.ImFontBakedPtr.WantDestroy { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12803) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fcd6466a4512ce24"></a>[`public struct Inno.Native.ImGui.ImFontBakedPtr : System.IEquatable<Inno.Native.ImGui.ImFontBakedPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12772) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontConfig</code> · 68 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontConfig 823ac208e4158d735a296b1385ae3bdf78eb8c218d08574eefdb03e85b9e7e3b -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_0 c04eb2ed41de14e0fd473d4f622cec72ab46aeb7e213c2b691110a8a96461201 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_1 574235c969247db6fcc2ccd75dd88bee392c57cb81bb85f7d65867a292751d54 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_2 fe330bc5ac6e6f972d53e95fe1a2ec2d01ce09011d29fd265760f79a32ef70f7 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_3 23ea4f55c48ec32952f71792da456918f41f13d6bc86fd56f433035a0c92fb1b -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_4 968525eb4b03512fc55f2aca292a7b750589aa3416f3d61c5581cc06bfb34f06 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_5 3d1c02bfe91574917acc05ac1f3da469b8396169c61b7842607977aa2fc53348 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_6 5532cd2a734c6abda7fd0be9577fcb8f55c1e3552ab6d75ae417e0e0537d2b9e -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_7 552265d9da31cbaaba1e0e4137c8263289dac67c09ab64ae3f670ded6f28deb6 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_8 f778436982eec8e94b9eabd1cced8a889d4fc52c2d6540458335afb9b0b8b142 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_9 86f8c02674f6f87288e4090f0101df4a6bd1be2cf85b5b6b2bd293308a1df0d7 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_10 c65f78cd463c53fa6f8a22cb2c2d6b2c16010abcbf05e86099f0ed0e862e2e41 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_11 438d308c11bdc86a21a94ba8f230dd308a32744579fbe0f0ddcf9cbae38fdb2d -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_12 acabeb9b7f1f8431abaed359773453fc7c11ea4dfa62f3f757b6a504566fab5b -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_13 e8231350c0609c5b36d23212303da40db744bedef9bbb9b62ea256d29a6bc0d1 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_14 2c8a0718d33a35520f49686684bbd5f5bcddb6fefdc7672ef04008a877e59e5a -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_15 3196eda141530de3a9f6ba92f8453d50c89a1b25eb71804cd92e76f377f21538 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_16 f38110f252dffd33c29b678775162ce218349419f2bfee9de8e1a1d374e522cf -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_17 2d810ee341a1a4200346561b5fe0a40c9729f654a61f46ac7fa271b838f80fd3 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_18 1c033c5773cf71f906620cb94021e9e0f8bcc29b0a89492aee28b82ef06ef687 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_19 b81e450f488370cf16a984fd413bf59c5edc764a5bb5afb13a82b53cb4a34dbe -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_20 89fe2152dfc8cf2e8929d980119cefc2215ce510f25e10c9d65f1aa53d985e78 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_21 c20f77da1334151c92d09bd03b8ef5d6684b3314c1819255f5cf06df9b2cafa8 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_22 60e75d0c79d3d8ca5287fb74c23a964b8c7d489d79bd4876dfbcb01c8555479c -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_23 ebfa6073f9f5d8ec0b614d6b0521bd844b236155ccbf0839f910d36f81c21cfc -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_24 33d5fe4f21d2ab3f27a9e27448a65ecd6cdbc46a81a4d723f1bebdf10fb20960 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_25 e7c9a5869243a89b49d2a0a99f8c4737b977ea9a13da39e3c2c3839d281e1568 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_26 83d548dcc4874735f2ece7befa97acffc2d0fa08af7041133a0b636848fdd872 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_27 e0e366ab085acece83da1a63b26c6f1779d168a735bd6cb41150cda628935ed2 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_28 716696b00631c7cb10ac0c7f71a8bfa3d9ba489dffbbbebf85bea2eacd6f3452 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_29 4838bb65318e72390b44bac5e16d7af6f37c364d3319d7bdf5032d693145bd8b -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_30 cfba695641eefa5011bb3b800e18331841d7c8e4bf1c9947c9edb882778f84a0 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_31 2338caa7ab0436a9232ac0f4d206ce73f1954e99ae6d26d1335b713faee2b6d6 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_32 8320ad523190c490a8e97306e18cd15d476cf3920c5b7b2800ba7ad46309a433 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_33 10eb5423222383ea8928fea05421719832c9da8d23036985c2fc0ee649f9c928 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_34 0b5175fc34c5a920c851678e2c12c945558c8b392372c288741726b649271129 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_35 4b8305909e5db48dc4d2a1994abdb0f98e8a1f90ec6097ec98b09d67efe890be -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_36 a67e9d5c9d5da3ddbb57a6b3a9785eadc069a05a8f4bb7c668c2ef84f6730fa7 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_37 3f168ebee708c8019f5b2c4fab4cc91979fab19f566bda7f886403de9ac33a42 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_38 80359795d01c2507ae89979e304ad3cbcfa4e9012001077c9cd3270db7fbd32f -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Name_39 2da6d7ee519fdff32d2eb8d365593f29e75eed73d55d1c5fe7bba9adb8c77a3b -->
<!-- api: P:Inno.Native.ImGui.ImFontConfig.Name 353e69c8b79a0afb574a9b0c624918ccea720edfe99dc103337d630dae186233 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontData 8bc14e762f59e225d11656df3ee54839b16401366d5ce510f4d54fbc596486ca -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontDataSize c2685aa3aed4e25d1960b207ecd685145bdd441e5d3cef388cdad2a29677ad2f -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontDataOwnedByAtlas ccba5b455a890ce595e02a5fa576b2cbb9685910c64ccf312c9524ac0dd7e41f -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.MergeMode 58035bee83a8b66a5903bad5facaff4f9f42219aa49ea409a96b1897f8fb9566 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.PixelSnapH 61156fe7d73c2cead26c75f3b9215f94339272cd4bc047d589b80d149b0a16a7 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.OversampleH 05ce16e0b86d2e2abcc8e15463abe34f658942349b40e41ed54129b2b771d41f -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.OversampleV e3f92e77deafcd442ea0224c2b02f580dd2c11c78b2603485afb4c602d58ea52 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.EllipsisChar e6892a28797e83829ed0c81991a7b08dcc6c2a5ae0c9d5c3eb853a74e557baf5 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.SizePixels 9f8d0be4885029f80be17892c07330c69bed4e1028fe14024ce66738bc8b1f71 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.GlyphRanges 252543dc0fd3519ce1beed9ba9fb8c3f737c67e47ea1eab247d5bc4fd9b08bbb -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.GlyphExcludeRanges 0b977aabb4fcd601f5479743871bb8d0cd6d4a18499d91d8f1ba747aa6e89cea -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.GlyphOffset a389877bc64eb76b0bbbf0f3cfb84fc986b8da64baa09db40528cc2031a2e105 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.GlyphMinAdvanceX 60ac1b73702ff5b0cacd586b7eb4340d1becd5641832bad55db44a1b4a4522b7 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.GlyphMaxAdvanceX 63fec218032f20a94881b863a7cf61daef46f1e281745c1c21aaa1e4068afe3d -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.GlyphExtraAdvanceX 47af0f7333f08e9b2475c01449a3caf5689acc55aaf21d57b685d20c018b7339 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontNo 304de142e7de9b412fb00d09c1fdf59a64badb99aededc1341ef656676f65f77 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontLoaderFlags 80a60d892c058d0e25c01f8ab6a3d43d3bad36574e9744d2dfc8cb30bf7ab6ab -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.RasterizerMultiply 954c87aac86da7741712d5e89f3dbea548f51d3498de170597d9cd206f736afa -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.RasterizerDensity d5a19d17e18b572eae8ed46af135fc73960c4f647562cacefea4270df32b793f -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.ExtraSizeScale 437db4b9236b2b7277110b7d2a77467219fabd95f550e064688bce5a7d0d73fa -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.Flags 67164e204c9bd412d1d3c80e4671a61cb16c8090ecf7773e8230f50364118a81 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.DstFont d268e4bb907a4450bc849236ca602dc91b497243f0d9b0f97eb4379b1b93c23d -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontLoader 4315784b8dc0012e82178b622969c8a025143139cf46114c82b92177da33d8d1 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfig.FontLoaderData d54e08a889dfd441ab590fea00e30a168db9f09855ba45643806924095a3d5ee -->
<!-- api: M:Inno.Native.ImGui.ImFontConfig.#ctor(System.Byte*,System.Void*,System.Int32,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.UInt16,System.Single,System.UInt16*,System.UInt16*,System.Numerics.Vector2,System.Single,System.Single,System.Single,System.UInt32,System.UInt32,System.Single,System.Single,System.Single,Inno.Native.ImGui.ImFontFlags,Inno.Native.ImGui.ImFont*,Inno.Native.ImGui.ImFontLoader*,System.Void*) 7137c4173daf6796808476738fef375e67aef1b5cb356f36edbf14fc88ce9864 -->
<!-- api: M:Inno.Native.ImGui.ImFontConfig.Destroy dfee5596997f8b841318cd8dfd3d018e69f9e885775ade35a774422238cb804c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f7a782a852fd8df2"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontConfig.DstFont`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2017) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e124191f544ed040"></a>[`public ushort Inno.Native.ImGui.ImFontConfig.EllipsisChar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2003) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c821cd35c0297253"></a>[`public float Inno.Native.ImGui.ImFontConfig.ExtraSizeScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2015) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c4b17a3fff70e9a4"></a>[`public Inno.Native.ImGui.ImFontFlags Inno.Native.ImGui.ImFontConfig.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2016) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a3adf85e179cfe2"></a>[`public void* Inno.Native.ImGui.ImFontConfig.FontData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1996) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ee48a63074d3d5ef"></a>[`public byte Inno.Native.ImGui.ImFontConfig.FontDataOwnedByAtlas`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1998) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b0ee7fca47abcd77"></a>[`public int Inno.Native.ImGui.ImFontConfig.FontDataSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1997) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-573335bd0068963b"></a>[`public Inno.Native.ImGui.ImFontLoader* Inno.Native.ImGui.ImFontConfig.FontLoader`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2018) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-16ac50676bd181ae"></a>[`public void* Inno.Native.ImGui.ImFontConfig.FontLoaderData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2019) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6ce0cfa96d1f2e77"></a>[`public uint Inno.Native.ImGui.ImFontConfig.FontLoaderFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2012) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d12400559f1828aa"></a>[`public uint Inno.Native.ImGui.ImFontConfig.FontNo`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2011) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-039e88813ecd0afb"></a>[`public ushort* Inno.Native.ImGui.ImFontConfig.GlyphExcludeRanges`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2006) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-90324cfee3b31e9c"></a>[`public float Inno.Native.ImGui.ImFontConfig.GlyphExtraAdvanceX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2010) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2bcbe73f40768c30"></a>[`public float Inno.Native.ImGui.ImFontConfig.GlyphMaxAdvanceX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2009) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-406a7164ad2ffa5f"></a>[`public float Inno.Native.ImGui.ImFontConfig.GlyphMinAdvanceX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2008) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c0251835e77feb0"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImFontConfig.GlyphOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2007) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-10211709913ba67e"></a>[`public ushort* Inno.Native.ImGui.ImFontConfig.GlyphRanges`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2005) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-98fdc2f1ba2abfc2"></a>[`public byte Inno.Native.ImGui.ImFontConfig.MergeMode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1999) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7be46e57cc604ed9"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1954) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5540698c17899c59"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1955) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13a220f17dfeb066"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1964) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d492c87ff0c74b9b"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1965) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-51cece2d45512d48"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1966) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c01975a37c96e79b"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1967) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8f6bd1963702e018"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1968) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-955ee978ee41c130"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1969) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-236717d32e4cb92f"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1970) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f73d1656f75fcfd3"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1971) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6146dcbc655afa05"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1972) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-abcfabc268d76414"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1973) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2a17714cf1d1188"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1956) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-995a06423bbc7f21"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1974) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cb489e92c3e270cc"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1975) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a879316f80f0a1c"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1976) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d445a203ab2d70e2"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1977) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8866b1ba44afdefb"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1978) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f9d5d28d9478ab1a"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1979) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d458a093b95969a"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1980) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce2ca585ff250748"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1981) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-412fa96d442d0199"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1982) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-496e78332f239ea4"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1983) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-94d9b33842904de2"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1957) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f5e43149ec0fa0b"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1984) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-342b7a2a5bf03c14"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1985) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d853d043592b77d"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1986) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cefd5423fff139d9"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_33`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1987) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a5d95fa86750cf83"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_34`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1988) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-188582077168bb0f"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_35`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1989) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2aba48301211cafc"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_36`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1990) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-78a11ab04396b14a"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_37`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1991) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6ed59afe30867844"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_38`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1992) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64453b62863b2499"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_39`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1993) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b4bd21de9e6e2ad"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1958) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-23d8e359e2dce5be"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1959) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b4a38cab05679077"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1960) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-921311e51e2f271b"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1961) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-044e7990c521d32f"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1962) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc10fa195a65d70a"></a>[`public byte Inno.Native.ImGui.ImFontConfig.Name_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1963) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5f9a5ee08aa733b6"></a>[`public byte Inno.Native.ImGui.ImFontConfig.OversampleH`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2001) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-caae6c2d960239b5"></a>[`public byte Inno.Native.ImGui.ImFontConfig.OversampleV`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2002) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-73634d547402eee6"></a>[`public byte Inno.Native.ImGui.ImFontConfig.PixelSnapH`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2000) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eb205bf404ccd87e"></a>[`public float Inno.Native.ImGui.ImFontConfig.RasterizerDensity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2014) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b6d581e03caffac"></a>[`public float Inno.Native.ImGui.ImFontConfig.RasterizerMultiply`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2013) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cab5c1423b74c83e"></a>[`public float Inno.Native.ImGui.ImFontConfig.SizePixels`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2004) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5cb7b6981e085588"></a>[`public Inno.Native.ImGui.ImFontConfig.ImFontConfig(byte* name = null, void* fontData = null, int fontDataSize = 0, byte fontDataOwnedByAtlas = 0, byte mergeMode = 0, byte pixelSnapH = 0, byte oversampleH = 0, byte oversampleV = 0, ushort ellipsisChar = 0, float sizePixels = 0, ushort* glyphRanges = null, ushort* glyphExcludeRanges = null, System.Numerics.Vector2 glyphOffset = default(System.Numerics.Vector2), float glyphMinAdvanceX = 0, float glyphMaxAdvanceX = 0, float glyphExtraAdvanceX = 0, uint fontNo = 0, uint fontLoaderFlags = 0, float rasterizerMultiply = 0, float rasterizerDensity = 0, float extraSizeScale = 0, Inno.Native.ImGui.ImFontFlags flags = Inno.Native.ImGui.ImFontFlags.None, Inno.Native.ImGui.ImFont* dstFont = null, Inno.Native.ImGui.ImFontLoader* fontLoader = null, void* fontLoaderData = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2020) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ae9f59a7e819a2dd"></a>[`public void Inno.Native.ImGui.ImFontConfig.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65284) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-81d66819b997c7d9"></a>[`public System.Span<byte> Inno.Native.ImGui.ImFontConfig.Name { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1994) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e5d93f6165a04048"></a>[`public struct Inno.Native.ImGui.ImFontConfig`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1952) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontConfigPtr</code> · 39 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontConfigPtr df8558d5506fa889c3e3fd4836ea368719ee6e77046fbfcbf138e02567214ec8 -->
<!-- api: M:Inno.Native.ImGui.ImFontConfigPtr.#ctor(Inno.Native.ImGui.ImFontConfig*) e560b6d942a39a07a689414307e7ed22e4eafdca24d45439a53ae1a733c45960 -->
<!-- api: F:Inno.Native.ImGui.ImFontConfigPtr.Handle 1664a8a714fe593f307dc56ccb696cefd61855c194b88843521d161e52e94193 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.IsNull 9ac249006d9973b62e68b8cabca46772eff957521c58dab206b0dd4e966099f0 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.Null 40ea31d9aaf3fcb7a1d531e4874fa27eeb798902697c70414b536f1010b7cb61 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.Item(System.Int32) 81ee66aaef11eb9db5fbe38f567f54cedfb883f367561b1546284e83052fa9ba -->
<!-- api: M:Inno.Native.ImGui.ImFontConfigPtr.op_Implicit(Inno.Native.ImGui.ImFontConfig*)~Inno.Native.ImGui.ImFontConfigPtr 0ea4174e69ea21288e01458a0b656cdfbfcbfeae935eeabc21665d92aae53a17 -->
<!-- api: M:Inno.Native.ImGui.ImFontConfigPtr.op_Implicit(Inno.Native.ImGui.ImFontConfigPtr)~Inno.Native.ImGui.ImFontConfig* df478775b0d6d8616cc6ee3d915d11bb73ab948e71b1fe1c5d7486c91a192a16 -->
<!-- api: M:Inno.Native.ImGui.ImFontConfigPtr.op_Equality(Inno.Native.ImGui.ImFontConfigPtr,Inno.Native.ImGui.ImFontConfigPtr) 1665812c4fa77234767debf6ba4f2c0d528d8d9fd1402dabfa6460f1a8898546 -->
<!-- api: M:Inno.Native.ImGui.ImFontConfigPtr.op_Inequality(Inno.Native.ImGui.ImFontConfigPtr,Inno.Native.ImGui.ImFontConfigPtr) 6d7d9b9b962a5a9911ff07e1ce553ce5558911c26fb4c3fa76adef52fe42e366 -->
<!-- api: M:Inno.Native.ImGui.ImFontConfigPtr.Equals(Inno.Native.ImGui.ImFontConfigPtr) 1db898fc632153ac3f1889571ad88d5ca668225a1e746da573c52fe975dff944 -->
<!-- api: M:Inno.Native.ImGui.ImFontConfigPtr.Equals(System.Object) 8602e4a4c294461c530cf7bf7252d1c13c459dfecc4ee1c70f88354a408c4a63 -->
<!-- api: M:Inno.Native.ImGui.ImFontConfigPtr.GetHashCode dd4251d3b6d23cdecf23128f4fe56ac0f7e5668d675ddfa18413fce1a460e346 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.Name 56d20a452df1a1247761b46b6c52d828a57875b42a780c99568ed794ec370293 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.FontData df77c848263fc76720a6ca84969e2bb1a09b8f43501b26c180706bbc8270d762 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.FontDataSize 16479cd58514c21a1f646f1ccffb3aae3fb22e89341ddb114df8e68fcd12c0a7 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.FontDataOwnedByAtlas cc8a6aa92395f25ce70353901f3630e7e2f17a6d1152c57e60f5e5dbdf438450 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.MergeMode 7457bc5c40c12b58f01f9b30251819ab422cbaae03e6df2b3b18a7032562bba3 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.PixelSnapH c73fc6efd781dda34a0868ed7b2e9f0763836444b09ddf7d7bea26cfd74ce1b1 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.OversampleH 88c2f2511d78c611835013d005b6c7dec6a859178a7c562b0399a3c71ba3ebcb -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.OversampleV 436fa3d60c5220628d3c22aab9a5966a74e73faa0af3d3d89df31d3bcf207559 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.EllipsisChar 76ae7c11205364f4b155b7bdad153be32eb74e2d3b99044b2cf3b6ec8fdb04f7 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.SizePixels 55a07a4d8037bcfa38a920a273276d7f95bb819fdfb58bf38dbf1814e3e826c7 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.GlyphRanges 3aad2d5e726d4e329de26df3957633cca415d35b6d73df128218777f0eb474b8 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.GlyphExcludeRanges 62df604b2cd4bf811e94f7bbc4961bb7d739d8c1302aaca5bc04934d02aa4b63 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.GlyphOffset 5aecfa853dbd0c8e0d0824e47bdb7a621983dbf48ecb2997e3d47be7f9e1ab10 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.GlyphMinAdvanceX 97e54faf7a02f45e91c6061d57aab7f688cca21580985e06efebe6005f79e83b -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.GlyphMaxAdvanceX 7a013033832f9ff98a6642f017709e62f1f02b3266a8f4c1ee40727ce63e998b -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.GlyphExtraAdvanceX 59310ad0f7eb7eff36e8f17352c7b4257fe4376482622478e972f386fb21a200 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.FontNo 5f2d099784aa1344c0a9ef6e08c7bc7493361896fe2b9ddca422f7440df82e32 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.FontLoaderFlags dcac356667668f0c796f83113c808731c2e8892eb3012eaebf938ec43229d6d5 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.RasterizerMultiply dbf1d970fb56eb54d15af019b618c857b6883e93b7edb3003f611f6d7561a9fc -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.RasterizerDensity 94dafd91c6e3281cda200bde271199bd7907143ee2b6d21fd30b675c9dc3ce1b -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.ExtraSizeScale 59b9472b2f753fdfdd2efad2cb2a04c2d18a3a1f7dce78712a564a2302a29644 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.Flags d875dc9da046b37c4e6a767ab8d55cdaf2f570f40450c1cd93bfd096b05e1b04 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.DstFont 748cb44d09ffa7285d5daa4b980b7cfea1599c8edb817efe0ba5c8d7ff35b8a1 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.FontLoader 2ff98a3fa5a4dbf5feb5fba6e935cdd22bf37c3bfa2efe20c737e81c1e3a3253 -->
<!-- api: P:Inno.Native.ImGui.ImFontConfigPtr.FontLoaderData 711636a54710d9d8ea3cb1b4332250824ab4273e37decbb4e828ce709ecfea11 -->
<!-- api: M:Inno.Native.ImGui.ImFontConfigPtr.Destroy 6dac3e9f3617e1732969606f6a77a51afeb75a9d01c2996c268b22c3e294432b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f855ac9991f6f2ad"></a>[`public Inno.Native.ImGui.ImFontConfig* Inno.Native.ImGui.ImFontConfigPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12868) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f44125900b2b21c"></a>[`public Inno.Native.ImGui.ImFontConfigPtr.ImFontConfigPtr(Inno.Native.ImGui.ImFontConfig* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12863) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-09580f871041f635"></a>[`public void Inno.Native.ImGui.ImFontConfigPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12911) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f13692e349d40d71"></a>[`public bool Inno.Native.ImGui.ImFontConfigPtr.Equals(Inno.Native.ImGui.ImFontConfigPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12878) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-eb5f3a17280f512a"></a>[`public override bool Inno.Native.ImGui.ImFontConfigPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12879) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-4908ef392d8732a2"></a>[`public override int Inno.Native.ImGui.ImFontConfigPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12880) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-7ef79e5cbbad802d"></a>[`public static bool Inno.Native.ImGui.ImFontConfigPtr.operator ==(Inno.Native.ImGui.ImFontConfigPtr left, Inno.Native.ImGui.ImFontConfigPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12876) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9eabd9b3d77181ff"></a>[`public static Inno.Native.ImGui.ImFontConfigPtr.implicit operator Inno.Native.ImGui.ImFontConfigPtr(Inno.Native.ImGui.ImFontConfig* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12874) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e60d33664ecc1902"></a>[`public static Inno.Native.ImGui.ImFontConfigPtr.implicit operator Inno.Native.ImGui.ImFontConfig*(Inno.Native.ImGui.ImFontConfigPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12875) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-615340d8ccfeef7d"></a>[`public static bool Inno.Native.ImGui.ImFontConfigPtr.operator !=(Inno.Native.ImGui.ImFontConfigPtr left, Inno.Native.ImGui.ImFontConfigPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12877) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1fe7f9d7ecd4a74a"></a>[`public ref Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontConfigPtr.DstFont { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12904) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-68c2f131f474b7e4"></a>[`public ref ushort Inno.Native.ImGui.ImFontConfigPtr.EllipsisChar { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12890) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7c6595191f9dbc28"></a>[`public ref float Inno.Native.ImGui.ImFontConfigPtr.ExtraSizeScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12902) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-372c3c00ceafd4d8"></a>[`public ref Inno.Native.ImGui.ImFontFlags Inno.Native.ImGui.ImFontConfigPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12903) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b884d4b5163ee351"></a>[`public void* Inno.Native.ImGui.ImFontConfigPtr.FontData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12883) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-df7ad4fc68642f1e"></a>[`public bool Inno.Native.ImGui.ImFontConfigPtr.FontDataOwnedByAtlas { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12885) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2770a6442cacad44"></a>[`public ref int Inno.Native.ImGui.ImFontConfigPtr.FontDataSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12884) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e73d229fcf7f68f5"></a>[`public ref Inno.Native.ImGui.ImFontLoaderPtr Inno.Native.ImGui.ImFontConfigPtr.FontLoader { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12905) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-481dc7c3d581f40b"></a>[`public void* Inno.Native.ImGui.ImFontConfigPtr.FontLoaderData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12906) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7d3921ec92a1a01d"></a>[`public ref uint Inno.Native.ImGui.ImFontConfigPtr.FontLoaderFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12899) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-31e7d833c6623cca"></a>[`public ref uint Inno.Native.ImGui.ImFontConfigPtr.FontNo { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12898) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e05b239994015ffc"></a>[`public ushort* Inno.Native.ImGui.ImFontConfigPtr.GlyphExcludeRanges { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12893) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1e097f687f8dd0e1"></a>[`public ref float Inno.Native.ImGui.ImFontConfigPtr.GlyphExtraAdvanceX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12897) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c67038f8591be211"></a>[`public ref float Inno.Native.ImGui.ImFontConfigPtr.GlyphMaxAdvanceX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12896) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6ca269f55b4961cb"></a>[`public ref float Inno.Native.ImGui.ImFontConfigPtr.GlyphMinAdvanceX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12895) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-99ee8e2cc53dd3d7"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImFontConfigPtr.GlyphOffset { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12894) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-358cbc50b518b2bd"></a>[`public ushort* Inno.Native.ImGui.ImFontConfigPtr.GlyphRanges { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12892) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c9e330cbaed279a2"></a>[`public bool Inno.Native.ImGui.ImFontConfigPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12869) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8af467b8aca6a75c"></a>[`public Inno.Native.ImGui.ImFontConfig Inno.Native.ImGui.ImFontConfigPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12872) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5429b66a9740c9a3"></a>[`public bool Inno.Native.ImGui.ImFontConfigPtr.MergeMode { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12886) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-45f6d146c023d923"></a>[`public System.Span<byte> Inno.Native.ImGui.ImFontConfigPtr.Name { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12882) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-060fa639d72a0374"></a>[`public static Inno.Native.ImGui.ImFontConfigPtr Inno.Native.ImGui.ImFontConfigPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12870) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-383f7fa1ebbc70ec"></a>[`public ref byte Inno.Native.ImGui.ImFontConfigPtr.OversampleH { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12888) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3f5e769bfc4195d4"></a>[`public ref byte Inno.Native.ImGui.ImFontConfigPtr.OversampleV { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12889) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-32ba7a1d2a34585d"></a>[`public bool Inno.Native.ImGui.ImFontConfigPtr.PixelSnapH { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12887) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ab4d252bfdf37755"></a>[`public ref float Inno.Native.ImGui.ImFontConfigPtr.RasterizerDensity { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12901) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5c4bdc5f83c28e69"></a>[`public ref float Inno.Native.ImGui.ImFontConfigPtr.RasterizerMultiply { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12900) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2e5f3ed83ca312fc"></a>[`public ref float Inno.Native.ImGui.ImFontConfigPtr.SizePixels { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12891) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b29144a210ca8447"></a>[`public struct Inno.Native.ImGui.ImFontConfigPtr : System.IEquatable<Inno.Native.ImGui.ImFontConfigPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12861) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontFlags</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontFlags 54598648af3768231609d7ab70544492da20d8b4ce5bf8b6e99a8d4332f83b1b -->
<!-- api: F:Inno.Native.ImGui.ImFontFlags.None aa2af6644aa8b4cd7a34ec9bd89a67108639b91bcbbbbb316e279be815274906 -->
<!-- api: F:Inno.Native.ImGui.ImFontFlags.NoLoadError 8d1c5cf5d7c959997af2ced45d0adc5ee265d66c184f5ebfb47bb1bbe783f8f8 -->
<!-- api: F:Inno.Native.ImGui.ImFontFlags.NoLoadGlyphs 2cb8514cc87d36879db54c81e51d9e52e0399b280efb8de7e984a670a7f571c0 -->
<!-- api: F:Inno.Native.ImGui.ImFontFlags.LockBakedSizes 17fbef4eaee5ebe2b2b391e6d7ab6b6a08f1ba3ee1ba6bc1302f228a028c1573 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9b7c644d2816048e"></a>[`Inno.Native.ImGui.ImFontFlags.LockBakedSizes = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1163) | 当前常量或枚举值：`8`。 | — |
| <a id="api-2b0167e30546066c"></a>[`Inno.Native.ImGui.ImFontFlags.NoLoadError = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1161) | 当前常量或枚举值：`2`。 | — |
| <a id="api-b059ef7f97762d98"></a>[`Inno.Native.ImGui.ImFontFlags.NoLoadGlyphs = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1162) | 当前常量或枚举值：`4`。 | — |
| <a id="api-8272abf84b30e3ae"></a>[`Inno.Native.ImGui.ImFontFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1160) | 当前常量或枚举值：`0`。 | — |
| <a id="api-3cf963794c5dde07"></a>[`public enum Inno.Native.ImGui.ImFontFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1158) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontGlyph</code> · 21 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontGlyph 913c22ecec1c0ee2b408870236b1332f903950ae3c4d2f28ff84fe8a32041f19 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.RawBits0_0 7188cc8e1a58da6cbdf15ff26e543592fc6b662315a5680ff2a0c6d97a5c644b -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.RawBits0_1 8e05cbf5db3cfbd2a3fd8480ad652348d67d27c746d57a6f5ac840cc1e09e110 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.RawBits0_2 40a3779fd2b7d29d162145a4eab43e476cd1ebc04f1a21332af83171203ddcef -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.RawBits0_3 1c62152eda2fcf0e1d8c8251b11f2b098032fa39650ce08e7b417580fda2bd6b -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyph.Colored 08ba8a9dbc741c2d6b9dcfdb67f007cdb38ede9b4031f9e9e4da3cd05432127f -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyph.Visible a2af7f780e4830b700db94686cfcfae574ba6c9ee2d0884b10725609b91ba4db -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyph.SourceIdx c6d765a401960e7f7ce68c090fe81d1b997bfd2f5b67f31ac119a136cf0b1737 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyph.Codepoint bb9f049f37a7267e7f080583a361ad251ab83a6d16b17bf5da9cd20cfe4124a9 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.AdvanceX 3fe8ee80580152bd152938af09d51a8566d558363b2817a0aa3f7d740764dba9 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.X0 83700d5c0fa4a70b219f3ed71c99111e7169922165126f80a76d3f90d2847e53 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.Y0 c8b5e8d34a8bf5688a88c7f34d8cf24fbf271ec97bac54ca18bf762d417bc59a -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.X1 31580d9c0c92da85099900e25f916095dbee8952258692f88de2418a1b25645c -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.Y1 ffa8854969811b4f421948a0deba5991f45cec8ec1e068338fe3a549ed18c777 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.U0 cbe169fd778e0ce80216c92313d8d2c8e45b623ca534212014045a16d5aa67f6 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.V0 6fa4d011a83fdaf4e1ca82f99808678c32938a8066b9d0dae5a33c069e235181 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.U1 ceeca41e605d2a784346fb8f0286ef421322bf43a49dac1489028e8978d3bc6b -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.V1 bc576e043d3653ca9cea0a46be84ff7da055e1d27a5939070c70af08f3933d50 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyph.PackId 36e79c69011dff8870a3eb2ad05b72d020e739b5a85764898370397b0aa05584 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyph.#ctor(System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Int32) 6d90b04297e0df00a7a9a30b68e38aceda328b6521424898b0ff85538c4541a4 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyph.Destroy eb7e6c1e7c427a134f9e20cf344ff8dfd85d885ef479fcdcd401207f55da20e9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a9e6287fe720a977"></a>[`public float Inno.Native.ImGui.ImFontGlyph.AdvanceX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1812) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e76fe05f547912e2"></a>[`public int Inno.Native.ImGui.ImFontGlyph.PackId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1830) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc7fec8c35bf5dcf"></a>[`public byte Inno.Native.ImGui.ImFontGlyph.RawBits0_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1799) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-329ae2c827f8ee7a"></a>[`public byte Inno.Native.ImGui.ImFontGlyph.RawBits0_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1801) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e63db38ff14ae8ed"></a>[`public byte Inno.Native.ImGui.ImFontGlyph.RawBits0_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1803) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cdf7052f9e050b5c"></a>[`public byte Inno.Native.ImGui.ImFontGlyph.RawBits0_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1805) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7f48b3244b71961"></a>[`public float Inno.Native.ImGui.ImFontGlyph.U0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1822) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-37b10ac68ec03c4b"></a>[`public float Inno.Native.ImGui.ImFontGlyph.U1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1826) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc741280795e74fd"></a>[`public float Inno.Native.ImGui.ImFontGlyph.V0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1824) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e9c3a761bf886477"></a>[`public float Inno.Native.ImGui.ImFontGlyph.V1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1828) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e128d0e6f42edf4c"></a>[`public float Inno.Native.ImGui.ImFontGlyph.X0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1814) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35fb168aac94c4f7"></a>[`public float Inno.Native.ImGui.ImFontGlyph.X1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1818) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-38669eb8bff6325a"></a>[`public float Inno.Native.ImGui.ImFontGlyph.Y0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1816) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-adf9b0f054122c32"></a>[`public float Inno.Native.ImGui.ImFontGlyph.Y1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1820) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-32d6ce4b14d48f43"></a>[`public Inno.Native.ImGui.ImFontGlyph.ImFontGlyph(uint colored = 0, uint visible = 0, uint sourceIdx = 0, uint codepoint = 0, float advanceX = 0, float x0 = 0, float y0 = 0, float x1 = 0, float y1 = 0, float u0 = 0, float v0 = 0, float u1 = 0, float v1 = 0, int packId = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1831) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-433e83b5d8c6cb74"></a>[`public void Inno.Native.ImGui.ImFontGlyph.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65298) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-83c27d408eade71b"></a>[`public uint Inno.Native.ImGui.ImFontGlyph.Codepoint { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1809) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f611044551fb1fd2"></a>[`public uint Inno.Native.ImGui.ImFontGlyph.Colored { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1806) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dff84c83cf47eddd"></a>[`public uint Inno.Native.ImGui.ImFontGlyph.SourceIdx { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1808) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4af75ec4fa119cd2"></a>[`public uint Inno.Native.ImGui.ImFontGlyph.Visible { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1807) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3bb452e355f15a64"></a>[`public struct Inno.Native.ImGui.ImFontGlyph`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1796) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontGlyphPtr</code> · 28 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontGlyphPtr fb899606a9f2c89ab8f8ad0562c8cdfcba14ada4568db550775611c9fb5830c4 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphPtr.#ctor(Inno.Native.ImGui.ImFontGlyph*) f34030683927e5eaa2374d754a642db08276d124732cd824a6559ec5969a8f57 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyphPtr.Handle 2de056f0151569c68d7f48f14e60029565e35baab94a84248aa2357036e50daf -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.IsNull 77a041b964d1b81e85e582c0dd9fb899d3a66c2be8a891e174f14b214e22fc17 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.Null 6cf251aa0bcad20272bdbffc49afa8cb154f2e9e2d4e5225e4a0f82b01d2eaa6 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.Item(System.Int32) c6dd8529f6a26d102135ad69fbd92fbfb931a4c52b0009cd89e7ddabad6fce8f -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphPtr.op_Implicit(Inno.Native.ImGui.ImFontGlyph*)~Inno.Native.ImGui.ImFontGlyphPtr d1c2a19c151e663da33ef3a7de16195d82c966b291a93e8e0b46f8f52a671d50 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphPtr.op_Implicit(Inno.Native.ImGui.ImFontGlyphPtr)~Inno.Native.ImGui.ImFontGlyph* 53edbf452b16a2c1ab94dc72ec0023324f5b9df72281c87cc0361578267b31be -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphPtr.op_Equality(Inno.Native.ImGui.ImFontGlyphPtr,Inno.Native.ImGui.ImFontGlyphPtr) e5e3ed3be10daefbd9bae11e1adf4294da3330fcf00d29076e13855e2880246b -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphPtr.op_Inequality(Inno.Native.ImGui.ImFontGlyphPtr,Inno.Native.ImGui.ImFontGlyphPtr) f51ae36bc5658cd66cb7f5eb9ad8fbd3c69dbe65afa92f102b7d8856d8724081 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphPtr.Equals(Inno.Native.ImGui.ImFontGlyphPtr) 0be06a19b00c868a7565dfb2893403cba0a726be0ea2767932ab0c488f6029a3 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphPtr.Equals(System.Object) 15902beec89d626cb2cda66d55838f22acc59d6bb061cb7b1e865dc5d28db891 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphPtr.GetHashCode 079591633e38f9162371fa01828a2096cf48d72818022cfc2aa0bb4edeb26529 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.Colored 5d0861ffcdd977099d1ffc16c27d1b9e80c8ca79cd9e2cb6651c787a4e23a228 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.Visible 11917c6628b93b4561703320235c77deafc7e6eb655611742c6a77cd781db210 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.SourceIdx dc4e0d74089c8230ba8d1b371c45155fbf3a8b502cda5e99bec8af1685029990 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.Codepoint 8b2e190e5b94b95e4923615a879a4b116196e04f2383773fa54b6613da7acd8f -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.AdvanceX 157482a6ce9c70db88687c44563d36b10f6a2e93e211c85808fa2be6a1d87d5a -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.X0 2b4171652b045e9a30fd41b1a6dfd113b2136903a0444fab207fef32f8135d00 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.Y0 bfdedd4f4a4fe4a6287549e6ad39b37dde6c753a4bb7337649f2f17e6bf038c6 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.X1 400ff6809e44281bc975bcee26c641de0647d291b0e4c689af347ad37767a531 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.Y1 cf4ee3798269a5a0ef7d6433180a83ea57a34a3248a667117d4e63004d9cd55a -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.U0 d1a7868c8130c5d806590ff5965f214bb1283cc256031446bc5fda811a7b5dff -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.V0 f0293569a583aaf085acac76bfc8bf88350b251ccefa207b1dec448e12aae369 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.U1 3082b2a082c4cb45aba68a3190766da8334d8de9a6dccdbb9bbd7c3663170b28 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.V1 81845dfe04a1ad98fb123cdda58e5c9dd1f158bd7ff91556fdbb2839fe6eb812 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphPtr.PackId 3ba1a5a559c8a56032a60e192d95418613b6084d0af8c8819d9ad68b54de1f59 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphPtr.Destroy 043ffa19be993de7ccab9c209c6e4edf783cddff38cda7d220bff8c44fd8e4c8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d439e7865ad96d90"></a>[`public Inno.Native.ImGui.ImFontGlyph* Inno.Native.ImGui.ImFontGlyphPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12925) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b3903c4fdcfc01ab"></a>[`public Inno.Native.ImGui.ImFontGlyphPtr.ImFontGlyphPtr(Inno.Native.ImGui.ImFontGlyph* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12920) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3eb67a21c595ce5f"></a>[`public void Inno.Native.ImGui.ImFontGlyphPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12957) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-08e5a25c25c0b909"></a>[`public bool Inno.Native.ImGui.ImFontGlyphPtr.Equals(Inno.Native.ImGui.ImFontGlyphPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12935) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-f08cd0ecde580d3c"></a>[`public override bool Inno.Native.ImGui.ImFontGlyphPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12936) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-9d1269fa4ddfe167"></a>[`public override int Inno.Native.ImGui.ImFontGlyphPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12937) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-63b4e5cd0d21a503"></a>[`public static bool Inno.Native.ImGui.ImFontGlyphPtr.operator ==(Inno.Native.ImGui.ImFontGlyphPtr left, Inno.Native.ImGui.ImFontGlyphPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12933) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d3343dbc3bef6af4"></a>[`public static Inno.Native.ImGui.ImFontGlyphPtr.implicit operator Inno.Native.ImGui.ImFontGlyphPtr(Inno.Native.ImGui.ImFontGlyph* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12931) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a46cd820e1b1d043"></a>[`public static Inno.Native.ImGui.ImFontGlyphPtr.implicit operator Inno.Native.ImGui.ImFontGlyph*(Inno.Native.ImGui.ImFontGlyphPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12932) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-69c0080576f920cb"></a>[`public static bool Inno.Native.ImGui.ImFontGlyphPtr.operator !=(Inno.Native.ImGui.ImFontGlyphPtr left, Inno.Native.ImGui.ImFontGlyphPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12934) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a9673fa74d0498a9"></a>[`public ref float Inno.Native.ImGui.ImFontGlyphPtr.AdvanceX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12943) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-23caf5fd23364b09"></a>[`public uint Inno.Native.ImGui.ImFontGlyphPtr.Codepoint { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12942) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-99abb23e6fabe7d6"></a>[`public uint Inno.Native.ImGui.ImFontGlyphPtr.Colored { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12939) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-324459ec1bb71478"></a>[`public bool Inno.Native.ImGui.ImFontGlyphPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12926) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-41cdf747e7fe51be"></a>[`public Inno.Native.ImGui.ImFontGlyph Inno.Native.ImGui.ImFontGlyphPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12929) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d3c18ac4e41d63f1"></a>[`public static Inno.Native.ImGui.ImFontGlyphPtr Inno.Native.ImGui.ImFontGlyphPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12927) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b0206445d8b5ed47"></a>[`public ref int Inno.Native.ImGui.ImFontGlyphPtr.PackId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12952) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba96cc9b3a0b8e6c"></a>[`public uint Inno.Native.ImGui.ImFontGlyphPtr.SourceIdx { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12941) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-49d14ff2f026d7f0"></a>[`public ref float Inno.Native.ImGui.ImFontGlyphPtr.U0 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12948) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-478904b54170e35d"></a>[`public ref float Inno.Native.ImGui.ImFontGlyphPtr.U1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12950) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-778961b060d0fd7e"></a>[`public ref float Inno.Native.ImGui.ImFontGlyphPtr.V0 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12949) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1100e33dfa8b4174"></a>[`public ref float Inno.Native.ImGui.ImFontGlyphPtr.V1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12951) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b189911a0a9da596"></a>[`public uint Inno.Native.ImGui.ImFontGlyphPtr.Visible { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12940) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3074a83335e966c0"></a>[`public ref float Inno.Native.ImGui.ImFontGlyphPtr.X0 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12944) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a1003fc457ca923d"></a>[`public ref float Inno.Native.ImGui.ImFontGlyphPtr.X1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12946) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7abb4b45c91f53b2"></a>[`public ref float Inno.Native.ImGui.ImFontGlyphPtr.Y0 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12945) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a9b450b70cfb18dc"></a>[`public ref float Inno.Native.ImGui.ImFontGlyphPtr.Y1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12947) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a02486a2cd8a31a1"></a>[`public struct Inno.Native.ImGui.ImFontGlyphPtr : System.IEquatable<Inno.Native.ImGui.ImFontGlyphPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12918) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontGlyphRangesBuilder</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontGlyphRangesBuilder 2167ab80df8a0274e19377aff72029f4fc057d4af3ba836c370d66ed656b3931 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyphRangesBuilder.UsedChars 0f4b5e72e3c9d4cba6977d1f12074f75903b8e4252cef120524d814ccc43774c -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilder.#ctor(Inno.Native.ImGui.ImVector{System.UInt32}) 78260c93b31c5c1fbd244c7cd091b42b168edee92e6f579d683d36b37a39ff41 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilder.Destroy a9f8309d4ffe99a11f057e225b144132b2ada763679e3f11bfba3f94278eda6e -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilder.Clear be39945780d62756fa2e266ea8c23898f5e70644101a226f64aa67196adbe1e2 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilder.GetBit(System.UIntPtr) 7d63555befa7979badde0d989d7546938b2be1328ad3b46894d93ca3b9655fc3 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilder.SetBit(System.UIntPtr) f35ec580616400198ad23715e32cb194c44706be4f77a91be6de1b0e5c2b7ea0 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilder.AddChar(System.UInt16) c9f2bdf98fbe84b1c9e3167df990964841f240a3d0c8851e9e7458702ba4ee11 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilder.AddText(System.Byte*,System.Byte*) 4666f5184962135766cb205da4b3255c2a5b59d91ff7d5cc43944027dad47a7e -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilder.AddRanges(System.UInt16*) d5cd6cfcd28bd5001db14e16e8c6a006a5c1c811aef2b6b89b58d1589e215b99 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilder.BuildRanges(Inno.Native.ImGui.ImVector{System.UInt32}*) d5c5f8e9f250204497378df3432aaa5a4a76d47df891442d22e58d5d60b530bf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fe3b427f212bb41a"></a>[`public Inno.Native.ImGui.ImVector<uint> Inno.Native.ImGui.ImFontGlyphRangesBuilder.UsedChars`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9473) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fcb6ffad24325071"></a>[`public Inno.Native.ImGui.ImFontGlyphRangesBuilder.ImFontGlyphRangesBuilder(Inno.Native.ImGui.ImVector<uint> usedChars = default(Inno.Native.ImGui.ImVector<uint>))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9474) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2e3808baedaf0fef"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilder.AddChar(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65356) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f6b15ea6fe8edd2b"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilder.AddRanges(ushort* ranges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65378) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b2a5b7ed9217883c"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilder.AddText(byte* text, byte* textEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65367) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b55338cda6ae2a05"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilder.BuildRanges(Inno.Native.ImGui.ImVector<uint>* outRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65389) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6f809fd300705a03"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilder.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65323) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-db999d2498b7648d"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilder.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65312) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-14f4d4d1e29c874c"></a>[`public byte Inno.Native.ImGui.ImFontGlyphRangesBuilder.GetBit(nuint n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65334) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e1ce5c65e39e114a"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilder.SetBit(nuint n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65345) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8412bac7b47a34b5"></a>[`public struct Inno.Native.ImGui.ImFontGlyphRangesBuilder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9471) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr</code> · 23 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr a017dd8520613518b6aa4eb95c47e6d37c0ceb27b96847c71fbd56e1e4d4d529 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.#ctor(Inno.Native.ImGui.ImFontGlyphRangesBuilder*) 2cf4bbcd7c2e3fce947ed7ad1e03b6fad6cf2a83e9cd1430a4341012212f6931 -->
<!-- api: F:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.Handle 6edb0198f307b8087b8a9e160318c67abaf2c66c868fd03d900c09885e6fcafa -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.IsNull a321617edf2fa141c6143ea7cf3c64aae64d40aba97ed96f25f63de0991af878 -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.Null a7bf6df19b9c8f1f5de0622127814fa14bee8a1d644df1ce5a88d17d5209e77b -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.Item(System.Int32) e2426a8a027658f92d28e78a21c6d6b703674c7a4540caab3df1ef50513a56dd -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.op_Implicit(Inno.Native.ImGui.ImFontGlyphRangesBuilder*)~Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr 71446c4902bb64e52a39e03bda49ec6b3b7984f0752b50ea63440109b240588a -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.op_Implicit(Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr)~Inno.Native.ImGui.ImFontGlyphRangesBuilder* 8a28173a7f215035849818aace517e6901de40efb9bd735cbb9b9c824aacd448 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.op_Equality(Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr,Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr) 46db1b05f11295d590e347e40ad7b2e03eee18b9e9af0ba06b2e3ad7213b9527 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.op_Inequality(Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr,Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr) db52503008996d5ac3ad525b7bdb5919ebc517c7b1b69b02ffd4cc78191262b0 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.Equals(Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr) 1d9d5c89106529b9f0f8cb283e05f7a6337835f59e837d1a451fcc905835c2bb -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.Equals(System.Object) 1366b16ced46d6b07cf921e5260944f2956c742fa39f9a0d3b217e2d86198661 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.GetHashCode 7be269d33bb115f684a0d1e65f3a44c4b8421de85391ce52fdb4422085d4f5da -->
<!-- api: P:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.UsedChars 686a6f63af75d09512e1c58f4dc0c6d6a3d9b06099f8232ed45f02046f55a3ca -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.Destroy 78b23d01763ecd4bf46bbf47f07f2bab7409d5d0b915be0e7010f7e49d18bc5d -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.Clear 7d291063f288f1c0fa929c6aec6d96c77e114552129baf683707e73524ad636b -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.GetBit(System.UIntPtr) 33a4edc60d30f237a447d3529138fe0bd22416b4c3e500360a4e1d3ad7154d8e -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.SetBit(System.UIntPtr) dfe0d5691ec5b719447671f42636982e6d7d1dd442962c7c7de95bf2cc93617c -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.AddChar(System.UInt16) 2e22cddbc9786757460c5075b4a4cf1f21336de837ecafd754370e47a69bcdd1 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.AddText(System.String,System.String) 2242551f2a3c39748bc951b0e62b7e54915f87b297033a88e6309e31c7bbbfb8 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.AddText(System.String) b3be7ef6fe240a64120de0efab35fd29daadbcf250aaf44052835f7114156c62 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.AddRanges(System.UInt16*) 866656cd719ba26d8ac4022b8d59da0d2aa1c77256d4aaf1b2df78b75007dd12 -->
<!-- api: M:Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.BuildRanges(Inno.Native.ImGui.ImVector{System.UInt32}@) 7d1b415fbc7a332313b4f4612d540f6cb31d1c4ee49d9aac4cb493a81188031a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e061a62381363363"></a>[`public Inno.Native.ImGui.ImFontGlyphRangesBuilder* Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12971) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d86926f1adf84c4"></a>[`public Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.ImFontGlyphRangesBuilderPtr(Inno.Native.ImGui.ImFontGlyphRangesBuilder* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12966) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f245926b56de79de"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.AddChar(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13022) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ba44ccf65be9a7bf"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.AddRanges(ushort* ranges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13046) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ad5fad097cc04c9c"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.AddText(string text, string? textEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13030) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bf96d0e5067a5d04"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.AddText(string text)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13038) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-14e326cdf2bd8925"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.BuildRanges(ref Inno.Native.ImGui.ImVector<uint> outRanges)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13054) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0153d67e8cf7b927"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12998) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4fa76d5101538f01"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12990) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-066f45ddb7a6bdf1"></a>[`public bool Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.Equals(Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12981) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-36dd5babaf5f8689"></a>[`public override bool Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12982) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-c09f50aec0ef04c4"></a>[`public bool Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.GetBit(nuint n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13006) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bbecc0efdd33d6c1"></a>[`public override int Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12983) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-3181769df1b68eb4"></a>[`public static bool Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.operator ==(Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr left, Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12979) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c9f10919b37c27c0"></a>[`public static Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.implicit operator Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr(Inno.Native.ImGui.ImFontGlyphRangesBuilder* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12977) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-55ef4217a2c78e75"></a>[`public static Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.implicit operator Inno.Native.ImGui.ImFontGlyphRangesBuilder*(Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12978) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-85d47e9df97f6cec"></a>[`public static bool Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.operator !=(Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr left, Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12980) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2512115e739d8ebb"></a>[`public void Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.SetBit(nuint n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13014) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d8b5fd01817c04fb"></a>[`public bool Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12972) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f3fecd37db6e5f63"></a>[`public Inno.Native.ImGui.ImFontGlyphRangesBuilder Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12975) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-15da5f305fa63116"></a>[`public static Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12973) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ff29c4bc33839c35"></a>[`public ref Inno.Native.ImGui.ImVector<uint> Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr.UsedChars { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12985) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-137783c78e8c9a92"></a>[`public struct Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr : System.IEquatable<Inno.Native.ImGui.ImFontGlyphRangesBuilderPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L12964) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontLoader</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontLoader d6a071afba4393babd17e389bfe621dd4f314a2729854fedc5bdffae9f79a5b2 -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.Name 1e059d362bcf84544f2c81a0de20b61560d44a00f971a7554e5b9109dc8315aa -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.LoaderInit 890ea7e105a3e30de7c102158d5c122233435e4756d7b4cf91cef4d3544d53b8 -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.LoaderShutdown e9f13dfc74f17741f67eef169889fe8fd663e8d7b11e904936dad1b0f5fdb12a -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontSrcInit 7689742be49c884b07fa6beec8da98205a6079606af028ad8c3baf2720a9c421 -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontSrcDestroy b024ceb8ae9625e26aaf5bccd49687b2f8189057736f88c31c5994e624863a36 -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontSrcContainsGlyph 9f2a8fc4292fa3fc90b762b6bde6af1eeaeeac9318d9fcb393525a007a547607 -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontBakedInit af34fe0a926c4cd3eacf83b7c314d98803d7897abf21a064411b9259447bd26a -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontBakedDestroy 90946ec5a02a66c7d68c676e0cf06539551e9969b5386ef37ce87b474dc7488c -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontBakedLoadGlyph a4ec00b46fbf3b8002fb7802d55ffd5f094512c7df5b09ae6d5592f335af81b0 -->
<!-- api: F:Inno.Native.ImGui.ImFontLoader.FontBakedSrcLoaderDataSize 8ef74e0d42dac41993d022127768265fc60659c6fe844c6fd455e34ad56473fa -->
<!-- api: M:Inno.Native.ImGui.ImFontLoader.#ctor(System.Byte*,,,,,,,,,System.UIntPtr) b2258023cc2ebfc328f9f646ca0a75e19f76214977bbe6206ca9d959b99e5899 -->
<!-- api: M:Inno.Native.ImGui.ImFontLoader.Destroy b057153e0582985bc2b246e03de04465943ee7e13d200dae66ad4b08edc35927 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-24d33c9c6725f980"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, void> Inno.Native.ImGui.ImFontLoader.FontBakedDestroy`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1930) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-02c1d17cd328971b"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, byte> Inno.Native.ImGui.ImFontLoader.FontBakedInit`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1929) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c3e6ec6883ed7aa2"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, ushort, Inno.Native.ImGui.ImFontGlyph*, float*, byte> Inno.Native.ImGui.ImFontLoader.FontBakedLoadGlyph`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1931) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bd31edbbd16150c8"></a>[`public nuint Inno.Native.ImGui.ImFontLoader.FontBakedSrcLoaderDataSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1932) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-365f0bdea6e1882e"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, ushort, byte> Inno.Native.ImGui.ImFontLoader.FontSrcContainsGlyph`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1928) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4003547bc06cdea3"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, void> Inno.Native.ImGui.ImFontLoader.FontSrcDestroy`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1927) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-29c5da3f1f3f6081"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, byte> Inno.Native.ImGui.ImFontLoader.FontSrcInit`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1926) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b737a24b92e15ed7"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, byte> Inno.Native.ImGui.ImFontLoader.LoaderInit`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1924) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03895e95c9cc2f99"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, void> Inno.Native.ImGui.ImFontLoader.LoaderShutdown`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1925) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a3830146dfe4390a"></a>[`public byte* Inno.Native.ImGui.ImFontLoader.Name`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1923) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-565d7def5cf67991"></a>[`public Inno.Native.ImGui.ImFontLoader.ImFontLoader(byte* name = null, delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, byte> loaderInit = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, byte>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, void> loaderShutdown = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, byte> fontSrcInit = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, byte>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, void> fontSrcDestroy = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, ushort, byte> fontSrcContainsGlyph = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, ushort, byte>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, byte> fontBakedInit = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, byte>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, void> fontBakedDestroy = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, ushort, Inno.Native.ImGui.ImFontGlyph*, float*, byte> fontBakedLoadGlyph = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, ushort, Inno.Native.ImGui.ImFontGlyph*, float*, byte>), nuint fontBakedSrcLoaderDataSize = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1933) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fdc9dd2e20d151af"></a>[`public void Inno.Native.ImGui.ImFontLoader.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67670) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bc7ee5a3fa9692ad"></a>[`public struct Inno.Native.ImGui.ImFontLoader`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1921) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontLoaderPtr</code> · 24 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontLoaderPtr c5d46a719745220deb0a8c897b0946ccbdeac756306907f3177b12a7eaf10949 -->
<!-- api: M:Inno.Native.ImGui.ImFontLoaderPtr.#ctor(Inno.Native.ImGui.ImFontLoader*) 5f6c4681418a881f50f2cfa3cf78a83a19d8934c6198f530a68bae170f93059b -->
<!-- api: F:Inno.Native.ImGui.ImFontLoaderPtr.Handle 805d9aa1df09d554c4f2e5f37921aa70c5663caa1f751627f30643da38234851 -->
<!-- api: P:Inno.Native.ImGui.ImFontLoaderPtr.IsNull 217d6501ed11b2227bddd4062cb1a3695472ba0792585bfafeacaa227bbe6666 -->
<!-- api: P:Inno.Native.ImGui.ImFontLoaderPtr.Null 34f89c7f1e01a2de11525ea009e0b37283ffabd9089e82accb7556f4fda69eeb -->
<!-- api: P:Inno.Native.ImGui.ImFontLoaderPtr.Item(System.Int32) 357516cfb098c1059825f0b80235c393d248f7d178aed47df8b8fd184e42f317 -->
<!-- api: M:Inno.Native.ImGui.ImFontLoaderPtr.op_Implicit(Inno.Native.ImGui.ImFontLoader*)~Inno.Native.ImGui.ImFontLoaderPtr 35550911665f72595f57b90c0136dfaf2b6c24b50f1498380b36ff7d5b9e164d -->
<!-- api: M:Inno.Native.ImGui.ImFontLoaderPtr.op_Implicit(Inno.Native.ImGui.ImFontLoaderPtr)~Inno.Native.ImGui.ImFontLoader* 82b0428de9ceac35918a044d2f29c108a98116c4bca0bd306ef2175d032d17e1 -->
<!-- api: M:Inno.Native.ImGui.ImFontLoaderPtr.op_Equality(Inno.Native.ImGui.ImFontLoaderPtr,Inno.Native.ImGui.ImFontLoaderPtr) 7188b0419632f6ad4955cde954bdb1b30a9e062ae276486368846762929fdef9 -->
<!-- api: M:Inno.Native.ImGui.ImFontLoaderPtr.op_Inequality(Inno.Native.ImGui.ImFontLoaderPtr,Inno.Native.ImGui.ImFontLoaderPtr) 203cf181a765f27ac198759ee460b2fb9a8fa9b88eb16eb73e77916aa49b19a9 -->
<!-- api: M:Inno.Native.ImGui.ImFontLoaderPtr.Equals(Inno.Native.ImGui.ImFontLoaderPtr) a0d88424ed5fa16759bb6a7090e7c0b80b0eaf16f98b446a4666a18919c24cab -->
<!-- api: M:Inno.Native.ImGui.ImFontLoaderPtr.Equals(System.Object) 60d150dc4ff182a2712172b2d61adb3e5f05b0e3dd13388c1b4237356cfc4c19 -->
<!-- api: M:Inno.Native.ImGui.ImFontLoaderPtr.GetHashCode fa34d78b97bf2e11a28d91ab39af2e692c52a1b751bfdce10efcb4a9fe724005 -->
<!-- api: P:Inno.Native.ImGui.ImFontLoaderPtr.Name 2b7eea1a2fbe0b3c2e04f101dae87e99e6e70f0051134caf0aeaf73c5f623a00 -->
<!-- api: P:Inno.Native.ImGui.ImFontLoaderPtr.LoaderInit 2758a071cd644d33adf1b6798da2ac55e126c4790b08f2e7bd7d9395140ea854 -->
<!-- api: P:Inno.Native.ImGui.ImFontLoaderPtr.LoaderShutdown 12aa59f8d0fe5ad65ebbda03742401fa0541ae7032bba12f61f3be60c5e1cdf1 -->
<!-- api: P:Inno.Native.ImGui.ImFontLoaderPtr.FontSrcInit 7eee8194e75bb47438bd1c7b3ad6d174ecfdf203b3397d9b197abde0525b89d6 -->
<!-- api: P:Inno.Native.ImGui.ImFontLoaderPtr.FontSrcDestroy 70639b131a36c045af048165677b9e6fa80e03bbf89c1fef210d47a6c7c0ce87 -->
<!-- api: P:Inno.Native.ImGui.ImFontLoaderPtr.FontSrcContainsGlyph de83fe8afdec2441347a8096f352e742bc08ecc63fa2cb25bd7da7cc3943913e -->
<!-- api: P:Inno.Native.ImGui.ImFontLoaderPtr.FontBakedInit a112eac5174caa238bef044850c31f60f11d30954db00dfc79a707e5847b0f20 -->
<!-- api: P:Inno.Native.ImGui.ImFontLoaderPtr.FontBakedDestroy 6c344c556342cd9cec44e55d9351bee052d495854a00894179220c09e53dda7f -->
<!-- api: P:Inno.Native.ImGui.ImFontLoaderPtr.FontBakedLoadGlyph 99dfcdb5843c62a1e7b3fdd89a8de4ddff81ad933b5858e137f441ef6adeafd9 -->
<!-- api: P:Inno.Native.ImGui.ImFontLoaderPtr.FontBakedSrcLoaderDataSize ba1e3bffa930760f917728a396ac4e7c9e2db0a08adf06f22926e8ae8a34e9f4 -->
<!-- api: M:Inno.Native.ImGui.ImFontLoaderPtr.Destroy 53f2686f6587fa2343638844ba0325f9be2a81fe19f5aae9d2e69c28f077a478 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-68c8a58e3ed39c6b"></a>[`public Inno.Native.ImGui.ImFontLoader* Inno.Native.ImGui.ImFontLoaderPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13068) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5758b0c032dd1bef"></a>[`public Inno.Native.ImGui.ImFontLoaderPtr.ImFontLoaderPtr(Inno.Native.ImGui.ImFontLoader* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13063) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-027dc4ef73abec85"></a>[`public void Inno.Native.ImGui.ImFontLoaderPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13096) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d37d06bef1c5ff8e"></a>[`public bool Inno.Native.ImGui.ImFontLoaderPtr.Equals(Inno.Native.ImGui.ImFontLoaderPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13078) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-12fab288afe61b96"></a>[`public override bool Inno.Native.ImGui.ImFontLoaderPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13079) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-04815337c7df19d9"></a>[`public override int Inno.Native.ImGui.ImFontLoaderPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13080) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-946b5c0134f6fae8"></a>[`public static bool Inno.Native.ImGui.ImFontLoaderPtr.operator ==(Inno.Native.ImGui.ImFontLoaderPtr left, Inno.Native.ImGui.ImFontLoaderPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13076) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e9abffa616c34691"></a>[`public static Inno.Native.ImGui.ImFontLoaderPtr.implicit operator Inno.Native.ImGui.ImFontLoaderPtr(Inno.Native.ImGui.ImFontLoader* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13074) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c3ec8b6f40361613"></a>[`public static Inno.Native.ImGui.ImFontLoaderPtr.implicit operator Inno.Native.ImGui.ImFontLoader*(Inno.Native.ImGui.ImFontLoaderPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13075) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-af718123e984d2fb"></a>[`public static bool Inno.Native.ImGui.ImFontLoaderPtr.operator !=(Inno.Native.ImGui.ImFontLoaderPtr left, Inno.Native.ImGui.ImFontLoaderPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13077) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-318455d84b82e0ff"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, void> Inno.Native.ImGui.ImFontLoaderPtr.FontBakedDestroy { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13089) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a0bc8a46888035d0"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, byte> Inno.Native.ImGui.ImFontLoaderPtr.FontBakedInit { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13088) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ad15d813f2187cd2"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, Inno.Native.ImGui.ImFontBaked*, void*, ushort, Inno.Native.ImGui.ImFontGlyph*, float*, byte> Inno.Native.ImGui.ImFontLoaderPtr.FontBakedLoadGlyph { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13090) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-52c78f6dbbe41d2c"></a>[`public ref nuint Inno.Native.ImGui.ImFontLoaderPtr.FontBakedSrcLoaderDataSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13091) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5c24908d93dab4a7"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, ushort, byte> Inno.Native.ImGui.ImFontLoaderPtr.FontSrcContainsGlyph { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13087) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ea2d4638ac02d716"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, void> Inno.Native.ImGui.ImFontLoaderPtr.FontSrcDestroy { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13086) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c0405cc59b8ea682"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, Inno.Native.ImGui.ImFontConfig*, byte> Inno.Native.ImGui.ImFontLoaderPtr.FontSrcInit { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13085) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-46572da81994733a"></a>[`public bool Inno.Native.ImGui.ImFontLoaderPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13069) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-22fb8c8bcfc66955"></a>[`public Inno.Native.ImGui.ImFontLoader Inno.Native.ImGui.ImFontLoaderPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13072) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-228ab6e5c706a478"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, byte> Inno.Native.ImGui.ImFontLoaderPtr.LoaderInit { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13083) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-17b35a94817d3044"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImFontAtlas*, void> Inno.Native.ImGui.ImFontLoaderPtr.LoaderShutdown { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13084) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fd1f662e98932c03"></a>[`public byte* Inno.Native.ImGui.ImFontLoaderPtr.Name { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13082) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9c1c0c2ae19bf689"></a>[`public static Inno.Native.ImGui.ImFontLoaderPtr Inno.Native.ImGui.ImFontLoaderPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13070) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e0be49bc1e1bce0b"></a>[`public struct Inno.Native.ImGui.ImFontLoaderPtr : System.IEquatable<Inno.Native.ImGui.ImFontLoaderPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13061) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontPtr</code> · 45 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontPtr 81a308f656a0135999c2fbbcd7f164c191531880acbd7ba12d1f764ab726d9e1 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.#ctor(Inno.Native.ImGui.ImFont*) c8882dd81c72d017363436c0dbc09329fc8d6036dd33b15da1c5809c38eb5565 -->
<!-- api: F:Inno.Native.ImGui.ImFontPtr.Handle eaac7eda99ebfc97ddd3daff8ec52c4beca14bf47047c56995ead7fe24bdf94f -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.IsNull 0ee771205b4da37c88cf600321323c0e9732f2e581019998619d292ee4c38f7c -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.Null 1760a6b78dcd2807030381f024b55ac9bfde6910953997d3675165005605ce2f -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.Item(System.Int32) b3825979d86a8c009f4dc85abdee5be60f0af847523a9dd88caef6dd55b3d627 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.op_Implicit(Inno.Native.ImGui.ImFont*)~Inno.Native.ImGui.ImFontPtr eea926b57afda8420ea53cb15f329e34fcc1ae71a9386dfb1df565f874de3366 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.op_Implicit(Inno.Native.ImGui.ImFontPtr)~Inno.Native.ImGui.ImFont* 588b68ff30c211e52a7da92d4b7a2b9cbcaaa7da8a155c1596b4d4ce6a934213 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.op_Equality(Inno.Native.ImGui.ImFontPtr,Inno.Native.ImGui.ImFontPtr) 3d06798c4e348fb22cd498ee4007424b018572fa3e35f6f0e13a2d61c06b2222 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.op_Inequality(Inno.Native.ImGui.ImFontPtr,Inno.Native.ImGui.ImFontPtr) 74dafbe7d05856c39f524fd93d5376ca8ebcca24a0f7849432cf20410ecaed64 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.Equals(Inno.Native.ImGui.ImFontPtr) ec6fffa68b1701a4b770a0b1f4a2487f5bbde1b831e95124f5d090255ad69ca8 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.Equals(System.Object) 95515c446be61ab89214874f8d9757d7464a14c40e9c7c79b230cfd84c73d7ab -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.GetHashCode ea443e52ca399a80e8b5ba74fb7ddfc291ce38038d379f8ff2c4460de182653c -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.LastBaked 603377308e36300581ee563b0e5e3a05c6ac068f982917d33f2ec0c7a9a61f0d -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.OwnerAtlas f0e3f5182e2485b7c15e6acf24cbad6cd1ea053cc259a7602a6f4e950450d9d4 -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.Flags 32c270e03da94a7a3103af37cd0d097b30ab154ef489d291929999306cf82342 -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.CurrentRasterizerDensity 680dce8307c2040898b4c2342062557f332302d5898a8c1c76df6a52d31e9ee5 -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.FontId 14566d84e8a12dc8e9ce42c93ecf3d9eba88cf7e9d900b56fa2e54ee3bd066ca -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.LegacySize f28983793ac75ff670ecff174d7e646b389586d171d54ddaaed66f048a1d0ada -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.Sources d0f73de3dd263d284a578982525cfd022df9b59ee09ccde4482559ba552fa3ad -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.EllipsisChar adf4b0f7a22fedab74474e30ce9312fa6656f906512205a3040efe5755b45436 -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.FallbackChar 0e98021ffcaa6d60b6dd278281da3ca8349757ecc9dbcd9cba54de24b4b22412 -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.Used8kPagesMap 7ec1e4deb04a0a9eaddf7a20caafe34cf5b885436028c5ce340fa3ce917509fe -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.EllipsisAutoBake 2f7909adb069be3a30f9ab8f274d2e7d9d9cc3f362eacb94fc6809eb2d7618e5 -->
<!-- api: P:Inno.Native.ImGui.ImFontPtr.RemapPairs abf14b29995823cecd634beb1c31d8a63abf4a82079801c336c05373d927a6f4 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.Destroy 5baaad1f58d18d843559966dc62d6f1bd093f0bdc72fac79df082287602a07c7 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.IsGlyphInFont(System.UInt16) 1953cefbc0c0680923177628aebdaaeb8f5e79f8776b9fe5298db6607190b7f1 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.IsLoaded 14d7b43d4fedff78756b91d75a80aaf23697260ccb28aec658dfc0ace0b56999 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.GetDebugName 6cf7275b88feca3fc83b54724bc4f1324d485bc0ac313089a80590f4203618f0 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.GetFontBaked(System.Single,System.Single) 49bf94f8fc4fa3a3ce993ae84fb425c64326ab743fbee0652c909716f189f4c4 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.GetFontBaked(System.Single) 22aca1b9ce347538f571c1ba421d14f2f44c532655b822ab8bf2a6a032a5bd0c -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.CalcTextSizeA(System.Single,System.Single,System.Single,System.String,System.String,System.Byte**) 1519297349a8a64847ed1f2c9483d8fd19dd75a7ed023cb611e807e9b926652a -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.CalcTextSizeA(System.Single,System.Single,System.Single,System.String) 84de0d19aa9951153ebd5d97a113cc886576b67bd8303a18e2a5a6d49db2545a -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.CalcTextSizeA(System.Single,System.Single,System.Single,System.String,System.String) 4b8394f971edcd53b61264c60f7d9924686b046a42fa795cabe14b3cd69ca02b -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.CalcTextSizeA(System.Single,System.Single,System.Single,System.String,System.Byte**) 393bcf5fb49d3d9694d335b159b99d7397baba4ee6e73e351b0cc5def4136f27 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.CalcWordWrapPosition(System.Single,System.String,System.String,System.Single) c989948e5f6eefb56924241186ecd76710a48cc8174f6817cbb373d70ad6896f -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.RenderChar(Inno.Native.ImGui.ImDrawListPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.UInt16,System.Numerics.Vector4*) ddddb25fd683401f549c1887eceaeb394dc061dde55f76132ece787a0d9f7920 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.RenderChar(Inno.Native.ImGui.ImDrawListPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.UInt16) aefc086986299e7ab344aeef53c46a3a2d343a2ffd7eacc4c0a634ed0d049799 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.RenderText(Inno.Native.ImGui.ImDrawListPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.Numerics.Vector4,System.String,System.String,System.Single,Inno.Native.ImGui.ImDrawTextFlags) b6e652f49c4fa35e304a0ecf32cca42ef2ff2f95ba868838ce85398fbbdba56f -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.RenderText(Inno.Native.ImGui.ImDrawListPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.Numerics.Vector4,System.String,System.String) 9332370f191a06352330bc8a6287f862cb80490ff317b202ec67dd8a221bd23d -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.RenderText(Inno.Native.ImGui.ImDrawListPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.Numerics.Vector4,System.String,System.String,System.Single) 2fcd7859336a82694e660bd3decd8bdeb0407cc5fff8e70f3412339ba03f1526 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.RenderText(Inno.Native.ImGui.ImDrawListPtr,System.Single,System.Numerics.Vector2,System.UInt32,System.Numerics.Vector4,System.String,System.String,Inno.Native.ImGui.ImDrawTextFlags) c901a1b62ed483c17e00255feb56b0715d74df1d29c6f04b2d80cb628c85fd8a -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.ClearOutputData b1805e68f0900e7dae8e0a37f91330ce9daeb9b8b4a6a00b024cc088369b881c -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.AddRemapChar(System.UInt16,System.UInt16) eb28f09a2cf9b26950006eee53df8748d11213cf42986c965173e930769323d2 -->
<!-- api: M:Inno.Native.ImGui.ImFontPtr.IsGlyphRangeUnused(System.UInt32,System.UInt32) eff392caa1b73546064777d6f9caa2d17a58fc4c03696d1c0996f1a25a09c303 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-75d9ae2b29a62c47"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13110) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-352577b8b9acde53"></a>[`public Inno.Native.ImGui.ImFontPtr.ImFontPtr(Inno.Native.ImGui.ImFont* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13105) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-45c41b0e2157cb1e"></a>[`public void Inno.Native.ImGui.ImFontPtr.AddRemapChar(ushort fromCodepoint, ushort toCodepoint)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13284) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dc6002d886f80948"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImFontPtr.CalcTextSizeA(float size, float maxWidth, float wrapWidth, string textBegin, byte** outRemaining)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13212) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-499970bdf8b61fb1"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImFontPtr.CalcTextSizeA(float size, float maxWidth, float wrapWidth, string textBegin, string? textEnd, byte** outRemaining)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13188) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b5b366e7f389e65c"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImFontPtr.CalcTextSizeA(float size, float maxWidth, float wrapWidth, string textBegin, string? textEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13204) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-965638f684c20fc1"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImFontPtr.CalcTextSizeA(float size, float maxWidth, float wrapWidth, string textBegin)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13196) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8cc822ba54a34e00"></a>[`public string? Inno.Native.ImGui.ImFontPtr.CalcWordWrapPosition(float size, string text, string textEnd, float wrapWidth)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13220) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8e90994b8e00b3e6"></a>[`public void Inno.Native.ImGui.ImFontPtr.ClearOutputData()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13276) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f8883b2d3db4e919"></a>[`public void Inno.Native.ImGui.ImFontPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13140) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a2abbd381ff2f294"></a>[`public bool Inno.Native.ImGui.ImFontPtr.Equals(Inno.Native.ImGui.ImFontPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13120) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-1e1004547b901d75"></a>[`public override bool Inno.Native.ImGui.ImFontPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13121) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-a111aecbef43be59"></a>[`public string? Inno.Native.ImGui.ImFontPtr.GetDebugName()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13164) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ece34641ce8bb205"></a>[`public Inno.Native.ImGui.ImFontBakedPtr Inno.Native.ImGui.ImFontPtr.GetFontBaked(float fontSize, float density)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13172) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3a33400c94f79469"></a>[`public Inno.Native.ImGui.ImFontBakedPtr Inno.Native.ImGui.ImFontPtr.GetFontBaked(float fontSize)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13180) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-691f6fbcfc951f9d"></a>[`public override int Inno.Native.ImGui.ImFontPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13122) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-21b9520d3d04c287"></a>[`public bool Inno.Native.ImGui.ImFontPtr.IsGlyphInFont(ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13148) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-30f3510e84db9495"></a>[`public bool Inno.Native.ImGui.ImFontPtr.IsGlyphRangeUnused(uint cBegin, uint cLast)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13292) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e58f3867ceae7211"></a>[`public bool Inno.Native.ImGui.ImFontPtr.IsLoaded()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13156) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-04435684c726b5c6"></a>[`public static bool Inno.Native.ImGui.ImFontPtr.operator ==(Inno.Native.ImGui.ImFontPtr left, Inno.Native.ImGui.ImFontPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13118) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-687fa4c836e524fa"></a>[`public static Inno.Native.ImGui.ImFontPtr.implicit operator Inno.Native.ImGui.ImFontPtr(Inno.Native.ImGui.ImFont* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13116) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ec71081d757077d"></a>[`public static Inno.Native.ImGui.ImFontPtr.implicit operator Inno.Native.ImGui.ImFont*(Inno.Native.ImGui.ImFontPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13117) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1171231b05756944"></a>[`public static bool Inno.Native.ImGui.ImFontPtr.operator !=(Inno.Native.ImGui.ImFontPtr left, Inno.Native.ImGui.ImFontPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13119) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-00aed0a5470ec2eb"></a>[`public void Inno.Native.ImGui.ImFontPtr.RenderChar(Inno.Native.ImGui.ImDrawListPtr drawList, float size, System.Numerics.Vector2 pos, uint col, ushort c, System.Numerics.Vector4* cpuFineClip)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13228) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7b3b23ab0befbd8d"></a>[`public void Inno.Native.ImGui.ImFontPtr.RenderChar(Inno.Native.ImGui.ImDrawListPtr drawList, float size, System.Numerics.Vector2 pos, uint col, ushort c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13236) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ef959045ec6be57f"></a>[`public void Inno.Native.ImGui.ImFontPtr.RenderText(Inno.Native.ImGui.ImDrawListPtr drawList, float size, System.Numerics.Vector2 pos, uint col, System.Numerics.Vector4 clipRect, string textBegin, string textEnd, Inno.Native.ImGui.ImDrawTextFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13268) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f7d5db89f30ef58d"></a>[`public void Inno.Native.ImGui.ImFontPtr.RenderText(Inno.Native.ImGui.ImDrawListPtr drawList, float size, System.Numerics.Vector2 pos, uint col, System.Numerics.Vector4 clipRect, string textBegin, string textEnd, float wrapWidth, Inno.Native.ImGui.ImDrawTextFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13244) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5ed66f34f1d391fe"></a>[`public void Inno.Native.ImGui.ImFontPtr.RenderText(Inno.Native.ImGui.ImDrawListPtr drawList, float size, System.Numerics.Vector2 pos, uint col, System.Numerics.Vector4 clipRect, string textBegin, string textEnd, float wrapWidth)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13260) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b41f4594bd15f5f1"></a>[`public void Inno.Native.ImGui.ImFontPtr.RenderText(Inno.Native.ImGui.ImDrawListPtr drawList, float size, System.Numerics.Vector2 pos, uint col, System.Numerics.Vector4 clipRect, string textBegin, string textEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13252) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eb68320ab8b14ec0"></a>[`public ref float Inno.Native.ImGui.ImFontPtr.CurrentRasterizerDensity { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13127) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-83b9905fc8c6ca8c"></a>[`public bool Inno.Native.ImGui.ImFontPtr.EllipsisAutoBake { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13134) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6afad1ed4531b020"></a>[`public ref ushort Inno.Native.ImGui.ImFontPtr.EllipsisChar { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13131) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-380a9ea196c8deab"></a>[`public ref ushort Inno.Native.ImGui.ImFontPtr.FallbackChar { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13132) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a481d2621c593971"></a>[`public ref Inno.Native.ImGui.ImFontFlags Inno.Native.ImGui.ImFontPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13126) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a4c2f496734c32fa"></a>[`public ref uint Inno.Native.ImGui.ImFontPtr.FontId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13128) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-42c77b290c5097ad"></a>[`public bool Inno.Native.ImGui.ImFontPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13111) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-976de786fb01a679"></a>[`public Inno.Native.ImGui.ImFont Inno.Native.ImGui.ImFontPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13114) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ec016c46055a01b7"></a>[`public ref Inno.Native.ImGui.ImFontBakedPtr Inno.Native.ImGui.ImFontPtr.LastBaked { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13124) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3ac7c85867e5d06a"></a>[`public ref float Inno.Native.ImGui.ImFontPtr.LegacySize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13129) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5552c9a86a4c3b02"></a>[`public static Inno.Native.ImGui.ImFontPtr Inno.Native.ImGui.ImFontPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13112) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3a97dc126d02af4f"></a>[`public ref Inno.Native.ImGui.ImFontAtlasPtr Inno.Native.ImGui.ImFontPtr.OwnerAtlas { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13125) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8f9e7a1c20834d9e"></a>[`public ref Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImFontPtr.RemapPairs { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13135) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e8215946e60bf4b6"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontConfigPtr> Inno.Native.ImGui.ImFontPtr.Sources { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13130) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dcfcb5905eacf5e3"></a>[`public System.Span<byte> Inno.Native.ImGui.ImFontPtr.Used8kPagesMap { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13133) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cdb9ec7dea772dd3"></a>[`public struct Inno.Native.ImGui.ImFontPtr : System.IEquatable<Inno.Native.ImGui.ImFontPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13103) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImFontStackData</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImFontStackData 94c8114e892e5c964e67af21b04454095e7a681a84afebd250d5a38ed3af918c -->
<!-- api: F:Inno.Native.ImGui.ImFontStackData.Font 5eca0d0f8c67d0aae7fd80edc8832ca31ffb91ba490ac67da7b32dab3c1e9bba -->
<!-- api: F:Inno.Native.ImGui.ImFontStackData.FontSizeBeforeScaling b398baf4b6e6c725ef4e73b781102cf59dcb3124103c1d5d79b3e02951f8b79c -->
<!-- api: F:Inno.Native.ImGui.ImFontStackData.FontSizeAfterScaling e88f4bba7006e2e51e0255475fe032c04328c45c6e70a927ea306c0fb5ca4ac9 -->
<!-- api: M:Inno.Native.ImGui.ImFontStackData.#ctor(Inno.Native.ImGui.ImFont*,System.Single,System.Single) 70b3b0c65be249ad97ccf0c46d26f3ed23b07c44840c746ccd2e8ed5776c1914 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f0f43cb17ac2c087"></a>[`public Inno.Native.ImGui.ImFont* Inno.Native.ImGui.ImFontStackData.Font`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5724) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aeec05dc0e63f10d"></a>[`public float Inno.Native.ImGui.ImFontStackData.FontSizeAfterScaling`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5726) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0da61bca47fb0ba1"></a>[`public float Inno.Native.ImGui.ImFontStackData.FontSizeBeforeScaling`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5725) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed701da9940526a7"></a>[`public Inno.Native.ImGui.ImFontStackData.ImFontStackData(Inno.Native.ImGui.ImFont* font = null, float fontSizeBeforeScaling = 0, float fontSizeAfterScaling = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5727) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-220afb8a85a1ff94"></a>[`public struct Inno.Native.ImGui.ImFontStackData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5722) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
