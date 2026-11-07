# Inno.Tooling.Architecture

[Tooling 索引](README.md) · [Wiki 首页](../../README.md)

## 独立输出目录的检查

使用 `--dotnet <host>` 和 `--artifacts-path <absolute-output-root>` 检查独立 SDK 输出。求值图的 `TargetPath` 决定公开程序集的 owner，Wiki 的 `ReferencePath`、`Compile`、语言和符号也使用同一输出选择。不要把独立输出复制回默认 bin 来满足检查。

`--api-project` 只限制需要审查的 owner，不裁掉其公开签名依赖。编译元数据审查与 XML 语义检查共用一次批量 SDK 引用解析，包含实际传递依赖和包引用；没有选中审查的依赖仍从当前输出闭包读取，不能借用旧默认目录中的 DLL。

完整架构检查附带 `--wiki-page` 时，页面选择仅限制 Wiki 检查，引用解析仍覆盖完整架构职责；独立 `--wiki-only` 才能只准备所选页面及其依赖。两种调用不能共用一个过窄的引用集合。

`--configuration` 支持 `Debug`、`SourceDebug` 和 `Release`。`--artifacts-path` 要求显式 `--dotnet`；SDK 失败、缺失引用或取消均不能当作通过。


退休 catch 的 AST 规则拒绝直接捕获 `RetirementPendingException` / `RetirementTimeoutException`（包括全限定名）。
应捕获 Exception，并用 Core `RetirementPendingException.Find` 检查整棵异常树后原样传播。
规则包含两个反例和一个合法 filter 正例；它是源码约束，不声称静态证明任意动态异常、类型别名或 native callback 行为。

Registry 派生实现不得直接调用 `OnCleanupFailed` 代替退休失败传播。源码 AST 检查会拒绝该绕过；统一清理应使用 `DisposeExtensions` 并让 TypeRegistry 封锁 generation。负向行为见[CLI 测试项目](../testing/Inno.Tooling.Architecture.Tests.md)。

本项目是架构验证库，由统一 Build CLI 调用。`ArchitectureValidator.Execute(arguments, cancellationToken)` 从包含
`InnoEngine.sln` 的根目录加载源码与 `.csproj` 图，返回 0 表示通过，1 表示违反项；源码根缺失时抛出
`DirectoryNotFoundException`。可指定源码根与 `--configuration Debug|Release`，默认 Debug。
未知选项、重复配置和多个根抛出 `ArgumentException`。
默认检查手写源码的多参数声明：逐参数换行、右括号对齐，以及紧随参数列表的 `) {`。
检查基于 C# 语法节点，覆盖方法、构造函数、委托、主构造函数、局部函数和 Lambda，不改写源码。

Editor 源码还检查同类型重复别名、重复 XML summary，以及循环中依赖手动 PopId 收尾的 ImGui ID 作用域。循环行必须在局部 finally 中释放 ID，确保异常、break 和正常完成均保持配对。该 AST 门禁识别已确认的源码模式，不替代其他 Begin/End、Push/Pop 的所有权审查或真实 Native 异常测试；分组取得与包装方法仍需沿实际调用方核对。

## 检查范围

统一验证计划先运行 `--documentation-only` 的全仓结构、内容边界与链接检查，再以 `--wiki-contracts-only` 检查选中页面的 API 签名与可编译示例。后者不声称覆盖结构和链接；计划必须将成功的文档检查列为前置证据。独立调用 `--wiki --wiki-only` 仍提供完整 Wiki 检查。

`--documentation-only` 复用 Wiki owner 覆盖、正文与链接规则，验证导航和文档结构而不准备无关 native 产品。包含 API 符号或示例的变更仍由计划选择相应编译与 Wiki 证据。

`--source-only` 只执行全仓廉价源码/所有权规则，不声称编译元数据已经验证。`--metadata-only` 与重复的 `--api-project <repo-relative.csproj>` 核对所选已构建公开边界；默认 `verify` 仍保留完整行为。统一计划将两类证据分别登记，并要求元数据阶段的编译前提通过。

XML 存在性、格式和参数说明在源码阶段检查；`inheritdoc` 的真实继承契约、内容层调用和平台字段的类型语义在所选 owner 的编译阶段使用 SDK 引用验证。干净 checkout 的源码门禁不借用旧 bin；语义规则仍是必须通过的独立证据。

同次源码检查按文件共享 C# 语法树，供格式、XML、退休传播和 using 规则读取；调用结束即释放，不跨运行保存源码状态。需要语义信息时仍使用实际 SDK 编译模型的语法树。共享解析不缩小全仓规则范围。

SDK 图现在复用 `ManagedProjectGraphCapture` 的单进程批量求值。`--sdk-snapshot <file>` 供持有当前输入身份的统一计划消费同一 snapshot；校验器核对当前完整项目集合，或明确选定的 API owner 集合，调用方负责保证对应配置与内容身份。此选项不能用于把旧图当作当前输入认证。

