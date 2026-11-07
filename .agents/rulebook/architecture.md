# architecture

按完整规则单元加载；路由与来源见 [规则登记](coverage.json)。

<!-- rule-unit: style -->
<a id="style"></a>
## 1. 项目风格目标
- 保持当前仓库一致的可读性与可扩展性。
- 优先保持层次清晰、边界明确、低耦合。
- 所有新增注释默认使用英文（尤其是公开 API）。
<!-- /rule-unit: style -->

<!-- rule-unit: encapsulation -->
<a id="encapsulation"></a>
## 4. 封装与职责边界
- 公开 API 以最小必要暴露原则实现。
- 组合优先、继承次之。
- 通过注册器 / 工厂 / 抽象接口解耦。
<!-- /rule-unit: encapsulation -->

<!-- rule-unit: directories -->
<a id="directories"></a>
## 10. 目录边界
- `src/foundation`：Core、扩展目录、类型及代际契约。
- `src/content`：Assets、References、Scene、Animation。
- `src/services`：后端中立的领域契约与运行服务。
- `src/runtime`：Runtime、部署、生成器、Plugin 与创作态脚本流程。
- `src/adapters`：中立 SPI、开放 Provider/Catalog 与 Core Events 输入实现。
- `backends/<component>`：共享 backend 的 runtime、native、build recipe、组件测试；每个组件只有一个源码 owner。
- `platforms/<platform>`：系统与 SDK 接入、明确目标、Editor/Player 薄入口、布局和 Support Pack；不得引用另一个平台。
- `src/composition`：默认引擎、Shell、Player 与 Editor 组合入口。
- `entry`：两个轻量开发入口与当前宿主路由；其他生产层不得反向引用它。BGCS 包族由 `Inno.Packages.props` 固定 NuGet 版本；不提供源码依赖入口。
- `build`、`tools`、`tests`：中立统一构建、具体发行注册、验证库和跨组件契约测试。
- 新文件尽量放置在匹配现有分层与职责目录。
<!-- /rule-unit: directories -->

<!-- rule-unit: no-legacy -->
<a id="no-legacy"></a>
## 15. 禁止 Legacy 兼容与 Schema Version
- InnoEngine 是自用且始终按当前源码、当前 Project 数据共同演进的引擎。新增或修改功能时不支持旧版文件、旧版 schema、旧字段、旧缓存目录、旧 namespace、旧 API 或旧序列化布局，也不得为它们添加 fallback reader、migration、compatibility alias、former ID、deprecated wrapper 或双写逻辑。
- 持久化模型、Attribute、Importer、Build Processor、History、Workspace、Scene、Prefab、Asset metadata、Catalog 和脚本 manifest 不得引入用于 legacy 适配的 `version`、`schemaVersion`、`formatVersion`、`formerVersion` 等字段。格式发生变化时直接更新当前 writer、reader、测试、文档和当前 Project 数据；旧数据可以明确失效并重新生成。
- 不得因为“未来可能兼容”预留迁移分支。只有用户在具体任务中明确要求导入某一种旧格式时，才可以实现一次性、边界清晰的转换工具；转换逻辑不得进入正常运行路径。
- 删除或重构 API 时同步修改所有调用方，不保留旧 overload、旧 namespace facade、转发类型或 `[Obsolete]` 兼容层，除非用户明确要求保留。
- 以上规则不禁止保障当前运行正确性所需的运行时标识，例如 TypeCache/Assembly generation、并发 revision、change counter、content hash、artifact fingerprint、MVID、job handle generation，以及只用于拒绝损坏或错误格式输入的严格 magic/header。它们不得演变成读取多代 legacy schema 的兼容机制。
- 代码审查或清理包含 `version`、`legacy`、`migration`、`compatibility`、`former`、`deprecated` 等名称的实现时，必须先判断其是否只服务于旧数据/API；如果是，应连同测试和文档一起删除，而不是继续扩展。
<!-- /rule-unit: no-legacy -->

