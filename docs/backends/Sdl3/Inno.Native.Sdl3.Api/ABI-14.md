# Inno.Native.Sdl3 ABI · SDLThread → XEvent

[项目与生命周期](../Inno.Native.Sdl3.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.Sdl3.SDLThread</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLThread e489844969c35a5555e523ce711032f84ab5830de52f6ea72ea7a7d4b014eda3 -->
<!-- api: M:Inno.Native.Sdl3.SDLThread.#ctor(System.IntPtr) 597341aaf8d752a6ed723f1dc5472d8e9b9c8b31051cdb1c28c4e4b0b7181443 -->
<!-- api: P:Inno.Native.Sdl3.SDLThread.Handle 039b95cdb820a8392d7b1ef07f7c27e9eae620759d206d3a66cf9e6367a5c7a3 -->
<!-- api: P:Inno.Native.Sdl3.SDLThread.IsNull 50ed6a5dcc475a453d63bd7cd513246cf048cec7426ef7ba6116f984026e0b09 -->
<!-- api: P:Inno.Native.Sdl3.SDLThread.Null e56879539b707fd7a22550b6fb5430a0049324da427f664ecd18f2ca88fdf5b3 -->
<!-- api: M:Inno.Native.Sdl3.SDLThread.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLThread 7eb21d15bb4b4f7cab5c4281b8b6bcda097a857f29b66e2b23ad447adf2bba61 -->
<!-- api: M:Inno.Native.Sdl3.SDLThread.op_Equality(Inno.Native.Sdl3.SDLThread,Inno.Native.Sdl3.SDLThread) 5a2502a63085a59c1a7b2a69be3362af2bfe085c813e2ca55df3c779703329e7 -->
<!-- api: M:Inno.Native.Sdl3.SDLThread.op_Inequality(Inno.Native.Sdl3.SDLThread,Inno.Native.Sdl3.SDLThread) 803be49b879e4e11e3cc744351274baca7b8e9c29bb359e211044ca4eaf93061 -->
<!-- api: M:Inno.Native.Sdl3.SDLThread.Equals(Inno.Native.Sdl3.SDLThread) f791da041f89b146ef983434571a41af464acc4a0638502bc1649e1e8602e57d -->
<!-- api: M:Inno.Native.Sdl3.SDLThread.Equals(System.Object) 2a2720b4e3da2c67ac2cac5c627b03e94027ae73a6a395d2bf204e4639e12564 -->
<!-- api: M:Inno.Native.Sdl3.SDLThread.GetHashCode f70c8eb730420b4ed7ae36943f3b9932e72c792112fd1e1dabab525d53cce408 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-452e782fdc2fea31"></a>[`public Inno.Native.Sdl3.SDLThread.SDLThread(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2098) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-23adf89d70107536"></a>[`public bool Inno.Native.Sdl3.SDLThread.Equals(Inno.Native.Sdl3.SDLThread other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2110) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-2e0c196bbb56b4fd"></a>[`public override bool Inno.Native.Sdl3.SDLThread.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2111) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-a0e93e540fa4dd3e"></a>[`public override int Inno.Native.Sdl3.SDLThread.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2112) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-db1a3c80c90d9287"></a>[`public static bool Inno.Native.Sdl3.SDLThread.operator ==(Inno.Native.Sdl3.SDLThread left, Inno.Native.Sdl3.SDLThread right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2108) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-96b7a0dcb31d2bf7"></a>[`public static Inno.Native.Sdl3.SDLThread.implicit operator Inno.Native.Sdl3.SDLThread(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2107) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a26874aec9d341fd"></a>[`public static bool Inno.Native.Sdl3.SDLThread.operator !=(Inno.Native.Sdl3.SDLThread left, Inno.Native.Sdl3.SDLThread right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2109) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6b24a70a832a15bc"></a>[`public nint Inno.Native.Sdl3.SDLThread.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2103) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f3413ea8ce24dd5c"></a>[`public bool Inno.Native.Sdl3.SDLThread.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2104) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9057119c1ea44459"></a>[`public static Inno.Native.Sdl3.SDLThread Inno.Native.Sdl3.SDLThread.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2105) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ef2f4448e608ae0"></a>[`public readonly struct Inno.Native.Sdl3.SDLThread : System.IEquatable<Inno.Native.Sdl3.SDLThread>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2096) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLThreadFunction</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLThreadFunction 8fbc1266fa6e5f1bc0178e93e70d2eb51ea7e3a3311fdf83db2fb5489fb4dffa -->
<!-- api: M:Inno.Native.Sdl3.SDLThreadFunction.#ctor(System.Object,System.IntPtr) 2b0538104505a1883ee31f744c66235cf862acfe47629910afd4215559a2b80a -->
<!-- api: M:Inno.Native.Sdl3.SDLThreadFunction.Invoke(System.Void*) de945fa1dc3eb7350a21596a386b093e5f294e86acb47c4d7cfdfca59eab23d1 -->
<!-- api: M:Inno.Native.Sdl3.SDLThreadFunction.BeginInvoke(System.Void*,System.AsyncCallback,System.Object) fe2c8bf707ffde7caf91e9f842ae9c072626672205a609b9f22a33e3df8e2bbb -->
<!-- api: M:Inno.Native.Sdl3.SDLThreadFunction.EndInvoke(System.IAsyncResult) 0600f562323fce69dfd2a3e11738e477ba389783e6908451d1464d752f397689 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e7c269e1796ba997"></a>[`public Inno.Native.Sdl3.SDLThreadFunction.SDLThreadFunction(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6815) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9bbe5b16f2c3ffd8"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLThreadFunction.BeginInvoke(void* data, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6815) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e34fe24a8bf6f13d"></a>[`public virtual int Inno.Native.Sdl3.SDLThreadFunction.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6815) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e8e87019cc572ab9"></a>[`public virtual int Inno.Native.Sdl3.SDLThreadFunction.Invoke(void* data)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6815) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f01a552e1b109130"></a>[`public delegate int Inno.Native.Sdl3.SDLThreadFunction(void* data)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6815) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLThreadPriority</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLThreadPriority c0443890fc8f7e67ec247c694f4ea57932020b096eaa06bc5633a16baccc6ec4 -->
<!-- api: F:Inno.Native.Sdl3.SDLThreadPriority.Low 54f8751d5eb073437214023e739f9903ff2226d7e408fd288efd665d4c08d74c -->
<!-- api: F:Inno.Native.Sdl3.SDLThreadPriority.Normal 63b1dd2b15a170e35a42d2194de9fbc5ee49638c788ca1bf24bbef2cb5ca1561 -->
<!-- api: F:Inno.Native.Sdl3.SDLThreadPriority.High 2195602c5a16e4879cdb204ceda31dbf359915e525e80442cbff0cf560e8857d -->
<!-- api: F:Inno.Native.Sdl3.SDLThreadPriority.TimeCritical f7791aeda07167268dcd250216fc0c65caa922f1d32e06e3b0576d2830949b58 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f126386f0e8a435d"></a>[`Inno.Native.Sdl3.SDLThreadPriority.High = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L127) | 当前常量或枚举值：`2`。 | — |
| <a id="api-95df5760aedbd388"></a>[`Inno.Native.Sdl3.SDLThreadPriority.Low = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L125) | 当前常量或枚举值：`0`。 | — |
| <a id="api-84ca490f1014a3be"></a>[`Inno.Native.Sdl3.SDLThreadPriority.Normal = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L126) | 当前常量或枚举值：`1`。 | — |
| <a id="api-b88f6868d2a7f6d2"></a>[`Inno.Native.Sdl3.SDLThreadPriority.TimeCritical = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L128) | 当前常量或枚举值：`3`。 | — |
| <a id="api-99766c11a4ec6754"></a>[`public enum Inno.Native.Sdl3.SDLThreadPriority : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L123) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLThreadState</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLThreadState 6d608282a425e18c40861b3eb205f7c71a981361653446e35d7e1f04a71c5e3d -->
<!-- api: F:Inno.Native.Sdl3.SDLThreadState.Unknown 442684e26bfbd49e7671f3416abc454f41b3a191241e0c425ad6110311f45aed -->
<!-- api: F:Inno.Native.Sdl3.SDLThreadState.Alive e9722b8f75aeb5cb6c7f4e7d968116f1fa375f04a370c812b87d56369f9da8a3 -->
<!-- api: F:Inno.Native.Sdl3.SDLThreadState.Detached 48a891a0368e58f70f9e8a91126e4f4ab36f43f6595dd1825084ec6c391ac53d -->
<!-- api: F:Inno.Native.Sdl3.SDLThreadState.Complete 93d5d7413643f7d39b75ed87df2b9a391f873743ed7d865463b9f89544d5d93c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-919e40625994a200"></a>[`Inno.Native.Sdl3.SDLThreadState.Alive = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L137) | 当前常量或枚举值：`1`。 | — |
| <a id="api-94ec4d8aaeaf0094"></a>[`Inno.Native.Sdl3.SDLThreadState.Complete = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L139) | 当前常量或枚举值：`3`。 | — |
| <a id="api-8bd4b17fe8460100"></a>[`Inno.Native.Sdl3.SDLThreadState.Detached = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L138) | 当前常量或枚举值：`2`。 | — |
| <a id="api-96eccdc011cd3cb2"></a>[`Inno.Native.Sdl3.SDLThreadState.Unknown = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L136) | 当前常量或枚举值：`0`。 | — |
| <a id="api-f9c4c5caa4145ed3"></a>[`public enum Inno.Native.Sdl3.SDLThreadState : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L134) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLTimeFormat</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLTimeFormat 18c13c839ebdea6f2bd56dbc3c320b1d47ffb6d54af5d6e80de62f37d01e3547 -->
<!-- api: F:Inno.Native.Sdl3.SDLTimeFormat.Format24Hr 50d09ecab2c4022f75760e93a644a2f8fbe6c807ea142590b55c64ff48c79cb0 -->
<!-- api: F:Inno.Native.Sdl3.SDLTimeFormat.Format12Hr 08a604f7f8bb56138337e0daf37d075ef83b730073f2df2f372ccf8292250120 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6f6666fc1147e407"></a>[`Inno.Native.Sdl3.SDLTimeFormat.Format12Hr = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1900) | 当前常量或枚举值：`1`。 | — |
| <a id="api-0212e594ab20102d"></a>[`Inno.Native.Sdl3.SDLTimeFormat.Format24Hr = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1899) | 当前常量或枚举值：`0`。 | — |
| <a id="api-7f8aad689b2efbba"></a>[`public enum Inno.Native.Sdl3.SDLTimeFormat : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1897) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLTimerCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLTimerCallback bf80147c6406a4e610cfd4e79c0206c3a4d73d7ae380ed32e3365c66a930ac08 -->
<!-- api: M:Inno.Native.Sdl3.SDLTimerCallback.#ctor(System.Object,System.IntPtr) 02a39b5f67f5c5e9a0ef404f6cc0dd59b073a3b6ce3780dad1ffeb7c7982ea3c -->
<!-- api: M:Inno.Native.Sdl3.SDLTimerCallback.Invoke(System.Void*,System.UInt32,System.UInt32) 2703e6387da0802cb8cb6c196c6d4efb94a8ee64b1e10256bcde0ea86df00316 -->
<!-- api: M:Inno.Native.Sdl3.SDLTimerCallback.BeginInvoke(System.Void*,System.UInt32,System.UInt32,System.AsyncCallback,System.Object) 49144228556a3f309b427505f83c1aa58d7b12008300062ef2e62261458ab370 -->
<!-- api: M:Inno.Native.Sdl3.SDLTimerCallback.EndInvoke(System.IAsyncResult) c79b9b3d1ba79b863932025ad91f148656bf475b6a7964abad78d110dce6e9a0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ab769b91ef6eb4d6"></a>[`public Inno.Native.Sdl3.SDLTimerCallback.SDLTimerCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6925) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-c321cda922b780ae"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLTimerCallback.BeginInvoke(void* userdata, uint timerID, uint interval, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6925) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ebb7314494ccdcdf"></a>[`public virtual uint Inno.Native.Sdl3.SDLTimerCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6925) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2a85ac1a47a1a3c7"></a>[`public virtual uint Inno.Native.Sdl3.SDLTimerCallback.Invoke(void* userdata, uint timerID, uint interval)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6925) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ddb5a8a31744563f"></a>[`public delegate uint Inno.Native.Sdl3.SDLTimerCallback(void* userdata, uint timerID, uint interval)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6925) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLTLSDestructorCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLTLSDestructorCallback e5c18b00ae6a1af2cad85204527ed60a645229deefae4f1a9fe4f54078bca5c2 -->
<!-- api: M:Inno.Native.Sdl3.SDLTLSDestructorCallback.#ctor(System.Object,System.IntPtr) b768093c5ccaa17c1d1fdb2bc948328612e7002d5425cb9229986fc272871af1 -->
<!-- api: M:Inno.Native.Sdl3.SDLTLSDestructorCallback.Invoke(System.Void*) e920fd9652e897d80a5cf384b1d943300899e731cfd9598dd08c4b63c91ae2f5 -->
<!-- api: M:Inno.Native.Sdl3.SDLTLSDestructorCallback.BeginInvoke(System.Void*,System.AsyncCallback,System.Object) e695f05867c81bb64de504a5a2f51a5c2e4167bbab3f23c1e26dcddeda7e08db -->
<!-- api: M:Inno.Native.Sdl3.SDLTLSDestructorCallback.EndInvoke(System.IAsyncResult) 9b4ad20c3c7c0ea620811a8a58071c8369a2f6d303a5631ddfb0535760c3c733 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6876832fa583159b"></a>[`public Inno.Native.Sdl3.SDLTLSDestructorCallback.SDLTLSDestructorCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6820) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5f9f5297ab579f65"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLTLSDestructorCallback.BeginInvoke(void* value, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6820) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b06a8d7bf19d5c58"></a>[`public virtual void Inno.Native.Sdl3.SDLTLSDestructorCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6820) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4e9a41079ec3fb30"></a>[`public virtual void Inno.Native.Sdl3.SDLTLSDestructorCallback.Invoke(void* value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6820) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7c87b9ab7fe301c7"></a>[`public delegate void Inno.Native.Sdl3.SDLTLSDestructorCallback(void* value)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6820) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLTouchDeviceType</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLTouchDeviceType ec78fcec739ac268ee772350bdef54454b13c606f74c946d872171a87014ca90 -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchDeviceType.Invalid 28f40e821fe14f6489a335ba30b40ef0b42b4314f9d9a6983474aa2e8c0f6961 -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchDeviceType.Direct 17a1eedd452a84eda2f81670a6169032b3ab25ff787bb6c9d3e1c40febe1d6bc -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchDeviceType.IndirectAbsolute 1e5d1363edc9bfc9301f36c25446a3a1c3f34ea17f6db83fa83e80b9fd7f57b8 -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchDeviceType.IndirectRelative f7197be766b86ae945b231dba933085122d5bd5adcaf8c65ee1ba7487ff62d4e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0f1cfe7ddb7d1d4d"></a>[`Inno.Native.Sdl3.SDLTouchDeviceType.Direct = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1130) | 当前常量或枚举值：`0`。 | — |
| <a id="api-33a256f10e81c597"></a>[`Inno.Native.Sdl3.SDLTouchDeviceType.IndirectAbsolute = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1131) | 当前常量或枚举值：`1`。 | — |
| <a id="api-38f2c61e480f4be3"></a>[`Inno.Native.Sdl3.SDLTouchDeviceType.IndirectRelative = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1132) | 当前常量或枚举值：`2`。 | — |
| <a id="api-ec2dea97699b48b3"></a>[`Inno.Native.Sdl3.SDLTouchDeviceType.Invalid = -1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1129) | 当前常量或枚举值：`-1`。 | — |
| <a id="api-740aaee6ff18e06b"></a>[`public enum Inno.Native.Sdl3.SDLTouchDeviceType : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1127) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLTouchFingerEvent</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLTouchFingerEvent 3ded74051cd4e0d968f700283004c5676a7866fff712d1e9f4d0f10dd2ea64f3 -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchFingerEvent.Type e092e26078d9ef2c46a925871fb7108924d7ece8e6b4302c64cc9b52b7f71657 -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchFingerEvent.Reserved dc7b0b52cc7633e8dc7fbd43d8fc3bf8e87f17d73c5a605eaa943b50b8bfc079 -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchFingerEvent.Timestamp 77c9cfe6501ee8694055cc7f197e75c2ebfa8fcc51094eec7b3f55fc58fe05d1 -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchFingerEvent.TouchId a0007b92fe9a71cdf79c02c2b9bcef744e9beeaa36ac4d6fcf5b1bbf6f4eeff8 -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchFingerEvent.FingerId 483de16f19c924cdf81a61a4fa372838e1f9e3517df8bc4acc86edfdabcc7498 -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchFingerEvent.X eb7e92ac7acaa01283305a839b8cdcf98abd27202009356e0c497d4d9d02933f -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchFingerEvent.Y 63c432cdbb190a32067dcd56b9f4c16173229714a1b8675bef086f50daff07e2 -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchFingerEvent.Dx 155b51fec9edd9d35cd7e3f26cd296df6cff2ae69157bfdcf9895712f223c6eb -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchFingerEvent.Dy b07e1dd498eef2d80c4826c8d6f0e12be4e6cfc2bd75cc20884141ccd21ae8b3 -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchFingerEvent.Pressure 6206b685f77d31672491e2cc9c9bf78cb49e11d526fa154ce751f9646a055777 -->
<!-- api: F:Inno.Native.Sdl3.SDLTouchFingerEvent.WindowId ccf915ea8fe9dae2f11311163b8db5a56dbb642df4c0c26fc31f24831e8bfe3e -->
<!-- api: M:Inno.Native.Sdl3.SDLTouchFingerEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt64,System.UInt64,System.Single,System.Single,System.Single,System.Single,System.Single,System.UInt32) 15947a686f0ae3d3ee4c9462d04a76c65e314936fe4380fb8c782db34b77865d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e3a2825e3ef7e077"></a>[`public float Inno.Native.Sdl3.SDLTouchFingerEvent.Dx`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3756) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a1fa61b4097c155a"></a>[`public float Inno.Native.Sdl3.SDLTouchFingerEvent.Dy`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3757) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2ce22daf23163f93"></a>[`public ulong Inno.Native.Sdl3.SDLTouchFingerEvent.FingerId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3753) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7bf966fd5496bed0"></a>[`public float Inno.Native.Sdl3.SDLTouchFingerEvent.Pressure`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3758) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b679860f3987365c"></a>[`public uint Inno.Native.Sdl3.SDLTouchFingerEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3750) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b1bba62a215f3a4d"></a>[`public ulong Inno.Native.Sdl3.SDLTouchFingerEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3751) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb3b36fe40be5753"></a>[`public ulong Inno.Native.Sdl3.SDLTouchFingerEvent.TouchId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3752) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0fc422c2038c064a"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLTouchFingerEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3749) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-179d132b4cd61937"></a>[`public uint Inno.Native.Sdl3.SDLTouchFingerEvent.WindowId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3759) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f23862b0930a893c"></a>[`public float Inno.Native.Sdl3.SDLTouchFingerEvent.X`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3754) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ae6606a13bdf7fc"></a>[`public float Inno.Native.Sdl3.SDLTouchFingerEvent.Y`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3755) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-10b7fd29bc90b99a"></a>[`public Inno.Native.Sdl3.SDLTouchFingerEvent.SDLTouchFingerEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, ulong touchID = 0, ulong fingerID = 0, float x = 0, float y = 0, float dx = 0, float dy = 0, float pressure = 0, uint windowID = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3760) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3af0614067066017"></a>[`public struct Inno.Native.Sdl3.SDLTouchFingerEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3747) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLTransferCharacteristics</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLTransferCharacteristics 2b220c60489e6c5291b103e99a1562f0f7d386f56c54b5169ade8237a200f5d6 -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Unknown 1b2f6de69a9e668fbb048cd470d9616e6a5b9f1aa27e5582eb854e4cdeffbf67 -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Bt709 48295cf908f455bef18c2033fa7996eeb96e15ec2d6b2575e25375b593296392 -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Unspecified 6f91435d5b4a643051915109a564b4699cdf903e9b5d0708a9f3936b3bd15415 -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Gamma22 e8a515bdb82675372d22ee688195b7e54dab237d590872b7b4a6bb7978f6ef3e -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Gamma28 f4259cf7b600f77eebcd9961ea5f326c7bb6b55badbb18ad0358021234123b52 -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Bt601 f20785ff1cbad86bc59ddae4dd5ea365e639bafb0470ecd98dc06e1cdfc02534 -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Smpte240 ec693b853772a6fe32e8d0227d23c64c586001fe68cc012043565f1e15dc6bcf -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Linear 82a2537c76be7f0375ef61447441dfc417c37ed684b8eb3eaf1a4628d54f8450 -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Log100 2f0479aa6d83519356b50bc752dfcad047590596659b95112893dbf63a8b542f -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Log100Sqrt10 0362e5f33aa08884c33452c8deb99efecfa9dcd30679b966f17fe4dad2189ff4 -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Iec61966 199ad3dec0d764b0212158740e68c8f54a912f92f1f6dbb06cfd96382bfd82f7 -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Bt1361 8414e3fdb1e8b607b0f51ca09caa5d505f1dd2538e07cedf44ede1625b8669f5 -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Srgb eb9c6aedf29d28ec73c82dabfd42f96a2f4bc13df3e25188cce0521db8e52061 -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Bt202010Bit 970587a9925f0a65a6b4171eea16e722edf4f93e7c195832480198563ad288cd -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Bt202012Bit bf6a2bf6b6e4982d94a244d53ebbeffa277638022cf370d711debd1f121d6f5b -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Pq 3b99ddf1610023d874413e6395eb70d71162f4166b26176e6de52b61a296552d -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Smpte428 6345467e896a87feaced34455e8f1556d7df7bdca4dfd2920cb757c19eebeb95 -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Hlg da077e7c7ca7086924c6001e043011a64b649321fa0327d1b420346d7cf26afc -->
<!-- api: F:Inno.Native.Sdl3.SDLTransferCharacteristics.Custom c346982151fc8c5484b6ddfa6832ed0070f56ea7e3625f1bd6d069f9911196ce -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-84f8cab9071bb3dd"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Bt1361 = 12`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L437) | 当前常量或枚举值：`12`。 | — |
| <a id="api-338d12e3b49fbbcf"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Bt202010Bit = 14`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L439) | 当前常量或枚举值：`14`。 | — |
| <a id="api-4a80277a37f26d55"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Bt202012Bit = 15`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L440) | 当前常量或枚举值：`15`。 | — |
| <a id="api-143618d26f6365dc"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Bt601 = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L431) | 当前常量或枚举值：`6`。 | — |
| <a id="api-ac75b1d3e56dc151"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Bt709 = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L427) | 当前常量或枚举值：`1`。 | — |
| <a id="api-04a61978d5f193a3"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Custom = 31`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L444) | 当前常量或枚举值：`31`。 | — |
| <a id="api-72276922493556d4"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Gamma22 = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L429) | 当前常量或枚举值：`4`。 | — |
| <a id="api-4f4fb7fe991bb899"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Gamma28 = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L430) | 当前常量或枚举值：`5`。 | — |
| <a id="api-7b4f28964c67fb0f"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Hlg = 18`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L443) | 当前常量或枚举值：`18`。 | — |
| <a id="api-6ec0f49058cde3da"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Iec61966 = 11`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L436) | 当前常量或枚举值：`11`。 | — |
| <a id="api-108e498c5072fe62"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Linear = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L433) | 当前常量或枚举值：`8`。 | — |
| <a id="api-e776749aecbd2c3d"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Log100 = 9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L434) | 当前常量或枚举值：`9`。 | — |
| <a id="api-af14d2c2b00b12cd"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Log100Sqrt10 = 10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L435) | 当前常量或枚举值：`10`。 | — |
| <a id="api-57b459620f082516"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Pq = 16`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L441) | 当前常量或枚举值：`16`。 | — |
| <a id="api-e3367a898e374261"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Smpte240 = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L432) | 当前常量或枚举值：`7`。 | — |
| <a id="api-b5600e4bcbb6083e"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Smpte428 = 17`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L442) | 当前常量或枚举值：`17`。 | — |
| <a id="api-3407e58be9a41434"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Srgb = 13`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L438) | 当前常量或枚举值：`13`。 | — |
| <a id="api-3d28108af9215ad5"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Unknown = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L426) | 当前常量或枚举值：`0`。 | — |
| <a id="api-6a68256c66cc20bd"></a>[`Inno.Native.Sdl3.SDLTransferCharacteristics.Unspecified = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L428) | 当前常量或枚举值：`2`。 | — |
| <a id="api-22fcc7acba494f54"></a>[`public enum Inno.Native.Sdl3.SDLTransferCharacteristics : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L424) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLTray</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLTray 05d1465ce5b1cc193d45de3ff20185546b6a2f21d1f79ad6dfe3262bd0c6e6ac -->
<!-- api: M:Inno.Native.Sdl3.SDLTray.#ctor(System.IntPtr) 525dc18113207010fbd9ebeefb07b130da33ffc391e4514860b2e009c5397337 -->
<!-- api: P:Inno.Native.Sdl3.SDLTray.Handle 8b4d8aa8e7e9474a127503ed717d637f034ef13d4f32903c062270dd924c41ea -->
<!-- api: P:Inno.Native.Sdl3.SDLTray.IsNull 11012819399d3950b3a8e67ba9b3ffe305d2ede041e8cede074513ceb64bdef9 -->
<!-- api: P:Inno.Native.Sdl3.SDLTray.Null 3ce190283a32941443ffad53bdabbc9c8197a027fae5e649f11329112d2be6b1 -->
<!-- api: M:Inno.Native.Sdl3.SDLTray.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLTray 1f3848fd7b07127fa2c2786766f4e699262d5237d29ad2e72251f89571495c7e -->
<!-- api: M:Inno.Native.Sdl3.SDLTray.op_Equality(Inno.Native.Sdl3.SDLTray,Inno.Native.Sdl3.SDLTray) 8ac954f02875ca9218484ff8619401a10af750d59ffa82cc15e977fce2131cbe -->
<!-- api: M:Inno.Native.Sdl3.SDLTray.op_Inequality(Inno.Native.Sdl3.SDLTray,Inno.Native.Sdl3.SDLTray) 5b2e5f483b7fde2e136158f534b9aeb1084697a2bc115f33e6cb7fac92ad719d -->
<!-- api: M:Inno.Native.Sdl3.SDLTray.Equals(Inno.Native.Sdl3.SDLTray) dfa9bae10a416d79621a0cefc91c1895dc02bf987d6f3dcf8de7bf189ddfbc57 -->
<!-- api: M:Inno.Native.Sdl3.SDLTray.Equals(System.Object) 3688f336c5180f6cf13ce4db2220ec3cecf478f6aee8cac3c2d09b388068671a -->
<!-- api: M:Inno.Native.Sdl3.SDLTray.GetHashCode 1e87a03bda286ed5d09ca8fef64ac79af100d23cb6202999c1dc9d57603bbbf1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-390a75637dcd9428"></a>[`public Inno.Native.Sdl3.SDLTray.SDLTray(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6404) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6803b3f00666ca00"></a>[`public bool Inno.Native.Sdl3.SDLTray.Equals(Inno.Native.Sdl3.SDLTray other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6416) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-d29ae82e23d3a11e"></a>[`public override bool Inno.Native.Sdl3.SDLTray.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6417) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-cbcd20c01aa4b124"></a>[`public override int Inno.Native.Sdl3.SDLTray.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6418) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-12b986e141a60263"></a>[`public static bool Inno.Native.Sdl3.SDLTray.operator ==(Inno.Native.Sdl3.SDLTray left, Inno.Native.Sdl3.SDLTray right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6414) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0cc450063dfca921"></a>[`public static Inno.Native.Sdl3.SDLTray.implicit operator Inno.Native.Sdl3.SDLTray(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6413) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-80ffec7c1a52b50e"></a>[`public static bool Inno.Native.Sdl3.SDLTray.operator !=(Inno.Native.Sdl3.SDLTray left, Inno.Native.Sdl3.SDLTray right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6415) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e64f2c1afa562198"></a>[`public nint Inno.Native.Sdl3.SDLTray.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6409) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b5849d3b4ab48c48"></a>[`public bool Inno.Native.Sdl3.SDLTray.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6410) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f8ea0a3fe64639a5"></a>[`public static Inno.Native.Sdl3.SDLTray Inno.Native.Sdl3.SDLTray.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6411) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-25b4dd1899474681"></a>[`public readonly struct Inno.Native.Sdl3.SDLTray : System.IEquatable<Inno.Native.Sdl3.SDLTray>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6402) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLTrayCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLTrayCallback e9bf1c917c83ae3751892a505d7282a8491368610f59a1a499bb586c9586c2da -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayCallback.#ctor(System.Object,System.IntPtr) dadc30e954784255096441ad8363b1c1c59e9582c0d9646fa79538d7bba758f2 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayCallback.Invoke(System.Void*,Inno.Native.Sdl3.SDLTrayEntry) 81ba6d8ecd6a093aa13a353a78a77d7174455a44a3495f7423309b5a2fec0ef4 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayCallback.BeginInvoke(System.Void*,Inno.Native.Sdl3.SDLTrayEntry,System.AsyncCallback,System.Object) 160897c2a0d88f6520f1293a12957f4a3acce7739918acea988bb6a4f6696dcd -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayCallback.EndInvoke(System.IAsyncResult) ce67aed3724d1518ca6ae003715060ed6a001771f5cab086c0c73f41adc5b562 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-63b548e1251b16a7"></a>[`public Inno.Native.Sdl3.SDLTrayCallback.SDLTrayCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6935) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-003c032884b82fcd"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLTrayCallback.BeginInvoke(void* userdata, Inno.Native.Sdl3.SDLTrayEntry entry, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6935) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-078c639a1e8f3d15"></a>[`public virtual void Inno.Native.Sdl3.SDLTrayCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6935) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-248efc56c34d02e3"></a>[`public virtual void Inno.Native.Sdl3.SDLTrayCallback.Invoke(void* userdata, Inno.Native.Sdl3.SDLTrayEntry entry)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6935) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cb02f1519eea5c40"></a>[`public delegate void Inno.Native.Sdl3.SDLTrayCallback(void* userdata, Inno.Native.Sdl3.SDLTrayEntry entry)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6935) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLTrayClickCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLTrayClickCallback 8f5fef8fbb01637a8dddbc80a5461333f87012dd978af457832d254d54befbdd -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayClickCallback.#ctor(System.Object,System.IntPtr) 98cb2ee5f422cb97e92a260845d5bacc053fac99a7f569607215cf1b985eb956 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayClickCallback.Invoke(System.Void*,Inno.Native.Sdl3.SDLTray) 602a63f05e308f67069b284d426ce74fe257dca5bee8351783a20d65ab276447 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayClickCallback.BeginInvoke(System.Void*,Inno.Native.Sdl3.SDLTray,System.AsyncCallback,System.Object) 128f5ebf4d2c015bdad88adccd9b822dac01f41190b659ef601ce76d518befb0 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayClickCallback.EndInvoke(System.IAsyncResult) 57b7d9820b13fcef35559c9d5b8120a8b66f637da4c1da7842f410cf74fcbb27 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-992a402c63f12c28"></a>[`public Inno.Native.Sdl3.SDLTrayClickCallback.SDLTrayClickCallback(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6940) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f58b63bc59d97410"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLTrayClickCallback.BeginInvoke(void* userdata, Inno.Native.Sdl3.SDLTray tray, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6940) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bf69654c7a15fef3"></a>[`public virtual byte Inno.Native.Sdl3.SDLTrayClickCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6940) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9f928b01e5cdd099"></a>[`public virtual byte Inno.Native.Sdl3.SDLTrayClickCallback.Invoke(void* userdata, Inno.Native.Sdl3.SDLTray tray)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6940) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cb63ef00d1a4c6e2"></a>[`public delegate byte Inno.Native.Sdl3.SDLTrayClickCallback(void* userdata, Inno.Native.Sdl3.SDLTray tray)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6940) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLTrayEntry</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLTrayEntry 62abd35ddf121584bd0db963055c0967a615a686beba3d289c4af4477b25788b -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayEntry.#ctor(System.IntPtr) 85efcececfd45cfe42a99428edf03b38405fc694f28f0a1497cf6a91172ab5f6 -->
<!-- api: P:Inno.Native.Sdl3.SDLTrayEntry.Handle 5b1c68e02fdbc364c2eb8698e4f0103f1169062c7c58e6d20164c9222741528a -->
<!-- api: P:Inno.Native.Sdl3.SDLTrayEntry.IsNull d81aff461caa9d443ed5dba2709d7bdce435ed8e4c836fbcda4956f958919969 -->
<!-- api: P:Inno.Native.Sdl3.SDLTrayEntry.Null 764cf273cbe44d90e450c49d23b91f45d4053817843b4aeb612aaf11aef9b0c3 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayEntry.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLTrayEntry 722b517feb30331752235c58d45d89b55c4c57499ede43207b92efea7ab85ab1 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayEntry.op_Equality(Inno.Native.Sdl3.SDLTrayEntry,Inno.Native.Sdl3.SDLTrayEntry) 10321f7d993975c9edf57db7b4902c2323fd075d1e4cf20cb67e557a8fe1ab28 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayEntry.op_Inequality(Inno.Native.Sdl3.SDLTrayEntry,Inno.Native.Sdl3.SDLTrayEntry) 0a776b6eeae20568b31789bf9797677d9f24b90aca080294fc753796cb969e48 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayEntry.Equals(Inno.Native.Sdl3.SDLTrayEntry) 30cf4b1c454a857e2f3952d346981ef55bbac258cb7692169aaf5b895b4de893 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayEntry.Equals(System.Object) 1156a894b8d1e3deb2da42f1e7a4d72a67730a201943af9bebc0cc5129761c96 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayEntry.GetHashCode 8f9ebb6b265c49072732d4e10e44d9dd2faef352b22bbb2fe62b3152d85c9571 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-12cf3465d5920090"></a>[`public Inno.Native.Sdl3.SDLTrayEntry.SDLTrayEntry(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6452) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-36d4560df8f28346"></a>[`public bool Inno.Native.Sdl3.SDLTrayEntry.Equals(Inno.Native.Sdl3.SDLTrayEntry other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6464) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-efc5f0005a9d05b8"></a>[`public override bool Inno.Native.Sdl3.SDLTrayEntry.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6465) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-a511ed574689c70b"></a>[`public override int Inno.Native.Sdl3.SDLTrayEntry.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6466) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-2e0d7eda308464bf"></a>[`public static bool Inno.Native.Sdl3.SDLTrayEntry.operator ==(Inno.Native.Sdl3.SDLTrayEntry left, Inno.Native.Sdl3.SDLTrayEntry right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6462) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-34e7682bd32bb6e4"></a>[`public static Inno.Native.Sdl3.SDLTrayEntry.implicit operator Inno.Native.Sdl3.SDLTrayEntry(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6461) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-90b03ea4d8583ed7"></a>[`public static bool Inno.Native.Sdl3.SDLTrayEntry.operator !=(Inno.Native.Sdl3.SDLTrayEntry left, Inno.Native.Sdl3.SDLTrayEntry right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6463) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cc0bf2ae601d8852"></a>[`public nint Inno.Native.Sdl3.SDLTrayEntry.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6457) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-45a3a1bcc3017785"></a>[`public bool Inno.Native.Sdl3.SDLTrayEntry.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6458) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-48817a28396c6f20"></a>[`public static Inno.Native.Sdl3.SDLTrayEntry Inno.Native.Sdl3.SDLTrayEntry.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6459) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f031ae8af2096b7b"></a>[`public readonly struct Inno.Native.Sdl3.SDLTrayEntry : System.IEquatable<Inno.Native.Sdl3.SDLTrayEntry>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6450) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLTrayMenu</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLTrayMenu 71d0451b80f9f6dcc2962e87f185723b4a7ddba7650c74f5b2d436dd836799f3 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayMenu.#ctor(System.IntPtr) 17e2f401f0454249a654971c96121607148c7880d721b1967683e2f56cc9fc6c -->
<!-- api: P:Inno.Native.Sdl3.SDLTrayMenu.Handle c098530a7f15658bd9247e6b43a26eb75489a0311007295377b9435ae4c1573e -->
<!-- api: P:Inno.Native.Sdl3.SDLTrayMenu.IsNull 17b01a5b2a437ad07776c233a367815f6d1455fe43edb1b5f1e295ab7b068c13 -->
<!-- api: P:Inno.Native.Sdl3.SDLTrayMenu.Null 047e5b816fffc4fe72de5c5ea413573c196416b680d4ebac837e409111b55b88 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayMenu.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLTrayMenu 146734c53b342d6c4ce8954b4c35429f95b1892d6b34b9210eaf4e2f708ec43e -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayMenu.op_Equality(Inno.Native.Sdl3.SDLTrayMenu,Inno.Native.Sdl3.SDLTrayMenu) 20408967e881e6773a75393290d383d5b3b4d8be93551b0e2e3c0696a5f5964e -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayMenu.op_Inequality(Inno.Native.Sdl3.SDLTrayMenu,Inno.Native.Sdl3.SDLTrayMenu) bbb8ff4c7ccead63bc767a43d62dedf967ffdf9ac2c96383e057bb7d178191f9 -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayMenu.Equals(Inno.Native.Sdl3.SDLTrayMenu) cf95a7637d1891c898f373828948ac8b643595645143441ceb829b43e54b700a -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayMenu.Equals(System.Object) 5d6c5117d0e1c59070878e2112195bf211149c74d11c3b029606fa2084cebfac -->
<!-- api: M:Inno.Native.Sdl3.SDLTrayMenu.GetHashCode e500f2092d569e07523b2be59f8af0f18279a14751f51cdb19a447601ea35bd5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-54639aeac290a7fd"></a>[`public Inno.Native.Sdl3.SDLTrayMenu.SDLTrayMenu(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6428) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-faf88f91c348499b"></a>[`public bool Inno.Native.Sdl3.SDLTrayMenu.Equals(Inno.Native.Sdl3.SDLTrayMenu other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6440) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-6b87183318f7e98a"></a>[`public override bool Inno.Native.Sdl3.SDLTrayMenu.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6441) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-4c75502d58ff238f"></a>[`public override int Inno.Native.Sdl3.SDLTrayMenu.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6442) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-0a6151ab37778b36"></a>[`public static bool Inno.Native.Sdl3.SDLTrayMenu.operator ==(Inno.Native.Sdl3.SDLTrayMenu left, Inno.Native.Sdl3.SDLTrayMenu right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6438) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6330751290811920"></a>[`public static Inno.Native.Sdl3.SDLTrayMenu.implicit operator Inno.Native.Sdl3.SDLTrayMenu(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6437) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-79e6bbb98763cc26"></a>[`public static bool Inno.Native.Sdl3.SDLTrayMenu.operator !=(Inno.Native.Sdl3.SDLTrayMenu left, Inno.Native.Sdl3.SDLTrayMenu right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6439) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-99d1c9033fada85d"></a>[`public nint Inno.Native.Sdl3.SDLTrayMenu.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6433) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f8ab7a1de41cf41b"></a>[`public bool Inno.Native.Sdl3.SDLTrayMenu.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6434) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7db460b586354a65"></a>[`public static Inno.Native.Sdl3.SDLTrayMenu Inno.Native.Sdl3.SDLTrayMenu.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6435) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7395e6b46f877304"></a>[`public readonly struct Inno.Native.Sdl3.SDLTrayMenu : System.IEquatable<Inno.Native.Sdl3.SDLTrayMenu>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6426) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLUserEvent</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLUserEvent d3d6e91a4e312bf4721bbd044b645adeed5570039a0fc43dd885c259a056bc86 -->
<!-- api: F:Inno.Native.Sdl3.SDLUserEvent.Type 0ef3d5eccf7246d9bb01c401cf615cda21f41d91ca0e2b47f27b34875342ac1f -->
<!-- api: F:Inno.Native.Sdl3.SDLUserEvent.Reserved 9ea9244bd76b996b47ce6d37fb52c6a8d0194ce98ddbe1ee605229ce9a1326c9 -->
<!-- api: F:Inno.Native.Sdl3.SDLUserEvent.Timestamp 4e9575a5de03bf7e98739be7ee13d18349800de8f8260008e1fb561072d525de -->
<!-- api: F:Inno.Native.Sdl3.SDLUserEvent.WindowId 6ac1f813f5694310c095a4bd3204aced5c15bb1d6f7b368348184816c35ab340 -->
<!-- api: F:Inno.Native.Sdl3.SDLUserEvent.Code e4d0616c1e8fd58792f011ae9e7c197aabfdb3ff5fddf4d4beb6ea3e7146008a -->
<!-- api: F:Inno.Native.Sdl3.SDLUserEvent.Data1 abfdb98447cf007380d6629d59f791fcd68ce6fb32a645840b1faa608f01eccb -->
<!-- api: F:Inno.Native.Sdl3.SDLUserEvent.Data2 23bdfd54355511aa05899dd3e151b7dc5f46133b86a90e4c99c93ae46b3dda15 -->
<!-- api: M:Inno.Native.Sdl3.SDLUserEvent.#ctor(System.UInt32,System.UInt32,System.UInt64,System.UInt32,System.Int32,System.Void*,System.Void*) eb60e68e65d8987ce444c9135e157e864f4f0ada0e638be2d679b14067a4cd80 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7c99942abeb1ffe6"></a>[`public int Inno.Native.Sdl3.SDLUserEvent.Code`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4054) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-82f21ae9ee7555ec"></a>[`public void* Inno.Native.Sdl3.SDLUserEvent.Data1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4055) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-19aa8abf45c75537"></a>[`public void* Inno.Native.Sdl3.SDLUserEvent.Data2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4056) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f60bb7c8b4c4c60c"></a>[`public uint Inno.Native.Sdl3.SDLUserEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4051) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc74069bebcb1e9d"></a>[`public ulong Inno.Native.Sdl3.SDLUserEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4052) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f252a786b474fb43"></a>[`public uint Inno.Native.Sdl3.SDLUserEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4050) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-88fe81c29b63027b"></a>[`public uint Inno.Native.Sdl3.SDLUserEvent.WindowId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4053) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e9c52fb055176524"></a>[`public Inno.Native.Sdl3.SDLUserEvent.SDLUserEvent(uint type = 0, uint reserved = 0, ulong timestamp = 0, uint windowID = 0, int code = 0, void* data1 = null, void* data2 = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4057) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b720615ec1faa42b"></a>[`public struct Inno.Native.Sdl3.SDLUserEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4048) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLVertex</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLVertex dfffbcd46c5cf9d954d285d742b9cc1245775f60f76160f9503c993e965dfd7d -->
<!-- api: F:Inno.Native.Sdl3.SDLVertex.Position 7ab884954e6bed7a15023ca455e49251beb47e81bbad7f4a4ad5f197010a55b1 -->
<!-- api: F:Inno.Native.Sdl3.SDLVertex.Color 8e54c3fcfa131872d76eb521c48f5584a8a128dbd3ba884952f22e17b132364c -->
<!-- api: F:Inno.Native.Sdl3.SDLVertex.TexCoord c6c64927106cee6155afc5023d88b985a05828c81fc2b196b721fad65f60070c -->
<!-- api: M:Inno.Native.Sdl3.SDLVertex.#ctor(Inno.Native.Sdl3.SDLFPoint,Inno.Native.Sdl3.SDLFColor,Inno.Native.Sdl3.SDLFPoint) 34569c0e5ab9bd01a65d344a795f689c2bf334ee6cef0ba2f68f57cd8dc957a3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-bfceeb4dafd2990f"></a>[`public Inno.Native.Sdl3.SDLFColor Inno.Native.Sdl3.SDLVertex.Color`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6158) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f9d602c7676d8cc7"></a>[`public Inno.Native.Sdl3.SDLFPoint Inno.Native.Sdl3.SDLVertex.Position`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6157) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27095c7cb8f4ab4f"></a>[`public Inno.Native.Sdl3.SDLFPoint Inno.Native.Sdl3.SDLVertex.TexCoord`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6159) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-adcf5f032bbe8a17"></a>[`public Inno.Native.Sdl3.SDLVertex.SDLVertex(Inno.Native.Sdl3.SDLFPoint position = default(Inno.Native.Sdl3.SDLFPoint), Inno.Native.Sdl3.SDLFColor color = default(Inno.Native.Sdl3.SDLFColor), Inno.Native.Sdl3.SDLFPoint tex_coord = default(Inno.Native.Sdl3.SDLFPoint))`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6160) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-40988840c6952b04"></a>[`public struct Inno.Native.Sdl3.SDLVertex`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6155) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLVertexPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLVertexPtr fa961975cf8ee990bb40f13bccde862ad1bbd5388727d6d1df25becc82d935b8 -->
<!-- api: M:Inno.Native.Sdl3.SDLVertexPtr.#ctor(Inno.Native.Sdl3.SDLVertex*) e8b2d2a958af739c2c1fd15151078b79be92848fabba939790ce706ffbe3cfb5 -->
<!-- api: F:Inno.Native.Sdl3.SDLVertexPtr.Handle d23a5c41243f8e10fe1c3b272481936b77ba17f30e8d20457c17f900433c756e -->
<!-- api: P:Inno.Native.Sdl3.SDLVertexPtr.IsNull b52a9f6303e4b3cee9772d60f070e59b4e017e16389385a59d6380954c722518 -->
<!-- api: P:Inno.Native.Sdl3.SDLVertexPtr.Null 74ea18609c3876d3bc3359fabd854c89eb713732699b11d283e56d71f6d59739 -->
<!-- api: P:Inno.Native.Sdl3.SDLVertexPtr.Item(System.Int32) 494e986d54fc5a21e1a75c77296c3b43a4a4451008c8b4553588df18b4fcf07c -->
<!-- api: M:Inno.Native.Sdl3.SDLVertexPtr.op_Implicit(Inno.Native.Sdl3.SDLVertex*)~Inno.Native.Sdl3.SDLVertexPtr 5ace43fa904f71615f6dfb512261176dd2dee7c2cfab7a29968d9e47504fb3b1 -->
<!-- api: M:Inno.Native.Sdl3.SDLVertexPtr.op_Implicit(Inno.Native.Sdl3.SDLVertexPtr)~Inno.Native.Sdl3.SDLVertex* 31b0b40b4a587a7f40ce551d466844235099c9da69232827876e791ff37ef900 -->
<!-- api: M:Inno.Native.Sdl3.SDLVertexPtr.op_Equality(Inno.Native.Sdl3.SDLVertexPtr,Inno.Native.Sdl3.SDLVertexPtr) bee942f475d5867b160ac48a91fa369cf420afb90b2f47e294f34631756967af -->
<!-- api: M:Inno.Native.Sdl3.SDLVertexPtr.op_Inequality(Inno.Native.Sdl3.SDLVertexPtr,Inno.Native.Sdl3.SDLVertexPtr) aebd0c6ec49e32a6dd9b1a2b7899f3c8ddd139008c88373a23305bea260ff095 -->
<!-- api: M:Inno.Native.Sdl3.SDLVertexPtr.Equals(Inno.Native.Sdl3.SDLVertexPtr) deb8184a68f79c732bc689cbd02d7a1d0bb07c689afdc9b428ce3efea183603f -->
<!-- api: M:Inno.Native.Sdl3.SDLVertexPtr.Equals(System.Object) e206db77a9572d3cd5d10b835167f5fc61e9af0981db390df527b75faa338486 -->
<!-- api: M:Inno.Native.Sdl3.SDLVertexPtr.GetHashCode 9cf2ade3f2b7344c05f775bd1fa92c8b11ac92022067cfae0cace6cfdd36d095 -->
<!-- api: P:Inno.Native.Sdl3.SDLVertexPtr.Position f5cc90a0a274050dfa7198e3d9610ffa31628eb8b1f5e1264c612ad5fd71004a -->
<!-- api: P:Inno.Native.Sdl3.SDLVertexPtr.Color 44d59f931c9a65193cefe37e7a83bc1d424b72598396fa7f33c77c5effc48f87 -->
<!-- api: P:Inno.Native.Sdl3.SDLVertexPtr.TexCoord e4884b47c2a6445a3fc6aa8ea0260f16525a335a693522a8a560fda69d336a10 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-906e165f21419df9"></a>[`public Inno.Native.Sdl3.SDLVertex* Inno.Native.Sdl3.SDLVertexPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9725) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9c938c934c2eeaa5"></a>[`public Inno.Native.Sdl3.SDLVertexPtr.SDLVertexPtr(Inno.Native.Sdl3.SDLVertex* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9720) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7db0d76c612e8cbf"></a>[`public bool Inno.Native.Sdl3.SDLVertexPtr.Equals(Inno.Native.Sdl3.SDLVertexPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9735) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-4f8061caf1a43537"></a>[`public override bool Inno.Native.Sdl3.SDLVertexPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9736) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-8d4559e173e22c05"></a>[`public override int Inno.Native.Sdl3.SDLVertexPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9737) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-7d9bac2de64a3261"></a>[`public static bool Inno.Native.Sdl3.SDLVertexPtr.operator ==(Inno.Native.Sdl3.SDLVertexPtr left, Inno.Native.Sdl3.SDLVertexPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9733) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2b5b888140364a10"></a>[`public static Inno.Native.Sdl3.SDLVertexPtr.implicit operator Inno.Native.Sdl3.SDLVertexPtr(Inno.Native.Sdl3.SDLVertex* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9731) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-21b6d4d96cd2645f"></a>[`public static Inno.Native.Sdl3.SDLVertexPtr.implicit operator Inno.Native.Sdl3.SDLVertex*(Inno.Native.Sdl3.SDLVertexPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9732) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2cdc6f8f7361dea4"></a>[`public static bool Inno.Native.Sdl3.SDLVertexPtr.operator !=(Inno.Native.Sdl3.SDLVertexPtr left, Inno.Native.Sdl3.SDLVertexPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9734) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3acc7de0e093e9b2"></a>[`public ref Inno.Native.Sdl3.SDLFColor Inno.Native.Sdl3.SDLVertexPtr.Color { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9740) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ec0849cc2e963a5d"></a>[`public bool Inno.Native.Sdl3.SDLVertexPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9726) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fdeb572ef8e86dc7"></a>[`public Inno.Native.Sdl3.SDLVertex Inno.Native.Sdl3.SDLVertexPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9729) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2aca8b1fb2955906"></a>[`public static Inno.Native.Sdl3.SDLVertexPtr Inno.Native.Sdl3.SDLVertexPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9727) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6485f4b442838101"></a>[`public ref Inno.Native.Sdl3.SDLFPoint Inno.Native.Sdl3.SDLVertexPtr.Position { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9739) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3e447cfff06d1684"></a>[`public ref Inno.Native.Sdl3.SDLFPoint Inno.Native.Sdl3.SDLVertexPtr.TexCoord { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9741) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7da8a95a4e389c25"></a>[`public struct Inno.Native.Sdl3.SDLVertexPtr : System.IEquatable<Inno.Native.Sdl3.SDLVertexPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9718) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLVirtualJoystickDesc</code> · 30 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLVirtualJoystickDesc 4f30cd3f6ca390f38700a1568904e1e3d699ef537a65a8a2a2046abc3c2f46d3 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Version c1c935c8ae552ed51745b772b3f09f19a4ef42e75fa8897e824b32aea9783a65 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Type e8eccf6c8cc29925923593ba6f730729612131c66c04722c574577f061bf45f1 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Padding 97b7a4c58a2c61fefc804a44fe265c9cffe9b8cfdd58cd2a9c1737116f4c6bbf -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.VendorId 76ec4491f4b3973c94670c0fc1dfc07a4621badc2e3e232be68f7a17e82a2fef -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.ProductId c69f03febb6fd81174e7d47de545f8e8d175756a19391e86468b0c4a0fe3029f -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Naxes d51f5a598647ac5c4476bd0f9b3e387e1fae86c44fe1136f19ac24d436635b21 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Nbuttons cf908ed1c06c320010a430edd3913a49a04bf72c6e8ec64f51d53710f8313774 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Nballs 9a1525e8576ea9f59977c495cb32f1f61fd9ead83e1fa1851168247e5d8dcac4 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Nhats ea2575b4d6a47523a7b19870341b0eb371444b589d0aab87c631d2c114d40f70 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Ntouchpads c5c540a3f43330c212a5272fd363c33d6d8441a99866b593bb0e9c22a836428b -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Nsensors 49b14d54684d1578fc273de656a46312e2a9a393333b59bcf153092ffc2dd9d6 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Padding2_0 d81e559418062f6d7a0bcfab0d1496e7e55e737e17e35bb01dc9e45d721a8684 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Padding2_1 f7cd11693d1cc40980bb0992f0ebb580dffa9ec20b883051d2736aafd3899d0c -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Padding2 867c059f1aa2ecd9bf9b48676c011ace047438c5b89f2dd831ffae4e60fe7e79 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.ButtonMask 0c19346d6212cefc8bbc710de363179db8e95bd9248d07a27dc7ddc446c119c2 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.AxisMask 68b6eb9f72f4a825b1436935078fe12a076252fbef7e26908bb70f2a3f1fbca6 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Name d1bd26e266cf7b275d3afd26f37ba2c14314fb490bf7c8832f8f80d08605e883 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Touchpads 41a3e53030c844bf4250d3bfea7319b9366fa8589aabe6804ae72f47beeafd47 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Sensors 5cca124e2a5039cd1e57f6fd61126af21eda81ad561617cc551573d674601349 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Userdata bb60e690acbfae13c4b177ff7d3cc3c5bb6d0e5e6540f27f3c15563bd849b995 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Update 10667515a2269ab7608f1da9a48050e5c14ec53cafdea34709ba8b30f8ac2e40 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.SetPlayerIndex dd38e11a1f52e927e04898100b5416db665192826e096ee66d8e0366597480d0 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Rumble 5f807c0f9b91556242b232edf062481be68d3effa2e4c7c404933ab7dd8730af -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.RumbleTriggers eecc8c2d4db66357572b9e1157c61918631d7a127841753bd815c15bf5149718 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.SetLED 0926272d6abf855fafa7ce0b9089ee94489ce20a215ea71f8fe6e495a01461e4 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.SendEffect edf0ccd44ed4f13720655b561d4825f5d7ad280efc8cbe761c387d0429dcbd5a -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.SetSensorsEnabled 188da959b0d646f9203c25dc597877d45cc247110fac76bb2dcf42ac6b9964ab -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDesc.Cleanup 056175661828b7c1ca016d4739b5acf335c9f8df17497a6e6b88e3dadac93418 -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickDesc.#ctor(System.UInt32,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16,System.UInt16*,System.UInt32,System.UInt32,System.Byte*,Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc*,Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc*,System.Void*,,,,,,,,) 6820d9e02d7f1096e9dc7394bdd91347807b11a3c13b3fe4df11dc3e270df269 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-02ad3b55ba0ea897"></a>[`public uint Inno.Native.Sdl3.SDLVirtualJoystickDesc.AxisMask`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2842) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0452ad426228b1b3"></a>[`public uint Inno.Native.Sdl3.SDLVirtualJoystickDesc.ButtonMask`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2841) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-da16c04f5cc47294"></a>[`public delegate* unmanaged[Cdecl]<void*, void> Inno.Native.Sdl3.SDLVirtualJoystickDesc.Cleanup`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2854) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39283cf6fea2d4f9"></a>[`public byte* Inno.Native.Sdl3.SDLVirtualJoystickDesc.Name`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2843) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ac338cb0fa0ad9f8"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickDesc.Naxes`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2831) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1f7ff56331f5f760"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickDesc.Nballs`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2833) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c6116a9cbf2bd43f"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickDesc.Nbuttons`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2832) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07a35b5675ecc7ff"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickDesc.Nhats`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2834) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-506df0913508e902"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickDesc.Nsensors`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2836) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-43f88c4650a58f97"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickDesc.Ntouchpads`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2835) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-55f240bc7f5c0702"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickDesc.Padding`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2828) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fa684837e69c8edf"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickDesc.Padding2_0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2837) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bede822fc7721fa7"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickDesc.Padding2_1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2838) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b0fe5a3fd33afa41"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickDesc.ProductId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2830) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f7093c2c57921f65"></a>[`public delegate* unmanaged[Cdecl]<void*, ushort, ushort, byte> Inno.Native.Sdl3.SDLVirtualJoystickDesc.Rumble`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2849) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-254dab5e36e08dff"></a>[`public delegate* unmanaged[Cdecl]<void*, ushort, ushort, byte> Inno.Native.Sdl3.SDLVirtualJoystickDesc.RumbleTriggers`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2850) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-322e24547ebcad9f"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, int, byte> Inno.Native.Sdl3.SDLVirtualJoystickDesc.SendEffect`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2852) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-814b49f460a4ba54"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc* Inno.Native.Sdl3.SDLVirtualJoystickDesc.Sensors`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2845) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d1d732d60855357c"></a>[`public delegate* unmanaged[Cdecl]<void*, byte, byte, byte, byte> Inno.Native.Sdl3.SDLVirtualJoystickDesc.SetLED`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2851) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9b3f382974aeb80d"></a>[`public delegate* unmanaged[Cdecl]<void*, int, void> Inno.Native.Sdl3.SDLVirtualJoystickDesc.SetPlayerIndex`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2848) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-91d2f639296db824"></a>[`public delegate* unmanaged[Cdecl]<void*, byte, byte> Inno.Native.Sdl3.SDLVirtualJoystickDesc.SetSensorsEnabled`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2853) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccd6ad3f08a671db"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc* Inno.Native.Sdl3.SDLVirtualJoystickDesc.Touchpads`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2844) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d7e3f8acce30d604"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickDesc.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2827) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-63f61a26e53216ae"></a>[`public delegate* unmanaged[Cdecl]<void*, void> Inno.Native.Sdl3.SDLVirtualJoystickDesc.Update`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2847) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-33f5ea0c28abb9b9"></a>[`public void* Inno.Native.Sdl3.SDLVirtualJoystickDesc.Userdata`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2846) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9526670416badc41"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickDesc.VendorId`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2829) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1652a7b7fce7f2f9"></a>[`public uint Inno.Native.Sdl3.SDLVirtualJoystickDesc.Version`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2826) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f1b5acabb6cb979a"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickDesc.SDLVirtualJoystickDesc(uint version = 0, ushort type = 0, ushort padding = 0, ushort vendor_id = 0, ushort product_id = 0, ushort naxes = 0, ushort nbuttons = 0, ushort nballs = 0, ushort nhats = 0, ushort ntouchpads = 0, ushort nsensors = 0, ushort* padding2 = null, uint button_mask = 0, uint axis_mask = 0, byte* name = null, Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc* touchpads = null, Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc* sensors = null, void* userdata = null, delegate* unmanaged[Cdecl]<void*, void> update = default(delegate* unmanaged[Cdecl]<void*, void>), delegate* unmanaged[Cdecl]<void*, int, void> setPlayerIndex = default(delegate* unmanaged[Cdecl]<void*, int, void>), delegate* unmanaged[Cdecl]<void*, ushort, ushort, byte> rumble = default(delegate* unmanaged[Cdecl]<void*, ushort, ushort, byte>), delegate* unmanaged[Cdecl]<void*, ushort, ushort, byte> rumbleTriggers = default(delegate* unmanaged[Cdecl]<void*, ushort, ushort, byte>), delegate* unmanaged[Cdecl]<void*, byte, byte, byte, byte> setLED = default(delegate* unmanaged[Cdecl]<void*, byte, byte, byte, byte>), delegate* unmanaged[Cdecl]<void*, void*, int, byte> sendEffect = default(delegate* unmanaged[Cdecl]<void*, void*, int, byte>), delegate* unmanaged[Cdecl]<void*, byte, byte> setSensorsEnabled = default(delegate* unmanaged[Cdecl]<void*, byte, byte>), delegate* unmanaged[Cdecl]<void*, void> cleanup = default(delegate* unmanaged[Cdecl]<void*, void>))`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2855) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b2fc9802b64d9a4a"></a>[`public System.Span<ushort> Inno.Native.Sdl3.SDLVirtualJoystickDesc.Padding2 { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2839) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4b454f7de9fa867a"></a>[`public struct Inno.Native.Sdl3.SDLVirtualJoystickDesc`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2824) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLVirtualJoystickDescPtr</code> · 39 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr 39a0450addb18eb468b5641ffe366d7469268abe7447f2f939c2487b851ab62a -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.#ctor(Inno.Native.Sdl3.SDLVirtualJoystickDesc*) e3d96aacc213f9e0cc1722f0bafc19501d82e44484718c2384822fbb97f9ca69 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Handle 02d9665ac41a2381d28a4545ff9e2dee35e4944de5b0cf7702d9ab5243f41ebe -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.IsNull dd607636d95dc1aa906b8074f6914ca0f398ca5bbbddb3e3d08a5ee2fd794957 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Null 558e621d9658a897806d7023c735b1e8eb33e447dfe15e378468bc58e0b2ed28 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Item(System.Int32) 6a79f531918e50ecffc59a002457ca4e882277e22399d0c2a6e47bda40505d91 -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.op_Implicit(Inno.Native.Sdl3.SDLVirtualJoystickDesc*)~Inno.Native.Sdl3.SDLVirtualJoystickDescPtr eccb47649759753388379402319d25f5b01aadbe8c9bceed32988bbb002d273e -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.op_Implicit(Inno.Native.Sdl3.SDLVirtualJoystickDescPtr)~Inno.Native.Sdl3.SDLVirtualJoystickDesc* 21e3cb10be042f20914427a24c64895114cdf1836351436f15a427c451b5cc0e -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.op_Equality(Inno.Native.Sdl3.SDLVirtualJoystickDescPtr,Inno.Native.Sdl3.SDLVirtualJoystickDescPtr) eeee80071149bfc84a5a072c346d4642b7c2ba6d6a02518ff0ebcd658e2e1547 -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.op_Inequality(Inno.Native.Sdl3.SDLVirtualJoystickDescPtr,Inno.Native.Sdl3.SDLVirtualJoystickDescPtr) d1de59502a9634a3c335bf6de31e6e6abee71f168fd1e2624d0a86d7ad05b466 -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Equals(Inno.Native.Sdl3.SDLVirtualJoystickDescPtr) f97128038a840139210b7542e04cff3d0d59d36cfa0e11bfe661c503d65ade9d -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Equals(System.Object) f6f685eae0f4a1e3bdf97df69a66dec5a7d03d841cf6c34bad50e155c941feaa -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.GetHashCode 01b705b93fb212c06d69f40e32ca9f74093a4f80eb12e17eae167f2ce8f43d7f -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Version 1ddb76626b60928dc5a85d58abec1b0f80285029f41bc8d78e4a693d42dba861 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Type b69d0d646030b047f9339859c95655872405b24f24c235715f9b178c73adb7f2 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Padding fe99dd48456739b7d14fd16e6e239f3155a6fe405eee81dda997e86427315569 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.VendorId 350415bb054787afadd16f0be39cb7f4812f78be053984c7b54a6650e8f8595d -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.ProductId fc1455e7ae2a7857f8add5e2cde1169047fd8d7dc576e2a2429fb1067c65fd95 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Naxes 19af1bc93119bdb3e3f85d1dc7c44af6f431d09108c91856875855cf7fd89300 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Nbuttons 468b78cf4a6d2f51886eb48300ce36aa85a6fbac231c4b0558d5da57b2d19a0b -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Nballs 3d141422e5665564046a3f1a891be01332355cc7259544483265f48afc75a069 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Nhats 9c161d337fb6d1117a8bcef4a32233e887658f87db09f91b5b5360882d170b58 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Ntouchpads 2f314aaafef4abb481136c0561be1e903afba3ccbdd9481c2e609a3c5cd0bfea -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Nsensors 53af4b6c6ad3dd8dea863bc2d626e8646fdba3f7ff6a1a098eb945b71eca9b0b -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Padding2 1475fb08a5e39ba614e88bc78409536b778f959f924c47ba2faf660d758731c4 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.ButtonMask 7f46fddf7780473040da1694305a2f370506b10a63a0471ae2a0297572b23bbb -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.AxisMask e429c02985fc0732f31b00668ca09dfb7a1f4c6881db00e66331019836eebb58 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Name f552ce182251275fa1494dd41118928be22d1cb1fdbd5cac89e3b85693fcf127 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Touchpads aaaf814994535e8e347cd7bd4bf54d9c2a7c512d129cdc9ae06b1da44deb1f3a -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Sensors fd6d787b6606da3803f881616a127b77f7f17ad0724e9364382bf2cb53db51f5 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Userdata 61344e1a5fad758412843e1a238acfab18e1d63e9d093c4d8584eb31bfb300db -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Update ab58a0fd9d74cc5355676ca560c3a08f0f137688591267f898e52ee477a191d3 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.SetPlayerIndex 24d85ab9e4ebb2c1a986d4a497707c44e70c360e1ab8ccd18bc29fe1ef25bdb6 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Rumble 08651661284438630bd66bc21f06575a46512571821162fadbf4b5693420b7ea -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.RumbleTriggers 5b890a1986947bf0229e9a924a53ee1b4fde44860a96645b71a370e469a1496a -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.SetLED 88f8b61406f70c558adaad88c084d2bdcc4e34a4d5c97c167b04ac246ceb93e1 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.SendEffect 3460c4e0c10565a6f220f39c838c584cc4cfc49877c21a4851eddc6ee492c1aa -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.SetSensorsEnabled eac00dde952fd77a8ccc8fe2d65dc02f0833170aed5d4ac6e8538fe6266a4b93 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Cleanup 88d46bc6b1d4cf1c8fb52466a25a323a38df99528a43876a535b929792aa5e09 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-93beca70164ebbed"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickDesc* Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9752) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0cf42222222ccd70"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.SDLVirtualJoystickDescPtr(Inno.Native.Sdl3.SDLVirtualJoystickDesc* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9747) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0ee2179e056a57c7"></a>[`public bool Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Equals(Inno.Native.Sdl3.SDLVirtualJoystickDescPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9762) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-f84de1f86f633862"></a>[`public override bool Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9763) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-bd3b48f8ea4a8fe8"></a>[`public override int Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9764) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-37c3cb2eb7afca6f"></a>[`public static bool Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.operator ==(Inno.Native.Sdl3.SDLVirtualJoystickDescPtr left, Inno.Native.Sdl3.SDLVirtualJoystickDescPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9760) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c25e575a2ef4ca6c"></a>[`public static Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.implicit operator Inno.Native.Sdl3.SDLVirtualJoystickDescPtr(Inno.Native.Sdl3.SDLVirtualJoystickDesc* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9758) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-acd601c57cf4d176"></a>[`public static Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.implicit operator Inno.Native.Sdl3.SDLVirtualJoystickDesc*(Inno.Native.Sdl3.SDLVirtualJoystickDescPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9759) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ef059604a479788f"></a>[`public static bool Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.operator !=(Inno.Native.Sdl3.SDLVirtualJoystickDescPtr left, Inno.Native.Sdl3.SDLVirtualJoystickDescPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9761) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2bc4989fae12e187"></a>[`public ref uint Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.AxisMask { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9779) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bbaed84c2bd35f41"></a>[`public ref uint Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.ButtonMask { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9778) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4227c02078540501"></a>[`public delegate* unmanaged[Cdecl]<void*, void> Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Cleanup { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9791) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-888ad7be1790a338"></a>[`public bool Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9753) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cf43061cae91d68c"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickDesc Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9756) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ab37a97a1dfc1b86"></a>[`public byte* Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Name { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9780) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-92145d4a709c2879"></a>[`public ref ushort Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Naxes { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9771) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-acb0d4efae8b4d9c"></a>[`public ref ushort Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Nballs { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9773) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2c26d5b96b2088ba"></a>[`public ref ushort Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Nbuttons { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9772) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-36d428a49027b113"></a>[`public ref ushort Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Nhats { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9774) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-60426a2f64754308"></a>[`public ref ushort Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Nsensors { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9776) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-38c61bf321c02a45"></a>[`public ref ushort Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Ntouchpads { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9775) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cfa1ba4ec8ef24c6"></a>[`public static Inno.Native.Sdl3.SDLVirtualJoystickDescPtr Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9754) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3e3078938958ccb6"></a>[`public ref ushort Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Padding { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9768) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cf5e865402801cc8"></a>[`public System.Span<ushort> Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Padding2 { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9777) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b39f1685bdc657c5"></a>[`public ref ushort Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.ProductId { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9770) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d7987c85fe4190ae"></a>[`public delegate* unmanaged[Cdecl]<void*, ushort, ushort, byte> Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Rumble { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9786) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-973354d1a7277e71"></a>[`public delegate* unmanaged[Cdecl]<void*, ushort, ushort, byte> Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.RumbleTriggers { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9787) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d71e909188d90bdc"></a>[`public delegate* unmanaged[Cdecl]<void*, void*, int, byte> Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.SendEffect { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9789) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-da424e27de424170"></a>[`public ref Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Sensors { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9782) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-772e0ef010256d92"></a>[`public delegate* unmanaged[Cdecl]<void*, byte, byte, byte, byte> Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.SetLED { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9788) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a0a5ad159f8ec92d"></a>[`public delegate* unmanaged[Cdecl]<void*, int, void> Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.SetPlayerIndex { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9785) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c41e1c7ce4677f00"></a>[`public delegate* unmanaged[Cdecl]<void*, byte, byte> Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.SetSensorsEnabled { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9790) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-26901a9e9d959eac"></a>[`public ref Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Touchpads { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9781) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d47ea9982dddf19c"></a>[`public ref ushort Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Type { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9767) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-769151b28e2cd5d6"></a>[`public delegate* unmanaged[Cdecl]<void*, void> Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Update { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9784) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-19da728a0a8150b6"></a>[`public void* Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Userdata { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9783) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ef5daa55de9f6c0e"></a>[`public ref ushort Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.VendorId { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9769) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1b3b13a8223f781c"></a>[`public ref uint Inno.Native.Sdl3.SDLVirtualJoystickDescPtr.Version { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9766) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-da3b7748e7bbe9c5"></a>[`public struct Inno.Native.Sdl3.SDLVirtualJoystickDescPtr : System.IEquatable<Inno.Native.Sdl3.SDLVirtualJoystickDescPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9745) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc ac3700597671f9db75501e32bbb16256787ec5a1234f224decbd677ef2d6d610 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc.Type 48fcef9ba6c45733fdab5deeadd3679c58c3c1319e710f20413b53446929947d -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc.Rate 4945284a7fe230d5cb987e3f6c19310cf69fa61ce8e125f52ac0d748bb19f072 -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc.#ctor(Inno.Native.Sdl3.SDLSensorType,System.Single) 470d23768b9b22767ebc7aa59781a68188d266d143daa1f846a6be595d6c1bae -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a525c40ac530c67c"></a>[`public float Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc.Rate`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2812) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c762cd2588444410"></a>[`public Inno.Native.Sdl3.SDLSensorType Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2811) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1eae2fdf3b3a836d"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc.SDLVirtualJoystickSensorDesc(Inno.Native.Sdl3.SDLSensorType type = Inno.Native.Sdl3.SDLSensorType.Unknown, float rate = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2813) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4dad08bc3399685f"></a>[`public struct Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2809) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr 2dc80bd88c36200be542a35db63bee88f34d3331753ceb66e361828e4275b4c4 -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.#ctor(Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc*) 75aa9b8219b6f79989f1f297fb2a6ac3fa7d1e35b2c76b46727343a4b9c23a1a -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.Handle bea781df300523dcc52633148f865133ca8298c5f2ff9dbba64b28c187bae420 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.IsNull b4cd694c2893de5b6e4fb1995c368e50771adbc575b07f1daa75ebe5f181cd30 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.Null 8f5f45f0d711a2b49a9f7aeb478197a1f8bd5bcd20d8ed98ad3042e7409f196d -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.Item(System.Int32) 1d14f1f141f33df267529f19613662667d3ec66f724bfa0a29f3c28e9728a58a -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.op_Implicit(Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc*)~Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr 4d993851ff7b3acd038b3462aafc1b91d97cc1e3077a5dcea1d2cf3a969025ad -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.op_Implicit(Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr)~Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc* eea34db223843ca3f70b1f4a8e852bc85029511f0d2b923a650ba5dc783331ff -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.op_Equality(Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr,Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr) 0a95e7e5ede96558c4ed3b8914cfcf1edc84a6f09f431c42928e1eaa8462ec28 -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.op_Inequality(Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr,Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr) 1ea5c5ec3b70922aa799c0798d9a1e69abfe5cf5f85de388415bfb782ab9d2ec -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.Equals(Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr) b1142252f599cf0a700cc1b0effb9a1ecf538a67cb4aa0a97be6095c9b7ba7d2 -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.Equals(System.Object) 73405fcc23b16d661973ccf607ce3e5abadd0fd2a664aeb36fc6704a6429f12f -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.GetHashCode 6ac216b919757449bb2822c70ab32b40eea0fe3d05d6b02437d3f7e60ea4aa8a -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.Type 816b0c9bee53d4a371321a330fd9664d637ba5bd69e9f0a091d49d24364be3c1 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.Rate 93548f3b30b8d899227066dfd601d76eb959ee29f835f744a605fd0dbb719ada -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dc5a47f4f512e2a8"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc* Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9802) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1bbdc15f182e125f"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.SDLVirtualJoystickSensorDescPtr(Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9797) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d89806aa033dd58e"></a>[`public bool Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.Equals(Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9812) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-8d1890da638a604f"></a>[`public override bool Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9813) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-a4e6e646b2fee14f"></a>[`public override int Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9814) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-4443813759ace294"></a>[`public static bool Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.operator ==(Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr left, Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9810) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-02ba609546ad9e43"></a>[`public static Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.implicit operator Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr(Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9808) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c3deb6ca4915ad8d"></a>[`public static Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.implicit operator Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc*(Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9809) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0fedcb7076ff115a"></a>[`public static bool Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.operator !=(Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr left, Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9811) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-42cd420ca4e3b2ed"></a>[`public bool Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9803) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5c716aaf420934ef"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickSensorDesc Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9806) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3f768fd855817999"></a>[`public static Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9804) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b65540399548e2f2"></a>[`public ref float Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.Rate { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9817) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cf35e765a5bb4037"></a>[`public ref Inno.Native.Sdl3.SDLSensorType Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr.Type { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9816) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-56523b6a802cc45a"></a>[`public struct Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr : System.IEquatable<Inno.Native.Sdl3.SDLVirtualJoystickSensorDescPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9795) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc 276d1bacbd90174798acf1edfff6d369e9eb7c646cdf11f3ce64c005e8b1b0b4 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc.Nfingers 79147e08e9f383f5949ce15698a57f144cb4dabdec70158aa897b996f84d7fc8 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc.Padding_0 ba09f2c7223066292d1c2265f52d4599e8fd11237c58857e218f8c74aea9103b -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc.Padding_1 a08b7a9741eba4fb91cad6d039e37279b64807edd7a999e43ec1c9ebe4315344 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc.Padding_2 09b41870c0df7bfe6f735653b605ce97c4cb3f1b14ea01fa1b323153e603a4ba -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc.Padding 04cc3a78ba49c5b509c7047ff7a2cafa8a34dcfe6d49c5005c46c0e9da5ab435 -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc.#ctor(System.UInt16,System.UInt16*) 308390aaad7691d6f7648fcbfd78b92618e3dd9bc7e0a22bab16092407ac2fe4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e57d95fcb460148f"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc.Nfingers`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2787) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d2988001f6690875"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc.Padding_0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2788) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-effeb3c8582b358a"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc.Padding_1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2789) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-82948ff6dcea7b7a"></a>[`public ushort Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc.Padding_2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2790) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6fb6c7426a4e6137"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc.SDLVirtualJoystickTouchpadDesc(ushort nfingers = 0, ushort* padding = null)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2793) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d18d96b1e7bae8e6"></a>[`public System.Span<ushort> Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc.Padding { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2791) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9ab974c822d53422"></a>[`public struct Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2785) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr acc589ee11ab0f5ecc7a5ea04d7626b748130ea920489cf9cde77c39ae38ff13 -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.#ctor(Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc*) 97e24bab57f222d11f9dc7eac44013c680eb2b65d8fbe81d6595c8eea42b8655 -->
<!-- api: F:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.Handle 8550aefa7649a427e71edda7f83c3b95325fa80ad3e9952e59c37429ce684f02 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.IsNull b020cf3569d14b4add787f7c90d51d112ca12af3c0d1a2211dfd557ea87be12b -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.Null 20aa08e3ead59dca6b9ab855a5dc1f5cb33a81066ccb32244f47b8aa6ee2bb69 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.Item(System.Int32) 4c9f3e393e55bf9727f5b0fff412f80bc23d436c1b50627639a946f7c91db48b -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.op_Implicit(Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc*)~Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr c47edc3a72fa7c73c68cc81dcdf219e402987caacf12f515864f0d43f31e91d4 -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.op_Implicit(Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr)~Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc* bb00f1aa5327a3e8e1cf8d74eec9638091ec770d64651c86d67e25764258f6d4 -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.op_Equality(Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr,Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr) c2100fd0796532a3cc7a5b02e4d13385dde11a5c79de8517a26321e16b6ade3c -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.op_Inequality(Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr,Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr) a4d6e0ee15dd5f1d443dca4da061c5c6b235686a09fe014c6c4b3199aadbc182 -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.Equals(Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr) 84c40ae878c8191c839ea266413e2dede40068a5f76353e50cc6475beff2d11d -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.Equals(System.Object) 706d67552d13626366fe4dce8283121eef31052294cba8eced08e04c78377aae -->
<!-- api: M:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.GetHashCode 2eb87b245e092d6734943d88bef904c53b336f9b8fb8c43bf3ad9ea1e0e0fa9c -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.Nfingers 919aecf36c063e9067253bccf346cb7206aebff73940444783eb3aa2b8dc9038 -->
<!-- api: P:Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.Padding 9987eced48dc10dc6b1b075b2f80670a068b03ab45d960431943218d5ac3c665 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a069c5e6fe7a86ad"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc* Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9828) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d438c5112dcf255b"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.SDLVirtualJoystickTouchpadDescPtr(Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9823) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ecabe5325b573154"></a>[`public bool Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.Equals(Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9838) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-eb2e202b9c704e38"></a>[`public override bool Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9839) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-1453ebb93b13b5b5"></a>[`public override int Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9840) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-d160eb6fc2f23543"></a>[`public static bool Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.operator ==(Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr left, Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9836) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-444eeb8ce3643637"></a>[`public static Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.implicit operator Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr(Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9834) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6fe86d076dce8b83"></a>[`public static Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.implicit operator Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc*(Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9835) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-620690e49aeede68"></a>[`public static bool Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.operator !=(Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr left, Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9837) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0f09b921be023988"></a>[`public bool Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9829) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-43eaff039a8e7058"></a>[`public Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDesc Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9832) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2c930d41de33832f"></a>[`public ref ushort Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.Nfingers { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9842) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6ae2984f5ba37b30"></a>[`public static Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9830) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cff643236fae7258"></a>[`public System.Span<ushort> Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr.Padding { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9843) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bf6d5beba8a859d4"></a>[`public struct Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr : System.IEquatable<Inno.Native.Sdl3.SDLVirtualJoystickTouchpadDescPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L9821) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLWindow</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLWindow 7370f08ae86b12c30102992207e7b0cac1de3569982a6c4770410d8ebbca7592 -->
<!-- api: M:Inno.Native.Sdl3.SDLWindow.#ctor(System.IntPtr) c90c6cd58cb46853cdc3af041c05034215686536f005a93c1d5efc9bf01e3e33 -->
<!-- api: P:Inno.Native.Sdl3.SDLWindow.Handle c8ee43f1ecda37982280ab6281c2e4c2b390b9223e626f1ba9dec30ad6608439 -->
<!-- api: P:Inno.Native.Sdl3.SDLWindow.IsNull bc472a556ee69d15e7cdfcfd8da965d2e742cb6c5b105853766606eb815f2214 -->
<!-- api: P:Inno.Native.Sdl3.SDLWindow.Null 2fe2e8c13653ff289a3b6f3ab4588ccd8c4c2d7aab42db3d58b48a94dfdfcfa6 -->
<!-- api: M:Inno.Native.Sdl3.SDLWindow.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLWindow 335f567c20902b482ee2fa1110ff0534824b94146a149c50468dba287c37da05 -->
<!-- api: M:Inno.Native.Sdl3.SDLWindow.op_Equality(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLWindow) 2c5744605fe85c522b32c0eb1cc46fbdc0c9a30f1c8725690f46fdfa296bc52e -->
<!-- api: M:Inno.Native.Sdl3.SDLWindow.op_Inequality(Inno.Native.Sdl3.SDLWindow,Inno.Native.Sdl3.SDLWindow) 0c1e111cbe85ea9aa16bd37e01f155a4577caa5ea593f3d273326a7916b79463 -->
<!-- api: M:Inno.Native.Sdl3.SDLWindow.Equals(Inno.Native.Sdl3.SDLWindow) 87156673a6d80ec183979a4fdb652375fcc03fd3df6b5cab579a5056494bacd5 -->
<!-- api: M:Inno.Native.Sdl3.SDLWindow.Equals(System.Object) 1807ee90d429ebaa877d3c60f487b0e0bb8208060fd40c4ef035db1d6fc50691 -->
<!-- api: M:Inno.Native.Sdl3.SDLWindow.GetHashCode 96bc2b884525588dcb13bc2d90ab9a8d1c4a16b9a678ebb91371c1b00ddfb1c4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2f8ca04b5e44471a"></a>[`public Inno.Native.Sdl3.SDLWindow.SDLWindow(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2628) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5931cea25b041d2b"></a>[`public bool Inno.Native.Sdl3.SDLWindow.Equals(Inno.Native.Sdl3.SDLWindow other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2640) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-c70cf10dcf885eab"></a>[`public override bool Inno.Native.Sdl3.SDLWindow.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2641) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-271dbe7809fd457a"></a>[`public override int Inno.Native.Sdl3.SDLWindow.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2642) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-c942109971efa6e5"></a>[`public static bool Inno.Native.Sdl3.SDLWindow.operator ==(Inno.Native.Sdl3.SDLWindow left, Inno.Native.Sdl3.SDLWindow right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2638) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e652a3b83880742a"></a>[`public static Inno.Native.Sdl3.SDLWindow.implicit operator Inno.Native.Sdl3.SDLWindow(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2637) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ac511a3b9fcf9997"></a>[`public static bool Inno.Native.Sdl3.SDLWindow.operator !=(Inno.Native.Sdl3.SDLWindow left, Inno.Native.Sdl3.SDLWindow right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2639) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4aebfd70a05f906f"></a>[`public nint Inno.Native.Sdl3.SDLWindow.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2633) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1f20e4f0d4423d4d"></a>[`public bool Inno.Native.Sdl3.SDLWindow.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2634) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-590400bdb44f0a8b"></a>[`public static Inno.Native.Sdl3.SDLWindow Inno.Native.Sdl3.SDLWindow.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2635) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c2d73018e05118cf"></a>[`public readonly struct Inno.Native.Sdl3.SDLWindow : System.IEquatable<Inno.Native.Sdl3.SDLWindow>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2626) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLWindowEvent</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLWindowEvent 28faf00c6c5aa40bdcb809cb2969096a641028803b3966d6fa747c646ad2513b -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowEvent.Type c7254834c57a55f5f6322344262d54964cd624a413ad1db6161344375d74d201 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowEvent.Reserved 30cc4af9095aef005be7a6589b7c1d2a3427092f7d7dc94210d082718a66c6a3 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowEvent.Timestamp 269a9a9f83a637df2e58372018bb851164a908a5ee0629e83a41b70d40dd9131 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowEvent.WindowID a624e56c7fa8eeb884fa84e59d1655e6620fc62d6e649511905dc9e66152995f -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowEvent.Data1 0a7e56a3d95d3987543cb486928b4765437738bde991dc561cddbb1c48a37baf -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowEvent.Data2 e8e9a9c047fbb005da58a27c200e4ef3351688de0b0fd7d45493e2facb9c141e -->
<!-- api: M:Inno.Native.Sdl3.SDLWindowEvent.#ctor(Inno.Native.Sdl3.SDLEventType,System.UInt32,System.UInt64,System.UInt32,System.Int32,System.Int32) fe67eb8cce62390192583d14dd41ef9e9f32561dbb931a5b97facab469fa29cf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-25f086f440b6df03"></a>[`public int Inno.Native.Sdl3.SDLWindowEvent.Data1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3117) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ca5bea660d2ad37e"></a>[`public int Inno.Native.Sdl3.SDLWindowEvent.Data2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3118) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7a5bf03119de3e41"></a>[`public uint Inno.Native.Sdl3.SDLWindowEvent.Reserved`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3114) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-934a81b4a9fda024"></a>[`public ulong Inno.Native.Sdl3.SDLWindowEvent.Timestamp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3115) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-36e8e2e863c0e127"></a>[`public Inno.Native.Sdl3.SDLEventType Inno.Native.Sdl3.SDLWindowEvent.Type`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3113) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1585700f564630bb"></a>[`public uint Inno.Native.Sdl3.SDLWindowEvent.WindowID`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3116) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-97f416af7ea6054d"></a>[`public Inno.Native.Sdl3.SDLWindowEvent.SDLWindowEvent(Inno.Native.Sdl3.SDLEventType type = Inno.Native.Sdl3.SDLEventType.First, uint reserved = 0, ulong timestamp = 0, uint windowID = 0, int data1 = 0, int data2 = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3119) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-25be89c5f012715b"></a>[`public struct Inno.Native.Sdl3.SDLWindowEvent`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L3111) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLWindowFlags</code> · 26 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLWindowFlags 5e97c19f7c2ed34f91395786088011155863a2208e49204de35b228ded842fcb -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Fullscreen 8d7c2bcc0b0cd087b1397bf53737d8a9e5f565273960cfb9b4e76be89f711f7a -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Opengl 64282cd217086dbb1f09de34193a24372fbf13f400651d2bd83e616113977285 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Occluded 188de0dc59a9e4e2471d777f91cdf1b32ad7ea2c33021b8fe1512ae0d69f7e23 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Hidden 372a38a4a32d22087fb52b47f48ec3a83544735e5859c99351e9d0d51bc53244 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Borderless c133711eda1597e4519da10461bbe5c619f9bf1f2d650e6aa08e524f503b3aa2 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Resizable 8e3ec0788573f84866edede40a6973a0a2e209430c78f9218bf7cbf3c3f3e082 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Minimized 3b9a1161280633c3efa2fa09f67b80d14daed002e950a65dab73e9b3c2f8e189 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Maximized 22b21dbcebe344686141878849dad20e32a99ed19c956d2213f773dba8e8a168 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.MouseGrabbed 9774211f4b72fe051fc7dfc499beac61f5fff2d993434a25f9f84f171822a008 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.InputFocus ec7496fe1ee3ae531af562635f93d199e8e5d98bcd7bdce8ae3a26afd3668372 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.MouseFocus 76a5666b9433fd606b9d07155809eef9b14fe2b044b09f58a52ffa6dc0031e46 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.External 234b3f183378cd4294e9e3c2494d684942a78feab898f2dddbfd9ce4c022f30c -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Modal 62c770afd0a10d06cc9aa6d246347b793ce9b9373cae36a2477ab18d52b2d83f -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.HighPixelDensity c80027414782e9297c4e8ae4f1be7f7d84265534404987578a427f6c666776c7 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.MouseCapture 6be7cf5e3f080b6fc5a939f4ef8d777b693dc23a99448bf26b9b4f0337d744dd -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.MouseRelativeMode d3580ec8f4be7aae7fbec8c05f8465f1a1a4dc06a0136a873f0f6445bad0e9fd -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.AlwaysOnTop 410b4ca79848a220f540e0bde17d4561ce0469ba2a8734ebf8c1a4baa3164581 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Utility e09221e19ba5a6a41ee55168c1a4251b0ea22074a0310b3fb8196f69bbc45b29 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Tooltip 35b62160bd75cede2d322b0ddb2ab27965fc142840f76e28a8c64651616e69f5 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.PopupMenu a183f8b216b771b1ec357227b45579a0cbdb2484970ef6edc4ac40609c138b91 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.KeyboardGrabbed cecb4316beaab7a0bd8a7ed937c81f2f98357fe332cce9a9c2d3d98bcf93d820 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Vulkan 82f49d4725e13387b83682e0e431649bfda912278a572baf7e8441f80b4de4c4 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Metal e0408a519ca044da6fe949eebc6cca759ffe5177f96718683d1435ec23a664cd -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.Transparent 284efd559e2c96a7d3c1bfbc7a6523320ef93b289982c66f243911dd0f675584 -->
<!-- api: F:Inno.Native.Sdl3.SDLWindowFlags.NotFocusable 297272ad4356a58224c78984536f41ab49435c8983c6962ee5cf074dda7eb3fa -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d54c5cbd908237dc"></a>[`Inno.Native.Sdl3.SDLWindowFlags.AlwaysOnTop = 65536`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6620) | 当前常量或枚举值：`65536`。 | — |
| <a id="api-e2c1b707a260b45e"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Borderless = 16`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6608) | 当前常量或枚举值：`16`。 | — |
| <a id="api-32fc87fa50ab304a"></a>[`Inno.Native.Sdl3.SDLWindowFlags.External = 2048`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6615) | 当前常量或枚举值：`2048`。 | — |
| <a id="api-1182bdefd3e56256"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Fullscreen = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6604) | 当前常量或枚举值：`1`。 | — |
| <a id="api-ee2379e467f2f927"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Hidden = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6607) | 当前常量或枚举值：`8`。 | — |
| <a id="api-c7d3a08c7311d8cd"></a>[`Inno.Native.Sdl3.SDLWindowFlags.HighPixelDensity = 8192`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6617) | 当前常量或枚举值：`8192`。 | — |
| <a id="api-86b7cb4376953be8"></a>[`Inno.Native.Sdl3.SDLWindowFlags.InputFocus = 512`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6613) | 当前常量或枚举值：`512`。 | — |
| <a id="api-8e3dcb9a65066231"></a>[`Inno.Native.Sdl3.SDLWindowFlags.KeyboardGrabbed = 1048576`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6624) | 当前常量或枚举值：`1048576`。 | — |
| <a id="api-5a7b2649aa6c4193"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Maximized = 128`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6611) | 当前常量或枚举值：`128`。 | — |
| <a id="api-6ce3603c8ca48523"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Metal = 536870912`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6626) | 当前常量或枚举值：`536870912`。 | — |
| <a id="api-18544ad679c11b70"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Minimized = 64`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6610) | 当前常量或枚举值：`64`。 | — |
| <a id="api-da8e4996acc8676d"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Modal = 4096`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6616) | 当前常量或枚举值：`4096`。 | — |
| <a id="api-47374f1142131581"></a>[`Inno.Native.Sdl3.SDLWindowFlags.MouseCapture = 16384`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6618) | 当前常量或枚举值：`16384`。 | — |
| <a id="api-6da10fbf9bc64ab7"></a>[`Inno.Native.Sdl3.SDLWindowFlags.MouseFocus = 1024`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6614) | 当前常量或枚举值：`1024`。 | — |
| <a id="api-3fcb5e8831c0fddb"></a>[`Inno.Native.Sdl3.SDLWindowFlags.MouseGrabbed = 256`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6612) | 当前常量或枚举值：`256`。 | — |
| <a id="api-82e4e45759632cdc"></a>[`Inno.Native.Sdl3.SDLWindowFlags.MouseRelativeMode = 32768`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6619) | 当前常量或枚举值：`32768`。 | — |
| <a id="api-0b34365bb6ac257f"></a>[`Inno.Native.Sdl3.SDLWindowFlags.NotFocusable = 2147483648`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6628) | 当前常量或枚举值：`2147483648`。 | — |
| <a id="api-703507c37f95ee93"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Occluded = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6606) | 当前常量或枚举值：`4`。 | — |
| <a id="api-0df58aadd88f8321"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Opengl = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6605) | 当前常量或枚举值：`2`。 | — |
| <a id="api-8ecc1a361da96207"></a>[`Inno.Native.Sdl3.SDLWindowFlags.PopupMenu = 524288`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6623) | 当前常量或枚举值：`524288`。 | — |
| <a id="api-0273ef0a4a2ab26b"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Resizable = 32`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6609) | 当前常量或枚举值：`32`。 | — |
| <a id="api-0cb2225cd0ab89ce"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Tooltip = 262144`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6622) | 当前常量或枚举值：`262144`。 | — |
| <a id="api-1f80e84a86401ca3"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Transparent = 1073741824`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6627) | 当前常量或枚举值：`1073741824`。 | — |
| <a id="api-17f6f8a60806199c"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Utility = 131072`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6621) | 当前常量或枚举值：`131072`。 | — |
| <a id="api-d399a92fa4f1b579"></a>[`Inno.Native.Sdl3.SDLWindowFlags.Vulkan = 268435456`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6625) | 当前常量或枚举值：`268435456`。 | — |
| <a id="api-4297041940fea704"></a>[`public enum Inno.Native.Sdl3.SDLWindowFlags : ulong`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6602) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLWindowsMessageHook</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLWindowsMessageHook 202866a8ee9378db3cf6780b3f319cbe0e4ed2bcc97b96e8955aaa2a6baa8cdc -->
<!-- api: M:Inno.Native.Sdl3.SDLWindowsMessageHook.#ctor(System.Object,System.IntPtr) f2cfda951a45998515e8a9014b71a155f67c58ac4e5c646f6493761241308f0f -->
<!-- api: M:Inno.Native.Sdl3.SDLWindowsMessageHook.Invoke(System.Void*,Inno.Native.Sdl3.Msg) f30cafc1828485e799d47813762843172a36053c1e5fa2f63bfa882b55ecf8f2 -->
<!-- api: M:Inno.Native.Sdl3.SDLWindowsMessageHook.BeginInvoke(System.Void*,Inno.Native.Sdl3.Msg,System.AsyncCallback,System.Object) 6574fa8e34e37310aeab4d17129b016dcf40856ef5ad1f802c68fa6e648ed04b -->
<!-- api: M:Inno.Native.Sdl3.SDLWindowsMessageHook.EndInvoke(System.IAsyncResult) ec86417c7d3dd065ce4857d32ab54a873f69459b483ad9864d040c04628a258d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-44b00757cab21409"></a>[`public Inno.Native.Sdl3.SDLWindowsMessageHook.SDLWindowsMessageHook(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6915) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-abb958ac513a58f0"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLWindowsMessageHook.BeginInvoke(void* userdata, Inno.Native.Sdl3.Msg msg, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6915) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a062da9eaeb8ae8d"></a>[`public virtual byte Inno.Native.Sdl3.SDLWindowsMessageHook.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6915) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5542f2675d3cde74"></a>[`public virtual byte Inno.Native.Sdl3.SDLWindowsMessageHook.Invoke(void* userdata, Inno.Native.Sdl3.Msg msg)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6915) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe3fd266c500c118"></a>[`public delegate byte Inno.Native.Sdl3.SDLWindowsMessageHook(void* userdata, Inno.Native.Sdl3.Msg msg)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6915) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLX11EventHook</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLX11EventHook 47fefd5b9aeb311fcc1d5264f3cc12aa8386319d757ba159b88693214947eced -->
<!-- api: M:Inno.Native.Sdl3.SDLX11EventHook.#ctor(System.Object,System.IntPtr) e96003bccafc2215e55f2d129b230cba4dc6cb2103ec6956373e30922b1176e7 -->
<!-- api: M:Inno.Native.Sdl3.SDLX11EventHook.Invoke(System.Void*,Inno.Native.Sdl3.XEvent) fb20dd51588102a66656d6e8330fec1a636c53d31e0684ef7860ab50d8280eea -->
<!-- api: M:Inno.Native.Sdl3.SDLX11EventHook.BeginInvoke(System.Void*,Inno.Native.Sdl3.XEvent,System.AsyncCallback,System.Object) 6098908b3cb548c65a27ecbff190763b0fc9284692cc9f6fe5041302486ef756 -->
<!-- api: M:Inno.Native.Sdl3.SDLX11EventHook.EndInvoke(System.IAsyncResult) 11771e804b9b45b84827485f0dd9633a05d5498a4e4f51beaf449942a35216cf -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ed1d10e45ccb8774"></a>[`public Inno.Native.Sdl3.SDLX11EventHook.SDLX11EventHook(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6920) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-04e469e10c927783"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SDLX11EventHook.BeginInvoke(void* userdata, Inno.Native.Sdl3.XEvent xevent, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6920) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1afa03b511fc4afd"></a>[`public virtual byte Inno.Native.Sdl3.SDLX11EventHook.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6920) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d4eed11d0ac6d4d3"></a>[`public virtual byte Inno.Native.Sdl3.SDLX11EventHook.Invoke(void* userdata, Inno.Native.Sdl3.XEvent xevent)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6920) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e1078fa82003ad5d"></a>[`public delegate byte Inno.Native.Sdl3.SDLX11EventHook(void* userdata, Inno.Native.Sdl3.XEvent xevent)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6920) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.Seek</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.Seek e626631a2688f89b62c1c8ca00c56bfb3c388d4f4539dc2bb791e74fa10bef8f -->
<!-- api: M:Inno.Native.Sdl3.Seek.#ctor(System.Object,System.IntPtr) d93ba9574ebc47e82d254d07de05c5a2c98388220d89e0783b82051290a26407 -->
<!-- api: M:Inno.Native.Sdl3.Seek.Invoke(System.Void*,System.Int64,Inno.Native.Sdl3.SDLIOWhence) 271ce8c180355b159eff630ff1388d84fdc3224252ab913e23f7e1596dfb277a -->
<!-- api: M:Inno.Native.Sdl3.Seek.BeginInvoke(System.Void*,System.Int64,Inno.Native.Sdl3.SDLIOWhence,System.AsyncCallback,System.Object) aaac6334df7d6239b04d85bfba05cb77f71720f72f793684d606d3a171f0616f -->
<!-- api: M:Inno.Native.Sdl3.Seek.EndInvoke(System.IAsyncResult) 78faec59ad705d9bd44b425f45845e9837aac8bbcd54bac1ecf6c98b1e437c6d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4eef506799bc64d5"></a>[`public Inno.Native.Sdl3.Seek.Seek(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6640) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5dd5c1d0401118c6"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.Seek.BeginInvoke(void* userdata, long offset, Inno.Native.Sdl3.SDLIOWhence whence, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6640) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-42f159155ff2e97b"></a>[`public virtual long Inno.Native.Sdl3.Seek.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6640) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a042316fea4c8812"></a>[`public virtual long Inno.Native.Sdl3.Seek.Invoke(void* userdata, long offset, Inno.Native.Sdl3.SDLIOWhence whence)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6640) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-19e907c57444a219"></a>[`public delegate long Inno.Native.Sdl3.Seek(void* userdata, long offset, Inno.Native.Sdl3.SDLIOWhence whence)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6640) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SendEffect</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SendEffect 35e9a5491b5cd7b5e2ef6a2d4857f98c0ed362fade5fb4f5b93e5b21bbeff07d -->
<!-- api: M:Inno.Native.Sdl3.SendEffect.#ctor(System.Object,System.IntPtr) d879d876ac051c1653a1b6d67575d61bb2a86e515a661508e0f75c62d422a76c -->
<!-- api: M:Inno.Native.Sdl3.SendEffect.Invoke(System.Void*,System.Void*,System.Int32) 7214f441750aaf9426dcb17a3fb5083f40aa14f9a0b4fd44b6a1b55be200adc0 -->
<!-- api: M:Inno.Native.Sdl3.SendEffect.BeginInvoke(System.Void*,System.Void*,System.Int32,System.AsyncCallback,System.Object) ef665ddb860170464d08855138c65a6322b21d17423bda269b676aa3c3f5cb0d -->
<!-- api: M:Inno.Native.Sdl3.SendEffect.EndInvoke(System.IAsyncResult) 42616458e8f20ace6f3c9a47a2b9e4608b07aed3105c0e139b6a4a99064ad686 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e551c6eeecdd1ee5"></a>[`public Inno.Native.Sdl3.SendEffect.SendEffect(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6690) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-51d1f07d28b13916"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SendEffect.BeginInvoke(void* userdata, void* data, int size, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6690) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1895dfb7a772f147"></a>[`public virtual byte Inno.Native.Sdl3.SendEffect.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6690) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-24762765cfc1416f"></a>[`public virtual byte Inno.Native.Sdl3.SendEffect.Invoke(void* userdata, void* data, int size)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6690) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-afeb8224dfad0cad"></a>[`public delegate byte Inno.Native.Sdl3.SendEffect(void* userdata, void* data, int size)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6690) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SetLED</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SetLED a87e470ae9ee10979c89fb4ff22e43d3ca2f15e691c76dd67fa5c1875197f0ce -->
<!-- api: M:Inno.Native.Sdl3.SetLED.#ctor(System.Object,System.IntPtr) b104d5c215b227119e3c05593d9553f3129474d1571d6d1255e21e2ded770610 -->
<!-- api: M:Inno.Native.Sdl3.SetLED.Invoke(System.Void*,System.Byte,System.Byte,System.Byte) bee1824a6ad1271b1598231dfa1a4814e8ee583cbff55fcbe9a89f818a9304d8 -->
<!-- api: M:Inno.Native.Sdl3.SetLED.BeginInvoke(System.Void*,System.Byte,System.Byte,System.Byte,System.AsyncCallback,System.Object) 1d12c771493c30e6a0231cbf9625d703e03c1e91278f8a3d69fc329061bd45b0 -->
<!-- api: M:Inno.Native.Sdl3.SetLED.EndInvoke(System.IAsyncResult) d40ae0d303c9cb451b7c58ed282e47ba608ed57074237965abb7c6a61ea938d2 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-951bb135fe7cf488"></a>[`public Inno.Native.Sdl3.SetLED.SetLED(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6685) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d93b60c6bb7d7963"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SetLED.BeginInvoke(void* userdata, byte red, byte green, byte blue, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6685) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3a1b0ea91cd610a3"></a>[`public virtual byte Inno.Native.Sdl3.SetLED.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6685) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-52254ea9f91bdca2"></a>[`public virtual byte Inno.Native.Sdl3.SetLED.Invoke(void* userdata, byte red, byte green, byte blue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6685) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a7fcf36a75f68b7f"></a>[`public delegate byte Inno.Native.Sdl3.SetLED(void* userdata, byte red, byte green, byte blue)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6685) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SetPlayerIndex</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SetPlayerIndex 27223f6979ff21449328c70cf0c612f25482bc25555450a995c67117f5dfd7e3 -->
<!-- api: M:Inno.Native.Sdl3.SetPlayerIndex.#ctor(System.Object,System.IntPtr) 4624ca7212c3e55e6237c956cac3b5004598e3baf0177db256e4d62faf9d2dc6 -->
<!-- api: M:Inno.Native.Sdl3.SetPlayerIndex.Invoke(System.Void*,System.Int32) d151681a403789d5d37949b08b08c56accd6aaebe189f4205f7f58bd74281df7 -->
<!-- api: M:Inno.Native.Sdl3.SetPlayerIndex.BeginInvoke(System.Void*,System.Int32,System.AsyncCallback,System.Object) 2a712946f44a0eb6c3801f54a30abffbf128caeb6c0298a87bfdfb660d5e09d6 -->
<!-- api: M:Inno.Native.Sdl3.SetPlayerIndex.EndInvoke(System.IAsyncResult) 916fc4fc61bc7438ad522b2e0894841d940f108ad8b2e439aefd31731a540bfd -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4bf1b9b5bc2da061"></a>[`public Inno.Native.Sdl3.SetPlayerIndex.SetPlayerIndex(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6670) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-cbb1b6cafa842c37"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SetPlayerIndex.BeginInvoke(void* userdata, int playerIndex, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6670) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9324805500189fcf"></a>[`public virtual void Inno.Native.Sdl3.SetPlayerIndex.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6670) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bcca629b346d093b"></a>[`public virtual void Inno.Native.Sdl3.SetPlayerIndex.Invoke(void* userdata, int playerIndex)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6670) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e5b8403b90b5cf87"></a>[`public delegate void Inno.Native.Sdl3.SetPlayerIndex(void* userdata, int playerIndex)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6670) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SetSensorsEnabled</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SetSensorsEnabled e2cb2e1b9fe6707df2cbfea365aeb0e2dc6247df0f2fb9b6bdf6ff1815663ee8 -->
<!-- api: M:Inno.Native.Sdl3.SetSensorsEnabled.#ctor(System.Object,System.IntPtr) 2eaaafec5d4af713ca1c97d40e975d903ef67e398e832239b1d02f5fbff0dd56 -->
<!-- api: M:Inno.Native.Sdl3.SetSensorsEnabled.Invoke(System.Void*,System.Byte) 0eeab744e2f832d41db2eeaa69eeee919aea541f0e24cd31012adeffec1f961a -->
<!-- api: M:Inno.Native.Sdl3.SetSensorsEnabled.BeginInvoke(System.Void*,System.Byte,System.AsyncCallback,System.Object) a3f5bca9c20657258ff28244f7b97647b2b92a3b212d9495e791519ffbcf39e9 -->
<!-- api: M:Inno.Native.Sdl3.SetSensorsEnabled.EndInvoke(System.IAsyncResult) ef36b1b417498305c07fb1fa849d40f727df3acc553c33b5ffc40495bc7b29f0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7398d867fd6ce05d"></a>[`public Inno.Native.Sdl3.SetSensorsEnabled.SetSensorsEnabled(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6695) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-34b6d029e5b23d8b"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SetSensorsEnabled.BeginInvoke(void* userdata, byte enabled, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6695) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-80cb4203ab088687"></a>[`public virtual byte Inno.Native.Sdl3.SetSensorsEnabled.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6695) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6949659c3c366e68"></a>[`public virtual byte Inno.Native.Sdl3.SetSensorsEnabled.Invoke(void* userdata, byte enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6695) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f93085ecbc85e1a6"></a>[`public delegate byte Inno.Native.Sdl3.SetSensorsEnabled(void* userdata, byte enabled)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6695) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.Size</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.Size 16a33ec3e196bcfdbbfe3ab5d457b814870e94615fe30ae5c85ba00eca01a629 -->
<!-- api: M:Inno.Native.Sdl3.Size.#ctor(System.Object,System.IntPtr) 839f152f4a89369f412bc5daab38aba3a64001db96890346f4ecd8b9fd51b00e -->
<!-- api: M:Inno.Native.Sdl3.Size.Invoke(System.Void*) b8a273891afca95e4277db0786ab5c528d205d6d2854ff1f9a64688e89357a5f -->
<!-- api: M:Inno.Native.Sdl3.Size.BeginInvoke(System.Void*,System.AsyncCallback,System.Object) 7d2a723e67c9a04a02a422d19af4c83767dfbe3756085252cb568b67dd30323a -->
<!-- api: M:Inno.Native.Sdl3.Size.EndInvoke(System.IAsyncResult) 0cbd577c30a751f75ecbda42b973e98dcb45f87c03a5dcb8234aa7ed0c2ad48f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-40c64af89d093164"></a>[`public Inno.Native.Sdl3.Size.Size(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6635) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-72d2e49098e0bf93"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.Size.BeginInvoke(void* userdata, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6635) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4c77f80a469945ec"></a>[`public virtual long Inno.Native.Sdl3.Size.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6635) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e02cf1835d2a1270"></a>[`public virtual long Inno.Native.Sdl3.Size.Invoke(void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6635) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-15a3ef2eed495bb9"></a>[`public delegate long Inno.Native.Sdl3.Size(void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6635) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SpaceRemaining</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SpaceRemaining 69e558f5bf8f5483e92b3740cada72b61a5c366e1ea2ebfd30f6df1b1ff99858 -->
<!-- api: M:Inno.Native.Sdl3.SpaceRemaining.#ctor(System.Object,System.IntPtr) e102269906d2b49ca1dda6b55c718ae51a90d5f1e5143ab0173117e1e63dd214 -->
<!-- api: M:Inno.Native.Sdl3.SpaceRemaining.Invoke(System.Void*) f5335c2b5a83a30aba75cc0595075408d6d2300128e6de6e34807d73ccc26d7c -->
<!-- api: M:Inno.Native.Sdl3.SpaceRemaining.BeginInvoke(System.Void*,System.AsyncCallback,System.Object) a9e2a21c94c7d14defd9f47b99789c387a8010c4795e0087d34a043f3171f3a0 -->
<!-- api: M:Inno.Native.Sdl3.SpaceRemaining.EndInvoke(System.IAsyncResult) 61de4fb9d093c0a34ce7c68c78db58f2fb53cd29cb0e9b02f5fed859c2a4b2c3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c6d74a8f255d08f9"></a>[`public Inno.Native.Sdl3.SpaceRemaining.SpaceRemaining(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6760) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3f03e1b928961763"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.SpaceRemaining.BeginInvoke(void* userdata, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6760) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0dcf1365ed8f2c53"></a>[`public virtual ulong Inno.Native.Sdl3.SpaceRemaining.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6760) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-77e9fc1575e08207"></a>[`public virtual ulong Inno.Native.Sdl3.SpaceRemaining.Invoke(void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6760) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0a87460c3e8f3157"></a>[`public delegate ulong Inno.Native.Sdl3.SpaceRemaining(void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6760) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.Update</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.Update bf7f50107cfc705cfb82f72e01caa2a05bc31e1da8b9e6cef464f2a646ba57c1 -->
<!-- api: M:Inno.Native.Sdl3.Update.#ctor(System.Object,System.IntPtr) ade3e0c2dbee16c2f24daca21a9a706aeb3aa851d35fc8c016f715419dc941a1 -->
<!-- api: M:Inno.Native.Sdl3.Update.Invoke(System.Void*) 21d0c6f8855dcc7fb79c0757aaca524bdc9ed7193a0bbd6764158d57030e2a3f -->
<!-- api: M:Inno.Native.Sdl3.Update.BeginInvoke(System.Void*,System.AsyncCallback,System.Object) 5893d337db30a9ac43f0f06459d63ea7d8d7ffa0a66c56621c9a2182e8e136b0 -->
<!-- api: M:Inno.Native.Sdl3.Update.EndInvoke(System.IAsyncResult) 7176b56b68b4c7b127dd39724637c6c71d07d9d0b2ec3f6761a19a2b4111f8d7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2d25c73c03fd0f5e"></a>[`public Inno.Native.Sdl3.Update.Update(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6665) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8809d27b20e03d51"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.Update.BeginInvoke(void* userdata, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6665) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8be1e2ad1185f564"></a>[`public virtual void Inno.Native.Sdl3.Update.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6665) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a221738eae7873a6"></a>[`public virtual void Inno.Native.Sdl3.Update.Invoke(void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6665) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c51c78686dda708c"></a>[`public delegate void Inno.Native.Sdl3.Update(void* userdata)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6665) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.Write</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.Write 1d7b05baf2060cbfccff7e5ce54b6dca5d4bf823e3a7d0dad917fbaef1207090 -->
<!-- api: M:Inno.Native.Sdl3.Write.#ctor(System.Object,System.IntPtr) e680a4d905616325fb8af3af2ff3037a6d9711f86cab1aed990b730bab627c4b -->
<!-- api: M:Inno.Native.Sdl3.Write.Invoke(System.Void*,System.Void*,System.UInt64,Inno.Native.Sdl3.SDLIOStatus*) 9e30f1171a4f0ccf95b24c004c58110ee31bc5903491752aa9556b7c14d5ca05 -->
<!-- api: M:Inno.Native.Sdl3.Write.BeginInvoke(System.Void*,System.Void*,System.UInt64,Inno.Native.Sdl3.SDLIOStatus*,System.AsyncCallback,System.Object) 85fab028472eb11d4d926aab113b70870f7a2968afc8ee7f327db317e98e9c10 -->
<!-- api: M:Inno.Native.Sdl3.Write.EndInvoke(System.IAsyncResult) 302803e0af9d881545ebd9a62e919a5ed6c9bde6ccf41806c1b1c89a3d069655 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-018cbc8ca365ae55"></a>[`public Inno.Native.Sdl3.Write.Write(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6650) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f11f77c38781685b"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.Write.BeginInvoke(void* userdata, void* ptr, ulong size, Inno.Native.Sdl3.SDLIOStatus* status, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6650) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-618b6ff3e487ab78"></a>[`public virtual ulong Inno.Native.Sdl3.Write.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6650) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-34a371380dd7660e"></a>[`public virtual ulong Inno.Native.Sdl3.Write.Invoke(void* userdata, void* ptr, ulong size, Inno.Native.Sdl3.SDLIOStatus* status)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6650) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d3482c8979aeb0d3"></a>[`public delegate ulong Inno.Native.Sdl3.Write(void* userdata, void* ptr, ulong size, Inno.Native.Sdl3.SDLIOStatus* status)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6650) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.WriteFile</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.WriteFile adaaa29fd5fc83bf180fd35b4f0468411695b9bd133a7e1582aef3991d693a28 -->
<!-- api: M:Inno.Native.Sdl3.WriteFile.#ctor(System.Object,System.IntPtr) 7f4e0c632c4703f61eef886985957684f56439af48ce943d394a9a900653cbd0 -->
<!-- api: M:Inno.Native.Sdl3.WriteFile.Invoke(System.Void*,System.Byte*,System.Void*,System.UInt64) c33721fa5e23748876e5bc5a60b889a46d724ada5d03d353adc18ab31d804e81 -->
<!-- api: M:Inno.Native.Sdl3.WriteFile.BeginInvoke(System.Void*,System.Byte*,System.Void*,System.UInt64,System.AsyncCallback,System.Object) acd4f300de00731e36a248705e614c19ce96ffe2f96a72a20c19042e86c9bca7 -->
<!-- api: M:Inno.Native.Sdl3.WriteFile.EndInvoke(System.IAsyncResult) fad01c9e080885d4f50d8a3de4345651d0ba2ddc1d44bbca69b98622292328ff -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-240aaded06bba739"></a>[`public Inno.Native.Sdl3.WriteFile.WriteFile(object @object, nint method)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6735) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-37ce36deb4496fb9"></a>[`public virtual System.IAsyncResult Inno.Native.Sdl3.WriteFile.BeginInvoke(void* userdata, byte* path, void* source, ulong length, System.AsyncCallback callback, object @object)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6735) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bf9af07bc0755f7e"></a>[`public virtual byte Inno.Native.Sdl3.WriteFile.EndInvoke(System.IAsyncResult result)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6735) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6ef2f43ff0b5d5e4"></a>[`public virtual byte Inno.Native.Sdl3.WriteFile.Invoke(void* userdata, byte* path, void* source, ulong length)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6735) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ed0f6d13cffc5bd6"></a>[`public delegate byte Inno.Native.Sdl3.WriteFile(void* userdata, byte* path, void* source, ulong length)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6735) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.XEvent</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.XEvent 07042d1b128281b4215ad6e954b4a7589b46316f8c96b9ce04c7ffe5fb03d254 -->
<!-- api: M:Inno.Native.Sdl3.XEvent.#ctor(System.IntPtr) d7d6af1f9f8a2d4bcdfe26cbac31d13327920efb256aec64a418f8260b16dd1c -->
<!-- api: P:Inno.Native.Sdl3.XEvent.Handle c1d6e700480019eafc48f30798e6652cfc122775d4537dcf2a5d1ee292d5724f -->
<!-- api: P:Inno.Native.Sdl3.XEvent.IsNull 015b00a6be4d1b1d40dee15a49f7a0234c0c6a30e89f05e8f4e9f58d7b5021b8 -->
<!-- api: P:Inno.Native.Sdl3.XEvent.Null 784e5ca9eef10952b30c7bcf4863b88a03ad897babe063c0530e931a9dadbfde -->
<!-- api: M:Inno.Native.Sdl3.XEvent.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.XEvent 992ae6260f75295ba27f0d567bb995b8f55c20a83dc204eb83152f3c6b9a92fc -->
<!-- api: M:Inno.Native.Sdl3.XEvent.op_Equality(Inno.Native.Sdl3.XEvent,Inno.Native.Sdl3.XEvent) f8f6d51bbd1f4647b98dbad82d6d98003bb853cce686c4791c1118851eb3b4e2 -->
<!-- api: M:Inno.Native.Sdl3.XEvent.op_Inequality(Inno.Native.Sdl3.XEvent,Inno.Native.Sdl3.XEvent) 6469766b6bf7f508350232ae12b364e5907dad1f9e57a664741196ea7ecd04d2 -->
<!-- api: M:Inno.Native.Sdl3.XEvent.Equals(Inno.Native.Sdl3.XEvent) 33d12f6894bbd7fb6135086db36cad51911b147a38ec7794afa6a3033b9d58fc -->
<!-- api: M:Inno.Native.Sdl3.XEvent.Equals(System.Object) 4fd1eb5575a16087a77ac88d28ca0a4f7c4d4a1d2b690ad87efe5a5d951c31d4 -->
<!-- api: M:Inno.Native.Sdl3.XEvent.GetHashCode 23824ffb0763c99c09790e96f297ce6d26d5c4b9ec04832efbbcb83090b553d4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f4574cbc8263dabc"></a>[`public Inno.Native.Sdl3.XEvent.XEvent(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6351) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-acb73dd03054ec42"></a>[`public bool Inno.Native.Sdl3.XEvent.Equals(Inno.Native.Sdl3.XEvent other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6363) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-5c3f27282b86513b"></a>[`public override bool Inno.Native.Sdl3.XEvent.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6364) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-cfa83cd5d2d923cf"></a>[`public override int Inno.Native.Sdl3.XEvent.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6365) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-2fa56d338618b54f"></a>[`public static bool Inno.Native.Sdl3.XEvent.operator ==(Inno.Native.Sdl3.XEvent left, Inno.Native.Sdl3.XEvent right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6361) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4cf3df78eae4e215"></a>[`public static Inno.Native.Sdl3.XEvent.implicit operator Inno.Native.Sdl3.XEvent(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6360) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f023501d9ab904d6"></a>[`public static bool Inno.Native.Sdl3.XEvent.operator !=(Inno.Native.Sdl3.XEvent left, Inno.Native.Sdl3.XEvent right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6362) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6cfe4186295adf3e"></a>[`public nint Inno.Native.Sdl3.XEvent.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6356) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6281221135d3a2a0"></a>[`public bool Inno.Native.Sdl3.XEvent.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6357) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-569b487d21d474f2"></a>[`public static Inno.Native.Sdl3.XEvent Inno.Native.Sdl3.XEvent.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6358) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5f15724aa76c55e6"></a>[`public readonly struct Inno.Native.Sdl3.XEvent : System.IEquatable<Inno.Native.Sdl3.XEvent>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6349) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
