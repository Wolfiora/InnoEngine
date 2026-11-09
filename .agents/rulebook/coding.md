# coding

按完整规则单元加载；路由与来源见 [规则登记](coverage.json)。

<!-- rule-unit: naming -->
<a id="naming"></a>
## 2. 命名规范
- 文件名与主类型名保持一致。
- 默认命名空间与目录层级保持一致；`src/composition/editor` 使用第 13 节定义的项目级命名空间规则。
- 类型名使用 `PascalCase`。
- 接口以 `I` 前缀。
- 成员参数使用语义化 `camelCase`。
- 私有字段使用 `m_` 前缀。
- 常量：`C_` 前缀（如 `C_MAX_COUNT`）或语义清晰的 `readonly static` 命名。
- 代码中的 Unity 风格 API（现有的如 `transform`、`scene`、`active`、`name`）保持周边兼容。
<!-- /rule-unit: naming -->

<!-- rule-unit: members -->
<a id="members"></a>
## 3. 成员声明组织（重要）
- 在类中按访问级别及角色分组，尽量放在类顶部：
  1. 常量（const / static）
  2. 字段（m_）
  3. 构造函数
  4. 公共属性 / 方法
  5. 受保护/内部成员
  6. 私有方法与工具函数
- 一个类内同作用域同类型成员应尽量集中放置，避免在类中散落。
- “public 成员优先”：先写公开成员，再写受限成员。
<!-- /rule-unit: members -->

<!-- rule-unit: xml -->
<a id="xml"></a>
## 5. 注释规范（必须英文）
- 所有对外公开成员（`public`）以及可由外部派生类型重写的 `protected` 成员必须具有完整的英文 XML 注释：
  - 类型与成员必须包含有实际说明意义的 `/// <summary>`，不得只重复成员名称。
  - 每个参数必须有对应的 `/// <param>`；每个泛型参数必须有对应的 `/// <typeparam>`。
  - 非 `void` 方法必须包含 `/// <returns>`，并说明返回值语义以及失败/空值状态。
  - 对调用者可观察的重要异常必须使用 `/// <exception>` 说明触发条件。
  - 重写或实现成员可以使用 `/// <inheritdoc />`，但新增的约束、异常或语义必须在当前成员补充说明。
- 启用 XML 文档输出的项目应将 `CS1572`、`CS1573` 和 `CS1591` 视为编译错误，防止无效参数标签、遗漏参数说明或缺少公开成员注释。
- 关键的 `internal` API 如果对行为有关键影响，也建议补齐 XML 注释。
- 禁止中文注释；临时注释（如 TODO）避免长期留存。
- 复杂逻辑可添加完整英文短句注释。
<!-- /rule-unit: xml -->

<!-- rule-unit: errors -->
<a id="errors"></a>
## 6. 错误处理
- 参数校验优先：`ArgumentNullException.ThrowIfNull`、清晰的 `ArgumentException`。
- 安全失败与异常失败分离。
- 对关键边界返回明确异常信息。
<!-- /rule-unit: errors -->

<!-- rule-unit: identity-naming -->
<a id="identity-naming"></a>
## 7. 命名与清晰度（Transform / Identity 相关约定）
- 对齐上层语义，保持现有 API 命名风格。
- 本地变量避免与属性名重合，减少 shadowing 与可读性冲突。
- 属性值更新后应保持内部缓存一致性，避免重复实时全量计算造成不必要代价。
<!-- /rule-unit: identity-naming -->

<!-- rule-unit: formatting -->
<a id="formatting"></a>
## 23. 源码可读性与排版
- 同一文件针对同一类型只保留一个语义清楚的别名，避免多个 facade 别名交替表达同一调用。XML 文档必须对应当前真实成员，一个成员只保留一个有意义的 summary，不保留已删除成员的说明。
- 可读性是所有手写源码的持续要求，覆盖 InnoEngine、Canvas 与 Rendering2D；生成的 Bindings 和第三方 `extern` 源码不纳入手工排版。改进排版时不得改变执行顺序、初始化顺序、公开契约、序列化数据、诊断或用户体验。
- 优先让读者一眼看清声明和意图：字段、属性、无参数或单参数函数、简单表达式在合理行宽内能完整写成一行时，保持一行；不得仅因机械行宽规则把名称、箭头表达式或成员修饰符随意拆散。
- 所有参数超过一个的函数声明（包括方法、构造函数、局部函数、委托、运算符、主构造函数和 Lambda）必须在 `(` 后换行，每个参数独占一行，最后一个参数后换行，并让 `)` 与声明起始行对齐。紧随参数列表的函数体左花括号与 `)` 同行，写成 `) {`；表达式体、约束和初始化器则在 `)` 后接其语法所需内容。无参数及单参数函数不强制展开。新增和修改源码均执行此规则；自动格式化不能覆盖该规则。

