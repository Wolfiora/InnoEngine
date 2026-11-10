# Inno.Testing.Editor.Native

[测试与 fixture 索引](README.md) · [PlayMode 契约](Inno.Editor.PlayMode.Tests.md) · [规模与性能](Inno.Editor.Performance.Tests.md)

## 概览与设计

测试专用的原生 Editor fixture 库，唯一拥有真实 ImGui 上下文、输入注入和临时项目组合的共享实现。契约与性能程序集通过 ProjectReference 消费；不复制 Compile 源码，不引用另一个测试程序集，不增加引擎测试入口或暴露生产 internal 成员。

该项目显式声明 `IsTestProject=false`，只引用提供断言的 `xunit.assert`，不引入把工程标记为测试项目的 `xunit.core` 构建配置。它不包含独立运行的测试用例，不声明测试执行层；真正执行测试的消费项目继续声明各自的 `InnoValidationLayer` 和原生产品准备要求。

## 使用教程

创建 `NativePanelHost` 或 `NativeProjectPanelHost` 时显式传入调用测试类型的 `Assembly`，并在同一线程完成使用和释放。调用方项目必须声明实际动态加载的 panel 和序列化类型依赖。项目 host 提供工作区、资产、场景与编辑服务；简单 panel host 只准备其声明的组合。

调用顺序为：准备匹配目标的原生部署 → 创建 fixture → `GetPanel` / `CreateScene` → 普通公开操作或 `NativePanelInput` → 断言 → `Dispose`。`DrawPanel` 执行真实帧；失败的 panel quarantine 和非法 draw offsets 仍直接报错。

## 实现原理与使用契约

fixture 使用真实模块目录、反射类型/序列化 provider 和 Editor 模块构造注入。`NativeProjectPanelProbe` 通过正常模块组合取得 workspace、graph 和 scene edits，没有穿透私有成员。

所有返回服务均为借用；不能独立释放或在 fixture 释放后保留。项目 host 按原顺序退休 interaction、native UI、console、plugins、assets、settings、execution、session 与 engine，再删除自身创建的临时项目。原生上下文具有线程归属；消费程序集必须禁用并行，不重叠使用 ImGui 全局状态。此项目不负责生成绑定或准备 Native 产品。

这是测试基础设施的公开库契约，仍纳入 API 文档覆盖；位于 `tests/` 并不免除其公开签名映射。真正声明 `IsTestProject=true` 的测试程序集不把 xUnit 测试方法作为产品 API。

## API 参考

| API | 行为、参数与所有权 |
| --- | --- |
| `NativePanelHost(Assembly composition, params string[] assemblies)` | `composition` 是显式测试组合根；`assemblies` 是构建依赖中已声明的 panel 程序集名。默认禁用 vertex offsets。 |
| `NativePanelHost(Assembly composition, bool supportsVertexOffsets, params string[] assemblies)` | 显式声明 vertex-offset 能力，启用时验证真实 draw data。 |
| `NativePanelHost(Assembly composition, bool supportsVertexOffsets, EditorKeyboardPolicy keyboard, params string[] assemblies)` | 另外指定键盘约定；构造、绘制、释放必须在同一线程。 |
| `NativePanelHost.diagnostics`, `logs`, `console`, `runtime`, `ui` | 当前 host 生命周期内借用的真实诊断、日志、Console、interaction 和 UI 服务。 |
| `NativePanelHost.GetPanel(string id)` | 打开唯一注册 panel 并返回借用对象；无匹配或重复匹配抛出 `InvalidOperationException`。 |
| `NativePanelHost.Frame(Action draw, float deltaTime = 1f / 60f)` | 在真实 native frame 内执行 `draw`，使用指定秒数更新时间，结束时验证 draw data 和 clipboard 回调错误。 |
| `NativePanelHost.SetDisplayMetrics(Vector2 logicalSize, float framebufferScale, float zoom)` | 设置逻辑尺寸、物理缩放和 UI zoom，供真实缩放矩阵验收。 |
| `NativePanelHost.DrawPanel(EditorPanelExtension panel, Action? afterDraw = null, float deltaTime = 1f / 60f)` | 在有界窗口中绘制 panel，随后执行可选回调；真实 quarantine 抛出带诊断的异常。 |
| `NativePanelHost.Dispose()` | 释放自身拥有的 native/UI/服务；项目 host 借入的外层服务由项目 host 释放。 |
| `NativeProjectPanelHost(Assembly composition, bool supportsVertexOffsets = false, bool includeSettings = false, IByteDocumentStore? settingsDocument = null)` | 创建临时项目和完整 authoring 组合；可启用真实 settings 服务并替换公开 document-store 边界。 |
| `NativeProjectPanelHost.reloads`, `projectSettings`, `assets`, `serialization`, `runtime`, `workspace`, `graphs`, `edits`, `native` | fixture 所有、调用方借用的实际组合服务；不能跨 fixture 生命周期保存。 |
| `NativeProjectPanelHost.projectRoot` | fixture 独占的绝对临时项目路径，释放时删除。 |
| `NativeProjectPanelHost.CreateScene()` | 在所属 Edit session additive 创建并加载场景，返回 session 所有的 scene。 |
| `NativeProjectPanelHost.RefreshTypes()` | 对显式组合进行真实类型目录重建。 |
| `NativeProjectPanelHost.Dispose()` | 按所有权顺序释放全部项目资源并删除其临时项目。 |
| `NativePanelInput.Focus(NativePanelHost host, EditorPanelExtension panel, string windowFragment)` | 绘制并聚焦实际 active native window；找不到名称片段时抛出 `InvalidOperationException`。 |
| `NativePanelInput.Key(NativePanelHost host, EditorPanelExtension panel, ImGuiKey key, bool shift = false, bool primary = false)` | 通过真实帧发送完整按下和释放；`primary` 使用 fixture 明确的 Control 约定。 |
| `NativeProjectPanelProbe(IEditorSceneWorkspace workspace, GraphEditorModule graphs, SceneEdits edits)` | 正常 Editor 模块构造注入入口，不供产品组合使用。 |
| `NativeProjectPanelProbe.workspace`, `graphs`, `edits` | 正常模块组合得到的借用服务。 |

