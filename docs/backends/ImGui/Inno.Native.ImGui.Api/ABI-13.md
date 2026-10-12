# Inno.Native.ImGui ABI · ImGuiDataTypeInfoPtr → ImGuiInputFlags

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImGuiDataTypeInfoPtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDataTypeInfoPtr 39deff2a89e15ff235069e150a3bffae833515f0f00632992809a4d2df58ca9d -->
<!-- api: M:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.#ctor(Inno.Native.ImGui.ImGuiDataTypeInfo*) 31ed7bce179072e9d3e1c1825c930471b568c76e72f5dfc83f9eff1e6d251c8f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.Handle e578b193b1db28e8f2f95e812e4b0d803e5a5e13500ba8173affdfbf55389abd -->
<!-- api: P:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.IsNull 235675459c8c985acaccd7354885777301f6b12b9ac5adc0beb213864a3b3073 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.Null 3d9c89afe1b697553806b89cd98975b1702d6295c7c1cf26fe92cfd8fe1bda9c -->
<!-- api: P:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.Item(System.Int32) 11158a8970bbe122ef2f9d42f50db019d875dffbc10c5b66d6bdd06432eaa32b -->
<!-- api: M:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.op_Implicit(Inno.Native.ImGui.ImGuiDataTypeInfo*)~Inno.Native.ImGui.ImGuiDataTypeInfoPtr 4bf80f6c0b74a8f8fec345788c01d14eab4e76cd9e8e945ac13ba55b769ce5e5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.op_Implicit(Inno.Native.ImGui.ImGuiDataTypeInfoPtr)~Inno.Native.ImGui.ImGuiDataTypeInfo* d9ca9c8b0631508f0f461fb86aa6f1ccaff18a63d996536ea13bf7aef11a00fa -->
<!-- api: M:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.op_Equality(Inno.Native.ImGui.ImGuiDataTypeInfoPtr,Inno.Native.ImGui.ImGuiDataTypeInfoPtr) 7aeddcb894f6e2ff5648db8a6bb5e06f6dd5c6aef782b2f0e0da81bad393df49 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.op_Inequality(Inno.Native.ImGui.ImGuiDataTypeInfoPtr,Inno.Native.ImGui.ImGuiDataTypeInfoPtr) dac28879c557b44fd291f7a44ad62f133a79dd9de361d592b41d3640e83db0f6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.Equals(Inno.Native.ImGui.ImGuiDataTypeInfoPtr) f1c2923eb7b582ea4e24c584e90b3e42d3444878756b6eb4c2bb1a7a5d890c8f -->
<!-- api: M:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.Equals(System.Object) 053518216feb3400b52e8dae84e6315a56bf4f78e531753f3777c473bb582d78 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.GetHashCode dfae9bd1af34aec3818a32e84abbde105696707e2fd02c79ef2a64f5c03dd1b7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.Size 27f7804e8bf4221647ccc0c49e1829cdfe944e98322c23cbabfd711b8d7da5db -->
<!-- api: P:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.Name 7f32f568e193495a41c0496ea5d47aef3c36cf405300e26fcdf7158027f06d6e -->
<!-- api: P:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.PrintFmt 2e3c8bb7aacc5691bfd1470fe2651d0a60efb717288fd803c65c3f0b4de525e3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDataTypeInfoPtr.ScanFmt 3d41fd8471a2be67416a8b39d0a0ee606344f3cea3281cb0619dda5fd80b01f9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c4c70be630e82876"></a>[`public Inno.Native.ImGui.ImGuiDataTypeInfo* Inno.Native.ImGui.ImGuiDataTypeInfoPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13784) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-06a6f5d0f7f573b8"></a>[`public Inno.Native.ImGui.ImGuiDataTypeInfoPtr.ImGuiDataTypeInfoPtr(Inno.Native.ImGui.ImGuiDataTypeInfo* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13779) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d0205ec543180eba"></a>[`public bool Inno.Native.ImGui.ImGuiDataTypeInfoPtr.Equals(Inno.Native.ImGui.ImGuiDataTypeInfoPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13794) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-980919992ac8fba5"></a>[`public override bool Inno.Native.ImGui.ImGuiDataTypeInfoPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13795) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-27e7be597b50f8f5"></a>[`public override int Inno.Native.ImGui.ImGuiDataTypeInfoPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13796) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-fd0fa5dfdb6669bd"></a>[`public static bool Inno.Native.ImGui.ImGuiDataTypeInfoPtr.operator ==(Inno.Native.ImGui.ImGuiDataTypeInfoPtr left, Inno.Native.ImGui.ImGuiDataTypeInfoPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13792) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f774a0c71c1e01dd"></a>[`public static Inno.Native.ImGui.ImGuiDataTypeInfoPtr.implicit operator Inno.Native.ImGui.ImGuiDataTypeInfoPtr(Inno.Native.ImGui.ImGuiDataTypeInfo* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13790) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dd296381cbadef50"></a>[`public static Inno.Native.ImGui.ImGuiDataTypeInfoPtr.implicit operator Inno.Native.ImGui.ImGuiDataTypeInfo*(Inno.Native.ImGui.ImGuiDataTypeInfoPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13791) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d533d0a7330beac3"></a>[`public static bool Inno.Native.ImGui.ImGuiDataTypeInfoPtr.operator !=(Inno.Native.ImGui.ImGuiDataTypeInfoPtr left, Inno.Native.ImGui.ImGuiDataTypeInfoPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13793) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-63d49f1f55a1dbc0"></a>[`public bool Inno.Native.ImGui.ImGuiDataTypeInfoPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13785) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fd3eceada4119171"></a>[`public Inno.Native.ImGui.ImGuiDataTypeInfo Inno.Native.ImGui.ImGuiDataTypeInfoPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13788) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ee53f573481eff80"></a>[`public byte* Inno.Native.ImGui.ImGuiDataTypeInfoPtr.Name { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13799) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-434866158ff75906"></a>[`public static Inno.Native.ImGui.ImGuiDataTypeInfoPtr Inno.Native.ImGui.ImGuiDataTypeInfoPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13786) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ce32032864499dbf"></a>[`public byte* Inno.Native.ImGui.ImGuiDataTypeInfoPtr.PrintFmt { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13800) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4189d0c354b333f5"></a>[`public byte* Inno.Native.ImGui.ImGuiDataTypeInfoPtr.ScanFmt { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13801) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-39861d79207fc5f3"></a>[`public ref nuint Inno.Native.ImGui.ImGuiDataTypeInfoPtr.Size { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13798) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-115df190146c1b89"></a>[`public struct Inno.Native.ImGui.ImGuiDataTypeInfoPtr : System.IEquatable<Inno.Native.ImGui.ImGuiDataTypeInfoPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13777) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDataTypePrivate</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDataTypePrivate 792afe142d864e8bbb84986bbe7bdf2a1769e9c7f21d071130ddf53350cf80a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypePrivate.Pointer 557494ca6601b79d842caff840af11fa5b2ee753783eb5e89540a7c944f71e4b -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypePrivate.Id c1761f22fc273bc8c4427aacd79b6b4255fe86d2ca98d64e801b507133331113 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1a1a5ff567abbcc0"></a>[`Inno.Native.ImGui.ImGuiDataTypePrivate.Id = 13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1215) | 当前常量或枚举值：`13`。 | — |
| <a id="api-eeb98e7079de8f1c"></a>[`Inno.Native.ImGui.ImGuiDataTypePrivate.Pointer = 12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1214) | 当前常量或枚举值：`12`。 | — |
| <a id="api-91955bea6b752508"></a>[`public enum Inno.Native.ImGui.ImGuiDataTypePrivate : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1212) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDataTypeStorage</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDataTypeStorage 657b5abb2d264b594225838aee68d7903ae3dd94c5815266a64a3add63626b6c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeStorage.Data_0 07b94c1bb3cf6b7dd7595af0a1e9fb924608c8e5c0cfe98371828cf6fbdf1207 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeStorage.Data_1 2f59cbaeba650fb476fb5c639ca87e10ff00c088009fa3c1a68d6bef05f50501 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeStorage.Data_2 a42d9500953db89e12b14f15b0c89f700aa42e45b2976a5b30e09e814e5fd4c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeStorage.Data_3 61415ff53769c2dc2a176c76c8f07e781fc409c694e798320f0f3ab5a94c2e03 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeStorage.Data_4 ebc78ab5777e7428618c3e216c18d5314290bdc02a65c31216ef5f2ee68288f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeStorage.Data_5 0a863a5f2c321a86f67d27a5318e2a99c8f50f9ca36a27dac1a23b7836b45152 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeStorage.Data_6 dbf57cf1c466bd95f45cfebe6695383e659af96e2f949eec7b9d738a5b0c81fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiDataTypeStorage.Data_7 a8cdf2afde638fa15d46b80eecaea000a13a4a81688706c335dd4b12386e77ca -->
<!-- api: P:Inno.Native.ImGui.ImGuiDataTypeStorage.Data c40d3c4c9b31c82abf29fa99b8fb60ad1a4fd73847effdbdaef293dbb6769e73 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDataTypeStorage.#ctor(System.Byte*) 8f5db89d75f8dc1496774d697272f3e11b8480a4a4ccc74a89be7a330191de77 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a051d36f470a399a"></a>[`public byte Inno.Native.ImGui.ImGuiDataTypeStorage.Data_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5133) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b6aaf8670140dfe"></a>[`public byte Inno.Native.ImGui.ImGuiDataTypeStorage.Data_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5134) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e168704fee1d3c83"></a>[`public byte Inno.Native.ImGui.ImGuiDataTypeStorage.Data_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5135) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da612711d47a92db"></a>[`public byte Inno.Native.ImGui.ImGuiDataTypeStorage.Data_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5136) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9403dd014ffb8ba4"></a>[`public byte Inno.Native.ImGui.ImGuiDataTypeStorage.Data_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5137) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35dfcce4e4346574"></a>[`public byte Inno.Native.ImGui.ImGuiDataTypeStorage.Data_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5138) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58f44ce450b92e0e"></a>[`public byte Inno.Native.ImGui.ImGuiDataTypeStorage.Data_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5139) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d3f219352c1c8abe"></a>[`public byte Inno.Native.ImGui.ImGuiDataTypeStorage.Data_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5140) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60e7b7f73d6487d5"></a>[`public Inno.Native.ImGui.ImGuiDataTypeStorage.ImGuiDataTypeStorage(byte* data = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5143) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-76c11f7f127b94d7"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiDataTypeStorage.Data { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5141) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-42ef28221d587b54"></a>[`public struct Inno.Native.ImGui.ImGuiDataTypeStorage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5131) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDeactivatedItemData</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDeactivatedItemData f2bf588100001a723dec763efd55365e8fe339cccafa5aaeea895093d86c7e8b -->
<!-- api: F:Inno.Native.ImGui.ImGuiDeactivatedItemData.ID 866d9ca91d424ecd7660e1e2b18f2e31641dd572aac3f8a4f897635df530c1b4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDeactivatedItemData.ElapseFrame baca15f780fae1d22d04e051b06840ca5a1b0b35f8ed355be5950cf217c36c4f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDeactivatedItemData.HasBeenEditedBefore dc5a2647105d339e09f64bead84275a8caf898bb35e1d3cdea17aaf38097430e -->
<!-- api: F:Inno.Native.ImGui.ImGuiDeactivatedItemData.IsAlive 6ce2faee6f3e589494ca734fd6125b1294b2a701e0bc75e4f20815dd6404be11 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDeactivatedItemData.#ctor(System.UInt32,System.Int32,System.Byte,System.Byte) da8580db9600461251341391d16102eea70b81b1c3add7322317fb3aa68885c9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ff8ecf23bc6eae5f"></a>[`public int Inno.Native.ImGui.ImGuiDeactivatedItemData.ElapseFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5115) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e496fb8b3afc70b1"></a>[`public byte Inno.Native.ImGui.ImGuiDeactivatedItemData.HasBeenEditedBefore`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5116) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c18da6c20b968ee"></a>[`public uint Inno.Native.ImGui.ImGuiDeactivatedItemData.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5114) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35d0375be9e657a4"></a>[`public byte Inno.Native.ImGui.ImGuiDeactivatedItemData.IsAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5117) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-888283dbc2f4229c"></a>[`public Inno.Native.ImGui.ImGuiDeactivatedItemData.ImGuiDeactivatedItemData(uint iD = 0, int elapseFrame = 0, byte hasBeenEditedBefore = 0, byte isAlive = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5118) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7cdf3d5c290ffa4c"></a>[`public struct Inno.Native.ImGui.ImGuiDeactivatedItemData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5112) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDebugAllocEntry</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDebugAllocEntry 9754a98399f7195aaebe6224359d2e92cdae8b455fc523488b05b7c7899d4ad0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocEntry.FrameCount 2e1015f6122f8150784c65a856bfbcbe7a9fd715ff480875ef0b20edd184963f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocEntry.AllocCount c9cbdbd461332f623bb508c792838a4a437fadf071a174e8b306b09f9612202c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocEntry.FreeCount 2b2ea4782944e01bcf19006a48cfb4276f73847b6e2eaee6511f0b2be7acae6b -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugAllocEntry.#ctor(System.Int32,System.Int16,System.Int16) 20b3ddbb2640b67b1807c0919ac315faedea564cf8b6309c09acdad42197ca53 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0f8bdd72efee5c5d"></a>[`public short Inno.Native.ImGui.ImGuiDebugAllocEntry.AllocCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7549) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eb7709c7bf465916"></a>[`public int Inno.Native.ImGui.ImGuiDebugAllocEntry.FrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7548) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a71acc2efb2a559f"></a>[`public short Inno.Native.ImGui.ImGuiDebugAllocEntry.FreeCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7550) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a45dcd710e2a9631"></a>[`public Inno.Native.ImGui.ImGuiDebugAllocEntry.ImGuiDebugAllocEntry(int frameCount = 0, short allocCount = 0, short freeCount = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7551) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-609ee6a3ccd35c76"></a>[`public struct Inno.Native.ImGui.ImGuiDebugAllocEntry`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7546) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDebugAllocInfo</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDebugAllocInfo 07da321b8bc5a8f7be5080d86c04b79122932cf4626664e0b5e63d89d588b1c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfo.TotalAllocCount 5310079a116dbb6f660d672aacf7613ec085bab0f6d891d174b3212ca4764d0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfo.TotalFreeCount 6cc94815a570feced0c989ad4b442409e248af38e0b83117a863c74c7ce983f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesIdx 02cfcc2fb30c260a5b3e3950c1a097a5f2c0682b488f469bac9bc0ed990b6c88 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf_0 b085ef8fe16747576c56169f949d557d03ae816356e1451947c69b0bd993fc5d -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf_1 221615432690292ece736d0ac517547fcbe045db45b29b640f1685454258cdb1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf_2 e7b8390e2db0dee7adfe999d44aea403c6cb512da2125ba069f67004afe8f924 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf_3 93b42080e1d0398aba529a906b0acfdf16f1f2e114c671849c22c7fe1c1ffbbc -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf_4 296a17c4af2f49ed874325bd5d60ea16aa6fac5e3ae8017810b14e65478daa91 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf_5 c8a126494afbed6357746b35c6b112b7a998d1fb9bbcd52a495d96a2216c0ad3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf 63440a5f91d4a9c083c3cafeb8a6363ba5319e095ab836ef5656a1603a54e3c9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugAllocInfo.#ctor(System.Int32,System.Int32,System.Int16,Inno.Native.ImGui.ImGuiDebugAllocEntry*) 56c0d612efa6352cd4bed76df9e0e79bc5883336bdec674a6757ea53547dcfc7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugAllocInfo.Destroy cc67c2726e1948669ab14bcbdd44581bfc2f21594cdc47b8ad61e4e8ff6ddf05 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-87bd7285fe33e22a"></a>[`public Inno.Native.ImGui.ImGuiDebugAllocEntry Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7568) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3347d3898c659b77"></a>[`public Inno.Native.ImGui.ImGuiDebugAllocEntry Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7569) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-758e1c3747cec1af"></a>[`public Inno.Native.ImGui.ImGuiDebugAllocEntry Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7570) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f46379975455546a"></a>[`public Inno.Native.ImGui.ImGuiDebugAllocEntry Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7571) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b1bfa27ec91923da"></a>[`public Inno.Native.ImGui.ImGuiDebugAllocEntry Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7572) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a51ad558bc8dc4ea"></a>[`public Inno.Native.ImGui.ImGuiDebugAllocEntry Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7573) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d069bc55598ddec8"></a>[`public short Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7567) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1a0578971080ce47"></a>[`public int Inno.Native.ImGui.ImGuiDebugAllocInfo.TotalAllocCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7565) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1880d7370271332"></a>[`public int Inno.Native.ImGui.ImGuiDebugAllocInfo.TotalFreeCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7566) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c50ca0fac4c970ce"></a>[`public Inno.Native.ImGui.ImGuiDebugAllocInfo.ImGuiDebugAllocInfo(int totalAllocCount = 0, int totalFreeCount = 0, short lastEntriesIdx = 0, Inno.Native.ImGui.ImGuiDebugAllocEntry* lastEntriesBuf = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7576) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-48274d488aea53c1"></a>[`public void Inno.Native.ImGui.ImGuiDebugAllocInfo.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67361) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-12b2a3ae250e2f95"></a>[`public System.Span<Inno.Native.ImGui.ImGuiDebugAllocEntry> Inno.Native.ImGui.ImGuiDebugAllocInfo.LastEntriesBuf { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7574) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bfc0041608595c27"></a>[`public struct Inno.Native.ImGui.ImGuiDebugAllocInfo`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7563) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDebugAllocInfoPtr</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr 0054f452f1ad991e71161494f623aa91ad37f046de393ced87c6e75cd40c24be -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.#ctor(Inno.Native.ImGui.ImGuiDebugAllocInfo*) 2dd4aee5e22866335eb9bd356d36b7436e5ddc213520f4e5ea3af08f4189846d -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.Handle 919fd9b8e273cf821fc3ee884949e31ad30fe65576cfb9a451ad21ec1ef3d6bd -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.IsNull 2932986b8abcee1fa3214ea0bb7720293ab43289607a1d6503b22bbea2f8a8cd -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.Null e4f4cd0eae0d58674e4b925157699a384cb549dd4db4695a1fcd90b774ccfc6b -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.Item(System.Int32) c6c3e5bfdf991d7d0dec8e6e38aaa46811608cfd4d6112e3883ede2c54b29f12 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.op_Implicit(Inno.Native.ImGui.ImGuiDebugAllocInfo*)~Inno.Native.ImGui.ImGuiDebugAllocInfoPtr 4b73d75ae6730a05d12fbc9449435a0e6b769e65b99a9dc143770d8b84027336 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.op_Implicit(Inno.Native.ImGui.ImGuiDebugAllocInfoPtr)~Inno.Native.ImGui.ImGuiDebugAllocInfo* 31a347b6717a406cac42830703ce7bff7596f55ade0a65a29474c356a9a3f325 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.op_Equality(Inno.Native.ImGui.ImGuiDebugAllocInfoPtr,Inno.Native.ImGui.ImGuiDebugAllocInfoPtr) 8a8125051296b4e03ce735d47b8ee9a34e8b07aff8dcb9ba92582385f04947c7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.op_Inequality(Inno.Native.ImGui.ImGuiDebugAllocInfoPtr,Inno.Native.ImGui.ImGuiDebugAllocInfoPtr) b0c365c5cc4b2f0d1f3e4cc810e88196a589d565e4a958f6888a21ab380130bc -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.Equals(Inno.Native.ImGui.ImGuiDebugAllocInfoPtr) d4efe9cec1c43a605f1f8d133266b7f62ff6148a0a9db65301a7a337da2d41a7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.Equals(System.Object) e892562bbea7c77ae10230208c69ebbdbe5b5e0f680b99b9ba4de31409a7ba50 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.GetHashCode 7ce85e80b81a6acecaaf1a3cccb28d12145d235a68a6b093917e24c8d56cf7ba -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.TotalAllocCount d17ed5a184bbc094b2b542a744882efe886f70cf6a24db9305b32f043701b136 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.TotalFreeCount 42b142ef769dd79cebefbedefb623dde89678f7b86738b3b7327bfd0caa9c910 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.LastEntriesIdx 349fe822fd04537891090a52c09837145652a1684b6a8d1ebb988c3b7fb413e4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.LastEntriesBuf 38d626aa86f17e7f7799fa40d54904a4e11ca95e539d913b8f963b8ed80d06b9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.Destroy e4354036fe261cade4154f2181c2c22d5c872567bc27da20a5d35981d4615dc2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3740701eb32a8492"></a>[`public Inno.Native.ImGui.ImGuiDebugAllocInfo* Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13812) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ababec43879dd1d8"></a>[`public Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.ImGuiDebugAllocInfoPtr(Inno.Native.ImGui.ImGuiDebugAllocInfo* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13807) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-edcccc897ad37e00"></a>[`public void Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13834) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-71c6412fb982cd4a"></a>[`public bool Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.Equals(Inno.Native.ImGui.ImGuiDebugAllocInfoPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13822) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-5b73ef69bd6aa83a"></a>[`public override bool Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13823) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-4978867516075ddf"></a>[`public override int Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13824) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-6f2d1aa10ff5301d"></a>[`public static bool Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.operator ==(Inno.Native.ImGui.ImGuiDebugAllocInfoPtr left, Inno.Native.ImGui.ImGuiDebugAllocInfoPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13820) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ab94da7bd3092992"></a>[`public static Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.implicit operator Inno.Native.ImGui.ImGuiDebugAllocInfoPtr(Inno.Native.ImGui.ImGuiDebugAllocInfo* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13818) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7ee49e5fe3df708b"></a>[`public static Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.implicit operator Inno.Native.ImGui.ImGuiDebugAllocInfo*(Inno.Native.ImGui.ImGuiDebugAllocInfoPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13819) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c2ee6da97924de4b"></a>[`public static bool Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.operator !=(Inno.Native.ImGui.ImGuiDebugAllocInfoPtr left, Inno.Native.ImGui.ImGuiDebugAllocInfoPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13821) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-75d225d105002840"></a>[`public bool Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13813) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2e9adedab5136c03"></a>[`public Inno.Native.ImGui.ImGuiDebugAllocInfo Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13816) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7a454489217c2d03"></a>[`public System.Span<Inno.Native.ImGui.ImGuiDebugAllocEntry> Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.LastEntriesBuf { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13829) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-904566a3c4dd0b84"></a>[`public ref short Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.LastEntriesIdx { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13828) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b4c14e6930eee7a6"></a>[`public static Inno.Native.ImGui.ImGuiDebugAllocInfoPtr Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13814) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fb729a7a1acd0d4e"></a>[`public ref int Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.TotalAllocCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13826) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-497cd324b662dea5"></a>[`public ref int Inno.Native.ImGui.ImGuiDebugAllocInfoPtr.TotalFreeCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13827) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-992e985028281fca"></a>[`public struct Inno.Native.ImGui.ImGuiDebugAllocInfoPtr : System.IEquatable<Inno.Native.ImGui.ImGuiDebugAllocInfoPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13805) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDebugItemPathQuery</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDebugItemPathQuery ebf1f28a79d907e5556d349e922d19b826f47ff2a16eb5d9603e3fdcc807641b -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.MainID bc59add8f26b8a7a9fa5ffd550fa1a1cb4d6bd3c2f096eb5f714bba4b7ae9241 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.Active 51de303f0d0ba8dac5a298b0f604a0386cd70e6d5dc6cedaecef57a6805f0ddd -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.Complete 03db705e16a1ffe65865a60136203973b07b91985c8c04eb08f153a08ca23161 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.Step 6356079c974850fac77703b20cfd0ce5c737de8393b66e68bb981a33895b40f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.Results 396a3e9bcdbe65695ea80d7273ce2547bc5b34f9c9dbde956f1e05ce3aefd7aa -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.ResultsDescBuf aac3a978cc869648a69f05cfbac7cc6cf0f9b5b7ab0ca10e29606a8f7a611851 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQuery.ResultPathBuf d621a87ed3b39d3138e30dc3742cb3b98cf327b66cbfe031517a6e1b469d6445 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugItemPathQuery.#ctor(System.UInt32,System.Byte,System.Byte,System.Byte,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiStackLevelInfo},Inno.Native.ImGui.ImGuiTextBuffer,Inno.Native.ImGui.ImGuiTextBuffer) b654b49d9454bc74b6570ed6c702910ff92b0d45446fab65659058e75fbfec4b -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugItemPathQuery.Destroy a9fa04285c44240c01b94012fe653e0002cabc83a52b4aec1591114d965aaa77 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-cae7a729d314030e"></a>[`public byte Inno.Native.ImGui.ImGuiDebugItemPathQuery.Active`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7505) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-877efcf3af99abda"></a>[`public byte Inno.Native.ImGui.ImGuiDebugItemPathQuery.Complete`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7506) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9dfce846b0059390"></a>[`public uint Inno.Native.ImGui.ImGuiDebugItemPathQuery.MainID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7504) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3861750ecf11c95e"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiDebugItemPathQuery.ResultPathBuf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7510) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d3af90225c9ebe3e"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiStackLevelInfo> Inno.Native.ImGui.ImGuiDebugItemPathQuery.Results`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7508) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd11eafae4e50ccc"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiDebugItemPathQuery.ResultsDescBuf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7509) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af0b6c07b42ac899"></a>[`public byte Inno.Native.ImGui.ImGuiDebugItemPathQuery.Step`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7507) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d1de883fa18fcca"></a>[`public Inno.Native.ImGui.ImGuiDebugItemPathQuery.ImGuiDebugItemPathQuery(uint mainID = 0, byte active = 0, byte complete = 0, byte step = 0, Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiStackLevelInfo> results = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiStackLevelInfo>), Inno.Native.ImGui.ImGuiTextBuffer resultsDescBuf = default(Inno.Native.ImGui.ImGuiTextBuffer), Inno.Native.ImGui.ImGuiTextBuffer resultPathBuf = default(Inno.Native.ImGui.ImGuiTextBuffer))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7511) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b377b1a06716a904"></a>[`public void Inno.Native.ImGui.ImGuiDebugItemPathQuery.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67389) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c831fcee993cd0a8"></a>[`public struct Inno.Native.ImGui.ImGuiDebugItemPathQuery`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7502) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr</code> · 21 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr cd5ebb0203d0506f82e8655a9eb64251bb1778eb5c03d8acebb00c4924b960f9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.#ctor(Inno.Native.ImGui.ImGuiDebugItemPathQuery*) 729f2806318879b839cd03018f64418687055fd0d1e90cfbe73088a07d73c52d -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Handle 377c5291c029463eb25bee0b10dd12cb76a74c573743260a8bd2152f0656962e -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.IsNull 8df53fb0c5c756f8cb0b94b37a5c4feead185df3d9e82564c3c2295869660b57 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Null 61ab69ea106b102a3ae8fdf76e324329ead7682134a0654b25c9b46c54aeb962 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Item(System.Int32) 107cd7dca0ba11892bf456ec34c71b6f419594d8fb8cc747528ccf1c1d5b25ca -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.op_Implicit(Inno.Native.ImGui.ImGuiDebugItemPathQuery*)~Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr 9707cc84d0585529f78e00633c8e1b0e0948a2500361be9583f44d3a5ce6db6a -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.op_Implicit(Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr)~Inno.Native.ImGui.ImGuiDebugItemPathQuery* f66fef87e4dcc73804565080f5b5ea84163a25086744463638288f7b23a399b6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.op_Equality(Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr,Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr) 5096ca05164ad694d89d137eb0ffbcb5544b62dd3da8209e757bb63bb21fc4dd -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.op_Inequality(Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr,Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr) 07e8ce3fd89697ad716d93dbf22cf92704e104025cbcc2f1fd4171ae7b107fe8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Equals(Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr) 871c36ed3de888e128b663ac78e30cf8fb0b87aa80eae3926d24c85754617146 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Equals(System.Object) 0c15f9366b3361f6e3a60fbfc7ea4a561a524865b0d94c0962f85d0950d5c4a1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.GetHashCode 486db8f7b27a926bb4da2f1b8aec957b1a49ce17c18eb17df9b562517c6bdd2e -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.MainID 797502f3dc9122b41f0b1d10008eaddd0ea53b03241a63090eea6a7946c2d2ef -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Active b2195abd916c8307d4581d5b2a4de3210afdea32969362e426846ba2420f12e0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Complete 424c07e2a3186e456b8da5eeb892151b8be0f3ffe72d707a594da5f454d2a2a5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Step 23f997b15c792bbccdd2f586000b8b7e69efdc556cf2d9d5c075bbec4bdc0b90 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Results 8db0d540603076aa915a9e29932c33505bf61a34009a6ebef69c2e389eb30d87 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.ResultsDescBuf 4b4779f885c82ec31fb8f08d69ca96dce36769f220a677e415011811ce198432 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.ResultPathBuf b5099bfc7226012824d1fe9a8b8d6467eda19a9d5dc5205f16009c2782dfc4d7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Destroy 627e6df8be29ccc1db28e79efdb5a9eb97b6f79937e94a0609ac4282a26beb3e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4e6aac5545a93278"></a>[`public Inno.Native.ImGui.ImGuiDebugItemPathQuery* Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13848) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-19ccfba084898baf"></a>[`public Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.ImGuiDebugItemPathQueryPtr(Inno.Native.ImGui.ImGuiDebugItemPathQuery* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13843) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-c0d4acb4b8b1b234"></a>[`public void Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13873) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a5194772a8cb920c"></a>[`public bool Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Equals(Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13858) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-069e5d4547b15071"></a>[`public override bool Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13859) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-71a75284bedcf76c"></a>[`public override int Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13860) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-1a3891021a41ef3a"></a>[`public static bool Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.operator ==(Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr left, Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13856) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-baba99a8e0ef1978"></a>[`public static Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.implicit operator Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr(Inno.Native.ImGui.ImGuiDebugItemPathQuery* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13854) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a9eb70ae20b7e136"></a>[`public static Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.implicit operator Inno.Native.ImGui.ImGuiDebugItemPathQuery*(Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13855) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a7149c768b6f0046"></a>[`public static bool Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.operator !=(Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr left, Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13857) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-992035a900e85c8b"></a>[`public bool Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Active { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13863) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7640a3b634026093"></a>[`public bool Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Complete { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13864) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7df3fda1e5de46d0"></a>[`public bool Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13849) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f7814701f22b15ed"></a>[`public Inno.Native.ImGui.ImGuiDebugItemPathQuery Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13852) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9a7cc5aea3c55cfa"></a>[`public ref uint Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.MainID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13862) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8b246b30bcbe7bf2"></a>[`public static Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13850) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ef8ad988eb4e74ef"></a>[`public ref Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.ResultPathBuf { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13868) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b28f661cdef5a34d"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiStackLevelInfo> Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Results { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13866) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-60b835b43dc397d9"></a>[`public ref Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.ResultsDescBuf { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13867) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d83a41dad13ac02f"></a>[`public ref byte Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr.Step { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13865) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aff6c8024393833d"></a>[`public struct Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr : System.IEquatable<Inno.Native.ImGui.ImGuiDebugItemPathQueryPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13841) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDebugLogFlags</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDebugLogFlags 3f642ae7d0520154dc48d12c2beae78046b285a1a7ffd4319c96eda23e7703d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.None 0c1cd8890d6f7d48459fe3db82af858f7bad1f9ec0253632f826c1978483d3a4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventError 3f7988e732ff6ec766731d1a1e003fb56e62e33cb585ff8c98c1f8afc1413ebb -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventActiveId 5a7f66cae6796cdf9e5b260ce940904c32f3f0f28e6f99e6f7df51403c10aaed -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventFocus c28af45a27e2c81e95ff1db8dd4a5d5f14516a17bb12b862edda3c20d5f0dfa4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventPopup 45c2af9abfc5bfbb107c99d1e7d679f859d354108f5de8dbb6eb163d5e982411 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventNav a3cfe7d9c916356913616cd2160ede6f2fefa0d43831c7c246fcc2f352d98b15 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventClipper f08e5ff31f5f1415126dc832f4191c4e6b17729fb3e93b216f63d875b23ade1c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventSelection 555428b1768c339006bd6109bbb9299e3ff2cf90a5713bafa9c816389338fbab -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventIo cb9687b49654f0fb70f15a082fb1f331f7adabc2def0443acced7402badee42e -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventFont 9d399dba738ec76cf391ff985040eac2c809ed6d34500cc1183d8ebe8c818d21 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventInputRouting 9c6e0743775b3e2871b754831f3b6b7277a57926953bcf65dcd2a8a34c585d85 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventDocking 8ca7583cab0eb8789730aeaf26937d7cb2136d405cb9c50d051f5c6a44c7498a -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventViewport 728163581207d6495c09f676defb0e1262e2b2fb930372c76894789220b26084 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.EventMask 77629ff2b8486624af89b9e27e41851df465dbd948ef465cedeb655059fd10f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.OutputToTty 8643639a7a8fb00a8c52f7bd81a7e665827b7f41b0551d82db951ed6c4d42a90 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.OutputToDebugger 082eaf8ea663acfb466e2528cf5b084c972a6c6056c4ca216937fcbd7a9ce323 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDebugLogFlags.OutputToTestEngine d2984fdc18f091d6bd391413afb73b7b3c14ee95790ae4f3d360b8e8d2a44528 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-24782b9fea8d0c9b"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventActiveId = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1659) | 当前常量或枚举值：`2`。 | — |
| <a id="api-a7262b7b42aa6d38"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventClipper = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1663) | 当前常量或枚举值：`32`。 | — |
| <a id="api-ce31126d2a527df0"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventDocking = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1668) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-4e2d0abd6a1d8671"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventError = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1658) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d7f7886c6903ea06"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventFocus = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1660) | 当前常量或枚举值：`4`。 | — |
| <a id="api-3d9b573266145b3d"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventFont = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1666) | 当前常量或枚举值：`256`。 | — |
| <a id="api-3de9448b4926a3bd"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventInputRouting = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1667) | 当前常量或枚举值：`512`。 | — |
| <a id="api-8131fc55b32b6597"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventIo = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1665) | 当前常量或枚举值：`128`。 | — |
| <a id="api-76105d237e11ec5a"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventMask = 4095`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1670) | 当前常量或枚举值：`4095`。 | — |
| <a id="api-bc8df9eef99c87e4"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventNav = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1662) | 当前常量或枚举值：`16`。 | — |
| <a id="api-4205f4f42034d340"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventPopup = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1661) | 当前常量或枚举值：`8`。 | — |
| <a id="api-6570ff0a41ecfd7f"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventSelection = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1664) | 当前常量或枚举值：`64`。 | — |
| <a id="api-40e271d9e610901c"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.EventViewport = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1669) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-71b35ebf3682d109"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1657) | 当前常量或枚举值：`0`。 | — |
| <a id="api-5752c7395f24123a"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.OutputToDebugger = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1672) | 当前常量或枚举值：`2097152`。 | — |
| <a id="api-2b06a45f02381ac9"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.OutputToTestEngine = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1673) | 当前常量或枚举值：`4194304`。 | — |
| <a id="api-5ed9130f7e4d2887"></a>[`Inno.Native.ImGui.ImGuiDebugLogFlags.OutputToTty = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1671) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-4a218753af2921fc"></a>[`public enum Inno.Native.ImGui.ImGuiDebugLogFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1655) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDir</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDir 1cef80568729ed97c9ba2c94c71dbc59acdff20fb178c40c8e9f362871da8cb2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDir.None 00e88cceda45629797e247a3ca62c8a0e484157521ecf6a0474b86c53e14abd7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDir.Left 50658553ec6d5020271cb72a871d6c5ce4607fa193c220b8b5dfbf93314fe66b -->
<!-- api: F:Inno.Native.ImGui.ImGuiDir.Right fa53e20c630d52f792338df6a59b7fd9275e086ee025f3e4bf22132d8e79e5f5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDir.Up 11d462f3b308a7bfdcc5a7b9ec998ff3ce88e195808c1f94a0c942f78d5fd283 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDir.Down 0638e7645cd4a4a3cc39b17445528ac780a93bd4cc2f100ae12bb5ff491c8429 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDir.Count dd046e12a8c6e27e00b10a7e36898d0038dbfdb7cde70bd990cb39cd038ac251 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-76057f7e3e8ae460"></a>[`Inno.Native.ImGui.ImGuiDir.Count = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L86) | 当前常量或枚举值：`4`。 | — |
| <a id="api-ddada31e995ed4cc"></a>[`Inno.Native.ImGui.ImGuiDir.Down = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L85) | 当前常量或枚举值：`3`。 | — |
| <a id="api-c5ddf8721645aa0c"></a>[`Inno.Native.ImGui.ImGuiDir.Left = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L82) | 当前常量或枚举值：`0`。 | — |
| <a id="api-54ef62041121f742"></a>[`Inno.Native.ImGui.ImGuiDir.None = -1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L81) | 当前常量或枚举值：`-1`。 | — |
| <a id="api-67059de5645ec266"></a>[`Inno.Native.ImGui.ImGuiDir.Right = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L83) | 当前常量或枚举值：`1`。 | — |
| <a id="api-360919daa1f4e053"></a>[`Inno.Native.ImGui.ImGuiDir.Up = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L84) | 当前常量或枚举值：`2`。 | — |
| <a id="api-64d19c996f8d9b87"></a>[`public enum Inno.Native.ImGui.ImGuiDir : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L79) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDockContext</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDockContext da43a1f7a242495c785541f2d4bb6e72d63e435ea98e1178629b7e1e00cb2fa7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockContext.Nodes 1578e93beb29d8f8c4da1f03f23bcbc641420b7d8a1731412752c8cee00d9a4c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockContext.Requests 21b26c1d7c49468139efd0ab1442f1ae6e171d5593c87866aaefe4d696f63503 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockContext.NodesSettings 99145f5a2e68ec7e783e25d41da10acb845c205fc6081a10f1f758e1e87e8afe -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockContext.WantFullRebuild 8191e143784209eac5df172ae668599f2ede932c13a7853ac8fb88a8d4fe5632 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockContext.#ctor(Inno.Native.ImGui.ImGuiStorage,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiDockRequest},Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiDockNodeSettings},System.Byte) e55ee65caeec0c404b24b88f90d0566487b045e04461c9fc7475c34b906ae291 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockContext.Destroy 2c73b28053b581e980415ff2d6aca93ef9a1e9b72ef8f78e6afcf5a12185c0ff -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ffa5783685f7745f"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImGuiDockContext.Nodes`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7347) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a52c021c16c5416"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiDockNodeSettings> Inno.Native.ImGui.ImGuiDockContext.NodesSettings`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7349) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6787ebdedac72b3e"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiDockRequest> Inno.Native.ImGui.ImGuiDockContext.Requests`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7348) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8f0032db012694e9"></a>[`public byte Inno.Native.ImGui.ImGuiDockContext.WantFullRebuild`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7350) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3089695027b51388"></a>[`public Inno.Native.ImGui.ImGuiDockContext.ImGuiDockContext(Inno.Native.ImGui.ImGuiStorage nodes = default(Inno.Native.ImGui.ImGuiStorage), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiDockRequest> requests = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiDockRequest>), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiDockNodeSettings> nodesSettings = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiDockNodeSettings>), byte wantFullRebuild = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7351) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6993f7be98a51d6b"></a>[`public void Inno.Native.ImGui.ImGuiDockContext.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67217) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7ed47bb1a8781e01"></a>[`public struct Inno.Native.ImGui.ImGuiDockContext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7345) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDockContextPtr</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDockContextPtr b46cbeedc51e9f43a5470997766ddbb743f3165f1d9b2ae7f31e211bf60639a4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockContextPtr.#ctor(Inno.Native.ImGui.ImGuiDockContext*) 47218bddbc98ee6d74c2c500eef339d6d66b4a53113ac102d3f3c24376ee150a -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockContextPtr.Handle ed119039afb6b9ffecf1cca7ff5fc9b8909d82e2699f2a22fa3c3b8e15e65ce3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockContextPtr.IsNull 48d566fd7ba116f852b5db8215465fb0ce47ce2f61426c1840bdec40eba0aa3d -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockContextPtr.Null ba74636520588413acd60e2c8d0f013a7c471ccbef34a93274769af7a8006f2e -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockContextPtr.Item(System.Int32) b78fdcf363e13d7911f602a09234676d86aaeb30778841b3ae2aec809c37ec4e -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockContextPtr.op_Implicit(Inno.Native.ImGui.ImGuiDockContext*)~Inno.Native.ImGui.ImGuiDockContextPtr 1020916401537eab2bb8875d21477768834edbd9a4ffb9b0e29daa1cbe363fd6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockContextPtr.op_Implicit(Inno.Native.ImGui.ImGuiDockContextPtr)~Inno.Native.ImGui.ImGuiDockContext* 16c1181ae5e6262df6d0e3843c7d777b0cf3d99cad844c0e30ab5e3d821f9100 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockContextPtr.op_Equality(Inno.Native.ImGui.ImGuiDockContextPtr,Inno.Native.ImGui.ImGuiDockContextPtr) d8a1b61d78a54efbb55af48464bcd91adacbf861cd36dfaed2dfe50d055da559 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockContextPtr.op_Inequality(Inno.Native.ImGui.ImGuiDockContextPtr,Inno.Native.ImGui.ImGuiDockContextPtr) 7ce6ca9a7eb1ee7ee1b7f54d8836779b41b339e4416f5cad9915934648329d9a -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockContextPtr.Equals(Inno.Native.ImGui.ImGuiDockContextPtr) 41097d1e4b8723a2cff7484541a08341dd2b68e36b84eca63c201d17c0ef3faa -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockContextPtr.Equals(System.Object) d320ba80d1c5c814bfea2e9500bce4f4f25ed18e0a90ab1aa1b6670c1504b512 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockContextPtr.GetHashCode 535b0e9afb244d6eba61eaeb500cda021b948a5ae875bcb5a447547c2a21e522 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockContextPtr.Nodes 288bed5f3788d97e63a0181b7196a428e3e36fc186bc71e9a03a9559c4ed15d9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockContextPtr.Requests 2c99d47ebcf3255a36ed536685006bdc758ae1bfe0e1671bc2e3d31e1eeee8e0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockContextPtr.NodesSettings 7eeb91da2c380104cb8421e92d7626900f85465c7fdab4778ef894d4ed7a48a7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockContextPtr.WantFullRebuild 6fea38381eb722fd9c4503eb15d791d1bd2b04931829fc70a3d1f1e89f921db5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockContextPtr.Destroy cf9448c12ea19187921d561ff02bf4641310a41f837482582af672367973f875 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-707fa83fe3c8a493"></a>[`public Inno.Native.ImGui.ImGuiDockContext* Inno.Native.ImGui.ImGuiDockContextPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13887) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9eb84de186ef3ed3"></a>[`public Inno.Native.ImGui.ImGuiDockContextPtr.ImGuiDockContextPtr(Inno.Native.ImGui.ImGuiDockContext* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13882) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1406c2fa92219185"></a>[`public void Inno.Native.ImGui.ImGuiDockContextPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13909) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d9b823ca3c405618"></a>[`public bool Inno.Native.ImGui.ImGuiDockContextPtr.Equals(Inno.Native.ImGui.ImGuiDockContextPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13897) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-f01b545091f2c8d8"></a>[`public override bool Inno.Native.ImGui.ImGuiDockContextPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13898) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-19774841c075fb8f"></a>[`public override int Inno.Native.ImGui.ImGuiDockContextPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13899) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-24f6595448d28fd4"></a>[`public static bool Inno.Native.ImGui.ImGuiDockContextPtr.operator ==(Inno.Native.ImGui.ImGuiDockContextPtr left, Inno.Native.ImGui.ImGuiDockContextPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13895) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-45275a37b5263250"></a>[`public static Inno.Native.ImGui.ImGuiDockContextPtr.implicit operator Inno.Native.ImGui.ImGuiDockContextPtr(Inno.Native.ImGui.ImGuiDockContext* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13893) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-97a5e1afd99a3e4c"></a>[`public static Inno.Native.ImGui.ImGuiDockContextPtr.implicit operator Inno.Native.ImGui.ImGuiDockContext*(Inno.Native.ImGui.ImGuiDockContextPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13894) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9a75ca4fe955e5db"></a>[`public static bool Inno.Native.ImGui.ImGuiDockContextPtr.operator !=(Inno.Native.ImGui.ImGuiDockContextPtr left, Inno.Native.ImGui.ImGuiDockContextPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13896) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a78c7d303a558ca7"></a>[`public bool Inno.Native.ImGui.ImGuiDockContextPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13888) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-babf65c4e6a882de"></a>[`public Inno.Native.ImGui.ImGuiDockContext Inno.Native.ImGui.ImGuiDockContextPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13891) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dc47f94eaec40196"></a>[`public ref Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImGuiDockContextPtr.Nodes { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13901) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1185d5346ce00c75"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiDockNodeSettings> Inno.Native.ImGui.ImGuiDockContextPtr.NodesSettings { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13903) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fa023ee756eb96cc"></a>[`public static Inno.Native.ImGui.ImGuiDockContextPtr Inno.Native.ImGui.ImGuiDockContextPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13889) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-05bc35d3f6aad115"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiDockRequest> Inno.Native.ImGui.ImGuiDockContextPtr.Requests { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13902) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-68c87bb4ccf70da2"></a>[`public bool Inno.Native.ImGui.ImGuiDockContextPtr.WantFullRebuild { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13904) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a0bd1e242746c50d"></a>[`public struct Inno.Native.ImGui.ImGuiDockContextPtr : System.IEquatable<Inno.Native.ImGui.ImGuiDockContextPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13880) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDockNode</code> · 65 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDockNode 997025c8cc2be46c7d8c01f8069381f097a2a3c1d0341b7f95cb1dc80df5f289 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.ID 83cf6dc9cb7c0b2a8068ef0e9c2099b76f59a0af6a4253e8b05c77cdc27d1116 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.SharedFlags 6d12ece900ff066baef81c8fa11591e40db1c5ea926a85ee0fbca66e40fc7940 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LocalFlags 6524def95bbbd72a8cdd596c7f117294e35234d4cd2ccd59cab6ea43cf314de8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LocalFlagsInWindows 970c6ca7dea1bc29be9a54976b198e3951f8c57868d63b16eeef65babd199bd3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.MergedFlags 823c8e043088c355d21219fa8ee9452688d1bc1b6cc896490eb17670c11f77b3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.State c71c4f27bc8164d8251ad1e9ee4679bc8a292e2a69a6ebf441fb8d69aedafcd4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.ParentNode 43371c690fa3deb111bfd1658a3e00012a7b7fdcf5a82303464f2314bb157063 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.ChildNodes_0 963d3a5c909c34fae8de6156a07a7aeb683ed7767ef931d2222426877d3d956a -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.ChildNodes_1 75c0abb801f4fca42f2cb9aa4f7fe9d107a4fc4ad69024426ad990e8a4a90685 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.ChildNodes 67243df8833d8f76c84b727afb3963819bc283397d2623d50fd88bbadf5ecd61 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.Windows 4c595e4c050cd808fc21af126772033829cec1804a7a87c2e794f7fb1e7da514 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.TabBar 5162767d665919c53a3e132282cbecb664011b35454ce480451eec2ecaef1f09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.Pos 898d95cf6a768605cf53e18194f184fa6419b8b6bebf29c269de02a8acd73774 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.Size bdc0084624b49063d68195aa33f59310c2197421eeca720c03cc57a6e745122e -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.SizeRef bb77cc5bbd4beae50efea348cb7256c2f01ff017d468b0d4707a7dd8f01f795a -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.SplitAxis f29df46cc65e3e2ea6658f1c15ae5d8520fc7e38eeac322cf7e8edc346d18d8e -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.WindowClass 9034c18dd73de69780bb396eb9cbd8267d9a982f436b2c745c8087ed54d1f9ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LastBgColor 413a7d83229e6bbd1a093ba75f38447b4ec2552cb80df8b819bf48ec01e09dde -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.HostWindow 4ee164f7052af22493c8c6d74a5ca61a16ce1f92c727b5534619d4ea60c6b6d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.VisibleWindow 1bb3c69dbf98d413dfec5cd2f8b62aae0f0aaa5d47d2df85c2516d3877eed0f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.CentralNode bff53ea71985265ecf013c72d2b33138361a8b691f9c16b5334030b033fd59c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.OnlyNodeWithWindows 3bd5a29153e12559f20a8f30ff18f9f3062a9f06f28edf7d5a349b40cf55007a -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.CountNodeWithWindows 9d3a7263fc279b997508fa2d106be36dbbb97391b8c4f4f33b046a814af81de9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LastFrameAlive a8053871af02ccb409a20533eba0de54d4d657908cc79829385d16a1002eb32a -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LastFrameActive fa99399f2ef65d4a6c63a7e885efc0efd1469bb030c67599739f10124b2491df -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LastFrameFocused 8938a8ca5e8b109d02efe115fdbbecb1a70a984c1df287192296408dac97418c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.LastFocusedNodeId 591ace644a8bdc5cd4da1360c35df1944a63c797dc18d584699e0514d64075a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.SelectedTabId d0dc2dd4290c2918b58d6a1fc2c9adb50b2d77e0b6db7ed36c3eeaf4a094e631 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.WantCloseTabId 8ac7f36775900f625a9849427a44590370c218b0816a45d8c0c9ac15eabc99d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.RefViewportId 4ffe38488f649e20ba6dc907c4050fe7eed8c9dd96425a2fcb771e4174bcf4ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.RawBits0_0 ac85fb08bada9d5010adc83f56c4008c8cc7320c937c629f7558d338117e614b -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.RawBits0_1 45871e2b004636210b5d3dbd6d54a57da2e6d9657359682e406bdd987960d2b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.RawBits0_2 6865ea50b3725b941f084f3b9ad342b6f70bfe9bdfb92821da8d53098b791637 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.RawBits0_3 fa350e50e868d081c05623469d0012118525132f7c3540acf457469320893499 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.RawBits0_4 c10a39d75b6447f1364b2c4a346700de5bd4ffbad9a373f30545d2532808f03e -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNode.RawBits0_5 e1935e10d39955859ad16bc1344b79f50bfd380919785a9b2a5c946c53d4cb57 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.AuthorityForPos 6867358d77caf7ff35d5016f4f71a1d167dc7d6dfb31b1c645f4fa7e782afd34 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.AuthorityForSize b02f20ecbfd21b995061de36ab187eff2cd4ffaea43358ed6527d25d80506520 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.AuthorityForViewport 07e715888e707bbef79650abb6c105b222bc6ff8907eb2d670b4c5521b8f33ee -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.IsVisible 7ab239993337fdd963dc7282b9b41fed7c816bf9d43413cac204ad3cd3609c18 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.IsFocused 37c2d42e1c135bf18acbed4e3b78d92463cc6b41a22134ab959bee53b42ea8e8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.IsBgDrawnThisFrame 64e97bc5129577b469ea2337f08355e05d763b8bb40ca9f301a09e7e567ebdef -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.HasCloseButton 6e66d5b967ad27136ea82f3b29be427fb461b3396965977a75479184022d5974 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.HasWindowMenuButton d60d28a62b2acbbbef3d96c5f2e7f8b5b1ee6a6cbf4c9f6b0bab4c1be229fc46 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.HasCentralNodeChild 3bbae4d29a889c6f94754a140e324bfd77b7275556241e2445e089102dbd93c9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.WantCloseAll e3c6f5d853947a119bc13bf4854fd8a1d3f2388d24dee67fd1dcc11f1426f5ff -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.WantLockSizeOnce 88ebc7c6c6972d533c1ddf91f665acbc606e83fc36591bfbfa5d316fea5be50d -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.WantMouseMove 7a1d63e949c21dda2404a34131c6dbdab8cb2b058683eb7e7e8b875bccf92353 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.WantHiddenTabBarUpdate 624fcf998053490d803bd1ee5375e611192f8bc6f1e71b3afa735b0967939340 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNode.WantHiddenTabBarToggle 9113d959a7bc6a76770f1635ce3cae12e3875b6033bc123dce1e72a9d5a5b4b1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.#ctor(System.UInt32,Inno.Native.ImGui.ImGuiDockNodeFlags,Inno.Native.ImGui.ImGuiDockNodeFlags,Inno.Native.ImGui.ImGuiDockNodeFlags,Inno.Native.ImGui.ImGuiDockNodeFlags,Inno.Native.ImGui.ImGuiDockNodeState,Inno.Native.ImGui.ImGuiDockNode*,Inno.Native.ImGui.ImGuiDockNode**,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiWindowPtr},Inno.Native.ImGui.ImGuiTabBar*,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,Inno.Native.ImGui.ImGuiAxis,Inno.Native.ImGui.ImGuiWindowClass,System.UInt32,Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiDockNode*,Inno.Native.ImGui.ImGuiDockNode*,System.Int32,System.Int32,System.Int32,System.Int32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,Inno.Native.ImGui.ImGuiDataAuthority,Inno.Native.ImGui.ImGuiDataAuthority,Inno.Native.ImGui.ImGuiDataAuthority,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte) 220058e0996c5ecd3b0d0d97a58c62087cb77b901595239c7c7a7807e7f52e0e -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.Destroy a8287772e71cba0bc115201ef7ceebe442778144b20f6f6c168b9f7df705d6c1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.IsRootNode 075fa66971249739b5b4ddfbd5ae50f3953bec09087125e5abeafb4db51a5d20 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.IsDockSpace 0a620255b9f78095a7293382623d60747abc00d494143bad369cba1868dfe5d6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.IsFloatingNode 0d23bcb61349501318c431cc3c3c9400817788497b465eddce031044448f8e90 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.IsCentralNode be2115e4a8e89dd05a55429c8e5cf3fd62c382c07288bbc3fb0be4a3c979c730 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.IsHiddenTabBar 623713096f5e90eded0772ed03d6b033fe5635e4d5237afc61bb5d447b03f5a3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.IsNoTabBar cf8fd710269d2a1f32a19cd6789bf1078fe5143c3d630a9b9fef08f059ea16ea -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.IsSplitNode f2cac3bdbb90c055a0650e210201a1253b315fc0783cb4089a5e46945d5f8b49 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.IsLeafNode 97112b956ed7f54807f7830dba7d2a3ab5b1b3854006a93d8450a601977b0df3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.IsEmpty ac4210fedca0d628840f789c300922415972901c996c12bff5515e6381273549 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.Rect 58c3bbf8664e160e2410cb939063bf18aa489296244831a089a5126955f53e02 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.SetLocalFlags(Inno.Native.ImGui.ImGuiDockNodeFlags) 91ca203381f3cceba73b1cc65f4bcc38d46948c4e618e8c746892129780da4be -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNode.UpdateMergedFlags e821851e1524d2bc95b48ed424defd04da0839643d787d615150a5b4dfecbf9b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-28763f272e1d6cd6"></a>[`public Inno.Native.ImGui.ImGuiDockNode* Inno.Native.ImGui.ImGuiDockNode.CentralNode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4511) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fd16fb2321702bd5"></a>[`public BGCS.Runtime.Pointer<Inno.Native.ImGui.ImGuiDockNode> Inno.Native.ImGui.ImGuiDockNode.ChildNodes_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4485) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc9fa9ed0b786403"></a>[`public BGCS.Runtime.Pointer<Inno.Native.ImGui.ImGuiDockNode> Inno.Native.ImGui.ImGuiDockNode.ChildNodes_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4487) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-378591bc97506952"></a>[`public int Inno.Native.ImGui.ImGuiDockNode.CountNodeWithWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4515) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af0558960ca01a21"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiDockNode.HostWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4507) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9cfbc36419a3e9d7"></a>[`public uint Inno.Native.ImGui.ImGuiDockNode.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4471) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a214656dc3e3c20"></a>[`public uint Inno.Native.ImGui.ImGuiDockNode.LastBgColor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4505) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cbffb8609369541b"></a>[`public uint Inno.Native.ImGui.ImGuiDockNode.LastFocusedNodeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4523) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc780a82e9aa4548"></a>[`public int Inno.Native.ImGui.ImGuiDockNode.LastFrameActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4519) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-522d71c858004949"></a>[`public int Inno.Native.ImGui.ImGuiDockNode.LastFrameAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4517) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a055b5e879fb2075"></a>[`public int Inno.Native.ImGui.ImGuiDockNode.LastFrameFocused`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4521) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3052565cf232bedd"></a>[`public Inno.Native.ImGui.ImGuiDockNodeFlags Inno.Native.ImGui.ImGuiDockNode.LocalFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4475) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c3ff7b7aebcfb96"></a>[`public Inno.Native.ImGui.ImGuiDockNodeFlags Inno.Native.ImGui.ImGuiDockNode.LocalFlagsInWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4477) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-788e196f6de5643e"></a>[`public Inno.Native.ImGui.ImGuiDockNodeFlags Inno.Native.ImGui.ImGuiDockNode.MergedFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4479) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5ba23708724aa92"></a>[`public Inno.Native.ImGui.ImGuiDockNode* Inno.Native.ImGui.ImGuiDockNode.OnlyNodeWithWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4513) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-71e80f00248151b6"></a>[`public Inno.Native.ImGui.ImGuiDockNode* Inno.Native.ImGui.ImGuiDockNode.ParentNode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4483) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3477c90c8e407e79"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiDockNode.Pos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4495) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b7ce5de27b91486"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.RawBits0_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4531) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aeaf0920458dda3f"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.RawBits0_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4533) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fbcfa5a3b783c168"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.RawBits0_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4535) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cbb7e5b63fa50b76"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.RawBits0_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4537) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a9616d80f8cfa477"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.RawBits0_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4539) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7da675579b3d55e0"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.RawBits0_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4541) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c3666060ea429c2"></a>[`public uint Inno.Native.ImGui.ImGuiDockNode.RefViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4529) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b1de62fec720a946"></a>[`public uint Inno.Native.ImGui.ImGuiDockNode.SelectedTabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4525) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7a878c1c9a9b64d8"></a>[`public Inno.Native.ImGui.ImGuiDockNodeFlags Inno.Native.ImGui.ImGuiDockNode.SharedFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4473) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-50069db99f31c7f5"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiDockNode.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4497) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2dbe0f5a0a0ad70"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiDockNode.SizeRef`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4499) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-81d91c40828df37c"></a>[`public Inno.Native.ImGui.ImGuiAxis Inno.Native.ImGui.ImGuiDockNode.SplitAxis`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4501) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-16b0ea67cb59a23c"></a>[`public Inno.Native.ImGui.ImGuiDockNodeState Inno.Native.ImGui.ImGuiDockNode.State`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4481) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-273b2b6aa06f9262"></a>[`public Inno.Native.ImGui.ImGuiTabBar* Inno.Native.ImGui.ImGuiDockNode.TabBar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4493) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-afe4842d12545f40"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiDockNode.VisibleWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4509) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e532ce708a8cd468"></a>[`public uint Inno.Native.ImGui.ImGuiDockNode.WantCloseTabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4527) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df9fbd3a16e83d53"></a>[`public Inno.Native.ImGui.ImGuiWindowClass Inno.Native.ImGui.ImGuiDockNode.WindowClass`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4503) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d6e5a7f91bba7b09"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiWindowPtr> Inno.Native.ImGui.ImGuiDockNode.Windows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4491) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ac5cbb4a54780148"></a>[`public Inno.Native.ImGui.ImGuiDockNode.ImGuiDockNode(uint iD = 0, Inno.Native.ImGui.ImGuiDockNodeFlags sharedFlags = Inno.Native.ImGui.ImGuiDockNodeFlags.None, Inno.Native.ImGui.ImGuiDockNodeFlags localFlags = Inno.Native.ImGui.ImGuiDockNodeFlags.None, Inno.Native.ImGui.ImGuiDockNodeFlags localFlagsInWindows = Inno.Native.ImGui.ImGuiDockNodeFlags.None, Inno.Native.ImGui.ImGuiDockNodeFlags mergedFlags = Inno.Native.ImGui.ImGuiDockNodeFlags.None, Inno.Native.ImGui.ImGuiDockNodeState state = Inno.Native.ImGui.ImGuiDockNodeState.Unknown, Inno.Native.ImGui.ImGuiDockNode* parentNode = null, Inno.Native.ImGui.ImGuiDockNode** childNodes = null, Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiWindowPtr> windows = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiWindowPtr>), Inno.Native.ImGui.ImGuiTabBar* tabBar = null, System.Numerics.Vector2 pos = default(System.Numerics.Vector2), System.Numerics.Vector2 size = default(System.Numerics.Vector2), System.Numerics.Vector2 sizeRef = default(System.Numerics.Vector2), Inno.Native.ImGui.ImGuiAxis splitAxis = Inno.Native.ImGui.ImGuiAxis.X, Inno.Native.ImGui.ImGuiWindowClass windowClass = default(Inno.Native.ImGui.ImGuiWindowClass), uint lastBgColor = 0, Inno.Native.ImGui.ImGuiWindow* hostWindow = null, Inno.Native.ImGui.ImGuiWindow* visibleWindow = null, Inno.Native.ImGui.ImGuiDockNode* centralNode = null, Inno.Native.ImGui.ImGuiDockNode* onlyNodeWithWindows = null, int countNodeWithWindows = 0, int lastFrameAlive = 0, int lastFrameActive = 0, int lastFrameFocused = 0, uint lastFocusedNodeId = 0, uint selectedTabId = 0, uint wantCloseTabId = 0, uint refViewportId = 0, Inno.Native.ImGui.ImGuiDataAuthority authorityForPos = Inno.Native.ImGui.ImGuiDataAuthority.Auto, Inno.Native.ImGui.ImGuiDataAuthority authorityForSize = Inno.Native.ImGui.ImGuiDataAuthority.Auto, Inno.Native.ImGui.ImGuiDataAuthority authorityForViewport = Inno.Native.ImGui.ImGuiDataAuthority.Auto, byte isVisible = 0, byte isFocused = 0, byte isBgDrawnThisFrame = 0, byte hasCloseButton = 0, byte hasWindowMenuButton = 0, byte hasCentralNodeChild = 0, byte wantCloseAll = 0, byte wantLockSizeOnce = 0, byte wantMouseMove = 0, byte wantHiddenTabBarUpdate = 0, byte wantHiddenTabBarToggle = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4557) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-00b69226e1c62dfb"></a>[`public void Inno.Native.ImGui.ImGuiDockNode.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67071) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9f502a42c7bd813c"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.IsCentralNode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67115) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-765af224f8bf37bb"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.IsDockSpace()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67093) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c05e2272ed3cadbb"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.IsEmpty()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67170) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5abef252632e72e0"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.IsFloatingNode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67104) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4f3e0328395a8b53"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.IsHiddenTabBar()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67126) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-068fbf6e30642b45"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.IsLeafNode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67159) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1c03cc377bdc1d11"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.IsNoTabBar()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67137) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6e37b1e27c328b1c"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.IsRootNode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67082) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4141c612f1282de6"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.IsSplitNode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67148) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eb259ed4847e5459"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiDockNode.Rect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67181) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-010acb94103bfff3"></a>[`public void Inno.Native.ImGui.ImGuiDockNode.SetLocalFlags(Inno.Native.ImGui.ImGuiDockNodeFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67192) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-84a6859a1f315b8e"></a>[`public void Inno.Native.ImGui.ImGuiDockNode.UpdateMergedFlags()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67203) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-33aebe01419ad356"></a>[`public Inno.Native.ImGui.ImGuiDataAuthority Inno.Native.ImGui.ImGuiDockNode.AuthorityForPos { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4542) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-044859898111758d"></a>[`public Inno.Native.ImGui.ImGuiDataAuthority Inno.Native.ImGui.ImGuiDockNode.AuthorityForSize { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4543) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-837292b76f626705"></a>[`public Inno.Native.ImGui.ImGuiDataAuthority Inno.Native.ImGui.ImGuiDockNode.AuthorityForViewport { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4544) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d32c56bfe5b0508d"></a>[`public System.Span<BGCS.Runtime.Pointer<Inno.Native.ImGui.ImGuiDockNode>> Inno.Native.ImGui.ImGuiDockNode.ChildNodes { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4488) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b96b1460c5d248b8"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.HasCentralNodeChild { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4550) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6d934938644fd2f5"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.HasCloseButton { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4548) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-09bc7b1555590ab3"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.HasWindowMenuButton { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4549) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3f89b79ef2889527"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.IsBgDrawnThisFrame { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4547) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c2980174af4eaec6"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.IsFocused { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4546) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d70936391c1a6024"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.IsVisible { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4545) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d32a8f4dd30de638"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.WantCloseAll { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4551) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dd00322592fbc57b"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.WantHiddenTabBarToggle { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4555) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c6da53c7c9f7a9bb"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.WantHiddenTabBarUpdate { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4554) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-25e180e7e2e59c31"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.WantLockSizeOnce { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4552) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba6dd687d2cad44e"></a>[`public byte Inno.Native.ImGui.ImGuiDockNode.WantMouseMove { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4553) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3b461c2777649317"></a>[`public struct Inno.Native.ImGui.ImGuiDockNode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4468) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDockNodeFlags</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDockNodeFlags f39c0f6de209be3fca5dd7eaef16692d6acffbc31ec06be8d522f94af130c1c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.None 178c50d7deb255ac58b994dabe749a930500120038a312900627a22d99dcd421 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.KeepAliveOnly dbf7af3742cfb1966c6b1f42139ff5ecd461f061d6718fee652dcfc8adb96da3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.NoDockingOverCentralNode 9127305d46c4655e46bd4950ce973d0020d2f733ae510bba537c1c96b547ba6e -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.PassthruCentralNode 33cb04a405cfa14e5fbcbfba8e5fc5a6e6bd5169346523eac40c98ccbc51e650 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.NoDockingSplit 9b9670f5cd933bddd5f55acb3461fdcebd0843ea93bbae95e6c44f0b5e0a80b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.NoResize fe2af67ead24cd624870dbc0c6e15b0ace0b703b9d8e61567c62cbe5860ed70c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.AutoHideTabBar 2f88ee1e5e8ac7d0ec921a661d58ee95c720c60ca94cc558b6061d8333b4f43f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlags.NoUndocking 8d602906ec7061fc42b8b6cf9c4eb5abc65e95ea6e4f50bc7a41c0493ef53b11 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8a999bb07e13357d"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.AutoHideTabBar = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L649) | 当前常量或枚举值：`64`。 | — |
| <a id="api-3919153543b48450"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.KeepAliveOnly = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L644) | 当前常量或枚举值：`1`。 | — |
| <a id="api-f9b10e923aaf38b6"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.NoDockingOverCentralNode = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L645) | 当前常量或枚举值：`4`。 | — |
| <a id="api-c356494214e38f83"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.NoDockingSplit = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L647) | 当前常量或枚举值：`16`。 | — |
| <a id="api-fc8a70341509cd22"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L643) | 当前常量或枚举值：`0`。 | — |
| <a id="api-4723e3f8a62c1532"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.NoResize = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L648) | 当前常量或枚举值：`32`。 | — |
| <a id="api-14fd3ea0a9d530c6"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.NoUndocking = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L650) | 当前常量或枚举值：`128`。 | — |
| <a id="api-541a6e8aef2d6e32"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlags.PassthruCentralNode = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L646) | 当前常量或枚举值：`8`。 | — |
| <a id="api-c9326247c7cf4373"></a>[`public enum Inno.Native.ImGui.ImGuiDockNodeFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L641) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate 856b678134bab474c23aa4ec9a7647c1ca6991090d4dae546c0e7df6def1a8c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.Space f5f6cc04a4a873e713526441c96728a776c95452f0ee39f8708d19482294a99d -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.CentralNode 051e3ee7d115d89414ad4bb60ece189813df109518c2e2e0159c774eb03b3999 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoTabBar 8868c588820999686fa2168547f1ae9a1851c76d8e5b7f04c46f24e21cc84333 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.HiddenTabBar 270ee4a90eff712046a43e59f57174e167f1164e769252f21519a0ba87c8b9b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoWindowMenuButton f426df40de2883d6b7367fbdd5de9e1c6fe9c453db82e40c2edbfdbb4e93aeb3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoCloseButton 5179adf36fbde6250b1f57449719b5a87bd2cdfd3c233992cdd429dfad0f1ab4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoResizeX 46eeb3f8609f38b168196c590991935a9af458b742150de0ab21d0232c3bf460 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoResizeY 59a4ac1c4d5404ec95f01dd1380444e2cc5c0971d56da1e8f452a9c1b455c0ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.DockedWindowsInFocusRoute 7484140a6673dc4950c08863934efa82536155efe784eda3f4e08ab99f96d45b -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingSplitOther ab1366cb21afedcc7cd98a5672daf774eeaf363985ebdb0ca4ad7f159838db4a -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingOverMe 9a1c9d9896f29e1f82d6718174240e0c9a934c962d10610e8d3b16fe230351de -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingOverOther 69ce8030034e60fea87504fae98e6ae4151ed2b61f0c0fd7db79cb89cae0bb53 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingOverEmpty 74e7c45b175d3faec0e259d4cf989c28deb8135fdcfb2a58fe239ad28d2a5606 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDocking 58f9cc5ab73b4b4ba69e4038588c6ecf64445859277dfc5db16a2a396a3e035f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.SharedFlagsInheritMask b1601cd309daff2eb7a4b061ff34c160063f7eabb91c48797ba50e2e19d26f78 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoResizeFlagsMask f75d5afd92f292522df74720216b724cb9479f1e8cc1058a2eca4d09dbda9636 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.LocalFlagsTransferMask 30bb5349f13195a706edd246b9a599437041e79c836fc3574066d1d3df9ba56f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.SavedFlagsMask 38a6d7cc1c4f0d2aeb7b10a62c9e1a1c500a13a1f54febb5f7164e5595d3c848 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-aa08bb3811330f67"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.CentralNode = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1606) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-410624b248e03810"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.DockedWindowsInFocusRoute = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1613) | 当前常量或枚举值：`262144`。 | — |
| <a id="api-31467c4fbd4c0636"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.HiddenTabBar = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1608) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-1350f8fb3f915a37"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.LocalFlagsTransferMask = 260208`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1621) | 当前常量或枚举值：`260208`。 | — |
| <a id="api-2062a72ccaf91c6f"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoCloseButton = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1610) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-0570c016111719f6"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDocking = 7864336`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1618) | 当前常量或枚举值：`7864336`。 | — |
| <a id="api-31bf3e293fe1cac8"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingOverEmpty = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1617) | 当前常量或枚举值：`4194304`。 | — |
| <a id="api-e2a369798e731db6"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingOverMe = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1615) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-2d7b185044398799"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingOverOther = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1616) | 当前常量或枚举值：`2097152`。 | — |
| <a id="api-589c6ef7f2c9406e"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoDockingSplitOther = 524288`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1614) | 当前常量或枚举值：`524288`。 | — |
| <a id="api-1e4dec7efd09c6c8"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoResizeFlagsMask = 196640`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1620) | 当前常量或枚举值：`196640`。 | — |
| <a id="api-5596bfbdc43ab9c5"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoResizeX = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1611) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-522d2e7c00b01acd"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoResizeY = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1612) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-19395a46f6301502"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoTabBar = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1607) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-f525167f49f8984f"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.NoWindowMenuButton = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1609) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-614c00740e165ef5"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.SavedFlagsMask = 261152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1622) | 当前常量或枚举值：`261152`。 | — |
| <a id="api-ce421bbe97f8b6d1"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.SharedFlagsInheritMask = -1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1619) | 当前常量或枚举值：`-1`。 | — |
| <a id="api-ee98bdbc1caba50a"></a>[`Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate.Space = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1605) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-eaf0c9c178a3554c"></a>[`public enum Inno.Native.ImGui.ImGuiDockNodeFlagsPrivate : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1603) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDockNodePtr</code> · 68 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDockNodePtr c25c6045be0aeaa10dad4d0b4b9a743c739c436c11c172bf6a5878eb0bb2e29a -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.#ctor(Inno.Native.ImGui.ImGuiDockNode*) 594862f6adba9ff101129674b0e7e6e40bce8935df7d8f5a587a53b086561e27 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodePtr.Handle c33a1ace0bfe4d2094955d88d67e542448f040637e7f9164d922bdbad2d1022c -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.IsNull 5dc1585289d1b5bc764cfc397c34e6e8ce1f36d694c2c286114a11c35d1048ad -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.Null 901adc4db79f3e43d4b3e5a7d80b9d65c3bd6c2d0e554b79536e55917a7611ad -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.Item(System.Int32) cf82553f7fd3884a0fa7ebfdd05fcc388d5bd8e2704feac864965ef220f96c1f -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.op_Implicit(Inno.Native.ImGui.ImGuiDockNode*)~Inno.Native.ImGui.ImGuiDockNodePtr 799f09e5e855faf3bd5de06fa6a30df0473a1a3bb229cea19bbd3b6134e619bb -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.op_Implicit(Inno.Native.ImGui.ImGuiDockNodePtr)~Inno.Native.ImGui.ImGuiDockNode* 30eda603bab510b22bb3d610d2bde862031cf14674ad57a462bc8d666a7c2dd6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.op_Equality(Inno.Native.ImGui.ImGuiDockNodePtr,Inno.Native.ImGui.ImGuiDockNodePtr) dcfba4f21601d2c922f55edd84a1b58837deaa81bc5c9ca00956132389222c14 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.op_Inequality(Inno.Native.ImGui.ImGuiDockNodePtr,Inno.Native.ImGui.ImGuiDockNodePtr) 86e858cd3a986f673d3b4b2d9cb8e87927829cf66533537afdd07f431b1e14c8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.Equals(Inno.Native.ImGui.ImGuiDockNodePtr) d89cd5cf950c990c62d03cb63e1d4aa15e86e503297d32e87f23e6d46cf7a149 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.Equals(System.Object) ee7fd1dd4083009861d9b3f7b588e47e4cdd106943e9963eb2e037be7111c414 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.GetHashCode 3b1c7981385b9781ebf044759f202778d880d7217cbc39eba7788a233e7c0302 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.ID 1adb19a5c2469cd15c70de6d88338d76c45ee9b7e4af1bf98672a18669c87ca7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.SharedFlags 46291f980c15f6525c94ee514f6d23ca62b0d459705a168feaed67a216885a63 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.LocalFlags f26c33e6824b2417eed0010761fe18bf202f9846ef3fc046e6a5ebbf898269c7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.LocalFlagsInWindows 43323411f39994f643188e0b7d7a0edf6c1757e2b582618d8595888307830029 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.MergedFlags 9da108451bf1a3fb8105de3f9e94184b2936f2492dc678f94584b7c7e82656a8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.State 34e2e469ca0b1d8a579c356a507c94a255f4deedd0e0cf4017807abc117fd45d -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.ParentNode 20fdb5a1e67136bccc113e767226673cb4806f17a4a8b5d574680b9634c6e14d -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.ChildNodes aac170c411fb27d8eada2b6c9a8472055d1c7d9275a1576c2dde16c7b2f5c966 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.Windows faa907e572a72dfe0b294f2bcd420d23b31fa731fb47010544530f04c0fe5330 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.TabBar 33757274917853c2f65a42dacdc39a28eae825c67ee4fdc9917ee4b8c3bd4947 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.Pos 95a56b5de3525e7d91dcaf988d528b8745252c04bf8da2a544d0f1b5226beb1b -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.Size ee3f98fcf1473e81cfc34b3d51a25edeaaf399874cac698ab7af1d1c533fff11 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.SizeRef 21648500131d4f4556f059ec939b4d66ca41ed091cd44a9e7d886326bc7954e2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.SplitAxis e691a2127646effffa6a6fb2ed8968a01f98e0ba6346d22e4839bd6ad8c65c21 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.WindowClass 9d75ed39413b06db0a443b4509ed688aaba6524acc1df5d4d4940a7ef0b6b893 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.LastBgColor a934c5158bacd91bfc3b647b0c422a279729b43dc39cf6c1795c83b54646de9d -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.HostWindow 2f0d060f80dd93a0d0cb3f709d4926b3af475f1e06c0ec7622449742c4ebbe60 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.VisibleWindow 8f1a75dd0cd8b1a11eb14d4af97f502529c37ae85ddb9a4345bb128624ca7ded -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.CentralNode 6601923a03e8c8aabc20bd8017378c24a288cac3632b7aa1c44fd4eeb9843fcc -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.OnlyNodeWithWindows 5c2348f46beae622ce6b8ce4c07bd512dc13718d6f44c07d8df68ac1c1e0204c -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.CountNodeWithWindows 8d7ff228d98b64ea333bdc1326693a35d875864153372281f6feb3ba13cb4399 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.LastFrameAlive 31bbad105e2efe45d2003369ebc587e197fd381059c728ec9bca7229b464bf1d -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.LastFrameActive 3e4db12d36eb867e9eb955de718b8cce495f7ea3a7a44f2223e7e7a010c12ad1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.LastFrameFocused 53fae86c8b9108f41a16db06c4ef00d1b6dbdb17156f851930cd5abf30db18a9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.LastFocusedNodeId f5c201c171699d769b43070eee2c65109eee5b065597bdffbc077e61873719d1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.SelectedTabId e7655aede540c0472949ed52fac8bb92600820b5bfdeabb4cb30450e30d4a324 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.WantCloseTabId e8d67de34fa807858ddda1922baa7e150907e1d5685d33e7ee85eb2324c20693 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.RefViewportId 11a149cd3dfcee671ad38a1b19d7c3da4aa994c9f24fc4d0fff374fb9c9b1257 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.AuthorityForPos 8da88e2b12aa5898e598e71b7ebfcffa63743612f135fc21f4335d1ee10d02b0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.AuthorityForSize 05d2234a212e4c4aff3e31bba53f77083663b6bde380444346eb6a5acc3650f4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.AuthorityForViewport bdabfc0b7a85b3123d0b7a5f2071b53c24bb5357f07e0eba83a29d2611e9afef -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.IsVisible 92facf5c5e5349d7e0de292c3473d2d6678d0fbaa7cd1f7e18bca4e7b0e5b93c -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.IsFocused af859fe685093b1cdd37c37e8d1c51fcc8480bd4c38be22846ff279f565a89db -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.IsBgDrawnThisFrame f1cf442b7ea8e4402eb4aca4fc12626fe7648ff2049f41ebda41c10d5aede3d0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.HasCloseButton 5e37807c7937836e377147c691a552ada190d5ac372b2ee977d683f537f8e927 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.HasWindowMenuButton 1889c02d8a751aaac49e67a9753927da1b6d130462b59bee81b8cbdc4c85470e -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.HasCentralNodeChild 53dffada2e3724d8c58b34a35abc29cb1fae5db2e6faa7d2a9095c88f002c5a7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.WantCloseAll 41e2883ce6c0ee3d4f458299e33eb50385420571e12c2d76df30c72e9ea503ee -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.WantLockSizeOnce e966f17cc8d5383dd92be9f0686b694e8d576414f42644f34f6d1656b6ae8608 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.WantMouseMove 0290a2a2106bb531be73c051936a45f8a5c6e0b402e640c515ddd48eaa865cdd -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.WantHiddenTabBarUpdate ea89ce493e346a3e14a0355dd8d0745d130b5e9621cb061147246792ab6218f5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodePtr.WantHiddenTabBarToggle 9af43143dc6cfeb39f0f5961144465ea0fa553d9989175ecae35b6bf4fd5dd04 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.Destroy 8cdc90a2698e64675e04514b70c1ecd7403ee1de23e3fc3383f2e4c4ee60f13e -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.IsRootNode 1cef0e12e12eb79e6caed20069ede158b6e392d1a8904c8e2c32933e7b2e1a91 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.IsDockSpace 8c8004aaa8e094bff9aa4701c8c2aaeeb4e14626f9e70079f4459f62a075a586 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.IsFloatingNode 79f82d9fc090fc10bb02e03624dfc17dcb3a5cf14538dd5d44996aeb7d629fc9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.IsCentralNode 67c59df871a56cd0b7eb1e4f6c393c16880b3e793837dac58ba3f4c4617e49e2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.IsHiddenTabBar c2fa5ca3b25138c523c4400e84e9f451de96d5812f0ba408d77730aba52354f4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.IsNoTabBar 3712fd58aa97a080838d2d876cdee21873f0cc96f4948cd35d6550c6e4fdc7df -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.IsSplitNode 308f69321b9227f74f8027c1959f28b93e3bda6c0315e27909b2c01d8600ce82 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.IsLeafNode 58c5c55e8ff904ee1f7b1c905d7664bfbd90ebecb960b15f3804167cbff1a177 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.IsEmpty 175473e97d978c5d4069c4f697ce99219c5acc660c68c2bb5b0e0869e3dbe635 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.Rect 1dd41fca5ef0d0eed96b3202a7e01dc578dde25ab57195157872a07b4a7335eb -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.SetLocalFlags(Inno.Native.ImGui.ImGuiDockNodeFlags) 2fa93414adc9c2c30480ac7a8527e9a1e912e49d13c97366deea09a1c029a33b -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodePtr.UpdateMergedFlags fac3bb1eb025dcaca8d7b9ac484400b64d19b9ec1e34aa12087ecf2a5ebb9b46 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4d2fd412b0e8ef5d"></a>[`public Inno.Native.ImGui.ImGuiDockNode* Inno.Native.ImGui.ImGuiDockNodePtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13923) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9076b33623eb0a2"></a>[`public Inno.Native.ImGui.ImGuiDockNodePtr.ImGuiDockNodePtr(Inno.Native.ImGui.ImGuiDockNode* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13918) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bb6acb1a5a74db22"></a>[`public void Inno.Native.ImGui.ImGuiDockNodePtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13983) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-56192cf9c02c2628"></a>[`public bool Inno.Native.ImGui.ImGuiDockNodePtr.Equals(Inno.Native.ImGui.ImGuiDockNodePtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13933) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-fef024f31a09d941"></a>[`public override bool Inno.Native.ImGui.ImGuiDockNodePtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13934) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-6fca9c7fd5a46138"></a>[`public override int Inno.Native.ImGui.ImGuiDockNodePtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13935) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-4682123cb4ea6e81"></a>[`public bool Inno.Native.ImGui.ImGuiDockNodePtr.IsCentralNode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14015) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-954a202421e89891"></a>[`public bool Inno.Native.ImGui.ImGuiDockNodePtr.IsDockSpace()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13999) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-16f072e492b7ffe0"></a>[`public bool Inno.Native.ImGui.ImGuiDockNodePtr.IsEmpty()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14055) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6b824107b7916bc3"></a>[`public bool Inno.Native.ImGui.ImGuiDockNodePtr.IsFloatingNode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14007) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7a838a0aefdd2bc6"></a>[`public bool Inno.Native.ImGui.ImGuiDockNodePtr.IsHiddenTabBar()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14023) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cec7a1fe5c1aca53"></a>[`public bool Inno.Native.ImGui.ImGuiDockNodePtr.IsLeafNode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14047) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-356453196198f978"></a>[`public bool Inno.Native.ImGui.ImGuiDockNodePtr.IsNoTabBar()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14031) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0639ad9da4ff8d9d"></a>[`public bool Inno.Native.ImGui.ImGuiDockNodePtr.IsRootNode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13991) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-138bc9fde35018fb"></a>[`public bool Inno.Native.ImGui.ImGuiDockNodePtr.IsSplitNode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14039) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-99ad1316680d957f"></a>[`public static bool Inno.Native.ImGui.ImGuiDockNodePtr.operator ==(Inno.Native.ImGui.ImGuiDockNodePtr left, Inno.Native.ImGui.ImGuiDockNodePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13931) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0495061e4cc67538"></a>[`public static Inno.Native.ImGui.ImGuiDockNodePtr.implicit operator Inno.Native.ImGui.ImGuiDockNodePtr(Inno.Native.ImGui.ImGuiDockNode* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13929) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bad0b0fb30d3c6d6"></a>[`public static Inno.Native.ImGui.ImGuiDockNodePtr.implicit operator Inno.Native.ImGui.ImGuiDockNode*(Inno.Native.ImGui.ImGuiDockNodePtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13930) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fc166001d1ccf81a"></a>[`public static bool Inno.Native.ImGui.ImGuiDockNodePtr.operator !=(Inno.Native.ImGui.ImGuiDockNodePtr left, Inno.Native.ImGui.ImGuiDockNodePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13932) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6fcf367b84607b4c"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiDockNodePtr.Rect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14063) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7a39b0c170f3f0a0"></a>[`public void Inno.Native.ImGui.ImGuiDockNodePtr.SetLocalFlags(Inno.Native.ImGui.ImGuiDockNodeFlags flags)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14071) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-98c466ba92765113"></a>[`public void Inno.Native.ImGui.ImGuiDockNodePtr.UpdateMergedFlags()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14079) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d37e96fcfe268406"></a>[`public Inno.Native.ImGui.ImGuiDataAuthority Inno.Native.ImGui.ImGuiDockNodePtr.AuthorityForPos { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13965) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dcd756cb7e6e0c32"></a>[`public Inno.Native.ImGui.ImGuiDataAuthority Inno.Native.ImGui.ImGuiDockNodePtr.AuthorityForSize { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13966) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ab7c4bc3f816aefd"></a>[`public Inno.Native.ImGui.ImGuiDataAuthority Inno.Native.ImGui.ImGuiDockNodePtr.AuthorityForViewport { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13967) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-56fe6317f04e9cf1"></a>[`public ref Inno.Native.ImGui.ImGuiDockNodePtr Inno.Native.ImGui.ImGuiDockNodePtr.CentralNode { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13955) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8a256d97445ecbc1"></a>[`public System.Span<BGCS.Runtime.Pointer<Inno.Native.ImGui.ImGuiDockNode>> Inno.Native.ImGui.ImGuiDockNodePtr.ChildNodes { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13944) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba3bb477cf48a1b5"></a>[`public ref int Inno.Native.ImGui.ImGuiDockNodePtr.CountNodeWithWindows { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13957) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-550199becd8b7395"></a>[`public byte Inno.Native.ImGui.ImGuiDockNodePtr.HasCentralNodeChild { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13973) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-224535d4d3ec6ea6"></a>[`public byte Inno.Native.ImGui.ImGuiDockNodePtr.HasCloseButton { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13971) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a4a254371b310879"></a>[`public byte Inno.Native.ImGui.ImGuiDockNodePtr.HasWindowMenuButton { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13972) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f93380c8881e3bf5"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiDockNodePtr.HostWindow { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13953) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-54ba357f45de9d29"></a>[`public ref uint Inno.Native.ImGui.ImGuiDockNodePtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13937) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-70a352a38810779e"></a>[`public byte Inno.Native.ImGui.ImGuiDockNodePtr.IsBgDrawnThisFrame { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13970) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-854e366b4bdab6dd"></a>[`public byte Inno.Native.ImGui.ImGuiDockNodePtr.IsFocused { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13969) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ac12d05f07b296f1"></a>[`public bool Inno.Native.ImGui.ImGuiDockNodePtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13924) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ff2820e4e822223c"></a>[`public byte Inno.Native.ImGui.ImGuiDockNodePtr.IsVisible { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13968) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9b465efebe77b02b"></a>[`public Inno.Native.ImGui.ImGuiDockNode Inno.Native.ImGui.ImGuiDockNodePtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13927) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-289c8b78c900e6d3"></a>[`public ref uint Inno.Native.ImGui.ImGuiDockNodePtr.LastBgColor { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13952) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b303dba68cd750e0"></a>[`public ref uint Inno.Native.ImGui.ImGuiDockNodePtr.LastFocusedNodeId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13961) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9de29d674ff361de"></a>[`public ref int Inno.Native.ImGui.ImGuiDockNodePtr.LastFrameActive { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13959) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-079894df8a629683"></a>[`public ref int Inno.Native.ImGui.ImGuiDockNodePtr.LastFrameAlive { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13958) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1147ac90fb01dfaf"></a>[`public ref int Inno.Native.ImGui.ImGuiDockNodePtr.LastFrameFocused { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13960) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-de3a1ebecb3627a4"></a>[`public ref Inno.Native.ImGui.ImGuiDockNodeFlags Inno.Native.ImGui.ImGuiDockNodePtr.LocalFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13939) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c7f0cdb8f507c771"></a>[`public ref Inno.Native.ImGui.ImGuiDockNodeFlags Inno.Native.ImGui.ImGuiDockNodePtr.LocalFlagsInWindows { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13940) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-517bb464a895190f"></a>[`public ref Inno.Native.ImGui.ImGuiDockNodeFlags Inno.Native.ImGui.ImGuiDockNodePtr.MergedFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13941) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9727daf82336076c"></a>[`public static Inno.Native.ImGui.ImGuiDockNodePtr Inno.Native.ImGui.ImGuiDockNodePtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13925) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-901c9d1568cc0fe3"></a>[`public ref Inno.Native.ImGui.ImGuiDockNodePtr Inno.Native.ImGui.ImGuiDockNodePtr.OnlyNodeWithWindows { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13956) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b69ebeaaa10639cc"></a>[`public ref Inno.Native.ImGui.ImGuiDockNodePtr Inno.Native.ImGui.ImGuiDockNodePtr.ParentNode { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13943) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7161b61c1f2de9fd"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiDockNodePtr.Pos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13947) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-54eeae52aff7d99f"></a>[`public ref uint Inno.Native.ImGui.ImGuiDockNodePtr.RefViewportId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13964) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c7b983d1bae4ecf5"></a>[`public ref uint Inno.Native.ImGui.ImGuiDockNodePtr.SelectedTabId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13962) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6f685a5357d28fd1"></a>[`public ref Inno.Native.ImGui.ImGuiDockNodeFlags Inno.Native.ImGui.ImGuiDockNodePtr.SharedFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13938) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9cbd94f4b0ef4a9d"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiDockNodePtr.Size { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13948) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-39b043bd214c618d"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiDockNodePtr.SizeRef { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13949) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a161f659cfe1e39b"></a>[`public ref Inno.Native.ImGui.ImGuiAxis Inno.Native.ImGui.ImGuiDockNodePtr.SplitAxis { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13950) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c0a41aa99753c954"></a>[`public ref Inno.Native.ImGui.ImGuiDockNodeState Inno.Native.ImGui.ImGuiDockNodePtr.State { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13942) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b2736ada483dfaf6"></a>[`public ref Inno.Native.ImGui.ImGuiTabBarPtr Inno.Native.ImGui.ImGuiDockNodePtr.TabBar { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13946) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5bdd425fa4661171"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiDockNodePtr.VisibleWindow { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13954) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fe993c6e1493e628"></a>[`public byte Inno.Native.ImGui.ImGuiDockNodePtr.WantCloseAll { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13974) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bb19340755a864c4"></a>[`public ref uint Inno.Native.ImGui.ImGuiDockNodePtr.WantCloseTabId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13963) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-45b95213dac2cc5b"></a>[`public byte Inno.Native.ImGui.ImGuiDockNodePtr.WantHiddenTabBarToggle { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13978) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0bd08f87e8cbe804"></a>[`public byte Inno.Native.ImGui.ImGuiDockNodePtr.WantHiddenTabBarUpdate { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13977) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b775b3986a73602d"></a>[`public byte Inno.Native.ImGui.ImGuiDockNodePtr.WantLockSizeOnce { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13975) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bc6c64fc22e34171"></a>[`public byte Inno.Native.ImGui.ImGuiDockNodePtr.WantMouseMove { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13976) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2d1daca65842db3b"></a>[`public ref Inno.Native.ImGui.ImGuiWindowClass Inno.Native.ImGui.ImGuiDockNodePtr.WindowClass { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13951) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4fe926c63fa847df"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiWindowPtr> Inno.Native.ImGui.ImGuiDockNodePtr.Windows { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13945) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5f8c089a61536607"></a>[`public struct Inno.Native.ImGui.ImGuiDockNodePtr : System.IEquatable<Inno.Native.ImGui.ImGuiDockNodePtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L13916) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDockNodeSettings</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDockNodeSettings 73a1adf64408d0a9dead5307b6448697ac15bcf459f672fb2acf7af432cdd7ad -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodeSettings.#ctor(System.IntPtr) e4ebec583c6364d61c2f3c597e705505f6b19fdf64b4804f5b15c3150a98e5b6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodeSettings.Handle 26c7fc1e8c75d7e09d83a7b0899a59377a855583449d32dcac034a4d1e4cc4a1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodeSettings.IsNull 0bea7eca0b84163976f1d062f437f639a8f90275b823ac4fb30a17b6923d2bff -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockNodeSettings.Null 5a40a7113c1b89a713c97b4941f1a78733fd2953ca3fdf524a7b0436501bb079 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodeSettings.op_Implicit(System.IntPtr)~Inno.Native.ImGui.ImGuiDockNodeSettings 5fc898655e512825dccb66db21031b10e5c4e34c28e8401ad100d21ce01b9537 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodeSettings.op_Equality(Inno.Native.ImGui.ImGuiDockNodeSettings,Inno.Native.ImGui.ImGuiDockNodeSettings) a48f8128db5be2c384aa5fa315e31bffd357d852f304a1bb20f5b9c50e7ee2fb -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodeSettings.op_Inequality(Inno.Native.ImGui.ImGuiDockNodeSettings,Inno.Native.ImGui.ImGuiDockNodeSettings) 6cd6511c637c7163431a3dacb4ea3449b3accaf061b225d1a09fe151839e0d59 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodeSettings.Equals(Inno.Native.ImGui.ImGuiDockNodeSettings) 06e1c4d3b1e044df24a230674a2fd9e39ecb5beae5b91017ce587f73b0a88590 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodeSettings.Equals(System.Object) af704bdc750b3ebbcffa1369c127748dde56675c38059c947647020f28e3f76d -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockNodeSettings.GetHashCode 8e2e59f35c09585d5710c0a916965a16ffcc0cf8c50458a8a939b6d3c6a64e30 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-59ac852243ce3f67"></a>[`public Inno.Native.ImGui.ImGuiDockNodeSettings.ImGuiDockNodeSettings(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7323) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7b1d2a7aa579e8dd"></a>[`public bool Inno.Native.ImGui.ImGuiDockNodeSettings.Equals(Inno.Native.ImGui.ImGuiDockNodeSettings other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7335) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-4defa7d8fcee986e"></a>[`public override bool Inno.Native.ImGui.ImGuiDockNodeSettings.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7336) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-2157ce543ae05bc3"></a>[`public override int Inno.Native.ImGui.ImGuiDockNodeSettings.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7337) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-cc151e2e20337c83"></a>[`public static bool Inno.Native.ImGui.ImGuiDockNodeSettings.operator ==(Inno.Native.ImGui.ImGuiDockNodeSettings left, Inno.Native.ImGui.ImGuiDockNodeSettings right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7333) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a548fa114ca8ae0c"></a>[`public static Inno.Native.ImGui.ImGuiDockNodeSettings.implicit operator Inno.Native.ImGui.ImGuiDockNodeSettings(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7332) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f57c4a193409acc5"></a>[`public static bool Inno.Native.ImGui.ImGuiDockNodeSettings.operator !=(Inno.Native.ImGui.ImGuiDockNodeSettings left, Inno.Native.ImGui.ImGuiDockNodeSettings right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7334) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e8d2b8260d8307f"></a>[`public nint Inno.Native.ImGui.ImGuiDockNodeSettings.Handle { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7328) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8a8b782cb5fc9757"></a>[`public bool Inno.Native.ImGui.ImGuiDockNodeSettings.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7329) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-57221bfcb5c05c50"></a>[`public static Inno.Native.ImGui.ImGuiDockNodeSettings Inno.Native.ImGui.ImGuiDockNodeSettings.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7330) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8cf3cc9464797eff"></a>[`public readonly struct Inno.Native.ImGui.ImGuiDockNodeSettings : System.IEquatable<Inno.Native.ImGui.ImGuiDockNodeSettings>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7321) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDockNodeState</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDockNodeState 96d633645a65ab623fb53c189f17ab4e1cb8900043ede39440ad6ab9e2efd3ee -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeState.Unknown 71d42f4227d3e5f6ebdbcd97cd4861b9860d366cfb22f46635d40c3b2c553ba7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeState.HostWindowHiddenBecauseSingleWindow 3587b05420f89296c80b9ee61d161c7c1b4704c9574538452260e545b77dd7d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeState.HostWindowHiddenBecauseWindowsAreResizing bc4cca0095703b1b1e1a11992c96424591d58741ec1e094c6856c60dd87cf06c -->
<!-- api: F:Inno.Native.ImGui.ImGuiDockNodeState.HostWindowVisible 7e8e9ae6febd17e3d242a405b5d745e5218238c4cea927604acb14b9212f71cb -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ea8a3c54424b2084"></a>[`Inno.Native.ImGui.ImGuiDockNodeState.HostWindowHiddenBecauseSingleWindow = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L305) | 当前常量或枚举值：`1`。 | — |
| <a id="api-68d83e6823e1e0e5"></a>[`Inno.Native.ImGui.ImGuiDockNodeState.HostWindowHiddenBecauseWindowsAreResizing = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L306) | 当前常量或枚举值：`2`。 | — |
| <a id="api-6f4d5cfe622b753d"></a>[`Inno.Native.ImGui.ImGuiDockNodeState.HostWindowVisible = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L307) | 当前常量或枚举值：`3`。 | — |
| <a id="api-ea41da876a87f544"></a>[`Inno.Native.ImGui.ImGuiDockNodeState.Unknown = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L304) | 当前常量或枚举值：`0`。 | — |
| <a id="api-1bfa5d5fb2957758"></a>[`public enum Inno.Native.ImGui.ImGuiDockNodeState : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L302) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDockRequest</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDockRequest ce29c09e03d8391bd1206cd630ad931ce0a861b3d043ef7bd396d0e6ba2115fa -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockRequest.#ctor(System.IntPtr) 417d309e2049c03767c916deb77e058ad2629b9a27d5c04b384e72a817cf76cb -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockRequest.Handle 2445a91b1d69d2e7d1e0e425809bd3d27d56264ef76a1c450f035cb7d9407df6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockRequest.IsNull 88841fe56ab3f49c6fdd79390102a19ddf04dff51799fcb0632765a45f29c076 -->
<!-- api: P:Inno.Native.ImGui.ImGuiDockRequest.Null 29ce89bb7201977cbd9157c72d01deed78f2d9f6faf4f6071f73ea6002dbb7e6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockRequest.op_Implicit(System.IntPtr)~Inno.Native.ImGui.ImGuiDockRequest e947f253a2058677b935dd41737716312c90a34dfac0ca36c43bdb0350c2c63e -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockRequest.op_Equality(Inno.Native.ImGui.ImGuiDockRequest,Inno.Native.ImGui.ImGuiDockRequest) 2b6b64e5dfe1edb6bb6d996d9eadf0be4a9217d11114a4bb71e55114d888ca3f -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockRequest.op_Inequality(Inno.Native.ImGui.ImGuiDockRequest,Inno.Native.ImGui.ImGuiDockRequest) 5a3bfc1547640d449dd0ed99911a4f973f05681382f290a4190b316e7eeba491 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockRequest.Equals(Inno.Native.ImGui.ImGuiDockRequest) 2b8af09f4e0b7a8cffeba6e6be1ff7fa68b51c736a6ab2393967dd43b2af69d6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockRequest.Equals(System.Object) 4a0b1d213e5bfa433f07cc50ded5f2641064e3b140a97847455617b6d4a66a4a -->
<!-- api: M:Inno.Native.ImGui.ImGuiDockRequest.GetHashCode 4e72f0ba20705e5e399d8bb3ecbd678b21af0c1aa8d06a38969c9b301cbadf2e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-66bf986f9bede7a0"></a>[`public Inno.Native.ImGui.ImGuiDockRequest.ImGuiDockRequest(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7299) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3eb2a1cd1faca2c6"></a>[`public bool Inno.Native.ImGui.ImGuiDockRequest.Equals(Inno.Native.ImGui.ImGuiDockRequest other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7311) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-51c611b232d5253e"></a>[`public override bool Inno.Native.ImGui.ImGuiDockRequest.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7312) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-d646dcf43289d58d"></a>[`public override int Inno.Native.ImGui.ImGuiDockRequest.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7313) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-4fb75ad29e20822e"></a>[`public static bool Inno.Native.ImGui.ImGuiDockRequest.operator ==(Inno.Native.ImGui.ImGuiDockRequest left, Inno.Native.ImGui.ImGuiDockRequest right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7309) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b3de32165ec99a6a"></a>[`public static Inno.Native.ImGui.ImGuiDockRequest.implicit operator Inno.Native.ImGui.ImGuiDockRequest(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7308) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-27931886adf75d6a"></a>[`public static bool Inno.Native.ImGui.ImGuiDockRequest.operator !=(Inno.Native.ImGui.ImGuiDockRequest left, Inno.Native.ImGui.ImGuiDockRequest right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7310) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-85eccc33a5d90f61"></a>[`public nint Inno.Native.ImGui.ImGuiDockRequest.Handle { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7304) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b9b5a6c13f75f10c"></a>[`public bool Inno.Native.ImGui.ImGuiDockRequest.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7305) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-92738928c2eb335b"></a>[`public static Inno.Native.ImGui.ImGuiDockRequest Inno.Native.ImGui.ImGuiDockRequest.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7306) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-42347209a40cf6fe"></a>[`public readonly struct Inno.Native.ImGui.ImGuiDockRequest : System.IEquatable<Inno.Native.ImGui.ImGuiDockRequest>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7297) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiDragDropFlags</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiDragDropFlags 0f1aa8fd33e4c50eb926127dfef6b25b0ddab3bb5263a2a9891fb17c5c0d38ab -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.None d4fe042828fae51a4987425580d303b61d6c173a1c6e8ef0c33e22f190a41ad9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.SourceNoPreviewTooltip bb16204be45d1664683e035512d668d5f01fede8fa1c74dca5bb8442971299c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.SourceNoDisableHover 0c730bbc8f30596b315e93167409248ed57eb128be4d8c069389e3ff7ad8ee03 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.SourceNoHoldToOpenOthers 4d544d4686ebf9d915a905500d6b350afd8cffe219da4660320552231e62b0c9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.SourceAllowNullId 1d9ed25097edc818e386a6dcb9f5bb979ed9e3c43bcd240faa20106e994735ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.SourceExtern bd2f6bc5820a8d1f318bd24cdf514d10b59f473d1620640e489d47d770330b4b -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.PayloadAutoExpire 509ec95567dcb87e056c3a5bc8de211300cf0032130a28cb112712ade1193f88 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.PayloadNoCrossContext e41d3b47a41758df64004a31f0e38bb182f89268fd7c45259f7ec003e5bc1e37 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.PayloadNoCrossProcess 313eff03e12348146dbbe83a1c95a89dc353985a8d76c4a24b832b624f430e0d -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.AcceptBeforeDelivery 7afcd94396ded7a7da6f9b09d325e159f7a63593b4c00c6b1bfcd9723620c14f -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.AcceptNoDrawDefaultRect 6bbae35b30677839430cc75aeb014fba25b44d2676041a4f018f5fd15f1263bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.AcceptNoPreviewTooltip 6b282ec13c4eef0bd0d8be0f8dbdaaf60c26e09ec5cb3e8fcfcb2adc7b95d692 -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.AcceptDrawAsHovered e461d1650dad05ab82c0069845a8f84f409c544cb88f2a3cdacc474aff68b1af -->
<!-- api: F:Inno.Native.ImGui.ImGuiDragDropFlags.AcceptPeekOnly 8e00261b512ae1cea0227770757c57b3381ee9d7e22fcdbcbf5936c22e75b074 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f898b77e19d4eb63"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.AcceptBeforeDelivery = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L667) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-f5e7e48915eb9799"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.AcceptDrawAsHovered = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L670) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-101f0599cfd9a748"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.AcceptNoDrawDefaultRect = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L668) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-acde484a0972af2e"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.AcceptNoPreviewTooltip = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L669) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-10ef5dd79d0a1321"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.AcceptPeekOnly = 3072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L671) | 当前常量或枚举值：`3072`。 | — |
| <a id="api-d15145c419f10c2a"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L658) | 当前常量或枚举值：`0`。 | — |
| <a id="api-3f355eae0cb27ad2"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.PayloadAutoExpire = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L664) | 当前常量或枚举值：`32`。 | — |
| <a id="api-ca707c1e554cfa1e"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.PayloadNoCrossContext = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L665) | 当前常量或枚举值：`64`。 | — |
| <a id="api-0198b836614f22cd"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.PayloadNoCrossProcess = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L666) | 当前常量或枚举值：`128`。 | — |
| <a id="api-1e71d0b09cb05ded"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.SourceAllowNullId = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L662) | 当前常量或枚举值：`8`。 | — |
| <a id="api-1374667047c4a7d7"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.SourceExtern = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L663) | 当前常量或枚举值：`16`。 | — |
| <a id="api-3bf079b0d2834924"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.SourceNoDisableHover = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L660) | 当前常量或枚举值：`2`。 | — |
| <a id="api-5b2b11e02c68dce0"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.SourceNoHoldToOpenOthers = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L661) | 当前常量或枚举值：`4`。 | — |
| <a id="api-b8a5d66595f80be1"></a>[`Inno.Native.ImGui.ImGuiDragDropFlags.SourceNoPreviewTooltip = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L659) | 当前常量或枚举值：`1`。 | — |
| <a id="api-87468e8e0afb27e7"></a>[`public enum Inno.Native.ImGui.ImGuiDragDropFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L656) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiErrorCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiErrorCallback 6b1f1032437a6de91b6cca6b5accbed2b57faf8ec4b08f2168d3d573ded3bad7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorCallback.#ctor(System.Object,System.IntPtr) 007254f56bcf8b0f165ccd0291ba6e43f4d165d57289b6f871453367ade8eb6e -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorCallback.Invoke(Inno.Native.ImGui.ImGuiContext*,System.Void*,System.Byte*) f149f5ca68c89bbc67ca0e1d6d68c94bc6d41eb6113c7d77229dd173681e21ed -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorCallback.BeginInvoke(Inno.Native.ImGui.ImGuiContext*,System.Void*,System.Byte*,System.AsyncCallback,System.Object) 62bc41777f015f95730050fa60554bbfb8637c4a420398b153c262cb06d4c112 -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorCallback.EndInvoke(System.IAsyncResult) a62248d17d899e30bedffe22732e36235cc4ed75267922abfed114b6c9d75802 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-37c9935249af1478"></a>[`public Inno.Native.ImGui.ImGuiErrorCallback.ImGuiErrorCallback(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10628) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e6876bf83cfa1d08"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.ImGuiErrorCallback.BeginInvoke(Inno.Native.ImGui.ImGuiContext* ctx, void* userData, byte* msg, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10628) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b4d22ec79fe31c8a"></a>[`public virtual void Inno.Native.ImGui.ImGuiErrorCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10628) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9eaa0cc1b0e8a1df"></a>[`public virtual void Inno.Native.ImGui.ImGuiErrorCallback.Invoke(Inno.Native.ImGui.ImGuiContext* ctx, void* userData, byte* msg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10628) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-97ce415271bc054b"></a>[`public delegate void Inno.Native.ImGui.ImGuiErrorCallback(Inno.Native.ImGui.ImGuiContext* ctx, void* userData, byte* msg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10628) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiErrorRecoveryState</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiErrorRecoveryState 8048f34e558cc30073cb3c2e52fc64f289fd8c2fabfa0bfc599311d691c4976e -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfWindowStack 5e64af8c6f5a9e0f315cb2644a6c95e2f2fbe12532dc69bb2a1a96b5727d273c -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfIDStack 9ca08856cbc4505011a1917ba0581a92663222088e153568fbba2814159abc90 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfTreeStack 4bcf12c9750324e4814424c81817cfa90d308e7c597869bd78d2426cfeb89eb2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfColorStack e5cca104a5b5e6671d3988ca6d82028ea8934fcc94c6fe1c964c94ec8b1a0a2e -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfStyleVarStack d5c0129a5a3adf2a2df019a13f6bd88467bafc3ac71d3cf0defc904c7aae3144 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfFontStack 378088e7cb65918b536c06d4e3896a38ae9de7f782fd7ed8c8a20ef47691caf9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfFocusScopeStack 85fadf2c952d006abfa5b72be5c46899ab775a4e575e11193e54db323f76e0f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfGroupStack 88ccb2abd6e61cd628cc31fbac6d69340136946dd2222ff14f0b2ea690255a01 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfItemFlagsStack 6595bcb6788acecb4e01167f3145370d764c4e1c461b52af50073a5a77837355 -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfBeginPopupStack c8eca3a1843e99f919a2d665abae9c39c101e8e6d74a6a811fb27e70738dfa2f -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfDisabledStack 94e9cb8891adb3df232a673ec916ca26d9ca88a8da807fd98968ddfc5d7b9a81 -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorRecoveryState.#ctor(System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16) 6022c39ca4dcfac1c8f21ab3012e5169dda543f4a9418ad2628991846832741b -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorRecoveryState.Destroy c23c7605a3fc16ceed51589cf148fad08316e160538de721d99d989e46ce3705 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f02cf74adfa050c5"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfBeginPopupStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5069) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c90d803f289c9924"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfColorStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5063) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b11d46f800d9ccb"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfDisabledStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5070) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fcadac409cb2a22f"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfFocusScopeStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5066) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-914619ca46481864"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfFontStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5065) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e276f1ed5189f152"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfGroupStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5067) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a1987ad74824aee"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfIDStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5061) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9bd83aa72cb80646"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfItemFlagsStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5068) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-511412a9b16c4f07"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfStyleVarStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5064) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf68557ca5e847be"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfTreeStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5062) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f1e48a8c8b24b48"></a>[`public short Inno.Native.ImGui.ImGuiErrorRecoveryState.SizeOfWindowStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5060) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4b9d79803c47eee9"></a>[`public Inno.Native.ImGui.ImGuiErrorRecoveryState.ImGuiErrorRecoveryState(short sizeOfWindowStack = 0, short sizeOfIDStack = 0, short sizeOfTreeStack = 0, short sizeOfColorStack = 0, short sizeOfStyleVarStack = 0, short sizeOfFontStack = 0, short sizeOfFocusScopeStack = 0, short sizeOfGroupStack = 0, short sizeOfItemFlagsStack = 0, short sizeOfBeginPopupStack = 0, short sizeOfDisabledStack = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5071) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bae0b77c344f9ee6"></a>[`public void Inno.Native.ImGui.ImGuiErrorRecoveryState.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66795) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4c5df669aad96dbd"></a>[`public struct Inno.Native.ImGui.ImGuiErrorRecoveryState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5058) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr</code> · 25 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr 3e29c6a6696b21fd7c135d08d2e2fb1a6aadb386ff31bacc492c516a1e83b995 -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.#ctor(Inno.Native.ImGui.ImGuiErrorRecoveryState*) e5c8a2637e0535798ccde3a2679a925eb49429674206ebddaf8d3f9e906befdd -->
<!-- api: F:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.Handle 26d69a20ca90c79fabba17ba36589cf902e3f96d249130b7cf626fe6a9016066 -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.IsNull b518d396a9034d887882121c75022b596addee5c72fe6f36f2a90e6bb1508a86 -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.Null c4203f21c7048dbffa04f1aa3f79e256468b091b3e3a572312fd862226e30af9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.Item(System.Int32) 307b59bfa09b7d1d676086238749d40ed5fb4d2179f4028e59aadb1bc394546d -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.op_Implicit(Inno.Native.ImGui.ImGuiErrorRecoveryState*)~Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr 2c0518ef2679552415abb7961ef4a4923051f681b5a9ac2d1be9a6f43838c2fc -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.op_Implicit(Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr)~Inno.Native.ImGui.ImGuiErrorRecoveryState* 98b3dc04ee556c0d9dd70888fda3587b49357238b78caeff04fddea969cb830a -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.op_Equality(Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr,Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr) 6be89fd6167270ae8c289d9c8197a9bbbc5b93ed1f35daeee17a38b3eebd97ed -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.op_Inequality(Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr,Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr) 3784fe1358eabfc13e82b6aea8176010c0bfabf9fe4c93ae59dc7091994f0c2c -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.Equals(Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr) 21a02171a0934e5c67221671b7faa42581e5d40312973b82ef1a27503d4435ad -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.Equals(System.Object) 0be3b49d67c2c2b6f1d2d0180d7dc10a6a65bbd5606b0aed23fa487f558aec3a -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.GetHashCode 2b37326d887c10eeb523747246d78d4105b663f14a7c56b844cfe5f80b85869b -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfWindowStack 7a04fa718e3bebaa9185634491d1cbbdc410b659caa51a3eacdd4b114fe66697 -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfIDStack 1b884c30f6877bdebcdd77e50527c98a77fcc92ad5f51f86e7c9cf0f24a419a1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfTreeStack 676e3fe89095509b95df1e9eac3eeca3dfe3857db039aea3c8622c792522029f -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfColorStack decc8a9b09da4dce034a47d455d82547eafce172e7ac9a04370b15c22c614a87 -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfStyleVarStack ae0bc8dc1c84347fa9495c8142ea5ef7e7f71d5b8c662ab01277e0088659cff6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfFontStack 21714bae4e1e88311d9933575df1413c74fa827e924188c720971c0c9325b2b5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfFocusScopeStack fe122aad4d8d7eeb74b6965dad73be21d65e1366c327f160cc8f2794aa12ae6f -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfGroupStack 87cc711240585c3ddded959c5b92801a1110610f5c93ea9db5da7c303b66522d -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfItemFlagsStack 8f2340c7fd3f491c150cd40b36bbc39371a11964b53351fb4df9025cda992a58 -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfBeginPopupStack 66c01e138e98195100aa55b719b4bb16c20dc02d968c490ec31322784fb1a4f5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfDisabledStack 9c61473c442d9bc7e4a539a92bdbf265ad8bbbe76310fdf805d34995398de5f8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.Destroy c4ea588499680b49906a14869d22a3828e0e221bd5b5906c964731b863929ac6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fed5a873cd98b954"></a>[`public Inno.Native.ImGui.ImGuiErrorRecoveryState* Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14093) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-399d940a14a51a71"></a>[`public Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.ImGuiErrorRecoveryStatePtr(Inno.Native.ImGui.ImGuiErrorRecoveryState* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14088) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0aeebd8ebbbb717e"></a>[`public void Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14122) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-24e52ada0d8ac185"></a>[`public bool Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.Equals(Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14103) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-5464c574cef39def"></a>[`public override bool Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14104) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-b542d53bb62453e5"></a>[`public override int Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14105) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-805eaf74e169ef16"></a>[`public static bool Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.operator ==(Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr left, Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14101) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bec018574d9625cf"></a>[`public static Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.implicit operator Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr(Inno.Native.ImGui.ImGuiErrorRecoveryState* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14099) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-935d6b6647115f37"></a>[`public static Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.implicit operator Inno.Native.ImGui.ImGuiErrorRecoveryState*(Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14100) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c28e44cf14570cd3"></a>[`public static bool Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.operator !=(Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr left, Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14102) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ddbe998bb223c754"></a>[`public bool Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14094) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b15e160e8ceca89e"></a>[`public Inno.Native.ImGui.ImGuiErrorRecoveryState Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14097) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ddfd57693da27f21"></a>[`public static Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14095) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4f0e2318bdb78c30"></a>[`public ref short Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfBeginPopupStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14116) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a96d853c5ab73ad2"></a>[`public ref short Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfColorStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14110) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-710c806bf6f586c7"></a>[`public ref short Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfDisabledStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14117) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a43bc982f71cd22e"></a>[`public ref short Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfFocusScopeStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14113) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6b9d36fa30d8e301"></a>[`public ref short Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfFontStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14112) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b1472e5a1ea8c241"></a>[`public ref short Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfGroupStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14114) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c013545ac029fcd8"></a>[`public ref short Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfIDStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14108) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-04c84a02e4344aca"></a>[`public ref short Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfItemFlagsStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14115) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fedf37d64f96cf8b"></a>[`public ref short Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfStyleVarStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14111) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-31729b4a7d8ead0d"></a>[`public ref short Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfTreeStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14109) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3802fe2c1660753f"></a>[`public ref short Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr.SizeOfWindowStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14107) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a4aba9df0c8d0162"></a>[`public struct Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr : System.IEquatable<Inno.Native.ImGui.ImGuiErrorRecoveryStatePtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14086) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiFocusedFlags</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiFocusedFlags db2b6ab392600169d27631c51ce256de6d89650a1377c388d1c4ab81dce6d41c -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.None c3abb5dfd70e022fec5958a80cb89467de94108ef23959bf816b2e3d2c5dea4d -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.ChildWindows 531294df59105bfa6f8a656766a023101850dc917ffc2f58070f1c82bf5e3379 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.RootWindow d2a56705b0e4a13294e257ff61523da1324bfde99421d06be4791dd95b923cc2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.AnyWindow 5a5e4184da532c70d0236287b5944e0cca955a9d471918278cf1af059f9ba718 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.NoPopupHierarchy 980ebdb5fa8c39d9456b762dbda59f14f160aa0e47d92e2e3d38347790409c49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.DockHierarchy b9195f314ed30b9bdc2005d86df52a087236a966c4c37877bdaf0dbbedbecc26 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusedFlags.RootAndChildWindows e2d32f9ff08ce2bae0830e63f55ebb3c853934ed3ed89d192afe09d1508d504a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-beafb7259b651458"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.AnyWindow = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L604) | 当前常量或枚举值：`4`。 | — |
| <a id="api-2fd951a0e3cdd132"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.ChildWindows = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L602) | 当前常量或枚举值：`1`。 | — |
| <a id="api-ece639883333e74c"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.DockHierarchy = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L606) | 当前常量或枚举值：`16`。 | — |
| <a id="api-271a75417115f0f2"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L601) | 当前常量或枚举值：`0`。 | — |
| <a id="api-2cf1e4d8ad21d9e0"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.NoPopupHierarchy = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L605) | 当前常量或枚举值：`8`。 | — |
| <a id="api-bb3961076ef734eb"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.RootAndChildWindows = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L607) | 当前常量或枚举值：`3`。 | — |
| <a id="api-02c277c7ba788224"></a>[`Inno.Native.ImGui.ImGuiFocusedFlags.RootWindow = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L603) | 当前常量或枚举值：`2`。 | — |
| <a id="api-909455616f790052"></a>[`public enum Inno.Native.ImGui.ImGuiFocusedFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L599) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiFocusRequestFlags</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiFocusRequestFlags 94478ed9a1f68de03ff4c4f7486a3d55302152daf324e00fdc3d5e19074645ed -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusRequestFlags.None 319f3410fc1cbb2b4b8a5780302d026d182636e964b8e0ac16b668f2df3d62f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusRequestFlags.RestoreFocusedChild afad0a0425aaecbf09be616f8986ef7453de828d49b110bb900351a6b70fea3e -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusRequestFlags.UnlessBelowModal 6b66194762e235c5d1c9d53d9acdd19f7f5814047c31cb3030376f91c0006030 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c4196a7b2c7a4b73"></a>[`Inno.Native.ImGui.ImGuiFocusRequestFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1359) | 当前常量或枚举值：`0`。 | — |
| <a id="api-5f05a812496bdf64"></a>[`Inno.Native.ImGui.ImGuiFocusRequestFlags.RestoreFocusedChild = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1360) | 当前常量或枚举值：`1`。 | — |
| <a id="api-e6d7f7740dd52d76"></a>[`Inno.Native.ImGui.ImGuiFocusRequestFlags.UnlessBelowModal = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1361) | 当前常量或枚举值：`2`。 | — |
| <a id="api-6f2bc899e9ebf7c8"></a>[`public enum Inno.Native.ImGui.ImGuiFocusRequestFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1357) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiFocusScopeData</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiFocusScopeData ee442175b19d6f200fc365f78b579f58e8de4de43b21ad2b0bfb143b1681ec4f -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusScopeData.ID d096c9d5d403b73e55a3b38a2c58024369861db18b5d20fccf0a077986440ef2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiFocusScopeData.WindowID b924b4733454e4aa8dff2bb0cf433ebaa3daccb4b291680fdcb9d4aebbb7cb43 -->
<!-- api: M:Inno.Native.ImGui.ImGuiFocusScopeData.#ctor(System.UInt32,System.UInt32) c9639ef2c9d5fe53a4d1e311a8095eda9d30ce0cea79ab94ca486b76b0ef8c76 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-12d06390c0a14585"></a>[`public uint Inno.Native.ImGui.ImGuiFocusScopeData.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5741) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c867ef710cc748ea"></a>[`public uint Inno.Native.ImGui.ImGuiFocusScopeData.WindowID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5742) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-42dc9d5389b8033b"></a>[`public Inno.Native.ImGui.ImGuiFocusScopeData.ImGuiFocusScopeData(uint iD = 0, uint windowID = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5743) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-dd844d498541086f"></a>[`public struct Inno.Native.ImGui.ImGuiFocusScopeData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5739) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiGroupData</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiGroupData a3b326d2793280be975486c80830f7a9517a2d28002c4ae7a9b66e7c8e804820 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.WindowID d539e65811c0adb3da6843e2fdb839bb6a037b5a2d769f2208acee57ac0961e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupCursorPos 113971418d50953ff86757d1f69fd0d23ba91be6e78f5bf3538c648808017b5c -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupCursorMaxPos 61b0c32cb450bafb9aa8732e935bc5fab04f7865cd081006b834fd2f6074a669 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupCursorPosPrevLine 966162d2cdb39730413b94b687384b8ab5cdb388a35526c16ee5ad4931e4ed82 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupIndent 11a1885e377c5a1fdcb8becd32e5e29c4286ab3c8f7bebc98378724e70f13182 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupGroupOffset 7d4bf8ac889145243e61d110dc7a7a2c6fa00f0b6a7244c0c79175bcb1837d9e -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupCurrLineSize a9539526c37268090f12c3098b2513f0f4265fee0524a703227b367c6f69cb18 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupCurrLineTextBaseOffset ba1b53b8f358e3d75bf27d852df2c8703092a6b885f2ab1d502c377d92c8b027 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupActiveIdIsAlive 5e21b0bde4cc95b797be1616beffc6686b00f0672c31ee64a5a4e099eacd71e0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupActiveIdHasBeenEditedThisFrame 2ca8716829703bc7822950c5d2ab69b5f2ea6e3ca3c9761ce0ebf39b3f001af4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupDeactivatedIdIsAlive 4d9791f36e58510516a172b4ed142943cb96dc90734fc89272e7dd7b980370d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupHoveredIdIsAlive daff51bc1c888ee21f1d4e6fc0a2b2b60b72807219689682043995f8d195be31 -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.BackupIsSameLine b3d01556b0e8ed404903845509351a88d889fe957d4ddc405680ba585e0c9b1d -->
<!-- api: F:Inno.Native.ImGui.ImGuiGroupData.EmitItem 8ea3bf3ba86c8ac6e571fe625adb3d92bdc977563bea7982c670082356d98cc8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiGroupData.#ctor(System.UInt32,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,Inno.Native.ImGui.ImVec1,Inno.Native.ImGui.ImVec1,System.Numerics.Vector2,System.Single,System.UInt32,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte) 55dbb1697e284d59b88218f7fea1a449016446ee26350e2f5b5d1ca66a92cc52 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ba5c605940367179"></a>[`public byte Inno.Native.ImGui.ImGuiGroupData.BackupActiveIdHasBeenEditedThisFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5765) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1f0f8fc6912ac1a"></a>[`public uint Inno.Native.ImGui.ImGuiGroupData.BackupActiveIdIsAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5764) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-096fa740eff0d33b"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiGroupData.BackupCurrLineSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5762) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-094a4d0f13917e47"></a>[`public float Inno.Native.ImGui.ImGuiGroupData.BackupCurrLineTextBaseOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5763) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9808f65b38937f3"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiGroupData.BackupCursorMaxPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5758) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e9d4eed84d9d1089"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiGroupData.BackupCursorPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5757) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1dfcc8ac08b25b26"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiGroupData.BackupCursorPosPrevLine`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5759) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8622e68c1ee85272"></a>[`public byte Inno.Native.ImGui.ImGuiGroupData.BackupDeactivatedIdIsAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5766) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8f05173f5959611b"></a>[`public Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImGuiGroupData.BackupGroupOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5761) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be7edcd25369440f"></a>[`public byte Inno.Native.ImGui.ImGuiGroupData.BackupHoveredIdIsAlive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5767) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b975d270e74562d7"></a>[`public Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImGuiGroupData.BackupIndent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5760) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0672762cb3858339"></a>[`public byte Inno.Native.ImGui.ImGuiGroupData.BackupIsSameLine`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5768) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-74480013c7dcec70"></a>[`public byte Inno.Native.ImGui.ImGuiGroupData.EmitItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5769) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a43b8f5b5245bc64"></a>[`public uint Inno.Native.ImGui.ImGuiGroupData.WindowID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5756) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0ad1fec09f2c6454"></a>[`public Inno.Native.ImGui.ImGuiGroupData.ImGuiGroupData(uint windowID = 0, System.Numerics.Vector2 backupCursorPos = default(System.Numerics.Vector2), System.Numerics.Vector2 backupCursorMaxPos = default(System.Numerics.Vector2), System.Numerics.Vector2 backupCursorPosPrevLine = default(System.Numerics.Vector2), Inno.Native.ImGui.ImVec1 backupIndent = default(Inno.Native.ImGui.ImVec1), Inno.Native.ImGui.ImVec1 backupGroupOffset = default(Inno.Native.ImGui.ImVec1), System.Numerics.Vector2 backupCurrLineSize = default(System.Numerics.Vector2), float backupCurrLineTextBaseOffset = 0, uint backupActiveIdIsAlive = 0, byte backupActiveIdHasBeenEditedThisFrame = 0, byte backupDeactivatedIdIsAlive = 0, byte backupHoveredIdIsAlive = 0, byte backupIsSameLine = 0, byte emitItem = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5770) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6feffd6d1b09d27f"></a>[`public struct Inno.Native.ImGui.ImGuiGroupData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5754) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiHoveredFlags</code> · 22 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiHoveredFlags 863cd5ad4580e808a8564c7b4622583f9a00b14a38e7f987bc27601d0cc90fec -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.None 38b0283bef9785ae661d33bc557617099eb8056555015b9f0182697bb4d47e93 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.ChildWindows 5595d0b918e3e9f7cd65be528245bbaeeb832abefb81fa333f4e308503069438 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.RootWindow 8ef160a26447f05a3055355c5e13a535b2c7511ecd7401b69ed1b340f0dfd8de -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AnyWindow 3eace326716589447b2f4a3213932bb57117a740b78af27b07527054a4e1bcbd -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.NoPopupHierarchy 7f7b2ec42146c84314aa61d156b54dfa0042896f197a0507a5fd5d1f364f5165 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.DockHierarchy 0ba70c2f2485420cb81adae4a5265e2d6addebb1c9fc728b3291e5086709fcc7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenBlockedByPopup 02e45b1aade27e380726ae85777eb76c0ca4e33e4b7bb0733c0b1efce2f0d11b -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenBlockedByActiveItem 43aa9a06a8dec4a28b0ff820f2aad6f7456ce9e6401b57a21190daef75db7281 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenOverlappedByItem dd3db1e1b023dfd27c0e45c3e4dd45e8bf0a7ad39a73aa27b42bf6a5a65940c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenOverlappedByWindow c1be38ba67bf51d36aad19b2c85b0c29debbe52463e768b59b0ec2dbfb6b7afe -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenDisabled e9ea227df2815f612c9b36f4c0a06bedfabb9ed6de201a7c40b88a4940451551 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.NoNavOverride 5300470c9cb9aa53552ec2b18c3437128e3c200a45fbaed1035b7fd7d6c0176b -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenOverlapped a7a54669386d05ccaa9fd799617638af5f169205b71c45a37628caa44c155d9b -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.RectOnly 0941a97eb163ac016ded328b8591dc1438944eadcb2da85684d2eddda6cbf89e -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.RootAndChildWindows 5dcc376351a92625fdfe28935e50d52c13052d5c0e25680fc3cc4993cc3e45f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.ForTooltip 36a095518c30f0ab3e7944b0de40ce37f9588ef65202267ae1b24ebcb07098f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.Stationary 14299ce4c48681d60da5127c098a510a12b5997d7db734a740eddd5677bc74db -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.DelayNone e624fb40f1f5e3d54cfb23bd44b938adb13777fe9e5748433fba620eab5c24db -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.DelayShort 4b984b9dbc2fc74695f40db013bbd5a52ef2dc7839edf052843c03669efa40fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.DelayNormal f6831006ed7a5dcf4e7f230b3152212fcc8f1b6bb162254488654de2cbee00d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlags.NoSharedDelay 8643e7cbbef7f7f55b9a50896956d5e61ea26b00a1d9969585a1b5b5cd806f07 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a7c6e84c08dbc9a1"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenBlockedByActiveItem = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L622) | 当前常量或枚举值：`128`。 | — |
| <a id="api-bddc03d9d79bc331"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenBlockedByPopup = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L621) | 当前常量或枚举值：`32`。 | — |
| <a id="api-44c29cb6c6e45262"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenDisabled = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L625) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-9aea98909bb3d768"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenOverlapped = 768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L627) | 当前常量或枚举值：`768`。 | — |
| <a id="api-84dfa71f0425bb7c"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenOverlappedByItem = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L623) | 当前常量或枚举值：`256`。 | — |
| <a id="api-f0d172182aa750b1"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AllowWhenOverlappedByWindow = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L624) | 当前常量或枚举值：`512`。 | — |
| <a id="api-c1c3c4cdd62b122d"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.AnyWindow = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L618) | 当前常量或枚举值：`4`。 | — |
| <a id="api-068571d0414937b1"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.ChildWindows = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L616) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d2f93567ee8e5703"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.DelayNone = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L632) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-4047f8d865052d33"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.DelayNormal = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L634) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-0a933930e7436547"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.DelayShort = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L633) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-3c919fc43ec2b09a"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.DockHierarchy = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L620) | 当前常量或枚举值：`16`。 | — |
| <a id="api-73dc70081922a8c0"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.ForTooltip = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L630) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-53927798dcbaa74e"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.NoNavOverride = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L626) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-3830371d23b30d84"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L615) | 当前常量或枚举值：`0`。 | — |
| <a id="api-569d1fed0bd148d6"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.NoPopupHierarchy = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L619) | 当前常量或枚举值：`8`。 | — |
| <a id="api-ff89039f3136289f"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.NoSharedDelay = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L635) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-16c3a9e304c7250e"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.RectOnly = 928`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L628) | 当前常量或枚举值：`928`。 | — |
| <a id="api-9dd9742f4826b3e9"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.RootAndChildWindows = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L629) | 当前常量或枚举值：`3`。 | — |
| <a id="api-14c8092c5b85651a"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.RootWindow = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L617) | 当前常量或枚举值：`2`。 | — |
| <a id="api-3f5a2f2b7e28464b"></a>[`Inno.Native.ImGui.ImGuiHoveredFlags.Stationary = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L631) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-8257a5ac49b61170"></a>[`public enum Inno.Native.ImGui.ImGuiHoveredFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L613) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiHoveredFlagsPrivate</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiHoveredFlagsPrivate dab94d75b7f9a6e4b4063e57eb82740efd6accd8b1b5d6b79b26ffb60a75408c -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlagsPrivate.DelayMask 46c4092453feacb5e9f7168b33dc9f4cb2b333bdd51ac6da3bfe9fc3035bd7b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlagsPrivate.AllowedMaskForIsWindowHovered 25bc20b9fce157a3bb97ea75c1ebd3c6160ce0c7621a06d7fe5b305d93945593 -->
<!-- api: F:Inno.Native.ImGui.ImGuiHoveredFlagsPrivate.AllowedMaskForIsItemHovered 0a652a304672b35ca64fea7a6b80cf35621d49fdd60467597a760138685c44f3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6cf83c9ede1ed36e"></a>[`Inno.Native.ImGui.ImGuiHoveredFlagsPrivate.AllowedMaskForIsItemHovered = 262048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1262) | 当前常量或枚举值：`262048`。 | — |
| <a id="api-654d00efdb2533f9"></a>[`Inno.Native.ImGui.ImGuiHoveredFlagsPrivate.AllowedMaskForIsWindowHovered = 12479`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1261) | 当前常量或枚举值：`12479`。 | — |
| <a id="api-3d444fcd46ad5422"></a>[`Inno.Native.ImGui.ImGuiHoveredFlagsPrivate.DelayMask = 245760`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1260) | 当前常量或枚举值：`245760`。 | — |
| <a id="api-1a9acaf292365db5"></a>[`public enum Inno.Native.ImGui.ImGuiHoveredFlagsPrivate : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1258) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiIDStackTool</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiIDStackTool f602e09c4e7303e48614bc91206cfb66e6629d65eb59736991a373ef44487072 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIDStackTool.OptHexEncodeNonAsciiChars ee3fba6141481ac64b2c8701794dee9670d91630ed485a6cc2e4e3fc2aad9a2f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIDStackTool.OptCopyToClipboardOnCtrlC bafa88af02563baab3b05a0856af94f5330f56e502989fa26350b65fc80f4b5f -->
<!-- api: F:Inno.Native.ImGui.ImGuiIDStackTool.LastActiveFrame f8524c04af1f16d3da6de78d10e58cd08760921cc8eb6eb10324a5126397be9a -->
<!-- api: F:Inno.Native.ImGui.ImGuiIDStackTool.CopyToClipboardLastTime 99c432fbc1c97f5dee7459d5431531f7de3a0958c05396b619ebf729115f9399 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIDStackTool.#ctor(System.Byte,System.Byte,System.Int32,System.Single) 992bb478b097953f0fe41d32b2284736f3b8ea4f38f02bdca57e5dc6ee06c4bd -->
<!-- api: M:Inno.Native.ImGui.ImGuiIDStackTool.Destroy 65bd47cd890fb51b3899043801ae04f881ed152be528902303a66a219e66c9b1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a67572abdcb56c5e"></a>[`public float Inno.Native.ImGui.ImGuiIDStackTool.CopyToClipboardLastTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7532) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-795cb5f135cb95ae"></a>[`public int Inno.Native.ImGui.ImGuiIDStackTool.LastActiveFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7531) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccc5e195aa6c2493"></a>[`public byte Inno.Native.ImGui.ImGuiIDStackTool.OptCopyToClipboardOnCtrlC`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7530) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce15ac9012098860"></a>[`public byte Inno.Native.ImGui.ImGuiIDStackTool.OptHexEncodeNonAsciiChars`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7529) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-571530f3153d1b7f"></a>[`public Inno.Native.ImGui.ImGuiIDStackTool.ImGuiIDStackTool(byte optHexEncodeNonAsciiChars = 0, byte optCopyToClipboardOnCtrlC = 0, int lastActiveFrame = 0, float copyToClipboardLastTime = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7533) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1a6c4f1178c9e5b5"></a>[`public void Inno.Native.ImGui.ImGuiIDStackTool.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67403) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-783d3ffb4a64616b"></a>[`public struct Inno.Native.ImGui.ImGuiIDStackTool`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7527) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiIDStackToolPtr</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiIDStackToolPtr dafba5d781719b318a2b0504dc35d0b8a7c1380f1a7fdbfea59854000f414c74 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIDStackToolPtr.#ctor(Inno.Native.ImGui.ImGuiIDStackTool*) 9006fbf2e6859a3527d8ba8e4e3b5570f998a1904dc20c93001ee17ecc6c4e67 -->
<!-- api: F:Inno.Native.ImGui.ImGuiIDStackToolPtr.Handle c798cf7450c56ae3badc5351173e7bdb0ded1fe679fba78e841567365e5f9615 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIDStackToolPtr.IsNull d70cb3c76aaeda149f4fca1e473bc8ca40680c2041f64ac7c09c23fdb426e1cb -->
<!-- api: P:Inno.Native.ImGui.ImGuiIDStackToolPtr.Null c239c466b8e3f16d752751548b8471836f39a9f9e6a346e6fea7e60104b415eb -->
<!-- api: P:Inno.Native.ImGui.ImGuiIDStackToolPtr.Item(System.Int32) 38b1b1fb98b2912c4337f8914967c0e8cfb88c489c55d6c812f4f9b5d43aeee6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIDStackToolPtr.op_Implicit(Inno.Native.ImGui.ImGuiIDStackTool*)~Inno.Native.ImGui.ImGuiIDStackToolPtr 353e36711ae80a5e87d8575a5b2742a4244bc6b2d62dc6ef18ada3ede21075a2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIDStackToolPtr.op_Implicit(Inno.Native.ImGui.ImGuiIDStackToolPtr)~Inno.Native.ImGui.ImGuiIDStackTool* bdb90eb22ae300465d3e8b4ddd024553344815fc5ee12e8eee682697a907dc60 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIDStackToolPtr.op_Equality(Inno.Native.ImGui.ImGuiIDStackToolPtr,Inno.Native.ImGui.ImGuiIDStackToolPtr) 9a8f82e787caffe53d30a04297d30f624edcea453fc1e76f84786b30f90f29a0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIDStackToolPtr.op_Inequality(Inno.Native.ImGui.ImGuiIDStackToolPtr,Inno.Native.ImGui.ImGuiIDStackToolPtr) 0eac753d4b1ae1eeae79ba5f9417fdfc0c96d4b98a6816d386761312019f8288 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIDStackToolPtr.Equals(Inno.Native.ImGui.ImGuiIDStackToolPtr) 2818debea6dbcde857373a6973e4a8e85aeac09d5e1014fbbb0706643fd1713f -->
<!-- api: M:Inno.Native.ImGui.ImGuiIDStackToolPtr.Equals(System.Object) 757073d6a74e59e3c0f7979cb1589d9c43e26338a18f70c37d0b9937050fb92a -->
<!-- api: M:Inno.Native.ImGui.ImGuiIDStackToolPtr.GetHashCode 547f43b318fd8a9acedbe92f5f83215b59b0ea3c03458a1733446365fa5738b7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIDStackToolPtr.OptHexEncodeNonAsciiChars 4a5bac604715832c35bdd68361621b578a3b23eea65d62419a87b883affce45f -->
<!-- api: P:Inno.Native.ImGui.ImGuiIDStackToolPtr.OptCopyToClipboardOnCtrlC d330a1aaa04509543eae28cd129561adf842ce88fe9c0ecc69b58ae54376b74f -->
<!-- api: P:Inno.Native.ImGui.ImGuiIDStackToolPtr.LastActiveFrame da61b5844c32b91713d7b72602da583eaaa02239ebdb526479805db55fc786e6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiIDStackToolPtr.CopyToClipboardLastTime 3d7d0098f4674a3da4a9dc0674766781f965b27a8087b350ff94a9246ea18219 -->
<!-- api: M:Inno.Native.ImGui.ImGuiIDStackToolPtr.Destroy 10cf2d93f8140ec5c62f5d9b13cd4145c21439bd794e7c6bd579a2af74cc8e4f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f5084b447e11712f"></a>[`public Inno.Native.ImGui.ImGuiIDStackTool* Inno.Native.ImGui.ImGuiIDStackToolPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14136) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d840dc746f77d61c"></a>[`public Inno.Native.ImGui.ImGuiIDStackToolPtr.ImGuiIDStackToolPtr(Inno.Native.ImGui.ImGuiIDStackTool* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14131) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3ac6b239152f05e4"></a>[`public void Inno.Native.ImGui.ImGuiIDStackToolPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14158) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-24b66f3593f2e538"></a>[`public bool Inno.Native.ImGui.ImGuiIDStackToolPtr.Equals(Inno.Native.ImGui.ImGuiIDStackToolPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14146) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-ea80059e56693450"></a>[`public override bool Inno.Native.ImGui.ImGuiIDStackToolPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14147) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-66ac8b93c2e56c75"></a>[`public override int Inno.Native.ImGui.ImGuiIDStackToolPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14148) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-d4c08d7e66ff05ee"></a>[`public static bool Inno.Native.ImGui.ImGuiIDStackToolPtr.operator ==(Inno.Native.ImGui.ImGuiIDStackToolPtr left, Inno.Native.ImGui.ImGuiIDStackToolPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14144) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-611ead9bb0e72628"></a>[`public static Inno.Native.ImGui.ImGuiIDStackToolPtr.implicit operator Inno.Native.ImGui.ImGuiIDStackToolPtr(Inno.Native.ImGui.ImGuiIDStackTool* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14142) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b7a40dfda28be2ed"></a>[`public static Inno.Native.ImGui.ImGuiIDStackToolPtr.implicit operator Inno.Native.ImGui.ImGuiIDStackTool*(Inno.Native.ImGui.ImGuiIDStackToolPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14143) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-17e76d47ecb96a09"></a>[`public static bool Inno.Native.ImGui.ImGuiIDStackToolPtr.operator !=(Inno.Native.ImGui.ImGuiIDStackToolPtr left, Inno.Native.ImGui.ImGuiIDStackToolPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14145) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-512d114c851235f7"></a>[`public ref float Inno.Native.ImGui.ImGuiIDStackToolPtr.CopyToClipboardLastTime { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14153) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-74fbba9fc073b0d8"></a>[`public bool Inno.Native.ImGui.ImGuiIDStackToolPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14137) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-177145fab65f44e0"></a>[`public Inno.Native.ImGui.ImGuiIDStackTool Inno.Native.ImGui.ImGuiIDStackToolPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14140) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-14c20cd3a48bad88"></a>[`public ref int Inno.Native.ImGui.ImGuiIDStackToolPtr.LastActiveFrame { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14152) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bc9bfbc23aab466c"></a>[`public static Inno.Native.ImGui.ImGuiIDStackToolPtr Inno.Native.ImGui.ImGuiIDStackToolPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14138) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b46b458de2b1a78a"></a>[`public bool Inno.Native.ImGui.ImGuiIDStackToolPtr.OptCopyToClipboardOnCtrlC { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14151) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c7d4417fb4835869"></a>[`public bool Inno.Native.ImGui.ImGuiIDStackToolPtr.OptHexEncodeNonAsciiChars { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14150) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1d990db495719a58"></a>[`public struct Inno.Native.ImGui.ImGuiIDStackToolPtr : System.IEquatable<Inno.Native.ImGui.ImGuiIDStackToolPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14129) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputEvent</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputEvent 8e08e33a0ebd017846ac64795f2c3eac2b3c904d93a332c1dd63236db3a8a821 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.Type a9b587c050f3f68ca961f5ba8b44afdd51a40eda5e5896d1d2dc33c1021f5992 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.Source 748690d893db36a7c2bc4b51ec218756dcd7a4a3f1fca55f3cec037e862bbcd3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.EventId 937485f180b87fabc6711d09949144e4e8d58908e17d5b9de0787789979750a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.AnonymousField3 a32cf23b9eaaf63b7b1beb6d59f6809ab7e6645099db5006f4a98757cf797bdc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.AddedByTestEngine 036fd63f37ed9502808d1e39bff9e5f1682680df43c33e5fd25c1771a644cc77 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEvent.#ctor(Inno.Native.ImGui.ImGuiInputEventType,Inno.Native.ImGui.ImGuiInputSource,System.UInt32,Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous,System.Byte) 41cf3bda0faf52eae1bf6007453396d82bb7dab3ce00743203e9533be4089c7f -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEvent.Destroy def91716578d53a5ed72f2fa4bf78d3ded65ca38302a9b285770cc902ee5d57a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d3d367b63b7286a7"></a>[`public byte Inno.Native.ImGui.ImGuiInputEvent.AddedByTestEngine`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3900) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8d73971af9da73eb"></a>[`public Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous Inno.Native.ImGui.ImGuiInputEvent.AnonymousField3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3899) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d0a73a481e3218b3"></a>[`public uint Inno.Native.ImGui.ImGuiInputEvent.EventId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3898) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b0c5ac815dad30c3"></a>[`public Inno.Native.ImGui.ImGuiInputSource Inno.Native.ImGui.ImGuiInputEvent.Source`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3897) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4c293f8a09786f8e"></a>[`public Inno.Native.ImGui.ImGuiInputEventType Inno.Native.ImGui.ImGuiInputEvent.Type`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3896) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2ecbcd8a9e8d52e2"></a>[`public Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEvent(Inno.Native.ImGui.ImGuiInputEventType type = Inno.Native.ImGui.ImGuiInputEventType.None, Inno.Native.ImGui.ImGuiInputSource source = Inno.Native.ImGui.ImGuiInputSource.None, uint eventId = 0, Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous value3 = default(Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous), byte addedByTestEngine = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3901) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a870172b15c67f37"></a>[`public void Inno.Native.ImGui.ImGuiInputEvent.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66837) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-60045c6af1f24426"></a>[`public struct Inno.Native.ImGui.ImGuiInputEvent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3872) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous be8f3698ed4ed01e0465cb325b58aabc8223c782177013b9dfd7aae88e9d3fe7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.MousePos daa60f3471a22589e9f9b7c7596e7f334384ad10330bb0de1115765dadcd5cd6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.MouseWheel 41950f66470dd687113065da7b9687e04b694b9f6945379d13464d2eca5797a3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.MouseButton 3896bc7c98e3827f850f1352d8b8f2ffd5a22393531b8bda08e764c41117f4ea -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.MouseViewport 9a8797e6f4d8ad30cf647c7746b5b615b25b8dd42929504c9e00637cd87ee3e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.Key bdbee7f4febc02b10778a2f5abe9caae4354c9a6a05dbd5082d8ec20ce4a0a68 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.Text 86ddb067eeefad2577e835fa0edb1bf87ead4f362fd414d51fdab3c133974f95 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.AppFocused 7f346dc39324df3b21e9e8c140f5393ae79c9a5c67b2097d266952338246a007 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6e3219aa80eb8179"></a>[`public Inno.Native.ImGui.ImGuiInputEventAppFocused Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.AppFocused`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3893) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c1994d80daadba4"></a>[`public Inno.Native.ImGui.ImGuiInputEventKey Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.Key`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3889) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fdeca7fb68cb87fd"></a>[`public Inno.Native.ImGui.ImGuiInputEventMouseButton Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.MouseButton`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3885) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8041da31c2ca4dbd"></a>[`public Inno.Native.ImGui.ImGuiInputEventMousePos Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.MousePos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3881) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20b19f11a51ebe8c"></a>[`public Inno.Native.ImGui.ImGuiInputEventMouseViewport Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.MouseViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3887) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e0b02f0bd5ca9f3"></a>[`public Inno.Native.ImGui.ImGuiInputEventMouseWheel Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.MouseWheel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3883) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e9c979c6746501b0"></a>[`public Inno.Native.ImGui.ImGuiInputEventText Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous.Text`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3891) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6b49c5552260cb6e"></a>[`public struct Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3878) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputEventAppFocused</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputEventAppFocused c614c1f94b2ebf9e08d972dbbf6e289a2693b5f6fa44d51f5a873f0ea237631e -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventAppFocused.Focused 6b4ecb380f49d28a9f5802bd9466675c9902e24cfd76b9b2d970443056bf2d0d -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventAppFocused.#ctor(System.Byte) 301734d5a65b5b60f624220e357732de37c003f83618718e70d12e50f36566df -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-49c271e9b7abfd47"></a>[`public byte Inno.Native.ImGui.ImGuiInputEventAppFocused.Focused`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3861) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f7efd8b6d427ceb3"></a>[`public Inno.Native.ImGui.ImGuiInputEventAppFocused.ImGuiInputEventAppFocused(byte focused = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3862) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2d399272a020fc4f"></a>[`public struct Inno.Native.ImGui.ImGuiInputEventAppFocused`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3859) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputEventKey</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputEventKey d1cd614d24931060d6949b1a2511ac3d20486b973a46838a87f41fa737111361 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventKey.Key e2cf8d92d5fc5c0a940ee2ea3d37ec78b55421bc38423c910a26e7d327fc145b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventKey.Down fcccbec40a9fed88bc4cfa06725fd76598037fa45c0dc0af2d35cfc164be0b3b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventKey.AnalogValue ae3c5cea90839c92d9202b4402cc2f9c1c9865748eac6f7676f8e988305ff598 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventKey.#ctor(Inno.Native.ImGui.ImGuiKey,System.Byte,System.Single) 71fe24892f045bc21ead464e007e3e83474bb6fbe02fc65fbeff6757ffca1008 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-232e57af6c0f63cc"></a>[`public float Inno.Native.ImGui.ImGuiInputEventKey.AnalogValue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3833) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aecddddbc63ce495"></a>[`public byte Inno.Native.ImGui.ImGuiInputEventKey.Down`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3832) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4b9c78dc11bb7ab1"></a>[`public Inno.Native.ImGui.ImGuiKey Inno.Native.ImGui.ImGuiInputEventKey.Key`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3831) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e283130c35d9f1a8"></a>[`public Inno.Native.ImGui.ImGuiInputEventKey.ImGuiInputEventKey(Inno.Native.ImGui.ImGuiKey key = Inno.Native.ImGui.ImGuiKey.ModNone, byte down = 0, float analogValue = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3834) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1ded22643fe2eb67"></a>[`public struct Inno.Native.ImGui.ImGuiInputEventKey`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3829) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputEventMouseButton</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputEventMouseButton 9b529a3959e34e06339bcb04769bb865375fcf11f4957f0705f706d8d06b0279 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseButton.Button 6a7b26be94a7ff930ed4189078cbb77ea39150d68d9fded2f7037f24466f4039 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseButton.Down 9f466675459d775b4332af35819951e3a0aa1e92f4ede105ab689dd6c5aa974f -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseButton.MouseSource 94a440c80296b8973c9a413072c3fecc374c65d07905b978fb4e7e6952e3e96b -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventMouseButton.#ctor(System.Int32,System.Byte,Inno.Native.ImGui.ImGuiMouseSource) 6bf3603cabadb0ab22957bf9d346ab0b469c6e158bdafe25d6317ba8de044913 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ac4fde28c41f119a"></a>[`public int Inno.Native.ImGui.ImGuiInputEventMouseButton.Button`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3801) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7efc5f1d3a1e6293"></a>[`public byte Inno.Native.ImGui.ImGuiInputEventMouseButton.Down`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3802) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3df9c69dac0ecc5e"></a>[`public Inno.Native.ImGui.ImGuiMouseSource Inno.Native.ImGui.ImGuiInputEventMouseButton.MouseSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3803) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-76ee679f62ba81b9"></a>[`public Inno.Native.ImGui.ImGuiInputEventMouseButton.ImGuiInputEventMouseButton(int button = 0, byte down = 0, Inno.Native.ImGui.ImGuiMouseSource mouseSource = Inno.Native.ImGui.ImGuiMouseSource.Mouse)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3804) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d1a12d855f534709"></a>[`public struct Inno.Native.ImGui.ImGuiInputEventMouseButton`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3799) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputEventMousePos</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputEventMousePos 9b9d3df8470725555965a09b00452dfe350261af59dd176928163649abb4526c -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMousePos.PosX 627d975847c3da37233d3f28f310e18040ca1b6eba7e84e199546b0209c5346a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMousePos.PosY a15fd1e000eee1e3b0b7f1c9d0a5c3e3ea13865383434388bf494d3c0344217a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMousePos.MouseSource cf4d1d79dd36cc91659bcf7b7ee7b5564b8b2ca967fdfa286ed7bc83152ae7d3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventMousePos.#ctor(System.Single,System.Single,Inno.Native.ImGui.ImGuiMouseSource) 956107ce6d6abed6a6e52821ae6a00f3c0a6b46f5397755f362cff71a0db06d5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-97fb78e518686f83"></a>[`public Inno.Native.ImGui.ImGuiMouseSource Inno.Native.ImGui.ImGuiInputEventMousePos.MouseSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3769) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc7b7402c06f5d2f"></a>[`public float Inno.Native.ImGui.ImGuiInputEventMousePos.PosX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3767) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5cd97ea4cefb3422"></a>[`public float Inno.Native.ImGui.ImGuiInputEventMousePos.PosY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3768) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d0e97cfe01bf07f2"></a>[`public Inno.Native.ImGui.ImGuiInputEventMousePos.ImGuiInputEventMousePos(float posX = 0, float posY = 0, Inno.Native.ImGui.ImGuiMouseSource mouseSource = Inno.Native.ImGui.ImGuiMouseSource.Mouse)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3770) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4e26e8dda9d385fa"></a>[`public struct Inno.Native.ImGui.ImGuiInputEventMousePos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3765) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputEventMouseViewport</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputEventMouseViewport c1b0bf5e2995c089003a288b4f1f280d286a98e0ae58b7609675a376f3bf6391 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseViewport.HoveredViewportID 60ff0fb70b6f3b02bd1c48204c0f775acf5f7ad154f1e2452b35c947f8094c6b -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventMouseViewport.#ctor(System.UInt32) d4a81b190115cc6459ce22c2403356d451b93d18b35dd26a8007d6d20e9cf0e6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b4d892f5ac7f3067"></a>[`public uint Inno.Native.ImGui.ImGuiInputEventMouseViewport.HoveredViewportID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3818) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-12a0fb61cac1924e"></a>[`public Inno.Native.ImGui.ImGuiInputEventMouseViewport.ImGuiInputEventMouseViewport(uint hoveredViewportID = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3819) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-94a255bd91c7d1e2"></a>[`public struct Inno.Native.ImGui.ImGuiInputEventMouseViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3816) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputEventMouseWheel</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputEventMouseWheel 12419385eef8266ae9a3793bfef9599e97512c435ffdda5fca7b216abc1e6f39 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseWheel.WheelX 13ba434353d88e760aaa14ae11168c6148c6fc9528ed217d2de037356ca5703e -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseWheel.WheelY 57acac0bfd2ea498e5d563ebddc076c634a0b36d6d3f4430f18b88a42bf3629a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventMouseWheel.MouseSource 733d4b55fa997408126463884af1c0c4c0ccf9f88824d17c2ee550ebd72339fc -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventMouseWheel.#ctor(System.Single,System.Single,Inno.Native.ImGui.ImGuiMouseSource) 2fcfa2e43953a5e18b9ccd26d556d4162f12752ae69ef670455f10da0b7b93f4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1bd2abab6ee8fd9b"></a>[`public Inno.Native.ImGui.ImGuiMouseSource Inno.Native.ImGui.ImGuiInputEventMouseWheel.MouseSource`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3786) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e990e8ed0ecd954"></a>[`public float Inno.Native.ImGui.ImGuiInputEventMouseWheel.WheelX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3784) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad236e17e1bc8f73"></a>[`public float Inno.Native.ImGui.ImGuiInputEventMouseWheel.WheelY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3785) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-114904fc53f9090b"></a>[`public Inno.Native.ImGui.ImGuiInputEventMouseWheel.ImGuiInputEventMouseWheel(float wheelX = 0, float wheelY = 0, Inno.Native.ImGui.ImGuiMouseSource mouseSource = Inno.Native.ImGui.ImGuiMouseSource.Mouse)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3787) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-eb0ecf59891cf460"></a>[`public struct Inno.Native.ImGui.ImGuiInputEventMouseWheel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3782) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputEventPtr</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputEventPtr 37f5523770f99a8658c8ed983364673351727d30054af99ae455eec886423a91 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventPtr.#ctor(Inno.Native.ImGui.ImGuiInputEvent*) 8c1ffb30253b98221648a171c7d3c86be3bb9403ca89ee1d8858a20f1989c924 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventPtr.Handle c69acb3d9bdeac40ed08eb77379c11fef9ef5353ac97c5456d80ab5928885bc6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputEventPtr.IsNull f20774d30abd7a5a7cb3bb682f82856bc859381c6447a07472e33621201da105 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputEventPtr.Null a14d1a0b22a986b33e55c5bd2d675ba62ec03f6d387a881ddc20e97f278f426f -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputEventPtr.Item(System.Int32) e1b08508203bb938e831a38a4634291c51d97535ae12b920777a671b8566d25a -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventPtr.op_Implicit(Inno.Native.ImGui.ImGuiInputEvent*)~Inno.Native.ImGui.ImGuiInputEventPtr f80f85164a5c0a7c6092feb12888a1864dd9dc5fa925334fc3a462261f6d4bcf -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventPtr.op_Implicit(Inno.Native.ImGui.ImGuiInputEventPtr)~Inno.Native.ImGui.ImGuiInputEvent* a669ee3391b3b71f891fd5423f4700f28a1595f67ee75baad8e30a9cab302fff -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventPtr.op_Equality(Inno.Native.ImGui.ImGuiInputEventPtr,Inno.Native.ImGui.ImGuiInputEventPtr) 8134b195272033bcc50392ba2ac81f31ac9dd7efa90c4fe2e2edf5e1496e5e6c -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventPtr.op_Inequality(Inno.Native.ImGui.ImGuiInputEventPtr,Inno.Native.ImGui.ImGuiInputEventPtr) 65d80ca95a07552c33557c20cd34c81e14ce0de64df64775d812704eb0c6a1ae -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventPtr.Equals(Inno.Native.ImGui.ImGuiInputEventPtr) 9764e8a9562566b13277d4eabe14dd21306f09e9c317cffec75b904e608663c4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventPtr.Equals(System.Object) dccd9a945e9bb026d57e69890d6871bc9e4ebad1689fd23d3e7ab079127d7425 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventPtr.GetHashCode ba8c187df3afb37b5a665c53ebf4c49e868f5dca255556a28593490350e4056a -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputEventPtr.Type 8bfb14020dc1059b723a9af8e00d4b77e8d6b0db2e3625723fc264bba217c58d -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputEventPtr.Source f4457638265f62d5f58041e8ddb3e4cec487c6fe61035a0fdaa42653a22541e3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputEventPtr.EventId 4766d0aedffe5e2e1fadcd47e45b4c575b8d012a157045d3a2e51eafd4a7fedf -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputEventPtr.AnonymousField3 8eda261ac4a7a1ce4be23c323f32015721f1f17423dacbdd20f6d873fdb8984c -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputEventPtr.AddedByTestEngine 95f1bb723b59a804a6c66a55a4962b9cf381bc607d5305c62053685138cbc1df -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventPtr.Destroy acfa22d3d6dfc2c01e126990c95973779a00e4a527f07ce10001c9aa84d504a0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-44a859e6fb686db2"></a>[`public Inno.Native.ImGui.ImGuiInputEvent* Inno.Native.ImGui.ImGuiInputEventPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14452) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1a28677172022e1"></a>[`public Inno.Native.ImGui.ImGuiInputEventPtr.ImGuiInputEventPtr(Inno.Native.ImGui.ImGuiInputEvent* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14447) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f78dc06223555e51"></a>[`public void Inno.Native.ImGui.ImGuiInputEventPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14475) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b26de1f26eea0590"></a>[`public bool Inno.Native.ImGui.ImGuiInputEventPtr.Equals(Inno.Native.ImGui.ImGuiInputEventPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14462) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-d9c040e2002b15d9"></a>[`public override bool Inno.Native.ImGui.ImGuiInputEventPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14463) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-d33e8945570aefa1"></a>[`public override int Inno.Native.ImGui.ImGuiInputEventPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14464) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-ea8b0b2a8251fab3"></a>[`public static bool Inno.Native.ImGui.ImGuiInputEventPtr.operator ==(Inno.Native.ImGui.ImGuiInputEventPtr left, Inno.Native.ImGui.ImGuiInputEventPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14460) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e6a41910577cb78"></a>[`public static Inno.Native.ImGui.ImGuiInputEventPtr.implicit operator Inno.Native.ImGui.ImGuiInputEventPtr(Inno.Native.ImGui.ImGuiInputEvent* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14458) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4e4441c6c4db82c9"></a>[`public static Inno.Native.ImGui.ImGuiInputEventPtr.implicit operator Inno.Native.ImGui.ImGuiInputEvent*(Inno.Native.ImGui.ImGuiInputEventPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14459) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-54fb72400379807c"></a>[`public static bool Inno.Native.ImGui.ImGuiInputEventPtr.operator !=(Inno.Native.ImGui.ImGuiInputEventPtr left, Inno.Native.ImGui.ImGuiInputEventPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14461) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8d13de4ea23087ab"></a>[`public bool Inno.Native.ImGui.ImGuiInputEventPtr.AddedByTestEngine { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14470) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4113cd9478e24e67"></a>[`public ref Inno.Native.ImGui.ImGuiInputEvent.ImGuiInputEventAnonymous Inno.Native.ImGui.ImGuiInputEventPtr.AnonymousField3 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14469) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0a5677fd0f77bf68"></a>[`public ref uint Inno.Native.ImGui.ImGuiInputEventPtr.EventId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14468) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-576780c9184d5d11"></a>[`public bool Inno.Native.ImGui.ImGuiInputEventPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14453) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4e0bbee012c7ca4b"></a>[`public Inno.Native.ImGui.ImGuiInputEvent Inno.Native.ImGui.ImGuiInputEventPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14456) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-11d61eee536c463c"></a>[`public static Inno.Native.ImGui.ImGuiInputEventPtr Inno.Native.ImGui.ImGuiInputEventPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14454) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-694457623aa83c83"></a>[`public ref Inno.Native.ImGui.ImGuiInputSource Inno.Native.ImGui.ImGuiInputEventPtr.Source { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14467) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-20480adf396b8e75"></a>[`public ref Inno.Native.ImGui.ImGuiInputEventType Inno.Native.ImGui.ImGuiInputEventPtr.Type { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14466) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7c90c176fe85bac4"></a>[`public struct Inno.Native.ImGui.ImGuiInputEventPtr : System.IEquatable<Inno.Native.ImGui.ImGuiInputEventPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14445) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputEventText</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputEventText 23afff136db76f564766f07b6c43899cb6f70b92f60ce478d279beff5428d74c -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventText.Char 20d91b6b24f6a64711eae3389924c6e8c94a2b4b5f2583167940f95d0233b8eb -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputEventText.#ctor(System.UInt32) 180a039c20615773998f2866cb16860c34f03c2922fdddd8cb560a175cb3f44f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6cbac024d599088b"></a>[`public uint Inno.Native.ImGui.ImGuiInputEventText.Char`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3848) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6edde25f9485ba77"></a>[`public Inno.Native.ImGui.ImGuiInputEventText.ImGuiInputEventText(uint @char = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3849) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-82045ef2ea57c578"></a>[`public struct Inno.Native.ImGui.ImGuiInputEventText`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3846) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputEventType</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputEventType 3c2e836db5f002e4d06fa8a71c154a960c010f783a6bbda601bf789740e78040 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.None c090510d9c0661dc0cfb1c96cc30e94006a718a26315a2bffe502971379b6048 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.MousePos 81f11d5d68d04e39d34aec7643dd49ba7492e131da5a66d32d3deeaca53281dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.MouseWheel 211d54a42343d7f6361075b7f478ea3b698bce86f402867372d46bb642b73bba -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.MouseButton 8418b32d9069431bba8900a9b6d6d845c607d0826f9f154a90bdc03b6fca2c19 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.MouseViewport 66e368c22b87efc4eaef86ead2599a4623c2bf0c09cef7e9cf31dcf0a212683b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.Key dc6cd0ac8d970c8117576344ef1c8ed5f10a6a5b47115da8da5560673eca2a55 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.Text ecb8a5d58834e0e23c810a12d54f6f54fc62da39df762cd08ce649d015182311 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.Focus 84eb12eae92ea23d0ba58f98e5d391ad90c64b1e460c9ca8f22d8d1d25aedcfc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputEventType.Count 6d117ac86c73e12a58b874f8a905f932e4e1780fce204a7b77cc1df6f08d9a79 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-abb1cdcb3da05bee"></a>[`Inno.Native.ImGui.ImGuiInputEventType.Count = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L102) | 当前常量或枚举值：`8`。 | — |
| <a id="api-b9ced90929a735c2"></a>[`Inno.Native.ImGui.ImGuiInputEventType.Focus = 7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L101) | 当前常量或枚举值：`7`。 | — |
| <a id="api-70e39ff855567f56"></a>[`Inno.Native.ImGui.ImGuiInputEventType.Key = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L99) | 当前常量或枚举值：`5`。 | — |
| <a id="api-839916ef576d2680"></a>[`Inno.Native.ImGui.ImGuiInputEventType.MouseButton = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L97) | 当前常量或枚举值：`3`。 | — |
| <a id="api-7c89f0e73d5422dd"></a>[`Inno.Native.ImGui.ImGuiInputEventType.MousePos = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L95) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d501f762e3a35e92"></a>[`Inno.Native.ImGui.ImGuiInputEventType.MouseViewport = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L98) | 当前常量或枚举值：`4`。 | — |
| <a id="api-3ecf117f190db062"></a>[`Inno.Native.ImGui.ImGuiInputEventType.MouseWheel = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L96) | 当前常量或枚举值：`2`。 | — |
| <a id="api-66949ffa0fd8cc4f"></a>[`Inno.Native.ImGui.ImGuiInputEventType.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L94) | 当前常量或枚举值：`0`。 | — |
| <a id="api-5db03e42e9f40ca8"></a>[`Inno.Native.ImGui.ImGuiInputEventType.Text = 6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L100) | 当前常量或枚举值：`6`。 | — |
| <a id="api-b6cfab67dd839eb6"></a>[`public enum Inno.Native.ImGui.ImGuiInputEventType : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L92) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputFlags</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputFlags 4f19064d66e45c5d40d264d8b4b2b7589a321b66aea7528cd768b91917cbabb2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.None 1460de4a15c2d7a9c5f81dd2b13627ca3ca9b141a2226fbcb4ec0d8c205aa55f -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.Repeat 1f1f37216c8d2cc2e2ab0df9e5e418a121b833c97e1bb1c4d09ef3ee55f83c09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteActive 969d400a39f92ec7bffa5ffe4d5628cdce3e24f29090a7261fcbf363e750f5fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteFocused 975e85609926f92a6e06836586fd9aea634324f2b8e792c1cc423fb7e6123cff -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteGlobal 4b52efb500802a1f0e15ec03049a62c05e34301932c416a305b93483a032cb1a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteAlways b79dc0fba74364281ef4505d0e744cf41c1f7ccc98111e8446e803d9508b5c75 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteOverFocused 67784bd3bd142265d3cc713de68dfc4819d29789e7a7289df12de448cd9f927c -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteOverActive e6ffefeb08807c148da8869d573c8b0a5833d4d0c39c38d7ed8eadbeac609df3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteUnlessBgFocused b73f3d5603413d122bb4d94baccdae6ed99592482bc5ab0ada045d894fc5da4c -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.RouteFromRootWindow d06aeaac7454dfd18dd4755ed8bd6030cf5958a8e6e5a6f36e640d21640b0328 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlags.Tooltip cf948211a0d5cc0410de0e3b658d629582efba2022b07b456bd87069ca055ad4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3eab52058f6101f0"></a>[`Inno.Native.ImGui.ImGuiInputFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L699) | 当前常量或枚举值：`0`。 | — |
| <a id="api-e59a42d1316a7a0f"></a>[`Inno.Native.ImGui.ImGuiInputFlags.Repeat = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L700) | 当前常量或枚举值：`1`。 | — |
| <a id="api-23950f50c3c21f03"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteActive = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L701) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-ce2a86c1de287a27"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteAlways = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L704) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-38e634e2293a1701"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteFocused = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L702) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-fb26673e4f57f05a"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteFromRootWindow = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L708) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-8c54b89b106ca86a"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteGlobal = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L703) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-45f243a2f814b183"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteOverActive = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L706) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-f901ad3f70db8917"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteOverFocused = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L705) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-f7076d32306b2d55"></a>[`Inno.Native.ImGui.ImGuiInputFlags.RouteUnlessBgFocused = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L707) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-6b3432ce32970977"></a>[`Inno.Native.ImGui.ImGuiInputFlags.Tooltip = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L709) | 当前常量或枚举值：`262144`。 | — |
| <a id="api-e8829ef9b9a6bd2e"></a>[`public enum Inno.Native.ImGui.ImGuiInputFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L697) | Native 取值集合；当前取值见本类型成员。 | — |

</details>
