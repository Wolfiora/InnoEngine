# 仓库文件归属与残留清理验收（2026-10-25）

## 范围与结论

检查 InnoEngine、Canvas、Rendering2D、Samples 的本地工作区及 `git fetch origin` 后的全部远端分支树。基线为 InnoEngine `f406f9ad7472246cd79d4477489a7618de2269f9`；四个工作区开始时均干净。

- 当前 `dotnet10` 没有游离 NuGet 锁文件：725 个文件全部被本轮重新生成的真实 SDK 还原图选择，无额外或缺失文件。
- Rendering2D 的一份测试锁文件有实际工程消费者，locked restore 通过。Canvas、Samples 没有创作源码锁文件；生成的 IDE 工程及其输出属于 Project Library 等缓存 owner。
- 清理本地已退出工程、SDK 流程及旧 TFM 输出的 99,594 个文件、7,303 个目录，逻辑大小 16,962,237,308 bytes，约 15.8 GiB。没有删除任何已提交的源码或依赖清单。
- 验证结束后通过正式 cache maintenance 再回收 731,876,332 logical bytes，保护 174,121,583 bytes，延期项为 0；C 盘最终空闲约 96.1 GiB。
- 删除旧源码缓存位置的 ignore 规则、重复 ignore 项和 Native 检查豁免。有效的 `obj` / `artifacts` 输出仍由目录 owner 忽略及管理。
- 远端较早分支保留其完整代码快照，不跨分支只删锁文件、不删除分支、不改写历史。此次新增源码变更仅本地提交，不推送。

这是一轮文件归属、配置消费者和残留清理验收，不代表证明全部业务代码没有死代码，也不替代跨平台产品验收。

## 远端快照

| 仓库 / 分支 | 检查时 revision | NuGet 锁文件 | 处理 |
|---|---|---:|---|
| InnoEngine `origin/dotnet10` | `f406f9ad` | 725 | 与本地基线一致，重新核对消费者后全部保留 |
| InnoEngine `origin/master` / `origin/HEAD` | `c4de94e9` | 994 | 尚未包含当前分支的后续改造；其中 531 个采用旧 Native target 命名。保留此分支的完整快照 |
| InnoEngine `origin/code-format` | `1f53d664` | 664 | 较早代码快照；其中 363 个采用旧 Native target 命名。没有单独删文件 |
| InnoEngine `origin/legacy` | `c0035324` | 0 | 历史分支保留；分支名称不等于当前执行链存在兼容实现 |
| Canvas `origin/main` | `eed5fd9` | 0 | 本地与远端一致，无待清理源码文件 |
| Rendering2D `origin/main` | `b5b501c` | 1 | 本地与远端一致，测试工程实际使用 |
| Samples `origin/main` | `6eb52f3` | 0 | 本地与远端一致，无待清理源码文件 |

要让 `master` 使用当前规则，应正常集成完整的 `dotnet10` 变更。只删除它的旧文件会破坏该分支自己的构建契约；当前任务没有执行跨分支集成或远端写入。

## NuGet 清单与配置消费者

本轮再次执行 19 组 `GenerateRestoreGraphFile`：Solution SourceDebug、两个正式入口、Editor Debug/Release、Windows/Browser/macOS Player、中立/真实 RID/link/AOT 请求、macOS 宿主路由 fixture，以及两个生成器的显式 RID 请求。

725 个当前清单的用途分布如下：

| 清单名称 | 数量 |
|---|---:|
| `packages.lock.json` | 233 |
| `packages.host-win-x64.lock.json` | 1 |
| `packages.host-osx-arm64.lock.json` | 1 |
| `packages.rid-win-x64.lock.json` | 75 |
| `packages.rid-osx-arm64.lock.json` | 72 |
| `packages.rid-browser-wasm.link.lock.json` | 67 |
| `packages.rid-win-x64.aot.lock.json` | 69 |
| `packages.rid-win-x64.link.lock.json` | 69 |
| `packages.rid-osx-arm64.aot.lock.json` | 69 |
| `packages.rid-osx-arm64.link.lock.json` | 69 |

相同内容不表示清单无用；SDK 请求的实际 RID、隐式包能力和宿主引用图仍决定选择。没有为了减少数量剥除任何 SDK 属性。