原生库缺失、类型发现缺失或实际服务操作失败保持原异常／断言，不返回假的可用 host。完整声明和英文 XML 见 [项目源码](../../../tests/editor/fixtures/Inno.Testing.Editor.Native/NativeProjectPanelHost.cs)。

<!-- api: T:Inno.Testing.Editor.Native.NativePanelHost 69d385dd39cf1b35dd8119c3a23de685ab5642cc574be245c3fb52b95ca4c69d -->
<!-- api: M:Inno.Testing.Editor.Native.NativePanelHost.#ctor(System.Reflection.Assembly,System.String[]) bdceaf467c8731bee8ce03785f85cf794a879b7966c174b7494db18d335ddc8e -->
<!-- api: M:Inno.Testing.Editor.Native.NativePanelHost.#ctor(System.Reflection.Assembly,System.Boolean,System.String[]) 1db04fbd72cdc7bd300819fe1fa7bb8ef00ed0a1bac08ade8d3596db161a497b -->
<!-- api: M:Inno.Testing.Editor.Native.NativePanelHost.#ctor(System.Reflection.Assembly,System.Boolean,Inno.Editor.Core.EditorKeyboardPolicy,System.String[]) 6d3dfd23fcf4682063c2eb582321dc9cc1568ee08434fe6bee1a5ba756a5ac68 -->
<!-- api: P:Inno.Testing.Editor.Native.NativePanelHost.diagnostics dcb031cf0bfe17deb5e1ecc42ab4610498a73c8305300962145308d4dd6bd3ac -->
<!-- api: P:Inno.Testing.Editor.Native.NativePanelHost.logs 25951fd97859db03ab45b589a5642db48fa1a5a109860b8a21d978e4e5bbc370 -->
<!-- api: P:Inno.Testing.Editor.Native.NativePanelHost.console eb16c3f70b36305f89a0ca8bf985832a22c92545d1c46aea091904b1d4d3bc87 -->
<!-- api: P:Inno.Testing.Editor.Native.NativePanelHost.runtime b68b519a35959061bb12b0281a34584c0abc2101e851591ce2a9aff801d7bdcf -->
<!-- api: P:Inno.Testing.Editor.Native.NativePanelHost.ui 8f408dca57b9ce962d65a17e8dd1b3c46f341648404eec8c8a7a910f01710bd8 -->
<!-- api: M:Inno.Testing.Editor.Native.NativePanelHost.GetPanel(System.String) 8356dcd903a1562d5e4630e475787598f4cf34fda9b330b05dfe9d10a007f079 -->
<!-- api: M:Inno.Testing.Editor.Native.NativePanelHost.Frame(System.Action,System.Single) a5402267199444acae91919421fba6ede3d0c024ebb8e38de1bae1fed4a59aba -->
<!-- api: M:Inno.Testing.Editor.Native.NativePanelHost.SetDisplayMetrics(System.Numerics.Vector2,System.Single,System.Single) d23536f40052a0ce47234a6f34807f61c6f58bf378c3078bd1fb962072f5ac56 -->
<!-- api: M:Inno.Testing.Editor.Native.NativePanelHost.DrawPanel(Inno.Editor.Interactions.EditorPanelExtension,System.Action,System.Single) 2ac9c9857104ee0bc6aebfaeb7b57d4c084202875940c9be9faab8c73603b010 -->
<!-- api: M:Inno.Testing.Editor.Native.NativePanelHost.Dispose a04607593f521959ce71377bdf49866752da3c59e44901dd0e5cb29ff8f9011c -->
<!-- api: T:Inno.Testing.Editor.Native.NativePanelInput c23b4b0e2d422e1eab111ffb22938c7511f8bc2938c47fc4e8a393e4fb2ce72d -->
<!-- api: M:Inno.Testing.Editor.Native.NativePanelInput.Focus(Inno.Testing.Editor.Native.NativePanelHost,Inno.Editor.Interactions.EditorPanelExtension,System.String) 5f273c89c9ff589fd9e388385e7e90c2adedec32b275900a95122803309f87a2 -->
<!-- api: M:Inno.Testing.Editor.Native.NativePanelInput.Key(Inno.Testing.Editor.Native.NativePanelHost,Inno.Editor.Interactions.EditorPanelExtension,Inno.Native.ImGui.ImGuiKey,System.Boolean,System.Boolean) 5a1e35190265bd4ccd643ce8737636de0ea3fe9d287dca3f2ff4036ee4031baa -->
<!-- api: T:Inno.Testing.Editor.Native.NativeProjectPanelHost f0287968df1db41b52e8c31642eff463c3dba114618969990e63ad1207789482 -->
<!-- api: M:Inno.Testing.Editor.Native.NativeProjectPanelHost.#ctor(System.Reflection.Assembly,System.Boolean,System.Boolean,Inno.Core.IO.IByteDocumentStore) d075e65dccdde97d11b9519706f748c81d2581c6f6c5306987a2f2375e18f1f3 -->
<!-- api: P:Inno.Testing.Editor.Native.NativeProjectPanelHost.reloads 09575358e8a3a3f749d9daff134f68314ebf5354bf37fddd009c3a623b84745c -->
<!-- api: P:Inno.Testing.Editor.Native.NativeProjectPanelHost.projectSettings 20049b791098f5bd09ab6092ddfa3eef81510e40b45a7869209652bd2e791c9a -->
<!-- api: P:Inno.Testing.Editor.Native.NativeProjectPanelHost.assets fa56d21dc3da51c2baf603858e157f719b10a87c467f27b8146c95e741199394 -->
<!-- api: P:Inno.Testing.Editor.Native.NativeProjectPanelHost.serialization 290cd010b07d3d43feb1bb663d6a8f06fd4cc4ba9642c629c5f235c212bc7e34 -->
<!-- api: P:Inno.Testing.Editor.Native.NativeProjectPanelHost.projectRoot 1839590838600586fb90e336033cca589c400a2c91a7e496757c33e0fafcdf2a -->
<!-- api: P:Inno.Testing.Editor.Native.NativeProjectPanelHost.runtime c9a3195d28fa50e3c2ccd6cc1fdeeafecd7d8008ab3b717bedc1874dc2d435d1 -->
<!-- api: P:Inno.Testing.Editor.Native.NativeProjectPanelHost.workspace 3dc7b70cc4c4465d59c0e473cfd652857f47542965708069ae85e24043a891fd -->
<!-- api: P:Inno.Testing.Editor.Native.NativeProjectPanelHost.graphs 2fdc949993b375692cc8e14f31962dfcb54172c768904f901a86829bee59f964 -->
<!-- api: P:Inno.Testing.Editor.Native.NativeProjectPanelHost.edits a98b8ce0588b332c7208cd84bdd8a29252aea12a9d10b3da9ac03bc59da9376f -->
<!-- api: P:Inno.Testing.Editor.Native.NativeProjectPanelHost.native 3c07794544e0f01879b833fb615acf48d8435274cec86abd95bcd1f965347be3 -->
<!-- api: M:Inno.Testing.Editor.Native.NativeProjectPanelHost.CreateScene 03bedeb83182bc4a3006ad6ad33fb6868d4998f00e57e7002b7b169d4e785de7 -->
<!-- api: M:Inno.Testing.Editor.Native.NativeProjectPanelHost.RefreshTypes 79e11ec0d279666899b5f82b8af1cb6f8ae3e1d4695495d85794e373e13c1103 -->
<!-- api: M:Inno.Testing.Editor.Native.NativeProjectPanelHost.Dispose 8050ec925afbb0abe1b3e44f4df35ee854e5675431aa5f14db5acc8a540361b4 -->
<!-- api: T:Inno.Testing.Editor.Native.NativeProjectPanelProbe 0545ceabceb007bbf960592d616c18515557fae784bfbaa8c059b22bc8aed11b -->
<!-- api: M:Inno.Testing.Editor.Native.NativeProjectPanelProbe.#ctor(Inno.Editor.Scene.IEditorSceneWorkspace,Inno.Editor.Graph.GraphEditorModule,Inno.Editor.Scene.SceneEdits) 1350df2a73e7ddd2f7750bbd741331de2caf62a3576860cd4ea652451c5553b0 -->
<!-- api: P:Inno.Testing.Editor.Native.NativeProjectPanelProbe.workspace d2cc67dc5e46362cfa320428cfbbaee6096ea3f10144efb8a5b59ce63e5916fc -->
<!-- api: P:Inno.Testing.Editor.Native.NativeProjectPanelProbe.graphs 4c6ead9be440bd66b9a540a95d5ffae42d6e81e67ab5175d0eee9a1e805da181 -->
<!-- api: P:Inno.Testing.Editor.Native.NativeProjectPanelProbe.edits d1bcf8a69480db4b8597db80bd66c2590c0d7b406f6dba7926686c026eaec336 -->
