# Inno.Content API

[项目入口](../Inno.Content.md)

## 公开 API

| 类型与入口 | 当前语义 |
| --- | --- |
| `ContentKey(string)`、`value`、`ToString()` | 精确大小写的可移植逻辑路径；默认值未赋值。拒绝逃逸、宿主路径语法与歧义路径段 |
| `ContentEntry(key, length, contentHash)` | 不可变条目，包含长度与规范化 SHA-256 |
| `ContentPackDescriptor(contentHash, fileName)` | 完整 Pack 身份；文件名必须与哈希一致 |
| `ContentPackIndex(entries)`、`entries`、`totalLength`、`TryGetEntry(key, out entry)` | 冻结、排序的唯一条目集合；拒绝折叠冲突及目录／文件冲突 |
| `ContentPackIndex.C_INDEX_KEY` | 唯一保留的索引条目 `Content.index`；索引不包含自身 |
| `ContentPackIndexCodec.Encode(index, serialization)`／`Decode(data, serialization)` | 使用当前序列化 generation 写入、读取相同协议，无旧格式分支 |
| `ContentReadLimits(...)` | Pack、单条目、展开总量、索引与条目数的有限预算 |
| `ContentPackReader.Open(pack, descriptor, serialization, limits, cancellationToken)` | 接管可读、可定位流；验证整个 Pack 哈希、索引、Archive 集合及所有 payload |
| `IRuntimeContentStore.descriptor`、`index`、`Acquire(key)`、`Dispose()` | 只读来源；不公开物理路径 |
| `ContentReadLease(entry, openRead, release)`、`entry`、`OpenRead()`、`Dispose()` | Provider 交出租约 pin；每条读取流拥有独立 pin |
| `PackContentStore` | Reader 返回的 owned Archive store；公开面与 store 契约一致 |

参数名称及完整异常说明以源码英文 XML 为准。未知 key 报 `FileNotFoundException`，损坏和预算错误报 `InvalidDataException`，取消报 `OperationCanceledException`；已经关闭的 owner 拒绝新租约。
