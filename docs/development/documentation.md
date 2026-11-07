# 文档组织与本地查询

[开发指南](README.md) · [文档首页](../README.md) · [工具契约](tooling/Inno.Tooling.Architecture.md)

`docs/` 是人和 AI 共用的产品知识。设计、原理、使用教程与调用边界放在唯一项目入口或其同名分片；开发要求属于 `.agents/rulebook/`，实施历史属于 `.agents/records/`。每个事实只在一个权威位置维护，其余页面链接过去。

## 项目页面

先解释用途与职责，再说明使用、设计、实现和调用契约，最后进入 API。短项目可以合并章节；重要能力必须回答谁创建、谁借用、在哪个线程使用、何时失效、失败后保留什么以及谁释放。内部机制不能冒充公开承诺。

长页保留 `<Project>.md` 入口，细节放在同名目录。API 表使用 `api-include` 纳入原覆盖门禁；现有 Native `.Api` 分页仍有效。源码符号与签名标记用于核对精确身份，不证明行为说明已完整审查。

## 本地查询

先正常构建一次 `entry/InnoEngine.Build/InnoEngine.Build.csproj`。后续查询直接运行已构建入口，或使用 `dotnet run --no-build --project entry/InnoEngine.Build/InnoEngine.Build.csproj --` 作为下列命令前缀。查询命令本身不会执行 SDK 求值、Restore、引擎编译、Native 准备或网络调用。

```text
docs index --engine-root <root>
docs search --engine-root <root> --query "ConsoleStyle" --limit 5
docs read --engine-root <root> --project Inno.Core.Identity
docs read --engine-root <root> --page docs/architecture/ownership.md --section 构建产物
docs symbol --engine-root <root> --project Inno.Core.Identity --id P:Inno.Core.Identity.Identity.runtimeId
```

加入 `--format json` 获取来源、项目、章节、行号和快照身份。默认搜索只覆盖 docs；规则通过规则加载器取得。搜索摘要用于导航，读取返回原文；大结果显式给出后续章节。返回 `Partial` 时，使用 continuation 的 page、section 和 `--offset` 继续读取，直到没有续页；不要把搜索摘要或部分正文视为完整契约。`--section` 支持标题锚点和精确 `api-…` 锚点；API 锚点同时带出所属项目的使用约束。符号使用 XML documentation ID，非精确名称列出候选，不擅自选择 overload。

## 新鲜度与验证

索引位于 `artifacts/documentation/`，按页面内容、路径、包含关系和工具身份更新，使用现有 Core IO lease、原子写入及构建 owner 登记。mtime 不作为正确性依据。查询结果中的 `Unverified` 明确表示没有核对当前源码 API；索引最新不等于编译与契约验证通过。

文档结构、链接和项目归属由 `verify --documentation-only` 检查；规则、skills 和 records 的结构由 `verify --agent-documents-only` 检查。`validate` 为 API 与示例选择必要的 owner，完整维护要求见 [文档规则](../../.agents/rulebook/documentation.md)。
