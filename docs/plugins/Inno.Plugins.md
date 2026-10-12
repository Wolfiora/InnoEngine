# Inno.Plugins

[Plugins 索引](README.md) · [Authoring](Inno.Plugins.Authoring.md) · [Runtime](../runtime/Inno.Runtime.md)

该 Player-safe project 只拥有当前 Plugin manifest contract。

## 公开 API

- `PluginManifest`：稳定 Plugin ID、显示名、依赖 ID、override 与 Project Setting contribution 的结构化文档。

Manifest 实现 `ISerializable` 并通过统一 SerializationRegistry 读写。它不列出 Component、Importer、Panel、Shader Node 等扩展类型；这些由 Attribute Registry 发现。Manifest 无 schema version、旧字段 alias 或 fallback reader。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Plugins.PluginManifest`

<!-- api: T:Inno.Plugins.PluginManifest 5ff9b5abbb25ae7b31426a248028e4b7b8f99ed140d76b62e407e042bc3d4a4b -->
<!-- api: P:Inno.Plugins.PluginManifest.pluginId d0dbc679bd4a190106158bfbaa28de5969e3e0d6bd0b5260301900f18d55bd54 -->
<!-- api: P:Inno.Plugins.PluginManifest.displayName 9958427cb3375bb83bf43cbb6095f5201eb9cf8ea4a938736054e3c75a64325a -->
<!-- api: P:Inno.Plugins.PluginManifest.dependencies e2f701724c54f88f9b750a5e406a6a1bc87d4222b1a1d21283767fe3bb4c9788 -->
<!-- api: P:Inno.Plugins.PluginManifest.overrides 5fccb93d8fc675920fddc5cbfc922d2700ce27303093065f04f7c87fc5415e01 -->
<!-- api: P:Inno.Plugins.PluginManifest.contentRoots 0dfcf5177c07fcc77c574bf3bfa359525ac9c8f25939a452f02f8038e3540eb3 -->
<!-- api: P:Inno.Plugins.PluginManifest.assemblyDefinitions cfcfcd332d1d50f17471929e3ef35eb838f33f6d31d60cc4042fba2b08ba808f -->
<!-- api: P:Inno.Plugins.PluginManifest.settingContributions 744602be6a4c9aa12639973e6e9bde8cb8a3385cb070723cd8ecbb6690f33129 -->
<!-- api: M:Inno.Plugins.PluginManifest.Validate 52c540c0c6aa71497b868b879dae1c58ccd522631c1af00df5e4900fdbedbc54 -->
<!-- api: M:Inno.Plugins.PluginManifest.#ctor 5912bf76059f563aa9fca81b5ea4892c2c94cbe6b5a0fc72eade7abc8b330661 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-858df0af83b00de6"></a>[`public Inno.Plugins.PluginManifest.PluginManifest()`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L12) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-438da519d3601df9"></a>[`public void Inno.Plugins.PluginManifest.Validate()`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L62) | Validates stable identity and dependency declarations. | — | `System.IO.InvalidDataException`：Thrown when manifest data is invalid. |
| <a id="api-3bf6d5ca661c56b1"></a>[`public string[] Inno.Plugins.PluginManifest.assemblyDefinitions { get; set; }`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L48) | Gets or sets source-local assembly definition entries. | — | — |
| <a id="api-e5d210f5f60d6805"></a>[`public string[] Inno.Plugins.PluginManifest.contentRoots { get; set; }`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L42) | Gets or sets source-local content roots relative to the Plugin container. | — | — |
| <a id="api-252d68106ad2ba0d"></a>[`public string[] Inno.Plugins.PluginManifest.dependencies { get; set; }`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L30) | Gets or sets Plugin IDs that must activate before this Plugin. | — | — |
| <a id="api-c5f91edec0589dd8"></a>[`public string Inno.Plugins.PluginManifest.displayName { get; set; }`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L24) | Gets or sets the artist-facing display name. | — | — |
| <a id="api-9b44d60520625953"></a>[`public string[] Inno.Plugins.PluginManifest.overrides { get; set; }`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L36) | Gets or sets dependency Plugin IDs whose contributions may be explicitly replaced. | — | — |
| <a id="api-7adfb7a72e7027bd"></a>[`public string Inno.Plugins.PluginManifest.pluginId { get; set; }`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L18) | Gets or sets the globally stable lowercase Plugin ID. | — | — |
| <a id="api-b56533b19e7062fe"></a>[`public Inno.Core.Settings.ProjectSettingRecord[] Inno.Plugins.PluginManifest.settingContributions { get; set; }`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L54) | Gets or sets default project setting contributions. | — | — |
| <a id="api-8610ff9ffa0c5577"></a>[`public sealed class Inno.Plugins.PluginManifest : Inno.Core.Serialization.ISerializable`](../../src/runtime/plugins/Inno.Plugins/PluginManifest.cs#L12) | Describes one local .iplugin package without listing discovered extension types. | — | — |

## 项目依赖

- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
