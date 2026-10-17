# Inno.Native.Sdl3 ABI · SDLGLAttr → SDLGPUIndirectDispatchCommand

[项目与生命周期](../Inno.Native.Sdl3.md) · [ABI 索引](README.md)

<details>
<summary><code>Inno.Native.Sdl3.SDLGLAttr</code> · 29 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGLAttr 625ce1f5adc4e3c859d46f56f3627f90ad7b61cec2e21aef909efae692240962 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.RedSize eb852b0b6af9c2d40bc43f280aed7ab834c5539ab0131aac67277073fd15bffd -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.GreenSize 54bedd32b7aea122135ba6df456629a61e876db9de3cd61285226dc92c74e978 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.BlueSize 81cea545feb8272ec11434cafd325642e0a5091f71013d1bfb95be658804bd33 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.AlphaSize 7769299ac926a686d7af15b2871dc85deeaefae987030c37eac383902c0c3186 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.BufferSize ae57d1b40c3729fd685e27869edbc28cd6ce52cc145db595a606e691a3500099 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.Doublebuffer e81e1c9e9050c6cddbf8d5a2fafeee74fa8c7a90b689e726906d554f1c9c72b2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.DepthSize 8410071aed212e8afaee5c17cb85f98f1cc33a5ce6955515983b9cfe556c850e -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.StencilSize 0c90227597507ed975955e3db65ddb9c2ebab8ad43e0f32374b7176f47f49d63 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.AccumRedSize 7118f9af9f299efbfd676c72f191298cb95971bcdd92396a98e2109468293ca0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.AccumGreenSize 1333f218bad3b3506a9cbfee1dbf783520abea8752a7943205e0af1f2a966d40 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.AccumBlueSize fb1ad3af3b242cb5d9ec2be4a6ef7f00117d092c069ba72cffef815a0d09bf21 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.AccumAlphaSize 7be10c09320fa88b0a468b08166f317a51f1987fe2a887c67e535e8726ddc839 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.Stereo 9348fd1f8e1f6c45f6a75957000041ceb30b7c95bedccbada7729fa0b6cb4b30 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.Multisamplebuffers c6542a06af9fb68b627d8bf6cf6bc32e56fd6f5724d299f26fa058a703f23030 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.Multisamplesamples d5ff1c2293eb22c17cb552a5afd42fbabee9aa5bd182dc3084fd2798dd0b623e -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.AcceleratedVisual d8ccc0f79b85ef7f355053eba77ede87bed38c59607c41f742ad509c99d90935 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.RetainedBacking c29260f313ae7cb68158e9ee9e0211f9b9c0086dc72b3749692fe286a07dc96f -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.ContextMajorVersion 0978a78f8fe91126a0611d17a634ea22dce9a587d8a7180b6153bd66f280921e -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.ContextMinorVersion 40f6db4291b3785f102d2402c8a7251616410c398b17d3e2e4dc52d054af37f1 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.ContextFlags ed4f0606a32932e30a9306179bbf94565b4a37c4cdc9220cda0a9550095d7473 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.ContextProfileMask 359b6bada7c3a21490ad26a28ee93f7f4f65664427894c2005265edf36b0af51 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.ShareWithCurrentContext 724ac3fab8319a880114ae24b285c76dd0d03c31ffaa9f287eb439b4af067c22 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.FramebufferSrgbCapable f87c866e38875a02e0591bff646b1fd99393a0eddf571065b7843364e4a789b9 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.ContextReleaseBehavior 08f1a062353f44671d7dcd6ed8ad67ceb807e53b7a659b0b6fe7ba87b88a646c -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.ContextResetNotification af6dee92608d246276cfbc31bd0a9c228f4b0570e28242eac1244767017e3483 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.ContextNoError 9c0a07d7505193e2b749f60cc846b09bdf5760d9ba12b545cb6d8196c730fa86 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.Floatbuffers 1ecff88e1cdc69dab561f672e4de1213b461b90b994424cd6757253d43b8eb22 -->
<!-- api: F:Inno.Native.Sdl3.SDLGLAttr.EglPlatform 92cc65806929b451b32751d96c4ac1560eba5d25ac3c90643aa19671d69a23f4 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-569c724998fcbf59"></a>[`Inno.Native.Sdl3.SDLGLAttr.AcceleratedVisual = 15`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L607) | 当前常量或枚举值：`15`。 | — |
| <a id="api-b9fb305195523e16"></a>[`Inno.Native.Sdl3.SDLGLAttr.AccumAlphaSize = 11`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L603) | 当前常量或枚举值：`11`。 | — |
| <a id="api-ec49a3a355a4175f"></a>[`Inno.Native.Sdl3.SDLGLAttr.AccumBlueSize = 10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L602) | 当前常量或枚举值：`10`。 | — |
| <a id="api-b4a18d21154625b2"></a>[`Inno.Native.Sdl3.SDLGLAttr.AccumGreenSize = 9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L601) | 当前常量或枚举值：`9`。 | — |
| <a id="api-acccadb36d9c27ef"></a>[`Inno.Native.Sdl3.SDLGLAttr.AccumRedSize = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L600) | 当前常量或枚举值：`8`。 | — |
| <a id="api-e2d926736bbdf72d"></a>[`Inno.Native.Sdl3.SDLGLAttr.AlphaSize = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L595) | 当前常量或枚举值：`3`。 | — |
| <a id="api-b1c61df45b8023ac"></a>[`Inno.Native.Sdl3.SDLGLAttr.BlueSize = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L594) | 当前常量或枚举值：`2`。 | — |
| <a id="api-b48229fae6dcf01b"></a>[`Inno.Native.Sdl3.SDLGLAttr.BufferSize = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L596) | 当前常量或枚举值：`4`。 | — |
| <a id="api-4539b19efe3147d1"></a>[`Inno.Native.Sdl3.SDLGLAttr.ContextFlags = 19`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L611) | 当前常量或枚举值：`19`。 | — |
| <a id="api-f35ee0a46ed83916"></a>[`Inno.Native.Sdl3.SDLGLAttr.ContextMajorVersion = 17`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L609) | 当前常量或枚举值：`17`。 | — |
| <a id="api-31df6e5ab3ed3eb7"></a>[`Inno.Native.Sdl3.SDLGLAttr.ContextMinorVersion = 18`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L610) | 当前常量或枚举值：`18`。 | — |
| <a id="api-2db33fd18b54a434"></a>[`Inno.Native.Sdl3.SDLGLAttr.ContextNoError = 25`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L617) | 当前常量或枚举值：`25`。 | — |
| <a id="api-b6f791efa03f111e"></a>[`Inno.Native.Sdl3.SDLGLAttr.ContextProfileMask = 20`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L612) | 当前常量或枚举值：`20`。 | — |
| <a id="api-fa88bef9481344a0"></a>[`Inno.Native.Sdl3.SDLGLAttr.ContextReleaseBehavior = 23`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L615) | 当前常量或枚举值：`23`。 | — |
| <a id="api-7604f15d8abcac44"></a>[`Inno.Native.Sdl3.SDLGLAttr.ContextResetNotification = 24`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L616) | 当前常量或枚举值：`24`。 | — |
| <a id="api-d3f489a0d50683f6"></a>[`Inno.Native.Sdl3.SDLGLAttr.DepthSize = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L598) | 当前常量或枚举值：`6`。 | — |
| <a id="api-cc42e004a54756da"></a>[`Inno.Native.Sdl3.SDLGLAttr.Doublebuffer = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L597) | 当前常量或枚举值：`5`。 | — |
| <a id="api-3c70c9c84dc02a1c"></a>[`Inno.Native.Sdl3.SDLGLAttr.EglPlatform = 27`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L619) | 当前常量或枚举值：`27`。 | — |
| <a id="api-576c014f5a75c0f8"></a>[`Inno.Native.Sdl3.SDLGLAttr.Floatbuffers = 26`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L618) | 当前常量或枚举值：`26`。 | — |
| <a id="api-a4836ee1185ef9ba"></a>[`Inno.Native.Sdl3.SDLGLAttr.FramebufferSrgbCapable = 22`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L614) | 当前常量或枚举值：`22`。 | — |
| <a id="api-efd7a861c2cbdb07"></a>[`Inno.Native.Sdl3.SDLGLAttr.GreenSize = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L593) | 当前常量或枚举值：`1`。 | — |
| <a id="api-c6fda6840b3dcbf3"></a>[`Inno.Native.Sdl3.SDLGLAttr.Multisamplebuffers = 13`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L605) | 当前常量或枚举值：`13`。 | — |
| <a id="api-b0efa5b8536a4f4f"></a>[`Inno.Native.Sdl3.SDLGLAttr.Multisamplesamples = 14`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L606) | 当前常量或枚举值：`14`。 | — |
| <a id="api-3ce37594e624c944"></a>[`Inno.Native.Sdl3.SDLGLAttr.RedSize = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L592) | 当前常量或枚举值：`0`。 | — |
| <a id="api-b66894568ce4888c"></a>[`Inno.Native.Sdl3.SDLGLAttr.RetainedBacking = 16`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L608) | 当前常量或枚举值：`16`。 | — |
| <a id="api-fdcfd58f8affe2c7"></a>[`Inno.Native.Sdl3.SDLGLAttr.ShareWithCurrentContext = 21`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L613) | 当前常量或枚举值：`21`。 | — |
| <a id="api-d92630918b523476"></a>[`Inno.Native.Sdl3.SDLGLAttr.StencilSize = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L599) | 当前常量或枚举值：`7`。 | — |
| <a id="api-f60a98b513a80c5d"></a>[`Inno.Native.Sdl3.SDLGLAttr.Stereo = 12`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L604) | 当前常量或枚举值：`12`。 | — |
| <a id="api-8d324a39380d7a98"></a>[`public enum Inno.Native.Sdl3.SDLGLAttr : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L590) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGLContext</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGLContext 172415fa3c61a8a0fa5df7a827106f7b174f9acbc3abc049c28519acdc282a68 -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContext.#ctor(System.IntPtr) 82620138c17115aec1310c23cf2192c8e48c62022f95f3612768ddae8672a43a -->
<!-- api: P:Inno.Native.Sdl3.SDLGLContext.Handle a05756e18820d754213b3bfd67fca1a1525de4e6ba2f9ff58cac7b798642dbcf -->
<!-- api: P:Inno.Native.Sdl3.SDLGLContext.IsNull c0fa0ff4ca7240ccea2763c076ee144829d30ebf0f0e393fc8ebdb574e9f6ff2 -->
<!-- api: P:Inno.Native.Sdl3.SDLGLContext.Null 349d2eed08d8d495066361b0201a3f6979b69c50609b3f66cfd23dc1007c677c -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContext.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLGLContext 02256d2b67b33fa78e5524d2bfc85b8cb280d576709e1170fcdcae730ace5a6a -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContext.op_Equality(Inno.Native.Sdl3.SDLGLContext,Inno.Native.Sdl3.SDLGLContext) a655819f413aea09616acb087105444c16094409d6c994b1be2753b31d814d1c -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContext.op_Inequality(Inno.Native.Sdl3.SDLGLContext,Inno.Native.Sdl3.SDLGLContext) 8439454f9a9fada6f62fe60f747921035c4b4c019896faf1483680216322abc0 -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContext.Equals(Inno.Native.Sdl3.SDLGLContext) e72ed4b9d5e2d7f017841dc19a2701e58d94caa036683b0131ddb2f06d763ae0 -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContext.Equals(System.Object) dbd7992424f09a068b32978da487f9f00d2d434caabfee91c1171f6c5771a3a6 -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContext.GetHashCode 9cbfdc155e59c1a7e2ee26b9967352a9adc7ee7c58c2e2fb1cf3091f181b35ce -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7accfad664bc786e"></a>[`public Inno.Native.Sdl3.SDLGLContext.SDLGLContext(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6500) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-43a729a5528182ae"></a>[`public bool Inno.Native.Sdl3.SDLGLContext.Equals(Inno.Native.Sdl3.SDLGLContext other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6512) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-c8457c3d5bcdaff1"></a>[`public override bool Inno.Native.Sdl3.SDLGLContext.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6513) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-6f29af6c0b340eaf"></a>[`public override int Inno.Native.Sdl3.SDLGLContext.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6514) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-86e111a20a4fbd57"></a>[`public static bool Inno.Native.Sdl3.SDLGLContext.operator ==(Inno.Native.Sdl3.SDLGLContext left, Inno.Native.Sdl3.SDLGLContext right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6510) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-c76b509425d798c0"></a>[`public static Inno.Native.Sdl3.SDLGLContext.implicit operator Inno.Native.Sdl3.SDLGLContext(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6509) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4da5480a4f37231a"></a>[`public static bool Inno.Native.Sdl3.SDLGLContext.operator !=(Inno.Native.Sdl3.SDLGLContext left, Inno.Native.Sdl3.SDLGLContext right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6511) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fbf7583c7506b82e"></a>[`public nint Inno.Native.Sdl3.SDLGLContext.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6505) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-387f60229a1add9c"></a>[`public bool Inno.Native.Sdl3.SDLGLContext.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6506) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9745d53f23ce06c5"></a>[`public static Inno.Native.Sdl3.SDLGLContext Inno.Native.Sdl3.SDLGLContext.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6507) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4bdfb13deb38e890"></a>[`public readonly struct Inno.Native.Sdl3.SDLGLContext : System.IEquatable<Inno.Native.Sdl3.SDLGLContext>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6498) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGLContextState</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGLContextState 1ba78c69b535c7c9defc1e9ace9fac1a4c493573b094a17a0d36d60853c6078e -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContextState.#ctor(System.IntPtr) 677245d1ad322717a58e6d6e0d7cf7e8a495701e222983b5d72626d0ba3bd3b3 -->
<!-- api: P:Inno.Native.Sdl3.SDLGLContextState.Handle 71e8cdabceb667dd19a685c982c2c4ce7330d20ddc66e3656e507eea56e6d95f -->
<!-- api: P:Inno.Native.Sdl3.SDLGLContextState.IsNull c8865c5dc984aee96a212058f175d7e874ad1218d46f75e84140976fb7308869 -->
<!-- api: P:Inno.Native.Sdl3.SDLGLContextState.Null 62b8ca04da6544ca2cc7d1a6a5ccd2433dc7c1d2eb2557c70d43392a68f8e054 -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContextState.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLGLContextState 33e210f9864e5a34172932d2c100978d84d9f12635beddf9b2aa6f1613642491 -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContextState.op_Equality(Inno.Native.Sdl3.SDLGLContextState,Inno.Native.Sdl3.SDLGLContextState) c2f62436e22aa3f23c25059a95dfa46fa0795211deddb468f082aff8ff7e3dd2 -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContextState.op_Inequality(Inno.Native.Sdl3.SDLGLContextState,Inno.Native.Sdl3.SDLGLContextState) b96fa4bff1b65cf4e3af18c7e038695bf0130bfd7ad13f7bded0b3e5bb80cdaa -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContextState.Equals(Inno.Native.Sdl3.SDLGLContextState) ec5623933a2806dcf86ba4449be9f46b8c4e3cb1aaa00e7603bb4a0c054210e8 -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContextState.Equals(System.Object) eaa724b96e956ccaf8e3edb5529e9aeb8a777f71389f0986a5d24034d918caab -->
<!-- api: M:Inno.Native.Sdl3.SDLGLContextState.GetHashCode 1c8e921ff1525308462dfe359492a032e7d6a133a4bcca99476a5cc39b01e6a7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-aa0914afbf96eb47"></a>[`public Inno.Native.Sdl3.SDLGLContextState.SDLGLContextState(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2652) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a782d3472d859079"></a>[`public bool Inno.Native.Sdl3.SDLGLContextState.Equals(Inno.Native.Sdl3.SDLGLContextState other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2664) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-9eaa65e295fe409a"></a>[`public override bool Inno.Native.Sdl3.SDLGLContextState.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2665) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-ae3f26e9a3d47a65"></a>[`public override int Inno.Native.Sdl3.SDLGLContextState.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2666) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-44f16f92bf32b273"></a>[`public static bool Inno.Native.Sdl3.SDLGLContextState.operator ==(Inno.Native.Sdl3.SDLGLContextState left, Inno.Native.Sdl3.SDLGLContextState right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2662) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8db370597c411bdf"></a>[`public static Inno.Native.Sdl3.SDLGLContextState.implicit operator Inno.Native.Sdl3.SDLGLContextState(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2661) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8d31e204f775978c"></a>[`public static bool Inno.Native.Sdl3.SDLGLContextState.operator !=(Inno.Native.Sdl3.SDLGLContextState left, Inno.Native.Sdl3.SDLGLContextState right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2663) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-569c08b10cc06bd8"></a>[`public nint Inno.Native.Sdl3.SDLGLContextState.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2657) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6cd2c4807503fefa"></a>[`public bool Inno.Native.Sdl3.SDLGLContextState.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2658) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-64c5bb472a0f3539"></a>[`public static Inno.Native.Sdl3.SDLGLContextState Inno.Native.Sdl3.SDLGLContextState.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2659) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9557c67c482c7848"></a>[`public readonly struct Inno.Native.Sdl3.SDLGLContextState : System.IEquatable<Inno.Native.Sdl3.SDLGLContextState>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L2650) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBlendFactor</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBlendFactor 70f4c3dc160bffe3c9165c7186564e6ce99ba21c98faed12b016b5dfb2e3a7f5 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorInvalid f7b928cb68c35fa932847f28417ceaa4ddd9f434a559e9e3b8aea1ec6d8de72b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorZero 8533c0216166ea7069b9ec207e4b431e055be2cd94f673214939879e8d9a0594 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorOne ea3419167cdec8bc64b64bf6dddc5fd3236d99e4356bcf0f0c8a44f74861a0e9 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorSrcColor 7798afe11fd4a1143c3867ec3be4adca93395ae8ecc932994d240e697c932a79 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorOneMinusSrcColor e69a3229e481ede2cbcea38a9c8c30cc2bf831a139b8285b3944710fa78ac875 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorDstColor 83ebd244e241801c7d596aeeb5b40937e4e16a30998fbcd28288ccd6a73c8a2b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorOneMinusDstColor 0adf06a5287d69444c0b7b730711c1a61cd9cc11ba47d2dfce4ffea45566c083 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorSrcAlpha e6aab77d8e00f37c4a7abd9d444f43b332b9be544c69a2d4fc4ee3d4b5b1180e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorOneMinusSrcAlpha c78c12554ad666d166dcc543f1c404670a2edffc8d69971e07546adc10ee33d7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorDstAlpha 61f287b34107d58215459f8990c8b6a8e98df000b9e7361fffceb3d118cb0ed2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorOneMinusDstAlpha 392e7d8d731a4242e4b2aad52fe0b9f83b8d58c9a9fd4e6d1ff1306e94defc45 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorConstantColor 7a302898f114bc95fe6c1a4d3d51376fecf7bf102d296b728f6377754da1f4dd -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorOneMinusConstantColor e13e266790709a0c2267f61109facad7022cf944666bfe50986347b90620d449 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorSrcAlphaSaturate 46211c04038e7837e3ab80e8e9e8df6fd1a590302ac3d0f31c12ec4178740b3a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e77fe3cd2378d547"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorConstantColor = 11`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1686) | 当前常量或枚举值：`11`。 | — |
| <a id="api-946dd2b99e753dbb"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorDstAlpha = 9`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1684) | 当前常量或枚举值：`9`。 | — |
| <a id="api-ac9df78d9dc413f8"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorDstColor = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1680) | 当前常量或枚举值：`5`。 | — |
| <a id="api-bcfa4ab9a8eb8c12"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorInvalid = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1675) | 当前常量或枚举值：`0`。 | — |
| <a id="api-c827e0e9b7979f2b"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorOne = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1677) | 当前常量或枚举值：`2`。 | — |
| <a id="api-ef9ed4a6409a7f4d"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorOneMinusConstantColor = 12`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1687) | 当前常量或枚举值：`12`。 | — |
| <a id="api-0dd092c740d43504"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorOneMinusDstAlpha = 10`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1685) | 当前常量或枚举值：`10`。 | — |
| <a id="api-a1ba70774c64a9fb"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorOneMinusDstColor = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1681) | 当前常量或枚举值：`6`。 | — |
| <a id="api-583e6db0bbb656e7"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorOneMinusSrcAlpha = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1683) | 当前常量或枚举值：`8`。 | — |
| <a id="api-beda5eed17724c23"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorOneMinusSrcColor = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1679) | 当前常量或枚举值：`4`。 | — |
| <a id="api-64fbf9c695c35877"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorSrcAlpha = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1682) | 当前常量或枚举值：`7`。 | — |
| <a id="api-035bec480c2a3288"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorSrcAlphaSaturate = 13`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1688) | 当前常量或枚举值：`13`。 | — |
| <a id="api-7ce71b43403945c5"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorSrcColor = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1678) | 当前常量或枚举值：`3`。 | — |
| <a id="api-80b47e4e862433f2"></a>[`Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorZero = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1676) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d75c01692b75dbff"></a>[`public enum Inno.Native.Sdl3.SDLGPUBlendFactor : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1673) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBlendOp</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBlendOp 1e9b10ea0cadd9914c9fdc2b268d67ad921622252249c7bf611a822bb00a91d7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendOp.BlendopInvalid 539acba4c7b235c3509213735ef3108ae810f30e86a3c228605487fde98e05e5 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendOp.BlendopAdd c9413c7aa43f35e8830a63091976688e661353aeb53f8fa16d7e2d9153bf5852 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendOp.BlendopSubtract fc766c1ce49f6f104acd639342095a368de17bdab0e3c02c5c19f038ee375a0b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendOp.BlendopReverseSubtract 4e472fac1c66c32c2137e07cdea472741e9e86d9239e597bf848d920b3956242 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendOp.BlendopMin 4eedbfe2143c44d12c2764ba48928a1873e6540ebc41590b1b12799bbda0976c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlendOp.BlendopMax b8085bbc88c3097e0e667b17e01a7bd4e49f1056299b8173ce4c91c2312e1538 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-7ce33637492b6c1f"></a>[`Inno.Native.Sdl3.SDLGPUBlendOp.BlendopAdd = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1663) | 当前常量或枚举值：`1`。 | — |
| <a id="api-df3f42f3b811b991"></a>[`Inno.Native.Sdl3.SDLGPUBlendOp.BlendopInvalid = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1662) | 当前常量或枚举值：`0`。 | — |
| <a id="api-cebef967a6cfd32d"></a>[`Inno.Native.Sdl3.SDLGPUBlendOp.BlendopMax = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1667) | 当前常量或枚举值：`5`。 | — |
| <a id="api-ca47a98ef15a6c49"></a>[`Inno.Native.Sdl3.SDLGPUBlendOp.BlendopMin = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1666) | 当前常量或枚举值：`4`。 | — |
| <a id="api-ee96fbca63e902e1"></a>[`Inno.Native.Sdl3.SDLGPUBlendOp.BlendopReverseSubtract = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1665) | 当前常量或枚举值：`3`。 | — |
| <a id="api-91ca034c70e1f91b"></a>[`Inno.Native.Sdl3.SDLGPUBlendOp.BlendopSubtract = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1664) | 当前常量或枚举值：`2`。 | — |
| <a id="api-2cf63675cc166bcc"></a>[`public enum Inno.Native.Sdl3.SDLGPUBlendOp : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1660) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBlitInfo</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBlitInfo 5e0826a31cb2a89fcc0cfcd6ab0dff3444e4546f4edd4218cb91adc9695eb736 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Source 4b76d6610e21dcf8cf3b26823b1547f70b191649ff5239a7f22f6fe6aa573ccc -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Destination f19564b596097298cc51be0d4ebe1c96f9c23e2463dbfce0612ed8c7f824002e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.LoadOp 5e93a331cec493cb834263897d633a5208173e0484bd1b5261ae1a9fbca797b3 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.ClearColor 71f88b95228c2a266e9bfd6c96c22553710f9e437effe6956bd97213de4e7b22 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.FlipMode ee8fcead0e79fb00db78680eb115cdf19fee91e8bf4870887628ce8de72ebffc -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Filter 47d143330f096ce26b8503b4d6c91767ad1ffd3ff0a945e9f4e34dc0b43e9144 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Cycle 3ba65142612bbac0aab46a9ce190215f305bb5fd9cf2c9d20e3fc156ef2bd168 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Padding1 8da221ecc372431156f8f14cb9a0d46ecb46d8d450e2fccc7748bcaa22970fca -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Padding2 ab48213d20d461c60fd095e6a0ae421215eedd33c64ce91cd12b8af4c95f3a0c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfo.Padding3 cf4ebe5322aad7f502134c0eabbc32babd331d4556cfe43635175da47e202e17 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBlitInfo.#ctor(Inno.Native.Sdl3.SDLGPUBlitRegion,Inno.Native.Sdl3.SDLGPUBlitRegion,Inno.Native.Sdl3.SDLGPULoadOp,Inno.Native.Sdl3.SDLFColor,Inno.Native.Sdl3.SDLFlipMode,Inno.Native.Sdl3.SDLGPUFilter,System.Byte,System.Byte,System.Byte,System.Byte) 8031c15ae6baa98836858f262c074e1ebbfd56bd36d2a8f192b1cbf6eac96938 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ced264f119073f55"></a>[`public Inno.Native.Sdl3.SDLFColor Inno.Native.Sdl3.SDLGPUBlitInfo.ClearColor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5486) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-612331c85dc2d651"></a>[`public byte Inno.Native.Sdl3.SDLGPUBlitInfo.Cycle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5489) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0582b80b3a842409"></a>[`public Inno.Native.Sdl3.SDLGPUBlitRegion Inno.Native.Sdl3.SDLGPUBlitInfo.Destination`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5484) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e8f76ddff7113016"></a>[`public Inno.Native.Sdl3.SDLGPUFilter Inno.Native.Sdl3.SDLGPUBlitInfo.Filter`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5488) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4032df9a7cc17eab"></a>[`public Inno.Native.Sdl3.SDLFlipMode Inno.Native.Sdl3.SDLGPUBlitInfo.FlipMode`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5487) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-98c5ac9518c05df6"></a>[`public Inno.Native.Sdl3.SDLGPULoadOp Inno.Native.Sdl3.SDLGPUBlitInfo.LoadOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5485) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-68acb939abaee135"></a>[`public byte Inno.Native.Sdl3.SDLGPUBlitInfo.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5490) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-04416342950f72d3"></a>[`public byte Inno.Native.Sdl3.SDLGPUBlitInfo.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5491) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-370346dba7c62aa0"></a>[`public byte Inno.Native.Sdl3.SDLGPUBlitInfo.Padding3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5492) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5df08f93df8fe0ce"></a>[`public Inno.Native.Sdl3.SDLGPUBlitRegion Inno.Native.Sdl3.SDLGPUBlitInfo.Source`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5483) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b378dad5a56cd2c9"></a>[`public Inno.Native.Sdl3.SDLGPUBlitInfo.SDLGPUBlitInfo(Inno.Native.Sdl3.SDLGPUBlitRegion source = default(Inno.Native.Sdl3.SDLGPUBlitRegion), Inno.Native.Sdl3.SDLGPUBlitRegion destination = default(Inno.Native.Sdl3.SDLGPUBlitRegion), Inno.Native.Sdl3.SDLGPULoadOp load_op = Inno.Native.Sdl3.SDLGPULoadOp.LoadopLoad, Inno.Native.Sdl3.SDLFColor clear_color = default(Inno.Native.Sdl3.SDLFColor), Inno.Native.Sdl3.SDLFlipMode flip_mode = Inno.Native.Sdl3.SDLFlipMode.None, Inno.Native.Sdl3.SDLGPUFilter filter = Inno.Native.Sdl3.SDLGPUFilter.Nearest, byte cycle = 0, byte padding1 = 0, byte padding2 = 0, byte padding3 = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5493) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bdc08f467029a7f6"></a>[`public struct Inno.Native.Sdl3.SDLGPUBlitInfo`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5481) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBlitInfoPtr</code> · 23 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBlitInfoPtr a3b533a67dcd33c526a5026ea39e7d285075420512fdd1d69e30fff47737cc7f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.#ctor(Inno.Native.Sdl3.SDLGPUBlitInfo*) 95f2853573dea6a7e5db392c0933be2df3fab4da4b9337274803e2cc483c2bc7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Handle 1868c84168ccf47069f219de1aba7edbca452f3266e1ea890e6bfd308f6e23f5 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.IsNull 733ce943a60b00835f5939e2ad4ab7e940e6505707efde569ba7d0a6b604b75e -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Null 697a328a6ae7b21261f6932bc2c9d7ac425d7bf22551c56e736d19c85d472dcb -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Item(System.Int32) 833d55356cfdf7de5b373f8facec59cff13b5a95688fec39f7fce380204e75e6 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUBlitInfo*)~Inno.Native.Sdl3.SDLGPUBlitInfoPtr c21f53c19a4ee62417aa2de1daa52afc1330dcf3ee795a0b7e0d1f826cef1175 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUBlitInfoPtr)~Inno.Native.Sdl3.SDLGPUBlitInfo* d1d59fdf72408a18b34b7bcba1250d54b89a4fc3744894db3aea5c31050d80ca -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.op_Equality(Inno.Native.Sdl3.SDLGPUBlitInfoPtr,Inno.Native.Sdl3.SDLGPUBlitInfoPtr) 7f2ff7cbf4b59bff87550062dc2e6713e48ded28cf318823cfb73edf60c45522 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUBlitInfoPtr,Inno.Native.Sdl3.SDLGPUBlitInfoPtr) 749ec128ad5932b52a2d1157e9fe4a70c13f0e438654e5370c929da3040bd1e5 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUBlitInfoPtr) 0f9969cc61283d72818c4069498c7af9536ac3a97ef16f81721ba22a401ed436 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Equals(System.Object) 8997f4bbcd2a6723ee4d263f375e9a5346beef7d912a1d409737dcbb128370b5 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.GetHashCode ff186220ef54efc4c0f2bbe82dc458470d9a3e1d7a43895fbb20d3240272169b -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Source bf677a522c26d88908c5a533fe581eeb62eb62f7e38ff2fc1ae7201e6334478c -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Destination db8411039311d2b4da3f642d04a5291a58bc48a6a3fc4f174cdaab20c6d7ba2b -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.LoadOp c6369eac911cef187ff26845ba093123a41bbd1a8f06af544cbd8b797e9c410c -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.ClearColor 2272e5590cdf259a50f17ed3e44d810073729ecc664254a8b1b0b08aa07b2380 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.FlipMode f1469efc0ca7fb7478d2443e2b542896208c99ba7e0d364be31d50c596adc8f9 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Filter 8158788231e8b9598bb5dab532070873cada793bec33462a25fabf62b52d7296 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Cycle b61268c9f23d7bca1f311fbb9bb3c4dad53df63e8ec2cd3922910306ae259f03 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Padding1 5f4e7165f6c0815bc3375e12f2641e70b010337a9ea81dd730151cb33d8eb1ba -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Padding2 6881b275f98ab5a5e9f0c294bf44f942e5604be3900fcf9006f1c73e5a387dc4 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Padding3 6914d2bb58c0347aef60ae09bbccea50a4823eafc437a31e8e7750f4351e3f70 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c0b15d2661c72255"></a>[`public Inno.Native.Sdl3.SDLGPUBlitInfo* Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8432) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-879e8cd9c90a9e4e"></a>[`public Inno.Native.Sdl3.SDLGPUBlitInfoPtr.SDLGPUBlitInfoPtr(Inno.Native.Sdl3.SDLGPUBlitInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8427) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-91f70f38e363cf5a"></a>[`public bool Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUBlitInfoPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8442) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-a6a8757cde7947c2"></a>[`public override bool Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8443) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-f4743b70388906bc"></a>[`public override int Inno.Native.Sdl3.SDLGPUBlitInfoPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8444) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-c9a7bf5cb5f8fa56"></a>[`public static bool Inno.Native.Sdl3.SDLGPUBlitInfoPtr.operator ==(Inno.Native.Sdl3.SDLGPUBlitInfoPtr left, Inno.Native.Sdl3.SDLGPUBlitInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8440) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5ca0a5b80e573f59"></a>[`public static Inno.Native.Sdl3.SDLGPUBlitInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUBlitInfoPtr(Inno.Native.Sdl3.SDLGPUBlitInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8438) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-91c578161da26813"></a>[`public static Inno.Native.Sdl3.SDLGPUBlitInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUBlitInfo*(Inno.Native.Sdl3.SDLGPUBlitInfoPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8439) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-95f22ea4adb07b47"></a>[`public static bool Inno.Native.Sdl3.SDLGPUBlitInfoPtr.operator !=(Inno.Native.Sdl3.SDLGPUBlitInfoPtr left, Inno.Native.Sdl3.SDLGPUBlitInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8441) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f46d258f985bbfb6"></a>[`public ref Inno.Native.Sdl3.SDLFColor Inno.Native.Sdl3.SDLGPUBlitInfoPtr.ClearColor { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8449) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-66eb384835b27ad7"></a>[`public bool Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Cycle { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8452) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d8fbb8302d9ad7d5"></a>[`public ref Inno.Native.Sdl3.SDLGPUBlitRegion Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Destination { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8447) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2717be7f4d34fb32"></a>[`public ref Inno.Native.Sdl3.SDLGPUFilter Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Filter { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8451) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b27f2e4a8e1cb456"></a>[`public ref Inno.Native.Sdl3.SDLFlipMode Inno.Native.Sdl3.SDLGPUBlitInfoPtr.FlipMode { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8450) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-59c0d160faeabc02"></a>[`public bool Inno.Native.Sdl3.SDLGPUBlitInfoPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8433) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-beb41dbbc361e332"></a>[`public Inno.Native.Sdl3.SDLGPUBlitInfo Inno.Native.Sdl3.SDLGPUBlitInfoPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8436) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-57b0c925fbb37359"></a>[`public ref Inno.Native.Sdl3.SDLGPULoadOp Inno.Native.Sdl3.SDLGPUBlitInfoPtr.LoadOp { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8448) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3b6ec0e1d2f7e1bb"></a>[`public static Inno.Native.Sdl3.SDLGPUBlitInfoPtr Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8434) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b138a615fe2459cf"></a>[`public ref byte Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Padding1 { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8453) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2d7ddb2104e8b32a"></a>[`public ref byte Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Padding2 { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8454) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f035f4d30981e34a"></a>[`public ref byte Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Padding3 { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8455) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-af3691ba5d0649fd"></a>[`public ref Inno.Native.Sdl3.SDLGPUBlitRegion Inno.Native.Sdl3.SDLGPUBlitInfoPtr.Source { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8446) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-10ea44adc21e590d"></a>[`public struct Inno.Native.Sdl3.SDLGPUBlitInfoPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUBlitInfoPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8425) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBlitRegion</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBlitRegion 6d2d578c6942a797237129fc30275d10281b8ffce9e1410ac0c6d92b40c2b613 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.Texture 8931805af5248bf6ccc896bb3b9379d073c26748442e970fca3c8815e141ead4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.MipLevel e6dd900e83facab8bae9376d3a668cf83f2975538dd6a8faf252a8ddc366854e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.LayerOrDepthPlane 0aa791756bbf4f30ecf14efdad1e44529dfdc34add2c430927149e778f0b2a17 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.X d6125dae92953ae70222328a5d0e1ee743c089bd5e9f7af44d7ff1b5ef388307 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.Y b2a26150821eb1f498377760a3bddb12fc5871d688e6b18b0f60740019381db7 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.W 4683ff9b140bf8cd7ee2afa1387cbf08d1250b034e11e7c4572758ce942584db -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBlitRegion.H c62e86db00691ffc5276c9759411e459c68dc051da3d81cf74d6b735caec1e3e -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBlitRegion.#ctor(Inno.Native.Sdl3.SDLGPUTexture,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32) e533db3f3a5bf12a8e21548dae1ad16273268459f3fe81bf73e9d733072c9270 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-dd40e02837e39b76"></a>[`public uint Inno.Native.Sdl3.SDLGPUBlitRegion.H`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4866) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8b2e1bd02f9f6edd"></a>[`public uint Inno.Native.Sdl3.SDLGPUBlitRegion.LayerOrDepthPlane`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4862) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4748f3f57bb178d1"></a>[`public uint Inno.Native.Sdl3.SDLGPUBlitRegion.MipLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4861) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ad5b992ab427f16"></a>[`public Inno.Native.Sdl3.SDLGPUTexture Inno.Native.Sdl3.SDLGPUBlitRegion.Texture`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4860) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-94cfbc6b8f6df79b"></a>[`public uint Inno.Native.Sdl3.SDLGPUBlitRegion.W`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4865) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c40df929d7f169b"></a>[`public uint Inno.Native.Sdl3.SDLGPUBlitRegion.X`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4863) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0956c56464adf908"></a>[`public uint Inno.Native.Sdl3.SDLGPUBlitRegion.Y`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4864) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7903184b9d74d409"></a>[`public Inno.Native.Sdl3.SDLGPUBlitRegion.SDLGPUBlitRegion(Inno.Native.Sdl3.SDLGPUTexture texture = default(Inno.Native.Sdl3.SDLGPUTexture), uint mip_level = 0, uint layer_or_depth_plane = 0, uint x = 0, uint y = 0, uint w = 0, uint h = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4867) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d682ba40b4c38e28"></a>[`public struct Inno.Native.Sdl3.SDLGPUBlitRegion`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4858) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBuffer</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBuffer 0cb32a2175a2dabd87a7d958458843b8eb9f630729804a96484d006596bbe333 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBuffer.#ctor(System.IntPtr) 3540b12ca32e1a88dd0ad17bdcabc1bef5d9f8d594fd9e385476f6051c825dbf -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBuffer.Handle 66e433fdfd3c88ce7947301d56cc40e448313340f8cbb061b273d9a47d6700d8 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBuffer.IsNull 1612d2e1c173f815c106bc369893ddf9a0316b3a208f69463501130570dd32f1 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBuffer.Null a5d93e9087a43f2093e4358375ef025c214af617a2bba75dd3c202269c7c2478 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBuffer.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLGPUBuffer 2cab75ba47e34c5c4f1901a4b98443025363040b9d1146362c1e0afdc10acc30 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBuffer.op_Equality(Inno.Native.Sdl3.SDLGPUBuffer,Inno.Native.Sdl3.SDLGPUBuffer) d097391b8b874d92d6ce416129303da075e3da70ebb1591004eccf46b50e5046 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBuffer.op_Inequality(Inno.Native.Sdl3.SDLGPUBuffer,Inno.Native.Sdl3.SDLGPUBuffer) be7aed20512211f9708d39a0c39a572dc07a0464df8458a81f121875354e44ae -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBuffer.Equals(Inno.Native.Sdl3.SDLGPUBuffer) b79ce1a17762b16b8107c459c2906dc1e1b9d0bd54bf9eff089ffb35c50f3886 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBuffer.Equals(System.Object) 9b46b41071bff489cece662c0beeb4d6d701d27bf6c1567a980ccf670ec3326b -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBuffer.GetHashCode 27b23748ce6addffdcd46628ca9df199cc84ff207ed943c9d7cd08c12725a585 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3374f86dea69a12d"></a>[`public Inno.Native.Sdl3.SDLGPUBuffer.SDLGPUBuffer(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4463) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-d2cd40a590f6a5e6"></a>[`public bool Inno.Native.Sdl3.SDLGPUBuffer.Equals(Inno.Native.Sdl3.SDLGPUBuffer other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4475) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-805ff5648645fbf5"></a>[`public override bool Inno.Native.Sdl3.SDLGPUBuffer.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4476) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-d27591068ccbf089"></a>[`public override int Inno.Native.Sdl3.SDLGPUBuffer.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4477) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-7d99a1e27b18a14c"></a>[`public static bool Inno.Native.Sdl3.SDLGPUBuffer.operator ==(Inno.Native.Sdl3.SDLGPUBuffer left, Inno.Native.Sdl3.SDLGPUBuffer right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4473) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d79ab0a9429b0ef9"></a>[`public static Inno.Native.Sdl3.SDLGPUBuffer.implicit operator Inno.Native.Sdl3.SDLGPUBuffer(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4472) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe203a62f64d1ba0"></a>[`public static bool Inno.Native.Sdl3.SDLGPUBuffer.operator !=(Inno.Native.Sdl3.SDLGPUBuffer left, Inno.Native.Sdl3.SDLGPUBuffer right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4474) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bcc28c20e783a6dd"></a>[`public nint Inno.Native.Sdl3.SDLGPUBuffer.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4468) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e66d9050ccaccde9"></a>[`public bool Inno.Native.Sdl3.SDLGPUBuffer.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4469) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-123907f4465b50b4"></a>[`public static Inno.Native.Sdl3.SDLGPUBuffer Inno.Native.Sdl3.SDLGPUBuffer.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4470) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3490aa12ebba5d75"></a>[`public readonly struct Inno.Native.Sdl3.SDLGPUBuffer : System.IEquatable<Inno.Native.Sdl3.SDLGPUBuffer>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4461) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBufferBinding</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBufferBinding f1c671695cd2073e83afbcd67b62ca3cee5ca3ef062e6aae3592055f838e319f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferBinding.Buffer 217c75985050f6ed5e2afce13a2eea48dff469f3f594c72a32e5fada34736d81 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferBinding.Offset 6c7dc82f8a0549ea7b300875cd591784db647ea69df211de04c29f0551f2daff -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferBinding.#ctor(Inno.Native.Sdl3.SDLGPUBuffer,System.UInt32) 9c2537d030c9743349e2d7ec04d9d81973b22d4fac2d54bbb4d3f87e187f3c47 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-928cbdd1d93557ae"></a>[`public Inno.Native.Sdl3.SDLGPUBuffer Inno.Native.Sdl3.SDLGPUBufferBinding.Buffer`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5514) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a4a85d71e9224986"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferBinding.Offset`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5515) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-94c6f3d9e88c1e2b"></a>[`public Inno.Native.Sdl3.SDLGPUBufferBinding.SDLGPUBufferBinding(Inno.Native.Sdl3.SDLGPUBuffer buffer = default(Inno.Native.Sdl3.SDLGPUBuffer), uint offset = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5516) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-47e147712a9ccdf4"></a>[`public struct Inno.Native.Sdl3.SDLGPUBufferBinding`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5512) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBufferBindingPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBufferBindingPtr 45bba927ddb4cd3e1bf36d0f99adc7c035a2f786c3e3ac6ab1a09eff575cd134 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.#ctor(Inno.Native.Sdl3.SDLGPUBufferBinding*) 7a8e17531e90c1b10cbbd2c21e0c0b431f33580ecd1bf218aa1823d26424b554 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.Handle 521aaf56d5f92272030827e83c0f7d9555eb7af29c674153efcd259736da4753 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.IsNull 280ceae17013773e1afcc0276ff80d969a7a6a97b95da741ba2e7cf05081da98 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.Null 2427e677e43c7a4d6dbd4cca9e138262bb20e1f49f66be9e0ca024776fc35b00 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.Item(System.Int32) 4a56017ef99bac184b624e77b05eed2e6495db770de4391c58e5f5986dd10bad -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUBufferBinding*)~Inno.Native.Sdl3.SDLGPUBufferBindingPtr a750c093650354a846e83c870a2ee9eddd4d456a8adc7c24d163954c50471dd7 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUBufferBindingPtr)~Inno.Native.Sdl3.SDLGPUBufferBinding* 86268a3c1c5e3d8dd30eb8103a45d49ab3c67eeca9b7f72cc177dc2550891950 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.op_Equality(Inno.Native.Sdl3.SDLGPUBufferBindingPtr,Inno.Native.Sdl3.SDLGPUBufferBindingPtr) ddcc51e7085d4cedcf8acbcddeb3bca81abdbea55544f4f39258020c519649ec -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUBufferBindingPtr,Inno.Native.Sdl3.SDLGPUBufferBindingPtr) a912118724891c05c97b55695f340ffa3cc84f26f802bfc71add52538129d46c -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.Equals(Inno.Native.Sdl3.SDLGPUBufferBindingPtr) 81938d09c70887977fb75399fce5ca12d7386872c0547d589657592c48b5889a -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.Equals(System.Object) e5301387738f336695614e13f3f36513bbdad75fdb9031110e632192d453d3f8 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.GetHashCode d24d8cc83d321cd4cca73752a6d95e4f362f2bd8623296fb15d138a64f246a91 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.Buffer 5f5038f4d4a447dc7155217da8f2b60487d68eb9a10f1247bed17b2c9637b500 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferBindingPtr.Offset 1ba1e660d4574575182fccbe6b5b3f147980b080c76ee3c5a0726e7604c87cf9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-08947d2e787b266d"></a>[`public Inno.Native.Sdl3.SDLGPUBufferBinding* Inno.Native.Sdl3.SDLGPUBufferBindingPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8466) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-947d06585fdeb1b2"></a>[`public Inno.Native.Sdl3.SDLGPUBufferBindingPtr.SDLGPUBufferBindingPtr(Inno.Native.Sdl3.SDLGPUBufferBinding* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8461) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-25971f7aee5adcd1"></a>[`public bool Inno.Native.Sdl3.SDLGPUBufferBindingPtr.Equals(Inno.Native.Sdl3.SDLGPUBufferBindingPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8476) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-2da240edde7f0821"></a>[`public override bool Inno.Native.Sdl3.SDLGPUBufferBindingPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8477) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-ed9201fa08dc46c5"></a>[`public override int Inno.Native.Sdl3.SDLGPUBufferBindingPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8478) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-2f6b16485f726fab"></a>[`public static bool Inno.Native.Sdl3.SDLGPUBufferBindingPtr.operator ==(Inno.Native.Sdl3.SDLGPUBufferBindingPtr left, Inno.Native.Sdl3.SDLGPUBufferBindingPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8474) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-891256c161a5c821"></a>[`public static Inno.Native.Sdl3.SDLGPUBufferBindingPtr.implicit operator Inno.Native.Sdl3.SDLGPUBufferBindingPtr(Inno.Native.Sdl3.SDLGPUBufferBinding* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8472) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-48935fef414b445c"></a>[`public static Inno.Native.Sdl3.SDLGPUBufferBindingPtr.implicit operator Inno.Native.Sdl3.SDLGPUBufferBinding*(Inno.Native.Sdl3.SDLGPUBufferBindingPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8473) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-68db9b687ee9370c"></a>[`public static bool Inno.Native.Sdl3.SDLGPUBufferBindingPtr.operator !=(Inno.Native.Sdl3.SDLGPUBufferBindingPtr left, Inno.Native.Sdl3.SDLGPUBufferBindingPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8475) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8b84e462d04bced7"></a>[`public ref Inno.Native.Sdl3.SDLGPUBuffer Inno.Native.Sdl3.SDLGPUBufferBindingPtr.Buffer { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8480) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d8d3ee0da8f20d7f"></a>[`public bool Inno.Native.Sdl3.SDLGPUBufferBindingPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8467) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8799449e468bebf5"></a>[`public Inno.Native.Sdl3.SDLGPUBufferBinding Inno.Native.Sdl3.SDLGPUBufferBindingPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8470) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-796b7fe41a746cf5"></a>[`public static Inno.Native.Sdl3.SDLGPUBufferBindingPtr Inno.Native.Sdl3.SDLGPUBufferBindingPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8468) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0e4702439e4aa2d0"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUBufferBindingPtr.Offset { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8481) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7da4fb3e05893835"></a>[`public struct Inno.Native.Sdl3.SDLGPUBufferBindingPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUBufferBindingPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8459) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBufferCreateInfo</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBufferCreateInfo 9c86ddd5576d2162da433f53e771d81deb577f36218055dc0549cabe303e0097 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferCreateInfo.Usage 1d48ce5ecab6b455e66792ec96109211180714ef4fdb06b552e7141917c5b7b8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferCreateInfo.Size 596ff9694b992b82f3f70bbcc857d4f86bf3e9cdb782a4ec894110f7338b1480 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferCreateInfo.Props 79cfb80a6cd4c2e6b90e091d4f0dd217771139ff085fa6c767db242760a1e12b -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferCreateInfo.#ctor(System.UInt32,System.UInt32,System.UInt32) a4a8dfee0cd7fb24719af7a5440bca0a275a147f9421450bea9c73137b85b17d -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-2611177a924db3a4"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferCreateInfo.Props`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5188) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-86f4737412136484"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferCreateInfo.Size`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5187) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8af81c004445463f"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferCreateInfo.Usage`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5186) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f73ab3d7dde126f"></a>[`public Inno.Native.Sdl3.SDLGPUBufferCreateInfo.SDLGPUBufferCreateInfo(uint usage = 0, uint size = 0, uint props = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5189) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-cb32ac7a094cf2df"></a>[`public struct Inno.Native.Sdl3.SDLGPUBufferCreateInfo`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5184) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr 7b13496ae437fa31f4bee03705cc72a40d2459a5e66a1ea4acddba92f2f8d6ef -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.#ctor(Inno.Native.Sdl3.SDLGPUBufferCreateInfo*) 89d193e9c8cb278349bab1dc76be86510c0a82808762b3ad9b6313746ebb045f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Handle c5a3eff5fcc274dfd703dd4757e51418c394abdbb41072eed16c72764d01442a -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.IsNull 3b193ae0e6874e571349399cfe891308c9e45e6430f3ff43a4b38c0211c5efa1 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Null f4aa6a1054c5fff66117898294913789498ed337e973cdad1ab96f2ee291e20a -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Item(System.Int32) d0cc0b87cb2c66c36c0b05420a0d08e0273c7be2fac6a88756e6a86edb1d9156 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUBufferCreateInfo*)~Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr 161b449aff588d55ba38a40e576f690e603a7786046e0ef98d9a17df7a85e946 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr)~Inno.Native.Sdl3.SDLGPUBufferCreateInfo* e4c60b3ea302a47caab75bc85a558fa6f5b9872cd5a90f7dc2f863b44a4162de -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.op_Equality(Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr,Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr) 68089d1d9ce8197ecca215988ba9764ac9f67807d093a4f409a1e8a91d886910 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr,Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr) a5aa576699e8f06fd06b95ee6ded109df76afda0058e12b3ec3df49eb8f31191 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr) 6fde6648717fc946d4fa7616378a9595ff4ebb409612fadd1e83340b48e42a5f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Equals(System.Object) cf691dd754b015fed966f3d561c0965499397ccc9553803402ae38a40a9337a8 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.GetHashCode ee3055f35b2ea477ff92fa3fe5c4f5cd03619cf1cf4686952ab9954449a4f79c -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Usage 82426d9ea9dd1cbd4deb057b6c0afa36e15d8ad889cfd0ce78d66bf6c5f38a45 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Size ccc5922748da417c0080e338ad68ff9556abfca4f20f920da02e16db598d7d68 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Props 2076e4184919674451a16419b9488076941faf7a415666e8125ebd7aeba22d99 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-e9239bf846334234"></a>[`public Inno.Native.Sdl3.SDLGPUBufferCreateInfo* Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8492) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a73ffa13843b6aa8"></a>[`public Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.SDLGPUBufferCreateInfoPtr(Inno.Native.Sdl3.SDLGPUBufferCreateInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8487) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-68d9dc94a91a16ba"></a>[`public bool Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8502) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-64778976e75cd70b"></a>[`public override bool Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8503) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-96e171753ee72eb8"></a>[`public override int Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8504) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-a8f15355da25bece"></a>[`public static bool Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.operator ==(Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr left, Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8500) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-7a6920d6322ad9f6"></a>[`public static Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr(Inno.Native.Sdl3.SDLGPUBufferCreateInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8498) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bd565dc1bf774610"></a>[`public static Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUBufferCreateInfo*(Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8499) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-013211fe3edba9f7"></a>[`public static bool Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.operator !=(Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr left, Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8501) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-a0da00c00322d1f1"></a>[`public bool Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8493) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6add41f5d0ca93f8"></a>[`public Inno.Native.Sdl3.SDLGPUBufferCreateInfo Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8496) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a8b56700f6b9020e"></a>[`public static Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8494) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0b5d13761c85994f"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Props { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8508) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-957d7ee46e45cc83"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Size { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8507) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f6eadc98ce7f59ae"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr.Usage { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8506) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a8cc334a3d3f2bd6"></a>[`public struct Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUBufferCreateInfoPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8485) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBufferLocation</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBufferLocation c3894a2fe6a48b327f8509a145d48c6e9f8fb03bef36bdf314fe5f00c4ddeaf0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferLocation.Buffer cbdb195d9d2065c14588a18787dc34e0ac59c4020cf7694b674d7e2f24d6b9b8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferLocation.Offset 052c57d5e9f81a564f85a4b31775ef0663d7492709235e5e7485bdcff3435363 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferLocation.#ctor(Inno.Native.Sdl3.SDLGPUBuffer,System.UInt32) fedde08684d6a314e50fd9717c67559f02bd3ca386e1e0b0c4b82ef3490a17d3 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-3e6fd62dff56660c"></a>[`public Inno.Native.Sdl3.SDLGPUBuffer Inno.Native.Sdl3.SDLGPUBufferLocation.Buffer`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4885) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c7a66eea8a3acabb"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferLocation.Offset`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4886) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8bbf7d174d090d3b"></a>[`public Inno.Native.Sdl3.SDLGPUBufferLocation.SDLGPUBufferLocation(Inno.Native.Sdl3.SDLGPUBuffer buffer = default(Inno.Native.Sdl3.SDLGPUBuffer), uint offset = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4887) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a092f253ecbd2fac"></a>[`public struct Inno.Native.Sdl3.SDLGPUBufferLocation`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4883) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBufferLocationPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBufferLocationPtr fc70b96a70c0024b8e29a4098bf4ca7086fd6238346d0f8215a8c1996d803d73 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.#ctor(Inno.Native.Sdl3.SDLGPUBufferLocation*) 4ffac569c1d4aecacd639a18f76e5a3a045050e9909606364cafc7dd4cf3c6a8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.Handle 13bef0b5b6df79fd442bfdcfd0b4f0a4fff23a03d0e9ce84b0bf7f88cde6a182 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.IsNull cf8f9c841873646f3c4851e7f24ed88b2610d1ac9d014119c5f0afe6dc6b8c59 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.Null fd5719db8af901e6a96248cd49dbb9ca1291a660ccd2ac3c01e597b6fa41313e -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.Item(System.Int32) 8d8df61323b4a1436c23b2dc770c7e8758888e8dd5b9053eb24733d736b0d1c3 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUBufferLocation*)~Inno.Native.Sdl3.SDLGPUBufferLocationPtr 54b8f5fc8406f304ec84550a7321e945822adc185484230a55a9704ec88f6fc8 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUBufferLocationPtr)~Inno.Native.Sdl3.SDLGPUBufferLocation* 9df955e9e945bbc6660eb36d5d33e8f4882cc086317da047a60f2f1c714c6c3a -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.op_Equality(Inno.Native.Sdl3.SDLGPUBufferLocationPtr,Inno.Native.Sdl3.SDLGPUBufferLocationPtr) 19a97975fdcc62d699102f4295a3fd05027ef1336f814512761fcc152031420b -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUBufferLocationPtr,Inno.Native.Sdl3.SDLGPUBufferLocationPtr) c7f559bb186c07554b2357eb73ee57a976c8650a1bab7e76960652928166a5cb -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.Equals(Inno.Native.Sdl3.SDLGPUBufferLocationPtr) 7ab7d64d0abc85039895c8d7e2aa7eed7e8fb21a350de5f02cf810d0893b7631 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.Equals(System.Object) fac95299c1ec2cf357f0a90aa545e317b40842b1ba9616baffe6e674166cfeef -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.GetHashCode 8df0fe11902bf10a5b85342045b1fa102a067772b8dc53fed1bea59e58b7110b -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.Buffer c9e7b7863d3d67073ad045251b5a6b9c758d068696be214dcb88b14a6d95a395 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferLocationPtr.Offset bec72511e86f2b558bd15272eb41d1c3b97223a203aa26aa26aaed6f9f5667a1 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-94116a868b3ce26e"></a>[`public Inno.Native.Sdl3.SDLGPUBufferLocation* Inno.Native.Sdl3.SDLGPUBufferLocationPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8519) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ec8cdb1cf0edf459"></a>[`public Inno.Native.Sdl3.SDLGPUBufferLocationPtr.SDLGPUBufferLocationPtr(Inno.Native.Sdl3.SDLGPUBufferLocation* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8514) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-7df4b6ed6445b84a"></a>[`public bool Inno.Native.Sdl3.SDLGPUBufferLocationPtr.Equals(Inno.Native.Sdl3.SDLGPUBufferLocationPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8529) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-dcbb711a6a018bed"></a>[`public override bool Inno.Native.Sdl3.SDLGPUBufferLocationPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8530) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-867717f52aa9396f"></a>[`public override int Inno.Native.Sdl3.SDLGPUBufferLocationPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8531) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-e165c737d6a07be4"></a>[`public static bool Inno.Native.Sdl3.SDLGPUBufferLocationPtr.operator ==(Inno.Native.Sdl3.SDLGPUBufferLocationPtr left, Inno.Native.Sdl3.SDLGPUBufferLocationPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8527) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-465d00c6d01a5139"></a>[`public static Inno.Native.Sdl3.SDLGPUBufferLocationPtr.implicit operator Inno.Native.Sdl3.SDLGPUBufferLocationPtr(Inno.Native.Sdl3.SDLGPUBufferLocation* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8525) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-71b68d57bc9c6c33"></a>[`public static Inno.Native.Sdl3.SDLGPUBufferLocationPtr.implicit operator Inno.Native.Sdl3.SDLGPUBufferLocation*(Inno.Native.Sdl3.SDLGPUBufferLocationPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8526) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-39ac576523c195fb"></a>[`public static bool Inno.Native.Sdl3.SDLGPUBufferLocationPtr.operator !=(Inno.Native.Sdl3.SDLGPUBufferLocationPtr left, Inno.Native.Sdl3.SDLGPUBufferLocationPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8528) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-43f40be6560c0c1f"></a>[`public ref Inno.Native.Sdl3.SDLGPUBuffer Inno.Native.Sdl3.SDLGPUBufferLocationPtr.Buffer { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8533) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0e4b709bb6140828"></a>[`public bool Inno.Native.Sdl3.SDLGPUBufferLocationPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8520) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ffe240d5ad3e345d"></a>[`public Inno.Native.Sdl3.SDLGPUBufferLocation Inno.Native.Sdl3.SDLGPUBufferLocationPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8523) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-549ffea8ba569a39"></a>[`public static Inno.Native.Sdl3.SDLGPUBufferLocationPtr Inno.Native.Sdl3.SDLGPUBufferLocationPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8521) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-804c789b41f1fa1d"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUBufferLocationPtr.Offset { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8534) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-bcb5b704ad0c3339"></a>[`public struct Inno.Native.Sdl3.SDLGPUBufferLocationPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUBufferLocationPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8512) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBufferRegion</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBufferRegion 913d7ee254b07d6448a7b4ed5e6a4a0fd68a9a310c06ff1acca1c1deb978a581 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferRegion.Buffer 2cf704cefd5316534abab9a2430ead888d08f74ca79de0d409fcae4461992efb -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferRegion.Offset 79acf5cf3352a9aa36b817c1d375018979195b47adbbf7f990f6526e7c5d9703 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferRegion.Size b249809490737aacd589e6ac3b116a2ff301a4801d459273a446558a542c2f6c -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferRegion.#ctor(Inno.Native.Sdl3.SDLGPUBuffer,System.UInt32,System.UInt32) 7ebc0886d471cb9f9062a44b76f41f9a6a98856de3ed62fd836f6da2973ab373 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-699957de57d271e2"></a>[`public Inno.Native.Sdl3.SDLGPUBuffer Inno.Native.Sdl3.SDLGPUBufferRegion.Buffer`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4900) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f7456bfaa1d2c18"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferRegion.Offset`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4901) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-408bdb1dbd707586"></a>[`public uint Inno.Native.Sdl3.SDLGPUBufferRegion.Size`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4902) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c4db146b27138709"></a>[`public Inno.Native.Sdl3.SDLGPUBufferRegion.SDLGPUBufferRegion(Inno.Native.Sdl3.SDLGPUBuffer buffer = default(Inno.Native.Sdl3.SDLGPUBuffer), uint offset = 0, uint size = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4903) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-f3e149d4d1918ea3"></a>[`public struct Inno.Native.Sdl3.SDLGPUBufferRegion`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4898) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUBufferRegionPtr</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUBufferRegionPtr dad6c662a50852ec018a582556b79e469cb51cae99fabeec3b6da28ca94a0aae -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.#ctor(Inno.Native.Sdl3.SDLGPUBufferRegion*) 890727be8831d3fe448dec6699fa341ae934106f734b48416d3007637d2c10f9 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Handle ae1ff97fe349354f65971f4a18853144d141a0cf43d8964727fb181c2ae00263 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.IsNull c01fa0e87af92aa7c4edd1ea8325cdc3738a22dd65e70b342310d689fad7daea -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Null ac102b71c50727370569d61ee699ccb656f0f4cfc89e43486ced597143370e22 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Item(System.Int32) b12bad44ff6022a469c10a3377bfa52712758bf5ff1c3d0cee73df97f6949c67 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUBufferRegion*)~Inno.Native.Sdl3.SDLGPUBufferRegionPtr 640ffba2495ffd54fee23363b2b49272227d6ffe5660b84ec513714f42e07452 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUBufferRegionPtr)~Inno.Native.Sdl3.SDLGPUBufferRegion* 6355da3cc42aa66f78f56663cdbac8b3a983b301a4af776b711a1365477be30d -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.op_Equality(Inno.Native.Sdl3.SDLGPUBufferRegionPtr,Inno.Native.Sdl3.SDLGPUBufferRegionPtr) 36eeaec625ace2308d566e77a86b387fdebc577fcdf793faa12759b75db97da4 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUBufferRegionPtr,Inno.Native.Sdl3.SDLGPUBufferRegionPtr) 4c66ba245fa23f728b81d84296849ea234138b19b59d44d512403c83cb63c517 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Equals(Inno.Native.Sdl3.SDLGPUBufferRegionPtr) babdf72b196d98d35e99bf1d65315ffa5ab6c13cfa0611e2f627d0b14da06707 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Equals(System.Object) 27468f73304387d33ce46bbe536e913c5a8a943feddd0f95561548c26db5fe52 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.GetHashCode a6bcbb73b51bb37c0ad5164ba3a547e7fce1a25b757ae8adecb4de067f2b4005 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Buffer ffeeda191701f4bdfb3f742478d582d1c5e0a5c1f727ab37dd95bc0804138090 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Offset 41704045942399f9b43882606e3829937161da4d1d1214d98701d9024c586f46 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Size 9b21202fdfba907cedd16ce7a9fcc5f81b3a5fac1bf1c42902c1370f62452e2a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-13e46fbcd2031eb5"></a>[`public Inno.Native.Sdl3.SDLGPUBufferRegion* Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8545) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-432f53a4eb539e97"></a>[`public Inno.Native.Sdl3.SDLGPUBufferRegionPtr.SDLGPUBufferRegionPtr(Inno.Native.Sdl3.SDLGPUBufferRegion* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8540) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-25f088310e5cdca0"></a>[`public bool Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Equals(Inno.Native.Sdl3.SDLGPUBufferRegionPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8555) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-dd43111a5556f411"></a>[`public override bool Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8556) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-9ca4b01888a38714"></a>[`public override int Inno.Native.Sdl3.SDLGPUBufferRegionPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8557) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-dfcfd5b26101df3e"></a>[`public static bool Inno.Native.Sdl3.SDLGPUBufferRegionPtr.operator ==(Inno.Native.Sdl3.SDLGPUBufferRegionPtr left, Inno.Native.Sdl3.SDLGPUBufferRegionPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8553) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-253947f6eadd11c0"></a>[`public static Inno.Native.Sdl3.SDLGPUBufferRegionPtr.implicit operator Inno.Native.Sdl3.SDLGPUBufferRegionPtr(Inno.Native.Sdl3.SDLGPUBufferRegion* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8551) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-985f24c212ffd795"></a>[`public static Inno.Native.Sdl3.SDLGPUBufferRegionPtr.implicit operator Inno.Native.Sdl3.SDLGPUBufferRegion*(Inno.Native.Sdl3.SDLGPUBufferRegionPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8552) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-dc11199eb3598c1e"></a>[`public static bool Inno.Native.Sdl3.SDLGPUBufferRegionPtr.operator !=(Inno.Native.Sdl3.SDLGPUBufferRegionPtr left, Inno.Native.Sdl3.SDLGPUBufferRegionPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8554) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8ee1a006d493e0fe"></a>[`public ref Inno.Native.Sdl3.SDLGPUBuffer Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Buffer { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8559) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-626f2d5a4244de7f"></a>[`public bool Inno.Native.Sdl3.SDLGPUBufferRegionPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8546) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cc140c89cd1b76b5"></a>[`public Inno.Native.Sdl3.SDLGPUBufferRegion Inno.Native.Sdl3.SDLGPUBufferRegionPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8549) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8da551a68e081cbf"></a>[`public static Inno.Native.Sdl3.SDLGPUBufferRegionPtr Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8547) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5ca2b9773b8b7337"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Offset { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8560) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-db04b34e81f7fba7"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUBufferRegionPtr.Size { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8561) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-74a04081925ab302"></a>[`public struct Inno.Native.Sdl3.SDLGPUBufferRegionPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUBufferRegionPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8538) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUColorComponentFlags</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUColorComponentFlags 49fefb7743ce4c1401b204e1d2dbdbb93c07c7a322f5ad526db8e8b541d898b6 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorComponentFlags.R 8d69ebff248089c0e7218c0b1e3d5c21b8674259ac7e7cf02ad7647a50d7b519 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorComponentFlags.G 23feb06efc235a0c6d85a4f422660c85b32eb67b2ab21f51433e6f68d9efd712 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorComponentFlags.B 4206c4d5128ec1b987d2ff3b383530b15414a4eaa6c3ce467477eb5d9a454fd3 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorComponentFlags.A c47588bf6e03e77215c12792a8b3f562f71b069c136f50e07a5c683e1cc610d7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-46957d2c145f1754"></a>[`Inno.Native.Sdl3.SDLGPUColorComponentFlags.A = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6527) | 当前常量或枚举值：`8`。 | — |
| <a id="api-09fc2c00c150229c"></a>[`Inno.Native.Sdl3.SDLGPUColorComponentFlags.B = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6526) | 当前常量或枚举值：`4`。 | — |
| <a id="api-5f3a92b363979a49"></a>[`Inno.Native.Sdl3.SDLGPUColorComponentFlags.G = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6525) | 当前常量或枚举值：`2`。 | — |
| <a id="api-7862570e94d05022"></a>[`Inno.Native.Sdl3.SDLGPUColorComponentFlags.R = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6524) | 当前常量或枚举值：`1`。 | — |
| <a id="api-c4b588a4d5dadca4"></a>[`public enum Inno.Native.Sdl3.SDLGPUColorComponentFlags : byte`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L6522) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUColorTargetBlendState</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUColorTargetBlendState 4fb4bcf2cb49888a980c254603bb933165c7e74b5b26f8312e714c9c45af43bd -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.SrcColorBlendfactor 00af5fcf8ac23417689bbccd20aab4a75412e2c29133d22b3abc891ca988fa1f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.DstColorBlendfactor 7183c8f4ae3d2c4b2348aacf5250e8a1bd4a9e24c0f768cb4cf3ab36b8af9f63 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.ColorBlendOp 6e63020ab65c33253c187b50ca92ae8959c64245548ad8df6590bd5a628b192b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.SrcAlphaBlendfactor b1b78b3a0c5b7c921e0626d8371b484fcefef4bf2612c40732453a9445617546 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.DstAlphaBlendfactor 68d5c604adc54f07a21297e5fa5c74377eb67007563ec5db0c640ed90f0faccd -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.AlphaBlendOp 0ad10b0500b57b7cb8ca3de1349ca762087ccfa90a4793bbb9e93233ce90804c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.ColorWriteMask 83d359d4d117a21562208067abcbe27a9d0fcf5d18fd94460d00bf53c5a17d02 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.EnableBlend ace5a8bc84ca089404ce0e1abd8273552463efc2285cbd2ee35f103a3da80e15 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.EnableColorWriteMask 149914a59c5055c59f98f568cad1a694a292fca56699646f9009c1b432260fa8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.Padding1 09cbafc91909c3a4f8e11604365f5587beda62a82bde3ec6bc2d675da663982c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.Padding2 9756e1d4374e37da84f04e330fa70f2a89010bdfcc943afb5a3c123d79403d7b -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetBlendState.#ctor(Inno.Native.Sdl3.SDLGPUBlendFactor,Inno.Native.Sdl3.SDLGPUBlendFactor,Inno.Native.Sdl3.SDLGPUBlendOp,Inno.Native.Sdl3.SDLGPUBlendFactor,Inno.Native.Sdl3.SDLGPUBlendFactor,Inno.Native.Sdl3.SDLGPUBlendOp,Inno.Native.Sdl3.SDLGPUColorComponentFlags,System.Byte,System.Byte,System.Byte,System.Byte) c272bca61803e4d84547e9981586dda54d31a280af10638a7e0d5437a54d8ca9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f4bb44dc79bee9fc"></a>[`public Inno.Native.Sdl3.SDLGPUBlendOp Inno.Native.Sdl3.SDLGPUColorTargetBlendState.AlphaBlendOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5098) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e38c918e048ae0a7"></a>[`public Inno.Native.Sdl3.SDLGPUBlendOp Inno.Native.Sdl3.SDLGPUColorTargetBlendState.ColorBlendOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5095) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e8aafa1f734cd4a4"></a>[`public Inno.Native.Sdl3.SDLGPUColorComponentFlags Inno.Native.Sdl3.SDLGPUColorTargetBlendState.ColorWriteMask`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5099) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-31db2d9fa31a77bf"></a>[`public Inno.Native.Sdl3.SDLGPUBlendFactor Inno.Native.Sdl3.SDLGPUColorTargetBlendState.DstAlphaBlendfactor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5097) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b086d3313e5ce169"></a>[`public Inno.Native.Sdl3.SDLGPUBlendFactor Inno.Native.Sdl3.SDLGPUColorTargetBlendState.DstColorBlendfactor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5094) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c19dc688bab3c48"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetBlendState.EnableBlend`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5100) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-734b966b0b9d6026"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetBlendState.EnableColorWriteMask`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5101) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-313e0adddc9889ea"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetBlendState.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5102) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-df34bc00c7382348"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetBlendState.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5103) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-195330b5a4ff2f4a"></a>[`public Inno.Native.Sdl3.SDLGPUBlendFactor Inno.Native.Sdl3.SDLGPUColorTargetBlendState.SrcAlphaBlendfactor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5096) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4ba4fe691436b881"></a>[`public Inno.Native.Sdl3.SDLGPUBlendFactor Inno.Native.Sdl3.SDLGPUColorTargetBlendState.SrcColorBlendfactor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5093) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e9a290dbea1cebf3"></a>[`public Inno.Native.Sdl3.SDLGPUColorTargetBlendState.SDLGPUColorTargetBlendState(Inno.Native.Sdl3.SDLGPUBlendFactor src_color_blendfactor = Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorInvalid, Inno.Native.Sdl3.SDLGPUBlendFactor dst_color_blendfactor = Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorInvalid, Inno.Native.Sdl3.SDLGPUBlendOp color_blend_op = Inno.Native.Sdl3.SDLGPUBlendOp.BlendopInvalid, Inno.Native.Sdl3.SDLGPUBlendFactor src_alpha_blendfactor = Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorInvalid, Inno.Native.Sdl3.SDLGPUBlendFactor dst_alpha_blendfactor = Inno.Native.Sdl3.SDLGPUBlendFactor.BlendfactorInvalid, Inno.Native.Sdl3.SDLGPUBlendOp alpha_blend_op = Inno.Native.Sdl3.SDLGPUBlendOp.BlendopInvalid, Inno.Native.Sdl3.SDLGPUColorComponentFlags color_write_mask = (Inno.Native.Sdl3.SDLGPUColorComponentFlags)0, byte enable_blend = 0, byte enable_color_write_mask = 0, byte padding1 = 0, byte padding2 = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5104) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-017c58b5b650a4f3"></a>[`public struct Inno.Native.Sdl3.SDLGPUColorTargetBlendState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5091) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUColorTargetDescription</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUColorTargetDescription e01ea388ae92a4a4fa4a6005af540cf90661bee1edf09eb248529c55e2104c55 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetDescription.Format 398ccd4bad81936fd30507c039348a337b26eeec298424d8f9102c903ffbaaa0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetDescription.BlendState faecfa66e70f725b4adc82dc83ff404b39300912b08c1923ba2911d64b3e1204 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetDescription.#ctor(Inno.Native.Sdl3.SDLGPUTextureFormat,Inno.Native.Sdl3.SDLGPUColorTargetBlendState) 9ec585ff7432cdccc542756b323cf6777ae1c42e9f65270b3bef673a7f457915 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-259b9b1ad5bfbee4"></a>[`public Inno.Native.Sdl3.SDLGPUColorTargetBlendState Inno.Native.Sdl3.SDLGPUColorTargetDescription.BlendState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5308) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d00dac8038b57cd7"></a>[`public Inno.Native.Sdl3.SDLGPUTextureFormat Inno.Native.Sdl3.SDLGPUColorTargetDescription.Format`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5307) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3484c9e0aeb5f65a"></a>[`public Inno.Native.Sdl3.SDLGPUColorTargetDescription.SDLGPUColorTargetDescription(Inno.Native.Sdl3.SDLGPUTextureFormat format = Inno.Native.Sdl3.SDLGPUTextureFormat.TextureformatInvalid, Inno.Native.Sdl3.SDLGPUColorTargetBlendState blend_state = default(Inno.Native.Sdl3.SDLGPUColorTargetBlendState))`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5309) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-834e65944c8dff70"></a>[`public struct Inno.Native.Sdl3.SDLGPUColorTargetDescription`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5305) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr 4f71a56f28b222853c59fb65c05c435e074d2207fc484ca5976023eb303f9c7d -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.#ctor(Inno.Native.Sdl3.SDLGPUColorTargetDescription*) 486e8246b10593c0560895adad6ae7b43facd9d2ef47771c7a785053ae6a6ac3 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.Handle 15573dd4024a815b8345b42d1853cc40455d913138411b92061ea1a7a4c04ec2 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.IsNull 0a976aa266cc540453d2cd311df31408050fda1b2d852835274b05938bdbb438 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.Null a0b156ca28ed4f6c0ca253f1a2432429a1573bbbbf970ce547a5fdae9ec700b7 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.Item(System.Int32) 172b3279a8acfa4ed2888cb3801f9a734ad34d0ccba810a8bc5aca2c1ddd6ab1 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUColorTargetDescription*)~Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr e23449f965caf15edbdf372c45b94eb902f2419cf521fdcedf215cbf183bd63c -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr)~Inno.Native.Sdl3.SDLGPUColorTargetDescription* 9f9f0599c27575924428c6993a660d426b1382bd31939c085c6fb3f52de7469e -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.op_Equality(Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr,Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr) 5d2fe69ec513eeb16e42180b808aa251e9a7e61197e67d44ea2bf0dffddc448f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr,Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr) e74ffe2fdbe7b3c577d40bee7fef59d061db1e9006509cb460838b95b109c268 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.Equals(Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr) 5e70587fff19618e4103815f1ff903d6286471457fd40fe682726bf4ec8c2cb1 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.Equals(System.Object) b5a5e91c03110e15e956f25cbf30a83145fad0f1dc0a27a8a5c8b474be423ca1 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.GetHashCode 0bde8322413cf3b4512fe328403222f8ce0028c46928a596c5418f93e859d0ca -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.Format 8dd5e85c3accb8b23165f2c6a1cc22e1971e4df73dd8b30749b58fa8e643efba -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.BlendState ace5b33d05ded83c4ede28b012075ef7e3fd2f9e7f152ea53946ec1430540f8f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-aef2bed083a0a6e8"></a>[`public Inno.Native.Sdl3.SDLGPUColorTargetDescription* Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8572) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-625361e7afd3e5ab"></a>[`public Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.SDLGPUColorTargetDescriptionPtr(Inno.Native.Sdl3.SDLGPUColorTargetDescription* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8567) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4d7de14e905da9de"></a>[`public bool Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.Equals(Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8582) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-ddc640f5e63dd024"></a>[`public override bool Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8583) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-178dd3ee2cdb4a03"></a>[`public override int Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8584) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-4568bca2af3ddf9c"></a>[`public static bool Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.operator ==(Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr left, Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8580) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fb3decef0a01f427"></a>[`public static Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.implicit operator Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr(Inno.Native.Sdl3.SDLGPUColorTargetDescription* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8578) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0eed3e459b63cf90"></a>[`public static Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.implicit operator Inno.Native.Sdl3.SDLGPUColorTargetDescription*(Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8579) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-14b17f1ead32d749"></a>[`public static bool Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.operator !=(Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr left, Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8581) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d3c714098c66e8a7"></a>[`public ref Inno.Native.Sdl3.SDLGPUColorTargetBlendState Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.BlendState { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8587) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-28ca5ee39eaa09ea"></a>[`public ref Inno.Native.Sdl3.SDLGPUTextureFormat Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.Format { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8586) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2c16901034c8235f"></a>[`public bool Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8573) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ab4a9c502b3cb2bf"></a>[`public Inno.Native.Sdl3.SDLGPUColorTargetDescription Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8576) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f5984858eccc07ba"></a>[`public static Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8574) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ca13efdbc0f11a50"></a>[`public struct Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUColorTargetDescriptionPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8565) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUColorTargetInfo</code> · 15 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUColorTargetInfo 73bd17af9e0db7611f5aed3cdf6d62da0ea23699ba408363c846d633a57c0c20 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.Texture 6c6db54bf40fa6945830253f27ffc4bcc4ad3e8068251b90693e2dcfab5383ee -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.MipLevel eb14f067621410e718a0f1dd201e206a6d75d0d2969db3a855c7821b9aca10af -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.LayerOrDepthPlane b5bf04832ae927e80eac764f83bc292236a904e9d133e4126c9e5ec5c7858aed -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.ClearColor 7b7c783c8927e3b7b46ba01e44f7a53f966f3583676654ac3db347f3e6c8d4ec -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.LoadOp bb98dba03de7d1a8f81f52f5298342982fc5eedc3aff4c09f847f40fe152752e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.StoreOp 4d2d4853977feea1d2cc6184777f5b4689500f555fb39a877b8814360fb4a106 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.ResolveTexture 4a20b7391a077683bcf145d3f0d9a4cd01a21ca5ec3073688766629e5e8dd45b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.ResolveMipLevel b1b3014f2c9ffa313a33c588beb8c4e161402c4849d5a6001bc143f5c8c89831 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.ResolveLayer a378d2083f8750ae0fd7c81f6476afd432c7871584c40721b98f296232a3312f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.Cycle fa1aa6546e242f035a1cddd0499e3abdbe4252bfb6de3682a79e305375c5c2f4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.CycleResolveTexture 651777bce3c82969cae1db8eeecc8e7f3984af60436e2ba4af21af0decd2f839 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.Padding1 30aff1a07332e7b408d94efad926e3aceecd2c0ecd9b4e9e8691b6a6f57df3f0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfo.Padding2 041ffd230d645070cf812cd8acef663158640faec3607cd9c6a83ccfc831d935 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetInfo.#ctor(Inno.Native.Sdl3.SDLGPUTexture,System.UInt32,System.UInt32,Inno.Native.Sdl3.SDLFColor,Inno.Native.Sdl3.SDLGPULoadOp,Inno.Native.Sdl3.SDLGPUStoreOp,Inno.Native.Sdl3.SDLGPUTexture,System.UInt32,System.UInt32,System.Byte,System.Byte,System.Byte,System.Byte) e55c34d3c41db8f4386ee153688744169ee9950821647f1404bcd112c62196d9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-12c7a115050ae934"></a>[`public Inno.Native.Sdl3.SDLFColor Inno.Native.Sdl3.SDLGPUColorTargetInfo.ClearColor`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5418) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-38b74adb16b633f9"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetInfo.Cycle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5424) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-61fb8395a36ffa53"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetInfo.CycleResolveTexture`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5425) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e76fc4279d73f233"></a>[`public uint Inno.Native.Sdl3.SDLGPUColorTargetInfo.LayerOrDepthPlane`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5417) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-19afd85354d75bb4"></a>[`public Inno.Native.Sdl3.SDLGPULoadOp Inno.Native.Sdl3.SDLGPUColorTargetInfo.LoadOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5419) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d3b3215c23bbe81c"></a>[`public uint Inno.Native.Sdl3.SDLGPUColorTargetInfo.MipLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5416) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11513ef3a254110a"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetInfo.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5426) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1c6a67545fd5e8cf"></a>[`public byte Inno.Native.Sdl3.SDLGPUColorTargetInfo.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5427) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2cac32bae676cc34"></a>[`public uint Inno.Native.Sdl3.SDLGPUColorTargetInfo.ResolveLayer`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5423) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5c711ddbe24f7e8c"></a>[`public uint Inno.Native.Sdl3.SDLGPUColorTargetInfo.ResolveMipLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5422) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c28a390efe102a39"></a>[`public Inno.Native.Sdl3.SDLGPUTexture Inno.Native.Sdl3.SDLGPUColorTargetInfo.ResolveTexture`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5421) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-49a890b5cd4d2dd4"></a>[`public Inno.Native.Sdl3.SDLGPUStoreOp Inno.Native.Sdl3.SDLGPUColorTargetInfo.StoreOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5420) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-11164400bcfba14b"></a>[`public Inno.Native.Sdl3.SDLGPUTexture Inno.Native.Sdl3.SDLGPUColorTargetInfo.Texture`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5415) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6f77e816b78199ce"></a>[`public Inno.Native.Sdl3.SDLGPUColorTargetInfo.SDLGPUColorTargetInfo(Inno.Native.Sdl3.SDLGPUTexture texture = default(Inno.Native.Sdl3.SDLGPUTexture), uint mip_level = 0, uint layer_or_depth_plane = 0, Inno.Native.Sdl3.SDLFColor clear_color = default(Inno.Native.Sdl3.SDLFColor), Inno.Native.Sdl3.SDLGPULoadOp load_op = Inno.Native.Sdl3.SDLGPULoadOp.LoadopLoad, Inno.Native.Sdl3.SDLGPUStoreOp store_op = Inno.Native.Sdl3.SDLGPUStoreOp.StoreopStore, Inno.Native.Sdl3.SDLGPUTexture resolve_texture = default(Inno.Native.Sdl3.SDLGPUTexture), uint resolve_mip_level = 0, uint resolve_layer = 0, byte cycle = 0, byte cycle_resolve_texture = 0, byte padding1 = 0, byte padding2 = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5428) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9edc9401d7c16152"></a>[`public struct Inno.Native.Sdl3.SDLGPUColorTargetInfo`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5413) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr</code> · 26 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr 3ce7588a07170ddc2015c8e440ea62fc76b86f801d5852da2ce4a68c95afb213 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.#ctor(Inno.Native.Sdl3.SDLGPUColorTargetInfo*) 8a97b2f09318a5f81f510a4c77565182c083e3d9395a4e97820c12876ce12a6c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Handle 2f404e81ec90c14e446f78d151e1a1b5f241cf1637414c54ee20631b8cb9dbff -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.IsNull a6bc1be1685e2d70e9330f94c1bd3c21ee9c7b19c01afc8f41df9fba37db1a8b -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Null c312bb51fede5190801bfadf4e8b88d6bdcdc494b3464fcfa2a80146f7111434 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Item(System.Int32) 0ccae5aacdeb8b3febdce372de3c83a0bfa3ab4fd6fcf97f734b2e9cfc3fb742 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUColorTargetInfo*)~Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr 2797848b4e9af9c475e81b5e083df46258e023f0bceef767b085e3edaf33bc7c -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr)~Inno.Native.Sdl3.SDLGPUColorTargetInfo* 9b8dd529765fbb71271da4f0f9f73e62b6d32d798b7ace4e3f3a541f0dca5621 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.op_Equality(Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr,Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr) 29a461ef039619333127da3853d7b7eae172dd9012de55aafee6453d353b89fb -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr,Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr) e190fb22f4051748fe0451306d26eb0eed128240af95ad884f043aa49173c0b7 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr) e031b002d66627d4668a632b64a34dfe22d158ba8ef3859f21ecc517dff0466f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Equals(System.Object) 966042638b75c9e44e3ae8fcfbeca1e94984feaf73c35685adc246b7b56ffb2d -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.GetHashCode 3be1a57d6468058488147d4bc4e77241fe58c4e97729b344f4ffc5f7320d94d4 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Texture 03fce3dcc4b872fb5bcf7a7b06786d9eff2dc8dc386c683ad2c3c8b6c0332f81 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.MipLevel d4fcbe3dc37ab82145afd0735692a4d641ad740d7dc3a2971e4aa564e587ed0c -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.LayerOrDepthPlane cfc2772b94a08a90bdd72031d5f2bc69939e4c9b6bd91b7f5a7ef586fa69ad7b -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.ClearColor 3ed630072629c174955b22a472545ec77f69dbf332410174f30902fc4bce5574 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.LoadOp d048e2e60c1ef0fc4ff6ceb7221217a67b3555da116d03c6b5969f4b5633a764 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.StoreOp bac1985a5f699ef28bd74dac0f8d370e2ea12283e257aac1efda72a7ba204692 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.ResolveTexture 5fc2ad5d05d459438f79d04a427d4b852a8333dd8b3a0dc519f6a2d49b1c21ad -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.ResolveMipLevel 8aac7fd5164d1a1b2d85c442b0afe7b3b935974ab185c9b5cea51b3d2c9b7ff4 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.ResolveLayer 082d9e92e9b8ed903ab9ccdbe1444dcb6cd2c727f5147c068ebe16e37891e319 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Cycle 2435c4c8840ea7972f52a4198e92b785aa55755f6161a0053586b4ec6573c0e3 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.CycleResolveTexture 77805da5ddf2dc147fc63dc0a4266ce8525b9c3b565fd7671e5f867c82e792dc -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Padding1 251c0ee87b60691576be59b200f181344c363eebb547de51c535c4c35fea93d6 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Padding2 bb0853bfea6d91866d740537cb5ad1a207970f87c746b9eaf6ae6ba0e9b01494 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-21f11c3a9ac1c307"></a>[`public Inno.Native.Sdl3.SDLGPUColorTargetInfo* Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8598) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-01f5be3cdd50b11c"></a>[`public Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.SDLGPUColorTargetInfoPtr(Inno.Native.Sdl3.SDLGPUColorTargetInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8593) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2b63e52fbb55238a"></a>[`public bool Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8608) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-600d57da669defe2"></a>[`public override bool Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8609) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-1883b120c0b99632"></a>[`public override int Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8610) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-a3fe087311080bcb"></a>[`public static bool Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.operator ==(Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr left, Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8606) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9dd76b6427c4502c"></a>[`public static Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr(Inno.Native.Sdl3.SDLGPUColorTargetInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8604) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fa66b3ec769e2e83"></a>[`public static Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUColorTargetInfo*(Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8605) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d138335ddcd4fa86"></a>[`public static bool Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.operator !=(Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr left, Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8607) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-47c65c8ce238c329"></a>[`public ref Inno.Native.Sdl3.SDLFColor Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.ClearColor { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8615) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c7dd3e65be60debe"></a>[`public bool Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Cycle { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8621) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1f1f3469c19c403b"></a>[`public bool Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.CycleResolveTexture { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8622) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1db00af995ec18a2"></a>[`public bool Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8599) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ecceadc0e49ef5fe"></a>[`public Inno.Native.Sdl3.SDLGPUColorTargetInfo Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8602) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-58c6354450640ee2"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.LayerOrDepthPlane { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8614) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b66da7278c1b57c3"></a>[`public ref Inno.Native.Sdl3.SDLGPULoadOp Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.LoadOp { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8616) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-154a3b6cfa28e576"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.MipLevel { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8613) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-318bf7f00b957919"></a>[`public static Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8600) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2d5fc879aa9e575e"></a>[`public ref byte Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Padding1 { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8623) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-285c03cc2ce8fd68"></a>[`public ref byte Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Padding2 { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8624) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6ceb50fcc50f76b9"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.ResolveLayer { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8620) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c5e1ea9f53fc1fcd"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.ResolveMipLevel { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8619) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3000aecb0cb4ff49"></a>[`public ref Inno.Native.Sdl3.SDLGPUTexture Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.ResolveTexture { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8618) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c47329bddc2471bf"></a>[`public ref Inno.Native.Sdl3.SDLGPUStoreOp Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.StoreOp { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8617) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5fb19d87545290c9"></a>[`public ref Inno.Native.Sdl3.SDLGPUTexture Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr.Texture { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8612) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-00c416e40dd16b54"></a>[`public struct Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUColorTargetInfoPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8591) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUCommandBuffer</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUCommandBuffer 83d6881634992587107d9129e9b386f1481a2c060d8c2b76613e10c8a24c7971 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCommandBuffer.#ctor(System.IntPtr) e9050bc47b51efc95c5a3b01e6c4ce7ac6196b265c0642860826e1aaa745758e -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUCommandBuffer.Handle 28ad39139a8a7208cb4975e659d49474edb1198d845fbcabd2dd6a7c5ccbb401 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUCommandBuffer.IsNull 68429c87b19967cff9de9ebae8b3e8a680586acde5c26c1406d01d29dd982aee -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUCommandBuffer.Null 00a3232e17f5824f8e1c1ccf716b1433df635386e691884e13ed181638e912b2 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCommandBuffer.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLGPUCommandBuffer 90ccbc3ac87cd7e5617cd11ab67abf1eda32e401ac9ca0911da0d82f26c80108 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCommandBuffer.op_Equality(Inno.Native.Sdl3.SDLGPUCommandBuffer,Inno.Native.Sdl3.SDLGPUCommandBuffer) 03fc6fb0c89f057a96201f3d5e7414eab91393f0915a311ad7940cb051d2e9df -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCommandBuffer.op_Inequality(Inno.Native.Sdl3.SDLGPUCommandBuffer,Inno.Native.Sdl3.SDLGPUCommandBuffer) 8de1b6630ae1b943076fce9f316537fd5b4a80b09ba9d8fd772efe86d284048c -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCommandBuffer.Equals(Inno.Native.Sdl3.SDLGPUCommandBuffer) 4ff7cf90cb072008b505dae7de635f2fccf50d7491bfe6efa9f6b3d57e0f99a9 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCommandBuffer.Equals(System.Object) c2ee95310cc19d9b8a260120274bdec6fac55d084bd456764c2e775dd3e56c4f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCommandBuffer.GetHashCode 6f53547e4053c1206de93d6e249a8722485c193d239829987aa6147e7cc087a0 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6b4a92405543e885"></a>[`public Inno.Native.Sdl3.SDLGPUCommandBuffer.SDLGPUCommandBuffer(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4631) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-e486c21c1bf43496"></a>[`public bool Inno.Native.Sdl3.SDLGPUCommandBuffer.Equals(Inno.Native.Sdl3.SDLGPUCommandBuffer other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4643) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-d15a01a6f3cbe832"></a>[`public override bool Inno.Native.Sdl3.SDLGPUCommandBuffer.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4644) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-56c4d9c4438433c2"></a>[`public override int Inno.Native.Sdl3.SDLGPUCommandBuffer.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4645) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-a0c4cfe11f031321"></a>[`public static bool Inno.Native.Sdl3.SDLGPUCommandBuffer.operator ==(Inno.Native.Sdl3.SDLGPUCommandBuffer left, Inno.Native.Sdl3.SDLGPUCommandBuffer right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4641) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-f163430cd09fdde8"></a>[`public static Inno.Native.Sdl3.SDLGPUCommandBuffer.implicit operator Inno.Native.Sdl3.SDLGPUCommandBuffer(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4640) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e9deb33d16c46e5c"></a>[`public static bool Inno.Native.Sdl3.SDLGPUCommandBuffer.operator !=(Inno.Native.Sdl3.SDLGPUCommandBuffer left, Inno.Native.Sdl3.SDLGPUCommandBuffer right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4642) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-cfcd4b6c8d28ff6d"></a>[`public nint Inno.Native.Sdl3.SDLGPUCommandBuffer.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4636) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dbce156d0dd7f301"></a>[`public bool Inno.Native.Sdl3.SDLGPUCommandBuffer.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4637) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c96515b6c2cd0ab3"></a>[`public static Inno.Native.Sdl3.SDLGPUCommandBuffer Inno.Native.Sdl3.SDLGPUCommandBuffer.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4638) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-779fa5a080373e29"></a>[`public readonly struct Inno.Native.Sdl3.SDLGPUCommandBuffer : System.IEquatable<Inno.Native.Sdl3.SDLGPUCommandBuffer>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4629) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUCompareOp</code> · 10 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUCompareOp 404ca5f60368f00629e9f6985fd7383f1c7121138a0708f7b1a6c4cac896e37f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.CompareopInvalid 628b3760bb9cf77618af6e456f4ff607f827891197d3f4a27f1c9d28fdc9cced -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.CompareopNever fd1e6085a55ce22828194d3cf033c2506f9d2033ee275649176b22a666447822 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.CompareopLess df3991eef086507621a90175bb4a06d5a6c4b79d8ddd19cb7839b08f6b06db5a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.CompareopEqual 9dae2d20423a19ac59e4eb653c26d63676981c94cda578e2dc870f7916b72661 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.CompareopLessOrEqual 5835c8002681b1bf35f17563d4c1879bf473c8ac5c3d8fbba8c073a632f45bc2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.CompareopGreater 9ff05204facef3e6e2ade26a42d7196781845293fe19a66d6c2cd782885ed976 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.CompareopNotEqual b9d7da2e44ae2b45d295bdb0a2a25e1fdaadd84e2db780226cf0968eedcb8168 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.CompareopGreaterOrEqual 705522d8f56a83f27cbb7656b85e828f789bc943e83b897270b265724f3b0773 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCompareOp.CompareopAlways 9af1e562dbb47551942d36efc477770819587c3a9a04d9d482a5b5efacebf9e7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-4a1d44968731a7aa"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.CompareopAlways = 8`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1638) | 当前常量或枚举值：`8`。 | — |
| <a id="api-d3e2a09d99e2b832"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.CompareopEqual = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1633) | 当前常量或枚举值：`3`。 | — |
| <a id="api-d50cf7b9bdb20a9a"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.CompareopGreater = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1635) | 当前常量或枚举值：`5`。 | — |
| <a id="api-b20043d5e17cb358"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.CompareopGreaterOrEqual = 7`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1637) | 当前常量或枚举值：`7`。 | — |
| <a id="api-868a7162da1ff881"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.CompareopInvalid = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1630) | 当前常量或枚举值：`0`。 | — |
| <a id="api-ad191582403a977f"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.CompareopLess = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1632) | 当前常量或枚举值：`2`。 | — |
| <a id="api-7b709ba8850440de"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.CompareopLessOrEqual = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1634) | 当前常量或枚举值：`4`。 | — |
| <a id="api-8a6efa5fee3fce49"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.CompareopNever = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1631) | 当前常量或枚举值：`1`。 | — |
| <a id="api-0d1955ea418b6503"></a>[`Inno.Native.Sdl3.SDLGPUCompareOp.CompareopNotEqual = 6`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1636) | 当前常量或枚举值：`6`。 | — |
| <a id="api-db83e8d41c0766c6"></a>[`public enum Inno.Native.Sdl3.SDLGPUCompareOp : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1628) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUComputePass</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUComputePass cfd8fb8759357624e897b3f3de56a6af50d892e8a22c3f4d530626f715afe64f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePass.#ctor(System.IntPtr) ebc163a10c2a6a412c54b670a58941a12e0982529e7a30141691e2aa9d76cb5c -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePass.Handle 5cffed507c00b4bb80fb4e255d9917ba5c333a8c55f1187e758aa0554124b5fc -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePass.IsNull f8b2b0ae4c5a7a3a128e1d78268597ed0dc2cb14272b185f498e717ad3c136fb -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePass.Null b57cc440ee4388a0bc84cd7e674ef28ff94a421d269ebb5399d4a9cf37f4511d -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePass.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLGPUComputePass 861898512f008fa2494ffdda4419b3e8e4554c71a67470df28eff5b68ca8f48f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePass.op_Equality(Inno.Native.Sdl3.SDLGPUComputePass,Inno.Native.Sdl3.SDLGPUComputePass) 6a0d783cded2881af21476fd003a6bc7228d92104977f2bf0b1700efb261d543 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePass.op_Inequality(Inno.Native.Sdl3.SDLGPUComputePass,Inno.Native.Sdl3.SDLGPUComputePass) 3b8a6795d0dd44b4d45a83d88d40232b515f9470b0996284d0c5dcd492b4a309 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePass.Equals(Inno.Native.Sdl3.SDLGPUComputePass) eba5d2d0f56799e74b0a560f9626c35ce07cbd9bf06c455cf190ddec23993387 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePass.Equals(System.Object) fecf2cbbf9b4ddebebe912d2901d5f24505bab3b217f3295d39890ea4ad38b4d -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePass.GetHashCode 5179a6e695f400494a103e9e5de7451e5af4d8c354a6f60925ee3a9fcd39ce79 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-cc28403903ad7e5d"></a>[`public Inno.Native.Sdl3.SDLGPUComputePass.SDLGPUComputePass(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4679) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-333a6c218c213720"></a>[`public bool Inno.Native.Sdl3.SDLGPUComputePass.Equals(Inno.Native.Sdl3.SDLGPUComputePass other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4691) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-124d4ffb849bf1d1"></a>[`public override bool Inno.Native.Sdl3.SDLGPUComputePass.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4692) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-80a8119fb67b865e"></a>[`public override int Inno.Native.Sdl3.SDLGPUComputePass.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4693) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-cc2e31fc60532d2d"></a>[`public static bool Inno.Native.Sdl3.SDLGPUComputePass.operator ==(Inno.Native.Sdl3.SDLGPUComputePass left, Inno.Native.Sdl3.SDLGPUComputePass right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4689) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ff2b98033ab736f5"></a>[`public static Inno.Native.Sdl3.SDLGPUComputePass.implicit operator Inno.Native.Sdl3.SDLGPUComputePass(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4688) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-0af2663d713d4b66"></a>[`public static bool Inno.Native.Sdl3.SDLGPUComputePass.operator !=(Inno.Native.Sdl3.SDLGPUComputePass left, Inno.Native.Sdl3.SDLGPUComputePass right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4690) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-56600cdf1c5f0346"></a>[`public nint Inno.Native.Sdl3.SDLGPUComputePass.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4684) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1a614cc3963e22eb"></a>[`public bool Inno.Native.Sdl3.SDLGPUComputePass.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4685) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9dfb71636690b35b"></a>[`public static Inno.Native.Sdl3.SDLGPUComputePass Inno.Native.Sdl3.SDLGPUComputePass.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4686) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-c457ccb367203f71"></a>[`public readonly struct Inno.Native.Sdl3.SDLGPUComputePass : System.IEquatable<Inno.Native.Sdl3.SDLGPUComputePass>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4677) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUComputePipeline</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUComputePipeline c9bc68364c23a7358fe53c231b3c3e1cf8776dd6f8918a5c9132fe38b2f3ee18 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipeline.#ctor(System.IntPtr) b9b09ad6cd2f611a5bf8162f3e1ba70f417c0374abd329cb6d5862e317b6a753 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipeline.Handle c3718801f8f66b36dbd86cf595b72a4003f50d06dce52d198b3c486e1035eda8 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipeline.IsNull 134f1466186d52b4dd065b03b0f91aa3d2456f078e2c6f2f473b1931470b5f25 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipeline.Null 0edb9939ce934e24cf36fbdba14b3bf8eb2bbbe1ce4629f584c4e25e9bb88c55 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipeline.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLGPUComputePipeline d7217bc867d17b3d264d42045be6f459a07d6b40cab0e02690ce12011bf4495c -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipeline.op_Equality(Inno.Native.Sdl3.SDLGPUComputePipeline,Inno.Native.Sdl3.SDLGPUComputePipeline) da78ed2da8c10835a4d93d3bdfb05d3995ab15fbacf43a7e1b7edad3b95b1195 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipeline.op_Inequality(Inno.Native.Sdl3.SDLGPUComputePipeline,Inno.Native.Sdl3.SDLGPUComputePipeline) 619d5ec23a2b4cc2d1598ab60b87cd824cbf13bd40378ef01fbf89615027b653 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipeline.Equals(Inno.Native.Sdl3.SDLGPUComputePipeline) 3b1caddddc0f4babc15b31e2e6f01f55f9bfa577cb90830174f21b1c0adcb21c -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipeline.Equals(System.Object) de42e4c26d631a64209cc143a34b6aff7f77a8401b43f5acb9dd8cb8503590ba -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipeline.GetHashCode 0ad5344be77968bd45e84fb3ab0ec76a3ad4c1c75b39e008522505b4911f3567 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-16dfaa3b3e43ab21"></a>[`public Inno.Native.Sdl3.SDLGPUComputePipeline.SDLGPUComputePipeline(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4583) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8581966cefc05af0"></a>[`public bool Inno.Native.Sdl3.SDLGPUComputePipeline.Equals(Inno.Native.Sdl3.SDLGPUComputePipeline other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4595) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-58fb758ab6a569cc"></a>[`public override bool Inno.Native.Sdl3.SDLGPUComputePipeline.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4596) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-64182a506f5bbe00"></a>[`public override int Inno.Native.Sdl3.SDLGPUComputePipeline.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4597) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-5fca6b258009b1cd"></a>[`public static bool Inno.Native.Sdl3.SDLGPUComputePipeline.operator ==(Inno.Native.Sdl3.SDLGPUComputePipeline left, Inno.Native.Sdl3.SDLGPUComputePipeline right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4593) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-05ffce2765b104bd"></a>[`public static Inno.Native.Sdl3.SDLGPUComputePipeline.implicit operator Inno.Native.Sdl3.SDLGPUComputePipeline(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4592) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-05cf27990656eba3"></a>[`public static bool Inno.Native.Sdl3.SDLGPUComputePipeline.operator !=(Inno.Native.Sdl3.SDLGPUComputePipeline left, Inno.Native.Sdl3.SDLGPUComputePipeline right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4594) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-e708a52404c0fc65"></a>[`public nint Inno.Native.Sdl3.SDLGPUComputePipeline.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4588) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f6c22765c551ae8a"></a>[`public bool Inno.Native.Sdl3.SDLGPUComputePipeline.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4589) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0530dee7b0a20419"></a>[`public static Inno.Native.Sdl3.SDLGPUComputePipeline Inno.Native.Sdl3.SDLGPUComputePipeline.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4590) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a1d7a15cfc085fce"></a>[`public readonly struct Inno.Native.Sdl3.SDLGPUComputePipeline : System.IEquatable<Inno.Native.Sdl3.SDLGPUComputePipeline>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4581) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo</code> · 16 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo df537a85ccdef2d53bc0d76b44924c670748f7fa8d029bf0c87332be960a510f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.CodeSize 6ca8dafdb50b53db464dbd9e0ac1600be7973e003dcc58b5ddc136fe4cba3cab -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Code fed0235a8744e436336afd2ec9d7fa1736312625b47276a4c2cc5913d937b833 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Entrypoint 969ac1ba28e626cc39a6bf1db3967a5649a1c1ee51a5d4b0a5602f1d675a62cd -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Format 4c2ad89b731dad1f2552dde59bc741e19a558f45f0c585d6b9cac1f9fc6499d8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumSamplers 2ff63fe62a427c08cb9459a55306fa80285a6a36061f87e9f5067b90589e3c88 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadonlyStorageTextures 3b7f254d4a9063d4d4d842390d2f57922d007c71037cea24e075b3561e32d370 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadonlyStorageBuffers cd8cb34b3fc1a43b66249c687ee037ea614b637e66cc621100cf2637870568fe -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadwriteStorageTextures 42e433c065d8b81311626ed93724c73c35e490c7ecedd7c7215be444f8229c93 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadwriteStorageBuffers 25de7297b66158050b2ab1bcee456013db274f6694b26c71fa6af05fccec8ff1 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumUniformBuffers 3abd5f480f1fc09b97423be4c5bb65c04256ce2a49afd18c14a57a49227ddbce -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.ThreadcountX c0f01b9054b48b4b31cf2b7d7425fb631c5844ba1965969d7d7712adee12fce0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.ThreadcountY 76344aac81a4f1edca122b73dd72c09d870bba924b65f8dd44887776f87da00e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.ThreadcountZ b80188c4e7f99791981208ebdb53b841ac801349a6d899e01349ef40bbbbcacc -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Props 859e07abc7bcac6b963b30dceabb72e01face61431cedd5cb190bff50801ee6b -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.#ctor(System.UIntPtr,System.Byte*,System.Byte*,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32,System.UInt32) a36127d2d662430d668a7e441ddcf0956edf2f0277670d0355cbeb22b4f335b7 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ae4f6280f5f55c91"></a>[`public byte* Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Code`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5377) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-95047c173a43c213"></a>[`public nuint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.CodeSize`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5376) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-b20ddd55e6aa2734"></a>[`public byte* Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Entrypoint`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5378) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e9d469202517fb5"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Format`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5379) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-672d71b5b6340c9d"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadonlyStorageBuffers`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5382) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5670b007f16c1836"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadonlyStorageTextures`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5381) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3ed72d94d5f410a5"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadwriteStorageBuffers`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5384) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dc4fd09c38067b45"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumReadwriteStorageTextures`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5383) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-fe37bb0e6089e0ec"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumSamplers`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5380) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e94e446e94c76734"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.NumUniformBuffers`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5385) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0e9bc91184212ef6"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.Props`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5389) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ff9944cf1fe5663c"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.ThreadcountX`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5386) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f75e9689e03c59a"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.ThreadcountY`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5387) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-dfc315c66bdd8747"></a>[`public uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.ThreadcountZ`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5388) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-7dc9534f9f346db9"></a>[`public Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo.SDLGPUComputePipelineCreateInfo(nuint code_size = 0, byte* code = null, byte* entrypoint = null, uint format = 0, uint num_samplers = 0, uint num_readonly_storage_textures = 0, uint num_readonly_storage_buffers = 0, uint num_readwrite_storage_textures = 0, uint num_readwrite_storage_buffers = 0, uint num_uniform_buffers = 0, uint threadcount_x = 0, uint threadcount_y = 0, uint threadcount_z = 0, uint props = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5390) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-373426a59883e9c0"></a>[`public struct Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5374) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr</code> · 27 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr 7ba1349a938874255864e3852f04d62913a4f5e7cb2043e1654a9834389b60cc -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.#ctor(Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo*) 526ca69d93a5b0e0367eb69a4218f3d55d1fd3dc789e7f8eca4bdfa4c96131cb -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Handle 5cbcac0ac433222491384e87447810beb8815c6c5d197a9af5d5b83615e43c26 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.IsNull 2e08b66feabd6ab3494e1c8a3b8f005aa73b943e9d295b1fbf721a99b2897a97 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Null 9952f27f89503e14aecc1f54e571e654e4b57929482260a055ba3048fd017c62 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Item(System.Int32) 760ba1ff792a3b881f8885177c9f5fdb749a79e433d04debc80f16cce26a7287 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo*)~Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr b9ff56f6da9c2225be777d72a2d270ad72c466e751ccc787ed3b85a0368526b2 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr)~Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo* 4b5ed843fdf2e8e268232894391fce66aeb07fb2c5fef12114f66854ae503b9e -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.op_Equality(Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr,Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr) 8c4aa4c557127192677eefb2adc6d6c644ac18e74ad93661817ea6e69c7876fc -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr,Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr) 21288ac8bd9533d9b0df2e506614b3dfdedae262131ad8b6e23bd6b0c5e9fafb -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr) c38bc5b21167f4d9de779253b112fcb37572bfaad0a218d02aaa4269f5aaa0ac -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Equals(System.Object) 4b5894e8b215e5234782344c87c84bb5ab7a5af5a16cfacf92e3a64832be0a74 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.GetHashCode 574aa913d7ae26176106f3d401889e03d793a47bb85dc3c33cec697de32f260d -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.CodeSize 14c1156a50457956065b0a4d7380b0bc2d46d401ba1b75502f929e51a9390a58 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Code 22bbda6a88ee5e67ea856690a509c6b6d8c829fe127ba42bcd9458ceb26d7c0f -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Entrypoint 17aa5ecd789d0033dcf5f5d6d26cce0e507ae19326d458b75a5827d4eb644e01 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Format fbe5652b834a3690498e9462ad283851b3f16df6852bdd8036c4741e0ccada8b -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.NumSamplers 4e15120c25afca6d8c88ad4d8824b6ae806b63e0788bdba023e62dcdfddcf553 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.NumReadonlyStorageTextures 632a21fe980be68bf7e28fbcb39d7fdb2dbb40060dce258046f3707b606cffee -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.NumReadonlyStorageBuffers 9d7e134302f716f92ee7ead5011fe93079fa8b9e743dd0d6f2056b858c98b4f9 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.NumReadwriteStorageTextures 6c3eb3e9d292d1f66dc65d01c68d228eacb8f8e124ce5f589a85df25cab9877b -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.NumReadwriteStorageBuffers 4154fba7c708deb394bce6d049e1a094bd5dcfb0e4524a7a34527fcc4ab98a89 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.NumUniformBuffers def504fae0f5d9194491df5da5f65f2185b47abe1ad488b332a1512689149b13 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.ThreadcountX 5066eb343c08c54c399b8496e816805084c8b45e3ef0cda13dec25a27ac56716 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.ThreadcountY 9ed9f7f8ebf9519ad43b6fab0c49667fe4d829af7a2b1e07dd5f5a472c705a7e -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.ThreadcountZ 2fa367e4faa11a4f8188b14e89813cff5401e0f8f2dafcfb4364362c93d3b65c -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Props 1071a3ce3b184678a146a0797500d014c2989ff9763a6af5667ca509dd3abe6f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-5ee82b42c72f09d7"></a>[`public Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo* Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8635) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-03fce263f84eecc9"></a>[`public Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.SDLGPUComputePipelineCreateInfoPtr(Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8630) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-aadc05831e3e478d"></a>[`public bool Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8645) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-13a5c71f23220cd7"></a>[`public override bool Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8646) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-fb7bc17f164f902c"></a>[`public override int Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8647) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-1bb0b667db556e62"></a>[`public static bool Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.operator ==(Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr left, Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8643) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-ebeaebbde4510b8c"></a>[`public static Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr(Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8641) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-8a2498f913180a53"></a>[`public static Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo*(Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8642) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fd6b2795c24c42a7"></a>[`public static bool Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.operator !=(Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr left, Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8644) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-418364c80c3a59cb"></a>[`public byte* Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Code { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8650) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-de908ed0c68c2f02"></a>[`public ref nuint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.CodeSize { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8649) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-0c5f488270aec5fc"></a>[`public byte* Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Entrypoint { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8651) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ec025a0c8f4bbe05"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Format { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8652) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7a9e745f4732643f"></a>[`public bool Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8636) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-21115891c625cb0f"></a>[`public Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfo Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8639) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3a8cfeab56c438d1"></a>[`public static Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8637) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-5cd2a9cf397b2a12"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.NumReadonlyStorageBuffers { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8655) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7b1e9436843a6eba"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.NumReadonlyStorageTextures { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8654) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-75fcdee56df87fa4"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.NumReadwriteStorageBuffers { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8657) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b1d44ce4a02f38ec"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.NumReadwriteStorageTextures { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8656) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ac28aaa52ed321e7"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.NumSamplers { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8653) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-314f9ae3814eed4b"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.NumUniformBuffers { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8658) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-3c84c07c3cacdac0"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.Props { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8662) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6bb016ed9f42bd5f"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.ThreadcountX { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8659) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-6e7dcc029b84165b"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.ThreadcountY { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8660) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1fadf7313fffe86c"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr.ThreadcountZ { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8661) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dc01c73dedb0dbb6"></a>[`public struct Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUComputePipelineCreateInfoPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8628) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUCopyPass</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUCopyPass 863e4020c9c129e716f1f8487fd847ea64cbf25e94dcbdfa216c8582c03bd93e -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCopyPass.#ctor(System.IntPtr) 0a9c7aa3f9148754be6201f4b3a71fd7b4c1333c7bb22ff86583ecbb9b3dd90f -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUCopyPass.Handle 54e4ec0f8eeb5fbc2f6004576397d11147e4d2c04b74413db0a56853465782cf -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUCopyPass.IsNull 53943e5b7df33d4fd529eba89a201cf765b87de3f2afa0e703bfe404fb21bd88 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUCopyPass.Null 48c41d0dab4091d005c475e5832972d39e2e7513578a4b8b23d7516e9818a4c4 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCopyPass.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLGPUCopyPass a6a78f204c6305544006d288d95433ec2ac2f08a7c8ea98bbdd25f902e670769 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCopyPass.op_Equality(Inno.Native.Sdl3.SDLGPUCopyPass,Inno.Native.Sdl3.SDLGPUCopyPass) 146934f62748be1bfaae3a2fa183569a7ec7b47d5739c45505938f949ceb8be4 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCopyPass.op_Inequality(Inno.Native.Sdl3.SDLGPUCopyPass,Inno.Native.Sdl3.SDLGPUCopyPass) 7d442ff41e80da8757a6e5502526a96d3a7c4fbbdf17f9a559f16c95721c483f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCopyPass.Equals(Inno.Native.Sdl3.SDLGPUCopyPass) 5430f4164e38d420997141a60dfa6a41ea1e20971cd59fef999e4c2dca6881b5 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCopyPass.Equals(System.Object) 4c4b480d15d0a71cc0a0cf8c4e5b517fc27b37f0d7d0fc4e3775a51297f2fd6a -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUCopyPass.GetHashCode e7f8c6f0a78a95d7a1776774f4b3fd5a5dc5fa1f752729bd19f4ffd48f31d939 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f49d6e42d421faf9"></a>[`public Inno.Native.Sdl3.SDLGPUCopyPass.SDLGPUCopyPass(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4703) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-ccda906a3e01fb24"></a>[`public bool Inno.Native.Sdl3.SDLGPUCopyPass.Equals(Inno.Native.Sdl3.SDLGPUCopyPass other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4715) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-ea77b7658cdde715"></a>[`public override bool Inno.Native.Sdl3.SDLGPUCopyPass.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4716) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-2fd97f6583fe5f1a"></a>[`public override int Inno.Native.Sdl3.SDLGPUCopyPass.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4717) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-8f9473c71447b60e"></a>[`public static bool Inno.Native.Sdl3.SDLGPUCopyPass.operator ==(Inno.Native.Sdl3.SDLGPUCopyPass left, Inno.Native.Sdl3.SDLGPUCopyPass right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4713) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-814f67694b6ff36e"></a>[`public static Inno.Native.Sdl3.SDLGPUCopyPass.implicit operator Inno.Native.Sdl3.SDLGPUCopyPass(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4712) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5de06399d7f9eada"></a>[`public static bool Inno.Native.Sdl3.SDLGPUCopyPass.operator !=(Inno.Native.Sdl3.SDLGPUCopyPass left, Inno.Native.Sdl3.SDLGPUCopyPass right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4714) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-bb6bd9a4a608b72a"></a>[`public nint Inno.Native.Sdl3.SDLGPUCopyPass.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4708) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-fd8b4a2d7e1ea4dc"></a>[`public bool Inno.Native.Sdl3.SDLGPUCopyPass.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4709) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7dff01a7aa5f5f8e"></a>[`public static Inno.Native.Sdl3.SDLGPUCopyPass Inno.Native.Sdl3.SDLGPUCopyPass.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4710) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e46f22d699b51269"></a>[`public readonly struct Inno.Native.Sdl3.SDLGPUCopyPass : System.IEquatable<Inno.Native.Sdl3.SDLGPUCopyPass>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4701) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUCubeMapFace</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUCubeMapFace 162d264fc25e5aec5bb7cd3dd6c4eb377d8d3e89cb9800f5c8357ea633c5364c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCubeMapFace.CubemapfacePositivex a0b9b49cbcf3ff96116cf0f4c6b6dff5a13ad6069186af845a7d0169e30abd0e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCubeMapFace.CubemapfaceNegativex 23530df87730c007e46a69c7f0ef6637e84e9bcbb1598a34141276122783e664 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCubeMapFace.CubemapfacePositivey d49f6dade89a2a83d7eb01f62a18f42c8de07184c3d3f93e285c35159a2c805f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCubeMapFace.CubemapfaceNegativey 7796b2ecefcf7836b881ee939919d9a2a195c516b516b730247d4ce65be570cc -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCubeMapFace.CubemapfacePositivez 5743c2827c8eb390e0f6e98fc497a8a810dd3eabc4651d630e36a9db17415705 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCubeMapFace.CubemapfaceNegativez ba8226ab5fe228c78087cac19345090843d8365e6ac4571c75047614a987388f -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-78e541f5ed067d4f"></a>[`Inno.Native.Sdl3.SDLGPUCubeMapFace.CubemapfaceNegativex = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1525) | 当前常量或枚举值：`1`。 | — |
| <a id="api-f2d248ca5e50787d"></a>[`Inno.Native.Sdl3.SDLGPUCubeMapFace.CubemapfaceNegativey = 3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1527) | 当前常量或枚举值：`3`。 | — |
| <a id="api-55decb1971d99c22"></a>[`Inno.Native.Sdl3.SDLGPUCubeMapFace.CubemapfaceNegativez = 5`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1529) | 当前常量或枚举值：`5`。 | — |
| <a id="api-2b848b6e23b2c300"></a>[`Inno.Native.Sdl3.SDLGPUCubeMapFace.CubemapfacePositivex = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1524) | 当前常量或枚举值：`0`。 | — |
| <a id="api-9c40a51e5b0edf0b"></a>[`Inno.Native.Sdl3.SDLGPUCubeMapFace.CubemapfacePositivey = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1526) | 当前常量或枚举值：`2`。 | — |
| <a id="api-7b92c592579cf94a"></a>[`Inno.Native.Sdl3.SDLGPUCubeMapFace.CubemapfacePositivez = 4`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1528) | 当前常量或枚举值：`4`。 | — |
| <a id="api-27944495b5d43bba"></a>[`public enum Inno.Native.Sdl3.SDLGPUCubeMapFace : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1522) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUCullMode</code> · 4 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUCullMode adce0c31d63ca18dca8fdf356057c55e40509011813f1cb6c562897d21099c4f -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCullMode.CullmodeNone 849b4704abf4f279e0fe554eb46cc70ef32b60d6326d698458479f16f429364c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCullMode.CullmodeFront 7c247b1f96d2b7c49571d4177c288527fd63f451573cec9f110439dc227db893 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUCullMode.CullmodeBack 979359932b3e27dc48a473e926d635d45d75ce0fa35e3ea2c3693f2a37949224 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-6e2d52dd56140c87"></a>[`Inno.Native.Sdl3.SDLGPUCullMode.CullmodeBack = 2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1613) | 当前常量或枚举值：`2`。 | — |
| <a id="api-6941bf94a4b886b5"></a>[`Inno.Native.Sdl3.SDLGPUCullMode.CullmodeFront = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1612) | 当前常量或枚举值：`1`。 | — |
| <a id="api-3334c3b91c131200"></a>[`Inno.Native.Sdl3.SDLGPUCullMode.CullmodeNone = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1611) | 当前常量或枚举值：`0`。 | — |
| <a id="api-bbed636aeb637d13"></a>[`public enum Inno.Native.Sdl3.SDLGPUCullMode : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1609) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUDepthStencilState</code> · 13 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUDepthStencilState bf271792491bdfcdd2d3254d21def7b3123c642225403b974c3d152837852152 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.CompareOp e23991a5cf33966cf22c22c79feb86426c599cb704bcfd04f3f4bb446eaf4c53 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.BackStencilState bf1c92f5b4ad80d8adebb51b721b0f3f50878c3760a896a0a9ef0a370f20ef99 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.FrontStencilState 1f6b6c29c819331cef17f3ba260d23e95b0a84a43b893f73796269318262631b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.CompareMask 1f31f9141b18e3b8cdac7c080148cdce32bddb3a74b17f43beb7073d512d10a9 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.WriteMask 31281812b316b9c251d4bc1bca28f19f88df6aaee4e4b6fbb123357b190bfac6 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.EnableDepthTest cc1b57f963459e25607c5e78c75bde570ff9b2504dd9cdc3a4d8553a3c180af8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.EnableDepthWrite 4b39363a5a8e05fb350d5ad3b9d7748c23090a2e87fedf699c130233b1d47042 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.EnableStencilTest c4847c9d41f6290229c7df263e942bcb28d03d3e0ba2d712fad7e47d0c9432d1 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.Padding1 9c786dc66e9dd83d10ca7ab66d7f7ca0d4a56f96f09882d70aed322d471bb1e0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.Padding2 6e5c69f9715f865c9ec7a5c6f8225c5d6abec1824251542ab958e9036c899d80 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilState.Padding3 e93bd4844115d7fb141d9ce748b2f233b844d3d27d8e3badbbb11aa552f0b264 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDepthStencilState.#ctor(Inno.Native.Sdl3.SDLGPUCompareOp,Inno.Native.Sdl3.SDLGPUStencilOpState,Inno.Native.Sdl3.SDLGPUStencilOpState,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte,System.Byte) 6784e522d99593988d61b77a6af512e3829f12a34ab12fc6f034cf433dbf8530 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9c974ae4731ac100"></a>[`public Inno.Native.Sdl3.SDLGPUStencilOpState Inno.Native.Sdl3.SDLGPUDepthStencilState.BackStencilState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5275) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4335843960ae8585"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.CompareMask`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5277) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bb024dc262910bde"></a>[`public Inno.Native.Sdl3.SDLGPUCompareOp Inno.Native.Sdl3.SDLGPUDepthStencilState.CompareOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5274) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-725f56de512611bd"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.EnableDepthTest`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5279) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-13f03fe6b2ce3452"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.EnableDepthWrite`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5280) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3879406e4cefcc50"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.EnableStencilTest`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5281) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-8ff589fda47eed30"></a>[`public Inno.Native.Sdl3.SDLGPUStencilOpState Inno.Native.Sdl3.SDLGPUDepthStencilState.FrontStencilState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5276) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-09324ff27cd87935"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5282) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e0f354a746691e5a"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5283) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-63e2fb1097c0657c"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.Padding3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5284) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a6524f13ce6a9be7"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilState.WriteMask`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5278) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-afa670231683fb33"></a>[`public Inno.Native.Sdl3.SDLGPUDepthStencilState.SDLGPUDepthStencilState(Inno.Native.Sdl3.SDLGPUCompareOp compare_op = Inno.Native.Sdl3.SDLGPUCompareOp.CompareopInvalid, Inno.Native.Sdl3.SDLGPUStencilOpState back_stencil_state = default(Inno.Native.Sdl3.SDLGPUStencilOpState), Inno.Native.Sdl3.SDLGPUStencilOpState front_stencil_state = default(Inno.Native.Sdl3.SDLGPUStencilOpState), byte compare_mask = 0, byte write_mask = 0, byte enable_depth_test = 0, byte enable_depth_write = 0, byte enable_stencil_test = 0, byte padding1 = 0, byte padding2 = 0, byte padding3 = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5285) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-2b38358ed93c3f17"></a>[`public struct Inno.Native.Sdl3.SDLGPUDepthStencilState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5272) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo</code> · 12 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo b7f3816902da6bf8856ae042f5d1010f4233a6b2030606a5ae6b9446c43cc80e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.Texture 6fe60835cd33498d83d2326f1f7e3839ae576cb0b3e82b635d8e2d08d2b6dfdb -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.ClearDepth 14e22998a1554845d81748a40f0aa91b3d12abd0f4aea03f018c55522ccba426 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.LoadOp 8b92d4d76218accf01c45f88c4501a633db8bbf29824ed96da97ac4a6521b97d -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.StoreOp fd7f11023584900024329f9aa7cb629a8011f991a95b4c71e676f311012432cf -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.StencilLoadOp 299e6e16a3545c990eb81fdc70654aa18806760cf6db09ad3881a77954f80d29 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.StencilStoreOp e98b6ff248ba369e601af032099fa937dbeae97157d8624a8b713149ed8a8b06 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.Cycle 1376ffc573c15313b1c0d3f1aa6c6c375d2b2bae940b8536697f14f92ef2c536 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.ClearStencil b4816a76f980dce54eb83ebed723e88334cc2f46a194f9d53f2ebea4f4aea5e8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.MipLevel d37ab47077f8da4a98881ccf51f2eda15bb395a27b7ab0b2934ebe24a4c9b421 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.Layer 17ae95a8ad15742c945870a4664951d06eee3acd79fb7c5cab67f9428dc52528 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.#ctor(Inno.Native.Sdl3.SDLGPUTexture,System.Single,Inno.Native.Sdl3.SDLGPULoadOp,Inno.Native.Sdl3.SDLGPUStoreOp,Inno.Native.Sdl3.SDLGPULoadOp,Inno.Native.Sdl3.SDLGPUStoreOp,System.Byte,System.Byte,System.Byte,System.Byte) a138ffc9751ed47a950979dffdad217c1e23888698446a0d0e1e224a8f205284 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-caec6c8e385a32ce"></a>[`public float Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.ClearDepth`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5453) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-163250aa57456c1f"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.ClearStencil`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5459) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-43dd376013531022"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.Cycle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5458) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-bca61a4ce50e305a"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.Layer`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5461) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-be4ae3a3fe71c906"></a>[`public Inno.Native.Sdl3.SDLGPULoadOp Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.LoadOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5454) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-46c3d97e7a64750f"></a>[`public byte Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.MipLevel`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5460) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-5a0df9c6ed686dcc"></a>[`public Inno.Native.Sdl3.SDLGPULoadOp Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.StencilLoadOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5456) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f2bfcd70c1e12d8"></a>[`public Inno.Native.Sdl3.SDLGPUStoreOp Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.StencilStoreOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5457) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-77a5852246713489"></a>[`public Inno.Native.Sdl3.SDLGPUStoreOp Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.StoreOp`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5455) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-54d6fe3a47440f3d"></a>[`public Inno.Native.Sdl3.SDLGPUTexture Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.Texture`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5452) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e480f92932fbabf5"></a>[`public Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo.SDLGPUDepthStencilTargetInfo(Inno.Native.Sdl3.SDLGPUTexture texture = default(Inno.Native.Sdl3.SDLGPUTexture), float clear_depth = 0, Inno.Native.Sdl3.SDLGPULoadOp load_op = Inno.Native.Sdl3.SDLGPULoadOp.LoadopLoad, Inno.Native.Sdl3.SDLGPUStoreOp store_op = Inno.Native.Sdl3.SDLGPUStoreOp.StoreopStore, Inno.Native.Sdl3.SDLGPULoadOp stencil_load_op = Inno.Native.Sdl3.SDLGPULoadOp.LoadopLoad, Inno.Native.Sdl3.SDLGPUStoreOp stencil_store_op = Inno.Native.Sdl3.SDLGPUStoreOp.StoreopStore, byte cycle = 0, byte clear_stencil = 0, byte mip_level = 0, byte layer = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5462) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-90b2ea5e668764bf"></a>[`public struct Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5450) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr</code> · 23 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr 5d164cc3962b82ef1a3513dbeb237386ecb0c8dcc7c3bc3f0d6dea39e5d4a57a -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.#ctor(Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo*) bad4a7807b312ad9b09bfecaa186e4de84249c35c52874c1bd4182e3ff17591d -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Handle 3dcd8cdf3fd5504e41ec2a5f951ec4f80947122da29093ac48d400f4e65d6d34 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.IsNull 796cda7e4179b03d4dd0d11d84c605c41c21bf9baa74b3092255f6be143aa4f3 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Null 3740d714b349b306d6997ee45bb69b2036dca90571a65e263bb5a4acbc750509 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Item(System.Int32) 1df49525b0ecc8276b55bbc4e1293612111daf9f3e6317174effefb33aeed6db -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo*)~Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr ccad1c25fa1f89e992241e47b91031a3e17bb844be3fa3af2d46a1b7921beec0 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr)~Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo* c8fdd685e31586d37eaa31efbb295b206822b9da9ecf6ed6f569ff7f25de7ad7 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.op_Equality(Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr,Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr) f93c7f16520a6e0b881c4416ecfb3f137d71d287b6a2b7f70470575732cf0362 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr,Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr) 1c2c1afd326d409d1e5d807e7cceaf73fcef529b225a6fdb01656047656f72b8 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr) abaa01099155c48b0f270758cb6c60c68be1545ae479e96f8adc733770cfd891 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Equals(System.Object) 67fc79d1a8438152d898f304a39bae421781aa780d4ac828fca77f4d8aaa8f7f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.GetHashCode a313996f07a3648e0f7037abadfa933c9a098dd65e5809187f0ecfb016829f97 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Texture 9e3c30e096046afc10df175f63431a21a3861d6766a25117e4bc275c1c1b93a1 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.ClearDepth c69b9c9414f360a66da001c27f865026fd27e86d3e5c63a77b0dfd6e8f12cba4 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.LoadOp bf7fe8f429c1dd79c7d1c10d6e79b8ec05c379bf9420c6a521b11ce97b792ef1 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.StoreOp b0cb3b013df79f7d5cd97c11e84f19246401888ebeaee6fca9f793abd6d54019 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.StencilLoadOp 114e88522849e34a33db4af21cda195e7773bbe2c5a5ae330eac55ea25ed0d17 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.StencilStoreOp 5ffc04b754c16cb792257a12c58e80187a286c2b3cb9c1d80b80394d64d7fc86 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Cycle 805edab112dc6da7b6bea52937cb384214e5bfc80f00c045d696915ea792dc87 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.ClearStencil b09d654ab6102b885c5809c5a4ecc1161533080ec9f852e97c37d93bcacd0be2 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.MipLevel f991ea04c8fb1525d5bab801a83b7d4812ae25148a297aa4999f0ae0b4f8d218 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Layer 0696aad2b2f4c8adbf0f732f77a84c7ed1d8dcc7ef3df024015931d03aa50a7e -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-988c1f6f28967ba4"></a>[`public Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo* Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8673) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2ab7df353babe971"></a>[`public Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.SDLGPUDepthStencilTargetInfoPtr(Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8668) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a4f68e565b2d6597"></a>[`public bool Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8683) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-adec7ac74d893004"></a>[`public override bool Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8684) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-9e4966e977c73b9c"></a>[`public override int Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8685) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-ef774780c9c2a8ce"></a>[`public static bool Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.operator ==(Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr left, Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8681) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-84c22b13bf8960a8"></a>[`public static Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr(Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8679) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b4eb1b0838486808"></a>[`public static Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo*(Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8680) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-fe349ceb7d1eb3f2"></a>[`public static bool Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.operator !=(Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr left, Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8682) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d01699e6b1504f4e"></a>[`public ref float Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.ClearDepth { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8688) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-b4e65a198d39e6e8"></a>[`public ref byte Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.ClearStencil { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8694) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-be0108a014d68ee8"></a>[`public bool Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Cycle { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8693) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7ea73a3372def5f9"></a>[`public bool Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8674) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ba6ec791b9751638"></a>[`public Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfo Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8677) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-1d792dd16ec6bd7e"></a>[`public ref byte Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Layer { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8696) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ec9bf600792e40ec"></a>[`public ref Inno.Native.Sdl3.SDLGPULoadOp Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.LoadOp { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8689) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9503ea80a189e95b"></a>[`public ref byte Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.MipLevel { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8695) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9d89eef0b0b16450"></a>[`public static Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8675) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-074d03a2a2aabeb1"></a>[`public ref Inno.Native.Sdl3.SDLGPULoadOp Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.StencilLoadOp { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8691) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-475a1a5d5c559cff"></a>[`public ref Inno.Native.Sdl3.SDLGPUStoreOp Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.StencilStoreOp { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8692) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7ab6bfd55983c1ba"></a>[`public ref Inno.Native.Sdl3.SDLGPUStoreOp Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.StoreOp { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8690) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-99809084e76b4dad"></a>[`public ref Inno.Native.Sdl3.SDLGPUTexture Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr.Texture { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8687) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-8a55066c6cb7353b"></a>[`public struct Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUDepthStencilTargetInfoPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8666) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUDevice</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUDevice 4557fe4ac5883d0abd46ea4e3ba4167daa62799ceb4d28d8aa0e5e2e37f76d84 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDevice.#ctor(System.IntPtr) 071d079f75e9dac74da66d413eb17bd3515f94b95789718ec9794a3fa8016657 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDevice.Handle a5a07674a5d8bc0a32fdce96bf32f9ff00a5e2d12f5d74c4408541557ceb684f -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDevice.IsNull a6f25262ef29e6c00eea36284ca52e67cfc518e9aaff516eb51c2f910b62b90a -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUDevice.Null 178ff6e02ef62dac0128d0bb89abbba4c6bfb54ea056a0983c2df3c52d3d2c4a -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDevice.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLGPUDevice dba96d2bc443de54b7de5168fb81fb64488e0a68c996bdd0fc76fbb5fc0d1a00 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDevice.op_Equality(Inno.Native.Sdl3.SDLGPUDevice,Inno.Native.Sdl3.SDLGPUDevice) 5b1d0a9bce65a9226a3a962d44ec46725039510acb4a8eb73465067f919becc5 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDevice.op_Inequality(Inno.Native.Sdl3.SDLGPUDevice,Inno.Native.Sdl3.SDLGPUDevice) 72056b9eac716e418988a09ec345b4b12ad3e1782f1548d775e26b0682976067 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDevice.Equals(Inno.Native.Sdl3.SDLGPUDevice) 0f6587d181c2b1c4cae496489fff4af7058da9321d5194c9dc079df65fcb4e63 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDevice.Equals(System.Object) 60310aeb7dc71ed8e2b02aebd8b633855a5737d37e94390f29b8ea7112726b53 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUDevice.GetHashCode 11902b02135187ff7e91819fff05730006b0c768c4a707e6108584ac3292257c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-f1012ef23ff449ff"></a>[`public Inno.Native.Sdl3.SDLGPUDevice.SDLGPUDevice(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4439) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-afb7ccb5f7365307"></a>[`public bool Inno.Native.Sdl3.SDLGPUDevice.Equals(Inno.Native.Sdl3.SDLGPUDevice other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4451) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-40525de2df9520d6"></a>[`public override bool Inno.Native.Sdl3.SDLGPUDevice.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4452) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-f21abc40423ff1a5"></a>[`public override int Inno.Native.Sdl3.SDLGPUDevice.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4453) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-f5ae1eba130ec175"></a>[`public static bool Inno.Native.Sdl3.SDLGPUDevice.operator ==(Inno.Native.Sdl3.SDLGPUDevice left, Inno.Native.Sdl3.SDLGPUDevice right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4449) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-593dcbddbcb66ead"></a>[`public static Inno.Native.Sdl3.SDLGPUDevice.implicit operator Inno.Native.Sdl3.SDLGPUDevice(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4448) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-889583e369455b4f"></a>[`public static bool Inno.Native.Sdl3.SDLGPUDevice.operator !=(Inno.Native.Sdl3.SDLGPUDevice left, Inno.Native.Sdl3.SDLGPUDevice right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4450) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-74a56ff0a6f9b803"></a>[`public nint Inno.Native.Sdl3.SDLGPUDevice.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4444) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4d66eebb2321cf9c"></a>[`public bool Inno.Native.Sdl3.SDLGPUDevice.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4445) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-18a4b2d9285b94a5"></a>[`public static Inno.Native.Sdl3.SDLGPUDevice Inno.Native.Sdl3.SDLGPUDevice.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4446) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2712c5066847098b"></a>[`public readonly struct Inno.Native.Sdl3.SDLGPUDevice : System.IEquatable<Inno.Native.Sdl3.SDLGPUDevice>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4437) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUFence</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUFence 37046057d011246ab4b3a781f32009696c9d4ee7b686b24f40765a444f22786e -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUFence.#ctor(System.IntPtr) b8430b0a6e3a45886e07e530bbee97621a64d2a36d1cf76f56a1e90536b722b3 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUFence.Handle d3029b342b98d1e3bb9ec0f90d7f56bc12713da5c4abcdeff557790b48247121 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUFence.IsNull 07f15ee71479ee77d6f2213372a2f278cafc18a34996c1fbbf8bfc2d09eec6ec -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUFence.Null 5fa5f59697adace05b0e4cac0017a4ab4a150eb2e0d21ddfa746bc0c89f9a42e -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUFence.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLGPUFence 6b1f0be19fc7a6f99c7ce894c5f74f030f14feac088384eb2fa0337b7b3ee31e -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUFence.op_Equality(Inno.Native.Sdl3.SDLGPUFence,Inno.Native.Sdl3.SDLGPUFence) 90a2c24b213e40910fc1d68738b578f0bebca36bda605653e83a0f9d54af2938 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUFence.op_Inequality(Inno.Native.Sdl3.SDLGPUFence,Inno.Native.Sdl3.SDLGPUFence) be7244f322d79678a9d1046ea4dff9c4de1abc1a42b22ba64bab96a5d4156368 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUFence.Equals(Inno.Native.Sdl3.SDLGPUFence) 2fe0ed211a92a779ac699174b6e15f86f70417ec56af3a64475e02f954cbccec -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUFence.Equals(System.Object) ae7b9577bd4df58fbc9e2be3e644646e770000dbd117b57726c2c5146ba84a64 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUFence.GetHashCode 36de4e425db6e2d983ba1aaa056199c5f1172c3b8cbd312489676d334d830068 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-b457e48c56c27c9c"></a>[`public Inno.Native.Sdl3.SDLGPUFence.SDLGPUFence(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4727) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-1037d3a556254823"></a>[`public bool Inno.Native.Sdl3.SDLGPUFence.Equals(Inno.Native.Sdl3.SDLGPUFence other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4739) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-9e42a1c927b27570"></a>[`public override bool Inno.Native.Sdl3.SDLGPUFence.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4740) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-721f62e23fa51f95"></a>[`public override int Inno.Native.Sdl3.SDLGPUFence.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4741) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-b05d5a397604dbe0"></a>[`public static bool Inno.Native.Sdl3.SDLGPUFence.operator ==(Inno.Native.Sdl3.SDLGPUFence left, Inno.Native.Sdl3.SDLGPUFence right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4737) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-095f6bce8ad87ea7"></a>[`public static Inno.Native.Sdl3.SDLGPUFence.implicit operator Inno.Native.Sdl3.SDLGPUFence(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4736) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-5ee4cf39d3d71879"></a>[`public static bool Inno.Native.Sdl3.SDLGPUFence.operator !=(Inno.Native.Sdl3.SDLGPUFence left, Inno.Native.Sdl3.SDLGPUFence right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4738) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-86efe9252706438c"></a>[`public nint Inno.Native.Sdl3.SDLGPUFence.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4732) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-df859c9c71477289"></a>[`public bool Inno.Native.Sdl3.SDLGPUFence.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4733) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-d796999929eb380c"></a>[`public static Inno.Native.Sdl3.SDLGPUFence Inno.Native.Sdl3.SDLGPUFence.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4734) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-eb48bea2241e96d3"></a>[`public readonly struct Inno.Native.Sdl3.SDLGPUFence : System.IEquatable<Inno.Native.Sdl3.SDLGPUFence>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4725) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUFillMode</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUFillMode 1f09da08c710716aa4ef5a15c778c218be8e5b737242535c2a467c21f97718e3 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUFillMode.FillmodeFill c1e55255902e0a5a099a8d7d7f7cc7d73deca28c132189baf7037e0ac62c30b3 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUFillMode.FillmodeLine 65db280acb22f88dcc70a11b15920aa994eb29f6b6685e727adf1d14196402cb -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-df1d631ea26b9ad5"></a>[`Inno.Native.Sdl3.SDLGPUFillMode.FillmodeFill = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1602) | 当前常量或枚举值：`0`。 | — |
| <a id="api-ce9961be41179149"></a>[`Inno.Native.Sdl3.SDLGPUFillMode.FillmodeLine = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1603) | 当前常量或枚举值：`1`。 | — |
| <a id="api-2409ee802dc55d4e"></a>[`public enum Inno.Native.Sdl3.SDLGPUFillMode : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1600) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUFilter</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUFilter 3afb4cc8af6abd5dc90b1804377880b756c1755589ce81f85c5bff7772bf80d0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUFilter.Nearest feb2d29f6ba96de36a47b9b23c9e79a1b0f1f5fd16e160f7aff1287d7dc684b0 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUFilter.Linear 46e63c793297609c257cf2e2df0d405962e0643db6a6aedb8d17c855cfcb0825 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-112cd1f8252c3c4a"></a>[`Inno.Native.Sdl3.SDLGPUFilter.Linear = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1697) | 当前常量或枚举值：`1`。 | — |
| <a id="api-46ee44bb6c1070b3"></a>[`Inno.Native.Sdl3.SDLGPUFilter.Nearest = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1696) | 当前常量或枚举值：`0`。 | — |
| <a id="api-a34cc4021be1734f"></a>[`public enum Inno.Native.Sdl3.SDLGPUFilter : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1694) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUFrontFace</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUFrontFace fa0e590f4ad008626254931697f13bed392c89da5505e0c0188738f9a34794e9 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUFrontFace.FrontfaceCounterClockwise befab47bc2cde316e8bc9cdd9392c09a07fe2e3e11e1890f42f6a7d1dead6be2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUFrontFace.FrontfaceClockwise 1d5afcbb44ca68f80feedd9aac3c36b41d78c5997ff68f430626fb75ccf09da9 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c51b33ac56b25598"></a>[`Inno.Native.Sdl3.SDLGPUFrontFace.FrontfaceClockwise = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1622) | 当前常量或枚举值：`1`。 | — |
| <a id="api-04f19360188bb53a"></a>[`Inno.Native.Sdl3.SDLGPUFrontFace.FrontfaceCounterClockwise = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1621) | 当前常量或枚举值：`0`。 | — |
| <a id="api-49a50892c36f8f13"></a>[`public enum Inno.Native.Sdl3.SDLGPUFrontFace : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1619) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUGraphicsPipeline</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUGraphicsPipeline 3d5b7646d2b7e93d86ca9f00cabc0e24ac0c6be258ad5bbb051d99e72b699a82 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipeline.#ctor(System.IntPtr) 7d29ae6d8644cafadf836e462e66206cf021bddb37dad7e05ae38c7256086925 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipeline.Handle 6bf17442755ce5124aa38e5aad2d87b435f6b072e80ca62710e6b28f12d0e1bc -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipeline.IsNull 29d62c4254e676ae3b18ac65b3570f017dbb8b72c58d61b7c4643d5cc7866a57 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipeline.Null 6a9bac8cd83c76c5afc1dd3b64589656bebb26cd9b8f7a3918f20ec94790ba75 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipeline.op_Implicit(System.IntPtr)~Inno.Native.Sdl3.SDLGPUGraphicsPipeline cef7a261202a86569b549bec75023262fa2f210b29e0a57da8fff99741318e65 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipeline.op_Equality(Inno.Native.Sdl3.SDLGPUGraphicsPipeline,Inno.Native.Sdl3.SDLGPUGraphicsPipeline) c8f0939f4c711a7037e4a14005a637b4824087003f42b1e8e7de7d5c1e37f0f1 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipeline.op_Inequality(Inno.Native.Sdl3.SDLGPUGraphicsPipeline,Inno.Native.Sdl3.SDLGPUGraphicsPipeline) 4510bdf7388f4a81724fd2d9c6a9adf399b9bf43765376e04ebe71395a62565a -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipeline.Equals(Inno.Native.Sdl3.SDLGPUGraphicsPipeline) 16558250497527a4a8dea23f8b653ed1b886edfede73f021b33ee1a7badd2cbe -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipeline.Equals(System.Object) 9d81309b1ba123c037b48c59e0dac2724f2bb1741a108277223ceac0352b9d02 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipeline.GetHashCode 06be2d2cbd65fa9391b33ca723567bd56e5282e31b71ca99e004bec6f907fc65 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0961d4ef8ab22f5b"></a>[`public Inno.Native.Sdl3.SDLGPUGraphicsPipeline.SDLGPUGraphicsPipeline(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4607) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-a8437065c137417a"></a>[`public bool Inno.Native.Sdl3.SDLGPUGraphicsPipeline.Equals(Inno.Native.Sdl3.SDLGPUGraphicsPipeline other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4619) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-25bca2204cb3cf8a"></a>[`public override bool Inno.Native.Sdl3.SDLGPUGraphicsPipeline.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4620) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-a85cc755753c2c1b"></a>[`public override int Inno.Native.Sdl3.SDLGPUGraphicsPipeline.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4621) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-27518d2fa9612b3f"></a>[`public static bool Inno.Native.Sdl3.SDLGPUGraphicsPipeline.operator ==(Inno.Native.Sdl3.SDLGPUGraphicsPipeline left, Inno.Native.Sdl3.SDLGPUGraphicsPipeline right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4617) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-1038a51b82322b37"></a>[`public static Inno.Native.Sdl3.SDLGPUGraphicsPipeline.implicit operator Inno.Native.Sdl3.SDLGPUGraphicsPipeline(nint handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4616) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-d4d76b143644212f"></a>[`public static bool Inno.Native.Sdl3.SDLGPUGraphicsPipeline.operator !=(Inno.Native.Sdl3.SDLGPUGraphicsPipeline left, Inno.Native.Sdl3.SDLGPUGraphicsPipeline right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4618) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-2ab65ade95b09d72"></a>[`public nint Inno.Native.Sdl3.SDLGPUGraphicsPipeline.Handle { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4612) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-cd1d0a46d0c320f3"></a>[`public bool Inno.Native.Sdl3.SDLGPUGraphicsPipeline.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4613) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-7727f92e27e9e60a"></a>[`public static Inno.Native.Sdl3.SDLGPUGraphicsPipeline Inno.Native.Sdl3.SDLGPUGraphicsPipeline.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4614) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f013755876e1442c"></a>[`public readonly struct Inno.Native.Sdl3.SDLGPUGraphicsPipeline : System.IEquatable<Inno.Native.Sdl3.SDLGPUGraphicsPipeline>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4605) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo</code> · 11 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo caa018c3aa321dce91cd94988646c1a6c195d2ab78b7e4c44a3168b9be3913db -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.VertexShader 986a451b07f2da7c31e935a4fc0bbd3763826e6c48c79b3d449c48c9b1cff73e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.FragmentShader 75d1bc6c0ab1a0f81651a1850d379f03a5b996c159471f1185e431c437c2df01 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.VertexInputState 75f0dd2b08831f4f2e3d2bfc956a0de17a3eb9876a704c43c53be5f5459dc813 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.PrimitiveType 06ce54afb192ae3311631ed1fbe41b9f436b73cb245d41eb2024e1fb60834688 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.RasterizerState b1c8fd9f24b736b94a17ce8c623f08d60d8e0a53bb25dd40c6e62bd53c76a9ea -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.MultisampleState 54c567058304d3d6f673ae6029364c635ca6db1f1b1bdbb2abed229fbc857a9a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.DepthStencilState 3fa460ad986c53db65499bee5271ce0b949e54a02af461aeed764d404aa7b37c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.TargetInfo 570e664245e7b54cc457ed2a0d06266cb1282df33098436050502f58c2939c2b -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.Props bff4bc9b99ab889f968e0a7fdf241e979066860b3ccbecd73771425d56d468cb -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.#ctor(Inno.Native.Sdl3.SDLGPUShader,Inno.Native.Sdl3.SDLGPUShader,Inno.Native.Sdl3.SDLGPUVertexInputState,Inno.Native.Sdl3.SDLGPUPrimitiveType,Inno.Native.Sdl3.SDLGPURasterizerState,Inno.Native.Sdl3.SDLGPUMultisampleState,Inno.Native.Sdl3.SDLGPUDepthStencilState,Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo,System.UInt32) 6dd8463d458740292f289d7ced08e25559cb518b4bce6e1438f71da24081150a -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c6d5951ef93d04fe"></a>[`public Inno.Native.Sdl3.SDLGPUDepthStencilState Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.DepthStencilState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5353) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-257b0b445eda67ec"></a>[`public Inno.Native.Sdl3.SDLGPUShader Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.FragmentShader`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5348) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-9f31f824128f656d"></a>[`public Inno.Native.Sdl3.SDLGPUMultisampleState Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.MultisampleState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5352) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-d9cf5840dd9fe140"></a>[`public Inno.Native.Sdl3.SDLGPUPrimitiveType Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.PrimitiveType`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5350) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-678417503d8a59f3"></a>[`public uint Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.Props`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5355) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a08e2189312d1013"></a>[`public Inno.Native.Sdl3.SDLGPURasterizerState Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.RasterizerState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5351) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-05aaebcde82282ad"></a>[`public Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.TargetInfo`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5354) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0d900ef2e786d226"></a>[`public Inno.Native.Sdl3.SDLGPUVertexInputState Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.VertexInputState`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5349) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-6b2db7963bf9728b"></a>[`public Inno.Native.Sdl3.SDLGPUShader Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.VertexShader`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5347) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-10b663e08224e14c"></a>[`public Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo.SDLGPUGraphicsPipelineCreateInfo(Inno.Native.Sdl3.SDLGPUShader vertex_shader = default(Inno.Native.Sdl3.SDLGPUShader), Inno.Native.Sdl3.SDLGPUShader fragment_shader = default(Inno.Native.Sdl3.SDLGPUShader), Inno.Native.Sdl3.SDLGPUVertexInputState vertex_input_state = default(Inno.Native.Sdl3.SDLGPUVertexInputState), Inno.Native.Sdl3.SDLGPUPrimitiveType primitive_type = Inno.Native.Sdl3.SDLGPUPrimitiveType.PrimitivetypeTrianglelist, Inno.Native.Sdl3.SDLGPURasterizerState rasterizer_state = default(Inno.Native.Sdl3.SDLGPURasterizerState), Inno.Native.Sdl3.SDLGPUMultisampleState multisample_state = default(Inno.Native.Sdl3.SDLGPUMultisampleState), Inno.Native.Sdl3.SDLGPUDepthStencilState depth_stencil_state = default(Inno.Native.Sdl3.SDLGPUDepthStencilState), Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo target_info = default(Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo), uint props = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5356) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-bd2979b6e9123982"></a>[`public struct Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5345) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr</code> · 22 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr 510ba9c4156b729f3e5406b7a5af0bc3cd7ecf885a48025e19cdfae323838b29 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.#ctor(Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo*) c393baf8307f22f9c684e429a2e8af42260de343f61f9dcb4d84cdc03a475022 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.Handle 46a144e5df087626d624c4fccc47afafbaf1a66bedd38e1f8e05a4f29a4e9b77 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.IsNull 37b066c2c2f61b46ddd4ef6a2b6ce13524e9789a649ac956f9d68c8f3de7cfda -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.Null 881adf5896b9a8cdb56912898447bab8b01fb5dcf3fcc61ca420beaa97d4eff3 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.Item(System.Int32) 13addfcc9682de95fe86cd561b467fd82fe0016aafdc0b63f978faa3d4931515 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo*)~Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr 795be213a4cbeb323bd699662576c4d66447b147e57cb5a2306ae24c0cb22511 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.op_Implicit(Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr)~Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo* adc04af1fa144b94124be15325e4925ec33b5de697e03b0e59fc5371215a3ef7 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.op_Equality(Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr,Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr) 0ab87409ddbadf636a6f5ad92db39cc0fec5f21757841f1cead0c8395ae44819 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.op_Inequality(Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr,Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr) 9c22807837775e87f64862a60f35d71039ce4c1ed3b5603f04e1c63e40b9c5a0 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr) 28499d02c81da2ca22fd4870bf1c528075c8d008baa7dc3e97fc79b9e265f739 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.Equals(System.Object) a785459d7b405957bf2b644454f57fe2c2b469e733ac332915cbb1f95d74e06d -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.GetHashCode fcf359dd981532a27a46ca4b1267f655a8e56a57b6bbb62c49ed185ae298e2c6 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.VertexShader 139f10b369b886a7890c4fc4050a40bf800fbf53fc423797d56b53b3cdd1c3dd -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.FragmentShader dd6941ded41305e1cdf8707fc8bd35259b1fdcea3956bc4d77099f9252a2cf5b -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.VertexInputState 4ad14f250a668a493833729b4fd795fae973133b9c7e9cca06baeb423eabe5f4 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.PrimitiveType fc5685650903bd7a7056e15877eb0cb56283fe0e3c359e5b71107237d55f81e3 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.RasterizerState acc2c17b2d25eebcc0b765add4c03ade060c7f1615102b322aab6a25df4963fa -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.MultisampleState bd21549ef2dec8cbe99677a7af07f0c979676e4e012cc35ca2380d695961bfb9 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.DepthStencilState c9951950e53fc3b8a1bfbc674371dd3241ab1bd333e818e6e1555482d66d6585 -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.TargetInfo 18324dff9b9b9f523660f5eda58203438ed34f891bfe0a7f73131c629dc1ae4b -->
<!-- api: P:Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.Props ceb59631fe306466838717dd8c67bc776a3e42e17a223fb0218206bd7317f551 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-0e70791144a005c4"></a>[`public Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo* Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.Handle`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8707) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ccf5a8ed93a18b75"></a>[`public Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.SDLGPUGraphicsPipelineCreateInfoPtr(Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8702) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-252614c669c6eb61"></a>[`public bool Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.Equals(Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr other)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8717) | Indicates whether the current object is equal to another object of the same type.<br>返回：if the current object is equal to the `other` parameter; otherwise, . | — |
| <a id="api-61978ed5bc603c8b"></a>[`public override bool Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.Equals(object? obj)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8718) | Indicates whether this instance and a specified object are equal.<br>返回：if `obj` and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-4202cd18dc44a143"></a>[`public override int Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.GetHashCode()`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8719) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-2061974dcbeea32d"></a>[`public static bool Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.operator ==(Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr left, Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8715) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-737b32779b2cd437"></a>[`public static Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr(Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo* handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8713) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-b32423fe1db16d59"></a>[`public static Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.implicit operator Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo*(Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr handle)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8714) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-9d47dc59a9f5a7df"></a>[`public static bool Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.operator !=(Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr left, Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr right)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8716) | 当前生成调用或值操作；参数、返回值及泛型约束见精确声明。 | — |
| <a id="api-4d6724eb3423c359"></a>[`public ref Inno.Native.Sdl3.SDLGPUDepthStencilState Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.DepthStencilState { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8727) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-f80319d8fbc4c4c4"></a>[`public ref Inno.Native.Sdl3.SDLGPUShader Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.FragmentShader { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8722) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-41798b95aa988fc2"></a>[`public bool Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.IsNull { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8708) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-a446ff49252e8022"></a>[`public Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfo Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.this[int index] { get; set; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8711) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-dbc3592707c9b571"></a>[`public ref Inno.Native.Sdl3.SDLGPUMultisampleState Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.MultisampleState { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8726) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4217a9383ad39ce4"></a>[`public static Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.Null { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8709) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-4c49b97683f583aa"></a>[`public ref Inno.Native.Sdl3.SDLGPUPrimitiveType Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.PrimitiveType { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8724) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-2294f99621eb23e7"></a>[`public ref uint Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.Props { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8729) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-53a54085bfa861e2"></a>[`public ref Inno.Native.Sdl3.SDLGPURasterizerState Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.RasterizerState { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8725) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-e84b51caa677e512"></a>[`public ref Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.TargetInfo { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8728) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-78cfe3e69993b730"></a>[`public ref Inno.Native.Sdl3.SDLGPUVertexInputState Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.VertexInputState { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8723) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-ebd757f6a66fee3f"></a>[`public ref Inno.Native.Sdl3.SDLGPUShader Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr.VertexShader { get; }`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8721) | 按声明读取或修改当前值/视图；get/set 能力由签名明确给出。 | — |
| <a id="api-9cfee4023fa9d92d"></a>[`public struct Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr : System.IEquatable<Inno.Native.Sdl3.SDLGPUGraphicsPipelineCreateInfoPtr>`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L8700) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo</code> · 9 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo 7a2e1e0ce51afaa8e946fb4c085ccf025ad38dde54fa7ef53f8ec5c73b1a708c -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.ColorTargetDescriptions 900cc2127586d4daf8f546aa1e552bf6f5d2ca9bf3964a54f907675aae694062 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.NumColorTargets 9b29700df84f9905be8163770f628d64c0f3bab1818f8a5e4a97f710a1f62aa2 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.DepthStencilFormat 61bd4db139a7b8c58a1ccf1b8904e784eb0cd6c0ac5c2b788f521efa86a1f9cb -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.HasDepthStencilTarget 2f2994eeb03ea8e4a47fa2eed5ef96e3b72266d792c9cba5d840ffd97b709658 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.Padding1 8fabc1dae780daf428158a2ab62c9e1d4ccb2fcfd1c48a2b24f42ccbd6ac4b13 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.Padding2 116659d21330f1f31a0ecca20904fa96758edcee337ee79362d16d2f9fe7eaa4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.Padding3 2b65be0427623049f4c639b267520cdacd07c431bbff5d7c0b53062ae8457592 -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.#ctor(Inno.Native.Sdl3.SDLGPUColorTargetDescription*,System.UInt32,Inno.Native.Sdl3.SDLGPUTextureFormat,System.Byte,System.Byte,System.Byte,System.Byte) 6bbac1df3eb1340f78b785ef93a3a6f01ccfada9e497d49d24eb135eba939ed6 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-c78fd011e1167f27"></a>[`public Inno.Native.Sdl3.SDLGPUColorTargetDescription* Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.ColorTargetDescriptions`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5322) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-e2204b325307818c"></a>[`public Inno.Native.Sdl3.SDLGPUTextureFormat Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.DepthStencilFormat`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5324) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-2f41456f13b234a1"></a>[`public byte Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.HasDepthStencilTarget`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5325) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-4f1bb746f2691bf6"></a>[`public uint Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.NumColorTargets`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5323) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-ee6173c73b2347f7"></a>[`public byte Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.Padding1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5326) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-35dd2c9bb79258ec"></a>[`public byte Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.Padding2`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5327) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-efbf8e2361147910"></a>[`public byte Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.Padding3`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5328) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-c5ed03b5655a30aa"></a>[`public Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo.SDLGPUGraphicsPipelineTargetInfo(Inno.Native.Sdl3.SDLGPUColorTargetDescription* color_target_descriptions = null, uint num_color_targets = 0, Inno.Native.Sdl3.SDLGPUTextureFormat depth_stencil_format = Inno.Native.Sdl3.SDLGPUTextureFormat.TextureformatInvalid, byte has_depth_stencil_target = 0, byte padding1 = 0, byte padding2 = 0, byte padding3 = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5329) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-9b5dbabf898f3d44"></a>[`public struct Inno.Native.Sdl3.SDLGPUGraphicsPipelineTargetInfo`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L5320) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand</code> · 7 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand eb40b5c2b7e8ba32fb5a167deb9a0c249da855174436ecbda696aaa1f7420acd -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.NumIndices 9177ad37b9fa31dee30501767973fb54d4339694954be2f09d493befdef7d12a -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.NumInstances 2e1b032397336d62fff2ca75ffb71806becc830e266ad2763e938547d4e79a93 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.FirstIndex e9280f97a44deb28bb5627d9f421e935edd186d7e551a91ce0514b23a7cfc5f8 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.VertexOffset 0754f6968855caede7249874ccafbe8ae05c2612abe26c6d04a0b019f4478169 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.FirstInstance 7f1123871a885ab660e493cf2641d77a2f7269c0b79329578d514a1d0355f46f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.#ctor(System.UInt32,System.UInt32,System.UInt32,System.Int32,System.UInt32) 39cb054f404d0a120a1245581d4ecb3cff8765f3ec95307001c8eb36407a085c -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-ef7de4ec61df4c1b"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.FirstIndex`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4938) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-041027202ef52e59"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.FirstInstance`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4940) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-897c854a57dcea2b"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.NumIndices`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4936) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-0be3f9088b4565b9"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.NumInstances`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4937) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-3b581729636447f9"></a>[`public int Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.VertexOffset`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4939) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-1fff02921b9f0c49"></a>[`public Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand.SDLGPUIndexedIndirectDrawCommand(uint num_indices = 0, uint num_instances = 0, uint first_index = 0, int vertex_offset = 0, uint first_instance = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4941) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-8c25b4a5c839ea52"></a>[`public struct Inno.Native.Sdl3.SDLGPUIndexedIndirectDrawCommand`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4934) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUIndexElementSize</code> · 3 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUIndexElementSize 48c06d3f4c29d5881740dffe0fc5ab2e33ef500ad4281633e8d631f8a118dae3 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexElementSize.Indexelementsize16Bit cece4c88b72fc19ed7597396a5207039c51b8825b2edc9e0013356bdd37d538e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndexElementSize.Indexelementsize32Bit d01c3cae5fd76520997df14c7e9a5e2ce38515a31abfcd05b37c260b1964d598 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-9791836d1a742562"></a>[`Inno.Native.Sdl3.SDLGPUIndexElementSize.Indexelementsize16Bit = 0`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1380) | 当前常量或枚举值：`0`。 | — |
| <a id="api-6634e3127fe6de87"></a>[`Inno.Native.Sdl3.SDLGPUIndexElementSize.Indexelementsize32Bit = 1`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1381) | 当前常量或枚举值：`1`。 | — |
| <a id="api-d756dad27d9fc6a4"></a>[`public enum Inno.Native.Sdl3.SDLGPUIndexElementSize : int`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L1378) | Native 取值集合；当前取值见本类型成员。 | — |

</details>

<details>
<summary><code>Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand</code> · 5 个声明</summary>

<!-- api: T:Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand 7974911de42591905fb44e6f0aa66373490817310f203a6e573077d195e37536 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.GroupcountX d19428a9a21278cbae8461839f157457bdbefe67539515ed87f52be79bac0a9e -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.GroupcountY 76bcda7432660566b04ec75959a7560ae6a6f52797f57da0df6c38943b8882b4 -->
<!-- api: F:Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.GroupcountZ 1ee8cd8a507bea3ab503f7b6b672f6fab4f7d7110e104e6b2a712543f81d506f -->
<!-- api: M:Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.#ctor(System.UInt32,System.UInt32,System.UInt32) 5534b5388927598794803a6db74f92ab92984228c35e334b465548bb89f1a809 -->

| 声明 | 行为与结果 / ABI 信息 | 所有权与限制 |
| --- | --- | --- |
| <a id="api-bbbb37d776ee3469"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.GroupcountX`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4957) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-edbb1b015ff68843"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.GroupcountY`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4958) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a628c2141805c09a"></a>[`public uint Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.GroupcountZ`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4959) | 当前数据布局的字段；值或地址类型由声明明确给出。 | — |
| <a id="api-a4c0b6140170cca5"></a>[`public Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand.SDLGPUIndirectDispatchCommand(uint groupcount_x = 0, uint groupcount_y = 0, uint groupcount_z = 0)`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4960) | 构造当前托管类型；此声明本身不证明取得原生销毁权。 | — |
| <a id="api-4c21a29f4982d6bd"></a>[`public struct Inno.Native.Sdl3.SDLGPUIndirectDispatchCommand`](../../../../backends/Sdl3/native/Inno.Native.Sdl3/Generated/Bindings.cs#L4955) | 当前 ABI 的托管类型或借用视图声明。 | — |

</details>