- 一行过长或包含多个独立概念时按语义分行。多行调用与声明采用一致缩进，每行表达清晰的参数或逻辑组；链式调用、条件、集合初始化和异常构造也遵循同一原则。不要为了压缩行数把多条可执行语句挤在一行。
- 空行用于分隔职责与处理阶段，缩进、括号、空格和换行在同一文件内保持统一。简单自动属性可以紧凑；包含流程控制、异常处理或多个语句的成员必须展开成容易逐步阅读的块。
- 注释解释原因、约束和生命周期，不复述代码；XML `summary`、`param`、`returns` 等标签遵守现有架构校验器要求的展开形式，较长内容按完整语义换行。英文注释要求仍按第 5 节执行。
- 可读性整理应逐文件审阅并以纯格式变更为原则；批量处理后核对语法 token、构建与相关测试。不要把重命名、逻辑重构或行为修复混入纯排版提交。

多参数函数示例：

```csharp
public void AAA(
    int param1,
    int param2
) {
    // Method body.
}
```
<!-- /rule-unit: formatting -->

<!-- rule-unit: csharp-standard-0 -->
<a id="csharp-standard-0"></a>
# C# 开发规范

本规范适用于手写 C# 源码。生成代码和第三方源码按其生成协议或上游规范管理。仓库领域协议与产品发布政策由各自架构文档补充。
<!-- /rule-unit: csharp-standard-0 -->

<!-- rule-unit: csharp-standard-1 -->
<a id="csharp-standard-1"></a>
## 1. 设计与职责

- 架构清晰度、依赖方向、所有权、封装和低耦合优先。
- 文件名对应主要类型；目录表达独立可理解的功能，命名空间与所属目录或项目约定一致。
- 公开 API 只暴露完成外部组合所需的最小完整契约。组合优先，依赖通过构造参数或明确组合入口提供。
- 先检查当前仓库已有的事件、身份、序列化、数学和诊断能力；优先复用，避免平行管理系统。
- 扩展行为由接口或基类表达；不可变发现元数据只声明一次。平台、存储、编译器及 presentation 通过可替换边界隔离。
- 实现与持久协议分开。持久数据使用稳定身份与中立值，runtime 对象、委托和类型引用由明确生命周期管理。
- 共用机制、具体后端、平台系统接入、产品组合和构建发行分别拥有源码；共享机制不得选择具体平台或产品。
- 构建宿主只描述工具执行环境；运行目标、CPU、ABI 和托管部署由请求显式选择，不能通过宿主事实推导。
- 第三方组件的运行、Native facade、绑定配置和构建 recipe 由同一个 backend 维护；平台产品通过能力和冻结描述选择它们。
<!-- /rule-unit: csharp-standard-1 -->

<!-- rule-unit: csharp-standard-2 -->
<a id="csharp-standard-2"></a>
## 2. 命名

| 对象 | 规则 | 示例 |
| --- | --- | --- |
| 类型、方法、枚举项 | PascalCase | GenerationRequest、TryResolve、Borrowed |
| 接口 | I 前缀 | IOutputTransaction |
| 属性、参数、局部变量 | camelCase | targetId、outputDirectory |
| 私有字段 | m_ 前缀 | m_generation |
| 常量 | C_ 前缀 | C_MAX_COUNT |
| 静态只读成员 | 清晰语义名称 | DefaultOptions |

- 外部接口实现、override、CLR 特殊成员和外部协议签名遵守其契约。
- 生成绑定名称由对应配置声明。
- 名称表达语义、单位及所有权，避免缩写、无意义 helper 和局部变量 shadowing。
- 源码属性名称与序列化 wire 名称分别声明；命名重构不得意外改变数据协议。
<!-- /rule-unit: csharp-standard-2 -->

