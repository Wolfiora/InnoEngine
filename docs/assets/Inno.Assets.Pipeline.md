# Inno.Assets.Pipeline

同步 Pipeline 修改与 loader 操作通过共享 `GenerationCoordinator.AcquireOperation` 保护完整调用范围。
Pipeline 在捕获 loader 前对账 dirty Catalog；进入操作后 Type/Serializer 查询引发的自动刷新只能延后，
不能在当前 loader 的调用栈中退休它自身。候选发布仅在自己的控制线程借用该保护，不放宽 GC 或 Faulted gate。
Catalog 文件提升在 journal 删除失败时先恢复旧 snapshot；外层 metadata stage 再逆序恢复已写 sidecar。
恢复失败聚合上抛并由 publication owner Fault，不能宣称成功。文件系统多文件操作仍是带补偿的安全点提交，
不是跨文件 crash-atomic 事务。

## Missing 与只读失败记录

Asset reference 和 dependency record 只按 persistent ID 解析；last-known path 仅作诊断。旧路径出现新 identity 时原引用保持 Missing。

Tombstone 保留中立 property bytes、依赖描述与 last-successful artifact key；释放 live payload/强引用，不将旧 artifact 视为当前可加载内容。Tombstone 的旧 key 不在活动 path 集合中，因此仍允许正常 CAS 回收；保留 key 不等于永久保留缓存文件。

只读源 metadata 不一致时，失败进入可写 Catalog；记录失败不会再次调用只读 source writer，也不会修改安装包。缺失 sidecar 或 identity 冲突仍明确拒绝，不提供 legacy metadata fallback。Source mount dependency set 和发布的 mount 列表为不可修改快照。

文件占用或暂时的访问拒绝引起的导入失败会在下次 Rescan 时重新尝试，即使源码和 Importer 未变化。其他确定性的导入失败仍按源码、设置、依赖或 Importer 变化触发重导入，避免无意义的重复工作。

[Assets 索引](README.md) · [Runtime Assets](Inno.Assets.md) · [Plugins](../plugins/Inno.Plugins.Authoring.md)

## 职责与边界

该 authoring project 拥有 Source Mount、文件索引/watcher、Importer、Build Processor、`.imeta`、依赖图、Catalog、CAS Artifact、canonical authoring object 和 runtime closure export。Player 不引用它。

## 命名产物与发布范围

`AssetImportWriter<T>.WriteArtifactAsync` 和 `AssetArtifactWriter.WriteAsync` 接收可选的 `deploymentScope`，默认 `Runtime`；
创作期缓存可明确标为 `AuthoringOnly`。保留的 `runtime` / `asset-state` 槽不可标为创作期输出。
部署范围写入不可变 manifest，并参与内容指纹。`AssetImportContext.AcquireArtifact(id, output)` 声明 artifact 依赖并返回 lease；
`AssetExportContext.artifacts` 提供当前导出 owner 的显式查询，调用方不得直接绕过 lease 读取任意旧缓存。

Runtime 导出不复制整个创作期 bundle：校验各输出的相对文件名、长度和 SHA-256，只投影 Runtime 输出到新的 CAS key，
通过 owner serializer 克隆并重写部署 Catalog/metadata 的 artifact key，清除仅用于重新导入的依赖记录。
源 Catalog 和 sidecar 不变。该协议不认识 Shader、Sprite、字体或其他具体资产类型。

## 公开 API

| 分组 | 主要 API |
| --- | --- |
| Composition | `AssetPipeline`, `AssetPipelineOptions`, `AssetPipelineMode`, `AssetSourcePolicy`, `AssetCacheOptions` |
| Source | `IAssetSourceSnapshot`, `AssetSampleImportTransaction`, `AssetSourceMount`, `AssetSourceMountTransaction`, `AssetFileSystem`, `AssetFileEntry`, `AssetSample`, `AssetSampleTransformContext`, `IAssetSampleSourceRewriter`, `AssetSampleSourceRewriterAttribute`, `AssetChangedEvent` |
| Import | `AssetImporter`, `AssetImporter<T>`, `AssetImportContext`, `AssetImportWriter<T>`, `AssetImportHealthSnapshot`, `AssetImportFailure` |
| Import settings | `AssetImportSettingsSnapshot`、`AssetImporter.CreateImportSettings()`、`AssetImportContext.importSettings`、Pipeline/Loader 的 `GetImportSettings` 与 `SaveImportSettings` |
| Build | `AssetBuildProcessor`, `AssetBuildProcessor<T>`, `AssetBuildContext<T>`, `AssetArtifactWriter` |
| Transactions/export | `AssetCatalogCandidate`, `AssetExportContext`, `AssetSerializationServices`, `AssetDeploymentScope`, `NativeAssetSourceSerialization` |
| Script authoring | `EditorAssets` 提供当前 Editor Session 中显式受限的保存入口；不暴露 Pipeline owner 或 mutation graph。 |
| Advanced facade | `AssetLoader`，用于独立 authoring host；普通 Application 优先使用 `AssetPipeline` |

Plugin Importer 继承 `InnoEditor.Assets` 中的 Importer base，并使用 Context、Writer 与
`NativeAssetSourceSerialization`。原生结构化 Source helper 只接收 Context 提供的
`AssetSerializationServices`，不会向 Plugin 泄漏 `TypeCatalog`、`SerializationRegistry` 或候选
Asset resolver。Importer 源码属于 Plugin 的 Editor assembly，不能进入 Runtime Plugin assembly。

## 组合与生命周期

```csharp
using var assets = new AssetPipeline(
    modules,
    types,
    serialization,
    identities,
    diagnostics,
    logs,
    AssetPipelineOptions.Create(assetRoot, libraryRoot));

assets.Update();
TextAsset value = assets.Load<TextAsset>(AssetPath.Project("Config/value.txt"));
```

所有 mutation 必须在构造线程执行。Save、Import、Sample Commit、Move、Delete、CreateDirectory 和 source candidate commit 各自发布一个 revision；后台 `ExportRuntimeArtifactsAsync` 使用 owner thread 捕获的 immutable Serialization generation，并在 worker 完成、失败或取消之前持续持有严格的 generation read lease。不能在提交 Task 后提前释放租约；Pending/Faulted generation 不允许开始导出。

Artifact key 使用完整的 64 位十六进制 SHA-256；空值表示尚未分配，其他不完整或含路径字符的值在构造时失败。
缓存 manifest 的输出名称和文件名必须唯一，文件名必须为 bundle 内的叶文件名，内容指纹和长度必须有效。
`TryGetArtifact` 检查 manifest 和文件长度；读取 payload 与导出时检查实际 SHA-256。
损坏缓存报告 `InvalidDataException`，不会返回未经验证的 payload 或把损坏产物写入 Player。

`Save(path, detachedAsset)` 替换已有 source 内容时以目标 `.imeta` / Catalog 的 persistent ID 为权威，并原位更新已加载的 canonical asset；草稿对象自身的临时 identity 不会把同一路径保存成一个新资产。因此 Scene、Camera、Material 等现有引用在 Inspector 保存后仍指向同一个资产。只有目标路径尚未拥有 identity 时，保存才采用待保存对象的 identity 或创建新的 identity。

Authoring 启动与 Rescan 时，当前 source 的 `.imeta` 是“路径属于哪个 persistent ID”的唯一权威；Catalog 是可重建的索引与 artifact cache。若 Catalog 在同一路径保留历史 live ID，而 sidecar 已声明另一个 ID，Loader 会把历史记录退休为 tombstone，并以 sidecar ID 建立当前记录；合并 tombstone 时也只移除它自己拥有的 path mapping，不能误删后来建立的 live 记录。这样 Save、崩溃恢复、候选 Catalog 提升或历史重复记录都不会在下一次启动反向改写 source ID，已保存的 Camera/Material/Scene 引用也不会因启动顺序变成 Missing。

运行时导出同时校验创作依赖：沿 Artifact 依赖递归检查当前导入状态和源指纹（包括 include/Source 输入）。Editor 可以继续使用 last-good，但失败、缺失或过期的必需创作输入不能认证 Player 构建。导出不在中途重导入，以免已编译的目标产物与新资产混用 generation；拒绝时报告完整依赖路径，需重导入后重新构建。

### Source Mount 候选与共同 Recovery

`PrepareSourceMounts` 返回未发布的 `AssetSourceMountTransaction`。`sourceMounts`、
`GetFileSystemEntries`、`TryGetInfo`、`TryGetArtifact` 和 `Load<TAsset>` 只查询候选。
候选 canonical object 已有 persistent ID，但激活前没有 runtime ID，不能占用活动对象的 Identity 注册。

`Activate` 使用共享 `ReferenceRecoveryTransaction` 发布新 loader/文件索引的 Identity，
随后解析先前已加载资产与 tombstone 的中立引用槽。`recoveryChanges` 是只读、无对象强引用的
`ReferenceRecoveryChange` 列表：激活前/回滚后为空，激活后提供真实 Resolved/Missing 结果。
这个入口只供 Host 诊断与事务组合，不导出到游戏脚本。损坏的 Imported canonical recovery 拒绝候选；
真正暂缺的资产保留同一 ID、type、路径提示与 property bytes。原 source 和原 `.imeta` 返回后可恢复；
同路径新建文件但没有原 metadata 不视为同一资产。
提交 Missing 结果时，候选 Loader 保存这些恢复槽的中立数据；它们不依赖 Missing 占位对象的弱引用存活，
因此 GC 或下一次 Source Mount 切换不会丢失先前已加载资产的恢复意图。未曾成为活动引用根的普通
Catalog tombstone 不会仅因存在于索引中而生成新的恢复槽。

`Complete` 提升 catalog 并退休旧 loader；`Rollback` 恢复未被修改的旧 canonical object，再退休候选。
两者均复用 Core `LifetimeScope` 与 `RetirementBarrier`，退出真正完成前不清空候选或旧 owner。
短暂 Pending 在 owner 线程驱动至完成；超时保留两代所有权并 Fault 共享 generation gate，
后续访问与依赖销毁均拒绝，必须重启 Host。普通退休错误仍尝试其余可释放资源并明确报告。

独立使用 `AssetLoader.Dispose` 时，它只报告 Pending，不同步阻塞正在执行/排队的操作；
Host 应保留 loader 并在安全点重试。停止接收新工作后，已准入操作仍可完成；
canonical 卸载 hook、Identity 注册、registry 和诊断按依赖顺序退休，包含仅留在 ID 索引中的 tombstone。

Assembly Catalog 更换 Importer/type 时也复用这一隔离候选：Prepare 冻结原有可写 import failure 指纹，
Activate 在候选 Type/Serializer 生效后只刷新候选 loader、校验新增失败，再发布其 Identity/recovery。
失败直接恢复未修改的旧 loader，不再标记“下次访问重新 Rescan”。首次注册 participant 不重复发布已初始化的源。
如果 Plugin 编译已准备 Source Mount candidate，则程序集事务只加入该候选进行验证，不创建第二份候选，也不
抢占 Plugin 原有的 Activate/Complete/Rollback 所有权。Settings/Graph/Plugin availability 全域 Recovery 仍需继续迁移。

候选的 `.imeta` 读写进入内部 `AssetSourceMetadataStage`，包含创建、目录 metadata、重定位和删除；
Prepare/Activate 不改创作源 sidecar。Complete 先验证所有已读取 metadata 的原始 bytes 是否仍匹配，
再写入差异并提升 Catalog；其中任何一步失败会逆序恢复已经写入的 sidecar，保留两种错误并 Fault。
外部修改冲突不会覆盖外部新数据。未提交候选不能 Save 创作源；正常已提交 loader 仍使用原本的可写 API。
这提供 owner-safe-point 事务及失败补偿，不声称文件系统支持跨多个文件的单条原子指令。

