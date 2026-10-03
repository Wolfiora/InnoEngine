# Web 宿主与统一构建重构计划

[架构索引](README.md) · [Wiki 首页](../README.md)

## 目标与边界

游戏与共用服务使用相同的契约；平台差异归属平台入口、适配器与目标工具链。每个原生组件只有一个项目与共同绑定定义。构建、清理、绑定验证、Support Pack 与游戏导出使用一个 Build CLI。平台入口保留各自 SDK 和启动协议。

## 实施顺序

- [x] 合并五个 Browser Native 项目。共同 BGCS 定义与目标 profile 分离，生成与编译输出按目标隔离，适配器使用固定项目引用。
- [x] 建立最小 Player 启动契约与真正共享的生命周期项目。内容位置、模块激活、存储和执行策略由组合入口提供，删除共用 Host 中的平台判断及源码链接。
- [x] Shell 使用统一帧驱动契约与一套生命周期。输入继续使用同一平台事件入口与 Core Events，保证 Session 隔离及消费语义。
- [x] 原生工具链、Shader 工具、Support Pack、原生验证与架构检查改为库。一个 Build CLI 组织依赖及命令，共同解析配置、错误和取消。
- [x] 删除被替代的项目、Program、文档和调用路径。同步解决方案、公共 XML、Wiki、架构验证及公共行为测试。
- [x] 单进程构建并执行契约测试，使用 Samples 的 FlappyBird 做 Web 和 Windows 导出与运行验收。后台验证优先；用户最新已允许 computer use。

已完成本轮六个实施阶段。最终构建、597 项测试、Windows D3D11/OpenGL 和 Web 完整玩法证据见
[验收报告](WEB_HOST_REFACTOR_ACCEPTANCE_2026_10_02.md)。macOS/Safari 和 Linux 的实际运行限制在报告中单独记录。

## 验收门槛

- Native 项目不再使用 `.Browser` 副本，绑定源由 BGCS 生成，目标产物不会交叉污染。
- 共用 Player、Shell、Logging 与输入逻辑没有浏览器平台判断。
- 生命周期、事件消费、按键释放、取消及失败清理具有真实公共契约测试。
- 只有一个构建进程入口；组件工具链没有 Main，也不通过启动其他组件 CLI 构建。
- FlappyBird Web 保留现有颜色空间、光照、星星、UI 和输入结果。
- 架构校验、相关项目构建及测试通过。macOS/Safari 的实际验证状态独立记录。
- 最终报告记录实现、公开 API 必要性、验证命令与结果及环境限制。
