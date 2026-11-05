# Inno.Core.Settings

[分类索引](README.md) · [Wiki 首页](../../README.md)

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

## API 参考

[完整声明、行为与调用约束](Inno.Core.Settings/api.md)

<!-- api-include: Inno.Core.Settings/api.md -->

## 项目依赖

- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Collections](Inno.Core.Collections.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.IO](Inno.Core.IO.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