候选的 import/build/reference/catalog 诊断只暂存中立数据，激活时才创建 Core `DiagnosticReporter`；
停用时撤销当前 reporter，但保留回滚所需的中立报告。旧代 Dispose 不清除新代报告，回滚会恢复旧未解决诊断。
领域并未新增第二个 DiagnosticHub 或 sink。

## 通用 Import Settings

Importer 可以重写 `CreateImportSettings()`，每次返回新的 `ISerializable` 对象；该类型必须注册
`StableTypeId`，字段使用共同的 `SerializableProperty`。无设置时返回 null。设置不属于运行时 Asset
属性，也不进入 runtime closure；它们通过现有 `.imeta.importerSettingsBytes` 保存稳定类型 ID、
共同序列化的 property bytes 与依赖描述，不新增 JSON、配置资产或 Shader 特例。

`AssetPipeline` 和独立 `AssetLoader` 具有相同接口：

```csharp
AssetImportSettingsSnapshot snapshot = assets.GetImportSettings(path);
// Edit the detached snapshot.value with the registered inspector.
bool imported = assets.SaveImportSettings(path, snapshot.value, snapshot.fingerprint);
```

- `GetImportSettings` 不写盘，返回当前代际的独立 `value` 与 sidecar 的 `fingerprint`。
  调用者不能跨 reload 保留这个对象；长期编辑/历史只保留稳定身份和中立 bytes。
- `SaveImportSettings(path, value, expectedFingerprint)` 拒绝类型不匹配和外部冲突，原子写入一个
  sidecar 后尝试导入。传 null 表示重置为 importer 默认值。返回 false 表示**设置已经保存，但导入失败**；
  last-good artifact 和当前诊断保持分离。保存失败抛异常，不静默覆盖外部设置。
- Importer 从 `AssetImportContext.importSettings` 取得候选代际恢复后的值。导入期间改动这个对象不写回设置。
- 导入失败记录若再次读取设置也失败，会在同一导入诊断中附上该次设置检查错误；不会把设置读取失败静默记录为空 hash 并伪装为完整诊断。
- 设置中的 Asset 引用以 persistent ID 恢复，声明为 source/artifact **导入依赖**，不会仅因出现在设置中
  而成为 runtime dependency。跨 mount 引用沿用现有权限校验。临时缺失的引用仍保留原 identity。
- 设置内容参与 artifact fingerprint；移动文件身份不变，重建 Library 仍从 `.imeta` 恢复设置。
  Importer 执行期间 sidecar 内容变化会拒绝提交，不能把旧设置生成的结果标为当前结果。
- Watcher 会发布 `.imeta` 变化供 Loader 检查失效；File Browser 仍隐藏这些 sidecar，`.abin` 仍被过滤。
  自身产生但语义不变的 sidecar 通知不会重复导入；损坏的 sidecar 不会被失败记录擦除。
- 只读 mount 可读取设置，但不能保存；隔离的 Asset candidate 也不能编辑创作源。所有 Pipeline 操作
  都在 owner thread、共同 generation operation scope 内执行。现阶段尚未添加专门的设置 Inspector UI。

## `~` 开发目录与安装态 `.isample`

Project Source Mount 中，名称以 `~` 开头的目录在 File Browser 中显示为 `ISAMPLE`，但仍使用普通创作语义：Asset Import、Catalog、Artifact、authoring 脚本编译、Editor 运行和 Play Mode 都会正常处理，完整 Project 导出为 `.iplugin` 时也会携带这些源文件。`AssetSample.HasSampleDirectoryName(path)` 只表达这个与 Source 无关的命名/显示分类，不表示该目录需要导入。`AssetSample.IsRuntimeExcluded(path, isDirectory)` 则统一表达 deployment 边界：Game 的 runtime Asset 与 runtime script closure 始终剔除任何 Source 下的 `~` 子树；普通 runtime Asset 若依赖其中内容，导出会因闭包不完整而明确失败，Startup Scene 位于其中时也会被明确拒绝。

只读 Plugin Source Mount 中，名称以 `~` 开头的目录才是逻辑 `.isample`。`AssetFileSystem` 索引目录及后代，`AssetFileEntry.isSample` 标记目录本身，`isSampleContent` 标记完整子树；Editor 可以直接打开其中的场景并进入 Play。样例脚本属于独立的作者端程序集，不进入插件运行程序集或 Player 闭包。

`AssetPipeline.PrepareSampleImport(source)` 返回 `AssetSampleImportTransaction`，后台复制并重写私有 stage；目标是 `Assets/<原始~目录名>/`，完整保留全部前导 `~`，不添加 Plugin ID。Project 副本正常参与 authoring 编译与 Play，仍按共同规则从 Player 的 `~` runtime closure 中排除。

每份 `.imeta` 获得新资产身份，结构化源数据和 sidecar 内嵌的 importer settings 同时按完整身份映射重写。源语言扩展仍通过 `IAssetSampleSourceRewriter` 与带稳定 ID 的 Attribute 发现；它在后台只操作 transaction-owned stage，必须检查 `AssetSampleTransformContext.cancellationToken`，不得访问 live Assets、Editor 或其他线程所属的 native 状态。C# 实现位于 Scripting Compiler，不把 Roslyn 引入资产层。后台持有共享 generation read lease 和冻结的 Serialization generation，取消后也不会提前释放。

`Advance()` 在复制完成后由 owner 捕获恢复状态并移入候选目录，后台准备隔离 Catalog、导入已识别资产并刷新索引；完成后在 owner thread 采用现有 `AssetSourceMountTransaction`。校验期间活动 Loader、File Browser 索引和 Identity domain 保持原状；候选仅通过只读 `IAssetSourceSnapshot` 供编译输入捕获。`BeginValidation` 在 owner thread 调用 validator，允许它先捕获输入再异步计算；其 continuation 不得修改 live Assets。未完成任务不会在 Editor 帧内同步等待。复制、哈希、身份重写、候选资产导入与 History archive 均在后台执行；owner 只捕获当前恢复状态、采用候选、记录 History 和最终发布/退休。这些共同事务安全点仍有工作量，不承诺任意用户扩展或全量状态捕获的固定帧耗时。

| 入口 | 当前契约 |
| --- | --- |
| `PrepareSampleImport(source)` | 开始唯一 Sample 事务；拒绝目标冲突、其他源候选、Pending/Faulted generation。 |
| `AssetSampleImportTransaction.target` | 保留原名的 Project 目标路径。 |
| `Advance()` | 后台未完成返回 false；完成后采用隔离候选，出错保留事务供 Rollback。 |
| `BeginValidation(validate)` | 恰好一次 preflight；传入只读候选快照和共享取消 token。 |
| `isValidationComplete` | 表示工作已 drain，不表示验证成功。 |
| `Commit(beforePublish)` | 重抛验证错误；成功时在一个 owner safe point 激活候选、完成可选 History finalization、提交与退休，最后发布一次 Changed。 |
| `Cancel()` | 请求取消，保留全部资源。 |
| `Rollback()` / `Dispose()` | drain 后撤销未完成候选并移走副本；大目录清理在独立受控后台阶段完成，Pending 保留 owner 并允许重试。 |
| `isFaulted` | publication、rollback 或退休期限失败后要求完整重启 Host。 |
| `IAssetSourceSnapshot` | `sourceMounts`、`GetFileSystemEntries`、`Load<TAsset>`、`TryGetInfo`、`TryGetArtifact`；活动 Pipeline 与源候选共用的只读输入边界，不提供发布操作。 |

`AssetLoader.Rescan(cancellationToken = default)` 把取消传递到本轮扫描和 Importer。取消候选扫描会在退出前报告取消，由所属事务退休未发布的 Catalog；调用方不得把已取消的候选继续当作可发布快照。

Frame owner 的典型流程：

```csharp
AssetSampleImportTransaction import = assets.PrepareSampleImport(samplePath);
// On later owner-thread frames, call Advance until it returns true.
if (import.Advance())
{
    import.BeginValidation(async (
        sources,
        cancellationToken
    ) =>
    {
        ScriptCompilationResult result = await compiler.CompileAuthoringGenerationAsync(
            cancellationToken: cancellationToken, sourceSnapshot: sources).ConfigureAwait(false);
        if (!result.success)
            throw new InvalidOperationException("Sample validation failed.");
    });
}
// On a later owner-thread frame, after isValidationComplete, call Commit.
// On cancellation or failure, retry Rollback while RetirementPendingException is reported.
```

`.abin` 与 source noise 不复制；符号链接、复制期间文件变化、重写错误或 preflight 失败不发布半个目录。`AssetSample.GetImportName(source)` 可预先得到目标名。Watcher 恢复后强制全量对账，避免暂停窗口中外部源变化被丢弃。停止与取消复用 `LifetimeScope` / `RetirementBarrier`，三十秒退休 deadline 后明确 Fault，绝不清空尚未 drain 的任务。这里是 owner-safe-point 事务与文件系统补偿，不是跨多文件 crash-atomic 操作。
完整源对账或恢复 rescan 必须同时刷新 Catalog 与 FileSystem 索引，再通知 observer，不能只更新 Loader 后让 File Browser 保持过时的目录视图。

损坏当前格式、只读 mount 写入、Importer 冲突、Artifact closure 不完整和 observer failure 都明确报告。`Library` 可删除重建，不作为创作事实来源。






## 本轮边界与所有权

