## 12. Scripting API 清单
- 参与脚本 API 的每个项目只允许一个 `Properties/ScriptingApi.cs`，不得把导出 attribute 分散到业务源码或集中到一个反向依赖所有模块的清单项目。
- 使用 `ScriptingApiExport` 逐类型显式导出；禁止恢复按程序集暴露全部 public API 的 metadata/property 机制。
- 使用稳定脚本分组名（如 `InnoEngine.Scene`、`InnoEngine.Mathematics`、`InnoEditor.Inspection`）和 `ScriptingApiNamespace` 映射真实 CLR namespace。
- 新增模块（如 Rendering）只修改自己的 `Properties/ScriptingApi.cs`，不得在 Editor 编译器中维护中央程序集/type 白名单。
- 运行时编译和 IDE project 必须共用同一组裁剪 reference assemblies；修改清单后需同时验证两条路径。
- 完全禁止 compilation-wide/global using，包括手写指令、MSBuild `Using` item、隐式导入和通过 metadata 注入。所有源码与脚本必须在使用它们的文件中显式声明普通 `using`。
- 脚本必须使用逻辑 namespace（如 `using InnoEngine.Scene;`），不得直接使用实现侧 `Inno.*` namespace。
