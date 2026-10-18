# Inno.Native.ImGui ABI · ImGuiInputFlagsPrivate → ImGuiInputTextStatePtr

[项目与生命周期](../Inno.Native.ImGui.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputFlagsPrivate</code> · 25 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputFlagsPrivate a759fae85e8676d472bc66c02c72e9d43988a4037429bf90210a182e4c73c827 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateDefault 7ad2d4f024bf5ce5602a28c2404c9124a438269be9c9e6ae7aac2b6add17c9c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateNavMove 5f73dcbc2072165e36a6fb541199b88e72683d4e430f0827f91efb4abe9cb523 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateNavTweak 57f540508d185fb347c2fdc2afa5f1a55beaa92c50688f4aa58526645bf6e0bb -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilRelease dadfeb7d76b1a41872f5c84f95de7cd737ae367229313e6f2518b9567aff1689 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilKeyModsChange 2d1b2a2ecfc53b58a655335dcb7ae2d25426d11701ddab7eec0a86050990c4cb -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilKeyModsChangeFromNone 3aeef51ae1b991a05741049d5fc900701e9a936ec3ab97e0bdaf29bdd311947d -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilOtherKeyPress 12484234d827946793e8a48e93a3ae26d92359753a5e1600ce56bdfd70d6a522 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.LockThisFrame bb2f2eab324dec6610502d47a37b3f4de2c213587e16e6c3b248616cfbe25e24 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.LockUntilRelease 7299963ddee860f58fa4ff3d4e74cd3ad2fbbfec914a15915cd3c4627784f542 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondHovered d5d51ae360cf8a80ff3c4e307213d3e396ecd3411aa1187edefa9a5f5b7e90c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondActive 06c2aaa40c0ab0181800190a9d117cf78584cb480a0559e82111d9fa027b91f6 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondDefault 7c373ada6d99d435bdf656c09df679b3e84150463e62fb01cffe24d1c1b7e05d -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateMask e82f5b2195ab76728927525f0ac02d7f3e8cb56b39168c01068470b71c4c4e8b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilMask e42acab981a012324358350708513a32ed27893c94a42e041898eddc6d1c7ce3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatMask dc9938a0f7a06739fd038831ddb838c152858f83321426d8a21e1b31407d5198 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondMask 6723a1233fcf7259e469ab4d11ef81d8d159b8f994264aae63c01c31c2bf53dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RouteTypeMask 972e9e134b51248be8cb72d064ff948a1a21d2c4ffe8b2f9e0b3463cc4737990 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.RouteOptionsMask ff28bc14e1345b0fd4dbe3acaa56f288c871e283a2a80df9e912509a41d5443b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedByIsKeyPressed fa9ebfdc9f8532cb784009b354539053682998f6e843fe39832c704e0f8c1ab8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedByIsMouseClicked e2021b8b876373e1400ce3f730b7b607464c5c456862a8a3b361d9cd3f084e95 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedByShortcut caf50f2abd23eed8c914a1384ec11b05f8ec70caffc5018c6fa233c0f0504e77 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedBySetNextItemShortcut ba3d552c96b50b2b0ac093442bbf23b32cd1de866db0743a7504305ee1ec4e14 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedBySetKeyOwner dc56031ee6f8d91881e2250150b5f4675e5f210b27f72d3fa187d03eb70f763b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedBySetItemKeyOwner 0a9c45af7e1269dd0ba757d316264b5787d8c93346c19661ad27a64c979d647c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-146a64e4204f98e7"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondActive = 8388608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1494) | 当前常量或枚举值：`8388608`。 | — |
| <a id="api-31661f429ae898a5"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondDefault = 12582912`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1495) | 当前常量或枚举值：`12582912`。 | — |
| <a id="api-64d42f308bc246e2"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondHovered = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1493) | 当前常量或枚举值：`4194304`。 | — |
| <a id="api-81e76d27642f5993"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.CondMask = 12582912`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1499) | 当前常量或枚举值：`12582912`。 | — |
| <a id="api-f002fc5752e1ae28"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.LockThisFrame = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1491) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-1ef053660f8f79db"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.LockUntilRelease = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1492) | 当前常量或枚举值：`2097152`。 | — |
| <a id="api-a456a39a8c810ced"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatMask = 255`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1498) | 当前常量或枚举值：`255`。 | — |
| <a id="api-f13b0484f21f645f"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateDefault = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1484) | 当前常量或枚举值：`2`。 | — |
| <a id="api-84aeae692b0fd2cb"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateMask = 14`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1496) | 当前常量或枚举值：`14`。 | — |
| <a id="api-32f64fe59e8d6c08"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateNavMove = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1485) | 当前常量或枚举值：`4`。 | — |
| <a id="api-7b52357b75da66e7"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatRateNavTweak = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1486) | 当前常量或枚举值：`8`。 | — |
| <a id="api-fa475bb169cf230d"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilKeyModsChange = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1488) | 当前常量或枚举值：`32`。 | — |
| <a id="api-7ac505b4484f39d7"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilKeyModsChangeFromNone = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1489) | 当前常量或枚举值：`64`。 | — |
| <a id="api-7f92220556527bc9"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilMask = 240`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1497) | 当前常量或枚举值：`240`。 | — |
| <a id="api-4fdc892eb07e6ac5"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilOtherKeyPress = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1490) | 当前常量或枚举值：`128`。 | — |
| <a id="api-5b7dbc4285889dc4"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RepeatUntilRelease = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1487) | 当前常量或枚举值：`16`。 | — |
| <a id="api-5356231d0adf2cc4"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RouteOptionsMask = 245760`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1501) | 当前常量或枚举值：`245760`。 | — |
| <a id="api-1b6a12456a5c6191"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.RouteTypeMask = 15360`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1500) | 当前常量或枚举值：`15360`。 | — |
| <a id="api-20066c00619c5859"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedByIsKeyPressed = 255`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1502) | 当前常量或枚举值：`255`。 | — |
| <a id="api-75855ef117cf4a7b"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedByIsMouseClicked = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1503) | 当前常量或枚举值：`1`。 | — |
| <a id="api-75952a09dc7ed737"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedBySetItemKeyOwner = 15728640`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1507) | 当前常量或枚举值：`15728640`。 | — |
| <a id="api-844917e97c0dac94"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedBySetKeyOwner = 3145728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1506) | 当前常量或枚举值：`3145728`。 | — |
| <a id="api-c4d11bc015e6cba0"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedBySetNextItemShortcut = 523519`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1505) | 当前常量或枚举值：`523519`。 | — |
| <a id="api-a4fb999e5ad24088"></a>[`Inno.Native.ImGui.ImGuiInputFlagsPrivate.SupportedByShortcut = 261375`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1504) | 当前常量或枚举值：`261375`。 | — |
| <a id="api-ed29e8c4d1853b6e"></a>[`public enum Inno.Native.ImGui.ImGuiInputFlagsPrivate : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1482) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputSource</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputSource 05926bb3f7fbcf3c9ea9270991b9f03ae96e6fc7a6ad8b51364aadb9efc6852b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputSource.None 2437bf87fe39c936cffcafa5415e0e2c60b6e72aa67aa1cd3abaa39ea93c3fe3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputSource.Mouse 4be8bbc3c2b0fa7ccf7ed03e2de7d9630ee490c8cf0b638e4228117cee044e56 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputSource.Keyboard 344310fcb47497c4a9d26085cd68b6b694402a4816ad5ab4f60e4a3b8f99eea5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputSource.Gamepad 29c9fa0a2a5d206da2b652766fa6c1010018433182e2e115b42a066bc1f37b8a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputSource.Count 71ca3af0c02cc19394f73d77a4843d0d7012e32873b30e4e782426a2a97f0ab1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c5e065e19ba3cfad"></a>[`Inno.Native.ImGui.ImGuiInputSource.Count = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L114) | 当前常量或枚举值：`4`。 | — |
| <a id="api-6ba31005ac26d385"></a>[`Inno.Native.ImGui.ImGuiInputSource.Gamepad = 3`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L113) | 当前常量或枚举值：`3`。 | — |
| <a id="api-cad50b0e8c962e69"></a>[`Inno.Native.ImGui.ImGuiInputSource.Keyboard = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L112) | 当前常量或枚举值：`2`。 | — |
| <a id="api-676a3d166e3ab44f"></a>[`Inno.Native.ImGui.ImGuiInputSource.Mouse = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L111) | 当前常量或枚举值：`1`。 | — |
| <a id="api-4fe4be45e79f4328"></a>[`Inno.Native.ImGui.ImGuiInputSource.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L110) | 当前常量或枚举值：`0`。 | — |
| <a id="api-1353ad388e4daeb7"></a>[`public enum Inno.Native.ImGui.ImGuiInputSource : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L108) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputTextCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputTextCallback 9f7eee6629c19efa112c54956f3d502185e941d4c8e5771227d17348605aac26 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallback.#ctor(System.Object,System.IntPtr) 82556443bc98d2f3cc0921f36140da0ee7b83c8bc89e913bab95852b3edce223 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallback.Invoke(Inno.Native.ImGui.ImGuiInputTextCallbackData*) 6dda231177c46f5209b1eb6cb1524684d081cbd8a09f58897e80b315cafb55c1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallback.BeginInvoke(Inno.Native.ImGui.ImGuiInputTextCallbackData*,System.AsyncCallback,System.Object) 84fe949886c4fdd193566e9742dbaca72a7c34acc6d88b63429cd82c90fa0717 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallback.EndInvoke(System.IAsyncResult) a7ab8880b7147fad82d0546f78137fed0c57786e8bb8c98d2353fa0e45d3a469 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a845599e44e5ccde"></a>[`public Inno.Native.ImGui.ImGuiInputTextCallback.ImGuiInputTextCallback(object @object, nint method)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10663) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0a930b3dec5ac68e"></a>[`public virtual System.IAsyncResult Inno.Native.ImGui.ImGuiInputTextCallback.BeginInvoke(Inno.Native.ImGui.ImGuiInputTextCallbackData* data, System.AsyncCallback callback, object @object)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10663) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2abdfa894d39bfe4"></a>[`public virtual int Inno.Native.ImGui.ImGuiInputTextCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10663) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f31855382edf6972"></a>[`public virtual int Inno.Native.ImGui.ImGuiInputTextCallback.Invoke(Inno.Native.ImGui.ImGuiInputTextCallbackData* data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10663) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-92c04f049cf41763"></a>[`public delegate int Inno.Native.ImGui.ImGuiInputTextCallback(Inno.Native.ImGui.ImGuiInputTextCallbackData* data)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10663) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputTextCallbackData</code> · 24 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputTextCallbackData e7ad792e7e59fc05bc4f482a992a31ed65d5dbec9738ae330de7751c4affb729 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.Ctx 3ce7037ca0106c68ce7365fa87defae6ccaf21c32b7ab840a7c96a4a3e8ae5d4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.EventFlag d0f9e7c118ddd7c2e751fe3559b87c5bbc8cb8962dc347368a459aede13ecec3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.Flags 3d008679f0fe3ecada67224660cd8030cbadf7c5f7075a67d1ab29a8b1e9b65e -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.UserData 293316177cb871ff028fcfbba4ea6a788de82a1776613e8454e020d15f7494f8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.Id afcc03e16e1b4fa9d621337c940522dcc6b7f8fee88e67a675f533be30148b65 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.EventKey d78970fd8619ac743b25719f52a9a0ac43acb91fab68c766801d07c94caef6ae -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.EventChar bbcd5b29e595be67ab0a5733eb06f5de74943a597c7c8282977adb3f83d58fde -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.EventActivated 7d84fb39727b98c0db0012bfe89b4aac13d944ad0b41638be0d7be3724af8563 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.BufDirty 7dd4583ed7b94a14f71690f33bb92a1289206286fcab25ac9644f8927022df1b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.Buf d362c125af02485e4cecd2c451cb2f70a462c06ad85d82f5535523b4ac31f5b4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.BufTextLen 8cedb7f2fcd337d0a5d73dc72885982b80a76d83b5cb29d12836c4c4ef0a36cc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.BufSize b67ab7fbd5eec114e8159c8f6fd25f337423e84cf8140ce0810d5edce896a2c1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.CursorPos 40a9ffad725c5ee63769c17b70be14fe59dd7e9704402c5ab2df16394202e3c2 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.SelectionStart 9e273ab99b2b5770ff20929e8b1bbb348309664ae13a3705a9db4acd465bf7ce -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackData.SelectionEnd 594c28c1ac35eb08be80389e363aa1a64a454bf45f50bba243c7e306af08dcab -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackData.#ctor(Inno.Native.ImGui.ImGuiContext*,Inno.Native.ImGui.ImGuiInputTextFlags,Inno.Native.ImGui.ImGuiInputTextFlags,System.Void*,System.UInt32,Inno.Native.ImGui.ImGuiKey,System.UInt16,System.Byte,System.Byte,System.Byte*,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32) e033fe453318bdefbf6f9d679c97266071c7396d069f7b8a0c5d2c28f8d88141 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackData.Destroy be49cd169b76270341630a0c57822bd7ee892609825f8f6cc146980c5c251055 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackData.DeleteChars(System.Int32,System.Int32) 0601e8c353c0e840f2e14321c554a4e1b6ebc429cc01e2f131b6184024185b73 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackData.InsertChars(System.Int32,System.Byte*,System.Byte*) 488f3e41f18df5c412a8be937ee294f5891ad997e0f3bfb42d6ad185b675edf4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackData.SelectAll b3f9dd01ee8a2dddd4f4a1ddbb4ee1e7fc277a04d8f9507e666e10279090c47c -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackData.SetSelection(System.Int32,System.Int32) 6739a79391a988c4f046b44d283761bad810c079acb1172fbf20c72cc12e8491 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackData.ClearSelection d38738b8e6417163a840e18a3e960cf1bc52db708e201f373867a2f59c578491 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackData.HasSelection 4da6f0ca726cdfbe42ed52dc51e9f4675eac5ded78b57dc91a83330ca1eeaca0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5199ade47dabd476"></a>[`public byte* Inno.Native.ImGui.ImGuiInputTextCallbackData.Buf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9508) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b5b065523663484c"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextCallbackData.BufDirty`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9507) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-81029f8674dc084f"></a>[`public int Inno.Native.ImGui.ImGuiInputTextCallbackData.BufSize`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9510) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-520a672dc9c5ee5d"></a>[`public int Inno.Native.ImGui.ImGuiInputTextCallbackData.BufTextLen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9509) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-22196866a9fb958d"></a>[`public Inno.Native.ImGui.ImGuiContext* Inno.Native.ImGui.ImGuiInputTextCallbackData.Ctx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9499) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2def5190b9257987"></a>[`public int Inno.Native.ImGui.ImGuiInputTextCallbackData.CursorPos`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9511) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2c5701c08995132f"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextCallbackData.EventActivated`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9506) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7a3ed9e7c16ae135"></a>[`public ushort Inno.Native.ImGui.ImGuiInputTextCallbackData.EventChar`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9505) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2936eec12840863"></a>[`public Inno.Native.ImGui.ImGuiInputTextFlags Inno.Native.ImGui.ImGuiInputTextCallbackData.EventFlag`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9500) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a9db71fe5b85ae32"></a>[`public Inno.Native.ImGui.ImGuiKey Inno.Native.ImGui.ImGuiInputTextCallbackData.EventKey`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9504) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-22cd0c2105a24b9b"></a>[`public Inno.Native.ImGui.ImGuiInputTextFlags Inno.Native.ImGui.ImGuiInputTextCallbackData.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9501) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4c8967b49e23a1b5"></a>[`public uint Inno.Native.ImGui.ImGuiInputTextCallbackData.Id`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9503) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4762f6fb5a44da04"></a>[`public int Inno.Native.ImGui.ImGuiInputTextCallbackData.SelectionEnd`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9513) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8f1c6ab3169ad9e1"></a>[`public int Inno.Native.ImGui.ImGuiInputTextCallbackData.SelectionStart`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9512) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-967de5a05135b3ff"></a>[`public void* Inno.Native.ImGui.ImGuiInputTextCallbackData.UserData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9502) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d6117086ad214db8"></a>[`public Inno.Native.ImGui.ImGuiInputTextCallbackData.ImGuiInputTextCallbackData(Inno.Native.ImGui.ImGuiContext* ctx = null, Inno.Native.ImGui.ImGuiInputTextFlags eventFlag = Inno.Native.ImGui.ImGuiInputTextFlags.None, Inno.Native.ImGui.ImGuiInputTextFlags flags = Inno.Native.ImGui.ImGuiInputTextFlags.None, void* userData = null, uint iD = 0, Inno.Native.ImGui.ImGuiKey eventKey = Inno.Native.ImGui.ImGuiKey.ModNone, ushort eventChar = 0, byte eventActivated = 0, byte bufDirty = 0, byte* buf = null, int bufTextLen = 0, int bufSize = 0, int cursorPos = 0, int selectionStart = 0, int selectionEnd = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9514) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a47ab419058221bd"></a>[`public void Inno.Native.ImGui.ImGuiInputTextCallbackData.ClearSelection()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63492) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5522bf533d7eb702"></a>[`public void Inno.Native.ImGui.ImGuiInputTextCallbackData.DeleteChars(int pos, int bytesCount)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63448) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4d3d0a63d1865dc4"></a>[`public void Inno.Native.ImGui.ImGuiInputTextCallbackData.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63437) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-43c69ef5b20e32ae"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextCallbackData.HasSelection()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63503) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b04e97f00a9b702f"></a>[`public void Inno.Native.ImGui.ImGuiInputTextCallbackData.InsertChars(int pos, byte* text, byte* textEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63459) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b2b8898c1329307d"></a>[`public void Inno.Native.ImGui.ImGuiInputTextCallbackData.SelectAll()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63470) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b0aa7730f5c883f1"></a>[`public void Inno.Native.ImGui.ImGuiInputTextCallbackData.SetSelection(int s, int e)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L63481) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-51639d2c7ccc07da"></a>[`public struct Inno.Native.ImGui.ImGuiInputTextCallbackData`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L9497) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr</code> · 36 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr eb8631ab0b425aa14456fa84f0f5bfe6e243dad54e48d4bcfa9e4c32e9d4803d -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.#ctor(Inno.Native.ImGui.ImGuiInputTextCallbackData*) 07d77e45d2706e800698ec28484ab4fe1ebac6a1a2ed0d3cd1dd1fa7e05d918d -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Handle f6909fe5ee667f1f8a28a02aec9f52104f40a0901295cd47e8d2d6e574703abe -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.IsNull 1dca2caeb51c7a94079caf2ebfa01156980f38db130431d46358708e0571e718 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Null d40265dac35b4a6618284123545207fc62e3f89ebd69c6ccd2b5c96dca835eb4 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Item(System.Int32) 69a78df15a794246093d0f2ae9872ee4ec3401b4b4e2efd76f543a252979284e -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiInputTextCallbackData*)~Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr 3e13c5ec59c3ead4f62c1915859e5d25a7d8b71bbf2466798661f0bf186e7d31 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.op_Implicit(Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr)~Inno.Native.ImGui.ImGuiInputTextCallbackData* 62d22b36a7e46ca1ae7a60fa4afce5d4b04e143366f678f5fe714689575a7df3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.op_Equality(Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr,Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr) c0de52377dfe1482334c00286aa64f9dd6e3edb8f0814a19abe0d7de4d2bc063 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.op_Inequality(Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr,Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr) a29b6821c96d2189884d35df5ecbb1b6015fc410b3c52b0e61d166e556732c85 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Equals(Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr) 4cf6d17dee594f47b85afbc3563cacd7425c172a198a9af5adda575b0d2fa67f -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Equals(System.Object) 864d353c0d19eb1424762479604e90e2bdb079910d60113ed8554c4bbc74dbef -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.GetHashCode 99d02271c848e6240a5c893264e14b8fe78caa5703225e06438aa7ed8708aa7e -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Ctx fd2599fb7bab53d1ff58c81a0827d26a60d8b28adbb4d32b1914c2846306eece -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.EventFlag e3e46e6f5ca5531309f7cfedd16827b35de9e7e28fb3e3ba49b9609285604401 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Flags 3e4c5cc65e8c8c1d7aa0d8462d66066400a09c7cc89c61facd572a38ddbe0b80 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.UserData 0df080bfc089643c440e45927f5f794ffc791108b22767d7ffa5ad6d5c7fd8a3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Id 200594cbd209da2b631fdb58db87df710ddc7dae473369ee5e63e444d1feaa2f -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.EventKey d3d9e00a595d2301c9656955dbfcf839ebb2c9b808fdeb02847699a0561503c8 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.EventChar 7dfeff6c695773d5d02e46ce8fa1111d50b9ec11f8487db04fd57065dbde4ef9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.EventActivated d73cf0948648cd244ff877e50c12dc6112e4d6bf36833987adaea8de61502e65 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.BufDirty 9f35c50ce6724bcc5bdb83e3caaa9448b67eb834f4f83a6a580793806b8982fd -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Buf c57b1753e465ec5670c5d6469ddf357108973f49c6d8ae54fc8317c981f0c96b -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.BufTextLen f8350a57841abd4d504083d44e3254be2e095aafbdeb528482ea2bced7674b59 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.BufSize d8b933eae79daab26faca174fb885848dd0c0afd7807f1367fcf2d2e62d7b519 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.CursorPos ae637c0622164dfc6be893b78aa05e8418f49e9b96f7c7f6745c22e293184603 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.SelectionStart 9fd06a6b5641f8c237686d8e4c48e35dd24717709dd2d05de0b6e618b2b660a5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.SelectionEnd 7c8c34ba19706f461a25ebfef3a9b9d80231dbe80173baa3c82b05184d5b93a4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Destroy d3260c7a1c31be1347ae365e0079c82467e0ad45214bda87debf5f4fa8e0589a -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.DeleteChars(System.Int32,System.Int32) a0d76267043c2d25968eaf383be86ad6146368cc02215839b2f71638c3ca451c -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.InsertChars(System.Int32,System.String,System.String) 51caa8cefd271787e79bae4fd7042b71c4439d03ad1685e450d45a8ed5a550d6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.InsertChars(System.Int32,System.String) 34edd4a6384ff68b560fd89bec226aee56deac30a179707d826c1702979a1a92 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.SelectAll 919b7c9dbdbf37adb963f821b1c3016ce6f432a6f781e04227fc9493f8830cf0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.SetSelection(System.Int32,System.Int32) 00ec5f58fece0c0c833d00ce4e8047b68288aad4faab04f863b8a0c8d9907326 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.ClearSelection 70fbe4e71374052bf76a56c4ec4405416538e07de9389fb8c3fc5961c2ff4d03 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.HasSelection f96465be45734f711a73798e394391a266f9f2ade4368087d8fea30bea627c3d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b07c458beea9ef07"></a>[`public Inno.Native.ImGui.ImGuiInputTextCallbackData* Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14489) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-022e506ff04adc7d"></a>[`public Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.ImGuiInputTextCallbackDataPtr(Inno.Native.ImGui.ImGuiInputTextCallbackData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14484) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d3a329688f78a2b6"></a>[`public void Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.ClearSelection()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14570) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9ec1b9f67f708eeb"></a>[`public void Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.DeleteChars(int pos, int bytesCount)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14530) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fa9224de57f6be34"></a>[`public void Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14522) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-87b35b64d6f50e4c"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Equals(Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14499) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-b3d2a5464d507ea7"></a>[`public override bool Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14500) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-0d8076f142532675"></a>[`public override int Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14501) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-61496e3cde626f50"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.HasSelection()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14578) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-55704e9f26dd650f"></a>[`public void Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.InsertChars(int pos, string text, string? textEnd)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14538) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-62959618ad9e66cc"></a>[`public void Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.InsertChars(int pos, string text)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14546) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7762cefadbf2456a"></a>[`public static bool Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.operator ==(Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr left, Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14497) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c06229835d8bb145"></a>[`public static Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.implicit operator Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr(Inno.Native.ImGui.ImGuiInputTextCallbackData* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14495) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-244e61752e2fce25"></a>[`public static Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.implicit operator Inno.Native.ImGui.ImGuiInputTextCallbackData*(Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14496) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-45db5f6f89e858b8"></a>[`public static bool Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.operator !=(Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr left, Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14498) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-940034377ef5298e"></a>[`public void Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.SelectAll()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14554) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d8940279c7e79799"></a>[`public void Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.SetSelection(int s, int e)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14562) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6f73bfe62590b75f"></a>[`public byte* Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Buf { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14512) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0bcef2f836d070df"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.BufDirty { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14511) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ab58940be4096470"></a>[`public ref int Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.BufSize { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14514) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fd187af39badbcf2"></a>[`public ref int Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.BufTextLen { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14513) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ec0b40953f506193"></a>[`public ref Inno.Native.ImGui.ImGuiContextPtr Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Ctx { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14503) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dd6d6b15538e8b84"></a>[`public ref int Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.CursorPos { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14515) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2ec4bab7fcf2caf9"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.EventActivated { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14510) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-642896316dd65462"></a>[`public ref ushort Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.EventChar { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14509) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f8789cb8541df56b"></a>[`public ref Inno.Native.ImGui.ImGuiInputTextFlags Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.EventFlag { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14504) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-af4d81f0ac318a6e"></a>[`public ref Inno.Native.ImGui.ImGuiKey Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.EventKey { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14508) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c53efcd43a556353"></a>[`public ref Inno.Native.ImGui.ImGuiInputTextFlags Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14505) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0adbfae027ee37a5"></a>[`public ref uint Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Id { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14507) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-390bdd1a6f84faec"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14490) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-09807f93a65b44ea"></a>[`public Inno.Native.ImGui.ImGuiInputTextCallbackData Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14493) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d9b77aed082bd757"></a>[`public static Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14491) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bd080b794cea9754"></a>[`public ref int Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.SelectionEnd { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14517) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a00272475d7a2c29"></a>[`public ref int Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.SelectionStart { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14516) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4b771bca17ae0498"></a>[`public void* Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr.UserData { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14506) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-542420467b67bca2"></a>[`public struct Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr : System.IEquatable<Inno.Native.ImGui.ImGuiInputTextCallbackDataPtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14482) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputTextDeactivateData</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputTextDeactivateData 5cad9fbf4e28c8e1862ec88fb28938db3a81569ddff48c387e99b52ce3c635fc -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivateData.#ctor(System.IntPtr) 2d88ca14d844fe69fda35405f2b277778776a96005eaa18461a03561da4e2429 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextDeactivateData.Handle 1dff871d16a3ec8206f1b0e2c169cdfa5b621d363a9e3ab5e33684ce0964a18b -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextDeactivateData.IsNull d6fe28cbba2f9b3443670c1ac72d45ef3d45e3096453dce620f2990c89c3ac20 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextDeactivateData.Null 38e9f26d15479111f630b76af1f667ecdc3813d01afcf5c7aabe55e8468143f0 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivateData.op_Implicit(System.IntPtr)~Inno.Native.ImGui.ImGuiInputTextDeactivateData 958b750162086f2bb56693dbe1e81b842b7d135d2245993c2391d72e4af4c069 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivateData.op_Equality(Inno.Native.ImGui.ImGuiInputTextDeactivateData,Inno.Native.ImGui.ImGuiInputTextDeactivateData) b12eab3552e4dc8825cfbe7bd3006b16b7905a88aecb80cb7fbe3ae198d97399 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivateData.op_Inequality(Inno.Native.ImGui.ImGuiInputTextDeactivateData,Inno.Native.ImGui.ImGuiInputTextDeactivateData) 3990d342b06419d46d9f9b6a7d2a74ff51a551fc600f946a412c0b372bdaee30 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivateData.Equals(Inno.Native.ImGui.ImGuiInputTextDeactivateData) 2d8487cf4e5e4e4d0ea8b75be98bff08c19d0b00d2b49dcf975bf08e208d77a5 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivateData.Equals(System.Object) 47310f9bae10ed43599035b213c9fba984c2a8c0a3497ae340da68b01e223756 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivateData.GetHashCode 898d2102b5a6d05ccfac192f7b6e1127f2d9e7dcabf0cc27cd2e603ef63739ee -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-df6c4c25c4b43756"></a>[`public Inno.Native.ImGui.ImGuiInputTextDeactivateData.ImGuiInputTextDeactivateData(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10202) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-23525cc6587f38b2"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextDeactivateData.Equals(Inno.Native.ImGui.ImGuiInputTextDeactivateData other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10214) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-3ca8028e8e84baf7"></a>[`public override bool Inno.Native.ImGui.ImGuiInputTextDeactivateData.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10215) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-07fd6575d4393f0c"></a>[`public override int Inno.Native.ImGui.ImGuiInputTextDeactivateData.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10216) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-825946f87bef01e8"></a>[`public static bool Inno.Native.ImGui.ImGuiInputTextDeactivateData.operator ==(Inno.Native.ImGui.ImGuiInputTextDeactivateData left, Inno.Native.ImGui.ImGuiInputTextDeactivateData right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10212) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b25952273b13b2ec"></a>[`public static Inno.Native.ImGui.ImGuiInputTextDeactivateData.implicit operator Inno.Native.ImGui.ImGuiInputTextDeactivateData(nint handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10211) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-96aaa706530c4080"></a>[`public static bool Inno.Native.ImGui.ImGuiInputTextDeactivateData.operator !=(Inno.Native.ImGui.ImGuiInputTextDeactivateData left, Inno.Native.ImGui.ImGuiInputTextDeactivateData right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10213) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a78a282d4c752a41"></a>[`public nint Inno.Native.ImGui.ImGuiInputTextDeactivateData.Handle { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10207) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-69545880ce2d2d29"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextDeactivateData.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10208) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3f6a6d552ab6944a"></a>[`public static Inno.Native.ImGui.ImGuiInputTextDeactivateData Inno.Native.ImGui.ImGuiInputTextDeactivateData.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10209) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-592a1b44c1e5611a"></a>[`public readonly struct Inno.Native.ImGui.ImGuiInputTextDeactivateData : System.IEquatable<Inno.Native.ImGui.ImGuiInputTextDeactivateData>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L10200) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputTextDeactivatedState</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputTextDeactivatedState 722ed3cca1017f22ebee0f3f866a532648ee46182f79ee120248e6259f5e9ae3 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextDeactivatedState.ID d235deafb51c231ab0f8c5399dcff4567ddb6dd5ecd9305cf4d11ee0be2ba146 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextDeactivatedState.TextA 8e699ee4e74017e56aae2030bec82ee0fc32cea32c6f0f7f92666919fefe818c -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivatedState.#ctor(System.UInt32,Inno.Native.ImGui.ImVector{System.Byte}) 46cd73a8a26339f7377cd72ca2bb541fe9ba7d2b41ff9f7e23acee5a8877d74b -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivatedState.Destroy 665732cff2458c72332fced462a10f2dbde0a167f8369648e39bd1b7f409bcd3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivatedState.ClearFreeMemory b3b68d9a2deacacaddf3da9620d45922a5bbc2700085f3cec4e742a6708963d1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ccb9488687586598"></a>[`public uint Inno.Native.ImGui.ImGuiInputTextDeactivatedState.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7083) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5adee9adfbfa07d1"></a>[`public Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImGuiInputTextDeactivatedState.TextA`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7084) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f8fc00845a09d5a6"></a>[`public Inno.Native.ImGui.ImGuiInputTextDeactivatedState.ImGuiInputTextDeactivatedState(uint iD = 0, Inno.Native.ImGui.ImVector<byte> textA = default(Inno.Native.ImGui.ImVector<byte>))`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7085) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-739e33de12c36aee"></a>[`public void Inno.Native.ImGui.ImGuiInputTextDeactivatedState.ClearFreeMemory()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66516) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ebcea7c893b66a03"></a>[`public void Inno.Native.ImGui.ImGuiInputTextDeactivatedState.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66505) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dc90f10976161d64"></a>[`public struct Inno.Native.ImGui.ImGuiInputTextDeactivatedState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7081) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr 1075f3d3af4e7c24602eb5d6e9ceb8ed876d877c3c41fe21b78df13121e275b3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.#ctor(Inno.Native.ImGui.ImGuiInputTextDeactivatedState*) 4ad2fb2e3a244be636ee66cc445d5cef18cd16eeca05f2ca5aec05ea32482b3c -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.Handle da1a83c87e4e776b0c1570fc534bd8068cfba2ccbd98237f31cd70d6b93347ef -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.IsNull dd157791cf3807ad2d809f7ad9a1ade891a4f0c3d80841fa0376490c37c3d22b -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.Null 163e58404ab69a7c2ce71fdc9dce423a706fe80b0fb0b5be159fe3bca241f0ee -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.Item(System.Int32) fcc23c536fc83b885a26f13556554c80954c31bdf86bc103cbebc988ecae50b9 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.op_Implicit(Inno.Native.ImGui.ImGuiInputTextDeactivatedState*)~Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr a4533ad273700aed515e0f65366f4d544f6fc2381fea41b43f19edfcd5a0f1c4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.op_Implicit(Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr)~Inno.Native.ImGui.ImGuiInputTextDeactivatedState* a9294a9c92f0d86a0eb6b7d30669d2566ef66229847d0337377be0ac0278e964 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.op_Equality(Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr,Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr) 96c9f083acb2e762306322aa58be86c89644e51d08e0279e6a379c72917c65c6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.op_Inequality(Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr,Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr) 6038668552fb7414bb142b714c9d3ee7ce20dff0d3e7b43a00b000d30cec55a7 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.Equals(Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr) 400e90647e4108f6b55f9e9a0199d83b63aa85e23c5a9871926e13324c31fd94 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.Equals(System.Object) 3a8c62dbc23f4b2e6fbd32e14a538cea511d274d1f2e81e580c3b37e3e4aef06 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.GetHashCode dc625ce27371cbe4c0aeab9f8bd614d41a1b70ee05a88eb6297a23bd2f6b3994 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.ID 4359b57e9f85943065408842e50b0db9435dfa01d94f2a430ca9ee3a0d00fad9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.TextA db08d490de710acab8fc28b558909a9ca4ed3a8b9abc026718358358a0ae6805 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.Destroy 6d7acc7efdf8f3e5de1738d6e7e1aae0f9aa0a479b10ae42e38abc80f1cafa9f -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.ClearFreeMemory 08bfc08c1250cd6922b76e5032008486135234d7270bd56fc951a6f934434f67 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3fe6ec6959f72600"></a>[`public Inno.Native.ImGui.ImGuiInputTextDeactivatedState* Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14592) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-867676adbae3a5f4"></a>[`public Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.ImGuiInputTextDeactivatedStatePtr(Inno.Native.ImGui.ImGuiInputTextDeactivatedState* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14587) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-eec417501e7f9a39"></a>[`public void Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.ClearFreeMemory()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14620) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3f62f35235e6db13"></a>[`public void Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14612) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a46237f6b040ba7f"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.Equals(Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14602) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-dd7d52cd6fdcc8ea"></a>[`public override bool Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14603) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-f8321953cf31c138"></a>[`public override int Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14604) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-b566b6a94a96e343"></a>[`public static bool Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.operator ==(Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr left, Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14600) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-58cb239aaac81137"></a>[`public static Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.implicit operator Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr(Inno.Native.ImGui.ImGuiInputTextDeactivatedState* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14598) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eb6b1ad308e8a980"></a>[`public static Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.implicit operator Inno.Native.ImGui.ImGuiInputTextDeactivatedState*(Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14599) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a8150726d2c69218"></a>[`public static bool Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.operator !=(Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr left, Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14601) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-06ddda87ebb282ef"></a>[`public ref uint Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14606) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c3d47dcc5fc1f616"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14593) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5f26f0c4b392574a"></a>[`public Inno.Native.ImGui.ImGuiInputTextDeactivatedState Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14596) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-209c81e162731593"></a>[`public static Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14594) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fddea895eba14b9e"></a>[`public ref Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr.TextA { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14607) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5442b3f90f0bdf8c"></a>[`public struct Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr : System.IEquatable<Inno.Native.ImGui.ImGuiInputTextDeactivatedStatePtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14585) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputTextFlags</code> · 27 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputTextFlags 536768b85bd2c3be7bf72cc3ad3c87c52654493c294a0c96552c24763297cdf9 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.None 618cbe1bce830e5d3a3179b9a0b1a519aa796c856e6a7312c9fc6ca5c89f5309 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CharsDecimal 798b6963a5f1e28cc57b425ca48d74c0449523736200d7b811b22c2ea7f6a974 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CharsHexadecimal ac946a86a41128358b8631d909d0d6091423ef170da45c8edaf579e2418cb648 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CharsScientific e2207a27131afec39f2c870c18967c838d5288f93f968c3f2ce4e4e263fea2b8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CharsUppercase 031b05334c4b7a9f31b106d28b088f23a1d6cd5c62b6c8b5a98f4f96e759c02e -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CharsNoBlank b4b2180fb0f246f2c5eefc223c0cfd28cf965fa121f015907f418e28e968869e -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.AllowTabInput 9fef9e0aded44b517435b278cca59f62c8c3b1296495232c7c3ac4f043c48b09 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.EnterReturnsTrue 8acebc393c89e1eef9717e3da09a0bcb59acd810d97c787e0d253bb56f83aba1 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.EscapeClearsAll 9a26935275c21120215a7b8bff5d168f84f882a70f60e3663a3939c0d59f0e57 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CtrlEnterForNewLine cf3697796c20a3c68627a4cba19f2d8b9cf7139031d0bdc64b93f7f8b42470ac -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.ReadOnly 449365763f014160473986958bbb52cab88a70b43b70e7626d981c6ad131e0a8 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.Password dffd546fae3cbcda955dcc5320293f6c33126896f1643e22616acadb6a3a778a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.AlwaysOverwrite cb903cc8fe2d9c293c2aac5027a8bec05d76536d88231d4301deb2987b67692f -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.AutoSelectAll 889e6440b182e30955d60ec2d5360caca65309da6816900403bcaafaf3fc4979 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.ParseEmptyRefVal dcbd061432f9895791661d0e1e0129af4c4e2dad2b7555c6e8e1d10a927623a4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.DisplayEmptyRefVal 589eb61751bba330bb440f62a6b3833476f773391f9cc7b7401fb42eba80f023 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.NoHorizontalScroll bfe66162a036f5324dae21a16bb71b4cb512eea977785c55ece592e081b66cab -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.NoUndoRedo 9b6d2320e7daf5477de943a2e520c0b568a42082e6c5b99c8c73fa8d11ec7711 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.ElideLeft 079304c36132d7c6b3254df83d20f87bd597858451e600b7820c44f8f14b6b46 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CallbackCompletion 86cc14614cafea01054ec6f647bba74ca27379d999a54232b0ed82d63354b20a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CallbackHistory 749cd2186cd98e41ebe2b6407dae1cde158322503629c19be92b04f82d36dd2f -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CallbackAlways 2d92c8b8336390ea3c7b768f16b8d7d67b3604286eb872e02b5765ca7dfb8138 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CallbackCharFilter dbf47a7ae6ceab6d19e12ad4278d0dcec3c8e8162bd1b85d8e728a24b69ef5e4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CallbackResize d0b3a3bf04e4832f61f170eab8e79b5b42a839bc907d62719877d9ef8456a5ba -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.CallbackEdit 1e62e2fb8d5825bcadc07597b3c67cbf400a97fcf64edbb274f10917d8dccd8a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlags.WordWrap 94770230d2f3cb407326760309d8661d1b1493721a875450acfac4dd22c0d75d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2d0d5279b2962bed"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.AllowTabInput = 32`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L456) | 当前常量或枚举值：`32`。 | — |
| <a id="api-84bae4c474c690ae"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.AlwaysOverwrite = 2048`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L462) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-d74da270609d1dff"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.AutoSelectAll = 4096`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L463) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-f07a7fb97151529e"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CallbackAlways = 1048576`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L471) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-d95d6db30c2db7b0"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CallbackCharFilter = 2097152`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L472) | 当前常量或枚举值：`2097152`。 | — |
| <a id="api-e7fe67572e7822d5"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CallbackCompletion = 262144`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L469) | 当前常量或枚举值：`262144`。 | — |
| <a id="api-986c72ecab998a19"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CallbackEdit = 8388608`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L474) | 当前常量或枚举值：`8388608`。 | — |
| <a id="api-3fe064e026de0ebe"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CallbackHistory = 524288`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L470) | 当前常量或枚举值：`524288`。 | — |
| <a id="api-0172b7feccb629fd"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CallbackResize = 4194304`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L473) | 当前常量或枚举值：`4194304`。 | — |
| <a id="api-09d8961b0c572784"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CharsDecimal = 1`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L451) | 当前常量或枚举值：`1`。 | — |
| <a id="api-f2d4f5ed9e86b97e"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CharsHexadecimal = 2`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L452) | 当前常量或枚举值：`2`。 | — |
| <a id="api-310e5f13a0183724"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CharsNoBlank = 16`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L455) | 当前常量或枚举值：`16`。 | — |
| <a id="api-a05dc9a3277ca2a5"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CharsScientific = 4`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L453) | 当前常量或枚举值：`4`。 | — |
| <a id="api-bbaf35c301fedfc3"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CharsUppercase = 8`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L454) | 当前常量或枚举值：`8`。 | — |
| <a id="api-87b737161ab68ce9"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.CtrlEnterForNewLine = 256`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L459) | 当前常量或枚举值：`256`。 | — |
| <a id="api-e3ba3b41cbf08065"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.DisplayEmptyRefVal = 16384`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L465) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-423b914fdd45d540"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.ElideLeft = 131072`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L468) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-71cf6018391460d2"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.EnterReturnsTrue = 64`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L457) | 当前常量或枚举值：`64`。 | — |
| <a id="api-d47f159fc6d1a485"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.EscapeClearsAll = 128`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L458) | 当前常量或枚举值：`128`。 | — |
| <a id="api-763f6fcc2544b0ca"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.NoHorizontalScroll = 32768`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L466) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-11fac55123ca452a"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.None = 0`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L450) | 当前常量或枚举值：`0`。 | — |
| <a id="api-3e5d5f93dd2818c9"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.NoUndoRedo = 65536`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L467) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-befa4078f096ae1b"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.ParseEmptyRefVal = 8192`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L464) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-be94303d04adab1a"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.Password = 1024`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L461) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-f5c062ca03485ea5"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.ReadOnly = 512`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L460) | 当前常量或枚举值：`512`。 | — |
| <a id="api-a57196c33bf25e2e"></a>[`Inno.Native.ImGui.ImGuiInputTextFlags.WordWrap = 16777216`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L475) | 当前常量或枚举值：`16777216`。 | — |
| <a id="api-50c9fae03a628b3b"></a>[`public enum Inno.Native.ImGui.ImGuiInputTextFlags : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L448) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputTextFlagsPrivate</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputTextFlagsPrivate cbcbd1c135f2ee0a1bc347f84ded0e0941d68b6aca292f0544f33a5be7a1003b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlagsPrivate.Multiline eda54ac23da738c14f0ce7578e71e61b19d95367d80772c7c4e87bf0be215f5b -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlagsPrivate.MergedItem 0200a627b273cc00b8f3ac587bb298da95c6d5bed349828a0a23e5a7bf53868c -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextFlagsPrivate.LocalizeDecimalPoint 00ae5b44da4de8fbfb671c49f5b7b6831714783fd8375aa123ae6f11d4b2faa0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f5601ac60a81dbc7"></a>[`Inno.Native.ImGui.ImGuiInputTextFlagsPrivate.LocalizeDecimalPoint = 268435456`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1272) | 当前常量或枚举值：`268435456`。 | — |
| <a id="api-14150ef76488cb68"></a>[`Inno.Native.ImGui.ImGuiInputTextFlagsPrivate.MergedItem = 134217728`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1271) | 当前常量或枚举值：`134217728`。 | — |
| <a id="api-3246f263b32f5118"></a>[`Inno.Native.ImGui.ImGuiInputTextFlagsPrivate.Multiline = 67108864`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1270) | 当前常量或枚举值：`67108864`。 | — |
| <a id="api-594a55cce5a14a14"></a>[`public enum Inno.Native.ImGui.ImGuiInputTextFlagsPrivate : int`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L1268) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputTextState</code> · 42 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputTextState 42f1984eb23843679400213e47346db4d2c9b25610fdf7efec259fe21ecb72bf -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.Ctx 807c946217f7a614d938b8359039fed8e160bbdf45ffd86c40bf12ccbd6fd140 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.Stb e96917b0e6658dde1083c519dee78622620cb831f932d79c264e013345bd0e6a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.Flags c90f7583327c96e785abf9713028eeede2c5ec068cf7a647ffdbcdd67da6968d -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.ID 0e7527150d279777c93d18bdadcdfeabfd5759880ac96566ada272f72f8895ee -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.TextLen a24db05f880e63e956d660294baaa3d01533b0d4c0db9b5f411d6676aa327e37 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.TextSrc 7b9fa885884bbbf4d9f241093a22e500cd4088b55f3222d10c2c01687bfd3a2d -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.TextA 4370d24996dbd9f494c8252ee2746402678d1e517b84e67cc069df07b5338f31 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.TextToRevertTo e6eb125550be97a2227b63af9563eab58102e35ec76934e8fc1164f020e2b5fe -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.CallbackTextBackup a3e2243c74ccb8a18996059a8677c803f7e22f269180aefa8dd3f32835d2e717 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.BufCapacity 1b11d96dc4665c3f5b4f919e9caf7e7a25446eedc6af4d72c6dbfdd351a6ff87 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.Scroll 532c3e808dfb26e03be7f82d275c62ca7ac2b3167e5863012ba6934a97bf8def -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.LineCount 31b0d68c24ba3d918515f43cb4a6dce23753334441086696d4ebaca8a7dc535c -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.WrapWidth da0e32f9149449c1b67d033336bc1b6d2865d26fe8aed8b161dfab4455d5fcc4 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.CursorAnim 0d3322710dff28330eb6420b651887f8443656cae1865d54a91e9f49eab695c5 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.CursorFollow 7fe801325961be8ed5617e1ad283ba4c5a8a608554a2e7403cc6c0b7d7894aba -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.CursorCenterY 88cddca66251bb5514a75e9ebfd913248500b61dd234d24fc1bab72411e6e85a -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.SelectedAllMouseLock b569ed9c899a4c5ac17b4e8ed50a6c30e22ac0b46529bddd1893b9b65916e152 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.Edited 60be2cab6987ade82b0e9a335e44c38e528bceb818f4c395b0f99135dc35d722 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.WantReloadUserBuf 4db46609c314bfc9529b386bbb7f3605bb4fdaee0eba5432dd543defce715bb0 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.LastMoveDirectionLR 5a3ad8e772cb8defa45cd756fda1daa226fd11abc4cc3111d723899049d7f550 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.ReloadSelectionStart ae1c6b449f35a20f0216aef9656a69fb4002f5e09c5f84f86b88201d3fb052a7 -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextState.ReloadSelectionEnd ed1d25967ab4e09443b1ce05969c8a7a151771ff5c6552b068d21f02e3f5628f -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.#ctor(Inno.Native.ImGui.ImGuiContext*,Inno.Native.ImGui.STBTexteditState,Inno.Native.ImGui.ImGuiInputTextFlags,System.UInt32,System.Int32,System.Byte*,Inno.Native.ImGui.ImVector{System.Byte},Inno.Native.ImGui.ImVector{System.Byte},Inno.Native.ImGui.ImVector{System.Byte},System.Int32,System.Numerics.Vector2,System.Int32,System.Single,System.Single,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Int32,System.Int32) e2d3f7424843bae984a21825ee6e8731957d738c3bd1dcd3ff894ad089ff7f72 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.Destroy 19cd1693817f610f41c3f47c3eb542c87c63f23fbd50638a68eb1a080a901671 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.ClearText f2477febb8f4e721661d1db5d76752240ab134c0953710cbb49ca9d6aadd0381 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.ClearFreeMemory a5f3ad78d862e6c8c38e517311a30ef58b84899316a5849c50d8901415364808 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.OnKeyPressed(System.Int32) 1fc6136a097956d37aaf0db90a0cb14f0eea6d2336468cfc051f662b8923d493 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.OnCharPressed(System.UInt32) b34564f355810d606b9798f555a2f5154d4ac56abebf71956617168d4853ab25 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.GetPreferredOffsetX 765fd10c5560befc86c6e2ec32cdc08335bd108f6f655cc02d9b122f8107a5ef -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.CursorAnimReset e3f978f4912947582a3fe75cb6e41d281b53a43b397b40afaf52b15753a5f7fd -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.CursorClamp da7e8222c02264bc4e03122b9e09618bc602b41d01f4304e5aaada2b6d159574 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.HasSelection 289cefcad992dd0d2e25b5a57d6774e8704aa1a7c6e7e8c441a5475945d96c90 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.ClearSelection bb2700afa818e7cdd4b3e81ee08018ace475b84553db53277b75d32ce98632c1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.GetCursorPos 10401d42e66c2d0576b19b979852bd1b6dc4c6f3a8b703d4ec9dd021e59b5819 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.GetSelectionStart 736299f669dbfd6b71f67b40894ef60d10c4efa932a9d10a30b713a411524846 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.GetSelectionEnd 0ac7fd66212fe773f2eda21a2d8d90ddb4abadc0dea4ea3737326fabe932df9d -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.SetSelection(System.Int32,System.Int32) 4a338370e2823e9caf7ec668297ae90e23865152ccd310620d942a1becbd3418 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.SelectAll ca89658022632529d16fb1c841821fddefddbafaf5e5b95a2ddff728895c79ff -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.ReloadUserBufAndSelectAll b339ba0670f4ce69030e819768ced9456f79e0af82729883f37a8650cf5b41fd -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.ReloadUserBufAndKeepSelection 71fa477a052a8feed2c8277e447fecd2dc0cb1b094272f755091a95ddffbb5ba -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextState.ReloadUserBufAndMoveToEnd 15dbcc6be3f36286dff04f42826ce0b7422b6f48a79dce936e2b4dc0b4c8a5fb -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6b16806af3af357e"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.BufCapacity`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7022) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-92acd58866af8f37"></a>[`public Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImGuiInputTextState.CallbackTextBackup`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7021) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f9beb5db65f7497a"></a>[`public Inno.Native.ImGui.ImGuiContext* Inno.Native.ImGui.ImGuiInputTextState.Ctx`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7013) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a9b43d415bf5155a"></a>[`public float Inno.Native.ImGui.ImGuiInputTextState.CursorAnim`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7026) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-76256e4b345e1850"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextState.CursorCenterY`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7028) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b34abc175270c73"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextState.CursorFollow`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7027) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ba72441b697548e2"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextState.Edited`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7030) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-aa44a53ee7824747"></a>[`public Inno.Native.ImGui.ImGuiInputTextFlags Inno.Native.ImGui.ImGuiInputTextState.Flags`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7015) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ecafe0127d489de2"></a>[`public uint Inno.Native.ImGui.ImGuiInputTextState.ID`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7016) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bf726fb631efff59"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextState.LastMoveDirectionLR`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7032) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-193a2cd8184b798f"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.LineCount`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7024) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-302eddccff819aa0"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.ReloadSelectionEnd`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7034) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc83a2be39e0c88b"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.ReloadSelectionStart`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7033) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b80957c4a18f3f5"></a>[`public System.Numerics.Vector2 Inno.Native.ImGui.ImGuiInputTextState.Scroll`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7023) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3f4a91c2858cd636"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextState.SelectedAllMouseLock`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7029) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-42227ebc9b7372c9"></a>[`public Inno.Native.ImGui.STBTexteditState Inno.Native.ImGui.ImGuiInputTextState.Stb`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7014) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-55cece1f443c7f0b"></a>[`public Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImGuiInputTextState.TextA`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7019) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-273c57ebeaa61659"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.TextLen`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7017) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7b151c84a5ca37ac"></a>[`public byte* Inno.Native.ImGui.ImGuiInputTextState.TextSrc`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7018) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2431b54e173248ea"></a>[`public Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImGuiInputTextState.TextToRevertTo`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7020) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5bc66a7a04be61b0"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextState.WantReloadUserBuf`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7031) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e9d3b40f31ae7229"></a>[`public float Inno.Native.ImGui.ImGuiInputTextState.WrapWidth`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7025) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1594eddfa1ba8b5"></a>[`public Inno.Native.ImGui.ImGuiInputTextState.ImGuiInputTextState(Inno.Native.ImGui.ImGuiContext* ctx = null, Inno.Native.ImGui.STBTexteditState stb = default(Inno.Native.ImGui.STBTexteditState), Inno.Native.ImGui.ImGuiInputTextFlags flags = Inno.Native.ImGui.ImGuiInputTextFlags.None, uint iD = 0, int textLen = 0, byte* textSrc = null, Inno.Native.ImGui.ImVector<byte> textA = default(Inno.Native.ImGui.ImVector<byte>), Inno.Native.ImGui.ImVector<byte> textToRevertTo = default(Inno.Native.ImGui.ImVector<byte>), Inno.Native.ImGui.ImVector<byte> callbackTextBackup = default(Inno.Native.ImGui.ImVector<byte>), int bufCapacity = 0, System.Numerics.Vector2 scroll = default(System.Numerics.Vector2), int lineCount = 0, float wrapWidth = 0, float cursorAnim = 0, byte cursorFollow = 0, byte cursorCenterY = 0, byte selectedAllMouseLock = 0, byte edited = 0, byte wantReloadUserBuf = 0, byte lastMoveDirectionLR = 0, int reloadSelectionStart = 0, int reloadSelectionEnd = 0)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7035) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-72dc1cba16029501"></a>[`public void Inno.Native.ImGui.ImGuiInputTextState.ClearFreeMemory()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66552) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5600fdbb7a522a33"></a>[`public void Inno.Native.ImGui.ImGuiInputTextState.ClearSelection()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66629) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2c5bbc34ff5312c3"></a>[`public void Inno.Native.ImGui.ImGuiInputTextState.ClearText()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66541) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-00912b1eacc6f5d9"></a>[`public void Inno.Native.ImGui.ImGuiInputTextState.CursorAnimReset()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66596) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-690cd1eafee96f28"></a>[`public void Inno.Native.ImGui.ImGuiInputTextState.CursorClamp()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66607) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-97f699d8eda56fee"></a>[`public void Inno.Native.ImGui.ImGuiInputTextState.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66530) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c1e0dd799d413293"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.GetCursorPos()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66640) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5d054f0174c54108"></a>[`public float Inno.Native.ImGui.ImGuiInputTextState.GetPreferredOffsetX()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66585) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-994e6da81ef63624"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.GetSelectionEnd()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66662) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c9e6a1f8095afef0"></a>[`public int Inno.Native.ImGui.ImGuiInputTextState.GetSelectionStart()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66651) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ea7a1f410c34b4d0"></a>[`public byte Inno.Native.ImGui.ImGuiInputTextState.HasSelection()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66618) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6f81cb681baa1679"></a>[`public void Inno.Native.ImGui.ImGuiInputTextState.OnCharPressed(uint c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66574) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-513c97ac9af15ece"></a>[`public void Inno.Native.ImGui.ImGuiInputTextState.OnKeyPressed(int key)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66563) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eb6b37ae2b73b516"></a>[`public void Inno.Native.ImGui.ImGuiInputTextState.ReloadUserBufAndKeepSelection()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66706) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6600fec60db62fc2"></a>[`public void Inno.Native.ImGui.ImGuiInputTextState.ReloadUserBufAndMoveToEnd()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66717) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-287b5f965e3d768b"></a>[`public void Inno.Native.ImGui.ImGuiInputTextState.ReloadUserBufAndSelectAll()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66695) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9428178b5db4507a"></a>[`public void Inno.Native.ImGui.ImGuiInputTextState.SelectAll()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66684) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d40de8903cdbc229"></a>[`public void Inno.Native.ImGui.ImGuiInputTextState.SetSelection(int start, int end)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L66673) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8a0a2bf10f7c85b7"></a>[`public struct Inno.Native.ImGui.ImGuiInputTextState`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L7011) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.ImGui.ImGuiInputTextStatePtr</code> · 53 个声明</summary>

