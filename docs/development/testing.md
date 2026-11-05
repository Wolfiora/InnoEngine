# 选择与执行验证

[开发指南](README.md) · [验证工具](tooling/Inno.Tooling.Validation.md) · [测试项目](testing/README.md)

统一入口是 Build 的 `validate plan/run/full/evidence`。计划根据基线、工作区变化、实际 SDK 项目引用、imports 和附加输入选择 owner，显示编译前提、检查原因、环境及排除项。命令参数和状态语义以 [验证工具项目页](tooling/Inno.Tooling.Validation.md) 为准。

日常修改先读计划，再执行受影响的领域、契约或集成检查。无需窗口、GPU 和原生部署的领域套件与 Native 套件按执行前提隔离；真实原生生命周期仍由所属 backend 验证。

架构、领域单元、公共契约、集成、Native、产品/设备、性能/长期运行分别提供证据。缺少环境、空发现、取消或缺少结果不能算成功。证据只在源码、测试、工具和环境输入仍匹配时复用。

纯文档导航与 agent 记录的结构检查不需要编译引擎；API 分片与可执行示例由对应项目的引用闭包验证。完整产品与设备矩阵显式执行，有限范围通过不代替完整认证。