<!-- rule-unit: csharp-standard-3 -->
<a id="csharp-standard-3"></a>
## 3. 成员组织

按以下顺序分组：常量/静态成员、字段、构造函数、public、protected/internal、private 实现。相同作用域及类型的成员集中组织，职责之间使用空行。
<!-- /rule-unit: csharp-standard-3 -->

<!-- rule-unit: csharp-standard-4 -->
<a id="csharp-standard-4"></a>
## 4. 声明与排版

- 无参数、单参数函数及短表达式在合理行宽内完整写成一行。
- 超过一个参数的方法、构造函数、局部函数、委托、运算符、主构造函数及 Lambda：在左括号后换行，每个参数独占一行，右括号与声明起始行对齐。
- 紧随参数列表的函数体写作 `) {`；表达式、约束及初始化器接其语法所需内容。
- 长调用、条件、链式调用及集合按语义分行。禁止把多条可执行语句挤在一行。
- 含流程控制、异常处理或多个语句的成员展开为清晰块。
- 使用空行区分职责及执行阶段；全文件缩进与括号风格统一。
- 自动格式化必须保留上述规则。

```csharp
using System;
using System.Collections.Generic;

internal sealed class CommandSet
{
    private readonly Dictionary<string, Action> m_commands = new(StringComparer.Ordinal);

    internal void Register(
        string commandId,
        Action command
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandId);
        ArgumentNullException.ThrowIfNull(command);
        m_commands.Add(commandId, command);
    }
}
```
<!-- /rule-unit: csharp-standard-4 -->

<!-- rule-unit: csharp-standard-5 -->
<a id="csharp-standard-5"></a>
## 5. using 与编译

- 每个文件显式声明普通 using。
- 禁止 global using、隐式导入及 MSBuild Using item。
- 启用 nullable。unsafe 仅限真实互操作边界，并明确缓冲区、指针及资源生命周期。
- 编译目标和 SDK 从工程声明解析；缓存不能替代正确性校验。

<!-- approved-addition -->
- 手写 C# 代码禁止使用 `global::` 限定名。名称冲突通过显式普通 `using`、文件级语义明确的 `using` 类型／命名空间别名或调整局部命名解决；同一目标只保留一个别名，不逐处重复完整限定名。生成代码、生成器输出模板及用于验证该输出的测试按生成协议管理，不为满足手写风格规则改写其限定名。
<!-- /rule-unit: csharp-standard-5 -->

<!-- rule-unit: csharp-standard-6 -->
<a id="csharp-standard-6"></a>
## 6. 文档和注释

- 源码注释使用英文，解释原因、约束、所有权或生命周期。
- public 类型与成员，以及外部可重写的 protected 成员具有完整英文 XML。
- summary 必须说明实际行为；param、typeparam 与签名逐项对应；非 void 方法说明 returns，包含失败或空值语义。
- 重要且可观察的异常使用 exception 说明条件。
- override 或接口实现可使用 inheritdoc；新增约束须补充说明。
- 禁止仅复述名称的模板注释。XML 标签采用可读的展开形式。
- XML 输出项目将 CS1572、CS1573、CS1591 作为编译错误。
<!-- /rule-unit: csharp-standard-6 -->

<!-- rule-unit: csharp-standard-7 -->
<a id="csharp-standard-7"></a>
## 7. 校验、异常与事务

- 优先校验参数，使用 ArgumentNullException.ThrowIfNull 和清晰的 ArgumentException。
- Try 操作的安全失败与异常失败分开。
- 候选完整准备并验证后原子发布；失败保留 last-good 状态。
- 不吞掉关键异常，不把能力不支持当作成功，不使用静默 fallback。
- 修改 API 时更新当前调用方、文档和测试；删除已替代路径。格式变化按所属产品的当前协议处理。
<!-- /rule-unit: csharp-standard-7 -->

<!-- rule-unit: csharp-standard-8 -->
<a id="csharp-standard-8"></a>
## 8. 资源、异步与并发

- 明确创建者、owner、使用线程、借用期、释放者和最终释放顺序。
- borrowed 与 owned 严格区分；Dispose 的幂等性和失败语义明确。
- 异步操作贯穿 cancellationToken；观察任务和子进程退出，排空输出，避免后台悬挂。
- 回调异常不得穿过 unmanaged 边界；注销及取消后确保不再访问已释放 owner。
- 共享进程状态由显式宿主管理，避免匿名全局状态和全局服务定位器。
- 代际资源先停止新工作，再退休并验证释放；失败保留明确故障状态。
<!-- /rule-unit: csharp-standard-8 -->

