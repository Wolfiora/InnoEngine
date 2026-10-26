# Inno.Editor.Settings

[Editor 索引](README.md) · [Settings 界面](Inno.Editor.Panel.Settings.md) · [Core Project Settings](../core/Inno.Core.Settings.md) · [Wiki 首页](../README.md)

`Inno.Editor.Settings` 为同一个 Settings 窗口提供两种明确的编辑协议，但不合并它们的持久化语义：

```text
Editor/...                                      Project/...
EditorSetting + EditorSettingObject             ProjectSettingEditor<TSetting>
             │                                               │
             ▼                                               ▼
Settings.Editor.inno（Inno Serialization）        Settings.Project.inno（Inno Serialization）
Editor-only、不会进入 Player                     Runtime/Plugin 可读取并进入构建
```

统一 frontend 还提供内置 `Build/...` 根。它直接编辑 [Inno.Build](../build/Inno.Build.md) 的强类型 `BuildSettings` 并写入 `Settings.Build.inno`；该文件是团队共享的 authoring/build 默认值，不进入 Player，也不是 Plugin 可贡献的 runtime Setting 协议。

## Editor Settings

公开 `definitions` 集合不暴露内部数组。Settings/ProjectSettingEditor 候选构造使用 TypeRegistry 的资源归属；冲突验证失败会补偿全部新实例，退休失败聚合并 Fault，而不是只写诊断。

Editor Settings 用于主题、图标、缩放、面板行为等只影响 Editor 的偏好。路径必须为 `Editor` 或以 `Editor/` 开头；完整路径同时是注册身份、读取地址和 Inno Serialization property key。

```csharp
using InnoEditor.ImGui;
using InnoEditor.Settings;

[EditorSettingPath("Editor/Appearance/Grid/Visible")]
public sealed class GridVisibilitySetting : EditorSetting
{
    public override EditorSettingObject defaultValue
    {
        get
        {
            var value = new EditorSettingObject();
            value.SetAsBoolean("value", true);
            return value;
        }
    }

    public override string description => "Shows the authoring grid.";

    protected override void OnDraw(EditorSettingObject setting)
    {
        bool value = setting.GetAsBoolean("value", true);
        if (ImGui.Checkbox("##visible", ref value))
            setting.SetAsBoolean("value", value);
    }
}
```

没有 override `OnDraw` 的 `EditorSetting` 是页面定义；override 后是字段定义。`EditorSettingObject` 支持受控的 Boolean、整数、浮点、String 与数组 GetAs/SetAs 方法，不允许保存 `Type`、delegate、runtime object 或 GPU 资源。

唯一读取入口仍是原始路径：

```csharp
using Inno.Editor.Settings;

static bool IsGridVisible(EditorSettings editorSettings)
{
    EditorSettingObject value = editorSettings.Get("Editor/Appearance/Grid/Visible");
    return value.GetAsBoolean("value", true);
}
```

返回对象始终隔离；只有 `EditorSettings.Apply(values, resets)` 才通过 `SerializationRegistry` 原子更新 `Settings.Editor.inno` 并写入统一 Editor History。`EditorSettings.changed` 用于刷新 Editor-only 消费者。

例如 Console 的保留策略只在 `Editor/Diagnostics/Console/Clear on Play` 注册和持久化，默认值为 `true`。Console backend 订阅 `EditorSettings.changed` 并在 Apply、Undo、Redo 后读取当前有效值；Console Panel 不再使用 `editor.ini` 或 toolbar 维护同名状态。

## Project Settings 的 Editor 表现

Project Settings 的运行时定义属于 [Inno.Core.Settings](../core/Inno.Core.Settings.md)。本项目只提供可选的 Editor Drawer 协议，使 Plugin 的强类型设置自动出现在同一个窗口的 `Project/...` 页面。

```csharp
using InnoEditor.ImGui;
using InnoEditor.Settings;
using InnoEngine.Settings;
using InnoEngine.Serialization;

[ProjectSettingDefinition("example.rendering")]
public sealed class MyRenderingSettings : ISerializable
{
    [SerializableProperty]
    public bool enabled { get; set; } = true;
}

[ProjectSettingPath("Project/MyPlugin/Rendering")]
public sealed class RenderingSettingEditor : ProjectSettingEditor<MyRenderingSettings>
{
    public override string description => "Configures the runtime rendering provider.";

    protected override void OnDraw(MyRenderingSettings setting)
    {
        bool enabled = setting.enabled;
        if (ImGui.Checkbox("##enabled", ref enabled))
            setting.enabled = enabled;
    }
}
```

Editor presentation 不再 override 一个常量 `settingId`。Catalog 从 `TSetting` 自身的 `ProjectSettingDefinitionAttribute` 读取唯一协议 ID，并在构造 presentation 时绑定；`ProjectSettingPathAttribute` 只携带 UI placement。这样运行时定义 Attribute 是身份的单一权威，多个 presentation 不会重复声明同一常量。

`Project/Identity/Project ID` 是内置的 `ProjectIdentitySettings` drawer；它只接受 portable lowercase namespace。Layer、Tag、Sorting Layer 等 Project-owned 定义不再让用户输入完整 ID，而是保存 local key 并在运行时组合为 `projectId.name`。Game/Plugin 导出身份也直接读取这个 Project ID，不在 Build Settings 或导出 modal 中维护第二份可编辑 ID。

`ProjectSettingEditor<TSetting>` 收到的是当前 generation 的隔离暂存副本。它只能修改该副本；统一 Apply 中的 Project scope 以“Host + Plugin 合成结果”为 baseline：有 Composer 的协议只写语义 delta，没有 Composer 的协议写完整 replacement。Reset Project 删除项目 contribution，随后重新使用 Host 默认值与 Plugin 默认贡献的合成结果。如果编辑结果等于 baseline，Apply 会自动移除已有 project record，不留下空 override。

