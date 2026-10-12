# NuGet 依赖图提交

[统一构建](README.md) · [Wiki 首页](../README.md) · [绑定生成](Inno.Build.Bindings.md)

## 职责与入口

[`nuget-dependencies.yml`](../../.github/workflows/nuget-dependencies.yml) 准备真实的源码依赖并还原 Solution，随后提交解析后的 NuGet 依赖图。
它不发布 NuGet 包，不执行 Native 编译、绑定生成或产品验收。
该流程属于 CI 环境准备，不改变平台、backend 或构建库的依赖边界。

```text
GitHub Actions workspace/
├─ InnoEngine/                 本次提交
│  ├─ InnoEngine.sln
│  └─ dependencies/
│     └─ BindGen-CS/           引擎 Git submodule 固定的 commit
│        ├─ global.json
│        └─ src/
│           ├─ BGCS/
│           ├─ BGCS.Cpp2C/
│           └─ BGCS.Runtime/
```

BGCS revision 只由 Git submodule 声明；workflow 初始化该依赖，不维护第二份 commit 或同级 checkout。
两个 SDK 分别从各自 `global.json` 安装。Browser 源码宿主现在为普通 SDK Library；Wasm workload 属于真实发布工程，不是源码依赖图还原前提。
使用 `-p:Configuration=SourceDebug` 还原完整源码 Solution；默认 Debug/Release 仅还原轻量 entry 闭包。不扫描第三方 C++ 工程作为 NuGet 根。

PR 执行完整还原检查；push 和手动运行才提交 snapshot。
自动触发覆盖 `master`、`main` 和当前 `kernel` 分支的项目/构建声明修改。
提交步骤需要 `contents: write`；fork PR 的 token 由 GitHub 降为只读，且不执行提交步骤。
该任务不使用 `pull_request_target`，checkout 不持久保存凭据。

## 为什么 GitHub 自动任务反复失败

2026-10-10，任务 [38027308770 / 114140780174](https://github.com/Wolfiora/InnoEngine/actions/runs/38027308770/job/114140780174?pr=11)
在 `validate-project` 还原 `Inno.Build.Tasks` 时失败。
日志包含 `MSB4025`：同级 `BindGen-CS/src/BGCS`、`BGCS.Cpp2C` 和 `BGCS.Runtime` 项目文件不存在。

GitHub 管理的 `Automatic Dependency Submission (NuGet)` 仅 checkout InnoEngine，不能准备这个仓库的同级源码依赖。
其 NuGet cache 恢复和 .NET 9.0.318 选择都成功，错误发生在 snapshot 提交之前。
重复运行相同自动任务仍会缺少 BGCS，清理 NuGet cache 不能补上源码。

## 切换仓库设置

仓库自管 workflow 不会自动替换 GitHub 的 dynamic workflow。完整切换步骤：

1. 提交并推送本 workflow，确认 `NuGet dependency graph` 的还原与 snapshot 提交成功。
2. 打开仓库 **Settings → Advanced Security → Dependency graph**。
3. 将 **Automatic dependency submission** 设置为 **Disabled**，保留 Dependency graph 和 Dependabot alerts。
4. 若分支保护规则要求旧 `dynamic / submit-nuget`，将该项替换为新 workflow 的检查。

这些设置不属于仓库文件，本次本地改动没有修改它们，也没有自动提交或推送。
GitHub 的说明见[自动任务配置](https://docs.github.com/en/code-security/how-tos/secure-your-supply-chain/secure-your-dependencies/submit-dependencies-automatically)
和[自动提交实现与限制](https://docs.github.com/en/code-security/reference/supply-chain-security/automatic-dependency-submission)。

## 验证范围

2026-10-10 入口整改前的历史验证通过：YAML 结构、固定 BGCS revision、源码路径、文档链接及完整 Solution 还原。
使用 engine SDK 9.0.318 与已安装的 wasm-tools，独立 `--artifacts-path` 保存还原产物；
236 个实际还原项目路径覆盖 Solution 的 229/229 个项目及 7 个 BGCS 引用项目，无缺失。
命令为 `dotnet restore InnoEngine.sln --disable-parallel --artifacts-path <本次验收目录>/restore`，退出码 0。
日志和覆盖结果位于 `artifacts/acceptance/nuget-ci-2026-10-10/`。
GitHub runner、snapshot API 和仓库设置切换仍须由实际 workflow 运行确认；本地还原成功不能替代远端结果。

`rendering-ci.yml` 已同步使用 entry 的 Build、Editor 和高级命令，并显式构建 SourceDebug。远端 Windows/macOS 产品矩阵尚未运行，本地源码修改不表示 CI 已通过。
开发入口与固定依赖路径的当前验证范围见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。