创作态可以持有物理缓存，但向消费者提供相同的路径中立 ArtifactLease。AssetLoader 与 AssetPipeline 按职责展开为 partial 文件，字段和生命周期仍集中在唯一 owner。Sample 复制异步执行，提交回 owner thread；恢复、Missing 与 source mount 使用现有候选事务。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Assets.Pipeline.AssetArtifactWriter`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Assets.Pipeline.AssetArtifactWriter.ReportDiagnostic(string message)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Artifacts/AssetArtifactWriter.cs#L60) | Adds a build diagnostic. |
| [`System.Threading.Tasks.ValueTask Inno.Assets.Pipeline.AssetArtifactWriter.WriteAsync(string outputName, System.ReadOnlyMemory<byte> bytes, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken), Inno.Assets.Pipeline.AssetDeploymentScope deploymentScope = Inno.Assets.Pipeline.AssetDeploymentScope.Runtime)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Artifacts/AssetArtifactWriter.cs#L36) | Writes one named build output. |
| [`Inno.Assets.Pipeline.AssetArtifactWriter`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Artifacts/AssetArtifactWriter.cs#L11) | Collects immutable named outputs for an aggregate asset build. |

### `Inno.Assets.Pipeline.AssetBuildContext<TDefinition>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetBuildContext<TDefinition>.AssetBuildContext(TDefinition definition, System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetInfo> inputs)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Building/AssetBuildContext.cs#L25) | Creates a build context. |
| [`TDefinition Inno.Assets.Pipeline.AssetBuildContext<TDefinition>.definition`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Building/AssetBuildContext.cs#L36) | Gets the build definition. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetInfo> Inno.Assets.Pipeline.AssetBuildContext<TDefinition>.inputs`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Building/AssetBuildContext.cs#L41) | Gets the input catalog snapshots. |
| [`Inno.Assets.Pipeline.AssetBuildContext<TDefinition>`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Building/AssetBuildContext.cs#L14) | Provides a stable input snapshot to an aggregate asset build. |

### `Inno.Assets.Pipeline.AssetBuildProcessor`

| 当前声明 | 行为 |
| --- | --- |
| [`abstract System.Type Inno.Assets.Pipeline.AssetBuildProcessor.definitionType`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Building/AssetBuildProcessor.cs#L51) | Gets the definition type accepted by this processor. |
| [`string Inno.Assets.Pipeline.AssetBuildProcessor.processorId`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Building/AssetBuildProcessor.cs#L44) | Gets the stable processor identifier used by build cache keys. |
| [`Inno.Assets.Pipeline.AssetBuildProcessor`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Building/AssetBuildProcessor.cs#L37) | Defines an automatically discovered aggregate asset build processor. |

### `Inno.Assets.Pipeline.AssetBuildProcessor<TDefinition>`

| 当前声明 | 行为 |
| --- | --- |
| [`abstract System.Threading.Tasks.ValueTask Inno.Assets.Pipeline.AssetBuildProcessor<TDefinition>.BuildAsync(Inno.Assets.Pipeline.AssetBuildContext<TDefinition> context, Inno.Assets.Pipeline.AssetArtifactWriter output, System.Threading.CancellationToken cancellationToken)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Building/AssetBuildProcessor.cs#L101) | Builds immutable outputs from a consistent asset snapshot. |
| [`override sealed System.Type Inno.Assets.Pipeline.AssetBuildProcessor<TDefinition>.definitionType`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Building/AssetBuildProcessor.cs#L84) | Gets the concrete type handled by this extension implementation. |
| [`Inno.Assets.Pipeline.AssetBuildProcessor<TDefinition>`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Building/AssetBuildProcessor.cs#L78) | Provides a strongly typed aggregate asset build processor. |

### `Inno.Assets.Pipeline.AssetBuildProcessorAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetBuildProcessorAttribute.AssetBuildProcessorAttribute(string id)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Building/AssetBuildProcessor.cs#L22) | Creates build-processor discovery metadata. |
| [`string Inno.Assets.Pipeline.AssetBuildProcessorAttribute.id`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Building/AssetBuildProcessor.cs#L31) | Gets the globally stable build processor identifier. |
| [`Inno.Assets.Pipeline.AssetBuildProcessorAttribute`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Building/AssetBuildProcessor.cs#L13) | Declares the immutable cache protocol identity of an automatically discovered asset build processor. |

### `Inno.Assets.Pipeline.AssetCacheOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Assets.Pipeline.AssetCacheOptions Inno.Assets.Pipeline.AssetCacheOptions.CreateDefault()`](../../src/content/assets/Inno.Assets.Pipeline/AssetCacheOptions.cs#L26) | Creates the default cache policy. |
| [`System.TimeSpan Inno.Assets.Pipeline.AssetCacheOptions.garbageCollectionGracePeriod`](../../src/content/assets/Inno.Assets.Pipeline/AssetCacheOptions.cs#L18) | Gets the minimum age of an unreachable artifact before it can be collected. |
| [`long Inno.Assets.Pipeline.AssetCacheOptions.maximumSizeBytes`](../../src/content/assets/Inno.Assets.Pipeline/AssetCacheOptions.cs#L13) | Gets the maximum artifact cache size in bytes, or zero for no size limit. |
| [`Inno.Assets.Pipeline.AssetCacheOptions`](../../src/content/assets/Inno.Assets.Pipeline/AssetCacheOptions.cs#L8) | Controls cleanup of rebuildable asset database data. |

### `Inno.Assets.Pipeline.AssetCatalogCandidate`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Assets.Pipeline.AssetCatalogCandidate.Commit()`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetCatalogCandidate.cs#L48) | Publishes staged source metadata and atomically promotes the validated catalog, compensating metadata on failure. |
| [`void Inno.Assets.Pipeline.AssetCatalogCandidate.Dispose()`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetCatalogCandidate.cs#L60) | Removes the candidate catalog staging storage without disposing the candidate loader. |
| [`Inno.Assets.Pipeline.AssetLoader Inno.Assets.Pipeline.AssetCatalogCandidate.loader`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetCatalogCandidate.cs#L34) | Gets the isolated loader whose catalog and in-memory records represent the candidate generation. |
| [`Inno.Assets.Pipeline.AssetCatalogCandidate`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetCatalogCandidate.cs#L14) | Owns the isolated catalog storage used to validate one candidate Asset Pipeline generation. |

### `Inno.Assets.Pipeline.AssetChangedEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`System.IO.WatcherChangeTypes Inno.Assets.Pipeline.AssetChangedEvent.changeType`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetChangedEvent.cs#L30) | Underlying file-system change type. |
| [`string Inno.Assets.Pipeline.AssetChangedEvent.oldRelativePath`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetChangedEvent.cs#L36) | Old path relative to watched root for rename operations. Empty for non-rename changes. |
| [`string Inno.Assets.Pipeline.AssetChangedEvent.relativePath`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetChangedEvent.cs#L25) | Changed path relative to watched root. |
| [`Inno.Assets.Pipeline.AssetChangedEvent`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetChangedEvent.cs#L17) | Batched file-system change event for asset source files. |

### `Inno.Assets.Pipeline.AssetDeploymentScope`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetDeploymentScope.AuthoringOnly`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetDeploymentScope.cs#L16) | The asset exists only for authoring workflows and is omitted from deployed catalogs. |
| [`Inno.Assets.Pipeline.AssetDeploymentScope.Runtime`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetDeploymentScope.cs#L11) | The asset is deployed and must produce a named runtime artifact output. |
| [`Inno.Assets.Pipeline.AssetDeploymentScope`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetDeploymentScope.cs#L6) | Defines whether an imported asset participates in deployed runtime content. |

### `Inno.Assets.Pipeline.AssetExportContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.IAssetArtifactLookup Inno.Assets.Pipeline.AssetExportContext.artifacts`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetExportContext.cs#L45) | Gets the owner-bound immutable outputs used to reconstruct editable sources. |
| [`Inno.Core.Serialization.SerializationRegistry Inno.Assets.Pipeline.AssetExportContext.serialization`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetExportContext.cs#L34) | Gets the serialization registry bound to the active export generation. |
| [`Inno.Assets.Pipeline.AssetSerializationServices Inno.Assets.Pipeline.AssetExportContext.services`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetExportContext.cs#L40) | Gets the narrow structured serialization API bound to this export generation. |
| [`Inno.Extensibility.Types.TypeCatalog Inno.Assets.Pipeline.AssetExportContext.types`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetExportContext.cs#L28) | Gets the type catalog bound to the active export generation. |
| [`Inno.Assets.Pipeline.AssetExportContext`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetExportContext.cs#L12) | Provides generation-bound services for one editable asset source export. |

### `Inno.Assets.Pipeline.AssetFileEntry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetPath Inno.Assets.Pipeline.AssetFileEntry.assetPath`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileEntry.cs#L30) | Gets the isolated source path. |
| [`string Inno.Assets.Pipeline.AssetFileEntry.extension`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileEntry.cs#L64) | Gets the normalized lower-case file extension. |
| [`bool Inno.Assets.Pipeline.AssetFileEntry.isDirectory`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileEntry.cs#L60) | Gets whether the entry represents a directory. |
| [`bool Inno.Assets.Pipeline.AssetFileEntry.isReadOnly`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileEntry.cs#L40) | Gets whether source mutations are forbidden. |
| [`bool Inno.Assets.Pipeline.AssetFileEntry.isSample`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileEntry.cs#L45) | Gets whether this entry is the root of an installed Plugin sample awaiting Project import. |
| [`bool Inno.Assets.Pipeline.AssetFileEntry.isSampleContent`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileEntry.cs#L50) | Gets whether this entry belongs to an installed Plugin sample subtree awaiting Project import. |
| [`string Inno.Assets.Pipeline.AssetFileEntry.name`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileEntry.cs#L16) | Gets the final source path segment, or the semantic mount-root label when this entry is a source root. |
| [`string Inno.Assets.Pipeline.AssetFileEntry.nameWithoutExtension`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileEntry.cs#L25) | Gets the final source path segment without its last extension. |
| [`Inno.Assets.AssetPath Inno.Assets.Pipeline.AssetFileEntry.parentAssetPath`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileEntry.cs#L55) | Gets the isolated parent directory path. |
| [`Inno.Assets.AssetSourceId Inno.Assets.Pipeline.AssetFileEntry.source`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileEntry.cs#L35) | Gets the owning source mount identity. |
| [`Inno.Assets.Pipeline.AssetFileEntry`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileEntry.cs#L11) | One node in the asset source filesystem index. |

### `Inno.Assets.Pipeline.AssetFileSystem`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetFileSystem.AssetFileSystem(System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetSourceMount> mounts, bool autoStart = true, int flushDelayMs = 80, Inno.Assets.Pipeline.AssetSourcePolicy? sourcePolicy = null, bool requireWritableProject = true)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L96) | Creates an indexed file system over isolated source mounts. |
| [`Inno.Assets.Pipeline.AssetFileSystem.AssetFileSystem(string assetRoot, bool autoStart = true, int flushDelayMs = 80, Inno.Assets.Pipeline.AssetSourcePolicy? sourcePolicy = null)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L59) | Creates an indexed source file system. |
| [`void Inno.Assets.Pipeline.AssetFileSystem.Dispose()`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L366) | Releases the resources owned by this implementation. |
| [`bool Inno.Assets.Pipeline.AssetFileSystem.Exists(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L210) | Determines whether an indexed source entry exists. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetFileEntry> Inno.Assets.Pipeline.AssetFileSystem.GetChildren(Inno.Assets.AssetPath parent)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L285) | Gets immediate children of an indexed directory. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetFileEntry> Inno.Assets.Pipeline.AssetFileSystem.GetEntries(bool includeDirectories = true)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L261) | Gets a stable snapshot of indexed entries. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetChangedEvent> Inno.Assets.Pipeline.AssetFileSystem.PollChanges()`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L308) | Polls normalized changes and refreshes the indexed source snapshot. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetChangedEvent> Inno.Assets.Pipeline.AssetFileSystem.PollChanges(out bool requiresFullRescan)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L319) | Polls changes and reports whether watcher recovery requires a full rescan. |
| [`void Inno.Assets.Pipeline.AssetFileSystem.Refresh()`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L188) | Rebuilds the indexed source file snapshot. |
| [`void Inno.Assets.Pipeline.AssetFileSystem.Start()`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L168) | Starts source file watching. |
| [`void Inno.Assets.Pipeline.AssetFileSystem.Stop()`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L177) | Stops source file watching. |
| [`bool Inno.Assets.Pipeline.AssetFileSystem.TryGetEntry(Inno.Assets.AssetPath path, out Inno.Assets.Pipeline.AssetFileEntry entry)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L232) | Tries to resolve an indexed source entry. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetChangedEvent> Inno.Assets.Pipeline.AssetFileSystem.WaitForIdle()`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L335) | Waits for a quiet watcher window, refreshes the index, and returns queued changes. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetChangedEvent> Inno.Assets.Pipeline.AssetFileSystem.WaitForIdle(out bool requiresFullRescan)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L346) | Waits for queued changes and reports whether a full rescan is required. |
| [`string Inno.Assets.Pipeline.AssetFileSystem.assetRoot`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L37) | Gets the absolute source asset root. |
| [`bool Inno.Assets.Pipeline.AssetFileSystem.isWatching`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L42) | Gets whether source file watching is active. |
| [`Inno.Assets.Pipeline.AssetFileSystem`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetFileSystem.cs#L18) | Indexed asset source filesystem model backed by . |

### `Inno.Assets.Pipeline.AssetImportContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.ArtifactLease Inno.Assets.Pipeline.AssetImportContext.AcquireArtifact(System.Guid assetId, string outputName)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L106) | Acquires an immutable dependency output and records its invalidation dependency automatically. |
| [`void Inno.Assets.Pipeline.AssetImportContext.DependsOnArtifact(System.Guid persistentId)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L301) | Declares an imported artifact that invalidates this imported asset. |
| [`void Inno.Assets.Pipeline.AssetImportContext.DependsOnAsset(Inno.Assets.AssetDependency dependency)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L243) | Declares a direct runtime dependency by persistent descriptor. |
| [`void Inno.Assets.Pipeline.AssetImportContext.DependsOnAsset(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L230) | Declares a direct runtime dependency by isolated source path. |
| [`void Inno.Assets.Pipeline.AssetImportContext.DependsOnCustomInput(string key, string fingerprint)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L320) | Declares a custom deterministic input that invalidates this asset. |
| [`void Inno.Assets.Pipeline.AssetImportContext.DependsOnSource(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L285) | Declares a source file that invalidates this imported asset. |
| [`System.ReadOnlyMemory<byte> Inno.Assets.Pipeline.AssetImportContext.ReadSourceBytes(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L199) | Reads another source from the same candidate mount snapshot and records it as an import dependency. |
| [`string Inno.Assets.Pipeline.AssetImportContext.ReadSourceUtf8Text(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L216) | Reads another source as UTF-8 from the current candidate mount snapshot and records the dependency. |
| [`string Inno.Assets.Pipeline.AssetImportContext.ReadUtf8Text()`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L179) | Reads the source bytes as UTF-8 text. |
| [`TAsset Inno.Assets.Pipeline.AssetImportContext.ResolveDependency<TAsset>(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L263) | Resolves and declares a strongly typed runtime asset dependency during import. |
| [`string Inno.Assets.Pipeline.AssetImportContext.absolutePath`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L122) | Gets the absolute source path. |
| [`Inno.Assets.AssetPath Inno.Assets.Pipeline.AssetImportContext.assetPath`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L117) | Gets the isolated source path. |
| [`string Inno.Assets.Pipeline.AssetImportContext.extension`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L171) | Gets the normalized lower-case source extension. |
| [`Inno.Core.Serialization.ISerializable? Inno.Assets.Pipeline.AssetImportContext.importSettings`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L166) | Gets the settings restored from this source's sidecar against the isolated import generation. The value is detached and changes made during import are not saved to the sidecar. |
| [`System.Guid Inno.Assets.Pipeline.AssetImportContext.persistentId`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L137) | Gets the persistent identity assigned to the source asset. |
| [`Inno.Assets.IAssetReferenceResolver Inno.Assets.Pipeline.AssetImportContext.references`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L154) | Gets the asset-reference resolver bound to the isolated candidate generation. |
| [`Inno.Core.Serialization.SerializationRegistry Inno.Assets.Pipeline.AssetImportContext.serialization`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L148) | Gets the serialization registry bound to the active import generation. |
| [`Inno.Assets.Pipeline.AssetSerializationServices Inno.Assets.Pipeline.AssetImportContext.services`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L160) | Gets the narrow structured serialization API bound to this importer candidate. |
| [`System.ReadOnlyMemory<byte> Inno.Assets.Pipeline.AssetImportContext.sourceBytes`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L127) | Gets the raw source bytes. |
| [`string Inno.Assets.Pipeline.AssetImportContext.sourceHash`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L132) | Gets the deterministic source hash. |
| [`Inno.Extensibility.Types.TypeCatalog Inno.Assets.Pipeline.AssetImportContext.types`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L142) | Gets the type catalog bound to the active import generation. |
| [`Inno.Assets.Pipeline.AssetImportContext`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportContext.cs#L16) | Collects source data and dependency declarations for one import operation. |

### `Inno.Assets.Pipeline.AssetImportExtensionUnavailableException`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetImportExtensionUnavailableException.AssetImportExtensionUnavailableException(string extensionKind, string extensionId)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportExtensionUnavailableException.cs#L22) | Describes a missing extension using generation-neutral identities. |
| [`string Inno.Assets.Pipeline.AssetImportExtensionUnavailableException.extensionId`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportExtensionUnavailableException.cs#L42) | Gets the required implementation's stable identity. |
| [`string Inno.Assets.Pipeline.AssetImportExtensionUnavailableException.extensionKind`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportExtensionUnavailableException.cs#L37) | Gets the stable extension protocol identity. |
| [`Inno.Assets.Pipeline.AssetImportExtensionUnavailableException`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportExtensionUnavailableException.cs#L11) | Reports a required authoring extension absent from the current generation. |

### `Inno.Assets.Pipeline.AssetImportFailure`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetImportFailure`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportHealthSnapshot.cs#L44) | Describes a writable-source import failure introduced after an earlier health snapshot. |

### `Inno.Assets.Pipeline.AssetImportHealthSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Assets.Pipeline.AssetImportHealthSnapshot Inno.Assets.Pipeline.AssetImportHealthSnapshot.empty`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportHealthSnapshot.cs#L32) | Gets the immutable snapshot used when no Asset Pipeline generation is active. |
| [`Inno.Assets.Pipeline.AssetImportHealthSnapshot`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportHealthSnapshot.cs#L14) | Represents an immutable observation of writable-source import failures at one point in time. |

### `Inno.Assets.Pipeline.AssetImportSettingsSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`string Inno.Assets.Pipeline.AssetImportSettingsSnapshot.fingerprint`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportSettingsSnapshot.cs#L30) | Gets the source metadata fingerprint required when saving this copy. |
| [`Inno.Core.Serialization.ISerializable? Inno.Assets.Pipeline.AssetImportSettingsSnapshot.value`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportSettingsSnapshot.cs#L25) | Gets the editable settings copy, or null for an importer without settings. |
| [`Inno.Assets.Pipeline.AssetImportSettingsSnapshot`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImportSettingsSnapshot.cs#L12) | Provides a detached settings value and an optimistic concurrency token for its source sidecar. |

### `Inno.Assets.Pipeline.AssetImportWriter<TAsset>`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Assets.Pipeline.AssetImportWriter<TAsset>.DependsOnArtifact(System.Guid persistentId)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Importing/AssetImportWriter.cs#L132) | Declares an asset artifact input that invalidates this import. |
| [`void Inno.Assets.Pipeline.AssetImportWriter<TAsset>.DependsOnAsset(Inno.Assets.AssetDependency dependency)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Importing/AssetImportWriter.cs#L116) | Declares a runtime dependency by persistent descriptor. |
| [`void Inno.Assets.Pipeline.AssetImportWriter<TAsset>.DependsOnAsset(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Importing/AssetImportWriter.cs#L108) | Declares a runtime dependency by isolated source path. |
| [`void Inno.Assets.Pipeline.AssetImportWriter<TAsset>.DependsOnCustomInput(string key, string fingerprint)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Importing/AssetImportWriter.cs#L143) | Declares a custom deterministic import input. |
| [`void Inno.Assets.Pipeline.AssetImportWriter<TAsset>.DependsOnSource(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Importing/AssetImportWriter.cs#L124) | Declares a source input that invalidates this import. |
| [`void Inno.Assets.Pipeline.AssetImportWriter<TAsset>.ReportDiagnostic(string message)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Importing/AssetImportWriter.cs#L154) | Adds a non-fatal import diagnostic. |
| [`void Inno.Assets.Pipeline.AssetImportWriter<TAsset>.SetAsset(TAsset asset)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Importing/AssetImportWriter.cs#L58) | Assigns the managed asset produced by the importer. |
| [`void Inno.Assets.Pipeline.AssetImportWriter<TAsset>.SetDeploymentScope(Inno.Assets.Pipeline.AssetDeploymentScope deploymentScope)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Importing/AssetImportWriter.cs#L43) | Selects the deployment scope for this imported asset. This permits one importer to produce runtime assets and editor-only assets from different source documents without changing their asset type. |
| [`System.Threading.Tasks.ValueTask Inno.Assets.Pipeline.AssetImportWriter<TAsset>.WriteArtifactAsync(string outputName, System.ReadOnlyMemory<byte> bytes, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken), Inno.Assets.Pipeline.AssetDeploymentScope deploymentScope = Inno.Assets.Pipeline.AssetDeploymentScope.Runtime)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Importing/AssetImportWriter.cs#L84) | Writes one immutable named artifact output. |
| [`TAsset? Inno.Assets.Pipeline.AssetImportWriter<TAsset>.asset`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Importing/AssetImportWriter.cs#L33) | Gets the candidate asset assigned by the importer. |
| [`Inno.Assets.Pipeline.AssetImportWriter<TAsset>`](../../src/content/assets/Inno.Assets.Pipeline/Importing/Importing/AssetImportWriter.cs#L16) | Collects the complete candidate output of one source import. |

### `Inno.Assets.Pipeline.AssetImporter`

| 当前声明 | 行为 |
| --- | --- |
| [`virtual Inno.Core.Serialization.ISerializable? Inno.Assets.Pipeline.AssetImporter.CreateImportSettings()`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImporter.cs#L71) | Creates a detached settings value for this importer in the current extension generation. |
| [`virtual Inno.Assets.Pipeline.AssetDeploymentScope Inno.Assets.Pipeline.AssetImporter.deploymentScope`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImporter.cs#L52) | Gets whether imported assets are deployed or retained only for authoring workflows. |
| [`string Inno.Assets.Pipeline.AssetImporter.importerId`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImporter.cs#L45) | Gets the stable importer implementation identifier. |
| [`abstract System.Collections.Generic.IReadOnlyList<string> Inno.Assets.Pipeline.AssetImporter.supportedExtensions`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImporter.cs#L62) | Gets the normalized source extensions accepted by this importer. |
| [`abstract System.Type Inno.Assets.Pipeline.AssetImporter.targetAssetType`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImporter.cs#L57) | Gets the concrete asset type produced by this importer. |
| [`Inno.Assets.Pipeline.AssetImporter`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImporter.cs#L38) | Defines metadata shared by automatically discovered asset importers. |

### `Inno.Assets.Pipeline.AssetImporter<TAsset>`

| 当前声明 | 行为 |
| --- | --- |
| [`virtual System.Threading.Tasks.ValueTask<System.ReadOnlyMemory<byte>?> Inno.Assets.Pipeline.AssetImporter<TAsset>.ExportAsync(Inno.Assets.Pipeline.AssetExportContext context, TAsset asset, System.Threading.CancellationToken cancellationToken)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImporter.cs#L144) | Exports an asset back into source bytes. |
| [`abstract System.Threading.Tasks.ValueTask Inno.Assets.Pipeline.AssetImporter<TAsset>.ImportAsync(Inno.Assets.Pipeline.AssetImportContext context, Inno.Assets.Pipeline.AssetImportWriter<TAsset> output, System.Threading.CancellationToken cancellationToken)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImporter.cs#L123) | Imports one source into a managed asset and named artifact outputs. |
| [`override sealed System.Type Inno.Assets.Pipeline.AssetImporter<TAsset>.targetAssetType`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImporter.cs#L106) | Gets the runtime asset type accepted by this importer implementation. |
| [`Inno.Assets.Pipeline.AssetImporter<TAsset>`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImporter.cs#L101) | Provides the strongly typed implementation base for an asset importer. |

### `Inno.Assets.Pipeline.AssetImporterAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetImporterAttribute.AssetImporterAttribute(string id)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImporter.cs#L23) | Creates importer discovery metadata. |
| [`string Inno.Assets.Pipeline.AssetImporterAttribute.id`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImporter.cs#L32) | Gets the globally stable importer protocol identifier. |
| [`Inno.Assets.Pipeline.AssetImporterAttribute`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetImporter.cs#L14) | Declares the immutable protocol identity of an automatically discovered asset importer. |

### `Inno.Assets.Pipeline.AssetLoader`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Action<Inno.Assets.AssetObject>? Inno.Assets.Pipeline.AssetLoader.AssetReloaded`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.cs#L308) | Occurs after a loaded canonical asset is updated in place. |
| [`Inno.Assets.Pipeline.AssetLoader.AssetLoader(Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Identity.IdentityAllocator identities, Inno.Core.Diagnostics.DiagnosticHub diagnostics, Inno.Core.Logging.LogRouter logs, System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetSourceMount> mounts, string libraryRoot, Inno.Assets.Pipeline.AssetSourcePolicy? sourcePolicy = null, bool runtimeArtifactsOnly = false)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.cs#L197) | Creates an asset loader over one project source and zero or more isolated sources. |
| [`Inno.Assets.Pipeline.AssetLoader.AssetLoader(Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Identity.IdentityAllocator identities, Inno.Core.Diagnostics.DiagnosticHub diagnostics, Inno.Core.Logging.LogRouter logs, string assetRoot, string libraryRoot, Inno.Assets.Pipeline.AssetSourcePolicy? sourcePolicy = null)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.cs#L145) | Creates an asset loader for one source and Library root pair. |
| [`Inno.Assets.ArtifactLease Inno.Assets.Pipeline.AssetLoader.AcquireArtifact(System.Guid persistentId, string outputName)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Residency.cs#L109) | Acquires an immutable output while excluding concurrent artifact collection. |
| [`void Inno.Assets.Pipeline.AssetLoader.ApplySourceChanges(System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetChangedEvent> changes)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.SourceChanges.cs#L60) | Applies normalized source file changes to the persistent catalog. |
| [`System.Threading.Tasks.ValueTask<Inno.Assets.AssetArtifactKey> Inno.Assets.Pipeline.AssetLoader.BuildAsync(Inno.Assets.AssetObject definition, System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetInfo> inputs, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Importing.cs#L55) | Builds and validates the requested artifact asynchronously before publishing it. |
| [`Inno.Assets.Pipeline.AssetImportHealthSnapshot Inno.Assets.Pipeline.AssetLoader.CaptureWritableImportHealth()`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Diagnostics.cs#L35) | Captures the current failure identity of every writable source without exposing catalog internals. |
| [`int Inno.Assets.Pipeline.AssetLoader.CollectArtifacts(System.TimeSpan gracePeriod, long maximumSizeBytes)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Residency.cs#L41) | Collects unreachable content-addressed artifacts. |
| [`void Inno.Assets.Pipeline.AssetLoader.Dispose()`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Retirement.cs#L38) | Stops new loads and releases canonical assets before their registries and diagnostics. |
| [`Inno.Assets.AssetRuntimeContentInfo Inno.Assets.Pipeline.AssetLoader.ExportRuntimeArtifacts(string destinationLibraryRoot)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.RuntimeExport.cs#L45) | Writes a source-free runtime catalog and its exact immutable artifact closure. |
| [`Inno.Assets.AssetRuntimeContentInfo Inno.Assets.Pipeline.AssetLoader.ExportRuntimeArtifacts(string destinationLibraryRoot, System.Threading.CancellationToken cancellationToken)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.RuntimeExport.cs#L66) | Exports the validated runtime artifact closure while observing cooperative cancellation between files. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetImportFailure> Inno.Assets.Pipeline.AssetLoader.FindIntroducedImportFailures(Inno.Assets.Pipeline.AssetImportHealthSnapshot baseline)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Diagnostics.cs#L59) | Finds writable-source failures that are new or changed relative to an earlier health snapshot. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> Inno.Assets.Pipeline.AssetLoader.GetDependencies(Inno.Assets.AssetObject asset, bool recursive = false)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Dependencies.cs#L41) | Gets direct or transitive runtime dependencies of an asset. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetPath> Inno.Assets.Pipeline.AssetLoader.GetImportDependencies(Inno.Assets.AssetObject asset, bool recursive = false)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Dependencies.cs#L61) | Gets source import dependencies that invalidate an asset artifact. |
| [`Inno.Assets.Pipeline.AssetImportSettingsSnapshot Inno.Assets.Pipeline.AssetLoader.GetImportSettings(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.ImportSettings.cs#L31) | Reads a detached importer settings value without changing source metadata. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetPath> Inno.Assets.Pipeline.AssetLoader.GetLoadedPaths()`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Catalog.cs#L209) | Gets isolated source paths of all canonical loaded assets. |
| [`Inno.Assets.AssetReferenceInfo Inno.Assets.Pipeline.AssetLoader.GetReferenceInfo(Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Dependencies.cs#L86) | Gets an engine-known reference diagnostic snapshot. |
| [`bool Inno.Assets.Pipeline.AssetLoader.Import(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Importing.cs#L38) | Imports one isolated source file into metadata and a runtime artifact. |
| [`Inno.Assets.AssetObject? Inno.Assets.Pipeline.AssetLoader.Load(Inno.Assets.AssetPath path, System.Type requestedAssetType)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Loading.cs#L41) | Loads a canonical asset by isolated source path. |
| [`Inno.Assets.AssetObject? Inno.Assets.Pipeline.AssetLoader.Load(System.Guid persistentId, System.Type requestedAssetType)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Loading.cs#L85) | Loads a canonical asset by persistent identity. |
| [`System.Threading.Tasks.ValueTask<Inno.Assets.AssetObject?> Inno.Assets.Pipeline.AssetLoader.LoadAsync(Inno.Assets.AssetPath path, System.Type requestedAssetType, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Loading.cs#L132) | Asynchronously loads a canonical asset by isolated source path. |
| [`System.Threading.Tasks.ValueTask<Inno.Assets.AssetObject?> Inno.Assets.Pipeline.AssetLoader.LoadAsync(System.Guid persistentId, System.Type requestedAssetType, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Loading.cs#L172) | Asynchronously loads a canonical asset by persistent identity. |
| [`Inno.Assets.Pipeline.AssetCatalogCandidate Inno.Assets.Pipeline.AssetLoader.PrepareCatalogCandidate(System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetSourceMount> mounts, Inno.Assets.Pipeline.AssetSourcePolicy? sourcePolicy = null)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Catalog.cs#L47) | Creates an isolated source-mount and catalog candidate without changing the active catalog. |
| [`bool Inno.Assets.Pipeline.AssetLoader.RefreshRegistries()`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.cs#L344) | Refreshes extension registries and reimports affected sources when their snapshot changed. |
| [`void Inno.Assets.Pipeline.AssetLoader.Rescan(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.SourceChanges.cs#L38) | Reconciles source files, metadata, artifacts and the in-memory catalog. |
| [`Inno.Assets.AssetObject Inno.Assets.Pipeline.AssetLoader.ResolveReference(System.Guid persistentId, System.Guid stableTypeId, string lastKnownPath, System.Type expectedType)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Loading.cs#L214) | Resolves a serialized reference or creates a persistent missing placeholder. |
| [`void Inno.Assets.Pipeline.AssetLoader.RestoreProperties<TValue>(System.Guid stableTypeId, byte[] propertyData, TValue target)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.cs#L101) | Restores serialized properties to the existing asset object. |
| [`bool Inno.Assets.Pipeline.AssetLoader.Save(Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Saving.cs#L38) | Saves an asset back to its current source path. |
| [`bool Inno.Assets.Pipeline.AssetLoader.Save(Inno.Assets.AssetPath path, Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Saving.cs#L58) | Saves an asset to its initial or existing isolated source path while preserving an existing destination identity. |
| [`bool Inno.Assets.Pipeline.AssetLoader.SaveImportSettings(Inno.Assets.AssetPath path, Inno.Core.Serialization.ISerializable? settings, string expectedFingerprint)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.ImportSettings.cs#L68) | Atomically saves importer settings and reimports the source. Failed imports retain both the saved settings and the previous successful artifact; callers must inspect the returned import status separately from saving. |
| [`bool Inno.Assets.Pipeline.AssetLoader.TryGetArtifact(System.Guid persistentId, string outputName, out Inno.Assets.AssetArtifactInfo? artifact)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Residency.cs#L74) | Tries to resolve a named output from the current artifact bundle. |
| [`bool Inno.Assets.Pipeline.AssetLoader.TryGetAssetType(Inno.Assets.AssetPath path, out System.Type? assetType)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Catalog.cs#L189) | Tries to resolve the concrete asset type without loading it. |
| [`bool Inno.Assets.Pipeline.AssetLoader.TryGetInfo(Inno.Assets.AssetPath path, out Inno.Assets.AssetInfo? info)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Catalog.cs#L114) | Tries to get a catalog snapshot by isolated source path. |
| [`bool Inno.Assets.Pipeline.AssetLoader.TryGetInfo(System.Guid persistentId, out Inno.Assets.AssetInfo? info)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Catalog.cs#L140) | Tries to get a catalog snapshot by persistent identity. |
| [`bool Inno.Assets.Pipeline.AssetLoader.TryGetPersistentId(Inno.Assets.AssetPath path, out System.Guid persistentId)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Catalog.cs#L163) | Tries to resolve a persistent identity without loading the asset. |
| [`bool Inno.Assets.Pipeline.AssetLoader.TryLoad(Inno.Assets.AssetPath path, System.Type requestedAssetType, out Inno.Assets.AssetObject? asset)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Loading.cs#L64) | Tries to load a canonical asset by isolated source path. |
| [`bool Inno.Assets.Pipeline.AssetLoader.TryLoad(System.Guid persistentId, System.Type requestedAssetType, out Inno.Assets.AssetObject? asset)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Loading.cs#L108) | Tries to load a canonical asset by persistent identity. |
| [`int Inno.Assets.Pipeline.AssetLoader.UnloadUnusedAssets()`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Residency.cs#L126) | Collects canonical assets that have no external managed references. |
| [`void Inno.Assets.Pipeline.AssetLoader.WaitForIdle()`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.cs#L336) | Waits for pending import and build work. |
| [`string Inno.Assets.Pipeline.AssetLoader.artifactRoot`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.cs#L303) | Gets the derived content-addressed artifact root. |
| [`string Inno.Assets.Pipeline.AssetLoader.assetRoot`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.cs#L293) | Gets the absolute source root. |
| [`string Inno.Assets.Pipeline.AssetLoader.libraryRoot`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.cs#L298) | Gets the absolute rebuildable Library root. |
| [`Inno.Assets.Pipeline.AssetLoader`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetLoader.Catalog.cs#L27) | Coordinates importing, persistent cataloging, canonical loading, reloading and collection for one source and artifact root pair. |

### `Inno.Assets.Pipeline.AssetPipeline`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Action<Inno.Assets.AssetObject>? Inno.Assets.Pipeline.AssetPipeline.AssetReloaded`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L163) | Occurs after a canonical loaded asset has been updated in place. |
| [`System.Action<Inno.Assets.AssetChangeSet>? Inno.Assets.Pipeline.AssetPipeline.Changed`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L158) | Occurs after an asset database transaction has committed. |
| [`System.Action? Inno.Assets.Pipeline.AssetPipeline.SourceMountsChanged`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L169) | Occurs after a complete isolated source-mount generation is atomically replaced. |
| [`Inno.Assets.Pipeline.AssetPipeline.AssetPipeline(Inno.Extensibility.Modules.ModuleHost modules, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Identity.IdentityAllocator identities, Inno.Core.Diagnostics.DiagnosticHub diagnostics, Inno.Core.Logging.LogRouter logs, Inno.Assets.Pipeline.AssetPipelineOptions options)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L198) | Creates one isolated authoring or deployed-runtime asset pipeline. |
| [`Inno.Assets.ArtifactLease Inno.Assets.Pipeline.AssetPipeline.AcquireArtifact(System.Guid persistentId, string outputName)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Artifacts.cs#L61) | Acquires one verified authoring artifact generation for an explicit lifetime. |
| [`System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.Pipeline.AssetPipeline.AcquireAsync<TAsset>(Inno.Assets.AssetPath path, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Loading.cs#L225) | Asynchronously acquires a canonical authoring asset by path for an explicit managed lifetime. |
| [`System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.Pipeline.AssetPipeline.AcquireAsync<TAsset>(System.Guid persistentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Loading.cs#L250) | Asynchronously acquires a canonical authoring asset by persistent identity. |
| [`System.Threading.Tasks.ValueTask<Inno.Assets.AssetArtifactKey> Inno.Assets.Pipeline.AssetPipeline.BuildAsync(Inno.Assets.AssetObject definition, System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetInfo> inputs, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Artifacts.cs#L81) | Runs an aggregate asset build using the processor registered for a definition. |
| [`Inno.Assets.AssetPropertySnapshot Inno.Assets.Pipeline.AssetPipeline.CaptureProperties<TValue>(TValue value)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L116) | Captures native settings and nested asset dependencies through this explicit authoring owner. |
| [`void Inno.Assets.Pipeline.AssetPipeline.CompleteExtensionDiscovery()`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Discovery.cs#L30) | Ends initial authoring extension discovery and strictly retries dependent imports. |
| [`void Inno.Assets.Pipeline.AssetPipeline.CreateDirectory(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Mutations.cs#L298) | Creates a tracked source directory and its persistent metadata. |
| [`Inno.Assets.Pipeline.AssetSourceStore Inno.Assets.Pipeline.AssetPipeline.CreateSourceStore()`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L101) | Creates a detached source editor sharing this owner's mounts, native converters and references. |
| [`void Inno.Assets.Pipeline.AssetPipeline.Delete(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Mutations.cs#L230) | Deletes a source asset and its metadata while retaining a Library tombstone for existing references. |
| [`void Inno.Assets.Pipeline.AssetPipeline.Dispose()`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Retirement.cs#L27) | Releases watchers, catalog participants, canonical objects, and rebuildable staging state. |
| [`Inno.Assets.AssetRuntimeContentInfo Inno.Assets.Pipeline.AssetPipeline.ExportRuntimeArtifacts(string destinationContentRoot)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Artifacts.cs#L107) | Exports the current source-free runtime catalog and its exact artifact closure. |
| [`System.Threading.Tasks.Task<Inno.Assets.AssetRuntimeContentInfo> Inno.Assets.Pipeline.AssetPipeline.ExportRuntimeArtifactsAsync(string destinationContentRoot, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Artifacts.cs#L143) | Exports a runtime-only artifact snapshot on a worker without loading artifact files into memory. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> Inno.Assets.Pipeline.AssetPipeline.GetDependencies(Inno.Assets.AssetObject asset, bool recursive = false)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Loading.cs#L304) | Gets direct or transitive runtime dependencies of an asset. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetFileEntry> Inno.Assets.Pipeline.AssetPipeline.GetFileSystemChildren(Inno.Assets.AssetPath parent)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Changes.cs#L84) | Gets immediate indexed children of a source directory. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetFileEntry> Inno.Assets.Pipeline.AssetPipeline.GetFileSystemEntries(bool includeDirectories = true)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Changes.cs#L72) | Gets indexed source entries. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetPath> Inno.Assets.Pipeline.AssetPipeline.GetImportDependencies(Inno.Assets.AssetObject asset, bool recursive = false)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Loading.cs#L322) | Gets source import dependencies that invalidate an asset artifact. |
| [`Inno.Assets.Pipeline.AssetImportSettingsSnapshot Inno.Assets.Pipeline.AssetPipeline.GetImportSettings(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Mutations.cs#L57) | Reads a detached settings copy for the currently registered importer. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetPath> Inno.Assets.Pipeline.AssetPipeline.GetLoadedPaths()`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Loading.cs#L290) | Gets isolated source paths for all canonical loaded assets. |
| [`Inno.Assets.AssetReferenceInfo Inno.Assets.Pipeline.AssetPipeline.GetReferenceInfo(Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Loading.cs#L337) | Gets an engine-known reference diagnostic snapshot. |
| [`bool Inno.Assets.Pipeline.AssetPipeline.Import(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Mutations.cs#L33) | Imports one source asset from an isolated source mount. |
| [`Inno.Assets.AssetObject Inno.Assets.Pipeline.AssetPipeline.Load(Inno.Assets.AssetPath path, System.Type assetType)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Loading.cs#L64) | Loads a canonical asset using a runtime asset type selected by an authoring workflow. |
| [`System.Threading.Tasks.ValueTask<TAsset> Inno.Assets.Pipeline.AssetPipeline.LoadAsync<TAsset>(Inno.Assets.AssetPath path, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Loading.cs#L167) | Asynchronously loads a canonical asset by isolated source path. |
| [`System.Threading.Tasks.ValueTask<TAsset> Inno.Assets.Pipeline.AssetPipeline.LoadAsync<TAsset>(System.Guid persistentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Loading.cs#L196) | Asynchronously loads a canonical asset by persistent identity. |
| [`TAsset Inno.Assets.Pipeline.AssetPipeline.Load<TAsset>(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Loading.cs#L36) | Loads a canonical asset by isolated source path. |
| [`TAsset Inno.Assets.Pipeline.AssetPipeline.Load<TAsset>(System.Guid persistentId)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Loading.cs#L95) | Loads a canonical asset by persistent identity. |
| [`void Inno.Assets.Pipeline.AssetPipeline.Move(Inno.Assets.AssetPath source, Inno.Assets.AssetPath target)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Mutations.cs#L163) | Moves a source asset while preserving its persistent identity and generated metadata. |
| [`Inno.Assets.Pipeline.AssetSampleImportTransaction Inno.Assets.Pipeline.AssetPipeline.PrepareSampleImport(Inno.Assets.AssetPath source)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetPipeline.Samples.cs#L47) | Starts a private background sample clone without waiting for file copying or transformation. |
| [`Inno.Assets.Pipeline.AssetSourceMountTransaction Inno.Assets.Pipeline.AssetPipeline.PrepareSourceMounts(System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetSourceMount> mounts)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetPipeline.SourceMounts.cs#L171) | Builds and validates an isolated source-mount candidate without changing active AssetPipeline state. |
| [`void Inno.Assets.Pipeline.AssetPipeline.ReplaceSourceMounts(System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetSourceMount> mounts)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetPipeline.SourceMounts.cs#L154) | Validates and atomically replaces the complete source-mount generation while preserving the active generation after any candidate failure. |
| [`void Inno.Assets.Pipeline.AssetPipeline.Rescan()`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Changes.cs#L27) | Reconciles source files, generated files and the persistent catalog. |
| [`void Inno.Assets.Pipeline.AssetPipeline.RestoreProperties<TValue>(System.Guid stableTypeId, byte[] propertyData, TValue target)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L138) | Restores serialized properties to the existing asset object. |
| [`bool Inno.Assets.Pipeline.AssetPipeline.Save(Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Mutations.cs#L107) | Saves an asset to its current source path. |
| [`bool Inno.Assets.Pipeline.AssetPipeline.Save(Inno.Assets.AssetPath path, Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Mutations.cs#L125) | Saves an asset to a writable isolated source path, preserving the destination source identity when it exists. |
| [`bool Inno.Assets.Pipeline.AssetPipeline.SaveImportSettings(Inno.Assets.AssetPath path, Inno.Core.Serialization.ISerializable? settings, string expectedFingerprint)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Mutations.cs#L82) | Saves import settings with conflict detection and immediately attempts to reimport the source. |
| [`bool Inno.Assets.Pipeline.AssetPipeline.TryGetArtifact(System.Guid persistentId, string outputName, out Inno.Assets.AssetArtifactInfo? artifact)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Artifacts.cs#L39) | Tries to resolve a named artifact output. |
| [`bool Inno.Assets.Pipeline.AssetPipeline.TryGetAssetType(Inno.Assets.AssetPath path, out System.Type? assetType)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Discovery.cs#L51) | Tries to resolve an asset type without loading the asset. |
| [`bool Inno.Assets.Pipeline.AssetPipeline.TryGetFileSystemEntry(Inno.Assets.AssetPath path, out Inno.Assets.Pipeline.AssetFileEntry entry)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Changes.cs#L98) | Tries to resolve an indexed source entry. |
| [`bool Inno.Assets.Pipeline.AssetPipeline.TryGetInfo(Inno.Assets.AssetPath path, out Inno.Assets.AssetInfo? info)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Discovery.cs#L85) | Tries to get a catalog snapshot by source-relative path. |
| [`bool Inno.Assets.Pipeline.AssetPipeline.TryGetInfo(System.Guid persistentId, out Inno.Assets.AssetInfo? info)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Discovery.cs#L102) | Tries to get a catalog snapshot by persistent identity. |
| [`bool Inno.Assets.Pipeline.AssetPipeline.TryGetPersistentId(Inno.Assets.AssetPath path, out System.Guid persistentId)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Discovery.cs#L68) | Tries to resolve a persistent identity without loading the asset. |
| [`bool Inno.Assets.Pipeline.AssetPipeline.TryLoad<TAsset>(Inno.Assets.AssetPath path, out TAsset? asset)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Loading.cs#L117) | Tries to load a canonical asset by isolated source path. |
| [`bool Inno.Assets.Pipeline.AssetPipeline.TryLoad<TAsset>(System.Guid persistentId, out TAsset? asset)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Loading.cs#L142) | Tries to load a canonical asset by persistent identity. |
| [`int Inno.Assets.Pipeline.AssetPipeline.UnloadUnusedAssets()`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Retirement.cs#L40) | Collects assets that have no external managed references. |
| [`void Inno.Assets.Pipeline.AssetPipeline.Update()`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Changes.cs#L39) | Applies queued source and build changes on the initialization thread. |
| [`void Inno.Assets.Pipeline.AssetPipeline.WaitForIdle()`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L294) | Waits until queued source watcher changes have been processed. |
| [`string Inno.Assets.Pipeline.AssetPipeline.artifactRoot`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L75) | Gets the absolute generated artifact root. |
| [`string Inno.Assets.Pipeline.AssetPipeline.assetRoot`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L65) | Gets the absolute source asset root. |
| [`Inno.Core.Identity.IdentityAllocator Inno.Assets.Pipeline.AssetPipeline.identities`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L92) | Gets the authoring identity domain shared by canonical assets and addressable source entries. |
| [`bool Inno.Assets.Pipeline.AssetPipeline.isInitialized`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L60) | Gets whether asset services are initialized. |
| [`string Inno.Assets.Pipeline.AssetPipeline.libraryRoot`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L70) | Gets the absolute root containing rebuildable asset database data. |
| [`long Inno.Assets.Pipeline.AssetPipeline.revision`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L80) | Gets the monotonic identity of the current committed asset and source-mount state. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetSourceMount> Inno.Assets.Pipeline.AssetPipeline.sourceMounts`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.cs#L86) | Gets the active isolated source mount snapshot. |
| [`Inno.Assets.Pipeline.AssetPipeline`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipeline.Artifacts.cs#L22) | Provides the single application-level entry point for importing, loading, saving and collecting assets. |

### `Inno.Assets.Pipeline.AssetPipelineMode`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetPipelineMode.Authoring`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineMode.cs#L11) | Reconciles writable source files and produces immutable artifacts. |
| [`Inno.Assets.Pipeline.AssetPipelineMode.RuntimeArtifacts`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineMode.cs#L16) | Loads a read-only deployed catalog and its content-addressed artifacts without source files. |
| [`Inno.Assets.Pipeline.AssetPipelineMode`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineMode.cs#L6) | Defines which side of the asset pipeline an instance serves. |

### `Inno.Assets.Pipeline.AssetPipelineOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Assets.Pipeline.AssetPipelineOptions Inno.Assets.Pipeline.AssetPipelineOptions.Create(string assetRoot, string libraryRoot)`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineOptions.cs#L73) | Creates options with sensible defaults for most projects. |
| [`string Inno.Assets.Pipeline.AssetPipelineOptions.assetRoot`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineOptions.cs#L29) | Gets the root folder containing source assets. |
| [`Inno.Assets.Pipeline.AssetCacheOptions Inno.Assets.Pipeline.AssetPipelineOptions.cacheOptions`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineOptions.cs#L59) | Gets the rebuildable cache policy. |
| [`bool Inno.Assets.Pipeline.AssetPipelineOptions.deferUnavailableExtensions`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineOptions.cs#L19) | Gets whether the composition host must still publish its initial authoring extensions. |
| [`bool Inno.Assets.Pipeline.AssetPipelineOptions.enableFileSystemWatcher`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineOptions.cs#L39) | Gets whether file-system watching is enabled. |
| [`int Inno.Assets.Pipeline.AssetPipelineOptions.fileWatcherFlushDelayMs`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineOptions.cs#L44) | Gets the watcher change coalescing delay in milliseconds. |
| [`string Inno.Assets.Pipeline.AssetPipelineOptions.libraryRoot`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineOptions.cs#L34) | Gets the root folder containing rebuildable project data. |
| [`Inno.Assets.Pipeline.AssetPipelineMode Inno.Assets.Pipeline.AssetPipelineOptions.mode`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineOptions.cs#L24) | Gets whether the pipeline reconciles authoring sources or consumes a deployed artifact catalog. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetSourceMount>? Inno.Assets.Pipeline.AssetPipelineOptions.sourceMounts`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineOptions.cs#L54) | Gets the complete source mount snapshot, or null to mount only . |
| [`Inno.Assets.Pipeline.AssetSourcePolicy? Inno.Assets.Pipeline.AssetPipelineOptions.sourcePolicy`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineOptions.cs#L49) | Gets the source filtering policy. |
| [`Inno.Assets.Pipeline.AssetPipelineOptions`](../../src/content/assets/Inno.Assets.Pipeline/AssetPipelineOptions.cs#L11) | Initialization options for . |

### `Inno.Assets.Pipeline.AssetSample`

| 当前声明 | 行为 |
| --- | --- |
| [`const string Inno.Assets.Pipeline.AssetSample.fileType`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSample.cs#L16) | Gets the logical File Browser type used for a sample directory. |
| [`static bool Inno.Assets.Pipeline.AssetSample.Contains(Inno.Assets.AssetPath path, bool isDirectory)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSample.cs#L64) | Determines whether a source path is a sample directory or is contained by one. |
| [`static string Inno.Assets.Pipeline.AssetSample.GetImportName(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSample.cs#L84) | Gets the original sample directory name for a writable Project copy. |
| [`static bool Inno.Assets.Pipeline.AssetSample.HasSampleDirectoryName(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSample.cs#L28) | Determines whether the final path segment uses the sample-directory naming convention. |
| [`static bool Inno.Assets.Pipeline.AssetSample.IsRoot(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSample.cs#L46) | Determines whether the final segment identifies a sample directory. |
| [`static bool Inno.Assets.Pipeline.AssetSample.IsRuntimeExcluded(Inno.Assets.AssetPath path, bool isDirectory)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSample.cs#L108) | Determines whether a path belongs to an authoring-only sample subtree that must not enter a Player runtime closure. |
| [`Inno.Assets.Pipeline.AssetSample`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSample.cs#L11) | Defines authoring sample directories and their installed Plugin behavior. |

### `Inno.Assets.Pipeline.AssetSampleImportTransaction`

| 当前声明 | 行为 |
| --- | --- |
| [`bool Inno.Assets.Pipeline.AssetSampleImportTransaction.Advance()`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleImportTransaction.cs#L113) | Starts background indexing after cloning, then adopts its completed catalog without waiting. |
| [`void Inno.Assets.Pipeline.AssetSampleImportTransaction.BeginValidation(System.Func<Inno.Assets.Pipeline.IAssetSourceSnapshot, System.Threading.CancellationToken, System.Threading.Tasks.ValueTask> validate)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleImportTransaction.cs#L167) | Starts one authoring preflight after indexing, retaining its dependencies until it drains. |
| [`void Inno.Assets.Pipeline.AssetSampleImportTransaction.Cancel()`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleImportTransaction.cs#L222) | Requests cancellation of copying and validation while retaining all dependencies. |
| [`void Inno.Assets.Pipeline.AssetSampleImportTransaction.Commit(System.Action<Inno.Assets.AssetPath>? beforePublish = null)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleImportTransaction.cs#L203) | Publishes a successfully validated import exactly once, without waiting for unfinished work. |
| [`void Inno.Assets.Pipeline.AssetSampleImportTransaction.Dispose()`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleImportTransaction.cs#L271) | Retires an uncommitted import on its owner thread after cancellation has drained. |
| [`void Inno.Assets.Pipeline.AssetSampleImportTransaction.Rollback()`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleImportTransaction.cs#L230) | Cancels work and removes an uncommitted copy after all background consumers have drained. |
| [`bool Inno.Assets.Pipeline.AssetSampleImportTransaction.isFaulted`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleImportTransaction.cs#L94) | Gets whether failed publication or retirement has faulted the host and requires a restart. |
| [`bool Inno.Assets.Pipeline.AssetSampleImportTransaction.isValidationComplete`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleImportTransaction.cs#L99) | Gets whether background validation has drained and Commit can inspect its outcome. |
| [`Inno.Assets.AssetPath Inno.Assets.Pipeline.AssetSampleImportTransaction.target`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleImportTransaction.cs#L89) | Gets the destination path with the original sample directory name preserved. |
| [`Inno.Assets.Pipeline.AssetSampleImportTransaction`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleImportTransaction.cs#L22) | Owns a cancellable sample clone, its pinned generation and its owner-thread publication. |

### `Inno.Assets.Pipeline.AssetSampleSourceRewriterAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetSampleSourceRewriterAttribute.AssetSampleSourceRewriterAttribute(string id)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleSourceRewriter.cs#L144) | Creates one ordered source-language transformer declaration. |
| [`string Inno.Assets.Pipeline.AssetSampleSourceRewriterAttribute.id`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleSourceRewriter.cs#L153) | Gets the stable extension identity. |
| [`Inno.Assets.Pipeline.AssetSampleSourceRewriterAttribute`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleSourceRewriter.cs#L135) | Registers a source-language sample transformer through the reloadable type catalog. |

### `Inno.Assets.Pipeline.AssetSampleTransformContext`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Assets.Pipeline.AssetSampleTransformContext.MapType(System.Guid oldId, System.Guid newId)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleSourceRewriter.cs#L102) | Registers one source-language type replacement for subsequent structured asset rewriting. |
| [`bool Inno.Assets.Pipeline.AssetSampleTransformContext.TryGetSourceIdentity(string relativePath, out System.Guid oldId, out System.Guid newId)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleSourceRewriter.cs#L73) | Resolves the source and clone identity of one file relative to the sample directory. |
| [`System.Threading.CancellationToken Inno.Assets.Pipeline.AssetSampleTransformContext.cancellationToken`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleSourceRewriter.cs#L51) | Gets cancellation for this private, generation-pinned transformation. |
| [`System.Collections.Generic.IReadOnlyDictionary<System.Guid, System.Guid> Inno.Assets.Pipeline.AssetSampleTransformContext.identityMap`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleSourceRewriter.cs#L56) | Gets cloned source identities, including directory and file metadata. |
| [`Inno.Assets.AssetPath Inno.Assets.Pipeline.AssetSampleTransformContext.source`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleSourceRewriter.cs#L41) | Gets the installed sample source path. |
| [`string Inno.Assets.Pipeline.AssetSampleTransformContext.stagedRoot`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleSourceRewriter.cs#L36) | Gets the absolute private staging directory; files here are committed only after all transforms succeed. |
| [`Inno.Assets.AssetPath Inno.Assets.Pipeline.AssetSampleTransformContext.target`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleSourceRewriter.cs#L46) | Gets the writable project destination path. |
| [`Inno.Assets.Pipeline.AssetSampleTransformContext`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleSourceRewriter.cs#L12) | Gives a source-language extension access to a staged sample clone before serialized references are remapped. |

### `Inno.Assets.Pipeline.AssetSerializationServices`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetPropertySnapshot Inno.Assets.Pipeline.AssetSerializationServices.CaptureProperties<TValue>(TValue value)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetSerializationServices.cs#L69) | Captures typed properties and their references as one neutral, owner-independent value. |
| [`TValue Inno.Assets.Pipeline.AssetSerializationServices.Deserialize<TValue>(System.ReadOnlySpan<byte> bytes)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetSerializationServices.cs#L126) | Deserializes one structured value against the active importer candidate generation. |
| [`System.Guid Inno.Assets.Pipeline.AssetSerializationServices.GetStableTypeId<TValue>()`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetSerializationServices.cs#L55) | Resolves the stable persistent type identity of a registered serializable type. |
| [`byte[] Inno.Assets.Pipeline.AssetSerializationServices.Serialize<TValue>(TValue value)`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetSerializationServices.cs#L96) | Serializes one structured value and declares every encountered asset reference as a runtime dependency. |
| [`Inno.Assets.Pipeline.AssetSerializationServices`](../../src/content/assets/Inno.Assets.Pipeline/Importing/AssetSerializationServices.cs#L12) | Provides generation-bound structured serialization without exposing host registries to importer extensions. |

### `Inno.Assets.Pipeline.AssetSourceMount`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetSourceMount.AssetSourceMount(Inno.Assets.AssetSourceId id, string rootPath, bool isReadOnly, System.Collections.Generic.IEnumerable<Inno.Assets.AssetSourceId>? dependencies = null)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSourceMount.cs#L31) | Creates an asset source mount. |
| [`string Inno.Assets.Pipeline.AssetSourceMount.Resolve(string localPath)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSourceMount.cs#L77) | Resolves a source-local path and rejects physical root escape. |
| [`System.Collections.Generic.IReadOnlySet<Inno.Assets.AssetSourceId> Inno.Assets.Pipeline.AssetSourceMount.dependencySourceIds`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSourceMount.cs#L66) | Gets explicitly declared cross-source dependencies. |
| [`Inno.Assets.AssetSourceId Inno.Assets.Pipeline.AssetSourceMount.id`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSourceMount.cs#L51) | Gets the stable source identity. |
| [`bool Inno.Assets.Pipeline.AssetSourceMount.isReadOnly`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSourceMount.cs#L61) | Gets whether source mutations are forbidden. |
| [`string Inno.Assets.Pipeline.AssetSourceMount.rootPath`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSourceMount.cs#L56) | Gets the controlled physical source root. |
| [`Inno.Assets.Pipeline.AssetSourceMount`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSourceMount.cs#L14) | Maps one isolated asset source to a controlled physical root. |

### `Inno.Assets.Pipeline.AssetSourceMountTransaction`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.ArtifactLease Inno.Assets.Pipeline.AssetSourceMountTransaction.AcquireArtifact(System.Guid persistentId, string outputName)`](../../src/content/assets/Inno.Assets.Pipeline/AssetSourceMountTransaction.cs#L135) | See the implemented contract. |
| [`void Inno.Assets.Pipeline.AssetSourceMountTransaction.Activate()`](../../src/content/assets/Inno.Assets.Pipeline/AssetSourceMountTransaction.cs#L172) | Activates this candidate without releasing the previous generation or notifying observers. |
| [`void Inno.Assets.Pipeline.AssetSourceMountTransaction.Complete()`](../../src/content/assets/Inno.Assets.Pipeline/AssetSourceMountTransaction.cs#L196) | Commits an activated candidate, notifies observers, and retires the previous generation. |
| [`void Inno.Assets.Pipeline.AssetSourceMountTransaction.Dispose()`](../../src/content/assets/Inno.Assets.Pipeline/AssetSourceMountTransaction.cs#L249) | Discards an unfinished candidate and releases its rebuildable staging storage. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetFileEntry> Inno.Assets.Pipeline.AssetSourceMountTransaction.GetFileSystemEntries(bool includeDirectories = true)`](../../src/content/assets/Inno.Assets.Pipeline/AssetSourceMountTransaction.cs#L82) | Gets candidate source entries without publishing them to active AssetPipeline consumers. |
| [`TAsset Inno.Assets.Pipeline.AssetSourceMountTransaction.Load<TAsset>(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/AssetSourceMountTransaction.cs#L104) | Loads one candidate asset by isolated source path. |
| [`void Inno.Assets.Pipeline.AssetSourceMountTransaction.Rollback()`](../../src/content/assets/Inno.Assets.Pipeline/AssetSourceMountTransaction.cs#L222) | Discards the candidate or restores the previous generation after provisional activation. |
| [`bool Inno.Assets.Pipeline.AssetSourceMountTransaction.TryGetArtifact(System.Guid persistentId, string outputName, out Inno.Assets.AssetArtifactInfo? artifact)`](../../src/content/assets/Inno.Assets.Pipeline/AssetSourceMountTransaction.cs#L159) | Tries to resolve one named candidate artifact. |
| [`bool Inno.Assets.Pipeline.AssetSourceMountTransaction.TryGetInfo(Inno.Assets.AssetPath path, out Inno.Assets.AssetInfo? info)`](../../src/content/assets/Inno.Assets.Pipeline/AssetSourceMountTransaction.cs#L125) | Tries to get candidate catalog information by isolated source path. |
| [`System.Collections.Generic.IReadOnlyList<Inno.References.ReferenceRecoveryChange> Inno.Assets.Pipeline.AssetSourceMountTransaction.recoveryChanges`](../../src/content/assets/Inno.Assets.Pipeline/AssetSourceMountTransaction.cs#L70) | Gets neutral canonical reference outcomes after activation, or an empty set before activation or after rollback. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetSourceMount> Inno.Assets.Pipeline.AssetSourceMountTransaction.sourceMounts`](../../src/content/assets/Inno.Assets.Pipeline/AssetSourceMountTransaction.cs#L64) | Gets the complete isolated mount snapshot represented by this candidate. |
| [`Inno.Assets.Pipeline.AssetSourceMountTransaction`](../../src/content/assets/Inno.Assets.Pipeline/AssetSourceMountTransaction.cs#L15) | Holds an isolated source-mount candidate that can be inspected before it atomically replaces the active Asset Database. |

### `Inno.Assets.Pipeline.AssetSourcePolicy`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.Pipeline.AssetSourcePolicy.AssetSourcePolicy()`](../../src/content/assets/Inno.Assets.Pipeline/Sources/Filtering/AssetSourcePolicy.cs#L47) | Creates a source policy using the engine's default noise filters. |
| [`Inno.Assets.Pipeline.AssetSourcePolicy.AssetSourcePolicy(System.Collections.Generic.IEnumerable<string>? ignoredFileNames, System.Collections.Generic.IEnumerable<string>? ignoredDirectoryNames, System.Collections.Generic.IEnumerable<string>? ignoredPrefixes, System.Collections.Generic.IEnumerable<string>? ignoredSuffixes)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/Filtering/AssetSourcePolicy.cs#L67) | Creates a source policy with additional ignored names and affixes. |
| [`static bool Inno.Assets.Pipeline.AssetSourcePolicy.IsGeneratedPath(string relativePath)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/Filtering/AssetSourcePolicy.cs#L131) | Determines whether a path is generated asset metadata. |
| [`bool Inno.Assets.Pipeline.AssetSourcePolicy.IsIgnored(string relativePath, bool isDirectory)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/Filtering/AssetSourcePolicy.cs#L98) | Determines whether an entry is excluded from the source database. |
| [`static Inno.Assets.Pipeline.AssetSourcePolicy Inno.Assets.Pipeline.AssetSourcePolicy.defaultPolicy`](../../src/content/assets/Inno.Assets.Pipeline/Sources/Filtering/AssetSourcePolicy.cs#L84) | Gets the default source policy. |
| [`Inno.Assets.Pipeline.AssetSourcePolicy`](../../src/content/assets/Inno.Assets.Pipeline/Sources/Filtering/AssetSourcePolicy.cs#L10) | Defines which physical entries are excluded from an asset source tree. |

### `Inno.Assets.Pipeline.AssetSourceSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`byte[] Inno.Assets.Pipeline.AssetSourceSnapshot.bytes`](../../src/content/assets/Inno.Assets.Pipeline/Editing/AssetSourceStore.cs#L161) | Gets a copy of the captured native source bytes. |
| [`string Inno.Assets.Pipeline.AssetSourceSnapshot.contentHash`](../../src/content/assets/Inno.Assets.Pipeline/Editing/AssetSourceStore.cs#L165) | Gets the source fingerprint required by a subsequent save. |
| [`bool Inno.Assets.Pipeline.AssetSourceSnapshot.isReadOnly`](../../src/content/assets/Inno.Assets.Pipeline/Editing/AssetSourceStore.cs#L169) | Gets whether the installation source cannot be edited in place. |
| [`Inno.Assets.Pipeline.AssetSourceSnapshot`](../../src/content/assets/Inno.Assets.Pipeline/Editing/AssetSourceStore.cs#L146) | Contains detached authoring bytes and conflict-detection state. |

### `Inno.Assets.Pipeline.AssetSourceStore`

| 当前声明 | 行为 |
| --- | --- |
| [`TAsset Inno.Assets.Pipeline.AssetSourceStore.Decode<TAsset>(System.ReadOnlySpan<byte> bytes)`](../../src/content/assets/Inno.Assets.Pipeline/Editing/AssetSourceStore.cs#L67) | Restores a detached native value using current-generation asset references. |
| [`byte[] Inno.Assets.Pipeline.AssetSourceStore.Encode<TAsset>(TAsset asset)`](../../src/content/assets/Inno.Assets.Pipeline/Editing/AssetSourceStore.cs#L53) | Encodes native asset properties with the owner's reference context and dependency capture. |
| [`Inno.Assets.Pipeline.AssetSourceSnapshot Inno.Assets.Pipeline.AssetSourceStore.Read(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Editing/AssetSourceStore.cs#L34) | Reads a mounted source without modifying its canonical asset or compiled artifacts. |
| [`string Inno.Assets.Pipeline.AssetSourceStore.Save(Inno.Assets.AssetPath path, byte[] bytes, string? expectedHash)`](../../src/content/assets/Inno.Assets.Pipeline/Editing/AssetSourceStore.cs#L85) | Atomically saves bytes after checking the source fingerprint; import is a separate operation. |
| [`Inno.Assets.Pipeline.AssetSourceStore`](../../src/content/assets/Inno.Assets.Pipeline/Editing/AssetSourceStore.cs#L12) | Reads detached authoring bytes and saves them independently of successful import. |

### `Inno.Assets.Pipeline.EditorAssets`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Assets.AssetPropertySnapshot Inno.Assets.Pipeline.EditorAssets.CaptureProperties<TValue>(TValue value)`](../../src/content/assets/Inno.Assets.Pipeline/EditorAssets.cs#L64) | Captures reload-safe settings using the currently bound authoring owner's converters and references. |
| [`static TAsset Inno.Assets.Pipeline.EditorAssets.DecodeNative<TAsset>(byte[] bytes)`](../../src/content/assets/Inno.Assets.Pipeline/EditorAssets.cs#L43) | Decodes detached native asset bytes with the current authoring pipeline's converters and reference context. |
| [`static byte[] Inno.Assets.Pipeline.EditorAssets.EncodeNative(Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets.Pipeline/EditorAssets.cs#L22) | Encodes a detached native asset with the current authoring pipeline's converters and reference context. |
| [`static bool Inno.Assets.Pipeline.EditorAssets.Save(Inno.Assets.AssetPath path, Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets.Pipeline/EditorAssets.cs#L88) | Creates or replaces a writable project asset source and imports the committed result. |
| [`Inno.Assets.Pipeline.EditorAssets`](../../src/content/assets/Inno.Assets.Pipeline/EditorAssets.cs#L11) | Provides Editor-script asset mutations through the authoring pipeline bound by the current host. |

### `Inno.Assets.Pipeline.IAssetSampleSourceRewriter`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Assets.Pipeline.IAssetSampleSourceRewriter.Transform(Inno.Assets.Pipeline.AssetSampleTransformContext context)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleSourceRewriter.cs#L129) | Rewrites staged source files and registers any changed serialized type identities. |
| [`Inno.Assets.Pipeline.IAssetSampleSourceRewriter`](../../src/content/assets/Inno.Assets.Pipeline/Sources/AssetSampleSourceRewriter.cs#L117) | Rewrites a source language in a private sample clone before source assets are published. |

### `Inno.Assets.Pipeline.IAssetSourceSnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetFileEntry> Inno.Assets.Pipeline.IAssetSourceSnapshot.GetFileSystemEntries(bool includeDirectories = true)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/IAssetSourceSnapshot.cs#L31) | Captures the indexed entries represented by this view. |
| [`TAsset Inno.Assets.Pipeline.IAssetSourceSnapshot.Load<TAsset>(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/IAssetSourceSnapshot.cs#L48) | Loads an authoring asset without publishing a candidate catalog. |
| [`bool Inno.Assets.Pipeline.IAssetSourceSnapshot.TryGetInfo(Inno.Assets.AssetPath path, out Inno.Assets.AssetInfo? info)`](../../src/content/assets/Inno.Assets.Pipeline/Sources/IAssetSourceSnapshot.cs#L62) | Resolves immutable catalog information by source path. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetSourceMount> Inno.Assets.Pipeline.IAssetSourceSnapshot.sourceMounts`](../../src/content/assets/Inno.Assets.Pipeline/Sources/IAssetSourceSnapshot.cs#L20) | Gets the source mounts represented by this view. |
| [`Inno.Assets.Pipeline.IAssetSourceSnapshot`](../../src/content/assets/Inno.Assets.Pipeline/Sources/IAssetSourceSnapshot.cs#L15) | Provides read-only authoring source access for active or isolated candidate compilation. |

### `Inno.Assets.Pipeline.NativeAssetSourceSerialization`

| 当前声明 | 行为 |
| --- | --- |
| [`static byte[] Inno.Assets.Pipeline.NativeAssetSourceSerialization.Export<TAsset>(TAsset asset, Inno.Assets.Pipeline.AssetSerializationServices services)`](../../src/content/assets/Inno.Assets.Pipeline/Serialization/NativeAssetSourceSerialization.cs#L30) | Serializes one asset's editable properties and direct asset dependencies. |
| [`static TAsset Inno.Assets.Pipeline.NativeAssetSourceSerialization.Import<TAsset>(System.ReadOnlySpan<byte> bytes, Inno.Assets.Pipeline.AssetSerializationServices services, out System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> dependencies)`](../../src/content/assets/Inno.Assets.Pipeline/Serialization/NativeAssetSourceSerialization.cs#L73) | Restores one concrete asset and its declared direct dependencies. |
| [`Inno.Assets.Pipeline.NativeAssetSourceSerialization`](../../src/content/assets/Inno.Assets.Pipeline/Serialization/NativeAssetSourceSerialization.cs#L13) | Imports and exports editable asset source state through the common native serializer. |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Collections](../core/Inno.Core.Collections.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Assets](Inno.Assets.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.References](../references/Inno.References.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
