# Inno.Core.Settings

[分类索引](README.md) · [Wiki 首页](../README.md)

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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Settings.IProjectSettingsLookup`

<!-- api: T:Inno.Core.Settings.IProjectSettingsLookup 743c8a0e788fb5b29908f9f3dc980c7b90013b95db297318a275339eebf19889 -->
<!-- api: P:Inno.Core.Settings.IProjectSettingsLookup.ownerId b43ead42e7472196ac7a9ed21b1b5e63ca0b541978bf13aa03704a04d98d8fab -->
<!-- api: P:Inno.Core.Settings.IProjectSettingsLookup.revision 97da921573810d434f001b1f15370ffb461345cdcfeac2c21c032853cbeb94d0 -->
<!-- api: M:Inno.Core.Settings.IProjectSettingsLookup.Get``1(Inno.Core.Settings.ProjectSettingId) 21e441f40fd56b7ac457218b631f657c83f1f4fbda3dcc480fe5333aacf0032e -->
<!-- api: M:Inno.Core.Settings.IProjectSettingsLookup.TryGet``1(Inno.Core.Settings.ProjectSettingId,``0@) f516a6ef4ae69874dbb8195ecc625031d3d4894ff19a4fb190949c0e3eb50b3d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-346ce592b7ab41e6"></a>[`TSetting Inno.Core.Settings.IProjectSettingsLookup.Get<TSetting>(Inno.Core.Settings.ProjectSettingId id) where TSetting : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Settings/IProjectSettingsLookup.cs#L33) | Gets an isolated effective setting from the active extension generation.<br>返回：An independently owned effective setting snapshot.<br>`id`：The stable setting protocol identity. | — | — |
| <a id="api-d3e2f6e4a29426ef"></a>[`bool Inno.Core.Settings.IProjectSettingsLookup.TryGet<TSetting>(Inno.Core.Settings.ProjectSettingId id, out TSetting? setting) where TSetting : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Settings/IProjectSettingsLookup.cs#L52) | Tries to get an isolated effective setting from the active extension generation.<br>返回：true when a compatible setting exists; otherwise, false.<br>`id`：The stable setting protocol identity.<br>`setting`：Receives an independently owned effective snapshot when available. | — | — |
| <a id="api-6c7c9f8de733843a"></a>[`System.Guid Inno.Core.Settings.IProjectSettingsLookup.ownerId { get; }`](../../src/foundation/core/Inno.Core.Settings/IProjectSettingsLookup.cs#L14) | Gets the unique lifetime identity of this settings owner; revisions from different owners are not interchangeable. | — | — |
| <a id="api-d51558205784ed78"></a>[`long Inno.Core.Settings.IProjectSettingsLookup.revision { get; }`](../../src/foundation/core/Inno.Core.Settings/IProjectSettingsLookup.cs#L19) | Gets the monotonic revision of the active effective settings snapshot. | — | — |
| <a id="api-1b3fc87171ce8d21"></a>[`public interface Inno.Core.Settings.IProjectSettingsLookup`](../../src/foundation/core/Inno.Core.Settings/IProjectSettingsLookup.cs#L9) | Defines the read-only effective settings boundary shared by Editor and Player hosts. | — | — |

### `Inno.Core.Settings.ProjectId`

<!-- api: T:Inno.Core.Settings.ProjectId 592c844dd046633ed83fe07c96a6cac4cff88478014a4b9896d68392351af085 -->
<!-- api: M:Inno.Core.Settings.ProjectId.#ctor(System.String) 840421b578070957a48e8e0349b0d1ef0a99f670652e9b86e10b9d8ee21514ba -->
<!-- api: M:Inno.Core.Settings.ProjectId.FromName(System.String) 5b91847790c00ef9af6c520b746f9ff4774245190b542a09d3d106a560ec9934 -->
<!-- api: P:Inno.Core.Settings.ProjectId.value 139b6be9476be1d34e145db95ca9384d4770882c2a20f462a2560a6447f4c6ae -->
<!-- api: P:Inno.Core.Settings.ProjectId.isValid 99a0acf599d25c32abddabbe5fc23caa27cb8f4580bf53d4d3b2dc7bb5dab14d -->
<!-- api: M:Inno.Core.Settings.ProjectId.Qualify(Inno.Core.Settings.ProjectLocalId) 56758c239a619c59fbe7d835f3f11263fb897356a2a08617fe7080dbaaf61301 -->
<!-- api: M:Inno.Core.Settings.ProjectId.ToString 1c0e2a1dc937a2d58ed8c55cc5ffd19e414690f10d5dd696da71a68b10b0503e -->
<!-- api: M:Inno.Core.Settings.ProjectId.op_Inequality(Inno.Core.Settings.ProjectId,Inno.Core.Settings.ProjectId) a7d08d4cfc37ddd43ad0e09551b1a4a8655ae0afc03cccbc81afa8450271f804 -->
<!-- api: M:Inno.Core.Settings.ProjectId.op_Equality(Inno.Core.Settings.ProjectId,Inno.Core.Settings.ProjectId) db30c2fa31a7af1d275d4e30f09edb61cf59eda8df0db310050578671a6a32b5 -->
<!-- api: M:Inno.Core.Settings.ProjectId.GetHashCode 7c330ed36205e6317df3a9899d6629e7fd4a701d20d0f276868c5dadca347e8a -->
<!-- api: M:Inno.Core.Settings.ProjectId.Equals(System.Object) 55aedb2947c91e8670e54f60ca77a313134f3707df304b948dbc6253c784fdaf -->
<!-- api: M:Inno.Core.Settings.ProjectId.Equals(Inno.Core.Settings.ProjectId) 28fc35e75f69be7f2a23af80c6f03aed12a59590783dc13b4be13c8b55cd584c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-228e8807810a4213"></a>[`public Inno.Core.Settings.ProjectId.ProjectId(string value)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L20) | Creates a portable project identifier.<br>`value`：Lowercase ASCII namespace segments. | — | — |
| <a id="api-d6281a11a129dea5"></a>[`public bool Inno.Core.Settings.ProjectId.Equals(Inno.Core.Settings.ProjectId other)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L12) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-ee81da27ad2c45ec"></a>[`public override bool Inno.Core.Settings.ProjectId.Equals(object obj)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L12) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-0cccb4e72ce18161"></a>[`public static Inno.Core.Settings.ProjectId Inno.Core.Settings.ProjectId.FromName(string name)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L37) | Creates a portable initial project identifier from a user-facing project name.<br>返回：A portable initial project identifier.<br>`name`：The project name used to derive the initial namespace. | — | — |
| <a id="api-560abded89e820d2"></a>[`public override int Inno.Core.Settings.ProjectId.GetHashCode()`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L12) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-9459b3d2779eea72"></a>[`public static bool Inno.Core.Settings.ProjectId.operator ==(Inno.Core.Settings.ProjectId left, Inno.Core.Settings.ProjectId right)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L12) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-f3ea9906dd4cb232"></a>[`public static bool Inno.Core.Settings.ProjectId.operator !=(Inno.Core.Settings.ProjectId left, Inno.Core.Settings.ProjectId right)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L12) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-327c556dffdacb62"></a>[`public Inno.Core.Settings.ProjectScopedId Inno.Core.Settings.ProjectId.Qualify(Inno.Core.Settings.ProjectLocalId name)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L84) | Qualifies a local project name beneath this project namespace.<br>返回：The complete project-scoped identity.<br>`name`：The portable local name. | — | — |
| <a id="api-c65490a682cb1bbe"></a>[`public override string Inno.Core.Settings.ProjectId.ToString()`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L92) | Formats the canonical identifier.<br>返回：The canonical identifier text. | — | — |
| <a id="api-047844d1cf72b12c"></a>[`public bool Inno.Core.Settings.ProjectId.isValid { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L73) | Gets whether the value contains a usable identifier. | — | — |
| <a id="api-b101322c97bcc148"></a>[`public string Inno.Core.Settings.ProjectId.value { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L68) | Gets the canonical identifier text. | — | — |
| <a id="api-401681257cf30d35"></a>[`public readonly record struct Inno.Core.Settings.ProjectId : System.IEquatable<Inno.Core.Settings.ProjectId>`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L12) | Identifies one project namespace used to qualify project-authored logical names. | — | — |

### `Inno.Core.Settings.ProjectIdentitySettings`

<!-- api: T:Inno.Core.Settings.ProjectIdentitySettings f3de0bf998899b67e63a7843ff5e7c7b9240d22d252ba0eeb4f6dbc20f91a4b6 -->
<!-- api: F:Inno.Core.Settings.ProjectIdentitySettings.settingProtocolId 3a18c2df8c332452b2b0f3a2ffc2952762f14eca1d52f55f28229854d0f05e1c -->
<!-- api: P:Inno.Core.Settings.ProjectIdentitySettings.settingId bc195d41c75656ee17f79672af04d279f57443ef737e5175f591b819c11612a1 -->
<!-- api: P:Inno.Core.Settings.ProjectIdentitySettings.projectId 798dbc8d3e36b40d651349e24db9b4b1c97a72975c686dee0df10d940f169273 -->
<!-- api: P:Inno.Core.Settings.ProjectIdentitySettings.id 3a4bcf25e57d0ce8b772be806d056f99e4a300f0ad4453ce19e635dffa798056 -->
<!-- api: M:Inno.Core.Settings.ProjectIdentitySettings.Qualify(System.String) 6aa9fb61b0a73fc5c3e6d4225062fb6dbe018b697e7b83b56b16aa0a0a7c56c7 -->
<!-- api: M:Inno.Core.Settings.ProjectIdentitySettings.#ctor 341bc7b4c60f3ff8bbff3378345bd4919295dd74279a399db2de197fcc6f6b69 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1ea517f2564ade3e"></a>[`public const string Inno.Core.Settings.ProjectIdentitySettings.settingProtocolId = "inno.project.identity"`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L265) | Gets the immutable project-setting protocol value used by discovery metadata. | — | — |
| <a id="api-022a997a62a16490"></a>[`public Inno.Core.Settings.ProjectIdentitySettings.ProjectIdentitySettings()`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L260) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-5da1d122153820c7"></a>[`public Inno.Core.Settings.ProjectScopedId Inno.Core.Settings.ProjectIdentitySettings.Qualify(string name)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L298) | Qualifies a display name under the current project identifier.<br>返回：The complete project-scoped identity.<br>`name`：The project-local display name. | — | — |
| <a id="api-1306929530e8430e"></a>[`public Inno.Core.Settings.ProjectId Inno.Core.Settings.ProjectIdentitySettings.id { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L287) | Gets the validated project identifier. | — | — |
| <a id="api-0da70cc19b4e1040"></a>[`public string Inno.Core.Settings.ProjectIdentitySettings.projectId { get; set; }`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L278) | Gets or sets the editable project namespace. | — | — |
| <a id="api-251f16d464830833"></a>[`public static Inno.Core.Settings.ProjectSettingId Inno.Core.Settings.ProjectIdentitySettings.settingId { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L272) | Gets the stable project setting protocol identity. | — | — |
| <a id="api-68a262c4cb1153c6"></a>[`public sealed class Inno.Core.Settings.ProjectIdentitySettings : Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L260) | Stores the editable identity namespace of the current project. | — | — |

### `Inno.Core.Settings.ProjectLocalId`

<!-- api: T:Inno.Core.Settings.ProjectLocalId 673845f8c7a9fcf6f5ac2616a7495168a2801218debf9dfa6bebd635b6b1d801 -->
<!-- api: M:Inno.Core.Settings.ProjectLocalId.#ctor(System.String) 912e7eebab5575732cd781775b22d53452905c21c76104245946ef2f64875b91 -->
<!-- api: P:Inno.Core.Settings.ProjectLocalId.value 864c690134e57e6cf8c729937ab2cca189ecb91611dc356c2ea6988de107b68d -->
<!-- api: M:Inno.Core.Settings.ProjectLocalId.FromName(System.String) 642133bd3ec23f26c6bf40b63cf9cbd9ba8baefdbe89cf7c5c70359fb83985f5 -->
<!-- api: M:Inno.Core.Settings.ProjectLocalId.ToString 753994b3e644060bb3a7ced0567a073d5af62a981eaa117fbbbbcee9564f1e54 -->
<!-- api: M:Inno.Core.Settings.ProjectLocalId.op_Inequality(Inno.Core.Settings.ProjectLocalId,Inno.Core.Settings.ProjectLocalId) 6531b12d072891fb216f378b39024819a5348f485beab592752e830c6701a44d -->
<!-- api: M:Inno.Core.Settings.ProjectLocalId.op_Equality(Inno.Core.Settings.ProjectLocalId,Inno.Core.Settings.ProjectLocalId) 871670c8c36bdf1ff98c731271f21655111f9e88a4047f86ad32c786fc80e1a0 -->
<!-- api: M:Inno.Core.Settings.ProjectLocalId.GetHashCode 659a2113a6df9e7b7da5c7456507e28497da888cb19bea869d61fcce5f26c18d -->
<!-- api: M:Inno.Core.Settings.ProjectLocalId.Equals(System.Object) e15913a487c0734f64121156a6feb2e560a84f89d305e7c439543165bf4fa323 -->
<!-- api: M:Inno.Core.Settings.ProjectLocalId.Equals(Inno.Core.Settings.ProjectLocalId) c77cae8978cf1c5fb6ee6e34153870824b9605cc3b582423fbb92b4564e784ae -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2180077a83881ae9"></a>[`public Inno.Core.Settings.ProjectLocalId.ProjectLocalId(string value)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L132) | Creates a portable local identity.<br>`value`：Lowercase ASCII local namespace segments. | — | — |
| <a id="api-4d15502430d9e5a0"></a>[`public bool Inno.Core.Settings.ProjectLocalId.Equals(Inno.Core.Settings.ProjectLocalId other)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L124) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-5bcae41a70f1cdb2"></a>[`public override bool Inno.Core.Settings.ProjectLocalId.Equals(object obj)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L124) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-d8d3887c24d09d83"></a>[`public static Inno.Core.Settings.ProjectLocalId Inno.Core.Settings.ProjectLocalId.FromName(string name)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L154) | Creates a deterministic portable local identity from a display name.<br>返回：The normalized local identity.<br>`name`：The user-facing name. | — | — |
| <a id="api-7fc7c5d67025e1a7"></a>[`public override int Inno.Core.Settings.ProjectLocalId.GetHashCode()`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L124) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-d7791a6852e5f9aa"></a>[`public static bool Inno.Core.Settings.ProjectLocalId.operator ==(Inno.Core.Settings.ProjectLocalId left, Inno.Core.Settings.ProjectLocalId right)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L124) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-fdc1d6631ecd1722"></a>[`public static bool Inno.Core.Settings.ProjectLocalId.operator !=(Inno.Core.Settings.ProjectLocalId left, Inno.Core.Settings.ProjectLocalId right)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L124) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-f54005fd036c53ca"></a>[`public override string Inno.Core.Settings.ProjectLocalId.ToString()`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L193) | Formats the canonical local identity.<br>返回：The canonical local identity text. | — | — |
| <a id="api-5ebebf0b5e5c4e43"></a>[`public string Inno.Core.Settings.ProjectLocalId.value { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L143) | Gets the canonical local identity text. | — | — |
| <a id="api-9c34f18e543c590b"></a>[`public readonly record struct Inno.Core.Settings.ProjectLocalId : System.IEquatable<Inno.Core.Settings.ProjectLocalId>`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L124) | Stores the project-independent portion of one project-scoped identity. | — | — |

### `Inno.Core.Settings.ProjectScopedId`

<!-- api: T:Inno.Core.Settings.ProjectScopedId 02132f7d1f1d540c84239a12ecaad6a28e796e2145bc8b12bd7efb358a152d6b -->
<!-- api: M:Inno.Core.Settings.ProjectScopedId.#ctor(Inno.Core.Settings.ProjectId,Inno.Core.Settings.ProjectLocalId) 2d5e5468cff8bc88f6d8813e89adcdcb54c20212d69221216ae76ac015df3b01 -->
<!-- api: P:Inno.Core.Settings.ProjectScopedId.projectId 36fd78440304a05343865f826f8b1a3fdf8887608143ee962c93c566b982bfb8 -->
<!-- api: P:Inno.Core.Settings.ProjectScopedId.name 53d2ab0c2a833b5d73e25b9ce23c4f47dd3bf966a1976be44e49bd342471a7a1 -->
<!-- api: P:Inno.Core.Settings.ProjectScopedId.value 6dd7b115a938e5b91a5450d5e54b0d58dd9cdf6b6c163a2a939ce7389c76ae5f -->
<!-- api: M:Inno.Core.Settings.ProjectScopedId.ToString b671709c3b563c3b84b3a58404b9a88f674018d04253dacf9cfd5bf6b3d4639a -->
<!-- api: M:Inno.Core.Settings.ProjectScopedId.op_Inequality(Inno.Core.Settings.ProjectScopedId,Inno.Core.Settings.ProjectScopedId) 1d1f7e4d1f494b6d959f171771cd5f31ce63868cc373a0f200b2e08db26a90ee -->
<!-- api: M:Inno.Core.Settings.ProjectScopedId.op_Equality(Inno.Core.Settings.ProjectScopedId,Inno.Core.Settings.ProjectScopedId) 3e3351094a6d9c681ab5f459d5ce115c2da1dd7cc88b7431169557e688c44a94 -->
<!-- api: M:Inno.Core.Settings.ProjectScopedId.GetHashCode af57eedf412ef36a0563d77de3f9a8d96416948d42f758f91887e616407435e3 -->
<!-- api: M:Inno.Core.Settings.ProjectScopedId.Equals(System.Object) 0d92fa0d54ab8c0dab8abfca65247be983121e277cdde837485e0df1a647a1ce -->
<!-- api: M:Inno.Core.Settings.ProjectScopedId.Equals(Inno.Core.Settings.ProjectScopedId) 815ded8f4c1735bbf279b4693e5f705edfc5564e95c2a152ce6e578997a023c0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-817fce5c24113614"></a>[`public Inno.Core.Settings.ProjectScopedId.ProjectScopedId(Inno.Core.Settings.ProjectId projectId, Inno.Core.Settings.ProjectLocalId name)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L219) | Creates a qualified project identity.<br>`projectId`：The current project namespace.<br>`name`：The stable local identity. | — | — |
| <a id="api-a673f76728769025"></a>[`public bool Inno.Core.Settings.ProjectScopedId.Equals(Inno.Core.Settings.ProjectScopedId other)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L208) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-ea009c97707763c5"></a>[`public override bool Inno.Core.Settings.ProjectScopedId.Equals(object obj)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L208) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-3240edf6044b8ab8"></a>[`public override int Inno.Core.Settings.ProjectScopedId.GetHashCode()`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L208) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-d11b6e3f859e4005"></a>[`public static bool Inno.Core.Settings.ProjectScopedId.operator ==(Inno.Core.Settings.ProjectScopedId left, Inno.Core.Settings.ProjectScopedId right)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L208) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-656aa111d8fcc8c3"></a>[`public static bool Inno.Core.Settings.ProjectScopedId.operator !=(Inno.Core.Settings.ProjectScopedId left, Inno.Core.Settings.ProjectScopedId right)`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L208) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-cfff116bb7e2ce14"></a>[`public override string Inno.Core.Settings.ProjectScopedId.ToString()`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L252) | Formats the canonical qualified identity.<br>返回：The projectId.name representation. | — | — |
| <a id="api-ca1796425a4072f3"></a>[`public Inno.Core.Settings.ProjectLocalId Inno.Core.Settings.ProjectScopedId.name { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L239) | Gets the project-independent local identity. | — | — |
| <a id="api-29582e919b03ef34"></a>[`public Inno.Core.Settings.ProjectId Inno.Core.Settings.ProjectScopedId.projectId { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L234) | Gets the project namespace. | — | — |
| <a id="api-ef7fac555d4460e7"></a>[`public string Inno.Core.Settings.ProjectScopedId.value { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L244) | Gets the canonical projectId.name representation. | — | — |
| <a id="api-1470da8e0a18c581"></a>[`public readonly record struct Inno.Core.Settings.ProjectScopedId : System.IEquatable<Inno.Core.Settings.ProjectScopedId>`](../../src/foundation/core/Inno.Core.Settings/ProjectIdentity.cs#L208) | Combines a mutable project namespace with a stable project-independent local identity. | — | — |

### `Inno.Core.Settings.ProjectSettingComposer`

<!-- api: T:Inno.Core.Settings.ProjectSettingComposer 979bca92226836bbbf2890282470bb2dbb6c60ead7d7aa102b88b863b94ce2d3 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingComposer.#ctor 1b6082dd7fe9d0eab31fc9b069d2697aae2cd7f8d40cd6fcaaae3851f4e5bb0e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-43d8a550e77bbbb6"></a>[`protected Inno.Core.Settings.ProjectSettingComposer.ProjectSettingComposer()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L134) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-a05d0706a3a23b82"></a>[`public abstract class Inno.Core.Settings.ProjectSettingComposer`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L134) | Defines the non-generic base for a protocol-owned project setting composer. | — | — |

### `Inno.Core.Settings.ProjectSettingComposer`2`

<!-- api: T:Inno.Core.Settings.ProjectSettingComposer`2 c3b2229c83ea08d8af2228f6d38f24205a7623212742535adbe2f5e4497d1a9d -->
<!-- api: M:Inno.Core.Settings.ProjectSettingComposer`2.CaptureContribution(`0,`0) 82e9665c65a26647e5bc3692b1b0756e8d36183d4e2b363e80e14c9ffc63fe28 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingComposer`2.IsEmpty(`1) 4a569d9bae47be3170a593cf71794b59e09713132466749008bb82ed3df1161c -->
<!-- api: M:Inno.Core.Settings.ProjectSettingComposer`2.Compose(`0,System.Collections.Generic.IReadOnlyList{Inno.Core.Settings.ProjectSettingContribution{`1}}) 4bba4a81669144172837da202f3408abcbe90c7c4e2be5adfabeeec8bea33f56 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingComposer`2.#ctor 5fc3e144434a688ef536790b6fb29e78e5bd6debd97c356c4e0704e10d5384c7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d1baf26cf4939775"></a>[`protected Inno.Core.Settings.ProjectSettingComposer<TSetting, TContribution>.ProjectSettingComposer()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L163) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-698bb549b538aaed"></a>[`protected abstract TContribution Inno.Core.Settings.ProjectSettingComposer<TSetting, TContribution>.CaptureContribution(TSetting baseline, TSetting value)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L179) | Captures the contribution introduced by the supplied project setting value.<br>返回：A detached neutral contribution.<br>`baseline`：The value composed from lower-precedence contributors.<br>`value`：The complete authored value. | — | — |
| <a id="api-0a3bbdc0f4954766"></a>[`protected abstract void Inno.Core.Settings.ProjectSettingComposer<TSetting, TContribution>.Compose(TSetting target, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingContribution<TContribution>> contributions)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L204) | Composes dependency-ordered Plugin deltas and the optional final project delta into a host default.<br>`target`：The newly created host default to mutate into the effective value.<br>`contributions`：Decoded contributions in deterministic precedence order. | — | — |
| <a id="api-ed31dad718efbb0b"></a>[`protected abstract bool Inno.Core.Settings.ProjectSettingComposer<TSetting, TContribution>.IsEmpty(TContribution contribution)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L193) | Gets whether a captured contribution contains no semantic operation.<br>返回：true when the record should be omitted.<br>`contribution`：The contribution to inspect. | — | — |
| <a id="api-02177eda4a3a4117"></a>[`public abstract class Inno.Core.Settings.ProjectSettingComposer<TSetting, TContribution> where TSetting : class, Inno.Core.Serialization.ISerializable where TContribution : class, Inno.Core.Serialization.ISerializable : Inno.Core.Settings.ProjectSettingComposer`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L163) | Lets one setting protocol define deterministic delta capture and multi-contributor composition. | — | — |

### `Inno.Core.Settings.ProjectSettingComposerAttribute`

<!-- api: T:Inno.Core.Settings.ProjectSettingComposerAttribute 14cfb305fae8da5750bdd74d53e406a1549770b8d319b128299d309775f22b0a -->
<!-- api: M:Inno.Core.Settings.ProjectSettingComposerAttribute.#ctor(System.String) 8669f1f1296b72188d438d52076db11d629003d8a0a2117b8a4b51cc19a66427 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingComposerAttribute.settingId 383cbac147f93a348268a4b1b290f52abf40393bb896d87749c5ca16ba2fb670 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-df6e6a0a23fded98"></a>[`public Inno.Core.Settings.ProjectSettingComposerAttribute.ProjectSettingComposerAttribute(string settingId)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L21) | Creates a composer declaration for one stable setting protocol.<br>`settingId`：The setting protocol composed by the attributed type. | — | — |
| <a id="api-39987c3a1b38d6cb"></a>[`public Inno.Core.Settings.ProjectSettingId Inno.Core.Settings.ProjectSettingComposerAttribute.settingId { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L30) | Gets the stable setting protocol composed by the attributed type. | — | — |
| <a id="api-0ffdb7e0fd4a3d6e"></a>[`public sealed class Inno.Core.Settings.ProjectSettingComposerAttribute : System.Attribute`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L13) | Declares the protocol-owned composer used to combine contributions for one project setting. Settings without a composer retain the default whole-value replacement behavior. | — | — |

### `Inno.Core.Settings.ProjectSettingContribution`1`

<!-- api: T:Inno.Core.Settings.ProjectSettingContribution`1 20c3a6c4b80cbf9ea0f8a455882028c439a32fd32cfb1008d68132a95550b73a -->
<!-- api: P:Inno.Core.Settings.ProjectSettingContribution`1.context 63c9b9fb826075d2a2492d3279433e26bfa369c5aeca54879bd20abcc130b2b8 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingContribution`1.value 5c15f8af057cb42d94ceed3f47cb7a04e52cad46024bffeae96550f50022caee -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-462d0b3bc7c92bc6"></a>[`public Inno.Core.Settings.ProjectSettingContributionContext Inno.Core.Settings.ProjectSettingContribution<TContribution>.context { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L123) | Gets the ownership and dependency context for this contribution. | — | — |
| <a id="api-177d3c0614f4efc3"></a>[`public TContribution Inno.Core.Settings.ProjectSettingContribution<TContribution>.value { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L128) | Gets the decoded protocol-owned contribution value. | — | — |
| <a id="api-36e02a41adb13bbe"></a>[`public sealed class Inno.Core.Settings.ProjectSettingContribution<TContribution> where TContribution : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L109) | Provides one decoded contribution and its immutable composition context. | — | — |

### `Inno.Core.Settings.ProjectSettingContributionContext`

<!-- api: T:Inno.Core.Settings.ProjectSettingContributionContext be531588ed257fa8a2a46a88eec825722e70c289f59cf47e9ffa19ec37f00ff6 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingContributionContext.contributorId 6d99d2998750f86bc68e4336aabbe5c99ddf8775eb7a58bcbe90afdffb203810 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingContributionContext.source 5d27d4b988df5176e18780bf555fd791610fdc4913907614f79f25ca992e67b3 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingContributionContext.isProject 89b7f467e25fc119056405493b0417ea848605890981dbe568959a5123f6e6ae -->
<!-- api: M:Inno.Core.Settings.ProjectSettingContributionContext.CanOverride(System.String) 233456c42d4e50302a4092d29f54d4bcb90a2d0fc13f84b8305b1ddb7cc84586 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2be447fbd606effd"></a>[`public bool Inno.Core.Settings.ProjectSettingContributionContext.CanOverride(string ownerId)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L94) | Gets whether this contribution may explicitly replace data owned by another contributor.<br>返回：true for project data, or when the Plugin both depends on and explicitly overrides ownerId.<br>`ownerId`：The stable owner whose data would be replaced. | — | — |
| <a id="api-a2d36dc8f8bf33b4"></a>[`public string Inno.Core.Settings.ProjectSettingContributionContext.contributorId { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L72) | Gets the stable Plugin ID, or project for the project-authored contribution. | — | — |
| <a id="api-6bde92fcf8474084"></a>[`public bool Inno.Core.Settings.ProjectSettingContributionContext.isProject { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L82) | Gets whether this is the project-authored highest-precedence contribution. | — | — |
| <a id="api-171ddc2808d6a1f4"></a>[`public Inno.Core.Settings.ProjectSettingContributionSource Inno.Core.Settings.ProjectSettingContributionContext.source { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L77) | Gets the contribution source. | — | — |
| <a id="api-3d77cd9c50932fde"></a>[`public sealed class Inno.Core.Settings.ProjectSettingContributionContext`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L52) | Exposes ownership and dependency information while a protocol-owned composer combines one contribution. | — | — |

### `Inno.Core.Settings.ProjectSettingContributionSource`

<!-- api: T:Inno.Core.Settings.ProjectSettingContributionSource 5e3701e64a3ced236c8321712e1b814e2514bdcc56961f9a9702d913672e61d3 -->
<!-- api: F:Inno.Core.Settings.ProjectSettingContributionSource.Plugin ef0a43e3d868f501b943b6731e92f5d9d82549bf8ff482e24541fc2604063b73 -->
<!-- api: F:Inno.Core.Settings.ProjectSettingContributionSource.Project 049a8ed3fdc4f9a8a3f41b79d2997b48d6c49ba1ded062526e3ef56df61cd66f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c0c3f3ab11ada60a"></a>[`Inno.Core.Settings.ProjectSettingContributionSource.Plugin = 0`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L41) | The contribution is a default supplied by an activated Plugin. | — | — |
| <a id="api-507386cdfed8b6f8"></a>[`Inno.Core.Settings.ProjectSettingContributionSource.Project = 1`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L46) | The contribution is the project-authored delta with highest precedence. | — | — |
| <a id="api-10129bf85e2235bc"></a>[`public enum Inno.Core.Settings.ProjectSettingContributionSource : int`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingComposition.cs#L36) | Identifies where one setting contribution originated. | — | — |

### `Inno.Core.Settings.ProjectSettingDefinitionAttribute`

<!-- api: T:Inno.Core.Settings.ProjectSettingDefinitionAttribute b41303ae105ff6c93398fbba9a96dd674d7e6272b8282d1eaf1e280516ef45c8 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingDefinitionAttribute.#ctor(System.String,System.Boolean) 2bbc77253bb0d65af4671d6eb775c91e9f2a0158401ebf5b1f6b5b5019a38002 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingDefinitionAttribute.id edef209397de44f43e1851a508075ad10185625ff9e7aeb516a47deac4a8ed94 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingDefinitionAttribute.allowPluginContributions 5d952cdf8eb777925d0c6dff57cc43a39ade3ef1ef28048b6669a97759ba92cd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5d3def9af670778e"></a>[`public Inno.Core.Settings.ProjectSettingDefinitionAttribute.ProjectSettingDefinitionAttribute(string id, bool allowPluginContributions = true)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L59) | Creates a project setting declaration.<br>`id`：Globally stable setting identifier.<br>`allowPluginContributions`：Whether Plugin packages may contribute values to this protocol. | — | — |
| <a id="api-be469d4c65024a1a"></a>[`public bool Inno.Core.Settings.ProjectSettingDefinitionAttribute.allowPluginContributions { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L76) | Gets whether Plugin packages may contribute values to this protocol. | — | — |
| <a id="api-662ad2858db0d9a2"></a>[`public string Inno.Core.Settings.ProjectSettingDefinitionAttribute.id { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L71) | Gets the globally stable setting identifier. | — | — |
| <a id="api-7f6e807f6978a3fc"></a>[`public sealed class Inno.Core.Settings.ProjectSettingDefinitionAttribute : System.Attribute`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L48) | Declares a reloadable settings type under one stable protocol identity. | — | — |

### `Inno.Core.Settings.ProjectSettingId`

<!-- api: T:Inno.Core.Settings.ProjectSettingId 1a097f138ef84a7b5fff2396d9decbe896387c9e72378f966d116158aa0625b0 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingId.#ctor(System.String) 59672488752b7906bbbce21b813a030ce64de010d915946f1c53c7e05e8ccb64 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingId.value 05ddcbaf06b44bf3e98c50250089e4c65a53c44decdbd4ef8de8a5d3144c12d6 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingId.isValid 97885ae0fe4f0c5051ba150c0dc94c70074fe7ca9a06df59540d6dd68d58a582 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingId.ToString 68b5f128add5b933259e711e6896c89661fd910fd14e471b3fd2e38b61f19e47 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingId.op_Inequality(Inno.Core.Settings.ProjectSettingId,Inno.Core.Settings.ProjectSettingId) 8f0249c5e36de2254535f88bfb7bc1b930794b6dde79b07555b08e89fca80575 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingId.op_Equality(Inno.Core.Settings.ProjectSettingId,Inno.Core.Settings.ProjectSettingId) d6b4131a5d644a6299c8ad5396b3017870e13074297828d303fc96576181f825 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingId.GetHashCode 42961ccd455745c4d27a4e41c371de6657777f94c480899f42081aa07b98071a -->
<!-- api: M:Inno.Core.Settings.ProjectSettingId.Equals(System.Object) 0ccebf967d04f1be42ac6f487a427f8d48347c465de73282145e27d0712e8738 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingId.Equals(Inno.Core.Settings.ProjectSettingId) e8c2121108c0b71f5d47a39e1dbfa266be08335104c9fc98029da6c1c3ee3870 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-803c7c41df8f55af"></a>[`public Inno.Core.Settings.ProjectSettingId.ProjectSettingId(string value)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L19) | Creates a project setting identifier.<br>`value`：Globally stable setting value. | — | — |
| <a id="api-75cb0df42a30de01"></a>[`public readonly bool Inno.Core.Settings.ProjectSettingId.Equals(Inno.Core.Settings.ProjectSettingId other)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L11) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-f70aedcb4fcc8ce0"></a>[`public override readonly bool Inno.Core.Settings.ProjectSettingId.Equals(object obj)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L11) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-7a9e09807089b1fa"></a>[`public override readonly int Inno.Core.Settings.ProjectSettingId.GetHashCode()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L11) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-68df6bc0c82a9b2f"></a>[`public static bool Inno.Core.Settings.ProjectSettingId.operator ==(Inno.Core.Settings.ProjectSettingId left, Inno.Core.Settings.ProjectSettingId right)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L11) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-a85f154b9cd15864"></a>[`public static bool Inno.Core.Settings.ProjectSettingId.operator !=(Inno.Core.Settings.ProjectSettingId left, Inno.Core.Settings.ProjectSettingId right)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L11) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-9f0257176a44dafb"></a>[`public override readonly string Inno.Core.Settings.ProjectSettingId.ToString()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L41) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-4361f7df8cb51baa"></a>[`public readonly bool Inno.Core.Settings.ProjectSettingId.isValid { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L33) | Gets whether the identifier has a usable value. | — | — |
| <a id="api-18203c3defa25f99"></a>[`public string Inno.Core.Settings.ProjectSettingId.value { get; set; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L28) | Gets or sets the globally stable setting value. | — | — |
| <a id="api-9d7ddf66eac22a28"></a>[`public record struct Inno.Core.Settings.ProjectSettingId : System.IEquatable<Inno.Core.Settings.ProjectSettingId>`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L11) | Identifies one host-neutral project setting protocol. | — | — |

### `Inno.Core.Settings.ProjectSettingRecord`

<!-- api: T:Inno.Core.Settings.ProjectSettingRecord 992f6eca3752fe117afffe06e655af022618f5e08f67925a63612cb1faa4ddcf -->
<!-- api: M:Inno.Core.Settings.ProjectSettingRecord.#ctor(Inno.Core.Settings.ProjectSettingId,System.Guid,System.ReadOnlySpan{System.Byte}) 1e4c4b80da05c9b32ca7896d7a98a7c6a8c6bd96fcaea911dcbf26fa92883d32 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingRecord.id 18cf0e8da86c09fbf210681aab2cc7050167ee7917ab306aa889f15cb1081127 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingRecord.stableTypeId 1aabf38c3c916c2c85397a899ce04620f18150a078b9a7654fffc431fbac4200 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingRecord.propertyData 5c93b7836f3e3fd9f06715b00eb27cc6243d6f4992a63ae6f1e971f018dbd3fd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3693f82c9b256316"></a>[`public Inno.Core.Settings.ProjectSettingRecord.ProjectSettingRecord(Inno.Core.Settings.ProjectSettingId id, System.Guid stableTypeId, System.ReadOnlySpan<byte> propertyData)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L96) | Creates a setting record.<br>`id`：Stable setting identity.<br>`stableTypeId`：Stable settings type identity.<br>`propertyData`：Composer-owned contribution bytes, or complete property bytes for replacement settings. | — | — |
| <a id="api-097d4cad71de21db"></a>[`public Inno.Core.Settings.ProjectSettingId Inno.Core.Settings.ProjectSettingRecord.id { get; set; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L113) | Gets or sets the stable setting identity. | — | — |
| <a id="api-3769ac2cbcfd7da3"></a>[`public byte[] Inno.Core.Settings.ProjectSettingRecord.propertyData { get; set; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L123) | Gets or sets the composer-owned payload or default complete-property payload. | — | — |
| <a id="api-69226b733f024859"></a>[`public System.Guid Inno.Core.Settings.ProjectSettingRecord.stableTypeId { get; set; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L118) | Gets or sets the stable settings type identity. | — | — |
| <a id="api-8008ee4a91284a0e"></a>[`public struct Inno.Core.Settings.ProjectSettingRecord`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L82) | Stores one protocol-owned neutral setting contribution for persistence and composition. | — | — |

### `Inno.Core.Settings.ProjectSettings`

<!-- api: T:Inno.Core.Settings.ProjectSettings 728873b8a48811e3ff51f59a459578243dc7659f39ad03a4d882318d772cae3f -->
<!-- api: M:Inno.Core.Settings.ProjectSettings.#ctor(Inno.Core.IO.IByteDocumentStore,Inno.Extensibility.Types.TypeCatalog,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Settings.ProjectId,Inno.Core.Serialization.SerializationContext,System.Collections.Generic.IReadOnlyList{Inno.Core.Settings.ProjectSettingsContributor}) 456ea40019e5fb63a8cded2412e6f12388da371da33298e050b89dc94a5b89dc -->
<!-- api: M:Inno.Core.Settings.ProjectSettings.Get``1(Inno.Core.Settings.ProjectSettingId) 5d194741c7130cc868a477a042c3188787126fbabfcc953f556954cbf936ec23 -->
<!-- api: M:Inno.Core.Settings.ProjectSettings.TryGet``1(Inno.Core.Settings.ProjectSettingId,``0@) 5c61561e9f6f7014621179ba9e7b58e7e022c80a49561b4b255ccc2479a9d114 -->
<!-- api: M:Inno.Core.Settings.ProjectSettings.TryCapture(Inno.Core.Settings.ProjectSettingId,System.String,System.Collections.Generic.IReadOnlySet{System.String},System.Collections.Generic.IReadOnlySet{System.String},System.Collections.Generic.IReadOnlyList{Inno.Core.Settings.ProjectSettingsContributor},Inno.Core.Settings.ProjectSettingRecord@) 0af63d4bfe430ddf4062487006aa3e7760f23354bc92a98f2c1ccdf9fabe5141 -->
<!-- api: M:Inno.Core.Settings.ProjectSettings.HasProjectOverride(Inno.Core.Settings.ProjectSettingId) 8230232f33185c9f78328f471063a87f7f7331dae1132f43db70c432324ff95f -->
<!-- api: M:Inno.Core.Settings.ProjectSettings.TryClone(Inno.Core.Settings.ProjectSettingId,Inno.Core.Serialization.ISerializable@) cf8654586e69f89d29c450885d0dd45d27794fe0dc21850f6d4d7026b7008580 -->
<!-- api: M:Inno.Core.Settings.ProjectSettings.TryCloneComposedDefault(Inno.Core.Settings.ProjectSettingId,System.Collections.Generic.IReadOnlyList{Inno.Core.Settings.ProjectSettingsContributor},Inno.Core.Serialization.ISerializable@) 8bdf0301b729dd8bdbe6a41b52ecb2f9e416eb4626379e4651c231240996338a -->
<!-- api: M:Inno.Core.Settings.ProjectSettings.CaptureDocument 472e6dafd5828bbe895996098823e3bb5e6b2004d6dfc4d018d0b7d41a6ed067 -->
<!-- api: M:Inno.Core.Settings.ProjectSettings.Rebuild(System.Collections.Generic.IReadOnlyList{Inno.Core.Settings.ProjectSettingsContributor},System.Boolean) ee0003ec45c3d92093c31a288b0794182b3457d23e716105b199d0a7a6f5e1f9 -->
<!-- api: M:Inno.Core.Settings.ProjectSettings.SetProjectOverride(Inno.Core.Settings.ProjectSettingId,Inno.Core.Serialization.ISerializable,System.Collections.Generic.IReadOnlyList{Inno.Core.Settings.ProjectSettingsContributor}) b9b155eee744c67ba97c10b08f9f45999d9e3ec3a4c2a8d2a32742ee97ea4704 -->
<!-- api: M:Inno.Core.Settings.ProjectSettings.ApplyProjectOverrides(System.Collections.Generic.IReadOnlyDictionary{Inno.Core.Settings.ProjectSettingId,Inno.Core.Serialization.ISerializable},System.Collections.Generic.IReadOnlySet{Inno.Core.Settings.ProjectSettingId},System.Collections.Generic.IReadOnlyList{Inno.Core.Settings.ProjectSettingsContributor}) 859bd7d5583fff52161b04a4135ac87b21bc32962d59ac464f1dee17ed475527 -->
<!-- api: M:Inno.Core.Settings.ProjectSettings.RestoreDocument(System.ReadOnlySpan{System.Byte},System.Collections.Generic.IReadOnlyList{Inno.Core.Settings.ProjectSettingsContributor}) ac0548890874dc345cb8c176c1dd040d46c7e82c7fbe0ccf38e1c0decbc7cdc3 -->
<!-- api: M:Inno.Core.Settings.ProjectSettings.Dispose 267f35d74b54db4f39e35297dbe1e8cf1f612c58b4f72887f074c7c02a10e874 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3f807e0d1f2663f8"></a>[`public Inno.Core.Settings.ProjectSettings.ProjectSettings(Inno.Core.IO.IByteDocumentStore documentStore, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Settings.ProjectId defaultProjectId, Inno.Core.Serialization.SerializationContext serializationContext, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor>? contributors = null)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L51) | Loads one project settings document and builds the initial effective snapshot.<br>`documentStore`：The project document source; read-only deployments cannot persist project overrides.<br>`types`：The type catalog that owns project setting definitions and composers.<br>`serialization`：The serialization registry used for settings documents and contribution payloads.<br>`defaultProjectId`：The project namespace used when the document has no authored identity override.<br>`contributors`：Dependency-ordered default contributors.<br>`serializationContext`：The owner's complete reference resolver context, used for defaults, contributions and returned snapshots. | — | `System.ArgumentNullException`：A required service or the owner's serialization context is null. |
| <a id="api-934ba0481d6db3b4"></a>[`public bool Inno.Core.Settings.ProjectSettings.ApplyProjectOverrides(System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Settings.ProjectSettingId, Inno.Core.Serialization.ISerializable> values, System.Collections.Generic.IReadOnlySet<Inno.Core.Settings.ProjectSettingId>? resets, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L417) | Applies multiple project contributions and removals as one atomic document update.<br>返回：true when the project document changed.<br>`values`：Complete authored values keyed by stable setting identity.<br>`resets`：Setting identities whose project-authored contributions are removed.<br>`contributors`：Dependency-ordered defaults used to rebuild effective values. | — | — |
| <a id="api-83c2035d34fe1a82"></a>[`public byte[] Inno.Core.Settings.ProjectSettings.CaptureDocument()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L308) | Captures the native project contribution document.<br>返回：A newly owned native document payload. | — | — |
| <a id="api-f124fe9a80720d3b"></a>[`public void Inno.Core.Settings.ProjectSettings.Dispose()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L494) | Releases registry snapshots and generation-local setting objects. | — | — |
| <a id="api-dd4088548d239caf"></a>[`public TSetting Inno.Core.Settings.ProjectSettings.Get<TSetting>(Inno.Core.Settings.ProjectSettingId id) where TSetting : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L96) | Gets an isolated snapshot of a current-generation setting.<br>返回：An independently owned snapshot of the effective current-generation value.<br>`id`：Stable setting identifier. | — | `System.Collections.Generic.KeyNotFoundException`：Thrown when no definition exists.<br>`System.InvalidCastException`：Thrown when the definition does not implement the requested contract. |
| <a id="api-be4d5b5cc28a151a"></a>[`public bool Inno.Core.Settings.ProjectSettings.HasProjectOverride(Inno.Core.Settings.ProjectSettingId id)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L235) | Gets whether the project document explicitly contributes to one setting protocol.<br>返回：true when a project contribution record exists.<br>`id`：Stable setting identifier. | — | — |
| <a id="api-10cd86bf6346ae7b"></a>[`public void Inno.Core.Settings.ProjectSettings.Rebuild(System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors, bool allowUnresolvedContributions = false)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L323) | Atomically rebuilds effective settings for a new extension generation.<br>`contributors`：Dependency-ordered default contributors.<br>`allowUnresolvedContributions`：Whether contributions awaiting Plugin type activation are retained but skipped. | — | — |
| <a id="api-c3c61b34ff87d77c"></a>[`public void Inno.Core.Settings.ProjectSettings.RestoreDocument(System.ReadOnlySpan<byte> document, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L481) | Atomically restores a native project settings document.<br>`document`：Native document bytes.<br>`contributors`：Dependency-ordered defaults used to rebuild effective values. | — | — |
| <a id="api-de89a361b308ac3f"></a>[`public void Inno.Core.Settings.ProjectSettings.SetProjectOverride(Inno.Core.Settings.ProjectSettingId id, Inno.Core.Serialization.ISerializable value, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L391) | Persists one project-authored semantic contribution and rebuilds the effective value.<br>`id`：Stable setting identifier.<br>`value`：Current generation setting value.<br>`contributors`：Dependency-ordered defaults used to rebuild. | — | — |
| <a id="api-3b839f008d4b74b6"></a>[`public bool Inno.Core.Settings.ProjectSettings.TryCapture(Inno.Core.Settings.ProjectSettingId id, string contributorId, System.Collections.Generic.IReadOnlySet<string> declaredDependencies, System.Collections.Generic.IReadOnlySet<string> declaredOverrides, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors, out Inno.Core.Settings.ProjectSettingRecord record)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L164) | Captures and validates the project-authored delta as one prospective Plugin contribution.<br>返回：true when the project document contains an effective semantic delta.<br>`id`：Stable setting identifier.<br>`contributorId`：Stable identity of the Plugin being exported.<br>`declaredDependencies`：Direct dependency Plugin IDs declared by the exported Plugin.<br>`declaredOverrides`：Dependency Plugin IDs whose owned values may be replaced.<br>`contributors`：All currently active dependency-ordered contributors.<br>`record`：Receives the normalized semantic Plugin contribution. | — | `System.InvalidOperationException`：Thrown when a replacement setting depends on an undeclared contributor, or when the authored delta is not permitted by the declared dependency and override ownership. |
| <a id="api-332b4fce7fbdc4f7"></a>[`public bool Inno.Core.Settings.ProjectSettings.TryClone(Inno.Core.Settings.ProjectSettingId id, out Inno.Core.Serialization.ISerializable? setting)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L253) | Creates an isolated editable copy of one effective setting.<br>返回：true when the setting is defined.<br>`id`：Stable setting identifier.<br>`setting`：Receives a generation-local editable copy. | — | — |
| <a id="api-c156868d54ee7c46"></a>[`public bool Inno.Core.Settings.ProjectSettings.TryCloneComposedDefault(Inno.Core.Settings.ProjectSettingId id, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors, out Inno.Core.Serialization.ISerializable? setting)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L285) | Creates an isolated copy composed without the project-authored contribution.<br>返回：true when the setting is defined.<br>`id`：Stable setting identifier.<br>`contributors`：Dependency-ordered Plugin default contributors.<br>`setting`：Receives the composed default value. | — | — |
| <a id="api-d859ad1685969898"></a>[`public bool Inno.Core.Settings.ProjectSettings.TryGet<TSetting>(Inno.Core.Settings.ProjectSettingId id, out TSetting? setting) where TSetting : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L120) | Tries to get an isolated snapshot of a current-generation setting.<br>返回：true when a compatible definition exists.<br>`id`：Stable setting identifier.<br>`setting`：Receives an independently owned effective snapshot. | — | — |
| <a id="api-318292fbf4f76738"></a>[`public sealed class Inno.Core.Settings.ProjectSettings : System.IDisposable`](../../src/foundation/core/Inno.Core.Settings/ProjectSettings.cs#L16) | Composes setting defaults, Plugin contributions, and project overrides atomically. | — | — |

### `Inno.Core.Settings.ProjectSettingsContributor`

<!-- api: T:Inno.Core.Settings.ProjectSettingsContributor 592d2a8d3d684478057a9d94d5d547ba76a575b452a2fb281d2945ddf029a2e9 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsContributor.#ctor(System.String,System.Collections.Generic.IEnumerable{System.String},System.Collections.Generic.IEnumerable{System.String},System.Collections.Generic.IEnumerable{Inno.Core.Settings.ProjectSettingRecord}) 1889eacdf12e348932fdf0f342daf9566081fc4a45c18b5e9ca824ffe7f4a4fa -->
<!-- api: P:Inno.Core.Settings.ProjectSettingsContributor.id dc6b0e914ede0954bf49f578dc8e909363c17a4a53c4f05dd1952a950a0cddf6 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingsContributor.dependencies a0d26825fe2866e80bf87d0217900901f30a428dabe80c5847b7247d1b6f7219 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingsContributor.overrides 762c68fda0a23baa2e7dc40e5947b2bb0dbe5d2e4a5c0ce85e46f293fea01a26 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingsContributor.settings 7ac76dcd62f5f1546ec3047afd6e0a9212db7ca253cc89e70b4842fba0b02559 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-151bd0d92eddf735"></a>[`public Inno.Core.Settings.ProjectSettingsContributor.ProjectSettingsContributor(string id, System.Collections.Generic.IEnumerable<string> dependencies, System.Collections.Generic.IEnumerable<string> overrides, System.Collections.Generic.IEnumerable<Inno.Core.Settings.ProjectSettingRecord> settings)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L160) | Creates one settings contributor.<br>`id`：Stable contributor identity.<br>`dependencies`：Contributors that must precede this contributor.<br>`overrides`：Dependencies whose defaults may be explicitly replaced.<br>`settings`：Default setting contributions. | — | — |
| <a id="api-1825de11c11fcdcc"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Core.Settings.ProjectSettingsContributor.dependencies { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L184) | Gets contributors that must precede this contributor. | — | — |
| <a id="api-f9b999b7a030b6cd"></a>[`public string Inno.Core.Settings.ProjectSettingsContributor.id { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L179) | Gets the stable contributor identity. | — | — |
| <a id="api-c0348ac83c93076e"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Core.Settings.ProjectSettingsContributor.overrides { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L189) | Gets dependencies whose defaults may be replaced. | — | — |
| <a id="api-aac779f3d6dfd466"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingRecord> Inno.Core.Settings.ProjectSettingsContributor.settings { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L194) | Gets a detached read-only contribution snapshot, including independently owned payload bytes. | — | — |
| <a id="api-34f6535854d32230"></a>[`public sealed class Inno.Core.Settings.ProjectSettingsContributor`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L141) | Describes one dependency-ordered provider of default setting values. | — | — |

### `Inno.Core.Settings.ProjectSettingsDocument`

<!-- api: T:Inno.Core.Settings.ProjectSettingsDocument 2beb946cff2993187bc7ab15adaf0669a700f1ac3b57d142acd26cc795994e4a -->
<!-- api: P:Inno.Core.Settings.ProjectSettingsDocument.overrides 3e29d83b372adedd5247cb19b49be36b4bb75b80f368af5c46a29e22a02184ae -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsDocument.#ctor 204c591afedb783a4b078139547b43950af4e73fa87c7a0ec6d17e02432ca636 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-10aba3dff68843cc"></a>[`public Inno.Core.Settings.ProjectSettingsDocument.ProjectSettingsDocument()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L129) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-5625364deded6ef8"></a>[`public Inno.Core.Settings.ProjectSettingRecord[] Inno.Core.Settings.ProjectSettingsDocument.overrides { get; set; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L135) | Gets or sets project-authored protocol contributions. | — | — |
| <a id="api-8c6ee1068c8ae02b"></a>[`public sealed class Inno.Core.Settings.ProjectSettingsDocument : Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingContracts.cs#L129) | Stores project-authored setting contributions in one native document. | — | — |

### `Inno.Core.Settings.ProjectSettingsExecutionContext`

<!-- api: T:Inno.Core.Settings.ProjectSettingsExecutionContext 3d519070c263f3c104f1293521632686cc7a2c4691120bcf64669682ff573eda -->
<!-- api: P:Inno.Core.Settings.ProjectSettingsExecutionContext.current 388c0c8383b3afdde2777e7f47441f52cdcd8bddfbab53a887f39f03b34c2424 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsExecutionContext.EnterScope(Inno.Core.Settings.IProjectSettingsLookup) 72bca524e410187d6cbd6230c1af5cefcfc06c6f8db05c40747559774c93ec3d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6792bb19c641bbb8"></a>[`public static System.IDisposable Inno.Core.Settings.ProjectSettingsExecutionContext.EnterScope(Inno.Core.Settings.IProjectSettingsLookup settings)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsExecutionContext.cs#L38) | Binds a settings lookup until the returned strict last-in-first-out scope is disposed.<br>返回：A strict last-in-first-out execution scope owned by the caller.<br>`settings`：The host-owned settings lookup to expose to script-facing operations. | — | `System.ArgumentNullException`：Thrown when settings is null. |
| <a id="api-c055d8707a8e2c50"></a>[`public static Inno.Core.Settings.IProjectSettingsLookup Inno.Core.Settings.ProjectSettingsExecutionContext.current { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsExecutionContext.cs#L24) | Gets the settings lookup bound to the current asynchronous execution context. | — | `System.InvalidOperationException`：Thrown when no settings lookup is active for the caller. |
| <a id="api-e01d080313ab9e06"></a>[`public static class Inno.Core.Settings.ProjectSettingsExecutionContext`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsExecutionContext.cs#L14) | Binds one host-owned settings lookup to the current asynchronous script execution context. | This type is a composition boundary for Editor and Player hosts. Game code should use Inno.Core.Settings.Settings instead of managing execution scopes directly. | — |

### `Inno.Core.Settings.ProjectSettingsStore`

<!-- api: T:Inno.Core.Settings.ProjectSettingsStore f9089e85f5436eefbfb16a8a0070715bb477b8885558c8f201bba0a90aef2480 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingsStore.ownerId 8c4df5e98b80b5e1acc7df766f8c408a5a3810c9a8b9999c78973a4248294410 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.#ctor(Inno.Core.IO.IByteDocumentStore,Inno.Extensibility.Types.TypeCatalog,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Settings.ProjectId,Inno.Core.Serialization.SerializationContext) 5d7dd12b89d1231b20f2f1403df14a3a8e20c3170239ebbbdcc2c459ffd2300f -->
<!-- api: P:Inno.Core.Settings.ProjectSettingsStore.unavailableSettings 57e933ca6b956b6c3983f20d05a05757e435374861f4d99c4e0f2675bfd998b0 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.CreateReloadChange 4ba65563e9a344ff2c20a8396326d894d97f5f0f6e90fd34e2555ed1a5ffa681 -->
<!-- api: P:Inno.Core.Settings.ProjectSettingsStore.projectId 26900f272bcbff69d674f33939b0e040b5524287d92f131802bc41cd75e32f7e -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.QualifyId(System.String) b45d07c79829a469af83b63cfbf32d936bdfe7125d4d22115b50ea6c12ada48c -->
<!-- api: P:Inno.Core.Settings.ProjectSettingsStore.isInitialized ec2937865f9df0bf4ccc6f5bfdf73f71c0781221ada75b72b3bbdab65a89026f -->
<!-- api: P:Inno.Core.Settings.ProjectSettingsStore.revision b68fd633a233aa2e4f879ebd0502833cdb6d57fafff7f5bb0648c6d4383c8e19 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.EnterExecutionScope 842647391234caea187c90786d1171d593077190bce57c3ea59be43aad168bec -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.Get``1(Inno.Core.Settings.ProjectSettingId) 6c05a15d0a024dfd5e51ac7c6001a26de34641a9ad3259b02e5063c34aaae329 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.TryGet``1(Inno.Core.Settings.ProjectSettingId,``0@) 50d9c24d78924dd27c451deba13d7bdd951ad14e5e335e051a7aeb7a3a891f20 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.HasProjectOverride(Inno.Core.Settings.ProjectSettingId) 91430752395d6352ceb8298f4d5809d9c593e78dc9f8679e450b718629f18166 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.TryCapture(Inno.Core.Settings.ProjectSettingId,System.String,System.Collections.Generic.IReadOnlySet{System.String},System.Collections.Generic.IReadOnlySet{System.String},Inno.Core.Settings.ProjectSettingRecord@) 950d167e2a16e0b11f3912be3ae24e1876e81b8f37543cda28af2ddc1242a09b -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.TryClone(Inno.Core.Settings.ProjectSettingId,Inno.Core.Serialization.ISerializable@) 591ac7d4cbafbdcdce73e2485a81f58daedf98b668230a1279d55e1b4c33ef87 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.TryCloneComposedDefault(Inno.Core.Settings.ProjectSettingId,Inno.Core.Serialization.ISerializable@) 547725d0a041b90c651c4c69086fad3d63c4a451351beea7feb9895989b5edaf -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.CaptureDocument 101fec709696b54af4b0f1847bd73b1e3691d656cbd1f65c3b92d8502d0e084b -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.Rebuild(System.Collections.Generic.IReadOnlyList{Inno.Core.Settings.ProjectSettingsContributor}) 1031b606ab1a2009156594aa77cb45a3ec5d8d7b7e37bc6ac9de7bfc0da7278e -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.SetContributors(System.Collections.Generic.IReadOnlyList{Inno.Core.Settings.ProjectSettingsContributor}) 49ab1af6f10e32e13416492761c87b58d4f68d8dbbd3f964533cd826b3b1e626 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.RebuildCurrent(System.Boolean) f5e130d21bcf0e6744b9bd86f26169a849e0d15313b939e5f4143051b6dfdce9 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.SetProjectOverride(Inno.Core.Settings.ProjectSettingId,Inno.Core.Serialization.ISerializable,System.Collections.Generic.IReadOnlyList{Inno.Core.Settings.ProjectSettingsContributor}) 9fa30946a461da4ab985adbc87910e804249d4dbd470d2478d0f249d4f4a5146 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.ApplyProjectOverrides(System.Collections.Generic.IReadOnlyDictionary{Inno.Core.Settings.ProjectSettingId,Inno.Core.Serialization.ISerializable},System.Collections.Generic.IReadOnlySet{Inno.Core.Settings.ProjectSettingId}) 8efd80b07eb6797fba03710977647f3905a8e18413d8c416e4febf297f588685 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.RestoreDocument(System.ReadOnlySpan{System.Byte}) 67a8e5ad4677115cab1956e71c3f9ea78589784ad2ff7014d77eaf82b46cc3a4 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.ValidateDocument(System.ReadOnlySpan{System.Byte}) 6317ced952de6c39da7a2c7c27fe7df6520b438c9879ce61e6e9f528bd944ca4 -->
<!-- api: M:Inno.Core.Settings.ProjectSettingsStore.Dispose 06b8cbb16472854d95b652fac8b7326a3358aa4eb9068983580eb2a3c7e106c7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-310a052b5fd36e54"></a>[`public Inno.Core.Settings.ProjectSettingsStore.ProjectSettingsStore(Inno.Core.IO.IByteDocumentStore documentStore, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Settings.ProjectId defaultProjectId, Inno.Core.Serialization.SerializationContext serializationContext)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L48) | Creates a project settings store from one type and serialization generation owner.<br>`documentStore`：The borrowed document boundary; writable authoring and read-only runtime sources use the same protocol.<br>`types`：The type catalog that owns setting definitions and composers.<br>`serialization`：The serialization registry used for documents and contribution payloads.<br>`defaultProjectId`：The project namespace used until an authored identity override is saved.<br>`serializationContext`：The owner's complete resolver context; settings never construct a partial reference context themselves. | — | `System.ArgumentNullException`：Thrown when a service dependency is null. |
| <a id="api-c6423f071e8ae57e"></a>[`public bool Inno.Core.Settings.ProjectSettingsStore.ApplyProjectOverrides(System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Settings.ProjectSettingId, Inno.Core.Serialization.ISerializable> values, System.Collections.Generic.IReadOnlySet<Inno.Core.Settings.ProjectSettingId>? resets = null)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L420) | Applies a native batch of project-authored overrides and removals.<br>返回：true when the native document changed.<br>`values`：Setting values keyed by stable protocol identity.<br>`resets`：Setting identities whose project overrides are removed. | — | — |
| <a id="api-a66f8fde071a031d"></a>[`public byte[] Inno.Core.Settings.ProjectSettingsStore.CaptureDocument()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L320) | Captures the current native project override document.<br>返回：A newly owned native document payload. | — | — |
| <a id="api-95ca754f8cf7e7ce"></a>[`public Inno.Extensibility.Reload.IGenerationChange Inno.Core.Settings.ProjectSettingsStore.CreateReloadChange()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L85) | Creates a five-phase settings change for the host's shared generation transaction.<br>返回：An owner-thread change that captures neutral effective values before publication, rebuilds against candidate types, and restores exact previous values and contributors without writing the project document on rollback. | — | — |
| <a id="api-72d1df3aeb586fec"></a>[`public void Inno.Core.Settings.ProjectSettingsStore.Dispose()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L463) | Shuts down settings and releases generation-scoped values. | — | — |
| <a id="api-a16ad823d976c6f4"></a>[`public System.IDisposable Inno.Core.Settings.ProjectSettingsStore.EnterExecutionScope()`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L137) | Binds this settings store to the current asynchronous script execution context.<br>返回：A strict last-in-first-out execution scope owned by the caller. | — | `System.ObjectDisposedException`：Thrown after this settings store has been disposed. |
| <a id="api-6e0ebe889f4e99cc"></a>[`public TSetting Inno.Core.Settings.ProjectSettingsStore.Get<TSetting>(Inno.Core.Settings.ProjectSettingId id) where TSetting : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L158) | Gets an isolated effective setting snapshot from the active extension generation.<br>返回：An independently owned effective setting snapshot.<br>`id`：Stable setting protocol identity. | — | — |
| <a id="api-71e8582dc877192a"></a>[`public bool Inno.Core.Settings.ProjectSettingsStore.HasProjectOverride(Inno.Core.Settings.ProjectSettingId id)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L207) | Gets whether the active project document explicitly overrides one setting protocol.<br>返回：true when the project owns an override record.<br>`id`：Stable setting protocol identity. | — | — |
| <a id="api-1db8d4a9f9b33b9e"></a>[`public Inno.Core.Settings.ProjectScopedId Inno.Core.Settings.ProjectSettingsStore.QualifyId(string name)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L101) | Qualifies one local name under the current project namespace.<br>返回：The canonical projectId.name identity.<br>`name`：The local display name. | — | — |
| <a id="api-5e8e829d74e16ec6"></a>[`public void Inno.Core.Settings.ProjectSettingsStore.Rebuild(System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L333) | Rebuilds settings for one dependency-ordered extension generation.<br>`contributors`：Host and Plugin default contributions. | — | — |
| <a id="api-4ddb72e7c5a0c055"></a>[`public void Inno.Core.Settings.ProjectSettingsStore.RebuildCurrent(bool allowUnresolvedContributions = false)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L371) | Rebuilds effective settings after the active type catalog changes.<br>`allowUnresolvedContributions`：Whether Plugin contributions awaiting type activation are skipped. | — | — |
| <a id="api-78c053e43290ab88"></a>[`public void Inno.Core.Settings.ProjectSettingsStore.RestoreDocument(System.ReadOnlySpan<byte> document)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L441) | Restores a previously captured native project settings document.<br>`document`：Native project settings bytes. | — | — |
| <a id="api-760d35fa39b6b4c2"></a>[`public void Inno.Core.Settings.ProjectSettingsStore.SetContributors(System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L352) | Publishes dependency-ordered default contributors for the next current-generation rebuild without constructing setting instances from types that may still be awaiting assembly activation.<br>`contributors`：Complete dependency-ordered contributor snapshot. | — | — |
| <a id="api-dcc2a1fc54573fb0"></a>[`public void Inno.Core.Settings.ProjectSettingsStore.SetProjectOverride(Inno.Core.Settings.ProjectSettingId id, Inno.Core.Serialization.ISerializable value, System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingsContributor> contributors)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L393) | Persists one project-authored override through the native settings document.<br>`id`：Stable setting protocol identity.<br>`value`：Current generation setting value.<br>`contributors`：Current dependency-ordered default contributions. | — | — |
| <a id="api-d134bfb051b7a41e"></a>[`public bool Inno.Core.Settings.ProjectSettingsStore.TryCapture(Inno.Core.Settings.ProjectSettingId id, string contributorId, System.Collections.Generic.IReadOnlySet<string> declaredDependencies, System.Collections.Generic.IReadOnlySet<string> declaredOverrides, out Inno.Core.Settings.ProjectSettingRecord record)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L235) | Captures one normalized Plugin setting contribution from the project-authored delta.<br>返回：true when the project document contains an effective semantic delta.<br>`id`：Stable setting protocol identity.<br>`contributorId`：Stable identity of the Plugin being exported.<br>`declaredDependencies`：Direct dependency Plugin IDs declared by the exported Plugin.<br>`declaredOverrides`：Dependency Plugin IDs whose owned values may be replaced.<br>`record`：Receives the normalized semantic contribution payload. | — | — |
| <a id="api-0d51f718cd43d35a"></a>[`public bool Inno.Core.Settings.ProjectSettingsStore.TryClone(Inno.Core.Settings.ProjectSettingId id, out Inno.Core.Serialization.ISerializable? setting)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L270) | Creates an isolated editable copy of one effective setting.<br>返回：true when the setting is defined.<br>`id`：Stable setting protocol identity.<br>`setting`：Receives an isolated native setting object. | — | — |
| <a id="api-a8dfbf69aec0369c"></a>[`public bool Inno.Core.Settings.ProjectSettingsStore.TryCloneComposedDefault(Inno.Core.Settings.ProjectSettingId id, out Inno.Core.Serialization.ISerializable? setting)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L298) | Creates an isolated setting value without the project-authored override.<br>返回：true when the setting is defined.<br>`id`：Stable setting protocol identity.<br>`setting`：Receives the composed host and Plugin default value. | — | — |
| <a id="api-bb46bd23cf42151f"></a>[`public bool Inno.Core.Settings.ProjectSettingsStore.TryGet<TSetting>(Inno.Core.Settings.ProjectSettingId id, out TSetting? setting) where TSetting : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L180) | Tries to get an isolated effective setting snapshot from the active extension generation.<br>返回：true when a compatible setting exists.<br>`id`：Stable setting protocol identity.<br>`setting`：Receives an independently owned effective snapshot when available. | — | — |
| <a id="api-22e62fef2a1f1e91"></a>[`public void Inno.Core.Settings.ProjectSettingsStore.ValidateDocument(System.ReadOnlySpan<byte> document)`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L457) | Validates one native project settings document without changing active state.<br>`document`：Native project settings bytes. | — | — |
| <a id="api-5d519896a4dd3182"></a>[`public bool Inno.Core.Settings.ProjectSettingsStore.isInitialized { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L106) | Gets whether project settings are initialized. | — | — |
| <a id="api-9a9f88404866638a"></a>[`public System.Guid Inno.Core.Settings.ProjectSettingsStore.ownerId { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L25) | Gets the unique, non-persistent lifetime identity used with the effective snapshot revision. | — | — |
| <a id="api-f53261b65d20c3a1"></a>[`public Inno.Core.Settings.ProjectId Inno.Core.Settings.ProjectSettingsStore.projectId { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L90) | Gets the current project namespace. | — | — |
| <a id="api-35f658eaa039cf0a"></a>[`public long Inno.Core.Settings.ProjectSettingsStore.revision { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L119) | Gets the monotonic revision of the active effective settings snapshot. Runtime extensions may compare this value without registering reload-unsafe static delegates. | — | — |
| <a id="api-ea8eb1a872e71d50"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Settings.ProjectSettingRecord> Inno.Core.Settings.ProjectSettingsStore.unavailableSettings { get; }`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L68) | Gets detached neutral contributions whose setting definitions are currently Missing. | — | — |
| <a id="api-4305c3a765b0eada"></a>[`public sealed class Inno.Core.Settings.ProjectSettingsStore : System.IDisposable, Inno.Core.Settings.IProjectSettingsLookup`](../../src/foundation/core/Inno.Core.Settings/ProjectSettingsStore.cs#L15) | Owns one project's effective settings, contributors, persistence, and generation revision. | — | — |

### `Inno.Core.Settings.Settings`

<!-- api: T:Inno.Core.Settings.Settings f7c700f708e820ac5b02bef45d814af68f1af0e27ca7db20fff593535327bee9 -->
<!-- api: P:Inno.Core.Settings.Settings.projectId f741461b69cc5fd1df1b04d4aae2cbf22f03f1115bf9c105ddc9ea25bbbcd62e -->
<!-- api: M:Inno.Core.Settings.Settings.QualifyId(System.String) 9c43335e7e3115c98b866ac03f85f935613f7135c1509a7a953be6af6ba3a5ae -->
<!-- api: P:Inno.Core.Settings.Settings.revision 8e97d9777c56cfe27ce93c7d729058b3806010793b17686de7f883f851bf61d7 -->
<!-- api: P:Inno.Core.Settings.Settings.ownerId 5f8d8e3d3d5a2645aa9f7cde1ddb1e870f4614eaf052b562a7257211d08bdf18 -->
<!-- api: M:Inno.Core.Settings.Settings.Get``1(Inno.Core.Settings.ProjectSettingId) 46198432784a25324a33f61da627486151aedee19f41dc054ddfe86b624fb06d -->
<!-- api: M:Inno.Core.Settings.Settings.TryGet``1(Inno.Core.Settings.ProjectSettingId,``0@) 5cda07d5166925aee495688770c91295896aa7980be4718c7641e13f897c4140 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6dc5d0da471a26f8"></a>[`public static TSetting Inno.Core.Settings.Settings.Get<TSetting>(Inno.Core.Settings.ProjectSettingId id) where TSetting : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L60) | Gets an isolated effective setting from the current extension generation.<br>返回：An independently owned effective setting snapshot.<br>`id`：The stable setting protocol identity. | — | `System.InvalidOperationException`：Thrown when no project settings lookup is active or the setting is unavailable. |
| <a id="api-61c68cc1d78932f6"></a>[`public static Inno.Core.Settings.ProjectScopedId Inno.Core.Settings.Settings.QualifyId(string name)`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L30) | Creates one complete project identity from a display or local name.<br>返回：The canonical projectId.name identity.<br>`name`：The project-local name. | — | — |
| <a id="api-70d863d582facaef"></a>[`public static bool Inno.Core.Settings.Settings.TryGet<TSetting>(Inno.Core.Settings.ProjectSettingId id, out TSetting? setting) where TSetting : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L83) | Tries to get an isolated effective setting from the current extension generation.<br>返回：true when a compatible setting exists; otherwise, false.<br>`id`：The stable setting protocol identity.<br>`setting`：Receives an independently owned effective snapshot when available. | — | `System.InvalidOperationException`：Thrown when no project settings lookup is active for the caller. |
| <a id="api-25f788459d2d8f55"></a>[`public static System.Guid Inno.Core.Settings.Settings.ownerId { get; }`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L43) | Gets the current owner's lifetime identity. Pair it with revision when caching an isolated settings snapshot. | — | — |
| <a id="api-a4e24d899ed45a37"></a>[`public static Inno.Core.Settings.ProjectId Inno.Core.Settings.Settings.projectId { get; }`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L19) | Gets the current project namespace used to qualify project-authored logical names. | — | — |
| <a id="api-aaf42fdab9f0a098"></a>[`public static long Inno.Core.Settings.Settings.revision { get; }`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L38) | Gets the revision of the settings snapshot active in the current execution context. | — | `System.InvalidOperationException`：Thrown when no project settings lookup is active for the caller. |
| <a id="api-0b7550a8dccf3dfd"></a>[`public static class Inno.Core.Settings.Settings`](../../src/foundation/core/Inno.Core.Settings/Settings.cs#L14) | Provides script-facing project settings queries through the current runtime execution context. | This façade owns no settings state. Engine and Editor infrastructure should depend on an explicit Inno.Core.Settings.IProjectSettingsLookup instance. | — |

### `Inno.Core.Settings.SettingsDocumentStore`1`

<!-- api: T:Inno.Core.Settings.SettingsDocumentStore`1 4e25b9b0f1f03afa3104550019bb17c9bf913ec71ebfaade84c0197fdb563da3 -->
<!-- api: M:Inno.Core.Settings.SettingsDocumentStore`1.#ctor(Inno.Core.IO.IByteDocumentStore,Inno.Core.Serialization.SerializationRegistry,System.Func{`0},System.Action{`0}) c8a670fb6d4b6734ccac8c52751a1a7e2c6ab961c0d4a56bee139758c544d19a -->
<!-- api: P:Inno.Core.Settings.SettingsDocumentStore`1.documentName c4dc8fbd9c7efc1ac41708a14b702053a36605c85285e1305368e7c33956f8ab -->
<!-- api: P:Inno.Core.Settings.SettingsDocumentStore`1.exists 9f142a9e418197e6410942048d004c23c404252c59498cec202d89f29d7e8905 -->
<!-- api: M:Inno.Core.Settings.SettingsDocumentStore`1.Load 125e90cfa10e755bdb2d568e88fbaf296f703baf93c48285811db32cb580da8a -->
<!-- api: M:Inno.Core.Settings.SettingsDocumentStore`1.LoadRequired edf81ea63136ede799de072652c491c417d9d2416167c2480aadb905aacdb5b2 -->
<!-- api: M:Inno.Core.Settings.SettingsDocumentStore`1.Save(`0) a6c0859efdef38787789887b5f5b75d9cabdc3cea4a69348a386352f34e94dfc -->
<!-- api: M:Inno.Core.Settings.SettingsDocumentStore`1.Capture(`0) 885c6f787f2c5a56815c374c16c761ce428e307b6c6b50ba5e5b3aa263b18778 -->
<!-- api: M:Inno.Core.Settings.SettingsDocumentStore`1.Deserialize(System.ReadOnlySpan{System.Byte}) 5ac75b481a7bf11438f753bbb33189e2e6070e1c302d78116cc744389aa4aa3d -->
<!-- api: M:Inno.Core.Settings.SettingsDocumentStore`1.Restore(System.ReadOnlySpan{System.Byte}) b3311b0cfb6fbca936812c4550b5ed151352ea598b2f345d7a7609e4bc6cc02b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8f1dfc0ebae97755"></a>[`public Inno.Core.Settings.SettingsDocumentStore<TDocument>.SettingsDocumentStore(Inno.Core.IO.IByteDocumentStore document, Inno.Core.Serialization.SerializationRegistry serialization, System.Func<TDocument> createDefault, System.Action<TDocument>? validate = null)`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L38) | Creates a type-safe settings document store.<br>`document`：The borrowed document boundary; read-only sources explicitly reject writes.<br>`serialization`：The active serialization registry.<br>`createDefault`：Creates a newly owned value when the document does not exist.<br>`validate`：Validates one deserialized or candidate document. | — | — |
| <a id="api-75e6ceb318979e80"></a>[`public byte[] Inno.Core.Settings.SettingsDocumentStore<TDocument>.Capture(TDocument document)`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L116) | Serializes a validated document without changing the file.<br>返回：A newly owned native payload.<br>`document`：The document to capture. | — | — |
| <a id="api-0dbefc470f33e9b6"></a>[`public TDocument Inno.Core.Settings.SettingsDocumentStore<TDocument>.Deserialize(System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L132) | Deserializes and validates a native payload without changing the file.<br>返回：A newly owned current-format document.<br>`data`：The native payload. | — | — |
| <a id="api-c1c35db90a13f6da"></a>[`public TDocument Inno.Core.Settings.SettingsDocumentStore<TDocument>.Load()`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L69) | Loads the saved value, or creates a validated default when absent.<br>返回：A newly owned current-format document. | — | — |
| <a id="api-9aa1469f19c41e5a"></a>[`public TDocument Inno.Core.Settings.SettingsDocumentStore<TDocument>.LoadRequired()`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L87) | Loads a required saved value.<br>返回：A newly owned current-format document. | — | — |
| <a id="api-a52cb2420429e857"></a>[`public void Inno.Core.Settings.SettingsDocumentStore<TDocument>.Restore(System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L157) | Validates and atomically restores a native document payload.<br>`data`：The native payload. | — | — |
| <a id="api-be6aaef7f3d96c00"></a>[`public void Inno.Core.Settings.SettingsDocumentStore<TDocument>.Save(TDocument document)`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L100) | Validates and atomically replaces the complete document.<br>`document`：The complete candidate document. | — | — |
| <a id="api-e9634e232e9d29b2"></a>[`public string Inno.Core.Settings.SettingsDocumentStore<TDocument>.documentName { get; }`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L56) | Gets the source's logical diagnostic name without requiring a filesystem location. | — | — |
| <a id="api-a6a093044a8e74a3"></a>[`public bool Inno.Core.Settings.SettingsDocumentStore<TDocument>.exists { get; }`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L61) | Gets whether the document currently exists. | — | — |
| <a id="api-813837915dfa05cb"></a>[`public sealed class Inno.Core.Settings.SettingsDocumentStore<TDocument> where TDocument : class, Inno.Core.Serialization.ISerializable`](../../src/foundation/core/Inno.Core.Settings/SettingsDocumentStore.cs#L15) | Provides validated current-format serialization and atomic persistence for one settings document type. | — | — |

### `Inno.Core.Settings.SettingsFileNames`

<!-- api: T:Inno.Core.Settings.SettingsFileNames b3eec227d090acb31d0212da6c833ef0b19eeb3551d5686c18cea883cc87d1ed -->
<!-- api: F:Inno.Core.Settings.SettingsFileNames.editor 72a6a735f34905d345f9659d4bcda2190029c2b7d39a095dbdabac3bf53435b6 -->
<!-- api: F:Inno.Core.Settings.SettingsFileNames.project cb6bf028d1120a5c43b75a82ad9aab6952974ead92463f021f2a15bcbc422668 -->
<!-- api: F:Inno.Core.Settings.SettingsFileNames.build 6deec18f3a6413cf7d8efb3a3c1a6df618fd8d00f53f3aa91d35f3b51236c52a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-33df9e8591ee99b7"></a>[`public const string Inno.Core.Settings.SettingsFileNames.build = "Settings.Build.inno"`](../../src/foundation/core/Inno.Core.Settings/SettingsFileNames.cs#L21) | Gets the authoring build-default document name. | — | — |
| <a id="api-8fc9aa0e2e0b6982"></a>[`public const string Inno.Core.Settings.SettingsFileNames.editor = "Settings.Editor.inno"`](../../src/foundation/core/Inno.Core.Settings/SettingsFileNames.cs#L11) | Gets the machine/editor preference document name. | — | — |
| <a id="api-7233442d39c027ad"></a>[`public const string Inno.Core.Settings.SettingsFileNames.project = "Settings.Project.inno"`](../../src/foundation/core/Inno.Core.Settings/SettingsFileNames.cs#L16) | Gets the runtime project settings document name. | — | — |
| <a id="api-233f9bcbbeeb513a"></a>[`public static class Inno.Core.Settings.SettingsFileNames`](../../src/foundation/core/Inno.Core.Settings/SettingsFileNames.cs#L6) | Defines the canonical project-root names of the independent settings documents. | — | — |

## 项目依赖

- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Collections](Inno.Core.Collections.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.IO](Inno.Core.IO.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