<!-- rule-unit: csharp-standard-9 -->
<a id="csharp-standard-9"></a>
## 9. 性能

- 明确热路径的缓存、所有权与失效条件。
- 避免每帧完整扫描、几何复制、纹理复制及重复命令编码。
- 优先增量更新、稳定句柄、持久资源缓存及具有明确释放协议的池化缓冲区。
- 不以性能为由泄漏原生借用内存或扩大公共 API。
<!-- /rule-unit: csharp-standard-9 -->

<!-- rule-unit: csharp-standard-10 -->
<a id="csharp-standard-10"></a>
## 10. 测试与验收

- 使用真实公开契约及可替换 public boundary。
- 禁止 InternalsVisibleTo、测试专用后门、反射穿透或为测试扩大 internal 可见性。
- 覆盖实际成功、失败、取消、边界和生命周期，不编写复述实现的测试。
- 原生调用、AOT、裁剪和平台支持需要实际产物证据。
- 构建、测试、SDK、宿主与设备状态分别报告，未执行不得标记通过。
<!-- /rule-unit: csharp-standard-10 -->

<!-- rule-unit: csharp-standard-11 -->
<a id="csharp-standard-11"></a>
## 11. 改动管理

- 纯排版保持语法 token、执行顺序、契约和诊断不变。
- 命名、职责和行为变化与纯排版分开验证。
- 修改公开 API、引用边界及生成协议时同步消费者、索引、示例和架构验证。
- 完成前保证目标范围编译通过，记录验证命令、结果和剩余限制。
<!-- /rule-unit: csharp-standard-11 -->

<!-- rule-unit: csharp-standard-12 -->
<a id="csharp-standard-12"></a>
## 12. Solution 与 MSBuild 项目可读性

- Solution Folder 表达实际职责分层；清理没有项目或有效 Solution Items 的空分组。规模较小的仓库不添加仅包装单个项目的额外类别。
- 所有创作源码项目都应进入 Solution，包括绑定生成扩展和测试 fixture；生成产物与第三方项目不纳入。无入站引用的项目先核对宿主入口、MSBuild 工具调用和生成配置，再判断是否废弃。
- `.csproj`、`.props`、`.targets` 使用两空格缩进和 LF；顶层职责块之间保留一个空行。不要混用 Tab 与空格。
- 简单声明保持一行；过长的属性列表放在元素名后的独立行，每个属性占一行。不得通过拆分属性值改变路径、条件或命令的含义。
- `ProjectReference` 的相对路径统一使用 `/`。实现依赖与公开签名依赖按项目规则分别分组；条件不同、引用可见性不同或有独立职责的分组不得机械合并。
- 删除无属性、无内容和无注释的空 PropertyGroup/ItemGroup；仅在条件、可见性一致且没有中间求值步骤时合并相邻引用分组。
- MSBuild 按顺序求值。纯整理必须核对属性值、条件、metadata、Import、Target 和项目引用的有效顺序，不能把一般 XML 排序当作安全的项目整理。
- 保留真实平台条件及 SDK/工具链约束，平台项目的可读性整理不得将其转移到中立业务层。
<!-- /rule-unit: csharp-standard-12 -->

<!-- rule-unit: csharp-standard-13 -->
<a id="csharp-standard-13"></a>
## 职责展开与边界变更

大 owner 允许按可独立理解的职责使用 partial 文件，但字段、构造和唯一生命周期仍集中在主要文件。先移动成员并核对语法 token 与顺序，再单独改行为/性能。公共 DTO 文件与主类型一致，紧密枚举留在所属协议内；不能为了缩短文件建立平行事件、身份或 Undo 管理器。

公开契约修改同步全部消费者、项目引用可见性、脚本清单、Stable ID、Wiki、成功/失败/边界测试及架构门禁。读取流、后台任务、snapshot、scratch、native callback 和设备资源必须记录 owner、取消、线程归属与最终释放。性能结论由实际测量支持，未测量数值保持未知。
<!-- /rule-unit: csharp-standard-13 -->
