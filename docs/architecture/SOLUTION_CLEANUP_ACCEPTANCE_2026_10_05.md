# Solution 与项目文件整理验收（2026-10-05）

[架构索引](README.md) · [通用开发规范](CSHARP_DEVELOPMENT_STANDARD.md) · [平台扩展示例](PLATFORM_EXTENSION_GUIDE.md)

## 范围与结果

- InnoEngine 删除空 `build/verification` Solution Folder；82 个分组变为 81 个。
- BGCS 将八个生产项目直接放在 `src`，删除 `core/runtime/generator/applications` 四个冗余包装分组；
  七个分组变为 `src`、`tests`、`tests/fixtures` 三个，并统一 fixture 分组大小写。
- InnoEngine Solution 保留 202 个实际项目；BGCS 保留 22 个。项目路径、GUID 和 Debug/Release 配置不变。
- 扫描两个仓库全部 227 个创作 `.csproj`，整理 153 个非测试项目：InnoEngine 143 个，BGCS 10 个（含两个 example）。
  74 个测试/fixture 项目文件未修改，测试源码和运行行为未修改。
- 工程文件统一两空格缩进、LF、职责空行、长属性列表排版和 `/` ProjectReference 路径。
- 删除 `Inno.Build` 与 `Inno.Scripting.Reload` 的两个空 ItemGroup；合并 60 个项目内共 81 个条件和可见性一致的相邻引用分组。
  Editor 实现依赖与公开 API 依赖仍分别分组，PrivateAssets 保持原值。
- 两仓库 `.editorconfig`、`.gitattributes`、通用规范副本及 AGENTS 记录工程整理规则，确保 Windows checkout 后仍保持 LF。

## 语义核对

证据根为 `artifacts/acceptance/2026-10-05-solution-cleanup`。
`before` 以 `.snapshot` 后缀保留本轮修改前的项目与 Solution 内容；不是 Git HEAD 的历史基线，
也不作为源码项目参与架构扫描。

- 每个项目比较有序 XML 模型：属性/条件/metadata/Import/Target 保持；
  仅允许无内容组删除、等价相邻引用组合及相对路径分隔符规范化。
- Solution 项目声明和全部非 NestedProjects 全局配置逐字核对；
  重设分组父子关系后检查无空分组、无悬空父节点、无缺失项目文件。
- `cleanup.json` 保存逐文件前后 SHA-256、分组删除/合并和路径整理记录。
- `verification.json` 保存最终文件复核、规范副本及链接检查状态。

## 当前验证入口

```text
dotnet build InnoEngine.sln -c Release --disable-build-servers -m:1 -nodeReuse:false
dotnet run --project build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj -c Release --no-build -- verify . --configuration Release

dotnet build BindGen-CS.sln -c Release --disable-build-servers -m:1 -nodeReuse:false
dotnet run --project src/BGCS.Tool/BGCS.Tool.csproj -c Release --no-build -- validate architecture .
dotnet run --project src/BGCS.Tool/BGCS.Tool.csproj -c Release --no-build -- validate style src
dotnet run --project src/BGCS.Tool/BGCS.Tool.csproj -c Release --no-build -- validate documentation src
```

本轮使用项目声明的 SDK：InnoEngine 9.0.318、BGCS 10.0.401；Windows x64 宿主。
日志为 `inno-build.log`、`inno-architecture.log`、`bgcs-build.log`、`bgcs-architecture.log`、
`bgcs-style.log`、`bgcs-documentation.log` 及两个 `*-solution-list.log`。

## 结论

| 验证 | 本轮结果 |
| --- | --- |
| InnoEngine 整套 Release Solution 构建 | 通过，0 warning / 0 error，8 分 1 秒 |
| InnoEngine 架构验证 | 通过 |
| BGCS 整套 Release Solution 构建 | 通过，0 warning / 0 error，47 秒 |
| BGCS 架构验证 | 八个生产项目，0 violation |
| BGCS C# 风格 | 0 个需要整理的文件 |
| BGCS 公开 XML | 3,226 个可访问声明，0 violation |
| 项目有效 XML/最终散列 | 153 个全部通过 |
| Solution 有效项目和配置 | 202 / 22 个项目，身份与配置保持，清理后无空分组 |
| 两仓库 Wiki 相对文件链接 | 252 页，0 个失效链接 |
| 当前改动范围 Git whitespace 检查 | 两仓库通过 |

首次验证命令使用了不支持的 `--root` 选项，已改为公开的 positional root 和 Release configuration。
本轮备份初次使用 `.csproj` 后缀导致全仓库项目扫描识别了副本，随后改为 `.snapshot` 并重新通过验证；
原始日志分别保留在 `inno-verify-invocation-error.log` 和 `inno-verify-snapshot-collision.log`。
这两个执行问题均未修改生产 C# 或架构规则。

没有运行逻辑/API 变化，本轮没有重跑游戏导出或单元测试矩阵；整套 Solution 构建包含现有测试项目的编译。

本轮属于 Solution/工程组织与排版，不重新宣称整个跨平台运行矩阵已验收。
当前平台实际运行及 macOS/iOS/主机等边界继续见[完整重构验收](PLATFORM_RUNTIME_ACCEPTANCE.md)。
没有自动提交。当前 Windows 主机无法访问指定的 `/System/Library/Sounds/Glass.aiff`，提示音未播放。