<!-- rule-unit: architecture -->
<a id="architecture"></a>
## 21. 架构优先、语言前端与第三方实现边界
- 架构清晰度、依赖方向、职责归属与低耦合是所有功能、性能与开发便利性之上的最高优先级。任何新功能或扩展都必须先明确所属层、公共契约、生命周期与失败边界；不得以临时直连、实现名泄漏、全局单例或跨层便利调用换取短期进度。
- 通用源码资产在服务层与资产层中必须被视为惰性的文本或字节载体。除非某种语言已经被明确规定为引擎公共标准，否则核心导入器、运行时服务与脚本契约不得解析、推断或依赖第三方语法，也不得根据当前后端或文件名隐式选择语义。
- 语言与实现选择必须使用稳定、显式、可序列化的 `languageId` 与 `implementationId`。语言前端必须通过现有 TypeRegistry/TypeCache 体系形成代际化的不可变快照；前端只接收受控源码解析器与不可变请求，并产出中立依赖、结构化诊断和冻结产物。词法、预处理、语法解析、降级规则与原生编译必须留在具体前端、适配器或工具链中。
- 若引擎决定把某种语言定义为公共标准，必须在公共契约和文档中明确声明；否则 BGFX SC、RML、RmlUi、FreeType、HarfBuzz 等实现名、专有术语、能力限制和错误文本不得出现在服务层、脚本层或中立资产契约中，只能存在于具体实现及其诊断中。
- 可扩展运行时后端必须使用开放的稳定 ID、provider catalog 与 capability 描述；不得用封闭枚举或核心层 `switch` 作为第三方扩展入口。服务层只能表达中立能力缺失，具体实现负责解释自己的限制。
- 第三方 C++ 库必须由所属 `backends/<component>/native/Inno.Native.XXX/Native` 中的手写语义适配器隔离；同一项目的 BGCS 配置与生成扩展归属 `Bindings/`，managed 单文件归属 `Generated/Bindings.cs`；CMake 中间产物归属对应 `backends/<component>/build/Inno.Build.Toolchains.XXX/obj/native`，不得写入 `Native/`；`extern` 第三方源码不得修改。适配器只公开后端中立的窄 C++ facade，并通过 PImpl/不透明句柄和中立 DTO 隐藏第三方类型。默认由 BGCS Cpp2C 从该 facade 生成 C ABI，再由 BGCS 从生成的 C header 生成平台无关路径的单文件 managed binding；原生和 managed 生成物分别归属独立输出根，禁止手写 export/import 或修改生成代码。仅当 BGCS 无法表达且有明确记录的 lowering/shim 需求时，才允许额外手写 C ABI。原生句柄、符号和第三方类型不得越过适配层；帧热路径 callback 应留在原生侧，不得仅为 binding 方便穿越 managed 边界。第三方库的进程全局状态必须由显式进程宿主管理引用计数、线程归属、会话隔离、代际与最终释放，禁止用匿名全局变量伪装成 session-local 状态。
- 为源码插件提供后端中立的底层公共 API 是允许且必要的；“最小 public”指完成外部插件组合所需的最小稳定表面，而不是只允许游戏逻辑级 API。插件作者 API 必须显式进入 scripting 导出清单；后端 SPI、原生桥、具体实现和第三方句柄必须保持未导出。
- 帧热路径必须显式定义所有权与生命周期。禁止每帧无条件复制完整几何、索引、纹理像素或为每条绘制命令重复编码；优先使用代际化稳定句柄、资源增量更新/释放、持久 GPU 缓存与具有明确释放协议的池化或所有权缓冲区。不得以优化为由向公共层暴露不安全的原生借用内存。
- 新系统或边界调整必须同步更新架构验证器中的项目分类、解决方案归属、依赖规则、原生消费规则、scripting allowlist 与公共文档；仓库级架构验证通过之前不得视为完成。
<!-- /rule-unit: architecture -->

<!-- rule-unit: core-reuse -->
<a id="core-reuse"></a>
## 22. 基础能力复用
- 新增事件、身份、序列化、数学、诊断或其他基础能力前，先检查 `Inno.Core` 是否已有可用契约；优先直接使用或封装现有契约，禁止在 UI、Canvas、插件等上层模块另建平行管理系统。
- UI 与插件事件的订阅、分发及生命周期统一复用 `Inno.Core.Events`。领域可以定义自己的事件类型及适配入口，但不能另建队列、监听器注册表或事件总线来取代核心事件管理。
<!-- /rule-unit: core-reuse -->

<!-- rule-unit: solution -->
<a id="solution"></a>
## 26. Solution 与项目文件整理

- Solution Folder 与实际架构职责对应，删除没有有效项目或 Solution Items 的空分组；小型项目集合避免无必要的单项目包装层。
- `entry`、`src`、`backends`、`platforms`、`build`、`tools` 与 `tests` 中的创作源码项目必须纳入 Solution，包括绑定生成扩展和测试 fixture；`obj`、`artifacts` 及 extern 的独立第三方源码项目不纳入。没有入站 ProjectReference 不代表项目无用，清理前必须核对宿主入口、MSBuild 工具调用与生成配置。
- `.csproj`、`.props`、`.targets` 使用两空格缩进，顶层职责块以空行分隔，长属性列表逐属性换行，ProjectReference 相对路径使用 `/`。
- 只合并条件和引用可见性一致的相邻引用分组。不得重排 Import、Property、Target 或改变条件、metadata 和求值顺序；Editor 公开与实现引用边界必须保留。
- 详细规则见 `.agents/rulebook/coding.md`；整理后核对有效 XML、Solution 项目身份与配置，并运行受影响构建及架构验证。
- 长时间构建与验收应在阶段结束后清理本次操作所属的旧工具快照、私有加载目录和测试浏览器 profile，并检查可用磁盘空间。清理前核对明确 owner、解析后的绝对路径、符号链接和活跃进程；保留当前产物及验收日志，不清理用户或 SDK 的全局缓存。含 hardlink/压缩文件的逻辑大小不能当作实际释放空间。
<!-- /rule-unit: solution -->
