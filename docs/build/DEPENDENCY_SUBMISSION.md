# NuGet 依赖图提交

[统一构建](README.md) · [Wiki 首页](../README.md) · [绑定生成](Inno.Build.Bindings.md)

## 当前依赖来源

BGCS 生成器与 Runtime 完全由 NuGet 恢复。`build/msbuild/Inno.Packages.props` 固定包族版本，项目求值选定的 NuGet 锁文件保存解析版本和内容哈希。引擎不要求 BGCS 源码仓库。

Native 第三方源码属于 `extern/`，不是 NuGet 依赖图的根。Browser Player 的 Wasm workload 由实际发布工程使用，默认开发入口还原不应要求它。

## 工作流

[`nuget-dependencies.yml`](../../.github/workflows/nuget-dependencies.yml) 使用引擎 SDK，对 `SourceDebug` 图执行 locked restore。默认 Debug/Release 仅还原两个开发入口；SourceDebug 才包含中立源码、构建库和测试。

```powershell
./tests/Invoke-NuGetLockChecks.ps1
```

PR 检查还原；push 和手动运行提交解析后的依赖 snapshot。提交权限仅属于该步骤，fork PR 不执行写入步骤。工作流不发布包，不编译 Native，不代表产品运行验收通过。

## 锁文件的唯一归属

所有创作源码工程继续参与逐工程依赖锁定。`Inno.NuGetLockFiles.targets` 是唯一命名规则；NuGet 自己解析依赖、框架、包资产和内容哈希。

| 工程实际输入 | 清单 |
| --- | --- |
| 没有 SDK RID | `packages.lock.json` |
| 明确 SDK RID | `packages.rid-<RID>.lock.json` |
| Editor 入口的宿主路由 | `packages.host-<SDK宿主RID>.lock.json` |
| 宿主路由与实际 RID 同时存在 | `packages.host-<SDK宿主RID>.rid-<实际RID>.lock.json` |
| SDK 需要 NativeAOT 编译工具包 | 在上述名称的 `.lock.json` 前增加 `.aot` |
| SDK 需要 trimming / analyzer 工具包，且不是 NativeAOT | 在上述名称的 `.lock.json` 前增加 `.link` |

引擎 Native target、配置和输出目录继续按原职责传递，但不会自动扩大清单文件名。普通编译器工具引用保持中立；显式向生成器传入 SDK RID 是不同的还原请求，不能因工具身份而忽略。临时发布、测试和输入求值工程的清单属于它们自己的 owner。

规则通过 SDK 的 `AfterMicrosoftNETSdkTargets` 扩展点执行，保留既有扩展列表，读取 SDK 已推导的发布／analyzer 能力。同一个 RID 的 CoreCLR、NativeAOT 和 trimming 请求可能具有不同隐式 NuGet 依赖，不能合并。例如 `packages.rid-win-x64.aot.lock.json` 包含 SDK 请求的编译工具包；没有更改或屏蔽 SDK 属性。

清单必须提交，不添加笼统的 ignore 规则。`owner.lock` 等跨进程协调文件属于已经忽略的构建输出目录，不能与 NuGet 依赖清单混淆。后者也不属于按字节压力清理的构建缓存。

修改包声明后，使用普通 NuGet 命令明确更新当前正式请求的清单，再执行 locked 检查：

```powershell
dotnet restore InnoEngine.sln -p:Configuration=SourceDebug --force-evaluate
./tests/Invoke-NuGetLockChecks.ps1
```

具有独立 RID、隐式工具包或宿主上下文的正式请求也必须分别更新和检查，不能复制另一个请求的 JSON。检查脚本接受 Project、Configuration、Target、RuntimeIdentifier、ProductProperties、ArtifactsPath、PublishAot 和 PublishTrimmed，并记录 SDK 的实际图、锁路径、assets 输出和包身份。它不解析另一套依赖规则，也不会整理或删除源码清单。

`--force-evaluate` 属于显式更新操作，不能带入 locked 验收：NuGet 会按这个明确请求重新生成清单，即使同时设置了 locked mode。验收只使用 `--locked-mode` 并检查文件没有变化。

CI 同时检查文件集合、内容、时间戳和未跟踪文件。只运行 `git diff` 无法发现新生成的 untracked 清单；还原通过也不能代替源码目录卫生检查。普通开发在依赖未变时不应新增或改写清单。

## 失败定位

- `NU1004`：项目声明与锁文件不一致。更新真实项目图的锁文件后重新 locked restore。
- 包不可取得：核对精确版本、NuGet source 和网络；不回退到源码项目。
- 默认入口要求 Wasm workload：核对产品属性是否泄漏到工具或共同库。
- snapshot 提交失败：查看 GitHub token 权限和服务返回；本地还原成功不能证明远端提交成功。

仓库设置和远端 workflow 状态单独检查，当前结果见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。