同一个 `ProjectSettingId` 可以注册多个不同 placement 的 Editor 表现，只要它们的
`TSetting` 完全相同。所有表现共享同一份隔离暂存对象、dirty 状态、Reset、Apply 和
History 事务；这允许一个较大的运行时设置协议在 UI 中拆成多个完整 section，而不必
为了排版拆碎运行时数据模型。各 placement path 仍必须全局唯一。

## 公开 API

| API | 稳定语义 |
| --- | --- |
| `EditorSettingPathAttribute` | 把 Editor-only page/field 放入 `Editor/...`。 |
| `EditorSetting` | Editor-only page/field 定义，`OnDraw(EditorSettingObject)` 是绘制扩展点。 |
| `EditorSettingObject` | 使用当前 Inno Serialization 的隔离结构化值对象。 |
| `EditorSettings` | `Settings.Editor.inno` 的读取、Apply、Reset、History 与变更通知。 |
| `ProjectSettingPathAttribute` | 把强类型 Project Setting Drawer 放入 `Project/...`。 |
| `ProjectSettingEditor` | frontend 使用的非泛型定义与 placement metadata。 |
| `ProjectSettingEditor<TSetting>` | Plugin/Host 实现的强类型 `OnDraw(TSetting)` 扩展点。 |
| `ProjectSettingsEditor` | Host-owned Project contribution staging、Apply 与 History 服务；普通业务脚本读取设置时应使用 `ProjectSettingsStore`。 |

## 生命周期与约束

- Editor、Project、Build 三个域共享窗口、搜索、页面树、控件布局与一个 Apply 按钮，但仍拥有独立文档和 History entry，不伪装成跨文件原子事务。
- `Editor/...` 不参与 Plugin 默认贡献，也不进入 Player；`Project/...` 使用强类型 Setting 协议而不是 Editor property bag。
- `Build/...` 是 Host 内置编辑面，不提供 Plugin contribution；导出 modal 只复制默认值作为临时 draft。
- `SettingsDocumentStore<T>` 统一三个域的 current-format 验证、capture/restore 与原子写入；`EditorSettings`、`ProjectSettingsStore`、`BuildSettingsStore` 继续拥有不同生命周期和 History。
- Project setting 的长期身份是 `ProjectSettingId` 与 Stable Type ID，UI 路径只决定 Editor 中的位置。
- Catalog generation 先完整构建候选再原子切换；Drawer 不应订阅静态事件或长期保存传入的 setting 实例。
- 删除或移动路径时同步当前项目数据、调用方与 Wiki，不保留旧 key alias。

