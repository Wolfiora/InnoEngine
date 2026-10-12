# Inno.Editor.Scripting

[Editor 索引](README.md) · [Scripting API](../scripting/Inno.Scripting.Api.md) · [Assets](../assets/README.md) · [Modules](../extensibility/Inno.Extensibility.Modules.md) · [目标 reload 强制标准](../architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md)

`Inno.Editor.Scripting` 把 Project 与已激活 `.iplugin` Mount 中的 C# source、assembly definition 当成正式资产，再把它们编译为可回滚的 collectible Script Module。文件发现和变化来源是统一 Asset Database；该项目没有自己的 `FileSystemWatcher`，也不递归扫描 Project 目录。

## Source 资产

| Source | Asset | Importer | Named outputs |
| --- | --- | --- | --- |
| `*.cs` | `ScriptSourceAsset` | `CSharpScriptImporter` | `source`, `diagnostics`, `type-manifest`, `asset-state` |
| `*.iasmdef` | `ScriptAssemblyDefinitionAsset` | `ScriptAssemblyDefinitionImporter` | `source`, `asset-state` |

两个 Importer 都显式声明 `AssetDeploymentScope.AuthoringOnly`：这些 Asset 在 Editor 中保留 persistent identity、编译输入和诊断，但 Game 导出只部署编译后的 Runtime DLL，不复制 `.cs`、`.iasmdef` 或它们的 source artifact。

每个受支持 source 都有 `.imeta` 和 persistent ID。`type-manifest` 保存该 source 的声明、位置和 partial 信息；聚合编译后还会生成 assembly 级 `*.types.cache`，记录可附加类型最终使用的 source identity、Stable Type ID、类型种类和 canonical source。该文件和 `diagnostics.cache` 都是可丢弃、严格校验的内部编译缓存，不是项目结构化资产。C# 语法错误不会取消 source identity；parse diagnostics 进入 source asset，聚合 assembly build 可以失败并继续运行旧程序集。

Compiler 读取不可变 artifact snapshot，不直接读取正在被外部编辑器写入的 source 文件。普通 Project 编译使用已提交 Asset Catalog；Plugin 安装/更新使用隔离 `AssetSourceMountTransaction` 的候选 Catalog。两者都保证一次 build 的 fingerprint、语法树和 Plugin source 来自同一 revision，但候选 Plugin 在成功前不会出现在 active Asset/Plugin API 中。

每个脚本 assembly 还会生成 `Inno.AssetSource` metadata：Project assembly 的值是 `project`，安装包 assembly 的值是所属 Plugin ID。业务脚本使用 `Assets.LocalPath("Materials/Default.imaterial")` 建立 source-local 引用，因此同一份完整 Project 源码在直接开发时读取 Project Assets，导出并安装为 `.iplugin` 后读取自己的只读 Mount。业务代码不声明、探测或硬编码 Plugin source ID；这项能力属于所有脚本系统，不是 Rendering 专用适配。

Project `Assets` 中的 `~` 目录仍作为普通 source 导入并参与 assembly discovery，开发工程可以直接运行其中代码。相同目录被打入 `.iplugin` 后，在安装态 Mount 中成为待 Import sample，不参与 Plugin assembly；任何 `~` 子树都不会进入 Player runtime closure。

## Assembly Definition

```csharp
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Scripting.Compiler;

static bool SaveDefinition(AssetPipeline assets)
{
    var definition = new ScriptAssemblyDefinitionAsset(
        "Project.Gameplay", ScriptAssemblyScope.Runtime,
        references: ["Project.Common"], defines: ["GAMEPLAY_DEBUG"]);
    return assets.Save(AssetPath.Project("Scripts/Gameplay/Gameplay.iasmdef"), definition);
}
```

`.iasmdef` 是 Inno Serialization 原生资产，不是 JSON 文档。Editor 与工具通过 `AssetPipeline.Save` 和对应 Importer 共用同一序列化、metadata 与依赖通道。

最近父目录的 `.iasmdef` 决定脚本归属。没有 definition 时：

- `*.editor.cs` 进入 `Inno.EditorScripts`；
- 其他 `.cs` 进入 `Inno.GameScripts`。

显式 asmdef 优先于 filename convention。Editor assembly 可引用 Runtime；Runtime 引用 Editor 会在候选 build 前失败。引用 cycle、未知 assembly、保留名冲突同样作为 compilation diagnostic 返回。

固定 builtin assembly name 不取 Project 文件夹名，因此 Project 改名不会改变 fallback Stable Type ID。

