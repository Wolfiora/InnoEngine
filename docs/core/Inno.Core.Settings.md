# Inno.Core.Settings

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

## 职责与边界

SettingsDocumentStore、ProjectSettings 与 ProjectSettingsStore 接收 IByteDocumentStore，不要求物理文件名。创作宿主注入 FileByteDocumentStore；Player 从 content lease 捕获 bytes 后注入 ReadOnlyByteDocumentStore。写入只读来源明确失败；完整 SerializationContext 由 owner 提供。

## 文档来源和代际

```csharp
using Inno.Core.IO;
using Inno.Core.Serialization;
using Inno.Core.Settings;

static SettingsDocumentStore<TDocument> CreateStore<TDocument>(
    IByteDocumentStore document,
    SerializationRegistry serialization,
    System.Func<TDocument> createDefault
) where TDocument : class, ISerializable
{
    return new SettingsDocumentStore<TDocument>(document, serialization, createDefault);
}
```

FileByteDocumentStore 使用 AtomicFile；ReadOnlyByteDocumentStore 持有独立 bytes 并拒绝写入。Settings store 借用文档来源，不能偷偷选择另一文件。读取先验证当前格式，再生成独立数据；损坏不是默认值。ProjectSettingsStore 接收 owner 的完整 SerializationContext，贡献 snapshot 在统一 generation 候选成功后切换；暂时不可用的 settings 保留中立数据。

创作 composition 注入可写来源；Player 从 content lease 捕获 Project Settings 并注入只读来源。Editor 与 Build 使用同一 Settings 协议，各宿主只决定文档介质。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Core.Settings.IProjectSettingsLookup`

| 当前声明 | 行为 |
| --- | --- |
| [`TSetting Inno.Core.Settings.IProjectSettingsLookup.Get<TSetting>(Inno.Core.Settings.ProjectSettingId id)`](../../src/foundation/core/Inno.Core.Settings/IProjectSettingsLookup.cs#L33) | Gets an isolated effective setting from the active extension generation. |
| [`bool Inno.Core.Settings.IProjectSettingsLookup.TryGet<TSetting>(Inno.Core.Settings.ProjectSettingId id, out TSetting? setting)`](../../src/foundation/core/Inno.Core.Settings/IProjectSettingsLookup.cs#L52) | Tries to get an isolated effective setting from the active extension generation. |
| [`System.Guid Inno.Core.Settings.IProjectSettingsLookup.ownerId`](../../src/foundation/core/Inno.Core.Settings/IProjectSettingsLookup.cs#L14) | Gets the unique lifetime identity of this settings owner; revisions from different owners are not interchangeable. |
| [`long Inno.Core.Settings.IProjectSettingsLookup.revision`](../../src/foundation/core/Inno.Core.Settings/IProjectSettingsLookup.cs#L19) | Gets the monotonic revision of the active effective settings snapshot. |
| [`Inno.Core.Settings.IProjectSettingsLookup`](../../src/foundation/core/Inno.Core.Settings/IProjectSettingsLookup.cs#L9) | Defines the read-only effective settings boundary shared by Editor and Player hosts. |

### `Inno.Core.Settings.ProjectId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectId.ProjectId(string value)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L20) | Creates a portable project identifier. |
| [`static Inno.Core.Settings.ProjectId Inno.Core.Settings.ProjectId.FromName(string name)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L37) | Creates a portable initial project identifier from a user-facing project name. |
| [`Inno.Core.Settings.ProjectScopedId Inno.Core.Settings.ProjectId.Qualify(Inno.Core.Settings.ProjectLocalId name)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L84) | Qualifies a local project name beneath this project namespace. |
| [`override string Inno.Core.Settings.ProjectId.ToString()`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L92) | Formats the canonical identifier. |
| [`bool Inno.Core.Settings.ProjectId.isValid`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L73) | Gets whether the value contains a usable identifier. |
| [`string Inno.Core.Settings.ProjectId.value`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L68) | Gets the canonical identifier text. |
| [`Inno.Core.Settings.ProjectId`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L12) | Identifies one project namespace used to qualify project-authored logical names. |

