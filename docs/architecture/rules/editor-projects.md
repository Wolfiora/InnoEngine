## 13. Editor 项目组织与引用边界
- `src/composition/editor` 中每个项目的业务源码统一使用与 `.csproj`/程序集名称完全相同的命名空间；功能目录只负责组织文件，不追加到命名空间。例如 `Inno.Editor.Inspection/PropertyDrawing` 中的类型仍使用 `namespace Inno.Editor.Inspection;`。
- 可复用的 InspectionDrawer、PropertyDrawer、Registry 与 serialized property renderer 统一属于 `Inno.Editor.Inspection`；业务 Panel 只在自身项目中实现具体 Drawer，不得为了扩展检查显示而引用 `Inno.Editor.Panel.Inspector`。
- 唯一命名空间例外是 `Inno.Editor.ImGui/Widgets`：其中所有类型使用 `namespace Inno.Editor.ImGui.ImGuiWidget;`。
- `Inno.Editor.ImGui/Widgets` 只允许 `ImGuiWidget.*.cs` 文件。Widget 的 presentation、options、result 与私有状态应收口到对应的 `ImGuiWidget.<Feature>.cs`，不得创建独立的 Widget helper 文件。
- Editor 项目内部按可独立理解的功能建立目录（如 `Interactions`、`Documents`、`Zoom`、`PropertyDrawing`）；同一功能内的 Action、Menu、DragDrop、Runtime 与 Presentation 不得仅按类型角色机械拆成多个细碎目录。禁止使用含义模糊的 `Internal` 目录，访问级别由 C# 声明表达，不由目录名表达。
- Editor `.csproj` 的 `ProjectReference` 必须按公开 API 边界分组：第一个 `ItemGroup` 只放未出现在任何 public/protected API 中的实现依赖，并逐项设置 `PrivateAssets="compile"`；第二个 `ItemGroup` 只放公开签名、公开基类或公开接口实际泄漏的依赖。没有使用的引用应直接删除。
- `ProjectReference` 是否公开必须根据真实 API 签名判断，不能因为运行时会使用某程序集就默认向下游传递。调整公开类型、基类、参数、返回值或属性后，应同步复核引用分组。