## Project 布局

```text
<Project>/
├─ Assets/
│  ├─ Scripts/**/*.cs
│  └─ **/*.iasmdef
├─ Plugins/
│  └─ *.iplugin
├─ Library/
│  ├─ AssetDatabase/
│  ├─ Plugins/<id>/<contentHash>/Assets/
│  ├─ Artifacts/
│  │  ├─ ab/cd/<asset-key>/...
│  │  └─ ScriptAssemblies/
│  │     ├─ .assemblies/<assembly-key>/...
│  │     └─ <generation-key>/
│  │        ├─ *.dll
│  │        ├─ *.pdb
│  │        ├─ *.xml
│  │        ├─ *.types.cache
│  │        └─ diagnostics.cache
│  ├─ ScriptApi/
│  └─ IDE/
├─ Inno.GameScripts.csproj
├─ Inno.EditorScripts.csproj
├─ <asmdef-name>.csproj
└─ InnoProject.sln
```

Project 根目录和 `InnoProject.sln` 只投影用户可编辑的 Project assembly。安装在 `Plugins` 中的代码由同一编译图构建并参与原子 generation，但其 source 与内部 assembly topology 不作为 Rider/IDE 项目暴露；`Inno.GameScripts`、`Inno.EditorScripts` 和 Project `.iasmdef` 工程引用 `Library/IDE/PluginReferences` 中按同一裁剪逻辑 API 从 Plugin source 生成的 metadata reference。这样 IDE 与运行时编译保留一致的逻辑类型身份，同时 Project 根目录不出现 Plugin 工程。每次成功激活都会删除历史遗留的 `Inno.Plugin.*.csproj` 与对应 API map。

`ScriptAssemblies` 不再出现 `1/2/3...` 数字 generation。每个 asmdef/builtin assembly 的 key 覆盖脚本内容 hash、规范化 asmdef 配置、scope/options、公开 Script API contract fingerprint、所属 Source Mount 及直接 dependency key；它不使用无关 Importer 实现 MVID。依赖 key 变化会自然传播到反向依赖，而无关 assembly 直接复用 `.assemblies` 中的 DLL/PDB/XML/type manifest/diagnostics。完整 generation key 组合有序 assembly key 与 Plugin content key，并在成功后一次性形成 load staging。Script assembly cache 使用 7 天 grace period 与 4 GiB 上限；`Library/ScriptApi` reference artifact cache 独立使用 7 天与 512 MiB 上限，并保护当前 Runtime/Editor contract 目录。

增量同时作用于 artifact 与 ALC closure。每个已激活 Plugin 使用独立 `Plugin.<id>` collectible ALC，并通过 manifest 依赖形成显式拓扑；Project Runtime 与 Editor Scripts 各使用一个 ALC。Editor-only 变化只替换 Editor；Runtime 变化替换 Runtime + Editor；某个 Plugin 变化只替换该 Plugin、依赖它的 Plugin 及 Project Scripting。未重编译 assembly 可以复用不可变字节产物，但每个被纳入 closure 的目标 ALC 都是新 generation，下游绑定同一事务中的精确上游 Assembly 实例。

ModuleHost 自己的 runtime generation 仍存在于 assembly shadow cache，用于区分 collectible ALC；它不写入 `.imeta`、Scene、Prefab 或 artifact identity。

## 请求编译与观察完成

Editor 的公开工作流入口是 `IEditorScriptCompilation`。内部 `EditorScripting` Module 拥有编译调度和激活流程；`ScriptCompiler`、`ScriptReloadHost` 分别负责产物编译与 generation publication，具体契约见 [Compiler](../scripting/Inno.Scripting.Compiler.md) 和 [Reload](../scripting/Inno.Scripting.Reload.md)。

`RequestCompilation()` 返回绑定到本次请求的 `IScriptCompilationTicket`。调用方保留票据，在后续 owner-thread 安全点观察 `isCompleted` 和 `state`；不能用另一个请求的成功结果启动 Play/Build，也不能在 UI 线程忙等待。

`Succeeded` 要求编译、激活及旧代际退休验证完成。失败、取消或被新请求替代不授予发布权限；Faulted Host 需要重启。

```csharp
using Inno.Editor.Scripting;

static IScriptCompilationTicket Request(IEditorScriptCompilation scripts)
{
    return scripts.RequestCompilation();
}

static bool CanContinue(IScriptCompilationTicket ticket)
{
    return ticket.isCompleted && ticket.state == ScriptCompilationTicketState.Succeeded;
}
```

