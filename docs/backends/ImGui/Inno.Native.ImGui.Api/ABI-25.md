# Inno.Native.ImGui ABI · ImGuiTextFlags → ImGuiWindowClass

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImGuiTextFlags</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTextFlags 23d1a0d012e08c424886fbd1f59ff7048b470fd7f6351c251ac6177e0ce44e5f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFlags.None 537453d7f24da5562d6c75a3996885d318856e75070f60747945b50755fdd344 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextFlags.NoWidthForLargeClippedText 71c4c4733b4d53bb57620cbdeb7472791ab473b42dbec4cd721c72ff4f0d5b8e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d97a653199b4969a"></a>[`Inno.Native.ImGui.ImGuiTextFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1369) | 当前常量或枚举值：`0`。 | — |
| <a id="api-9d6b1f41af55d91a"></a>[`Inno.Native.ImGui.ImGuiTextFlags.NoWidthForLargeClippedText = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1370) | 当前常量或枚举值：`1`。 | — |
| <a id="api-3d973e836af92294"></a>[`public enum Inno.Native.ImGui.ImGuiTextFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1367) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTextIndex</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTextIndex 99a6e43d754c798e085cbbc77f241b409090781529cf56bd8b41929f1b225785 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextIndex.Offsets 2a0d2a7743f3f3b9cc7477392b7c4d4774b76d01788e3ad31695f4a84a4ab5a4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextIndex.EndOffset ae1e632d6cb1ca091f5e5dd07aaad960369d731efa338f03907e2e51d178f1bb -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndex.#ctor(Inno.Native.ImGui.ImVector{System.Int32},System.Int32) 33e6f6b0ac83c82ebac71b1c672dee4889af783b94f807aac4a285f2a74dee3c -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndex.clear 49397e5866e9e26cfa9fe0a7d6035c25922e0a83f310eca5ca1ec06ba39e8146 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndex.size b474a9f4eee2c0c14e84bd71fcea4a028d83ceeb0cf9bd408704e531dc49e65b -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndex.get_line_begin(System.Byte*,System.Int32) f6de29fee68b70fe091c9c074e68f63dca0099e6cea5272f0655fa600603df51 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndex.get_line_end(System.Byte*,System.Int32) 6c9855512a038f0b9e065864e43e1fc7b26428ad016a8664b6c90d98c7eb9a5d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndex.append(System.Byte*,System.Int32,System.Int32) a6a4e971105eb71f995cbd8f4af5f6b413f038ea81864f7de7dfb1cd1af0d87e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4ae3d87f4969ca18"></a>[`public int Inno.Native.ImGui.ImGuiTextIndex.EndOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7069) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ac8e58baada4ede4"></a>[`public Inno.Native.ImGui.ImVector<int> Inno.Native.ImGui.ImGuiTextIndex.Offsets`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7068) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c93fcb7d862248fc"></a>[`public Inno.Native.ImGui.ImGuiTextIndex.ImGuiTextIndex(Inno.Native.ImGui.ImVector<int> offsets = default(Inno.Native.ImGui.ImVector<int>), int endOffset = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7070) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-115676e98a7c81e3"></a>[`public void Inno.Native.ImGui.ImGuiTextIndex.append(byte* baseValue, int oldSize, int newSize)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66363) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-af2c5009ea3a6e63"></a>[`public void Inno.Native.ImGui.ImGuiTextIndex.clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66319) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9e92620b9eff8316"></a>[`public byte* Inno.Native.ImGui.ImGuiTextIndex.get_line_begin(byte* baseValue, int n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66341) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-07e0754e4dc5271f"></a>[`public byte* Inno.Native.ImGui.ImGuiTextIndex.get_line_end(byte* baseValue, int n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66352) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d0bf0f6986821b5d"></a>[`public int Inno.Native.ImGui.ImGuiTextIndex.size()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66330) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-448b4a7cbd336560"></a>[`public struct Inno.Native.ImGui.ImGuiTextIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7066) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTextIndexPtr</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTextIndexPtr 997c065450cad47f39fe3d6f0105c02b3ff26149d8e0a617920c5107c5934d0c -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndexPtr.#ctor(Inno.Native.ImGui.ImGuiTextIndex*) fbbe449ef5b8f9142ebf032a58eda8559b3f1e46deca0125175c84fd48bbeb00 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextIndexPtr.Handle b461fd67f30f0b3572f73928dd7528210deea6ed6310be07f03046a195535765 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextIndexPtr.IsNull 515045fdf81b130e9081272f5698b785f9667d18c83e0d0aef5ae8f66d89c852 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextIndexPtr.Null 822b4720ca830b4b5c90d4e60ee59d7a6e8acc72625804610abcbb3c4feca267 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextIndexPtr.Item(System.Int32) c861cf5100b51f72a139794e69a551b56beb982e70255da1f7d4727b679c5bb3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndexPtr.op_Implicit(Inno.Native.ImGui.ImGuiTextIndex*)~Inno.Native.ImGui.ImGuiTextIndexPtr 4af55af12ea66a69da4bab01326d459d379ce31b0ecd438f16b21d24e3fe3d50 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndexPtr.op_Implicit(Inno.Native.ImGui.ImGuiTextIndexPtr)~Inno.Native.ImGui.ImGuiTextIndex* ef443f8e8788ecf03276a053c8bbf7f9b10cd87dd51b7fb4a30a9b7e7dbded84 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndexPtr.op_Equality(Inno.Native.ImGui.ImGuiTextIndexPtr,Inno.Native.ImGui.ImGuiTextIndexPtr) c2246984e70813c3e3c05c3d68590b6af9a22baf836978cf7021e0662e032862 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndexPtr.op_Inequality(Inno.Native.ImGui.ImGuiTextIndexPtr,Inno.Native.ImGui.ImGuiTextIndexPtr) 753d2bcb0e4e56970eeab0d20f81272d11a9f6644c2b8096375d917a03f964ad -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndexPtr.Equals(Inno.Native.ImGui.ImGuiTextIndexPtr) 0c20c1ba3113fbe05022fd5ff97c081fd0092edf7f1d0defaa21a475b444d198 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndexPtr.Equals(System.Object) ca2bf81086f330788e8737804302f31c2701e7918157a540939130e060040091 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndexPtr.GetHashCode d917d42cf331442024b06d9a2f6b39c01b07b0fd6fcc108f4750fb59eb639683 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextIndexPtr.Offsets 8c27b5ffba85ff552c38e3eeb6b40debeea08d2e9cd0b495b733263d45710cd9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextIndexPtr.EndOffset d39ce2658e95d2bb0b54622d828e7b140e312617ec7d109c33ad1290034dc4c8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndexPtr.clear 151dc875904d3f9c4bc5e9d8399aee6b775bf80f9a911cacea52af13f33bfe53 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndexPtr.size 298071a35f5fc0928100e08547ef468c5140111c4130124e08e2774856270cfd -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndexPtr.get_line_begin(System.String,System.Int32) 8ccbed9580ee638460f66cd6dea4972e47c65afc2e6bfdc41935658244f08d57 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndexPtr.get_line_end(System.String,System.Int32) 0775cd1040b75ab0e913718c97482515e6cf34d5a26f3a3455a6e60d32f5288f -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextIndexPtr.append(System.String,System.Int32,System.Int32) 7bc21d0e163ba917669960a5c8c501420c285564049d4b1fe3c1bb580fa5d653 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-81db05f690d5375d"></a>[`public Inno.Native.ImGui.ImGuiTextIndex* Inno.Native.ImGui.ImGuiTextIndexPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17481) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-654ee2a218b89989"></a>[`public Inno.Native.ImGui.ImGuiTextIndexPtr.ImGuiTextIndexPtr(Inno.Native.ImGui.ImGuiTextIndex* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17476) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a15fc48e204df74e"></a>[`public void Inno.Native.ImGui.ImGuiTextIndexPtr.append(string baseValue, int oldSize, int newSize)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17533) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-584930142559417c"></a>[`public void Inno.Native.ImGui.ImGuiTextIndexPtr.clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17501) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b81226bbde9f36d9"></a>[`public bool Inno.Native.ImGui.ImGuiTextIndexPtr.Equals(Inno.Native.ImGui.ImGuiTextIndexPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17491) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-b4d5664ed684f2fd"></a>[`public override bool Inno.Native.ImGui.ImGuiTextIndexPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17492) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-9dd09bd7350d7716"></a>[`public string? Inno.Native.ImGui.ImGuiTextIndexPtr.get_line_begin(string baseValue, int n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17517) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a9c450aa850abe44"></a>[`public string? Inno.Native.ImGui.ImGuiTextIndexPtr.get_line_end(string baseValue, int n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17525) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c43931d1f5bd2a01"></a>[`public override int Inno.Native.ImGui.ImGuiTextIndexPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17493) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-9edd17a64fdbb25a"></a>[`public static bool Inno.Native.ImGui.ImGuiTextIndexPtr.operator ==(Inno.Native.ImGui.ImGuiTextIndexPtr left, Inno.Native.ImGui.ImGuiTextIndexPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17489) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8f575258bb4a9135"></a>[`public static Inno.Native.ImGui.ImGuiTextIndexPtr.implicit operator Inno.Native.ImGui.ImGuiTextIndexPtr(Inno.Native.ImGui.ImGuiTextIndex* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17487) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a4cc34329dbd20e7"></a>[`public static Inno.Native.ImGui.ImGuiTextIndexPtr.implicit operator Inno.Native.ImGui.ImGuiTextIndex*(Inno.Native.ImGui.ImGuiTextIndexPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17488) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-08c91b10bbd41d02"></a>[`public static bool Inno.Native.ImGui.ImGuiTextIndexPtr.operator !=(Inno.Native.ImGui.ImGuiTextIndexPtr left, Inno.Native.ImGui.ImGuiTextIndexPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17490) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-00b77d45f64d2499"></a>[`public int Inno.Native.ImGui.ImGuiTextIndexPtr.size()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17509) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-556156aeb21be6a6"></a>[`public ref int Inno.Native.ImGui.ImGuiTextIndexPtr.EndOffset { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17496) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a145049e4f03bfb9"></a>[`public bool Inno.Native.ImGui.ImGuiTextIndexPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17482) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-423f703237e9b271"></a>[`public Inno.Native.ImGui.ImGuiTextIndex Inno.Native.ImGui.ImGuiTextIndexPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17485) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a563ea6673c15f2a"></a>[`public static Inno.Native.ImGui.ImGuiTextIndexPtr Inno.Native.ImGui.ImGuiTextIndexPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17483) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3f91d2e1da535b4d"></a>[`public ref Inno.Native.ImGui.ImVector<int> Inno.Native.ImGui.ImGuiTextIndexPtr.Offsets { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17495) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-95c17d0db211f4fb"></a>[`public struct Inno.Native.ImGui.ImGuiTextIndexPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTextIndexPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17474) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTextRange</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTextRange 30b743ccb0ab69abf6527a3889cfb2a560c79ac8c77ab983833fee4d26b143ca -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextRange.B 48368100f90614ac8e4ec7f33859770106746d1b97113c9276134373bf2cef91 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextRange.E aee6265382033cdfd5aa3afe66e17fda74a4a9dad55f755ac96ab6fc3fa36bbc -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRange.#ctor(System.Byte*,System.Byte*) cccc3447df8ca0ad61e0560e85c51b7371b3468f8c3255568e30f99f13212378 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRange.Destroy 8253fcfb29da0fda1b2f682be17c99e9da22a3e9cd2d05ffde92f28cccea012e -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRange.empty f7ebca1ddde3289ec8e1d9b299258ddd02461144607067b51fc2ef3ae9da5e36 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRange.split(System.Byte,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiTextRange}*) e0afb37c35f1be25c5202226d5efa8776ffc5bc2ede8e527a1b46dd036c1dac5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-081754acb7096079"></a>[`public byte* Inno.Native.ImGui.ImGuiTextRange.B`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9591) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2eee60232ed2806"></a>[`public byte* Inno.Native.ImGui.ImGuiTextRange.E`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9592) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-447601bf7525f64c"></a>[`public Inno.Native.ImGui.ImGuiTextRange.ImGuiTextRange(byte* b = null, byte* e = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9593) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-195ff6a3608fba89"></a>[`public void Inno.Native.ImGui.ImGuiTextRange.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63672) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d68bcaa532a50d4a"></a>[`public byte Inno.Native.ImGui.ImGuiTextRange.empty()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63683) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-39a2395b11d80857"></a>[`public void Inno.Native.ImGui.ImGuiTextRange.split(byte separator, Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTextRange>* output)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63694) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-10f5f5cfe42eeaa3"></a>[`public struct Inno.Native.ImGui.ImGuiTextRange`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9589) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTextRangePtr</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTextRangePtr e50c3c8b1bf4585c27c291298d9052956abd9f4d45f4982e703c485755427689 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRangePtr.#ctor(Inno.Native.ImGui.ImGuiTextRange*) e8c8ff92702fccd731dd5bceed1b6b300b3bfeaf77a38e5151ae55b17cae297f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTextRangePtr.Handle 4f3e4b6d8b3902bf068ddab076da58cd99d51700b409126ec3ddf659bd13bb83 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextRangePtr.IsNull 60bce4404c3c9909d1491cd2c4158733236b932da58c756b5a82fad4ff5736b5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextRangePtr.Null 3b645ec7af4c88a9d6db8760492b419c8b7dd682d04365537a4d99c1d100a987 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextRangePtr.Item(System.Int32) 7c0c0d7c5de81ecbb43abd8a051ca8efeaf0d8d9d4ff34e18a0f7e063914ae42 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRangePtr.op_Implicit(Inno.Native.ImGui.ImGuiTextRange*)~Inno.Native.ImGui.ImGuiTextRangePtr de4444c52a703bdc00a76debdea1b7c4f1b7b84d14709c8152f5fe2337da0478 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRangePtr.op_Implicit(Inno.Native.ImGui.ImGuiTextRangePtr)~Inno.Native.ImGui.ImGuiTextRange* 769026678b715683ddaf53b7bc89797e11d6439c143b952379a2002d47702cc2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRangePtr.op_Equality(Inno.Native.ImGui.ImGuiTextRangePtr,Inno.Native.ImGui.ImGuiTextRangePtr) 96ade1c623a0502ec12672243c416fa8edd43b1d1d4c1972531809fb0f543f48 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRangePtr.op_Inequality(Inno.Native.ImGui.ImGuiTextRangePtr,Inno.Native.ImGui.ImGuiTextRangePtr) fed6415730f054dd94c97c5e54b85ea9f359c6b55214b8ec9c58663890b863cd -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRangePtr.Equals(Inno.Native.ImGui.ImGuiTextRangePtr) 1beccae491627a995fc36f054a24209bd053574ced619cbbe9f3360b76b1f37d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRangePtr.Equals(System.Object) 650ffaa7ab62407132157e585763e3fae52e274beef5f6ad88a1d50cb45f4660 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRangePtr.GetHashCode 9f1d110db537a07eef7f7deb0cf302cf88b2163ef617be62d3952e5284efe1c9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextRangePtr.B 6c58c23f4187e8a8d0d258c3bea8d2e6e7e8b145b2c0a0083cf57188092c9e58 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTextRangePtr.E 8138a9a83f4b76aa4d80d5c5316a3d858fd113c8131a9d3569f1f519c1acc73a -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRangePtr.Destroy 818734419fed6ba89febd24e2a139078fff0c9c78f284629a35fc981740af659 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRangePtr.empty b3bd748a963689d6025d2fd267beb89cf868bcdcb786753eb66421f8e4f7a3e1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTextRangePtr.split(System.Byte,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiTextRange}@) a2aec79e384d6ea0ebf9de479ebccb07b8d1b8e3247b7c162e37a6e1978511cd -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4ae26a08a8c414da"></a>[`public Inno.Native.ImGui.ImGuiTextRange* Inno.Native.ImGui.ImGuiTextRangePtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17547) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9897808ce7592a6a"></a>[`public Inno.Native.ImGui.ImGuiTextRangePtr.ImGuiTextRangePtr(Inno.Native.ImGui.ImGuiTextRange* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17542) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2f617b0c07def661"></a>[`public void Inno.Native.ImGui.ImGuiTextRangePtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17567) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3a3745deb75a876c"></a>[`public bool Inno.Native.ImGui.ImGuiTextRangePtr.empty()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17575) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4937d25f9bd2e280"></a>[`public bool Inno.Native.ImGui.ImGuiTextRangePtr.Equals(Inno.Native.ImGui.ImGuiTextRangePtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17557) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-53bc0ed416812a70"></a>[`public override bool Inno.Native.ImGui.ImGuiTextRangePtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17558) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-224e3ea639ca4706"></a>[`public override int Inno.Native.ImGui.ImGuiTextRangePtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17559) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-a28eb5d7a0e3b41f"></a>[`public static bool Inno.Native.ImGui.ImGuiTextRangePtr.operator ==(Inno.Native.ImGui.ImGuiTextRangePtr left, Inno.Native.ImGui.ImGuiTextRangePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17555) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a746fe812ef800c3"></a>[`public static Inno.Native.ImGui.ImGuiTextRangePtr.implicit operator Inno.Native.ImGui.ImGuiTextRangePtr(Inno.Native.ImGui.ImGuiTextRange* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17553) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-682d9615b408c1e3"></a>[`public static Inno.Native.ImGui.ImGuiTextRangePtr.implicit operator Inno.Native.ImGui.ImGuiTextRange*(Inno.Native.ImGui.ImGuiTextRangePtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17554) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6f36f35828652b5d"></a>[`public static bool Inno.Native.ImGui.ImGuiTextRangePtr.operator !=(Inno.Native.ImGui.ImGuiTextRangePtr left, Inno.Native.ImGui.ImGuiTextRangePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17556) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-055f0e28d8f3bc0f"></a>[`public void Inno.Native.ImGui.ImGuiTextRangePtr.split(byte separator, ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTextRange> output)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17583) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0cea8be5a861c752"></a>[`public byte* Inno.Native.ImGui.ImGuiTextRangePtr.B { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17561) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-221c39ca04c91ca6"></a>[`public byte* Inno.Native.ImGui.ImGuiTextRangePtr.E { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17562) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7b135ed09e84b11c"></a>[`public bool Inno.Native.ImGui.ImGuiTextRangePtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17548) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1c9565b3604323e8"></a>[`public Inno.Native.ImGui.ImGuiTextRange Inno.Native.ImGui.ImGuiTextRangePtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17551) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a1fe9b93a255e98b"></a>[`public static Inno.Native.ImGui.ImGuiTextRangePtr Inno.Native.ImGui.ImGuiTextRangePtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17549) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4cb19ab556f07b69"></a>[`public struct Inno.Native.ImGui.ImGuiTextRangePtr : System.IEquatable<Inno.Native.ImGui.ImGuiTextRangePtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17540) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTooltipFlags</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTooltipFlags 93255cb47c59dae4d3337d25d808531f5d7e702cd9e3606ff84fae4f115770ee -->
<!-- api: F:Inno.Native.ImGui.ImGuiTooltipFlags.None 607c7c7e31a23708d81ec6efcb662e1ec93bb240eef3c39714bec6ec824fc5fd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTooltipFlags.OverridePrevious 3d2dcb842e5067e5a924020830db4c0ee5abd266f10f0044274d6b0e5051b735 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f3f787d41c7faf75"></a>[`Inno.Native.ImGui.ImGuiTooltipFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1378) | 当前常量或枚举值：`0`。 | — |
| <a id="api-aab728b0f73db94c"></a>[`Inno.Native.ImGui.ImGuiTooltipFlags.OverridePrevious = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1379) | 当前常量或枚举值：`2`。 | — |
| <a id="api-c142e4f839a74de7"></a>[`public enum Inno.Native.ImGui.ImGuiTooltipFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1376) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTreeNodeFlags</code> · 23 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTreeNodeFlags 3b38290fba64c59c1d70b4789023ea97611b4d6944c1514847279a3d8de230dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.None ccb4f06b960b4da95bf21b63107f7e4a8d95ec5ec592be096ac11d02dfdb16f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.Selected 07d887285ecee664390d78d5023b9b480ac2fa4a1e44633c482a9dd750ed49bb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.Framed 818cf9f19bf59718535b9a0bdd6def4938cdb2f664d0c844bf17fafd43099a5f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.AllowOverlap 5cca712641611e4dbde3f34b65bcc769c75b1cfa4012ac8333811fa028d02ee8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.NoTreePushOnOpen aaf384e3de636e93b45fcfdcd67d7d717198e8f29eac131a1f28c542d6701a40 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.NoAutoOpenOnLog 4afb7e187aab1529cc5291667a6b0380031151694ebd11b5a9ec573ed3b6a715 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.DefaultOpen 9cf0e0c3757b07fe3aa75bc25f3dbfcb9fd34bee6dcba313237e9ea992043570 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.OpenOnDoubleClick 8449f16496a630ec2d816499f75d9125fdeaa3782d1fb901f90a6d19099664ec -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.OpenOnArrow 2b508dff8303a14bd399a7ee5ca2d8a55e538965a344b34d024eaa9a5f2c9e98 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.Leaf 014b6bb8b9499fb9a99a29d7663b6f51668875a19903ba71bb9f9170567b3128 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.Bullet 894e6f889a5de292448d72fcfc14635ab025909d4953d97f315bd1c9dcef5bb6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.FramePadding 81a3f8ef580c6b63fc9584b53b1d2147f27b396bff27ef33d820c0d754bc028d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanAvailWidth 40a578ca28ebb19cd7e08e08323c412c7964b58c1b30886df087ace18512c450 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanFullWidth 3382adc0b96ae30b4bc286932459d6d5b3bc04f3fe4211c8b004fe682cadaec1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanLabelWidth 18daac04611a50e4eedfefac0490de7b2a37d01af1674b79164ef37af3b68eb5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanAllColumns 5bb145552533a629ad5dc67ad17a4129cbff35d5549f9a1fe2d83d0e8756126a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.LabelSpanAllColumns a8b53b64b25663189335cfade14b5966272bb4858d72014051b453e476a808dd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.NavLeftJumpsToParent 136be8472e80abc90a3fc122f3cb6070c4f28306c4ef36710f0711d4f12102c6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.CollapsingHeader ce00073df6e736ce3676d277d65579cfe193de1dd5c1387eeb64fab73eb92597 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.DrawLinesNone 23f1d0b975e3d7b4942d2c0728e76ad057f277b0ae6412ffe0eba6e42eadc261 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.DrawLinesFull 568304f531af95eddcea0d0c9a3d1d60fc24be7cc2a9cb842c940f0b87a3fd07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlags.DrawLinesToNodes 37bb907a5ae57e1e1d1bab5e1b115cb522da5e542215c0111ccaabf686cf371c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3a97a6ecb92ac5af"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.AllowOverlap = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L486) | 当前常量或枚举值：`4`。 | — |
| <a id="api-d7e29ae0df79f3a0"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.Bullet = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L493) | 当前常量或枚举值：`512`。 | — |
| <a id="api-b4e38e56463d9d99"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.CollapsingHeader = 26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L501) | 当前常量或枚举值：`26`。 | — |
| <a id="api-62a4456c38f5e2e3"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.DefaultOpen = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L489) | 当前常量或枚举值：`32`。 | — |
| <a id="api-e30ac06bcc779164"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.DrawLinesFull = 524288`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L503) | 当前常量或枚举值：`524288`。 | — |
| <a id="api-c994280b21aafab9"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.DrawLinesNone = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L502) | 当前常量或枚举值：`262144`。 | — |
| <a id="api-000bd7c70ed58c43"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.DrawLinesToNodes = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L504) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-66d28e9d10efc12c"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.Framed = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L485) | 当前常量或枚举值：`2`。 | — |
| <a id="api-11b3a3b4e70b7f01"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.FramePadding = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L494) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-7f478abb7103345e"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.LabelSpanAllColumns = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L499) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-74e8f224bd4f5d05"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.Leaf = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L492) | 当前常量或枚举值：`256`。 | — |
| <a id="api-8fd685f784075163"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.NavLeftJumpsToParent = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L500) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-79989ec16385b45d"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.NoAutoOpenOnLog = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L488) | 当前常量或枚举值：`16`。 | — |
| <a id="api-9f2af96b7ef7dab4"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L483) | 当前常量或枚举值：`0`。 | — |
| <a id="api-7d10dd2737a0bed8"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.NoTreePushOnOpen = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L487) | 当前常量或枚举值：`8`。 | — |
| <a id="api-50a3c083c98747ed"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.OpenOnArrow = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L491) | 当前常量或枚举值：`128`。 | — |
| <a id="api-50f0d517688f871c"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.OpenOnDoubleClick = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L490) | 当前常量或枚举值：`64`。 | — |
| <a id="api-6d2156bdc18b8e92"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.Selected = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L484) | 当前常量或枚举值：`1`。 | — |
| <a id="api-f95d5b9f02df749f"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanAllColumns = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L498) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-68763c1d7c077abe"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanAvailWidth = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L495) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-841cb3de82ad1097"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanFullWidth = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L496) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-6058cc1b4dfdea54"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlags.SpanLabelWidth = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L497) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-b0f7e5266fbae5d7"></a>[`public enum Inno.Native.ImGui.ImGuiTreeNodeFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L481) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate 28254c288a9f2f0e3f56dc83c26ede6546745434c6b0dd5c5cec4ec4b95c3cfe -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.NoNavFocus e9a17b9b43b7e5c7579dd48372895a65333f1888d35a659489ba62fccf1d54d8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.ClipLabelForTrailingButton abfc44796c23c0b14a6a817ed7a6a51966628304862206ad013117911977c31d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.UpsideDownArrow 06c2b93412e412a6a927b7cba8b35fce9f0f870efec98f0ed478db6e91dcea1b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.OpenOnMask e5bcfbe269839ff4735a56213cdf164b9f5cb616b01b44199bfa4f7694979fb1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.DrawLinesMask 337240cc5c727d01559c505ed0f2f2f91fd2e9e6805fe5890f9d1cf2f4e69ac1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0a6a939cbc7824b1"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.ClipLabelForTrailingButton = 268435456`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1337) | 当前常量或枚举值：`268435456`。 | — |
| <a id="api-9fd7d920d8127c69"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.DrawLinesMask = 1835008`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1340) | 当前常量或枚举值：`1835008`。 | — |
| <a id="api-f52f0d1096c078c5"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.NoNavFocus = 134217728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1336) | 当前常量或枚举值：`134217728`。 | — |
| <a id="api-648701d6809c3795"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.OpenOnMask = 192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1339) | 当前常量或枚举值：`192`。 | — |
| <a id="api-62a360fb26ec7f33"></a>[`Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate.UpsideDownArrow = 536870912`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1338) | 当前常量或枚举值：`536870912`。 | — |
| <a id="api-8198211016c7c2de"></a>[`public enum Inno.Native.ImGui.ImGuiTreeNodeFlagsPrivate : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1334) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTreeNodeStackData</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTreeNodeStackData a9a6bd0081f6e4ff4606fb8ee483d52ad09b36572fcbb088ba3b1cf03922149c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.ID 8e81f00390357b5a405b1587caa11469a171fe82e0b54a62333aaeb2dfc440bf -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.TreeFlags c749524c6bebe7025eea4ab66eb6bdc3af91b845468fb670dc6ba75d0137da75 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.ItemFlags b3b9a9e618682c80639f1aa4fad1317a9600b514273268c8680a4cb4aec5bdaa -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.NavRect f38b1f10df51f266b8fcabe104197ab3c7251f15122699a9b6348b2bb5c9f2e0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.DrawLinesX1 00d8115440c7b11402ab178fea65100daffc03658a12c3d4926d0cef38d82c23 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.DrawLinesToNodesY2 4880fe11a1dd00b98aac46c07295d761ac042cac69dea18439c174d8213158ea -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackData.DrawLinesTableColumn a4935ee94c17801abc338900da747d7b58f94949671bb7c2e51584215dc8cf3b -->
<!-- api: M:Inno.Native.ImGui.ImGuiTreeNodeStackData.#ctor(System.UInt32,Inno.Native.ImGui.ImGuiTreeNodeFlags,Inno.Native.ImGui.ImGuiItemFlags,Inno.Native.ImGui.ImRect,System.Single,System.Single,System.Int16) c5c422ca3fb02869ee90d5fe7f2136cfc0e40c4b06d90d0b7261142976c37659 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-834b288b2a376cab"></a>[`public short Inno.Native.ImGui.ImGuiTreeNodeStackData.DrawLinesTableColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5828) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-837db83193266e71"></a>[`public float Inno.Native.ImGui.ImGuiTreeNodeStackData.DrawLinesToNodesY2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5827) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7fb2f7442c84c297"></a>[`public float Inno.Native.ImGui.ImGuiTreeNodeStackData.DrawLinesX1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5826) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d366ace816252860"></a>[`public uint Inno.Native.ImGui.ImGuiTreeNodeStackData.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5822) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7cb32bae92244eeb"></a>[`public Inno.Native.ImGui.ImGuiItemFlags Inno.Native.ImGui.ImGuiTreeNodeStackData.ItemFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5824) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a01e8a23d5c06881"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTreeNodeStackData.NavRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5825) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-922defd594b230b1"></a>[`public Inno.Native.ImGui.ImGuiTreeNodeFlags Inno.Native.ImGui.ImGuiTreeNodeStackData.TreeFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5823) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c32d5595ce2348c8"></a>[`public Inno.Native.ImGui.ImGuiTreeNodeStackData.ImGuiTreeNodeStackData(uint iD = 0, Inno.Native.ImGui.ImGuiTreeNodeFlags treeFlags = Inno.Native.ImGui.ImGuiTreeNodeFlags.None, Inno.Native.ImGui.ImGuiItemFlags itemFlags = Inno.Native.ImGui.ImGuiItemFlags.None, Inno.Native.ImGui.ImRect navRect = default(Inno.Native.ImGui.ImRect), float drawLinesX1 = 0, float drawLinesToNodesY2 = 0, short drawLinesTableColumn = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5829) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-104e426231559fb2"></a>[`public struct Inno.Native.ImGui.ImGuiTreeNodeStackData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5820) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr 3039f12ff04ae22a25f24a9d1bfba7d947e559f6b143ae2f26e61e24034a788f -->
<!-- api: M:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.#ctor(Inno.Native.ImGui.ImGuiTreeNodeStackData*) 2f95497ca302481d448dd006ce8e14d44ac5e512c3d39f8efa6f043a48de74ab -->
<!-- api: F:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.Handle e305256b5a4de22c6a98f16dd46a0b047fc1d1dec01244064af155ca1c1bc9d0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.IsNull d5f9bc7612190a6cf7742161c4c023f34f03cee15c90f4101c23aacdd8ec85db -->
<!-- api: P:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.Null 4b241cb467de46ced0514161d98eaba691430727285fa2db75a4b56c945db31f -->
<!-- api: P:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.Item(System.Int32) 1d617322f7e5771572bb846573269053b5a13c7fe68eccd75fd4b189c84ba976 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiTreeNodeStackData*)~Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr 322b214c02d3869baedfc486b4914bd05325df7f7ec57ffa201253625acae7e9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr)~Inno.Native.ImGui.ImGuiTreeNodeStackData* 82636e1654eb4b086a2a52712d2aa91d4e6eac36b3713235666e9fa4a8a05b02 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.op_Equality(Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr,Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr) 09dda478b03fb00d7b0fe671be24f7b542a33b43fce24114a1346000b2d0ad1c -->
<!-- api: M:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.op_Inequality(Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr,Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr) 1e8a41e27401288fa86320d482ea56c42c2acb9e69a83c24e70c381e631831dc -->
<!-- api: M:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.Equals(Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr) 2b82edeeff1cd9298d279eae5fde8aaedbccce9cd6d1a9f6e04ea1821906f65f -->
<!-- api: M:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.Equals(System.Object) bf6f5334e67f69aa54848f58ec727784cebcc27ef11cc0bf1e31a87752b81ace -->
<!-- api: M:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.GetHashCode 8ee426c9c7d0cd04bb580e14c0ebe8aea144bcd7aff9a1638c4d3e54c215df98 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.ID 07ecd80ef0141b884c5204510c55f7044b9c583415a101acbadeb082c9d756cd -->
<!-- api: P:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.TreeFlags d52f65feceaa8fc0ff9a2af55b3f55ec73ee1926fd74c0d8b305abd3dc7eec72 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.ItemFlags 0986f621715790e44e6440d9bc63dfe13ceba682ea01309c5ef301517cb2cc65 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.NavRect f33535703d605073ba95ccf78769dd52394e0b95966ce3a0859b2a10a65991a4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.DrawLinesX1 cf2d78d0287204eff4130023e47a881d58e6204ad71bdf033114fa1473389368 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.DrawLinesToNodesY2 785a87f79d534cd71158af8c9f5c770e4b6fe6b028c8146933b7aa9a94672115 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.DrawLinesTableColumn 6f0b95db15a15f97306478571eabe38a0baa90aa811eed19de814a0b00b4701b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-225214090e97beae"></a>[`public Inno.Native.ImGui.ImGuiTreeNodeStackData* Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17597) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8470a7df5f44d534"></a>[`public Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.ImGuiTreeNodeStackDataPtr(Inno.Native.ImGui.ImGuiTreeNodeStackData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17592) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d7e344e417723e1b"></a>[`public bool Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.Equals(Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17607) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-6da540caa15c4454"></a>[`public override bool Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17608) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-b362258d570cfccb"></a>[`public override int Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17609) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-6ec44c179561107e"></a>[`public static bool Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.operator ==(Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr left, Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17605) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6c78fae50cd02b0a"></a>[`public static Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.implicit operator Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr(Inno.Native.ImGui.ImGuiTreeNodeStackData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17603) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ee2edd5e56cc5012"></a>[`public static Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.implicit operator Inno.Native.ImGui.ImGuiTreeNodeStackData*(Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17604) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-53dcca03771abc2e"></a>[`public static bool Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.operator !=(Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr left, Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17606) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e46285d4951773d1"></a>[`public ref short Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.DrawLinesTableColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17617) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f22b1bf124fa9e7a"></a>[`public ref float Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.DrawLinesToNodesY2 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17616) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-956d541b2f4d27c9"></a>[`public ref float Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.DrawLinesX1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17615) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-875e137a56bc2aa7"></a>[`public ref uint Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17611) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6864eca69c432050"></a>[`public bool Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17598) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c688110398f41cf4"></a>[`public Inno.Native.ImGui.ImGuiTreeNodeStackData Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17601) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2dc94685f51a1d90"></a>[`public ref Inno.Native.ImGui.ImGuiItemFlags Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.ItemFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17613) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fffb848d269e484a"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.NavRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17614) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1103511718c0f325"></a>[`public static Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17599) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7aa6e4a0ebd9f9d8"></a>[`public ref Inno.Native.ImGui.ImGuiTreeNodeFlags Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr.TreeFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17612) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cee620efe2a62166"></a>[`public struct Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTreeNodeStackDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17590) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTypingSelectFlags</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTypingSelectFlags 2cc740ad3667235431050e0436acfedcf431d0d18fd67d17f61b7afb092dce9a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectFlags.None 5eed297f5340753a2b5f7eb259c214ef3d9b33f1f82ecea8421f5218749a1f6c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectFlags.AllowBackspace c139073fd39370a39b7448dbb71a5326a94d6f7e89df5747ae7563da4cf2de56 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectFlags.AllowSingleCharMode b989ce02ffbaaca6648dca4e0d62ed651d4c00111b670ee1bf241191326a2456 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1b5a6d2aeba0f507"></a>[`Inno.Native.ImGui.ImGuiTypingSelectFlags.AllowBackspace = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1583) | 当前常量或枚举值：`1`。 | — |
| <a id="api-1111da51f1f631f0"></a>[`Inno.Native.ImGui.ImGuiTypingSelectFlags.AllowSingleCharMode = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1584) | 当前常量或枚举值：`2`。 | — |
| <a id="api-5d4ee99a1bd8472a"></a>[`Inno.Native.ImGui.ImGuiTypingSelectFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1582) | 当前常量或枚举值：`0`。 | — |
| <a id="api-1b97dcce1175e32f"></a>[`public enum Inno.Native.ImGui.ImGuiTypingSelectFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1580) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTypingSelectRequest</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTypingSelectRequest 2d93ca54722ad8c3283b44ab822e09a08d573085984b9e8e2317e6af76367275 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectRequest.Flags 91aecea448944c22710991314c78868aa5035121d4b3abb560d0fcd1a895d167 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectRequest.SearchBufferLen cf5b1a881009744ab35d6bf168f93170874398d9873c2cddc7587b1721285322 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectRequest.SearchBuffer 0d2d1c370486a7102f11df29000c2591e0fc58c260b17e89d7b5b36bb54f3bed -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectRequest.SelectRequest 1f8adf2d7817e0587de0e3ee3f27bd09c56108ada9a6f54d5c5d27fff55bed26 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectRequest.SingleCharMode 203be941a256b16d623ea295d04d4b2476f98718688f9221f06e2697b6273d89 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectRequest.SingleCharSize 452174c4dba8c4cabab9912b4fd0fd6085313432a0e081d48722c5e210b35aa7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectRequest.#ctor(Inno.Native.ImGui.ImGuiTypingSelectFlags,System.Int32,System.Byte*,System.Byte,System.Byte,System.Byte) 6f528b9486cd2324f1ba91e5a95842b4284d2c24643c5e16e561cbcab7863134 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-609b285aadb2fce0"></a>[`public Inno.Native.ImGui.ImGuiTypingSelectFlags Inno.Native.ImGui.ImGuiTypingSelectRequest.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7121) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ac7a37d4c408a659"></a>[`public byte* Inno.Native.ImGui.ImGuiTypingSelectRequest.SearchBuffer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7123) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-601926cd3136ce8d"></a>[`public int Inno.Native.ImGui.ImGuiTypingSelectRequest.SearchBufferLen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7122) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5214dfcc4dce6480"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectRequest.SelectRequest`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7124) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f22275f7b09a7929"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectRequest.SingleCharMode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7125) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4cc046953890b0f8"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectRequest.SingleCharSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7126) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1f730eb076c21fc"></a>[`public Inno.Native.ImGui.ImGuiTypingSelectRequest.ImGuiTypingSelectRequest(Inno.Native.ImGui.ImGuiTypingSelectFlags flags = Inno.Native.ImGui.ImGuiTypingSelectFlags.None, int searchBufferLen = 0, byte* searchBuffer = null, byte selectRequest = 0, byte singleCharMode = 0, byte singleCharSize = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7127) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b5c6b8f3e8b3fb0c"></a>[`public struct Inno.Native.ImGui.ImGuiTypingSelectRequest`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7119) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTypingSelectRequestPtr</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr 2a56c37f562497f809778cbd39c36555d59c7d673d67ecde30d17da505522cec -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.#ctor(Inno.Native.ImGui.ImGuiTypingSelectRequest*) b784b36ddb48dd060862f9597cd70ad2b51ad2617afd86bef084a3f66e6ed001 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.Handle 5fe23dc88730b7d372235a5949c9c8692016e9de4f2451b2f3afb6c08603c744 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.IsNull e09a67c875be291bc1f10fcdd2eb3eae89906f64a265cb71df5a3b58fc84d993 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.Null 443858e04b197d20d222f28c410e8b797952e3426e379b5cb41411add4029652 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.Item(System.Int32) 7476310936db2196d962c231d147bef5911e40bb2d0580617a6e8aad17babbb9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.op_Implicit(Inno.Native.ImGui.ImGuiTypingSelectRequest*)~Inno.Native.ImGui.ImGuiTypingSelectRequestPtr a7b7c239ad1faf629a6597f3607d230ff5961efe778f2577508b7159e8b1da14 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.op_Implicit(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr)~Inno.Native.ImGui.ImGuiTypingSelectRequest* b0962640cfdac50ceb06625cc0c89cf1c088af131c96c026f8958f49735a5484 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.op_Equality(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr,Inno.Native.ImGui.ImGuiTypingSelectRequestPtr) 75ea5546ccf2604beb2426599c59b1900ecb24b5d06f2762a43f5fa0179bccba -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.op_Inequality(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr,Inno.Native.ImGui.ImGuiTypingSelectRequestPtr) 2439abc4079a205b37390ef216077ef3e0f6f52d2641f86c4627ee24b5637abf -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.Equals(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr) d985afcf6f1269c3dcea4c2221ce230512ad37c2e44179094e7a861b696fcb1d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.Equals(System.Object) b31c036d1064ddc8e70242cc07531ee1d8f2c832078104958bcd5985a9af7743 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.GetHashCode 6e6f939a0adabf3bc779694d49fa919dd3a79ace2c797bdf34088a1c7dc03122 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.Flags c3938654dd96750b84ffaca29343087ee625cbdb6d333c4a116da03cd0fa85d8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.SearchBufferLen 74efad583d0b041295dfa40ff6c2a0ef6ad8fdadbf14de0fd3cfe1ade7884a7b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.SearchBuffer f632fcc1dd770841c0902f39804aa12475ccc11652f83b3c2a09bfbb84d9594c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.SelectRequest e3e74aab66d397408e6fcdfc3e411c03f75792d5a32a73681c09c28f274fee41 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.SingleCharMode d0730a082a2e67f8195f807a3724fbfce452ccc3e8405cc8a2b91cba5b347467 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.SingleCharSize d2bf03f805503147520769fa013ba37c23f8a8f799ff1b8ec4bb607697a2d00a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5718b0d748192f83"></a>[`public Inno.Native.ImGui.ImGuiTypingSelectRequest* Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17628) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-73b55cd5c72871bc"></a>[`public Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.ImGuiTypingSelectRequestPtr(Inno.Native.ImGui.ImGuiTypingSelectRequest* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17623) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-001d33f55ce37536"></a>[`public bool Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.Equals(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17638) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-3c7eb6564451bc36"></a>[`public override bool Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17639) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-e15fa3a8138c00db"></a>[`public override int Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17640) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-23138724966c59fe"></a>[`public static bool Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.operator ==(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr left, Inno.Native.ImGui.ImGuiTypingSelectRequestPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17636) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-42b509a2d7ad2d05"></a>[`public static Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.implicit operator Inno.Native.ImGui.ImGuiTypingSelectRequestPtr(Inno.Native.ImGui.ImGuiTypingSelectRequest* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17634) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-da8ab19fe0ac8b78"></a>[`public static Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.implicit operator Inno.Native.ImGui.ImGuiTypingSelectRequest*(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17635) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6bf3a7ac942f459b"></a>[`public static bool Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.operator !=(Inno.Native.ImGui.ImGuiTypingSelectRequestPtr left, Inno.Native.ImGui.ImGuiTypingSelectRequestPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17637) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5127daca0dfc46d4"></a>[`public ref Inno.Native.ImGui.ImGuiTypingSelectFlags Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17642) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-14a76720b99b1b92"></a>[`public bool Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17629) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e3d80272b60be7f2"></a>[`public Inno.Native.ImGui.ImGuiTypingSelectRequest Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17632) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e358404baa011b91"></a>[`public static Inno.Native.ImGui.ImGuiTypingSelectRequestPtr Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17630) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5c86227bb42bdb35"></a>[`public byte* Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.SearchBuffer { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17644) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-790771d7b8944b37"></a>[`public ref int Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.SearchBufferLen { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17643) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3dcd1d3aa086ee49"></a>[`public bool Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.SelectRequest { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17645) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-51155cb3f5935318"></a>[`public bool Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.SingleCharMode { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17646) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7d367abcb1b88b0c"></a>[`public ref byte Inno.Native.ImGui.ImGuiTypingSelectRequestPtr.SingleCharSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17647) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d97e3c8665a9d5ed"></a>[`public struct Inno.Native.ImGui.ImGuiTypingSelectRequestPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTypingSelectRequestPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17621) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTypingSelectState</code> · 74 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTypingSelectState c81597a38ad4b4331e1a323d7c93db807be71ab26fc7855deb0296d0fc4567b5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.Request 7478a76bb393bfe159d4ff6cacd39d3da1c5643ee717d05033f1a94df4533a4d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_0 d7372c1252dea33d8cf4b73dac5b6fe675f17cd34e0a75f821f441cea4e23041 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_1 0007e1350ea60f2ec481dca9f29f93bb88f3f8b32f188b8911d27a49d5120e9e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_2 c7825ad70ca544b677050f5ba1bba22f4278a27e632f3f632c58b77d4591981d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_3 78fd68ff4d7f7742b4f7fccc9b42089249bc37c3ecfd1a56f12b7dae7381347e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_4 76191be70212229bf3ce80792feb5f7f248e2ca21d522205b7db6ca886ad81f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_5 3eb6171fd6af6a57ccff042649b7e3e86999cf872f50789dc9f9472e095e5a6a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_6 5dadc59647ba9a025d2a77378553ef5e9ab60668e22c9b7458651f296fde0e06 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_7 97738b646de53e8586296b8992fa0c8d4747ae3683a33cf7793505408d05bdba -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_8 e05f306cb0eb83d2b4ea368182709f0d4fc74c1bc4a319b1b4427d0da32c0dd9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_9 9d5cc950d2a09d7ec4ac1eaa033dde92fff8d9d5675d16d8146e4ba85f730d35 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_10 e46e3ba621af8704d79daec112ff149071524c482c69874ae724425bdddea826 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_11 87c85a73554737c9804dbd07ab2695724011668f8e43a8eed945500dba107bef -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_12 c9ad338477db6ae9b67c2bc532d61362a4a398107f43c1d419029ac3b6a6fd33 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_13 83c48ce338e10a3b53dc8b7f0d31407528e1e3f3b5a6da965facd45ad2f098bf -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_14 b1778cfc5b37e51817efc5af26ec8e9b5b7dfc2d54024acc75a7c8695a4de069 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_15 4d48490bff8602ee982e6085f6e4735f02d408050bd17c4443c0c3b5959a433a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_16 dd70d7a063c4d12d0ef8767ea43af297f4616dfddbc8e9fc71e5069b8069ee53 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_17 032236dd38ea3373ef4f1f8c5fbed4170b0d94c1ffe51a6a99111069ca691073 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_18 6ee4ddbc9c8bb4a3edf46c182cce87890eb95199b733a52a94bf90ae96cf48a7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_19 8154c8c3ce37b782993d03b5872f79c451fec4f5b7854561d37b319d6b4137d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_20 af4e813a163508803c9b301c928839c7a6b9be93faab6020a2ddf08f899c1eb1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_21 65626b4ead51d937ffe3ee9338f0e04a3252c11dab6c77a90899d0f9efb879f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_22 4aaf3ce5a607745b49db7112d8e2a25dbe1abe0bbf72965d48de46e9785a56a2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_23 c2952b416d7be8421538bde96ab8e69d9c18050577e3ea71b4c6c2f00771b44c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_24 747055b00871191a4bf5738adee653521b5296696a1733aa8ddbb9c2ccf259fa -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_25 bd910d6bfcbab40f15cd62690ef62765821b8a7ad03089852550e766fab5c6f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_26 7b33a38910e44909ed43dbe75c8908760509b8531e4b303df42356c70c4c8f1d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_27 dd926a85f65682c6292344fdb1d3fa6e8d5a492db74e7e701423a83f7517f09e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_28 ee01fb145a3e99bb040d94f8811a02bbdfe96d877bd3e8cfff19e231a0cf757d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_29 1d546b3bcb3eabcdfdece777533152eb66bfcf9760fa06c819327ba9663eca12 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_30 3bee1bf3c0282799df30fd7ac745fe8f617380149d50ecd93742a78012b0955c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_31 a8994a1fc828d3e8e58d9e16d7a2ff3c482373fb3bb3ebf3ced627cb360e43b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_32 bd8911eee7dc7794e4c8aa546bd718ae62fe34c8e9e321fd2776ea064b74874b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_33 00b6d4ce76aa682c38c74268669fa2e87b3a94fb16900d3e1c1e63e74ab01b26 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_34 aaa9839c73817dbc34bf207cc881d1db996e5dfbad4d576bb22f3edc01434fe7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_35 6df5e8a7d8d31ecdf1fb2737e33194a63c0c2f75bbc9aa5a61fd9ae185ac00bd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_36 1afa2e5ed09da54763fbf6778dde0f9d535ab4fa532e63a0af38cd17a3f1c095 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_37 1bcbb70fa3ac211ecc86400136adff2aa7fcab73120f3ec3e48c68ccd8324831 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_38 36136da33474e1bc38fb287c6770f573d5461fdeac4c32bc2be42216090e535a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_39 2c9cfbaed5f7e6dafbf540755a779da6c1ef179386fe8510cbcc032dcd0f2bb3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_40 efbc2a2462c6ec0d7de5627734e60f56be66f869912f257710ee06dad9fb5f64 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_41 b4b520d4da658a172c720d3aa0d2370faa1f2dddb2948cd0afb377193653eaa0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_42 f70b5c57b04cb9fda70b030c2709a927c3caaa9e7e639026f1c14fe5bc2c93a5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_43 bf0bc802f3ac9dcd41399139e27c3366f3dcd1db51c277a4aaf4e060d17314de -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_44 21499cca4574cea9d34f98a4a81c5ae8319da207c96b0271cbbbf4d4d9097a07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_45 409c3cf62152384a82d2887596aaf7dadac77b1d0dbf9023ed6170d85c5c4655 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_46 1e6b34d767e5cf32853cf57fce4be46b17f4594735507d9d86624877ee0a6cf1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_47 130a154a1c0901b702702bf1bd56084c69301c31810ae3599802c1dc2d071623 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_48 78f57174835b584d4be9dc03ae8aba22acd2b3b2b3f5bccc1a6c0f79407d0708 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_49 6e23e0806ad5eb75836513edddfa6a4ea808655c4d97abffb21d6561fc263830 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_50 1bf2b6c7bed455ce5c1cabb18ceede4e0b7640f1b490b8185b6cea40ac51e246 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_51 bf33ba2993eb175ae427750e4ce2b6bced78647b1265121035518c3250642a81 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_52 fbdf6ed2e3522c596ba300554097c233d0510ab50c8d7b8c0e85d100cab69ef9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_53 319d7a9ed285c2c62febb3bc9b5cf480ad63e74ed3c291be0c1da07afff15e39 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_54 11952386b4e55e19bc447cf13811778c32b376d2eb6564b891735425514997d1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_55 bcc2f3dab26e8d2a77ab3c178309c038529356df82a63800a82015fe0fcdb0b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_56 0ff01506350d7040386029a671414bfce0626d8e7460326a22483a408d3f5b38 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_57 c21ed327b8ef9f9f447c4aa28f63c05403b80b8c6990d5253d606d530d41dca8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_58 774573f433a72a4e7a27fefda19bbdc14817b5c2a130d7dd023eb2787a9afb4b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_59 9df435bd7144ec888bf95657d14a62325d63568b8e3d1f614536ebd4fca64667 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_60 0c1db231419eff5ce1e3e09fbcdcdb8738483807572a2c118271054488847bd4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_61 dc326583cf837b979bbcbf923149af71898c411a6d120424960ffcd87b2432a9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_62 dcd733ad58f179c592353e8dfeac2cf8714429866e88c88d0a803cd9692c0e2c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_63 9729bd804f4e4c0f25d75d49461e2f1c061adcc9e6081ea7f80dcabd7600fd0f -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer d4ccc21bf3975a05c89402e6e28deed3c71db3af67993a20c02857f80370e5fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.FocusScope a25f82b252a16dcec810ca61cb7d17e9f8b36eebb914ac7272a03f47c64505db -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.LastRequestFrame 6fd797d0d2a1957def86331be0ad530bfbe98a54ffa12dc3bbb2db4a7648b1b3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.LastRequestTime da5cb0c547a5b140b2cb8527ab93c0ab4f9c66427b95d4ed36499c7d2d01c4bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectState.SingleCharModeLock 0d45ebea8707ed02c05798d5584203d2aa8519cc969f820a7df1b7d73570241c -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectState.#ctor(Inno.Native.ImGui.ImGuiTypingSelectRequest,System.Byte*,System.UInt32,System.Int32,System.Single,System.Byte) 9cbb36f1e39bbae4ae26e9ec183fb78647d3f02388ed0d1247909d59cd41f177 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectState.Destroy 33d9259429e01b1639b0b3bba2ca2ce1cabeea2138dd67aeb8b83796a948f3bf -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectState.Clear 8dfdaeddfe673393679f22c6bc6e3a7ea85ba86aeedeb91ca8fe94c3d4461d00 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c8bf0527a5fb3ea6"></a>[`public uint Inno.Native.ImGui.ImGuiTypingSelectState.FocusScope`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7211) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-412e92c258094694"></a>[`public int Inno.Native.ImGui.ImGuiTypingSelectState.LastRequestFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7212) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6ead8c8d96b46328"></a>[`public float Inno.Native.ImGui.ImGuiTypingSelectState.LastRequestTime`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7213) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a60a9640813ca2c5"></a>[`public Inno.Native.ImGui.ImGuiTypingSelectRequest Inno.Native.ImGui.ImGuiTypingSelectState.Request`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7144) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a2f4cbe152c780e9"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7145) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8d0d30207b3ac436"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7146) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ace94755b587bbfb"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_10`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7155) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c9b100ca8cf83119"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_11`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7156) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b1f2c1ec02d8bbbe"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_12`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7157) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-557b03b5adaca279"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_13`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7158) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-853ef4e7dd7b6d54"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7159) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-47b98e73b6e48311"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_15`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7160) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6210b3f526be5199"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7161) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a681c9d01bd66d50"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_17`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7162) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4d03aa8d51530126"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_18`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7163) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b802d82a7e951e99"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_19`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7164) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8dc730e11f0790ec"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7147) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7953a800ea966019"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_20`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7165) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c924a7d82ae8e8f"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_21`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7166) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f89812cb81c9f070"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_22`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7167) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-84eaa63371b07783"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_23`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7168) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7af4d5ee76c7d550"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7169) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58d1804ec565723f"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_25`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7170) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-abce699049da4637"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_26`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7171) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-04cf5911499f8448"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_27`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7172) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e0bd883a5275f93"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_28`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7173) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-81ba500bb3b0b07f"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_29`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7174) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d90d33ab52505c0"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7148) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-29d20d02b794bbb0"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_30`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7175) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-083be15a6cab07e4"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_31`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7176) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-32efe78b7355c249"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7177) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da13334461b5c76b"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_33`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7178) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-995eba23178c5e6b"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_34`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7179) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a2bef5114a8b1c3f"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_35`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7180) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc6f5667b00d76d0"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_36`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7181) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-12b116cd4fe4259d"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_37`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7182) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a3e70d94b3aa1b2"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_38`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7183) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f2b4746b5974462"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_39`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7184) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5b05fc6e2732d429"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7149) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-610de0d548ba792b"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_40`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7185) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aebd15dd819dd0a4"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_41`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7186) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5b6cb66f010227da"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_42`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7187) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35bf98c63800d159"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_43`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7188) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7946251c861f6e0"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_44`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7189) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fbeb6b471b513a16"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_45`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7190) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dada57c097ca5705"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_46`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7191) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-87b005e4d6a03710"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_47`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7192) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-48150e8630941885"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_48`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7193) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9450b273789bcb5a"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_49`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7194) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d50d68ef750ea206"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7150) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bd1f75e054085a3b"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_50`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7195) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4418b94fe9bb0add"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_51`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7196) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5ab0db6bfd1bd53c"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_52`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7197) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-791b924cc741e47a"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_53`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7198) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a08af172d0c1079d"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_54`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7199) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4d0d26a249c3ab8c"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_55`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7200) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b2e519679fa6003"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_56`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7201) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4ef21002a276be87"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_57`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7202) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-08c422226f89b9e2"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_58`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7203) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-581cf47bf259d574"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_59`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7204) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a4a80b4784a2f77e"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7151) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb7859d263763b92"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_60`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7205) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-71918d4221f5fe79"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_61`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7206) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c6719c54d9c86760"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_62`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7207) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-919eef8fc10b2312"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_63`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7208) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e19b339bdb17541b"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7152) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe932f42531b26bf"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7153) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f12eeb3b4c8287da"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer_9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7154) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce2dfe39cd0608cc"></a>[`public byte Inno.Native.ImGui.ImGuiTypingSelectState.SingleCharModeLock`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7214) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8a87d7e34b20db89"></a>[`public Inno.Native.ImGui.ImGuiTypingSelectState.ImGuiTypingSelectState(Inno.Native.ImGui.ImGuiTypingSelectRequest request = default(Inno.Native.ImGui.ImGuiTypingSelectRequest), byte* searchBuffer = null, uint focusScope = 0, int lastRequestFrame = 0, float lastRequestTime = 0, byte singleCharModeLock = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7215) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-df9b6251650e8b21"></a>[`public void Inno.Native.ImGui.ImGuiTypingSelectState.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66965) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7774637ff8d06167"></a>[`public void Inno.Native.ImGui.ImGuiTypingSelectState.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66954) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-99b4cab2183584a4"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiTypingSelectState.SearchBuffer { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7209) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ed2402e776128690"></a>[`public struct Inno.Native.ImGui.ImGuiTypingSelectState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7142) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTypingSelectStatePtr</code> · 21 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTypingSelectStatePtr c38a43eae9a8f4b6b767ff99f376c5d41f701cb56e52c9f178eca692a994836a -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.#ctor(Inno.Native.ImGui.ImGuiTypingSelectState*) 81e374277d836501ede1d82eebf99b14718ea604412f1c98d300389f30dceb10 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Handle f956f67163e3ddc1138727c915ecce07180f0e62bd888cc015b39a563614ea12 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.IsNull 54bd5c12417e6a4ba17d84a461b60c91db914f4766a9cf1ef798a3f2135aa69c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Null e123cc6c00263d0c1d5f1acefd731f6d4ee58a49d6d54867e9dfe3f3f9c71984 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Item(System.Int32) 5664f1c10d181787a18fbb21b6886376eea27f2dbc41663884b712bf2f88bdfb -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.op_Implicit(Inno.Native.ImGui.ImGuiTypingSelectState*)~Inno.Native.ImGui.ImGuiTypingSelectStatePtr 5642d70d21ae7c8b2bec96c56c22a7a82f7eda999c7482917745b991ea9376d1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.op_Implicit(Inno.Native.ImGui.ImGuiTypingSelectStatePtr)~Inno.Native.ImGui.ImGuiTypingSelectState* 0cd4339fd36bca8bd96aa4b79782022bac6749326bd48c82f35c8178cc8d3603 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.op_Equality(Inno.Native.ImGui.ImGuiTypingSelectStatePtr,Inno.Native.ImGui.ImGuiTypingSelectStatePtr) 9d1258b1ce630756af27130fd67b91ca4182250c22fa13bfaf057c99879720bc -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.op_Inequality(Inno.Native.ImGui.ImGuiTypingSelectStatePtr,Inno.Native.ImGui.ImGuiTypingSelectStatePtr) 32045243310eaca0351b98d0d9303e887c673bb700149390d1917a278dc58bd7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Equals(Inno.Native.ImGui.ImGuiTypingSelectStatePtr) 0758001113f64d48df6576e7989f636f96f422516fd2ffdf4317e4c163557d45 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Equals(System.Object) 8a72fb57d7b84103a7081607b8f446d07aad690d5f24254aefb222dad9112144 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.GetHashCode 70a000a98d57b4c48e352fbae25a95e0299555a31c5aa92631cb5af66ed4b8db -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Request 7a6fb88d9fc89fe4ee0d6d0260ccfcb4331a1eb6cc343217efc31f376cc36c9d -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.SearchBuffer 80d22ee6b78c2e732d9416fe6ca01a9ade270852badb6ca6ab5a86600a83957e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.FocusScope 059642fee3e801d61ca672818f4b00907043e43fa10701ee35bec80f9d65d607 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.LastRequestFrame 2cfed921f59aa3b479ec49eedee4a3a421e5cd082e3259b5160223fa7ab9b7df -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.LastRequestTime 80b69cea035cceb96abadb3a156810535b83260d1653a5e60a6ff6d36b801368 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.SingleCharModeLock 350393784939d47f3f603d90d339c712c91f2ad62d94cd332ad7f1701838e503 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Destroy 02f3263c10565cf5d79f869a6f73f16804cfa27f155de431f55b51ae51750123 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Clear 111d81ecdfcd91cfbe91c1b28b1d5875a5969d89a0b018f857549cfd9a1cab6d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9d4f18104e3a6516"></a>[`public Inno.Native.ImGui.ImGuiTypingSelectState* Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17658) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-30fcc7502cd30fad"></a>[`public Inno.Native.ImGui.ImGuiTypingSelectStatePtr.ImGuiTypingSelectStatePtr(Inno.Native.ImGui.ImGuiTypingSelectState* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17653) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ee0a6f705c241d15"></a>[`public void Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Clear()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17690) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a08b1be927eb0399"></a>[`public void Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17682) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-62285503cfc69546"></a>[`public bool Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Equals(Inno.Native.ImGui.ImGuiTypingSelectStatePtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17668) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-fc585552d854f44b"></a>[`public override bool Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17669) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-60cf9db25422abed"></a>[`public override int Inno.Native.ImGui.ImGuiTypingSelectStatePtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17670) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-ef36b2ce05a94b4d"></a>[`public static bool Inno.Native.ImGui.ImGuiTypingSelectStatePtr.operator ==(Inno.Native.ImGui.ImGuiTypingSelectStatePtr left, Inno.Native.ImGui.ImGuiTypingSelectStatePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17666) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2ce74daa6a3fe264"></a>[`public static Inno.Native.ImGui.ImGuiTypingSelectStatePtr.implicit operator Inno.Native.ImGui.ImGuiTypingSelectStatePtr(Inno.Native.ImGui.ImGuiTypingSelectState* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17664) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ba64ba5e360e48db"></a>[`public static Inno.Native.ImGui.ImGuiTypingSelectStatePtr.implicit operator Inno.Native.ImGui.ImGuiTypingSelectState*(Inno.Native.ImGui.ImGuiTypingSelectStatePtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17665) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d2a0fb0153255a42"></a>[`public static bool Inno.Native.ImGui.ImGuiTypingSelectStatePtr.operator !=(Inno.Native.ImGui.ImGuiTypingSelectStatePtr left, Inno.Native.ImGui.ImGuiTypingSelectStatePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17667) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-322dfec9242cdead"></a>[`public ref uint Inno.Native.ImGui.ImGuiTypingSelectStatePtr.FocusScope { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17674) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-89bfde9330d05627"></a>[`public bool Inno.Native.ImGui.ImGuiTypingSelectStatePtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17659) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-040f3067c52af299"></a>[`public Inno.Native.ImGui.ImGuiTypingSelectState Inno.Native.ImGui.ImGuiTypingSelectStatePtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17662) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f988562996113245"></a>[`public ref int Inno.Native.ImGui.ImGuiTypingSelectStatePtr.LastRequestFrame { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17675) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a3afe2e89e3f174f"></a>[`public ref float Inno.Native.ImGui.ImGuiTypingSelectStatePtr.LastRequestTime { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17676) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0e63d81968faeccd"></a>[`public static Inno.Native.ImGui.ImGuiTypingSelectStatePtr Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17660) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cd94eef620790baf"></a>[`public ref Inno.Native.ImGui.ImGuiTypingSelectRequest Inno.Native.ImGui.ImGuiTypingSelectStatePtr.Request { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17672) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e426af4f15273035"></a>[`public System.Span<byte> Inno.Native.ImGui.ImGuiTypingSelectStatePtr.SearchBuffer { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17673) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6e906fd193216975"></a>[`public bool Inno.Native.ImGui.ImGuiTypingSelectStatePtr.SingleCharModeLock { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17677) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fb21b2ca3523f13e"></a>[`public struct Inno.Native.ImGui.ImGuiTypingSelectStatePtr : System.IEquatable<Inno.Native.ImGui.ImGuiTypingSelectStatePtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17651) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiViewport</code> · 24 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiViewport a64cb3402a9adab3db72e9a08dabe564ca35c00357bc6a71bbf1d1c4ae7398ab -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.ID f3b2b2a43c33b311903ced9aec17518711459d9b10c363e71a5af35c4163f93c -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.Flags 31803a0e2578087d54f280347eefffa05a53a830273c04b21d8d0a2710f2427a -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.Pos 8e2613713433a5a432e39103299f216d3b4151b880437d90a7997ca23b832150 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.Size 3835eb9ce2d060e4910fdc3b63dc171414f2403de09687161cc1864eca5a7933 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.FramebufferScale 234fea0e42f0896330995a8512b1fed51ca10eb8d285e82568682d8338b977a9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.WorkPos 6e1127b666105d0750c670f44b00b76e99349ed97486e210a3d89e540bb3b63d -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.WorkSize 12741d6a60e30b673a488667201ec8c31d16c30dfb11f287c76efcbfb8a947f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.DpiScale 6e0ed563018cf4c827304544986bda59a07179673f3006916b1cb39187ac5fbd -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.ParentViewportId 8e725bd57e3f571454aaea1c0f5264b94035c67295a3d618c482b965ad04f396 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.ParentViewport b99198055742b827a65bfbc94eb9b6b013b3baf7cb4d547c79aeac896f51e54e -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.DrawData b8653d1569dfe1c3d5d21275c918988c435854007dcae808bf2cc02f542acf25 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.RendererUserData f6663043b0b3f9274bcc295bb20c54c2d21df1c227cc6325a1fa8bbe9cf67a24 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformUserData 49b39b58b7bab38759338493266204beca545a786720711855b36a7afccddbb3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformHandle 42b0dd83f867f5e1b76ecc092b633d87dedb8e3ceea5c0ec7585b8fab2fad4f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformHandleRaw fbda7a3a8ff9071b458a8eba021d45b6c04f2ae7d2a61a03914b1219c2d89ace -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformWindowCreated 0df67cb518ef290d7a290566667ba57f2a02dac3cc33273dd059e8ca00631e4c -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformRequestMove b3c2ee715445169c3a78b197575a82808ca0867c56019f8ecfc35827beeb000a -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformRequestResize 472f7ea0a296d72844206b670ec8e4a9fa2d2a37452d0439c27d147258b459dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewport.PlatformRequestClose a0e0c3ff20c930d5c3b10626a590177c42f396bf10a5ec2b2d3e832f4b18f45c -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewport.#ctor(System.UInt32,Inno.Native.ImGui.ImGuiViewportFlags,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Single,System.UInt32,Inno.Native.ImGui.ImGuiViewport*,Inno.Native.ImGui.ImDrawData*,System.Void*,System.Void*,System.Void*,System.Void*,System.Byte,System.Byte,System.Byte,System.Byte) dd6fed5d89033375bb340f2e8e1ba289f8cd78b620240c37b7424aac610a608e -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewport.Destroy c4f50f05063fee4968cb2eff38b5225999f590ff945d921b179da46d43d109c5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewport.GetCenter 78e0498b97a797d06a34f6f23583302fdbd3bf958ae2453fb1bff051934f40e9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewport.GetWorkCenter 2944b6251d64a6e19743ad692551b39b838328d649c77faf30777ded3553a343 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0d5c7ce8628b96e2"></a>[`public float Inno.Native.ImGui.ImGuiViewport.DpiScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3305) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-30572c957fd1bd06"></a>[`public Inno.Native.ImGui.ImDrawData* Inno.Native.ImGui.ImGuiViewport.DrawData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3308) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d9fd27e7bc986e84"></a>[`public Inno.Native.ImGui.ImGuiViewportFlags Inno.Native.ImGui.ImGuiViewport.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3299) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13d1a436cce51d13"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewport.FramebufferScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3302) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fc04399bd9111850"></a>[`public uint Inno.Native.ImGui.ImGuiViewport.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3298) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8cfd7dcdce7dbd08"></a>[`public Inno.Native.ImGui.ImGuiViewport* Inno.Native.ImGui.ImGuiViewport.ParentViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3307) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53527ecfbe3c4d63"></a>[`public uint Inno.Native.ImGui.ImGuiViewport.ParentViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3306) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-766bd6bbb2a4d996"></a>[`public void* Inno.Native.ImGui.ImGuiViewport.PlatformHandle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3311) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-819a03cddd8b1c1b"></a>[`public void* Inno.Native.ImGui.ImGuiViewport.PlatformHandleRaw`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3312) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a10ec2dd373c1d0"></a>[`public byte Inno.Native.ImGui.ImGuiViewport.PlatformRequestClose`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3316) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03072fbd0f40aebe"></a>[`public byte Inno.Native.ImGui.ImGuiViewport.PlatformRequestMove`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3314) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-458b6e3686a03c31"></a>[`public byte Inno.Native.ImGui.ImGuiViewport.PlatformRequestResize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3315) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-237df79cf8e32673"></a>[`public void* Inno.Native.ImGui.ImGuiViewport.PlatformUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3310) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-782e17cc91cfab54"></a>[`public byte Inno.Native.ImGui.ImGuiViewport.PlatformWindowCreated`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3313) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-59a1476c05b9da42"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewport.Pos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3300) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f33ea75fe018b2f0"></a>[`public void* Inno.Native.ImGui.ImGuiViewport.RendererUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3309) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-454bef48e9174fd5"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewport.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3301) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f4508b49d05e3e9"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewport.WorkPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3303) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ef4c635caef6e527"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewport.WorkSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3304) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0248d96a4b0cdf03"></a>[`public Inno.Native.ImGui.ImGuiViewport.ImGuiViewport(uint iD = 0, Inno.Native.ImGui.ImGuiViewportFlags flags = Inno.Native.ImGui.ImGuiViewportFlags.None, System.Numerics.Vector2 pos = default(System.Numerics.Vector2), System.Numerics.Vector2 size = default(System.Numerics.Vector2), System.Numerics.Vector2 framebufferScale = default(System.Numerics.Vector2), System.Numerics.Vector2 workPos = default(System.Numerics.Vector2), System.Numerics.Vector2 workSize = default(System.Numerics.Vector2), float dpiScale = 0, uint parentViewportId = 0, Inno.Native.ImGui.ImGuiViewport* parentViewport = null, Inno.Native.ImGui.ImDrawData* drawData = null, void* rendererUserData = null, void* platformUserData = null, void* platformHandle = null, void* platformHandleRaw = null, byte platformWindowCreated = 0, byte platformRequestMove = 0, byte platformRequestResize = 0, byte platformRequestClose = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3317) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-415ad28073ca935a"></a>[`public void Inno.Native.ImGui.ImGuiViewport.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65844) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5360cb99dba09ef6"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewport.GetCenter()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65855) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4d19dd4b160b84f5"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewport.GetWorkCenter()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65866) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-758462621a62beb1"></a>[`public struct Inno.Native.ImGui.ImGuiViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3296) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiViewportFlags</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiViewportFlags 95fdb100fdd20b4a36035c57126a49cd1dce6f85fae5cab8246dbc442c638b05 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.None 43c7b1dca5e52f29280540c180bc474efc6ded15d633c0100e019f93437f3aef -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.IsPlatformWindow 519f9f9d13779780f88abc457ae7030056bc1402bd85e21b00bba2ab35cf9a03 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.IsPlatformMonitor c5bd25006bdcff7a4b28f8e410ea51f5e4d5384d1f3f2798d4419b2b64f0f2ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.OwnedByApp 7beb3dc317766871fa7681ff5ea8cef5707080bb14ed2a277bcbd1418f99cb29 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoDecoration dc8d0c366d0fed102fe55b15b6be3858bd3f0c89254867a80731961b7d81f8e1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoTaskBarIcon 40b3a30fde8b36c1ca551c56ec09c17e03dc8e97c464ee37113757e1d61afced -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoFocusOnAppearing a8016c7718ec383442783541525d9a269c95911e3317cda21549bee1e11a9f81 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoFocusOnClick 687d4550dd496152dedc4f0e5487c14a700ca9fb2ba6cbe2da631bc3fc3694e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoInputs 423e2d50da0e5cb5ea57eb37e8dc25c99a6b62ae73baf38b3670b8d59001f963 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoRendererClear 4f70204cc719cecf01b3159c948b9ebef7de429f7e0f4280e27368444d7f0d80 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.NoAutoMerge b4b4818b2db35c77a079d785947b580ccb831654d38784f8316f6b347035466f -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.TopMost 13184c0b2705eb78094df0fd5f0492de6169b42d2315c13b1412b66745760091 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.CanHostOtherWindows c7628b97b9eb758293b7f507e52c407f4cce1fa051750615e2dabbe228224bcf -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.IsMinimized b65c8583b8cc9b569e39396b29f4b4f6098152976d043aacf26e70fb8f25f6d6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportFlags.IsFocused 4fb5af53319007bf2f563bd6dc13c4cab197309c7d50d996ddd249cfa3da5294 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-75676e92db69eeb1"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.CanHostOtherWindows = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1183) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-d7042caa28bb04c3"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.IsFocused = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1185) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-e91364d4eca17739"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.IsMinimized = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1184) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-5657e1896cfeccf1"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.IsPlatformMonitor = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1173) | 当前常量或枚举值：`2`。 | — |
| <a id="api-5d111ef1f9016f69"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.IsPlatformWindow = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1172) | 当前常量或枚举值：`1`。 | — |
| <a id="api-dbe2709e4ff3322d"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoAutoMerge = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1181) | 当前常量或枚举值：`512`。 | — |
| <a id="api-c80714a094cb2368"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoDecoration = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1175) | 当前常量或枚举值：`8`。 | — |
| <a id="api-979d655dbd45021d"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoFocusOnAppearing = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1177) | 当前常量或枚举值：`32`。 | — |
| <a id="api-17ffee14cb639cb5"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoFocusOnClick = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1178) | 当前常量或枚举值：`64`。 | — |
| <a id="api-2aabac1082cfa0b2"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoInputs = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1179) | 当前常量或枚举值：`128`。 | — |
| <a id="api-4f2cae143d94f35c"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1171) | 当前常量或枚举值：`0`。 | — |
| <a id="api-ec7811248978bf2b"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoRendererClear = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1180) | 当前常量或枚举值：`256`。 | — |
| <a id="api-5f64e5040cab6b52"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.NoTaskBarIcon = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1176) | 当前常量或枚举值：`16`。 | — |
| <a id="api-1c15c1c6dfd00954"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.OwnedByApp = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1174) | 当前常量或枚举值：`4`。 | — |
| <a id="api-a0bbf5d84162b6cc"></a>[`Inno.Native.ImGui.ImGuiViewportFlags.TopMost = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1182) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-1d9dc6bd8d41004a"></a>[`public enum Inno.Native.ImGui.ImGuiViewportFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1169) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiViewportP</code> · 37 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiViewportP 9d8c956c852323d9410f6fdb55ca8df56110f9d4f536973f8bbff82dd801f045 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.ImGuiViewport 3a6ffea8fad0c5438533f76224a75a37b07a48f0c8ae4b5b23f80d43b36cfa29 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.Window 2f61462f8253c0b09ca534be92aa74462d574e19289bbaa7918e037d9f1d2929 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.Idx 4801e158eb41a24438a4c63ca448bb005f935b557c2100d78e3dd900e2c53279 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastFrameActive e0b4fd21294cca6f67a6623e9d09c6212ac6dfb49d14dab13671999f72349ae9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastFocusedStampCount e71cb38f1ec2b51571c71d5ebafeae09ba4a7a7f4e63772daf3842e8cb64b046 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastNameHash 69f7ff2038b01b28a45072ace51cd8e8871705a0c9ae68e112de432d4bb86f6f -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastPos 0e62ffea13f5828555db0293540330d94beee8c73bfec088dca0e1c034d85c91 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastSize 4c8dee6444275500166b2c750be685928f5d05d5f3841ee3e2c51d93f46772e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.Alpha 4caadab41b4ceb60e19bcf7c3975b0e633e7ca580417ebec2c7887060408bab0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastAlpha d0f8a6b8037f650f54ffd77327d14be28acc372ecea78af090ddf469990fb12a -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastFocusedHadNavWindow 141df790b0667e6eaf178c82575e3821388d356c68b3737564ecf36980e1ce35 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.PlatformMonitor c0deeef766fe55df126328a21f0d3886abe44be82bd1014a18cb0da8908e5366 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.BgFgDrawListsLastFrame_0 3adcf4e8f2d153594e433dc5b87d81b5fcb827369abb129f830168b263ae25fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.BgFgDrawListsLastFrame_1 ca09927a1b840f1cce9872add42660a22da53a7dadc73ed84089d806861279a6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportP.BgFgDrawListsLastFrame 6e0219e7b79a398eef75a893fe701c6d612224c1cde6cd1b00262acd01d38eae -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.BgFgDrawLists_0 de52d9f094671d920cefe19965ba512279d3ae629a870f49b7f6fe52eba9aca9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.BgFgDrawLists_1 de4e2c81b0ec8a3d526bfc964071e0c2479c3ac7b8fd1d188c7c77e44c0f9218 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportP.BgFgDrawLists 921b3b89bb69bf765276f54c2e7762f69d7c7876c91ce9689df3105d02c08362 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.DrawDataP 4dd2d63e81c32afb2a8ad5a53b637fbfe254811c9ed01bd3e611036e88c800fd -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.DrawDataBuilder 9987b786ba97772e31d4168d4e34b8c28196b576c7ab369d81c6619931b32416 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastPlatformPos 5e11eaf4c8b03daef049cc49c7f33678c46f5b529231cfd8a2a326f2cd95e4ec -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastPlatformSize c0b1a8a29a1fd2a7fde6c904c303704f19d43e861192f60ebdc842d2d513024d -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.LastRendererSize 52754d227e7181819fff04c64cefb98e27190cff70d6da62b60a3e0a021c0983 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.WorkInsetMin 376bdf3b046a9395b9fa76ef7adb2c5241974101c40af18fd7030ea11d7c61d0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.WorkInsetMax ca34891347be165e7a80dfa459aa2d0e11bb629396084460b334686f0c3f2704 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.BuildWorkInsetMin dfcf84fdee3494c792c77dfbe22dcf4ae72774d3d065937e6ece0be44b3f6b55 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportP.BuildWorkInsetMax 867e418913f4caa387a985d1354418bc70c12c7ca1d250fe65a641a74634eb3d -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportP.#ctor(Inno.Native.ImGui.ImGuiViewport,Inno.Native.ImGui.ImGuiWindow*,System.Int32,System.Int32,System.Int32,System.UInt32,System.Numerics.Vector2,System.Numerics.Vector2,System.Single,System.Single,System.Byte,System.Int16,System.Int32*,Inno.Native.ImGui.ImDrawList**,Inno.Native.ImGui.ImDrawData,Inno.Native.ImGui.ImDrawDataBuilder,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2) 0c665af7e49fcdca051f0bd27985ecb21424369e94161b12b1624e09c20286d3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportP.Destroy 7dec9622d88c4891cbbc698cd2536ccbda55b627bf22899ee4f90c5e917b86c5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportP.ClearRequestFlags 83cd495571f9ce274149ed679a11480c325de95ba87d6c9be919a20c71cfa13d -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportP.CalcWorkRectPos(System.Numerics.Vector2) 7f1d89db0cee9d58088fca149ee623b88299d237c633eac48c8c3b7fcb913297 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportP.CalcWorkRectSize(System.Numerics.Vector2,System.Numerics.Vector2) 973b271ae96e29d7cd1b6d8427a7c4fe22f1bae845b73d1062e69482d5ac2c21 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportP.UpdateWorkRect 1a37c66ddffe1f20ba12420ecf1438c0e359ab0ea05ab740da40cd191f4dcf6b -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportP.GetMainRect 549dbf19e5267467ce2de7d9821fc8d8cf08465ba717113595ecfe45a06dcca7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportP.GetWorkRect 4c1cafa21e122b1f2c3abc4c2131dc09d104dcbe0ea1ff228d30e4b0698f3ad6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportP.GetBuildWorkRect b394e6988a9096d2b793a9d41e7a6640ff5bf17aa6e4121e35e61f933e5088e8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b0fbdb9863f65ba6"></a>[`public float Inno.Native.ImGui.ImGuiViewportP.Alpha`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3977) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b2fe5d82e4743863"></a>[`public BGCS.Runtime.Pointer<Inno.Native.ImGui.ImDrawList> Inno.Native.ImGui.ImGuiViewportP.BgFgDrawLists_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3985) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-09f77dca6b685e00"></a>[`public BGCS.Runtime.Pointer<Inno.Native.ImGui.ImDrawList> Inno.Native.ImGui.ImGuiViewportP.BgFgDrawLists_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3986) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e119782f7d5e28b2"></a>[`public int Inno.Native.ImGui.ImGuiViewportP.BgFgDrawListsLastFrame_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3981) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21c3d31cc68fe7b4"></a>[`public int Inno.Native.ImGui.ImGuiViewportP.BgFgDrawListsLastFrame_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3982) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07a313d41949adc6"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportP.BuildWorkInsetMax`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3997) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a81ae790d5088240"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportP.BuildWorkInsetMin`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3996) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd035b114d2ca616"></a>[`public Inno.Native.ImGui.ImDrawDataBuilder Inno.Native.ImGui.ImGuiViewportP.DrawDataBuilder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3990) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-70a641c318103463"></a>[`public Inno.Native.ImGui.ImDrawData Inno.Native.ImGui.ImGuiViewportP.DrawDataP`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3989) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e82498f053e0d3c1"></a>[`public int Inno.Native.ImGui.ImGuiViewportP.Idx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3971) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d950429b194008c"></a>[`public Inno.Native.ImGui.ImGuiViewport Inno.Native.ImGui.ImGuiViewportP.ImGuiViewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3969) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f18cbedf75a6ccb5"></a>[`public float Inno.Native.ImGui.ImGuiViewportP.LastAlpha`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3978) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b1c43eb66526ab0b"></a>[`public byte Inno.Native.ImGui.ImGuiViewportP.LastFocusedHadNavWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3979) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da0e73c47047e677"></a>[`public int Inno.Native.ImGui.ImGuiViewportP.LastFocusedStampCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3973) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e0989d46f1b37a08"></a>[`public int Inno.Native.ImGui.ImGuiViewportP.LastFrameActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3972) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01426bb0313be199"></a>[`public uint Inno.Native.ImGui.ImGuiViewportP.LastNameHash`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3974) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d5a01608ab94cabc"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportP.LastPlatformPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3991) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-46691feaa92973ee"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportP.LastPlatformSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3992) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d8177a9c75e1a6d0"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportP.LastPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3975) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd79a7181956946d"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportP.LastRendererSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3993) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c2e7d7d998ae985f"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportP.LastSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3976) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd8b0cad0163684e"></a>[`public short Inno.Native.ImGui.ImGuiViewportP.PlatformMonitor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3980) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aba34f852e003ad2"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiViewportP.Window`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3970) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c8221fe5bd4915bd"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportP.WorkInsetMax`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3995) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4b03a0e8f5bfd8bb"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportP.WorkInsetMin`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3994) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cbaf6adc47ef2ca1"></a>[`public Inno.Native.ImGui.ImGuiViewportP.ImGuiViewportP(Inno.Native.ImGui.ImGuiViewport imGuiViewport = default(Inno.Native.ImGui.ImGuiViewport), Inno.Native.ImGui.ImGuiWindow* window = null, int idx = 0, int lastFrameActive = 0, int lastFocusedStampCount = 0, uint lastNameHash = 0, System.Numerics.Vector2 lastPos = default(System.Numerics.Vector2), System.Numerics.Vector2 lastSize = default(System.Numerics.Vector2), float alpha = 0, float lastAlpha = 0, byte lastFocusedHadNavWindow = 0, short platformMonitor = 0, int* bgFgDrawListsLastFrame = null, Inno.Native.ImGui.ImDrawList** bgFgDrawLists = null, Inno.Native.ImGui.ImDrawData drawDataP = default(Inno.Native.ImGui.ImDrawData), Inno.Native.ImGui.ImDrawDataBuilder drawDataBuilder = default(Inno.Native.ImGui.ImDrawDataBuilder), System.Numerics.Vector2 lastPlatformPos = default(System.Numerics.Vector2), System.Numerics.Vector2 lastPlatformSize = default(System.Numerics.Vector2), System.Numerics.Vector2 lastRendererSize = default(System.Numerics.Vector2), System.Numerics.Vector2 workInsetMin = default(System.Numerics.Vector2), System.Numerics.Vector2 workInsetMax = default(System.Numerics.Vector2), System.Numerics.Vector2 buildWorkInsetMin = default(System.Numerics.Vector2), System.Numerics.Vector2 buildWorkInsetMax = default(System.Numerics.Vector2))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3998) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-10f72501acdc0d4c"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportP.CalcWorkRectPos(System.Numerics.Vector2 insetMin)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67253) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6b479a570da4bbbe"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportP.CalcWorkRectSize(System.Numerics.Vector2 insetMin, System.Numerics.Vector2 insetMax)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67264) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-82e95f7cf02cbfc4"></a>[`public void Inno.Native.ImGui.ImGuiViewportP.ClearRequestFlags()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67242) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5db15dc4c0f3b0fc"></a>[`public void Inno.Native.ImGui.ImGuiViewportP.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67231) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1d952bba94b55f3a"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiViewportP.GetBuildWorkRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67308) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-726c2d55c3a50ce0"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiViewportP.GetMainRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67286) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f81a93422ac947a3"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiViewportP.GetWorkRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67297) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9abdef84cfe970f6"></a>[`public void Inno.Native.ImGui.ImGuiViewportP.UpdateWorkRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67275) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0d67b12edb7ca3c7"></a>[`public System.Span<BGCS.Runtime.Pointer<Inno.Native.ImGui.ImDrawList>> Inno.Native.ImGui.ImGuiViewportP.BgFgDrawLists { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3987) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4d93c6f755da2da3"></a>[`public System.Span<int> Inno.Native.ImGui.ImGuiViewportP.BgFgDrawListsLastFrame { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3983) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a3d0b98de641d891"></a>[`public struct Inno.Native.ImGui.ImGuiViewportP`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3967) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiViewportPPtr</code> · 44 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiViewportPPtr 95d2e91994dc01e8c47818f124892f754f9e408ee30d614836b4d62cc5b5aeae -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.#ctor(Inno.Native.ImGui.ImGuiViewportP*) ab443ce9934c578a590d7f4f9671184b348850bc01a73eaf868b754de6b76a2a -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportPPtr.Handle 748ebba5aace7ba0ccc932fb575c58b99bcd64a198dc3422849aefde502e1c35 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.IsNull b6ec15767bca757d5abc9907bbf27a91009b329ca102289c232c0ef5e5414c32 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.Null 80a18ddb3eb5ff05b3e751939f084b9f59fac838ec832b014675d4cfb87d5ce1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.Item(System.Int32) e0af4f37baebd451ecbc44614543368ac6ea5df7547bf2ea3c5a6caf110061b8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.op_Implicit(Inno.Native.ImGui.ImGuiViewportP*)~Inno.Native.ImGui.ImGuiViewportPPtr 5f044090184462b3471170f4ebaf594f5d13a94bbc88cf9964f0e80ea5796b20 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.op_Implicit(Inno.Native.ImGui.ImGuiViewportPPtr)~Inno.Native.ImGui.ImGuiViewportP* c20be7c53ea6aac884eda1ea737444da30814a94c3ad8cb15133039d02db417a -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.op_Equality(Inno.Native.ImGui.ImGuiViewportPPtr,Inno.Native.ImGui.ImGuiViewportPPtr) 7556d889949c125af83c539bc5f39657e582856d9c9b3eae358e6ed26470e837 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.op_Inequality(Inno.Native.ImGui.ImGuiViewportPPtr,Inno.Native.ImGui.ImGuiViewportPPtr) 1c90bce3f6f747cb5eb535908ab8268b86448b6d1c166c8e98d4337fcfbbfa27 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.Equals(Inno.Native.ImGui.ImGuiViewportPPtr) 2460afcd2fa3a9d60f5e1c692563848ddac8c53f5fe6169c5d93dbd1132ff230 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.Equals(System.Object) 846546c4b267512f749fc6d986babc3eebe99739414e64adda7f2865da9ecbe4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.GetHashCode c2cae337f130ad43aa6ce75fc3dab4ad5d0e4a3ae16ae123480124e13e2f86ea -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.ImGuiViewport 46ef2bc3c18787da700a8e7c5e2a8aeb0a51b81c32638842b8ccb0696c2d0dbe -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.Window 875786d74d4c8a8d0181d5991bfc0220dd08fce76ab54912abb227220576b81e -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.Idx 440b444fa5e663f003f803e963f31981d4bcc6b1e5789e6d9d60278418931a16 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.LastFrameActive 0956afb53839fcd7668eabc1474172413e9136fa138a22aea77692e0bad28528 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.LastFocusedStampCount 2a950e2abc230b5c81ccc20a613c53ea45f3c1f1816ea0bf940208912f66279a -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.LastNameHash 70c86dd25b46f999328d0ac553c8c9c5bead66ac74fdde2a7495944586068681 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.LastPos e39fd8e5a4bb3e79549575a4ef9689965d0d6e1c69433b1433abb5cb4727282a -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.LastSize 1d4b42996a6a27b39575b0b95e686e3bed438db1d035f1270280c0e1233cb11d -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.Alpha 4abb5b2fd46eb6cebe366d73f6e65462efd388d11215d76af1546a949a25c7fc -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.LastAlpha 67974de6d9d4e1ac1e6030b709abfa41f16435f9e92dec4e69dbcddb1c1507e8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.LastFocusedHadNavWindow cfd68a8fb09942dff23a76283657ae578ccf48546ded3a9ce2a2e1c76555850c -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.PlatformMonitor deb1b069c2e9f670c7e4e194228945be685aea6d11e5187993353f29b5b8b401 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.BgFgDrawListsLastFrame f30a292926d9fd7967ed976278c01ca8b4758581294d6b55dd791a3826b9cc2b -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.BgFgDrawLists cb375376e05e4a3e2c7268419ceedf42fe68c8211dcd4e8bcc38f7872a4df00c -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.DrawDataP 7409def56a69e32935fafa1fa60d30e61c0fff2775fab5857e46de5c91b372da -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.DrawDataBuilder 8f45b78330322bd38ee7ff5bc67b8a5615a41de56cae5084c8b4303c7abc392b -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.LastPlatformPos 058d1a396dc2adafab74e15700804060d4adc2512692dd1d224a1268ec66ab5e -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.LastPlatformSize de435c88d863bd518d165266373d730a0bba3c4423e5f1266e2c1a47c98c83f6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.LastRendererSize 9e4dbcea5f04d14128e978226a86154bf1393e4f4e144da3943488e640255652 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.WorkInsetMin f758946c8c26a113587ef8490d5c082fe4213df44b1ff43fbbdeaa20677f0157 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.WorkInsetMax d6e74c126efc9e4df8d4beaa3de577627e6d46821b54b389d5d003cfe2fa8576 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.BuildWorkInsetMin 061d0c79a5a995bcf5a06fa2c81fb834ac0e173f9c53508cd54f0a70def7044c -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPPtr.BuildWorkInsetMax dae1e502ae15de08d67a7f14a22111c4cc5943f1a6a284dc0ee1e9d3bb2b9129 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.Destroy eac66b2a3d3cbc2b3a4f7963be931420187fbe7024e44e26a641d60678fd3f96 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.ClearRequestFlags 766c48c5e2be75453d69336a19ca9d05aaf9204346f097f2da718613fc0e12fc -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.CalcWorkRectPos(System.Numerics.Vector2) 44ea6d9098c0bdf7da6c350aa4d02937acc45524c62c53b0e2d90e52d3b72ba0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.CalcWorkRectSize(System.Numerics.Vector2,System.Numerics.Vector2) 23823aed78da5e7cc3ec705578effb18d6233aba22a20aee47e7fab495cc418b -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.UpdateWorkRect 95e25d0d494babf596335b571614f282acb0e982883db58742254c7fc936f58a -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.GetMainRect 7bd9f2ec7c65c872445d9f3256e6b5e375d2e9b36676909fd2bacf7974c1cd43 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.GetWorkRect 38d3d98ed18308121b7be8089dc9b39f0b715e69ba3bd4dd2a7b60763eea8932 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPPtr.GetBuildWorkRect a86de210877cb5d65c613cb781df2a069d28f0cec8b3519edf2bf28577c3df5a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-84f355db4edd41d2"></a>[`public Inno.Native.ImGui.ImGuiViewportP* Inno.Native.ImGui.ImGuiViewportPPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17704) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6c6b92bbcb32ee82"></a>[`public Inno.Native.ImGui.ImGuiViewportPPtr.ImGuiViewportPPtr(Inno.Native.ImGui.ImGuiViewportP* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17699) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7f1851181d97af7e"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPPtr.CalcWorkRectPos(System.Numerics.Vector2 insetMin)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17761) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-93388380aabafb52"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPPtr.CalcWorkRectSize(System.Numerics.Vector2 insetMin, System.Numerics.Vector2 insetMax)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17769) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ce229be68fe89b7a"></a>[`public void Inno.Native.ImGui.ImGuiViewportPPtr.ClearRequestFlags()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17753) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1b06515962220fab"></a>[`public void Inno.Native.ImGui.ImGuiViewportPPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17745) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bac4fd30a6d31570"></a>[`public bool Inno.Native.ImGui.ImGuiViewportPPtr.Equals(Inno.Native.ImGui.ImGuiViewportPPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17714) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-c09053a55f10ed72"></a>[`public override bool Inno.Native.ImGui.ImGuiViewportPPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17715) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-7f1b2f577c9e477c"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiViewportPPtr.GetBuildWorkRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17801) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-713eeb4bbe06cffd"></a>[`public override int Inno.Native.ImGui.ImGuiViewportPPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17716) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-353110ef17f86b7f"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiViewportPPtr.GetMainRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17785) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1dcf49f52d52c49a"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiViewportPPtr.GetWorkRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17793) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d22e5279cb73586d"></a>[`public static bool Inno.Native.ImGui.ImGuiViewportPPtr.operator ==(Inno.Native.ImGui.ImGuiViewportPPtr left, Inno.Native.ImGui.ImGuiViewportPPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17712) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6a88806428583456"></a>[`public static Inno.Native.ImGui.ImGuiViewportPPtr.implicit operator Inno.Native.ImGui.ImGuiViewportPPtr(Inno.Native.ImGui.ImGuiViewportP* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17710) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a3e8d9bf6645bfff"></a>[`public static Inno.Native.ImGui.ImGuiViewportPPtr.implicit operator Inno.Native.ImGui.ImGuiViewportP*(Inno.Native.ImGui.ImGuiViewportPPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17711) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ed99dd164f3a07aa"></a>[`public static bool Inno.Native.ImGui.ImGuiViewportPPtr.operator !=(Inno.Native.ImGui.ImGuiViewportPPtr left, Inno.Native.ImGui.ImGuiViewportPPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17713) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9080237b74f7acd3"></a>[`public void Inno.Native.ImGui.ImGuiViewportPPtr.UpdateWorkRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17777) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1603ed34fb44bd6b"></a>[`public ref float Inno.Native.ImGui.ImGuiViewportPPtr.Alpha { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17726) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1370d803a47bc73d"></a>[`public System.Span<BGCS.Runtime.Pointer<Inno.Native.ImGui.ImDrawList>> Inno.Native.ImGui.ImGuiViewportPPtr.BgFgDrawLists { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17731) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a94b1b88b5290c9f"></a>[`public System.Span<int> Inno.Native.ImGui.ImGuiViewportPPtr.BgFgDrawListsLastFrame { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17730) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-01e5113a6c1e2ace"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPPtr.BuildWorkInsetMax { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17740) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6cdba0e481caec73"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPPtr.BuildWorkInsetMin { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17739) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a6ff52b4ef688e4f"></a>[`public ref Inno.Native.ImGui.ImDrawDataBuilder Inno.Native.ImGui.ImGuiViewportPPtr.DrawDataBuilder { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17733) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-42e3fdfb98c662e8"></a>[`public ref Inno.Native.ImGui.ImDrawData Inno.Native.ImGui.ImGuiViewportPPtr.DrawDataP { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17732) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e4f57ce12b51d21c"></a>[`public ref int Inno.Native.ImGui.ImGuiViewportPPtr.Idx { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17720) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f3c3552d26962b88"></a>[`public ref Inno.Native.ImGui.ImGuiViewport Inno.Native.ImGui.ImGuiViewportPPtr.ImGuiViewport { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17718) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-55c7372b331335ca"></a>[`public bool Inno.Native.ImGui.ImGuiViewportPPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17705) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-85caccf431fe7af1"></a>[`public Inno.Native.ImGui.ImGuiViewportP Inno.Native.ImGui.ImGuiViewportPPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17708) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-549718d79f9c6a61"></a>[`public ref float Inno.Native.ImGui.ImGuiViewportPPtr.LastAlpha { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17727) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5db21028d01450e8"></a>[`public bool Inno.Native.ImGui.ImGuiViewportPPtr.LastFocusedHadNavWindow { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17728) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-28203f29c2f7138f"></a>[`public ref int Inno.Native.ImGui.ImGuiViewportPPtr.LastFocusedStampCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17722) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f16ee2e01350aa9c"></a>[`public ref int Inno.Native.ImGui.ImGuiViewportPPtr.LastFrameActive { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17721) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c427b2625dd07775"></a>[`public ref uint Inno.Native.ImGui.ImGuiViewportPPtr.LastNameHash { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17723) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-616a3938a19c632c"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPPtr.LastPlatformPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17734) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dd808817499a77b8"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPPtr.LastPlatformSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17735) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f311c058e906f91f"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPPtr.LastPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17724) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-84dc8898b9ff8ff8"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPPtr.LastRendererSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17736) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a681f7b2f7076e43"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPPtr.LastSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17725) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6d8f09f4946ba32c"></a>[`public static Inno.Native.ImGui.ImGuiViewportPPtr Inno.Native.ImGui.ImGuiViewportPPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17706) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0b7aeef6245d36ef"></a>[`public ref short Inno.Native.ImGui.ImGuiViewportPPtr.PlatformMonitor { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17729) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0ab3613a6e26c23e"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiViewportPPtr.Window { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17719) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1e10f33677ed6058"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPPtr.WorkInsetMax { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17738) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d2b1523d7d4d7b2c"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPPtr.WorkInsetMin { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17737) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-19b826e105eb7798"></a>[`public struct Inno.Native.ImGui.ImGuiViewportPPtr : System.IEquatable<Inno.Native.ImGui.ImGuiViewportPPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17697) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiViewportPtr</code> · 35 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiViewportPtr f7e2304b5c48a349a72df46b785d6609b9e5fa37fec2c5d7eae400e856c55140 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPtr.#ctor(Inno.Native.ImGui.ImGuiViewport*) 30d416d19452d80a03035f478d01d20582c282a5c5215373b098d3b30670c891 -->
<!-- api: F:Inno.Native.ImGui.ImGuiViewportPtr.Handle ba52cc1890f95ac2ba403d5784894efe3e7ac32f41917f00e4d18f7a85e13c49 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.IsNull 2be130a3b5f1a7370db6a2f96b58b10f4c2ea66fa8550371466deb54b35163f7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.Null 9f36eeff4f89742e262a01daf6586a8e19e510453f7f997f4a31755d11aa1e8e -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.Item(System.Int32) f4873b9b4b479f3ac2bce0e8aed1bf968d91ccb614ef8d33ec3c564ee062d443 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPtr.op_Implicit(Inno.Native.ImGui.ImGuiViewport*)~Inno.Native.ImGui.ImGuiViewportPtr a7a56b82fb236e6aa5c0956f08bae113d9e3470540c47f018dd2516b4a0f1d1b -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPtr.op_Implicit(Inno.Native.ImGui.ImGuiViewportPtr)~Inno.Native.ImGui.ImGuiViewport* 9f2d7f453e45cf5ef2b192effc104d9d233006d33c89ea77234c96d0cc245f88 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPtr.op_Equality(Inno.Native.ImGui.ImGuiViewportPtr,Inno.Native.ImGui.ImGuiViewportPtr) ac68ab2a04e3d1406c261851ee6cb963916b107c3bd7e198caca5b2089f2444f -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPtr.op_Inequality(Inno.Native.ImGui.ImGuiViewportPtr,Inno.Native.ImGui.ImGuiViewportPtr) 74db2a7decf55b913ec42a838c18f7065472bda4f48449886bf77656ed02e9bf -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPtr.Equals(Inno.Native.ImGui.ImGuiViewportPtr) 478d226059c8b05a906ed55b8d30ecc10b49b09d5a50a99c9ba9f2da2fd165a8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPtr.Equals(System.Object) e4ebaad6f495bf33bcc12e993ed655a1b4dea57d8c4b4dbafc265606acf8e754 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPtr.GetHashCode bd227992092f8d3328666303158812778045d8e53cb2b3856b5bb86e544aa9da -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.ID 046d43e4a44555629cce971e7ef20f1313d57ca7952c60e058dee133e70d7895 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.Flags 7d379ebfeb467aff6012a45a16c1c22260b492960034189147224cf795e2b567 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.Pos 2cbd5f95f2978cc029357b2695cb314689c41459d0a9a42e98339bec03c63331 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.Size a8c7c27f36b9727e011ad3559c189852cf4d3be97e9b3c1c77c8e140efd05f80 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.FramebufferScale 4606e3223ec0a74eacc41ba0cf8bf10d1f020ffae6ae0787a3615192d25fcbea -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.WorkPos eabbe6ee60f1946938151350ab1da928ef9b28774fcddc6b510250c69365fe32 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.WorkSize 5e5303e284f311940ae165cb2860d3de540d98be0bc1c0b8569414f3d8b5a3bc -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.DpiScale 86fb50e9b73e7b0bc6757c0aaecd067da0e81314c26067caa14f38cff50a1f99 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.ParentViewportId d50df56ed4deb97c818b0158c10181fb9cb78d3f70ca13b5d20f16626c740883 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.ParentViewport 3a1308e5358f2d0e64a18becccb04952c70bb88636ca62d9e3155c7856d2f3e5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.DrawData 664acf8ca20177cd6f80a283e0ee1613b5c36abf2c465a1833b174002a0f3d03 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.RendererUserData 8331776f989bb3c525774892502d99a0fc1b03c9ef80e5fedea8b4902d20db8c -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.PlatformUserData 7275d48ea3d6aea94103b65f783a84031ca5a1f9b258f9fd8b7cbb3bea88f1db -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.PlatformHandle 7ba150ff38f10b32b5c7beb80b735f584321b6eaaeab48ab8383d16ce84f3a54 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.PlatformHandleRaw f544233d4cfd7dea6936d5b6c15ee9445ba63348c08aba1da57652fed4f0b5f2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.PlatformWindowCreated c4e0f28ab83ccaf6bb2402a6a50fb83979669f5676ee74d8e6145820a2e9e8fc -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.PlatformRequestMove 229b411b5716c34186ca57e122a429f994202a970128902021788728367f6131 -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.PlatformRequestResize 9345d31e15afab8eaf038a3ac590a4ac6ece3f94d9d4a90989fbccb8faf6a79b -->
<!-- api: P:Inno.Native.ImGui.ImGuiViewportPtr.PlatformRequestClose a43b8ae0b98044e1dd5e4aa12647ad18e5b4898db832dd94b90d1fe7db68f616 -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPtr.Destroy 9e1b4ee8c6a9d4ec64fbd821768acd5c1f622483ea51d1ea849a0d922b06381b -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPtr.GetCenter 7c2a94d03ea8b4fb9dc10ea78ac820ce257a31dd0239ad4e05b94c102827c22c -->
<!-- api: M:Inno.Native.ImGui.ImGuiViewportPtr.GetWorkCenter 211525ceb249b8b3a8e792d29cceea705e684227eb8d86c30dbc6b7a51d9b165 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-847b60e34a6e7425"></a>[`public Inno.Native.ImGui.ImGuiViewport* Inno.Native.ImGui.ImGuiViewportPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17815) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-275dd11ec18c9a3d"></a>[`public Inno.Native.ImGui.ImGuiViewportPtr.ImGuiViewportPtr(Inno.Native.ImGui.ImGuiViewport* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17810) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e2a122eca07b66a1"></a>[`public void Inno.Native.ImGui.ImGuiViewportPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17852) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-547076958fd91a30"></a>[`public bool Inno.Native.ImGui.ImGuiViewportPtr.Equals(Inno.Native.ImGui.ImGuiViewportPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17825) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-11b7c5e36a8e521b"></a>[`public override bool Inno.Native.ImGui.ImGuiViewportPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17826) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-e78e441ea7c9727a"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPtr.GetCenter()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17860) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a8fe5a755c4fb921"></a>[`public override int Inno.Native.ImGui.ImGuiViewportPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17827) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-61a493e5a715b2ff"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPtr.GetWorkCenter()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17868) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-43dc6b8e9ed4d54c"></a>[`public static bool Inno.Native.ImGui.ImGuiViewportPtr.operator ==(Inno.Native.ImGui.ImGuiViewportPtr left, Inno.Native.ImGui.ImGuiViewportPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17823) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-84b525589a899c9d"></a>[`public static Inno.Native.ImGui.ImGuiViewportPtr.implicit operator Inno.Native.ImGui.ImGuiViewportPtr(Inno.Native.ImGui.ImGuiViewport* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17821) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8cf462a9ddcb5ea9"></a>[`public static Inno.Native.ImGui.ImGuiViewportPtr.implicit operator Inno.Native.ImGui.ImGuiViewport*(Inno.Native.ImGui.ImGuiViewportPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17822) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-53571dbada709eac"></a>[`public static bool Inno.Native.ImGui.ImGuiViewportPtr.operator !=(Inno.Native.ImGui.ImGuiViewportPtr left, Inno.Native.ImGui.ImGuiViewportPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17824) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c91967c77c750c48"></a>[`public ref float Inno.Native.ImGui.ImGuiViewportPtr.DpiScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17836) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-494cb12246832fd5"></a>[`public ref Inno.Native.ImGui.ImDrawDataPtr Inno.Native.ImGui.ImGuiViewportPtr.DrawData { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17839) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c6d2efaf38318cd5"></a>[`public ref Inno.Native.ImGui.ImGuiViewportFlags Inno.Native.ImGui.ImGuiViewportPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17830) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-81fee6025e27d780"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPtr.FramebufferScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17833) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-112d7de19b330c38"></a>[`public ref uint Inno.Native.ImGui.ImGuiViewportPtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17829) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2bbedadff0bbf9d0"></a>[`public bool Inno.Native.ImGui.ImGuiViewportPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17816) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cdc36d6f6449eafa"></a>[`public Inno.Native.ImGui.ImGuiViewport Inno.Native.ImGui.ImGuiViewportPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17819) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4aab2a758602e1a2"></a>[`public static Inno.Native.ImGui.ImGuiViewportPtr Inno.Native.ImGui.ImGuiViewportPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17817) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-db1bb75fcc49dd13"></a>[`public ref Inno.Native.ImGui.ImGuiViewportPtr Inno.Native.ImGui.ImGuiViewportPtr.ParentViewport { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17838) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a348707f95c5e26f"></a>[`public ref uint Inno.Native.ImGui.ImGuiViewportPtr.ParentViewportId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17837) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-47f8fb0957441b52"></a>[`public void* Inno.Native.ImGui.ImGuiViewportPtr.PlatformHandle { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17842) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e8179b07fa0f09e1"></a>[`public void* Inno.Native.ImGui.ImGuiViewportPtr.PlatformHandleRaw { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17843) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d9415064993e6e77"></a>[`public bool Inno.Native.ImGui.ImGuiViewportPtr.PlatformRequestClose { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17847) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6bf25a239fa35576"></a>[`public bool Inno.Native.ImGui.ImGuiViewportPtr.PlatformRequestMove { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17845) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-18001141c15b1cfe"></a>[`public bool Inno.Native.ImGui.ImGuiViewportPtr.PlatformRequestResize { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17846) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7e7eb5e2c5009ae6"></a>[`public void* Inno.Native.ImGui.ImGuiViewportPtr.PlatformUserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17841) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8d6c6f4e4638d159"></a>[`public bool Inno.Native.ImGui.ImGuiViewportPtr.PlatformWindowCreated { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17844) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-79f586da5baa47f6"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPtr.Pos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17831) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4013f3d1cb854d06"></a>[`public void* Inno.Native.ImGui.ImGuiViewportPtr.RendererUserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17840) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-91c68ef65dcf7152"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPtr.Size { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17832) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1e0b4b975ba0d9bc"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPtr.WorkPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17834) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cea1edd2f867ad0b"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiViewportPtr.WorkSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17835) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b226a5b21a21149b"></a>[`public struct Inno.Native.ImGui.ImGuiViewportPtr : System.IEquatable<Inno.Native.ImGui.ImGuiViewportPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17808) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindow</code> · 148 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindow a333d790727ac667e96742989c791e5d40695b66fe3b32abf7fb4afdc538cf2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Ctx 3534fb468c6544ff84d2547e94b6b20818f1656dd80ae8b785edd7d002ce43ed -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Name ccdcc8204def6cc436099208b4595596a0a2f8edb6995cdb5fb1682f68386614 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ID 1cf64230b2fdbe5ba812e21789c586d2fce6e6eea4bbd914d5f5ba98951f152e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Flags 54684a231c8c7d3540cb707c70cfffcb86b4927231d11901ed2c1f5b52d7b429 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.FlagsPreviousFrame 3b170268500b07942a6adf6630ac5417c4f039eb4bddc78e710b73aa9f63fb8b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ChildFlags a60a2a02c07bec75910348db763844beaa6b61d9fe7926504a3c84e36fff31ff -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WindowClass 3c8a76503b0a75a31bebfabe24143a7325b80fb0d31cf2cbc0405f0719685ed9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Viewport 1566afac1c1a42b8c360b427e489c56fe9dc0382007562a38610ec33b7f52e12 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ViewportId f957c9a72408a709bcbf89b1f93f89922169bfd49d616038d85653193e7c1937 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ViewportPos 0090d1e42aec030d86bf169fea0773014e76880dced85b96df512377ad5b6c09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ViewportAllowPlatformMonitorExtend c727abf1be03c57c3346c88b1862ee78d8fbc6b33d7a1f99ef6af5816f64ff90 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Pos cd3442a420475858d095cde9bbba6d7813b4237656b8c57eec9c222e72f6d6c0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Size c0f45ee5904f1aa870ee900c28971756d8539168323dc4d7c8529f19536d1df9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.SizeFull 5ae0bc29ffd900abcfca4fcad94f86a925698cd98e26bc8dadae8f98aca70297 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ContentSize 9e6d0cb8289b39c9fe58ac91ec808a829d0251f9eab3b4e993ce682721209baa -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ContentSizeIdeal 72855d93ebf004f9ce7b7d1c225bab318e896df8a8700eaef8009706578a9099 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ContentSizeExplicit ba026653b190b64911725df70be76b5a3d3512e774a63645a5777379074a889e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WindowPadding 97265b0b7f969efd9610a67dc8d012d54ecc5a4e7f6bd606fedfece2cefa454d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WindowRounding 8461cb94e205ee5c109eaa7d10e3795aef02e3ab0405ba56b51c4b6a463945b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WindowBorderSize 8cf86e70872a566394b2fd9cb9553624c1e99c849cab8c63d4b858cede20139f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.TitleBarHeight b2976c4353652e4b76bcd6b62acafee41fcb0ba004cc1d99501d8d04b2d6e8a3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.MenuBarHeight 588f3a750c3a393f058e7479c91374a0a33d2ee175f3619ecc8d4806e2e99272 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeX1 84f3fb84e5255a37df679f3efbbd0df914ccbdf811a7242150c67c870f6a700e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeY1 f19bad36efd3c04e7ade7c12dd2b99740ca4982fbe6326cf14d2e2ed48f6f3d4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeX2 2d061576f4b769a105fd1e6544c97746c1ccf1658a84c90c423d4869b5d8869e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeY2 94efb7478319cd712fc6dcfa6e29a3920d8d3228926856fa63eaa27e0fdf779d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DecoInnerSizeX1 6a4769216bb4ebfd74f5183de87e4bf419bb638dd393d64e09e42fb23d885209 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DecoInnerSizeY1 483f81fea96c236207e846774beb7260eed3c68841b1222c81b9544b72fc4b5f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NameBufLen 6a35fc6e51d85bf7ab2eb77661c3eeb8286dc5193796cde3a8db9adc8d607577 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.MoveId c97223bfa0349b861dd7feb2a6d3782354edef87f6a06fb001e9336d286a7a51 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.TabId 6df9eacde57f4840f3ad8990632daf4e942b0f377813e7b7496a74cd2c047ee9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ChildId 786004ba7c05f66e039f570ec86f90023fcf61ac904354fe101ecae1891ce812 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.PopupId accd526eec6aa04dc5e8fc624bf3862dd696bfb7443be24bad3b2e2827b8283b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Scroll fc9ef8546739489032d4157ad07f798f3c61e79e05e66b9f5315bc963cc13e9f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollMax fffe5e05bd17ad5d763f65e0a34b6e19fcba009235cdbf07b13298e68f233b97 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollTarget b8483212cb78ed9eb3dd54f46bff2bee54c93f8128dcd2d47888b2986212beb6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollTargetCenterRatio fb4e9d8cc4618ffba7c9f1efacb0fa7f7a14752f594e10d9709e459f6bf5a913 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollTargetEdgeSnapDist 11be1c41506a52b0e9281f6ed0043575e5798744bb4ac0d677ecc8e2104247e4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollbarSizes 89fa2d5dd25ede638bbcb13bc8d79ac5347cea2df9c3b98b16fed8112357753d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollbarX f2194d7c7eb1f654187122a1fdb5860192bab396a6e83a39aad9c38e1baf97d2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollbarY aa82bd9ba94537de9c7be2dab0cf2862a42057c0fc35b3f9dac979b012457ad7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollbarXStabilizeEnabled 95f3896ea6f35678f9575f11eddb2ae504a1de67c10188eb1aa044754e2035df -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ScrollbarXStabilizeToggledHistory b7d904be74ab9e467b229483821f90510a915466e3e904ab60ce8a8b7a43ad9e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ViewportOwned 05523704b73b4118294a90cb507cdb365f0f56651ec6bd1aa66e54388ad58319 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Active 919d0ea6238382d1dfbacae08dc56d53f0d66cd7f47d78fe44da3d4512da3e98 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WasActive e6b6d02de3dd21330e89092616335707b1e15718d3cd8ab52530c3748b7be19b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WriteAccessed ad6db20800a75711c1a64295b4b88888a857206fb88aa5cfcf83d009affeda0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Collapsed 232a09e8e28abc84d16cafd17f6f28aed57193be2eb08aa3aa56181cde101a7e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WantCollapseToggle 3c82e549926f39966381e2a2d5e9ccf3dbaf83513ab1ce008324ee17971d9812 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.SkipItems 4a235e302c78116ee1af379b155fbdaac0a4ff0ba16b54858ce3379134acecaa -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.SkipRefresh 132d12c60e003ea1fd83c37f4e559815b45186b9575d4c105198e50a8cd60ab7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Appearing 50ca0f55682e9e9bcd22ea5cfcce8b1307ea7da78a04a345d88cbdd40ed3f4a9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.Hidden 04edbcd24a0cc21bc4408d8653463302233c8424c8768a11469702154d0eb557 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.IsFallbackWindow 143c1827c81a868e378609165a1cd54d7ee1e543e2e898126a55c577429554d8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.IsExplicitChild 7fdaef5ff07b46e1aec582a89aae2a3ba8f8e48dcc5c6136eb2ff4f8f9556bcb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.HasCloseButton a3ec5c3eaf083003f2bf217a2fae18f4f4a04ed2c6f40040644a2677cb9bbab4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ResizeBorderHovered 9ae8f299506d8fccd72fd52b271050d79c1449e054495aa659ac1a27dbb87c3d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ResizeBorderHeld cca42b552717b5651fe47a6918c4e814e2a0c7ab3dbf0e5a485d176105b47d0a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.BeginCount 065ca40d6e5e7e9473adbb7c71e528906adc6c5a5b0bb68448046dd151d18f68 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.BeginCountPreviousFrame 9d01652671e3ab44eb5407178bdaf36fcb8e359a48ee2836d423e7aca00fd1fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.BeginOrderWithinParent 381396cdf6275de281ac2889843191f3e9b984266e34c8459ae71a7ed735908d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.BeginOrderWithinContext 377e73c612c0e949f2a0b31d43a85ecca4ce7932bad341a2740b30b83660a4cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.FocusOrder d6db49de6b89c35ccb7c463f795d1d9e909019d5e8fac38bd0b75eb7649fb3d8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.AutoPosLastDirection 367a3c28b751122e63394f0148518fda67ed37904b4daf347ef4f2d86788c58a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.AutoFitFramesX 65439b0cdf479dbeb63fdc983d49276238ed916947330ea3e42a961b5f8c84b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.AutoFitFramesY 55cfcbd17ba69fd8c368dd969f5dda72490fa13d2f20f58a228aacdead6d7606 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.AutoFitOnlyGrows 2c7e9226bbe4fece36ec975d964112357733f736c4f04a8254e6a65390e9526e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.HiddenFramesCanSkipItems 521d5ab2341aee48f1e151b9ba85daadebeb94291a78edf50289bdaaa947c6c0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.HiddenFramesCannotSkipItems ffce7f49d694743389369bf4e4262215d96f8ecb0ff21441b7efdc8b54b0fc07 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.HiddenFramesForRenderOnly a1abc42e3e8170a61b26da594397fb0fd55ee545d468717780ec87de3927b9b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DisableInputsFrames a0877bfea60501063e45d865761db1f8786d6e200ba759cde522c46aa5fe34c5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RawBits0_0 105d911b4db1a98559616aa0b0efc06db5bc9a3ddb1db768b9c6b6d4d883e9b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RawBits0_1 8f67001f90ecf94e665284e978589e5e9748ef193e7d0a843279809f026e21e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RawBits0_2 81374efc2f50b630147db338c69b96ee451202100d96c7eb41681db631ac5b0d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RawBits0_3 5349581fcec6d74c704baae3b7c744dba3e64b6ea299c9d57f4786cc08998cda -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RawBits0_4 bc8881129742ec717c95260a5633ea19288daa3d6189be35ec259b7d7edf94b3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindow.BgClickFlags 380c00d47fbfca166cd072ebe04f993d919acfcb3352d7f22b8283537bcd4c67 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindow.SetWindowPosAllowFlags 6d21494e9544f1694f46565422251d8eca2aa6f58fcce1b39a16e7fff3ebac4c -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindow.SetWindowSizeAllowFlags 3f96c489f01828abe23032b0ae844576d413b0e1beb8bad1f08bfc61f481942f -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindow.SetWindowCollapsedAllowFlags c6112455250ff4dc6e8605a32489af25bb66d79713c29e3c18f71a520468470d -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindow.SetWindowDockAllowFlags 7db581a63d68e25dd9e5708bb71cbd378d65d4bc0ee30717cd1d0be803308157 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.SetWindowPosVal 21061a040d8bf7cb8c5884efef52c4c478ace3c7b6e4871deb3db9ee9962e968 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.SetWindowPosPivot d876f81c1e6b8d01b1ffb366d5b56c1e89fd994349d20d231f41b22f7aeec272 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.IDStack 971cac8626d8236ca58c24d0fa35d46acbe9427ed97bca6c4731c574dc38091d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DC e05d46aa35f1c561b5e5f7580a4937452514c5a16a921c8d1c0c5ec9ebe61264 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.OuterRectClipped 5bc85ba68be511a78ae90d5d4d12ac5f49c4a15b6a4167de10eb84e944771b03 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.InnerRect 3614047cb7fc66e53ecaecbc7d7eb8b3bbf17f08585f6c3601b324d9e7bedcd8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.InnerClipRect 80ee33dedc658b8183f2b9bc0ae86195917441e6c105caae5d94df7ea52a2310 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.WorkRect b9ca97bf64be12a41d7e8f717fd7c4eb0cb039f53b97cdd34336abeb7936b803 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ParentWorkRect 0541bf05df99ab20e18c27b18f589166e8327f6264080e5d6316a749ee4615b9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ClipRect 1a30b1dc19f5d01e2a14db010721803fbb9363a71c354e61a4afcf32c67812eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ContentRegionRect 499494324cb5253cdbaf645b0de961f74bd0154b197fcfb65018a2a97460d45f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.HitTestHoleSize 1ca184f116b4e36a8a844480edb04984044766e760217723beef3e71401b0cae -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.HitTestHoleOffset e3313c4994076746ab61405a121a2521daa1e15fd5368cb510f4e4d017be8cf1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.LastFrameActive 79bfb127fff75545cec12ad7f18f32993a1a9258f194e3d3f92c7138e987608c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.LastFrameJustFocused cae2dbb5e09d45096018ac4a8ff6bf33ea1a4b5d7931c73a522bc1b6dbf93ccb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.LastTimeActive 98f1989ca4d196543eef7f389f32e0311709213ea211b8d684fcf54ac35bb1b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.StateStorage eb601cbd745b89b7cdc2e17cd394ee9a4ce8f21f9ed674f49c326fa9f8bf37eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ColumnsStorage 3810e457c42594b375706262f82769901ad8ae00eabf3d6344d97e1bb54e0a2f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.FontWindowScale 438f23f5fef8de08e27f13c87988315417bc70c43c1efb84e95df402c0034a49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.FontWindowScaleParents 666bad46ca2f243f2564fa6d93d430514df254047b8c088cecf4e51227804942 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.FontRefSize 6c02155efbf336bdec901840e739f3a379aeb85c41b3465020a3d8f6e5731e08 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.SettingsOffset 6b929d719a6e7022b04196f00fcbc7db8596bbe409d9c849439d9a80de2c8bc0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DrawList 2f0b1d42a37c52b8e423f1c42e0ec638049bd5450ed276cc5f874b4b1b0c4b81 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DrawListInst 8e5fb113a4a620508ab0f906ae178f8745ebf8bc711a4ad53f6d10594d05baba -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ParentWindow c4f48ff1359ae798addeaaf2711cab190c8ed4dce0d5bdd82eed318b0c216e42 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ParentWindowInBeginStack 721bcf4ce6545c7416f1cafa51407fc64156ea8eaf0ec1bac0589fe2e4240443 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RootWindow 5ad622d3274971ad859b66efc8612d1f759dc7f9a0de8370958c09093b20ee3f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RootWindowPopupTree cb6b3fa3a33cfd605ed502d3ab8ea520340dfdb983459e7043a045793bdaac42 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RootWindowDockTree 6ee3896fc2705d62c891108547db4923596165e7bb2c15c17ab1985b4191a162 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RootWindowForTitleBarHighlight c563b89c4acab4b9dc73877daf56cb21e961d91c6906cd79937c84aa1fab416f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RootWindowForNav dd10250ed4c058bd316ea2705c9c1846c29abba84af69a48980dce0204c0df65 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.ParentWindowForFocusRoute e6a3c5b9ba5e9e6f66170eea4bd0f0ced3bfd8df91c0616aaa65b583ff0fea9f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NavLastChildNavWindow 4840fc253e3141f7ab8b95a5c5b453c020aeb85ed8a986ce0e3c2ec9dbf1c615 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NavLastIds_0 52dfaa71e26afa558e40026ea8edeaebfc9c8c50b54a427058f8b5a1349a579f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NavLastIds_1 5f04bb1d1900c53beebbe6cf946aa0ba0dd42cf2baa1a6686a43d7d496bb784c -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindow.NavLastIds a1deabc45d3209f565c049febde53ffdac46694dd4fdda386d83037ed0d75cf5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NavRectRel_0 b0b3efe2f7313a04ddf8eeef7db1663db5932bd13d917b3818f86c342679fced -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NavRectRel_1 2a0cc865afd2f6324068733ece7198f1dc47fb14354f3ede9e3edbb60f804164 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindow.NavRectRel 884560a5ae3ff0fe909be8b27fdd2031961a78de142edddfa6f3805f8a827cdc -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NavPreferredScoringPosRel_0 b61fd4e16acbc0635b9da1a3e3da4310692d2e4da6c1c22f16289915b5ebd962 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NavPreferredScoringPosRel_1 e3c6d6b583924c261b15972e6b75d3c9036d588a418d588851b74342a70a6b0a -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindow.NavPreferredScoringPosRel db61388d7e92c1264ad8a3204e8858c896179c5684709e9165298f73138f50ae -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.NavRootFocusScopeId 21f220836078f6e44e9620af718db0b03e7fcbb8e2d0277ea4bce9f4000b7804 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.MemoryDrawListIdxCapacity 8793502507ac2401a4b5b56db25057be8e526f8e93b50fb3405381deb41c741b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.MemoryDrawListVtxCapacity ae7e0b2731675684f66427480c485882d82080befeb31557a0ae7b40113a3645 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.MemoryCompacted 4fdffa6df563f028503a12901451fda0bab699cfc09ebceb5963c28a039b2b9b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.RawBits1_0 605695a3f64c1623b2a0ca906dee434d4a4e081eaf21b91821d10d71b1e67bdb -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindow.DockIsActive 6727b799bc0317837fd7cd70f00f4405507684f523d5694964f6e8a4b0c48db0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindow.DockNodeIsVisible d0996fe57ca8b540f9cfe30d6720057fea1e87d8a09a5546e5c84dd8b50d4fef -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindow.DockTabIsVisible 6df35c9713a4babcbcf2c24bbb3439703faea5d654a2254207185297bab2140b -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindow.DockTabWantClose 3ec3e1d709f512bd472c42f379c8519379ff35aa0c77395f48df90c3aeac54f7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DockOrder 4232db82a18272c7b310e1eaa92a1b4df0e9de8e49f93eeeaf26681bd624353a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DockStyle 0a6e2eebb97673e65ea245d96fb1d2ebe74cb95c655b566e589379705e5d8546 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DockNode 41ed8d8522f79b37322155850327389dc75e356ee884a17952cd5b296841ab69 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DockNodeAsHost 1e1f43a7d01c42538de30c2f6b58a097f198332a16ce380c57a440380608f441 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindow.DockId 23052e5f1586f19d40c14fd5edd385a80bc5d1a99a5840bc94f52e756de8e473 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindow.#ctor(Inno.Native.ImGui.ImGuiContext*,System.Byte*,System.UInt32,Inno.Native.ImGui.ImGuiWindowFlags,Inno.Native.ImGui.ImGuiWindowFlags,Inno.Native.ImGui.ImGuiChildFlags,Inno.Native.ImGui.ImGuiWindowClass,Inno.Native.ImGui.ImGuiViewportP*,System.UInt32,System.Numerics.Vector2,System.Int32,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Int32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,Inno.Native.ImGui.ImGuiDir,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,Inno.Native.ImGui.ImGuiWindowBgClickFlags,Inno.Native.ImGui.ImGuiCond,Inno.Native.ImGui.ImGuiCond,Inno.Native.ImGui.ImGuiCond,Inno.Native.ImGui.ImGuiCond,System.Numerics.Vector2,System.Numerics.Vector2,Inno.Native.ImGui.ImVector{System.UInt32},Inno.Native.ImGui.ImGuiWindowTempData,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImVec2Ih,Inno.Native.ImGui.ImVec2Ih,System.Int32,System.Int32,System.Single,Inno.Native.ImGui.ImGuiStorage,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiOldColumns},System.Single,System.Single,System.Single,System.Int32,Inno.Native.ImGui.ImDrawList*,Inno.Native.ImGui.ImDrawList,Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiWindow*,System.UInt32*,Inno.Native.ImGui.ImRect*,System.Numerics.Vector2*,System.UInt32,System.Int32,System.Int32,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Int16,Inno.Native.ImGui.ImGuiWindowDockStyle,Inno.Native.ImGui.ImGuiDockNode*,Inno.Native.ImGui.ImGuiDockNode*,System.UInt32) 6dd09efc6b57ddc02f4cd52a7673963988844c91a2a705f318f75d79f77690af -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindow.Destroy aab2213836eef669f94a7bab6b9d84bf7fd56740cad0244b6d4f5c269c44770d -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindow.GetID(System.Byte*,System.Byte*) d0328a5d1d7fd3608d52b49b9867ab8426db8f64ae03e7d926dac7683de5b16f -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindow.GetID(System.Void*) 80b3889a1866382f667e98fef8f95c9b9b62ee68fb25b32d07c0695e2a968cec -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindow.GetID(System.Int32) 83582cd44963a296a400c1c5f182aab1cc32239cd61add8578a4fb0ee5288d34 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindow.GetIDFromPos(System.Numerics.Vector2) 745f84e5c56f67dbf4843660c1906ae467919eacd12272b68c1b29c80ce416f9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindow.GetIDFromRectangle(Inno.Native.ImGui.ImRect) df5d004f2dfc29e232f0561de890aea902d673280d5b19e3864446e451bf2b86 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindow.Rect 21717ab0ae60bdf2047ad136d936a493f4f11e069a9bd033d0f0b197e8a25788 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindow.TitleBarRect 703f61204b2da8274cd3baeaf21d738f2c0f108e2e294b023c588385cc50028e -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindow.MenuBarRect 6988dd4bab5bbd44364bf4cfef7f82ed73f2ce118a37c92ab769509d556bff26 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-80dca4416e80591e"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.Active`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4704) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69467560368323cd"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.Appearing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4718) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a8012777649cb8c"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.AutoFitFramesX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4744) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d9a6e3cc272bb167"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.AutoFitFramesY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4746) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e7e2c3b0d9621fdf"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.AutoFitOnlyGrows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4748) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a558e19a7dea30dc"></a>[`public Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiWindow.AutoPosLastDirection`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4742) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-420b3bc302f20b17"></a>[`public short Inno.Native.ImGui.ImGuiWindow.BeginCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4732) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e1f46585d9db7117"></a>[`public short Inno.Native.ImGui.ImGuiWindow.BeginCountPreviousFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4734) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f795c56ea1ca5003"></a>[`public short Inno.Native.ImGui.ImGuiWindow.BeginOrderWithinContext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4738) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-47ca08f9349c921b"></a>[`public short Inno.Native.ImGui.ImGuiWindow.BeginOrderWithinParent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4736) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ca55ad39d77092c6"></a>[`public Inno.Native.ImGui.ImGuiChildFlags Inno.Native.ImGui.ImGuiWindow.ChildFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4626) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-79d795b0eb4215a6"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.ChildId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4678) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-731af9d100384c9c"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.ClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4792) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8922cfb7e0b10024"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.Collapsed`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4710) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-368287765ac98e04"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiOldColumns> Inno.Native.ImGui.ImGuiWindow.ColumnsStorage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4808) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-59d8cd54ebbaa1e4"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.ContentRegionRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4794) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fa7a98c6f5c41624"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.ContentSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4644) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2c99d3f000efd663"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.ContentSizeExplicit`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4648) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-335b7f98d7ecd17b"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.ContentSizeIdeal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4646) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-92d3a281f3fdb2ad"></a>[`public Inno.Native.ImGui.ImGuiContext* Inno.Native.ImGui.ImGuiWindow.Ctx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4616) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa2cebf9f495ccbb"></a>[`public Inno.Native.ImGui.ImGuiWindowTempData Inno.Native.ImGui.ImGuiWindow.DC`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4780) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-694b8444de5377ef"></a>[`public float Inno.Native.ImGui.ImGuiWindow.DecoInnerSizeX1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4668) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c2301d2d2383bbe8"></a>[`public float Inno.Native.ImGui.ImGuiWindow.DecoInnerSizeY1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4670) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f888b8cae35b0bcc"></a>[`public float Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeX1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4660) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-34aa076504db78e1"></a>[`public float Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeX2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4664) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-489d5e6871980308"></a>[`public float Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeY1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4662) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3bcedf37081928b3"></a>[`public float Inno.Native.ImGui.ImGuiWindow.DecoOuterSizeY2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4666) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9dcac30eceb910c9"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.DisableInputsFrames`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4756) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-49cadf083fdeefdc"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.DockId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4881) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b86dc1deb1e4269c"></a>[`public Inno.Native.ImGui.ImGuiDockNode* Inno.Native.ImGui.ImGuiWindow.DockNode`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4877) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec0ee9516e9cffdc"></a>[`public Inno.Native.ImGui.ImGuiDockNode* Inno.Native.ImGui.ImGuiWindow.DockNodeAsHost`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4879) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ba1e6d244acc2b8"></a>[`public short Inno.Native.ImGui.ImGuiWindow.DockOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4873) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e0ab33a745f637d1"></a>[`public Inno.Native.ImGui.ImGuiWindowDockStyle Inno.Native.ImGui.ImGuiWindow.DockStyle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4875) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8bb73f245ab27416"></a>[`public Inno.Native.ImGui.ImDrawList* Inno.Native.ImGui.ImGuiWindow.DrawList`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4818) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3f4a7c2d58c16c45"></a>[`public Inno.Native.ImGui.ImDrawList Inno.Native.ImGui.ImGuiWindow.DrawListInst`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4820) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-31639eae3625393d"></a>[`public Inno.Native.ImGui.ImGuiWindowFlags Inno.Native.ImGui.ImGuiWindow.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4622) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da278c7738fbb30f"></a>[`public Inno.Native.ImGui.ImGuiWindowFlags Inno.Native.ImGui.ImGuiWindow.FlagsPreviousFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4624) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-62a3c655ca286844"></a>[`public short Inno.Native.ImGui.ImGuiWindow.FocusOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4740) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a364ab39b523f94"></a>[`public float Inno.Native.ImGui.ImGuiWindow.FontRefSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4814) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fd630d985bf35286"></a>[`public float Inno.Native.ImGui.ImGuiWindow.FontWindowScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4810) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-faa40eb756c95580"></a>[`public float Inno.Native.ImGui.ImGuiWindow.FontWindowScaleParents`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4812) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dedb9f78c164eec2"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.HasCloseButton`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4726) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b2302e9889c31187"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.Hidden`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4720) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6de1229a0383e12d"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.HiddenFramesCannotSkipItems`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4752) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d3ed9d47a0c93e79"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.HiddenFramesCanSkipItems`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4750) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b3c0c62959c98fe"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.HiddenFramesForRenderOnly`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4754) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a765bd74e93f226c"></a>[`public Inno.Native.ImGui.ImVec2Ih Inno.Native.ImGui.ImGuiWindow.HitTestHoleOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4798) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-38145a8f32d1b839"></a>[`public Inno.Native.ImGui.ImVec2Ih Inno.Native.ImGui.ImGuiWindow.HitTestHoleSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4796) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-77260794b6a18588"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4620) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-52b13c630d0f5c82"></a>[`public Inno.Native.ImGui.ImVector<uint> Inno.Native.ImGui.ImGuiWindow.IDStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4778) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1ccff29a8e615617"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.InnerClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4786) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-636f470e6d7929ae"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.InnerRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4784) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9abd45c8964a842b"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.IsExplicitChild`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4724) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d0d68798066d9718"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.IsFallbackWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4722) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-591feeaba7c13970"></a>[`public int Inno.Native.ImGui.ImGuiWindow.LastFrameActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4800) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf0faa9fbce5b869"></a>[`public int Inno.Native.ImGui.ImGuiWindow.LastFrameJustFocused`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4802) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4224061eb8ad84e4"></a>[`public float Inno.Native.ImGui.ImGuiWindow.LastTimeActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4804) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-864d3354071f40a7"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.MemoryCompacted`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4864) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a3624e577065095"></a>[`public int Inno.Native.ImGui.ImGuiWindow.MemoryDrawListIdxCapacity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4860) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-74c41a44c7722677"></a>[`public int Inno.Native.ImGui.ImGuiWindow.MemoryDrawListVtxCapacity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4862) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d84a7af9cf848cda"></a>[`public float Inno.Native.ImGui.ImGuiWindow.MenuBarHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4658) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7c90b97c5a43e77"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.MoveId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4674) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba4bfe0112400fd0"></a>[`public byte* Inno.Native.ImGui.ImGuiWindow.Name`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4618) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-554dfe5fe9a3b941"></a>[`public int Inno.Native.ImGui.ImGuiWindow.NameBufLen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4672) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0168fbc69518a8ba"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.NavLastChildNavWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4838) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6966127369912f4b"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.NavLastIds_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4840) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc6e2aba85390d29"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.NavLastIds_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4842) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-99b7d3acfc15c3c7"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.NavPreferredScoringPosRel_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4852) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9382a9f051456890"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.NavPreferredScoringPosRel_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4854) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a82693c63ee964ca"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.NavRectRel_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4846) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7874e310056a5c2"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.NavRectRel_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4848) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6033e5631feb0b2"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.NavRootFocusScopeId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4858) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2feb9db7078541f"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.OuterRectClipped`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4782) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b8593154dcd4b38c"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.ParentWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4822) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-077758854e3d9bf2"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.ParentWindowForFocusRoute`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4836) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-40ddbda550e4b89b"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.ParentWindowInBeginStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4824) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a9957bf814bd582"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.ParentWorkRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4790) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-22dfeadcebb789db"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.PopupId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4680) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-99f04f7d751abdc8"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.Pos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4638) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d296b14673286bef"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.RawBits0_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4758) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d25d0d86ab0f5dde"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.RawBits0_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4760) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab0143c02698cdc2"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.RawBits0_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4762) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d410557296b4f9a8"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.RawBits0_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4764) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-982f65cdaf12aa10"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.RawBits0_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4766) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-36bafc82c22304f7"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.RawBits1_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4866) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9a7283922fafd711"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.ResizeBorderHeld`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4730) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a3b9e934bae749fb"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.ResizeBorderHovered`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4728) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9fbba09a41a715b3"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.RootWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4826) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e03966956e3ad821"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.RootWindowDockTree`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4830) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-37934f85547dcfeb"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.RootWindowForNav`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4834) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce7966d01ef249ae"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.RootWindowForTitleBarHighlight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4832) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ee6f068c35cdf381"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindow.RootWindowPopupTree`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4828) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bbdfdb9c289da5b3"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.Scroll`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4682) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b14a782b4decf598"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.ScrollbarSizes`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4692) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8b3ccb8e4a533ea9"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.ScrollbarX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4694) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-41d93596d173cc9f"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.ScrollbarXStabilizeEnabled`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4698) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c64c32e49af23c85"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.ScrollbarXStabilizeToggledHistory`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4700) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7617ff020df61f53"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.ScrollbarY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4696) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfce0e2bd3409247"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.ScrollMax`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4684) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4889f5c8a7c6d243"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.ScrollTarget`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4686) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21c51818097c87e1"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.ScrollTargetCenterRatio`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4688) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ae33a242dc4b8b2"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.ScrollTargetEdgeSnapDist`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4690) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-18066a3077e6c2a9"></a>[`public int Inno.Native.ImGui.ImGuiWindow.SettingsOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4816) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b6f31ed66d9a980"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.SetWindowPosPivot`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4776) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-15a9dd191586a787"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.SetWindowPosVal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4774) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f7affa54bed984ff"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4640) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-80c0a125e670f8fe"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.SizeFull`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4642) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e47daa5e5118fb2e"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.SkipItems`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4714) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01eea428f43bc1a1"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.SkipRefresh`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4716) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4ef93030f84ab0a5"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImGuiWindow.StateStorage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4806) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-262440c2aaca759e"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.TabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4676) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2fa364cd07c07f3d"></a>[`public float Inno.Native.ImGui.ImGuiWindow.TitleBarHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4656) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-341fc7fa1e5b59bc"></a>[`public Inno.Native.ImGui.ImGuiViewportP* Inno.Native.ImGui.ImGuiWindow.Viewport`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4630) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-31676bb86d1a2341"></a>[`public int Inno.Native.ImGui.ImGuiWindow.ViewportAllowPlatformMonitorExtend`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4636) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-79e2cf1c05a182bc"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.ViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4632) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5d4f11c3774e5666"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.ViewportOwned`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4702) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a8c3ab847a2e9383"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.ViewportPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4634) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d9a7e48953ef3d1b"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.WantCollapseToggle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4712) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-badb43cf9f04a94e"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.WasActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4706) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a57342312b30b3fc"></a>[`public float Inno.Native.ImGui.ImGuiWindow.WindowBorderSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4654) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c90be11f5b8e04bf"></a>[`public Inno.Native.ImGui.ImGuiWindowClass Inno.Native.ImGui.ImGuiWindow.WindowClass`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4628) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ab7d1f30abb6514"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindow.WindowPadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4650) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fa21ee1d01997f54"></a>[`public float Inno.Native.ImGui.ImGuiWindow.WindowRounding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4652) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-af3a8cd33c6cd6a4"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.WorkRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4788) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ecab06ebf2c36b64"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.WriteAccessed`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4708) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-927718411487e481"></a>[`public Inno.Native.ImGui.ImGuiWindow.ImGuiWindow(Inno.Native.ImGui.ImGuiContext* ctx = null, byte* name = null, uint iD = 0, Inno.Native.ImGui.ImGuiWindowFlags flags = Inno.Native.ImGui.ImGuiWindowFlags.None, Inno.Native.ImGui.ImGuiWindowFlags flagsPreviousFrame = Inno.Native.ImGui.ImGuiWindowFlags.None, Inno.Native.ImGui.ImGuiChildFlags childFlags = Inno.Native.ImGui.ImGuiChildFlags.None, Inno.Native.ImGui.ImGuiWindowClass windowClass = default(Inno.Native.ImGui.ImGuiWindowClass), Inno.Native.ImGui.ImGuiViewportP* viewport = null, uint viewportId = 0, System.Numerics.Vector2 viewportPos = default(System.Numerics.Vector2), int viewportAllowPlatformMonitorExtend = 0, System.Numerics.Vector2 pos = default(System.Numerics.Vector2), System.Numerics.Vector2 size = default(System.Numerics.Vector2), System.Numerics.Vector2 sizeFull = default(System.Numerics.Vector2), System.Numerics.Vector2 contentSize = default(System.Numerics.Vector2), System.Numerics.Vector2 contentSizeIdeal = default(System.Numerics.Vector2), System.Numerics.Vector2 contentSizeExplicit = default(System.Numerics.Vector2), System.Numerics.Vector2 windowPadding = default(System.Numerics.Vector2), float windowRounding = 0, float windowBorderSize = 0, float titleBarHeight = 0, float menuBarHeight = 0, float decoOuterSizeX1 = 0, float decoOuterSizeY1 = 0, float decoOuterSizeX2 = 0, float decoOuterSizeY2 = 0, float decoInnerSizeX1 = 0, float decoInnerSizeY1 = 0, int nameBufLen = 0, uint moveId = 0, uint tabId = 0, uint childId = 0, uint popupId = 0, System.Numerics.Vector2 scroll = default(System.Numerics.Vector2), System.Numerics.Vector2 scrollMax = default(System.Numerics.Vector2), System.Numerics.Vector2 scrollTarget = default(System.Numerics.Vector2), System.Numerics.Vector2 scrollTargetCenterRatio = default(System.Numerics.Vector2), System.Numerics.Vector2 scrollTargetEdgeSnapDist = default(System.Numerics.Vector2), System.Numerics.Vector2 scrollbarSizes = default(System.Numerics.Vector2), byte scrollbarX = 0, byte scrollbarY = 0, byte scrollbarXStabilizeEnabled = 0, byte scrollbarXStabilizeToggledHistory = 0, byte viewportOwned = 0, byte active = 0, byte wasActive = 0, byte writeAccessed = 0, byte collapsed = 0, byte wantCollapseToggle = 0, byte skipItems = 0, byte skipRefresh = 0, byte appearing = 0, byte hidden = 0, byte isFallbackWindow = 0, byte isExplicitChild = 0, byte hasCloseButton = 0, byte resizeBorderHovered = 0, byte resizeBorderHeld = 0, short beginCount = 0, short beginCountPreviousFrame = 0, short beginOrderWithinParent = 0, short beginOrderWithinContext = 0, short focusOrder = 0, Inno.Native.ImGui.ImGuiDir autoPosLastDirection = Inno.Native.ImGui.ImGuiDir.Left, byte autoFitFramesX = 0, byte autoFitFramesY = 0, byte autoFitOnlyGrows = 0, byte hiddenFramesCanSkipItems = 0, byte hiddenFramesCannotSkipItems = 0, byte hiddenFramesForRenderOnly = 0, byte disableInputsFrames = 0, Inno.Native.ImGui.ImGuiWindowBgClickFlags bgClickFlags = Inno.Native.ImGui.ImGuiWindowBgClickFlags.None, Inno.Native.ImGui.ImGuiCond setWindowPosAllowFlags = Inno.Native.ImGui.ImGuiCond.None, Inno.Native.ImGui.ImGuiCond setWindowSizeAllowFlags = Inno.Native.ImGui.ImGuiCond.None, Inno.Native.ImGui.ImGuiCond setWindowCollapsedAllowFlags = Inno.Native.ImGui.ImGuiCond.None, Inno.Native.ImGui.ImGuiCond setWindowDockAllowFlags = Inno.Native.ImGui.ImGuiCond.None, System.Numerics.Vector2 setWindowPosVal = default(System.Numerics.Vector2), System.Numerics.Vector2 setWindowPosPivot = default(System.Numerics.Vector2), Inno.Native.ImGui.ImVector<uint> iDStack = default(Inno.Native.ImGui.ImVector<uint>), Inno.Native.ImGui.ImGuiWindowTempData dC = default(Inno.Native.ImGui.ImGuiWindowTempData), Inno.Native.ImGui.ImRect outerRectClipped = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect innerRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect innerClipRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect workRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect parentWorkRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect clipRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect contentRegionRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImVec2Ih hitTestHoleSize = default(Inno.Native.ImGui.ImVec2Ih), Inno.Native.ImGui.ImVec2Ih hitTestHoleOffset = default(Inno.Native.ImGui.ImVec2Ih), int lastFrameActive = 0, int lastFrameJustFocused = 0, float lastTimeActive = 0, Inno.Native.ImGui.ImGuiStorage stateStorage = default(Inno.Native.ImGui.ImGuiStorage), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiOldColumns> columnsStorage = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiOldColumns>), float fontWindowScale = 0, float fontWindowScaleParents = 0, float fontRefSize = 0, int settingsOffset = 0, Inno.Native.ImGui.ImDrawList* drawList = null, Inno.Native.ImGui.ImDrawList drawListInst = default(Inno.Native.ImGui.ImDrawList), Inno.Native.ImGui.ImGuiWindow* parentWindow = null, Inno.Native.ImGui.ImGuiWindow* parentWindowInBeginStack = null, Inno.Native.ImGui.ImGuiWindow* rootWindow = null, Inno.Native.ImGui.ImGuiWindow* rootWindowPopupTree = null, Inno.Native.ImGui.ImGuiWindow* rootWindowDockTree = null, Inno.Native.ImGui.ImGuiWindow* rootWindowForTitleBarHighlight = null, Inno.Native.ImGui.ImGuiWindow* rootWindowForNav = null, Inno.Native.ImGui.ImGuiWindow* parentWindowForFocusRoute = null, Inno.Native.ImGui.ImGuiWindow* navLastChildNavWindow = null, uint* navLastIds = null, Inno.Native.ImGui.ImRect* navRectRel = null, System.Numerics.Vector2* navPreferredScoringPosRel = null, uint navRootFocusScopeId = 0, int memoryDrawListIdxCapacity = 0, int memoryDrawListVtxCapacity = 0, byte memoryCompacted = 0, byte dockIsActive = 0, byte dockNodeIsVisible = 0, byte dockTabIsVisible = 0, byte dockTabWantClose = 0, short dockOrder = 0, Inno.Native.ImGui.ImGuiWindowDockStyle dockStyle = default(Inno.Native.ImGui.ImGuiWindowDockStyle), Inno.Native.ImGui.ImGuiDockNode* dockNode = null, Inno.Native.ImGui.ImGuiDockNode* dockNodeAsHost = null, uint dockId = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4882) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f48c1a1bd937885e"></a>[`public void Inno.Native.ImGui.ImGuiWindow.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67445) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d2d5a10675c1101d"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.GetID(byte* str, byte* strEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67456) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-05170a1fd42e40cb"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.GetID(int n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67478) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-62067281c9f707d6"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.GetID(void* ptr)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67467) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5dd780779fafc375"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.GetIDFromPos(System.Numerics.Vector2 pAbs)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67489) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-77d3b7501f0ee002"></a>[`public uint Inno.Native.ImGui.ImGuiWindow.GetIDFromRectangle(Inno.Native.ImGui.ImRect rAbs)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67500) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d0ab6d3ffd2a7c38"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.MenuBarRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67533) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b05ad48994158253"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.Rect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67511) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c87372c3eb192527"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindow.TitleBarRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67522) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bef05f043070011a"></a>[`public Inno.Native.ImGui.ImGuiWindowBgClickFlags Inno.Native.ImGui.ImGuiWindow.BgClickFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4767) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d5cd23ebc1539dd0"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.DockIsActive { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4867) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-683494d1cca59fd8"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.DockNodeIsVisible { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4868) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-da70254893617eb0"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.DockTabIsVisible { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4869) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6810c312bb1300d7"></a>[`public byte Inno.Native.ImGui.ImGuiWindow.DockTabWantClose { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4870) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9030cf59cc5b3b84"></a>[`public System.Span<uint> Inno.Native.ImGui.ImGuiWindow.NavLastIds { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4843) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2c7dbd871cd4a049"></a>[`public System.Span<System.Numerics.Vector2> Inno.Native.ImGui.ImGuiWindow.NavPreferredScoringPosRel { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4855) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c9f589c3990eabf0"></a>[`public System.Span<Inno.Native.ImGui.ImRect> Inno.Native.ImGui.ImGuiWindow.NavRectRel { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4849) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-448fefc36fabf9cb"></a>[`public Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiWindow.SetWindowCollapsedAllowFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4770) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9967cb7557037eeb"></a>[`public Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiWindow.SetWindowDockAllowFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4771) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-557c18d07eae4a46"></a>[`public Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiWindow.SetWindowPosAllowFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4768) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6b766f08c839bbb4"></a>[`public Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiWindow.SetWindowSizeAllowFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4769) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0474454bedbfa9e7"></a>[`public struct Inno.Native.ImGui.ImGuiWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4613) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindowBgClickFlags</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindowBgClickFlags 9d944be1007cfb83f48d95415939c823ed5358c6edacae94c0d32aa9bae404d6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowBgClickFlags.None 0b94df2082d7e1164ad7b5d5cb853f06d8fac1e73c143c3b81eb2a0bf205253f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowBgClickFlags.Move b58f2f4a7de18048950436b68838df3c9eeee24ce1892590fe6ba2b7e6b967d2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-63388454d905add2"></a>[`Inno.Native.ImGui.ImGuiWindowBgClickFlags.Move = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1430) | 当前常量或枚举值：`1`。 | — |
| <a id="api-e5f27b533be215db"></a>[`Inno.Native.ImGui.ImGuiWindowBgClickFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1429) | 当前常量或枚举值：`0`。 | — |
| <a id="api-d4cd262adbf8953f"></a>[`public enum Inno.Native.ImGui.ImGuiWindowBgClickFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1427) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindowClass</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindowClass 315177cf51dddd01473638a7ed6a745db8a3dce11d168c0f9f1ec3b5bc48c32f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.ClassId 2705421d90d5f19f10f4da4460e94d5ce60533d02a3ffb0081c32cf5f79b7f13 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.ParentViewportId 8463b45b1b6ce1d78df198d863745c12b04f8ee643dab06fe6ebbffa66c544a5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.FocusRouteParentWindowId d8c870c44668af0ea034b1e6ef79f313f13abd266ca4f4798049bb0d421ef133 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.ViewportFlagsOverrideSet c362db6a3914a406e508e221600c983ff4f29666dc61341159fd9349cbd08602 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.ViewportFlagsOverrideClear c924ac1c218684d0cbf6a5568e182e28a8ec0948acc8d2dfdf66b8615c50689d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.TabItemFlagsOverrideSet b1b570bdf9c63255a7802183d86e4e4791a95d9ccc35033c63cac5afbeb5ed24 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.DockNodeFlagsOverrideSet 29adf1a8448b1b76fdbfb11d8bcea9126c39a7e4cdea480e5c84e53fc290921c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.DockingAlwaysTabBar 75e17b864dff77a261ea9f13a7f219b4659d0fa1f2964de6558416fa74fa4e89 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClass.DockingAllowUnclassed b47b61fd1f5b6d96185d8ecef1fec832ef167a8e043af70ea04ef6671600c567 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowClass.#ctor(System.UInt32,System.UInt32,System.UInt32,Inno.Native.ImGui.ImGuiViewportFlags,Inno.Native.ImGui.ImGuiViewportFlags,Inno.Native.ImGui.ImGuiTabItemFlags,Inno.Native.ImGui.ImGuiDockNodeFlags,System.Byte,System.Byte) 0a795ba271a5047215948ed8d6833086ba2881a1ac46204f593c3a79b843bb72 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowClass.Destroy 4afe88eddbd6f3a0bc043f9a50403fdf1fff92799fac15089ef89732982fd98b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e00a3775db996eb4"></a>[`public uint Inno.Native.ImGui.ImGuiWindowClass.ClassId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3917) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a9635a8709fba30f"></a>[`public byte Inno.Native.ImGui.ImGuiWindowClass.DockingAllowUnclassed`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3925) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc6512d01b5dbf7b"></a>[`public byte Inno.Native.ImGui.ImGuiWindowClass.DockingAlwaysTabBar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3924) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c2a5d15e5ab451bf"></a>[`public Inno.Native.ImGui.ImGuiDockNodeFlags Inno.Native.ImGui.ImGuiWindowClass.DockNodeFlagsOverrideSet`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3923) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ab0f4b9db11e33b"></a>[`public uint Inno.Native.ImGui.ImGuiWindowClass.FocusRouteParentWindowId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3919) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-456b86daa9dde79b"></a>[`public uint Inno.Native.ImGui.ImGuiWindowClass.ParentViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3918) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c66d8a9d5d1177e5"></a>[`public Inno.Native.ImGui.ImGuiTabItemFlags Inno.Native.ImGui.ImGuiWindowClass.TabItemFlagsOverrideSet`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3922) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc07e819aa715dc9"></a>[`public Inno.Native.ImGui.ImGuiViewportFlags Inno.Native.ImGui.ImGuiWindowClass.ViewportFlagsOverrideClear`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3921) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-12f2d544a46558d2"></a>[`public Inno.Native.ImGui.ImGuiViewportFlags Inno.Native.ImGui.ImGuiWindowClass.ViewportFlagsOverrideSet`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3920) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c586f6ab784a1291"></a>[`public Inno.Native.ImGui.ImGuiWindowClass.ImGuiWindowClass(uint classId = 0, uint parentViewportId = 0, uint focusRouteParentWindowId = 0, Inno.Native.ImGui.ImGuiViewportFlags viewportFlagsOverrideSet = Inno.Native.ImGui.ImGuiViewportFlags.None, Inno.Native.ImGui.ImGuiViewportFlags viewportFlagsOverrideClear = Inno.Native.ImGui.ImGuiViewportFlags.None, Inno.Native.ImGui.ImGuiTabItemFlags tabItemFlagsOverrideSet = Inno.Native.ImGui.ImGuiTabItemFlags.None, Inno.Native.ImGui.ImGuiDockNodeFlags dockNodeFlagsOverrideSet = Inno.Native.ImGui.ImGuiDockNodeFlags.None, byte dockingAlwaysTabBar = 0, byte dockingAllowUnclassed = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3926) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1441cfc75292c278"></a>[`public void Inno.Native.ImGui.ImGuiWindowClass.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63517) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3de4a1690f47b006"></a>[`public struct Inno.Native.ImGui.ImGuiWindowClass`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L3915) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
