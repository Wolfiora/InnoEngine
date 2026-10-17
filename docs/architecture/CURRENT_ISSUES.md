# 当前问题与待确认范围

[架构索引](README.md) · [当前验收](CURRENT_ACCEPTANCE.md) · [Wiki 首页](../README.md)

2026-10-15，生产源码基线 `2f3fbc48`。本页区分确认问题、条件性风险与未执行验收；详细源码、测量、证据及关闭条件见[完整审查清单](../../records/architecture/acceptance/CURRENT_ENGINE_OPEN_ITEMS_2026_10_15.md)。未验证不等于存在缺陷，契约通过不等于所有设备已验收。

## 已确认且仍开放

| ID | 优先级 / 所属部分 | 事实与影响 |
| --- | --- | --- |
| O01 | P2 / BGFX、macOS验收 | 当前head两条macOS CI都在单线程Metal surface退休用例超时，根因未确认。后续隔离、Pack与renderer smoke未执行；PR未全绿，不能直接归责BGCS。 |
| O02 | P2 / 构建性能 | 最新热Editor Build为465.297 / 575.609 / 777.781秒；没有重复Native、绑定或完整Task编译，每次输入哈希仍读取约5.23GB。同期高负载，当前revision空闲基线与归因尚缺。 |
| O03 | P2 / Rendering并行录制 | opt-in并行录制逐命令保存捕获委托。公开边界fixture：1000 Draw并行约107.8KB/帧、串行约1.6KB；10000 Draw并行约1.15MB。这是分配成本，不是GPU或所有场景掉帧结论。 |
| O04 | P2 / 测试诊断 | 主线程子进程超时丢弃已读取的stdout/stderr，缺阶段/线程证据；三个测试程序集复制同一进程机制。属于定位/维护问题，不是生产层依赖错误。 |
| O05 | P2 / 验收调度 | 单个隔离用例失败截断其余独立用例。应保留整体失败并汇总独立结果，产品阶段仍要求必要构建及有效产物。当前macOS覆盖缺口明确存在。 |
| O06 | P3 / XML与Wiki | 部分注释仍为模板化参数说明，值类型也笼统描述ownership。符号/签名/示例覆盖不等于行为说明充分，应补有实际意义的约束和失败语义。 |

## 待确认和未执行

| ID | 范围 | 当前边界 |
| --- | --- | --- |
| U01 | BGCS新NuGet包 | 当前精确固定2.1.0；重构后的新版本尚未切换和验收，本轮不核对未来包发布状态。 |
| U02 | 七组件新包生成 | HostSource/TargetArtifacts、C/C++ bridge、extension、CheckOnly、ABI/exports、取消/并发/损坏及原子发布待专项复验。Browser不含Editor-only ImGui/ImGuizmo。 |
| U03 | 最终产品revision | 最近完整Windows CoreCLR/NativeAOT、Web解释执行/AOT发布运行属于aaeb57ca。后续模板有当前SDK/CI回归，但四条最终产品未在最终源码与未来新包完整重做。 |
| U04 | MacOSArm64 | runner的Native编译、源码、架构与共同测试已实测；O01后的隔离、Pack/Editor smoke、最终游戏产品与用户机器未闭合，不能笼统标全部未测或全部通过。 |
| U05 | GUI/视觉/声学 | 按要求不执行。用户曾报告的viewport拖动顿挫，以及DPI/live-resize、真实焦点、Reload菜单、Console等尚无修改后实际操作结论。 |
| U06 | 负载与长期性能 | fixture、短运行与headless不证明真实GPU帧率、尾延迟、复杂项目资源增长或多小时频繁reload无泄漏。 |
| U07 | 特殊文件系统 | Core.IO严格ownership已修复；Windows大小写敏感目录的Native输入去重仍需确认，内部仍按OS选比较器。独立探针启用属性被拒绝，没有有效复现；macOS两种卷格式亦未完整对照。 |
| U08 | SDK间歇异常 | 历史runner的两个内部MissingMethodException根因未确认；当前head未再出现，不写成已根治，也不据此认定业务代码错误。 |

## 磁盘与本次修正

- **200,000,000,000字节目标未达到**。2026-10-15 18:09（台北）只读采样约73.52GB可用；其他进程仍可能改变占用。37个历史目录按此前自动审批拒绝保留，不补owner或绕过删除。
- 旧Wiki突出旧耗时和过期磁盘/macOS状态。本次重写本页和当前验收，明确revision与范围；原始测量保留在记录区。标注修正不等于源码性能问题已修复。

## 已有依据支持的架构结论

共同领域、Runtime与Hosting消费中立契约；backend、平台基础、integration和发行组合职责清楚。230个跟踪项目有Solution归属，日常生产入口为两个；未发现需要推翻整体架构的依据，也没有本次新确认的核心生产P0/P1架构缺陷。

此前五项整改已实现：严格路径、RenderGraph前缀验证、SDK宿主RID、单调退休序号、轻量缓存维护。O03为另一条录制路径，不表示前缀优化无效。项目数量、API数量、必要完整哈希、安全保留缓存和未实现平台本身不作为缺陷。

BGCS/第三方专项按要求暂缓；失败保留于[PR Checks](https://github.com/Wolfiora/InnoEngine/pull/11/checks)，不删除断言或改过滤条件制造通过。

## 开发入口变更的验证边界（2026-10-17）

普通工程启动简化已经完成代码整改及既有支持条件下的编译。完整首次 Native/BGCS 准备和普通 Native 热命中未验证；Rider 实机由用户执行。该限制不等于本轮引入了已复现的运行缺陷，详细证据见[入口报告](../../records/architecture/acceptance/ENTRY_PROJECT_BUILD_SIMPLIFICATION_ACCEPTANCE.md)。上述历史测量和其他未决项仍按其原始范围保留。