菜单中的 `Scripting/Reload Scripts` 与 `Scripting/Reload Plugins` 由本项目的内部 Action 转给相同 owner。扩展不得自己建立第二套编译或卸载队列。关闭时先取消并排空编译，再完成退休，最后释放借用的资产与模块服务。

## 编译进度 modal

用户从菜单排队请求或文件观察器产生请求时，Editor 立即以 0% 打开阻塞式 Modal，与 Settings Modal 使用相同的后方交互禁用和 popup 规则。Modal 使用固定宽度，状态文字按可用 content width 自动换行，并提供 Cancel。后台编译、Editor frame 与进度绘制继续运行；阻塞的是后方窗口输入，而原子激活候选仍只发生在短 safe point。

进度由 project generation、source parse、API analysis、diagnostics、emit 和 reload preparation 等真实工作项推进。`ScriptCompilationResult.stageTimings` 按执行顺序保留每个完成阶段的 wall time。编译阶段占 0–80%，candidate staging 推进到 86%，Scene/extension 原子迁移推进到 94%，事务提交后进入 97% 的强制卸载验证，只有验证成功、验证确定失败或无需 reload 时才显示 100%。Roslyn 单次 Emit 没有内部百分比 callback，因此执行某个工作项时进度会停留，完成后跳到下一个比例；不会用计时器伪造连续进度。

成功 assembly artifact 同时保存完整的内部 `diagnostics.cache`。启动命中内容缓存时会重新发布同一组 warning，而不是只复用 DLL/PDB 后返回空 diagnostics；因此缓存命中与实际 Roslyn emit 对 Console Panel 具有一致的可观察结果。IDE 投影在 generation 激活之后独立执行，其失败只影响代码编辑体验，并通过 `Script IDE Projection` 诊断来源报告 Warning；它不会把已经成功的脚本候选伪装成编译或激活失败。

## Editor script readiness contract

`IEditorScriptCompilation` 是 host feature 使用的最小只读门禁；internal `EditorScripting` module 实现它，[Play Mode](Inno.Editor.PlayMode.md) 只依赖该公开接口与票据，不引用内部编译调度实现。

| 成员 | 语义 |
| --- | --- |
| `RequestCompilation()` | 为 Play/Export 等宿主工作流排队一次新的 cache-aware 编译。 |
| `state` | `Initializing`、`Compiling`、`Ready` 或 `Failed`。 |
| `status` | 当前 compiler/reload 阶段的可读说明。 |
| `lastCompilation` | 最近完成的 `ScriptCompilationResult`，首次完成前为 `null`。 |

`Ready` 表示最近编译成功、generation 已完成激活且旧代 GC 验证通过；编译、candidate activation 和旧 ALC unload verification 期间均为 `Compiling`。`IScriptCompilationTicket` 也必须等待上述完整过程，不能在 80% 编译完成或刚发布候选时提前成功。IDE 投影在屏障完成后生成，避免在 AwaitingCollection 期间申请新 read lease。即使其他 Host 边界先推进完成了屏障，下一次 Editor Update 仍会完成对应票据并关闭弹窗，且不依赖窗口焦点。

编译、激活或退休失败均为 `Failed`，新的 Play entry 不会静默使用过期脚本。退休失败由 Core Diagnostics 显式报告并关闭进度弹窗；Faulted 优先于 queued/compiling flags，遗留排队标记不能让 Editor 永远显示忙碌。Faulted Host 的新请求直接获得失败票据，需要重启，不能继续 reload。普通编译失败时 active generation 保持不变；Plugin availability 失败时 active generation 已明确退休不可成立的 module closure，场景状态以 Missing 保留。该 contract 是 Editor host API，不加入 EditorScripts 的逻辑 facade。

成功结果的 `runtimeAssemblyPaths` 是本 generation 中所有 runtime-scope main/preload assembly 的去重绝对路径。Game Export 只消费这组路径，因此不会凭输出目录猜测 DLL，也不会把 Editor assembly 部署进 Player；路径对应的 generation 必须在导出开始时仍是最新成功结果。

## Scripting API facade

脚本只能引用各项目 `Properties/ScriptingApi.cs` 明确导出的逻辑 API：

```csharp
using InnoEngine.Logging;
using InnoEngine.Reflection;
using InnoEngine.Scene;
using InnoEngine.Serialization;

[StableTypeId("c14f7138-0c5c-4e69-8376-cec8edc3056c")]
public sealed class PlayerController : GameBehavior
{
    [SerializableProperty]
    private float m_speed = 5f;

    protected override void Update()
    {
        Log.Debug("Speed: {0}", [m_speed]);
    }
}
```