[上一页：Inno.Editor.Scene](Inno.Editor.Scene.md) · [下一页：Inno.Editor.Panel.Settings](Inno.Editor.Panel.Settings.md)

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Editor.Settings.EditorSetting cd9770c4bcd971730429e0b55d2a42fa9f24ee486890550370af2bcb34053ff7 -->
<!-- api: M:Inno.Editor.Settings.EditorSetting.#ctor 3947b5e5a500a458c20fd61160b10d512e78184c1f930470017dde09f5720f5f -->
<!-- api: P:Inno.Editor.Settings.EditorSetting.path b650c54b066622f2b5e2744f23938c2a7b7391eb929e912fb1db4dec80f716b9 -->
<!-- api: P:Inno.Editor.Settings.EditorSetting.pagePath 1a92dee70908b4696a8b8516bd94344531f9ba63eba3adbf5730e75ff55d4b2d -->
<!-- api: P:Inno.Editor.Settings.EditorSetting.label 45e21ed558a5061bec98a5b544d056f61fa393b7a844217a54190dbac103c5da -->
<!-- api: P:Inno.Editor.Settings.EditorSetting.order b553e0e0cf9e4a5ec9e36d1c990dd9731448cd90a2907a2b521ecba9e49e9693 -->
<!-- api: P:Inno.Editor.Settings.EditorSetting.hasValue 24a122e033ce6450e73ef9661d883eae274658b868c84be096efe9bf6ea897c4 -->
<!-- api: P:Inno.Editor.Settings.EditorSetting.defaultValue 98cf94b7b0c0242a2c293a02c2ec7acda5f9affb65b30c4c1ea14623f00cb405 -->
<!-- api: P:Inno.Editor.Settings.EditorSetting.section 73540288b1fd9dbf9b67aaa0cda961185a9b0e105f03f181644a25d292a08622 -->
<!-- api: P:Inno.Editor.Settings.EditorSetting.description 96a4ac45624f8ae454d6a3144c773a496db824f99140a780f5190d219464b469 -->
<!-- api: M:Inno.Editor.Settings.EditorSetting.Draw(Inno.Editor.Settings.EditorSettingObject) adf79b750198cdd499e86ad19cc04080a656f836dfdff4129affb89b22640d1e -->
<!-- api: M:Inno.Editor.Settings.EditorSetting.IsDefault(Inno.Editor.Settings.EditorSettingObject) a2d85cebff8f566a93a12941f14e61988f8d1c7cdf9d513e3ec51a2959c2cef2 -->
<!-- api: M:Inno.Editor.Settings.EditorSetting.OnDraw(Inno.Editor.Settings.EditorSettingObject) 0fc2b28791051188c7e188569c5526ed6cba108ad7a776584dc1cf0bad3a03f1 -->
<!-- api: T:Inno.Editor.Settings.EditorSettingObject 1a9495f2fe7461cd3f1b5e4e6bc11b8da64a376712829d04182aa2d7ffa20a38 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.#ctor 8fb4817c76ad367c696bf64464d432fd2070f9ae12fd6491b27e6631863ae7fe -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsBoolean(System.String,System.Boolean) c580ed1f2358c341b2309e3fae8ef7b3f04b2b81c511e576383abc0643f545cb -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsBoolean(System.String,System.Boolean) bf43b8cf612778da02ed62344a9806fcd9551b27341d17c1861325ba6cf7ae3f -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsInt32(System.String,System.Int32) 88f349eae82128313b1516c130b201e4cdad3466f9ead665c9e0190d540e0c1f -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsInt32(System.String,System.Int32) 813b94c60df5ae2082ba909e9692881ff222ae2d1501ce8cf6fafbe3e5d9055d -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsUInt32(System.String,System.UInt32) 95f851acbbfe368d0bfa4ddd651ccc285efe4bd1ad86cd44167d1b50bac1afdc -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsUInt32(System.String,System.UInt32) bf5ba10c90eccbdcad9190c75fba3286b81ac8ce8d55f5df0b209e774c2beb0c -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsInt64(System.String,System.Int64) 4c205c276944c0e5fc895231d1d97c3ae1b6e8c89d2369a35738911baef905c8 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsInt64(System.String,System.Int64) db4c1c77f20d62a5c6af6e16f86eb043a863e05b4025365eeadf238e16a3421e -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsUInt64(System.String,System.UInt64) e77155d07afd4a9354670d4e224d3a59354cb16657285cfa3ef2a2fa62be5958 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsUInt64(System.String,System.UInt64) 65db6c7fc9c67d0a98089157df78d51fbe47d8d1aa81f73d32d912b183038060 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsSingle(System.String,System.Single) 72b68afbb5f84010e686da21b7c78e8ed0c5bf891eddefaa09d9a33a58e27e86 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsSingle(System.String,System.Single) 13b58cdd0b9114d8194d6bb237360ebb0355f107fea7011401aa801eb3e89e63 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsDouble(System.String,System.Double) 0162957670b060cc36c204c58937d80009623506e0990b0f17767f3971d7c209 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsDouble(System.String,System.Double) 0ca50b927c4a60a496020e455fbb890450c6b9708c39ea70d489a90cf0458b13 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsString(System.String,System.String) 8f1136199f8ea218cba94784bbe414575bf410a628e723abd001179bbfcf96ca -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsString(System.String,System.String) bc2614cd3b74b0fef22dfee44546932e314bde0498d800f84691f44a7610d926 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsBooleanArray(System.String,System.Boolean[]) 9a4bdfb6dda751c8d5a028bebbab7526aec39500b25e23acf115857de95aba65 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsBooleanArray(System.String,System.Boolean[]) 9f9fb0161e90944544ce1009a1e7b4c91fbf054689e8839d27e0c2c7c23607b5 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsInt32Array(System.String,System.Int32[]) 10f077d9a3dd29821618b6812058c278a171409171de2ad20fbc22cba84ad932 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsInt32Array(System.String,System.Int32[]) 742babda52c279c508fda18a245bea728ecd18d7087b98f2e4c3feb8ee048ede -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsUInt32Array(System.String,System.UInt32[]) 48b2347f05c9481eb28ad558f5da3c7580b9143c1a0b152d619b72a972b8937d -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsUInt32Array(System.String,System.UInt32[]) 507f41940b612244515562e8cc9526be2533becfb4de9ce77f38f4de2a8ccb9a -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsSingleArray(System.String,System.Single[]) d2adc93d6168d938c501731cbfdedc4edd1d39efe0fc0fdde5f4db3528b60ff6 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsSingleArray(System.String,System.Single[]) 84624a6dae2b8d73312cc281c0f64b1127d82e917435718ea9c36acc55e9b3ef -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsDoubleArray(System.String,System.Double[]) ee4535b5603130f8e62f0a71920339f7d5a47badd25dfcb7226044710b987869 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsDoubleArray(System.String,System.Double[]) ba7e4e9de2ae8a357082f635dacfa0736db5e5bf4bf72c80486e38015ec68f14 -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.GetAsStringArray(System.String,System.String[]) c9db51dd102024def0fef1eb9436d94aef1a3e14886af7cb6ec991cea059ba7d -->
<!-- api: M:Inno.Editor.Settings.EditorSettingObject.SetAsStringArray(System.String,System.String[]) 855b8d4193cd786b76ab97f65964737587d8eaed193945f6f8a9e1086183a9e9 -->
<!-- api: T:Inno.Editor.Settings.EditorSettingPathAttribute f01838160201844be96fc6c6380d9716947427299fd8de692dc07920e752ab8b -->
<!-- api: M:Inno.Editor.Settings.EditorSettingPathAttribute.#ctor(System.String,System.Int32) fb69c26ed33800b8a9923fa895ae3b895cb9dafbeaf4bf8b3530ae56bdb4c14b -->
<!-- api: P:Inno.Editor.Settings.EditorSettingPathAttribute.path fc71c2f5b226e858b50144e355463f93a489e8935358bfd54d9b9fd028ff08c1 -->
<!-- api: P:Inno.Editor.Settings.EditorSettingPathAttribute.order c7267dc658290f6b31c8afe5c3a0475e0e22b4ec7d6a0a26e6bf12d9a14f1867 -->
<!-- api: T:Inno.Editor.Settings.ProjectSettingEditor e6035c01966848d035ffbf2ca0b0e55a3ec394571fa842e4a2de915434c8fae8 -->
<!-- api: P:Inno.Editor.Settings.ProjectSettingEditor.settingId 82b2cbb88f8ef907064b02701e84131df12b27d30ed7d82c6f5f71a68a438ccd -->
<!-- api: P:Inno.Editor.Settings.ProjectSettingEditor.path cda3eb3964dadfe13f1407043239a52359fa004b78f186caec721559248fef6a -->
<!-- api: P:Inno.Editor.Settings.ProjectSettingEditor.pagePath d74028bc9cebeed9391eb9018ca90e47a7e099f7963613a2f9829a5055914d18 -->
<!-- api: P:Inno.Editor.Settings.ProjectSettingEditor.label 056738687c2cd3075dd34bbee1ab990a96e559fa49041facb5859784f39280c0 -->
<!-- api: P:Inno.Editor.Settings.ProjectSettingEditor.order ada7a421cfc3edfc80946b60f6a13de77f90c31456a02c33c37c5aa7fb6ec95f -->
<!-- api: P:Inno.Editor.Settings.ProjectSettingEditor.section e05b27aab12f9cec4f051d322a7a5634dfbb0165f8cfc24a786231c505453068 -->
<!-- api: P:Inno.Editor.Settings.ProjectSettingEditor.description 849ed897d68fbfdccfae3ee4961c9278936ddebf9f7cb5013becff2f12c27bbc -->
<!-- api: M:Inno.Editor.Settings.ProjectSettingEditor.Draw(Inno.Core.Serialization.ISerializable) 3d9eb44bf90db054cdeb745283dd507d2914061ebd679eb2211609ed94142594 -->
<!-- api: M:Inno.Editor.Settings.ProjectSettingEditor.ValuesEqual(Inno.Core.Serialization.ISerializable,Inno.Core.Serialization.ISerializable) 14e2b1f88572e0db6855d84bc3798d52797f4f654aeaeb5c62a8f58134012cbe -->
<!-- api: M:Inno.Editor.Settings.ProjectSettingEditor.#ctor 4cda19a183024ccfa6c9b63b3928061242dbd7d04678cc31c246554bb1d269fc -->
<!-- api: T:Inno.Editor.Settings.ProjectSettingEditor`1 646e66df271a132a4545090b74b897ea29e75796d16fde701e487527460ec92b -->
<!-- api: M:Inno.Editor.Settings.ProjectSettingEditor`1.OnDraw(`0) 63238e399d7395210e1bcfd6e1312a60ada4b0255bbdba11bb2002105890a9f9 -->
<!-- api: M:Inno.Editor.Settings.ProjectSettingEditor`1.#ctor 1238d9bead464ed8a9bcd1d9162e7d2e8e3072502db98d77e66568c99181ae30 -->
<!-- api: T:Inno.Editor.Settings.ProjectSettingPathAttribute 1207a742ee59ad63ef8a1572108a5f242ccdb8fa302cf1c494f3166f5f202dec -->
<!-- api: M:Inno.Editor.Settings.ProjectSettingPathAttribute.#ctor(System.String,System.Int32) 245793ec3d939dc8f034bdf7fb35d48adf42ece030ad9c8bab43b6fb4ec8796e -->
<!-- api: P:Inno.Editor.Settings.ProjectSettingPathAttribute.path 499a5f42c1b3a46b52455c77956fbb5e76becbd8ceae5a5da3d5a3a93b4983da -->
<!-- api: P:Inno.Editor.Settings.ProjectSettingPathAttribute.order 9d029d3556f488e2f63538b966fd784dcecfb1ee5d5d3d56e91ee85fa3f0fa00 -->
<!-- api: T:Inno.Editor.Settings.EditorSettings c18d3aa06b5ab5b9ee8dceaea94c2595cf1550a990d4a909ccba73195ed7a915 -->
<!-- api: M:Inno.Editor.Settings.EditorSettings.ListenChanges(System.Action{Inno.Editor.Settings.EditorSettings}) 9a4efae314fb7827dff6f507f0230f7484493f8cd6bb65176e968c2dd33b423e -->
<!-- api: P:Inno.Editor.Settings.EditorSettings.catalogRevision 16b5073677455f54a3ae28fe3b9750fefdf7dbba6e5f96597b163e215af2c657 -->
<!-- api: P:Inno.Editor.Settings.EditorSettings.definitions c0ef5bab917c6ed31f28dde1cccfee57c30023d3816c09ab7c0ff2f47c1efd46 -->
<!-- api: M:Inno.Editor.Settings.EditorSettings.Get(System.String) 6b7e6c36fa37ff36f09ff7313035c42221f6d5ad385d4fae84bc19629ebf2a7e -->
<!-- api: M:Inno.Editor.Settings.EditorSettings.Apply(System.Collections.Generic.IReadOnlyDictionary{System.String,Inno.Editor.Settings.EditorSettingObject},System.Collections.Generic.IReadOnlySet{System.String}) ca3966f1d26f6d9073fcb433170359aed37e62df32293ca1b2a36c02e7f345af -->
<!-- api: M:Inno.Editor.Settings.EditorSettings.OnDispose 4a880b4fe8cbcff40b8c4e50ac8b96b0cf84d4c27f2e431576240e888896fa4f -->
<!-- api: T:Inno.Editor.Settings.ProjectSettingsEditor 5561d778856259256d8ddee3ef66ab46be1aad0f81823517d1182bb7602f87ae -->
<!-- api: P:Inno.Editor.Settings.ProjectSettingsEditor.catalogRevision e7ff202d18f540ab5ca742d372ad7c3078262e8174a72063d2952442d2587cac -->
<!-- api: P:Inno.Editor.Settings.ProjectSettingsEditor.definitions 0ea263c513bbb7e748800cca0233dafd404afef0aefaf237b147c5c08cc99273 -->
<!-- api: M:Inno.Editor.Settings.ProjectSettingsEditor.Get(Inno.Editor.Settings.ProjectSettingEditor) 9918105a13dee3ee20fc226cc690d2924866d93dac7a2cef57fe65c19ebae50f -->
<!-- api: M:Inno.Editor.Settings.ProjectSettingsEditor.GetComposedDefault(Inno.Editor.Settings.ProjectSettingEditor) ed7b720c5bc83075213def8bade6a1fbca676188ba427f715567d38b21363e86 -->
<!-- api: M:Inno.Editor.Settings.ProjectSettingsEditor.Apply(System.Collections.Generic.IReadOnlyDictionary{Inno.Core.Settings.ProjectSettingId,Inno.Core.Serialization.ISerializable},System.Collections.Generic.IReadOnlySet{Inno.Core.Settings.ProjectSettingId}) 5e0b024a8ba9a474d7bd2f37050a4a27048e3259b7a889bb364476b6989d27ca -->
<!-- api: M:Inno.Editor.Settings.ProjectSettingsEditor.OnDispose bdf448a3e122ba2abe9407bb0969668a336c209c6fc2f7c82bafd5734c8f6371 -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-be5332dcc539a1e8"></a>[`public abstract class Inno.Editor.Settings.EditorSetting`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L10) | Defines one path-addressed Settings page or one custom-drawn Settings field. | — | — |
| <a id="api-6eda555c28359e20"></a>[`protected Inno.Editor.Settings.EditorSetting.EditorSetting()`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L23) | Creates a Settings definition. A field supplies its persisted default through ; a page keeps the base implementation. | — | — |
| <a id="api-3ad06d4c684853bc"></a>[`public string Inno.Editor.Settings.EditorSetting.path { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L30) | Gets the complete slash-delimited identity and placement path. | — | — |
| <a id="api-bbd9f815aac36800"></a>[`public string Inno.Editor.Settings.EditorSetting.pagePath { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L35) | Gets the page that owns this field, or this page's own path for a page definition. | — | — |
| <a id="api-9a86d7142dac3276"></a>[`public string Inno.Editor.Settings.EditorSetting.label { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L40) | Gets the display label derived from the final path segment. | — | — |
| <a id="api-15b0bc3c501b685c"></a>[`public int Inno.Editor.Settings.EditorSetting.order { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L45) | Gets the stable order among fields with the same section and label. | — | — |
| <a id="api-ae70a8f61b3069e4"></a>[`public bool Inno.Editor.Settings.EditorSetting.hasValue { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L50) | Gets whether this definition owns one persisted JSON object. | — | — |
| <a id="api-538db028c21159a0"></a>[`public virtual Inno.Editor.Settings.EditorSettingObject Inno.Editor.Settings.EditorSetting.defaultValue { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L59) | Creates the default object for a field. Page definitions keep the base implementation, whose internal value is . | — | — |
| <a id="api-1aa95b2c27b7a550"></a>[`public virtual string Inno.Editor.Settings.EditorSetting.section { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L65) | Gets the section heading used to group fields alphabetically. Definitions without a section keep the base implementation, whose internal value is . | — | — |
| <a id="api-caf7986f7b8f4bc4"></a>[`public virtual string Inno.Editor.Settings.EditorSetting.description { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L70) | Gets the page or field explanation displayed by the Settings frontend. | — | — |
| <a id="api-f1a559641684c45e"></a>[`public bool Inno.Editor.Settings.EditorSetting.Draw(Inno.Editor.Settings.EditorSettingObject setting)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L85) | Draws this field inside the frontend-managed content container.<br>`setting`: The isolated mutable object being edited. | when the staged object differs from the value it held when this frontend first drew that object. | `System.ArgumentNullException`: Thrown when is . |
| <a id="api-cf16ed70a1d78e89"></a>[`public bool Inno.Editor.Settings.EditorSetting.IsDefault(Inno.Editor.Settings.EditorSettingObject setting)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L110) | Determines whether a staged field object equals this definition's bound default value.<br>`setting`: The isolated staged object to compare. | when every stored property equals the field default. | `System.ArgumentNullException`: Thrown when is .<br>`System.InvalidOperationException`: Thrown when this definition describes a page rather than a field. |
| <a id="api-8bf30fa95217f9be"></a>[`protected virtual void Inno.Editor.Settings.EditorSetting.OnDraw(Inno.Editor.Settings.EditorSettingObject setting)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L126) | Draws the field using the staged JSON object owned by the Settings frontend. A type that does not override this method describes a page.<br>`setting`: The isolated mutable object being edited. | — | — |
| <a id="api-b899ff2be4685015"></a>[`public sealed class Inno.Editor.Settings.EditorSettingObject : Inno.Core.Serialization.ISerializable`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L13) | Represents one isolated, strongly typed object exposed to a Settings field and its consumers. | — | — |
| <a id="api-2d3b0fcbc105fad1"></a>[`public Inno.Editor.Settings.EditorSettingObject.EditorSettingObject()`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L21) | Creates an empty Settings object. | — | — |
| <a id="api-52d344f4a6fe5ade"></a>[`public bool Inno.Editor.Settings.EditorSettingObject.GetAsBoolean(string name, bool defaultValue = false)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L42) | Gets a Boolean property, or a fallback when the property is absent.<br>`name`: The stable property name.<br>`defaultValue`: The value returned when the property is absent. | The stored Boolean or . | — |
| <a id="api-682e6bd33c37d398"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsBoolean(string name, bool value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L56) | Sets a Boolean property.<br>`name`: The stable property name.<br>`value`: The value to store. | — | — |
| <a id="api-63f063a508f1ed73"></a>[`public int Inno.Editor.Settings.EditorSettingObject.GetAsInt32(string name, int defaultValue = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L73) | Gets a 32-bit signed integer property, or a fallback when the property is absent.<br>`name`: The stable property name.<br>`defaultValue`: The value returned when the property is absent. | The stored integer or . | — |
| <a id="api-aaf6331f9e327223"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsInt32(string name, int value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L87) | Sets a 32-bit signed integer property.<br>`name`: The stable property name.<br>`value`: The value to store. | — | — |
| <a id="api-b1eaf16fc6bf6b0e"></a>[`public uint Inno.Editor.Settings.EditorSettingObject.GetAsUInt32(string name, uint defaultValue = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L104) | Gets a 32-bit unsigned integer property, or a fallback when the property is absent.<br>`name`: The stable property name.<br>`defaultValue`: The value returned when the property is absent. | The stored integer or . | — |
| <a id="api-581d174ebf7a6922"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsUInt32(string name, uint value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L118) | Sets a 32-bit unsigned integer property.<br>`name`: The stable property name.<br>`value`: The value to store. | — | — |
| <a id="api-35eb08e18fa487b4"></a>[`public long Inno.Editor.Settings.EditorSettingObject.GetAsInt64(string name, long defaultValue = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L135) | Gets a 64-bit signed integer property, or a fallback when the property is absent.<br>`name`: The stable property name.<br>`defaultValue`: The value returned when the property is absent. | The stored integer or . | — |
| <a id="api-9a50df0f89d14699"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsInt64(string name, long value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L149) | Sets a 64-bit signed integer property.<br>`name`: The stable property name.<br>`value`: The value to store. | — | — |
| <a id="api-d843f95a8b0fddd1"></a>[`public ulong Inno.Editor.Settings.EditorSettingObject.GetAsUInt64(string name, ulong defaultValue = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L166) | Gets a 64-bit unsigned integer property, or a fallback when the property is absent.<br>`name`: The stable property name.<br>`defaultValue`: The value returned when the property is absent. | The stored integer or . | — |
| <a id="api-8befba820ea695b1"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsUInt64(string name, ulong value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L180) | Sets a 64-bit unsigned integer property.<br>`name`: The stable property name.<br>`value`: The value to store. | — | — |
| <a id="api-bbb80dd033e00446"></a>[`public float Inno.Editor.Settings.EditorSettingObject.GetAsSingle(string name, float defaultValue = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L197) | Gets a single-precision property, or a fallback when the property is absent.<br>`name`: The stable property name.<br>`defaultValue`: The value returned when the property is absent. | The stored number or . | — |
| <a id="api-19c69881d331bda9"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsSingle(string name, float value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L214) | Sets a finite single-precision property.<br>`name`: The stable property name.<br>`value`: The finite value to store. | — | `System.ArgumentOutOfRangeException`: Thrown when is not finite. |
| <a id="api-7984b58553b27520"></a>[`public double Inno.Editor.Settings.EditorSettingObject.GetAsDouble(string name, double defaultValue = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L235) | Gets a double-precision property, or a fallback when the property is absent.<br>`name`: The stable property name.<br>`defaultValue`: The value returned when the property is absent. | The stored number or . | — |
| <a id="api-c08144e3ba5bded3"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsDouble(string name, double value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L252) | Sets a finite double-precision property.<br>`name`: The stable property name.<br>`value`: The finite value to store. | — | `System.ArgumentOutOfRangeException`: Thrown when is not finite. |
| <a id="api-2ba1f77cf1ac4a53"></a>[`public string? Inno.Editor.Settings.EditorSettingObject.GetAsString(string name, string? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L273) | Gets a string property, or a fallback when the property is absent or null.<br>`name`: The stable property name.<br>`defaultValue`: The value returned when the property is absent or null. | The stored string or . | — |
| <a id="api-a2dad42d7bfcab90"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsString(string name, string? value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L287) | Sets a nullable string property.<br>`name`: The stable property name.<br>`value`: The value to store. | — | — |
| <a id="api-2e0bed4527e06352"></a>[`public bool[] Inno.Editor.Settings.EditorSettingObject.GetAsBooleanArray(string name, bool[]? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L304) | Gets a Boolean array property, or a copied fallback when the property is absent.<br>`name`: The stable property name.<br>`defaultValue`: The array copied when the property is absent. | An independently owned array. | — |
| <a id="api-504220d58e235327"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsBooleanArray(string name, bool[] value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L318) | Sets a Boolean array property by value.<br>`name`: The stable property name.<br>`value`: The array copied into the object. | — | — |
| <a id="api-cd7a64b77078f384"></a>[`public int[] Inno.Editor.Settings.EditorSettingObject.GetAsInt32Array(string name, int[]? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L335) | Gets a 32-bit signed integer array property, or a copied fallback when absent.<br>`name`: The stable property name.<br>`defaultValue`: The array copied when the property is absent. | An independently owned array. | — |
| <a id="api-11a5919ca723195e"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsInt32Array(string name, int[] value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L349) | Sets a 32-bit signed integer array property by value.<br>`name`: The stable property name.<br>`value`: The array copied into the object. | — | — |
| <a id="api-999f3779298cff37"></a>[`public uint[] Inno.Editor.Settings.EditorSettingObject.GetAsUInt32Array(string name, uint[]? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L366) | Gets a 32-bit unsigned integer array property, or a copied fallback when absent.<br>`name`: The stable property name.<br>`defaultValue`: The array copied when the property is absent. | An independently owned array. | — |
| <a id="api-5da95bd82ee751b3"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsUInt32Array(string name, uint[] value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L380) | Sets a 32-bit unsigned integer array property by value.<br>`name`: The stable property name.<br>`value`: The array copied into the object. | — | — |
| <a id="api-201731320fccd9d4"></a>[`public float[] Inno.Editor.Settings.EditorSettingObject.GetAsSingleArray(string name, float[]? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L397) | Gets a single-precision array property, or a copied fallback when absent.<br>`name`: The stable property name.<br>`defaultValue`: The array copied when the property is absent. | An independently owned array. | — |
| <a id="api-9de7740540209997"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsSingleArray(string name, float[] value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L411) | Sets a single-precision array property by value.<br>`name`: The stable property name.<br>`value`: The array copied into the object. | — | — |
| <a id="api-f7a1acb8c344eab1"></a>[`public double[] Inno.Editor.Settings.EditorSettingObject.GetAsDoubleArray(string name, double[]? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L428) | Gets a double-precision array property, or a copied fallback when absent.<br>`name`: The stable property name.<br>`defaultValue`: The array copied when the property is absent. | An independently owned array. | — |
| <a id="api-ce9c1b6d5ab8ded9"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsDoubleArray(string name, double[] value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L442) | Sets a double-precision array property by value.<br>`name`: The stable property name.<br>`value`: The array copied into the object. | — | — |
| <a id="api-652a931e9b9a8c4f"></a>[`public string?[] Inno.Editor.Settings.EditorSettingObject.GetAsStringArray(string name, string?[]? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L459) | Gets a nullable string array property, or a copied fallback when absent.<br>`name`: The stable property name.<br>`defaultValue`: The array copied when the property is absent. | An independently owned array. | — |
| <a id="api-1128593d209a243e"></a>[`public void Inno.Editor.Settings.EditorSettingObject.SetAsStringArray(string name, string?[] value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L473) | Sets a nullable string array property by value.<br>`name`: The stable property name.<br>`value`: The array copied into the object. | — | — |
| <a id="api-2da3dcf9bd18d135"></a>[`public sealed class Inno.Editor.Settings.EditorSettingPathAttribute : System.Attribute`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingPathAttribute.cs#L9) | Places an at an arbitrary string Settings path. | — | — |
| <a id="api-adfe8559cc6fd72d"></a>[`public Inno.Editor.Settings.EditorSettingPathAttribute.EditorSettingPathAttribute(string path, int order = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingPathAttribute.cs#L24) | Creates a path placement. A definition that overrides its drawing method becomes a field; a definition that keeps the default drawing method describes the page at the complete path.<br>`path`: The slash-delimited page and field path.<br>`order`: The stable order among fields with the same section and label. | — | `System.ArgumentException`: Thrown when contains an empty segment or is outside the Editor root. |
| <a id="api-0249aec9eab02a48"></a>[`public string Inno.Editor.Settings.EditorSettingPathAttribute.path { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingPathAttribute.cs#L43) | Gets the normalized path including the field label. | — | — |
| <a id="api-7e20abebb148fb4a"></a>[`public int Inno.Editor.Settings.EditorSettingPathAttribute.order { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingPathAttribute.cs#L48) | Gets the stable order among fields with the same section and label. | — | — |
| <a id="api-4e80e324a6987f6f"></a>[`public abstract class Inno.Editor.Settings.ProjectSettingEditor`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L13) | Describes one Editor presentation for a strongly typed runtime project setting. | — | — |
| <a id="api-b85e48a182be2801"></a>[`public Inno.Core.Settings.ProjectSettingId Inno.Editor.Settings.ProjectSettingEditor.settingId { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L26) | Gets the stable runtime project setting protocol read from the target setting type's by the Editor catalog. | — | — |
| <a id="api-b3fa6495838b37d4"></a>[`public string Inno.Editor.Settings.ProjectSettingEditor.path { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L31) | Gets the complete slash-delimited placement path. | — | — |
| <a id="api-b54534bc83ef8b74"></a>[`public string Inno.Editor.Settings.ProjectSettingEditor.pagePath { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L36) | Gets the page that owns this field. | — | — |
| <a id="api-f4181d3e8547f17a"></a>[`public string Inno.Editor.Settings.ProjectSettingEditor.label { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L41) | Gets the display label derived from the final path segment. | — | — |
| <a id="api-82578bd99d6707b0"></a>[`public int Inno.Editor.Settings.ProjectSettingEditor.order { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L46) | Gets the stable order among fields in the same section. | — | — |
| <a id="api-aec42cb9cc8f323f"></a>[`public virtual string Inno.Editor.Settings.ProjectSettingEditor.section { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L51) | Gets the section heading used to group this field. | — | — |
| <a id="api-cd29ab35e3b25599"></a>[`public virtual string Inno.Editor.Settings.ProjectSettingEditor.description { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L56) | Gets the explanation displayed by the unified Settings frontend. | — | — |
| <a id="api-398232a83e308a89"></a>[`public bool Inno.Editor.Settings.ProjectSettingEditor.Draw(Inno.Core.Serialization.ISerializable value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L71) | Draws one isolated staged value through this presentation.<br>`value`: The exact setting type owned by this presentation. | when the value differs from its first drawn state. | `System.ArgumentException`: Thrown when has another runtime type. |
| <a id="api-0509e5363cca3275"></a>[`public bool Inno.Editor.Settings.ProjectSettingEditor.ValuesEqual(Inno.Core.Serialization.ISerializable left, Inno.Core.Serialization.ISerializable right)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L89) | Compares two exact-type values through their native serialized property data.<br>`left`: The first setting value.<br>`right`: The second setting value. | when both values contain equal serialized properties. | `System.ArgumentException`: Thrown when either value has another runtime type. |
| <a id="api-953a34deadff7a5a"></a>[`protected Inno.Editor.Settings.ProjectSettingEditor.ProjectSettingEditor()`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L13) | Inherited or compiler-provided contract. | — | — |
| <a id="api-f3c65486f1ad1216"></a>[`public abstract class Inno.Editor.Settings.ProjectSettingEditor<TSetting> where TSetting : class, Inno.Core.Serialization.ISerializable : Inno.Editor.Settings.ProjectSettingEditor`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L173) | Provides a typed drawing extension for one runtime project setting protocol.<br>`TSetting`: The exact serializable setting type registered by the runtime Plugin or host. | — | — |
| <a id="api-923d473d1e914f20"></a>[`protected abstract void Inno.Editor.Settings.ProjectSettingEditor<TSetting>.OnDraw(TSetting setting)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L184) | Draws the isolated staged value. Mutations remain local until the Settings frontend applies them.<br>`setting`: The isolated current-generation setting snapshot. | — | — |
| <a id="api-f704f9cb9c03e1aa"></a>[`protected Inno.Editor.Settings.ProjectSettingEditor<TSetting>.ProjectSettingEditor()`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L173) | Inherited or compiler-provided contract. | — | — |
| <a id="api-12a17d9cb96eea88"></a>[`public sealed class Inno.Editor.Settings.ProjectSettingPathAttribute : System.Attribute`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingPathAttribute.cs#L9) | Places a strongly typed project setting editor under the Project root of the unified Settings window. | — | — |
| <a id="api-c3c9fa62bd8bdeac"></a>[`public Inno.Editor.Settings.ProjectSettingPathAttribute.ProjectSettingPathAttribute(string path, int order = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingPathAttribute.cs#L23) | Creates a project setting placement.<br>`path`: The slash-delimited field path beginning with Project/.<br>`order`: The stable order among fields in the same section. | — | `System.ArgumentException`: Thrown when is outside the Project root or contains an empty segment. |
| <a id="api-0b6dc98f50d8d7c5"></a>[`public string Inno.Editor.Settings.ProjectSettingPathAttribute.path { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingPathAttribute.cs#L41) | Gets the normalized complete placement path. | — | — |
| <a id="api-ddd7ba010aa95a99"></a>[`public int Inno.Editor.Settings.ProjectSettingPathAttribute.order { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingPathAttribute.cs#L46) | Gets the stable order among fields in the same section. | — | — |
| <a id="api-0c07cf7232992830"></a>[`public sealed class Inno.Editor.Settings.EditorSettings : Inno.Editor.Core.EditorModule`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L16) | Owns the discovered Settings catalog and the project-root Settings document. | — | — |
| <a id="api-2cf08b41819578b9"></a>[`public System.IDisposable Inno.Editor.Settings.EditorSettings.ListenChanges(System.Action<Inno.Editor.Settings.EditorSettings> observer)`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L80) | Observes committed Apply, Undo and Redo changes through an owned Core Event hub.<br>`observer`: Synchronous callback reading the current committed settings; observer failures are logged and isolated. | A subscription owner that must be disposed before its feature or extension generation retires. | `System.ArgumentNullException`: The observer is null. |
| <a id="api-3f20f2e2c713f9d7"></a>[`public long Inno.Editor.Settings.EditorSettings.catalogRevision { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L103) | Gets the current discovered type-catalog revision. | — | — |
| <a id="api-7fcfa8938283631d"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Editor.Settings.EditorSetting> Inno.Editor.Settings.EditorSettings.definitions { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L107) | Gets all discovered path definitions in deterministic path order. | — | — |
| <a id="api-c059b1bc6fafe3e5"></a>[`public Inno.Editor.Settings.EditorSettingObject Inno.Editor.Settings.EditorSettings.Get(string path)`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L121) | Reads an isolated effective object from one complete Settings path.<br>`path`: The slash-delimited Settings field path. | The stored object, or an isolated copy of the field's default object. | `System.ArgumentException`: Thrown when is invalid, missing, or describes a page. |
| <a id="api-835035f6e0741e8e"></a>[`public bool Inno.Editor.Settings.EditorSettings.Apply(System.Collections.Generic.IReadOnlyDictionary<string, Inno.Editor.Settings.EditorSettingObject> values, System.Collections.Generic.IReadOnlySet<string>? resets = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L154) | Atomically applies staged field objects as one shared Undo and Redo history entry.<br>`values`: The complete or partial path-addressed objects to apply from the active catalog.<br>`resets`: Optional paths whose persisted overrides are removed during the same Apply. | when the persisted document changed and one history entry was recorded. | `System.ArgumentNullException`: Thrown when is or contains a null object.<br>`System.ArgumentException`: Thrown when a supplied path is invalid, absent, or describes a page.<br>`System.IO.IOException`: Thrown when the project-root document or its history payload cannot be written.<br>`System.UnauthorizedAccessException`: Thrown when the project-root document is inaccessible. |
| <a id="api-aeb097e55f697350"></a>[`protected override void Inno.Editor.Settings.EditorSettings.OnDispose()`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L231) | Releases resources retained by this feature after it has stopped. | — | — |
| <a id="api-5aab52e5ae248dcc"></a>[`public sealed class Inno.Editor.Settings.ProjectSettingsEditor : Inno.Editor.Core.EditorModule`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L17) | Owns the reloadable Editor presentations and history-aware project override workflow for runtime settings. | — | — |
| <a id="api-1f1897c39ac19dd2"></a>[`public long Inno.Editor.Settings.ProjectSettingsEditor.catalogRevision { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L59) | Gets the active project setting Editor catalog revision. | — | — |
| <a id="api-2e9d8426fa521cad"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Editor.Settings.ProjectSettingEditor> Inno.Editor.Settings.ProjectSettingsEditor.definitions { get; }`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L64) | Gets all active strongly typed project setting presentations. | — | — |
| <a id="api-92edf19d86251442"></a>[`public Inno.Core.Serialization.ISerializable Inno.Editor.Settings.ProjectSettingsEditor.Get(Inno.Editor.Settings.ProjectSettingEditor definition)`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L81) | Creates an isolated editable snapshot for one registered presentation.<br>`definition`: The active presentation whose runtime protocol is requested. | An isolated current-generation value. | `System.ArgumentNullException`: Thrown when is .<br>`System.InvalidOperationException`: Thrown when the runtime setting definition is unavailable. |
| <a id="api-19247b24656fc364"></a>[`public Inno.Core.Serialization.ISerializable Inno.Editor.Settings.ProjectSettingsEditor.GetComposedDefault(Inno.Editor.Settings.ProjectSettingEditor definition)`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L108) | Creates the composed host and Plugin default without the project override.<br>`definition`: The active presentation whose default is requested. | An isolated composed default value. | `System.ArgumentNullException`: Thrown when is .<br>`System.InvalidOperationException`: Thrown when the runtime setting definition is unavailable. |
| <a id="api-c2bb0d4fbdf2e742"></a>[`public bool Inno.Editor.Settings.ProjectSettingsEditor.Apply(System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Settings.ProjectSettingId, Inno.Core.Serialization.ISerializable> values, System.Collections.Generic.IReadOnlySet<Inno.Core.Settings.ProjectSettingId>? resets = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L137) | Atomically applies project-authored overrides and records one stable history entry.<br>`values`: Staged exact-type values keyed by stable setting identity.<br>`resets`: Setting identities whose project overrides should be removed. | when the project override document changed. | `System.ArgumentNullException`: Thrown when is . |
| <a id="api-aecf45a326edb790"></a>[`protected override void Inno.Editor.Settings.ProjectSettingsEditor.OnDispose()`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L162) | Releases resources retained by this feature after it has stopped. | — | — |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Core.Events` | 实现依赖；不传递 compile surface |
| `Inno.Core.Logging` | 实现依赖；不传递 compile surface |
| `Inno.Extensibility.Types` | 实现依赖；不传递 compile surface |
| `Inno.Scripting.Api` | 实现依赖；不传递 compile surface |
| `Inno.Editor.Interactions` | 实现依赖；不传递 compile surface |
| `Inno.Core.Serialization.Generators` | 编译器 Analyzer；不进入运行时 |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Core.Settings` | 公开签名依赖 |
| `Inno.Editor.Core` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
