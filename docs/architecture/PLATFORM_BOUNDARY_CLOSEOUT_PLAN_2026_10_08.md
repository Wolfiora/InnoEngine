# 平台边界收口与静态图还原修复

[架构索引](README.md) · [问题复核](PLATFORM_OWNERSHIP_PLAN_AUDIT_2026_10_08.md)

## 目标和执行顺序

本次处理两个 P2 源码缺口，以及 Windows IDE 产品构建的 NuGet 子进程启动失败。
保持六个产品入口、当前稳定目标 ID、领域运行契约、BGCS 独立性和全部现有发布能力。
不增加兼容工厂、平台枚举、源码复制、空项目或新的 Program。

1. 复现并修复 NuGet 静态图还原的大属性传输；通过真实产品还原入口验证。
2. 将 BGFX Native SDK invocation 和 Shader 编译配置由平台显式贡献。
3. Support Pack 改为预检冻结、隔离执行、验证发布三个阶段。
4. 同步全部消费者、公开 XML、Wiki、架构检查和失败/取消/扩展测试。
5. 验证普通 Editor Build、实际 Windows/Web 发布与运行、热复用和目录完整性，记录本次证据。

## BGFX 的边界

- `backends/Bgfx` 拥有生成、源快照、BGFX recipe、工具运行、输出收集和内容编译。
- 平台拥有实际 GENie/SDK 参数、输出布局 token、Shader 方言、离线能力与产品 API 选择。
- Native profile 只生成冻结的 invocation；共同执行器将完整参数和布局纳入 recipe 指纹，再执行一次。
- Shader profile 是不可变开放配置，不是命名平台枚举。共享 compiler 按配置解析 renderer，不识别 Inno 平台名单。
- 标准发行在一个绑定列表中组合平台贡献与 Shader profile；中立 BuildDistribution 不依赖 BGFX。
- 静态组件明确声明由聚合 executor 执行，不填入不会执行的 GENie producer。Browser 继续使用真实静态聚合实现。
- 删除旧 BgfxBuilderFactory、命名平台 builder、BgfxShaderTargetPlatform 和三个内容 compiler 工厂；更新当前所有消费者。

## Support Pack 的边界

```text
注册与参数校验
→ 只读 SDK/项目/模板预检
→ 冻结 managed CLI、Native selection 和产品闭包
→ 取得发布 lease
→ 创建 staging
→ 使用同一冻结计划执行
→ 完整校验
→ 原子发布
→ 清理候选
```

Planning context 不含输出或 staging。source 返回固定目标的 plan，publisher 校验身份后才创建输出。
plan 不再次读取 PATH 或选择 SDK；文件准备和 Browser 准备各自保存冻结选择，构建调用使用记录的 managed CLI。
共享 publisher 不知道平台 SDK、BGFX、Emscripten 或具体包装规则。
取消、缺 SDK、不支持宿主和预检失败不创建输出；执行失败仍保留上次完整发布。
计划只保存不可变数据和借用的 source，不持有跨阶段运行中的进程或临时 native 资源。

## 文件与契约闭包

| 位置 | 工作 |
| --- | --- |
| 根 Directory.Build.props / Directory.Solution.props | 启用 NuGet 标准输入传递全局属性，保留静态图还原 |
| BGFX Native build | 新增 profile/invocation，删除封闭工厂和平台 builder，指纹覆盖实际 invocation |
| Windows/MacOS/Linux Build | 提供 SDK invocation；Windows/MacOS/Browser 提供完整 Shader profile |
| BGFX Tools / Shaders | 接收不可变 profile；删除平台枚举、固定内容工厂与目标 switch |
| Standard Distribution / Native plans | 唯一绑定注册；明确独立组件执行与静态聚合执行 |
| SupportPacks.Core | planning context、frozen plan、publisher 顺序、File preparation |
| 三平台 Support Pack source | 预检构造计划；实际准备消费该计划 |
| CLI / Tasks / Editor / 产品 props | 同步新契约和显式 target/profile 选择 |
| Build / BGFX / Architecture tests | 大属性、未知目标、完整指纹、预检无副作用、失败/取消/并发与实际消费 |
| 对应 Wiki / 架构索引 / CURRENT_ISSUES | 当前 API、删除清单、证据和扩展示例同步 |

## 验收要求

- 65,536 字符全局属性：旧传输复现同样错误，新传输经真实产品 restore 成功。
- 新 Native/Shader fixture target 不修改 backend 平台名单即可组合。
- 平台配置、参数与能力改变正确失效，输出损坏拒绝复用。
- 缺 SDK、错误宿主、预取消、计划目标错误：无输出和 staging。
- 等待锁取消、执行失败/取消、验证失败：保留旧完整输出并清理候选。
- SDK/工具只在规划时解析，执行阶段使用同一冻结 selection。
- 普通 Windows Editor Debug/Release 构建；Windows CoreCLR/NativeAOT 与 Browser 解释/AOT 实际导出和运行。
- 架构、引用、公开 XML、源码风格、项目归属与 Markdown 链接通过。
- macOS/Linux 实机仍由对应环境验收，Windows 构建不能替代。

本文件是本次执行规格；完成状态、实际命令和限制另记验收报告。
