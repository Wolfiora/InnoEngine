# Inno.Core.Input

[上一页：Identity](Inno.Core.Identity.md) · [Core 索引](README.md) · [下一页：Diagnose](Inno.Core.Diagnostics.md)

Input 项目只定义跨平台输入语义枚举，不维护按键状态。状态采集由 Platform 层完成，事件传递参见 [Inno.Core.Events](Inno.Core.Events.md)。

## KeyCode

| 分组 | 值 |
| --- | --- |
| 未知 | `Unknown` |
| 字母 | `A`–`Z` |
| 主键盘数字 | `D0`–`D9` |
| 常用控制 | `Escape`, `Space`, `Enter`, `Tab`, `Backspace` |
| 方向 | `LeftArrow`, `UpArrow`, `RightArrow`, `DownArrow` |
| 修饰键实体 | `LeftSuper`, `RightSuper`, `LeftShift`, `RightShift`, `LeftCtrl`, `RightCtrl`, `LeftAlt`, `RightAlt` |
| 编辑/导航 | `CapsLock`, `Insert`, `Delete`, `Home`, `End`, `PageUp`, `PageDown` |
| 数字键盘 | `NumPad0`–`NumPad9`, `NumLock` |
| 锁定 | `ScrollLock` |
| 功能键 | `F1`–`F12` |
| 标点 | `Plus`, `Comma`, `Minus`, `Period`, `Slash`, `Tilde`, `Backslash`, `Semicolon`, `Quote`, `LeftBracket`, `RightBracket` |

枚举数值接近常见虚拟键码，但业务代码应比较枚举名，不要假定所有平台都直接提供相同整数。

## KeyModifier

`[Flags]` enum：`None`、`Alt`、`Control`、`Shift`、`Super`。

```csharp
using Inno.Core.Input;

static bool IsSaveShortcut(
    KeyCode key,
    KeyModifier modifiers
) {
    return key == KeyCode.S && (modifiers & KeyModifier.Control) != 0;
}
```

macOS Command / Windows key 等平台主修饰键可映射到 `Super`。`KeyModifier` 表示事件发生时的组合状态，与 `KeyCode.LeftCtrl` 这种具体实体键不同。

## MouseButton

`Left`、`Right`、`Middle`、`XButton1`、`XButton2`。

## MouseCursor

| 值 | 用途 |
| --- | --- |
| `None` | 隐藏/不设置光标。 |
| `Arrow` | 默认箭头。 |
| `TextInput` | 文本输入 I-beam。 |
| `ResizeAll` | 全方向移动/缩放。 |
| `ResizeNS`, `ResizeEW` | 垂直、水平 resize。 |
| `ResizeNESW`, `ResizeNWSE` | 两种对角 resize。 |
| `Hand` | 可点击链接/抓手。 |
| `NotAllowed` | 禁止操作。 |

## 事件组合

