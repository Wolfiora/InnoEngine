# 开发规则

本目录只保存修改仓库时的要求。产品设计、原理、用法和使用契约见 [docs](../../docs/README.md)；任务进展和历史证据见 [records](../records/README.md)。同一套规则适用于人和 AI。

用仓库根的 `tests/ci/Test-AgentRules.ps1 -Paths <仓库相对路径> -ShowInstructions` 发现完整单元。`-Rules` 显式补选单元，`-Behaviors` 补选行为标签；`-Stage` 分阶段读取。全部权威路径、来源、适用条件和关联要求由 [coverage.json](coverage.json) 登记，不维护第二张路由表。

行为标签表达本次工作，例如 `new-boundary`、`public-api`、`ownership`、`serialization`、`reload`、`native`、`documentation`、`rules`、`skills`、`records` 和 `delivery`。同一单元可以被路径、行为或依赖同时选中；只输出一份完整正文并保留所有选中理由。

| 主题 | 权威文件 |
| --- | --- |
| 架构、边界、复用及项目归属 | [architecture](architecture.md) |
| 命名、XML、排版、异常及并发 | [coding](coding.md) |
| 文档、API、示例与内容边界 | [documentation](documentation.md) |
| 测试、编译、完成与审查 | [validation](validation.md) |
| 提交、PR 与通知 | [delivery](delivery.md) |
| Editor | [editor](domains/editor.md) |
| 身份、Missing 与热重载 | [identity-reload](domains/identity-reload.md) |
| 脚本 | [scripting](domains/scripting.md) |
| Plugin | [plugins](domains/plugins.md) |
| 渲染与帧 | [rendering](domains/rendering.md) |
| 构建、Native 与平台 | [build-native](domains/build-native.md) |

规则以完整章节为读取单元；文件合并不意味着任务必须读取整份文件。覆盖检查同时核对 241 条原规则和补充规范的完整原文，不能以更新目标哈希代替语义保留。迁移只调整权威路径与加载方式，不放宽工程要求。

经用户明确要求新增的条款，可在原单元末尾以 `<!-- approved-addition -->` 分隔，在该单元的 `approvedAddition` 中登记批准依据及新增正文的 SHA-256（LF 换行并去除首尾空白）。加载器仍逐字核对历史正文，单独核对新增正文，再完整输出两者；该机制只允许追加，不允许删改原规则，也不能省略新增内容的登记。
