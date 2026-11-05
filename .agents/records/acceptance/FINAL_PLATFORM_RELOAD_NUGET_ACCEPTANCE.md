# 最终平台、Reload 与 NuGet 收口验收

基线：`a82fb6b9ed50405029ea653c1d5283854ef2ca0e`，分支 `kernel`，2026-10-13 更新。没有自动提交。

## 当前结论与范围

用户已取消 GUI 与 Computer Use 验收。本轮源码整改、非 GUI 功能门禁及下面列明的本机发布路径通过；没有发现新的阻止提交的架构、耦合或功能缺陷，**当前源码可以提交**。

**完整原计划尚未全部完成**：真实小 Native 修改的产品级性能测量、最新浏览器交互、完整 GUI/声学及 macOS 实机证据仍缺；磁盘尚未恢复到约 200 GB。不能把这些项改写成通过，不能称引擎绝无问题。

阶段 16 之前的逐次失败、恢复及 GUI 停止记录保留在 [历史检查点](../history/FINAL_PLATFORM_RELOAD_NUGET_STAGE16_CHECKPOINTS.md)。本页是当前结果，历史结果不代替最新部署。

## C01–C13

| 编号 | 当前结果 |
| --- | --- |
| C01 | Reload guard、菜单与执行入口共享准入；Play 自动变化暂存，停止退休完成后处理；generation read lease 防止播放中发布。 |
| C02 | Scene reload 全阶段进入所属 Session，旧 identity 在结构修改前校验，失败恢复旧注册和事务状态；真实 Plugin 与 owner/rollback 回归通过。 |
| C03 | Console 标题仅等级/消息/次数，Kind 留在详情；原生布局测试通过。 |
| C04 | 目录、项目、程序集、类型和 Solution 采用 WindowsX64、MacOSArm64、BrowserWasm32；稳定 target ID 保留。 |
| C05 | 未选择的 Linux 平台项目、注册、集成及当前文档支持声明已删除。 |
| C06 | dependencies 与源码依赖入口已删除；BGCS 包族 NuGet 固定 2.1.0。653 份锁文件中的 286 条包记录一致。 |
| C07 | target-scoped cache cleanup 的候选选择和全局依赖保护专项通过。 |
| C08 | 平台 validator 负责布局/ABI，所选 managed/native/binding 闭包由中立 requirements 验证，不在平台中复制标准后端名单。 |
| C09 | Shader tools 配置固定 Release，GENie/SDK/参数由集成明确贡献；路径规范化修复错误身份；三次热构建 compiler 为 0。 |
| C10 | Task publication 的 activity、文件 lease 与 process reader 交接无裸路径空窗；fresh Restore 身份和 Build-scope 复用专项通过。 |
| C11 | 全量严格 Wiki 门禁通过，当前 API/示例/链接/内容边界同步；当前问题页已移除过期的待实现状态。 |
| C12 | Windows CoreCLR/NativeAOT 与 Browser 解释执行/AOT 最新发布通过；GUI/其他设备及部分性能项目按下文列明。 |
| C13 | 仅两个日常启动项目；平台产品库不参加默认 Solution Build 时显示 not built，入口独立准备/加载它们。 |

## 新发现的真实缺陷与修复

阶段 15 的 NativeAOT 发布成功，但首次运行 `Log.Info` 在 `Assembly.GetCallingAssembly()` 抛 PlatformNotSupportedException。这条路径记录为失败，随后正式替换全部静态 Log overload 为显式 `Type sourceType`：`Log.Info(typeof(Owner), ...)`。

来源类型只借用一次，日志复制中立分类和文本，不长期保存 Type/Assembly；复用 Modules 的弱分类缓存，删除第二份来源表和旧 overload。没有 AOT/OS 判断、私有反射或兼容转发。Logging、Runtime、Wiki 示例、实际消费者源码和 Plugin 包同步，最新 NativeAOT 两次运行复验成功。

## 架构与可扩展性审查

- 共享 Foundation/Content/Services/Runtime 不依赖平台实现；Rendering 契约保持中立。
- 平台基础 build/runtime、运行 integration、构建 integration 和产品选择分别归属；backend 不反向依赖平台或 integration。
- 默认 Catalog 必须注入 platform/rendering factory，具体选择来自产品组合。
- SDL 共同窗口操作只有一个 owner；Win32/Cocoa/Canvas 差异属于 integration。
- BGFX surface SPI 接收冻结借用描述，Device 不维护平台白名单；附加窗口先退休 framebuffer，确认渲染命令处理后再由 SDL owner 销毁。
- 内容 compiler 与平台 packager 通过 binding 组合；更换图形 backend 不改平台布局实现。
- 绑定生成、Native workspace、immutable artifact、Pack、Task reader 都有明确 owner 和失败/取消边界；generation/GC/Faulted gate 保留。
- 新平台增加真实目标、SDK、必要运行/构建 integration 和薄产品组合；新 backend 增加共享实现、内容 compiler/recipe 与必要 integration，再由发行组合选择。共同领域业务不增加平台判断。

目前不需要再推翻整体分层。构建成本仍偏高、历史磁盘治理尚未完成；这些实际问题必须保留，不能用“现代”或“优雅”代替证据。

## 编译、测试与发布证据

原始证据根：`artifacts/acceptance/final-platform-reload-nuget/20261013-final-closure/`。