### `Inno.Core.Settings.ProjectIdentitySettings`

| 当前声明 | 行为 |
| --- | --- |
| [`const string Inno.Core.Settings.ProjectIdentitySettings.settingProtocolId`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L265) | Gets the immutable project-setting protocol value used by discovery metadata. |
| [`Inno.Core.Settings.ProjectScopedId Inno.Core.Settings.ProjectIdentitySettings.Qualify(string name)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L298) | Qualifies a display name under the current project identifier. |
| [`Inno.Core.Settings.ProjectId Inno.Core.Settings.ProjectIdentitySettings.id`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L287) | Gets the validated project identifier. |
| [`string Inno.Core.Settings.ProjectIdentitySettings.projectId`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L277) | Gets or sets the editable project namespace. |
| [`static Inno.Core.Settings.ProjectSettingId Inno.Core.Settings.ProjectIdentitySettings.settingId`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L272) | Gets the stable project setting protocol identity. |
| [`Inno.Core.Settings.ProjectIdentitySettings`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L258) | Stores the editable identity namespace of the current project. |

### `Inno.Core.Settings.ProjectLocalId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectLocalId.ProjectLocalId(string value)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L132) | Creates a portable local identity. |
| [`static Inno.Core.Settings.ProjectLocalId Inno.Core.Settings.ProjectLocalId.FromName(string name)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L154) | Creates a deterministic portable local identity from a display name. |
| [`override string Inno.Core.Settings.ProjectLocalId.ToString()`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L193) | Formats the canonical local identity. |
| [`string Inno.Core.Settings.ProjectLocalId.value`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L143) | Gets the canonical local identity text. |
| [`Inno.Core.Settings.ProjectLocalId`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L124) | Stores the project-independent portion of one project-scoped identity. |

### `Inno.Core.Settings.ProjectScopedId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectScopedId.ProjectScopedId(Inno.Core.Settings.ProjectId projectId, Inno.Core.Settings.ProjectLocalId name)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L219) | Creates a qualified project identity. |
| [`override string Inno.Core.Settings.ProjectScopedId.ToString()`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L252) | Formats the canonical qualified identity. |
| [`Inno.Core.Settings.ProjectLocalId Inno.Core.Settings.ProjectScopedId.name`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L239) | Gets the project-independent local identity. |
| [`Inno.Core.Settings.ProjectId Inno.Core.Settings.ProjectScopedId.projectId`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L234) | Gets the project namespace. |
| [`string Inno.Core.Settings.ProjectScopedId.value`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L244) | Gets the canonical projectId.name representation. |
| [`Inno.Core.Settings.ProjectScopedId`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L208) | Combines a mutable project namespace with a stable project-independent local identity. |

### `Inno.Core.Settings.ProjectSettingComposer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectSettingComposer`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L134) | Defines the non-generic base for a protocol-owned project setting composer. |

### `Inno.Core.Settings.ProjectSettingComposer<TSetting, TContribution>`

| 当前声明 | 行为 |
| --- | --- |
| [`abstract TContribution Inno.Core.Settings.ProjectSettingComposer<TSetting, TContribution>.CaptureContribution(TSetting baseline, TSetting value)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L179) | Captures the contribution introduced by the supplied project setting value. |
| [`abstract void Inno.Core.Settings.ProjectSettingComposer<TSetting, TContribution>.Compose(TSetting target, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingContribution<TContribution>> contributions)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L204) | Composes dependency-ordered Plugin deltas and the optional final project delta into a host default. |
| [`abstract bool Inno.Core.Settings.ProjectSettingComposer<TSetting, TContribution>.IsEmpty(TContribution contribution)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L193) | Gets whether a captured contribution contains no semantic operation. |
| [`Inno.Core.Settings.ProjectSettingComposer<TSetting, TContribution>`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L163) | Lets one setting protocol define deterministic delta capture and multi-contributor composition. |

### `Inno.Core.Settings.ProjectSettingComposerAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectSettingComposerAttribute.ProjectSettingComposerAttribute(string settingId)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L21) | Creates a composer declaration for one stable setting protocol. |
| [`Inno.Core.Settings.ProjectSettingId Inno.Core.Settings.ProjectSettingComposerAttribute.settingId`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L30) | Gets the stable setting protocol composed by the attributed type. |
| [`Inno.Core.Settings.ProjectSettingComposerAttribute`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L12) | Declares the protocol-owned composer used to combine contributions for one project setting. Settings without a composer retain the default whole-value replacement behavior. |

