# 新项目默认 Editor 布局验收

[Issue #47](https://github.com/Wolfiora/InnoEngine/issues/47) · [PR #26](https://github.com/Wolfiora/InnoEngine/pull/26) · [执行台账](../EDITOR_UI_CLOSURE_PLAN.md) · [Host Wiki](../../../docs/editor/Inno.Editor.Hosting.md)

## 结果与范围

本地专项验收通过。新项目首次打开时，标准 Host 为不存在的 `editor.ini` 原子安装默认文档，然后再加载 presentation。布局采用当前 FlappyBird 的通用停靠比例；原始样例文件 SHA-256 为 `a62d29ad965db98e7ba1ebab357b5444b58633530922daa9a82db0c992758cca`，未修改该文件。

左上 Scene/Game 并排、左下 File、中间 Hierarchy、右上 Inspector/Shader Editor 标签、右下 Stats/Console 标签。模板不包含场景、Asset ID、Plugin 窗口、导航状态、Selection、History、编译或导出弹窗；屏幕绝对位置不从样例复制。现有布局（包括刻意留空的文件）在初始化时保持原样；正常使用后的统一状态保存仍会记录最新布局。

本项不代表完整 UI 原计划已经闭合。PR 保持 Draft；最终提交的远端 CI 独立报告，issue 合并时才关闭。

## 实际职责与文件树

```text
src/composition/editor/
├── framework/Inno.Editor.Core/
│   ├── Runtime/EditorContext.cs
│   ├── Runtime/EditorLayoutSettings.cs
│   └── Inno.Editor.Core.csproj
└── hosting/Inno.Editor.Hosting/
    ├── Layout/
    │   ├── DefaultEditorLayout.ini
    │   └── EditorDefaultLayout.cs
    ├── Hosting/EditorHost.cs
    └── Inno.Editor.Hosting.csproj

tests/editor/Inno.Editor.PlayMode.Tests/
├── EditorLayoutInitializationTests.cs
└── DefaultLayoutNativeTests.cs
```

Host 拥有默认窗口与布局数据，Core 不知道标准面板或示例项目。唯一新增公开边界是现有 Host-only `EditorContext` 构造入口的可选 `initialLayoutDocument` 参数；它传递中立文档，不进入 Scripting API，也不要求项目作者填写配置。保留一个构造实现，没有旧 overload 或转发层。

首次创建和普通保存都复用 `Inno.Core.IO.AtomicFile`，删除了原先私有固定 `.tmp` 写入协议。首次安装使用 `overwrite: false`，并发创建后读取实际完成者的文档；真实 IO 失败传播，不发布半成品。Core IO 引用属于实现依赖，没有泄漏到公开签名。

## 本地验收

证据根为 `D:/InnoEngine-Verification/editor-ui-closure-2026-10-27/e47`。本节记录基于 `870a0af0` 的工作区候选，源码/资源哈希保存在 `source-inputs.json`；提交后以 PR 的实际 head 与 CI 为准。

| 检查 | 结果与证据 |
| --- | --- |
| 新增专项 | 9 项通过，`initial-layout.trx`：缺失创建、已有/空文件保护、16 路并发、失败清理、保存重开、无默认值时的被动 Context，以及两组真实 Native docking/resize。 |
| PlayMode 相关套件 | 150 项通过，`layout-related-suite.trx`；包含新增 9 项，不能再次相加。 |
| Interactions | 131 项通过，`Inno.Editor.Interactions.Tests.trx`；覆盖现有状态、History 与设置工作流。 |
| Hosting | 10 项通过，`Inno.Editor.Hosting.Tests.trx`；预取消和配置拒绝继续不创建项目。 |
| 标准 Editor Debug | 构建通过，0 编译错误；`layout-product-build.log/binlog`。保留 4 个 NU1900 网络/漏洞服务警告。 |
| 完整 SourceDebug Solution | 构建通过，0 编译错误；`layout-source-solution.log/binlog`，同样保留网络警告。 |
| 当前 Player 引用闭包 | 正式项目构建通过；`wiki-player-reference-build.log/binlog`。不借用 Editor 或默认目录的引用。 |
| 架构/XML/Wiki/示例 | 最终通过，241/241 SDK 工程归属；`wiki-final-gate.log`、`wiki-final/api.json`、`wiki-final/examples.json`。 |
| 实际空项目首开 | 隐藏窗口 D3D12 Host 完成 300 帧并有序退出；`first-run-hidden-result.json` 与 stdout/stderr。自动生成的 INI 含八个实际 docked 窗口，没有示例或 Untitled 场景。 |

Native 几何测试通过真实 `ImGuiEditorRuntime` 加载标准 dock tree，再从 1600×900 调整到 1280×720、800×600，检查窗口边界、列比例、上下关系和两个标签组。它没有创建桌面操作或冒充物理显示器视觉验证。实际隐藏 Host 进一步证明嵌入资源进入了正式产品初始化流程；隐藏运行不证明人工视觉体验。

首轮 Wiki 门禁发现新构造签名映射、API 页中的外部样例引用，以及独立输出 owner 缺少选定 Player 闭包，共五项诊断。签名按 SDK 清单更新，样例出处留在本验收记录，Player 按正式工程构建。最终门禁全部通过，没有忽略或降低校验规则。

## 深层自查

- 默认值归组合 owner，Core 只接收不可变字符串并管理同一可读文档；没有平台名单、渲染 API 分支或领域类型反向依赖。
- 原子创建、失败清理、保存和竞态复用 Core IO；没有平行 Writer、事件系统、身份表、Workspace 或格式兼容机制。
- 首次初始化发生在 presentation 读取之前；已有文档的读取权威、模块恢复与退出原子保存顺序不变。没有隐式创建 Scene。
- 未修改 BGCS、NuGet 包、第三方或生成绑定，没有新增 global/implicit using、IVT、私有反射或生产测试入口。
- 用户停止电脑操作后未再使用 computer use；本专项通过原生 Context 测试与隐藏 Host 验收，不操作桌面焦点。

指定的 `/System/Library/Sounds/Glass.aiff` 在本 Windows 环境不可用，未播放。
