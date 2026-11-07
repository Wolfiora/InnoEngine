# domains/rendering

按完整规则单元加载；路由与来源见 [规则登记](../coverage.json)。

<!-- rule-unit: rendering -->
<a id="rendering"></a>
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
<!-- /rule-unit: rendering -->

<!-- rule-unit: content-frame -->
<a id="content-frame"></a>
## 27. 内容、渲染层次与帧所有权

- `Inno.Content` 只依赖必要 Foundation 契约，统一逻辑 ContentKey、Pack 索引、读取预算和只读 lease。共享 Player、Runtime Session 与 AssetDatabase 不读取部署文件或创建物理目录；具体内容来源负责文件、HTTP、缓存与 owned stream。Asset 消费复用 `ArtifactLease.OpenRead()`，打开的流持有独立 pin。
- 内容缓存必须校验完整文件集合、实际长度及内容哈希；标记文件、mtime 或长度不能代替完整性。候选在独立 staging 验证后原子发布，reader 固定 generation，旧目录只有在全部 reader 释放后才退休。提交后清理失败必须与提交失败区分。
- Rendering 严格分为 Core 图形机制、Assets 运行资产、Assets.Authoring 创作/编译与 Runtime 模型/资源服务。Core 不引用 Assets 或 References；Assets 不引用 Runtime、Shaders 或 Pipeline；Runtime 与 Player 发布闭包不引用 Authoring、Assets Pipeline 或 Shader 编译工具。物理输入文件只属于具体 SDK/toolchain 适配边界。
- 通用输入使用 `Inno.Adapter.Input` 的 Core Events 实现，不能把中立事件快照放进 SDL 项目。存储 namespace 与物理根分离，文档 IO 使用 `IByteDocumentStore`，Session 日志由宿主 factory 交付且只由所属 Session 释放。
- 主呈现尺寸必须真实且显式可用：`null` 表示没有主输出；零尺寸或内部 1×1 资源不能伪装为可用 surface。没有主输出时跳过其建图与输入换算，独立离屏请求仍可运行。
- Contributor snapshot 仅在注册变化时重建，帧开始固定；私有 scratch 在正常帧、异常和退休路径都清空 extension 引用。Graph Validate 只分析，最终 Compile 只执行一次完整资源分配；mutation rollback 同步回滚图与验证状态，不允许修改已接受的 pass。
- Native recipe 声明真实输入闭包与工具身份。operation 内共用初始输入扫描，等待 lease 后及发布前保留必要的重新验证；无关程序集 MVID 不能代替组件 recipe 身份。Task 引导统一、目标属性隔离，内容相同的 native 部署不复制、不替换加载中的 DLL、不更新 mtime。
- Task 完整运行闭包的共享编译必须使用 Core IO lease 协调跨进程写入，等待时释放 MSBuild 节点；最小引导 publisher 使用操作私有中间态，随后由现有不可变发布与 reader 协议管理加载及退休。
<!-- /rule-unit: content-frame -->
