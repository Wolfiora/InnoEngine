# Inno.Native.Sdl3 ABI · SDLGPUTransferBuffer → SDLJoyBatteryEvent

[项目与生命周期](../Inno.Native.Sdl3.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUTransferBuffer</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUTransferBuffer 5debc7f0ec70bce3f3a5d9c5017d922757774d5ecba0124b16ef3de93bf2b376 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBuffer.#ctor(System.IntPtr) 85538584b1eec0844bdd49a4fbd49f571fec85c30adc6d1171824a940c0e67c6 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBuffer.Handle 6a901c0b09010a7995118f13d200207fc87a42a0775d5ad97d0a5d0230907973 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBuffer.IsNull a0f35d7edb66a909b6711eb5d2569561af82afe8569eee20499388e27ce1980a -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBuffer.Null accb9d06882bdca441447824551215a8ca4e90046f02b98b1640e3c3e83e8846 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBuffer.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLGPUTransferBuffer 9f6f9ad52145f95e09b4225b2d459fa7165766050f62d8954640774d6e7f57e0 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBuffer.op_Equality(Inno.Native.Sdl3.SDLGPUTransferBuffer,Inno.Native.Sdl3.SDLGPUTransferBuffer) 3309dbcb123bb2892c3207450e747b29230d843f80946a64ed50aab5e805b038 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBuffer.op_Inequality(Inno.Native.Sdl3.SDLGPUTransferBuffer,Inno.Native.Sdl3.SDLGPUTransferBuffer) 400de771513d619ae5b90f286bf04867219d0e270a4479c58d81ed3476192bb5 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBuffer.Equals(Inno.Native.Sdl3.SDLGPUTransferBuffer) 974634020cdad47cb6fbf2a6a00765a97dc3ec8cbfa4e4c13de6a3e584c5be62 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBuffer.Equals(System.Object) d2247aaad7de071ac4799166eda9f7fb4ef8de0de09f27d7a0501cdebc48e139 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBuffer.GetHashCode 32db2358327274d8d997e193035c26b54c537e0018b070ad32c74ea43a250839 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-de0bf72c50d30f32"></a>[`public Inno.Native.Sdl3.SDLGPUTransferBuffer.SDLGPUTransferBuffer(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4487) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d26b9cc57cad9d1e"></a>[`public bool Inno.Native.Sdl3.SDLGPUTransferBuffer.Equals(Inno.Native.Sdl3.SDLGPUTransferBuffer other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4499) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-d974b738b4e5ce72"></a>[`public override bool Inno.Native.Sdl3.SDLGPUTransferBuffer.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4500) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-c654587dbb334e04"></a>[`public override int Inno.Native.Sdl3.SDLGPUTransferBuffer.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4501) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-23ac4f6a9dd8d266"></a>[`public static bool Inno.Native.Sdl3.SDLGPUTransferBuffer.operator ==(Inno.Native.Sdl3.SDLGPUTransferBuffer left, Inno.Native.Sdl3.SDLGPUTransferBuffer right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4497) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bc7982325076e34e"></a>[`public static Inno.Native.Sdl3.SDLGPUTransferBuffer.implicit operator Inno.Native.Sdl3.SDLGPUTransferBuffer(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4496) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-708a2183b627851b"></a>[`public static bool Inno.Native.Sdl3.SDLGPUTransferBuffer.operator !=(Inno.Native.Sdl3.SDLGPUTransferBuffer left, Inno.Native.Sdl3.SDLGPUTransferBuffer right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4498) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c5530615d8400864"></a>[`public nint Inno.Native.Sdl3.SDLGPUTransferBuffer.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4492) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b48b310eccc81b5d"></a>[`public bool Inno.Native.Sdl3.SDLGPUTransferBuffer.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4493) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f4cb119eb9cbb7e0"></a>[`public static Inno.Native.Sdl3.SDLGPUTransferBuffer Inno.Native.Sdl3.SDLGPUTransferBuffer.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4494) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-22442a66f3dae278"></a>[`public readonly struct Inno.Native.Sdl3.SDLGPUTransferBuffer : System.IEquatable<Inno.Native.Sdl3.SDLGPUTransferBuffer>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4485) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo f035ae103818c9ac9c8f26482e791960ec3a606efb5545b334110e8a342b248a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo.Usage c30878205061a7e76c61278817e9a54edd1efd3b9fac805687ec1b09719cdb62 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo.Size 87ba0d023c29f0899d847b56eba0c1311d3448975bec3334b7a75106852b843b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo.Props cd43f34058ce07aabb97ec937e30c3a3fbb13ecd7db271cb76bce74e2ff38a2e -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo.#ctor(Inno.Native.Sdl3.SDLGPUTransferBufferUsage,System.UInt32,System.UInt32) 9468757717a6e9f30774155ce967c73a2df1e7bb39fa23c474f8d4b0a52f532b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e3b696cbb88574e3"></a>[`public uint Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo.Props`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5205) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0abb438f181054b0"></a>[`public uint Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo.Size`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5204) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e07ceeae527dbec"></a>[`public Inno.Native.Sdl3.SDLGPUTransferBufferUsage Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo.Usage`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5203) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e04ca2e98e269f57"></a>[`public Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo.SDLGPUTransferBufferCreateInfo(Inno.Native.Sdl3.SDLGPUTransferBufferUsage usage = Inno.Native.Sdl3.SDLGPUTransferBufferUsage.TransferbufferusageUpload, uint size = 0, uint props = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5206) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-43036f8950f96c53"></a>[`public struct Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5201) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr ca674fe02ef18ebff14aa6d408dabc82d5b747a2b4eedb5af055a0ab5cffff42 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.#ctor(Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo*) db1718cca557a5c169b77b1dc13f5b5889c9e65c5cef65f3f8a95b06d55a5c75 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Handle 1082e9183eb3b27874bf5221dc4c9ef52497c6a6344081a90339c1ae4ee17c6a -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.IsNull 0da00d2e5dbd554a466cf0c64c3e5eececeadd4375d00d23c71c4d1575d55fe8 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Null b7684cf07ea81e898727c1ccc157a6f33d7c8f3d6e2800a2172e9bb1e5532e58 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Item(System.Int32) ef0f6b2fa322c87851a262c87a4cb3b3d663eaecaebc23aa4810a464b3692389 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo*)~Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr ba4cdf655fcb82787fa6ab12c4099407f57e3700ad5bdb8fde7c3565326ec504 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr)~Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo* 43d3b85e9f409282e8dd5af06ff52711ced328beef3bfcd4855e0ce2eaa9d31c -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.op_Equality(Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr,Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr) 830dd7eba5388462d5f0317eddf442c8aedbda91d0d7ef416594294640288fac -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr,Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr) bc7b780ba78927f91144062b18e2390f4e2304857a0491ffd921e6414ceb252f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr) c9b70d180675bed6d7d839ff242888748b71139f890d741d5d019c4c7587fe69 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Equals(System.Object) bed40c969a2d1ab9b4394896a3b78e277044ae5933af75ca46480f25357ae8db -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.GetHashCode 15d0178b0934bef62d8f023df5be7955ddfe49f13e68d7d029b7922c285ff6c3 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Usage 2839e01bb69ef1b7576f0d5f362bdb457ccc14e4c72e87da66065470dc88e0f4 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Size 126b7dd7565dd327add821690fca57adeca0cefaef18b15c9d324dd73b90d004 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Props 5e4e8e23236644a0eac65b0a23eea467ede5f044ca20708c5362c52164458ea5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-59536afcaa3c60c0"></a>[`public Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo* Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9056) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-34915906da1d635d"></a>[`public Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.SDLGPUTransferBufferCreateInfoPtr(Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9051) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a069eaddbed7db8f"></a>[`public bool Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9066) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-ae77074c804f0bd4"></a>[`public override bool Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9067) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-b3f2d6d71e71b053"></a>[`public override int Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9068) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-0af61d2e25da68b2"></a>[`public static bool Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.operator ==(Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr left, Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9064) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6fa4d537a823d337"></a>[`public static Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr(Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9062) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e3f92d89770d685c"></a>[`public static Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo*(Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9063) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aa73e7df6c6d84f7"></a>[`public static bool Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.operator !=(Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr left, Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9065) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f81684f2fc00f999"></a>[`public bool Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9057) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-65953335b3377ff1"></a>[`public Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfo Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9060) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c9676a7175494d84"></a>[`public static Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9058) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-51018840b37e6878"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Props { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9072) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-93c3219f0a1c50e1"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Size { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9071) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-134de080b6f43662"></a>[`public ref Inno.Native.Sdl3.SDLGPUTransferBufferUsage Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr.Usage { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9070) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d84c35b17f2d77f6"></a>[`public struct Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUTransferBufferCreateInfoPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9049) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUTransferBufferLocation</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUTransferBufferLocation 77a956eb69e9be344bf87a17cc376cacca9f64c3d95cd65af8336fd8c1c884bc -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUTransferBufferLocation.TransferBuffer 844b2bc1e7d8c0b9ba871372d124905665964e55eae32ab2d55974c9ee9885d5 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUTransferBufferLocation.Offset 435472b13aba304f7510a20654ae61ed444605fa0e2bdbb0ad877bc04c18bfb8 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferLocation.#ctor(Inno.Native.Sdl3.SDLGPUTransferBuffer,System.UInt32) 69e0ef236f1e9f181af326e43bed05a0ddda72c7adff05fadb1d6287ee4ebc8c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-29951865ab3e6169"></a>[`public uint Inno.Native.Sdl3.SDLGPUTransferBufferLocation.Offset`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4794) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d93166b0f590dce2"></a>[`public Inno.Native.Sdl3.SDLGPUTransferBuffer Inno.Native.Sdl3.SDLGPUTransferBufferLocation.TransferBuffer`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4793) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-534aacb9e1f8d3ba"></a>[`public Inno.Native.Sdl3.SDLGPUTransferBufferLocation.SDLGPUTransferBufferLocation(Inno.Native.Sdl3.SDLGPUTransferBuffer transfer_buffer = default(Inno.Native.Sdl3.SDLGPUTransferBuffer), uint offset = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4795) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8ab359cc8b6ffa34"></a>[`public struct Inno.Native.Sdl3.SDLGPUTransferBufferLocation`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4791) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr 63248175f59ccc1936ce5ffe295ec91e2f45b637a84036da9779a48a63065f03 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.#ctor(Inno.Native.Sdl3.SDLGPUTransferBufferLocation*) 4df573372fe94d777737c5a67d7b041fd27fce3176265af4cd7bf4a00ef33785 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.Handle 2142be3aa182a9a257269d296292a50235c30b3345fa0f8fcda38bbca31e3258 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.IsNull 6fecd22ef8d72e1bc27e5b1db2b51ff811b9ef83a38a88a8816f72b0fde0f990 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.Null 07c1f20b0afd60954f27f0ebdbd973ca31715e46d65802a528646eaad22343bc -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.Item(System.Int32) 53c7f34c3b6bfbf007d038068808a446760e9b3a4748124de16ab5c2598867d9 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUTransferBufferLocation*)~Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr f68beef99fa8f2c5d08a11a27213925c80c0e956f7e92ef4b979b95dd42f0025 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr)~Inno.Native.Sdl3.SDLGPUTransferBufferLocation* bb94aff09206350ba9210b304f43e266abae1e3a237fa8daaab14bfd3bac9d57 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.op_Equality(Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr,Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr) 678add87a8e8fba9817c62e29d3fefef693c662998ceeb0e796448be524152e1 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr,Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr) 6a32a8087e4a925d971e9d9258bb725781df5681a606c4a76b9fee647d31a5f0 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.Equals(Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr) 5fa7300dfb8aaa0a88005ec0fe73c6e7d6c693cc910304b6aaff9d3528b544c4 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.Equals(System.Object) 01d74a4b12574a96f426da095fa0a30b273f3e26a245f705498dd194785b3d36 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.GetHashCode 0fd4d904113028f07b501b1d0bd192b1a2b43e28acfa9e050562927eb8c8325a -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.TransferBuffer 561285a5601ea74f2373e6f4b84b6c28b0f47f41e663ec308a5d7ff6eb988a00 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.Offset d28489907e28b1ef7abfa36d3a85058a62662bd8d984808f9ca107654de715b5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a7d8e7412f8222d3"></a>[`public Inno.Native.Sdl3.SDLGPUTransferBufferLocation* Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9083) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f4d49a64278f90bb"></a>[`public Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.SDLGPUTransferBufferLocationPtr(Inno.Native.Sdl3.SDLGPUTransferBufferLocation* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9078) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2021bbe4280a3eb9"></a>[`public bool Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.Equals(Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9093) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-215ed4beffc8e77b"></a>[`public override bool Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9094) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-f80c7a011dea4af8"></a>[`public override int Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9095) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-5358a980deb96855"></a>[`public static bool Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.operator ==(Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr left, Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9091) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-590de50437527fba"></a>[`public static Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.implicit operator Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr(Inno.Native.Sdl3.SDLGPUTransferBufferLocation* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9089) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-88aa23aa506dad07"></a>[`public static Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.implicit operator Inno.Native.Sdl3.SDLGPUTransferBufferLocation*(Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9090) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-314d975a79c9c7b6"></a>[`public static bool Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.operator !=(Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr left, Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9092) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-053569acbf077757"></a>[`public bool Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9084) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1214234a7b9eee79"></a>[`public Inno.Native.Sdl3.SDLGPUTransferBufferLocation Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9087) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a89c5269fa70110e"></a>[`public static Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9085) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dcc4e8f1769ae4cf"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.Offset { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9098) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-73bdb353acf357de"></a>[`public ref Inno.Native.Sdl3.SDLGPUTransferBuffer Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr.TransferBuffer { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9097) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8a27aba553bc63d9"></a>[`public struct Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUTransferBufferLocationPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9076) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUTransferBufferUsage</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUTransferBufferUsage 8bdc4eaecf394f0bb8e9652ae4a8140336cb1957a8fd1b887a3208b4a770fdb7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUTransferBufferUsage.TransferbufferusageUpload 3bc1c59fe62ebf18db2041bd4881517e71c87fc7f8b6add0660db048e2f9eea5 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUTransferBufferUsage.TransferbufferusageDownload f291a9b2180912997ef955ec08c69a920079ee406c26f8b4cd78f80c11f86a76 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0d5ebdabfa59af7a"></a>[`Inno.Native.Sdl3.SDLGPUTransferBufferUsage.TransferbufferusageDownload = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1538) | 当前常量或枚举值：`1`。 | — |
| <a id="api-6a311b216212d4c2"></a>[`Inno.Native.Sdl3.SDLGPUTransferBufferUsage.TransferbufferusageUpload = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1537) | 当前常量或枚举值：`0`。 | — |
| <a id="api-c53ab6bd125ec080"></a>[`public enum Inno.Native.Sdl3.SDLGPUTransferBufferUsage : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1535) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUVertexAttribute</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUVertexAttribute f98aec4f288870ef1e6fd37048f5a15258d1460ce1c7f3e090daecd0d12e2f66 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexAttribute.Location 986a2dda907e505280b3103f8464a596c356b37836b986798aec64536a76c9f5 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexAttribute.BufferSlot 4fa8c1304be0637f20cc3a98af9655ddbfcab777466d34124fd036a4bea40105 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexAttribute.Format f10b674d0c45468d3d0ee6a38c32ef24ca4a4b4d21c14b6de929af36fa4bce16 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexAttribute.Offset 0c03169df6f8abb7ff17829e0b5a275e80d22e949ce230481f9dab4652c1fddd -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexAttribute.#ctor(System.UInt32,System.UInt32,Inno.Native.Sdl3.SDLGPUVertexElementFormat,System.UInt32) de512d6e4ce2c9530bdb8d6728b5a8d65fe69cfb00ff3f68815a134b78149e7c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-caa4183f2f1fd3ff"></a>[`public uint Inno.Native.Sdl3.SDLGPUVertexAttribute.BufferSlot`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5037) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e528860fb2b6a1b"></a>[`public Inno.Native.Sdl3.SDLGPUVertexElementFormat Inno.Native.Sdl3.SDLGPUVertexAttribute.Format`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5038) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-67b6ff600669359c"></a>[`public uint Inno.Native.Sdl3.SDLGPUVertexAttribute.Location`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5036) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-950fe4ef656fc910"></a>[`public uint Inno.Native.Sdl3.SDLGPUVertexAttribute.Offset`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5039) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a417732de4861998"></a>[`public Inno.Native.Sdl3.SDLGPUVertexAttribute.SDLGPUVertexAttribute(uint location = 0, uint buffer_slot = 0, Inno.Native.Sdl3.SDLGPUVertexElementFormat format = Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatInvalid, uint offset = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5040) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6d3952d0b39f6580"></a>[`public struct Inno.Native.Sdl3.SDLGPUVertexAttribute`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5034) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUVertexAttributePtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUVertexAttributePtr 97042788a8fa490a9d6b7304dea19953341ca1c97914cafc37d711e804d1cff8 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.#ctor(Inno.Native.Sdl3.SDLGPUVertexAttribute*) 68250091ab8689ec1c7145ac560aa8029ba23c24243c0da0c7f66d96c8016ac2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Handle 80d32388833316d4dae9dc5ce8503850bf4ee8972a62d3deb3eddb86964da394 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.IsNull 6915509ea040c524bcc1e861f9093b2d7a559f3c9ac6a029327a531551519f5a -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Null 379afc4427fdf1cbec84254ab88bc57fa7e3e796f3c2b4befadf92e345a2afb1 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Item(System.Int32) 7e6164690207b0f1b9266673441bf1ab9f2457c359c04778a0a81b779d9fa332 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.op_Implicit(Inno.Native.Sdl3.SDLGPUVertexAttribute*)~Inno.Native.Sdl3.SDLGPUVertexAttributePtr 952e39c240c89a363188307b05ea8f0bc822ac2eb0ed5b1b88e38bb4a51660ad -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.op_Implicit(Inno.Native.Sdl3.SDLGPUVertexAttributePtr)~Inno.Native.Sdl3.SDLGPUVertexAttribute* 99a962f6d97f286e7a455bf4e7cb39351d56be59924d2918d0712b6d8d7555a6 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.op_Equality(Inno.Native.Sdl3.SDLGPUVertexAttributePtr,Inno.Native.Sdl3.SDLGPUVertexAttributePtr) eaeb450967607179cb0a525a2530d9638ebc365b532dbe92d7e4cad8a2e785fa -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.op_Inequality(Inno.Native.Sdl3.SDLGPUVertexAttributePtr,Inno.Native.Sdl3.SDLGPUVertexAttributePtr) f06b9e378808e088ef766ba5e60f6230b4f8dfde0e18e93bc3d8c17983d885b8 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Equals(Inno.Native.Sdl3.SDLGPUVertexAttributePtr) f5ed08ecbe9cafd3a9c44e37654aee32b00e0cd2dee224c23683c4898477ef25 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Equals(System.Object) abee8f034f3dcf108bb9a5826f87d75e424f2dca6491e9025bdf1580bbf24e5f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.GetHashCode 24b9b307773606a4f24bfebe60df0d6e77dc5b072ced9b160716384b03cca07a -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Location de232233dcca1f107b0b56c890d2d24633ff40ac0c083decab894a4a2c89b9d1 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.BufferSlot efa353b44c36f3abc183f508520b19b8560fe31b15281c1e1a6fe553a0ef5022 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Format 133bc252b97cc377233b907fe5c20c62e4e443bcebd975cbfd59b9ea311f3fa5 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Offset 89b9449e6d0e5d6cc6ed496d2c72740d1e8b3299d5e914c65b79e99b06aa72b2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4a58a844970c371c"></a>[`public Inno.Native.Sdl3.SDLGPUVertexAttribute* Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9109) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-618eb38cfd9de23d"></a>[`public Inno.Native.Sdl3.SDLGPUVertexAttributePtr.SDLGPUVertexAttributePtr(Inno.Native.Sdl3.SDLGPUVertexAttribute* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9104) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-55b36c3c45a36d25"></a>[`public bool Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Equals(Inno.Native.Sdl3.SDLGPUVertexAttributePtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9119) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-2d0dd933870ee3c9"></a>[`public override bool Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9120) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-07ef1cd6e168a3cb"></a>[`public override int Inno.Native.Sdl3.SDLGPUVertexAttributePtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9121) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-22a794cbfbef781b"></a>[`public static bool Inno.Native.Sdl3.SDLGPUVertexAttributePtr.operator ==(Inno.Native.Sdl3.SDLGPUVertexAttributePtr left, Inno.Native.Sdl3.SDLGPUVertexAttributePtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9117) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1beb4c6875a376a9"></a>[`public static Inno.Native.Sdl3.SDLGPUVertexAttributePtr.implicit operator Inno.Native.Sdl3.SDLGPUVertexAttributePtr(Inno.Native.Sdl3.SDLGPUVertexAttribute* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9115) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-63772dba8192d173"></a>[`public static Inno.Native.Sdl3.SDLGPUVertexAttributePtr.implicit operator Inno.Native.Sdl3.SDLGPUVertexAttribute*(Inno.Native.Sdl3.SDLGPUVertexAttributePtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9116) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3affa41fb61615d5"></a>[`public static bool Inno.Native.Sdl3.SDLGPUVertexAttributePtr.operator !=(Inno.Native.Sdl3.SDLGPUVertexAttributePtr left, Inno.Native.Sdl3.SDLGPUVertexAttributePtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9118) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b0701688c5d57a62"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUVertexAttributePtr.BufferSlot { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9124) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bf88b505aa637c8b"></a>[`public ref Inno.Native.Sdl3.SDLGPUVertexElementFormat Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Format { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9125) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b902cf74080cc8e9"></a>[`public bool Inno.Native.Sdl3.SDLGPUVertexAttributePtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9110) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ccc09feba3f592dc"></a>[`public Inno.Native.Sdl3.SDLGPUVertexAttribute Inno.Native.Sdl3.SDLGPUVertexAttributePtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9113) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9c81d342537049fb"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Location { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9123) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4afa8808cf015f61"></a>[`public static Inno.Native.Sdl3.SDLGPUVertexAttributePtr Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9111) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5614cff0e4e28ac4"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUVertexAttributePtr.Offset { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9126) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-de1373daf3bd25e7"></a>[`public struct Inno.Native.Sdl3.SDLGPUVertexAttributePtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUVertexAttributePtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9102) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUVertexBufferDescription</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUVertexBufferDescription bbb5c72cc48863e5af5cdec66de1a62cbcaa79457a9d2d075de30f6a0da55fa7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexBufferDescription.Slot d5eccfa2e928aa3315663efe9e1c8003559f3172e5c915948ef156034de0cbde -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexBufferDescription.Pitch 774fdd80c2a32edabfc688e015ef768c64489b4d686760b49585df37ada205fa -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexBufferDescription.InputRate 81a5052660c84ecaaf2dd4f64ec6c0d2827c5137230a54485655c9f6053021d1 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexBufferDescription.InstanceStepRate ae578352cc9de7d7a00d6b8ef280556b4599f97d88c86bd9b22bb126d6fef034 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexBufferDescription.#ctor(System.UInt32,System.UInt32,Inno.Native.Sdl3.SDLGPUVertexInputRate,System.UInt32) 6f7821a1c82e25bb84ffea716e8a0870b4ed6d9a010c2989f538e65800ce6fa0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-80e7ed560823a4cf"></a>[`public Inno.Native.Sdl3.SDLGPUVertexInputRate Inno.Native.Sdl3.SDLGPUVertexBufferDescription.InputRate`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5019) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d7ca0135cb71a92b"></a>[`public uint Inno.Native.Sdl3.SDLGPUVertexBufferDescription.InstanceStepRate`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5020) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eded3a6640dabd8b"></a>[`public uint Inno.Native.Sdl3.SDLGPUVertexBufferDescription.Pitch`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5018) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a418c607fe0b7c21"></a>[`public uint Inno.Native.Sdl3.SDLGPUVertexBufferDescription.Slot`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5017) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-56c6f2087208d2c4"></a>[`public Inno.Native.Sdl3.SDLGPUVertexBufferDescription.SDLGPUVertexBufferDescription(uint slot = 0, uint pitch = 0, Inno.Native.Sdl3.SDLGPUVertexInputRate input_rate = Inno.Native.Sdl3.SDLGPUVertexInputRate.VertexinputrateVertex, uint instance_step_rate = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5021) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2fac27dca3334f56"></a>[`public struct Inno.Native.Sdl3.SDLGPUVertexBufferDescription`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5015) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr d3ef6d42e6ad3930bb8ba14204644a4499ec83a7bb65dc98316ee6b8ef407479 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.#ctor(Inno.Native.Sdl3.SDLGPUVertexBufferDescription*) 137bc550671d39046f8d2c5fb1867977e95cb419338523b6a5066daff7758e81 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.Handle e1b5c7c3baa5b98b06aab0dac2948e75496d04fe0c3181e065de888390fe40d4 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.IsNull 546fa4452f229a9203f13ae35bdcb290fc936be7b61ff6cd042c0c15c09384c4 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.Null 848c33d39821a143c5e7f9935650b64b1fb8656dbd1f9c77a2dfd4fb53bf5269 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.Item(System.Int32) 247cbc3de79adac69c4671103afd0d14ec37e18a99103197b42353f25c2f7430 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUVertexBufferDescription*)~Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr 311897d548824ced596c62591c46ec175578b6b8d949bca2108e87006c0beef1 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr)~Inno.Native.Sdl3.SDLGPUVertexBufferDescription* 5020f1db0e20ff264cc6ffbdaa0451e8515284aaf7accad1ed7fc3fb3037c029 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.op_Equality(Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr,Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr) 5ced9d0376745052ba58830666d4b67d28a2c16551b8addf6eb37c44e1a080af -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr,Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr) 3e9fc032f40e33b3a3fb39cdbe41f13e7f109be9241b7575ce33e110194d1f23 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.Equals(Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr) 2d2efe63e07951f45d47cef01b612f69349031a718838009955e62086953fe8d -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.Equals(System.Object) 8d4ca685578cc516b6a07a2a6ef3f7ff1376ae3b06dceb2b345633ae5e4ccebb -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.GetHashCode 6d0e08b98088b5d9252baa5f988c521a5dbbe98d18442e00aab50c7f0f28c13b -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.Slot a32e54fce1a4341e2c7d6f91eed41d9db2261296eb99f306ac451cc96d85b80b -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.Pitch 4c7df93ee89a2ba701795db27fede6907336b64bea808706db3d487fb4fce007 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.InputRate acb462058e8549c797921d4bd3cc4d44a1afb2162b4ab1380c77abd853bfebd1 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.InstanceStepRate 01acd63a8cdebbbde769fce8a595b450536ad0f2e42bf5f3045d0ed51962ac60 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f98d94dbd4ab7c71"></a>[`public Inno.Native.Sdl3.SDLGPUVertexBufferDescription* Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9137) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb56a177c1ac0783"></a>[`public Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.SDLGPUVertexBufferDescriptionPtr(Inno.Native.Sdl3.SDLGPUVertexBufferDescription* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9132) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5a6a9d0411939f82"></a>[`public bool Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.Equals(Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9147) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-db57a36a3a0879f1"></a>[`public override bool Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9148) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-cea14c0c5bfed543"></a>[`public override int Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9149) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-7189b62a8f6f15cf"></a>[`public static bool Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.operator ==(Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr left, Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9145) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5fc7a93ef8495af7"></a>[`public static Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.implicit operator Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr(Inno.Native.Sdl3.SDLGPUVertexBufferDescription* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9143) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f2069a15d4dafa8e"></a>[`public static Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.implicit operator Inno.Native.Sdl3.SDLGPUVertexBufferDescription*(Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9144) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9c033a926183b927"></a>[`public static bool Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.operator !=(Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr left, Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9146) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-096c481cfe5a7939"></a>[`public ref Inno.Native.Sdl3.SDLGPUVertexInputRate Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.InputRate { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9153) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-10f01215b36eea65"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.InstanceStepRate { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9154) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-508349f2bf1eb2c1"></a>[`public bool Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9138) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0d44d4c69e5ed5b6"></a>[`public Inno.Native.Sdl3.SDLGPUVertexBufferDescription Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9141) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4d23a4a396bc8318"></a>[`public static Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9139) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-52f2e95303d4fa50"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.Pitch { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9152) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1c066c36df78ecbc"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr.Slot { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9151) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d9d7d3183664d156"></a>[`public struct Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUVertexBufferDescriptionPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9130) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUVertexElementFormat</code> · 32 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUVertexElementFormat 6fd10ae80b7670a9e95bcd5dabe2e02646b61d50e4f0374ed2b380156ab24ea8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatInvalid 9564b04148d7a710770fda8f1cd20c46e5a81048fe61aa6070315780b0fd141b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatInt 9ff3c6d3bdd77f46bf28966e75c338c54c82a7f4536007f8c10cd0324ae6840a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatInt2 49d52e7ff886d03d3a6a831deea5bbd90366395ca14696e43e3ab3e719420f23 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatInt3 4304f37476f795c9eb88bc80aa500de2b0429d3ef8386ef5793f955b6129f43a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatInt4 a689cdcfcfd3fdc04da86b95f89cde6d833ae0ffdd569ddf81165d6ba0345928 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUint fb3b004e0ed0cbb3fcc6ab099904a9bea60a1edec0fa56ed60db199ede94b202 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUint2 6aa22e0969f8e5c5951d6f008946006677c11cb874fa4cb2d96fadd68cf05dc5 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUint3 d53dae49032f0267661bd05746ea193b33daf75a3cd637bfb2de41884506e9b7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUint4 0c1278fcc8abb26fa09d630abb46fb748974013ccd0a64a68f61587520c76d29 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatFloat 1ddc15e55d5d639c29952ba10fba1283ea54d55333895d9041988e9953adcec1 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatFloat2 4e94a3502ba1aff834eb4c7c2bc190cc59f0ad1400eb80e57095d9e5bb949c4f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatFloat3 e9775e578bcd3752a289a468896651ea531b7939a78b765be34784571e2d393a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatFloat4 d4a88a37c420a20a243b8c173b9c0c91619dd0473fdeb036340abec52ac83c4a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatByte2 10c8dcbc1195f8c12ae6b804befa5b75028b30f840d4dff8ff1f1cd757c39f74 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatByte4 602aa780c42e9dc3ca533f0d5fa52ff0bf93103268517ca50f69294ebe8ed244 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUbyte2 6476595fb02d69e15abb59080907f5eea002e712545bb9c17fbf3c859b6e7bec -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUbyte4 9be43658de2e97233f598cf03644ab0c951e4d8e7df0b0fb33abc2e5c41d9bbd -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatByte2Norm a7abbfda4e53b4de595705c80ed239d7d5379360d407683d1aae7b96b263beb4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatByte4Norm db9741e2c89e5a90d9a2ddaeb1f4b8140a097a3be24e023eff41d74729c04445 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUbyte2Norm 221166b154cf10295d0366639bd22f79ed64c8731a95e12c45750126e12268f1 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUbyte4Norm 6ea82d7d045f2839b14fbb17e5cf3e3c120db714230579bab8c655794789d173 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatShort2 98d21f8cb817544de55d565d6b320897356708a281f05c3fe004e8539b82d932 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatShort4 bd9c2793422724ccf093ccb11e555fcbde7b2e7143c6057b49b60dfcc02f02df -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUshort2 849d1b447afbd6771a968f3c877bbf928105bf758ad6a80074fa17a8212aab54 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUshort4 2b14d0503c83ac45127da465b7fadfd2d920a185d90460ced9fbed858944f4fa -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatShort2Norm 645c32b7d17d1e3076e81349df168913cf7820e86df63d1bbd12c4531432e807 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatShort4Norm d3201693a4ccac07bda29eaf7926fcbfaff31f7e2cb9a150b4d4f3cec5104db4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUshort2Norm cd96ab47d53a7e93f0edb11e28a23ae45c3a25649cc138757172b2a915afdbfb -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUshort4Norm e4bfec1a72a0e7c720e40aa6054c13038743f1eb1cd0e159baeb0b9bf43e705f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatHalf2 7b246da8ab8343a02764fa64e446e54333d661efa1ecef77a8ba185a17649ad4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatHalf4 80c868b1264bb0fb7f67ef974a472a8c1e471037574e81fa408c08b1349cb659 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-84e6e506bdebb36f"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatByte2 = 13`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1568) | 当前常量或枚举值：`13`。 | — |
| <a id="api-cd3a1a5b7869fe00"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatByte2Norm = 17`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1572) | 当前常量或枚举值：`17`。 | — |
| <a id="api-089c045f80eda6ea"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatByte4 = 14`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1569) | 当前常量或枚举值：`14`。 | — |
| <a id="api-6c606c06235f3e83"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatByte4Norm = 18`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1573) | 当前常量或枚举值：`18`。 | — |
| <a id="api-9af499045590842d"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatFloat = 9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1564) | 当前常量或枚举值：`9`。 | — |
| <a id="api-5fc11999e6929bd8"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatFloat2 = 10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1565) | 当前常量或枚举值：`10`。 | — |
| <a id="api-4ba3f31eda99f13e"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatFloat3 = 11`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1566) | 当前常量或枚举值：`11`。 | — |
| <a id="api-a3fffce71d46acfd"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatFloat4 = 12`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1567) | 当前常量或枚举值：`12`。 | — |
| <a id="api-3ebbf0d619b2a8cd"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatHalf2 = 29`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1584) | 当前常量或枚举值：`29`。 | — |
| <a id="api-4e6920010c343856"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatHalf4 = 30`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1585) | 当前常量或枚举值：`30`。 | — |
| <a id="api-233a2e55a26799ff"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatInt = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1556) | 当前常量或枚举值：`1`。 | — |
| <a id="api-a45584ee4e61537e"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatInt2 = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1557) | 当前常量或枚举值：`2`。 | — |
| <a id="api-e999d1a608157fd8"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatInt3 = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1558) | 当前常量或枚举值：`3`。 | — |
| <a id="api-70967adbfcec162a"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatInt4 = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1559) | 当前常量或枚举值：`4`。 | — |
| <a id="api-dc73bd7aa933919e"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatInvalid = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1555) | 当前常量或枚举值：`0`。 | — |
| <a id="api-2c80d9f77d43aacc"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatShort2 = 21`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1576) | 当前常量或枚举值：`21`。 | — |
| <a id="api-41eb3fefdddec5dc"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatShort2Norm = 25`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1580) | 当前常量或枚举值：`25`。 | — |
| <a id="api-0a124c5340976312"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatShort4 = 22`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1577) | 当前常量或枚举值：`22`。 | — |
| <a id="api-2140e8d6fbeab79f"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatShort4Norm = 26`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1581) | 当前常量或枚举值：`26`。 | — |
| <a id="api-f88cc4ab6f77b5d3"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUbyte2 = 15`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1570) | 当前常量或枚举值：`15`。 | — |
| <a id="api-78daf5327895b3f6"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUbyte2Norm = 19`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1574) | 当前常量或枚举值：`19`。 | — |
| <a id="api-5f92b69e86096072"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUbyte4 = 16`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1571) | 当前常量或枚举值：`16`。 | — |
| <a id="api-dc477b5a92a49271"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUbyte4Norm = 20`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1575) | 当前常量或枚举值：`20`。 | — |
| <a id="api-268b47b6c7e461b5"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUint = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1560) | 当前常量或枚举值：`5`。 | — |
| <a id="api-6b670f25a591b1d1"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUint2 = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1561) | 当前常量或枚举值：`6`。 | — |
| <a id="api-2281f730f79d48a5"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUint3 = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1562) | 当前常量或枚举值：`7`。 | — |
| <a id="api-019b8b5a562b59b7"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUint4 = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1563) | 当前常量或枚举值：`8`。 | — |
| <a id="api-4857e42305ba7e66"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUshort2 = 23`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1578) | 当前常量或枚举值：`23`。 | — |
| <a id="api-6cecf6e71f66dcb8"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUshort2Norm = 27`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1582) | 当前常量或枚举值：`27`。 | — |
| <a id="api-29d1847e32c9d88b"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUshort4 = 24`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1579) | 当前常量或枚举值：`24`。 | — |
| <a id="api-16ce1063a55f865a"></a>[`Inno.Native.Sdl3.SDLGPUVertexElementFormat.VertexelementformatUshort4Norm = 28`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1583) | 当前常量或枚举值：`28`。 | — |
| <a id="api-67d7d2e59b9b58d4"></a>[`public enum Inno.Native.Sdl3.SDLGPUVertexElementFormat : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1553) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUVertexInputRate</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUVertexInputRate 73879461aff0aa81b43d9869b3396e2c720fdb1959b6b1aa639e2d7ed5769d41 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexInputRate.VertexinputrateVertex 4f654dbbaede309aaae85e5b23648404b541f5f6302d22f2ebf02235c77ef657 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexInputRate.VertexinputrateInstance 5361ba32bfff57da1f601d30a126109d3c417b69d011bd6ea3fbaeaf7fd8905c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-11737d39210d8c17"></a>[`Inno.Native.Sdl3.SDLGPUVertexInputRate.VertexinputrateInstance = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1594) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d5a89ce704a09ead"></a>[`Inno.Native.Sdl3.SDLGPUVertexInputRate.VertexinputrateVertex = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1593) | 当前常量或枚举值：`0`。 | — |
| <a id="api-a50a05cca6aceb79"></a>[`public enum Inno.Native.Sdl3.SDLGPUVertexInputRate : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1591) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUVertexInputState</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUVertexInputState 65a3f27d519805950e94986b199c92b3daa58003ce8f586cf756dcf6a3814a22 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexInputState.VertexBufferDescriptions d00fa825ad9e8f0439b42d2d35e124ce93ec07d6e5420da1a9e0433be542f49a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexInputState.NumVertexBuffers b9fc067eb5986348b1aeb4fbfa6e967019d7bd3b6a3810aca716b2e93d325f7f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexInputState.VertexAttributes b907b1b574446ec27cad6a01504b3d574f782966c3974a1b684d02db819c66bc -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVertexInputState.NumVertexAttributes 8e5b2dceac29b9113df8c5d691523d507888d28646a46d1e7684add1abab4457 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVertexInputState.#ctor(Inno.Native.Sdl3.SDLGPUVertexBufferDescription*,System.UInt32,Inno.Native.Sdl3.SDLGPUVertexAttribute*,System.UInt32) 5bd6c280810055be7ce197a9d509c7fff5452576bac804ea5b2da4dde4599ba2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-095a3d4f9248da39"></a>[`public uint Inno.Native.Sdl3.SDLGPUVertexInputState.NumVertexAttributes`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5058) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11884da2c0d7022d"></a>[`public uint Inno.Native.Sdl3.SDLGPUVertexInputState.NumVertexBuffers`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5056) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0d5383b0a102841f"></a>[`public Inno.Native.Sdl3.SDLGPUVertexAttribute* Inno.Native.Sdl3.SDLGPUVertexInputState.VertexAttributes`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5057) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a394c2d6476ee890"></a>[`public Inno.Native.Sdl3.SDLGPUVertexBufferDescription* Inno.Native.Sdl3.SDLGPUVertexInputState.VertexBufferDescriptions`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5055) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b47301471e681743"></a>[`public Inno.Native.Sdl3.SDLGPUVertexInputState.SDLGPUVertexInputState(Inno.Native.Sdl3.SDLGPUVertexBufferDescription* vertex_buffer_descriptions = null, uint num_vertex_buffers = 0, Inno.Native.Sdl3.SDLGPUVertexAttribute* vertex_attributes = null, uint num_vertex_attributes = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5059) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1912512483c73d49"></a>[`public struct Inno.Native.Sdl3.SDLGPUVertexInputState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5053) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUViewport</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUViewport 686ccdc6ecfb6f37bca9914b7a601bc74c94b4912f1fd5a6bed8f1ed657cc5af -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUViewport.X c620650eb16c674910b8c3c2e5bc8b11aff9faf550a630743b4463266577b178 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUViewport.Y 35dd69cc7196d0828abcace196301c7905597f3bcdc012ae9b40176376ed71f8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUViewport.W 402d52558da490ce54452bd863f02f08e92216daa8ed6b899d6c095c313f09dd -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUViewport.H 306d0aceb66854627e17677e225a7d737f87476702542288741cb0503eed70be -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUViewport.MinDepth 11a2f27235493939b060552946762e5c1fa228e15a0a43cdb2c8f8b3c7fa9bda -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUViewport.MaxDepth b5efd18c7cbfbb762cf24e51219b5f5f3b6d0842850f183735c0f7da4608b062 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUViewport.#ctor(System.Single,System.Single,System.Single,System.Single,System.Single,System.Single) c2b3227d1d2d224421af2e5da87fcfa3fd5558d0e617588b55949326fc96ce19 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f1429f8b151927f1"></a>[`public float Inno.Native.Sdl3.SDLGPUViewport.H`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4754) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a2d25fa2d6f6e498"></a>[`public float Inno.Native.Sdl3.SDLGPUViewport.MaxDepth`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4756) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-437cfe15b346e1dc"></a>[`public float Inno.Native.Sdl3.SDLGPUViewport.MinDepth`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4755) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-464084603c050b95"></a>[`public float Inno.Native.Sdl3.SDLGPUViewport.W`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4753) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c87dd519ddc209c9"></a>[`public float Inno.Native.Sdl3.SDLGPUViewport.X`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4751) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a58b7014da5d0cde"></a>[`public float Inno.Native.Sdl3.SDLGPUViewport.Y`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4752) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ac2c76630b798a03"></a>[`public Inno.Native.Sdl3.SDLGPUViewport.SDLGPUViewport(float x = 0, float y = 0, float w = 0, float h = 0, float min_depth = 0, float max_depth = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4757) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-21c69b3ba67c3fe0"></a>[`public struct Inno.Native.Sdl3.SDLGPUViewport`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4749) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUViewportPtr</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUViewportPtr 6dc77cebaf09e7f2d1f67fb4aefd5a57b2ea4276caa67ab57ce508e603fec25f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUViewportPtr.#ctor(Inno.Native.Sdl3.SDLGPUViewport*) 45b2e9ccda2b47c0ad42b7a9d9990e9eeb878b45415db19a15faa0b82794a025 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUViewportPtr.Handle 8dddc7613aec92a54c8763747e8a8b10c0d2eb9a9da8a70b6a014beb438529b4 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUViewportPtr.IsNull 2ec3e8fd951247d8a2d548075fd1f9b25940895ce21ae3e8c6f103a88fa1e2c6 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUViewportPtr.Null 087807ce8b0c7496a91cebcb206224f662882231833cd987770cf2aba75434e0 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUViewportPtr.Item(System.Int32) 406476e8bb1135d7d75a85b218ac39c8b2dbdbf08801ed0737f3c870925ced73 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUViewportPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUViewport*)~Inno.Native.Sdl3.SDLGPUViewportPtr 0e4fb7c1251baf88c8689bb2b3f4ea084988b7fa57c617d9c0b1aaf80b4867b2 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUViewportPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUViewportPtr)~Inno.Native.Sdl3.SDLGPUViewport* 1d944d9c1f312162bc7a66da57730c4a4b7bc95867340243387fe5ea7ed9804f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUViewportPtr.op_Equality(Inno.Native.Sdl3.SDLGPUViewportPtr,Inno.Native.Sdl3.SDLGPUViewportPtr) c5552e236a31a0cdb7f3d13952dee5c7facd1dfc31815ce229e625e7191d1f72 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUViewportPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUViewportPtr,Inno.Native.Sdl3.SDLGPUViewportPtr) a5850c5b2808e8def818470fcf5547a1c59af874017fb829da893c7dee532f21 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUViewportPtr.Equals(Inno.Native.Sdl3.SDLGPUViewportPtr) 3848b905c9dc6cf73938dccca59ba5efd0de66e92e41853e4430fbae7a508153 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUViewportPtr.Equals(System.Object) 2e49ec607ed78a28787005bdcd958de44d69ac544ce9c783a0531ad3bd4cc55f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUViewportPtr.GetHashCode 8229787e0798b8cae9c0ee417a673fd760f4df57c5ca517094d8be66449bd68c -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUViewportPtr.X 11f421c712ee1def78bcd6c6c71e9671017e67ad3c13b8c142c8b795ec93af80 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUViewportPtr.Y d18fb0a62c16e24ed76b54720cc13ae976d556ea03fc30f9a39a1ad95e5ba487 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUViewportPtr.W a941ac9eadb9266ce3a332738d77010fa9fa22438382ffc5fc454efd2fa19f98 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUViewportPtr.H 182461d5465fc315ba8b3f4d639c58be7e7ce493e29ca82e7797f52b5838ec1e -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUViewportPtr.MinDepth a7c8f960c121e7c9ddf69d80aef66b86848eb6dbc6610af4d2376010ff2d5095 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUViewportPtr.MaxDepth 2dce4996b9adfced5c8cb23e61664abdbef645c0a7417baf943f1cd7478d8a51 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4d2f614137679d56"></a>[`public Inno.Native.Sdl3.SDLGPUViewport* Inno.Native.Sdl3.SDLGPUViewportPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9165) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e8119a6371206c25"></a>[`public Inno.Native.Sdl3.SDLGPUViewportPtr.SDLGPUViewportPtr(Inno.Native.Sdl3.SDLGPUViewport* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9160) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a781997f025da9f0"></a>[`public bool Inno.Native.Sdl3.SDLGPUViewportPtr.Equals(Inno.Native.Sdl3.SDLGPUViewportPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9175) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-9bfe87bdb58d6f83"></a>[`public override bool Inno.Native.Sdl3.SDLGPUViewportPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9176) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-bf8f08e88f861332"></a>[`public override int Inno.Native.Sdl3.SDLGPUViewportPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9177) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-11149711184b51e2"></a>[`public static bool Inno.Native.Sdl3.SDLGPUViewportPtr.operator ==(Inno.Native.Sdl3.SDLGPUViewportPtr left, Inno.Native.Sdl3.SDLGPUViewportPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9173) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7b7be515db34634f"></a>[`public static Inno.Native.Sdl3.SDLGPUViewportPtr.implicit operator Inno.Native.Sdl3.SDLGPUViewportPtr(Inno.Native.Sdl3.SDLGPUViewport* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9171) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3d7a61f59f15fcdb"></a>[`public static Inno.Native.Sdl3.SDLGPUViewportPtr.implicit operator Inno.Native.Sdl3.SDLGPUViewport*(Inno.Native.Sdl3.SDLGPUViewportPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9172) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-506b8b0252d9eaaf"></a>[`public static bool Inno.Native.Sdl3.SDLGPUViewportPtr.operator !=(Inno.Native.Sdl3.SDLGPUViewportPtr left, Inno.Native.Sdl3.SDLGPUViewportPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9174) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-04810c31dd73a111"></a>[`public ref float Inno.Native.Sdl3.SDLGPUViewportPtr.H { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9182) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9e3767547c13797d"></a>[`public bool Inno.Native.Sdl3.SDLGPUViewportPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9166) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-22200c07e197d0bf"></a>[`public Inno.Native.Sdl3.SDLGPUViewport Inno.Native.Sdl3.SDLGPUViewportPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9169) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f7b481c7b66249a8"></a>[`public ref float Inno.Native.Sdl3.SDLGPUViewportPtr.MaxDepth { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9184) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-06086be1fcb60698"></a>[`public ref float Inno.Native.Sdl3.SDLGPUViewportPtr.MinDepth { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9183) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f015db89a972c02a"></a>[`public static Inno.Native.Sdl3.SDLGPUViewportPtr Inno.Native.Sdl3.SDLGPUViewportPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9167) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e3b02d429a0bec8b"></a>[`public ref float Inno.Native.Sdl3.SDLGPUViewportPtr.W { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9181) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-76ca6095658f8a75"></a>[`public ref float Inno.Native.Sdl3.SDLGPUViewportPtr.X { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9179) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5a76e7d68fd65dd0"></a>[`public ref float Inno.Native.Sdl3.SDLGPUViewportPtr.Y { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9180) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-db7aaefe7de0488c"></a>[`public struct Inno.Native.Sdl3.SDLGPUViewportPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUViewportPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9158) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUVulkanOptions</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUVulkanOptions ab3eb50d05839e3e8581e10ecc8c72b99b7e383124493fe647d6d6296064906f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVulkanOptions.VulkanApiVersion 43be728e9616ef6d00c2489b714f76ad0788ffe0ea71a92f81d5a41742e3a51f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVulkanOptions.FeatureList 5e89bf461e4ec8e869af31b85c01e00f56094b592274feede031cd17e98b6c36 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVulkanOptions.Vulkan10PhysicalDeviceFeatures 8a984301658707b99cd10399dd605604effb00b173bd701c0fa8d362b7b73ff0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVulkanOptions.DeviceExtensionCount de5f3139f1e4953ddf94e60fac558a350b999077b489b7030d20926a182b5582 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVulkanOptions.DeviceExtensionNames 7e6e7edea2d78296a958e85e86c4ef4fef3d6ca500821494e102c5bda0fa7616 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVulkanOptions.InstanceExtensionCount d8ee1ca84754fdcc8b395f2037b2d2e91577d06e5665b6a261c004850aec4be5 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUVulkanOptions.InstanceExtensionNames 187f355ad70587ab0e3a9d58c2fa28455b67e49cb4c501d9b9ff483f59ddffef -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUVulkanOptions.#ctor(System.UInt32,System.Void*,System.Void*,System.UInt32,System.Byte**,System.UInt32,System.Byte**) 3b362e1953d30e15ed48a88dacd7a3d6968116f5ff46affcd715f07cef52dfc4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-13f0c0731cb0c6a2"></a>[`public uint Inno.Native.Sdl3.SDLGPUVulkanOptions.DeviceExtensionCount`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5593) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d9e8e2ea6b15f87e"></a>[`public byte** Inno.Native.Sdl3.SDLGPUVulkanOptions.DeviceExtensionNames`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5594) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27384b3422e50bec"></a>[`public void* Inno.Native.Sdl3.SDLGPUVulkanOptions.FeatureList`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5591) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b651fa5694dd0e9f"></a>[`public uint Inno.Native.Sdl3.SDLGPUVulkanOptions.InstanceExtensionCount`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5595) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1e87d424cff61e6"></a>[`public byte** Inno.Native.Sdl3.SDLGPUVulkanOptions.InstanceExtensionNames`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5596) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f9db53ec9fd7e652"></a>[`public void* Inno.Native.Sdl3.SDLGPUVulkanOptions.Vulkan10PhysicalDeviceFeatures`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5592) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6a69adbd15bd274a"></a>[`public uint Inno.Native.Sdl3.SDLGPUVulkanOptions.VulkanApiVersion`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5590) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-224c0af46c461e70"></a>[`public Inno.Native.Sdl3.SDLGPUVulkanOptions.SDLGPUVulkanOptions(uint vulkan_api_version = 0, void* feature_list = null, void* vulkan_10_physical_device_features = null, uint device_extension_count = 0, byte** device_extension_names = null, uint instance_extension_count = 0, byte** instance_extension_names = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5597) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b618818c97cf64ae"></a>[`public struct Inno.Native.Sdl3.SDLGPUVulkanOptions`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5588) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SdlGuid</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SdlGuid f106adae4b451aad67409d382f83b88b0d6529cb9407ebd2b1d963b8e4a45a69 -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_0 c35001825223d0e21fe233109b7f14f1d0a877e916e7cdfead9f50bd46c2e55c -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_1 d37fb9ce171f027ade5a700f93dd458574e7ff1a4cc00d2808b16351bef98acb -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_2 8228cf35912685f85cbd7d874db226511d53e7d9d17182771addf8a5cf230bf5 -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_3 b423691e5c3dc9b9540318b15a199bc32bf4b87c1d48c31cec0fa320237cf4b7 -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_4 f033c70a9f4725799a6be9a98a6c4dfb2ed8c89b0337906fa234a535aaf3eba2 -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_5 adf8ed61d43352686b0df7772fa5a6e6f83b6dc4efa50f47bec87e64da5224bb -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_6 4cd031c853a72c484465523b156020a50d9d9668f363352d047d1d35ed68e968 -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_7 cf1fc727bc9aea12268daf2e0be9943caf0067932a44261882888cd33072d6dd -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_8 0f36fac3f1035f670f2411955fc188daceb2254c62c7a8d2bb6feafbf0e691ec -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_9 0db2b075a0ccbe31363b55450016c6c2b68be566866604bc6e5442a2f8fa03f1 -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_10 c64f580a95c1eea8d6acb4b4944af6922fb891f359eb58340c94917ba1daa4f8 -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_11 7d0c4983b7928fe6f26de984c32fba582d5328d4b9b68f5693b48b4372f529a0 -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_12 6211207261e3c9d5ba6f442698c5b6c77703399500340e3feb2d714ef1e9c049 -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_13 13dacb7f65ea8b969928699dbbdbe956028656b8b0586628682a6fce632e3cd7 -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_14 7ba463bb889b6e5800edfe8b1efd4ac7822d50a11351b7da2897dfcc52f0c418 -->
<!-- api: F:Inno.Native.Sdl3.SdlGuid.Data_15 6903af48e0df7b4294d2278af3adac6f9a857b0c681e3910a4200353c8b4ed5d -->
<!-- api: P:Inno.Native.Sdl3.SdlGuid.Data fd7ec83facd0ba1784b5d65c08de36217d2dcb27ee4caa157867018be85239a9 -->
<!-- api: M:Inno.Native.Sdl3.SdlGuid.#ctor(System.Byte*) 3757ccf2f25fc6a395e426fd15ba27f2a35a8166d14bcd509aae471def679a03 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c53c31fabf80306e"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2691) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bd0e2f61938c3bbb"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2692) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-401324ed86c92ec3"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2701) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c43dd3e0e9798e4"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_11`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2702) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa7feacf60095b90"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_12`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2703) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6acbc45715bc7b1"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_13`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2704) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1300a05e9b2cab26"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_14`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2705) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0eaa90fee479796c"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_15`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2706) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4638ecb1f026723b"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2693) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-44bed626d5f9a63c"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2694) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-88b180a96478cca3"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2695) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fdc4263355798295"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2696) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-17c3d2039578040d"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2697) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-75c6daf9d865d258"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2698) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-617143fe4522d53b"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2699) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3166e9af46c7961b"></a>[`public byte Inno.Native.Sdl3.SdlGuid.Data_9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2700) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-acdff4f07569d94d"></a>[`public Inno.Native.Sdl3.SdlGuid.SdlGuid(byte* data = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2709) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3c070df3f55e84a4"></a>[`public System.Span<byte> Inno.Native.Sdl3.SdlGuid.Data { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2707) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f6207848295646c1"></a>[`public struct Inno.Native.Sdl3.SdlGuid`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2689) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHaptic</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHaptic 084a4bc6cdc84616eb1c663a00dc503cece193c14b3c6d805bb2c95c8b7a31e2 -->
<!-- api: M:Inno.Native.Sdl3.SDLHaptic.#ctor(System.IntPtr) d5cc99e3c41acb1a28a8c94bf44f0a2245a9cbe47650fe4ad96880faab6f46eb -->
<!-- api: P:Inno.Native.Sdl3.SDLHaptic.Handle d99d389ba919ec051378e48a53f055ff23b8d9d31520c908afb9acac8f0756e8 -->
<!-- api: P:Inno.Native.Sdl3.SDLHaptic.IsNull c1de4474e7a5305b93f07006eea14a6a93bce85636a4b0899c269e9d2baabda4 -->
<!-- api: P:Inno.Native.Sdl3.SDLHaptic.Null 99db2d8687c61374cbf1ded5c9551fdfb254ea52639b062ce7651a4a95847088 -->
<!-- api: M:Inno.Native.Sdl3.SDLHaptic.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLHaptic e456d207f43ea7a90f1a2703f76bcc531ff94083965ce5a941ba77947c8a8858 -->
<!-- api: M:Inno.Native.Sdl3.SDLHaptic.op_Equality(Inno.Native.Sdl3.SDLHaptic,Inno.Native.Sdl3.SDLHaptic) bef5d6bc81dca70124e607455f36eaadc4d4c2b8c2856b9317a6f4d562b0f540 -->
<!-- api: M:Inno.Native.Sdl3.SDLHaptic.op_Inequality(Inno.Native.Sdl3.SDLHaptic,Inno.Native.Sdl3.SDLHaptic) f670cdab04d40a920b8ed0696703fb357b8399a521f8d0e658c3dbb38bdcacc3 -->
<!-- api: M:Inno.Native.Sdl3.SDLHaptic.Equals(Inno.Native.Sdl3.SDLHaptic) e9017b97c86e516cc120a798296c08eb106550b4810fd73f9e5be1213ddc9551 -->
<!-- api: M:Inno.Native.Sdl3.SDLHaptic.Equals(System.Object) 25bb45d3a5520b9bc62975d82fdfdfb581bc981fbf012aa9bf4fd0989c5c9c71 -->
<!-- api: M:Inno.Native.Sdl3.SDLHaptic.GetHashCode c074bbb1dd211932ee3d9be420aeeaf6f25ac14d0f1570178d8a121033269a3a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ef4c93ad464c9e57"></a>[`public Inno.Native.Sdl3.SDLHaptic.SDLHaptic(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5615) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-825056d15e4235e5"></a>[`public bool Inno.Native.Sdl3.SDLHaptic.Equals(Inno.Native.Sdl3.SDLHaptic other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5627) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-1aa4a597f0bf42ef"></a>[`public override bool Inno.Native.Sdl3.SDLHaptic.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5628) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-762c8f48592ad001"></a>[`public override int Inno.Native.Sdl3.SDLHaptic.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5629) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-7b863edeb7987394"></a>[`public static bool Inno.Native.Sdl3.SDLHaptic.operator ==(Inno.Native.Sdl3.SDLHaptic left, Inno.Native.Sdl3.SDLHaptic right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5625) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2da8c0c45c8ecc76"></a>[`public static Inno.Native.Sdl3.SDLHaptic.implicit operator Inno.Native.Sdl3.SDLHaptic(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5624) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3b0d64fdfa12667a"></a>[`public static bool Inno.Native.Sdl3.SDLHaptic.operator !=(Inno.Native.Sdl3.SDLHaptic left, Inno.Native.Sdl3.SDLHaptic right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5626) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0ca11fe0b5e479db"></a>[`public nint Inno.Native.Sdl3.SDLHaptic.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5620) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d0ffdd261df9ce35"></a>[`public bool Inno.Native.Sdl3.SDLHaptic.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5621) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5bb8225381086d95"></a>[`public static Inno.Native.Sdl3.SDLHaptic Inno.Native.Sdl3.SDLHaptic.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5622) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a98e2d95dd30a3cd"></a>[`public readonly struct Inno.Native.Sdl3.SDLHaptic : System.IEquatable<Inno.Native.Sdl3.SDLHaptic>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5613) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHapticCondition</code> · 32 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHapticCondition 3384c28989e136f1ea9912269c75cc9a80b9420df4d348ea9096b9fd7346c5d9 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.Type a148d5ea0a5ff2a7535ca0a9a60be2fc71ccc64f2ea40e79563c99b1b53f4cb2 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.Direction 31815aa53b0839dc606f1dd1e4a65d08a958ee0ea141d7feb9d82deb255d4c47 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.Length 964f3ac30651fa4d0f62897fe36dcf3fe65a766e8baa2826dfc3feff46eda9a4 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.Delay 3f39466a9ef5bf4f3e99d3ccfeb9512c91ceeb8b4bc0b79bca4e0604edc22891 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.Button e488e9e80654efcb190f6d95f115ee8b1c48deecb75bf40aa857e5629d5be017 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.Interval 105941b82d51f94b4efc659a10b6edd1a544a704bc330cfdfec59303c1e460b2 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.RightSat_0 1b31f56d7d0062875aa3623dbb43a2b1ffafa30cd5ed8f67dad24c5722fe4d1f -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.RightSat_1 b4cf71965e75de5aa9a4d6de7a5e0a5b1d692106253695c06280801c4b76a843 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.RightSat_2 f6e61718ebc5a77c214c3cc987d548600f44d0be92ad9adb22f42157f4e5cbcb -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticCondition.RightSat 3e4f37222bdde92d6af33d3639c40ecbfc5c3506134224797c7bd18ca7e253ce -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.LeftSat_0 1929eb2777ed351bc79107e1063f7f80d04a5aa0726d7fc8b5ed206ea88e9bc8 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.LeftSat_1 00aaa5f16eb4c9fb1804e5f5faf762e32e8b240c9bca9a60925c4c183ca1a0e1 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.LeftSat_2 f5424206536b9c92225fc5fc38cb52563b47afd103aaa436bcaaf4a751601049 -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticCondition.LeftSat 7d784c5e361cf5d22af79c5fc596b446fdae63065605dc86e14b440c4f2ce428 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.RightCoeff_0 ccbb56c6871718d1991185385c1ee30b14e4ec7fbaeb08863c3789c3534529a3 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.RightCoeff_1 f000c555b0e6194e565e05dc6938d24bf99eeab179052caaa73fb2d0d0236404 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.RightCoeff_2 94c3175b563c224d6c5f3618fc44e09bbae1947bb31f59736557e7e72f9f5efc -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticCondition.RightCoeff a736e55fe633e49e037dd21de54e3ca249844e979da39cb29f08f9bc814e44f6 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.LeftCoeff_0 2ea73af31c83c88f12fcb80f9317d1433f2da8404501786d889ec5099fe7db12 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.LeftCoeff_1 26247a43940c3b1c4c79a294744f19d98a27672b6060eab4882d235af00f8d45 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.LeftCoeff_2 92a4f1bf4d8083a50484a20a98e10d1c7558624a742d7ab79df8ae88f7f5cfba -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticCondition.LeftCoeff 9c6bc0c45dc3e1537fa8a25bd939acce7b94dc2599cddfd332d106eaa118065c -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.Deadband_0 f94dcf453af2261559fa1946ad55f2d3f1571995c076dca6f9f32f9689dd4d86 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.Deadband_1 e74a6173c7914886ecc64ac00b555c73ff7aabd9004f313fedd91be5192a9a19 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.Deadband_2 66c5037f29c453f145d3633f7b7cbb8c1c2d2a88091f322e5464f79bc09e2f83 -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticCondition.Deadband ddb916c3e7a206f053275f6b19fcc862d82db4380e713ed4c7930ba34bf98a4f -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.Center_0 02add252266c426f7c676a050b65e62f07e271c1c2424930c40522915290c11b -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.Center_1 9b87463b922557c877541cb0cc8dc46ce2f5864b09684edaafb68bd8f3648256 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCondition.Center_2 9e624b5c034bbe73b98096bd1d5dea3b33853f444d87f1d358593136c2a912df -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticCondition.Center ae7c009a13eaa064315b67b46c6c97592da5d832b8bb109b04a5992dcaf9b3fc -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticCondition.#ctor(System.UInt16,Inno.Native.Sdl3.SDLHapticDirection,System.UInt32,System.UInt16,System.UInt16,System.UInt16,System.UInt16*,System.UInt16*,System.Int16*,System.Int16*,System.UInt16*,System.Int16*) 9197fc31b40d325d4b929c3b175c9529f68816c994832541100187b8970b08a3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-cce3edf9c013d45c"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCondition.Button`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5739) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6868161ed2807fa0"></a>[`public short Inno.Native.Sdl3.SDLHapticCondition.Center_0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5766) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3387511280fb0588"></a>[`public short Inno.Native.Sdl3.SDLHapticCondition.Center_1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5767) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f347c9dd97ef0dce"></a>[`public short Inno.Native.Sdl3.SDLHapticCondition.Center_2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5768) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ffc1c2410703a8ca"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCondition.Deadband_0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5761) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2b76a299a032ff32"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCondition.Deadband_1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5762) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0b9e1add4d8083d2"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCondition.Deadband_2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5763) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-92cb6bda8f16cc0b"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCondition.Delay`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5738) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-062afc8284eeb1cc"></a>[`public Inno.Native.Sdl3.SDLHapticDirection Inno.Native.Sdl3.SDLHapticCondition.Direction`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5736) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-366f02540ee66c9c"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCondition.Interval`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5740) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e8c400f23acee569"></a>[`public short Inno.Native.Sdl3.SDLHapticCondition.LeftCoeff_0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5756) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7fff470126f503ab"></a>[`public short Inno.Native.Sdl3.SDLHapticCondition.LeftCoeff_1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5757) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-38da013db4484caa"></a>[`public short Inno.Native.Sdl3.SDLHapticCondition.LeftCoeff_2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5758) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-62a27c4a0fccc612"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCondition.LeftSat_0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5746) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c6b5ef10467b51dd"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCondition.LeftSat_1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5747) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-992ecb2884c4aced"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCondition.LeftSat_2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5748) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d838210f866fd8b"></a>[`public uint Inno.Native.Sdl3.SDLHapticCondition.Length`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5737) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b59c6985bdb02a99"></a>[`public short Inno.Native.Sdl3.SDLHapticCondition.RightCoeff_0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5751) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf4df137b9d73d26"></a>[`public short Inno.Native.Sdl3.SDLHapticCondition.RightCoeff_1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5752) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a19a800ad1f116f8"></a>[`public short Inno.Native.Sdl3.SDLHapticCondition.RightCoeff_2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5753) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6782ef2c4a33db0"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCondition.RightSat_0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5741) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2c5186db5fea3d4"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCondition.RightSat_1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5742) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f9af9a1a62d04836"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCondition.RightSat_2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5743) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9dd91b436c7d2a53"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCondition.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5735) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-899692adab9e9427"></a>[`public Inno.Native.Sdl3.SDLHapticCondition.SDLHapticCondition(ushort type = 0, Inno.Native.Sdl3.SDLHapticDirection direction = default(Inno.Native.Sdl3.SDLHapticDirection), uint length = 0, ushort delay = 0, ushort button = 0, ushort interval = 0, ushort* right_sat = null, ushort* left_sat = null, short* right_coeff = null, short* left_coeff = null, ushort* deadband = null, short* center = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5771) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fc933faf6ab9d784"></a>[`public System.Span<short> Inno.Native.Sdl3.SDLHapticCondition.Center { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5769) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ed6a344bb129c681"></a>[`public System.Span<ushort> Inno.Native.Sdl3.SDLHapticCondition.Deadband { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5764) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-afe6f2dba0eab7dc"></a>[`public System.Span<short> Inno.Native.Sdl3.SDLHapticCondition.LeftCoeff { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5759) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-44774b93d4027f98"></a>[`public System.Span<ushort> Inno.Native.Sdl3.SDLHapticCondition.LeftSat { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5749) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-435af8d1e1ddcee5"></a>[`public System.Span<short> Inno.Native.Sdl3.SDLHapticCondition.RightCoeff { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5754) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3ae9c377a792d21d"></a>[`public System.Span<ushort> Inno.Native.Sdl3.SDLHapticCondition.RightSat { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5744) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-faddb6827589c392"></a>[`public struct Inno.Native.Sdl3.SDLHapticCondition`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5733) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHapticConstant</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHapticConstant bf7434cdd91c137821866bb3beb5e607197e67fcc7d00db29ab84cd8f36af87a -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticConstant.Type 4f19826b4dbeeb41e00227d0bfd1415d8a03f8b12f7e8e1af2d3e4e13de07d74 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticConstant.Direction 98e962cd73fc1800be06cdc55378b436264e490ad8674d0e909195976f7c4355 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticConstant.Length 99c82a63f5d074373084d54686b40564847d5e1b0ab94a7a018332293e08f0f4 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticConstant.Delay b02db942653f991877388cba22c5291fa3635d0e06662d87cce69ed17081d483 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticConstant.Button 7846c7243d6583fc6d7d762de64769c1deb6d0af790119ea49504674c8dc0ecc -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticConstant.Interval 1ccef3c099728252ed565fbf9dbfc5aa460538ef817cc884b81960cefaaa2447 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticConstant.Level b4f0b34da88b1c47fc7440007358bf7e071c5249e3b5f0e77b90cb54ffe2eed3 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticConstant.AttackLength f88abdc48b223aea003392640bffae51e5777ae05188041a7d22b3cf6a65f17c -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticConstant.AttackLevel bcecfbd8562ec2b0e823c1c7c70ea5ffd5f4c0d8854346d582bfd2790effa17d -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticConstant.FadeLength 292d51c846fc2b54c71064fc6864e4aa5e12edf29796df4de463a1ec1538b12c -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticConstant.FadeLevel 3f6d48d86dcc6b30eae1ad3c6953138548ee80314a0224cfb30babfb54015cf4 -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticConstant.#ctor(System.UInt16,Inno.Native.Sdl3.SDLHapticDirection,System.UInt32,System.UInt16,System.UInt16,System.UInt16,System.Int16,System.UInt16,System.UInt16,System.UInt16,System.UInt16) 50f94ccf6841bc964e45df7eda7db1aadebabda375e2032f52b791ae6354c450 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-819076257634fe69"></a>[`public ushort Inno.Native.Sdl3.SDLHapticConstant.AttackLength`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5670) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d6aa003ba9d45adb"></a>[`public ushort Inno.Native.Sdl3.SDLHapticConstant.AttackLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5671) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-82c80dcdb0147a60"></a>[`public ushort Inno.Native.Sdl3.SDLHapticConstant.Button`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5667) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-721f432bba8ff1be"></a>[`public ushort Inno.Native.Sdl3.SDLHapticConstant.Delay`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5666) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-77c1f1cb2533a175"></a>[`public Inno.Native.Sdl3.SDLHapticDirection Inno.Native.Sdl3.SDLHapticConstant.Direction`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5664) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c1355c30e22d64f7"></a>[`public ushort Inno.Native.Sdl3.SDLHapticConstant.FadeLength`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5672) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b6eb3283f26dacb5"></a>[`public ushort Inno.Native.Sdl3.SDLHapticConstant.FadeLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5673) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c073e0c6e442a187"></a>[`public ushort Inno.Native.Sdl3.SDLHapticConstant.Interval`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5668) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b0b856e315ae24ee"></a>[`public uint Inno.Native.Sdl3.SDLHapticConstant.Length`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5665) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d7e1a0dee8a0de5e"></a>[`public short Inno.Native.Sdl3.SDLHapticConstant.Level`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5669) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc93f00949fdf153"></a>[`public ushort Inno.Native.Sdl3.SDLHapticConstant.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5663) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9bd30ab83ab06862"></a>[`public Inno.Native.Sdl3.SDLHapticConstant.SDLHapticConstant(ushort type = 0, Inno.Native.Sdl3.SDLHapticDirection direction = default(Inno.Native.Sdl3.SDLHapticDirection), uint length = 0, ushort delay = 0, ushort button = 0, ushort interval = 0, short level = 0, ushort attack_length = 0, ushort attack_level = 0, ushort fade_length = 0, ushort fade_level = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5674) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0b0966b35fdbbde9"></a>[`public struct Inno.Native.Sdl3.SDLHapticConstant`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5661) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHapticCustom</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHapticCustom 92d2c7def709974e617be0c9c154f12453b82944dcb1286b1e62d313ba5c2543 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.Type 495c8a5b8f9eda32816e8a7cf334b7041123a8d628cab73c886030123c6cd61d -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.Direction 0526c465f197c8eb7d8146ad58bbc7c7b153a2da8a706d7867b0e4544276e2e6 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.Length ef0a3fedc47e680ec6229fe13b49b43c6b2a454e74e888f61f1410aab503b953 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.Delay ad90216cc3fc09dc4037056dc271f994000f5520e1728b53739f4478e61ebb08 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.Button d3ab85f88629f0614d2015a5541720d14418a7ce4ac02117c72b92e7a9825848 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.Interval 879a2265487170627e79514c2cc211b1388cae40ef88c7695adcb76f3f2144d9 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.Channels b3d6866ef8a493c33f93af1c022a4addf676eafd7dad4ff11edea030a30cac91 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.Period 63b8ab96103dc0ecde6da3ae1ce7ebccf9daf55e826586dcd5db84e776fe425f -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.Samples 28d75518de240033b6c8788a768cea323a3a4328c983e263701d3f42eefa30b9 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.Data 85ca7099919ac303ec8a35060069d0a39e3d5eec3979a07a89339decc08e3ffd -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.AttackLength 2998ca55ef10085709a6265f100d1bb3c3d33eefbac14d0eb9148d60db419898 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.AttackLevel 6901cc88c3da7b097d5082df2bcaaa89c349c2c1bba6e4a3578f9fdc2d7434b5 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.FadeLength 285e1d549d200b5bcbad11be3318b30cc5a0770f8719c944033747573f957fed -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticCustom.FadeLevel e5660ab1e96b73e52fd2ab54d7ebc23ffc7c41becff38867f2e882d9b85f9db5 -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticCustom.#ctor(System.UInt16,Inno.Native.Sdl3.SDLHapticDirection,System.UInt32,System.UInt16,System.UInt16,System.UInt16,System.Byte,System.UInt16,System.UInt16,System.UInt16*,System.UInt16,System.UInt16,System.UInt16,System.UInt16) 1cf15d0d5b698109aae74cbb1b7689ca93cf9d084bce3a19a17ce2d6b6fc98c3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-858de57de7b11cf3"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCustom.AttackLength`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5893) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5ced65adfa6282df"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCustom.AttackLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5894) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec57d971db960803"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCustom.Button`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5887) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6e448626f3019c3"></a>[`public byte Inno.Native.Sdl3.SDLHapticCustom.Channels`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5889) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7c02763f34250789"></a>[`public ushort* Inno.Native.Sdl3.SDLHapticCustom.Data`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5892) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-91a942f442637d99"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCustom.Delay`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5886) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35c74584733ee55e"></a>[`public Inno.Native.Sdl3.SDLHapticDirection Inno.Native.Sdl3.SDLHapticCustom.Direction`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5884) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-813a369e6956d106"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCustom.FadeLength`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5895) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af0f42ecf5f2d148"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCustom.FadeLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5896) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58f72ed7fe97b9f5"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCustom.Interval`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5888) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aab8cb940c2303ea"></a>[`public uint Inno.Native.Sdl3.SDLHapticCustom.Length`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5885) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfe5d85d1c170538"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCustom.Period`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5890) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f828b2f60af95a66"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCustom.Samples`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5891) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89cbe9674069ed24"></a>[`public ushort Inno.Native.Sdl3.SDLHapticCustom.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5883) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-25a09e0002521bac"></a>[`public Inno.Native.Sdl3.SDLHapticCustom.SDLHapticCustom(ushort type = 0, Inno.Native.Sdl3.SDLHapticDirection direction = default(Inno.Native.Sdl3.SDLHapticDirection), uint length = 0, ushort delay = 0, ushort button = 0, ushort interval = 0, byte channels = 0, ushort period = 0, ushort samples = 0, ushort* data = null, ushort attack_length = 0, ushort attack_level = 0, ushort fade_length = 0, ushort fade_level = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5897) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d129d4e52ebc4edf"></a>[`public struct Inno.Native.Sdl3.SDLHapticCustom`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5881) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHapticDirection</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHapticDirection d7bfd019b29fb777d057cc6da31a693b0c0653d021c8331fa42db31e6315f653 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticDirection.Type 7f5155190ed7818fd3cbfa2ee7f922de4f8757b8254874bee79284a25e45afe7 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticDirection.Dir_0 e54eeab418e105526435ea705f31b81f3885d633692f43e06139c555300fced7 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticDirection.Dir_1 034818891412558853f63cc6fa3cf10d3f0bf2214dd3bc86c31dcbbf6eb28ac8 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticDirection.Dir_2 45513559aaef4368a320d6e5a1b3146218b89fa237152225f407efdfbddf85f2 -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticDirection.Dir 88ed85c0664dd40709de06bdcedf9146f7be3013a6c25de62ddc417b9b13127b -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticDirection.#ctor(System.Byte,System.Int32*) f3d7309c90e20d3352fc60e9e3ae743ea868cec0954bb283ce7e629ac4485afb -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e4138bff15ad4056"></a>[`public int Inno.Native.Sdl3.SDLHapticDirection.Dir_0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5640) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-66f0e687b41ed60b"></a>[`public int Inno.Native.Sdl3.SDLHapticDirection.Dir_1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5641) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-978924a9d9815498"></a>[`public int Inno.Native.Sdl3.SDLHapticDirection.Dir_2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5642) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6fe8cd237af90cf8"></a>[`public byte Inno.Native.Sdl3.SDLHapticDirection.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5639) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6e36a18dacda9c6"></a>[`public Inno.Native.Sdl3.SDLHapticDirection.SDLHapticDirection(byte type = 0, int* dir = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5645) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6527a1249f5130df"></a>[`public System.Span<int> Inno.Native.Sdl3.SDLHapticDirection.Dir { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5643) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a1bd2e3665f1bca1"></a>[`public struct Inno.Native.Sdl3.SDLHapticDirection`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5637) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHapticEffect</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHapticEffect 0b3fc5b030342fe8568a49be3834222cec9b750012c670e33f6622a8f2c82e31 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticEffect.Type c65725d12dc86a204f4715b2b59f30ec33cdd1db0ca91e76bec13af528face9a -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticEffect.Constant 5a0a0f63ded219721c8d5b076f5350535b0454be23149094a6b8533109bfda3f -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticEffect.Periodic 107702debbc366606c58b739395912e0be6160977f90dea45e196c29ffc98625 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticEffect.Condition 60e624d62260f1899da82772ce60de67410b898032f6a3350927dc355bae0eaf -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticEffect.Ramp d5fc8dffb287710f09fd2e394d23c3b02e499635fffd7a1f16f44caf0c097259 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticEffect.Leftright 4f9a40e929557e3fd8d3c8b67a0b904395b20046ecce1d6a0227bd9470508ac4 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticEffect.Custom 0d29de40f6270d4ce3e2ffae938e80f7beaab186aec3d569ca8e9e7b67c9ed5c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-108394cad0231ce0"></a>[`public Inno.Native.Sdl3.SDLHapticCondition Inno.Native.Sdl3.SDLHapticEffect.Condition`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5929) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a098df9cc81c3f63"></a>[`public Inno.Native.Sdl3.SDLHapticConstant Inno.Native.Sdl3.SDLHapticEffect.Constant`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5925) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e04087e883c5db00"></a>[`public Inno.Native.Sdl3.SDLHapticCustom Inno.Native.Sdl3.SDLHapticEffect.Custom`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5935) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7af9757670a9636e"></a>[`public Inno.Native.Sdl3.SDLHapticLeftRight Inno.Native.Sdl3.SDLHapticEffect.Leftright`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5933) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3535eb3b290690bc"></a>[`public Inno.Native.Sdl3.SDLHapticPeriodic Inno.Native.Sdl3.SDLHapticEffect.Periodic`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5927) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-146b109ff51b3426"></a>[`public Inno.Native.Sdl3.SDLHapticRamp Inno.Native.Sdl3.SDLHapticEffect.Ramp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5931) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-922a50f1bca526ce"></a>[`public ushort Inno.Native.Sdl3.SDLHapticEffect.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5923) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c90a477b8aa44e5"></a>[`public struct Inno.Native.Sdl3.SDLHapticEffect`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5920) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHapticEffectPtr</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHapticEffectPtr ef2436602fbaaeb668a569ae7b975b231b3c934bb016e5dcd5a70171b8a7e0d5 -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticEffectPtr.#ctor(Inno.Native.Sdl3.SDLHapticEffect*) 8e52c554311e6217fbbbeb3e02837f763a5e97437c1c742924f071ee531f4de7 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticEffectPtr.Handle 2c44f1bedd68013a0553a32105445854bc7adeb4a3296f8e880d6eb6ecaa555f -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticEffectPtr.IsNull d17ce8415d41f0cf60bdac0ef1547e30fc5d46a8034b73ee1434c789bf145079 -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticEffectPtr.Null 3035c86060e67b0fbd2daabf2ac4db1082bb01adaa74df06a1f48db3f20a2f57 -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticEffectPtr.Item(System.Int32) 6bce9971b642ac807d92271b12ae6347457a1040e314427c72a8ce18fd121e3f -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticEffectPtr.op_Implicit(Inno.Native.Sdl3.SDLHapticEffect*)~Inno.Native.Sdl3.SDLHapticEffectPtr 0d194bfca371b45e6e7dc3f63e2e17608ee70309753f1b804aba14c5844de9cd -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticEffectPtr.op_Implicit(Inno.Native.Sdl3.SDLHapticEffectPtr)~Inno.Native.Sdl3.SDLHapticEffect* afaf3e7d0c0a4cfd1a5cef6d9ef0454247d818e3e0856b47e21aae1d721f8fdd -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticEffectPtr.op_Equality(Inno.Native.Sdl3.SDLHapticEffectPtr,Inno.Native.Sdl3.SDLHapticEffectPtr) 5dd0f9846f99e6b1dee67cb96517089e9f4a1e575070d1845e894c9df4338d26 -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticEffectPtr.op_Inequality(Inno.Native.Sdl3.SDLHapticEffectPtr,Inno.Native.Sdl3.SDLHapticEffectPtr) c4016bfea6b8ddbd05c7a2496ac4805ebde3f927aea545b1bfead6dd767e41e3 -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticEffectPtr.Equals(Inno.Native.Sdl3.SDLHapticEffectPtr) bc8b21093a2b7f0d5c59e5bfcef3877743802a762f1348bad860e822d8cfd4f6 -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticEffectPtr.Equals(System.Object) 701658d2676295ad8329c182361641f38cbbbecda0074896936204feff5be5d2 -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticEffectPtr.GetHashCode 140f49da04744432de22a860738c9e8ef66dc7e82b3ffe474e80b3b3d21bf58f -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticEffectPtr.Type 9ffe7383f79fce52db38a55e38c579198c0c8fcc897419d5341889eee2a68374 -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticEffectPtr.Constant 18ab1ed6fc5bfc284a7e9ea6bd01e9bb2be029d6ec40b8f19f22019c22be34ae -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticEffectPtr.Periodic 697efc4911db1104fac6685f73315773d1837e6f7484eab67993182bb27ae37b -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticEffectPtr.Condition 4c10f8203127e42000f9f3fde627471cdcf7f0a0208378911dbcc0e45a1bec59 -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticEffectPtr.Ramp 7209b707bca84aa19365b6271b6b6ef8dbca7587575005c7071aa24534c6dcfd -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticEffectPtr.Leftright 451920d82d25e954723b7b58a7c8bcbfcf55cf23edddb889045d4c677e512b71 -->
<!-- api: P:Inno.Native.Sdl3.SDLHapticEffectPtr.Custom 2a1a7276fcfe58a25370d07c6a8716d69b426a5291244424dd03f8b02e4417a2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dc1aaf7071a87638"></a>[`public Inno.Native.Sdl3.SDLHapticEffect* Inno.Native.Sdl3.SDLHapticEffectPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9219) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad2640f870e14bd3"></a>[`public Inno.Native.Sdl3.SDLHapticEffectPtr.SDLHapticEffectPtr(Inno.Native.Sdl3.SDLHapticEffect* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9214) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f6207f310ae88872"></a>[`public bool Inno.Native.Sdl3.SDLHapticEffectPtr.Equals(Inno.Native.Sdl3.SDLHapticEffectPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9229) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-8527c8656061e1c7"></a>[`public override bool Inno.Native.Sdl3.SDLHapticEffectPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9230) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-b75f59b12776ad10"></a>[`public override int Inno.Native.Sdl3.SDLHapticEffectPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9231) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-98c8babee75d6d9e"></a>[`public static bool Inno.Native.Sdl3.SDLHapticEffectPtr.operator ==(Inno.Native.Sdl3.SDLHapticEffectPtr left, Inno.Native.Sdl3.SDLHapticEffectPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9227) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9a86ebb9e685ef7f"></a>[`public static Inno.Native.Sdl3.SDLHapticEffectPtr.implicit operator Inno.Native.Sdl3.SDLHapticEffectPtr(Inno.Native.Sdl3.SDLHapticEffect* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9225) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ed0c2f9b3a415d43"></a>[`public static Inno.Native.Sdl3.SDLHapticEffectPtr.implicit operator Inno.Native.Sdl3.SDLHapticEffect*(Inno.Native.Sdl3.SDLHapticEffectPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9226) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a6d0ffe269446a17"></a>[`public static bool Inno.Native.Sdl3.SDLHapticEffectPtr.operator !=(Inno.Native.Sdl3.SDLHapticEffectPtr left, Inno.Native.Sdl3.SDLHapticEffectPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9228) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-873e8ad456fccf54"></a>[`public ref Inno.Native.Sdl3.SDLHapticCondition Inno.Native.Sdl3.SDLHapticEffectPtr.Condition { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9236) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-74854f0710fe3eb9"></a>[`public ref Inno.Native.Sdl3.SDLHapticConstant Inno.Native.Sdl3.SDLHapticEffectPtr.Constant { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9234) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e351426805783427"></a>[`public ref Inno.Native.Sdl3.SDLHapticCustom Inno.Native.Sdl3.SDLHapticEffectPtr.Custom { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9239) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5a85dc5dbe9299ef"></a>[`public bool Inno.Native.Sdl3.SDLHapticEffectPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9220) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c993e8357f8a6146"></a>[`public Inno.Native.Sdl3.SDLHapticEffect Inno.Native.Sdl3.SDLHapticEffectPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9223) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1dbfe5bdf3cc77a7"></a>[`public ref Inno.Native.Sdl3.SDLHapticLeftRight Inno.Native.Sdl3.SDLHapticEffectPtr.Leftright { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9238) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1beafcc67bb1954c"></a>[`public static Inno.Native.Sdl3.SDLHapticEffectPtr Inno.Native.Sdl3.SDLHapticEffectPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9221) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-30d1f56757fe8d3c"></a>[`public ref Inno.Native.Sdl3.SDLHapticPeriodic Inno.Native.Sdl3.SDLHapticEffectPtr.Periodic { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9235) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f6a0e6616f7417ad"></a>[`public ref Inno.Native.Sdl3.SDLHapticRamp Inno.Native.Sdl3.SDLHapticEffectPtr.Ramp { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9237) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d591e3b886788697"></a>[`public ref ushort Inno.Native.Sdl3.SDLHapticEffectPtr.Type { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9233) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-55e81d788e04b1fb"></a>[`public struct Inno.Native.Sdl3.SDLHapticEffectPtr : System.IEquatable<Inno.Native.Sdl3.SDLHapticEffectPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9212) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHapticLeftRight</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHapticLeftRight bbaf13e2d7c096f1fdd34f942865d74668dc9ec7083b7be2f0dcf0a76c21f453 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticLeftRight.Type fa40db04c4592da1ed2150449863cab6c4b3c0c45a9661e55e1c5e05d18c9696 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticLeftRight.Length 707f66b223536a5e59ceac587629e29909dee9b054bdf9eaffafce3581afe487 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticLeftRight.LargeMagnitude beb8e497efa4158308fd0d9a7ff2e32049ca7f9f4ad1f7574a079c54c5af7d6f -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticLeftRight.SmallMagnitude faddedef3e0e692649ae0dab34d8c9e4ab0b1a8db2913022526020e436f9e8eb -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticLeftRight.#ctor(System.UInt16,System.UInt32,System.UInt16,System.UInt16) 41b2c115f4be65e5b78720f4fde4f75bbd08bd00681cb72480c28e646157123d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-84437e8516ba370b"></a>[`public ushort Inno.Native.Sdl3.SDLHapticLeftRight.LargeMagnitude`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5866) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c0ee66494db50ec1"></a>[`public uint Inno.Native.Sdl3.SDLHapticLeftRight.Length`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5865) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8621a70032829b50"></a>[`public ushort Inno.Native.Sdl3.SDLHapticLeftRight.SmallMagnitude`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5867) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-18eae651770621b0"></a>[`public ushort Inno.Native.Sdl3.SDLHapticLeftRight.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5864) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-73b9f47777176460"></a>[`public Inno.Native.Sdl3.SDLHapticLeftRight.SDLHapticLeftRight(ushort type = 0, uint length = 0, ushort large_magnitude = 0, ushort small_magnitude = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5868) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8f2925e277ba5978"></a>[`public struct Inno.Native.Sdl3.SDLHapticLeftRight`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5862) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHapticPeriodic</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHapticPeriodic 0f22cd8a7b4bf6720b434f86066f7130f505d9e12d540ea8c2cea6947812aaab -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.Type cf3263df63f6eef941b06df1047d2d6be2495ddf927ace602e9cf448a13da566 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.Direction f6907494e7f9eff3b51287797ff9d5b2f8b8d173fcc608d226ecf48e0515cd14 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.Length 32d7c29a24cdfb72e20e70b59c53d8fcdd955dc52c6667621a04575b83ee224f -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.Delay a86e77b54b3c08c20b9749fdf3249fc5602b1861a79e16f565e8c05f9b330a07 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.Button 868aafafb6dc59a2aecf65c81fa21860cf0e471200442cb08262722747ff0655 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.Interval 8dad549fb854d369d41808a7e3abf0b9c6519052b34c0b179232d0565df910d2 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.Period f671b7d9a1570583aee3601061b7a545633998077e8d778c43cb9bab4ca1d51b -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.Magnitude 3ce1cbe033773ffe1fbc9c13cfcc03b038c92a48178c9879085f74986910394b -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.Offset 676436f1f711f189bf50dc9cb2a20b8522ddd664a5f92a6adff9b7d0106f34c2 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.Phase de7fe5f3e9be9feb6adb9f0522e2990d6d87d36688fc490fdf3116c71865c9d3 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.AttackLength 3faab683cfa47ca3db370640d00db072472672e5be32399afc7fe3849e581abc -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.AttackLevel 5b34594d1b4c1e31f5de22eebff6e9cdaf8b0e07b8131dc42a3408242db18fc5 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.FadeLength 2b8c6653c264737d6d198927ab85c81391c8896bf393fc89fe83a72bbbc484d2 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticPeriodic.FadeLevel d5383afa6b565471593d98e8bd35d69a370b0a4f2c719e0bbe986f5af34d765b -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticPeriodic.#ctor(System.UInt16,Inno.Native.Sdl3.SDLHapticDirection,System.UInt32,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.Int16,System.Int16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16) 98c7cfb5e91645837d8de66147ca82cff9bcf28aeaa72fecc00b2c17e5d1365d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-89578510a465414f"></a>[`public ushort Inno.Native.Sdl3.SDLHapticPeriodic.AttackLength`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5706) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de653b3cf8d4ce36"></a>[`public ushort Inno.Native.Sdl3.SDLHapticPeriodic.AttackLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5707) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fa626aeb96ccc900"></a>[`public ushort Inno.Native.Sdl3.SDLHapticPeriodic.Button`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5700) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f0dad50c28c7767"></a>[`public ushort Inno.Native.Sdl3.SDLHapticPeriodic.Delay`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5699) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a38b7573086e52c4"></a>[`public Inno.Native.Sdl3.SDLHapticDirection Inno.Native.Sdl3.SDLHapticPeriodic.Direction`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5697) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec43eed01862d66c"></a>[`public ushort Inno.Native.Sdl3.SDLHapticPeriodic.FadeLength`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5708) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f517a506652786a"></a>[`public ushort Inno.Native.Sdl3.SDLHapticPeriodic.FadeLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5709) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1bd516368fed2663"></a>[`public ushort Inno.Native.Sdl3.SDLHapticPeriodic.Interval`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5701) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-90a0409d1db4ec09"></a>[`public uint Inno.Native.Sdl3.SDLHapticPeriodic.Length`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5698) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6d57642a5ad9cf78"></a>[`public short Inno.Native.Sdl3.SDLHapticPeriodic.Magnitude`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5703) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-10cbd5f54b343ea3"></a>[`public short Inno.Native.Sdl3.SDLHapticPeriodic.Offset`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5704) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-728a49a7b594b7ea"></a>[`public ushort Inno.Native.Sdl3.SDLHapticPeriodic.Period`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5702) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e73db9775f02c748"></a>[`public ushort Inno.Native.Sdl3.SDLHapticPeriodic.Phase`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5705) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4bea43378bae7bb5"></a>[`public ushort Inno.Native.Sdl3.SDLHapticPeriodic.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5696) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-485822f2691fdeae"></a>[`public Inno.Native.Sdl3.SDLHapticPeriodic.SDLHapticPeriodic(ushort type = 0, Inno.Native.Sdl3.SDLHapticDirection direction = default(Inno.Native.Sdl3.SDLHapticDirection), uint length = 0, ushort delay = 0, ushort button = 0, ushort interval = 0, ushort period = 0, short magnitude = 0, short offset = 0, ushort phase = 0, ushort attack_length = 0, ushort attack_level = 0, ushort fade_length = 0, ushort fade_level = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5710) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4150717593b1cba5"></a>[`public struct Inno.Native.Sdl3.SDLHapticPeriodic`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5694) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHapticRamp</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHapticRamp 496f6fc73bc5bd6dc7cf68227380f8c9cf7b5948a9f484b9f3b8a63aaa066ae6 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticRamp.Type 5cc6e99833750aa859da46352b319d2358a401b37b02bd03f83b1b680f76b4f6 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticRamp.Direction 847a9994b3cf543eb9482946ccef87be0c6537fcbeb1e3e3ffeb1529ecf20933 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticRamp.Length c6b5377e696b11acec687433011586e176283ab2e59f1e314fe5e57c238f44fd -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticRamp.Delay 9450170a3cbf901233385d9096c315cde071b44204d916e5c5ba5869905c3b3d -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticRamp.Button 6fd4ebf720e36c41c9c77d291b38b8461e1d2ec5eac0d3408e83d2274d1995ec -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticRamp.Interval 22e247515437dedf518f2a0a6aaa2518dde42740765adfd101b9d2f8b0f07b58 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticRamp.Start 6f940f5516156d213661ad0263e9379c23e9d635cb0f924fe904aa86b5dd7b5c -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticRamp.End 9c08bbdcc0bd6d63f7ff00ea3bc89f5090efef2d61dfc47abbe02a3e35d16975 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticRamp.AttackLength 518455f9cec0e33306ed07e246e71fc88769639276d33bb4b5689ea7982d5034 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticRamp.AttackLevel 29cb82f9e1ce421249e49f904b203d213981ccfa5634edd328570e2792d48bfb -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticRamp.FadeLength 0c0b19be580528affae83bd6946955a0e0cc7b49b2cc3ac3a9f6459f566d5646 -->
<!-- api: F:Inno.Native.Sdl3.SDLHapticRamp.FadeLevel b8d251a0536b4f03874be9c30166bcf2e9831853b7497edf0ff95803162c8938 -->
<!-- api: M:Inno.Native.Sdl3.SDLHapticRamp.#ctor(System.UInt16,Inno.Native.Sdl3.SDLHapticDirection,System.UInt32,System.UInt16,System.UInt16,System.UInt16,System.Int16,System.Int16,System.UInt16,System.UInt16,System.UInt16,System.UInt16) 491877343013bf1066346070dfef893f8a064edea89eb30f21d2e04f4e5e3370 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1f9d042766823528"></a>[`public ushort Inno.Native.Sdl3.SDLHapticRamp.AttackLength`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5837) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8abb2a1c95a2b026"></a>[`public ushort Inno.Native.Sdl3.SDLHapticRamp.AttackLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5838) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8006eb8e8522d9c1"></a>[`public ushort Inno.Native.Sdl3.SDLHapticRamp.Button`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5833) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a641e53e7385f300"></a>[`public ushort Inno.Native.Sdl3.SDLHapticRamp.Delay`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5832) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1d66ef1855a4f52"></a>[`public Inno.Native.Sdl3.SDLHapticDirection Inno.Native.Sdl3.SDLHapticRamp.Direction`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5830) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2b107dbcbd84e5f2"></a>[`public short Inno.Native.Sdl3.SDLHapticRamp.End`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5836) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e2a56ac21feb2ebc"></a>[`public ushort Inno.Native.Sdl3.SDLHapticRamp.FadeLength`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5839) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8d5ff3051f22de68"></a>[`public ushort Inno.Native.Sdl3.SDLHapticRamp.FadeLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5840) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3845ecb16392e126"></a>[`public ushort Inno.Native.Sdl3.SDLHapticRamp.Interval`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5834) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8d48429f0e38aa74"></a>[`public uint Inno.Native.Sdl3.SDLHapticRamp.Length`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5831) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab815650d71b7b60"></a>[`public short Inno.Native.Sdl3.SDLHapticRamp.Start`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5835) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-394b2e9fd0ee1e6f"></a>[`public ushort Inno.Native.Sdl3.SDLHapticRamp.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5829) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7046df1cc045776d"></a>[`public Inno.Native.Sdl3.SDLHapticRamp.SDLHapticRamp(ushort type = 0, Inno.Native.Sdl3.SDLHapticDirection direction = default(Inno.Native.Sdl3.SDLHapticDirection), uint length = 0, ushort delay = 0, ushort button = 0, ushort interval = 0, short start = 0, short end = 0, ushort attack_length = 0, ushort attack_level = 0, ushort fade_length = 0, ushort fade_level = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5841) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e5321b6c162187e1"></a>[`public struct Inno.Native.Sdl3.SDLHapticRamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5827) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHidBusType</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHidBusType 36a87c27d4b0633e3000bb21d9cdc68dd270447fdfbf452960ef6f9120431107 -->
<!-- api: F:Inno.Native.Sdl3.SDLHidBusType.ApiBusUnknown 6659b3d6d1270edd23ca7ca969339980f0bb6b39236d83c673cd0f7f210bfc16 -->
<!-- api: F:Inno.Native.Sdl3.SDLHidBusType.ApiBusUsb 4b1bb06a6f4452319e3bf45bd0b676e006573107739fb6ab68ebe6c800885a21 -->
<!-- api: F:Inno.Native.Sdl3.SDLHidBusType.ApiBusBluetooth 2b497b2f571f95c13b3b3cde28f31440ca74404a63c569833e800bd19236d90e -->
<!-- api: F:Inno.Native.Sdl3.SDLHidBusType.ApiBusI2C 86afa4c24c9cb92655f1fa847a33dd18333b2fbb28e6902822a0739f80f01336 -->
<!-- api: F:Inno.Native.Sdl3.SDLHidBusType.ApiBusSpi 59d9587f456fb63be8db77907fe71a030a819af4a7ffd0a36ebff27936a78277 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6e181113f6fe58bf"></a>[`Inno.Native.Sdl3.SDLHidBusType.ApiBusBluetooth = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1747) | 当前常量或枚举值：`2`。 | — |
| <a id="api-ed65d56ad00a3181"></a>[`Inno.Native.Sdl3.SDLHidBusType.ApiBusI2C = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1748) | 当前常量或枚举值：`3`。 | — |
| <a id="api-cf037849e21c7dbf"></a>[`Inno.Native.Sdl3.SDLHidBusType.ApiBusSpi = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1749) | 当前常量或枚举值：`4`。 | — |
| <a id="api-c48f71f1f7867da8"></a>[`Inno.Native.Sdl3.SDLHidBusType.ApiBusUnknown = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1745) | 当前常量或枚举值：`0`。 | — |
| <a id="api-d8c795eb4a0080a1"></a>[`Inno.Native.Sdl3.SDLHidBusType.ApiBusUsb = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1746) | 当前常量或枚举值：`1`。 | — |
| <a id="api-c256c0d4a9d4be62"></a>[`public enum Inno.Native.Sdl3.SDLHidBusType : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1743) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHidDevice</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHidDevice 8522bcc058dfe250573119d6df04f98f5ecadc8700dbc1d74085c1964bff20dd -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDevice.#ctor(System.IntPtr) b6e921b99e73d9bdf0858e9567148b2714c9538700495bf67cde6fb2d5e7384f -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDevice.Handle 6a2b20d34c9169e38bb02c30aee14df947793eff8b9e94aa4c5743dcf4e3bcba -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDevice.IsNull bfe82dfde9998fcb0ff079414805bc2930b74c2e5ef2b402e45b38b8a8622171 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDevice.Null fef8c728c7ee428dddd8e8ef08125ce17f7586164a969ffe434945de6abcea9e -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDevice.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLHidDevice 2998bcc854a313238fd1c0bf77efe5066f5459f4a967a1563eb394997edc7baa -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDevice.op_Equality(Inno.Native.Sdl3.SDLHidDevice,Inno.Native.Sdl3.SDLHidDevice) 5d814ffe41daaa5633cfc599e8215f799db8aeeaa01f66e9d4bb93262e9cd8f6 -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDevice.op_Inequality(Inno.Native.Sdl3.SDLHidDevice,Inno.Native.Sdl3.SDLHidDevice) a4bd688220b21535339aa4c39e737a9d2ca21eeb053adba9c9425c1b2f8a73bd -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDevice.Equals(Inno.Native.Sdl3.SDLHidDevice) c3020adb3213e44ec501d96ce26d9c723090f12e28fcb710bb66bfd8ae13d016 -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDevice.Equals(System.Object) 73de678f3cec4e1fb5954e38dcc3ddff2a7c7d11a2943a8c9825c12fc0dca3ab -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDevice.GetHashCode 75e27b474012a371c85e387fad7ddcf7053387f5ab775dbac581989753e46b7e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-50b94274d30570b4"></a>[`public Inno.Native.Sdl3.SDLHidDevice.SDLHidDevice(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5944) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-026f744a5c371807"></a>[`public bool Inno.Native.Sdl3.SDLHidDevice.Equals(Inno.Native.Sdl3.SDLHidDevice other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5956) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-efe5d88d2e6530d9"></a>[`public override bool Inno.Native.Sdl3.SDLHidDevice.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5957) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-f4178e14d9613af4"></a>[`public override int Inno.Native.Sdl3.SDLHidDevice.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5958) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-0934166241554b40"></a>[`public static bool Inno.Native.Sdl3.SDLHidDevice.operator ==(Inno.Native.Sdl3.SDLHidDevice left, Inno.Native.Sdl3.SDLHidDevice right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5954) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d8e08f65b97ecfce"></a>[`public static Inno.Native.Sdl3.SDLHidDevice.implicit operator Inno.Native.Sdl3.SDLHidDevice(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5953) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2b41f0d171d85d87"></a>[`public static bool Inno.Native.Sdl3.SDLHidDevice.operator !=(Inno.Native.Sdl3.SDLHidDevice left, Inno.Native.Sdl3.SDLHidDevice right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5955) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-be21bdd03dbd1585"></a>[`public nint Inno.Native.Sdl3.SDLHidDevice.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5949) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a0003845f801f1e0"></a>[`public bool Inno.Native.Sdl3.SDLHidDevice.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5950) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-37227d31a5a91041"></a>[`public static Inno.Native.Sdl3.SDLHidDevice Inno.Native.Sdl3.SDLHidDevice.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5951) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-41cbbac6a1d53b04"></a>[`public readonly struct Inno.Native.Sdl3.SDLHidDevice : System.IEquatable<Inno.Native.Sdl3.SDLHidDevice>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5942) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHidDeviceInfo</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHidDeviceInfo 211b3076a2ab185f26e775d196c286fbd3deea12a0208ce64c3b865804fb12d7 -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.Path 2c98f6d2b7cc206e3d2b3fd7e9ee57eed50993ae1444349c99ed12e88a47ecae -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.VendorId 03598c32ab2c4d8dc4192d65dc5ab226679fddf479012fbb6315b1229046b72d -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.ProductId 3b7956d3dc3727778953b9bbf1b619a83af9a975d5d2dd12e4c6ab58dab40387 -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.SerialNumber cbe1aea888a0a4f44f6ef4e867a897573f2e23b0589141108805ccc50ad08c69 -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.ReleaseNumber 9f6fa84cb1d599565eec12ae41e33d14793ab9f7ad3b9ab605182203d82c511b -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.ManufacturerString c71eb7ecacc2563fef94e4df75ca8c95a05bf0b0fc20ab828c891779a7c3c030 -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.ProductString 2b31bcfb2e8f92fe217e55c9312547893d3612ccb0516eba6cf01fa9d3a434f4 -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.UsagePage 9870e60094d061e46813eaf20bd142671fc01a0e4edb7ea3b2e3082030a14179 -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.Usage 3d03cda048a8339c74ff481ccc50fb2dd973a9ec750f84bf32e6753a66018eec -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.InterfaceNumber 65cc85fd388059a1645edf2aa0e49c541664d390f3162bd634580439541a78ac -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.InterfaceClass f2a063be880b2b389061c1dd4c346ac2b5587647928f1b20202a823c2708a3b0 -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.InterfaceSubclass ce9be96733ed0a4abed05b24fd05b2c3357fe7d0d37faff141e3a84b3d7b4f1c -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.InterfaceProtocol 23e4e36f42324a910817353230fcf9f8a09d0ebd627caf3f530c0b0d70ad728d -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.BusType c641033c7ce83c51e3092b19c432b4cbc943f97ddf25d6fb44be525e0a68d8d6 -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfo.Next abbc7569bcb3ceaeadca96f584604b6323b9bbc6cd3b34e39fa53db40e7ff422 -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDeviceInfo.#ctor(System.Byte*,System.UInt16,System.UInt16,System.UInt16*,System.UInt16,System.UInt16*,System.UInt16*,System.UInt16,System.UInt16,System.Int32,System.Int32,System.Int32,System.Int32,Inno.Native.Sdl3.SDLHidBusType,Inno.Native.Sdl3.SDLHidDeviceInfo*) 5980ead009e0f474aed320eca538979b40b7c1093ed27ee6d9968e8eb8afd52b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-77ffd6c097e85fb1"></a>[`public Inno.Native.Sdl3.SDLHidBusType Inno.Native.Sdl3.SDLHidDeviceInfo.BusType`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5981) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c1118cf2da5187b1"></a>[`public int Inno.Native.Sdl3.SDLHidDeviceInfo.InterfaceClass`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5978) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-37d6cc9229e1b2a2"></a>[`public int Inno.Native.Sdl3.SDLHidDeviceInfo.InterfaceNumber`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5977) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f122899660a82082"></a>[`public int Inno.Native.Sdl3.SDLHidDeviceInfo.InterfaceProtocol`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5980) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e22d1ede5b08000"></a>[`public int Inno.Native.Sdl3.SDLHidDeviceInfo.InterfaceSubclass`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5979) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03a0df227be48fbb"></a>[`public ushort* Inno.Native.Sdl3.SDLHidDeviceInfo.ManufacturerString`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5973) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-711d24ba8adff0ae"></a>[`public Inno.Native.Sdl3.SDLHidDeviceInfo* Inno.Native.Sdl3.SDLHidDeviceInfo.Next`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5982) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-005c7b8e37976aeb"></a>[`public byte* Inno.Native.Sdl3.SDLHidDeviceInfo.Path`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5968) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccb5f71811012e74"></a>[`public ushort Inno.Native.Sdl3.SDLHidDeviceInfo.ProductId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5970) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c466c7995c385ad3"></a>[`public ushort* Inno.Native.Sdl3.SDLHidDeviceInfo.ProductString`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5974) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3fb7266b71534550"></a>[`public ushort Inno.Native.Sdl3.SDLHidDeviceInfo.ReleaseNumber`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5972) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dba1a9fdb37d30d5"></a>[`public ushort* Inno.Native.Sdl3.SDLHidDeviceInfo.SerialNumber`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5971) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b554e9220adb27b6"></a>[`public ushort Inno.Native.Sdl3.SDLHidDeviceInfo.Usage`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5976) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-872a52afc0230e21"></a>[`public ushort Inno.Native.Sdl3.SDLHidDeviceInfo.UsagePage`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5975) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69f2c7f530a827c3"></a>[`public ushort Inno.Native.Sdl3.SDLHidDeviceInfo.VendorId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5969) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c6e1782624a270d"></a>[`public Inno.Native.Sdl3.SDLHidDeviceInfo.SDLHidDeviceInfo(byte* path = null, ushort vendor_id = 0, ushort product_id = 0, ushort* serial_number = null, ushort release_number = 0, ushort* manufacturer_string = null, ushort* product_string = null, ushort usage_page = 0, ushort usage = 0, int interface_number = 0, int interface_class = 0, int interface_subclass = 0, int interface_protocol = 0, Inno.Native.Sdl3.SDLHidBusType bus_type = Inno.Native.Sdl3.SDLHidBusType.ApiBusUnknown, Inno.Native.Sdl3.SDLHidDeviceInfo* next = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5983) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6c95aec3108664a3"></a>[`public struct Inno.Native.Sdl3.SDLHidDeviceInfo`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5966) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHidDeviceInfoPtr</code> · 28 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHidDeviceInfoPtr a5d5314ab78a8371d4a171cf4456dc4194e27207cb5fea105aef1ca7180dbe15 -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.#ctor(Inno.Native.Sdl3.SDLHidDeviceInfo*) 1241e9416adae815314ae762be6f1dfffc4bf88d5d8950f9870d424f351c04c1 -->
<!-- api: F:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Handle a72841038b8ccb98d9af87174c33bb6432d2f213aa39af0a29239c38390a5ac5 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.IsNull 3656ee443ac5817236d3a53a2b677fefc855790c34bd277cc12c50048fc3c824 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Null 937f5b184b9db3cfe24f69633e43be4ebb755ae1b70bf752ff60bb43becf13af -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Item(System.Int32) 66e9174a5e2e560f17e3f1d825a44638cec223023251b63853ba9669838fd6e5 -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLHidDeviceInfo*)~Inno.Native.Sdl3.SDLHidDeviceInfoPtr d7e038c1c19c13b863bcd0afd59127c338513258c7af9e647b00847b14979253 -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLHidDeviceInfoPtr)~Inno.Native.Sdl3.SDLHidDeviceInfo* 6701259af50c8f3e3a2e4d11ebc54c1ed0637541fd0dcacd75d218b804852252 -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.op_Equality(Inno.Native.Sdl3.SDLHidDeviceInfoPtr,Inno.Native.Sdl3.SDLHidDeviceInfoPtr) aaaedadbdb7bc675a49567afe1f8a05b3c2af4e62207681e96dd2bfa192f8fcd -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.op_Inequality(Inno.Native.Sdl3.SDLHidDeviceInfoPtr,Inno.Native.Sdl3.SDLHidDeviceInfoPtr) 5ec7c2f9ac1c14a2bbd941abab80cc5406f6ee9a4e74bb0d0e14bd4fb0ef874f -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Equals(Inno.Native.Sdl3.SDLHidDeviceInfoPtr) 6182161493313dbcd8c3e91e75c6abf37ea8dbbaff2afc93ffd9c7373514b029 -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Equals(System.Object) 92b9e67415d9026587da9e907b64169cbd8c73780498d250746d5820b74245b8 -->
<!-- api: M:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.GetHashCode bd8a600c6e53ea7b5f6d3ff6eabbac8214a08a2fce97144c850b8b6e99c50863 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Path 89c198a1f4c4881e3aca0cda5e7265ed02d024e8dfea52dd86a9f9eb28fbdf56 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.VendorId 44acaf374136d1febc7cd6bf681d69af3a12cacc37c3ff1600c36b33b80d17db -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.ProductId 98995686ac4eacbba0869870ccb3eb6b2418540aac908d5588b4adf44d3dee13 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.SerialNumber 9eae90cc566fd55b49335ef94457b5a490cc6c89e626b1c96bac6c1ebd9158f5 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.ReleaseNumber 1e792e9bfc0be32b3368327fcfefba6af37ab197db63448d6a053238c573ebbd -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.ManufacturerString 42ed37b0b53a9829d727d86579160754a3f89c458cf657dd44046b80bee96694 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.ProductString c9ec90fd61a631349c893d89981ebd13a8a96220d1a63fb0c83fdb046d15ed43 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.UsagePage d76ec041e05762a63cc1abf6d13af1b67599df3c2307cbdec90dd9ca09d90942 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Usage 7ce53d5c2e288e324bf95d9ca5a00527ebedba34a77c6aa384f010c113ec9fbb -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.InterfaceNumber 178b16a3db7032f943198e7ba498cd3bca3f261e231dbeacb7608c8510fbab80 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.InterfaceClass 83885d0c5e35104cdc7249bbdf4d4698a314ae49fa7e567f9bbdbd95f6462186 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.InterfaceSubclass 2e21f06fad45b1c2d882609260ec25291a0d3425dc6421e8835ea75484931df6 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.InterfaceProtocol 0eda5b2f1e77e7a2bfbb7d3f7b99277325bc9c2e0656d6f82091eeb584cfac0f -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.BusType 7149ce6f5ef9b98460ac89f734a4d96761ea96ae27b7e2a28782449fb0bcdc05 -->
<!-- api: P:Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Next f876008db687e55df3f3d7004ae14880a94f1da4b6d332cb285d2b2499a0315b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0b3d101883ffb7f2"></a>[`public Inno.Native.Sdl3.SDLHidDeviceInfo* Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9250) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a1c4bffe9e935384"></a>[`public Inno.Native.Sdl3.SDLHidDeviceInfoPtr.SDLHidDeviceInfoPtr(Inno.Native.Sdl3.SDLHidDeviceInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9245) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8e46192d966f7029"></a>[`public bool Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Equals(Inno.Native.Sdl3.SDLHidDeviceInfoPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9260) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-6e155312eda21f7a"></a>[`public override bool Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9261) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-bed6ba64998b3847"></a>[`public override int Inno.Native.Sdl3.SDLHidDeviceInfoPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9262) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-315c2d06b34b5cc4"></a>[`public static bool Inno.Native.Sdl3.SDLHidDeviceInfoPtr.operator ==(Inno.Native.Sdl3.SDLHidDeviceInfoPtr left, Inno.Native.Sdl3.SDLHidDeviceInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9258) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f4f9aed9b05d9cc1"></a>[`public static Inno.Native.Sdl3.SDLHidDeviceInfoPtr.implicit operator Inno.Native.Sdl3.SDLHidDeviceInfoPtr(Inno.Native.Sdl3.SDLHidDeviceInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9256) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7650e6d90c3beb91"></a>[`public static Inno.Native.Sdl3.SDLHidDeviceInfoPtr.implicit operator Inno.Native.Sdl3.SDLHidDeviceInfo*(Inno.Native.Sdl3.SDLHidDeviceInfoPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9257) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9d3faab8fb384548"></a>[`public static bool Inno.Native.Sdl3.SDLHidDeviceInfoPtr.operator !=(Inno.Native.Sdl3.SDLHidDeviceInfoPtr left, Inno.Native.Sdl3.SDLHidDeviceInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9259) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-de27b289286942bb"></a>[`public ref Inno.Native.Sdl3.SDLHidBusType Inno.Native.Sdl3.SDLHidDeviceInfoPtr.BusType { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9277) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-48c49166eec2e8b4"></a>[`public ref int Inno.Native.Sdl3.SDLHidDeviceInfoPtr.InterfaceClass { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9274) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f6485f409667fceb"></a>[`public ref int Inno.Native.Sdl3.SDLHidDeviceInfoPtr.InterfaceNumber { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9273) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-020d5a40c5a5cb71"></a>[`public ref int Inno.Native.Sdl3.SDLHidDeviceInfoPtr.InterfaceProtocol { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9276) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a8af03dee26e1af2"></a>[`public ref int Inno.Native.Sdl3.SDLHidDeviceInfoPtr.InterfaceSubclass { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9275) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b7c37d5385f5db56"></a>[`public bool Inno.Native.Sdl3.SDLHidDeviceInfoPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9251) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1c4636a298caaa61"></a>[`public Inno.Native.Sdl3.SDLHidDeviceInfo Inno.Native.Sdl3.SDLHidDeviceInfoPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9254) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-37a5ef775e20a29b"></a>[`public ushort* Inno.Native.Sdl3.SDLHidDeviceInfoPtr.ManufacturerString { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9269) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-64382db4b597e63d"></a>[`public ref Inno.Native.Sdl3.SDLHidDeviceInfoPtr Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Next { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9278) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c2e5f66f7b56abc3"></a>[`public static Inno.Native.Sdl3.SDLHidDeviceInfoPtr Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9252) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-16f5a52df6cea3c9"></a>[`public byte* Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Path { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9264) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7ab9ed7897820c2f"></a>[`public ref ushort Inno.Native.Sdl3.SDLHidDeviceInfoPtr.ProductId { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9266) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4265a2819b8e823e"></a>[`public ushort* Inno.Native.Sdl3.SDLHidDeviceInfoPtr.ProductString { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9270) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1c0a4a250876bf9c"></a>[`public ref ushort Inno.Native.Sdl3.SDLHidDeviceInfoPtr.ReleaseNumber { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9268) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b0384b4d11e956f9"></a>[`public ushort* Inno.Native.Sdl3.SDLHidDeviceInfoPtr.SerialNumber { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9267) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7f43e5c520e18957"></a>[`public ref ushort Inno.Native.Sdl3.SDLHidDeviceInfoPtr.Usage { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9272) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-79f7b24346f28450"></a>[`public ref ushort Inno.Native.Sdl3.SDLHidDeviceInfoPtr.UsagePage { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9271) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fd850eb6179b7df3"></a>[`public ref ushort Inno.Native.Sdl3.SDLHidDeviceInfoPtr.VendorId { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9265) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a7120b31995b6122"></a>[`public struct Inno.Native.Sdl3.SDLHidDeviceInfoPtr : System.IEquatable<Inno.Native.Sdl3.SDLHidDeviceInfoPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9243) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHintCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHintCallback ffe8b5c315042f1dffd006189d6c54980a6cc7140346b3193995e671aa7b80fe -->
<!-- api: M:Inno.Native.Sdl3.SDLHintCallback.#ctor(System.Object,System.IntPtr) 73bdb8351e8489c15e12895218cf827cb8cd3c5337336ff3b8af516a1407da28 -->
<!-- api: M:Inno.Native.Sdl3.SDLHintCallback.Invoke(System.Void*,System.Byte*,System.Byte*,System.Byte*) ea21f280f9dfc34067ff4cdec453603074129371174173abdf8dba88fa0af43f -->
<!-- api: M:Inno.Native.Sdl3.SDLHintCallback.BeginInvoke(System.Void*,System.Byte*,System.Byte*,System.Byte*,System.AsyncCallback,System.Object) 823293bf51b8e607d9f0e6e32f4046cd75ae355eb9b4438078ecfe726768eec1 -->
<!-- api: M:Inno.Native.Sdl3.SDLHintCallback.EndInvoke(System.IAsyncResult) cf547eb04cb043c001be9268e7983799fe84cd415dc72d37526c97a1e6b8c159 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3babe7f66ea9683a"></a>[`public Inno.Native.Sdl3.SDLHintCallback.SDLHintCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6880) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-432cac314edb49c9"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLHintCallback.BeginInvoke(void* userdata, byte* name, byte* oldValue, byte* newValue, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6880) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-041e021aefe4ddf1"></a>[`public virtual void Inno.Native.Sdl3.SDLHintCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6880) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-487c13019a8eb8c5"></a>[`public virtual void Inno.Native.Sdl3.SDLHintCallback.Invoke(void* userdata, byte* name, byte* oldValue, byte* newValue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6880) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6bb23e34753b72db"></a>[`public delegate void Inno.Native.Sdl3.SDLHintCallback(void* userdata, byte* name, byte* oldValue, byte* newValue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6880) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHintPriority</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHintPriority cf940158e8693fd799aada82c5b295844350b7414a1a38e3f3a4b1be4e4b4941 -->
<!-- api: F:Inno.Native.Sdl3.SDLHintPriority.Default 763a521b31e201ebebf532a2c77caae7b03e99e42037bac158a10b0d0d06fc81 -->
<!-- api: F:Inno.Native.Sdl3.SDLHintPriority.Normal a7154521d5fcfd675188c12c0fd7dda2f18d92a874f7d568fff0935e545097b1 -->
<!-- api: F:Inno.Native.Sdl3.SDLHintPriority.Override 137cf0c47c4317526b1bf78594bd7afe6fcdd84c51b533e42ba734d7f826cd0f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-79f829083f94f738"></a>[`Inno.Native.Sdl3.SDLHintPriority.Default = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1757) | 当前常量或枚举值：`0`。 | — |
| <a id="api-cb05c125e0f6df99"></a>[`Inno.Native.Sdl3.SDLHintPriority.Normal = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1758) | 当前常量或枚举值：`1`。 | — |
| <a id="api-a6798b20009135ee"></a>[`Inno.Native.Sdl3.SDLHintPriority.Override = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1759) | 当前常量或枚举值：`2`。 | — |
| <a id="api-ace30a58d2b8f057"></a>[`public enum Inno.Native.Sdl3.SDLHintPriority : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1755) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHitTest</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHitTest 6a3d4cd68b1af96b826f214859001a04e19d6930324d4c40a392d84e1ad8bf6c -->
<!-- api: M:Inno.Native.Sdl3.SDLHitTest.#ctor(System.Object,System.IntPtr) 439558476a5b3a879b443e250d177a63502f84a11a0594154490a566b35943a2 -->
<!-- api: M:Inno.Native.Sdl3.SDLHitTest.Invoke(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLPoint*,System.Void*) d7a373556a53f2756e4a39979b04afe3759ddc89a2b4b9b3046e063773dfc5a6 -->
<!-- api: M:Inno.Native.Sdl3.SDLHitTest.BeginInvoke(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLPoint*,System.Void*,System.AsyncCallback,System.Object) e400b5606419a4dc102ecc681a8210e68bdc7cf9de9ddcc0ed331b95b6a3164b -->
<!-- api: M:Inno.Native.Sdl3.SDLHitTest.EndInvoke(System.IAsyncResult) 5f4d469eb97807bf4f2dd051dd6dbdace20f99a5743e300a4ddaf46c812577ca -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0a1fd2e510071d2f"></a>[`public Inno.Native.Sdl3.SDLHitTest.SDLHitTest(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6860) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a8c46b2d7d97591f"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLHitTest.BeginInvoke(Inno.Native.Sdl3.SDLWindow win, Inno.Native.Sdl3.SDLPoint* area, void* data, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6860) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9d30f490415a7e7f"></a>[`public virtual Inno.Native.Sdl3.SDLHitTestResult Inno.Native.Sdl3.SDLHitTest.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6860) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-179fda10426e9336"></a>[`public virtual Inno.Native.Sdl3.SDLHitTestResult Inno.Native.Sdl3.SDLHitTest.Invoke(Inno.Native.Sdl3.SDLWindow win, Inno.Native.Sdl3.SDLPoint* area, void* data)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6860) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d7f6ffaada68b8b1"></a>[`public delegate Inno.Native.Sdl3.SDLHitTestResult Inno.Native.Sdl3.SDLHitTest(Inno.Native.Sdl3.SDLWindow win, Inno.Native.Sdl3.SDLPoint* area, void* data)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6860) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLHitTestResult</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLHitTestResult 8ae4e711e6d671f5e2c436090c5073d8c7a3eb6b5bf1204a9f9028b32bc95199 -->
<!-- api: F:Inno.Native.Sdl3.SDLHitTestResult.HittestNormal fcc63299c00f5b60f8bee335516d1307e0ca8600aa79c06bca8d12e1b6836e7b -->
<!-- api: F:Inno.Native.Sdl3.SDLHitTestResult.HittestDraggable 73b804c74eb077bb3ec26776674c6dc281b96e9839b6b46ca8d1eec6f9dd5974 -->
<!-- api: F:Inno.Native.Sdl3.SDLHitTestResult.HittestResizeTopleft 74c3970ea3939fb0f644cc9a69c424d630b4eb85c9b7461db13a079a3304a1a6 -->
<!-- api: F:Inno.Native.Sdl3.SDLHitTestResult.HittestResizeTop 3e2e9aa1f6d4bd7cec510d8d8c8d508144773cadfed39637ef24a0513caa2868 -->
<!-- api: F:Inno.Native.Sdl3.SDLHitTestResult.HittestResizeTopright 54588665f78fdf59af3bc5a75d410d1ea2503f1e0000cd21f0a4cb01d9d2cf8a -->
<!-- api: F:Inno.Native.Sdl3.SDLHitTestResult.HittestResizeRight 3787a6d30b052c71be1be4a520b6cdb2d0fc4bdadbac44987e0ce5279b6a9a44 -->
<!-- api: F:Inno.Native.Sdl3.SDLHitTestResult.HittestResizeBottomright bc2d2eddf5af454d7eebaba06314636a5c5584de55628e9c23aeb897e07d116b -->
<!-- api: F:Inno.Native.Sdl3.SDLHitTestResult.HittestResizeBottom 26602413cea8369d812be43591c3fe3ad18b3a8281b5efea4119044ae47d1d39 -->
<!-- api: F:Inno.Native.Sdl3.SDLHitTestResult.HittestResizeBottomleft 5ccf85a95223b1a31828f2bdc8900d2216771efe8b8eddb5d8abdd44fcc05fb6 -->
<!-- api: F:Inno.Native.Sdl3.SDLHitTestResult.HittestResizeLeft 35d2c18fa2dd2d99d9ab0c142e76a44fd8cf2b7516fbf4476feed9af3741b6b8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f3c7b1498538c10e"></a>[`Inno.Native.Sdl3.SDLHitTestResult.HittestDraggable = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L628) | 当前常量或枚举值：`1`。 | — |
| <a id="api-040dca3d3fc4310e"></a>[`Inno.Native.Sdl3.SDLHitTestResult.HittestNormal = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L627) | 当前常量或枚举值：`0`。 | — |
| <a id="api-4988590e66a8af0a"></a>[`Inno.Native.Sdl3.SDLHitTestResult.HittestResizeBottom = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L634) | 当前常量或枚举值：`7`。 | — |
| <a id="api-862f1957768512a0"></a>[`Inno.Native.Sdl3.SDLHitTestResult.HittestResizeBottomleft = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L635) | 当前常量或枚举值：`8`。 | — |
| <a id="api-0c8142f53d8da307"></a>[`Inno.Native.Sdl3.SDLHitTestResult.HittestResizeBottomright = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L633) | 当前常量或枚举值：`6`。 | — |
| <a id="api-a858c0b9bdf18440"></a>[`Inno.Native.Sdl3.SDLHitTestResult.HittestResizeLeft = 9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L636) | 当前常量或枚举值：`9`。 | — |
| <a id="api-5f04d24113edd2f2"></a>[`Inno.Native.Sdl3.SDLHitTestResult.HittestResizeRight = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L632) | 当前常量或枚举值：`5`。 | — |
| <a id="api-e9d3018ec8af7960"></a>[`Inno.Native.Sdl3.SDLHitTestResult.HittestResizeTop = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L630) | 当前常量或枚举值：`3`。 | — |
| <a id="api-2a20f752cc67658c"></a>[`Inno.Native.Sdl3.SDLHitTestResult.HittestResizeTopleft = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L629) | 当前常量或枚举值：`2`。 | — |
| <a id="api-72168b0235b83c3e"></a>[`Inno.Native.Sdl3.SDLHitTestResult.HittestResizeTopright = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L631) | 当前常量或枚举值：`4`。 | — |
| <a id="api-131a0e0b8f9ad0d0"></a>[`public enum Inno.Native.Sdl3.SDLHitTestResult : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L625) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLIconv</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLIconv 0f3db5a773bd92fc3605eb88980cc5fd518c9da6a4164512df4f4aa7252da2ac -->
<!-- api: M:Inno.Native.Sdl3.SDLIconv.#ctor(System.IntPtr) a0e4a0a4a1cc699c373d3da9e55fb22168d057037984f68224e38c6ed4231be1 -->
<!-- api: P:Inno.Native.Sdl3.SDLIconv.Handle 7f41036f6fa5671d4ec3181ba56231812b11335f0e4769118073ad61d2d0892b -->
<!-- api: P:Inno.Native.Sdl3.SDLIconv.IsNull b1e2a8405dd1a92ff680b63f481e87164519e02a677bb11016018150e9c154c3 -->
<!-- api: P:Inno.Native.Sdl3.SDLIconv.Null 8800cf9200823e1b9916feab65f0614a86ee23c56bbf40007a9f3f06760f9e97 -->
<!-- api: M:Inno.Native.Sdl3.SDLIconv.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLIconv 15c6ba4f498b0c8a536c6a308dbd5bdd0045bdbeb1d0ee61bd788cd24761b748 -->
<!-- api: M:Inno.Native.Sdl3.SDLIconv.op_Equality(Inno.Native.Sdl3.SDLIconv,Inno.Native.Sdl3.SDLIconv) adc162d403c8949ebcffc55325e8e76516b0e381371fb86d0dcceee004d2bf65 -->
<!-- api: M:Inno.Native.Sdl3.SDLIconv.op_Inequality(Inno.Native.Sdl3.SDLIconv,Inno.Native.Sdl3.SDLIconv) 54ae8dece34296b690d02cbde6e9f46dfbc4fc2e57625184054e11947035a090 -->
<!-- api: M:Inno.Native.Sdl3.SDLIconv.Equals(Inno.Native.Sdl3.SDLIconv) 41117eeeaed884501236ec2a53694bb42c43345a73ea591d8d83be53cbce8a83 -->
<!-- api: M:Inno.Native.Sdl3.SDLIconv.Equals(System.Object) f8733db79a03c317448b723248b9faf21946cd6ab6a8fcbefeab51a679efdb4c -->
<!-- api: M:Inno.Native.Sdl3.SDLIconv.GetHashCode 43a48b3271232f53f2d96fc2d8b96a8e4991c988bb3d99528e29d09f8163ceea -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-42d541ffab358072"></a>[`public Inno.Native.Sdl3.SDLIconv.SDLIconv(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6476) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3a733bb0f99be914"></a>[`public bool Inno.Native.Sdl3.SDLIconv.Equals(Inno.Native.Sdl3.SDLIconv other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6488) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-21413f248792ab56"></a>[`public override bool Inno.Native.Sdl3.SDLIconv.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6489) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-9c5d5bb11b4f8abf"></a>[`public override int Inno.Native.Sdl3.SDLIconv.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6490) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-979fbd32e2c59256"></a>[`public static bool Inno.Native.Sdl3.SDLIconv.operator ==(Inno.Native.Sdl3.SDLIconv left, Inno.Native.Sdl3.SDLIconv right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6486) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-64d71ec65d0ffba2"></a>[`public static Inno.Native.Sdl3.SDLIconv.implicit operator Inno.Native.Sdl3.SDLIconv(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6485) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3d050360178022e9"></a>[`public static bool Inno.Native.Sdl3.SDLIconv.operator !=(Inno.Native.Sdl3.SDLIconv left, Inno.Native.Sdl3.SDLIconv right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6487) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8f6482d3427f9a39"></a>[`public nint Inno.Native.Sdl3.SDLIconv.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6481) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-04ecc195fc75052c"></a>[`public bool Inno.Native.Sdl3.SDLIconv.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6482) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a727377d2bb3a414"></a>[`public static Inno.Native.Sdl3.SDLIconv Inno.Native.Sdl3.SDLIconv.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6483) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ed40bbf40916c19"></a>[`public readonly struct Inno.Native.Sdl3.SDLIconv : System.IEquatable<Inno.Native.Sdl3.SDLIconv>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6474) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLIconvData</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLIconvData b85cde49003d35ea52100b3dba36a501e7ee2bada41c7af1403df94e7371a6a7 -->
<!-- api: M:Inno.Native.Sdl3.SDLIconvData.#ctor(System.IntPtr) 50c691fe8dc33ff627f93fa0e22413b09c618818838b575c6bb0e3fb68a40850 -->
<!-- api: P:Inno.Native.Sdl3.SDLIconvData.Handle 163317c828b1757ea4bda0e373d6a1d2f625e8569db3b701a549a2fc31a35b97 -->
<!-- api: P:Inno.Native.Sdl3.SDLIconvData.IsNull deda908420f638ef180a8fe66370b4b4c54a7c459800fdde7a30a6551175c3a0 -->
<!-- api: P:Inno.Native.Sdl3.SDLIconvData.Null 786825325ff764a0bd660f62de9f66edfd06ccd811f726f5000432e01a5a46e6 -->
<!-- api: M:Inno.Native.Sdl3.SDLIconvData.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLIconvData f93b167b4c496b16c5f0f56bf9480f5b973990010e8e17a3715f5f9da46057a2 -->
<!-- api: M:Inno.Native.Sdl3.SDLIconvData.op_Equality(Inno.Native.Sdl3.SDLIconvData,Inno.Native.Sdl3.SDLIconvData) 118f27ca04243497d05ba19916a4d7fca22e7ac6a9ceb055a5868ffb2f396f3f -->
<!-- api: M:Inno.Native.Sdl3.SDLIconvData.op_Inequality(Inno.Native.Sdl3.SDLIconvData,Inno.Native.Sdl3.SDLIconvData) 26254fbc9c7b11f68dcebc9a667617b9e96c57bbf664e01ab814d95cdda666ac -->
<!-- api: M:Inno.Native.Sdl3.SDLIconvData.Equals(Inno.Native.Sdl3.SDLIconvData) 22027a88bb8d409e5da9cdfd9a77b2b7a9e852f2a91a2ad2a7a5931e8483c023 -->
<!-- api: M:Inno.Native.Sdl3.SDLIconvData.Equals(System.Object) b5ddebb0e882a5553c900b0c70d2b0f71c1b827a916f39dbb13118261a804f92 -->
<!-- api: M:Inno.Native.Sdl3.SDLIconvData.GetHashCode 0047dc7b8a7ae7d5bdcb5b7c1b36647d8752d5d5c92c432c2cd7c062f08f6f63 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e071828f1cd0a687"></a>[`public Inno.Native.Sdl3.SDLIconvData.SDLIconvData(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1948) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9fe291ddfa73d0e9"></a>[`public bool Inno.Native.Sdl3.SDLIconvData.Equals(Inno.Native.Sdl3.SDLIconvData other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1960) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-4a0727a125f1b970"></a>[`public override bool Inno.Native.Sdl3.SDLIconvData.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1961) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-03e981a4a919c403"></a>[`public override int Inno.Native.Sdl3.SDLIconvData.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1962) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-abb23dc6c5490f47"></a>[`public static bool Inno.Native.Sdl3.SDLIconvData.operator ==(Inno.Native.Sdl3.SDLIconvData left, Inno.Native.Sdl3.SDLIconvData right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1958) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-28cc5c77a8fa4817"></a>[`public static Inno.Native.Sdl3.SDLIconvData.implicit operator Inno.Native.Sdl3.SDLIconvData(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1957) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2d47d1b64313b91b"></a>[`public static bool Inno.Native.Sdl3.SDLIconvData.operator !=(Inno.Native.Sdl3.SDLIconvData left, Inno.Native.Sdl3.SDLIconvData right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1959) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cf9b6661d58214c0"></a>[`public nint Inno.Native.Sdl3.SDLIconvData.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1953) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d8934353b88c6a5b"></a>[`public bool Inno.Native.Sdl3.SDLIconvData.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1954) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bcca0f09cdc838a7"></a>[`public static Inno.Native.Sdl3.SDLIconvData Inno.Native.Sdl3.SDLIconvData.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1955) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-64b6f683dfe3e368"></a>[`public readonly struct Inno.Native.Sdl3.SDLIconvData : System.IEquatable<Inno.Native.Sdl3.SDLIconvData>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1946) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLInitFlags</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLInitFlags a6ec23c2dc82a774678e112e08de5d9e7938fc12f339cbb097f58f874967d63e -->
<!-- api: F:Inno.Native.Sdl3.SDLInitFlags.Timer 75dd8c05e5d883dc9d33bb2e183b7d56fa342533bb59afb8683ecb8fa14d362a -->
<!-- api: F:Inno.Native.Sdl3.SDLInitFlags.Audio aad0c92fa911766687122f56631c6425276c458b539f79974c269b029ae62a62 -->
<!-- api: F:Inno.Native.Sdl3.SDLInitFlags.Video 2e3fd0a577242078800e4004460a572fe6ecd020b6be7b69e2e0bdb34ca0def6 -->
<!-- api: F:Inno.Native.Sdl3.SDLInitFlags.Joystick 0aee0a8bf5e6b101e19b4394fa4e1a25e161bdd1ae8d01d3fb913bd82931ae20 -->
<!-- api: F:Inno.Native.Sdl3.SDLInitFlags.Haptic c1770488cdb304cfc936b66e227aec17c26bb8893252a90bc3e4afc0a1fbbbaa -->
<!-- api: F:Inno.Native.Sdl3.SDLInitFlags.Gamepad 17a33e885232819a21f2133ea0bdef11299c058b1d56aa0fe8eba0a4ee87d832 -->
<!-- api: F:Inno.Native.Sdl3.SDLInitFlags.Events 1a8c92dfe5e48af7531d32d75125686d6db39b049bdcbd759a424899bd95dac5 -->
<!-- api: F:Inno.Native.Sdl3.SDLInitFlags.Sensor 72c8be9ccc8353b80d23f792a05447b17e9f5821ce705dca5fc2a29701563f3c -->
<!-- api: F:Inno.Native.Sdl3.SDLInitFlags.Camera f279be2ed1e9bc41fa5c54199db99faa11b22cb7b0fa2750f3438b84adfe1ac4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8b8d9b855fe47bec"></a>[`Inno.Native.Sdl3.SDLInitFlags.Audio = 16`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6537) | 当前常量或枚举值：`16`。 | — |
| <a id="api-ca77e6364b71eefc"></a>[`Inno.Native.Sdl3.SDLInitFlags.Camera = 65536`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6544) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-fc5bf6794fc3070a"></a>[`Inno.Native.Sdl3.SDLInitFlags.Events = 16384`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6542) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-4e8b6711bd241bd8"></a>[`Inno.Native.Sdl3.SDLInitFlags.Gamepad = 8192`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6541) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-6d6a93e92671385b"></a>[`Inno.Native.Sdl3.SDLInitFlags.Haptic = 4096`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6540) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-2540613c559304e8"></a>[`Inno.Native.Sdl3.SDLInitFlags.Joystick = 512`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6539) | 当前常量或枚举值：`512`。 | — |
| <a id="api-e2945a1cf3e09ce5"></a>[`Inno.Native.Sdl3.SDLInitFlags.Sensor = 32768`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6543) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-5cc1074796d7b6c7"></a>[`Inno.Native.Sdl3.SDLInitFlags.Timer = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6536) | 当前常量或枚举值：`1`。 | — |
| <a id="api-81c68b2fefba82e1"></a>[`Inno.Native.Sdl3.SDLInitFlags.Video = 32`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6538) | 当前常量或枚举值：`32`。 | — |
| <a id="api-a513e5d801f02f47"></a>[`public enum Inno.Native.Sdl3.SDLInitFlags : uint`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6534) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLInitState</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLInitState 774ada7cd873ec328b2a7734fd8d741349594f78b2b8c297e4585a682d2f3537 -->
<!-- api: F:Inno.Native.Sdl3.SDLInitState.Status 3d0312c95fa0e8e33ca00be24cb4f961293327587d8fc681c2e7547a88af6dd5 -->
<!-- api: F:Inno.Native.Sdl3.SDLInitState.Thread 28c0678cfa5a72c082c983064ef1f721e82acf3d5ee7b67602a1a0d2f0e2e3fa -->
<!-- api: F:Inno.Native.Sdl3.SDLInitState.Reserved 74c8d5b39d8fe075c8f85648a7040d576098209ebda243654418c95bf00b4e04 -->
<!-- api: M:Inno.Native.Sdl3.SDLInitState.#ctor(Inno.Native.Sdl3.SDLAtomicInt,System.UInt64,System.Void*) 726bcda83e8f134842f545fb99cd7bd8c16c75be03d6c12b655a4d183ee35a94 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-bba58cf7b55126d4"></a>[`public void* Inno.Native.Sdl3.SDLInitState.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2220) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-86deda26a73802f1"></a>[`public Inno.Native.Sdl3.SDLAtomicInt Inno.Native.Sdl3.SDLInitState.Status`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2218) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64f67e59bbedf93e"></a>[`public ulong Inno.Native.Sdl3.SDLInitState.Thread`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2219) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39a1fb0a30aed364"></a>[`public Inno.Native.Sdl3.SDLInitState.SDLInitState(Inno.Native.Sdl3.SDLAtomicInt status = default(Inno.Native.Sdl3.SDLAtomicInt), ulong thread = 0, void* reserved = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2221) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4a1b8879e48ae963"></a>[`public struct Inno.Native.Sdl3.SDLInitState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2216) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLInitStatePtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLInitStatePtr 7a39162ef0785e1ced1ad64abf4468908c7c10b8308a497fd9f9cb3cbf92fdf6 -->
<!-- api: M:Inno.Native.Sdl3.SDLInitStatePtr.#ctor(Inno.Native.Sdl3.SDLInitState*) b590c68fac24a5cc7ce36feb74c6644209cf1e6a912bb1f6e3091efafae91f0d -->
<!-- api: F:Inno.Native.Sdl3.SDLInitStatePtr.Handle 4034711d83b35479facf5055438da5c29ce384955c77e24cbd8afd40bccd7e60 -->
<!-- api: P:Inno.Native.Sdl3.SDLInitStatePtr.IsNull 921dbc870b6a32bcc28c0f2022047fa5edc4770efadb046699ec843bd0ab839f -->
<!-- api: P:Inno.Native.Sdl3.SDLInitStatePtr.Null d1b751aa79c49a77590363b51b8a403267602f561939ac969349fd60499889c5 -->
<!-- api: P:Inno.Native.Sdl3.SDLInitStatePtr.Item(System.Int32) 78f4885fc513257434b7bec97c0c1113add8fa2f25bbdd0293c74fe41ab7f663 -->
<!-- api: M:Inno.Native.Sdl3.SDLInitStatePtr.op_Implicit(Inno.Native.Sdl3.SDLInitState*)~Inno.Native.Sdl3.SDLInitStatePtr 2f244ff5263d9a71dedc10d75e5d082ff2b4aec5d839b5507143e5a175dfeabc -->
<!-- api: M:Inno.Native.Sdl3.SDLInitStatePtr.op_Implicit(Inno.Native.Sdl3.SDLInitStatePtr)~Inno.Native.Sdl3.SDLInitState* dee2cd147c3225ae0f731b6fbb99c2b8c93dc379b10e1ee72d94eaa5c9efd26c -->
<!-- api: M:Inno.Native.Sdl3.SDLInitStatePtr.op_Equality(Inno.Native.Sdl3.SDLInitStatePtr,Inno.Native.Sdl3.SDLInitStatePtr) a1db8ab4868b2ea6dbadbbc2ef4861742824701be7114df46fdcb7bbd2634145 -->
<!-- api: M:Inno.Native.Sdl3.SDLInitStatePtr.op_Inequality(Inno.Native.Sdl3.SDLInitStatePtr,Inno.Native.Sdl3.SDLInitStatePtr) 9edfeefb0cbe863466d7c89a7e443c94b82a742d5761b9c3dd0fd2d79c6b0407 -->
<!-- api: M:Inno.Native.Sdl3.SDLInitStatePtr.Equals(Inno.Native.Sdl3.SDLInitStatePtr) 5b2eb8bafa19ce143e9892737cb00486690d4c7a1a4050a1e9ab56797ee4ca13 -->
<!-- api: M:Inno.Native.Sdl3.SDLInitStatePtr.Equals(System.Object) dba8169242f1d3e68801717899d4c70276c79d7e60799d967ba79f7ab48eb986 -->
<!-- api: M:Inno.Native.Sdl3.SDLInitStatePtr.GetHashCode cebcf7e45eb798be406396cb99f8df653d1973fbc083353d2568c4b3047841a9 -->
<!-- api: P:Inno.Native.Sdl3.SDLInitStatePtr.Status 5d3a8be0a49664c2233c802893d6dcad642ec2265ddeacebba6e6c2a165c9efc -->
<!-- api: P:Inno.Native.Sdl3.SDLInitStatePtr.Thread 764df742a85cda27e78a66a70a8a245e47bd09371ee7908786534a895f4aee27 -->
<!-- api: P:Inno.Native.Sdl3.SDLInitStatePtr.Reserved 04a5033ab68e4a58fd673d4a8548c60dac49a0a0f0947cf189aecbc7eab875ee -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7c8e1dae92194e67"></a>[`public Inno.Native.Sdl3.SDLInitState* Inno.Native.Sdl3.SDLInitStatePtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9320) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58271666a8c61f08"></a>[`public Inno.Native.Sdl3.SDLInitStatePtr.SDLInitStatePtr(Inno.Native.Sdl3.SDLInitState* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9315) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1cd15feeb054f9ea"></a>[`public bool Inno.Native.Sdl3.SDLInitStatePtr.Equals(Inno.Native.Sdl3.SDLInitStatePtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9330) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-a62e3fa271710d27"></a>[`public override bool Inno.Native.Sdl3.SDLInitStatePtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9331) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-5268ac560bf6adae"></a>[`public override int Inno.Native.Sdl3.SDLInitStatePtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9332) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-204297c559b771c8"></a>[`public static bool Inno.Native.Sdl3.SDLInitStatePtr.operator ==(Inno.Native.Sdl3.SDLInitStatePtr left, Inno.Native.Sdl3.SDLInitStatePtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9328) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e91e7f6c444de9b3"></a>[`public static Inno.Native.Sdl3.SDLInitStatePtr.implicit operator Inno.Native.Sdl3.SDLInitStatePtr(Inno.Native.Sdl3.SDLInitState* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9326) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-609878bc2d13a90b"></a>[`public static Inno.Native.Sdl3.SDLInitStatePtr.implicit operator Inno.Native.Sdl3.SDLInitState*(Inno.Native.Sdl3.SDLInitStatePtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9327) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-46553241b60800f5"></a>[`public static bool Inno.Native.Sdl3.SDLInitStatePtr.operator !=(Inno.Native.Sdl3.SDLInitStatePtr left, Inno.Native.Sdl3.SDLInitStatePtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9329) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-602a723d2263947b"></a>[`public bool Inno.Native.Sdl3.SDLInitStatePtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9321) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-abe1f8ac95b81ae2"></a>[`public Inno.Native.Sdl3.SDLInitState Inno.Native.Sdl3.SDLInitStatePtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9324) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d072333d0af69480"></a>[`public static Inno.Native.Sdl3.SDLInitStatePtr Inno.Native.Sdl3.SDLInitStatePtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9322) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-01a6c997f88f633a"></a>[`public void* Inno.Native.Sdl3.SDLInitStatePtr.Reserved { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9336) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3237084419fb0246"></a>[`public ref Inno.Native.Sdl3.SDLAtomicInt Inno.Native.Sdl3.SDLInitStatePtr.Status { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9334) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-45999e4701357efb"></a>[`public ref ulong Inno.Native.Sdl3.SDLInitStatePtr.Thread { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9335) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cfe8f369f73d684a"></a>[`public struct Inno.Native.Sdl3.SDLInitStatePtr : System.IEquatable<Inno.Native.Sdl3.SDLInitStatePtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9313) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLInitStatus</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLInitStatus 32fa2574cb95109c9862df4b62f8ba3711f607d01a8844a7fbec0f4c728657c0 -->
<!-- api: F:Inno.Native.Sdl3.SDLInitStatus.Uninitialized e7b287dc18e68017400b11817844a5235179616c2752e72cf97809769fd38d7a -->
<!-- api: F:Inno.Native.Sdl3.SDLInitStatus.Initializing e093d2ae01bda879cb2470dd20dda47b4ad4ad3bad53ffe2396ebaf6417bd00b -->
<!-- api: F:Inno.Native.Sdl3.SDLInitStatus.Initialized 99d32f9e07985935b242dbc2a4db6db99ff3363db7f86b4dc8de172fb4439897 -->
<!-- api: F:Inno.Native.Sdl3.SDLInitStatus.Uninitializing 871310e658043b8b94dd671e367cf7fe2fdce871cee3d21fdd43dc538269ea99 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f5ee511628676734"></a>[`Inno.Native.Sdl3.SDLInitStatus.Initialized = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L149) | 当前常量或枚举值：`2`。 | — |
| <a id="api-f1b4be9dd1c789c5"></a>[`Inno.Native.Sdl3.SDLInitStatus.Initializing = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L148) | 当前常量或枚举值：`1`。 | — |
| <a id="api-16c6348fadcec703"></a>[`Inno.Native.Sdl3.SDLInitStatus.Uninitialized = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L147) | 当前常量或枚举值：`0`。 | — |
| <a id="api-5825453d5591577c"></a>[`Inno.Native.Sdl3.SDLInitStatus.Uninitializing = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L150) | 当前常量或枚举值：`3`。 | — |
| <a id="api-bb1cb5cb38398f62"></a>[`public enum Inno.Native.Sdl3.SDLInitStatus : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L145) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLIOStatus</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLIOStatus 694e4ad23976547650630dee003ee7ec9f148be146ea51c2e7c153d484699af4 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStatus.Ready e1151200e5285251e6b4993a3695dcffc4250fe3caa1ecc6c097fc70e70758e9 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStatus.Error 71a1d6f9e0c78e29ef97bce2a744c734623511c1e58d7bbc0f6e1c1109849ff6 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStatus.Eof cc0656a4ae88d80d84e787ae9d89e76a3a5ad14563e2c4eaa03ec9d558c11c1c -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStatus.NotReady 09eae5c7c607b51b5196448b83275b6ddd192fddc739944350c85fb33834a240 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStatus.Readonly e252b32d8e05172017b840885a10c8ecc7f51c39cadcc8c28f349c43b991aaa9 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStatus.Writeonly efa88747eb5ecf77ee4f8b8effd659299c29828a1344d96eedea5cc742113b28 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ab63cebab05ce8f3"></a>[`Inno.Native.Sdl3.SDLIOStatus.Eof = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L160) | 当前常量或枚举值：`2`。 | — |
| <a id="api-ebd4e043804f795c"></a>[`Inno.Native.Sdl3.SDLIOStatus.Error = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L159) | 当前常量或枚举值：`1`。 | — |
| <a id="api-46835554259925e8"></a>[`Inno.Native.Sdl3.SDLIOStatus.NotReady = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L161) | 当前常量或枚举值：`3`。 | — |
| <a id="api-72250d8dffd5f746"></a>[`Inno.Native.Sdl3.SDLIOStatus.Readonly = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L162) | 当前常量或枚举值：`4`。 | — |
| <a id="api-d8b15db47c9b24fa"></a>[`Inno.Native.Sdl3.SDLIOStatus.Ready = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L158) | 当前常量或枚举值：`0`。 | — |
| <a id="api-d08d49e9bab05b77"></a>[`Inno.Native.Sdl3.SDLIOStatus.Writeonly = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L163) | 当前常量或枚举值：`5`。 | — |
| <a id="api-ac81956ed6aa3292"></a>[`public enum Inno.Native.Sdl3.SDLIOStatus : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L156) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLIOStream</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLIOStream 6705a319ae7dcd225a9a99bc9e7dc3d825be2c1b82c3588ffbe90feee8ec4a0d -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStream.#ctor(System.IntPtr) 19b6db4d43876d230906e94fa22a4b7b0519e3861125aa6152e8d8a44fb0af41 -->
<!-- api: P:Inno.Native.Sdl3.SDLIOStream.Handle ada5416d88518f489d60ab14ba2f12ae2e2406f011507430f991e929a7d9a1ab -->
<!-- api: P:Inno.Native.Sdl3.SDLIOStream.IsNull 0641093d563f4f3f8a61446354b219ed81e620cd11f1ba273a40281335c80be9 -->
<!-- api: P:Inno.Native.Sdl3.SDLIOStream.Null 1f06bcefb9d3aec52d2255ba322d9036b38987dbb0d9a0b51d5103d0990a3827 -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStream.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLIOStream 1b5bfb22846d039c275a855d0e87fd66f52d2d3d2bdccf1864b36cbd4df8334e -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStream.op_Equality(Inno.Native.Sdl3.SDLIOStream,Inno.Native.Sdl3.SDLIOStream) cbcfadbad5b38bf8d8282206b858abaa7c988c1a7becc7f20dd21602e8aa6206 -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStream.op_Inequality(Inno.Native.Sdl3.SDLIOStream,Inno.Native.Sdl3.SDLIOStream) 8466ca7c15cf346ee08bfa875a19cd182913a0c8bf4be3cef07f81cad16e5363 -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStream.Equals(Inno.Native.Sdl3.SDLIOStream) c70037c8a532a7ea126cb344bacd3d76df328b0f079654bd7e4022c34d30a68a -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStream.Equals(System.Object) c72ab819da43658310e35de4a6f07d86fbfc5dc1cd82fa41bf18682060e1f6d2 -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStream.GetHashCode ed728dbc4d7194065f9e268076e7e836b0a04218a1a9707f8e0e789817128ce0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3ab9acf41289b681"></a>[`public Inno.Native.Sdl3.SDLIOStream.SDLIOStream(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2260) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8bdc4d47e89e2949"></a>[`public bool Inno.Native.Sdl3.SDLIOStream.Equals(Inno.Native.Sdl3.SDLIOStream other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2272) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-4302aa873360512a"></a>[`public override bool Inno.Native.Sdl3.SDLIOStream.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2273) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-3b34c38fb298dcf4"></a>[`public override int Inno.Native.Sdl3.SDLIOStream.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2274) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-97e35626147dcfce"></a>[`public static bool Inno.Native.Sdl3.SDLIOStream.operator ==(Inno.Native.Sdl3.SDLIOStream left, Inno.Native.Sdl3.SDLIOStream right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2270) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-86b22e5043688cff"></a>[`public static Inno.Native.Sdl3.SDLIOStream.implicit operator Inno.Native.Sdl3.SDLIOStream(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2269) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a88420d59d12c909"></a>[`public static bool Inno.Native.Sdl3.SDLIOStream.operator !=(Inno.Native.Sdl3.SDLIOStream left, Inno.Native.Sdl3.SDLIOStream right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2271) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-50241a3bd6c20e36"></a>[`public nint Inno.Native.Sdl3.SDLIOStream.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2265) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-392cf50f118eccf9"></a>[`public bool Inno.Native.Sdl3.SDLIOStream.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2266) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e036c3070f2cc806"></a>[`public static Inno.Native.Sdl3.SDLIOStream Inno.Native.Sdl3.SDLIOStream.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2267) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-82fd48e371be425d"></a>[`public readonly struct Inno.Native.Sdl3.SDLIOStream : System.IEquatable<Inno.Native.Sdl3.SDLIOStream>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2258) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLIOStreamInterface</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLIOStreamInterface a7862c21455612cdf6bd95019833d177e3498b22983cb3b5a4a74875dff6fd3f -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStreamInterface.Version ad11a899381bbd477a6aeb7aaa3c438c5e6243c7629754704c1371eb90866af3 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStreamInterface.Size d577bdff439d4b9fbd53d9f06fd40d83f8e3937fad6662d42daff8ccedef0786 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStreamInterface.Seek 721f94d954b0adc5d46ad05c29d2f6b0ff75295f1910b22c0f701f3011f5e2e9 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStreamInterface.Read 4a12e451a39eb949b872f541c9175a1a6985bcd0f02bbbf9fe5f037f9b3cdd22 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStreamInterface.Write 79d5614aae0cbea096daeba561669445aee07848c1f6ef1ac4d955e9cfce0530 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStreamInterface.Flush f148842958e9663c53420f1d070aa618dc87f70bec9f0133b30d50b1c9b2beb8 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStreamInterface.Close 9f44a51c8aeb106e60294f7bb7f2b196bebd95e57dd24d9d930a1af4479ca5f2 -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStreamInterface.#ctor(System.UInt32,,,,,,) 2c40f562a3e4e81872a0dff5fb0c8b75fe755301f7f63a87cc61b3e044f20264 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-efdcd5bb6b0bcf07"></a>[`public delegate* unmanaged[Cdecl]<void*, byte> Inno.Native.Sdl3.SDLIOStreamInterface.Close`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2241) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5775846b40312b79"></a>[`public delegate* unmanaged[Cdecl]<void*, Inno.Native.Sdl3.SDLIOStatus*, byte> Inno.Native.Sdl3.SDLIOStreamInterface.Flush`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2240) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21c074416277d179"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, ulong, Inno.Native.Sdl3.SDLIOStatus*, ulong> Inno.Native.Sdl3.SDLIOStreamInterface.Read`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2238) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-31bd49bc0631669c"></a>[`public delegate* unmanaged[Cdecl]<void*, long, Inno.Native.Sdl3.SDLIOWhence, long> Inno.Native.Sdl3.SDLIOStreamInterface.Seek`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2237) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cfa6b6d4bdbab45f"></a>[`public delegate* unmanaged[Cdecl]<void*, long> Inno.Native.Sdl3.SDLIOStreamInterface.Size`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2236) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8019131067a4bde8"></a>[`public uint Inno.Native.Sdl3.SDLIOStreamInterface.Version`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2235) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-856cde988d823e44"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, ulong, Inno.Native.Sdl3.SDLIOStatus*, ulong> Inno.Native.Sdl3.SDLIOStreamInterface.Write`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2239) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ddd77191fdb62ce7"></a>[`public Inno.Native.Sdl3.SDLIOStreamInterface.SDLIOStreamInterface(uint version = 0, delegate* unmanaged[Cdecl]<void*, long> size = default(delegate* unmanaged[Cdecl]<void*, long>), delegate* unmanaged[Cdecl]<void*, long, Inno.Native.Sdl3.SDLIOWhence, long> seek = default(delegate* unmanaged[Cdecl]<void*, long, Inno.Native.Sdl3.SDLIOWhence, long>), delegate* unmanaged[Cdecl]<void*, void*, ulong, Inno.Native.Sdl3.SDLIOStatus*, ulong> read = default(delegate* unmanaged[Cdecl]<void*, void*, ulong, Inno.Native.Sdl3.SDLIOStatus*, ulong>), delegate* unmanaged[Cdecl]<void*, void*, ulong, Inno.Native.Sdl3.SDLIOStatus*, ulong> write = default(delegate* unmanaged[Cdecl]<void*, void*, ulong, Inno.Native.Sdl3.SDLIOStatus*, ulong>), delegate* unmanaged[Cdecl]<void*, Inno.Native.Sdl3.SDLIOStatus*, byte> flush = default(delegate* unmanaged[Cdecl]<void*, Inno.Native.Sdl3.SDLIOStatus*, byte>), delegate* unmanaged[Cdecl]<void*, byte> close = default(delegate* unmanaged[Cdecl]<void*, byte>))`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2242) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a0da7e0f8bdeed68"></a>[`public struct Inno.Native.Sdl3.SDLIOStreamInterface`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2233) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLIOStreamInterfacePtr</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLIOStreamInterfacePtr 9be307517ad3908b92013285c1aa351ac25168c27fb6f5d6e38824e941412960 -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.#ctor(Inno.Native.Sdl3.SDLIOStreamInterface*) fbcd1ffcec9104507c6973a17fde816365813dd9bf4e81b6f827f4feadd619aa -->
<!-- api: F:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Handle 5ce6cff20c2f107bb01d47a9a02bf8b85ce1b32f76c8cc6a2ebe410ffed5eb7f -->
<!-- api: P:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.IsNull 0f328722533ed6d492a96a6757892c230ae3cc31740cbfa50f7ffac1b4ed42b0 -->
<!-- api: P:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Null 1bc7474a239077f2b5b96f3a146e6a9aed90ed5aca0580fe12fa8c38befc3cca -->
<!-- api: P:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Item(System.Int32) 6344703ab4976c4203742663a8a6598b32e571fef4ce9825d6d96a4a15eddf52 -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.op_Implicit(Inno.Native.Sdl3.SDLIOStreamInterface*)~Inno.Native.Sdl3.SDLIOStreamInterfacePtr f7dd5599cbba3068a00f96b07ed68c1bf408e2147d7574562ae56e1c1b1e96c2 -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.op_Implicit(Inno.Native.Sdl3.SDLIOStreamInterfacePtr)~Inno.Native.Sdl3.SDLIOStreamInterface* 9d7e7e65960721274477b7cef30399d9fb84d700cf6ae816641256deb6d77b85 -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.op_Equality(Inno.Native.Sdl3.SDLIOStreamInterfacePtr,Inno.Native.Sdl3.SDLIOStreamInterfacePtr) bb46201de6b50800f4cbf70e413ff0ae8617fb71bde76a4c69e4b31f36ab422c -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.op_Inequality(Inno.Native.Sdl3.SDLIOStreamInterfacePtr,Inno.Native.Sdl3.SDLIOStreamInterfacePtr) 22ee7e330900dfbd2b1660fb35cb71d0260e7ae9b79e741fdf94527be4689c36 -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Equals(Inno.Native.Sdl3.SDLIOStreamInterfacePtr) 05f11be6250a63800693576326fe5a44040afb34f1f05a6d4c023061b9eb2158 -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Equals(System.Object) 884d25a9a612b5c54e477b231e541b99a04d842ac7a77657777686728df9f9a4 -->
<!-- api: M:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.GetHashCode 70e2b9b6c10e6f4cbd3d6c5f359ed4d9c3f59746b9e254f5b690e1f4ebbd3f2e -->
<!-- api: P:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Version 431aec1a38f542944f2b1aa7a96505ebd7e7f2b5a38c2b0710ea689000f77c17 -->
<!-- api: P:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Size 4758792841de1ca0538ce0b5b4786d4e3a15ca79307845c8ea6884fa19ad4dfb -->
<!-- api: P:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Seek 7398440e5a7608e6a28431134c7993f30fcb70c804b6c18cd8e5b56fb5c8c034 -->
<!-- api: P:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Read 3b771c55fdaa32950f213ab0130d6c123fb0b2c45ce44d1b3b9fc50df74e750c -->
<!-- api: P:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Write 31bbb870cbccced2bbdf238c802b4b5b7e8a1cd716ad9820b9832c7c43bb7e2e -->
<!-- api: P:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Flush 9e8858c607392e7fd050711ce3905e0ec34d828e30132f5cf102b7c36fac8812 -->
<!-- api: P:Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Close ca5230740f4377b42cd00f5f789774af585de16eb7dbb200ee75929e49fdd7a8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7c29c96e293e8d8d"></a>[`public Inno.Native.Sdl3.SDLIOStreamInterface* Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9289) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-23097c58257f0d92"></a>[`public Inno.Native.Sdl3.SDLIOStreamInterfacePtr.SDLIOStreamInterfacePtr(Inno.Native.Sdl3.SDLIOStreamInterface* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9284) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b044d9f763b111e1"></a>[`public bool Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Equals(Inno.Native.Sdl3.SDLIOStreamInterfacePtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9299) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-58968d7a2f3e12f6"></a>[`public override bool Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9300) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-f933f246a580a0a5"></a>[`public override int Inno.Native.Sdl3.SDLIOStreamInterfacePtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9301) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-81f9207bea54d9ae"></a>[`public static bool Inno.Native.Sdl3.SDLIOStreamInterfacePtr.operator ==(Inno.Native.Sdl3.SDLIOStreamInterfacePtr left, Inno.Native.Sdl3.SDLIOStreamInterfacePtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9297) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-305e848ac61bca8c"></a>[`public static Inno.Native.Sdl3.SDLIOStreamInterfacePtr.implicit operator Inno.Native.Sdl3.SDLIOStreamInterfacePtr(Inno.Native.Sdl3.SDLIOStreamInterface* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9295) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a756c1b0571846a7"></a>[`public static Inno.Native.Sdl3.SDLIOStreamInterfacePtr.implicit operator Inno.Native.Sdl3.SDLIOStreamInterface*(Inno.Native.Sdl3.SDLIOStreamInterfacePtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9296) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-752cb018ba2daadd"></a>[`public static bool Inno.Native.Sdl3.SDLIOStreamInterfacePtr.operator !=(Inno.Native.Sdl3.SDLIOStreamInterfacePtr left, Inno.Native.Sdl3.SDLIOStreamInterfacePtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9298) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-02f31e24a3e3509b"></a>[`public delegate* unmanaged[Cdecl]<void*, byte> Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Close { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9309) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7f33f2b77ff65b83"></a>[`public delegate* unmanaged[Cdecl]<void*, Inno.Native.Sdl3.SDLIOStatus*, byte> Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Flush { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9308) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0492f551f5ba8320"></a>[`public bool Inno.Native.Sdl3.SDLIOStreamInterfacePtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9290) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-08ac7796b4799066"></a>[`public Inno.Native.Sdl3.SDLIOStreamInterface Inno.Native.Sdl3.SDLIOStreamInterfacePtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9293) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-923b217494b8b0d1"></a>[`public static Inno.Native.Sdl3.SDLIOStreamInterfacePtr Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9291) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5a76637f8653852a"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, ulong, Inno.Native.Sdl3.SDLIOStatus*, ulong> Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Read { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9306) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fcdff11156cf5e08"></a>[`public delegate* unmanaged[Cdecl]<void*, long, Inno.Native.Sdl3.SDLIOWhence, long> Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Seek { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9305) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-941f818d79d031ee"></a>[`public delegate* unmanaged[Cdecl]<void*, long> Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Size { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9304) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-76c70aa7276f715f"></a>[`public ref uint Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Version { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9303) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-917fabd92a8720c2"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, ulong, Inno.Native.Sdl3.SDLIOStatus*, ulong> Inno.Native.Sdl3.SDLIOStreamInterfacePtr.Write { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9307) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-740d41cd91d0e706"></a>[`public struct Inno.Native.Sdl3.SDLIOStreamInterfacePtr : System.IEquatable<Inno.Native.Sdl3.SDLIOStreamInterfacePtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9282) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLIOWhence</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLIOWhence ab58e1f8f567c42c056acb7205ee3e255c9461b19740092f7c6621dee75755b9 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOWhence.SeekSet 60e6d7cf39ea9bee838aea4e7ef0255384a3965db3c0b518d9b69636d290b477 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOWhence.SeekCur 0769ce995d8a56c7f62b7e0541d0b0f0a7ef506a5f2020268194c7246f799498 -->
<!-- api: F:Inno.Native.Sdl3.SDLIOWhence.SeekEnd 8fa5fa8f6c4ad5da9ecacd67973a6528ab2adb21af1879b7f1efea90c9ece86b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dd8a9f0375224a67"></a>[`Inno.Native.Sdl3.SDLIOWhence.SeekCur = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L172) | 当前常量或枚举值：`1`。 | — |
| <a id="api-e91aaa220601113e"></a>[`Inno.Native.Sdl3.SDLIOWhence.SeekEnd = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L173) | 当前常量或枚举值：`2`。 | — |
| <a id="api-87a487e341389b6f"></a>[`Inno.Native.Sdl3.SDLIOWhence.SeekSet = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L171) | 当前常量或枚举值：`0`。 | — |
| <a id="api-db3fb0a1e83114eb"></a>[`public enum Inno.Native.Sdl3.SDLIOWhence : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L169) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLJoyAxisEvent</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLJoyAxisEvent ed40e429478330c339a0334c75139a77a4e444f2278b5d64e01a41e565e0828c -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyAxisEvent.Type 674c92018aabe050f2e512092c7c9a60c967a8db3104d9652468af4446115739 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyAxisEvent.Reserved b5014dc4a48c05956de444d42ad8dd862827b8e0df10889852a4f24bceaf95f8 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyAxisEvent.Timestamp bc3a06ef4ee56ff3884030abd062d3be2d8fa0bb2a045292e7e88abaff4ad2b8 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyAxisEvent.Which 21dac10031ab5916121c2afe69366aae99eda910cbc5279cdf1c30b215a9cc7c -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyAxisEvent.Axis 48cda77430ca12b63fdccc648ec58e152f0b2d059338502ad7d6608bac8abdca -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyAxisEvent.Padding1 921a2a88e4622b1a6971653f03fae948a950a4411b531e2e2d51c50624d95b2b -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyAxisEvent.Padding2 a48fd9c70a359f748fe76114bd20bddb80a35d90dd6f7c62f23aaddbc1c1d82c -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyAxisEvent.Padding3 570a30797d6ec4f7e201ca09753e4d1da2b53e9845addccf40ab136afed0c129 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyAxisEvent.Value 783e2155e5c40f4f68b4f80eb15e9229f61137cce429b19cfc1fb415471eb363 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyAxisEvent.Padding4 3bcec7097b7f0ce4e88c5c8cd375fc63b1dc4b9da7e739d0a433dbd25d4a640b -->
<!-- api: M:Inno.Native.Sdl3.SDLJoyAxisEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt32,System.Byte,System.Byte,System.Byte,System.Byte,System.Int16,System.UInt16) 09ae78a7156ee82547b658faab0c07804d4284afe088f29b90473cc859fafff6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7acc637172883f1d"></a>[`public byte Inno.Native.Sdl3.SDLJoyAxisEvent.Axis`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3389) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8d172e3291de38d0"></a>[`public byte Inno.Native.Sdl3.SDLJoyAxisEvent.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3390) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccaf01c0c3668c95"></a>[`public byte Inno.Native.Sdl3.SDLJoyAxisEvent.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3391) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7c4af721018851fc"></a>[`public byte Inno.Native.Sdl3.SDLJoyAxisEvent.Padding3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3392) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f331bae48e7fa0f5"></a>[`public ushort Inno.Native.Sdl3.SDLJoyAxisEvent.Padding4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3394) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-efeb68e9ff43a705"></a>[`public uint Inno.Native.Sdl3.SDLJoyAxisEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3386) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7fb97518f7ce85b"></a>[`public ulong Inno.Native.Sdl3.SDLJoyAxisEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3387) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c8280c04c32c039"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLJoyAxisEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3385) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c9de42bdeda68aa3"></a>[`public short Inno.Native.Sdl3.SDLJoyAxisEvent.Value`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3393) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c6ec59533e40028f"></a>[`public uint Inno.Native.Sdl3.SDLJoyAxisEvent.Which`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3388) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e0a564971ecd7e2b"></a>[`public Inno.Native.Sdl3.SDLJoyAxisEvent.SDLJoyAxisEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, uint which = 0, byte axis = 0, byte padding1 = 0, byte padding2 = 0, byte padding3 = 0, short value = 0, ushort padding4 = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3395) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b1ff114d395871c6"></a>[`public struct Inno.Native.Sdl3.SDLJoyAxisEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3383) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLJoyBallEvent</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLJoyBallEvent 7ae95ce94682349bb57fcca007981df549e3e444b95cff9fac81cf270c64191a -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBallEvent.Type 757c564f3488fa4ccb425b986d57b6e1628962b7922037627bf92ab11a9ff8ff -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBallEvent.Reserved ccd203151df93f637474617d31a90ab29e46a2d839a3e9fcb4071c0f8f1f307d -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBallEvent.Timestamp e08223d759ff3b2fe07e38379c4ca32628c9a8de9cf6373e88358f129c3e1778 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBallEvent.Which 1e7c5a4f56cabbbaa7c472585e60b2fbfc73867dc3c0b98842ac2900599da241 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBallEvent.Ball 30cbe3610731364797388cdec62672b1448da8c4581059d4919bb8852584b141 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBallEvent.Padding1 b48374cae33f9e08d3131a262df38be8318f304c146b207e1bf66bef5b4a24d7 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBallEvent.Padding2 3ba775129514dd6d65212cf6e1b9d8adf4ec2d64655eeb8cd61152205e816372 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBallEvent.Padding3 aae41a0d853b1394bfa5940eac3787cb6dbec9c593d3beb26403019d2a7c3b88 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBallEvent.Xrel 375e4040d33519b64a284fd966cc35277446ccd91c830ed23fee24bcc5abf3e8 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBallEvent.Yrel b58b2deb8a97d2b9249472c59df72fde07bdd2e82b656838d8bce59de5b69e23 -->
<!-- api: M:Inno.Native.Sdl3.SDLJoyBallEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt32,System.Byte,System.Byte,System.Byte,System.Byte,System.Int16,System.Int16) a360abb743cb43b9fc9c3952ac6eb67db8e573840cd3c50a38a2db18e05bce4c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0d106afd635e4206"></a>[`public byte Inno.Native.Sdl3.SDLJoyBallEvent.Ball`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3420) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6eb163654e37b4c"></a>[`public byte Inno.Native.Sdl3.SDLJoyBallEvent.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3421) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-90a53d4cfae83f5d"></a>[`public byte Inno.Native.Sdl3.SDLJoyBallEvent.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3422) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-535cb12b68125a03"></a>[`public byte Inno.Native.Sdl3.SDLJoyBallEvent.Padding3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3423) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1bf257696e169e17"></a>[`public uint Inno.Native.Sdl3.SDLJoyBallEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3417) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c3fc30e4d64bf23"></a>[`public ulong Inno.Native.Sdl3.SDLJoyBallEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3418) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-694ce53185a97f54"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLJoyBallEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3416) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb826ee903390eac"></a>[`public uint Inno.Native.Sdl3.SDLJoyBallEvent.Which`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3419) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-68210aad0aafe60a"></a>[`public short Inno.Native.Sdl3.SDLJoyBallEvent.Xrel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3424) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-78a2003de61ab5d2"></a>[`public short Inno.Native.Sdl3.SDLJoyBallEvent.Yrel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3425) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2695a4657ecab754"></a>[`public Inno.Native.Sdl3.SDLJoyBallEvent.SDLJoyBallEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, uint which = 0, byte ball = 0, byte padding1 = 0, byte padding2 = 0, byte padding3 = 0, short xrel = 0, short yrel = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3426) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6121f70fff828106"></a>[`public struct Inno.Native.Sdl3.SDLJoyBallEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3414) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLJoyBatteryEvent</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLJoyBatteryEvent 1364fb714b7517f96906d13132ea3967b0353f705bf0c34cf8f6ce866449d3cd -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBatteryEvent.Type 52d581b16201613afe93ad1939ae6fc53924868c8db8af19bb201719186eaefc -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBatteryEvent.Reserved 6d2adb7764ded98a0747c5d9e03543126bb93419aff96371c00c2bf35934b963 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBatteryEvent.Timestamp 4bace3930bab090d08860ca430fe567d31fe9d7b063fb86e488f3d3e4336fab8 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBatteryEvent.Which 4bfcb36d5fae892928b27a26c5e7273492a56a63a238de2670ef0b9d49afe6f0 -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBatteryEvent.State d1a8e35335835c3b039bfc763377e2da27adc30b6da91828a1fe06c3ae1ac88a -->
<!-- api: F:Inno.Native.Sdl3.SDLJoyBatteryEvent.Percent e5a6e617630165da10e0da68e0988aeebfe36abbadb7e60ed972ae5b298b5d05 -->
<!-- api: M:Inno.Native.Sdl3.SDLJoyBatteryEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt32,Inno.Native.Sdl3.SDLPowerState,System.Int32) cfbcced15296dcf68bd5e05d5ff9630592ce0202cf29a143f748e9740bffdddc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-64bfb995598234d4"></a>[`public int Inno.Native.Sdl3.SDLJoyBatteryEvent.Percent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3525) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-478f2b4380da4cb8"></a>[`public uint Inno.Native.Sdl3.SDLJoyBatteryEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3521) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f0e13c5894441d5"></a>[`public Inno.Native.Sdl3.SDLPowerState Inno.Native.Sdl3.SDLJoyBatteryEvent.State`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3524) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61501f646100ba3f"></a>[`public ulong Inno.Native.Sdl3.SDLJoyBatteryEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3522) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe2016e0a22da214"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLJoyBatteryEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3520) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a303da965864341e"></a>[`public uint Inno.Native.Sdl3.SDLJoyBatteryEvent.Which`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3523) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f777aa709ae7363"></a>[`public Inno.Native.Sdl3.SDLJoyBatteryEvent.SDLJoyBatteryEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, uint which = 0, Inno.Native.Sdl3.SDLPowerState state = Inno.Native.Sdl3.SDLPowerState.PowerstateUnknown, int percent = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3526) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0dd179305d4c7d7d"></a>[`public struct Inno.Native.Sdl3.SDLJoyBatteryEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3518) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
