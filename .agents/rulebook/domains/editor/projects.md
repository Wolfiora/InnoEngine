---
paths: ["src/composition/editor/**"]
tags: [editor-projects]
---
# Editor projects

## 13. Editor projects and reference boundaries
- Business source in each project under `src/composition/editor` uses a namespace exactly matching its csproj/assembly name. Feature directories organize files without adding namespace segments. For example, types under Inno.Editor.Inspection/PropertyDrawing still use namespace Inno.Editor.Inspection.
- Reusable InspectionDrawers, PropertyDrawers, Registries and serialized-property renderers belong to Inno.Editor.Inspection. Business Panels implement specific Drawers in their own projects and must not reference Inno.Editor.Panel.Inspector merely to extend inspection presentation.
- The sole namespace exception is Inno.Editor.ImGui/Widgets: all its types use namespace Inno.Editor.ImGui.ImGuiWidget.
- Inno.Editor.ImGui/Widgets contains only ImGuiWidget.*.cs files. Each widget's presentation, options, results and private state belong together in ImGuiWidget.<Feature>.cs. Do not create separate Widget helper files.
- Organize Editor project directories by independently understandable capabilities, such as Interactions, Documents, Zoom and PropertyDrawing. Do not mechanically scatter one feature's Actions, Menus, DragDrop, Runtime and Presentation into tiny type-role directories. Ambiguous Internal directories are prohibited; C# declarations, not directory names, express accessibility.
- Group Editor csproj ProjectReferences by public API boundary: the first ItemGroup contains only implementation dependencies absent from all public/protected APIs, each with PrivateAssets="compile"; the second contains only dependencies actually exposed through public signatures, base classes or interfaces. Remove unused references.
- Determine public reference visibility from actual API signatures, not merely runtime assembly use. Recheck the groups after changing public types, base classes, parameters, returns or properties.