### `Inno.Core.Settings.ProjectSettingContribution<TContribution>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectSettingContributionContext Inno.Core.Settings.ProjectSettingContribution<TContribution>.context`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L123) | Gets the ownership and dependency context for this contribution. |
| [`TContribution Inno.Core.Settings.ProjectSettingContribution<TContribution>.value`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L128) | Gets the decoded protocol-owned contribution value. |
| [`Inno.Core.Settings.ProjectSettingContribution<TContribution>`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L109) | Provides one decoded contribution and its immutable composition context. |

### `Inno.Core.Settings.ProjectSettingContributionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`bool Inno.Core.Settings.ProjectSettingContributionContext.CanOverride(string ownerId)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L94) | Gets whether this contribution may explicitly replace data owned by another contributor. |
| [`string Inno.Core.Settings.ProjectSettingContributionContext.contributorId`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L72) | Gets the stable Plugin ID, or project for the project-authored contribution. |
| [`bool Inno.Core.Settings.ProjectSettingContributionContext.isProject`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L82) | Gets whether this is the project-authored highest-precedence contribution. |
| [`Inno.Core.Settings.ProjectSettingContributionSource Inno.Core.Settings.ProjectSettingContributionContext.source`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L77) | Gets the contribution source. |
| [`Inno.Core.Settings.ProjectSettingContributionContext`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L52) | Exposes ownership and dependency information while a protocol-owned composer combines one contribution. |

### `Inno.Core.Settings.ProjectSettingContributionSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectSettingContributionSource.Plugin`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L41) | The contribution is a default supplied by an activated Plugin. |
| [`Inno.Core.Settings.ProjectSettingContributionSource.Project`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L46) | The contribution is the project-authored delta with highest precedence. |
| [`Inno.Core.Settings.ProjectSettingContributionSource`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L36) | Identifies where one setting contribution originated. |

### `Inno.Core.Settings.ProjectSettingDefinitionAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectSettingDefinitionAttribute.ProjectSettingDefinitionAttribute(string id, bool allowPluginContributions = true)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L59) | Creates a project setting declaration. |
| [`bool Inno.Core.Settings.ProjectSettingDefinitionAttribute.allowPluginContributions`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L76) | Gets whether Plugin packages may contribute values to this protocol. |
| [`string Inno.Core.Settings.ProjectSettingDefinitionAttribute.id`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L71) | Gets the globally stable setting identifier. |
| [`Inno.Core.Settings.ProjectSettingDefinitionAttribute`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L47) | Declares a reloadable settings type under one stable protocol identity. |

