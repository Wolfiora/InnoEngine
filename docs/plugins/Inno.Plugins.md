# Inno.Plugins

[Plugins 索引](README.md) · [Authoring](Inno.Plugins.Authoring.md) · [Runtime](../runtime/Inno.Runtime.md)

该 Player-safe project 只拥有当前 Plugin manifest contract。

## 公开 API

- `PluginManifest`：稳定 Plugin ID、显示名、依赖 ID、override 与 Project Setting contribution 的结构化文档。

Manifest 实现 `ISerializable` 并通过统一 SerializationRegistry 读写。它不列出 Component、Importer、Panel、Shader Node 等扩展类型；这些由 Attribute Registry 发现。Manifest 无 schema version、旧字段 alias 或 fallback reader。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Plugins.PluginManifest`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectSettingRecord[] Inno.Plugins.PluginManifest.settingContributions`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L53) | Gets or sets default project setting contributions. |
| [`Inno.Plugins.PluginManifest`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L12) | Describes one local .iplugin package without listing discovered extension types. |
| [`string Inno.Plugins.PluginManifest.displayName`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L23) | Gets or sets the artist-facing display name. |
| [`string Inno.Plugins.PluginManifest.pluginId`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L17) | Gets or sets the globally stable lowercase Plugin ID. |
| [`string[] Inno.Plugins.PluginManifest.assemblyDefinitions`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L47) | Gets or sets source-local assembly definition entries. |
| [`string[] Inno.Plugins.PluginManifest.contentRoots`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L41) | Gets or sets source-local content roots relative to the Plugin container. |
| [`string[] Inno.Plugins.PluginManifest.dependencies`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L29) | Gets or sets Plugin IDs that must activate before this Plugin. |
| [`string[] Inno.Plugins.PluginManifest.overrides`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L35) | Gets or sets dependency Plugin IDs whose contributions may be explicitly replaced. |
| [`void Inno.Plugins.PluginManifest.Validate()`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L62) | Validates stable identity and dependency declarations. |

## 项目依赖

- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