- IDE facade 真正定义 `InnoEngine.*` / `InnoEditor.*` metadata 类型，并携带重写后的 XML documentation。
- IDE csproj 不引用真实 `Inno.*` 实现程序集，因此实现 namespace 无法解析。
- Runtime Roslyn 使用保持真实 CLR identity 的裁剪 reference set，并把逻辑 using 重写为声明的实现 namespace。
- Stub builder 会枚举每个 exported type 的全部 public/protected member；签名依赖未导出类型时构建直接失败，并报告完整 member 与缺失类型。只有明确的 host/native API 可以用 member 级 `[ScriptingApiIgnore]` 忽略；Runtime 与 IDE reference assemblies 共用同一闭包校验结果。
- 裁剪 reference 只保留其实现成员同样可见的接口；只由 private 显式成员实现的基础设施接口不会出现在 facade base list，因此不会泄漏 `EditorModule` 的 internal Dispose adapter，也不会产生缺失接口成员的 reference assembly。Module/Panel 项目状态直接由 protected `EditorState` hooks 提供，facade 不导出 Workspace interface、reader/writer 或 JSON DOM。
- 所有脚本文件必须显式声明自己使用的 `InnoEngine.*` / `InnoEditor.*` namespace。
- 编译范围导入、MSBuild `Using` item、隐式导入和 plugin metadata 注入均不受支持。
- Script API fingerprint 来自规范化 public/protected contract、逻辑 namespace/type mapping 与可附加类型身份；实现方法体或实现程序集 MVID 单独变化不会使 reference artifact 失效。XML documentation 与该 fingerprint 的不可变 reference artifact 一起生成并复用，不作为独立的二进制公开契约输入。
- `AssetPipelineOptions` 等 host initialization 类型不导出；脚本只看到查询、加载、Importer writer 与 Build Processor 所需契约。

Editor facade 按 feature 分布：

| Facade | 来源 |
| --- | --- |
| `InnoEditor.Core` | `Inno.Editor.Core` lifecycle contracts |
| `InnoEditor.Interactions` | `Inno.Editor.Interactions` Action、Menu、Selection 与 Drag/Drop contracts |
| `InnoEditor.Assets` | `Inno.Editor.Panel.FileBrowser` AssetEditor contracts |
| `InnoEditor.Scene` | `IEditorSceneWorkspace` 查询/工作流接口与 `SceneEdits` 编辑门面 |
| `InnoEditor.Hierarchy` | `Inno.Editor.Panel.Hierarchy` area/action/drop contracts |
| `InnoEditor.Inspection` | `Inno.Editor.Inspection` drawer contracts，以及 `Inno.Editor.Panel.Inspector` 的 area/action/drop contracts |
| `InnoEditor.ImGui` | `ImGuiIcon`、pointer-free `NativeImGui`、`EditorPalette`、`EditorStyleMetrics` 与 widgets |
| `InnoEditor.Settings` | `EditorSetting`、`ProjectSettingEditor<T>` 与对应 placement attributes |

菜单直接声明在 Action 上，不需要 package class 或集中注册：

```csharp
using InnoEngine.Core;
using InnoEditor.Interactions;

[EditorAction("project.items.select", "project/items")]
[EditorMenu("project/items", "Selection/Select Item")]
public sealed class SelectItemAction : EditorAction<IdentityObject>
{
    protected override void Execute(EditorActionContext<IdentityObject> context)
        => context.interactions.For(context.area, context.target).Select();
}
```

`StableTypeId` 不是普通脚本 Component/System 的必填项。编译器使用下列 canonical source 规则：

- 每个 `.cs` 的 `.imeta` persistent ID 是 source identity；
- 文件名与 `GameBehavior`/`GameSystem` 类型名匹配的文件是默认 canonical source；`*.editor.cs` 比较时会忽略 `.editor`；
- 单文件只有一个可附加类型时，即使文件名与类型名暂时不一致，也能无歧义继承该 source identity，并给出命名 warning；
- partial 类型只有 canonical 文件提供身份，其他 partial 文件只是 compilation input；
- 一个文件包含多个可附加类型时，只为同名主类型自动分配 source-based ID；其他类型给出 `INNO2001`，应拆文件或显式添加 `[StableTypeId]`；
- 显式 `[StableTypeId]` 始终优先，适合跨 source 移动、特殊多类型布局和手动迁移。

