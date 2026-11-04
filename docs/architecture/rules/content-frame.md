## 27. 内容、渲染层次与帧所有权

- `Inno.Content` 只依赖必要 Foundation 契约，统一逻辑 ContentKey、Pack 索引、读取预算和只读 lease。共享 Player、Runtime Session 与 AssetDatabase 不读取部署文件或创建物理目录；具体内容来源负责文件、HTTP、缓存与 owned stream。Asset 消费复用 `ArtifactLease.OpenRead()`，打开的流持有独立 pin。
- 内容缓存必须校验完整文件集合、实际长度及内容哈希；标记文件、mtime 或长度不能代替完整性。候选在独立 staging 验证后原子发布，reader 固定 generation，旧目录只有在全部 reader 释放后才退休。提交后清理失败必须与提交失败区分。
- Rendering 严格分为 Core 图形机制、Assets 运行资产、Assets.Authoring 创作/编译与 Runtime 模型/资源服务。Core 不引用 Assets 或 References；Assets 不引用 Runtime、Shaders 或 Pipeline；Runtime 与 Player 发布闭包不引用 Authoring、Assets Pipeline 或 Shader 编译工具。物理输入文件只属于具体 SDK/toolchain 适配边界。
- 通用输入使用 `Inno.Adapter.Input` 的 Core Events 实现，不能把中立事件快照放进 SDL 项目。存储 namespace 与物理根分离，文档 IO 使用 `IByteDocumentStore`，Session 日志由宿主 factory 交付且只由所属 Session 释放。
- 主呈现尺寸必须真实且显式可用：`null` 表示没有主输出；零尺寸或内部 1×1 资源不能伪装为可用 surface。没有主输出时跳过其建图与输入换算，独立离屏请求仍可运行。
- Contributor snapshot 仅在注册变化时重建，帧开始固定；私有 scratch 在正常帧、异常和退休路径都清空 extension 引用。Graph Validate 只分析，最终 Compile 只执行一次完整资源分配；mutation rollback 同步回滚图与验证状态，不允许修改已接受的 pass。
- Native recipe 声明真实输入闭包与工具身份。operation 内共用初始输入扫描，等待 lease 后及发布前保留必要的重新验证；无关程序集 MVID 不能代替组件 recipe 身份。Task 引导统一、目标属性隔离，内容相同的 native 部署不复制、不替换加载中的 DLL、不更新 mtime。
- Task 完整运行闭包的共享编译必须使用 Core IO lease 协调跨进程写入，等待时释放 MSBuild 节点；最小引导 publisher 使用操作私有中间态，随后由现有不可变发布与 reader 协议管理加载及退休。
