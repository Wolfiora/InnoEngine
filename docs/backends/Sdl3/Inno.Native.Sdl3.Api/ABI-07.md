# Inno.Native.Sdl3 ABI · SDLAtomicIntPtr → SDLEnvironment

[项目与生命周期](../Inno.Native.Sdl3.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.Sdl3.SDLAtomicIntPtr</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAtomicIntPtr 53d1f388da4e670bcd22bd5c764302e7c5d447fc8fabb429259922d6ffd862ca -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicIntPtr.#ctor(Inno.Native.Sdl3.SDLAtomicInt*) 217b41dc972b85a2872bdd147876f8312df680a7d6f010fdd8d5d62e6147e05f -->
<!-- api: F:Inno.Native.Sdl3.SDLAtomicIntPtr.Handle d8a1bb7448a13bf55b1fdb4c4aafc9197921a273a2e9ca646aa3eb110cb3bcf2 -->
<!-- api: P:Inno.Native.Sdl3.SDLAtomicIntPtr.IsNull b3aad6fdc67e1e720d9a9c34d4897b50d939d5003d6be32677896bd49596be5a -->
<!-- api: P:Inno.Native.Sdl3.SDLAtomicIntPtr.Null 3b0e4c7eaae8b892113e3430ddd74d761c2e7036f35fc76be35cea9cc3d518f9 -->
<!-- api: P:Inno.Native.Sdl3.SDLAtomicIntPtr.Item(System.Int32) adeacab1cc46167895910dc57df54872d1a1ad55911980f726973f503638ceac -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicIntPtr.op_Implicit(Inno.Native.Sdl3.SDLAtomicInt*)~Inno.Native.Sdl3.SDLAtomicIntPtr 62e7f0f9b5964db016a72101d201e6ad0c3b8cf3a252f93a73c92b1ea4f6ae9a -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicIntPtr.op_Implicit(Inno.Native.Sdl3.SDLAtomicIntPtr)~Inno.Native.Sdl3.SDLAtomicInt* f96c82bb56e82f2872d0d135073fc8accf476da0d091571dca6a67071e495383 -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicIntPtr.op_Equality(Inno.Native.Sdl3.SDLAtomicIntPtr,Inno.Native.Sdl3.SDLAtomicIntPtr) 1a79766c01b55c001eb671df220d5d915165d3a95d41abc9cc43ffdb229ae909 -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicIntPtr.op_Inequality(Inno.Native.Sdl3.SDLAtomicIntPtr,Inno.Native.Sdl3.SDLAtomicIntPtr) f3bb2d90e76ca8bee0ff4c8fa7b3b1707234835d1fc0009925a9ef63139a9a7b -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicIntPtr.Equals(Inno.Native.Sdl3.SDLAtomicIntPtr) 53d5f91da024bfbdf8130b26ef8bb646e8f1ee8a97f6bb2921e3433e45d47b62 -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicIntPtr.Equals(System.Object) b789b9cd29b1eb5207debc3f2dbe61278ce70a6eaba41ef1fa92038848a40aa0 -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicIntPtr.GetHashCode 82d178a18493ff0fdfd0b39ebcb3527ce7de1a6f488f86d2493282bd3d325054 -->
<!-- api: P:Inno.Native.Sdl3.SDLAtomicIntPtr.Value cddee5694b5f952728f3c7a4379ee4fa7e026afce7b1d95921bb2fdd4f4271ab -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-30dba0d57a23d02a"></a>[`public Inno.Native.Sdl3.SDLAtomicInt* Inno.Native.Sdl3.SDLAtomicIntPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7961) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3dee8bd6eed9df89"></a>[`public Inno.Native.Sdl3.SDLAtomicIntPtr.SDLAtomicIntPtr(Inno.Native.Sdl3.SDLAtomicInt* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7956) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4d015b40a7886646"></a>[`public bool Inno.Native.Sdl3.SDLAtomicIntPtr.Equals(Inno.Native.Sdl3.SDLAtomicIntPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7971) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-0fb6467b32e5d597"></a>[`public override bool Inno.Native.Sdl3.SDLAtomicIntPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7972) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-f1de350cea8b1fd2"></a>[`public override int Inno.Native.Sdl3.SDLAtomicIntPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7973) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-4a70e31d2c59de31"></a>[`public static bool Inno.Native.Sdl3.SDLAtomicIntPtr.operator ==(Inno.Native.Sdl3.SDLAtomicIntPtr left, Inno.Native.Sdl3.SDLAtomicIntPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7969) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3a35d3d358351a31"></a>[`public static Inno.Native.Sdl3.SDLAtomicIntPtr.implicit operator Inno.Native.Sdl3.SDLAtomicIntPtr(Inno.Native.Sdl3.SDLAtomicInt* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7967) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c52fdfeb647bbe75"></a>[`public static Inno.Native.Sdl3.SDLAtomicIntPtr.implicit operator Inno.Native.Sdl3.SDLAtomicInt*(Inno.Native.Sdl3.SDLAtomicIntPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7968) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-248155ddcde0f683"></a>[`public static bool Inno.Native.Sdl3.SDLAtomicIntPtr.operator !=(Inno.Native.Sdl3.SDLAtomicIntPtr left, Inno.Native.Sdl3.SDLAtomicIntPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7970) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6d69d6dccf55a0bc"></a>[`public bool Inno.Native.Sdl3.SDLAtomicIntPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7962) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1932874811937869"></a>[`public Inno.Native.Sdl3.SDLAtomicInt Inno.Native.Sdl3.SDLAtomicIntPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7965) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ec3caf57bae828b5"></a>[`public static Inno.Native.Sdl3.SDLAtomicIntPtr Inno.Native.Sdl3.SDLAtomicIntPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7963) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aac55e2787e2f65d"></a>[`public ref int Inno.Native.Sdl3.SDLAtomicIntPtr.Value { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7975) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fc69bf96526b62e7"></a>[`public struct Inno.Native.Sdl3.SDLAtomicIntPtr : System.IEquatable<Inno.Native.Sdl3.SDLAtomicIntPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7954) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAtomicU32</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAtomicU32 7010470919a587ab47f258ede2fc151a00fbeff1e91e72c4773b00c71023a56b -->
<!-- api: F:Inno.Native.Sdl3.SDLAtomicU32.Value 40ce7c8fa1b2c35dee2779ec4a356c8891de0b74ea849debee319d7d82287e00 -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicU32.#ctor(System.UInt32) 31bf3b63a1764e2163e15c73866845c979bbd5b2f45a2cfd9e04cba75523e02a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e3bc032624b21c93"></a>[`public uint Inno.Native.Sdl3.SDLAtomicU32.Value`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2085) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b5b25581a5533829"></a>[`public Inno.Native.Sdl3.SDLAtomicU32.SDLAtomicU32(uint value = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2086) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0a9362eb1b44eeb8"></a>[`public struct Inno.Native.Sdl3.SDLAtomicU32`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2083) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAtomicU32Ptr</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAtomicU32Ptr b2fd8e0c97ce72c7b8f066a00a1eb94bc3449d4b1dd650d3c7db3c73fca01aea -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicU32Ptr.#ctor(Inno.Native.Sdl3.SDLAtomicU32*) 657ab239b848cda712172343d62d6709924248a8c1ee20bfa5c2ec824b36119e -->
<!-- api: F:Inno.Native.Sdl3.SDLAtomicU32Ptr.Handle 5adb83b3378f3ad4c5bbec6f63a8958b0068a393d9220e70b61596322edb1abd -->
<!-- api: P:Inno.Native.Sdl3.SDLAtomicU32Ptr.IsNull 6dd9ea61d26ce5ce3934e1ed3094304832f9ec683d3f67da4afed790506185b0 -->
<!-- api: P:Inno.Native.Sdl3.SDLAtomicU32Ptr.Null 51ba9448bdc7d59d238e85868e0fbd535c6a78c2794946e4cdff402f2b3c308d -->
<!-- api: P:Inno.Native.Sdl3.SDLAtomicU32Ptr.Item(System.Int32) 12a91faa8975abda4d634e7563feea4363d757f70823f7c30b6c3519e17e5ac9 -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicU32Ptr.op_Implicit(Inno.Native.Sdl3.SDLAtomicU32*)~Inno.Native.Sdl3.SDLAtomicU32Ptr 213f07cb1ccf3acba3cca7c0a766c77060ceb6d97c4e701afd524f60f9dbfcb0 -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicU32Ptr.op_Implicit(Inno.Native.Sdl3.SDLAtomicU32Ptr)~Inno.Native.Sdl3.SDLAtomicU32* 5b44ebd165fb4e6d93ce640f71a49ef51c40947aca5f41148c93c043b1fd2d29 -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicU32Ptr.op_Equality(Inno.Native.Sdl3.SDLAtomicU32Ptr,Inno.Native.Sdl3.SDLAtomicU32Ptr) 49270cbf9501778490ef871fdb253b7192272ec2b0a2b4acc8bea8b23022d124 -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicU32Ptr.op_Inequality(Inno.Native.Sdl3.SDLAtomicU32Ptr,Inno.Native.Sdl3.SDLAtomicU32Ptr) 07a5ba6eb3a81b504702b5a9f3caa42e7b006f4f7e950c688690c8ba480b0d55 -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicU32Ptr.Equals(Inno.Native.Sdl3.SDLAtomicU32Ptr) 3c754ab46d497983c983bf73212a89333b52b5eed1e3dd38d6c289f1acb86e4c -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicU32Ptr.Equals(System.Object) 8f9e1ad3123be727048098b1289b6a2e0e9c6bd6f3a74bf8dad9e19a54e691ea -->
<!-- api: M:Inno.Native.Sdl3.SDLAtomicU32Ptr.GetHashCode 6a5000fa793eb5f21aeb3f58838430885e881f9572ab79323ff65f7da18948db -->
<!-- api: P:Inno.Native.Sdl3.SDLAtomicU32Ptr.Value d353f2e4a56e223add01afd2ede449f865c8121bc78ce5a8dfbab77f71597716 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5d7303a60c8bba4e"></a>[`public Inno.Native.Sdl3.SDLAtomicU32* Inno.Native.Sdl3.SDLAtomicU32Ptr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7986) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fc35d50bc535295d"></a>[`public Inno.Native.Sdl3.SDLAtomicU32Ptr.SDLAtomicU32Ptr(Inno.Native.Sdl3.SDLAtomicU32* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7981) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b92a0fb6323fd856"></a>[`public bool Inno.Native.Sdl3.SDLAtomicU32Ptr.Equals(Inno.Native.Sdl3.SDLAtomicU32Ptr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7996) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-37d4a368f54f2c9d"></a>[`public override bool Inno.Native.Sdl3.SDLAtomicU32Ptr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7997) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-b6d4a7e56375a69b"></a>[`public override int Inno.Native.Sdl3.SDLAtomicU32Ptr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7998) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-50603b99fa43d04e"></a>[`public static bool Inno.Native.Sdl3.SDLAtomicU32Ptr.operator ==(Inno.Native.Sdl3.SDLAtomicU32Ptr left, Inno.Native.Sdl3.SDLAtomicU32Ptr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7994) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b588a5fa1c55f3b6"></a>[`public static Inno.Native.Sdl3.SDLAtomicU32Ptr.implicit operator Inno.Native.Sdl3.SDLAtomicU32Ptr(Inno.Native.Sdl3.SDLAtomicU32* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7992) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0414dda829a58d2a"></a>[`public static Inno.Native.Sdl3.SDLAtomicU32Ptr.implicit operator Inno.Native.Sdl3.SDLAtomicU32*(Inno.Native.Sdl3.SDLAtomicU32Ptr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7993) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eecfab6e9cbf4895"></a>[`public static bool Inno.Native.Sdl3.SDLAtomicU32Ptr.operator !=(Inno.Native.Sdl3.SDLAtomicU32Ptr left, Inno.Native.Sdl3.SDLAtomicU32Ptr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7995) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c6d19a199b35c03a"></a>[`public bool Inno.Native.Sdl3.SDLAtomicU32Ptr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7987) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-624e37bef8030c9f"></a>[`public Inno.Native.Sdl3.SDLAtomicU32 Inno.Native.Sdl3.SDLAtomicU32Ptr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7990) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e5f8ecd8aa5e9c35"></a>[`public static Inno.Native.Sdl3.SDLAtomicU32Ptr Inno.Native.Sdl3.SDLAtomicU32Ptr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7988) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-812c4df4a5bcf4f0"></a>[`public ref uint Inno.Native.Sdl3.SDLAtomicU32Ptr.Value { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8000) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d3ba7fa8083fe91c"></a>[`public struct Inno.Native.Sdl3.SDLAtomicU32Ptr : System.IEquatable<Inno.Native.Sdl3.SDLAtomicU32Ptr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L7979) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAudioDeviceEvent</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAudioDeviceEvent 74968c1fe5b28830e9f9552b95c2975c8d7d2ff0a30aa441cece690f8f909b62 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioDeviceEvent.Type 16522a20d7d0106cfc5993bc06106c8cef8da7e109fe65e2580d02541532d5d8 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioDeviceEvent.Reserved 87be230b2e8e07dc1027ed9b58600cc0d7d40231de5d58ee045d416985374255 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioDeviceEvent.Timestamp ce49cc3de5e174b01251383d10f5eb96e999180141ec558e6c213fc7cd4e5483 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioDeviceEvent.Which b7896df2e8f01a8ac2aa500369fa05ad9f7cd84207f0686991fd3e95513d4e04 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioDeviceEvent.Recording ee68992c5fac8df19accc8e116999253f0ab0d7b978637be9b56e83fa3a9ddd3 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioDeviceEvent.Padding1 77953e838d0cbc97a1663b7d0e80fbf47b3190d45a246480f484e007eb2e2b16 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioDeviceEvent.Padding2 ce250ea69f8974cc1e000021fbbc89eed4b0e1d981622ec3587873bda849a966 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioDeviceEvent.Padding3 74b4a7b53b329abec0662abea4397fd647d42fa6ac2b60a67af5d6bb63b0c6c2 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioDeviceEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt32,System.Byte,System.Byte,System.Byte,System.Byte) 4559231e0678fc6ba9d9026d2d80d8ed23f922d20d3f280698ab1894c92d550c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f4b1ccb3f92fb320"></a>[`public byte Inno.Native.Sdl3.SDLAudioDeviceEvent.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3689) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d456410c0658128a"></a>[`public byte Inno.Native.Sdl3.SDLAudioDeviceEvent.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3690) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-965caa6f49a2b8fa"></a>[`public byte Inno.Native.Sdl3.SDLAudioDeviceEvent.Padding3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3691) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a4af78d0463d4663"></a>[`public byte Inno.Native.Sdl3.SDLAudioDeviceEvent.Recording`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3688) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64b42d3ca41c92e1"></a>[`public uint Inno.Native.Sdl3.SDLAudioDeviceEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3685) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01d0927bb449a9f6"></a>[`public ulong Inno.Native.Sdl3.SDLAudioDeviceEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3686) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1ec4d4c0865d8961"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLAudioDeviceEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3684) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-afe840e13ab69fb1"></a>[`public uint Inno.Native.Sdl3.SDLAudioDeviceEvent.Which`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3687) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-23a657a1c3ec9a7f"></a>[`public Inno.Native.Sdl3.SDLAudioDeviceEvent.SDLAudioDeviceEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, uint which = 0, byte recording = 0, byte padding1 = 0, byte padding2 = 0, byte padding3 = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3692) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-939e9b52e12fd65b"></a>[`public struct Inno.Native.Sdl3.SDLAudioDeviceEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3682) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAudioFormat</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAudioFormat abd7112ca6aa7cf04147d2d847b3cbc1e745a15aa1c9cd6af6153966534869af -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioFormat.Unknown a68151903f615e41fd78f3ce42631d032d0ced94e72506f4c669899aba602e9c -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioFormat.U8 41609f92f0b4aae2f114413f832aadb77ade7b445b73de8fc7f144fed0d2b95c -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioFormat.S8 59d292fc724352108d094e260d7b7e3999876a1598aa5a1ecf9976f55605b502 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioFormat.S16Le 9b4fe6d0da1b214f26c460bd83995eab74067467ddcef2a7160801f57af3b712 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioFormat.S16Be 9eecee1d31feea39fff98c72f789d085b7ce0ebd05e759ed8b7225033cf884a5 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioFormat.S32Le 088e1e345b976eb9fa48c5e08b3d0d0b9a1e21a200c6c7036aa7b262fa2d2c1c -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioFormat.S32Be d1d5956fb507720b1e8df7f1d7b79071b6c7b6f78322fde77f90ca616dac2f22 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioFormat.F32Le f7f7c4913426c8bb1dfb6cc2e0dd60008bc1d6487ca0740decc8d5feea9f4008 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioFormat.F32Be 231d13bb0395f3b5534d24be4263f213fae673056012e41cadd5fbc591295430 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioFormat.S16 7e0175b0a4a72c7123bdc144a6f971eece31213b5a80d8031aeb1803678d6ed4 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioFormat.S32 83510ff8c01e5402f0e2d7a721b690babcf49874c328cb8cddca374824881a50 -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioFormat.F32 ef71eb371867d3a24488da425b36aa688cf692241d529253b96f670e863d878b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ca35c7989028c1e7"></a>[`Inno.Native.Sdl3.SDLAudioFormat.F32 = 33056`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L192) | 当前常量或枚举值：`33056`。 | — |
| <a id="api-5aef51b054e0949f"></a>[`Inno.Native.Sdl3.SDLAudioFormat.F32Be = 37152`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L189) | 当前常量或枚举值：`37152`。 | — |
| <a id="api-dbd35ceeb19bc0de"></a>[`Inno.Native.Sdl3.SDLAudioFormat.F32Le = 33056`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L188) | 当前常量或枚举值：`33056`。 | — |
| <a id="api-caf8cdb4eedff128"></a>[`Inno.Native.Sdl3.SDLAudioFormat.S16 = 32784`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L190) | 当前常量或枚举值：`32784`。 | — |
| <a id="api-8e56901bfd25a512"></a>[`Inno.Native.Sdl3.SDLAudioFormat.S16Be = 36880`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L185) | 当前常量或枚举值：`36880`。 | — |
| <a id="api-190c24be41e571ab"></a>[`Inno.Native.Sdl3.SDLAudioFormat.S16Le = 32784`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L184) | 当前常量或枚举值：`32784`。 | — |
| <a id="api-b21f7053b7cf3cea"></a>[`Inno.Native.Sdl3.SDLAudioFormat.S32 = 32800`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L191) | 当前常量或枚举值：`32800`。 | — |
| <a id="api-660c3cdd7c4cfa6d"></a>[`Inno.Native.Sdl3.SDLAudioFormat.S32Be = 36896`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L187) | 当前常量或枚举值：`36896`。 | — |
| <a id="api-d45706cff531e923"></a>[`Inno.Native.Sdl3.SDLAudioFormat.S32Le = 32800`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L186) | 当前常量或枚举值：`32800`。 | — |
| <a id="api-87febff1e9382fda"></a>[`Inno.Native.Sdl3.SDLAudioFormat.S8 = 32776`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L183) | 当前常量或枚举值：`32776`。 | — |
| <a id="api-5ba605ffd44133d8"></a>[`Inno.Native.Sdl3.SDLAudioFormat.U8 = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L182) | 当前常量或枚举值：`8`。 | — |
| <a id="api-5528eb87121a6ab7"></a>[`Inno.Native.Sdl3.SDLAudioFormat.Unknown = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L181) | 当前常量或枚举值：`0`。 | — |
| <a id="api-17ca72e96dc6fde9"></a>[`public enum Inno.Native.Sdl3.SDLAudioFormat : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L179) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAudioPostmixCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAudioPostmixCallback 6953ee27d5b07251db0ce387d9992fc04d432cd097d5b561223965092d5c5995 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioPostmixCallback.#ctor(System.Object,System.IntPtr) 18df6b185cc2db6b2f2465a6e683146c6dac8a1dc951bceca115b9b3cd67cd7d -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioPostmixCallback.Invoke(System.Void*,Inno.Native.Sdl3.SDLAudioSpec*,System.Single*,System.Int32) 0825ecc43722fd26c65b163dbcd89d1a546c21f81d598c2b66210b93d0d853bd -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioPostmixCallback.BeginInvoke(System.Void*,Inno.Native.Sdl3.SDLAudioSpec*,System.Single*,System.Int32,System.AsyncCallback,System.Object) a298173959bc180be9cc5e79b3499217afa8235a34f6da2e0372e9ecd76d9b69 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioPostmixCallback.EndInvoke(System.IAsyncResult) 2c8903a1055e2230e2ea92bd629974077ef2900068a377a9b5deae95ab1ec782 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f6af3f4637ca8ad9"></a>[`public Inno.Native.Sdl3.SDLAudioPostmixCallback.SDLAudioPostmixCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6835) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7ac8eae77275bd45"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLAudioPostmixCallback.BeginInvoke(void* userdata, Inno.Native.Sdl3.SDLAudioSpec* spec, float* buffer, int buflen, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6835) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fa5066226e93296e"></a>[`public virtual void Inno.Native.Sdl3.SDLAudioPostmixCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6835) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-02d9a1aa69580842"></a>[`public virtual void Inno.Native.Sdl3.SDLAudioPostmixCallback.Invoke(void* userdata, Inno.Native.Sdl3.SDLAudioSpec* spec, float* buffer, int buflen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6835) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dc642905dd94637d"></a>[`public delegate void Inno.Native.Sdl3.SDLAudioPostmixCallback(void* userdata, Inno.Native.Sdl3.SDLAudioSpec* spec, float* buffer, int buflen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6835) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAudioSpec</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAudioSpec e919ded693be18432fa46f299ae47f7dd6f20b5de150913f570b799e3c0d5d5e -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioSpec.Format 09c3e24f3e67a3c698e5c8d1617c71fa060244dbbbcce402190e4ca8ac11ab7e -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioSpec.Channels 0afa2ea6e6c1a86ebd4602f62ce52ebf4f050f555ffed076a919fd599898420c -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioSpec.Freq a64b80c65e2151fee9e383d2b963ac9fcf59486526bd6dd57019059763fbd197 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioSpec.#ctor(Inno.Native.Sdl3.SDLAudioFormat,System.Int32,System.Int32) 197eff235260f88c0c73aed92c052ada17d53b9863a4f14eb5e0ff3b4ab4f8a5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c61518e335db02ef"></a>[`public int Inno.Native.Sdl3.SDLAudioSpec.Channels`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2285) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-34f6d2866a3b06c5"></a>[`public Inno.Native.Sdl3.SDLAudioFormat Inno.Native.Sdl3.SDLAudioSpec.Format`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2284) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2c07b10053eade18"></a>[`public int Inno.Native.Sdl3.SDLAudioSpec.Freq`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2286) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6a38d29df79d0dc5"></a>[`public Inno.Native.Sdl3.SDLAudioSpec.SDLAudioSpec(Inno.Native.Sdl3.SDLAudioFormat format = Inno.Native.Sdl3.SDLAudioFormat.Unknown, int channels = 0, int freq = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2287) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b8de248314be5ebd"></a>[`public struct Inno.Native.Sdl3.SDLAudioSpec`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2282) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAudioSpecPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAudioSpecPtr 09c1ace73e4dcf07638c7ad1a72fe5298da6d9d7e441897b7b6ffb1c81858c97 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioSpecPtr.#ctor(Inno.Native.Sdl3.SDLAudioSpec*) 87335c1c9dd6b445e2441aa5e48f26c6932ff95d979c69b0211fafc8c8aceb8d -->
<!-- api: F:Inno.Native.Sdl3.SDLAudioSpecPtr.Handle ef9c0482661dbb19cfa49976995d8679d510ac1dfd4c9ca01ec1a9d49f79be7e -->
<!-- api: P:Inno.Native.Sdl3.SDLAudioSpecPtr.IsNull fe46110cb1ddef23501fff7995395a3344a1e613a1804cd7a2b2f79022e4f9ad -->
<!-- api: P:Inno.Native.Sdl3.SDLAudioSpecPtr.Null 81932cc709b7d6b1cc600a1ee8ace2e6a78a721f13996e117b79fc571d23eb04 -->
<!-- api: P:Inno.Native.Sdl3.SDLAudioSpecPtr.Item(System.Int32) 94f611e65e1c89d7d07497657e66ddc4e8e5d7c2a92c45b93398b1c7b5578cbc -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioSpecPtr.op_Implicit(Inno.Native.Sdl3.SDLAudioSpec*)~Inno.Native.Sdl3.SDLAudioSpecPtr 373d4242827760f5e74b4bcb12126658f367a3cacb55bcd93d03919ff1744fcd -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioSpecPtr.op_Implicit(Inno.Native.Sdl3.SDLAudioSpecPtr)~Inno.Native.Sdl3.SDLAudioSpec* 6faba4f7902c5fb5b6117c803ca9aa0bed3090e36ab48973d388097d1dce4598 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioSpecPtr.op_Equality(Inno.Native.Sdl3.SDLAudioSpecPtr,Inno.Native.Sdl3.SDLAudioSpecPtr) b4600d8b47481e7c35b79afa8c7f480d0b6dd3d1df455bbcaaabfff96396477d -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioSpecPtr.op_Inequality(Inno.Native.Sdl3.SDLAudioSpecPtr,Inno.Native.Sdl3.SDLAudioSpecPtr) bc3c2f97f9cabc8937b21406a38deac231c1e1558b64fe664f08cc9f46f86451 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioSpecPtr.Equals(Inno.Native.Sdl3.SDLAudioSpecPtr) 9dbfbe96bc9408bd8b3256e11c5f054bfc5667d9e1b9fff423874197dd07597a -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioSpecPtr.Equals(System.Object) 808a4bd7258380b76dc0f9872ee8d2553492c3efdd755e762308342672eb35ca -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioSpecPtr.GetHashCode 635a42d51e61ee9db87179785e9797e10ffe4fb85054f912186bb35e578ff9b2 -->
<!-- api: P:Inno.Native.Sdl3.SDLAudioSpecPtr.Format 09ec57fe5b4d384cfdfa8856f9e7c2a28728ae9f300d8f425fd5e7fff7fa4d8a -->
<!-- api: P:Inno.Native.Sdl3.SDLAudioSpecPtr.Channels 9819fcba4449280577ad697dd2e4aaced9ed44e7db3fd42ca61d244023587221 -->
<!-- api: P:Inno.Native.Sdl3.SDLAudioSpecPtr.Freq 21dfefa36593670f2e164613e20bc9c976fedf110785601924f19a00b930aa17 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-56759e235d43de50"></a>[`public Inno.Native.Sdl3.SDLAudioSpec* Inno.Native.Sdl3.SDLAudioSpecPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8011) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-81750ac625117692"></a>[`public Inno.Native.Sdl3.SDLAudioSpecPtr.SDLAudioSpecPtr(Inno.Native.Sdl3.SDLAudioSpec* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8006) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8c0973e244971bb0"></a>[`public bool Inno.Native.Sdl3.SDLAudioSpecPtr.Equals(Inno.Native.Sdl3.SDLAudioSpecPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8021) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-057ff6f91fc31508"></a>[`public override bool Inno.Native.Sdl3.SDLAudioSpecPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8022) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-1cb154efceae0c5e"></a>[`public override int Inno.Native.Sdl3.SDLAudioSpecPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8023) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-961cbda37e92ad69"></a>[`public static bool Inno.Native.Sdl3.SDLAudioSpecPtr.operator ==(Inno.Native.Sdl3.SDLAudioSpecPtr left, Inno.Native.Sdl3.SDLAudioSpecPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8019) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-318548f8ffccfd27"></a>[`public static Inno.Native.Sdl3.SDLAudioSpecPtr.implicit operator Inno.Native.Sdl3.SDLAudioSpecPtr(Inno.Native.Sdl3.SDLAudioSpec* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8017) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d53b84ced2942909"></a>[`public static Inno.Native.Sdl3.SDLAudioSpecPtr.implicit operator Inno.Native.Sdl3.SDLAudioSpec*(Inno.Native.Sdl3.SDLAudioSpecPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8018) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f85d8f9123c7d68b"></a>[`public static bool Inno.Native.Sdl3.SDLAudioSpecPtr.operator !=(Inno.Native.Sdl3.SDLAudioSpecPtr left, Inno.Native.Sdl3.SDLAudioSpecPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8020) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ebbd4928bd285db3"></a>[`public ref int Inno.Native.Sdl3.SDLAudioSpecPtr.Channels { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8026) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9459ec586e243f80"></a>[`public ref Inno.Native.Sdl3.SDLAudioFormat Inno.Native.Sdl3.SDLAudioSpecPtr.Format { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8025) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-978ce4de8ef4eeb6"></a>[`public ref int Inno.Native.Sdl3.SDLAudioSpecPtr.Freq { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8027) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-acb8bdb69538a480"></a>[`public bool Inno.Native.Sdl3.SDLAudioSpecPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8012) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ff187b7762d85a0b"></a>[`public Inno.Native.Sdl3.SDLAudioSpec Inno.Native.Sdl3.SDLAudioSpecPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8015) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-db82ad3d03bfb173"></a>[`public static Inno.Native.Sdl3.SDLAudioSpecPtr Inno.Native.Sdl3.SDLAudioSpecPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8013) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0a7921f0cbd3e95e"></a>[`public struct Inno.Native.Sdl3.SDLAudioSpecPtr : System.IEquatable<Inno.Native.Sdl3.SDLAudioSpecPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8004) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAudioStream</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAudioStream 03dbf43bb8562cdbfe37d1933047f4a28b0ba099df58d4939b05b971fc26f54d -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStream.#ctor(System.IntPtr) 267537a342f4b7402e39d2323c464cf332ea33e7d30ec5610a999709ae2b9ad6 -->
<!-- api: P:Inno.Native.Sdl3.SDLAudioStream.Handle 755c9c36f512470552067695a93e9460e7304042f09db3000fbae65746bde497 -->
<!-- api: P:Inno.Native.Sdl3.SDLAudioStream.IsNull 5cc1550b2127436ae121efbdd8b08cc36f7f4c0e053882fe1d68f0bac62a776f -->
<!-- api: P:Inno.Native.Sdl3.SDLAudioStream.Null c365adf752ae02e51e789a112212fdf4fc2f10a120b5a8f7aad931dcc28e8274 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStream.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLAudioStream 9974804f7b95f3e58629c86f462fc5307afd805ce892d700c60982c6b90f5e09 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStream.op_Equality(Inno.Native.Sdl3.SDLAudioStream,Inno.Native.Sdl3.SDLAudioStream) 4bc54b7ff7b2b24b34b74ddbe0179011a5645aa25ee56512b197c1e0dec8e91e -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStream.op_Inequality(Inno.Native.Sdl3.SDLAudioStream,Inno.Native.Sdl3.SDLAudioStream) d12d0e8d69bc6fb1a55d3041ba6c2c603f33d9061a4798e8a0ba2f299311cb2d -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStream.Equals(Inno.Native.Sdl3.SDLAudioStream) 0bf2d0f537f72b7201549cff6a9b43e8d78e6d525ffa9943edd78c83aa177bbb -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStream.Equals(System.Object) 487b20cf9afb0af0048a69403138cffd1c155663cf4902b5b5f962426bb3ba8e -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStream.GetHashCode 872140a3956831fbc85d14708feb6727d84442f1641faf7d4f6599b39b3110c1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-096600fb4ba0add1"></a>[`public Inno.Native.Sdl3.SDLAudioStream.SDLAudioStream(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2301) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-21ca836b989aa877"></a>[`public bool Inno.Native.Sdl3.SDLAudioStream.Equals(Inno.Native.Sdl3.SDLAudioStream other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2313) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-a3be27b2ca81ac6c"></a>[`public override bool Inno.Native.Sdl3.SDLAudioStream.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2314) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-1a40f1659db19166"></a>[`public override int Inno.Native.Sdl3.SDLAudioStream.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2315) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-19394ac7db525c9a"></a>[`public static bool Inno.Native.Sdl3.SDLAudioStream.operator ==(Inno.Native.Sdl3.SDLAudioStream left, Inno.Native.Sdl3.SDLAudioStream right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2311) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ce5808abfdad2dcc"></a>[`public static Inno.Native.Sdl3.SDLAudioStream.implicit operator Inno.Native.Sdl3.SDLAudioStream(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2310) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4dc427268c7c297c"></a>[`public static bool Inno.Native.Sdl3.SDLAudioStream.operator !=(Inno.Native.Sdl3.SDLAudioStream left, Inno.Native.Sdl3.SDLAudioStream right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2312) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d8ec8fc31023a761"></a>[`public nint Inno.Native.Sdl3.SDLAudioStream.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2306) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2e580db994d11feb"></a>[`public bool Inno.Native.Sdl3.SDLAudioStream.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2307) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-758ff25935417c82"></a>[`public static Inno.Native.Sdl3.SDLAudioStream Inno.Native.Sdl3.SDLAudioStream.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2308) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-74c60c099ddcc6f9"></a>[`public readonly struct Inno.Native.Sdl3.SDLAudioStream : System.IEquatable<Inno.Native.Sdl3.SDLAudioStream>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2299) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAudioStreamCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAudioStreamCallback e095fab7eddd45291592b564c84e3369cdc1e4682220091f2a14ed6fca0694ce -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStreamCallback.#ctor(System.Object,System.IntPtr) 50ecedbef5ef6c77daddf4e70fda023ad8f4d29ca6c2e56f6fbcaa22a20fdf5e -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStreamCallback.Invoke(System.Void*,Inno.Native.Sdl3.SDLAudioStream,System.Int32,System.Int32) da0d55b9df856f933c60cb9ece80668e341abea6d83d6eb75593e2415153092c -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStreamCallback.BeginInvoke(System.Void*,Inno.Native.Sdl3.SDLAudioStream,System.Int32,System.Int32,System.AsyncCallback,System.Object) 8b7c2e3a6d40ba5ea824bf7d1fee2a9cde804c5b3fb5c4f1508bec438031ec62 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStreamCallback.EndInvoke(System.IAsyncResult) 76318f3f144ee99cb1d9125b9ebf62cdd63803a1a80203d212bd7f17fdc9d2aa -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-11ff2889660f3c69"></a>[`public Inno.Native.Sdl3.SDLAudioStreamCallback.SDLAudioStreamCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6830) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-331635dd9568b238"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLAudioStreamCallback.BeginInvoke(void* userdata, Inno.Native.Sdl3.SDLAudioStream stream, int additionalAmount, int totalAmount, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6830) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e23d6d7b64c72530"></a>[`public virtual void Inno.Native.Sdl3.SDLAudioStreamCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6830) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f8fd15fc47d43850"></a>[`public virtual void Inno.Native.Sdl3.SDLAudioStreamCallback.Invoke(void* userdata, Inno.Native.Sdl3.SDLAudioStream stream, int additionalAmount, int totalAmount)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6830) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a49403d54f23e6bf"></a>[`public delegate void Inno.Native.Sdl3.SDLAudioStreamCallback(void* userdata, Inno.Native.Sdl3.SDLAudioStream stream, int additionalAmount, int totalAmount)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6830) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLAudioStreamDataCompleteCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLAudioStreamDataCompleteCallback 025d08243eff21711878ed8d0070002e8f03bf2d4c2641320f18647703f1a5b1 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStreamDataCompleteCallback.#ctor(System.Object,System.IntPtr) 64d28c8b52ed7839dcf5e635f5e5fd681628c28f1f1c26c703b640eec28fcbc2 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStreamDataCompleteCallback.Invoke(System.Void*,System.Void*,System.Int32) 7797745d05ee33317c013c619134b68150bb32bec4b160cc4ca341f2b4ea37a2 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStreamDataCompleteCallback.BeginInvoke(System.Void*,System.Void*,System.Int32,System.AsyncCallback,System.Object) f71037b12f787fe6e2c6910347912c27a02a3572bad90b48c227b4deab907201 -->
<!-- api: M:Inno.Native.Sdl3.SDLAudioStreamDataCompleteCallback.EndInvoke(System.IAsyncResult) 4a2b5e1b98915d4a1bf804c9f508432e8843ac683979c077452e3e8a25f2c602 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-14d9b46bee6bd518"></a>[`public Inno.Native.Sdl3.SDLAudioStreamDataCompleteCallback.SDLAudioStreamDataCompleteCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6825) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-21a184644f5b5c25"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLAudioStreamDataCompleteCallback.BeginInvoke(void* userdata, void* buf, int buflen, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6825) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-64d9c2b8dabba6a0"></a>[`public virtual void Inno.Native.Sdl3.SDLAudioStreamDataCompleteCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6825) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-88d0815b9c8923be"></a>[`public virtual void Inno.Native.Sdl3.SDLAudioStreamDataCompleteCallback.Invoke(void* userdata, void* buf, int buflen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6825) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c9294042134180a4"></a>[`public delegate void Inno.Native.Sdl3.SDLAudioStreamDataCompleteCallback(void* userdata, void* buf, int buflen)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6825) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLBitmapOrder</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLBitmapOrder ead51ba0d770612cbc2389e1fc7e3d6a8fa59cea918c0d6adac9fe9408399633 -->
<!-- api: F:Inno.Native.Sdl3.SDLBitmapOrder.BitmaporderNone 731581b1995785da558429e73d860b52a0570a2839ecd6080863102430bb5db8 -->
<!-- api: F:Inno.Native.Sdl3.SDLBitmapOrder.Bitmaporder4321 469c4d09c87ab2605c8e5783440056bf403f72f85e5fc67e674083b6fdab8183 -->
<!-- api: F:Inno.Native.Sdl3.SDLBitmapOrder.Bitmaporder1234 8c5766829e14149f8f5307ebe72ac041f24979aa03addc4cd7a735630fae36ea -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7a058370ea2674a7"></a>[`Inno.Native.Sdl3.SDLBitmapOrder.Bitmaporder1234 = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L251) | 当前常量或枚举值：`2`。 | — |
| <a id="api-3deac0be451037d8"></a>[`Inno.Native.Sdl3.SDLBitmapOrder.Bitmaporder4321 = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L250) | 当前常量或枚举值：`1`。 | — |
| <a id="api-44df607fbb85f83d"></a>[`Inno.Native.Sdl3.SDLBitmapOrder.BitmaporderNone = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L249) | 当前常量或枚举值：`0`。 | — |
| <a id="api-0d94083fe0eaa031"></a>[`public enum Inno.Native.Sdl3.SDLBitmapOrder : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L247) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLBlendFactor</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLBlendFactor a508f62666cb04fc3e8b594ce3994959bfbd3bc05d2428bb0531493823566bdd -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendFactor.BlendfactorZero f591aac48b6d9d13d2446b9a798cc708f828fcedbe1b6bebb9308464a1808d4d -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendFactor.BlendfactorOne 950b4eeab39e25557d1d0ab7962be3fb398dccd524e13be9601f5ba2532a2e05 -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendFactor.BlendfactorSrcColor 61b596cad2c258ba8cab29d1b1d7c5af250ba3570acd1cc3cd2a663a0f661c5d -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendFactor.BlendfactorOneMinusSrcColor 3c57ffdbb3b195cf547fc47d28d6e7423df9466f387cf702a2ddf6fd8caab360 -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendFactor.BlendfactorSrcAlpha 26c954bef1446cec0e1cb5ada5a087a4d50621935b04e01cd12a7a2c08475d52 -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendFactor.BlendfactorOneMinusSrcAlpha 29851fe025af753c9b43c8683488773b90efb552a5b0e8324bbc8b180970b9ea -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendFactor.BlendfactorDstColor 01c96aed272fc3e4ddc5cd199f598a13d0aeead4f80f96b935ac08afe45c8ccd -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendFactor.BlendfactorOneMinusDstColor 891c2ddfa625fa50cf647f543e04b804cbbca6a88b689b5611e1a9c355e7b0d0 -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendFactor.BlendfactorDstAlpha 9ff79d85efe91e3514a7f26866a904c9017c62c87980063a4313985d785d1ff4 -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendFactor.BlendfactorOneMinusDstAlpha 804bfcadd1006794430dbdb3f511450580df7f6a2a459a35f14ee38ec378ddfa -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b21fcc5327b95bd7"></a>[`Inno.Native.Sdl3.SDLBlendFactor.BlendfactorDstAlpha = 9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L220) | 当前常量或枚举值：`9`。 | — |
| <a id="api-e730f321eacac89a"></a>[`Inno.Native.Sdl3.SDLBlendFactor.BlendfactorDstColor = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L218) | 当前常量或枚举值：`7`。 | — |
| <a id="api-7fb19e02dca580e7"></a>[`Inno.Native.Sdl3.SDLBlendFactor.BlendfactorOne = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L213) | 当前常量或枚举值：`2`。 | — |
| <a id="api-c914dd47ee91afbc"></a>[`Inno.Native.Sdl3.SDLBlendFactor.BlendfactorOneMinusDstAlpha = 10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L221) | 当前常量或枚举值：`10`。 | — |
| <a id="api-bc0bc05c75e4aea6"></a>[`Inno.Native.Sdl3.SDLBlendFactor.BlendfactorOneMinusDstColor = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L219) | 当前常量或枚举值：`8`。 | — |
| <a id="api-2f4ea36647d3e1b7"></a>[`Inno.Native.Sdl3.SDLBlendFactor.BlendfactorOneMinusSrcAlpha = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L217) | 当前常量或枚举值：`6`。 | — |
| <a id="api-f5f11611fecc9040"></a>[`Inno.Native.Sdl3.SDLBlendFactor.BlendfactorOneMinusSrcColor = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L215) | 当前常量或枚举值：`4`。 | — |
| <a id="api-cc433f5e909346c0"></a>[`Inno.Native.Sdl3.SDLBlendFactor.BlendfactorSrcAlpha = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L216) | 当前常量或枚举值：`5`。 | — |
| <a id="api-3294a9d9f0e2aa7e"></a>[`Inno.Native.Sdl3.SDLBlendFactor.BlendfactorSrcColor = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L214) | 当前常量或枚举值：`3`。 | — |
| <a id="api-9540db5560ee5408"></a>[`Inno.Native.Sdl3.SDLBlendFactor.BlendfactorZero = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L212) | 当前常量或枚举值：`1`。 | — |
| <a id="api-67dbf5cab1826e1c"></a>[`public enum Inno.Native.Sdl3.SDLBlendFactor : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L210) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLBlendOperation</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLBlendOperation 3c06a2d09e5b353800a6482dd882dc811b1ecc90f73f256f25397d50e668f92d -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendOperation.BlendoperationAdd 9c70552d23c0f0cdfc153706d6e34e7d260273fc351248eae518608bf344a63f -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendOperation.BlendoperationSubtract a33aaa5fd007646d92ef6c8985cecbcda4f99827f9721fb68c5645a92f470053 -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendOperation.BlendoperationRevSubtract 46688076dc25677fefcb93250bd5d2c789107d1567c124d8f56f58095058ff83 -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendOperation.BlendoperationMinimum a4767d35115fed1bd121f417cd282e2464992dbca7c9e81b60c2adc464c8d834 -->
<!-- api: F:Inno.Native.Sdl3.SDLBlendOperation.BlendoperationMaximum 0823f16b1d313def302d29392596a4146bdaf95374782cf11b17164f0aa2614b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7e9c8839b940077b"></a>[`Inno.Native.Sdl3.SDLBlendOperation.BlendoperationAdd = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L200) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d10a1c04b49b40db"></a>[`Inno.Native.Sdl3.SDLBlendOperation.BlendoperationMaximum = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L204) | 当前常量或枚举值：`5`。 | — |
| <a id="api-f1777a6d38773f81"></a>[`Inno.Native.Sdl3.SDLBlendOperation.BlendoperationMinimum = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L203) | 当前常量或枚举值：`4`。 | — |
| <a id="api-7e0cd7965c94bb60"></a>[`Inno.Native.Sdl3.SDLBlendOperation.BlendoperationRevSubtract = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L202) | 当前常量或枚举值：`3`。 | — |
| <a id="api-1aebf263440ac35f"></a>[`Inno.Native.Sdl3.SDLBlendOperation.BlendoperationSubtract = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L201) | 当前常量或枚举值：`2`。 | — |
| <a id="api-3803595770b36754"></a>[`public enum Inno.Native.Sdl3.SDLBlendOperation : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L198) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCallocFunc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCallocFunc bfe222ce4c7a2b0bf8e52ea53363620efa2be1465839291985a3a9cfd11d0c7a -->
<!-- api: M:Inno.Native.Sdl3.SDLCallocFunc.#ctor(System.Object,System.IntPtr) d6b9d392fca003f5945f70e6e3048d4b71ff31323e4aeef467ce905a0b9781eb -->
<!-- api: M:Inno.Native.Sdl3.SDLCallocFunc.Invoke(System.UInt64,System.UInt64) d42d5966100518019efc0f93d5b5cb53321a08d8ae038ab478a896953d1e5ed9 -->
<!-- api: M:Inno.Native.Sdl3.SDLCallocFunc.BeginInvoke(System.UInt64,System.UInt64,System.AsyncCallback,System.Object) 2d15a1ef74a3321ef32c4b89d55bd8213a55630228aca22edacc775110eb51b2 -->
<!-- api: M:Inno.Native.Sdl3.SDLCallocFunc.EndInvoke(System.IAsyncResult) 5abe8789558440026cd439b23c4e14f258b53400e224f6a403999dada74f415b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5a15f6c3f1a393ba"></a>[`public Inno.Native.Sdl3.SDLCallocFunc.SDLCallocFunc(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6770) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-20d8d92655049cc1"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLCallocFunc.BeginInvoke(ulong nmemb, ulong size, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6770) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e93e41620ef21e8"></a>[`public virtual void* Inno.Native.Sdl3.SDLCallocFunc.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6770) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-06be1b3e7834a9d7"></a>[`public virtual void* Inno.Native.Sdl3.SDLCallocFunc.Invoke(ulong nmemb, ulong size)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6770) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8e97382abbc64d3a"></a>[`public delegate void* Inno.Native.Sdl3.SDLCallocFunc(ulong nmemb, ulong size)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6770) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCamera</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCamera 71913eb66a46ae57684cd53872adbf740f0e50f15e828274c1ad6c728f1ac3c0 -->
<!-- api: M:Inno.Native.Sdl3.SDLCamera.#ctor(System.IntPtr) 616ae88d45aea4372d8b0e6ea680928fba7f8db5d6e8388d852a9af88ded2bb6 -->
<!-- api: P:Inno.Native.Sdl3.SDLCamera.Handle e2d10443a2cedf977b7232e5f32450279fe7ccb815c0c9ae677e300e4311c744 -->
<!-- api: P:Inno.Native.Sdl3.SDLCamera.IsNull 69f9accacea98ae8b0d750dd6581e98e2e876c8a4e987c4fcc2f7a9844f162c8 -->
<!-- api: P:Inno.Native.Sdl3.SDLCamera.Null c8a9ceb93827bc251018a40baf705679c713f26a60051abb3b4c167a94ad55a6 -->
<!-- api: M:Inno.Native.Sdl3.SDLCamera.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLCamera c969fd44f4e618c79c605123d591d61f6e0236c14d397a5a9650bf54f212f0d9 -->
<!-- api: M:Inno.Native.Sdl3.SDLCamera.op_Equality(Inno.Native.Sdl3.SDLCamera,Inno.Native.Sdl3.SDLCamera) 6db5b17e0f9a28c4c1279539a567ec9c9779f036ae27fdeb875df4ec4c01e3df -->
<!-- api: M:Inno.Native.Sdl3.SDLCamera.op_Inequality(Inno.Native.Sdl3.SDLCamera,Inno.Native.Sdl3.SDLCamera) cbf5ea71dc9f527934ba301c531e931332962fa13b21facd8833c1d56c1f1bff -->
<!-- api: M:Inno.Native.Sdl3.SDLCamera.Equals(Inno.Native.Sdl3.SDLCamera) bae22424dc675749972cf4c67c0889c2333756fb7ed745d282a778e238a7e393 -->
<!-- api: M:Inno.Native.Sdl3.SDLCamera.Equals(System.Object) 7f041272a8b990e18d77f8b7995a42ddff4c0dafbc22abb894d39743acddb952 -->
<!-- api: M:Inno.Native.Sdl3.SDLCamera.GetHashCode 433951e993eebd567e1d18b7824029efa053fd3417c545901dd5f9a5637f7e65 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b23e706093c58e3b"></a>[`public Inno.Native.Sdl3.SDLCamera.SDLCamera(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2528) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-de4c715e5b3b75d1"></a>[`public bool Inno.Native.Sdl3.SDLCamera.Equals(Inno.Native.Sdl3.SDLCamera other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2540) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-8ecf823690e55ee7"></a>[`public override bool Inno.Native.Sdl3.SDLCamera.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2541) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-6178d800fd4f18b4"></a>[`public override int Inno.Native.Sdl3.SDLCamera.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2542) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-6f91f97727f8b1bd"></a>[`public static bool Inno.Native.Sdl3.SDLCamera.operator ==(Inno.Native.Sdl3.SDLCamera left, Inno.Native.Sdl3.SDLCamera right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2538) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1f3dd6a5ca5fdd7c"></a>[`public static Inno.Native.Sdl3.SDLCamera.implicit operator Inno.Native.Sdl3.SDLCamera(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2537) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ee078a654ab033c6"></a>[`public static bool Inno.Native.Sdl3.SDLCamera.operator !=(Inno.Native.Sdl3.SDLCamera left, Inno.Native.Sdl3.SDLCamera right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2539) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-82d38f86902ca215"></a>[`public nint Inno.Native.Sdl3.SDLCamera.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2533) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-78a609499839e77c"></a>[`public bool Inno.Native.Sdl3.SDLCamera.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2534) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-99cf6af0f577a43c"></a>[`public static Inno.Native.Sdl3.SDLCamera Inno.Native.Sdl3.SDLCamera.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2535) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-38ed3b4d9f66108b"></a>[`public readonly struct Inno.Native.Sdl3.SDLCamera : System.IEquatable<Inno.Native.Sdl3.SDLCamera>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2526) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCameraDeviceEvent</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCameraDeviceEvent 89c014bf1398c11f36c6c256524f872979fffa693c705b0daa4a3fb6324a089c -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraDeviceEvent.Type 65db14e006dcd1d51fe29dbe8dc1c966619b7c8502d145e357fc07ef6bb7eb98 -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraDeviceEvent.Reserved c367a9481dbd6cd28e39a96075511f7afcae396254a8867ee30f9e7674b5457b -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraDeviceEvent.Timestamp fdb8c2f59e69e09e8cf7dcdeeaa1ddbc49fb71de9f84fc01973cf6ac0bf1dc0a -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraDeviceEvent.Which 6ed26cd556c10d5f191a58a6f24b31ef52d6c2848d6c787509f0346f9a87edac -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraDeviceEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt32) 3dc3b5fcd9b6a1e76df2e0f2fb11509e15dc7e7e6154bb9225cd8c3bef650b4e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-baf443c1715c412e"></a>[`public uint Inno.Native.Sdl3.SDLCameraDeviceEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3712) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6ec588ac9b22afac"></a>[`public ulong Inno.Native.Sdl3.SDLCameraDeviceEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3713) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-936944e32626ec4a"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLCameraDeviceEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3711) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf5e857e304db4be"></a>[`public uint Inno.Native.Sdl3.SDLCameraDeviceEvent.Which`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3714) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b157052513748d63"></a>[`public Inno.Native.Sdl3.SDLCameraDeviceEvent.SDLCameraDeviceEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, uint which = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3715) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1ecd755a178d04c0"></a>[`public struct Inno.Native.Sdl3.SDLCameraDeviceEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3709) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCameraPermissionState</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCameraPermissionState 646dd0496efd34eada25c517697e207adb671e3d14e2f1cc434646e11298c89e -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraPermissionState.Denied ca0fad9cb4a8f2883c44068ae65e8dd90ddb44beb3de071c7a0ce98c406f49fe -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraPermissionState.Pending 2bd1df20863d5b5571f91bfe57571001ca5cfd61fe9b56998bcad30871e54573 -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraPermissionState.Approved 68f8217db0a306477c51380d9bef601e0ac73a0d30f5bb8ae16c9f7015abf8c7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-cb1de97e860e5187"></a>[`Inno.Native.Sdl3.SDLCameraPermissionState.Approved = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L539) | 当前常量或枚举值：`1`。 | — |
| <a id="api-599e0838b89a2a91"></a>[`Inno.Native.Sdl3.SDLCameraPermissionState.Denied = -1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L537) | 当前常量或枚举值：`-1`。 | — |
| <a id="api-1629256fe97e7112"></a>[`Inno.Native.Sdl3.SDLCameraPermissionState.Pending = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L538) | 当前常量或枚举值：`0`。 | — |
| <a id="api-cf3c394ffc9aacba"></a>[`public enum Inno.Native.Sdl3.SDLCameraPermissionState : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L535) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCameraPosition</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCameraPosition adef8ee0ea799bbad4c5077991d0069bf0883997bd8a0d4b334df741f352d164 -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraPosition.Unknown cebc1993f043418e89f3063221cb0858b753ee875c99d6162be953e3bf892396 -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraPosition.FrontFacing c04c2ccaf3ec249b91f7875ce7ee3a81e0d393a77cb4d75a0dd4674f2e40e588 -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraPosition.BackFacing e8ad54c8e4b5df1fee9db8a8fe5b4ddfe84c8b483437dddce4f989858cfe5058 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-71f630f8a83fb1e0"></a>[`Inno.Native.Sdl3.SDLCameraPosition.BackFacing = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L529) | 当前常量或枚举值：`2`。 | — |
| <a id="api-84fb386f26f5ba25"></a>[`Inno.Native.Sdl3.SDLCameraPosition.FrontFacing = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L528) | 当前常量或枚举值：`1`。 | — |
| <a id="api-60069e126add3151"></a>[`Inno.Native.Sdl3.SDLCameraPosition.Unknown = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L527) | 当前常量或枚举值：`0`。 | — |
| <a id="api-7b9d73d3a7142ead"></a>[`public enum Inno.Native.Sdl3.SDLCameraPosition : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L525) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCameraSpec</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCameraSpec 9ed67b930be5c8243980ca68733db1f83c4dc1ab1628359ba2701efc7e037a2b -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraSpec.Format a0848b16a075deaa3c33d69d76d6bea332dd99eb1e2841ad78ece3e9e3f72915 -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraSpec.Colorspace 5cd54a3357d0acc307bbff18b86d82e7b0b3c12d803e446cf36c027e4164c5b5 -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraSpec.Width 67f25c9ac3bd2edd22f7f145661f0d676b3bee617b73e71ea222f4538f4e2911 -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraSpec.Height e6c1cd670ff9faffe82a1a8d24e4f66bd5c7c40185c84a38b6991e6a1f2f5e90 -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraSpec.FramerateNumerator da5dfc89964b36ae4bc85b5ac6859ea70a0f1372115bd698bcd99c5b073dd58a -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraSpec.FramerateDenominator 0cf77352ffc5279fe350abdf0d1974d61aab114afd821b053bb999ca6575db88 -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpec.#ctor(Inno.Native.Sdl3.SDLPixelFormat,Inno.Native.Sdl3.SDLColorspace,System.Int32,System.Int32,System.Int32,System.Int32) 28553a71cfc0da5387553cd6ed636aa9cd03f4f8c0474e4bc611b692992c0edf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f6559ec4f24db1e5"></a>[`public Inno.Native.Sdl3.SDLColorspace Inno.Native.Sdl3.SDLCameraSpec.Colorspace`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2553) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f77e6ee8d1c5214a"></a>[`public Inno.Native.Sdl3.SDLPixelFormat Inno.Native.Sdl3.SDLCameraSpec.Format`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2552) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c24725694e9cfc9"></a>[`public int Inno.Native.Sdl3.SDLCameraSpec.FramerateDenominator`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2557) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5ca7c7f6eeb5e970"></a>[`public int Inno.Native.Sdl3.SDLCameraSpec.FramerateNumerator`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2556) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-73ded881f659ad20"></a>[`public int Inno.Native.Sdl3.SDLCameraSpec.Height`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2555) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-97f7eb4148cb8904"></a>[`public int Inno.Native.Sdl3.SDLCameraSpec.Width`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2554) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab96dff2d5bd6f57"></a>[`public Inno.Native.Sdl3.SDLCameraSpec.SDLCameraSpec(Inno.Native.Sdl3.SDLPixelFormat format = Inno.Native.Sdl3.SDLPixelFormat.PixelformatUnknown, Inno.Native.Sdl3.SDLColorspace colorspace = Inno.Native.Sdl3.SDLColorspace.Unknown, int width = 0, int height = 0, int framerate_numerator = 0, int framerate_denominator = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2558) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b0dc6d3c61c6b7b5"></a>[`public struct Inno.Native.Sdl3.SDLCameraSpec`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2550) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCameraSpecPtr</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCameraSpecPtr 2c5d0d42ff5b0bc304e87f3629ff50a05b54a421dd64187fb54d508692f33dd2 -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtr.#ctor(Inno.Native.Sdl3.SDLCameraSpec*) c1a4b456da6c4cac88a0af407d8ed99f48b9a35788cb483ffdeb9815876f0ecd -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraSpecPtr.Handle 277cbb50725021afbbd2541c2e83bcf8daa53ec16430050e3a64abc98f44fafa -->
<!-- api: P:Inno.Native.Sdl3.SDLCameraSpecPtr.IsNull b375a7f7c69ed31a37348189faf8b18a01472925f0d0ca3d3f8637d0b58c3651 -->
<!-- api: P:Inno.Native.Sdl3.SDLCameraSpecPtr.Null c5133facc14637e6fc89089be60e1524fa6cf9ed5a6008df2c1f6a99c7f31ac7 -->
<!-- api: P:Inno.Native.Sdl3.SDLCameraSpecPtr.Item(System.Int32) 3d1df92cc35fb3211b9cdec1e8cfd7057433f5efd57a60f47d3bd849e697959c -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtr.op_Implicit(Inno.Native.Sdl3.SDLCameraSpec*)~Inno.Native.Sdl3.SDLCameraSpecPtr da72780f302fed4922f27068e3a102943e0479200ebfc478ecc3006535e7aa44 -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtr.op_Implicit(Inno.Native.Sdl3.SDLCameraSpecPtr)~Inno.Native.Sdl3.SDLCameraSpec* e75f9cddfa548d4a7c33bbeef80199c309a1f756c176cdb15bc5b4356880f2ad -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtr.op_Equality(Inno.Native.Sdl3.SDLCameraSpecPtr,Inno.Native.Sdl3.SDLCameraSpecPtr) fd560147eaa812a1f6d349c79676c3009e450329d66a714fe837fa09e22e04cf -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtr.op_Inequality(Inno.Native.Sdl3.SDLCameraSpecPtr,Inno.Native.Sdl3.SDLCameraSpecPtr) a05d16fe4afce4ff3a505a983d16c474392fcbf70506958c022b0750b7b3deed -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtr.Equals(Inno.Native.Sdl3.SDLCameraSpecPtr) d4c7fb27b0c5c895fb7dd0cbcbe1810949cf96ecbee61cf1a36c159f4a2e3d16 -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtr.Equals(System.Object) 4553e7f5b4c7285e706282db02c7387b70d4d4b1886bf37cb422744469e8a06d -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtr.GetHashCode dbd32008a81202833f4e647697a083ca0c76b4567d8875e1e547eb654be4b5b8 -->
<!-- api: P:Inno.Native.Sdl3.SDLCameraSpecPtr.Format fe138d69099213617c8f7519dce052b8b4de9264c316cf02657fca47f5423e8e -->
<!-- api: P:Inno.Native.Sdl3.SDLCameraSpecPtr.Colorspace 07c008588349b15a803473eccfa7767cd0c811cffdb4fa30d7591eb592f0df9b -->
<!-- api: P:Inno.Native.Sdl3.SDLCameraSpecPtr.Width 4cf23d6549fc8fc0535c74aa0e624ece3c34d9b54f5c392cbcb6d13eb8c50c69 -->
<!-- api: P:Inno.Native.Sdl3.SDLCameraSpecPtr.Height 8a74dc0656c69903dac63cc886f447d47d2c6a9bd017a2cf4788d65b31ac026a -->
<!-- api: P:Inno.Native.Sdl3.SDLCameraSpecPtr.FramerateNumerator 1be4ce17a65b1d798b2e50e5c2626a8114817aae39f42e1653abd9f4b4ed81fc -->
<!-- api: P:Inno.Native.Sdl3.SDLCameraSpecPtr.FramerateDenominator 84e5f3cfbef0076997e7105e31dbdbc1c6d6633fe3a5d0708f8d39a86a991577 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ba2507922a5c21c2"></a>[`public Inno.Native.Sdl3.SDLCameraSpec* Inno.Native.Sdl3.SDLCameraSpecPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8038) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-57e4c54de27e9059"></a>[`public Inno.Native.Sdl3.SDLCameraSpecPtr.SDLCameraSpecPtr(Inno.Native.Sdl3.SDLCameraSpec* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8033) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5f936e768e564421"></a>[`public bool Inno.Native.Sdl3.SDLCameraSpecPtr.Equals(Inno.Native.Sdl3.SDLCameraSpecPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8048) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-fdbc8c938a7e2870"></a>[`public override bool Inno.Native.Sdl3.SDLCameraSpecPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8049) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-a399aa2c01368c1f"></a>[`public override int Inno.Native.Sdl3.SDLCameraSpecPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8050) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-225aa8137f931d0c"></a>[`public static bool Inno.Native.Sdl3.SDLCameraSpecPtr.operator ==(Inno.Native.Sdl3.SDLCameraSpecPtr left, Inno.Native.Sdl3.SDLCameraSpecPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8046) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5562f562417f5d06"></a>[`public static Inno.Native.Sdl3.SDLCameraSpecPtr.implicit operator Inno.Native.Sdl3.SDLCameraSpecPtr(Inno.Native.Sdl3.SDLCameraSpec* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8044) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d535f9fb7ecd1e27"></a>[`public static Inno.Native.Sdl3.SDLCameraSpecPtr.implicit operator Inno.Native.Sdl3.SDLCameraSpec*(Inno.Native.Sdl3.SDLCameraSpecPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8045) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a9c30a7c7193ef5e"></a>[`public static bool Inno.Native.Sdl3.SDLCameraSpecPtr.operator !=(Inno.Native.Sdl3.SDLCameraSpecPtr left, Inno.Native.Sdl3.SDLCameraSpecPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8047) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d06242fb29f1a165"></a>[`public ref Inno.Native.Sdl3.SDLColorspace Inno.Native.Sdl3.SDLCameraSpecPtr.Colorspace { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8053) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c723799216491ed4"></a>[`public ref Inno.Native.Sdl3.SDLPixelFormat Inno.Native.Sdl3.SDLCameraSpecPtr.Format { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8052) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c3abbe5d2403ad7b"></a>[`public ref int Inno.Native.Sdl3.SDLCameraSpecPtr.FramerateDenominator { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8057) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5e21aaa4c5050de7"></a>[`public ref int Inno.Native.Sdl3.SDLCameraSpecPtr.FramerateNumerator { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8056) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7132b85488918148"></a>[`public ref int Inno.Native.Sdl3.SDLCameraSpecPtr.Height { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8055) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4caf1697adb8ae99"></a>[`public bool Inno.Native.Sdl3.SDLCameraSpecPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8039) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7fbfe2b0c886fe23"></a>[`public Inno.Native.Sdl3.SDLCameraSpec Inno.Native.Sdl3.SDLCameraSpecPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8042) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-060c75db8d811826"></a>[`public static Inno.Native.Sdl3.SDLCameraSpecPtr Inno.Native.Sdl3.SDLCameraSpecPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8040) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-009d4903c3c95e61"></a>[`public ref int Inno.Native.Sdl3.SDLCameraSpecPtr.Width { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8054) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cfcbfad63089c135"></a>[`public struct Inno.Native.Sdl3.SDLCameraSpecPtr : System.IEquatable<Inno.Native.Sdl3.SDLCameraSpecPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8031) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCameraSpecPtrPtr</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCameraSpecPtrPtr 7d9ccfbc4fd220ad85e2ec1d2f814bef50bf4839b8bfd120de528778b6920f6c -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtrPtr.#ctor(Inno.Native.Sdl3.SDLCameraSpec**) fc9365cf19d17478d5e199181663971c5a1f5ab8d5841ed2ffe0d8ee13c94245 -->
<!-- api: F:Inno.Native.Sdl3.SDLCameraSpecPtrPtr.Handle 67189dba3e5628d991b5d656d519f055d1735e952f16ad869b525cf4690cf925 -->
<!-- api: P:Inno.Native.Sdl3.SDLCameraSpecPtrPtr.IsNull 596d6212b99c6c22ca38d072311b9401b1983ae596dba726a1dc94db8e8b6983 -->
<!-- api: P:Inno.Native.Sdl3.SDLCameraSpecPtrPtr.Null 6d853ac8ce3a4769c88616589972a381b670dffca73a86b9c14640da51b933d0 -->
<!-- api: P:Inno.Native.Sdl3.SDLCameraSpecPtrPtr.Item(System.Int32) a9c2040dfb900b8e558733fc525c03aef53d8c5fe98898d4bbe97d1a49e4ef05 -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtrPtr.op_Implicit(Inno.Native.Sdl3.SDLCameraSpec**)~Inno.Native.Sdl3.SDLCameraSpecPtrPtr e37c9e7b4b3c396f0a28007beda43c78b248ee36535865e66ad7cf111aeafe32 -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtrPtr.op_Implicit(Inno.Native.Sdl3.SDLCameraSpecPtrPtr)~Inno.Native.Sdl3.SDLCameraSpec** 5894c522a3ec575b32c2402039efd113736c56e47b42ed4cfdaadce4cddd718f -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtrPtr.op_Equality(Inno.Native.Sdl3.SDLCameraSpecPtrPtr,Inno.Native.Sdl3.SDLCameraSpecPtrPtr) fe1f735c607c0d49d69d7270691e6dfb5d85bde5234848c08eb4a1850c13ddcf -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtrPtr.op_Inequality(Inno.Native.Sdl3.SDLCameraSpecPtrPtr,Inno.Native.Sdl3.SDLCameraSpecPtrPtr) 8cc7a4086d0e104725d444f0b06498d5a17fd95b459f3432cc9bb0623f37942a -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtrPtr.Equals(Inno.Native.Sdl3.SDLCameraSpecPtrPtr) 7ba91fa00b313dadf37006a495c978f1027a92695aaf9a01532927d75c32ded2 -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtrPtr.Equals(System.Object) c713d42bdf692c030ad9ba8e59c44bfea40e34b448355e8ba8876ce8cd132290 -->
<!-- api: M:Inno.Native.Sdl3.SDLCameraSpecPtrPtr.GetHashCode 224baad555da697c8dbd2725490a57488ae4e23b6579abf61f48873238780539 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2d77ceae788c30ce"></a>[`public Inno.Native.Sdl3.SDLCameraSpec** Inno.Native.Sdl3.SDLCameraSpecPtrPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8068) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a77ed577934789d5"></a>[`public Inno.Native.Sdl3.SDLCameraSpecPtrPtr.SDLCameraSpecPtrPtr(Inno.Native.Sdl3.SDLCameraSpec** handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8063) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d465317698b55c57"></a>[`public bool Inno.Native.Sdl3.SDLCameraSpecPtrPtr.Equals(Inno.Native.Sdl3.SDLCameraSpecPtrPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8078) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-3fd08e255e770084"></a>[`public override bool Inno.Native.Sdl3.SDLCameraSpecPtrPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8079) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-95776fef41d552b6"></a>[`public override int Inno.Native.Sdl3.SDLCameraSpecPtrPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8080) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-0d652106cbfff7a0"></a>[`public static bool Inno.Native.Sdl3.SDLCameraSpecPtrPtr.operator ==(Inno.Native.Sdl3.SDLCameraSpecPtrPtr left, Inno.Native.Sdl3.SDLCameraSpecPtrPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8076) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-07e089e26908dbd7"></a>[`public static Inno.Native.Sdl3.SDLCameraSpecPtrPtr.implicit operator Inno.Native.Sdl3.SDLCameraSpecPtrPtr(Inno.Native.Sdl3.SDLCameraSpec** handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8074) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-693033435a45d500"></a>[`public static Inno.Native.Sdl3.SDLCameraSpecPtrPtr.implicit operator Inno.Native.Sdl3.SDLCameraSpec**(Inno.Native.Sdl3.SDLCameraSpecPtrPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8075) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-35f4a82957ccc47e"></a>[`public static bool Inno.Native.Sdl3.SDLCameraSpecPtrPtr.operator !=(Inno.Native.Sdl3.SDLCameraSpecPtrPtr left, Inno.Native.Sdl3.SDLCameraSpecPtrPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8077) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-794648626c676dd3"></a>[`public bool Inno.Native.Sdl3.SDLCameraSpecPtrPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8069) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a4376fc1116ab742"></a>[`public Inno.Native.Sdl3.SDLCameraSpec* Inno.Native.Sdl3.SDLCameraSpecPtrPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8072) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a244badc5e164eea"></a>[`public static Inno.Native.Sdl3.SDLCameraSpecPtrPtr Inno.Native.Sdl3.SDLCameraSpecPtrPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8070) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f539471eb1d69df1"></a>[`public struct Inno.Native.Sdl3.SDLCameraSpecPtrPtr : System.IEquatable<Inno.Native.Sdl3.SDLCameraSpecPtrPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8061) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCapitalization</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCapitalization 6ba44d2c2dba19bb467893db9d2979796331e21bdcf6d8cd98c57e3c64f2b387 -->
<!-- api: F:Inno.Native.Sdl3.SDLCapitalization.CapitalizeNone c5e1035e9463e2eb08867adc7be44cd2348b778826fd0279ba4d79df6b445bf8 -->
<!-- api: F:Inno.Native.Sdl3.SDLCapitalization.CapitalizeSentences 58df99eb0616f7e49d6bc3fba98963fb732154f5cddb401277b5dd3f5610eefc -->
<!-- api: F:Inno.Native.Sdl3.SDLCapitalization.CapitalizeWords 77dd9eb2320125b54ca8b372b6c88772ae4291e5bb48c83a8c063de88c8baf73 -->
<!-- api: F:Inno.Native.Sdl3.SDLCapitalization.CapitalizeLetters faab8eb87bf7b8270c80390672d1107a4198d6612eb0f075bf246729ce30e689 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e8c8f26594dc9967"></a>[`Inno.Native.Sdl3.SDLCapitalization.CapitalizeLetters = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1084) | 当前常量或枚举值：`3`。 | — |
| <a id="api-d067d787ec2e008a"></a>[`Inno.Native.Sdl3.SDLCapitalization.CapitalizeNone = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1081) | 当前常量或枚举值：`0`。 | — |
| <a id="api-5761ce75ada4d637"></a>[`Inno.Native.Sdl3.SDLCapitalization.CapitalizeSentences = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1082) | 当前常量或枚举值：`1`。 | — |
| <a id="api-8801f4afa54cca49"></a>[`Inno.Native.Sdl3.SDLCapitalization.CapitalizeWords = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1083) | 当前常量或枚举值：`2`。 | — |
| <a id="api-fe42f8466fd24897"></a>[`public enum Inno.Native.Sdl3.SDLCapitalization : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1079) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLChromaLocation</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLChromaLocation 870ed8f686462e5742ca85e159c46df3abec31827a425152d12b5d6973a9e986 -->
<!-- api: F:Inno.Native.Sdl3.SDLChromaLocation.None af8f9916094b003c04fd51e796b5373d7382375beaf3877e7ed229463b11b456 -->
<!-- api: F:Inno.Native.Sdl3.SDLChromaLocation.Left e79916d76449b802c93182eeab5543ab4783d2a84573e4f4516a2f64581ff883 -->
<!-- api: F:Inno.Native.Sdl3.SDLChromaLocation.Center 667c1c2fd4eff29e7328403acd5fe780b013fa4a568d43468af29f131b9ee0b9 -->
<!-- api: F:Inno.Native.Sdl3.SDLChromaLocation.Topleft f54c69bd017c1e2adaf7cc9143b99af237b93d59a4e7de8b9c9e669428fc9fa9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2c3aedd73599e56e"></a>[`Inno.Native.Sdl3.SDLChromaLocation.Center = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L476) | 当前常量或枚举值：`2`。 | — |
| <a id="api-0670ae4462b34ced"></a>[`Inno.Native.Sdl3.SDLChromaLocation.Left = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L475) | 当前常量或枚举值：`1`。 | — |
| <a id="api-7d8146f53ead7e12"></a>[`Inno.Native.Sdl3.SDLChromaLocation.None = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L474) | 当前常量或枚举值：`0`。 | — |
| <a id="api-361502f78a7fb844"></a>[`Inno.Native.Sdl3.SDLChromaLocation.Topleft = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L477) | 当前常量或枚举值：`3`。 | — |
| <a id="api-46015dbe21d48d4c"></a>[`public enum Inno.Native.Sdl3.SDLChromaLocation : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L472) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCleanupPropertyCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCleanupPropertyCallback e93032531e82382c590e951dfa28b2957f59b86c60b552d2b1d4c080b3cb6395 -->
<!-- api: M:Inno.Native.Sdl3.SDLCleanupPropertyCallback.#ctor(System.Object,System.IntPtr) b316d2a6811660cf73586b82587d3d0bbb698f94dd72965b685bb89ca4f5f9aa -->
<!-- api: M:Inno.Native.Sdl3.SDLCleanupPropertyCallback.Invoke(System.Void*,System.Void*) 18b48b46db92fe2040f0f54be4ac6719f715f617e38c855160f55f34ab8e978b -->
<!-- api: M:Inno.Native.Sdl3.SDLCleanupPropertyCallback.BeginInvoke(System.Void*,System.Void*,System.AsyncCallback,System.Object) 65cc21b6db6428a194b7b231e1907bae62023e85dbca55fb02a15d18db169d1a -->
<!-- api: M:Inno.Native.Sdl3.SDLCleanupPropertyCallback.EndInvoke(System.IAsyncResult) cb7190b0053bb8df7139d41fed9a9fd58bdde613d4afdc401830705c8b97f4ff -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4d575794a6baf01f"></a>[`public Inno.Native.Sdl3.SDLCleanupPropertyCallback.SDLCleanupPropertyCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6805) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1d0bf331ef99924c"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLCleanupPropertyCallback.BeginInvoke(void* userdata, void* value, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6805) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-724d8cab6fedb389"></a>[`public virtual void Inno.Native.Sdl3.SDLCleanupPropertyCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6805) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-97b433c42a43aab7"></a>[`public virtual void Inno.Native.Sdl3.SDLCleanupPropertyCallback.Invoke(void* userdata, void* value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6805) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bf039bbcd3b68ef1"></a>[`public delegate void Inno.Native.Sdl3.SDLCleanupPropertyCallback(void* userdata, void* value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6805) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLClipboardCleanupCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLClipboardCleanupCallback 574ebfe0807d0eec415a2e683ff67192c95ad118de431b05b28bebd22f7366ac -->
<!-- api: M:Inno.Native.Sdl3.SDLClipboardCleanupCallback.#ctor(System.Object,System.IntPtr) 3aacd459bc5622ce2ad24b71aea5981a1cdf7949e5e2e32f25896023a6ade555 -->
<!-- api: M:Inno.Native.Sdl3.SDLClipboardCleanupCallback.Invoke(System.Void*) 1ccc6fb78a6394ec6175828ab0b4688220880b0e0bd4a2251906e4122b850841 -->
<!-- api: M:Inno.Native.Sdl3.SDLClipboardCleanupCallback.BeginInvoke(System.Void*,System.AsyncCallback,System.Object) db57e696269b910e2da39b78208c09ed2038f6f15735304f95050ab886f12d51 -->
<!-- api: M:Inno.Native.Sdl3.SDLClipboardCleanupCallback.EndInvoke(System.IAsyncResult) 319e32324bb0df1e58084c3cf79f6a1b164d06b8707630cd16933e1b64686577 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-27ab99bbbfb15a69"></a>[`public Inno.Native.Sdl3.SDLClipboardCleanupCallback.SDLClipboardCleanupCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6845) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-89c40d07aeb1042e"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLClipboardCleanupCallback.BeginInvoke(void* userdata, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6845) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b910f790badc2bed"></a>[`public virtual void Inno.Native.Sdl3.SDLClipboardCleanupCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6845) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-23591da10bd36aec"></a>[`public virtual void Inno.Native.Sdl3.SDLClipboardCleanupCallback.Invoke(void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6845) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8300903567591b16"></a>[`public delegate void Inno.Native.Sdl3.SDLClipboardCleanupCallback(void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6845) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLClipboardDataCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLClipboardDataCallback e29a2141d0e2f85e7c48e42c21348c79419673d003ee3153ec3d8a68c225c5b6 -->
<!-- api: M:Inno.Native.Sdl3.SDLClipboardDataCallback.#ctor(System.Object,System.IntPtr) 801a616d1d26136eaaf994063584718cc295d7afee6910208740ce8ab22573ca -->
<!-- api: M:Inno.Native.Sdl3.SDLClipboardDataCallback.Invoke(System.Void*,System.Byte*,System.UInt64*) 2ecf533746a440df4426ada4226b5ef646c7fec3da0fd155f14c0f8f06f88e58 -->
<!-- api: M:Inno.Native.Sdl3.SDLClipboardDataCallback.BeginInvoke(System.Void*,System.Byte*,System.UInt64*,System.AsyncCallback,System.Object) da1cb01efc36561d60d7dbc178f2e433e2d40fb99ac335a66e5609eb479bdf24 -->
<!-- api: M:Inno.Native.Sdl3.SDLClipboardDataCallback.EndInvoke(System.IAsyncResult) effb7deb810ea535534ce0e9b40c988ea7f1ef7f4d98859de795cebff74c00c0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4e63e6203ed58842"></a>[`public Inno.Native.Sdl3.SDLClipboardDataCallback.SDLClipboardDataCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6840) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-cd1a8f5d01eafae1"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLClipboardDataCallback.BeginInvoke(void* userdata, byte* mimeType, ulong* size, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6840) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eb6119d173a697b6"></a>[`public virtual void* Inno.Native.Sdl3.SDLClipboardDataCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6840) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b69a97ee04ee0b2b"></a>[`public virtual void* Inno.Native.Sdl3.SDLClipboardDataCallback.Invoke(void* userdata, byte* mimeType, ulong* size)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6840) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e9fca0a3097bf2dc"></a>[`public delegate void* Inno.Native.Sdl3.SDLClipboardDataCallback(void* userdata, byte* mimeType, ulong* size)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6840) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLClipboardEvent</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLClipboardEvent 1bc6bff83f6177929609dc1880230fda745185acb46f893e7ab4a553ae647019 -->
<!-- api: F:Inno.Native.Sdl3.SDLClipboardEvent.Type 0466781dbda61adb142b872800ab4d89b6c13c11e4bd23595bfc75d19efbe871 -->
<!-- api: F:Inno.Native.Sdl3.SDLClipboardEvent.Reserved 501fe1d698025c4bb9c879895ffabd03d156da0f5777e545352a3af5113095eb -->
<!-- api: F:Inno.Native.Sdl3.SDLClipboardEvent.Timestamp 7c3df93d98d77b141f935a8e5b02197af15c5f0c9280aa6d247f868588028622 -->
<!-- api: F:Inno.Native.Sdl3.SDLClipboardEvent.Owner 523ec9614f262e95422bb85badacc4cdd4f60ea1377b4c4f36655fe1d7108cc7 -->
<!-- api: F:Inno.Native.Sdl3.SDLClipboardEvent.NumMimeTypes a6cb0809570c1e8fd5f95c9015f9179d0db213f94d1a0bb3c989ede451e7af68 -->
<!-- api: F:Inno.Native.Sdl3.SDLClipboardEvent.MimeTypes caa097dccabcba2a860c61f82f5ae4ec8693bd2e2cbf49c3c9d705bb4395ab22 -->
<!-- api: M:Inno.Native.Sdl3.SDLClipboardEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.Byte,System.Int32,System.Byte**) eecf3302d231321badf404936808a95198e111f74df6465a733b8dc598b9db9b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d70a9e15359a778f"></a>[`public byte** Inno.Native.Sdl3.SDLClipboardEvent.MimeTypes`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3976) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f512b3d5b55f84e1"></a>[`public int Inno.Native.Sdl3.SDLClipboardEvent.NumMimeTypes`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3975) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd6016f2deba0cf3"></a>[`public byte Inno.Native.Sdl3.SDLClipboardEvent.Owner`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3974) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfc2456c773b482d"></a>[`public uint Inno.Native.Sdl3.SDLClipboardEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3972) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-63f230677cc1e2b8"></a>[`public ulong Inno.Native.Sdl3.SDLClipboardEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3973) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-edf5d4e1c6e9dbbd"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLClipboardEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3971) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f65244fd9d0a466c"></a>[`public Inno.Native.Sdl3.SDLClipboardEvent.SDLClipboardEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, byte owner = 0, int num_mime_types = 0, byte** mime_types = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3977) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b9e4928cfb1df17a"></a>[`public struct Inno.Native.Sdl3.SDLClipboardEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3969) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLColor</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLColor de94ac5444e70b474383fedc178ae7751e1ca0d45db4df419765757e31fba38f -->
<!-- api: F:Inno.Native.Sdl3.SDLColor.R 0a27a7e5b255ca5fc073210f91723a2d68d50ced4d413ed88367ca2ea9128538 -->
<!-- api: F:Inno.Native.Sdl3.SDLColor.G ae2ab8bb7866e910de2ece4354ec3edc137debdb6432c19a3db89b48f0fdfad4 -->
<!-- api: F:Inno.Native.Sdl3.SDLColor.B ba387722f9e3881f6dd620076c744dbbdc192f5dbb2e7c95dc9acedaf8614cc8 -->
<!-- api: F:Inno.Native.Sdl3.SDLColor.A 10296d270476856d5ce64a2c795732f7f5a4db8798e55ed3182ca7a1913aa511 -->
<!-- api: M:Inno.Native.Sdl3.SDLColor.#ctor(System.Byte,System.Byte,System.Byte,System.Byte) 30fd87506faa15c30b410cc87ff1ca6d94c3990c74813f4d99968fa52511820c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-238f7d57e10ecd92"></a>[`public byte Inno.Native.Sdl3.SDLColor.A`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2328) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-28ccb2488f8a7e69"></a>[`public byte Inno.Native.Sdl3.SDLColor.B`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2327) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4653823b42c3a5d"></a>[`public byte Inno.Native.Sdl3.SDLColor.G`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2326) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4fcc2ac9cd3915bb"></a>[`public byte Inno.Native.Sdl3.SDLColor.R`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2325) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fc6fcd18e8c410b8"></a>[`public Inno.Native.Sdl3.SDLColor.SDLColor(byte r = 0, byte g = 0, byte b = 0, byte a = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2329) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b9017aa12187b2ee"></a>[`public struct Inno.Native.Sdl3.SDLColor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2323) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLColorPrimaries</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLColorPrimaries 888f6ba3017ca96427f32bee3e59831e2b32662b1ffdb0293f47d690c14f7668 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.Unknown e7afb5a8f829d9c2e8d44e9780f328f28465b0f83fee393678d8e3d40b443e23 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.Bt709 4ff252e394e5eeb7b5c047cf5fd1c4c292e8f8ce4cdad916f5eb925497098003 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.Unspecified 5434e80bc0251fc52bcac06338672182d39ee9cfc22cbf4d08631f6dcdf73a52 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.Bt470M 298aaa65a6cfc24dd71ac0c2811ce7b0a1258449d6302854680995d990f04f1e -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.Bt470Bg e049d63aba1e7d7ed632893c32c9b29078f238d492edeb9b1add15e5b080959d -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.Bt601 f9b201e4e6e2a63ae5c9c7e32a4abd1cd66bfbde1b27d2d12d6d22d78c5ca4d8 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.Smpte240 63ac56be75468141b05d35e11019934819eb101f70858d53299db5b484b59cd9 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.GenericFilm c657a4ec1bdf8739967e25ca692af9d65f63ecc06658de051123d5c121b0b2c5 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.Bt2020 7887c875d8c8a805fc7ef4c26faab7769cef7de7bb06aec39ca5db71fd43e28a -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.Xyz 571086b2154ad1d09dca49de3304322611c023808da721dacc4f7b3421d4d7cc -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.Smpte431 5c995088ab73b366a1b050500fdef6dc4b4bbbfad7263517c9aa7152c48e1a31 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.Smpte432 bc2e3277345240541364d9b5ea555608f9707fe97997791b326bd5cdb5278fde -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.Ebu3213 2d2368a065b113b96afcf7600dcd6f0af133daabd71a8a4f1d3f57080d5bf1a9 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPrimaries.Custom 3726dcafe44c5c239ecb2a31b28d00962011b0ce7b3aafd88a45751156eedc68 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-72a464e56830e26d"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.Bt2020 = 9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L413) | 当前常量或枚举值：`9`。 | — |
| <a id="api-9be31637e38baead"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.Bt470Bg = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L409) | 当前常量或枚举值：`5`。 | — |
| <a id="api-0e29bbd3c6bd184e"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.Bt470M = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L408) | 当前常量或枚举值：`4`。 | — |
| <a id="api-28855bf0c98c9499"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.Bt601 = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L410) | 当前常量或枚举值：`6`。 | — |
| <a id="api-adb271e560ac4b58"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.Bt709 = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L406) | 当前常量或枚举值：`1`。 | — |
| <a id="api-8a0fd0b27987ee76"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.Custom = 31`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L418) | 当前常量或枚举值：`31`。 | — |
| <a id="api-d700b57dcfbab2d9"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.Ebu3213 = 22`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L417) | 当前常量或枚举值：`22`。 | — |
| <a id="api-cb64a7bcc0afc9dc"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.GenericFilm = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L412) | 当前常量或枚举值：`8`。 | — |
| <a id="api-e84ca87434b7c935"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.Smpte240 = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L411) | 当前常量或枚举值：`7`。 | — |
| <a id="api-a5c9a8d8458f8b1c"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.Smpte431 = 11`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L415) | 当前常量或枚举值：`11`。 | — |
| <a id="api-e8fc56ce4aff9dfb"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.Smpte432 = 12`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L416) | 当前常量或枚举值：`12`。 | — |
| <a id="api-2a03f286b0aa103b"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.Unknown = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L405) | 当前常量或枚举值：`0`。 | — |
| <a id="api-e7ba0de59df6c7cb"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.Unspecified = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L407) | 当前常量或枚举值：`2`。 | — |
| <a id="api-abb8e17c9871a7e7"></a>[`Inno.Native.Sdl3.SDLColorPrimaries.Xyz = 10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L414) | 当前常量或枚举值：`10`。 | — |
| <a id="api-5deaaf245ff5af2e"></a>[`public enum Inno.Native.Sdl3.SDLColorPrimaries : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L403) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLColorPtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLColorPtr 0d0e7b6c67ff3296f8b8e1e29153eeabd109fc43490306525dc2d3d25dff3acc -->
<!-- api: M:Inno.Native.Sdl3.SDLColorPtr.#ctor(Inno.Native.Sdl3.SDLColor*) fc89f155441cb47bf0903f736324e707d32d585e0743a8c1dca3ea0680d98499 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorPtr.Handle a500a5208429279d02d75e96c1610c04ea8d0d69b573929aec606836fb2a10b8 -->
<!-- api: P:Inno.Native.Sdl3.SDLColorPtr.IsNull 38e6ce02c4fa6219a70520a7598dc02f1b25358ac524051e81d19dbd4597b5e3 -->
<!-- api: P:Inno.Native.Sdl3.SDLColorPtr.Null 896733688a0465bd2acc62f532e23233c92cf54b0cb78560cd40f97c22701960 -->
<!-- api: P:Inno.Native.Sdl3.SDLColorPtr.Item(System.Int32) 756fc0c6d05304ad6bb58af391f85baf4af5bbb067b59b0fd854dcd304264a0d -->
<!-- api: M:Inno.Native.Sdl3.SDLColorPtr.op_Implicit(Inno.Native.Sdl3.SDLColor*)~Inno.Native.Sdl3.SDLColorPtr f501bda4648782c2257791d45ed4b47d2df5336943fe8d1e03a090b354f52f35 -->
<!-- api: M:Inno.Native.Sdl3.SDLColorPtr.op_Implicit(Inno.Native.Sdl3.SDLColorPtr)~Inno.Native.Sdl3.SDLColor* ac0f2e8df2158b82d8be8d8362581e44e1d8a17acd3c18a4c90558c89625a2ae -->
<!-- api: M:Inno.Native.Sdl3.SDLColorPtr.op_Equality(Inno.Native.Sdl3.SDLColorPtr,Inno.Native.Sdl3.SDLColorPtr) 97b24e4c0b5915edc9dc11a9c40fd487b37b733de6db80488e98b9896d70fbd5 -->
<!-- api: M:Inno.Native.Sdl3.SDLColorPtr.op_Inequality(Inno.Native.Sdl3.SDLColorPtr,Inno.Native.Sdl3.SDLColorPtr) 40c8b50df3bb1598b1587a779cd0d7cbd1f66382329d92c34e6fb8f64e068800 -->
<!-- api: M:Inno.Native.Sdl3.SDLColorPtr.Equals(Inno.Native.Sdl3.SDLColorPtr) 7ed934800c7f02abb919b82e133add4fab0feb30814b6109e9a076f9354d3e2c -->
<!-- api: M:Inno.Native.Sdl3.SDLColorPtr.Equals(System.Object) 25f7f72d1a8ad0a1a9dd8fe269dc090892a7c4331a8b2d855efcdf5a6ced4b36 -->
<!-- api: M:Inno.Native.Sdl3.SDLColorPtr.GetHashCode 17a3335a1f54452bec004a28f35c7abc603d61c9829fe620b994c9d92d33fa76 -->
<!-- api: P:Inno.Native.Sdl3.SDLColorPtr.R 0a37ee14fdaafc5c16bee553aa75064a57f9953886146804e8e3061562b0a25b -->
<!-- api: P:Inno.Native.Sdl3.SDLColorPtr.G 0fba6714cee5ba5753412b53a53f2b0a49e1429422139429638b48d66321bfd4 -->
<!-- api: P:Inno.Native.Sdl3.SDLColorPtr.B 3c9544c23b8739105b3aa11776ac0601efdd2ce668f2d04442cf6e7540405f26 -->
<!-- api: P:Inno.Native.Sdl3.SDLColorPtr.A 013732f3fa31ad0fb63b2faa70543f1f1c90559206417b2c5dedbdd2fbe4b75f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7a29a122d5be7468"></a>[`public Inno.Native.Sdl3.SDLColor* Inno.Native.Sdl3.SDLColorPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8092) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b0d8e754a3865895"></a>[`public Inno.Native.Sdl3.SDLColorPtr.SDLColorPtr(Inno.Native.Sdl3.SDLColor* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8087) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-66fb79c78cb49296"></a>[`public bool Inno.Native.Sdl3.SDLColorPtr.Equals(Inno.Native.Sdl3.SDLColorPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8102) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-3f79b13fdf33c0dc"></a>[`public override bool Inno.Native.Sdl3.SDLColorPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8103) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-b0a3a2a089509fc4"></a>[`public override int Inno.Native.Sdl3.SDLColorPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8104) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-2fd579c6cfe9f734"></a>[`public static bool Inno.Native.Sdl3.SDLColorPtr.operator ==(Inno.Native.Sdl3.SDLColorPtr left, Inno.Native.Sdl3.SDLColorPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8100) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e6e0ba99790359ad"></a>[`public static Inno.Native.Sdl3.SDLColorPtr.implicit operator Inno.Native.Sdl3.SDLColorPtr(Inno.Native.Sdl3.SDLColor* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8098) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ee0c67fc64cb6db1"></a>[`public static Inno.Native.Sdl3.SDLColorPtr.implicit operator Inno.Native.Sdl3.SDLColor*(Inno.Native.Sdl3.SDLColorPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8099) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7b4b007b54c10c78"></a>[`public static bool Inno.Native.Sdl3.SDLColorPtr.operator !=(Inno.Native.Sdl3.SDLColorPtr left, Inno.Native.Sdl3.SDLColorPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8101) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b31f7d38a81a81e3"></a>[`public ref byte Inno.Native.Sdl3.SDLColorPtr.A { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8109) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7a0434ef62505984"></a>[`public ref byte Inno.Native.Sdl3.SDLColorPtr.B { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8108) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0989dbd786311a04"></a>[`public ref byte Inno.Native.Sdl3.SDLColorPtr.G { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8107) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f64e3209a3a33c01"></a>[`public bool Inno.Native.Sdl3.SDLColorPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8093) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-26df266ade020773"></a>[`public Inno.Native.Sdl3.SDLColor Inno.Native.Sdl3.SDLColorPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8096) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-91cb568a9ef9e734"></a>[`public static Inno.Native.Sdl3.SDLColorPtr Inno.Native.Sdl3.SDLColorPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8094) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8a307b55d8f3bb7a"></a>[`public ref byte Inno.Native.Sdl3.SDLColorPtr.R { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8106) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7b0c800e94221492"></a>[`public struct Inno.Native.Sdl3.SDLColorPtr : System.IEquatable<Inno.Native.Sdl3.SDLColorPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8085) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLColorRange</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLColorRange 6c796f1d62e685e72a6c2a648ce19d4d96cbdbe057f259837c240bd46d3fd320 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorRange.Unknown 5ec310415e89776ebb3ee543ce73f6e76ae3155df13e92c31cb2d460f3b0818a -->
<!-- api: F:Inno.Native.Sdl3.SDLColorRange.Limited 192c616b16b9087bad0a9ef240c061bebe6a636d6e8479d3ed262fb2ec75c171 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorRange.Full c27bb2d47eece57e576f57b3959d55ab78181ac6078aecf264cce751415b8e8c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-96410c9401731661"></a>[`Inno.Native.Sdl3.SDLColorRange.Full = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L397) | 当前常量或枚举值：`2`。 | — |
| <a id="api-59e8b60665ff19e5"></a>[`Inno.Native.Sdl3.SDLColorRange.Limited = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L396) | 当前常量或枚举值：`1`。 | — |
| <a id="api-3d4f3560136ed55b"></a>[`Inno.Native.Sdl3.SDLColorRange.Unknown = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L395) | 当前常量或枚举值：`0`。 | — |
| <a id="api-b6ee84932a8fbe22"></a>[`public enum Inno.Native.Sdl3.SDLColorRange : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L393) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLColorspace</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLColorspace 3316eacfc0f274a52a4d65855b00b7669e762fa2529ac1f2ccb9c18128fabcd0 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorspace.Unknown c49f14c1e006f827eb58fe6256abcee84d4b46c91334356a0ea7c986f62c6194 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorspace.Srgb 5f91d9878def52d0c606ef5dcbef7d08087407a2f6963c7b726ae03e671bdff2 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorspace.SrgbLinear fa20e6a59aa421594009ddccd8ab86bb843de7a90c16f55c71e11b70967537c1 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorspace.Hdr10 632f82044327a6a05356699aaa2835a8f547d072cb4f5ef265e51790df5a9580 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorspace.Jpeg 8965aef80f3e96e413031652afa4fe5775772cce09dbf0d9aa0ffb35f3cc0028 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorspace.Bt601Limited c1d8b874a0989087447de0c74141986d24cb2e865e6c755cd8247e27921345f3 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorspace.Bt601Full 4c67fccd460aa46c7376818fd2ae88e1ce6711963efd379fa85e7e1ff0f2dd52 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorspace.Bt709Limited 3b8e4dbc2df613a7ec5847a62158042fd0032f375bfb0bde5047b9e518cdbd9a -->
<!-- api: F:Inno.Native.Sdl3.SDLColorspace.Bt709Full 16972ab42ba1a83481160e03aa19bbef4e3e99cc20c207a7a75fac371df61601 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorspace.Bt2020Limited 17b72a8e26ff76cec49ab91d8a99c0d9245acd45a7e15f26cbfc848a1b07c77a -->
<!-- api: F:Inno.Native.Sdl3.SDLColorspace.Bt2020Full 9403c407d5a0d3c15cd7f847a4173538fc4548a9acd06c42c2b0f41a3eb95fe7 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorspace.RgbDefault 053bf086fadce6cf256424e7e5f1a79844b1fc520f07c9640366af01a429e39b -->
<!-- api: F:Inno.Native.Sdl3.SDLColorspace.YuvDefault 2813a24bb1e92384de8585f77b72059ac14af96a92938a829e4b26327dcee0e6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b768a7bf9189a011"></a>[`Inno.Native.Sdl3.SDLColorspace.Bt2020Full = 571483657`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L495) | 当前常量或枚举值：`571483657`。 | — |
| <a id="api-1e4aa5ace38d81cc"></a>[`Inno.Native.Sdl3.SDLColorspace.Bt2020Limited = 554706441`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L494) | 当前常量或枚举值：`554706441`。 | — |
| <a id="api-9f776a17d8e43ce6"></a>[`Inno.Native.Sdl3.SDLColorspace.Bt601Full = 571480262`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L491) | 当前常量或枚举值：`571480262`。 | — |
| <a id="api-46f07782c7135fd3"></a>[`Inno.Native.Sdl3.SDLColorspace.Bt601Limited = 554703046`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L490) | 当前常量或枚举值：`554703046`。 | — |
| <a id="api-b0beb2b2b767cb64"></a>[`Inno.Native.Sdl3.SDLColorspace.Bt709Full = 571474977`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L493) | 当前常量或枚举值：`571474977`。 | — |
| <a id="api-29fc5e333f4d9bd7"></a>[`Inno.Native.Sdl3.SDLColorspace.Bt709Limited = 554697761`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L492) | 当前常量或枚举值：`554697761`。 | — |
| <a id="api-be579973c1808bec"></a>[`Inno.Native.Sdl3.SDLColorspace.Hdr10 = 301999616`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L488) | 当前常量或枚举值：`301999616`。 | — |
| <a id="api-b71ed17ce861bcf0"></a>[`Inno.Native.Sdl3.SDLColorspace.Jpeg = 570426566`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L489) | 当前常量或枚举值：`570426566`。 | — |
| <a id="api-3bed0c5029ee069e"></a>[`Inno.Native.Sdl3.SDLColorspace.RgbDefault = 301991328`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L496) | 当前常量或枚举值：`301991328`。 | — |
| <a id="api-263502c9d626c0ce"></a>[`Inno.Native.Sdl3.SDLColorspace.Srgb = 301991328`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L486) | 当前常量或枚举值：`301991328`。 | — |
| <a id="api-b76b2d1d86f01a4b"></a>[`Inno.Native.Sdl3.SDLColorspace.SrgbLinear = 301991168`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L487) | 当前常量或枚举值：`301991168`。 | — |
| <a id="api-334dfcb1a115d92d"></a>[`Inno.Native.Sdl3.SDLColorspace.Unknown = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L485) | 当前常量或枚举值：`0`。 | — |
| <a id="api-28a55e33a9c81264"></a>[`Inno.Native.Sdl3.SDLColorspace.YuvDefault = 554703046`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L497) | 当前常量或枚举值：`554703046`。 | — |
| <a id="api-ac6dace958220027"></a>[`public enum Inno.Native.Sdl3.SDLColorspace : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L483) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLColorType</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLColorType 3a2ceb8e70e5d69d525fa4d3665cb57977ae3b2bcfb3dd1adaac61b9d1a3547e -->
<!-- api: F:Inno.Native.Sdl3.SDLColorType.Unknown d093f43fa902be77e695bfcf1f9b54275b6bf40b0aff5e1c4c127e77d6f351d8 -->
<!-- api: F:Inno.Native.Sdl3.SDLColorType.Rgb 421de01413efc5d2625e0d2922621f2f796faa19e14cec0f850625c06d144f2e -->
<!-- api: F:Inno.Native.Sdl3.SDLColorType.Ycbcr 2c98b25264f07d9322aca9ccbe7e70bbb9e09de01f4f28df9ca166908234ed40 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-362d17d3ad40314a"></a>[`Inno.Native.Sdl3.SDLColorType.Rgb = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L386) | 当前常量或枚举值：`1`。 | — |
| <a id="api-834f153f2a72cc43"></a>[`Inno.Native.Sdl3.SDLColorType.Unknown = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L385) | 当前常量或枚举值：`0`。 | — |
| <a id="api-40c915899d9972f7"></a>[`Inno.Native.Sdl3.SDLColorType.Ycbcr = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L387) | 当前常量或枚举值：`2`。 | — |
| <a id="api-0fa6c2ce00e35c03"></a>[`public enum Inno.Native.Sdl3.SDLColorType : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L383) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCommonEvent</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCommonEvent 1ba5e02e6a09d4ef1a58e0751de46842a48ede702dc8e6c37b534ffaf89260aa -->
<!-- api: F:Inno.Native.Sdl3.SDLCommonEvent.Type ed28981ede7715cf2e12b456d9b40cdd2f8cb2472eba67c1fa4d2b30fd808cb3 -->
<!-- api: F:Inno.Native.Sdl3.SDLCommonEvent.Reserved 701e82720b57e40d88017a30581f5e860c9f0b7524910ad0a95cd7b2a799c619 -->
<!-- api: F:Inno.Native.Sdl3.SDLCommonEvent.Timestamp 63895b04e65772e943a7499daeb7d9b92a1af88be71bdbe205e689fd51787c78 -->
<!-- api: M:Inno.Native.Sdl3.SDLCommonEvent.#ctor(System.UInt32,System.UInt32,System.UInt64) 1b73afd8a9d0898559c4a87381f6a14c2ce9cbf6e1cab96dd82621d004ba10e6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2a4cb8d365d18df1"></a>[`public uint Inno.Native.Sdl3.SDLCommonEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3074) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-83487a719dd2fca4"></a>[`public ulong Inno.Native.Sdl3.SDLCommonEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3075) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8912f1ce82ead3de"></a>[`public uint Inno.Native.Sdl3.SDLCommonEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3073) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f812d5e2959f516f"></a>[`public Inno.Native.Sdl3.SDLCommonEvent.SDLCommonEvent(uint type = 0, uint reserved = 0, ulong timestamp = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3076) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7137f307871e982b"></a>[`public struct Inno.Native.Sdl3.SDLCommonEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3071) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCompareCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCompareCallback 9fac44c696960039a36989a41ade4c93e564ad75a02526d11f3eb5a65142cbfa -->
<!-- api: M:Inno.Native.Sdl3.SDLCompareCallback.#ctor(System.Object,System.IntPtr) 2af553124601ea6d0647c0b77703854fa862b6f9b99f05388991fc0afb0dbac9 -->
<!-- api: M:Inno.Native.Sdl3.SDLCompareCallback.Invoke(System.Void*,System.Void*) ddb587dc755205e957930171acccc27bb89244fcd85e03bcf8396386a2123d97 -->
<!-- api: M:Inno.Native.Sdl3.SDLCompareCallback.BeginInvoke(System.Void*,System.Void*,System.AsyncCallback,System.Object) b97877fe7fc1b7d2c12007808f2ca0fce2a2e5aaece09183584530acbe1ac505 -->
<!-- api: M:Inno.Native.Sdl3.SDLCompareCallback.EndInvoke(System.IAsyncResult) d5726eea256806d751dae303015916a2560e2c9f9a1415d744b39557e092110c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6d819673ce7cbfbf"></a>[`public Inno.Native.Sdl3.SDLCompareCallback.SDLCompareCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6785) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-479f57caf27501a1"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLCompareCallback.BeginInvoke(void* a, void* b, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6785) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-60d47fdf91e521ce"></a>[`public virtual int Inno.Native.Sdl3.SDLCompareCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6785) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8146fc3cc26d4e64"></a>[`public virtual int Inno.Native.Sdl3.SDLCompareCallback.Invoke(void* a, void* b)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6785) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8d755c31a9085aee"></a>[`public delegate int Inno.Native.Sdl3.SDLCompareCallback(void* a, void* b)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6785) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCompareCallbackR</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCompareCallbackR ec4f9c05efcbf7c01fb326a880483f528fa13e65af9f766b3e25c3661a089b69 -->
<!-- api: M:Inno.Native.Sdl3.SDLCompareCallbackR.#ctor(System.Object,System.IntPtr) e6201a74705b38d89683113dcb34a2e3dfda1cfc1ed3847f87860007d60afaae -->
<!-- api: M:Inno.Native.Sdl3.SDLCompareCallbackR.Invoke(System.Void*,System.Void*,System.Void*) d06522b32ef81304c4b4a47bfd1a008fad8ec1c037c3af0dd2062f56c5dda373 -->
<!-- api: M:Inno.Native.Sdl3.SDLCompareCallbackR.BeginInvoke(System.Void*,System.Void*,System.Void*,System.AsyncCallback,System.Object) f7065c1012e659678eb9ceca5830a574dbf301be96da09ace0e94b953c4471ea -->
<!-- api: M:Inno.Native.Sdl3.SDLCompareCallbackR.EndInvoke(System.IAsyncResult) 82e48c9e800ad4a73f8f2c6967d42d31a2083c1c830ced2a13f03de12532649f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-41e70773ad3c5328"></a>[`public Inno.Native.Sdl3.SDLCompareCallbackR.SDLCompareCallbackR(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6790) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a283aa507f22199e"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLCompareCallbackR.BeginInvoke(void* userdata, void* a, void* b, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6790) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-17e9dcd057652770"></a>[`public virtual int Inno.Native.Sdl3.SDLCompareCallbackR.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6790) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d1025b57a49c7773"></a>[`public virtual int Inno.Native.Sdl3.SDLCompareCallbackR.Invoke(void* userdata, void* a, void* b)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6790) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-65f7d8c796a13be0"></a>[`public delegate int Inno.Native.Sdl3.SDLCompareCallbackR(void* userdata, void* a, void* b)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6790) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCondition</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCondition f8913137b2840ccb731f2839b8874d6d146207bc3c4d8681596c9912fce64859 -->
<!-- api: M:Inno.Native.Sdl3.SDLCondition.#ctor(System.IntPtr) 3e698ac407c1b52acccc9e4d4bff5cdefede11aec9ba6a92bd18d833e6be8d05 -->
<!-- api: P:Inno.Native.Sdl3.SDLCondition.Handle 42d89fbdfd03288eac73cd9ca9d08fc17b3614e17c479d30c1ad4f94fa12894d -->
<!-- api: P:Inno.Native.Sdl3.SDLCondition.IsNull 79ad806206ebac75e2a6a4994738d944c5f09afa1ecd9713c601534ff2b18ffd -->
<!-- api: P:Inno.Native.Sdl3.SDLCondition.Null c45b4ed39934b660e07dd4b9b5a5509753778e9bb4d35317b32fe5f70c3fb00a -->
<!-- api: M:Inno.Native.Sdl3.SDLCondition.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLCondition 64eb8e5a8fcf80ba54857953a54602328ac8f12ed1e4fd9c3048d03e2524f08c -->
<!-- api: M:Inno.Native.Sdl3.SDLCondition.op_Equality(Inno.Native.Sdl3.SDLCondition,Inno.Native.Sdl3.SDLCondition) 911c22b8e52a8b34b4060a6e0830deb4ba1ef77868df54b7dc3034009a6060bf -->
<!-- api: M:Inno.Native.Sdl3.SDLCondition.op_Inequality(Inno.Native.Sdl3.SDLCondition,Inno.Native.Sdl3.SDLCondition) b7356d7b5b0895a8cd2843bf3f6716984ea4b2fab269a9df13e6e9ac71ef3fdc -->
<!-- api: M:Inno.Native.Sdl3.SDLCondition.Equals(Inno.Native.Sdl3.SDLCondition) 1898627b0584979572cd9bb47829ca9a53d113ed51d0b2eca582063dbb297a20 -->
<!-- api: M:Inno.Native.Sdl3.SDLCondition.Equals(System.Object) 9aba492e80650e9120808ac57ef1574ba9326c7b1a74e0211f3f92b8a3095ba2 -->
<!-- api: M:Inno.Native.Sdl3.SDLCondition.GetHashCode 52a293dcd7c3f6a0d097e278bda2a6172d3eede5ff0ffdc818c4d5c67b12a0f5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ebb25cc8f95a45f1"></a>[`public Inno.Native.Sdl3.SDLCondition.SDLCondition(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2194) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8de1abd723b5b186"></a>[`public bool Inno.Native.Sdl3.SDLCondition.Equals(Inno.Native.Sdl3.SDLCondition other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2206) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-d11b297ef490ebcb"></a>[`public override bool Inno.Native.Sdl3.SDLCondition.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2207) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-8f2be492fbcb58ad"></a>[`public override int Inno.Native.Sdl3.SDLCondition.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2208) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-59766bfadbe5b01a"></a>[`public static bool Inno.Native.Sdl3.SDLCondition.operator ==(Inno.Native.Sdl3.SDLCondition left, Inno.Native.Sdl3.SDLCondition right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2204) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d6f05b7667379aa4"></a>[`public static Inno.Native.Sdl3.SDLCondition.implicit operator Inno.Native.Sdl3.SDLCondition(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2203) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3dc58178d808f00f"></a>[`public static bool Inno.Native.Sdl3.SDLCondition.operator !=(Inno.Native.Sdl3.SDLCondition left, Inno.Native.Sdl3.SDLCondition right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2205) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-664d043b65955f8e"></a>[`public nint Inno.Native.Sdl3.SDLCondition.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2199) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0f093fdb5a7f4f38"></a>[`public bool Inno.Native.Sdl3.SDLCondition.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2200) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-52fa540bbd3a5491"></a>[`public static Inno.Native.Sdl3.SDLCondition Inno.Native.Sdl3.SDLCondition.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2201) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba2fff48c9179c64"></a>[`public readonly struct Inno.Native.Sdl3.SDLCondition : System.IEquatable<Inno.Native.Sdl3.SDLCondition>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2192) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCursor</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCursor ef5679608fab47a036a4ea83bf92e3c486f3d5f462df79071800e59348349339 -->
<!-- api: M:Inno.Native.Sdl3.SDLCursor.#ctor(System.IntPtr) 11817054f69f9085c805a1a3664983b0e665ffc52941b73229b744f2cf5872a1 -->
<!-- api: P:Inno.Native.Sdl3.SDLCursor.Handle dc3afafae802eb4d85a314d11762954923ac4e95130bdb94b1b8bf07e61022b0 -->
<!-- api: P:Inno.Native.Sdl3.SDLCursor.IsNull 8a9b85c41e32dfb75f43c5ae2b828da3a88640b2ae4a9940ad30339ab43da89e -->
<!-- api: P:Inno.Native.Sdl3.SDLCursor.Null d18e0c7ac1b4196e7f991b7749c9199dd5e3da75fd786fb4eeab2317bad176c6 -->
<!-- api: M:Inno.Native.Sdl3.SDLCursor.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLCursor e22c6e22e11ad4b84f8e04146431d855a7b24a571ca2934e380fb4675c042d2b -->
<!-- api: M:Inno.Native.Sdl3.SDLCursor.op_Equality(Inno.Native.Sdl3.SDLCursor,Inno.Native.Sdl3.SDLCursor) f15fce8641f7fb8637e62ceb92cede9c9a884cc70f3b6ea34f9aca1c6a1191c0 -->
<!-- api: M:Inno.Native.Sdl3.SDLCursor.op_Inequality(Inno.Native.Sdl3.SDLCursor,Inno.Native.Sdl3.SDLCursor) f315cb856df2b1cd2b62b6e53c9ffd0a3abe94417a474e72d604e47281b5d2d3 -->
<!-- api: M:Inno.Native.Sdl3.SDLCursor.Equals(Inno.Native.Sdl3.SDLCursor) f7b0bf07c08b47e7ce96ce789a87d80c1bca1615b1667c9178c01bc7965cd4b8 -->
<!-- api: M:Inno.Native.Sdl3.SDLCursor.Equals(System.Object) 7ebdeb0e3531eb3fd30ab2167cdc5c315ad78a64853594cb911ce33f216a71fc -->
<!-- api: M:Inno.Native.Sdl3.SDLCursor.GetHashCode 3a107a68a6042ec45912705b261541e143ced22893e17901eb0dc8ff4562f79b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4c062c4df1d97631"></a>[`public Inno.Native.Sdl3.SDLCursor.SDLCursor(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3015) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1293e0b72be7269e"></a>[`public bool Inno.Native.Sdl3.SDLCursor.Equals(Inno.Native.Sdl3.SDLCursor other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3027) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-ac4a3368f84848e2"></a>[`public override bool Inno.Native.Sdl3.SDLCursor.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3028) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-d9c41e66f98a1a7f"></a>[`public override int Inno.Native.Sdl3.SDLCursor.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3029) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-0a9b206a091d78b7"></a>[`public static bool Inno.Native.Sdl3.SDLCursor.operator ==(Inno.Native.Sdl3.SDLCursor left, Inno.Native.Sdl3.SDLCursor right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3025) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d07811712129d6da"></a>[`public static Inno.Native.Sdl3.SDLCursor.implicit operator Inno.Native.Sdl3.SDLCursor(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3024) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0c4f2209a573a996"></a>[`public static bool Inno.Native.Sdl3.SDLCursor.operator !=(Inno.Native.Sdl3.SDLCursor left, Inno.Native.Sdl3.SDLCursor right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3026) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c09cc82263bd0b97"></a>[`public nint Inno.Native.Sdl3.SDLCursor.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3020) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5c7ea4f77beb61a8"></a>[`public bool Inno.Native.Sdl3.SDLCursor.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3021) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c76b505d6a6b6bbb"></a>[`public static Inno.Native.Sdl3.SDLCursor Inno.Native.Sdl3.SDLCursor.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3022) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d5307815999c4a9a"></a>[`public readonly struct Inno.Native.Sdl3.SDLCursor : System.IEquatable<Inno.Native.Sdl3.SDLCursor>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3013) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCursorFrameInfo</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCursorFrameInfo cc603f283b6217f2edfe8374bffe1808454851cd071f246dc81fa6eb59688585 -->
<!-- api: F:Inno.Native.Sdl3.SDLCursorFrameInfo.Surface 0feaf8b6866ef7a07c8c18e076b4137b6a272e1776f0e941b8d5bdb6f6321413 -->
<!-- api: F:Inno.Native.Sdl3.SDLCursorFrameInfo.Duration 80b2b52678413993995f4e2ec596fe488bea867cce3d11dffa58ad9f55729d42 -->
<!-- api: M:Inno.Native.Sdl3.SDLCursorFrameInfo.#ctor(Inno.Native.Sdl3.SDLSurface*,System.UInt32) 8288ab2c082392d86add1266a5be3cd1ff7d383271b0ba19b8fff90592ff247a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a621e80b150258a7"></a>[`public uint Inno.Native.Sdl3.SDLCursorFrameInfo.Duration`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3040) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4c11e7b98ce488c1"></a>[`public Inno.Native.Sdl3.SDLSurface* Inno.Native.Sdl3.SDLCursorFrameInfo.Surface`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3039) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e49375355a2fb7a7"></a>[`public Inno.Native.Sdl3.SDLCursorFrameInfo.SDLCursorFrameInfo(Inno.Native.Sdl3.SDLSurface* surface = null, uint duration = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3041) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-41142f926e229646"></a>[`public struct Inno.Native.Sdl3.SDLCursorFrameInfo`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3037) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLCursorFrameInfoPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLCursorFrameInfoPtr 47d156f9ac671d4575a0a5391cf3a6125849d6b87a3af52d9f0226f09e86a145 -->
<!-- api: M:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.#ctor(Inno.Native.Sdl3.SDLCursorFrameInfo*) 1825cdf77fac154c5a11b0fb3386fb362c867b09bcdab2d8a708f6d9daa88c50 -->
<!-- api: F:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.Handle 7c09a17fa62d3cfa77cdc26d95c480c5ce08418fa94770db32ab97a2bb81e4fe -->
<!-- api: P:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.IsNull 8adc883979c5bce0824caf90b25f39331d4bdb25c7c8cc522bd08e7d51c46767 -->
<!-- api: P:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.Null 27fa4806373e62455e7e5720c8cfbce2c8718ad495f98f882d4f04cc0344deb7 -->
<!-- api: P:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.Item(System.Int32) 0e614245cc0a7f15cf8ee84bea0d9814f6e44411a4ca752d2fdec8cbc34608bc -->
<!-- api: M:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLCursorFrameInfo*)~Inno.Native.Sdl3.SDLCursorFrameInfoPtr 38932f47a8bd4a99fa389157f3ad0fa166263ac5cfbb53ec19b379176ba62799 -->
<!-- api: M:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLCursorFrameInfoPtr)~Inno.Native.Sdl3.SDLCursorFrameInfo* ce8a208a80b5349c94c3c93e2db337e710b323923eae8f783a6da7e694b99ef3 -->
<!-- api: M:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.op_Equality(Inno.Native.Sdl3.SDLCursorFrameInfoPtr,Inno.Native.Sdl3.SDLCursorFrameInfoPtr) 962b510daa3c23b29b2abeb065e65f54679d372c3ff9a9f1d1b67dd770571f7c -->
<!-- api: M:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.op_Inequality(Inno.Native.Sdl3.SDLCursorFrameInfoPtr,Inno.Native.Sdl3.SDLCursorFrameInfoPtr) 4bc19310274f328d9704410deaec9f5198860a368d456e208a6761954a4e1dd6 -->
<!-- api: M:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.Equals(Inno.Native.Sdl3.SDLCursorFrameInfoPtr) 1a4175546e185c1085e736214d38f87ef7d3aadc8496d903418306fc1100c0d7 -->
<!-- api: M:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.Equals(System.Object) 557aa4f4df0056fb991d18adf0ed40641b3f41305da997ae136dcd7fd2d3a50c -->
<!-- api: M:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.GetHashCode 8227afc254dc16d948ec3bddb3232d412deb51df493daa7dcc70cacab0ecae6b -->
<!-- api: P:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.Surface 1cd41459d0b1bf69f5698e3a2a77c3855dc95ef7abbf8729f73185b9bc1b3dd9 -->
<!-- api: P:Inno.Native.Sdl3.SDLCursorFrameInfoPtr.Duration f8db8a633314d27fffebf7e9a8c887dc96ef2c5360aa34e3ce575cebeb671f8c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-aaadac37bbf4d8cf"></a>[`public Inno.Native.Sdl3.SDLCursorFrameInfo* Inno.Native.Sdl3.SDLCursorFrameInfoPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8120) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-068f506a58615cdf"></a>[`public Inno.Native.Sdl3.SDLCursorFrameInfoPtr.SDLCursorFrameInfoPtr(Inno.Native.Sdl3.SDLCursorFrameInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8115) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-c2331994a51f1fab"></a>[`public bool Inno.Native.Sdl3.SDLCursorFrameInfoPtr.Equals(Inno.Native.Sdl3.SDLCursorFrameInfoPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8130) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-79eae8f299bdda09"></a>[`public override bool Inno.Native.Sdl3.SDLCursorFrameInfoPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8131) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-147da1f517e4039c"></a>[`public override int Inno.Native.Sdl3.SDLCursorFrameInfoPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8132) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-c7ffb545bb9e37ff"></a>[`public static bool Inno.Native.Sdl3.SDLCursorFrameInfoPtr.operator ==(Inno.Native.Sdl3.SDLCursorFrameInfoPtr left, Inno.Native.Sdl3.SDLCursorFrameInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8128) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8354dc73b92fc63b"></a>[`public static Inno.Native.Sdl3.SDLCursorFrameInfoPtr.implicit operator Inno.Native.Sdl3.SDLCursorFrameInfoPtr(Inno.Native.Sdl3.SDLCursorFrameInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8126) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eed1310221844a38"></a>[`public static Inno.Native.Sdl3.SDLCursorFrameInfoPtr.implicit operator Inno.Native.Sdl3.SDLCursorFrameInfo*(Inno.Native.Sdl3.SDLCursorFrameInfoPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8127) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a160df59302ee5e4"></a>[`public static bool Inno.Native.Sdl3.SDLCursorFrameInfoPtr.operator !=(Inno.Native.Sdl3.SDLCursorFrameInfoPtr left, Inno.Native.Sdl3.SDLCursorFrameInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8129) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a5fd03aa82ce176f"></a>[`public ref uint Inno.Native.Sdl3.SDLCursorFrameInfoPtr.Duration { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8135) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-56fbc282fc708bc9"></a>[`public bool Inno.Native.Sdl3.SDLCursorFrameInfoPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8121) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9431c969534c8316"></a>[`public Inno.Native.Sdl3.SDLCursorFrameInfo Inno.Native.Sdl3.SDLCursorFrameInfoPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8124) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eef12c7b17fee00c"></a>[`public static Inno.Native.Sdl3.SDLCursorFrameInfoPtr Inno.Native.Sdl3.SDLCursorFrameInfoPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8122) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ba1dcdbf5450078"></a>[`public ref Inno.Native.Sdl3.SDLSurfacePtr Inno.Native.Sdl3.SDLCursorFrameInfoPtr.Surface { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8134) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-519146b4a6a7ea4d"></a>[`public struct Inno.Native.Sdl3.SDLCursorFrameInfoPtr : System.IEquatable<Inno.Native.Sdl3.SDLCursorFrameInfoPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8113) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLDateFormat</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLDateFormat c7eb71b5ad6f0b41c361b4ad0e269e18b8bc7463c911529372d2d0f0501c9221 -->
<!-- api: F:Inno.Native.Sdl3.SDLDateFormat.Yyyymmdd 777c78f044323205a3b7f6789aaf431584a5e5d544ed90d74616df571cdaf9cc -->
<!-- api: F:Inno.Native.Sdl3.SDLDateFormat.Ddmmyyyy c054805f0bbb62531dfc15203403422c9956ad357327a0cf3c4c18afcae5be95 -->
<!-- api: F:Inno.Native.Sdl3.SDLDateFormat.Mmddyyyy 795986873ce3b6c78d52900a62cb24fe5038d9e733502dd013d5671e321c77ce -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0ac7f2fd21c28126"></a>[`Inno.Native.Sdl3.SDLDateFormat.Ddmmyyyy = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1890) | 当前常量或枚举值：`1`。 | — |
| <a id="api-aaf9f1bc4538e4f4"></a>[`Inno.Native.Sdl3.SDLDateFormat.Mmddyyyy = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1891) | 当前常量或枚举值：`2`。 | — |
| <a id="api-975a57d0af58e0ac"></a>[`Inno.Native.Sdl3.SDLDateFormat.Yyyymmdd = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1889) | 当前常量或枚举值：`0`。 | — |
| <a id="api-fb3991c6f7decc33"></a>[`public enum Inno.Native.Sdl3.SDLDateFormat : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1887) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLDateTime</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLDateTime 882f65df462bd27a276aadd7328cae1a2962396c59fd049dcc48b136abe86377 -->
<!-- api: F:Inno.Native.Sdl3.SDLDateTime.Year e033fe256c750f5330afbdf89e7f9a2cb28e34a22d157bf88e0bd6ff81b815de -->
<!-- api: F:Inno.Native.Sdl3.SDLDateTime.Month 5200cfe7de318d2681b94340e667089405cae9046195f5a44acb8190b2816cd7 -->
<!-- api: F:Inno.Native.Sdl3.SDLDateTime.Day b46a61ec7fe722f45415b40f4e75c262d85bae88cdc6042d17a4767d8bff2124 -->
<!-- api: F:Inno.Native.Sdl3.SDLDateTime.Hour 768ee13c9fa5891b62d60a6b8cb127cf2a80b36dd6074d8d42522c0b6db2779d -->
<!-- api: F:Inno.Native.Sdl3.SDLDateTime.Minute 5a373ecb674e47d40838439e95c48e3706c3861c029cafab8f9fa0e4e878c148 -->
<!-- api: F:Inno.Native.Sdl3.SDLDateTime.Second bb42e4ba03abd0316c5d358ce553ea5e382d470bb438cb460f60189c67a6d074 -->
<!-- api: F:Inno.Native.Sdl3.SDLDateTime.Nanosecond 2053555d54527f46f83e83abeead9f4513275cd71c7aedc4896bbe2524b4674f -->
<!-- api: F:Inno.Native.Sdl3.SDLDateTime.DayOfWeek e930fb4329ed0ca9bb7a8348f9f3b8edfbc5f862dfadfd4fdcb7369c384fcfc2 -->
<!-- api: F:Inno.Native.Sdl3.SDLDateTime.UtcOffset 3e098beef15ce64bfc0ee74d5ca36b79388d1903e09d5d19cab40f023cdab3a7 -->
<!-- api: M:Inno.Native.Sdl3.SDLDateTime.#ctor(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32) 27e120fa651561a84eda33cdb11334e478228121174d7cf0801e9fb4e03c90a5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5517eac28765c7b7"></a>[`public int Inno.Native.Sdl3.SDLDateTime.Day`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6377) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9791ee576e0ba72f"></a>[`public int Inno.Native.Sdl3.SDLDateTime.DayOfWeek`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6382) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bd4fa58478de8e32"></a>[`public int Inno.Native.Sdl3.SDLDateTime.Hour`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6378) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b70adba2d7825c0f"></a>[`public int Inno.Native.Sdl3.SDLDateTime.Minute`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6379) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-93e58975946b4cd1"></a>[`public int Inno.Native.Sdl3.SDLDateTime.Month`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6376) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf488883a8131f88"></a>[`public int Inno.Native.Sdl3.SDLDateTime.Nanosecond`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6381) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-25fa9104e94557d3"></a>[`public int Inno.Native.Sdl3.SDLDateTime.Second`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6380) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35f3bf1281a2a57f"></a>[`public int Inno.Native.Sdl3.SDLDateTime.UtcOffset`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6383) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-02424b785dc33b39"></a>[`public int Inno.Native.Sdl3.SDLDateTime.Year`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6375) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-108f738ac5495240"></a>[`public Inno.Native.Sdl3.SDLDateTime.SDLDateTime(int year = 0, int month = 0, int day = 0, int hour = 0, int minute = 0, int second = 0, int nanosecond = 0, int day_of_week = 0, int utc_offset = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6384) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2d928b667f88238d"></a>[`public struct Inno.Native.Sdl3.SDLDateTime`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6373) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLDateTimePtr</code> · 22 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLDateTimePtr 9fbead0a6806f420ce37ded88c3b6a9d768efeb9bed961fca6dbebe9bf7e10a8 -->
<!-- api: M:Inno.Native.Sdl3.SDLDateTimePtr.#ctor(Inno.Native.Sdl3.SDLDateTime*) 23b45c6441e43cd0885c7e3e10695118a551b99c043e6531817e108fc2986c51 -->
<!-- api: F:Inno.Native.Sdl3.SDLDateTimePtr.Handle 59b140a302460e1701c4f374fe315c15e8b46eb660a35c0939e97b2696dc3108 -->
<!-- api: P:Inno.Native.Sdl3.SDLDateTimePtr.IsNull 49ca56fe5f38a1b56f1ca86c8143fecc3442f64f2427cff96ffecb2f01054ac9 -->
<!-- api: P:Inno.Native.Sdl3.SDLDateTimePtr.Null 2266fe344762ebb29b3fd450a678da5d6eef655b629152d66b5d72107eb6726c -->
<!-- api: P:Inno.Native.Sdl3.SDLDateTimePtr.Item(System.Int32) 437c886b270810c4bd435b2b3422bf9322413bfff9a5642c1013c9468a9dc837 -->
<!-- api: M:Inno.Native.Sdl3.SDLDateTimePtr.op_Implicit(Inno.Native.Sdl3.SDLDateTime*)~Inno.Native.Sdl3.SDLDateTimePtr 78e1c19eaf95be66c64ba4e6137569a69893a05fe5143c5576adef9b19b4fca2 -->
<!-- api: M:Inno.Native.Sdl3.SDLDateTimePtr.op_Implicit(Inno.Native.Sdl3.SDLDateTimePtr)~Inno.Native.Sdl3.SDLDateTime* 1fbdcfe9806faa5039963bac6a53d8089ca7e557c1aa537666654d5c78d78434 -->
<!-- api: M:Inno.Native.Sdl3.SDLDateTimePtr.op_Equality(Inno.Native.Sdl3.SDLDateTimePtr,Inno.Native.Sdl3.SDLDateTimePtr) 9dd944dc8fb99e632fa598f990948bfa99fde295c72c177098715ee4f4c980c3 -->
<!-- api: M:Inno.Native.Sdl3.SDLDateTimePtr.op_Inequality(Inno.Native.Sdl3.SDLDateTimePtr,Inno.Native.Sdl3.SDLDateTimePtr) 5262078469d529d529bedda4f40a1a099e0f6b37a786196ce284bbaa9da972e8 -->
<!-- api: M:Inno.Native.Sdl3.SDLDateTimePtr.Equals(Inno.Native.Sdl3.SDLDateTimePtr) cec5e6e4ce69712de1a525b6e1c1fbf46cff28e7ff3afc043fad710b7d96d644 -->
<!-- api: M:Inno.Native.Sdl3.SDLDateTimePtr.Equals(System.Object) ad3735125f90ff2d11d93d308c1f8668c86100d6d91021441aadbea068515216 -->
<!-- api: M:Inno.Native.Sdl3.SDLDateTimePtr.GetHashCode a090c81452769150956a90aaed67e05ac547edd03905618e0978fb00ebd4253c -->
<!-- api: P:Inno.Native.Sdl3.SDLDateTimePtr.Year 93d9a102af305b1cf0d5e06d32903d6ef68b31b4fda658be5cb08fc1ad26902f -->
<!-- api: P:Inno.Native.Sdl3.SDLDateTimePtr.Month 5433c9f2384be2aea1271a86a89e7c11e7c6dec3f1ca38b761aef1136194e80b -->
<!-- api: P:Inno.Native.Sdl3.SDLDateTimePtr.Day 3a17698dc93769cdec53adeb7f30ff0eb7bafb8ef15cc75262f5aa8d1b096012 -->
<!-- api: P:Inno.Native.Sdl3.SDLDateTimePtr.Hour d3f82dffebaa3b2b7d2dbf241c8ade84d91d20123e7547cb1738d6aee2248ec2 -->
<!-- api: P:Inno.Native.Sdl3.SDLDateTimePtr.Minute a49aa93b6fb62d2154e39c3921ecc5c4f1cf4790acc372a8ad051ce6ecba0dd1 -->
<!-- api: P:Inno.Native.Sdl3.SDLDateTimePtr.Second 53ddf87ed3ea3f1c9d5cc4ce10d3ec8d52780eec3c5cebf9dc8f08505b1bfd80 -->
<!-- api: P:Inno.Native.Sdl3.SDLDateTimePtr.Nanosecond 98ad2688a329bfe99d34be099a104b481c75c71700e98f0f19e67d7eb1d22ce2 -->
<!-- api: P:Inno.Native.Sdl3.SDLDateTimePtr.DayOfWeek 3092cc3134e061bbdfe53760eae6ff3ab89f69a433a411a4d866b6de77989d0e -->
<!-- api: P:Inno.Native.Sdl3.SDLDateTimePtr.UtcOffset e6e1aeb89f49abb703b5500dbe2b64f32af08f9b3e9f36014d7151cb2c3a6884 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d6d2cc71cf0a50aa"></a>[`public Inno.Native.Sdl3.SDLDateTime* Inno.Native.Sdl3.SDLDateTimePtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8146) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae87955681f5d936"></a>[`public Inno.Native.Sdl3.SDLDateTimePtr.SDLDateTimePtr(Inno.Native.Sdl3.SDLDateTime* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8141) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8f27f11aedde5424"></a>[`public bool Inno.Native.Sdl3.SDLDateTimePtr.Equals(Inno.Native.Sdl3.SDLDateTimePtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8156) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-4c5303cf03bb10fd"></a>[`public override bool Inno.Native.Sdl3.SDLDateTimePtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8157) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-73d957bcefd4ea1f"></a>[`public override int Inno.Native.Sdl3.SDLDateTimePtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8158) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-d35ff85ff33aeb6b"></a>[`public static bool Inno.Native.Sdl3.SDLDateTimePtr.operator ==(Inno.Native.Sdl3.SDLDateTimePtr left, Inno.Native.Sdl3.SDLDateTimePtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8154) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a51533c0986991cb"></a>[`public static Inno.Native.Sdl3.SDLDateTimePtr.implicit operator Inno.Native.Sdl3.SDLDateTimePtr(Inno.Native.Sdl3.SDLDateTime* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8152) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ce3e1aec1679ba4e"></a>[`public static Inno.Native.Sdl3.SDLDateTimePtr.implicit operator Inno.Native.Sdl3.SDLDateTime*(Inno.Native.Sdl3.SDLDateTimePtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8153) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1a3e99b97ff1abf7"></a>[`public static bool Inno.Native.Sdl3.SDLDateTimePtr.operator !=(Inno.Native.Sdl3.SDLDateTimePtr left, Inno.Native.Sdl3.SDLDateTimePtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8155) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bcf8fe75ba846b62"></a>[`public ref int Inno.Native.Sdl3.SDLDateTimePtr.Day { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8162) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-044f66ff20bb6348"></a>[`public ref int Inno.Native.Sdl3.SDLDateTimePtr.DayOfWeek { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8167) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-20410ff60157b624"></a>[`public ref int Inno.Native.Sdl3.SDLDateTimePtr.Hour { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8163) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ec99a8cea1c8eef1"></a>[`public bool Inno.Native.Sdl3.SDLDateTimePtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8147) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ec5037aa5312077"></a>[`public Inno.Native.Sdl3.SDLDateTime Inno.Native.Sdl3.SDLDateTimePtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8150) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c2c79bd01ea13fe6"></a>[`public ref int Inno.Native.Sdl3.SDLDateTimePtr.Minute { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8164) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-39f9b69c5fe63093"></a>[`public ref int Inno.Native.Sdl3.SDLDateTimePtr.Month { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8161) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-422322ed1d9e4ecc"></a>[`public ref int Inno.Native.Sdl3.SDLDateTimePtr.Nanosecond { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8166) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1d8a852ad8303fe7"></a>[`public static Inno.Native.Sdl3.SDLDateTimePtr Inno.Native.Sdl3.SDLDateTimePtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8148) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6b2ff830e739390b"></a>[`public ref int Inno.Native.Sdl3.SDLDateTimePtr.Second { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8165) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-526613d45d32447e"></a>[`public ref int Inno.Native.Sdl3.SDLDateTimePtr.UtcOffset { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8168) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f814a07465876cff"></a>[`public ref int Inno.Native.Sdl3.SDLDateTimePtr.Year { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8160) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-804070785a0dc5a8"></a>[`public struct Inno.Native.Sdl3.SDLDateTimePtr : System.IEquatable<Inno.Native.Sdl3.SDLDateTimePtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8139) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLDialogFileCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLDialogFileCallback e45826eb7f581d0744b96987184a581d303b9d3e95f61c983b2c2370420d0cad -->
<!-- api: M:Inno.Native.Sdl3.SDLDialogFileCallback.#ctor(System.Object,System.IntPtr) 99dbdba02defcfcf6e3dc9b4f5b469096551565069b8b858e29f8526a1844793 -->
<!-- api: M:Inno.Native.Sdl3.SDLDialogFileCallback.Invoke(System.Void*,System.Byte**,System.Int32) d5be66f3da9a1cc07d20c5e057f7bf2a3fa5610222c12dbf2d726a2debdd6216 -->
<!-- api: M:Inno.Native.Sdl3.SDLDialogFileCallback.BeginInvoke(System.Void*,System.Byte**,System.Int32,System.AsyncCallback,System.Object) f09fa5fa990d5232039928f2bf998e83b61bd5511adb07054ea8adb18dab2e33 -->
<!-- api: M:Inno.Native.Sdl3.SDLDialogFileCallback.EndInvoke(System.IAsyncResult) bdae596995ccbe94840ab755912b64cb4af44d03eb22db00553cdc86f1b43c42 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3a05b3b52c723247"></a>[`public Inno.Native.Sdl3.SDLDialogFileCallback.SDLDialogFileCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6865) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8a5fe209b1cd904a"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLDialogFileCallback.BeginInvoke(void* userdata, byte** filelist, int filter, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6865) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0f15ed6a7b84f16c"></a>[`public virtual void Inno.Native.Sdl3.SDLDialogFileCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6865) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a0a4f0c0a92aafec"></a>[`public virtual void Inno.Native.Sdl3.SDLDialogFileCallback.Invoke(void* userdata, byte** filelist, int filter)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6865) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-551a4148f5fde0ac"></a>[`public delegate void Inno.Native.Sdl3.SDLDialogFileCallback(void* userdata, byte** filelist, int filter)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6865) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLDialogFileFilter</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLDialogFileFilter ff5f6658c2cdc6b7e885ad74b6b80429d79b48f9494fc7e92a63e968fdd9396f -->
<!-- api: F:Inno.Native.Sdl3.SDLDialogFileFilter.Name bad91923f4d7eaa599d3c675df9c239ff1532a161dc6a832d91ca6b20a6f077f -->
<!-- api: F:Inno.Native.Sdl3.SDLDialogFileFilter.Pattern 04b93941ec82ebfb23ab15d5a2ae201bbb9d71d31f1a3bafba53c1eb54fa6399 -->
<!-- api: M:Inno.Native.Sdl3.SDLDialogFileFilter.#ctor(System.Byte*,System.Byte*) 197d4070958cb405ed6ee2bf0dc9049e70a4a75a9b15aab128b7e5befc2a06b9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ea1f9b8789ea6266"></a>[`public byte* Inno.Native.Sdl3.SDLDialogFileFilter.Name`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2676) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-308b443adb1886b7"></a>[`public byte* Inno.Native.Sdl3.SDLDialogFileFilter.Pattern`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2677) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-37d2909fa89e60bd"></a>[`public Inno.Native.Sdl3.SDLDialogFileFilter.SDLDialogFileFilter(byte* name = null, byte* pattern = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2678) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b9e78f85a2cd14b5"></a>[`public struct Inno.Native.Sdl3.SDLDialogFileFilter`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2674) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLDialogFileFilterPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLDialogFileFilterPtr ee426f0d6d8f30aff04449cf290a0b3e3c88b11674f146692751753e6b3d9dee -->
<!-- api: M:Inno.Native.Sdl3.SDLDialogFileFilterPtr.#ctor(Inno.Native.Sdl3.SDLDialogFileFilter*) 90ed14575bfe89eed1d198c51acfe56a8e24abf8d63df7cc32187b34ca356c29 -->
<!-- api: F:Inno.Native.Sdl3.SDLDialogFileFilterPtr.Handle c44e7dba3ef31d9a08f7918af75e2394650689561f6141e139e3c2ec2b85e998 -->
<!-- api: P:Inno.Native.Sdl3.SDLDialogFileFilterPtr.IsNull c77dc173f29156e9d736a4021c8dba2c0b3186a77777a171db91f6b93b29948c -->
<!-- api: P:Inno.Native.Sdl3.SDLDialogFileFilterPtr.Null 123c245e2ee132791861aeef1245ab0e7ffffeb7ce183cd6f8a4871b5a65c2e9 -->
<!-- api: P:Inno.Native.Sdl3.SDLDialogFileFilterPtr.Item(System.Int32) ea4f17fe60087205249e0f88295b73f97d3b0e5f643a4271f6ff9c08d174b401 -->
<!-- api: M:Inno.Native.Sdl3.SDLDialogFileFilterPtr.op_Implicit(Inno.Native.Sdl3.SDLDialogFileFilter*)~Inno.Native.Sdl3.SDLDialogFileFilterPtr ed07ea9c8e86fccfca73fc990ff429e6f0ebc2799a53fade4375451b41bb2425 -->
<!-- api: M:Inno.Native.Sdl3.SDLDialogFileFilterPtr.op_Implicit(Inno.Native.Sdl3.SDLDialogFileFilterPtr)~Inno.Native.Sdl3.SDLDialogFileFilter* 0181bce1a9761cedbe697bfb888e9fc1c1f3f548af6e2a390f2dccb1c371b167 -->
<!-- api: M:Inno.Native.Sdl3.SDLDialogFileFilterPtr.op_Equality(Inno.Native.Sdl3.SDLDialogFileFilterPtr,Inno.Native.Sdl3.SDLDialogFileFilterPtr) 58171b8a866a210e4390a5a15613d068f1de7764856e476b675792220ea6dbe1 -->
<!-- api: M:Inno.Native.Sdl3.SDLDialogFileFilterPtr.op_Inequality(Inno.Native.Sdl3.SDLDialogFileFilterPtr,Inno.Native.Sdl3.SDLDialogFileFilterPtr) 78cc759449a5a5da155fab60699182a51a91a1acbb7cdd9dbe07201f0f101e86 -->
<!-- api: M:Inno.Native.Sdl3.SDLDialogFileFilterPtr.Equals(Inno.Native.Sdl3.SDLDialogFileFilterPtr) 944393cde4b358866b6ffe17302e512124a8409a8bf72d4839ecaa2cb5834c32 -->
<!-- api: M:Inno.Native.Sdl3.SDLDialogFileFilterPtr.Equals(System.Object) c1ae6d32a83186a829fad35884964c7af525978761083f1410f1b86dc6921e7d -->
<!-- api: M:Inno.Native.Sdl3.SDLDialogFileFilterPtr.GetHashCode 0c01501cc4cca831a54d77697ce1f6702ab8306dfd86af2f90f76684358c2ee1 -->
<!-- api: P:Inno.Native.Sdl3.SDLDialogFileFilterPtr.Name b7a49e6dfbf97556ba16d2ca6b3d6f926bc99f85bc2e83ab8663c356506b18bd -->
<!-- api: P:Inno.Native.Sdl3.SDLDialogFileFilterPtr.Pattern 205dc69814771f141fd4961e761d6451be490b9038b25750b919c9c388f529fc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-02fb84b6a304f3b2"></a>[`public Inno.Native.Sdl3.SDLDialogFileFilter* Inno.Native.Sdl3.SDLDialogFileFilterPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8179) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf0555a9a32ffec1"></a>[`public Inno.Native.Sdl3.SDLDialogFileFilterPtr.SDLDialogFileFilterPtr(Inno.Native.Sdl3.SDLDialogFileFilter* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8174) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-53bd613b43cc3676"></a>[`public bool Inno.Native.Sdl3.SDLDialogFileFilterPtr.Equals(Inno.Native.Sdl3.SDLDialogFileFilterPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8189) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-9a3e37ac3bc18ab1"></a>[`public override bool Inno.Native.Sdl3.SDLDialogFileFilterPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8190) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-276846428abd048d"></a>[`public override int Inno.Native.Sdl3.SDLDialogFileFilterPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8191) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-0b143440ee4d08af"></a>[`public static bool Inno.Native.Sdl3.SDLDialogFileFilterPtr.operator ==(Inno.Native.Sdl3.SDLDialogFileFilterPtr left, Inno.Native.Sdl3.SDLDialogFileFilterPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8187) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7228ea771b316c1f"></a>[`public static Inno.Native.Sdl3.SDLDialogFileFilterPtr.implicit operator Inno.Native.Sdl3.SDLDialogFileFilterPtr(Inno.Native.Sdl3.SDLDialogFileFilter* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8185) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-348436550ff2e98d"></a>[`public static Inno.Native.Sdl3.SDLDialogFileFilterPtr.implicit operator Inno.Native.Sdl3.SDLDialogFileFilter*(Inno.Native.Sdl3.SDLDialogFileFilterPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8186) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-79811d1ebe604da6"></a>[`public static bool Inno.Native.Sdl3.SDLDialogFileFilterPtr.operator !=(Inno.Native.Sdl3.SDLDialogFileFilterPtr left, Inno.Native.Sdl3.SDLDialogFileFilterPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8188) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-37981516574aad93"></a>[`public bool Inno.Native.Sdl3.SDLDialogFileFilterPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8180) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b50c435a4457fb6b"></a>[`public Inno.Native.Sdl3.SDLDialogFileFilter Inno.Native.Sdl3.SDLDialogFileFilterPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8183) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c5f3330d05c62a09"></a>[`public byte* Inno.Native.Sdl3.SDLDialogFileFilterPtr.Name { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8193) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-734ced09fbfc0a58"></a>[`public static Inno.Native.Sdl3.SDLDialogFileFilterPtr Inno.Native.Sdl3.SDLDialogFileFilterPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8181) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-016b5af64467e272"></a>[`public byte* Inno.Native.Sdl3.SDLDialogFileFilterPtr.Pattern { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8194) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-79e161f5fdae338e"></a>[`public struct Inno.Native.Sdl3.SDLDialogFileFilterPtr : System.IEquatable<Inno.Native.Sdl3.SDLDialogFileFilterPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8172) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLDisplayEvent</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLDisplayEvent b5e47f9cfc74496d59152606fb3845e0976ed277950f76c6a27bea549fbab0c5 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayEvent.Type 02b9f33bcdf5865ce423bfbf65edcde92d21f891bfd17b452ec0e4a3c9ac7c69 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayEvent.Reserved 94220dcec2560a1100a3515ede5ade704b4af0b1ff4267505ccec61d7fcf21c3 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayEvent.Timestamp a245014cbd6cbda55af380ffce80bd3429fa044f222d3595cbb45d6d5b43b2c2 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayEvent.DisplayId cab617b89d35a4c1e355502cb9eae28826bf1f92df122a5d1dec4bd954f67dd4 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayEvent.Data1 38677fff720204bba7e6b1d9e4a71b9abfe5542b60ad94b8d3b34b4701d53036 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayEvent.Data2 4e181f90a507d4377c28a35c1d53d3895fe6c91ebb99808d74736afc62556463 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt32,System.Int32,System.Int32) 189f0469288d9fdfd0753d748bb029c67274668f3c76cfb6843a005be2ca5e83 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e49f1599198a1f19"></a>[`public int Inno.Native.Sdl3.SDLDisplayEvent.Data1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3094) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7616902489eb5bd1"></a>[`public int Inno.Native.Sdl3.SDLDisplayEvent.Data2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3095) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c5e9e76b3ea210b0"></a>[`public uint Inno.Native.Sdl3.SDLDisplayEvent.DisplayId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3093) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-369efd7755b17a33"></a>[`public uint Inno.Native.Sdl3.SDLDisplayEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3091) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be1bc23ff572df7f"></a>[`public ulong Inno.Native.Sdl3.SDLDisplayEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3092) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c355fc67be459874"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLDisplayEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3090) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f40083e0e90e5e5"></a>[`public Inno.Native.Sdl3.SDLDisplayEvent.SDLDisplayEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, uint displayID = 0, int data1 = 0, int data2 = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3096) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b15d66f4ea51135b"></a>[`public struct Inno.Native.Sdl3.SDLDisplayEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3088) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLDisplayMode</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLDisplayMode 1f80c390b390f8906a1127997e27730f0b4f1e4f3111e8d420c9a8633a38127d -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.DisplayId cac946d0fa9fd96fa786aabfb775d86d20c28bcccaf4b1281bc3b6ee6023895c -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.Format 5b80b19a138e8dd051dbf569bfe23428581b4125d3d5c71603cc74e0c7ad9711 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.W e68865e2fdf1ebffbd7e418b83c9541218bbe2c43978083b56f973e6cbd78da3 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.H 7076cebe1b34dc38481fe8fe916e94f3763e4eeeb58540c2ae42e206cbdd217a -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.PixelDensity 7f251c513cbd0afc46890cb59cfce77128dc8c878c92d80ef3f2529c83670ed6 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.RefreshRate 450e97d55afbfae068e8e8fa1dcc158f478be709558f698686847b19a910ca14 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.RefreshRateNumerator 98b916eed96e65b73c93711ef306a49cec32768d2272ae54595c1e836c61b161 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.RefreshRateDenominator 4f99d7e151000b5b13970fb599cd1022395ec100ed3c7b6d81bdebd9ad76c84e -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayMode.Internal aa213edf8ddc013f6859800fce3aba71877b174ec18567bb4dd7becbca2b234d -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayMode.#ctor(System.UInt32,Inno.Native.Sdl3.SDLPixelFormat,System.Int32,System.Int32,System.Single,System.Single,System.Int32,System.Int32,Inno.Native.Sdl3.SDLDisplayModeData) c12efc822f01c22131bb74e7fcc89cd956121938de90ff5fa5138cd4396b8a32 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4c01065e3dd32b5b"></a>[`public uint Inno.Native.Sdl3.SDLDisplayMode.DisplayId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2599) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bfc7e879cce3b789"></a>[`public Inno.Native.Sdl3.SDLPixelFormat Inno.Native.Sdl3.SDLDisplayMode.Format`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2600) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc91cf117b42dad8"></a>[`public int Inno.Native.Sdl3.SDLDisplayMode.H`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2602) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-340bbf5dc692df16"></a>[`public Inno.Native.Sdl3.SDLDisplayModeData Inno.Native.Sdl3.SDLDisplayMode.Internal`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2607) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8297bbd0827fe520"></a>[`public float Inno.Native.Sdl3.SDLDisplayMode.PixelDensity`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2603) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc66b95b2ec1678d"></a>[`public float Inno.Native.Sdl3.SDLDisplayMode.RefreshRate`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2604) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-481e6b28810b3fc7"></a>[`public int Inno.Native.Sdl3.SDLDisplayMode.RefreshRateDenominator`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2606) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccea2bdf939a1488"></a>[`public int Inno.Native.Sdl3.SDLDisplayMode.RefreshRateNumerator`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2605) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-88766a65efee3b5f"></a>[`public int Inno.Native.Sdl3.SDLDisplayMode.W`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2601) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-45a8f97ba18fdc62"></a>[`public Inno.Native.Sdl3.SDLDisplayMode.SDLDisplayMode(uint displayID = 0, Inno.Native.Sdl3.SDLPixelFormat format = Inno.Native.Sdl3.SDLPixelFormat.PixelformatUnknown, int w = 0, int h = 0, float pixel_density = 0, float refresh_rate = 0, int refresh_rate_numerator = 0, int refresh_rate_denominator = 0, Inno.Native.Sdl3.SDLDisplayModeData @internal = default(Inno.Native.Sdl3.SDLDisplayModeData))`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2608) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-88e1395aa73d987d"></a>[`public struct Inno.Native.Sdl3.SDLDisplayMode`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2597) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLDisplayModeData</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLDisplayModeData 7cc53257a5eaa46aed894c5d10a5c7e78d961f86d7ca9c1a129fc6e3f13a830a -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModeData.#ctor(System.IntPtr) 253116b200eba6c16b76c70c1bfeeb73861fa3b2ca4c80d4c4599f69d216f578 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModeData.Handle f8c0a7438ca879ada44acc3194e3022c2d179e94506336ef2493c8e9906e78d0 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModeData.IsNull b634b85b2e299e56b38e9e6b8e01c5f98bd1feb430551148caa427c6b508c0ec -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModeData.Null e3aadec67559b9031d5886afcd216f2744d23f0a310231fc9b8a591df173bc12 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModeData.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLDisplayModeData 0935aac3009e5a2f90969de69579e370cde3190ace56f55b1196476f1c70dc91 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModeData.op_Equality(Inno.Native.Sdl3.SDLDisplayModeData,Inno.Native.Sdl3.SDLDisplayModeData) 444afd912986b63c6ddd9d285d328118bc08f91f4a988a47b066ea0067d46b01 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModeData.op_Inequality(Inno.Native.Sdl3.SDLDisplayModeData,Inno.Native.Sdl3.SDLDisplayModeData) bdb3430727cae83e271d5b7f20fb10cf82bf883bd7f96ce74f304395df2d054c -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModeData.Equals(Inno.Native.Sdl3.SDLDisplayModeData) a9090788abdb95a77d76cdba0e0ee14274e5686c30c0d27559ce226334e2185a -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModeData.Equals(System.Object) d967e36ff44f35c81d05c6d08326e2d628e92be6471b59aff7c023671f1a380a -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModeData.GetHashCode cf3c40a28649e4716763696883b9e96d9c9968cb4307178b2633df47b53bf4fc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ee10264e429e2fcf"></a>[`public Inno.Native.Sdl3.SDLDisplayModeData.SDLDisplayModeData(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2575) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3c1481da7c4a7def"></a>[`public bool Inno.Native.Sdl3.SDLDisplayModeData.Equals(Inno.Native.Sdl3.SDLDisplayModeData other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2587) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-3c0118db2f44fa43"></a>[`public override bool Inno.Native.Sdl3.SDLDisplayModeData.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2588) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-d377460e71cc9e6b"></a>[`public override int Inno.Native.Sdl3.SDLDisplayModeData.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2589) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-61b83d0a5ee6e4ad"></a>[`public static bool Inno.Native.Sdl3.SDLDisplayModeData.operator ==(Inno.Native.Sdl3.SDLDisplayModeData left, Inno.Native.Sdl3.SDLDisplayModeData right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2585) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b77551ee030d8d13"></a>[`public static Inno.Native.Sdl3.SDLDisplayModeData.implicit operator Inno.Native.Sdl3.SDLDisplayModeData(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2584) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e9097e57a131c23"></a>[`public static bool Inno.Native.Sdl3.SDLDisplayModeData.operator !=(Inno.Native.Sdl3.SDLDisplayModeData left, Inno.Native.Sdl3.SDLDisplayModeData right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2586) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5713ce7c9b738d00"></a>[`public nint Inno.Native.Sdl3.SDLDisplayModeData.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2580) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bdec555dab9ca09d"></a>[`public bool Inno.Native.Sdl3.SDLDisplayModeData.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2581) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0de81281708b4bba"></a>[`public static Inno.Native.Sdl3.SDLDisplayModeData Inno.Native.Sdl3.SDLDisplayModeData.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2582) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-735a14c3cb4db8fc"></a>[`public readonly struct Inno.Native.Sdl3.SDLDisplayModeData : System.IEquatable<Inno.Native.Sdl3.SDLDisplayModeData>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2573) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLDisplayModePtr</code> · 22 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLDisplayModePtr 29fe00915ddefa71c0579b7cb63cd134e27842e870d1e04b0cf8dc78ffa0ab56 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtr.#ctor(Inno.Native.Sdl3.SDLDisplayMode*) 78b2bb042c0b42e5839402c8612794d3e2fb6e1d76212e35a5809c647cfac126 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayModePtr.Handle e3d3d51d159cd33a19e8d3b31cfb9d5141036f0eb14b7b509f69d77aa802882a -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtr.IsNull ed2df253aef0949f10dc89a1618ed7002566516a8dbf3bb1feb37b3a9055896b -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtr.Null aa086f89eab6e933d050450a04a70a441bad66c729559dcdcf7f903a50052791 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtr.Item(System.Int32) c8f1597bbbb52ba588b7a1fb184c156056159c7dd4806749ad5bd78c44274028 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtr.op_Implicit(Inno.Native.Sdl3.SDLDisplayMode*)~Inno.Native.Sdl3.SDLDisplayModePtr 8690a047ddce70c2055fcd239e55a204a1107e701671f2415fddede68c337dc5 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtr.op_Implicit(Inno.Native.Sdl3.SDLDisplayModePtr)~Inno.Native.Sdl3.SDLDisplayMode* 17c273fa7c2d102b340478257f67e25051df3fc7a5b1789eb2c7b0e9170db87d -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtr.op_Equality(Inno.Native.Sdl3.SDLDisplayModePtr,Inno.Native.Sdl3.SDLDisplayModePtr) 9cb8e36f2e18dc136e1e2361c2277a559a6b7d4f052fcfddd72d5d5cd2870ac8 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtr.op_Inequality(Inno.Native.Sdl3.SDLDisplayModePtr,Inno.Native.Sdl3.SDLDisplayModePtr) 28647cbe00f58d55eee43a930a1e48492a89843700e0a25bc494874d15b81da3 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtr.Equals(Inno.Native.Sdl3.SDLDisplayModePtr) e4e428648d9222f524b6297f21dd47f8ea2c09bf3051512d5421465e3f442f1d -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtr.Equals(System.Object) 0ec7341c2532e64991222fa5451e1ef75d64f6d885dc0bd6d9bf41d97bb1ccce -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtr.GetHashCode 9ae39ef160f28d961df075f9aa46251e6c144144a6e8fca72d98ef5073173223 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtr.DisplayId 6bf0fa7f1fd84d09f2423abbe9d5a6ac0e1a98d261183086287463d0b9595d22 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtr.Format ab8544e3158d0829128f3b5063268e058edcc8bcea6e779ba0f2c7c3dca08e93 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtr.W 2c9ddf97bed24079235a9fb015e33c76d27f6fb8b169b5464762e52864d9a4b0 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtr.H cbcda4527709d6cf65e9cabab97b8487f697beaaec9e2576331b4ee6b5da00f6 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtr.PixelDensity c6f0c1065676a253ae2d4fa70a8bc97eb636e0e9eadd6f029b33109933a61949 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtr.RefreshRate 2fe2cd0bedbae5984f9ec45fbe8edd41fe718512bc00381a129c86e4982f25c2 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtr.RefreshRateNumerator c0083c3f1a8341cbc5a53be72c7fa9962f99a531f84af1f9dc3a9e1594dabf0c -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtr.RefreshRateDenominator fff659f5a2a1a77183ba53a1aa333b12faf2bcfdd24771dfd588181fd30c0723 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtr.Internal fadd468b90a8bc8cbe2857c92ec5cfb1d0723b8b72dbc8b737e89f2db0972fae -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a75004dbb11166f4"></a>[`public Inno.Native.Sdl3.SDLDisplayMode* Inno.Native.Sdl3.SDLDisplayModePtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8205) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c31f7bd1b0afb8d0"></a>[`public Inno.Native.Sdl3.SDLDisplayModePtr.SDLDisplayModePtr(Inno.Native.Sdl3.SDLDisplayMode* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8200) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-271c15901118c9f4"></a>[`public bool Inno.Native.Sdl3.SDLDisplayModePtr.Equals(Inno.Native.Sdl3.SDLDisplayModePtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8215) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-32a628c37eec2ee4"></a>[`public override bool Inno.Native.Sdl3.SDLDisplayModePtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8216) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-cbf15b956e889a36"></a>[`public override int Inno.Native.Sdl3.SDLDisplayModePtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8217) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-4d9d153e433f0cd8"></a>[`public static bool Inno.Native.Sdl3.SDLDisplayModePtr.operator ==(Inno.Native.Sdl3.SDLDisplayModePtr left, Inno.Native.Sdl3.SDLDisplayModePtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8213) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5007412464f22428"></a>[`public static Inno.Native.Sdl3.SDLDisplayModePtr.implicit operator Inno.Native.Sdl3.SDLDisplayModePtr(Inno.Native.Sdl3.SDLDisplayMode* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8211) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-de623d993cd11cdc"></a>[`public static Inno.Native.Sdl3.SDLDisplayModePtr.implicit operator Inno.Native.Sdl3.SDLDisplayMode*(Inno.Native.Sdl3.SDLDisplayModePtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8212) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bef5ae2ab0efebd9"></a>[`public static bool Inno.Native.Sdl3.SDLDisplayModePtr.operator !=(Inno.Native.Sdl3.SDLDisplayModePtr left, Inno.Native.Sdl3.SDLDisplayModePtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8214) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-df1ea8aaeaa5466e"></a>[`public ref uint Inno.Native.Sdl3.SDLDisplayModePtr.DisplayId { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8219) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-625c3867b6fbe566"></a>[`public ref Inno.Native.Sdl3.SDLPixelFormat Inno.Native.Sdl3.SDLDisplayModePtr.Format { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8220) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-153e4439850711cc"></a>[`public ref int Inno.Native.Sdl3.SDLDisplayModePtr.H { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8222) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9e8b56397d78b1f2"></a>[`public ref Inno.Native.Sdl3.SDLDisplayModeData Inno.Native.Sdl3.SDLDisplayModePtr.Internal { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8227) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0cd43d418834bf3f"></a>[`public bool Inno.Native.Sdl3.SDLDisplayModePtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8206) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-848b9fb81827c88b"></a>[`public Inno.Native.Sdl3.SDLDisplayMode Inno.Native.Sdl3.SDLDisplayModePtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8209) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9cddd63b81b543d3"></a>[`public static Inno.Native.Sdl3.SDLDisplayModePtr Inno.Native.Sdl3.SDLDisplayModePtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8207) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6753332bc0979a34"></a>[`public ref float Inno.Native.Sdl3.SDLDisplayModePtr.PixelDensity { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8223) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-72ba9e3c5cd7ebc6"></a>[`public ref float Inno.Native.Sdl3.SDLDisplayModePtr.RefreshRate { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8224) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eb3681cb8c2097e3"></a>[`public ref int Inno.Native.Sdl3.SDLDisplayModePtr.RefreshRateDenominator { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8226) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b861cfd242d2cfb9"></a>[`public ref int Inno.Native.Sdl3.SDLDisplayModePtr.RefreshRateNumerator { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8225) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7a2190f8a628cde9"></a>[`public ref int Inno.Native.Sdl3.SDLDisplayModePtr.W { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8221) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-04550f859ebcb5da"></a>[`public struct Inno.Native.Sdl3.SDLDisplayModePtr : System.IEquatable<Inno.Native.Sdl3.SDLDisplayModePtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8198) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLDisplayModePtrPtr</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLDisplayModePtrPtr b96101a41cb55411338deab12b9431ba780cde458e2f47de3241a3285e75ebfd -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtrPtr.#ctor(Inno.Native.Sdl3.SDLDisplayMode**) 027ea321e54ddb0939c06c5be437a1e449578c8189c362471ba1be749c082918 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayModePtrPtr.Handle 24c26c71fd1a17a7a058d4ed05ee3a2159458884baaf385ea780cb6f3cbc0a95 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtrPtr.IsNull 69891f68958eefcc0edc2ff843f007db31b940577c0a00c536e8993678a5baa8 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtrPtr.Null 2b23ed78a829da82032cfde4543b4959a2b39fec226e3a75ec0ede5305fad140 -->
<!-- api: P:Inno.Native.Sdl3.SDLDisplayModePtrPtr.Item(System.Int32) 1ec5002755f76327879043697b2d393faf3ab903ee0ba74a2d470e6092f34e4a -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtrPtr.op_Implicit(Inno.Native.Sdl3.SDLDisplayMode**)~Inno.Native.Sdl3.SDLDisplayModePtrPtr ee5ba4a461323bca36e1bca1f47fbe0145623143830b64127515fda0c7f48204 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtrPtr.op_Implicit(Inno.Native.Sdl3.SDLDisplayModePtrPtr)~Inno.Native.Sdl3.SDLDisplayMode** 0ddb1f269afb1b50828518b0887f67556c17c3391a9417111cb7ed3d63fcde33 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtrPtr.op_Equality(Inno.Native.Sdl3.SDLDisplayModePtrPtr,Inno.Native.Sdl3.SDLDisplayModePtrPtr) 48f0843b22bd92386794124a80fd27ad503c1f51f6b418bfc7663dd145290aa5 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtrPtr.op_Inequality(Inno.Native.Sdl3.SDLDisplayModePtrPtr,Inno.Native.Sdl3.SDLDisplayModePtrPtr) ac397451f0de8c0589a2fdba1b8dae7f873dc50c988e77f4f459b8bd2b4b8526 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtrPtr.Equals(Inno.Native.Sdl3.SDLDisplayModePtrPtr) c401d217d557df3c1f7c500a057645b6db04d2e54e3e28193fbe7cef7105a829 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtrPtr.Equals(System.Object) 5b293e0aa59705aa3ee9d2f45c4ef6fa63093d3762f749dc901cb8c87997a283 -->
<!-- api: M:Inno.Native.Sdl3.SDLDisplayModePtrPtr.GetHashCode 28e1c9833554dccd5c4f6ab2be44ac0da889da46bf28c3792fc23f9b324e5a11 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5c8565a6ab0db0ba"></a>[`public Inno.Native.Sdl3.SDLDisplayMode** Inno.Native.Sdl3.SDLDisplayModePtrPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8238) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-192c73f153d6baed"></a>[`public Inno.Native.Sdl3.SDLDisplayModePtrPtr.SDLDisplayModePtrPtr(Inno.Native.Sdl3.SDLDisplayMode** handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8233) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fe792d061ddd8473"></a>[`public bool Inno.Native.Sdl3.SDLDisplayModePtrPtr.Equals(Inno.Native.Sdl3.SDLDisplayModePtrPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8248) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-97ac619270e56a08"></a>[`public override bool Inno.Native.Sdl3.SDLDisplayModePtrPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8249) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-1dab37ba37b948ed"></a>[`public override int Inno.Native.Sdl3.SDLDisplayModePtrPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8250) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-eef2aa388fa30f24"></a>[`public static bool Inno.Native.Sdl3.SDLDisplayModePtrPtr.operator ==(Inno.Native.Sdl3.SDLDisplayModePtrPtr left, Inno.Native.Sdl3.SDLDisplayModePtrPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8246) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2016adbe3c02f96e"></a>[`public static Inno.Native.Sdl3.SDLDisplayModePtrPtr.implicit operator Inno.Native.Sdl3.SDLDisplayModePtrPtr(Inno.Native.Sdl3.SDLDisplayMode** handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8244) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eb9f5fa3c69396fb"></a>[`public static Inno.Native.Sdl3.SDLDisplayModePtrPtr.implicit operator Inno.Native.Sdl3.SDLDisplayMode**(Inno.Native.Sdl3.SDLDisplayModePtrPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8245) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-37f824c7ce61d65f"></a>[`public static bool Inno.Native.Sdl3.SDLDisplayModePtrPtr.operator !=(Inno.Native.Sdl3.SDLDisplayModePtrPtr left, Inno.Native.Sdl3.SDLDisplayModePtrPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8247) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-837f29ad3943362c"></a>[`public bool Inno.Native.Sdl3.SDLDisplayModePtrPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8239) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-69bec2c18bd2a7f1"></a>[`public Inno.Native.Sdl3.SDLDisplayMode* Inno.Native.Sdl3.SDLDisplayModePtrPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8242) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4f777accd9dd279c"></a>[`public static Inno.Native.Sdl3.SDLDisplayModePtrPtr Inno.Native.Sdl3.SDLDisplayModePtrPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8240) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-31e85937588e5575"></a>[`public struct Inno.Native.Sdl3.SDLDisplayModePtrPtr : System.IEquatable<Inno.Native.Sdl3.SDLDisplayModePtrPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8231) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLDisplayOrientation</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLDisplayOrientation deb9d5d55ad53ca02277c5a7b2dc4c9abde85820c26042f2017b7ce8c2eb02c4 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayOrientation.Unknown fb606431489873ba73086df9d3d179e29211bd07bda05a5039197730a19237b4 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayOrientation.Landscape eeca32ea2f3cb14b1b31b5ea99326e1f1fec27d3b3c09763d560baa1297a5cce -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayOrientation.LandscapeFlipped bbcc1d6b5fab6cfd04ade708ff29d129af8ecf0d8914500b616c80f30a757b58 -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayOrientation.Portrait da2862b4c97eb47b707a63862dc230567bf57ca5d7e9a6b9db7da7ae6381183f -->
<!-- api: F:Inno.Native.Sdl3.SDLDisplayOrientation.PortraitFlipped b05f8c7ef13b8e1d475f03b20f3742534ec094b456750d5900e325672e09633f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-43e2f91e23065d74"></a>[`Inno.Native.Sdl3.SDLDisplayOrientation.Landscape = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L558) | 当前常量或枚举值：`1`。 | — |
| <a id="api-dd04c8eb63a20558"></a>[`Inno.Native.Sdl3.SDLDisplayOrientation.LandscapeFlipped = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L559) | 当前常量或枚举值：`2`。 | — |
| <a id="api-90ae3a1758d7e1de"></a>[`Inno.Native.Sdl3.SDLDisplayOrientation.Portrait = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L560) | 当前常量或枚举值：`3`。 | — |
| <a id="api-8e3feab319721d53"></a>[`Inno.Native.Sdl3.SDLDisplayOrientation.PortraitFlipped = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L561) | 当前常量或枚举值：`4`。 | — |
| <a id="api-2650688e609c6ead"></a>[`Inno.Native.Sdl3.SDLDisplayOrientation.Unknown = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L557) | 当前常量或枚举值：`0`。 | — |
| <a id="api-16b384a8138b7355"></a>[`public enum Inno.Native.Sdl3.SDLDisplayOrientation : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L555) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLDropEvent</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLDropEvent 5299bc45d4dbf89fbb0e73d7c9016453d7e818a6d41acbbf4f2521b627048439 -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.Type 88aeea3f57c29bd3b209302db82af5eb77d2dc067941eb417fad332f6e24c708 -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.Reserved 765dd221695891849518c94b3746c89249b6a63d2753bbba0138e72c72e5f0ae -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.Timestamp 4a06275ecbe7c65fd17c7e7dc0bf4917c02852af5f9ffb9b5c93eabb30c50a75 -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.WindowId 4afc822a06ad6296a9962481d4f902ee3d295cd13148ee87c2cfed566f79f43a -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.X 27ad44ddb08c7df2f42017a3ec547ea1746256bdfda9869c59d9ed3d6a680779 -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.Y 83d979f59fd81ff6e6fb0f923651db49559ee887516b9729aa1562c5aa7d8c9d -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.Source 5e7ae6de21184e8a102ad004ebd3f2ce488a7f1b19072a8176b258f8059b2301 -->
<!-- api: F:Inno.Native.Sdl3.SDLDropEvent.Data 88015d518471c2819495e8711f86348940b50312f48433274f7bff1cfa4c2dae -->
<!-- api: M:Inno.Native.Sdl3.SDLDropEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt32,System.Single,System.Single,System.Byte*,System.Byte*) c8caac2374c9254a046e830f00e03911cf3310c4d844567bbf9754a9ecc53590 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9676ad155146486f"></a>[`public byte* Inno.Native.Sdl3.SDLDropEvent.Data`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3951) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c5f8084990830f4c"></a>[`public uint Inno.Native.Sdl3.SDLDropEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3945) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af2dfef2109389f8"></a>[`public byte* Inno.Native.Sdl3.SDLDropEvent.Source`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3950) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-31c9e83b409e9f50"></a>[`public ulong Inno.Native.Sdl3.SDLDropEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3946) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-934e49c123b6687f"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLDropEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3944) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-19b8df8684f59f6b"></a>[`public uint Inno.Native.Sdl3.SDLDropEvent.WindowId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3947) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7a2e098d24a9386"></a>[`public float Inno.Native.Sdl3.SDLDropEvent.X`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3948) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c2f48aec1525ac5d"></a>[`public float Inno.Native.Sdl3.SDLDropEvent.Y`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3949) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3656341705f47986"></a>[`public Inno.Native.Sdl3.SDLDropEvent.SDLDropEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, uint windowID = 0, float x = 0, float y = 0, byte* source = null, byte* data = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3952) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-65d561b0e37cc27a"></a>[`public struct Inno.Native.Sdl3.SDLDropEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3942) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SdlDummyEnum</code> · 2 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SdlDummyEnum a40d825c3fb603dfe614aefef4b3c67aa97078b5644773f97f4ad47d31e6e156 -->
<!-- api: F:Inno.Native.Sdl3.SdlDummyEnum.Value f9932970cd6e45b3c307489087940f0f95a5a2101a4d27c1f2f7c1a1bb3f1fbf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-605c2d215ce03186"></a>[`Inno.Native.Sdl3.SdlDummyEnum.Value = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L72) | 当前常量或枚举值：`0`。 | — |
| <a id="api-1baf7f14191edc28"></a>[`public enum Inno.Native.Sdl3.SdlDummyEnum : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L70) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLEGLAttribArrayCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLEGLAttribArrayCallback 92735a691d95b60a5baed6fc4e1afd89c14f35f4841985c248cf6fb673a5ddc1 -->
<!-- api: M:Inno.Native.Sdl3.SDLEGLAttribArrayCallback.#ctor(System.Object,System.IntPtr) fbbdcee495dd4829c7913bce069f2c07c09b2dc88da825c110024097885244dd -->
<!-- api: M:Inno.Native.Sdl3.SDLEGLAttribArrayCallback.Invoke(System.Void*) 181bf497006d4e8d93274d54fbb1807119e91414dbf351c1d7576df7e162411b -->
<!-- api: M:Inno.Native.Sdl3.SDLEGLAttribArrayCallback.BeginInvoke(System.Void*,System.AsyncCallback,System.Object) 01040b93becfc601c7b36890989d4fd9041e46a9f71443cdc3c0b5c6d0651e24 -->
<!-- api: M:Inno.Native.Sdl3.SDLEGLAttribArrayCallback.EndInvoke(System.IAsyncResult) 2f5f1cbee3daeaf571e9f64f8dbfcb8d6a79646108033e9cfb9e683d736ad77e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7b6b403a3cd680c0"></a>[`public Inno.Native.Sdl3.SDLEGLAttribArrayCallback.SDLEGLAttribArrayCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6850) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-348e33f6e46166ec"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLEGLAttribArrayCallback.BeginInvoke(void* userdata, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6850) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e9d23b1040d63267"></a>[`public virtual long* Inno.Native.Sdl3.SDLEGLAttribArrayCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6850) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1aeca6ad2a28ea76"></a>[`public virtual long* Inno.Native.Sdl3.SDLEGLAttribArrayCallback.Invoke(void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6850) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-310af20e8c6dcb5d"></a>[`public delegate long* Inno.Native.Sdl3.SDLEGLAttribArrayCallback(void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6850) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLEGLIntArrayCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLEGLIntArrayCallback 9ec56ddbd7c575b9d9c1149c3cf8bce2773dba0d2e1657bce2d1c948be7cf8db -->
<!-- api: M:Inno.Native.Sdl3.SDLEGLIntArrayCallback.#ctor(System.Object,System.IntPtr) ad909a22f7f051478c4f7ad60f7ad5d04873210e14492553ac4d02412b060b48 -->
<!-- api: M:Inno.Native.Sdl3.SDLEGLIntArrayCallback.Invoke(System.Void*,System.IntPtr,System.IntPtr) b80b5aa1503b30bce9d5ca4874b538c468fe261ef18932d5b710406a1df5ef7a -->
<!-- api: M:Inno.Native.Sdl3.SDLEGLIntArrayCallback.BeginInvoke(System.Void*,System.IntPtr,System.IntPtr,System.AsyncCallback,System.Object) 45a41e803a040cc046495f48affbeb15b50cd789adbab6acfe777cd688e31902 -->
<!-- api: M:Inno.Native.Sdl3.SDLEGLIntArrayCallback.EndInvoke(System.IAsyncResult) 4abb58cb624855b94d093f70bcf1f2e7dd4d3338e4ca63baa9bd05a3938a5fdc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-816becd3560fa42a"></a>[`public Inno.Native.Sdl3.SDLEGLIntArrayCallback.SDLEGLIntArrayCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6855) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d2a1eb1859532bc0"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLEGLIntArrayCallback.BeginInvoke(void* userdata, nint display, nint config, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6855) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-51a3c8db8ae9741b"></a>[`public virtual int* Inno.Native.Sdl3.SDLEGLIntArrayCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6855) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4cfe8bb21e60c876"></a>[`public virtual int* Inno.Native.Sdl3.SDLEGLIntArrayCallback.Invoke(void* userdata, nint display, nint config)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6855) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ca7f53696f10b581"></a>[`public delegate int* Inno.Native.Sdl3.SDLEGLIntArrayCallback(void* userdata, nint display, nint config)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6855) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLEnumerateDirectoryCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLEnumerateDirectoryCallback 81667c00276bb49f10a6f68120f2482914a680cdb126d3bc88ed5b7fb125b8a0 -->
<!-- api: M:Inno.Native.Sdl3.SDLEnumerateDirectoryCallback.#ctor(System.Object,System.IntPtr) 470e181df559db2eee0873375a582a83590428db55f982a7e22d111a9e5b9e91 -->
<!-- api: M:Inno.Native.Sdl3.SDLEnumerateDirectoryCallback.Invoke(System.Void*,System.Byte*,System.Byte*) 17b8cbd2b20fce31c9be02308d5d246853532a96555c9744c36b2074bc74b1c5 -->
<!-- api: M:Inno.Native.Sdl3.SDLEnumerateDirectoryCallback.BeginInvoke(System.Void*,System.Byte*,System.Byte*,System.AsyncCallback,System.Object) b908b18ea822c7b910fcb6170c5cbc1cf955297d13b0d3da1596b2302869daf8 -->
<!-- api: M:Inno.Native.Sdl3.SDLEnumerateDirectoryCallback.EndInvoke(System.IAsyncResult) 4e0c3129121a5564ce150bd9a4992dc811496557bf51bcc8439a8cb011c1ca20 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-82113e6ccd590df4"></a>[`public Inno.Native.Sdl3.SDLEnumerateDirectoryCallback.SDLEnumerateDirectoryCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6705) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f17b9cce4104897c"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLEnumerateDirectoryCallback.BeginInvoke(void* userdata, byte* dirname, byte* fname, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6705) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-afb9a200508aee6b"></a>[`public virtual Inno.Native.Sdl3.SDLEnumerationResult Inno.Native.Sdl3.SDLEnumerateDirectoryCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6705) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ce0fa71d77b573eb"></a>[`public virtual Inno.Native.Sdl3.SDLEnumerationResult Inno.Native.Sdl3.SDLEnumerateDirectoryCallback.Invoke(void* userdata, byte* dirname, byte* fname)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6705) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e048e132118986b5"></a>[`public delegate Inno.Native.Sdl3.SDLEnumerationResult Inno.Native.Sdl3.SDLEnumerateDirectoryCallback(void* userdata, byte* dirname, byte* fname)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6705) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLEnumeratePropertiesCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLEnumeratePropertiesCallback e8854d3ec72e1441f0f1a43e9236bd45f5a28aaa6018c6c4d7fc73cb25bf5070 -->
<!-- api: M:Inno.Native.Sdl3.SDLEnumeratePropertiesCallback.#ctor(System.Object,System.IntPtr) d0d12b0bb7c680fd7a0e37cfcb09a5aad72e936a94660f782a2e61f528e89309 -->
<!-- api: M:Inno.Native.Sdl3.SDLEnumeratePropertiesCallback.Invoke(System.Void*,System.UInt32,System.Byte*) 8c7a4c89a368fd16ffdac5cb9fa800cca77778a898e72c7d126541f2625c2d77 -->
<!-- api: M:Inno.Native.Sdl3.SDLEnumeratePropertiesCallback.BeginInvoke(System.Void*,System.UInt32,System.Byte*,System.AsyncCallback,System.Object) 98f88df2c1f26d4564dcdd220aee77b5a324758662e87a8bc48984b64ad32aa2 -->
<!-- api: M:Inno.Native.Sdl3.SDLEnumeratePropertiesCallback.EndInvoke(System.IAsyncResult) da9926e31ce43832d94dad3e9f9084556afd71a03771e1715c1c8209e2361e65 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9ed3c820e694b1a2"></a>[`public Inno.Native.Sdl3.SDLEnumeratePropertiesCallback.SDLEnumeratePropertiesCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6810) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2ce15756decf6e65"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLEnumeratePropertiesCallback.BeginInvoke(void* userdata, uint props, byte* name, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6810) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-89f629dbbed66f9f"></a>[`public virtual void Inno.Native.Sdl3.SDLEnumeratePropertiesCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6810) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e6ff7295152baeb1"></a>[`public virtual void Inno.Native.Sdl3.SDLEnumeratePropertiesCallback.Invoke(void* userdata, uint props, byte* name)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6810) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f579d141e5ce7063"></a>[`public delegate void Inno.Native.Sdl3.SDLEnumeratePropertiesCallback(void* userdata, uint props, byte* name)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6810) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLEnumerationResult</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLEnumerationResult f103095a32b3f68125aa52ad481b97ac7de4ab43029b3f0940c7cf5a9c068028 -->
<!-- api: F:Inno.Native.Sdl3.SDLEnumerationResult.EnumContinue 7e15a66a5cdef353eed767a5ecc62547af38e906ef05e9f8128213f2247cec18 -->
<!-- api: F:Inno.Native.Sdl3.SDLEnumerationResult.EnumSuccess 4cc9940c172cdc15b7e6dda3d890e196ff4d477513ea5e45b96a98544cb28e42 -->
<!-- api: F:Inno.Native.Sdl3.SDLEnumerationResult.EnumFailure df14b92b5bfdc149469854dbb6ca108a6dc3d5911e610d23fba7aadecacc9c97 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-02f9497cb6dad6e9"></a>[`Inno.Native.Sdl3.SDLEnumerationResult.EnumContinue = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1337) | 当前常量或枚举值：`0`。 | — |
| <a id="api-495a72fea63e6cdc"></a>[`Inno.Native.Sdl3.SDLEnumerationResult.EnumFailure = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1339) | 当前常量或枚举值：`2`。 | — |
| <a id="api-923825a7d7829ae6"></a>[`Inno.Native.Sdl3.SDLEnumerationResult.EnumSuccess = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1338) | 当前常量或枚举值：`1`。 | — |
| <a id="api-707c8e886d1bb5b8"></a>[`public enum Inno.Native.Sdl3.SDLEnumerationResult : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1335) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLEnvironment</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLEnvironment ada0f0c45cdf0daa63545acaf8dba28c0dc20ce14794dd3c4a9712aa352cc5ff -->
<!-- api: M:Inno.Native.Sdl3.SDLEnvironment.#ctor(System.IntPtr) 8b5aff95cb4930f020f737c4e8cd6c6c068062c77c211d55ce01264b3129c3fb -->
<!-- api: P:Inno.Native.Sdl3.SDLEnvironment.Handle feb73704fa8dfd54a0036b009d3aa5e183118ffad5ef1581eda5ada42e929398 -->
<!-- api: P:Inno.Native.Sdl3.SDLEnvironment.IsNull 8d8a29839ed2c14c1dd20a09dd5386bb6923accea365ef00aaff65fee005d30f -->
<!-- api: P:Inno.Native.Sdl3.SDLEnvironment.Null ad1cad8e288c5570fe1113226d583341d6dde5aa4e2557af32b0539900161ebe -->
<!-- api: M:Inno.Native.Sdl3.SDLEnvironment.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLEnvironment 308069a8b80fd6cde36da2e03cabdffc728b2c33dc1b94a84064ac77f30f86ad -->
<!-- api: M:Inno.Native.Sdl3.SDLEnvironment.op_Equality(Inno.Native.Sdl3.SDLEnvironment,Inno.Native.Sdl3.SDLEnvironment) aca4ca6b5e70a5d49594324ba27a6c9793e85d583ef09db9b4161480893c93bc -->
<!-- api: M:Inno.Native.Sdl3.SDLEnvironment.op_Inequality(Inno.Native.Sdl3.SDLEnvironment,Inno.Native.Sdl3.SDLEnvironment) 9dc23638455bea3e20e914cae4f1a028c5ba92fa10c2cc181307a5f1e6e54382 -->
<!-- api: M:Inno.Native.Sdl3.SDLEnvironment.Equals(Inno.Native.Sdl3.SDLEnvironment) d7d6f6e9e4e4b0f68ff9938baf44c0f0a4af112f83902e3271e7b1d1b3ef8117 -->
<!-- api: M:Inno.Native.Sdl3.SDLEnvironment.Equals(System.Object) 43884337bc214314c3cbccea247eba3a7d1544476fc4eddefba0990f18ace1ff -->
<!-- api: M:Inno.Native.Sdl3.SDLEnvironment.GetHashCode 6618b8d92510bc9676f7f6e41ab277e074359c848ebc3ff6814f71bbe57e8961 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1032e13fc50025ca"></a>[`public Inno.Native.Sdl3.SDLEnvironment.SDLEnvironment(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1924) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e86c69d3a84b3388"></a>[`public bool Inno.Native.Sdl3.SDLEnvironment.Equals(Inno.Native.Sdl3.SDLEnvironment other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1936) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-b05ec6b5a7a56f04"></a>[`public override bool Inno.Native.Sdl3.SDLEnvironment.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1937) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-1441630c82f31054"></a>[`public override int Inno.Native.Sdl3.SDLEnvironment.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1938) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-594d0998ef795d68"></a>[`public static bool Inno.Native.Sdl3.SDLEnvironment.operator ==(Inno.Native.Sdl3.SDLEnvironment left, Inno.Native.Sdl3.SDLEnvironment right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1934) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d6c9f4614e48051f"></a>[`public static Inno.Native.Sdl3.SDLEnvironment.implicit operator Inno.Native.Sdl3.SDLEnvironment(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1933) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-275f73bc40705c3f"></a>[`public static bool Inno.Native.Sdl3.SDLEnvironment.operator !=(Inno.Native.Sdl3.SDLEnvironment left, Inno.Native.Sdl3.SDLEnvironment right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1935) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5819cc4d6e9dcbda"></a>[`public nint Inno.Native.Sdl3.SDLEnvironment.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1929) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba69d934342f3362"></a>[`public bool Inno.Native.Sdl3.SDLEnvironment.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1930) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1bc66c9dac6ef88b"></a>[`public static Inno.Native.Sdl3.SDLEnvironment Inno.Native.Sdl3.SDLEnvironment.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1931) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-757622e0bf329f05"></a>[`public readonly struct Inno.Native.Sdl3.SDLEnvironment : System.IEquatable<Inno.Native.Sdl3.SDLEnvironment>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1922) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
