# Inno.Native.ImGui ABI · ImGuiSelectionBasicStoragePtr → ImGuiStyleVarInfo

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr</code> · 27 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr db0c32bfd7e51cee71d7ecbc5908599a65c203ab906021cd9fe405a0daadb7c3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.#ctor(Inno.Native.ImGui.ImGuiSelectionBasicStorage*) fab487a9d50398d4522375909feb93d6125f934e7fd80cd3ddeffacdb6d76af6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Handle 3e199375d61197f8d1e2ecbe7bd36a818758b441ab8b1d5976d182cb7e5e9aa2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.IsNull 2c502e6560085d1c08e72721e9726e3c56044db279194467b068c4e58413f35f -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Null fc333dccbbec9e042f79dc237e0bfeb77369568518d1e0a7974a810861343063 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Item(System.Int32) 1fee03a1562bb732ff74876484a8051c0a2a994a46316adcef9a569ef1b5132e -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.op_Implicit(Inno.Native.ImGui.ImGuiSelectionBasicStorage*)~Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr cf09a05bdaf266dbeab3c4aa38abe22145d2ccab1792fd82d05e9ca313ade5df -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.op_Implicit(Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr)~Inno.Native.ImGui.ImGuiSelectionBasicStorage* 5c0132e502f098bfd6f587898cf45ce75ab4b2b062e0283a77e1fbec50149a88 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.op_Equality(Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr,Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr) 21fd4d0dea85e3922e9f3f462aeeb7b7405491860620cee458ec1d09babaf5eb -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.op_Inequality(Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr,Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr) da8a8b620d2eb0b65d719e6b60aa8a810fc6cce1e631e0f28678789f80d5ccfd -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Equals(Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr) 7a111085f2f410bbf973c996427c8c81c7f7451218f6e3dcdd8661bd86d41d64 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Equals(System.Object) af8295a0927dc22022015d817d159eccec7e20a423a8b6e9f7688e6c11c2695c -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.GetHashCode 24b471931102250a3f4f0e8ab78ad4efa4186213e451ccab32a1e7149f953aeb -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Size abd0a74f95f20a031b3921a960bd75c9a6fb6780d02c716031dded5bc7252522 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.PreserveOrder e50334e1e50186b58ddf040a53069ae9b1a57f5dd17e7a51ee0e35cc662e682e -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.UserData 256ba9d48fe09efecce911018e2d0c565b28adb7b56768cf36046ea96a2682e4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.AdapterIndexToStorageId d491ee165caf0ebaf42770e5fae123914c7d1318dacf8495f2e648d423a14776 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.SelectionOrder d321b3256f34ac89620f9fb8d45a91635893bbb8b25a47b9957f3c0db6727bfb -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Storage 60d82d7b40e51d1e9f4b8141e472e44157e113cd4c6fb2997d9bd91aa6c993a0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Destroy bff3792204c167ef5070bae17b531e624309465c0044b4a3d43a9454fb08f899 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.ApplyRequests(Inno.Native.ImGui.ImGuiMultiSelectIOPtr) 2bbecd4de600c6c4f0a8e1cda6ced0ec975d78fae7c89efee9d62851f2488317 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Contains(System.UInt32) 68d2c1b32a4d90665ccaa2ed74338c57027642c5a5c0950c065ab0985e30fab4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Clear a4ef4d317bffee6b41a9262b3364ce16f2eb2ea19aa5b928d29282f5e976506b -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Swap(Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr) d5daa82c28416812977c5f7522dcfebc6263739130e8c3b7e378e7ef8083091f -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.SetItemSelected(System.UInt32,System.Boolean) 0eac43a1fa4e47c7f31ffdaa07d0e142ba859cd8eb9f5b42579a58526d7f2a52 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.GetNextSelectedItem(System.Void**,System.UInt32@) f73d4f874f4aacf35fe95d08b59997b70385d2b23c831b057f065d8921dcdf6d -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.GetStorageIdFromIndex(System.Int32) 23990354685cbac6b66cf28c6585dd28c0db872db75348833785da5781d0b93f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-267290e065a625c0"></a>[`public Inno.Native.ImGui.ImGuiSelectionBasicStorage* Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15965) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-50f71fa151784614"></a>[`public Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.ImGuiSelectionBasicStoragePtr(Inno.Native.ImGui.ImGuiSelectionBasicStorage* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15960) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-98fb6bd39ed836ba"></a>[`public void Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.ApplyRequests(Inno.Native.ImGui.ImGuiMultiSelectIOPtr msIo)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15997) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-537c9ec090543282"></a>[`public void Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16013) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-33ad7ad21406d1fd"></a>[`public bool Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Contains(uint id)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16005) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6b09f8f2c03f5e44"></a>[`public void Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15989) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fc2959ae38f3bffb"></a>[`public bool Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Equals(Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15975) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-b075f75a3b65a0dc"></a>[`public override bool Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15976) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-d61cc64c4e8f6de2"></a>[`public override int Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15977) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-763999efd435509b"></a>[`public bool Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.GetNextSelectedItem(void** opaqueIt, ref uint outId)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16037) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8008e8c642b61aed"></a>[`public uint Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.GetStorageIdFromIndex(int idx)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16045) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-911044b3168684b5"></a>[`public static bool Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.operator ==(Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr left, Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15973) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7a1089e3418d400f"></a>[`public static Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.implicit operator Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr(Inno.Native.ImGui.ImGuiSelectionBasicStorage* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15971) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8c5e8b4b676584c9"></a>[`public static Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.implicit operator Inno.Native.ImGui.ImGuiSelectionBasicStorage*(Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15972) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a83730e7c981520c"></a>[`public static bool Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.operator !=(Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr left, Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15974) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ce04dfd6b3a23cf"></a>[`public void Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.SetItemSelected(uint id, bool selected)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16029) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9c6ddf1c35862ab9"></a>[`public void Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Swap(Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16021) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-53e7e88a842edc22"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSelectionBasicStorage*, int, uint> Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.AdapterIndexToStorageId { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15982) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-426792413fb987f7"></a>[`public bool Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15966) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3c63966e404b8ee5"></a>[`public Inno.Native.ImGui.ImGuiSelectionBasicStorage Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15969) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6bf0bb11db5c5c07"></a>[`public static Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15967) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b9766b829f9e17d0"></a>[`public bool Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.PreserveOrder { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15980) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f3b4c0c48e796dcc"></a>[`public ref int Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.SelectionOrder { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15983) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5c4496c43a50f033"></a>[`public ref int Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Size { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15979) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-18ddfc14f583bcc4"></a>[`public ref Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.Storage { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15984) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a79f79be9e3aa1c3"></a>[`public void* Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr.UserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15981) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d9a52ab09121da33"></a>[`public struct Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr : System.IEquatable<Inno.Native.ImGui.ImGuiSelectionBasicStoragePtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L15958) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSelectionExternalStorage</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSelectionExternalStorage 85a23851bca110f0112af66a7bf5662cb6556830fc76b08b8cc9f798f5fc3694 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionExternalStorage.UserData 3b09bc56e19f88e927f28f81cdcb1caf42104d013cd4493147cb7b8826cc054e -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionExternalStorage.AdapterSetItemSelected 73f4886d84fcd7b8976eef8bed0fbe35ec7a887d787fb207c7d7a2cb7c05872a -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionExternalStorage.#ctor(System.Void*,) 009e38c5d3231af3c7cedef8ad3c59dcba0d87fc3c0d79f9cad6a4a4dae64369 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionExternalStorage.Destroy 3e2f5fc4fc0582d4ce1d7ad71b1b56e13b55e567a59c7553e50d64370576d67d -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionExternalStorage.ApplyRequests(Inno.Native.ImGui.ImGuiMultiSelectIO*) b52d0e57dfa599feee3f23b1bc075eff38889ad85ba2207448519fb13677a28f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-639dc2e40fe89d03"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSelectionExternalStorage*, int, byte, void> Inno.Native.ImGui.ImGuiSelectionExternalStorage.AdapterSetItemSelected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9577) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3fb18a3742e5ef61"></a>[`public void* Inno.Native.ImGui.ImGuiSelectionExternalStorage.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9576) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20958a62ea17178e"></a>[`public Inno.Native.ImGui.ImGuiSelectionExternalStorage.ImGuiSelectionExternalStorage(void* userData = null, delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSelectionExternalStorage*, int, byte, void> adapterSetItemSelected = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSelectionExternalStorage*, int, byte, void>))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9578) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b8d8528a35a2863a"></a>[`public void Inno.Native.ImGui.ImGuiSelectionExternalStorage.ApplyRequests(Inno.Native.ImGui.ImGuiMultiSelectIO* msIo)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64221) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fa86406403eb2527"></a>[`public void Inno.Native.ImGui.ImGuiSelectionExternalStorage.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64210) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1b57d0c6e8f54257"></a>[`public struct Inno.Native.ImGui.ImGuiSelectionExternalStorage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9574) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr eb83aaabd006c7c2eab9b340013e77dc2e9d028f3d9d5b75158659904beab460 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.#ctor(Inno.Native.ImGui.ImGuiSelectionExternalStorage*) 2126add0300d2256ec7006d97227dbfc973d844e66cdcf7a913d1e4640116b3f -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.Handle f2a9670b682e7b1cd666db62edc2fd8ea60eaeb318bb84de034ca23f81434838 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.IsNull bab3a83a09a7411deb55230993c181f5026f9a8fe87ec0d16f057b4e5d433401 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.Null 853ed3acaea025afa1a8ed8acac1bdc6edfd660b3490774b8cb4c664796daec7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.Item(System.Int32) 40df2e007cb595d5224eacf7f565f01ea3999b7cf35368cd2ae83e09a1eb77da -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.op_Implicit(Inno.Native.ImGui.ImGuiSelectionExternalStorage*)~Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr 5effacb5a02a7c842a662fa95fe8a1461efe3b8666affb59cdc58a26a4ce4c97 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.op_Implicit(Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr)~Inno.Native.ImGui.ImGuiSelectionExternalStorage* 05f7b57e89696ed46cc043da2ec5844682be0d9f1a9a6bd978ac579d85993ef0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.op_Equality(Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr,Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr) 62694c1ab579ec4ad9537349739bacf83d59884db93d4f6bbc6a743fdd7c4524 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.op_Inequality(Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr,Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr) b7ea11daf8ba424d048a900bbf9dc7351548c2388092eea09565affa9c026735 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.Equals(Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr) c86058a23ed207021acda014ee666bbeef32a2a2253440748360ecfa012caf44 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.Equals(System.Object) afe0b8e8751fa43160afc2282943638e99ba3fb3f3c2fc8bfb58ebde62e36da9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.GetHashCode ae22ef04eee8add608c52ad27e13a54d63ab6c962a82951c4ab1bce5136589be -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.UserData ae20ea09b0ecfbe78a351991d546cde39b0094aa226d25ae6aabdf9e276df513 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.AdapterSetItemSelected 827500b78c1d65d74c03c6787994a2ed781b08fb66a6387ac9d32e6e002573d7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.Destroy a5d62b1197e153e6379a74d99da058cd2219276bb054511084fb0f5311be2619 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.ApplyRequests(Inno.Native.ImGui.ImGuiMultiSelectIOPtr) c7d19c514dbfff875c86fefbc091b68b93d9775e9fd7a570bbef83837a0c0b09 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c99848ae7a75ab9c"></a>[`public Inno.Native.ImGui.ImGuiSelectionExternalStorage* Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16059) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6771cbb6f2798701"></a>[`public Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.ImGuiSelectionExternalStoragePtr(Inno.Native.ImGui.ImGuiSelectionExternalStorage* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16054) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d8434422845f6a08"></a>[`public void Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.ApplyRequests(Inno.Native.ImGui.ImGuiMultiSelectIOPtr msIo)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16087) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4f7cb33b20b7b1dc"></a>[`public void Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16079) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fb279f1b5fc51b3b"></a>[`public bool Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.Equals(Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16069) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-8ab36905be3e3042"></a>[`public override bool Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16070) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-52dedee7ed9c398d"></a>[`public override int Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16071) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-a7835bf8439418b4"></a>[`public static bool Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.operator ==(Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr left, Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16067) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d353d459858a45c3"></a>[`public static Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.implicit operator Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr(Inno.Native.ImGui.ImGuiSelectionExternalStorage* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16065) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5a601d9299add0cc"></a>[`public static Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.implicit operator Inno.Native.ImGui.ImGuiSelectionExternalStorage*(Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16066) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1792864eb551345f"></a>[`public static bool Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.operator !=(Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr left, Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16068) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-591e4e0a4c926fbd"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiSelectionExternalStorage*, int, byte, void> Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.AdapterSetItemSelected { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16074) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-97648b95e5224b52"></a>[`public bool Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16060) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4bbbe2f05bd73193"></a>[`public Inno.Native.ImGui.ImGuiSelectionExternalStorage Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16063) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-367e1ee340a1e481"></a>[`public static Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16061) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9ceb6fbbd84e178a"></a>[`public void* Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr.UserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16073) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7029a5b0214f68aa"></a>[`public struct Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr : System.IEquatable<Inno.Native.ImGui.ImGuiSelectionExternalStoragePtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16052) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSelectionRequest</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSelectionRequest fd5bc82f3c4e8b4714cb80ac6a62edd0342c640c57b4c3a7eb126d952c64cb63 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequest.Type 912aaed9983d797dc3c8ed5af47ce783cb1e5e209baee6a923707e4d90ac1d0d -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequest.Selected 439e579aff3c026844aa25be5329381f80316fd5d477dff3afd3b9c6876a20f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequest.RangeDirection 63a3eb3111be6464cb652dd06f20d15c49c591d455bbdc6b5afde4320d9e5d2f -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequest.RangeFirstItem 82b01665162b52883cfc34dca310be2ded37b097f4ea390e3840f730b165f821 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequest.RangeLastItem 1b473a0f9265aedc6475dd5269edcb1e6e44156c5694094296c9ecdd052b92b9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSelectionRequest.#ctor(Inno.Native.ImGui.ImGuiSelectionRequestType,System.Byte,System.Byte,System.Int64,System.Int64) 2e69a907018542ed1d346192c09f14955ca709f33d4f2bae9f2efdfa07ec711a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c732e00715c56833"></a>[`public byte Inno.Native.ImGui.ImGuiSelectionRequest.RangeDirection`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6858) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5bc01d7067aedb5d"></a>[`public long Inno.Native.ImGui.ImGuiSelectionRequest.RangeFirstItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6859) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21a9cc66509d1654"></a>[`public long Inno.Native.ImGui.ImGuiSelectionRequest.RangeLastItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6860) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fd76bcbd92be6db5"></a>[`public byte Inno.Native.ImGui.ImGuiSelectionRequest.Selected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6857) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-43a259c81397745d"></a>[`public Inno.Native.ImGui.ImGuiSelectionRequestType Inno.Native.ImGui.ImGuiSelectionRequest.Type`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6856) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5a9bedee647da11"></a>[`public Inno.Native.ImGui.ImGuiSelectionRequest.ImGuiSelectionRequest(Inno.Native.ImGui.ImGuiSelectionRequestType type = Inno.Native.ImGui.ImGuiSelectionRequestType.None, byte selected = 0, byte rangeDirection = 0, long rangeFirstItem = 0, long rangeLastItem = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6861) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1e0b4d7083d1fb1f"></a>[`public struct Inno.Native.ImGui.ImGuiSelectionRequest`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6854) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSelectionRequestType</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSelectionRequestType 24f93fd7e0acd8b5ccb8b6c4b7bdb89d130700aaf330b9b09408c34180e07be2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequestType.None 890642d17f89933cde0ae9f5f2d87aa69422571b5bc14a066b2a874bca64937a -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequestType.SetAll 4fcef370dc4b5135b5245033f73b867d5fdb7c37f233fc2de3f37a9e0b51ba04 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSelectionRequestType.SetRange 488fd2a83f0dd00e0451eb7bb156397a5899de4d0ab6a40658fd25c67872f852 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1d4857f5b6939363"></a>[`Inno.Native.ImGui.ImGuiSelectionRequestType.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L335) | 当前常量或枚举值：`0`。 | — |
| <a id="api-84eecf5aaef985b7"></a>[`Inno.Native.ImGui.ImGuiSelectionRequestType.SetAll = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L336) | 当前常量或枚举值：`1`。 | — |
| <a id="api-fbc7d82123b83360"></a>[`Inno.Native.ImGui.ImGuiSelectionRequestType.SetRange = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L337) | 当前常量或枚举值：`2`。 | — |
| <a id="api-f20b34622dcba05b"></a>[`public enum Inno.Native.ImGui.ImGuiSelectionRequestType : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L333) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSeparatorFlags</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSeparatorFlags bdc9ef417e68069562e4bff0da68d43ce44a9a2269a6032e9e7fc664ee9f8cb1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSeparatorFlags.None b23bd8aafd1757f4a386165acd26715946f2a3b22f53e22f4baf833e40169048 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSeparatorFlags.Horizontal 94062d97d692e139a296b66ab14317af98e9c90c795bdb2306368f85650444f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSeparatorFlags.Vertical 9d2dbe0e16857cbb252667910ca6221615985924c9930b3f3a3a87b7d93892f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSeparatorFlags.SpanAllColumns 0dcd0e6944441351e8237d723d33a84d2451674e2047877a8e4bd1cfcfc7fd7d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9543a3e5f1328e2f"></a>[`Inno.Native.ImGui.ImGuiSeparatorFlags.Horizontal = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1349) | 当前常量或枚举值：`1`。 | — |
| <a id="api-b8b36144ce4cd07f"></a>[`Inno.Native.ImGui.ImGuiSeparatorFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1348) | 当前常量或枚举值：`0`。 | — |
| <a id="api-89345f9edadb165f"></a>[`Inno.Native.ImGui.ImGuiSeparatorFlags.SpanAllColumns = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1351) | 当前常量或枚举值：`4`。 | — |
| <a id="api-98c8bd48b433031e"></a>[`Inno.Native.ImGui.ImGuiSeparatorFlags.Vertical = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1350) | 当前常量或枚举值：`2`。 | — |
| <a id="api-989188b8337c4397"></a>[`public enum Inno.Native.ImGui.ImGuiSeparatorFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1346) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSettingsHandler</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSettingsHandler 0161209c95d6636c3277bbe28087aedd5e9706bd36b3e3cb746ad8e515c68372 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.TypeName c72a321c1b3bc6314d5dc6b6f445fdace8e448d282206b2dca5817c7b45915b6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.TypeHash 8d6bd0f30e17b244e349533c9839ad043c3d761864d92fecbef4b6c2fca18354 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.ClearAllFn dd68075e964ff7da73c3d56fad3ddde31a456806b98b427e308bfcb8bf7e4d64 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.ReadInitFn 0131b2a57f10b31f6318bdd1b6eddbf0a6383286f144d86b9d6addc5e642bb51 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.ReadOpenFn 214524518abd909b3c9b4e0d58075a3d6f16c3d40b92780e1795641b0881d29c -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.ReadLineFn 6af9cd7fd1821d325d5cfc97898891a12cb637c3b91dc1b8f0055b624ba545f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.ApplyAllFn 15bb4d792c4212ad5f0e3a948d6fc89235678d7a565d111296fea9a037ef12a1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.WriteAllFn 050fe28049f55cceee8b5a5859e5bb638e869491a94517b07e5995e443db5159 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandler.UserData 03c4ac0c0c388ca330cb4a5bf2042cb034222b94bbcf7ca3539cdb42c725b875 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSettingsHandler.#ctor(System.Byte*,System.UInt32,,,,,,,System.Void*) 6879fef1a6f9c4c3bdf03f544f9c67f7b6146cce57704b282ca2879852735a9f -->
<!-- api: M:Inno.Native.ImGui.ImGuiSettingsHandler.Destroy 10c874c283ccb36a1a5670ab84ae85e9c2abb91178735bc4bb33ddb8338c6158 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-346e117589a8ffd2"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void> Inno.Native.ImGui.ImGuiSettingsHandler.ApplyAllFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7372) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5d6cb4e600042bbe"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void> Inno.Native.ImGui.ImGuiSettingsHandler.ClearAllFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7368) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfdbc831f81321dc"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void> Inno.Native.ImGui.ImGuiSettingsHandler.ReadInitFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7369) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4232ffd44543f674"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void*, byte*, void> Inno.Native.ImGui.ImGuiSettingsHandler.ReadLineFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7371) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c4450f3054f49e9"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, byte*, void*> Inno.Native.ImGui.ImGuiSettingsHandler.ReadOpenFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7370) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a0532820394aedd"></a>[`public uint Inno.Native.ImGui.ImGuiSettingsHandler.TypeHash`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7367) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c9458ce627c515ac"></a>[`public byte* Inno.Native.ImGui.ImGuiSettingsHandler.TypeName`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7366) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d04859f133f6027"></a>[`public void* Inno.Native.ImGui.ImGuiSettingsHandler.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7374) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2c23c45fde6bde39"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, Inno.Native.ImGui.ImGuiTextBuffer*, void> Inno.Native.ImGui.ImGuiSettingsHandler.WriteAllFn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7373) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c0569176d1d1c1a"></a>[`public Inno.Native.ImGui.ImGuiSettingsHandler.ImGuiSettingsHandler(byte* typeName = null, uint typeHash = 0, delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void> clearAllFn = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void> readInitFn = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, byte*, void*> readOpenFn = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, byte*, void*>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void*, byte*, void> readLineFn = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void*, byte*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void> applyAllFn = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void>), delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, Inno.Native.ImGui.ImGuiTextBuffer*, void> writeAllFn = default(delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, Inno.Native.ImGui.ImGuiTextBuffer*, void>), void* userData = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7375) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-becdd343fa2a0520"></a>[`public void Inno.Native.ImGui.ImGuiSettingsHandler.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67347) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d06a280a467021cf"></a>[`public struct Inno.Native.ImGui.ImGuiSettingsHandler`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7364) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSettingsHandlerPtr</code> · 23 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSettingsHandlerPtr e5c4627a9b76fb6c746a5373484f2b917ba7ca7281397a1d1e95faf7bc26f823 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.#ctor(Inno.Native.ImGui.ImGuiSettingsHandler*) bbe4f43516fd5d07ea743bcfe665f72e388b720256fae50d4b61329de88b4d6e -->
<!-- api: F:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.Handle a0e1eccb23ff3e03643f7a9b4720a1cb6fc568cda3aedb2d22e8b753f8bc6162 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.IsNull 24021382dbc0f476f0c9efaf5170d4ceafce507093bc34aad41b69596b02442c -->
<!-- api: P:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.Null 0e495dc0e68ee962e5588772baa9dec080afb5d816b38a11a4b2349fd999ca92 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.Item(System.Int32) b77bf2b7c0b02ec58242e53164a67bbcc0f23ef64409544403e14e3c379d784c -->
<!-- api: M:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.op_Implicit(Inno.Native.ImGui.ImGuiSettingsHandler*)~Inno.Native.ImGui.ImGuiSettingsHandlerPtr 78ef8ba54edca461038a2663fb4179a9083b0f2e6b605146261501501d788ce6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.op_Implicit(Inno.Native.ImGui.ImGuiSettingsHandlerPtr)~Inno.Native.ImGui.ImGuiSettingsHandler* a2810f73f891c66ee18b918f38209d73b7320f500d068b3454cda23c0e468b62 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.op_Equality(Inno.Native.ImGui.ImGuiSettingsHandlerPtr,Inno.Native.ImGui.ImGuiSettingsHandlerPtr) f87e772d9c3b3b4bf8f6db2870dd3859fd5d106ae2d61f32fef148fa929aa943 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.op_Inequality(Inno.Native.ImGui.ImGuiSettingsHandlerPtr,Inno.Native.ImGui.ImGuiSettingsHandlerPtr) 56bb7c81e50ba56055584db1e2fac6343fd86a465287dc214e199a197c890b7f -->
<!-- api: M:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.Equals(Inno.Native.ImGui.ImGuiSettingsHandlerPtr) 7b0a1faa6881b9c7c42143589fd1fb57f1debbbc0a901aecba5217cc9621b3a8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.Equals(System.Object) c18e7a72574aab3541556e043bc8c8e0a1c3e237a354fd16eb23afa8186e02f2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.GetHashCode eaf76619181296a438daf92281227cbdf429a43ceb0d2d60263fa8c15472101e -->
<!-- api: P:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.TypeName f776909edf3017f6170f3949e792928c6b5198ecee7513eec8f7bcac2b7b905c -->
<!-- api: P:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.TypeHash 6464f9459132456fb3e77e70b5ab9e5fea1bc52063cb6809351b518030dcb937 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.ClearAllFn 0a5226d4aaddad5df4f692862e509b8587fcd1c8663b61166188978ec0d42e14 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.ReadInitFn 717f24b60f10199293d7682c7792033ea90dcb711351d0d68a6dce136e6f4ff8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.ReadOpenFn 6ae4cdc8e4f6a4b897e530f4a4e49527c74c7128911145416d134826d8556757 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.ReadLineFn 6449352c893241406790eda5572e2eeaac53078a1618bedf5353553d8ef5d435 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.ApplyAllFn d0b3648ed018335a7083908e2b06464a12c92252497c506b00a011559f859c9e -->
<!-- api: P:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.WriteAllFn bffee7a63446b02f46d896303d7ab907955e6713e236dcba9c4a91da4e2d4513 -->
<!-- api: P:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.UserData 84bc930f377d07be05b3d6ae8c6cfe23aab48a1463653936b3b69dbd3323db66 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSettingsHandlerPtr.Destroy 88fe2f55564088eb386a7776e2556ee54e6da87818661a81e024b6414f69cef3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ecf7f4d6b398d0d6"></a>[`public Inno.Native.ImGui.ImGuiSettingsHandler* Inno.Native.ImGui.ImGuiSettingsHandlerPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16101) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-04d27cc356ac2e3a"></a>[`public Inno.Native.ImGui.ImGuiSettingsHandlerPtr.ImGuiSettingsHandlerPtr(Inno.Native.ImGui.ImGuiSettingsHandler* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16096) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-28b045b0bd4681d4"></a>[`public void Inno.Native.ImGui.ImGuiSettingsHandlerPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16128) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0561e269be1da5aa"></a>[`public bool Inno.Native.ImGui.ImGuiSettingsHandlerPtr.Equals(Inno.Native.ImGui.ImGuiSettingsHandlerPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16111) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-3d333d96ee9b15b9"></a>[`public override bool Inno.Native.ImGui.ImGuiSettingsHandlerPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16112) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-c4020204642d08ae"></a>[`public override int Inno.Native.ImGui.ImGuiSettingsHandlerPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16113) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-64e26f570aa84774"></a>[`public static bool Inno.Native.ImGui.ImGuiSettingsHandlerPtr.operator ==(Inno.Native.ImGui.ImGuiSettingsHandlerPtr left, Inno.Native.ImGui.ImGuiSettingsHandlerPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16109) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a53eee52adc2fd74"></a>[`public static Inno.Native.ImGui.ImGuiSettingsHandlerPtr.implicit operator Inno.Native.ImGui.ImGuiSettingsHandlerPtr(Inno.Native.ImGui.ImGuiSettingsHandler* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16107) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f91cf831fce77b42"></a>[`public static Inno.Native.ImGui.ImGuiSettingsHandlerPtr.implicit operator Inno.Native.ImGui.ImGuiSettingsHandler*(Inno.Native.ImGui.ImGuiSettingsHandlerPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16108) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-18c5129590afec27"></a>[`public static bool Inno.Native.ImGui.ImGuiSettingsHandlerPtr.operator !=(Inno.Native.ImGui.ImGuiSettingsHandlerPtr left, Inno.Native.ImGui.ImGuiSettingsHandlerPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16110) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f8e52124d127a51d"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void> Inno.Native.ImGui.ImGuiSettingsHandlerPtr.ApplyAllFn { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16121) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eafc317eeddba294"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void> Inno.Native.ImGui.ImGuiSettingsHandlerPtr.ClearAllFn { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16117) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-688f0228be07fe72"></a>[`public bool Inno.Native.ImGui.ImGuiSettingsHandlerPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16102) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-928add80fc88948c"></a>[`public Inno.Native.ImGui.ImGuiSettingsHandler Inno.Native.ImGui.ImGuiSettingsHandlerPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16105) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-98660381f5fdef68"></a>[`public static Inno.Native.ImGui.ImGuiSettingsHandlerPtr Inno.Native.ImGui.ImGuiSettingsHandlerPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16103) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cc8c23adcfdd5b89"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void> Inno.Native.ImGui.ImGuiSettingsHandlerPtr.ReadInitFn { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16118) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a28c6ae74e966f8b"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, void*, byte*, void> Inno.Native.ImGui.ImGuiSettingsHandlerPtr.ReadLineFn { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16120) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bcbbb6cd0a6e887c"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, byte*, void*> Inno.Native.ImGui.ImGuiSettingsHandlerPtr.ReadOpenFn { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16119) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-de16ced30fe6e891"></a>[`public ref uint Inno.Native.ImGui.ImGuiSettingsHandlerPtr.TypeHash { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16116) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e577f6d950c8622c"></a>[`public byte* Inno.Native.ImGui.ImGuiSettingsHandlerPtr.TypeName { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16115) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b4580388321d750e"></a>[`public void* Inno.Native.ImGui.ImGuiSettingsHandlerPtr.UserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16123) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1560ebe11484c513"></a>[`public delegate* unmanaged[Cdecl]<Inno.Native.ImGui.ImGuiContext*, Inno.Native.ImGui.ImGuiSettingsHandler*, Inno.Native.ImGui.ImGuiTextBuffer*, void> Inno.Native.ImGui.ImGuiSettingsHandlerPtr.WriteAllFn { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16122) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-135b14050515ef65"></a>[`public struct Inno.Native.ImGui.ImGuiSettingsHandlerPtr : System.IEquatable<Inno.Native.ImGui.ImGuiSettingsHandlerPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16094) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiShrinkWidthItem</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiShrinkWidthItem 027f6c7da2e94004a1a52af861cb45fa75466ef2a104c34492c21cf3a9c0032a -->
<!-- api: F:Inno.Native.ImGui.ImGuiShrinkWidthItem.Index 346aa0e2bde67fc3ed786fd8dd5a3cae0425ebc3499bd81d3c928a81780d6240 -->
<!-- api: F:Inno.Native.ImGui.ImGuiShrinkWidthItem.Width 0a25f0ea90677f5724b61bc233ddc575e5034d96bd96cfa5c7dd242701311d81 -->
<!-- api: F:Inno.Native.ImGui.ImGuiShrinkWidthItem.InitialWidth 3374afd5325b34eb9701d23ececfdf8592cd4ae589beb3086190e33c9ff250f2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiShrinkWidthItem.#ctor(System.Int32,System.Single,System.Single) 5d7071550d421760a9683ff447735346360e42bf7a1aca48d2d8057ed62bb5b0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-bd1ef678472e8df7"></a>[`public int Inno.Native.ImGui.ImGuiShrinkWidthItem.Index`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6779) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b864fc20f2d9ba32"></a>[`public float Inno.Native.ImGui.ImGuiShrinkWidthItem.InitialWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6781) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e944f6455fd44621"></a>[`public float Inno.Native.ImGui.ImGuiShrinkWidthItem.Width`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6780) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9ce1baf699172eed"></a>[`public Inno.Native.ImGui.ImGuiShrinkWidthItem.ImGuiShrinkWidthItem(int index = 0, float width = 0, float initialWidth = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6782) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e862ccc44c49bb9e"></a>[`public struct Inno.Native.ImGui.ImGuiShrinkWidthItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6777) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiShrinkWidthItemPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr 85e3133cb715d528e1625b339bfc2920e1f6484062a0dbbd5cc9c991a0e55dce -->
<!-- api: M:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.#ctor(Inno.Native.ImGui.ImGuiShrinkWidthItem*) 038b93df29502ed8ba14cc0bf0b69bbd167b4affa5f307ae627905c82565694c -->
<!-- api: F:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.Handle 40aaf42a417a6b7e9794452b4ab6ceef49a790f92d60e7154aa4753c7768003d -->
<!-- api: P:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.IsNull 42d12dc142699ac3438aeb5e34abbf0d8fde09b05120186c72512cd5ba4311fe -->
<!-- api: P:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.Null 93dd4e4ae8390dc7fd5b9717f5d5a4ec837661c3bdef39354e0b770a722dca88 -->
<!-- api: P:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.Item(System.Int32) ed2dc21816ff5246bd9cd9d55d4bca31e3af8a35ba107e99354878189997c4a8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.op_Implicit(Inno.Native.ImGui.ImGuiShrinkWidthItem*)~Inno.Native.ImGui.ImGuiShrinkWidthItemPtr 498364ea79a1965d5981f88de7245c322a7291fce7036496e0cb35da185191ad -->
<!-- api: M:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.op_Implicit(Inno.Native.ImGui.ImGuiShrinkWidthItemPtr)~Inno.Native.ImGui.ImGuiShrinkWidthItem* 4ecc873046cf7aa2779d9bc39dc164b53e12006099d6814cd7c42e47a9cfe0af -->
<!-- api: M:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.op_Equality(Inno.Native.ImGui.ImGuiShrinkWidthItemPtr,Inno.Native.ImGui.ImGuiShrinkWidthItemPtr) 0f807f0f2c0dc91e99cdf43b22d1fd5d88e21856ee1df7f9dec306fd71a9015c -->
<!-- api: M:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.op_Inequality(Inno.Native.ImGui.ImGuiShrinkWidthItemPtr,Inno.Native.ImGui.ImGuiShrinkWidthItemPtr) 2101854b8b5a3b6eb5962478a2167865dabdad48b3aca922d1267b9714c36f92 -->
<!-- api: M:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.Equals(Inno.Native.ImGui.ImGuiShrinkWidthItemPtr) 9bba9222136a1e0e06c9b3a77daf904fd44b85a5c2a71c539635ea7fa1a4f736 -->
<!-- api: M:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.Equals(System.Object) bfc8708ab11bd4c8034fc2b67c13fd2633d850c6de66d0c73aeb914b4c1e4ddd -->
<!-- api: M:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.GetHashCode da2a778505e1c5808f29245ba22af5b406db6ddefc8d3b6f5d856e5cf51c770b -->
<!-- api: P:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.Index 0c99cf11bc46c4471d3e41e53165daece16817d5eebce88893a64f858c25ca40 -->
<!-- api: P:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.Width 86773ad321e1f0d44d931c8a1e73f595af500d86c0358dfcb9c2784e17541a9e -->
<!-- api: P:Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.InitialWidth 76c5739d00aa84eee025934c3a85591aeda9bedb3183269bc559ae07c3596fc8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-faac9409470cdbc8"></a>[`public Inno.Native.ImGui.ImGuiShrinkWidthItem* Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16142) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a0ff98ff158a5fb6"></a>[`public Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.ImGuiShrinkWidthItemPtr(Inno.Native.ImGui.ImGuiShrinkWidthItem* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16137) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-da4cda63aa96a041"></a>[`public bool Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.Equals(Inno.Native.ImGui.ImGuiShrinkWidthItemPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16152) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-1fde644da7f76347"></a>[`public override bool Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16153) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-8dce48c5d8a1cbca"></a>[`public override int Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16154) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-87d5067a8e0d58f2"></a>[`public static bool Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.operator ==(Inno.Native.ImGui.ImGuiShrinkWidthItemPtr left, Inno.Native.ImGui.ImGuiShrinkWidthItemPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16150) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a201bda8db1f366e"></a>[`public static Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.implicit operator Inno.Native.ImGui.ImGuiShrinkWidthItemPtr(Inno.Native.ImGui.ImGuiShrinkWidthItem* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16148) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6e55946d48692b24"></a>[`public static Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.implicit operator Inno.Native.ImGui.ImGuiShrinkWidthItem*(Inno.Native.ImGui.ImGuiShrinkWidthItemPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16149) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3074caf19090885c"></a>[`public static bool Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.operator !=(Inno.Native.ImGui.ImGuiShrinkWidthItemPtr left, Inno.Native.ImGui.ImGuiShrinkWidthItemPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16151) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e9575845ce6903b0"></a>[`public ref int Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.Index { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16156) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-94fc3667c1cdb53a"></a>[`public ref float Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.InitialWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16158) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9fd2afdfe24bf0b8"></a>[`public bool Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16143) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-97bdaebffee71c96"></a>[`public Inno.Native.ImGui.ImGuiShrinkWidthItem Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16146) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-74c12920d5a02e48"></a>[`public static Inno.Native.ImGui.ImGuiShrinkWidthItemPtr Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16144) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f0015bda71145105"></a>[`public ref float Inno.Native.ImGui.ImGuiShrinkWidthItemPtr.Width { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16157) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-132b507e60e7bab2"></a>[`public struct Inno.Native.ImGui.ImGuiShrinkWidthItemPtr : System.IEquatable<Inno.Native.ImGui.ImGuiShrinkWidthItemPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16135) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSizeCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSizeCallback 05c284f79654aa8d571daa135e8a9400c35f414a970acef8ceae75378fc68b6e -->
<!-- api: M:Inno.Native.ImGui.ImGuiSizeCallback.#ctor(System.Object,System.IntPtr) f9c0865c91ea5f6d39bd5911d221f24083c58432c3496aa8e173a63245f7978f -->
<!-- api: M:Inno.Native.ImGui.ImGuiSizeCallback.Invoke(Inno.Native.ImGui.ImGuiSizeCallbackData*) 8fc20a07a69044c8d9ccf369dcbd72a441ca582dc691b0d71b03f65f09cc6479 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSizeCallback.BeginInvoke(Inno.Native.ImGui.ImGuiSizeCallbackData*,System.AsyncCallback,System.Object) c16364e4c1408b5253c6bd26549afd9fbe61712e47ff532f9e39b892dfd28e1b -->
<!-- api: M:Inno.Native.ImGui.ImGuiSizeCallback.EndInvoke(System.IAsyncResult) 6b3510636859efbb058d8f1d40248edf194486521832a0f00e1cec4495de2d22 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a8b446c149b57d0a"></a>[`public Inno.Native.ImGui.ImGuiSizeCallback.ImGuiSizeCallback(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10578) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-39e46a8177fda1bd"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.ImGuiSizeCallback.BeginInvoke(Inno.Native.ImGui.ImGuiSizeCallbackData* data, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10578) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0c7ce762f118a35f"></a>[`public virtual void Inno.Native.ImGui.ImGuiSizeCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10578) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ef80a9f5274cc6bd"></a>[`public virtual void Inno.Native.ImGui.ImGuiSizeCallback.Invoke(Inno.Native.ImGui.ImGuiSizeCallbackData* data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10578) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-150101be4d1d00aa"></a>[`public delegate void Inno.Native.ImGui.ImGuiSizeCallback(Inno.Native.ImGui.ImGuiSizeCallbackData* data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10578) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSizeCallbackData</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSizeCallbackData d8742dea64793073636396c3f1eef4df99ee0425e90f641f3b0efeee20cb62aa -->
<!-- api: F:Inno.Native.ImGui.ImGuiSizeCallbackData.UserData be3a97ca264399fa41dee8c6daacf0a05010d84422edd662532c58caa9b51c86 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSizeCallbackData.Pos c451c3a286e5de2b09d143db955768d3718d663d592d4a526968ce0c1b444821 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSizeCallbackData.CurrentSize 71386dbde0e013bf98344abf7fbd143c1cb3114b739159f07150ed262df9c092 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSizeCallbackData.DesiredSize c43f1410719cde0f765360f6cb85c975a6c2bddf80133f8dc0c5911c7b06ff10 -->
<!-- api: M:Inno.Native.ImGui.ImGuiSizeCallbackData.#ctor(System.Void*,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2) a327de8e5922e37f9417489647f48aedf9c0443bd1b1055a361901c43b43a036 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d442d220ed35e07d"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiSizeCallbackData.CurrentSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5601) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd10c8cc259e2a7d"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiSizeCallbackData.DesiredSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5602) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f33a1facf69000ef"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiSizeCallbackData.Pos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5600) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-20dd9de962a805fc"></a>[`public void* Inno.Native.ImGui.ImGuiSizeCallbackData.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5599) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-228a08ae3e38247b"></a>[`public Inno.Native.ImGui.ImGuiSizeCallbackData.ImGuiSizeCallbackData(void* userData = null, System.Numerics.Vector2 pos = default(System.Numerics.Vector2), System.Numerics.Vector2 currentSize = default(System.Numerics.Vector2), System.Numerics.Vector2 desiredSize = default(System.Numerics.Vector2))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5603) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0360eae324546dfc"></a>[`public struct Inno.Native.ImGui.ImGuiSizeCallbackData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5597) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSliderFlags</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSliderFlags 524536b69c2097e366d878ccdd338c151faee4d25b37f61dacab640fbd74c7f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.None a34ebe2706b271589785fcf5fd99984138cff2bec9595a912cee571ef5074e9c -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.Logarithmic 249b38753fe4ef6c0146a76db58dd3305bbfabfb347b341bd6be109fe7630370 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.NoRoundToFormat 5585f87ce96dcab43110afde1ea8f1a806dabcfb2714ebc02b15b95f81828665 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.NoInput 84cbe87b325e4690e20006369bd2474327c8c541cad3e45eb6efb286c6783f11 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.WrapAround e74ea226788b9a086db0607fb77ee7b814cca64d8e57263e87000132286dd5b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.ClampOnInput 48095b7e1ba7791347c5bc52cfe1d502525224c48feb095c4292a77bb315e26b -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.ClampZeroRange 7f73682be5d9d6083c16768d228198716e7d41bde886017474bc454fbd520253 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.NoSpeedTweaks ad8639c9538dcf9ca9d913315aa639c3a319990590c652ff442cec1a52bef3be -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.ColorMarkers 455ad6b332005c01a47bfaeb466d38badea8bb791c762e29103ef7aca936338f -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.AlwaysClamp e20bb75ad3f5496a27545f4c98bddb5b53262e5f7a6174e64367ecb161b5a56b -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlags.InvalidMask a91b94be547d055d0e068050dad99ea3a6921e15936262ca657201c3a3daf506 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b20c87c52c5b8e2a"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.AlwaysClamp = 1536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L931) | 当前常量或枚举值：`1536`。 | — |
| <a id="api-93e9795cdd25cd72"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.ClampOnInput = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L927) | 当前常量或枚举值：`512`。 | — |
| <a id="api-f4f24eb67af2604f"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.ClampZeroRange = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L928) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-88218c2539a4fcde"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.ColorMarkers = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L930) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-1d81c235421dcdaf"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.InvalidMask = 1879048207`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L932) | 当前常量或枚举值：`1879048207`。 | — |
| <a id="api-98509f542dc5e1a1"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.Logarithmic = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L923) | 当前常量或枚举值：`32`。 | — |
| <a id="api-d1b73c2eddbceba3"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.NoInput = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L925) | 当前常量或枚举值：`128`。 | — |
| <a id="api-0a4ab044b219dd5c"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L922) | 当前常量或枚举值：`0`。 | — |
| <a id="api-ebe87c4f0af59c04"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.NoRoundToFormat = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L924) | 当前常量或枚举值：`64`。 | — |
| <a id="api-ab881b9f26ab1df7"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.NoSpeedTweaks = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L929) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-e6a6876847099d17"></a>[`Inno.Native.ImGui.ImGuiSliderFlags.WrapAround = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L926) | 当前常量或枚举值：`256`。 | — |
| <a id="api-e959aebe92713c66"></a>[`public enum Inno.Native.ImGui.ImGuiSliderFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L920) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSliderFlagsPrivate</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSliderFlagsPrivate 445906240d5ddd53a149faaa41643f798ba08a1aff613a77f0494033c2295e9a -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlagsPrivate.Vertical 96b4e680a45a9db804e099d0b38ec629d52748e27f7390ba2a6dd86a54fe4996 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSliderFlagsPrivate.ReadOnly 1a8afefbd520105f50ed919104abda81687d42827970740f8beb5b9ec082b9eb -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f9d93215fa071f02"></a>[`Inno.Native.ImGui.ImGuiSliderFlagsPrivate.ReadOnly = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1314) | 当前常量或枚举值：`2097152`。 | — |
| <a id="api-09655decd92fb245"></a>[`Inno.Native.ImGui.ImGuiSliderFlagsPrivate.Vertical = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1313) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-50a60d9833b72204"></a>[`public enum Inno.Native.ImGui.ImGuiSliderFlagsPrivate : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1311) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiSortDirection</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiSortDirection ba195608dea2fca2c4816e27dae5301876f5981ff683f42807ebbbb64067f786 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSortDirection.None 9a1df5a6ad82f1819f56c2ca6947b64d475b7b4ff1941cfb61fa65f7d4ecc403 -->
<!-- api: F:Inno.Native.ImGui.ImGuiSortDirection.Ascending 7064f3ea2115a7e817d01f9bd5d29bfa38a133846db55ce165e3137f348a6c6b -->
<!-- api: F:Inno.Native.ImGui.ImGuiSortDirection.Descending 4876418496fb29f18b04263faef51ee740eda2b961c13ce643b880a41b87e98c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6fd71dc4e707eac0"></a>[`Inno.Native.ImGui.ImGuiSortDirection.Ascending = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L326) | 当前常量或枚举值：`1`。 | — |
| <a id="api-7c38738a3fe33dae"></a>[`Inno.Native.ImGui.ImGuiSortDirection.Descending = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L327) | 当前常量或枚举值：`2`。 | — |
| <a id="api-2885d34ac884f449"></a>[`Inno.Native.ImGui.ImGuiSortDirection.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L325) | 当前常量或枚举值：`0`。 | — |
| <a id="api-9729641c55ed02ec"></a>[`public enum Inno.Native.ImGui.ImGuiSortDirection : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L323) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStackLevelInfo</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStackLevelInfo 749dc36e07fc64923112933be7a0f0c862adbd973f7e68c7e90ca1d260e68845 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStackLevelInfo.ID 322b5bb16a4a954f081c2bcafea7c89f6d4b6f69aaedaf3a76d7a854d214ebe3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStackLevelInfo.QueryFrameCount 29915495c12dbc05cf7a9317b1fbd22fbb1ede4278a0420e06128c131e598886 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStackLevelInfo.QuerySuccess a007222545b3abae22716bba89a9b77459e08fa1ad1863620710551faacab831 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStackLevelInfo.DataType b3cea7758746eae415c4bee9cf604537a7804ce197834ec092f9379430816ba6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStackLevelInfo.DescOffset 7dd8e2673505ffb9c515f3a914afb221cc877ae1d03a1564eb13c7e236063e9b -->
<!-- api: M:Inno.Native.ImGui.ImGuiStackLevelInfo.#ctor(System.UInt32,System.Byte,System.Byte,System.Byte,System.Int32) 95aac01e8c086839a58c3244c959bdc245c947f6c8d055b8f9dfeea6bfc4eca9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStackLevelInfo.Destroy 83a1b5c80978fd3a8aa00fd36a4da1dfe0f582969750f552ebc87ae656425f58 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4e7ffa5040467b8c"></a>[`public byte Inno.Native.ImGui.ImGuiStackLevelInfo.DataType`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7486) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-894b214a2b2bbca5"></a>[`public int Inno.Native.ImGui.ImGuiStackLevelInfo.DescOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7487) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-34d24e3ce2f1145d"></a>[`public uint Inno.Native.ImGui.ImGuiStackLevelInfo.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7483) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d844c334b7dcdd62"></a>[`public byte Inno.Native.ImGui.ImGuiStackLevelInfo.QueryFrameCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7484) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bd51c14691f16c45"></a>[`public byte Inno.Native.ImGui.ImGuiStackLevelInfo.QuerySuccess`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7485) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4fe57b4cdfe67f67"></a>[`public Inno.Native.ImGui.ImGuiStackLevelInfo.ImGuiStackLevelInfo(uint iD = 0, byte queryFrameCount = 0, byte querySuccess = 0, byte dataType = 0, int descOffset = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7488) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b8052463e215a116"></a>[`public void Inno.Native.ImGui.ImGuiStackLevelInfo.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67375) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1bdec96b67a0d623"></a>[`public struct Inno.Native.ImGui.ImGuiStackLevelInfo`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7481) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStackLevelInfoPtr</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStackLevelInfoPtr fbf23bff89351fed45db99326ccd7032629d25e1ac62d750d38767eba9c29b53 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.#ctor(Inno.Native.ImGui.ImGuiStackLevelInfo*) cd771aea4745e293090b10ce5db0b3f747a05eb195db1c72d00d80deff6ec6ff -->
<!-- api: F:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.Handle f187bcfe30f8b0f57a325827c87d51d6a3033d7033e0cd25a15a3174eabd8f82 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.IsNull 6084b7aa1bee20af35cf271b51c4c04f26cfcf80871b248d4f1958b113ab7731 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.Null 6026a14226484db1d4d8e30ae256c75c5d6ccc4b1f02ed9e4f37f2c4157e5692 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.Item(System.Int32) eed4a9385e5299d710f68f752661c7e1502320b59de1cc8b03e57d6b5adeb788 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.op_Implicit(Inno.Native.ImGui.ImGuiStackLevelInfo*)~Inno.Native.ImGui.ImGuiStackLevelInfoPtr 583d6e8af8fd2f67ea99ed089cbd5a29d19fba08198d82297e57c17d3e2230a5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.op_Implicit(Inno.Native.ImGui.ImGuiStackLevelInfoPtr)~Inno.Native.ImGui.ImGuiStackLevelInfo* 82ffb8f8d46192d30cfa83a21c70b30ac717da18de4e3a093215522537b8b76e -->
<!-- api: M:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.op_Equality(Inno.Native.ImGui.ImGuiStackLevelInfoPtr,Inno.Native.ImGui.ImGuiStackLevelInfoPtr) 2de01d3e8562b1007abc814fa81eae5e924f4a25d835d8e8722d0d18bc620607 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.op_Inequality(Inno.Native.ImGui.ImGuiStackLevelInfoPtr,Inno.Native.ImGui.ImGuiStackLevelInfoPtr) 3b81e525545c23e6fe2b7980564cc546fa84c37dd7b19becc6caf09b81a0f735 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.Equals(Inno.Native.ImGui.ImGuiStackLevelInfoPtr) 73d679faac82f1e202e4170c5fd5ede68ae194251da6d3e2934e6e02506be88f -->
<!-- api: M:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.Equals(System.Object) 7e06c360745378a4ea839aa88a45d1eb1ac2b3f1cb1fd4c2a3a09a9b013015cb -->
<!-- api: M:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.GetHashCode 50b975ae0903b8061a64a084aed25fbb61b245c7e54b04cdb0301f9e412c540a -->
<!-- api: P:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.ID c714e332e0a0a79f6f585eecc85773ff1c4193f128680fc4e2ad9a04a5d3169e -->
<!-- api: P:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.QueryFrameCount 5aaaf89bfe2d64cf93d353af9b3772b65bf6b78186dd18e1418acd102e2b49d9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.QuerySuccess 2ec15198682f74291c672f0c818f3e2fa103408f9a786eda887610be97ca73f2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.DataType b5cc33abe55a255c5d8a3333bd344a9928cee71bd3fd1ec141d5d6a0ac8b8849 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.DescOffset caa5df78dd832faed86cdc7b6f940cf6f3556ca7588b38479bb67dc21f352b6a -->
<!-- api: M:Inno.Native.ImGui.ImGuiStackLevelInfoPtr.Destroy ac5eeece4325fb4790f00a2faafb18ac953bed2abe6aab63756a721f80315d66 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fdb3d4d0f915e8d6"></a>[`public Inno.Native.ImGui.ImGuiStackLevelInfo* Inno.Native.ImGui.ImGuiStackLevelInfoPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16169) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5b03d883580ef138"></a>[`public Inno.Native.ImGui.ImGuiStackLevelInfoPtr.ImGuiStackLevelInfoPtr(Inno.Native.ImGui.ImGuiStackLevelInfo* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16164) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fdfdafdbb020b782"></a>[`public void Inno.Native.ImGui.ImGuiStackLevelInfoPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16192) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e2d0043541d8629e"></a>[`public bool Inno.Native.ImGui.ImGuiStackLevelInfoPtr.Equals(Inno.Native.ImGui.ImGuiStackLevelInfoPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16179) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-ec35d60e6ea7a40a"></a>[`public override bool Inno.Native.ImGui.ImGuiStackLevelInfoPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16180) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-01d197fac037edcb"></a>[`public override int Inno.Native.ImGui.ImGuiStackLevelInfoPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16181) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-ac40bace75e97290"></a>[`public static bool Inno.Native.ImGui.ImGuiStackLevelInfoPtr.operator ==(Inno.Native.ImGui.ImGuiStackLevelInfoPtr left, Inno.Native.ImGui.ImGuiStackLevelInfoPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16177) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2abcb9911dbf423b"></a>[`public static Inno.Native.ImGui.ImGuiStackLevelInfoPtr.implicit operator Inno.Native.ImGui.ImGuiStackLevelInfoPtr(Inno.Native.ImGui.ImGuiStackLevelInfo* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16175) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c615a3d9db13b670"></a>[`public static Inno.Native.ImGui.ImGuiStackLevelInfoPtr.implicit operator Inno.Native.ImGui.ImGuiStackLevelInfo*(Inno.Native.ImGui.ImGuiStackLevelInfoPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16176) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-29f3a9b8fa01b5b6"></a>[`public static bool Inno.Native.ImGui.ImGuiStackLevelInfoPtr.operator !=(Inno.Native.ImGui.ImGuiStackLevelInfoPtr left, Inno.Native.ImGui.ImGuiStackLevelInfoPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16178) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8f4d28d58297bb96"></a>[`public ref byte Inno.Native.ImGui.ImGuiStackLevelInfoPtr.DataType { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16186) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a97c8c7c04794c82"></a>[`public ref int Inno.Native.ImGui.ImGuiStackLevelInfoPtr.DescOffset { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16187) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4e2fc91018f81d2c"></a>[`public ref uint Inno.Native.ImGui.ImGuiStackLevelInfoPtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16183) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9ba3e9b02179b213"></a>[`public bool Inno.Native.ImGui.ImGuiStackLevelInfoPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16170) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eda56e3375a80919"></a>[`public Inno.Native.ImGui.ImGuiStackLevelInfo Inno.Native.ImGui.ImGuiStackLevelInfoPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16173) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7904dc8c1baa16e2"></a>[`public static Inno.Native.ImGui.ImGuiStackLevelInfoPtr Inno.Native.ImGui.ImGuiStackLevelInfoPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16171) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1aef179ab46a375d"></a>[`public ref byte Inno.Native.ImGui.ImGuiStackLevelInfoPtr.QueryFrameCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16184) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d0085e76d543fafa"></a>[`public bool Inno.Native.ImGui.ImGuiStackLevelInfoPtr.QuerySuccess { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16185) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f5839eafee256102"></a>[`public struct Inno.Native.ImGui.ImGuiStackLevelInfoPtr : System.IEquatable<Inno.Native.ImGui.ImGuiStackLevelInfoPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16162) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStorage</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStorage 96181b5ca00bec51261eb1c1439bc975b4f232903786694e3f276e4be6381d91 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStorage.Data bb934d46136e6beeb9b6b2db5d8e7d3756c74ef592b73e9d1c14500d38ba569c -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.#ctor(Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiStoragePair}) cfca1d825f03c7e4dd98e5db931f1c354ecff3dbe763961adf1b8d6166e5a5cf -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.Clear 3e92a66752ba563aad7dbce8cd5fc87c14bf7c11a1bfe3bd8763849396f7699d -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.GetInt(System.UInt32,System.Int32) e43f30abe19b9307c3728256d3f82a9281b68ed5ac73309a414ece584be761b5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.SetInt(System.UInt32,System.Int32) 355d48163d79c3c2381f38cf223bc7bec581d2a551d98d412ab746a08f416a39 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.GetBool(System.UInt32,System.Byte) 1d41604563e0b7ce5d5c379dd72c63d481af045a82a17fb237eab8a4580210bf -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.SetBool(System.UInt32,System.Byte) 5a860daa88e2bfa829bdc6bdd990ffcf034b92f00d838e2df3e3c59374cf6a21 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.GetFloat(System.UInt32,System.Single) 1b94b7634e5f1fe8cc2e275e02b3eeba720b0ad9dc9fccdb8ec5715d3008bb18 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.SetFloat(System.UInt32,System.Single) 5012afe6498e0b92b5433258cdd60b6d361c1c07712d83971647f0059429f734 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.GetVoidPtr(System.UInt32) b60972eff7351dec788586e4f6fcdf9281af0b3136a88149ec59b3945f8c18ae -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.SetVoidPtr(System.UInt32,System.Void*) f5daf3e8a5d58de467f693ee4b52d9d1b7203d02bd8e59b1ca2ca3b85cda665e -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.GetIntRef(System.UInt32,System.Int32) b781028326e1f58a14a84dba944ab09b6afd5794793a3b72d49146b41e28afa1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.GetBoolRef(System.UInt32,System.Byte) 57d4f2325ec2c1abdb05a6abc4cfc09391104446131904a7820084bbe3028c69 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.GetFloatRef(System.UInt32,System.Single) c3af828dcdeaf003ecf48e239904c7422e7d5c4941a81a753d2cc25607d040ba -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.GetVoidPtrRef(System.UInt32,System.Void*) e6ac2d459f905031781f3a52d4b0a9f02b97ad5185fa4d3f0e6db2f76ebbfec9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.BuildSortByKey 31cd8159e1e94f82e5f6654117548d12009e839bac2be8eea93999af4bea388c -->
<!-- api: M:Inno.Native.ImGui.ImGuiStorage.SetAllInt(System.Int32) cfde01fd114916337caa7abd44c4676a06b11a8421911c0740061ce438121e86 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-76a26838d1f3aece"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiStoragePair> Inno.Native.ImGui.ImGuiStorage.Data`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2128) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b343dfb16e37346f"></a>[`public Inno.Native.ImGui.ImGuiStorage.ImGuiStorage(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiStoragePair> data = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiStoragePair>))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2129) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f7a95963aef3342f"></a>[`public void Inno.Native.ImGui.ImGuiStorage.BuildSortByKey()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63989) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dddb416eb6eb46d3"></a>[`public void Inno.Native.ImGui.ImGuiStorage.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63846) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c6d8cdac00e0be96"></a>[`public byte Inno.Native.ImGui.ImGuiStorage.GetBool(uint key, byte defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63879) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-793203fe435d690c"></a>[`public byte* Inno.Native.ImGui.ImGuiStorage.GetBoolRef(uint key, byte defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63956) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-706e4f12312f8ed6"></a>[`public float Inno.Native.ImGui.ImGuiStorage.GetFloat(uint key, float defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63901) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-95dcca68ddb9a49e"></a>[`public float* Inno.Native.ImGui.ImGuiStorage.GetFloatRef(uint key, float defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63967) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-925577f6b49f54fa"></a>[`public int Inno.Native.ImGui.ImGuiStorage.GetInt(uint key, int defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63857) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c78010e610a0819e"></a>[`public int* Inno.Native.ImGui.ImGuiStorage.GetIntRef(uint key, int defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63945) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-91976fca44532755"></a>[`public void* Inno.Native.ImGui.ImGuiStorage.GetVoidPtr(uint key)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63923) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eb215b1ecabdd2f1"></a>[`public void** Inno.Native.ImGui.ImGuiStorage.GetVoidPtrRef(uint key, void* defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63978) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fc6c3ef1ca6a1671"></a>[`public void Inno.Native.ImGui.ImGuiStorage.SetAllInt(int val)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L64000) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-101f82b24c549109"></a>[`public void Inno.Native.ImGui.ImGuiStorage.SetBool(uint key, byte val)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63890) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0a9e748bf701bb4c"></a>[`public void Inno.Native.ImGui.ImGuiStorage.SetFloat(uint key, float val)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63912) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7c57df2d471b67ab"></a>[`public void Inno.Native.ImGui.ImGuiStorage.SetInt(uint key, int val)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63868) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a8e470bb8987a766"></a>[`public void Inno.Native.ImGui.ImGuiStorage.SetVoidPtr(uint key, void* val)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63934) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7bd641ac3594b791"></a>[`public struct Inno.Native.ImGui.ImGuiStorage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2126) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStoragePair</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStoragePair fee8188d28850529dc309a9eea15e3d240c05edb64d60a9464622a1a03cc8d20 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStoragePair.Key b03a481fc71904ed9dfae24c5aaa327ad699ee1824eb4fb3443358c6329371ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiStoragePair.AnonymousField1 100c09deef099d55f17373c3b3da55d119b6a6b14fae0f4bc2c5507d89e336b8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePair.#ctor(System.UInt32,Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous) 96046b4b620222650ed480f6a95d4948da76a3f0c14dea34c2d1a61a5020ef29 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePair.Destroy 6b1d4dc74ca8a75409296e84b56e64a67a3920b94b579162176045e12a3083d2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-08bd42b5a3895f8f"></a>[`public Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous Inno.Native.ImGui.ImGuiStoragePair.AnonymousField1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2114) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0542418d188e0832"></a>[`public uint Inno.Native.ImGui.ImGuiStoragePair.Key`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2113) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccaf76cee9c6d3f1"></a>[`public Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePair(uint key = 0, Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous value1 = default(Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2115) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f8a80d50d56f7ca6"></a>[`public void Inno.Native.ImGui.ImGuiStoragePair.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63832) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f1670444b495f6ae"></a>[`public struct Inno.Native.ImGui.ImGuiStoragePair`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2097) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous 8ee81e646fabc3cc8b797d8bc8b382af58dfd0ed7e9ffd434a2206f8e53af9eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous.ValI dc5f8bcb8c494f38859f31eaa65d14c6bde25f1ec3dd8671db4629cef0812c54 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous.ValF f449663f43b542fc3c823e72a8204f053ba5237642999f60c9ebc8d1565c08d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous.ValP 37f7549602dbd39575a8202e9fce89589d26d5cb0e0c2646371da85b507f9e6c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-28cc212ac5301ce7"></a>[`public float Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous.ValF`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2108) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c322de8941850e8f"></a>[`public int Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous.ValI`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2106) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c3562f46a5f77fc8"></a>[`public void* Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous.ValP`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2110) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cecb54f446f141e5"></a>[`public struct Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2103) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStoragePairPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStoragePairPtr 2246108b715e131535371d2ee7e7c52073160f8929c157749f339cd17d98f93f -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePairPtr.#ctor(Inno.Native.ImGui.ImGuiStoragePair*) 650876ce6ecf2963de1e98242c7d446ba87eead0445ed1ab7504e6ba733a7792 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStoragePairPtr.Handle 5718257d38f6ea5728eb55341bc5bc4fa460f70255cee2dfec77c9292e0360de -->
<!-- api: P:Inno.Native.ImGui.ImGuiStoragePairPtr.IsNull c512211a7aa284b20feca8ac2996b60db597b757e3e6ecbbc0470b5a8bcfaff2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStoragePairPtr.Null 0b06d09d82702a44f997eacb200ce99a68920cfc28da4df3c6dfdb59dccafab3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStoragePairPtr.Item(System.Int32) 5eeca0451bb309772a7bf47b4448d1e66ba3838060fb46b7f9d76836fb5659ec -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePairPtr.op_Implicit(Inno.Native.ImGui.ImGuiStoragePair*)~Inno.Native.ImGui.ImGuiStoragePairPtr fff07e1bc15ae43f7e4a608f2ab12e0cad370be30206b40c6799ebd4a8d8b97d -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePairPtr.op_Implicit(Inno.Native.ImGui.ImGuiStoragePairPtr)~Inno.Native.ImGui.ImGuiStoragePair* 5eb7cc46f0d249fc5ae5aa13d2210d823ac4954e84a57a5dca99b8bee1e0d692 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePairPtr.op_Equality(Inno.Native.ImGui.ImGuiStoragePairPtr,Inno.Native.ImGui.ImGuiStoragePairPtr) 62abc5300ca07bd00e2ef577a0670b7c22b54ab3647832be7324ea066d95fb93 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePairPtr.op_Inequality(Inno.Native.ImGui.ImGuiStoragePairPtr,Inno.Native.ImGui.ImGuiStoragePairPtr) 22b9ad984c6ba7786d15f5490895aa05d9ed409d58dcf546c56545432ff749ff -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePairPtr.Equals(Inno.Native.ImGui.ImGuiStoragePairPtr) 4c143c34cd1d43eef42dec541e5d1dad24f0e9a003e8dac7c17d1f450d03ecab -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePairPtr.Equals(System.Object) ce0f202df85dfa0d30a74990704bc9e3b11e6c7d6837af66e73781c78e269e3a -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePairPtr.GetHashCode 4010492282bf0c99113d68ac4c5ea8392ca1239f19aea2f45ea26d553c15ecf4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStoragePairPtr.Key dae1deec3a8471cfbefab4646a5424d3f65bae6c7fd5e2c569086dca726dbaa9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStoragePairPtr.AnonymousField1 a1460d6896ba1710963d438cc11ce668a5f717f73fd3e1f297095d6d0eea3045 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePairPtr.Destroy 06155da93c8377e684b9fec31af2b991a3ba14d6c0f4cd02d1648bd6dbe7e064 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9bd81b5f042bbaaf"></a>[`public Inno.Native.ImGui.ImGuiStoragePair* Inno.Native.ImGui.ImGuiStoragePairPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16206) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e0667cf486473ad7"></a>[`public Inno.Native.ImGui.ImGuiStoragePairPtr.ImGuiStoragePairPtr(Inno.Native.ImGui.ImGuiStoragePair* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16201) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-695c6882b501bed9"></a>[`public void Inno.Native.ImGui.ImGuiStoragePairPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16226) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2132f84f0f6b6979"></a>[`public bool Inno.Native.ImGui.ImGuiStoragePairPtr.Equals(Inno.Native.ImGui.ImGuiStoragePairPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16216) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-e4e3a8d537868652"></a>[`public override bool Inno.Native.ImGui.ImGuiStoragePairPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16217) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-b179aeb00b74aa9a"></a>[`public override int Inno.Native.ImGui.ImGuiStoragePairPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16218) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-bea4ea052f8ea81f"></a>[`public static bool Inno.Native.ImGui.ImGuiStoragePairPtr.operator ==(Inno.Native.ImGui.ImGuiStoragePairPtr left, Inno.Native.ImGui.ImGuiStoragePairPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16214) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-83a44fdea3ab2fc7"></a>[`public static Inno.Native.ImGui.ImGuiStoragePairPtr.implicit operator Inno.Native.ImGui.ImGuiStoragePairPtr(Inno.Native.ImGui.ImGuiStoragePair* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16212) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4faa6c323e980a0d"></a>[`public static Inno.Native.ImGui.ImGuiStoragePairPtr.implicit operator Inno.Native.ImGui.ImGuiStoragePair*(Inno.Native.ImGui.ImGuiStoragePairPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16213) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1797f5f82af01c6a"></a>[`public static bool Inno.Native.ImGui.ImGuiStoragePairPtr.operator !=(Inno.Native.ImGui.ImGuiStoragePairPtr left, Inno.Native.ImGui.ImGuiStoragePairPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16215) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0d86f623b29ff0e8"></a>[`public ref Inno.Native.ImGui.ImGuiStoragePair.ImGuiStoragePairAnonymous Inno.Native.ImGui.ImGuiStoragePairPtr.AnonymousField1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16221) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3cb0ff68596b8e7f"></a>[`public bool Inno.Native.ImGui.ImGuiStoragePairPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16207) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b11596be2a9a2850"></a>[`public Inno.Native.ImGui.ImGuiStoragePair Inno.Native.ImGui.ImGuiStoragePairPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16210) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d89108e06dd0d668"></a>[`public ref uint Inno.Native.ImGui.ImGuiStoragePairPtr.Key { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16220) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-131a8df3f5bacb7d"></a>[`public static Inno.Native.ImGui.ImGuiStoragePairPtr Inno.Native.ImGui.ImGuiStoragePairPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16208) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3d54b37b28bfeaa0"></a>[`public struct Inno.Native.ImGui.ImGuiStoragePairPtr : System.IEquatable<Inno.Native.ImGui.ImGuiStoragePairPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16199) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStoragePtr</code> · 36 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStoragePtr ed010b0d6f7aef88f1a5a1c65c05c61f2ee39aa260ece1ad117628b04287e0ab -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.#ctor(Inno.Native.ImGui.ImGuiStorage*) 11bdb156288a9a5d8e0d8887c7136ca2144758c7169199755199f0a90d94c302 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStoragePtr.Handle 4e519020258133db47577e5b20d8302d0a7fa925abe5b86726ea00829dd0314c -->
<!-- api: P:Inno.Native.ImGui.ImGuiStoragePtr.IsNull 217afd49dd625de3520b578ca3edced3d2c680b631e15b8ab6f2461b54b285dc -->
<!-- api: P:Inno.Native.ImGui.ImGuiStoragePtr.Null 970b058bd8d5f7d0db700d4266a3ccc1d7a4b99746ce410ea3d2153dd8eca3cc -->
<!-- api: P:Inno.Native.ImGui.ImGuiStoragePtr.Item(System.Int32) 7780499761a87956129a0b11b6e58be33a65400e504b82f24d93c65dbea79ea5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.op_Implicit(Inno.Native.ImGui.ImGuiStorage*)~Inno.Native.ImGui.ImGuiStoragePtr 9020954e1c6397f144684b2be82f0721e1fcc5a72ffd357b574d70b193a3d21e -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.op_Implicit(Inno.Native.ImGui.ImGuiStoragePtr)~Inno.Native.ImGui.ImGuiStorage* eac80e725ab18f7814596944c4d5e496a14cc940d48e75e9a006a350d83522d0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.op_Equality(Inno.Native.ImGui.ImGuiStoragePtr,Inno.Native.ImGui.ImGuiStoragePtr) bd322ac24ff6ad7a910bf298721858d50d2048679e748c3641d3e00e20a5f2b1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.op_Inequality(Inno.Native.ImGui.ImGuiStoragePtr,Inno.Native.ImGui.ImGuiStoragePtr) 300aac8a3b6ae10e5bdd5e8e8c05e3182c75e1bd54952e70b6a2c6a661e27a00 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.Equals(Inno.Native.ImGui.ImGuiStoragePtr) bb0c18ec1fd972b894e74cf6d856664bc4ab1b6ea234a5442f4c96cfccc2e971 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.Equals(System.Object) 24305bacf06b53587e7624060a0e8ffe56b6f61984cf1d55e0e835ec6548baa6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetHashCode 61d548960286e44c74a4ed466eedc928a9d5d4ca4fc1f16df88596749c236ad7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStoragePtr.Data 35f73bd4a1815b29aaaeaf23aeef2942b673a474ca09e07fab6eb14b3aae0c0e -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.Clear 2a423f005250bf5e6e0897634074ad71945ad7e0b6ac8f1eab266072e19bea94 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetInt(System.UInt32,System.Int32) 8ef93170cec88189bae74a6c0d76d67853ae41879b6251467c53824d64f47b08 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetInt(System.UInt32) 55f73cbe4be388bc0c928e0f7747157dba36731a6a86b8a7dfaddb8c9215c64c -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.SetInt(System.UInt32,System.Int32) d1bd15188e6867d805c8e4a23e02357a9d6e00fa4db0bb37e2acd3b8789a82dc -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetBool(System.UInt32,System.Boolean) b34757161d462966ab95fde9cffea31ffd179584c942ee61d9beef90587189bd -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetBool(System.UInt32) 761f7042378a0933e561ee6378a496b1ad8bd90094e91ec98d3002e43ebe3cf4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.SetBool(System.UInt32,System.Boolean) 49aab24b126251e325ed2f1b8acdc9f00e33e96f571b10556155b73e403ee45b -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetFloat(System.UInt32,System.Single) 06028612305608ab56869612b08a4f7e2a2a983daaa349a1b953156046432f80 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetFloat(System.UInt32) a86dc79b2bf06a76788200c33672927667fefc4bbcec3ab6779b09d3d4527ccc -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.SetFloat(System.UInt32,System.Single) 6fc04304f0609c5d29f0aa5ba280a0d7662e1ba3fba8633aea74235288a1dcec -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetVoidPtr(System.UInt32) 903ba2b167854a10866c58fc2f4d399854b4f6af7ea7e57f62c8b4ffa79a4773 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.SetVoidPtr(System.UInt32,System.Void*) 9d3cf0337e37dc5b3e131ab8e6e6f0e48e96c3c4bb740048ece0c7ca27478e6a -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetIntRef(System.UInt32,System.Int32) 306686573d6d4b92b45f0d5c203216763842f76198e302273ba5add904a249b1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetIntRef(System.UInt32) 9f02ccb4002d3c170a692a6fdcade1f1b1c4af8d3c3fff63f6b453ba9647adb1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetBoolRef(System.UInt32,System.Boolean) dfb8e937003daa22d2ec88f4e9aa9fc2f93c7f15c85fbdedd93643edff57cea0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetBoolRef(System.UInt32) 1587768169cbcfac90aa6ac893f1ae410979d24eea182f5791a4c272e826549e -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetFloatRef(System.UInt32,System.Single) 7a2b0cabb1c927f003eca385f21c4c88c67d26b9b36db1ba16a60cf6e600d795 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetFloatRef(System.UInt32) 322f896c5c86df0a986afa08d4286c2ac5924303d0f3fbfc6ff4068dd6a1ce77 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetVoidPtrRef(System.UInt32,System.Void*) 744efedaf5468bbdf249c88717c6c82344b1b4743e8bc15bb13376445524150c -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.GetVoidPtrRef(System.UInt32) 13d027972abdfc4a597d60ef5224d18daeae33b8b552762d6eaaa4ff1f7dbcd9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.BuildSortByKey 2a244c813c53ac52d9d145ab8527ff646ec1dd51a2d7c0cb35d0dcd70af06b81 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStoragePtr.SetAllInt(System.Int32) 4e385a7b0ed43c8ad44a534edc980f756b06929cdade932f71f0e76bb1e3e119 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d598d54e4a270d5b"></a>[`public Inno.Native.ImGui.ImGuiStorage* Inno.Native.ImGui.ImGuiStoragePtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16240) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea803db1a93c6fde"></a>[`public Inno.Native.ImGui.ImGuiStoragePtr.ImGuiStoragePtr(Inno.Native.ImGui.ImGuiStorage* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16235) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6c2d6161b6538fda"></a>[`public void Inno.Native.ImGui.ImGuiStoragePtr.BuildSortByKey()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16419) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c933a17f0245824c"></a>[`public void Inno.Native.ImGui.ImGuiStoragePtr.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16259) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7f67748e935bceaf"></a>[`public bool Inno.Native.ImGui.ImGuiStoragePtr.Equals(Inno.Native.ImGui.ImGuiStoragePtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16250) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-cc8f79a9576c80ce"></a>[`public override bool Inno.Native.ImGui.ImGuiStoragePtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16251) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-5cc7159a3fdb1c0b"></a>[`public bool Inno.Native.ImGui.ImGuiStoragePtr.GetBool(uint key, bool defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16291) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-27e515425b4f3f3c"></a>[`public bool Inno.Native.ImGui.ImGuiStoragePtr.GetBool(uint key)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16299) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3486be1702fc8f41"></a>[`public byte* Inno.Native.ImGui.ImGuiStoragePtr.GetBoolRef(uint key, bool defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16371) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7bfc2e6a1aeea262"></a>[`public byte* Inno.Native.ImGui.ImGuiStoragePtr.GetBoolRef(uint key)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16379) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7f52f3e8aaabfd3f"></a>[`public float Inno.Native.ImGui.ImGuiStoragePtr.GetFloat(uint key, float defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16315) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3d01747e38bd662a"></a>[`public float Inno.Native.ImGui.ImGuiStoragePtr.GetFloat(uint key)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16323) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2c76ed40cd0771be"></a>[`public float* Inno.Native.ImGui.ImGuiStoragePtr.GetFloatRef(uint key, float defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16387) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2032947c58d5582f"></a>[`public float* Inno.Native.ImGui.ImGuiStoragePtr.GetFloatRef(uint key)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16395) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9196eac5854cc97b"></a>[`public override int Inno.Native.ImGui.ImGuiStoragePtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16252) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-1a07da4c59a0aab2"></a>[`public int Inno.Native.ImGui.ImGuiStoragePtr.GetInt(uint key, int defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16267) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-09b9115706421bf0"></a>[`public int Inno.Native.ImGui.ImGuiStoragePtr.GetInt(uint key)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16275) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d6a750c6e9caafd4"></a>[`public int* Inno.Native.ImGui.ImGuiStoragePtr.GetIntRef(uint key, int defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16355) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5bb910935a1d54cf"></a>[`public int* Inno.Native.ImGui.ImGuiStoragePtr.GetIntRef(uint key)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16363) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-584d249d4674ae90"></a>[`public void* Inno.Native.ImGui.ImGuiStoragePtr.GetVoidPtr(uint key)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16339) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-044a2bd4007f20b8"></a>[`public void** Inno.Native.ImGui.ImGuiStoragePtr.GetVoidPtrRef(uint key, void* defaultVal)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16403) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0fe4c2cd0f3d4447"></a>[`public void** Inno.Native.ImGui.ImGuiStoragePtr.GetVoidPtrRef(uint key)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16411) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3514257824b80e69"></a>[`public static bool Inno.Native.ImGui.ImGuiStoragePtr.operator ==(Inno.Native.ImGui.ImGuiStoragePtr left, Inno.Native.ImGui.ImGuiStoragePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16248) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-58ca4ec742987caa"></a>[`public static Inno.Native.ImGui.ImGuiStoragePtr.implicit operator Inno.Native.ImGui.ImGuiStoragePtr(Inno.Native.ImGui.ImGuiStorage* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16246) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7d092e120a72d854"></a>[`public static Inno.Native.ImGui.ImGuiStoragePtr.implicit operator Inno.Native.ImGui.ImGuiStorage*(Inno.Native.ImGui.ImGuiStoragePtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16247) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-db53d7cb6485e8e1"></a>[`public static bool Inno.Native.ImGui.ImGuiStoragePtr.operator !=(Inno.Native.ImGui.ImGuiStoragePtr left, Inno.Native.ImGui.ImGuiStoragePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16249) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3e89960fd80768c1"></a>[`public void Inno.Native.ImGui.ImGuiStoragePtr.SetAllInt(int val)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16427) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d8744582f877f8e0"></a>[`public void Inno.Native.ImGui.ImGuiStoragePtr.SetBool(uint key, bool val)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16307) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0b151e837bf34503"></a>[`public void Inno.Native.ImGui.ImGuiStoragePtr.SetFloat(uint key, float val)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16331) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ff17b54a97278b2c"></a>[`public void Inno.Native.ImGui.ImGuiStoragePtr.SetInt(uint key, int val)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16283) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-54b2ac7a8408e882"></a>[`public void Inno.Native.ImGui.ImGuiStoragePtr.SetVoidPtr(uint key, void* val)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16347) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f9c274e48304a285"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiStoragePair> Inno.Native.ImGui.ImGuiStoragePtr.Data { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16254) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-47f46d1e11357a97"></a>[`public bool Inno.Native.ImGui.ImGuiStoragePtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16241) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ad9fde427e759909"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImGuiStoragePtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16244) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6d54cb16e287a930"></a>[`public static Inno.Native.ImGui.ImGuiStoragePtr Inno.Native.ImGui.ImGuiStoragePtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16242) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e18aeaaafcaafd80"></a>[`public struct Inno.Native.ImGui.ImGuiStoragePtr : System.IEquatable<Inno.Native.ImGui.ImGuiStoragePtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16233) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStyle</code> · 140 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStyle 29c5c831326a11c401ed02792a533f2269dceee2cb944e13b713ec8da81b340c -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.FontSizeBase e3bba90f5e86991bf82756964499255c5ae8ac21bcc3cea9c16a1f7edd5780f7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.FontScaleMain 7b6fc1425ff1bf01ae41183fd6150d5d05ddf12a5cb38d13ba1da89959bbdf80 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.FontScaleDpi 1cb4ac465c5555707f76ccbcb015ba6db5d1f83f97792de13072f0280d08f4be -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Alpha df3fa12df592598ae1c50d4f83e4ad23497f04f857864f620ba945cb8fcb5d61 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DisabledAlpha 1bebc4cb818d2253779aa448d91bc16f543080bbf9c2e7a702769873f08074eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowPadding 04877e85dcbd17eb51cac39b358fe6010c83b851fe7a1d7ea129348d8baf62f5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowRounding 2e59ddb8ddf997446391d808d72ed3fcc1ac7b7feec59538fec07e7536fd2e0b -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowBorderSize e4a0b750ec759e2027e0e84350ea5bcf29ea3e73a7f30a49fd2b2ca512eeb725 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowBorderHoverPadding 0b3fca86ff9812bba76f99bdb064420af7b67889b6e71884d0d69adbff2dd1c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowMinSize 7cb5d10239f930f1777c04ccc5829a22384cce021b2432a38f93d9505298b987 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowTitleAlign 3a726af934030f5d4ac855a33c0fd5d7ebf43d12c1e1f8eb50de2e9028f49d93 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.WindowMenuButtonPosition fac317691348544c8d6abfa6e0adaae81588545145b6522aa1f41a99a63ef43e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ChildRounding 48c123bb04490ce0239ff5323627555bd63c2407cca7bece5b3617ea8abd709c -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ChildBorderSize 0b195ffdffcb0f1461bf39641c6b5770833241dfd681c77fc98102b5c30e940d -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.PopupRounding f79f789fc3781c57d8ab6c1015f4506a9cd9d5d5efb45c591f7a1fd1a4e9360a -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.PopupBorderSize 0b23f022257662466c1cae6e726e8cab213b5e46e50e7616b33b945e759ac083 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.FramePadding 444b6da9f3a1818914a9128a60ddeca0e4e49fad659b31405958207f29b8af2e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.FrameRounding 9969c634946329355ba47bf59a3d47f46d668662e00945917c4fb58af4c06fbd -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.FrameBorderSize 3a76433a72a3b3efeb2d1cd11014d9e6fa995321aa83c38cb27c9db9b749acfc -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ItemSpacing 1d2993af5a08e9823503c63a6c58e38ccc2d62245677fe6ae5e05d85fcf46456 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ItemInnerSpacing a4dba9566f4080ff6ddb308de51c3d51a630fa74a502d5ae9833e4684df1c838 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.CellPadding 0708f2c4f9f3f9bf86defe9a97a034a48d0c80260953813fc510aaed64aab10b -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TouchExtraPadding 46bfef75f730dec02bebdda3a6df5e4c267c3689b40ba76b9ce4c833cf4e1b95 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.IndentSpacing 69f1aac1ec365816db88622feb14c12444c2df3d0554ea0de9077305b1bf3960 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ColumnsMinSpacing 666b5db6cc7eb2ea79763392783f8a15723f45b457d8a8c8263e3de83cefc5f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ScrollbarSize 7b1b91446c51ef08a48ed73800e1c35d47d1f5e21f06693d852af92633b35e6f -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ScrollbarRounding beb2265cfc77efdab799fa0cdeb7860825222703818727770f97a6823c670cae -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ScrollbarPadding 49fca0161a6e9867d91b916de21549f7f0bc3d3847eb35a49092a891995fa1f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.GrabMinSize f23859f15d8e73559d8cf25abd991e5969444dc691a2056dcf21d73f8b5cae20 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.GrabRounding 7adcbb67f47da8cf436c8fec939b1cf0f8d436ec4b10db6c03b8409a248951dd -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.LogSliderDeadzone f611431807c74660ba5f163f240befd88a7f8b9b31d62fe480d04991fe094884 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ImageRounding f8fa0c424534e6268b6c366e0acfba437b1d432a7bd11ee446b7e610ad689a07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ImageBorderSize 494ba1fdf338e2a5d65821edb232809e3d24646247bbe12286d0985574202c18 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabRounding b9e74be6b7118e591e3779872738faaeb8850f797b207f5e247ecf8bc1506577 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabBorderSize 43fe43cc7ed74b013ca4b8c355df46a9fe1adde96dd6dbb962f1756cc22f2743 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabMinWidthBase d3c69f8b760ef1fac741d0d531701ad34cd4628df6c8bbc3d16333e9b7453e52 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabMinWidthShrink cb43cf34a54371764dd4a668ed78c7069cd5e1040004b00a4d40a45c62d058ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabCloseButtonMinWidthSelected b69e5aae8795cce729443d84592a4379b2bd03140d8e371456952aa470913fe6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabCloseButtonMinWidthUnselected f1e486ee85268b9f780e91998b019daaec08c14bd00820cbe7f738baf52f6ece -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabBarBorderSize ac9cc4fde4ccd7e2a5da3b2850292689fbce8fc616e05c5d3de508f104e62bad -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TabBarOverlineSize f345b6b2ecbac86950c4443591280151961f5630b60fb388e5dbadd04cc5dcd4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TableAngledHeadersAngle 603452fbaced36e5d772ba39f0b5599ba98a601d74d9fe10f775bc9bf2a1fc43 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TableAngledHeadersTextAlign 891c5d178f8f2de21980b748817a7d5d5ce1ae2602b176cc20c254e91d462a3a -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TreeLinesFlags f003e36a9dbb6b2c8cd8260d21d6ac0464769bc20480684d7cdec1de28b3a1a7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TreeLinesSize e07bfa960b9ed536451a10e75269ac606c92be58b23f521dea99a7f39496efaf -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.TreeLinesRounding 66436947becac494e90b0b19562d2f50c27f90af919293e5f743e0e7c1876635 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DragDropTargetRounding b97e28ececa9749df5f4ac0b55e075f603d6e59373040363f498bf54c9ff009f -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DragDropTargetBorderSize ae7b78b4cbf4c57d495662741f5184a723ac8d335d2b681942b7e09d56c8abf6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DragDropTargetPadding ac7a4215df20e760a1ad682924a364dcee96cb078876f735a68410f5919e6ddd -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ColorMarkerSize d93b6e5f151664970cd71b1f15a379121a8e591b56d28e0a4d3617f50f4c67e6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ColorButtonPosition 96e8c25b33493c87c4466fbc6275e99b537fefd858cab257b34e1dc61268ccc6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.ButtonTextAlign 715da29637c1f2581302c7dec660dafd782d6ff30539abb8b82cca66efac233b -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.SelectableTextAlign 826f373e6a9489a74e09520d95fe85b9b3c09da1ea3cc1f0a998ea709970bae5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.SeparatorTextBorderSize d13366e8c909c593c14b51f23bac6ae1478bb3a8b8f1f3b00cbb29a1d3b2ec45 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.SeparatorTextAlign db198078c22902f8a46d2e6987332fc6debf7ac9d74d63451db0d61db08e9aeb -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.SeparatorTextPadding a39607ca842dd83df18fbc600a7f4d0e465e1e80440ad614e6d16f8af87e834e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DisplayWindowPadding f8a5f651159a6d87bfa3965687178c8004082dcb2db2c41da0c052d816ba1a10 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DisplaySafeAreaPadding c4b76f639641d157ab9232c201907b2454e2d498363559dca337996796b1437b -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DockingNodeHasCloseButton 0b6f9f0cd29ffb162deb311328735b33fa010b7b521610ed39715637069a4cb8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.DockingSeparatorSize 012378928ae55ef7d63a752845af06558a3a6b67a59e8b88ee103d32db6eb940 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.MouseCursorScale cdfabe442cd9cc81ce676151e990563bbd0ddd86a6fe3ae728fc8e7418f508fb -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.AntiAliasedLines 0dea99e0493c894c93949671318da19ac8252839f7c5a5ff9af8d24ed59a62e0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.AntiAliasedLinesUseTex ee4752c7acd29ab47f291fe661ad17e6b6ace216a9e32764d263702fd35a360f -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.AntiAliasedFill d30d590226a71fb567b706f7dce948fa1b8339900ec3ffa332cc1fa2673feedc -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.CurveTessellationTol 154cf7977064be6012a0bdb92f60cf76fd7354cd79beeaa2bf2bcfebb3208c13 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.CircleTessellationMaxError e833d4d1eb31582514ad8d52f5b77c19bf8986d5fd8ba7ca03112e4af8b7ce25 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_0 7de8f5869f18f2b693509f5568ede2c1b7cd8cad0cde9947e5bd041167890aa4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_1 7bde10906d42549df550017ba538db7d5f2d8ca98cdb7a81adbc2e71fb3232b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_2 ab4505b2209361643c688b39d8b8fc99b8489ebfeeaf0daaeadbbaadc34a92a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_3 891a9041281d522fb709782f2e677e88d68ce31108e415acceb9069f660b72e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_4 86a6c01e7c1fc0f0a5c1ef501d4f51e3e90b7292072ea9f93a9f87d299935a27 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_5 46cdf4cabcf8a3aa3bba368db5ce965892a3693271cf32e256ffe25627d63ab0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_6 52d273c21e22d31183768b7c170b2ef4b7026039491a6e065b1053ebe4cf6eef -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_7 a376eb591d69e173d934826082575e2efe54bae17feddb33985a656e1524e77d -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_8 14f611981f33995df982b4439f61c76ffac6fabed8967d9f65e081987f1d0659 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_9 50a2b7e5db083887ffa555dd9c1c224a7adb772377b39762eee4a971c2db9d60 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_10 ca69511484d2b19d11bce1e66d8ccaf97e3d6e7409ab7e38e5d338d736d70b21 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_11 c1c392cf8e15ab57b4502e404b8e2a0067fed65f75cc93d659ba347c4af97271 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_12 4c597f5a7006b401bf282febce4d73ec05606a68ee17783aafecb67aeeaea6c9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_13 1379c9b6baa1532aead8462e4f778c8afc2c162e3fb5cac1cd3d9f35ce094dd8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_14 e69b5b4cd85069dfd609919d908e72bcd74a6bf7387223ff39a9033020b14623 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_15 376c7babe04a8e6ba67b6be8761b746b810b46eb077b50e39c387376f8ec0f9e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_16 6e14c52b63e7f969b8ef959f518819db020ed387862925685f0ef4405c8b5a9a -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_17 5f05ff6589ac56833f7a9cd38360324e9ab9766dd41e78eb5262205dc71321fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_18 d6ed86a25ca55b67e60b7a50c3947f69149581478799ef2dd2cfb4fd77082546 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_19 eaa3d4d8071a6c8de9e253a589d2b366537bfded819e93061df40484df2d93d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_20 bc04d594c6d320499ef7428c40f3683e51fa729e5a4fcad4c57d608c9494f6e2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_21 33b8a149e2cedaa00491dc4b4d8566f16fd269e8f5d5979753ea5ed5566b9f29 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_22 7e545e8928de17ed8c07c2af1cdffb50012c6832066571cd22f77ae37b3bcacf -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_23 a5fe5c3bd91dcd07fe51ba3c8bee94ef502dde8ab504afad15e803e57e8eff84 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_24 2efe6a690502d80601b894064f185618a643f1464e985a4786204b0adc23102c -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_25 695c090edec654d22c110e92fb5717745363424d124dea73d9de7e1defbc9ee0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_26 b17af1f4176c20bcf36130b55a6a79bdc9600a765c32a4e5fdae3060afbede6a -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_27 590462fc968c793e7b430194a59dc2ba8ec4de9c6ad9e40fb0686bc89b79c228 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_28 195aaefb5eae749bdf7d9e1407699dd20a65153ad164db4c1616b964531557ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_29 de4166f43e55dc62d66b917d84903333c285589b1d5da6b0987e355c2c9ff538 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_30 8773ca841525c77c5bf88416c711fc29bd3c0496a551d2bede3f4bcaab7fb6aa -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_31 bdcd23cfb7db51648f019e958756a867fdfe55c95fe9115181704c66366a6ae5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_32 9b5358da4e837c75c5f334b64d46bde41fbadfba29baddabf7fd0103ba4f7cb4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_33 6d1a5d0cc7d81aa646b222a82b4cb2372e58622d8c8180fcb04b37fc48134a8e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_34 faa2e93e06ad409a46b3c24d84f309e0effd725f655d849a289028ea3d8d22cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_35 3047d74222692e7d781f98f338c58e41763aac6817a17bf6724b162c8c3a2dca -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_36 6242808b3d1112ab543f792f3b75e1a8ff386169841ab27c19273c3765ab41db -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_37 0dbbb3bd0d7e9346cb13d90fe3570f146d375794fb36f3370f9c481246cf16de -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_38 36a8e5c0284b6857b6cabc0a61e285c05bb79a5fba17dec0cf7cb8afd9ea465b -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_39 6412837783a57cdc7addb1735aa576e11cd1767d41cf0abffa0e027a5c2f709e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_40 d7f45e9ea7c165083dbc70dc01556ca7b72e0c04d8e29a42339e9c81edc55b6e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_41 dfea1361f951e1540abd27e3ab49e37f2dc9ae160320a14adc717529f3379c06 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_42 ca3478d80414021f37dd6f8f8f4714005d8fdd1d7873b85f85841db1cf77450e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_43 5cdc45536b2da2b2bbd9040c6bc607abc662f74047b76ecbd0cfca7f67589ced -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_44 f114fbe5d980e43cc83a42dbd865679c2dcc0bbeb3b6b8429ba527d8810f2a86 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_45 8503665e50acb86f8f0d3ec549d1b3a7b828bafad5d4aecc523df1fb14ddc2b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_46 3c5ff16123cbaced3219c7b8ad2b728a7dd7ff944d9a8bc5b0a14eb3964b582f -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_47 08c078e042fcb301fd0cee313546528958f1a568a839ff2085fe905b3251e770 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_48 caf3c169c59b18724f8f462a9a79a761a557ecafb13d638fd108ddd9551a1827 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_49 1afd1a47c6885876c0563a975518505ba0bb221dde402f38ca3a6857ad6ceea1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_50 13fa0e38d82d95681e2a95438088313748de50686f22aed5229f213e7cd9f50e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_51 17c5688a8f911c5a09f4245d0dfcdfc8b20845a4d599dae29961849366734aa7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_52 36b4de68409858191ec874d7b383185a8680895e77b9d4c1621a56d70caae6e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_53 8297ffa510c2339ba286b33262739b790771facf352c453c7e033f073e708f1c -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_54 8146337c9790cda630ff912231b393530ac9b347f896efee99f272f236b6b932 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_55 40f1295e5b966972572633169f8b8900bc8887963696575e898a2fcde2bfe549 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_56 74dd99cecb4c5ff469088069a23da974e5898ba540c065156b002314872b0b84 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_57 3b0caba581c910e8aacdb9dcbd5e75dd73f006b74a5be8f52d289342a050d0e0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_58 113c349ca5f86d60abfca75c83b3488570572466247be7c6b5a97f443807ec93 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_59 1ad45b8c34257af14164e24f3db30b7228c3d4e6eeef2483ef7b604f6d877a2d -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_60 649385033e5d007b39bea03c31ddaa0c51d085858ab93dd14a67e745679482b6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.Colors_61 55324f5690b96a1006be497ae7aa1c86538e416a23fa481190d5573b7257fef4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyle.Colors 05637cd71d170eb23ca61c0eca8775a320fe87c514621e478cd2d615f21913b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.HoverStationaryDelay abafbfc93f083e8f672efda0bf2977882b9d0ad1752f56a2a2f1d3b426749b63 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.HoverDelayShort a7b0ee335ded9c3837bdad65f37db48007c5b5dda3c4a29ca01927b6bd777488 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.HoverDelayNormal 8a880111ff95e288f369825cd378f670396d162fa476778530a4dc8dc9fb5e16 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.HoverFlagsForTooltipMouse 198af5ad81c30769aba4d18ffb7ca56fcbc393f8912ec59d89b3ed04141f65b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.HoverFlagsForTooltipNav 649bf8bfec28263097987a45d29a13a64f4d417a6f07865077d6baedf5937781 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.MainScale d38bca700c024ba7cefd6f37215c3b8e5ef77c5435963a69aad290dac2564829 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyle.NextFrameFontSizeBase 2048b9b57c8dfd97a146e3d0e6171dfb9abb824b642a8eacaba80e0fa41b742c -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyle.#ctor(System.Single,System.Single,System.Single,System.Single,System.Single,System.Numerics.Vector2,System.Single,System.Single,System.Single,System.Numerics.Vector2,System.Numerics.Vector2,Inno.Native.ImGui.ImGuiDir,System.Single,System.Single,System.Single,System.Single,System.Numerics.Vector2,System.Single,System.Single,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Numerics.Vector2,Inno.Native.ImGui.ImGuiTreeNodeFlags,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,Inno.Native.ImGui.ImGuiDir,System.Numerics.Vector2,System.Numerics.Vector2,System.Single,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Byte,System.Single,System.Single,System.Byte,System.Byte,System.Byte,System.Single,System.Single,System.Numerics.Vector4*,System.Single,System.Single,System.Single,Inno.Native.ImGui.ImGuiHoveredFlags,Inno.Native.ImGui.ImGuiHoveredFlags,System.Single,System.Single) 24b5944136a6e9e5e3181651da632f60719a1b3e2aa80bf73f6f179d08d49d3b -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyle.Destroy e1008366b8f89748c383e2de64bd5b096080a395ea105857f59525f9b3b0ff42 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyle.ScaleAllSizes(System.Single) 7ba1e0a573f3e8f6d5bea811a99081e14a481817507267977c9b3c1c35a8d46e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6ef591bb0c0dce3e"></a>[`public float Inno.Native.ImGui.ImGuiStyle.Alpha`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3483) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b07068c43f0fb233"></a>[`public byte Inno.Native.ImGui.ImGuiStyle.AntiAliasedFill`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3543) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7f599aa5818a59da"></a>[`public byte Inno.Native.ImGui.ImGuiStyle.AntiAliasedLines`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3541) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ea026ae9af1dda5"></a>[`public byte Inno.Native.ImGui.ImGuiStyle.AntiAliasedLinesUseTex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3542) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5722ffb5cb47d962"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.ButtonTextAlign`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3531) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-41c90bbf0ee50b47"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.CellPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3501) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-994c912497ea8ad8"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ChildBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3493) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e4179c5543f51fbc"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ChildRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3492) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c86e0b0e6084ff5"></a>[`public float Inno.Native.ImGui.ImGuiStyle.CircleTessellationMaxError`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3545) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64d570b6a983c24d"></a>[`public Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiStyle.ColorButtonPosition`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3530) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d17e68bab6396d47"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ColorMarkerSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3529) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0811f1b6f0fe0853"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3546) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9fc8239b9e83c977"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3547) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c1651d9d711458f"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3556) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1482f15576f67d68"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3557) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-84cb019271dac9cb"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3558) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af8963af1d06ad63"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3559) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4492d56a096c1aaf"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3560) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6e556491797a9fcc"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3561) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0b3e986ac5eb1f27"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3562) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e4b1e957f7e00c62"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3563) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a33a6c02577169e5"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3564) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9a1c7f1feb616730"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3565) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7809ddcfa5f47a06"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3548) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b19131112407dcd0"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3566) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df44eb7d0855d2b9"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3567) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0537e5c3592cdced"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3568) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3809bb0807c667b2"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3569) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-810d5ee2b65eaa64"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3570) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7df969b9cafadb6a"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3571) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c0e3a93a5fb25cd"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3572) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf5d145ac0146ed6"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3573) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d7cc4be5d516375f"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3574) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b98a94bf9f65091a"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3575) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e79cbff902f954ef"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3549) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc8326e1033ee047"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3576) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b697f6a2c9b5c9a9"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3577) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df1f8388ab602565"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3578) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-998fa91087a421fd"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_33`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3579) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-662fb045f3a639c7"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_34`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3580) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f56cbcf01d102133"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_35`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3581) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2b7fb29954fcf04a"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_36`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3582) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6b998fdbdca61128"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_37`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3583) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-deb4e6cbba8f55cc"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_38`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3584) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ca1c95792f99d601"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_39`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3585) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b222f53cb1bafead"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3550) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-389640dd5331878f"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_40`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3586) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd20fb275934f637"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_41`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3587) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d25c1c5cb0f3e9e5"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_42`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3588) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c12aa2d73b5e70a8"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_43`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3589) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8f9d1e2900709381"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_44`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3590) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6a916968274508a3"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_45`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3591) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4354774e48c84b1f"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_46`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3592) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-423d17de456b843b"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_47`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3593) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccf933b5e05739d3"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_48`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3594) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa0d60cc45fbbac5"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_49`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3595) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-026acc1cf6a11027"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3551) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-68d9187949f355ad"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_50`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3596) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa87360b4851f788"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_51`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3597) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-528b509cb2bd5969"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_52`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3598) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-409de5bf9932bc06"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_53`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3599) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c63713c8dec28cc6"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_54`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3600) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-74d236f7d9a30754"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_55`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3601) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4559b21c175ac8b"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_56`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3602) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d3d9e6504ad29758"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_57`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3603) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ef38f663de5ed13d"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_58`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3604) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1afcf3d401b66a61"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_59`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3605) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2fe2fcda69a8a95f"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3552) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-90a2ea8cd9d6c9d3"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_60`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3606) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5233fc13929cd2d0"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_61`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3607) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b6fafd04a87fd657"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3553) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-41eb68675f64bcff"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3554) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fbec509347dc3952"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImGuiStyle.Colors_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3555) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b45c626999de213"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ColumnsMinSpacing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3504) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6e118ce57cb59e3c"></a>[`public float Inno.Native.ImGui.ImGuiStyle.CurveTessellationTol`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3544) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fead72db9eb52e99"></a>[`public float Inno.Native.ImGui.ImGuiStyle.DisabledAlpha`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3484) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f03d779c27451eee"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.DisplaySafeAreaPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3537) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6f0ea87a47f7526"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.DisplayWindowPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3536) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89a57e5c0450b4dd"></a>[`public byte Inno.Native.ImGui.ImGuiStyle.DockingNodeHasCloseButton`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3538) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-95e02ec03b26164d"></a>[`public float Inno.Native.ImGui.ImGuiStyle.DockingSeparatorSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3539) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-163a2ecf8e7e9493"></a>[`public float Inno.Native.ImGui.ImGuiStyle.DragDropTargetBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3527) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a3e2161d1adb92bd"></a>[`public float Inno.Native.ImGui.ImGuiStyle.DragDropTargetPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3528) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-240ef39e41989482"></a>[`public float Inno.Native.ImGui.ImGuiStyle.DragDropTargetRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3526) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e3370b1c6643a08"></a>[`public float Inno.Native.ImGui.ImGuiStyle.FontScaleDpi`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3482) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c04cc26d02191d7"></a>[`public float Inno.Native.ImGui.ImGuiStyle.FontScaleMain`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3481) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-71575ba54d1d5ebd"></a>[`public float Inno.Native.ImGui.ImGuiStyle.FontSizeBase`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3480) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-acc47f9fefcfe033"></a>[`public float Inno.Native.ImGui.ImGuiStyle.FrameBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3498) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fff4fa87263022eb"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.FramePadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3496) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a640d64dfd1fac3"></a>[`public float Inno.Native.ImGui.ImGuiStyle.FrameRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3497) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-db3411846e904b96"></a>[`public float Inno.Native.ImGui.ImGuiStyle.GrabMinSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3508) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c175d31c4fca73fc"></a>[`public float Inno.Native.ImGui.ImGuiStyle.GrabRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3509) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fac00b8e160c3335"></a>[`public float Inno.Native.ImGui.ImGuiStyle.HoverDelayNormal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3612) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0db3459e1786ee7e"></a>[`public float Inno.Native.ImGui.ImGuiStyle.HoverDelayShort`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3611) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba9d6dd2f65a4052"></a>[`public Inno.Native.ImGui.ImGuiHoveredFlags Inno.Native.ImGui.ImGuiStyle.HoverFlagsForTooltipMouse`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3613) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e1cc88ee28439430"></a>[`public Inno.Native.ImGui.ImGuiHoveredFlags Inno.Native.ImGui.ImGuiStyle.HoverFlagsForTooltipNav`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3614) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ff3fbe9e600498c3"></a>[`public float Inno.Native.ImGui.ImGuiStyle.HoverStationaryDelay`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3610) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6990e2649707f5ec"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ImageBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3512) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cdbeff14e4b2bd2f"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ImageRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3511) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-046e488af5e97e09"></a>[`public float Inno.Native.ImGui.ImGuiStyle.IndentSpacing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3503) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-83e9202f67d39b55"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.ItemInnerSpacing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3500) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-45ce9f00abc10b06"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.ItemSpacing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3499) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21c28c24b92b2e96"></a>[`public float Inno.Native.ImGui.ImGuiStyle.LogSliderDeadzone`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3510) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-553d5ba6ce3ab326"></a>[`public float Inno.Native.ImGui.ImGuiStyle.MainScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3615) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-664c3314f63fc258"></a>[`public float Inno.Native.ImGui.ImGuiStyle.MouseCursorScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3540) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2cbfcce9020cb1f"></a>[`public float Inno.Native.ImGui.ImGuiStyle.NextFrameFontSizeBase`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3616) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c7607553eb09b91f"></a>[`public float Inno.Native.ImGui.ImGuiStyle.PopupBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3495) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9aa1d3f6f683f5be"></a>[`public float Inno.Native.ImGui.ImGuiStyle.PopupRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3494) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e383f90281f2a8ae"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ScrollbarPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3507) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-52f1864181ab3103"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ScrollbarRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3506) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-075e79c6474e5b35"></a>[`public float Inno.Native.ImGui.ImGuiStyle.ScrollbarSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3505) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b786b5e793fa9e1"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.SelectableTextAlign`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3532) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b148f428863522a"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.SeparatorTextAlign`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3534) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e670f22cf167c056"></a>[`public float Inno.Native.ImGui.ImGuiStyle.SeparatorTextBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3533) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-51973c57077fe667"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.SeparatorTextPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3535) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ebe5554e15a160db"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabBarBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3519) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d65cf64fe9f2cec"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabBarOverlineSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3520) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-db333c541726c49b"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3514) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f7e09a3979fd8e62"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabCloseButtonMinWidthSelected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3517) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e4e40f7462410644"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabCloseButtonMinWidthUnselected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3518) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a8de2ca5ee1e84ec"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TableAngledHeadersAngle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3521) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13d39dabb5fcdd18"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.TableAngledHeadersTextAlign`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3522) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9bde22232725965a"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabMinWidthBase`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3515) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f71d1380475d8546"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabMinWidthShrink`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3516) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89d9f0e5473a9b0b"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TabRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3513) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e99c12bdc53eb46"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.TouchExtraPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3502) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-42e20a72e3a6f369"></a>[`public Inno.Native.ImGui.ImGuiTreeNodeFlags Inno.Native.ImGui.ImGuiStyle.TreeLinesFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3523) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c5d43e108046b37"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TreeLinesRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3525) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-600ba61660acf748"></a>[`public float Inno.Native.ImGui.ImGuiStyle.TreeLinesSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3524) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d7c8f3b399b71d10"></a>[`public float Inno.Native.ImGui.ImGuiStyle.WindowBorderHoverPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3488) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4309b50615a5f17c"></a>[`public float Inno.Native.ImGui.ImGuiStyle.WindowBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3487) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b0969369746e519d"></a>[`public Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiStyle.WindowMenuButtonPosition`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3491) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0ed37898ed8ec126"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.WindowMinSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3489) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-038bf9667c158bcd"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.WindowPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3485) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c836bb5efa11e76"></a>[`public float Inno.Native.ImGui.ImGuiStyle.WindowRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3486) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53b712cea320be03"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStyle.WindowTitleAlign`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3490) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-15160f573abe03fd"></a>[`public Inno.Native.ImGui.ImGuiStyle.ImGuiStyle(float fontSizeBase = 0, float fontScaleMain = 0, float fontScaleDpi = 0, float alpha = 0, float disabledAlpha = 0, System.Numerics.Vector2 windowPadding = default(System.Numerics.Vector2), float windowRounding = 0, float windowBorderSize = 0, float windowBorderHoverPadding = 0, System.Numerics.Vector2 windowMinSize = default(System.Numerics.Vector2), System.Numerics.Vector2 windowTitleAlign = default(System.Numerics.Vector2), Inno.Native.ImGui.ImGuiDir windowMenuButtonPosition = Inno.Native.ImGui.ImGuiDir.Left, float childRounding = 0, float childBorderSize = 0, float popupRounding = 0, float popupBorderSize = 0, System.Numerics.Vector2 framePadding = default(System.Numerics.Vector2), float frameRounding = 0, float frameBorderSize = 0, System.Numerics.Vector2 itemSpacing = default(System.Numerics.Vector2), System.Numerics.Vector2 itemInnerSpacing = default(System.Numerics.Vector2), System.Numerics.Vector2 cellPadding = default(System.Numerics.Vector2), System.Numerics.Vector2 touchExtraPadding = default(System.Numerics.Vector2), float indentSpacing = 0, float columnsMinSpacing = 0, float scrollbarSize = 0, float scrollbarRounding = 0, float scrollbarPadding = 0, float grabMinSize = 0, float grabRounding = 0, float logSliderDeadzone = 0, float imageRounding = 0, float imageBorderSize = 0, float tabRounding = 0, float tabBorderSize = 0, float tabMinWidthBase = 0, float tabMinWidthShrink = 0, float tabCloseButtonMinWidthSelected = 0, float tabCloseButtonMinWidthUnselected = 0, float tabBarBorderSize = 0, float tabBarOverlineSize = 0, float tableAngledHeadersAngle = 0, System.Numerics.Vector2 tableAngledHeadersTextAlign = default(System.Numerics.Vector2), Inno.Native.ImGui.ImGuiTreeNodeFlags treeLinesFlags = Inno.Native.ImGui.ImGuiTreeNodeFlags.None, float treeLinesSize = 0, float treeLinesRounding = 0, float dragDropTargetRounding = 0, float dragDropTargetBorderSize = 0, float dragDropTargetPadding = 0, float colorMarkerSize = 0, Inno.Native.ImGui.ImGuiDir colorButtonPosition = Inno.Native.ImGui.ImGuiDir.Left, System.Numerics.Vector2 buttonTextAlign = default(System.Numerics.Vector2), System.Numerics.Vector2 selectableTextAlign = default(System.Numerics.Vector2), float separatorTextBorderSize = 0, System.Numerics.Vector2 separatorTextAlign = default(System.Numerics.Vector2), System.Numerics.Vector2 separatorTextPadding = default(System.Numerics.Vector2), System.Numerics.Vector2 displayWindowPadding = default(System.Numerics.Vector2), System.Numerics.Vector2 displaySafeAreaPadding = default(System.Numerics.Vector2), byte dockingNodeHasCloseButton = 0, float dockingSeparatorSize = 0, float mouseCursorScale = 0, byte antiAliasedLines = 0, byte antiAliasedLinesUseTex = 0, byte antiAliasedFill = 0, float curveTessellationTol = 0, float circleTessellationMaxError = 0, System.Numerics.Vector4* colors = null, float hoverStationaryDelay = 0, float hoverDelayShort = 0, float hoverDelayNormal = 0, Inno.Native.ImGui.ImGuiHoveredFlags hoverFlagsForTooltipMouse = Inno.Native.ImGui.ImGuiHoveredFlags.None, Inno.Native.ImGui.ImGuiHoveredFlags hoverFlagsForTooltipNav = Inno.Native.ImGui.ImGuiHoveredFlags.None, float mainScale = 0, float nextFrameFontSizeBase = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3617) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-797a2ae13835ae14"></a>[`public void Inno.Native.ImGui.ImGuiStyle.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63222) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-22eb83e98234d7db"></a>[`public void Inno.Native.ImGui.ImGuiStyle.ScaleAllSizes(float scaleFactor)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63233) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-95b1f7cecf285cda"></a>[`public System.Span<System.Numerics.Vector4> Inno.Native.ImGui.ImGuiStyle.Colors { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3608) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-277de4eee3f0cc8b"></a>[`public struct Inno.Native.ImGui.ImGuiStyle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3478) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStyleMod</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStyleMod 18726e7e65cde78836b7ae3af7aa1bd0d93898df4c5124024651284774c7db38 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleMod.VarIdx 19bb453917139c40f4149265d41262f467748c20ff5241fa0970233688841473 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleMod.AnonymousField1 a5bf0be10f2ec98ccb73d0fd38657af31c4a266f228334caa7fa54374a772d28 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleMod.#ctor(Inno.Native.ImGui.ImGuiStyleVar,Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous) fb93424ffef45889ef25c23e9a3bc2c8249ccc64faca41a557c3e0389296862e -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleMod.Destroy 3dc696a46ca6d8f01ea608f4baf4d5f9f9a2d393a97c252d09aac76a7902dcd2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9557dbd2824ff960"></a>[`public Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous Inno.Native.ImGui.ImGuiStyleMod.AnonymousField1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5710) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a75b9abf0d0ba9d"></a>[`public Inno.Native.ImGui.ImGuiStyleVar Inno.Native.ImGui.ImGuiStyleMod.VarIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5709) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-17a869cdeebf3cd3"></a>[`public Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleMod(Inno.Native.ImGui.ImGuiStyleVar varIdx = Inno.Native.ImGui.ImGuiStyleVar.Alpha, Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous value1 = default(Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5711) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d01f7f8985b673bb"></a>[`public void Inno.Native.ImGui.ImGuiStyleMod.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66430) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a5dddd86ffd1aa22"></a>[`public struct Inno.Native.ImGui.ImGuiStyleMod`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5688) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous 5e7072689ec95473833be421f702f9fcb2f04e891ef1c802893ff91234bf65f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous.BackupInt_0 1c9943284017a475cd8f60352b3cad9972cb04b56398808a6deb309b3d7cd07d -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous.BackupInt_1 206a5789ab41c033ad47d0e98ff3e7314db9240526bbfe9fe9c20334e02b9fb6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous.BackupInt 843377402eb15381b37056460125c8f127085a9c5ac33a96142c21beea8bd5e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous.BackupFloat_0 ce6cd60c3f552a66e1e8c953c5c07ee89a13c517e65a0c315c602d7f3010afdf -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous.BackupFloat_1 d6bfe53a49e2aa8dec970c33913d0424dd09e9f501fe983dc80ed292691d0670 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous.BackupFloat 4bd558969b426c6c67a6186be8da26d517e2550d36c206ed672957dfc617e401 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-54779f06549d858f"></a>[`public float Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous.BackupFloat_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5703) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-462df4fc9bed5abb"></a>[`public float Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous.BackupFloat_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5705) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a85cb931d6eb570a"></a>[`public int Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous.BackupInt_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5697) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-96b7fd11b600a287"></a>[`public int Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous.BackupInt_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5699) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2c3d54ce4cc5abfa"></a>[`public System.Span<float> Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous.BackupFloat { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5706) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dc5e3cf4a8ff3a10"></a>[`public System.Span<int> Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous.BackupInt { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5700) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-253d92cc8c1d990b"></a>[`public struct Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5694) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStyleModPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStyleModPtr c9bc4c750886fed4ffa5d54047d652af70371d1d1774d865e5456c0d4c3438e0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleModPtr.#ctor(Inno.Native.ImGui.ImGuiStyleMod*) 2f500cd31000ccf609cda6802f691962025f6a924319be1c156f39abe5ae89aa -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleModPtr.Handle b12bdb81a4ba7e07de4fc4f44b4d965bb48432c8260a4c216c9717f94db6926b -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleModPtr.IsNull c7193d4f7b929c6da3e66de19fc42636cfc93da0c6cec99c914a0e9fe4158028 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleModPtr.Null f278cf78381f0d7ce3084fe2f6c7a7d6a8d417bea9d30bb16743b7ed20c8de3d -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleModPtr.Item(System.Int32) b6dd8eff11a1c9ee697769b0993bd85cd5348bf9fc519413bf323a097f2e47d8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleModPtr.op_Implicit(Inno.Native.ImGui.ImGuiStyleMod*)~Inno.Native.ImGui.ImGuiStyleModPtr c405dad35ef16b92c3611f3ca1d917189218205408d435958b484d9b7003c480 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleModPtr.op_Implicit(Inno.Native.ImGui.ImGuiStyleModPtr)~Inno.Native.ImGui.ImGuiStyleMod* d7a00a11bfbdfd17a3f2015b5bbb70259b1ef074085d9d0ece2917200b1408e8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleModPtr.op_Equality(Inno.Native.ImGui.ImGuiStyleModPtr,Inno.Native.ImGui.ImGuiStyleModPtr) 414003845a16587497221b688ad68420ef80f240175fe5713e230947ab1f6044 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleModPtr.op_Inequality(Inno.Native.ImGui.ImGuiStyleModPtr,Inno.Native.ImGui.ImGuiStyleModPtr) df4c123481e9140b5324bf5d63c4ebed4aae476e7beb6081ee68d1d312f4aa19 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleModPtr.Equals(Inno.Native.ImGui.ImGuiStyleModPtr) 52b64d2803cd6e5d034721cbbee59563ed7e7b8947855579f1a2d2c96dae82e1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleModPtr.Equals(System.Object) 7ed871d69371ee5d7e54c2a4f4d0566e6224ef6b6662ec37e55dbd98cb832baa -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleModPtr.GetHashCode 95107953d037e157aa269ebe7680aee1e24576e1d3b9ae27829f5051a32594b3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleModPtr.VarIdx 54740c5ef5b9e992d8d90a67f8efc3b761231844ead24997ea269998efaf2663 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleModPtr.AnonymousField1 c72e6fee12d6db94fc63046ba0f0b4138f6f08e0fe72dc45db34ed6a0815b32d -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleModPtr.Destroy 50756530dc74649ce07b02bd7a45f430895bc36923ca4274013cb72b361af2a3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9ebcf6e74c72ece3"></a>[`public Inno.Native.ImGui.ImGuiStyleMod* Inno.Native.ImGui.ImGuiStyleModPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16441) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6716eab2fd189b5e"></a>[`public Inno.Native.ImGui.ImGuiStyleModPtr.ImGuiStyleModPtr(Inno.Native.ImGui.ImGuiStyleMod* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16436) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b1e20a554bb10bd6"></a>[`public void Inno.Native.ImGui.ImGuiStyleModPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16461) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8e0dfe7b371ee7db"></a>[`public bool Inno.Native.ImGui.ImGuiStyleModPtr.Equals(Inno.Native.ImGui.ImGuiStyleModPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16451) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-c3fff83df978abb3"></a>[`public override bool Inno.Native.ImGui.ImGuiStyleModPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16452) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-c1a327f8b091e249"></a>[`public override int Inno.Native.ImGui.ImGuiStyleModPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16453) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-16e7deca45cdc644"></a>[`public static bool Inno.Native.ImGui.ImGuiStyleModPtr.operator ==(Inno.Native.ImGui.ImGuiStyleModPtr left, Inno.Native.ImGui.ImGuiStyleModPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16449) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0b18dbc812a9fa90"></a>[`public static Inno.Native.ImGui.ImGuiStyleModPtr.implicit operator Inno.Native.ImGui.ImGuiStyleModPtr(Inno.Native.ImGui.ImGuiStyleMod* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16447) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d19094637eb8bf5e"></a>[`public static Inno.Native.ImGui.ImGuiStyleModPtr.implicit operator Inno.Native.ImGui.ImGuiStyleMod*(Inno.Native.ImGui.ImGuiStyleModPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16448) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ce2bdc503c30362e"></a>[`public static bool Inno.Native.ImGui.ImGuiStyleModPtr.operator !=(Inno.Native.ImGui.ImGuiStyleModPtr left, Inno.Native.ImGui.ImGuiStyleModPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16450) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e6d6aa530e9ddcc5"></a>[`public ref Inno.Native.ImGui.ImGuiStyleMod.ImGuiStyleModAnonymous Inno.Native.ImGui.ImGuiStyleModPtr.AnonymousField1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16456) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-42ca5dd7394193ef"></a>[`public bool Inno.Native.ImGui.ImGuiStyleModPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16442) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b1f108b03cf5e833"></a>[`public Inno.Native.ImGui.ImGuiStyleMod Inno.Native.ImGui.ImGuiStyleModPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16445) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bcb7a0bf45f2e30a"></a>[`public static Inno.Native.ImGui.ImGuiStyleModPtr Inno.Native.ImGui.ImGuiStyleModPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16443) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-49ea6e462d5e32f0"></a>[`public ref Inno.Native.ImGui.ImGuiStyleVar Inno.Native.ImGui.ImGuiStyleModPtr.VarIdx { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16455) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c99fefbf5f492185"></a>[`public struct Inno.Native.ImGui.ImGuiStyleModPtr : System.IEquatable<Inno.Native.ImGui.ImGuiStyleModPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16434) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStylePtr</code> · 89 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStylePtr d1ab0018f75306894544fda05ee770c3a8fab1fa42d9fe608832cf74a5230bf2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStylePtr.#ctor(Inno.Native.ImGui.ImGuiStyle*) 076b62c069dc7ad80b60f00f09394d470372615178f5070c43e17f17c9ac40fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiStylePtr.Handle ba4f728e79c9d6d521a4b1935f266fd594c0efca6ba6824bf614b8bb7420ef0b -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.IsNull d70a69e3c3181da4ed442511ba44c03fcaacd02a49e1fd12d36417dd2c5f2cd2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.Null 639781abe5f98f9ce3dcd1a362c7f1d9f395c78f144972145b3f85e49cdf0c71 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.Item(System.Int32) d9a2ac4b370e5b29043c537e52d9d18373fda934552ab16879903b99316c6218 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStylePtr.op_Implicit(Inno.Native.ImGui.ImGuiStyle*)~Inno.Native.ImGui.ImGuiStylePtr 3762701bdb0b4d7982a9dc2e68c806077df6e2f9587e22b99ac70adaf9ac8c05 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStylePtr.op_Implicit(Inno.Native.ImGui.ImGuiStylePtr)~Inno.Native.ImGui.ImGuiStyle* 1f9ac09f04577043be1b885d158fee05ec5acad0fdba74ec16c0f777e3b612e2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStylePtr.op_Equality(Inno.Native.ImGui.ImGuiStylePtr,Inno.Native.ImGui.ImGuiStylePtr) 62398aafa3d8ba54523f9c9f0fac1a7dffbc773d174679a701b073c3396abfa1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStylePtr.op_Inequality(Inno.Native.ImGui.ImGuiStylePtr,Inno.Native.ImGui.ImGuiStylePtr) 671729338fb45c4fe1cd5f8fbc825dd4bc1a69d933a68b3f3a5f8c7e8f112aec -->
<!-- api: M:Inno.Native.ImGui.ImGuiStylePtr.Equals(Inno.Native.ImGui.ImGuiStylePtr) 5834d7b82b082adbe7ec39bdecb730c7a91fae21d2c238718d88580d51d77852 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStylePtr.Equals(System.Object) 1f17d3b81ad3653619277bd91d12542ac1fbf5a7e166ddf9f1105f9d5630076b -->
<!-- api: M:Inno.Native.ImGui.ImGuiStylePtr.GetHashCode 869893b1b791883788795c64a6ac0841a4151335234aee50b809e34c375529bc -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.FontSizeBase 9cb14be403d8c490d571a937238ef8cb8d59d3c077c658d1c41cbc2f97c86293 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.FontScaleMain c1735db8af5d5b50db2157862b91f5c89d7aa977f069e10aac5d7bc2b16a39a3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.FontScaleDpi 4a148104cf38fdbdbf1519b92fa49ad439fb8404a846db74b64d8f2837bef885 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.Alpha f39ebea00170534caf549059d4fe752ad69512e595983f8309f6e6474efeb1ca -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.DisabledAlpha 971304972508bdb3b5040d0b0b77a587d16cd9339f434cb417aa46175adaec4b -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.WindowPadding 409b0515b5d950ee195da267ed5283a5fbe1d87dcc492a72b324c1ccb2bea58c -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.WindowRounding 390a405cb81dd1dcfad73d443a82a6791cf5cd348d10e96b28103b195a2eb67d -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.WindowBorderSize db9d9a9f269d068926b1c865650a2e67f0ffb626231cca3e144b8d9b89f95ac2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.WindowBorderHoverPadding b319e886ad424b5c8a3fe7e513ffe797d94e2fd4cc0c6b9a7ee7f425bb059aa2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.WindowMinSize 8ad25f3606a9d1413f0f1dfc4151a04881fe12cef16c07732f7ee3240f9796d0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.WindowTitleAlign d1c788bb0f402c24ec279f2d990d993bb56ca51900029f86296fba51f7ff93ad -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.WindowMenuButtonPosition fd817e26a3db4670cb389bbc0a63f4ed00ff69c2c82bdd3ab43dd1157f526b50 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.ChildRounding 688ceb5a440a5815ef345e063b9a79a02142f8e3e6f003e12296afe756cfab1e -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.ChildBorderSize 467565ba834fc4086341c47df56d9569252dce1ee334741b9532a1c17558add8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.PopupRounding 04ea5bbcade27370330899c8e7f250edb938bcf2994ccfec255055f754678c7f -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.PopupBorderSize ac7903d77421ec70e4210f97ba1cb54db588acfbf4dfc7460eb2f0792e8d675c -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.FramePadding 12e8ed75262df54dd5dbce4806a0abf6aa04161b2022c58d5047548ee4d95102 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.FrameRounding be7a6844ff93f20148c7d4a1311240dc9fe575247c60319896b519f43ba84d91 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.FrameBorderSize 878e544826621fc22c4717af5b50e843e4f560a3c85739bb67ec9ce59b12c4bd -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.ItemSpacing a40861267568ff664e513b2f3cc4f5652edd61ab806525488c517d9733d2db20 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.ItemInnerSpacing 9ee9d0d1d7191ba4513270a04d554a07bbf65b87f5f4d94f6edcf7dd4fb62fcb -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.CellPadding 270ba6a9f3e6dce5506925c14450af27d294d72465cfa90be87c49fb92c9a0b0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TouchExtraPadding 60657f802b942ebc80352b9cefa46dd733f1824f3b55cbfa3dfbaaaa22d54018 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.IndentSpacing 5400d2258a4bd8b25389d7b78646e117973994dfc2fc578220b4f3d4bda0870c -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.ColumnsMinSpacing 8d8a35a6afe6f4af9f6ebe0a136fed404343bd4826418a99196616b28fb8431f -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.ScrollbarSize 4d7c7db2951057012d1462282c1ffb7f5acb77b933acb8dca3cc3cb74504efd3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.ScrollbarRounding 5100992e047ebf3d910833b09bca5b59d5d1d7868747b9be39d33346a8fe66e4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.ScrollbarPadding e1d11f49dff5580b8a992e4f528008b73db5eab7c63f84489fc37fc857c7314c -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.GrabMinSize 8d0843f4e56e43e89c3e930ebcb804901f46e7ee2dd4b806c4a91e9006aae8dd -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.GrabRounding dc4a77ceaacb4d3edc24728b2ddb0bd125f1dc4a6c621ee32aa7531f576acb6f -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.LogSliderDeadzone 9ff034b39e1b4c238f63fffb7238386dc62b16ed13295afe408d9f0e5524775a -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.ImageRounding 2d8832219c40de24348b8b96fa3c20dcd70e8d4ded81fc5c0ac6f64d513bd128 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.ImageBorderSize c14a052f22a7a203857f9ff22cd7b6ef80c53db6633f508fb857e69018cba3ee -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TabRounding cdc855cb5cf583ad6b43b1c60f02d1aac7f85e81f701674ec1126389104d060f -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TabBorderSize 082628fe5baf1793a22cbe3527b57cda9e76d32feee9e5db9c339d2ce04bf9cc -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TabMinWidthBase 78abe7871c6b12454eeeff62326756dacf216457aee2a09a4502bf09ff2bfec4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TabMinWidthShrink cc2d62158e89983f0ac9e159fdab7e0485d362dabeabb1cf933268fe4c529b0d -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TabCloseButtonMinWidthSelected ef43c98e83bd22279287f19c2c1669c95cd6b78eb748d6fa07bb3ce9e4b5bc5e -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TabCloseButtonMinWidthUnselected 76f0f957480d5f5d83e9fce4045712db632387d4bc05dc1224b25b46b3a6b070 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TabBarBorderSize 003d799053e43bbb9a793a69eb4306d19d12479949cb7df2aee63ba2ef606ead -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TabBarOverlineSize bfe099cc7ccd4a05515339b2845cc795af731806ccb03edea914db3d0dc98d3f -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TableAngledHeadersAngle 7b051348095f68e9b6d3328be643d67f7c265675453df3ff93beab214278d9d8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TableAngledHeadersTextAlign 063b805956b448a21344ec4ee0b2759477ffe28d71a0650cb37741210c86806e -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TreeLinesFlags 048dc2c7afd8ecbfe531f2e90609a77ab01f1fef9ced3d9f92425946d44bd616 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TreeLinesSize 4acc8bd7e3cada789cf491ba00bc6df2252eada7b42f3f0db576185cacb9ac76 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.TreeLinesRounding 73d29c1e6942cd7470eea62458d2c3ab290bd184b1c1681897c78ea323974a27 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.DragDropTargetRounding b346d4e357cc8ed0903e6d404b5ab9b38e2093d0fceaba90ffbf0f9aa6fad331 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.DragDropTargetBorderSize 0fdb87b6dc51ae964a5ac46eab7b2858b4180bcad69fe53e09ca7dd0d291817c -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.DragDropTargetPadding 731ea04d1f2e5f020becb29971f84a07e321e66beaeec6d028723dc2eb38bfc7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.ColorMarkerSize 91e97f81b96e6006ee3c02bd7b8484d410b5a5901fac8fb58d1541e2344e5745 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.ColorButtonPosition add69dc9b23ceaf23601f793330d32c0bb3ae6b9ea7fd2812bd5cc97a72ea5bd -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.ButtonTextAlign b93e2263eee4a6e5bc5dbffe3a70ac8dbb975a3d33d2ff0dbdf9df3be16420a6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.SelectableTextAlign 086a5b03e2b85254c5688574ce0194dbca496f358d9aa37b26d7a9930ad97d48 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.SeparatorTextBorderSize 28d83e8e0f21b9af5254c384cde1be94af909544893164c42faf449013713cb9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.SeparatorTextAlign 1f27c958e00f5fc2815a082cea5244a31550be606a374402de711dee856b1ef9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.SeparatorTextPadding 6ea55bea8217f9b7b9010e6a8efa8ccc00c834220609fc2eed027ab5b66d7eb8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.DisplayWindowPadding fe4aa032ef81af1f845f8b1350cbbf621e032cc62178061448c52a5e5d043c4c -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.DisplaySafeAreaPadding e77e27322ae142a5299c6fea580e32b6683f73e282c7348571fafca631936e4d -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.DockingNodeHasCloseButton b40eaa37367891ce3d4511258516ee5e0243dc72955a044b4071706c6d77f98e -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.DockingSeparatorSize 2d74f78ea6f3e5171dbe316b506cad4ef453fd157f8f8b2edd557fbfab4fa3fe -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.MouseCursorScale 949a9e17b54088288723f79fea553d0ca92c3dcfdc08405c5b822de800f0852c -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.AntiAliasedLines 240f16a170acd711ca7e7a723b32b8a47d5866b8e13124bdcf9cb2ad02c0f888 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.AntiAliasedLinesUseTex effdfcad2916ab60e5f8c2301aa354f2ab464ebe6ed6c06009192e2e51d7b819 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.AntiAliasedFill 0a7fc9edd13f56e4b1b8849a847245d8a1f5cb42cd60954614a1a79287b92478 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.CurveTessellationTol 90fe510aec8fa3b35d6307a4e58f8bd0a71d5da3b8de25f56532c21392f82332 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.CircleTessellationMaxError 49ae605138fa91cde4982dfdc2274be789d38464a8bc4bc27df8d6c4bb7fddbe -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.Colors 16e6188e5a0f6b585ae333894daa0964f11ff71690c57eb4b16c265d7bd162ab -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.HoverStationaryDelay 98e23316a7426c7ea4ff6f22f5b7221c56b232d663499786ba394ad299887e03 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.HoverDelayShort f29c29bcbcb1b79506cd6980bc3944117a05dfe7539412e925c3216128f3f090 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.HoverDelayNormal aa6a7ad83866ba01a2bda2a9f3ff9f6929a6c376d4077d60573e2dc464be1d12 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.HoverFlagsForTooltipMouse d54c72235bcecdf438fa11b5b2924b3e48bb7d5f6955dd55ea3b9ac58bd99b1e -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.HoverFlagsForTooltipNav d7f654782b031196fd2146a6d7ab5f907a561ffa24d220bea618db80c21a27e1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.MainScale 63d833a6721871bef83973fdae8225cddc5ce20b2de6188238de4ce6044b1dda -->
<!-- api: P:Inno.Native.ImGui.ImGuiStylePtr.NextFrameFontSizeBase 1999b288226a3177436ed1b130ef9364d8179b69cdb191ffedcd35c87b133672 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStylePtr.Destroy 2bce47685aa1a2f5794a1bdb74220a1abd604cb433706b5eab4800eb47e0c174 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStylePtr.ScaleAllSizes(System.Single) ac917968c46bc45d94812dfa3c922b35355c0d4b3b4bc75e1a631b7ddda9c9e1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6919bcca54297bf6"></a>[`public Inno.Native.ImGui.ImGuiStyle* Inno.Native.ImGui.ImGuiStylePtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16475) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a128e466863f1a1e"></a>[`public Inno.Native.ImGui.ImGuiStylePtr.ImGuiStylePtr(Inno.Native.ImGui.ImGuiStyle* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16470) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e4687a1b2ebf6cb6"></a>[`public void Inno.Native.ImGui.ImGuiStylePtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16567) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7a05ee501f26a187"></a>[`public bool Inno.Native.ImGui.ImGuiStylePtr.Equals(Inno.Native.ImGui.ImGuiStylePtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16485) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-dc082c331c870722"></a>[`public override bool Inno.Native.ImGui.ImGuiStylePtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16486) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-2ff0c671ced84e26"></a>[`public override int Inno.Native.ImGui.ImGuiStylePtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16487) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-f2197017aea4f93a"></a>[`public static bool Inno.Native.ImGui.ImGuiStylePtr.operator ==(Inno.Native.ImGui.ImGuiStylePtr left, Inno.Native.ImGui.ImGuiStylePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16483) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e43baf0b84f6b6a4"></a>[`public static Inno.Native.ImGui.ImGuiStylePtr.implicit operator Inno.Native.ImGui.ImGuiStylePtr(Inno.Native.ImGui.ImGuiStyle* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16481) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3a70bbd41b928424"></a>[`public static Inno.Native.ImGui.ImGuiStylePtr.implicit operator Inno.Native.ImGui.ImGuiStyle*(Inno.Native.ImGui.ImGuiStylePtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16482) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4f860b479821ce03"></a>[`public static bool Inno.Native.ImGui.ImGuiStylePtr.operator !=(Inno.Native.ImGui.ImGuiStylePtr left, Inno.Native.ImGui.ImGuiStylePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16484) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a63509251e41d352"></a>[`public void Inno.Native.ImGui.ImGuiStylePtr.ScaleAllSizes(float scaleFactor)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16575) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2a813093c66f0ba7"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.Alpha { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16492) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c87af5ffe9e88406"></a>[`public bool Inno.Native.ImGui.ImGuiStylePtr.AntiAliasedFill { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16552) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f9549521f43039e6"></a>[`public bool Inno.Native.ImGui.ImGuiStylePtr.AntiAliasedLines { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16550) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-40a10c0bc8f9ab7f"></a>[`public bool Inno.Native.ImGui.ImGuiStylePtr.AntiAliasedLinesUseTex { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16551) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-78c2eb226c4c707a"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.ButtonTextAlign { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16540) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-42eba0c879500e2e"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.CellPadding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16510) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4406c79bde3886e4"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.ChildBorderSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16502) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-383c347a74405f19"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.ChildRounding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16501) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f70e8e589dc5185b"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.CircleTessellationMaxError { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16554) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-978184fd69bcb65b"></a>[`public ref Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiStylePtr.ColorButtonPosition { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16539) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5a634adbee95e66e"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.ColorMarkerSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16538) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ed444213aa987467"></a>[`public System.Span<System.Numerics.Vector4> Inno.Native.ImGui.ImGuiStylePtr.Colors { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16555) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dcbfb038ed67e24c"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.ColumnsMinSpacing { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16513) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-50f4cfcef1f92ec5"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.CurveTessellationTol { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16553) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-823b192d564b795a"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.DisabledAlpha { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16493) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ddc5d3b4e1c57094"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.DisplaySafeAreaPadding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16546) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c0fd442a42383461"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.DisplayWindowPadding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16545) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1f665575916152f8"></a>[`public bool Inno.Native.ImGui.ImGuiStylePtr.DockingNodeHasCloseButton { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16547) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-af1002473cc0ae35"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.DockingSeparatorSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16548) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cdbff52049314626"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.DragDropTargetBorderSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16536) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b3958f85cccb717a"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.DragDropTargetPadding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16537) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cf7091eed3334df1"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.DragDropTargetRounding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16535) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-29362d40bc8d8f7b"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.FontScaleDpi { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16491) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d0a492789b21d8f4"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.FontScaleMain { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16490) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-980b9d7f9450a4b3"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.FontSizeBase { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16489) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d30b1d26927226b4"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.FrameBorderSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16507) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-80b51c35a22e141e"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.FramePadding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16505) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9aee849e062ce61f"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.FrameRounding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16506) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6f3b4a50d22743f9"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.GrabMinSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16517) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-263eec5b2e092dbc"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.GrabRounding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16518) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-033a1e4978adde13"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.HoverDelayNormal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16558) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1fbf299bbfee48e1"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.HoverDelayShort { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16557) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-da8f39046d3fc193"></a>[`public ref Inno.Native.ImGui.ImGuiHoveredFlags Inno.Native.ImGui.ImGuiStylePtr.HoverFlagsForTooltipMouse { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16559) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b1e937d0cbd81cd7"></a>[`public ref Inno.Native.ImGui.ImGuiHoveredFlags Inno.Native.ImGui.ImGuiStylePtr.HoverFlagsForTooltipNav { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16560) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-db9e7fe2c65bb8cb"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.HoverStationaryDelay { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16556) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-688719225984a933"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.ImageBorderSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16521) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9be1a0454e4eb051"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.ImageRounding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16520) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fe94d3625293c3eb"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.IndentSpacing { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16512) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f40a3237d10a0330"></a>[`public bool Inno.Native.ImGui.ImGuiStylePtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16476) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8d4dd13452f07369"></a>[`public Inno.Native.ImGui.ImGuiStyle Inno.Native.ImGui.ImGuiStylePtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16479) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-75047b4cd5be9894"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.ItemInnerSpacing { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16509) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f7ab994fba25c779"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.ItemSpacing { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16508) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e9099be4414a4639"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.LogSliderDeadzone { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16519) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d53478bba6940043"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.MainScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16561) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0de65d6bfa9829e1"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.MouseCursorScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16549) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b2c9384379c4c733"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.NextFrameFontSizeBase { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16562) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-27312be094c0d9da"></a>[`public static Inno.Native.ImGui.ImGuiStylePtr Inno.Native.ImGui.ImGuiStylePtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16477) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e9906ec45e157a0e"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.PopupBorderSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16504) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f5578ea2523b87b6"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.PopupRounding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16503) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0069b0e70a4c1604"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.ScrollbarPadding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16516) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-28dd5cb4f0f78dce"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.ScrollbarRounding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16515) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3f6a8d25ba936836"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.ScrollbarSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16514) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-251c741c663127f0"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.SelectableTextAlign { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16541) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9ea4307f43343ebc"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.SeparatorTextAlign { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16543) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1ba25e9ae2f64b81"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.SeparatorTextBorderSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16542) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-47f6e16617f781e6"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.SeparatorTextPadding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16544) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-718cc9eb2769b11c"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.TabBarBorderSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16528) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8501aa0ecdb216cc"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.TabBarOverlineSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16529) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-08e1dde5c44e0bcc"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.TabBorderSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16523) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-214a8d2f3d36046d"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.TabCloseButtonMinWidthSelected { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16526) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-34ee19b105cb9e72"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.TabCloseButtonMinWidthUnselected { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16527) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-47a0451e7f3a26b7"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.TableAngledHeadersAngle { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16530) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-33d95e2aca48cbd5"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.TableAngledHeadersTextAlign { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16531) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f053674405a1e346"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.TabMinWidthBase { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16524) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-93aa594f7a8b3c08"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.TabMinWidthShrink { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16525) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-894dace2ea510294"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.TabRounding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16522) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fd9d1c46df2d60a5"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.TouchExtraPadding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16511) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0813a4552585b99f"></a>[`public ref Inno.Native.ImGui.ImGuiTreeNodeFlags Inno.Native.ImGui.ImGuiStylePtr.TreeLinesFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16532) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba66d74753a180f5"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.TreeLinesRounding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16534) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a8203759af993700"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.TreeLinesSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16533) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e7186c68ee008efe"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.WindowBorderHoverPadding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16497) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6b73b3e2107b742e"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.WindowBorderSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16496) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-273e5e2fd8625a1e"></a>[`public ref Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiStylePtr.WindowMenuButtonPosition { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16500) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-88cd091015a160aa"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.WindowMinSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16498) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1645c701c3100f49"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.WindowPadding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16494) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6e92fcef6053dbd2"></a>[`public ref float Inno.Native.ImGui.ImGuiStylePtr.WindowRounding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16495) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-652c15c608c6b15f"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiStylePtr.WindowTitleAlign { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16499) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-378fa89dc21dff45"></a>[`public struct Inno.Native.ImGui.ImGuiStylePtr : System.IEquatable<Inno.Native.ImGui.ImGuiStylePtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16468) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStyleVar</code> · 43 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStyleVar 9b2c4d31193f12d9579f5b9902e867279bed5e7ff1507504ab83897dc9c55069 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.Alpha 505143a19231b608333a1244b2fa61639623710b42cccab51873f46947c790f1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.DisabledAlpha a7a8e556559a46155df3a1b5bcf73a253da96ba0b0fcf958fbd61f009fe5ac60 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.WindowPadding b26f703855cffdeda52c34de761d74aaba8232492af4808b81ecd70414d4142c -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.WindowRounding f58de425c6800fb21d5d68e0c9333b21f40533350355725e842fe62dd7081a82 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.WindowBorderSize a8044c3e984b6f76f02c7d4454c6d7c887a050ede2a6aa0aaa05503b9263257d -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.WindowMinSize d6ba1749d12047d8fd882d7134b2ce8e1c41d58842be18fd2cad57e27da2387b -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.WindowTitleAlign be8580d6c6d457bc5a028d1ffa310733c4919a3275b0d5ec57f38a9c7758c403 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ChildRounding 531bac72fad0e0aee36b34a5115190af4a8cc40161dd6a7ee4ab80977950b337 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ChildBorderSize 4fda34a77d2afea697ef321ae567bf2d6ba966adb8b3a4bc5b99142dc8947fe4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.PopupRounding a3d7ddf6a43bd64cae13df9f7627d718132797cbfbe4ac7a3b4b7743aaa1a7fa -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.PopupBorderSize ae2e32786ede1e320e0754cdac506b11b6391e3d5222594bca0712f091422ed0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.FramePadding e4885399d90ef43285fbe93edeb234bb9057df249431ea24bd6ba7a71a0a20a1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.FrameRounding 14f62ddbb2526724f4af727d75b8e2629aca932b56b04458659587f7893cb1f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.FrameBorderSize 40fab9c62d666271c77a4d17124a1b74cb211fa0b9f431bce874589c59c765a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ItemSpacing 9feded6639ad590149802298230556b773d21c4368d9e1e7f8e75999b949fe6d -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ItemInnerSpacing bc6f3bc6181f11cf3dbffa36ab87bd142badd95410dad2b379a7c9ebe3ca0ad9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.IndentSpacing 8e094af1603cfd80d66122cb8919b66bee0e4c09c819e3750da930d03fe77c8e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.CellPadding c663828fa0e833250d8afe9451c45a7760f5a405cfc7c8bd82c036e01e1fbb56 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ScrollbarSize 414838963846bf4362bb73395c916f8ffd3e43ab6e9ee0ce0a6d3b1c768efd32 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ScrollbarRounding 18ab6150a04d0bbb852b6231e517db0e6163bf03534f9772ad60c89bcd7cf566 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ScrollbarPadding 1cf57d6d937e629c2262cfb9405c334c156de6b48404a437580f7876d0091350 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.GrabMinSize b9053ea3cc69de2481c26d82953cdb12076bf2d23c508f28316201445e0230d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.GrabRounding 6b6a51384257b07da1cf360ab6948f0888b54e1a8055906d7fe051dae2abc87c -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ImageRounding b80b38ce095e0f48268986fc0f15709b4b383394b6a76fef4f17bb1c13ad9d43 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ImageBorderSize 33b03ac7980a6586bc7ef6e8481327884ad36a07107a7d27ffcf8c67dbf72090 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TabRounding da3da2f88f137d3fc056318944c14dededb5d19653f27793e92db9131acbecbe -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TabBorderSize 1815ba7ccedf48158bb91d5ea494510b349b699a2209b35073e7d3f966aedd10 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TabMinWidthBase a669d6098f6710ce0520b077fb430ab9adcf58a0608ec865d551466106f1ccfc -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TabMinWidthShrink b8c148700898ff9beb73b88a537828e6869d22219fe4f8da2fe9f4bb4b9021cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TabBarBorderSize f10762e3945939f713a5459f650475a613c7f4877610cdf633393c7a2ed8aecc -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TabBarOverlineSize ee10862b0db69155515728b0082ac586e1cb0363284873815c0b95f1f5b0eb7d -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TableAngledHeadersAngle 676adf7ed3fdf2c1c100fc40123622b422356b3a0fc0e7138ccab353a9990ba5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TableAngledHeadersTextAlign 5260ee4b80d7098b18df51b054be9154c81ba90e12f38f57c3d14992656f37de -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TreeLinesSize 6865d239b7e0f439dce0bd12a0e588118080dd139f45241b542ad21427b0f652 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.TreeLinesRounding df4db1182df980b67ddea878eaeb5360a83a03299233eb7f0f54bbe5f87d2b27 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.ButtonTextAlign aee08ceccb94ebbdfaa2322da4d9ae62c7b1613947da24831dad34c15cb07d52 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.SelectableTextAlign 4ae369d792d57c4e5693265480ac8474b3abe991efdc61e1d5117530d8f54534 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextBorderSize 00c91d52c298d07e512ffe9023194311215d84c5ee832376052044b3eea81850 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextAlign 2c9b61e51949299a8e390c5a674b1f2c96e5fb100b1a58dfe4e6b3eab7bee9f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextPadding c759093a8efcc6c97ae770367c622c46c4716438b1857f122e9169e884d161c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.DockingSeparatorSize e2efb33680cf3ede5d8db52d70e5a2158e022d1a221821217273020c4c787f9e -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVar.Count b234cd1025ea132a66bbd51f21fccfbfc175392449bfafe7b1858a056c630870 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4178bd418c1842b3"></a>[`Inno.Native.ImGui.ImGuiStyleVar.Alpha = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L821) | 当前常量或枚举值：`0`。 | — |
| <a id="api-f458270b0cbf4ce3"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ButtonTextAlign = 35`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L856) | 当前常量或枚举值：`35`。 | — |
| <a id="api-36b231643629a070"></a>[`Inno.Native.ImGui.ImGuiStyleVar.CellPadding = 17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L838) | 当前常量或枚举值：`17`。 | — |
| <a id="api-30161d591c9b5460"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ChildBorderSize = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L829) | 当前常量或枚举值：`8`。 | — |
| <a id="api-1c006d4cee3106a5"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ChildRounding = 7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L828) | 当前常量或枚举值：`7`。 | — |
| <a id="api-3d23b52a51829094"></a>[`Inno.Native.ImGui.ImGuiStyleVar.Count = 41`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L862) | 当前常量或枚举值：`41`。 | — |
| <a id="api-b0b181b52014f2fa"></a>[`Inno.Native.ImGui.ImGuiStyleVar.DisabledAlpha = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L822) | 当前常量或枚举值：`1`。 | — |
| <a id="api-95113eae2a08ccd2"></a>[`Inno.Native.ImGui.ImGuiStyleVar.DockingSeparatorSize = 40`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L861) | 当前常量或枚举值：`40`。 | — |
| <a id="api-3a63ddfe53af5d0c"></a>[`Inno.Native.ImGui.ImGuiStyleVar.FrameBorderSize = 13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L834) | 当前常量或枚举值：`13`。 | — |
| <a id="api-2a0195db57e16e84"></a>[`Inno.Native.ImGui.ImGuiStyleVar.FramePadding = 11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L832) | 当前常量或枚举值：`11`。 | — |
| <a id="api-f0dfe068c67810c0"></a>[`Inno.Native.ImGui.ImGuiStyleVar.FrameRounding = 12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L833) | 当前常量或枚举值：`12`。 | — |
| <a id="api-49f2271008e0cacd"></a>[`Inno.Native.ImGui.ImGuiStyleVar.GrabMinSize = 21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L842) | 当前常量或枚举值：`21`。 | — |
| <a id="api-1e26e73019b19d0f"></a>[`Inno.Native.ImGui.ImGuiStyleVar.GrabRounding = 22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L843) | 当前常量或枚举值：`22`。 | — |
| <a id="api-e4b8b0d7bb7e4294"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ImageBorderSize = 24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L845) | 当前常量或枚举值：`24`。 | — |
| <a id="api-105ef4c5c851f115"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ImageRounding = 23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L844) | 当前常量或枚举值：`23`。 | — |
| <a id="api-127ff08efd6c1316"></a>[`Inno.Native.ImGui.ImGuiStyleVar.IndentSpacing = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L837) | 当前常量或枚举值：`16`。 | — |
| <a id="api-a33dd420e4d6165f"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ItemInnerSpacing = 15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L836) | 当前常量或枚举值：`15`。 | — |
| <a id="api-5f3ddaee458f6de7"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ItemSpacing = 14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L835) | 当前常量或枚举值：`14`。 | — |
| <a id="api-f22fd2591fe8948d"></a>[`Inno.Native.ImGui.ImGuiStyleVar.PopupBorderSize = 10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L831) | 当前常量或枚举值：`10`。 | — |
| <a id="api-f44119cef0700e94"></a>[`Inno.Native.ImGui.ImGuiStyleVar.PopupRounding = 9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L830) | 当前常量或枚举值：`9`。 | — |
| <a id="api-31265abf938800d9"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ScrollbarPadding = 20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L841) | 当前常量或枚举值：`20`。 | — |
| <a id="api-62d5adcfd404b473"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ScrollbarRounding = 19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L840) | 当前常量或枚举值：`19`。 | — |
| <a id="api-42760993743aa03b"></a>[`Inno.Native.ImGui.ImGuiStyleVar.ScrollbarSize = 18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L839) | 当前常量或枚举值：`18`。 | — |
| <a id="api-f56f7265999278f8"></a>[`Inno.Native.ImGui.ImGuiStyleVar.SelectableTextAlign = 36`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L857) | 当前常量或枚举值：`36`。 | — |
| <a id="api-19883bbd25510880"></a>[`Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextAlign = 38`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L859) | 当前常量或枚举值：`38`。 | — |
| <a id="api-500b80266597e861"></a>[`Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextBorderSize = 37`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L858) | 当前常量或枚举值：`37`。 | — |
| <a id="api-da6254d0c3deb9f1"></a>[`Inno.Native.ImGui.ImGuiStyleVar.SeparatorTextPadding = 39`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L860) | 当前常量或枚举值：`39`。 | — |
| <a id="api-f660e8d97ad0015b"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TabBarBorderSize = 29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L850) | 当前常量或枚举值：`29`。 | — |
| <a id="api-5717698e19c49113"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TabBarOverlineSize = 30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L851) | 当前常量或枚举值：`30`。 | — |
| <a id="api-17a13688cac42cde"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TabBorderSize = 26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L847) | 当前常量或枚举值：`26`。 | — |
| <a id="api-0957e1a17f57d09a"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TableAngledHeadersAngle = 31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L852) | 当前常量或枚举值：`31`。 | — |
| <a id="api-b0bd417bc68fe7ec"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TableAngledHeadersTextAlign = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L853) | 当前常量或枚举值：`32`。 | — |
| <a id="api-d59dd0f0b7c9894c"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TabMinWidthBase = 27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L848) | 当前常量或枚举值：`27`。 | — |
| <a id="api-2267e41608dab3c2"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TabMinWidthShrink = 28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L849) | 当前常量或枚举值：`28`。 | — |
| <a id="api-83f3b34009692680"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TabRounding = 25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L846) | 当前常量或枚举值：`25`。 | — |
| <a id="api-d44a5d3357a9013f"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TreeLinesRounding = 34`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L855) | 当前常量或枚举值：`34`。 | — |
| <a id="api-59a9ddff2c5424c8"></a>[`Inno.Native.ImGui.ImGuiStyleVar.TreeLinesSize = 33`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L854) | 当前常量或枚举值：`33`。 | — |
| <a id="api-3516af8c00967a88"></a>[`Inno.Native.ImGui.ImGuiStyleVar.WindowBorderSize = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L825) | 当前常量或枚举值：`4`。 | — |
| <a id="api-cf4848e9285f9ed7"></a>[`Inno.Native.ImGui.ImGuiStyleVar.WindowMinSize = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L826) | 当前常量或枚举值：`5`。 | — |
| <a id="api-b1cb7c8795e873c3"></a>[`Inno.Native.ImGui.ImGuiStyleVar.WindowPadding = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L823) | 当前常量或枚举值：`2`。 | — |
| <a id="api-110cde35a826c3af"></a>[`Inno.Native.ImGui.ImGuiStyleVar.WindowRounding = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L824) | 当前常量或枚举值：`3`。 | — |
| <a id="api-c64462a281251aca"></a>[`Inno.Native.ImGui.ImGuiStyleVar.WindowTitleAlign = 6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L827) | 当前常量或枚举值：`6`。 | — |
| <a id="api-1beabf816a999228"></a>[`public enum Inno.Native.ImGui.ImGuiStyleVar : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L819) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiStyleVarInfo</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStyleVarInfo dc47a50eaccf2c3943cc2a23f0c97077163541ad4efb30605dc698da95550209 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVarInfo.RawBits0_0 7a16f68346ba2442a4a8d8c80c7fba7334a0a275f7d1b4051767bce84b9df7e6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVarInfo.RawBits0_1 c67005bea6e4bf366aec738fd87033d448f97ec3c888121a3d66086110b41e20 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVarInfo.RawBits0_2 8ed390e8c048d379cfa0a78bb11544e1de07bc4b86b70029743be070f30929b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVarInfo.RawBits0_3 60df99eb09227e57d81c45ab82e1fabcd7e0ae3630b38a58c72cbf27f0316d0d -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleVarInfo.Count 59d5348e79d61a9276bd56ae8ebf67b72139bb031f4ab5defdb12af328e77730 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleVarInfo.DataType d374cf220eaa8b1dab6b23492d481b30ce4fb43b8c3652bbc384b4659c32b894 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleVarInfo.Offset 6a1f3780d9195f8409120a41f4a7ad0e5367d6cf17d7001cd62efbbe21979f72 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleVarInfo.#ctor(System.UInt32,Inno.Native.ImGui.ImGuiDataType,System.UInt32) 90a32b5ea6de4c0dfcfeb11a0e0a2869c5b2ef8be11a420d4f43d6ac6d726e02 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleVarInfo.GetVarPtr(System.Void*) bcbcf7be264f7798389d5006401aff6fbc55f92a7381c3f57f75e724130e3cac -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-92a60e7c479c8b42"></a>[`public byte Inno.Native.ImGui.ImGuiStyleVarInfo.RawBits0_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10242) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d3078bb627bc74f"></a>[`public byte Inno.Native.ImGui.ImGuiStyleVarInfo.RawBits0_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10244) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9fa103ad340afebe"></a>[`public byte Inno.Native.ImGui.ImGuiStyleVarInfo.RawBits0_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10246) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b6e0af9620f62f47"></a>[`public byte Inno.Native.ImGui.ImGuiStyleVarInfo.RawBits0_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10248) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a1a16a249f5f758"></a>[`public Inno.Native.ImGui.ImGuiStyleVarInfo.ImGuiStyleVarInfo(uint count = 0, Inno.Native.ImGui.ImGuiDataType dataType = Inno.Native.ImGui.ImGuiDataType.S8, uint offset = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10253) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-605046fa5540cef3"></a>[`public void* Inno.Native.ImGui.ImGuiStyleVarInfo.GetVarPtr(void* parent)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66416) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a63738b583699b81"></a>[`public uint Inno.Native.ImGui.ImGuiStyleVarInfo.Count { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10249) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d9efe403a0d4bea9"></a>[`public Inno.Native.ImGui.ImGuiDataType Inno.Native.ImGui.ImGuiStyleVarInfo.DataType { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10250) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d1c1c52313f5019e"></a>[`public uint Inno.Native.ImGui.ImGuiStyleVarInfo.Offset { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10251) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-37af52fbdc909902"></a>[`public struct Inno.Native.ImGui.ImGuiStyleVarInfo`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10239) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
