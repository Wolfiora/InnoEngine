# Inno.Native.ImGui ABI · ImGuiWindowClassPtr → ImTextureRef

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindowClassPtr</code> · 23 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindowClassPtr 04b3176bfa8f9befbcccf820be736c3d6a65fb9212b68fc1fed8931b75d24587 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowClassPtr.#ctor(Inno.Native.ImGui.ImGuiWindowClass*) f0b26dc13fadd94a1313183987b758c007b42e8df86b20f3b3c0fc1ac538fd3a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowClassPtr.Handle 005b998edadee5ffccb0fddf5c937c27443108aa083cf64988a56cae7825656f -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowClassPtr.IsNull ffc5001f97afe44871e37dfe9817470cc805c9f83a421d3efce516db048f43e2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowClassPtr.Null de357f2f627e557898de24bb221a8a7177dbfd5d4e0fddb649af1032e446fe8b -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowClassPtr.Item(System.Int32) b52688a998bdc365e7eda446fa74cff8515dd4aff0338892668e6b998837d837 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowClassPtr.op_Implicit(Inno.Native.ImGui.ImGuiWindowClass*)~Inno.Native.ImGui.ImGuiWindowClassPtr ed00e33dad20c91c65054a2255ce11f08c9cce0f4d484bd7b75fbc04f0c80b1b -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowClassPtr.op_Implicit(Inno.Native.ImGui.ImGuiWindowClassPtr)~Inno.Native.ImGui.ImGuiWindowClass* 0cbf25cfea55a9f80d2ebdaef598d7ddd73cbbd409e50da6813eeebb5962f3bd -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowClassPtr.op_Equality(Inno.Native.ImGui.ImGuiWindowClassPtr,Inno.Native.ImGui.ImGuiWindowClassPtr) d6b0d980e37af5449c929bc7cf51ad60ba899e19770963376bef4316f03a445e -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowClassPtr.op_Inequality(Inno.Native.ImGui.ImGuiWindowClassPtr,Inno.Native.ImGui.ImGuiWindowClassPtr) e949fcb10584e2570d37110799134bfcde5177d4942e1600b4d019e98c3fd1d7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowClassPtr.Equals(Inno.Native.ImGui.ImGuiWindowClassPtr) 7a3c9be1c491283aebf52e73fc402958db5b89e9a620ac92713c514d569d3766 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowClassPtr.Equals(System.Object) dfc73de3ef0282026f587b11bee7caa98a0afdc649ae9b8f53c7d2f496cdd806 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowClassPtr.GetHashCode 46ee1b21d1daeb5da948565afd88bfd5ba2779f5585bb9daf658ceb220e86305 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowClassPtr.ClassId ababd039df4247a07e026353bfb977abeb043b9b5cd42b638c9b27d124166c34 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowClassPtr.ParentViewportId a19c960865dc4f7a2ecbc9c43917d4b07ef82208282e8163a5955111c9f2c044 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowClassPtr.FocusRouteParentWindowId b7fb1107d8065f107c8404d7e15cd1cceb55f3a598dd6037599a03a111ad73cc -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowClassPtr.ViewportFlagsOverrideSet 7e352a4b3921b2a4c67446df74c5c71e5299ac003dc353d5385441148621ec7e -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowClassPtr.ViewportFlagsOverrideClear 3427a270ac2bc8597f6fe9831a8bfd25a80309c0c05b0c810890d11efc587d38 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowClassPtr.TabItemFlagsOverrideSet a980d04910adfeafe3366bc03932edb3a58b110e6b9052e48b94ee53c317a9f7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowClassPtr.DockNodeFlagsOverrideSet a9497e01f84a5152530297c88be15b619b6639ad97231598cce0948630165383 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowClassPtr.DockingAlwaysTabBar 25ddd8ea83f3194a5d0a4d63f31be51bc2255aa4558ef4352e519f22951d942a -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowClassPtr.DockingAllowUnclassed f66448a4477a70dc6508ea0ff3622d27ecc6fcb61a8600a6621370a9a5126e38 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowClassPtr.Destroy f1fc205687143e54c5831733dbb2b4c79352add4572cd17d62b265c84ff76607 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6dc5b8e09a6f885d"></a>[`public Inno.Native.ImGui.ImGuiWindowClass* Inno.Native.ImGui.ImGuiWindowClassPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17882) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-463693e7cfb394da"></a>[`public Inno.Native.ImGui.ImGuiWindowClassPtr.ImGuiWindowClassPtr(Inno.Native.ImGui.ImGuiWindowClass* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17877) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-32f903a69663a2ab"></a>[`public void Inno.Native.ImGui.ImGuiWindowClassPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17909) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2deb83738f0434a5"></a>[`public bool Inno.Native.ImGui.ImGuiWindowClassPtr.Equals(Inno.Native.ImGui.ImGuiWindowClassPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17892) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-3a97451ff691f229"></a>[`public override bool Inno.Native.ImGui.ImGuiWindowClassPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17893) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-a683e30b71bb359a"></a>[`public override int Inno.Native.ImGui.ImGuiWindowClassPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17894) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-6d5fb5955a1a28d2"></a>[`public static bool Inno.Native.ImGui.ImGuiWindowClassPtr.operator ==(Inno.Native.ImGui.ImGuiWindowClassPtr left, Inno.Native.ImGui.ImGuiWindowClassPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17890) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-85dc89cc69851a83"></a>[`public static Inno.Native.ImGui.ImGuiWindowClassPtr.implicit operator Inno.Native.ImGui.ImGuiWindowClassPtr(Inno.Native.ImGui.ImGuiWindowClass* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17888) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f7c5e3b9cf11827b"></a>[`public static Inno.Native.ImGui.ImGuiWindowClassPtr.implicit operator Inno.Native.ImGui.ImGuiWindowClass*(Inno.Native.ImGui.ImGuiWindowClassPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17889) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3af843c8ba585590"></a>[`public static bool Inno.Native.ImGui.ImGuiWindowClassPtr.operator !=(Inno.Native.ImGui.ImGuiWindowClassPtr left, Inno.Native.ImGui.ImGuiWindowClassPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17891) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-97bd1e62f994a593"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowClassPtr.ClassId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17896) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5e2fe1c3bd005c4f"></a>[`public bool Inno.Native.ImGui.ImGuiWindowClassPtr.DockingAllowUnclassed { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17904) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fe3a5a391eb0babc"></a>[`public bool Inno.Native.ImGui.ImGuiWindowClassPtr.DockingAlwaysTabBar { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17903) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ebd8986680706a37"></a>[`public ref Inno.Native.ImGui.ImGuiDockNodeFlags Inno.Native.ImGui.ImGuiWindowClassPtr.DockNodeFlagsOverrideSet { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17902) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-27215914b0d4a567"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowClassPtr.FocusRouteParentWindowId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17898) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7228ae915b2df3b7"></a>[`public bool Inno.Native.ImGui.ImGuiWindowClassPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17883) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3b20cdfa33fcdc11"></a>[`public Inno.Native.ImGui.ImGuiWindowClass Inno.Native.ImGui.ImGuiWindowClassPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17886) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-99b507c3eaec7c00"></a>[`public static Inno.Native.ImGui.ImGuiWindowClassPtr Inno.Native.ImGui.ImGuiWindowClassPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17884) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a4aa299f42695650"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowClassPtr.ParentViewportId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17897) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4babeb95ba4f7a2e"></a>[`public ref Inno.Native.ImGui.ImGuiTabItemFlags Inno.Native.ImGui.ImGuiWindowClassPtr.TabItemFlagsOverrideSet { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17901) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7c6c8d0bf7cabf10"></a>[`public ref Inno.Native.ImGui.ImGuiViewportFlags Inno.Native.ImGui.ImGuiWindowClassPtr.ViewportFlagsOverrideClear { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17900) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1f79c0b8a47890a1"></a>[`public ref Inno.Native.ImGui.ImGuiViewportFlags Inno.Native.ImGui.ImGuiWindowClassPtr.ViewportFlagsOverrideSet { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17899) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7f641ce35b165772"></a>[`public struct Inno.Native.ImGui.ImGuiWindowClassPtr : System.IEquatable<Inno.Native.ImGui.ImGuiWindowClassPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17875) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindowDockStyle</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindowDockStyle 7e8c52f7c9f53d1c9262b3fed214d907cba52d41c42f30c073ba286b1f0f8e5a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_0 fc2c58f8c531f7d42cf4dad260bfdea0d3f11fed09560c8567eaded67bc72e4c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_1 77a9d173d1a75a1144624df6b082fb39ffac6873c32315ca4938344907b47d42 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_2 27c89176dadca5ee20f8d549b306d06b9fc73047daacf486a0c14c17ed384eb4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_3 ccd9bf99559b120a5619089ff66f146b39a9104d17f07937dd89807c37da6e86 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_4 bd5950fa4a05e67055408f823a0d879b9f97cbb0c7d173cb14a255099a3ad0a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_5 759e46795a73b548538561593bbd288b52128e3b3e63598473b29f9817b30702 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_6 78d5ed094267d0a4969402302eae29676f81ca24e82af9e992beda5c6e0f7d6f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_7 1f8683851cf9844f23226cba9738c3d708175cb3df9400be32ee9ab286faf596 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_8 3631e9532eec3986f10595ca0d2491e7f37a5143b965a3f8dc924aefaf109dce -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowDockStyle.Colors be9e3324ac3bd55e6adfb0904b632529be715f97f62b540f77588ca5f120682f -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowDockStyle.#ctor(System.UInt32*) 22940edeea7c353e95136cb755a878f2e2fc16f67a6f99ca1060ff57a0e7960f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f9bad95c88f2a7f0"></a>[`public uint Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4301) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ceb4090536955c9c"></a>[`public uint Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4302) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d80a703598705ca4"></a>[`public uint Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4303) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-343ba3fdc504f0d4"></a>[`public uint Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4304) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d20b69358c74d228"></a>[`public uint Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4305) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e6925ef457c8cb55"></a>[`public uint Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4306) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fdfa3d53a7b2fa43"></a>[`public uint Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4307) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6564097f2b1bf50a"></a>[`public uint Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4308) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ea48b39248beb77"></a>[`public uint Inno.Native.ImGui.ImGuiWindowDockStyle.Colors_8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4309) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1903109d4e9a33f7"></a>[`public Inno.Native.ImGui.ImGuiWindowDockStyle.ImGuiWindowDockStyle(uint* colors = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4312) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-919389e4c701749e"></a>[`public System.Span<uint> Inno.Native.ImGui.ImGuiWindowDockStyle.Colors { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4310) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bbf136dbe633d66c"></a>[`public struct Inno.Native.ImGui.ImGuiWindowDockStyle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4299) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindowDockStyleCol</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindowDockStyleCol 4ea5d8fccce289d7d2913b29abdad83b65c46c47e30d28b6d16714fddb7d7c97 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.Text 606ae666cc28ad0485e4fca59d3cbbb266ce9e40f400c6ae5427c8e2b48db5cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabHovered 948aca87e57dcbb22e730b0ede4e516e94dcfdc72cb9b372fef322f77a835107 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabFocused bea1a433abff591f8d00b009c37840f0a7c4e9edb2170319aefb1fb555a9b5ab -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabSelected 43b1d7c8210ce38aa4ce5c3c98e16805ac4797b59141a62975da2159a539a954 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabSelectedOverline 3eb0ad848d20fd65799e31e291ff6cb35c98daaeb141fd9556547e56dc908a91 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabDimmed 2068abba28caae5d047418b697bfc1dba46653a4f0d190845a2c90a5388c65f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabDimmedSelected adce32789152b389cdfc416d36ff2f14d749908091ba74a1916cf4697980f6f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabDimmedSelectedOverline b5455062bce4c46c9866a97c9455e8a5ef5a4b5b545fdc97b49b58842c3b3969 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.UnsavedMarker e6769b50a798e16592ce2a1e258369cfb1171fb4c5606b20d6dc1a15a90590eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowDockStyleCol.Count fd673b0175f15666f2f6f88415baa2089d2f22a246c28605fe5502d6c408efad -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fe41bd80698972fc"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.Count = 9`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1649) | 当前常量或枚举值：`9`。 | — |
| <a id="api-f3da4f316631940a"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabDimmed = 5`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1645) | 当前常量或枚举值：`5`。 | — |
| <a id="api-65d8489a8c7d5b2a"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabDimmedSelected = 6`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1646) | 当前常量或枚举值：`6`。 | — |
| <a id="api-1e085d208f4b4776"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabDimmedSelectedOverline = 7`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1647) | 当前常量或枚举值：`7`。 | — |
| <a id="api-87a2cd64758d7221"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabFocused = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1642) | 当前常量或枚举值：`2`。 | — |
| <a id="api-1994b402d7351cc8"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabHovered = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1641) | 当前常量或枚举值：`1`。 | — |
| <a id="api-fae55d2f8efc0fb4"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabSelected = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1643) | 当前常量或枚举值：`3`。 | — |
| <a id="api-bb1e4b5381d88f13"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.TabSelectedOverline = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1644) | 当前常量或枚举值：`4`。 | — |
| <a id="api-e9ef0b64d675a226"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.Text = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1640) | 当前常量或枚举值：`0`。 | — |
| <a id="api-dd95cc935822797b"></a>[`Inno.Native.ImGui.ImGuiWindowDockStyleCol.UnsavedMarker = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1648) | 当前常量或枚举值：`8`。 | — |
| <a id="api-5d36a138114a9b0e"></a>[`public enum Inno.Native.ImGui.ImGuiWindowDockStyleCol : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1638) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindowFlags</code> · 31 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindowFlags 37cb9e8353c4a43383eea2766d814c45858d71c3e89d86a4f78c2c361dd6f014 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.None f045c2c0e19f0e3b4e71fa297bcff1789f51ffb2dff263fd76ae439b62c13b59 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoTitleBar e0dc9d459797a0be3364d2747afd84c362fa604d0d7d7f55e47eaf0d638f644f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoResize 875640a55f410a23a7dd4bc63bb17ff5f7237775b5a078c8a83a84e565b8cd90 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoMove 816b6fa50a307965c431c2dc014e7d6e42a2a9b00ae4a06fb63100dedc3cd7b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoScrollbar 10c250fc04b011341bbb6416adc0706cc760633e976dfb01619dc5ff7515d6cf -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoScrollWithMouse 2310e2d408022f45b4953170c622d177457be5d1a143046ba759cb059d7776b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoCollapse ff42a7c3368a7a8432a08ed563cfd567452eb68d87367de1625cc472998bae49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.AlwaysAutoResize a3907901a65b84fe6bdce21dcb44adb5f5e976943a410ecd4da617d3ebefbac3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoBackground b93d1d1ff69f441f4cbb82d7d6461a6361e28cf6cd2cc2e11e41a300c58b217e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoSavedSettings c7899d39d5faf926416fe95018cda71782456a164d6eedd4bc64c468b07197fb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoMouseInputs 6ccaf9ce641d06410919a5d7d11adca4897fb375b00ee83d1c2e663f15584981 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.MenuBar e787c64d0e2ed51ff1aa7828ea01c6e2c1c8bf2f15923a5856c3df8353a3fe94 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.HorizontalScrollbar 5ece295352288becb9e870d024b779eb5b3beb3c99052a766c51e1d82a88d940 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoFocusOnAppearing dc728c3fee7d327becd62cd07f6dabd1dd75bb34bd2a47dd9f760a7920087d53 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoBringToFrontOnFocus decfaff1ce1d9d5ff94f64fdfec1ee7537a069ff9d13d866c90957a96a47be88 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.AlwaysVerticalScrollbar 240c0374fc0b4d9e1b8b3e576386954b78d555f051213f59ea6d0216524ec8b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.AlwaysHorizontalScrollbar 1ce26edebb4d8204c0630c62a0142383c43b62cf4fc04b6fe6f26ee6dd2e9a09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoNavInputs a73c4b075815c56809e337319ca38675e1b8c8dfdc97b9370198c56bfc38158c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoNavFocus 98e13b35afaea0ddcbff957859b64330ffe5a8d86e8242d2b35fb36bb3df6275 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.UnsavedDocument 8f533f8a810571e0e084c8fab755c1423964b4b611c90ca2a8db0f3e0180e8c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoDocking d216a660ee432da378f016ba8a8c97c3b3ce942f62fc4de23b2885d47c2899b7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoNav b7e53a431adb05ecbc3dfe2992174c52a2d327987e3b2577fa71cdaf8adc277a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoDecoration 8de2384363287e8f9caf950758516c5b76e4124a84f4778a1af9a60bd79dfabe -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.NoInputs 362684f79e03eb5f6c8e5562557332b152da38e53236ec96cc0c36fb1012ef5a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.DockNodeHost d69e26321c06c86a21fefaf978855fb4483b62d85a976a32856e9fad83e24599 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.ChildWindow 4dbef0793a95eca4be246e6abbdaf74e4a9c316cb7f9e2a5cff50cc5381e1660 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.Tooltip 565fd5e4f4b5cb44a281707e4c18f379c59b2c45e71fcd9bc6c7455f226c48ec -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.Popup 07224abf6a0dbd4a1328cf306a1a33eeefa67a9b212708446c354d7bc4c26c08 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.Modal 817154d4cb0f94db44b6cc8d4f0ca1cc28c81c0c7ba610de4b31afd73fab263d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowFlags.ChildMenu 16735d6c6519b8c7741e7780a280fd28622a7d04ed0583ecf70ee81c3db50bed -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3107eb2de586bf92"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.AlwaysAutoResize = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L388) | 当前常量或枚举值：`64`。 | — |
| <a id="api-c485c8d464a47a60"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.AlwaysHorizontalScrollbar = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L397) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-3e1277527edebc97"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.AlwaysVerticalScrollbar = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L396) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-216a8ac02eb0a56b"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.ChildMenu = 268435456`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L410) | 当前常量或枚举值：`268435456`。 | — |
| <a id="api-391cd2018009ac96"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.ChildWindow = 16777216`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L406) | 当前常量或枚举值：`16777216`。 | — |
| <a id="api-be65c06811b8053e"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.DockNodeHost = 8388608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L405) | 当前常量或枚举值：`8388608`。 | — |
| <a id="api-ca3d967357bf2d21"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.HorizontalScrollbar = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L393) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-0b9a068669520889"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.MenuBar = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L392) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-5a536eb923c8e4b5"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.Modal = 134217728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L409) | 当前常量或枚举值：`134217728`。 | — |
| <a id="api-c564f146f9b22f6d"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoBackground = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L389) | 当前常量或枚举值：`128`。 | — |
| <a id="api-39935cd83cadb4d5"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoBringToFrontOnFocus = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L395) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-b8eac89e2da3cc2a"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoCollapse = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L387) | 当前常量或枚举值：`32`。 | — |
| <a id="api-46d7f3310f721cf2"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoDecoration = 43`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L403) | 当前常量或枚举值：`43`。 | — |
| <a id="api-e2c1a8a3216fb433"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoDocking = 524288`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L401) | 当前常量或枚举值：`524288`。 | — |
| <a id="api-f1a242f418472cff"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoFocusOnAppearing = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L394) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-ae25992af2430683"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoInputs = 197120`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L404) | 当前常量或枚举值：`197120`。 | — |
| <a id="api-85248a96449eae75"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoMouseInputs = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L391) | 当前常量或枚举值：`512`。 | — |
| <a id="api-22d852c3cd0e6952"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoMove = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L384) | 当前常量或枚举值：`4`。 | — |
| <a id="api-5edb13ac6dcb743c"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoNav = 196608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L402) | 当前常量或枚举值：`196608`。 | — |
| <a id="api-1c64ab4f9b4856df"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoNavFocus = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L399) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-d95cfeba0469d6e6"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoNavInputs = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L398) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-aa798aa47da16614"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L381) | 当前常量或枚举值：`0`。 | — |
| <a id="api-959cb4fad057fcd6"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoResize = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L383) | 当前常量或枚举值：`2`。 | — |
| <a id="api-df39fed2562f2b6e"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoSavedSettings = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L390) | 当前常量或枚举值：`256`。 | — |
| <a id="api-3480dbbf9bab926a"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoScrollbar = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L385) | 当前常量或枚举值：`8`。 | — |
| <a id="api-5fde1189e545a0f1"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoScrollWithMouse = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L386) | 当前常量或枚举值：`16`。 | — |
| <a id="api-572d7124fa48f034"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.NoTitleBar = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L382) | 当前常量或枚举值：`1`。 | — |
| <a id="api-9591427aea941770"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.Popup = 67108864`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L408) | 当前常量或枚举值：`67108864`。 | — |
| <a id="api-1db323a27a405042"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.Tooltip = 33554432`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L407) | 当前常量或枚举值：`33554432`。 | — |
| <a id="api-0cdb780bfc563630"></a>[`Inno.Native.ImGui.ImGuiWindowFlags.UnsavedDocument = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L400) | 当前常量或枚举值：`262144`。 | — |
| <a id="api-746deaba0a518e8b"></a>[`public enum Inno.Native.ImGui.ImGuiWindowFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L379) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindowPtr</code> · 148 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindowPtr 9d698bdbca3f1354f041dc3854fc56743f1474a33e2e3b5eabe35f3d056a22d0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.#ctor(Inno.Native.ImGui.ImGuiWindow*) 410714c7da0b2c57cf3c00e6e2fdb5589ec7ea4896329c9dc23b786500b5984e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowPtr.Handle f39c036c0c931197ba3d6517960998537823899a1015ecb352d91d7e048e2301 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.IsNull 1da1015e5e215661b5454d9f76af25ab87420cf594cf4a59f70658fe9b732b67 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.Null 3839da9377c7139168c307d7eeeb5c11ecc8dece066985be2b3f102ee5d10bfc -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.Item(System.Int32) 3b1a770caad99faf1958d551d82960c33403745ed072fc907fd09d6a46eee160 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.op_Implicit(Inno.Native.ImGui.ImGuiWindow*)~Inno.Native.ImGui.ImGuiWindowPtr f05fca602b0aba6ee38c21c8749703c923e1275e100383d0125288962e576581 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.op_Implicit(Inno.Native.ImGui.ImGuiWindowPtr)~Inno.Native.ImGui.ImGuiWindow* 04057d8bb34a8f89e08c9b3d20de7b73dbd6a9379cceeba3c35e6360cd45aef1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.op_Equality(Inno.Native.ImGui.ImGuiWindowPtr,Inno.Native.ImGui.ImGuiWindowPtr) eed67be3989ebbac87dea3b29574f306e84150e10f722ad91ca81f771ee18922 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.op_Inequality(Inno.Native.ImGui.ImGuiWindowPtr,Inno.Native.ImGui.ImGuiWindowPtr) a527400728828ac501dc36336f5e1160a0278e37683f0417f12c548204fb9d20 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.Equals(Inno.Native.ImGui.ImGuiWindowPtr) 87d71b5e4cbe05c72ddf4cc0704f77c29f08c441a7dbe523ed6ee0914bf51bcb -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.Equals(System.Object) 495706b620cf8be668595e6ee2bc72fa2ce0a6ce2e9de34c27335c3e67aa8b31 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.GetHashCode c61a7dd823d406f09c99bd8d0a7cf8cdbe08a51139fd94ce0e8d0053d4db5727 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.Ctx f55861aee0d0a658d2a87befee7cdcc04ff70fb7fce130b2745f6d93d9106444 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.Name 92a13cf3efa8feedea70c0ba4b069b93b80fb452f30a832884929d3ac7bb5ba7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ID 7610857b52ccd8012d4c1462868267132479dd64bceb144c9db1b1b1c1c2ebf0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.Flags 382b3964d056f896ab8ea9c64b878b05b44fa9ed2f4de3cc6b94182467cd77de -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.FlagsPreviousFrame 9dcd01efc323a0267025e5197251326f71741a2a79acb14d0091dc112844d6ee -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ChildFlags 67c8cf16d5d9be4abbf28c28695f60eb7b086078f2eb4053121000b70023ea56 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.WindowClass 4e1be237fded0328c1bdaafd62f2ea6d376b279ce5f92597466140196ee4a765 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.Viewport 18e6708348409a58390cd1f8f3e90cb8360f1a257f0f7637b1b5fdd8336689ef -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ViewportId 74e542f81683ccb5c305f81b5d3e3376fcb5d931b5c34c6636f087bf885475d4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ViewportPos 58f0401a194aea0d725c0a2e54eb8e1c8d42adf66ba8341fec44c6f0cadc47a5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ViewportAllowPlatformMonitorExtend 34f1cf09a5c47c1cd64b7543607c9adc90926fdf48d1a0605a8d23bf8d386735 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.Pos 5bb7dbaa9a957acb99209b5bdbfb56e058bb3017e47d4cdc63287affd2b7fdd9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.Size 195d2e8e15b239daa06a59510c67234fde529b2a6fbde9df28e1d8e1fd553fcf -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.SizeFull 1abff090e4972b2452be1c4b982f3811fff59cd5e5478884dce5f6fd1ffd64d0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ContentSize f353e3ca0f2a9b7d0534553957c9c89bb59bedc97372f707b45c82b22fe4f68b -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ContentSizeIdeal 6aae53fa2e7360f6327ec4f0dec371a9b998f92499c5c9c79432126638465405 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ContentSizeExplicit 61b68daef4a2de44c4673c13c62ab56d92f54ee900a6eed3f6536809ab96a2da -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.WindowPadding facfd5909f9fda86f45b61523e0eaecb104a605c02f912dbbdec56891512a59c -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.WindowRounding e5f4c9947f1e15085e1626e17df6a37ffded13160c1fa915c6c13969100c8cec -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.WindowBorderSize d073c768af16ad325c2baef458754c4a40179827ff1f1aa55cf183c1fa3fa057 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.TitleBarHeight f60484a6820b4acb35a40d8c5de5a263049834fa7904da1097bc839cf5a2613e -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.MenuBarHeight f288b1b191fd80e8c2c34b74881ad3d26e7f703ef5a4fa0b4a3f81bcbca1115e -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DecoOuterSizeX1 f005dbd4ed74d15f39a85ab35db0b871c91743720384064ab0675c453fa8adef -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DecoOuterSizeY1 b44636494bbbc929307c4f8687bb4a266d3c49fe34f03cd48608a26925db7786 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DecoOuterSizeX2 dca19c674b818f6306ba1237bfce9ff8c17f39aca91baea7209fc189d5af1abe -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DecoOuterSizeY2 2b12fe92521eb74def742c1b91aff42e28005ea7b0a9477a831d93dadf930e42 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DecoInnerSizeX1 860bd2312e87cfa682594893deb7e9f6786651b144f6453c09f2560e75f9c089 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DecoInnerSizeY1 eca0d3047a40bccf2ff3ae8b4a0329888fd0d7ebd8747619ebea895ef280e188 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.NameBufLen 99b10e9fb696adc2cd8bb5f7bed82a3daed302b68e424a49347985ce873d17bc -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.MoveId d12872c8a49a6080453ae8cb4b90aec5d482662a36042d127e5899ac13b2d62f -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.TabId 945c5e1a1c985ace6be82c2ea70e8d94edd31a927dee4764cf699ec190d2a4dd -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ChildId 858e57fc193c17027bd47c2e703a6a81b06ff2b93ddd43ac4d9962d915515e47 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.PopupId a735abf533335463a7d570ec8c67e7c5bb561cbc7dce94a9311bded96a671968 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.Scroll bc3c0dcfd974ce8c331d4a0189a8cfd94859d5021c4e763dc8483660adac3cd7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ScrollMax 6d116e329d0b7f88fc0257dfe82d169b527991c77c7edbbf6d5aacd899d72be9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ScrollTarget 047dee62133116e4f072da55f7035846740682b32a6e78ccae09710cbb6a4ae4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ScrollTargetCenterRatio 5a692a553472d4d05b77d14def8aadb1e5da8967f8d3c6456e78bc496edaa157 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ScrollTargetEdgeSnapDist e5fded09d90a333c3ee879e5e2fe39ae8ba412e6ca3f4985713f1e733afc890d -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ScrollbarSizes 675c612b5cf01b093160733b463eb7d0db5790fd6bc0d2a31425f8c49d2bd57f -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ScrollbarX ec8a46f8998114aa7f3e06c4179afb4a406253b569415a455344aa1f541f698c -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ScrollbarY 301285f759a4e64824143134baf71b9dd5c442be4a49f957945d14b7fa9493f2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ScrollbarXStabilizeEnabled 9be295dd8b3541a31f52d0869855453c40d64d1d88a7630bd6e1046027ba5c21 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ScrollbarXStabilizeToggledHistory a434fa6f47bb5bfb0ce5ec297ce4e92940415667f121446986f82df264d17d26 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ViewportOwned 65979a39425e5917c7d9fb02be732b439d722a2f253c5968f29ce58c193c4a62 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.Active ccb573b14fc5c9cd130ccd1a4839c9799cef7c5867425cb33c90e18cd09f1c68 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.WasActive a43f80e9e0c830f83dbd15209015bb545f73d8b94a869ff6f69c27c5d5c73ddd -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.WriteAccessed 05cfd2db9fc46e90d70c3e3e9ad5fc74149ce293104dad2516fada1e98a519dc -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.Collapsed 3f4a6b0dcf62b5f1177e7b360bf0d47533c20c16b43166831d84a03c50b0160d -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.WantCollapseToggle f9cc1ce35e1358db03b22f4a678fdacbcbf0bb359fb018e11f247b1ce1e73b2f -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.SkipItems 53407b3d5f9d3463d980bacb0d6cf64ee5c975b0b6747093eeab2ccb43512475 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.SkipRefresh e24a2992fee40fe2236e1fd401b5921a04190a1ae4f9e401913c5441e94cb104 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.Appearing c92fa00c254ff5848fbf10e5034d2126d3c988d8641f4c47a516e6220969475d -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.Hidden fe949cad5eec40e20885e6733e0a02cddb4dcaa963d17cf131067d0d014c238e -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.IsFallbackWindow 3c51d7b8aa344e1b4f657bbc93841fd8265488365c9b1cbb6d03add6ddbef218 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.IsExplicitChild 9bbb1107a1e9c1ae8edf07cd2b0761c17254d1376913643a91b8b577a79b2182 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.HasCloseButton 430a77ff34a44d910138a4cd657c3a8fea0258c0c8d3bd051f5af8767c1a1511 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ResizeBorderHovered 8a082494e3419d31e1c10be99f5baf05792729ec88d2fb972081ff8230d7729b -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ResizeBorderHeld 3bf9c7e730e29479a913cfc7e2461b0075d5c84f21772a85e02b1f0c87014f3b -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.BeginCount 5c1e04882d98b048129bdd68bcf2f1d7f8a24fdcc61d5487f9770309f134e8ef -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.BeginCountPreviousFrame 7349141b2f0c1a80fd927588d17660b58e17d10cd13cb7318d1e84bbb6d698c9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.BeginOrderWithinParent 3e115ca55d31368fd1f2a2bfd87f8ef755ae8b89338c6699a30dc9529b7c1391 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.BeginOrderWithinContext 1e4522fa012f96122479c2ad0fbfa3c27cff03eb0b194865311bff590d9f5879 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.FocusOrder 6bc2c1d1b79f85b6873ff39a879dd85f3de5173a43fa46799a8a54ece24d3e1a -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.AutoPosLastDirection 0a52e543e034812c4006559ec3a522b5d9b3fa658e037b75979395885b2565fb -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.AutoFitFramesX 5fda51b6eae9ec28ae6174308c49f8975831c7cd88473aa773fdbd08c510f4c8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.AutoFitFramesY a5125131103a5972c4fad6c82e46123f6a2f6fafa6543a00656f2e3e8a71b4a6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.AutoFitOnlyGrows a97ea8a6802de046fa9d5701fe06d9efc249d7c85cabdbd1780cb087889847a5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.HiddenFramesCanSkipItems 1489e9ebb7e4474e38ee1765bf2b99dd33db753228be2b70d31398ea9eb4c518 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.HiddenFramesCannotSkipItems f59f2788240b8d20efa146979104e191bc110c753c0231d9e2449465ca82b8e8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.HiddenFramesForRenderOnly e4720e25ce576fcc165d7043584f346f2409f3fe1338e2a291a4436d79b8a744 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DisableInputsFrames 89ec9059f68d1757446a675d5d17b5302260a6f1cfb7a09622817d55a775d5e0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.BgClickFlags 7cf0e21cec1ae2656c9cdda1662c1afb52fa1ba94cc39ef961c4d719a05c9f5a -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.SetWindowPosAllowFlags be77be0899b513ac6caa3c7140fe97e187ca9909a2b25884e4bce5848f56961e -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.SetWindowSizeAllowFlags 2f06259a018a3c9c41fb8b58b2173368ddac57e9873b19909d0c329a1759f818 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.SetWindowCollapsedAllowFlags 71072b5c59bc8c27fa65a66264038e7bda611db8b1855aafa2315cb3cc9909a2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.SetWindowDockAllowFlags 687df2b91937d6218d4748e51df0da792c3cdd88387df3684a51b11054dd0d3e -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.SetWindowPosVal 7dc32e5241a956647ea0dee27fa8f5fb8842c38b2b7486eec667bba0f35a1a2b -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.SetWindowPosPivot cf3396b5287d47af283f309b976be3deb90059c97e90f810af984175a5f59360 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.IDStack 2ef36a02e3e5b554aac69437cf36fd5780112f5600bdedfdf86638fbbd535394 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DC e8c05b60e26e602eac883e51eb79148ae116f02a65e02abad9ccc5b709a0dc28 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.OuterRectClipped 5993f20b63df1df0af2eee707848f17a5228fea02c1258150e0d8bac12415b20 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.InnerRect ab73162d67bf5a4668474b3e58774defd1f53dc421e19bab71227c55a261fdec -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.InnerClipRect 9f5b9dd1d0d9aa127993cb3ac460b8f9edc1ad9fa1be0d9f686b0cd88818f4e8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.WorkRect 423f6577b52d7e2f508f0c2a882cd78c489a1251e14601a8a685918af10ca443 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ParentWorkRect 461ea9a375d09d7570e5dcc3256e216ffc6631bb4e60f8dbfec0b817c1834108 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ClipRect bd7814bd1fa05275b08c2d001334e510d814250de8a910927615245fd7dd1de6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ContentRegionRect 54b313fe99a2aa6ef797ee226f7c6ab9481c4a98dc33ff97d529fa2f0224c3b6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.HitTestHoleSize ebb830aa382cd3cb804449bc5cb16ed2de495ed46e467b7545f469bb70304801 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.HitTestHoleOffset 999168d6a601fe377f11bb484b1d319c42f2b57a37727fee5eac89d2d1a28e0c -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.LastFrameActive 21f5258344ae9d515d33d33170e4641c4b5617b71a6a5903a1e7078368ef6620 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.LastFrameJustFocused 872b1f8854bde3b4e45d67de5e71c9395f6902e5ba211a3b5ee956048855140a -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.LastTimeActive 281a50e24d68f63b041098d8d6f05210263556345003ae986777da1fb9056549 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.StateStorage 87083c51888e2b81fae5a134cc6efa29e303834f8829e7075a081b91f1cb8e76 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ColumnsStorage af789bf0105a624ef377143579f3d9663463ff1c364a26330088347be721dc6e -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.FontWindowScale 29dec42dfeca1ef587207233dfffb452cd97b81a8c7cc60ac7e4d01722f6fb3c -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.FontWindowScaleParents 71b236bcfbad51dc1385b3809b3912fcb3d3b8c0fb17bd0e2dfca42bccecff93 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.FontRefSize fc5b1bcd0066afd0247bd898521d5f7c1bc3481dde25558d57bfe29e3c7a2d07 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.SettingsOffset be3c91205a294e2ed223f00aca7579d2a2da707c5e92d4f5c333c90b8deec65e -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DrawList 37ac779b1188a34c10996b18355d897cb1786951000ed914a81cf909106b56c5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DrawListInst 03b7f84add031022d0071c0633ede871cc5ef9c79408dba8b9e3f2444ffeaecb -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ParentWindow c968b601857fec3354ba62d2403517f47061872ef5e4e49032eabf5030a8421d -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ParentWindowInBeginStack e27129296206b7c05ec05dcebd97b5996a869f2a583c1c515bc72834261914ee -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.RootWindow c89e7148d617a77373d216b28f55f917a992bdb16466d939760b5d991d004f08 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.RootWindowPopupTree 4df5af652839acdecd5725d310fdda55a788547590a14fd4838f3badddbc3c55 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.RootWindowDockTree 8702a5163dba5ae6a74449fe9497a76633a0e5e68bddaf81a7e6b20772970be1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.RootWindowForTitleBarHighlight fe3b419b48e84fab353f8c8bbf7e712020fc34c2a79d83af3a3a6ab9d7f86382 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.RootWindowForNav 0038f164d58d00adfd8989fd82607b557c49c6c6e6dd63a781355f628f5ac160 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.ParentWindowForFocusRoute fb58cde73e70fcc9890c1a20f136b1c83eddef267beac3ba76ea6a41981ccc81 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.NavLastChildNavWindow 4ab3198257a1f55ddcdfc7947222b7bd1a5cc77aed245458b997bd6e307fa0ec -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.NavLastIds ec9a93654b2fc721463ef9b4dfcd84ff6550e379fd6149b7dab0e78f2efa7e0d -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.NavRectRel 9fccc74e6fabf22717b0ef2933fb45cdba0d6ca5fd280bb23b0a573f5fa38afc -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.NavPreferredScoringPosRel dcead687bd7c7d021aa5a41a9d6bb6595978cc56cc478d9f23219d2eb762d707 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.NavRootFocusScopeId 76fb17239e02a421c7ef785c19de392a6d056169da2a61f693470f7dd5dceb52 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.MemoryDrawListIdxCapacity 0b9c3295139ee590c02a7802a632666911cb4b3a3292785d9e3fe04cd2aa4594 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.MemoryDrawListVtxCapacity 4c895cf5a451be98d84c58f9552827594992f5d5a829cc33738316e50772444f -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.MemoryCompacted 3556393874d89a9ec2ed4c2d0bd1d68cc0bd30f4f0a2917d6fa266ce8c9eb451 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DockIsActive b129b87aae4bd418836cb910c790a0b9a0e62233b33552a84042472a649d66eb -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DockNodeIsVisible f7cbf2ab2b2188a3308a7f7812e1c27b1026589738e8676aaa5aa76ad7047a1c -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DockTabIsVisible 81b6fa135dfc0eef926a39196f555fac67bbddb1b22d184bd58ae9addbbc04b7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DockTabWantClose cbf7bf9c41dd1b44d8d3c308caf61369630e3875a3b9a091f69dd7f4ffe400a8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DockOrder b2324258fed8971bffcd702648f8f187f53952a27b2deda3452a34f89ac36332 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DockStyle 610f1e47a6fe2efe2151003956139f8846d07b69e1383e086e4f13cd47311f17 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DockNode 8cad74e62164c8a081d306cc65a92bced7abd82add00ba8cc05a6ed5c0ea38e6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DockNodeAsHost 1538f672358271ac35579239d591a7209a5f7b0a02ed126b06b539f467d35805 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtr.DockId 344221778c78b0ad5e6b8737caca91186380707cd149ab49416caa4b8360a6b0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.Destroy 1750708dd1a03bd48b9b6aacdb458edc8a0d3c5df4101f56c508e22463c77ed1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.GetID(System.String,System.String) c7e76091e09a69513b1fa743a038747b2109ea6c4a7a1a6a0b78864a0a154ea7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.GetID(System.String) 0b5a5e6a3a744fefe28205abb3769e27fdcdcd2eb3c7c504bf9d4b5ac4f1e3c9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.GetID(System.Void*) a4dff3e1a07bd7163ba3bd5a1974f726c726f46a3214403e8b8b809db35496f6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.GetID(System.Int32) 45e74ed94a621533ea191f231adb0a5b69c1293e02e738df2122f68b12d277e6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.GetIDFromPos(System.Numerics.Vector2) 30404f3532b9e117acf56d42a1d8d8abc1990abdad622f674037434c9b8edb6f -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.GetIDFromRectangle(Inno.Native.ImGui.ImRect) 81d36e798ef31ac37228c1ec7e474f610343074a1e7921bfe1826d2564790ede -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.Rect b8fa780bdba7a53c144947666cd74c6b484a0f5b70571c7c875257e4ad85caf0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.TitleBarRect 1a5ad250aac1e415642837fb872448946da6473263aa56bbed730769adea5163 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtr.MenuBarRect 87ed3f564a394ef85899cb15191e5ea6abe1882e1c6be8d9838f43693473f5a1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7ea2e0a51d8ec0c3"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindowPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17923) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-617e076c3be8ede6"></a>[`public Inno.Native.ImGui.ImGuiWindowPtr.ImGuiWindowPtr(Inno.Native.ImGui.ImGuiWindow* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17918) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0469169e7e21374c"></a>[`public void Inno.Native.ImGui.ImGuiWindowPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18066) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-11b640be4c393650"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.Equals(Inno.Native.ImGui.ImGuiWindowPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17933) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-055f5aacdead08c3"></a>[`public override bool Inno.Native.ImGui.ImGuiWindowPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17934) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-970025f9e47a4a06"></a>[`public override int Inno.Native.ImGui.ImGuiWindowPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17935) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-19b809cf19ddec1b"></a>[`public uint Inno.Native.ImGui.ImGuiWindowPtr.GetID(int n)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18098) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-540fe930046fa0bd"></a>[`public uint Inno.Native.ImGui.ImGuiWindowPtr.GetID(string str, string? strEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18074) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8c44c6003733abb9"></a>[`public uint Inno.Native.ImGui.ImGuiWindowPtr.GetID(string str)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18082) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b1c6c73c32a6ce6a"></a>[`public uint Inno.Native.ImGui.ImGuiWindowPtr.GetID(void* ptr)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18090) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5219f5be285c245b"></a>[`public uint Inno.Native.ImGui.ImGuiWindowPtr.GetIDFromPos(System.Numerics.Vector2 pAbs)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18106) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d702cee16f7d68f4"></a>[`public uint Inno.Native.ImGui.ImGuiWindowPtr.GetIDFromRectangle(Inno.Native.ImGui.ImRect rAbs)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18114) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e77537cc41e4d8f"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindowPtr.MenuBarRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18138) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4a8d425c8d495760"></a>[`public static bool Inno.Native.ImGui.ImGuiWindowPtr.operator ==(Inno.Native.ImGui.ImGuiWindowPtr left, Inno.Native.ImGui.ImGuiWindowPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17931) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6a5e156285dae68d"></a>[`public static Inno.Native.ImGui.ImGuiWindowPtr.implicit operator Inno.Native.ImGui.ImGuiWindowPtr(Inno.Native.ImGui.ImGuiWindow* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17929) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9d98c1784417b13f"></a>[`public static Inno.Native.ImGui.ImGuiWindowPtr.implicit operator Inno.Native.ImGui.ImGuiWindow*(Inno.Native.ImGui.ImGuiWindowPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17930) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2ecc644d57ae3b53"></a>[`public static bool Inno.Native.ImGui.ImGuiWindowPtr.operator !=(Inno.Native.ImGui.ImGuiWindowPtr left, Inno.Native.ImGui.ImGuiWindowPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17932) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9f68869a8e7bb499"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindowPtr.Rect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18122) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cc0f257039e9585f"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindowPtr.TitleBarRect()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18130) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ecbbc8cc0cd337d0"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.Active { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17981) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3bd8b844ce7e6851"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.Appearing { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17988) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dd81d7dc6165d17a"></a>[`public ref byte Inno.Native.ImGui.ImGuiWindowPtr.AutoFitFramesX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18001) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-41e3c29e360c15a4"></a>[`public ref byte Inno.Native.ImGui.ImGuiWindowPtr.AutoFitFramesY { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18002) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9890c89a45a00608"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.AutoFitOnlyGrows { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18003) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1bddc417688735f4"></a>[`public ref Inno.Native.ImGui.ImGuiDir Inno.Native.ImGui.ImGuiWindowPtr.AutoPosLastDirection { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18000) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-43ea9aea6f4c9210"></a>[`public ref short Inno.Native.ImGui.ImGuiWindowPtr.BeginCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17995) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7a15be9a19e36ca2"></a>[`public ref short Inno.Native.ImGui.ImGuiWindowPtr.BeginCountPreviousFrame { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17996) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2327d5d9e32ccbc1"></a>[`public ref short Inno.Native.ImGui.ImGuiWindowPtr.BeginOrderWithinContext { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17998) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fd242b37fd6db325"></a>[`public ref short Inno.Native.ImGui.ImGuiWindowPtr.BeginOrderWithinParent { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17997) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dbc0a8523e22f3fa"></a>[`public Inno.Native.ImGui.ImGuiWindowBgClickFlags Inno.Native.ImGui.ImGuiWindowPtr.BgClickFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18008) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-184bbd4213b2cee3"></a>[`public ref Inno.Native.ImGui.ImGuiChildFlags Inno.Native.ImGui.ImGuiWindowPtr.ChildFlags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17942) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cafbf759df7bebe2"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowPtr.ChildId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17968) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0a08d5bea952776c"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindowPtr.ClipRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18022) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba18087cac05a489"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.Collapsed { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17984) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4127d2cdb9144c68"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiOldColumns> Inno.Native.ImGui.ImGuiWindowPtr.ColumnsStorage { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18030) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1b17b5e7c81b6edb"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindowPtr.ContentRegionRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18023) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f42010d71f4a61e2"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.ContentSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17951) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2204426c3535f5bf"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.ContentSizeExplicit { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17953) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a34d41546f100c0f"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.ContentSizeIdeal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17952) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cb3058095fe1abb5"></a>[`public ref Inno.Native.ImGui.ImGuiContextPtr Inno.Native.ImGui.ImGuiWindowPtr.Ctx { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17937) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f485208c14ae67ba"></a>[`public ref Inno.Native.ImGui.ImGuiWindowTempData Inno.Native.ImGui.ImGuiWindowPtr.DC { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18016) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cc72c77e1d19c5f8"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.DecoInnerSizeX1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17963) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-972a6b02fba19337"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.DecoInnerSizeY1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17964) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f43a8b23831e6c80"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.DecoOuterSizeX1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17959) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b784f80174db36bf"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.DecoOuterSizeX2 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17961) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3c1909dcdab9fd5c"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.DecoOuterSizeY1 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17960) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-23859298204092a8"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.DecoOuterSizeY2 { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17962) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6b9860fae959fa98"></a>[`public ref byte Inno.Native.ImGui.ImGuiWindowPtr.DisableInputsFrames { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18007) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba7ff2e2045b9528"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowPtr.DockId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18061) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-de3bfd4f5d3851c9"></a>[`public byte Inno.Native.ImGui.ImGuiWindowPtr.DockIsActive { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18053) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e5b945ef646c00a5"></a>[`public ref Inno.Native.ImGui.ImGuiDockNodePtr Inno.Native.ImGui.ImGuiWindowPtr.DockNode { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18059) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-86718ce251f3debc"></a>[`public ref Inno.Native.ImGui.ImGuiDockNodePtr Inno.Native.ImGui.ImGuiWindowPtr.DockNodeAsHost { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18060) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f4d159dec0392b0b"></a>[`public byte Inno.Native.ImGui.ImGuiWindowPtr.DockNodeIsVisible { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18054) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2690bcd4b083ca30"></a>[`public ref short Inno.Native.ImGui.ImGuiWindowPtr.DockOrder { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18057) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aee42a1cda0a173e"></a>[`public ref Inno.Native.ImGui.ImGuiWindowDockStyle Inno.Native.ImGui.ImGuiWindowPtr.DockStyle { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18058) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e2ec933f641a366a"></a>[`public byte Inno.Native.ImGui.ImGuiWindowPtr.DockTabIsVisible { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18055) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-776e76e8c2c1d868"></a>[`public byte Inno.Native.ImGui.ImGuiWindowPtr.DockTabWantClose { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18056) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-71a87829afce9056"></a>[`public ref Inno.Native.ImGui.ImDrawListPtr Inno.Native.ImGui.ImGuiWindowPtr.DrawList { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18035) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c639b1fdebf763d9"></a>[`public ref Inno.Native.ImGui.ImDrawList Inno.Native.ImGui.ImGuiWindowPtr.DrawListInst { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18036) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5390af7988adb264"></a>[`public ref Inno.Native.ImGui.ImGuiWindowFlags Inno.Native.ImGui.ImGuiWindowPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17940) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ff7e9629382abbf3"></a>[`public ref Inno.Native.ImGui.ImGuiWindowFlags Inno.Native.ImGui.ImGuiWindowPtr.FlagsPreviousFrame { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17941) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5de5ff20af58de63"></a>[`public ref short Inno.Native.ImGui.ImGuiWindowPtr.FocusOrder { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17999) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a8941fe433562595"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.FontRefSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18033) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2555ed02fada94ae"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.FontWindowScale { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18031) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aa08961dbb456eb6"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.FontWindowScaleParents { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18032) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-686723b86226dfea"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.HasCloseButton { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17992) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a7cc8657d15f1083"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.Hidden { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17989) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b42beff148bd5961"></a>[`public ref byte Inno.Native.ImGui.ImGuiWindowPtr.HiddenFramesCannotSkipItems { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18005) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9ad7ac854f9d4750"></a>[`public ref byte Inno.Native.ImGui.ImGuiWindowPtr.HiddenFramesCanSkipItems { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18004) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-27ac9733f6837410"></a>[`public ref byte Inno.Native.ImGui.ImGuiWindowPtr.HiddenFramesForRenderOnly { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18006) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dcfff12e6fda3dca"></a>[`public ref Inno.Native.ImGui.ImVec2Ih Inno.Native.ImGui.ImGuiWindowPtr.HitTestHoleOffset { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18025) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-08c4076b8bd539e6"></a>[`public ref Inno.Native.ImGui.ImVec2Ih Inno.Native.ImGui.ImGuiWindowPtr.HitTestHoleSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18024) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cb923e47bfa68011"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowPtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17939) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-81af633b57cb07d9"></a>[`public ref Inno.Native.ImGui.ImVector<uint> Inno.Native.ImGui.ImGuiWindowPtr.IDStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18015) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-67256a3fd1a1c86b"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindowPtr.InnerClipRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18019) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6453edcd6bb2d737"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindowPtr.InnerRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18018) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7635b9247d8da18d"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.IsExplicitChild { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17991) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e3e1d420b3514668"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.IsFallbackWindow { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17990) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-730573627ae7eb69"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17924) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8e9676caa0b5e05e"></a>[`public Inno.Native.ImGui.ImGuiWindow Inno.Native.ImGui.ImGuiWindowPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17927) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e3f9a3c38053aa45"></a>[`public ref int Inno.Native.ImGui.ImGuiWindowPtr.LastFrameActive { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18026) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-df182774298656e1"></a>[`public ref int Inno.Native.ImGui.ImGuiWindowPtr.LastFrameJustFocused { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18027) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c8b522f575f87bc4"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.LastTimeActive { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18028) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cf0e5e8b1e386023"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.MemoryCompacted { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18052) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6565def66164d564"></a>[`public ref int Inno.Native.ImGui.ImGuiWindowPtr.MemoryDrawListIdxCapacity { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18050) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-af7d991bc0053858"></a>[`public ref int Inno.Native.ImGui.ImGuiWindowPtr.MemoryDrawListVtxCapacity { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18051) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6e75c3467c72d9d5"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.MenuBarHeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17958) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-55d2baab9ed5cc3d"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowPtr.MoveId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17966) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-772ccaf39bc4504a"></a>[`public byte* Inno.Native.ImGui.ImGuiWindowPtr.Name { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17938) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-61c3ce9dcec52c3d"></a>[`public ref int Inno.Native.ImGui.ImGuiWindowPtr.NameBufLen { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17965) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-820449b99e3c2801"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiWindowPtr.NavLastChildNavWindow { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18045) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f1dbae3f7d9c3d2b"></a>[`public System.Span<uint> Inno.Native.ImGui.ImGuiWindowPtr.NavLastIds { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18046) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9f71053407b5a092"></a>[`public System.Span<System.Numerics.Vector2> Inno.Native.ImGui.ImGuiWindowPtr.NavPreferredScoringPosRel { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18048) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cadb2683bade284c"></a>[`public System.Span<Inno.Native.ImGui.ImRect> Inno.Native.ImGui.ImGuiWindowPtr.NavRectRel { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18047) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8dafb513e0677554"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowPtr.NavRootFocusScopeId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18049) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-12812158f2b28340"></a>[`public static Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiWindowPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17925) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4504fee68e293e6d"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindowPtr.OuterRectClipped { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18017) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5da347e78b82c130"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiWindowPtr.ParentWindow { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18037) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aec2a5bc69201b4f"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiWindowPtr.ParentWindowForFocusRoute { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18044) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8384ff0608e66f1c"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiWindowPtr.ParentWindowInBeginStack { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18038) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-788066d7efc8b683"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindowPtr.ParentWorkRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18021) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-93098d6f3a0fdf06"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowPtr.PopupId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17969) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6b5076ddf2394572"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.Pos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17948) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c82624d5202c8e41"></a>[`public ref byte Inno.Native.ImGui.ImGuiWindowPtr.ResizeBorderHeld { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17994) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8bb390de45927880"></a>[`public ref byte Inno.Native.ImGui.ImGuiWindowPtr.ResizeBorderHovered { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17993) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-098357341995e81a"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiWindowPtr.RootWindow { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18039) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cb97d33c64666bf4"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiWindowPtr.RootWindowDockTree { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18041) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4f62518ee4ba5ba1"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiWindowPtr.RootWindowForNav { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18043) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8bc693a81cd26635"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiWindowPtr.RootWindowForTitleBarHighlight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18042) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4fcdffb340f20274"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiWindowPtr.RootWindowPopupTree { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18040) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d2f105f8339a52e6"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.Scroll { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17970) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1fa2f4da8ef61500"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.ScrollbarSizes { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17975) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-33c61a944eb358a4"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.ScrollbarX { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17976) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e05493a49de92eef"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.ScrollbarXStabilizeEnabled { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17978) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c36039defeb28169"></a>[`public ref byte Inno.Native.ImGui.ImGuiWindowPtr.ScrollbarXStabilizeToggledHistory { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17979) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a78944dbbeeb882f"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.ScrollbarY { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17977) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-36419ab767e74f5d"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.ScrollMax { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17971) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d7165b7a43dd8d52"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.ScrollTarget { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17972) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e90d66c7bfac56ec"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.ScrollTargetCenterRatio { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17973) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0abd1b85d9a411ec"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.ScrollTargetEdgeSnapDist { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17974) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fb53bb74c5871dbb"></a>[`public ref int Inno.Native.ImGui.ImGuiWindowPtr.SettingsOffset { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18034) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d3a687be51a12bdd"></a>[`public Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiWindowPtr.SetWindowCollapsedAllowFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18011) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9a8978240603c8e3"></a>[`public Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiWindowPtr.SetWindowDockAllowFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18012) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e605d60670ac6783"></a>[`public Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiWindowPtr.SetWindowPosAllowFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18009) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c0cff0831468c871"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.SetWindowPosPivot { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18014) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a0a902ee2bf02a15"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.SetWindowPosVal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18013) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-711c2ab33e03ece1"></a>[`public Inno.Native.ImGui.ImGuiCond Inno.Native.ImGui.ImGuiWindowPtr.SetWindowSizeAllowFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18010) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-efcd3de3d8fba8c5"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.Size { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17949) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0d0a839ca43ba929"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.SizeFull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17950) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5875a686435bfa5d"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.SkipItems { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17986) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9cb1a7635b7a90a1"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.SkipRefresh { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17987) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e4a0a69aaa2b7dd7"></a>[`public ref Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImGuiWindowPtr.StateStorage { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18029) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2d8b459cd3bb5130"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowPtr.TabId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17967) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7c8b36d0a7020443"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.TitleBarHeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17957) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a06bf522832e33c4"></a>[`public ref Inno.Native.ImGui.ImGuiViewportPPtr Inno.Native.ImGui.ImGuiWindowPtr.Viewport { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17944) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-75bd753a6a5c719b"></a>[`public ref int Inno.Native.ImGui.ImGuiWindowPtr.ViewportAllowPlatformMonitorExtend { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17947) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f6e0d4f516e05ac0"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowPtr.ViewportId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17945) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-48adf4980003f5aa"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.ViewportOwned { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17980) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-db894c8c78af449a"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.ViewportPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17946) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e60ede6b9da6c818"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.WantCollapseToggle { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17985) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-19605ffe4387d518"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.WasActive { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17982) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-96db47d95caeec7f"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.WindowBorderSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17956) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-39aedc00a327f4ef"></a>[`public ref Inno.Native.ImGui.ImGuiWindowClass Inno.Native.ImGui.ImGuiWindowPtr.WindowClass { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17943) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-202ec154320c863c"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowPtr.WindowPadding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17954) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a030d3275f0e4f42"></a>[`public ref float Inno.Native.ImGui.ImGuiWindowPtr.WindowRounding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17955) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9fa7fd0a8a20466f"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindowPtr.WorkRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18020) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-84fa0a325b3fa3a4"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtr.WriteAccessed { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17983) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-259af42cd9f155dc"></a>[`public struct Inno.Native.ImGui.ImGuiWindowPtr : System.IEquatable<Inno.Native.ImGui.ImGuiWindowPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L17916) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindowPtrPtr</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindowPtrPtr c44b8d36b632a63c2d916f15c041a6cb3213d6dddc1dceaf8546614222260a3f -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtrPtr.#ctor(Inno.Native.ImGui.ImGuiWindow**) a3cbb41ebd58ed4b632a57ba4491b92f95d78dad8dc3fb1d82e130dcc650ba74 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowPtrPtr.Handle 25660585181c6c402c9b4dff241dc6505df74a747e88190c2f0f8a9e47e04659 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtrPtr.IsNull fc0e8b76313af0acf5be06b88f7e7af2270264751a36e4ed018103c037ae9454 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtrPtr.Null 99f16b7223e8e664b2c00531d7a8503f3a736a6f3ba5def0694b227dc04d01de -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowPtrPtr.Item(System.Int32) 4a012c88162fa4bb9e3b425e534b04ee0a04d8eeae8d3dfa9a4a73fc918b707a -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtrPtr.op_Implicit(Inno.Native.ImGui.ImGuiWindow**)~Inno.Native.ImGui.ImGuiWindowPtrPtr 8d4cdaf56325aa2f6676808e90cfe038ef8eff93d5d71fc9cbfa4aa5407d2ac3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtrPtr.op_Implicit(Inno.Native.ImGui.ImGuiWindowPtrPtr)~Inno.Native.ImGui.ImGuiWindow** d5ceabddcb444ea489ed595604155a957db42225ec92e3a416f5a62374242cf7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtrPtr.op_Equality(Inno.Native.ImGui.ImGuiWindowPtrPtr,Inno.Native.ImGui.ImGuiWindowPtrPtr) 53ff1fc180216bafa21bd2ddf456c0f248a717dcd5e90a20809e32383497362a -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtrPtr.op_Inequality(Inno.Native.ImGui.ImGuiWindowPtrPtr,Inno.Native.ImGui.ImGuiWindowPtrPtr) 9169fe34fdc1f1c3a60aa02a66a9b914fd2b62205c2f8ee84dbe24f7eb6481e0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtrPtr.Equals(Inno.Native.ImGui.ImGuiWindowPtrPtr) e5cbf95293a2b114bcde044ac7867ffc6bf30808fa669862c6da0c0f68c98f61 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtrPtr.Equals(System.Object) bcca48679b6c8e52305929d4420a7dc8a75c1856d3ee03431a4a3d6189117757 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowPtrPtr.GetHashCode a4bc5c80781bdb95d3ca85270a2843e5145ba02ad1d747db17a780f0b0e9c119 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-090b61b14f816b91"></a>[`public Inno.Native.ImGui.ImGuiWindow** Inno.Native.ImGui.ImGuiWindowPtrPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18152) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfc121a7345f97f4"></a>[`public Inno.Native.ImGui.ImGuiWindowPtrPtr.ImGuiWindowPtrPtr(Inno.Native.ImGui.ImGuiWindow** handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18147) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-880b142ccf0587be"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtrPtr.Equals(Inno.Native.ImGui.ImGuiWindowPtrPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18162) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-8891f6f5a0eca2f0"></a>[`public override bool Inno.Native.ImGui.ImGuiWindowPtrPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18163) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-6f2a0ec3193349c7"></a>[`public override int Inno.Native.ImGui.ImGuiWindowPtrPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18164) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-119554c2bd213e22"></a>[`public static bool Inno.Native.ImGui.ImGuiWindowPtrPtr.operator ==(Inno.Native.ImGui.ImGuiWindowPtrPtr left, Inno.Native.ImGui.ImGuiWindowPtrPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18160) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bc05b9af9deb72f4"></a>[`public static Inno.Native.ImGui.ImGuiWindowPtrPtr.implicit operator Inno.Native.ImGui.ImGuiWindowPtrPtr(Inno.Native.ImGui.ImGuiWindow** handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18158) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-10016e9ff6dab1d8"></a>[`public static Inno.Native.ImGui.ImGuiWindowPtrPtr.implicit operator Inno.Native.ImGui.ImGuiWindow**(Inno.Native.ImGui.ImGuiWindowPtrPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18159) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c7a626269bf2454e"></a>[`public static bool Inno.Native.ImGui.ImGuiWindowPtrPtr.operator !=(Inno.Native.ImGui.ImGuiWindowPtrPtr left, Inno.Native.ImGui.ImGuiWindowPtrPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18161) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0f96d74ac9d93f27"></a>[`public bool Inno.Native.ImGui.ImGuiWindowPtrPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18153) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2280c4d2c78236a2"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindowPtrPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18156) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-57b49978dcb2b16e"></a>[`public static Inno.Native.ImGui.ImGuiWindowPtrPtr Inno.Native.ImGui.ImGuiWindowPtrPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18154) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6e06dfdbb13f8030"></a>[`public struct Inno.Native.ImGui.ImGuiWindowPtrPtr : System.IEquatable<Inno.Native.ImGui.ImGuiWindowPtrPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18145) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindowRefreshFlags</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindowRefreshFlags 382dfc6e8b87199ad61d8c1b6cc28cc93f2ef8fc8b3020e9e75656d7d5fc37ae -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowRefreshFlags.None 054a6bd7cd3838ad5cbf9e803d53dfdbb16df5d31af51edd5436c3cde3be615f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowRefreshFlags.TryToAvoidRefresh 1be565597f401ae0a45c31256a77d9206992272afbdf706eccc87aad66792482 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowRefreshFlags.OnHover 24c85d179b817074d506d8a866a82b6a6964d8861fdc0418f087c8ec46e8cebc -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowRefreshFlags.OnFocus fbddd4514f1036fb78303b0e6b86af81d97a868242c09652b1dbf3d8126a36b2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4acf3904b93abec9"></a>[`Inno.Native.ImGui.ImGuiWindowRefreshFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1418) | 当前常量或枚举值：`0`。 | — |
| <a id="api-cda002fd5751b4f9"></a>[`Inno.Native.ImGui.ImGuiWindowRefreshFlags.OnFocus = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1421) | 当前常量或枚举值：`4`。 | — |
| <a id="api-70043fdf1706a0cb"></a>[`Inno.Native.ImGui.ImGuiWindowRefreshFlags.OnHover = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1420) | 当前常量或枚举值：`2`。 | — |
| <a id="api-38075743be0523aa"></a>[`Inno.Native.ImGui.ImGuiWindowRefreshFlags.TryToAvoidRefresh = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1419) | 当前常量或枚举值：`1`。 | — |
| <a id="api-bb56cad4d18fff8b"></a>[`public enum Inno.Native.ImGui.ImGuiWindowRefreshFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1416) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindowSettings</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindowSettings 1c94ccbe237a79d05f127f149702f95f42692fce2e586903bfee292a9ee42051 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.ID 6af8853414788dc884cd22aeab1a6fe6e27edcf738fc4614e32efbd77fb5c7e1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.Pos aa9e640eae144701c6a310d887552717653e86faba285086cbd2167074a3f160 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.Size 8b78883f4d79238a329c21f83256d8142a3d3fe7e5c2acaf9d5e9b5eaca49e76 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.ViewportPos 3a432f6690f571335af7aeb7b2ef69370697f9bc441118e771d3de000c3328bb -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.ViewportId 2967e60558ec524cbbf14d0439559fa2e59b7380824d13b837a2efdf448d5e43 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.DockId b4b6e133e63cc087150c6abd0a12e4a0880ddc8268cc5089777e16409316d5f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.ClassId e53aacca933ef54a88ba8906a72c563c22e3b78552f3c1861ab8bd69627ee721 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.DockOrder 2cb54d9c023d0a3af29c4a46165d23c013ce9e28927ed1ecf8a4fa34d0c8a65a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.Collapsed 4a1a3831b8461e25270ed33eeca31199a0f54cf874d02688cedcea79129db586 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.IsChild 782b2dccbe5ab8c376c08b78e4c455b0d9dc9b6f60bd201bb503bff95fa9c288 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.WantApply 35dd3326d35a72baa89ad7ab30eba3d8dd1b8bad8195b4cef7745c9ffc2c342c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettings.WantDelete 123218a88cc64b95ad83adaea163ec9f1f29d6c40f04df8ddc50cd05cfe49532 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowSettings.#ctor(System.UInt32,Inno.Native.ImGui.ImVec2Ih,Inno.Native.ImGui.ImVec2Ih,Inno.Native.ImGui.ImVec2Ih,System.UInt32,System.UInt32,System.UInt32,System.Int16,System.Byte,System.Byte,System.Byte,System.Byte) 127e53ac6faba59c59099e8fe31309a8e6c567a81871860d8df05fc6930fb1cf -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowSettings.Destroy 7051faf9cfcc998688ffb2fbfa6eebd9f5708318f7aabb512ec369a512aa78ac -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowSettings.GetName 21b24cabdc7b65d74b7d9de6d922b1561b27016b60d2dceed6461b9625e7f8cf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1b9424d4f4d63bb3"></a>[`public uint Inno.Native.ImGui.ImGuiWindowSettings.ClassId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10320) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d93cbaf8263b50d"></a>[`public byte Inno.Native.ImGui.ImGuiWindowSettings.Collapsed`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10322) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa98c2da220d194a"></a>[`public uint Inno.Native.ImGui.ImGuiWindowSettings.DockId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10319) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-543c5549e3e8ef66"></a>[`public short Inno.Native.ImGui.ImGuiWindowSettings.DockOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10321) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-19b9f71a9549ef3d"></a>[`public uint Inno.Native.ImGui.ImGuiWindowSettings.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10314) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f6ac0826f5a1bb6e"></a>[`public byte Inno.Native.ImGui.ImGuiWindowSettings.IsChild`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10323) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2f9978ed631eacb"></a>[`public Inno.Native.ImGui.ImVec2Ih Inno.Native.ImGui.ImGuiWindowSettings.Pos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10315) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c37aee57e1bac8cc"></a>[`public Inno.Native.ImGui.ImVec2Ih Inno.Native.ImGui.ImGuiWindowSettings.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10316) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-555cdd09107b2323"></a>[`public uint Inno.Native.ImGui.ImGuiWindowSettings.ViewportId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10318) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d05e1212175ecbe8"></a>[`public Inno.Native.ImGui.ImVec2Ih Inno.Native.ImGui.ImGuiWindowSettings.ViewportPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10317) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-44eb9583854cf14f"></a>[`public byte Inno.Native.ImGui.ImGuiWindowSettings.WantApply`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10324) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ea6b9f78cc45aa3d"></a>[`public byte Inno.Native.ImGui.ImGuiWindowSettings.WantDelete`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10325) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03ed734dd375d1ca"></a>[`public Inno.Native.ImGui.ImGuiWindowSettings.ImGuiWindowSettings(uint iD = 0, Inno.Native.ImGui.ImVec2Ih pos = default(Inno.Native.ImGui.ImVec2Ih), Inno.Native.ImGui.ImVec2Ih size = default(Inno.Native.ImGui.ImVec2Ih), Inno.Native.ImGui.ImVec2Ih viewportPos = default(Inno.Native.ImGui.ImVec2Ih), uint viewportId = 0, uint dockId = 0, uint classId = 0, short dockOrder = 0, byte collapsed = 0, byte isChild = 0, byte wantApply = 0, byte wantDelete = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10326) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ab064f1c3fef19b8"></a>[`public void Inno.Native.ImGui.ImGuiWindowSettings.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67322) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-93491ca97c0336b2"></a>[`public byte* Inno.Native.ImGui.ImGuiWindowSettings.GetName()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67333) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b74e982697b8dad3"></a>[`public struct Inno.Native.ImGui.ImGuiWindowSettings`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10312) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindowSettingsPtr</code> · 27 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindowSettingsPtr 35860f3bce562d9e939aa4aadf4cfe188398e67c9b86df696559a243a81c4893 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowSettingsPtr.#ctor(Inno.Native.ImGui.ImGuiWindowSettings*) 5ce89cd82a93e64b1c950f357b2ca4e158f118bc83e00ca935952012b9f380b5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowSettingsPtr.Handle 99386f4ae0a800e49ec730a82cc411ca2ad908c2657f333fe355a7845bcb3607 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.IsNull 28b24764b223ae0bc3bd564d1205a660aed3513e3a42b3ea8895e74f422dbe88 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.Null c74dbf9c7dc2c3a084b76650d12bdbd587605ccd507a745d44d8156649e5740a -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.Item(System.Int32) 84779c061ef79713a721c8fcbfa421dee48e6cef096f1a6ca691287080d1ff1a -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowSettingsPtr.op_Implicit(Inno.Native.ImGui.ImGuiWindowSettings*)~Inno.Native.ImGui.ImGuiWindowSettingsPtr 39b49cf1adbc754dfe77345d976258c10ffdc074dcba65d82d18306128cabb9c -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowSettingsPtr.op_Implicit(Inno.Native.ImGui.ImGuiWindowSettingsPtr)~Inno.Native.ImGui.ImGuiWindowSettings* 3f179759ad0bac7f13fea4644ce6c506be4e08a7a6979275654e320eb924bcf6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowSettingsPtr.op_Equality(Inno.Native.ImGui.ImGuiWindowSettingsPtr,Inno.Native.ImGui.ImGuiWindowSettingsPtr) 405e6a30e6c5035a6c8b3a65c0437ddfcfb1643f8abff1234c393cdd0e4866a9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowSettingsPtr.op_Inequality(Inno.Native.ImGui.ImGuiWindowSettingsPtr,Inno.Native.ImGui.ImGuiWindowSettingsPtr) a8be69b46b5c44f66b4b237a5e5965c15365e23054f06668594c2fb1b0261087 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowSettingsPtr.Equals(Inno.Native.ImGui.ImGuiWindowSettingsPtr) 99281550da84efdfcfac60d87d71212651434fc0735324b5f535d7ef56617b2b -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowSettingsPtr.Equals(System.Object) 3cc0c5381f82ca9decd3990446f57e6c96284520f3967d6ecc15492176b31b7e -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowSettingsPtr.GetHashCode 396af72aad2fd6b18f602eecf23fcd87fcc89b73e8db072fac0771099ea12c85 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.ID d3a8256e38de7a2fe90d4ad765a2148332a30fd33a315d5fb4036f8911be6494 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.Pos a3bbeade2028603eb684f1a18ddd23cde12281e27c32e1e4381da0808bf34e15 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.Size d74b7635dead8042936f73db9d42fff9458619d2548081b33084c3a9b478609b -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.ViewportPos ab32671dbea8d620052273d63805f430591cec4b7e6e633a59d43bd048966b7b -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.ViewportId 35c59f81ffbf22bf11ee2e54e4e39e8b84bd72810b46115ecbdc3c845ad84b57 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.DockId 409dc960ef8fb7763a3acaa45ad5be260bbf6cbad0a2c8bdb9d8741afe9ccc80 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.ClassId 9894ede14f4df11ce63143c60c6d380ef489b68082c4eae0353894d38961623e -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.DockOrder 9f3bfe3581d7fca732d70f7fcfd229274b8f8aecf027d3c5c1aa815ce5ca5c1e -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.Collapsed 418c3d853bc65c4821f551dce48873d1a4f180178004ac2498ac931892d52a76 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.IsChild 2d690bdac1ec6a3f72b5a7acb07048ec89eb3c71a5e4f7aa345db48ea82789e9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.WantApply c45cb4dfe98cdea1c151c6cecf9f12836b6f9afb40fb2ffb7d2b540af667ca3f -->
<!-- api: P:Inno.Native.ImGui.ImGuiWindowSettingsPtr.WantDelete ee0fe136d7b06387ce02861c5471f3d8c1c50e055ebe5328392a62562a592137 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowSettingsPtr.Destroy 8d1731fcd564e4cd8d6359266e6c1c33930e512ec95fd1548559b4e27e2a57f4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowSettingsPtr.GetName d663683f0a674c925c1b69a3a4808e7b68359eaaeaea1ccbe18057ed9c34d90b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-de0fcf7d141678aa"></a>[`public Inno.Native.ImGui.ImGuiWindowSettings* Inno.Native.ImGui.ImGuiWindowSettingsPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18176) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ceab3ec48600e54b"></a>[`public Inno.Native.ImGui.ImGuiWindowSettingsPtr.ImGuiWindowSettingsPtr(Inno.Native.ImGui.ImGuiWindowSettings* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18171) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-cf3af14aba9fd920"></a>[`public void Inno.Native.ImGui.ImGuiWindowSettingsPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18206) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-998a14b85aac3d18"></a>[`public bool Inno.Native.ImGui.ImGuiWindowSettingsPtr.Equals(Inno.Native.ImGui.ImGuiWindowSettingsPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18186) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-d843993b67af033a"></a>[`public override bool Inno.Native.ImGui.ImGuiWindowSettingsPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18187) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-f3794a8b46803a07"></a>[`public override int Inno.Native.ImGui.ImGuiWindowSettingsPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18188) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-bfff8b39028c81ae"></a>[`public string? Inno.Native.ImGui.ImGuiWindowSettingsPtr.GetName()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18214) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cd79784f7c842102"></a>[`public static bool Inno.Native.ImGui.ImGuiWindowSettingsPtr.operator ==(Inno.Native.ImGui.ImGuiWindowSettingsPtr left, Inno.Native.ImGui.ImGuiWindowSettingsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18184) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-523470b68a0bc5c8"></a>[`public static Inno.Native.ImGui.ImGuiWindowSettingsPtr.implicit operator Inno.Native.ImGui.ImGuiWindowSettingsPtr(Inno.Native.ImGui.ImGuiWindowSettings* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18182) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f1d66b62e1e0c1b3"></a>[`public static Inno.Native.ImGui.ImGuiWindowSettingsPtr.implicit operator Inno.Native.ImGui.ImGuiWindowSettings*(Inno.Native.ImGui.ImGuiWindowSettingsPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18183) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-24869c707e04024b"></a>[`public static bool Inno.Native.ImGui.ImGuiWindowSettingsPtr.operator !=(Inno.Native.ImGui.ImGuiWindowSettingsPtr left, Inno.Native.ImGui.ImGuiWindowSettingsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18185) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ce855ec869d53422"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowSettingsPtr.ClassId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18196) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e3a6786a5b614f80"></a>[`public bool Inno.Native.ImGui.ImGuiWindowSettingsPtr.Collapsed { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18198) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-75a4d88382892957"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowSettingsPtr.DockId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18195) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-59d0fab9e23e7da5"></a>[`public ref short Inno.Native.ImGui.ImGuiWindowSettingsPtr.DockOrder { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18197) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5e6edf3efd09221c"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowSettingsPtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18190) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dee02a5c3c301866"></a>[`public bool Inno.Native.ImGui.ImGuiWindowSettingsPtr.IsChild { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18199) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5cd7ce7875a643cd"></a>[`public bool Inno.Native.ImGui.ImGuiWindowSettingsPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18177) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bcefa1a49e38d434"></a>[`public Inno.Native.ImGui.ImGuiWindowSettings Inno.Native.ImGui.ImGuiWindowSettingsPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18180) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4e4dc064c028959f"></a>[`public static Inno.Native.ImGui.ImGuiWindowSettingsPtr Inno.Native.ImGui.ImGuiWindowSettingsPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18178) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-951388606fcd27d0"></a>[`public ref Inno.Native.ImGui.ImVec2Ih Inno.Native.ImGui.ImGuiWindowSettingsPtr.Pos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18191) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eb8867ee0735f463"></a>[`public ref Inno.Native.ImGui.ImVec2Ih Inno.Native.ImGui.ImGuiWindowSettingsPtr.Size { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18192) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6bc152665af6a094"></a>[`public ref uint Inno.Native.ImGui.ImGuiWindowSettingsPtr.ViewportId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18194) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-63dbb8a9c45a5213"></a>[`public ref Inno.Native.ImGui.ImVec2Ih Inno.Native.ImGui.ImGuiWindowSettingsPtr.ViewportPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18193) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1245efcf100ef8ec"></a>[`public bool Inno.Native.ImGui.ImGuiWindowSettingsPtr.WantApply { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18200) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e0ed2041c2a0c067"></a>[`public bool Inno.Native.ImGui.ImGuiWindowSettingsPtr.WantDelete { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18201) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0eee9260722cf1ec"></a>[`public struct Inno.Native.ImGui.ImGuiWindowSettingsPtr : System.IEquatable<Inno.Native.ImGui.ImGuiWindowSettingsPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18169) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindowStackData</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindowStackData 38e0964fa1a63723d54b86dbbd4748c9a6f308949ef9599570d53e40b9eb03ab -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowStackData.Window dea37fa6b1c249065c7fb13bfb136525ba9045f0c3fe1d9bb6ad362b44346623 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowStackData.ParentLastItemDataBackup 0933fec14c6668c94a39da687d9060e2b56ff13b48520d1b6e56a4bbf76c3dd0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowStackData.StackSizesInBegin ab1ef4590675cc21d314f678e4a9a6b640055eb42c661846fb3ecb85bb078320 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowStackData.DisabledOverrideReenable 932a6370b5dfcd784e01ceb9b65687e8a275050f090e1e6d7cda2131ea7d597f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowStackData.DisabledOverrideReenableAlphaBackup 80cd099eec3cbbdbb73a60e6769839ca107b7ff0dd3303f171f6a68784b1263b -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowStackData.#ctor(Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiLastItemData,Inno.Native.ImGui.ImGuiErrorRecoveryState,System.Byte,System.Single) 1af8f00e20f6805a50ff4e5c68662f0d6d7ca8c71f5c535f06c3db66ec232bbc -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0c7d50cd6a243687"></a>[`public byte Inno.Native.ImGui.ImGuiWindowStackData.DisabledOverrideReenable`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5096) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c2c265a92ccae49"></a>[`public float Inno.Native.ImGui.ImGuiWindowStackData.DisabledOverrideReenableAlphaBackup`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5097) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d4a9dcc128e41598"></a>[`public Inno.Native.ImGui.ImGuiLastItemData Inno.Native.ImGui.ImGuiWindowStackData.ParentLastItemDataBackup`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5094) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1814e20a9521d803"></a>[`public Inno.Native.ImGui.ImGuiErrorRecoveryState Inno.Native.ImGui.ImGuiWindowStackData.StackSizesInBegin`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5095) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-adfb19bae3dd4e3a"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiWindowStackData.Window`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5093) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b3550432f18b0f7"></a>[`public Inno.Native.ImGui.ImGuiWindowStackData.ImGuiWindowStackData(Inno.Native.ImGui.ImGuiWindow* window = null, Inno.Native.ImGui.ImGuiLastItemData parentLastItemDataBackup = default(Inno.Native.ImGui.ImGuiLastItemData), Inno.Native.ImGui.ImGuiErrorRecoveryState stackSizesInBegin = default(Inno.Native.ImGui.ImGuiErrorRecoveryState), byte disabledOverrideReenable = 0, float disabledOverrideReenableAlphaBackup = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5098) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b1372d181fbfc483"></a>[`public struct Inno.Native.ImGui.ImGuiWindowStackData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L5091) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiWindowTempData</code> · 45 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiWindowTempData 8ed3e8d81a8695e1fd28c7981baa1839a5ce3d06567f3c69bb8ed2b0b335d13d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CursorPos b8635671ebbd14626a3b129003065a60e38e1b546eba54964e0651794afd9492 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CursorPosPrevLine 74cc9124ce7c3a4065e073504c7e500d8832ed6b03ee5105d1f0c6bdad74be0c -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CursorStartPos a53e2192d14ef524cd057c4b2694326bab1681c443d71f1484a5d967212f0d62 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CursorMaxPos 187113a414e29edf8c639776fd50933c9f38c069fb4666bb6d4635f0f276420a -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.IdealMaxPos 5d5f174857d1f54d48daf2158e454baf1ba51c2c2b43cba2c7b40bb73e8e5b34 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CurrLineSize ae0638167b14cbd19ab4a831f180e1ccd80286ffec3a093d7414b07da443261b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.PrevLineSize 1616fc85caa028079be6865300b7432836da80d5a820282a6090b1357c436a06 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CurrLineTextBaseOffset 8e65d5eb9f422b0f0eb64121f6d1805da728fa367e2bd9d9a7a8f6e25b661c47 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.PrevLineTextBaseOffset 14746b8aeafa585ff8ee9b823c6f07ad2f10c0b97ab2eb226140b68fdf2c0d35 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.IsSameLine 315e01a8f2324369733e0c31c6b7344b2f51102f8a320801c3f42c1691b89515 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.IsSetPos 635f5af6f477f4bef4d9acb62446e1ca48bb2ce147bff06e66a1c601cb2b65a3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.Indent 64698146d54c033fc3bcc15a3085ef1501a713c4c0ad5a08a0e76a8d9fe064e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ColumnsOffset 01b07e1e9dcbb6c1b5bbc020f77717e8797a8cc25c55d3cccc24be6e488a470b -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.GroupOffset cc47f6adf5bdfea69be2ad46215fb4a0e7eea6c8ddb7807a33aaedf4b82137e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CursorStartPosLossyness 53fbf530dbff603ea7e0a8880c9d7471f1375f41ef53bea69fb833b206a7973f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.NavLayerCurrent bb40e928ec4f39e69fe6f34013231765529d0d8d981983c7458376ed6b6b08b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.NavLayersActiveMask e078d667a29d787f6f6ed163ca6ff7fa190a44b7f5ca9ddee883522e0807e3de -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.NavLayersActiveMaskNext 76e273d994c473919812704309a40f3662b6dd6fbbcff7f4482910cc317825a9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.NavIsScrollPushableX 075e84ec6fe3d5ee2b9417366b6355d8f40314b6fe6577f0890d20451b98c4ef -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.NavHideHighlightOneFrame 8c71afeae95bcf98adcff1e0f6afb25d67566f28de5e4f97fbd99b746912f334 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.NavWindowHasScrollY 29038cd56609fa19dec5f6fbd7e477a3a0540a8e0e313a3f451c00c12378a16f -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.MenuBarAppending 58cff1a3e2bf807bd57588780fceca61d7ba1baab275afa16c1805429afa62a5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.MenuBarOffset 22ad32a2cdf0d711805897635d3e0b30b10b5778d977fa36008c48ef66a6239d -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.MenuColumns 96a86883f03a2521d34431c5dc88da2b87aa92ef9798a70f494b963d120ea175 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.TreeDepth 7b48c9dc1a3a002543d6f69442ada3ed2d133ab40ceebe3da3473f238dd5240e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.TreeHasStackDataDepthMask 00605da12741f901d7fd4ed3a7b377fc0e50cb0567f9f299c22856291cfa61a0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.TreeRecordsClippedNodesY2Mask d3ffbba6ba1bbbbe06ebc8559f63f0551ac2bb152f86461eb510b0f90bde2f84 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ChildWindows 7f60fb78d148175a6ed82bad48f66d6a308cf68e09a65c3280866a7df6b94334 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.StateStorage 33f9217982bf1a863d34eabcd4071785741829bbdc6d78c74d66befc32ce2c48 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CurrentColumns 56488fe3e6e040bdae31c7bbf783f02b148d1dd64637e493849f99665bc48294 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.CurrentTableIdx 84849585b74f46ea17e23eed7c981a9310c7fd428394ad745d8f711ba5269da9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.LayoutType aefcc9bcce28968b1ae4c0ac1cbf181fd85711f7cf90aa87e96c926411f1d6e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ParentLayoutType 028bb9d95fd68bc4cd874034e85ea7f3152ce0b1555b170dabf9bb21b1d53840 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ModalDimBgColor 4f630576ca1d9692453ee7a71c0389e11c82185f0afe768c28cc416af4a46376 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.WindowItemStatusFlags dd4e2b94ee56a85794ec213097867dc5e7c6f5623aae06fd8c13401686d4c367 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ChildItemStatusFlags e01f5bf47f1b663f1e74d0a381a840826e75bdf9d0ac31d40a94f6e53f17941e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.DockTabItemStatusFlags 1396e506381d18a7703b8c575bfd15b5d2824537aba6956e7d50f9102210b58e -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.DockTabItemRect 5d6982d561132a8eca601236e9d394505e96391160289b0e1934a0e2c27cc733 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ItemWidth 9eed310cab6962832d0826afd9c09bf0bd98b0e39fe58133ebc309d26f0dca06 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ItemWidthDefault e16e7f0ad6633c1b56da5cbf4aa64929c4aa097bd04a325410532a70bd73c0cd -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.TextWrapPos 5dbc5a265122481543a22769134310b87d52a2fb133e8df14b126785820df939 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.ItemWidthStack b1cca7764f0b3690a9a23527b7394e7d9229983fe1d9cf2ae982683a1c66c4d2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiWindowTempData.TextWrapPosStack 12f1d366ab3a10d2368269e56acc6aefe5b0f5a5451ef5eba58b137b35e0e087 -->
<!-- api: M:Inno.Native.ImGui.ImGuiWindowTempData.#ctor(System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2,System.Single,System.Single,System.Byte,System.Byte,Inno.Native.ImGui.ImVec1,Inno.Native.ImGui.ImVec1,Inno.Native.ImGui.ImVec1,System.Numerics.Vector2,Inno.Native.ImGui.ImGuiNavLayer,System.Int16,System.Int16,System.Byte,System.Byte,System.Byte,System.Byte,System.Numerics.Vector2,Inno.Native.ImGui.ImGuiMenuColumns,System.Int32,System.UInt32,System.UInt32,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiWindowPtr},Inno.Native.ImGui.ImGuiStorage*,Inno.Native.ImGui.ImGuiOldColumns*,System.Int32,Inno.Native.ImGui.ImGuiLayoutType,Inno.Native.ImGui.ImGuiLayoutType,System.UInt32,Inno.Native.ImGui.ImGuiItemStatusFlags,Inno.Native.ImGui.ImGuiItemStatusFlags,Inno.Native.ImGui.ImGuiItemStatusFlags,Inno.Native.ImGui.ImRect,System.Single,System.Single,System.Single,Inno.Native.ImGui.ImVector{System.Single},Inno.Native.ImGui.ImVector{System.Single}) 699cb2b25bd9a798a9c8bc5a0076e70febaccb1414a3c6495ec9d6c69a7ab98e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-50b57f9411d64a29"></a>[`public Inno.Native.ImGui.ImGuiItemStatusFlags Inno.Native.ImGui.ImGuiWindowTempData.ChildItemStatusFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4224) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1e85fd61b471fd63"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiWindowPtr> Inno.Native.ImGui.ImGuiWindowTempData.ChildWindows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4216) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-24d1a6c27918ca28"></a>[`public Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImGuiWindowTempData.ColumnsOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4201) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0cb6f6ad0d52a7f8"></a>[`public Inno.Native.ImGui.ImGuiOldColumns* Inno.Native.ImGui.ImGuiWindowTempData.CurrentColumns`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4218) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b98a72054a0f3c1"></a>[`public int Inno.Native.ImGui.ImGuiWindowTempData.CurrentTableIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4219) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6b08af5c2b2d8809"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowTempData.CurrLineSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4194) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3d966df174614d9d"></a>[`public float Inno.Native.ImGui.ImGuiWindowTempData.CurrLineTextBaseOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4196) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-30fc3a60a5b96a31"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowTempData.CursorMaxPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4192) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c616b91559e25b13"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowTempData.CursorPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4189) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5cbf8b73296f614"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowTempData.CursorPosPrevLine`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4190) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e807b50448744c93"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowTempData.CursorStartPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4191) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7ab958cd463b434"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowTempData.CursorStartPosLossyness`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4203) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-83733f98367ed604"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiWindowTempData.DockTabItemRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4226) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f0ce38c8e05fae2"></a>[`public Inno.Native.ImGui.ImGuiItemStatusFlags Inno.Native.ImGui.ImGuiWindowTempData.DockTabItemStatusFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4225) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5ecf60549addd7f4"></a>[`public Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImGuiWindowTempData.GroupOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4202) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a2bd34fd7632b488"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowTempData.IdealMaxPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4193) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9aac6ab787cc53f"></a>[`public Inno.Native.ImGui.ImVec1 Inno.Native.ImGui.ImGuiWindowTempData.Indent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4200) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e58408ab9fa30c6f"></a>[`public byte Inno.Native.ImGui.ImGuiWindowTempData.IsSameLine`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4198) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9456e9d524c61bcf"></a>[`public byte Inno.Native.ImGui.ImGuiWindowTempData.IsSetPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4199) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aaa2dc6262d8cf5b"></a>[`public float Inno.Native.ImGui.ImGuiWindowTempData.ItemWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4227) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9045e7079f19859b"></a>[`public float Inno.Native.ImGui.ImGuiWindowTempData.ItemWidthDefault`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4228) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3c43bf5bb54a8886"></a>[`public Inno.Native.ImGui.ImVector<float> Inno.Native.ImGui.ImGuiWindowTempData.ItemWidthStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4230) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7eecadbbaa264746"></a>[`public Inno.Native.ImGui.ImGuiLayoutType Inno.Native.ImGui.ImGuiWindowTempData.LayoutType`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4220) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-85ba52893bbabca7"></a>[`public byte Inno.Native.ImGui.ImGuiWindowTempData.MenuBarAppending`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4210) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f9dd89c226398d99"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowTempData.MenuBarOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4211) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-674994db3c661125"></a>[`public Inno.Native.ImGui.ImGuiMenuColumns Inno.Native.ImGui.ImGuiWindowTempData.MenuColumns`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4212) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb7353119d136582"></a>[`public uint Inno.Native.ImGui.ImGuiWindowTempData.ModalDimBgColor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4222) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e507bf3d9fd3de52"></a>[`public byte Inno.Native.ImGui.ImGuiWindowTempData.NavHideHighlightOneFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4208) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a2b96fe58de0a53f"></a>[`public byte Inno.Native.ImGui.ImGuiWindowTempData.NavIsScrollPushableX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4207) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2275e05700fece88"></a>[`public Inno.Native.ImGui.ImGuiNavLayer Inno.Native.ImGui.ImGuiWindowTempData.NavLayerCurrent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4204) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7e580b66042d6528"></a>[`public short Inno.Native.ImGui.ImGuiWindowTempData.NavLayersActiveMask`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4205) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bacec926eee71805"></a>[`public short Inno.Native.ImGui.ImGuiWindowTempData.NavLayersActiveMaskNext`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4206) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6d663f7240eb0892"></a>[`public byte Inno.Native.ImGui.ImGuiWindowTempData.NavWindowHasScrollY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4209) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d0a46eb3ac9a03ec"></a>[`public Inno.Native.ImGui.ImGuiLayoutType Inno.Native.ImGui.ImGuiWindowTempData.ParentLayoutType`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4221) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be755de938a5b21c"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiWindowTempData.PrevLineSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4195) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7de7d1c666a04d6"></a>[`public float Inno.Native.ImGui.ImGuiWindowTempData.PrevLineTextBaseOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4197) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5d006153a1af7d1f"></a>[`public Inno.Native.ImGui.ImGuiStorage* Inno.Native.ImGui.ImGuiWindowTempData.StateStorage`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4217) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-170ef4966aa9a5da"></a>[`public float Inno.Native.ImGui.ImGuiWindowTempData.TextWrapPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4229) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5140323d1c72a903"></a>[`public Inno.Native.ImGui.ImVector<float> Inno.Native.ImGui.ImGuiWindowTempData.TextWrapPosStack`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4231) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f46a1fc3a1017130"></a>[`public int Inno.Native.ImGui.ImGuiWindowTempData.TreeDepth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4213) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f75784c86340f72e"></a>[`public uint Inno.Native.ImGui.ImGuiWindowTempData.TreeHasStackDataDepthMask`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4214) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e68d3a606819dd19"></a>[`public uint Inno.Native.ImGui.ImGuiWindowTempData.TreeRecordsClippedNodesY2Mask`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4215) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-47f6796ae5514b35"></a>[`public Inno.Native.ImGui.ImGuiItemStatusFlags Inno.Native.ImGui.ImGuiWindowTempData.WindowItemStatusFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4223) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d44795d25a954858"></a>[`public Inno.Native.ImGui.ImGuiWindowTempData.ImGuiWindowTempData(System.Numerics.Vector2 cursorPos = default(System.Numerics.Vector2), System.Numerics.Vector2 cursorPosPrevLine = default(System.Numerics.Vector2), System.Numerics.Vector2 cursorStartPos = default(System.Numerics.Vector2), System.Numerics.Vector2 cursorMaxPos = default(System.Numerics.Vector2), System.Numerics.Vector2 idealMaxPos = default(System.Numerics.Vector2), System.Numerics.Vector2 currLineSize = default(System.Numerics.Vector2), System.Numerics.Vector2 prevLineSize = default(System.Numerics.Vector2), float currLineTextBaseOffset = 0, float prevLineTextBaseOffset = 0, byte isSameLine = 0, byte isSetPos = 0, Inno.Native.ImGui.ImVec1 indent = default(Inno.Native.ImGui.ImVec1), Inno.Native.ImGui.ImVec1 columnsOffset = default(Inno.Native.ImGui.ImVec1), Inno.Native.ImGui.ImVec1 groupOffset = default(Inno.Native.ImGui.ImVec1), System.Numerics.Vector2 cursorStartPosLossyness = default(System.Numerics.Vector2), Inno.Native.ImGui.ImGuiNavLayer navLayerCurrent = Inno.Native.ImGui.ImGuiNavLayer.Main, short navLayersActiveMask = 0, short navLayersActiveMaskNext = 0, byte navIsScrollPushableX = 0, byte navHideHighlightOneFrame = 0, byte navWindowHasScrollY = 0, byte menuBarAppending = 0, System.Numerics.Vector2 menuBarOffset = default(System.Numerics.Vector2), Inno.Native.ImGui.ImGuiMenuColumns menuColumns = default(Inno.Native.ImGui.ImGuiMenuColumns), int treeDepth = 0, uint treeHasStackDataDepthMask = 0, uint treeRecordsClippedNodesY2Mask = 0, Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiWindowPtr> childWindows = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiWindowPtr>), Inno.Native.ImGui.ImGuiStorage* stateStorage = null, Inno.Native.ImGui.ImGuiOldColumns* currentColumns = null, int currentTableIdx = 0, Inno.Native.ImGui.ImGuiLayoutType layoutType = Inno.Native.ImGui.ImGuiLayoutType.Horizontal, Inno.Native.ImGui.ImGuiLayoutType parentLayoutType = Inno.Native.ImGui.ImGuiLayoutType.Horizontal, uint modalDimBgColor = 0, Inno.Native.ImGui.ImGuiItemStatusFlags windowItemStatusFlags = Inno.Native.ImGui.ImGuiItemStatusFlags.None, Inno.Native.ImGui.ImGuiItemStatusFlags childItemStatusFlags = Inno.Native.ImGui.ImGuiItemStatusFlags.None, Inno.Native.ImGui.ImGuiItemStatusFlags dockTabItemStatusFlags = Inno.Native.ImGui.ImGuiItemStatusFlags.None, Inno.Native.ImGui.ImRect dockTabItemRect = default(Inno.Native.ImGui.ImRect), float itemWidth = 0, float itemWidthDefault = 0, float textWrapPos = 0, Inno.Native.ImGui.ImVector<float> itemWidthStack = default(Inno.Native.ImGui.ImVector<float>), Inno.Native.ImGui.ImVector<float> textWrapPosStack = default(Inno.Native.ImGui.ImVector<float>))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4232) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-77110cd3bea73b35"></a>[`public struct Inno.Native.ImGui.ImGuiWindowTempData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4187) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImPoolImGuiMultiSelectState</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImPoolImGuiMultiSelectState 4a4247314329c7ef356dca3d36af3ed7edef70ca5c03607faf9d94fc6ed456f6 -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiMultiSelectState.Buf 61734744516c11f5a98690bb817e63e2dc928e65663d4b80e6e81d66615c87ea -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiMultiSelectState.Map 25aeebfb3d2b693af3a9a6c20293968623d60f5b3bbb47739e930fcec5493b6f -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiMultiSelectState.FreeIdx 3f3216e8b05e4089cda67f501325d6ab90e92085e69d8458281b72f8c92e8018 -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiMultiSelectState.AliveCount f3a2a2c84f40162766c5c7d435003b66bf68d4144c2926220a2572ca202e08e0 -->
<!-- api: M:Inno.Native.ImGui.ImPoolImGuiMultiSelectState.#ctor(Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiMultiSelectState},Inno.Native.ImGui.ImGuiStorage,System.Int32,System.Int32) fdd992969da0e518b01db6ffceb4de8ee630a8da8658c3dc12f82219ff78c7cf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-75a910388662f108"></a>[`public int Inno.Native.ImGui.ImPoolImGuiMultiSelectState.AliveCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6973) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dd935003e8c54304"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiMultiSelectState> Inno.Native.ImGui.ImPoolImGuiMultiSelectState.Buf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6970) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eab3c4c35185559a"></a>[`public int Inno.Native.ImGui.ImPoolImGuiMultiSelectState.FreeIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6972) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da339cd85e65788e"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImPoolImGuiMultiSelectState.Map`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6971) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b009514f3df83c6"></a>[`public Inno.Native.ImGui.ImPoolImGuiMultiSelectState.ImPoolImGuiMultiSelectState(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiMultiSelectState> buf = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiMultiSelectState>), Inno.Native.ImGui.ImGuiStorage map = default(Inno.Native.ImGui.ImGuiStorage), int freeIdx = 0, int aliveCount = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6974) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b6012f1f587f62e6"></a>[`public struct Inno.Native.ImGui.ImPoolImGuiMultiSelectState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6968) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImPoolImGuiTabBar</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImPoolImGuiTabBar dc1719d6b251bee91a8015edf9fd8d41b6a84d3ee669081df5b7e408cbd7ee4a -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTabBar.Buf b6c8eb5425b80e739c2886ddefa227cb00cbe095f5cb9851455ccb9a3abd0344 -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTabBar.Map 444ac11551105859d8fd7b954a18847139b3b03c85eaadc1757ba23022b363b7 -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTabBar.FreeIdx 0c09bdaacf9ca81f4cd0f60936c755db4ae4c7006efde1f5858eb15b1bc8fa1c -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTabBar.AliveCount ad7722f4eaa3038538fcef822f3060617e27d499333c13aea53e6826417a5cd4 -->
<!-- api: M:Inno.Native.ImGui.ImPoolImGuiTabBar.#ctor(Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiTabBar},Inno.Native.ImGui.ImGuiStorage,System.Int32,System.Int32) 5a7bdbcea156faf5e2926695f2cb0f483f096a754b22bb0f9d62c8ddd44dad6c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9e6e3791d951fc0e"></a>[`public int Inno.Native.ImGui.ImPoolImGuiTabBar.AliveCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6748) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e3e9437461d3bb2"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTabBar> Inno.Native.ImGui.ImPoolImGuiTabBar.Buf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6745) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-daada1943dfa133f"></a>[`public int Inno.Native.ImGui.ImPoolImGuiTabBar.FreeIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6747) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cd91269887376225"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImPoolImGuiTabBar.Map`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6746) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8cc69e3f48e4ffa5"></a>[`public Inno.Native.ImGui.ImPoolImGuiTabBar.ImPoolImGuiTabBar(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTabBar> buf = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTabBar>), Inno.Native.ImGui.ImGuiStorage map = default(Inno.Native.ImGui.ImGuiStorage), int freeIdx = 0, int aliveCount = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6749) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-34a6d34b3610a0ef"></a>[`public struct Inno.Native.ImGui.ImPoolImGuiTabBar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6743) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImPoolImGuiTable</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImPoolImGuiTable 3a31b00d7f55a40c395e5476dc7435f256a9a2b6061d6a80d9f64b73beaa4e43 -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTable.Buf 9ea76bd01e1e86afe75dd6a19768785e42ec599e7c1ddea3cd01833fc73a8688 -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTable.Map a3f9d6ea369687dd3f76d65d90d893b2e6f28d24796a19425e1cd5db35f8905d -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTable.FreeIdx 566af8c4049121566219884d034f1712912d111285efeb6eebb23aa2ecf8c4ed -->
<!-- api: F:Inno.Native.ImGui.ImPoolImGuiTable.AliveCount 2ba64543d1b9bceb0fbe1e6595f1df0b6cec742f01db6a5b9bb6af6b0a4b014e -->
<!-- api: M:Inno.Native.ImGui.ImPoolImGuiTable.#ctor(Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiTable},Inno.Native.ImGui.ImGuiStorage,System.Int32,System.Int32) 2db898641263a1f1e22ca7ada13e62990a6533b02790563639d9529645403ac5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4fbc71ebe517f26e"></a>[`public int Inno.Native.ImGui.ImPoolImGuiTable.AliveCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6729) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad9b88272f488a20"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTable> Inno.Native.ImGui.ImPoolImGuiTable.Buf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6726) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-395debc92e31c640"></a>[`public int Inno.Native.ImGui.ImPoolImGuiTable.FreeIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6728) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-78149f1481ab6ae2"></a>[`public Inno.Native.ImGui.ImGuiStorage Inno.Native.ImGui.ImPoolImGuiTable.Map`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6727) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e1b041b542e6188"></a>[`public Inno.Native.ImGui.ImPoolImGuiTable.ImPoolImGuiTable(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTable> buf = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTable>), Inno.Native.ImGui.ImGuiStorage map = default(Inno.Native.ImGui.ImGuiStorage), int freeIdx = 0, int aliveCount = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6730) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-25ab8de5d7822809"></a>[`public struct Inno.Native.ImGui.ImPoolImGuiTable`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6724) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImRect</code> · 30 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImRect 99974bcd7a96b60e6b6162fa937200f4f0696585a04b65e848858e70b2d0f420 -->
<!-- api: F:Inno.Native.ImGui.ImRect.Min 982f4ea7410428dd6a8712a2465db730aa6ed441fe47a40d7257176143090f0a -->
<!-- api: F:Inno.Native.ImGui.ImRect.Max 9cd4a8db3313e3fb910394f7257e38c9349c49ffd481d27dff1bd605aee1a2b4 -->
<!-- api: M:Inno.Native.ImGui.ImRect.#ctor(System.Numerics.Vector2,System.Numerics.Vector2) 8537e0a94cf13ebdbe93a5685c811ac7e4e605585d2189acc2a8492dfaf8e956 -->
<!-- api: M:Inno.Native.ImGui.ImRect.Destroy d963afe5851d2392329647aaa012b0607a64ff1d555a1e810c04791c9e666dba -->
<!-- api: M:Inno.Native.ImGui.ImRect.GetCenter fcd4468182923b4d048b05f034e2914974d0fff2a3de1cf63054c101d8fbd7de -->
<!-- api: M:Inno.Native.ImGui.ImRect.GetSize c72293c07686ce911eba99d62bac48e3510291b060cd5f32ae111c4503d381e8 -->
<!-- api: M:Inno.Native.ImGui.ImRect.GetWidth 34fd0743a0746c3db7df0da967e60df121afb771f11558a18d0d25081d3bc589 -->
<!-- api: M:Inno.Native.ImGui.ImRect.GetHeight 348b5a76a8bfd7d2de788d9cab9cee7b9358b233ba4af868f106969eb2cc8a8f -->
<!-- api: M:Inno.Native.ImGui.ImRect.GetArea 5b617450a3971e69107ff25ed09e481d014c2382ae3e51a3dc2ee3913d311979 -->
<!-- api: M:Inno.Native.ImGui.ImRect.GetTL 1e601bc8c0945ef43d47ee52c5be965811a8041af56a89238f5085af7a4a2035 -->
<!-- api: M:Inno.Native.ImGui.ImRect.GetTR 55fde5fcbef4b65faa6600275efa3eaa659950f028eae485abc9dd119e74a31e -->
<!-- api: M:Inno.Native.ImGui.ImRect.GetBL daf66cd054116b4efe98b0036aa4f1c753c8dd05b76483d4f01e42b6ba680b06 -->
<!-- api: M:Inno.Native.ImGui.ImRect.GetBR 86c7351fe8405a106dd5dcb4d96cc53e6573650ee026fbc57c467f4daa8bee05 -->
<!-- api: M:Inno.Native.ImGui.ImRect.Contains(System.Numerics.Vector2) 38cad4f4f029f436df0dbc897dc4550fc1c71b23c7d67afdc866b08cffc65a30 -->
<!-- api: M:Inno.Native.ImGui.ImRect.Contains(Inno.Native.ImGui.ImRect) 44c614482a2ab72b6d6b9754cea091ba976a1e8a17f6be8fc401346329eb98b9 -->
<!-- api: M:Inno.Native.ImGui.ImRect.ContainsWithPad(System.Numerics.Vector2,System.Numerics.Vector2) b99ac619ce7627ed8280f486d9b2cc82f7633a1b027e67b9e71ced4bfe826b19 -->
<!-- api: M:Inno.Native.ImGui.ImRect.Overlaps(Inno.Native.ImGui.ImRect) becd8e60f4ce5ae3f6fceff943536f368afcc474d1d033ffa46cbc0476bef359 -->
<!-- api: M:Inno.Native.ImGui.ImRect.Add(System.Numerics.Vector2) dddfe9f7ff34d8fc75e3e72e5d88881c93b2074a992c82dfa37da31006112675 -->
<!-- api: M:Inno.Native.ImGui.ImRect.Add(Inno.Native.ImGui.ImRect) 0d01cd939efc0d605f8bae3c320c5a0739291efe7768d2aa07f02a64e5537479 -->
<!-- api: M:Inno.Native.ImGui.ImRect.Expand(System.Single) 5a51021452e0c2e583907bdf2bae8adeecc4f6dca832fd5f42f2d16882887254 -->
<!-- api: M:Inno.Native.ImGui.ImRect.Expand(System.Numerics.Vector2) 1d1cb73b80e65bdf71fcc9c6a5b309489ecd3e3e123c2fe4b9fa519ae7e810da -->
<!-- api: M:Inno.Native.ImGui.ImRect.Translate(System.Numerics.Vector2) 62b4a6df1a627a2b838d8ec13f28bb7a3202fba72aca473b947e25b0df3f0e06 -->
<!-- api: M:Inno.Native.ImGui.ImRect.TranslateX(System.Single) 7291459f29a58ce0e4f2a1837b241a3dcba83b5fd0baf00b700deafa9580a2db -->
<!-- api: M:Inno.Native.ImGui.ImRect.TranslateY(System.Single) 7ca4f1a14d24eda19524b2931340c357fabdc8edbaaf01f9d09865ad7b45ea8a -->
<!-- api: M:Inno.Native.ImGui.ImRect.ClipWith(Inno.Native.ImGui.ImRect) 257c046bc04586bf46a3291e721552a0d2f6305c37cfa898d55b9f623f05b300 -->
<!-- api: M:Inno.Native.ImGui.ImRect.ClipWithFull(Inno.Native.ImGui.ImRect) d888d0273b5fe72bac7c71a0e0858ff836c4a2d6bb439952e465a47bce23521c -->
<!-- api: M:Inno.Native.ImGui.ImRect.IsInverted fe3e423929e2b3171b6a53a630b458e2f5aaead644399a46818202aad795dea1 -->
<!-- api: M:Inno.Native.ImGui.ImRect.ToVec4 aad61bd3831b42365750923707e8a520123a1775e01aa4a27c43571862437d4e -->
<!-- api: M:Inno.Native.ImGui.ImRect.AsVec4 ecc7c4e69077ae2a86d3817699c0175b37756ace9c2b177daca4769d50b4462a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-568b54d069318fd1"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRect.Max`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4094) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-295cbedf86a451d6"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRect.Min`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4093) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed7cb1362a3dd55e"></a>[`public Inno.Native.ImGui.ImRect.ImRect(System.Numerics.Vector2 min = default(System.Numerics.Vector2), System.Numerics.Vector2 max = default(System.Numerics.Vector2))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4095) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fdbe805639fb3be2"></a>[`public void Inno.Native.ImGui.ImRect.Add(Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66137) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c869c2bb5a88efe6"></a>[`public void Inno.Native.ImGui.ImRect.Add(System.Numerics.Vector2 p)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66126) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4666226dc944d701"></a>[`public System.Numerics.Vector4* Inno.Native.ImGui.ImRect.AsVec4()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66247) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0cb3c93d6d966cfc"></a>[`public void Inno.Native.ImGui.ImRect.ClipWith(Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66203) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-67e9490c93f6a6b9"></a>[`public void Inno.Native.ImGui.ImRect.ClipWithFull(Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66214) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e356a43df665c21d"></a>[`public byte Inno.Native.ImGui.ImRect.Contains(Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66093) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f02f34007ff8cfae"></a>[`public byte Inno.Native.ImGui.ImRect.Contains(System.Numerics.Vector2 p)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66082) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-430f602a5faa141f"></a>[`public byte Inno.Native.ImGui.ImRect.ContainsWithPad(System.Numerics.Vector2 p, System.Numerics.Vector2 pad)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66104) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a12be31a010f55e4"></a>[`public void Inno.Native.ImGui.ImRect.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65972) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-87e51496860c422a"></a>[`public void Inno.Native.ImGui.ImRect.Expand(System.Numerics.Vector2 amount)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66159) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e96f54775bd3f364"></a>[`public void Inno.Native.ImGui.ImRect.Expand(float amount)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66148) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7a61a14ae5677ed6"></a>[`public float Inno.Native.ImGui.ImRect.GetArea()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66027) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-27778f04669a0e3e"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRect.GetBL()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66060) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-70c0f6635d213f07"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRect.GetBR()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66071) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1354c495c88c5ce9"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRect.GetCenter()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65983) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-11394856ef6eb26e"></a>[`public float Inno.Native.ImGui.ImRect.GetHeight()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66016) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-812733093da0f70d"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRect.GetSize()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65994) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4ee4dbcf722e0f1c"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRect.GetTL()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66038) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a0cff935cddc3cbf"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRect.GetTR()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66049) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9caf53d928b8d5ff"></a>[`public float Inno.Native.ImGui.ImRect.GetWidth()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66005) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-025c8b68ddc94f9e"></a>[`public byte Inno.Native.ImGui.ImRect.IsInverted()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66225) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3dcaea5704ad71e1"></a>[`public byte Inno.Native.ImGui.ImRect.Overlaps(Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66115) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5cf48f4708b8d85a"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImRect.ToVec4()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66236) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-214e31129c9c5270"></a>[`public void Inno.Native.ImGui.ImRect.Translate(System.Numerics.Vector2 d)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66170) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0705e602ff54b9d9"></a>[`public void Inno.Native.ImGui.ImRect.TranslateX(float dx)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66181) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-187f0f90f13c999a"></a>[`public void Inno.Native.ImGui.ImRect.TranslateY(float dy)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66192) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f6c726f83a76dc93"></a>[`public struct Inno.Native.ImGui.ImRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4091) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImRectPtr</code> · 41 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImRectPtr 9026c2e79d0ead73074b36492d60f15de0022f7b94a4e2c903af94c2fb55e4fa -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.#ctor(Inno.Native.ImGui.ImRect*) d538a23b616699ff62c298427e8b407ebe12491a416746b8ea47dc0c98ff79c0 -->
<!-- api: F:Inno.Native.ImGui.ImRectPtr.Handle 06422e8bb88d0c7c44308c5f617d2bdba4876005831f08783a94a22993a4392e -->
<!-- api: P:Inno.Native.ImGui.ImRectPtr.IsNull 6db7760ed7afd11d73f080289f47202b7e8b7906249a885e79b1a10bc45422c1 -->
<!-- api: P:Inno.Native.ImGui.ImRectPtr.Null dfb558a3d11bed5acd5ac48ff5e4ebe051ba7a04f7773065507a5515ad6b295a -->
<!-- api: P:Inno.Native.ImGui.ImRectPtr.Item(System.Int32) 24b008e16d36b3798a7b180d871ad91575cdafea03093e78d7290636eb8dca8e -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.op_Implicit(Inno.Native.ImGui.ImRect*)~Inno.Native.ImGui.ImRectPtr 9d6d45b0c4a260a56e9c3de02b9e28e8efdcc55cd52fe0ea8b520d0367846e4b -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.op_Implicit(Inno.Native.ImGui.ImRectPtr)~Inno.Native.ImGui.ImRect* 35f836a0f4e88d0e3d82b1232e2ab07584638795e1cd7fa239e11246adf557fe -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.op_Equality(Inno.Native.ImGui.ImRectPtr,Inno.Native.ImGui.ImRectPtr) 586e39c4aa159d6d9fb6523453ccd41722a6570dbac1b57fe54978d4b0625ecd -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.op_Inequality(Inno.Native.ImGui.ImRectPtr,Inno.Native.ImGui.ImRectPtr) 1f3fa080f7a0eb89cdb20edd66c51b903a6d99f476d5682b6891b444a67c8686 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.Equals(Inno.Native.ImGui.ImRectPtr) 414f3ec7f31e4ee4a7f9af51a6a68790618f54ed8f124f0ecd1605cb80242ab0 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.Equals(System.Object) d198e952672990791adbcab03bb47669017720ef43d3c1df240db0bcd84ecee4 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.GetHashCode 5833a01664bd26b61e2fedcfc4e2d530ec0d421154f00afeeed0e070bc8b8ec1 -->
<!-- api: P:Inno.Native.ImGui.ImRectPtr.Min fe71f40cb7da50d3e8b84516b5af535e80228e033ac20194c65cd0b662116767 -->
<!-- api: P:Inno.Native.ImGui.ImRectPtr.Max b6748c4ec49d47b976fbd5e552a808a5d8eca64faa6d9971cbc21ed691df83d0 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.Destroy b42977ced55944a88c772aecc3b69dda466fbbd7ceafe3b1f248da71991b9c2e -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.GetCenter e48d7e49d9f8c3e99f756b8735fc14cc9a9f2d00e28e7c8716e8d1dc1f3c67c2 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.GetSize c87192b5bfd5c111c192288aebbf9b168b65ffede204627c2fb46c83e4829bf9 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.GetWidth d423d6df274c4bd26844232d9fe0ea5ea519f40fe2073ccfe2733b0cd8a722d8 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.GetHeight 23ed8e86b217a5f6ed78da1dc0ff452f24e4e5e21036bc6ccaeadeaddf9a69ea -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.GetArea ed15870649acdc3d80ceebfd89c6ffe958fa83c4608a24f6874558b232d17b12 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.GetTL eee82a1f15ab25793530c589de66396621dddb1b065a8f9b7a8c31ac2902df8f -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.GetTR 006daa296bcac348395b945dedb38b663af8ef7937cee6fd191175f4f6c71e7a -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.GetBL 4bea31d6d8080d424fff84d86bf5c026321c4cfb8cf0ea3d030a068c6888c8a7 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.GetBR ad626b45a2b89029f99c7abcdb7851f9819015023d01e5c965b6d4bdfe8a4626 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.Contains(System.Numerics.Vector2) 2f2b21ba671e2b5de3108aef1f54d23c77b7dfa9516876ad3d1ac29b0e77af42 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.Contains(Inno.Native.ImGui.ImRect) f31f33a93aaea0a71b457bc0d0837a99fff435d3bc92b1625e8ebe5348b665da -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.ContainsWithPad(System.Numerics.Vector2,System.Numerics.Vector2) 4cd66986d0b699a84104a459e45637eb3d638c0a1c01de935bff05d8a2766ec9 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.Overlaps(Inno.Native.ImGui.ImRect) 5f67ff87061fc86431a942d07d0f7778a04855b4b4f347f915f2d0491b754db2 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.Add(System.Numerics.Vector2) 65afd079559480888b11da051d4258461c62cabcdf780a19501ef754e4e6d0bf -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.Add(Inno.Native.ImGui.ImRect) b20ef13b3e86fbae53c6dbca294103d36e035e1b7656ff7c8e43ceaaf298abd9 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.Expand(System.Single) d6c793c2d305c491d1cefcdce796b55efd90fe011f9aab822473b0f708b64b0b -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.Expand(System.Numerics.Vector2) 75ff5f4615d72efeb61fe581a423905b019a0f4cd7908230c2639055da0d32b4 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.Translate(System.Numerics.Vector2) d1e58bc8ef49cbb1dbff1f92ccc7d12a02c367aa1c0b3f8b985ce1397e5ca879 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.TranslateX(System.Single) 2801482cc71600d59726f5e2635a7679fcc17e8176609c8eed4c3042cc8ee264 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.TranslateY(System.Single) 98fddf0dda742c93efb5315140452a2d34fb19f7de9492d51cbccf13019b587a -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.ClipWith(Inno.Native.ImGui.ImRect) 8b74d47ff12fdd99ab63371704f8c0c9ad7def5eacc8464c9854a79749aa9ddf -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.ClipWithFull(Inno.Native.ImGui.ImRect) 802a5df17e80992445127300076d60388b7d4a2dc9af4e6351f1116749f768a7 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.IsInverted 949c97142f69e7ce846407e99daa1fe2445a1b43424a5873c5e517fecbcc013c -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.ToVec4 209228c0f7937698c059cec7440d0e95472476d30aa127cc9eae089f56ba5457 -->
<!-- api: M:Inno.Native.ImGui.ImRectPtr.AsVec4 7a18df22c81ae0e99bf457a80c34b073329f073926a1d3a6955403d745e5da78 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0246d19a05016427"></a>[`public Inno.Native.ImGui.ImRect* Inno.Native.ImGui.ImRectPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18228) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3c9ee7470e440908"></a>[`public Inno.Native.ImGui.ImRectPtr.ImRectPtr(Inno.Native.ImGui.ImRect* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18223) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-147ffab1631d8651"></a>[`public void Inno.Native.ImGui.ImRectPtr.Add(Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18368) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eb812d2df3075a09"></a>[`public void Inno.Native.ImGui.ImRectPtr.Add(System.Numerics.Vector2 p)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18360) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-457590c50c210590"></a>[`public System.Numerics.Vector4* Inno.Native.ImGui.ImRectPtr.AsVec4()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18448) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-081b4c085621c8de"></a>[`public void Inno.Native.ImGui.ImRectPtr.ClipWith(Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18416) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ac96c1318f42daa2"></a>[`public void Inno.Native.ImGui.ImRectPtr.ClipWithFull(Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18424) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8f9ca87d6dd1731a"></a>[`public bool Inno.Native.ImGui.ImRectPtr.Contains(Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18336) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7cc15a5b4bed7031"></a>[`public bool Inno.Native.ImGui.ImRectPtr.Contains(System.Numerics.Vector2 p)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18328) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-35cad9805dc160cb"></a>[`public bool Inno.Native.ImGui.ImRectPtr.ContainsWithPad(System.Numerics.Vector2 p, System.Numerics.Vector2 pad)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18344) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b37d30d334214b23"></a>[`public void Inno.Native.ImGui.ImRectPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18248) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d2bce5ecdf0a9083"></a>[`public bool Inno.Native.ImGui.ImRectPtr.Equals(Inno.Native.ImGui.ImRectPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18238) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-44d03ea084461abd"></a>[`public override bool Inno.Native.ImGui.ImRectPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18239) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-aa04006578169bfe"></a>[`public void Inno.Native.ImGui.ImRectPtr.Expand(System.Numerics.Vector2 amount)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18384) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b57c38e6d0c8f5b0"></a>[`public void Inno.Native.ImGui.ImRectPtr.Expand(float amount)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18376) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3907e1a738a50982"></a>[`public float Inno.Native.ImGui.ImRectPtr.GetArea()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18288) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7516ff604c60f45f"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRectPtr.GetBL()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18312) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4e4c22a02b862637"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRectPtr.GetBR()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18320) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ea82b46b7d19fa4d"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRectPtr.GetCenter()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18256) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7e6604f8a253230f"></a>[`public override int Inno.Native.ImGui.ImRectPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18240) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-5a631c4b62d19696"></a>[`public float Inno.Native.ImGui.ImRectPtr.GetHeight()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18280) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-299c7023378621a4"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRectPtr.GetSize()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18264) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1a85d74e643f7d97"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRectPtr.GetTL()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18296) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-37670f29f8930247"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImRectPtr.GetTR()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18304) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dfb23baa9e5a77e9"></a>[`public float Inno.Native.ImGui.ImRectPtr.GetWidth()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18272) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7898cb6227404edd"></a>[`public bool Inno.Native.ImGui.ImRectPtr.IsInverted()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18432) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-acb0f3d794f615e0"></a>[`public static bool Inno.Native.ImGui.ImRectPtr.operator ==(Inno.Native.ImGui.ImRectPtr left, Inno.Native.ImGui.ImRectPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18236) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8884cd837650c471"></a>[`public static Inno.Native.ImGui.ImRectPtr.implicit operator Inno.Native.ImGui.ImRectPtr(Inno.Native.ImGui.ImRect* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18234) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1fa35e12b3256995"></a>[`public static Inno.Native.ImGui.ImRectPtr.implicit operator Inno.Native.ImGui.ImRect*(Inno.Native.ImGui.ImRectPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18235) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-159474dca3b6aa16"></a>[`public static bool Inno.Native.ImGui.ImRectPtr.operator !=(Inno.Native.ImGui.ImRectPtr left, Inno.Native.ImGui.ImRectPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18237) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-195a2496cd7b928b"></a>[`public bool Inno.Native.ImGui.ImRectPtr.Overlaps(Inno.Native.ImGui.ImRect r)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18352) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-115638cfc25e2d24"></a>[`public System.Numerics.Vector4 Inno.Native.ImGui.ImRectPtr.ToVec4()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18440) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cbad40a77714a8a9"></a>[`public void Inno.Native.ImGui.ImRectPtr.Translate(System.Numerics.Vector2 d)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18392) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6c478ff4173ffb6a"></a>[`public void Inno.Native.ImGui.ImRectPtr.TranslateX(float dx)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18400) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-92dc3e4419feb727"></a>[`public void Inno.Native.ImGui.ImRectPtr.TranslateY(float dy)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18408) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f1de2565015a6bc6"></a>[`public bool Inno.Native.ImGui.ImRectPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18229) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1ef1f7ec1211f053"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImRectPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18232) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ef2bddb4c8c1ad5"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImRectPtr.Max { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18243) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fcf95775654e9f92"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImRectPtr.Min { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18242) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fa609bda27830cf0"></a>[`public static Inno.Native.ImGui.ImRectPtr Inno.Native.ImGui.ImRectPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18230) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-217c466038e9e161"></a>[`public struct Inno.Native.ImGui.ImRectPtr : System.IEquatable<Inno.Native.ImGui.ImRectPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18221) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImSpanImGuiTableCellData</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImSpanImGuiTableCellData a38e4b4885866c8bf10f218e1cd2c8a2303693c3ce58477b2e5d236a78ab2b13 -->
<!-- api: F:Inno.Native.ImGui.ImSpanImGuiTableCellData.Data 0b39db61dba6416e9590fce74a52071ffe05e78d8a9a886982e6c6dd9305b871 -->
<!-- api: F:Inno.Native.ImGui.ImSpanImGuiTableCellData.DataEnd ed5b7c366ff160a595be6ffef8c887db8041a4b04e58b8c299bdaa47fe22b625 -->
<!-- api: M:Inno.Native.ImGui.ImSpanImGuiTableCellData.#ctor(Inno.Native.ImGui.ImGuiTableCellData*,Inno.Native.ImGui.ImGuiTableCellData*) 602c0b46204ba28ea19a8146eb75377de72218700709753e38e7c9377159755c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e1f680cd0923443f"></a>[`public Inno.Native.ImGui.ImGuiTableCellData* Inno.Native.ImGui.ImSpanImGuiTableCellData.Data`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6286) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1a9e88af198c1f13"></a>[`public Inno.Native.ImGui.ImGuiTableCellData* Inno.Native.ImGui.ImSpanImGuiTableCellData.DataEnd`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6287) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d0f2a930071fe380"></a>[`public Inno.Native.ImGui.ImSpanImGuiTableCellData.ImSpanImGuiTableCellData(Inno.Native.ImGui.ImGuiTableCellData* data = null, Inno.Native.ImGui.ImGuiTableCellData* dataEnd = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6288) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-365060a45dcd6fd6"></a>[`public struct Inno.Native.ImGui.ImSpanImGuiTableCellData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6284) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImSpanImGuiTableColumn</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImSpanImGuiTableColumn 0955bd2e3e86b5914ef14ea095d64441e3c15c43294b6bd1b327250b948fa15a -->
<!-- api: F:Inno.Native.ImGui.ImSpanImGuiTableColumn.Data 3e4c9652029ca6a15a3aeab51b65eff290508ee935a0f6c275e26f88fe063c7b -->
<!-- api: F:Inno.Native.ImGui.ImSpanImGuiTableColumn.DataEnd 3491f8ccdfac8dc527495b1cfcb8a1802084bf529de9a4ac65317a41698e829e -->
<!-- api: M:Inno.Native.ImGui.ImSpanImGuiTableColumn.#ctor(Inno.Native.ImGui.ImGuiTableColumn*,Inno.Native.ImGui.ImGuiTableColumn*) a0bb4f363d38acc8055a8a2b0fcbb1598f555fa35781325ee391c7844ff140c2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7b33d0db393ab999"></a>[`public Inno.Native.ImGui.ImGuiTableColumn* Inno.Native.ImGui.ImSpanImGuiTableColumn.Data`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6241) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4a1299d2836b6149"></a>[`public Inno.Native.ImGui.ImGuiTableColumn* Inno.Native.ImGui.ImSpanImGuiTableColumn.DataEnd`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6242) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfe1fc55c58bebe3"></a>[`public Inno.Native.ImGui.ImSpanImGuiTableColumn.ImSpanImGuiTableColumn(Inno.Native.ImGui.ImGuiTableColumn* data = null, Inno.Native.ImGui.ImGuiTableColumn* dataEnd = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6243) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4150ae9b790c6e9d"></a>[`public struct Inno.Native.ImGui.ImSpanImGuiTableColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6239) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImSpanImGuiTableColumnIdx</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImSpanImGuiTableColumnIdx 76e19a0b50f9ec936b3e1fc13c17bdbddaff00daba2382710c95cef6db1b1e44 -->
<!-- api: F:Inno.Native.ImGui.ImSpanImGuiTableColumnIdx.Data 107d583ed0a651e586239d58b85e3ea9c7b8178ad4c6140d99b0a6b045115b03 -->
<!-- api: F:Inno.Native.ImGui.ImSpanImGuiTableColumnIdx.DataEnd e934d87ee62124e1b232bf85e865d557a98521a202755a53c3e26f1662429334 -->
<!-- api: M:Inno.Native.ImGui.ImSpanImGuiTableColumnIdx.#ctor(System.Int16*,System.Int16*) 5df03ecd26ba5d6fcdbc2e8bfa9791782d7e5535a04318da229610c3b862f129 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-359093d4ce6e11db"></a>[`public short* Inno.Native.ImGui.ImSpanImGuiTableColumnIdx.Data`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6256) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4ea551f5cf1f88c9"></a>[`public short* Inno.Native.ImGui.ImSpanImGuiTableColumnIdx.DataEnd`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6257) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-43ad8a07b821f719"></a>[`public Inno.Native.ImGui.ImSpanImGuiTableColumnIdx.ImSpanImGuiTableColumnIdx(short* data = null, short* dataEnd = null)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6258) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d1e4b1fd45e7f4e4"></a>[`public struct Inno.Native.ImGui.ImSpanImGuiTableColumnIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6254) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImStableVectorImFontBaked32</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImStableVectorImFontBaked32 f216df248e468acfd958927a26008a8a9c6349f9a05ebb276dc3987dc590f8c5 -->
<!-- api: F:Inno.Native.ImGui.ImStableVectorImFontBaked32.Size dc0e032025c96462d91411f03136da5c3729872d6490db43eb022c9373279c6a -->
<!-- api: F:Inno.Native.ImGui.ImStableVectorImFontBaked32.Capacity eb53414a4e333f28b4c81dd16b6c3e5f257d354be2b92255c5b95ab910cfd017 -->
<!-- api: F:Inno.Native.ImGui.ImStableVectorImFontBaked32.Blocks 6250f7a8a6e155a8e59fac5edc1c0e59f5ad1667836acb02cd9a7e6c4d6118bf -->
<!-- api: M:Inno.Native.ImGui.ImStableVectorImFontBaked32.#ctor(System.Int32,System.Int32,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImFontBakedPtr}) 2de84e96634c422750b661ce2ade8266bf2d50541bdbb1264b10f96049b44fde -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-68a9ef6afff9eade"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontBakedPtr> Inno.Native.ImGui.ImStableVectorImFontBaked32.Blocks`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2423) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f503c01858f2c9dc"></a>[`public int Inno.Native.ImGui.ImStableVectorImFontBaked32.Capacity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2422) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d814b693cde40b72"></a>[`public int Inno.Native.ImGui.ImStableVectorImFontBaked32.Size`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2421) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a66b3c5767772c30"></a>[`public Inno.Native.ImGui.ImStableVectorImFontBaked32.ImStableVectorImFontBaked32(int size = 0, int capacity = 0, Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontBakedPtr> blocks = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImFontBakedPtr>))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2424) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6e301fd2c8b8b553"></a>[`public struct Inno.Native.ImGui.ImStableVectorImFontBaked32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L2419) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImTextureData</code> · 29 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImTextureData d1d7b7724ee99eb38ab7182ee91193db613c67d3f9e3f62a86344d1f94a8dd78 -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.UniqueID 7f9908c7e21132870c4c91aa641287acaa14f8bfa2ba91df89981606b422727e -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.Status f8654a5dfa4853dcf2b9fb78e3e8748de89624126c7709fcde85d7b596ca86b4 -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.BackendUserData b9e50ae002260f7a594ec12790fb6e895fe66a89dee1b0236f5807116bfa7809 -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.TexID f716b79ff7ace801368c90fafc81d5c91b54aea4333a7644b99cc70dbba80541 -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.Format 28e445bc0cb19bb524b46b2e8d7da2afe922722716fd2684fd7b98c313c1dcf9 -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.Width 4df65c579183a05fe1ef61c14dbf22f2f3a00f923b2a410e9e9df7e844a1beef -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.Height 4b7bf15c50c6009d9cbdeb06da1899555e8f2770ad2090fabcd17a06769d9309 -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.BytesPerPixel 163e0b4a1277ee6a9cf6bf227c44085537651a23a2b7e3e2bc42af06982530bb -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.Pixels 3276c089a98c3bf56eaf382f9b8e8fe6f102817dd24d3730ac5c97943090fc16 -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.UsedRect a56b521f0bca057e0c3e4da6d7e3a6f4ef6f2f309058ba992e9b1c13e9995074 -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.UpdateRect fc261cac5aeb898cf6c9386f46365dcc6adee8491c0b7ba2f982854bca876644 -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.Updates 62a974be3ffea1f803542ff450ab4862db0f8b7988b7fe2f4a714824a4dfccf3 -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.UnusedFrames 87bc3a49a1bfcf8d37d620e526bd0a8ef7a69f9f4f33024204b8a9abaeb968df -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.RefCount 605c99817d02f9f037e1c278b0532328a006d72d7363fa8fe45456d338e1b496 -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.UseColors 3ea4c025f294f63d2bf0f9d141cbb233d7fcb719225c5a22848de5ea873b5513 -->
<!-- api: F:Inno.Native.ImGui.ImTextureData.WantDestroyNextFrame 293fbc9ea5af9a221e130d615f410b76123d0fd7545132431e4d12f5a62331f5 -->
<!-- api: M:Inno.Native.ImGui.ImTextureData.#ctor(System.Int32,Inno.Native.ImGui.ImTextureStatus,System.Void*,Inno.Native.ImGui.ImTextureID,Inno.Native.ImGui.ImTextureFormat,System.Int32,System.Int32,System.Int32,System.Byte*,Inno.Native.ImGui.ImTextureRect,Inno.Native.ImGui.ImTextureRect,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImTextureRect},System.Int32,System.UInt16,System.Byte,System.Byte) 9460cc725c6b73313058d5e429e4a02ff1838f3c41087052c85ac1b53499f694 -->
<!-- api: M:Inno.Native.ImGui.ImTextureData.Destroy 07be9ae23ff637d3e90394d4909dced81174bcd1ca88930fdf76681b8bb44aa6 -->
<!-- api: M:Inno.Native.ImGui.ImTextureData.Create(Inno.Native.ImGui.ImTextureFormat,System.Int32,System.Int32) 4b8eeb2df9fef7bef00d831a5857d6aa4f3a1541855149dde38774a04c27e101 -->
<!-- api: M:Inno.Native.ImGui.ImTextureData.DestroyPixels cf2bdc16fabd1b38027ac5ba90263f302c3e26775d374418580038ad722b9bee -->
<!-- api: M:Inno.Native.ImGui.ImTextureData.GetPixels 04cfb8a64f2aec39624b744c981ce355d093c030bb92245c54680d2c4e21250b -->
<!-- api: M:Inno.Native.ImGui.ImTextureData.GetPixelsAt(System.Int32,System.Int32) 300f50849a5f7b2585348a8b574ddf8cf2a2901dfe9eba4fda1bb83978ee7aa9 -->
<!-- api: M:Inno.Native.ImGui.ImTextureData.GetSizeInBytes a120e93cb19b581763138856fb5f2c0a6540dfb322a7316bcfcde32c0546bf0d -->
<!-- api: M:Inno.Native.ImGui.ImTextureData.GetPitch d50489a540385a727fc6ad577c3cc2ed4db12f0fe820eba035ebe89773ec8889 -->
<!-- api: M:Inno.Native.ImGui.ImTextureData.GetTexRef 20b8c32978089e14e481607a33cf0936ace6df2dcd0bfcf5adb0307308377951 -->
<!-- api: M:Inno.Native.ImGui.ImTextureData.GetTexID ddec4a885f4d9e2a8af7d867aafd0c2bc4610692e293258787d97aee4776bb6b -->
<!-- api: M:Inno.Native.ImGui.ImTextureData.SetTexID(Inno.Native.ImGui.ImTextureID) 49c0fe98accbd9ed2b3359a72cd198074d8f8d126fa9c445c0e6d82db824603b -->
<!-- api: M:Inno.Native.ImGui.ImTextureData.SetStatus(Inno.Native.ImGui.ImTextureStatus) 1f4c93e5b3e3384be09e34e40b864d945bdab1308387d92ff9468c3651718226 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-69a8d346b9589245"></a>[`public void* Inno.Native.ImGui.ImTextureData.BackendUserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1725) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-603ac07f21698e36"></a>[`public int Inno.Native.ImGui.ImTextureData.BytesPerPixel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1730) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d32feed64748ebdf"></a>[`public Inno.Native.ImGui.ImTextureFormat Inno.Native.ImGui.ImTextureData.Format`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1727) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bd459eb180f0fde8"></a>[`public int Inno.Native.ImGui.ImTextureData.Height`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1729) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-30d0602ca1bd7992"></a>[`public byte* Inno.Native.ImGui.ImTextureData.Pixels`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1731) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-33d457049a60e22f"></a>[`public ushort Inno.Native.ImGui.ImTextureData.RefCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1736) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e3fd6a844b3534a8"></a>[`public Inno.Native.ImGui.ImTextureStatus Inno.Native.ImGui.ImTextureData.Status`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1724) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07b10ce85f194407"></a>[`public Inno.Native.ImGui.ImTextureID Inno.Native.ImGui.ImTextureData.TexID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1726) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-34780ec5555ea24c"></a>[`public int Inno.Native.ImGui.ImTextureData.UniqueID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1723) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d9e8d08fb4c9091b"></a>[`public int Inno.Native.ImGui.ImTextureData.UnusedFrames`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1735) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0db052624b003287"></a>[`public Inno.Native.ImGui.ImTextureRect Inno.Native.ImGui.ImTextureData.UpdateRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1733) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-86e8b0fc9ed0e3d0"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureRect> Inno.Native.ImGui.ImTextureData.Updates`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1734) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f25f2374c9fe7c15"></a>[`public byte Inno.Native.ImGui.ImTextureData.UseColors`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1737) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe583fd6640ff524"></a>[`public Inno.Native.ImGui.ImTextureRect Inno.Native.ImGui.ImTextureData.UsedRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1732) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da0c6fdc5a9bea20"></a>[`public byte Inno.Native.ImGui.ImTextureData.WantDestroyNextFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1738) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec2049fa7c4cca12"></a>[`public int Inno.Native.ImGui.ImTextureData.Width`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1728) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ec22b200bf0abe6"></a>[`public Inno.Native.ImGui.ImTextureData.ImTextureData(int uniqueID = 0, Inno.Native.ImGui.ImTextureStatus status = Inno.Native.ImGui.ImTextureStatus.Ok, void* backendUserData = null, Inno.Native.ImGui.ImTextureID texID = default(Inno.Native.ImGui.ImTextureID), Inno.Native.ImGui.ImTextureFormat format = Inno.Native.ImGui.ImTextureFormat.Rgba32, int width = 0, int height = 0, int bytesPerPixel = 0, byte* pixels = null, Inno.Native.ImGui.ImTextureRect usedRect = default(Inno.Native.ImGui.ImTextureRect), Inno.Native.ImGui.ImTextureRect updateRect = default(Inno.Native.ImGui.ImTextureRect), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureRect> updates = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureRect>), int unusedFrames = 0, ushort refCount = 0, byte useColors = 0, byte wantDestroyNextFrame = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1739) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3015c6c9be42c108"></a>[`public void Inno.Native.ImGui.ImTextureData.Create(Inno.Native.ImGui.ImTextureFormat format, int w, int h)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65171) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-153e6de1a0541e0e"></a>[`public void Inno.Native.ImGui.ImTextureData.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65160) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b61956139b87c9ca"></a>[`public void Inno.Native.ImGui.ImTextureData.DestroyPixels()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65182) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-04a229a455a340ac"></a>[`public int Inno.Native.ImGui.ImTextureData.GetPitch()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65226) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-39b909b6aa9ba337"></a>[`public void* Inno.Native.ImGui.ImTextureData.GetPixels()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65193) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a6ab180ebe57daaf"></a>[`public void* Inno.Native.ImGui.ImTextureData.GetPixelsAt(int x, int y)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65204) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-aed27110efad0486"></a>[`public int Inno.Native.ImGui.ImTextureData.GetSizeInBytes()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65215) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-022ce560148c6fea"></a>[`public Inno.Native.ImGui.ImTextureID Inno.Native.ImGui.ImTextureData.GetTexID()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65248) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e5ea3dc25fea9737"></a>[`public Inno.Native.ImGui.ImTextureRef Inno.Native.ImGui.ImTextureData.GetTexRef()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65237) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7370f6fef0c790d1"></a>[`public void Inno.Native.ImGui.ImTextureData.SetStatus(Inno.Native.ImGui.ImTextureStatus status)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65270) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe7425b6bab69a9e"></a>[`public void Inno.Native.ImGui.ImTextureData.SetTexID(Inno.Native.ImGui.ImTextureID texId)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L65259) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-30bb8fe888b12e65"></a>[`public struct Inno.Native.ImGui.ImTextureData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1721) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImTextureDataPtr</code> · 40 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImTextureDataPtr 8caba8336f67d602c481a3c43a4bf90e22426058698a8855350f01c78d9c3af1 -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.#ctor(Inno.Native.ImGui.ImTextureData*) 7bc4574323a04649f0668c9d56e28f1462d1ca44a1d79950ad262341ec57f15b -->
<!-- api: F:Inno.Native.ImGui.ImTextureDataPtr.Handle 1737294e12eb61d1d9cae4c972e2a965a1779d860d7db395f094f66f913f3a73 -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.IsNull 35ddb5d0a54b7f78e7507746a56df5c33d3db8dde1922904f9c99b4438aa455c -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.Null 57f2ac7ea72d78edd2b2d37cc3aa1f83c13318a78b0acee1979534335a2ea402 -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.Item(System.Int32) fb42ccc87eccb9c1ac56e339fb9f6f52961244b2f9cc6ea6c4bcea5e9f47a7b7 -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.op_Implicit(Inno.Native.ImGui.ImTextureData*)~Inno.Native.ImGui.ImTextureDataPtr ec5b464555678b5d462dc313a184e6008d50d805bc35583ad5d7db0730000746 -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.op_Implicit(Inno.Native.ImGui.ImTextureDataPtr)~Inno.Native.ImGui.ImTextureData* 174224efbd9c6360cf6888b0feb001699667734689b5ca4ad0b0c8430166c16f -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.op_Equality(Inno.Native.ImGui.ImTextureDataPtr,Inno.Native.ImGui.ImTextureDataPtr) 58b57d5ab4a547052bf3677dca2a40f0a9bdef9f8ed229f362656145933e4627 -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.op_Inequality(Inno.Native.ImGui.ImTextureDataPtr,Inno.Native.ImGui.ImTextureDataPtr) 055333f0d60a5b09dac74c387a76ebb48b5a70fa160e7aa62082ff8866315fe2 -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.Equals(Inno.Native.ImGui.ImTextureDataPtr) 6a37a5b7fda58b1382c71809d0bcf10bf0785bb0972ec5d540168a062030fe8b -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.Equals(System.Object) 3e3c4a86543305deb48f43f15b40bcb80c7ab3ee8eb338e86564fa281e383d5b -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.GetHashCode c6e373601158fca16e789c362db5e6794b8646b6a1ed58a94c997f49c7d3672a -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.UniqueID 0feaefee0c5b9124815aca99887b1799c6d724c22a28bdef2b6ca2d05267c987 -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.Status 180625477510c3ea4e27bbe9962a6c2fa0197ae9e5fd85881b90496e4ae0492b -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.BackendUserData 6c4450221f62ea00811f81ba1e9e163b0e09befc750ef384b8c4f1710d8a3cf2 -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.TexID 33b4581f2367a7750c7ac9f78f527af051b315e31184f02146b4482cd247cf24 -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.Format 2330372392cf6ccb4c7c0e84955f1f9b521d6e867014d697b5606607a3e04898 -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.Width b9dfb5146f2e0fa7cbadd2978038b2c6f327464ab97145afb930132bb4b058b5 -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.Height 33dd0478ba4814637efcdb0b1b6256236c918b8b878a70eaac263c9592a95c76 -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.BytesPerPixel e7a9de4a0ce0de32fcfa7ec7d2f116e6cfabda465319538d01a0e259441becfe -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.Pixels 5fba84ba07b6da0b629536327e73f0e07a8eac6b08f6bb5965dc199a08f3a410 -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.UsedRect 4655d261355449aec0464307a4a3c011a6467bcfb00e10c0dacee92186d3a64d -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.UpdateRect b1ce10839ea9b675ab675c9cdc05153d8794d1735f6dc1314f8df49254b19306 -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.Updates b38fc3998c12eec2ea487e413b2c0a0b9791cf3d2f6df370507c9d4ca68ecc22 -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.UnusedFrames 7e07feb46e006275aa57888ed9e8ec0d366d3cb61b975c42a0b7f1cede1e65f3 -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.RefCount 6dc209ed12b601830f3637e8e5b8a5d6361d5da54ccedb87a23c246f9fac005f -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.UseColors 981daabaf45117d8b472120f6d13d32862c6f0519f2ccb10f3c6632330dd25a9 -->
<!-- api: P:Inno.Native.ImGui.ImTextureDataPtr.WantDestroyNextFrame a8cfbfd757aa887fdb71747b30edc4c6a6dd1dc90749eada5ac8c52705c7fe12 -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.Destroy f41cd14513875e83b61e61c1daea4751f8050f2ae3d3bb89675a9b871e07fe37 -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.Create(Inno.Native.ImGui.ImTextureFormat,System.Int32,System.Int32) 5918985a785b3201e52313a17d2c491ce8ba9e73229a2b0cd2c854205b68c8ba -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.DestroyPixels f68893444e389755a72ca3ea1c802b4de3a0965ed0b09c4b9cd3816f32a63b93 -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.GetPixels e6b305f8ec9e673b628566669ab93a30ce0a2ea81f4f3f801fc0b1795c8d36e1 -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.GetPixelsAt(System.Int32,System.Int32) 4a576d027b7760b58950cd07a550bac6942da0f4b1197e860013efae168b2c21 -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.GetSizeInBytes 520973da35742439eb65956bc960458daa883720578cc59226c8aca36d578237 -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.GetPitch 60a6ba961e09c4cd2397701259c235f79de97210bca0329d96e4c35d78b449a1 -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.GetTexRef 0ebae06f3f7ea26d2b77732244d3443845ace07db705ffb1808461b60b53a5cd -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.GetTexID bc64536474fbe0bd4acfb7efc9a938be970243df15e53e2fb7b247fd0cdedd67 -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.SetTexID(Inno.Native.ImGui.ImTextureID) 8307e9217c5ab27b9ce3523a9a742eebeee8fe34596f585b7e7d4e714143842a -->
<!-- api: M:Inno.Native.ImGui.ImTextureDataPtr.SetStatus(Inno.Native.ImGui.ImTextureStatus) 0a8a87b90303a259946b6545eca0852591d65cadb0fd3595b7877d5d1e8ff5ac -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e717129f310aeac7"></a>[`public Inno.Native.ImGui.ImTextureData* Inno.Native.ImGui.ImTextureDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18462) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-752e1fc479f327ea"></a>[`public Inno.Native.ImGui.ImTextureDataPtr.ImTextureDataPtr(Inno.Native.ImGui.ImTextureData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18457) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a7a2c802afd8ac0d"></a>[`public void Inno.Native.ImGui.ImTextureDataPtr.Create(Inno.Native.ImGui.ImTextureFormat format, int w, int h)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18504) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f4a7fec06d093762"></a>[`public void Inno.Native.ImGui.ImTextureDataPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18496) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-771a60d71a9ae841"></a>[`public void Inno.Native.ImGui.ImTextureDataPtr.DestroyPixels()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18512) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-06b401206036aaed"></a>[`public bool Inno.Native.ImGui.ImTextureDataPtr.Equals(Inno.Native.ImGui.ImTextureDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18472) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-8a1563673b6c7e55"></a>[`public override bool Inno.Native.ImGui.ImTextureDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18473) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-932cf419495dfcd5"></a>[`public override int Inno.Native.ImGui.ImTextureDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18474) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-5b80fa6850325e0d"></a>[`public int Inno.Native.ImGui.ImTextureDataPtr.GetPitch()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18544) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-601be2aee803d9c9"></a>[`public void* Inno.Native.ImGui.ImTextureDataPtr.GetPixels()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18520) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fb6b17aa994276f5"></a>[`public void* Inno.Native.ImGui.ImTextureDataPtr.GetPixelsAt(int x, int y)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18528) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-af71583f72ff3cff"></a>[`public int Inno.Native.ImGui.ImTextureDataPtr.GetSizeInBytes()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18536) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8d1272b3f804120f"></a>[`public Inno.Native.ImGui.ImTextureID Inno.Native.ImGui.ImTextureDataPtr.GetTexID()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18560) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bd6520d76ffbbdc1"></a>[`public Inno.Native.ImGui.ImTextureRef Inno.Native.ImGui.ImTextureDataPtr.GetTexRef()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18552) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8376752fec2d4fa6"></a>[`public static bool Inno.Native.ImGui.ImTextureDataPtr.operator ==(Inno.Native.ImGui.ImTextureDataPtr left, Inno.Native.ImGui.ImTextureDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18470) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-14fcbc6b0f6389b2"></a>[`public static Inno.Native.ImGui.ImTextureDataPtr.implicit operator Inno.Native.ImGui.ImTextureDataPtr(Inno.Native.ImGui.ImTextureData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18468) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ba4340e81cb0e4dc"></a>[`public static Inno.Native.ImGui.ImTextureDataPtr.implicit operator Inno.Native.ImGui.ImTextureData*(Inno.Native.ImGui.ImTextureDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18469) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c5d5c272984db48e"></a>[`public static bool Inno.Native.ImGui.ImTextureDataPtr.operator !=(Inno.Native.ImGui.ImTextureDataPtr left, Inno.Native.ImGui.ImTextureDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18471) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5c7a4d81b449c40a"></a>[`public void Inno.Native.ImGui.ImTextureDataPtr.SetStatus(Inno.Native.ImGui.ImTextureStatus status)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18576) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7957ff6fd6cf3470"></a>[`public void Inno.Native.ImGui.ImTextureDataPtr.SetTexID(Inno.Native.ImGui.ImTextureID texId)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18568) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-078862dc004def0d"></a>[`public void* Inno.Native.ImGui.ImTextureDataPtr.BackendUserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18478) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-084dff8f4720139b"></a>[`public ref int Inno.Native.ImGui.ImTextureDataPtr.BytesPerPixel { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18483) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c85611dbfd5ce933"></a>[`public ref Inno.Native.ImGui.ImTextureFormat Inno.Native.ImGui.ImTextureDataPtr.Format { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18480) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-34b62d158080bea3"></a>[`public ref int Inno.Native.ImGui.ImTextureDataPtr.Height { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18482) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7de8f2e24cd1a01b"></a>[`public bool Inno.Native.ImGui.ImTextureDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18463) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3063314678fab02d"></a>[`public Inno.Native.ImGui.ImTextureData Inno.Native.ImGui.ImTextureDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18466) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-87452ed3b22c7cd3"></a>[`public static Inno.Native.ImGui.ImTextureDataPtr Inno.Native.ImGui.ImTextureDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18464) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-01c44045d8f8b1ce"></a>[`public byte* Inno.Native.ImGui.ImTextureDataPtr.Pixels { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18484) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ce500ef28b5a15fa"></a>[`public ref ushort Inno.Native.ImGui.ImTextureDataPtr.RefCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18489) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-aef1bacc867f762b"></a>[`public ref Inno.Native.ImGui.ImTextureStatus Inno.Native.ImGui.ImTextureDataPtr.Status { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18477) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d0b85ff42a512e25"></a>[`public ref Inno.Native.ImGui.ImTextureID Inno.Native.ImGui.ImTextureDataPtr.TexID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18479) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-90831e319c25df02"></a>[`public ref int Inno.Native.ImGui.ImTextureDataPtr.UniqueID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18476) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-170052007a755ee7"></a>[`public ref int Inno.Native.ImGui.ImTextureDataPtr.UnusedFrames { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18488) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a2ebc33806bdb8ff"></a>[`public ref Inno.Native.ImGui.ImTextureRect Inno.Native.ImGui.ImTextureDataPtr.UpdateRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18486) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bb8096180c6eb450"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImTextureRect> Inno.Native.ImGui.ImTextureDataPtr.Updates { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18487) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4d0d541a4f9f73eb"></a>[`public bool Inno.Native.ImGui.ImTextureDataPtr.UseColors { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18490) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2b8751a2f79f8f7a"></a>[`public ref Inno.Native.ImGui.ImTextureRect Inno.Native.ImGui.ImTextureDataPtr.UsedRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18485) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0c9460d6ebdeb861"></a>[`public bool Inno.Native.ImGui.ImTextureDataPtr.WantDestroyNextFrame { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18491) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9d5ce49c0a8c90a8"></a>[`public ref int Inno.Native.ImGui.ImTextureDataPtr.Width { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18481) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f19437862b0ab4a8"></a>[`public struct Inno.Native.ImGui.ImTextureDataPtr : System.IEquatable<Inno.Native.ImGui.ImTextureDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18455) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImTextureFormat</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImTextureFormat 0c199fce66630b4e14d1ddeee7c78a79b8a686356678f8fc2906239626dfe93a -->
<!-- api: F:Inno.Native.ImGui.ImTextureFormat.Rgba32 bafc168d417c49ebe364353c424fd9ddacb769ff86b1c4d6e0984de2a3aa3d30 -->
<!-- api: F:Inno.Native.ImGui.ImTextureFormat.Alpha8 c0b6c835eda8e76ba193fa51ea5181bd1bbbea49e8c1a36db5d2b0563db28cc6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3ab549b6ab9a686b"></a>[`Inno.Native.ImGui.ImTextureFormat.Alpha8 = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L62) | 当前常量或枚举值：`1`。 | — |
| <a id="api-bc1cea0c144c0227"></a>[`Inno.Native.ImGui.ImTextureFormat.Rgba32 = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L61) | 当前常量或枚举值：`0`。 | — |
| <a id="api-11a3a715b0d126d4"></a>[`public enum Inno.Native.ImGui.ImTextureFormat : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L59) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImTextureID</code> · 29 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImTextureID f25c1796c0c8f18155bf2d5fd191e18bd871f104c8732500495d66775e2be8e4 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.#ctor(System.UInt64) 5429fa16b069db8bcc190299d44177b203681a176907771cddfea5108f7e6591 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.#ctor(System.IntPtr) 2dde9043b2dd2f28070966ff5b8e2f8dcc65067d20c7c73c543afae89363ded5 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.#ctor(System.UIntPtr) ad375b5fa7aaec6838bbabe0f99d781c4a0176e3893aa20307fedf3e5f130052 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.#ctor(System.Void*) 278f49a3d97ef547a99b1b8e5cf9435acb876bf50e3a5c170fa99aa63e0cd446 -->
<!-- api: P:Inno.Native.ImGui.ImTextureID.Handle 6806db34e288e3b77b141305c47fb8b136aef4842284bf9649311f5390a05443 -->
<!-- api: P:Inno.Native.ImGui.ImTextureID.IsNull b5eeb512703cc78d103184d7ec7c3db527d10ed1b95ef88dc08eb30dcefbe7cc -->
<!-- api: P:Inno.Native.ImGui.ImTextureID.Null 5a7e52e9dec576e966e71a3da4ab783c253e92d17229fbb8c34bea1587c06a39 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Implicit(System.UInt64)~Inno.Native.ImGui.ImTextureID e4427f0d6e732c0eb65a29bc0e3961360cb2c39b30a0710e0ade58132187fed9 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Implicit(System.IntPtr)~Inno.Native.ImGui.ImTextureID f76ec456dde2e1b92b446daee3139928709d38c2f90414ea4f1b2f38afe3d4ee -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Implicit(System.UIntPtr)~Inno.Native.ImGui.ImTextureID f5056b5e7c56932fdeefab8f9b308db9c6ba564620ee1fc684313e928cbd25ac -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Implicit(System.Void*)~Inno.Native.ImGui.ImTextureID e045aa08b711bc23b631dc345e434b25411c23bfbdc5b9a7cb0a3793d2117e2b -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Implicit(Inno.Native.ImGui.ImTextureID)~System.UInt64 aab3f3d3407bf79c8878dc95d38fd9c1debd5a5dead05a75fa3104ec5689fbf1 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Implicit(Inno.Native.ImGui.ImTextureID)~System.IntPtr ad2cf6651b39cd498c125eb778b2aac685c12af065f4dc11ff9620a3ea943f37 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Implicit(Inno.Native.ImGui.ImTextureID)~System.UIntPtr 08576cbce1d524ebb17e170f686851159ae3f0c34a784b5c414cc3192655c8b8 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Implicit(Inno.Native.ImGui.ImTextureID)~System.Void* 469836462ba0e014d4f86e5b81545fded87ecc6fb0f8b82bc2a83bf9936c733e -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Equality(Inno.Native.ImGui.ImTextureID,Inno.Native.ImGui.ImTextureID) 0bea30b3e007ebd7eb72943cb6f3c9a4f2439bac2fd6b221e30b21da538f3ef2 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Inequality(Inno.Native.ImGui.ImTextureID,Inno.Native.ImGui.ImTextureID) 919dfef65a029cf4d19e6794ae2290b41e855e3748e6757f612494a936b5f6a0 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Equality(Inno.Native.ImGui.ImTextureID,System.UInt64) 9c53176caed2cc20f9a6f43c91b538065cbb10cbeb10a079f18904bba6df9084 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Inequality(Inno.Native.ImGui.ImTextureID,System.UInt64) 0a53a7d954233239dbbcdb2f2d1edc796c76a00c76a573f0135047e20f0455ed -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Equality(Inno.Native.ImGui.ImTextureID,System.IntPtr) e11a4287aaa4094a2ae83da74544ed3a66ab5250a010162f0bb0af46de2e87a5 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Inequality(Inno.Native.ImGui.ImTextureID,System.IntPtr) 5eb67639866f99be253ab1ab85c649c7284de8421ad5c2da3dcbe13070ff811c -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Equality(Inno.Native.ImGui.ImTextureID,System.UIntPtr) fa58e35b3d36f2e0fde5ab328e207872d9653bfd924113b92724d19c2314beaf -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Inequality(Inno.Native.ImGui.ImTextureID,System.UIntPtr) 4beec9c16148028ae410b4c7b41c0467f4cbfef93f128c489e4e800770d9744e -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Equality(Inno.Native.ImGui.ImTextureID,System.Void*) 3582777547b2ee85ed8673f475ffaf173480c35c772238cedc7e142c0196cba0 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.op_Inequality(Inno.Native.ImGui.ImTextureID,System.Void*) 23adf9c31f0f4d4fd2c07824837a42adc459d719ce1c95d0fd93ac756b930202 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.Equals(Inno.Native.ImGui.ImTextureID) e9a5aa26661317d4444e6fcf1470010fc29b16c59e1f8f87d9a50453e075a448 -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.Equals(System.Object) 67e8e0b68bdffe0a91c16358cf50f3a8055198e3f33cbf5d09efdd40704dd85c -->
<!-- api: M:Inno.Native.ImGui.ImTextureID.GetHashCode 454ebbc229f5ed32716a6f4a4fbb3b8234a33567ec225b3cc15602b3a2a2117a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-13115a68ca7bc347"></a>[`public Inno.Native.ImGui.ImTextureID.ImTextureID(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69292) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1070b03f57fd9e43"></a>[`public Inno.Native.ImGui.ImTextureID.ImTextureID(ulong handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69287) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-66a5b8cacb72464c"></a>[`public Inno.Native.ImGui.ImTextureID.ImTextureID(nuint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69297) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1a421f311d6dd285"></a>[`public Inno.Native.ImGui.ImTextureID.ImTextureID(void* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69302) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d5f09bf9b3aaec9c"></a>[`public bool Inno.Native.ImGui.ImTextureID.Equals(Inno.Native.ImGui.ImTextureID other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69329) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-15d07f08aad8022a"></a>[`public override bool Inno.Native.ImGui.ImTextureID.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69331) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-78b276c733e48d0c"></a>[`public override int Inno.Native.ImGui.ImTextureID.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69333) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-3e85591a4a350178"></a>[`public static bool Inno.Native.ImGui.ImTextureID.operator ==(Inno.Native.ImGui.ImTextureID left, Inno.Native.ImGui.ImTextureID right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69319) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e437facf5cebc974"></a>[`public static bool Inno.Native.ImGui.ImTextureID.operator ==(Inno.Native.ImGui.ImTextureID left, nint right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69323) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-216c4d5dd2dfaeec"></a>[`public static bool Inno.Native.ImGui.ImTextureID.operator ==(Inno.Native.ImGui.ImTextureID left, ulong right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69321) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2d96673928f22412"></a>[`public static bool Inno.Native.ImGui.ImTextureID.operator ==(Inno.Native.ImGui.ImTextureID left, nuint right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69325) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a5fe816844d3546c"></a>[`public static bool Inno.Native.ImGui.ImTextureID.operator ==(Inno.Native.ImGui.ImTextureID left, void* right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69327) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b42ce4e9926e6d06"></a>[`public static Inno.Native.ImGui.ImTextureID.implicit operator nint(Inno.Native.ImGui.ImTextureID handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69316) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-54ab3a9cebad9bdf"></a>[`public static Inno.Native.ImGui.ImTextureID.implicit operator ulong(Inno.Native.ImGui.ImTextureID handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69315) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cb416e58d58379c5"></a>[`public static Inno.Native.ImGui.ImTextureID.implicit operator nuint(Inno.Native.ImGui.ImTextureID handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69317) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-abbabf798d8c825c"></a>[`public static Inno.Native.ImGui.ImTextureID.implicit operator void*(Inno.Native.ImGui.ImTextureID handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69318) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-54c81bc94fe83f8b"></a>[`public static Inno.Native.ImGui.ImTextureID.implicit operator Inno.Native.ImGui.ImTextureID(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69312) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9d04f6fae8604343"></a>[`public static Inno.Native.ImGui.ImTextureID.implicit operator Inno.Native.ImGui.ImTextureID(ulong handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69311) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4475603aeea26656"></a>[`public static Inno.Native.ImGui.ImTextureID.implicit operator Inno.Native.ImGui.ImTextureID(nuint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69313) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-301ff09c09a9df16"></a>[`public static Inno.Native.ImGui.ImTextureID.implicit operator Inno.Native.ImGui.ImTextureID(void* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69314) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9b29ca9a98f38eed"></a>[`public static bool Inno.Native.ImGui.ImTextureID.operator !=(Inno.Native.ImGui.ImTextureID left, Inno.Native.ImGui.ImTextureID right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69320) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-978b4ccedae699a9"></a>[`public static bool Inno.Native.ImGui.ImTextureID.operator !=(Inno.Native.ImGui.ImTextureID left, nint right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69324) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-90db1d3b50fd1ebf"></a>[`public static bool Inno.Native.ImGui.ImTextureID.operator !=(Inno.Native.ImGui.ImTextureID left, ulong right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69322) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b72e54a99c5f6d97"></a>[`public static bool Inno.Native.ImGui.ImTextureID.operator !=(Inno.Native.ImGui.ImTextureID left, nuint right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69326) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a0067dc3a1ce4df0"></a>[`public static bool Inno.Native.ImGui.ImTextureID.operator !=(Inno.Native.ImGui.ImTextureID left, void* right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69328) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-42597c37ed993964"></a>[`public ulong Inno.Native.ImGui.ImTextureID.Handle { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69307) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7b79dd36dbd3399a"></a>[`public bool Inno.Native.ImGui.ImTextureID.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69308) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-43128d4773131fbd"></a>[`public static Inno.Native.ImGui.ImTextureID Inno.Native.ImGui.ImTextureID.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69309) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fab089684e97f3ad"></a>[`public readonly struct Inno.Native.ImGui.ImTextureID : System.IEquatable<Inno.Native.ImGui.ImTextureID>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L69285) | Represents a texture identity wrapped by for image drawing. | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImTextureRect</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImTextureRect 517d065040d8bc6586b6b7ef02df1a08d54668303aa227df710750c4da129a15 -->
<!-- api: F:Inno.Native.ImGui.ImTextureRect.X 97d4c864052eecad5b71528d4d036d22681a61a16b3d168d82868c9c61344b0b -->
<!-- api: F:Inno.Native.ImGui.ImTextureRect.Y a1e36f1c85dc204ca4dbd9866c7d6b3c5ed2d30de0626616f0e73363418061dd -->
<!-- api: F:Inno.Native.ImGui.ImTextureRect.W a6f9f1d37677251cc843124eb6eae51e4235f0d2dfedd36a4d62cae98dfe6ac0 -->
<!-- api: F:Inno.Native.ImGui.ImTextureRect.H 9c44e6ab8e93b6200ddf568b254a5106876e24c9054174fb418556eb1483d4e9 -->
<!-- api: M:Inno.Native.ImGui.ImTextureRect.#ctor(System.UInt16,System.UInt16,System.UInt16,System.UInt16) 908223c5299ba78d3e0172b1403aefe72463371599be0120cf48f2329ee1c9f5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-eeafe7cc280fecb6"></a>[`public ushort Inno.Native.ImGui.ImTextureRect.H`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1707) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d9fdc59a98e8ddf3"></a>[`public ushort Inno.Native.ImGui.ImTextureRect.W`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1706) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ee9f96d567fca3e3"></a>[`public ushort Inno.Native.ImGui.ImTextureRect.X`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1704) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a67cd3cffa0b5b2"></a>[`public ushort Inno.Native.ImGui.ImTextureRect.Y`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1705) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7d3062d0cf26623"></a>[`public Inno.Native.ImGui.ImTextureRect.ImTextureRect(ushort x = 0, ushort y = 0, ushort w = 0, ushort h = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1708) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-81a89a9d431968d6"></a>[`public struct Inno.Native.ImGui.ImTextureRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1702) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImTextureRectPtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImTextureRectPtr 5395af5cdb105f47d1543835b364bafa950a6af962c2216e657971f69ed84d92 -->
<!-- api: M:Inno.Native.ImGui.ImTextureRectPtr.#ctor(Inno.Native.ImGui.ImTextureRect*) 9d3d3c05711fc673673216d1e22650c9963024d3ed7ee601197fc069bf81d02a -->
<!-- api: F:Inno.Native.ImGui.ImTextureRectPtr.Handle 6d0ec8859bdada3521724f26a8e6e50852f0b60247a0e71d875a5d0a1bed431f -->
<!-- api: P:Inno.Native.ImGui.ImTextureRectPtr.IsNull f19cfb2f8ab9226902a5c429167f35994b8658cc509f4181b4a5c300a2330572 -->
<!-- api: P:Inno.Native.ImGui.ImTextureRectPtr.Null 4cab2edf570b2b750f92e533bdcf7f53106b0c87c3d92c25b20d3a68985b2f0c -->
<!-- api: P:Inno.Native.ImGui.ImTextureRectPtr.Item(System.Int32) 9f280265092a6f4619949fb4201ac17a324a9c98b90fb5c14eb06dbad58e27aa -->
<!-- api: M:Inno.Native.ImGui.ImTextureRectPtr.op_Implicit(Inno.Native.ImGui.ImTextureRect*)~Inno.Native.ImGui.ImTextureRectPtr 732f0ef6401f563f0fa5c7b2fc5615271f51d5dac1d152b0c2ca3c0460c78047 -->
<!-- api: M:Inno.Native.ImGui.ImTextureRectPtr.op_Implicit(Inno.Native.ImGui.ImTextureRectPtr)~Inno.Native.ImGui.ImTextureRect* 7980a7ad12c751f9e944afcb3ccc6a6a34982db43d2dc17a2d61b4d14e43bbd8 -->
<!-- api: M:Inno.Native.ImGui.ImTextureRectPtr.op_Equality(Inno.Native.ImGui.ImTextureRectPtr,Inno.Native.ImGui.ImTextureRectPtr) 8dd4d881e386dd3c971fcdd1728a009500976abca30c90ad656427f15a9b9e1c -->
<!-- api: M:Inno.Native.ImGui.ImTextureRectPtr.op_Inequality(Inno.Native.ImGui.ImTextureRectPtr,Inno.Native.ImGui.ImTextureRectPtr) fcf544b1cdf92d884e96f1d1be331974ff8718548d1a38aaa5c86ca6f7856728 -->
<!-- api: M:Inno.Native.ImGui.ImTextureRectPtr.Equals(Inno.Native.ImGui.ImTextureRectPtr) ff736fb8d82097b1cba612be5373f9f09387e645814550fa25f71255c976f40c -->
<!-- api: M:Inno.Native.ImGui.ImTextureRectPtr.Equals(System.Object) 9263edba24395d66b7cc9ee641f464e9e0dfb1f37fa859a390ad31a9799bd642 -->
<!-- api: M:Inno.Native.ImGui.ImTextureRectPtr.GetHashCode e4ef97ada0993c8f28963ed7d6bc2ea7145b791649bcf2023a882bdcbf5036dc -->
<!-- api: P:Inno.Native.ImGui.ImTextureRectPtr.X 829de9f852a4ea542f9a09592eb68177a34a9ba9a69be36d2b07663121a40e15 -->
<!-- api: P:Inno.Native.ImGui.ImTextureRectPtr.Y 4cda7aca652f830f9ca7d75a45d591262c5a8c14b374ef0645d493b01450cf82 -->
<!-- api: P:Inno.Native.ImGui.ImTextureRectPtr.W f664c83507998fd0ee8ee1942f2b48d4fb65bac646d836366155939d8a8f3d3e -->
<!-- api: P:Inno.Native.ImGui.ImTextureRectPtr.H b77ade5b277adb15c828e0ce6c1e1efcd0e0b7c91b075e7fede2e1759121e053 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b7541fd0f07ee2ac"></a>[`public Inno.Native.ImGui.ImTextureRect* Inno.Native.ImGui.ImTextureRectPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18590) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b27cade8ebe953af"></a>[`public Inno.Native.ImGui.ImTextureRectPtr.ImTextureRectPtr(Inno.Native.ImGui.ImTextureRect* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18585) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-44d29e5e49435b02"></a>[`public bool Inno.Native.ImGui.ImTextureRectPtr.Equals(Inno.Native.ImGui.ImTextureRectPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18600) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-47c5cc713df698b5"></a>[`public override bool Inno.Native.ImGui.ImTextureRectPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18601) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-14d140c21eb7d234"></a>[`public override int Inno.Native.ImGui.ImTextureRectPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18602) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-c5dcbd4237500fda"></a>[`public static bool Inno.Native.ImGui.ImTextureRectPtr.operator ==(Inno.Native.ImGui.ImTextureRectPtr left, Inno.Native.ImGui.ImTextureRectPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18598) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-87d3269c79613257"></a>[`public static Inno.Native.ImGui.ImTextureRectPtr.implicit operator Inno.Native.ImGui.ImTextureRectPtr(Inno.Native.ImGui.ImTextureRect* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18596) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-15245ef07c69dcb6"></a>[`public static Inno.Native.ImGui.ImTextureRectPtr.implicit operator Inno.Native.ImGui.ImTextureRect*(Inno.Native.ImGui.ImTextureRectPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18597) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-05ff38a03229fd7b"></a>[`public static bool Inno.Native.ImGui.ImTextureRectPtr.operator !=(Inno.Native.ImGui.ImTextureRectPtr left, Inno.Native.ImGui.ImTextureRectPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18599) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b23c7f7fae63488a"></a>[`public ref ushort Inno.Native.ImGui.ImTextureRectPtr.H { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18607) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f5f1bae704da80bc"></a>[`public bool Inno.Native.ImGui.ImTextureRectPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18591) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5084417a3e1197b6"></a>[`public Inno.Native.ImGui.ImTextureRect Inno.Native.ImGui.ImTextureRectPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18594) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dd5b27897e0813e1"></a>[`public static Inno.Native.ImGui.ImTextureRectPtr Inno.Native.ImGui.ImTextureRectPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18592) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0838166cb8c8ec75"></a>[`public ref ushort Inno.Native.ImGui.ImTextureRectPtr.W { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18606) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8f7e8ad67ab05165"></a>[`public ref ushort Inno.Native.ImGui.ImTextureRectPtr.X { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18604) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-12bed79586de2514"></a>[`public ref ushort Inno.Native.ImGui.ImTextureRectPtr.Y { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18605) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b88d58959823fea6"></a>[`public struct Inno.Native.ImGui.ImTextureRectPtr : System.IEquatable<Inno.Native.ImGui.ImTextureRectPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L18583) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImTextureRef</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImTextureRef f2c07013e7eacea026a1047c0fb50f2b16e8bae9fa99fb97b9225df50d26706b -->
<!-- api: F:Inno.Native.ImGui.ImTextureRef.TexData b0b9d25c2803b18c9d388814746780382af4b16b33e4232770a27b4240680af6 -->
<!-- api: F:Inno.Native.ImGui.ImTextureRef.TexID 6580caa0909e574479829044b83bee1caa609b621bb645f86ff17eff076d6298 -->
<!-- api: M:Inno.Native.ImGui.ImTextureRef.#ctor(Inno.Native.ImGui.ImTextureData*,Inno.Native.ImGui.ImTextureID) 5ffd867bccadacc6811f0540c3a479bde96074ee806108162a3910d311b2852f -->
<!-- api: M:Inno.Native.ImGui.ImTextureRef.Destroy 8af6cafa4effa4f000ebcbbf6ea19dd8180e6147e7188cb8b8661a681421bad0 -->
<!-- api: M:Inno.Native.ImGui.ImTextureRef.GetTexID 90f3e3234ef1ba99431bd62dd56dd086a328419897b32768bb29ae0ac606d7eb -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4aa56ef2a7640bb0"></a>[`public Inno.Native.ImGui.ImTextureData* Inno.Native.ImGui.ImTextureRef.TexData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1766) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-399337fbb7465d74"></a>[`public Inno.Native.ImGui.ImTextureID Inno.Native.ImGui.ImTextureRef.TexID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1767) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0c097ec5b8a549c0"></a>[`public Inno.Native.ImGui.ImTextureRef.ImTextureRef(Inno.Native.ImGui.ImTextureData* texData = null, Inno.Native.ImGui.ImTextureID texID = default(Inno.Native.ImGui.ImTextureID))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1768) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b8840f94e62cb40c"></a>[`public void Inno.Native.ImGui.ImTextureRef.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63169) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d906befdf2fc53b1"></a>[`public Inno.Native.ImGui.ImTextureID Inno.Native.ImGui.ImTextureRef.GetTexID()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63180) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3709453182ebfa29"></a>[`public struct Inno.Native.ImGui.ImTextureRef`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1764) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
