# 当前验收

[架构索引](README.md) · [Wiki 首页](../README.md) · [问题台账](CURRENT_ISSUES.md)

## 基线与结论

2026-10-15，生产源码基线 `2f3fbc487520bc9eee28a35b588fd0f952a8cd8d`。本次标注审查没有修改生产源码或tests，没有使用Computer Use。

**Windows当前完整CI通过；PR整体未通过。** macOS独立BGFX单线程Metal用例超时，后续阶段未到达。BGCS新包/Binding/第三方专项按要求暂缓；GUI、焦点、视觉与声学不执行。

具体问题和关闭条件见[问题台账](CURRENT_ISSUES.md)及[完整审查](../../records/architecture/acceptance/CURRENT_ENGINE_OPEN_ITEMS_2026_10_15.md)。不同revision的成功不合并成“最终全部通过”。

## 当前head的远端证据

| 范围 | 实际结果 |
| --- | --- |
| Windows PR/push | 源码、架构、完整测试、所选Pack和D3D11/sRGB三帧/正常退出均通过。 |
| Windows测试/Wiki | push上传82份TRX：2055 passed、0 failed、18环境Skip；45598符号、190段实际编译示例、Wiki错误0。覆盖不证明每段语义文档充分。 |
| macOS源码/共同套件 | Native Debug/Release、SourceDebug、架构与共同套件通过，包括两份桌面模板的SDK求值/发布协议及Wiki。 |
| macOS独立阶段 | 已上传2068 passed、1 failed；inline Metal surface子进程90秒超时；其后隔离、Pack、renderer smoke未执行。 |
| NuGet | dependency graph与自动依赖提交均success。 |
| 项目图 | 230跟踪项目纳入Solution；架构测试123 passed；日常生产入口仍只有Editor/Build。 |

[PR Engine run38699587876](https://github.com/Wolfiora/InnoEngine/actions/runs/38699587876) · [push Engine run38699580441](https://github.com/Wolfiora/InnoEngine/actions/runs/38699580441) · [当前Checks](https://github.com/Wolfiora/InnoEngine/pull/11/checks)

## 产品与测量的确切范围

| revision/范围 | 证据与限制 |
| --- | --- |
| aaeb57ca四条产品 | CoreCLR、NativeAOT、Web解释执行/AOT均发布并各运行两次；存储、回调、帧推进、暂停/恢复与退出通过。Web为隔离headless/SwiftShader；后续模板有独立回归，最终四条产品仍需在最终组合及新包复验。 |
| c9efaa3c热Build | 465.297 / 575.609 / 777.781秒；Native compiler、binding generation、完整Task重编译、materialization均0；每次输入哈希5,227,563,904字节，仍有一个managed准备/检查进程。 |
| 同次机器负载 | CPU79–100%、可用物理内存约0.67–0.83GB；不能与旧空闲结果作同条件比较，亦不能未经剖析把全部耗时归于外部。 |
| 已准备Editor | 首帧约4.08 / 3.98 / 4.57秒，120帧退出，无完整Native/Binding/Pack准备；后台空项目不证明大项目加载或真实UI体验。 |
| 前缀验证fixture | 32 contributors：371104→58920 bytes/frame，1.57957→0.3108766 ms/frame，最终Compile=1；中立CPU路径。 |
| 本次录制fixture | 单pass 1000 Draw：串行约1.6KB/帧，并行约107.8KB；10000 Draw并行约1.15MB。统计全部线程且验证回放，无Native后端，不作为GPU结果。 |

原始证据：`artifacts/acceptance/local-boundary-refinement/20261014-implementation/`；本次补充：`artifacts/acceptance/framework-open-items/20261015-review/`。过程见[局部验收](../../records/architecture/acceptance/LOCAL_BOUNDARY_REFINEMENT_ACCEPTANCE.md)及[上轮验收](../../records/architecture/acceptance/BUILD_COST_TOOL_HOST_DISK_CLOSURE_ACCEPTANCE.md)，旧结果不覆盖当前结论。

## 当前不能宣称的完成项

- 新BGCS七组件生成、ABI/exports、AOT/回调及失败路径未专项验收；当前NuGet固定2.1.0，不能外推未来包。
- macOS最终产品/后续隔离、特殊文件系统、多小时稳定性及真实GPU性能未闭合。
- viewport拖动、焦点、DPI、视觉和听音没有实际复验。
- 200GB空间目标未达到；历史拒绝目录保留，没有绕过删除，当前采样见问题台账。

当前kernel是已有Windows和主要功能证据的阶段成果；**master合并条件尚未满足**。暂停专项、旧revision成功和文档覆盖率不能代替剩余失败与未知项。平台not built及两入口说明见[启动说明](../platform/PRODUCT_STARTUP.md)。

## 普通工程与 entry 简化（2026-10-17）

两个入口已切换为正常项目引用和直接调用。229 个项目全部纳入 Solution，当前宿主产品通过 entry 构建；旧 Bootstrap、启动 receipt 路径、CLI worker 和共享 .run 已删除。既有支持条件下的 Editor/Build 编译、15 个 Native 部署文件及仓库静态架构检查通过。普通首次 Native/BGCS 准备按约定暂缓；Rider 与 macOS 实机未验收。详见[本轮报告](../../records/architecture/acceptance/ENTRY_PROJECT_BUILD_SIMPLIFICATION_ACCEPTANCE.md)。
