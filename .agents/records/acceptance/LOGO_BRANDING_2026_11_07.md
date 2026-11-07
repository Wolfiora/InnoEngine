# Logo、默认字体图标与署名验收

[获批计划](../plans/LOGO_BRANDING.md) · [品牌资源](../../../assets/branding/README.md)

## 后续修订：Inno 命名与 README 尺寸

基线 `e3b3143e`，分支 `logo`。将两个公开图标常量改为 `ImGuiIcon.Inno`，同步字体测试、使用示例和 API 映射；目录从公开常量自动取得名称 `Inno`，不保留旧别名。Editor 常量引用使用 `global::` 避免与根命名空间同名。README 的 Logo 宽度改为 240，居中与替代文本保留。按用户最后确认，只删除根 README 的 Logo 设计致谢；品牌说明、代码注释和 TTF 作者元数据保留。字体、PNG、SVG 内容没有变化。

本轮采用同一显式 SDK 10.0.402 与原生构建上下文。当前通过：

- Editor ImGui 测试工程及其实际引用闭包正常构建，0 警告、0 错误，保留原生准备和部署核验：`artifacts/branding-work/inno-build.log`。
- 改名后的 FontCoverageTests 9 项通过、0 失败、0 跳过：`artifacts/validation/logo-inno-focused/fonts/fonts.trx`。
- 两个生产 owner 的编译 API 检查、全仓源码架构和最终文档结构检查通过：`artifacts/branding-work/inno-api.log`、`inno-source.log`、`inno-documentation-final.log`。
- 文档工具及其闭包正常构建，0 警告、0 错误；使用本轮生成的逻辑脚本引用核验两个相关项目页，API 映射和全部示例通过：`artifacts/branding-work/inno-cli-build-retry.log`、`inno-wiki-projection.log`、`inno-wiki.log`。清单位于 `artifacts/validation/logo-inno-focused/wiki-api.json`、`wiki-examples.json`。
- 根 README 不再包含作者致谢，品牌说明、代码注释、字体 name 表的 designer 和 URL 仍保留；TTF 和全部图片资源相对基线无差异。

统一计划为 `artifacts/validation/logo-inno-plan-final/plan.json`（35 个受影响 owner、47 个必需检查）。当前 SDK 元数据预检仍复现既有原生 fixture 执行层分类违规，见 `artifacts/branding-work/inno-fixture-preflight.log`；没有执行或声称通过整个统一计划、完整产品旅程和其他平台矩阵。一次文档查询命令参数写法错误，改用 `docs symbol --project ... --id ...` 后定位正确页面；首次 CLI 准备在用户调整署名范围时取消，后续正常重建，不将取消的执行记为通过。

最终文件输入清单位于 `artifacts/validation/logo-inno-focused/input-manifest.json`。收尾只补写记录与 README 致谢，不改变已验证的生产代码、测试或字体输入。架构、owner、加载方式、状态和生命周期均延续原实现；此次只改变公开常量与目录名称、README 展示，未修改交互、Undo/Redo 或菜单流程。下文旧证据不作为重命名后的编译证据。

## 当前修订：空心字体与移除内置原图

- 基线为 `402a191e`，分支 `logo`。用户确认四块各自描边的预览，并要求替换为其提供的 v2 SVG、彻底删除内置原图能力。
- 正式 TTF 与已确认预览字节相同，U+E000、度量、作者署名和合并入口不变；字形由八条内外轮廓组成，四块内部透明。
- 正式 SVG 与用户提供文件字节相同；512px 透明 PNG 从新 SVG 导出，仅保存在品牌目录。默认组合的图片 API、资源链接、专用测试和测试引用已删除，产品文档同步清理。
- SVG SHA-256：`62f84df21f90b626db5aef502cb064645c05d3bcd92a6d1fc152cf3ef0ba55ed`。
- TTF SHA-256：`143dce4e0ca463e7a71d647fd52d607e34e56068089dfb70620c105124ca5cc3`。
- SDK 显式使用 `C:/Users/23842/.dotnet/dotnet.exe` 10.0.402；没有修改全局 SDK 配置。实现后的记录收尾于 2026-11-08；以下专项证据属于本轮输入。

### 本轮实际证据