自动 Stable Type ID 由 canonical `.cs.imeta` persistent ID 确定性派生，因此移动文件、文件与类型一起改名、或在唯一 source 内改名都不会改变 Scene 中的类型身份。编译器只生成当前 canonical ID，不注册“程序集名 + 完整类型名”former alias。没有唯一 canonical source 的可附加类型会让编译失败，直到拆分文件或显式添加 `[StableTypeId]`。

## IDE 工程

Generator 从 Asset Catalog 与 asmdef graph 生成每个 assembly 的 SDK-style csproj：

- `EnableDefaultItems=false`；
- 只有明确 `Assets/**/*.cs` Compile item；
- `Library` 不显示为 source folder；
- Editor project 引用允许的 Runtime project；Plugin 依赖使用 IDE metadata reference；
- facade reference 绑定同 fingerprint XML documentation；
- `bin`/`obj`/analyzer/map 全部位于 `Library/IDE`。

IDE 工程是补全和诊断模型；Editor 实际热编译仍使用进程内 Roslyn 与同一 API/catalog 输入。

## Plugin

Plugin 只能来自项目根 `Plugins/*.iplugin`，并通过 `Plugin.inno`、archive 路径安全、依赖图与 `.imeta` 完整性校验，再进入隔离只读 `AssetSourceMount` 候选；编译与迁移全部成功后才进入统一 active Asset Database。Folder Plugin、`.zip` Plugin、预编译 DLL 和“路径里含 Plugins 就视为插件”的协议不存在。无 `.iasmdef` 时，每个 Plugin ID 自动获得 Runtime 与 Editor 默认程序集；`*.editor.cs` 进入 Editor scope，其余 `.cs` 进入 Runtime scope。显式 `.iasmdef` 必须列在清单 `assemblyDefinitions` 中。

Plugin 只能引用 Host API 与清单声明的依赖 Plugin，不能引用项目脚本；Runtime Scripts 能看到 runtime-scope Plugin，Editor Scripts 可看到 Runtime Scripts 与 Plugin Editor API。每个 Plugin ID 对应独立 `Plugin.<id>` collectible ALC，清单依赖成为 `upstreamModuleNames`，更新/移除只影响反向依赖 closure。错误方向、scope 泄漏、程序集名称冲突、缺失依赖、cycle 或 Roslyn 编译错误会在 stage/publish 前失败；Editor host 会丢弃隔离候选，旧模块 generation、Mount、Catalog、Type/Registry、资产和设置保持活动。ALC 是卸载隔离，不是安全沙箱；把带代码容器放入 `Plugins/` 就表示允许它以项目脚本相同的本机权限执行。

## Reload coordination 与领域状态

成功 build 通过一个多模块 `ModuleHost.BeginReload` 准备完整候选 Assembly catalog、TypeCache 与 Registry。Scripting 随后只把 prepared session 交给 [Inno.Editor.Core](Inno.Editor.Core.md) 的 `EditorReloadCoordinator`；它不知道 Scene、Component/System、Missing 或任何 Panel。Scene 等独立 feature 通过 Core 的中立 participant contract 捕获自己的 generation-bound live state。旧 extension 的 Stop/Detach、交互瞬态清理、候选 Start/Attach 与各 feature 状态恢复都在发布回调返回前完成；全部迁移成功才进入 cleanup-only Complete。

内部 safe-point apply 在成功前只读取 candidate，不提前消费。Plugin/Runtime/Editor 任一 stage、Registry、Asset rescan、Scene restore 或 extension activation 失败都逆序恢复 Scene、extensions、TypeCache、Registry 与全部 previous modules，不发布部分 generation；同一 pending candidate 保留以便显式重试，后续成功编译可原子替换它。

Serialized state 使用逐成员兼容迁移：成员类型保持兼容时恢复旧值；新增成员保留新默认值；删除成员忽略旧数据；同名成员改成不兼容类型时保留新默认值并输出 `INNOHR0001` warning，不会仅因单个字段变化回滚整个程序集。普通 Serialization API 仍保持严格模式。

失败时所有已捕获 feature transaction 和 Assembly transaction 一起反向 rollback。Scene feature 自己实现 Scene migration、Coroutine owner 停止与 Scene diagnostics；Scripting 不调用这些领域 API。Scene 的 Edit Mode migration 不触发生命周期；Runtime reload 会对旧 active instance 调用一次 `OnDisable`，新 instance 在下一次正常 update 调用 `OnEnable`，不会重复 `Awake`/`Start`，也不会调用 `Reset`/`OnDestroy`。

