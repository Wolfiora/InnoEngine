# Inno.Scripting.Api

[Scripting 索引](README.md) · [Extensibility Modules](../extensibility/Inno.Extensibility.Modules.md) · [Editor Scripting](../editor/Inno.Editor.Scripting.md) · [Wiki 首页](../README.md)

`Inno.Scripting.Api` 是一个极小、无 Roslyn 依赖的声明程序集。它定义“哪些真实引擎类型属于脚本 API”的 assembly-level attribute，以及由领域基类声明脚本派生类型类别的 type-level metadata；脚本发现、reference assembly 生成与编译由 Scripting Compiler 负责，热重载由共享 generation 流程与宿主协调。

## 设计边界

- 每个参与脚本 API 的 feature 项目只保留一个 `Properties/ScriptingApi.cs`。清单通常导出本项目类型，也可以选择性导出依赖项目的类型；不设置反向依赖全部模块的中央清单项目。
- 清单必须逐类型显式导出，不允许用“整个 public assembly 都可用”代替。
- `InnoEngine.Scene`、`InnoEngine.Mathematics` 等名称是稳定且可直接 `using` 的脚本 API namespace；它们映射到一个或多个真实 CLR namespace。
- `InnoEngine.Events` 导出 Core `Event`、`EventDispatcher` 与 `EventHub`，上层交互模块复用这组订阅、分发及生命周期契约。
- 导出会为 IDE 生成逻辑 API facade，但 facade 只是编辑期代码模型。Editor 内的热编译仍将逻辑 namespace 转换为真实类型身份，因此热重载、`TypeCatalog` 和序列化看到的是真实 `Inno.*` 类型体系。
- 该项目不知道源文件目录、Roslyn、程序集加载上下文或 Scene 迁移。

## Public API

### ScriptingApiScope

| 值 | 可见范围 |
| --- | --- |
| `Runtime` | `Inno.GameScripts` 和 `Inno.EditorScripts`。 |
| `Editor` | 仅 `Inno.EditorScripts`。 |
| `Authoring` | GameScripts/EditorScripts 的创作编译与 IDE 可见；Player 绑定前移除标注使用、派生标注声明及其 namespace import。 |

`Authoring` 用于独立 namespace 中的编译期标注及其枚举，不是运行时服务权限。自定义展示标注可以派生公开的标注基类；编译器依据导出清单及符号继承关系处理，不维护 Editor 类型名单。把这类类型当作 Player 的普通字段或业务对象使用会产生编译错误，而不会把 Editor DLL 带入 Player。

### ScriptingApiExportAttribute

```csharp
using System;
using Inno.Scripting.Api;

static ScriptingApiExportAttribute DescribeExport(Type exportedType)
{
    return new ScriptingApiExportAttribute(exportedType, ScriptingApiScope.Runtime);
}
```

将一个 public runtime 类型加入指定 profile。声明 assembly 不需要拥有该类型，因此上层 feature 可以有选择地导出底层依赖的类型；reference builder 始终按 `type.Assembly` 生成 runtime reference，CLR 类型身份不会被错误地改成声明 assembly。该实现 namespace 必须存在唯一的 `ScriptingApiNamespace` 映射。

第二个重载只改变脚本 facade 中的类型名称，最终运行时 IL 仍引用原始 CLR 类型。Editor 会同步处理 IDE reference、普通 `using`、完全限定类型名和 XML documentation identity。例如：

```csharp
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("Example.Editor", "Example.Implementation", ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(Example.Implementation.ImplementationType), "PublicApiName", ScriptingApiScope.Editor)]

namespace Example.Implementation
{
    public sealed class ImplementationType { }
}
```

EditorScripts 看到的是 `PublicApiName`，Host 和运行时仍使用 `ImplementationType`。已有 XML 注释会迁移到别名后的 identity；泛型类型不能更名，因为 C# 不支持为开放泛型声明普通 using alias。

清单所有权属于决定公开该能力的 feature，类型的运行时所有权仍属于原程序集。当前 `ImGuiIcon` 和 `NativeImGui` 由 `Inno.Editor.ImGui` 自己导出到 `InnoEditor.ImGui`，FileBrowser 不再维护别名。

