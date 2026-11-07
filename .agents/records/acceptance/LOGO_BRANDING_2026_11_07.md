# Logo、默认字体图标与署名验收

[获批计划](../plans/LOGO_BRANDING.md) · [品牌资源](../../../assets/branding/README.md)

## 范围与输入

- 分支 `logo`，基线 `1294d148f445f00df7e6a9dbf22de53a75b886fa`，工作区实现尚未提交。
- 完成品牌 SVG、512px 透明 PNG、单色源稿、U+E000 独立图标字体、README 展示与截图迁移、xdzzyq 署名、默认组合原图读取 API、字体回归和使用文档。
- SDK：`C:/Users/23842/.dotnet/dotnet.exe`，10.0.402，Windows x64。没有修改 global.json 或系统 PATH；验证子进程使用本次命令的 SDK PATH。
- 统一计划位于 `artifacts/validation/logo-plan/plan.json`。未归属生产工程的 SVG 与截图被计划器保守扩大为全量；没有手工缩减或改写计划。
- 本次执行源码快照为 `402e66aad4955278a2608b73fa9011c825e5096663081c4a427c24d115efd678`。记录补写与索引更新后仅复核文档结构；生产源码、测试、品牌资源未改变。最终文件身份另存于 `artifacts/validation/logo-focused/input-manifest.json`。

## 实际证据

| 检查 | 结果与证据 |
| --- | --- |
| PNG 与字体制作 | 512×512 RGBA；可见 alpha 边界 (34,16)…(478,496)；中心三个方向采样 alpha=255，无透底；TTF 为 512 units/em，ascender=459，descender=-75；全部默认字体 cmap 中 U+E000 原先空闲。`artifacts/branding-work/asset-checks.json`。 |
| 视觉检查 | 深浅背景，14/16/24/32px，100/125/150/200% 缩放预览；保留原灰阶、轮廓和镂空。`artifacts/branding-work/branding-review.png`。 |
| 截图迁移 | 原样移动至 assets/screenshots/editor.png；SHA-256 保持 `eee344a322fd0b8140f04089bd41c909bc9eafb1f529510cefe8df7e3284ec0d`。 |
| 全仓基础检查 | 规则、源码架构、agent 文档、产品文档结构、CI 协议检查通过。 |
| 托管编译与契约 | 编译 0 警告、0 错误；计划选择的托管 API、Wiki 示例与托管测试全部通过，包含验证工具自身的 65 项测试。 |
| PNG 读取 | EngineBrandingTests 两项通过：内容哈希匹配正式 PNG，无 Session 依赖，独立位置、独立释放、只读与重新打开成立。原始 TRX 位于 `artifacts/validation/logo-run-02/managed-trx`。 |
| 原生产品编译 | Windows Debug Editor 与计划选择的测试程序集构建成功，0 错误；1 条自动缓存盘点超时警告，未退休任何缓存。`artifacts/validation/logo-run-02/native-build.log`。 |
| 原生字体专项 | 使用上述已构建的 Inno.Editor.ImGui.Tests.dll 执行完整 FontCoverageTests：9 项通过、0 失败、0 跳过；覆盖四种样式、0.75/1/1.5/2 缩放、实际 Logo 与 CJK 字形、既有图标、共享补充字体源、Context 释放/重建。`artifacts/validation/logo-focused/fonts`。 |
| 受影响原生 API | 定向检查 Inno.Engine.Default、ImGui SDL3 backend 与 Editor ImGui 三个生产 owner，通过。`artifacts/branding-work/focused-api.log`。 |
| 受影响 Wiki | 使用当前 Windows 原生产物与本次生成的逻辑脚本引用验证三个项目页，API 映射与全部示例通过。`artifacts/validation/logo-focused/wiki-api.json`、`wiki-examples.json`。 |

PNG SHA-256：`5ac155bac41eb5ac99f157635b187fd27a84d8e6caf6f77733a748346c6ce2df`。
TTF SHA-256：`171afea1982bb6bb5326f4233bfa152a4d5118254116e75e8e45a01e2dc44cf3`。

## 首次失败、全量阻塞与未完成项

1. `logo-run-01` 的 CI 协议脚本从 PATH 取得旧 SDK，未找到 10.0.402；保留首次日志。`logo-run-02` 为本次命令及子进程设置正确 SDK PATH 后，该检查通过。
2. `logo-run-02` 全量结果为 295 项 Passed、1 项 Failed、28 项 Blocked、7 项 NotApplicable。失败是 `compiled-api:native`：现有支持库 `tests/editor/fixtures/Inno.Testing.Editor.Native/Inno.Testing.Editor.Native.csproj` 经 xUnit 包导入后 `IsTestProject=true`，但 `InnoValidationLayer` 为空。该工程本次未修改，诊断与 SDK 求值保留在 `native-sdk-graph.json` 与总日志。
3. 因上述前置门禁失败，全量原生 Wiki/测试、产品旅程、Release、Support Pack、NativeAOT restore 和发布归档没有取得通过证据。定向字体、API 与 Wiki 结果仅证明 Logo 任务范围，不代替全量门禁。
4. fixture 分类问题另记 [当前问题](../open-items.md#原生-editor-测试支持库的执行层分类)，由测试基础设施 owner 按支持库/runner 的实际职责修正；本次不改写无关测试基础设施。
5. macOS 产品与其他真实设备未验收。Windows 环境没有仓库约定的 `/System/Library/Sounds/Glass.aiff`，未播放提示音。

## 语义审查

| 方面 | 结论与边界 |
| --- | --- |
| 架构与组织 | 品牌源稿集中于 assets，字体归既有 backend，原图 API 归默认组合；未把品牌数据放入 Core 或中立 SPI。源码架构与三个受影响生产 API 检查通过。 |
| 状态与生命周期 | 原图 API 无缓存或全局可变状态，每次返回调用方所有流；字体继续由原生 atlas/Context 管理。读取契约和现有字体生命周期测试通过。 |
| 抽象、复用与颗粒度 | 仅增加一个小型资源读取入口，复用程序集资源、既有字体合并和 EditorIconCatalog；没有平行资源管理器、生成系统或绘制控件。 |
| 样式与视觉 | 灰阶图片与随文字颜色变化的单色字形各有明确用途；小尺寸和深浅背景预览已检查，未增加原设计没有的分面细缝。 |
| 扩展与使用 | 常量同步后端、Editor 和逻辑 API 所属类；公开 XML、项目页与示例通过实际编译检查。字体制作参数与码点保存在品牌说明。 |
| 交互、Undo/Redo 与菜单 | 本任务只增加资源和字形词汇，没有修改交互、History 或菜单行为；未将全量产品交互检查冒充通过。 |

功能实现与 Logo 专项验收完成；全量门禁仍被上述既有 fixture 分类问题阻塞。
