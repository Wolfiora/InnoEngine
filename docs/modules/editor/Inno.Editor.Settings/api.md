# Inno.Editor.Settings API

[项目入口](../Inno.Editor.Settings.md)

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
