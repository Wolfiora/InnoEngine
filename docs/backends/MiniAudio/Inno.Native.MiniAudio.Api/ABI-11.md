# Inno.Native.MiniAudio ABI · MaDeviceInfoPtr → MaFaderConfig

[项目与生命周期](../Inno.Native.MiniAudio.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceInfoPtr</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceInfoPtr 3a068a95fb4582ed628a8cb8b2741adfd533e5660311119b99de1d2bd6fdfb0f -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtr.#ctor(Inno.Native.MiniAudio.MaDeviceInfo*) 37e76b96a11d2dc74f106214157c8698a61ff5109fd72a796c296f3d6d1c635c -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceInfoPtr.Handle c6c9526fbb4fd19624a78dbc0579cccfc27904a8bf54d17bdfb36c87d3693a6c -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceInfoPtr.IsNull 78af8d9398bbdd87266f3ce61c82c650750f1cc0900f814ccdc0aa4eb4453a4e -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceInfoPtr.Null a7a7a6184d29ac9092c37a7159386eee66cb5fd48ca099656c202a65d8f207ef -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceInfoPtr.Item(System.Int32) 8d1212f85f40a0cb96c0c86e5272f8fe3dc382da9369f9df7db31306a63aca63 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtr.op_Implicit(Inno.Native.MiniAudio.MaDeviceInfo*)~Inno.Native.MiniAudio.MaDeviceInfoPtr 5d27ceef8a1457a6ab0fd5f7c3e2bf0e0dc1f965afe0a6a827b656fe8d6d96a8 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtr.op_Implicit(Inno.Native.MiniAudio.MaDeviceInfoPtr)~Inno.Native.MiniAudio.MaDeviceInfo* 205d871b3604dca5307177dbbb53f797d1702e85d9dfa9148015a63ab1059510 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtr.op_Equality(Inno.Native.MiniAudio.MaDeviceInfoPtr,Inno.Native.MiniAudio.MaDeviceInfoPtr) f9949aab1e7c93f31c65e22129e2cb1bd7982fd06f790e4e8f802914675af2d3 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtr.op_Inequality(Inno.Native.MiniAudio.MaDeviceInfoPtr,Inno.Native.MiniAudio.MaDeviceInfoPtr) 0bbe807ca1fbd93339b548adb5b784e92c9cdd34e6b585ee60c58cc538b6d17d -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtr.Equals(Inno.Native.MiniAudio.MaDeviceInfoPtr) b692d021c256dfed87951c911b6b47743fae3de77f4b79d85da56f88cf6237b3 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtr.Equals(System.Object) e0910e54bdae1df7a4d3e251b1b50c1368afcacb9c5ee864d5eb669837b9ce29 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtr.GetHashCode 3e5f600e3003f9e49b187536fb362a11f36dd552832caac9baa507eb9b6361ef -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceInfoPtr.Id 5a18573798b0523c65797b60d65530fb511f007d7e19652372ef24d549f5ed9e -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceInfoPtr.Name 00c8fd8313a417cbf68df0f6f25bf20f08548eed52bd642fae3a3d220ff0ea13 -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceInfoPtr.IsDefault fef6ceb9d3cd3b50fd81611eeec8adb28abc16743e07653f10bc409eb2d20d2d -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceInfoPtr.NativeDataFormatCount 23d80373755cd8502343f4aef1519eabb5fd770331ac3c4853c13f7dfa73a421 -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceInfoPtr.NativeDataFormats e946add1aed9984534c7c086ca24f3d46e68c6fd0de80787d496c540c8ec4047 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c5e00d94e59af289"></a>[`public Inno.Native.MiniAudio.MaDeviceInfo* Inno.Native.MiniAudio.MaDeviceInfoPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15581) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e64a642506c0cf3"></a>[`public Inno.Native.MiniAudio.MaDeviceInfoPtr.MaDeviceInfoPtr(Inno.Native.MiniAudio.MaDeviceInfo* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15576) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8b595cce62b9b23d"></a>[`public bool Inno.Native.MiniAudio.MaDeviceInfoPtr.Equals(Inno.Native.MiniAudio.MaDeviceInfoPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15591) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-e363f39784e95476"></a>[`public override bool Inno.Native.MiniAudio.MaDeviceInfoPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15592) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-5f3db281e77840e2"></a>[`public override int Inno.Native.MiniAudio.MaDeviceInfoPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15593) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-44614983b80b3657"></a>[`public static bool Inno.Native.MiniAudio.MaDeviceInfoPtr.operator ==(Inno.Native.MiniAudio.MaDeviceInfoPtr left, Inno.Native.MiniAudio.MaDeviceInfoPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15589) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8e39541eee6cdcc2"></a>[`public static Inno.Native.MiniAudio.MaDeviceInfoPtr.implicit operator Inno.Native.MiniAudio.MaDeviceInfoPtr(Inno.Native.MiniAudio.MaDeviceInfo* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15587) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e72b5e6467ad077"></a>[`public static Inno.Native.MiniAudio.MaDeviceInfoPtr.implicit operator Inno.Native.MiniAudio.MaDeviceInfo*(Inno.Native.MiniAudio.MaDeviceInfoPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15588) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-596a8335f58e0ee6"></a>[`public static bool Inno.Native.MiniAudio.MaDeviceInfoPtr.operator !=(Inno.Native.MiniAudio.MaDeviceInfoPtr left, Inno.Native.MiniAudio.MaDeviceInfoPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15590) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f17e828584b6e865"></a>[`public ref Inno.Native.MiniAudio.MaDeviceId Inno.Native.MiniAudio.MaDeviceInfoPtr.Id { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15595) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0bc0d6441e12ddd3"></a>[`public ref uint Inno.Native.MiniAudio.MaDeviceInfoPtr.IsDefault { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15597) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1baaae738ac024cc"></a>[`public bool Inno.Native.MiniAudio.MaDeviceInfoPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15582) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1b2661d1fe94fa20"></a>[`public Inno.Native.MiniAudio.MaDeviceInfo Inno.Native.MiniAudio.MaDeviceInfoPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15585) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2a90f5d6e6047c7c"></a>[`public System.Span<byte> Inno.Native.MiniAudio.MaDeviceInfoPtr.Name { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15596) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c2c2f745fecc40e2"></a>[`public ref uint Inno.Native.MiniAudio.MaDeviceInfoPtr.NativeDataFormatCount { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15598) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-775271b9f955d1fd"></a>[`public System.Span<Inno.Native.MiniAudio.MaDeviceInfo.NativeDataFormat> Inno.Native.MiniAudio.MaDeviceInfoPtr.NativeDataFormats { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15599) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1e96ba3e393da947"></a>[`public static Inno.Native.MiniAudio.MaDeviceInfoPtr Inno.Native.MiniAudio.MaDeviceInfoPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15583) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cc11b446bba51559"></a>[`public struct Inno.Native.MiniAudio.MaDeviceInfoPtr : System.IEquatable<Inno.Native.MiniAudio.MaDeviceInfoPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15574) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceInfoPtrPtr</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceInfoPtrPtr ebe85f3b23dcc3eac52e3f2af57999def3068117a8a80514636c3f9322384f40 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.#ctor(Inno.Native.MiniAudio.MaDeviceInfo**) 42fd7a39385566c676c80d85a826f002d1cfab3041673b397602db2ca51defac -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.Handle d4b5b1de44110a6991f374f4bb2e4e09a8b5d51d17351e2a30921739adcdf48d -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.IsNull a3e9b96c64785f2c9bfdf580a2a6e128afb00151ee832511c7273512b4175a3f -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.Null 8e18371ed6e029ee67fb29ca2387c7ee58fe24ce2009226d0b23f24b6605db44 -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.Item(System.Int32) 969ad61f6eb15e0696e62c87738267d13ea2335abc4fc134617845a82685e874 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.op_Implicit(Inno.Native.MiniAudio.MaDeviceInfo**)~Inno.Native.MiniAudio.MaDeviceInfoPtrPtr ebeb3460bb75a886c7ee2ec3c4b8b46fdba4a978454ca57fc874aa33f0f985fa -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.op_Implicit(Inno.Native.MiniAudio.MaDeviceInfoPtrPtr)~Inno.Native.MiniAudio.MaDeviceInfo** 791ca1193af812fd046cad86aee70f8033b568eea2ae0288351f831de0548169 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.op_Equality(Inno.Native.MiniAudio.MaDeviceInfoPtrPtr,Inno.Native.MiniAudio.MaDeviceInfoPtrPtr) b4f98a60021e0a50219d0cdf8782a2da9f0c3b996f04be488e917ee3d73ef499 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.op_Inequality(Inno.Native.MiniAudio.MaDeviceInfoPtrPtr,Inno.Native.MiniAudio.MaDeviceInfoPtrPtr) 2ece63d2edfb8c8211deb095005a6af666cc527ac505230d7de5045f6eae471a -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.Equals(Inno.Native.MiniAudio.MaDeviceInfoPtrPtr) 5d80cf7fe9d3a148bb4b0cca7b6e9712f0faa13ad7884ac9614ac13ebb5989cb -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.Equals(System.Object) 75be7af80294e6f632e3410c7d3ec72b375fc8a4bc372973c07979fb3da6dd30 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.GetHashCode 682b6fd3b9149439351a1453a5c330cef2ae541b0c4aa734a762f687d66e73d2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7be91b4c350fc5d3"></a>[`public Inno.Native.MiniAudio.MaDeviceInfo** Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15610) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-885bbc2e056caf63"></a>[`public Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.MaDeviceInfoPtrPtr(Inno.Native.MiniAudio.MaDeviceInfo** handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15605) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8621676b1db726f8"></a>[`public bool Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.Equals(Inno.Native.MiniAudio.MaDeviceInfoPtrPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15620) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-da6638bc4eecd6f0"></a>[`public override bool Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15621) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-e4214e84ef6741e8"></a>[`public override int Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15622) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-bbc520df5d19547e"></a>[`public static bool Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.operator ==(Inno.Native.MiniAudio.MaDeviceInfoPtrPtr left, Inno.Native.MiniAudio.MaDeviceInfoPtrPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15618) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-28693cde2bce5dd8"></a>[`public static Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.implicit operator Inno.Native.MiniAudio.MaDeviceInfoPtrPtr(Inno.Native.MiniAudio.MaDeviceInfo** handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15616) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fa5467cf535177a4"></a>[`public static Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.implicit operator Inno.Native.MiniAudio.MaDeviceInfo**(Inno.Native.MiniAudio.MaDeviceInfoPtrPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15617) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4ddf4b480db196b9"></a>[`public static bool Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.operator !=(Inno.Native.MiniAudio.MaDeviceInfoPtrPtr left, Inno.Native.MiniAudio.MaDeviceInfoPtrPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15619) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c29e12a971717bba"></a>[`public bool Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15611) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c907ada80889a43e"></a>[`public Inno.Native.MiniAudio.MaDeviceInfo* Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15614) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7f7334aebdd7f542"></a>[`public static Inno.Native.MiniAudio.MaDeviceInfoPtrPtr Inno.Native.MiniAudio.MaDeviceInfoPtrPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15612) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0c88c77686b96f5a"></a>[`public struct Inno.Native.MiniAudio.MaDeviceInfoPtrPtr : System.IEquatable<Inno.Native.MiniAudio.MaDeviceInfoPtrPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15603) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceJobThread</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceJobThread 881e335c1ef38cbcdb577f42effae9c4adbcc8f00aa7e752fd4ace735547fdc1 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceJobThread.Thread 5962354e1f19ca9cc2750892249722c11eea50f86f3b73b2e95cea828cce2ca9 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceJobThread.JobQueue 2dfce99fc72a27c6dc70be3ea18a6fcd5ed48db7a23780b31db956bcc7511722 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceJobThread.Hasthread 6342c2edd118155ca878b6963baa270a8fa6055932833a13694f1156eccb4c83 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThread.#ctor(System.IntPtr,Inno.Native.MiniAudio.MaJobQueue,System.UInt32) 8dfd876b1bae7e16b71e628d77d359a938984908f1fed3a7840bb6811699d955 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3f5709e4932f2960"></a>[`public uint Inno.Native.MiniAudio.MaDeviceJobThread.Hasthread`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11962) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d01c1206173d0f90"></a>[`public Inno.Native.MiniAudio.MaJobQueue Inno.Native.MiniAudio.MaDeviceJobThread.JobQueue`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11961) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4351fb0f95fee545"></a>[`public nint Inno.Native.MiniAudio.MaDeviceJobThread.Thread`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11960) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a32f84451d265e10"></a>[`public Inno.Native.MiniAudio.MaDeviceJobThread.MaDeviceJobThread(nint thread = 0, Inno.Native.MiniAudio.MaJobQueue jobQueue = default(Inno.Native.MiniAudio.MaJobQueue), uint hasThread = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11963) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-be24e4f74249347b"></a>[`public struct Inno.Native.MiniAudio.MaDeviceJobThread`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11958) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceJobThreadConfig</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceJobThreadConfig 4d35414747ed924a239010adede4c485deaf58f6cce142f6e5f6982d4e0e996f -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceJobThreadConfig.NoThread 48c386e17be14573c21f04018dd54adef4c7020d8dca25990470bf5e61a3b897 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceJobThreadConfig.JobQueueCapacity b36bb1dd378b7190d140c8be9bdb398263b1446d4e6086215e8e0c01ff3e7d23 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceJobThreadConfig.JobQueueFlags 6363a310342caf03dfca4aa8aafac6ee08693592aa72190086accfca222ccece -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadConfig.#ctor(System.UInt32,System.UInt32,System.UInt32) 36e9803c081c7026cb1e2a014cc4031c0bd1513e517e9a335587033b0ed18ee2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8af0b128376b620a"></a>[`public uint Inno.Native.MiniAudio.MaDeviceJobThreadConfig.JobQueueCapacity`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11944) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4aec20173f249cf9"></a>[`public uint Inno.Native.MiniAudio.MaDeviceJobThreadConfig.JobQueueFlags`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11945) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-08db119719fe2f20"></a>[`public uint Inno.Native.MiniAudio.MaDeviceJobThreadConfig.NoThread`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11943) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-17f35167ef2d5c9e"></a>[`public Inno.Native.MiniAudio.MaDeviceJobThreadConfig.MaDeviceJobThreadConfig(uint noThread = 0, uint jobQueueCapacity = 0, uint jobQueueFlags = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11946) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7fe1950ba9f091c8"></a>[`public struct Inno.Native.MiniAudio.MaDeviceJobThreadConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11941) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr 2a862be42df79c30d6b1c98dc688eb0817e2af5131f407332db243bd0b4696b1 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.#ctor(Inno.Native.MiniAudio.MaDeviceJobThreadConfig*) 0725c21c0ba3d823d9a6dd5be4d7fc5d1fec1f8630399d42ffb376fb4ffa5903 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.Handle 4c1830acacc5b3eecdeaf75e0742390a1036e14dd44ba1ed1f4889b3e596d6c6 -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.IsNull 191cd2b506b40379dbc87537c6b74871b9177f912807218a5451cf118c610b18 -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.Null 50762a689a4d315cff53b5e688452161fbcb8d16086a5663e3f2a589e7768ef0 -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.Item(System.Int32) 0fc1699122a4e4ceae2973717cfbb316979294e110824937b58b9e72dd963c4e -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaDeviceJobThreadConfig*)~Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr 97f793b3d142292a8a1fa81626c6edefc1cc910cbdf6f999a722f94f3c0eeb0f -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr)~Inno.Native.MiniAudio.MaDeviceJobThreadConfig* d25536698e15c028793469d98c3138579fb44f9bf5c810f75f41fa83777ec20d -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.op_Equality(Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr,Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr) fddccdbe6441e627457089dd48274968891d55f9cab19cfbeea9f729c4a132b7 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr,Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr) 29bb466e5b30b2a5baeaf231c3db225ed777b24ece627299878bb25a41448833 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.Equals(Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr) 5118eef510eea1a476e75012450f6e1cc4049ef3cd8b112d582d225729cd7563 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.Equals(System.Object) 50f251a550b21134b5763b45ea67516b98379845aa616d12a14e99f1dc4ed450 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.GetHashCode 36c6f105941d91b0ab85675b4bd764dd7e05fc0528431c6548f92aaa44cbc9bd -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.NoThread 6ec0a62ea0cdcd3744732e5d41091ad2c7771f4b290c77804295072503006c03 -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.JobQueueCapacity c7c0b86fe8467b5f0e8469f9da6d427ee1bf1cf590d8a5a1c6ae56dcda7f766a -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.JobQueueFlags e41a00254214f6328044e3cef52f1f95b5e3a00ac278c0f4d632ba5d0057c193 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-87e7e33521b931ac"></a>[`public Inno.Native.MiniAudio.MaDeviceJobThreadConfig* Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15634) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-043e7e6e6ffb5d3b"></a>[`public Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.MaDeviceJobThreadConfigPtr(Inno.Native.MiniAudio.MaDeviceJobThreadConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15629) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1809bed6f98a99e6"></a>[`public bool Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.Equals(Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15644) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-3ef8286431d7680d"></a>[`public override bool Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15645) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-91a8e4c327ad7f11"></a>[`public override int Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15646) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-1ba807da23f9b963"></a>[`public static bool Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.operator ==(Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr left, Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15642) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-08cfa1f43ad45c11"></a>[`public static Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.implicit operator Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr(Inno.Native.MiniAudio.MaDeviceJobThreadConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15640) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6df53f84ad76d496"></a>[`public static Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.implicit operator Inno.Native.MiniAudio.MaDeviceJobThreadConfig*(Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15641) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-29b2c7501ec02e31"></a>[`public static bool Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.operator !=(Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr left, Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15643) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-66f0eff8500c6c77"></a>[`public bool Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15635) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7d87a2d6f4cd2f81"></a>[`public Inno.Native.MiniAudio.MaDeviceJobThreadConfig Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15638) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-256847011aa43c00"></a>[`public ref uint Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.JobQueueCapacity { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15649) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9b04af32c3315e38"></a>[`public ref uint Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.JobQueueFlags { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15650) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b3c12c46f49a11a7"></a>[`public ref uint Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.NoThread { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15648) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-30fbbaacec766e16"></a>[`public static Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15636) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1ad05df98be38071"></a>[`public struct Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaDeviceJobThreadConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15627) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceJobThreadPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceJobThreadPtr 7464710f6a664ec362e9b547857e36b0266e79ddd485588be4e100bf0619d76a -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.#ctor(Inno.Native.MiniAudio.MaDeviceJobThread*) c249bd1be601b05f7836530de45571bb119dd0f612bb71c33688c999e6e6ee1d -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.Handle e72d40cc1b29e39e6105b6473f4e40976e8f0faf2f7dc34f5a15860d0e3bb70e -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.IsNull 19081ead7e1309496f4c0dc36c3d8957e3e2a7f570faf97bdda70e603fed3a89 -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.Null e5ec411edbf01850404ba9ac64796091bcbe3175f571fb2a81581dfc274d3541 -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.Item(System.Int32) 10d967158a4cbd8a73ce9b806784449e0472ee0d958b5b5458e64576a9ee41c9 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.op_Implicit(Inno.Native.MiniAudio.MaDeviceJobThread*)~Inno.Native.MiniAudio.MaDeviceJobThreadPtr d97bb347e9ab29143a7feec39c5eae3f1541173721ad915aa8be17aea8abf4c4 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.op_Implicit(Inno.Native.MiniAudio.MaDeviceJobThreadPtr)~Inno.Native.MiniAudio.MaDeviceJobThread* 384ad4b87485ac91a9e4624a7939f277cd198d7eb2a05dc98d4ebb220c1744de -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.op_Equality(Inno.Native.MiniAudio.MaDeviceJobThreadPtr,Inno.Native.MiniAudio.MaDeviceJobThreadPtr) 9f81ae0a30ebd5c891c44b779b337158e9993b00cdf0ff1551cdfbb3759ee6d6 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.op_Inequality(Inno.Native.MiniAudio.MaDeviceJobThreadPtr,Inno.Native.MiniAudio.MaDeviceJobThreadPtr) fd763eee20cb3d36532b3032e032076f9b8b0ab7629a96f0aa191c80dacde6ed -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.Equals(Inno.Native.MiniAudio.MaDeviceJobThreadPtr) db232b018971d7306517761be247ff0db7b4dfdf3253967189baf65f308bff76 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.Equals(System.Object) 497f579ef9debda30ad42566d0fee1d9fd54c7534c7b16e64b4258a7ed038e5d -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.GetHashCode 8955a64277b2e8754f29f116f116c2238757f820fbb62c5251e0e2cee0b1c96e -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.Thread d1ef37540e326ee07bee057bc4641d630621546ae3f80afb2280175df194806e -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.JobQueue fb8fd7aee5f8cd0827a7292d09bbb2964d94f8f6be934335b7a8d5fece000d2c -->
<!-- api: P:Inno.Native.MiniAudio.MaDeviceJobThreadPtr.Hasthread f0a0850170a7e9059ae0999e2acdd8e77778a77a59f864a334f62b377d06b5a0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4a1522088bc466d0"></a>[`public Inno.Native.MiniAudio.MaDeviceJobThread* Inno.Native.MiniAudio.MaDeviceJobThreadPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15661) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccfc4c07d3e1eec8"></a>[`public Inno.Native.MiniAudio.MaDeviceJobThreadPtr.MaDeviceJobThreadPtr(Inno.Native.MiniAudio.MaDeviceJobThread* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15656) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e5427d6ec0ecfd60"></a>[`public bool Inno.Native.MiniAudio.MaDeviceJobThreadPtr.Equals(Inno.Native.MiniAudio.MaDeviceJobThreadPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15671) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-3a603299ce434647"></a>[`public override bool Inno.Native.MiniAudio.MaDeviceJobThreadPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15672) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-49ad19cda8035be3"></a>[`public override int Inno.Native.MiniAudio.MaDeviceJobThreadPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15673) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-cac74015a2f5786b"></a>[`public static bool Inno.Native.MiniAudio.MaDeviceJobThreadPtr.operator ==(Inno.Native.MiniAudio.MaDeviceJobThreadPtr left, Inno.Native.MiniAudio.MaDeviceJobThreadPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15669) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-18c7086f68fd664a"></a>[`public static Inno.Native.MiniAudio.MaDeviceJobThreadPtr.implicit operator Inno.Native.MiniAudio.MaDeviceJobThreadPtr(Inno.Native.MiniAudio.MaDeviceJobThread* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15667) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4157286692285ad6"></a>[`public static Inno.Native.MiniAudio.MaDeviceJobThreadPtr.implicit operator Inno.Native.MiniAudio.MaDeviceJobThread*(Inno.Native.MiniAudio.MaDeviceJobThreadPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15668) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ddffa7f36bf5d128"></a>[`public static bool Inno.Native.MiniAudio.MaDeviceJobThreadPtr.operator !=(Inno.Native.MiniAudio.MaDeviceJobThreadPtr left, Inno.Native.MiniAudio.MaDeviceJobThreadPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15670) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4eaa30daa188ae48"></a>[`public ref uint Inno.Native.MiniAudio.MaDeviceJobThreadPtr.Hasthread { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15677) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-95b7b9f64da6dc56"></a>[`public bool Inno.Native.MiniAudio.MaDeviceJobThreadPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15662) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-862010ec6d377bc5"></a>[`public Inno.Native.MiniAudio.MaDeviceJobThread Inno.Native.MiniAudio.MaDeviceJobThreadPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15665) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6cfafeb8adaa11c8"></a>[`public ref Inno.Native.MiniAudio.MaJobQueue Inno.Native.MiniAudio.MaDeviceJobThreadPtr.JobQueue { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15676) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6a14ad87c2e446af"></a>[`public static Inno.Native.MiniAudio.MaDeviceJobThreadPtr Inno.Native.MiniAudio.MaDeviceJobThreadPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15663) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d04dd80db2dbba52"></a>[`public ref nint Inno.Native.MiniAudio.MaDeviceJobThreadPtr.Thread { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15675) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1420c44221573b10"></a>[`public struct Inno.Native.MiniAudio.MaDeviceJobThreadPtr : System.IEquatable<Inno.Native.MiniAudio.MaDeviceJobThreadPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15654) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceNotification</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceNotification 6b8ad5b5c366c2410c08600aa2222599edd3518e3509520effcf8b274bb2b2fa -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotification.PDevice 55168f2c6d00d7dea3509198ef55059d5781ddf56f173b2894f52813960f9857 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotification.Type 6cf9bb06bed1e880a59c878dba60a3b99a282117376f9c557c0f93d149a15c20 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotification.Data d29a796bf4c076328521f91b44feb406837837968ab2ba39d6c3c1fe4ce9d768 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceNotification.#ctor(Inno.Native.MiniAudio.MaDevice*,Inno.Native.MiniAudio.MaDeviceNotificationType,Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous) 867813b665db5a2a45287255e2d85ae1a9fc1f1c920b3f03ae8f6b38a2ac6449 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-674f25c5dc91f858"></a>[`public Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous Inno.Native.MiniAudio.MaDeviceNotification.Data`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5181) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03f2ca2d9030b88c"></a>[`public Inno.Native.MiniAudio.MaDevice* Inno.Native.MiniAudio.MaDeviceNotification.PDevice`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5179) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da92237d3cfb1475"></a>[`public Inno.Native.MiniAudio.MaDeviceNotificationType Inno.Native.MiniAudio.MaDeviceNotification.Type`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5180) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a5541bf5e539460f"></a>[`public Inno.Native.MiniAudio.MaDeviceNotification.MaDeviceNotification(Inno.Native.MiniAudio.MaDevice* pDevice = null, Inno.Native.MiniAudio.MaDeviceNotificationType type = Inno.Native.MiniAudio.MaDeviceNotificationType.Started, Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous data = default(Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5182) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6b6c474049f9e947"></a>[`public struct Inno.Native.MiniAudio.MaDeviceNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5109) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous c9cde0938ae330eb444f6505d0d1eed38dbfdfff8d2e0ffcde0b6f7be777f99d -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.Started a54bbffedb722db7527373507cf932a6d48863d5ebde4834aed3daf76b4342e8 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.Stopped 2525234b4c5e634058ea2267d03a0bd7d38810057fc8423957a5fa84afd58c8f -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.Rerouted be3a01c2ef38946c01e45bca8564d918916a07dd5c8c37ca4237a79e824f40cb -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.Interruption 07a1566152821c515fcb67b035936e6dd9b1af5c12e6e0740df67235be067119 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d122cf4a7b96a0d6"></a>[`public Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.InterruptionAnonymous Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.Interruption`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5176) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1258de3978700533"></a>[`public Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.ReroutedAnonymous Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.Rerouted`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5174) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b0548c262bcf0e0c"></a>[`public Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StartedAnonymous Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.Started`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5170) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2627499083b5c511"></a>[`public Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StoppedAnonymous Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.Stopped`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5172) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a84a2971f7f802b3"></a>[`public struct Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5115) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.InterruptionAnonymous</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.InterruptionAnonymous 9895c6df6b28f5c71815d903a5187d85fba415eb022359fd7a002daba80f9e9a -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.InterruptionAnonymous.Unused aafb3160a228fbb57f2af7349383570480452e636537a07d6e7fb79fc2371450 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.InterruptionAnonymous.#ctor(System.Int32) 063bff0803383d480341962a3b9a002cce2dd67f7d975af7710d1f505acfeb1e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-cb4ca79e64152666"></a>[`public int Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.InterruptionAnonymous.Unused`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5162) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-10bd1bc700c9cdc4"></a>[`public Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.InterruptionAnonymous.InterruptionAnonymous(int unused = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5163) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b9882b3baf23b577"></a>[`public struct Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.InterruptionAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5160) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.ReroutedAnonymous</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.ReroutedAnonymous 8559b02eb9c983d14c750f93b19c21d7cf1bbad5ae0a060e89efad0b46f964c6 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.ReroutedAnonymous.Unused d62d81e4187cc749a783b3166e63394b689c16ece11fb505326333bd58e7ef56 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.ReroutedAnonymous.#ctor(System.Int32) 36cf929c02f8b48374fd169109fdf12f5b46a935ad3dc89ad63ec2768abeac64 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ff06bb863ed6c845"></a>[`public int Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.ReroutedAnonymous.Unused`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5149) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-09416444d5959f0f"></a>[`public Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.ReroutedAnonymous.ReroutedAnonymous(int unused = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5150) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6ad0b9bdab3b4213"></a>[`public struct Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.ReroutedAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5147) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StartedAnonymous</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StartedAnonymous dc0869549af72bcdbed0d6b6399c4fa48abb9cc4110bf32c0a844677dccf595d -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StartedAnonymous.Unused 34182310de43547a2705752db89ed533a39d34375448aeeede26caf50fc019d7 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StartedAnonymous.#ctor(System.Int32) 8d6196e0e716c3df6bdb615664f996d95bfe73c09348eb399659501f13d245d6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a978f680b1edb56d"></a>[`public int Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StartedAnonymous.Unused`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5123) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-763a8a1ab719ad46"></a>[`public Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StartedAnonymous.StartedAnonymous(int unused = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5124) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7f1217c34e45b2f5"></a>[`public struct Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StartedAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5121) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StoppedAnonymous</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StoppedAnonymous dedfe18dd3b5954be0561768e8b8a26eaa24113c5336c86fe29752af9ec59d55 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StoppedAnonymous.Unused a498be2d7e8dab50489f5708db506b9b1e39ca421533a2b6dc88ba25e1859ce9 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StoppedAnonymous.#ctor(System.Int32) df2fa1f9755b88a741ac8ef6cae743683c86b360fa58ddcad5bbfdecc23b3cdf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7db24c1a3cb80b86"></a>[`public int Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StoppedAnonymous.Unused`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5136) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cb105c8b7cd6a92b"></a>[`public Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StoppedAnonymous.StoppedAnonymous(int unused = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5137) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a5402c2dd45767cb"></a>[`public struct Inno.Native.MiniAudio.MaDeviceNotification.DataAnonymous.StoppedAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5134) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceNotificationProc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceNotificationProc 021de3079fecbccebbb4bcded8cf70e01fb3b667e2aee028ee9e9986db870748 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceNotificationProc.#ctor(System.Object,System.IntPtr) 9097b571f1d61f7f102dad4a88e332229ee7da613e4c6204218b55f52a0cbcfc -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceNotificationProc.Invoke(Inno.Native.MiniAudio.MaDeviceNotification*) f830316968c7a5825c3247dbefdb241a6890dc39ca1a061eb96330fabe291541 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceNotificationProc.BeginInvoke(Inno.Native.MiniAudio.MaDeviceNotification*,System.AsyncCallback,System.Object) 004cd9fb0202740d133e706955ad02d1a86c388ac1115384731568a4351c9105 -->
<!-- api: M:Inno.Native.MiniAudio.MaDeviceNotificationProc.EndInvoke(System.IAsyncResult) b16cf0c275b1c1bfb955c042a7fc7c87c46bb4cf14cf2ff7d1da377efed84fbb -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5327585efd9154cb"></a>[`public Inno.Native.MiniAudio.MaDeviceNotificationProc.MaDeviceNotificationProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13906) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d07c69357d5805eb"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaDeviceNotificationProc.BeginInvoke(Inno.Native.MiniAudio.MaDeviceNotification* pNotification, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13906) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-96184b1dae5f078d"></a>[`public virtual void Inno.Native.MiniAudio.MaDeviceNotificationProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13906) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e2bcb0a15e41fb8b"></a>[`public virtual void Inno.Native.MiniAudio.MaDeviceNotificationProc.Invoke(Inno.Native.MiniAudio.MaDeviceNotification* pNotification)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13906) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aafbafa51c7d6df4"></a>[`public delegate void Inno.Native.MiniAudio.MaDeviceNotificationProc(Inno.Native.MiniAudio.MaDeviceNotification* pNotification)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13906) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceNotificationType</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceNotificationType 7e195c50b4a137fc0366ed8f5c26c171e191b608f379b9fbbad0057e7bab349b -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotificationType.Started 2d352884f46632b4522896bd6004738f756f8dfb2750e6b6ab437fb72472e7c1 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotificationType.Stopped d1c63abbd68ce1a1694ac7a2b1697a3ce7a5f31fbea70285fecfc3b8772434b0 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotificationType.Rerouted f938349c5a46bc77084637a5ee7428efe5f1e5f4b3d5276bf3c4c45662fda512 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotificationType.InterruptionBegan b02f75c94f5b8c452ce2bac5b081722543cdb65ee7238e91fbd8669248848903 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotificationType.InterruptionEnded 789445e8bb640346361f0c67da1be0c387d3d3d50c806bcd33134e1839bb61ad -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceNotificationType.Unlocked 58b939e342d11f520b57ea81f33fe4d465a8d6292755e9e71f233af88005a1c4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2fd6399d3cc467de"></a>[`Inno.Native.MiniAudio.MaDeviceNotificationType.InterruptionBegan = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L214) | 当前常量或枚举值：`3`。 | — |
| <a id="api-34dd09330b83c0ff"></a>[`Inno.Native.MiniAudio.MaDeviceNotificationType.InterruptionEnded = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L215) | 当前常量或枚举值：`4`。 | — |
| <a id="api-83e03b58429a8d38"></a>[`Inno.Native.MiniAudio.MaDeviceNotificationType.Rerouted = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L213) | 当前常量或枚举值：`2`。 | — |
| <a id="api-dcb7c84d9aac30c7"></a>[`Inno.Native.MiniAudio.MaDeviceNotificationType.Started = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L211) | 当前常量或枚举值：`0`。 | — |
| <a id="api-4647cf1a2129ebe0"></a>[`Inno.Native.MiniAudio.MaDeviceNotificationType.Stopped = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L212) | 当前常量或枚举值：`1`。 | — |
| <a id="api-8e74d4899780f48d"></a>[`Inno.Native.MiniAudio.MaDeviceNotificationType.Unlocked = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L216) | 当前常量或枚举值：`5`。 | — |
| <a id="api-4567d643dc037753"></a>[`public enum Inno.Native.MiniAudio.MaDeviceNotificationType : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L209) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDevicePtr</code> · 38 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDevicePtr 2a67b981434faebd51869fcebfdb8d683348a20acb4a319d44ac424845e7115a -->
<!-- api: M:Inno.Native.MiniAudio.MaDevicePtr.#ctor(Inno.Native.MiniAudio.MaDevice*) 182ef4c52c8f70c5564548d26bf76091856dfc13672c879f4f3d72b22b635cc0 -->
<!-- api: F:Inno.Native.MiniAudio.MaDevicePtr.Handle 9ed16f3f03ea70179df066954088f3a3b85fdbe5a2aa58720559ae12e3bdfa59 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.IsNull 407139dacd642ea55d53d3db36c4020118b921f66150225185958d734f214213 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.Null ad34ec506309911c221da0a93f78d23bd101161c71e241381ab8a8940206a822 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.Item(System.Int32) 67476d5716eef2d822cf3ba2ecd05c67841b00205fa50bad276668e8a69e7018 -->
<!-- api: M:Inno.Native.MiniAudio.MaDevicePtr.op_Implicit(Inno.Native.MiniAudio.MaDevice*)~Inno.Native.MiniAudio.MaDevicePtr 32576f71b325e8147e98e3a6980c234221e3026cdc58ea7731ecb2ff97520de4 -->
<!-- api: M:Inno.Native.MiniAudio.MaDevicePtr.op_Implicit(Inno.Native.MiniAudio.MaDevicePtr)~Inno.Native.MiniAudio.MaDevice* 21becf61e964fbce23af312245e7885f90eb38ba9b1355e5335a3d460d4875db -->
<!-- api: M:Inno.Native.MiniAudio.MaDevicePtr.op_Equality(Inno.Native.MiniAudio.MaDevicePtr,Inno.Native.MiniAudio.MaDevicePtr) d6bd4911088a988698df7054b089da30e83de8aacf379610836063d52d01f9bb -->
<!-- api: M:Inno.Native.MiniAudio.MaDevicePtr.op_Inequality(Inno.Native.MiniAudio.MaDevicePtr,Inno.Native.MiniAudio.MaDevicePtr) 4db310553027d396d5a3b483ae637ab688ff445c5fc317cee618cf88cd97ebad -->
<!-- api: M:Inno.Native.MiniAudio.MaDevicePtr.Equals(Inno.Native.MiniAudio.MaDevicePtr) 96e288a50db85db5af3d0183f7d55ea58f1d2cc25e7bb2d7be91c47fe08b902e -->
<!-- api: M:Inno.Native.MiniAudio.MaDevicePtr.Equals(System.Object) c93ab79c14c171ca9f8d3e9701ad59c8a83ec633996224f25d7bd75a63d34bd4 -->
<!-- api: M:Inno.Native.MiniAudio.MaDevicePtr.GetHashCode af354842dcae4e6476950229d167489cd6c66ba04c155780708be9c7ee5acb63 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.PContext f33f26e6c0c7f614c7a11514df3b4a7eb613177e3eb04a5984c92976e2e913eb -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.Type 89bbb3e574221e5ac99abba949f7da3110d06c5a65438fbc73e4b90bbec2dffa -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.SampleRate 8d8356b0046c0b6ee36638ca823b3507d787b87892851f1e9a5edc5f9ba2e8bf -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.State 5d6288009be5c11f9e0e51c9519619eda89048184b1d083e370e30361b839d54 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.OnData b2c3a4e4f3ebb8e9c5e84f4fd9c98495f08ab7eb10312f36e76e2b596fa36a8e -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.OnNotification 70123468c8c11c98b6b8e7b2099e9a40f27276d7586b8a948776bbde577f71c8 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.OnStop 8e364eea0105a9c4070144d87acb5cf3c625a8c9582572f8d49556cfb74c1ed0 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.PUserData ed66de194eff1f438c73c8310b74edab46b7f0f1a6d29b8c72b0f66dd13eedd0 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.StartStopLock 0752d2303cbe6cb40181e094617fd88a24eee0a88957bbb03528735fd197d8ed -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.WakeupEvent 2eff03ca83dc71883495f97ad86666916314c6444d298baf075163b4420e1234 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.StartEvent 9abc013aa890eae9a21163fa90d0d57ef0dc16ae2c44b9d544eeea4d526532a9 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.StopEvent 4e4efa06e877c7c8960af59343d66533f2e7c1961c8517f16571a3bfb28bd2c0 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.Thread 4b0b170b94504575cd7e41a3a49b80a7c7387b427f441106e2f18702d2ba4dec -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.WorkResult 9d72fd221d8757b4c5b90c6e3c1ef7db9ed88a02094c2cc493d209903cbb5d74 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.IsOwnerOfContext 44bd6c512179831608cf4a123971f15538d955f5d7abe52be4dbafe5ba0b182b -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.NoPreSilencedOutputBuffer 7e1b421f37e253b86d193305f75f52cb207b4f540dc21ac1fe6691be428e9b68 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.NoClip 9133ff1e5be0e47d806002f13e7790acb59c0cfb051dc5dfdbf3f9f373e1f6a3 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.NoDisableDenormals bc093a927909e54ac7c19cadb5c80e49c68207047fb968174748804e1b0213b8 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.NoFixedSizedCallback 073788b2145e93e6754b996ae0660e487202c3a75246c1459fa2a6b555cd8fb6 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.MasterVolumeFactor eab43f978f55ba7ee9eda8ebafc4472c4a789d77b2ca6da90c21eb8c534d2850 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.DuplexRb 8825e21049fd5a478ffebfdd2256737b84c914f90292a3163dba0349e16fce76 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.Resampling 4c3fc9ec5b4f014e222270a73bd33c654bcd91a99cbd05d550ea6f9a8ba0edc9 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.Playback 1d0785f51573c8db4e8ffca338d1206b7ac63e456fb9ab29873b84f80fd2db42 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.Capture ae5cc1706413deecddf8f9fce7da0cceacdb557743d2d1978d7d09eb21f89ef1 -->
<!-- api: P:Inno.Native.MiniAudio.MaDevicePtr.AnonymousField24 56befc389bbf98c45be06b3787a35439e244cf748da0e897ba25452476b5a285 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f4e41de50f25225d"></a>[`public Inno.Native.MiniAudio.MaDevice* Inno.Native.MiniAudio.MaDevicePtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15688) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58c98a085d21ba8f"></a>[`public Inno.Native.MiniAudio.MaDevicePtr.MaDevicePtr(Inno.Native.MiniAudio.MaDevice* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15683) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8581e9ddb0f9087d"></a>[`public bool Inno.Native.MiniAudio.MaDevicePtr.Equals(Inno.Native.MiniAudio.MaDevicePtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15698) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-55ce70b051f8e1a2"></a>[`public override bool Inno.Native.MiniAudio.MaDevicePtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15699) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-9f5b28ccc35e7a9c"></a>[`public override int Inno.Native.MiniAudio.MaDevicePtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15700) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-c080e2654389be6d"></a>[`public static bool Inno.Native.MiniAudio.MaDevicePtr.operator ==(Inno.Native.MiniAudio.MaDevicePtr left, Inno.Native.MiniAudio.MaDevicePtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15696) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-61e75c8d14546788"></a>[`public static Inno.Native.MiniAudio.MaDevicePtr.implicit operator Inno.Native.MiniAudio.MaDevicePtr(Inno.Native.MiniAudio.MaDevice* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15694) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b57961afb4306892"></a>[`public static Inno.Native.MiniAudio.MaDevicePtr.implicit operator Inno.Native.MiniAudio.MaDevice*(Inno.Native.MiniAudio.MaDevicePtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15695) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-33e64f16b91ddc4b"></a>[`public static bool Inno.Native.MiniAudio.MaDevicePtr.operator !=(Inno.Native.MiniAudio.MaDevicePtr left, Inno.Native.MiniAudio.MaDevicePtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15697) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9c3e9045e91b2950"></a>[`public ref Inno.Native.MiniAudio.MaDevice.MaDeviceAnonymous24 Inno.Native.MiniAudio.MaDevicePtr.AnonymousField24 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15726) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bcee97b82a386145"></a>[`public ref Inno.Native.MiniAudio.MaDevice.CaptureAnonymous Inno.Native.MiniAudio.MaDevicePtr.Capture { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15725) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0249a98748229d53"></a>[`public ref Inno.Native.MiniAudio.MaDuplexRb Inno.Native.MiniAudio.MaDevicePtr.DuplexRb { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15722) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-83c2abd08f51a4d3"></a>[`public bool Inno.Native.MiniAudio.MaDevicePtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15689) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e77f1f1fb29e44c0"></a>[`public ref byte Inno.Native.MiniAudio.MaDevicePtr.IsOwnerOfContext { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15716) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9835d041dd668c1b"></a>[`public Inno.Native.MiniAudio.MaDevice Inno.Native.MiniAudio.MaDevicePtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15692) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bd31ee17fcfe731e"></a>[`public ref Inno.Native.MiniAudio.MaAtomicFloat Inno.Native.MiniAudio.MaDevicePtr.MasterVolumeFactor { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15721) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f7dc522ab05e8a17"></a>[`public ref byte Inno.Native.MiniAudio.MaDevicePtr.NoClip { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15718) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f60ec901ae8b3fbd"></a>[`public ref byte Inno.Native.MiniAudio.MaDevicePtr.NoDisableDenormals { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15719) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5555a6195ebc9997"></a>[`public ref byte Inno.Native.MiniAudio.MaDevicePtr.NoFixedSizedCallback { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15720) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4a6c04567617afe8"></a>[`public ref byte Inno.Native.MiniAudio.MaDevicePtr.NoPreSilencedOutputBuffer { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15717) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-78346aeadd7eee15"></a>[`public static Inno.Native.MiniAudio.MaDevicePtr Inno.Native.MiniAudio.MaDevicePtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15690) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a3757211fa25b73e"></a>[`public void* Inno.Native.MiniAudio.MaDevicePtr.OnData { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15706) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ce071944abb4c4e2"></a>[`public void* Inno.Native.MiniAudio.MaDevicePtr.OnNotification { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15707) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ebd913a5a98b2cca"></a>[`public void* Inno.Native.MiniAudio.MaDevicePtr.OnStop { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15708) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6c7fcf6029661fc6"></a>[`public ref Inno.Native.MiniAudio.MaContextPtr Inno.Native.MiniAudio.MaDevicePtr.PContext { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15702) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dc9527836c0b4cf2"></a>[`public ref Inno.Native.MiniAudio.MaDevice.PlaybackAnonymous Inno.Native.MiniAudio.MaDevicePtr.Playback { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15724) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1eaec1177474fa48"></a>[`public void* Inno.Native.MiniAudio.MaDevicePtr.PUserData { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15709) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5f9cee4192b4b263"></a>[`public ref Inno.Native.MiniAudio.MaDevice.ResamplingAnonymous Inno.Native.MiniAudio.MaDevicePtr.Resampling { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15723) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-adc1261ff0a1f22f"></a>[`public ref uint Inno.Native.MiniAudio.MaDevicePtr.SampleRate { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15704) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e261fc9939242d5c"></a>[`public ref nint Inno.Native.MiniAudio.MaDevicePtr.StartEvent { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15712) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9ab1f221f149d6bd"></a>[`public ref nint Inno.Native.MiniAudio.MaDevicePtr.StartStopLock { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15710) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-647cc0a906f8e80c"></a>[`public ref Inno.Native.MiniAudio.MaAtomicDeviceState Inno.Native.MiniAudio.MaDevicePtr.State { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15705) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-08636866b3aeb325"></a>[`public ref nint Inno.Native.MiniAudio.MaDevicePtr.StopEvent { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15713) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4b26913bcec5d7c7"></a>[`public ref nint Inno.Native.MiniAudio.MaDevicePtr.Thread { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15714) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ce6f7bf69f3942b6"></a>[`public ref Inno.Native.MiniAudio.MaDeviceType Inno.Native.MiniAudio.MaDevicePtr.Type { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15703) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d39a21906c426776"></a>[`public ref nint Inno.Native.MiniAudio.MaDevicePtr.WakeupEvent { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15711) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b192461916f057fd"></a>[`public ref Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaDevicePtr.WorkResult { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15715) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a8e1802a6a08899b"></a>[`public struct Inno.Native.MiniAudio.MaDevicePtr : System.IEquatable<Inno.Native.MiniAudio.MaDevicePtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15681) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceState</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceState 3aa0ec474cc1ec17640faa7ec6faaa7446120fde1f610be5241cb6ef8b8c4922 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceState.Uninitialized 920178a55413df65f389a8be6f2fcc963515025389ca6aa80c603d03044f9ebf -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceState.Stopped 96e6bc9ab9e944b603fdd858072365b76aaffd77ed5f8b89d0cbe63d3ee32bc1 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceState.Started daf011e4aa6d39dea877db326af679c505b8307a29b19b8092a07c95e9efa5be -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceState.Starting fdde200db1a80ab173efa0a2e3bc0739ce4915dd2a834be7293adad2d4765cd5 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceState.Stopping 28b130de1ef02fb01d9eea441697979886ae9f70d1ea1b438119948260bf4079 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-33bc89d1a8593035"></a>[`Inno.Native.MiniAudio.MaDeviceState.Started = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L201) | 当前常量或枚举值：`2`。 | — |
| <a id="api-3973d52e59a59583"></a>[`Inno.Native.MiniAudio.MaDeviceState.Starting = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L202) | 当前常量或枚举值：`3`。 | — |
| <a id="api-d39a9bf9489e384e"></a>[`Inno.Native.MiniAudio.MaDeviceState.Stopped = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L200) | 当前常量或枚举值：`1`。 | — |
| <a id="api-e5b3c02c4c12f95e"></a>[`Inno.Native.MiniAudio.MaDeviceState.Stopping = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L203) | 当前常量或枚举值：`4`。 | — |
| <a id="api-819e9c2e510eaec8"></a>[`Inno.Native.MiniAudio.MaDeviceState.Uninitialized = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L199) | 当前常量或枚举值：`0`。 | — |
| <a id="api-0d1e6f7a128a7a01"></a>[`public enum Inno.Native.MiniAudio.MaDeviceState : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L197) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDeviceType</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDeviceType cbbcddecc801bb325b7b8bc382fdc2b7202960949cdca29a106f4f18b31f1b20 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceType.Playback f1d656daad31affedea5441f146027750d6593e1bd52976477ecd2a1c242b658 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceType.Capture 63f57eb1e192fbd60adf5e7768739701aa363bc5612f55692239a8a00c90bc04 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceType.Duplex 1be557470be1dd4f05ba09802d113555f68360a1ed192c13588e9ff3278d7e56 -->
<!-- api: F:Inno.Native.MiniAudio.MaDeviceType.Loopback 55423a92a8056a17535ef08e2bead2fb625a476a942211683cc97b74e1e1f751 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-527534a980a73283"></a>[`Inno.Native.MiniAudio.MaDeviceType.Capture = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L175) | 当前常量或枚举值：`2`。 | — |
| <a id="api-5d53bf97fbc4b30f"></a>[`Inno.Native.MiniAudio.MaDeviceType.Duplex = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L176) | 当前常量或枚举值：`3`。 | — |
| <a id="api-0469a5702c1e3bb6"></a>[`Inno.Native.MiniAudio.MaDeviceType.Loopback = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L177) | 当前常量或枚举值：`4`。 | — |
| <a id="api-1ce2fb5284ed0420"></a>[`Inno.Native.MiniAudio.MaDeviceType.Playback = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L174) | 当前常量或枚举值：`1`。 | — |
| <a id="api-6528aff890e085c7"></a>[`public enum Inno.Native.MiniAudio.MaDeviceType : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L172) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDitherMode</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDitherMode a562fd5e6523f1ddd6ef2d8ce11c41b0a494d60618c07b38694bc2fbbc640c38 -->
<!-- api: F:Inno.Native.MiniAudio.MaDitherMode.None 5221e0ea29060f497acab0e677cb58ca0f4186ffe571117a65546c3c9f122152 -->
<!-- api: F:Inno.Native.MiniAudio.MaDitherMode.Rectangle 30def2d8834f09ffa89463139817b51a576e845d02a76dc4bc0cb3344394185d -->
<!-- api: F:Inno.Native.MiniAudio.MaDitherMode.Triangle 351731f46b1975c47de011e3c68df3546f7b219ed77b3b872c38eff014e7af10 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a3155b698a654728"></a>[`Inno.Native.MiniAudio.MaDitherMode.None = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L253) | 当前常量或枚举值：`0`。 | — |
| <a id="api-b4480de883ed392d"></a>[`Inno.Native.MiniAudio.MaDitherMode.Rectangle = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L254) | 当前常量或枚举值：`1`。 | — |
| <a id="api-52c77780dbf6f035"></a>[`Inno.Native.MiniAudio.MaDitherMode.Triangle = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L255) | 当前常量或枚举值：`2`。 | — |
| <a id="api-e8e4fd2e74c6b66a"></a>[`public enum Inno.Native.MiniAudio.MaDitherMode : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L251) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDuplexRb</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDuplexRb 3e5e191f3e4c9a17e8f9989a850c5e50757e2c706a524bfc3c09d68842d71d11 -->
<!-- api: F:Inno.Native.MiniAudio.MaDuplexRb.Rb c7f43e6cd335b2fab25b6810087c30701f644e70ee053fb5f65fa49e27f02953 -->
<!-- api: M:Inno.Native.MiniAudio.MaDuplexRb.#ctor(Inno.Native.MiniAudio.MaPcmRb) bfe8d1cc288ad7638b3ff6a8b511487485ef2ab66abb9e3f9591a1602e3435ea -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0624fc118b257495"></a>[`public Inno.Native.MiniAudio.MaPcmRb Inno.Native.MiniAudio.MaDuplexRb.Rb`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5313) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-719dcfa412395816"></a>[`public Inno.Native.MiniAudio.MaDuplexRb.MaDuplexRb(Inno.Native.MiniAudio.MaPcmRb rb = default(Inno.Native.MiniAudio.MaPcmRb))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5314) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ff22d9d35168cf94"></a>[`public struct Inno.Native.MiniAudio.MaDuplexRb`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5311) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaDuplexRbPtr</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaDuplexRbPtr 96d1c640b5ede288109e6d0abd6c2ada9cbb9d0789e108087e299103fe8c0089 -->
<!-- api: M:Inno.Native.MiniAudio.MaDuplexRbPtr.#ctor(Inno.Native.MiniAudio.MaDuplexRb*) 38b5eba803d06ea2bc23225eaaaf66ce29b17717e17122b519d8254111665620 -->
<!-- api: F:Inno.Native.MiniAudio.MaDuplexRbPtr.Handle 72a728b32c019ed181d665c2ad8ead14d6d41d56a34845bd197b270c0bc518c6 -->
<!-- api: P:Inno.Native.MiniAudio.MaDuplexRbPtr.IsNull 2304e02bed3fddb7d35338f547c56b805b92ad507096f5514d07a198c9acef5c -->
<!-- api: P:Inno.Native.MiniAudio.MaDuplexRbPtr.Null a17ba37ed531e39b4f308665b452bda082951cdef3fdafc3c6003406b3d47d2e -->
<!-- api: P:Inno.Native.MiniAudio.MaDuplexRbPtr.Item(System.Int32) 24cbf2ce52487cf3bad6decaa2d58f8b3ad7c9198bb5ee2ffae916cbd5ae1ffa -->
<!-- api: M:Inno.Native.MiniAudio.MaDuplexRbPtr.op_Implicit(Inno.Native.MiniAudio.MaDuplexRb*)~Inno.Native.MiniAudio.MaDuplexRbPtr 45b3f094f65fffaa01bbfddaf679404e4bc46922cf88c8bed4c765a6fdb845a0 -->
<!-- api: M:Inno.Native.MiniAudio.MaDuplexRbPtr.op_Implicit(Inno.Native.MiniAudio.MaDuplexRbPtr)~Inno.Native.MiniAudio.MaDuplexRb* 2350279d7574ccd783eb0b03b5d333c129646eed648f1e8caa01fdfd18ff6a41 -->
<!-- api: M:Inno.Native.MiniAudio.MaDuplexRbPtr.op_Equality(Inno.Native.MiniAudio.MaDuplexRbPtr,Inno.Native.MiniAudio.MaDuplexRbPtr) f212e146a1df17897e766a91cfebc4da006b1374b9787f7fa46bda62b5c8cb9f -->
<!-- api: M:Inno.Native.MiniAudio.MaDuplexRbPtr.op_Inequality(Inno.Native.MiniAudio.MaDuplexRbPtr,Inno.Native.MiniAudio.MaDuplexRbPtr) 242e5e36fbe7bdff2a14d98a8c16db45d6ecc9557951a7991f419ac6dd583f7f -->
<!-- api: M:Inno.Native.MiniAudio.MaDuplexRbPtr.Equals(Inno.Native.MiniAudio.MaDuplexRbPtr) e3caf16ce38b269bbf86acd52ecadd9839d9635c58896273bb785bb9ebbefb4f -->
<!-- api: M:Inno.Native.MiniAudio.MaDuplexRbPtr.Equals(System.Object) 10d6e7ccfc532655030cb482a2749a2c3b676c5a4c305517b4f9df0e099e5d1f -->
<!-- api: M:Inno.Native.MiniAudio.MaDuplexRbPtr.GetHashCode 2ee079fc8a76831b5c7fa6a5fdd5849868408f47fede09b7cfb8d6a2db65aba0 -->
<!-- api: P:Inno.Native.MiniAudio.MaDuplexRbPtr.Rb 6fa0d92328eafc45f777e989d418d8415144e0b834e09bc3a2593ad259f5b037 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-15178b1f1383b126"></a>[`public Inno.Native.MiniAudio.MaDuplexRb* Inno.Native.MiniAudio.MaDuplexRbPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15737) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2720059a6e39174"></a>[`public Inno.Native.MiniAudio.MaDuplexRbPtr.MaDuplexRbPtr(Inno.Native.MiniAudio.MaDuplexRb* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15732) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2af1261dbb741d8f"></a>[`public bool Inno.Native.MiniAudio.MaDuplexRbPtr.Equals(Inno.Native.MiniAudio.MaDuplexRbPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15747) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-aec02902f5b616d3"></a>[`public override bool Inno.Native.MiniAudio.MaDuplexRbPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15748) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-26e8d28d2fde489f"></a>[`public override int Inno.Native.MiniAudio.MaDuplexRbPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15749) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-f0e88e5d388acdf2"></a>[`public static bool Inno.Native.MiniAudio.MaDuplexRbPtr.operator ==(Inno.Native.MiniAudio.MaDuplexRbPtr left, Inno.Native.MiniAudio.MaDuplexRbPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15745) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1cb104fd6f52f995"></a>[`public static Inno.Native.MiniAudio.MaDuplexRbPtr.implicit operator Inno.Native.MiniAudio.MaDuplexRbPtr(Inno.Native.MiniAudio.MaDuplexRb* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15743) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0a67d1610cb87e7b"></a>[`public static Inno.Native.MiniAudio.MaDuplexRbPtr.implicit operator Inno.Native.MiniAudio.MaDuplexRb*(Inno.Native.MiniAudio.MaDuplexRbPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15744) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-32ee372a8a648d40"></a>[`public static bool Inno.Native.MiniAudio.MaDuplexRbPtr.operator !=(Inno.Native.MiniAudio.MaDuplexRbPtr left, Inno.Native.MiniAudio.MaDuplexRbPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15746) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0d31f14855557fcc"></a>[`public bool Inno.Native.MiniAudio.MaDuplexRbPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15738) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c5469baafdf5a372"></a>[`public Inno.Native.MiniAudio.MaDuplexRb Inno.Native.MiniAudio.MaDuplexRbPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15741) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-20a537a0134f5881"></a>[`public static Inno.Native.MiniAudio.MaDuplexRbPtr Inno.Native.MiniAudio.MaDuplexRbPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15739) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-67b9d4a61d92cd45"></a>[`public ref Inno.Native.MiniAudio.MaPcmRb Inno.Native.MiniAudio.MaDuplexRbPtr.Rb { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15751) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-03e8b8ec3efc05be"></a>[`public struct Inno.Native.MiniAudio.MaDuplexRbPtr : System.IEquatable<Inno.Native.MiniAudio.MaDuplexRbPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15730) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEncoder</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEncoder 4ac7a028d9dfd68722b3d2ffb01131e4024f9c9d54b2930a381a642ad82b23fb -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoder.Config 09e91c87e01cc0803e1842ebdcd94439978f83e1a5ac33540e01107fc56b349d -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoder.OnWrite 62c1b372302005f0017c46491a83107601d9c01359e658ab9b3a69aadb252b66 -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoder.OnSeek 631bbca33955c154e8f60f84af789174f9b038964ba1b82e887293129df8bdaf -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoder.OnInit 421d35039a661d9532f76cf09462f0d2d0244ba18117a9ad111bf7fcdc573516 -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoder.OnUninit 55f01411a41231d6a70c11b7ad56204ebf242e843e4beddacbc9168c828d72a4 -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoder.OnWritePcmFrames c5fc17ae7d9fab7ec8621c01f6d63ba91cfc1ab14e4f5de61dd41977c5f1f0aa -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoder.PUserData a7be18ad70b76ce685fa6cb45c601ccadbc0e8fea737e7a416586ebe9a0ba680 -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoder.PInternalEncoder 2a42357db9f02654cf5822490e34f212e633b787d2ec3b3080438836644aecce -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoder.Data a34c6a1efe329b3d8b3d682029a01595e0ac0c01199c912b0262b9e73b9d6e9d -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoder.#ctor(Inno.Native.MiniAudio.MaEncoderConfig,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,Inno.Native.MiniAudio.MaEncoder.DataAnonymous) 5a6928be8a56a52171f443c41334935688da33564983b256b830e6eaf8d645f5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e1b3fd66f7f5d0b8"></a>[`public Inno.Native.MiniAudio.MaEncoderConfig Inno.Native.MiniAudio.MaEncoder.Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12266) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-712303ed99fefc2d"></a>[`public Inno.Native.MiniAudio.MaEncoder.DataAnonymous Inno.Native.MiniAudio.MaEncoder.Data`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12274) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-034320b0eafc6721"></a>[`public void* Inno.Native.MiniAudio.MaEncoder.OnInit`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12269) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9246d7925e2b411a"></a>[`public void* Inno.Native.MiniAudio.MaEncoder.OnSeek`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12268) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ba11ea441d8acf3"></a>[`public void* Inno.Native.MiniAudio.MaEncoder.OnUninit`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12270) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0cac8bfc3b7aa730"></a>[`public void* Inno.Native.MiniAudio.MaEncoder.OnWrite`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12267) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-42dfad1180de13b1"></a>[`public void* Inno.Native.MiniAudio.MaEncoder.OnWritePcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12271) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c5d71d0cc7011566"></a>[`public void* Inno.Native.MiniAudio.MaEncoder.PInternalEncoder`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12273) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4ac82304377db16a"></a>[`public void* Inno.Native.MiniAudio.MaEncoder.PUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12272) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2511440900a78652"></a>[`public Inno.Native.MiniAudio.MaEncoder.MaEncoder(Inno.Native.MiniAudio.MaEncoderConfig config = default(Inno.Native.MiniAudio.MaEncoderConfig), void* onWrite = null, void* onSeek = null, void* onInit = null, void* onUninit = null, void* onWritePCMFrames = null, void* pUserData = null, void* pInternalEncoder = null, Inno.Native.MiniAudio.MaEncoder.DataAnonymous data = default(Inno.Native.MiniAudio.MaEncoder.DataAnonymous))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12275) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-56bbe9693d05d367"></a>[`public struct Inno.Native.MiniAudio.MaEncoder`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12239) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEncoder.DataAnonymous</code> · 2 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEncoder.DataAnonymous 9f5934794b960c21a42bf24e2758ee28014ffc2eb2eb672ac9939fb054753b6a -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoder.DataAnonymous.Vfs c2254b87675e873193008dd68db3e7039bc848f9518d3ad99a9cd20cb43d9a50 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c2b8e2fc8de75c9b"></a>[`public Inno.Native.MiniAudio.MaEncoder.DataAnonymous.VfsAnonymous Inno.Native.MiniAudio.MaEncoder.DataAnonymous.Vfs`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12263) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2c9bc1534650b4bc"></a>[`public struct Inno.Native.MiniAudio.MaEncoder.DataAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12245) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEncoder.DataAnonymous.VfsAnonymous</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEncoder.DataAnonymous.VfsAnonymous 3818ed476501768bf0d74cbd52b2ef4a46187e2b26216b68ed188e0a1f89d41d -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoder.DataAnonymous.VfsAnonymous.PVfs 440218b4d131b8953ba42997e073a4dbf5d08aa36df14e03fbefdb24cf6e5c59 -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoder.DataAnonymous.VfsAnonymous.File 7d001cd8dde2ee42cc83c8c2ad3366e3739d7419a112044ea43344e449d0b639 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoder.DataAnonymous.VfsAnonymous.#ctor(Inno.Native.MiniAudio.MaVfs*,System.IntPtr) 05143cb871b196ce28c27654c4a964b2f42e5d5f2f78ca6e26a1966c6cf72676 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9c1d391e2450d84f"></a>[`public nint Inno.Native.MiniAudio.MaEncoder.DataAnonymous.VfsAnonymous.File`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12254) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4cdf2ec81e3ced7c"></a>[`public Inno.Native.MiniAudio.MaVfs* Inno.Native.MiniAudio.MaEncoder.DataAnonymous.VfsAnonymous.PVfs`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12253) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-923cf16c801cbb4d"></a>[`public Inno.Native.MiniAudio.MaEncoder.DataAnonymous.VfsAnonymous.VfsAnonymous(Inno.Native.MiniAudio.MaVfs* pVFS = null, nint file = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12255) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7d8aea40188e41e3"></a>[`public struct Inno.Native.MiniAudio.MaEncoder.DataAnonymous.VfsAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12251) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEncoderConfig</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEncoderConfig 24a0cabd696574ec00170014813d3ee81e6c29433743ae4db2b4af00670272ee -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoderConfig.EncodingFormat 1c35cd2cc67566f6e8204f3608048ab5e111f284980fe09aab9d96c159222c3b -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoderConfig.Format 15e589c94763ec4b8730df3e4ce0caf134f56267c8e46978b1a2990e5c8d48f5 -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoderConfig.Channels 166d1028ab593b5e0e6e596b50bafb69e3b9eebb176b46b2ed48d6d84d6d3d2c -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoderConfig.SampleRate e40980d9af517e371dd658cca559693cbb2aa46af32eca0d48bd2139166d8778 -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoderConfig.AllocationCallbacks 692e9491cfd8890bcd2abf54b67e74d0028687da5b85d7229b2cd2d569eb6218 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderConfig.#ctor(Inno.Native.MiniAudio.MaEncodingFormat,Inno.Native.MiniAudio.MaFormat,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaAllocationCallbacks) 87fc75ca59d1f7666b4558f586c7303c52df0daaf322f5aa6dd2b7a9e0ffc5c2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c82f6e1bd9cbcd1e"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaEncoderConfig.AllocationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12224) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01933e86e683b308"></a>[`public uint Inno.Native.MiniAudio.MaEncoderConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12222) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c500de61f22f6262"></a>[`public Inno.Native.MiniAudio.MaEncodingFormat Inno.Native.MiniAudio.MaEncoderConfig.EncodingFormat`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12220) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb45ccbae216ad2b"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaEncoderConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12221) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e0deab6785fac43a"></a>[`public uint Inno.Native.MiniAudio.MaEncoderConfig.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12223) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-84c8f7c480f94bde"></a>[`public Inno.Native.MiniAudio.MaEncoderConfig.MaEncoderConfig(Inno.Native.MiniAudio.MaEncodingFormat encodingFormat = Inno.Native.MiniAudio.MaEncodingFormat.Unknown, Inno.Native.MiniAudio.MaFormat format = Inno.Native.MiniAudio.MaFormat.Unknown, uint channels = 0, uint sampleRate = 0, Inno.Native.MiniAudio.MaAllocationCallbacks allocationCallbacks = default(Inno.Native.MiniAudio.MaAllocationCallbacks))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12225) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-23cfdff0f01fcfb7"></a>[`public struct Inno.Native.MiniAudio.MaEncoderConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L12218) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEncoderConfigPtr</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEncoderConfigPtr 91a311b695bc92ebf0ac24589d120217ef6068349d503eba3b34db43c1609782 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderConfigPtr.#ctor(Inno.Native.MiniAudio.MaEncoderConfig*) 2265e9444ca051fb05bc83daddd03b3dc154abe5afd213940ae3d18c8fe31271 -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoderConfigPtr.Handle 978f77759084695cfdf7e39509403afb3cef7eaf5729904d846a140734895965 -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderConfigPtr.IsNull 302a413a1dbfd1e812b129af40236dc496dcf216586a49ef7fc83b55275714ed -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderConfigPtr.Null 1b29078e954df455baff66b49b7d57e69ce189b68e08928795f5f90af6d6ab13 -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderConfigPtr.Item(System.Int32) ae7c1c1f824dbf9be9c6dfb8c3f5b0e91488ad7842efe0caebf1a33ff09cd2ca -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaEncoderConfig*)~Inno.Native.MiniAudio.MaEncoderConfigPtr d017ecc17e1d8768eb7fd06f397719249d7049d5262a84df719049d189b7b18c -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaEncoderConfigPtr)~Inno.Native.MiniAudio.MaEncoderConfig* 3098c0e10e0318ca9b35388993ca95077a0d78c43b2c53711e70b2fd21621a4e -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderConfigPtr.op_Equality(Inno.Native.MiniAudio.MaEncoderConfigPtr,Inno.Native.MiniAudio.MaEncoderConfigPtr) f6425c9a738fde2af83dd123795861c441a3e8d12bd611c3286d2b02c67383e9 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaEncoderConfigPtr,Inno.Native.MiniAudio.MaEncoderConfigPtr) c7706a201153b805d6e443830315f5f756aa28059fba814a542113bdfca6bc85 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderConfigPtr.Equals(Inno.Native.MiniAudio.MaEncoderConfigPtr) 6a144b926ff59c9a5ed0cfd88bff7dee1ddb97f59b21806fa523baa854e79b26 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderConfigPtr.Equals(System.Object) 44210659af7328df148d1948bd16525db0ee76ac46abf3946059edbda587e252 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderConfigPtr.GetHashCode 95542ef7b05db0a7bd04cc962e6852942550870f598a9972d41575bc17c21642 -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderConfigPtr.EncodingFormat cb8497b05671d01c199165df9458da86dfb5597f57d69324a2e705d056ce2d5b -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderConfigPtr.Format ffc7d4fb9193675c5e5fd34795bbde9a935bfa4f04c17ae479820187a6688a9e -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderConfigPtr.Channels 575b1540a6c9846cdd3def4704603cf18a361b468d4912023b3d831f457392fc -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderConfigPtr.SampleRate bf765fc66e5cc25aecac9f45804004d70fb7ec2712d6e94e82fb2ccecda25bd0 -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderConfigPtr.AllocationCallbacks f35a564eb3a59f38f89fe98152a7b0290fecf3d182f8ff93375ab6f00297639b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ce3af91376028796"></a>[`public Inno.Native.MiniAudio.MaEncoderConfig* Inno.Native.MiniAudio.MaEncoderConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15762) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-47d6a588370b2d48"></a>[`public Inno.Native.MiniAudio.MaEncoderConfigPtr.MaEncoderConfigPtr(Inno.Native.MiniAudio.MaEncoderConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15757) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e8fa89600edd2f59"></a>[`public bool Inno.Native.MiniAudio.MaEncoderConfigPtr.Equals(Inno.Native.MiniAudio.MaEncoderConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15772) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-c17246cde74c0d74"></a>[`public override bool Inno.Native.MiniAudio.MaEncoderConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15773) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-6338ade3b8303358"></a>[`public override int Inno.Native.MiniAudio.MaEncoderConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15774) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-32bcfa14b1e8ac45"></a>[`public static bool Inno.Native.MiniAudio.MaEncoderConfigPtr.operator ==(Inno.Native.MiniAudio.MaEncoderConfigPtr left, Inno.Native.MiniAudio.MaEncoderConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15770) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b254d93329d31d88"></a>[`public static Inno.Native.MiniAudio.MaEncoderConfigPtr.implicit operator Inno.Native.MiniAudio.MaEncoderConfigPtr(Inno.Native.MiniAudio.MaEncoderConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15768) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c7a39f7001382125"></a>[`public static Inno.Native.MiniAudio.MaEncoderConfigPtr.implicit operator Inno.Native.MiniAudio.MaEncoderConfig*(Inno.Native.MiniAudio.MaEncoderConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15769) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-00d359b0234a562e"></a>[`public static bool Inno.Native.MiniAudio.MaEncoderConfigPtr.operator !=(Inno.Native.MiniAudio.MaEncoderConfigPtr left, Inno.Native.MiniAudio.MaEncoderConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15771) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a7f8032316afc31e"></a>[`public ref Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaEncoderConfigPtr.AllocationCallbacks { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15780) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3c5250ebb5159a61"></a>[`public ref uint Inno.Native.MiniAudio.MaEncoderConfigPtr.Channels { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15778) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6ca0e9d2b09396c9"></a>[`public ref Inno.Native.MiniAudio.MaEncodingFormat Inno.Native.MiniAudio.MaEncoderConfigPtr.EncodingFormat { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15776) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-055aafc0f4ee0e74"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaEncoderConfigPtr.Format { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15777) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-92b3c4a34ab39402"></a>[`public bool Inno.Native.MiniAudio.MaEncoderConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15763) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b10da35e73f9050d"></a>[`public Inno.Native.MiniAudio.MaEncoderConfig Inno.Native.MiniAudio.MaEncoderConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15766) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9924bcae783da204"></a>[`public static Inno.Native.MiniAudio.MaEncoderConfigPtr Inno.Native.MiniAudio.MaEncoderConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15764) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f943e596fde967b1"></a>[`public ref uint Inno.Native.MiniAudio.MaEncoderConfigPtr.SampleRate { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15779) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2eb7194321e5d1f4"></a>[`public struct Inno.Native.MiniAudio.MaEncoderConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaEncoderConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15755) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEncoderInitProc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEncoderInitProc 403ed2610b44d89d42827ca35fd952dc9a24a52646de629f0aaa2e23c542ff7c -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderInitProc.#ctor(System.Object,System.IntPtr) f93f58d6f26491ffb8c1a11b4aea590032e55d210abef44a07cbff0a9ae0abcd -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderInitProc.Invoke(Inno.Native.MiniAudio.MaEncoder*) e952930825ffe0cd5fa28df908e29074681d56ab0eff5300094659735a282b21 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderInitProc.BeginInvoke(Inno.Native.MiniAudio.MaEncoder*,System.AsyncCallback,System.Object) 3a8bedc08a326a35c04ace193e968f2eee5274ca6162d6d9000fe38894bf5192 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderInitProc.EndInvoke(System.IAsyncResult) 601d6376db7c4261ab4bf3a407eca90f7b38b023cbaa965c6a97e361a5e65970 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-41bffc377630a174"></a>[`public Inno.Native.MiniAudio.MaEncoderInitProc.MaEncoderInitProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14236) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9bf4b9b03a84bf9a"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaEncoderInitProc.BeginInvoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14236) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1c7f0cee8e6c12c7"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaEncoderInitProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14236) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5764038887535235"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaEncoderInitProc.Invoke(Inno.Native.MiniAudio.MaEncoder* pEncoder)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14236) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c63eb2efe5be5f6e"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaEncoderInitProc(Inno.Native.MiniAudio.MaEncoder* pEncoder)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14236) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEncoderPtr</code> · 22 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEncoderPtr 177458d7eaa28eba0f8e8b363a430001be4e5f97bafdd9b309f15587ffcd6e0f -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderPtr.#ctor(Inno.Native.MiniAudio.MaEncoder*) 80996ac5236c31212944250bf3eb6f64540521d7136bd3c69f17629128402b59 -->
<!-- api: F:Inno.Native.MiniAudio.MaEncoderPtr.Handle 4f4613b8616553fc41e4721982c7289a8aec3d536535b82b71106014dfbb00f5 -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderPtr.IsNull e17075b4e8e8ecd2fa475738782c66075b058e2851b0a04dc8193c25a07c6050 -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderPtr.Null 9dcaab8f768a270abf77256dc1a6fd5a80f4235e5408b872363f6ade04c205ef -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderPtr.Item(System.Int32) 28309539e974728b32ce2627791036aa553665d129454533156d06b329f96c4b -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderPtr.op_Implicit(Inno.Native.MiniAudio.MaEncoder*)~Inno.Native.MiniAudio.MaEncoderPtr 3392d78731585af531c38d9f64717a84dd9e3d10182494e4c65488404af4d831 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderPtr.op_Implicit(Inno.Native.MiniAudio.MaEncoderPtr)~Inno.Native.MiniAudio.MaEncoder* dee20893456119129990ab44e165a98d10a7845a0cad9846ac5f0d62755aa948 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderPtr.op_Equality(Inno.Native.MiniAudio.MaEncoderPtr,Inno.Native.MiniAudio.MaEncoderPtr) c2e541e244515625261971eac3c6fd263eca24dd6ef75a93f52886edd74ed14b -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderPtr.op_Inequality(Inno.Native.MiniAudio.MaEncoderPtr,Inno.Native.MiniAudio.MaEncoderPtr) 8dbdb718070249cd383eec67283ae32f796b26b0cc27ff43ad0aa842ef1bbf9a -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderPtr.Equals(Inno.Native.MiniAudio.MaEncoderPtr) abb6b261bd74932bf819d3c425557b57bc908e6dd309c20089231e68e2f14a8f -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderPtr.Equals(System.Object) d4388a45e1bbb02d5743a0aa5a138771af1ea459545a0f21f219f24fdaa5f8be -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderPtr.GetHashCode 396ae68fc4a421bad6095d97be013ebfaa3b988d40c9da0af8648affa5a632df -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderPtr.Config cca3a9f76a35e917d9fa036364becfd79cfa951e9d8fc4ac94ab88022016a1c8 -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderPtr.OnWrite 97b243b2d45b2f4e2c63f769fef5a0b7a90205ea4d33e86b05221ac3658dbc8e -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderPtr.OnSeek d4a4549680266c340c41bf94c0186e5ded6e4634cca83d0ed24e8428f8384f66 -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderPtr.OnInit a386e6758635b0103833c4e0731e79e4c5e771664f52374d2f2886046ea25bc5 -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderPtr.OnUninit 87720eebe8ae33583acf4693ce7040cc76fa0c1fd2f4030ea86a03a95bcc84e4 -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderPtr.OnWritePcmFrames 2afefdcf37147368b9f2e9743a612a68764b6157064e00098c5f833951d215d1 -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderPtr.PUserData feada829106308e4c9620fe4edab2b9460b8e75b37d8c023e6db43fc5458253f -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderPtr.PInternalEncoder c58aef3f27072d02bfb47b55cf972bcbcaf1d732f57d5b93bc03a86971114ea5 -->
<!-- api: P:Inno.Native.MiniAudio.MaEncoderPtr.Data f0c946420a029cfe3c3853451e2933bb83379ddf3217be534d18d7c11a79863d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ebeb6625afab2985"></a>[`public Inno.Native.MiniAudio.MaEncoder* Inno.Native.MiniAudio.MaEncoderPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15791) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-25e6bca0df5dc498"></a>[`public Inno.Native.MiniAudio.MaEncoderPtr.MaEncoderPtr(Inno.Native.MiniAudio.MaEncoder* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15786) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-96046184f3d43109"></a>[`public bool Inno.Native.MiniAudio.MaEncoderPtr.Equals(Inno.Native.MiniAudio.MaEncoderPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15801) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-57317f5591e9abb5"></a>[`public override bool Inno.Native.MiniAudio.MaEncoderPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15802) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-46c47d7019a7ff27"></a>[`public override int Inno.Native.MiniAudio.MaEncoderPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15803) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-2af2b2ec44b7fc4b"></a>[`public static bool Inno.Native.MiniAudio.MaEncoderPtr.operator ==(Inno.Native.MiniAudio.MaEncoderPtr left, Inno.Native.MiniAudio.MaEncoderPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15799) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fb8e6038e9dd9bdc"></a>[`public static Inno.Native.MiniAudio.MaEncoderPtr.implicit operator Inno.Native.MiniAudio.MaEncoderPtr(Inno.Native.MiniAudio.MaEncoder* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15797) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4a09168d202899a2"></a>[`public static Inno.Native.MiniAudio.MaEncoderPtr.implicit operator Inno.Native.MiniAudio.MaEncoder*(Inno.Native.MiniAudio.MaEncoderPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15798) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b533efe27d488f8c"></a>[`public static bool Inno.Native.MiniAudio.MaEncoderPtr.operator !=(Inno.Native.MiniAudio.MaEncoderPtr left, Inno.Native.MiniAudio.MaEncoderPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15800) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-42a0c4cc73b0337e"></a>[`public ref Inno.Native.MiniAudio.MaEncoderConfig Inno.Native.MiniAudio.MaEncoderPtr.Config { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15805) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b46d623fdf0b7c81"></a>[`public ref Inno.Native.MiniAudio.MaEncoder.DataAnonymous Inno.Native.MiniAudio.MaEncoderPtr.Data { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15813) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c30b94dd2d842101"></a>[`public bool Inno.Native.MiniAudio.MaEncoderPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15792) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-92de943129595962"></a>[`public Inno.Native.MiniAudio.MaEncoder Inno.Native.MiniAudio.MaEncoderPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15795) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5ac2a1a4074e8750"></a>[`public static Inno.Native.MiniAudio.MaEncoderPtr Inno.Native.MiniAudio.MaEncoderPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15793) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aefd05f9e729529f"></a>[`public void* Inno.Native.MiniAudio.MaEncoderPtr.OnInit { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15808) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e01f2cbdd11163d0"></a>[`public void* Inno.Native.MiniAudio.MaEncoderPtr.OnSeek { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15807) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-82af9c14299c2464"></a>[`public void* Inno.Native.MiniAudio.MaEncoderPtr.OnUninit { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15809) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cf9a9787f9d9ba36"></a>[`public void* Inno.Native.MiniAudio.MaEncoderPtr.OnWrite { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15806) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3c4e9fa207d2412a"></a>[`public void* Inno.Native.MiniAudio.MaEncoderPtr.OnWritePcmFrames { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15810) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d5655ab7dba78540"></a>[`public void* Inno.Native.MiniAudio.MaEncoderPtr.PInternalEncoder { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15812) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cbe68ab346fc0b71"></a>[`public void* Inno.Native.MiniAudio.MaEncoderPtr.PUserData { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15811) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c9b176c0d8977087"></a>[`public struct Inno.Native.MiniAudio.MaEncoderPtr : System.IEquatable<Inno.Native.MiniAudio.MaEncoderPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15784) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEncoderSeekProc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEncoderSeekProc 2b6859b201522c78a2c45bd8df43e2c8be9389ab7cbdd42206778ce3ef944afd -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderSeekProc.#ctor(System.Object,System.IntPtr) d819582a6acea240f6ed4cde61e9418b930794ae1f8b770f848f8c8b200b642c -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderSeekProc.Invoke(Inno.Native.MiniAudio.MaEncoder*,System.Int64,Inno.Native.MiniAudio.MaSeekOrigin) 3d6224a14d1663cddf32d40d313cc3e187889a63addcdf71a6a47f497836d4af -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderSeekProc.BeginInvoke(Inno.Native.MiniAudio.MaEncoder*,System.Int64,Inno.Native.MiniAudio.MaSeekOrigin,System.AsyncCallback,System.Object) cb8b2bbae061fa6d26701320299ec82243d9f5041b5fa21719094ecd772c5db4 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderSeekProc.EndInvoke(System.IAsyncResult) d87f84955a8aa823c3deb12230610b1c6bc716fe8334edac01cf8f7d43cf2e5e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-548d049ab2f3c0fe"></a>[`public Inno.Native.MiniAudio.MaEncoderSeekProc.MaEncoderSeekProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14231) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-00fea0cc6e8183ba"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaEncoderSeekProc.BeginInvoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14231) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f5e3f0d6bcfd9f10"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaEncoderSeekProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14231) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b1fa6f7947ee73d2"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaEncoderSeekProc.Invoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14231) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7ed807f74bddab06"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaEncoderSeekProc(Inno.Native.MiniAudio.MaEncoder* pEncoder, long offset, Inno.Native.MiniAudio.MaSeekOrigin origin)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14231) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEncoderUninitProc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEncoderUninitProc ded37c30b46483e8d1df873ca53e0bcafbe401e7d604de5340430661b61ce686 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderUninitProc.#ctor(System.Object,System.IntPtr) 750f3a512f548d5385d956364d1c5e127d8c4278ae2fd7a8991d1e2f7566822a -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderUninitProc.Invoke(Inno.Native.MiniAudio.MaEncoder*) e149bd7304b1b79da97ae4a589c009380a80d4713f1cf47947cfb2fbea24508d -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderUninitProc.BeginInvoke(Inno.Native.MiniAudio.MaEncoder*,System.AsyncCallback,System.Object) dac6b0133f89333decaede1779af56511c6c11f306c7c4af97d146065e101d28 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderUninitProc.EndInvoke(System.IAsyncResult) 4112841ef09440dadf70b7333d01b7c865811f08abe86c0c7e04b3b136f6c0d2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a24cd4f0e158fbdc"></a>[`public Inno.Native.MiniAudio.MaEncoderUninitProc.MaEncoderUninitProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14241) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-c1ffeeb36b5b117f"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaEncoderUninitProc.BeginInvoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14241) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9a71f6002e9f2d26"></a>[`public virtual void Inno.Native.MiniAudio.MaEncoderUninitProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14241) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2043d075f5c2fe79"></a>[`public virtual void Inno.Native.MiniAudio.MaEncoderUninitProc.Invoke(Inno.Native.MiniAudio.MaEncoder* pEncoder)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14241) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-47830404e5ca4b3c"></a>[`public delegate void Inno.Native.MiniAudio.MaEncoderUninitProc(Inno.Native.MiniAudio.MaEncoder* pEncoder)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14241) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEncoderWritePcmFramesProc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEncoderWritePcmFramesProc bdbc8456c004f12e10953fdafc70103fa327cba02ce2daecd67888c361651af3 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderWritePcmFramesProc.#ctor(System.Object,System.IntPtr) 0fa1f31bc9f42a25d2ffd037a4888c0baee855126a39e55423027f6de0d731f0 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderWritePcmFramesProc.Invoke(Inno.Native.MiniAudio.MaEncoder*,System.Void*,System.UInt64,System.UInt64*) d6c3de63611142ccf258aa82b3eb855fab17cfdfc276f6b978d098bbc1f9600b -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderWritePcmFramesProc.BeginInvoke(Inno.Native.MiniAudio.MaEncoder*,System.Void*,System.UInt64,System.UInt64*,System.AsyncCallback,System.Object) ca512094b97c0ff7094b0a0aa78ff9a89fac8bfc5a3479a65fd02aebd28c81e1 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderWritePcmFramesProc.EndInvoke(System.IAsyncResult) a0772783df6d9f88d21cf26aad6da1f71901881a4ef2abc155caf3ce5a4b97de -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b2a7c882d39fac58"></a>[`public Inno.Native.MiniAudio.MaEncoderWritePcmFramesProc.MaEncoderWritePcmFramesProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14246) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ac110bafcfe6bf28"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaEncoderWritePcmFramesProc.BeginInvoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, void* pFramesIn, ulong frameCount, ulong* pFramesWritten, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14246) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b915301e3208b612"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaEncoderWritePcmFramesProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14246) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7375c4228fde12dc"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaEncoderWritePcmFramesProc.Invoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, void* pFramesIn, ulong frameCount, ulong* pFramesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14246) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c7d2ed3ba0482dc1"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaEncoderWritePcmFramesProc(Inno.Native.MiniAudio.MaEncoder* pEncoder, void* pFramesIn, ulong frameCount, ulong* pFramesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14246) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEncoderWriteProc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEncoderWriteProc 9145e10161b69a568becd93a60b75424e2c928e9df163a97ff48dcd85eb802fe -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderWriteProc.#ctor(System.Object,System.IntPtr) bce2250b5e0a1662a6716d9bb39b87eb4a71e3d379b092eff8247d9965306694 -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderWriteProc.Invoke(Inno.Native.MiniAudio.MaEncoder*,System.Void*,System.UIntPtr,System.UIntPtr*) 69d339f0e5e63b74f4bce83a82e99180d2b288adc6fd87f074880e59aaec780c -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderWriteProc.BeginInvoke(Inno.Native.MiniAudio.MaEncoder*,System.Void*,System.UIntPtr,System.UIntPtr*,System.AsyncCallback,System.Object) 10f3715e1d98f72db75635f3812c59f6aabc78360124ea97dba310b65af4c1ab -->
<!-- api: M:Inno.Native.MiniAudio.MaEncoderWriteProc.EndInvoke(System.IAsyncResult) bba3b7756f704c6f43df4148cb92124f21d012fe8a6f8af4dae7d92ab3a1b48c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-bfa3119483113d79"></a>[`public Inno.Native.MiniAudio.MaEncoderWriteProc.MaEncoderWriteProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14226) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-642576a0b064ab91"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaEncoderWriteProc.BeginInvoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, void* pBufferIn, nuint bytesToWrite, nuint* pBytesWritten, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14226) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-73b2dcadb46ff8ea"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaEncoderWriteProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14226) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-29800799d5a8c710"></a>[`public virtual Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaEncoderWriteProc.Invoke(Inno.Native.MiniAudio.MaEncoder* pEncoder, void* pBufferIn, nuint bytesToWrite, nuint* pBytesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14226) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1a5d25b2e75af5e2"></a>[`public delegate Inno.Native.MiniAudio.MaResult Inno.Native.MiniAudio.MaEncoderWriteProc(Inno.Native.MiniAudio.MaEncoder* pEncoder, void* pBufferIn, nuint bytesToWrite, nuint* pBytesWritten)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14226) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEncodingFormat</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEncodingFormat 6a4c667d3e68136ce99d38ea3ffde1af1d7cd04d60f63d6c9954c4dd353569d7 -->
<!-- api: F:Inno.Native.MiniAudio.MaEncodingFormat.Unknown aefaec6b5639a5ceebdfb8b54057e9256b618d45bc45b230865f50c5d65a229e -->
<!-- api: F:Inno.Native.MiniAudio.MaEncodingFormat.Wav 2d2e267cce63a33ca2a86437cc2e0493125952167f55127c644d9f70f7864f79 -->
<!-- api: F:Inno.Native.MiniAudio.MaEncodingFormat.Flac b3edc19e33c1c7e4a7b2c963b7438f4a1171ffe8d901b63780e3cb46245c7d9b -->
<!-- api: F:Inno.Native.MiniAudio.MaEncodingFormat.Mp3 8f2ab7c167b03e31af089804ec53aab919d8a2d3debeed869ba9e1b42a237c08 -->
<!-- api: F:Inno.Native.MiniAudio.MaEncodingFormat.Vorbis 1731c1dce75442c5440535d240aab6bc0868ca40b5150087abd9e1f698ead246 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7411651cfb789fcb"></a>[`Inno.Native.MiniAudio.MaEncodingFormat.Flac = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L649) | 当前常量或枚举值：`2`。 | — |
| <a id="api-fab70ad56d41b73e"></a>[`Inno.Native.MiniAudio.MaEncodingFormat.Mp3 = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L650) | 当前常量或枚举值：`3`。 | — |
| <a id="api-9f79be064602da29"></a>[`Inno.Native.MiniAudio.MaEncodingFormat.Unknown = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L647) | 当前常量或枚举值：`0`。 | — |
| <a id="api-3e119e57f9ce059d"></a>[`Inno.Native.MiniAudio.MaEncodingFormat.Vorbis = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L651) | 当前常量或枚举值：`4`。 | — |
| <a id="api-1e68f84852576438"></a>[`Inno.Native.MiniAudio.MaEncodingFormat.Wav = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L648) | 当前常量或枚举值：`1`。 | — |
| <a id="api-fbef70bedd11129b"></a>[`public enum Inno.Native.MiniAudio.MaEncodingFormat : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L645) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEngine</code> · 25 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEngine 7605c22f56878491aa605627097f260fd900062a504d8cbc9ca202f398a7e8e3 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.NodeGraph a233863f2f6286b0d22a69cf65f95a892512aee7b1a422c703a72b5df709e07c -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.PResourceManager 37e5cab744240dd7ae59474cbac8bf14f8716cfe121a568e4f607896e432a33a -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.PDevice 96a1cbc967230377196ddc3106394c908a7b94b43d7f809a946fa4c4adf76ed9 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.PLog 5117a327801c52dbce9aeaff2ee1b60ac9ce679e2aeccb59d84a16eb688fa59e -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.SampleRate 314db153121dca27857d3f8a394a2f44c3019f1fe66a5364d1e3217ed589478f -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.ListenerCount 05e8cf88de03a386547bc1c58d7407248b7825030a1db36f96471ff310a4c6a4 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.Listeners_0 4c1f6c60909f398a94aa5f9df10afc1897a82224df8fbaf2a348b63f316009fa -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.Listeners_1 e8b51231a44f570de21e4d41f51709021ecb4cb566124f19e146055987ca7ed7 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.Listeners_2 75203e6c77346024c1bbff0fcdc45e433f86916a1fa4cb4ad12de32ebd33384e -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.Listeners_3 35c7f8ae66616b5643a86c63a7e87735ff4e3a67dac1c4237cc5a5177e705ad5 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngine.Listeners 2f339acf370dd74533d3544bffffb5469fd3f363230d99e582ff730a1805ea11 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.AllocationCallbacks 2cfa7f37dde89e9d941e213d05b002895c502ce8af694f5f02d043dc915fbb4e -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.OwnsResourceManager a2e2aa976dba0d1d8b85f12878809d8524beb74a2517a00eae8366a0dc367b54 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.OwnsDevice 35291822e8c8f978e5c09585ae33deccac03cced030b2cb1281aee7526413381 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.InlinedSoundLock 50e0f9ccab0ceffb8e92e1adf980eeb0a519361d9f964b75200f0025f42c8bd8 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.PInlinedSoundHead a4f4f15c552d1b8848f944859d4407c77ebd96e2d7e95a9353fcaf881bd155fd -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.InlinedSoundCount 6bf08574f11fc2bf2c8ef60d001e836033e6f245c66b330f63ead512056a2229 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.GainSmoothTimeInFrames 06d9426a9454494de98656e0f9ab5d9f54149a7a9f5e5d75975db09cd504feff -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.DefaultVolumeSmoothTimeInPcmFrames 766a39ab1653798a21540d992c22aecbc1e536fba7de24cbb8c6c4844421e711 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.MonoExpansionMode 5e2ed24270cab427d0ad0e2912a74c11e75f8b64c7c2c23e12d02637de56d913 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.OnProcess 8f1ed03476d225fa96764f9166160cd6eded602ea677155137ae80d537dafcf9 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.PProcessUserData 468232976ce75c5539ad9b5478d0fb34c7e558f382fa3ecefabd269a3637fcdd -->
<!-- api: F:Inno.Native.MiniAudio.MaEngine.PitchResamplingConfig 9c98b0aa88c5c24f94b9fa7a2979d1c6e099c1a6faabcc8dc7167fa7f4159168 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngine.#ctor(Inno.Native.MiniAudio.MaNodeGraph,Inno.Native.MiniAudio.MaResourceManager*,Inno.Native.MiniAudio.MaDevice*,Inno.Native.MiniAudio.MaLog*,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaSpatializerListener*,Inno.Native.MiniAudio.MaAllocationCallbacks,System.Byte,System.Byte,System.UInt32,Inno.Native.MiniAudio.MaSoundInlined*,System.UInt32,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaMonoExpansionMode,System.Void*,System.Void*,Inno.Native.MiniAudio.MaResamplerConfig) c77b4c630edb2dffaf53e45eb4f37488be111b3dab45a889bf53d33e33d4e601 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8eaec81c3aa0dc23"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaEngine.AllocationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13671) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6b10c1bb80f81208"></a>[`public uint Inno.Native.MiniAudio.MaEngine.DefaultVolumeSmoothTimeInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13678) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f761f1fc3976acb"></a>[`public uint Inno.Native.MiniAudio.MaEngine.GainSmoothTimeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13677) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cddb335e7b703440"></a>[`public uint Inno.Native.MiniAudio.MaEngine.InlinedSoundCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13676) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f859298af5808694"></a>[`public uint Inno.Native.MiniAudio.MaEngine.InlinedSoundLock`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13674) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e443383c16dd9137"></a>[`public uint Inno.Native.MiniAudio.MaEngine.ListenerCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13664) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5d0599c6d63314b4"></a>[`public Inno.Native.MiniAudio.MaSpatializerListener Inno.Native.MiniAudio.MaEngine.Listeners_0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13665) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-db4f92a7466c9602"></a>[`public Inno.Native.MiniAudio.MaSpatializerListener Inno.Native.MiniAudio.MaEngine.Listeners_1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13666) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-352c3168bcaf2ac9"></a>[`public Inno.Native.MiniAudio.MaSpatializerListener Inno.Native.MiniAudio.MaEngine.Listeners_2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13667) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-090123ad1a227c5a"></a>[`public Inno.Native.MiniAudio.MaSpatializerListener Inno.Native.MiniAudio.MaEngine.Listeners_3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13668) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9428b0d315bd729a"></a>[`public Inno.Native.MiniAudio.MaMonoExpansionMode Inno.Native.MiniAudio.MaEngine.MonoExpansionMode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13679) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0317f7f220d5644b"></a>[`public Inno.Native.MiniAudio.MaNodeGraph Inno.Native.MiniAudio.MaEngine.NodeGraph`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13659) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f4cf5adc699c9cdc"></a>[`public void* Inno.Native.MiniAudio.MaEngine.OnProcess`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13680) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ce9c5c37808adbc"></a>[`public byte Inno.Native.MiniAudio.MaEngine.OwnsDevice`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13673) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-afed1f597637d46f"></a>[`public byte Inno.Native.MiniAudio.MaEngine.OwnsResourceManager`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13672) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f666f599a01d3cce"></a>[`public Inno.Native.MiniAudio.MaDevice* Inno.Native.MiniAudio.MaEngine.PDevice`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13661) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6b29a43a31b2c28"></a>[`public Inno.Native.MiniAudio.MaSoundInlined* Inno.Native.MiniAudio.MaEngine.PInlinedSoundHead`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13675) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af1e88c6c68e9858"></a>[`public Inno.Native.MiniAudio.MaResamplerConfig Inno.Native.MiniAudio.MaEngine.PitchResamplingConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13682) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bd6fff27ca9842b2"></a>[`public Inno.Native.MiniAudio.MaLog* Inno.Native.MiniAudio.MaEngine.PLog`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13662) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ebd91dafba31fe65"></a>[`public void* Inno.Native.MiniAudio.MaEngine.PProcessUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13681) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce140cfffb946971"></a>[`public Inno.Native.MiniAudio.MaResourceManager* Inno.Native.MiniAudio.MaEngine.PResourceManager`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13660) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5e6d08207f1c82eb"></a>[`public uint Inno.Native.MiniAudio.MaEngine.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13663) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc8ae574ed3c9564"></a>[`public Inno.Native.MiniAudio.MaEngine.MaEngine(Inno.Native.MiniAudio.MaNodeGraph nodeGraph = default(Inno.Native.MiniAudio.MaNodeGraph), Inno.Native.MiniAudio.MaResourceManager* pResourceManager = null, Inno.Native.MiniAudio.MaDevice* pDevice = null, Inno.Native.MiniAudio.MaLog* pLog = null, uint sampleRate = 0, uint listenerCount = 0, Inno.Native.MiniAudio.MaSpatializerListener* listeners = null, Inno.Native.MiniAudio.MaAllocationCallbacks allocationCallbacks = default(Inno.Native.MiniAudio.MaAllocationCallbacks), byte ownsResourceManager = 0, byte ownsDevice = 0, uint inlinedSoundLock = 0, Inno.Native.MiniAudio.MaSoundInlined* pInlinedSoundHead = null, uint inlinedSoundCount = 0, uint gainSmoothTimeInFrames = 0, uint defaultVolumeSmoothTimeInPCMFrames = 0, Inno.Native.MiniAudio.MaMonoExpansionMode monoExpansionMode = Inno.Native.MiniAudio.MaMonoExpansionMode.Default, void* onProcess = null, void* pProcessUserData = null, Inno.Native.MiniAudio.MaResamplerConfig pitchResamplingConfig = default(Inno.Native.MiniAudio.MaResamplerConfig))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13683) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5b0218e9d16845cd"></a>[`public System.Span<Inno.Native.MiniAudio.MaSpatializerListener> Inno.Native.MiniAudio.MaEngine.Listeners { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13669) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-701442711bd1c9c3"></a>[`public struct Inno.Native.MiniAudio.MaEngine`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13657) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEngineConfig</code> · 27 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEngineConfig 0b012f45bfbf619a48e1b4797379a6fde0fb9162dd7a82dd24fe8899db1c4889 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.PResourceManager a64e7aee7a32a1bb3db8b5d122241716d2d1ca38d82cb19a94177d1ebacd3de4 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.PContext eb722fa2f7eab32ae6e0c74e63ef692d4d059cadfc56f585feac5a9dd8702084 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.PDevice 1ea3901ce21470cf7d65fdc3bcfe5e93fe3f1c19fbc954aed1d178a04222cb0c -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.PPlaybackDeviceId 4efcacc851a027a2319f31df8851936270300866a759110efb47ec18c5c9dcbd -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.DataCallback 0ba6cb63c86d81a7f79250674a84e7e25066dcccf607f9c41451ace40bdabcac -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.NotificationCallback 7acc8d238992a44fe62f23149858b83059434917ff6cb90bc84fc9d05df83ac3 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.PLog c5149af04ee541baa1a5662af1b8209a7204d095d75b56936a074f0afbfbf98a -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.ListenerCount 9a45156b194e07057acc780c1d82e14203829e7875d151b7b7e6b774058f1bed -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.Channels a48f1006c9b679edd5d637646942451af7c3253ea9aaa14abd097f17c69e29a3 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.SampleRate 277b110be83ace95fd31b8262492104b89fd5ccc5b6397462f3c1d85bd9e980d -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.PeriodSizeInFrames 43bd15c085b617b62723f4f5dd66621ba328cced3153aad569b6856acebaf5e1 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.PeriodSizeInMilliseconds b0475372350ecfc4e61c9678a58c8178f7704600a18b7a40ce453ca02043ba29 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.GainSmoothTimeInFrames 00b5165f445a57818c22af52d6d1e907793b2db77b54ba7a9582a7dbcfcf55ad -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.GainSmoothTimeInMilliseconds 4c21d98a63c0b7f5ce471161287c63089fac3e4611fe335401000e070a162468 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.DefaultVolumeSmoothTimeInPcmFrames bd93aafa3104f809befa3131c4d80caf5199c3ff68561444cc943c3b36451142 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.PreMixStackSizeInBytes 389bf27f7a6bc713bdbb3ad06972649c1ca696715fcf29e1707b6761d2276d4f -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.AllocationCallbacks 485cdbfeab0e8561be9e863dbcdca0e35f56180a791607a9c7917083757e78e4 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.NoAutoStart 8b35a210989f51310a606acfec3e686c38412e02706237867cc4136a0e282904 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.NoDevice fbff0c99e266f42ca4df944e148c9cf4377e30c9a214e6cbef5777960299e3eb -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.MonoExpansionMode d640b8bc8fe51996bd7fc6ce22857c695075005591854b2d68f0493616273dc3 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.PResourceManagerVfs b1db6f1128d1f33b33560aa680ce7192e3db8ee8080fce19afca2523798753d4 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.OnProcess e0ee6f5b003c857574903b1809b947392d402b0570cd3e3ed2f4d1bda4e2b955 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.PProcessUserData 479b7a1a6ab72efcfe70c091bbdef2830ff72ee24a1e4dfd4b7e10cd305f4864 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.ResourceManagerResampling 5d2d912e84487490a14471a1f99110ede1159e0da0692f48aa1fe0dee08016a1 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfig.PitchResampling 30a47dc838a513997958ca15f3d6e6197203e7366119d7eef38f87e3605de68f -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineConfig.#ctor(Inno.Native.MiniAudio.MaResourceManager*,Inno.Native.MiniAudio.MaContext*,Inno.Native.MiniAudio.MaDevice*,Inno.Native.MiniAudio.MaDeviceId*,System.Void*,System.Void*,Inno.Native.MiniAudio.MaLog*,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaAllocationCallbacks,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaMonoExpansionMode,Inno.Native.MiniAudio.MaVfs*,System.Void*,System.Void*,Inno.Native.MiniAudio.MaResamplerConfig,Inno.Native.MiniAudio.MaResamplerConfig) 7c7ccee92f7e5fc2dd0f686643db0f6508453e5326fc1743094fd977e0f2a25b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b46b2d701d85308b"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaEngineConfig.AllocationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13822) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4fef5d999690c8c5"></a>[`public uint Inno.Native.MiniAudio.MaEngineConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13814) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-30e5d97999a3ade6"></a>[`public void* Inno.Native.MiniAudio.MaEngineConfig.DataCallback`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13810) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3da7de0450377a23"></a>[`public uint Inno.Native.MiniAudio.MaEngineConfig.DefaultVolumeSmoothTimeInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13820) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bcd507927e593455"></a>[`public uint Inno.Native.MiniAudio.MaEngineConfig.GainSmoothTimeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13818) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5fe699e84caf204a"></a>[`public uint Inno.Native.MiniAudio.MaEngineConfig.GainSmoothTimeInMilliseconds`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13819) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a4695ded8e28a78"></a>[`public uint Inno.Native.MiniAudio.MaEngineConfig.ListenerCount`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13813) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c4866154bb75e36"></a>[`public Inno.Native.MiniAudio.MaMonoExpansionMode Inno.Native.MiniAudio.MaEngineConfig.MonoExpansionMode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13825) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-204458febbe0128e"></a>[`public uint Inno.Native.MiniAudio.MaEngineConfig.NoAutoStart`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13823) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b54288e2fdef485b"></a>[`public uint Inno.Native.MiniAudio.MaEngineConfig.NoDevice`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13824) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d506675dd681a5cd"></a>[`public void* Inno.Native.MiniAudio.MaEngineConfig.NotificationCallback`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13811) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6279e1205d6286ab"></a>[`public void* Inno.Native.MiniAudio.MaEngineConfig.OnProcess`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13827) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3417396295b1ba9b"></a>[`public Inno.Native.MiniAudio.MaContext* Inno.Native.MiniAudio.MaEngineConfig.PContext`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13807) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd6c4fe238dd937d"></a>[`public Inno.Native.MiniAudio.MaDevice* Inno.Native.MiniAudio.MaEngineConfig.PDevice`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13808) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-71a4bbd5825e1c22"></a>[`public uint Inno.Native.MiniAudio.MaEngineConfig.PeriodSizeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13816) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe1e48325afed211"></a>[`public uint Inno.Native.MiniAudio.MaEngineConfig.PeriodSizeInMilliseconds`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13817) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-107e6ddfc718dfd9"></a>[`public Inno.Native.MiniAudio.MaResamplerConfig Inno.Native.MiniAudio.MaEngineConfig.PitchResampling`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13830) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e1841edfdda3bc69"></a>[`public Inno.Native.MiniAudio.MaLog* Inno.Native.MiniAudio.MaEngineConfig.PLog`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13812) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-caf447cc80dc1bcb"></a>[`public Inno.Native.MiniAudio.MaDeviceId* Inno.Native.MiniAudio.MaEngineConfig.PPlaybackDeviceId`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13809) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c003842af04b04dd"></a>[`public void* Inno.Native.MiniAudio.MaEngineConfig.PProcessUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13828) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2431018d5fdc91e4"></a>[`public uint Inno.Native.MiniAudio.MaEngineConfig.PreMixStackSizeInBytes`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13821) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b4fcc023ff28426d"></a>[`public Inno.Native.MiniAudio.MaResourceManager* Inno.Native.MiniAudio.MaEngineConfig.PResourceManager`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13806) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf21aae364bba6ad"></a>[`public Inno.Native.MiniAudio.MaVfs* Inno.Native.MiniAudio.MaEngineConfig.PResourceManagerVfs`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13826) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb86d6cc4e4b9eec"></a>[`public Inno.Native.MiniAudio.MaResamplerConfig Inno.Native.MiniAudio.MaEngineConfig.ResourceManagerResampling`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13829) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-677073e7370ecba3"></a>[`public uint Inno.Native.MiniAudio.MaEngineConfig.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13815) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-71f6b5e5ad0d5e6d"></a>[`public Inno.Native.MiniAudio.MaEngineConfig.MaEngineConfig(Inno.Native.MiniAudio.MaResourceManager* pResourceManager = null, Inno.Native.MiniAudio.MaContext* pContext = null, Inno.Native.MiniAudio.MaDevice* pDevice = null, Inno.Native.MiniAudio.MaDeviceId* pPlaybackDeviceID = null, void* dataCallback = null, void* notificationCallback = null, Inno.Native.MiniAudio.MaLog* pLog = null, uint listenerCount = 0, uint channels = 0, uint sampleRate = 0, uint periodSizeInFrames = 0, uint periodSizeInMilliseconds = 0, uint gainSmoothTimeInFrames = 0, uint gainSmoothTimeInMilliseconds = 0, uint defaultVolumeSmoothTimeInPCMFrames = 0, uint preMixStackSizeInBytes = 0, Inno.Native.MiniAudio.MaAllocationCallbacks allocationCallbacks = default(Inno.Native.MiniAudio.MaAllocationCallbacks), uint noAutoStart = 0, uint noDevice = 0, Inno.Native.MiniAudio.MaMonoExpansionMode monoExpansionMode = Inno.Native.MiniAudio.MaMonoExpansionMode.Default, Inno.Native.MiniAudio.MaVfs* pResourceManagerVFS = null, void* onProcess = null, void* pProcessUserData = null, Inno.Native.MiniAudio.MaResamplerConfig resourceManagerResampling = default(Inno.Native.MiniAudio.MaResamplerConfig), Inno.Native.MiniAudio.MaResamplerConfig pitchResampling = default(Inno.Native.MiniAudio.MaResamplerConfig))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13831) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-19ad99698c4bbabf"></a>[`public struct Inno.Native.MiniAudio.MaEngineConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13804) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEngineConfigPtr</code> · 38 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEngineConfigPtr 8e62190c6f9cc51038fe93c54f592c4126b76cf4a6b110ba5ea12e8cde6ecf3c -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineConfigPtr.#ctor(Inno.Native.MiniAudio.MaEngineConfig*) 5c2f7b31d439e93674564fa10a8fbd0f9b866cecd5f91e30a75c9649a7d7d008 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineConfigPtr.Handle 64b3cdd22f86dbb03892c4621dd6bc7cd20ac298e5a17afd24d35746c263af37 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.IsNull ffb45492fd01eece652057429ad827fb85f72ed9667999bec1723bb39ac7ae2d -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.Null afd85a46477ff09717a2e9a032fdb3f6e40f0763c17362af23d195ab8fa39cc2 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.Item(System.Int32) a12007a6d67c4dbeefad61919ef6bb8c51a32a5c6e112433805d1adc1f59f6d8 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaEngineConfig*)~Inno.Native.MiniAudio.MaEngineConfigPtr 08a422335b8a3f3e3ae9bb3158f96a38b161967350abc41ff1dcdc589b4eb677 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaEngineConfigPtr)~Inno.Native.MiniAudio.MaEngineConfig* 33041d8dd48bf3d594da79c88f465638d2ec4270cad7340456eba672ad969fe8 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineConfigPtr.op_Equality(Inno.Native.MiniAudio.MaEngineConfigPtr,Inno.Native.MiniAudio.MaEngineConfigPtr) d002d6867aa65fed3149cb140facaf5b5d6475bc334695d6099e898427f6a075 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaEngineConfigPtr,Inno.Native.MiniAudio.MaEngineConfigPtr) 6108dca801107d487a3c5a49c537b9d2f11c7779cdda3640f910fab4f4450d03 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineConfigPtr.Equals(Inno.Native.MiniAudio.MaEngineConfigPtr) 46af8515d1eebc045364e6646f18e5c0b6eb9b124e5ab87fca24e7dd756511a1 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineConfigPtr.Equals(System.Object) f3a09370b321189506a561994016cef020192714fb19c95960807b3dac5e508d -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineConfigPtr.GetHashCode 642d8cc6888051225eb416c4e74bd8e4e3d1c8c9a922219301a1808c08bbd134 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.PResourceManager 66d8cbdcbbc92f0843c80ce2b1d7319269c14a508a5b416bb078b4ac6e1ceb31 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.PContext 26d57ed426df1efd826c8a71e8a390bd6a7be972a3be5581b98127a5b2de555e -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.PDevice e7e2744a98bd39554790e294b8647b9ef3d821dad3e482767bb1c1b212b86616 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.PPlaybackDeviceId f5ab75573b409b4085e25837da45ef12965a854ceffda2dd27898021bbcba3ce -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.DataCallback e0fd489702b35c06047a85b993a3613a453902cce9e097098846c33ea4c40811 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.NotificationCallback 3ebc629e513831f6066f6361f4da95ff0e9ce0f7faa5e7f3b524e1b791475251 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.PLog 62e14e991de75ef08b25f00b47d756f8de18a5073e4389205657bc8982b669e2 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.ListenerCount ea2ca8a08df385ba532ec8822562029d6e883ecaa43570d30f7f699e653dc459 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.Channels e66d5f2588cc421b2872a9952798cb4e948aef7ea07bedd7d768375579dd35ac -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.SampleRate 80b302981fbe34cde216c897ea03f7043ef3b972532e2180cbfb5c85dcd16111 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.PeriodSizeInFrames bb6c89d1a8deb293d4a3c297a40dac4032036fd0e8396c1754fae4df5462cd5b -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.PeriodSizeInMilliseconds 8a277f2d12b45243d68bdd1f6f6afec30ed6d18cc5b93a264fd6834688cbfbd6 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.GainSmoothTimeInFrames 98a03332cb721ca6f3c99ed23e5fb3485599d972b1710c9500fab03b9178b983 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.GainSmoothTimeInMilliseconds caa97a7038905429b4216c851de27187f354c2590e806735dd245e0655aa7eab -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.DefaultVolumeSmoothTimeInPcmFrames 3c22c6aa1f007b45413f4d51bfa76067ef03fba38841d3bc250e81eadeaf4bce -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.PreMixStackSizeInBytes 0c4f13a6919574fce1eb15695fc3deaf399d875ba987634c185ef3e0f167c7f0 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.AllocationCallbacks 24bf2047a8d119ba0da204a8bbefa247c9f8c6efca72de743c41878f1781ea91 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.NoAutoStart 4369c5b24a7b52fb125765e45521d4788b1a729604ae89dd068d45bc48d0cacb -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.NoDevice d9efa3269d8218bc09348bfa30e1119273001278f1a0750255534cb3fe50bb92 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.MonoExpansionMode 5680f6669fa95f2ce0f26ee8940a708c1005326116fa9b52afe15d6ebfa0355e -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.PResourceManagerVfs 3078ef7a6730b3df40feff7d002abc38a28b273cd95a5e5d4718fc1e3973ed9e -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.OnProcess cefb92210aadb3e6c9f2eeceb536493e5eda8ee0175e16e5a6ca214d081874b0 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.PProcessUserData 444a98ca6b285d95b109dd435d51df219dd7cea4a3c5bcd3b1ee3abcb1539cad -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.ResourceManagerResampling aae3582cdf0d9c402c29bcd2f788aba04aacc09e2cbedf53897a8244ad873380 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineConfigPtr.PitchResampling 83809b5f756b5996156487e4a9c8d8720009f827b42368e6cc8f36041364ffb9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4df4df98acae3059"></a>[`public Inno.Native.MiniAudio.MaEngineConfig* Inno.Native.MiniAudio.MaEngineConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15824) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b697478c9ef8b9f7"></a>[`public Inno.Native.MiniAudio.MaEngineConfigPtr.MaEngineConfigPtr(Inno.Native.MiniAudio.MaEngineConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15819) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a7293f1d4c641330"></a>[`public bool Inno.Native.MiniAudio.MaEngineConfigPtr.Equals(Inno.Native.MiniAudio.MaEngineConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15834) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-3b3bbb18f2a3d7b3"></a>[`public override bool Inno.Native.MiniAudio.MaEngineConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15835) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-08e654f0cceaf8e4"></a>[`public override int Inno.Native.MiniAudio.MaEngineConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15836) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-860ee9a432919954"></a>[`public static bool Inno.Native.MiniAudio.MaEngineConfigPtr.operator ==(Inno.Native.MiniAudio.MaEngineConfigPtr left, Inno.Native.MiniAudio.MaEngineConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15832) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2ff3625a9aa53706"></a>[`public static Inno.Native.MiniAudio.MaEngineConfigPtr.implicit operator Inno.Native.MiniAudio.MaEngineConfigPtr(Inno.Native.MiniAudio.MaEngineConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15830) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-850a737908eab754"></a>[`public static Inno.Native.MiniAudio.MaEngineConfigPtr.implicit operator Inno.Native.MiniAudio.MaEngineConfig*(Inno.Native.MiniAudio.MaEngineConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15831) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a1124c4e22ef501c"></a>[`public static bool Inno.Native.MiniAudio.MaEngineConfigPtr.operator !=(Inno.Native.MiniAudio.MaEngineConfigPtr left, Inno.Native.MiniAudio.MaEngineConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15833) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ad99ddb45c17e0aa"></a>[`public ref Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaEngineConfigPtr.AllocationCallbacks { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15854) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4eeb2b5fe2ab77ce"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineConfigPtr.Channels { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15846) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ffd27ee550ef6075"></a>[`public void* Inno.Native.MiniAudio.MaEngineConfigPtr.DataCallback { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15842) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e1b810314772612f"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineConfigPtr.DefaultVolumeSmoothTimeInPcmFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15852) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-044295cf8de2da6b"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineConfigPtr.GainSmoothTimeInFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15850) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-087e7311f532f130"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineConfigPtr.GainSmoothTimeInMilliseconds { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15851) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8681ffe9c1aa0bf8"></a>[`public bool Inno.Native.MiniAudio.MaEngineConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15825) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-14d65759ef39a28e"></a>[`public Inno.Native.MiniAudio.MaEngineConfig Inno.Native.MiniAudio.MaEngineConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15828) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-27b45914ac9ed548"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineConfigPtr.ListenerCount { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15845) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ab83f3901b62c9c0"></a>[`public ref Inno.Native.MiniAudio.MaMonoExpansionMode Inno.Native.MiniAudio.MaEngineConfigPtr.MonoExpansionMode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15857) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c00d6292fdce3c87"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineConfigPtr.NoAutoStart { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15855) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-40b7b9360854407a"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineConfigPtr.NoDevice { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15856) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0063704978752a43"></a>[`public void* Inno.Native.MiniAudio.MaEngineConfigPtr.NotificationCallback { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15843) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1011abd975281963"></a>[`public static Inno.Native.MiniAudio.MaEngineConfigPtr Inno.Native.MiniAudio.MaEngineConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15826) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-05018d99274fd414"></a>[`public void* Inno.Native.MiniAudio.MaEngineConfigPtr.OnProcess { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15859) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-394b42f105103cb6"></a>[`public ref Inno.Native.MiniAudio.MaContextPtr Inno.Native.MiniAudio.MaEngineConfigPtr.PContext { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15839) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bd792c672579d457"></a>[`public ref Inno.Native.MiniAudio.MaDevicePtr Inno.Native.MiniAudio.MaEngineConfigPtr.PDevice { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15840) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-802842b13dd2c246"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineConfigPtr.PeriodSizeInFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15848) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bc130e7c4a2f1dd7"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineConfigPtr.PeriodSizeInMilliseconds { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15849) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-97cb45c39a286d8a"></a>[`public ref Inno.Native.MiniAudio.MaResamplerConfig Inno.Native.MiniAudio.MaEngineConfigPtr.PitchResampling { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15862) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a4e292f3999830ca"></a>[`public ref Inno.Native.MiniAudio.MaLogPtr Inno.Native.MiniAudio.MaEngineConfigPtr.PLog { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15844) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a64e7c3243c2f630"></a>[`public ref Inno.Native.MiniAudio.MaDeviceIdPtr Inno.Native.MiniAudio.MaEngineConfigPtr.PPlaybackDeviceId { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15841) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cdc2cbd66e75682a"></a>[`public void* Inno.Native.MiniAudio.MaEngineConfigPtr.PProcessUserData { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15860) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7ed3a660c8b3409f"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineConfigPtr.PreMixStackSizeInBytes { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15853) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-da120d568885b701"></a>[`public ref Inno.Native.MiniAudio.MaResourceManagerPtr Inno.Native.MiniAudio.MaEngineConfigPtr.PResourceManager { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15838) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0086baced2d265ac"></a>[`public Inno.Native.MiniAudio.MaVfs* Inno.Native.MiniAudio.MaEngineConfigPtr.PResourceManagerVfs { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15858) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fb67ee9fe70dbd36"></a>[`public ref Inno.Native.MiniAudio.MaResamplerConfig Inno.Native.MiniAudio.MaEngineConfigPtr.ResourceManagerResampling { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15861) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ef7f6be0dfbbbe5"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineConfigPtr.SampleRate { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15847) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bafcce9c525e8051"></a>[`public struct Inno.Native.MiniAudio.MaEngineConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaEngineConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15817) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEngineNode</code> · 22 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEngineNode b4381cec2f2f9845168cd48953c8947eaa7ac509016fddd1d9c1d9e185d3a8e5 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.BaseNode ddd101d11f245430a410a5ef449bc441e6d7d39fdd98d15cf809ee8bd60a543f -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.PEngine 8f35b2f59e69032d8af0789657bc9940e74f862023adbb54b4f9118e63ecf0a3 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.SampleRate d18f28405ce91e196c06979608f7de04b739a6e421ceada2544a2617687b514d -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.VolumeSmoothTimeInPcmFrames 9b890bc7ee5a86cca25d91d34507cfdcdb1560e6b4d7f235a7bf2cba42f2df9d -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.MonoExpansionMode 36196896f9484cbdc3aab157b896970eacffbe9f250f1cb81c6aabeeb93ec71c -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.Fader fb1e8aa0cd3321b946d6d167c87e1a85ee05c409960bb9c9891e2ead12dc34e3 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.Resampler 3850633307a1463ff50dafe837d36fa2009ceff67677c0d957b8f9f52af8af9c -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.Spatializer aca83fd254398b8fb0f378300a20cb85d10fd4ea6ea76366a3464ec3b1255f8a -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.Panner bad7962b1565ef296e25083ebc30218d17c155da822ab4f01641d509a735110c -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.VolumeGainer 24cd104f64613ccfc6da71eb0da9f8d58114f2ab39ed97a572d1988c49336004 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.Volume 70590ba4d754b970fdf8dfa8cb54cf132b01450541d663fac047c3e3bde857fa -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.Pitch 7958dc3f35d55f7e760195c72aa06db5f9348373a6e39eaf1104fc5bda137624 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.OldPitch e4f34665941d4c44b61b5fc6fd7599d9f6dc27258ef91b39740f28fa2b99d171 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.OldDopplerPitch c34463c34a66f31ae4f6f16790e652771827b188abb767257fdf241b9713a346 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.IsPitchDisabled 43fee1f56c15025427a19417806fd01fe0a1f7a1e8a801f24e065e35bdba4042 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.IsSpatializationDisabled b3b3f924aeab505a6aaf2834796eeac3e2104a3d5670ae9ce73d7cfb7e1e68df -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.PinnedListenerIndex 65277e115eee32d025a198434b5339e46c00552c39749ded42ea58bdae00c24a -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.FadeSettings b458256fcb472f1b9a06a79306082d220e618a05c6cb8a37c929fc26f9aeb0f7 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.Ownsheap b7515cfff730b52577e1f3b4d6b9d28fb42707eacf709bdc420d7882d27fe6a7 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.Pheap cff6f5082b7d767008694331b026a822727d04d0448ec272bc44f95a1ae4a800 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNode.#ctor(Inno.Native.MiniAudio.MaNodeBase,Inno.Native.MiniAudio.MaEngine*,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaMonoExpansionMode,Inno.Native.MiniAudio.MaFader,Inno.Native.MiniAudio.MaResampler,Inno.Native.MiniAudio.MaSpatializer,Inno.Native.MiniAudio.MaPanner,Inno.Native.MiniAudio.MaGainer,Inno.Native.MiniAudio.MaAtomicFloat,System.Single,System.Single,System.Single,System.UInt32,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous,System.Byte,System.Void*) 87dee93a9a958a590a2d74980153ba262eeed421bece0d4d6ec07ed852061704 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-86d540bce51a009e"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaEngineNode.BaseNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13558) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8107d6d1513bc36f"></a>[`public Inno.Native.MiniAudio.MaFader Inno.Native.MiniAudio.MaEngineNode.Fader`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13563) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d20abc52f3a55b2a"></a>[`public Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous Inno.Native.MiniAudio.MaEngineNode.FadeSettings`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13575) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8995ae89c4d92f66"></a>[`public uint Inno.Native.MiniAudio.MaEngineNode.IsPitchDisabled`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13572) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a47cd6bdf657bc5"></a>[`public uint Inno.Native.MiniAudio.MaEngineNode.IsSpatializationDisabled`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13573) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6bede2eb327c05f"></a>[`public Inno.Native.MiniAudio.MaMonoExpansionMode Inno.Native.MiniAudio.MaEngineNode.MonoExpansionMode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13562) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8a28298fe806da44"></a>[`public float Inno.Native.MiniAudio.MaEngineNode.OldDopplerPitch`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13571) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2adb11f7786a80e4"></a>[`public float Inno.Native.MiniAudio.MaEngineNode.OldPitch`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13570) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3453872a1f28c033"></a>[`public byte Inno.Native.MiniAudio.MaEngineNode.Ownsheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13576) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-258b3ce79fe957d8"></a>[`public Inno.Native.MiniAudio.MaPanner Inno.Native.MiniAudio.MaEngineNode.Panner`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13566) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60a423e1bdebd6c8"></a>[`public Inno.Native.MiniAudio.MaEngine* Inno.Native.MiniAudio.MaEngineNode.PEngine`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13559) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-24451dc0732db76c"></a>[`public void* Inno.Native.MiniAudio.MaEngineNode.Pheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13577) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d12f2d8934ef67a5"></a>[`public uint Inno.Native.MiniAudio.MaEngineNode.PinnedListenerIndex`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13574) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21f364b0caf1c0d4"></a>[`public float Inno.Native.MiniAudio.MaEngineNode.Pitch`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13569) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd0e4b7f9bf07258"></a>[`public Inno.Native.MiniAudio.MaResampler Inno.Native.MiniAudio.MaEngineNode.Resampler`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13564) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-14d3cf1dd690ab78"></a>[`public uint Inno.Native.MiniAudio.MaEngineNode.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13560) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7986beb290d47122"></a>[`public Inno.Native.MiniAudio.MaSpatializer Inno.Native.MiniAudio.MaEngineNode.Spatializer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13565) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ef9de94e75a3f867"></a>[`public Inno.Native.MiniAudio.MaAtomicFloat Inno.Native.MiniAudio.MaEngineNode.Volume`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13568) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-edbf9a2a895a5d1d"></a>[`public Inno.Native.MiniAudio.MaGainer Inno.Native.MiniAudio.MaEngineNode.VolumeGainer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13567) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1a33f9810807c08b"></a>[`public uint Inno.Native.MiniAudio.MaEngineNode.VolumeSmoothTimeInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13561) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e90f2af826ba9191"></a>[`public Inno.Native.MiniAudio.MaEngineNode.MaEngineNode(Inno.Native.MiniAudio.MaNodeBase baseNode = default(Inno.Native.MiniAudio.MaNodeBase), Inno.Native.MiniAudio.MaEngine* pEngine = null, uint sampleRate = 0, uint volumeSmoothTimeInPCMFrames = 0, Inno.Native.MiniAudio.MaMonoExpansionMode monoExpansionMode = Inno.Native.MiniAudio.MaMonoExpansionMode.Default, Inno.Native.MiniAudio.MaFader fader = default(Inno.Native.MiniAudio.MaFader), Inno.Native.MiniAudio.MaResampler resampler = default(Inno.Native.MiniAudio.MaResampler), Inno.Native.MiniAudio.MaSpatializer spatializer = default(Inno.Native.MiniAudio.MaSpatializer), Inno.Native.MiniAudio.MaPanner panner = default(Inno.Native.MiniAudio.MaPanner), Inno.Native.MiniAudio.MaGainer volumeGainer = default(Inno.Native.MiniAudio.MaGainer), Inno.Native.MiniAudio.MaAtomicFloat volume = default(Inno.Native.MiniAudio.MaAtomicFloat), float pitch = 0, float oldPitch = 0, float oldDopplerPitch = 0, uint isPitchDisabled = 0, uint isSpatializationDisabled = 0, uint pinnedListenerIndex = 0, Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous fadeSettings = default(Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous), byte ownsHeap = 0, void* pHeap = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13578) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1048388fec106276"></a>[`public struct Inno.Native.MiniAudio.MaEngineNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13537) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous 93524e68512461c0ff485e4ebeac3ef94c85e14075433fbded87a25b05a63d7e -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous.VolumeBeg 6ec4234df6d132711363e71d984879bddd7356b27531ff07e06c0305fec74a6e -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous.VolumeEnd d6c7f33f7801aad67a6f3280949f4aaa730c50710eac9915e4e966467355454b -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous.FadeLengthInFrames 30de3749c2efb5963889e30122909cb26d4abaf8b28732966636b3a0efb229b1 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous.AbsoluteGlobalTimeInFrames 5c0f380184259ca41f9c1a7378f31071010efeaf5e5f68bad3d7fab0d9f8c66a -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous.#ctor(Inno.Native.MiniAudio.MaAtomicFloat,Inno.Native.MiniAudio.MaAtomicFloat,Inno.Native.MiniAudio.MaAtomicUint64,Inno.Native.MiniAudio.MaAtomicUint64) 9539cf4214275e92727f3fb752ad059a7e4d844ecc5e46d22bf8c39822b150fc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e05d7966aa505f9c"></a>[`public Inno.Native.MiniAudio.MaAtomicUint64 Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous.AbsoluteGlobalTimeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13548) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc2374d00fbdc47b"></a>[`public Inno.Native.MiniAudio.MaAtomicUint64 Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous.FadeLengthInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13547) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01fcee41c9ec0ab0"></a>[`public Inno.Native.MiniAudio.MaAtomicFloat Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous.VolumeBeg`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13545) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-32f85c3f525bfc0d"></a>[`public Inno.Native.MiniAudio.MaAtomicFloat Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous.VolumeEnd`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13546) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e615c5cb95dc3904"></a>[`public Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous.FadeSettingsAnonymous(Inno.Native.MiniAudio.MaAtomicFloat volumeBeg = default(Inno.Native.MiniAudio.MaAtomicFloat), Inno.Native.MiniAudio.MaAtomicFloat volumeEnd = default(Inno.Native.MiniAudio.MaAtomicFloat), Inno.Native.MiniAudio.MaAtomicUint64 fadeLengthInFrames = default(Inno.Native.MiniAudio.MaAtomicUint64), Inno.Native.MiniAudio.MaAtomicUint64 absoluteGlobalTimeInFrames = default(Inno.Native.MiniAudio.MaAtomicUint64))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13549) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2604157601044a42"></a>[`public struct Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13543) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEngineNodeConfig</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEngineNodeConfig ce051395bacc4462cd672671e46797ff95715a7568a96f311a1285f7287df03f -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeConfig.PEngine 490decda93a45b279108c969bea5dd801d8f977252a0bc3c5818fd48e1acb962 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeConfig.Type a9073a9b35b553b01534b67b75c4ddf3576e3705945ab2e2327a30709c140abf -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeConfig.ChannelsIn 39dcada4a59210fbfd1ddafe8215008c1b114b47d3210403e8ca1a40d3a4d58c -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeConfig.ChannelsOut 818ec092dd1a18aad33bbe07511f6dd536b0e934bbc2ea90ba2a635f916e895b -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeConfig.SampleRate 06d9c70cfca1c0b9c8fa6848d2b7beb3445aed639975e47c5fc7acec5de8e072 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeConfig.VolumeSmoothTimeInPcmFrames d41525f1a7c8288c2e4895c0bea0edab4c87c682d55c78a08aa1a5f56fead913 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeConfig.MonoExpansionMode e23c8cb587974e84114135c97864af3fc9abc51a65d923d746107d877a69c2be -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeConfig.IsPitchDisabled 580af5e98cefcf4a6f2686c6aa7d3df2c621135001612099ccda317b0f41b3a1 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeConfig.IsSpatializationDisabled ea3b1701ec43fdad7b6f5db3b6a323488841c768d0f23de71507584485e4a2a8 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeConfig.PinnedListenerIndex 414c12b0f9bff9b0b392194e9b8ac111d004247860f4e548e20e7f729b533838 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeConfig.Resampling 528d04af90e1620984a7801d324c4f99398059616cd6abf2e61589a981e4995e -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodeConfig.#ctor(Inno.Native.MiniAudio.MaEngine*,Inno.Native.MiniAudio.MaEngineNodeType,System.UInt32,System.UInt32,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaMonoExpansionMode,System.Byte,System.Byte,System.Byte,Inno.Native.MiniAudio.MaResamplerConfig) 520b72866693714fbc54ab39550ad9467618eda05468cb0b77545a10812ffaf1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5eb3d7dc8a5c63f3"></a>[`public uint Inno.Native.MiniAudio.MaEngineNodeConfig.ChannelsIn`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13722) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89874be08b10ed50"></a>[`public uint Inno.Native.MiniAudio.MaEngineNodeConfig.ChannelsOut`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13723) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c5012cb54f32aad9"></a>[`public byte Inno.Native.MiniAudio.MaEngineNodeConfig.IsPitchDisabled`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13727) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c7ddf93bb2a13c61"></a>[`public byte Inno.Native.MiniAudio.MaEngineNodeConfig.IsSpatializationDisabled`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13728) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c1aed393f0ca4bf1"></a>[`public Inno.Native.MiniAudio.MaMonoExpansionMode Inno.Native.MiniAudio.MaEngineNodeConfig.MonoExpansionMode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13726) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4ace8bfc76f30942"></a>[`public Inno.Native.MiniAudio.MaEngine* Inno.Native.MiniAudio.MaEngineNodeConfig.PEngine`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13720) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9bd610d7886e6535"></a>[`public byte Inno.Native.MiniAudio.MaEngineNodeConfig.PinnedListenerIndex`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13729) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e4b1e94beda80759"></a>[`public Inno.Native.MiniAudio.MaResamplerConfig Inno.Native.MiniAudio.MaEngineNodeConfig.Resampling`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13730) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-534fbee88fee40bf"></a>[`public uint Inno.Native.MiniAudio.MaEngineNodeConfig.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13724) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-986d360fab4ef524"></a>[`public Inno.Native.MiniAudio.MaEngineNodeType Inno.Native.MiniAudio.MaEngineNodeConfig.Type`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13721) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-72cfd5404e0db83c"></a>[`public uint Inno.Native.MiniAudio.MaEngineNodeConfig.VolumeSmoothTimeInPcmFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13725) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-523e88c6eb6ee16b"></a>[`public Inno.Native.MiniAudio.MaEngineNodeConfig.MaEngineNodeConfig(Inno.Native.MiniAudio.MaEngine* pEngine = null, Inno.Native.MiniAudio.MaEngineNodeType type = Inno.Native.MiniAudio.MaEngineNodeType.Sound, uint channelsIn = 0, uint channelsOut = 0, uint sampleRate = 0, uint volumeSmoothTimeInPCMFrames = 0, Inno.Native.MiniAudio.MaMonoExpansionMode monoExpansionMode = Inno.Native.MiniAudio.MaMonoExpansionMode.Default, byte isPitchDisabled = 0, byte isSpatializationDisabled = 0, byte pinnedListenerIndex = 0, Inno.Native.MiniAudio.MaResamplerConfig resampling = default(Inno.Native.MiniAudio.MaResamplerConfig))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13731) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2d06c0c5855a3eed"></a>[`public struct Inno.Native.MiniAudio.MaEngineNodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13718) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEngineNodeConfigPtr</code> · 24 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEngineNodeConfigPtr 4fbf33b01dc01bca3c1352108b5f24213bb8678344a9a4d1f82607299737df3b -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.#ctor(Inno.Native.MiniAudio.MaEngineNodeConfig*) 8344b7f14a6165aec8fc9d6c8081e040ee77a176baa9915239ccba3021e6cbeb -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.Handle 0aba10bd0a2cf78b072aa92a6a494dbbac0143e9c626e493fb924a85c5df3fa5 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.IsNull 9d61baba0bbaee5b735a76ab63b2b767007aa3233190692cc801301c31e447a5 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.Null db93c3be98f52639cc997c34bd3c10f0ede9eb247745f1aabb3db5ad7f7e0830 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.Item(System.Int32) 46b3956436fa42a38efa6a779fce8f6ef783c094188ea2a2e8d5d7a57a633d26 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaEngineNodeConfig*)~Inno.Native.MiniAudio.MaEngineNodeConfigPtr e61b2cfabb72e46ae2d44c3458b996fbdeaf2731abe54de62bf1c8510b568c25 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaEngineNodeConfigPtr)~Inno.Native.MiniAudio.MaEngineNodeConfig* 25fe8e25d777687065fc7c564ce75b7c69892575431a5c79089a8d77a5b9efe9 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.op_Equality(Inno.Native.MiniAudio.MaEngineNodeConfigPtr,Inno.Native.MiniAudio.MaEngineNodeConfigPtr) 020eb75a836665e5fdb3aecae9935ccd2ad5552580ecdef65229e1195d291b2c -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaEngineNodeConfigPtr,Inno.Native.MiniAudio.MaEngineNodeConfigPtr) 6728d00de6ada726c97527f80b4f7d4f38a0b5801af47814fa3a0e5ebdc54257 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.Equals(Inno.Native.MiniAudio.MaEngineNodeConfigPtr) 6ddd447ac152927baab5de367e12e1b07169cb406726695cf8e2d118e1cd3ccf -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.Equals(System.Object) b5ae66bce85ab65d0c5b00c176b38f5fb321eee00619b85ff40ef551619afe5d -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.GetHashCode fd695835cddf925dd6a82754c279ce6c2c107c02a64173b0b1e2fbbfd0718847 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.PEngine 334a9eb0d06c51b1898025da8a4917b12261aa693e611ca725f7cee4a57aa60d -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.Type e8874c0bf8f80ffaea58613de3f84566ecbd50f12c7af245f0fbda2c3a563158 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.ChannelsIn 97bfd7b990c1af48a6bc29f99fc979eddfbe09bb26825d634c776a7df5ee7e9a -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.ChannelsOut dc197daed700c6c7ca8c6fdbe389852a86ffa7a10be23266512459ff61cca242 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.SampleRate bdaaf73dd9056c697cfb19f16f5f790299266956e8d208fa8a4ebda16d67728b -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.VolumeSmoothTimeInPcmFrames 60f300a15a5268bbd5440992c86e5db0a4a9681d4a7d97e64d41afdbf0c9f4d3 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.MonoExpansionMode 43d98204d1ae061de27d0d7e3d0d304bc678257b673447e7a8054694f29f5500 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.IsPitchDisabled dea68dc3fefacf73989461ac6549f7aa15097d1d914a69bccce8b2be80a3a59a -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.IsSpatializationDisabled 1de48a1ef7135dbd0d52f29238789b9c53180f6a2f36f8af38cb06276da413f2 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.PinnedListenerIndex efef544e08fffd1fdd07f98e12489793efbe768436dcb588aeac69f319bf0019 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodeConfigPtr.Resampling 2d90b87d28366c8ab63f63e6180d4f0e236e555699239d9c5ccf339fefa0e6c9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-63627bce80716fbb"></a>[`public Inno.Native.MiniAudio.MaEngineNodeConfig* Inno.Native.MiniAudio.MaEngineNodeConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15873) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-db8b40ebece43e5f"></a>[`public Inno.Native.MiniAudio.MaEngineNodeConfigPtr.MaEngineNodeConfigPtr(Inno.Native.MiniAudio.MaEngineNodeConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15868) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-167dd10cf724987c"></a>[`public bool Inno.Native.MiniAudio.MaEngineNodeConfigPtr.Equals(Inno.Native.MiniAudio.MaEngineNodeConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15883) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-d687d1f58dffd3ab"></a>[`public override bool Inno.Native.MiniAudio.MaEngineNodeConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15884) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-190c14e34b5c790a"></a>[`public override int Inno.Native.MiniAudio.MaEngineNodeConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15885) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-cdd9c14754f5f911"></a>[`public static bool Inno.Native.MiniAudio.MaEngineNodeConfigPtr.operator ==(Inno.Native.MiniAudio.MaEngineNodeConfigPtr left, Inno.Native.MiniAudio.MaEngineNodeConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15881) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c53b8b03d41f79a8"></a>[`public static Inno.Native.MiniAudio.MaEngineNodeConfigPtr.implicit operator Inno.Native.MiniAudio.MaEngineNodeConfigPtr(Inno.Native.MiniAudio.MaEngineNodeConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15879) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7a2664cc7caf4b11"></a>[`public static Inno.Native.MiniAudio.MaEngineNodeConfigPtr.implicit operator Inno.Native.MiniAudio.MaEngineNodeConfig*(Inno.Native.MiniAudio.MaEngineNodeConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15880) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-85720b3ffc929978"></a>[`public static bool Inno.Native.MiniAudio.MaEngineNodeConfigPtr.operator !=(Inno.Native.MiniAudio.MaEngineNodeConfigPtr left, Inno.Native.MiniAudio.MaEngineNodeConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15882) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-30322ef3f4abf4d3"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineNodeConfigPtr.ChannelsIn { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15889) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-26e681e926565d41"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineNodeConfigPtr.ChannelsOut { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15890) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3ab0687b368cc459"></a>[`public bool Inno.Native.MiniAudio.MaEngineNodeConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15874) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-010068c0c9158f10"></a>[`public ref byte Inno.Native.MiniAudio.MaEngineNodeConfigPtr.IsPitchDisabled { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15894) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a2806a37e03a7c14"></a>[`public ref byte Inno.Native.MiniAudio.MaEngineNodeConfigPtr.IsSpatializationDisabled { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15895) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a3ba54448e288986"></a>[`public Inno.Native.MiniAudio.MaEngineNodeConfig Inno.Native.MiniAudio.MaEngineNodeConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15877) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-947683afb8ea789d"></a>[`public ref Inno.Native.MiniAudio.MaMonoExpansionMode Inno.Native.MiniAudio.MaEngineNodeConfigPtr.MonoExpansionMode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15893) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dd79584bc74eb24c"></a>[`public static Inno.Native.MiniAudio.MaEngineNodeConfigPtr Inno.Native.MiniAudio.MaEngineNodeConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15875) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f6826b6fe1a60ef0"></a>[`public ref Inno.Native.MiniAudio.MaEnginePtr Inno.Native.MiniAudio.MaEngineNodeConfigPtr.PEngine { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15887) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-babb35d36c3e6b2e"></a>[`public ref byte Inno.Native.MiniAudio.MaEngineNodeConfigPtr.PinnedListenerIndex { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15896) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-760a4cfba45b1ddb"></a>[`public ref Inno.Native.MiniAudio.MaResamplerConfig Inno.Native.MiniAudio.MaEngineNodeConfigPtr.Resampling { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15897) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a93a250f4f684017"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineNodeConfigPtr.SampleRate { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15891) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b055cd2c36a5d83b"></a>[`public ref Inno.Native.MiniAudio.MaEngineNodeType Inno.Native.MiniAudio.MaEngineNodeConfigPtr.Type { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15888) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c37eabf8021db0dc"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineNodeConfigPtr.VolumeSmoothTimeInPcmFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15892) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b43dcb20193fac5c"></a>[`public struct Inno.Native.MiniAudio.MaEngineNodeConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaEngineNodeConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15866) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEngineNodePtr</code> · 33 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEngineNodePtr a57607bb0fd81fa7f683aa3cb8002354179af96208f1b5a8cc92f2fd1e4538c5 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodePtr.#ctor(Inno.Native.MiniAudio.MaEngineNode*) 401c2c72f25ffb7eff7100181a9ad566e0a8ea425df1f6df5bb07f763bcb6995 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodePtr.Handle 9af4016b57bfb58a90ac6a37aa0bb1ddec84fa942a427594e20d14721ede0796 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.IsNull a25d0d5548b7b6bd04e6321605debe51829110c5b205a58de0477f89f5752704 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.Null dd680b30ccbc962e77aae0ed156cec315448d39e260a48db753245bb466f359b -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.Item(System.Int32) 292273076e0a040f770c7916b05b921ccc3bce436fcdc8a199e9d1a00b3b195d -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodePtr.op_Implicit(Inno.Native.MiniAudio.MaEngineNode*)~Inno.Native.MiniAudio.MaEngineNodePtr 8d5026c4041058e80b8f8f9539a986cc90d455063d37e3af401631ce6b374eb9 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodePtr.op_Implicit(Inno.Native.MiniAudio.MaEngineNodePtr)~Inno.Native.MiniAudio.MaEngineNode* cdd71c842064632b401403afa1488f6a1663ad00ca3b0ddb7dbc1cd692b990ac -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodePtr.op_Equality(Inno.Native.MiniAudio.MaEngineNodePtr,Inno.Native.MiniAudio.MaEngineNodePtr) 7a4a467247d8520928b409fbb45a4994464930319d2bec270761ca6e09870e4b -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodePtr.op_Inequality(Inno.Native.MiniAudio.MaEngineNodePtr,Inno.Native.MiniAudio.MaEngineNodePtr) 4dea696cc402d3ccf6a444bea904afcef06d89d353b9e57ef392131662dbf68b -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodePtr.Equals(Inno.Native.MiniAudio.MaEngineNodePtr) ec0e73ce47522bc5bc23af5b4316f0b24a9161a142d78f278445d5461afec0a7 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodePtr.Equals(System.Object) 5d33e0239d73bb7e85bd784fea27de543169880e4d10a5901c1d870902ad5173 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineNodePtr.GetHashCode 4bc921a90ad8ffafb08ec4332a9c7cdb65b62290323e36dc8c480a16aa8c74d4 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.BaseNode 437d971683e051fa25de9402cc22d27027313b78f12a2eef3256f7f33560fab6 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.PEngine 3cddb775e36721a10260a8ccc9346f40e536da9e4a99b9d3318cb8623e4fa507 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.SampleRate 6d4a4740b52780798847c92a1abbd191092fc4581d96c7a6d616f790d53fc64e -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.VolumeSmoothTimeInPcmFrames f13adb6b05c49e51945ba5e9dc87f1696369b709a61f0bd97f8b87bbca6064ea -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.MonoExpansionMode a213348167afd1ce83b2f46e97670a820a274d0be70430deb30a687f203f2901 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.Fader b89b732ba9b4dd5adf4671df5ff629c968d73228dd90a559c12833f56b64ea71 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.Resampler 846daca2f0970c3e3864dc500c9fe39eba93a42d54aef908a5c78478b7b2cdee -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.Spatializer 94cd9bade981ac8562508add8616d3bc205485e65c1fdf03d4531d1aed3fd361 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.Panner 49fbdc5f59c4d26cc127b37ff4126c3b4a6392181c39143cd6dc6278fd5c1bc9 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.VolumeGainer ed20de6db6dca0299ea11604164f0778dea8f2342a7a38b684e5a229ce1b372e -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.Volume a52faae579f26aa3759132e728724fc75f0651905edd122acc89a8a88d385e7a -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.Pitch bab4b2c1dc0857c2b17c93869446e98e380bbfa995edc2cc30ad91d914d894a7 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.OldPitch 98ee0c3984eef939f54cea77c5330ebb23576a90e259e8a29641f1cc8e01234b -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.OldDopplerPitch 96d4adc5055114361b855097ab1ba8dce1cb58f148be370f7e133cef861a9f12 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.IsPitchDisabled 4ec9e034feb489fdda180ac884db519e7ba1055662fdc00cfa30b62b17388831 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.IsSpatializationDisabled fe225bbc7d985656c0897aedf650976cbe2d582c5ef81fd82f5b4f0c93a78cde -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.PinnedListenerIndex 9d76d1a4801ed569e2d0fa5ea4f9960d42b942b41aefebe7a74a59a97010d604 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.FadeSettings 2a28723c479c27c2cd66b52a44cec82b1eeb667d3d3d111316d9d8f75de3e571 -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.Ownsheap 0a51745989ff0725308c7c97a322c40c7aa19b06bef332ff65b4c8d4665482ca -->
<!-- api: P:Inno.Native.MiniAudio.MaEngineNodePtr.Pheap 3bf2d475db653035b8dd9ee64eff1e70cf2f81a1cbf3448e4adeaca5f37a3c51 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0f9862bf83206d75"></a>[`public Inno.Native.MiniAudio.MaEngineNode* Inno.Native.MiniAudio.MaEngineNodePtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15908) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-898a0e4b23968084"></a>[`public Inno.Native.MiniAudio.MaEngineNodePtr.MaEngineNodePtr(Inno.Native.MiniAudio.MaEngineNode* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15903) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f8387a6e8eb36f37"></a>[`public bool Inno.Native.MiniAudio.MaEngineNodePtr.Equals(Inno.Native.MiniAudio.MaEngineNodePtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15918) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-23c420616af10b4a"></a>[`public override bool Inno.Native.MiniAudio.MaEngineNodePtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15919) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-c9108a8fcaa7bb9b"></a>[`public override int Inno.Native.MiniAudio.MaEngineNodePtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15920) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-9e2ffbea8c0661fe"></a>[`public static bool Inno.Native.MiniAudio.MaEngineNodePtr.operator ==(Inno.Native.MiniAudio.MaEngineNodePtr left, Inno.Native.MiniAudio.MaEngineNodePtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15916) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a959d13526c67d27"></a>[`public static Inno.Native.MiniAudio.MaEngineNodePtr.implicit operator Inno.Native.MiniAudio.MaEngineNodePtr(Inno.Native.MiniAudio.MaEngineNode* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15914) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-94d0d2af286c61bf"></a>[`public static Inno.Native.MiniAudio.MaEngineNodePtr.implicit operator Inno.Native.MiniAudio.MaEngineNode*(Inno.Native.MiniAudio.MaEngineNodePtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15915) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3c89ba839fd42e88"></a>[`public static bool Inno.Native.MiniAudio.MaEngineNodePtr.operator !=(Inno.Native.MiniAudio.MaEngineNodePtr left, Inno.Native.MiniAudio.MaEngineNodePtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15917) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-50c5e5ec4c6e1eea"></a>[`public ref Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaEngineNodePtr.BaseNode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15922) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6673198fea65f0ae"></a>[`public ref Inno.Native.MiniAudio.MaFader Inno.Native.MiniAudio.MaEngineNodePtr.Fader { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15927) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4b61a90c489b2192"></a>[`public ref Inno.Native.MiniAudio.MaEngineNode.FadeSettingsAnonymous Inno.Native.MiniAudio.MaEngineNodePtr.FadeSettings { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15939) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-98f80a279d009e04"></a>[`public bool Inno.Native.MiniAudio.MaEngineNodePtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15909) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b482e7a4def12ba1"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineNodePtr.IsPitchDisabled { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15936) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-98c15539f8011bdb"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineNodePtr.IsSpatializationDisabled { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15937) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5cb12d2103ea8f8d"></a>[`public Inno.Native.MiniAudio.MaEngineNode Inno.Native.MiniAudio.MaEngineNodePtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15912) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cf062f777c01ba0f"></a>[`public ref Inno.Native.MiniAudio.MaMonoExpansionMode Inno.Native.MiniAudio.MaEngineNodePtr.MonoExpansionMode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15926) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ac9c6f0b837b6a3"></a>[`public static Inno.Native.MiniAudio.MaEngineNodePtr Inno.Native.MiniAudio.MaEngineNodePtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15910) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d3a1ad72d5d1b8f2"></a>[`public ref float Inno.Native.MiniAudio.MaEngineNodePtr.OldDopplerPitch { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15935) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cf25862e9e1c9e70"></a>[`public ref float Inno.Native.MiniAudio.MaEngineNodePtr.OldPitch { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15934) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ff0b262bf9e68067"></a>[`public ref byte Inno.Native.MiniAudio.MaEngineNodePtr.Ownsheap { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15940) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6db4dab82ce56090"></a>[`public ref Inno.Native.MiniAudio.MaPanner Inno.Native.MiniAudio.MaEngineNodePtr.Panner { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15930) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-233197b3d6814604"></a>[`public ref Inno.Native.MiniAudio.MaEnginePtr Inno.Native.MiniAudio.MaEngineNodePtr.PEngine { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15923) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cb62044bdc56b31b"></a>[`public void* Inno.Native.MiniAudio.MaEngineNodePtr.Pheap { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15941) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-623ef14ae1bcbf00"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineNodePtr.PinnedListenerIndex { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15938) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c6ba70499dee82d0"></a>[`public ref float Inno.Native.MiniAudio.MaEngineNodePtr.Pitch { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15933) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2cd30fbe5f954df1"></a>[`public ref Inno.Native.MiniAudio.MaResampler Inno.Native.MiniAudio.MaEngineNodePtr.Resampler { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15928) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-667b1f251f249e27"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineNodePtr.SampleRate { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15924) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-adc2a4792fdadaa5"></a>[`public ref Inno.Native.MiniAudio.MaSpatializer Inno.Native.MiniAudio.MaEngineNodePtr.Spatializer { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15929) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f09d606b3a45cf20"></a>[`public ref Inno.Native.MiniAudio.MaAtomicFloat Inno.Native.MiniAudio.MaEngineNodePtr.Volume { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15932) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-89c607d8b3df5758"></a>[`public ref Inno.Native.MiniAudio.MaGainer Inno.Native.MiniAudio.MaEngineNodePtr.VolumeGainer { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15931) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-51011f10a303a7ba"></a>[`public ref uint Inno.Native.MiniAudio.MaEngineNodePtr.VolumeSmoothTimeInPcmFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15925) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5db2469548e24f5d"></a>[`public struct Inno.Native.MiniAudio.MaEngineNodePtr : System.IEquatable<Inno.Native.MiniAudio.MaEngineNodePtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15901) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEngineNodeType</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEngineNodeType 99d98b8fd9a17e8dc0d303f69e751e8f2e970dafdee1c1a1d70f9b0afde27872 -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeType.Sound 6f2c335bac94885bfb847d514c549cc8c52b6b25568af1051aaacda4d1bcf6df -->
<!-- api: F:Inno.Native.MiniAudio.MaEngineNodeType.Group 2fd06ef4c7ba7b831068579d53b4b621cdf4ece0f6991cd55a7c088e05c017fd -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1409aa627250c369"></a>[`Inno.Native.MiniAudio.MaEngineNodeType.Group = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L751) | 当前常量或枚举值：`1`。 | — |
| <a id="api-c17951f150eb41cf"></a>[`Inno.Native.MiniAudio.MaEngineNodeType.Sound = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L750) | 当前常量或枚举值：`0`。 | — |
| <a id="api-eda6500dd601a27d"></a>[`public enum Inno.Native.MiniAudio.MaEngineNodeType : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L748) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEngineProcessProc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEngineProcessProc dfd30df4e60a11a57d4f43037202126e1cf120c6d4e0303ed42a7521eb9b5475 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineProcessProc.#ctor(System.Object,System.IntPtr) 3d703d11f24b8b728e7dece8adecf6bd173ba5383d8f6b0dad64c8dde3001cd3 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineProcessProc.Invoke(System.Void*,System.Single*,System.UInt64) bc52e4a209ce05e21c880fbf150cd6865f58fbb12cf499897cd2d2ab3db83a70 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineProcessProc.BeginInvoke(System.Void*,System.Single*,System.UInt64,System.AsyncCallback,System.Object) ab1317aa5f4b7613039c50d2ac7a62f64af092b9fee7e93b75fbf81888a21fb1 -->
<!-- api: M:Inno.Native.MiniAudio.MaEngineProcessProc.EndInvoke(System.IAsyncResult) b43416cf76ab564cc3b0980d2bf54efef9993c7a6cbc8415927f82cb529ced43 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7db956b435ff3070"></a>[`public Inno.Native.MiniAudio.MaEngineProcessProc.MaEngineProcessProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14296) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e22ba185d7d0c1c6"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaEngineProcessProc.BeginInvoke(void* pUserData, float* pFramesOut, ulong frameCount, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14296) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-549cb343dbdabff4"></a>[`public virtual void Inno.Native.MiniAudio.MaEngineProcessProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14296) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b1bb8d27ccf4d80d"></a>[`public virtual void Inno.Native.MiniAudio.MaEngineProcessProc.Invoke(void* pUserData, float* pFramesOut, ulong frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14296) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9c35d0a9cc85bb14"></a>[`public delegate void Inno.Native.MiniAudio.MaEngineProcessProc(void* pUserData, float* pFramesOut, ulong frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14296) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEnginePtr</code> · 32 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEnginePtr 55b011b96ea00971d5bdd6c921150260361006744cf9ed43ac90319a9c4fdd16 -->
<!-- api: M:Inno.Native.MiniAudio.MaEnginePtr.#ctor(Inno.Native.MiniAudio.MaEngine*) d6dc235f854f4afd31df3bc350921258a606483bae2368a721e122f94b283e32 -->
<!-- api: F:Inno.Native.MiniAudio.MaEnginePtr.Handle a5968bdf34b48536d71cfc8482e6e2950c16806acb02c6975e2f4e64032cbec7 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.IsNull 4f7134fb37b899e75724b7fa79c6f440ed2b07e234dbdf44f67b4f8545ba6194 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.Null c2e51e2454649c32789cb652e3c2849fb4abd6f343b921538bd38eeb77b4a8f9 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.Item(System.Int32) b0d1b0bf9aa7504beb76b7237de57aecca2533b953a7c358cdee7be07bf94d61 -->
<!-- api: M:Inno.Native.MiniAudio.MaEnginePtr.op_Implicit(Inno.Native.MiniAudio.MaEngine*)~Inno.Native.MiniAudio.MaEnginePtr bbaa042cd882333b7b7a595046e024130879f8569889b10d27fa5a06ea1d6b51 -->
<!-- api: M:Inno.Native.MiniAudio.MaEnginePtr.op_Implicit(Inno.Native.MiniAudio.MaEnginePtr)~Inno.Native.MiniAudio.MaEngine* edcec46dc3a6865cf40de6074d761107e21c590d167c5d9e22c71688235a0c52 -->
<!-- api: M:Inno.Native.MiniAudio.MaEnginePtr.op_Equality(Inno.Native.MiniAudio.MaEnginePtr,Inno.Native.MiniAudio.MaEnginePtr) f788dfca0cef78c7d4555a4e28ae399a1ce07ba47809fc458a79cb2cf98103a0 -->
<!-- api: M:Inno.Native.MiniAudio.MaEnginePtr.op_Inequality(Inno.Native.MiniAudio.MaEnginePtr,Inno.Native.MiniAudio.MaEnginePtr) 971a534fcf4d894cdfcb85888d706dddac6b2c91642f71690a27da4f4c55163b -->
<!-- api: M:Inno.Native.MiniAudio.MaEnginePtr.Equals(Inno.Native.MiniAudio.MaEnginePtr) 318a91da0082705feb956fdf21b798a0e97c645e091d1b7614fe9956c9180ec2 -->
<!-- api: M:Inno.Native.MiniAudio.MaEnginePtr.Equals(System.Object) 96ca4f6bf4a6cde39791976302021211b1442d7936c08ac7b9a3129732150682 -->
<!-- api: M:Inno.Native.MiniAudio.MaEnginePtr.GetHashCode c147c0dd5cce6150c0dbcf5f8ceda62ca9db0a4bd5909d50d1e068689367e9bf -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.NodeGraph 659555334c82e73b449a1d8db38ed70f48875e11f4e9694ba227cb6bbc3f1fee -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.PResourceManager 0ab867f52c716659959f065b39b1f4d5eacb5af1f588ea7bf58de225858b3924 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.PDevice 68c3614d1bef54eb746d9fb68c9e35d4972bda101483833ddd83651e37392816 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.PLog 844f210c3135c566a6186159e14e79d7f7e4b38d1f4c3d9c94eef70364bb6869 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.SampleRate d344af74da15728bafc9e9b2463a0f77c6f1238419aba8257baab965f7debe2d -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.ListenerCount e878cb9719ee3026a3943a9e070c4d827d6f12ab08d1da4d4c86de5f4ec86323 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.Listeners 3b5cfc587a1fcfe534f5a7746147c97f41d579914f3bf50f5ea9848abd7b7b8c -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.AllocationCallbacks 641274f77056e03a3a7d57bbaf0c060a6e532146dd18fec992d50fcddbba197c -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.OwnsResourceManager 2a38f4614445147c2fa93a345a826c92c4e59db606224e54cdebcc87515aa601 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.OwnsDevice dcfc8436c954d82157e60139d9be7be1523a27f99319b44fb5c874e80a331529 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.InlinedSoundLock 8db1ce7722c8111b63206ab266fc10bb9b8381b52e524ca12ffd860603649a3e -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.PInlinedSoundHead 05a01d9a6c8aa669d25b4c307c59d153f90fe5bc9233fbf775a2496750c13126 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.InlinedSoundCount 7b19e203980f6efd2284fbd1add61e3747eb939346b9c3099abcd8be5451a340 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.GainSmoothTimeInFrames 27360aa6cb85088f44259877ca7913c67c1b11ec418b4c302c33947d2c9017ed -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.DefaultVolumeSmoothTimeInPcmFrames 335690eb55277af76308f0230435a91cc6e3fd69bd12caabd033008ebd29ae25 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.MonoExpansionMode 619f14292f0b505a432a87aaaecd7de47e62d8fd017e4f212ec348782742f47b -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.OnProcess 5ea16b57fac358413109d1a0ea19a899b24257cda8ab8bb2ea9fecdd46371e52 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.PProcessUserData 8f6fa15f72432ca739da20f9c35b53a7618ccc0c28f3de850d6a332a82c7ad23 -->
<!-- api: P:Inno.Native.MiniAudio.MaEnginePtr.PitchResamplingConfig 4eb237a0b642f820bf6ca6be09e1b911b4c538bcb803a66adccaa0d9720784dd -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dbd1070abb73c4e5"></a>[`public Inno.Native.MiniAudio.MaEngine* Inno.Native.MiniAudio.MaEnginePtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15952) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c85d18ffe6b5fef"></a>[`public Inno.Native.MiniAudio.MaEnginePtr.MaEnginePtr(Inno.Native.MiniAudio.MaEngine* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15947) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9e61af2ac388e8c1"></a>[`public bool Inno.Native.MiniAudio.MaEnginePtr.Equals(Inno.Native.MiniAudio.MaEnginePtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15962) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-70d1eca539db1e62"></a>[`public override bool Inno.Native.MiniAudio.MaEnginePtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15963) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-46bd8d5bb1327627"></a>[`public override int Inno.Native.MiniAudio.MaEnginePtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15964) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-118f176d94c2a11f"></a>[`public static bool Inno.Native.MiniAudio.MaEnginePtr.operator ==(Inno.Native.MiniAudio.MaEnginePtr left, Inno.Native.MiniAudio.MaEnginePtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15960) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0f10b38f0996a961"></a>[`public static Inno.Native.MiniAudio.MaEnginePtr.implicit operator Inno.Native.MiniAudio.MaEnginePtr(Inno.Native.MiniAudio.MaEngine* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15958) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d09bd6ec5a620ab7"></a>[`public static Inno.Native.MiniAudio.MaEnginePtr.implicit operator Inno.Native.MiniAudio.MaEngine*(Inno.Native.MiniAudio.MaEnginePtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15959) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-21100ac02685110c"></a>[`public static bool Inno.Native.MiniAudio.MaEnginePtr.operator !=(Inno.Native.MiniAudio.MaEnginePtr left, Inno.Native.MiniAudio.MaEnginePtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15961) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1feca69b9114626b"></a>[`public ref Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaEnginePtr.AllocationCallbacks { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15973) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f0edb43d0b957da8"></a>[`public ref uint Inno.Native.MiniAudio.MaEnginePtr.DefaultVolumeSmoothTimeInPcmFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15980) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0078377d1720c25b"></a>[`public ref uint Inno.Native.MiniAudio.MaEnginePtr.GainSmoothTimeInFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15979) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6ffeb5451fec1ee2"></a>[`public ref uint Inno.Native.MiniAudio.MaEnginePtr.InlinedSoundCount { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15978) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cb547d800f94cff8"></a>[`public ref uint Inno.Native.MiniAudio.MaEnginePtr.InlinedSoundLock { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15976) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9ba9df0645a95d6e"></a>[`public bool Inno.Native.MiniAudio.MaEnginePtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15953) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-813c225226d21d9e"></a>[`public Inno.Native.MiniAudio.MaEngine Inno.Native.MiniAudio.MaEnginePtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15956) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e862e81b1102511f"></a>[`public ref uint Inno.Native.MiniAudio.MaEnginePtr.ListenerCount { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15971) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2c6804f2f3fdc472"></a>[`public System.Span<Inno.Native.MiniAudio.MaSpatializerListener> Inno.Native.MiniAudio.MaEnginePtr.Listeners { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15972) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-73800c449f2b1ddf"></a>[`public ref Inno.Native.MiniAudio.MaMonoExpansionMode Inno.Native.MiniAudio.MaEnginePtr.MonoExpansionMode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15981) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-70b3e0fdc6cc10d7"></a>[`public ref Inno.Native.MiniAudio.MaNodeGraph Inno.Native.MiniAudio.MaEnginePtr.NodeGraph { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15966) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1c2cba9cb2df5c00"></a>[`public static Inno.Native.MiniAudio.MaEnginePtr Inno.Native.MiniAudio.MaEnginePtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15954) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-94108ee44449b829"></a>[`public void* Inno.Native.MiniAudio.MaEnginePtr.OnProcess { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15982) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0c09780203193c96"></a>[`public ref byte Inno.Native.MiniAudio.MaEnginePtr.OwnsDevice { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15975) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0b6f7da69e631ca7"></a>[`public ref byte Inno.Native.MiniAudio.MaEnginePtr.OwnsResourceManager { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15974) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dd156f7f6d8e407c"></a>[`public ref Inno.Native.MiniAudio.MaDevicePtr Inno.Native.MiniAudio.MaEnginePtr.PDevice { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15968) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-018541e9bab17dae"></a>[`public ref Inno.Native.MiniAudio.MaSoundInlinedPtr Inno.Native.MiniAudio.MaEnginePtr.PInlinedSoundHead { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15977) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dbcdd98f2545a953"></a>[`public ref Inno.Native.MiniAudio.MaResamplerConfig Inno.Native.MiniAudio.MaEnginePtr.PitchResamplingConfig { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15984) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3a8eaa7bdda076f2"></a>[`public ref Inno.Native.MiniAudio.MaLogPtr Inno.Native.MiniAudio.MaEnginePtr.PLog { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15969) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-90166994103ffb12"></a>[`public void* Inno.Native.MiniAudio.MaEnginePtr.PProcessUserData { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15983) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e2ed4bc6f77fb334"></a>[`public ref Inno.Native.MiniAudio.MaResourceManagerPtr Inno.Native.MiniAudio.MaEnginePtr.PResourceManager { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15967) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-61d7bb3bf5bf0c22"></a>[`public ref uint Inno.Native.MiniAudio.MaEnginePtr.SampleRate { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15970) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d330337c1c34a8d6"></a>[`public struct Inno.Native.MiniAudio.MaEnginePtr : System.IEquatable<Inno.Native.MiniAudio.MaEnginePtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L15945) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaEnumDevicesCallbackProc</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaEnumDevicesCallbackProc 31808cbc42c682677c5737376e769a5b113689cb2d0bbea602f1e6484dff0266 -->
<!-- api: M:Inno.Native.MiniAudio.MaEnumDevicesCallbackProc.#ctor(System.Object,System.IntPtr) 0e1e3675d4fdb52d9a0e06e30875b24d35a0f0d2b6749d83f4c0aeeac003c758 -->
<!-- api: M:Inno.Native.MiniAudio.MaEnumDevicesCallbackProc.Invoke(Inno.Native.MiniAudio.MaContext*,Inno.Native.MiniAudio.MaDeviceType,Inno.Native.MiniAudio.MaDeviceInfo*,System.Void*) 3be43886a5e6604b31d67a2b696d738bcdbead4f4635ceb4360772171ba58221 -->
<!-- api: M:Inno.Native.MiniAudio.MaEnumDevicesCallbackProc.BeginInvoke(Inno.Native.MiniAudio.MaContext*,Inno.Native.MiniAudio.MaDeviceType,Inno.Native.MiniAudio.MaDeviceInfo*,System.Void*,System.AsyncCallback,System.Object) 46e223503bd4cde005407e4e82a989c67ff3240aaf79b93e05ca9c1ddec82bc1 -->
<!-- api: M:Inno.Native.MiniAudio.MaEnumDevicesCallbackProc.EndInvoke(System.IAsyncResult) 1040414d430c92e44ae10b3b14f0325cc4452321369ede4ead0cdaf5c92e22dd -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1e6faab07a90f43b"></a>[`public Inno.Native.MiniAudio.MaEnumDevicesCallbackProc.MaEnumDevicesCallbackProc(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14326) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4349e5fd31b7f9b7"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.MaEnumDevicesCallbackProc.BeginInvoke(Inno.Native.MiniAudio.MaContext* pContext, Inno.Native.MiniAudio.MaDeviceType deviceType, Inno.Native.MiniAudio.MaDeviceInfo* pInfo, void* pUserData, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14326) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0686495344419786"></a>[`public virtual uint Inno.Native.MiniAudio.MaEnumDevicesCallbackProc.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14326) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3c227c890cfd2b04"></a>[`public virtual uint Inno.Native.MiniAudio.MaEnumDevicesCallbackProc.Invoke(Inno.Native.MiniAudio.MaContext* pContext, Inno.Native.MiniAudio.MaDeviceType deviceType, Inno.Native.MiniAudio.MaDeviceInfo* pInfo, void* pUserData)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14326) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1279a829c5d71105"></a>[`public delegate uint Inno.Native.MiniAudio.MaEnumDevicesCallbackProc(Inno.Native.MiniAudio.MaContext* pContext, Inno.Native.MiniAudio.MaDeviceType deviceType, Inno.Native.MiniAudio.MaDeviceInfo* pInfo, void* pUserData)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14326) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaFader</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaFader 6fe1ebabf6d77d0dc7b071c1359650281f809161c4802d20b11adef14467c469 -->
<!-- api: F:Inno.Native.MiniAudio.MaFader.Config 3faa7fecf1c40d90036235012e1f4c2656391161b14b4fc2cdf1a05058dc16ee -->
<!-- api: F:Inno.Native.MiniAudio.MaFader.VolumeBeg 85a6f4373cc8e535758d45e0444fa0b130073c2acab26b83874f169e39390204 -->
<!-- api: F:Inno.Native.MiniAudio.MaFader.VolumeEnd be2d7a8eeb82268a3427be8201065453ad98dfc7c55d457490f68a83c2cd5dff -->
<!-- api: F:Inno.Native.MiniAudio.MaFader.LengthInFrames 02d4fbc4d0a5209ace6034767b7608e6f87cf8813f42bab50fdee46f019427f3 -->
<!-- api: F:Inno.Native.MiniAudio.MaFader.CursorInFrames 4217be06c1195df3c72247b13cefc3dcebbe07e5b677101967e231b8c6be24ef -->
<!-- api: M:Inno.Native.MiniAudio.MaFader.#ctor(Inno.Native.MiniAudio.MaFaderConfig,System.Single,System.Single,System.UInt64,System.Int64) 1c86bb0d5247f522803b8349bb9d84fdf627d040753640027c74e2179719af0b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-71c8486a3fd1b79d"></a>[`public Inno.Native.MiniAudio.MaFaderConfig Inno.Native.MiniAudio.MaFader.Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11015) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a310c81932cad7d4"></a>[`public long Inno.Native.MiniAudio.MaFader.CursorInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11019) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e471eb52d8e33730"></a>[`public ulong Inno.Native.MiniAudio.MaFader.LengthInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11018) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c7e3428d893369fe"></a>[`public float Inno.Native.MiniAudio.MaFader.VolumeBeg`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11016) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad5d38822585175e"></a>[`public float Inno.Native.MiniAudio.MaFader.VolumeEnd`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11017) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b302bb7a00c792f"></a>[`public Inno.Native.MiniAudio.MaFader.MaFader(Inno.Native.MiniAudio.MaFaderConfig config = default(Inno.Native.MiniAudio.MaFaderConfig), float volumeBeg = 0, float volumeEnd = 0, ulong lengthInFrames = 0, long cursorInFrames = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11020) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-63719b69bf7dd2bd"></a>[`public struct Inno.Native.MiniAudio.MaFader`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11013) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaFaderConfig</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaFaderConfig ec953b1a2f81d902d304917c43bcf13d2800eb26dea78ad6254173679244e70b -->
<!-- api: F:Inno.Native.MiniAudio.MaFaderConfig.Format 6fb2f05c501fc129fd0266b47655982eb2cf4ae14fb007573531a1ca5150e6db -->
<!-- api: F:Inno.Native.MiniAudio.MaFaderConfig.Channels e737281b96cdb62d206235c2b1e0791921405c77c15f8a0ce6993962b69d1d75 -->
<!-- api: F:Inno.Native.MiniAudio.MaFaderConfig.SampleRate d483cf59e7f7bc90ae05ddebbb6155123e1ca8a041a57f8d0093a2dee7f489bc -->
<!-- api: M:Inno.Native.MiniAudio.MaFaderConfig.#ctor(Inno.Native.MiniAudio.MaFormat,System.UInt32,System.UInt32) 35f28f5ac9903f23cfc7cea948899bfd0d6271bd6c1ab274d727012f2a4fc2b7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-23fc3991fb19b251"></a>[`public uint Inno.Native.MiniAudio.MaFaderConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10999) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d167b87529965d6d"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaFaderConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10998) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a14108e72c2754dc"></a>[`public uint Inno.Native.MiniAudio.MaFaderConfig.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11000) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7f8ebe9a48e13a5"></a>[`public Inno.Native.MiniAudio.MaFaderConfig.MaFaderConfig(Inno.Native.MiniAudio.MaFormat format = Inno.Native.MiniAudio.MaFormat.Unknown, uint channels = 0, uint sampleRate = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11001) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-28e7c289690c8f94"></a>[`public struct Inno.Native.MiniAudio.MaFaderConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10996) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