### `Inno.Core.Settings.ProjectSettingId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectSettingId.ProjectSettingId(string value)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L19) | Creates a project setting identifier. |
| [`override string Inno.Core.Settings.ProjectSettingId.ToString()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L41) | Formats this value as a human-readable representation. |
| [`bool Inno.Core.Settings.ProjectSettingId.isValid`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L33) | Gets whether the identifier has a usable value. |
| [`string Inno.Core.Settings.ProjectSettingId.value`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L28) | Gets or sets the globally stable setting value. |
| [`Inno.Core.Settings.ProjectSettingId`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L11) | Identifies one host-neutral project setting protocol. |

### `Inno.Core.Settings.ProjectSettingRecord`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectSettingRecord.ProjectSettingRecord(Inno.Core.Settings.ProjectSettingId id, System.Guid stableTypeId, System.ReadOnlySpan<byte> propertyData)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L96) | Creates a setting record. |
| [`Inno.Core.Settings.ProjectSettingId Inno.Core.Settings.ProjectSettingRecord.id`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L113) | Gets or sets the stable setting identity. |
| [`byte[] Inno.Core.Settings.ProjectSettingRecord.propertyData`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L123) | Gets or sets the composer-owned payload or default complete-property payload. |
| [`System.Guid Inno.Core.Settings.ProjectSettingRecord.stableTypeId`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L118) | Gets or sets the stable settings type identity. |
| [`Inno.Core.Settings.ProjectSettingRecord`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L82) | Stores one protocol-owned neutral setting contribution for persistence and composition. |

### `Inno.Core.Settings.ProjectSettings`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectSettings.ProjectSettings(Inno.Core.IO.IByteDocumentStore documentStore, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Settings.ProjectId defaultProjectId, Inno.Core.Serialization.SerializationContext serializationContext, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor>? contributors = null)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L51) | Loads one project settings document and builds the initial effective snapshot. |
| [`bool Inno.Core.Settings.ProjectSettings.ApplyProjectOverrides(System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Settings.ProjectSettingId, Inno.Core.Serialization.ISerializable> values, System.Collections.Generic.IReadOnlySet<Inno.Core.Settings.ProjectSettingId>? resets, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L417) | Applies multiple project contributions and removals as one atomic document update. |
| [`byte[] Inno.Core.Settings.ProjectSettings.CaptureDocument()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L308) | Captures the native project contribution document. |
| [`void Inno.Core.Settings.ProjectSettings.Dispose()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L494) | Releases registry snapshots and generation-local setting objects. |
| [`TSetting Inno.Core.Settings.ProjectSettings.Get<TSetting>(Inno.Core.Settings.ProjectSettingId id)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L96) | Gets an isolated snapshot of a current-generation setting. |
| [`bool Inno.Core.Settings.ProjectSettings.HasProjectOverride(Inno.Core.Settings.ProjectSettingId id)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L235) | Gets whether the project document explicitly contributes to one setting protocol. |
| [`void Inno.Core.Settings.ProjectSettings.Rebuild(System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors, bool allowUnresolvedContributions = false)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L323) | Atomically rebuilds effective settings for a new extension generation. |
| [`void Inno.Core.Settings.ProjectSettings.RestoreDocument(System.ReadOnlySpan<byte> document, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L481) | Atomically restores a native project settings document. |
| [`void Inno.Core.Settings.ProjectSettings.SetProjectOverride(Inno.Core.Settings.ProjectSettingId id, Inno.Core.Serialization.ISerializable value, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L391) | Persists one project-authored semantic contribution and rebuilds the effective value. |
| [`bool Inno.Core.Settings.ProjectSettings.TryCapture(Inno.Core.Settings.ProjectSettingId id, string contributorId, System.Collections.Generic.IReadOnlySet<string> declaredDependencies, System.Collections.Generic.IReadOnlySet<string> declaredOverrides, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors, out Inno.Core.Settings.ProjectSettingRecord record)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L164) | Captures and validates the project-authored delta as one prospective Plugin contribution. |
| [`bool Inno.Core.Settings.ProjectSettings.TryClone(Inno.Core.Settings.ProjectSettingId id, out Inno.Core.Serialization.ISerializable? setting)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L253) | Creates an isolated editable copy of one effective setting. |
| [`bool Inno.Core.Settings.ProjectSettings.TryCloneComposedDefault(Inno.Core.Settings.ProjectSettingId id, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors, out Inno.Core.Serialization.ISerializable? setting)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L285) | Creates an isolated copy composed without the project-authored contribution. |
| [`bool Inno.Core.Settings.ProjectSettings.TryGet<TSetting>(Inno.Core.Settings.ProjectSettingId id, out TSetting? setting)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L120) | Tries to get an isolated snapshot of a current-generation setting. |
| [`Inno.Core.Settings.ProjectSettings`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L16) | Composes setting defaults, Plugin contributions, and project overrides atomically. |

### `Inno.Core.Settings.ProjectSettingsContributor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectSettingsContributor.ProjectSettingsContributor(string id, System.Collections.Generic.IEnumerable<string> dependencies, System.Collections.Generic.IEnumerable<string> overrides, System.Collections.Generic.IEnumerable<Inno.Core.Settings.ProjectSettingRecord> settings)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L160) | Creates one settings contributor. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Core.Settings.ProjectSettingsContributor.dependencies`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L184) | Gets contributors that must precede this contributor. |
| [`string Inno.Core.Settings.ProjectSettingsContributor.id`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L179) | Gets the stable contributor identity. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Core.Settings.ProjectSettingsContributor.overrides`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L189) | Gets dependencies whose defaults may be replaced. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingRecord> Inno.Core.Settings.ProjectSettingsContributor.settings`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L194) | Gets a detached read-only contribution snapshot, including independently owned payload bytes. |
| [`Inno.Core.Settings.ProjectSettingsContributor`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L141) | Describes one dependency-ordered provider of default setting values. |

### `Inno.Core.Settings.ProjectSettingsDocument`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectSettingRecord[] Inno.Core.Settings.ProjectSettingsDocument.overrides`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L134) | Gets or sets project-authored protocol contributions. |
| [`Inno.Core.Settings.ProjectSettingsDocument`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L129) | Stores project-authored setting contributions in one native document. |

