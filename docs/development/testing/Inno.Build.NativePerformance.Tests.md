# Inno.Build.NativePerformance.Tests

[测试与 fixture 索引](README.md)

[测试索引](README.md) · [构建契约](Inno.Build.Tests.md) · [统一验证](../tooling/Inno.Tooling.Validation.md)

该项目拥有真实 Windows 原生产品成本验收，声明 performance 层、Windows 执行环境及 Editor 原生准备前提。普通领域修改和每日纯功能矩阵明确排除性能采样；构建、原生、reload、该测试输入变化或显式 extended 验收会选择它。其他宿主明确记为 NotApplicable，不准备该 owner 的 Windows 产品。

`ProductNativeCostTests` 从原构建契约项目迁入，保留真实 ImGui/ImGuizmo 构建断言、串行与批次 producer 对照、零原生进程、零绑定生成、读取字节减少和唯一批次锁阶段断言。两个环境变量开关及提前返回已删除；五个独立 warm 样本通过 xUnit 输出记录 JSON 耗时和全部输入统计，随 TRX 归档。构建准备负责预热，冷构建另由 extended 计划记录；样本不允许用冷构建冒充热缓存。

测试直接通过 `StandardBuildDistribution` 和公开工具链接口取得真实 Editor 闭包，不新增生产入口、不复制产品或后端名单，也不跨项目链接手写实现。未执行的性能检查不能算作通过，首次失败和已产生样本保留。