同时核对当前配置的真实归属：238 个源码工程由正式架构校验器执行 SDK 求值；七个 `Bindings/bindgen.json` 经各组件唯一的 `bindings.props` 消费；七个 `Native/Generated/compilation.json` 属于当前桥接编译输入；平台 Player 模板和 props 继续归属所属平台；根级 SDK、Solution、Git、EditorConfig 和 CI 文件保留其标准或显式入口。没有把通配编译纳入的 C# 文件仅因缺少文本文件名引用判定为无用。

## 已清理的本地残留

以下 21 个目录不包含任何 tracked 文件，当前项目图没有这些旧工程；有文件的目录仅剩已退出工程的 `bin/obj`。检查中核对了完整文件清单、SHA-256、原 assets 的工程身份、实际路径、reparse point 和活动进程。删除前再次核对哈希；未知或发生变化的内容不会进入删除。

```text
backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/
build/pipeline/Inno.Build.Cli/
build/support/Inno.Build.SupportPacks/
build/toolchains/Inno.Build.Toolchains/Platforms/
build/toolchains/Inno.Build.Toolchains.Host/
build/verification/
docs/platform/Linux/
entry/Editor/
entry/Inno.Entry.Bootstrap/
entry/Rebuild/
platforms/MacOS/
platforms/Windows/integrations/
src/adapters/input/Inno.Adapter.Input.Sdl3/
src/composition/editor/host/
src/composition/editor/panels/Inno.Editor.Panel.ShaderGraph/
src/services/rendering/Inno.Rendering.ShaderGraph/
tests/audio/Inno.Adapter.Audio.MiniAudio.Tests/
tests/Infrastructure/
tests/native/
tests/rendering/Inno.Adapter.Rendering.Bgfx.Tests/
tests/rendering/Inno.Rendering.ShaderGraph.Tests/
```

这些目录共 2,237 个文件。另外清理 21 个旧 SDK publication 锁、5 个旧 binding definition 锁，以及 ImGui/UI/Text 三个旧 `Bindings/.bgcs` 工作区中的 3 个零字节锁。锁文件删除前要求独占打开；它们是已退出 producer 的叶文件，未触及当前协调域。

当前 BGCS 请求在 `BindingGenerationConfiguration.Load` 固定缓存到 `artifacts/build-tools/bindings/sdk-workspaces/<component>/<target>`；候选输出由 `BindingGenerationPublication` 放入受 owner 管理的工作目录。`BindingGenerationDiagnostics` 复用 Core IO lease 和现有缓存 publisher。上述源码位置已经没有当前 producer，清理不是把仍在使用的普通锁按后缀删除。

进一步检查仍存在的源码工程，发现另外 1,892 个标准 SDK `bin/obj` 下的 `net9.0` 输出根：InnoEngine 1,888 个、Canvas 4 个。这些工程经本轮 SDK 还原图及 Canvas 独立求值确认，当前目标为 `net10.0` 或 `net10.0-browser`；源码和配置没有指向这些旧输出的实际引用。不是仅凭目录名删除：逐项核对工程、当前 TFM、Git ignored/非 tracked 归属、文件集合、mtime/长度、reparse point 和活动进程，删除时独占打开文件；不修改文件属性或通过终止进程强行释放。当前 `netstandard2.0` 生成器没有进入退休集合。

这部分清理 97,328 个文件、6,977 个目录，逻辑大小 16,741,487,821 bytes，保护/失败项为 0。目录按文件清单逐一移除，再仅删除已空目录，未递归删除未知内容。逐文件 inventory 留在验收证据中，不进入生产构建。

第一阶段 C 盘空闲空间观察值从 86,343,614,464 增至 86,568,595,456 bytes；旧 TFM 阶段从 85,834,326,016 增至 102,579,118,080 bytes。阶段间正式验证生成了当前 Task 工具缓存；前后值也包含同期系统活动，不能当作每个文件实际占用的精确加总。旧 TFM 清理后 C 盘空闲约 95.5 GiB。没有清理用户、SDK、全局 NuGet 缓存、当前产品或历史验收证据。

最终使用正式 CLI `clean --scope cache --cache-time-seconds 600`，先 dry-run 再执行；由既有 owner 协调与 fresh 校验退休 17 个无读取保护的工作缓存/旧 Task publication，回收 731,876,332 logical bytes，保护 174,121,583 bytes，延期项 0。此阶段空闲空间观察值从 102,571,298,816 增至 103,161,008,128 bytes，未将 hardlink 的逻辑大小等同于实际释放空间。连同前述残留，逻辑清理量约 16.5 GiB。

