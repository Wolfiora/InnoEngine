# documentation

按完整规则单元加载；路由与来源见 [规则登记](coverage.json)。

<!-- rule-unit: wiki -->
<a id="wiki"></a>
## 11. Wiki 文档维护
- API Wiki 统一位于根目录 `docs/`，入口为 `docs/README.md`。
- Wiki 目录优先映射源码分层：`docs/modules/core`、`docs/modules/assets`、`docs/modules/runtime`、`docs/modules/rendering`、`docs/modules/editor`、`docs/platform`；每个分类必须有 `README.md` 索引。
- 默认每个 `.csproj` 对应一个独立 Markdown 项目页，文件名使用完整项目名，例如 `docs/modules/extensibility/Inno.Extensibility.Types.md`。
- 项目页至少包含：职责与边界、依赖/初始化顺序、所有 `public` API、面向派生实现者的重要 `protected` 扩展点、常见工作流、可编译风格示例、错误/生命周期/热重载注意事项、相邻页面导航。
- API 表格与示例必须以当前源码为依据；不得把 `internal` 实现描述成稳定公开契约。若解释内部机制，应明确标注其非公开性质。
- 新增、删除、重命名或改变公开 API 行为时，在同一变更中同步对应项目页和分类索引。新增项目时同步创建项目页并加入 `docs/README.md` 的覆盖状态。
- 多页之间使用相对 Markdown 链接；每个项目页顶部至少提供分类索引和 Wiki 首页/相邻页面入口。移动页面时必须修复所有入站链接。
- Wiki 正文默认使用中文以便项目查阅；API 名称、代码、代码注释和公开 XML 注释保持英文。不要复制大段源码，用小而完整的示例解释组合方式。
- 文档应区分“当前稳定行为”“内部实现细节”“未来规划”，不得把规划写成已经存在的 API。
- 续写前先从对应目录运行公开类型/成员检索并阅读相关 `.csproj` 依赖；完成后检查 Markdown 链接、页面索引和源码签名是否一致。
<!-- /rule-unit: wiki -->

<!-- rule-unit: documentation-standard-0 -->
<a id="documentation-standard-0"></a>
# Wiki 维护与验证

[架构索引](../../docs/architecture/README.md) · [验证工具](../../docs/development/tooling/Inno.Tooling.Architecture.md) · [Wiki 首页](../../docs/README.md)
<!-- /rule-unit: documentation-standard-0 -->

<!-- rule-unit: documentation-standard-1 -->
<a id="documentation-standard-1"></a>
## 一页一个项目

每个有效项目有唯一同名项目页，并进入所属分类的 README。测试项目页解释验证范围、fixture、前提和执行方式，不把测试方法描述成产品 API。当前使用说明放在 `docs/`；日期化计划、执行过程和历史验收放在 `.agents/records/`。

页面先解释职责、快速工作流、初始化与退出，再展开 API、扩展、失败和验证。所有权必须说明创建方、借用或转移、线程和结束条件；内部机制显式标记为内部实现。
<!-- /rule-unit: documentation-standard-1 -->

<!-- rule-unit: documentation-standard-2 -->
<a id="documentation-standard-2"></a>
## API 表与源码

公开类型、构造、成员、枚举项以及可供外部派生使用的 protected 成员逐项映射。签名包含 overload、nullable、默认参数和泛型约束；类型还核对继承、接口和委托签名。record 的合成公共成员也属于检查范围。

每行给出声明、行为和结果、调用约束、失败信息。源码链接用来定位实现，不能代替使用说明。`inheritdoc` 应解析实际继承契约，并保留当前成员新增约束。编译器生成常量或成员按其真实语义解释，不修改生成源码补文档。

`<!-- api: ... -->` 是符号与签名的核查标记，不是行为质量证明。自动更新的 API 表仍需复核所有权、线程和上下文；表中没有额外约束也不代表线程安全或不会失败。
<!-- /rule-unit: documentation-standard-2 -->

<!-- rule-unit: documentation-standard-3 -->
<a id="documentation-standard-3"></a>
## 示例

- 可执行 C# 示例使用 `csharp` 围栏，并包含所需 using、参数和宿主前提。
- 通过参数借用 Session、资产管线和构建上下文，不假定未声明的全局变量。
- 脚本示例使用逻辑命名空间，应针对当前裁剪脚本 references 验证；不能靠改用实现 namespace 让检查通过。
- 异步示例说明 owner 线程恢复和资源释放；普通 await 不意味着恢复指定线程。
- 只展示声明或流程的文本明确标注用途，不能将失败的可执行示例改成普通文字来绕过验证。
<!-- /rule-unit: documentation-standard-3 -->

