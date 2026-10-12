# Inno.Native.ImGui ABI · ImGuiStyleVarInfoPtr → ImGuiTableHeaderData

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImGuiStyleVarInfoPtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiStyleVarInfoPtr 0c722eb5bf27ba40612460068dc674d7e14a99fdae20957808b8e28e86206290 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.#ctor(Inno.Native.ImGui.ImGuiStyleVarInfo*) 2dda76693116325e0b6549793bb34725a2ab1fb9d860ec47aee8d40f858baca7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.Handle 253a30bbcb495272cfc614f4fe7508a649e1a6b2d879ab8539ad4b3b907d7d34 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.IsNull 675d646e3784f46684844a1a2ca25ca6af8c333dbd73d72493dfbe5e8eb1b9c0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.Null 52646826826879439838dcf58b5db6f1830145d32ab2250b0255450732cc2f73 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.Item(System.Int32) 863411cd56647e4f410130d8e715074b3c8693fbf0d664af4397e15299e50620 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.op_Implicit(Inno.Native.ImGui.ImGuiStyleVarInfo*)~Inno.Native.ImGui.ImGuiStyleVarInfoPtr adf3eb990f0c6c4e5a036d02f34e3b4d5e178c8dbeef114686a1d782984382c6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.op_Implicit(Inno.Native.ImGui.ImGuiStyleVarInfoPtr)~Inno.Native.ImGui.ImGuiStyleVarInfo* a2bf6c2a7e5e51025e4d7fee43451d733f1979b715ed15399113efd8fd80ba84 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.op_Equality(Inno.Native.ImGui.ImGuiStyleVarInfoPtr,Inno.Native.ImGui.ImGuiStyleVarInfoPtr) 4247ac74f84f388b9104fe59ce164412c30f7a5dfedc876e0d11b9c9b71eaa30 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.op_Inequality(Inno.Native.ImGui.ImGuiStyleVarInfoPtr,Inno.Native.ImGui.ImGuiStyleVarInfoPtr) d3ec77c912483306247793fcd6b989107addfa5752e5f20e5441ed1f610c406f -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.Equals(Inno.Native.ImGui.ImGuiStyleVarInfoPtr) 84d9d78894683b8b92a270e40656e6f3b887b14121c5c90fc4638d3ebd1a0c22 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.Equals(System.Object) aa1365c983e53d19aaa3d60246837946438641fddc497b817ba115992284dfa2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.GetHashCode 4ecc04e9fe872fd829987c669536071625717f9910881a1c0fb9098f4e098a8d -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.Count 12b6b81aa2fec4125abf76a83dc14515cff8aaa9e358d98de6c3bb7cb6d61529 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.DataType 7267c0de53f57e1d44b24eac49198b1116b1eb3c4fec9847d457b497d381f418 -->
<!-- api: P:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.Offset e3375f5560bf88a3e6ec96db490018653fbb01ed1100ae9f4d3d8eb91273cf73 -->
<!-- api: M:Inno.Native.ImGui.ImGuiStyleVarInfoPtr.GetVarPtr(System.Void*) 3ef6df70f7b337a2ea968734adaa1d137b2e9b20b6e44de1d4eca8aa4c4de4c5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3655c9f055a1567e"></a>[`public Inno.Native.ImGui.ImGuiStyleVarInfo* Inno.Native.ImGui.ImGuiStyleVarInfoPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16589) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9a1184afcdd74c9d"></a>[`public Inno.Native.ImGui.ImGuiStyleVarInfoPtr.ImGuiStyleVarInfoPtr(Inno.Native.ImGui.ImGuiStyleVarInfo* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16584) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-45bd9b7570dbb130"></a>[`public bool Inno.Native.ImGui.ImGuiStyleVarInfoPtr.Equals(Inno.Native.ImGui.ImGuiStyleVarInfoPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16599) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-b6657d7f40b06f26"></a>[`public override bool Inno.Native.ImGui.ImGuiStyleVarInfoPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16600) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-e0c61ef08eaeb04a"></a>[`public override int Inno.Native.ImGui.ImGuiStyleVarInfoPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16601) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-1fdb7df52fbb137b"></a>[`public void* Inno.Native.ImGui.ImGuiStyleVarInfoPtr.GetVarPtr(void* parent)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16610) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-599ef7bcb6d3817e"></a>[`public static bool Inno.Native.ImGui.ImGuiStyleVarInfoPtr.operator ==(Inno.Native.ImGui.ImGuiStyleVarInfoPtr left, Inno.Native.ImGui.ImGuiStyleVarInfoPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16597) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b825eefe14398e4f"></a>[`public static Inno.Native.ImGui.ImGuiStyleVarInfoPtr.implicit operator Inno.Native.ImGui.ImGuiStyleVarInfoPtr(Inno.Native.ImGui.ImGuiStyleVarInfo* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16595) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c7965c4cf3e8bd23"></a>[`public static Inno.Native.ImGui.ImGuiStyleVarInfoPtr.implicit operator Inno.Native.ImGui.ImGuiStyleVarInfo*(Inno.Native.ImGui.ImGuiStyleVarInfoPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16596) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7d72b002123d2737"></a>[`public static bool Inno.Native.ImGui.ImGuiStyleVarInfoPtr.operator !=(Inno.Native.ImGui.ImGuiStyleVarInfoPtr left, Inno.Native.ImGui.ImGuiStyleVarInfoPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16598) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c086d701f29fbeb5"></a>[`public uint Inno.Native.ImGui.ImGuiStyleVarInfoPtr.Count { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16603) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f5845b1d674a2c59"></a>[`public Inno.Native.ImGui.ImGuiDataType Inno.Native.ImGui.ImGuiStyleVarInfoPtr.DataType { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16604) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-92023d5eaf58703b"></a>[`public bool Inno.Native.ImGui.ImGuiStyleVarInfoPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16590) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2b8f39735aaeb876"></a>[`public Inno.Native.ImGui.ImGuiStyleVarInfo Inno.Native.ImGui.ImGuiStyleVarInfoPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16593) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-299debdddf1e107c"></a>[`public static Inno.Native.ImGui.ImGuiStyleVarInfoPtr Inno.Native.ImGui.ImGuiStyleVarInfoPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16591) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-59ccc407d57c477e"></a>[`public uint Inno.Native.ImGui.ImGuiStyleVarInfoPtr.Offset { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16605) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-63fe11989bf6bb9e"></a>[`public struct Inno.Native.ImGui.ImGuiStyleVarInfoPtr : System.IEquatable<Inno.Native.ImGui.ImGuiStyleVarInfoPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16582) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTabBar</code> · 40 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTabBar f03a72512dc634cf950fd33feb347b4cba5b50ab7e2cef6c914f3b3b8b51e0cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.Window f98b7a6ccc054bf382af7b517ed6b5f0948306e286cb9ace891b72e5974f4c94 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.Tabs d1a277938010f61fcf93ef7073c850130252d3413dda94c8cad7ba2278169276 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.Flags de0ba4d45ba18d53f7632cb4307bc579de2c9d797acbbaed9c3075608e26f789 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ID 7bcce30541e6120a2f1786f598d60f983953a7c6b52e7de1fdaa9929c077bbba -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.SelectedTabId 2cf70c281d3e26a95b61fce5df649ef270299a43e4a48b40e5b547c33be8dd93 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.NextSelectedTabId 4e3a842481974832a61e8755585c9f8c406834fb09a80f82895ec2f00dcf6a9a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.NextScrollToTabId 98f7e80123c1c7a8d501d13da15ac7c11881299e000a4740904ea1fcc9197830 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.VisibleTabId acc55895c6d5f12f6cf28e490ecfe72b1a923edd04f01a246c892e3336ef8430 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.CurrFrameVisible cc08c3258f59cb2423e22208d7c7d2a006be8e829d0d200ed370dc2d1c81eec7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.PrevFrameVisible 95da69035a2d071826b33ccd10b018d6c9cc6b949592a537caecba22b65f43e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.BarRect 1c925581390a17f55c0b18f09da0279a7425e4b20a2928cf19f605cf42d024fd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.BarRectPrevWidth dfd50973ea3c5a20bb349d9d054f56021ede92f5d7ac00607bc7f9a19904ed86 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.CurrTabsContentsHeight ed693ac1b0f9f27393eca97b76c193670bfe2b37763359d552be08456cc32ba4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.PrevTabsContentsHeight c7637cec6c40ae986895962b9edcdcac68109aad8c00894994871224efbb3c3e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.WidthAllTabs 2e42c6e9a894973a382ca0e5ebf4b93151c4961b9cd32a408f268958a14b0208 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.WidthAllTabsIdeal 990435f78dd798dcd7f96ae68081f8ac311499c75bbdaf00a8005294346b259b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollingAnim d584af7d0673286b4bd7f852fe604900af243a5be54a17be4f0f85faa3a0e0b1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollingTarget ec0db00b1dde5209e48f6ac206bf9217dfdc7afbd620ea142c5abe251bc5e780 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollingTargetDistToVisibility 17a1422d1c417516f25ff6447c881ec22335521a9fb078551317795a7d11ed7a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollingSpeed 0f12b056e2baf2e03a70eb055c86406383620e9717592841247b9a37ef637ba7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollingRectMinX a9536fae96436de5f8486d764c91596365ed9dd0dc469f52ad79ca0ece38b594 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollingRectMaxX 04503f407a66da3c29cc1740c64542910849994de0d5c5e7b24bdbe5d807c83b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.SeparatorMinX 2041f2c5abfeb9cbe03fa39ea6bd0be0d6dbd1ab8ee3629f8f8db4beb1123d9e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.SeparatorMaxX b37a0567bcdd239b5b584b5e11c072ef053b1552caae3935fc56ac366adb7750 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ReorderRequestTabId fdc8f0d15c659d319e56481f04b4dc3ea600f6b5f7e0260a9b5c2f1406bd9418 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ReorderRequestOffset c18467facf18648d9abc6d7c1e84c6cdcc164d18c66355fc217052eaf407db03 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.BeginCount 8e574ce7dcfb3110384050e24f981f8c70c70878adf19e3404f75a5b8b05ea8b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.WantLayout a6faa73f90086fc10582be811c9d052f5f72cf4032663f68767ca98cd297bb03 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.VisibleTabWasSubmitted 02671cf59902dd9353aabcee9ede6079cd70d0c86a683dbc4e049f3cee82afa0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.TabsAddedNew 938dd0e019440b8a88d800c2d532017f19c76dfa339e597417fa91140021df9f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ScrollButtonEnabled 806689d3c66f0bf8738cf41cce0890df9f781f9024cf38f356885f36b6baccff -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.TabsActiveCount bb3655d8a9a426309bb7dc0eb4190786dfdc6406bdd2066bce8a66280e6743e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.LastTabItemIdx 3e33ea94053994405008cf1820fecd2cb0ddb025d7e5d876c67dfc2b5b3b37b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.ItemSpacingY 18d62ae7a1651b9be5e6460159f9c6fadeb3b38dd7cc8f3a0bb4992004355810 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.FramePadding 4a8509c7b226ecb25cf5a5bb8a65991d5610d77d58d27ab3e8745bf964548b84 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.BackupCursorPos 9b2b1846b15c5b2f08554e1f39bb274fd3a76f865fb3e661cf6eedfa062f35d9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBar.TabsNames 23f1021d3acd4e0b783dd21ada60786fe816dfb3929445dbd7aa24659b4329f6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabBar.#ctor(Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiTabItem},Inno.Native.ImGui.ImGuiTabBarFlags,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.Int32,System.Int32,Inno.Native.ImGui.ImRect,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.UInt32,System.Int16,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Int16,System.Int16,System.Single,System.Numerics.Vector2,System.Numerics.Vector2,Inno.Native.ImGui.ImGuiTextBuffer) ae2aaa8d8f3fc2ddef5327158f4cfa191c4578b98f8439ebff634efd27ef532d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabBar.Destroy 919849cf580a111be63a5658d70086f2f57de8b80fdbbaf521f5b71982ff5fd9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-67b14375a67a1e91"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiTabBar.BackupCursorPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4420) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2a517d8d61a1baa6"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTabBar.BarRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4395) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ce51e903f66d9817"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.BarRectPrevWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4396) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c49473fc23be1d1d"></a>[`public byte Inno.Native.ImGui.ImGuiTabBar.BeginCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4411) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c51ee8b385eabc5e"></a>[`public int Inno.Native.ImGui.ImGuiTabBar.CurrFrameVisible`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4393) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-039ce7855bffd2c8"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.CurrTabsContentsHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4397) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2cbc61f7878b5c8f"></a>[`public Inno.Native.ImGui.ImGuiTabBarFlags Inno.Native.ImGui.ImGuiTabBar.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4387) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0bedc89ac523972a"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiTabBar.FramePadding`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4419) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5220e152c01a7369"></a>[`public uint Inno.Native.ImGui.ImGuiTabBar.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4388) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c09fac665a2d1ec1"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ItemSpacingY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4418) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3c11cac5c36eda80"></a>[`public short Inno.Native.ImGui.ImGuiTabBar.LastTabItemIdx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4417) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2385839a1804fe0c"></a>[`public uint Inno.Native.ImGui.ImGuiTabBar.NextScrollToTabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4391) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cb3ec40cdefa30cb"></a>[`public uint Inno.Native.ImGui.ImGuiTabBar.NextSelectedTabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4390) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e3c900c4c2e96d8"></a>[`public int Inno.Native.ImGui.ImGuiTabBar.PrevFrameVisible`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4394) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a525bdaac392ba95"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.PrevTabsContentsHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4398) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-907cad1ab7055bb4"></a>[`public short Inno.Native.ImGui.ImGuiTabBar.ReorderRequestOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4410) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9e19aba7011f3651"></a>[`public uint Inno.Native.ImGui.ImGuiTabBar.ReorderRequestTabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4409) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-83fa12d722e2b39e"></a>[`public byte Inno.Native.ImGui.ImGuiTabBar.ScrollButtonEnabled`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4415) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39cf5a6035ebf20b"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ScrollingAnim`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4401) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a7c01f9ba2891df3"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ScrollingRectMaxX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4406) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-db80af7daa2f8fe7"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ScrollingRectMinX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4405) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aac19d4dee97d40f"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ScrollingSpeed`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4404) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ddd18d777f53fad1"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ScrollingTarget`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4402) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-58d7ae4aa3cf9280"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.ScrollingTargetDistToVisibility`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4403) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d127544ab5897478"></a>[`public uint Inno.Native.ImGui.ImGuiTabBar.SelectedTabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4389) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c33a08a2857bbfde"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.SeparatorMaxX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4408) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3f2e566a808c1ef8"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.SeparatorMinX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4407) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed1d33c0bd121eda"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTabItem> Inno.Native.ImGui.ImGuiTabBar.Tabs`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4386) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d3666a78cc59473"></a>[`public short Inno.Native.ImGui.ImGuiTabBar.TabsActiveCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4416) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0617a13ef3faef4a"></a>[`public byte Inno.Native.ImGui.ImGuiTabBar.TabsAddedNew`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4414) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad47e53f3e8a2ff5"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiTabBar.TabsNames`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4421) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e396064fd251036e"></a>[`public uint Inno.Native.ImGui.ImGuiTabBar.VisibleTabId`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4392) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1aa4b863d26dcb9"></a>[`public byte Inno.Native.ImGui.ImGuiTabBar.VisibleTabWasSubmitted`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4413) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-525cd689ae7a6e7a"></a>[`public byte Inno.Native.ImGui.ImGuiTabBar.WantLayout`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4412) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-135d10a9f55d3b30"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.WidthAllTabs`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4399) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a7dc12a3973cd09"></a>[`public float Inno.Native.ImGui.ImGuiTabBar.WidthAllTabsIdeal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4400) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e5adb36731f5a305"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiTabBar.Window`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4385) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a9d8da6c9abfb074"></a>[`public Inno.Native.ImGui.ImGuiTabBar.ImGuiTabBar(Inno.Native.ImGui.ImGuiWindow* window = null, Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTabItem> tabs = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTabItem>), Inno.Native.ImGui.ImGuiTabBarFlags flags = Inno.Native.ImGui.ImGuiTabBarFlags.None, uint iD = 0, uint selectedTabId = 0, uint nextSelectedTabId = 0, uint nextScrollToTabId = 0, uint visibleTabId = 0, int currFrameVisible = 0, int prevFrameVisible = 0, Inno.Native.ImGui.ImRect barRect = default(Inno.Native.ImGui.ImRect), float barRectPrevWidth = 0, float currTabsContentsHeight = 0, float prevTabsContentsHeight = 0, float widthAllTabs = 0, float widthAllTabsIdeal = 0, float scrollingAnim = 0, float scrollingTarget = 0, float scrollingTargetDistToVisibility = 0, float scrollingSpeed = 0, float scrollingRectMinX = 0, float scrollingRectMaxX = 0, float separatorMinX = 0, float separatorMaxX = 0, uint reorderRequestTabId = 0, short reorderRequestOffset = 0, byte beginCount = 0, byte wantLayout = 0, byte visibleTabWasSubmitted = 0, byte tabsAddedNew = 0, byte scrollButtonEnabled = 0, short tabsActiveCount = 0, short lastTabItemIdx = 0, float itemSpacingY = 0, System.Numerics.Vector2 framePadding = default(System.Numerics.Vector2), System.Numerics.Vector2 backupCursorPos = default(System.Numerics.Vector2), Inno.Native.ImGui.ImGuiTextBuffer tabsNames = default(Inno.Native.ImGui.ImGuiTextBuffer))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4422) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-61d44f5acdf8b868"></a>[`public void Inno.Native.ImGui.ImGuiTabBar.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67561) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dba2b5b368a1248b"></a>[`public struct Inno.Native.ImGui.ImGuiTabBar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4383) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTabBarFlags</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTabBarFlags 8f5a1c5898f71c52e5cfe0196b27a664697d99bd907a17c4c0128f8535ae535d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlags.None a5a234e4e04e066788d4e2f97550611e4c897713ae486f503496f456aea81701 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlags.Reorderable a28504e8a83d0083b775cc8ffbc90b657899001051cf239f77417874e1781228 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlags.AutoSelectNewTabs b67e546bf04703ea867fe12e3f33ea1eab089af2ee363bb4f5b643fd510cf495 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlags.ListPopupButton fcd6027e2f3fa4a0a202aafd5abb771149377dd639217036d292347150f2d56e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlags.NoCloseWithMiddleMouseButton 11938c01a1c8c22b5892942a0c750d321673204b99065ac5bed21bfad3275e90 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlags.NoTabListScrollingButtons 21398b26976c988559513b22fc5040db3a9e468438713fee9140e31695e4a4be -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlags.NoTooltip 02f4bef4417ab09bd99398f5530e71aa9eece35b6240e943aa3cf304f56fab58 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlags.DrawSelectedOverline 8fdb7a3f2721acf2e9d024b476606b38ae37d446dc7c02be4b6c0cdffee970cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyMixed ae6413108304c300a04fae95c594cb9a74f1d57a6a3cfaacb3fc01dad1582414 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyShrink cbb99fa8ba152f3222d9670b17cc77647775576f080addffddc1ad6dd57eb18d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyScroll 5c0b13fe9bc78a0687adb143ea6386aa2a428c741df80be5522350d4098c3bf0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyMask 09d91bc22d4e82899c1a1cb4273a2de9e7ec22879bfae172d0a4c3121d689293 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyDefault ab79605ebb90d849492d8b798344d607e7dc47e77d63f0f02fe49bf392ab90fe -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-730a88d39b9860c1"></a>[`Inno.Native.ImGui.ImGuiTabBarFlags.AutoSelectNewTabs = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L566) | 当前常量或枚举值：`2`。 | — |
| <a id="api-7dbea0d6f1e46016"></a>[`Inno.Native.ImGui.ImGuiTabBarFlags.DrawSelectedOverline = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L571) | 当前常量或枚举值：`64`。 | — |
| <a id="api-af3629f02c850f88"></a>[`Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyDefault = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L576) | 当前常量或枚举值：`128`。 | — |
| <a id="api-6789f47a53173080"></a>[`Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyMask = 896`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L575) | 当前常量或枚举值：`896`。 | — |
| <a id="api-eb9183e55890e671"></a>[`Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyMixed = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L572) | 当前常量或枚举值：`128`。 | — |
| <a id="api-4494d2033437224a"></a>[`Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyScroll = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L574) | 当前常量或枚举值：`512`。 | — |
| <a id="api-2417855d3012c470"></a>[`Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyShrink = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L573) | 当前常量或枚举值：`256`。 | — |
| <a id="api-696be1d639969c82"></a>[`Inno.Native.ImGui.ImGuiTabBarFlags.ListPopupButton = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L567) | 当前常量或枚举值：`4`。 | — |
| <a id="api-5ddc7a813f414e70"></a>[`Inno.Native.ImGui.ImGuiTabBarFlags.NoCloseWithMiddleMouseButton = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L568) | 当前常量或枚举值：`8`。 | — |
| <a id="api-66ec552af7cbf425"></a>[`Inno.Native.ImGui.ImGuiTabBarFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L564) | 当前常量或枚举值：`0`。 | — |
| <a id="api-a9adb1e8a0828a91"></a>[`Inno.Native.ImGui.ImGuiTabBarFlags.NoTabListScrollingButtons = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L569) | 当前常量或枚举值：`16`。 | — |
| <a id="api-06f9a75d331729da"></a>[`Inno.Native.ImGui.ImGuiTabBarFlags.NoTooltip = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L570) | 当前常量或枚举值：`32`。 | — |
| <a id="api-00ce0e490500bb57"></a>[`Inno.Native.ImGui.ImGuiTabBarFlags.Reorderable = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L565) | 当前常量或枚举值：`1`。 | — |
| <a id="api-a22a729ececa848d"></a>[`public enum Inno.Native.ImGui.ImGuiTabBarFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L562) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTabBarFlagsPrivate</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTabBarFlagsPrivate c1bfdb03d357769d25a7e88a1a126e8fbfdcb5e2f42d127612a8434194e26515 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlagsPrivate.DockNode 71c23d2c0584f9ee25e72c23f173ba87c1b5e89db3b5948ca7a9ca98c773df4a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlagsPrivate.IsFocused 1e1f4a32caa5b85bbd640832b77de56cd08c437ac399b7a1d2a7c569983cffdc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarFlagsPrivate.SaveSettings 6eb11f7d5135f0d61a01a4c09e97642533ed335901d9288b7cc1c8a883fa39c2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-22a0e6898ad960ad"></a>[`Inno.Native.ImGui.ImGuiTabBarFlagsPrivate.DockNode = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1681) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-2f0a009e074a627f"></a>[`Inno.Native.ImGui.ImGuiTabBarFlagsPrivate.IsFocused = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1682) | 当前常量或枚举值：`2097152`。 | — |
| <a id="api-1ac45a87eb41f911"></a>[`Inno.Native.ImGui.ImGuiTabBarFlagsPrivate.SaveSettings = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1683) | 当前常量或枚举值：`4194304`。 | — |
| <a id="api-25b74547314f7563"></a>[`public enum Inno.Native.ImGui.ImGuiTabBarFlagsPrivate : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1679) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTabBarPtr</code> · 51 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTabBarPtr 64628874e62bfeaad7d5e83d7ff58eecb85c19f6cf16c9707fdf61cb241072a9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabBarPtr.#ctor(Inno.Native.ImGui.ImGuiTabBar*) 684ebaf228d148b9e14ececfd8cbff60f545e2d9be36d6981f5d2645e96b6632 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabBarPtr.Handle e407e79295beef18189fc550e60ee986afb80d434935e13b98e87da7032de82f -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.IsNull afadf36f3e683fd673ba051266ea9b4ffb91971030463e6944695395bef5a01f -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.Null f300e8ca013d1470ccd49e691be8a8be50e81acad6a68a2ee6bd4ebcf26569a5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.Item(System.Int32) 67efb6bece79e770b8734499b6918d6a8b8dfffd376f5f054d1775d9b9daf4d8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabBarPtr.op_Implicit(Inno.Native.ImGui.ImGuiTabBar*)~Inno.Native.ImGui.ImGuiTabBarPtr 7d172648a496c595cff9fb866645595cc71a53630b011b7676d3f77a96913579 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabBarPtr.op_Implicit(Inno.Native.ImGui.ImGuiTabBarPtr)~Inno.Native.ImGui.ImGuiTabBar* 00c64a62c3ec64838dcdfcd486cc1be0f89b7185d7477489e8312c44d9fe699c -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabBarPtr.op_Equality(Inno.Native.ImGui.ImGuiTabBarPtr,Inno.Native.ImGui.ImGuiTabBarPtr) 4d82c4ec6f8816a782b8f38be688ce1163b7932e4406eab7ccc28fa996d1da45 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabBarPtr.op_Inequality(Inno.Native.ImGui.ImGuiTabBarPtr,Inno.Native.ImGui.ImGuiTabBarPtr) 09b7c21ee6acdecce39ef2a0038ecebdb35d0fc9f7627008fd3b0c8150c06a01 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabBarPtr.Equals(Inno.Native.ImGui.ImGuiTabBarPtr) 5411d76e4f179e8dbb9239c29066f80eba32cbf148552dccc12f9585da42bdc6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabBarPtr.Equals(System.Object) 69975269ee834b68ed2da50a57b543a7f766efdda65f922aed8ec3e40bd43003 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabBarPtr.GetHashCode daffea7d2811a65f5a1288a06bec8ba97e0d0c79806ca2101c72ae1519609a7d -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.Window c568c00c2ba33b56057676458b5bc9eedddc875673a0e1c08ae04db4ea05788f -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.Tabs daeee8f97e7d700d8468baef1899314c6f6d0bf692565d4ef6056adec8f5b89c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.Flags 8f3e6d7b5c576e0f6d06a5a8989fa1537483872750fa437a6f1a646a6f3bf299 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.ID 8acebbadf6a17821d930191e8920c696f7d7d7b6f6e26c3503c166f7ead17b3e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.SelectedTabId 6e65b45fd48647137eecddc342210cae5bbfcdaf25d5dbcada602b6ad89c8b48 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.NextSelectedTabId bf3406dc5020639f6f068bd081470633b6284bd5ba11908158bfed84c08d22f5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.NextScrollToTabId 83977d8422b4347560a6622fbd9f9d26f3875bf346585b2a0fe3974a3c3a46fd -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.VisibleTabId 56f1ff2a70a60382dfdaf9ada7117e93c6d2ba46bc7ccc52363055a6de3eab53 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.CurrFrameVisible 212c157d6094f08cbb4655d90676f080b921b8ab40c7c4dcfa02ba3932975ed6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.PrevFrameVisible d712b9d87d0ac8d7d51f3a37a01e97b543f3eb49832092e7412876b7d5df13b6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.BarRect 7789b778e5e7096cd0a6b1a28471ae215aaa520620a32bdfb5ce393e8f74edd1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.BarRectPrevWidth bedd3bf1ddcf26aa90c91af41c75b3dc3e3acda531763d93997aab8a8ce0d07c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.CurrTabsContentsHeight 54bd65f0b487a272830822cc15911c8b9bc1757d962d5b3628393b14d2f9b93b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.PrevTabsContentsHeight 280f5aacc0c7326cad28ecc7d3f3b7b58ac767e886d93e7c4f672fee15042bba -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.WidthAllTabs 05c9f2b7461de163d7cfdb3b78e98a63a8026cabc389f8d8bb1a32946b1e5785 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.WidthAllTabsIdeal 47c25690cd5846711ce005355298624220f02ffa08ac5b0945afc4802e2dd699 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.ScrollingAnim 36e6c650adcb65ac742d490f7378267031db93228fcc94416793371009cff9fb -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.ScrollingTarget 1745a55d82f70ddcb166cdcac484c757d5e7a6c1094daa7be7a37d3595d90ad4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.ScrollingTargetDistToVisibility a92faeae58ff5e4048f91a30b4764221f8a00018d3e99cc5d527aa81ec3bc960 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.ScrollingSpeed 25b2a381be53fb5a0086f8726f9f021d3f5caa22a9f55e2c05340603effaa5dc -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.ScrollingRectMinX 1cb96bec32c711767597e6e3e4f5c813006aaa7a191daf2373e0e0fe13372520 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.ScrollingRectMaxX db6819202c7eab91d09af824fba27d6993f18078a8002f9f4f6f8a26e055c36b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.SeparatorMinX c33d7863c29140d4ccc164278c0041f3085c468be7ec1745a2d14ae06c4a77e8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.SeparatorMaxX 854417c806f58350c7716f57712a07c553c5e7d6f4a9413219090c3069181ef4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.ReorderRequestTabId 4528f6052e84e709b6181ffcf25774336f42dd3a5fd1ce6aefcae732c9814119 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.ReorderRequestOffset b72f6baebc4b3c7270273472724689d0fed05157a0629549ffc221f986402d91 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.BeginCount beac27cddc3dd32746de59a1f55ee604fc4efa0087fad56a9cfa2f1aae0d9d59 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.WantLayout 9d22f9002df950747abf2774b4d14d1f10d9b4abd7c8cd037f7399c47e589a19 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.VisibleTabWasSubmitted d817fe5b2b221a8978a172f9732ba51b884c6c8f28cb1a8a86f7f2480da8ed5c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.TabsAddedNew 337bbe4dfd85e33fc77cf168cf45a1a750d3b49f5c55bcd2b6836639b4967c70 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.ScrollButtonEnabled 0311de12b29ad6222b358eaeb1631e3e251d570eca50c680dae4609933b19580 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.TabsActiveCount 2153b3f5049e8dbe52596016d7afacab70daf63bc56691fd5403ad839b8651a5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.LastTabItemIdx aa07710885cc5300c7e75de02fb332ade9017c0021c1dc406839d5d4d7675098 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.ItemSpacingY 487e8db777d583b946e4b77c43448cf5dd00f01380100c353895894588ec3ed6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.FramePadding 01829bc71504634da6573fd0f8b4ad1776248ea928307414f4643c98a13504c5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.BackupCursorPos 77ea78d08cafa8ebdd21014d19107a0477d418b7c62d458ad28c391b45144774 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabBarPtr.TabsNames 0fb6e88238a7748394dbb8ea8b786cbe123fcf50ec6d526be934d494620f66cf -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabBarPtr.Destroy 3d8bef7c5f4a07706d4bdb05d7958274d8ba7f8a11e505091527e601cb1e968a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1989193798612719"></a>[`public Inno.Native.ImGui.ImGuiTabBar* Inno.Native.ImGui.ImGuiTabBarPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16624) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-28339476f219cbd0"></a>[`public Inno.Native.ImGui.ImGuiTabBarPtr.ImGuiTabBarPtr(Inno.Native.ImGui.ImGuiTabBar* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16619) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-13da3ca8651acc05"></a>[`public void Inno.Native.ImGui.ImGuiTabBarPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16679) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6fe9a44ebe8395cc"></a>[`public bool Inno.Native.ImGui.ImGuiTabBarPtr.Equals(Inno.Native.ImGui.ImGuiTabBarPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16634) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-ae5859df6f7ae006"></a>[`public override bool Inno.Native.ImGui.ImGuiTabBarPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16635) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-7c14b3457e56c839"></a>[`public override int Inno.Native.ImGui.ImGuiTabBarPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16636) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-161d53c954e623a9"></a>[`public static bool Inno.Native.ImGui.ImGuiTabBarPtr.operator ==(Inno.Native.ImGui.ImGuiTabBarPtr left, Inno.Native.ImGui.ImGuiTabBarPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16632) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2a6b06d7e67efb32"></a>[`public static Inno.Native.ImGui.ImGuiTabBarPtr.implicit operator Inno.Native.ImGui.ImGuiTabBarPtr(Inno.Native.ImGui.ImGuiTabBar* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16630) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7d575297cd18ade1"></a>[`public static Inno.Native.ImGui.ImGuiTabBarPtr.implicit operator Inno.Native.ImGui.ImGuiTabBar*(Inno.Native.ImGui.ImGuiTabBarPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16631) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0254567b02d21428"></a>[`public static bool Inno.Native.ImGui.ImGuiTabBarPtr.operator !=(Inno.Native.ImGui.ImGuiTabBarPtr left, Inno.Native.ImGui.ImGuiTabBarPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16633) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a96fdfc9de5c10e5"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiTabBarPtr.BackupCursorPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16673) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-06f8a60c4d32ad83"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTabBarPtr.BarRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16648) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d154aa95b6ca5bf2"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.BarRectPrevWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16649) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-269d4b82f3b08ebb"></a>[`public ref byte Inno.Native.ImGui.ImGuiTabBarPtr.BeginCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16664) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8159d8daad6fa681"></a>[`public ref int Inno.Native.ImGui.ImGuiTabBarPtr.CurrFrameVisible { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16646) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-416cbaf7b2d92679"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.CurrTabsContentsHeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16650) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-724eba0d56b7e943"></a>[`public ref Inno.Native.ImGui.ImGuiTabBarFlags Inno.Native.ImGui.ImGuiTabBarPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16640) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9c378dd59a9e9b51"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiTabBarPtr.FramePadding { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16672) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-03fa5527164ee52c"></a>[`public ref uint Inno.Native.ImGui.ImGuiTabBarPtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16641) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-679d32b0528816c5"></a>[`public bool Inno.Native.ImGui.ImGuiTabBarPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16625) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b32018759820e78e"></a>[`public Inno.Native.ImGui.ImGuiTabBar Inno.Native.ImGui.ImGuiTabBarPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16628) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d468d8d5a7ec0f97"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.ItemSpacingY { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16671) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ee374c08c6e9589e"></a>[`public ref short Inno.Native.ImGui.ImGuiTabBarPtr.LastTabItemIdx { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16670) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0b8b03a67960ec30"></a>[`public ref uint Inno.Native.ImGui.ImGuiTabBarPtr.NextScrollToTabId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16644) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bd60b20535bccc8c"></a>[`public ref uint Inno.Native.ImGui.ImGuiTabBarPtr.NextSelectedTabId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16643) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-03b5dbd82485058f"></a>[`public static Inno.Native.ImGui.ImGuiTabBarPtr Inno.Native.ImGui.ImGuiTabBarPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16626) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-701e7b48a2abd690"></a>[`public ref int Inno.Native.ImGui.ImGuiTabBarPtr.PrevFrameVisible { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16647) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-292faae0451f4c4c"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.PrevTabsContentsHeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16651) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d26c2c9c9e1a9009"></a>[`public ref short Inno.Native.ImGui.ImGuiTabBarPtr.ReorderRequestOffset { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16663) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8b47510fbfda9c9c"></a>[`public ref uint Inno.Native.ImGui.ImGuiTabBarPtr.ReorderRequestTabId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16662) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-81fb81ffa613fc20"></a>[`public bool Inno.Native.ImGui.ImGuiTabBarPtr.ScrollButtonEnabled { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16668) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-751e3f01fd86ab0d"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.ScrollingAnim { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16654) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e6943ecca2f6f4f2"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.ScrollingRectMaxX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16659) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2a5aa729e8f09738"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.ScrollingRectMinX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16658) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5b14c652d1970ec8"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.ScrollingSpeed { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16657) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c68f531680bd2eb8"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.ScrollingTarget { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16655) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-76a9078da056cce0"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.ScrollingTargetDistToVisibility { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16656) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-31a169604176f63f"></a>[`public ref uint Inno.Native.ImGui.ImGuiTabBarPtr.SelectedTabId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16642) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9595829fb98a23e3"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.SeparatorMaxX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16661) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-97ba51c308ccf17b"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.SeparatorMinX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16660) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4bd19c09561506b5"></a>[`public ref Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTabItem> Inno.Native.ImGui.ImGuiTabBarPtr.Tabs { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16639) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-802338d83c5d12c9"></a>[`public ref short Inno.Native.ImGui.ImGuiTabBarPtr.TabsActiveCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16669) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-86406d99985890ee"></a>[`public bool Inno.Native.ImGui.ImGuiTabBarPtr.TabsAddedNew { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16667) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-09cf418fc12484bf"></a>[`public ref Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiTabBarPtr.TabsNames { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16674) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-67125da8b1623f1b"></a>[`public ref uint Inno.Native.ImGui.ImGuiTabBarPtr.VisibleTabId { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16645) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bd70782b68bd8cad"></a>[`public bool Inno.Native.ImGui.ImGuiTabBarPtr.VisibleTabWasSubmitted { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16666) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0d9c55e394d41306"></a>[`public bool Inno.Native.ImGui.ImGuiTabBarPtr.WantLayout { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16665) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a220598e1919f8ff"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.WidthAllTabs { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16652) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2efe233aec73ee86"></a>[`public ref float Inno.Native.ImGui.ImGuiTabBarPtr.WidthAllTabsIdeal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16653) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c43a445a6c8a6891"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiTabBarPtr.Window { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16638) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-88186e319abcacb1"></a>[`public struct Inno.Native.ImGui.ImGuiTabBarPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTabBarPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16617) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTabItem</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTabItem bc9b905b5ba6e86b568041011ef675ab86bc51ba06c4a61222a420076ada140d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItem.ID 0641adbc0b2f5955e321e58dd34e8c765e518982209c66b2172c265ae5c0e22b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItem.Flags 6fd3834e8d9588347ec57ef9a4546f3a350bf6f52e25d75ea43a01488de55120 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItem.Window 1cbf13b1fad67a863b87be725db5f6f9c24125a1e2900d5fa5eb22ef3768299e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItem.LastFrameVisible f4fc2eeb12b29d939ec0b180871f056df9057ed0fb948dc7af99a802083c90e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItem.LastFrameSelected fcee5a39f54abe3fbbb38bbb35cd1d913da888245566dab18a2aa254ffcf9636 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItem.Offset ac75b53c32d2df347a56be35cd45004e271a01b96429d97218afa117c5e6402e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItem.Width 233bffb6dfe26798a60b5eee37b70258273981533e156e81b16eafd47c052a67 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItem.ContentWidth f1a07f98f66798decc1544b7b39ae9cb17e81ecb46137e18476eef673f2de720 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItem.RequestedWidth af745595db6ef1a2a0092a1208c9ffd5f82df5bf521314a1ef36a1d7d9cc21dd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItem.NameOffset a5dc0872e321ac3d69ce62335bad628c2abf8090538a5ebe4763f4da0beac74c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItem.BeginOrder 4e063a94732919fa7792632547cef15770851981479afd7239299e2bd86ef26e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItem.IndexDuringLayout 33112ff6ad8354322a6d9cd3846a4c909b6c1c7a6f24dfa9574482b5bf963d61 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItem.WantClose f142f874285a4ceb946153bb868b152f1d8a3b5c68c7ed86a2696302f516d75e -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabItem.#ctor(System.UInt32,Inno.Native.ImGui.ImGuiTabItemFlags,Inno.Native.ImGui.ImGuiWindow*,System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Int16,System.Int16,System.Byte) a7e70ab3b8698e34a808f0324d6c37068bdb17610c3e724097663dd7dd87be01 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabItem.Destroy 1b5a9802bbb3eec3d4552b8ef3094a1e75fad997334b9aba5be04e8348d35aa8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-289166b53e430a95"></a>[`public short Inno.Native.ImGui.ImGuiTabItem.BeginOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4345) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-12865e8b04f3f329"></a>[`public float Inno.Native.ImGui.ImGuiTabItem.ContentWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4342) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1551af9528512865"></a>[`public Inno.Native.ImGui.ImGuiTabItemFlags Inno.Native.ImGui.ImGuiTabItem.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4336) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c956909d647bf8cf"></a>[`public uint Inno.Native.ImGui.ImGuiTabItem.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4335) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c19d03f96a1f8715"></a>[`public short Inno.Native.ImGui.ImGuiTabItem.IndexDuringLayout`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4346) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe01052db48e92f8"></a>[`public int Inno.Native.ImGui.ImGuiTabItem.LastFrameSelected`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4339) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27051a398f65eb7e"></a>[`public int Inno.Native.ImGui.ImGuiTabItem.LastFrameVisible`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4338) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f4ec7c21fc00ac5"></a>[`public int Inno.Native.ImGui.ImGuiTabItem.NameOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4344) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0b83f8a4f4c7ba4f"></a>[`public float Inno.Native.ImGui.ImGuiTabItem.Offset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4340) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-421234d4970ae818"></a>[`public float Inno.Native.ImGui.ImGuiTabItem.RequestedWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4343) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e4a5ff3fc01e775d"></a>[`public byte Inno.Native.ImGui.ImGuiTabItem.WantClose`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4347) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be0876802bc675ed"></a>[`public float Inno.Native.ImGui.ImGuiTabItem.Width`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4341) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb57f4d6c2f45977"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiTabItem.Window`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4337) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-345c2ad8aa511ef5"></a>[`public Inno.Native.ImGui.ImGuiTabItem.ImGuiTabItem(uint iD = 0, Inno.Native.ImGui.ImGuiTabItemFlags flags = Inno.Native.ImGui.ImGuiTabItemFlags.None, Inno.Native.ImGui.ImGuiWindow* window = null, int lastFrameVisible = 0, int lastFrameSelected = 0, float offset = 0, float width = 0, float contentWidth = 0, float requestedWidth = 0, int nameOffset = 0, short beginOrder = 0, short indexDuringLayout = 0, byte wantClose = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4348) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bb91f714c228078b"></a>[`public void Inno.Native.ImGui.ImGuiTabItem.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67547) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fda059d15246a538"></a>[`public struct Inno.Native.ImGui.ImGuiTabItem`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L4333) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTabItemFlags</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTabItemFlags 9f36953b387b05af74dd37100c939f2ac630d92b7952d3186fcc6acc401a949d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlags.None 9e652f0e11b0ab04c2f0b0a92830986de3f15461d070d3e73073b4534ff87f50 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlags.UnsavedDocument ead3aba07ecef424c82e4eb62d0cb7dedd5a4e11a789e854a7fcec3333def299 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlags.SetSelected c2ac07070ce5af45a61cf9e413e3fcf2a34e391ab5965e04793c2ec9b7e5d40a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlags.NoCloseWithMiddleMouseButton 46052f9737e338868856dcf6e0fc95ee273bde2e936c2edb719d8eb84a7420d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlags.NoPushId 264a454ca0b1a8759d497e3d261cf27e63d9d547c2a4fa04ab8ba0784e22a730 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlags.NoTooltip 7694f837d732c9b7cc27955acf70c0de50159409ad181fd39e23a6f6ca8e3455 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlags.NoReorder fe349ec40bd5de180ec0d0e9838f70d3f2e535e60c4938270dccfaa34652e383 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlags.Leading 1e8a491f3f4821f7a042325b0aaf724f7d1042a86e7e50b850c356ddcde4e03b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlags.Trailing c3aeda9112d9af61559b4b63608d096cd94e80c36cc8927b5772084bed82e7db -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlags.NoAssumedClosure d8f520e82688513b630338b3f321450248ff4ce65ac18d8382ea677793b04149 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ae14a9a5713210bb"></a>[`Inno.Native.ImGui.ImGuiTabItemFlags.Leading = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L591) | 当前常量或枚举值：`64`。 | — |
| <a id="api-0c1fc51744ef204a"></a>[`Inno.Native.ImGui.ImGuiTabItemFlags.NoAssumedClosure = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L593) | 当前常量或枚举值：`256`。 | — |
| <a id="api-2f8d25a774abdd37"></a>[`Inno.Native.ImGui.ImGuiTabItemFlags.NoCloseWithMiddleMouseButton = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L587) | 当前常量或枚举值：`4`。 | — |
| <a id="api-e60484363b67ad6d"></a>[`Inno.Native.ImGui.ImGuiTabItemFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L584) | 当前常量或枚举值：`0`。 | — |
| <a id="api-efbd751d664ccad8"></a>[`Inno.Native.ImGui.ImGuiTabItemFlags.NoPushId = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L588) | 当前常量或枚举值：`8`。 | — |
| <a id="api-f1862a31a54cca4b"></a>[`Inno.Native.ImGui.ImGuiTabItemFlags.NoReorder = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L590) | 当前常量或枚举值：`32`。 | — |
| <a id="api-b714707fb8d5542a"></a>[`Inno.Native.ImGui.ImGuiTabItemFlags.NoTooltip = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L589) | 当前常量或枚举值：`16`。 | — |
| <a id="api-73356005a3df902a"></a>[`Inno.Native.ImGui.ImGuiTabItemFlags.SetSelected = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L586) | 当前常量或枚举值：`2`。 | — |
| <a id="api-4bf2f969e63ab7a8"></a>[`Inno.Native.ImGui.ImGuiTabItemFlags.Trailing = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L592) | 当前常量或枚举值：`128`。 | — |
| <a id="api-67068f8882d57823"></a>[`Inno.Native.ImGui.ImGuiTabItemFlags.UnsavedDocument = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L585) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d9c97ccd92db098b"></a>[`public enum Inno.Native.ImGui.ImGuiTabItemFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L582) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTabItemFlagsPrivate</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTabItemFlagsPrivate 15c109d3a8dc47355e20145ade10aa4fef4de0acc7798f13d6ced07b8a7da0e3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlagsPrivate.SectionMask e40c7abe5a057a1de29e330a8f83965fb2f94cff4f4602065f3caa5d8faef288 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlagsPrivate.NoCloseButton f21dcca965679cf29a77687caf7834e3ed140178e6256c8dc898982700d4f5e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlagsPrivate.Button c9818899b2873a5c9d7c738edfa04af098045287126d55842b7b68d30b55dfec -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlagsPrivate.Invisible be18c2a940485f4e834fbada38e54d3e68493111c06f2ec82116f09d777a8f40 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemFlagsPrivate.Unsorted 960cc2dea9686eecfca563ef9d618d5a00b62a2171293fe22229b14b19ef1190 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-348ced68eceff21e"></a>[`Inno.Native.ImGui.ImGuiTabItemFlagsPrivate.Button = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1693) | 当前常量或枚举值：`2097152`。 | — |
| <a id="api-f6fd236f51ad9eaf"></a>[`Inno.Native.ImGui.ImGuiTabItemFlagsPrivate.Invisible = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1694) | 当前常量或枚举值：`4194304`。 | — |
| <a id="api-d14ca2bd7940b02f"></a>[`Inno.Native.ImGui.ImGuiTabItemFlagsPrivate.NoCloseButton = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1692) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-09ff7f3df04c8f8e"></a>[`Inno.Native.ImGui.ImGuiTabItemFlagsPrivate.SectionMask = 192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1691) | 当前常量或枚举值：`192`。 | — |
| <a id="api-6d9a774d15e15d83"></a>[`Inno.Native.ImGui.ImGuiTabItemFlagsPrivate.Unsorted = 8388608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1695) | 当前常量或枚举值：`8388608`。 | — |
| <a id="api-4621101e76ac83e3"></a>[`public enum Inno.Native.ImGui.ImGuiTabItemFlagsPrivate : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1689) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTabItemPtr</code> · 27 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTabItemPtr bdc3d0465d5c2e229d80a236e201a33751998401b89d0697a5805c3cb0b3670d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabItemPtr.#ctor(Inno.Native.ImGui.ImGuiTabItem*) f070f7f01a5d7a7b0fdb598148b6bdf719ed29de1610196738ec4a5c87a76eff -->
<!-- api: F:Inno.Native.ImGui.ImGuiTabItemPtr.Handle c00e22b82b98ed84c361d73994c4ad91a92d8388e278c0899de4a5f2c06a4c47 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.IsNull 018831fc47d0b9e65df614c4c7f3066bac2727345e85e0d4c55c15b70d863af3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.Null 4ab4cd24185caee36cb4bfd34252344a53befc916688ec42b466e124caeeb236 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.Item(System.Int32) 3373877c2d504922858449999018e44693a426d5bfd15361651efe69831f4e55 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabItemPtr.op_Implicit(Inno.Native.ImGui.ImGuiTabItem*)~Inno.Native.ImGui.ImGuiTabItemPtr 8df963fa0ab54c99a8dbb179328b1c5537ab7abb34254861cbbc6e5a45002e4e -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabItemPtr.op_Implicit(Inno.Native.ImGui.ImGuiTabItemPtr)~Inno.Native.ImGui.ImGuiTabItem* fe8b3650d2ac1c26bce070e276a92fcfc55f735625955a9e63c7ac8b796cb815 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabItemPtr.op_Equality(Inno.Native.ImGui.ImGuiTabItemPtr,Inno.Native.ImGui.ImGuiTabItemPtr) 928ae37d49f0903d05b413a9a2c2d41d707649a5c19b972ba11c0422fe18289e -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabItemPtr.op_Inequality(Inno.Native.ImGui.ImGuiTabItemPtr,Inno.Native.ImGui.ImGuiTabItemPtr) 336f922ff8b64a0bca4295cf776ca6a036e3ee1e132bc51bc66c3863554d1225 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabItemPtr.Equals(Inno.Native.ImGui.ImGuiTabItemPtr) 93de06e9f80707b8ded131799e0acd33b1b0708e976d3959585dafd7cbee9b82 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabItemPtr.Equals(System.Object) 9791ab70a7b59dfbd387cbefed5f11c1422068086dce97d2b2b5ebf14ec3cda9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabItemPtr.GetHashCode 0d15b003639ef33d0acb333f45d8bc3d7cda4aa4269aaf8735e1f65b5f35d777 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.ID 61d0d2c66ef4bc39b31b65249d627e0c3b144e4d75401adfaaca85c53589f502 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.Flags 38256e2fc5552e785580db401d71c34614d1a1d7d87e598db7c276c49950110f -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.Window f99fc94fffe5767f9a2e9ba023a825754a768c9f82253751d3394112c3528766 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.LastFrameVisible 55714ec002d488c5393b2eee54f52202586d556a0a227f447acdbd39c6396024 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.LastFrameSelected 7c435c8e3d59a462ff884bd1900e9b94e9d5241c11bd51588e9c86c4c24a21f5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.Offset 32fb83474d5256e78769623078e2fc9aa6462a8fe74a43aa54869d67a16813b5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.Width 6869c91a43d2529cadbc9d8e81fb8334c8035c303335954ee2bec75acf6db8e2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.ContentWidth b67371f56d30c6148c1128a699d93086e4aa2afe5302f9f1bc30968b08186745 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.RequestedWidth 972309830368ba5f472c75a889d26f26d8a1be50ff79c543a745ce19b78665a7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.NameOffset 04ffd0d06bb0e0c6128b27c11981c507d3cd64c28edccfcea67e0fcc492b7041 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.BeginOrder 414d6ed191fc6b7e9675f21023378fd92957f438e2b433b76f4b4a57a68d9510 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.IndexDuringLayout 1f3337ad81e9410f00aa885795d1e7f3176cd33fbe7742fc38b7cc5e1ee6379b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTabItemPtr.WantClose 978dadc170ed9f98826ecc069b2710c45e0b47a672d31769d43e893d82c7494d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTabItemPtr.Destroy bda5d62466ce4bf1ff1ac94f690c83ed8d931ddc1f3741e14c132f38f0ef30ae -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-bf6c86965bfb2a5d"></a>[`public Inno.Native.ImGui.ImGuiTabItem* Inno.Native.ImGui.ImGuiTabItemPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16693) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cbe197f95948608e"></a>[`public Inno.Native.ImGui.ImGuiTabItemPtr.ImGuiTabItemPtr(Inno.Native.ImGui.ImGuiTabItem* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16688) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a5c2928a76880bab"></a>[`public void Inno.Native.ImGui.ImGuiTabItemPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16724) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-53502d58d99c2c93"></a>[`public bool Inno.Native.ImGui.ImGuiTabItemPtr.Equals(Inno.Native.ImGui.ImGuiTabItemPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16703) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-b94e47f2c9a235e6"></a>[`public override bool Inno.Native.ImGui.ImGuiTabItemPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16704) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-24d7081ec3ca3dc0"></a>[`public override int Inno.Native.ImGui.ImGuiTabItemPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16705) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-ba620bd57576b396"></a>[`public static bool Inno.Native.ImGui.ImGuiTabItemPtr.operator ==(Inno.Native.ImGui.ImGuiTabItemPtr left, Inno.Native.ImGui.ImGuiTabItemPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16701) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a6b0cf6e83048da7"></a>[`public static Inno.Native.ImGui.ImGuiTabItemPtr.implicit operator Inno.Native.ImGui.ImGuiTabItemPtr(Inno.Native.ImGui.ImGuiTabItem* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16699) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-336bce51239cbf47"></a>[`public static Inno.Native.ImGui.ImGuiTabItemPtr.implicit operator Inno.Native.ImGui.ImGuiTabItem*(Inno.Native.ImGui.ImGuiTabItemPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16700) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7f14baf7db8cc942"></a>[`public static bool Inno.Native.ImGui.ImGuiTabItemPtr.operator !=(Inno.Native.ImGui.ImGuiTabItemPtr left, Inno.Native.ImGui.ImGuiTabItemPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16702) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-14bd6c74e273e012"></a>[`public ref short Inno.Native.ImGui.ImGuiTabItemPtr.BeginOrder { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16717) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2ef5f52466484982"></a>[`public ref float Inno.Native.ImGui.ImGuiTabItemPtr.ContentWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16714) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c4dc070c1df1cee9"></a>[`public ref Inno.Native.ImGui.ImGuiTabItemFlags Inno.Native.ImGui.ImGuiTabItemPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16708) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2486506dc79a9312"></a>[`public ref uint Inno.Native.ImGui.ImGuiTabItemPtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16707) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-02533cb7e970de60"></a>[`public ref short Inno.Native.ImGui.ImGuiTabItemPtr.IndexDuringLayout { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16718) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0f6c4583ef2d1b83"></a>[`public bool Inno.Native.ImGui.ImGuiTabItemPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16694) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-21736e309f9e3830"></a>[`public Inno.Native.ImGui.ImGuiTabItem Inno.Native.ImGui.ImGuiTabItemPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16697) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-053125813b752675"></a>[`public ref int Inno.Native.ImGui.ImGuiTabItemPtr.LastFrameSelected { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16711) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-deecb200e1746a55"></a>[`public ref int Inno.Native.ImGui.ImGuiTabItemPtr.LastFrameVisible { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16710) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2670da2f3755bc42"></a>[`public ref int Inno.Native.ImGui.ImGuiTabItemPtr.NameOffset { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16716) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8d54f870b551271c"></a>[`public static Inno.Native.ImGui.ImGuiTabItemPtr Inno.Native.ImGui.ImGuiTabItemPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16695) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bed38b513e5bbfba"></a>[`public ref float Inno.Native.ImGui.ImGuiTabItemPtr.Offset { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16712) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3e0dc27f1bf9690c"></a>[`public ref float Inno.Native.ImGui.ImGuiTabItemPtr.RequestedWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16715) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8216af70f8c0152e"></a>[`public bool Inno.Native.ImGui.ImGuiTabItemPtr.WantClose { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16719) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5fa892c6a4bf6696"></a>[`public ref float Inno.Native.ImGui.ImGuiTabItemPtr.Width { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16713) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fd97aa1080fc4d25"></a>[`public ref Inno.Native.ImGui.ImGuiWindowPtr Inno.Native.ImGui.ImGuiTabItemPtr.Window { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16709) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d6479387fe843531"></a>[`public struct Inno.Native.ImGui.ImGuiTabItemPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTabItemPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16686) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTable</code> · 122 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTable 7011e90b2575fbd7c6bdfad84f15c4c7da49533498594f2278ffd30203a5a17a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ID 34ab728cabe217c88e017f00b3c5d4c27d8868bc938e92934be8c33db6c0155b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.Flags 0bf1366a24931d5a9c60e23b115180c06ae707fe550938cd27f290eb845755bb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RawData 4e3ae7efeeef882105e7e3b11957029579b9a57b7f7abfd78864de9d146fa88f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.TempData 1f43d3ff6d586dbc93b5371838ea117eb26278efb3dc8b35f251d9977cc16f00 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.Columns 13f200975b03b63f4608631148c11c45fa7ae6730dc20874e39b0aebaeb4a2e3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.DisplayOrderToIndex a3ff00dde5b7c5f5f168aa9d27ccac426c186a85f508c6d3182bea3ddf6c6e1d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RowCellData de889c1c34beafd474026246c9209501ca7c4152b6c20e8ac3a862dc6e61f1dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.EnabledMaskByDisplayOrder fa8a7430952d33ec1a4a90b62be194f8ccc62bac2c1349dfb9ba9a8ecbb933a9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.EnabledMaskByIndex 1b70fd5c8d6b5842e4a22e6b34e26c53fb33c62dfbafaa16cbd1385447b6467f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.VisibleMaskByIndex fc9b7820d6a61713b936009d2daa7a5c25df73a53c891788794c67e916186918 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.SettingsLoadedFlags c832b8e2835a615d68cfa88c101afd16644bf878c6235af771bd5c46c9c1eb14 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.SettingsOffset 89177f432aa0279ac42fa039629018cf26d529904e406e8a6b71c6485c522911 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.LastFrameActive bd9581b1806442de16039ddc0cbd5a4294871aefa944706d39b9cd31cf6f5f93 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ColumnsCount 43b8196cc8693cf52a3df8178d3e49792b0fd3c24a0069a0a2e91431fe1fde4d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.CurrentRow 3bb8d72973d666e46c6fe88950d178eacaeb307dfc4beaea9eabdd1f7c809fa4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.CurrentColumn bb38b4e8946739418b5a391a7cfa90239aeb54a45763b2b2044564e74e4b7a63 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.InstanceCurrent 1399d7b707352d69863d4a0d745bd1e62aeb83ec77ecce574be520c9444346eb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.InstanceInteracted d478a29c34f63419b78b3b16d202fe6641b946ee544b44a08889a3353eac3742 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RowPosY1 efda3a0e63513b8c9b93cf16d6c45bd2e5e6bfe9ff314a84fb93c04d28b21bd7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RowPosY2 79aa2b299dfc00acda7617453b0d9a68733f66330d65859ad925373879100dd3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RowMinHeight 4e8572ac753940c5e809f0fe75f9ac790097813c7f082d028e832fe431bf2c4a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RowCellPaddingY 61e3330ee50239ccb8dfa872f23b7b6ef56f2c86afa597eb457dce081f58a5f3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RowTextBaseline 5f117c3c2b2cd82121eb2814a80593bf694a1e16138fcfe8576a4bae04b2a42f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RowIndentOffsetX e9b145fc6a587df7ce703e7f588da04e80ce3f39c8410353c38a6edff745db65 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RawBits0_0 18e0cc0eccdb7cdad6cab98c204bfc95f2b0d2118c38f8952308449047b5a706 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RawBits0_1 1cb52d80f69ce09350950ab9dc4dffcc86859fd09607fc586f705bf0545b1c13 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RawBits0_2 8ff128b7ee0773f11ac29551d642a8c8981c8df4f873a7ae370006ff6e434c6e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RawBits0_3 296cd76ba95170c2f1ed466b60bf9f0c9f61d8ee36743caf669a0ef04af7f6a2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTable.RowFlags b8321a54c7c235969026b69985c75e7913525c66a1de9a4f485d9e6e0236fa9f -->
<!-- api: P:Inno.Native.ImGui.ImGuiTable.LastRowFlags 12964cfc0787d4b47058b0b98e1023f1cfa943c0ee979644deb965b820ca8f5f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RowBgColorCounter f24608e06e2f1692f828ed1424f41be52de4cb1c4c06849fbbf8bff3802cbfb0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RowBgColor_0 7038766f163b5971c9f9d0bf08dae466378d9034b302823ca0dce4ba311032f2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RowBgColor_1 3433c07efddad1d04a01b4684a4336b69b332ec334532910fe2c683f9647dc8e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTable.RowBgColor adc399ff4563218ec19800c3413ad67f264544b0eb85171c8ca4909980d34fe3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.BorderColorStrong 1ab63e38221d87afe76cf473b1f80fe4eeb5ee74d090a723bc13ccde5fc3011a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.BorderColorLight 86493c29e976d82e577f7635617944cb5039ee7dea2cbbe562b891f9e1c25874 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.BorderX1 c22de6794818b57016b6d09d576e6d97ba35f0dc5234ce26c7a9a3faff3f6118 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.BorderX2 d60956c5e30651e1011afbb900a5d21402edbcd7e35ca5b6de5d846faa3e456f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.HostIndentX 6a2a11e9e5f822333d7a2fcde4a43d4ba73dd89f4e48dba0e8a9f4c42bef8364 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.MinColumnWidth 2c9679bb9b0cb16a1db0704cbe71607abe551bcbb26fa0ff38b48346bef1ae2b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.OuterPaddingX 6f1d8efdc41e88fa03efa4c2dbc067b46b3f918e16157fefdb9cfdb786bb9aa7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.CellPaddingX 9c57ae859ffda587833dfbfc0dca4850289f275363d654a899b3af18d25e6398 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.CellSpacingX1 3c92574b38ae7ca9c5027aae9c9b0c0abbec29953fe9c00babd488d6ea3ae569 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.CellSpacingX2 a2323bdc7b68ff4b03d5175b8bdc6054053a59c4bfeaafa55a2980dbd53d5171 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.InnerWidth 0f53da776ef4678974e0717af37ff944ee6b8182dc8bc57aff0380b823968c4e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ColumnsGivenWidth b34a041b5de32e61f9c75a98dcdd30fa5961464517b426c46a3c94ffca218d90 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ColumnsAutoFitWidth 2cc3e1a3eaf13cfd4c90615a1c4e85a82a9280f3d0db7312dfc0063bf24a2366 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ColumnsStretchSumWeights e8c8c94fcbfcad75781ad82d0af7a0f61debe837659e9054fed271416fd36c86 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ResizedColumnNextWidth afdae1d771b964f02c433dce240c9c8a80d8a13af4799e442adac2b3e289b495 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ResizeLockMinContentsX2 73f3243dba91106f1d9188b0e87052643eba29a3e0286595a14bc2b053f87a98 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RefScale 8581a11a83cfbfe9e4077c8260f65e2eed048e61eb134261656120f41f7926da -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.AngledHeadersHeight b96b97002172f1e3c879d22b00f158f74e1abfc60eabed252ee77348e3d70ca3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.AngledHeadersSlope 2b520375f026145ccb84b08d396e19d0bef541918f9ced4333a9507c91463156 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.OuterRect 32eed0c9286570ecee20b44f80c12988eddc51d18c33aea6733fa87f68ee9d1a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.InnerRect 06634f501243a9c462809a083edc5cf3b230bde9333bbb640e4f95e700d44ad5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.WorkRect a074654f9b98021f4bef7a26729dff78c469611c55d68501a6996978f35ec088 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.InnerClipRect 0d12536245c79a36f5a05e2e79e534afc9b7360662e98267e285a2a2caf292ff -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.BgClipRect 4e28e1b435dd6d519b5fa34f5f3c6caf136a27badb701edb4b5e59aa2739bb89 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.Bg0ClipRectForDrawCmd 3c162f1632c186c8cc3cef88bd33d0835ed7e6046b080ce64a14cb2f54a19274 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.Bg2ClipRectForDrawCmd a6c3cb707100e905f8dd86554ef8bed53728e0d2c14479d5e51428b0a6eccc59 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.HostClipRect f9e34fa16d11f184a8914e370cda0a16e4eed078c5267a89851eabc029a12d38 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.HostBackupInnerClipRect 29fc6dfdcab30c61c9fb15f31ca2c9cb23a3c3ab38c285dc6be9deef79ba5b3f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.OuterWindow 2ca92e47fae2d75b7d3443f0ae89163745d2218ac79367e65604f041d61773a6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.InnerWindow e306ddc033c2a84f23521815d25eda5964ea7ee6120ef8971b5a815dba085d21 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ColumnsNames 717a7435185f501772467cf12d99de7c5802547f111f7f3191762c966bf7f525 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.DrawSplitter 7ab36e1ae007f59a67ed3ff2ae52998eaa4368fb309a1a11b8f75a54f0105a0b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.InstanceDataFirst b436b3e3ff103f535e127bdb68a07a8c91dcf67f758e4919ff52d5f4522ab8aa -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.InstanceDataExtra c969abb3161e591e3312bd2d862cf247af5acab249a239b7aa6f0cbe1d682135 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.SortSpecsSingle 7f48129a09c88286b36d6590bef2e1dc9009e77fe576dd6738d929bc18722a54 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.SortSpecsMulti aa365b3efc969c6e9b0fcee2d71d852f126a3c6eceff3790d8c21695639363f0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.SortSpecs 5ce718d008fd75a76453c615b06c236713cabc714fa32d4c658e528e41d73f8f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.SortSpecsCount 50f89de22695b450db41b652a6e1d9b59bb2e353907da545adc541048d0bfbc2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ColumnsEnabledCount 84821f775b4268f18eec00bcdd5b399441823d63ae4a64557dd2de36d22f7365 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ColumnsEnabledFixedCount 89b37b9b3d3de1fe24630a2063e1e953bc79d6f7151797ebb4bed64ef04f4b69 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.DeclColumnsCount 4567cff9e59a1d888fab0186c0b1101345c572669bf7badf7614ad524bd16a6a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.AngledHeadersCount e5f996f5312d08c8bf8de0b297fca78e693f18b56f548d2b1c953ea43fccd79f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.HoveredColumnBody 7aa9c760f16d8d4d06df3f5b6e5272e5313afbbe477470a613a5e755d71facb6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.HoveredColumnBorder 53ded10c494a7d2477522e2ff65ef9f6a996b3d9e8e49893768647390681c679 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.HighlightColumnHeader acb2041666759ca19fe0b5bcd039fee8c1f74120498ae49daa7ecfa791241fce -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.AutoFitSingleColumn a976203e6efd7fb78cad5412cc6654ccd6a663b1186ac7a4cc14b0d24d6103f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ResizedColumn e70099e03a2b943e343cebfc97c4382edb4f55015d75f6b5fa38fe607e2b8e50 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.LastResizedColumn 553210d13e4adacf50c2df50634ff1353151472bdde9bb9df080cb181c685060 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.HeldHeaderColumn 8d2ef088153bce11cea0287029effa2ec783b64101f3fdb6b21a3f33da3e8fd3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ReorderColumn 5488738e57dfa68a4837fc3ed138b707df70c6707afca550af0db89ee9946c1b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ReorderColumnDir 11693bbee1acc566d72dcc34aaf2144ab6cf10d287805cd95d428d2f88ad110b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.LeftMostEnabledColumn 191240daa56ba5bf121ddd7f8decc393708b74695f91723a98f577d7fa54b7e8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RightMostEnabledColumn 4890bc28d359f72c546b868e5a6dde7bd65fd8336c74c1de170986ed46828155 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.LeftMostStretchedColumn bb5792a9d3f8e704af07fd0a1d678c8a0b9fa02c7d5953a9e82890b15ac76ab8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RightMostStretchedColumn 8bf548cb6c9fbba9dcb5e5bcf8e7bf6f436750282b69fd86dc20f25bcc82076e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.ContextPopupColumn 73bb9da280d6f09320d2f7784b7b8de8e98f709b556c09066b5b7a975fc3ddb1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.FreezeRowsRequest 8bfb7076e061f191cdb6184a0db0d18bc59e44b1da72c4388f37dcfefa866e23 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.FreezeRowsCount 197e9f9358ee1b29900b784965e7a2e3b32ef232e76fe5493f3bb3fdbdcd6147 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.FreezeColumnsRequest 695e86a1423d484414fb6a9d651061aa335c1059f29a15a6328e430c2f8a01bf -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.FreezeColumnsCount 84dc1c26fd67586e77767fe51503be34662c7880201cbf3a0d47a52efe62a1be -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.RowCellDataCurrent f948c0bb93268f2d47b3452ef1c978b0e4dafa930e93c2e7cfa6fc362da0c99c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.DummyDrawChannel c99a216d5be59eadc74b6dfdb37eacf10e98af8ab4180c249dc5600ad052d92b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.Bg2DrawChannelCurrent df06b08bcb7a4ab196269994e1c28256ce8157bb0c3dd92357b6a502749f2e5f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.Bg2DrawChannelUnfrozen 5cd352de38ffc4f2e578d59a2effec551a8347d1dfb89301149976350ed30a8b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.NavLayer 0c8e49c431bc7936d941d0ff24ab2f2fc3320a59dce9766176f94f9bab21f623 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsLayoutLocked 36e51e3d335db7dfdeb3e9736b09d028e29795f62076e4be56aca045fbf409ab -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsInsideRow c82c14b9171182e8b05cb577723d08b0280f2aa7e209a4c3a1e91d383e074b9b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsInitializing c04af1f4c53ca6b782d98eac7a54a866fedff2312d2e5eb3130c25eb14c73ac9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsSortSpecsDirty 5cfabfc125a6d6c92bf2eb678c7124991c88880fd7c8b4d3e75b0b899b5068ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsUsingHeaders 68e5658647bff89e05902142aa00ccf53bbbafb622ad9ef417ff28edaa215860 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsContextPopupOpen 25590a1116fdf6358b15284028e95170e08c8c96edc70085f82fd64ed7737560 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.DisableDefaultContextMenu 6082875242a403397866d8696c9ac4beef6ab28164702e6346ad817559175231 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsSettingsRequestLoad eca935b0bf9a3c47d8c1916743bda030ffa8e0e6e8428e0027bef854783ef4a1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsSettingsDirty ce5fe66c2c3c5b311e422f698871dcc9f0d0a8933792b615327e6e57752635dd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsDefaultDisplayOrder fdc81f5cfc26ade6cd961a458dfde114fcd0085decfe709a57959119d5f80a98 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsResetAllRequest aba28f12a2358b0d522e15a16c9a51a8d91e515a99bed089a47a9bb311f6e3c8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsResetDisplayOrderRequest 0e9f2d8d449b210070c810c222257a1a30624381a1153213f6a89a9a3115ba39 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsUnfrozenRows 5c7dbe87de690e85b8d8155dc102cef1183dc769802118532fe675b593281edc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsDefaultSizingPolicy a00d4e440b9dfdd4622a6e8a5c4c0c80931d6468cb4ad78f1ef5895cd4910e02 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsActiveIdAliveBeforeTable 20259dc1344d45fa276d6e65570d6bb3305be72cf7804d7edfc3d330f01a6f9a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.IsActiveIdInTable cebce30e3906a601a9f44dd80a1886051ecc794194b660425ed008421b2083bd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.HasScrollbarYCurr 9525a815dd6ce0542e891df3150086095d32022154c00ada470b9b977e297652 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.HasScrollbarYPrev 923d203956ccc60ca128629fa4ab52d0a0cedac19ac4de90c4f6f01e698c154b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.MemoryCompacted 5535370b45de8202b41fbf464dd55a6b19fab63abaf8f871a94f7720d5a3df73 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTable.HostSkipItems 1c98b9f0384181497f08bbe69b6f394e41406f6866b1e3da6c0bde9a305888d0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTable.#ctor(System.UInt32,Inno.Native.ImGui.ImGuiTableFlags,System.Void*,Inno.Native.ImGui.ImGuiTableTempData*,Inno.Native.ImGui.ImSpanImGuiTableColumn,Inno.Native.ImGui.ImSpanImGuiTableColumnIdx,Inno.Native.ImGui.ImSpanImGuiTableCellData,System.UInt32*,System.UInt32*,System.UInt32*,Inno.Native.ImGui.ImGuiTableFlags,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int16,System.Int16,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,Inno.Native.ImGui.ImGuiTableRowFlags,Inno.Native.ImGui.ImGuiTableRowFlags,System.Int32,System.UInt32*,System.UInt32,System.UInt32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImRect,Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiWindow*,Inno.Native.ImGui.ImGuiTextBuffer,Inno.Native.ImGui.ImDrawListSplitter*,Inno.Native.ImGui.ImGuiTableInstanceData,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiTableInstanceData},Inno.Native.ImGui.ImGuiTableColumnSortSpecs,Inno.Native.ImGui.ImVector{Inno.Native.ImGui.ImGuiTableColumnSortSpecs},Inno.Native.ImGui.ImGuiTableSortSpecs,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.UInt16,System.UInt16,System.UInt16,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte) 6930872ef238dc8ce098ba906890386698dbbaf2d39621595958385addc2d555 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTable.Destroy a6f5b675368c2e99bc542d789b1c24b34e2fcd47090b33391c53a734ba10e72a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-024ec78b72bd4a96"></a>[`public short Inno.Native.ImGui.ImGuiTable.AngledHeadersCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6510) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de8775af6e7afa16"></a>[`public float Inno.Native.ImGui.ImGuiTable.AngledHeadersHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6462) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cac189528ae72490"></a>[`public float Inno.Native.ImGui.ImGuiTable.AngledHeadersSlope`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6464) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4523f10ae9add20d"></a>[`public short Inno.Native.ImGui.ImGuiTable.AutoFitSingleColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6518) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9debe10770a1589b"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTable.Bg0ClipRectForDrawCmd`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6476) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b6fcbe92d7a0dca"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTable.Bg2ClipRectForDrawCmd`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6478) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-22c2a19c313386c0"></a>[`public ushort Inno.Native.ImGui.ImGuiTable.Bg2DrawChannelCurrent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6552) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e0a85ad194b7643"></a>[`public ushort Inno.Native.ImGui.ImGuiTable.Bg2DrawChannelUnfrozen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6554) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5ea2a29aa363e4a2"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTable.BgClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6474) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-08fecf543b1e1fb1"></a>[`public uint Inno.Native.ImGui.ImGuiTable.BorderColorLight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6430) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-65d97cc6cc0dea27"></a>[`public uint Inno.Native.ImGui.ImGuiTable.BorderColorStrong`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6428) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1962453447d91cc1"></a>[`public float Inno.Native.ImGui.ImGuiTable.BorderX1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6432) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ddaaaa9e154dfa8e"></a>[`public float Inno.Native.ImGui.ImGuiTable.BorderX2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6434) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c5aae149cd354f8"></a>[`public float Inno.Native.ImGui.ImGuiTable.CellPaddingX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6442) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9cede383efc57889"></a>[`public float Inno.Native.ImGui.ImGuiTable.CellSpacingX1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6444) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e60ff3b14c64b1ac"></a>[`public float Inno.Native.ImGui.ImGuiTable.CellSpacingX2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6446) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e77054529ef18ede"></a>[`public Inno.Native.ImGui.ImSpanImGuiTableColumn Inno.Native.ImGui.ImGuiTable.Columns`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6369) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-80cdb33b30310143"></a>[`public float Inno.Native.ImGui.ImGuiTable.ColumnsAutoFitWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6452) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-49139d3d4f676122"></a>[`public int Inno.Native.ImGui.ImGuiTable.ColumnsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6387) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53ce7dd97eba20b4"></a>[`public short Inno.Native.ImGui.ImGuiTable.ColumnsEnabledCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6504) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-00461f42b862d39e"></a>[`public short Inno.Native.ImGui.ImGuiTable.ColumnsEnabledFixedCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6506) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ae835b637b4a0ad8"></a>[`public float Inno.Native.ImGui.ImGuiTable.ColumnsGivenWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6450) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a45a7a6e8850e9d5"></a>[`public Inno.Native.ImGui.ImGuiTextBuffer Inno.Native.ImGui.ImGuiTable.ColumnsNames`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6488) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53346bf9260a9f61"></a>[`public float Inno.Native.ImGui.ImGuiTable.ColumnsStretchSumWeights`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6454) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4458d2533e425f9f"></a>[`public short Inno.Native.ImGui.ImGuiTable.ContextPopupColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6538) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-edd47f71807f36cf"></a>[`public int Inno.Native.ImGui.ImGuiTable.CurrentColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6391) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-167ce14be161d23b"></a>[`public int Inno.Native.ImGui.ImGuiTable.CurrentRow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6389) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f16a8374a1425492"></a>[`public short Inno.Native.ImGui.ImGuiTable.DeclColumnsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6508) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f10a17c2826472ac"></a>[`public byte Inno.Native.ImGui.ImGuiTable.DisableDefaultContextMenu`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6570) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6b8d285e5f9d7626"></a>[`public Inno.Native.ImGui.ImSpanImGuiTableColumnIdx Inno.Native.ImGui.ImGuiTable.DisplayOrderToIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6371) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61b345d2d68e7c12"></a>[`public Inno.Native.ImGui.ImDrawListSplitter* Inno.Native.ImGui.ImGuiTable.DrawSplitter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6490) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-28ead707a871db3f"></a>[`public ushort Inno.Native.ImGui.ImGuiTable.DummyDrawChannel`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6550) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e1fe0b7ede03fa0f"></a>[`public uint* Inno.Native.ImGui.ImGuiTable.EnabledMaskByDisplayOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6375) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab500c0aac8096c6"></a>[`public uint* Inno.Native.ImGui.ImGuiTable.EnabledMaskByIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6377) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1a40767aab00239f"></a>[`public Inno.Native.ImGui.ImGuiTableFlags Inno.Native.ImGui.ImGuiTable.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6363) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ee18080cac935048"></a>[`public short Inno.Native.ImGui.ImGuiTable.FreezeColumnsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6546) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-83776d4df19482f0"></a>[`public short Inno.Native.ImGui.ImGuiTable.FreezeColumnsRequest`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6544) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-faf68566bbb5a4ea"></a>[`public short Inno.Native.ImGui.ImGuiTable.FreezeRowsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6542) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-077bba58a2f0d461"></a>[`public short Inno.Native.ImGui.ImGuiTable.FreezeRowsRequest`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6540) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e282be1e87e9d226"></a>[`public byte Inno.Native.ImGui.ImGuiTable.HasScrollbarYCurr`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6590) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e405cda2d0acb5b8"></a>[`public byte Inno.Native.ImGui.ImGuiTable.HasScrollbarYPrev`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6592) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1974595c5f4c3194"></a>[`public short Inno.Native.ImGui.ImGuiTable.HeldHeaderColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6524) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b9c53f3a28dbd521"></a>[`public short Inno.Native.ImGui.ImGuiTable.HighlightColumnHeader`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6516) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5d9d176c332a1cc0"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTable.HostBackupInnerClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6482) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4af51937d412857c"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTable.HostClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6480) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a3e71d47e5e03ecd"></a>[`public float Inno.Native.ImGui.ImGuiTable.HostIndentX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6436) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d9bc6f6fd4cb50f"></a>[`public byte Inno.Native.ImGui.ImGuiTable.HostSkipItems`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6596) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f2e566a862b35ca1"></a>[`public short Inno.Native.ImGui.ImGuiTable.HoveredColumnBody`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6512) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e3f0a2187a1f8bb"></a>[`public short Inno.Native.ImGui.ImGuiTable.HoveredColumnBorder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6514) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-071d1036dd70c40f"></a>[`public uint Inno.Native.ImGui.ImGuiTable.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6361) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6bff4e08831365e2"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTable.InnerClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6472) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6a8b59ba0b58207e"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTable.InnerRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6468) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-53f7dae043dce775"></a>[`public float Inno.Native.ImGui.ImGuiTable.InnerWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6448) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-72c2b73fd81fd88b"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiTable.InnerWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6486) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3008531855c7f387"></a>[`public short Inno.Native.ImGui.ImGuiTable.InstanceCurrent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6393) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9723593931b56c12"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTableInstanceData> Inno.Native.ImGui.ImGuiTable.InstanceDataExtra`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6494) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-404e4d64f357b1bf"></a>[`public Inno.Native.ImGui.ImGuiTableInstanceData Inno.Native.ImGui.ImGuiTable.InstanceDataFirst`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6492) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-10eec0ee453452ce"></a>[`public short Inno.Native.ImGui.ImGuiTable.InstanceInteracted`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6395) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-106f7ea70609f897"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsActiveIdAliveBeforeTable`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6586) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2fed16cf54829ce6"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsActiveIdInTable`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6588) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aefe7fb3024736ea"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsContextPopupOpen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6568) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa192a5a915ddf04"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsDefaultDisplayOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6576) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-de33235387fce38a"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsDefaultSizingPolicy`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6584) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-05b8867e670e356c"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsInitializing`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6562) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb8d8f29675d2710"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsInsideRow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6560) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a8a30d8ecb6467af"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsLayoutLocked`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6558) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7176d5ebb28634cb"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsResetAllRequest`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6578) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c81ddd73501586ef"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsResetDisplayOrderRequest`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6580) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4784c7ae4a438d39"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsSettingsDirty`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6574) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-05831f9bc3e9707d"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsSettingsRequestLoad`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6572) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-45655e948534ae22"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsSortSpecsDirty`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6564) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ed70671d786d2cea"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsUnfrozenRows`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6582) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a844e6af93825ab"></a>[`public byte Inno.Native.ImGui.ImGuiTable.IsUsingHeaders`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6566) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6e706838b268c531"></a>[`public int Inno.Native.ImGui.ImGuiTable.LastFrameActive`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6385) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2833af2b1e5a7193"></a>[`public short Inno.Native.ImGui.ImGuiTable.LastResizedColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6522) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b865528bfc44d89"></a>[`public short Inno.Native.ImGui.ImGuiTable.LeftMostEnabledColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6530) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b21a1822f8e9f56e"></a>[`public short Inno.Native.ImGui.ImGuiTable.LeftMostStretchedColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6534) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-80e318779808775e"></a>[`public byte Inno.Native.ImGui.ImGuiTable.MemoryCompacted`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6594) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ecc017b642b770d5"></a>[`public float Inno.Native.ImGui.ImGuiTable.MinColumnWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6438) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f4de8f681854fa6d"></a>[`public byte Inno.Native.ImGui.ImGuiTable.NavLayer`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6556) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-eca181cd83d3c729"></a>[`public float Inno.Native.ImGui.ImGuiTable.OuterPaddingX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6440) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-930f37d8164b5f17"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTable.OuterRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6466) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-65621c988c950bc7"></a>[`public Inno.Native.ImGui.ImGuiWindow* Inno.Native.ImGui.ImGuiTable.OuterWindow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6484) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5439e13ea0395362"></a>[`public byte Inno.Native.ImGui.ImGuiTable.RawBits0_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6409) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9552588e68a60e40"></a>[`public byte Inno.Native.ImGui.ImGuiTable.RawBits0_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6411) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb703d55d3871a96"></a>[`public byte Inno.Native.ImGui.ImGuiTable.RawBits0_2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6413) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be21c5ffeb55e709"></a>[`public byte Inno.Native.ImGui.ImGuiTable.RawBits0_3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6415) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3451005782201b20"></a>[`public void* Inno.Native.ImGui.ImGuiTable.RawData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6365) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-09476d7a1d12a493"></a>[`public float Inno.Native.ImGui.ImGuiTable.RefScale`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6460) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e87ec32e395fa84f"></a>[`public short Inno.Native.ImGui.ImGuiTable.ReorderColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6526) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8625b2c86c7e5b31"></a>[`public short Inno.Native.ImGui.ImGuiTable.ReorderColumnDir`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6528) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e0a21c109892c2bb"></a>[`public short Inno.Native.ImGui.ImGuiTable.ResizedColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6520) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-253fd72a3c2707c1"></a>[`public float Inno.Native.ImGui.ImGuiTable.ResizedColumnNextWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6456) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11c648449ceb483e"></a>[`public float Inno.Native.ImGui.ImGuiTable.ResizeLockMinContentsX2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6458) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-71e71e811dee8c1a"></a>[`public short Inno.Native.ImGui.ImGuiTable.RightMostEnabledColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6532) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-374b2bdd64295c4f"></a>[`public short Inno.Native.ImGui.ImGuiTable.RightMostStretchedColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6536) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-60fd709306af6f7c"></a>[`public uint Inno.Native.ImGui.ImGuiTable.RowBgColor_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6422) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b043e5a6adcb60d"></a>[`public uint Inno.Native.ImGui.ImGuiTable.RowBgColor_1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6424) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d38155bc3b56026f"></a>[`public int Inno.Native.ImGui.ImGuiTable.RowBgColorCounter`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6420) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b823f8f1894d460"></a>[`public Inno.Native.ImGui.ImSpanImGuiTableCellData Inno.Native.ImGui.ImGuiTable.RowCellData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6373) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8bcbea27a250a457"></a>[`public short Inno.Native.ImGui.ImGuiTable.RowCellDataCurrent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6548) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ddfe47e19605f1bb"></a>[`public float Inno.Native.ImGui.ImGuiTable.RowCellPaddingY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6403) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3a658a9b527357d3"></a>[`public float Inno.Native.ImGui.ImGuiTable.RowIndentOffsetX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6407) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7655939b67897401"></a>[`public float Inno.Native.ImGui.ImGuiTable.RowMinHeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6401) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe061948a52b64a8"></a>[`public float Inno.Native.ImGui.ImGuiTable.RowPosY1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6397) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cba4a469a52e9e76"></a>[`public float Inno.Native.ImGui.ImGuiTable.RowPosY2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6399) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0a1df0882f93f17b"></a>[`public float Inno.Native.ImGui.ImGuiTable.RowTextBaseline`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6405) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bdbb73741a9260b6"></a>[`public Inno.Native.ImGui.ImGuiTableFlags Inno.Native.ImGui.ImGuiTable.SettingsLoadedFlags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6381) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7ff666fbb0ce608c"></a>[`public int Inno.Native.ImGui.ImGuiTable.SettingsOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6383) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6bd1eaae2dbf277e"></a>[`public Inno.Native.ImGui.ImGuiTableSortSpecs Inno.Native.ImGui.ImGuiTable.SortSpecs`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6500) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b71cd1054c63cd7f"></a>[`public short Inno.Native.ImGui.ImGuiTable.SortSpecsCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6502) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13ceb1aea1d971da"></a>[`public Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTableColumnSortSpecs> Inno.Native.ImGui.ImGuiTable.SortSpecsMulti`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6498) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7945aa00878828b3"></a>[`public Inno.Native.ImGui.ImGuiTableColumnSortSpecs Inno.Native.ImGui.ImGuiTable.SortSpecsSingle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6496) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c18c5d61ab48a721"></a>[`public Inno.Native.ImGui.ImGuiTableTempData* Inno.Native.ImGui.ImGuiTable.TempData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6367) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7604053c7f07f5ca"></a>[`public uint* Inno.Native.ImGui.ImGuiTable.VisibleMaskByIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6379) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ac9fd6357d8f91e"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTable.WorkRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6470) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a51f1a07425ad3d7"></a>[`public Inno.Native.ImGui.ImGuiTable.ImGuiTable(uint iD = 0, Inno.Native.ImGui.ImGuiTableFlags flags = Inno.Native.ImGui.ImGuiTableFlags.None, void* rawData = null, Inno.Native.ImGui.ImGuiTableTempData* tempData = null, Inno.Native.ImGui.ImSpanImGuiTableColumn columns = default(Inno.Native.ImGui.ImSpanImGuiTableColumn), Inno.Native.ImGui.ImSpanImGuiTableColumnIdx displayOrderToIndex = default(Inno.Native.ImGui.ImSpanImGuiTableColumnIdx), Inno.Native.ImGui.ImSpanImGuiTableCellData rowCellData = default(Inno.Native.ImGui.ImSpanImGuiTableCellData), uint* enabledMaskByDisplayOrder = null, uint* enabledMaskByIndex = null, uint* visibleMaskByIndex = null, Inno.Native.ImGui.ImGuiTableFlags settingsLoadedFlags = Inno.Native.ImGui.ImGuiTableFlags.None, int settingsOffset = 0, int lastFrameActive = 0, int columnsCount = 0, int currentRow = 0, int currentColumn = 0, short instanceCurrent = 0, short instanceInteracted = 0, float rowPosY1 = 0, float rowPosY2 = 0, float rowMinHeight = 0, float rowCellPaddingY = 0, float rowTextBaseline = 0, float rowIndentOffsetX = 0, Inno.Native.ImGui.ImGuiTableRowFlags rowFlags = Inno.Native.ImGui.ImGuiTableRowFlags.None, Inno.Native.ImGui.ImGuiTableRowFlags lastRowFlags = Inno.Native.ImGui.ImGuiTableRowFlags.None, int rowBgColorCounter = 0, uint* rowBgColor = null, uint borderColorStrong = 0, uint borderColorLight = 0, float borderX1 = 0, float borderX2 = 0, float hostIndentX = 0, float minColumnWidth = 0, float outerPaddingX = 0, float cellPaddingX = 0, float cellSpacingX1 = 0, float cellSpacingX2 = 0, float innerWidth = 0, float columnsGivenWidth = 0, float columnsAutoFitWidth = 0, float columnsStretchSumWeights = 0, float resizedColumnNextWidth = 0, float resizeLockMinContentsX2 = 0, float refScale = 0, float angledHeadersHeight = 0, float angledHeadersSlope = 0, Inno.Native.ImGui.ImRect outerRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect innerRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect workRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect innerClipRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect bgClipRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect bg0ClipRectForDrawCmd = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect bg2ClipRectForDrawCmd = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect hostClipRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImRect hostBackupInnerClipRect = default(Inno.Native.ImGui.ImRect), Inno.Native.ImGui.ImGuiWindow* outerWindow = null, Inno.Native.ImGui.ImGuiWindow* innerWindow = null, Inno.Native.ImGui.ImGuiTextBuffer columnsNames = default(Inno.Native.ImGui.ImGuiTextBuffer), Inno.Native.ImGui.ImDrawListSplitter* drawSplitter = null, Inno.Native.ImGui.ImGuiTableInstanceData instanceDataFirst = default(Inno.Native.ImGui.ImGuiTableInstanceData), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTableInstanceData> instanceDataExtra = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTableInstanceData>), Inno.Native.ImGui.ImGuiTableColumnSortSpecs sortSpecsSingle = default(Inno.Native.ImGui.ImGuiTableColumnSortSpecs), Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTableColumnSortSpecs> sortSpecsMulti = default(Inno.Native.ImGui.ImVector<Inno.Native.ImGui.ImGuiTableColumnSortSpecs>), Inno.Native.ImGui.ImGuiTableSortSpecs sortSpecs = default(Inno.Native.ImGui.ImGuiTableSortSpecs), short sortSpecsCount = 0, short columnsEnabledCount = 0, short columnsEnabledFixedCount = 0, short declColumnsCount = 0, short angledHeadersCount = 0, short hoveredColumnBody = 0, short hoveredColumnBorder = 0, short highlightColumnHeader = 0, short autoFitSingleColumn = 0, short resizedColumn = 0, short lastResizedColumn = 0, short heldHeaderColumn = 0, short reorderColumn = 0, short reorderColumnDir = 0, short leftMostEnabledColumn = 0, short rightMostEnabledColumn = 0, short leftMostStretchedColumn = 0, short rightMostStretchedColumn = 0, short contextPopupColumn = 0, short freezeRowsRequest = 0, short freezeRowsCount = 0, short freezeColumnsRequest = 0, short freezeColumnsCount = 0, short rowCellDataCurrent = 0, ushort dummyDrawChannel = 0, ushort bg2DrawChannelCurrent = 0, ushort bg2DrawChannelUnfrozen = 0, byte navLayer = 0, byte isLayoutLocked = 0, byte isInsideRow = 0, byte isInitializing = 0, byte isSortSpecsDirty = 0, byte isUsingHeaders = 0, byte isContextPopupOpen = 0, byte disableDefaultContextMenu = 0, byte isSettingsRequestLoad = 0, byte isSettingsDirty = 0, byte isDefaultDisplayOrder = 0, byte isResetAllRequest = 0, byte isResetDisplayOrderRequest = 0, byte isUnfrozenRows = 0, byte isDefaultSizingPolicy = 0, byte isActiveIdAliveBeforeTable = 0, byte isActiveIdInTable = 0, byte hasScrollbarYCurr = 0, byte hasScrollbarYPrev = 0, byte memoryCompacted = 0, byte hostSkipItems = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6597) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7ebac4f4116139b2"></a>[`public void Inno.Native.ImGui.ImGuiTable.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67603) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-327e3dc3d94019e5"></a>[`public Inno.Native.ImGui.ImGuiTableRowFlags Inno.Native.ImGui.ImGuiTable.LastRowFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6417) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-286aa39bb3a1a72c"></a>[`public System.Span<uint> Inno.Native.ImGui.ImGuiTable.RowBgColor { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6425) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5f715e00ce4640c8"></a>[`public Inno.Native.ImGui.ImGuiTableRowFlags Inno.Native.ImGui.ImGuiTable.RowFlags { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6416) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-310cd01e655f1bab"></a>[`public struct Inno.Native.ImGui.ImGuiTable`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6358) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableBgTarget</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableBgTarget e5115c57ac55c6324a44edbe4f155b802b1223a632ebcddc6e9aa9d3ff061bac -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableBgTarget.None 156919d62f8fde0a553924ae912be7a4fd10285d39328b3feb8e4bcbdef988b2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableBgTarget.RowBg0 8556a27c100d183d661bccee7625b95295bfa8d2ee48ffc930c2ac6fc960f471 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableBgTarget.RowBg1 64f63b72be09088ca1abe7f43ad425b50d85826da5796a82a53896cc100fb0ca -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableBgTarget.CellBg 22052f522c36ea2711ec6cc1da607cc04fc02a83d8ffc11b05a619c4a2211cf1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a1148ec3372c248f"></a>[`Inno.Native.ImGui.ImGuiTableBgTarget.CellBg = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1074) | 当前常量或枚举值：`3`。 | — |
| <a id="api-d3dce89ebeccac76"></a>[`Inno.Native.ImGui.ImGuiTableBgTarget.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1071) | 当前常量或枚举值：`0`。 | — |
| <a id="api-2d6c81570a930f17"></a>[`Inno.Native.ImGui.ImGuiTableBgTarget.RowBg0 = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1072) | 当前常量或枚举值：`1`。 | — |
| <a id="api-84f61938cee19c13"></a>[`Inno.Native.ImGui.ImGuiTableBgTarget.RowBg1 = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1073) | 当前常量或枚举值：`2`。 | — |
| <a id="api-2b9fc226f2107b18"></a>[`public enum Inno.Native.ImGui.ImGuiTableBgTarget : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1069) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableCellData</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableCellData ef6828d7e3a3026f4840147e7c01b71ffc930b40eca142a57967a12a016b0b67 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableCellData.BgColor 10bc388ec14c01d009d8503e6ce49dfd2bdeee38087a70e24b082241aeb89ffc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableCellData.Column 10e03bad9be4641c6ba0e3ddfa143a75bd9a6b583e118f8e8304150723006726 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableCellData.#ctor(System.UInt32,System.Int16) c3c9d82520a54efa99c071cacee701267f142f3d19d88809fafbf0def8fb5993 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dd77d582ec37eb2a"></a>[`public uint Inno.Native.ImGui.ImGuiTableCellData.BgColor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6271) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cf418364e978dacb"></a>[`public short Inno.Native.ImGui.ImGuiTableCellData.Column`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6272) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b320ba782f588797"></a>[`public Inno.Native.ImGui.ImGuiTableCellData.ImGuiTableCellData(uint bgColor = 0, short column = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6273) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3ca653f48f6c2100"></a>[`public struct Inno.Native.ImGui.ImGuiTableCellData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6269) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableCellDataPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableCellDataPtr f7d6c095a90e2d0aefb534c6c1b1a8546d464d22e60f81961f657cd93dd31fd1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableCellDataPtr.#ctor(Inno.Native.ImGui.ImGuiTableCellData*) cd8da49de348c8ad9abee0455aef3b41ef71484926c97619be20e1ec1ea7a018 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableCellDataPtr.Handle 2b4c2d98f450ace8ae4db0734dcd711649254e0468c6dd14dbb18f98e8edd47e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableCellDataPtr.IsNull 25e170d23611650536c586ee9f17e25f13cdfe190181891026ffd01e94cf39cc -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableCellDataPtr.Null f07a84363f8c47fd1d669622e15937a95683b634f7f54adf4a0a66cc93095ab7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableCellDataPtr.Item(System.Int32) 759fee70f1f5d7a91dfec15d81a62379f8646cff8522ee8e661296a26479e2b8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableCellDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableCellData*)~Inno.Native.ImGui.ImGuiTableCellDataPtr c9943451a15d83e2c96d1437de12d748714b1d7f886a98fae10082b1350a4a53 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableCellDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableCellDataPtr)~Inno.Native.ImGui.ImGuiTableCellData* bfd537fb367ff876acbdc1b9ce615dfc1d002db4decb95f171af79509e511b05 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableCellDataPtr.op_Equality(Inno.Native.ImGui.ImGuiTableCellDataPtr,Inno.Native.ImGui.ImGuiTableCellDataPtr) 2fab5510716b511493d462fa5e9ef1a80c83207d24b27420fed41bbf535152f6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableCellDataPtr.op_Inequality(Inno.Native.ImGui.ImGuiTableCellDataPtr,Inno.Native.ImGui.ImGuiTableCellDataPtr) 43a15873049c7929751fb16d9d20d9acda4a80b30e1590ae691f36383bc39c97 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableCellDataPtr.Equals(Inno.Native.ImGui.ImGuiTableCellDataPtr) bfd7b600661959d5ed61cd297304591cebe04ab261528272d32a4284bee79056 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableCellDataPtr.Equals(System.Object) 498104233ced512dbb60de8b879009bca2b8e5a011f03951a969c2c7a3daaab7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableCellDataPtr.GetHashCode 98666cd4308ae4b88263ca64e531a2210d68956c7c42035f3ce6e913e8062496 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableCellDataPtr.BgColor 3263d488e3718b8cb7835f8e231d702acc3f57de02569e59b0a0479ea0d168bc -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableCellDataPtr.Column e9fffdfec3ec715301d6f215fe4b4648243b40ffca2317b9b1ad531514386730 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-93e503bd6f60c94e"></a>[`public Inno.Native.ImGui.ImGuiTableCellData* Inno.Native.ImGui.ImGuiTableCellDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16738) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c844659d44aafaa6"></a>[`public Inno.Native.ImGui.ImGuiTableCellDataPtr.ImGuiTableCellDataPtr(Inno.Native.ImGui.ImGuiTableCellData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16733) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7583d1600feffb47"></a>[`public bool Inno.Native.ImGui.ImGuiTableCellDataPtr.Equals(Inno.Native.ImGui.ImGuiTableCellDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16748) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-7045f36f5329ea4c"></a>[`public override bool Inno.Native.ImGui.ImGuiTableCellDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16749) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-3b7378de47b22389"></a>[`public override int Inno.Native.ImGui.ImGuiTableCellDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16750) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-d4d8f27bfa5ab7f1"></a>[`public static bool Inno.Native.ImGui.ImGuiTableCellDataPtr.operator ==(Inno.Native.ImGui.ImGuiTableCellDataPtr left, Inno.Native.ImGui.ImGuiTableCellDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16746) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-76d1b7f9c8056129"></a>[`public static Inno.Native.ImGui.ImGuiTableCellDataPtr.implicit operator Inno.Native.ImGui.ImGuiTableCellDataPtr(Inno.Native.ImGui.ImGuiTableCellData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16744) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6b6736d7f989cb67"></a>[`public static Inno.Native.ImGui.ImGuiTableCellDataPtr.implicit operator Inno.Native.ImGui.ImGuiTableCellData*(Inno.Native.ImGui.ImGuiTableCellDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16745) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2fb2903dbd3c5c6e"></a>[`public static bool Inno.Native.ImGui.ImGuiTableCellDataPtr.operator !=(Inno.Native.ImGui.ImGuiTableCellDataPtr left, Inno.Native.ImGui.ImGuiTableCellDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16747) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7ef92c2732c4dbd2"></a>[`public ref uint Inno.Native.ImGui.ImGuiTableCellDataPtr.BgColor { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16752) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cc304689d58d1d47"></a>[`public ref short Inno.Native.ImGui.ImGuiTableCellDataPtr.Column { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16753) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-46b1155a4ba1056a"></a>[`public bool Inno.Native.ImGui.ImGuiTableCellDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16739) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e9cc0e31acac343f"></a>[`public Inno.Native.ImGui.ImGuiTableCellData Inno.Native.ImGui.ImGuiTableCellDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16742) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-adf1fe8dd1bff0d4"></a>[`public static Inno.Native.ImGui.ImGuiTableCellDataPtr Inno.Native.ImGui.ImGuiTableCellDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16740) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-217594a96a028eb4"></a>[`public struct Inno.Native.ImGui.ImGuiTableCellDataPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTableCellDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16731) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableColumn</code> · 46 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableColumn 8d9800d87a66dd21b16ba1046dec2a1c42ba5d472684edc454673ba63bdd3501 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.Flags 0387d1b437c6d68ff71031c037346f321a26d80f7f0e3c23065aca3d4534405e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.WidthGiven 9cabff12f8fe376605e580409a15ec88784c2ed16e56b3979d4ffdf54d1b87b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.MinX 24edaa7fc2f5703cb6e36ae123b6f82693bede762b763b335612a72cc4ca8a1d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.MaxX 0e4982ca9730afc149e2f10bc223d50cf2693edefc09a4697ad70173b8d1aa10 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.WidthRequest d05af496cf31c3f30d1a484a20277991650cd41c950ca3ff821ed579efe7255b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.WidthAuto 1be87ca6da59f1ef0aa33d0698dd5a5b6d44ce411d4ea881cf2bfa667b97131e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.WidthMax 9465954270fe188f977d2b22d88a1d33ac9b5c5df81281543fdefdaf5b657a29 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.StretchWeight e392a9b452530b97cc1115906df6499a1f3ad3229742ccaee479d09b8a66c1f4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.InitStretchWeightOrWidth c7fca9e28e05374b97fc7cf2b1ad312ab228119920224e2e4af74b8acc9b6632 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.ClipRect f515161a93ae0edfaf4ba465f0a5960b58a64c41e14d349276da01e549f4e499 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.UserID a616d0b8469cc8bfb09ef8b32d3b7f3255423d01c3b0bdb2c56366590db1aec2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.WorkMinX 2b80d68a6e9c240222b66592cff1625f988dd2c95c26c1f2961253f7c92b4a2d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.WorkMaxX 8185bef2f455ab41403f91eeef09adc776dc9d9a25aae2dd54f29ff3bedff89b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.ItemWidth 9e1d42f78f287d9ade1ef675e39ef2fb82e6a48353f8616b9a9bd65e08514a7b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.ContentMaxXFrozen 6bd4f5deed54a91a991bd9ce2bd4ba00a1f43981e0d8678548d73b172b9e2ebb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.ContentMaxXUnfrozen 35a0999288842f8d05dd86888acf2e071b836ff3f0992f322772db59840b1923 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.ContentMaxXHeadersUsed 6475fee42886f8c8e40960bb26bd71dd49e3e35f70b1b34060c9ed08729b83d8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.ContentMaxXHeadersIdeal ea70f3305932e0a0f8a9123cb521c1b08d7c15eebb07f553c0a055840a6e617d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.NameOffset 2ac903d64b196b7a1c89260c0f01116693f64c00b9eb6b1c1dfd7cdf878fb27e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.DisplayOrder b472dcd46684ea888e6bce7dc5ee855c4add31e5221af00c930c49619c7426dd -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.IndexWithinEnabledSet 3d81f21e219cddfc0418de6547d5b772cf9de21f76b0df4e36958a7ea8f9a8ab -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.PrevEnabledColumn c7266eb6c8588c2f6aed2c5579f94f8a1d1f0b36e39d39bb4a57055f53f5ec51 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.NextEnabledColumn 410ca639edbf74a32d37977bd3140dc989a2923084b7c7a82691c3b0bd9b17fc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.SortOrder d8c70d80cc18869b176c9dfd015823f1104d80a4344e886a3b95310237b1225a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.DrawChannelCurrent b499af44548ee5d169e27a15a5016470f2c57b2771e051ba1e63a3130713a691 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.DrawChannelFrozen 6327fe07db921ac8ea78525441ae37e54d62b9fde0592e4a848812c3021d2919 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.DrawChannelUnfrozen 94d02d8f3c18444e0a036f53e0e5c161d817d3fa64acd7d85f4885ccd4c7d176 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.IsEnabled 6ff5a2afe747d3e6c006876bf010910bfaea44c4bf6a3d12fb0811022efe1b9c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.IsUserEnabled 3ec0fd0145e7fe08f1d4dec225570f2b98feb1ad3120dc67f5b1fa1ea67b3334 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.IsUserEnabledNextFrame 00b562ff6633c1d2fbab1213ab4a8d2a49108971771b72d552ba5e7e75252eae -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.IsVisibleX 69bb10ce89b8a7e0e7c153b0259de24aea244c08c50b2cbad09f828f856990c0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.IsVisibleY 2d6525dfbd627ee20b107a58c22a02cf7d8713a0e8556beb02266013119ac6b6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.IsRequestOutput 1d35f113ca60a690ba8728a43109bac69e4d8eb56f2f367a5a2553fc0bca7792 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.IsSkipItems ee944ceb29201bcd6f9ea73362d572743537d5fd376e7602d9dc4597d8c8cdbc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.IsPreserveWidthAuto 8c1372dbfa29b15a9cb7137ac17abf7dc114c096ee3ae98ec92b312a162522b3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.NavLayerCurrent b94ea94e1d69862aba66abb4ce81fbcbc3c768ebbcfcbd382ac95c9f3c8309f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.AutoFitQueue 9a8fc4431ac75d6f6d7be2d71159db11cffaf0a759aba249795bbaa2bc3e72bb -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.CannotSkipItemsQueue 38d50a360d414da913b6649cab33b2df05c437f98ab2ac9dbe5abec105c0b532 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.RawBits0_0 417606fb1ced620236dfa454b3a1404fc016dbfb2a77f442ad9cfeb0bb3be736 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumn.SortDirection ca6ad277aee6ec23570c07337d42eeae115cce311b2c26b4db6a49c82f74f8d5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumn.SortDirectionsAvailCount 341633ef12015eb67b3f19fab6dbabb6a9a70e45376136b3cc664a17ebe1008a -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumn.SortDirectionsAvailMask b247d4a972e06e12dc2be8ccabdc678eae4468900b8cab79fbd9075fe7c83758 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumn.SortDirectionsAvailList bd5d55d005adace07b0dfe96ef6857b77fe832d750d7acebc790b9f0a6f62171 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumn.#ctor(Inno.Native.ImGui.ImGuiTableColumnFlags,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,Inno.Native.ImGui.ImRect,System.UInt32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.Int16,System.UInt16,System.UInt16,System.UInt16,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte) 4a55636fb8624db3de01e8bb84d300d4a280dab8967d297df70287da2e9a64be -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumn.Destroy 715fbb49377193ec9ac31eb2c087e53efa05e035bc904bc7cc7db9d02bc7b246 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8809914415b16de1"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.AutoFitQueue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6177) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fcbe5bccac6e8cde"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.CannotSkipItemsQueue`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6179) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1be5dac5024381c6"></a>[`public Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTableColumn.ClipRect`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6123) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7a935a58943ebefc"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.ContentMaxXFrozen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6133) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b2edf13628d91471"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.ContentMaxXHeadersIdeal`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6139) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-441d38679abbe4c4"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.ContentMaxXHeadersUsed`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6137) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb8ff313c7bbc797"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.ContentMaxXUnfrozen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6135) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b13335173d2f5898"></a>[`public short Inno.Native.ImGui.ImGuiTableColumn.DisplayOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6143) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e2440e1b9ca39ea"></a>[`public ushort Inno.Native.ImGui.ImGuiTableColumn.DrawChannelCurrent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6153) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9db9d8028a464af0"></a>[`public ushort Inno.Native.ImGui.ImGuiTableColumn.DrawChannelFrozen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6155) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f595a1f00bd7d3f"></a>[`public ushort Inno.Native.ImGui.ImGuiTableColumn.DrawChannelUnfrozen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6157) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-225af8b4ce1b60fd"></a>[`public Inno.Native.ImGui.ImGuiTableColumnFlags Inno.Native.ImGui.ImGuiTableColumn.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6105) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6aa9ac29c648993d"></a>[`public short Inno.Native.ImGui.ImGuiTableColumn.IndexWithinEnabledSet`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6145) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad5fb8db5fdec191"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.InitStretchWeightOrWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6121) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f62fa91aa46bcec9"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.IsEnabled`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6159) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c4c092bb20d130e1"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.IsPreserveWidthAuto`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6173) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bab64e4d3fc883e8"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.IsRequestOutput`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6169) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dada9ec47050f615"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.IsSkipItems`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6171) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61f8c0be3e1966ab"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.IsUserEnabled`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6161) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-38b682e3946c8897"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.IsUserEnabledNextFrame`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6163) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cfade77ef4980eb1"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.IsVisibleX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6165) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-576af28f712ea680"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.IsVisibleY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6167) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6266ecb7fb9571be"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.ItemWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6131) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3dd5c8af0bbf7197"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.MaxX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6111) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a2d7c4f20882623"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.MinX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6109) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a5893187a5b6e63c"></a>[`public short Inno.Native.ImGui.ImGuiTableColumn.NameOffset`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6141) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-41159b84a1bae18f"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.NavLayerCurrent`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6175) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-90feb436c4cf43e8"></a>[`public short Inno.Native.ImGui.ImGuiTableColumn.NextEnabledColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6149) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e3fd151505d6890"></a>[`public short Inno.Native.ImGui.ImGuiTableColumn.PrevEnabledColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6147) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-195abcac5bfc4051"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.RawBits0_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6181) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a272f66a46241dde"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.SortDirectionsAvailList`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6187) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1b87b439dfc18158"></a>[`public short Inno.Native.ImGui.ImGuiTableColumn.SortOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6151) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fcb01bb628c70ab6"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.StretchWeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6119) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-049dd941fe395d91"></a>[`public uint Inno.Native.ImGui.ImGuiTableColumn.UserID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6125) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2724d5c18da8c567"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.WidthAuto`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6115) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-701208f996b0af84"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.WidthGiven`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6107) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5554518d2c669f66"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.WidthMax`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6117) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-15bc6e61c93f871b"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.WidthRequest`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6113) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d2e7c5f7f088fbb"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.WorkMaxX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6129) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01dad11d031cc8a8"></a>[`public float Inno.Native.ImGui.ImGuiTableColumn.WorkMinX`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6127) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-57ef87c147359835"></a>[`public Inno.Native.ImGui.ImGuiTableColumn.ImGuiTableColumn(Inno.Native.ImGui.ImGuiTableColumnFlags flags = Inno.Native.ImGui.ImGuiTableColumnFlags.None, float widthGiven = 0, float minX = 0, float maxX = 0, float widthRequest = 0, float widthAuto = 0, float widthMax = 0, float stretchWeight = 0, float initStretchWeightOrWidth = 0, Inno.Native.ImGui.ImRect clipRect = default(Inno.Native.ImGui.ImRect), uint userID = 0, float workMinX = 0, float workMaxX = 0, float itemWidth = 0, float contentMaxXFrozen = 0, float contentMaxXUnfrozen = 0, float contentMaxXHeadersUsed = 0, float contentMaxXHeadersIdeal = 0, short nameOffset = 0, short displayOrder = 0, short indexWithinEnabledSet = 0, short prevEnabledColumn = 0, short nextEnabledColumn = 0, short sortOrder = 0, ushort drawChannelCurrent = 0, ushort drawChannelFrozen = 0, ushort drawChannelUnfrozen = 0, byte isEnabled = 0, byte isUserEnabled = 0, byte isUserEnabledNextFrame = 0, byte isVisibleX = 0, byte isVisibleY = 0, byte isRequestOutput = 0, byte isSkipItems = 0, byte isPreserveWidthAuto = 0, byte navLayerCurrent = 0, byte autoFitQueue = 0, byte cannotSkipItemsQueue = 0, byte sortDirection = 0, byte sortDirectionsAvailCount = 0, byte sortDirectionsAvailMask = 0, byte sortDirectionsAvailList = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6188) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0ac60e2fe60b2198"></a>[`public void Inno.Native.ImGui.ImGuiTableColumn.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67575) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e66128bbe9fcccb2"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.SortDirection { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6182) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-96cf0c50e0e16344"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.SortDirectionsAvailCount { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6183) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6b72d249a8240a13"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumn.SortDirectionsAvailMask { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6184) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-49fd92478c5669cf"></a>[`public struct Inno.Native.ImGui.ImGuiTableColumn`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6102) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableColumnFlags</code> · 29 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableColumnFlags fef18130f039de8a7250ac68f12ce53eb0c57151085eb21780e4e4d1ee873dc8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.None 4cd871053a4f61ac0d7f2a3169897b626faeb73f9266d473bc949c6978d34e28 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.Disabled cf53fc7ec2ab82b46377e0f314e507709bba14db7daac60598e16a0ce645ca87 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.DefaultHide 2b5e3c2ff6c99327286b288730060f10e89802f87ad73f05534818de1d563c39 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.DefaultSort e8e94b1a119c6cc55a0bf811ebeba913dfe9969b2aa14b3235ab3eb645cc193e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.WidthStretch 0d358350e76cb111b41023bf38818c8ed8afde0ed8f763b9f7e2da31b06c6356 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.WidthFixed fd78ec583f9c7294cf21f5eb43036f2ff6d84221f3913759a5c8d416ad924293 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.NoResize 59bb9de588780d1e7aeaa89567e7487130075268c3b69189759adea894a8c141 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.NoReorder 9c8cf454041ce8782d3f2fe1ebf73aea543d1a3ee750182e0e39e8ec459cebb8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.NoHide 811985fa4e345beb1f1665cc4bb290bca9874c1f068053f2b73a6bf4c051d122 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.NoClip 7bbe3932e68a1ba554c180a321cda9cbb6c7d1a2e2af4a63bc7df21b5b81c757 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.NoSort 0ef4f5a9c429b204332e5e561c3f0491c4ea20250d346f66940da500f26fa14e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.NoSortAscending c873872107005343513ce8c5e770961b7f614bc9f60fadbd926649090910ce36 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.NoSortDescending 11c834642e6b86c387fdda4bb0fbac19005176e5f6613f4eb139ff8a732c9464 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.NoHeaderLabel a11cdd4a98ef62623b8243713dfa7eaf5d55a24e65ebb4775ff9bd6c36575778 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.NoHeaderWidth bbec17d653295aa94813bac856373c9517bdd6209776679162470ed4b9769c72 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.PreferSortAscending 420f941fc4f9140f701858b191ab4cb66009e0b3d71e75eec6e72bf1a77ca0ad -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.PreferSortDescending 2224088596f84896d3929794325b26b368e98bb4e716997b33f3dea429f10b06 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.IndentEnable 0c28b00f575e66972547255fd425571751e7033ec8ce3a4f55562b5fd46e9bba -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.IndentDisable 141a28b3d5be4c299e866a748ec907f3b045e0d6228954d7ec4fa7f96b01f182 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.AngledHeader f43df01ebf8ad9ccafae915233ca4d124ccc6c26973832139a7d1696a6202b97 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.IsEnabled 6688c6f6200b11ebd4afede79e2b71002c321cb0e92d2e1c58489376a4ba6b5f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.IsVisible 27db26a490184f241ad100d4c26cf9bb6b1d3ff13262aa96d4864d3abc785b75 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.IsSorted 1e2c390b066c66d5a7144bfa320281e0c44677f02771a90b1e7bc9f5b6b7dc73 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.IsHovered 491b1fd0488754381d3802501f8d58a74dfc35d95a5deb50bd59b1a873f143f7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.WidthMask 8d1c301af53bab8d4d08b8b4faed2e73a29b86837ee34bb341eba1165eb5cc18 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.IndentMask edd26b5bdab434e60bc14bd209dada0574151e7b4e023b8f50caadae3edb33e9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.StatusMask 76325e3b813821f4ffb1f82734f27ae87b8f188604cc1ce496195940117906c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnFlags.NoDirectResize 407a72294d5ccf841c865f8b2896e47ff2cb8f4beef7f40a081e3feb057f7ab5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8c52e98bcef23bef"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.AngledHeader = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1046) | 当前常量或枚举值：`262144`。 | — |
| <a id="api-8d7dca1acad160b2"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.DefaultHide = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1029) | 当前常量或枚举值：`2`。 | — |
| <a id="api-6afbcaff57466810"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.DefaultSort = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1030) | 当前常量或枚举值：`4`。 | — |
| <a id="api-4cb4032f32e71031"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.Disabled = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1028) | 当前常量或枚举值：`1`。 | — |
| <a id="api-69c0476a3207d513"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.IndentDisable = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1045) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-e4dc3c80f2c07a69"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.IndentEnable = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1044) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-9140022456b0f718"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.IndentMask = 196608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1052) | 当前常量或枚举值：`196608`。 | — |
| <a id="api-609b696e8604fc1d"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.IsEnabled = 16777216`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1047) | 当前常量或枚举值：`16777216`。 | — |
| <a id="api-ea50a70c79f27f57"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.IsHovered = 134217728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1050) | 当前常量或枚举值：`134217728`。 | — |
| <a id="api-e4ef4cf30a84db4b"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.IsSorted = 67108864`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1049) | 当前常量或枚举值：`67108864`。 | — |
| <a id="api-625d28885cd22b69"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.IsVisible = 33554432`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1048) | 当前常量或枚举值：`33554432`。 | — |
| <a id="api-387150a150deeddd"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.NoClip = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1036) | 当前常量或枚举值：`256`。 | — |
| <a id="api-5677893b24d28f01"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.NoDirectResize = 1073741824`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1054) | 当前常量或枚举值：`1073741824`。 | — |
| <a id="api-1ab71494df288de3"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.NoHeaderLabel = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1040) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-e18bc3ea74d0fd62"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.NoHeaderWidth = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1041) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-ae17b76f7b4badf1"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.NoHide = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1035) | 当前常量或枚举值：`128`。 | — |
| <a id="api-f9e7411558471717"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1027) | 当前常量或枚举值：`0`。 | — |
| <a id="api-93ef0e09c0d4ff8e"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.NoReorder = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1034) | 当前常量或枚举值：`64`。 | — |
| <a id="api-e2ada2b2c970ed76"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.NoResize = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1033) | 当前常量或枚举值：`32`。 | — |
| <a id="api-60f2d5ff19464178"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.NoSort = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1037) | 当前常量或枚举值：`512`。 | — |
| <a id="api-708b13dc6c6f0334"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.NoSortAscending = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1038) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-2b3507f45c7c27ab"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.NoSortDescending = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1039) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-f9914e369a5ca77d"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.PreferSortAscending = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1042) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-4c12aa792805a85b"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.PreferSortDescending = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1043) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-8431a7ad3327764f"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.StatusMask = 251658240`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1053) | 当前常量或枚举值：`251658240`。 | — |
| <a id="api-29c9ae3e43d32303"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.WidthFixed = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1032) | 当前常量或枚举值：`16`。 | — |
| <a id="api-d3a38e54a0527f3e"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.WidthMask = 24`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1051) | 当前常量或枚举值：`24`。 | — |
| <a id="api-d952c06c2f46e09e"></a>[`Inno.Native.ImGui.ImGuiTableColumnFlags.WidthStretch = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1031) | 当前常量或枚举值：`8`。 | — |
| <a id="api-ee984dd68aff5816"></a>[`public enum Inno.Native.ImGui.ImGuiTableColumnFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1025) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableColumnPtr</code> · 56 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableColumnPtr 7b523262dc93d7823c9707d4e5af78b05d81bb0d0381e549d9462d7cd1697695 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnPtr.#ctor(Inno.Native.ImGui.ImGuiTableColumn*) 86da641415da66862df9f8ce8fd30dbf4fd9c09cb34e32da3c6a6f745f13580b -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnPtr.Handle b4c432ace3aacb5ba9103fedd8086dcfcd39f8cc36843f0cd9ab610d5a5257c5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.IsNull e15e451eb78e4894c7c8a5f54c61f5c821efb76d3029be48be630cbd9d799cfa -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.Null aac82be3afcc87019bd8bfb22cc9a1712bb4b541fc860633fd1eab3640ca28dd -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.Item(System.Int32) 28e4bdb0a94a8bc85f5ce1a7474c8e2db704541c57d17688cd712bc5ef3389aa -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableColumn*)~Inno.Native.ImGui.ImGuiTableColumnPtr 10c66883357d3cd1126db6edbf723af2819249a25cbc7dbc8d2fb53b89d529dd -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableColumnPtr)~Inno.Native.ImGui.ImGuiTableColumn* d5a345de65c0dc04a0bc02bc55a930255687422833ab51f7d8786439597c2902 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnPtr.op_Equality(Inno.Native.ImGui.ImGuiTableColumnPtr,Inno.Native.ImGui.ImGuiTableColumnPtr) 3fad598d78e1535aa96960750f06e340d37dcc10bc8f0e50b4234b7601bd07ea -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnPtr.op_Inequality(Inno.Native.ImGui.ImGuiTableColumnPtr,Inno.Native.ImGui.ImGuiTableColumnPtr) 740c347addf86e3e19cfdc4c59ce579388f5f0da9ba5fb52bc5df62719452bdd -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnPtr.Equals(Inno.Native.ImGui.ImGuiTableColumnPtr) a646a80d9db639ecd342100d42630c5a719a57b2cc29f68fbe12826dce730e47 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnPtr.Equals(System.Object) 00bb57d11559624e62d0f6f20ec2226bbdf888c9b472d89db1c55148545fcdd5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnPtr.GetHashCode dc9d1068ed86b4f7a4272e4d2529c464801a9747b070e2f8daedd67851a3e17c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.Flags ba58df7226b7fb441526531ec3d15487873c50435eb174f0b2bd1d0f8b7f421b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.WidthGiven 727aecd38599745032acbce1c2cca81d4e4fd181695066848afdb8529bf1a904 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.MinX 17a210bdb2700ab35e82ad3afbce1c6542c0a04a23efbe97492d2620018c5f9c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.MaxX 5c499a4defed7fa52d12bcc08c2f1bb9ec8ee888419fc083414c13e09d304941 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.WidthRequest e9982252a74589208a925fa73838c1bc239a892d28debd8c6e361f9c33fe84bb -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.WidthAuto 42ff51356eabdbe65605c55565d8fbf1eef6cad5ec2b3874823b40bdf2a362dc -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.WidthMax e6755781a48e1a6b6c7ef38fe7978ba71adefab42817923f15f91f2bfa5f04c7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.StretchWeight 33d7d59f31d9377a40c886dc38cdd268f94ff929f4d3720cbd5872b0a1ea833a -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.InitStretchWeightOrWidth bad9027579e1746164a7e89d92bc2938edb2096bd357af794a93e72e1649ac65 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.ClipRect 2828df3c39a3c39897b1d4beea2281b532a501ac65a6565c578d8852d2f9db89 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.UserID 11e5f88eaa2c6bc386d331c5cf393d9cd9dc6e6f020c79d8d9c122d3c820485a -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.WorkMinX c377ad5392cec70f788e74b93a14e40876e07939c05732a6fd48e735cd3350f9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.WorkMaxX 3fa4d6e35cfa93e955ce6b0571013776e99c17678309e802d75c5310a567e44f -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.ItemWidth 6ba3ad6ddb5cb3ed8871159eb0236f7699d66001d9a1243d9a2621021892b29c -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.ContentMaxXFrozen fe93ff36077595d9385eb654561922feef00bddc95c14c0b7a6238a1e520b23b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.ContentMaxXUnfrozen 221c529972a5cfb2547ebd8c5e1464c730bc08850251490ac82aad1a2efc5c0a -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.ContentMaxXHeadersUsed 5517b667c1f8b36778f26b3402b4b7acde70cc3f5bf55d6e2f56d6bb7e38cd27 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.ContentMaxXHeadersIdeal 3aa7f2e1665e180bd8fb51c9e235f1d18deca1ef5ac3477f630983f5ed4531a4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.NameOffset 0948d2ee47c74769a0eeb497396eb6ac264bf637430e83138633b6ce91dccf3e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.DisplayOrder d0464590ef779ce761813e1d38cb601898e4b30842e1e8cb387572e77ea947cd -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.IndexWithinEnabledSet 75e0a452ae10506538fcdb6e8108b0abbed2f4dfe1e02e34ead5f898beb3cea9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.PrevEnabledColumn 2002b1aae315858eb3ef077213c1f9cb52523f8e1f8cd7b32736d10d0496c8ea -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.NextEnabledColumn e06fc2f22ebc2f9cb5d0d7762b8d9bc125de400480662bd3d1bd8a47362fa761 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.SortOrder 463885ca713eea5e6e038e5d47830411599a7de16c8915f1c9fa1679d3cd331b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.DrawChannelCurrent b1afe9817441960573bbe73b7e6b7228e618fe46f598348ba03dd805a8b68280 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.DrawChannelFrozen 5561c4a7e2e8c0818c29a8fc3d57f7a36e8bff0d14c8097b5c79ec15b9a90039 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.DrawChannelUnfrozen 7518994d1e64f17f8c49c3ad545bd1338ead68bbff3d920eca233635671138e2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.IsEnabled b96acbf17b1c8399dac40f63147bb4c7a25d52b6decc4cff016280a9e61c7fb2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.IsUserEnabled f9fa301a24724b286a830091864cb72ec2db812fe70847a5fc5ba9cc6b7c959e -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.IsUserEnabledNextFrame 13442079b4f2a09f49eb9224e52011fdbc1271be4fdc894b3f85286882753e96 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.IsVisibleX 651b499704cba1279382a0ea98adbc17e95ca3ef940f64b7b5d4d59250fcaeb9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.IsVisibleY b99d534e841c871f24e2ad1abbb18ca9ed5268ae9b73e7b80b43d20696b7af14 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.IsRequestOutput 714e9738f07555a4b8c7cab9caee9fc949c349e87569dfc99430cc7156a1f736 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.IsSkipItems fb28cd078063da3bf021523ad698137c7e03835486539b8f975e32f621748d34 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.IsPreserveWidthAuto 776b918099b3593c182b73d6d75f667de4f28236d7924588a87a83566acf46a2 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.NavLayerCurrent d25288ba3279d88aeb5fff5e8c6b3c9113a964d4452e9f87711b0f0d1a605554 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.AutoFitQueue 715a4d00fdc48ca6b2cac0ed4d17b591109a505d1bc773c16a25362f45bf7e0b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.CannotSkipItemsQueue 64b88e6fb0b9b5ac37642a8a1aefe49e96a4ff736d88a6b9f01a8791663ae230 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.SortDirection 5c965913d288809c04744a07d03af25f4aedabffa02562050da576a84ab6ee48 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.SortDirectionsAvailCount 15dab32c55ed63fff2c251ca58fd6265ab61babe26e4861554d286c98515f36f -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.SortDirectionsAvailMask 532475822227c3cc63ad817b1000c6ff899a5c2a5cc23b3fc7a57155ec5c2be9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnPtr.SortDirectionsAvailList ed308221f0e81177e81a3c810a527833dcd78aa27e58b4003cb132cc35d1647d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnPtr.Destroy 8260801bbaf2a15c0271385fc20acdde07ad6673ab87acd9dcce4ac12b23ac9d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-06c045767a33da2f"></a>[`public Inno.Native.ImGui.ImGuiTableColumn* Inno.Native.ImGui.ImGuiTableColumnPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16764) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-552485d6b08a0a6d"></a>[`public Inno.Native.ImGui.ImGuiTableColumnPtr.ImGuiTableColumnPtr(Inno.Native.ImGui.ImGuiTableColumn* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16759) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ddccc51a1d5b5bea"></a>[`public void Inno.Native.ImGui.ImGuiTableColumnPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16824) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-78a50102cf3b0ded"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnPtr.Equals(Inno.Native.ImGui.ImGuiTableColumnPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16774) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-b3eb88e878909470"></a>[`public override bool Inno.Native.ImGui.ImGuiTableColumnPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16775) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-11172e74901ef9e4"></a>[`public override int Inno.Native.ImGui.ImGuiTableColumnPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16776) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-19b199262edc16a9"></a>[`public static bool Inno.Native.ImGui.ImGuiTableColumnPtr.operator ==(Inno.Native.ImGui.ImGuiTableColumnPtr left, Inno.Native.ImGui.ImGuiTableColumnPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16772) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6409373f3503af89"></a>[`public static Inno.Native.ImGui.ImGuiTableColumnPtr.implicit operator Inno.Native.ImGui.ImGuiTableColumnPtr(Inno.Native.ImGui.ImGuiTableColumn* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16770) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5e3b528286bbfedf"></a>[`public static Inno.Native.ImGui.ImGuiTableColumnPtr.implicit operator Inno.Native.ImGui.ImGuiTableColumn*(Inno.Native.ImGui.ImGuiTableColumnPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16771) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c8e39d58ddce1b50"></a>[`public static bool Inno.Native.ImGui.ImGuiTableColumnPtr.operator !=(Inno.Native.ImGui.ImGuiTableColumnPtr left, Inno.Native.ImGui.ImGuiTableColumnPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16773) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-113e16f8f6441004"></a>[`public ref byte Inno.Native.ImGui.ImGuiTableColumnPtr.AutoFitQueue { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16814) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-09b73a00885f7a65"></a>[`public ref byte Inno.Native.ImGui.ImGuiTableColumnPtr.CannotSkipItemsQueue { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16815) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-06c27ca7e1f08f6f"></a>[`public ref Inno.Native.ImGui.ImRect Inno.Native.ImGui.ImGuiTableColumnPtr.ClipRect { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16787) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9b07540066b48000"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.ContentMaxXFrozen { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16792) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ed783dedd5adf391"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.ContentMaxXHeadersIdeal { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16795) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-083f45bc36dab3b8"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.ContentMaxXHeadersUsed { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16794) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bffa3761527ee0c0"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.ContentMaxXUnfrozen { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16793) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bf3a8ecd34e74ed1"></a>[`public ref short Inno.Native.ImGui.ImGuiTableColumnPtr.DisplayOrder { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16797) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c5c631fcf2d93103"></a>[`public ref ushort Inno.Native.ImGui.ImGuiTableColumnPtr.DrawChannelCurrent { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16802) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-60abe0831e07367f"></a>[`public ref ushort Inno.Native.ImGui.ImGuiTableColumnPtr.DrawChannelFrozen { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16803) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ed332a2774717789"></a>[`public ref ushort Inno.Native.ImGui.ImGuiTableColumnPtr.DrawChannelUnfrozen { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16804) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-70220150ba9ca46b"></a>[`public ref Inno.Native.ImGui.ImGuiTableColumnFlags Inno.Native.ImGui.ImGuiTableColumnPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16778) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-92f2b7a2d7dac444"></a>[`public ref short Inno.Native.ImGui.ImGuiTableColumnPtr.IndexWithinEnabledSet { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16798) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-905eea166f6d8b2c"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.InitStretchWeightOrWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16786) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-25345d458fa04300"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnPtr.IsEnabled { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16805) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d5aa97e4a2108adc"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16765) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9884a8bcf156f8bb"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnPtr.IsPreserveWidthAuto { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16812) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1aa77e060ba3fde0"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnPtr.IsRequestOutput { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16810) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-88d44d6a4e93e0b3"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnPtr.IsSkipItems { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16811) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0a324bf702fbab83"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnPtr.IsUserEnabled { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16806) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-90b2853451b65326"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnPtr.IsUserEnabledNextFrame { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16807) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b6526fde9ad79a5b"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnPtr.IsVisibleX { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16808) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8e70720236609610"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnPtr.IsVisibleY { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16809) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8c11bf2d281460c0"></a>[`public Inno.Native.ImGui.ImGuiTableColumn Inno.Native.ImGui.ImGuiTableColumnPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16768) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5d7642685a171a7a"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.ItemWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16791) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a05e15109e5fc11b"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.MaxX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16781) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7b9ed28c753d8ac4"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.MinX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16780) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b6b3df41460d0328"></a>[`public ref short Inno.Native.ImGui.ImGuiTableColumnPtr.NameOffset { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16796) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-394c553da54ac432"></a>[`public ref byte Inno.Native.ImGui.ImGuiTableColumnPtr.NavLayerCurrent { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16813) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7b79d9b4edbeb839"></a>[`public ref short Inno.Native.ImGui.ImGuiTableColumnPtr.NextEnabledColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16800) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4f7847e8a8ef00ae"></a>[`public static Inno.Native.ImGui.ImGuiTableColumnPtr Inno.Native.ImGui.ImGuiTableColumnPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16766) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d866704dffb5ee91"></a>[`public ref short Inno.Native.ImGui.ImGuiTableColumnPtr.PrevEnabledColumn { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16799) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-29202a82314d3025"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumnPtr.SortDirection { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16816) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9fef8a206912ba7d"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumnPtr.SortDirectionsAvailCount { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16817) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ac66519ee15d285a"></a>[`public ref byte Inno.Native.ImGui.ImGuiTableColumnPtr.SortDirectionsAvailList { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16819) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6c19fa55a93b6856"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumnPtr.SortDirectionsAvailMask { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16818) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2df53a792555b6ec"></a>[`public ref short Inno.Native.ImGui.ImGuiTableColumnPtr.SortOrder { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16801) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ea504bba5b8fe0d9"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.StretchWeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16785) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a128d1b81aed518c"></a>[`public ref uint Inno.Native.ImGui.ImGuiTableColumnPtr.UserID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16788) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b5a30739a9d6daaf"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.WidthAuto { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16783) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f8548fe7967f68b6"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.WidthGiven { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16779) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d3df9f5d8580a709"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.WidthMax { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16784) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-60830d42d91a9a15"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.WidthRequest { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16782) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c71e15ed5fcda3ae"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.WorkMaxX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16790) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a76faa249488f3ec"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnPtr.WorkMinX { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16789) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1b80f2c39fe4c317"></a>[`public struct Inno.Native.ImGui.ImGuiTableColumnPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTableColumnPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16757) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableColumnSettings</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableColumnSettings 7f1e91f8699eb99016ec288a9092ba4d49c385965de709a83657accb588128d7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSettings.WidthOrWeight b930f97f15dda069a59fccbc06ace2c40ea167e7027695f757d9a4961e8053fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSettings.UserID dd7780b7280756f1ad96fa15af2b2daa323689e2c05793ca3c93d78b8d04ce03 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSettings.Index 3b6f196a62660100f1ad199bfe1a59d15245582d06bd266772b69d2a8ac0dfe3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSettings.DisplayOrder 07409fc95a0dda1741c3511a1d6ca49ad2ba25ab9ef9cf365a01427ca3249a49 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSettings.SortOrder d50e60ae441dc84080ad72ce2037989866d5f0746391d2f07975b79e30648304 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSettings.RawBits0_0 2d954084d084ffbba3b53bde6788be6f72ef36a16ba4b5902577aa5170063484 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettings.SortDirection b55e65f2c5ed14c85ebae8c159e8399290d17aaeac990089a99dee240354cef7 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettings.IsEnabled 3a09c2ce188afb0618d5dbb7949cb42c15ba7e196357d46873a66aa2c0687bf4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettings.IsStretch 316f8f3f55b5160be1cc94092edd7d6e49f4b70b97870437251539b68ec1ba49 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSettings.#ctor(System.Single,System.UInt32,System.Int16,System.Int16,System.Int16,System.Byte,System.Byte,System.Byte) 8aa9eb4b0d354166aabd1b8231171a5f3cb50bb37b026cfa7d17dec896f52296 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSettings.Destroy 696c73058d178f9bd62d408624a7290b2d4e456e2dae9982b34cc214801e28a3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-db83a78bcbfa64f4"></a>[`public short Inno.Native.ImGui.ImGuiTableColumnSettings.DisplayOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10356) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-472c62a4557a3efb"></a>[`public short Inno.Native.ImGui.ImGuiTableColumnSettings.Index`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10354) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1d86303057a78b32"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumnSettings.RawBits0_0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10360) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d059053ebc6646b"></a>[`public short Inno.Native.ImGui.ImGuiTableColumnSettings.SortOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10358) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4553f349e15c6adf"></a>[`public uint Inno.Native.ImGui.ImGuiTableColumnSettings.UserID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10352) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cabdc2d5ecfcfb3e"></a>[`public float Inno.Native.ImGui.ImGuiTableColumnSettings.WidthOrWeight`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10350) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8acc11a728d30b46"></a>[`public Inno.Native.ImGui.ImGuiTableColumnSettings.ImGuiTableColumnSettings(float widthOrWeight = 0, uint userID = 0, short index = 0, short displayOrder = 0, short sortOrder = 0, byte sortDirection = 0, byte isEnabled = 0, byte isStretch = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10365) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fe11898ac8d4554e"></a>[`public void Inno.Native.ImGui.ImGuiTableColumnSettings.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L67631) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5bf78b9198fa4d62"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumnSettings.IsEnabled { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10362) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2606d60cfd38264d"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumnSettings.IsStretch { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10363) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8598cc1f89e4e693"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumnSettings.SortDirection { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10361) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-45b9edb2593b1250"></a>[`public struct Inno.Native.ImGui.ImGuiTableColumnSettings`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10347) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableColumnSettingsPtr</code> · 22 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr d6bde1fb125e0c7a59577079a02ca646b5620a1ff548af1e46701521749da242 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.#ctor(Inno.Native.ImGui.ImGuiTableColumnSettings*) cd2a9ff0cfd3412daa6eb11a87714604faeaba2763732a4be3512a9a1bdceed2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.Handle c16210f618dedaa519f90e06e2486c8ee2d424984ef67778ba5e48a2ec101786 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.IsNull 0fa721db47c9ef777413cf4da8aaf1349ad9360179866a58eccf8b1b0f7f43b8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.Null 5277b28fe332fde86231e865cf3af09cf39d1523e478b186aecc810ef32651d5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.Item(System.Int32) 53a45ab3c10f8d4f2feb87bcb458433b4668d058662ada938787e7ee11706675 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableColumnSettings*)~Inno.Native.ImGui.ImGuiTableColumnSettingsPtr 019a399a7b0ead1d25f4966ce215a2b427ccdeaec901042c995fc8b73f1d3bd8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableColumnSettingsPtr)~Inno.Native.ImGui.ImGuiTableColumnSettings* be6c72111a250ebe1851f149402f4c186badd0cecf3e854c7a741eb5ac3144b1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.op_Equality(Inno.Native.ImGui.ImGuiTableColumnSettingsPtr,Inno.Native.ImGui.ImGuiTableColumnSettingsPtr) b54e7d2bc075b04ccdab3aaf9986714afb6faccb92bb4cbd1b91fbc017595ac3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.op_Inequality(Inno.Native.ImGui.ImGuiTableColumnSettingsPtr,Inno.Native.ImGui.ImGuiTableColumnSettingsPtr) dc53280da2ab2c43533fd34cf25061fc2ff845d62626e351a61bd1eb382a1fa7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.Equals(Inno.Native.ImGui.ImGuiTableColumnSettingsPtr) 83d08879b7281b9e85d2898e50b781400ae4d641f85a0d613351019a2b4572c4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.Equals(System.Object) 903335c04bb613a5dbb42da63db8f92a935a40c03367d655fafebfca9d8dc203 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.GetHashCode 70283e6003a7910b9cffaf25803d7b01233e63bdc0af9b87966a093872898b81 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.WidthOrWeight 08af7c935a04b9aaeb85bbb0d350433fcd7f1245c1a39a92df450fe6bed3d0b8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.UserID fdec5596a21153a0928539fa66cd4f0fb7ad9e46533ca5d015596a9d5c59ec67 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.Index 18fc5e711a78d9720b7b8dad7f41ac7c2e95aa55dff660245fae91010229a3ef -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.DisplayOrder 771951fe1ad3e7c957c07901bf0587fb080f21e90486f1d65eb70bf6526e2c81 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.SortOrder 41f00bdf204dc2aaf3f835d0ad6ea573f8e1ebbe1e2cd6d39c6421bdae83a95a -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.SortDirection 65a9d4db0c71ab6f2e00f0ef6359d837805ef61bb62a5294c6807a5d2f60ddad -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.IsEnabled 2211e2fac0f0e5cefb8d68e691937fdf063a651c2c300a7a1ee066b4945ae279 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.IsStretch 63a47f6f6343ec409ab7c60cbfc33846db21e2c8894f40592aa23dce61ed2f8d -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.Destroy d39ab9e00f671daeed279d726faf07dc3c4686f5a0a529e38306eb80ffbfb53a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-296999407ae03c1d"></a>[`public Inno.Native.ImGui.ImGuiTableColumnSettings* Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16838) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b13a249ad697a841"></a>[`public Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.ImGuiTableColumnSettingsPtr(Inno.Native.ImGui.ImGuiTableColumnSettings* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16833) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-53e464d883813126"></a>[`public void Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16864) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3781c21e69598779"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.Equals(Inno.Native.ImGui.ImGuiTableColumnSettingsPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16848) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-be8037c650298dd8"></a>[`public override bool Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16849) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-0091c6b0fc6ac1c3"></a>[`public override int Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16850) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-1c81f2c3319bc07e"></a>[`public static bool Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.operator ==(Inno.Native.ImGui.ImGuiTableColumnSettingsPtr left, Inno.Native.ImGui.ImGuiTableColumnSettingsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16846) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-663caa8c28e80ac0"></a>[`public static Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.implicit operator Inno.Native.ImGui.ImGuiTableColumnSettingsPtr(Inno.Native.ImGui.ImGuiTableColumnSettings* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16844) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fc6408cf4afcabdc"></a>[`public static Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.implicit operator Inno.Native.ImGui.ImGuiTableColumnSettings*(Inno.Native.ImGui.ImGuiTableColumnSettingsPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16845) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-260f22337f514e95"></a>[`public static bool Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.operator !=(Inno.Native.ImGui.ImGuiTableColumnSettingsPtr left, Inno.Native.ImGui.ImGuiTableColumnSettingsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16847) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-632aab4e41e52391"></a>[`public ref short Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.DisplayOrder { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16855) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-354b8bd7fa33f60f"></a>[`public ref short Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.Index { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16854) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bc6c310205fb1088"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.IsEnabled { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16858) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-afb7c90d218bd97d"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16839) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-80ce130589acc51e"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.IsStretch { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16859) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2a14f144627c7cfa"></a>[`public Inno.Native.ImGui.ImGuiTableColumnSettings Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16842) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6b795b44fa56be9c"></a>[`public static Inno.Native.ImGui.ImGuiTableColumnSettingsPtr Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16840) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b7155d2646868b61"></a>[`public byte Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.SortDirection { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16857) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-14e355a488727e37"></a>[`public ref short Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.SortOrder { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16856) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6dcc42d5ec1327da"></a>[`public ref uint Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.UserID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16853) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b73cd62306742b2a"></a>[`public ref float Inno.Native.ImGui.ImGuiTableColumnSettingsPtr.WidthOrWeight { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16852) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1ca9f5fab8908d79"></a>[`public struct Inno.Native.ImGui.ImGuiTableColumnSettingsPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTableColumnSettingsPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16831) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableColumnSortSpecs</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableColumnSortSpecs 261c1a1f5947dba1184ad9c6be638f462be0aa648e38073d10123154c5968ef3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSortSpecs.ColumnUserID 152bb5800cca29a0dcb730bcfd99f8526cc61dbe61231c392c93a03b1be3a6af -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSortSpecs.ColumnIndex 452ff7abd35a19e200a5759f37861b37bbe626d63d8519e8d5869a60554a6e5d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSortSpecs.SortOrder 0cbe8cba84ff0bdbfc8930b773a5b2cd1d83b04f73f23fbc77913331a36e51c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSortSpecs.SortDirection e8a0df8c88bc45d62f329547c0bc7fead7d58747996e86ecb32ad2713e91fe08 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSortSpecs.#ctor(System.UInt32,System.Int16,System.Int16,Inno.Native.ImGui.ImGuiSortDirection) 503786a1e2ddfb904dea069f37b3d23baa51037ba2f361287cf346425f73aab8 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSortSpecs.Destroy 45cdbb4b7be90c1c68dd78475140fe66f16df2ed55f8a219e6a795b5ee669254 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e85dcde0b3165a16"></a>[`public short Inno.Native.ImGui.ImGuiTableColumnSortSpecs.ColumnIndex`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6325) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4d3cff2400399922"></a>[`public uint Inno.Native.ImGui.ImGuiTableColumnSortSpecs.ColumnUserID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6324) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-085df0ec171ff685"></a>[`public Inno.Native.ImGui.ImGuiSortDirection Inno.Native.ImGui.ImGuiTableColumnSortSpecs.SortDirection`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6327) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c249022bfb40f8e0"></a>[`public short Inno.Native.ImGui.ImGuiTableColumnSortSpecs.SortOrder`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6326) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-949328fecedb5dd0"></a>[`public Inno.Native.ImGui.ImGuiTableColumnSortSpecs.ImGuiTableColumnSortSpecs(uint columnUserID = 0, short columnIndex = 0, short sortOrder = 0, Inno.Native.ImGui.ImGuiSortDirection sortDirection = Inno.Native.ImGui.ImGuiSortDirection.None)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6328) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d85d23e39b9f7568"></a>[`public void Inno.Native.ImGui.ImGuiTableColumnSortSpecs.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63208) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-36475549eac22403"></a>[`public struct Inno.Native.ImGui.ImGuiTableColumnSortSpecs`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6322) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr c609d1a2c16adee121255255db5aa0a8ef941a16c50db010e36c9e65bec9845b -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.#ctor(Inno.Native.ImGui.ImGuiTableColumnSortSpecs*) 70a6d5bcea7cbcea5f273b1cccaeb0c975e6c8e562da4a3ac7fa6a9dfe039dc6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.Handle affceab88db2cd35943fea77178c83613fad0d4e514aeac7188ebdc1f22af3a8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.IsNull e15e8d57774f3b8f7e4fce31121199016530f4a4ae74b0cf297428414a334cc6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.Null b4adc6e2e89b0a2798f987efd7aed28365d0b8c3b613a644776dc39d257a2429 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.Item(System.Int32) 064282fe394ca8d7ea133dede21f518aad67a3406c70f021b5a166bcdf0e0042 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableColumnSortSpecs*)~Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr 8a0e5c4a577adf9348d2660056cb70f1ad11739d5506dae59fa702cb41bb98f5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.op_Implicit(Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr)~Inno.Native.ImGui.ImGuiTableColumnSortSpecs* 11f55c2a31de9cf49b26f82ecde7dfa4d6594bf4a3479311e6cd32566a7bdacc -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.op_Equality(Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr,Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr) 07e4c464948c0594a8a2b6dd47a005e7748bd5a291ce7aee57dcf647626f66c3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.op_Inequality(Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr,Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr) 903ec83f6694154c98b86b91bfe406f4492d11a563ccfe183665f17570864013 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.Equals(Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr) f83b3aaa3a6b0af95136260f6a113d83e91cbd3daaf7d9e052fee54eb8dac680 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.Equals(System.Object) a0dcb6ea60fe56c11fbd92a0179818f0c30191b74f533ef13d32dd3daec10564 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.GetHashCode 7bd2c04aaa812ff1795236ab3b04a1179422e92332e57783597b2f8fb39a4a73 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.ColumnUserID 9260dc1d998e44cad66139a4c0e4f9d5ae1a8c1bbb2f4d28d4f1f16679c641f1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.ColumnIndex cec1cf47484322973c5b309d29c6c08b8b06103897220113cb6fa6da1c95178b -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.SortOrder 93a51a7c5a6e5193e6a0072f803b6a9227422820b6be8e2510fdc576ec21e672 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.SortDirection 132980e82946b52101fe9de1b9260e7e81d633fc33f986ff37d152ca9b1340bb -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.Destroy 2caa5f81aa99cc926e88c288ab0acf93801b451fc5ebccc1925c55a932e6716d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5e4c3331b2fab4a5"></a>[`public Inno.Native.ImGui.ImGuiTableColumnSortSpecs* Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16878) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c33203250384fa74"></a>[`public Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.ImGuiTableColumnSortSpecsPtr(Inno.Native.ImGui.ImGuiTableColumnSortSpecs* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16873) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a73da23a0d1205d2"></a>[`public void Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16900) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-353d98b27536fef9"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.Equals(Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16888) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-df3465a6ccd608fc"></a>[`public override bool Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16889) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-edcd2656a52f1d21"></a>[`public override int Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16890) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-e53d61366180c36e"></a>[`public static bool Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.operator ==(Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr left, Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16886) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1a0b1d94e6fa2c82"></a>[`public static Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.implicit operator Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr(Inno.Native.ImGui.ImGuiTableColumnSortSpecs* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16884) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b112df7cd4d405c4"></a>[`public static Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.implicit operator Inno.Native.ImGui.ImGuiTableColumnSortSpecs*(Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16885) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7f988100fcb7f522"></a>[`public static bool Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.operator !=(Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr left, Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16887) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fa4a6624832612b5"></a>[`public ref short Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.ColumnIndex { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16893) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c9536a5d24e0fee9"></a>[`public ref uint Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.ColumnUserID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16892) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3ea445933374dbe0"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16879) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-502468cb50129551"></a>[`public Inno.Native.ImGui.ImGuiTableColumnSortSpecs Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16882) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5c91f3c40ffedec7"></a>[`public static Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16880) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ed270f5d3358d0c2"></a>[`public ref Inno.Native.ImGui.ImGuiSortDirection Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.SortDirection { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16895) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-183041734787eeeb"></a>[`public ref short Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr.SortOrder { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16894) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e528272885d62c38"></a>[`public struct Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr : System.IEquatable<Inno.Native.ImGui.ImGuiTableColumnSortSpecsPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L16871) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableColumnsSettings</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableColumnsSettings 6a1538b36a3557d4e038ca75967a0ac83d48ac7374bb07243b764a9ed853028f -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnsSettings.#ctor(System.IntPtr) 824335c2491ff687116e92f2dcd545f69601acefd6d60aba18bc995dc7d95a77 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnsSettings.Handle 42dcdb93e391489cd407b42824fca0aa9e29809888f64d731a1dfd1d0614be05 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnsSettings.IsNull c1f33363f4ea74ff1a03b41c00b9de7cb08cd1d11c526e171dc86bed1662e2a0 -->
<!-- api: P:Inno.Native.ImGui.ImGuiTableColumnsSettings.Null b66ff5288d008d320963ba4f10f658ed00cafd85d65f67d9104e75e99918e39e -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnsSettings.op_Implicit(System.IntPtr)~Inno.Native.ImGui.ImGuiTableColumnsSettings 78f717b9c44ce34410b3417f502ea526b2dd80601a5081b8c22a1b0bc4af433f -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnsSettings.op_Equality(Inno.Native.ImGui.ImGuiTableColumnsSettings,Inno.Native.ImGui.ImGuiTableColumnsSettings) 55ca3dbb4aa12d273d6b81ea4a71a7ea48cea686d16bba140e511973b8275268 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnsSettings.op_Inequality(Inno.Native.ImGui.ImGuiTableColumnsSettings,Inno.Native.ImGui.ImGuiTableColumnsSettings) 30526c5d426ce5d1843e3db3ae09017dee128aba978fbf327c64028f23ac7980 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnsSettings.Equals(Inno.Native.ImGui.ImGuiTableColumnsSettings) 5c5aacbcf7befddffdc8fad0b5676c6d9c9d73f55b2acc9f35f779d8befb37a2 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnsSettings.Equals(System.Object) 43001e72c866a2610b2f641ac4e556cf586900f953e874c7b07c8ecba38e1c53 -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableColumnsSettings.GetHashCode cf2127b81b4fa86f0b1dee8a40aa6a2fa4fda8f3084fbc3024ad4175351ec24d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0cc91e63ee97c78b"></a>[`public Inno.Native.ImGui.ImGuiTableColumnsSettings.ImGuiTableColumnsSettings(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10290) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-c1687cd1b733819a"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnsSettings.Equals(Inno.Native.ImGui.ImGuiTableColumnsSettings other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10302) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-418c785cfef4221b"></a>[`public override bool Inno.Native.ImGui.ImGuiTableColumnsSettings.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10303) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-5a00aaae5cd5824a"></a>[`public override int Inno.Native.ImGui.ImGuiTableColumnsSettings.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10304) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-23aa83ae2e939d98"></a>[`public static bool Inno.Native.ImGui.ImGuiTableColumnsSettings.operator ==(Inno.Native.ImGui.ImGuiTableColumnsSettings left, Inno.Native.ImGui.ImGuiTableColumnsSettings right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10300) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f04d0584adf26bab"></a>[`public static Inno.Native.ImGui.ImGuiTableColumnsSettings.implicit operator Inno.Native.ImGui.ImGuiTableColumnsSettings(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10299) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-506da070411ed0a9"></a>[`public static bool Inno.Native.ImGui.ImGuiTableColumnsSettings.operator !=(Inno.Native.ImGui.ImGuiTableColumnsSettings left, Inno.Native.ImGui.ImGuiTableColumnsSettings right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10301) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5de6f9c3d8705e93"></a>[`public nint Inno.Native.ImGui.ImGuiTableColumnsSettings.Handle { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10295) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-310d1ce21759b153"></a>[`public bool Inno.Native.ImGui.ImGuiTableColumnsSettings.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10296) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1ae77b1015f69621"></a>[`public static Inno.Native.ImGui.ImGuiTableColumnsSettings Inno.Native.ImGui.ImGuiTableColumnsSettings.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10297) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ebf1e15bd301e205"></a>[`public readonly struct Inno.Native.ImGui.ImGuiTableColumnsSettings : System.IEquatable<Inno.Native.ImGui.ImGuiTableColumnsSettings>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10288) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableFlags</code> · 38 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableFlags a98cfb868c56e5e71eba8395c276d8866fb6715262682fa2ab6df2b79e090f10 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.None 7b8bd4df1c37f63c1a5c6d27cebd3c67c64ad8e4cbf62b75e6c2e9ade73e8c74 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.Resizable dd353a22eff3b77c1d19ea78c65e8750c0cf9251984e62939bb56b25cda7dd4f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.Reorderable 2ea0993f3e1fac04336b6adab896d01dce0c65c93b7286f729914f146dc36cf4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.Hideable 19f3f0b352813423ab15e494b82bbb4e4085224628b2a1ceb983d1ede9e549bc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.Sortable f2bf29817e56de8fe517e30f27441cd19b53e13da0e77da9ed377a4844392faf -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoSavedSettings e2904cec19423405b16892280cfedf496e38f0a7fe6f0b98730cb45d200ae947 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.ContextMenuInBody 3768ed4b3b02342c6c71e9e2ad2f6ee779b1083cf31928226decd5875dbc9b63 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.RowBg d35d23dbf33fcc140d067d2127c106c46974c957c77719e4504d972cbed8db29 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersInnerH 1c0e73864abfc2e85f1b2cdb9a15ea764a8a1ec377f663f7229052f71d4887c4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersOuterH 7f2b13a475c453a8ad65d184de4b29d70a5bd2ba1014e3e7b0b9e997884dadc4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersInnerV 5a6c6d22a4cb75079b101c7c3b8494943de8cea08c7a63d24dc3129fe3a4db2d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersOuterV cd535e6eda6b955d24a50aa4c713b715fd1b2106e6e9e239de4b25e48bb1968a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersH 172064a263535cd60251aad80578f440f55891751531086d7331b33132b2e867 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersV 0cd3f47820294e15671c7e04eb5b43545a7bb629a4309285ece5cbac37e9cf56 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersInner 0d4019036e10b835bbe8630209dd1d36cb72c06b62133c911efd07cd52010628 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.BordersOuter 0f6131e3f50b6afaa94dac6c76b535f632cd68e821532eccf881e6c3d7e17ffc -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.Borders f08ef0cb0e8256af986ea853776353f0511dcdcba7c6bfcc561bf85dc2396f5c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoBordersInBody c26e77bd0a57aee5c7269259c781c3cf995643dc6f4461ad81a17a349e7c22b4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoBordersInBodyUntilResize 68af7e0fa32b3f3b267bd9c8e027f04d43d3b64d7821c43054d3a818acd6d9fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SizingFixedFit 5df469577f4f5e438e8e652669be65c0247abee9ca675d9e4bd84a07b008d0aa -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SizingFixedSame 432a7a3a131fb4e4a1922be044c133bb8e525aa286bd8915b46b363d72300aea -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SizingStretchProp 5e04f10fed8b5ed602030eff37838f6db99c3a175880b97452ce888bd50341e5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SizingStretchSame 1507b2edcc85e3c0a63b74b1713a76a7bd1eb4c2c5e746e461dec6648a3b7501 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoHostExtendX f0062597c890c70cd10642c3c4de8b6bff7ed890435f6b040f49e9a7ea09c17e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoHostExtendY c5f7268810423b613e380433486eeba70f79a7bd1d174a4bfcc6392411c22eb7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoKeepColumnsVisible c7aaba62ca55fbdffc7ef9f0cdec02aed5454487891596bdef0ab060192ad78c -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.PreciseWidths fdd700c5b5670cfaeeebefc537396054d6936dc2c95dccf3f245f441cf825a8e -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoClip ae1869315656c4dd9211c64ec27c7183581b91692719761d85d2f78780b307b0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.PadOuterX be5daa224279763cf5093f3e014366e2820c054f063b2ebb0c75375455dc485a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoPadOuterX 80aff3649ebb9368ef5466e693515edc9c1891200e1b3825d689c2e8891e373f -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.NoPadInnerX 91e9b56ad64c21b3b88f6a145e8e0568534adfd648a3796625543027aee26e60 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.ScrollX 622ffc74daddda718d810725784787b196be427917e837516b26acbae0212a63 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.ScrollY 118c582da38e1072ee5fbdbbecc8c4f064bcfbc4b1e6bff2f50c4fb678c40b72 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SortMulti 827df129ad13c6866d7a27b24f46ad4776130041421c9bf647c1a90b29a967fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SortTristate ea5af6a4a3bbf27fa31539e85471809edcd0d65ddb25b25cdf0ecaa3b30a208a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.HighlightHoveredColumn 2c67873a977e342a09c70f552a2fa5e48a6540b312944e68deb3159cec876f48 -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableFlags.SizingMask 4ae7c0347a308b4b918215c452c0c6a602433c7d3d0ddc008a0da587166117b8 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f6b3d6087f47d1f4"></a>[`Inno.Native.ImGui.ImGuiTableFlags.Borders = 1920`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L999) | 当前常量或枚举值：`1920`。 | — |
| <a id="api-9c5132d1b29f4443"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersH = 384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L995) | 当前常量或枚举值：`384`。 | — |
| <a id="api-844a150a2499be33"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersInner = 640`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L997) | 当前常量或枚举值：`640`。 | — |
| <a id="api-a79e21fadfaeb37c"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersInnerH = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L991) | 当前常量或枚举值：`128`。 | — |
| <a id="api-f950d9f2bdf0965a"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersInnerV = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L993) | 当前常量或枚举值：`512`。 | — |
| <a id="api-79118132ff48700d"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersOuter = 1280`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L998) | 当前常量或枚举值：`1280`。 | — |
| <a id="api-e378c6c936712259"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersOuterH = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L992) | 当前常量或枚举值：`256`。 | — |
| <a id="api-6b61b2dc8fa27243"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersOuterV = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L994) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-ca71a8a71309ef10"></a>[`Inno.Native.ImGui.ImGuiTableFlags.BordersV = 1536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L996) | 当前常量或枚举值：`1536`。 | — |
| <a id="api-59e9bce3a562cda3"></a>[`Inno.Native.ImGui.ImGuiTableFlags.ContextMenuInBody = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L989) | 当前常量或枚举值：`32`。 | — |
| <a id="api-54d493e7d0d7bd1f"></a>[`Inno.Native.ImGui.ImGuiTableFlags.Hideable = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L986) | 当前常量或枚举值：`4`。 | — |
| <a id="api-e5648f466c9e5237"></a>[`Inno.Native.ImGui.ImGuiTableFlags.HighlightHoveredColumn = 268435456`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1018) | 当前常量或枚举值：`268435456`。 | — |
| <a id="api-b14792932639e9f3"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoBordersInBody = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1000) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-cf4f0293d4f40dbc"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoBordersInBodyUntilResize = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1001) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-96cf9fbdbd5110d8"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoClip = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1010) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-f356741da1017c27"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoHostExtendX = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1006) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-154b06c11643a7b3"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoHostExtendY = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1007) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-5a52457abe6542d0"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoKeepColumnsVisible = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1008) | 当前常量或枚举值：`262144`。 | — |
| <a id="api-78045e6e89a79067"></a>[`Inno.Native.ImGui.ImGuiTableFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L983) | 当前常量或枚举值：`0`。 | — |
| <a id="api-d1ce5f62f285c94e"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoPadInnerX = 8388608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1013) | 当前常量或枚举值：`8388608`。 | — |
| <a id="api-cf4dd114a4086a71"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoPadOuterX = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1012) | 当前常量或枚举值：`4194304`。 | — |
| <a id="api-d3dc1816f6a37977"></a>[`Inno.Native.ImGui.ImGuiTableFlags.NoSavedSettings = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L988) | 当前常量或枚举值：`16`。 | — |
| <a id="api-585c1f118b9795b2"></a>[`Inno.Native.ImGui.ImGuiTableFlags.PadOuterX = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1011) | 当前常量或枚举值：`2097152`。 | — |
| <a id="api-9a0fcd5c749bb465"></a>[`Inno.Native.ImGui.ImGuiTableFlags.PreciseWidths = 524288`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1009) | 当前常量或枚举值：`524288`。 | — |
| <a id="api-275ec52bcf0c9eb2"></a>[`Inno.Native.ImGui.ImGuiTableFlags.Reorderable = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L985) | 当前常量或枚举值：`2`。 | — |
| <a id="api-0cf1cdf2acbda102"></a>[`Inno.Native.ImGui.ImGuiTableFlags.Resizable = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L984) | 当前常量或枚举值：`1`。 | — |
| <a id="api-0887b5ab04c72ff3"></a>[`Inno.Native.ImGui.ImGuiTableFlags.RowBg = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L990) | 当前常量或枚举值：`64`。 | — |
| <a id="api-692a398acc464700"></a>[`Inno.Native.ImGui.ImGuiTableFlags.ScrollX = 16777216`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1014) | 当前常量或枚举值：`16777216`。 | — |
| <a id="api-ca99295fd91cf0ae"></a>[`Inno.Native.ImGui.ImGuiTableFlags.ScrollY = 33554432`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1015) | 当前常量或枚举值：`33554432`。 | — |
| <a id="api-435bc4294b942807"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SizingFixedFit = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1002) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-478ac1cf53c0c6b1"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SizingFixedSame = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1003) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-c33c46f037468cdd"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SizingMask = 57344`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1019) | 当前常量或枚举值：`57344`。 | — |
| <a id="api-6730dd9feecd8a06"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SizingStretchProp = 24576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1004) | 当前常量或枚举值：`24576`。 | — |
| <a id="api-6fa16952294db7ed"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SizingStretchSame = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1005) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-4a2b0a53ecaa4b92"></a>[`Inno.Native.ImGui.ImGuiTableFlags.Sortable = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L987) | 当前常量或枚举值：`8`。 | — |
| <a id="api-2b020aafac5e8a47"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SortMulti = 67108864`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1016) | 当前常量或枚举值：`67108864`。 | — |
| <a id="api-fcc32383389b862f"></a>[`Inno.Native.ImGui.ImGuiTableFlags.SortTristate = 134217728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1017) | 当前常量或枚举值：`134217728`。 | — |
| <a id="api-94347a51fa762fe7"></a>[`public enum Inno.Native.ImGui.ImGuiTableFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L981) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiTableHeaderData</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiTableHeaderData 70f33006ec7b1cc7a5474ed8d8f4eab2af1edf4e0ba0b4b59b58ad5de2ded5be -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableHeaderData.Index fe230c3f92c757b299cea821f0a6288011e7a9f80745fd4c1378a0102bd6487d -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableHeaderData.TextColor f9b74676e7684339e6a27793082322616a9612fc43189d4ba9717adb3ee342ff -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableHeaderData.BgColor0 9f0dddf645ee9abb71fb8c6d08431d60140ada236fb639c7c8f19fd71fac162a -->
<!-- api: F:Inno.Native.ImGui.ImGuiTableHeaderData.BgColor1 dc39fc70b33803a6a5fdf5a7c192cab5cf1e511d8a20c0dbc7987c220026ba6b -->
<!-- api: M:Inno.Native.ImGui.ImGuiTableHeaderData.#ctor(System.Int16,System.UInt32,System.UInt32,System.UInt32) e83e1c9997cf991d8921598a69f6197eaacd26b89fe3501122f7c5e6cbc0ca91 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-67d900d489fdbb43"></a>[`public uint Inno.Native.ImGui.ImGuiTableHeaderData.BgColor0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6046) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3c12a84ef3d7924b"></a>[`public uint Inno.Native.ImGui.ImGuiTableHeaderData.BgColor1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6047) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c69d4dd6d6e7136f"></a>[`public short Inno.Native.ImGui.ImGuiTableHeaderData.Index`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6044) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39c6f861499d5a98"></a>[`public uint Inno.Native.ImGui.ImGuiTableHeaderData.TextColor`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6045) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54a9cd33cced1d2e"></a>[`public Inno.Native.ImGui.ImGuiTableHeaderData.ImGuiTableHeaderData(short index = 0, uint textColor = 0, uint bgColor0 = 0, uint bgColor1 = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6048) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6f9f5e5232fa831c"></a>[`public struct Inno.Native.ImGui.ImGuiTableHeaderData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L6042) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
