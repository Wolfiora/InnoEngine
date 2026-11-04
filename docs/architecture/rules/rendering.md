## 17. Rendering 强制边界
- Rendering 的公开设计必须同时满足：跨平台、API 易用、扩展灵活和低耦合。不得以实现便利为由破坏其中任一项。
- 只有 `Inno.Adapter.Rendering.Bgfx`、BGFX build toolchains 与对应 native tests 可以引用 `Inno.Native.Bgfx`。BGFX handle、View ID、原生指针和 BGFX 枚举不得出现在其他项目的 public/protected API 中。
- `Inno.Rendering` 必须保持后端中立，且不得引用 Scene、Assets、Editor 或任何具体图形后端。上层模块通过资源描述、能力集合、RenderGraph 和命令编码接口工作。
- 通用 Graph 不得引用 Rendering 或 ImGui；Rendering 也不得反向引用 Shader 创作层或 Editor Graph。Shader 图与源码函数模块属于内置创作层，Material 只保存 Shader 引用与参数，不保存图。
- 所有 Shader 通过图创作，并经统一的 Shader IR、编译、反射、验证和产物缓存链进入运行时；源码语言解析和后端生成由对应 provider 实现，不得建立第二套完整源码 Shader 创作路径。
- Pipeline、Feature、Pass、Shader Node、GPU 资源与编译产物必须 capability-aware、generation-scoped 且 reload-safe。持久状态只保存 Stable ID 与中立数据，禁止长期保存 collectible ALC 的 `Type`、delegate 或 runtime 对象。
- Project 脚本扩展只允许使用后端中立 Rendering API。扩展失败必须隔离，候选成功后只能在帧安全点原子切换，并保留 last-good Pipeline、Shader 和 GPU 资源。
- Rendering Core 只提供图形机制，不得内建 2D、2.5D、3D、PBR、Forward、Deferred、Light、Shadow、Camera、MeshRenderer 或任何具体渲染世界观。所有具体渲染模型必须能够由 Project 脚本或 Plugin 从零组合。
- Shader、Technique、Material 与 Pipeline 通过开放 Stable ID 契约组合；内核不得维护封闭 Pass Tag、Render Path、资源语义或质量设置名单。
