# 最终平台、Reload 与 NuGet 收口执行规格

[执行规格索引](README.md) · [本轮验收](../acceptance/FINAL_PLATFORM_RELOAD_NUGET_ACCEPTANCE.md)

本记录对应用户批准的最终收口计划。验收目标是最终本机收口：源码、消费者、项目图、Wiki、Windows/Web 发布运行、GUI、reload、性能与磁盘证据闭合。macOS Native 和产品实机证据独立标记，不以 Windows 结果替代。本轮不自动提交。

## 确定边界

- 保留两个开发入口 `InnoEngine.Editor`、`InnoEngine.Build`。
- 平台及相关项目、程序集、namespace、类型、模板和文档改为 WindowsX64、MacOSArm64、BrowserWasm32；稳定 target ID 保留。
- 删除 Linux 和仓库内 dependencies，BGCS 完全使用精确 NuGet 包引用与锁文件。
- 保留 backend 的职责命名、既有持久身份及统一 Identity/Missing/generation 机制。
- 不修改 extern，不手工修改生成绑定，不建立兼容入口、IVT、私有反射或测试后门。
- 用户允许实际 GUI 操作；需要其他实机的项目单列证据条件。

## 问题和门禁

| 项目 | 必需整改 | 验收 |
| --- | --- | --- |
| C01 | Play 手动 Reload 禁用，自动请求暂存，停止退休后推进 | Preparing/Playing/Stopping 准入、最新请求、启动编译与 GC gate |
| C02 | Editor 更新及 Scene 事务进入所属 Edit Session；Identity 变更前验证 owner | 相同 persistent ID 的跨 domain、替换失败及逆序回滚、真实 Plugin reload |
| C03 | Console 标题仅保留等级、消息、计数，Kind 留在详情 | 实际布局、长消息、筛选与折叠 |
| C04 | 三个明确目标完整命名迁移 | 源码、项目、程序集、模板、JS、Solution、Wiki，无旧有效入口 |
| C05 | Linux 整个平台退出 | 注册、项目、测试、文档与物理路径删除 |
| C06 | BGCS 包族固定本轮最新稳定版本，无源码依赖 | locked restore、包 hash、实际生成、编译、NativeAOT/Wasm 调用 |
| C07 | cache clean 按显式目标筛选，保护计算保持全局 | 全缓存/目标 dry-run、其他目标/共享依赖、活动 reader |
| C08 | 平台只验证 ABI/布局，标准发行验证所选组件闭包 | 错误 PE/Mach-O/Wasm、缺失/额外产物和部署映射 |
| C09 | 离线工具配置独立，所有消费者选择同一实际工具产品 | 冻结身份差异、热命中不启动 compiler、游戏内容编译 |
| C10 | Task/工具 publication 与 reader 无交接空窗 | 复用、恢复后的输入、并发清理、载入 DLL 与退出 owner |
| C11 | 当前 Wiki 对应当前符号、示例、链接和行为 | 全项目唯一页、逻辑脚本引用、SDK 编译、无外部插件职责耦合 |
| C12 | 补齐产品、GUI、reload 与性能矩阵 | 修改后的新证据，不复用历史通过结论 |
| C13 | 正确解释 not built 和实际启动链 | 默认 Solution 两入口、独立产品准备、无参数目录选择 |

## 执行归属

```text
entry/Inno.Entry.Bootstrap/            轻量路由、准备记录、公开应用入口
src/composition/editor/
├─ framework/Inno.Editor.Core/        Reload guard 与事务协调
├─ features/Inno.Editor.PlayMode/     Play generation 读取保护与停止
├─ features/Inno.Editor.Scripting/    请求、编译、激活准入
├─ features/Inno.Editor.Scene/        所属 Session 事务
├─ hosting/Inno.Editor.Hosting/       Edit/Play 更新及呈现作用域
└─ panels/Inno.Editor.Panel.Logging/  Console 展示
src/content/scene/Inno.Scene/         Identity 替换验证与回滚
platforms/
├─ WindowsX64/                       系统、产品、SDK、布局及真实集成
├─ MacOSArm64/
└─ BrowserWasm32/
backends/                            唯一 runtime/native/build owner
build/
├─ msbuild/                          包版本、产品与工具属性隔离
├─ bindings/                         NuGet 生成应用与生成身份
├─ toolchains/                       冻结输入、workspace、产物及 lease
├─ support/                          准备缓存与所选闭包校验
├─ tasks/                            Task build scope 和 publication
├─ distributions/                    唯一标准组合
└─ cli/                              命令与借用工具的构建应用
tests/                               真实公开边界回归
docs/                                当前权威说明
records/architecture/                执行规格与历史验收
artifacts/acceptance/
└─ final-platform-reload-nuget/
   └─ 20261013-final-closure/         基线、文件/项目/包映射、日志与截图
```

## 本轮实际检查补充

检查发现 Build CLI 从可写 SDK 输出载入，以及工具消费者仍以自己的 DEBUG 符号选择 `shaderc-debug`。整改沿现有边界完成：工具 SDK 输出使用独占 owner，完整部署发布到受保护的不可变目录；离线工具使用配置无关的文件名，配置由所选 Native 步骤及指纹表达。内容 compiler 借用同一组 executable/include 路径，项目构建持有工具结果至内容编译结束，不修改不可变应用部署。

## 阶段顺序

基线与复现 → Reload/Identity → Console → NuGet → 明确目标迁移/Linux 删除 → cache/Pack/Task → 工具身份与成本 → Wiki/Solution/架构 → 产品/GUI/性能 → 最终审查和 owner 清理。

每阶段完成消费者、XML、引用、文档及必要测试；失败修复后重跑受影响门禁。原始失败证据保留，不能将未执行写为通过。

## 最终判定

最终本机收口要求 Windows Debug/Release Editor、CoreCLR/NativeAOT Player、Browser 解释执行/AOT、公开消费者、真实 reload/GC、GUI 与视觉结果闭合，并且无未解决 P1/P2。macOS 源码与组合检查、Native/产品实机结果分别记录。缓存仅依据明确 owner 和活动保护清理，记录逻辑回收量与实际卷空间。
