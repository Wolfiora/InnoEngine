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
EditorSettingObject value = editorSettings.Get("Editor/Appearance/Grid/Visible");
bool visible = value.GetAsBoolean("value", true);
```

返回对象始终隔离；只有 `EditorSettings.Apply(values, resets)` 才通过 `SerializationRegistry` 原子更新 `Settings.Editor.inno` 并写入统一 Editor History。`EditorSettings.changed` 用于刷新 Editor-only 消费者。

例如 Console 的保留策略只在 `Editor/Diagnostics/Console/Clear on Play` 注册和持久化，默认值为 `true`。Console backend 订阅 `EditorSettings.changed` 并在 Apply、Undo、Redo 后读取当前有效值；Console Panel 不再使用 `editor.ini` 或 toolbar 维护同名状态。

## Project Settings 的 Editor 表现

Project Settings 的运行时定义属于 [Inno.Core.Settings](../core/Inno.Core.Settings.md)。本项目只提供可选的 Editor Drawer 协议，使 Plugin 的强类型设置自动出现在同一个窗口的 `Project/...` 页面。

```csharp
using InnoEditor.ImGui;
using InnoEditor.Settings;
using InnoEngine.Settings;

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

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Editor.Settings.EditorSetting`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Settings.EditorSetting.EditorSetting()`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L23) | Creates a Settings definition. A field supplies its persisted default through ; a page keeps the base implementation. |
| [`bool Inno.Editor.Settings.EditorSetting.Draw(Inno.Editor.Settings.EditorSettingObject setting)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L85) | Draws this field inside the frontend-managed content container. |
| [`bool Inno.Editor.Settings.EditorSetting.IsDefault(Inno.Editor.Settings.EditorSettingObject setting)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L110) | Determines whether a staged field object equals this definition's bound default value. |
| [`virtual void Inno.Editor.Settings.EditorSetting.OnDraw(Inno.Editor.Settings.EditorSettingObject setting)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L126) | Draws the field using the staged JSON object owned by the Settings frontend. A type that does not override this method describes a page. |
| [`virtual Inno.Editor.Settings.EditorSettingObject Inno.Editor.Settings.EditorSetting.defaultValue`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L59) | Creates the default object for a field. Page definitions keep the base implementation, whose internal value is . |
| [`virtual string Inno.Editor.Settings.EditorSetting.description`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L70) | Gets the page or field explanation displayed by the Settings frontend. |
| [`bool Inno.Editor.Settings.EditorSetting.hasValue`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L50) | Gets whether this definition owns one persisted JSON object. |
| [`string Inno.Editor.Settings.EditorSetting.label`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L40) | Gets the display label derived from the final path segment. |
| [`int Inno.Editor.Settings.EditorSetting.order`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L45) | Gets the stable order among fields with the same section and label. |
| [`string Inno.Editor.Settings.EditorSetting.pagePath`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L35) | Gets the page that owns this field, or this page's own path for a page definition. |
| [`string Inno.Editor.Settings.EditorSetting.path`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L30) | Gets the complete slash-delimited identity and placement path. |
| [`virtual string Inno.Editor.Settings.EditorSetting.section`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L65) | Gets the section heading used to group fields alphabetically. Definitions without a section keep the base implementation, whose internal value is . |
| [`Inno.Editor.Settings.EditorSetting`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSetting.cs#L10) | Defines one path-addressed Settings page or one custom-drawn Settings field. |

### `Inno.Editor.Settings.EditorSettingObject`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Settings.EditorSettingObject.EditorSettingObject()`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L21) | Creates an empty Settings object. |
| [`bool Inno.Editor.Settings.EditorSettingObject.GetAsBoolean(string name, bool defaultValue = false)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L42) | Gets a Boolean property, or a fallback when the property is absent. |
| [`bool[] Inno.Editor.Settings.EditorSettingObject.GetAsBooleanArray(string name, bool[]? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L304) | Gets a Boolean array property, or a copied fallback when the property is absent. |
| [`double Inno.Editor.Settings.EditorSettingObject.GetAsDouble(string name, double defaultValue = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L235) | Gets a double-precision property, or a fallback when the property is absent. |
| [`double[] Inno.Editor.Settings.EditorSettingObject.GetAsDoubleArray(string name, double[]? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L428) | Gets a double-precision array property, or a copied fallback when absent. |
| [`int Inno.Editor.Settings.EditorSettingObject.GetAsInt32(string name, int defaultValue = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L73) | Gets a 32-bit signed integer property, or a fallback when the property is absent. |
| [`int[] Inno.Editor.Settings.EditorSettingObject.GetAsInt32Array(string name, int[]? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L335) | Gets a 32-bit signed integer array property, or a copied fallback when absent. |
| [`long Inno.Editor.Settings.EditorSettingObject.GetAsInt64(string name, long defaultValue = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L135) | Gets a 64-bit signed integer property, or a fallback when the property is absent. |
| [`float Inno.Editor.Settings.EditorSettingObject.GetAsSingle(string name, float defaultValue = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L197) | Gets a single-precision property, or a fallback when the property is absent. |
| [`float[] Inno.Editor.Settings.EditorSettingObject.GetAsSingleArray(string name, float[]? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L397) | Gets a single-precision array property, or a copied fallback when absent. |
| [`string? Inno.Editor.Settings.EditorSettingObject.GetAsString(string name, string? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L273) | Gets a string property, or a fallback when the property is absent or null. |
| [`string?[] Inno.Editor.Settings.EditorSettingObject.GetAsStringArray(string name, string?[]? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L459) | Gets a nullable string array property, or a copied fallback when absent. |
| [`uint Inno.Editor.Settings.EditorSettingObject.GetAsUInt32(string name, uint defaultValue = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L104) | Gets a 32-bit unsigned integer property, or a fallback when the property is absent. |
| [`uint[] Inno.Editor.Settings.EditorSettingObject.GetAsUInt32Array(string name, uint[]? defaultValue = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L366) | Gets a 32-bit unsigned integer array property, or a copied fallback when absent. |
| [`ulong Inno.Editor.Settings.EditorSettingObject.GetAsUInt64(string name, ulong defaultValue = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L166) | Gets a 64-bit unsigned integer property, or a fallback when the property is absent. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsBoolean(string name, bool value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L56) | Sets a Boolean property. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsBooleanArray(string name, bool[] value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L318) | Sets a Boolean array property by value. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsDouble(string name, double value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L252) | Sets a finite double-precision property. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsDoubleArray(string name, double[] value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L442) | Sets a double-precision array property by value. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsInt32(string name, int value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L87) | Sets a 32-bit signed integer property. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsInt32Array(string name, int[] value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L349) | Sets a 32-bit signed integer array property by value. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsInt64(string name, long value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L149) | Sets a 64-bit signed integer property. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsSingle(string name, float value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L214) | Sets a finite single-precision property. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsSingleArray(string name, float[] value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L411) | Sets a single-precision array property by value. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsString(string name, string? value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L287) | Sets a nullable string property. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsStringArray(string name, string?[] value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L473) | Sets a nullable string array property by value. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsUInt32(string name, uint value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L118) | Sets a 32-bit unsigned integer property. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsUInt32Array(string name, uint[] value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L380) | Sets a 32-bit unsigned integer array property by value. |
| [`void Inno.Editor.Settings.EditorSettingObject.SetAsUInt64(string name, ulong value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L180) | Sets a 64-bit unsigned integer property. |
| [`Inno.Editor.Settings.EditorSettingObject`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingObject.cs#L12) | Represents one isolated, strongly typed object exposed to a Settings field and its consumers. |

### `Inno.Editor.Settings.EditorSettingPathAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Settings.EditorSettingPathAttribute.EditorSettingPathAttribute(string path, int order = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingPathAttribute.cs#L24) | Creates a path placement. A definition that overrides its drawing method becomes a field; a definition that keeps the default drawing method describes the page at the complete path. |
| [`int Inno.Editor.Settings.EditorSettingPathAttribute.order`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingPathAttribute.cs#L48) | Gets the stable order among fields with the same section and label. |
| [`string Inno.Editor.Settings.EditorSettingPathAttribute.path`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingPathAttribute.cs#L43) | Gets the normalized path including the field label. |
| [`Inno.Editor.Settings.EditorSettingPathAttribute`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/EditorSettingPathAttribute.cs#L8) | Places an at an arbitrary string Settings path. |

### `Inno.Editor.Settings.EditorSettings`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Action<Inno.Editor.Settings.EditorSettings>? Inno.Editor.Settings.EditorSettings.changed`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L72) | Occurs after a complete Settings Apply, Undo, or Redo changes the effective document. The committed service is the event's only argument. |
| [`bool Inno.Editor.Settings.EditorSettings.Apply(System.Collections.Generic.IReadOnlyDictionary<string, Inno.Editor.Settings.EditorSettingObject> values, System.Collections.Generic.IReadOnlySet<string>? resets = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L131) | Atomically applies staged field objects as one shared Undo and Redo history entry. |
| [`Inno.Editor.Settings.EditorSettingObject Inno.Editor.Settings.EditorSettings.Get(string path)`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L96) | Reads an isolated effective object from one complete Settings path. |
| [`override void Inno.Editor.Settings.EditorSettings.OnDispose()`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L209) | Releases resources retained by this feature after it has stopped. |
| [`long Inno.Editor.Settings.EditorSettings.catalogRevision`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L77) | Gets the current discovered type-catalog revision. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Settings.EditorSetting> Inno.Editor.Settings.EditorSettings.definitions`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L82) | Gets all discovered path definitions in deterministic path order. |
| [`Inno.Editor.Settings.EditorSettings`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/EditorSettings.cs#L16) | Owns the discovered Settings catalog and the project-root Settings document. |

### `Inno.Editor.Settings.ProjectSettingEditor`

| 当前声明 | 行为 |
| --- | --- |
| [`bool Inno.Editor.Settings.ProjectSettingEditor.Draw(Inno.Core.Serialization.ISerializable value)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L70) | Draws one isolated staged value through this presentation. |
| [`bool Inno.Editor.Settings.ProjectSettingEditor.ValuesEqual(Inno.Core.Serialization.ISerializable left, Inno.Core.Serialization.ISerializable right)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L88) | Compares two exact-type values through their native serialized property data. |
| [`virtual string Inno.Editor.Settings.ProjectSettingEditor.description`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L56) | Gets the explanation displayed by the unified Settings frontend. |
| [`string Inno.Editor.Settings.ProjectSettingEditor.label`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L41) | Gets the display label derived from the final path segment. |
| [`int Inno.Editor.Settings.ProjectSettingEditor.order`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L46) | Gets the stable order among fields in the same section. |
| [`string Inno.Editor.Settings.ProjectSettingEditor.pagePath`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L36) | Gets the page that owns this field. |
| [`string Inno.Editor.Settings.ProjectSettingEditor.path`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L31) | Gets the complete slash-delimited placement path. |
| [`virtual string Inno.Editor.Settings.ProjectSettingEditor.section`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L51) | Gets the section heading used to group this field. |
| [`Inno.Core.Settings.ProjectSettingId Inno.Editor.Settings.ProjectSettingEditor.settingId`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L26) | Gets the stable runtime project setting protocol read from the target setting type's by the Editor catalog. |
| [`Inno.Editor.Settings.ProjectSettingEditor`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L13) | Describes one Editor presentation for a strongly typed runtime project setting. |

### `Inno.Editor.Settings.ProjectSettingEditor<TSetting>`

| 当前声明 | 行为 |
| --- | --- |
| [`abstract void Inno.Editor.Settings.ProjectSettingEditor<TSetting>.OnDraw(TSetting setting)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L184) | Draws the isolated staged value. Mutations remain local until the Settings frontend applies them. |
| [`Inno.Editor.Settings.ProjectSettingEditor<TSetting>`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingEditor.cs#L173) | Provides a typed drawing extension for one runtime project setting protocol. |

### `Inno.Editor.Settings.ProjectSettingPathAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Settings.ProjectSettingPathAttribute.ProjectSettingPathAttribute(string path, int order = 0)`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingPathAttribute.cs#L23) | Creates a project setting placement. |
| [`int Inno.Editor.Settings.ProjectSettingPathAttribute.order`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingPathAttribute.cs#L46) | Gets the stable order among fields in the same section. |
| [`string Inno.Editor.Settings.ProjectSettingPathAttribute.path`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingPathAttribute.cs#L41) | Gets the normalized complete placement path. |
| [`Inno.Editor.Settings.ProjectSettingPathAttribute`](../../src/composition/editor/framework/Inno.Editor.Settings/Definitions/ProjectSettingPathAttribute.cs#L8) | Places a strongly typed project setting editor under the Project root of the unified Settings window. |

### `Inno.Editor.Settings.ProjectSettingsEditor`

| 当前声明 | 行为 |
| --- | --- |
| [`bool Inno.Editor.Settings.ProjectSettingsEditor.Apply(System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Settings.ProjectSettingId, Inno.Core.Serialization.ISerializable> values, System.Collections.Generic.IReadOnlySet<Inno.Core.Settings.ProjectSettingId>? resets = null)`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L137) | Atomically applies project-authored overrides and records one stable history entry. |
| [`Inno.Core.Serialization.ISerializable Inno.Editor.Settings.ProjectSettingsEditor.Get(Inno.Editor.Settings.ProjectSettingEditor definition)`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L81) | Creates an isolated editable snapshot for one registered presentation. |
| [`Inno.Core.Serialization.ISerializable Inno.Editor.Settings.ProjectSettingsEditor.GetComposedDefault(Inno.Editor.Settings.ProjectSettingEditor definition)`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L108) | Creates the composed host and Plugin default without the project override. |
| [`override void Inno.Editor.Settings.ProjectSettingsEditor.OnDispose()`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L162) | Releases resources retained by this feature after it has stopped. |
| [`long Inno.Editor.Settings.ProjectSettingsEditor.catalogRevision`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L59) | Gets the active project setting Editor catalog revision. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Settings.ProjectSettingEditor> Inno.Editor.Settings.ProjectSettingsEditor.definitions`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L64) | Gets all active strongly typed project setting presentations. |
| [`Inno.Editor.Settings.ProjectSettingsEditor`](../../src/composition/editor/framework/Inno.Editor.Settings/Runtime/ProjectSettingsEditor.cs#L16) | Owns the reloadable Editor presentations and history-aware project override workflow for runtime settings. |

## 项目依赖

- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Editor.Interactions](Inno.Editor.Interactions.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Serialization.Generators](../core/Inno.Core.Serialization.Generators.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Editor.Core](Inno.Editor.Core.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