| 检查 | 结果与证据 |
| --- | --- |
| 正式资源 | TTF 与批准预览、新 SVG 与提供文件逐字节一致。八条内外轮廓方向相反，四块内部采样透明，SVG 精确复现 TTF 轮廓；PNG 为 512×512 RGBA，可见边界 (34,16)…(478,496)，中央接缝 57 个采样 alpha 均为 255。`artifacts/branding-work/outline-checks.json`。 |
| 视觉 | 批准的空心字体预览保存在 `artifacts/branding-work/outline-preview/icon-preview.png`；新灰阶 PNG 深浅背景检查见 `artifacts/branding-work/logo-v2-review.png`。 |
| 原生专项编译 | 正常构建默认组合、Editor ImGui 测试及 Build 工具依赖闭包，没有跳过原生准备或项目引用；0 警告、0 错误。`artifacts/branding-work/outline-focused-build.log`。 |
| 字体回归 | 当前部署的 TTF 哈希与批准字体一致；FontCoverageTests 9 项通过、0 失败、0 跳过，覆盖四种样式、缩放、CJK、既有图标、字体源共享和 Context 生命周期。`artifacts/validation/logo-outline-focused/fonts/font-coverage.trx`。 |
| 运行时回归 | 移除专用图片测试与额外引用后重新构建，0 警告、0 错误；剩余 62 项通过、0 失败、0 跳过。`artifacts/branding-work/outline-runtime-build.log`、`artifacts/validation/logo-outline-focused/runtime/runtime.trx`。 |
| 删除完整性 | 源码、项目文件、测试和当前使用文档没有被删 API 或资源名的引用。对新编译默认组合的 PE 元数据检查 21 个类型，无品牌类型，资源表为空。`artifacts/validation/logo-outline-focused/default-assembly.json`。 |
| 编译 API | 默认组合、ImGui SDL3 backend、Editor ImGui 三个 owner 检查通过。`artifacts/branding-work/outline-api.log`。 |
| Wiki 契约 | 使用本次生成的 Windows 逻辑脚本引用检查三个相关项目页，API 映射和示例通过。`artifacts/branding-work/outline-wiki-retry.log`；清单位于 `artifacts/validation/logo-outline-focused/wiki-api.json`、`wiki-examples.json`。 |
| 最终源码与文档 | 249 个工程的源码架构检查、规则登记检查、产品文档结构与 agent 文档检查通过；`git diff --check` 通过。日志为 `outline-source-final.log`、`outline-rules-final-retry.log`、`outline-documentation-final.log`、`outline-agent-documents-final.log`（均在 `artifacts/branding-work`）。规则脚本首次多路径参数写法有误，改为 PowerShell 数组后通过，首次输出保留在 `outline-rules-final.log`。 |

PNG SHA-256：`2656f31f52669a51345928718b18d8bb94f2a239d5ee8e93f754e32359e60aa9`。默认组合 DLL SHA-256：`4ccaad2c86c753425d9d09070d61aa2482e50824c3e2a6293d940745c56db64b`。最终文件输入清单位于 `artifacts/validation/logo-outline-focused/input-manifest.json`；收尾只补写记录，未改变已验证的产品或测试输入。

### 本轮统一计划与未完成项

1. 正确工具生成的统一计划为 `artifacts/validation/logo-outline-plan-02/plan.json`；品牌文件未知 owner 触发保守全量选择（249 个 owner、324 个必需检查），没有手工改写计划。一次较早的运行误用了旧缓存 CLI，识别后终止，保留 `outline-run.log`，不作为证据。
2. 正确全量运行 `logo-outline-run-02` 执行过程中，独立的当前 SDK 元数据预检再次确认既有 fixture 问题：`Inno.Testing.Editor.Native` 为 `IsTestProject=true`，但 `InnoValidationLayer` 为空；预检退出 1，仅此一项违规。日志为 `artifacts/branding-work/outline-fixture-preflight.log`，对应 [现有问题](../open-items.md#原生-editor-测试支持库的执行层分类)。
3. 确认该全量门禁阻塞后终止尚在进行的全量原生构建；该运行 `completed:false`，不提供全量完成认证，也不将其中已观察到的通过数作为完成证据。上表来自随后独立完成的专项执行，不替代统一全量门禁。
4. 首次 Wiki 命令错误地从输出子目录寻找生成项目，缺少引用参数而失败；改为实际项目根目录读取引用后检查通过。首次日志 `outline-wiki.log` 保留。脚本投影成功，但自动缓存盘点超过预算而推迟；没有退休缓存，见 `outline-wiki-projection.log`。
5. 全量原生测试、产品旅程、Release、Support Pack、NativeAOT、发布归档及 macOS/其他设备没有本轮完成证据；未将既有环境阻塞记为通过。Windows 不存在 `/System/Library/Sounds/Glass.aiff`，未播放提示音。

### 本轮语义审查

品牌 SVG/PNG 只保留在仓库资源目录；默认组合删除图片 API、资源链接与专用测试，没有兼容入口、替代管理器或运行时图片依赖。字体沿用现有 loader、常量、码点和 EditorIconCatalog；生命周期由原有字体测试覆盖。空心图标精确采用用户批准预览，灰阶原稿采用用户新文件。交互、Undo/Redo 和菜单行为未修改，其全量旅程也未宣称通过。

以下均为初版历史证据，不代表本轮字形、资源或删除行为通过。

## 初版历史证据（已被上述范围取代）

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
