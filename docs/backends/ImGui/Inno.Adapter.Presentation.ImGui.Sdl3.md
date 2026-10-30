# Inno.Adapter.Presentation.ImGui.Sdl3

Viewport 位置与尺寸 setter 经共享 Native ImGui 的标量 callback adapter 注册。聚合参数 ABI 留在 C++ 编译器；托管侧不强转 BGCS 的不透明 callback 地址。注册归当前 context 和 owner thread，附加窗口与 renderer 退休完成后再检查原生 callback quiescence 并注销。失败保留依赖 owner，不能提前销毁 context；详见 [Native ImGui 生命周期](Inno.Native.ImGui.md#viewport-callback-abi-适配)。

[分类索引](README.md) · [Editor ImGui](../../editor/Inno.Editor.ImGui.md) · [BGFX ImGui](Inno.Adapter.Presentation.ImGui.Bgfx.md) · [Wiki 首页](../../README.md)

`RenderFrame` 的 preparation callback 在选择所属 Context、更新显示 metrics 后执行，并早于 Native NewFrame。准备、绘制和发布期间拒绝重入或 Dispose。准备失败不创建 Native frame；绘制 callback 失败只结束所属帧，不向 renderer 发布部分 draw data，原异常继续传播。调用方仍负责平衡自己的窗口和 Push/Pop；后端不会清空其他 Context 的栈。返回的 draw-data 指针仅借用至下一帧或 Context 退休。

## 公开 API

Viewport 的位置与逻辑尺寸由 ImGui 的平台 Move/Resize 请求及 getter/setter 协议协调。
每帧的 drawable metrics 同步只更新 FramebufferScale、初始 DPI、像素尺寸与 renderer 输出，
不回写 Pos/WorkPos/Size/WorkSize，也不根据二者的暂时差异制造 Move/Resize 请求。
SDL 的真实窗口事件仍通过同一 Application extension 入口发出平台请求，避免拖动过程中把新位置拉回旧位置。

- `PlatformImGuiContext`、`ImGuiContextFlags`：ImGui/viewport session ownership。
- `PlatformImGuiContext.TryGetWindowId`：把主 viewport 或独立 viewport 的 ImGui ID 映射到当前 SDL window ID；窗口销毁后返回 `false`。
- `IPlatformImGuiRenderer`, `PlatformImGuiViewportTarget`, `ImGuiTextureHandle`：平台与 renderer bridge。
- `Sdl3PlatformApplicationImGuiExtensions`：在 SDL3 Application 上组合 ImGui。
- `ImGuiFont`, `ImGuiFontStyle`, `ImGuiIcon`：Editor presentation assets。

该项目属于 Editor deployment，不进入 Player。Context 必须在 SDL3 Application 活跃期间 Dispose；viewport 与 GPU renderer 的 retiring resources 在帧安全点释放。

每个 `PlatformImGuiContext` 独立拥有自己的 viewport、SDL window 与 renderer target 映射。Native
ImGui callback 只使用当前 `ImGuiContext` 查找 owner backend；创建第二个 context 不会覆盖第一个
context 的路由，Dispose 只注销精确 owner。该 router 保存 Host-owned backend，不保存 Plugin 类型、
实例或 delegate，因此不会延长 collectible extension generation。跨窗口拖拽时 SDL mouse capture 仅用于保持同一次按下/释放事件完整；释放鼠标不主动改变窗口焦点或层级。只有操作系统焦点事件或 ImGui 显式的 `PlatformSetWindowFocus` 请求会改变 native 窗口焦点。

这项 context 隔离不改变 BGFX 的进程约束：平台层可以正确管理多个 ImGui context，但当前 BGFX
Adapter 同一进程只允许一个活动设备。Editor Application 是唯一图形 composition root；若未来要
同时运行多个图形 Host，应选择多进程隔离或实现真正支持多 device 的另一个 Rendering backend。

## 源码归属

当前唯一源码 owner：`backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Inno.Adapter.Presentation.ImGui.Sdl3.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 交互与 viewport 尺度

CreateImGuiContext 必须显式传入 ImGuiInteractionOptions，窗口必须属于应用。主窗口与 viewport 共享应用 host/surface，附加窗口创建后登记、销毁前释放登记。Platform_GetWindowFramebufferScale 为空；每个 NewFrame 前读取已有窗口真实尺寸、像素尺度与 DPI，无窗口快照复制、WindowsX64 返回 ABI 或重复 Win32/Cocoa 解析。最小化或隐藏窗口不呈现。

## 附加视口退休与 renderer 所有权

关闭回调只解除路由/登记和 callback 数据，调用 RetireViewport 后保留原生 SDL 窗口。在下一 managed 安全点检查 Task，成功才销毁一次；失败保留 owner 并报告。Context 成功创建后接管 renderer；失败创建由调用者保留 renderer，重复创建明确拒绝。最终 Context Dispose 调用 drain，成功后释放 Native Context 与 renderer。Application 在成功退休前保留 context 登记。内部文件按 Context/Viewports/Rendering/Interop 组织；不存在另一份窗口目录或事件总线。

## 文本字体与 Context 退休

四种默认字形保留 JetBrains Mono 与 Font Awesome 图标，并合并随产品分发的 Noto Sans CJK SC 2.004。补充字体的来源、SIL Open Font License 与内容身份位于 `BuiltIn/Fonts/Supplemental`。只读取产品自己的字体资源，不探测系统安装目录；缺失、长度错误或内容身份错误立即拒绝创建 Context。

每个 Context 只持有一份固定内存的补充字体 bytes，四种 face 共享借用至 Native Context 销毁。应用不长期缓存字体数据；自定义字体注册同样补齐 CJK。该资源属于共享呈现 backend，不把文字支持下放为平台分支。

直接调用 Context 的 `Dispose()` 与 `DestroyImGuiContext()` 使用同一完成路径。全部 viewport、renderer 和 native callback 成功退休后才移除该 Context 的精确应用登记并释放字体；Pending 或失败保留登记和依赖，供合法的后续退休操作重试。应用退出先冻结其 Context 集合，成功退休逐个解除登记，退休开始后拒绝新建 Context。关闭的 Context 不会因仍存活的另一个窗口而长期驻留。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions 54465e014c4ba58cae918d664fcb0bec7e9e4f38ff3d24aa8787153af7b44c41 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions.#ctor(System.Boolean,System.Int32) 0733d09e25e12371174be90a7a93db31d613db3de1a2f69565801c25ed560ea9 -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions.commandKeyBehavior fb4d5e2f8f9ec37fa8230ee6f9cea2fc7f0be85fffec4531eb25d62501df8d7b -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions.horizontalWheelDirection f1c3fb3fdc8440c46fe5aa2aadaad66c453e6c9b16ee4a107ab4a1e114a55150 -->
<!-- api: T:Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle a399ccb0a7c6abe63653599393a0a9ee2c6a8ca901fe3734143c2709782a98c7 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.#ctor(System.UInt64) 7f5ebad0a61ba70945bdc88c877bf8464a72e9cd539bd738135652b993b7e848 -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.value 17bd7170498c41b297b3ef2be5e48aa9798ca90dee929cff101c88e24acfb2f0 -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.isValid b4d23f5ca3b9aa703a435f697ae75c09cccc6278d5bac5bc28e71f0363abd966 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.ToString edb708f8c6d7ad3f746cf051aee813fc76b58d03054e1dfc9986e4eebcd4a80f -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.op_Inequality(Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle,Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle) 865058cbaeba949416f5d72caefdea6b11928db34f7d22b07151f6c665937025 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.op_Equality(Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle,Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle) fdf1ae759ff8faa8f7166a3c2e4f6eafb5e581e3e758e13dd9f41ced4b4c9978 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.GetHashCode 6dd5d2226a5107f88ffddc0e94ac95a0b1a3f4b71df74dee0b8ceebf3bce84da -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.Equals(System.Object) 0d2e89c2991e95b4e94de3a95f670bb47d3aac7c278ffecca0b12ba8386609d1 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.Equals(Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle) e6290a09f4e3dd553f74764817ada9178c956338792bbd196b5903a2065a7f01 -->
<!-- api: T:Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget f204f9e29fc4777070ca4d68d877c42ec8b4943b2b48292223c01506732fae6f -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget.viewportId 175af3927bde0f0b21bc441fc8799d1a8eece521999fd02dc4228d70a3f33c0e -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget.windowId 016ec9d8ca2fcd45404905af26fbe3d56defa65986c9a23bb1bcd276d0b62909 -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget.nativeHandles e355b351ed699725b9a2016dab2d10535df8b99caa9d8e516551abfb2190fd24 -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget.width 517d495ddd8fb8c39211157849ae0e1bf8ee92e16692605410d869c02af47043 -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget.height 405e44dc254265b4d903ee9508826e50b1451b6463b67f347cf554c26661d065 -->
<!-- api: T:Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer e86fb57e583e5233cc8bc428fe979377fe7c8add022cd47b72f6db450bb014fd -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.supportsVertexOffsets b44548dbd5d47a974ba9fc8bd3a332dfc574f4274edfa34827052176295902ee -->
<!-- api: P:Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.supportsViewports 45abd82f74bffe6a8e46c0bce4fd932871a0df79f556cc6a38a71a9215d307ba -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.RenderMain(System.IntPtr) 59407f9137e41315b72ecb1262d6972f62bc6c9fcca2f7c5ddfadd262adc6efd -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.SynchronizeMainOutput(System.Int32,System.Int32) c37afa35d850997b8b1244e9d0ea52111313dca69b05bdd1c1537eb2b3da7f19 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.CreateViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget) f332d992ebc8f7fdefc0da4bd9448304e76eb3f5cf5baef28b96825a67fc6e16 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.ResizeViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget) 85799ae7c36d662a77b3ce1d89593da102dc13b8c1e9d0704f0c855196239c2b -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.RenderViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget,System.IntPtr) 0b06e6795c791f75ef93749450df1584e0744bb8fea3f80b8329f0518ee05e6d -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.PresentViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget) 3c162c8fe0e8b13e1dcb6cfb8fdc62b8e3f5318968e7bb1465d49055ae49ec94 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.RetireViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget) 247af96cc94719f349fd36d330c905f7c73b3fa29c1896939fa0b7372121860a -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.DrainViewportRetirements fc574c82b2ff20b466775ce433ed901aec548d638bebecb191f89cd411137441 -->
<!-- api: T:Inno.Adapter.Presentation.ImGui.PlatformImGuiContext fc23723cc2c6def17df324fa4e6c76c6aac4da7b3c95ecb19976b2d002459dad -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.TryGetWindowId(System.UInt32,System.UInt32@) fbd79083fffada7ca2cf071451090d2c14c5816539f29f0d547dbc38b638dec7 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.RegisterFontStyle(Inno.Adapter.Presentation.ImGui.ImGuiFontStyle,System.String,System.Single) fc3ce6570fa14c455c96d854858d04e3b3eaef84c64b0f2861dc47e8d1f01a4f -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.SetIniFile(System.String) 8d138552aa15758cb2eb961e2bbc723a962045d79d375d118493fcd146cde0ac -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.LoadIniSettings(System.String) 761284eff1562388f1b0bf52b558c671ff19ec2c5c06323b145616188f3dc5cc -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.TryCaptureIniSettings(System.String@,System.Boolean) 3a84c12c060eace20a633aa429db1e74fcca7227c91512572e3314d2208b25f6 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.RenderFrame(System.Action,System.Action) e3b8c8e8a70f77aeedef350832be225d517f9a9d85fafe01453467d9fcfea1ea -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.DrawImage(Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle,System.Numerics.Vector2,System.Numerics.Vector2,System.Numerics.Vector2) 9d915920ad1a61bf6bf51ee6c8535a9b77f5e44347a5d5a60313a3a93010a8e2 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.Dispose d4c038d433399287c10d9507df8e4136e8acce284d19fe5db38c6f0d249d1a48 -->
<!-- api: T:Inno.Adapter.Presentation.ImGui.Sdl3PlatformApplicationImGuiExtensions 43c72939254fab00f0dd5199297def40bbe73948b58f8410c4ba39a9893798f9 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.Sdl3PlatformApplicationImGuiExtensions.CreateImGuiContext(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication,Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow,Inno.Adapter.Presentation.ImGui.ImGuiContextFlags,Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions,Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer) 474c91685ae7f6af7ed7dba89763261750f858f5fb8128240506a98e3599b8ff -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.Sdl3PlatformApplicationImGuiExtensions.DestroyImGuiContext(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication,Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow) 25b38053a09f503d8e7b036861d9327f7d8e55201cc3d2a89e10d480ff56c45d -->
<!-- api: T:Inno.Adapter.Presentation.ImGui.ImGuiContextFlags 2294827e8bd4ad721974488383ce0417fc20f3d27b30b9ac14773e63cc22d67f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiContextFlags.None 26be2d17d86c01a3a2cb7bc67b8033bbe8210d0beec0edfe8713577b43c7f1f7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiContextFlags.EnableViewports afa49c1dacc65b49d86027b19bc369495f705c3fb7c067615d1e7dd265a9c3be -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiContextFlags.EnableDocking 97ef56df508e8d8e11a394879ec72836abd6190c84ffdfda35b9fc0c6eba0ff0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiContextFlags.EnableSmoothResize 8fcbab6ef34f07b8a28a5d8aeca80601db227766b7aa6a907899678304e10171 -->
<!-- api: T:Inno.Adapter.Presentation.ImGui.ImGuiFont 115e345bafd60262fcc00ea69b6366465e38597243a31e7c7fb04a350b2fb1a8 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.ImGuiFont.IsAvailable(Inno.Adapter.Presentation.ImGui.ImGuiFontStyle) 2f4eeccb870d56a9202203cff79049e8043496b004af2eac113ebc9f6be3b199 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.ImGuiFont.PushStyle(Inno.Adapter.Presentation.ImGui.ImGuiFontStyle) 12f259c98c7bc1cdef62b7f5e06410c46defb2fb0473a82ad2673f451e980876 -->
<!-- api: T:Inno.Adapter.Presentation.ImGui.ImGuiFontScope e55876dfac68517fb79c2e78ecf3a342cccb5326df5a4fd2276dfad76eaf63e6 -->
<!-- api: M:Inno.Adapter.Presentation.ImGui.ImGuiFontScope.Dispose a367a31f15436d101dfb3841c5cb11cd3b28c4fc2c320e005cf8fa856f189985 -->
<!-- api: T:Inno.Adapter.Presentation.ImGui.ImGuiFontStyle 612103a9ded03790d0309ca0fa23f5ac989c5e21cd2d530a3f08a2a6cdd183f2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiFontStyle.Regular bffa08538a5a27a42aa207bcc9f01b2060dd6e0c33fd53024bec52607deeadfd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiFontStyle.Bold be886a16653bc72b756d3434f8fb4e3820e9a1d386bd480a29aa59ece0cfcd23 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiFontStyle.Italic 8425479d4db51847e771c4743175796a9aea81848006570f492701110f950ab9 -->
<!-- api: T:Inno.Adapter.Presentation.ImGui.ImGuiIcon 9b7501108acc4de700cdc53e6a376b07adaebf8305cf0eef34fa4f0899c734f4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shader c31f58c911aaf7914dedcbd194251f149e4502f11036a259e3446f249f377655 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num0 30b8973d1fd829e67007e89c6508d3b63f17e759fa3a997941d4faaf4458a962 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num1 2d6227d97311bbf51d737db49b0060f0d0986163841ecf6f8ea43b1cb7ff5f05 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num2 518139a66b72249441724bcc7b2e778d9585e30bb5a4cbc1bdacc1daa203e3f3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num3 fc01158f32fb975eddf398232ad932b25cd443aa9705fd35b83cef03dcb849c0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num4 9a0b192e5a5ce289bbfc1eac25824471d733307ffc282971dae79d3df49ca43b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num5 67079eaa2dda1d0541e6fcbbc9796be861dea3dd24ed0df9803c76d33c3748c4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num6 19d34c10e2ce23d37b72c1a2e606b836fc2905b71866e1284a83371ee89938d8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num7 aa0a08bb74feb4644e8d1be5ba0ef64f1d8be1e7d49dea034c93f2ba0ab28e63 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num8 67232bd84b056a23753cbb13b5bb92411602dc1873b206656bdaecb448d16000 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num9 e8b7593cc212be5975f9a78e3fc17562b1a0cdce0ec8e6f10feb11af90850bd6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.A ab21a223c0fdb0b39cc69d2ae4e853cd55ed0a2f3fc932593e9d281c444889f1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AddressBook 05398c07e7ab6e5f4a9b09841142ebe09ab47ad78f34d230be0dac542da19820 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AddressCard ed59b26a3eb9c1b0802373188f51d78c4e40597413e53ed7f63572fafe939f00 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AlignCenter 471d105eb0257189e3217b8077f1bd37df151bdc4c34d7895a51f48df2f0eb53 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AlignJustify ffde9c363710355e750d968d8c8fe4b027ac95222449412990182b4a3aaf965d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AlignLeft 39df046a64b02558d72097f13a39bd77490227fa5dcae218d016350c89104172 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AlignRight e8f3fd521279b978d163360e98bd7045e63b9b61e16459738ce27a256ab5403c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Anchor 1dcc1ea4c2914f460c1cf0dc34d22b955d7d6c8012f246e1332d5aa15a7b328b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnchorCircleCheck d02e42eaa031e7c452148b60a7c4b0102a87f7cec3d9a0b0b1f0c23c22cb7fac -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnchorCircleExclamation 166f4c8655907621c1e36ed431cc03652e520a99cae27f44a15d187c6a491997 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnchorCircleXmark 18b46563dba41165040fd2dfa2cdafb10251e92a87afd4083f00544cf71d45b9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnchorLock 140baf14362f849e247b497857a455f6b428c8ee634c20637e02fd89264eb291 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AngleDown 8f689723de40f0409d28cc031cba72044324c5253bce2034f45f65cbca6ec922 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AngleLeft e11126354e78f2981b49ce19fd5fd44457a50a8fde87b766db18eac80d2f5506 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AngleRight 29c9ea7c1cd28c5f4c633fc0a0346f215e597c0ab942015901abaa05b14098e0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AngleUp 30bf57379589c02daebdc4a6a2a2423a84bd90356652e38a19d8f2e80a0547ea -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnglesDown a816e0ede528d78fbacb9b6ab7c776337797add687827600ff9a44976a96dccc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnglesLeft 2c73895629b9a6290b83cd872a1668dc07b3a66a7cca0ac2e5e8f0a2bbc4a017 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnglesRight 5a52d45994003db6eb18957793cb72b976d5f6bb906549a677335eb092b133c9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnglesUp 928f9319879e146002d27a1edb369cd893ac5a23cf238171a0b9b14925a2a5fe -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ankh 241256756bee659c493eaa4a8c8c85e9a1dae2f543254da63538b54a1abd2a24 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AppleWhole f29539cc96a774d6c62899eaa11aa9101c31b1a293326ade2bc1010ed7852a18 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Archway f20e259650dfd786ffeb8dfda8af35b4ce4004419dc541b0424992e0b8b80e7b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDown 45cb03282abe7f87fb72addc3a516f0e2c67289d04ef9de9905c3cb1cc87ada7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDown19 cabec380f5ed38c1f2cf17ef3a7fe2645c4649cbc71cafd97b6d7d2462ca664e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDown91 3bcac609fdaf3369d59672ce79bdb439a2200bafb5eda99ccdc9e964d913edd8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownAZ e0915bd7030f6f304ebedd5fe5130d16d196ac95467d221547ab7779b491d8c1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownLong 65d363afa5df47b74e2ac8dfea04e23aaec41c52db0ea56362a8876d0f8065a3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownShortWide 2a454c9a351a1a5c500d113cbdc33fc379f18cdbd2e2b5073d9209602eb068ee -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownUpAcrossLine a4c54e04050ca255c97a396fc1a7169584a81cd21c26673488a416801e225a28 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownUpLock 61fa0a97cb1185205d721c909143ce14e0e94f46853ed49bfccdd2b15908bef5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownWideShort 64f576ea1a543735e99c85005d739af424edf92dfb58f51df12543421becb31e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownZA 4c111424a2bb481c1861d3589519a57bd08266ba1c17f58c22b5fb5394d4210a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowLeft c03f5f2bc2c4e35f8331132f590e59d6760ab5ba07398dc23138c148d92a89f0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowLeftLong 0ea0550e497fb755eed6dff261e1b515b6dec7c7bf13bcd09ec56031766a0d0b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowPointer e27c945efd58b12846ed809b4fa2abeed1438ef5dcbdf611bbb91ede13476291 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRight 9d849dd0b536ee692601e70c993ac3ee429b2a1673765149f2bc635fdeaf0e4b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRightArrowLeft 74f0f73faa9390410a4e62cd56b41ef212fe474f5f479ec9b83823f55052347c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRightFromBracket 26ebf14bb0f7e3c2b4c34609c99ee335f94d2337675dd544e90061543660f20a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRightLong 9cd678e7a6723a3282576726df0a4cada5416dacab855ed0d34292be77cb69b8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRightToBracket cf2d11a8113884946a509a8e6047c0a101bfd26c6f580a38f12c5aa81df8353c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRightToCity 26c739fe73c9c5f80caa13ad47965195ad5cd9529d21833f66495563bea27718 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRotateLeft 73836b5b7c2649df38f8d1ead9e0fb39f5ef3b36627854ea5d73c839565657db -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRotateRight 2ac96ab8fe7bc6df94487f3a2e956518ed36e48dadbb8587918e6f8505251907 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowTrendDown 1af385b2348458ac010ebc4128207d2af850b255d695ab9711d9d6e121ce9cb5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowTrendUp 3b1e035fed51d894d8bc2ad3359a0c4c6f7d57b278770f86d254e25cc4b9800b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowTurnDown bc0ec3bf283257891b2a8d5ea381fe805958429788be5e46c968885f320383f1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowTurnUp e0510400954a70b8d167f102ce12997e93303a71e2225f3f274a95e455450fd6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUp 23d07672640387b246df41dd9749d0e8ba62452292bc8cfe89e75d3fce5f48df -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUp19 ac9e6020550653312f9f273dc48052159b58a6d3879db769cbf72737352c5dfe -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUp91 67205698a9f07541c76b5965d8eac474ae6d9747c68d929c2144568e461d3cdc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpAZ 21abd970ba4866bed91d7f95ed9c9947cbb0a2099b3c2c223ea6a8a25c6036e7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpFromBracket a80ea1cd6091aa5c4776193f7aad124792a49db8c0202b72fdd00f0ceb90098b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpFromGroundWater 9503dbd3ffd01a41fb4f9d50620f115a0a701b4311b0baa0caab5d00830a1aed -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpFromWaterPump 925472d582e932c60afc9edebbbd5640271067886dbef06e00a57dcb3522a043 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpLong 36ada97af55bfb8b7884ec0dc1534df0a2e60a19363ba0643491bcdd711b80cc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpRightDots 55bfd8e2e1f766dc77ded2489ea5e67bbd2d70687bf9e0df760aecabfaf9da47 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpRightFromSquare 3759de8d32d3dd377184f7a60268d06db3c48d93736a48b7641d4533ada77608 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpShortWide 222aa4a4e5ca810a1fbe0dec4e6f40bc2592f1cd6744030f3affbe3b2104feae -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpWideShort 2770931b4fd1153c9e3ddcbe2b5ed2e450a83cb33e99d173db5114896c863a00 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpZA f7b76cc82d0bcd305c25614d73986827f636f3c696eddc37cbdd36f55202c1ab -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsDownToLine b8d113b419518247ac04954492b93325dcce31bf936935ee76e427f13b7f3bf0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsDownToPeople 237aa57fe5b4736696a3b9481a119091bd4454cff19de495488bc244ea8d88f7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsLeftRight 9020015d36742c6c5e620f1499114a35bcf5542d347c1c7c3796fe03ddb2b408 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsLeftRightToLine 6c27e72013b67bcce5edb2a3a81e7ec3310746068e656c9683eeb2ad77534212 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsRotate d636fee49c27c21bfd9062795c9264d0bc07fe6e8f1edbb89b735f49546a53fc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsSpin 6f6a8a64a3165219492a7b16ae1190f2e3cf6201d665644a39acfa004780ac54 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsSplitUpAndLeft ecbeee62940a12d1f292da9927c9adf0d2dbe98ca042f20e697a5e971118cf4c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsToCircle 54da3cab47d06687d313fab85ee4925235737e53948ee5bc41200582e612b988 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsToDot be14f0b237286b7dce52d92a546fa4ec808f404d4f9da616c6d30eff2d1dde4f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsToEye a5a466e376082ecdbc204c8e3178014c275230e7fb9a9a24a82fd31e6ea6d8e7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsTurnRight d58bb162ebcda52aaa1f59fbeb1d767df022edc101609912c4999f1b092aa274 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsTurnToDots c139c0cfe8ed6ed841ee803d39e15670524375b25905db7f1e876ed8e6a11ce7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsUpDown e7f5acebe85ba74a8d6adf43adad9e83366f0066d4b756b02d78ba7166668731 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsUpDownLeftRight 79f171b9e6731ba3551093c067fea317216cc0bb7e5da5483ed44be892c4eaf6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsUpToLine f87289ead3687987b1bb5d0c905590ff2f70b5849315cd616b49a57c58fd3777 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Asterisk 8696e6aa39644394df6daacaa8c72762e72b523d8d1a23bb6340a385d49547ad -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.At 79747e4363208cddb4c97c9c2ab3a569a00c820e51e4b6eb5f66bef1f99236e2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Atom 3ece91b862c9fd129756db08632677190a10f464fa26991fa3769aa3215b4016 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AudioDescription adc171931ea3cc12b254ff2e3bb94aaa9c9301f0bbba8d65b47c9becbec63694 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.AustralSign 1410b76b1b9a2a245bd00d2bbea42bc001c3acaf855ea856616a59dca9da0788 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Award e4c1381b44a3586933f5c2d5881f8a58930b1b9ad7d1740af3180c03146faad4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.B b6de61e9313f9a5e4636a81b3fdbbda5e2c8c4ef626b1e54748cb2a19575173e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Baby 90eeaf822a11ae417c9780a4ab8b38280e3617dde31529b9306e0031fed51d81 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BabyCarriage 88a5fbf68961ba3c490c1fc8e8312decd18822b288e6244c82e7c608da5daec5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Backward b49e62755c9bde1676ef1ae63f05948c4c22a47483e44900430354c6444fd9a6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BackwardFast f85cf9cde6be35d99d064211d6c98dfbea4dc265e9d32ade6955619483e9def8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BackwardStep b7140a53676ad4e7350e4f495feb377eef3ae2c22eaae9a4ba5467b29f806cdc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bacon c84b997e5802722f3aebb31f1e4eca1672996a76ea3b7a6fcae54aee5adc7e9f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bacteria 5acc6415ade2fdb71f61db6ffa96bcf55429d5e249bd16016728c2244ecc5a24 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bacterium a470aac0842c69e3d0fff25a91fb46d8d571084c865487629ce9161733597d69 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BagShopping fb4a0df4a3e752de04476ea5dfbf046a56de9d26ebfb1839b36f1047e0397505 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bahai ec3532a3f24076a9f9693d29286855b2abb789f23465c2e4ece3a6a62eecccdb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BahtSign f541070acc295f6bbc858a2a38c12809ac721d8ce70f420a376d173969e4194a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ban 1eb894775ac073345ab8e24bfbc4cad271f879260db50c6b4b823e7a23d8106d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BanSmoking f47a791da220d4b5adebfdcefc7013a807aac3e303f1a4db2ad1896019020fe8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bandage 775d5b41e27ef5de3b592645ff0378068255d20a51350f112f58c350f424e38e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BangladeshiTakaSign 43d2cee14309cee412a0e44893cf0baeb9958eec9f197d0701d7904dee313bf9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Barcode f7b213d11578cb3198e800a50c06908056489886654d91a16069bb796d327609 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bars 14a03ca624663accef9c1f4b94b8f5d222e55c40d5564902172b00b444748169 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BarsProgress d693a6cde740b062b611b91568c223ecc285a0562607520f791c4d4e2638bf67 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BarsStaggered 56176b013e0c85787628f600bf3ded86233aaf539748cf2f021690667b05f96a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Baseball 111ad6a908869eb1e6015a4c8f3d5fa020f129553f4d08f30428cc4c5d4ce636 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BaseballBatBall c8d665f09aea01d2035b4e27144c245b9ec4e3d1103e14ddf6e7784a4f5ac6ab -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BasketShopping d4e7ecba68a9801e57448c7a0856ab6b5a291c2f149326fdb5d26c8ad835bd4b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Basketball 1549e3ae2e8298a58da7ce3e73c23bae733368d639f1d44cd68c7d296a51bd00 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bath 79c741a9ca812fe86adcf5b01a9a2d6a49a8600bd868f924bf3cd771c691e23a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BatteryEmpty fb0e5925ce4e59150701463de12c3983d9ca872999017863d3392967a172ed21 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BatteryFull 129c6fff83a9d3e2add2676cd18253e09c74b238e2904270041b19b632b45435 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BatteryHalf b81cbcd11991d2fd8ab6b34c879a0ee3f0e253825620b81f03ca0ffb615c3063 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BatteryQuarter 9733692287426cc787f26503186639ea7d19af86c7e541c47a0a91df2d3f283f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BatteryThreeQuarters 8602257bb8b09dd4df92b01fdcce2fc78c97acc3074b4716bfcb1adf7d431adc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bed 9c89539aa0015ce8171ed9649a10bf051863a38f14dc126dabee86dd10734ea1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BedPulse 71715fc5e1972f5ebf98a2f8afa4d89e612cbefe639eada2c4b4e8de9f425a37 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BeerMugEmpty cb13bb4a3c84861fe51e9db685abb6e67abe17a72b7c1ed8a79e209a3b458f64 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bell 39a8fab4ef9cfe5abe5233defd37b4f81f4dc93425b3cfaf1fa4415a6e2521f0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BellConcierge 9923c802631c1e3352665aa1784bfa9f31fd437bf611dab5b17d1fc673b27ae3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BellSlash be86c3df7f001c6d8a49a70ad4ac58f689c6b516b49370e77a0050ee7a8c48ef -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BezierCurve c0428f6b0df3dbc8a3f755956875db29a2d6156f1b9cbd724f04fad4935c8c69 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bicycle bf62bf03c80c9fd968de189063855e7e28a46dfacf174c2776ee8064c1215980 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Binoculars b616abee754b918ecc31e60a0bcea4e38b495220ca293b600c42bffe0d6edd00 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Biohazard 3c4028af4b4a67efc8af0c3296cc39a478c8aef5b94f012e8612d9c3d05805a8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BitcoinSign 41ea700a2f87c03df598879183e9a98ae70e48b0c8eef9a8cd199bd44a96736e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Blender 6ab7cdffc748a93284ce6182053ab0eda2c11c8452c3effd4b75792eee241a81 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BlenderPhone a7acbb68c9bc494c3356bc35fd7fb13d198fc1457897ef8ae0c053776729a03c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Blog 428dcee973e8131c43443aa782f6f02d978cba311302420e7fb49bfa2bf15e03 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bold 651607a33013bec63fac6504fae1aeac24f2c80df38b06060af5dedbe8d6abbb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bolt 0262a35c0c4210ab3c6478c140f8a39a03d3e33dfeff71836245dd6f3c096e4a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoltLightning 2bbd710f650086cd7ede0bab95923011c17ca15cca00385f3082b27c594fd673 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bomb a3b0501293360f2bd26c93a05f008e1e7e53ba8bc63829ac30e5db104a6eff73 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bone 48d5a17b72a6a3afb8b330d3417f7667e8ee7a2d110bb47ed83a5a537ad48c11 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bong 8fcdae89ba5b8f5b2c3d5117f08ae776503d9f534abf4731462a61c4db94c337 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Book 9a602241d996496afeabc9922cf180b7fb0f0f7cada08b447fd4cfda459b2322 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookAtlas 53abd20959c11e02c7a387f30b7075d68a50881fe5a2bd53d4a86e264f93ee94 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookBible 88a6de3e869c9d0cd7aeb016500a96ebb10cca9bcae2870d3bdfecf95109971b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookBookmark bac15216be02527fd0cd635252a148db8d4671f13d2a1e0e45f660f5e8918203 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookJournalWhills 196189f33b63d036f874e3736d15f9721a34b2c092e31bc99c674767d646e858 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookMedical 3e2c8e5243c007665989c561f2f457c208311e40e4e98be2d3752704b138e7b4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookOpen 489db1afa020983e9923f8a691c95414605f950070d42472286a0203fcc6084c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookOpenReader 0a2d8cfdf0f02cd34f391002cf4869e3c7d16256332becfc83c2b0767727f304 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookQuran 7c604f11beee3ba210780c9d5f3c16d009aac1da1ca6c8b6823ed7b073ef7d0d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookSkull a4b6c8cefd8cb34a318478499aeb8d257edc91a0148eb4ab05e5bfcac1c56625 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookTanakh 2ce3ad8b587193d3f9565fd9330b5351b89accd608566d33781391fa16d96a16 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bookmark 88618881db836bbc8d24ca946759454fd9e162f03d3443214cb6e471148dbcbc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BorderAll 956490801a5c6dbf45d31df3b00df58de4d870c0f7670a68ba860ea0ea6ec002 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BorderNone e268591d9d60b726030167198b4eb01d90d76ce1d2ca3dbb5c84f075e17f608d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BorderTopLeft 3ba19bacc54cc2a73bb69bba8f759e7daa487954db265cbb09852b44f20fa2f1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoreHole e8c4db31652963c42eff4997fc51e79825522f56c5fd71e1bab53a2ea0bcaa98 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BottleDroplet e3d6e1d7cc06fd4b005be6f72d9e9d0b03320e81778f74cbbbe9023dc5294ff1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BottleWater 36305c33ef4bcfbebae585a7d4e9f387ce58e65050984d12acada7d056fa5a3d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BowlFood f955eb4b65e7f6f9bd435bc88b71449865500aee819e46cb56e113c8c8cbacd9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BowlRice a7b596792f9891efcd7b6d993e73de424bffeed8e890194b39c757bfdb0c0e17 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BowlingBall 6e0c6c58c48ba1e3a6400b22df258383358c54ba8a26a67077c738a74afb0245 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Box 216a38898f0ff71e614fdd425faf2658686266ef3b0bea8beb11daa05eb56f2c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoxArchive 84364139dffac470e5576a76620ea036b7668c01b2ea5fbb8c897e993f1eff19 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoxOpen a39d63f9f2fbce04c2cc9d2644ba170055e733df5cbc6ff0564a2c4f8af112bc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoxTissue bcc1d9860380781226133799eda59452d95c3ccdad433c220b8928529378f8b2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoxesPacking 0f1080b67dcebbe928ef8c8959c4388b952c004562cd99292dadff3156bacace -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoxesStacked cc6ffb87bed3693d41c429e7d4c3e27a81eefc43f3a1f9fa33747fc3fa2e7dc9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Braille 672a2e94c33aa6d31664965b8859386a834e873cbfcab49164d9dfc5563a6b99 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Brain 9b94fb1733ef5aa136f79478026983f9db4da7f3ae0cf6b19a068c11d0a2af00 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BrazilianRealSign db81b51176545436712ebbf5b4acc787abfe51ef50d484c0a72330e6ad599996 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BreadSlice 5c81ba80775dd3007886ea8872855f55d28a1283088c21e0d9854f623c7371af -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bridge 231e45f88f1e2a29426f73ce2f47768983df7b3d96ee84fc6c9155d93a1ec55f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BridgeCircleCheck 052b44bd7116742cd24cc5c6e4d5f41a2cb6c15efadd52cb50d75bc4ae374a6b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BridgeCircleExclamation 5ca71c442c3fba2a18e55b725db0d12e0e8ccda597abd7f98a6cbc090c70b74e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BridgeCircleXmark 8f47d6c1915bff2dbc6b92ff928610962683d13e840752c6aef6ab363f33c53c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BridgeLock a29aed1182c82dea40d2fd932c8e6f27a7c9f70f90a0614f5d95a8f4ba219633 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BridgeWater f1d963de49a7fb07d3a945351bc18d61d26365875c91282259b6b416a068f599 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Briefcase 34e0e2bdac3317e9c6ea8c9b4101a80ba8927571f6eb5d8b04f530be92e23667 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BriefcaseMedical 82d9fb4051b6491e3e2442bbea12f65a713f28bfdc15f2037cc3c04f83b8d47e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Broom 9e398ae18de616aa60568ac592ca1f8650a92e5209e03d821d9a524ba8945d4c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BroomBall 0f24fd80c18a0f221c70b3bd71ab72dc4b68f9790528b7c84e1bf71a36343561 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Brush 1939389dc16a5df085bd45dd156beadb1a63cc2bfef2683dc6426550f983a76c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bucket fef8bf9c56cede3caaf7e85deb56138c8f9108043493b53a00f226464547a8fb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bug 38c39c85b72d48d4dbe539342b9ec63ac9509ea78e29ae82068a04ab3d4d4daf -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BugSlash e4225f48b7863f1dcc354fc1882d8a125aebf75be85b01e65391fe3a74e0239b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bugs 2fd9f553c5a6b51ce859be1d861263b5255c33433442cfbacc932283e8c7327e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Building 17a5ccbef9ebd1ed596c8a720e8b7e6c7eca3e593b6f3b2e43a8384f10544b17 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingCircleArrowRight e35ecd46bccf07678f44f3465b9e795860dc1d0e116a5dd1c23dc14d977f6c5e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingCircleCheck b5dd98ca2ea10d634fc0a5be73aee5a5c9657d90aa02b4f186fb936aa7041158 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingCircleExclamation 880d0d399d54522c0e71a6c84d6769aa036696590d7b5e2cc2e979e878f06dc6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingCircleXmark dd4d34aae2eba32f2e565684fdb5d83453ef1ac64922f1babdb52b367ac01762 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingColumns 788102fa8fa6f142f2da0d32dfc9ce37ae4e25d0374f72d66a82e39f81c9f560 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingFlag b1f05abb98c9f482411a7dfa0c888f14541fd671472f7d16b820cb138d0c1e60 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingLock acbb0d163b44cb9f7941ba8439aa10bf03bf23c5ac31e6c7ed9d0bdc45dc9e45 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingNgo 05fd80aafd6de1a0c22fc2e199cc7b8c8112e449181f86573c9b506d4771706d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingShield 34ea4f0e41a292da8e105c20530bfa390c13133fa36948d3685731e53ecaa5a7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingUn 8f4fd94877f6f12dc47220c73840cbbcfaaae69141590f57354440f5eec5319f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingUser 535288076389c3d34654c842a9a03386eeac8daba4625f71f6660872f80c3bdd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingWheat 3791b29ba00e14a2db5c6e5786bb358b42030454ea866c4b61ba32cfa57ec970 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bullhorn dc7db607e656ade9aa7531566db3d44eab5d226f99778bf5e19b2d1c0728db0b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bullseye 2ee95c6ee5f96d15987b68e6aa4cc887453c634602756b1886d58950bd889a13 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Burger adf8c7661ede4c2247b965f81a87fe16f587f922a9e0e258a4c46af152806a4e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Burst 91c0eb8875cc7c24d0b9e337686f82c4bec8abf59c73f761cb281aee011d00c3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bus 40c7b14fa5d6b28136fb376fa8c06ca61142abe793e77b5d83963c3126531d02 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BusSimple 51a0da52cb4b31de815f2621413f57ec3b7392abfde490e7cb0da3b9fd0fb96c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.BusinessTime 7add5b3c4249553941d413386541e1ff864926156468081d8816a0b33341f055 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.C a42b98d41184c8b000ddc8b9854da5eab5fe5139ac2dddde674a6a3b1f9d1f7d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CableCar 11c64c920824e52fc7d4cc38e1aa8879ba3bbad1b93c07eb6d99b9d03f6a9f61 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CakeCandles 8e4747037de1e6ede1efd7e03db9b14286fdd6f1fa56b754a0aa6f176ec14b62 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Calculator cdb959570572b4bd095b4d2fd7438894e1657a76cdd0d25857e4c8993f7718ec -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Calendar d1fc2dd1ed4020ab4ed5f5519afb2661647629507425ecf49a7398fe87b4adad -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarCheck a885b3c89c1c924f39437580a79c24fce2757af641ef35b4aec6b1be8e59c93c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarDay 85d6df5a0609dd6fcb7e02827197518d777333b504cacb3563b3081b4fb4c7d7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarDays 960fb1062cd6b18cf2ddfd455aecaecd58e277d7e3a9a3ec797f554b7a092d3f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarMinus 60eaca8a77264a391ae3a640eb058417f4f6a569fb1e364ba1ff25f347c525dc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarPlus a4c1e3bd9fe7f568a6f4bade11dfddabcc964096640e266f98b9f9ef1c86be0a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarWeek 824918b760aa28a1fc29c6abc272b3c0867f357caf4d70316b49c671d0c10d7d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarXmark 65cdc30e02a94d2646cab8b84908c27f98eee54e32e89396a2c589e6606c4e86 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Camera a29764508e40ad2697abd2500d1275218af220db38c910c6a5eda8b06ab926db -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CameraRetro 2ce245ea5646da850e9c66d6a9b1001c08f20654d3c68483949ae6d9cab89b48 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CameraRotate ee424db5e72ca1874c92d25dffc533ee7f8657465a3250050dc0af21ac504a86 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Campground e4f1fd17b46593ecbb4f19dde3988f2e8cfcba7b8330a02de4bc5cdb1a731ae7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CandyCane f3ed39458958b77c4285afa295ba0d1a4c6c7dedddf958b3339355a22c5c653d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cannabis 823744177c5cecb4d0a98c694a37458d86cf2c96487ba85fe93c0b91e5e44fb9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Capsules 6e707a375387616ec154664af17ce137268e9c1d62885a2d68aeea24bf647300 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Car de15270eb22bde53551403b1d206e4a74d6fe4bd281ec97eac5e17671633675f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CarBattery 0d1040f8c874678d942835c14213808f551097bde47735b3a0e0a1b8889ee396 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CarBurst 01ac57305cc3847f2a5405949f9b88c4f2c68c1011d6127d392dcadd17596863 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CarOn db85bcabd3e6f343ebe12bcd7bcd4c7a41d3a23ce1a181fa922d83769a6d38c8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CarRear 7bf5142d0f88d5ba00b950d059f25d091cf405fc4ab689b2ae6928b2b572223d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CarSide a18c94c1da1858f8b5b5e45019fbc08cd264dee0dba053cefd4c1b6a6ae7b204 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CarTunnel 534cae3519e066f1bb67749c72697c51d02f9ce739b2bb6dbedd10103fd91732 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Caravan af6913ada9b1ff9164f59e94b782777b518f532307abcee86aea314fdf6f3042 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CaretDown 96617f3d60915ecbb76dce07803b3c174822a9757621fe44af8f0a938a8b5411 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CaretLeft c94374eeb15e85dc4f1d98424c28c6def1de2a458d49a3027ad1164b5dc748b4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CaretRight 52ab8d95a810e8c345b3c26be1bc3400d3a3df10d4266ba2d29acee0a8220f00 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CaretUp 91eb51ca6d142035e4f7ab4a1edf4b450d48793fc67793502ee3ce46553889af -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Carrot 3a748eaaf592dd0c93853d5f6d11016ff97707492fd649a7c1cc4d770d514341 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CartArrowDown 84e90c264e05268f5c87d34f92573e1e316f4fccb08a25ac5e8009012fd03e25 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CartFlatbed db124bf82298523f679ef659b5e2684e48268ceaf700b14444ea13bd89dd908e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CartFlatbedSuitcase d0f439d734f4ff8a67d27969882de87e16603e8eb6e08ace7da6d66ae719043c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CartPlus f293600546ab1d098f1c4266b77c45ec5e740d8c38ad5df4b3aa89241623facb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CartShopping 8bcd87812f15fd02ab9ed0ba1ccc0dd55e298f1dafeea055cdb25e93309b67e7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CashRegister 0f17b3e7019f1ff042a8f54db35661b3890a27547756f2326e38b63289e01f56 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cat d638555c13e92fd79f630abd5cd69864045cb556bff3d7648436b23424d8ce64 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CediSign d9596385209384795ebdf73ac31168f7208a2afcbbceefdfe16d15cc0bc3ab8f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CentSign adce804c215d2589512c4c9453bb04a6de3220bf1998cfe009a09827615a8c0f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Certificate 73d6e50363000d82401b65c57b3021ebf064e897c318a7b7aa1238aeb0f20fe4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Chair 4897775cdbddbd8af67eca683f02d52b60911539913985f7c062c9d1f9eb5b57 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Chalkboard 3be04613d78affac9f74f0d7c8e61497d541b52311e3cac8044fb712a83bd741 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChalkboardUser 541e834b5312920b63b63e63b95d2ef4a95f0627d420b19ba8ced86413dd8502 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChampagneGlasses ca0063fd2456c0c51e5f3294caa6b3d5cc62166355e4bbde9d092fe17bca4e91 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChargingStation 4607f45f32df832110fbf46634282d22229cc75ac5ebac67a9a3505fdc4d4c19 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartArea 7b73dfbb4e6859d85359a79ad152e3450342213f2b3ec028ec422ce3c5f3a690 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartBar 06f26573febcf8184c1b54331027a77d7f5e6269825e0e854940604273c7675c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartColumn 3cd4259c99a6c7dd898836d54ddba75155efb33e9fd6de000e22d0c9a3f7fe3d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartDiagram e96171d768209210e78d3a7f2d36a0e201d2588529783bd86587c66d867b7f93 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartGantt a62cbedcc40f63b7a46890810735db185b457b957bb1b2b2bf53289437769c1b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartLine accabf59824566a361d50da15ca33c3cc913529902de255cae6bf933a28d4aa1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartPie 0383c683356c32198a098c7b389e3e36c185c7115dbe85b2f3876ac83367542a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartSimple 98ac1c27c2212d9589aa48c50dcff736694ab0808194727168b5c2c8e11411eb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Check 56e8bed95ac2b65656229bf81512febd814158d1f8facc73e98b7025924879b6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CheckDouble 0220def42522af90e48fc23bbd53e204379154f10ed1364cdc03af3f2c8b26e3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CheckToSlot a5bdfddf23e8e6031de8acff28947b5345c29bcc08744d1bcf215f1f78bb5e3d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cheese 3b7b1cc773c98d598fb3fa3eb7518e63aac4effcc07e5276593573c59fcf45b7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Chess 0f67dfcd91b5fbd13f90dd39a0442d6918bc1148f328fa76a459f1c2d8be05e2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessBishop cf5bcb0871b8590ab9980df3fdffe652b0e522b7160cf369536a16af8f1cc31d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessBoard cd519499a401c97ab205c74b70690b4569ff4c4e2752537a1fef1392ca7cf4a0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessKing 38a44e092965a1df1d3909e43d0f616e0825cbba0b3b319b50b9ecf9a4ce976e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessKnight 849434f266a477db36a221c6c469fa1e51ef32269bfdc506aa1ce34a1b8a3502 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessPawn 004e99040f760d61f4221b38af566a3a1e2b4e70148b011b79676c0c8b6d5eb7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessQueen c5035f7cf674ebfeb50aa53dbf93eebbe3c861e8d8b85162b0a97a83d2dbf358 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessRook ede61cd8a91c39197e930b3eb4b4011e2193e7854557b47892c5906faea5ce29 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChevronDown 4994733064e7e4c7bb2912fb3bb7c24579cf3bf06d49c0ea6be453fe2c1d3dd0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChevronLeft 7ffde30795a4ba00154784ecea0b2b34146d4bfe050b212f27bcd323d787ea45 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChevronRight 06c5069bc7669795c83b1feaa23f34f77a399107d9611f3d3803de42efb3503d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChevronUp becc19b9d5fd778996065107d402d4e48f31212dc46c6fcbc7fb8abfb768cb5c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Child 809cd543bee49514fc9e1900bfdc3795321f8aeb60c16d809d5918fe627e0efc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChildCombatant f329a7baacb3ed3ac630b9608731bebca07c753c8a5e8f5ea6676cb6a8f423ee -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChildDress 1b25bc0de141b14a628b685b4a33d7a171b238f90a56baa6b066e944e698f55b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChildReaching e023111c6b1b3b5d36f940c72c457e67c22e4d3fa61e389000e663bf5ea81bd1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Children 328cf6f83cd3b721bab051270765665f7089f6b54ca11c10f1d48109915b9be6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Church 73a7757be0b034cec2be0f7761d61bdf93037067c78f0e1bc42ca3a749153c63 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Circle 6f1100cc6cd34e2b498d6b379337c379e67e0974e888bfcf5df5c06113f9886d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleArrowDown d840d203d071e20304dfaaaa8c547559932ef54f36579bdbf7c267ba59ec41b8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleArrowLeft 5777ad00344788281fd19d59b11b1a611f55d99d72a0dd7342853bdaedbf2c25 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleArrowRight 32fc406ddec6bd134bc4bf29d04e8ee8a28cad2ff0dd3f50f41cfd8c7764c3b1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleArrowUp 104eed8da8bc77b7843c4fe3b77e02269a99ac8c089f3a9f6824e1d1f7d41a32 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleCheck fc0ba1b3dbe5b2f5e6b4b7ca1361dd6cfb620daf341e1dfc6e7d3481daa589b4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleChevronDown 6c0fc9c132ff7f1be3a01d1de7e8d66e626b37e5a6ce0551552621cb28a46f67 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleChevronLeft 99f8328d44ad4629ed80e44339cbe5146590c998e39e813fee5285ee24d4b225 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleChevronRight 8f70c10b89be5c26207d983a98affd76de933564f2e8153bc56e3a95cb324d5a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleChevronUp 1967c9eac80f332210243b19379f4b0d6c43b61db8ac96823c8e85d6b32af9f6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleDollarToSlot cbed1c736ac9bfc24d27805595c83e029a941dbaf330544957c73b2d32d0b303 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleDot b6208bd903414df6ed6b4dd88f009d3a780527da18e8429b6501bd0fa4b3095b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleDown fee584b9d2c2d067780a8b0b6ff26e8b1f3b64cbf8a5ea8ee6a960c10461c777 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleExclamation dd7bd2c6d8d1bfaf8b261014ccfeab63764a11e5b6983b9a6ecc3d06f07fcca6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleH b30758b001283fb3b297b7fa726c6ca1b24f69126104370065c5b824b74b7365 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleHalfStroke 5e63bb6201c9f029cfe3083ea1bab08b6dc4d9494b9920e6e9ce27a2a5bfcee7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleInfo 9bda24e5820b2eda98d413c6dc0461c5031711fb380920d4af5f3a352242bb30 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleLeft 9ce0c6140c01c1816b59093214273c6afb9991d143eb82695acdb1dda8f4d519 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleMinus 1d54297e749a4a307a423acc99c3a4bf96adac91a85fe158cdfad1d67f712895 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleNodes 2fc3a61d12fe557845d4a054730987b9204dff097ae392a4e634f67145c888bf -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleNotch 5ed5c36beded1ae9d719a35734eeaac341bfff0fe97839c7c84c4103511da746 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CirclePause a8e103a665501e5620995bf5eb18068fcf0167fa8eeb1575d60c9b341106a52d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CirclePlay 7a6f83fde5ea81e35a673d15887d446d200fbafd9bf8a82b33093b7b510b37ac -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CirclePlus 7aa129795344bdd25d57a02e2697cddeb02e10485c4b05996b6e2d106f0cb1f6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleQuestion b996d4c7cc970061de2d8743b22c035f665ef8a1d8d59d1c9ea98900fefb26bf -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleRadiation d081dc657397f2453997bc6f9a415fa49f138cf677a9f178389bd107d664848b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleRight 2014d588d85ffd987c559bd3ad0abd7f764820fe016861066f212d5cf483268a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleStop f16200b5fa9bf6ed9e3e759cd5fb484567cb917b333965a1e685f913a18e8a5e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleUp abfe2f5e8e6770d30c7420aea1513ac87175bea318ed4b8b86ba1f58c595ff50 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleUser d7dc64363f571e0b33f64f29e712da8dc5c4204d27f2a72553cc451cc5c3b034 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleXmark 1bbce2a99998e6cf7a87a5035f48576db601de3f72b0fec544b545f54a066c1a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.City 28492291f48f1a392e0d37ff258b361130b5245298ea6b3e5e5aa1b2dc85ff9d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Clapperboard 6fa90b5a5ce10a5836f9d1e9c9617b8720d66154da05044eda77db7617d4c409 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Clipboard b18180f26be7e2287cee2a9571d12c251ba946f57375497fa1f466c6ab4e4660 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ClipboardCheck 67c2e0223a9bc1afde0e40e8e102463e71dcabbed77781ef23f9137ccdca7069 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ClipboardList 56385e769bfd68e21f0f00a52ab3fb326ed9fb0f3b38ef702c7d4ea07d247847 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ClipboardQuestion 0419015f2b6b219ad635e31ad895d1a87c36e8af52b427127da24c08a0997d30 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ClipboardUser fd27f80eed6409c4632f605fad3d873bcdc1f029d9257e38450267dd49f450fa -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Clock e4942a716813efe499f1aeaf29f480846686077d29c9374876d75f10c3158760 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ClockRotateLeft cb80db9e33707a9f5788a672ed9a8ba6451575d07aa20d748fb524a4aafa30a3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Clone c4e47f6b9e638fe1f13d5784d71873eebe32b52719f419e0a58ceae5fd7ed392 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ClosedCaptioning 8cc33543a96eabed15969928ef8f8a6d2123dd8e0ef43f347f599627b9ec68a2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cloud 83438420c526f53e2ef45d39c3b29d728abd224fbef23df6058f232005ea2025 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudArrowDown 17087a4fdd4d7df2702475f0d4e4fafe141e5863684037ec9dbb2f374f1138ea -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudArrowUp ccbcd03822f36410eff1a7bfe1a181ad58803f8eed232df6c3178f7e6904b321 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudBolt 34cd8c1745de4c1dd59e5bd584a1ab0b4ae9572523d7ad0f726d5a7fbe95334b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudMeatball 092757eac8bea2e30cef6e672f3d263bb8c68b6792ea5a1db272b6c90966d8d1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudMoon 0218f470e98f0b0a6d4a2a74704d89c26f7759f988ae67f683e5d3d3668200aa -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudMoonRain 26a28c7951dd439720665e7662e0149596645aee0e051dca2b93241b3d14c4e3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudRain 994b71f57fd15cf87ff2017a86f911378e27c3711f25e292226e9962d302f264 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudShowersHeavy 84a585da2ec856bb740870f90554973aecbf6131be9a63f7cc4a73bb01f62891 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudShowersWater 2475596e9f4c87691785014b23cb90ddc2586875f78aee8d77b94b236563f159 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudSun c54b1954c819b16f649201d28aa849eb6fd22e58ef92f17f37444aa6a1a22314 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudSunRain 3e0bff2ec6286391c0250581d35355b3ba0a8e6797fc72952a2c70240755b063 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Clover 5dc8117352510c38c6928603869d77b145e252d88d0c05a391259062aa546aa9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Code 4fb5231adfdee91a2a2808801a82ff3e1f745c1e06350f2315fa9cf3c9444888 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CodeBranch 58c9783b8953a9546395f6feb25c237e8c6612c94c7630b5ef4239de75942cb3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CodeCommit fa15c07f0fe1f28222b0d83f134cee766547dcfff013c9fdc51c95c3d03909e8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CodeCompare c8382f6f3e5a2a253bf36cf5481104474906db0b5fcd009a0c7c57d527cc1fe9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CodeFork 9dd707fc6a32b9038b38881a8a22b1b7a894d2a8be7b61b2d50604a15f6ba5a0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CodeMerge 75d27875571b9bb4416334c1b0587e33b726c5e7d197bfc82f2dc66b9cec3c7a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CodePullRequest a3eb06fa1e310935516fa03c8ede4cb09ff58e1d2010fde0650bfb9097d4b266 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Coins 3b97f7fa1ffdee80d589d1c91bbb6c8f854d97609bb57443ad8ea23f26971a5f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ColonSign fee889520941bb50c537a7ce79366ef6c1f72dd16777fc9419247788c67ca3a3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Comment 1237f11c681f43dd3324ca38b5c75416f58c863b9419f0c4424d84e5c796dc03 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentDollar 9085a86b3d4fc1d90b4a459f4e8ff181f880b64c2453b69f8b7bbbe43afa1a28 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentDots 9cb1fe955379d9d579af7baaa9cc4dded7fcac221936dc6509af388ca2a8b859 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentMedical 86c23b44e14997932ead0b2170fd9b3b2ae648979510a0b583aa5131d140727d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentNodes c8205b20b37f82966a41719b1f13a9e0b73ade23adbd4a9674383593365ffe02 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentSlash d094450dc3d5c14023008d884d28295dc3a2f0fc551d5ff6172e8d910b53bf90 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentSms c2861a766588f9e8ed1320472ce4b5ebc8cec86af38cb7b59808e7845323b1a9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Comments cb4fd4b85616fefb94b866c9921ac1fc78934cfd217209ded0208eb15a9f9b83 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentsDollar f4e8dfee58aa9746b10b1221a9ed7a67bf182eb9000aead351ae8a36aa5aa41e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CompactDisc 20260f1eed0b3cc20a365f8dd77f0549bdbadfc3c6b2e7101c8f9aecb66b9932 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Compass 66ec75fd220d1163229db9fadaf75d5d6e6313112e3214ee5bde9de9e79e563c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CompassDrafting 4414c5ac7535fa2f3f825ea02b3bdb40ec6a9af0ac3bfef4c76cb0c02183e054 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Compress 947e6389e0e59834e053e7910245b6f44f08e281625c120f4c9c58da59fc8467 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Computer 435ce9bcaaf31e6390ef501e13a0459b581c45589c6a758cdf406fd7eaa54aca -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ComputerMouse bb955fb31464b3193254ac2427082c5ac1f5e702539d346e04cb49ff16be227e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cookie 9eecf1cdd101ae6c8e62434f6cd4eefc2b9a69717b27c631f7ec7cda7003aea8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CookieBite c88375a718d60acc51e92406827d69470f61ecf96caef92afcf623ef180ba590 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Copy 1fa31246de781e901d329156c51ab0001f1d35a81d31a41be2850183c5d7548b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Copyright 652bcef07e393d1ff02d3a3b06b5bae8c89c73e1f8afd700af8e8f2d62e5bea3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Couch 8a894f52e2f6eb690b017391116d3b286363faff1372593fa800ff78c409c70b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cow f03d69bba98d66bbe49b3cd2c206b0b4ffa416c2c313416a60e11beef7f91174 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CreditCard d1f40b158d61307c363ef5dad015fc2ed5d3bb425ebfacbafacba40a31f2c6a0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Crop 079963acdda251541e8e9cfb517cf89cd6ff9d1f386b126dab137cd6b72786c2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CropSimple 391f3294e367169bb85c71b817bdd495eff73a0ecf224c788f4e45f05c9fac00 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cross 37c0947e5e8bbee660029ade19930efb6351801f3a493a7c65a58a563adecebb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Crosshairs e9793a086e6a93baf9aa98ef54df64bc42f01575a60d16b5dc077d59cad698f6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Crow b5b8390ed0aae230164cf3f7b0c3b707f87503b77d0afcedfb09a1e47013f718 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Crown cb24fa63942bf287bcc954ad2f206d0c4931847359734627ba72c78540dbae7f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Crutch 3ca49bf66c8cd0f9ffcf72b6bdf4fae9abdc0542d8b096cf0d196e7cffb6fa27 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CruzeiroSign df8b48188b0ae31473f2efc914dc5f031159428f40a577ddae54b4158c3cd1b9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cube 5146412f9674f6cf3facab4378a8e51d503bed5156d7c2b999868614768e21ba -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cubes 00888892767f9211c9edeb173686cae1d45274765dfc69c2cea37a11a6c709e3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.CubesStacked 4e9cd42302b34a696bd0ea0f624b75b0d887b10b9422e29a2289cf017b71ed3d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.D d2ee466a4b498bf50a6840d7fbfd1a1d8db6365b32f4bdd232cbb9027adf4d94 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Database 6b910b4b6449d3811af0df0b383a8f0c11258d0fa80c13339e4a6ed133e253a0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DeleteLeft 82232d6880b89e32b08c1e89ac29a9dd7d776eb725be469829696296189e6151 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Democrat 2d6f11ef05d9f1fd2a56e01c6314fedcc73350aeef589d0c317c26513cbe558b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Desktop a07b4fd40aa97861f0d612d10de2c2c9a2df6280096f08824b50b18c5f423795 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dharmachakra 831abc7eb8d23ac2bc6835df5b85fe9da590b686e242412bc1b664c30e7393e4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiagramNext f711280e7637befec178e18ae91e2396ab25eaba4305295381a3dcbcdd372f48 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiagramPredecessor 092c23891abb0c9bdf344cd6b0e5086a56acd53fa6db64c0e388d5210f20eb94 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiagramProject f5acd3a8edc98039d47901657166a50ebf83355583e3abfb8c5b8e7b2512c804 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiagramSuccessor 405eea39d55bf17651dea424e7f2a8dee8734973a46f843b4d45790f10858899 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Diamond 0a74a7fc313809ecb9298f128d11a5900b7206e0f49a6679210112f6a521729f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiamondTurnRight e1fe42dbef7e48f878c4356a7effc9ee4811307dda9910122dabd075da21186b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dice 90a6ca1931dab885fdc5ed37874da421a960ea7d2b80d871b14e5af51ba23af1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceD20 5b19d47b746bccf5c58d2c27d8d34bb58bd51b43d895a322b97c22c8d0b16c03 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceD6 2fe9e01550a7cad91c62b2117a6fd76256820bfa4842426319e5e548d2969386 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceFive 1d1aa674c8c7a746b858a54b89fa683e77fe513cdd7ad53aad1f46fcb5014885 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceFour f47c3fe3a013b66a322d94a97b32c80dd9cdbde6150bd31fdf8368f345102c0d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceOne 97e1c13c3583ac1b25d9f338e60095463a1c2a0330179cf960475c503f146a99 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceSix 7f89b12ee0cf2e726c26a798566fa687c7485cf0ccacfdc119a0109eb18fd9b5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceThree e067e908688f809b38b840e54e3fe844673bdcc173190ec3d11b26f990f7ca94 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceTwo 9127cb74dd57cf0e8beab312c434330c3ab34e44bd2e029a0c5c66eea783482f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Disease cdd63a8c0ced0925a9fb3e2a398ba9d1a2e81158b689967dd4fb20348907a6fb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Display c0a0b1cc2a615e743ad2f297411b8b7e3daf7236b485eb097eb5e794bfb3bc41 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Divide 9ed7f051b2ad37648785b0f78bd1a86a51344c7aaf862b6e0e74d9aecbd3a552 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dna e5e41637886ddcd51f05ddab9ddc17ac4bf21c7ca5e0b2f0a34ceb73405f63ea -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dog 4a5e3cd1ef2173d90c63bbdcc23aed77456efb03a0f3cad0cb34f36d997aee2e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DollarSign 16eb7d65d397188812314bf21f63da3bd113f81e065fe8ce9a9f2745c5fbace6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dolly 769ff9fbc108f415af202cba4f594b44c2bcff7af15cc38a3339d0366231c15e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DongSign 4f3a7d4afc1db46fde2a7b65a38d67e1ae37793ef1d559b5c9423a99aac4abe7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DoorClosed 1983a531f3cf0d1dc260fac7ba6ff81a0b3f12ec7ba79ce136aca52524bfec88 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DoorOpen 58a3dfa6eb2337bc2fc7e90016be268931452bb799f98eb28e8811f2262d6b59 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dove ab809070bce0f47dec63481efa7e3eda4c956b3419c7323957ed59440418871b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DownLeftAndUpRightToCenter 11d35d5db0e5288f30a5ddf4faaf81dca80c4333284983281a1f28ba977c9457 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DownLong 87936a1c239cb655032a46ffea1b167a57e9857ba3043d079d519ef9d0817c85 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Download 4fe6aa70cf5033c5f3acdd0bb50d759b9cbf497072528da290f1a4ee2a470e86 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dragon a33c983c8b063b5bf5eb45d9b1a1766e3f7d64d314822bd1516cab328c810062 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DrawPolygon 94a8bec300452de8da5d41c14c59376143ee27d340c91cc9a6845b8530969b7c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Droplet fb6525879a878a59acad6b4742639b7578f1b62e89e4af79eda16b3e6d8dcb01 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DropletSlash 8ace9a7038520bd37f30538f883a11448bc11411dc46f015fadc6b8437175fea -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Drum a6f180b8b7ef23be49e3a3966380fc469b70ce069e4f6a4c7f4ee5f8a7c108fb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DrumSteelpan c99401769569f3c46686b70981714b502546d962f132e93769889804c55e4bd2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DrumstickBite 334bc3918689bbae02cd5961eb632f6d81e65755a70dff6f36eacdb9a9917293 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dumbbell 41fdb4cf98412efcb1a0e76e4a9513d2623641090b449afb1df44d5941d9c523 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dumpster 0d28313c4bafbf22a257219d4ac15363919dfabf16aad10027d1ed1ef32e3e7e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.DumpsterFire 6323ce291c482278c752cb33597f7290593878ae4426d26e4e772ec7e5fee808 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dungeon 5570c6bf296ef9113d4662f48d4ade28d5ef0ec973b2aa11b7061194f5cb1a3d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.E d5e000c89f2cb38bc2bfcbaccf4cadd2d349ad698b588da2055d4b118340cc69 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarDeaf c0eff9797fb6e860a4e3601976c8ac8567d8d353c1e7fd6b991db0e08e5e7b8d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarListen 256bb096b6915b56a63ee41157493ff66a17f4f32cfe4396299daa465d2f8b79 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarthAfrica 69dc23d4f0990f11f614fb78bd9204fce922d4e19d0d02d5f59a517ce30e3b88 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarthAmericas f77242d5353d2a8e1152cff156c1da5f4c3ba2358086b878c10e5a3713e495b3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarthAsia ee9d93c52b96393d4e9d1061351202e7d7f40bfddb51ec04a0e6548acef0f696 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarthEurope 13ab9557e22a3ce850f8fc8d7dcc708aceae4bbba600aee3e4b37c03dcd7cadd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarthOceania fae601a0c0ee51ab8576a086fe68f2e73067b08b4b7ef2ed682dc5f777993bea -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Egg 029133253c111dcf503aa715c9dcc7606d85cbacd1003cc6dea3e3042caf42db -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Eject bb781143d7dd892319d92d849ea4e9a4e30e68a5f95bb217c40034eb325a10a6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Elevator 3c1e32e93f3294f8b6f20675600825aea50a44161cfda2193a1956781b6d0d4e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ellipsis c7ea4d6807ed524a22d769133ca971c37f2f57bf14356a1425632012f4d59fdf -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EllipsisVertical 9fa20dc8d1333db9eb66f9c8a2c4014ffc8a9c96b0dc394865d41e8831d5c925 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Envelope d9f42c9c1b4f24aa28e7aa1828a005e88d2d2255052754e3401b09a6bdc88419 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EnvelopeCircleCheck cc37b433cc9aba3cf8e31302277502e24f4605bb2657a8c786f28a0f91494b41 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EnvelopeOpen bdfb6e6a51f0cc6a01c43f8448e60f7c5f701fbf528f66e7c3c202551e695d14 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EnvelopeOpenText adb0dcf61d6714a100b92119013f9664c7fdba0546c219967368303759be3e4d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EnvelopesBulk f43cc1dc0bbbab0df78bd62ae9749488c320d3913cbb4094cb0401178b14bc84 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Equals 087a8a58e8f7c7d3f1306e35e5345223b0be90e1445b62683cee9280390dbebc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Eraser f1314b52d34a7021ce8086eb63588d6916d53ad0ab9ca546a55241620c704d08 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ethernet 63ea1d8001f90003c3c4d42b8129ecc1732b41b983d8146d9bfb76acfa79fec4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EuroSign d4a647b1eaddcb8691acce5925193d89feaf1712380ccd6caafce51b4b38648f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Exclamation 1fadcb0ded38080ab0f5fbae45c5f793c69782f08ca08c455c84f1a53e02a97e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Expand d4108bfed973af715fd794ae2288634ebe531317e27d9958ceded880d9136d78 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Explosion 6d42d588ed9a50bd3bbd1d11d3b001b4b2d8ddb69235b75fb18615fdd98f11ca -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Eye 834503d68f77214054cc45be51d058c635aa987500843bb0f0c251818a477036 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EyeDropper dcbefd4c00205adee3f12a5ac9725520ab91c6bcbadf81db3d5dc5b81779ef16 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EyeLowVision 95e58b3e2ff9ba8c32afd73bd99a8563208e5b4f1a82d18a5173a66fb432db71 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.EyeSlash 9884348d84032db764ebcf132f4acdefca94c625c69c68417015914767d89b46 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.F 3b3af974ab4f66c00cf83abbea970617f0d946ceb7a411b99130c97002ef24b2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceAngry bc5234b9e20fe0769db43943783fef85d753296df26b8f0cc55897a43755f564 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceDizzy a5961c590212164df802bfd2b3531dbc7b52e2c320bf900d5ddabbba42610633 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceFlushed 1509351a022e2639e97e6e177af6e3ff0440d73793468f79ee6281d11316662b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceFrown b65bd489a23c29f6649470e5f5a7083a9513e024f153e57219e645cf464cdb38 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceFrownOpen 33b404ed8c35c8ed83efc9639cbb09d3cbca8ebfb9d5a8da7b7ddea0e1e3fdbe -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrimace 1226b8290e61cb9a278b959a42d4f750e8d762de2b71980e6c6cb53969c106de -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrin 4dcba1696422b31d3e9ebc298b1d109ee5a1fc7367e8c5beb39582f8f44fda16 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinBeam 275661c96e9ca4949e00c51af09d9285b445008d3703d7290ec08f37d4a977fb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinBeamSweat aa31973153ac6d20f46d9813d18a16126ae31f5163ecad36b4d4a76b9b22b134 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinHearts b3f5381dcbf6e0761e6b481ef6c6fc061273c428989648cd28c3c9689bf3d569 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinSquint f575c6c571d6b1d9ad73526b231b7285a40730b5e62f9ef095134d7f605847bf -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinSquintTears f166e3192396ad94022d6241ecd8541fdd1d9cd5351d63b511de0f4b57abe611 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinStars 3bdc4203479f8b7476eeaf1d4562ed99f41822a285a3b2ca72d4ee2d3d865e9e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinTears 178a3109b668d97ff158d250e15ae61e26c669043957d89a0bb634ec75ddb78c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinTongue ffc3aad60f8f098abe0fff069ba1a921617d782f9c56d6605a54f36069f28fec -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinTongueSquint 882dd46180fb95874fbe5d4e9ec7f37e1aeed9f778784c8aed27a4f81946b999 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinTongueWink 2ebeb8821e09ab7adb0db6b7e2c9228692fc89ffd02ae4c50d7984f6d18435ab -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinWide 5813a64c67a5d0ac935818a0a7f85982b1e8a009e9032d2f1cb9ced31d8f7572 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinWink 5fa28e5cb63e8b00adacba39b615f52ee094dafd97bd83370ec3197207cecc2c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceKiss 99da2b7a3d002bcf23b16cdc3f63dc34db3bc0047cf1d8205bebb28cd654a744 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceKissBeam 72739836a5b5c5fc8b1042c68fb00aa13de71a1275fbbd80567aec9177d2d089 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceKissWinkHeart 66cbdb4bba581d1d6ebea4d1978cc37e146efd9f654a2b3ccd1a3547a6408e7a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceLaugh ebfe2f8d2ae823ff96512ecee088df349870f493aac34da6697f3c820b7d4c36 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceLaughBeam 80eeb7f8289f05f9b78a8321ed27ed009bdd9c9ea8b56f5fb27a191551709cbd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceLaughSquint b21893728d6a0838bbda9c2ebe2ca2a5a671d7460ac950058e550d56485d8744 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceLaughWink 6e5df8f0f5041273cc4e1dbe78adfd117a1ecd3a845b7e9a756e83f3629b0ba4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceMeh 1cff522fb6aa6153b78db72196f9c4ec565aa6caa9b6e3ccc96ce0cf6b7eba35 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceMehBlank b315268dd8bec71a327385549c7c1550f6ff1891d813928ba059e75327bbfe00 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceRollingEyes 20285df157c28e1027fa524068aaeb328a8fe4e33c1762773a234bed96da5404 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceSadCry f30334a4c953cff071a03dc8ca99e89c5e27fabf5c452ed950dc1006dafdfd23 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceSadTear 8da813ec054c299ac0521d620a9c5ad854d13af5d2b2a81f733b07c312485739 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceSmile 0ec4ef048d0acf959e0e3f47142acb631548ee34599723b69ad0524dfa46c83a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceSmileBeam f07cb498842e25d972bc9e49c38aef680f993d26e1f676df5e7907069d3c3804 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceSmileWink 9c1a52f9dcb035a20fbc92210fdbc50565b143240a6d2cbb12d7a0b245fed4ff -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceSurprise 9e7c59faf02ea7f947eba22c121cfca762d5d2af5fd037fa3e1908e95fc07ca4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceTired a308656ad2f56a27a178d8bf39f1168922dd9daaf78a2790cb45949f4bd4652e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Fan da9a18a33c4a1994c38454ece3ce40fbb6d5ae7a1f08c64f5994f8f85e405d43 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Faucet 6a2ced13ac6cdcb1f5bca0995d864241a54317c274f07fbde4547d7b34fda4f7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaucetDrip e9307736819de3c8c5ff7c9e2f980e2c12f2dbdb97be8a933461407c5fc04808 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Fax ef91668436a398a06ea007cb521b981ecfdae93f682de0dd5601ca2aa5367cad -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Feather 93e1bc79d4ea726a672e047c54d7300321e67dfbd43ea2e8be3e173a1ac8cd68 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FeatherPointed 87dc47ceb18773b54e545ad6bd254e8339511b576e78bdb2274640cc7a4d7a93 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ferry 85d028fab51a1ef6836f714ed7e841558c9eda0e9c9e03f9edc5f2b900eb127e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.File aae9d5c2f98e9ad3c4a37125f3a0ba3fe36927d2fa0dafc964d1938ccf42c530 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileArrowDown 565e54fcd5fe2546468d3a79e7941fc35799eb5ab393dfa7c5318a49b32ff896 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileArrowUp c21a04f817d3df5f865bf31f004e272700825e05672434079839bcc3ed524b4e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileAudio e640df73e46f9969f39da3e4989eca4bd33bd5079a9fa482e16080eba937a859 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCircleCheck ee4b4ef51be1c4fee1c4284464636d0dfb3a90d4321038d48bec4696fd714466 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCircleExclamation a80485b5453abd476e8239fc31b7944ff3144cd2e33967f28b7a8f5bfd1f924d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCircleMinus ea9cdaf485bb500ed228df69116cd87ac980fdaf5a405bfdaf99cc98a2853aca -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCirclePlus 1d185b41532c58b65604f450db62a388bc8ce5403b125a419078776b54b41eb1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCircleQuestion c8c2b0a0d9f5b286e8345865cfae089272a957451721233f69c8d1f7bb0a974d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCircleXmark 88cd91a918a93a3d381f8ac2b786c0e08f769a42795fe497117b017e885e7691 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCode 411dcb7b36dfd35650454523beaa7f1ea6804273208d46db90586ca43543d38b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileContract a9e0e6519207c8cc284a97ab739a702c3e7d0f995d87e6d91f5c47ac8a10bdfa -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCsv 795adead41241ee335c5bd1de4de7bbf1caa6a58cfa87a1ffef0c73ec63cd4c5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileExcel 455d2f568640d8d760063dabb01ab6515bb2380814d96da29dc725f344651e11 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileExport b4c7534e80685ac4bcc9b39eb043d32ba5a3be74527d04e5a88a9f00d8d0042b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileFragment b7c3889ba1d5b52c2f45e3ed9c2d24283e82e36986413949f230aa76c69898c8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileHalfDashed 469dd46177bb3de8bb49ccae77fd73c6c5fd9bf606d23ed4f0f82e3f747eb3ac -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileImage 4152a63adfeb0986a7a1e1c991adfc40c9406400d481727843d06c8f2a020a14 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileImport 53938fd1feddf87380fb9885b92db61c16ba7109f8c18a78b138e5eb61f8bf7b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileInvoice 539d35fb2547a46e28cec6376ca230f7f97e2ed9cc35b472000163ed4c86ea25 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileInvoiceDollar 2aebe687d72c744eeb0383d34d8d67f55645e9141e0cf44bf5e6061d8694634d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileLines 11feeeb8748d7fc57a7de244354aaa75e6596642731191e9f13b5d7644393870 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileMedical 3941778c0ea697d7b7f54d83a86ecb5e056a3fdf58dbc8d0f1d2f9670c569a9f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FilePdf 1722480496060b9cb64d0665c643513236a8ea1237f668568f0a32ae91b74e09 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FilePen 6e3403741a0041ec1883ad432985e8ff8b92ad1e2d60004e2c43d01c5269c4d1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FilePowerpoint 1f0c4fddd4d75fa67e31a3f1b27fbfbb69a0e9de6674ec20ccb9084a9bedbb9f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FilePrescription e47208f157f6921526dc8bde39841738f23348b14d0681c9acc67f8a8f389bd0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileShield 2aabbff348f984272723178c041bb09a89f3891635505273ad17f68658fa46b1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileSignature 0ac429867e350491ea6e37c00a8ca9a455b29e8f37be53edbec23f1ea1784f92 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileVideo f174cc77937344787ae32cb2ca9b9e151a8158b29570c542337966df06d8585a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileWaveform 778d73679087c1fa2dcaf631b36a21539d4bec7e874641cccb540c3447177973 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileWord 1c4f424f432c8d59736c334f87d7a01472b6faa07721cf7faa14e53cbce5af5c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileZipper f8f33abc8c1021e684bfbf5313c64cd63bd0ce51c4a1908bffe3412642c6ebff -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Fill 814beafafa2318462aa6fc751fcc59f18db30f97f3a700f08dcfe22c960195e7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FillDrip 3494607fd32255917ce312219404df25cd09746f22c5d36bbedaa768b4df5894 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Film 4852bf06d3ae63927a7e94fedbbf088cdc355d990774dd80cf05ed8a13a7ab78 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Filter 23ea6939ac415a446f6c01ebce921822f66d7594e1f68417cb4eca718c872f21 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FilterCircleDollar 5163e27b5efb306d32e129a30954baa271255ce05e3112707fe7804d71e3b5ff -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FilterCircleXmark cdb835808847bf5b6cff1c03b9ffa91b112a98b6ff4f830598e1698885118b7e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Fingerprint 730d1207145cd2ecee1ffc2745be1e32c618624097f8ae56bb11812a46cdff2f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Fire 4c65a2f6da74c6871a718a30144ce24a7bfa086eb78e12c17367e86bf227239b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FireBurner 2dc5fadee3cb638cb58188dab4ebfbcb6e6ec5732818c8bab15142dc2685e6e1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FireExtinguisher fd8434b7362600f8e361c516ba7871728fc94aeb6d1f5f7a1868a787018de565 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FireFlameCurved c43078c9b07ea013492014a26bba59ed377aefd6e57621c19dc53dbeee30b584 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FireFlameSimple 9a5e6c306d48b3e4e0e80473c63c7ead11365c79f21e47a15a9288b12502ae76 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Fish 9b53833eda997560bc4fcf99b007cd830343e6dd089b98a3b538ac160e9e073b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FishFins f7148f242910192d75eddd4c50865f4d3248643a6d6bc98b77aac30cd3fe3857 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Flag 0d31d4621962c1c3a05909ef288cdae2009009cbe6aed5eefbab4ecafd7a8985 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FlagCheckered b02d319d3e393393b24c3f043ee4b78999e02f0e01a6f9e7e11b28fa4254e130 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FlagUsa b8471a96f7bd4285bfe44bc2aee7a80481f09d908d4bf2f88d7e44ccbad72129 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Flask 4f099f6bcf342eb468ae70c09e3cd5b5514a004f0b94f419d289c3bb5b007b67 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FlaskVial 1e657de97db45e5a8c02a2cb2e47a248172074fd7b33226fb76c0bd6230cce1b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FloppyDisk 85f00dbd3d719b512064b654d9f617befc0c71e939b6110d44797990a3919a32 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FlorinSign dffb7af2e291756569e17698722e98166d11b5c36211393c95c4e3d945e7d69a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Folder a6d9261b25f1dddf29d97ac125688e9d0c6c029adbf03735d0f53dde20366cc9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FolderClosed 2bb12e10cfc0dfe9c54fa887ccc30d004c5a5e78511d5562174b8c342e3462ac -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FolderMinus 87c7a18b3b818fc7c53012b5b1fd86acf58eef068ae03c6d59181f024a748cd9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FolderOpen f69ade8a71761acbfa4f2ec75ba295da3bac0af351e8184201a17f7b449185ba -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FolderPlus 32aa41817518002ae5d18b9bb0b6ff2f207833fed5df1a78efa409f77a0782e6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FolderTree 2cf849900985336fd7e547cf03b9b116f4e28175b34c3e794b4bfe1f7c782189 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Font 4a4fd8d477bea807a30d6db8ccc76b25286f08ad3fa5e3fc423bb66b471150e3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FontAwesome f7ce4894cc14b0efc7dfeb64353fad60ce7e7c3b3319081cb212f1ecedeb43ec -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Football 84ebf5a8333059ddfb17cdc9a599701078f9a29fd00124b0152c5c523f6f2e8d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Forward d54eafbd0af00228a681465c476559f45608200ab20b33725b9b59d739a01598 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ForwardFast d278c7211b8802aa3ce9458071baa9e9b1e75b7527b0f5ed480ca22f6bb632e7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ForwardStep 533e1990ed80c6156e9fdf20fb5e2ae144ede4c618a4318c8f1e7d2bd8727196 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.FrancSign a994d08d457682478d20679819e8e5434873ab5953a983a1872a3481f57ad6c2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Frog c3ba8105c2057282ee019bfb57c5e10ff58c61ec4fd035aae817ed164c89fd0c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Futbol c1bce3f54a0dd20a50b80e4f77a99fba9bb33a036bd3cdeef33c6869d1fd6ddb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.G 9fe1f6b851b84800dcf164196b3a482eb3ab618f2bf1cff1d6c40e69d0e70cf2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gamepad de5dbef4dcc393e40cb805a74be0a566045cedbe4fcfb5b3703e7fd99aea10b9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GasPump 7e8f3fd67606d7b9e778707fd24d88de9dc9c693419729eba7e2009ce89efba9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gauge f67ba33a8b8db5b03267cbe491b1e4119e3630a5ea7b300279d23ed67f785ea0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GaugeHigh 631a695687a9c4a6c13f7dd9e4c2897016ccb5f4c4d0969d04ccb3cc94fcabe7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GaugeSimple 0522fa2d0677cfc7020354352480c5b9372bfec1b2644aee55953b39ce64fd32 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GaugeSimpleHigh 738a09b026d90b12eb1de054a56795321f0ab53d85d05d92b9494e89041bf6e8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gavel cfe5d00546ea959938204b99c763151ed5816bdf71ee1eaad6b0b43ec7a76f26 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gear 264ffaafb2dd65289c886bdce9eeb88f23ceb7185367e3e768ce7d77a92aa40a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gears 403e5fb7dc67578ceec8fb10b7f28081b453b310faa5a71dcdab61656d59f29d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gem ae4e5f0a2d313bec1850fe5cb949c8e93483b7855e9031232d97a35b05574e09 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Genderless ebd1f7a6798451aa0fe976b3df7b718b1b9d995b809af76f291f47813955e443 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ghost 27ac7bb6c1f97ea95675b97026e125a84795c945ce340868a31813b4349f4715 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gift d032fa7acd1f46c77b09fcf832e60e2ec88ee1cb5f0708b656699f48655f782b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gifts 1dbfd87dab1da44aff964025bdf513cafa54c4cd2676825e6e16464fe5c8d7b7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GlassWater 0e2c45e25035e4850814909a5c11674d93874f610e1d1434bb4d00a07374d2b9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GlassWaterDroplet 0c159e8db5726f30f18594474986ece06a0919a42ac7889a8396285869d66d54 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Glasses bc57dcd6f8d22267baad2347d31a95e8b26a782daccdf3d9b5877e033f37cf62 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Globe a6dc79b22a6ac447b349af4a0536d48588ebc2ec1ad266f0cd5e7865c3b52335 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GolfBallTee 9bfc0a1a1793687d1946348e6f73baf39881c3af3f8879ef119fc75c1fbc18c2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gopuram f43edf00764f612db9e24d154c20581f288c251d7202a9f7613c7ec73f75f54c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GraduationCap 821b138be497fe6d688af86046a23dd714e5f4b766eb44dddb0308b42a5d82c6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GreaterThan 5ef68ec4fe82c09e9cabb2ee9447d0b1268835a1d72c637d4d520c306ef1849d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GreaterThanEqual e034caa8b9cb2cf34d47ca62c7ba9fbf96b4dc2726fb5bf4b2aa8645080aec94 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Grip 62496b1fe009c245f0f08a592f48718feded973eddfe41c682997eba7dd9ac8d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GripLines 32036d58705dd4222725e96aad17b60c277feba3e9a20606a3fbe1d33d4ce594 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GripLinesVertical d01ce4dded883f5e5954ec71eefb702e74ad871fca29252de0c985e80f119ee6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GripVertical e434017d5b181b34bf5d04a9ab049443405445c70d1688cd18d6f5c1a3040813 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GroupArrowsRotate b66a7d4e0c5133da7461f6748d970772574af2b60afc2a2afe189efe27f1bfd3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.GuaraniSign 691ffb468d4b9005aec9e47f9a28077b90b42932b4b2815dab2439936d663da5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Guitar 6b54e6fd3f80746e08b3b13cac224ff86eabd48ce66c8e80b72e8b97af011c03 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gun c9607ae0f2cee125a2087e36a4ee64b44463d46611bdb9766f9fc2f6cc6d5520 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.H 871aed2b10884575a832a0274e50829ef06a69aaf19a7ffa3e7483c3925c26e0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hammer 3ce44937f2536494be43eed931f14e178a0e4a03551701a9b837f8241b8a40e7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hamsa fd2d484400cca64bf925bc858cb2e7940029c5fea3fad6cfb4832924eaf21620 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hand 8754e3f8aadbc3e40d37571253840de95cf25365cd247244da65f3b0eecc0195 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandBackFist d505d5797a20ba8315e0585410dd0e0b4c1738de542e3afbd02b4d5cf4bc0fd4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandDots 262d3d9d91444741c8f2123fec80a50aa180f3720be096c959be717c350cc35a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandFist 006bac71dc77bfa98c9f188cf1fb21b34e8464830332280cc54e69bded155ad7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandHolding 33fa92dd3252de521b0f5731aea3b78af1e320f5e9d329e5994cf21359aef8d2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandHoldingDollar 99eabb2651e2f778235481d29825fdbde4f3bc654db15825283ab66d9038d169 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandHoldingDroplet bf3a0fe93011bad6cc753284ebcc86e77202c080c3d8b60b5f390897124d813c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandHoldingHand 1d65f4d47e72708bc92098bdfc3677e53c7b9acd6c3c8afa11c113b18b130ff7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandHoldingHeart 872f4d8c83d3691e5dc97c79a361b3255612e47f3e209de39cf82364eddfc8bd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandHoldingMedical 95389f0aacfe220f03494ee030e769d284711ebcab628c23b9b05a85d4e17f42 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandLizard 0723003ecac4d4b8367c569062ea34aa2aaa616050ce59c475aea5accdaed146 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandMiddleFinger 1fb7eb11d3ee4f23459723a0e1431a558426a966a1d9264d00f642accf06e441 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandPeace 6310cf85b5e75c32f8a28dd3f5744df8e0518abb0f32910e09662c77b029f562 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandPointDown 73aab51542df68ac6e2023082d689dfdb71db1e1630c054b88783833afc7f952 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandPointLeft 7e0f1dda10929df7082c7495a56ceb4a838956b10af52479e1ab45b59fd6aca1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandPointRight 3eb4f8dc9dbdf4d394d63672ed0382e425541b9384ee593f9958ffbb63ead2c9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandPointUp ea01dc8f782ed83f90046cbf29d6da7b9cd01a696b3372a81548eb8cd4d54fbd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandPointer 1087f46c07b25a47c1079030417d50693604facbcbe6cf87cfdb341b2dd947e0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandScissors 60ff9523dd4dfe7cc3c1ba4442045defdbce065f4e55992a9df8621f0cc148ae -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandSparkles 3f112739c4225c52311e8ac0db093cc769ed67a29701f9d109651cb44004be9f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandSpock c9b1dea489583c6f4afeaabe1e884c7fbf706f33d291757682af8b2346da995e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Handcuffs 546816fdbc7e425b7443482d52df1ec5f54c6c29441004b6dc7dea18d1a5b9ee -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hands b6152f21b0778f6a843a8cbbd82d1f2813c0a6a603c2a00d99c8a05162b87a1f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsAslInterpreting 8c1af8db0cc1e280fb56fa0b65ecc3fdb95a859e68b00ded01400036fe0fd6bb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsBound 85f985cdcc49f80e71a18c7988e7d0a6ac1f78c508157ea734df9a220ab3cdfe -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsBubbles b52b06892004211aa2f8fcd002ed3233f69dc618c261f52aac4dafc6870827d9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsClapping 3a2c4ecefd83bba0f0a6cf6e2c0932c091f6fea142e5e73472d24ae01ea6297e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsHolding eceaee5e15290c8d3a7ec0f629f9b69cc787eeca54a2bf163e004ab0b556c7f0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsHoldingChild 60117b968b8ca85a2cd6b57299dcc8d6fd0973186348df4f63ba6a0e8db769c1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsHoldingCircle 6b78e544c88d0f0446781835f31dc4466c8941230ef29fab13be1eebf068774d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsPraying 3e377da9c21f37b99f0c22588f5e7330fad24772baaea156fd2d543b34e7eb53 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Handshake 9529135e4d32932f99100ba991a65407255fc6ccba5a51729442aea0ac1cddd5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandshakeAngle 42f3d6af56572f7d28ef601a0d713374f64d524f49570fbcff9f0da71044c140 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandshakeSimple ef61c28b1c4abd51c122b6bce3c87f54e7debb25f1af8132673b2879adeaad86 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandshakeSimpleSlash 22cb1bdeb6a7e9592361b2bba9d0dc90259ecd8348dfd445e73a8d13ea945f75 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandshakeSlash ce01bb598596983647a3b52cbbf44cabb294a294925477fe88fd98fe48319ad4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hanukiah 9e537fde9e89ba646eb5186e6ffa8d7f4c2cb050fcf2ec379ea45c27e1db19c2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HardDrive 5c7b08fafee353b3ca23ec722f3faa9cba5deca2abd810622ccbc42abd5cf12e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hashtag c3a092492bedddf381eeaf78e61d7784b10d7da86aa74c298796d763f02e2f2f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HatCowboy ee6ec98fdb1e2a3658f0b7acc1d1ead304c91cfe5cf7a026295e21a918bd4c1e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HatCowboySide ff4cb94bacae8a38b3cbc2aad6aaf8e7dcd37ebca48dc32cb5e2de8e22de88dd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HatWizard 7868be82434f049d168fa47f9935efcf398b762db5ed75c79b7d86eb081f0960 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeadSideCough aec1f38fca77475fcee39950228840729e819f1aa80964f996bf86ac89b13742 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeadSideCoughSlash 338b3b8aace381022a4f672d9aba8a4f662317d514815da301ae794615bd43b4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeadSideMask e1a574ba81d1d4c42ceff775dc51ac322a24e3ae488e8f727aa7ac32f0500491 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeadSideVirus eb3e31ca092a488fab3edf6fa2f426fc9af253399831d1e350a2405d245ed017 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Heading 006f148c08634be1259c02e9052bfb84add43480ba2dc50dc00a40823d25ab24 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Headphones 90b7c2cc9a9ce4adbbb8b5047469e1c87d71a71a8fd805e9165c4c4820d06eab -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeadphonesSimple 234486872b92f90e58747a1bdbb9002ee347e16cc28180ade9a33fc9b6798976 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Headset c899a6f5aad14d65b3b141c6b293d15e49eced7128b510ed1b535f248383672e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Heart 430256411490e3dc88e6e96d9324450ecc4486d8c5587f5802df644e2fdbf367 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCircleBolt e4b44080f81c0d2e0f135aba5270b46dba45b6189ebf427ecc6812f370a2c65a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCircleCheck 3e553b6c7c8e5adcbbbf479af5551880cbf7aaf11eb61b7df1ff7cf02808b158 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCircleExclamation 8577020584431a588c93bf120e86c2353ca7290f938fee56b22af647885d6127 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCircleMinus 64032b890fac66ad4a1e637c67602b2a12ee59d4f3468f2e8d2645b65090c2a8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCirclePlus 2093011030d59abe993183c1206de9813a16282490a01d241570e2e997a3379f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCircleXmark 20e7c0c72e100f5c21cf57e3eb4ad770c07231ff03c819ee8f68d0f44f66cf3a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCrack d263d91c5a10bdbc4da538aa3843c6a8f33b3ca8b445dff7d16cfb04afe56a7a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartPulse db920add0d841d44a2f59b96b5a07e96261ad90fade693f98a561f0068034e40 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Helicopter edf7726afb93e59c9143ece8122d6c3d9255a6d196f5a31b60590f4a6cb23882 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HelicopterSymbol 16aa870a2d6e1a1fb6c898c1516ffb0a4c3bcd431cd77a6bfef4da9288537f4e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HelmetSafety 58adebc3df586269961bc3bef39c3ebbce45d71a553ceabfefd8a97c897b1e07 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HelmetUn 5e9dfd2cf427638df4b3fcb108e2bee314a592282fba9b7a294507c5c7e26ed5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HexagonNodes 71e5348969bfd84bdd17ddb8a9249829d63b0ff2718a480e48a9f8156d57c7bd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HexagonNodesBolt b6ff946d061cc36f60e5d7831ebd51ebdb0adc136a046421882cc2fdb3e36a4d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Highlighter bdf2c8c05773dedf6271ebba56172f798a00cc8587f9e6ec94da10e8d56f9eb7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HillAvalanche a31de0aad2f66265be5ca38705ff8b035cc8a15f5dbb6d3a0fa61a5aa0f668c9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HillRockslide a01fe1d6eb7e30cfe20cb5a822492d5d8ef0382d24ea5f78cdfc651343e759bc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hippo b24c4ac369877a70cd2f85d046b8452aaaaf43bdf6fe1b906b7cb0a3c34a5558 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HockeyPuck ab477e56584050aa9a08b091b48b3b6ca0a3827e50a15286e1e6dd6290944950 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HollyBerry d9df622cad8c88fc5388bc404be55082b1e1925d903d46f804282ec79ab0d308 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Horse 7f0321e33a30888dc6556431759593d66a46d253b2f5a0a3fba6131e7bafb197 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HorseHead e1d365fb964307211e1f52b27879dc4c9f0273df09160d112f862fc0264ad3df -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hospital 43d1bbb2b9c511a6c74284e741d08f8db9b833a9504dc027a058784f3ee8871f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HospitalUser c5a144b50ed18b4a443cc855e4dbccc798b6e7521fc06e2ec80b9b608b45935f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HotTubPerson d0e2de86e1008e9261ed56277a30ec83c0ecf55dc255c79e5eb8fc8c97ea0f0c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hotdog 8b1f01630d9990037fbc827e9f9fd0f2f20480ff4e24a3f4c05eafd1a90a5df6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hotel 85817560fd8cdae0ac87bf79e2b1c4081ad74f26f33ee241165d837fda475b44 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hourglass 0f938e44b74d5b12df802b81766eacd7a527839baf68637f4800cb3d570d0287 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HourglassEnd 7efbfe65f5cc340a63040185dd9fd8810a031ac0a69dc4962e9a55887c0b7a5e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HourglassHalf 26d883fb03b848c5f15e85387b12f03c8b2e7e7f7d9669165bb9f13963a74260 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HourglassStart 437a08e9330cf425e5209e42f0de63085c610f6b5a7ef00f200c6b388c207576 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.House 935b3e18bf7d6247db5c4b9451e1bd1ad2eaf35725183ffdbb8a881486d0809a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseChimney fe5bd7acc63490de1e12db5fe4cbdf191f1949d40fa22d4a009907b09f5aec84 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseChimneyCrack 659e768781d421da91c211251cf7cec8a55afd6861ab7e5d34553927cebe101c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseChimneyMedical 526efa8701b1b102513c64668108b5ba4540e91454966054802ae4f09e58741a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseChimneyUser 50c99e11a2a7f1d7a0df05b00248188067a0ed406449449f24e43096ef5110eb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseChimneyWindow 52064789f65f58de4d4505f67af10249cfa42c119a34cd8841387870e3274255 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseCircleCheck 43991212940f66e85990b9b9347d90cda84441c923d5fe66067582cc5d273a76 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseCircleExclamation 7775b370dec7410c8911894321c24e63492326f77e1a1ca31fe5fda157be0817 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseCircleXmark 32800f6454945ef5a83ca0be81171d6b8e616926d70008267d30b502b5ae5549 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseCrack 296682287f7639d78f8fd7d7f41849b4d3541e045a3948d03b56a6ce539fdfbe -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseFire fd2db604ef332469438066047b68ffc45c72b0825d775ae3b0a60428ae528d1d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseFlag ec6af20cc31095e5f6f96cde00d29088f652035433f6be655df774694a7bbced -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseFloodWater 8727e441ddec7a317f6777fac3f82522c07f867b7a2fd83bc5c24175124be21d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseFloodWaterCircleArrowRight d38c461d131794b752ba35585f5fb40c8492d24ab6c8761718d1c22cbda965d3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseLaptop 3d602960c68d4a1eb8d43bbf7df3218457fa1c5ff839db56faa06c15f198b593 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseLock be742391415ce4a15b15b6807be3a0923f61b823a7805cfe247c5fb6f6b1395e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseMedical 6eb1d477db9510f7222a13c07e392ddd6b6cbf77858d26d3150c611e6276e0b2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseMedicalCircleCheck 5f4ec148017008f8061a8a0d1a6fe7f7f2f30ce0cea48f93264977d0d706b06c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseMedicalCircleExclamation 5629937ab337bed99b9357c3bde92777ef27c4de5bea00ff931a62f2767979fb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseMedicalCircleXmark 9f550a582d9b564a0b68c0a8b6006fe4376822cb4816deb0df22be673fc92ac2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseMedicalFlag ad33beb942f2e2881c7a0c7caf6527f693179cba0a2a6b9def40e32b66d3002e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseSignal 102c87d3edc4de4bf93a2bd2450aea07fa84393e266dedb09875f04a9fb2904c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseTsunami 5e92af98101a658c0f8afbc5f2102ae10d8a65e84fcc3575abe0382db0d28625 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseUser 91d8edfdbdd0c037f57e092372a2cf67121c074e05c547bc2b5a5ccb00f455f9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.HryvniaSign 5cc3541cd8b5b33fa7c63d838f76293d038a555f4c893468c3d38c8328b3a715 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hurricane 0e196587e11e1a4aa9f4cd3a6c3286f13e27082544aaca6b1b53b8a364cce67f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.I 4c50d9be63155f77d0ee39bde39fabdac405e884696ac2613eb0db099e9bb424 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ICursor 8bc8422b08d2a821a5e22bd8892e473d0330bea289fe35b606d8157162e836b6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.IceCream 15599e2acbde801f73610e6a8ec7026679f248c1d8d728b3c78efb4240ee7241 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Icicles 77c6053a827c7ec5135bf8eed968f193eb7765d8ca1e06c148610266b53be427 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Icons 9da930d31421e9a05a83b3bc0cb110ac7d1c91bb9fd984d34c55eda0ec70207a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.IdBadge 4575dac75e987df026dd0eb68a2b6681ecc77b325c8bc985e859710a82d02985 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.IdCard 7ff533871ac13f78ebe56951fad3d4e9b815f7a3f34eb7791918ca90efb6cc74 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.IdCardClip 17db6c97d8526716648739664e65bcab7afa09cd5c3a8f54773660e596ef95aa -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Igloo 428811dd6a43896ea369761d965fe3cc60f30567afdb609741dba79b903a6817 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Image 78c53047a5a6ece79094b11f8d05b5f076aed68f1e97772eacbdca6e3db5e138 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ImagePortrait 77cedac806568abac1a0b678e2f9fb5f9899430cc1e5d583f2b6b2559537e34e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Images adb891281c8f2bf3499e3975ac986d58c47de74a2c17acdb4db8725f18fb76c8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Inbox db0107a4177f43b243216a707e1ce53dae28be7ab46e35ca56257f6cd5b5da50 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Indent ad4c01aac31f5d3c1ff3e8ea090836cdb04c238cd2308b15e42c8cb6d34ed25e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.IndianRupeeSign 2eca68ace2e7eb4df02061caa3780af7687d644b6094703b2b9fe03ce254c7e8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Industry d8564ead6be93c518b7f1f9a3678f90c04bad04931e36c5e71b75d766870935e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Infinity 2320585786b6bfb2b1bcadbc2bb06676f070207f0294e8f0665eb4e0a14c8ae8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Info d9d62736cd51ca9069f5f0ff8735d6d6d6a5647e0c7387aaafa8c566c2524401 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Italic 1591901cd4dc1bed19c2ca562db11281af0e33fea2153c1b7c66c1c74b8d309d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.J e60d30838489318691534016d17b34da85171cfee85bc508bdeca4b89ebfeec1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Jar c4148a5c9487cc866ec5002647d880c098da475070b063c4d7011e32bd3b37fa -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.JarWheat b9fa5ceaa849f4e8fe1570ab7e4b9fddb018d057dc8e3989eb49cfb0e68a6acf -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Jedi 6579dbd9914622c55a1f3c6e27cab976e47d06231deb7c2906d012a38d2bbcf3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.JetFighter aeccdf50c1d24798d5c042264fd02df833015a3c76713571ff386e61d21bea08 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.JetFighterUp fa051b4e1dd56e4d50e3f20c42ae80e2c7b4cc7b9e3745e843f40d34bb592809 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Joint d165d91e43e9e9b5ed166d56db8bb8c10ae27e3193e36350b53f7592ee231131 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.JugDetergent 6d6b51852f1fd49e8d7c654a060a90ce5b636a1d660f1652ac8a21f4e6354b57 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.K c6693279007dd2a1025cf55f0ac7d06d7b7dc694f45f06df8b3a6429e0c0cf0a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Kaaba 12885ba73f2238fb6d627c300976c0333e096e9cb8debf248b713566a7ce3d36 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Key 8d3220b40387596a73d4ba9d6b25979aae9e238d778af1269f7c540e79c308cd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Keyboard aef4594942fdc06f4fc639baa38a0c9693f8ee9758805636fed5780e5b361c19 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Khanda ae55d6960f255852e8b97f621b035f548da84e78a25269390dbf2a0fa077a866 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.KipSign bb95b7f9d17647cea49ef56a5024219cdc12906e1e8d6bf1e2b95247e7aca70e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.KitMedical 1fa887b6ec38c3e5d739390fda63f130ea3b27395ed30d4484509fb059508ccc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.KitchenSet d72576929b5759be8b123c69f5c16525cb3b1638c794a5bc7f695d55f9a6a691 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.KiwiBird 6850477bc932a5864684e4b9e01518eca34df531b44a0b17c91e94b87c10d33c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.L d2179919504a87bcdf05e9ab44961330ee671b1f5ab75779548bc4b75ffd885b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LandMineOn 6cc670b280f0b56c29baf13dad1e10a4e3fc74b7155961fd686b31cd86cee5ff -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Landmark 82b02869731042f8f4f1c2fefaf34a9a735715705d4b7d58be9355a7ff09c6f6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LandmarkDome 6fa39854163601db4ce28ca7d293229f4d43434251e61d4f764a77c865d770b3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LandmarkFlag 2ccdd825d324f2e9f01f2419866b66c378ec0ddfe9ac5ce7774ccb1d30e64047 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Language de8379d49400db07a64cea0ab694b82a38705f84a712f788562b608c2c8b72ec -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Laptop 41bd2198589b82a343f3f91474b42d74d71a0a15d7d1b0cb523b36ec298d51b3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LaptopCode 433943a8a27836215554f31e24ff636c231e7441f90781db9c62c3cef37202dd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LaptopFile 1356be1aed3d32781f47ffaba1a2a4134d063126238432a5d8b01ed547d79312 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LaptopMedical 1f3b3365ca154cc8f5310362d776d40254af11d0e65b8db75c67a54e81acc425 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LariSign 172a9b7640c55b889d3e12afaf6bfa0e96ad04ff3e38a3880e8494e964c47e30 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LayerGroup 03afac9000b80b9a076b27b6e2c2341e714c0da7d05847104d3540522851d92b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Leaf eb644672bd07e1a103b1646f1fdc2e5f376bb39849b3a9f65d06aabeca5fac70 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LeftLong 200d82485f8e59e5607fe238725fc685e18781a7be2d5b62c3425a9fc26ec237 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LeftRight 36ec521b5e9b67937997c2357572e1cf16a142c520002f10976718f6d71abc8e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Lemon 322893716407ea32d309bcedf8114978777f8de235a7e953ddba0858ca427791 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LessThan a138ed3e61ae48c61213d7ec80c80fcb5310f73608c3a762973c2dd88d8eb47b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LessThanEqual 59beadcadbc946d1fe10cf992d0912b69ec8a92ac5e0861a94621078e6ddf167 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LifeRing bea83ec2fc1f1be480276367d7b08d921046c090b9983100892332f232c8df84 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Lightbulb 7c2417b1cf5b6b89602595e92c8430f67e78d9335310796879d9d468d417aac9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LinesLeaning 2471e64435547d7941d7d5ed47958ba2c88fb89061fb2d999890b56d528a2e21 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Link 293fe88f920befe8dbf732121bd81dda10477f9758e707553a5c0cc4a2523a7a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LinkSlash 6fc57edf249fe5700380fe113fcabe8647f90ea35d18064967271cfb4fb8b48c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LiraSign 4debb263b276844f68276e97eebf16701539e367f58ed71d15986cec04bd084b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.List 1f0cd4e8a7cc4de90c15d37c0ffbb66e65b885e4653cf4bc852060f021028057 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ListCheck fcc490c53e3486102e90d153beeee7f1d60947f82e48105ccedb9e1d0a0e6ce3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ListOl e656b02f72538248c385f5547fda3a2c6404a16722788c3cd7ad91efe8a2411f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ListUl a1d6d9251a2da28953cc0f363cf895793c4d960741c4cac3098d942d51211d20 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LitecoinSign 90be212baf91edadb6cf7bb3b25c8f5fddf2f0986893203140551af1e1b9152f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LocationArrow 7133a1d84e24e7420a7e74ccb743599bbaaf1514194e0d4a0b81c4ca5e5e91e5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LocationCrosshairs 012e389e1233d64626b4275afa29050bd3c082871f70a2db4ea3c9f0a3e8e083 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LocationDot f5825703b359d05aecb16b8ef317dcf547a06cac129e6731a1c571aa568b538f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LocationPin 877459afa6c9288ac63364639e26b1cce38a096f59d1db5fd9f2ba9da50c6212 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LocationPinLock 981d61d4f852d9c177fa52ebe5b60288fdaa436eda254ed8acb8a9fd6143158b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Lock b7aaa895b201e3ac54a5d9974fe5a055522fe9b2774b5aa2577982db8ca1d715 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LockOpen 29ec7766e67e2ad658b56f46becb20f4efa80ed843f1ff95df342da8d80098a0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Locust e8008bfed181bbb5bb6b0eff8a424f94d8e90158e647b204c55886578d8c8c63 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Lungs 6dbe2cc6c8b62a2350fe114b6b6de64a5ef9afd9697131f1857b6aaca6d2ee29 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.LungsVirus 623b8afa4e91af9e81b96abc6451ae96e6b6689962825a667dab69dacdb6fa39 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.M 7ff8f93189a1318ad7d49356ba9d6893582661cb6e93148776382eca28bffce4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Magnet 82a8244a6d9d83952698571ee62a41e4759e80b4a08b3d38f5ebfe4542ca1808 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlass a80e63e393b0d3c3c646a7af509ec766b7e4894617d392848f80e7fb3eb0c2ec -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlassArrowRight cfb970cb7387ecb5f8b8b56d61c5b59b03cf92b3761cf33f54a110c8ffc1c54a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlassChart a8c4deea00079e505ad2e25fb27d61e87bebff2405a022aa3495f836ab0cf79e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlassDollar 8562e722b3d64f14edb32aed069ef77072d53e7446e1ceec21500e49b341a93f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlassLocation b96624f2953e9dafe085248ae876d93957359c4a730ab8839b4ceec35422a469 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlassMinus 61d3acb0400ab6bcd4ad5728dc68336cae29970bde88f810a371c7cfc989da96 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlassPlus f7c718801bf8d991723b80625c521578a2134359a33d81b7ffe215d6a9518b8e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ManatSign cad6ca4b3398d8fbb18d1b181dea977c1a5250569313b0a641d1718d57f72c42 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Map 2336ed107f7b2b107d42946e7b0ea3f5ec47ac4603def34ada797972692c2a1c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MapLocation 07e6ee61e7817eed2b5bce3636ce901e7ee4d638a7375c368a7ded9a41e9826c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MapLocationDot c1eec29f3627609c4144aed2755279c0729ec2fb3ecb7980659ef1c190ae96e5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MapPin 58e4fabe92d28a010b359c5c476f2343c51384bbc596264d3aa25269d68d3e89 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Marker f7f13302f41e29f611c3a4a6e6be10a07f662e52660e72d28312a45722c87828 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mars 807291a4fe3733a625f4b1ce1adace407327496b1008b1e0096d9b79803d2ce9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MarsAndVenus 31d9844e522e07b047b6fc17b1c9c020a24cb008d3e846317946003f00dbddef -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MarsAndVenusBurst 1bf83b5f9456d3de0d5c34a49f5234939ce1ebe0e1ed7eb3a9ecc45cb24caa38 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MarsDouble 22842d4b7813b924e6bf747c80404d69ea385b519d5546735c13741f2b2734fc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MarsStroke 12ba6a3f9c6c876e56b8b4a79895b2ef79e40313e36ca43651670950b6ea8a06 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MarsStrokeRight 90fb844bbe24cb037849a90fe1cf76cb50b2f715dc6d17848f731d1f045fbe21 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MarsStrokeUp be51b5e242e3bb1c363aed851ae13a797ff0a14b7aa5ee150efc21781d30c1c3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MartiniGlass a75e13eb2625956a173b42c0842c36bea73f4e9ba1d31710396b6eae60e121e7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MartiniGlassCitrus 967b1b456a69425c3e27045ce3ae0867c846ea08bd58a575880ebd925a959754 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MartiniGlassEmpty 99daef16849483e5a5ebd71ddd650ae3efa4ff07160ce0897460baa891185325 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mask 4584c976dd0597c9a974c1ebcc30fed568fe7e91157647ac97d2c139c496eb13 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MaskFace e76a67a8a6aebab8af4a7d4fc3138a01aae4f41825fbddfc2befa29cc706f9be -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MaskVentilator 460a4fd8a3ef7258d3b2376151dbada6a1d72f1946c2aa5fea53b43c9b8d0459 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MasksTheater aa9a09492ee3058de6208187ad6d19667c735a46f5154587da125db28e16f2cf -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MattressPillow fc5175f949c7043b8d08a9b6389f630ec0c4ed3b406b88ef0b79545c439d9e34 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Maximize 8d8466e8a794ee6b23de731d0f33c2be2c0fa4ff82bd0b6475919f3629f043fe -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Medal 0226204969f1926af2cbd68052cf615d05ca0c85d42c5b41bf2759df3464c101 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Memory 85840e45314d5678e7cd76634d8a20b6e275b82f458a2ee4be9f602e8ae04c9f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Menorah 822044b45e7347d9afbbdc0bf9df9fe7e35d35d4ec0e3468084c5066768c8af5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mercury 7a38ceec4112ccdd71ab8a4bb4d5c1f8451aeeffd5f028a66e3a8edb41fcf11d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Message e49b7f1178df3d2ac7ccecd27e25de0f865d0b27e7451747801c06ee4cdd5bf5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Meteor 1e9012ecd71d2a70d3d1766bbaa4c0d990686878a85b9d7544e91af692322598 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Microchip 24b4608737757db67d6fd290c3ba1487efc42190c649d7b394b5b79ec5f4432a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Microphone d16a84a909553542a6ac3f66d00fed467118b4246dec87b2e54ab75fe44fc217 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MicrophoneLines 74a8e2adb7e08f906f6ac05c0e6425e543ffb440c49e623713c30757a305d835 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MicrophoneLinesSlash 4822017a428972b6b32315a83aba542e0573531e6571d272cc47618a3f05ead3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MicrophoneSlash 9bf06af345f659d8271ca6de740cc9e4918434e12ddcff9360d9c0ce255dd5ea -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Microscope bd3d03edd3a84adcc38c5e1f528edc5bde1c8f7d87c0d0283a217f086476d9b9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MillSign b350ad03cdcb937314c2be4643ad6d2f9823db969032eebb2593311540f3a4f7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Minimize 8dbd292261c54817f0008292b56ce4e97ddafa725669ad774af18ee040ba2099 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Minus 4f5528001c2d47a2b5cf41f30d2835491f424f3684b909965d5385b86f37ff33 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mitten cc2126963d359857fdbd2b5da57b7010aa4201401d3950b453e542d3cd2d24f4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mobile b66c26ed92a95a5291d91b50b35d53dcc3760b52a6fb564b01d812aa65720174 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MobileButton 46dae0cfd87959ce0d4ded05e5c783cec66d40f119a7c7bdda62f8e77007310d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MobileRetro c3148897d8beac9ab81094a3c7c22707465c3158d9b2a53865e2c94e84682479 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MobileScreen 244fa6267a8e5f7dda2f1b0891a83c818353c66f5c13ce8f98f7cc80be119930 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MobileScreenButton 2d3321ea66d5b69671f8bb7c59053b6ac5f1ed8df7a9187be7453b6e2260a780 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBill 13848a6c55ae7408a723a2af78825bfa65dd9a8776eeb5423c6c471ff8c31238 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBill1 b1fbb1582eabfa4ebd45bccb54de498d2d1f9ac6cfbc62560257b232df737d89 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBill1Wave f71d8c6fda62655a881d3cfbeb0ce958f07b87144fc107172edb4ffb86d5166e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBillTransfer 9c621a80c1d405079278af4818b01597a18e10b6ebe5dbee8460ac819088a080 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBillTrendUp 0bfd23d2377d822c115a81bcd08ce89565c419328b8e5da53e51112f66b9ffa1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBillWave 9f3b94b8adec188e5a4845a5f8f9ded7fa233992058b4eac978c71367f47df77 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBillWheat e7f9dfe450c4eb556f8d340e01c77a6ca0971cc9167bb7b7ff09de51c25301b0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBills e97232e1b61034e1eaa2e8f3551166f1eaa4e247be53dc274f7473c8d3bf77ee -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyCheck de1e1c92502d314e1a78dbcabc9f2dceea93fac9232861881d99f4ad9ac44ad2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyCheckDollar 2dce82f70cbd710730bb55115bdeb680b7c3586a4f1ececa4a82cc6eb91ff859 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Monument daed79f1f91e8a80c42d35a4b50fe94d959c2440067cfee932e76f1004d1d19f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Moon 6e3d3c9bfebf71a243aec3e787d6d22fc62c60e0621c88862bece299a6289b47 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MortarPestle 0c4ced7b6af5e2baf7a9a162ee47401251e80a72bbcd8508ca677ede749dcfff -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mosque 6180f20f295fef3d614ab9159183716d50fe92f0ffbef17ccec3ac3a19a60593 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mosquito c5ac52687600113266223907e5a6e640be12d9d8932e1c9bce2ced7fbe2df29f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MosquitoNet 4eaebf26defe7f0acfc07e51e53d8fe68e04927ed20fb66136eb9f926dcd522b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Motorcycle eb91c01b6866d11eb2cd6f649a0b793102f68eb673025344a24748b333b75c5a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mound 67c6d1d43840092e839cff8fd1897f7f24e73bc760b429550ab60ec56beb31e0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mountain 0b0c17b4fc6ff4aa453aa1406575eafe24df4353643bd4d7a5983d7c481d078e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MountainCity 4f00250d72fcd69bc479b24d790a1437a30bc3d7d247124688a0141158030fc0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MountainSun e4414c740d65ac8dcd8de82f84199a60725ffcd7b37390c49ec837add3d608a0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MugHot ed6d5af331a4b2a2f7d62499a8966cdbbe916aca7e3f3ec7fad58753a6bd7ad0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.MugSaucer 78c00e77429477fe44647fc6a1eab7a1377fb0e8ca55b559dc1fc60784d95520 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Music 0d58a63512558f74b4db365c531b8c7225c80ac4098daace30ffcfda0d778687 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.N 431a6031a81d9af1e3b4a34d829ab1d726be1fd32fde34e1628ebe6479557ec6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.NairaSign 5640803510a63805cb353afe69f331ed982d5135ee2922d7caac8b02f2bc9d81 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.NetworkWired 4e490800df4e8ae261f29ff26f76fef2ef051bfa26d22678574356c1d5bb8c4c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Neuter 8a947544f6e0c95ad099036ccaeb6cf77be7a53ed248e9411cd5f753aece29d6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Newspaper cceb2a43ca91045e627a50a03dbcb0e5fd6c11a44f9975086701061f1e665974 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.NotEqual da05b14287a1c9dbbebe72cf0dabc1deea48480a4a44495e5c506bb81f12e4be -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Notdef 9cb6bb08c07d0630b23c9b43a237dd1f6cf8c694d26cb00117be8ee8b1c8b9a6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.NoteSticky ffa2da220a6209c334463c7d71e12400262ecb4fde626649b79e76fb48d686fb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.NotesMedical 91fd27875e66f23af71c4501a55a087180e06e13dbb07c37c33b073f5b8a78b7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.O 4658b1ac066de5619fb3fefe42f11194262f4e4eaacfb54a3152240449217cf7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ObjectGroup 1203f778a7d39d8207e452e408c8c4a3d478233df72d11fc1d7809aa34c428e2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ObjectUngroup 75d554952af094f3d724230a68154703f0b3b4e90df9470ede00a4c2805cf068 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.OilCan 4f58091672716ac41b7a2bcddb9ecf92b7af2609693cc2a333217a7c81d1107a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.OilWell 6effeeca247f976a793a747778346e809d90a04385fd8077f007998f04372232 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Om 33d47b56fe8c1e6d1fc3c973e794d8f897a3b935084614e4cbac486b869e6958 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Otter 7a0030212ca856dcc111c4e063901971ae4bb83ec40b657016df60b28f6ec205 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Outdent 99a9f7796d32d0fab6f5b9e26fbeb55f3b7f7ba92f422a853c2a4d936ad94309 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.P fd61d099c3a92c9019f28778e1aad550d5269d9ac57e74e52c98571f5d1ca872 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Pager 6e4154bfca69d145bf33df268a35ced23ac046005c693cec033057c028bf21f0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PaintRoller b8288f68e17abe7bfb43bb1c8f481cd4466dd5f7d6e58f0e6a731acb17909805 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Paintbrush c735762dc62e4599cbd3979850b429a9bb283f744a47b638c91f6324f80afb8c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Palette c7f55a4d9932d2ad804623f29ef37ddc2096e928aca191a256dbb56a2d69375d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Pallet 7626230b3414a06d328f85a6fa204d3e76a14f565fe469413de6a7fd5c96bdcd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Panorama 31c28cafff103d73961259728a37fc68b57600ced4cc7b01d676f0007ad80b30 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PaperPlane 167d5c3d51019b54f183b6b08d832aac9006b2c5c30245690b193d1986de68ea -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Paperclip 83de05ddb9fd0c4f06292bfebc984c0f5f19ac364b4cd2db115f1511e815e4e6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ParachuteBox d1694c47e533e73efd65fe43d6db6b035db4fcd56674090e6426b18f09f2fd52 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Paragraph abdbaaceeb3ab31503d16bdfc184f56a6463363ed57983de0c19f70abe90a319 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Passport 0116e8742f6bdf5c2015b11cd5af0d410d88e56ca9ec97862d38d3f0e1b41523 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Paste 90a6a00440d880e6ae83851b7f70a0bcbfe18d2c8b7086a10ae5be9a45ae161c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Pause a45502e0efe24948e1cfdf935bbdb5665c2c9a2063ea4b4a55d997abf2d533ac -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Paw a8450191e9d37652652a0d50e2c807d740b6392b1fde476a509f589e99a179b8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Peace b65948e73e0a7d606ee85fcdce16a0ea12511c5edea4b772937a8101dca86573 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Pen 1dd1cc9fa349fbec3069a026ad00cfbb62c934bf231223d35c614b7e24c8e4f8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PenClip eabc90f657900f77e8c18f238c7076c56b20c1526468f42828737f40874a9360 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PenFancy a0e2c073c0f164681949c6d0e7565498f4b29710aa193fe5116a3196d184846e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PenNib 57d104248189b3b87f2c3e5bda697f22eb490e7245291c35e490860df9422d02 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PenRuler 45e9463f3a11df106962364bb59adca56e65f655932db851c63dd06809164b6c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PenToSquare 26735a1cd6a96d7ce845891507e4640176b189b1f16f82a8a4cd66a8fa9fa66f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Pencil 9d7592f278f096a2f3ad49da8295fe8e094193ae50b75f8441c4fe0038e96688 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeopleArrows 144ed0e0956049d27a75eaa779a3466fba3ed6ca9ff6dbe71c9380719f770f5f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeopleCarryBox c18bff74597a23c0a06ae5486cf529cd6f337c9766acfc72892c2f801859cba6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeopleGroup fb9602baea062c5fd93622d2ba894bce63b107a41baa7a8c2598fe6e9ba53100 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeopleLine 90e1a9e8b2e3bef1a392f4f2ac0f378924fb621d996f5d3fbd30d1013512e742 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeoplePulling 46543a1bc1bc6478624e8ccdb4b7ade05f38145a39f7525e4849d3b2a7312cba -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeopleRobbery e44e1d2f79d3299de5b00f29ad29042df35e6ef0caeb2571d6f5bcd31ccbd6b5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeopleRoof 9d41958d86cf2ac91e66a40372721463eee40e2d2caf8b7115ce22d6f9c68892 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PepperHot ec691a7a2a4df66e971453da9c9888b3895d63b4015f726888969b301b4186a4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Percent 57d83d4b1a952279faf4808c1e33ec326b72da72f6d83508ece23013d4578888 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Person 0811e4f1ed486a97954549e722079546e0657fb059a881bc92bfc45a9919ae1d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonArrowDownToLine 3e0df82af2fcc16389b27546c30807f56c8acee451d328e8a2fe49daf9b969e6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonArrowUpFromLine 9982c237759d78fe92c4925b38b98a82e00c27fce15b0a448d78438c98f7f89c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonBiking b240979abead9d6543537b7159866963738dac5926d864c616a37f8bf67817c0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonBooth 50ddc6139e30b43f5e4d41372d440793655fdbde0e680e4678ea92be75b30e1c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonBreastfeeding 237dac7203124bd2153308e3de6e7f5626999442ae597455629fe8a5ba3dcccb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonBurst 94aae6dab4525c02fd64c54696dadcb3f72c870247e58f0604375da57856bcc5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCane a50d183813646abdf5ccfb24442713c15d1028186793396d8d5d17f746de70c1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonChalkboard 56552d53a08955b5269cddd518195295e4b2c634a4e8d410cd5dbb1fc7e9933c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCircleCheck a897d6a30dee9c8a2e57699a8d5aff550bd1c265861d1f2a552d0e3c8a39fdc0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCircleExclamation c598ac436ad27a16aaf55ec3c255236a80d2543141d90f759a5999d731f959ee -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCircleMinus c0066455afb6368a13db142a293fff434c3aa1e2336c5116d6f6062609f38835 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCirclePlus 43bcbe1e5fc80680211e16809b787535cdde750bcb552a1746f4350485d65c4a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCircleQuestion c3977ea916ecedffbf35a7bf8a456dc5ecb0ed41fd9a26b293b61a3296ebb38d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCircleXmark 0dca25dd00afb6c6d091a43c90ce9cd2c9111c9daecaafff7260fcbb45217e1a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonDigging 7385eb582183b17253acfc9d280030cf2f4631321c080d65676055cbfa5c45a8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonDotsFromLine e28aff957edfe637b0e578183c7b7b8f3eaf673c56d4362e70dda28259de3634 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonDress b9a5f8d3a7ff205589723dcdabb446c9cbf60afd9a62a303b7bf21c8787c2bc3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonDressBurst 94a67ef3a823006694b805a77ab5595f2a361fed99bb87ae7a1acebb75d0da28 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonDrowning d116b6ac543abbb872685c9b476687998526a0da7fd8e0a671e5cc3ee72b517e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonFalling 90433ac37b710b2f73db2c1fc925581f8878c00251a1f88ecad6f4644bccd1af -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonFallingBurst af9c76cb95cb6f250a0600687707e9b501168c6f07c6feb5c81b5fd1e77273f0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonHalfDress 3cbd26f690a9a9137f4633ab051235a96bb4e94142c5db9043a062c419849bef -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonHarassing 4ba372bb7a23fa7b366638e645381f475860fd041b92fea16f3292e67a2101c7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonHiking 8f812e8b7b43174aaef22e7b1e7465439cdcd3ba0b7956c34b0da71734b05a0e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonMilitaryPointing 3ada6f4f39c69dc34220a1801d5282e45f12754fffd9c221b76cffd5fa0f2750 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonMilitaryRifle 48cb4a7f45df57cc11840ebc5722492603661e63a6ed055bbda2c338e72abc48 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonMilitaryToPerson 980b21ad52339be652d2f6c3bb025e935d3c2cfd48df36f3d32b262434206919 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonPraying 65da74b507ea68427fb520de0e2f7765a9c30e352829aef6aee65fde490cac04 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonPregnant a78dd690b7e9d9867421737b579b7a9a8b9f149450832be13136215442aed866 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonRays 85e3e067523057191bd41b2e36990302854ee8b7ef2e13222cdecd27f76be1ae -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonRifle 0b97f21b02d6e3cd476fe018bbf01684c84027e5af037c410c484cc6d734df97 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonRunning 0ba34f0a185def3887a58a36ff4039542f94057eadd31e7c388b0c4ccdc4fefb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonShelter 71e587a89af0a050a4e4cf63b1d801c436593a9d8bc057b8c9fba11f5b6b7a0b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonSkating 42e83bec86e1290278f84c48e5a39978ae214fb641f88ca21fd9b6a62d5dc8dd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonSkiing 8b7112fffdef8d9af46ea49ee1a88145d88ddfd461bea6b50694fbf211c68cf0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonSkiingNordic b8e5fb26398a2f41faba0e3ec8a598f7d56a7e9faf260e136416475f82915b3e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonSnowboarding 1d0603706062fdfe999cf069c996a52695e6a3ca6e12b2ccc4a6fc4343e6064f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonSwimming 6f2fddcfaddcea71508859dff238aab178c92a21e74548226635e25b8419b69a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonThroughWindow 28d5b321457e4863b23e9dc24ebaf49b8ead8ae7dcc76f52a951342af28b4dda -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonWalking 4a96d046170973529c87cd72463f82bd8e5696ca38ba871b18198c2064e9bde9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonWalkingArrowLoopLeft f8ba2e081a0d69ddaba17875c4f2dfe9b0a85c1b4848354e1e67726e5909b063 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonWalkingArrowRight 348f1f0681eea5186cb044b9d29171798c722842ea2799aff10866b08b0c73d3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonWalkingDashedLineArrowRight 5412bf7ea7818a9a0b081e23af8a34f2826ead5d4bdfdb4b77059f3ed216c7ff -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonWalkingLuggage ae7698387c04040c6be154cbc87a00755c29d30bd6c0ae97d3304b26f069635e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonWalkingWithCane 45739cd3582f063ed723bba70a6e03900f847acc2dbcab6f6df388d40c772424 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PesetaSign 940e23c8e8e418a3a3781c9a3e06b5a0d0fb1b1fb2d0619ebe4f1f85eae1b21b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PesoSign b27435c14170940ec201e76c1da8ec5fd4465d54b77e183a13da2de8031294db -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Phone 429c856ac58ff63d1b1cdc201c4c448fcf594cf56c6c0f5101d56e3e12fc9c52 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PhoneFlip f09740967f18bd029b914fc37b97f7600fd529cb9a90521e7dcb6fc14488878c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PhoneSlash 48f77b08e4bb02c55f1c97aaaaf5e52a337a04a6b0bc5f5ac45de3782c05b467 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PhoneVolume d527796f4c83b3626fef07d29a1cf9a44bf9b2240479e2b0a158e47335d41f3b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PhotoFilm d396f09c1ed307419f373c7275c86c99b9023ce20d101f5249affd5034d42db4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PiggyBank c410f7e5dc5e4d2ed1484d58ec3bbb2ad03cf4aad4556609d16f5fc12c7fce77 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Pills 4d58f96de0626b6be102c6b7801a6fba8dfb489523562a3d251d6e21c354703a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PizzaSlice 8892314b8684893a33d60098d8cf05eb83f28b55420922ca9f6e30b5987b2ab8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaceOfWorship 66e5c1371cba64e0ead1db21ee374251e8c45669de3e9e4b5530b250d121e47e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Plane 3b2e08f90fc340438eea191ac421c6dc9f62ac3c31e41105a4de0039ba3303c8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneArrival c81e43e88845c8260a2c7e7f8e48519a4e1fea1d7984e0a0d614355c279ab7ea -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneCircleCheck 8a15a8935bac3b8db8c47a08d1f62b040d13e11eb2093d4b6d98105301624565 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneCircleExclamation 258d5dc5b3307c7097a25264c889f99e42ba19791909f61493482047b3b00fe3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneCircleXmark 59e6a4f955fc3da8182266c99b18c3850420a92181668dbde638b91b6e806286 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneDeparture 78ca9aba59af2662ddfc733592b9db97d949d641c7728be35b4ceafc0809a1e4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneLock 53f2fea2b4260a4387c1a5084d3109f8a6a1260e7148427e9bcbad15cba6fcb2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneSlash 3add4587717024d1eea6a8baebd6cfead1c6eae798562c124072321b751a7248 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneUp 334cdfab241dbc222bb08b06bb1a29065f439e10516bffefb160ee24f28caf14 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlantWilt af8fbeaa74d7959f9b2618819bf896f258f2d1819139c31ea03ddd0cbf85eb7b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlateWheat a7949fe949ce2e30194fa89b7a4a5a8439721d33f1fd7afa98b57c5d171b363c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Play 5b8d0fcb56af59cd1fdd961119f9e5725739e3fe89b00adb8627eba0e5801242 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Plug 954fb857f8e9615158135c302685ccc26e0ae4c7751189cb0f825ff7295f665f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlugCircleBolt 7b932214e387ff41adac86f91065748f0f2ce955a613519a61a99a39f990ec5a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlugCircleCheck d18e4cdaef3bc18b5a59d9bda57225a20799dd321b7c4db7d5ed1f252b5dd134 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlugCircleExclamation e616468d9b8ba2180c51a7a5eee18255c7c8b19eee9092c986d2576dd89b8484 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlugCircleMinus 5a40b974237109fa8b25b222e15dbada9723b975ff6890054603a4aac4d8885b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlugCirclePlus be51b44b52b1ffb6a0d3eca3aff9f731e67ae297e0c39f508a69a1e6cd7a61de -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlugCircleXmark 93846f5265cf07287c7499ca2c767cdc8e0147f14a469f784733e44c5ce946e6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Plus 506ba09a32ce521e07c73dec95f83f60b20ca024e8b12116da7ab88a8706ac01 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlusMinus 77d3528fd718dea9fb3168685bb2c200cc45436d6f2db5f9da5dfa0aaba9afca -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Podcast c17831e2ccad3e42c6a7d71e44a9eb3501ad7118887be315260ea5a5c4d34169 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Poo 9b2035560fdc88b954bb5fefb83ab90d16877b91f2b197114306920c46de8340 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PooStorm f3ddbf53a8ca2d051dd52c719ae24bd178a75d0073c70806ca20c9a0ea95ecb6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Poop 64f97c551b4c569b9431c32b51d152b49b319476ed46658250a30be2c6f15658 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PowerOff bdbf5f19aa881cb78716254be7865e758c6fab74e2d3183bf1467c92b667458b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Prescription 2ae3c6f661329a4083f20763910b24cda6625b132542dfba2f63290032ba7467 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PrescriptionBottle 47a2facf83a62ea79b43712630b07e55b3bd4d6b8d4ed0b41bbcdcb1247573c1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PrescriptionBottleMedical 63c383e52412eb80963a6da879f827a6f247cd17eb5aa5a45439f7dbb92f6a51 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Print 79f9efa1ce4718c935c236542912c80a578ea8bf306c321d672ca08ca15c3a74 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PumpMedical f9e13564ed7336ca608ada2a33e9823b6ac939e591660b759a856f184122c13b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PumpSoap 36ca70577599531315bb826d184b5ca085467f0525935ebf959e91de374ec753 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.PuzzlePiece 5f53205d27b17679667bdc6cedf44c206768ba9ce91ef72406f8b784dd46a4ad -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Q 83c9462e9cd0080a41bb297d7a7e30c6b2891d21747ff83ecd5bfeb786aefc40 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Qrcode 1af56b051dea8369db30d8c3972dbdd944321c6785dfffb1be68869790686441 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Question 92309146ea69f7bdb93a16320865471266c9e6e5bb6d348542752170e6216514 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.QuoteLeft bfba0790bcd7b0c03d0c29e206bbcfe09fe77c3f9a684b82832931064ffa7fba -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.QuoteRight 676930993210349897f842ce466ecb2167bbd0add165024260b975c67b045a1a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.R 6d1f555d0e5368121c2e5b187af4fea44abf2b8bd08adf8d542a5dd780eb1185 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Radiation d7dc78c43ec70c65338dbd44bd690c51c72f28860f91e4d980038759b8c8ce01 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Radio b5c6c01b5185d331dbde8d4265bbd17a2a8f3c706286ce6edf5802ea6cf70981 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Rainbow 7aa2bc633bab5c7f583cf3956db158f7e4b8ca37721cce5ca9c474a14859b1ab -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RankingStar ea54c4d672faaf0361c40d0428fb2ca0d2614b1a4064f4b29a27b9eb0bf08647 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Receipt 1c87f97fda8f42b16961f00bf250990c17e680a44c751f791fa0111d5ad085e0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RecordVinyl b57def18d3d4b9dc7e347e4d06238c395ccb7ea42c98ee9875233765a723ebc8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RectangleAd 74a19fe96c999ed7a69a9aa0b5e092523c4d14d4cdf1789887192c83f624e858 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RectangleList 0302987febe04da7af4cd8cc275c19219dd7a85577a92b5b47092829d54b1995 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RectangleXmark d3bb3517b9ec7c5e55fd23fef1e01006b86c869cb00f27da3b74afd3c50c4e81 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Recycle ad3e5d91baf2c3c6e7b6d00abc4f21cce7dd377128ee5ef6608b1bc5ab88f707 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Registered 6e3a87836274293122efd0597fe0b0aa248e85d82db31c93b82be4f5ba757a97 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Repeat 23f0ce1f24c2bb9abfad5c5df3ce4111a4a5cdd2a7ef5d5e67db43a6a180d8b1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Reply d5a9e205c0132ce2ca91dc9dd9d4a9b9e59876b95913d41ba3b6261c5cd5a155 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ReplyAll 246fcfa55723a3fe3d67e98580724a45345befcb0bc17583f794fc47153afba2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Republican b887b78c976e188f06340f136b4e047488dbefd50319485130dd53cc72047ac1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Restroom 63f498f2d1a1f63ecde846389d399c7788ff13ad3064dafd6d73b1695bc2d31a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Retweet f4b804d70c1d383d7e910bd16c252645eddfccc57409152eb39eb2a2930f530e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ribbon e6b710db74141088a4a13a158d7a670b1408bba545bc59fa6fdf11afbd413641 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RightFromBracket fcfebd8f33cc0b20e156ddfb09c827d4171d2f89a004142920cca708454506fc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RightLeft 4d75cecabe8ea11287928d34f674f266f1ece6d7b006b76e6012c1826fba63cc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RightLong 174f585ac4248848cbea5a7d227e5fa4fa8b6f86af06092950430802d96ae398 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RightToBracket e5a0109b0d6a5c1f658bcb189a2151db113c487ab0b81e0baf137c17256fa940 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ring 473e12c555153317b4a0eccff2978d94d0d2118bb019fd5262e6c004c4908f81 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Road 2e239fbfcd45880cf04bfcf31655ae13d587a37fb11b6d77c40bd5ef3a8e8422 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadBarrier a1c577079fe38b428ffb0e1c6cadf4e7614091192b6ed2671befb93fb1520930 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadBridge ba4ddf47a2afe4b3122af0423c96056bdb80f81aa6793010e3e556b78e018dae -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadCircleCheck 77262bdd6193484af0ec55a8b92c09b3dc5c1ad3bdb4523d307d33acf7e4e831 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadCircleExclamation 7358e097f1564d4a6a9484b45b3a465871cb56d350885ce36a25b31a410f61c6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadCircleXmark f566017565865a1ef32a9f896381e9f9309678606f9b0af9710098a77904848c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadLock 6f06a2b17d691b66c0b0217be8b65e026fd5dbe6382377e1d82edd98a32fb35e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadSpikes 5a61e499b877ecfadc2453689cd421255de2b9fd6754d15712d8bd12d47896ab -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Robot 0a895ca1248bd5235fe0d672d318efff57e72357ef0bc4fe2e141005b564b64a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Rocket da3a2025bfc228fd269f80df8b035e495845c1ee339805bfc1feddc62db2d49d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Rotate 23ad31deca69acefc151ad11f69d5f0440605d61f2bd33df6b42e44d710cdc2d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RotateLeft a151ba413c1e351492371cbdff4ad719ca60689d3b69204abef56c33d509ea52 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RotateRight b7d301c1b24c95506b71c4ec46a80698cd6ce9e7f6e2bb514535c870bef48b17 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Route 076d1269fc3e049303eb9bc85f91199fb4ccf2273868cf95721d024578c8fc86 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Rss a7fd745bf320e6deaf90dae59b115b1dd2774a06f8b9c86231677d5549bad0c5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RubleSign 6bac7458a6dfe475ea2012893f173fa4a047cff78dbd25661465ae2fd2cbbbef -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Rug cd9296c49e67725a1dd2016bb5271ef00195bf6869aee3d4bc21f8a6fa191b79 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ruler 413b1030a5489298d9c3366fb3f4c93476697c86e2aa73c06f8c56fd526c5075 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RulerCombined b3bf878ab175d55c187c1b202afdada17fa725c92cefa1e3b2a158111f9a403b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RulerHorizontal 33ba2e3d585ff9546450dde09cc708618df9309cf4902570171301b15877aaaf -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RulerVertical 0e62f93f393e2ff6aca76f011dd17bddd630dfcd80d4d3fdd2a1f138b1f7b05c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RupeeSign 597d911d0b5e1f3845477b4d8d889d0cddbc8f0eac3e8c1f167117e7d115cb14 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.RupiahSign 7e60de23f93e02b2ed40204e9d8a696be553a25ac69fe6b40a0705a9e2b3f7de -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.S c11ca122b050c26db8a0130b759f0684b21ba832f9597917002a1c9a237c1d16 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SackDollar 57be936917ed8ff6cf375d1730bc9f44aa71467bd6bdcd9d22cfdbc11dac306d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SackXmark c4143c7947d378e308e8dfd64aba285542dc7fc9a97d8b8176e60321b1ac12b4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sailboat 444b66f8f9b003be89c480e50f63189951f71e1666fbe7f54653418893b78bc8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Satellite 3b791f70128d0d3e3503b1b8ec99e6bb06cbb8dfe1b94d59957be46ed404ed86 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SatelliteDish 0d422351fd197fea660b4f40e23cc6af67eb1464e10d20bcb8bed62217d24f77 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ScaleBalanced 9f325fa05dd00e513476184bfe9d4f3257ff688da14a541eba4f977d4d258626 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ScaleUnbalanced f65b7d67416e82e7190e203745a3daf8588f0f222197d1dfccbbe466b760f92c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ScaleUnbalancedFlip 83f8736c9cc21c5d3213195c91645a44ad8682266c6e1ae8120971169df8d6ff -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.School f0564ecd02ddd8574bcc10b3affb362c55a06d00b1ad4f0e7b7e4c71e1421e1c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SchoolCircleCheck 2376fa1f72ff797ca75a4b88a2f1fec57bc12d9296efe3cc8b75396dc44a3855 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SchoolCircleExclamation b42eca8e472afeb7339dc49359523ac9010e99e1392ce03aaa18b8532a4f6df1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SchoolCircleXmark 1635e05c5886415ddb954fee6f72d6e4970e2d8a7e7f3f180345dc873b04c616 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SchoolFlag 639bd2dcc96bf1995a9ebeb7fa9727b87276a203715c9a5321fc90aa340761b1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SchoolLock 0ed7f9556c8a5737ae9ea8d0899d12d0c3ca6022df4b922179eba298dbd575a9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Scissors afd3204e3eaed64716e084d7921d21e9a5ef59efb055328a4d4e2c762cca4da3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Screwdriver 199b2de3fa9ffb9e5339a3c3394961a1ab344c66477d052161375dab774fe091 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ScrewdriverWrench cee8f0882942fc9b897d9ce5cb779d34a010ac080826ea75d30b11734b446838 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Scroll 74b6b114d428e9ae5f796b28acefb052d68baae848b3c97b078a100dd6c6bda2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ScrollTorah 9d2526dff7f2edb44a2cf83087d66a34c91e2ccca114d583ef52ed8a92374df4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SdCard ac87d6e35466343f2d20760092bbaf4a830d5e4ad54362c12db3902eacaefac3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Section 55d2f3d85dc13ab33f62728cec9189a578e3c660020b5d6bedde74f386d13ad8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Seedling 78ac1cb762ee50f773878f6f4ea8c2285acfb08436f19d85be413b5842480b5b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Server 068ffebe59e3e29bb3d7b7865f11909d82aa419e9830356e810ff273ecee582e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shapes b62de0fbfcc2249d437b01a845c7b49422382a249fa24dbf2bef0c19c715c1e6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Share 16720695525c793a0b8e143ecd65815f3df09b8cebab471c42463d472b844f27 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShareFromSquare db4f9e4dd71bb4ca80ce350b0ea303ac3bb75dda62bd364db6e503cfc1dc8344 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShareNodes b56ca6c3254ea84c0cb7afceb3867b39604788e599baf224b6a6f8c06f8a1f43 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SheetPlastic 27c5ea6dbd47781c0e0af2c306966aa865708fa51cb24b9845e3b032f3727dbc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShekelSign c3068a1db23ba091605c4cbcf4a07d0af0a6f248393c3d9ec26c7c810317aa78 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shield 59e5c4f99801fcd9ebf323ea45cf811a8c009481f3827d0914c445fdadf82434 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShieldCat ee8a5a744c7f58d1ff84af2a7f473db0284b566c6a1804d2be6997ef0c87103c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShieldDog eee07310ca8635c7beced84c3f437156917ddc762e138f73b4a244b473814a84 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShieldHalved 4e6cf15c0cdb8d5b4bc85175d8c1a5a9a1d75dddd582c359474a113f91b457fe -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShieldHeart 60c259f50c615f0836e8ff30a399ad2258381a3f91aca60202f29a2b36a4d2d4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShieldVirus 1b400cce604d94bf9dcaac7c8d638155c8d91c7e0acf93206a6d8272e2a6e7b0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ship 77c72132127d12571b7d9b0dd8448c2e51775137e2f34c5433301497ce4e1850 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shirt ef4f9f75bd25faa01d76826d3d2f0599bf33083c669cc22fe143c0d52b1d2c20 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShoePrints 6269e1aeb5226958bbb9c5753878bcd32476b6f2418238f4d9c1a877147d114c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shop dcd42db4bbc77621532fbf4ce63f2d73970d41aad91e4985635f661fce6a403b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShopLock 47cfbdfda5b831d9e9f29c81531cf113b45f26c870f3ca4e127eec80a889dc33 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShopSlash 7aba375b654e2b1663339c5207ff4b633060a79e604decfb469297b892e12bf1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shower 27158122252f9c5d6a494d0692b3b9b94939761352af963c3ea4887c20dbfe25 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shrimp 21b58597f66530461039e62a971353ce5a4f22408b5643f64135b0c5972a11fc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shuffle 9b3c0bd6a59989a98ecc8ef2be948a07c5a95f1c1a937747d3c564c9bfc6dd23 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShuttleSpace 219fb3892742903df30d12b53001f552f1f35e8ddb51b9d2ad74028eb620b346 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SignHanging 3d21d350187047423fb6a3f6ea0b88d90bfbb6ab16ae2145cfbf1cdebd2292e3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Signal 575287e0c6ec19b010b48a21df766f00c661ee82f4491cd27b406397cf33466b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Signature 3b0a04e30e4a0e58575779e28a3a46c9386536a09e7a0279a90e50f27601c992 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SignsPost 45015846f790d581befba74146c9e0b69e53b1ccc1b98584f3bbea8a3c37898f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SimCard f0974e14f98374fc00888dd2208cf93221a884e144267f3df1ca7ce82aebc8c3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sink 9eac0e5c2bac029aaa9f6910bf4cecf695b87920a349e30900ebd13817ada86c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sitemap 114d5d0c63a44cc128f080949085835d99567bced63cb1f1bb8992184fc265e5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Skull 2b9bb801ef98da72d0128277740c7060214d74fce528fa6ebb37c62257312faa -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SkullCrossbones d1ffaae0248bcf52329fe9755f2cf64bb727f6fcdfb3a3595edcc3146296420f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Slash 666eaa032c6a8b3045480d314fbf7ac76f3113b2309822022ad4fbc95fbf62a1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sleigh 9e2693433e67ae0c32f7ee45be14fa18e753fa101f9883a074a9c225cdb61aed -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sliders 3aa10ec470ab6d9e7e298e6f47d806bb8c1ff1f06ff519ba8c8cecef5beccf60 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Smog a7b50537a7198c2e76945ba29df2718297d8a911dc973af41f01a8387e42f27d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Smoking 20ffe207f6a89b9ac39d577020caf59f20b7f6ad0ee338c2c597561c309c5967 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Snowflake c9acf31721173d6a38d9d392a792b4e215fbcdd62b975f44b0eb9333d719d7c0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Snowman 3bb85139209514104977ba82baba65355a18cb9d77eb5c0d75cbf99755448968 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Snowplow 3c07c5ea787a434b08cf7b9f9eb16b23273b3a8901ac6a340b6788057e9a9924 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Soap dbcb2dd2faa2f4622e56ad27493bf8f2dd761382a6e60f750a51c7c765e0fbb2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Socks c2e469e0bcb1aa51abc38c2a865b844d2a99101c283eee3888fd86df371e8412 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SolarPanel 9eff250beeb6794c335348a814fbaa8d463f8ff337208f5f8ab613c896c00879 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sort 942e636824f9033cab9ddcc9f3f574b13066ba8e15b46ef36e77383d87340a32 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SortDown 598e78124d6985b8270f19fb7ba27c40b10f36bc2f39fee814cb0b0b5938d898 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SortUp 06b0ec07e7e0c24a23a24dac4224087a69848b6e31d8fdca50c046758d2f8747 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Spa 468047133369cefe2b22ef4ba248b9da76791a4ed0677d1e66f91f0968fb8818 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SpaghettiMonsterFlying c938b142e2148abde6b509e13cf2b82e08a049f43b45c6ec2e0cc3bdfefb5851 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SpellCheck 516da2208cc2cfca81287add649bbc188c50d3f511bc5a7b4f100970432e006e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Spider e9d09710d2e7ae5b7868842706adcd187375edc733d9fa08207e852d93352970 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Spinner 5f12e2d20125094113e692ab471ee143ead5d204181bd5541c4e84f8c3023568 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Splotch 9a3880aafb5a1029c0752683d36f1bd8dc40d87be55149bb00c1eadeedc2a77d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Spoon 1612fe28e964aeef4d0e0f1be17590b02c817d3e29eef34147cb19e16bcf00bd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SprayCan 6ef88bbe094ef61fecd6f9beaaf0490ed12286b95f600e8a0bb3a55ccf7b13a6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SprayCanSparkles 5f334bdde4d086ae9d70ccfd676309541e61e4fcfe06813a822079d4862ed060 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Square c4db5bc633dc47ec822bb89ae84d43fe0717737de59bcd23b02533828be1d37d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareArrowUpRight 51cc703a54da7f9dd1da4a663bbc880491de18b9a64a7d3e4020cc98bb9acf0d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareBinary fefe735f17b6b43ecfc3cbbb1c3325c40a4ea6603ed9b6be52ee5a27dd232056 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareCaretDown 48e2db1d2da73bde0339b2880a74d93205fe4d6d91cbce17188497e2bb695a20 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareCaretLeft 5ec91df353f25d3f6902c3044f8612ea13ce6ca2e8a793b072a8a4608297c92e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareCaretRight c5bc668e23d0c66e97d52295c5e8c64ceb4530b725f2af632b28df6f60707d45 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareCaretUp 4ec988d80463a1532c689559188fad0392f25300bd95f7158a1a50cf7fa8ccf8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareCheck 42066ee1f7ff389cdb4e572c3f1e3aa2fdc3ba481212a9f95e9ef4df47ed78ff -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareEnvelope aee9ee7d3dd1fcef6e52228e247be19673a50d7c9ffd483e4407ed41f61068e0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareFull 73c28482ddae3e6625111fef48bd2b25a8c68baf648bd08fd7b76f161e519725 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareH 43526d50bbc3d1a191e4786054e7a23044434d970dfa6d0990530e014573bd50 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareMinus d92999cecf8760916e8a59e260aa6a4a6f18a1440cfc83280f68404da9fdc3d7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareNfi 2df043165297f10c78a207197622fce3a14cf93cafd534a6766856ddf587f549 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareParking d3a4cc7fd21a508aef8a957fc6d4e0a0d69c064fc50e03dcde28d27ba8edfd32 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePen 15fd4682843814ef6140d9f3bb662cdf7fa90cccb02ab3b88f23187a228e3fe7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePersonConfined c204f12078ed2a1750bc096d8327b2e8a900f6df7c4a7b6b3cdec034c25cb03b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePhone ebae9b26e8d1606c31c753e438e3e4b62658655fe3d9a6f644fcc824eac36849 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePhoneFlip 2fed84905ea07ed0264215e13ca924726728a35a904114629665d5a69be77f81 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePlus 04c00a614f52ad8500a308961a297cb5af6c93b6279c22628cfe8db6308969d8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePollHorizontal 5c1301af3b00041bffc7df8b73c9b2aad0b4b636bcf5055c858e432f521945dc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePollVertical be072bd71ac6bfe5508717da11059f1d67e5dc37df2938a897bd7b176b257fb4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareRootVariable 26578a18d3944cc57a870036883aac3a1c588411ff39eb5e83d4f6911cb2d74a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareRss 9bf90625ec6bcb835849233f92741fd4cd08cd12e3fc4b606a371bbd777869c7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareShareNodes f25e804670ff9513f954fb4fa42ebaafb2af376b33ef73914087a46f31a0c560 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareUpRight dbfca8b0245cc0d9519d4a64959dee46a640ad658bd97d85e80726277e63cd94 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareVirus 8e90177c57eedfb258a4215b65af7502d96af88e167fb3d8a5fce30c9d702b06 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareXmark 76314ea9c74bce72c00ae1a653f4bce7ca279f7174e947249d07a23537d1c060 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.StaffSnake 6d1a6c974b323b6c46867a0adcc4bfb543a9e69be4d55a82856f1123f045c6a7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stairs 96a11e4ea092ec410d4596c592aacc9c692d358a93342abd5123497471735509 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stamp 1ccceda6c6618d0f2e41e1da84df1d8cf404e5b9688ed60088f6922bdd9083eb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stapler 06b168ea5034da40b8c927f9227198931fb62b2c4ad80f8decfabad00e7fb748 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Star 1de07e6d7f9d3e40c1fcbf4dc7b238bfddc9858cead1048d81b474df28324ae0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.StarAndCrescent e7fd224cd6e6e90a9344ac4e3dfe2bc3f4bc96b7155e34119f5a96f4fdc824fc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.StarHalf a6e8ed611f6701442049b5b5e3e2ddbeb611a666794cd5dc12f22fe8dc9ef7ee -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.StarHalfStroke f8d5dd14e3f589a0377afe60f075c2346f766acf182551f34c5a6095eaf75fd9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.StarOfDavid f03704d828c6dbfd894cfbbbe2ac941b1ffb21a514b2beb4daaae4d755828419 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.StarOfLife c1e65d0b675f19a142b556a2fd4b28b1cc72362dd374b54a3c65e1174e8527e3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SterlingSign 6fb6fa34b018edabd268065db6a3f5041b93891fbeb0cf4a4e34597bd7c77c66 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stethoscope 2cb5c52a1b4035326ca17ce8351f5e7f6298b26e9325017ebb4b6bc0c80f8802 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stop 28123f9fee83eeec18553aa5510c5327c8bb71b18bc6a9c29583da0dd720acf8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stopwatch 0452dcf33a86bf5d1c4405a8e57d5365eee5a2f18d02d0ee9998ad6059fb5566 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stopwatch20 d09969fb914db1bc5e6c815afdf60c6dcaf73bfd615a8517a71bfaf16504d7e7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Store 71aa5ae3c6ad75769c8553a25abc255954f5007c1f4f70988f475257565c4c86 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.StoreSlash b116c8f0ad89e1fd387ddd623961be3caa4c10d9a99659ae61a60bd37ee34398 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.StreetView b9318d53e3b717c9402c77130a86e3f40c3a2f5ff5ec403e02bdc7099e43e4f2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Strikethrough d380e55106be7a89d8b31366e44127de9932f52783778ff2bae5c584ef5c7a53 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stroopwafel 13cf50c29545c6bbcf8965ba790de11b1f1ed207066b4997e38a4655df81bfd0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Subscript 50a6ce7def7211b11d28a6a751bd0b158c8e02728fbb04e0c62e481d7f8c02ba -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Suitcase 529720aa9f6c1a2eddb0d705d846177c8d6ae3f9aa36be8e1e8b412c0a1af7e7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SuitcaseMedical b094381f7263c5eff5b6f6698eb41c501724ca2d44ef183487a35d2584ad2796 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SuitcaseRolling e08757682b85bdf39801c93aeda48ea6a744032c755d67e1a7bcbb252f96824b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sun ff97a838bc46084e70e039dfed42522808f4ff4f8501f9acc68cd44ff4958c33 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.SunPlantWilt 8fd8170899867a0cae0488270b5cf3d9f4fae164f68120b2d38559e5f408002c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Superscript 8ce8c5bad6336a6cd617a816ff4f5fa136b77395c731bf9c9381f600adc3458b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Swatchbook 38c51f6ed67328354bee47cb07a3efeef9ff52493c31a188f3757850af58d688 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Synagogue a028f4121f1fd3bf9523e7d690205ab95a5db2c3fa3dc8cb2602e288bea984ef -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Syringe 94829e8a03e7dfe5b25f22839450fc3cb8b93a7cd594aa0ce9a373ba82544ac5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.T d2d85e77658a805891a4140899bb8a1e8013a4aa4f23be2d15790b402a32138f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Table 29feac82c89849f83edee2eea587bd9c56b2ec168ed4e4404aeb5277f23414e5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableCells 569f223b4f4da45f43a90892edc2766cd1fd7c97078d5b69f9fb5515dcac3013 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableCellsColumnLock 590f16f8160fe5b2bd1360f6aca793b9a218d67bb70b1745206f5d9d3a0c7bea -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableCellsLarge b32d3659b55fb6e9bcb85eebc187bf476c6253c0bceca797e43b2b4df2dadfc3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableCellsRowLock ecdeccec35f5ca6b9a71bf1a34fba9638ec51ca29f1d419a4dd9bad7a5f0ea9a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableCellsRowUnlock f1a0dfa392d09da6821857cdfdc9ee8c8aa7bfbe9b90b65caa14325d1908709c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableColumns ace46a2047136bf33ab3b6bd8ecfd78e1cb01deddb1feb6aa5b9dfa488788695 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableList 13a73e1cdd16f741413a8356dbb573ca66a968ca20defaf4057398c84232b68f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableTennisPaddleBall 11613570794b9a2633c732f3df530e0bc7f407f1032c3ebb373288c08d1616d9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tablet 802ab7c2f3e7b77bfa61efcf0d57af64c96d42d3954880c4b8d2d12e65fea9b1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TabletButton 1bba5f7b194ef5d04c1955cdc7549471450cf242dec4c6bc3d1b141f058c5167 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TabletScreenButton c7ef133999989cf29a1141e4c723e81ef46b93363fbfc754bd8d17e4d32f2d7b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tablets e96c1e45b64e27b140c7cca56ac7af4fde2bbcdc184653af6e5496679b0a9e7e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TachographDigital a282fea756e499f6c58fc7e84384ec52f93365035b32af3916b720377e4771b8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tag eec391c3773fb0172c4f9f20fdf038055ea702db855aacaee370cb3d1449496b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tags d33e4eeca5bf9d575b15dce071514344acf1048c5f99f46688e7a464cd4766fb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tape 4d0e6e3fb756336d0aa679f201ba024a7e256f0547119b07fedb7decdc9a4303 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tarp 822f6c9c5a79b69c023308bb1ba0f13d69853eb713ba2173d206e140d2a9a7c7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TarpDroplet 2c5b89808f68dff2d0fcec7676d468f6c825510f4ebe1e1574ea2d88833bc13b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Taxi 9c5c412f6460c3768b0455d54a579d17f5d2e0d02e6f00485156c5e6bb3fdb68 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Teeth dc55428cb9cf230aeeeae2ee73e65748d6f3021fdb938220f33c918fd73aeea7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TeethOpen cd267ef15fa86a7b131a04f82474f9da4a07566c0d3ff8e44e7db62a422bace5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureArrowDown 3fe0e05b6ae19cad65299c8ff411e7e46826b6fb233e8690278c5f0851ce4a4a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureArrowUp c7325a7fe1dd5b5f00faf9544c82aefaf7dd366f295567360d726ec65b4645d4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureEmpty d5ded5727169a7985dd165549280d70e1deef850c5987aa2876da43b67376e05 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureFull 020a226228d769c06b04ec6536f06a255fc6d22dc8f08033fe7f57b769d8bae8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureHalf e5af743fafa2e347748c81989b12f203718db3788c78c8ecca493c2b85b3fe51 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureHigh ce144e3910b7a2d6431323bcd5f00e52bf889365157dd8e0e757dbb5bf55a4c8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureLow 4b3a7afc4cbb9e97d0ea630f879f9ba3c29b58de7e75941495ad7ae12eb38900 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureQuarter 7b5a0c690ef910108c3f519f280a8af5be199828aa4340b6da7bbef13e91139c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureThreeQuarters f71a8da3df5fe37abc6a02785bf7687dec4e8bda9578d2b32b5fe9a10a749a91 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TengeSign 6f677f3aa1a536961b64b1448306338adac72429dd539b86c356ac48649f2032 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tent 5188bde0bb8ec3bff01e368e16d8d2c6686842dee18a3fa3aa537608d569422b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TentArrowDownToLine ff901be1001f795c58d1c447121c69d79b6546d3d0d05b72bb9ed8fe8c2408b9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TentArrowLeftRight d1b7acaf39d60a2ba449990ca81b0fbb69a4f1fb764416403d6c97fe35032d7a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TentArrowTurnLeft 3346ed3931c996e4c2b56e35d86de5a130e035e16d2c6d4240eb1abb97bda68f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TentArrowsDown 8295951c84415578df1d6ea74594c1e5c153d135b4686893264011eb49084e51 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tents 58f8443713a5451aaa7d3a0af9a97d0f75b43240ea52a3a7f5322753c67741bc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Terminal 7261ea978b9a5e744647f4fba3f2ae0b1c6533d7bf4041d3731f3efd11894b58 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TextHeight a2b151fcc504ef4069ce46ef10d5722428ae5f47ad6188abf0561b59508b9038 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TextSlash 4931279114c28b4d2ed811101dd91336d83b6363fd7e1331872a47b8533a0306 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TextWidth d047dd46e15462379f2624374177d2d38726fcac1d28cb4e8fb920b2b222356c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Thermometer 530a5f03cf73c23665ac659c22db363b7cd01283bc5e42a2d90471ff44ce1fbd -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ThumbsDown 222168cfa2dbc72e7465fb871505f40f7d2e2e1091118197a692926b597524f6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ThumbsUp bdd301450dd6b98bb66a81fe4fd8c31d9d18e05a16cb227b3b90e6b34137de13 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Thumbtack 9e47d5b365872569ff05752c74fda35e3a49f88bc0af1634a677bd20c2f57c01 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ThumbtackSlash 1d3a71a3b02459c310489447b714f2c46116b0ecaba277f5bf45624679b5ef54 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ticket 10244aaffcd470541c2b575404dd843b44f28933e2480fcc81c826b432a28f2a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TicketSimple 3da678a638ee640ed713cc5b41a99760cb34caeebb48ec06574ed85a4d4d444b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Timeline f8248be26631e4ffd69f5256a37e3f7d2f1ee192b57f3cf4bb89072c916e3c08 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToggleOff 1e2814e622242c147533cf0a154dd87efa765a7e2a2d7f06f2a6debf128dde5e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToggleOn 624934aea47683573fd7e674837945e580f5eca71460c2fabbc9215a5a7e226a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Toilet 07ae014b23cd8e74d57e0ce0e8647f421ddc831d43b3be72cd97923df03fa259 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToiletPaper 50f7c73bf21ed7c82700729cb387be0a102f0cdebb0a23898452b82dee3b3d26 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToiletPaperSlash 771c288f2c312619d7fd030ad19456b9e7dbcfba4a857f38ce8e2789b855e5d7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToiletPortable de74755665766e4c80b71e227848aa3a3c43d159c416aed177adcde090620d81 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToiletsPortable 3be5b56825a63f4f465b621d0c468f8d0c17b4200a14d9bb7cfe1143b54e4a82 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Toolbox 8fece5df6f4af8e19b28dd969c96455d379915a08df2042ee99bf8f0b34b9444 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tooth 23c32a28e60d86a2960166d284c17913ab8a8dcc6cc71ad7e9c698ad7beda42d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToriiGate dcbc0a2efb2cd72c6c35bdff68544ebdc879ee6512478f5df8bebd0a7b6ceb69 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tornado fab0d8175fa31633868ebf77d904b2d1ea1254ac27c9126bf7ac89d9bc99dd7b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TowerBroadcast 36b7b694e51c6791edcd11c760a4f0ee9b0164107a7a669a1c2ef8f4848a8bc3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TowerCell 04cc893851e7f76ef5cb0733e746984067340f597be6f53285878d8b2967096e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TowerObservation 295b63ba6830f681a5c51cebc58cfeef24dcc0ee15de775b84c27bba1effb109 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tractor 8fb5d58f0db489ac4245e900b77d60a459cda0a49dc97e75696209576a4dc810 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Trademark 61e0d30700aa6ca474a815115ac0a0dc314df969e8139d3ec5b5f60f9897e1f4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrafficLight 1f5a25ca4806a84326fed9662ad30f56ee8f41edecf044197d729b54f87592d8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Trailer 8901c3f702392335d490091f61d16af9ae85b9f566aaf99aa536efcbc1302c82 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Train 66fd832f0a88beffe6b116acadede20992fed8446d41ac49b08832d3b6527a85 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrainSubway 9c85a1a13ad8999528aa6e457a1bfb72640cc7644ae5615c6242b9ebae06b992 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrainTram 0594f5cc450672d3aeecb796b65051beb7e1e40a9e3c2d2ccbb4b8ffca3c1789 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Transgender 86a434230b817bc51fb102ee46a65791683e19c2f4e44b5e16bafd3d318b46ea -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Trash 45b17d9232986998f4094833e6753127a5683a26fee0acdfa42deaf16785cc53 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrashArrowUp 97430ca0e284a2e69f7cea18897507d632deb22e84283de911dd94cee90da2e9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrashCan 5882998da536ebf7ce67f0557e5fdac425fb8617a64b59419d37bced717cc8bc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrashCanArrowUp 22fb933ecd40ec3cd0f4e4da97516e6e1ca663d52c3d39c12ce8202ccbfc951c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tree 151e25978fa486dfa7b3e391aac1423b13dd792319e4a47d30cf53c523e1c37b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TreeCity d31ddbcbad33d0a0de2f1b06c3eefc29e9433728618e894fbb7aab4febbca7e4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TriangleExclamation ff6b47b300dc8b19a8ffcddc203a7b782dda2d62d23578730d9cb893b8c4ac84 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Trophy e8e11ad3b22deb0fd8567eceeb976908b296a57cb15f94d052de50189dd5f061 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Trowel 505333d3dad804117af87cd3842ba9a59f532e46f527b4b94655e577444a6254 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrowelBricks 9898c2a156a0a8f75255a78a1b0f93d566f7ff71b3c874fd48a4c3cd683e3aa5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Truck e3b0db9fd4f89dcb6b6c49229e8baadbda527816d1aa0711b3e4981247cb61d5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckArrowRight b88c3c4d2cdb3629572a8e41f81f40227991fe1db5dcb9a9518107b9aa63b68e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckDroplet d6f5af69af27d78bc554e3f5d69d8bbe8c764192cd5379b9d90554f02ca52fcb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckFast 261f8d7810e7c074c3f65da3d646ae70b2f84063cd77800d47d7509206002981 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckField 45e274e78797f086e85d225558da3864fb33241ac1c40384d834fd1d1bd5ad34 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckFieldUn 9fd28293fe935a56be28467229b1c61d16f73d06ea18afe30ed6adc8b1d726f6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckFront 0e00304a0073f9b9e1f8bfd706a2f115dea872dbf9c339c036ff8bfe17ad889f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckMedical 9ad969879cd33ebdc0cde10e5ad2febfa0b6eb6d1dac3f6fbfbcda93d2cc57b2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckMonster b621b979e457c1eee4918bb81fe95b93b090950de703aaf606cedc17dba83f48 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckMoving abb3a8a989d15a8b5819dec77ee900740df29aedc1f9f13c3ff94b25a6d02ac2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckPickup 8826c13760574efa8824b302ab710941e06be4163f53446130cf2455e99d6163 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckPlane 5d4133bfdb49d489d5187ba5bc54023d26ef39469936abe824f049e578246b75 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckRampBox 364e908edf2409265c00c93ef17fd89ac1104685e3556f4fdfa0905c2c80dccc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tty f9dc7c9f4ac7036d6aa91e6b4e11c2ff3eb1181a753c6fbb92c6ab17d474d650 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TurkishLiraSign 0b41de7553d19cccc35c0cfc928701836fc50b3ac6df63fbb4625c7aa05e20a0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TurnDown 3128d1f581c27cfeca2b0405e71073a8f01a476315b139dcf70ef80e43835c1e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.TurnUp b5835ed0325e039229c8bc4d547c8deebbcc3d52cd691ced595731852f568858 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tv 67ce8ab61538b51aa6173716cdbd20cedabe8ba1c18c9999479d9afb7b01726e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.U 5e2deefc0497847a628596cbef85f954bec8e28274993aab606a7cf66d124aa4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Umbrella f0f3230033f71f5fde03a1d13a9bda536068e64e79bd8a5d4006db5703f9f463 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UmbrellaBeach cbf79feef057b50e57bc13f3a06467703322e4ac2d330821839e03e2374d4a18 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Underline a71c6983056b4a4db63182c9a6cc61ca217f2309d079cddba44213a6acc81140 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UniversalAccess 0be8f01d75d2f37edc8299798eda07bf43706eb67c12f3d3673dde320a167716 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Unlock dfb3743e24836445ba76215698a1692830139cb059eae3dd55d8cef5f1e2d5b3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UnlockKeyhole 57fa983f4c607b25941fc6602030dc84667be61b634f6702003928d057430d71 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UpDown f7f783a54eae8754ee324aec232cf248b74f33588f3c591415e91664c0ca1523 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UpDownLeftRight b3ed42bcf9adf5c2e9d06b0bd732f88900664bbf48a6f43e0cc2279bac61aa02 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UpLong f764802a9b34c785db8ce6966b88d7d357d8b517d6c08ea8624f8a86b45771b2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UpRightAndDownLeftFromCenter 38a638610e7feda5fed163f3e60e22f294dcbec177971ba5285860da52a82808 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UpRightFromSquare b10e28104507ae15fd267bda434836ed4684ba3f15904cb5c24746ac14a167ea -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Upload d7b8043caa9eebe0078a23540333ee3785ece098ce6de3149b42550c2c391ff2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.User b06a62418322a078363daf7ff99b3bbf934f6ca02f2f9f5edddaf1a704a567e0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserAstronaut 51867271b71d1e5b4280057c41bccd8d293899eb62493f4dc5446c6c30dc31ef -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserCheck 8e826de71821f06f1058e2310970f737bfc35007b09eb1fcb6d563061905eebb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserClock 85457eea6e50041de22a13244384c270c83c0a6719dc6b59497d7c1f7a391d79 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserDoctor 718aaba5473b0bfbc583f90e98c0bdf9fbaa2bfd55e241bf891457cca7ee79b3 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserGear 8f1dbcf02e2850f98241fe7df9b770532c0459c9427120e1fb4d1c95af84ea71 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserGraduate f122aafd5430d4a1487fc3f7aea762a9893b9409d59935a371698358683fb898 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserGroup bb3b47fa98ee594efe3e923df43b6c10078d5ba47b36c25de8ca9e9ba5e502c8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserInjured b84685c9ce8e867fa88a603596b8cd7fe037e2dc4a03250a7bdd779096db365c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserLarge b77ecebacdb5e7b6af402f5518914c28b699d80077051de47423b6efec2a3b23 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserLargeSlash 3f9eaf8ef61d6a67a3bb6f378ab73f452e4338c7f3bd2ce4e5d5662495c39625 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserLock 7320acaec982b518ad6985d2648725ff53df781eed1f8894484472d91aab2788 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserMinus 712faf8c5113ec10dbd745d862941be227d88e4cfb3ecafbb083ddac630cafe7 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserNinja 304e2f8ae8474752815f3f0aff61c953861dd030aee91a19557aa51dc22b0693 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserNurse 1984fedb0395574ba595b53ab92566eccc16a6cfa15be884a099e3ffa6dd2322 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserPen 4001c267e5cd9bd33c95ee5e054350e92d5b1c7517840c6a78535e8955a51308 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserPlus 01fd855b65054cfebdd7c70e3f198368cedd83fc6b74e16d849e04c00b010a7e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserSecret b6a0125f814a5baca0544d4d2569601889cf5f57f1962d5e6afbf2ecbf5592f8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserShield 4ba01b66819157fbe92911bfc0bbda695b2b9ea834b6264060b7940134992dbc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserSlash 24f93690b07e4673f69ae73dba6d0c00c4e76da6497703d3eca7a459b9e26382 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserTag 99319e96345fbdcbeb665ec665a0a8d33cc5d070c968b4a15d7ed43293b7beed -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserTie 85304a6073b0e362d900fbbd35f98d0be75c4d13eea204d14125909e52d1d9c8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserXmark a91af662d9517e1383be033c03b71c8e79da9060766ccc34a184406aa0a36fd9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Users 6f7d4fddee1156ee544da18661659ba1b38fedd00e1393e35f6af82962196806 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersBetweenLines 2c72295e365776c5c03fc79204fc81533dce8f25d883638279d00b6ab91e8843 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersGear 4d0d677bbadfcb482081ef761e030c481519e3662eb1c1b6f0d4b41d5966a374 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersLine 5994640e8f9a2de9fb14144fb52b92d71ab56ab607c1ac505cd6c587728ca2f9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersRays 28d6e5ed80fd7d71f8b30927b9b6d818755d1e6870cc803f4597d2ab6e2a9dc1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersRectangle 12c29bcdab3793a93aad22aa9c94183926492b3040799c5f38e54b1c15dcca3e -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersSlash 010c4f3934145c32fe8b3ac454bc6fe3d90844c32c0a18c020ca10e4f96a9c32 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersViewfinder a14d5b981b7939c2a46fb79cdfccb03c083c36afa35f310f3deed16d320bc556 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Utensils 0714245793778f5d8bb6a1e0253a566e7fd1badf3d08e6d74a37a85f4183928a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.V 77ed9bca0e6fc75c2c15e5dba33182d0b277067a92598ddad742aae83c7594b0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VanShuttle e3fadca61feb7a9c4314a4f1ee15443421948b24a50771603cf444f86fbaf966 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Vault 2589da297cd579c69299de6dd06f7d428f1a787827bd5efd89c1250bf9c0b271 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VectorSquare d9a7b96ff2ed15a3f31db08a00ac14f5cacd5121c8cb65bae1f27c8bc19cb9d6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Venus 81a3d45703c314c27cbad7bb5571c79e9289f265785de7b2273deb5b9864f13c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VenusDouble ad5a2ef4198b580849ab18837ac6db71052928767769ea42182b5ca4c41cf079 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VenusMars 8861d51d09dfe9eef35c420a29cf014f453225d271e0dc8dae30050fec1a3636 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Vest 8983751acc470706cb2bcde6a860dd869dc08a26a9d32564258efe5107175b48 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VestPatches 3684e74ad5923bfe57ff229d3c6d2eeb81e76ec7f5ee2f595719366ee4cb0140 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Vial 8f980ab53547eba00fe17423b6543342a1353c7f33b14780645f429a034cc7cf -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VialCircleCheck 4ec17f6bd7709fe9ba77b9abd7efc90f4338244949fd51b6ef01a12d27b4040f -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VialVirus ef7859a333ae10f927577fc5e727625a80d634995f2f39813ce745a614a28693 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Vials 172766cd00cf73fba05e319b603b358bfa1740d4ed407c5cc270b0b07a8fe43d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Video 91581771a90e15e26492e86efe6cc5d6b3f9d835fabfb970892fd56f10cb2212 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VideoSlash c5862ae79d0fdb22d1d6744f6a4a7132e9727e620a9f9f1e3544f1d3ceb023cc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Vihara 68546686cc56a558d6ecbece333030dad43a380204c71d2031b1fe508f80fcd6 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Virus e7feae1a3c18f8dc1969eddc07bbe8fba4f70d639a323bb17734d0b1c6462450 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VirusCovid 210013793f6cf215a0c1fb01fde65b3446622ec4bf297fdddb257058830eab87 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VirusCovidSlash 235ae392a1c96e6cc13649c7b82fb1512e5587589368ae8beab1c48bbc408029 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VirusSlash 0a4a79a70962554e7bd84fb8b4bf49033c7451e5093ff7449ad638c1704e61b2 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Viruses 3c47601215f262460080b18173f494f3316b6deb946bb00eff859bf6b3ff3d97 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Voicemail c63cb5f11d4c927d1a5449d1e491f3057b4e822b045c06ba3706a61b54a2cfdb -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Volcano 9efc89bcb995ab7c044677c6d1bfe996eb46d06995d20b4ea632e62cf7840878 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Volleyball 4dda73f1aad7765a60fd8d1674952ae3580660b5ee4379829fcaf305707882da -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VolumeHigh 4db2c5d647aa37b0c41dcfaa1b85e11555a0447da31860990f40f04f6f7ff96d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VolumeLow ec773790f03bda6692d79b932b48bac61ec94f174cd9ab24279b23a5d145d801 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VolumeOff 9c7635d14fca703daafa3fe3b8fb94409a979cb8bb930579d04bb9fe88dbc910 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VolumeXmark f42df2f52ca1ed48e36f33f1bb8a362b4ed49f18def8663a5d4af817fd145276 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.VrCardboard ec1742027442bd655ab0f146ad31a1aa81ce6644937b111768ba656d6d3f7bd9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.W 6e80f49b316296b157b23ce24ae89b3e55a71df3435732c77481a6fe8277ff73 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WalkieTalkie 6ef035808fe7181abc758cd1bbedab084f9ebf466ea70a1eddcad3f6a0710e25 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Wallet 3bcdabc26ba48dde28c783ae05d553399758dc70c2180b6f29954540d6bcf2ce -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WandMagic f959adf1b4d9347be027d853d816e2136a1b43394b65810fdd96a7fdabffb6c4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WandMagicSparkles 8b82ed1dcec2eb1481ac06305981297616547e51e2b0f9b7cef3fb1dc5a30367 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WandSparkles ce06b41b43b4adff6ecb0b7fae6607f13e2fec43a347c0bac9fbdcb17d16b428 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Warehouse 32a9d0e541a5464a07e04b87db8669c50c12576ea2a5643aeaf66892ef7a46db -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Water cb45bb80a9ad7202321ad1caa4562d39c1707034ee4655d9ef7017c628f6aa5c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WaterLadder 834b3cdf484058e2d4ceeb93ef44d4dd31c90582a4e9af08142c52fda219edb4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WaveSquare 3a53ee2701004d506b67fdffe73f47e683ab418cacd51b09247737b93ca182cf -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WebAwesome 30e0f1290a6df2264653800c1eabf981d7afbb476a3eed597f0239ab1e146c6b -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WeightHanging 12c7bb4530cd9be6f3b45e4e6f5b2493b82f55576f75267b6209ac2d2f678f71 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WeightScale 3bf0ab62d234b7d74a9d90e7095cef3c8e4ad0e4822e3946bccd085d9e79cbd9 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WheatAwn 77f2b167c8f41b4a752b75095bf3db41daafa74aab6bf421e0510ddea7cb7670 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WheatAwnCircleExclamation 97c642fe06e398d141338f60e348b763fffb7ec35a3d81453b53d91012417376 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Wheelchair a80f3c4d0f8a8e9b8f26f73f19bbe39fc5df0d1353df13c71bf513860f3441c8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WheelchairMove faa6aa02bc18a03ea12c49e778d7ffaec78ff62b4a7f281e4ba872d0ee1cce11 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WhiskeyGlass 6e2683e165ec380d6f76ea9b1d26a30bb98712185c17c393ddbab0d10c93caea -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Wifi 6c844c7cf0a8eb7e58cc5564f3cfd3866bb80f15a7387f25163abbeb44691746 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Wind 382cfb3b19b1eb6e40373b18a6bb169b6b69e146e8922085712119e96bee99e1 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WindowMaximize 915773950a39ae2bc6b2583fae27a60fb0b48fa78e8548a8d4c4c88b82550d56 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WindowMinimize 80c67a3d7a3f2bd6b7657d8bb7886d515bec3665c0a655c9f65a80eaf3c7709a -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WindowRestore f701e44a53f472af07d5a0b73ecc55863a1573225fd5dcb72778a7d1c7b5f0e8 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WineBottle 4e5170efccfc3388acf6c2b73846466e4bc84b012a937b3c7f70ada365483861 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WineGlass 523e98274e84dab2620e79024cbec588db4e61c3062b9570eb389e1ac3f7ce39 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WineGlassEmpty c74fafda90746b920bd7adbee8cd9565b5a7b34d0ced88a8519b7497908cda89 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.WonSign 697c7da15e5269253d84c0b60af3583f92995ce89a03edd793f5286cfcfa2c8c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Worm be3916003cd593099e63fe8b4fc936a30a1a5dfbb36158e2d64ad13dc2dc168d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Wrench 667f014dcb4cd12ff58d7bdc265954e7e9ecebe40bb2c08bc720334ef2710bf0 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.X fdae0c25a3bedffdb46258c2d131a3d36dfcc1758cc44cc3dbd7a0a30e6bedbf -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.XRay 1397a6bc63a797810fbb912b5e0bb0b453946d15a90b7837a2f8e842e839a9e5 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Xmark 3282c6b21c2f63eeb2062518523475b537baf429ff15d362b6dbce6f56d882fc -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.XmarksLines ef02b26a5d24183ad23ace0d8ba1a576ddd9f8bca4a0aa7d34d6c1ccf28d9d3d -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Y 703778026d3cce55826b08618e4fca07db078a4e6111d78ecccb37a71a39d1b4 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.YenSign f7654067d0bb0ff3e5574097d5c30d1cc1eee2ce3c759d6725c848817dc2e99c -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.YinYang 1fca122e7ad4d355b45e5ae7ed7e82c734eb152d68b635fd9ef870b86e7ea655 -->
<!-- api: F:Inno.Adapter.Presentation.ImGui.ImGuiIcon.Z 803ade85783394a5f131af6606d665a17478c2224c1d5baf04737df207058198 -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-a8808ec3a4a895ad"></a>[`public sealed class Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/ImGuiInteractionOptions.cs#L8) | Freezes product-selected keyboard conventions and horizontal wheel interpretation. | — | — |
| <a id="api-d3e884622286d619"></a>[`public Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions.ImGuiInteractionOptions(bool commandKeyBehavior, int horizontalWheelDirection)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/ImGuiInteractionOptions.cs#L22) | Captures interaction conventions without inferring them from the current operating system.<br>`commandKeyBehavior`: Whether ImGui should use Command-based editing and selection conventions.<br>`horizontalWheelDirection`: Either one or minus one, applied to the platform event's horizontal wheel value. | — | `System.ArgumentOutOfRangeException`: The wheel direction is not one or minus one. |
| <a id="api-36d0a0e2b7051adc"></a>[`public bool Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions.commandKeyBehavior { get; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/ImGuiInteractionOptions.cs#L35) | Gets the explicitly chosen editing and selection conventions. | — | — |
| <a id="api-445ecb755d66f6c6"></a>[`public int Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions.horizontalWheelDirection { get; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/ImGuiInteractionOptions.cs#L40) | Gets the horizontal platform wheel multiplier. | — | — |
| <a id="api-1238761f5754db87"></a>[`public readonly record struct Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle : System.IEquatable<Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle>`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L11) | Identifies a backend-owned texture through an opaque ImGui token instead of a native graphics handle. | — | — |
| <a id="api-f549b5df6e036775"></a>[`public Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.ImGuiTextureHandle(ulong value)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L19) | Creates an opaque texture token allocated by an ImGui presentation backend.<br>`value`: Non-zero backend token. | — | — |
| <a id="api-9edefbd98e821ea1"></a>[`public ulong Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.value { get; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L32) | Gets the opaque token consumed only by the active ImGui backend. | — | — |
| <a id="api-0c18d6053b5d6b9b"></a>[`public bool Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.isValid { get; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L37) | Gets whether this token identifies a registered texture. | — | — |
| <a id="api-eb437cf7a4ba7988"></a>[`public override string Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.ToString()`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L11) | Returns the fully qualified type name of this instance. | The fully qualified type name. | — |
| <a id="api-c329b3979315d734"></a>[`public static bool Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.operator !=(Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle left, Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle right)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L11) | Inherited or compiler-provided contract. | — | — |
| <a id="api-17f70689e5eac4b5"></a>[`public static bool Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.operator ==(Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle left, Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle right)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L11) | Inherited or compiler-provided contract. | — | — |
| <a id="api-28d53eda64fb9407"></a>[`public override int Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.GetHashCode()`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L11) | Returns the hash code for this instance. | A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-ea510de83c99ef80"></a>[`public override bool Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.Equals(object obj)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L11) | Indicates whether this instance and a specified object are equal.<br>`obj`: The object to compare with the current instance. | if and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-399bccbec047835a"></a>[`public bool Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle.Equals(Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle other)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L11) | Indicates whether the current object is equal to another object of the same type.<br>`other`: An object to compare with this object. | if the current object is equal to the parameter; otherwise, . | — |
| <a id="api-2ac9adfd730a8834"></a>[`public sealed class Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L43) | Describes one detached ImGui viewport without exposing SDL or graphics-backend handles. | — | — |
| <a id="api-88f7e3973dc70a48"></a>[`public uint Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget.viewportId { get; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L62) | Gets the stable Dear ImGui viewport identity. | — | — |
| <a id="api-ff095bf6f493f350"></a>[`public uint Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget.windowId { get; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L67) | Gets the platform window identity. | — | — |
| <a id="api-f70f5c8400c14910"></a>[`public Inno.Adapter.Platform.PlatformNativeHandles Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget.nativeHandles { get; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L72) | Gets backend-neutral native-window integration handles. | — | — |
| <a id="api-6b2901e9763b1fc0"></a>[`public int Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget.width { get; internal set; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L77) | Gets the current framebuffer width. | — | — |
| <a id="api-30e718337aeab682"></a>[`public int Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget.height { get; internal set; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L82) | Gets the current framebuffer height. | — | — |
| <a id="api-3ef9a806a2b2a92d"></a>[`public interface Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer : System.IDisposable`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L88) | Presents ImGui draw data through a replaceable renderer while platform input and windows stay shared. | — | — |
| <a id="api-3411a6b3305836a1"></a>[`bool Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.supportsVertexOffsets { get; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L93) | Gets whether every draw-data consumer applies per-command vertex offsets to 16-bit indices. | — | — |
| <a id="api-34fceaaf545ab15e"></a>[`bool Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.supportsViewports { get; }`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L98) | Gets whether detached viewport windows are rendered by this backend. | — | — |
| <a id="api-69f233ee5483f99e"></a>[`void Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.RenderMain(nint drawData)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L106) | Consumes current main-window draw data before the next ImGui frame begins.<br>`drawData`: Opaque native ImDrawData pointer valid for the current frame. | — | — |
| <a id="api-76a4ca78b2bedfb6"></a>[`void Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.SynchronizeMainOutput(int pixelWidth, int pixelHeight)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L117) | Synchronizes the primary drawable after a platform resize.<br>`pixelWidth`: Current drawable width in physical pixels.<br>`pixelHeight`: Current drawable height in physical pixels. | — | — |
| <a id="api-f0adaa2ca9ce92d3"></a>[`void Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.CreateViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L128) | Creates backend presentation state for one detached viewport.<br>`target`: New viewport target. | — | — |
| <a id="api-5fd8a89a21d3cf5b"></a>[`void Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.ResizeViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L136) | Resizes backend presentation state for one detached viewport.<br>`target`: Updated viewport target. | — | — |
| <a id="api-c7d3ce6c083839d8"></a>[`void Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.RenderViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target, nint drawData)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L147) | Consumes draw data for one detached viewport.<br>`target`: Viewport destination.<br>`drawData`: Opaque native ImDrawData pointer valid for the current frame. | — | — |
| <a id="api-9c9c1751f643eb6b"></a>[`void Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.PresentViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L158) | Marks one detached viewport ready for presentation.<br>`target`: Viewport destination. | — | — |
| <a id="api-2f6e3369f70c0554"></a>[`System.Threading.Tasks.Task Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.RetireViewport(Inno.Adapter.Presentation.ImGui.PlatformImGuiViewportTarget target)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L172) | Removes the viewport from future rendering and schedules release of its borrowed native window.<br>`target`: The viewport whose original owner keeps its native window alive during retirement. | A task completed only when the renderer no longer uses the native window. This is not a GPU fence. | — |
| <a id="api-079401d2ea88162f"></a>[`void Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer.DrainViewportRetirements()`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/IPlatformImGuiRenderer.Api.cs#L180) | Drains requested retirements at a closed-frame owner-thread point outside native UI callbacks. | — | — |
| <a id="api-e04833cfcd7e830d"></a>[`public sealed class Inno.Adapter.Presentation.ImGui.PlatformImGuiContext : System.IDisposable`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/PlatformImGuiContext.Api.cs#L9) | Manages Dear ImGui frame lifecycle and rendering integration for a platform window. | — | — |
| <a id="api-1124a2f25ba43703"></a>[`public bool Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.TryGetWindowId(uint viewportId, out uint windowId)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/PlatformImGuiContext.Api.cs#L26) | Resolves the SDL window currently owned by an ImGui viewport.<br>`viewportId`: The ImGui viewport identity.<br>`windowId`: The SDL window identity when available. | when the viewport has a live SDL window. | `System.ObjectDisposedException`: Thrown after the presentation context has been disposed. |
| <a id="api-152df2c66dd0dc7c"></a>[`public void Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.RegisterFontStyle(Inno.Adapter.Presentation.ImGui.ImGuiFontStyle style, string filePath, float fontSizePixels = 16)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/PlatformImGuiContext.Api.cs#L46) | Registers a named font style and its glyph ranges in the active ImGui font atlas.<br>`style`: The style consumed by register font style; ownership remains with the caller unless explicitly stated otherwise.<br>`filePath`: The file path text validated by the register font style operation.<br>`fontSizePixels`: The font size pixels consumed by register font style; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-8f61f11617ce061a"></a>[`public void Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.SetIniFile(string? filePath)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/PlatformImGuiContext.Api.cs#L61) | Updates the ini file state and applies the resulting invariants.<br>`filePath`: The file path text validated by the set ini file operation. | — | — |
| <a id="api-e55d3c393b7d43e0"></a>[`public void Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.LoadIniSettings(string? settings)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/PlatformImGuiContext.Api.cs#L72) | Loads the current editor layout text into Dear ImGui without retaining the source string.<br>`settings`: The validated configuration that controls this operation. | — | — |
| <a id="api-bb1b4b1c56059515"></a>[`public bool Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.TryCaptureIniSettings(out string settings, bool force = false)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/PlatformImGuiContext.Api.cs#L86) | Attempts to capture ini settings without changing state when the operation cannot complete.<br>`settings`: The validated configuration that controls this operation.<br>`force`: Whether force behavior is enabled while try capture ini settings executes. | when the requested condition is satisfied; otherwise, . | — |
| <a id="api-67fb9dba2b8e149d"></a>[`public nint Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.RenderFrame(System.Action drawFrame, System.Action? prepareFrame = null)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/PlatformImGuiContext.Api.cs#L112) | Records frame rendering for the current frame.<br>`drawFrame`: The callback invoked by render frame within the operation's owned lifetime.<br>`prepareFrame`: Optional callback invoked with the owned context before native frame creation. | Borrowed native draw data for the completed frame, valid until the next frame or retirement. | — |
| <a id="api-fbe3ab469126a8e3"></a>[`public void Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.DrawImage(Inno.Adapter.Presentation.ImGui.ImGuiTextureHandle texture, System.Numerics.Vector2 size, System.Numerics.Vector2 uv0 = default(System.Numerics.Vector2), System.Numerics.Vector2 uv1 = default(System.Numerics.Vector2))`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/PlatformImGuiContext.Api.cs#L135) | Renders the image presentation for the current editor frame.<br>`texture`: The texture consumed by draw image; ownership remains with the caller unless explicitly stated otherwise.<br>`size`: The size consumed by draw image; ownership remains with the caller unless explicitly stated otherwise.<br>`uv0`: The uv0 consumed by draw image; ownership remains with the caller unless explicitly stated otherwise.<br>`uv1`: The uv1 consumed by draw image; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-3828c87aeed8a438"></a>[`public void Inno.Adapter.Presentation.ImGui.PlatformImGuiContext.Dispose()`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/PlatformImGuiContext.Api.cs#L148) | Retires the context and its owned resources after the current frame operation has completed. | — | `System.InvalidOperationException`: A frame operation still owns the context; the caller must retry after that operation completes. |
| <a id="api-1f9b31fc57ed8367"></a>[`public static class Inno.Adapter.Presentation.ImGui.Sdl3PlatformApplicationImGuiExtensions`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/Sdl3PlatformApplicationImGuiExtensions.cs#L14) | Extension APIs that add ImGui integration on top of . | — | — |
| <a id="api-00f37cf44e2b1ae1"></a>[`public static Inno.Adapter.Presentation.ImGui.PlatformImGuiContext Inno.Adapter.Presentation.ImGui.Sdl3PlatformApplicationImGuiExtensions.CreateImGuiContext(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication application, Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow window, Inno.Adapter.Presentation.ImGui.ImGuiContextFlags contextFlags, Inno.Adapter.Presentation.ImGui.ImGuiInteractionOptions interaction, Inno.Adapter.Presentation.ImGui.IPlatformImGuiRenderer? renderer = null)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/Sdl3PlatformApplicationImGuiExtensions.cs#L98) | Creates one ImGui context and transfers the supplied renderer ownership only after successful creation.<br>`application`: Target platform application instance.<br>`window`: Target platform window.<br>`interaction`: Explicit immutable keyboard and wheel conventions supplied by the product.<br>`contextFlags`: ImGui context feature flags.<br>`renderer`: Optional presentation backend; ownership transfers on success and remains with the caller on failure. | The newly created context, which owns its renderer until complete retirement and disposal. | `System.ArgumentNullException`: An application, window, or interaction configuration is null.<br>`System.ArgumentException`: The window does not belong to the supplied application.<br>`System.InvalidOperationException`: The window is closed or already owns an ImGui context. |
| <a id="api-c2da187e980c2c44"></a>[`public static void Inno.Adapter.Presentation.ImGui.Sdl3PlatformApplicationImGuiExtensions.DestroyImGuiContext(Inno.Adapter.Platform.Sdl3.Sdl3PlatformApplication application, Inno.Adapter.Platform.Sdl3.Sdl3PlatformWindow window)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Api/Sdl3PlatformApplicationImGuiExtensions.cs#L141) | Retires and disposes the window's context, retaining its application registration if retirement fails.<br>`application`: Target platform application instance.<br>`window`: Target platform window. | — | `System.ArgumentNullException`: The application or window is null.<br>`System.InvalidOperationException`: Retirement cannot complete at the current owner-thread safe point; the context remains owned. |
| <a id="api-f0e6d7a8f487fd11"></a>[`public enum Inno.Adapter.Presentation.ImGui.ImGuiContextFlags : int`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiContextFlags.cs#L9) | Identifies the supported im gui context flags values for this contract. | — | — |
| <a id="api-9db124c872f4d3ef"></a>[`Inno.Adapter.Presentation.ImGui.ImGuiContextFlags.None = 0`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiContextFlags.cs#L14) | The none key. | — | — |
| <a id="api-755a47626241fe74"></a>[`Inno.Adapter.Presentation.ImGui.ImGuiContextFlags.EnableViewports = 1`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiContextFlags.cs#L18) | The enable viewports key. | — | — |
| <a id="api-42eb671367d1b832"></a>[`Inno.Adapter.Presentation.ImGui.ImGuiContextFlags.EnableDocking = 2`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiContextFlags.cs#L22) | The enable docking key. | — | — |
| <a id="api-ae827699ceaf1d05"></a>[`Inno.Adapter.Presentation.ImGui.ImGuiContextFlags.EnableSmoothResize = 4`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiContextFlags.cs#L26) | The enable smooth resize key. | — | — |
| <a id="api-d7080d2675a24a21"></a>[`public static class Inno.Adapter.Presentation.ImGui.ImGuiFont`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiFont.cs#L12) | Provides scoped access to font faces registered for the current ImGui context. | — | — |
| <a id="api-f069ffe2fe747113"></a>[`public static bool Inno.Adapter.Presentation.ImGui.ImGuiFont.IsAvailable(Inno.Adapter.Presentation.ImGui.ImGuiFontStyle style)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiFont.cs#L26) | Gets whether an exact font style is available in the current ImGui context.<br>`style`: The composable style to query. | when the exact face is registered. | — |
| <a id="api-9013dbf620200c94"></a>[`public static Inno.Adapter.Presentation.ImGui.ImGuiFontScope Inno.Adapter.Presentation.ImGui.ImGuiFont.PushStyle(Inno.Adapter.Presentation.ImGui.ImGuiFontStyle style)`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiFont.cs#L49) | Pushes a font style for the current ImGui context and returns a scope that restores it.<br>`style`: The composable style to use. | A disposable scope. Missing faces gracefully fall back to the nearest registered face. | — |
| <a id="api-cf66d2fa755594d8"></a>[`public ref struct Inno.Adapter.Presentation.ImGui.ImGuiFontScope`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiFont.cs#L157) | Restores the previous ImGui font when a scoped style ends. | — | — |
| <a id="api-65b708e2b9e671e8"></a>[`public void Inno.Adapter.Presentation.ImGui.ImGuiFontScope.Dispose()`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiFont.cs#L173) | Restores the font that was active before this scope. | — | — |
| <a id="api-96992a1ad0abe719"></a>[`public enum Inno.Adapter.Presentation.ImGui.ImGuiFontStyle : int`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiFontStyle.cs#L9) | Describes composable visual variants of an ImGui font family. | — | — |
| <a id="api-3ad1519d8b95525b"></a>[`Inno.Adapter.Presentation.ImGui.ImGuiFontStyle.Regular = 0`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiFontStyle.cs#L14) | Uses the regular font face. | — | — |
| <a id="api-f3c17c02a7516316"></a>[`Inno.Adapter.Presentation.ImGui.ImGuiFontStyle.Bold = 1`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiFontStyle.cs#L19) | Uses a bold font face. | — | — |
| <a id="api-1ce5be79e3f32529"></a>[`Inno.Adapter.Presentation.ImGui.ImGuiFontStyle.Italic = 2`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiFontStyle.cs#L24) | Uses an italic font face. | — | — |
| <a id="api-8e830a96b5e5805d"></a>[`public static class Inno.Adapter.Presentation.ImGui.ImGuiIcon`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L12) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e0eee802672a6329"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shader = "S"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L15) | Letter mark used for authored Shader graph assets. | — | — |
| <a id="api-f9a2e27719bc951c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num0 = "0"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L16) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9f7667a6955c80d9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num1 = "1"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L17) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fb2bed4b33b4dcac"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num2 = "2"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L18) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b6d2b5039dbf72bb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num3 = "3"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L19) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9c015c2afc64c08d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num4 = "4"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L20) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9ae7a310025bf41b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num5 = "5"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L21) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4ff926184aaea329"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num6 = "6"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L22) | Inherited or compiler-provided contract. | — | — |
| <a id="api-edf972c5dd23aafb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num7 = "7"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L23) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d5c76cf75400a0ff"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num8 = "8"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L24) | Inherited or compiler-provided contract. | — | — |
| <a id="api-80f61ea2e1348a3f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Num9 = "9"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L25) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a5e6c10269907ab3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.A = "A"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L26) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3dfcdcda22405208"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AddressBook = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L27) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0e4bd9f41b550ec2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AddressCard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L28) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b713e40b24a2db73"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AlignCenter = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L29) | Inherited or compiler-provided contract. | — | — |
| <a id="api-55ee1d4dc910af73"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AlignJustify = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L30) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b81ad534c043ce42"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AlignLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L31) | Inherited or compiler-provided contract. | — | — |
| <a id="api-60e5a3b16f010526"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AlignRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L32) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e92d749596fba409"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Anchor = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L33) | Inherited or compiler-provided contract. | — | — |
| <a id="api-db90ad41561b1a6a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnchorCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L34) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4be1a5de6d4464c2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnchorCircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L35) | Inherited or compiler-provided contract. | — | — |
| <a id="api-270d06c384edd671"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnchorCircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L36) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b82f57f34ecc0b61"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnchorLock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L37) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6690373bdc005cac"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AngleDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L38) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7efad907e9973c45"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AngleLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L39) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b3e64abbd4b7a478"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AngleRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L40) | Inherited or compiler-provided contract. | — | — |
| <a id="api-443481220b2550dd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AngleUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L41) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8c0fecfbe337ba7f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnglesDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L42) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d8c101241cbd6a1e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnglesLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L43) | Inherited or compiler-provided contract. | — | — |
| <a id="api-99461595c284f2a0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnglesRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L44) | Inherited or compiler-provided contract. | — | — |
| <a id="api-620c19541b94305d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AnglesUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L45) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ddfdfc20f144fe2d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ankh = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L46) | Inherited or compiler-provided contract. | — | — |
| <a id="api-aa5317e88e45e330"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AppleWhole = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L47) | Inherited or compiler-provided contract. | — | — |
| <a id="api-044c071f7560be69"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Archway = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L48) | Inherited or compiler-provided contract. | — | — |
| <a id="api-633622d57d20a0b5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L49) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1d8e83b014bd9d3c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDown19 = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L50) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e6b18a0f82c5e650"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDown91 = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L51) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5316d9e541205383"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownAZ = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L52) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7d087d87b8c5cf6f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownLong = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L53) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7e106bffe947a3a3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownShortWide = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L54) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a5f38c31ab374b83"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownUpAcrossLine = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L55) | Inherited or compiler-provided contract. | — | — |
| <a id="api-843f925eeefeda63"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownUpLock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L56) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ffb7a5f46f679ad4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownWideShort = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L57) | Inherited or compiler-provided contract. | — | — |
| <a id="api-83ce516470c31700"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowDownZA = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L58) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d088738426d9e91a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L59) | Inherited or compiler-provided contract. | — | — |
| <a id="api-28be98d5f9762fe0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowLeftLong = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L60) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cd260ae13332b0c3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowPointer = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L61) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8bec32a7a845c874"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L62) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bd0235e124287354"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRightArrowLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L63) | Inherited or compiler-provided contract. | — | — |
| <a id="api-88a2b78759d060e9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRightFromBracket = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L64) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5108366984577a61"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRightLong = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L65) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fb0772467ff3531b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRightToBracket = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L66) | Inherited or compiler-provided contract. | — | — |
| <a id="api-003826f75a2b37d3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRightToCity = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L67) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3e46fab6437c6407"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRotateLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L68) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8ecc921707a91050"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowRotateRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L69) | Inherited or compiler-provided contract. | — | — |
| <a id="api-994a9999769636d8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowTrendDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L70) | Inherited or compiler-provided contract. | — | — |
| <a id="api-44308b3b655d4f09"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowTrendUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L71) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6a77350e88fcd923"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowTurnDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L72) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2c002a952fc6e5d8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowTurnUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L73) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8d530b3344e60dfb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L74) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1af507260629e3ff"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUp19 = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L75) | Inherited or compiler-provided contract. | — | — |
| <a id="api-caae28c945fc283d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUp91 = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L76) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9c724d8440f1c2c7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpAZ = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L77) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e18540e354bf0222"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpFromBracket = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L78) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4ee36359e5ec334e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpFromGroundWater = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L79) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c6b350f5b3ccf276"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpFromWaterPump = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L80) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a273365bb2681e50"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpLong = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L81) | Inherited or compiler-provided contract. | — | — |
| <a id="api-296cb884bb59e2fb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpRightDots = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L82) | Inherited or compiler-provided contract. | — | — |
| <a id="api-af1f5a3158185427"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpRightFromSquare = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L83) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bf4e18274c68effa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpShortWide = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L84) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d1ce2c0d81f73702"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpWideShort = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L85) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6bb2dc66d82cd526"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowUpZA = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L86) | Inherited or compiler-provided contract. | — | — |
| <a id="api-833c5510bc246c4b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsDownToLine = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L87) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6e48aee332aba9e7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsDownToPeople = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L88) | Inherited or compiler-provided contract. | — | — |
| <a id="api-17fb6e3117a3c8fd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsLeftRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L89) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cc16784741d76888"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsLeftRightToLine = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L90) | Inherited or compiler-provided contract. | — | — |
| <a id="api-053f75b79561e3be"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsRotate = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L91) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6b4b3071d449ee34"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsSpin = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L92) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4ab6ef672ea8df8d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsSplitUpAndLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L93) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5b5e1157b08e517b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsToCircle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L94) | Inherited or compiler-provided contract. | — | — |
| <a id="api-be2028876ac54a3a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsToDot = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L95) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dd51ef3590cab872"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsToEye = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L96) | Inherited or compiler-provided contract. | — | — |
| <a id="api-01120902fb70009f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsTurnRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L97) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a52b423652e2a6e9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsTurnToDots = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L98) | Inherited or compiler-provided contract. | — | — |
| <a id="api-62dc21964cbb06ba"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsUpDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L99) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b7148d1a6ff4992f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsUpDownLeftRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L100) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a16d418bb8b9d481"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ArrowsUpToLine = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L101) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9e8c809542c19b9a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Asterisk = "*"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L102) | Inherited or compiler-provided contract. | — | — |
| <a id="api-976e09adaa4e24db"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.At = "@"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L103) | Inherited or compiler-provided contract. | — | — |
| <a id="api-393f28bfc83f572b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Atom = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L104) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ec0c54ea89ee6c59"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AudioDescription = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L105) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7800a5bb61070a3e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.AustralSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L106) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7350e9ca7a0e4306"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Award = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L107) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f0da7b76be5b80eb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.B = "B"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L108) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ac270430f8747196"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Baby = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L109) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6b2eeaf234e7eed4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BabyCarriage = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L110) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5f4e47a719567116"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Backward = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L111) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0ade6c5bf30698c3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BackwardFast = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L112) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6e8de6b61399afa7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BackwardStep = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L113) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a08e28d9765ccae8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bacon = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L114) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5f590affa00da41a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bacteria = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L115) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ee805f50e63a38cb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bacterium = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L116) | Inherited or compiler-provided contract. | — | — |
| <a id="api-53a50260d144a2b4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BagShopping = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L117) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b9575418b7424c71"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bahai = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L118) | Inherited or compiler-provided contract. | — | — |
| <a id="api-de68ba851802a32c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BahtSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L119) | Inherited or compiler-provided contract. | — | — |
| <a id="api-13a53dde2b0cac9a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ban = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L120) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2ec4da07a61db06d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BanSmoking = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L121) | Inherited or compiler-provided contract. | — | — |
| <a id="api-469417c4b333bf8a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bandage = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L122) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4b2a072575cbc2b7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BangladeshiTakaSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L123) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1e04fae545ea8d57"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Barcode = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L124) | Inherited or compiler-provided contract. | — | — |
| <a id="api-eec21929e1d2114f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bars = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L125) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1721c6d7e5a86df5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BarsProgress = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L126) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d87a2517a1fd2101"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BarsStaggered = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L127) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5661ec0f25ac4fe8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Baseball = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L128) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bc0f9518db766212"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BaseballBatBall = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L129) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bc93bdbfb90683c9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BasketShopping = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L130) | Inherited or compiler-provided contract. | — | — |
| <a id="api-aa2c51aff46ffb2b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Basketball = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L131) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d2b35e201b0065c3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bath = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L132) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a808a988514c4462"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BatteryEmpty = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L133) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a9eda72ad5945782"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BatteryFull = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L134) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d02632db0eebb92e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BatteryHalf = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L135) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7afd7ae54e7fe223"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BatteryQuarter = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L136) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8821f8ef55014a3a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BatteryThreeQuarters = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L137) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2662071298b60d12"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bed = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L138) | Inherited or compiler-provided contract. | — | — |
| <a id="api-35cf7b06d438d11d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BedPulse = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L139) | Inherited or compiler-provided contract. | — | — |
| <a id="api-467673fe5c51bcf4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BeerMugEmpty = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L140) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8e4b3bc4a012b675"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bell = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L141) | Inherited or compiler-provided contract. | — | — |
| <a id="api-92089a0c098b29ce"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BellConcierge = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L142) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9dc50897bdcdeb7f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BellSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L143) | Inherited or compiler-provided contract. | — | — |
| <a id="api-09946553732e8f29"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BezierCurve = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L144) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f68b42a044d62d3e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bicycle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L145) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d0d74f548b6bb94a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Binoculars = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L146) | Inherited or compiler-provided contract. | — | — |
| <a id="api-59d69843fa6e117e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Biohazard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L147) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2b84643eaf23628d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BitcoinSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L148) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7e042c09ea0e88de"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Blender = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L149) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f81e676bb1ceaa35"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BlenderPhone = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L150) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1b128c318c2ed6b5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Blog = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L151) | Inherited or compiler-provided contract. | — | — |
| <a id="api-97a139f942d85761"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bold = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L152) | Inherited or compiler-provided contract. | — | — |
| <a id="api-44cf6a620c5e6370"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bolt = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L153) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0f8b01e8ff22c5b8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoltLightning = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L154) | Inherited or compiler-provided contract. | — | — |
| <a id="api-08f701a0a88420d6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bomb = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L155) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d09d705e6e9c7c43"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bone = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L156) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8fba548ac7201aed"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bong = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L157) | Inherited or compiler-provided contract. | — | — |
| <a id="api-533676d035b57a11"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Book = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L158) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6aff281853c5176e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookAtlas = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L159) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7bfca16f05bedf69"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookBible = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L160) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5ad889bfc3f64180"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookBookmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L161) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b61d640b419a3fd6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookJournalWhills = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L162) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0913896673d27309"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L163) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4d1e22546f68378f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookOpen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L164) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d5d14c452986f823"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookOpenReader = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L165) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ff6cfd646ce31d82"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookQuran = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L166) | Inherited or compiler-provided contract. | — | — |
| <a id="api-64dd77fa5ee1c29a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookSkull = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L167) | Inherited or compiler-provided contract. | — | — |
| <a id="api-68406c2c0673e6e0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BookTanakh = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L168) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9e1e24fc04b6c9cc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bookmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L169) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a76dc1fa1ca355bf"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BorderAll = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L170) | Inherited or compiler-provided contract. | — | — |
| <a id="api-19699f6531ab4669"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BorderNone = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L171) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fabdbaa70e3c6d13"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BorderTopLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L172) | Inherited or compiler-provided contract. | — | — |
| <a id="api-75ea06576ad5e3f0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoreHole = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L173) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4ec74c6932a93154"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BottleDroplet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L174) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fd17792dd03a3948"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BottleWater = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L175) | Inherited or compiler-provided contract. | — | — |
| <a id="api-43f6fc93acd559db"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BowlFood = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L176) | Inherited or compiler-provided contract. | — | — |
| <a id="api-865b8c726c34b3a4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BowlRice = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L177) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ea333a731bbe91a5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BowlingBall = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L178) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8c3bc845fba9f475"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Box = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L179) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8598fc34de42dec3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoxArchive = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L180) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2c78a2cd552d53ed"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoxOpen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L181) | Inherited or compiler-provided contract. | — | — |
| <a id="api-084e62c562a3f37d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoxTissue = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L182) | Inherited or compiler-provided contract. | — | — |
| <a id="api-639014a5abf1d5f6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoxesPacking = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L183) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7bb173aaa8cc236c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BoxesStacked = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L184) | Inherited or compiler-provided contract. | — | — |
| <a id="api-80536aab78110db0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Braille = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L185) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5c7d3bcb12b14ddb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Brain = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L186) | Inherited or compiler-provided contract. | — | — |
| <a id="api-776ac12562d445ad"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BrazilianRealSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L187) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a71adc47741ababa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BreadSlice = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L188) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8a62d9672543dba4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bridge = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L189) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6f2c21a83173f27c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BridgeCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L190) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dc5a314d5e14a35f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BridgeCircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L191) | Inherited or compiler-provided contract. | — | — |
| <a id="api-90ede19804e04f7e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BridgeCircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L192) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fcdb0251735db230"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BridgeLock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L193) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7b1da59ef2fee84c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BridgeWater = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L194) | Inherited or compiler-provided contract. | — | — |
| <a id="api-48310cc1c474837f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Briefcase = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L195) | Inherited or compiler-provided contract. | — | — |
| <a id="api-25ebe40059753dce"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BriefcaseMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L196) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4d72eddea6d30e4c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Broom = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L197) | Inherited or compiler-provided contract. | — | — |
| <a id="api-21b5a8b1f020553f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BroomBall = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L198) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c1a8b032690e4561"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Brush = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L199) | Inherited or compiler-provided contract. | — | — |
| <a id="api-98eca038ca2bff4e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bucket = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L200) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0d3a800913998b56"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bug = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L201) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3de8d570b4e78104"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BugSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L202) | Inherited or compiler-provided contract. | — | — |
| <a id="api-607b741e4e3f36d5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bugs = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L203) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c2fe00f16fe2e761"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Building = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L204) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b785c1bbb6d7f6aa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingCircleArrowRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L205) | Inherited or compiler-provided contract. | — | — |
| <a id="api-76651139018a2ce5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L206) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ecd81a30ccf6064a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingCircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L207) | Inherited or compiler-provided contract. | — | — |
| <a id="api-de74d674935d464e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingCircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L208) | Inherited or compiler-provided contract. | — | — |
| <a id="api-77528443375b0ff1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingColumns = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L209) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a352a42fed38fe0f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingFlag = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L210) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b4541d80aea7ab2c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingLock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L211) | Inherited or compiler-provided contract. | — | — |
| <a id="api-31e5cad31b56d4ac"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingNgo = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L212) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9c977d8292bf5012"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingShield = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L213) | Inherited or compiler-provided contract. | — | — |
| <a id="api-299293f2f182a79c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingUn = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L214) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5f7d48fb9b7db892"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingUser = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L215) | Inherited or compiler-provided contract. | — | — |
| <a id="api-be76c473ce1cdd93"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BuildingWheat = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L216) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a6c34499066a963a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bullhorn = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L217) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b8c8bf729019e52f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bullseye = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L218) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5aaf0a21b5f04ffa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Burger = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L219) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8964e9224d200ed7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Burst = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L220) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ef9861958c503cd2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Bus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L221) | Inherited or compiler-provided contract. | — | — |
| <a id="api-74b5224e5cca79b0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BusSimple = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L222) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7606c15c7a4f9925"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.BusinessTime = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L223) | Inherited or compiler-provided contract. | — | — |
| <a id="api-931938cc0c7b22a5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.C = "C"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L224) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d7bb45ac3cf2e5b1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CableCar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L225) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6432eed3fe4bb300"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CakeCandles = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L226) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d816188c7029b6d9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Calculator = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L227) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b18c30ed9cfa5c5f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Calendar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L228) | Inherited or compiler-provided contract. | — | — |
| <a id="api-79231cfc8a36ffe0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L229) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a569e27235727279"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarDay = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L230) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0e4197618e36c22e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarDays = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L231) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dcebec2049574a26"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarMinus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L232) | Inherited or compiler-provided contract. | — | — |
| <a id="api-770fc5e39929db1c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarPlus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L233) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ab7d448fe62d255d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarWeek = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L234) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3b9b3d73cce30f5b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CalendarXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L235) | Inherited or compiler-provided contract. | — | — |
| <a id="api-520b5bfaa933771a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Camera = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L236) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e0100e32f8516618"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CameraRetro = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L237) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e199f50db392dc6b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CameraRotate = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L238) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0211145aeca2684d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Campground = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L239) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3ee28403fad8d39e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CandyCane = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L240) | Inherited or compiler-provided contract. | — | — |
| <a id="api-858753f9f577fff2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cannabis = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L241) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5e47a8a0325fd0e4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Capsules = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L242) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bbde953dd1bd8611"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Car = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L243) | Inherited or compiler-provided contract. | — | — |
| <a id="api-34203068835b6c3d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CarBattery = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L244) | Inherited or compiler-provided contract. | — | — |
| <a id="api-df69aed5ac27c344"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CarBurst = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L245) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b8c8ec708aa739e2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CarOn = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L246) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b9a8110a36404387"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CarRear = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L247) | Inherited or compiler-provided contract. | — | — |
| <a id="api-84117c8f5d95a839"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CarSide = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L248) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d4628b6770bd197b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CarTunnel = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L249) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0a19bba7b92cf1c6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Caravan = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L250) | Inherited or compiler-provided contract. | — | — |
| <a id="api-03286a7defe36eed"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CaretDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L251) | Inherited or compiler-provided contract. | — | — |
| <a id="api-50964854587eb1cd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CaretLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L252) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f2845acadf76e98a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CaretRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L253) | Inherited or compiler-provided contract. | — | — |
| <a id="api-131933a28e949c01"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CaretUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L254) | Inherited or compiler-provided contract. | — | — |
| <a id="api-918ee7ed3c50faf4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Carrot = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L255) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4c183304a0d97a99"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CartArrowDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L256) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e5bd7563b0203552"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CartFlatbed = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L257) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9ddec4d0a96e1d8e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CartFlatbedSuitcase = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L258) | Inherited or compiler-provided contract. | — | — |
| <a id="api-94e3dd31f039464e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CartPlus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L259) | Inherited or compiler-provided contract. | — | — |
| <a id="api-adaadf7a28333817"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CartShopping = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L260) | Inherited or compiler-provided contract. | — | — |
| <a id="api-71f8fdea9ce7099c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CashRegister = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L261) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f450b5ca1a69247f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cat = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L262) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d5602be649b2c545"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CediSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L263) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6979a51b771ce6a0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CentSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L264) | Inherited or compiler-provided contract. | — | — |
| <a id="api-642d9d0351207d3d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Certificate = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L265) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ccbd7f46c761a6c1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Chair = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L266) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4b0ee6428523084d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Chalkboard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L267) | Inherited or compiler-provided contract. | — | — |
| <a id="api-de44bd7903fad749"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChalkboardUser = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L268) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c5138dab4e814326"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChampagneGlasses = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L269) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f466f6510d9d17db"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChargingStation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L270) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8af064a8ab95e133"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartArea = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L271) | Inherited or compiler-provided contract. | — | — |
| <a id="api-664cb940450dc5f6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartBar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L272) | Inherited or compiler-provided contract. | — | — |
| <a id="api-539c2de2ae65248f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartColumn = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L273) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e195db92831fb130"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartDiagram = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L274) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cfc0c7fe0a9ac75f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartGantt = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L275) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b99d9e6e4e4c1183"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartLine = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L276) | Inherited or compiler-provided contract. | — | — |
| <a id="api-03bf10fd3327077d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartPie = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L277) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bc182669c6d1ffc1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChartSimple = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L278) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c293cf01c5b81ffe"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Check = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L279) | Inherited or compiler-provided contract. | — | — |
| <a id="api-97d5c4b1a5be4c94"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CheckDouble = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L280) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1a5c42d8b48177c9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CheckToSlot = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L281) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f96805676e73bd10"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cheese = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L282) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d4ee4c690f9ec805"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Chess = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L283) | Inherited or compiler-provided contract. | — | — |
| <a id="api-34d4b2dda8b78898"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessBishop = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L284) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d2d5ee7299aae3eb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessBoard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L285) | Inherited or compiler-provided contract. | — | — |
| <a id="api-35a453ec95af7005"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessKing = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L286) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e6af58c077fb432e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessKnight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L287) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a7163cce783c5940"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessPawn = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L288) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ffbdbe7967d28f49"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessQueen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L289) | Inherited or compiler-provided contract. | — | — |
| <a id="api-879fee7db6823cb3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChessRook = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L290) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1f17e9d2ec149f3f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChevronDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L291) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e88d2e4141941458"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChevronLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L292) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bf5633d1f4aa57aa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChevronRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L293) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0516d7f9fe4616bd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChevronUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L294) | Inherited or compiler-provided contract. | — | — |
| <a id="api-88dc2a920756d4f4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Child = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L295) | Inherited or compiler-provided contract. | — | — |
| <a id="api-49439845503828dd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChildCombatant = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L296) | Inherited or compiler-provided contract. | — | — |
| <a id="api-17a5d321bb13cd3c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChildDress = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L297) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b261218a7aa5e87c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ChildReaching = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L298) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c39d465bceb99946"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Children = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L299) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e8331652525395eb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Church = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L300) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e0486494dc585015"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Circle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L301) | Inherited or compiler-provided contract. | — | — |
| <a id="api-16abdbd612355fc8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleArrowDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L302) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a7ece6ef9fc0ddb1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleArrowLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L303) | Inherited or compiler-provided contract. | — | — |
| <a id="api-834dfdaa410ece60"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleArrowRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L304) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8232947676194b5c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleArrowUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L305) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ba009323e4bbbc66"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L306) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fd027603b3b7ff98"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleChevronDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L307) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bba432d867eb4afd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleChevronLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L308) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5f34c8fdd20df8b3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleChevronRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L309) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a884f73302a5845f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleChevronUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L310) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fe8e8ecab70562f0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleDollarToSlot = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L311) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9758efefd81f85f0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleDot = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L312) | Inherited or compiler-provided contract. | — | — |
| <a id="api-aa6f3a0215385883"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L313) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3e05c6b7b65e81e6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L314) | Inherited or compiler-provided contract. | — | — |
| <a id="api-05ca3f50082cc231"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleH = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L315) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6a1cfef0ef173b8a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleHalfStroke = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L316) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8cc734e39ed952e1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleInfo = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L317) | Inherited or compiler-provided contract. | — | — |
| <a id="api-370edd1f14549434"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L318) | Inherited or compiler-provided contract. | — | — |
| <a id="api-de9fae378ad5f7f5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleMinus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L319) | Inherited or compiler-provided contract. | — | — |
| <a id="api-372f3d3e838c38b2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleNodes = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L320) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c3756f7af09d675c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleNotch = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L321) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c84645afacf27abe"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CirclePause = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L322) | Inherited or compiler-provided contract. | — | — |
| <a id="api-aca494c9b8b07a23"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CirclePlay = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L323) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a3c36e56393e06ac"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CirclePlus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L324) | Inherited or compiler-provided contract. | — | — |
| <a id="api-31cee5cef84464d3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleQuestion = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L325) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fae7712ff0ece2f0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleRadiation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L326) | Inherited or compiler-provided contract. | — | — |
| <a id="api-156d1147e7ae5301"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L327) | Inherited or compiler-provided contract. | — | — |
| <a id="api-970066f62fb2ae3f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleStop = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L328) | Inherited or compiler-provided contract. | — | — |
| <a id="api-619c27dbc73d0b2b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L329) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5793ba88e5004a81"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleUser = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L330) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f67ff5b708362779"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L331) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d85057e9fc4fde58"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.City = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L332) | Inherited or compiler-provided contract. | — | — |
| <a id="api-48b2928efc18c5fc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Clapperboard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L333) | Inherited or compiler-provided contract. | — | — |
| <a id="api-16bcfffb0752cbf8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Clipboard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L334) | Inherited or compiler-provided contract. | — | — |
| <a id="api-314134512c82581d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ClipboardCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L335) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b2e0394fb1f5368a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ClipboardList = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L336) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e94d4ffb5f6571ff"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ClipboardQuestion = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L337) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1bf2cc9a8a974894"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ClipboardUser = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L338) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f77381c33f0739ae"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Clock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L339) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b3360c2e5ef50fcc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ClockRotateLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L340) | Inherited or compiler-provided contract. | — | — |
| <a id="api-044c99af28d2aa8a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Clone = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L341) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5b573bd6d4c71f47"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ClosedCaptioning = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L342) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b2c3533a63325e5a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cloud = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L343) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fd1d171de0b4c0ad"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudArrowDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L344) | Inherited or compiler-provided contract. | — | — |
| <a id="api-47e59de5e2bd49fb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudArrowUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L345) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ee78b2bc83028793"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudBolt = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L346) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d604717ceb851852"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudMeatball = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L347) | Inherited or compiler-provided contract. | — | — |
| <a id="api-15090056efc4fd95"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudMoon = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L348) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9f145a15bb5320e9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudMoonRain = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L349) | Inherited or compiler-provided contract. | — | — |
| <a id="api-db0bcff028e84268"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudRain = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L350) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e1932c2ffe675155"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudShowersHeavy = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L351) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f236aa6ccb28cc2e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudShowersWater = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L352) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2a22041d481585e1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudSun = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L353) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a7deec1acb20fb8f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CloudSunRain = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L354) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b54e58324325131a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Clover = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L355) | Inherited or compiler-provided contract. | — | — |
| <a id="api-885bc46e528d9f85"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Code = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L356) | Inherited or compiler-provided contract. | — | — |
| <a id="api-253eac899a3fd45a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CodeBranch = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L357) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8f4f683c584a50ea"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CodeCommit = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L358) | Inherited or compiler-provided contract. | — | — |
| <a id="api-90da903ee294e22e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CodeCompare = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L359) | Inherited or compiler-provided contract. | — | — |
| <a id="api-61496114e88462d3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CodeFork = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L360) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cb7b9294cc371296"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CodeMerge = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L361) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ec69a545e98d5593"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CodePullRequest = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L362) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c83f6322ee3ea499"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Coins = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L363) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2cc4691b96ed7c8f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ColonSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L364) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7ec5a33c618bcc6b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Comment = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L365) | Inherited or compiler-provided contract. | — | — |
| <a id="api-91b5c3d1c19de0a3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentDollar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L366) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f9e7d9143d564d1d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentDots = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L367) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1a634afaebdca7ee"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L368) | Inherited or compiler-provided contract. | — | — |
| <a id="api-56adc8d3cd37f30f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentNodes = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L369) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9de3c94b9f01d056"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L370) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f40c40e1ff62b4e2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentSms = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L371) | Inherited or compiler-provided contract. | — | — |
| <a id="api-266ff605d612956d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Comments = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L372) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ba4b9d681ae12282"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CommentsDollar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L373) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f68732b0d8ec0237"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CompactDisc = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L374) | Inherited or compiler-provided contract. | — | — |
| <a id="api-42bfd3fb1b1df1d8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Compass = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L375) | Inherited or compiler-provided contract. | — | — |
| <a id="api-657670ddc8ac747b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CompassDrafting = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L376) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6012631b37f41ca8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Compress = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L377) | Inherited or compiler-provided contract. | — | — |
| <a id="api-be9a614eaae3b408"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Computer = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L378) | Inherited or compiler-provided contract. | — | — |
| <a id="api-62c4b5d48e0f1897"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ComputerMouse = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L379) | Inherited or compiler-provided contract. | — | — |
| <a id="api-67395f6f932f8265"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cookie = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L380) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9ffffaa9a027a454"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CookieBite = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L381) | Inherited or compiler-provided contract. | — | — |
| <a id="api-121df2330ad828fd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Copy = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L382) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bf232d7712a91148"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Copyright = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L383) | Inherited or compiler-provided contract. | — | — |
| <a id="api-52c42ffd3b4417eb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Couch = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L384) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1aaac422dc6e57e3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cow = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L385) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7953846349073c4c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CreditCard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L386) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9b860a20f6795e29"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Crop = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L387) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f5ba053d0f80b487"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CropSimple = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L388) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b7cc10724bc10582"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cross = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L389) | Inherited or compiler-provided contract. | — | — |
| <a id="api-89163e24461f57a5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Crosshairs = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L390) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b7c5d0eb044541c2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Crow = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L391) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2ebaf4ea72cb5263"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Crown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L392) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f9cb4dcd56bb06f1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Crutch = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L393) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3d0f1e547653b8e0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CruzeiroSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L394) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f54c6ad228c7b24a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cube = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L395) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ac7be47fb08cbf93"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Cubes = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L396) | Inherited or compiler-provided contract. | — | — |
| <a id="api-444518ae0552afc7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.CubesStacked = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L397) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bd112343bd048d09"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.D = "D"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L398) | Inherited or compiler-provided contract. | — | — |
| <a id="api-842050f596bdb627"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Database = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L399) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3248289a8d307c55"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DeleteLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L400) | Inherited or compiler-provided contract. | — | — |
| <a id="api-51b48500ad49c549"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Democrat = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L401) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8bd21cd1f3dff9e9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Desktop = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L402) | Inherited or compiler-provided contract. | — | — |
| <a id="api-11bef30890c0ba77"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dharmachakra = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L403) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b8e31522c0132ca6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiagramNext = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L404) | Inherited or compiler-provided contract. | — | — |
| <a id="api-096a6247db68e50b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiagramPredecessor = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L405) | Inherited or compiler-provided contract. | — | — |
| <a id="api-65a788fd75a915e6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiagramProject = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L406) | Inherited or compiler-provided contract. | — | — |
| <a id="api-948185e750481584"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiagramSuccessor = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L407) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d93874c09dc659ac"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Diamond = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L408) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5afc8c7444570c16"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiamondTurnRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L409) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a98109aab10ccbc4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dice = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L410) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f9dba67d690237bf"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceD20 = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L411) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dd2f347142b4c454"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceD6 = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L412) | Inherited or compiler-provided contract. | — | — |
| <a id="api-afe6fb8d4aaefd10"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceFive = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L413) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0504594d30bb7a28"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceFour = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L414) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ad23149a76304e3f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceOne = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L415) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4aa2e5a70352dd79"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceSix = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L416) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6396b8abf4876369"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceThree = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L417) | Inherited or compiler-provided contract. | — | — |
| <a id="api-330be2c1b3e6c2be"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DiceTwo = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L418) | Inherited or compiler-provided contract. | — | — |
| <a id="api-432050290a73498c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Disease = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L419) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9843157c65a4dd6a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Display = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L420) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5b33fbcdd54c2fa2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Divide = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L421) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e8af70d8b114e9c0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dna = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L422) | Inherited or compiler-provided contract. | — | — |
| <a id="api-829e797af9420647"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dog = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L423) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6bfb070f934d050a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DollarSign = "$"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L424) | Inherited or compiler-provided contract. | — | — |
| <a id="api-61b1a067ae905e1e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dolly = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L425) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5a01a0fed46a8e71"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DongSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L426) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1e04e36386b3c9fb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DoorClosed = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L427) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6eab76c6f45678cc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DoorOpen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L428) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2ec5b905d51e42a0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dove = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L429) | Inherited or compiler-provided contract. | — | — |
| <a id="api-98ef6b0d5fd23a4e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DownLeftAndUpRightToCenter = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L430) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c4fae873c291c5b7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DownLong = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L431) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f382eaad689ca456"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Download = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L432) | Inherited or compiler-provided contract. | — | — |
| <a id="api-075eb145979ccc26"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dragon = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L433) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ceccc18549a0639e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DrawPolygon = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L434) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9d0fbfa90618341c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Droplet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L435) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6bc1ae1d24fefb46"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DropletSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L436) | Inherited or compiler-provided contract. | — | — |
| <a id="api-83c5f32ae28bd350"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Drum = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L437) | Inherited or compiler-provided contract. | — | — |
| <a id="api-30b30d86c55a937f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DrumSteelpan = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L438) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3f8e85091fe9aca0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DrumstickBite = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L439) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ee95a15ed8232e13"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dumbbell = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L440) | Inherited or compiler-provided contract. | — | — |
| <a id="api-57030d76ea41dca2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dumpster = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L441) | Inherited or compiler-provided contract. | — | — |
| <a id="api-61a8f0288285cf36"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.DumpsterFire = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L442) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8d76445938ba2253"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Dungeon = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L443) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d3e85e0d97f1bea0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.E = "E"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L444) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8704b20609e8fa61"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarDeaf = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L445) | Inherited or compiler-provided contract. | — | — |
| <a id="api-77673f7076c7a8e6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarListen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L446) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a440a2d1f79413f4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarthAfrica = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L447) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9edadfe87c27e955"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarthAmericas = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L448) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c237ae54815ba4e8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarthAsia = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L449) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f027e0463dd95111"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarthEurope = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L450) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7cb4929381e3f618"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EarthOceania = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L451) | Inherited or compiler-provided contract. | — | — |
| <a id="api-00abda92fff4624a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Egg = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L452) | Inherited or compiler-provided contract. | — | — |
| <a id="api-432b97d32dbd9523"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Eject = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L453) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a1d254bba5107430"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Elevator = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L454) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b1884b766743bd04"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ellipsis = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L455) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3770d0ab58fc4175"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EllipsisVertical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L456) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8254b7125bb8171b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Envelope = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L457) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2b22d994ffd34bcc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EnvelopeCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L458) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cb8b44b277202705"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EnvelopeOpen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L459) | Inherited or compiler-provided contract. | — | — |
| <a id="api-49454419ecab960a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EnvelopeOpenText = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L460) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bb4d53df6e28a74a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EnvelopesBulk = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L461) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ff3f705d58092c9a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Equals = "="`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L462) | Inherited or compiler-provided contract. | — | — |
| <a id="api-107d2736622037ed"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Eraser = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L463) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b06b32057237529c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ethernet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L464) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a79b66119e462403"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EuroSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L465) | Inherited or compiler-provided contract. | — | — |
| <a id="api-575639b27be7506b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Exclamation = "!"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L466) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0a1c3aa55d1d622f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Expand = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L467) | Inherited or compiler-provided contract. | — | — |
| <a id="api-83b563b3ae445363"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Explosion = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L468) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9e66eec965f03c4d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Eye = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L469) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fa87befd25a3392a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EyeDropper = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L470) | Inherited or compiler-provided contract. | — | — |
| <a id="api-62bb936c4e885367"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EyeLowVision = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L471) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ddc23a345e118bb4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.EyeSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L472) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c8732346f043a539"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.F = "F"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L473) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c57383289b5c873d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceAngry = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L474) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a1415114c748a3cc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceDizzy = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L475) | Inherited or compiler-provided contract. | — | — |
| <a id="api-61843f27efefa556"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceFlushed = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L476) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dfe819bc2b1f3b19"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceFrown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L477) | Inherited or compiler-provided contract. | — | — |
| <a id="api-43a86761ee63564f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceFrownOpen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L478) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f5d1c5f0aa1df355"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrimace = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L479) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ba185e69b8a8e23b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrin = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L480) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d3859044e5408ee3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinBeam = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L481) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6b4f562ed49821b1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinBeamSweat = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L482) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1b70d731a26813e1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinHearts = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L483) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b37c47b8c254e9d6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinSquint = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L484) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b9782eef38da18a5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinSquintTears = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L485) | Inherited or compiler-provided contract. | — | — |
| <a id="api-05432bd787d3f4f9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinStars = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L486) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4efc479c5e14fef7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinTears = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L487) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4d14fc77a9e63629"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinTongue = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L488) | Inherited or compiler-provided contract. | — | — |
| <a id="api-084e6aad0a8db981"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinTongueSquint = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L489) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f320d9a468f11d7e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinTongueWink = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L490) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3f2cd03470ba0ea3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinWide = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L491) | Inherited or compiler-provided contract. | — | — |
| <a id="api-757c547e7e385678"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceGrinWink = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L492) | Inherited or compiler-provided contract. | — | — |
| <a id="api-76881d49294758aa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceKiss = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L493) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8a65b1865ea32bf0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceKissBeam = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L494) | Inherited or compiler-provided contract. | — | — |
| <a id="api-37e6098e25445123"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceKissWinkHeart = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L495) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7f128e72a2f4ff9d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceLaugh = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L496) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3833283aaaa438c3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceLaughBeam = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L497) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7c006912bece6b91"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceLaughSquint = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L498) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8807b1cb99ecd352"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceLaughWink = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L499) | Inherited or compiler-provided contract. | — | — |
| <a id="api-aa5cf6a8ed6deb7b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceMeh = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L500) | Inherited or compiler-provided contract. | — | — |
| <a id="api-694f29ee7e9d7065"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceMehBlank = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L501) | Inherited or compiler-provided contract. | — | — |
| <a id="api-136433075072ec41"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceRollingEyes = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L502) | Inherited or compiler-provided contract. | — | — |
| <a id="api-19be171ed9fc7190"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceSadCry = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L503) | Inherited or compiler-provided contract. | — | — |
| <a id="api-833510cc8208acb5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceSadTear = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L504) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6b8479af3bc5a37f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceSmile = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L505) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f3a19cbd91d52588"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceSmileBeam = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L506) | Inherited or compiler-provided contract. | — | — |
| <a id="api-51fb1be7acc4ccd7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceSmileWink = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L507) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0f64e110eb6c1864"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceSurprise = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L508) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c8fae3e1780359e5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaceTired = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L509) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b98d9a46904b28d7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Fan = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L510) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1fdf0f9bce01757a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Faucet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L511) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0151655b8f17a15f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FaucetDrip = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L512) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9942c1efeace9a8f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Fax = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L513) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6432fbab39cae410"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Feather = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L514) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ac3fb011001a8070"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FeatherPointed = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L515) | Inherited or compiler-provided contract. | — | — |
| <a id="api-74cbaee658995392"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ferry = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L516) | Inherited or compiler-provided contract. | — | — |
| <a id="api-054761b9a52e7d57"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.File = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L517) | Inherited or compiler-provided contract. | — | — |
| <a id="api-13dcd313e6aed62a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileArrowDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L518) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fb0a805852f46143"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileArrowUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L519) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ce0ec0d68280f8d5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileAudio = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L520) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bab82ee04292e432"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L521) | Inherited or compiler-provided contract. | — | — |
| <a id="api-08dbf3a31e3a59ae"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L522) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c371dc5e82fe5665"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCircleMinus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L523) | Inherited or compiler-provided contract. | — | — |
| <a id="api-54357fdaaaadfd79"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCirclePlus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L524) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cf21715aebaa5f0a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCircleQuestion = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L525) | Inherited or compiler-provided contract. | — | — |
| <a id="api-508c67d2c7068b74"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L526) | Inherited or compiler-provided contract. | — | — |
| <a id="api-14173392fba095fd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCode = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L527) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1484bde93d4d6872"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileContract = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L528) | Inherited or compiler-provided contract. | — | — |
| <a id="api-514667dc3d307fc6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileCsv = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L529) | Inherited or compiler-provided contract. | — | — |
| <a id="api-77485e538b7bbc2c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileExcel = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L530) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b516945b178af95c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileExport = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L531) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ae30eb70cd42c97b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileFragment = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L532) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7dc88133278289ef"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileHalfDashed = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L533) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f681cc78e7ac095d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileImage = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L534) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bc366dde262c9d38"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileImport = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L535) | Inherited or compiler-provided contract. | — | — |
| <a id="api-66f744f9b5c90aa4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileInvoice = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L536) | Inherited or compiler-provided contract. | — | — |
| <a id="api-db495f4f5f7e03b0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileInvoiceDollar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L537) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b5ef015db93ca799"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileLines = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L538) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c28a29f93ee76fd7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L539) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ee6dced08ac21804"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FilePdf = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L540) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f4284f8bb505e1c0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FilePen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L541) | Inherited or compiler-provided contract. | — | — |
| <a id="api-02143fb88df3708d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FilePowerpoint = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L542) | Inherited or compiler-provided contract. | — | — |
| <a id="api-63b611cc285b8787"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FilePrescription = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L543) | Inherited or compiler-provided contract. | — | — |
| <a id="api-eacb94262dd0ee4c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileShield = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L544) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3f6899c71dfed2a3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileSignature = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L545) | Inherited or compiler-provided contract. | — | — |
| <a id="api-191f8107418e0b56"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileVideo = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L546) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dd9c76a593051ceb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileWaveform = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L547) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0390038b5dc84208"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileWord = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L548) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3754331e597d98d4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FileZipper = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L549) | Inherited or compiler-provided contract. | — | — |
| <a id="api-88cea387db8e82e7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Fill = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L550) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a701ee5ce7c0964c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FillDrip = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L551) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8391b2e5806d685a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Film = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L552) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1d377a0bd6903efd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Filter = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L553) | Inherited or compiler-provided contract. | — | — |
| <a id="api-262cfe8ae27dbc4b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FilterCircleDollar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L554) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e91d908417221c90"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FilterCircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L555) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4a45cf34370708a4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Fingerprint = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L556) | Inherited or compiler-provided contract. | — | — |
| <a id="api-06d88dc6f8fe96a1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Fire = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L557) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0c172c7cf10ff847"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FireBurner = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L558) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ed6c1dafcddbc99f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FireExtinguisher = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L559) | Inherited or compiler-provided contract. | — | — |
| <a id="api-64e248fe83076522"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FireFlameCurved = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L560) | Inherited or compiler-provided contract. | — | — |
| <a id="api-452f56387e7d0707"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FireFlameSimple = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L561) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5f32f46b4b6a7f0e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Fish = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L562) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9cc491084e816490"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FishFins = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L563) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bd066a76219b0176"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Flag = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L564) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f400b174750f382b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FlagCheckered = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L565) | Inherited or compiler-provided contract. | — | — |
| <a id="api-76c5a3b2bdf879b1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FlagUsa = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L566) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3129331fb8c20fb5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Flask = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L567) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f0bd3387643d100a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FlaskVial = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L568) | Inherited or compiler-provided contract. | — | — |
| <a id="api-93b7345ab877681b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FloppyDisk = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L569) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d0cd14ae70e847d4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FlorinSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L570) | Inherited or compiler-provided contract. | — | — |
| <a id="api-799f88aded4fbcca"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Folder = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L571) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0465c0e1ac930125"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FolderClosed = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L572) | Inherited or compiler-provided contract. | — | — |
| <a id="api-212e09901f864a1f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FolderMinus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L573) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5ed2889ba0924099"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FolderOpen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L574) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5a98ea25b088fea0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FolderPlus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L575) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c57e58a12b52de5d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FolderTree = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L576) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4b1b2f795053a36c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Font = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L577) | Inherited or compiler-provided contract. | — | — |
| <a id="api-93a0cc33b1361671"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FontAwesome = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L578) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a7a97db5bd2ebc58"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Football = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L579) | Inherited or compiler-provided contract. | — | — |
| <a id="api-87765a736718c1e1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Forward = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L580) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1f3d63a4bae663fa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ForwardFast = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L581) | Inherited or compiler-provided contract. | — | — |
| <a id="api-aa2ccc2c0f6c2afb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ForwardStep = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L582) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1049041aae352d56"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.FrancSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L583) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d04e2a9368b7463d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Frog = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L584) | Inherited or compiler-provided contract. | — | — |
| <a id="api-44fd36d13bad7fdc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Futbol = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L585) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cf28c9465adb9c50"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.G = "G"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L586) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5a7c76d942ef0095"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gamepad = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L587) | Inherited or compiler-provided contract. | — | — |
| <a id="api-05f3d3093c6565d7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GasPump = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L588) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9305a32534f8f38e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gauge = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L589) | Inherited or compiler-provided contract. | — | — |
| <a id="api-16069c8dd41c90fe"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GaugeHigh = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L590) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4e3bf201b67973a0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GaugeSimple = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L591) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4180f837b2fc506c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GaugeSimpleHigh = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L592) | Inherited or compiler-provided contract. | — | — |
| <a id="api-22592e1490341ab8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gavel = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L593) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3c2542220355d516"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gear = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L594) | Inherited or compiler-provided contract. | — | — |
| <a id="api-742dc6d423dfae35"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gears = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L595) | Inherited or compiler-provided contract. | — | — |
| <a id="api-33a2fd3fb4253675"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gem = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L596) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6a5ec045ca3a304d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Genderless = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L597) | Inherited or compiler-provided contract. | — | — |
| <a id="api-73805a23c0dbdf7e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ghost = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L598) | Inherited or compiler-provided contract. | — | — |
| <a id="api-eebbdb6b184872ed"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gift = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L599) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bf87a5eb2059dfb9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gifts = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L600) | Inherited or compiler-provided contract. | — | — |
| <a id="api-22095a97ef7fbc55"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GlassWater = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L601) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f99e3048e8b93074"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GlassWaterDroplet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L602) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2b6ab4e5b42b1bb0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Glasses = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L603) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a04f4a5c3b19c034"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Globe = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L604) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dd4fefc3902e36d9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GolfBallTee = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L605) | Inherited or compiler-provided contract. | — | — |
| <a id="api-267b690ccb364f26"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gopuram = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L606) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5f0c2d805bf5cf60"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GraduationCap = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L607) | Inherited or compiler-provided contract. | — | — |
| <a id="api-813b06b60682c934"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GreaterThan = ">"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L608) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b8d9c9c88eeb7ecb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GreaterThanEqual = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L609) | Inherited or compiler-provided contract. | — | — |
| <a id="api-21c38c03abce1f98"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Grip = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L610) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b9f065ef14016391"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GripLines = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L611) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f7615d3335e17336"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GripLinesVertical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L612) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8c8f57b978248eb5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GripVertical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L613) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f7bed5fb7e04ee98"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GroupArrowsRotate = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L614) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b08b789ce0fbe899"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.GuaraniSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L615) | Inherited or compiler-provided contract. | — | — |
| <a id="api-386a25c7d8667c8d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Guitar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L616) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e3aa9d2368897dde"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Gun = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L617) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2eeb15886926c083"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.H = "H"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L618) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8cdc80f12d2b4dd3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hammer = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L619) | Inherited or compiler-provided contract. | — | — |
| <a id="api-33f7775c0978e259"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hamsa = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L620) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4224020df0b90b18"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hand = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L621) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d70f1499e631772a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandBackFist = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L622) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0b3b0205a3f22e06"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandDots = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L623) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a0ec5ea7184c1744"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandFist = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L624) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d45c916187208909"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandHolding = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L625) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8e199399c4c15ec5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandHoldingDollar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L626) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4238c02243c17862"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandHoldingDroplet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L627) | Inherited or compiler-provided contract. | — | — |
| <a id="api-59c9795c874e789f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandHoldingHand = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L628) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c0caf1423e0ba3fc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandHoldingHeart = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L629) | Inherited or compiler-provided contract. | — | — |
| <a id="api-40d01378e14b0617"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandHoldingMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L630) | Inherited or compiler-provided contract. | — | — |
| <a id="api-06bdf130570ddf88"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandLizard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L631) | Inherited or compiler-provided contract. | — | — |
| <a id="api-11918571ec0ed914"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandMiddleFinger = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L632) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7e93d2d5c040fa75"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandPeace = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L633) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1ddb1e1ad115b701"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandPointDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L634) | Inherited or compiler-provided contract. | — | — |
| <a id="api-447718f849907ee2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandPointLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L635) | Inherited or compiler-provided contract. | — | — |
| <a id="api-50e8fcb4c3cae78d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandPointRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L636) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c32286a028057673"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandPointUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L637) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1ab16daedb228b65"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandPointer = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L638) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1f0d8eda33b8b41b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandScissors = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L639) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3446ada24edd1b60"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandSparkles = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L640) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ed499de425d13c9c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandSpock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L641) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c141447616b14efa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Handcuffs = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L642) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0f7796655e497dd2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hands = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L643) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fa5455e0f4769cd6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsAslInterpreting = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L644) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9b1d8beb0784612a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsBound = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L645) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b780fd7c916c44b5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsBubbles = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L646) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cb19e8c63b120a2d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsClapping = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L647) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e16df0888baa919b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsHolding = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L648) | Inherited or compiler-provided contract. | — | — |
| <a id="api-40ddc62feaa4bb65"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsHoldingChild = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L649) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c921ab0108a29794"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsHoldingCircle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L650) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4489083e34484cba"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandsPraying = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L651) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5552d415fbf76a37"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Handshake = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L652) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e0ef5fc71c0ff5eb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandshakeAngle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L653) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3b756036d159ba13"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandshakeSimple = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L654) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1931e50a3307d194"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandshakeSimpleSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L655) | Inherited or compiler-provided contract. | — | — |
| <a id="api-56c73f84c43b5f7f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HandshakeSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L656) | Inherited or compiler-provided contract. | — | — |
| <a id="api-04073fd222b4ff59"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hanukiah = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L657) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3985a175d535e8cb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HardDrive = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L658) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1e38711bc722ebee"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hashtag = "#"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L659) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0dbea7c7ced6f52f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HatCowboy = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L660) | Inherited or compiler-provided contract. | — | — |
| <a id="api-572effef33186fad"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HatCowboySide = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L661) | Inherited or compiler-provided contract. | — | — |
| <a id="api-827fdb62713c1942"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HatWizard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L662) | Inherited or compiler-provided contract. | — | — |
| <a id="api-81b13490577192cd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeadSideCough = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L663) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e6c82a61780c1574"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeadSideCoughSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L664) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cb4d034dfb299941"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeadSideMask = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L665) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2323e3f061783866"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeadSideVirus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L666) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8d1f74cff2f1a5b5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Heading = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L667) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f2f61d0818bf3d99"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Headphones = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L668) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5df1c5212dcea5a4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeadphonesSimple = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L669) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0681a787094e2976"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Headset = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L670) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9bb0a7705dbc348b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Heart = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L671) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0b688c2f860d4e3d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCircleBolt = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L672) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3df16fc358928441"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L673) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b96d413b06487abc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L674) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2831dcb309429e9a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCircleMinus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L675) | Inherited or compiler-provided contract. | — | — |
| <a id="api-99e07410f90435ce"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCirclePlus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L676) | Inherited or compiler-provided contract. | — | — |
| <a id="api-06f884d98a6df3c8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L677) | Inherited or compiler-provided contract. | — | — |
| <a id="api-036613310c41dd1e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartCrack = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L678) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5961fe7fe908de48"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HeartPulse = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L679) | Inherited or compiler-provided contract. | — | — |
| <a id="api-956fe4e9aa34e54c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Helicopter = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L680) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f400700d0771a51e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HelicopterSymbol = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L681) | Inherited or compiler-provided contract. | — | — |
| <a id="api-708f9c280d5619a6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HelmetSafety = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L682) | Inherited or compiler-provided contract. | — | — |
| <a id="api-557172753d0df239"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HelmetUn = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L683) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c864d27211589a08"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HexagonNodes = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L684) | Inherited or compiler-provided contract. | — | — |
| <a id="api-603b6bc814dc4381"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HexagonNodesBolt = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L685) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ef94427ffb8da7b2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Highlighter = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L686) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f078a42c91a1e795"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HillAvalanche = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L687) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7b118184f476274c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HillRockslide = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L688) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cfc5bbc7611e2bf6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hippo = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L689) | Inherited or compiler-provided contract. | — | — |
| <a id="api-94bfddef2ee40f08"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HockeyPuck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L690) | Inherited or compiler-provided contract. | — | — |
| <a id="api-aa48e1b3737bb34d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HollyBerry = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L691) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b9f9f51fa8f83414"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Horse = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L692) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7471c08dbcd2ff69"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HorseHead = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L693) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c448169de047e8cd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hospital = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L694) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b1a614abfc14fb61"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HospitalUser = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L695) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d4b15153a1d79218"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HotTubPerson = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L696) | Inherited or compiler-provided contract. | — | — |
| <a id="api-08881363561c220e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hotdog = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L697) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d1d85b3bea4e9ea8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hotel = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L698) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b5521bc4537bf3a3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hourglass = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L699) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4dd679a449d26b51"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HourglassEnd = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L700) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c5313b34f80cacd0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HourglassHalf = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L701) | Inherited or compiler-provided contract. | — | — |
| <a id="api-818633f573c32edc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HourglassStart = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L702) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4ad299b0dc77780b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.House = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L703) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3991a7fb8c7e83f0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseChimney = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L704) | Inherited or compiler-provided contract. | — | — |
| <a id="api-623139a0debd3c07"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseChimneyCrack = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L705) | Inherited or compiler-provided contract. | — | — |
| <a id="api-77a360772c022c43"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseChimneyMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L706) | Inherited or compiler-provided contract. | — | — |
| <a id="api-18c1e77319202c13"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseChimneyUser = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L707) | Inherited or compiler-provided contract. | — | — |
| <a id="api-08447590a4412c93"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseChimneyWindow = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L708) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8de00c1509c4705c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L709) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bc8d04bb5e2bea5a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseCircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L710) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d460b8b22a142572"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseCircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L711) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6ae15fb8676fc275"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseCrack = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L712) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3c21bae1bbcc0ada"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseFire = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L713) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3ce29af68dbbcece"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseFlag = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L714) | Inherited or compiler-provided contract. | — | — |
| <a id="api-eaa5206b4c529206"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseFloodWater = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L715) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4bbc2967894ad2d7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseFloodWaterCircleArrowRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L716) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c8de934a7f706652"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseLaptop = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L717) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ca97b7bccd7f3d1f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseLock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L718) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dd9465d47469be52"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L719) | Inherited or compiler-provided contract. | — | — |
| <a id="api-858c52c03745561f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseMedicalCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L720) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c6fe78126061187f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseMedicalCircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L721) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dcd208b9668b9d3b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseMedicalCircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L722) | Inherited or compiler-provided contract. | — | — |
| <a id="api-65a2f3fc9e28f309"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseMedicalFlag = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L723) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6345aed9aae72f5b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseSignal = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L724) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c8930e3251af3d81"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseTsunami = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L725) | Inherited or compiler-provided contract. | — | — |
| <a id="api-44d6dfe204b4b521"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HouseUser = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L726) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5a418fad49a15eca"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.HryvniaSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L727) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1f60e11409f14909"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Hurricane = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L728) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c2813dbea4410da4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.I = "I"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L729) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5937e6a2a081aee1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ICursor = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L730) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9ec6eea5d5e9cddf"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.IceCream = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L731) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1d3ba3210a418442"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Icicles = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L732) | Inherited or compiler-provided contract. | — | — |
| <a id="api-96e28b3993845be0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Icons = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L733) | Inherited or compiler-provided contract. | — | — |
| <a id="api-df53136f3da253fc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.IdBadge = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L734) | Inherited or compiler-provided contract. | — | — |
| <a id="api-13440e629220c66b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.IdCard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L735) | Inherited or compiler-provided contract. | — | — |
| <a id="api-11748ec3db4bb1ad"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.IdCardClip = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L736) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6cd011b7910cff04"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Igloo = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L737) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dfdedb1c4e092675"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Image = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L738) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e9ce5d27cd14fb3d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ImagePortrait = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L739) | Inherited or compiler-provided contract. | — | — |
| <a id="api-76f094cff6d47d5e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Images = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L740) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e448fece4f72a7bc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Inbox = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L741) | Inherited or compiler-provided contract. | — | — |
| <a id="api-211a1d5ddbe73c44"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Indent = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L742) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f02e34103e61e3bb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.IndianRupeeSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L743) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6882bcff71133053"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Industry = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L744) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bedb5a5a051a7694"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Infinity = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L745) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6e741b38d4d08477"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Info = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L746) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8e26557fad3b0662"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Italic = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L747) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6105c6b84be78e6e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.J = "J"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L748) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d89a54e3fefbc67b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Jar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L749) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f08b7f9668e7fc7d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.JarWheat = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L750) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ed54e6aae5d9f4c2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Jedi = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L751) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0c5281a5bfc0b025"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.JetFighter = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L752) | Inherited or compiler-provided contract. | — | — |
| <a id="api-72eb2fa6c0ca95fd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.JetFighterUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L753) | Inherited or compiler-provided contract. | — | — |
| <a id="api-96216d9b28c149e1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Joint = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L754) | Inherited or compiler-provided contract. | — | — |
| <a id="api-97de31c0e20e9fe1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.JugDetergent = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L755) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e14623a82377d446"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.K = "K"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L756) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9ea39e1bd7b23266"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Kaaba = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L757) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6d2a9cba234bb951"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Key = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L758) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d83e4cc1636ced1f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Keyboard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L759) | Inherited or compiler-provided contract. | — | — |
| <a id="api-53885b786d220261"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Khanda = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L760) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5b5253d76a058a32"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.KipSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L761) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f1eecd66dfd126b8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.KitMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L762) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ce47555892a1fe33"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.KitchenSet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L763) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e61bd5ffbc3dd115"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.KiwiBird = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L764) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3011b5a5d9fcf452"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.L = "L"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L765) | Inherited or compiler-provided contract. | — | — |
| <a id="api-12cdfc558aa96187"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LandMineOn = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L766) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cdfb6124ab50a090"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Landmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L767) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ccedd2b07e8e002d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LandmarkDome = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L768) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fe4144cef23a9e08"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LandmarkFlag = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L769) | Inherited or compiler-provided contract. | — | — |
| <a id="api-32d5b182c4395416"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Language = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L770) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ecd194e3e78b59e2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Laptop = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L771) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4fd2c27de1482529"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LaptopCode = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L772) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9913ddf96f2b0aa9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LaptopFile = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L773) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c56277b09f110d6d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LaptopMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L774) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8cf3c67442280fca"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LariSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L775) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f1237c696fd4265a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LayerGroup = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L776) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4d479fcfd4707d8a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Leaf = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L777) | Inherited or compiler-provided contract. | — | — |
| <a id="api-368ce7600618d2fb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LeftLong = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L778) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f64c83c9b2022404"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LeftRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L779) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e81b8a1246ad5aac"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Lemon = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L780) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ee741fdcf5e96afd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LessThan = "<"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L781) | Inherited or compiler-provided contract. | — | — |
| <a id="api-89d82ce44c97ceac"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LessThanEqual = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L782) | Inherited or compiler-provided contract. | — | — |
| <a id="api-345fb0916e92f042"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LifeRing = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L783) | Inherited or compiler-provided contract. | — | — |
| <a id="api-eac4b35bf77ab165"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Lightbulb = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L784) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0b387f65f501713e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LinesLeaning = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L785) | Inherited or compiler-provided contract. | — | — |
| <a id="api-76c91bd4a5b0ddd1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Link = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L786) | Inherited or compiler-provided contract. | — | — |
| <a id="api-99d6d60dffb4e178"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LinkSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L787) | Inherited or compiler-provided contract. | — | — |
| <a id="api-248a958c622e654e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LiraSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L788) | Inherited or compiler-provided contract. | — | — |
| <a id="api-30c6eeec034bd1c5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.List = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L789) | Inherited or compiler-provided contract. | — | — |
| <a id="api-96047a6670414e6e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ListCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L790) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6545ce8656461872"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ListOl = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L791) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0e5619911e746efc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ListUl = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L792) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2e7d2689865640e7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LitecoinSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L793) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c8d70e849a370a5d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LocationArrow = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L794) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9794a36e6e992e23"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LocationCrosshairs = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L795) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9607d1ca56efb1c8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LocationDot = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L796) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7b22208c3dcfbec8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LocationPin = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L797) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7d57efd908bd23a2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LocationPinLock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L798) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a61f4d6c7d777c8f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Lock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L799) | Inherited or compiler-provided contract. | — | — |
| <a id="api-31fc673cf8ade591"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LockOpen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L800) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0689e38b18dbcc56"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Locust = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L801) | Inherited or compiler-provided contract. | — | — |
| <a id="api-eb24f92253a7a536"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Lungs = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L802) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ab286c0cce229ba0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.LungsVirus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L803) | Inherited or compiler-provided contract. | — | — |
| <a id="api-248ff6333641e69f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.M = "M"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L804) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ad0bc883385ca26d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Magnet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L805) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0390cf09606ff1a1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlass = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L806) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c23f79e131aa7b67"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlassArrowRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L807) | Inherited or compiler-provided contract. | — | — |
| <a id="api-441e340cf21c0970"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlassChart = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L808) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cbe58c3ba60922e2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlassDollar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L809) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a2910e24221476e2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlassLocation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L810) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3818ad429eb70de9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlassMinus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L811) | Inherited or compiler-provided contract. | — | — |
| <a id="api-95abfde815554302"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MagnifyingGlassPlus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L812) | Inherited or compiler-provided contract. | — | — |
| <a id="api-474a7164e41159c4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ManatSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L813) | Inherited or compiler-provided contract. | — | — |
| <a id="api-514adc18c589bce1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Map = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L814) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9022d3f9d1fc5a7c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MapLocation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L815) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8aa130496a7eacaa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MapLocationDot = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L816) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e45b06007fbcb39c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MapPin = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L817) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e292a87adde0b5cd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Marker = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L818) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e1471480e2031958"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mars = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L819) | Inherited or compiler-provided contract. | — | — |
| <a id="api-79e35b98895737fa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MarsAndVenus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L820) | Inherited or compiler-provided contract. | — | — |
| <a id="api-aa18c2c62ba8ef0f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MarsAndVenusBurst = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L821) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9068967fc5f90669"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MarsDouble = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L822) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4f951d5da273fb1b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MarsStroke = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L823) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2267f4f76dbface6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MarsStrokeRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L824) | Inherited or compiler-provided contract. | — | — |
| <a id="api-885e92ead22b5c81"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MarsStrokeUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L825) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4cd4c82ee8709b2a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MartiniGlass = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L826) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8fe42528efecf5c6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MartiniGlassCitrus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L827) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c3103207c1661858"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MartiniGlassEmpty = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L828) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f6701a47c57052f5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mask = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L829) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0c72b3116beb95f0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MaskFace = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L830) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c1204a08ff43b17d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MaskVentilator = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L831) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6ea785737f30f73a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MasksTheater = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L832) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c2d9d36f1f6285d2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MattressPillow = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L833) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b64ea53d34770aeb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Maximize = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L834) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f40c5470f0799240"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Medal = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L835) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6ff72fc875739e14"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Memory = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L836) | Inherited or compiler-provided contract. | — | — |
| <a id="api-04495e4a5c413230"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Menorah = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L837) | Inherited or compiler-provided contract. | — | — |
| <a id="api-326f8fabb0af8d35"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mercury = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L838) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d8744b739bd0299a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Message = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L839) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9c24544242244c06"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Meteor = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L840) | Inherited or compiler-provided contract. | — | — |
| <a id="api-15ae02712d28ca29"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Microchip = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L841) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9fd141685b5d57a7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Microphone = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L842) | Inherited or compiler-provided contract. | — | — |
| <a id="api-77f73caf467e1b3a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MicrophoneLines = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L843) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e732abe6670a0446"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MicrophoneLinesSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L844) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5ae8a929b8362ea5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MicrophoneSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L845) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ea95953c9492ac27"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Microscope = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L846) | Inherited or compiler-provided contract. | — | — |
| <a id="api-520e42fa12b7780d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MillSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L847) | Inherited or compiler-provided contract. | — | — |
| <a id="api-77062b96137f146d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Minimize = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L848) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d301ba0e1fc37d18"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Minus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L849) | Inherited or compiler-provided contract. | — | — |
| <a id="api-915c3b165ea5649d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mitten = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L850) | Inherited or compiler-provided contract. | — | — |
| <a id="api-124700e30c478b9c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mobile = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L851) | Inherited or compiler-provided contract. | — | — |
| <a id="api-96ff1439fefc1bd2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MobileButton = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L852) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b34920ac9668d2fe"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MobileRetro = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L853) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2d7119b13b9376c0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MobileScreen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L854) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e5b6944fb849d450"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MobileScreenButton = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L855) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5d5d3de260445738"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBill = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L856) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a192206f5e773716"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBill1 = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L857) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a4a6ccdef481a98f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBill1Wave = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L858) | Inherited or compiler-provided contract. | — | — |
| <a id="api-58bad8aad07a00ec"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBillTransfer = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L859) | Inherited or compiler-provided contract. | — | — |
| <a id="api-88051a23724cbeb0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBillTrendUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L860) | Inherited or compiler-provided contract. | — | — |
| <a id="api-344a2ce3174a3673"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBillWave = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L861) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c2887191697fbac4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBillWheat = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L862) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6f1c20a88587a9c2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyBills = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L863) | Inherited or compiler-provided contract. | — | — |
| <a id="api-10c61d80fa17b24d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L864) | Inherited or compiler-provided contract. | — | — |
| <a id="api-07f2fb3ab6b2c7a9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MoneyCheckDollar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L865) | Inherited or compiler-provided contract. | — | — |
| <a id="api-17aa973d78592d91"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Monument = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L866) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ae919ee3d003bb0c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Moon = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L867) | Inherited or compiler-provided contract. | — | — |
| <a id="api-206a3cd1a001f030"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MortarPestle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L868) | Inherited or compiler-provided contract. | — | — |
| <a id="api-32f5603bbbf8c434"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mosque = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L869) | Inherited or compiler-provided contract. | — | — |
| <a id="api-415de8087dfb637b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mosquito = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L870) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4b3380508680dca8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MosquitoNet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L871) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7c76fd08128e8d46"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Motorcycle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L872) | Inherited or compiler-provided contract. | — | — |
| <a id="api-810bf9b20c97034f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mound = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L873) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f7017d0068547f17"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Mountain = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L874) | Inherited or compiler-provided contract. | — | — |
| <a id="api-78471c0acd73e728"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MountainCity = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L875) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0e45d5cdcd038d06"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MountainSun = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L876) | Inherited or compiler-provided contract. | — | — |
| <a id="api-417955e324950ae4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MugHot = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L877) | Inherited or compiler-provided contract. | — | — |
| <a id="api-840a76c63b85ae40"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.MugSaucer = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L878) | Inherited or compiler-provided contract. | — | — |
| <a id="api-51d9d1ed7399cba4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Music = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L879) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2b0ebaf921c01a24"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.N = "N"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L880) | Inherited or compiler-provided contract. | — | — |
| <a id="api-980690471c99a8f3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.NairaSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L881) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a3a460d93c883cc1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.NetworkWired = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L882) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dbe354c601db7077"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Neuter = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L883) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4ae3873ada643b57"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Newspaper = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L884) | Inherited or compiler-provided contract. | — | — |
| <a id="api-df15e1c28db4fa75"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.NotEqual = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L885) | Inherited or compiler-provided contract. | — | — |
| <a id="api-eb69a84c8c5ff628"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Notdef = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L886) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d5ae455945929b40"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.NoteSticky = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L887) | Inherited or compiler-provided contract. | — | — |
| <a id="api-01c963263ce1f48d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.NotesMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L888) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c19ac99c8a0eae41"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.O = "O"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L889) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e94740c7d8471cf7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ObjectGroup = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L890) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4660d30d77e54e1a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ObjectUngroup = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L891) | Inherited or compiler-provided contract. | — | — |
| <a id="api-99c3dff90b67fb58"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.OilCan = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L892) | Inherited or compiler-provided contract. | — | — |
| <a id="api-abb23bc262ebbcbc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.OilWell = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L893) | Inherited or compiler-provided contract. | — | — |
| <a id="api-36b360f1e4d7b6b7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Om = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L894) | Inherited or compiler-provided contract. | — | — |
| <a id="api-81b72ee7fb217c6c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Otter = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L895) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0003fec6e720bd96"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Outdent = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L896) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c81493df13ef7ce7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.P = "P"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L897) | Inherited or compiler-provided contract. | — | — |
| <a id="api-841cd6a1975fbe94"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Pager = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L898) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7c1fb4c681fdd8c1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PaintRoller = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L899) | Inherited or compiler-provided contract. | — | — |
| <a id="api-604385f7340cf98e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Paintbrush = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L900) | Inherited or compiler-provided contract. | — | — |
| <a id="api-87b312d21f1dc97d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Palette = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L901) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0a95215d52b3bb8a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Pallet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L902) | Inherited or compiler-provided contract. | — | — |
| <a id="api-03c8be9892cf0ba6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Panorama = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L903) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d4b7e1a64f8d44e7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PaperPlane = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L904) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7fec47c0b51bec64"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Paperclip = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L905) | Inherited or compiler-provided contract. | — | — |
| <a id="api-59cf4e5ca64db067"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ParachuteBox = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L906) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ce84c60c566f1583"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Paragraph = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L907) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6ce3bc703652de1d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Passport = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L908) | Inherited or compiler-provided contract. | — | — |
| <a id="api-481a9dab46863296"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Paste = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L909) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e1567388a0a17981"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Pause = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L910) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b5d87643b5b2a4aa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Paw = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L911) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3ccc7cda93e0e62d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Peace = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L912) | Inherited or compiler-provided contract. | — | — |
| <a id="api-611777a27938d83b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Pen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L913) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1e62743df77df7b0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PenClip = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L914) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3bb32d69728bb515"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PenFancy = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L915) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3a0a718078d9e0db"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PenNib = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L916) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ac1971b3ab29680c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PenRuler = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L917) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9747fb20f0ae08df"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PenToSquare = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L918) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f81660bf9e142dae"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Pencil = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L919) | Inherited or compiler-provided contract. | — | — |
| <a id="api-66f5fe62419d8d3e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeopleArrows = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L920) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3391c189a5973675"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeopleCarryBox = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L921) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cf266a714e5d52bc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeopleGroup = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L922) | Inherited or compiler-provided contract. | — | — |
| <a id="api-acab5299041ec272"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeopleLine = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L923) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a5c560f33f81b945"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeoplePulling = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L924) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7b81641c3e2eecbf"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeopleRobbery = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L925) | Inherited or compiler-provided contract. | — | — |
| <a id="api-62e33fc57088b424"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PeopleRoof = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L926) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bbeda386a39e91ee"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PepperHot = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L927) | Inherited or compiler-provided contract. | — | — |
| <a id="api-36074cb5f2c35b12"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Percent = "%"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L928) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6daae62b68a391cd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Person = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L929) | Inherited or compiler-provided contract. | — | — |
| <a id="api-02579ad6d2055529"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonArrowDownToLine = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L930) | Inherited or compiler-provided contract. | — | — |
| <a id="api-98541c79f283aba5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonArrowUpFromLine = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L931) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e85bad21da4a257e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonBiking = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L932) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3fa2928ab54db4b6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonBooth = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L933) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0cb2eb399441f5d4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonBreastfeeding = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L934) | Inherited or compiler-provided contract. | — | — |
| <a id="api-22da3b44dbce982f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonBurst = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L935) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a8a29f570a14cc3b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCane = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L936) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d7d3b50821af85c8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonChalkboard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L937) | Inherited or compiler-provided contract. | — | — |
| <a id="api-54bb3f9315ad60c3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L938) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4b7d98bc9d236d8f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L939) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1148841f4ac5c8d0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCircleMinus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L940) | Inherited or compiler-provided contract. | — | — |
| <a id="api-db08375c2ca30fcf"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCirclePlus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L941) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f53ff2130c946f3f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCircleQuestion = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L942) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b29193919c825599"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonCircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L943) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a39bc34f735a4c9a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonDigging = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L944) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e7b8ba0b21899caf"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonDotsFromLine = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L945) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1dd22f5472766df7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonDress = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L946) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a8348c09a9c0ca20"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonDressBurst = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L947) | Inherited or compiler-provided contract. | — | — |
| <a id="api-be91a787ed6dee5e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonDrowning = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L948) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3ffe80175bfa343b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonFalling = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L949) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bf39794b35b69487"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonFallingBurst = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L950) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cfdcdf29e14bc55c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonHalfDress = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L951) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b1ea533e72366a37"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonHarassing = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L952) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ffe548996232418d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonHiking = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L953) | Inherited or compiler-provided contract. | — | — |
| <a id="api-74feaefc1f1cdf5a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonMilitaryPointing = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L954) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0d2ad0625f360e38"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonMilitaryRifle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L955) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5ce622541e65770e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonMilitaryToPerson = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L956) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f198c8c8948b9cc4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonPraying = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L957) | Inherited or compiler-provided contract. | — | — |
| <a id="api-98ea06f79dd000ab"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonPregnant = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L958) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ca8cea45dbb54aa4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonRays = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L959) | Inherited or compiler-provided contract. | — | — |
| <a id="api-929dbf9bd66f82dd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonRifle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L960) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cccd6c1a5dafd084"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonRunning = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L961) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2ae27ec7a2d3b771"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonShelter = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L962) | Inherited or compiler-provided contract. | — | — |
| <a id="api-82d3d04ff4d75c4f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonSkating = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L963) | Inherited or compiler-provided contract. | — | — |
| <a id="api-da45d3a3322c48b5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonSkiing = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L964) | Inherited or compiler-provided contract. | — | — |
| <a id="api-275aa232a86fc210"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonSkiingNordic = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L965) | Inherited or compiler-provided contract. | — | — |
| <a id="api-68ff7be88f9e5416"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonSnowboarding = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L966) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2da368d097cb61d4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonSwimming = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L967) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9aebf448e2528f56"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonThroughWindow = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L968) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5116e3753572849d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonWalking = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L969) | Inherited or compiler-provided contract. | — | — |
| <a id="api-04ac8236891f0312"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonWalkingArrowLoopLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L970) | Inherited or compiler-provided contract. | — | — |
| <a id="api-99f8f08c84b3f707"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonWalkingArrowRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L971) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b3a7dec0f4d499d9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonWalkingDashedLineArrowRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L972) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8b1d91eebaf91f56"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonWalkingLuggage = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L973) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0ed734d9746dd95a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PersonWalkingWithCane = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L974) | Inherited or compiler-provided contract. | — | — |
| <a id="api-79d8c2f72de908ab"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PesetaSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L975) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6856c6e8d4d091ea"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PesoSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L976) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cf0416a9a4d16724"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Phone = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L977) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3284e2429c7b5f22"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PhoneFlip = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L978) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3b77869435a09e3c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PhoneSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L979) | Inherited or compiler-provided contract. | — | — |
| <a id="api-41ed932f1fa48eda"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PhoneVolume = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L980) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9afbea6216dfab9c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PhotoFilm = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L981) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9a32c339f752fc3e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PiggyBank = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L982) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ac67e6b1661a01aa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Pills = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L983) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8c4a07423ee1a6ae"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PizzaSlice = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L984) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0848e38e7c4517f7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaceOfWorship = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L985) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ee888c455c46ca5f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Plane = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L986) | Inherited or compiler-provided contract. | — | — |
| <a id="api-68b2e438ee35e1c6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneArrival = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L987) | Inherited or compiler-provided contract. | — | — |
| <a id="api-02b023b74cb37a74"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L988) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0b2dee2abb68f017"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneCircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L989) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f9392efd4e2578c8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneCircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L990) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ac43467e9fd224c9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneDeparture = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L991) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8eb28d5a399e66fe"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneLock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L992) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8d14f145199066da"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L993) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b7b141f3f3bc6e26"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlaneUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L994) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f5033055bfb87123"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlantWilt = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L995) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f5d2326323fc4372"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlateWheat = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L996) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5fb257135388288b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Play = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L997) | Inherited or compiler-provided contract. | — | — |
| <a id="api-697be4218d7a0ccf"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Plug = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L998) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bb6922df674c615d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlugCircleBolt = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L999) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a38a7f26050eb057"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlugCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1000) | Inherited or compiler-provided contract. | — | — |
| <a id="api-97dc1b8f5d25ddb7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlugCircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1001) | Inherited or compiler-provided contract. | — | — |
| <a id="api-64267a5dd7425c20"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlugCircleMinus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1002) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fa4ebdbdc228c668"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlugCirclePlus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1003) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3f2624ef97263be6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlugCircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1004) | Inherited or compiler-provided contract. | — | — |
| <a id="api-923aa5011fcb76d7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Plus = "+"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1005) | Inherited or compiler-provided contract. | — | — |
| <a id="api-72cb234b8d3b2978"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PlusMinus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1006) | Inherited or compiler-provided contract. | — | — |
| <a id="api-645062bc7bf63e5f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Podcast = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1007) | Inherited or compiler-provided contract. | — | — |
| <a id="api-108dcd41810071a6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Poo = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1008) | Inherited or compiler-provided contract. | — | — |
| <a id="api-921fdd530c01d857"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PooStorm = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1009) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6c6407bc6f0e6432"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Poop = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1010) | Inherited or compiler-provided contract. | — | — |
| <a id="api-318cb108873802fa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PowerOff = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1011) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6685beff64e304a2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Prescription = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1012) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e08277087acee49b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PrescriptionBottle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1013) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2be3c2fe84c136a9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PrescriptionBottleMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1014) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5ad2495ce77b8883"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Print = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1015) | Inherited or compiler-provided contract. | — | — |
| <a id="api-116e315b3b9751c8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PumpMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1016) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ad66547b480b22c0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PumpSoap = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1017) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e796922f0b0f5ad3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.PuzzlePiece = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1018) | Inherited or compiler-provided contract. | — | — |
| <a id="api-093d6744095e22d4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Q = "Q"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1019) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dd3ba001caa53948"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Qrcode = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1020) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3861f7a8d2045eb3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Question = "?"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1021) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dd46260d452bed2c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.QuoteLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1022) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e219bb76ccbee95b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.QuoteRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1023) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7ceef3f0c2293326"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.R = "R"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1024) | Inherited or compiler-provided contract. | — | — |
| <a id="api-36a5126ad6e29c0c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Radiation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1025) | Inherited or compiler-provided contract. | — | — |
| <a id="api-214dde3076f76940"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Radio = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1026) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6430099168fda025"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Rainbow = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1027) | Inherited or compiler-provided contract. | — | — |
| <a id="api-544568b522cef169"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RankingStar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1028) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2039e0036475cc73"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Receipt = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1029) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ed35d555a2e44f32"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RecordVinyl = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1030) | Inherited or compiler-provided contract. | — | — |
| <a id="api-664ec02755bfbcd6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RectangleAd = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1031) | Inherited or compiler-provided contract. | — | — |
| <a id="api-68c876c66f619603"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RectangleList = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1032) | Inherited or compiler-provided contract. | — | — |
| <a id="api-22e232517c12aff7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RectangleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1033) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4857a1f184ec2ef9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Recycle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1034) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6251b4812bf70e98"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Registered = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1035) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c9d01380b8ddcc30"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Repeat = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1036) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9f6176f4c5b124fa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Reply = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1037) | Inherited or compiler-provided contract. | — | — |
| <a id="api-617b15ec43c557c2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ReplyAll = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1038) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dc289e05c6e7e190"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Republican = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1039) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5ea384a26948e3d3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Restroom = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1040) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f8304b95b427beb4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Retweet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1041) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e47be77314a252b9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ribbon = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1042) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7c36b54b5b73164e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RightFromBracket = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1043) | Inherited or compiler-provided contract. | — | — |
| <a id="api-072fd8402bf24796"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RightLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1044) | Inherited or compiler-provided contract. | — | — |
| <a id="api-06d1140fd100b04c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RightLong = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1045) | Inherited or compiler-provided contract. | — | — |
| <a id="api-30e5c9f58054dba2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RightToBracket = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1046) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ebe00e8067056368"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ring = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1047) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bd9cfd8b69df795a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Road = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1048) | Inherited or compiler-provided contract. | — | — |
| <a id="api-216f79b2ea91774b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadBarrier = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1049) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1c9f2f40464255f1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadBridge = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1050) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e6c17fc2805312c1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1051) | Inherited or compiler-provided contract. | — | — |
| <a id="api-23134bae47f92dc9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadCircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1052) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6d1728257e00f5a7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadCircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1053) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b28faa6ea0b021a1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadLock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1054) | Inherited or compiler-provided contract. | — | — |
| <a id="api-95de3d1c8a22f6c0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RoadSpikes = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1055) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4a05a42a9fa58f6b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Robot = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1056) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f50737a5db7428eb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Rocket = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1057) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2b75e30cdae93f86"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Rotate = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1058) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8304da8e67ceccc1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RotateLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1059) | Inherited or compiler-provided contract. | — | — |
| <a id="api-587866c24cd53bc7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RotateRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1060) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8b3a6fa350819000"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Route = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1061) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b2360df28654df28"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Rss = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1062) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8f2d349d1ad93664"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RubleSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1063) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7b2604d5d0115baa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Rug = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1064) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6709b7437488a4d5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ruler = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1065) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e84c79e402e896c7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RulerCombined = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1066) | Inherited or compiler-provided contract. | — | — |
| <a id="api-aecaa03701b81388"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RulerHorizontal = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1067) | Inherited or compiler-provided contract. | — | — |
| <a id="api-75dda0cbc38e3a34"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RulerVertical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1068) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1b6f36d98cafe3cd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RupeeSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1069) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5ebc54784e17aa74"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.RupiahSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1070) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c5a6b00675e02e61"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.S = "S"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1071) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3adab0bd6befa02e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SackDollar = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1072) | Inherited or compiler-provided contract. | — | — |
| <a id="api-79be0b1fe14de984"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SackXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1073) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3a2f21bd860d57e5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sailboat = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1074) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2f22f2529f53b9a5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Satellite = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1075) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6e01a8147557757a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SatelliteDish = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1076) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7c42a228a2c37478"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ScaleBalanced = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1077) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d28151c273098a8f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ScaleUnbalanced = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1078) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ce847ddabd69342c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ScaleUnbalancedFlip = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1079) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f99148c3af2bacb9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.School = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1080) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c9c3092836ac7404"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SchoolCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1081) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6865e99c2da5f4dc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SchoolCircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1082) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b91d891f8c9a0e2d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SchoolCircleXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1083) | Inherited or compiler-provided contract. | — | — |
| <a id="api-95deee217763700b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SchoolFlag = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1084) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a3b2523b07856240"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SchoolLock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1085) | Inherited or compiler-provided contract. | — | — |
| <a id="api-56cc96304b8a1859"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Scissors = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1086) | Inherited or compiler-provided contract. | — | — |
| <a id="api-202d52198adc52c7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Screwdriver = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1087) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6de0123fd5d0492e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ScrewdriverWrench = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1088) | Inherited or compiler-provided contract. | — | — |
| <a id="api-894e69fff361cd88"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Scroll = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1089) | Inherited or compiler-provided contract. | — | — |
| <a id="api-01cea4d510d14d0b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ScrollTorah = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1090) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4ba1ae4224b3a124"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SdCard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1091) | Inherited or compiler-provided contract. | — | — |
| <a id="api-92838d763c81e5ef"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Section = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1092) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c893fe7207f60949"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Seedling = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1093) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0551144a93e2e43c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Server = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1094) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d599fbf5231f41af"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shapes = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1095) | Inherited or compiler-provided contract. | — | — |
| <a id="api-28a75669fb668ac4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Share = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1096) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cb3b5beafbc4d0cc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShareFromSquare = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1097) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2b44c5fbcd6a29a9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShareNodes = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1098) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cd339718c6fce321"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SheetPlastic = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1099) | Inherited or compiler-provided contract. | — | — |
| <a id="api-64f5f5a0b316c34c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShekelSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1100) | Inherited or compiler-provided contract. | — | — |
| <a id="api-103a676b411e3c20"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shield = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1101) | Inherited or compiler-provided contract. | — | — |
| <a id="api-76a3e11a48128c39"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShieldCat = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1102) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0df7a6b59ac9f5a4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShieldDog = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1103) | Inherited or compiler-provided contract. | — | — |
| <a id="api-df528497d7bf28eb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShieldHalved = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1104) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a82ccd30ca1b7c17"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShieldHeart = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1105) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fbf7edf0db5f0642"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShieldVirus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1106) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e716601595cf941d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ship = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1107) | Inherited or compiler-provided contract. | — | — |
| <a id="api-156524c89ce0a072"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shirt = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1108) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8b7dcfa17f368340"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShoePrints = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1109) | Inherited or compiler-provided contract. | — | — |
| <a id="api-641fd13503518b45"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shop = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1110) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ba932e853b38248f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShopLock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1111) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bacf641e09c242b2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShopSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1112) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4552fd8a036e26b9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shower = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1113) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dc140c2cce8cd976"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shrimp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1114) | Inherited or compiler-provided contract. | — | — |
| <a id="api-61387ea384d1ffbb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Shuffle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1115) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c8b38c11847610af"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ShuttleSpace = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1116) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1a506625be49d097"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SignHanging = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1117) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8d0238214b93f5b9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Signal = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1118) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b96e5b95f59873d2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Signature = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1119) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d61251ce07f1afe1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SignsPost = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1120) | Inherited or compiler-provided contract. | — | — |
| <a id="api-823269d9f7c8bafc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SimCard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1121) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ffdee009ca689fae"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sink = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1122) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4722faf594932133"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sitemap = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1123) | Inherited or compiler-provided contract. | — | — |
| <a id="api-abb9eee867d61ca1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Skull = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1124) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0d8882a21bca7edb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SkullCrossbones = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1125) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c224cdb97d7376fc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Slash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1126) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cf1a066ece1055c8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sleigh = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1127) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fcb9c96f07a028ce"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sliders = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1128) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ceb067328daba06a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Smog = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1129) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2de88aa8bf4b1e54"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Smoking = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1130) | Inherited or compiler-provided contract. | — | — |
| <a id="api-abde413e3d751368"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Snowflake = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1131) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0dc14e78b571ebd2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Snowman = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1132) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3e767a0b569f640e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Snowplow = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1133) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2181546c6d86bb5d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Soap = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1134) | Inherited or compiler-provided contract. | — | — |
| <a id="api-af1d90375092e490"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Socks = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1135) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b79cedeac56bf8f9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SolarPanel = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1136) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f70f0222a3443a2d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sort = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1137) | Inherited or compiler-provided contract. | — | — |
| <a id="api-69f5f36d8e52c9ec"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SortDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1138) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c25ca1deb80368a4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SortUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1139) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8f39feef2ba24acd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Spa = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1140) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0a8ca79b1f5dc050"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SpaghettiMonsterFlying = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1141) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3b375692a43ddc06"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SpellCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1142) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2c3b327efc21b219"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Spider = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1143) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e61c8b01dbafc921"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Spinner = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1144) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fd5d3380eac8c5c3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Splotch = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1145) | Inherited or compiler-provided contract. | — | — |
| <a id="api-49a0587855e299e6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Spoon = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1146) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3cb35abe077b996a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SprayCan = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1147) | Inherited or compiler-provided contract. | — | — |
| <a id="api-84659616520692fa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SprayCanSparkles = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1148) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a2816733654d211a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Square = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1149) | Inherited or compiler-provided contract. | — | — |
| <a id="api-187f0714edacee62"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareArrowUpRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1150) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5bd5786403c5c86f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareBinary = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1151) | Inherited or compiler-provided contract. | — | — |
| <a id="api-89d309930cae1d33"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareCaretDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1152) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1746687f7ec87d1c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareCaretLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1153) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2967c7b607e2ffbb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareCaretRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1154) | Inherited or compiler-provided contract. | — | — |
| <a id="api-12513642df6f95b1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareCaretUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1155) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ce3f9e987c38147e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1156) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f3b23aef6836e43f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareEnvelope = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1157) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cb42a456ab0a1d5c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareFull = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1158) | Inherited or compiler-provided contract. | — | — |
| <a id="api-44e4991210371283"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareH = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1159) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dda4a9f5193640d1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareMinus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1160) | Inherited or compiler-provided contract. | — | — |
| <a id="api-46a1aa3015459f8f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareNfi = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1161) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b6f62a488e96b135"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareParking = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1162) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9504c2f9d2789176"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1163) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5cbc5846a6c9443e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePersonConfined = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1164) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b7ed3de8244a5c5d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePhone = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1165) | Inherited or compiler-provided contract. | — | — |
| <a id="api-aa3af0cb792b2f7d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePhoneFlip = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1166) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2d9efd9bb3bd165d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePlus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1167) | Inherited or compiler-provided contract. | — | — |
| <a id="api-502fad117fa3ef43"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePollHorizontal = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1168) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6158ca08980135bb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquarePollVertical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1169) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a8810bf6b8812dd8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareRootVariable = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1170) | Inherited or compiler-provided contract. | — | — |
| <a id="api-95df4be16f09b376"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareRss = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1171) | Inherited or compiler-provided contract. | — | — |
| <a id="api-936fd0db5e8fbe8e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareShareNodes = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1172) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c3817e660d5dd3c5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareUpRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1173) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f0d6c5ddc967b76d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareVirus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1174) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e7676f3718d3793e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SquareXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1175) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bece84a9b09f25d9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.StaffSnake = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1176) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7d7256e8ab815fe3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stairs = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1177) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5729d7a559e44576"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stamp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1178) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dbd1d89a3de5c16c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stapler = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1179) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f0a2d66e9f9cce74"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Star = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1180) | Inherited or compiler-provided contract. | — | — |
| <a id="api-13628de596844214"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.StarAndCrescent = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1181) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3a33874464230452"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.StarHalf = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1182) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ff3284a9e86ed18a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.StarHalfStroke = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1183) | Inherited or compiler-provided contract. | — | — |
| <a id="api-85288aaaae04008a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.StarOfDavid = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1184) | Inherited or compiler-provided contract. | — | — |
| <a id="api-997573c499a8d867"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.StarOfLife = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1185) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f07a5586425e3fed"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SterlingSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1186) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6f3b2eb9d2ec0aee"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stethoscope = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1187) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ebd1689d1d247bc4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stop = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1188) | Inherited or compiler-provided contract. | — | — |
| <a id="api-221d6b1627c20431"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stopwatch = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1189) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dc490d1c1e428008"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stopwatch20 = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1190) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f913145de618062a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Store = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1191) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ca9c0a191e090e5a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.StoreSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1192) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1ef98a7fcb556083"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.StreetView = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1193) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a65fae5053dd3e1c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Strikethrough = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1194) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8e161d09f7020653"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Stroopwafel = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1195) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c9772a980a7ce4db"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Subscript = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1196) | Inherited or compiler-provided contract. | — | — |
| <a id="api-25c3f32f35a9a90b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Suitcase = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1197) | Inherited or compiler-provided contract. | — | — |
| <a id="api-446cbd76c82752e8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SuitcaseMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1198) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bed83f9edfed2b93"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SuitcaseRolling = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1199) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6328b4bf62bb4925"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Sun = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1200) | Inherited or compiler-provided contract. | — | — |
| <a id="api-342e29b589fe1aca"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.SunPlantWilt = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1201) | Inherited or compiler-provided contract. | — | — |
| <a id="api-77483c3df304baf9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Superscript = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1202) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c16830f8c59d1b2f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Swatchbook = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1203) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7c46fe3298c56ea8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Synagogue = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1204) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8a5f262872d6a11f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Syringe = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1205) | Inherited or compiler-provided contract. | — | — |
| <a id="api-20d7fd8a8ec8616b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.T = "T"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1206) | Inherited or compiler-provided contract. | — | — |
| <a id="api-84dc5c45ca6dd992"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Table = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1207) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dd4b725e2400dd64"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableCells = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1208) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f78beb975548deb9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableCellsColumnLock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1209) | Inherited or compiler-provided contract. | — | — |
| <a id="api-db111288dcd77808"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableCellsLarge = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1210) | Inherited or compiler-provided contract. | — | — |
| <a id="api-dd894ef4b3b242a9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableCellsRowLock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1211) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3f57e316cc346342"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableCellsRowUnlock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1212) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a3ab8fda24431fb0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableColumns = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1213) | Inherited or compiler-provided contract. | — | — |
| <a id="api-293bf6f8e8ac452e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableList = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1214) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4b10ea24f74680a7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TableTennisPaddleBall = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1215) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ede4d3e99061f1c2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tablet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1216) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c9cb1a9c19f38fb9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TabletButton = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1217) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1390832a74d10fdc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TabletScreenButton = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1218) | Inherited or compiler-provided contract. | — | — |
| <a id="api-eff5fdaa01ec9e65"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tablets = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1219) | Inherited or compiler-provided contract. | — | — |
| <a id="api-31035bea21ed25ee"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TachographDigital = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1220) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bbf1b953fefd06eb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tag = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1221) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8d0ee4a27872e117"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tags = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1222) | Inherited or compiler-provided contract. | — | — |
| <a id="api-eb907910eff6fce2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tape = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1223) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ec427e59bcb05048"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tarp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1224) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bb6c38c36fc21ad8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TarpDroplet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1225) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1c1abad2fc52cc45"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Taxi = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1226) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0aa1c289b1d0e19c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Teeth = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1227) | Inherited or compiler-provided contract. | — | — |
| <a id="api-42a1fd427f2f0284"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TeethOpen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1228) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6cebe2a54732dde6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureArrowDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1229) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bff852e38ba95497"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureArrowUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1230) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0524999f385113a6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureEmpty = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1231) | Inherited or compiler-provided contract. | — | — |
| <a id="api-aa9d2b239b754419"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureFull = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1232) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a2ff7dfae07696b2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureHalf = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1233) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0cb3e66e75a97790"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureHigh = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1234) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4ec52b347d03634e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureLow = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1235) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1a9e5e09fceec003"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureQuarter = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1236) | Inherited or compiler-provided contract. | — | — |
| <a id="api-964c4e69b7143f83"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TemperatureThreeQuarters = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1237) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cb6e125dd6afab02"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TengeSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1238) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d37fcfe5f706b27b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tent = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1239) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8fd80e802893fe7b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TentArrowDownToLine = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1240) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8f6ae1d67bc18a79"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TentArrowLeftRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1241) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1bbc0c9cddb4e271"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TentArrowTurnLeft = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1242) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d08a6789985e67b1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TentArrowsDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1243) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ecefb121dd94f1d4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tents = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1244) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7a593d1f6da017df"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Terminal = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1245) | Inherited or compiler-provided contract. | — | — |
| <a id="api-de4023820eb722d7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TextHeight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1246) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f86d8dccf80fd45c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TextSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1247) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9605feb7990425d7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TextWidth = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1248) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9356e59dee1d8108"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Thermometer = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1249) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fe9bbcaa0f3463f2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ThumbsDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1250) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4c54de58bd9bddd9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ThumbsUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1251) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5089d3b2a2cc06e0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Thumbtack = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1252) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d051b3f79f28b6ac"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ThumbtackSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1253) | Inherited or compiler-provided contract. | — | — |
| <a id="api-794a8bf486f26a41"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Ticket = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1254) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7b7ce23799031d43"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TicketSimple = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1255) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1035116391617e07"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Timeline = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1256) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f4faae17664f6ecc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToggleOff = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1257) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b38301656f2182b9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToggleOn = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1258) | Inherited or compiler-provided contract. | — | — |
| <a id="api-0235c2c2c5181711"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Toilet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1259) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e6a0fe9f9316e5dc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToiletPaper = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1260) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4a6b76ac0680a9d8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToiletPaperSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1261) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e90f95ae0261549b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToiletPortable = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1262) | Inherited or compiler-provided contract. | — | — |
| <a id="api-af0940d3972005a7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToiletsPortable = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1263) | Inherited or compiler-provided contract. | — | — |
| <a id="api-99ec210156d78c30"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Toolbox = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1264) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d6202a6c3da7f018"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tooth = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1265) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f5a2bfb21a41d043"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.ToriiGate = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1266) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c4fc7b9e67d51849"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tornado = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1267) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2e49c7de7d1c8329"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TowerBroadcast = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1268) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f40693f4158e187d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TowerCell = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1269) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9ed32c2b633e29bc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TowerObservation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1270) | Inherited or compiler-provided contract. | — | — |
| <a id="api-90cb0a68cc172175"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tractor = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1271) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bdc29255d86c14ec"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Trademark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1272) | Inherited or compiler-provided contract. | — | — |
| <a id="api-56b1adfea35f7736"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrafficLight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1273) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a059e3c347dd631a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Trailer = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1274) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f93d187f866e2558"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Train = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1275) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ea9f8425c8113fe1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrainSubway = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1276) | Inherited or compiler-provided contract. | — | — |
| <a id="api-163dc80df455da96"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrainTram = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1277) | Inherited or compiler-provided contract. | — | — |
| <a id="api-16f2149023e7e13d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Transgender = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1278) | Inherited or compiler-provided contract. | — | — |
| <a id="api-36712b96e52186c0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Trash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1279) | Inherited or compiler-provided contract. | — | — |
| <a id="api-397c7296cfe0f3ca"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrashArrowUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1280) | Inherited or compiler-provided contract. | — | — |
| <a id="api-63949684b000321f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrashCan = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1281) | Inherited or compiler-provided contract. | — | — |
| <a id="api-491bbe83d3e64e48"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrashCanArrowUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1282) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4e35c802261b7b35"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tree = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1283) | Inherited or compiler-provided contract. | — | — |
| <a id="api-82b5d4c65597372b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TreeCity = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1284) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4c877bfb4e4fca29"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TriangleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1285) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f02830cf24307fa6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Trophy = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1286) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1e7412d1a1476a66"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Trowel = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1287) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9a1643a6a764f895"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TrowelBricks = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1288) | Inherited or compiler-provided contract. | — | — |
| <a id="api-75befb8677880706"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Truck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1289) | Inherited or compiler-provided contract. | — | — |
| <a id="api-de940ca197d8d657"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckArrowRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1290) | Inherited or compiler-provided contract. | — | — |
| <a id="api-22fa02c86a7f9b6e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckDroplet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1291) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3ad1a46acc4995b9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckFast = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1292) | Inherited or compiler-provided contract. | — | — |
| <a id="api-69debe045ee182c4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckField = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1293) | Inherited or compiler-provided contract. | — | — |
| <a id="api-af5af78cda8633ae"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckFieldUn = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1294) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3555083892b43604"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckFront = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1295) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2deb41e9b9c7bc42"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckMedical = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1296) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3e9aa7556f565869"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckMonster = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1297) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8d95ca82261f8eb9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckMoving = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1298) | Inherited or compiler-provided contract. | — | — |
| <a id="api-41401f3a0f62a3a1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckPickup = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1299) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5f52385d8b0a9441"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckPlane = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1300) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a326c95a4b6385f8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TruckRampBox = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1301) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c826ec6babd9f343"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tty = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1302) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ba9bf6d90e2c67ca"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TurkishLiraSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1303) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c8f8f1ac1f0e0100"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TurnDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1304) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b66d88375dabe375"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.TurnUp = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1305) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9da06d1327263f15"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Tv = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1306) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6b567ad788aef2af"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.U = "U"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1307) | Inherited or compiler-provided contract. | — | — |
| <a id="api-807e7ba5c70d9702"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Umbrella = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1308) | Inherited or compiler-provided contract. | — | — |
| <a id="api-56b97283eb88161f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UmbrellaBeach = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1309) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e642ea8ce0620d55"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Underline = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1310) | Inherited or compiler-provided contract. | — | — |
| <a id="api-646b638664320d01"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UniversalAccess = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1311) | Inherited or compiler-provided contract. | — | — |
| <a id="api-353e66e4702b59a0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Unlock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1312) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9e224611a55915c0"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UnlockKeyhole = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1313) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2db6832e8a9dd8f5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UpDown = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1314) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9eeab463970b65b1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UpDownLeftRight = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1315) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d2c4798955c5dd06"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UpLong = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1316) | Inherited or compiler-provided contract. | — | — |
| <a id="api-79f57586e8053de2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UpRightAndDownLeftFromCenter = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1317) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cb776f94dd92430a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UpRightFromSquare = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1318) | Inherited or compiler-provided contract. | — | — |
| <a id="api-84c9dd3d79bf2bb8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Upload = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1319) | Inherited or compiler-provided contract. | — | — |
| <a id="api-45d878fc3ccddd3e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.User = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1320) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8d26bbef4a0264b5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserAstronaut = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1321) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7dfbd315448831ac"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1322) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3aea3ec1c9ab65e5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserClock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1323) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d1229ede0f8907a3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserDoctor = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1324) | Inherited or compiler-provided contract. | — | — |
| <a id="api-058eb91fee29fb1d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserGear = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1325) | Inherited or compiler-provided contract. | — | — |
| <a id="api-82eceada51f9e70e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserGraduate = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1326) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fa35b12cfb9ac285"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserGroup = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1327) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e05e614ab19d04a9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserInjured = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1328) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bc78b0e02831715f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserLarge = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1329) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ff97125b4aca86db"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserLargeSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1330) | Inherited or compiler-provided contract. | — | — |
| <a id="api-b105938cfc8fc072"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserLock = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1331) | Inherited or compiler-provided contract. | — | — |
| <a id="api-29cfaaf826478f23"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserMinus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1332) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2efcefbe69d8fa9e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserNinja = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1333) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7edb2929862fa979"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserNurse = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1334) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d5849f211ba8702e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserPen = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1335) | Inherited or compiler-provided contract. | — | — |
| <a id="api-565d388d8b05d222"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserPlus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1336) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4118f33fa73831b3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserSecret = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1337) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ff82f471c178bb01"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserShield = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1338) | Inherited or compiler-provided contract. | — | — |
| <a id="api-99f4d777abcbbec1"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1339) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6b92e04cc7606cbf"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserTag = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1340) | Inherited or compiler-provided contract. | — | — |
| <a id="api-58e101decca4e6e8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserTie = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1341) | Inherited or compiler-provided contract. | — | — |
| <a id="api-20c3ae52255d2841"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UserXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1342) | Inherited or compiler-provided contract. | — | — |
| <a id="api-029e80d2ddf3515e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Users = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1343) | Inherited or compiler-provided contract. | — | — |
| <a id="api-988d6e8fb57532cd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersBetweenLines = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1344) | Inherited or compiler-provided contract. | — | — |
| <a id="api-12eb3d669d574ee4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersGear = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1345) | Inherited or compiler-provided contract. | — | — |
| <a id="api-572dc0c0e1dc1f04"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersLine = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1346) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c911c5811bc63ca5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersRays = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1347) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2c302f987b7eb484"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersRectangle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1348) | Inherited or compiler-provided contract. | — | — |
| <a id="api-25692c4f74148383"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1349) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4e6a93c683cd5f69"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.UsersViewfinder = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1350) | Inherited or compiler-provided contract. | — | — |
| <a id="api-664eacd76f6bd42e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Utensils = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1351) | Inherited or compiler-provided contract. | — | — |
| <a id="api-66b3f876b14d5fee"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.V = "V"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1352) | Inherited or compiler-provided contract. | — | — |
| <a id="api-cdac42a62128ce03"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VanShuttle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1353) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ae7eb39e35bad0bb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Vault = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1354) | Inherited or compiler-provided contract. | — | — |
| <a id="api-51b68823b21c1549"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VectorSquare = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1355) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7cc6be86af21c9a5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Venus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1356) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5cc2c7ddd12d84f4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VenusDouble = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1357) | Inherited or compiler-provided contract. | — | — |
| <a id="api-38f98ffe96df2bbb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VenusMars = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1358) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f1662604b1b736a7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Vest = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1359) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e8f13c8c617e6e80"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VestPatches = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1360) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f4869097aa1c54fd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Vial = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1361) | Inherited or compiler-provided contract. | — | — |
| <a id="api-8b81b5dd85492177"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VialCircleCheck = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1362) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6e2a13487aca6b54"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VialVirus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1363) | Inherited or compiler-provided contract. | — | — |
| <a id="api-76191662578b446e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Vials = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1364) | Inherited or compiler-provided contract. | — | — |
| <a id="api-4c6f89ccb64b49bb"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Video = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1365) | Inherited or compiler-provided contract. | — | — |
| <a id="api-78fb0d35057860aa"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VideoSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1366) | Inherited or compiler-provided contract. | — | — |
| <a id="api-abb7880421be6970"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Vihara = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1367) | Inherited or compiler-provided contract. | — | — |
| <a id="api-92a595a4ce36d7c9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Virus = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1368) | Inherited or compiler-provided contract. | — | — |
| <a id="api-31f045f55befd409"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VirusCovid = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1369) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2bee7975b56702af"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VirusCovidSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1370) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9190b8a685f715f8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VirusSlash = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1371) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6afde9c59935be05"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Viruses = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1372) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ade2f56951c23444"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Voicemail = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1373) | Inherited or compiler-provided contract. | — | — |
| <a id="api-feaa5c61b2c06446"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Volcano = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1374) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a5bee93a92b791b8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Volleyball = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1375) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e66653f741b83b1e"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VolumeHigh = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1376) | Inherited or compiler-provided contract. | — | — |
| <a id="api-72f726626bfaf362"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VolumeLow = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1377) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fa34ca92a3857f3d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VolumeOff = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1378) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a8b94835887611f9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VolumeXmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1379) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fa7219fbe1da1cbf"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.VrCardboard = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1380) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7db0c6622dcfd0b7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.W = "W"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1381) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c740d7abc3a9a63a"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WalkieTalkie = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1382) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bc408be3b010fc3c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Wallet = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1383) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c3bad90fc3cedb82"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WandMagic = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1384) | Inherited or compiler-provided contract. | — | — |
| <a id="api-90253606efa638fd"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WandMagicSparkles = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1385) | Inherited or compiler-provided contract. | — | — |
| <a id="api-847bf6298527e21b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WandSparkles = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1386) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9cc3eb655b843a4f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Warehouse = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1387) | Inherited or compiler-provided contract. | — | — |
| <a id="api-777a863b82ad1fac"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Water = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1388) | Inherited or compiler-provided contract. | — | — |
| <a id="api-91fc67f1f0833f2d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WaterLadder = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1389) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5eed7986f0bf8d45"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WaveSquare = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1390) | Inherited or compiler-provided contract. | — | — |
| <a id="api-402d2e3faac08e16"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WebAwesome = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1391) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bbadd2de29f362b8"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WeightHanging = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1392) | Inherited or compiler-provided contract. | — | — |
| <a id="api-9b636b4b9e3c26dc"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WeightScale = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1393) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7c6543f5b1f9a4e6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WheatAwn = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1394) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f6b6a735cbb379d6"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WheatAwnCircleExclamation = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1395) | Inherited or compiler-provided contract. | — | — |
| <a id="api-31fe674322bc9227"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Wheelchair = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1396) | Inherited or compiler-provided contract. | — | — |
| <a id="api-475700a6bbb6eae3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WheelchairMove = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1397) | Inherited or compiler-provided contract. | — | — |
| <a id="api-fa5fc4d6c9499951"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WhiskeyGlass = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1398) | Inherited or compiler-provided contract. | — | — |
| <a id="api-3fb8ff9867686b8b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Wifi = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1399) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a3f9e77eecf8e562"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Wind = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1400) | Inherited or compiler-provided contract. | — | — |
| <a id="api-76ce8984f09ed689"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WindowMaximize = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1401) | Inherited or compiler-provided contract. | — | — |
| <a id="api-be397baa901ded2b"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WindowMinimize = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1402) | Inherited or compiler-provided contract. | — | — |
| <a id="api-af57e00beff046d3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WindowRestore = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1403) | Inherited or compiler-provided contract. | — | — |
| <a id="api-6f2028668f82ff72"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WineBottle = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1404) | Inherited or compiler-provided contract. | — | — |
| <a id="api-e320fb5457082c88"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WineGlass = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1405) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1acb30dbd8ca0228"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WineGlassEmpty = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1406) | Inherited or compiler-provided contract. | — | — |
| <a id="api-eb11c6812310fcb9"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.WonSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1407) | Inherited or compiler-provided contract. | — | — |
| <a id="api-1363c61a9883ed5c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Worm = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1408) | Inherited or compiler-provided contract. | — | — |
| <a id="api-bee82c3aaa20c7e4"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Wrench = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1409) | Inherited or compiler-provided contract. | — | — |
| <a id="api-5038a863623636d7"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.X = "X"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1410) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2b0c3e834ad9f36d"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.XRay = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1411) | Inherited or compiler-provided contract. | — | — |
| <a id="api-45d4048b64a8a47f"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Xmark = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1412) | Inherited or compiler-provided contract. | — | — |
| <a id="api-c3368aad87248bc2"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.XmarksLines = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1413) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d866d075180acaf3"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Y = "Y"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1414) | Inherited or compiler-provided contract. | — | — |
| <a id="api-39c58fe66e7c6e7c"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.YenSign = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1415) | Inherited or compiler-provided contract. | — | — |
| <a id="api-77212a01b5ce8699"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.YinYang = ""`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1416) | Inherited or compiler-provided contract. | — | — |
| <a id="api-48d68c78378c27e5"></a>[`public const string Inno.Adapter.Presentation.ImGui.ImGuiIcon.Z = "Z"`](../../../backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/ImGuiIcon.cs#L1417) | Inherited or compiler-provided contract. | — | — |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Core.Execution` | 实现依赖；不传递 compile surface |
| `Inno.Native.Sdl3` | 公开签名依赖 |
| `Inno.Native.ImGui` | 公开签名依赖 |
| `Inno.Adapter.Platform` | 公开签名依赖 |
| `Inno.Adapter.Presentation` | 公开签名依赖 |
| `Inno.Platform` | 公开签名依赖 |
| `Inno.Adapter.Platform.Sdl3` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
