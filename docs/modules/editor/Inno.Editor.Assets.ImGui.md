# Inno.Editor.Assets.ImGui

[Editor 索引](README.md) · [Wiki 首页](../../README.md) · [UI 架构](../../architecture/editor-ui.md)

## 职责与边界

Asset 浏览呈现与图标发现。依赖 Assets 编辑 feature、Inspection、Settings 和 ImGui；File Browser 消费此呈现模块。编辑操作、文件归档及 History 不属于此程序集。

## 依赖与初始化

产品通过 `EditorProduct.props` 显式部署此程序集。由当前 TypeRegistry/Editor Registry 发现声明并在候选成功后切换；不扫描目录、不建立独立注册表或全局状态。

标准产品组合 Workbench 的默认外观设置贡献。嵌入 Host 也必须组合所需的图标设置贡献；缺少 `Editor/Appearance/Icons/Folder` 等必需设置时明确失败，不从另一个 owner 隐式补齐配置。

## API 与扩展点

AssetBrowserState、AssetBrowserRoot、AssetIconAttribute 和 AssetIconRegistry 提供当前浏览位置、来源、图标声明与查询。AssetBrowserModule 组合图标及无界面的 AssetEditingModule；BasicAssetInspectionDrawer 只做显示。

`AssetBrowserStyle(EditorStyleMetrics)` 在一次绘制前冻结当前用户 Zoom，不保留 Session，也不再次乘上 DPI。默认值使用 100% Zoom。它包含 Browser 图标、网格、面包屑及列比例；通用 pane/splitter/toolbar 指标仍由共享 metrics 提供。尺度在帧边界更新后，调用方重新取得值快照。

## 工作流

使用逻辑 namespace `InnoEditor.Assets` 声明 AssetIconAttribute；修改源内容时调用 Assets feature，不能直接写安装包。

## 错误、生命周期与热重载

控件只在当前 EditorUiSession 的 owner thread、作用域与原生上下文中使用。跨帧只保留稳定 ID 和中立状态；Missing、只读和未赋值状态保持区别。手势失败保留 History 所有权和诊断；扩展在所属 generation 退休，不保留插件 Type、delegate 或实例。

所有公开签名的当前映射见本页 API 清单；内部实现不构成稳定扩展契约。

## API 参考

[完整声明、行为与调用约束](Inno.Editor.Assets.ImGui/api.md)

<!-- api-include: Inno.Editor.Assets.ImGui/api.md -->

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Scripting.Api` | 实现依赖；不传递 compile surface |
| `Inno.Editor.ImGui` | 公开签名依赖 |
| `Inno.Assets.Pipeline` | 公开签名依赖 |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
| `Inno.Extensibility.Types` | 公开签名依赖 |
| `Inno.Editor.Assets` | 公开签名依赖 |
| `Inno.Editor.Core` | 公开签名依赖 |
| `Inno.Editor.Inspection` | 公开签名依赖 |
| `Inno.Editor.Interactions` | 公开签名依赖 |
| `Inno.Editor.Settings` | 公开签名依赖 |
