# InnoEngine 文档

这里解释引擎的设计、原理、使用方法与调用边界。人和 AI 使用同一份正文；页面存在与签名标记齐全不等于所有行为已经实测。

| 阅读任务 | 入口 |
| --- | --- |
| 第一次使用 | [环境、启动与项目](getting-started/README.md) |
| 理解跨模块协作 | [架构与所有权](architecture/README.md) |
| 使用具体能力或 API | [模块索引](modules/README.md) |
| 了解第三方封装和 Native 限制 | [Backends](backends/README.md) |
| 选择平台、SDK 和产品 | [Platform](platform/README.md) |
| 构建、验证、扩展或查询文档 | [开发指南](development/README.md) |

每个项目有唯一同名入口；长页面的细节和 API 位于同名目录。先读用途和生命周期，再按需要查询类型与成员。跨代引用先读 [Identity 与恢复](architecture/identity-and-reload.md)。

修改仓库的要求见 [贡献指南](../CONTRIBUTING.md)；实施和验收状态见 [工作记录](../.agents/records/README.md)。历史命令、旧提交结果和未执行矩阵均不作为当前使用契约。