### ScriptingApiNamespaceAttribute

```csharp
using Inno.Scripting.Api;

static ScriptingApiNamespaceAttribute DescribeNamespace()
{
    return new ScriptingApiNamespaceAttribute("Example.Tools", "Example.Implementation", ScriptingApiScope.Editor);
}
```

把稳定脚本 namespace 映射到真实 namespace。一个脚本 namespace 可以有多个实现 namespace，例如 Scene 同时映射核心类型和 Components：

```csharp
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace(
    "InnoEngine.Scene",
    "Inno.Scene",
    ScriptingApiScope.Runtime)]

[assembly: ScriptingApiNamespace(
    "InnoEngine.Scene",
    "Inno.Scene.Components",
    ScriptingApiScope.Runtime)]
```

脚本可以直接写：

```csharp
using InnoEngine.Scene;

public sealed class PlayerController : GameBehavior
{
}
```

Editor 会为 IDE 生成一个真正声明 `InnoEngine.Scene.GameBehavior` 等编译期类型的 metadata-only facade。它只保留已导出 API，且生成的 IDE csproj 不参考任何真实引擎 DLL，所以 `Inno.*` 无法被 IDE 解析。

Plugin 源码与 Project 脚本会分别生成 IDE 工程，并通过普通 `ProjectReference` 连接；两者引用同一逻辑 facade，因此插件派生自 `GameBehavior` 等类型时，使用方不需要额外引用实现程序集。

运行时热编译不使用 facade 产物；`ScriptCompiler` 在内存中将已声明的逻辑 `using` 改写到对应实现 namespace，并使用保留真实程序集身份的裁剪参考集编译。最终 IL 因此仍引用 `Inno.Scene.GameBehavior`，不会把 facade 类型带入运行时。

脚本直接写 `using Inno.Scene;` 会得到 `INNO2001` 错误；使用已导出类型却没有导入对应逻辑 namespace 会得到 `INNO2002`。这两个诊断同时用于运行时 Roslyn 编译和生成的 IDE 工程。

### ScriptingAttachableTypeAttribute

```csharp
using Inno.Scripting.Api;

[ScriptingAttachableType("example.tool")]
public abstract class ToolBehavior
{
}
```

领域项目把该 Attribute 放在允许脚本派生、且需要稳定 source-owned Type ID 的根基类上。`kind` 是非空、由领域定义的 manifest 分类名称；API catalog 从真实领域基类读取 metadata 并构建中立映射，编译器再沿脚本类型的基类链匹配该映射，而不硬编码 Scene、Component、System 或任何具体领域类型名。该设计不依赖裁剪 reference assembly 是否复制 host-only Attribute。

例如 `GameBehavior` 与 `GameSystem` 分别声明自己的类别后，Editor Scripting 只依赖这项中立 metadata。将来其他领域增加可附着脚本基类时，只修改该领域基类，无需让 Scripting 项目引用该领域程序集，也无需扩展中央类型名列表。该 Attribute 本身不参与实例迁移，不保存 `Type`，也不会形成 collectible ALC 的长期引用。

### Namespace 导入规则

每个脚本文件必须通过普通 `using InnoEngine.*` 或 `using InnoEditor.*` 显式导入自己使用的逻辑 namespace。编译范围导入、隐式导入、MSBuild `Using` item 和 plugin metadata 注入均被禁止。运行时编译器会把每个文件中的逻辑 namespace 映射为该文件可见的实现 namespace，不会把导入扩散到其他源码文件。

## 标准 Properties/ScriptingApi.cs

每个项目在自己的 `Properties/ScriptingApi.cs` 声明逻辑 namespace 和逐类型导出。例如，拥有一个独立数值类型的模块：

```csharp
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace(
    "Example.Values",
    "Example.Implementation",
    ScriptingApiScope.Runtime)]

[assembly: ScriptingApiExport(typeof(Example.Implementation.Amount), ScriptingApiScope.Runtime)]

namespace Example.Implementation
{
    public readonly record struct Amount(float value);
}
```

新增 Rendering 扩展项目时，只在该项目的 `Properties/ScriptingApi.cs` 声明 `InnoEngine.Rendering` 及其导出类型；无需修改 `ModuleHost`、TypeCache 或中央项目名单。

