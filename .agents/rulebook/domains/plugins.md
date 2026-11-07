# domains/plugins

按完整规则单元加载；路由与来源见 [规则登记](../coverage.json)。

<!-- rule-unit: plugins -->
<a id="plugins"></a>
## 18. Plugin 与结构化内容强制边界
- Project 根目录的 `Plugins` 与 `Assets` 平级。`Assets` 是唯一官方可写创作源；完整 Project 通过 File 菜单的 `Export as Plugin` 直接导出 `.iplugin`，不创建 `PluginDefinitionAsset` 或第二套 package authoring asset。`Plugins/*.iplugin` 是唯一安装源；Folder Plugin、`.zip` Plugin 与其他扩展名均不支持。安装包必须经过完整校验并作为只读 Asset Source Mount 进入现有 Asset Catalog、Importer、依赖图和 Artifact 流程，禁止建立 Plugin 专用资产数据库。
- File Browser 与所有 Asset mutation API 必须把 `.iplugin` 安装内容视为逻辑只读。外部替换 `.iplugin` 只表示安装内容更新并触发候选事务，不授予 Editor 内写权限；`Library/Plugins` 始终是不可编辑、可完全重建的缓存。
- `.iplugin` 本质是使用 ZIP 容器格式的本地内容与代码包，不得引入 Package Manager、远程仓库、语义版本解析或平台产物发布系统。Plugin 依赖使用稳定 Plugin ID；导出器根据当前 active Plugin generation 自动声明依赖，并只在明确的 Editor Setting 开启时内嵌扁平、完整、确定性的依赖 `.iplugin`。规范化 source content hash 只用于候选、变化检测与缓存身份。
- Project `Assets` 中以 `~` 开头的目录仍按普通创作内容导入并参与脚本编译，便于完整开发和验证；Project 导出为 `.iplugin` 时这些目录随源内容进入包。安装后的 Plugin Mount 将其视为必须显式 Import 到 Project 的 Sample 子树；Import 必须完整保留原目录名及全部前导 `~`，导入后按 Project 普通内容运行。任何 Source 中的 `~` 子树都不得进入最终 Player runtime closure。
- 脚本引用同一 Source 内的资源必须使用 source-local Asset 路径协议，由脚本程序集的 Asset Source 元数据在 Project 开发态与 `.iplugin` 安装态自动解析；禁止在业务脚本中硬编码 Plugin source ID，也禁止为 Rendering 等具体领域建立 source mount 适配分支。
- Plugin 扩展必须复用现有 `AssemblyDomain.InnoPlugin`、collectible ALC、TypeCache、TypeRegistry 和候选事务。持久状态不得保存 Plugin `Type`、实例或 delegate。
- 所有结构化资产、Graph、Plugin 清单和 Project Settings 必须使用 `ISerializable`、`SerializableProperty`、Serialization Converter 与 `SerializationManager`。只有 C#、Shader source/include 和普通文档等天然文本允许保持文本格式；禁止为 Rendering、Plugin 或 Settings 建立独立 JSON 持久化旁路。
- Plugin 可以同时贡献资产、Shader、Pipeline、Importer、Component、Editor 扩展、设置和玩法代码。Manifest 不得维护各领域类型名单；具体扩展继续由稳定 attribute 和 TypeRegistry 自动发现。
<!-- /rule-unit: plugins -->
