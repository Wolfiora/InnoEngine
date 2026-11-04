## 15. 禁止 Legacy 兼容与 Schema Version
- InnoEngine 是自用且始终按当前源码、当前 Project 数据共同演进的引擎。新增或修改功能时不支持旧版文件、旧版 schema、旧字段、旧缓存目录、旧 namespace、旧 API 或旧序列化布局，也不得为它们添加 fallback reader、migration、compatibility alias、former ID、deprecated wrapper 或双写逻辑。
- 持久化模型、Attribute、Importer、Build Processor、History、Workspace、Scene、Prefab、Asset metadata、Catalog 和脚本 manifest 不得引入用于 legacy 适配的 `version`、`schemaVersion`、`formatVersion`、`formerVersion` 等字段。格式发生变化时直接更新当前 writer、reader、测试、文档和当前 Project 数据；旧数据可以明确失效并重新生成。
- 不得因为“未来可能兼容”预留迁移分支。只有用户在具体任务中明确要求导入某一种旧格式时，才可以实现一次性、边界清晰的转换工具；转换逻辑不得进入正常运行路径。
- 删除或重构 API 时同步修改所有调用方，不保留旧 overload、旧 namespace facade、转发类型或 `[Obsolete]` 兼容层，除非用户明确要求保留。
- 以上规则不禁止保障当前运行正确性所需的运行时标识，例如 TypeCache/Assembly generation、并发 revision、change counter、content hash、artifact fingerprint、MVID、job handle generation，以及只用于拒绝损坏或错误格式输入的严格 magic/header。它们不得演变成读取多代 legacy schema 的兼容机制。
- 代码审查或清理包含 `version`、`legacy`、`migration`、`compatibility`、`former`、`deprecated` 等名称的实现时，必须先判断其是否只服务于旧数据/API；如果是，应连同测试和文档一起删除，而不是继续扩展。