本项目定义输入值，不拥有事件派发器。订阅、派发和订阅释放见 [Core Events 输入事件示例](Inno.Core.Events.md#输入事件组合)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Input.KeyCode`

<!-- api: T:Inno.Core.Input.KeyCode 251a3164af6ca6294fe67d77a2131c26da870657157f75b556efb057a961aca5 -->
<!-- api: F:Inno.Core.Input.KeyCode.Unknown b206cdb54340239bc795a451f2162d7994bd3babbd983b486367dabf2a7742bc -->
<!-- api: F:Inno.Core.Input.KeyCode.A 2bd02cebcb678125172ace470d531ff37642cb7945dae97bbd96906d76ba7dbf -->
<!-- api: F:Inno.Core.Input.KeyCode.B 8cb5b34a037730e33ecb693c88870d9258e3af9b779f814d056d162a913f5b09 -->
<!-- api: F:Inno.Core.Input.KeyCode.C 40525129e5bb1ba695aae0cebe0f146238dd7f4c68c53a56d6eb29d76f7ffb27 -->
<!-- api: F:Inno.Core.Input.KeyCode.D a23e2d507f0c92ecdd443ab4e9bfc9cb4676fdbbf94c9a2f8452e60fc3113752 -->
<!-- api: F:Inno.Core.Input.KeyCode.E 6767ad8de3a438a28c74f9193445f6d613319538d362c2f40a0b2668229a2e16 -->
<!-- api: F:Inno.Core.Input.KeyCode.F c91bbf7814ad980034cd73fd6d9333fbab5b8348450f8eefb9f6f2c2cf07a0c2 -->
<!-- api: F:Inno.Core.Input.KeyCode.G 3993b4d87b0a933194ecee91e727ce176071211d4359dd5eb12391dce77f318b -->
<!-- api: F:Inno.Core.Input.KeyCode.H bbc7511c8a882dfe219b5ea557965e6a8511ea31daa9799432acfe34574217be -->
<!-- api: F:Inno.Core.Input.KeyCode.I 6fd6d93904895f83e0240d00c2995f1c9fbaf668c781724f65f2165186364c99 -->
<!-- api: F:Inno.Core.Input.KeyCode.J 1e57aba2c32436ced81c640b1ed15cb6e4f204d885f3709949e5d88ead86de54 -->
<!-- api: F:Inno.Core.Input.KeyCode.K f17b2847c80f11c35734f883098102d474e6d708f479a8238ca6dadb1782dce9 -->
<!-- api: F:Inno.Core.Input.KeyCode.L 065fbcb35b13a34f68d07fdf61b1e6bb044664027d37b8d28aa565b7bedede1b -->
<!-- api: F:Inno.Core.Input.KeyCode.M 4343472fd518839e4350f76bebd4e1452d24a818787c0ca24e72a02de2a0dbbd -->
<!-- api: F:Inno.Core.Input.KeyCode.N 055a55e666a09e880120e200e27d6ece2309dd5d6dbbc925957c38029918d62d -->
<!-- api: F:Inno.Core.Input.KeyCode.O e094fa686715756033f38e18571ba2877a0ae513d52b2a66f99c973b7901cd7b -->
<!-- api: F:Inno.Core.Input.KeyCode.P a577771e6ed568824f894c1f8be05fab29285b51b016c559b5d79c4094af11f1 -->
<!-- api: F:Inno.Core.Input.KeyCode.Q de3661f9ed336b9fc595e0fcf376d1882a6010aff0aea031d7f942a504bff70d -->
<!-- api: F:Inno.Core.Input.KeyCode.R 3d04ca06be5753a93b941683a513f05b6cc379a60669be0a7815a8b90b5a8be9 -->
<!-- api: F:Inno.Core.Input.KeyCode.S 1d4bf0bceb89186197a85767bea4f224a800531a0c8783546194a5e169448048 -->
<!-- api: F:Inno.Core.Input.KeyCode.T 03f976a5807242df66b7a4f5b129a5fe25d79cf66a943c7292b5a658586d8825 -->
<!-- api: F:Inno.Core.Input.KeyCode.U a688a0203ecdf11afbe96d9e53e11d829d98cf602f432f83c58e11d0097cc0ea -->
<!-- api: F:Inno.Core.Input.KeyCode.V 45fbfd771fbec49cac64ebdf16c74fde1887a28992876e37c8b782e71d9da45f -->
<!-- api: F:Inno.Core.Input.KeyCode.W 09ef816690c87ce6c3b3fcb0cf22c3eda24ad825e24eb89311133b33d7006887 -->
<!-- api: F:Inno.Core.Input.KeyCode.X b43dedf091372fa4eb2b7d487ce5ee080ec80765cef8d9199cc94fc4c69c0c6d -->
<!-- api: F:Inno.Core.Input.KeyCode.Y d916be136596d5658d49108c6ef99fd879b2de6af780920b4109abcf15ad10bd -->
<!-- api: F:Inno.Core.Input.KeyCode.Z 0db2e8be49b5462768116e0168f42b3c6fe1989d2a3fafd1618deb1d070a1cd5 -->
<!-- api: F:Inno.Core.Input.KeyCode.D0 aab1ec5fd3eba744c1df13707e558a356cca4e597ae17d80b581f1b9d18ec6b5 -->
<!-- api: F:Inno.Core.Input.KeyCode.D1 de7c0375ab865a825b0a60d8ba4e06faa7ad2591924bc6338d4a172a8e1c8650 -->
<!-- api: F:Inno.Core.Input.KeyCode.D2 634da4cb16811b576b08319e8a5d1c0918ad0fa9492733b991389440ee4e3e8f -->
<!-- api: F:Inno.Core.Input.KeyCode.D3 58bd2233af8d221e879104b65607bc3804820529978f4c5b066a9c0091a42917 -->
<!-- api: F:Inno.Core.Input.KeyCode.D4 b8d073f99929e47c13a5c06f56d713b1f02c6956370a07ac70d074594f40cb4e -->
<!-- api: F:Inno.Core.Input.KeyCode.D5 0a8c2730f205c758842b38c4a57ca028a62c3eed65494b8e4a1c6d33bf7d64e8 -->
<!-- api: F:Inno.Core.Input.KeyCode.D6 7893ee471764a6f4ac99ecef3a1d91a13aaa80d90a8dc3691231c148899bd647 -->
<!-- api: F:Inno.Core.Input.KeyCode.D7 f6d27c2fc82a75628edee55548eecb910175e9bf512327244bcddb55ef8f1ef1 -->
<!-- api: F:Inno.Core.Input.KeyCode.D8 0badc2b0b2880b701f2a49b3e718a261ec0927008065b1a335b59d546af87dfb -->
<!-- api: F:Inno.Core.Input.KeyCode.D9 4e28553a39ca4915ac379ee7f5546241b84a0ba3c198fbb72cafde463d98d70b -->
<!-- api: F:Inno.Core.Input.KeyCode.Escape 3b29e9665f90378ad12bccb4956ae4fc8addcb98769a8c16938bc08bea283abe -->
<!-- api: F:Inno.Core.Input.KeyCode.Space 8f39ae52aadc92f1863912709750cb92f48562df180d18647c1d9972a20c5499 -->
<!-- api: F:Inno.Core.Input.KeyCode.Enter 92d809c5d7664bbe49f8180a1b5982bf1b730ccf13f79583bea3cb4bc3627255 -->
<!-- api: F:Inno.Core.Input.KeyCode.Tab 561106f99aba9c92212a00857bbbe095ca1f484fff733ebfa917f0d9828f0ce9 -->
<!-- api: F:Inno.Core.Input.KeyCode.Backspace 976636da12c62c0659ba3bfe0b4f2dd82f2a1ed853cdfc2390b407b5a817a8e2 -->
<!-- api: F:Inno.Core.Input.KeyCode.LeftArrow 5f8cda4659f790bfeb4f5f3b432cbf2f235943e1c9c9b1c9a0d5e21c6710320c -->
<!-- api: F:Inno.Core.Input.KeyCode.UpArrow ad0ff3f65854f59d06bd6870574716511dd86a80e4df8237b75f2e61e50cd700 -->
<!-- api: F:Inno.Core.Input.KeyCode.RightArrow 5cf533c89627cb25a733935868bfc76726e92c79fe8a4529801434bd907a8a2e -->
<!-- api: F:Inno.Core.Input.KeyCode.DownArrow b1fa12a57702f20c31672ca62c3d187e3e8ec06acbf49659209ff1601175b00e -->
<!-- api: F:Inno.Core.Input.KeyCode.LeftSuper 2a2510931ffe70390babb9d27c42cd8d0c15009785ff2cfd47abe1b934d72ee5 -->
<!-- api: F:Inno.Core.Input.KeyCode.RightSuper 253aaec6dcbcb47c8207730ad4fd8fbb5016fce62028e7ab7fff5dbdded42a15 -->
<!-- api: F:Inno.Core.Input.KeyCode.LeftShift 21836ef75b76798c4072df63c999cd4a138e379c9aa44d30110b8642ac448329 -->
<!-- api: F:Inno.Core.Input.KeyCode.RightShift c7546df26faceb1ffb1c9252e393d4f5ddfb32f7ee3624c7fa60febc0b7c10c6 -->
<!-- api: F:Inno.Core.Input.KeyCode.LeftCtrl 0e3f75bd6c5e6badf2e2a62bb68632270bbe42b6af2d9b03fca9e66e8b828946 -->
<!-- api: F:Inno.Core.Input.KeyCode.RightCtrl 7ab6af4f2de5cd3bc53750ed493fbb31637a54340b41b249664a73d740f6e01e -->
<!-- api: F:Inno.Core.Input.KeyCode.LeftAlt 2b2bf0a91816a295fbc258a9705d8cb968ead3546544cf010bc583c0b15cc35a -->
<!-- api: F:Inno.Core.Input.KeyCode.RightAlt 90cbafd9eaafa5665d1d88f127236aa478b4a428a28f0b899df867101c9bdde4 -->
<!-- api: F:Inno.Core.Input.KeyCode.CapsLock 884eb8c992a01da7edcf14d99fc22ee61eab3cdcf40680df504ed3d3e6f1d65f -->
<!-- api: F:Inno.Core.Input.KeyCode.Insert 7b8d81d574f724149c6e84e30c8c1af3fa62ba7b3c47982470a81af2a8cb6cf2 -->
<!-- api: F:Inno.Core.Input.KeyCode.Delete e4fcf91afd85c3313efff33ae432f99553b13e5d37072fbc7004a402cae7502e -->
<!-- api: F:Inno.Core.Input.KeyCode.Home d1b44c38dadef31b310e22e9138a4354bafdb7a7b3515b19991ced4d926ae3a5 -->
<!-- api: F:Inno.Core.Input.KeyCode.End 1ffe51ec08d0f2006962db5f739f5847accf37838e8cc040f346bdb20c41ee7e -->
<!-- api: F:Inno.Core.Input.KeyCode.PageUp 9751d866bb294ac4a8a375e1605c3828a73824e538099adf19ebc82488aeb537 -->
<!-- api: F:Inno.Core.Input.KeyCode.PageDown d26ca27403abb8fe2dae8cc2757c9fab9d0b811f935bc45ffe82d621a3abc092 -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad0 cc28f573f845046e50aaf3a1c811d59f1c1fc776957c229d2244a6f2436fc57b -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad1 5e15554db1aa1896662142fc3d6677a324c4f56c8dd94a30030defc2d6f602e5 -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad2 b3d0dd9fe4a7ecd8a10680e64a600d6083a4a6bfb74e0056fe2902524b770bea -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad3 79ecdc3a85833d6fba23d772359235a1d1c88cf2ae03c6cf33cfb4e6c334b480 -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad4 265e3bc34b91d17247f49b92791edddadebd1bb72edfe83881fcc272fd87cb72 -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad5 b3ce72fcd5f593720749b3b9907361b92ca4dbc5420c88e9a409a6374416ca1e -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad6 7b1ac62df91fab6c896bcd89207a6f5f2f6ffae11add6130667ee03a999c52e8 -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad7 0a826073347b1a8eb19d2c31da0a11f82d0ec5384891b9f371b8eeee26a9457a -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad8 bc15c7962c217e8284910012a01d14dc3982c6f9a605b23e18a8ec10d84c4b30 -->
<!-- api: F:Inno.Core.Input.KeyCode.NumPad9 4dc9672f11ab4442ee6ec77926c535a7a04c567da227b6f1a54307dd4207877e -->
<!-- api: F:Inno.Core.Input.KeyCode.NumLock 9d8618f86435925315fc2126913ac99b75523d8df0e9a09d655cb62193c2d5d7 -->
<!-- api: F:Inno.Core.Input.KeyCode.ScrollLock f9f1f93c214c4419a103cf693cb30960118f15cda8090f7f459f7e807998b636 -->
<!-- api: F:Inno.Core.Input.KeyCode.F1 998746c5c65601ad28ca7f987ade0041d2c6e5eb77d96f99c37c34f271b027e7 -->
<!-- api: F:Inno.Core.Input.KeyCode.F2 4855f670aa07d7de8fc48f5416ba4f1207dc0f122bf50e680d408a679b3d4989 -->
<!-- api: F:Inno.Core.Input.KeyCode.F3 ad142e08f4fe1267af925d5a447453d0f632b030f87c132f56b730a82a59b9e4 -->
<!-- api: F:Inno.Core.Input.KeyCode.F4 0e44bfdced9844d7a9103b570e56ca303fd9ddd4bc22a2e202cfbdea0a9b72e4 -->
<!-- api: F:Inno.Core.Input.KeyCode.F5 eadad76cf354c289ccd5ef12bed886b5497ab090ce425400220bd8dc52ec01e0 -->
<!-- api: F:Inno.Core.Input.KeyCode.F6 2a11b9add8f5495d44a559ad424e6da3e0097d57847054f0cb91e3a9804e8e5f -->
<!-- api: F:Inno.Core.Input.KeyCode.F7 c6ba3e8ca993c40d8831687bef02d6e1e631ae424e6ab968ad64d0ad1dc0a305 -->
<!-- api: F:Inno.Core.Input.KeyCode.F8 9e59ad53490005cd94809cf578c7d5f196e68b2ae59e16545b83a2ecd60c7d20 -->
<!-- api: F:Inno.Core.Input.KeyCode.F9 625fd7b68e7bc1e4acdde3fbd2faab6860e4a2ab2d947c2496e2d0f23fa25f75 -->
<!-- api: F:Inno.Core.Input.KeyCode.F10 9ee0cb8481da32ebfb22d71722df4d47c6881f0cbee8d7b87865ad5abd81500c -->
<!-- api: F:Inno.Core.Input.KeyCode.F11 74cba16a9c715bb7bce4023c62cac5ae6dccb0c16aa38ccb68fa7faa67351faa -->
<!-- api: F:Inno.Core.Input.KeyCode.F12 abb1d48823dd610647ed4c187ccb234f4794d88f8d12ea109981b191b98cf2fa -->
<!-- api: F:Inno.Core.Input.KeyCode.Plus 8dd1479a21649a2127260de6697785a827fb6b59111ac282f8a0d248fe511b27 -->
<!-- api: F:Inno.Core.Input.KeyCode.Comma e404284c1dbf330abf783fbfef4b0cac03244cce4d67b302c4837154cc8970e6 -->
<!-- api: F:Inno.Core.Input.KeyCode.Minus 896a0593823c849def481aa56e661e3e72f0a486e0006919edba855fecc75be0 -->
<!-- api: F:Inno.Core.Input.KeyCode.Period 89b1fb2b6ba803a5146670590496f7aa8b8d295657e547ecbfcbf2ec2f05e2af -->
<!-- api: F:Inno.Core.Input.KeyCode.Slash bebddba80b5fc2b6f84ea6702fb7660a9238fee3eebde22b2035cf89a218b4cb -->
<!-- api: F:Inno.Core.Input.KeyCode.Tilde 6c65e0fe23dfc40f3de2de136e223fc8f4ff0ec67a906a8432b75bde457a048a -->
<!-- api: F:Inno.Core.Input.KeyCode.Backslash 1485a2e0ff53a59a5218d821072aeab0beb97fad82b86f03c3efd95a0c420d86 -->
<!-- api: F:Inno.Core.Input.KeyCode.Semicolon 560bb8d7705ece187af125abc28c42b3a84eac4194e806bd2839be2e44c0ec87 -->
<!-- api: F:Inno.Core.Input.KeyCode.Quote 36837c3947233541e2c22b9df6898f069a91e7f5eb103879941059033c134730 -->
<!-- api: F:Inno.Core.Input.KeyCode.LeftBracket 2f448aec323c519ba4dd0b085b86052f1a9982ee313e34c5be39dc67fc80fece -->
<!-- api: F:Inno.Core.Input.KeyCode.RightBracket 2723105ce5be960b3ed92779e32be73dd23a852d6ce711a43f486b5c753870ba -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0f91b108c5967cec"></a>[`Inno.Core.Input.KeyCode.A = 65`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L16) | The A letter key. | — | — |
| <a id="api-f44fce77f6a29122"></a>[`Inno.Core.Input.KeyCode.B = 66`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L20) | The B letter key. | — | — |
| <a id="api-63c45c05923f9b23"></a>[`Inno.Core.Input.KeyCode.Backslash = 220`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L385) | The backslash key. | — | — |
| <a id="api-fe37d9e3468d037e"></a>[`Inno.Core.Input.KeyCode.Backspace = 8`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L178) | The backspace key. | — | — |
| <a id="api-aadb04f938667619"></a>[`Inno.Core.Input.KeyCode.C = 67`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L24) | The C letter key. | — | — |
| <a id="api-446034ed1dcae190"></a>[`Inno.Core.Input.KeyCode.CapsLock = 20`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L233) | The caps lock key. | — | — |
| <a id="api-f244085f9e3e572f"></a>[`Inno.Core.Input.KeyCode.Comma = 188`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L365) | The comma key. | — | — |
| <a id="api-f23cc8a0493cd14e"></a>[`Inno.Core.Input.KeyCode.D = 68`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L28) | The D letter key. | — | — |
| <a id="api-2a8e17f07a77aae3"></a>[`Inno.Core.Input.KeyCode.D0 = 48`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L121) | The 0 key on the primary keyboard row. | — | — |
| <a id="api-7fd0e5057c0b736f"></a>[`Inno.Core.Input.KeyCode.D1 = 49`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L125) | The 1 key on the primary keyboard row. | — | — |
| <a id="api-a0b33b979d3bc951"></a>[`Inno.Core.Input.KeyCode.D2 = 50`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L129) | The 2 key on the primary keyboard row. | — | — |
| <a id="api-1fdd3c5ce1a171bc"></a>[`Inno.Core.Input.KeyCode.D3 = 51`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L133) | The 3 key on the primary keyboard row. | — | — |
| <a id="api-85815228791672d6"></a>[`Inno.Core.Input.KeyCode.D4 = 52`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L137) | The 4 key on the primary keyboard row. | — | — |
| <a id="api-eacc84ef5c15d733"></a>[`Inno.Core.Input.KeyCode.D5 = 53`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L141) | The 5 key on the primary keyboard row. | — | — |
| <a id="api-c84abb064bec2634"></a>[`Inno.Core.Input.KeyCode.D6 = 54`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L145) | The 6 key on the primary keyboard row. | — | — |
| <a id="api-c5ce4c12e615f257"></a>[`Inno.Core.Input.KeyCode.D7 = 55`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L149) | The 7 key on the primary keyboard row. | — | — |
| <a id="api-4a3e9dbde75ddb8f"></a>[`Inno.Core.Input.KeyCode.D8 = 56`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L153) | The 8 key on the primary keyboard row. | — | — |
| <a id="api-cb0feaee97d6e771"></a>[`Inno.Core.Input.KeyCode.D9 = 57`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L157) | The 9 key on the primary keyboard row. | — | — |
| <a id="api-3b545e1ef6ec16bd"></a>[`Inno.Core.Input.KeyCode.Delete = 46`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L241) | The delete key. | — | — |
| <a id="api-2ae54add65a510f4"></a>[`Inno.Core.Input.KeyCode.DownArrow = 40`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L195) | The down arrow key. | — | — |
| <a id="api-be5c463fad7dc64e"></a>[`Inno.Core.Input.KeyCode.E = 69`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L32) | The E letter key. | — | — |
| <a id="api-6098c653b32e3af2"></a>[`Inno.Core.Input.KeyCode.End = 35`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L249) | The end key. | — | — |
| <a id="api-e1c5b86514cba43a"></a>[`Inno.Core.Input.KeyCode.Enter = 13`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L170) | The enter key. | — | — |
| <a id="api-57cbf3ccd1e47f5e"></a>[`Inno.Core.Input.KeyCode.Escape = 27`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L162) | The escape key. | — | — |
| <a id="api-3221f022528217b7"></a>[`Inno.Core.Input.KeyCode.F = 70`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L36) | The F letter key. | — | — |
| <a id="api-7e1418c96fbeaed7"></a>[`Inno.Core.Input.KeyCode.F1 = 112`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L312) | The F1 function key. | — | — |
| <a id="api-c7358a7fd5162870"></a>[`Inno.Core.Input.KeyCode.F10 = 121`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L348) | The F10 function key. | — | — |
| <a id="api-38357c037c60a6ba"></a>[`Inno.Core.Input.KeyCode.F11 = 122`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L352) | The F11 function key. | — | — |
| <a id="api-aff8a123eb4bc3d3"></a>[`Inno.Core.Input.KeyCode.F12 = 123`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L356) | The F12 function key. | — | — |
| <a id="api-7b867e78505daa0c"></a>[`Inno.Core.Input.KeyCode.F2 = 113`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L316) | The F2 function key. | — | — |
| <a id="api-47154330e9e769fe"></a>[`Inno.Core.Input.KeyCode.F3 = 114`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L320) | The F3 function key. | — | — |
| <a id="api-7cd6dcc594c0a1af"></a>[`Inno.Core.Input.KeyCode.F4 = 115`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L324) | The F4 function key. | — | — |
| <a id="api-6818b9b3e5d7b9f9"></a>[`Inno.Core.Input.KeyCode.F5 = 116`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L328) | The F5 function key. | — | — |
| <a id="api-9546595ce027e142"></a>[`Inno.Core.Input.KeyCode.F6 = 117`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L332) | The F6 function key. | — | — |
| <a id="api-ca78835243556d12"></a>[`Inno.Core.Input.KeyCode.F7 = 118`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L336) | The F7 function key. | — | — |
| <a id="api-ed5a1365bf3feec5"></a>[`Inno.Core.Input.KeyCode.F8 = 119`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L340) | The F8 function key. | — | — |
| <a id="api-c84733e60004436c"></a>[`Inno.Core.Input.KeyCode.F9 = 120`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L344) | The F9 function key. | — | — |
| <a id="api-78f252e670675881"></a>[`Inno.Core.Input.KeyCode.G = 71`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L40) | The G letter key. | — | — |
| <a id="api-8c69e7b9da043206"></a>[`Inno.Core.Input.KeyCode.H = 72`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L44) | The H letter key. | — | — |
| <a id="api-3fd63433d8a17e12"></a>[`Inno.Core.Input.KeyCode.Home = 36`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L245) | The home key. | — | — |
| <a id="api-ac0dfe73c8335ebe"></a>[`Inno.Core.Input.KeyCode.I = 73`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L48) | The I letter key. | — | — |
| <a id="api-f784d3ca69ab34d4"></a>[`Inno.Core.Input.KeyCode.Insert = 45`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L237) | The insert key. | — | — |
| <a id="api-a7976a4d591eda5e"></a>[`Inno.Core.Input.KeyCode.J = 74`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L52) | The J letter key. | — | — |
| <a id="api-3430e23da93543fb"></a>[`Inno.Core.Input.KeyCode.K = 75`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L56) | The K letter key. | — | — |
| <a id="api-e0b1072645eaf168"></a>[`Inno.Core.Input.KeyCode.L = 76`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L60) | The L letter key. | — | — |
| <a id="api-8763a75fd5717ccc"></a>[`Inno.Core.Input.KeyCode.LeftAlt = 164`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L224) | The left alt key. | — | — |
| <a id="api-009d9895f585b422"></a>[`Inno.Core.Input.KeyCode.LeftArrow = 37`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L183) | The left arrow key. | — | — |
| <a id="api-e385b7c04522786a"></a>[`Inno.Core.Input.KeyCode.LeftBracket = 219`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L397) | The left bracket key. | — | — |
| <a id="api-daaa4b7b2b9bc047"></a>[`Inno.Core.Input.KeyCode.LeftCtrl = 162`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L216) | The left ctrl key. | — | — |
| <a id="api-fd6d0e111c789e35"></a>[`Inno.Core.Input.KeyCode.LeftShift = 160`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L208) | The left shift key. | — | — |
| <a id="api-e1b1f8cdd8f437a2"></a>[`Inno.Core.Input.KeyCode.LeftSuper = 91`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L200) | The left super key. | — | — |
| <a id="api-21330a1a6cb3ca74"></a>[`Inno.Core.Input.KeyCode.M = 77`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L64) | The M letter key. | — | — |
| <a id="api-be0fdb180e5ce201"></a>[`Inno.Core.Input.KeyCode.Minus = 189`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L369) | The minus key. | — | — |
| <a id="api-c0252f0e26d8a26e"></a>[`Inno.Core.Input.KeyCode.N = 78`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L68) | The N letter key. | — | — |
| <a id="api-e9746528312296c4"></a>[`Inno.Core.Input.KeyCode.NumLock = 144`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L303) | The num lock key. | — | — |
| <a id="api-bf1b2b2a13de96e7"></a>[`Inno.Core.Input.KeyCode.NumPad0 = 96`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L262) | The 0 key on the numeric keypad. | — | — |
| <a id="api-81563fe66421b12e"></a>[`Inno.Core.Input.KeyCode.NumPad1 = 97`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L266) | The 1 key on the numeric keypad. | — | — |
| <a id="api-a89c58d023bb1bd5"></a>[`Inno.Core.Input.KeyCode.NumPad2 = 98`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L270) | The 2 key on the numeric keypad. | — | — |
| <a id="api-398cec1f492de096"></a>[`Inno.Core.Input.KeyCode.NumPad3 = 99`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L274) | The 3 key on the numeric keypad. | — | — |
| <a id="api-ec7ee06f20d07479"></a>[`Inno.Core.Input.KeyCode.NumPad4 = 100`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L278) | The 4 key on the numeric keypad. | — | — |
| <a id="api-4f5112527a8c3704"></a>[`Inno.Core.Input.KeyCode.NumPad5 = 101`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L282) | The 5 key on the numeric keypad. | — | — |
| <a id="api-93c8cb91e1dad7fd"></a>[`Inno.Core.Input.KeyCode.NumPad6 = 102`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L286) | The 6 key on the numeric keypad. | — | — |
| <a id="api-32268a2abea9c31d"></a>[`Inno.Core.Input.KeyCode.NumPad7 = 103`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L290) | The 7 key on the numeric keypad. | — | — |
| <a id="api-48cee7f66bba0e35"></a>[`Inno.Core.Input.KeyCode.NumPad8 = 104`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L294) | The 8 key on the numeric keypad. | — | — |
| <a id="api-acf1d1b23e596fb0"></a>[`Inno.Core.Input.KeyCode.NumPad9 = 105`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L298) | The 9 key on the numeric keypad. | — | — |
| <a id="api-73fd055e3e2bfabc"></a>[`Inno.Core.Input.KeyCode.O = 79`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L72) | The O letter key. | — | — |
| <a id="api-52abd16e75290a6d"></a>[`Inno.Core.Input.KeyCode.P = 80`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L76) | The P letter key. | — | — |
| <a id="api-28ae504ccb2e1a38"></a>[`Inno.Core.Input.KeyCode.PageDown = 34`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L257) | The page down key. | — | — |
| <a id="api-9db44d714ffcbc79"></a>[`Inno.Core.Input.KeyCode.PageUp = 33`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L253) | The page up key. | — | — |
| <a id="api-c2f396b04becbd0a"></a>[`Inno.Core.Input.KeyCode.Period = 190`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L373) | The period key. | — | — |
| <a id="api-470b209259ab747a"></a>[`Inno.Core.Input.KeyCode.Plus = 187`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L361) | The plus key. | — | — |
| <a id="api-79f47b7d9be224d3"></a>[`Inno.Core.Input.KeyCode.Q = 81`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L80) | The Q letter key. | — | — |
| <a id="api-7c8438b3f93cc945"></a>[`Inno.Core.Input.KeyCode.Quote = 222`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L393) | The quote key. | — | — |
| <a id="api-c775d83b82e35b1f"></a>[`Inno.Core.Input.KeyCode.R = 82`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L84) | The R letter key. | — | — |
| <a id="api-7bc9fde7a2f74536"></a>[`Inno.Core.Input.KeyCode.RightAlt = 165`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L228) | The right alt key. | — | — |
| <a id="api-68e4537fe75d827c"></a>[`Inno.Core.Input.KeyCode.RightArrow = 39`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L191) | The right arrow key. | — | — |
| <a id="api-0a3d75a0f545a6c4"></a>[`Inno.Core.Input.KeyCode.RightBracket = 221`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L401) | The right bracket key. | — | — |
| <a id="api-ab259d3e2cbf0de5"></a>[`Inno.Core.Input.KeyCode.RightCtrl = 163`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L220) | The right ctrl key. | — | — |
| <a id="api-5f5646fa0eefe262"></a>[`Inno.Core.Input.KeyCode.RightShift = 161`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L212) | The right shift key. | — | — |
| <a id="api-16b2209864366b0b"></a>[`Inno.Core.Input.KeyCode.RightSuper = 92`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L204) | The right super key. | — | — |
| <a id="api-160901622332250c"></a>[`Inno.Core.Input.KeyCode.S = 83`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L88) | The S letter key. | — | — |
| <a id="api-8247e19dc1162a0a"></a>[`Inno.Core.Input.KeyCode.ScrollLock = 145`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L307) | The scroll lock key. | — | — |
| <a id="api-6a6aed0262595493"></a>[`Inno.Core.Input.KeyCode.Semicolon = 186`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L389) | The semicolon key. | — | — |
| <a id="api-1442b826d5b07589"></a>[`Inno.Core.Input.KeyCode.Slash = 191`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L377) | The slash key. | — | — |
| <a id="api-9ebe24b557c80c3c"></a>[`Inno.Core.Input.KeyCode.Space = 32`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L166) | The space key. | — | — |
| <a id="api-bb636558801586d7"></a>[`Inno.Core.Input.KeyCode.T = 84`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L92) | The T letter key. | — | — |
| <a id="api-0c24ccea3cc75455"></a>[`Inno.Core.Input.KeyCode.Tab = 9`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L174) | The tab key. | — | — |
| <a id="api-abb15547f5c1706a"></a>[`Inno.Core.Input.KeyCode.Tilde = 192`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L381) | The tilde key. | — | — |
| <a id="api-81cd7a5693ddc69a"></a>[`Inno.Core.Input.KeyCode.U = 85`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L96) | The U letter key. | — | — |
| <a id="api-467e9d3075a2fe60"></a>[`Inno.Core.Input.KeyCode.Unknown = 0`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L11) | No recognized physical key. | — | — |
| <a id="api-8b1dc7c5a6784adf"></a>[`Inno.Core.Input.KeyCode.UpArrow = 38`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L187) | The up arrow key. | — | — |
| <a id="api-69cec177f1c5b460"></a>[`Inno.Core.Input.KeyCode.V = 86`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L100) | The V letter key. | — | — |
| <a id="api-4e4424f2831e19ed"></a>[`Inno.Core.Input.KeyCode.W = 87`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L104) | The W letter key. | — | — |
| <a id="api-15ddd672e6446fb9"></a>[`Inno.Core.Input.KeyCode.X = 88`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L108) | The X letter key. | — | — |
| <a id="api-b48eeaaef450bc23"></a>[`Inno.Core.Input.KeyCode.Y = 89`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L112) | The Y letter key. | — | — |
| <a id="api-2c94164985c1afce"></a>[`Inno.Core.Input.KeyCode.Z = 90`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L116) | The Z letter key. | — | — |
| <a id="api-2ce06d9d7c30c576"></a>[`public enum Inno.Core.Input.KeyCode : int`](../../src/foundation/core/Inno.Core.Input/KeyCode.cs#L6) | Represents a backend-neutral physical key used by runtime input and editor shortcut contracts. | — | — |

### `Inno.Core.Input.KeyModifier`

<!-- api: T:Inno.Core.Input.KeyModifier f2f9bcd5b6a7ec816cc359a65134696ab78ec2a086ced5dfe18b46029c710bc6 -->
<!-- api: F:Inno.Core.Input.KeyModifier.None 602e73689a1774b0ef64b1d4b13f4716a732ce34157c56e1379aac25dd93aca3 -->
<!-- api: F:Inno.Core.Input.KeyModifier.Alt 4e61123c99c49be63c65ec99da8ff183dc53af472cbd1fb5a9f837616968a5ae -->
<!-- api: F:Inno.Core.Input.KeyModifier.Control 126564b667973c20b93951f51a580adc9549572c4d217ff4460174806d5eb03f -->
<!-- api: F:Inno.Core.Input.KeyModifier.Shift 7e858c8712655b7f1fa79fb277bfd7be967e27993b9d3abf79a314dfd1991ef4 -->
<!-- api: F:Inno.Core.Input.KeyModifier.Super 145315bc89c6d4460464dd5007ff42e046d7a9ad1dfe74d3ce5bbd4682689bc1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-23000daebdc38f64"></a>[`Inno.Core.Input.KeyModifier.Alt = 1`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L18) | The alt key. | — | — |
| <a id="api-06761bbd7e661b62"></a>[`Inno.Core.Input.KeyModifier.Control = 2`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L22) | The control key. | — | — |
| <a id="api-1b88d9abe40f9761"></a>[`Inno.Core.Input.KeyModifier.None = 0`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L14) | The none key. | — | — |
| <a id="api-d638bbb4d2f75288"></a>[`Inno.Core.Input.KeyModifier.Shift = 4`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L26) | The shift key. | — | — |
| <a id="api-0727c3cb697045d7"></a>[`Inno.Core.Input.KeyModifier.Super = 8`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L30) | The super key. | — | — |
| <a id="api-f0de48a27308a705"></a>[`public enum Inno.Core.Input.KeyModifier : int`](../../src/foundation/core/Inno.Core.Input/KeyModifier.cs#L9) | Identifies the supported key modifier values for this contract. | — | — |

### `Inno.Core.Input.MouseButton`

<!-- api: T:Inno.Core.Input.MouseButton f56eea1c919709e285645c021b7906e33efb417ec40aec927d639c9468d7d963 -->
<!-- api: F:Inno.Core.Input.MouseButton.Left aab5b19af45df4328f78986ec1e82eb70b15cf67029edb8835f463921a8b2fb1 -->
<!-- api: F:Inno.Core.Input.MouseButton.Right b32f3940bfa573835b25b3eff46b3688bfbd5df1a4a10009613a9cdba6304c76 -->
<!-- api: F:Inno.Core.Input.MouseButton.Middle 9231deff09d3964534a4289a75c4a777746e87a910167736ea5846a9c7443693 -->
<!-- api: F:Inno.Core.Input.MouseButton.XButton1 19f3bcab1ba4c02ec4456a67f078305a3e49e09b38da451000c540f9b4479421 -->
<!-- api: F:Inno.Core.Input.MouseButton.XButton2 aa2fb598ec37e694bdd580eb8696a08f5fff3e3b72fb6e2b8a86f460f3fa5d62 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-845906274d27b175"></a>[`Inno.Core.Input.MouseButton.Left = 0`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L11) | The left key. | — | — |
| <a id="api-64e82c041f04039e"></a>[`Inno.Core.Input.MouseButton.Middle = 2`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L19) | The middle key. | — | — |
| <a id="api-23f3072f56781ad4"></a>[`Inno.Core.Input.MouseButton.Right = 1`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L15) | The right key. | — | — |
| <a id="api-95181e1092f8f209"></a>[`Inno.Core.Input.MouseButton.XButton1 = 3`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L23) | The xbutton1 key. | — | — |
| <a id="api-568fdba226a42cda"></a>[`Inno.Core.Input.MouseButton.XButton2 = 4`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L27) | The xbutton2 key. | — | — |
| <a id="api-01b091eb5ecbacb5"></a>[`public enum Inno.Core.Input.MouseButton : int`](../../src/foundation/core/Inno.Core.Input/MouseButton.cs#L6) | Identifies the supported mouse button values for this contract. | — | — |

### `Inno.Core.Input.MouseCursor`

<!-- api: T:Inno.Core.Input.MouseCursor 7ef61aef518f37c54894212bb399f83993ff09b35e18b0a711ae6e1d90ccded3 -->
<!-- api: F:Inno.Core.Input.MouseCursor.None 4ff9ce909399a710f17399c11dfc0f2f46754713488d9fb867a08e7bc8073b79 -->
<!-- api: F:Inno.Core.Input.MouseCursor.Arrow 8c7c34e19b5fcfaf7cac4ed38b9964abc6e0dd265ddc970912fde3759d4fd709 -->
<!-- api: F:Inno.Core.Input.MouseCursor.TextInput a358e751f78c75a6248ca730065ae0fffcf734ec879e75c827ef7a07182f2188 -->
<!-- api: F:Inno.Core.Input.MouseCursor.ResizeAll 664d9031e5c7744fa3716c98e72b533e11b8730634477ccc373d702f18b9c78b -->
<!-- api: F:Inno.Core.Input.MouseCursor.ResizeNS 6d52f4a2a225ee8820cb7a31e4b0d9351c222cf7b184c29a3fb646cc0d5e0296 -->
<!-- api: F:Inno.Core.Input.MouseCursor.ResizeEW 706a8983f7eb34b1e14d4ac0543754989e3f0a33851d31c8a71631d97fc8f327 -->
<!-- api: F:Inno.Core.Input.MouseCursor.ResizeNESW aa70462e29c40c5e057f03d07e5caac3c92101278b52fea096f768b9a9db4b93 -->
<!-- api: F:Inno.Core.Input.MouseCursor.ResizeNWSE 419f95b699ae4d885eb7c5b77da6e0adff61a63181bd6fe413d55a30c3efe3bf -->
<!-- api: F:Inno.Core.Input.MouseCursor.Hand 85d804cad009fe8fc52e7f373fa1cf0f32b7241acd7b3fb4c6cfdedcdcf926ab -->
<!-- api: F:Inno.Core.Input.MouseCursor.NotAllowed cfeddc1c64eb28d70c6d17ae648b4b8495144c57478e52fc768b5f3d470d2002 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9e78ec82726a8961"></a>[`Inno.Core.Input.MouseCursor.Arrow = 0`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L16) | The arrow key. | — | — |
| <a id="api-d6df7b594df7a93a"></a>[`Inno.Core.Input.MouseCursor.Hand = 7`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L44) | The hand key. | — | — |
| <a id="api-df346b19603d4537"></a>[`Inno.Core.Input.MouseCursor.None = -1`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L11) | The none key. | — | — |
| <a id="api-3fc82460676573c7"></a>[`Inno.Core.Input.MouseCursor.NotAllowed = 8`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L48) | The not allowed key. | — | — |
| <a id="api-64d6a9feba2a5634"></a>[`Inno.Core.Input.MouseCursor.ResizeAll = 2`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L24) | The resize all key. | — | — |
| <a id="api-8aa7035795a11187"></a>[`Inno.Core.Input.MouseCursor.ResizeEW = 4`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L32) | The resize ew key. | — | — |
| <a id="api-61d39ddf9c7a19f9"></a>[`Inno.Core.Input.MouseCursor.ResizeNESW = 5`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L36) | The resize nesw key. | — | — |
| <a id="api-1b18cb3d6f92d300"></a>[`Inno.Core.Input.MouseCursor.ResizeNS = 3`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L28) | The resize ns key. | — | — |
| <a id="api-e1623b9d534c0657"></a>[`Inno.Core.Input.MouseCursor.ResizeNWSE = 6`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L40) | The resize nwse key. | — | — |
| <a id="api-f1a56f564ae8caff"></a>[`Inno.Core.Input.MouseCursor.TextInput = 1`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L20) | The text input key. | — | — |
| <a id="api-026f0b41f8bb1887"></a>[`public enum Inno.Core.Input.MouseCursor : int`](../../src/foundation/core/Inno.Core.Input/MouseCursor.cs#L6) | Identifies the supported mouse cursor values for this contract. | — | — |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