| 范围 | 最新结果 | 证据 |
| --- | --- | --- |
| SourceDebug / SourceRelease | 0 errors / 0 warnings；49.255 / 235.680 秒 | stage16-fixed-Source*.json、对应日志 |
| 架构 | 122/122，最新再次通过，54 秒 | stage19-architecture.trx |
| Wiki | 完整门禁 2/2；45,548 符号，189 示例，352 页面 | stage16-wiki-final.trx、wiki-api/examples.json、三个 wiki-*-map.tsv |
| Logging / Runtime | 41 + 61 项 | stage16-logging-runtime.trx |
| Cache / Pack / Task | 40 项 | stage16-cache-pack-task.trx |
| PlayMode / Hosting | 53 + 6 项 | 阶段 12/14 对应专项 TRX |
| Console / viewport retirement | 4 + 40 项 | 阶段 12/14 对应专项 TRX |
| Solution | 230/230，无遗漏项目和空 Folder，仅两个日常 Exe | stage19-solution-package-audit.json |
| CoreCLR 发布 | 473.172 秒，包括首次更新 Pack 准备 | stage16-coreclr-publish.json |
| NativeAOT 发布 | 86.780 秒 | stage16-nativeaot-publish.json |
| Wasm 解释执行发布 | 317.350 秒 | stage16-mono-wasm-publish.json |
| Wasm AOT 发布 | 399.282 秒，88 个程序集 AOT、最终链接完成 | stage16-mono-wasm-aot-publish.json |
| Windows 实际运行 | 各两次 360 帧，exit=0、stderr=0；存储 3→4→5 / 5→6→7，DSP 时钟推进 | stage16-windows-runtime-summary.json |
| 公开消费者 | 最新两条源码/Plugin 导出均 exit=0 | stage16-Canvas-plugin.json、stage16-Rendering2D-plugin.json |
| Editor 部署 | Debug 558、Release 507 payload 完整哈希一致，排除 native.lock | stage19-editor-deployment-integrity.json |
| 并发产品构建 | 两进程 exit=0；owner 协调，总时间 620.379 秒 | stage19-concurrent.json、两份日志 |

后台 Windows 运行不替代 GUI 或声学验收。最新 Web 编译/链接通过，之前 Web 实际运行证明焦点、输入、存储及 DSP，但不能冒充静态日志来源变更后的新交互证据。

## 性能与空间

三次热 Editor Build：281.564、194.049、251.685 秒。每次 Native compiler=0、binding generation=0；Task 在本次 Build 准备 1 次、复用 2 次，shader-tools compiler=0。Native 验证仍累计读取约 14.73 GB，加上 shader-tools 检查约 2.97 GB。此数据表明缓存正确命中，也表明验证成本仍显著；不宣称总耗时已完全优化。

并发复验发生在显式 cache clean 后，包含 Task runtime 重新准备及同产品 owner 排队，不作为两条独立热构建的加速比例。真实小 Native 修改的产品级性能测量仍未执行；fixture 的内容失效和 workspace 复用是功能证据。

此前 Release Editor 真实启动约 10.15 秒，没有 Native/BGCS/Pack 准备。`--status` 三次只验证宿主路由，不能当作三次真实启动计时。

同一 Browser Pack 请求返回相同 content fingerprint，未执行 Native compiler、binding generation 或 Player managed Build；158.752 秒包含一次 CLI bootstrap，不能描述为纯缓存检索时间。

全盘只读调查：InnoEngine 147,795,653,855 逻辑字节，独立 BGCS 仓库 22,195,485,750 字节。NVIDIA 缓存、分页文件、SDK、用户下载等单独归类，不认定都由本轮产生。正式 owner 清理已回收无引用产物；额外 37 个历史目录约 51.32 GiB 的逻辑占用，批量删除和重新授权后的单目录删除均被自动审批拒绝，没有绕过。

**约 200 GB 可用空间目标没有达到。** 详细目录、历史空间记录、实际卷回收与剩余操作见 [磁盘报告](DISK_CLEANUP_20261013.md)。不能把逻辑文件大小或 junction 统计当作实际可回收卷空间。

## 清理、公开变更与剩余项

- 验收副本的存储/DSP/frame 探针已先保存证据，再恢复 baseline 文件，字节哈希一致；原项目玩法源码没有被探针覆盖。
- 已停止本轮两个网页服务器，日志、截图、当前产品、NuGet 和 SDK 保留；没有删除锁文件。
- 必要公开变化为 reload guard/注册 lease、明确日志来源参数、surface integration/retirement Task、显式 Native/Pack 读取生命周期；每一项分别表达准入、来源、借用窗口或读取保护，没有测试专用后门。
- 实际消费者源码及正式导出的 Plugin 包已同步；外部仓库修改没有自动提交。
- MacOSArm64 实机未实测；当前未选目标不注册产品。
- 用户取消本轮 GUI，最新完整焦点/多屏 DPI/视觉/声学及 Web 交互不记通过。
- .NET 9 Build 入口的 tiered compilation 单独关闭是宿主运行异常的观测性处理，不宣称 CLR 内部根因已证明。
- 远端 CI 状态以对应 workflow 运行记录为准。

## 提交建议

当前源码和已执行功能门禁足以提交；提交说明保留上述性能、空间及未实测限制。没有自动提交，不能标注原计划“全部完成”。建议标题：

```text
fix(kernel): finalize platform boundaries and reload ownership
```

当前 Windows 不存在 `/System/Library/Sounds/Glass.aiff`，提示音未播放。
