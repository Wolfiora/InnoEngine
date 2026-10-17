# Inno.Native.ImGui ABI · ImTextureRefPtr → WriteAllFn

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImTextureRefPtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImTextureRefPtr bc3f11f0b276994fbccb6c7953e174d83b89d99543c7760a997b5ed3ecf3914b -->
<!-- api: M:Inno.Native.ImGui.ImTextureRefPtr.#ctor(Inno.Native.ImGui.ImTextureRef*) e87d7a9a9f5ddaa131166606016f21627cf1609a5c2306252c4747bc5bc9830d -->
<!-- api: F:Inno.Native.ImGui.ImTextureRefPtr.Handle 26e33670295fecba36d506599116c6e08cd364ee07052b93e79136fb09940005 -->
<!-- api: P:Inno.Native.ImGui.ImTextureRefPtr.IsNull 63a2e2d815565b62e2c0e28107d22e9dd83de97a0b69235385436d776bd3e711 -->
<!-- api: P:Inno.Native.ImGui.ImTextureRefPtr.Null a70adb915af1198a71f8566609e573765f17bb54e8b4d91d8fb41737c07c58ce -->
<!-- api: P:Inno.Native.ImGui.ImTextureRefPtr.Item(System.Int32) 978e8dc0736e084b88b905e04b9186f847a37c57674e39a8bf6116849816c19e -->
<!-- api: M:Inno.Native.ImGui.ImTextureRefPtr.op_Implicit(Inno.Native.ImGui.ImTextureRef*)~Inno.Native.ImGui.ImTextureRefPtr b85d5f30a9345e69747359aa932a13ca6b946896973028e5cad2bada0cbf1517 -->
<!-- api: M:Inno.Native.ImGui.ImTextureRefPtr.op_Implicit(Inno.Native.ImGui.ImTextureRefPtr)~Inno.Native.ImGui.ImTextureRef* 0cd495e4bc8d15159fafe00e5cf642b70eeba7b3369a80d2b18fcef7d64d49bc -->
<!-- api: M:Inno.Native.ImGui.ImTextureRefPtr.op_Equality(Inno.Native.ImGui.ImTextureRefPtr,Inno.Native.ImGui.ImTextureRefPtr) 518caf316aa3e49a74e46b0a4c49470b4e185961e7e7de0f77bcbb1319af7b1c -->
<!-- api: M:Inno.Native.ImGui.ImTextureRefPtr.op_Inequality(Inno.Native.ImGui.ImTextureRefPtr,Inno.Native.ImGui.ImTextureRefPtr) e970dd043d3df478ce4e3d7ec6b1dbaba9f84752d75cb88d1c1f8a2659000b7c -->
<!-- api: M:Inno.Native.ImGui.ImTextureRefPtr.Equals(Inno.Native.ImGui.ImTextureRefPtr) ba9e570ac8edf619af3a37fb7091e41e7b748827ccc3f33061162dddcca306a8 -->
<!-- api: M:Inno.Native.ImGui.ImTextureRefPtr.Equals(System.Object) bda0374942824f5511670fbe9c846027e8e4da81ba66ca16ce6d96af75b2d9ca -->
<!-- api: M:Inno.Native.ImGui.ImTextureRefPtr.GetHashCode 610798bd0e87fc1e1d691a8a28aadb6e744ac99d582756d68bc03c857ec30e20 -->
<!-- api: P:Inno.Native.ImGui.ImTextureRefPtr.TexData d1c32242aa2a776af75cce90c90acdc18f08e13769703520e0689ceb306afab7 -->
<!-- api: P:Inno.Native.ImGui.ImTextureRefPtr.TexID b5dfbea0ef027f43b85f32f5e1321f5f193ec4bed2abbde686d7baf1012d6d1c -->
<!-- api: M:Inno.Native.ImGui.ImTextureRefPtr.Destroy 690fbca306f684012d5719602feb2cc49012549f04e1cb10b160137feeb18b2d -->
<!-- api: M:Inno.Native.ImGui.ImTextureRefPtr.GetTexID 0fcc92f87d768755ea2209282708cdb48e91d9a50c6d54cdea34cdfcb6ccc756 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5194e7aab80c967c"></a>[`public Inno.Native.ImGui.ImTextureRef* Inno.Native.ImGui.ImTextureRefPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18618) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b61db16c607707da"></a>[`public Inno.Native.ImGui.ImTextureRefPtr.ImTextureRefPtr(Inno.Native.ImGui.ImTextureRef* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18613) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-c1333b16350bd244"></a>[`public void Inno.Native.ImGui.ImTextureRefPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18638) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c03024a29fad60e6"></a>[`public bool Inno.Native.ImGui.ImTextureRefPtr.Equals(Inno.Native.ImGui.ImTextureRefPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18628) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-01408f75fd99d137"></a>[`public override bool Inno.Native.ImGui.ImTextureRefPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18629) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-23eca25b1282cea2"></a>[`public override int Inno.Native.ImGui.ImTextureRefPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18630) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-3d78c464b88693cc"></a>[`public Inno.Native.ImGui.ImTextureID Inno.Native.ImGui.ImTextureRefPtr.GetTexID()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18646) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d0580c81f1e95308"></a>[`public static bool Inno.Native.ImGui.ImTextureRefPtr.operator ==(Inno.Native.ImGui.ImTextureRefPtr left, Inno.Native.ImGui.ImTextureRefPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18626) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a265e6ad8ac6a5b4"></a>[`public static Inno.Native.ImGui.ImTextureRefPtr.implicit operator Inno.Native.ImGui.ImTextureRefPtr(Inno.Native.ImGui.ImTextureRef* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18624) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bd479178239cc671"></a>[`public static Inno.Native.ImGui.ImTextureRefPtr.implicit operator Inno.Native.ImGui.ImTextureRef*(Inno.Native.ImGui.ImTextureRefPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18625) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4108f4ef0efe438b"></a>[`public static bool Inno.Native.ImGui.ImTextureRefPtr.operator !=(Inno.Native.ImGui.ImTextureRefPtr left, Inno.Native.ImGui.ImTextureRefPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18627) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3dd66fa394c49231"></a>[`public bool Inno.Native.ImGui.ImTextureRefPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18619) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-710b04b55867fc2d"></a>[`public Inno.Native.ImGui.ImTextureRef Inno.Native.ImGui.ImTextureRefPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18622) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-752e28917f8ea8fe"></a>[`public static Inno.Native.ImGui.ImTextureRefPtr Inno.Native.ImGui.ImTextureRefPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18620) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b51c54f606b85508"></a>[`public ref Inno.Native.ImGui.ImTextureDataPtr Inno.Native.ImGui.ImTextureRefPtr.TexData { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18632) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aa5565227164b7ea"></a>[`public ref Inno.Native.ImGui.ImTextureID Inno.Native.ImGui.ImTextureRefPtr.TexID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18633) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-be0efa51dfecee94"></a>[`public struct Inno.Native.ImGui.ImTextureRefPtr : System.IEquatable<Inno.Native.ImGui.ImTextureRefPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18611) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImTextureStatus</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImTextureStatus 103fd2f7b02d6ff3984848514e23fa783d804627b5e1e8f594dd3e27fca0280e -->
<!-- api: F:Inno.Native.ImGui.ImTextureStatus.Ok 208d7af3a772aa800a148fae9f841b9c66cf35a9ecd06361b267745b2dc98185 -->
<!-- api: F:Inno.Native.ImGui.ImTextureStatus.Destroyed 714ca06adb7ae9181cbf3d6f1211d0a75215a29a57bd82ff1e1756fea402134d -->
<!-- api: F:Inno.Native.ImGui.ImTextureStatus.WantCreate bdeee4e4ea5653910a28d70f62397d90210aa24cf145ea294e1c7c73a27b1194 -->
<!-- api: F:Inno.Native.ImGui.ImTextureStatus.WantUpdates b9aeafdb2d185dfb44df485199b56a628d385702c6d0c07b54f4f17562c0118e -->
<!-- api: F:Inno.Native.ImGui.ImTextureStatus.WantDestroy c05d41c752a93cd6f59d9439a1293f0145269e3d97936d69a7000ef9fbfd7c4f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1e09815505bf9019"></a>[`Inno.Native.ImGui.ImTextureStatus.Destroyed = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L50) | 当前常量或枚举值：`1`。 | — |
| <a id="api-2c85b26e0385db90"></a>[`Inno.Native.ImGui.ImTextureStatus.Ok = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L49) | 当前常量或枚举值：`0`。 | — |
| <a id="api-72902f2cec2bdb98"></a>[`Inno.Native.ImGui.ImTextureStatus.WantCreate = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L51) | 当前常量或枚举值：`2`。 | — |
| <a id="api-995e504a100be63e"></a>[`Inno.Native.ImGui.ImTextureStatus.WantDestroy = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L53) | 当前常量或枚举值：`4`。 | — |
| <a id="api-9e0884ba037eeb26"></a>[`Inno.Native.ImGui.ImTextureStatus.WantUpdates = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L52) | 当前常量或枚举值：`3`。 | — |
| <a id="api-6f523dca9ef12d7f"></a>[`public enum Inno.Native.ImGui.ImTextureStatus : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L47) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImVec1</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImVec1 dab357589108e61b92dec2698507877cf8a2ff01e4b914db2bde60a4a0cce1c6 -->
<!-- api: F:Inno.Native.ImGui.ImVec1.X 8ae2a830ed9c7bf77a3de4bfbfb5a3e311e3b97e89b0e7741f0c915c2a502fed -->
<!-- api: M:Inno.Native.ImGui.ImVec1.#ctor(System.Single) 37a540e9b27482818f7dc21e02ea4c8bc91749caf20fbe5cf95237aebc3422ee -->
<!-- api: M:Inno.Native.ImGui.ImVec1.Destroy 1b58d0f40641de86dbbe53cf734b8085d1d7416ece65e463743054397817745d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b5f1c57538afcf8b"></a>[`public float Inno.Native.ImGui.ImVec1.X`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4042) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e59916a41091d3a8"></a>[`public Inno.Native.ImGui.ImVec1.ImVec1(float x = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4043) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ab5741021d581e37"></a>[`public void Inno.Native.ImGui.ImVec1.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65944) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ff2e981b9edffc0c"></a>[`public struct Inno.Native.ImGui.ImVec1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4040) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImVec1Ptr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImVec1Ptr 88df94049373c803dee76db56d2f1da45ccc7ad3807d5b5dcc807290b19c1515 -->
<!-- api: M:Inno.Native.ImGui.ImVec1Ptr.#ctor(Inno.Native.ImGui.ImVec1*) 4727c42d4b233e506b930a6e37d7e3a82ea0db608c19f29535a64d646f57fc25 -->
<!-- api: F:Inno.Native.ImGui.ImVec1Ptr.Handle 1b297e6bbdff77a85a780803727ec38e72eec170932d69aa61af6a6e48772532 -->
<!-- api: P:Inno.Native.ImGui.ImVec1Ptr.IsNull a2c85b83d874c128e7a2791a0e2d10a829f7c4de6dd127f6fa7ff17d2053cb00 -->
<!-- api: P:Inno.Native.ImGui.ImVec1Ptr.Null 5cac4c1ecc37caeab024609644b0fadbb155d00863bc8e6a7ef47ee1220b1d1a -->
<!-- api: P:Inno.Native.ImGui.ImVec1Ptr.Item(System.Int32) 238cb9f519de5a8b505c15c709fdbc9896b12077eab2cca561206f3c89825418 -->
<!-- api: M:Inno.Native.ImGui.ImVec1Ptr.op_Implicit(Inno.Native.ImGui.ImVec1*)~Inno.Native.ImGui.ImVec1Ptr c5f4aab311c423cc5df28010af8edd0036860dd3a0706fed759fe279c605bfae -->
<!-- api: M:Inno.Native.ImGui.ImVec1Ptr.op_Implicit(Inno.Native.ImGui.ImVec1Ptr)~Inno.Native.ImGui.ImVec1* 1a3474d93c8ac425ebb87dca0eb8aec80917eead5c34bac45f288f47529877ca -->
<!-- api: M:Inno.Native.ImGui.ImVec1Ptr.op_Equality(Inno.Native.ImGui.ImVec1Ptr,Inno.Native.ImGui.ImVec1Ptr) 070d63419f255c2da0c4987b40947843f9f99b393a865da3f4c4661ba89b2308 -->
<!-- api: M:Inno.Native.ImGui.ImVec1Ptr.op_Inequality(Inno.Native.ImGui.ImVec1Ptr,Inno.Native.ImGui.ImVec1Ptr) 4c0cfefb0b6b28d71d823aa2ab7fa3ba7ff7069cfb878392e06467024c9b9327 -->
<!-- api: M:Inno.Native.ImGui.ImVec1Ptr.Equals(Inno.Native.ImGui.ImVec1Ptr) 4a8ccf6a27d09616cb22309af3b46b22de5221301173dbd718df0721d1f41e4b -->
<!-- api: M:Inno.Native.ImGui.ImVec1Ptr.Equals(System.Object) 4470d83837c4a4909c8002932a23043a8bf0729da7b8348a640da542531013a5 -->
<!-- api: M:Inno.Native.ImGui.ImVec1Ptr.GetHashCode 68d09b22416b83377e0144184101e7f26c808b8ac4927246846c59442bad0544 -->
<!-- api: P:Inno.Native.ImGui.ImVec1Ptr.X 2bba16db87806cf19167599135e7aae3494ee91da34ba0e686f7cc3780628a91 -->
<!-- api: M:Inno.Native.ImGui.ImVec1Ptr.Destroy 0752c9eafa2e98149eafc3ceb29ba2ab142a40c1b619977bdb3cb2bfba4faff9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-cba4f53c2606d827"></a>[`public Inno.Native.ImGui.ImVec1* Inno.Native.ImGui.ImVec1Ptr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18660) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2b8fa05efaf8c7a"></a>[`public Inno.Native.ImGui.ImVec1Ptr.ImVec1Ptr(Inno.Native.ImGui.ImVec1* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18655) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-aa99a9aee041a1dc"></a>[`public void Inno.Native.ImGui.ImVec1Ptr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18679) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b013fdd3a75ec11e"></a>[`public bool Inno.Native.ImGui.ImVec1Ptr.Equals(Inno.Native.ImGui.ImVec1Ptr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18670) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-a1727e37ea5be3eb"></a>[`public override bool Inno.Native.ImGui.ImVec1Ptr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18671) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-06a7877b64dbd91c"></a>[`public override int Inno.Native.ImGui.ImVec1Ptr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18672) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-7fe29272ed23642d"></a>[`public static bool Inno.Native.ImGui.ImVec1Ptr.operator ==(Inno.Native.ImGui.ImVec1Ptr left, Inno.Native.ImGui.ImVec1Ptr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18668) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-56317161538e3f27"></a>[`public static Inno.Native.ImGui.ImVec1Ptr.implicit operator Inno.Native.ImGui.ImVec1Ptr(Inno.Native.ImGui.ImVec1* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18666) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-45311a55e77542f0"></a>[`public static Inno.Native.ImGui.ImVec1Ptr.implicit operator Inno.Native.ImGui.ImVec1*(Inno.Native.ImGui.ImVec1Ptr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18667) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ddeb0fc135d65c52"></a>[`public static bool Inno.Native.ImGui.ImVec1Ptr.operator !=(Inno.Native.ImGui.ImVec1Ptr left, Inno.Native.ImGui.ImVec1Ptr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18669) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d918979a505d930c"></a>[`public bool Inno.Native.ImGui.ImVec1Ptr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18661) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2c6d56afb83deb2d"></a>[`public Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImVec1Ptr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18664) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c30e432fc1da5d5f"></a>[`public static Inno.Native.ImGui.ImVec1Ptr Inno.Native.ImGui.ImVec1Ptr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18662) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2c93e50c9ac59c36"></a>[`public ref float Inno.Native.ImGui.ImVec1Ptr.X { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18674) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-198166034121c2a2"></a>[`public struct Inno.Native.ImGui.ImVec1Ptr : System.IEquatable<Inno.Native.ImGui.ImVec1Ptr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18653) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImVec2IC</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImVec2IC a519815287114f49f441a9c6d3c15294686b960629966312f0b81f42df48e395 -->
<!-- api: F:Inno.Native.ImGui.ImVec2IC.X 8d46a0378453c86c05b2bec23c61cf555baa21ece286534df832961a117e5f3e -->
<!-- api: F:Inno.Native.ImGui.ImVec2IC.Y 3ed123f2f338dff278beab3e6367b6ab7d9cfc703dacf8b3267643645bc17d1a -->
<!-- api: M:Inno.Native.ImGui.ImVec2IC.#ctor(System.Int32,System.Int32) 2a79dfd44d63e49f51daf15b920d0217a3c5fb38aab55b6d4f3e6fa0d1622665 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-298d2082024f6dc8"></a>[`public int Inno.Native.ImGui.ImVec2IC.X`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2406) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eb86d04069c0ab18"></a>[`public int Inno.Native.ImGui.ImVec2IC.Y`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2407) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dbdcce0bb7c05d1d"></a>[`public Inno.Native.ImGui.ImVec2IC.ImVec2IC(int x = 0, int y = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2408) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-afa220a717074aef"></a>[`public struct Inno.Native.ImGui.ImVec2IC`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2404) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImVec2ICPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImVec2ICPtr 23114df87715d55f278709c3eb9496331c8362f84f304d2f5ef2d64d78c21562 -->
<!-- api: M:Inno.Native.ImGui.ImVec2ICPtr.#ctor(Inno.Native.ImGui.ImVec2IC*) 923b55cb11530a48c52c5b069c1043b7ce88fada9ba8c4e0928c93c2249e9192 -->
<!-- api: F:Inno.Native.ImGui.ImVec2ICPtr.Handle 6e0fd6bdb0f2703a625102506575f7045f6e9f2cadd483635c6b02040f109a93 -->
<!-- api: P:Inno.Native.ImGui.ImVec2ICPtr.IsNull bdb81ffb473e6677cee7dea68096e295842e4f76694bea49ed515c866a261d79 -->
<!-- api: P:Inno.Native.ImGui.ImVec2ICPtr.Null 12833f030d0b504838391a329bd948fc8e074959e216c4fc463c26d6c11ae24b -->
<!-- api: P:Inno.Native.ImGui.ImVec2ICPtr.Item(System.Int32) b7965e57f1b31dc3c180661e7751c617064e1172e770fb2b0a6a9d9f9f54419e -->
<!-- api: M:Inno.Native.ImGui.ImVec2ICPtr.op_Implicit(Inno.Native.ImGui.ImVec2IC*)~Inno.Native.ImGui.ImVec2ICPtr 5cdd43f567455294e794ed45183d143b4da2179a82dc8eace6a362ba3e19ed9c -->
<!-- api: M:Inno.Native.ImGui.ImVec2ICPtr.op_Implicit(Inno.Native.ImGui.ImVec2ICPtr)~Inno.Native.ImGui.ImVec2IC* 45b69922d29012c45ca7175270ec4d633edaa01af80e049742a75510179fb2cc -->
<!-- api: M:Inno.Native.ImGui.ImVec2ICPtr.op_Equality(Inno.Native.ImGui.ImVec2ICPtr,Inno.Native.ImGui.ImVec2ICPtr) 37b567d59616a741d48e64fc69f943ace49eaf713383301231c4f3695e309747 -->
<!-- api: M:Inno.Native.ImGui.ImVec2ICPtr.op_Inequality(Inno.Native.ImGui.ImVec2ICPtr,Inno.Native.ImGui.ImVec2ICPtr) 55f8ea8631dfc06a317b785c3f739c1cdacf8ba6e83b4132cc7a904768d38f15 -->
<!-- api: M:Inno.Native.ImGui.ImVec2ICPtr.Equals(Inno.Native.ImGui.ImVec2ICPtr) 97eee4768639f60da1e13af3e2de28fad7232e3da017cbe0e297d066f2102090 -->
<!-- api: M:Inno.Native.ImGui.ImVec2ICPtr.Equals(System.Object) e305aa053ef5e912a6c7b8aa1c8c4509ad4ce356d717c361e0056a52b6e908ec -->
<!-- api: M:Inno.Native.ImGui.ImVec2ICPtr.GetHashCode 2b94e083c9d1d6cf8716a6d69d645cf7e2e47ea4bbf6b8cf1d71828c589020c1 -->
<!-- api: P:Inno.Native.ImGui.ImVec2ICPtr.X 94ce7847e7bb12df0a1897ef0e51612c1544eee756a80e5689450df7d1b0d111 -->
<!-- api: P:Inno.Native.ImGui.ImVec2ICPtr.Y 35a0fe1f129e8a138063858dfaaf987e73e1b73d2862757dd1f1ec165b53b2fc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-392b4f90147bd627"></a>[`public Inno.Native.ImGui.ImVec2IC* Inno.Native.ImGui.ImVec2ICPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18693) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c5477fbfa2554d7"></a>[`public Inno.Native.ImGui.ImVec2ICPtr.ImVec2ICPtr(Inno.Native.ImGui.ImVec2IC* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18688) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2015e8262033ec65"></a>[`public bool Inno.Native.ImGui.ImVec2ICPtr.Equals(Inno.Native.ImGui.ImVec2ICPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18703) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-f5857172b54542ef"></a>[`public override bool Inno.Native.ImGui.ImVec2ICPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18704) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-21a091ad45173cb0"></a>[`public override int Inno.Native.ImGui.ImVec2ICPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18705) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-c45e1cc781b64906"></a>[`public static bool Inno.Native.ImGui.ImVec2ICPtr.operator ==(Inno.Native.ImGui.ImVec2ICPtr left, Inno.Native.ImGui.ImVec2ICPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18701) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a3fe933372b56c23"></a>[`public static Inno.Native.ImGui.ImVec2ICPtr.implicit operator Inno.Native.ImGui.ImVec2ICPtr(Inno.Native.ImGui.ImVec2IC* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18699) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7057e584971819d1"></a>[`public static Inno.Native.ImGui.ImVec2ICPtr.implicit operator Inno.Native.ImGui.ImVec2IC*(Inno.Native.ImGui.ImVec2ICPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18700) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d62e5582d4b4ccc6"></a>[`public static bool Inno.Native.ImGui.ImVec2ICPtr.operator !=(Inno.Native.ImGui.ImVec2ICPtr left, Inno.Native.ImGui.ImVec2ICPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18702) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e7f270fb2e380810"></a>[`public bool Inno.Native.ImGui.ImVec2ICPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18694) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-92f47e396cb9a670"></a>[`public Inno.Native.ImGui.ImVec2IC Inno.Native.ImGui.ImVec2ICPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18697) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b90fbcf81f644156"></a>[`public static Inno.Native.ImGui.ImVec2ICPtr Inno.Native.ImGui.ImVec2ICPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18695) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-01d5444f685569d9"></a>[`public ref int Inno.Native.ImGui.ImVec2ICPtr.X { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18707) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cf225b1d33604549"></a>[`public ref int Inno.Native.ImGui.ImVec2ICPtr.Y { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18708) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ec4ed2d24319f19e"></a>[`public struct Inno.Native.ImGui.ImVec2ICPtr : System.IEquatable<Inno.Native.ImGui.ImVec2ICPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18686) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImVec2Ih</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImVec2Ih b2d230fd3627ae9c15f2d19479c09a442fa508c6ab286a40009400da575b0ca5 -->
<!-- api: F:Inno.Native.ImGui.ImVec2Ih.X d9d5e7c3a4f9fb1d2eb853579f36bd3610dde9d9c1c341afd4008d62325ca2f3 -->
<!-- api: F:Inno.Native.ImGui.ImVec2Ih.Y bd0ab4264d95df4bf1468d4cd865c946db8917880d89027ce564d04ea16169cd -->
<!-- api: M:Inno.Native.ImGui.ImVec2Ih.#ctor(System.Int16,System.Int16) 180e9b73e78822a0f27db766732e234a9d1e6a10bc9252ae90b71fcfa67fe83f -->
<!-- api: M:Inno.Native.ImGui.ImVec2Ih.Destroy 39c99c9a9bf449ccee935451c9c03c46654ab6416ce7d90e1ac5017daaf0d398 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7a81333d405d63c1"></a>[`public short Inno.Native.ImGui.ImVec2Ih.X`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4286) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d70f5ec67572e4f5"></a>[`public short Inno.Native.ImGui.ImVec2Ih.Y`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4287) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bbdf00568b47d9cb"></a>[`public Inno.Native.ImGui.ImVec2Ih.ImVec2Ih(short x = 0, short y = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4288) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a9587ead7e458322"></a>[`public void Inno.Native.ImGui.ImVec2Ih.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65958) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5cee083e1ce7d6cb"></a>[`public struct Inno.Native.ImGui.ImVec2Ih`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4284) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImVec2IhPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImVec2IhPtr 6a80a1a2c8369d3a00122e544a2941120cdaeeb3b1232dabedb6b5d41fab8cd0 -->
<!-- api: M:Inno.Native.ImGui.ImVec2IhPtr.#ctor(Inno.Native.ImGui.ImVec2Ih*) 5b656f87dc1aa7774ee98432897904a53a48a22f24dd3cf80c90e4e1460b9f69 -->
<!-- api: F:Inno.Native.ImGui.ImVec2IhPtr.Handle 8c0f0c189acf381ca66c95d68d13a4dee08cd0f0c5894d3a7545c7abd975dace -->
<!-- api: P:Inno.Native.ImGui.ImVec2IhPtr.IsNull 9ea8c5a342eb30166f821bcf1731a84f9573afa50009d385a2f007717a9c916d -->
<!-- api: P:Inno.Native.ImGui.ImVec2IhPtr.Null 56cf5aefe2eb8593486a2e0cd8e020ad8ef12524b36cf155c076adba75f23ebb -->
<!-- api: P:Inno.Native.ImGui.ImVec2IhPtr.Item(System.Int32) fd5691035e74674acd985b8e11badb0b939185394197d9dd06622f14480115f0 -->
<!-- api: M:Inno.Native.ImGui.ImVec2IhPtr.op_Implicit(Inno.Native.ImGui.ImVec2Ih*)~Inno.Native.ImGui.ImVec2IhPtr 1b3bd89abea4c3023a0d165de0280312dc02efefe4908cd6988e1910fae15db6 -->
<!-- api: M:Inno.Native.ImGui.ImVec2IhPtr.op_Implicit(Inno.Native.ImGui.ImVec2IhPtr)~Inno.Native.ImGui.ImVec2Ih* 5c34d8a858465d73596744fb8148ce08df39878ffa4279e520b0e22fcfc52969 -->
<!-- api: M:Inno.Native.ImGui.ImVec2IhPtr.op_Equality(Inno.Native.ImGui.ImVec2IhPtr,Inno.Native.ImGui.ImVec2IhPtr) 45ff0f08654642d7a370280b81f3ccc618e3919c27ef1cb39d900d301f399d5b -->
<!-- api: M:Inno.Native.ImGui.ImVec2IhPtr.op_Inequality(Inno.Native.ImGui.ImVec2IhPtr,Inno.Native.ImGui.ImVec2IhPtr) 4d3ec02818992d17ef9c08a60d9fb97e859827e2c8a56cf4025f0fd3148f59fb -->
<!-- api: M:Inno.Native.ImGui.ImVec2IhPtr.Equals(Inno.Native.ImGui.ImVec2IhPtr) f06425224523261ec8d7a6ea6326b427efd3884c312f804ef5f47e1e05f47d03 -->
<!-- api: M:Inno.Native.ImGui.ImVec2IhPtr.Equals(System.Object) 4a3a16962f33474afbfc6fe194fd784f2f886a636d255a3f21909d840d6bca4b -->
<!-- api: M:Inno.Native.ImGui.ImVec2IhPtr.GetHashCode 799de0272e6a3f6fb915e78315cd7d1bc2ea4d806cbf81878bb22f3df3983c4e -->
<!-- api: P:Inno.Native.ImGui.ImVec2IhPtr.X 1d0d74e0ad8ae2893857684a3feed720602f2d7641373f78c8edb9ece34275d2 -->
<!-- api: P:Inno.Native.ImGui.ImVec2IhPtr.Y 90e9fb2c4705f951c257fbb99b2dcd5a4dacd7b6fe9cc5f6a60a169b53adb3ba -->
<!-- api: M:Inno.Native.ImGui.ImVec2IhPtr.Destroy ce67d8595f40f833af04a83eb36c09110e4d1f9712d5e2fcef94723f897dfca9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fe5341d46a2fe0f7"></a>[`public Inno.Native.ImGui.ImVec2Ih* Inno.Native.ImGui.ImVec2IhPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18719) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bbd6c9e5ccebddc6"></a>[`public Inno.Native.ImGui.ImVec2IhPtr.ImVec2IhPtr(Inno.Native.ImGui.ImVec2Ih* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18714) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-65f19713c2c46e43"></a>[`public void Inno.Native.ImGui.ImVec2IhPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18739) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-290d348ce7bb17d7"></a>[`public bool Inno.Native.ImGui.ImVec2IhPtr.Equals(Inno.Native.ImGui.ImVec2IhPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18729) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-c6fe0511d91e1893"></a>[`public override bool Inno.Native.ImGui.ImVec2IhPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18730) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-1ed8d26304c849c8"></a>[`public override int Inno.Native.ImGui.ImVec2IhPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18731) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-018849aaad43c468"></a>[`public static bool Inno.Native.ImGui.ImVec2IhPtr.operator ==(Inno.Native.ImGui.ImVec2IhPtr left, Inno.Native.ImGui.ImVec2IhPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18727) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-beef98beca1ee8e1"></a>[`public static Inno.Native.ImGui.ImVec2IhPtr.implicit operator Inno.Native.ImGui.ImVec2IhPtr(Inno.Native.ImGui.ImVec2Ih* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18725) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-287e8b9a20363876"></a>[`public static Inno.Native.ImGui.ImVec2IhPtr.implicit operator Inno.Native.ImGui.ImVec2Ih*(Inno.Native.ImGui.ImVec2IhPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18726) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b3f36c176a39fa40"></a>[`public static bool Inno.Native.ImGui.ImVec2IhPtr.operator !=(Inno.Native.ImGui.ImVec2IhPtr left, Inno.Native.ImGui.ImVec2IhPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18728) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-41d6c101bf34df28"></a>[`public bool Inno.Native.ImGui.ImVec2IhPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18720) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9d9af4bb37d6421b"></a>[`public Inno.Native.ImGui.ImVec2Ih Inno.Native.ImGui.ImVec2IhPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18723) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-73fa52aff3b242dd"></a>[`public static Inno.Native.ImGui.ImVec2IhPtr Inno.Native.ImGui.ImVec2IhPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18721) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-da7c219b46ac139e"></a>[`public ref short Inno.Native.ImGui.ImVec2IhPtr.X { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18733) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a0e1eb83eab81f68"></a>[`public ref short Inno.Native.ImGui.ImVec2IhPtr.Y { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18734) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ea53e4828f1171ce"></a>[`public struct Inno.Native.ImGui.ImVec2IhPtr : System.IEquatable<Inno.Native.ImGui.ImVec2IhPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18712) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImVector`1</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImVector`1 2ee7d57b9262aa522adf3a374be92e0d5098ad8d92138d91690ec1ce60ab08c1 -->
<!-- api: M:Inno.Native.ImGui.ImVector`1.#ctor(System.Int32,System.Int32,`0*) 29affe4dd4139ebb8d8fa09582ba82965dca3e514d1d9ba58aace9a97ba6bb3f -->
<!-- api: P:Inno.Native.ImGui.ImVector`1.Item(System.Int32) 1087725b00c739fa5285af01c43f7934d4496f3fcf2271aa75008015155dd2bf -->
<!-- api: P:Inno.Native.ImGui.ImVector`1.Data ca07abd9a9b6bb948ffb31ecdad10de373f803628e9745f335fc283111c8be96 -->
<!-- api: P:Inno.Native.ImGui.ImVector`1.Front 95e5efc1e8f34494b2ae4efccf242c070e17644df3f15cecf5f8331a1d2a6367 -->
<!-- api: P:Inno.Native.ImGui.ImVector`1.Back eaab6566e3a05202c7daa952daed7968037197a60f30261a12a048e2a5492a1d -->
<!-- api: P:Inno.Native.ImGui.ImVector`1.Capacity 5178569cbc0704614301dc58f94fc083dbc8e25e223d385834c609c119208842 -->
<!-- api: P:Inno.Native.ImGui.ImVector`1.Size dbe1c34ddc9fbb529f5f3683463a3e796e426a2cf31eb36f451c492e03d08a5a -->
<!-- api: M:Inno.Native.ImGui.ImVector`1.Grow(System.Int32) 7c25026fcff4d3ddff3d859b2efe8edef54113d023f06d58503e363da9df6c9e -->
<!-- api: M:Inno.Native.ImGui.ImVector`1.EnsureCapacity(System.Int32) 48b941daa510658d0b6bf55bc9aff957389143d61be24c10c01312823fef07f6 -->
<!-- api: M:Inno.Native.ImGui.ImVector`1.Resize(System.Int32) 0afe8cb2e636863e343ec36c71cffaf29733a5f43bf962e84c326f5e94b6fb8b -->
<!-- api: M:Inno.Native.ImGui.ImVector`1.Clear 853aca708fdec7cc50637a1829c59b9a79a01c62b9daf91db6b46885861b8a45 -->
<!-- api: M:Inno.Native.ImGui.ImVector`1.PushBack(`0) 3e47985abb20b4bc3b19322a7f6d33ad7a6fbcc0fff7973054b5cc7438f14913 -->
<!-- api: M:Inno.Native.ImGui.ImVector`1.PopBack 55e3a2c0112d9b3eec0d3da37f53dff7c8be9d239ca937d60b1f8aed7477791d -->
<!-- api: M:Inno.Native.ImGui.ImVector`1.Free 6c3bcb6dcf3ad9388ccb18d0913a7d8532facfd4b6956bd0ba6e1c5d98978483 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-429da93dc02b5ae7"></a>[`public Inno.Native.ImGui.ImVector<T>.ImVector(int size, int capacity, T* data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69359) | Initializes a new instance of the struct with the specified size, capacity, and data pointer. | — |
| <a id="api-242010ca4a5c818e"></a>[`public void Inno.Native.ImGui.ImVector<T>.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69516) | Clears all elements from the vector. | — |
| <a id="api-a98e2cc7ab0a2b42"></a>[`public void Inno.Native.ImGui.ImVector<T>.EnsureCapacity(int size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69491) | Ensures that the vector has at least the specified capacity. | — |
| <a id="api-e271b4872177d5b6"></a>[`public void Inno.Native.ImGui.ImVector<T>.Free()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69547) | Frees the memory allocated for the vector. | — |
| <a id="api-82f1579cfa4a0fd7"></a>[`public void Inno.Native.ImGui.ImVector<T>.Grow(int newCapacity)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69476) | Grows the capacity of the vector to at least the specified value. | — |
| <a id="api-5c5343bf917b90ce"></a>[`public void Inno.Native.ImGui.ImVector<T>.PopBack()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69536) | Removes the last element from the vector. | — |
| <a id="api-740deb21a5fe6552"></a>[`public void Inno.Native.ImGui.ImVector<T>.PushBack(T value)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69527) | Adds an element to the end of the vector. | — |
| <a id="api-8d92a61ff73ed67c"></a>[`public void Inno.Native.ImGui.ImVector<T>.Resize(int newSize)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69506) | Resizes the vector to the specified size. | — |
| <a id="api-f2472409b76713ad"></a>[`public readonly T* Inno.Native.ImGui.ImVector<T>.Back { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69415) | Gets a pointer to the last element of the vector. | — |
| <a id="api-5276bb5449cd0531"></a>[`public int Inno.Native.ImGui.ImVector<T>.Capacity { readonly get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69420) | Gets or sets the capacity of the vector. | — |
| <a id="api-871d16a590c5885a"></a>[`public readonly T* Inno.Native.ImGui.ImVector<T>.Data { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69407) | Gets a pointer to the first element of the vector. | — |
| <a id="api-54a33e67f7ac52c7"></a>[`public readonly T* Inno.Native.ImGui.ImVector<T>.Front { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69411) | Gets a pointer to the first element of the vector. | — |
| <a id="api-2f2db518841a8e61"></a>[`public T Inno.Native.ImGui.ImVector<T>.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69381) | Gets or sets the element at the specified index.<br>返回：The element at the specified index. | — |
| <a id="api-f6c22805f1785512"></a>[`public readonly int Inno.Native.ImGui.ImVector<T>.Size { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69468) | Gets the number of elements in the vector. | — |
| <a id="api-9c273fce8847ede3"></a>[`public struct Inno.Native.ImGui.ImVector<T> where T : unmanaged`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69344) | A structure representing the native ImVector layout for unmanaged types. | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImWcharClass</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImWcharClass 65eed81942580ef4bacd8093e7ce4baf4b25ebf8c43e7786f3cc997713c9980f -->
<!-- api: F:Inno.Native.ImGui.ImWcharClass.Blank 20af221030b49fb9933d887e4c26650240f084bfa809dc2d9565f74ce02a953c -->
<!-- api: F:Inno.Native.ImGui.ImWcharClass.Punct 98ed970deed84cb73ee84c81cadc211fb4a2b4a62aa3b8ef9a0f00f91eca8af1 -->
<!-- api: F:Inno.Native.ImGui.ImWcharClass.Other f20f209903a990c9ba0c3f273fbcc99cf7c5ebf982e792586e5a8d39fa5cdaf1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b4554012107f3c99"></a>[`Inno.Native.ImGui.ImWcharClass.Blank = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1204) | 当前常量或枚举值：`0`。 | — |
| <a id="api-746bf3900189018d"></a>[`Inno.Native.ImGui.ImWcharClass.Other = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1206) | 当前常量或枚举值：`2`。 | — |
| <a id="api-22d71853a5915df9"></a>[`Inno.Native.ImGui.ImWcharClass.Punct = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1205) | 当前常量或枚举值：`1`。 | — |
| <a id="api-552d61c05810c504"></a>[`public enum Inno.Native.ImGui.ImWcharClass : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1202) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.Iobuf</code> · 1 个声明</summary>

<!-- api: T:Inno.Native.ImGui.Iobuf b89a3852f8cfe55ea37a0859ba8d7efc23f5abf25a83d9b6e437ac05d579d50f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e6657f42001b91f6"></a>[`public struct Inno.Native.ImGui.Iobuf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10382) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.LoaderInit</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.LoaderInit 4a6a3d8866b55d68b4138b0216456758f914d90417b3a80dd31e6502d9591069 -->
<!-- api: M:Inno.Native.ImGui.LoaderInit.#ctor(System.Object,System.IntPtr) ff2997aae0a97376b023d29d2bc82307c03367f6cb6d27297d3346802be2d476 -->
<!-- api: M:Inno.Native.ImGui.LoaderInit.Invoke(Inno.Native.ImGui.ImFontAtlas*) dd55e075ffb173402a937822c781d4ba560cfd8b6aaa4f98ab1a9005b63036c7 -->
<!-- api: M:Inno.Native.ImGui.LoaderInit.BeginInvoke(Inno.Native.ImGui.ImFontAtlas*,System.AsyncCallback,System.Object) 73aa70a5e9b3243d1a83fecc470b558cc0ce3d70c0a41b9aecdd34dd2887b7ee -->
<!-- api: M:Inno.Native.ImGui.LoaderInit.EndInvoke(System.IAsyncResult) 4eaad9b8d8ececd698bf8ca22642b4973c0adcf767637227b5e83de6d979232c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e396433b730693d5"></a>[`public Inno.Native.ImGui.LoaderInit.LoaderInit(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10393) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9699e5228884fa77"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.LoaderInit.BeginInvoke(Inno.Native.ImGui.ImFontAtlas* atlas, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10393) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ca18d7780053c75f"></a>[`public virtual byte Inno.Native.ImGui.LoaderInit.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10393) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6fe207d7225a43c5"></a>[`public virtual byte Inno.Native.ImGui.LoaderInit.Invoke(Inno.Native.ImGui.ImFontAtlas* atlas)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10393) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e5a5175e43552f9e"></a>[`public delegate byte Inno.Native.ImGui.LoaderInit(Inno.Native.ImGui.ImFontAtlas* atlas)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10393) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.LoaderShutdown</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.LoaderShutdown 7e2b5fd616d2fec0342a2b9bec3f800f44e323ba23b57e6296b83aab1ab7e15b -->
<!-- api: M:Inno.Native.ImGui.LoaderShutdown.#ctor(System.Object,System.IntPtr) 325ea083bbcf7c96cdd016197fdd35d17b32c06a22c83c657bd04f7673e8ad9e -->
<!-- api: M:Inno.Native.ImGui.LoaderShutdown.Invoke(Inno.Native.ImGui.ImFontAtlas*) 0cc590910bd9dae19cf4fe0548e2de0ec3ea4305385d719ece8e7a787a5274da -->
<!-- api: M:Inno.Native.ImGui.LoaderShutdown.BeginInvoke(Inno.Native.ImGui.ImFontAtlas*,System.AsyncCallback,System.Object) 7175d286dd35869f17a8f83e97a024e5e5da4d7753bb4c2b3543f9ae19f96f29 -->
<!-- api: M:Inno.Native.ImGui.LoaderShutdown.EndInvoke(System.IAsyncResult) c179e12f0fbb636cb7fa84b1152ec4a8fd3931b1e6ba5ef5259dd5d858f8da75 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9004120f394d9141"></a>[`public Inno.Native.ImGui.LoaderShutdown.LoaderShutdown(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10398) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-92ab49ba6c9d9c0e"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.LoaderShutdown.BeginInvoke(Inno.Native.ImGui.ImFontAtlas* atlas, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10398) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cb5defea0aa98282"></a>[`public virtual void Inno.Native.ImGui.LoaderShutdown.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10398) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b26639118bf97dbb"></a>[`public virtual void Inno.Native.ImGui.LoaderShutdown.Invoke(Inno.Native.ImGui.ImFontAtlas* atlas)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10398) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-04f89d5b7e5c8af1"></a>[`public delegate void Inno.Native.ImGui.LoaderShutdown(Inno.Native.ImGui.ImFontAtlas* atlas)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10398) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformCreateVkSurface</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformCreateVkSurface 06c1f3c9e4ccd351e92afd36124816c70038cd2189e397a95e382ebe96e7807a -->
<!-- api: M:Inno.Native.ImGui.PlatformCreateVkSurface.#ctor(System.Object,System.IntPtr) c1a13b0d3f1c0f82a6cd1e8769c51f5e381f6148b8e7e3ac0b97655cce60f4b0 -->
<!-- api: M:Inno.Native.ImGui.PlatformCreateVkSurface.Invoke(Inno.Native.ImGui.ImGuiViewport*,System.UInt64,System.Void*,System.UInt64*) 8721af81a61680c59f33505e9779d5c08e8aa43e78809623cc99fe74c81aca71 -->
<!-- api: M:Inno.Native.ImGui.PlatformCreateVkSurface.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.UInt64,System.Void*,System.UInt64*,System.AsyncCallback,System.Object) c2afb0f12adce67da7b8bce34812243255c2d7c9984e25c9b8a1de5f7688e056 -->
<!-- api: M:Inno.Native.ImGui.PlatformCreateVkSurface.EndInvoke(System.IAsyncResult) 4f36c0d3a2ef52e1cbb8f674b31ac4a799965e41bda9d23b5b094eed0e071f91 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-04f81108086b2cd8"></a>[`public Inno.Native.ImGui.PlatformCreateVkSurface.PlatformCreateVkSurface(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10548) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-c106ca14f03ea4f2"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformCreateVkSurface.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, ulong vkInst, void* vkAllocators, ulong* outVkSurface, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10548) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-90329155344f2a24"></a>[`public virtual int Inno.Native.ImGui.PlatformCreateVkSurface.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10548) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b8aa4337efc76149"></a>[`public virtual int Inno.Native.ImGui.PlatformCreateVkSurface.Invoke(Inno.Native.ImGui.ImGuiViewport* vp, ulong vkInst, void* vkAllocators, ulong* outVkSurface)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10548) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cd9be68038c45d62"></a>[`public delegate int Inno.Native.ImGui.PlatformCreateVkSurface(Inno.Native.ImGui.ImGuiViewport* vp, ulong vkInst, void* vkAllocators, ulong* outVkSurface)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10548) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformCreateWindow</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformCreateWindow f34d30b6377042d2984a34d16a18cfa953bbe7cb2afc24eff3fdbbae9acfb57b -->
<!-- api: M:Inno.Native.ImGui.PlatformCreateWindow.#ctor(System.Object,System.IntPtr) 6dab1f5dc8b33ef764b3b5b9495fa6eab4e4ab3391e75ce7af80ed6d7b564d28 -->
<!-- api: M:Inno.Native.ImGui.PlatformCreateWindow.Invoke(Inno.Native.ImGui.ImGuiViewport*) 91384f7b067dea85a805d9c7b2affd9a8a2964aaf0aef1e38145f40436c4ed12 -->
<!-- api: M:Inno.Native.ImGui.PlatformCreateWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) 92b7275501e0ec2ddbebfe52a025f5046d8cf3c731eab5d5b78e378de81d11c6 -->
<!-- api: M:Inno.Native.ImGui.PlatformCreateWindow.EndInvoke(System.IAsyncResult) 65d658740dd4e3d804a79368b5e6770793327102f88bfacb1ae7586ad51e2c57 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5f9a6ffda97e4c0a"></a>[`public Inno.Native.ImGui.PlatformCreateWindow.PlatformCreateWindow(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10453) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a99335353a507cd1"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformCreateWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10453) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f411e115507bf0c2"></a>[`public virtual void Inno.Native.ImGui.PlatformCreateWindow.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10453) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6ae9088eda73fd25"></a>[`public virtual void Inno.Native.ImGui.PlatformCreateWindow.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10453) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8dd76b867a30413a"></a>[`public delegate void Inno.Native.ImGui.PlatformCreateWindow(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10453) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformDestroyWindow</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformDestroyWindow d828baffb2e1fd867bc0175480ea639535bbf3379097a48faf26ada7c9f3777a -->
<!-- api: M:Inno.Native.ImGui.PlatformDestroyWindow.#ctor(System.Object,System.IntPtr) 700601849816a8e483959aed593ec039b92d335962baba4e6165de1b649d2df9 -->
<!-- api: M:Inno.Native.ImGui.PlatformDestroyWindow.Invoke(Inno.Native.ImGui.ImGuiViewport*) 0a8c9b4d53b5e903af90745d6768a636a7d14362e0434c4ec0d6361f42bd415f -->
<!-- api: M:Inno.Native.ImGui.PlatformDestroyWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) 36870c1781fdf26e0e7d3d2660646b67098282cd6facc7e20bae90b33a9cb287 -->
<!-- api: M:Inno.Native.ImGui.PlatformDestroyWindow.EndInvoke(System.IAsyncResult) 42bce7da3d5080a80cd4992977f2b1942832fd0ab72b77b70228f5f61e02e28d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-bb3cb156b1f45780"></a>[`public Inno.Native.ImGui.PlatformDestroyWindow.PlatformDestroyWindow(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10458) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-da9750cd6aaf6140"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformDestroyWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10458) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7ec813257208799a"></a>[`public virtual void Inno.Native.ImGui.PlatformDestroyWindow.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10458) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eca181c59f465685"></a>[`public virtual void Inno.Native.ImGui.PlatformDestroyWindow.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10458) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2abfdecd512eb354"></a>[`public delegate void Inno.Native.ImGui.PlatformDestroyWindow(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10458) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformGetClipboardTextFn</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformGetClipboardTextFn 798c8a908d3fe4851e46ac9ede1909dba53c355ef82fa3a7f97195992b8a6a35 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetClipboardTextFn.#ctor(System.Object,System.IntPtr) 6adeee673f5651b3dae06a63807e19024121283e5e5b4d79db074cdd6acd3892 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetClipboardTextFn.Invoke(Inno.Native.ImGui.ImGuiContext*) 06de09784e0f3383d8a289669fb36aeff03237655e515da27008c47c3a978acf -->
<!-- api: M:Inno.Native.ImGui.PlatformGetClipboardTextFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext*,System.AsyncCallback,System.Object) 92187a90e7291bbafad089edac866c6a0a798f841dd8df2f83ea909d4346651a -->
<!-- api: M:Inno.Native.ImGui.PlatformGetClipboardTextFn.EndInvoke(System.IAsyncResult) 02ab28b8bc97d68e7725f6ec826ba315ad926e5d465e544e2daf6f233542daf9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fd6d9b599426894a"></a>[`public Inno.Native.ImGui.PlatformGetClipboardTextFn.PlatformGetClipboardTextFn(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10433) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fe3712dcca748bfa"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformGetClipboardTextFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext* ctx, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10433) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bc5a2af7ec4cbc7a"></a>[`public virtual byte* Inno.Native.ImGui.PlatformGetClipboardTextFn.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10433) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-098c5926c2d41917"></a>[`public virtual byte* Inno.Native.ImGui.PlatformGetClipboardTextFn.Invoke(Inno.Native.ImGui.ImGuiContext* ctx)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10433) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a29724f906bca2ab"></a>[`public delegate byte* Inno.Native.ImGui.PlatformGetClipboardTextFn(Inno.Native.ImGui.ImGuiContext* ctx)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10433) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformGetWindowDpiScale</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformGetWindowDpiScale 0706478321bf5b0a67429f09ca7cd2450ef5806c0e62fbcf9f43405472bc6f83 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowDpiScale.#ctor(System.Object,System.IntPtr) dc0ec62bf3315ced61ed17951a3362d9eca75e9f9a0ad740cb0065548d04313d -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowDpiScale.Invoke(Inno.Native.ImGui.ImGuiViewport*) 7e3fbd96e174cd56768c74fc9797c30ad63fef4fc3a5e76423dbea451d5dab53 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowDpiScale.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) b183844bbfcceba80557431f53b82fbc87144e0f29df2725bd1f4182fea43417 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowDpiScale.EndInvoke(System.IAsyncResult) fd5bcb304e77bbb459ef25556dfbdac0672fe27ef1ff085b54d58660a6f781aa -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-97732132c8375638"></a>[`public Inno.Native.ImGui.PlatformGetWindowDpiScale.PlatformGetWindowDpiScale(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10533) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4bacdeb86a0f9c21"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformGetWindowDpiScale.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10533) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8eddb93850d7819d"></a>[`public virtual float Inno.Native.ImGui.PlatformGetWindowDpiScale.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10533) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5337380c8e97cbca"></a>[`public virtual float Inno.Native.ImGui.PlatformGetWindowDpiScale.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10533) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f0329de257e66602"></a>[`public delegate float Inno.Native.ImGui.PlatformGetWindowDpiScale(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10533) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformGetWindowFocus</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformGetWindowFocus 6019208be6ff4d0b097b0f7add217989c7065185bb1644b5c2cfa93297cfd219 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowFocus.#ctor(System.Object,System.IntPtr) d5a6f9c768308b2f11c06331221cc4ac43fc53f6eae299dca6f274ff2342257b -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowFocus.Invoke(Inno.Native.ImGui.ImGuiViewport*) 5188c753af390982a437f17ba963ef2e88560a5451664578a059fc248c919143 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowFocus.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) 54b8d5099922ecd2951204cc524be25b0717a6d0dcc68bcfe8ce7a3c28acb9c9 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowFocus.EndInvoke(System.IAsyncResult) 4e5090d21f9cbb6d9bfa1a9a82468038da75b35ada8afc85c4a22c7cec2db3b2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f5f323f3f5b4295d"></a>[`public Inno.Native.ImGui.PlatformGetWindowFocus.PlatformGetWindowFocus(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10498) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-dfd5f458b8c2450d"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformGetWindowFocus.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10498) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9eb61511c9a4532c"></a>[`public virtual byte Inno.Native.ImGui.PlatformGetWindowFocus.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10498) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5f6d9a97b833d5fa"></a>[`public virtual byte Inno.Native.ImGui.PlatformGetWindowFocus.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10498) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b7ca7f05e03535a6"></a>[`public delegate byte Inno.Native.ImGui.PlatformGetWindowFocus(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10498) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformGetWindowFramebufferScale</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformGetWindowFramebufferScale a6e26f33544397c7a704cf305d389a8747f32071e9753fc65d0796fc5fc65129 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowFramebufferScale.#ctor(System.Object,System.IntPtr) 95f8a6eb3fc220ff378bfde64f00f0be48b8c494bdf3b5f94f916cc96e8360d2 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowFramebufferScale.Invoke(Inno.Native.ImGui.ImGuiViewport*) cb00184804bba9d4451b3fa933eaacb7a84104813f22d53ab3385f23ec8ce2f2 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowFramebufferScale.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) ab694fa65b9ca67170510a73096c64b9b235aac0324a6e305c5f2cf47c94b8c5 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowFramebufferScale.EndInvoke(System.IAsyncResult) bde97859a3c6460e437192730723b722ee0766f68bb31fa73b71f8110bcbc98c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-eaa6dcb5576e8c5f"></a>[`public Inno.Native.ImGui.PlatformGetWindowFramebufferScale.PlatformGetWindowFramebufferScale(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10488) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3d563d9740c7d54a"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformGetWindowFramebufferScale.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10488) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2fe4771b824024ea"></a>[`public virtual System.Numerics.Vector2 Inno.Native.ImGui.PlatformGetWindowFramebufferScale.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10488) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8d562c862683de68"></a>[`public virtual System.Numerics.Vector2 Inno.Native.ImGui.PlatformGetWindowFramebufferScale.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10488) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-71c317fa7f2a23f7"></a>[`public delegate System.Numerics.Vector2 Inno.Native.ImGui.PlatformGetWindowFramebufferScale(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10488) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformGetWindowMinimized</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformGetWindowMinimized d8b4c6b4c6e9ffc09a04ca7bc6139f804f889bf0308f9c9e6404d043ddc8466c -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowMinimized.#ctor(System.Object,System.IntPtr) 0d736aceca1d141c22b734d47a2556d432263524b904849f18c70f761489110d -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowMinimized.Invoke(Inno.Native.ImGui.ImGuiViewport*) 1be0db49bcdc63f4dd0987b79368ee338d844187a17983c9243ebaa4169dcd8a -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowMinimized.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) 5a3e9236d3700bdab43c14a991d9a98b3177c39a482dfc1ba4ad008e900fe472 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowMinimized.EndInvoke(System.IAsyncResult) e655916b9bc67e226fe5ae8c057b9b0b0d1e15ca784b2e549501d961fd357c0b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3466f809c6610aee"></a>[`public Inno.Native.ImGui.PlatformGetWindowMinimized.PlatformGetWindowMinimized(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10503) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-843ca7fde1ed5dd6"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformGetWindowMinimized.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10503) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-71390ea2769ebdd9"></a>[`public virtual byte Inno.Native.ImGui.PlatformGetWindowMinimized.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10503) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8ac10ac55b64f7cd"></a>[`public virtual byte Inno.Native.ImGui.PlatformGetWindowMinimized.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10503) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-728ead0474747b8f"></a>[`public delegate byte Inno.Native.ImGui.PlatformGetWindowMinimized(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10503) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformGetWindowPos</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformGetWindowPos b6181ef6aeb84993dd5d1cb550f5879de1fb21f011d7d8f109fda251f9f9f69e -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowPos.#ctor(System.Object,System.IntPtr) d1ddc2709755cd2d3aa580fb823b625b3208787ba88e2a4dac96f274d62b9514 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowPos.Invoke(Inno.Native.ImGui.ImGuiViewport*) e3c7fac5a19fa724a8e75a960a328b8daf91c45c334d943b1022f39da28d5244 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowPos.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) 78bc1856bbaaa990d56b92f3dfbfa3e50610ad20ce8dbbb2f9748295709323d2 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowPos.EndInvoke(System.IAsyncResult) bcf32c87938c3064ccb9d0f579716814c9bc8cbbbf6c79cc42eb679b249d79b4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-484d06f147dbf6aa"></a>[`public Inno.Native.ImGui.PlatformGetWindowPos.PlatformGetWindowPos(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10473) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2e7262dab78963ab"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformGetWindowPos.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10473) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0f947e3556d9f845"></a>[`public virtual System.Numerics.Vector2 Inno.Native.ImGui.PlatformGetWindowPos.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10473) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-42f6aa3c02cebc18"></a>[`public virtual System.Numerics.Vector2 Inno.Native.ImGui.PlatformGetWindowPos.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10473) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7efbefade7c6fc04"></a>[`public delegate System.Numerics.Vector2 Inno.Native.ImGui.PlatformGetWindowPos(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10473) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformGetWindowSize</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformGetWindowSize 1bfd9f9c310e8ce5181f7d52eef14d729f8b2f386e48f4ea1014ef79cfc25ed3 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowSize.#ctor(System.Object,System.IntPtr) caf9a996878f8b0dae39485b83edf008a177b1aa8a3b30d47c62cfe6c8c77d15 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowSize.Invoke(Inno.Native.ImGui.ImGuiViewport*) 97e096bca463edfa35f9a609c2408e8f19a6bed7ed213f47a425cb85c9566048 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowSize.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) f3673eb1f00606666eedb30cfb16d04b4c3b33635e37b8c18f7e79829771ca53 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowSize.EndInvoke(System.IAsyncResult) 99fcb85918c0a87c45f866e6c37df1386ad1f40d386bece7a9047cf6b0eb7dc2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-333e77ad575807e2"></a>[`public Inno.Native.ImGui.PlatformGetWindowSize.PlatformGetWindowSize(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10483) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3f9359e5f7dc116c"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformGetWindowSize.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10483) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-05d6a9ea09cc84c7"></a>[`public virtual System.Numerics.Vector2 Inno.Native.ImGui.PlatformGetWindowSize.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10483) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ec17fe92f587c16"></a>[`public virtual System.Numerics.Vector2 Inno.Native.ImGui.PlatformGetWindowSize.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10483) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-274c2f1657ca264d"></a>[`public delegate System.Numerics.Vector2 Inno.Native.ImGui.PlatformGetWindowSize(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10483) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformGetWindowWorkAreaInsets</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformGetWindowWorkAreaInsets bf75b29faf61b3a31ea0ba75a6d7a7137a85a192a2c04cbe5c2392f65f995b8f -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowWorkAreaInsets.#ctor(System.Object,System.IntPtr) 7a61a3a75a3d319be0e86cf6b2205f3c32dbce61184827f878e5440418bb8e42 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowWorkAreaInsets.Invoke(Inno.Native.ImGui.ImGuiViewport*) f53b6fa8964095777c8b65c50313c7bbbe5f723838cf56d3be786b641ab3329b -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowWorkAreaInsets.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) 86c147f1f5652ec58d4995a6c4c56ebf62fc3dc342fcabab095b760113abe199 -->
<!-- api: M:Inno.Native.ImGui.PlatformGetWindowWorkAreaInsets.EndInvoke(System.IAsyncResult) cfd15dd531659f838295c29a47dec5994ea220aacec279d6a4bc3dc58ae15a07 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5583492eb04cc372"></a>[`public Inno.Native.ImGui.PlatformGetWindowWorkAreaInsets.PlatformGetWindowWorkAreaInsets(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10543) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-12ea6462086008a6"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformGetWindowWorkAreaInsets.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10543) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e5bebecd8b3b220f"></a>[`public virtual System.Numerics.Vector4 Inno.Native.ImGui.PlatformGetWindowWorkAreaInsets.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10543) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cabcdc23c6a5898d"></a>[`public virtual System.Numerics.Vector4 Inno.Native.ImGui.PlatformGetWindowWorkAreaInsets.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10543) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bcbb4de72c4eb7bd"></a>[`public delegate System.Numerics.Vector4 Inno.Native.ImGui.PlatformGetWindowWorkAreaInsets(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10543) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformOnChangedViewport</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformOnChangedViewport f13edeff44b69d6f45925557e1facb7c4f3d1fc063a7e06fbcf5e309967c75cf -->
<!-- api: M:Inno.Native.ImGui.PlatformOnChangedViewport.#ctor(System.Object,System.IntPtr) 2b7e2806a4c50b58be917d83b1d63c3141e8bd930b199b28a708da0432697d0b -->
<!-- api: M:Inno.Native.ImGui.PlatformOnChangedViewport.Invoke(Inno.Native.ImGui.ImGuiViewport*) 99e2352ed3b25852978d5fe948326989aa0accd1907c8d55f261feb6352e0cc9 -->
<!-- api: M:Inno.Native.ImGui.PlatformOnChangedViewport.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) 1d6d89df1061f1b736315562c54a9acca3ae3d49176f2a7d38d8a8035cf6f685 -->
<!-- api: M:Inno.Native.ImGui.PlatformOnChangedViewport.EndInvoke(System.IAsyncResult) 304e467b25034a63342567857ee6e5d48a7cd7ada15855082410d01241c779ee -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-15b9012a574c5c56"></a>[`public Inno.Native.ImGui.PlatformOnChangedViewport.PlatformOnChangedViewport(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10538) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-dfd15cd3f0c85851"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformOnChangedViewport.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10538) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6a6648a720df0cd7"></a>[`public virtual void Inno.Native.ImGui.PlatformOnChangedViewport.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10538) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8def4fbd6acc99c8"></a>[`public virtual void Inno.Native.ImGui.PlatformOnChangedViewport.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10538) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-00bcb3e4a706b715"></a>[`public delegate void Inno.Native.ImGui.PlatformOnChangedViewport(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10538) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformOpenInShellFn</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformOpenInShellFn 626c35152f842cab588b2d750270f1a2537b872753ce927b204162f7f91d234a -->
<!-- api: M:Inno.Native.ImGui.PlatformOpenInShellFn.#ctor(System.Object,System.IntPtr) 086d3fb8390bf1ff3314d029923b1c8c46492dfccb93a7db17f87b8e87a370a7 -->
<!-- api: M:Inno.Native.ImGui.PlatformOpenInShellFn.Invoke(Inno.Native.ImGui.ImGuiContext*,System.Byte*) 4ae56eac64e9e82df77281f329dd7632911561db28e7949b0dfafb7bdf2aa470 -->
<!-- api: M:Inno.Native.ImGui.PlatformOpenInShellFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext*,System.Byte*,System.AsyncCallback,System.Object) cc5439383da8ff15d8f06bf3ed700fb8d0124875b7fe177b139d50dc98a86c1f -->
<!-- api: M:Inno.Native.ImGui.PlatformOpenInShellFn.EndInvoke(System.IAsyncResult) 0ec17325d7a56db032b01eb6897e3ba053fabe73ff7f6744f1dc733d220704f9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2584b60f20685d9e"></a>[`public Inno.Native.ImGui.PlatformOpenInShellFn.PlatformOpenInShellFn(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10443) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-643392a87abae78d"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformOpenInShellFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext* ctx, byte* path, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10443) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-41797c3dd650a2a0"></a>[`public virtual byte Inno.Native.ImGui.PlatformOpenInShellFn.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10443) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e655d80d869bfbf5"></a>[`public virtual byte Inno.Native.ImGui.PlatformOpenInShellFn.Invoke(Inno.Native.ImGui.ImGuiContext* ctx, byte* path)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10443) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2d01570e06086e63"></a>[`public delegate byte Inno.Native.ImGui.PlatformOpenInShellFn(Inno.Native.ImGui.ImGuiContext* ctx, byte* path)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10443) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformRenderWindow</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformRenderWindow a59d0948488bf82c645a909be29306af3f742f488ba162065c383724564f6ad5 -->
<!-- api: M:Inno.Native.ImGui.PlatformRenderWindow.#ctor(System.Object,System.IntPtr) 3cb98d7e8f45b2867417f5f26e0bfd0e6ce374f402c3206a9a56c93498950b42 -->
<!-- api: M:Inno.Native.ImGui.PlatformRenderWindow.Invoke(Inno.Native.ImGui.ImGuiViewport*,System.Void*) 84eefedb35063881a55ab7d3405f36c1d9535a48ed513989a9878ee793c6b5e1 -->
<!-- api: M:Inno.Native.ImGui.PlatformRenderWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.Void*,System.AsyncCallback,System.Object) 64861b57200d35f1d822667b411624f7bb831b43fcc7880710145160cf46150b -->
<!-- api: M:Inno.Native.ImGui.PlatformRenderWindow.EndInvoke(System.IAsyncResult) 56e7201e1a14d0b64b078a35ba56245f415187ad0f098e905a60f7f9a9a3b812 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b53453ab711f401f"></a>[`public Inno.Native.ImGui.PlatformRenderWindow.PlatformRenderWindow(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10523) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e729fee37f355773"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformRenderWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, void* renderArg, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10523) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d292607bb98c1bf8"></a>[`public virtual void Inno.Native.ImGui.PlatformRenderWindow.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10523) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-21af2ffde2d9dbe9"></a>[`public virtual void Inno.Native.ImGui.PlatformRenderWindow.Invoke(Inno.Native.ImGui.ImGuiViewport* vp, void* renderArg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10523) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2f19492581c9a299"></a>[`public delegate void Inno.Native.ImGui.PlatformRenderWindow(Inno.Native.ImGui.ImGuiViewport* vp, void* renderArg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10523) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformSetClipboardTextFn</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformSetClipboardTextFn f787214c4dbf3737581e851d570cffe54c62883a0272ebc06bc2affa1734a106 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetClipboardTextFn.#ctor(System.Object,System.IntPtr) 1364df21dbfc6c0a6fb6f51db6c8b7ef2210a4ccac50358bd2367cef08bf375e -->
<!-- api: M:Inno.Native.ImGui.PlatformSetClipboardTextFn.Invoke(Inno.Native.ImGui.ImGuiContext*,System.Byte*) b411aa8fd18f132bdda933fabed0efea85f48ea39c8af812aae82494d22fe393 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetClipboardTextFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext*,System.Byte*,System.AsyncCallback,System.Object) 858c640a87168ca7c28ec339e97ccaccc589af091dba561d6fc67e55a78e1d0d -->
<!-- api: M:Inno.Native.ImGui.PlatformSetClipboardTextFn.EndInvoke(System.IAsyncResult) 9a455c7104e77eb822b626a965ebaafaf3c4dfcfc6e593d9ab852a62ccd1bdc2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4efcf3e4fa1c8035"></a>[`public Inno.Native.ImGui.PlatformSetClipboardTextFn.PlatformSetClipboardTextFn(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10438) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-26370af89a69dee5"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformSetClipboardTextFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext* ctx, byte* text, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10438) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c13eaff145f7f1ea"></a>[`public virtual void Inno.Native.ImGui.PlatformSetClipboardTextFn.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10438) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f0d598a3a38cfea3"></a>[`public virtual void Inno.Native.ImGui.PlatformSetClipboardTextFn.Invoke(Inno.Native.ImGui.ImGuiContext* ctx, byte* text)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10438) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5f651ceb06a54990"></a>[`public delegate void Inno.Native.ImGui.PlatformSetClipboardTextFn(Inno.Native.ImGui.ImGuiContext* ctx, byte* text)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10438) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformSetImeDataFn</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformSetImeDataFn 3956e0fe0dd623645f9fba80688f50ee3879c3a67e4b8616e7a6fd32baf2f59d -->
<!-- api: M:Inno.Native.ImGui.PlatformSetImeDataFn.#ctor(System.Object,System.IntPtr) 362f1fd2e4d7d18638352f8ad1dcebfe9d11972dcc2f21fc2eb105d9de78c0eb -->
<!-- api: M:Inno.Native.ImGui.PlatformSetImeDataFn.Invoke(Inno.Native.ImGui.ImGuiContext*,Inno.Native.ImGui.ImGuiViewport*,Inno.Native.ImGui.ImGuiPlatformImeData*) 3bf15804f10a26ea0430dac5bb83706bc0b2c2386c4de7ac20c8df523753e9ce -->
<!-- api: M:Inno.Native.ImGui.PlatformSetImeDataFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext*,Inno.Native.ImGui.ImGuiViewport*,Inno.Native.ImGui.ImGuiPlatformImeData*,System.AsyncCallback,System.Object) 378327bd188c2d6f1df66693215ead0b465afb576c04fcd4ee09c9b6860430b0 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetImeDataFn.EndInvoke(System.IAsyncResult) c5db17239fc938bcbc262042be3ace9777215ade92d3f7e68d2a08f2eaa04f85 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a74c03ca1938cb90"></a>[`public Inno.Native.ImGui.PlatformSetImeDataFn.PlatformSetImeDataFn(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10448) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-80eba63a20247c4f"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformSetImeDataFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiViewport* viewport, Inno.Native.ImGui.ImGuiPlatformImeData* data, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10448) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0df4618d09bf5e27"></a>[`public virtual void Inno.Native.ImGui.PlatformSetImeDataFn.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10448) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-da1d6d035f046368"></a>[`public virtual void Inno.Native.ImGui.PlatformSetImeDataFn.Invoke(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiViewport* viewport, Inno.Native.ImGui.ImGuiPlatformImeData* data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10448) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-580f61b663c91117"></a>[`public delegate void Inno.Native.ImGui.PlatformSetImeDataFn(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiViewport* viewport, Inno.Native.ImGui.ImGuiPlatformImeData* data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10448) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformSetWindowAlpha</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformSetWindowAlpha 8dc7d5a213a8a9fcb280d7acc7f8b3c68451911d7704fdef0fd054af38d59f9b -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowAlpha.#ctor(System.Object,System.IntPtr) 6235bf22c81ffe01faaa76aae523c4f8ef4bfcee2f96d01d05c3d52483d70370 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowAlpha.Invoke(Inno.Native.ImGui.ImGuiViewport*,System.Single) 5c3bae43f63dff1ab6d4b8513fb6f632e05d8f251f0ec1516c0839c92fb114a9 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowAlpha.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.Single,System.AsyncCallback,System.Object) 0a913f558e63e439879c7ce8ae7e1808a23827c2d36d92d460bcbdc21ccb0b24 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowAlpha.EndInvoke(System.IAsyncResult) 687490fc75c37f91507d5bcd96c9f129f043730dba8576e070131913e747f6cc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-636b466cee9813a1"></a>[`public Inno.Native.ImGui.PlatformSetWindowAlpha.PlatformSetWindowAlpha(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10513) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9d602358cc0ff9b1"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformSetWindowAlpha.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, float alpha, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10513) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-734d470f1f389819"></a>[`public virtual void Inno.Native.ImGui.PlatformSetWindowAlpha.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10513) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-829044545c14243a"></a>[`public virtual void Inno.Native.ImGui.PlatformSetWindowAlpha.Invoke(Inno.Native.ImGui.ImGuiViewport* vp, float alpha)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10513) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f874e0b10d64b10a"></a>[`public delegate void Inno.Native.ImGui.PlatformSetWindowAlpha(Inno.Native.ImGui.ImGuiViewport* vp, float alpha)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10513) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformSetWindowFocus</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformSetWindowFocus 1a3d212e441ed833465f55ef4e5ad57b159adc2a52ecfda8b79f2e9edc14c288 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowFocus.#ctor(System.Object,System.IntPtr) c946e3622777350555756c80b18e447f0cf694c96e99e2d48dbe30ae2696bafd -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowFocus.Invoke(Inno.Native.ImGui.ImGuiViewport*) c915b1b2bb37b1d3ef177936696eec493bf40355736cfacb810f81676ab48cb8 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowFocus.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) 953fc850fe17875f27754b8606235b73efd208e414051b2af39f4da447c5fbf5 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowFocus.EndInvoke(System.IAsyncResult) 2abdf98403a2da7fdc956720e28c56412fcfe89c8b6655ea23affa34da3c51d8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3dae2d630af30858"></a>[`public Inno.Native.ImGui.PlatformSetWindowFocus.PlatformSetWindowFocus(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10493) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-65f3ffd226cd4ded"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformSetWindowFocus.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10493) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fbc3c0fcd856ef8b"></a>[`public virtual void Inno.Native.ImGui.PlatformSetWindowFocus.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10493) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dc887acf55fb90a5"></a>[`public virtual void Inno.Native.ImGui.PlatformSetWindowFocus.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10493) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e8bb05ac24e4043f"></a>[`public delegate void Inno.Native.ImGui.PlatformSetWindowFocus(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10493) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformSetWindowPos</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformSetWindowPos f68858de08b6afd2ca4949bb65dccb9da017df617313d4108ae3f98129448054 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowPos.#ctor(System.Object,System.IntPtr) 0fe47e2e48a1f80a0d37beb7b6fbd9b9016ec5163f6d5b41ba4e1dc1baae31ad -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowPos.Invoke(Inno.Native.ImGui.ImGuiViewport*,System.Numerics.Vector2) 90f1cbb5e5411a7a3efba68269da2545450c82098cbb579e6750ed25c52c1777 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowPos.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.Numerics.Vector2,System.AsyncCallback,System.Object) 6aba1080f19d50c40a983f3708a2bf878efa02864fdb7a0b0e71d7642c52dc77 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowPos.EndInvoke(System.IAsyncResult) 9c2955ee5800992427df8f09ab96fd0ff4ea7f728f8bf38ec6cf5b7499398064 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-44ec01f25e6bcc39"></a>[`public Inno.Native.ImGui.PlatformSetWindowPos.PlatformSetWindowPos(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10468) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6af4bcefe0315ebc"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformSetWindowPos.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.Numerics.Vector2 pos, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10468) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-871054108836ae1b"></a>[`public virtual void Inno.Native.ImGui.PlatformSetWindowPos.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10468) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe38ab5a7db19763"></a>[`public virtual void Inno.Native.ImGui.PlatformSetWindowPos.Invoke(Inno.Native.ImGui.ImGuiViewport* vp, System.Numerics.Vector2 pos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10468) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-db7fe7e58bf9a2f9"></a>[`public delegate void Inno.Native.ImGui.PlatformSetWindowPos(Inno.Native.ImGui.ImGuiViewport* vp, System.Numerics.Vector2 pos)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10468) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformSetWindowSize</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformSetWindowSize 619fe5476b9544f38c7d55a8eae33a0fa4a2b90bc26e3ab3d3615935bec369dc -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowSize.#ctor(System.Object,System.IntPtr) e1e6f996f0a7cae4c52c547d878d68327c9c071628e457b2291257cffed6c09e -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowSize.Invoke(Inno.Native.ImGui.ImGuiViewport*,System.Numerics.Vector2) d40615c1446ca7b0bad3d4261d106a22430b4df313bc2b07eb653b99b7a87d44 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowSize.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.Numerics.Vector2,System.AsyncCallback,System.Object) 7d960c7cb93b4be8134813e25f8bf7e482f41d7b877d00b9a02b21d068a66cb4 -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowSize.EndInvoke(System.IAsyncResult) 0f1d8cc88231a057611ab95fce20a904c0c2eda3713f4792e9cbd4a9518027e8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3679f6646460e1fb"></a>[`public Inno.Native.ImGui.PlatformSetWindowSize.PlatformSetWindowSize(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10478) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-c10b57b1637632cd"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformSetWindowSize.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.Numerics.Vector2 size, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10478) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f18c942241d9eb86"></a>[`public virtual void Inno.Native.ImGui.PlatformSetWindowSize.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10478) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6504b8819895ef66"></a>[`public virtual void Inno.Native.ImGui.PlatformSetWindowSize.Invoke(Inno.Native.ImGui.ImGuiViewport* vp, System.Numerics.Vector2 size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10478) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-16ca1fa0c13d2dca"></a>[`public delegate void Inno.Native.ImGui.PlatformSetWindowSize(Inno.Native.ImGui.ImGuiViewport* vp, System.Numerics.Vector2 size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10478) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformSetWindowTitle</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformSetWindowTitle 464d3c3f13e22193d662be96d707d23143f1614163e467498948a25b18955d8a -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowTitle.#ctor(System.Object,System.IntPtr) 0095999b3a5c4bbf9101be09894aabea9fdfa62197f4350d924010665f12d67f -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowTitle.Invoke(Inno.Native.ImGui.ImGuiViewport*,System.Byte*) 37089a9f86ca89348fb96f9e2618b73e88cb0e8cb2c08e17c7d0880e52759fab -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowTitle.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.Byte*,System.AsyncCallback,System.Object) b891a24b7450553140d25beb817dea3f7b3a362d4e46a59ac4acd0aaaed82c5d -->
<!-- api: M:Inno.Native.ImGui.PlatformSetWindowTitle.EndInvoke(System.IAsyncResult) a64916699d80c2931bb7f52bf0818e24f73f9423acada3f2954ab9c2c66e1de3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3a6835ad272bab35"></a>[`public Inno.Native.ImGui.PlatformSetWindowTitle.PlatformSetWindowTitle(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10508) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fd0cb62053089386"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformSetWindowTitle.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, byte* str, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10508) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-67c9935a0074e256"></a>[`public virtual void Inno.Native.ImGui.PlatformSetWindowTitle.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10508) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-97ec5be7b231f614"></a>[`public virtual void Inno.Native.ImGui.PlatformSetWindowTitle.Invoke(Inno.Native.ImGui.ImGuiViewport* vp, byte* str)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10508) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dd3aba9721dd3ae5"></a>[`public delegate void Inno.Native.ImGui.PlatformSetWindowTitle(Inno.Native.ImGui.ImGuiViewport* vp, byte* str)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10508) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformShowWindow</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformShowWindow 5c85ce8bb491cf7754b8045c835fd6167cc3cae9b74ea7f997f671e2679892c4 -->
<!-- api: M:Inno.Native.ImGui.PlatformShowWindow.#ctor(System.Object,System.IntPtr) b8e0f12e5ba5fb038199a6068f35b38e75b6d67e6893cdf6e5b5f7da6e78e241 -->
<!-- api: M:Inno.Native.ImGui.PlatformShowWindow.Invoke(Inno.Native.ImGui.ImGuiViewport*) bb0bf3d2b60d0a1d04aa8109c3be0074b39a5ddfbf0c54301952e21d08568aac -->
<!-- api: M:Inno.Native.ImGui.PlatformShowWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) 50d0d5f978e4a3a4340f2f1f476d08bd8b7ab7e0ed4bf9a19baf0ac9041f16c5 -->
<!-- api: M:Inno.Native.ImGui.PlatformShowWindow.EndInvoke(System.IAsyncResult) 80013dbad39481c8e8effbe72867f441b4d627827b7f6c1eb030597c48f3210b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-94902a4ceebf33c5"></a>[`public Inno.Native.ImGui.PlatformShowWindow.PlatformShowWindow(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10463) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-01cbbb8b093b39ec"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformShowWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10463) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bd97a99a99983ab3"></a>[`public virtual void Inno.Native.ImGui.PlatformShowWindow.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10463) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-478e4876c6d239ff"></a>[`public virtual void Inno.Native.ImGui.PlatformShowWindow.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10463) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-77691bb81ac980d3"></a>[`public delegate void Inno.Native.ImGui.PlatformShowWindow(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10463) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformSwapBuffers</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformSwapBuffers b33ca6b9467a83e4c3dfcb18b514980ac2bb3fcda235bb53f02628ab294424da -->
<!-- api: M:Inno.Native.ImGui.PlatformSwapBuffers.#ctor(System.Object,System.IntPtr) 8acc7991445c343eaa033a232cb1bb2c838f1838d060e5f19572c1d30c6bfaf8 -->
<!-- api: M:Inno.Native.ImGui.PlatformSwapBuffers.Invoke(Inno.Native.ImGui.ImGuiViewport*,System.Void*) e304d811564b6cd2dc4649bdf96f63b71334bc0f3c193488c8df91e68b8266a1 -->
<!-- api: M:Inno.Native.ImGui.PlatformSwapBuffers.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.Void*,System.AsyncCallback,System.Object) e54821a1e9538787f55c5d43ad8d4abd5056c993bb6fcef40e469cc27508a92e -->
<!-- api: M:Inno.Native.ImGui.PlatformSwapBuffers.EndInvoke(System.IAsyncResult) 0a186c74874db3a5489b66c0819c90a8541d6b0684369467f83b1065692d48ab -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-420b1790499367e8"></a>[`public Inno.Native.ImGui.PlatformSwapBuffers.PlatformSwapBuffers(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10528) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ade328f4e1af4fac"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformSwapBuffers.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, void* renderArg, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10528) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-38a6a2abf713cda7"></a>[`public virtual void Inno.Native.ImGui.PlatformSwapBuffers.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10528) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d87de4de90273f17"></a>[`public virtual void Inno.Native.ImGui.PlatformSwapBuffers.Invoke(Inno.Native.ImGui.ImGuiViewport* vp, void* renderArg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10528) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8f364da44eeb5a41"></a>[`public delegate void Inno.Native.ImGui.PlatformSwapBuffers(Inno.Native.ImGui.ImGuiViewport* vp, void* renderArg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10528) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.PlatformUpdateWindow</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.PlatformUpdateWindow 0bf33889016b661531476fc189a318148bc6226d5744d51ad784752681e1595b -->
<!-- api: M:Inno.Native.ImGui.PlatformUpdateWindow.#ctor(System.Object,System.IntPtr) 100b530000c890addd9130ba9ab4971ec6396904be5971be863cbc06909f1ace -->
<!-- api: M:Inno.Native.ImGui.PlatformUpdateWindow.Invoke(Inno.Native.ImGui.ImGuiViewport*) 7222ec049084bab41c0c77a43e129fe856c4d1961f52ba74c7acb27a1427d33b -->
<!-- api: M:Inno.Native.ImGui.PlatformUpdateWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) b970359b55360da9d91e99bacfecf395951a88a50cceedd1db496e1f3b9d6ade -->
<!-- api: M:Inno.Native.ImGui.PlatformUpdateWindow.EndInvoke(System.IAsyncResult) 7c6adab690f4266c07a68ece05780479f0c9f9ceb789cb7b80ae40a604c7b629 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f3f8055243fe0ebd"></a>[`public Inno.Native.ImGui.PlatformUpdateWindow.PlatformUpdateWindow(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10518) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f9807ffa7c0b30f2"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.PlatformUpdateWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10518) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-643f852e953d9026"></a>[`public virtual void Inno.Native.ImGui.PlatformUpdateWindow.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10518) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aba62db26f4d4500"></a>[`public virtual void Inno.Native.ImGui.PlatformUpdateWindow.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10518) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6f04f4659cf4d429"></a>[`public delegate void Inno.Native.ImGui.PlatformUpdateWindow(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10518) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ReadInitFn</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ReadInitFn 78cbf66655a003d05da4ae969e45b75b6f934cc38b3eb5a9f3165af8dcb6978b -->
<!-- api: M:Inno.Native.ImGui.ReadInitFn.#ctor(System.Object,System.IntPtr) 878e19b3ec8e95503ad583b5772736e7e19008baf46cbfd346f1bc66b94fbdd2 -->
<!-- api: M:Inno.Native.ImGui.ReadInitFn.Invoke(Inno.Native.ImGui.ImGuiContext*,Inno.Native.ImGui.ImGuiSettingsHandler*) c54588ce8641ccaf4281f2130e902e7fd32a2dcd27565e0c66b715aa127db3f6 -->
<!-- api: M:Inno.Native.ImGui.ReadInitFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext*,Inno.Native.ImGui.ImGuiSettingsHandler*,System.AsyncCallback,System.Object) 12899b15552020e8b4ea8426b7c252206c3e32b1a53d30071a3b99c5bef6287a -->
<!-- api: M:Inno.Native.ImGui.ReadInitFn.EndInvoke(System.IAsyncResult) ee4b0708d95e81da032ee45980901e2d2fce1eb395594ace5566af59ea3d4717 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e3ad14bcb5e33fc8"></a>[`public Inno.Native.ImGui.ReadInitFn.ReadInitFn(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10593) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6f050124eefe523c"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.ReadInitFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiSettingsHandler* handler, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10593) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-56c5735acfd31b2a"></a>[`public virtual void Inno.Native.ImGui.ReadInitFn.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10593) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-972e5c53a1ddce31"></a>[`public virtual void Inno.Native.ImGui.ReadInitFn.Invoke(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiSettingsHandler* handler)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10593) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-57204c9e80a374d2"></a>[`public delegate void Inno.Native.ImGui.ReadInitFn(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiSettingsHandler* handler)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10593) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ReadLineFn</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ReadLineFn 1044c598b1bf1b37a1eff979e75472bee2761dc301c67df708a8f56596efced1 -->
<!-- api: M:Inno.Native.ImGui.ReadLineFn.#ctor(System.Object,System.IntPtr) d97fab8a2a43b7d00d0e42248d871ce2d680b5d5a0f6256f7ca7adf3beefcb0a -->
<!-- api: M:Inno.Native.ImGui.ReadLineFn.Invoke(Inno.Native.ImGui.ImGuiContext*,Inno.Native.ImGui.ImGuiSettingsHandler*,System.Void*,System.Byte*) b59cba3464ebd1bc3e3f6b1c13d1666c340ac8e989772b19597e93f8df86ed89 -->
<!-- api: M:Inno.Native.ImGui.ReadLineFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext*,Inno.Native.ImGui.ImGuiSettingsHandler*,System.Void*,System.Byte*,System.AsyncCallback,System.Object) d530578bffbdeae1bfab88160745d342d0f72671d12c3f1455c7b31f7dbcb144 -->
<!-- api: M:Inno.Native.ImGui.ReadLineFn.EndInvoke(System.IAsyncResult) 4d60e0424ef58bec260a8914d9ad92198da2b41b05b34210e3971e67b6e173e7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-19eea07f23995d69"></a>[`public Inno.Native.ImGui.ReadLineFn.ReadLineFn(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10603) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-af575e826928ab47"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.ReadLineFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiSettingsHandler* handler, void* entry, byte* line, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10603) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-31c853bbc52d2487"></a>[`public virtual void Inno.Native.ImGui.ReadLineFn.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10603) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4095f708c74f46ca"></a>[`public virtual void Inno.Native.ImGui.ReadLineFn.Invoke(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiSettingsHandler* handler, void* entry, byte* line)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10603) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b27a4e0b58e8aaf9"></a>[`public delegate void Inno.Native.ImGui.ReadLineFn(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiSettingsHandler* handler, void* entry, byte* line)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10603) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ReadOpenFn</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ReadOpenFn afe39ca5bce2beca6e5f0eee3792b312b0c2382f3c4cd394eb5d47ad319c07bb -->
<!-- api: M:Inno.Native.ImGui.ReadOpenFn.#ctor(System.Object,System.IntPtr) 344fd19e9c3bdc63156864bf674334167ce2f2e9b1acaf9409f02b45c8e610b0 -->
<!-- api: M:Inno.Native.ImGui.ReadOpenFn.Invoke(Inno.Native.ImGui.ImGuiContext*,Inno.Native.ImGui.ImGuiSettingsHandler*,System.Byte*) a1030a608a4f3b7fb2be47f9a75ea0a4c28cbb9ac65af4b9ef4f23de2e28274d -->
<!-- api: M:Inno.Native.ImGui.ReadOpenFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext*,Inno.Native.ImGui.ImGuiSettingsHandler*,System.Byte*,System.AsyncCallback,System.Object) a5ffadc293c01bbff9df7ada73b85ec8313c8d72776378465f9f3b65c2da6f90 -->
<!-- api: M:Inno.Native.ImGui.ReadOpenFn.EndInvoke(System.IAsyncResult) 36227e7c937ba53d63f1e02f698297adaf06173f861b83a9e4f81253be8ff5b7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fe44ffb2ee7c3de3"></a>[`public Inno.Native.ImGui.ReadOpenFn.ReadOpenFn(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10598) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5203421150912574"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.ReadOpenFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiSettingsHandler* handler, byte* name, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10598) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ef59a27b36281e50"></a>[`public virtual void* Inno.Native.ImGui.ReadOpenFn.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10598) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-09a4871863dd70b0"></a>[`public virtual void* Inno.Native.ImGui.ReadOpenFn.Invoke(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiSettingsHandler* handler, byte* name)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10598) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e58f6722985bca2f"></a>[`public delegate void* Inno.Native.ImGui.ReadOpenFn(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiSettingsHandler* handler, byte* name)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10598) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.RendererCreateWindow</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.RendererCreateWindow be02788396237117cb04b13cfe3c0647bff82a5211f39dd62f198ca767a2dd66 -->
<!-- api: M:Inno.Native.ImGui.RendererCreateWindow.#ctor(System.Object,System.IntPtr) 09f2b4cf38a1ebc45f85932c2d2556ee8f697709872d7d05da7b7010b42053f4 -->
<!-- api: M:Inno.Native.ImGui.RendererCreateWindow.Invoke(Inno.Native.ImGui.ImGuiViewport*) a2e6f88169d80773778cbfd9843aca71dab0193f37da40a082457c174156f1b0 -->
<!-- api: M:Inno.Native.ImGui.RendererCreateWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) 37ff2ee704fc8fc97750775546d28fc9e642ed56d3605e794f09703f06ff1f7e -->
<!-- api: M:Inno.Native.ImGui.RendererCreateWindow.EndInvoke(System.IAsyncResult) ddd5983d1f321007b2a7a73d95626e63891ca2615185e847a4839ae97434edaa -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0a48b9f4f45091e9"></a>[`public Inno.Native.ImGui.RendererCreateWindow.RendererCreateWindow(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10553) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ff44dbe784ae6f84"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.RendererCreateWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10553) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-436defec336cdd2a"></a>[`public virtual void Inno.Native.ImGui.RendererCreateWindow.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10553) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3c82dfde537128b3"></a>[`public virtual void Inno.Native.ImGui.RendererCreateWindow.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10553) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-428fdf07782bd6db"></a>[`public delegate void Inno.Native.ImGui.RendererCreateWindow(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10553) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.RendererDestroyWindow</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.RendererDestroyWindow 80ded85aa2255712a645e2b48a4276fae98444daa7fe8363d6045eb6fdd551d4 -->
<!-- api: M:Inno.Native.ImGui.RendererDestroyWindow.#ctor(System.Object,System.IntPtr) 4da2cbc31b891954c6f6b7d89a94ec114b4af9d109df185ce5144de0a289f376 -->
<!-- api: M:Inno.Native.ImGui.RendererDestroyWindow.Invoke(Inno.Native.ImGui.ImGuiViewport*) ac0061f4a8fcac5f8eea2b733232e72a2710fd1fc37aa81b1bbe127f1bd29eed -->
<!-- api: M:Inno.Native.ImGui.RendererDestroyWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.AsyncCallback,System.Object) 729bdade44f8e8b922dd12c5759aedeaa6067159a5774e84d237ca45633d2b2f -->
<!-- api: M:Inno.Native.ImGui.RendererDestroyWindow.EndInvoke(System.IAsyncResult) a3cb4bcc0fdd6e6fa20a3336aa427edb9c3a2234255f34df84e2d65a9ad43cd2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2ac9fdb3372c764b"></a>[`public Inno.Native.ImGui.RendererDestroyWindow.RendererDestroyWindow(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10558) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-180cf7fd7cd053e7"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.RendererDestroyWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10558) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-05d002b7e6a3b66e"></a>[`public virtual void Inno.Native.ImGui.RendererDestroyWindow.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10558) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e93c465cdb9abf9e"></a>[`public virtual void Inno.Native.ImGui.RendererDestroyWindow.Invoke(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10558) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0b126003c5057445"></a>[`public delegate void Inno.Native.ImGui.RendererDestroyWindow(Inno.Native.ImGui.ImGuiViewport* vp)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10558) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.RendererRenderWindow</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.RendererRenderWindow 39e01b5c0a7bdf0a1649306c1c48831fece24e9df6244d20c012e2fd25ed3d64 -->
<!-- api: M:Inno.Native.ImGui.RendererRenderWindow.#ctor(System.Object,System.IntPtr) cfd850a6d66de2f578ad0e6a62356559b472114a2a76487afbb53636751258ee -->
<!-- api: M:Inno.Native.ImGui.RendererRenderWindow.Invoke(Inno.Native.ImGui.ImGuiViewport*,System.Void*) 66fa0dd11378d4b8fda3cbfa1146a57e6ad6633309b553d713a9849379d9ebcc -->
<!-- api: M:Inno.Native.ImGui.RendererRenderWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.Void*,System.AsyncCallback,System.Object) e9d83f307bd90c39b96f968f14c6301bef1af4cf5dcc59ac05cf0df6344f0340 -->
<!-- api: M:Inno.Native.ImGui.RendererRenderWindow.EndInvoke(System.IAsyncResult) 75a60a94c1c25d204c1ac84e8e6664e856565b76158e72dd4f7668a2672ccafa -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-644ca0fd0bceb0f0"></a>[`public Inno.Native.ImGui.RendererRenderWindow.RendererRenderWindow(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10568) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-aa251c82d071653a"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.RendererRenderWindow.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, void* renderArg, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10568) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-19f07d82b246e70b"></a>[`public virtual void Inno.Native.ImGui.RendererRenderWindow.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10568) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-02137d45feef88f1"></a>[`public virtual void Inno.Native.ImGui.RendererRenderWindow.Invoke(Inno.Native.ImGui.ImGuiViewport* vp, void* renderArg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10568) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5eb2695b8d2ab7fd"></a>[`public delegate void Inno.Native.ImGui.RendererRenderWindow(Inno.Native.ImGui.ImGuiViewport* vp, void* renderArg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10568) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.RendererSetWindowSize</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.RendererSetWindowSize 490064319198288de8c6e319af24786feded7ba15b5953416d293be294896362 -->
<!-- api: M:Inno.Native.ImGui.RendererSetWindowSize.#ctor(System.Object,System.IntPtr) 1f5d560be62455c99e1b1c314c6a0a6241566eee7bf73f0c45688e0692766e77 -->
<!-- api: M:Inno.Native.ImGui.RendererSetWindowSize.Invoke(Inno.Native.ImGui.ImGuiViewport*,System.Numerics.Vector2) 705674a0e0f3661e11cb84b839bc060046f5e9e22c4192af42615fc6a8ded382 -->
<!-- api: M:Inno.Native.ImGui.RendererSetWindowSize.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.Numerics.Vector2,System.AsyncCallback,System.Object) 27c8e6fc60438c2c2ed8dc2fc02e31941cf316364c9c5e0cce0ad0d93b0d80df -->
<!-- api: M:Inno.Native.ImGui.RendererSetWindowSize.EndInvoke(System.IAsyncResult) 815752f119f00e91f1f9396c0b8750db521227b8e9c2a3681a4b9e42b2247617 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f915ce3397e35b89"></a>[`public Inno.Native.ImGui.RendererSetWindowSize.RendererSetWindowSize(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10563) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-90c8d44f7101f3c2"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.RendererSetWindowSize.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, System.Numerics.Vector2 size, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10563) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9536c2df60ee0268"></a>[`public virtual void Inno.Native.ImGui.RendererSetWindowSize.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10563) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ffe6def3bbce8d90"></a>[`public virtual void Inno.Native.ImGui.RendererSetWindowSize.Invoke(Inno.Native.ImGui.ImGuiViewport* vp, System.Numerics.Vector2 size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10563) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3dd162a01d1dfad9"></a>[`public delegate void Inno.Native.ImGui.RendererSetWindowSize(Inno.Native.ImGui.ImGuiViewport* vp, System.Numerics.Vector2 size)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10563) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.RendererSwapBuffers</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.RendererSwapBuffers bb713f461fa5ec1154e8889f1e413317536bba9972a51558c32df5ec8bcefd98 -->
<!-- api: M:Inno.Native.ImGui.RendererSwapBuffers.#ctor(System.Object,System.IntPtr) 5496c5ae58b66a98ab48c9fc405838d02cd3577fe5cb66edf9b4edfec2c34a8c -->
<!-- api: M:Inno.Native.ImGui.RendererSwapBuffers.Invoke(Inno.Native.ImGui.ImGuiViewport*,System.Void*) d469eed3de2270fe8defd3be3566d90c1f1f90f3d9cb2e16eb187e0ea6584fbd -->
<!-- api: M:Inno.Native.ImGui.RendererSwapBuffers.BeginInvoke(Inno.Native.ImGui.ImGuiViewport*,System.Void*,System.AsyncCallback,System.Object) c98759912c788fb3f813a06f054fb7e50cd833c5603edd47ae3bcade54a7a2a0 -->
<!-- api: M:Inno.Native.ImGui.RendererSwapBuffers.EndInvoke(System.IAsyncResult) 7ec988a872deba76a79592ea6720b654718fc964ce62669f781f3ef63db5b447 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3a9053238047de3b"></a>[`public Inno.Native.ImGui.RendererSwapBuffers.RendererSwapBuffers(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10573) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-689a029b3e4203ae"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.RendererSwapBuffers.BeginInvoke(Inno.Native.ImGui.ImGuiViewport* vp, void* renderArg, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10573) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fc96ed8b26d09500"></a>[`public virtual void Inno.Native.ImGui.RendererSwapBuffers.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10573) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-77eb3057ce6a7009"></a>[`public virtual void Inno.Native.ImGui.RendererSwapBuffers.Invoke(Inno.Native.ImGui.ImGuiViewport* vp, void* renderArg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10573) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ad2263c9d50664de"></a>[`public delegate void Inno.Native.ImGui.RendererSwapBuffers(Inno.Native.ImGui.ImGuiViewport* vp, void* renderArg)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10573) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.SizeCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.SizeCallback cfa17fb9f9f0e06be692bfff3c7ef0d141ad8b3947a2bd10a13684d91d9f6c92 -->
<!-- api: M:Inno.Native.ImGui.SizeCallback.#ctor(System.Object,System.IntPtr) e9fc0670d2a6573111cc1f20fb95071112c1dbd729dd8776505f4e45e65c7fe3 -->
<!-- api: M:Inno.Native.ImGui.SizeCallback.Invoke(Inno.Native.ImGui.ImGuiSizeCallbackData*) 5f8e9068c22f24b3e8f37bdcce22b21de1d8e1cd61b467801eb4233522586c5b -->
<!-- api: M:Inno.Native.ImGui.SizeCallback.BeginInvoke(Inno.Native.ImGui.ImGuiSizeCallbackData*,System.AsyncCallback,System.Object) 49c5b047e9ec5fc471a29c315da3068474758646c0acfc7716992c71a9bb86a5 -->
<!-- api: M:Inno.Native.ImGui.SizeCallback.EndInvoke(System.IAsyncResult) e87a7f837392aceb2eca0409406ee5bb98ac901a79e7f6d24896aa908ab08226 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-79611128487f211f"></a>[`public Inno.Native.ImGui.SizeCallback.SizeCallback(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10583) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-52da6346affdbdcb"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.SizeCallback.BeginInvoke(Inno.Native.ImGui.ImGuiSizeCallbackData* data, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10583) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c6b0133c6563d855"></a>[`public virtual void Inno.Native.ImGui.SizeCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10583) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2c48cb7ebe27c85a"></a>[`public virtual void Inno.Native.ImGui.SizeCallback.Invoke(Inno.Native.ImGui.ImGuiSizeCallbackData* data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10583) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-af50afa6b0f0105a"></a>[`public delegate void Inno.Native.ImGui.SizeCallback(Inno.Native.ImGui.ImGuiSizeCallbackData* data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10583) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.StbrpContextOpaque</code> · 83 个声明</summary>

<!-- api: T:Inno.Native.ImGui.StbrpContextOpaque 95156853a30d0c8f22b23e751e20d3c485c1f1b30b3cf575749027ec097977a5 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_0 0eda14a867ad3c03a99133977bb3e1aeff9324c5983ac6ec7a86e5616b144939 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_1 f249949d7df583cd80c05c86fd7eb4bea9a0bc3dbcba95fa6664bb022703f887 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_2 0da91e0e30ad060c93dc3f9271ab663dd61fd4228154031b93c8ec04ca3af8fa -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_3 65e73d1e60fb396f5f2e804a96b7d16b43fb9325c93439a993ab0312d6e7912f -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_4 d5455f466d9b7500cd7b48f8ca04f25e941917759f3adecf4c1cdc80658cce3f -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_5 b4b5dce6fae73feb0423d726cf51fcc7aea9e357aa5820af21dd32552e15e880 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_6 0e49c02065fe54f30f9a43a508728ef2fa4004dfa4be7a2c1d2d77e0a4d4a4f1 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_7 238a2b7b96a1cf5d842083feda98d35f54c944e721fbad36ce92d40bcccab872 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_8 0a11ebed678744e50d4af74861b9fd95dc27897b2c9a2251861b4667217d2f0c -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_9 fe493c334cb80e37acdc770fd3ff0558731e15745da05aaceed393b3f6e0d0f5 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_10 370078437bdfc78800f58c6042a0b11aa2d671f25f783946fb0521f4384ab699 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_11 511a68a4bc207bd07015c1c43b25d0b54f39ca6c1bd44acb2cbfeb0b54eff35b -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_12 44cadc0ccc478a3d4d488325fb97f8398f026945203f7b0642ea34fbf8e8ae94 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_13 a299def202ab7ac1261b871ff2adc7beb638c4c5d27356632d38600e35a5670b -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_14 3db5a5aab7859fbe1f35b04728983bd96ae7d2d647f7688e4970bef862274d5c -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_15 a896d4b9c2fa85f6bd13033ffd4a8d3f43835a60591f5f0ebf7a342bf56a2906 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_16 f31771621c7aaddab52be8fc284ce4ae9c7b61afa293190505bd90dcf263c60d -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_17 9ed9672195ba4e21febe1f88e5e4baa9ce02d503b0c5d7d1136e0d86c16f541e -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_18 905a4bad3239e9dda4b85306bcefbed2afe1299dbdc1a01589c3aee84983fa17 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_19 0f0d4017c1c5444e9cbe15ee418006e36b3809a433c2097f0bb38e91a42d9049 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_20 a434ce52f6b95638c767e85c123b0b1489e293d36f917e64a53b321e060033cc -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_21 75d6c419162b448e5a7d121fe604e3bafe7c8fc3284f277af3990f6e06c9a93d -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_22 34fa6a9bacab6411601cb536531316ade472bce58a39fc97666064a4140aeac1 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_23 c909d7995751d833b9327ad9a29579a846571c9c9053413326eebe01cc72cfbd -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_24 dd9bf41872bf67a7fd65c02b3524fc0722dc6147e05d9984f2cedb6ebb2bf3c2 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_25 71abb11ced067735a26a4d07fecbafb849b35cc4753c0cee64a98e1ad796e2b1 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_26 eae9ac4358bd6911e7cc8b26d099040172b5f80514158684018cdf9ff53b0f42 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_27 0d877a4b166f3588307d89eda9a0e1a75f905c99ca8e622ebc976d4b9764eb8c -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_28 15638554e6efacf2fc3ccccd78f8453f7b664c0696bcded61d02f7c2006a5b5c -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_29 828a78963157bb8019c22113c4b0bb013cccf5d1d4718f0b282d1da74f107274 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_30 844fae84c7e4dae7335ac8c6c8d2a64b2f75d34dd6ed40c8c644c46f45251826 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_31 93dda4638427ab5262f02d57f844098749160764dd3cea07c01c24dc471514b6 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_32 dc349ac1cb9fc25fe1d7d2dbecfba8c10ab5bf82c965e686020d5ae63a878f5a -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_33 ab95ac72feea1a20ad2da338f69cb8a37460fafae5adfe56d2dfcebde0e0f45c -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_34 779632437d1779c125d2023a671152814491293464f1d8ef468dd13f803bcc99 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_35 4aad1ddc0876d9791f6323c457bc1ccd187c604d8cc5a29bbaaf30d6fcb4ec73 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_36 8194418fcf132de5b543d62735860f7dc0e0b673536a774697ccafbaeb7272b8 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_37 22a45b83932eabd04e51e124a5d9fdf99aa90ceb1691e28ffb71e7657c442905 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_38 25c872e01644e5ea62f0ab5ec85dbe4a5aa26b98b1a4cad6bb9f863233e6c405 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_39 cbb83c863b7a378b5b10a7e37e7deef4693a63bc0c58511a639ebb0d19e60257 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_40 91ea74d7c22bcf3c809ad0c936eda1e82f264887c8afe4b2c69a10adb5859483 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_41 45b5ed80dc4e5e91b206213e3fca199f15cef9ee68a1ce819c785f127c5f8071 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_42 28613030bb713d7605c89146173a639514781a2348fc39d572ec2c53824922af -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_43 de541984d796b9e8b3de7d394cb01a23cbd8fde70a3005a55d902bb5d45352d7 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_44 f717d7708833ed400b7abf31aaadb255306f9679a154c76e8b7b7c4e5fb57394 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_45 861c0577653be69f709904e3c92bd081350388b23a1a263b7ee1dde7b1e50663 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_46 2553b0f2433e65107fd09e82cc00465983d098b0904733a730bc86001d57fe0b -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_47 450acadfe5f9733460c33b26fd800c621a27784fb3b87cc2c469e7b16783c881 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_48 dc0abd2eb5095f72b6cfa80b022a361c7e345d35f3c3c7a93fdbea6ee4157730 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_49 3cb519ddfbd08dde354b67ff00d650984f3f14394a41a103976d0c33de053ee3 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_50 4d8c8119e646aff77b13a8c6e16bc9b5483c8ed610bbde492352212bff46ddb2 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_51 a70ab3c92c952ffcec18c3f7a8d4283560cabacd3f1a949f9b163b420748c578 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_52 d18f2cf9038954eece49f5034a2b581cc7066aa8d3e459b41c5d65f4553aaa8a -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_53 75435d1194440cd6f9738a2855605430acc4b345a72cf6ba1dd86e9b88f47429 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_54 a0d721f7613b6e2ab1bab2eb2b9aa549137c9676d3259bf47c88a9b7ca10f70f -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_55 6e909cfc2a0aec1de8962ebc2a96c9e5cf5fb1366671d24051dc5fd670e966ef -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_56 3b7bf4776485d8b4cfa5d44715e51abbf243a335845bd6b59ffe80ca9e685e0c -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_57 ff1fbf834a357f1141c16fc6e6bdcbf6def114e28fa1a7c97f1f653f4f38fe92 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_58 51c258b09aca48d9908800f5fc4a167fae65379a58fac483da54d6f43bb7ebaf -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_59 facad6498f215efb4251eaa591111a76ca72c7297ef8ce0f924d4c764f76f6a9 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_60 9d14ceceeb2e7fcf5c436b1199253a6b2efc0b3bc5ba166bd627da3964edb3ef -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_61 ec1e36287a6ac3fdac1414b06f3aaaa7b6e34c33318e9fd94065d859f999c92c -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_62 f243f5c44eb29e904a7dc83d817d8096fefe8f9cdf4d7b337782d23f5a0c4e5d -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_63 3915434911b4aaac63e8692d29404598b58f9f6d4ef60b829757da3ba39df089 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_64 243c309f3708a43fc7b5c867626b41ba96371b07086cb4f837d13904256d875b -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_65 027ac7dfe927668aa8c020539eead24118c4d0158874441bcaf31a0af9e698a0 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_66 ab51607b0a4c3146bac21461327db4bef6de0f713c51e08f2e4f4fb989e7d525 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_67 dbffacc71144b4f8c8ef825c66390e6ee1df0acc01a8b03145c1e9435faacce8 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_68 0457a52657c20154a0c048afa000e423fc8a38a2f352c90cd84e87012872ae1b -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_69 2f0786f36a70c01cc0ae4a4cee7e8873a0060c4fadfa1f0e14d02165697b1c75 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_70 96ca3efabbed1a5bb9fdbbd87d97bb2716e922256e8f4ed533e04515ea3c697c -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_71 ea9fb1030fe91a7d1fdbd1571c61e63e1d7688cbc94a12df50b8917b230a8f3f -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_72 6c8a309cfc77776ba137c0b7c01433b862f1a3373f9b032dcd32365093c18005 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_73 59fae6013f15de3622dad26faa5aa13256893f330bdd1060d32c0b37f0d544af -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_74 8b5f35cf612563fdc8b038e696fce90b4f66a486040f9c02f03a5f5170cf0db9 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_75 a8fdc6317e20bd657aeb0ada63239303b5a5c404eca9a34de8c1a42f3f181ef2 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_76 369ece700537e2b1bd32cdecc027b5746a86c714b4ec0a76e7408ca06f42148f -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_77 db725de61a4f3b7f2491eff4d8d7ad8dfc1d087241379912f32564e6190557b9 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_78 f25da98d9819e722578ba4f72f49964fa4e1ad042ba260e0eae7c05f00dc4535 -->
<!-- api: F:Inno.Native.ImGui.StbrpContextOpaque.Data_79 f757a3a065d807f02d549035ea9bfb5fd5cc5e37fe483b361c57eb1d6606970a -->
<!-- api: P:Inno.Native.ImGui.StbrpContextOpaque.Data 5d7bf7f2980ebf00900f420b09a121cad8519d4012e4280253cadf2117631ad8 -->
<!-- api: M:Inno.Native.ImGui.StbrpContextOpaque.#ctor(System.Byte*) 9aa7cff8d62ee63b8d078e219aa3455ae03e6bbfaacbe431632d70d7e45f0ea4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d7c10d6b28d447b5"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2180) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fc89bf3d3556ae92"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2181) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cdb25a4db751953f"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2190) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60b30026775acc9a"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2191) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cb2148d872b2e120"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2192) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc72c35a5659af6e"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2193) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de215a55128d098c"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2194) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-70ce1fcb96b2cca3"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2195) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e46e2c9450b8aef7"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2196) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-40876e36567a5893"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2197) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-36160be99b01ee1c"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2198) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-857f64206458ff94"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2199) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-68027ef05c40e5b9"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2182) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5cc9d581d6cf9e44"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2200) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-620f58b1716f631a"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2201) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6caf70b22858be84"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2202) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54ca8d36423df09f"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2203) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a36c3b5e66e64707"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2204) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-09caa7c3edec5e2e"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2205) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7851c7ec7f591082"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2206) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6821dee13b8bfe03"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2207) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af8466a2e40a39a1"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2208) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6506598004d6ccaa"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2209) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4323538f5c3f95e4"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2183) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f193f5357d8806a"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2210) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-26699ad26247d0d6"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2211) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8abcc707e3b60ada"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2212) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0fc07d6277f87d7f"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_33`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2213) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5f25ce4577450549"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_34`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2214) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f0d5fc6fe53426ef"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_35`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2215) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-32354f5706656f78"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_36`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2216) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2633944997fc3f2e"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_37`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2217) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-277315b9cf149d2e"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_38`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2218) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6c1ed54bb19fbb3"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_39`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2219) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5165be2e841cf73b"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2184) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df71d643b22fddee"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_40`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2220) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe0a6448e3f96d87"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_41`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2221) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e5351255857d6d64"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_42`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2222) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c960be3753781f6b"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_43`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2223) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f15ca62e0ac90afe"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_44`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2224) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6de2632de637b8f2"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_45`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2225) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc8c115cd543dc25"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_46`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2226) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-73f2c9a007faacfa"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_47`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2227) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9ca5b378bda839c"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_48`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2228) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4b407c4dcd02138a"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_49`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2229) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-29108aba5f82a8c8"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2185) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bbcd464ff91d2dfe"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_50`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2230) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-50c7f0efd26f4803"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_51`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2231) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d512882918945299"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_52`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2232) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da2f10e80fad93a7"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_53`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2233) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8006924667b98d6e"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_54`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2234) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-94ed3fceae995b4d"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_55`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2235) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cdeeb81f9bd7b1ea"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_56`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2236) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e18adc72127c6dda"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_57`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2237) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aba5fec680e1a851"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_58`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2238) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bfa914e0b94c8ef8"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_59`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2239) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-38d8c64a771e7cf1"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2186) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27f18af6a57d64b6"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_60`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2240) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-442cfbe5316082d9"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_61`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2241) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1724ed9b578d230"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_62`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2242) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1914806f13dbfef7"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_63`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2243) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3468c71a5b6f2cae"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2244) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba378b98cef4c023"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_65`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2245) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c505c1dac818f11"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_66`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2246) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6bf068d15f01b741"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_67`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2247) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c05ab34ccfaa8762"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_68`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2248) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53e1ad7934c3eea3"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_69`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2249) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a81dccf89fa07070"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2187) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9dd6c0fbce6e1bd1"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_70`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2250) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f667f040a84dd6ad"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_71`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2251) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e22d1251083ad476"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_72`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2252) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-48ce43bdba907ff7"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_73`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2253) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2b2c13eca2877f5"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_74`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2254) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ef2bff71eb0451c3"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_75`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2255) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7ea9100e84d4731"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_76`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2256) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-038fea894beff485"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_77`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2257) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-81d3b81539689bc8"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_78`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2258) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-768e666ad14ea036"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_79`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2259) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58d20224d4d22d99"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2188) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9aceb6b7d904793d"></a>[`public byte Inno.Native.ImGui.StbrpContextOpaque.Data_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2189) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-322b7b6820cd0770"></a>[`public Inno.Native.ImGui.StbrpContextOpaque.StbrpContextOpaque(byte* data = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2262) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ef1815226bec7aa9"></a>[`public System.Span<byte> Inno.Native.ImGui.StbrpContextOpaque.Data { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2260) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1ceaae9266c5bc24"></a>[`public struct Inno.Native.ImGui.StbrpContextOpaque`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2178) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.StbrpNode</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.StbrpNode 67238085ec6dc3c3ecfdce6570e0f8c53b0677c6eae12d4cd23dd7c6380a2f75 -->
<!-- api: M:Inno.Native.ImGui.StbrpNode.#ctor(System.IntPtr) a67298317cf79688c95ae1cc57e0a205ce232602e03bdcad4750cf5153b8d83c -->
<!-- api: P:Inno.Native.ImGui.StbrpNode.Handle 463bb94806651fc4330d55ec38b61e0e2edfc87d9989deaf0c750a5991041174 -->
<!-- api: P:Inno.Native.ImGui.StbrpNode.IsNull 1fa12ea50b4e16f834a1141d2d950232e24c2623aa85aeacec92ad21c38a6156 -->
<!-- api: P:Inno.Native.ImGui.StbrpNode.Null 1dccc6a933e060fb8215ce9241ab59a04a0d6528e0d864b7500ae59648bd5a89 -->
<!-- api: M:Inno.Native.ImGui.StbrpNode.op_Implicit(System.IntPtr)~Inno.Native.ImGui.StbrpNode 77abe1658ffd635c8e690133c419ad6f4876abc3b3e541a699c57781cba2787c -->
<!-- api: M:Inno.Native.ImGui.StbrpNode.op_Equality(Inno.Native.ImGui.StbrpNode,Inno.Native.ImGui.StbrpNode) 126cab9728c1c27835a41a5c5ec369906ce74ef311c8804e47e1ff7e98e6108a -->
<!-- api: M:Inno.Native.ImGui.StbrpNode.op_Inequality(Inno.Native.ImGui.StbrpNode,Inno.Native.ImGui.StbrpNode) 12b7c0733f12025bb625fb9c25893dd427da981a8c465ab4225041bc7e4b5d38 -->
<!-- api: M:Inno.Native.ImGui.StbrpNode.Equals(Inno.Native.ImGui.StbrpNode) 38e2f5698399bf8842fb0e79b07cc79b6d44e1381beb8781328c86c6cb871401 -->
<!-- api: M:Inno.Native.ImGui.StbrpNode.Equals(System.Object) 38fbf6862b2e3d312f58040a1167602a0a258c72f14fc4f451aedc1e78db1147 -->
<!-- api: M:Inno.Native.ImGui.StbrpNode.GetHashCode 160ff5fda618ea7a1f2d679ae53caddf16fc25bf5cb3c381525c62530f18ea44 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c61c8d4d438baaba"></a>[`public Inno.Native.ImGui.StbrpNode.StbrpNode(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2356) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1d217b8aad7bc2dc"></a>[`public bool Inno.Native.ImGui.StbrpNode.Equals(Inno.Native.ImGui.StbrpNode other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2368) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-13ec983b500cefd4"></a>[`public override bool Inno.Native.ImGui.StbrpNode.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2369) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-6250d8fe7983e2d2"></a>[`public override int Inno.Native.ImGui.StbrpNode.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2370) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-9a07ede66423e2d7"></a>[`public static bool Inno.Native.ImGui.StbrpNode.operator ==(Inno.Native.ImGui.StbrpNode left, Inno.Native.ImGui.StbrpNode right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2366) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e6312938971919af"></a>[`public static Inno.Native.ImGui.StbrpNode.implicit operator Inno.Native.ImGui.StbrpNode(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2365) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-068f40f3044ecda8"></a>[`public static bool Inno.Native.ImGui.StbrpNode.operator !=(Inno.Native.ImGui.StbrpNode left, Inno.Native.ImGui.StbrpNode right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2367) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ffa4e1ecc150cebb"></a>[`public nint Inno.Native.ImGui.StbrpNode.Handle { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2361) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5a5058ce0be6a694"></a>[`public bool Inno.Native.ImGui.StbrpNode.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2362) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-16f4f9c55f5f3bb0"></a>[`public static Inno.Native.ImGui.StbrpNode Inno.Native.ImGui.StbrpNode.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2363) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b2a76b0fa1281055"></a>[`public readonly struct Inno.Native.ImGui.StbrpNode : System.IEquatable<Inno.Native.ImGui.StbrpNode>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2354) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.STBTexteditState</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.STBTexteditState 56fa0ce533c73802a5be8abdb78e4309f876dd4c04da1a87017d8504c8e84421 -->
<!-- api: M:Inno.Native.ImGui.STBTexteditState.#ctor(System.IntPtr) a15a89735be24ab0f970ddda7b5425a4e93abcde0a957b4746e1668042628afb -->
<!-- api: P:Inno.Native.ImGui.STBTexteditState.Handle d57972914e3dc28659467d1ff399f30ed8768c78feeba3e0fb93159345d4869b -->
<!-- api: P:Inno.Native.ImGui.STBTexteditState.IsNull 9201a454fcf180a5eb1924550c20e3dd602225873240ae71a1a5eb8844524f57 -->
<!-- api: P:Inno.Native.ImGui.STBTexteditState.Null d9ff07dc8130199e73afb49dbc0e9b364666b486067a6fdedfc47aa3ddb9fdfa -->
<!-- api: M:Inno.Native.ImGui.STBTexteditState.op_Implicit(System.IntPtr)~Inno.Native.ImGui.STBTexteditState ab6c4a92660a39c9006646756be93477ec73e1ff292153f060befa0cd0c86b7a -->
<!-- api: M:Inno.Native.ImGui.STBTexteditState.op_Equality(Inno.Native.ImGui.STBTexteditState,Inno.Native.ImGui.STBTexteditState) 5b0bcfcfdd70a036a3537b551da1b77d7ae06faf3230d058aec1da603a19622e -->
<!-- api: M:Inno.Native.ImGui.STBTexteditState.op_Inequality(Inno.Native.ImGui.STBTexteditState,Inno.Native.ImGui.STBTexteditState) 20aee5840d11f7628f77543b90045143c2fe68391fdf26a24744a9aef2b95336 -->
<!-- api: M:Inno.Native.ImGui.STBTexteditState.Equals(Inno.Native.ImGui.STBTexteditState) f1c000eec86e7779e23326b139886bbd636d75973b0f6fffb1302ca47b188c14 -->
<!-- api: M:Inno.Native.ImGui.STBTexteditState.Equals(System.Object) 20938fcc56bad0c4402fbe517d25f6cc39772fb1d68c01c6fea28e043ad10d1f -->
<!-- api: M:Inno.Native.ImGui.STBTexteditState.GetHashCode ac3bd9e1bb02296a9130ee419106f067c5ed5c3a642346bd5d574731d0f5f617 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9c81ad036945afb0"></a>[`public Inno.Native.ImGui.STBTexteditState.STBTexteditState(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6989) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7d3cbe7ce21466e2"></a>[`public bool Inno.Native.ImGui.STBTexteditState.Equals(Inno.Native.ImGui.STBTexteditState other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7001) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-aff75aeaa62396c4"></a>[`public override bool Inno.Native.ImGui.STBTexteditState.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7002) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-d5b280f151823ce2"></a>[`public override int Inno.Native.ImGui.STBTexteditState.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7003) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-6d321105f0829b44"></a>[`public static bool Inno.Native.ImGui.STBTexteditState.operator ==(Inno.Native.ImGui.STBTexteditState left, Inno.Native.ImGui.STBTexteditState right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6999) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-70fcd210b1996dfe"></a>[`public static Inno.Native.ImGui.STBTexteditState.implicit operator Inno.Native.ImGui.STBTexteditState(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6998) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-770ec50af64750d0"></a>[`public static bool Inno.Native.ImGui.STBTexteditState.operator !=(Inno.Native.ImGui.STBTexteditState left, Inno.Native.ImGui.STBTexteditState right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7000) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c66e7d68c97f5905"></a>[`public nint Inno.Native.ImGui.STBTexteditState.Handle { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6994) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bf727f85ec775dd5"></a>[`public bool Inno.Native.ImGui.STBTexteditState.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6995) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2a18a919e34bc549"></a>[`public static Inno.Native.ImGui.STBTexteditState Inno.Native.ImGui.STBTexteditState.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6996) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-941e01954f273f0a"></a>[`public readonly struct Inno.Native.ImGui.STBTexteditState : System.IEquatable<Inno.Native.ImGui.STBTexteditState>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6987) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.UserCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.UserCallback b70ae5c78570581d64f2cefdcade7d9694904f9455911a8390eda4330b3197fe -->
<!-- api: M:Inno.Native.ImGui.UserCallback.#ctor(System.Object,System.IntPtr) fdc233ceee2bbe3160fb99a8ea00644c231d093229850e8edd32958dab1dfe04 -->
<!-- api: M:Inno.Native.ImGui.UserCallback.Invoke(Inno.Native.ImGui.ImDrawList*,Inno.Native.ImGui.ImDrawCmd*) bad9a6d7f7e5ed893a45e2c29da88e47b7b96ab7d2da1e52f4b9402fcd6370c7 -->
<!-- api: M:Inno.Native.ImGui.UserCallback.BeginInvoke(Inno.Native.ImGui.ImDrawList*,Inno.Native.ImGui.ImDrawCmd*,System.AsyncCallback,System.Object) 4533cd9f248ee6e1f6728d49cf31bfb467b766fe570e59d99252070c6decd498 -->
<!-- api: M:Inno.Native.ImGui.UserCallback.EndInvoke(System.IAsyncResult) f44f6bff0914150a36f6edf57de3854daa0377268ad81b89a1f3384ee7cdd664 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-60a0605678b0cbf1"></a>[`public Inno.Native.ImGui.UserCallback.UserCallback(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10648) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b70f6114b5df2fec"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.UserCallback.BeginInvoke(Inno.Native.ImGui.ImDrawList* parentList, Inno.Native.ImGui.ImDrawCmd* cmd, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10648) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4f5f1653b97ba661"></a>[`public virtual void Inno.Native.ImGui.UserCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10648) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-687349435df5b4f7"></a>[`public virtual void Inno.Native.ImGui.UserCallback.Invoke(Inno.Native.ImGui.ImDrawList* parentList, Inno.Native.ImGui.ImDrawCmd* cmd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10648) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cb0f3ae0bc436c93"></a>[`public delegate void Inno.Native.ImGui.UserCallback(Inno.Native.ImGui.ImDrawList* parentList, Inno.Native.ImGui.ImDrawCmd* cmd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10648) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.WriteAllFn</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.WriteAllFn 5c63c421cd88025c2c8dc2c65d4d9172242e4a9e4c1edc19c3995da2aba1b8d6 -->
<!-- api: M:Inno.Native.ImGui.WriteAllFn.#ctor(System.Object,System.IntPtr) 6d1e8d20abc59254e7691a05efd7befca90f87d7c91dafb05756188fbbd1107c -->
<!-- api: M:Inno.Native.ImGui.WriteAllFn.Invoke(Inno.Native.ImGui.ImGuiContext*,Inno.Native.ImGui.ImGuiSettingsHandler*,Inno.Native.ImGui.ImGuiTextBuffer*) 126bab6e03e26b0b80be59797931cbab968e45ac5cb7a49ac72d7935ecd4e23d -->
<!-- api: M:Inno.Native.ImGui.WriteAllFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext*,Inno.Native.ImGui.ImGuiSettingsHandler*,Inno.Native.ImGui.ImGuiTextBuffer*,System.AsyncCallback,System.Object) 410e539093e0e52ae7e228b206552f711d9571b627843c06e66343cf759b7ff1 -->
<!-- api: M:Inno.Native.ImGui.WriteAllFn.EndInvoke(System.IAsyncResult) 5552a7b6a70a9592d0364ba076c73e91f36385feb84c043123b2eab6f0eca3ca -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e13d69b12ff03946"></a>[`public Inno.Native.ImGui.WriteAllFn.WriteAllFn(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10613) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a38948af56b7763c"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.WriteAllFn.BeginInvoke(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiSettingsHandler* handler, Inno.Native.ImGui.ImGuiTextBuffer* outBuf, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10613) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cab82d6d6dc1a71e"></a>[`public virtual void Inno.Native.ImGui.WriteAllFn.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10613) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dbbcd832e8ee7e5e"></a>[`public virtual void Inno.Native.ImGui.WriteAllFn.Invoke(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiSettingsHandler* handler, Inno.Native.ImGui.ImGuiTextBuffer* outBuf)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10613) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a7125271928d3652"></a>[`public delegate void Inno.Native.ImGui.WriteAllFn(Inno.Native.ImGui.ImGuiContext* ctx, Inno.Native.ImGui.ImGuiSettingsHandler* handler, Inno.Native.ImGui.ImGuiTextBuffer* outBuf)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10613) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