### `Inno.Core.Settings.ProjectSettingsExecutionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.IDisposable Inno.Core.Settings.ProjectSettingsExecutionContext.EnterScope(Inno.Core.Settings.IProjectSettingsLookup settings)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsExecutionContext.cs#L38) | Binds a settings lookup until the returned strict last-in-first-out scope is disposed. |
| [`static Inno.Core.Settings.IProjectSettingsLookup Inno.Core.Settings.ProjectSettingsExecutionContext.current`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsExecutionContext.cs#L24) | Gets the settings lookup bound to the current asynchronous execution context. |
| [`Inno.Core.Settings.ProjectSettingsExecutionContext`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsExecutionContext.cs#L14) | Binds one host-owned settings lookup to the current asynchronous script execution context. |

### `Inno.Core.Settings.ProjectSettingsStore`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectSettingsStore.ProjectSettingsStore(Inno.Core.IO.IByteDocumentStore documentStore, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Settings.ProjectId defaultProjectId, Inno.Core.Serialization.SerializationContext serializationContext)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L48) | Creates a project settings store from one type and serialization generation owner. |
| [`bool Inno.Core.Settings.ProjectSettingsStore.ApplyProjectOverrides(System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Settings.ProjectSettingId, Inno.Core.Serialization.ISerializable> values, System.Collections.Generic.IReadOnlySet<Inno.Core.Settings.ProjectSettingId>? resets = null)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L419) | Applies a native batch of project-authored overrides and removals. |
| [`byte[] Inno.Core.Settings.ProjectSettingsStore.CaptureDocument()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L319) | Captures the current native project override document. |
| [`Inno.Extensibility.Reload.IGenerationChange Inno.Core.Settings.ProjectSettingsStore.CreateReloadChange()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L84) | Creates a five-phase settings change for the host's shared generation transaction. |
| [`void Inno.Core.Settings.ProjectSettingsStore.Dispose()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L462) | Shuts down settings and releases generation-scoped values. |
| [`System.IDisposable Inno.Core.Settings.ProjectSettingsStore.EnterExecutionScope()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L137) | Binds this settings store to the current asynchronous script execution context. |
| [`TSetting Inno.Core.Settings.ProjectSettingsStore.Get<TSetting>(Inno.Core.Settings.ProjectSettingId id)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L158) | Gets an isolated effective setting snapshot from the active extension generation. |
| [`bool Inno.Core.Settings.ProjectSettingsStore.HasProjectOverride(Inno.Core.Settings.ProjectSettingId id)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L206) | Gets whether the active project document explicitly overrides one setting protocol. |
| [`Inno.Core.Settings.ProjectScopedId Inno.Core.Settings.ProjectSettingsStore.QualifyId(string name)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L101) | Qualifies one local name under the current project namespace. |
| [`void Inno.Core.Settings.ProjectSettingsStore.Rebuild(System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L332) | Rebuilds settings for one dependency-ordered extension generation. |
| [`void Inno.Core.Settings.ProjectSettingsStore.RebuildCurrent(bool allowUnresolvedContributions = false)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L370) | Rebuilds effective settings after the active type catalog changes. |
| [`void Inno.Core.Settings.ProjectSettingsStore.RestoreDocument(System.ReadOnlySpan<byte> document)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L440) | Restores a previously captured native project settings document. |
| [`void Inno.Core.Settings.ProjectSettingsStore.SetContributors(System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L351) | Publishes dependency-ordered default contributors for the next current-generation rebuild without constructing setting instances from types that may still be awaiting assembly activation. |
| [`void Inno.Core.Settings.ProjectSettingsStore.SetProjectOverride(Inno.Core.Settings.ProjectSettingId id, Inno.Core.Serialization.ISerializable value, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L392) | Persists one project-authored override through the native settings document. |
| [`bool Inno.Core.Settings.ProjectSettingsStore.TryCapture(Inno.Core.Settings.ProjectSettingId id, string contributorId, System.Collections.Generic.IReadOnlySet<string> declaredDependencies, System.Collections.Generic.IReadOnlySet<string> declaredOverrides, out Inno.Core.Settings.ProjectSettingRecord record)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L234) | Captures one normalized Plugin setting contribution from the project-authored delta. |
| [`bool Inno.Core.Settings.ProjectSettingsStore.TryClone(Inno.Core.Settings.ProjectSettingId id, out Inno.Core.Serialization.ISerializable? setting)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L269) | Creates an isolated editable copy of one effective setting. |
| [`bool Inno.Core.Settings.ProjectSettingsStore.TryCloneComposedDefault(Inno.Core.Settings.ProjectSettingId id, out Inno.Core.Serialization.ISerializable? setting)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L297) | Creates an isolated setting value without the project-authored override. |
| [`bool Inno.Core.Settings.ProjectSettingsStore.TryGet<TSetting>(Inno.Core.Settings.ProjectSettingId id, out TSetting? setting)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L180) | Tries to get an isolated effective setting snapshot from the active extension generation. |
| [`void Inno.Core.Settings.ProjectSettingsStore.ValidateDocument(System.ReadOnlySpan<byte> document)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L456) | Validates one native project settings document without changing active state. |
| [`bool Inno.Core.Settings.ProjectSettingsStore.isInitialized`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L106) | Gets whether project settings are initialized. |
| [`System.Guid Inno.Core.Settings.ProjectSettingsStore.ownerId`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L25) | Gets the unique, non-persistent lifetime identity used with the effective snapshot revision. |
| [`Inno.Core.Settings.ProjectId Inno.Core.Settings.ProjectSettingsStore.projectId`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L90) | Gets the current project namespace. |
| [`long Inno.Core.Settings.ProjectSettingsStore.revision`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L119) | Gets the monotonic revision of the active effective settings snapshot. Runtime extensions may compare this value without registering reload-unsafe static delegates. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingRecord> Inno.Core.Settings.ProjectSettingsStore.unavailableSettings`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L67) | Gets detached neutral contributions whose setting definitions are currently Missing. |
| [`Inno.Core.Settings.ProjectSettingsStore`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L15) | Owns one project's effective settings, contributors, persistence, and generation revision. |

### `Inno.Core.Settings.Settings`

| 当前声明 | 行为 |
| --- | --- |
| [`static TSetting Inno.Core.Settings.Settings.Get<TSetting>(Inno.Core.Settings.ProjectSettingId id)`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L60) | Gets an isolated effective setting from the current extension generation. |
| [`static Inno.Core.Settings.ProjectScopedId Inno.Core.Settings.Settings.QualifyId(string name)`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L30) | Creates one complete project identity from a display or local name. |
| [`static bool Inno.Core.Settings.Settings.TryGet<TSetting>(Inno.Core.Settings.ProjectSettingId id, out TSetting? setting)`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L83) | Tries to get an isolated effective setting from the current extension generation. |
| [`static System.Guid Inno.Core.Settings.Settings.ownerId`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L43) | Gets the current owner's lifetime identity. Pair it with revision when caching an isolated settings snapshot. |
| [`static Inno.Core.Settings.ProjectId Inno.Core.Settings.Settings.projectId`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L19) | Gets the current project namespace used to qualify project-authored logical names. |
| [`static long Inno.Core.Settings.Settings.revision`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L38) | Gets the revision of the settings snapshot active in the current execution context. |
| [`Inno.Core.Settings.Settings`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L14) | Provides script-facing project settings queries through the current runtime execution context. |

### `Inno.Core.Settings.SettingsDocumentStore<TDocument>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.SettingsDocumentStore<TDocument>.SettingsDocumentStore(Inno.Core.IO.IByteDocumentStore document, Inno.Core.Serialization.SerializationRegistry serialization, System.Func<TDocument> createDefault, System.Action<TDocument>? validate = null)`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L38) | Creates a type-safe settings document store. |
| [`byte[] Inno.Core.Settings.SettingsDocumentStore<TDocument>.Capture(TDocument document)`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L116) | Serializes a validated document without changing the file. |
| [`TDocument Inno.Core.Settings.SettingsDocumentStore<TDocument>.Deserialize(System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L132) | Deserializes and validates a native payload without changing the file. |
| [`TDocument Inno.Core.Settings.SettingsDocumentStore<TDocument>.Load()`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L69) | Loads the saved value, or creates a validated default when absent. |
| [`TDocument Inno.Core.Settings.SettingsDocumentStore<TDocument>.LoadRequired()`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L87) | Loads a required saved value. |
| [`void Inno.Core.Settings.SettingsDocumentStore<TDocument>.Restore(System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L157) | Validates and atomically restores a native document payload. |
| [`void Inno.Core.Settings.SettingsDocumentStore<TDocument>.Save(TDocument document)`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L100) | Validates and atomically replaces the complete document. |
| [`string Inno.Core.Settings.SettingsDocumentStore<TDocument>.documentName`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L56) | Gets the source's logical diagnostic name without requiring a filesystem location. |
| [`bool Inno.Core.Settings.SettingsDocumentStore<TDocument>.exists`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L61) | Gets whether the document currently exists. |
| [`Inno.Core.Settings.SettingsDocumentStore<TDocument>`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L15) | Provides validated current-format serialization and atomic persistence for one settings document type. |

### `Inno.Core.Settings.SettingsFileNames`

| 当前声明 | 行为 |
| --- | --- |
| [`const string Inno.Core.Settings.SettingsFileNames.build`](../../src/foundation/core/Inno.Core.Settings/SettingsFileNames.cs#L21) | Gets the authoring build-default document name. |
| [`const string Inno.Core.Settings.SettingsFileNames.editor`](../../src/foundation/core/Inno.Core.Settings/SettingsFileNames.cs#L11) | Gets the machine/editor preference document name. |
| [`const string Inno.Core.Settings.SettingsFileNames.project`](../../src/foundation/core/Inno.Core.Settings/SettingsFileNames.cs#L16) | Gets the runtime project settings document name. |
| [`Inno.Core.Settings.SettingsFileNames`](../../src/foundation/core/Inno.Core.Settings/SettingsFileNames.cs#L6) | Defines the canonical project-root names of the independent settings documents. |

## 项目依赖

- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Collections](Inno.Core.Collections.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.IO](Inno.Core.IO.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Serialization](Inno.Core.Serialization.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Execution](Inno.Core.Execution.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