## 成员可见性

导出单位首先是类型。Editor 在生成裁剪 reference assembly 时只保留 public/protected 签名；若某个成员的签名引用了未导出的非框架类型，该成员不会进入脚本 reference assembly。这避免了通过一个返回内部子系统类型的属性意外扩大 API 闭包。

例如只导出 `StableTypeIdAttribute` 不会顺带让脚本访问 `TypeCatalog`、`TypeRegistry<TSnapshot>` 或 reload context。

## 常见误区

- Namespace 导入不是访问权限；真正的边界来自显式 export、IDE 逻辑 facade、运行时裁剪 reference assembly 和逻辑 namespace 分析器。
- 不要通过生成源码、项目项或 plugin metadata 注入编译范围导入；每个脚本文件都应显式声明依赖。
- 不要为方便而导出整个 Manager/Registry 程序集。先导出脚本确实需要的最小类型。
- 不要在多个文件分散 assembly attribute。每个项目唯一的 `Properties/ScriptingApi.cs` 是可审查的 API 清单。
- 脚本 API namespace 是稳定的源码契约；真实 CLR namespace 仍决定运行时类型身份。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Scripting.Api.ScriptingApiExportAttribute`

<!-- api: T:Inno.Scripting.Api.ScriptingApiExportAttribute 67b8b7c996941cb1c41cb2fa4366aabba8a55b5800269963ef093201681ed493 -->
<!-- api: M:Inno.Scripting.Api.ScriptingApiExportAttribute.#ctor(System.Type,Inno.Scripting.Api.ScriptingApiScope) 333aaaa3e00a9cce47bb618e9d6f6e8c6f3fafbb15d1ce2a7c7ccc428f115370 -->
<!-- api: M:Inno.Scripting.Api.ScriptingApiExportAttribute.#ctor(System.Type,System.String,Inno.Scripting.Api.ScriptingApiScope) 5fc575217e26ebe91f719211efc2004982fce11f78d950138c3ffd2e5c908fe5 -->
<!-- api: P:Inno.Scripting.Api.ScriptingApiExportAttribute.type f4d99e53abbe8749a900685b9130c6de6f17d6670e9b5a95cd0b78c629c363a6 -->
<!-- api: P:Inno.Scripting.Api.ScriptingApiExportAttribute.name 9a7d0328e352c96a2e7dc0b116cf9e78c359acaa2d4285144c30540040dbbc39 -->
<!-- api: P:Inno.Scripting.Api.ScriptingApiExportAttribute.scope 501fb97ecd240c69c459c9dca47e99b5a65a6444dac64a97faa937205e33c570 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8e7c1e256cb3934a"></a>[`public Inno.Scripting.Api.ScriptingApiExportAttribute.ScriptingApiExportAttribute(System.Type type, Inno.Scripting.Api.ScriptingApiScope scope)`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiExportAttribute.cs#L28) | Creates an explicit script API export.<br>`type`：The runtime type exposed to scripts.<br>`scope`：The script profile that receives the type. | — | `System.ArgumentNullException`：Thrown when type is null. |
| <a id="api-e26286d2f836ed54"></a>[`public Inno.Scripting.Api.ScriptingApiExportAttribute.ScriptingApiExportAttribute(System.Type type, string name, Inno.Scripting.Api.ScriptingApiScope scope)`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiExportAttribute.cs#L54) | Creates an explicit script API export with a script-facing type name.<br>`type`：The runtime type exposed to scripts.<br>`name`：The type name presented by the script facade.<br>`scope`：The script profile that receives the type. | — | `System.ArgumentNullException`：Thrown when type is null.<br>`System.ArgumentException`：Thrown when name is empty. |
| <a id="api-c990b5e57d0e88c2"></a>[`public string Inno.Scripting.Api.ScriptingApiExportAttribute.name { get; }`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiExportAttribute.cs#L74) | Gets the type name presented by the script facade. | — | — |
| <a id="api-6dccab98face4cee"></a>[`public Inno.Scripting.Api.ScriptingApiScope Inno.Scripting.Api.ScriptingApiExportAttribute.scope { get; }`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiExportAttribute.cs#L79) | Gets the script profile that receives the type. | — | — |
| <a id="api-e7528dffcbaf945c"></a>[`public System.Type Inno.Scripting.Api.ScriptingApiExportAttribute.type { get; }`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiExportAttribute.cs#L69) | Gets the runtime type exposed to scripts. | — | — |
| <a id="api-c08321ce7d4a65d3"></a>[`public sealed class Inno.Scripting.Api.ScriptingApiExportAttribute : System.Attribute`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiExportAttribute.cs#L14) | Exports one public runtime type to a script compilation profile. | The declaration assembly does not need to own the exported type. Feature projects may expose selected types from lower-level dependencies while the generated runtime reference preserves the exported type's original assembly identity. | — |

### `Inno.Scripting.Api.ScriptingApiIgnoreAttribute`

<!-- api: T:Inno.Scripting.Api.ScriptingApiIgnoreAttribute 76402879ea76ad699104685feb38265ab44ec80e91272cc503ad79c32ac9d3df -->
<!-- api: M:Inno.Scripting.Api.ScriptingApiIgnoreAttribute.#ctor ba879d92c40177bd4827930d9e6800881acac89356fd5f2a1e9bf4164498152e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3ee7f0324c93025e"></a>[`public Inno.Scripting.Api.ScriptingApiIgnoreAttribute.ScriptingApiIgnoreAttribute()`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiIgnoreAttribute.cs#L16) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-6f64bf8a64a6282e"></a>[`public sealed class Inno.Scripting.Api.ScriptingApiIgnoreAttribute : System.Attribute`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiIgnoreAttribute.cs#L16) | Explicitly excludes one otherwise visible member from generated scripting reference assemblies. | — | — |

### `Inno.Scripting.Api.ScriptingApiNamespaceAttribute`

<!-- api: T:Inno.Scripting.Api.ScriptingApiNamespaceAttribute c3800aab3475c6f86716fca498a3064e13ccca1715f837d8414aaf9048e48bd5 -->
<!-- api: M:Inno.Scripting.Api.ScriptingApiNamespaceAttribute.#ctor(System.String,System.String,Inno.Scripting.Api.ScriptingApiScope) 794727996a908719c048d4f1c9e64f7aca5ba0b3880cd25ed132920ac6ba3c98 -->
<!-- api: P:Inno.Scripting.Api.ScriptingApiNamespaceAttribute.name 233de5c09a60efba79ab765ef5db22e8fd5b19f5c14f251eaf90023f33778e01 -->
<!-- api: P:Inno.Scripting.Api.ScriptingApiNamespaceAttribute.implementationNamespace a2ee5839d4274447578801bf1dba063dd1d55c550f4443de7183624ca1b2a423 -->
<!-- api: P:Inno.Scripting.Api.ScriptingApiNamespaceAttribute.scope d99cd31a4290380cdd019a2eac63f603412435f488988852a1ea96753f158332 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d81ed8845797d251"></a>[`public Inno.Scripting.Api.ScriptingApiNamespaceAttribute.ScriptingApiNamespaceAttribute(string name, string implementationNamespace, Inno.Scripting.Api.ScriptingApiScope scope)`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiNamespaceAttribute.cs#L29) | Creates a script API namespace mapping.<br>`name`：The stable script API namespace, such as InnoEngine.Scene.<br>`implementationNamespace`：The CLR namespace containing exported runtime types.<br>`scope`：The script profile that receives the mapping. | — | — |
| <a id="api-17638db6873e8196"></a>[`public string Inno.Scripting.Api.ScriptingApiNamespaceAttribute.implementationNamespace { get; }`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiNamespaceAttribute.cs#L51) | Gets the CLR namespace that implements the script API namespace. | — | — |
| <a id="api-10a6fd943c3a0c9b"></a>[`public string Inno.Scripting.Api.ScriptingApiNamespaceAttribute.name { get; }`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiNamespaceAttribute.cs#L46) | Gets the stable script API namespace. | — | — |
| <a id="api-f2f2f2407968e949"></a>[`public Inno.Scripting.Api.ScriptingApiScope Inno.Scripting.Api.ScriptingApiNamespaceAttribute.scope { get; }`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiNamespaceAttribute.cs#L56) | Gets the script profile that receives the mapping. | — | — |
| <a id="api-ce6d3b3545876a82"></a>[`public sealed class Inno.Scripting.Api.ScriptingApiNamespaceAttribute : System.Attribute`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiNamespaceAttribute.cs#L15) | Maps a stable script-facing API namespace to one CLR implementation namespace. | Multiple implementation namespaces can map to the same script API namespace. This keeps script organization stable without introducing wrapper types with different runtime identities. A feature assembly may declare a mapping for a dependency namespace when that feature owns the decision to expose selected dependency types. | — |

### `Inno.Scripting.Api.ScriptingApiScope`

<!-- api: T:Inno.Scripting.Api.ScriptingApiScope 8199396d53f328feedbb3ac580996aa9be17b96c9060fc9ce8abc784fa1a58fd -->
<!-- api: F:Inno.Scripting.Api.ScriptingApiScope.Runtime 60ec3401a1ce917774b8959523142524ab19d587ddfa2f0151ddc6d40176ba09 -->
<!-- api: F:Inno.Scripting.Api.ScriptingApiScope.Editor b0a63aff9c71ae73afa3ecc6bf826db78d6dea077bb7f2609decf06dfec8237f -->
<!-- api: F:Inno.Scripting.Api.ScriptingApiScope.Authoring d027accd7ed7773673c3a19381e9e6b2063ac8d54fe67fc7cec24ba11cf97b9f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a88e01f56fe52046"></a>[`Inno.Scripting.Api.ScriptingApiScope.Authoring = 2`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiScope.cs#L22) | Exposes compile-time annotations to game and editor scripts. Player compilation erases their applications and derived annotation declarations before binding to target assemblies. | — | — |
| <a id="api-89f8b189626eaec4"></a>[`Inno.Scripting.Api.ScriptingApiScope.Editor = 1`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiScope.cs#L16) | Exposes the API only to editor scripts. | — | — |
| <a id="api-c04a3d0c8ebc80da"></a>[`Inno.Scripting.Api.ScriptingApiScope.Runtime = 0`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiScope.cs#L11) | Exposes the API to both game and editor scripts. | — | — |
| <a id="api-b0f62bcb7c3ca29f"></a>[`public enum Inno.Scripting.Api.ScriptingApiScope : int`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingApiScope.cs#L6) | Identifies the script compilation profile that receives an exported API. | — | — |

### `Inno.Scripting.Api.ScriptingAttachableTypeAttribute`

<!-- api: T:Inno.Scripting.Api.ScriptingAttachableTypeAttribute 44cb8ff97523e449e25f6636f08112e0b2c6eb6225632113eb023fc8914a64a0 -->
<!-- api: M:Inno.Scripting.Api.ScriptingAttachableTypeAttribute.#ctor(System.String) 7f6178e15493d2867d3b8bbd757ab3251b072bd71009e67740a6e6e31c0cea0c -->
<!-- api: P:Inno.Scripting.Api.ScriptingAttachableTypeAttribute.kind 9fb39b1c4d60673dcdf09d1cb9851b9d7e3355c64942e1bf1991fd87a3d417dd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6acd0337a55098a2"></a>[`public Inno.Scripting.Api.ScriptingAttachableTypeAttribute.ScriptingAttachableTypeAttribute(string kind)`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingAttachableTypeAttribute.cs#L21) | Creates attachable script-type metadata for one extensible base class.<br>`kind`：The non-empty domain-defined kind written to script type manifests and diagnostics. | — | `System.ArgumentException`：Thrown when kind is empty or contains only whitespace. |
| <a id="api-6eed48de95272ec8"></a>[`public string Inno.Scripting.Api.ScriptingAttachableTypeAttribute.kind { get; }`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingAttachableTypeAttribute.cs#L30) | Gets the domain-defined attachable type kind recorded in script manifests. | — | — |
| <a id="api-0d395e2cc6e06408"></a>[`public sealed class Inno.Scripting.Api.ScriptingAttachableTypeAttribute : System.Attribute`](../../src/foundation/scripting/Inno.Scripting.Api/ScriptingAttachableTypeAttribute.cs#L10) | Marks a script API base type whose concrete script-derived types require stable source-owned identity metadata. | — | — |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