## 源码与配置调整

- `.gitignore` 删除已退出的 `project.lock.json` / `project.fragment.lock.json`、源码侧 `.bgcs` / `.bindgen-cache`、全局 BGCS publication / generation lock 忽略；删除重复的 Rider 和旧 VS 临时文件条目。标准 `obj` / `artifacts` owner 仍覆盖当前运行锁。
- `NativeBindingsVerification.AuditNativeImports` 删除旧 `.bindgen-cache` 目录豁免及重复的大小写判断。新增手写 import 不能再靠旧目录名绕过该检查。
- `AGENTS.md` 记录 SDK 缓存归属、残留删除证据和远端分支处理规则。
- 不修改游戏/插件 API、平台 Profile、Native recipe、BGCS 源码/包、第三方源码或生成的 Bindings。没有新增公共 API、清理服务、兼容 reader 或依赖解析器。

## 验证

| 检查 | 结果 |
|---|---|
| 19 组本轮真实 SDK 还原图 | 通过；725 selected / 725 tracked，0 多余、0 缺失 |
| 当次 SDK 依赖检查，Solution SourceDebug | 通过；232 个 SDK 工程、当次依赖输入不变、BGCS 4.1.2；当前还原流程见下文 |
| Rendering2D 测试工程当次依赖还原，独立 ArtifactsPath | 通过；当前使用普通 SDK Restore |
| 正式 CLI 架构检查，SDK project graph | 清理后再次通过；238/238 工程 |
| Build 入口 Debug 编译，warnings as errors | 清理前后均通过；最终 50.19 秒，0 个 MSBuild 警告、0 个错误 |
| ImGui 官方 `CheckBindings` | 通过；BGCS 正式生成并比对，checked-in 生成物不变；源码侧旧 SDK 缓存和锁没有重新出现 |
| 清理后源码目录再次扫描 | 通过；上述残留目录均退出，插件的合法空 `Plugins` 目录保留 |
| 最终源码/生成物/包完整性、Git 与 ignore 检查 | 通过；725 个清单、全部 tracked 生成物、130 个 BGCS 包文件、三个相邻仓库源码不变；无游离清单，`git diff --check` 通过 |
| FlappyBird 两个 `.iplugin` 包内容检查 | 通过；48 + 206 个条目无构建输出、依赖锁、IDE 目录或临时/备份文件；包未改动 |
| 正式缓存维护 | 通过；先 dry-run 后执行，使用既有协议，延期项 0 |

本次没有改动领域运行行为，不重复执行上一轮完整 FlappyBird、GPU 或 Browser 产品矩阵。macOS 仍无设备，未进行实机验收。

BGCS 在 `CheckBindings` 中仍打印既有的 `BGCS-SAFETY-OWNERSHIP` 安全事实告警及 volatile preprocessing 导致缓存禁用的说明；没有改动或屏蔽。MSBuild 的 0 warning 终态不表示这些 SDK 诊断已解决，本任务未扩大到绑定语义改造。

## 深层自查

检查了真实 SDK 还原路径、宿主与目标上下文、平台配置归属、BGCS 请求的 cache/output 重定向、候选发布和锁生命周期。当前依赖清单没有第二份手工注册表，清理清单只保存在本轮证据中，没有进入生产执行链。普通锁继续由既有 Core IO / owner 协议管理；此次删除限定为已退出 owner 的残留，没有新增上层事件、身份或缓存管理系统。

负向测试中用于拒绝旧入口的路径字面量、热重载状态转移和历史验收记录仍有实际用途，不因为出现 `Rebuild`、`migration` 或较早版本号就机械删除。当前源码配置未发现需要额外删除的兼容执行路径；这不是对整个引擎作零缺陷保证。

证据保存在 `D:/InnoEngine-Verification/repository-hygiene-2026-10-25/`：远端/本地 inventory、19 个 SDK 图及 `current-restore-ownership.json`、逐文件 retirement hash 清单、`retirement-result.json`、locked restore 日志、架构图、编译日志/binlog 和最终完整性检查。该路径仅为本机验收位置，没有写入生产构建选择。

当前依赖规则及其独立验证见[标准 NuGet 还原验收](STANDARD_NUGET_RESTORE_ACCEPTANCE_2026_10_26.md)；本页仅保留当次仓库卫生证据。Windows 不存在指定的 `/System/Library/Sounds/Glass.aiff`，完成提示音无法播放。