如果候选 generation 缺少 live Component/System，Scene participant 会把对象变为 host-owned missing placeholder，并继续保存 `TypeRef`（落盘仅原逻辑 Stable ID）、原类型名、property bytes、asset dependencies、persistent ID、顺序和图引用。placeholder 身份和 missing 标志不进入 Scene schema，类型消失/恢复本身不改变 clean Scene 的 dirty 状态。类型删除、插件移除、Scene/Prefab round-trip 都不会持有旧 ALC；相同 Stable ID 返回时 `isValid` 自动恢复并原位重建。`INNOHR0002` 完全由 Scene feature 按当前 loaded Scene 与 TypeCache generation 对账：打开带 Missing 的 `.iscene` 后，在 Scene workspace 下一次安全更新就会出现在 Console，不需要等待 Recompile/Reload。每次协调 reload 完成还会请求各领域刷新诊断，因此无变化 Recompile 后仍会重新发布用户 Clear 掉但依然成立的 Missing；类型恢复后该组立即解除。恢复构造、identity 转移、property restore 或失败清理不完整时仍精确回滚到原占位对象。

无法自动迁移 static 字段、后台 Thread/Task、第三方事件订阅或外部裸 CLR 引用。用户代码需要在 `OnDisable` 释放这些资源。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Editor.Scripting.EditorScriptCompilationState`

<!-- api: T:Inno.Editor.Scripting.EditorScriptCompilationState 44361a7f85a4918a8d7e4bbed353a55b966c0d0fcb92f6b362793bb33b882378 -->
<!-- api: F:Inno.Editor.Scripting.EditorScriptCompilationState.Initializing 82439dbf7d096856fb84955a9095b751ce885c57ea159f4f1be1f226b55362ac -->
<!-- api: F:Inno.Editor.Scripting.EditorScriptCompilationState.Compiling 0b0c8c8946a1535b4dc7d1b5d49636be972abdcf9e2915cf9a1581411c2db5ba -->
<!-- api: F:Inno.Editor.Scripting.EditorScriptCompilationState.Ready fd31501237b10282d5e16ad4c449838e3117c865b3e5635cbb7d747ada9e6450 -->
<!-- api: F:Inno.Editor.Scripting.EditorScriptCompilationState.Failed 743fce31ecb80b65c373f9ce4af2110584ffee80202b3ce9969e5c797903f379 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5d2daed1bd593977"></a>[`Inno.Editor.Scripting.EditorScriptCompilationState.Compiling = 1`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IEditorScriptCompilation.cs#L18) | A requested script generation is compiling, activating, or completing unload verification. | — | — |
| <a id="api-a257dd0ce2c960c1"></a>[`Inno.Editor.Scripting.EditorScriptCompilationState.Failed = 3`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IEditorScriptCompilation.cs#L28) | Compilation, activation, or generation retirement failed, so game simulation must not start. | — | — |
| <a id="api-056ac92f3525aeda"></a>[`Inno.Editor.Scripting.EditorScriptCompilationState.Initializing = 0`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IEditorScriptCompilation.cs#L13) | The script service has not produced its initial active generation. | — | — |
| <a id="api-e49210b63225555c"></a>[`Inno.Editor.Scripting.EditorScriptCompilationState.Ready = 2`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IEditorScriptCompilation.cs#L23) | The most recent compilation succeeded, its generation is active, and retired generations passed GC verification. | — | — |
| <a id="api-f2c7e3b6b3a0b643"></a>[`public enum Inno.Editor.Scripting.EditorScriptCompilationState : int`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IEditorScriptCompilation.cs#L8) | Identifies whether the active project script generation can safely start game simulation. | — | — |

### `Inno.Editor.Scripting.IEditorScriptCompilation`

<!-- api: T:Inno.Editor.Scripting.IEditorScriptCompilation 4954c0d92fba082b7d26326ef71012cda046f90c2d62a186ab0e1041d4f5d682 -->
<!-- api: M:Inno.Editor.Scripting.IEditorScriptCompilation.RequestCompilation 779431968580637699439201ee00136d7a649df39162d204757b6bb78ea4538f -->
<!-- api: P:Inno.Editor.Scripting.IEditorScriptCompilation.currentTicket fc7171085e0c20e515a8808d38f9083b59f28ec28265e4e9b81436c7a37a2d5b -->
<!-- api: P:Inno.Editor.Scripting.IEditorScriptCompilation.state d97c2e8c0b62b8a065673682bf40e75bf158ab2a3e6d367d56e60af7a51ad80c -->
<!-- api: P:Inno.Editor.Scripting.IEditorScriptCompilation.status 2ef4995e39c7e97bd4cccac3b6db37b896635758e44df8ac7b435a88a63d4110 -->
<!-- api: P:Inno.Editor.Scripting.IEditorScriptCompilation.lastCompilation 88ec836faafa1bb3840f16681c566511683a2372738fa2c1757bb12d2a03de24 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d4347e54c4200dbb"></a>[`Inno.Editor.Scripting.IScriptCompilationTicket Inno.Editor.Scripting.IEditorScriptCompilation.RequestCompilation()`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IEditorScriptCompilation.cs#L43) | Queues a fresh cache-aware compilation for a build or simulation workflow.<br>返回：A ticket bound to the exact request. Success requires activation and completed retirement verification; a faulted host returns a failed ticket and requires restart. | — | — |
| <a id="api-b43f410406dc5df8"></a>[`Inno.Editor.Scripting.IScriptCompilationTicket? Inno.Editor.Scripting.IEditorScriptCompilation.currentTicket { get; }`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IEditorScriptCompilation.cs#L48) | Gets the most recently requested compilation ticket, or null when no explicit request exists. | — | — |
| <a id="api-fa6ff992cca36e38"></a>[`Inno.Scripting.Compiler.ScriptCompilationResult? Inno.Editor.Scripting.IEditorScriptCompilation.lastCompilation { get; }`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IEditorScriptCompilation.cs#L63) | Gets the most recently completed compilation, or null before the first attempt completes. | — | — |
| <a id="api-b28fe0d2d08348e4"></a>[`Inno.Editor.Scripting.EditorScriptCompilationState Inno.Editor.Scripting.IEditorScriptCompilation.state { get; }`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IEditorScriptCompilation.cs#L53) | Gets the current script-generation readiness state. | — | — |
| <a id="api-9fa59b7c8c1112dd"></a>[`string Inno.Editor.Scripting.IEditorScriptCompilation.status { get; }`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IEditorScriptCompilation.cs#L58) | Gets a human-readable description of current compiler work or readiness. | — | — |
| <a id="api-ee8f2866ab9a80d8"></a>[`public interface Inno.Editor.Scripting.IEditorScriptCompilation`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IEditorScriptCompilation.cs#L34) | Exposes the minimal script-generation readiness contract required by editor workflows. | — | — |

### `Inno.Editor.Scripting.IScriptCompilationTicket`

<!-- api: T:Inno.Editor.Scripting.IScriptCompilationTicket 12629f8643fa4680f7e3c235afd9a8fc31838601ace932b9069c2d4f56c61ad8 -->
<!-- api: P:Inno.Editor.Scripting.IScriptCompilationTicket.requestId 509b9443e780446ee84d4f9a1e62ef62f6c9735d9d9b9dc6d8814679406ce83e -->
<!-- api: P:Inno.Editor.Scripting.IScriptCompilationTicket.state b9e39cd7ac7c9e5764f69bcb31d94f96d8c210cd5612e673a5e857917501fd24 -->
<!-- api: P:Inno.Editor.Scripting.IScriptCompilationTicket.status ff40ce29ece35ade5e9d116da37c53bda967fea347dd1576e60304dcf0120b78 -->
<!-- api: P:Inno.Editor.Scripting.IScriptCompilationTicket.result d142d34b06e318e2f5e3605737bbdceb8f145045af985521fcf1bb9511d67e5c -->
<!-- api: P:Inno.Editor.Scripting.IScriptCompilationTicket.isCompleted 9f3c8252c4af28400802981ebbf58e2cba61c150f0db55f07ddf982594cc25a2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7b6f07239d6a5d4e"></a>[`bool Inno.Editor.Scripting.IScriptCompilationTicket.isCompleted { get; }`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IScriptCompilationTicket.cs#L33) | Gets whether no further state transition is valid for this request. | — | — |
| <a id="api-1d04ab49d4ec5331"></a>[`long Inno.Editor.Scripting.IScriptCompilationTicket.requestId { get; }`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IScriptCompilationTicket.cs#L13) | Gets the producer-local monotonic identifier of this request. | — | — |
| <a id="api-2c573e64fee8ecb9"></a>[`Inno.Scripting.Compiler.ScriptCompilationResult? Inno.Editor.Scripting.IScriptCompilationTicket.result { get; }`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IScriptCompilationTicket.cs#L28) | Gets the completed compiler result, or null before the compiler finishes. | — | — |
| <a id="api-ed20412afaf9e932"></a>[`Inno.Editor.Scripting.ScriptCompilationTicketState Inno.Editor.Scripting.IScriptCompilationTicket.state { get; }`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IScriptCompilationTicket.cs#L18) | Gets the current lifecycle state of this exact request. | — | — |
| <a id="api-6db681e389d4203f"></a>[`string Inno.Editor.Scripting.IScriptCompilationTicket.status { get; }`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IScriptCompilationTicket.cs#L23) | Gets a human-readable description of the current or terminal state. | — | — |
| <a id="api-4ef88a7d70746b4c"></a>[`public interface Inno.Editor.Scripting.IScriptCompilationTicket`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/IScriptCompilationTicket.cs#L8) | Exposes the immutable observer view of one exact script compilation and activation request. | — | — |

### `Inno.Editor.Scripting.ScriptCompilationTicketState`

<!-- api: T:Inno.Editor.Scripting.ScriptCompilationTicketState 5d506516b116507a41402c5d921316f0540fbe7986052e1de60ad614e5c328c3 -->
<!-- api: F:Inno.Editor.Scripting.ScriptCompilationTicketState.Queued ad9899dc3069abb0a933f10666796ad74d38b7703f8eee3738067b8a617e6074 -->
<!-- api: F:Inno.Editor.Scripting.ScriptCompilationTicketState.Compiling 896739a9b90d56c6f816d7a0bb8353aac18c1799982dd5b0801fa016b90ec3bc -->
<!-- api: F:Inno.Editor.Scripting.ScriptCompilationTicketState.Succeeded 88b0bc4daa3296076d7d31a02432c64d14a9b353a46b1c5e2f239b0e530f2483 -->
<!-- api: F:Inno.Editor.Scripting.ScriptCompilationTicketState.Failed 058f270dd26c819056b5ab09b8c0988a98861dd67179b0405b89d5d7fbfc722a -->
<!-- api: F:Inno.Editor.Scripting.ScriptCompilationTicketState.Canceled c1ad2118ef9842d0506f6d11f97e698e06d6601c6c775fd9db42181bdd1696a1 -->
<!-- api: F:Inno.Editor.Scripting.ScriptCompilationTicketState.Superseded 33deb502edd819a9101faed03bc9f500bdec85ae53158568b07eb962114bd67f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-23662c5f8bb587c7"></a>[`Inno.Editor.Scripting.ScriptCompilationTicketState.Canceled = 4`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/ScriptCompilationTicketState.cs#L31) | The request was explicitly canceled before activation. | — | — |
| <a id="api-e669239421f6f53f"></a>[`Inno.Editor.Scripting.ScriptCompilationTicketState.Compiling = 1`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/ScriptCompilationTicketState.cs#L16) | The request owns the compiler pipeline or is activating its candidate generation. | — | — |
| <a id="api-373348395de43d26"></a>[`Inno.Editor.Scripting.ScriptCompilationTicketState.Failed = 3`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/ScriptCompilationTicketState.cs#L26) | Compilation or candidate activation failed without replacing the active generation. | — | — |
| <a id="api-243f57a0cd9223de"></a>[`Inno.Editor.Scripting.ScriptCompilationTicketState.Queued = 0`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/ScriptCompilationTicketState.cs#L11) | The request is queued behind the active compiler operation. | — | — |
| <a id="api-5e6c53a7681ebedb"></a>[`Inno.Editor.Scripting.ScriptCompilationTicketState.Succeeded = 2`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/ScriptCompilationTicketState.cs#L21) | The request compiled and atomically activated its candidate generation. | — | — |
| <a id="api-a6d57635bb4929e8"></a>[`Inno.Editor.Scripting.ScriptCompilationTicketState.Superseded = 5`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/ScriptCompilationTicketState.cs#L36) | A newer request replaced this request before its candidate could activate. | — | — |
| <a id="api-e457e1b326a53818"></a>[`public enum Inno.Editor.Scripting.ScriptCompilationTicketState : int`](../../src/composition/editor/features/Inno.Editor.Scripting/Runtime/ScriptCompilationTicketState.cs#L6) | Identifies the lifecycle state of one exact script compilation request. | — | — |

## 项目依赖

- [Inno.Editor.ImGui](Inno.Editor.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Interactions](Inno.Editor.Interactions.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Plugins.Authoring](../plugins/Inno.Plugins.Authoring.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Reload](../scripting/Inno.Scripting.Reload.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Native.ImGui](../backends/ImGui/Inno.Native.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Core](Inno.Editor.Core.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Compiler](../scripting/Inno.Scripting.Compiler.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
