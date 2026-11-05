# Inno.Core.Graphs API

[项目入口](../Inno.Core.Graphs.md)

## 公开 API

| API | 作用 |
| --- | --- |
| `GraphDocument` | 增删节点/边、查询节点、保存中立 metadata。 |
| `GraphNodeRecord` | 保存 definition ID、位置和稳定 property 序列化 bytes。 |
| `GraphEdgeRecord` / `GraphEndpoint` | 保存 output 到 input 的稳定连接。 |
| `GraphSerializedValue` | 通过共同 SerializationRegistry 按类型读写中立 bytes；不是独立 JSON 协议。 |
| `GraphNodeDefinition` | reload-scoped 节点定义与动态端口扩展点。 |
| `IGraphNodeDefinitionResolver` | 通过 Stable ID 查询当前 generation 候选快照。 |
| `IGraphTypeConversion` | 声明有方向的隐式类型转换。 |
| `GraphValidator.Validate` | 生成确定顺序的结构化诊断。 |

`GraphNodeDefinition` 不使用 marker Attribute。静态节点由拥有该图语义的领域 Registry 按基类或接口发现；数据驱动节点由领域 `IGraphNodeDefinitionResolver` 根据当前 generation 构造并解析。定义自身的 `id` 是唯一身份来源，不能再通过 Attribute 维护第二份 ID。
