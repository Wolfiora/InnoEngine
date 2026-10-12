# Inno.Native.MiniAudio ABI · DataCallback → MaBpfNodePtr

[项目与生命周期](../Inno.Native.MiniAudio.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.MiniAudio.DataCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.DataCallback 3de695dc20c04e432f3f93e82e128f27f625705ac9d6c6cc026b810e4d4c7dfc -->
<!-- api: M:Inno.Native.MiniAudio.DataCallback.#ctor(System.Object,System.IntPtr) caea0d4e36c2a1ffe71468af456a90dc554c7a2c02617e8ce5a32ea3b9af5de8 -->
<!-- api: M:Inno.Native.MiniAudio.DataCallback.Invoke(Inno.Native.MiniAudio.MaDevice*,System.Void*,System.Void*,System.UInt32) e14702c5c06e536f6269058b377c96f6860efa0e4e8ad64763e0ba30c14059b6 -->
<!-- api: M:Inno.Native.MiniAudio.DataCallback.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,System.Void*,System.Void*,System.UInt32,System.AsyncCallback,System.Object) c13af489dcaf019ac6be0be6eca52d66b5952df838dcb36f7c5391e457cd5d27 -->
<!-- api: M:Inno.Native.MiniAudio.DataCallback.EndInvoke(System.IAsyncResult) c7b7fa3ea1544bde180e0de4c839443894645470f6940342eea332b127ce1bec -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-eedddda0ceb81067"></a>[`public Inno.Native.MiniAudio.DataCallback.DataCallback(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14021) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7f8ecc1db2f3fb9f"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.DataCallback.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, void* pOutput, void* pInput, uint frameCount, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14021) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-284bb98a5b9e6ec1"></a>[`public virtual void Inno.Native.MiniAudio.DataCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14021) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2c9fd42c3040250e"></a>[`public virtual void Inno.Native.MiniAudio.DataCallback.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice, void* pOutput, void* pInput, uint frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14021) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e49fe6c02cbaf2b0"></a>[`public delegate void Inno.Native.MiniAudio.DataCallback(Inno.Native.MiniAudio.MaDevice* pDevice, void* pOutput, void* pInput, uint frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14021) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.DataCallback1</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.DataCallback1 dd777bc317f6b6af8911816458a884da4586bc43f105451f87d44815d22d82ab -->
<!-- api: M:Inno.Native.MiniAudio.DataCallback1.#ctor(System.Object,System.IntPtr) 7ba5024aa8d41290330e7b2e474b932798c558349ca10a8ceb5dc1713bda935d -->
<!-- api: M:Inno.Native.MiniAudio.DataCallback1.Invoke(Inno.Native.MiniAudio.MaDevice*,System.Void*,System.Void*,System.UInt32) bdf20a2459a6421405b14000c44eb2c5c2d2b64adbebb757ca0e49d56f988462 -->
<!-- api: M:Inno.Native.MiniAudio.DataCallback1.BeginInvoke(Inno.Native.MiniAudio.MaDevice*,System.Void*,System.Void*,System.UInt32,System.AsyncCallback,System.Object) 4192166fd2a5036b97441716e255cb7053de5fa07481f2b81177cfebc869e001 -->
<!-- api: M:Inno.Native.MiniAudio.DataCallback1.EndInvoke(System.IAsyncResult) 21c350c632d37f048a8a6cb554b341d7ccb6bea595e72ea0d754ad77742af2c6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8cc4bcb28632b844"></a>[`public Inno.Native.MiniAudio.DataCallback1.DataCallback1(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14311) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-6e08e2dd9dc17f98"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.DataCallback1.BeginInvoke(Inno.Native.MiniAudio.MaDevice* pDevice, void* pOutput, void* pInput, uint frameCount, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14311) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-313f14b96285e1e1"></a>[`public virtual void Inno.Native.MiniAudio.DataCallback1.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14311) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7648869d8b4ae121"></a>[`public virtual void Inno.Native.MiniAudio.DataCallback1.Invoke(Inno.Native.MiniAudio.MaDevice* pDevice, void* pOutput, void* pInput, uint frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14311) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ab4d05da54036d5a"></a>[`public delegate void Inno.Native.MiniAudio.DataCallback1(Inno.Native.MiniAudio.MaDevice* pDevice, void* pOutput, void* pInput, uint frameCount)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14311) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.EndCallback</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.EndCallback ae73d99258e3f0cb667f25134cdfa1569e1c5ad05b555233af2b477ade8d5f57 -->
<!-- api: M:Inno.Native.MiniAudio.EndCallback.#ctor(System.Object,System.IntPtr) d0350177e223bb76c124f600b8476ed49a1d969b1d536db180b8b8ce76b9f7e8 -->
<!-- api: M:Inno.Native.MiniAudio.EndCallback.Invoke(System.Void*,Inno.Native.MiniAudio.MaSound*) 026abfb73f223c6772cd1c847cb6e394fff9f0336cdfc4f04f908b5640a96934 -->
<!-- api: M:Inno.Native.MiniAudio.EndCallback.BeginInvoke(System.Void*,Inno.Native.MiniAudio.MaSound*,System.AsyncCallback,System.Object) a32148a471ddcb893b4b9b1d99ba0b16d0238aabf8a34d65cf65000e96927a94 -->
<!-- api: M:Inno.Native.MiniAudio.EndCallback.EndInvoke(System.IAsyncResult) 5b80a67de0d59980ff3360b4368e16b5b2fd72277b8a1d2cbefcc6eeeb5b1001 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2ab7dad04efd699d"></a>[`public Inno.Native.MiniAudio.EndCallback.EndCallback(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14291) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-952cf650e80523a7"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.EndCallback.BeginInvoke(void* pUserData, Inno.Native.MiniAudio.MaSound* pSound, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14291) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1e19423d43fd400c"></a>[`public virtual void Inno.Native.MiniAudio.EndCallback.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14291) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-637e99d6e04d98fa"></a>[`public virtual void Inno.Native.MiniAudio.EndCallback.Invoke(void* pUserData, Inno.Native.MiniAudio.MaSound* pSound)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14291) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a34dbde24dd254cf"></a>[`public delegate void Inno.Native.MiniAudio.EndCallback(void* pUserData, Inno.Native.MiniAudio.MaSound* pSound)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14291) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.EndCallback1</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.EndCallback1 c02802e83125add41ee9e7086c6b13a5d2c718f778b6e6012856cbd4bebbf5fc -->
<!-- api: M:Inno.Native.MiniAudio.EndCallback1.#ctor(System.Object,System.IntPtr) 84265dc397f789801033441f2b58cc02ddf71dad6dff6923fc6c5a76db395010 -->
<!-- api: M:Inno.Native.MiniAudio.EndCallback1.Invoke(System.Void*,Inno.Native.MiniAudio.MaSound*) bc6fea56ad6edcc0619d5b933a7f8d77d7651ade6a5f6ade8cf2cc5d0ed70666 -->
<!-- api: M:Inno.Native.MiniAudio.EndCallback1.BeginInvoke(System.Void*,Inno.Native.MiniAudio.MaSound*,System.AsyncCallback,System.Object) 57969235f27b9b804ff8f77b3417c95b6d984ec6f22ed95829b6703e3d6967ed -->
<!-- api: M:Inno.Native.MiniAudio.EndCallback1.EndInvoke(System.IAsyncResult) cf949b46d1da638af21bece69c6c9fc9fe90106a91bb4a7cb3b8c9e2c1bd5a78 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d999a254fbcef930"></a>[`public Inno.Native.MiniAudio.EndCallback1.EndCallback1(object @object, nint method)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14306) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f18e34391e9ba413"></a>[`public virtual System.IAsyncResult Inno.Native.MiniAudio.EndCallback1.BeginInvoke(void* pUserData, Inno.Native.MiniAudio.MaSound* pSound, System.AsyncCallback callback, object @object)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14306) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-caced68ae977224e"></a>[`public virtual void Inno.Native.MiniAudio.EndCallback1.EndInvoke(System.IAsyncResult result)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14306) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-077687ed30fb44af"></a>[`public virtual void Inno.Native.MiniAudio.EndCallback1.Invoke(void* pUserData, Inno.Native.MiniAudio.MaSound* pSound)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14306) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e289bec936d94b91"></a>[`public delegate void Inno.Native.MiniAudio.EndCallback1(void* pUserData, Inno.Native.MiniAudio.MaSound* pSound)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14306) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy d3e0dcbc856f29e6d16bee565a404f61deb87a1d7035bdbc513232a7bee4ab3a -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.AllowCaptureDefault 0226f3593c5f12c636edd4f25ec5b99e1e135cb0a674ceecbb5ce0d955e829eb -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.AllowCaptureByAll e920b51bf54df40ce068760b162416a0145e95cb472bd9afc3ebc9ebba3f90f8 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.AllowCaptureBySystem 497375a2bd131b37d4b671555eb21a7eafe23876d4fe1c1642fdd6e2380d91d3 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.AllowCaptureByNone 3a5ad6d05d015283581202a25bea4017bdec4e6919654db00d37b0bb411e64d6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fd4651a12ef7a694"></a>[`Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.AllowCaptureByAll = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L386) | 当前常量或枚举值：`1`。 | — |
| <a id="api-bdf527b6e84ac641"></a>[`Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.AllowCaptureByNone = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L388) | 当前常量或枚举值：`3`。 | — |
| <a id="api-e4ba3f5b23cd87f6"></a>[`Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.AllowCaptureBySystem = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L387) | 当前常量或枚举值：`2`。 | — |
| <a id="api-ebc6fc5ff594d858"></a>[`Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy.AllowCaptureDefault = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L385) | 当前常量或枚举值：`0`。 | — |
| <a id="api-99e03934ceed2291"></a>[`public enum Inno.Native.MiniAudio.MaAaudioAllowedCapturePolicy : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L383) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAaudioContentType</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAaudioContentType 662b15fa44a9679b9b46efac6602c8c0834acccee266eb9d07a8bd381d9c31a8 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioContentType.Default f71b727dfaa31cdccf86259927bab306fb7dd8a2ef1ff4324bd44042757ccefb -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioContentType.Speech f42af0283141be367b8d7dcf5bb5d666a7089822a4f39a62792c50a7f21d02f3 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioContentType.Music 7b75fe2a5680322ec2d22ea646f550164a0c11f7e222bf06d1265d79681bc7ef -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioContentType.Movie da159dfe48afe212a11745edc5667a04ded8f67a04236e02883daba868998932 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioContentType.Sonification c7887112d14091cb9f3570700407e716d4c967ec83d1f7de140f4bd05ccf7625 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-12242aa18e48aca2"></a>[`Inno.Native.MiniAudio.MaAaudioContentType.Default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L359) | 当前常量或枚举值：`0`。 | — |
| <a id="api-cfe61a12cf696e2d"></a>[`Inno.Native.MiniAudio.MaAaudioContentType.Movie = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L362) | 当前常量或枚举值：`3`。 | — |
| <a id="api-cd32a18dcd419648"></a>[`Inno.Native.MiniAudio.MaAaudioContentType.Music = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L361) | 当前常量或枚举值：`2`。 | — |
| <a id="api-64ce0659d9a3f244"></a>[`Inno.Native.MiniAudio.MaAaudioContentType.Sonification = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L363) | 当前常量或枚举值：`4`。 | — |
| <a id="api-41ce3311965a70f3"></a>[`Inno.Native.MiniAudio.MaAaudioContentType.Speech = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L360) | 当前常量或枚举值：`1`。 | — |
| <a id="api-1b46be7888cb0c94"></a>[`public enum Inno.Native.MiniAudio.MaAaudioContentType : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L357) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAaudioInputPreset</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAaudioInputPreset 73268ca0812a1f574226452e23789be9168f3b04a72cb943ec812dfbf200b936 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.Default f876f7e9f064a707ac1d98b0aa97ea6cf97c1bc3a9dc3396c889ffcbe0489f9b -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.Generic a04f5dd5a8b8449882a4e5bf8227cf0afd4e609d20db388d5eeeec0e956ec949 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.Camcorder 4df1eae4eb3893694ddb1e0f0985a2eaa58ed39458caecbdbeba5155cf2524ee -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.VoiceRecognition 7cb3c3b2fa5ba0069e944485bcd4479f41bd170910bdd7bd8a6d3576b6faed74 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.VoiceCommunication 1b1bf992a2c747c8c0776b68e8f49d465d979ec1aa895bf05689b6a90d04a193 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.Unprocessed cf3493a3bbc372ccc311c0891658cdec2dce96a860ae53d2903facd046a87b79 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioInputPreset.VoicePerformance 4b679c43f8a24e4967ef851d298f068ea1b99542596663d2ac66e0c0a4bca44b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d50e0903e08f85ab"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.Camcorder = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L373) | 当前常量或枚举值：`2`。 | — |
| <a id="api-27e01d57d9cf560d"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.Default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L371) | 当前常量或枚举值：`0`。 | — |
| <a id="api-0675e6fb924deac9"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.Generic = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L372) | 当前常量或枚举值：`1`。 | — |
| <a id="api-ba843c61d5ad4ae2"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.Unprocessed = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L376) | 当前常量或枚举值：`5`。 | — |
| <a id="api-4901bdf4ff5442e5"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.VoiceCommunication = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L375) | 当前常量或枚举值：`4`。 | — |
| <a id="api-252f661e00622416"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.VoicePerformance = 6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L377) | 当前常量或枚举值：`6`。 | — |
| <a id="api-5b7fcf4a2cc534f4"></a>[`Inno.Native.MiniAudio.MaAaudioInputPreset.VoiceRecognition = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L374) | 当前常量或枚举值：`3`。 | — |
| <a id="api-5294462cbb7e90f2"></a>[`public enum Inno.Native.MiniAudio.MaAaudioInputPreset : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L369) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAaudioUsage</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAaudioUsage b8a2b386134a236a57c23cc76ce6a74097f683fe06125d9a0d9bca59a4a949c4 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.Default 9e7d2892ed0de3c548a2ca22beb2ac49c1e2d09c60c3703743a7760efec8ebde -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.Media e7f997a19db65cc76adc8102e125d5db6f3672b89ca0eb6fd4c699d847bfc27c -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.VoiceCommunication 50ddbb19057617f4c61cb52494ff3a06e1ab894174e9780407c786458f4bb360 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.VoiceCommunicationSignalling a7354f691ff0094adb39411b5b858282da5a5bfcf2daeafdfc57f0486f4e1c9e -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.Alarm 7d88814743353287018285eaf5b8fd63b5532849f02ed81c8c33db9841eefb30 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.Notification fa4a3daf0820449fbc63d912b985b7a902764c69e4cf6d74e0f1ac6f25a6ae6e -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.NotificationRingtone 72da05061186475b25144cdefe62dc33d603ce52c2a7c2b74ccdc066fe309091 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.NotificationEvent 01f46d86630d3a8153719954b9cf683aa60cb9ad3684254286fe32a54ba57c11 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.AssistanceAccessibility 6ea1fb3f90b13febf4811f547836142f197eb48425a28edd0b1d344dc1d35105 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.AssistanceNavigationGuidance ec95a56c26e3a4b7edea6303a26020a54ba90da4329b5368be5169fca46c6f9a -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.AssistanceSonification 8e4f01162bd53560e29acd3bd055098b768e0bf61a39b3db3b9960922b00f40e -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.Game a69ec8235168158956556708cbd691646d687abd6affb0ff03e37854ce6a4b3b -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.Assitant 4f0f11c525e3156621a1de73dc855c464b378ea8f4c70dafc8332671211fa7e3 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.Emergency 6e515446e628dccc073b778512651f897928f729a8c0e8a6d45aef67b8b18b60 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.Safety a04df7ccc010c4e3a5bbc2df220438dbb32c6915f54565c7bd13464dfd35d018 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.VehicleStatus c9d6cf4ca80db5b9ea30abc16b780cd56cc9fff73d7719549c8b5d35d8291a63 -->
<!-- api: F:Inno.Native.MiniAudio.MaAaudioUsage.Announcement e5a42306709676640ee8ca044d6c452894be0b0b9c0eb9b661105c041f538f6b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3418e44c10b8ddec"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.Alarm = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L339) | 当前常量或枚举值：`4`。 | — |
| <a id="api-b1e2d86b334f9caa"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.Announcement = 16`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L351) | 当前常量或枚举值：`16`。 | — |
| <a id="api-0a43165c8bfd89cc"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.AssistanceAccessibility = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L343) | 当前常量或枚举值：`8`。 | — |
| <a id="api-0257abcc49b39230"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.AssistanceNavigationGuidance = 9`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L344) | 当前常量或枚举值：`9`。 | — |
| <a id="api-dd772061a435dfeb"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.AssistanceSonification = 10`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L345) | 当前常量或枚举值：`10`。 | — |
| <a id="api-ddb3ce3203865c13"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.Assitant = 12`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L347) | 当前常量或枚举值：`12`。 | — |
| <a id="api-1eb2ef36c18d8118"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.Default = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L335) | 当前常量或枚举值：`0`。 | — |
| <a id="api-c04945347503c4fe"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.Emergency = 13`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L348) | 当前常量或枚举值：`13`。 | — |
| <a id="api-46aad1f523deb4fa"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.Game = 11`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L346) | 当前常量或枚举值：`11`。 | — |
| <a id="api-6c4efc6a33ba77ee"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.Media = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L336) | 当前常量或枚举值：`1`。 | — |
| <a id="api-ee97afd41442e528"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.Notification = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L340) | 当前常量或枚举值：`5`。 | — |
| <a id="api-6b4ca947305aaa53"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.NotificationEvent = 7`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L342) | 当前常量或枚举值：`7`。 | — |
| <a id="api-1c5c0e77d38d3e30"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.NotificationRingtone = 6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L341) | 当前常量或枚举值：`6`。 | — |
| <a id="api-d5148fa985d0ca2b"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.Safety = 14`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L349) | 当前常量或枚举值：`14`。 | — |
| <a id="api-2705e4d6a042a3ce"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.VehicleStatus = 15`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L350) | 当前常量或枚举值：`15`。 | — |
| <a id="api-06bd6f7737cdfe6d"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.VoiceCommunication = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L337) | 当前常量或枚举值：`2`。 | — |
| <a id="api-d24c5abb350a6528"></a>[`Inno.Native.MiniAudio.MaAaudioUsage.VoiceCommunicationSignalling = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L338) | 当前常量或枚举值：`3`。 | — |
| <a id="api-5129ac4268d378ab"></a>[`public enum Inno.Native.MiniAudio.MaAaudioUsage : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L333) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAllocationCallbacks</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAllocationCallbacks d21036e22b7e4cf1e81ecb76faef478542a709afce0c5392be3611278c03a97a -->
<!-- api: F:Inno.Native.MiniAudio.MaAllocationCallbacks.PUserData 40ccba2af34ce74aa8c2d713943abd7ad03798689453d153c0d66aded67d878c -->
<!-- api: F:Inno.Native.MiniAudio.MaAllocationCallbacks.OnMalloc bccc0245df8beb2cb044771e8086ba167c70fa44ace63c727665e49d0fc86c02 -->
<!-- api: F:Inno.Native.MiniAudio.MaAllocationCallbacks.OnRealloc 9faf7ad964557aed511676b7f2b8cb5b0d1b2ac7c375238713880081282ec85c -->
<!-- api: F:Inno.Native.MiniAudio.MaAllocationCallbacks.OnFree e53c57b497f0b98250456c8fe2a0a36107bead09ad658c4d23f49aae2f344a9d -->
<!-- api: M:Inno.Native.MiniAudio.MaAllocationCallbacks.#ctor(System.Void*,System.Void*,System.Void*,System.Void*) 26455e806c137681a5a4fa17d4775deff84f43a857f090911b26a629e1d8c8b5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-56ce109a3a8d3b2e"></a>[`public void* Inno.Native.MiniAudio.MaAllocationCallbacks.OnFree`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L778) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c8e2163351bace51"></a>[`public void* Inno.Native.MiniAudio.MaAllocationCallbacks.OnMalloc`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L776) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-637aa255449c682f"></a>[`public void* Inno.Native.MiniAudio.MaAllocationCallbacks.OnRealloc`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L777) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e50a66b6d59c8bb3"></a>[`public void* Inno.Native.MiniAudio.MaAllocationCallbacks.PUserData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L775) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2e30e67bcc8d3962"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks.MaAllocationCallbacks(void* pUserData = null, void* onMalloc = null, void* onRealloc = null, void* onFree = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L779) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7970250c2558ba17"></a>[`public struct Inno.Native.MiniAudio.MaAllocationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L773) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAllocationCallbacksPtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAllocationCallbacksPtr de61e8232c40109c37baa239cba681c094451430436e8f8ab3590816a5c36bea -->
<!-- api: M:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.#ctor(Inno.Native.MiniAudio.MaAllocationCallbacks*) 431d531d02498eedcf8c86d1b3898644132a8e32eb4047631b8096c3d9683a20 -->
<!-- api: F:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.Handle 5dc06437c1a21487932b72791ae10c6d4457b98af1f034981cb180c613782e67 -->
<!-- api: P:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.IsNull df5509c318330e9856e4f03fb7438902e7ecc2512f1a452e08dfcf20b03edd25 -->
<!-- api: P:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.Null 864aaea2added2a19fb33969fb9e0cca81f29b13327123dce9304daa1e0b4243 -->
<!-- api: P:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.Item(System.Int32) 85d2b8f0d138454cbd972124e3404437b56710bc4dffc74ba44b871271cb579d -->
<!-- api: M:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.op_Implicit(Inno.Native.MiniAudio.MaAllocationCallbacks*)~Inno.Native.MiniAudio.MaAllocationCallbacksPtr dba2ed34c1284e9fe702e4805e68fca83178f9d5b5b2371b7b564ba3456be148 -->
<!-- api: M:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.op_Implicit(Inno.Native.MiniAudio.MaAllocationCallbacksPtr)~Inno.Native.MiniAudio.MaAllocationCallbacks* 1a1eb61d8e6122537aaa1b07182a32c6f694a33c8c146be08a58ce000baf0aad -->
<!-- api: M:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.op_Equality(Inno.Native.MiniAudio.MaAllocationCallbacksPtr,Inno.Native.MiniAudio.MaAllocationCallbacksPtr) 688d06e9146f31f83aecf54eb5e0db07ec7397dd63f5b04bd9fb37ef8912cadc -->
<!-- api: M:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.op_Inequality(Inno.Native.MiniAudio.MaAllocationCallbacksPtr,Inno.Native.MiniAudio.MaAllocationCallbacksPtr) 21e0d1a80011669610468292019db20c99b37fcdf0b5fb340b53e678dc1f2d03 -->
<!-- api: M:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.Equals(Inno.Native.MiniAudio.MaAllocationCallbacksPtr) 2387da3b43b254dbec9291438f233944eb4701ec7cd12382099833b9de3f0548 -->
<!-- api: M:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.Equals(System.Object) d4a0cfa797524589d290d357cfd5935090c0b3a105ac5fbe24268fcdc85f3e1d -->
<!-- api: M:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.GetHashCode f12d6395fec9d605c58583d97c8006d4a2a6b36a858e957712c9d4d6ad0e03fe -->
<!-- api: P:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.PUserData 9e239aed0584d37d501798b1263212df3b084e73d439e1216cc0b841fb966303 -->
<!-- api: P:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.OnMalloc c72486c8178c85ef0231de45d7c42d71d922143f4551009e64524c6b195d5fce -->
<!-- api: P:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.OnRealloc c81188aaab6a678d8c76d02ece253ae0359c6f49fde6f63d0be48d1ae35e52f7 -->
<!-- api: P:Inno.Native.MiniAudio.MaAllocationCallbacksPtr.OnFree d5f4a3cff2e0cffb4a2ec2f7aac02f60330cd045dca059b120bd3082f21c2d5f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1d2200ba580efd24"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks* Inno.Native.MiniAudio.MaAllocationCallbacksPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14360) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-44d71818baf712cc"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacksPtr.MaAllocationCallbacksPtr(Inno.Native.MiniAudio.MaAllocationCallbacks* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14355) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-12b053c3d63baa53"></a>[`public bool Inno.Native.MiniAudio.MaAllocationCallbacksPtr.Equals(Inno.Native.MiniAudio.MaAllocationCallbacksPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14370) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-5e70bb5f0bdd8e0b"></a>[`public override bool Inno.Native.MiniAudio.MaAllocationCallbacksPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14371) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-d56fa39ca5aa2c5a"></a>[`public override int Inno.Native.MiniAudio.MaAllocationCallbacksPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14372) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-fcd51a4ea0f0d45e"></a>[`public static bool Inno.Native.MiniAudio.MaAllocationCallbacksPtr.operator ==(Inno.Native.MiniAudio.MaAllocationCallbacksPtr left, Inno.Native.MiniAudio.MaAllocationCallbacksPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14368) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b65133620c1e68fb"></a>[`public static Inno.Native.MiniAudio.MaAllocationCallbacksPtr.implicit operator Inno.Native.MiniAudio.MaAllocationCallbacksPtr(Inno.Native.MiniAudio.MaAllocationCallbacks* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14366) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e0b4457d20ae3637"></a>[`public static Inno.Native.MiniAudio.MaAllocationCallbacksPtr.implicit operator Inno.Native.MiniAudio.MaAllocationCallbacks*(Inno.Native.MiniAudio.MaAllocationCallbacksPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14367) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c33e5879a90bbd9c"></a>[`public static bool Inno.Native.MiniAudio.MaAllocationCallbacksPtr.operator !=(Inno.Native.MiniAudio.MaAllocationCallbacksPtr left, Inno.Native.MiniAudio.MaAllocationCallbacksPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14369) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-404c5d8d17bdb401"></a>[`public bool Inno.Native.MiniAudio.MaAllocationCallbacksPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14361) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-94152b5a52c7ea8c"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaAllocationCallbacksPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14364) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8ea1bcee918f8dbe"></a>[`public static Inno.Native.MiniAudio.MaAllocationCallbacksPtr Inno.Native.MiniAudio.MaAllocationCallbacksPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14362) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba69059e4e70533c"></a>[`public void* Inno.Native.MiniAudio.MaAllocationCallbacksPtr.OnFree { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14377) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-77c1fdb3c2941b03"></a>[`public void* Inno.Native.MiniAudio.MaAllocationCallbacksPtr.OnMalloc { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14375) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c8b83c68c51ab3da"></a>[`public void* Inno.Native.MiniAudio.MaAllocationCallbacksPtr.OnRealloc { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14376) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-90439400d169d7c5"></a>[`public void* Inno.Native.MiniAudio.MaAllocationCallbacksPtr.PUserData { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14374) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-775d7e92314b6770"></a>[`public struct Inno.Native.MiniAudio.MaAllocationCallbacksPtr : System.IEquatable<Inno.Native.MiniAudio.MaAllocationCallbacksPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14353) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAsyncNotification</code> · 1 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAsyncNotification d4bc68930f3b17ef4b24f9a8cd00a60847ca70bd44882c8003217a9b6d2dd9ba -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9eff462919358df4"></a>[`public struct Inno.Native.MiniAudio.MaAsyncNotification`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11465) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAsyncNotificationCallbacks</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAsyncNotificationCallbacks 21530b02ddb500cc9b9dff4029903fd25c8a185e76f14f530764a64d8688ab89 -->
<!-- api: F:Inno.Native.MiniAudio.MaAsyncNotificationCallbacks.OnSignal de3d8294ce92bc0b5261540b1b59b4753bc2ce756690ea9ec6b53bf0068e39ab -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationCallbacks.#ctor(System.Void*) 42feffb6cc73486c45eb0d82bdf8570bef45287ae76f3e8d5b73528ec2545258 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-da270ab00185c146"></a>[`public void* Inno.Native.MiniAudio.MaAsyncNotificationCallbacks.OnSignal`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11478) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-27bd85f6dda8e8a1"></a>[`public Inno.Native.MiniAudio.MaAsyncNotificationCallbacks.MaAsyncNotificationCallbacks(void* onSignal = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11479) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-509905e05727f162"></a>[`public struct Inno.Native.MiniAudio.MaAsyncNotificationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11476) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAsyncNotificationEvent</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAsyncNotificationEvent e6807295b067f3c0ff3557685512f69d84669ba80380cfdef479fdf1587a33e8 -->
<!-- api: F:Inno.Native.MiniAudio.MaAsyncNotificationEvent.Cb 964b5492c2f19714f827a78d13d2ef657b4d30e6ef91b62b2ad856929746ae55 -->
<!-- api: F:Inno.Native.MiniAudio.MaAsyncNotificationEvent.E 702863fc427e3f40735caacd9db4cbfa3225b0d92f8548c7d6c0f9351a9cae8e -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationEvent.#ctor(Inno.Native.MiniAudio.MaAsyncNotificationCallbacks,System.IntPtr) 03df4b55406d0eb04ed8cb3ab2d1e60bf7df628a987a7f94180f518a5b75ac18 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-bdd3863cc5009117"></a>[`public Inno.Native.MiniAudio.MaAsyncNotificationCallbacks Inno.Native.MiniAudio.MaAsyncNotificationEvent.Cb`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11506) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e731159d93d4ab74"></a>[`public nint Inno.Native.MiniAudio.MaAsyncNotificationEvent.E`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11507) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-21bb676b5233e5c5"></a>[`public Inno.Native.MiniAudio.MaAsyncNotificationEvent.MaAsyncNotificationEvent(Inno.Native.MiniAudio.MaAsyncNotificationCallbacks cb = default(Inno.Native.MiniAudio.MaAsyncNotificationCallbacks), nint e = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11508) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-942c94ec30d1f40f"></a>[`public struct Inno.Native.MiniAudio.MaAsyncNotificationEvent`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11504) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAsyncNotificationEventPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr f09b7a86fac01563264fdcea7c1f4046500d17bb4f0ca479c916f7f7b4251119 -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.#ctor(Inno.Native.MiniAudio.MaAsyncNotificationEvent*) d87d2f921f073353779166b69a8d4227f49385dde8eb2ad11bb5df927416d3e5 -->
<!-- api: F:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.Handle bdb154f8a935108bd2b0e46c272989e7909650a249a14144ef575bb558dc2b4a -->
<!-- api: P:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.IsNull ca3abb2e262d08b751e84f7698aa332fd7adb66df5c5f5e7ffd5ba98141418b5 -->
<!-- api: P:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.Null fa4c575a9c8ad052a9b39c7e6d00a95632a14ffa6a15e4d285ecc3aa7c2a2a5a -->
<!-- api: P:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.Item(System.Int32) 0c010806ac618ce0e5d2eec0f142f68dc8cb618b7a875ce9a17c3d2dd3adff5d -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.op_Implicit(Inno.Native.MiniAudio.MaAsyncNotificationEvent*)~Inno.Native.MiniAudio.MaAsyncNotificationEventPtr 8d76a29448a4eb31d468ae5088f136a368aa0b9c61d504fc7ca48c301d7786b2 -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.op_Implicit(Inno.Native.MiniAudio.MaAsyncNotificationEventPtr)~Inno.Native.MiniAudio.MaAsyncNotificationEvent* 08f0d23da1d0a04c8bb302da7d44d8233537553fd6bb8a18c1036d836ee4c4ff -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.op_Equality(Inno.Native.MiniAudio.MaAsyncNotificationEventPtr,Inno.Native.MiniAudio.MaAsyncNotificationEventPtr) 30333345f6623d92447e0175ab4272d2a747d1e96ddf3fc1687de0bdf9aacfb1 -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.op_Inequality(Inno.Native.MiniAudio.MaAsyncNotificationEventPtr,Inno.Native.MiniAudio.MaAsyncNotificationEventPtr) a8f776ccbfe97c5ccae779fe888d0e73ceb10c736076d88a315960dc9e8790e9 -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.Equals(Inno.Native.MiniAudio.MaAsyncNotificationEventPtr) 31c9df41b08334d2b005a73da2961cfb1b810c023a38b0d8195ef8307b3c4fdf -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.Equals(System.Object) 5de3461799a1d3959aded694408d46118bc9c9a797a1490486e11a1047b554e7 -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.GetHashCode 1f2c0da899ab2b519731966db716dc8b277c9253a22bd8a032b50d93c108e8b5 -->
<!-- api: P:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.Cb 11db507c297c93e56cdd28c47ce2406c2587136ab46169b85b0565fb8092014b -->
<!-- api: P:Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.E 1e585cab6adaffa48f4c20e54cd222982d982a3b548803cc9f484089d6d7b17d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-90e0eb3210c1aa32"></a>[`public Inno.Native.MiniAudio.MaAsyncNotificationEvent* Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14388) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3778e8c89d1e237a"></a>[`public Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.MaAsyncNotificationEventPtr(Inno.Native.MiniAudio.MaAsyncNotificationEvent* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14383) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bfdf308d60a971a6"></a>[`public bool Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.Equals(Inno.Native.MiniAudio.MaAsyncNotificationEventPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14398) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-722a6ca5eca509ad"></a>[`public override bool Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14399) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-8c5df7e49d6bca07"></a>[`public override int Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14400) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-b384b59875a5efaf"></a>[`public static bool Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.operator ==(Inno.Native.MiniAudio.MaAsyncNotificationEventPtr left, Inno.Native.MiniAudio.MaAsyncNotificationEventPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14396) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b0beaf68bf6ba785"></a>[`public static Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.implicit operator Inno.Native.MiniAudio.MaAsyncNotificationEventPtr(Inno.Native.MiniAudio.MaAsyncNotificationEvent* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14394) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9b2576c06842da5f"></a>[`public static Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.implicit operator Inno.Native.MiniAudio.MaAsyncNotificationEvent*(Inno.Native.MiniAudio.MaAsyncNotificationEventPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14395) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-167059ad486ead17"></a>[`public static bool Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.operator !=(Inno.Native.MiniAudio.MaAsyncNotificationEventPtr left, Inno.Native.MiniAudio.MaAsyncNotificationEventPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14397) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-25f667435b26be0f"></a>[`public ref Inno.Native.MiniAudio.MaAsyncNotificationCallbacks Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.Cb { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14402) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-04cf6ab995e6cdaa"></a>[`public ref nint Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.E { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14403) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6cc9ac4d673f365d"></a>[`public bool Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14389) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-350604344a6577b1"></a>[`public Inno.Native.MiniAudio.MaAsyncNotificationEvent Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14392) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cde56873b163891f"></a>[`public static Inno.Native.MiniAudio.MaAsyncNotificationEventPtr Inno.Native.MiniAudio.MaAsyncNotificationEventPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14390) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7283858d3b71bce3"></a>[`public struct Inno.Native.MiniAudio.MaAsyncNotificationEventPtr : System.IEquatable<Inno.Native.MiniAudio.MaAsyncNotificationEventPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14381) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAsyncNotificationPoll</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAsyncNotificationPoll a6095e30cfce32947b74cd82d76ab828685970efeade9acf8317da66aeff6d68 -->
<!-- api: F:Inno.Native.MiniAudio.MaAsyncNotificationPoll.Cb 9089e634b74d6050583bb03573433e7c9178adb94c3e0e3e89eefe1cb867bd2c -->
<!-- api: F:Inno.Native.MiniAudio.MaAsyncNotificationPoll.Signalled 561cab618cfb7527f1c05bf0116a30a4d08d7e7007ec3a9b2444b0031ae6b14b -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationPoll.#ctor(Inno.Native.MiniAudio.MaAsyncNotificationCallbacks,System.UInt32) b6bdd7a1edd06b7387e7002448774425026b4528bb68a58c692fbeb480ff5260 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f10779b131ccc28c"></a>[`public Inno.Native.MiniAudio.MaAsyncNotificationCallbacks Inno.Native.MiniAudio.MaAsyncNotificationPoll.Cb`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11491) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-841adfe9d0c66e00"></a>[`public uint Inno.Native.MiniAudio.MaAsyncNotificationPoll.Signalled`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11492) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07f3ff6e5c19019a"></a>[`public Inno.Native.MiniAudio.MaAsyncNotificationPoll.MaAsyncNotificationPoll(Inno.Native.MiniAudio.MaAsyncNotificationCallbacks cb = default(Inno.Native.MiniAudio.MaAsyncNotificationCallbacks), uint signalled = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11493) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-edfa3b7c95bf1e71"></a>[`public struct Inno.Native.MiniAudio.MaAsyncNotificationPoll`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11489) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAsyncNotificationPollPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr bb197f196006529983db2b3aa98e3bd4aacf312f5b5656adcb352fd541c89587 -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.#ctor(Inno.Native.MiniAudio.MaAsyncNotificationPoll*) 342c51d8e613b18c75083c1602479f99df3f605c05cd33458344e50cc94f8b0e -->
<!-- api: F:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.Handle 97f513619914ae64f9b7117dc62b8346bfdd6190eee3ebd6e763b122b7de85c3 -->
<!-- api: P:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.IsNull b977367904f2a3a707f9d8fa904e9d3c59d1e6752862b00889f8b7e49e1558fc -->
<!-- api: P:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.Null e609ff2a394a981f19b2c5ac27eb9401efef5a3700f42e6c81791bd6d4090029 -->
<!-- api: P:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.Item(System.Int32) 8e124736645f12099e5396f19ee0161ff2ff90cfca869a26ecd0cd2ca782f4cf -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.op_Implicit(Inno.Native.MiniAudio.MaAsyncNotificationPoll*)~Inno.Native.MiniAudio.MaAsyncNotificationPollPtr 8ac048bc39998b6b1846f28b8fb81906743f8962ea60a446eb5127f06bfa0fcb -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.op_Implicit(Inno.Native.MiniAudio.MaAsyncNotificationPollPtr)~Inno.Native.MiniAudio.MaAsyncNotificationPoll* a5544f28546c55f82660538e879a5feea7346fc5c532838411c483cd36a9153d -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.op_Equality(Inno.Native.MiniAudio.MaAsyncNotificationPollPtr,Inno.Native.MiniAudio.MaAsyncNotificationPollPtr) 54db6c097ac3138f59f4404aa19c05feb5f41c0f3cff4376ff31d7b605db4d32 -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.op_Inequality(Inno.Native.MiniAudio.MaAsyncNotificationPollPtr,Inno.Native.MiniAudio.MaAsyncNotificationPollPtr) 8823a4d9e613eb5e08824bd32f61c427dcfc0a46e8447436a2e8263defffec6f -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.Equals(Inno.Native.MiniAudio.MaAsyncNotificationPollPtr) edbb4db43c669eb9661a648cc31c0e2fe6e530097a92baad2855427fdcfaf774 -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.Equals(System.Object) f3579e0c570e5c7ee860585909154ac0242aedaf68350cd7bbd242b092767ef1 -->
<!-- api: M:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.GetHashCode 252df42b10ab664a622a7259eed37f48b2417db6a22512cef9b6b3e08cb7e909 -->
<!-- api: P:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.Cb 45e6a0fce6cb82446a3c9d2c1ad063f2228f681f96ab49ed8b53d68b1ee9095a -->
<!-- api: P:Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.Signalled 23c7e8eac4f61cbda6d2a16b6b79adbbd6bbdd5850a89606941d3b50d0d3fc68 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a8e92fceb88f93bf"></a>[`public Inno.Native.MiniAudio.MaAsyncNotificationPoll* Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14414) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e62605e2249689ed"></a>[`public Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.MaAsyncNotificationPollPtr(Inno.Native.MiniAudio.MaAsyncNotificationPoll* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14409) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0d2abf2fc4da33f3"></a>[`public bool Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.Equals(Inno.Native.MiniAudio.MaAsyncNotificationPollPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14424) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-d64bf2b558dc8f3d"></a>[`public override bool Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14425) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-8133a499c8a88078"></a>[`public override int Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14426) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-8a248b425e625d68"></a>[`public static bool Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.operator ==(Inno.Native.MiniAudio.MaAsyncNotificationPollPtr left, Inno.Native.MiniAudio.MaAsyncNotificationPollPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14422) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d5e6f401e6e85cd0"></a>[`public static Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.implicit operator Inno.Native.MiniAudio.MaAsyncNotificationPollPtr(Inno.Native.MiniAudio.MaAsyncNotificationPoll* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14420) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-eec9feee612eee93"></a>[`public static Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.implicit operator Inno.Native.MiniAudio.MaAsyncNotificationPoll*(Inno.Native.MiniAudio.MaAsyncNotificationPollPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14421) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5ccb08510ddc994d"></a>[`public static bool Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.operator !=(Inno.Native.MiniAudio.MaAsyncNotificationPollPtr left, Inno.Native.MiniAudio.MaAsyncNotificationPollPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14423) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dee22e191ea7d40d"></a>[`public ref Inno.Native.MiniAudio.MaAsyncNotificationCallbacks Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.Cb { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14428) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e0a9c8d230be2979"></a>[`public bool Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14415) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-66b3338cbe4369a5"></a>[`public Inno.Native.MiniAudio.MaAsyncNotificationPoll Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14418) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-25d41be49944e0dc"></a>[`public static Inno.Native.MiniAudio.MaAsyncNotificationPollPtr Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14416) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-50e6e0597f395e1e"></a>[`public ref uint Inno.Native.MiniAudio.MaAsyncNotificationPollPtr.Signalled { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14429) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ae4930a8c0c89165"></a>[`public struct Inno.Native.MiniAudio.MaAsyncNotificationPollPtr : System.IEquatable<Inno.Native.MiniAudio.MaAsyncNotificationPollPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14407) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAtomicBool32</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAtomicBool32 a3c1c39d2fa309f938992d373612e2b5a6ba3d832ba5eddca98b572ee92b99c6 -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicBool32.Value dc720aef214c6dc55cbfa177303d520083fd3130639961e08ff1b286a6fb996b -->
<!-- api: M:Inno.Native.MiniAudio.MaAtomicBool32.#ctor(System.UInt32) cae46939dab77f39a81db810c4f5fefd86014adfdc59378d608d68bec24fd830 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-af6a7e4c54322ab3"></a>[`public uint Inno.Native.MiniAudio.MaAtomicBool32.Value`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5733) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39b3bde0daa4039d"></a>[`public Inno.Native.MiniAudio.MaAtomicBool32.MaAtomicBool32(uint value = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5734) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b234b625c68207fc"></a>[`public struct Inno.Native.MiniAudio.MaAtomicBool32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5731) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAtomicDeviceState</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAtomicDeviceState 1989cb84a7cf475e67dc80a365fe060ce6049564460a2123e3f624d18ae53cb1 -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicDeviceState.Value 5c70151e8cbc8512f3e3a7f0c0ff21258642c6890626c94a35aff77b573a36f0 -->
<!-- api: M:Inno.Native.MiniAudio.MaAtomicDeviceState.#ctor(Inno.Native.MiniAudio.MaDeviceState) 8f8ff8606cc0cde176c186cb6f057852d3df1aa8a269072635060e8a6c1a4139 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f8a6a08cbc0e6ad7"></a>[`public Inno.Native.MiniAudio.MaDeviceState Inno.Native.MiniAudio.MaAtomicDeviceState.Value`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5098) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b33db4ecbc7cbece"></a>[`public Inno.Native.MiniAudio.MaAtomicDeviceState.MaAtomicDeviceState(Inno.Native.MiniAudio.MaDeviceState value = Inno.Native.MiniAudio.MaDeviceState.Uninitialized)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5099) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-614a8496e1d1d83a"></a>[`public struct Inno.Native.MiniAudio.MaAtomicDeviceState`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5096) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAtomicFloat</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAtomicFloat e3adb06bc1d6c4348308bf684f589c393ded83e2e4a0145a63e21ff4a3c5c824 -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicFloat.Value 893012d54cbecbf22ae83fe807498782ebfc1bde69bf3767d9e459b8ce948439 -->
<!-- api: M:Inno.Native.MiniAudio.MaAtomicFloat.#ctor(System.Single) 80fb8ef13d2c484b1653880702e2f44ba70483c68ef5c8bcb49575777308c54c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d27fd76fc672b6bb"></a>[`public float Inno.Native.MiniAudio.MaAtomicFloat.Value`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5196) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a152bfc37552cefd"></a>[`public Inno.Native.MiniAudio.MaAtomicFloat.MaAtomicFloat(float value = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5197) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1c8a3e88b48fb3fa"></a>[`public struct Inno.Native.MiniAudio.MaAtomicFloat`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5194) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAtomicInt32</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAtomicInt32 3dd309326e85c67705d612887c0de31d28a3ce663efcc4077923286cea85691d -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicInt32.Value b8b9ebd52251e7d62c3a07d150c7bc5f82d926371e8704b94b6c19138114edca -->
<!-- api: M:Inno.Native.MiniAudio.MaAtomicInt32.#ctor(System.Int32) bdd31759dcc3abd0b95ca85f83c707832bee9f2ea0e9bb52c149b7014fa31f1c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e3db8edac1c2555a"></a>[`public int Inno.Native.MiniAudio.MaAtomicInt32.Value`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10454) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cc993ca631ccacaa"></a>[`public Inno.Native.MiniAudio.MaAtomicInt32.MaAtomicInt32(int value = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10455) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8d7bbbe9674e9f2c"></a>[`public struct Inno.Native.MiniAudio.MaAtomicInt32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10452) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAtomicUint32</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAtomicUint32 14baa45802cd252fc6849acfce3a61d762c79963981c73aa7e116b765225cd7a -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicUint32.Value 8542759536b5f16a8b382dc6fb462ca7357cf4fede215075a5a5435a5f3a2ace -->
<!-- api: M:Inno.Native.MiniAudio.MaAtomicUint32.#ctor(System.UInt32) df981e850b56248d61f617f0b1e6d14228dfac344fef26b2a441aa3f690d9c1f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a008dd9f5ab8cfe8"></a>[`public uint Inno.Native.MiniAudio.MaAtomicUint32.Value`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10441) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6dfb2fa74b234dc0"></a>[`public Inno.Native.MiniAudio.MaAtomicUint32.MaAtomicUint32(uint value = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10442) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bfc1f276513f88c2"></a>[`public struct Inno.Native.MiniAudio.MaAtomicUint32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10439) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAtomicUint64</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAtomicUint64 87fe462f6a4da122ee956cd0442853d4bed4de720f25ca0b221b1f5203530ce6 -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicUint64.Value 8b1043aa919775537dd7b8d302bc4ca75e14b2c8fc270a9c2c9308643a2297e0 -->
<!-- api: M:Inno.Native.MiniAudio.MaAtomicUint64.#ctor(System.UInt64) f1de514eadadcf3a541131001aaee3c8b2f1f765b4ed3b7cfaee078cd57089eb -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9c57c8ef0e724f83"></a>[`public ulong Inno.Native.MiniAudio.MaAtomicUint64.Value`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10467) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-32a9ad7112810f38"></a>[`public Inno.Native.MiniAudio.MaAtomicUint64.MaAtomicUint64(ulong value = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10468) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-712c48a66ee1bb2d"></a>[`public struct Inno.Native.MiniAudio.MaAtomicUint64`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10465) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAtomicVec3F</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAtomicVec3F 15ec4141de3485916562abc7b0d929118d4fa987b7e5bfbefc78c4b3fb59502c -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicVec3F.V 3b0eed5b6ce3ec46464bbdba86cb22fc758b449c74718808307830e71bc541a9 -->
<!-- api: F:Inno.Native.MiniAudio.MaAtomicVec3F.Lock ea5cede993debad6506da257c66654165fb29e5a22c79e7883fd487a0efd8f7f -->
<!-- api: M:Inno.Native.MiniAudio.MaAtomicVec3F.#ctor(Inno.Native.MiniAudio.MaVec3F,System.UInt32) a19562331d4c00c418c842c7497853ce544c24aa51e2e2bcee9e42f1b60d039b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-114bd647ba431bae"></a>[`public uint Inno.Native.MiniAudio.MaAtomicVec3F.Lock`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11054) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-68ca621f448462b7"></a>[`public Inno.Native.MiniAudio.MaVec3F Inno.Native.MiniAudio.MaAtomicVec3F.V`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11053) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dbe5a7fb1527aae8"></a>[`public Inno.Native.MiniAudio.MaAtomicVec3F.MaAtomicVec3F(Inno.Native.MiniAudio.MaVec3F v = default(Inno.Native.MiniAudio.MaVec3F), uint @lock = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11055) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ff31fc9d555dba8f"></a>[`public struct Inno.Native.MiniAudio.MaAtomicVec3F`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11051) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAttenuationModel</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAttenuationModel 9912b4567915c337c24b96d0d25b66ae36695da29dac846e74bbc33d9fbf01ba -->
<!-- api: F:Inno.Native.MiniAudio.MaAttenuationModel.None 45aea2c3497e0df19ae9b3e7584427b9408e215d2fc57d299938681d0196a74e -->
<!-- api: F:Inno.Native.MiniAudio.MaAttenuationModel.Inverse 71ca1bd427b240a248e4205e83b6ea4c2c192fc6bb4b06698b38e303cdc40902 -->
<!-- api: F:Inno.Native.MiniAudio.MaAttenuationModel.Linear 154ca8e1e7f6478f17c059a6814f72413df34a478485a2373f2690a19ee2b6d1 -->
<!-- api: F:Inno.Native.MiniAudio.MaAttenuationModel.Exponential 108650337f81b6bbdac0a15c83147225d06541ec728cab7fb52a9190d1a84604 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7afc2a6e7e6032ab"></a>[`Inno.Native.MiniAudio.MaAttenuationModel.Exponential = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L549) | 当前常量或枚举值：`3`。 | — |
| <a id="api-f6e2e4d7a7250372"></a>[`Inno.Native.MiniAudio.MaAttenuationModel.Inverse = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L547) | 当前常量或枚举值：`1`。 | — |
| <a id="api-31ba02bc1d5ecdcd"></a>[`Inno.Native.MiniAudio.MaAttenuationModel.Linear = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L548) | 当前常量或枚举值：`2`。 | — |
| <a id="api-7d0f3a48ac98dabf"></a>[`Inno.Native.MiniAudio.MaAttenuationModel.None = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L546) | 当前常量或枚举值：`0`。 | — |
| <a id="api-7e332f6848527feb"></a>[`public enum Inno.Native.MiniAudio.MaAttenuationModel : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L544) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAudioBuffer</code> · 6 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAudioBuffer eb515c74fe8f4ea418ca15cc12f30040d641499476dce228757f52905a63687f -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBuffer.Ref 66ac32dadd92355f549e24702eee235591691f4a560d5f81b7ec7cae010c235a -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBuffer.AllocationCallbacks a9e16c68f46ba27fc60ff26f01a07880f369dad6af9d64a457e88458728c9404 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBuffer.OwnsData 7c2fc8f85e9a096c73daa420b321157604540d06be1a76bf6323ffc1e2b87f28 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBuffer.Pextradata 9c1bf501538ccf3b05c6a84d1b102f0013a46708efe284765144e0e7588a63e5 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBuffer.#ctor(Inno.Native.MiniAudio.MaAudioBufferRef,Inno.Native.MiniAudio.MaAllocationCallbacks,System.UInt32,System.Byte*) 66bdc3a3c99015e1fefca038497b333be3938fb3fb77c7828cebb1a13155d609 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5777ff0e91789cee"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaAudioBuffer.AllocationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11358) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-885321d5e8323c1d"></a>[`public uint Inno.Native.MiniAudio.MaAudioBuffer.OwnsData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11359) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bebb4eff184101a5"></a>[`public byte Inno.Native.MiniAudio.MaAudioBuffer.Pextradata`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11360) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c105df0cbbb308be"></a>[`public Inno.Native.MiniAudio.MaAudioBufferRef Inno.Native.MiniAudio.MaAudioBuffer.Ref`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11357) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-31ccef356dd3cb14"></a>[`public Inno.Native.MiniAudio.MaAudioBuffer.MaAudioBuffer(Inno.Native.MiniAudio.MaAudioBufferRef @ref = default(Inno.Native.MiniAudio.MaAudioBufferRef), Inno.Native.MiniAudio.MaAllocationCallbacks allocationCallbacks = default(Inno.Native.MiniAudio.MaAllocationCallbacks), uint ownsData = 0, byte* pExtraData = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11361) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-90adcd1428e540ba"></a>[`public struct Inno.Native.MiniAudio.MaAudioBuffer`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11355) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAudioBufferConfig</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAudioBufferConfig 28b05886c3547baba4af02509ab4608365553cf4496b781414e73ce57fce74e0 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferConfig.Format 84447d12cf61c921c1ffd30769d6b57338e7112215c15bfeb618482ad82ce098 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferConfig.Channels fa872b5568832ed17c833c051d14c10fb5c6fa517569d827da80e0d62b3614b3 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferConfig.SampleRate 15ffab6667b3b27cad25f0802bedad0db662fd707bf7af6bac8b0db9def516b1 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferConfig.SizeInFrames 07e4cff8403fbb51bf6ce5efb7aa76fd3c7244b170736fd89205f37aedae37be -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferConfig.PData 8e53b5eca8cf89223140f867517374e95eb305b0bf8f3128a94a36d5887bb456 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferConfig.AllocationCallbacks 4fee3ef4135266cff9abc9a2978435c3fd2522ad11d87b45feefc8281a1f2fb0 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferConfig.#ctor(Inno.Native.MiniAudio.MaFormat,System.UInt32,System.UInt32,System.UInt64,System.Void*,Inno.Native.MiniAudio.MaAllocationCallbacks) bc49dfdefb5f81cedea2dde01fe73d99ebc938acfc0798dbe6b3dd42a7744772 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f2a1c7db6d0a189d"></a>[`public Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaAudioBufferConfig.AllocationCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11339) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1edb8821d9a0e7d4"></a>[`public uint Inno.Native.MiniAudio.MaAudioBufferConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11335) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c38678dcd2d6970a"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaAudioBufferConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11334) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-471a7013de785799"></a>[`public void* Inno.Native.MiniAudio.MaAudioBufferConfig.PData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11338) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0f7e904b564a95e3"></a>[`public uint Inno.Native.MiniAudio.MaAudioBufferConfig.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11336) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e255526c0fcc8659"></a>[`public ulong Inno.Native.MiniAudio.MaAudioBufferConfig.SizeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11337) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ab9ebafac03f270c"></a>[`public Inno.Native.MiniAudio.MaAudioBufferConfig.MaAudioBufferConfig(Inno.Native.MiniAudio.MaFormat format = Inno.Native.MiniAudio.MaFormat.Unknown, uint channels = 0, uint sampleRate = 0, ulong sizeInFrames = 0, void* pData = null, Inno.Native.MiniAudio.MaAllocationCallbacks allocationCallbacks = default(Inno.Native.MiniAudio.MaAllocationCallbacks))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11340) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-5ad5f59e561894b4"></a>[`public struct Inno.Native.MiniAudio.MaAudioBufferConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11332) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAudioBufferConfigPtr</code> · 19 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAudioBufferConfigPtr 9b4d517dd13fa5815b90ec33ebe80c0031fbb45e0842d8b1d8c0c54562cd4971 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.#ctor(Inno.Native.MiniAudio.MaAudioBufferConfig*) ecf9aa101a8501360d5fe91dc900f58a3ddaa4eb1abd25cf71e9cea0bb1e9407 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.Handle daa33994a6bb23d5c85603e9733f3cbbe046ffe1e2f7579863dc6f886f4d4c80 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.IsNull 532e7e3331ba8f1bb2425a8d39fa7cb3b3080dfc121f7e6e44b081b78d1e1635 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.Null 139a31d5a297bb8b026041551f55e06bd2f7b6ecea910767a2494848c0d0d732 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.Item(System.Int32) e9830d8fcb2236b4f94c665bb71d01cd5216d46c03b1d49c24aa705c81e61ba8 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaAudioBufferConfig*)~Inno.Native.MiniAudio.MaAudioBufferConfigPtr 0f43fc49d318ea3dc6343e0fe64f4f40523ceec6544caaa47bbe91bd5bd0fd87 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaAudioBufferConfigPtr)~Inno.Native.MiniAudio.MaAudioBufferConfig* de179c657b07c79a12291c269023448e0271d153bac33893a29d5338a89e6335 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.op_Equality(Inno.Native.MiniAudio.MaAudioBufferConfigPtr,Inno.Native.MiniAudio.MaAudioBufferConfigPtr) 0498385e3462827c3fc744f39f987dfd5639646afe6e760166f276ab9b3c2b53 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaAudioBufferConfigPtr,Inno.Native.MiniAudio.MaAudioBufferConfigPtr) bac0cc99675e987670762632ba5e5a868800d5214f3bcdfe9007912ed53ca140 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.Equals(Inno.Native.MiniAudio.MaAudioBufferConfigPtr) ae53c37fe112908e1a6b17b03a9cd49499105af6e0b9641e8857c29afaadad89 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.Equals(System.Object) 10f89b74798541033220656ea1cfda76aa3c7f578b056617f289b6be0884de6d -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.GetHashCode f7a5433c872456120984a06b092021ee84ee0a0c673daa47fdb1706064642954 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.Format 6a5cb2b83fd15efe88b3bee36338510972670e79ee9a72d55f606cbd5f4a938d -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.Channels de8dbbd2807290bc4d3fcb12611d92e8d1a01ad380bd30b6bed82548bc31222f -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.SampleRate 5f5c3b84d8bfb351e21689c89688d2a866fc423c5bfc93811f67fa8907dad880 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.SizeInFrames 679b1a3f12623e637916daeb55317a7137e4a390e99ad4a9cd95f78188d58862 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.PData 71deacac2ed92a1ba753d37cfb9d062db663deab6f80881013842b969164ec8b -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferConfigPtr.AllocationCallbacks b3255e292ef87873be2b9c841af35ba81401a70f32008144e8c25b10f9c1517b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0f966d5962b55810"></a>[`public Inno.Native.MiniAudio.MaAudioBufferConfig* Inno.Native.MiniAudio.MaAudioBufferConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14440) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-34f0977402343c3b"></a>[`public Inno.Native.MiniAudio.MaAudioBufferConfigPtr.MaAudioBufferConfigPtr(Inno.Native.MiniAudio.MaAudioBufferConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14435) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-458d7c254cbfb1e0"></a>[`public bool Inno.Native.MiniAudio.MaAudioBufferConfigPtr.Equals(Inno.Native.MiniAudio.MaAudioBufferConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14450) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-7d4a0b036a4ab590"></a>[`public override bool Inno.Native.MiniAudio.MaAudioBufferConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14451) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-045c5af1ced4df38"></a>[`public override int Inno.Native.MiniAudio.MaAudioBufferConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14452) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-c8691e6432bc42d1"></a>[`public static bool Inno.Native.MiniAudio.MaAudioBufferConfigPtr.operator ==(Inno.Native.MiniAudio.MaAudioBufferConfigPtr left, Inno.Native.MiniAudio.MaAudioBufferConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14448) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-18fd125f1664f392"></a>[`public static Inno.Native.MiniAudio.MaAudioBufferConfigPtr.implicit operator Inno.Native.MiniAudio.MaAudioBufferConfigPtr(Inno.Native.MiniAudio.MaAudioBufferConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14446) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-943d29021d05aad5"></a>[`public static Inno.Native.MiniAudio.MaAudioBufferConfigPtr.implicit operator Inno.Native.MiniAudio.MaAudioBufferConfig*(Inno.Native.MiniAudio.MaAudioBufferConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14447) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4fe953a91ea88744"></a>[`public static bool Inno.Native.MiniAudio.MaAudioBufferConfigPtr.operator !=(Inno.Native.MiniAudio.MaAudioBufferConfigPtr left, Inno.Native.MiniAudio.MaAudioBufferConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14449) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-04fe6b362386f45a"></a>[`public ref Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaAudioBufferConfigPtr.AllocationCallbacks { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14459) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5214b45fbf46bf8e"></a>[`public ref uint Inno.Native.MiniAudio.MaAudioBufferConfigPtr.Channels { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14455) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2e0334b02f9c3614"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaAudioBufferConfigPtr.Format { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14454) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c37c10faa5366f41"></a>[`public bool Inno.Native.MiniAudio.MaAudioBufferConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14441) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-66407bcbb4fa4c04"></a>[`public Inno.Native.MiniAudio.MaAudioBufferConfig Inno.Native.MiniAudio.MaAudioBufferConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14444) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3f0d3cfee55ccf2a"></a>[`public static Inno.Native.MiniAudio.MaAudioBufferConfigPtr Inno.Native.MiniAudio.MaAudioBufferConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14442) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8c4426e0e564de63"></a>[`public void* Inno.Native.MiniAudio.MaAudioBufferConfigPtr.PData { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14458) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9a2ca831c9b5e34c"></a>[`public ref uint Inno.Native.MiniAudio.MaAudioBufferConfigPtr.SampleRate { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14456) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4d0b8a0fab859b99"></a>[`public ref ulong Inno.Native.MiniAudio.MaAudioBufferConfigPtr.SizeInFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14457) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dd0271ba6b2aee3f"></a>[`public struct Inno.Native.MiniAudio.MaAudioBufferConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaAudioBufferConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14433) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAudioBufferPtr</code> · 17 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAudioBufferPtr b60cb7438a22d12e3d024fbef2156d570de90921c20ee2204daa20e4a8808a1b -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtr.#ctor(Inno.Native.MiniAudio.MaAudioBuffer*) da514195ddbd3928ddd2844d18df13a85ecfdb802ea72e876a5432f8b319f4f3 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferPtr.Handle 83f7b7418f70b4b83d96e62ab4057d3d073d74929df4fda1c74d6e66c20d2ad4 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferPtr.IsNull fed97a49209f32c490a418398bab2ac31928be5fc5a3bbc9da6a2918a4bc641c -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferPtr.Null 0875ee2946fa1367e49e3cf9093c24e7f8c4642a08b665ee44a0f42f5bd94cdc -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferPtr.Item(System.Int32) 51a83a49cbe520934aca2832fff4eda94d2ea95ab40293cffdc8ec571a9f264a -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtr.op_Implicit(Inno.Native.MiniAudio.MaAudioBuffer*)~Inno.Native.MiniAudio.MaAudioBufferPtr 0b06d8b5e26f0e1de65c4e9f470a6dab6039e30a5e3afffc3c55d0db3f040364 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtr.op_Implicit(Inno.Native.MiniAudio.MaAudioBufferPtr)~Inno.Native.MiniAudio.MaAudioBuffer* 744b3b69c756fd07e84c7aea5a76f95c1d549ebaca59ddbb094efae61aadc347 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtr.op_Equality(Inno.Native.MiniAudio.MaAudioBufferPtr,Inno.Native.MiniAudio.MaAudioBufferPtr) 03778feb0ffb721b4bb9761d3140c1b567ec3d13df51ce905a5ebcbe4605bdbb -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtr.op_Inequality(Inno.Native.MiniAudio.MaAudioBufferPtr,Inno.Native.MiniAudio.MaAudioBufferPtr) 8834f6b20310b8f5472d875ca3144d4d357e7a25ffcde294e9283e09fdd571ee -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtr.Equals(Inno.Native.MiniAudio.MaAudioBufferPtr) 0edf1327e8518ceabc3f6660e1acf86a50f4da34fa7f9cafc28bc61678da04d0 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtr.Equals(System.Object) 9b7f851d6e93fc7fe44af8e22de99a91ec68e34676a6ba9d1a7e7a5141f778ef -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtr.GetHashCode 3de4ba636b5ec4667efdd702c9431b8c9ec49b5e03720af3353a3aa556707058 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferPtr.Ref 89d43d4b4fc42274bd7e03aaf2adbf9fe46c5e0a3e2bd5da9c7b00ce065d9092 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferPtr.AllocationCallbacks 2727639d0ea69c17493ac4ae3365b9a238de2b758b835156076b181489ee85b7 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferPtr.OwnsData 784a5f3ad520ef649402a26e039cdea07a7d20358e54dd6edb5fac285a04e4c6 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferPtr.Pextradata b6d264b4679dc647fbb731c4d42b310a045a8f6a21dcae86158f3e8132d9fa42 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-af11de3f2fd0f5bf"></a>[`public Inno.Native.MiniAudio.MaAudioBuffer* Inno.Native.MiniAudio.MaAudioBufferPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14470) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c87fe9d60e958bdd"></a>[`public Inno.Native.MiniAudio.MaAudioBufferPtr.MaAudioBufferPtr(Inno.Native.MiniAudio.MaAudioBuffer* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14465) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-30f8a910c2978f20"></a>[`public bool Inno.Native.MiniAudio.MaAudioBufferPtr.Equals(Inno.Native.MiniAudio.MaAudioBufferPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14480) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-f567e45b7943a5a0"></a>[`public override bool Inno.Native.MiniAudio.MaAudioBufferPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14481) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-addc37809d3262e2"></a>[`public override int Inno.Native.MiniAudio.MaAudioBufferPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14482) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-70af3cc4e260ba34"></a>[`public static bool Inno.Native.MiniAudio.MaAudioBufferPtr.operator ==(Inno.Native.MiniAudio.MaAudioBufferPtr left, Inno.Native.MiniAudio.MaAudioBufferPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14478) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3b46717c022b658c"></a>[`public static Inno.Native.MiniAudio.MaAudioBufferPtr.implicit operator Inno.Native.MiniAudio.MaAudioBufferPtr(Inno.Native.MiniAudio.MaAudioBuffer* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14476) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c07e4374f4d4e29b"></a>[`public static Inno.Native.MiniAudio.MaAudioBufferPtr.implicit operator Inno.Native.MiniAudio.MaAudioBuffer*(Inno.Native.MiniAudio.MaAudioBufferPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14477) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3938c21e2bf53f17"></a>[`public static bool Inno.Native.MiniAudio.MaAudioBufferPtr.operator !=(Inno.Native.MiniAudio.MaAudioBufferPtr left, Inno.Native.MiniAudio.MaAudioBufferPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14479) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f0947cd676666e5b"></a>[`public ref Inno.Native.MiniAudio.MaAllocationCallbacks Inno.Native.MiniAudio.MaAudioBufferPtr.AllocationCallbacks { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14485) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ec35fa8bae377416"></a>[`public bool Inno.Native.MiniAudio.MaAudioBufferPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14471) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6ffbf6c0654a8f4b"></a>[`public Inno.Native.MiniAudio.MaAudioBuffer Inno.Native.MiniAudio.MaAudioBufferPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14474) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-218d2b6e6edeea2c"></a>[`public static Inno.Native.MiniAudio.MaAudioBufferPtr Inno.Native.MiniAudio.MaAudioBufferPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14472) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e8bc50bc5921ee94"></a>[`public ref uint Inno.Native.MiniAudio.MaAudioBufferPtr.OwnsData { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14486) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a2ebab3f7c5facea"></a>[`public System.Span<byte> Inno.Native.MiniAudio.MaAudioBufferPtr.Pextradata { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14487) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-17d2dcb13066c424"></a>[`public ref Inno.Native.MiniAudio.MaAudioBufferRef Inno.Native.MiniAudio.MaAudioBufferPtr.Ref { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14484) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d1a97ec454224df4"></a>[`public struct Inno.Native.MiniAudio.MaAudioBufferPtr : System.IEquatable<Inno.Native.MiniAudio.MaAudioBufferPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14463) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAudioBufferPtrPtr</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAudioBufferPtrPtr abe03f6907d6066f0ecdd1aae6d951a29f99fde13859e57fe5872a60cdcae5fa -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtrPtr.#ctor(Inno.Native.MiniAudio.MaAudioBuffer**) 3447338edf2efd2e29eee711299fe322324561f77e046ced1d405f124c4b42ab -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferPtrPtr.Handle 4d6710db3fe1e16b54d9e2d5b4a96c186717bcc1f5d307969fcf2492ed0779c8 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferPtrPtr.IsNull c6ee5b5c742cd619fd46f7263a19da5eb15b0e242d0a8160296b7f2e75f74462 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferPtrPtr.Null b5cbd0f58e9d6333898899f16f2e2a23f908f335b345d6d694737073285a4bf0 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferPtrPtr.Item(System.Int32) a5166a4e908ee90191f9826df65d873c60ad41a116d73b93c4f45f40d96587b8 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtrPtr.op_Implicit(Inno.Native.MiniAudio.MaAudioBuffer**)~Inno.Native.MiniAudio.MaAudioBufferPtrPtr eb6b08aa89f66b342850fcae7c85ba579346e9d4bd499f8f8189f8ed5a9ebe8b -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtrPtr.op_Implicit(Inno.Native.MiniAudio.MaAudioBufferPtrPtr)~Inno.Native.MiniAudio.MaAudioBuffer** f2f69d3b4bcd3fef445d97b955c2643c6df8b2c57f35fc49756502d14c328cd3 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtrPtr.op_Equality(Inno.Native.MiniAudio.MaAudioBufferPtrPtr,Inno.Native.MiniAudio.MaAudioBufferPtrPtr) 5d595667201c3d5ee9ffd3b56443b6901e31f8cb15c57b6f6005e57db4433702 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtrPtr.op_Inequality(Inno.Native.MiniAudio.MaAudioBufferPtrPtr,Inno.Native.MiniAudio.MaAudioBufferPtrPtr) 371d247d666ccfc1745b02a24c84b04e2cc6c6309434e9783c47fc3f0b4c6950 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtrPtr.Equals(Inno.Native.MiniAudio.MaAudioBufferPtrPtr) 25ab21b99a7bcd962de6b108cf436bfab0618d718e8559d53d5d908872f656da -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtrPtr.Equals(System.Object) 32459ea276d3a38098696371511fb1774642a01c1ce93973717bb0cebe5e13a5 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferPtrPtr.GetHashCode f8b1a9e556fcba1d514cc74edf6dd307100b84788b0665ec01d44d59f43dc9ba -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-35475b6ca733d5f3"></a>[`public Inno.Native.MiniAudio.MaAudioBuffer** Inno.Native.MiniAudio.MaAudioBufferPtrPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14498) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4cf2182c072372e1"></a>[`public Inno.Native.MiniAudio.MaAudioBufferPtrPtr.MaAudioBufferPtrPtr(Inno.Native.MiniAudio.MaAudioBuffer** handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14493) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0d2e66ae50176c3e"></a>[`public bool Inno.Native.MiniAudio.MaAudioBufferPtrPtr.Equals(Inno.Native.MiniAudio.MaAudioBufferPtrPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14508) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-f9033ec90d3897b8"></a>[`public override bool Inno.Native.MiniAudio.MaAudioBufferPtrPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14509) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-8e8d9054c2dc15c8"></a>[`public override int Inno.Native.MiniAudio.MaAudioBufferPtrPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14510) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-a15b3453959d192a"></a>[`public static bool Inno.Native.MiniAudio.MaAudioBufferPtrPtr.operator ==(Inno.Native.MiniAudio.MaAudioBufferPtrPtr left, Inno.Native.MiniAudio.MaAudioBufferPtrPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14506) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a6d2389464a83bf7"></a>[`public static Inno.Native.MiniAudio.MaAudioBufferPtrPtr.implicit operator Inno.Native.MiniAudio.MaAudioBufferPtrPtr(Inno.Native.MiniAudio.MaAudioBuffer** handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14504) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f6815040a771afd3"></a>[`public static Inno.Native.MiniAudio.MaAudioBufferPtrPtr.implicit operator Inno.Native.MiniAudio.MaAudioBuffer**(Inno.Native.MiniAudio.MaAudioBufferPtrPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14505) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0539852d1336232a"></a>[`public static bool Inno.Native.MiniAudio.MaAudioBufferPtrPtr.operator !=(Inno.Native.MiniAudio.MaAudioBufferPtrPtr left, Inno.Native.MiniAudio.MaAudioBufferPtrPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14507) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2a8c8d7db05ddc8c"></a>[`public bool Inno.Native.MiniAudio.MaAudioBufferPtrPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14499) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f082f70c8e4132e5"></a>[`public Inno.Native.MiniAudio.MaAudioBuffer* Inno.Native.MiniAudio.MaAudioBufferPtrPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14502) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eb995ab20661c18c"></a>[`public static Inno.Native.MiniAudio.MaAudioBufferPtrPtr Inno.Native.MiniAudio.MaAudioBufferPtrPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14500) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-913f1167111b0635"></a>[`public struct Inno.Native.MiniAudio.MaAudioBufferPtrPtr : System.IEquatable<Inno.Native.MiniAudio.MaAudioBufferPtrPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14491) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAudioBufferRef</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAudioBufferRef 88ddd7872c72ef48bc0e3462a338f5697c9be67a1a676a1d36eeebea34637bf5 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.Ds 0ed73fa4d7fdc223f9c68e12473e6a761c0a1ee1f4c127d44bc3f7c5290b24f5 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.Format 193363daf7065dcdf73899211cf56190ae8fe2c0e9321c8f30ac783f738f2e10 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.Channels c384bb7e9dfee4a84019a5a7c2931bc0a4a7612998cbf8efd141e7809c93055e -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.SampleRate 2b1cf46bb2717257a64976670071c668fd99cc0582372d201b4d9551b3272369 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.Cursor 13979587330223bf3212d469c67b16414ceb824efc8e90caa7d38a137e807fb8 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.SizeInFrames d8c5bc2c9809bcf608ec25f7dc2e83d29e8164ccac6a354dd8a3876dff29df7b -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRef.PData afae3499f162142efc6bcfd8a377678a63573d7eb798a86def974fdff9b96454 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferRef.#ctor(Inno.Native.MiniAudio.MaDataSourceBase,Inno.Native.MiniAudio.MaFormat,System.UInt32,System.UInt32,System.UInt64,System.UInt64,System.Void*) b8106e69b078c3458730009d5e322522146f9e10c251bc2a16599fbeaed22b69 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-90b78726e7d97301"></a>[`public uint Inno.Native.MiniAudio.MaAudioBufferRef.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11311) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-046d3342978d613d"></a>[`public ulong Inno.Native.MiniAudio.MaAudioBufferRef.Cursor`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11313) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1469f179600f2e17"></a>[`public Inno.Native.MiniAudio.MaDataSourceBase Inno.Native.MiniAudio.MaAudioBufferRef.Ds`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11309) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7feca9adc0e4f676"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaAudioBufferRef.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11310) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-91ecaebc68a5deb5"></a>[`public void* Inno.Native.MiniAudio.MaAudioBufferRef.PData`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11315) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4142dfb0d88aefc0"></a>[`public uint Inno.Native.MiniAudio.MaAudioBufferRef.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11312) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03d4f77ba9b27955"></a>[`public ulong Inno.Native.MiniAudio.MaAudioBufferRef.SizeInFrames`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11314) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-026a931c36856a4e"></a>[`public Inno.Native.MiniAudio.MaAudioBufferRef.MaAudioBufferRef(Inno.Native.MiniAudio.MaDataSourceBase ds = default(Inno.Native.MiniAudio.MaDataSourceBase), Inno.Native.MiniAudio.MaFormat format = Inno.Native.MiniAudio.MaFormat.Unknown, uint channels = 0, uint sampleRate = 0, ulong cursor = 0, ulong sizeInFrames = 0, void* pData = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11316) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ad51a19e9634118e"></a>[`public struct Inno.Native.MiniAudio.MaAudioBufferRef`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L11307) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaAudioBufferRefPtr</code> · 20 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaAudioBufferRefPtr 1e9eaeca7d2ad752e808f11d34e065f6c6a529c33bc59ddc9c631c5220ab462c -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferRefPtr.#ctor(Inno.Native.MiniAudio.MaAudioBufferRef*) 5454d7ffafaf3454d4c907edc15a531293213f1f0707e825e234f5ab4cb62cd0 -->
<!-- api: F:Inno.Native.MiniAudio.MaAudioBufferRefPtr.Handle 5e8579c44d02707d4b944886bc4a8140ede6f1e66d0e58b0b4e6e6bfa601377d -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferRefPtr.IsNull d40b682ac1cd3c4f31b9621ffa5bb58b180e1fba6a9ad736c3bcccf12d6608b7 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferRefPtr.Null d982b82ebd5fff355f35f0bef71ca641f733ac787dd41736d65dd77eb7145f24 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferRefPtr.Item(System.Int32) dcdd8da6ddf71f7e8c1d044abe2f83e9c17b3cf547e9e5728373b25caf453754 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferRefPtr.op_Implicit(Inno.Native.MiniAudio.MaAudioBufferRef*)~Inno.Native.MiniAudio.MaAudioBufferRefPtr bf399640506d699d31ffff96b687cee3b0c683780585972c5b04c48bcd48b252 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferRefPtr.op_Implicit(Inno.Native.MiniAudio.MaAudioBufferRefPtr)~Inno.Native.MiniAudio.MaAudioBufferRef* 7b757c41c3a497b4b2ae5c2da77369b2690448b3e22d1ae8c911b0567200c449 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferRefPtr.op_Equality(Inno.Native.MiniAudio.MaAudioBufferRefPtr,Inno.Native.MiniAudio.MaAudioBufferRefPtr) ac05b0e10725b016884da445dd52385d76abd43b7e2b5f3c073b290af9d92d93 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferRefPtr.op_Inequality(Inno.Native.MiniAudio.MaAudioBufferRefPtr,Inno.Native.MiniAudio.MaAudioBufferRefPtr) b3f225e5376e70558027fe2f3cc3591b767e3ea33980621279e5093a66ecb223 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferRefPtr.Equals(Inno.Native.MiniAudio.MaAudioBufferRefPtr) a8b7d221ae04c23fea9c35a3b4edb7a6d00301c69046d4174ca041719f793c35 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferRefPtr.Equals(System.Object) c9703d25bc8142211524d2e8c4853bda0ff5e73636f8fd2837453404e59d2c08 -->
<!-- api: M:Inno.Native.MiniAudio.MaAudioBufferRefPtr.GetHashCode f12fda22cae45fc160ed1161b5fc0eab7a9ecb0dce9fb0a65e47a73ed15dd5a4 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferRefPtr.Ds 5a448edda19880ba0dd289575c91ee54b38511bb967c5397209d1da4000de453 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferRefPtr.Format 6f8804ef67f076c323b6fb1d937f44b7433ec12e88df452b2a84157db660a32c -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferRefPtr.Channels 4fe1f7d0f5a9ea9d40d2172376bbea7a3dcb99291d30960f985e1e6d90f30cc8 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferRefPtr.SampleRate 3d9b4933af93fcc8490359a9d4b67254eac7df48a95bc11b860deae1f24c7765 -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferRefPtr.Cursor 8c1a1290fd2f3c3166e3423fbab6589314b054e871d010f1121de5c95d4c6dea -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferRefPtr.SizeInFrames e819f1e3af019284860cbcfd2ce9a7afbc2d90850027a1de3fb057f69fde8c0b -->
<!-- api: P:Inno.Native.MiniAudio.MaAudioBufferRefPtr.PData 8812cdb9df699f8a5f1442aa0680069a42cdf8f427be42b1bdd6a11cd6f83951 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-617bf387fe77c89c"></a>[`public Inno.Native.MiniAudio.MaAudioBufferRef* Inno.Native.MiniAudio.MaAudioBufferRefPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14522) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ffa449e39fc4c90"></a>[`public Inno.Native.MiniAudio.MaAudioBufferRefPtr.MaAudioBufferRefPtr(Inno.Native.MiniAudio.MaAudioBufferRef* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14517) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-3d983b46d67c6edd"></a>[`public bool Inno.Native.MiniAudio.MaAudioBufferRefPtr.Equals(Inno.Native.MiniAudio.MaAudioBufferRefPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14532) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-1c8ad067c61f605b"></a>[`public override bool Inno.Native.MiniAudio.MaAudioBufferRefPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14533) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-f18bb17851affff9"></a>[`public override int Inno.Native.MiniAudio.MaAudioBufferRefPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14534) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-8668f660681d6dc6"></a>[`public static bool Inno.Native.MiniAudio.MaAudioBufferRefPtr.operator ==(Inno.Native.MiniAudio.MaAudioBufferRefPtr left, Inno.Native.MiniAudio.MaAudioBufferRefPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14530) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a339814d277672ac"></a>[`public static Inno.Native.MiniAudio.MaAudioBufferRefPtr.implicit operator Inno.Native.MiniAudio.MaAudioBufferRefPtr(Inno.Native.MiniAudio.MaAudioBufferRef* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14528) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1055673f419be4a2"></a>[`public static Inno.Native.MiniAudio.MaAudioBufferRefPtr.implicit operator Inno.Native.MiniAudio.MaAudioBufferRef*(Inno.Native.MiniAudio.MaAudioBufferRefPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14529) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a588da84f2b95057"></a>[`public static bool Inno.Native.MiniAudio.MaAudioBufferRefPtr.operator !=(Inno.Native.MiniAudio.MaAudioBufferRefPtr left, Inno.Native.MiniAudio.MaAudioBufferRefPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14531) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1019b6baa4cf7f9f"></a>[`public ref uint Inno.Native.MiniAudio.MaAudioBufferRefPtr.Channels { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14538) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6ef5b82ff30537e2"></a>[`public ref ulong Inno.Native.MiniAudio.MaAudioBufferRefPtr.Cursor { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14540) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e18e56a3c9a5639a"></a>[`public ref Inno.Native.MiniAudio.MaDataSourceBase Inno.Native.MiniAudio.MaAudioBufferRefPtr.Ds { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14536) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1c68fe78cab34b9c"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaAudioBufferRefPtr.Format { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14537) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-edced1f4373776d8"></a>[`public bool Inno.Native.MiniAudio.MaAudioBufferRefPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14523) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4a64431804fcdab2"></a>[`public Inno.Native.MiniAudio.MaAudioBufferRef Inno.Native.MiniAudio.MaAudioBufferRefPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14526) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-98c289bc8ded761b"></a>[`public static Inno.Native.MiniAudio.MaAudioBufferRefPtr Inno.Native.MiniAudio.MaAudioBufferRefPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14524) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1fdfc9d83def0b8a"></a>[`public void* Inno.Native.MiniAudio.MaAudioBufferRefPtr.PData { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14542) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0148cce62cc2c2a4"></a>[`public ref uint Inno.Native.MiniAudio.MaAudioBufferRefPtr.SampleRate { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14539) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fecfc64a227f84ff"></a>[`public ref ulong Inno.Native.MiniAudio.MaAudioBufferRefPtr.SizeInFrames { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14541) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b8dc6e3e9a6442e1"></a>[`public struct Inno.Native.MiniAudio.MaAudioBufferRefPtr : System.IEquatable<Inno.Native.MiniAudio.MaAudioBufferRefPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14515) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBackend</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBackend 9eedf87c7e2100c1c21a32fdefeeaa831442dc1112a7463722362ac33960f0e8 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Wasapi d1b3ad76b17728cfca40d6ac8c13105c7345d2a812dd260322a78dc5c0ed8564 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Dsound e89048902e77d5551b41cc620ce7e617a8a939fd69e4836a6d117bd3a011cad3 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Winmm e62ac5905a90a7111bfbddc4527b3b336d005a94f3881b5b5305b428ad786228 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Coreaudio e355d37f8fa579ca0bc1243fca6b12901305adeeccda41cd7c26a8a96118f133 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Sndio 9356d48c44a289af0b05a309fbfb2e14a5bb0e0f9995da179dbac2f5be832ffb -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Audio4 c8e43a0e45c1ef43a229968d3e3b4052e0c71171fc4128b9ad81399ee0935c24 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Oss 5966e5d7920097ceb5ee1676d0fd9956a97947cfb6d0aa4801151537815ccc14 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Pulseaudio 96577e737b40062a4f1f11a7a2c74d2d3f4d98cc0d02f1a528ee5ee2958ef0ce -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Alsa 8f157844675d5321e11ad9e8e30b1bb69998b853a5f62a627b370e8e1cf75436 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Jack d215fae736246b06e490bfa729217f83aeb3cb0f1ac57862e8d97612a3443937 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Aaudio b74ec12446e761c339743a8a1b8b570e91be3ab62c89de83df1ad133fb52e972 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Opensl 91b92b6ed841afbc2990df6682656d3f8cf6ede954c90ed9b917edd77e9acf42 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Webaudio 701cf139ba99119df32bbee1fca8450256dee19c391f253784fe7407215fb81c -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Custom 42f5a11d34da280a3b768283082227c9a0a7793993c8afeaa49ef57faef89b24 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackend.Null 44e9ae46a2d1a41c69122c78be4c72a7e557cc060ad423784adbb177804cbf49 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c81df24206d851ea"></a>[`Inno.Native.MiniAudio.MaBackend.Aaudio = 10`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L406) | 当前常量或枚举值：`10`。 | — |
| <a id="api-5e2e76dad72ad8ba"></a>[`Inno.Native.MiniAudio.MaBackend.Alsa = 8`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L404) | 当前常量或枚举值：`8`。 | — |
| <a id="api-48f1856f115788d9"></a>[`Inno.Native.MiniAudio.MaBackend.Audio4 = 5`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L401) | 当前常量或枚举值：`5`。 | — |
| <a id="api-dce508898adc256a"></a>[`Inno.Native.MiniAudio.MaBackend.Coreaudio = 3`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L399) | 当前常量或枚举值：`3`。 | — |
| <a id="api-e05670ee930f78fb"></a>[`Inno.Native.MiniAudio.MaBackend.Custom = 13`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L409) | 当前常量或枚举值：`13`。 | — |
| <a id="api-b13311b8aabbdd76"></a>[`Inno.Native.MiniAudio.MaBackend.Dsound = 1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L397) | 当前常量或枚举值：`1`。 | — |
| <a id="api-f59c5fa2be34c9c2"></a>[`Inno.Native.MiniAudio.MaBackend.Jack = 9`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L405) | 当前常量或枚举值：`9`。 | — |
| <a id="api-028e5af78ac15c19"></a>[`Inno.Native.MiniAudio.MaBackend.Null = 14`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L410) | 当前常量或枚举值：`14`。 | — |
| <a id="api-151cb8b17de2e036"></a>[`Inno.Native.MiniAudio.MaBackend.Opensl = 11`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L407) | 当前常量或枚举值：`11`。 | — |
| <a id="api-b8c05489e1438eb7"></a>[`Inno.Native.MiniAudio.MaBackend.Oss = 6`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L402) | 当前常量或枚举值：`6`。 | — |
| <a id="api-ae6d858022ed5807"></a>[`Inno.Native.MiniAudio.MaBackend.Pulseaudio = 7`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L403) | 当前常量或枚举值：`7`。 | — |
| <a id="api-2dd5de779a44f7c8"></a>[`Inno.Native.MiniAudio.MaBackend.Sndio = 4`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L400) | 当前常量或枚举值：`4`。 | — |
| <a id="api-0d1ce238b7b1b31b"></a>[`Inno.Native.MiniAudio.MaBackend.Wasapi = 0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L396) | 当前常量或枚举值：`0`。 | — |
| <a id="api-929c874caef63f0e"></a>[`Inno.Native.MiniAudio.MaBackend.Webaudio = 12`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L408) | 当前常量或枚举值：`12`。 | — |
| <a id="api-ff6cf327f329ecbc"></a>[`Inno.Native.MiniAudio.MaBackend.Winmm = 2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L398) | 当前常量或枚举值：`2`。 | — |
| <a id="api-bb961f8477c4d264"></a>[`public enum Inno.Native.MiniAudio.MaBackend : int`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L394) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBackendCallbacks</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBackendCallbacks 82bf91cf40cd3a3b3febf6f35edfd1c57f4ec07c1f157f60bcb93e759c3cd77c -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnContextInit f9dcdb3bb4a8e09dd43a4d96222bb771f6e013e150d2642b79c73eeeccb3e3d7 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnContextUninit 9cb8dceb8f468a0dd1e4790015764acc5f77ee5a2f6034bcee9c17260b19d534 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnContextEnumerateDevices 332ff869a453a85357eae96d91a3024eb6b61394bf750551019c54d18fce7c7c -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnContextGetDeviceInfo a3d93198a3f14fe045bce0759d87dce4bf277074d54377a5d03d8b287af6ec70 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceInit bfb144803fa4482cd0f0178fc98b7d582c536c1acbc2ae1c3f9926e44ec24ac9 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceUninit e440cfa11df1e8a1606a47b212fa4a2ba372a80c7379a643f94ca52feeb8a945 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceStart 36b1d6a07249c2063d734e1885031a7bf426bae6b91cc4609b3f28b56853dbd3 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceStop 8fde69148e63433c3da0651327e805d14b34986066a9cdb741c23d4c785faa95 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceRead aede4a6eab1bb7ded90d3835c4ffcf1d13bff964122f3878db1b48b3c9a14b7c -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceWrite 02e870a1afae5f7eba0b8714a88b1c3435d165018c4b7aebef8189be016dc913 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceDataLoop 8dd08f8595fcbef3d5b95c3bb2b717760592658b769fe35e41d2aea728769a1c -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceDataLoopWakeup a2e1e05778decb4cbed06e4ca7db6ca9a07ce4a5b5e398dac249a9bf669bf3b7 -->
<!-- api: F:Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceGetInfo 1929595d465586983d99231996c71d467f1c804d524cea5f50d764c1359b3492 -->
<!-- api: M:Inno.Native.MiniAudio.MaBackendCallbacks.#ctor(System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*,System.Void*) 66c9b8b63d8a5d8bfde4818addae45810012e54547be858362a636d14bf2f704 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0b722c6c357091f1"></a>[`public void* Inno.Native.MiniAudio.MaBackendCallbacks.OnContextEnumerateDevices`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10022) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c59169761e8480c8"></a>[`public void* Inno.Native.MiniAudio.MaBackendCallbacks.OnContextGetDeviceInfo`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10023) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9236703c4584431d"></a>[`public void* Inno.Native.MiniAudio.MaBackendCallbacks.OnContextInit`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10020) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d0ec56713da2b9e1"></a>[`public void* Inno.Native.MiniAudio.MaBackendCallbacks.OnContextUninit`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10021) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2c98ff7afcb8defc"></a>[`public void* Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceDataLoop`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10030) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-904942369558e9fe"></a>[`public void* Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceDataLoopWakeup`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10031) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c781e404bad636d0"></a>[`public void* Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceGetInfo`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10032) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39b08a31960db357"></a>[`public void* Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceInit`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10024) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9a57f4db463e2bab"></a>[`public void* Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceRead`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10028) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-265551867d74f1a4"></a>[`public void* Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceStart`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10026) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d68db46c0a105e00"></a>[`public void* Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceStop`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10027) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-994313f80a1c75d2"></a>[`public void* Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceUninit`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10025) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c8e957bb087921a6"></a>[`public void* Inno.Native.MiniAudio.MaBackendCallbacks.OnDeviceWrite`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10029) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-748cc9ee58cc0c60"></a>[`public Inno.Native.MiniAudio.MaBackendCallbacks.MaBackendCallbacks(void* onContextInit = null, void* onContextUninit = null, void* onContextEnumerateDevices = null, void* onContextGetDeviceInfo = null, void* onDeviceInit = null, void* onDeviceUninit = null, void* onDeviceStart = null, void* onDeviceStop = null, void* onDeviceRead = null, void* onDeviceWrite = null, void* onDeviceDataLoop = null, void* onDeviceDataLoopWakeup = null, void* onDeviceGetInfo = null)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10033) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-589c1903cb7aeb32"></a>[`public struct Inno.Native.MiniAudio.MaBackendCallbacks`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10018) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBiquad</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBiquad 9906eaa3a9447c6a02ec575979cccc91532b904e1f7921e1fe64656fbfa8b884 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.Format 617a5d57db485c60cb98f5024408c383bbac34d4808f9b77aad74834f696532f -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.Channels fa727f3dd4f0b3b75705e60fdf89322df7b51f57e141bccd721422ee613a1b30 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.B0 d6722a153e84e63b172bfe9f08c96a7e02d88ea8e5eb8ab1af1b30c371295da7 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.B1 fecc599048b841b93fceb3f1d9a5434475bd229982d0b71bbdfdd7b3762fe817 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.B2 73e5ee32c7ab519a88d87d6e32197548b0f164394c1ee6452e37e346afdc8b1f -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.A1 cb42b80555d205b3bf24a064f04f3b566185715f7f460ae905f1f1afad9c52c6 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.A2 af6ccb581333c293b10086ff649e5a83de733225997e91ec9608d93938e894fc -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.PR1 c90084dffdb480b81555672c47122a700619025d22e7d6cd68d11e3e55f7a647 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.PR2 82af9b550a8e7362c37bcd5e11452b6fe545d2f54370496661e610c324fb13b9 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.Pheap 05ffc3e5a0be32474231e2fb5690e2c1055746cfe8976c67131916911133e293 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquad.Ownsheap 3ac531f00df1e7f73350893a0b7b1eaa3f090c4f2632890a87e82ab0233f6202 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquad.#ctor(Inno.Native.MiniAudio.MaFormat,System.UInt32,Inno.Native.MiniAudio.MaBiquadCoefficient,Inno.Native.MiniAudio.MaBiquadCoefficient,Inno.Native.MiniAudio.MaBiquadCoefficient,Inno.Native.MiniAudio.MaBiquadCoefficient,Inno.Native.MiniAudio.MaBiquadCoefficient,Inno.Native.MiniAudio.MaBiquadCoefficient*,Inno.Native.MiniAudio.MaBiquadCoefficient*,System.Void*,System.UInt32) fdd658ef0793404b0df8d9a169547fe990014de85d48a8b9b037f0e9b8b75eb5 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a5f3a3c9fd4ba169"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquad.A1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5505) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9701bc2d10b29c5c"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquad.A2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5506) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fb0ad7b36ec5b754"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquad.B0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5502) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-773939b4974550f3"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquad.B1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5503) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9d7582951a407e6a"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquad.B2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5504) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-63ee30585af73b89"></a>[`public uint Inno.Native.MiniAudio.MaBiquad.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5501) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-84a2c7a5875a1826"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBiquad.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5500) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-189ba019c84aca20"></a>[`public uint Inno.Native.MiniAudio.MaBiquad.Ownsheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5510) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1832b0fd35849af9"></a>[`public void* Inno.Native.MiniAudio.MaBiquad.Pheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5509) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-89fbe3f6d6f6fcfa"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient* Inno.Native.MiniAudio.MaBiquad.PR1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5507) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c38df7bfa7f0275d"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient* Inno.Native.MiniAudio.MaBiquad.PR2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5508) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-031c3bf8924dbeb7"></a>[`public Inno.Native.MiniAudio.MaBiquad.MaBiquad(Inno.Native.MiniAudio.MaFormat format = Inno.Native.MiniAudio.MaFormat.Unknown, uint channels = 0, Inno.Native.MiniAudio.MaBiquadCoefficient b0 = default(Inno.Native.MiniAudio.MaBiquadCoefficient), Inno.Native.MiniAudio.MaBiquadCoefficient b1 = default(Inno.Native.MiniAudio.MaBiquadCoefficient), Inno.Native.MiniAudio.MaBiquadCoefficient b2 = default(Inno.Native.MiniAudio.MaBiquadCoefficient), Inno.Native.MiniAudio.MaBiquadCoefficient a1 = default(Inno.Native.MiniAudio.MaBiquadCoefficient), Inno.Native.MiniAudio.MaBiquadCoefficient a2 = default(Inno.Native.MiniAudio.MaBiquadCoefficient), Inno.Native.MiniAudio.MaBiquadCoefficient* pR1 = null, Inno.Native.MiniAudio.MaBiquadCoefficient* pR2 = null, void* pHeap = null, uint ownsHeap = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5511) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9966f115b78f8078"></a>[`public struct Inno.Native.MiniAudio.MaBiquad`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5498) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBiquadCoefficient</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBiquadCoefficient 0493f1073875191af902fc9a884ebe083a6048791c8bae5a6c19bf4a31e3ee67 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadCoefficient.F32 0a7f04fe62dee1cfe2810ece51a223d490b168102eafa23ecce1b13a7f81fc1d -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadCoefficient.S32 97678c522ec58d5216952fcdd5616487dddef088f24c7a9e4c42d9d17358552d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3b84aaa71646f6f1"></a>[`public float Inno.Native.MiniAudio.MaBiquadCoefficient.F32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5466) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d695e1c0bd9899a9"></a>[`public int Inno.Native.MiniAudio.MaBiquadCoefficient.S32`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5468) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8c4ca9f2ef70db0c"></a>[`public struct Inno.Native.MiniAudio.MaBiquadCoefficient`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L5463) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBiquadCoefficientPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBiquadCoefficientPtr b0a41eb54841faa7c84f5dc9e9d7a73bdb81f18f35bf8a6baa60059820361cdf -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.#ctor(Inno.Native.MiniAudio.MaBiquadCoefficient*) 81d35d3437539421c8d4b78478e4731cc85ff40e104f941dabcd1bc4ec4fb6b5 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.Handle c3c13a4135aaf183990cdca64b28d6ce0e0b123c7f96d1b1031aee05a42d5fda -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.IsNull 147ae0ca482b3fdf374da7e41d6d2d454045aa98f66ae64c7d344780342ed77a -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.Null b07ec97ec6e1280c141e40cdc300216a4578b3a18acd6a7bf95780411d01537e -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.Item(System.Int32) 5be3f8946f2d99948e806f31a35fe25fa38f7d5b4733e2697b2a2496864e9e6f -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.op_Implicit(Inno.Native.MiniAudio.MaBiquadCoefficient*)~Inno.Native.MiniAudio.MaBiquadCoefficientPtr 61917f68586f4a846f3ad1053752fd365f3d9b4fa64ce587476827c2e9d85100 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.op_Implicit(Inno.Native.MiniAudio.MaBiquadCoefficientPtr)~Inno.Native.MiniAudio.MaBiquadCoefficient* cd1fb7948150c93ba1b5c2014ba4ea94458b73f6fd71fdc37365344ac3e6a6cc -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.op_Equality(Inno.Native.MiniAudio.MaBiquadCoefficientPtr,Inno.Native.MiniAudio.MaBiquadCoefficientPtr) ce0da50eced48f0043b7bec31afd31ab624f5643ce4fe672d3e3c75c99d311f8 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.op_Inequality(Inno.Native.MiniAudio.MaBiquadCoefficientPtr,Inno.Native.MiniAudio.MaBiquadCoefficientPtr) 190bee0fe2b641637bca633e974eae2212671ec1c95707d642439780f839a948 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.Equals(Inno.Native.MiniAudio.MaBiquadCoefficientPtr) 1fa9611f11114d375d362590a752aee0950cf1fe18ce2123e1cf1e85a075ed65 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.Equals(System.Object) 58775cc235257e869e2b8b64dc637c5ee1ad887f63d7504c4e3470046d617786 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.GetHashCode 634f269130156a256620816b5aa9036843e98b51602dbfa5c258e9a239de1c07 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.F32 e1bb072666f000adcfb15ba74675714f0adcb56f0588d7a6c91dd09afccb576c -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadCoefficientPtr.S32 5f14eb694df91e90d660c0205e7dbaf3916107ff0aaebad42de9f52ef73f43cd -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-fa811d38ffd9ad4a"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient* Inno.Native.MiniAudio.MaBiquadCoefficientPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14553) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f89b4df5a9bafd7e"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficientPtr.MaBiquadCoefficientPtr(Inno.Native.MiniAudio.MaBiquadCoefficient* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14548) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-005eb790729f9b0c"></a>[`public bool Inno.Native.MiniAudio.MaBiquadCoefficientPtr.Equals(Inno.Native.MiniAudio.MaBiquadCoefficientPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14563) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-4ded0e65d504d067"></a>[`public override bool Inno.Native.MiniAudio.MaBiquadCoefficientPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14564) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-e163108c260598bf"></a>[`public override int Inno.Native.MiniAudio.MaBiquadCoefficientPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14565) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-eaa390c2a1800d2e"></a>[`public static bool Inno.Native.MiniAudio.MaBiquadCoefficientPtr.operator ==(Inno.Native.MiniAudio.MaBiquadCoefficientPtr left, Inno.Native.MiniAudio.MaBiquadCoefficientPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14561) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-61334c91111315e1"></a>[`public static Inno.Native.MiniAudio.MaBiquadCoefficientPtr.implicit operator Inno.Native.MiniAudio.MaBiquadCoefficientPtr(Inno.Native.MiniAudio.MaBiquadCoefficient* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14559) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6f27e7dd8f63716f"></a>[`public static Inno.Native.MiniAudio.MaBiquadCoefficientPtr.implicit operator Inno.Native.MiniAudio.MaBiquadCoefficient*(Inno.Native.MiniAudio.MaBiquadCoefficientPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14560) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-989b4d34eada436e"></a>[`public static bool Inno.Native.MiniAudio.MaBiquadCoefficientPtr.operator !=(Inno.Native.MiniAudio.MaBiquadCoefficientPtr left, Inno.Native.MiniAudio.MaBiquadCoefficientPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14562) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9193cb9b0049300e"></a>[`public ref float Inno.Native.MiniAudio.MaBiquadCoefficientPtr.F32 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14567) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d07508e07ead1e0a"></a>[`public bool Inno.Native.MiniAudio.MaBiquadCoefficientPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14554) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6c1fd8cf6bbb4333"></a>[`public Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquadCoefficientPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14557) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7e93db7b1348bd21"></a>[`public static Inno.Native.MiniAudio.MaBiquadCoefficientPtr Inno.Native.MiniAudio.MaBiquadCoefficientPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14555) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-885544a1a0967a9a"></a>[`public ref int Inno.Native.MiniAudio.MaBiquadCoefficientPtr.S32 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14568) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a66faf7f61899fe5"></a>[`public struct Inno.Native.MiniAudio.MaBiquadCoefficientPtr : System.IEquatable<Inno.Native.MiniAudio.MaBiquadCoefficientPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14546) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBiquadConfig</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBiquadConfig 8dc912db31a2d7ea77bc621dd12a4024eac68f17f4153c21fe3fd7f636d5b789 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.Format 25d3e4470dc926779c2f7be682e251b1334688bf52c7c575e593049ae221f971 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.Channels 145c10ea89398d1b46671b800eb8964a9da222d1ba712c61e2ef663a7473e5f6 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.B0 fd68adaa472133883676e7c626e0bdfcbc9d7341de9bad53a28c4cc16e938cef -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.B1 f03a68fe8a5619ffb1bad1fc6fe55faaec37f926c2a7c65b6f7e79a7853bc121 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.B2 aa1cb48273ab4207a94757bfe520408c883ee58959d219283d4d950cc1cbbe01 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.A0 4812dd6aa1aaf4b3fca25046732a7a2ce7ca5900d7e35e4a5d9bde730c40e5de -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.A1 4b3a5c0c492bfcda93610d8f36d7467ab29ed5981947f6c3b6ff4377bdf2e4d6 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfig.A2 fa36fdbe3b113a4027a390047cb3040270e5dd1d29acd93507372f1b25b36b08 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadConfig.#ctor(Inno.Native.MiniAudio.MaFormat,System.UInt32,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double) 83618343ebf1d59dd66c1ea05f49f0a15bb042aa70089e3c1ce1be7e617e9039 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-81d4a2a9c16fc39b"></a>[`public double Inno.Native.MiniAudio.MaBiquadConfig.A0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10485) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b0044bfc720c8370"></a>[`public double Inno.Native.MiniAudio.MaBiquadConfig.A1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10486) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b56bb8b570402ab2"></a>[`public double Inno.Native.MiniAudio.MaBiquadConfig.A2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10487) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-44f7bbde79d1e09d"></a>[`public double Inno.Native.MiniAudio.MaBiquadConfig.B0`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10482) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-23860d5205109b4f"></a>[`public double Inno.Native.MiniAudio.MaBiquadConfig.B1`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10483) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4e7cfc62cdd7e8f7"></a>[`public double Inno.Native.MiniAudio.MaBiquadConfig.B2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10484) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4d3c5d6369406f6a"></a>[`public uint Inno.Native.MiniAudio.MaBiquadConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10481) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3548d64f987e1288"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBiquadConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10480) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2c92e666ab35e61a"></a>[`public Inno.Native.MiniAudio.MaBiquadConfig.MaBiquadConfig(Inno.Native.MiniAudio.MaFormat format = Inno.Native.MiniAudio.MaFormat.Unknown, uint channels = 0, double b0 = 0, double b1 = 0, double b2 = 0, double a0 = 0, double a1 = 0, double a2 = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10488) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-91b76f6a32ec474f"></a>[`public struct Inno.Native.MiniAudio.MaBiquadConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10478) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBiquadConfigPtr</code> · 21 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBiquadConfigPtr 206a728f7308ebcf02926f06e19859ddbd5db17c4ab0283e3aed1911d8500d8d -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadConfigPtr.#ctor(Inno.Native.MiniAudio.MaBiquadConfig*) a49406a449c21112e723d6669a457b033abe775123c568c23ac77ce97789e13a -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadConfigPtr.Handle 68ec7efd84b11df039ba166d46de2a32d868807de9ce486bced94f06c6c2ac19 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadConfigPtr.IsNull d59f13c78af46e4e0e943feadf92e406bef44d88a36e5bfb314bb97acedb4371 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadConfigPtr.Null e1ff27b34230a15077949f36f21d04a93e74b2d8329df134bd3bb09561f18539 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadConfigPtr.Item(System.Int32) 2ebe66f63211e1514edff9808c2c0fe375413b8538426cdd5c789831777aa00e -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaBiquadConfig*)~Inno.Native.MiniAudio.MaBiquadConfigPtr 3447ced15b6a5315ea540eb4acb09cd96221f4072ad3bd51d9e82d0a0afab318 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaBiquadConfigPtr)~Inno.Native.MiniAudio.MaBiquadConfig* 8801e4ebea4d3e2d21ed8f2edc92e38d70e65c1cda9941d9442cc74e50bc2eb4 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadConfigPtr.op_Equality(Inno.Native.MiniAudio.MaBiquadConfigPtr,Inno.Native.MiniAudio.MaBiquadConfigPtr) c021d15367172c1b22a3f40b63d120519992da2ea94b5854d20a68c620bfee5a -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaBiquadConfigPtr,Inno.Native.MiniAudio.MaBiquadConfigPtr) 2634931a781724c3bba711d05e92c4c495cc8ba268f130ac4d4d03d00a2b4574 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadConfigPtr.Equals(Inno.Native.MiniAudio.MaBiquadConfigPtr) d81d04bf24125d324eafa85ae6605ea1fa76e63439a797f40037c91def6c3bf0 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadConfigPtr.Equals(System.Object) c647077cd9937ed35c3100a754fde35dd8d2e5ab0dd6c8c35a55114135df119e -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadConfigPtr.GetHashCode 6538108b8e671ce46b73bb36730a3e2cd4690b1950aa0a5992832bb5fd1651c0 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadConfigPtr.Format c40fd9a25bd7b11bc6957154e2dc10fc24369086290f997b872331fe38218c6b -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadConfigPtr.Channels 4b815748ac3fcc4fac8279494293cd3d7537867bc7ce8f84d41f4beb836a62f5 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadConfigPtr.B0 45df81aa0fe6bc63f94b41d054c8e8c65b2bd5320ed6f5e48efbf0b1a72f5fe9 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadConfigPtr.B1 14ad9cbae3f07709f98efd1f45eb191ed53475b3d8a843baadf6403efe6a490b -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadConfigPtr.B2 04760b60c3fea2fa010d767023ab191112ff54f8dce7864897ddfbc85f7ae512 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadConfigPtr.A0 9c6bd7275f44461d48f7defc4f26d37eae5d9c378f6f0bb559e2e7bdf22155fc -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadConfigPtr.A1 d4255282866375443e32727a57d6a459e9ae150721854fd3ccd3f56c5d11bc04 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadConfigPtr.A2 194ba066e39be269e5ccc0e8725bd7cbebec75d3fc4996d7871551d523a6b892 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6740bbd7de50abdf"></a>[`public Inno.Native.MiniAudio.MaBiquadConfig* Inno.Native.MiniAudio.MaBiquadConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14579) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-216c82f3f54e5079"></a>[`public Inno.Native.MiniAudio.MaBiquadConfigPtr.MaBiquadConfigPtr(Inno.Native.MiniAudio.MaBiquadConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14574) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-fdb831f3aa7795a2"></a>[`public bool Inno.Native.MiniAudio.MaBiquadConfigPtr.Equals(Inno.Native.MiniAudio.MaBiquadConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14589) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-5c8e4733f828d802"></a>[`public override bool Inno.Native.MiniAudio.MaBiquadConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14590) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-48fd5b2dde95a46c"></a>[`public override int Inno.Native.MiniAudio.MaBiquadConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14591) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-fb040b5101ef21ea"></a>[`public static bool Inno.Native.MiniAudio.MaBiquadConfigPtr.operator ==(Inno.Native.MiniAudio.MaBiquadConfigPtr left, Inno.Native.MiniAudio.MaBiquadConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14587) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f673b772fa881f43"></a>[`public static Inno.Native.MiniAudio.MaBiquadConfigPtr.implicit operator Inno.Native.MiniAudio.MaBiquadConfigPtr(Inno.Native.MiniAudio.MaBiquadConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14585) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f7aca7293a052954"></a>[`public static Inno.Native.MiniAudio.MaBiquadConfigPtr.implicit operator Inno.Native.MiniAudio.MaBiquadConfig*(Inno.Native.MiniAudio.MaBiquadConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14586) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f2176174ae9d77c0"></a>[`public static bool Inno.Native.MiniAudio.MaBiquadConfigPtr.operator !=(Inno.Native.MiniAudio.MaBiquadConfigPtr left, Inno.Native.MiniAudio.MaBiquadConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14588) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0cfdcb33ab9a5135"></a>[`public ref double Inno.Native.MiniAudio.MaBiquadConfigPtr.A0 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14598) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-868334482d976015"></a>[`public ref double Inno.Native.MiniAudio.MaBiquadConfigPtr.A1 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14599) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dd50083024708739"></a>[`public ref double Inno.Native.MiniAudio.MaBiquadConfigPtr.A2 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14600) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4c32cfd06d9c4bdd"></a>[`public ref double Inno.Native.MiniAudio.MaBiquadConfigPtr.B0 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14595) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-58614fdbceba4011"></a>[`public ref double Inno.Native.MiniAudio.MaBiquadConfigPtr.B1 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14596) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2e0a7cb7452aa117"></a>[`public ref double Inno.Native.MiniAudio.MaBiquadConfigPtr.B2 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14597) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ed8739e47779611e"></a>[`public ref uint Inno.Native.MiniAudio.MaBiquadConfigPtr.Channels { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14594) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3b1f494cb4d1f3b8"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBiquadConfigPtr.Format { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14593) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f44149f903cc789d"></a>[`public bool Inno.Native.MiniAudio.MaBiquadConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14580) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba879295ed508080"></a>[`public Inno.Native.MiniAudio.MaBiquadConfig Inno.Native.MiniAudio.MaBiquadConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14583) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-859e1fc9244051c7"></a>[`public static Inno.Native.MiniAudio.MaBiquadConfigPtr Inno.Native.MiniAudio.MaBiquadConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14581) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9eaa1192ebc3db47"></a>[`public struct Inno.Native.MiniAudio.MaBiquadConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaBiquadConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14572) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBiquadNode</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBiquadNode 6ed27d52a805fbd0f7cd7d38c376b365a68b55a3771ca4aa40df8362b4761733 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadNode.BaseNode 510396e24daaac4b7f226f4ca58635144b0d1013a65a80cb56f3bd22d950b9f9 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadNode.Biquad 56444e1ecc2747b0930bc4d080d29cb4a550a14f0cac3823c6e81dc809d96603 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNode.#ctor(Inno.Native.MiniAudio.MaNodeBase,Inno.Native.MiniAudio.MaBiquad) 2805606e2ef805933dc4fcc0faae46532fa77a90ebf57ff6ea4b869f3b01a882 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a580b0165bc90a2c"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaBiquadNode.BaseNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13284) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-259b664132479ea4"></a>[`public Inno.Native.MiniAudio.MaBiquad Inno.Native.MiniAudio.MaBiquadNode.Biquad`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13285) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-205e9d4c28f1eb55"></a>[`public Inno.Native.MiniAudio.MaBiquadNode.MaBiquadNode(Inno.Native.MiniAudio.MaNodeBase baseNode = default(Inno.Native.MiniAudio.MaNodeBase), Inno.Native.MiniAudio.MaBiquad biquad = default(Inno.Native.MiniAudio.MaBiquad))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13286) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-76b0cc735f190b04"></a>[`public struct Inno.Native.MiniAudio.MaBiquadNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13282) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBiquadNodeConfig</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBiquadNodeConfig 4ceeac2d4b11bcfa6bfa1b9d60c421cbae345a7208de2697373954fa2937300a -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadNodeConfig.NodeConfig 5c6cfc61913ed63463e051ad0bd3758d5280de3ea780a82a6a05d590c606ef93 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadNodeConfig.Biquad 89a7b219e052789f168af4837f623b3588f7e2e123890491cf3707d08ade7722 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodeConfig.#ctor(Inno.Native.MiniAudio.MaNodeConfig,Inno.Native.MiniAudio.MaBiquadConfig) 01eab5d6d27809aefafd73204e86a274915a5afcaeafb3a536ac5def87d3911e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ddca3fd52bb40864"></a>[`public Inno.Native.MiniAudio.MaBiquadConfig Inno.Native.MiniAudio.MaBiquadNodeConfig.Biquad`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13270) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-869425287704ba7d"></a>[`public Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaBiquadNodeConfig.NodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13269) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-30d50c5edde360c6"></a>[`public Inno.Native.MiniAudio.MaBiquadNodeConfig.MaBiquadNodeConfig(Inno.Native.MiniAudio.MaNodeConfig nodeConfig = default(Inno.Native.MiniAudio.MaNodeConfig), Inno.Native.MiniAudio.MaBiquadConfig biquad = default(Inno.Native.MiniAudio.MaBiquadConfig))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13271) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b523ae1a8a4d495e"></a>[`public struct Inno.Native.MiniAudio.MaBiquadNodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13267) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBiquadNodeConfigPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr e175b5ea5d270bd2116285b9b17247804e01cda48c39bb4c206b3e9c9413221a -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.#ctor(Inno.Native.MiniAudio.MaBiquadNodeConfig*) 9b655892bce9dce83814676a9598a60b2216359128fd8569ada9b3180284e151 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.Handle 411020b1cc91598ce49e0d568af3806df1fbc78c49647840fa7bec84baebc4c7 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.IsNull 13e23cb792523493d97254dee49ce4e5fd136d5fad46148b5e1269b7e4d39d5d -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.Null 3fd32cf4b14ce7f1664109d6d4047b888feac3c731b3e7c8c991e4702ffd3e87 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.Item(System.Int32) 1e64567c4f49f230ece3e217b2f078e422bfc5e94b1e3b7e265ced57ab1d0bb5 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaBiquadNodeConfig*)~Inno.Native.MiniAudio.MaBiquadNodeConfigPtr 6b9ab622d2b8ebcc868be6331e4bf6d001e38e0d0c7fd1a6b285606d03b4cfba -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaBiquadNodeConfigPtr)~Inno.Native.MiniAudio.MaBiquadNodeConfig* 4f2c8d0e829446bd9afd8119b8a92f7c0246e7feb25012bee61b4db874c500be -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.op_Equality(Inno.Native.MiniAudio.MaBiquadNodeConfigPtr,Inno.Native.MiniAudio.MaBiquadNodeConfigPtr) 9cf29d0f4c7e439933f76f13230be48f50a2090a7dcb554c816c6421235cba95 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaBiquadNodeConfigPtr,Inno.Native.MiniAudio.MaBiquadNodeConfigPtr) ee31d1a2ff76a58625375c356114167ceb02f9999faa9e2ba9e82a6ddce1b96b -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.Equals(Inno.Native.MiniAudio.MaBiquadNodeConfigPtr) a09f3b6e78f255ff4224804df22afd8e0dec4ba50f6e6e77070322ca53f5e1c1 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.Equals(System.Object) a81a4550dd7d12c86c4af7589f02d2e88e689279965a174158719bb0eaddaf55 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.GetHashCode b1de297823a8e736c90aee1bc6b2f4ba7c516681f6084ddf0884694d32223d6a -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.NodeConfig 2f420639890c397986321146523cbd3a60040238649cd957ca02483425279f1e -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.Biquad 60ef61a7a0528b2c484349a2d7450353f4aa51865a434581eb5e46d6e16214dd -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-73538c5673aa76d5"></a>[`public Inno.Native.MiniAudio.MaBiquadNodeConfig* Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14611) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6dd15c8ab3cc97e4"></a>[`public Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.MaBiquadNodeConfigPtr(Inno.Native.MiniAudio.MaBiquadNodeConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14606) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-23517bfbcc52a5ce"></a>[`public bool Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.Equals(Inno.Native.MiniAudio.MaBiquadNodeConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14621) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-d43ae2e116611c76"></a>[`public override bool Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14622) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-d5bf304145772f0c"></a>[`public override int Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14623) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-08d38b4b6034745b"></a>[`public static bool Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.operator ==(Inno.Native.MiniAudio.MaBiquadNodeConfigPtr left, Inno.Native.MiniAudio.MaBiquadNodeConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14619) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b0f386c44ad2365d"></a>[`public static Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.implicit operator Inno.Native.MiniAudio.MaBiquadNodeConfigPtr(Inno.Native.MiniAudio.MaBiquadNodeConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14617) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2b4be7f05de1088d"></a>[`public static Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.implicit operator Inno.Native.MiniAudio.MaBiquadNodeConfig*(Inno.Native.MiniAudio.MaBiquadNodeConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14618) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bf653389d9f01119"></a>[`public static bool Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.operator !=(Inno.Native.MiniAudio.MaBiquadNodeConfigPtr left, Inno.Native.MiniAudio.MaBiquadNodeConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14620) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0df6f1439752e38d"></a>[`public ref Inno.Native.MiniAudio.MaBiquadConfig Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.Biquad { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14626) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-348e35101a07e30c"></a>[`public bool Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14612) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b7e2e0d9442ff615"></a>[`public Inno.Native.MiniAudio.MaBiquadNodeConfig Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14615) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ffa02d7c232b8bd5"></a>[`public ref Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.NodeConfig { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14625) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-24d5782a2bce97e9"></a>[`public static Inno.Native.MiniAudio.MaBiquadNodeConfigPtr Inno.Native.MiniAudio.MaBiquadNodeConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14613) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8b37539cb3ad7bfb"></a>[`public struct Inno.Native.MiniAudio.MaBiquadNodeConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaBiquadNodeConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14604) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBiquadNodePtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBiquadNodePtr fa08543c4802ed3a89c6188f3697d9df9fb0c8d86ce7c543583f197eb092374e -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodePtr.#ctor(Inno.Native.MiniAudio.MaBiquadNode*) fa070ecae6f6cdb9bb3e2afaa0ba8cca8b80ba375a079aff4ec3dbd71b993ed6 -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadNodePtr.Handle 76c9017ad536441d5cd66768792f52379f1232d4402e84f693c10500e93b920a -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadNodePtr.IsNull 55b4ebaa8712a615cf7e94540255b30434e2f2f21c1ddc5350eb836f0c2faabc -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadNodePtr.Null 410fe5c4b5ab3f5ae343f91a71fe8877b5ecfc0db5ef3edf2ee712ef4a32a196 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadNodePtr.Item(System.Int32) 048def7b7edd2db4dff1349ae4f6c8f5c2cb54a0ba7f0a1d7af4f381ea29425b -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodePtr.op_Implicit(Inno.Native.MiniAudio.MaBiquadNode*)~Inno.Native.MiniAudio.MaBiquadNodePtr 13b69d7ea5683d4ba960324ba79225f1091201feeb16f1dc71df0274838532e5 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodePtr.op_Implicit(Inno.Native.MiniAudio.MaBiquadNodePtr)~Inno.Native.MiniAudio.MaBiquadNode* d1173d11079e6c2c68f73f677f7782eb0436cf4f98eab39e065437bfd2b90baf -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodePtr.op_Equality(Inno.Native.MiniAudio.MaBiquadNodePtr,Inno.Native.MiniAudio.MaBiquadNodePtr) f4b6a60b961d879b0083f0424f5b449bfb8350fb743623958185c1ab56292c8b -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodePtr.op_Inequality(Inno.Native.MiniAudio.MaBiquadNodePtr,Inno.Native.MiniAudio.MaBiquadNodePtr) bdfd837e71b7873446b967d224bd9928277cda4218e8513802ecb8695cb56f48 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodePtr.Equals(Inno.Native.MiniAudio.MaBiquadNodePtr) 1ab10748616dfc40b2e3fa1664ac3dd04fb7baec865180844b633d19803d39f7 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodePtr.Equals(System.Object) 536f016db6721ba592667263d66ac4e67654b623d0b8b279833ffd97ad85c755 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadNodePtr.GetHashCode d9cccdea6ca4db4823c5389b3c8d42f900987d108b5a884245d053a34ff9b024 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadNodePtr.BaseNode 49b2ebecaabe0030f689890bf0b321a92ce19d128c8b39ecc1316f707d3c9cf8 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadNodePtr.Biquad a125ddd3e06f694572e7acef000b1f1247538e407b0a6ba3d08bfc2b1ca3f61e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ea566d21d62f6aed"></a>[`public Inno.Native.MiniAudio.MaBiquadNode* Inno.Native.MiniAudio.MaBiquadNodePtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14637) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-671941785667f001"></a>[`public Inno.Native.MiniAudio.MaBiquadNodePtr.MaBiquadNodePtr(Inno.Native.MiniAudio.MaBiquadNode* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14632) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1a5a3f3654bde6a5"></a>[`public bool Inno.Native.MiniAudio.MaBiquadNodePtr.Equals(Inno.Native.MiniAudio.MaBiquadNodePtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14647) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-b3e9a29dcaa71adb"></a>[`public override bool Inno.Native.MiniAudio.MaBiquadNodePtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14648) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-27bdbbfbe5cd8858"></a>[`public override int Inno.Native.MiniAudio.MaBiquadNodePtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14649) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-762b3c6c020786de"></a>[`public static bool Inno.Native.MiniAudio.MaBiquadNodePtr.operator ==(Inno.Native.MiniAudio.MaBiquadNodePtr left, Inno.Native.MiniAudio.MaBiquadNodePtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14645) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9e8ed4dbcea7f8b3"></a>[`public static Inno.Native.MiniAudio.MaBiquadNodePtr.implicit operator Inno.Native.MiniAudio.MaBiquadNodePtr(Inno.Native.MiniAudio.MaBiquadNode* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14643) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5c621f54cfb9b7eb"></a>[`public static Inno.Native.MiniAudio.MaBiquadNodePtr.implicit operator Inno.Native.MiniAudio.MaBiquadNode*(Inno.Native.MiniAudio.MaBiquadNodePtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14644) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9b3f6361bc66b9b2"></a>[`public static bool Inno.Native.MiniAudio.MaBiquadNodePtr.operator !=(Inno.Native.MiniAudio.MaBiquadNodePtr left, Inno.Native.MiniAudio.MaBiquadNodePtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14646) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4cd4ee333e57148a"></a>[`public ref Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaBiquadNodePtr.BaseNode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14651) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6a44597527e30570"></a>[`public ref Inno.Native.MiniAudio.MaBiquad Inno.Native.MiniAudio.MaBiquadNodePtr.Biquad { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14652) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-803b0a5f00e2062e"></a>[`public bool Inno.Native.MiniAudio.MaBiquadNodePtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14638) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a08fefacc9b612ee"></a>[`public Inno.Native.MiniAudio.MaBiquadNode Inno.Native.MiniAudio.MaBiquadNodePtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14641) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cc30f7535bbb53fc"></a>[`public static Inno.Native.MiniAudio.MaBiquadNodePtr Inno.Native.MiniAudio.MaBiquadNodePtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14639) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c062511d4230f90d"></a>[`public struct Inno.Native.MiniAudio.MaBiquadNodePtr : System.IEquatable<Inno.Native.MiniAudio.MaBiquadNodePtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14630) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBiquadPtr</code> · 24 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBiquadPtr a1c7609e03d9ef453ac20f4bfc875f528276bf134b92580e1af849adb11b11f1 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadPtr.#ctor(Inno.Native.MiniAudio.MaBiquad*) 8c96c260ac657aa0799b6d120c05b781370f9c7473d5f44b49c4c42d842b8e0b -->
<!-- api: F:Inno.Native.MiniAudio.MaBiquadPtr.Handle 01eb911d781cf7e5e748800d3bd447ef2ccf5a0a16a611a4726198e71cd7e4b4 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.IsNull fc8eec257fcdfac7178bf567cb4475c0359f0cea6265fbd29bea298d591d43ff -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.Null 9b821ce251b77399bdae0a40f740f193f0101dc848883ce8d3a1b9f4e538d14f -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.Item(System.Int32) 4447a638ecb60a116fa3b6e91b09078bb450dfbf52914bb93f61442ab9ff809f -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadPtr.op_Implicit(Inno.Native.MiniAudio.MaBiquad*)~Inno.Native.MiniAudio.MaBiquadPtr 5b003f7971df8b8762cbd3cb400e43efb48b30c9fc1ed389a663412cedd94e6d -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadPtr.op_Implicit(Inno.Native.MiniAudio.MaBiquadPtr)~Inno.Native.MiniAudio.MaBiquad* addd0111f5c30a7c7c6e9ac035ba9e8bf6a3e02af0b3fc072ee4d30906a536dd -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadPtr.op_Equality(Inno.Native.MiniAudio.MaBiquadPtr,Inno.Native.MiniAudio.MaBiquadPtr) eb1495fc7f0849160f136b97f57c56b165da9b458c28ce198e84eaf1de370dc4 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadPtr.op_Inequality(Inno.Native.MiniAudio.MaBiquadPtr,Inno.Native.MiniAudio.MaBiquadPtr) e0d93104d868bfad532ccf9379ce13aab67a3134cff7cfb3f132c6185670725d -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadPtr.Equals(Inno.Native.MiniAudio.MaBiquadPtr) cdd8056efdc5bec3bfd7a4bac3562927d473250016bb22f33b03054816da8160 -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadPtr.Equals(System.Object) 51cef22f231cc7b1764652395b83ff5649fbbf005a6b0ef737831b08f72cd2ba -->
<!-- api: M:Inno.Native.MiniAudio.MaBiquadPtr.GetHashCode 5e4ef27c42ec7dd9fa91689bd94ba1aeaf40afe3dcfac720137dbae2b17d7288 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.Format 2947dfbf027f5544725961470b8488996445e4a9dd5606a9d7e98c34e1864dfc -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.Channels 433f5cfb27e90d1ebe92f96287a1eff38492bbc1a76fd44c29bf88185fbf622b -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.B0 b153ac4a5a18efca81fb0c89e30d26a2bee7a9fbad164bb0447df0a82ed7e046 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.B1 3f18904f0073853d022f99e878fd7d799cede4fc8cf7f4156f944ed06010662b -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.B2 17022b6a06b6f4d3f2ed906a55760286d1c2af7b5e7c7c0bf8718e8328ab3944 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.A1 94d6d1667e392e43831ecb6cbc4404e98854b4fad888e9bde46b327ab293cfb6 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.A2 c8a4eacd9f64242e10ffa08a3e4537e772b14a334ecd9ce88d40080d136a7c1e -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.PR1 e4f12cc4e0cbe5e1eaaeccb02418696225de77795243e57d0a1a383c204ac4e2 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.PR2 c06925cc4710c67c323973ba0451758214bead9609b77e86e883a8ad77d036c1 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.Pheap 0ce14af86820ea5bef1e200e8932ffdeba5a72627ffb92179b42188ab17e3cb2 -->
<!-- api: P:Inno.Native.MiniAudio.MaBiquadPtr.Ownsheap 0e21583540045c268fe42f506f33b7b076d33eeed978f6fd0bd6ae08c7fe5e49 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-526621aa8f2efa6d"></a>[`public Inno.Native.MiniAudio.MaBiquad* Inno.Native.MiniAudio.MaBiquadPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14663) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2503401b5224f530"></a>[`public Inno.Native.MiniAudio.MaBiquadPtr.MaBiquadPtr(Inno.Native.MiniAudio.MaBiquad* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14658) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-31eb5081078b30ce"></a>[`public bool Inno.Native.MiniAudio.MaBiquadPtr.Equals(Inno.Native.MiniAudio.MaBiquadPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14673) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-22233dc31bdee70e"></a>[`public override bool Inno.Native.MiniAudio.MaBiquadPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14674) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-1883a34974ded43a"></a>[`public override int Inno.Native.MiniAudio.MaBiquadPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14675) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-5ed1eaf3f7f50c54"></a>[`public static bool Inno.Native.MiniAudio.MaBiquadPtr.operator ==(Inno.Native.MiniAudio.MaBiquadPtr left, Inno.Native.MiniAudio.MaBiquadPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14671) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-813f0b22e8d021a8"></a>[`public static Inno.Native.MiniAudio.MaBiquadPtr.implicit operator Inno.Native.MiniAudio.MaBiquadPtr(Inno.Native.MiniAudio.MaBiquad* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14669) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e459eb0f2a3e5344"></a>[`public static Inno.Native.MiniAudio.MaBiquadPtr.implicit operator Inno.Native.MiniAudio.MaBiquad*(Inno.Native.MiniAudio.MaBiquadPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14670) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b886fbb0ee062770"></a>[`public static bool Inno.Native.MiniAudio.MaBiquadPtr.operator !=(Inno.Native.MiniAudio.MaBiquadPtr left, Inno.Native.MiniAudio.MaBiquadPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14672) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b273c692af2251c3"></a>[`public ref Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquadPtr.A1 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14682) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b38fcc213477f611"></a>[`public ref Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquadPtr.A2 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14683) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2eb5bc4677d38588"></a>[`public ref Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquadPtr.B0 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14679) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-09acc9a045808626"></a>[`public ref Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquadPtr.B1 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14680) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cc660177944d8b67"></a>[`public ref Inno.Native.MiniAudio.MaBiquadCoefficient Inno.Native.MiniAudio.MaBiquadPtr.B2 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14681) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-637abecd09162a5f"></a>[`public ref uint Inno.Native.MiniAudio.MaBiquadPtr.Channels { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14678) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3cf09a5ec7e5f098"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBiquadPtr.Format { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14677) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-18e9bf9ab30968b0"></a>[`public bool Inno.Native.MiniAudio.MaBiquadPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14664) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c68db243c693588f"></a>[`public Inno.Native.MiniAudio.MaBiquad Inno.Native.MiniAudio.MaBiquadPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14667) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9e985702f0972c25"></a>[`public static Inno.Native.MiniAudio.MaBiquadPtr Inno.Native.MiniAudio.MaBiquadPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14665) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7eefbde2d1e9a5b0"></a>[`public ref uint Inno.Native.MiniAudio.MaBiquadPtr.Ownsheap { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14687) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d092372203b9a0ee"></a>[`public void* Inno.Native.MiniAudio.MaBiquadPtr.Pheap { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14686) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cf638da467f8bffc"></a>[`public ref Inno.Native.MiniAudio.MaBiquadCoefficientPtr Inno.Native.MiniAudio.MaBiquadPtr.PR1 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14684) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-091302eda826012f"></a>[`public ref Inno.Native.MiniAudio.MaBiquadCoefficientPtr Inno.Native.MiniAudio.MaBiquadPtr.PR2 { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14685) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f28bf86a099b1b04"></a>[`public struct Inno.Native.MiniAudio.MaBiquadPtr : System.IEquatable<Inno.Native.MiniAudio.MaBiquadPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14656) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBpf</code> · 8 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBpf ee7b41b24372ba39a97b608c893a39c82f223dd516043cd12182aaca04bf6d7d -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf.Format 6644b88fcbc72eea7d1901baa8456afb038bb9f26e64c7367f3ad21c043c62a0 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf.Channels 24d7e74f82b8504fb011db6508c5930d2ce30d6d00093a307d8699108671e56f -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf.Bpf2Count 98ab6670264aaa23c1953f9780be9c2de1e9b3724af042a82d88d6b2af8f06d7 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf.PBpf2 5b66e52f042edb5b192ed8fca55d128d8abcf6313e585d635ca7072054ad83b2 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf.Pheap 1e0722fd8f70b7d2144c9f5599500dac898acc9f6174b498a7e4f88d8a4be502 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf.Ownsheap 613988f0e3f9d1543e8c3e558ccd9382d2df193e772863ae32d2b22f138f7d52 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf.#ctor(Inno.Native.MiniAudio.MaFormat,System.UInt32,System.UInt32,Inno.Native.MiniAudio.MaBpf2*,System.Void*,System.UInt32) e97bf98f46444f995f630a2ab7073713ffb28e096df3ee71629731563a80e93c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-1f06a4464206359c"></a>[`public uint Inno.Native.MiniAudio.MaBpf.Bpf2Count`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10713) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-80d3d7d2e6f7a586"></a>[`public uint Inno.Native.MiniAudio.MaBpf.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10712) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ad005ae8bc0f5814"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBpf.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10711) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3e8249521507562c"></a>[`public uint Inno.Native.MiniAudio.MaBpf.Ownsheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10716) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b1c3be2784fd2515"></a>[`public Inno.Native.MiniAudio.MaBpf2* Inno.Native.MiniAudio.MaBpf.PBpf2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10714) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e02fad75766ed269"></a>[`public void* Inno.Native.MiniAudio.MaBpf.Pheap`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10715) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5b37fbdf76e84ca8"></a>[`public Inno.Native.MiniAudio.MaBpf.MaBpf(Inno.Native.MiniAudio.MaFormat format = Inno.Native.MiniAudio.MaFormat.Unknown, uint channels = 0, uint bpf2Count = 0, Inno.Native.MiniAudio.MaBpf2* pBPF2 = null, void* pHeap = null, uint ownsHeap = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10717) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f794a76f22b6cd16"></a>[`public struct Inno.Native.MiniAudio.MaBpf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10709) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBpf2</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBpf2 db80227f10bafc5aaaa99e7919387158bcd07c4d6fe0e55c53bf3847d9fed795 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2.Bq 1a59b280aafee5a1b8bae663eebd3795f0a1ae260f6235d429194c2d3b7f77a8 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2.#ctor(Inno.Native.MiniAudio.MaBiquad) 54f5371b71282ffa4c3f267486fdb984ee452aca64b0f58ee150b0d1ea42158a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b3e11a1fc6a654d2"></a>[`public Inno.Native.MiniAudio.MaBiquad Inno.Native.MiniAudio.MaBpf2.Bq`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10677) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-243aa1806ce129f5"></a>[`public Inno.Native.MiniAudio.MaBpf2.MaBpf2(Inno.Native.MiniAudio.MaBiquad bq = default(Inno.Native.MiniAudio.MaBiquad))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10678) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-c6bd01e49fd93f74"></a>[`public struct Inno.Native.MiniAudio.MaBpf2`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10675) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBpf2Config</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBpf2Config 00eca6814514fea6a2e6c5ffcde2daba042a495347078ffecd5c26225be84c06 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2Config.Format 6708605beeb49a16fef0b55256c8cfdb3e89594456fe74ed77cdddd4a4ac5936 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2Config.Channels 875a240f39a859ef529d0a73c2b8ac1cea17663608d59740d55222ac04253f00 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2Config.SampleRate 5ceadcd3899baa2a9d865a136d414f2de8952bc3506dc976225f7f7cc01d8c11 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2Config.CutoffFrequency 915cbfa81d1ea651d8bab49f82470d272f512fb89970aa9031656c479de60a6f -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2Config.Q 18d4a28860b583be7aa38ffcbd8f124d5dac07eaf5dc04144d8b25ea553a2182 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2Config.#ctor(Inno.Native.MiniAudio.MaFormat,System.UInt32,System.UInt32,System.Double,System.Double) bdf81eda2b2af4fedbbe61a29ae79ff3ecee5ea23b2d8f59d6201acc01d14b8e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-a82f61099b22fcca"></a>[`public uint Inno.Native.MiniAudio.MaBpf2Config.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10657) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e5b7f389a0ed9169"></a>[`public double Inno.Native.MiniAudio.MaBpf2Config.CutoffFrequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10659) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-357b36e168348de3"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBpf2Config.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10656) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-f5cd72d6327cb54e"></a>[`public double Inno.Native.MiniAudio.MaBpf2Config.Q`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10660) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e70d4986d2ec716"></a>[`public uint Inno.Native.MiniAudio.MaBpf2Config.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10658) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-404aeef6879c6864"></a>[`public Inno.Native.MiniAudio.MaBpf2Config.MaBpf2Config(Inno.Native.MiniAudio.MaFormat format = Inno.Native.MiniAudio.MaFormat.Unknown, uint channels = 0, uint sampleRate = 0, double cutoffFrequency = 0, double q = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10661) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2fabda6c9fedf11a"></a>[`public struct Inno.Native.MiniAudio.MaBpf2Config`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10654) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBpf2ConfigPtr</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBpf2ConfigPtr 774acecef47e97e3856236105877dbd13cb5855defb2a20c5c84b78085ddfd6b -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2ConfigPtr.#ctor(Inno.Native.MiniAudio.MaBpf2Config*) f6fe4b7fac9eb0e4fb84fbcc2839f256261820fefa781e7a38ce22f12d13eda5 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2ConfigPtr.Handle e4370128aa7e7e8b15acb29a94d4a5679c9ddc23b0f5ec0782d8339c8eb0b0a2 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpf2ConfigPtr.IsNull a650a1515bccbd483cccd7fae5fdf75c97df0dbf4982c4492bc2e7b63b19b3ec -->
<!-- api: P:Inno.Native.MiniAudio.MaBpf2ConfigPtr.Null 0cbb58f7d4870430e794389e80cee81eda3d32018ed97e2c1d490cd2b50dc0b3 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpf2ConfigPtr.Item(System.Int32) fb6abd19832a394a96c8ed68e0b03dbf56bba50276e3abd43e482127b2dd49b5 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2ConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaBpf2Config*)~Inno.Native.MiniAudio.MaBpf2ConfigPtr daa606219b266c4e4f41c91ccacf796a7037fa2eac5a7e891e103a7a2e712140 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2ConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaBpf2ConfigPtr)~Inno.Native.MiniAudio.MaBpf2Config* 544efb76e0c8445a1aca9938c74d6c4cfd44dac00fce6d18f0edfef728333cfb -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2ConfigPtr.op_Equality(Inno.Native.MiniAudio.MaBpf2ConfigPtr,Inno.Native.MiniAudio.MaBpf2ConfigPtr) fa69ebb005ab3f67040a236c25fb3c72f3bbac767b7178a2180a3b692c683468 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2ConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaBpf2ConfigPtr,Inno.Native.MiniAudio.MaBpf2ConfigPtr) dde46f614b9a9fba20eaa55b2ded9f64b2bee788d4d224c445ac7b77f211e445 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2ConfigPtr.Equals(Inno.Native.MiniAudio.MaBpf2ConfigPtr) cdce25b69a96ab276e19a169f6db287dc2fbd66539071d772aa79764ee9f03a1 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2ConfigPtr.Equals(System.Object) c40017bb8c5c8cd2d0dd86e758d30e62b9e5fb89c4f56ea7900ed09354c34bde -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2ConfigPtr.GetHashCode 9ee7c6dbc372210c4a11c0fc1b02be684131df6bc7100c3d6789ce8b0d166cb0 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpf2ConfigPtr.Format 6c7c5300530741e910b169438e938ba0fa3856b58cb24479d71f70e62b982642 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpf2ConfigPtr.Channels 34081e77c8b3755070e7cecae56648a5ceaefe0147c425337190b4bbc07d85b7 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpf2ConfigPtr.SampleRate e39e23d04a64bae5812a2ff1cadbd698787b28f1eff9598d73de6e57e1ef1d53 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpf2ConfigPtr.CutoffFrequency 759976241fb71710d5d0d1a303d3915df0eb7ae2430520d86dfef45c6f69ff12 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpf2ConfigPtr.Q b4b6ff28b1aa0bb91b3900b9e57cd69d9f81fbf8415297dd0b43ec912b3359e0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-87a17c3702a86b45"></a>[`public Inno.Native.MiniAudio.MaBpf2Config* Inno.Native.MiniAudio.MaBpf2ConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14698) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-64e448c7c3dfffdf"></a>[`public Inno.Native.MiniAudio.MaBpf2ConfigPtr.MaBpf2ConfigPtr(Inno.Native.MiniAudio.MaBpf2Config* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14693) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-cda28803abc18874"></a>[`public bool Inno.Native.MiniAudio.MaBpf2ConfigPtr.Equals(Inno.Native.MiniAudio.MaBpf2ConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14708) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-211508f6ff20ea67"></a>[`public override bool Inno.Native.MiniAudio.MaBpf2ConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14709) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-8c793d023052d142"></a>[`public override int Inno.Native.MiniAudio.MaBpf2ConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14710) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-d8f0ce501d765c6a"></a>[`public static bool Inno.Native.MiniAudio.MaBpf2ConfigPtr.operator ==(Inno.Native.MiniAudio.MaBpf2ConfigPtr left, Inno.Native.MiniAudio.MaBpf2ConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14706) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-659cc4f5b49790ae"></a>[`public static Inno.Native.MiniAudio.MaBpf2ConfigPtr.implicit operator Inno.Native.MiniAudio.MaBpf2ConfigPtr(Inno.Native.MiniAudio.MaBpf2Config* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14704) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-87788639560a63eb"></a>[`public static Inno.Native.MiniAudio.MaBpf2ConfigPtr.implicit operator Inno.Native.MiniAudio.MaBpf2Config*(Inno.Native.MiniAudio.MaBpf2ConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14705) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0c6a9e857e568342"></a>[`public static bool Inno.Native.MiniAudio.MaBpf2ConfigPtr.operator !=(Inno.Native.MiniAudio.MaBpf2ConfigPtr left, Inno.Native.MiniAudio.MaBpf2ConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14707) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1d6de155dae4f3c9"></a>[`public ref uint Inno.Native.MiniAudio.MaBpf2ConfigPtr.Channels { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14713) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3f26fa085560a7ce"></a>[`public ref double Inno.Native.MiniAudio.MaBpf2ConfigPtr.CutoffFrequency { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14715) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b0f4f9bc8bd5b7cd"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBpf2ConfigPtr.Format { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14712) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1a12cce1c117174f"></a>[`public bool Inno.Native.MiniAudio.MaBpf2ConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14699) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-480e0eed45447e14"></a>[`public Inno.Native.MiniAudio.MaBpf2Config Inno.Native.MiniAudio.MaBpf2ConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14702) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c20c65b6f9b2f2bd"></a>[`public static Inno.Native.MiniAudio.MaBpf2ConfigPtr Inno.Native.MiniAudio.MaBpf2ConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14700) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dc760c3381bedf15"></a>[`public ref double Inno.Native.MiniAudio.MaBpf2ConfigPtr.Q { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14716) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c652afa07066e985"></a>[`public ref uint Inno.Native.MiniAudio.MaBpf2ConfigPtr.SampleRate { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14714) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6a42c7ea82ee1f64"></a>[`public struct Inno.Native.MiniAudio.MaBpf2ConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaBpf2ConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14691) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBpf2Ptr</code> · 14 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBpf2Ptr 0030bdf24a08bd02a4eda48d7d1e6e84033c527dfc646f42f9989e75672819b7 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2Ptr.#ctor(Inno.Native.MiniAudio.MaBpf2*) 2120a3a2c1816b7a58d16241be24a04fd765a636d03c7108e13362fc8bbe2657 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpf2Ptr.Handle 5807c1c6a9317e7e7b80978a161d24f36e88a21169201335490a5789e860abbb -->
<!-- api: P:Inno.Native.MiniAudio.MaBpf2Ptr.IsNull 4794e46d593f697b970de774580efe338324266d240342c21658f3a65d81b012 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpf2Ptr.Null 19a14d457685d697ed8f51046155dbb6da81be6f9281731453923b51f9f0cc63 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpf2Ptr.Item(System.Int32) 8990aae2a6b8849d66317c295cbc21874aa9a84d6261646c656d8996d4ea7f74 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2Ptr.op_Implicit(Inno.Native.MiniAudio.MaBpf2*)~Inno.Native.MiniAudio.MaBpf2Ptr e1f16a2cc3cb2f2c6b478847b9ff0f21d7a55bc25ba885759fa073def9723dc3 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2Ptr.op_Implicit(Inno.Native.MiniAudio.MaBpf2Ptr)~Inno.Native.MiniAudio.MaBpf2* 8840ad29001b1f362ac6afe5dbdae00f58aff339a3956d605d87fc487e17b944 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2Ptr.op_Equality(Inno.Native.MiniAudio.MaBpf2Ptr,Inno.Native.MiniAudio.MaBpf2Ptr) ef799eafdac05617c06bb55b833d499f94f0095a8ec0d3a53dfec99f65357e55 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2Ptr.op_Inequality(Inno.Native.MiniAudio.MaBpf2Ptr,Inno.Native.MiniAudio.MaBpf2Ptr) ca57eb21262616e89930c23361b288fe5ed3900d8f357c338a672efc7e339184 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2Ptr.Equals(Inno.Native.MiniAudio.MaBpf2Ptr) edde2f9f908d331afed500dd5a28325bc0c5fc0d3a097af6ab51bc7f728abbc9 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2Ptr.Equals(System.Object) 08e716b6a5ed6d57c1f563e47af70b6a450c5f7d600528f75991764d99a5d919 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpf2Ptr.GetHashCode 427470a69ee64f181feff8e74290de5144faa78364981a803b2e13339c47b438 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpf2Ptr.Bq 5b29a415e3dea2ac0a4c4a40d160e4e8e08388863bba0734c324035158dcea9f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0898993c37e6d442"></a>[`public Inno.Native.MiniAudio.MaBpf2* Inno.Native.MiniAudio.MaBpf2Ptr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14727) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-69fec1d79154d100"></a>[`public Inno.Native.MiniAudio.MaBpf2Ptr.MaBpf2Ptr(Inno.Native.MiniAudio.MaBpf2* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14722) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0155433effa98acc"></a>[`public bool Inno.Native.MiniAudio.MaBpf2Ptr.Equals(Inno.Native.MiniAudio.MaBpf2Ptr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14737) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-fc52d066207e48a4"></a>[`public override bool Inno.Native.MiniAudio.MaBpf2Ptr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14738) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-703b48222102266a"></a>[`public override int Inno.Native.MiniAudio.MaBpf2Ptr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14739) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-a3a9c5754483d305"></a>[`public static bool Inno.Native.MiniAudio.MaBpf2Ptr.operator ==(Inno.Native.MiniAudio.MaBpf2Ptr left, Inno.Native.MiniAudio.MaBpf2Ptr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14735) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-026db6de5bd91e25"></a>[`public static Inno.Native.MiniAudio.MaBpf2Ptr.implicit operator Inno.Native.MiniAudio.MaBpf2Ptr(Inno.Native.MiniAudio.MaBpf2* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14733) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-82152fa2ad29797e"></a>[`public static Inno.Native.MiniAudio.MaBpf2Ptr.implicit operator Inno.Native.MiniAudio.MaBpf2*(Inno.Native.MiniAudio.MaBpf2Ptr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14734) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9c34096cdb565aae"></a>[`public static bool Inno.Native.MiniAudio.MaBpf2Ptr.operator !=(Inno.Native.MiniAudio.MaBpf2Ptr left, Inno.Native.MiniAudio.MaBpf2Ptr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14736) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a2268c75aa6e5cad"></a>[`public ref Inno.Native.MiniAudio.MaBiquad Inno.Native.MiniAudio.MaBpf2Ptr.Bq { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14741) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-747cb45804206059"></a>[`public bool Inno.Native.MiniAudio.MaBpf2Ptr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14728) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e81df74852a9e345"></a>[`public Inno.Native.MiniAudio.MaBpf2 Inno.Native.MiniAudio.MaBpf2Ptr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14731) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-68a1fcaa20c4fd71"></a>[`public static Inno.Native.MiniAudio.MaBpf2Ptr Inno.Native.MiniAudio.MaBpf2Ptr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14729) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0fd1708f159678fb"></a>[`public struct Inno.Native.MiniAudio.MaBpf2Ptr : System.IEquatable<Inno.Native.MiniAudio.MaBpf2Ptr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14720) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBpfConfig</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBpfConfig 90fc7c71b22a6bbf9edecccde148af2149e1dae4dce217e2b6496c39d6e22eae -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfConfig.Format 08f71b47959d88299f5ce9e780e4df040bfabfd8b415d54b124de14064fd08d8 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfConfig.Channels 765efe5de199e1535901f1335977d99dd7e89a9e700c057d011a2eff32254bdc -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfConfig.SampleRate f59cb259c394adcd3c77ad8e649dc033c6c76854954109421145f7947446604c -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfConfig.CutoffFrequency 2283f9653e7e3dee6d4524db1d288e6c0a1f4b2b5e6aeb0cd7c09464b042c44d -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfConfig.Order b74d904f6b06708b63a6fd10ef8f367961a1371e3ab5dc0231ef899cae224649 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfConfig.#ctor(Inno.Native.MiniAudio.MaFormat,System.UInt32,System.UInt32,System.Double,System.UInt32) d5cde877a10449532ce4fac4ab84cb5c1f8a8d2df33fedf88edbbe2d489487e4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-db056d0bb30c8f21"></a>[`public uint Inno.Native.MiniAudio.MaBpfConfig.Channels`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10691) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c1d77df3aaa18a45"></a>[`public double Inno.Native.MiniAudio.MaBpfConfig.CutoffFrequency`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10693) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d057dae1f5043ef6"></a>[`public Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBpfConfig.Format`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10690) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-cb071e5d0cb9163a"></a>[`public uint Inno.Native.MiniAudio.MaBpfConfig.Order`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10694) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-39d168f3120759f2"></a>[`public uint Inno.Native.MiniAudio.MaBpfConfig.SampleRate`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10692) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9a767f607cc194cb"></a>[`public Inno.Native.MiniAudio.MaBpfConfig.MaBpfConfig(Inno.Native.MiniAudio.MaFormat format = Inno.Native.MiniAudio.MaFormat.Unknown, uint channels = 0, uint sampleRate = 0, double cutoffFrequency = 0, uint order = 0)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10695) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-0c076594920bd76e"></a>[`public struct Inno.Native.MiniAudio.MaBpfConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L10688) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBpfConfigPtr</code> · 18 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBpfConfigPtr ccc761c0957cd26b3bedfb0c70b75134766bbebdbb77528f20c43d23c2cac7b4 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfConfigPtr.#ctor(Inno.Native.MiniAudio.MaBpfConfig*) 7696ecdaf39fbf2983f2be0c65c8360dd820f2df03a4b6d0afebfe194ef886de -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfConfigPtr.Handle 772ceb3900d83fc2881121226e405e24fb48c33f956f28872e2eac771fd12a93 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfConfigPtr.IsNull b0259acfc6275915a48c6ad107b91e638223d88ebda2f8446b177440658f4107 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfConfigPtr.Null 302051ef837485130aa0df92c2cf95d4b2b60cbb91d79db070b8e58334ed2ef2 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfConfigPtr.Item(System.Int32) c747cf642c5d0171a8a31f715295e57911f0360b035f5fc01060b1a8b8bab20e -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaBpfConfig*)~Inno.Native.MiniAudio.MaBpfConfigPtr 8042c59487913729f4107b25d350c19ba6bc2ebed66bb613a21c2fb07152865a -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaBpfConfigPtr)~Inno.Native.MiniAudio.MaBpfConfig* 5e71fab884c5e168eb6f56017430089fee6b17ef47c2e0b120a00c3b1a61e45b -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfConfigPtr.op_Equality(Inno.Native.MiniAudio.MaBpfConfigPtr,Inno.Native.MiniAudio.MaBpfConfigPtr) 01265d93dae504dfbf06eeb20ff36cff9e10bcc67a9d967e279530bc422b85db -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaBpfConfigPtr,Inno.Native.MiniAudio.MaBpfConfigPtr) 2a4f1a4c13922214a6baf5650d1ad95c80a3a3b7cf91cb595484ca1b218d76a2 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfConfigPtr.Equals(Inno.Native.MiniAudio.MaBpfConfigPtr) ad5149ecc552ea6f3c7d31433c94905ef979796f12ee8a72e7ae91f5b72a3a9a -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfConfigPtr.Equals(System.Object) 81b60274597819256d411f729280d7da19f9624272e0fe22f60c8486ab303d41 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfConfigPtr.GetHashCode eb7697fc2ce7555eb71dc05e9b4bfcc555d2066165f414bf6a37bd9c0e9b0c25 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfConfigPtr.Format 4d7b6fa250c866ea0816ce5bba48a9c7b7cfb32381ed736598456ad91199e2f2 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfConfigPtr.Channels 44caf9eedabe6b61b6593bbf2c26e2c97972fc99e6298c43a809ef9d989ad9ae -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfConfigPtr.SampleRate 9ef6e951b243fb1d7a6646d5bc7d09b2eaa5ab22f736ce639eb8805a71555c59 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfConfigPtr.CutoffFrequency d26479cf7e72a05f0bfcce16dc3b1cc6fcdff5bb1d83c8edbfdd417916ab7f0d -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfConfigPtr.Order 1e5ad32aaddba33ed8e63472c444ea5a4e9c7732f97a7f494055b66fee050890 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-12bcf015dd1181a9"></a>[`public Inno.Native.MiniAudio.MaBpfConfig* Inno.Native.MiniAudio.MaBpfConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14752) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1177e0997e715137"></a>[`public Inno.Native.MiniAudio.MaBpfConfigPtr.MaBpfConfigPtr(Inno.Native.MiniAudio.MaBpfConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14747) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f13637e2a6fd0835"></a>[`public bool Inno.Native.MiniAudio.MaBpfConfigPtr.Equals(Inno.Native.MiniAudio.MaBpfConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14762) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-ae2d54ecef24cd21"></a>[`public override bool Inno.Native.MiniAudio.MaBpfConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14763) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-ad297b48eaf68d9e"></a>[`public override int Inno.Native.MiniAudio.MaBpfConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14764) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-a0325d7fd1d96c8b"></a>[`public static bool Inno.Native.MiniAudio.MaBpfConfigPtr.operator ==(Inno.Native.MiniAudio.MaBpfConfigPtr left, Inno.Native.MiniAudio.MaBpfConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14760) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e156f95d69ab5300"></a>[`public static Inno.Native.MiniAudio.MaBpfConfigPtr.implicit operator Inno.Native.MiniAudio.MaBpfConfigPtr(Inno.Native.MiniAudio.MaBpfConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14758) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-deaefef42dc07232"></a>[`public static Inno.Native.MiniAudio.MaBpfConfigPtr.implicit operator Inno.Native.MiniAudio.MaBpfConfig*(Inno.Native.MiniAudio.MaBpfConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14759) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-87dffb2d514f8536"></a>[`public static bool Inno.Native.MiniAudio.MaBpfConfigPtr.operator !=(Inno.Native.MiniAudio.MaBpfConfigPtr left, Inno.Native.MiniAudio.MaBpfConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14761) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bf7608db6a96e45f"></a>[`public ref uint Inno.Native.MiniAudio.MaBpfConfigPtr.Channels { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14767) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-670891eeb4ccbde9"></a>[`public ref double Inno.Native.MiniAudio.MaBpfConfigPtr.CutoffFrequency { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14769) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6e94b3c6a4e39440"></a>[`public ref Inno.Native.MiniAudio.MaFormat Inno.Native.MiniAudio.MaBpfConfigPtr.Format { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14766) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9f4430c3863bb050"></a>[`public bool Inno.Native.MiniAudio.MaBpfConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14753) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4edebd9c4cce153f"></a>[`public Inno.Native.MiniAudio.MaBpfConfig Inno.Native.MiniAudio.MaBpfConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14756) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eeb2f32be4ad290f"></a>[`public static Inno.Native.MiniAudio.MaBpfConfigPtr Inno.Native.MiniAudio.MaBpfConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14754) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eeb350584ff15c42"></a>[`public ref uint Inno.Native.MiniAudio.MaBpfConfigPtr.Order { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14770) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5e4556a92285cb59"></a>[`public ref uint Inno.Native.MiniAudio.MaBpfConfigPtr.SampleRate { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14768) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1ce11a84a6d9055b"></a>[`public struct Inno.Native.MiniAudio.MaBpfConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaBpfConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14745) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBpfNode</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBpfNode 8fdf09bf86d2c33c15ad7e87ba076c29f49242f282d9f735c7b93fdc5b42c44a -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfNode.BaseNode 2320018cd6a6e083caaabcd202359b7a94e03ee1260e65d980b233a51baf9614 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfNode.Bpf c0ddd71a223ee9cec7513a0ee529775f7f218cb86f7c1e143efe0dd5eab67658 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNode.#ctor(Inno.Native.MiniAudio.MaNodeBase,Inno.Native.MiniAudio.MaBpf) 685381c23c2b53053498fb4cb78c276bb5c32d4fc846ba2a51bfc8ade4a6f613 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-de2656691c32de19"></a>[`public Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaBpfNode.BaseNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13374) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fffce342e110bf70"></a>[`public Inno.Native.MiniAudio.MaBpf Inno.Native.MiniAudio.MaBpfNode.Bpf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13375) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9eb98553083bb756"></a>[`public Inno.Native.MiniAudio.MaBpfNode.MaBpfNode(Inno.Native.MiniAudio.MaNodeBase baseNode = default(Inno.Native.MiniAudio.MaNodeBase), Inno.Native.MiniAudio.MaBpf bpf = default(Inno.Native.MiniAudio.MaBpf))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13376) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-b1e781e81c1479dc"></a>[`public struct Inno.Native.MiniAudio.MaBpfNode`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13372) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBpfNodeConfig</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBpfNodeConfig 31593f55bcafa40548db9dc30cf906ce199eae17946ec610d0d901e71fb643b2 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfNodeConfig.NodeConfig e46bf278bb58b7cb831c0cbac020c713d189e92a5810f0294626ea65604078ca -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfNodeConfig.Bpf 9f208f3b903cddc0fff8e088f95f1c50bdec6bdf00bc9cc5a3a14609f2e691f3 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodeConfig.#ctor(Inno.Native.MiniAudio.MaNodeConfig,Inno.Native.MiniAudio.MaBpfConfig) 4a0aa850a1598e4ad3f79657d48ec7dac5cf98af9aac0c35091bcd3de5297e42 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-8c9be482ceada1e4"></a>[`public Inno.Native.MiniAudio.MaBpfConfig Inno.Native.MiniAudio.MaBpfNodeConfig.Bpf`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13360) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2d1a59b8a55ee899"></a>[`public Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaBpfNodeConfig.NodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13359) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-871d022a6b8af825"></a>[`public Inno.Native.MiniAudio.MaBpfNodeConfig.MaBpfNodeConfig(Inno.Native.MiniAudio.MaNodeConfig nodeConfig = default(Inno.Native.MiniAudio.MaNodeConfig), Inno.Native.MiniAudio.MaBpfConfig bpf = default(Inno.Native.MiniAudio.MaBpfConfig))`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13361) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-044ae804a40769b6"></a>[`public struct Inno.Native.MiniAudio.MaBpfNodeConfig`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L13357) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBpfNodeConfigPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBpfNodeConfigPtr 8906faba76cf1ce66516da7b2faaf01dca4f3274243bc234f684b92c380cc2c5 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.#ctor(Inno.Native.MiniAudio.MaBpfNodeConfig*) ec1ad77c0ae335ac82d8cff834e5f260b6dcd999fd14a0f88fa276dd014e737e -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.Handle 511ae743645e77481caf8856e4979ed56e0ebcd9a12d2ab991c5b5a77bc458ee -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.IsNull 587649e870ce5c167339f0b06fe633ef72b350ac27308a4ea479fc4d96cf7407 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.Null e7d007ed8d0cbeaf7cdd8474046fa1bae2321a6036ace7f79b6cfa5726915acc -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.Item(System.Int32) 34d3fdf28ac268e2cb2c70300ce4e7ef0ab2f9c2189042b35e4bd6712ac3ed26 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaBpfNodeConfig*)~Inno.Native.MiniAudio.MaBpfNodeConfigPtr 8b8e4dacea694aa7a0997808c6e7452803a66674dc0181d4f9f574818a3d597b -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.op_Implicit(Inno.Native.MiniAudio.MaBpfNodeConfigPtr)~Inno.Native.MiniAudio.MaBpfNodeConfig* 5cd2d2c428b017bdca85d2b73565f71a75628e4fbe30b01ce844e031da881fa8 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.op_Equality(Inno.Native.MiniAudio.MaBpfNodeConfigPtr,Inno.Native.MiniAudio.MaBpfNodeConfigPtr) 0a0b8b64cca76159cb5dff023ecf858603bcb8c30bfb07d5bb49be954e7d2ba4 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.op_Inequality(Inno.Native.MiniAudio.MaBpfNodeConfigPtr,Inno.Native.MiniAudio.MaBpfNodeConfigPtr) 8707bebb9976e1549d5cc4a989385a66587146722a4033d49edaf3b1dfa6fedd -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.Equals(Inno.Native.MiniAudio.MaBpfNodeConfigPtr) 6a97a6005a472cfdb4f2256b034cc4b2a4863baef3942e1756b37e6048dd5620 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.Equals(System.Object) ca15bbf8af2b90953f9471b96af079930fedee91cec7dff6638654a00e4b7df1 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.GetHashCode 9b96cc77bcd810affa32efa0fe8e034d8e7ebcef9db39750fe73fb39c79f6da0 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.NodeConfig 430000b45bd85b25acc44da7bbb91d0387ee48cdba9d3b23346c673fe021f34a -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfNodeConfigPtr.Bpf 3e4fa41b513cd7d541768ca5a3ad73eed2b1653c15c704083a32a0c0926b3c5b -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e27ddb3357dbbfa4"></a>[`public Inno.Native.MiniAudio.MaBpfNodeConfig* Inno.Native.MiniAudio.MaBpfNodeConfigPtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14781) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-07e8aaea17fac57a"></a>[`public Inno.Native.MiniAudio.MaBpfNodeConfigPtr.MaBpfNodeConfigPtr(Inno.Native.MiniAudio.MaBpfNodeConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14776) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-edf2047259c01b67"></a>[`public bool Inno.Native.MiniAudio.MaBpfNodeConfigPtr.Equals(Inno.Native.MiniAudio.MaBpfNodeConfigPtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14791) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-3b937ded9c9ced33"></a>[`public override bool Inno.Native.MiniAudio.MaBpfNodeConfigPtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14792) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-60b647bdfbb4d7f1"></a>[`public override int Inno.Native.MiniAudio.MaBpfNodeConfigPtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14793) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-3e9de645346398dd"></a>[`public static bool Inno.Native.MiniAudio.MaBpfNodeConfigPtr.operator ==(Inno.Native.MiniAudio.MaBpfNodeConfigPtr left, Inno.Native.MiniAudio.MaBpfNodeConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14789) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-720bf38a01bc34c4"></a>[`public static Inno.Native.MiniAudio.MaBpfNodeConfigPtr.implicit operator Inno.Native.MiniAudio.MaBpfNodeConfigPtr(Inno.Native.MiniAudio.MaBpfNodeConfig* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14787) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4f389c124f848707"></a>[`public static Inno.Native.MiniAudio.MaBpfNodeConfigPtr.implicit operator Inno.Native.MiniAudio.MaBpfNodeConfig*(Inno.Native.MiniAudio.MaBpfNodeConfigPtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14788) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9e945dbaeab5afd0"></a>[`public static bool Inno.Native.MiniAudio.MaBpfNodeConfigPtr.operator !=(Inno.Native.MiniAudio.MaBpfNodeConfigPtr left, Inno.Native.MiniAudio.MaBpfNodeConfigPtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14790) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e43513c8fd63ccd3"></a>[`public ref Inno.Native.MiniAudio.MaBpfConfig Inno.Native.MiniAudio.MaBpfNodeConfigPtr.Bpf { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14796) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0bdbb1872417b2a1"></a>[`public bool Inno.Native.MiniAudio.MaBpfNodeConfigPtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14782) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3fee64c4cd5a6dd0"></a>[`public Inno.Native.MiniAudio.MaBpfNodeConfig Inno.Native.MiniAudio.MaBpfNodeConfigPtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14785) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ccb765f3bc9fa694"></a>[`public ref Inno.Native.MiniAudio.MaNodeConfig Inno.Native.MiniAudio.MaBpfNodeConfigPtr.NodeConfig { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14795) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f66474f8f868d15b"></a>[`public static Inno.Native.MiniAudio.MaBpfNodeConfigPtr Inno.Native.MiniAudio.MaBpfNodeConfigPtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14783) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d213ff6148eced84"></a>[`public struct Inno.Native.MiniAudio.MaBpfNodeConfigPtr : System.IEquatable<Inno.Native.MiniAudio.MaBpfNodeConfigPtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14774) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.MiniAudio.MaBpfNodePtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.MiniAudio.MaBpfNodePtr 6e5af1e856f66e25a8114f223bab3d5eaa9714b0820f4ab487292f89b2b03546 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodePtr.#ctor(Inno.Native.MiniAudio.MaBpfNode*) 581f3e3d2aa2e07193cd0de1abd802b509153935b93e8fdb173a2f0406f61b72 -->
<!-- api: F:Inno.Native.MiniAudio.MaBpfNodePtr.Handle af01b7260db003768b8aad95b0349048f07f312c1073cc9545eaf5b49b033c4e -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfNodePtr.IsNull c422aa41440ff37242bd862a3ac71a51f97ab8632c34a3f201034c81d501df08 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfNodePtr.Null 980992c8b32be4e997350bd137c2dae85bb82768747a758ae22cac2f65f8c3af -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfNodePtr.Item(System.Int32) 7f22313d2f08d235eaf44dbd788e0b1a872c1707781ac69ce0450d16b14ae1e0 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodePtr.op_Implicit(Inno.Native.MiniAudio.MaBpfNode*)~Inno.Native.MiniAudio.MaBpfNodePtr 46dc1bce79223296229d75ddf4d411751636191a0f57401c867ac3674b043ad2 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodePtr.op_Implicit(Inno.Native.MiniAudio.MaBpfNodePtr)~Inno.Native.MiniAudio.MaBpfNode* 3ff9e656709662133e39d19348106dad384d0fc129f44d92f0a338f9bf6dbdc0 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodePtr.op_Equality(Inno.Native.MiniAudio.MaBpfNodePtr,Inno.Native.MiniAudio.MaBpfNodePtr) 4f029293c267f4ed67b5ee683f97e659e0703a429abbf8ccc224a7f96395edfd -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodePtr.op_Inequality(Inno.Native.MiniAudio.MaBpfNodePtr,Inno.Native.MiniAudio.MaBpfNodePtr) 88a344cc1312aa905a456cea7c1ccd4218e39919b8afd46e79c02d75fb3d0f19 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodePtr.Equals(Inno.Native.MiniAudio.MaBpfNodePtr) d4d57c16fdaca439054d14d1b8dee55044eee6cfdce0cc0e908e01e01c2d4a6c -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodePtr.Equals(System.Object) 286b0d6e1f7b70ae85444b17a55fd1ffda5fd9723785afe3621b12ba822b58c4 -->
<!-- api: M:Inno.Native.MiniAudio.MaBpfNodePtr.GetHashCode 81eedc0508ec6ff61a6c52f92b888816e80ee41a1d4bf7015f1ca158e60ad0d6 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfNodePtr.BaseNode 1f6e56694b2f40929055c56d8f253215222c433369379a8759a892f3fad9fb12 -->
<!-- api: P:Inno.Native.MiniAudio.MaBpfNodePtr.Bpf 625c9ef8bac72e62e2febdd6a5baf40009d2c806c064df7dcb73e6d4af757faa -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-d141e85090da4aba"></a>[`public Inno.Native.MiniAudio.MaBpfNode* Inno.Native.MiniAudio.MaBpfNodePtr.Handle`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14807) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-676323ad697032c5"></a>[`public Inno.Native.MiniAudio.MaBpfNodePtr.MaBpfNodePtr(Inno.Native.MiniAudio.MaBpfNode* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14802) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-728731defcb4664f"></a>[`public bool Inno.Native.MiniAudio.MaBpfNodePtr.Equals(Inno.Native.MiniAudio.MaBpfNodePtr other)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14817) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-379efb53d75861e2"></a>[`public override bool Inno.Native.MiniAudio.MaBpfNodePtr.Equals(object? obj)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14818) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-e90ad16067f585a3"></a>[`public override int Inno.Native.MiniAudio.MaBpfNodePtr.GetHashCode()`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14819) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-19830fcdd5f24ca5"></a>[`public static bool Inno.Native.MiniAudio.MaBpfNodePtr.operator ==(Inno.Native.MiniAudio.MaBpfNodePtr left, Inno.Native.MiniAudio.MaBpfNodePtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14815) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-3dad52f37bb10517"></a>[`public static Inno.Native.MiniAudio.MaBpfNodePtr.implicit operator Inno.Native.MiniAudio.MaBpfNodePtr(Inno.Native.MiniAudio.MaBpfNode* handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14813) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7f90cadbcadb55fa"></a>[`public static Inno.Native.MiniAudio.MaBpfNodePtr.implicit operator Inno.Native.MiniAudio.MaBpfNode*(Inno.Native.MiniAudio.MaBpfNodePtr handle)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14814) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b0196e61b3424a55"></a>[`public static bool Inno.Native.MiniAudio.MaBpfNodePtr.operator !=(Inno.Native.MiniAudio.MaBpfNodePtr left, Inno.Native.MiniAudio.MaBpfNodePtr right)`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14816) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-6cf1df8c603c6a26"></a>[`public ref Inno.Native.MiniAudio.MaNodeBase Inno.Native.MiniAudio.MaBpfNodePtr.BaseNode { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14821) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5d3742e2129e1e33"></a>[`public ref Inno.Native.MiniAudio.MaBpf Inno.Native.MiniAudio.MaBpfNodePtr.Bpf { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14822) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8a1f5922c7775d42"></a>[`public bool Inno.Native.MiniAudio.MaBpfNodePtr.IsNull { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14808) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a2b0d5a7a142b8a6"></a>[`public Inno.Native.MiniAudio.MaBpfNode Inno.Native.MiniAudio.MaBpfNodePtr.this[int index] { get; set; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14811) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8e524ddc6a595d87"></a>[`public static Inno.Native.MiniAudio.MaBpfNodePtr Inno.Native.MiniAudio.MaBpfNodePtr.Null { get; }`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14809) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5f5763c9e095700d"></a>[`public struct Inno.Native.MiniAudio.MaBpfNodePtr : System.IEquatable<Inno.Native.MiniAudio.MaBpfNodePtr>`](../../../../backends/MiniAudio/native/Inno.Native.MiniAudio/Generated/Bindings.cs#L14800) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
