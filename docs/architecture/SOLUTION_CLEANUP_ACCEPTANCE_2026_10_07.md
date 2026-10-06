# Solution 覆核与架构边界验收（2026-10-07）

[架构索引](README.md) · [Wiki 首页](../README.md) · [项目与依赖总览](ENGINE_ARCHITECTURE_OVERVIEW.md) · [开发规范](CSHARP_DEVELOPMENT_STANDARD.md)

## 1. 范围与结论

本轮检查 InnoEngine 的全部创作源码 `.csproj`、Solution 项目/分组/配置、MSBuild 有效引用和现有架构门禁。
发现一个遗漏项目并补齐归属；没有发现应删除的废弃项目、空 Solution Folder、失效项目或空项目文件分组。
构建另复现一个 Task 缓存退休错误，在原有构建所有权边界修复。

当前检查范围内，项目归属完整，引用方向和关键运行闭包符合现有架构约束。
这次没有新增领域 API、平台分支、兼容层或生产 Program，也没有修改测试源码、生成绑定、第三方源码或 BGCS。
游戏运行、Web/AOT 发布和 macOS 实机不属于本轮重新执行的验证；此前证据仍见
[2026-10-06 架构整改验收](ARCHITECTURE_CLEANUP_ACCEPTANCE_2026_10_06.md)。

## 2. 环境与基线

- 基线 revision：`f472f1183e42cb67df777a745e5c83ebb386e660`，开始时工作区干净。
- Windows x64，系统报告 `Microsoft Windows 10.0.26200`。
- SDK：`.NET 9.0.318`，宿主 `C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe`。
- 本轮改动未自动提交。
- 原始证据位于 `artifacts/acceptance/2026-10-07-solution-cleanup/`；该目录是可重建验收产物。

## 3. 发现与修正

### 3.1 ImGui 绑定生成扩展遗漏在 Solution 之外

`native/Inno.Native.ImGui/Bindings/Extension/Inno.Native.ImGui.BindingExtension.csproj`
由现有 `BindGenExtensionProject` 与生成配置使用，提供组件自己的 ABI layout 和 managed 模板。
它不是废弃项目。现已加入已有 `native` 分组，补齐 Debug/Release 的 Any CPU、x64、x86 配置映射，
未建立额外单项目包装层，也未增加 Player 对生成工具的运行时依赖。

架构验证器原先只检查 `src`、`build` 与 `tests` 的 Solution 覆盖；现将生产项目检查统一扩展到
`src`、`native`、`build`、`tools`。Native 组件及其绑定扩展归属 `native`，验证工具归属 `tools`。
AGENTS、通用开发规范和对应 Wiki 同步记录该要求。

### 3.2 Windows 拒绝进程查询导致有效构建失败

第一次 Release Solution 构建失败于 `BuildTaskHostRetirement.IsProcessAlive`：查询 owner 的
`Process.HasExited` 抛出 `System.ComponentModel.Win32Exception`，异常穿过可延后的缓存清理并导致 Shader Task 失败。
受保护系统进程 PID 0、4 的公开进程查询均复现同类访问拒绝。

修复留在 `Inno.Build.Tasks`：不能可靠查询 owner 时保留加载目录；只有确认全部 owner 已退出才退休。
这不放宽 Shader 编译、Native 完整性或部署门禁，也不删除无法证明闲置的缓存。
首次失败日志保留为 `logs/solution-release-build-initial.log`。

## 4. Solution 与项目文件清洁度

| 检查 | 结果 |
| --- | --- |
| 创作源码项目 | 211：146 个生产项目、65 个测试及 fixture 项目 |
| Solution 项目 | 210 → 211，全部源码项目都有归属 |
| Solution Folder | 86，全部有有效项目、子分组或 Solution Items |
| 空分组、失效 Solution Item | 0 |
| 丢失项目、重复项目路径/GUID/名称 | 0 |
| 游离嵌套关系、重复父分组、嵌套环 | 0 |
| 缺失或重复的项目配置映射 | 0 |
| 无源码项目、空 PropertyGroup/ItemGroup | 0 |
| 项目文件 Tab | 0 |
| MSBuild 有效引用 | 1789 条，目标全部存在；包含共同注入的 analyzer 和外部 BGCS 消费引用 |
| 有效引用环 | 0 |

