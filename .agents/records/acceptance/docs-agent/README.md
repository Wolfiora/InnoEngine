# 文档重组的局部验收附件

[阶段报告](../DOCS_AGENT_INFORMATION_ARCHITECTURE_ACCEPTANCE.md)

- [逐项目覆盖快照](project-coverage.json)：从实际 csproj 派生，不作为新的维护名单。
- [迁移与原始证据哈希](migration-audit.json)。
- [最终查询原始样本](query-performance.json)；[并发构建期间的较慢样本](query-performance-contended.json) 保留，不以重测抹去。
- [Console 搜索](console-search.json)、[Identity 精确符号](identity-symbol.json)、[Toolchains 读取](toolchains-read.json)。
- [所选 API 清单](selected-api.json) 和 [示例编译](selected-examples.json)。
- [合并前重复 API 行](merged-api-rows.json)：合并保留各行独有参数、行为和所有权。
- [最终文件树](final-tree.txt)；[实现输入身份](input-snapshot.json)。

大型日志仍在本地 artifacts/docs-reorganization/。这里只保存小型审查数据，不复制 TRX、构建缓存或原生产品。查询结果中的 Unverified 与独立源码契约检查分开。