<!-- rule-unit: documentation-standard-4 -->
<a id="documentation-standard-4"></a>
## 内容边界

Wiki 通过引擎自己的公开契约解释能力，不依赖外部插件产品、外部消费者路径或特有业务模型。引擎 Plugin 机制、`.iplugin`、Source Mount、Stable ID 和 generation 属于当前引擎文档。浏览器 Canvas 和一般画布概念也可以正常描述。

具体第三方实现名称放在对应 backend、Native、integration 或工具链页；中立服务页按其能力和契约说明职责。已删除的 API、项目路径和命令不能继续作为可用方法出现。
<!-- /rule-unit: documentation-standard-4 -->

<!-- rule-unit: documentation-standard-5 -->
<a id="documentation-standard-5"></a>
## 大型 ABI 参考

Native 项目保持一个权威项目页，先解释初始化、借用地址、callback、线程和退出顺序。完整 ABI 声明可以位于该项目的 `.Api` 子目录，按类型和函数分组；项目页通过明确 `api-include` 声明纳入签名门禁。引用只能指向当前 Wiki 内唯一 Markdown 页面，包含循环和越界直接失败。移动引用时必须修复入站锚点，不能复制第二份权威 API。分页不等于函数行为已经逐项核对；缺少原生语义的部分仍需从当前 facade/header 与真实消费验证。
<!-- /rule-unit: documentation-standard-5 -->

<!-- rule-unit: documentation-standard-6 -->
<a id="documentation-standard-6"></a>
## 当前验证方式

通过 Build 的 `verify` 命令执行 `--wiki` 检查，可额外提供 `--wiki-api <json>` 和 `--wiki-examples <json>` 导出清单。`--dotnet <绝对路径>` 与 `--project-graph <json>` 检查有效 MSBuild 项目图。

逻辑脚本示例使用 `--wiki-runtime-reference <dll>` 和 `--wiki-editor-reference <dll>` 显式选择当前 ScriptCompiler 生成的逻辑 API。验证器分别固定引用 bytes 并记录哈希，不以宿主实现程序集补齐脚本中不可见的类型。生成来源及本次产品状态需要同时记录。

当前 Wiki 检查包括项目页、签名映射、相对链接、内容边界及全部页面的 C# 示例。项目页使用所属项目的编译引用；架构指南、分类索引等非项目页默认只引用 .NET 契约。逻辑脚本例统一使用显式裁剪 references。需要具体引擎组合的完整例子放在对应项目页，指南链接到该例子，避免重复维护。

完整 Host 示例验收必须同时指定 `--dotnet`，以实际 SDK `ReferencePath` 和当前所属程序集编译，并在结果中标记 `host-sdk`。无 SDK 的 `host` 检查只适合初步诊断；仓库中存在的其他程序集不能证明示例拥有对应引用。

指定 SDK 后，API 模型枚举实际 `Compile` 项及条件编译符号。选中的生成绑定、生成类型、嵌套类型和 partial 成员都进入同一清单；位于 `obj` 或 `Generated` 不构成排除理由。未选中的其他目标生成文件不进入当前契约。该过程不执行 Roslyn 源码生成器，不能替代生成后程序集的公开边界检查，也不代表验证了所有产品配置。

Native 项目页分别说明托管初始化入口和生成的 ABI 表面。没有 XML 的生成声明必须核对所属 facade、生成配置和实际消费，补充指针借用、释放和线程限制；不能自动套用托管对象生命周期。生成成员数量与已完成行为说明的数量分别记录，不以签名标记齐全宣称这些说明已完成。

分析器必须理解工程实际选择的 C# 语言版本。无法识别的 `LangVersion` 直接失败，不能使用 Preview 猜测。所选目标、配置或引用输出缺失时先准备对应工程；不能使用其他配置或仓库中碰巧存在的 DLL 补齐检查。

逐页语义审查仍在完善；必须同时阅读[当前验收](../records/acceptance/current.md)，不能把页面存在或标记齐全视为全量 Wiki 已通过。
<!-- /rule-unit: documentation-standard-6 -->