`--wiki --wiki-only` 单独核对文档，避免重复执行源码和已登记的编译 API 门禁。重复的 `--wiki-page <docs-relative.md>` 限定语义签名与示例检查；项目页唯一性、分类索引、内容边界和链接仍全仓检查。页面必须真实存在且属于 docs。编译引用和隐式符号按所选上下文在同一个 SDK 进程批量解析，缺失输出不会退回不完整的参考程序集。规范目录由完整规则覆盖检查负责，其中的说明性代码片段不作为消费者 Wiki 示例执行。

实际 SDK 依赖图同时检查测试层次声明；领域测试的传递构建依赖不得包含绑定生成或原生产品准备，即使相关引用不是运行时引用也不能绕过这一限制。

SDK 工程图检查读取实际程序集身份、产品目标、ProjectReference、Compile 和 using 项，验证依赖方向、源码 owner 与可执行入口。`--project-graph` 的非公开诊断模型只描述真实求值结果，不维护 NuGet 解析规则；[依赖还原](../build/DEPENDENCY_SUBMISSION.md)直接使用标准 SDK 流程。

`--wiki` 同时检查项目页、公开符号、链接、内容边界与独立示例。`--wiki-api inventory.json` 和 `--wiki-examples examples.json` 输出核查清单。
逻辑脚本示例必须显式提供当前脚本编译器生成的 `--wiki-runtime-reference <Inno.ScriptApi.Runtime.dll>`，Editor 示例还需 `--wiki-editor-reference <Inno.ScriptApi.Editor.dll>`。
这些示例只使用 .NET framework 与对应逻辑 API reference；宿主实现程序集不参与。验证器检查程序集身份、冻结输入 bytes，并在示例结果中记录 SHA-256；缺少引用明确失败。
reference 由当前 `ScriptCompiler.GenerateProjectFiles()` 或创作编译生成，不能拿旧生成物的通过结果证明当前 API。
完整示例验收同时指定 `--dotnet <绝对 SDK executable>`：普通宿主示例通过所属项目的 `ResolveReferences` 取得实际 `ReferencePath`，并引用该项目当前已构建的程序集。
每个项目在一次验证中只解析一次；不会引用其他项目碰巧部署的 DLL。结果清单以 `host-sdk` 标记该模式。
同一次求值还读取实际 `Compile` 项、条件编译符号、nullable 和 unsafe 设置，供手写源码 API 清单使用。
被项目排除的文件和未选中的预处理分支不进入清单。已有的生成绑定可参与类型解析，但不手工复制为公共 Wiki 契约；生成器最终输出仍需真实产品编译和元数据检查。
没有指定 SDK 时，`host` 结果只是源码模型的初步检查，不能作为所属项目引用闭包已通过的证据。

源码检查之外，工具通过 Roslyn 读取明确选定配置的程序集元数据，递归检查 public/protected
基类、接口、返回值、参数、泛型约束与外层封闭泛型、数组、tuple、指针及 unmanaged function pointer 中的依赖。
它能识别类型别名和多行签名，不依赖名称正则。有效可见性同时考虑外层容器，internal/private 容器中的 public
成员不被误判为对外 API；private-protected 成员不形成程序集外扩展契约。对应正反例由真实编译 DLL 驱动的 CLI 测试覆盖。
Editor 公开签名中实际需要的项目不得标为 PrivateAssets=compile；只用于实现的项目必须声明 PrivateAssets=compile。
ProjectReference 收口为前置实现组和后置公开 API 组，禁止混合分组、重复类别分组或颠倒顺序。
这两项判断基于实际程序集的公开签名；只看运行时会不会加载某个程序集不能决定其传递性。
Host/Shell 不得公开具体 Adapter。
Native 符号检查目前覆盖 BGFX、SDL3、MiniAudio；ImGui presentation 的边界仍由专项规则约束，
不宣称已经通过统一规则证明所有 ImGui 类型均不可见。

编译元数据检查使用所选 owner 与配置的当前输出，由[统一验证入口](Inno.Tooling.Validation.md)按实际计划准备。源码门禁本身无需预先编译整个解决方案；工具不替代必要编译，也不证明任意插件的动态线程行为。
当前宿主产品采用已构建元数据；没有当前宿主 SDK 的产品组合库采用 Roslyn 源码签名审计，未解析的公开类型明确失败。这项契约检查不表示已在其他平台运行。
只有完整编译元数据能证明某个公开引用未被使用；外国产品的手写源码审计不执行生成器，不能据此把生成注册协议的引用误判成实现依赖。该模式仍检查手写 public/protected 签名、缺失/错误私有引用、禁止的具体类型和分组顺序。生成后的完整公开表面仍需所属平台编译后审计。