<!-- api: T:Inno.Native.ImGui.ImGuiInputTextStatePtr e2ae3fbb649ad6c072260bd332163b2df5b37cd6b4305b461443df8ad8d02e6b -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.#ctor(Inno.Native.ImGui.ImGuiInputTextState*) aa4ff2f62574b84b0a2136f6af689aee30471d7b37a8fc9a825eb49dde58a0dc -->
<!-- api: F:Inno.Native.ImGui.ImGuiInputTextStatePtr.Handle 0e1628120e3fd24e95889e58ed51d9e965b88307beba61f2ee06103f7f2b1b66 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.IsNull 51a1267e543faf645efb74f852a43d0387c80e0483a2580810ec9ef9540c78bb -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.Null 55679663867d82367cf9551bd5187c6950e866aab6a45414b94d5ac8e5e03920 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.Item(System.Int32) 07164a08a148cf7ce3f09ad5b22d7b71703a96cd55a0ffdc250b2a5b329ed01a -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.op_Implicit(Inno.Native.ImGui.ImGuiInputTextState*)~Inno.Native.ImGui.ImGuiInputTextStatePtr ac964c83b403218d3ec3c5259c2b98eb432ed9f53392119ddcc2b9ae460feb38 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.op_Implicit(Inno.Native.ImGui.ImGuiInputTextStatePtr)~Inno.Native.ImGui.ImGuiInputTextState* b4d8124094399e2df16687ea86b544c123238293aa4da453160b6a55c76db57c -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.op_Equality(Inno.Native.ImGui.ImGuiInputTextStatePtr,Inno.Native.ImGui.ImGuiInputTextStatePtr) dd57788ee4b5628fab11e5f9f7b5fda9f6393c7c775178e1854bc129f0cc9f28 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.op_Inequality(Inno.Native.ImGui.ImGuiInputTextStatePtr,Inno.Native.ImGui.ImGuiInputTextStatePtr) b09ac542548e0380cd1b427fe96deb8c2e76faf9e6482dc6a9a1c8fff2792819 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.Equals(Inno.Native.ImGui.ImGuiInputTextStatePtr) 7aafd6aff184a436ddff0a45764d28521251b37ce17ef3b65bbdb0cba8a34ffd -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.Equals(System.Object) 64880eaec52096e04e4a30561f6a2d8bcd2e711908fd1882c8045aa91fb35fde -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.GetHashCode 0b01108fff8917d1f8271af5bcace02888ff167693fcf6b41b119b3b6c5fd126 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.Ctx 9da230244db2c392e2683cbdc741e5f69c17142c01069a22642bc2c69a7b6008 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.Stb e1359f9d7f0455352cc2913c6f302c1cf0ff3560b3c80ffc8cf8946fa5f0bbcf -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.Flags 2639e265739c7685b35cfa432ba5aab6b6d789072ce8f7f9f29eb2892df9cab3 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.ID b703ad84ce6da8d43009de58a0d06faed5ea94807099fb31fe8acce2d9c9fb33 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.TextLen 6986ef6a4e158cabcf8c2bbe0f4b34c84a7afd963709413e330c5c665c3a4a78 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.TextSrc c6e0078f38a5515bc6bd97b6ff014f7747788b834ccd17ade0a7966991c99b23 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.TextA 4016ed6f6b8781be92731a431c7a6d43f7bcac12b13731e153eecd96520305ff -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.TextToRevertTo 16178982303d67bbd064cee971fc4d4fc958560530187adb5be143356fa01f6b -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.CallbackTextBackup 1bcc94d5a1a49530e374bee6f5525a999b51115c4c8c26c824c543a0147b5682 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.BufCapacity 488aa0b064345f698c633ea8ec96f460727943a3252089c3b1e3c4599d467998 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.Scroll df22335f9b0b7be40c54c959675c7042dea5c302471a782409f7ceef950114a5 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.LineCount 37ebaf59dde469486ea0c9710bfd68513a743b893a71ec81d3eac3362cad922e -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.WrapWidth 50e59a835b300668b00fe1ae65585f4373d18505f24ef4d67a57a7dfe58cea96 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.CursorAnim f3f0a88192ed86d831d2e74d2040816f231c78d3540be6d0ed7e324577f7d4d9 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.CursorFollow 94e2f67ef764a7f95d04989f1cb5d46c165b83b9bde86fdcfd634bcf184d9a28 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.CursorCenterY 8fe0992dae5aba476c8b7ce75b15a9fc0ce22bcdcede07ae6a1d317713899e15 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.SelectedAllMouseLock 5e7d971789cbd38fe12f89e4d03d158d9fa0df6030324a486072d4993a8b3fe1 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.Edited 39906068edcff54b94c41304a4ed23c03056d6b3a6aded297417996d9747e2c6 -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.WantReloadUserBuf 78c8d4ef77fb35d966d0413e1fdbc741a128eb309475ae9a92649786b4f02c3c -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.LastMoveDirectionLR c7c2a9c298da2740da10496a4ec344243594288ba8b30bd9b5ec275b05e2c45e -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.ReloadSelectionStart 3c53846a95c0e3b2b749b54d5e9aefd84aaf43169a502707a22d8776073fd36e -->
<!-- api: P:Inno.Native.ImGui.ImGuiInputTextStatePtr.ReloadSelectionEnd 6b961cfa251c88315d0fece01158f4ee8fc5a00d61728587953232fde3d37c45 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.Destroy 94acf1274e54033852d1721995472d5c59a8c3493035dbb4db8d0545f6fe728d -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.ClearText 3d88d82dd913081248dd901ef43cd55779c92edeb4f4c24dafaa6a2537a9d9c4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.ClearFreeMemory 06c7c31e959f7794963919953a7579d00d0d58f104ccd709a7b348fbc2472882 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.OnKeyPressed(System.Int32) 4d6651a90e9fc2d0f1a652222cbdae7ba9d56ec8999d8971be172ec86b4f2f25 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.OnCharPressed(System.UInt32) a50245a414466410cf0bc8685de2ae51d687c78588b811fd6324ba80ec477957 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.GetPreferredOffsetX c988a73f95b5b2e34b5be5e4afbbd10e100fe93748cce3594fb1d37587a044d4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.CursorAnimReset f51e7a9969ed53ac6cef193fd60e65ed97bf4f407bab0d7976a2f7187c7dff95 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.CursorClamp a965b004473ef35f011d2f3378c36751b8879bad8db165104ed6f901afd531dd -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.HasSelection ea3e4ac07bd3ed43ead7475f021fe4fa0e77556c41dc5ebf688777580855ce9d -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.ClearSelection ea6cc51417fc30ad309c6ba050b03c25b9b2f437ad5024723e6fda3937dda948 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.GetCursorPos 0a36778faf00acb3501821dc55e3393a27e64543c1dc1efda7d2edf640829df3 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.GetSelectionStart daa8bf9c4791ce6b841fc6a92f16b46fa9d326ed7b254fd044904cc98c51e544 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.GetSelectionEnd a914b788618ccd0f664272f75ab23f366be394759f9edf71155e26981ce5b9eb -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.SetSelection(System.Int32,System.Int32) bad19df75044e1ba9666f684c9e9486b4c937a94e9b412e893ef411886a265a1 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.SelectAll 0e1bb663b68fc9deb62942596e3d039bfca844f5e21272685fba0c97e1696a93 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.ReloadUserBufAndSelectAll 815a5b876b2929656ff1c52e0d5c5d0b38b613349afeb80bb1c35e51f497e3c6 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.ReloadUserBufAndKeepSelection e540743b3ace27bdb14be5f11f88688239bfbe4f76b588e4f3bffb50f73551e4 -->
<!-- api: M:Inno.Native.ImGui.ImGuiInputTextStatePtr.ReloadUserBufAndMoveToEnd 598850643d09d9ff81987e78ccd6ee5354ebed5a7cd8ced1f062a12e797ac5ff -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9296152613f5ace1"></a>[`public Inno.Native.ImGui.ImGuiInputTextState* Inno.Native.ImGui.ImGuiInputTextStatePtr.Handle`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14634) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bc1b797528874172"></a>[`public Inno.Native.ImGui.ImGuiInputTextStatePtr.ImGuiInputTextStatePtr(Inno.Native.ImGui.ImGuiInputTextState* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14629) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-14ba33851a19f151"></a>[`public void Inno.Native.ImGui.ImGuiInputTextStatePtr.ClearFreeMemory()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14690) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-04767412d419d9ac"></a>[`public void Inno.Native.ImGui.ImGuiInputTextStatePtr.ClearSelection()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14746) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-95ca8b5f1ed76cce"></a>[`public void Inno.Native.ImGui.ImGuiInputTextStatePtr.ClearText()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14682) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a6d8beac0c6067f2"></a>[`public void Inno.Native.ImGui.ImGuiInputTextStatePtr.CursorAnimReset()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14722) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-42ec754495d6666a"></a>[`public void Inno.Native.ImGui.ImGuiInputTextStatePtr.CursorClamp()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14730) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d12c6841c3d86aea"></a>[`public void Inno.Native.ImGui.ImGuiInputTextStatePtr.Destroy()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14674) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e0854034dadee9ed"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextStatePtr.Equals(Inno.Native.ImGui.ImGuiInputTextStatePtr other)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14644) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-fc7e449911725383"></a>[`public override bool Inno.Native.ImGui.ImGuiInputTextStatePtr.Equals(object? obj)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14645) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-41dffccd3efafb00"></a>[`public int Inno.Native.ImGui.ImGuiInputTextStatePtr.GetCursorPos()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14754) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eaefef899a092817"></a>[`public override int Inno.Native.ImGui.ImGuiInputTextStatePtr.GetHashCode()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14646) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-9f28da557f73bdae"></a>[`public float Inno.Native.ImGui.ImGuiInputTextStatePtr.GetPreferredOffsetX()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14714) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c0505ed8d8878090"></a>[`public int Inno.Native.ImGui.ImGuiInputTextStatePtr.GetSelectionEnd()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14770) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-089f069d1eb2139b"></a>[`public int Inno.Native.ImGui.ImGuiInputTextStatePtr.GetSelectionStart()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14762) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ac5cee5b67dff298"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextStatePtr.HasSelection()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14738) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe0edb05d10289bf"></a>[`public void Inno.Native.ImGui.ImGuiInputTextStatePtr.OnCharPressed(uint c)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14706) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-94f5869b8606f504"></a>[`public void Inno.Native.ImGui.ImGuiInputTextStatePtr.OnKeyPressed(int key)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14698) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fc5f92a77e268c5e"></a>[`public static bool Inno.Native.ImGui.ImGuiInputTextStatePtr.operator ==(Inno.Native.ImGui.ImGuiInputTextStatePtr left, Inno.Native.ImGui.ImGuiInputTextStatePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14642) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e8eec4b711ab32c5"></a>[`public static Inno.Native.ImGui.ImGuiInputTextStatePtr.implicit operator Inno.Native.ImGui.ImGuiInputTextStatePtr(Inno.Native.ImGui.ImGuiInputTextState* handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14640) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fb76a9c6fe495eac"></a>[`public static Inno.Native.ImGui.ImGuiInputTextStatePtr.implicit operator Inno.Native.ImGui.ImGuiInputTextState*(Inno.Native.ImGui.ImGuiInputTextStatePtr handle)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14641) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d2a34c44100f6ad2"></a>[`public static bool Inno.Native.ImGui.ImGuiInputTextStatePtr.operator !=(Inno.Native.ImGui.ImGuiInputTextStatePtr left, Inno.Native.ImGui.ImGuiInputTextStatePtr right)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14643) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5728c26138ac3618"></a>[`public void Inno.Native.ImGui.ImGuiInputTextStatePtr.ReloadUserBufAndKeepSelection()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14802) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0fdb8bcdad397d1c"></a>[`public void Inno.Native.ImGui.ImGuiInputTextStatePtr.ReloadUserBufAndMoveToEnd()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14810) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d9da6cefa6c67bec"></a>[`public void Inno.Native.ImGui.ImGuiInputTextStatePtr.ReloadUserBufAndSelectAll()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14794) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3bb22100c2514f46"></a>[`public void Inno.Native.ImGui.ImGuiInputTextStatePtr.SelectAll()`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14786) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-15ad871ff9ec96e3"></a>[`public void Inno.Native.ImGui.ImGuiInputTextStatePtr.SetSelection(int start, int end)`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14778) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7219c9d238f8bcba"></a>[`public ref int Inno.Native.ImGui.ImGuiInputTextStatePtr.BufCapacity { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14657) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-79f96d033f974288"></a>[`public ref Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImGuiInputTextStatePtr.CallbackTextBackup { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14656) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7db1a8d74f3d6706"></a>[`public ref Inno.Native.ImGui.ImGuiContextPtr Inno.Native.ImGui.ImGuiInputTextStatePtr.Ctx { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14648) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-500557e7097ccf3d"></a>[`public ref float Inno.Native.ImGui.ImGuiInputTextStatePtr.CursorAnim { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14661) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a3ca0166357eab28"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextStatePtr.CursorCenterY { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14663) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-36b93ebc62272eac"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextStatePtr.CursorFollow { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14662) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-95b41b99fcf3ebd6"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextStatePtr.Edited { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14665) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a8e3c980e3e3d2f8"></a>[`public ref Inno.Native.ImGui.ImGuiInputTextFlags Inno.Native.ImGui.ImGuiInputTextStatePtr.Flags { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14650) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-56a1ff44edc5ecbb"></a>[`public ref uint Inno.Native.ImGui.ImGuiInputTextStatePtr.ID { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14651) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5639175368ad3ba5"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextStatePtr.IsNull { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14635) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1b23319d94602674"></a>[`public Inno.Native.ImGui.ImGuiInputTextState Inno.Native.ImGui.ImGuiInputTextStatePtr.this[int index] { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14638) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-89f16a4edd84e432"></a>[`public ref byte Inno.Native.ImGui.ImGuiInputTextStatePtr.LastMoveDirectionLR { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14667) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-03b7114e90108596"></a>[`public ref int Inno.Native.ImGui.ImGuiInputTextStatePtr.LineCount { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14659) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2b99effd850dfa69"></a>[`public static Inno.Native.ImGui.ImGuiInputTextStatePtr Inno.Native.ImGui.ImGuiInputTextStatePtr.Null { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14636) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-852a21a3f63da566"></a>[`public ref int Inno.Native.ImGui.ImGuiInputTextStatePtr.ReloadSelectionEnd { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14669) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ced2f43ce332990"></a>[`public ref int Inno.Native.ImGui.ImGuiInputTextStatePtr.ReloadSelectionStart { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14668) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bef96d50bfa09be7"></a>[`public ref System.Numerics.Vector2 Inno.Native.ImGui.ImGuiInputTextStatePtr.Scroll { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14658) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8cb1cd1cdc8bd9f6"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextStatePtr.SelectedAllMouseLock { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14664) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-31c182fee493e304"></a>[`public ref Inno.Native.ImGui.STBTexteditState Inno.Native.ImGui.ImGuiInputTextStatePtr.Stb { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14649) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e13f8c13f22a4dc6"></a>[`public ref Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImGuiInputTextStatePtr.TextA { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14654) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-10663990f435d206"></a>[`public ref int Inno.Native.ImGui.ImGuiInputTextStatePtr.TextLen { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14652) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5e504c277482afad"></a>[`public byte* Inno.Native.ImGui.ImGuiInputTextStatePtr.TextSrc { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14653) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2065afc650ebb584"></a>[`public ref Inno.Native.ImGui.ImVector<byte> Inno.Native.ImGui.ImGuiInputTextStatePtr.TextToRevertTo { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14655) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-345565e6e96038d9"></a>[`public bool Inno.Native.ImGui.ImGuiInputTextStatePtr.WantReloadUserBuf { get; set; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14666) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bd2bbae38cfd9171"></a>[`public ref float Inno.Native.ImGui.ImGuiInputTextStatePtr.WrapWidth { get; }`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14660) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c324aa838e724607"></a>[`public struct Inno.Native.ImGui.ImGuiInputTextStatePtr : System.IEquatable<Inno.Native.ImGui.ImGuiInputTextStatePtr>`](../../../../backends/ImGui/native/Inno.Native.ImGui/Generated/Bindings.cs#L14627) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