四个没有普通入站 ProjectReference 的生产项目分别是 Editor、Desktop Player、Browser Player
入口和上述绑定生成扩展，均有明确用途。没有因“无人引用”而误删宿主或 MSBuild 工具项目。
现有 `.csproj` 已满足本轮检查，未进行无行为收益的重新排序或批量排版。
Browser 项目的 Solution 编辑归属与目标发布入口保持现有规则；宿主 Solution 编译不替代 Web 发布验收。

## 5. 架构复核

有效引用闭包区分编译/运行程序集引用与 `OutputItemType=Analyzer`、`ReferenceOutputAssembly=false`
的构建依赖，避免把共同生成器误认为运行时耦合。1523 条代码引用边参与关键闭包检查。

| 边界 | 当前结果 |
| --- | --- |
| Rendering Core | 代码依赖闭包仅包含 Foundation，不含资产、创作、具体 Adapter 或 Native |
| Rendering Assets | 不含 Rendering Runtime、Shaders 或 Assets Pipeline |
| Rendering Runtime | 不含 Assets Authoring 或 Assets Pipeline |
| Content | 代码依赖闭包仅包含 Foundation |
| 通用输入 | 无 SDL 或 Native 依赖 |
| Player Runtime | 无 Editor、Authoring、Build 或 DotNet 动态模块实现 |
| 通用 Build | 不反向引用 Inno.Build.Composition |

仓库架构门禁还检查公开签名、Editor 引用可见性、Native 消费限制、脚本导出、源码规范和代际清理约束。
上述结果支持“当前程序集分层和依赖边界清楚”的结论；静态门禁不证明任意插件的动态线程或资源行为。
Native、平台宿主与工具链仍有其正常平台职责，不应把这些叶层实现误认为核心污染。

## 6. 实际验证

以下命令使用上述 SDK 宿主，从仓库根执行；完整日志保存到本轮证据目录。

```powershell
dotnet build InnoEngine.sln --configuration Release -nr:false --nologo
dotnet build InnoEngine.sln --configuration Debug -nr:false --nologo
dotnet build/cli/Inno.Build.Cli/bin/Release/net9.0/Inno.Build.Cli.dll verify . --configuration Release
dotnet test tests/tooling/Inno.Tooling.Architecture.Tests/Inno.Tooling.Architecture.Tests.csproj --configuration Debug -nr:false
```

| Gate | 结果与证据 |
| --- | --- |
| Release 完整 Solution | 通过，0 warning / 0 error，4 分 24 秒；`logs/solution-release-build.log` |
| Debug 完整 Solution | 通过，0 warning / 0 error，4 分 54 秒；`logs/solution-debug-build.log` |
| 仓库架构门禁 | Debug/Release 均通过；`logs/architecture-debug-after.log`、`logs/architecture-after.log` |
| 现有架构测试 | 77 passed / 0 failed / 0 skipped；`results/architecture-tests.trx` |
| Solution 覆盖正反例 | 3 项通过：全部归类、遗漏 Native、遗漏 Tooling；`results/solution-guard-results.json` |
| Task owner 退休 | 3 项通过：不可查询 owner 保留、活跃 owner 保留、已退出 owner 删除；`results/task-cache-retirement.json` |
| Wiki 完整性 | 146 个生产项目均有页面，117 个本轮文档链接有效；`results/documentation-checks.json` |
| 全项目与配置盘点 | `results/solution-audit.json`、`results/project-inventory.json`、`results/solution-folders.json` |
| MSBuild 有效图与关键闭包 | `results/effective-project-references.tsv`、`results/effective-reference-audit.json`、`results/architecture-closure-checks.json` |

Solution 正反例通过真实 CLI 验证公开行为；小型 fixture 没有编译元数据，断言只针对项目覆盖诊断，
不把它们记录为完整架构门禁通过。缓存退休验收使用实际 MSBuild Task 的构建流程，未通过反射访问内部实现。

## 7. 交付边界

本轮仅修改 Solution、架构验证器、Task 内部缓存退休、规范及 Wiki；所有公开 API 与运行流程保持现有契约。
未自动提交。指定 `/System/Library/Sounds/Glass.aiff` 在 Windows 环境不存在，提示音未播放。