- friend assembly、Obsolete、type forwarder、兼容字段和禁用实现名；
- global/implicit using、循环 ProjectReference 和 removed project；
- Core/Build/Runtime/Rendering/Audio/Native 依赖方向；
- Player dependency closure；
- Native 类型泄漏和 Engine 内部脚本 Log facade；
- tests 的 non-public reflection；
- `entry`、`src`、`backends`、`platforms`、`build`、`tools` 的全部创作源码项目必须进入 Solution，按真实职责和组件/平台 owner 归类；验证工具共用 `tools`；
- 全部 test project 必须进入 Solution；组件测试归所属 backend，通用领域/集成测试位于根 `tests`；TestModule/TestAssembly/TestDependency 保留 fixture 职责；
- `Inno.Audio` 不得引用 Runtime、Scene、Editor、Platform、Native 或具体 backend；只有 MiniAudio adapter/toolchain/native/tests 可直接引用 native binding；
- Audio scripting 清单不得导出设备、native binding 或 backend，MiniAudio 实时适配源码不得持有托管 extension generation/reflection 对象；
- public/protected 多行英文 XML 的 summary/param/typeparam/returns/exception contract。

日常开发由统一 `validate` 计划选择编译、源码、API 和 Wiki 证据。单独诊断源码规则时可执行：

```text
dotnet run --project "entry/InnoEngine.Build/InnoEngine.Build.csproj" -- verify . --source-only
```

指定 `--dotnet <绝对 SDK executable>` 后还会通过 SDK 的只读 MSBuild item/property 查询检查实际导入后的
ProjectReference、Compile、程序集身份、产品与 Native target。不会执行 Restore、Build、Native 或生成 target。
与 `--wiki` 同用时，还会在 design-time 模式执行引用解析，可能更新 SDK 的引用解析缓存；要求先还原并构建对应项目，缺少程序集明确失败。
`--project-graph <output.json>` 同时保存此次有效图，必须与 `--dotnet` 一起使用。每个查询有一分钟预算；取消会
停止并完成已启动的子进程。检查包含 imported 平台依赖、普通源码的第二 owner、重复程序集、循环与 Player 创作/构建闭包。
这些检查与源码/公开 API 检查共同使用，不能用只读 evaluation 代替真实编译、发布或实机运行。

`--expand-xml` 只机械展开已有 XML 标签，不改变说明内容。缺失说明由作者补齐，
工具不根据方法名称猜测语义，也不把继承契约替换为占位文本。正常 CI 不使用写入参数。

## 库入口与 Host 隔离检查

项目现在为库，公开 ArchitectureValidator.Execute(arguments) 返回 0/1，默认从当前目录寻找源码根，首个参数可指定根。使用统一 CLI verify 调用。新检查拒绝重复 Native Browser 项目、额外工具 Exe、共享 Foundation/Shell/Player Runtime 的 IsBrowser 判断及 Browser Player 的源码链接。核心 Build 的 Editor 禁止依赖规则继续生效，仅 Build CLI composition root 可组合作者端参考。

BGCS/Cpp2C target profile 必须显式声明 Native owner 内的输出目录，与宿主输出不能相同或互相包含。
此规则覆盖 managed binding 和 C++ bridge，避免任一生成器的原子目录替换删除另一目标的产物。



## 缺失目标契约的诊断

显式 SDK Wiki 检查使用该项目实际求值的引用。目标引用缺失时，记录项目与文件路径，继续检查其他可用项目和示例，最终仍返回失败。缺失项目不计为 API 覆盖，也不会被误报成全部符号已删除。输出清单必须与本次诊断一起阅读；仅有清单文件不能证明全仓通过。

### 明确目标的 Wiki 验证

`verify --wiki --dotnet <SDK> --wiki-target <targetId>` 对共同项目与所选产品执行真实 SDK/API/示例编译，同时保持全仓项目页面、导航、链接和内容边界检查。其他产品目标不会借用宿主 DLL；它们的示例清单标记 `foreign-product` 和 `notEvaluatedReason`，API 编译在日志中明确记录未执行。省略 `--wiki-target` 时仍要求全部产品的真实引用闭包，不自动忽略缺失引用。CI 的 WindowsX64 与 MacOSArm64 job 分别覆盖各自产品；BrowserWasm32 产品编译须单独具备对应环境，目标限定成功不能称为全目标编译通过。

## 开发入口项目边界

