# 架构

[Wiki 首页](../README.md) · [当前验收](CURRENT_ACCEPTANCE.md) · [当前问题](CURRENT_ISSUES.md)

本分类解释跨程序集的依赖、所有权与扩展规则。具体签名和工作流位于各项目的唯一项目页。

| 页面 | 适用问题 |
| --- | --- |
| [架构总览](ENGINE_ARCHITECTURE_OVERVIEW.md) | 每层负责什么，项目应该放在哪里？ |
| [开发入口](DEVELOPMENT_ENTRY_ARCHITECTURE.md) | 两个入口如何选择宿主、准备和启动？ |
| [构建缓存](BUILD_CACHE_ARCHITECTURE.md) | 源码、工作目录、产物和读取保护分别由谁拥有？ |
| [Wiki 维护](DOCUMENTATION_STANDARD.md) | 页面、API、示例和当前证据怎样核查？ |
| [平台与运行时](PLATFORM_RUNTIME_ARCHITECTURE.md) | 平台、运行目标、产品和部署方式如何分离？ |
| [平台扩展指南](PLATFORM_EXTENSION_GUIDE.md) | 如何添加平台、backend 和必要的集成？ |
| [Web Player](WEB_PLAYER_ARCHITECTURE.md) | 浏览器启动、回调、链接与运行生命周期如何组合？ |
| [Identity 与热重载](IDENTITY_REFERENCE_RELOAD_STANDARD.md) | 如何跨代定位对象、保留 Missing、退休旧模块？ |
| [C# 开发规范](CSHARP_DEVELOPMENT_STANDARD.md) | 如何组织源码、公开契约、错误和注释？ |
| [当前问题](CURRENT_ISSUES.md) | 哪些能力或证据仍未收口？ |
| [当前验收](CURRENT_ACCEPTANCE.md) | 当前代码实际验证了什么，有哪些环境限制？ |

## 依赖原则

共同领域依赖中立契约与 Foundation。平台模块负责系统和 SDK；backend 负责共享实现；integration 连接真实差异；产品入口和发行组合选择实现。

运行时不引用构建工具。共同 Editor Hosting 不选择命名平台或第三方实现。当前可用实现与未来接入方法分别描述，未实测的平台不标记为已经验收。

日期化计划、阶段报告和旧审查保存于仓库 records 目录；当前 Wiki 不通过历史完成声明解释当前能力。

- [Native 构建平台职责与文件树](NATIVE_BUILD_PLATFORM_OWNERSHIP.md)