项目归属检查要求 entry 仅包含两个普通可执行项目：Editor 以宿主条件项目引用选择真实产品，Build 直接引用共享 CLI。共同生产层不得反向依赖 entry；backend、平台基础模块与 integration 的原有边界继续检查。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Tooling.Architecture.ArchitectureValidator 3647c06158b3bf9362d2d341a8f2ef9e17b9d60c971a1578b3d17e29334db90a -->
<!-- api: M:Inno.Tooling.Architecture.ArchitectureValidator.Execute(System.String[],System.Threading.CancellationToken) aafb5a4f0fe7db5e5d8991f1cad9ca334d0d0a12bd11fc30f9da266defaf1e25 -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-9862893de0812526"></a>[`public static class Inno.Tooling.Architecture.ArchitectureValidator`](../../../tools/Inno.Tooling.Architecture/ArchitectureValidator.cs#L17) | Checks repository dependency, API documentation and source ownership invariants. | — | — |
| <a id="api-c7d46d4b90e3ee02"></a>[`public static int Inno.Tooling.Architecture.ArchitectureValidator.Execute(string[] arguments, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../tools/Inno.Tooling.Architecture/ArchitectureValidator.cs#L63) | Executes repository validation or an explicitly requested documentation maintenance operation.<br>`arguments`: The optional repository path, configuration, XML maintenance flag, explicit SDK, evaluated graph output, and Wiki inventories with explicitly selected runtime and editor script reference assemblies. --wiki-target limits product compilation to that explicit target; shared Wiki checks remain complete.<br>`cancellationToken`: Cancels owned SDK evaluation processes and drains them before returning. | Zero when validation succeeds, or one when any invariant is violated. | `System.ArgumentException`: An option is unknown or more than one repository path is supplied.<br>`System.IO.DirectoryNotFoundException`: The requested path is not inside an engine checkout.<br>`System.OperationCanceledException`: The caller cancels evaluated SDK project inspection.<br>`System.IO.InvalidDataException`: A selected Wiki script reference is not the declared logical API assembly. |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
| `Inno.Build.Toolchains` | 实现依赖；不传递 compile surface |

## 本地查询与文档归属

设计上复用现有项目清单和 API 标记，Markdig 1.5.1 负责 Markdown 语法树与源码位置；查询输出与验证证据分开。用法见 [文档指南](../documentation.md)。

<!-- api: T:Inno.Tooling.Architecture.DocumentationCommand b5ef56492a6fb3fda56415e3556e1d987f3943751b70002bca43b42942116872 -->
<!-- api: M:Inno.Tooling.Architecture.DocumentationCommand.GetProjectDocuments(System.String) b7d2172ae933ee9be3db0ca7b9a28294d8a48271a12f053250d8fc8d121a0b0e -->
<!-- api: M:Inno.Tooling.Architecture.DocumentationCommand.ExecuteAsync(System.String[],System.Threading.CancellationToken) ac37fc1608a5d22da10aac1a2d7659a0c1c1f561e4a8fe620a5e0814f73f3f5a -->

| 声明 | 行为与结果 | 使用契约 | 失败与限制 |
| --- | --- | --- | --- |
| <a id="api-87479b64d7760137"></a>[`public static class Inno.Tooling.Architecture.DocumentationCommand`](../../../tools/Inno.Tooling.Architecture/DocumentationCommand.cs#L14) | 提供本地文档查询与文档归属映射，供现有 Build 和统一验证工具组合。 | 只属于工具层，不是引擎运行时 API，也不进入脚本导出。 | 不负责产品构建或源码 API 认证。 |
| <a id="api-ab5d4f2f87bd5a58"></a>[`public static System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Tooling.Architecture.DocumentationCommand.GetProjectDocuments(string root)`](../../../tools/Inno.Tooling.Architecture/DocumentationCommand.cs#L35) | 以 root 指定的绝对仓库根读取实际项目页及分片，返回仓库相对 Markdown 路径到 csproj 路径的只读映射。 | 复用现有项目覆盖清单与包含解析；不求值 SDK。缺失项目页由必要的文档结构门禁报错，映射不能替代覆盖验收。 | 重复项目页、孤立 API、失效/越界/循环包含拒绝返回部分成功映射。 |
| <a id="api-939d4493f08270bb"></a>[`public static System.Threading.Tasks.Task<int> Inno.Tooling.Architecture.DocumentationCommand.ExecuteAsync(string[] arguments, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../tools/Inno.Tooling.Architecture/DocumentationCommand.cs#L66) | 按 arguments 执行 index、search、read 或 symbol；支持 --format text/json。完成输出返回 0，空搜索和歧义候选明确标记；--offset 继续读取大型章节。 | cancellationToken 贯穿准备和锁等待；索引通过现有 owner、lease 与原子文件发布。查询不启动 SDK、引擎、Native 或网络；sourceApiStatus 明确保持 Unverified。 | 未知/重复/缺值选项、项目或符号抛 ArgumentException；缺页、越界、包含循环和读取中修改抛 IOException；取消抛 OperationCanceledException。 |
