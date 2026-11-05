# .NET 10 / C# 14 / BGCS 3.0.1 当前实施记录

本记录覆盖 Windows x64 工作区正在进行的实施与验收。SDK 为 10.0.402，BGCS 包族精确固定为公共 NuGet 3.0.1；未引用或修改 BindGen-CS 源码仓库。四仓库的已有修改已在实施前记录并保留。

## 已取得的证据

日志根目录：`artifacts/acceptance/bgcs301-2026-10-20`。

| 检查 | 当前结果 | 证据 |
| --- | --- | --- |
| 公共 BGCS 还原及生成库构建 | 通过 | `bindings-build-01.log`、`native-probe-build-10.log` |
| 七组件 HostSource 正式生成 | 通过；ImGuizmo 命名配置另行正式生成 | `components-generation-03.log`、`imguizmo-naming-generation.log` |
| 纯 C HostSource / TargetArtifacts 重复生成与 CheckOnly | 通过 | `pure-c-contract-02.log` |
| 中立原生清单 | 8 项通过，0 失败、0 跳过 | `toolchains-tests-02.log`、`toolchains-tests-02.trx` |
| SDL/ImGui 适配器 Release 编译 | 通过，0 警告、0 错误 | `imgui-adapter-build-04.log`；后续完整闭包构建继续复核 |
| BGFX ImGui renderer Release 编译 | 通过，0 警告、0 错误 | `imgui-bgfx-build-03.log` |
| ImGui 原生冻结副本编译、链接和原子发布 | 通过 | `imgui-native-02.log` |
| ImGui DLL 加载及必需导出 | 1,565 个全部存在；负例符号正确拒绝 | `imgui-exports.json` |
| Text / UI 原生编译及导出 | 最终重跑通过；实际加载 DLL，10 / 37 个必需导出全部存在，负例符号正确拒绝 | `text-native-05.log`、`ui-native-02.log`、`text-exports.json`、`ui-exports.json` |
| 完整 Build 测试引用闭包 | 编译通过，0 警告、0 错误 | `build-tests-build-15.log` |
| 完整 SourceRelease 解决方案 | 当前单一 owner 修正后通过，0 警告、0 错误 | `managed-windows-release-build-04.log` |
| 当前配置、Browser 工具链及真实 CMake 回归 | 8 项通过，0 失败、0 跳过 | `configuration-tests-07.log`、`configuration-tests-07.trx` |
| 生成与工具链统一复跑 | 20 项通过，0 失败、0 跳过 | `binding-final-08.log`、`binding-final-08.trx` |
| 仓库架构及 Wiki 门禁 | 233/233 工程完成有效 MSBuild 归属检查，整体通过 | `architecture-wiki-final-04.log`、`project-graph-final-07.json`；其他平台产品 API 编译按所选目标明确标为未执行 |
| 生成、CheckOnly、取消、损坏、并发及 CMake 专项 | 18 个用例均已有通过记录；统一完整套件仍须复跑 | `binding-generation-tests-04.trx` 中 17/18，通过修正后的 `SourceChanges...` 另见 `binding-task-tests-05.trx` |
| 正式 MSBuild Task、静态选择及传递消费 | 通过；与上述修正回归合计 2/2 | `binding-task-tests-05.log`、`.trx` |
| 平台 SDL integration | Windows、macOS、Browser 三个 Release 工程均编译通过 | `Inno.Integration.*.Sdl3-current-build.log`；不代表 macOS/Browser 运行通过 |
| CI 静态校验 | YAML 解析通过；9 个 PowerShell 脚本在替换 Actions 表达式后语法通过 | `ci-syntax-validation.txt`；尚未运行远端 CI |
| Windows Editor 完整原生支持 | 八组件通过，执行七组件 BGCS 生成及 16 个原生进程 | `windows-editor-support-release-01.log`；原生告警保留 |
| Browser Player 绑定 | 失败；BGFX 按值记录缺少 Browser aggregate ABI profile | `browser-bindings-03.log`，见[SDK 问题记录](BGCS301_BROWSER_AGGREGATE_ABI.md) |
| Canvas 正式脚本及 IDE 投影 | 通过正式 `scripts --ide` 生成 | `canvas-scripts-01.log`；IDE SDK 编译另行验收 |
| 三个当前 Project 正式脚本及 IDE 投影 | Canvas、Rendering2D、FlappyBird 均成功；安装插件的 Assets 与对应源逐文件一致 | `*-scripts-01.log`、`plugin-source-comparison.json` |
| C# 14 及 IDE 隔离专项 | 5 项通过，0 失败、0 跳过 | `csharp14-final-01.log`、`.trx` |
| Host 运行闭包及平台目录契约 | 12 项通过，0 失败、0 跳过；包含实际构建、动态发现和拒绝私有编译引用 | `host-runtime-closure-01.log`、`.trx` |
| 修正后的 Windows Editor Release | 正式入口构建通过，0 警告、0 错误 | `windows-editor-release-02.log` |
| 当前七组件部署 DLL 的必需导出 | 实际逐库加载，4,002 个全部存在；每库负例符号正确拒绝，最终释放全部模块 | `editor-native-exports-final.json` |
| 当前 IDE SDK 构建 | Canvas、Rendering2D、FlappyBird 的 Debug IDE 工程均通过，0 警告、0 错误 | `*-ide-build-02.log` |
| Windows Player Release | 正式产品工程构建通过，0 警告、0 错误 | `windows-player-release-01.log`；不代替最终游戏发布 |
| 测试输入 owner | 中立和 Windows 编译均通过；4 项成功、失败及边界回归通过 | `test-source-library-*-build.log`、`repository-owner-tests-01.trx` |
| 首轮完整运行 | 2,038 通过、55 失败、18 跳过；59 个程序集完成隔离发现，28 个隔离用例全部通过 | `full-release-tests-01.log`、`full-release-first-totals.json`；55 项均为测试源码定位假设，修正后完整复跑中 |
| 第二轮完整运行 | 2,094 通过、3 失败、18 跳过；59 个程序集隔离发现和 28 个隔离用例均通过 | `full-release-second-totals.json`、`full-release-second-failures.json`；三项入口 fixture 已修正，须编译复验 |
| 完整运行与失败项复验 | 合并为 2,097 通过、18 跳过，原三项失败均已有成功复验 | `release-tests-reconciled-01.json`、`build-entry-fixes-final-02.trx`、`architecture-entry-fixes-final-02.trx`；不是重新执行整套，原失败日志保留 |
| SourceDebug 基线 | 完整项目图构建通过，0 警告、0 错误；后续 CLI 显式源码输入修正另做相关 Debug/Release 构建验证 | `managed-windows-debug-build-01.log` |
| CLI 显式源码输入 | 当前 Build entry 与 Build.Tests 的 Debug/Release 四次相关构建均通过，全部 0 警告、0 错误；两配置的入口与源码预检各 11/11 通过 | `*-source-input-*-build-01.log`、`project-source-input-*-01.trx`；包含 1 项新增源码预检回归，不重复计入原有十项 |
| 独立输出的 Canvas 流程 | D 盘 CLI 的正式 scripts --ide 通过；Canvas 测试实际构建及运行 5 通过、1 跳过、0 失败 | `external-cli-source-after.log`、`canvas-evaluated-inputs-final.json`、`canvas-release-final-01.trx`；跳过项明确要求 macOS Metal 工具链 |
| Windows Support Pack | 第二次正式准备、冻结源码编译及发布完整性校验通过；托管阶段 0 警告、0 错误 | `windows-support-pack-final-02.log`；身份 `a1622dac7d4d194030f2ed0e61df83d031d69e4668c0335e68b84b1a21efb0f5` |
| FlappyBird CoreCLR | 正式游戏发布通过；D3D11、D3D12、Vulkan 各完成 60 帧且正常退出，2 Views / 25 Draws | `flappybird-coreclr-publish-final-01.log`、`flappybird-coreclr-smoke-final-02.json`；隐藏窗口，不代替视觉及声学验收 |
| FlappyBird NativeAOT | 正式游戏发布通过；D3D11、D3D12、Vulkan 各完成 60 帧且正常退出，2 Views / 25 Draws | `flappybird-nativeaot-publish-final-01.log`、`flappybird-nativeaot-smoke-final-01.json`；实际游戏场景运行 |
| 当前 Wiki 示例 | 191 项编译通过，0 错误、0 未执行，包含新增测试支持项目页 | `wiki-examples-final-04.json`；实际 Release 调用方及逻辑脚本引用一起校验 |
| 七组件完整输出复验 | Windows x64 的七项 HostSource managed/native 比对，以及七项目标生成后 CheckBindings 均通过 | `check-bindings-reconciled-01.json`；UI Host 清单正式重新生成后复验通过，原失败保留 |
| Editor 产品 smoke | Canvas 的 D3D11 / D3D12 以及 Rendering2D、FlappyBird 的 D3D11 完成帧推进并正常退出 | `editor-*-smoke-*.log`；隐藏窗口，不代替视觉/焦点/声学检查 |
| Vulkan 关闭修正 | 修正前新增真实窗口回归导致 testhost 崩溃；关闭排空移除 touch 后，五次 Vulkan 与 D3D11 / D3D12 产品 smoke 全部完成 60 帧并正常退出 | `shutdown-before-vulkan.log`、`windows-editor-release-03.log`、`editor-shutdown-fixed-results.json`；完整回归仍继续验收 |

ImGui 原生产物身份：`53731214a19a354e1d02ec895e45f0437b40c0bd2fbe44fa2be358c858af982e`。必需导出检查实际加载 DLL，随后释放模块；它不代替 GPU、字体、窗口或完整 Editor 运行验收。原生编译存在 SDK/第三方来源的普通告警，不归为“零告警”。

## 本轮修正

- SDK 的类型化 native source manifest 在 `Inno.Build.Bindings` 中转换为中立编译描述。Toolchains 和平台不依赖 BGCS SDK 类型。
- 用户、项目与生成输入映射到冻结复制树；保留重叠根与 include 顺序，使用前验证长度和 SHA-256。外部 SDK 继续使用明确选择的工具链。
- 编译文件来自清单，保留真实相对路径；SDK 会话报告不进入编译输出或 CheckOnly 比较。
- 解析器宏事实与生成单元所需宏分开。生成宏只作用于清单翻译单元，不改写上游 target 的编译定义。ImGui 的实际重定义失败及修正后成功均保留日志。
- ImGui/SDL/ImGuizmo 调用方使用实际 Raw ABI 和声明完整语义后生成的便利方法，不恢复已移除的指针包装，不手改生成源码。
- 回调不向 native 栈传播托管异常；字体和剪贴板按真实 native 所有权释放。字体动态加载路径不保留栈上 glyph range 地址。
- 文本调用复用现有字面文本控件，避免把用户内容交给 native 格式化入口；Gizmo 测量及两次绘制复用一次 UTF-8 编码。
- 禁止 global/implicit using 的规则保持不变；新增 using 和别名均限定在具体文件。
- SDK worker 由其公开 `WorkerAssetProvisioner` 从实际部署程序集目录准备，再通过 `WithWorkerAssetRoot` 提供给 Host。正式 MSBuild Task 已复验，不依赖 MSBuild 的 application base。
- 冻结外部头文件的真实 CMake 回归暴露了 267 字符的 Windows 路径问题；目录键使用保留完整 SHA-256 的 Base64URL 编码缩短，实际隐藏原始头文件后仍成功编译、加载并返回预期值。
- Fixture 源码与锁、缓存、输出分目录，避免把自身构建输出纳入 SDK 的头文件搜索目录成员快照。保留实际输入变化的严格失效校验。ABI 变化 fixture 使用 `long long`，避免错误假定 Windows C `long` 为 64 位。
- `verify-native` 复用统一普通/进程隔离测试入口，报告记录真实结果目录；CI 不默认排除 BGCS，补充 SDK、编译器、构建日志和 binlog。
- Browser 五组件使用 SDK 正式的 `emscripten-wasm32-emscripten` 目标，平台 provider 明确传递所选 workload 的 clang/clang++。配置读取与工具链回归通过，实际 Browser 生成、链接和运行继续验收。
- Browser 的静态 Player 计划明确注册到本平台的 `nativeProducts`，供共同绑定与支持准备入口解析，不在 CLI 或 Task 中增加平台组件名单。
- 原生 Wiki 的 API 表从实际语义清单重建，示例使用 Raw ABI 及每次调用的错误对象释放协议；未手改生成绑定。
- 首次 Editor 启动发现平台组合库的私有运行依赖与自身 `.deps.json` 没有传到入口，导致 Panel 发现为空。CLI 与两平台组合库现在显式启用共同的 `InnoExposeRuntimeClosure` SDK 规则，传递实际运行依赖并保持 `PrivateAssets="compile"`；未增加扩展类型名单。修正后的产品运行仍继续验收。

## 本轮主要文件职责

```text
build/
  bindings/Inno.Build.Bindings/
    Configuration/BindingConfigurationPaths.cs       SDK 的明确工具链输入
    Identity/NativeBindingGenerationIdentity.cs      输入与实现身份
    Generation/
      BindingHostFactory.cs                         SDK worker 与 Host 组合
      BindingHostExecution.cs                       正式生成与已验证产物消费
      BindingNativeCompilation.cs                   SDK 清单到中立契约的转换
  toolchains/Inno.Build.Toolchains/Native/
    NativeBindingCompilation.cs                     严格中立编译描述
    NativeBindingSourceRoot.cs                      根身份与冻结策略
    NativeBindingSourceFile.cs                      文件、哈希及翻译单元
    NativeBindingIncludeDirectory.cs                有序 include 搜索语义
    NativeBindingInputPaths.cs                      冻结复制树映射
    NativeBindingCMake.cs                           CMake 要求与宏作用域
  tasks/Inno.Build.Tasks/GenerateBindingsTask.cs     参数、日志与取消适配
  cli/Inno.Build.Cli/NativeBindingsVerification.cs   验收流程与统一测试入口
backends/<component>/native/Inno.Native.<name>/
  Native/                                         手写窄 facade
  Bindings/                                       正式 SDK 配置
  Native/Generated/                               正式原生生成物
  Generated/Bindings.cs                           正式单文件 managed 生成物
platforms/<platform>/                             SDK、平台与产品组合
tests/build/
  Inno.Build.Toolchains.Tests/                     无 BGCS 的中立清单回归
  Inno.Build.Tests/NativeBindingCMakeTests.cs        真实冻结头文件及宏隔离
  Inno.Build.Tests/NativeBinding*Tests.cs           生成、Task、选择与事务回归
tests/RepositoryInput.targets                     测试 checkout 的显式 SDK 输入
tests/source/Inno.Testing.Source/                  单一源码 owner 的测试输入读取器
docs/native/BindingGeneration.md                   当前生成协议
```

公共领域 API、Stable ID 与序列化形状保持原有契约。新增 public 编译描述只用于可替换生成器与中立构建器之间的边界，不进入脚本导出或 Player 闭包。两个直接依赖原生类型的 Widget 工具签名同步使用 SDK 的实际指针：`GetGlyphVisualBounds(ImFont*, ...)` 与 `AddGlyphCentered(ImDrawList*, ImFont*, ...)`；源码调用方和 Wiki 已同步，不保留被 SDK 移除的 Ptr 包装或兼容 overload。这是技术接口变动，不能笼统宣称所有 public 签名完全未变。

## 仍需完成

1. 完整 Debug 源码构建、全部适用普通和隔离测试。SourceRelease 和真实 CMake 冻结头文件/宏作用域回归已通过，后续源码变化仍需对应复验。
2. 最终代码下的七组件 CheckOnly、目标生成、正式 Task、取消/失败/损坏/并发和产物闭包检查。
3. 其余原生组件、插件、Samples、Windows 产品与发布模式验收；Browser 解释执行/AOT；可用设备上的实际 GPU/GUI/音频检查。
4. API/XML、项目图、using 策略、Wiki 覆盖、签名、链接和示例，以及最终 CI 与四仓库文档同步。
5. 记录当前代码的构建与运行测量，区分结构简化和已测得的性能结果。未测量的改动不声称性能收益。

macOS ARM64 和无法访问的实际设备只记录未执行，不从 Windows 结果推断通过。SDK 源码缺陷、引擎集成缺陷、验收操作错误与环境限制分别记录。任何未通过或未执行项都不能计入完成结果。

## 验收操作记录

一次探针构建与该探针执行重叠，Windows 文件锁导致复制失败；等待进程退出后串行重建通过。该事件属于本轮验收操作顺序错误。首次 Editor Release 产物复制后，C 盘曾短暂降至 49.82 GiB；通过正式缓存 owner 回收后恢复至 52.22 GiB。后续验收在开始前检查空间，并通过 SDK ArtifactsPath 使用独立 D 盘输出目录，目录位置记录于 `drive-output-root.txt`。清理只通过所属工程的 `dotnet clean` 和已登记缓存协议执行，保留源码、验收日志、全局 SDK/NuGet 缓存及 BGCS 工作区。

IDE 生成的 Solution 仅声明 Debug。首次验收命令指定 Release，被 SDK 明确拒绝；按实际声明改用 Debug 后三个 Project 均通过，未为此修改生成工程或增加配置。该失败属于验收命令选择错误。

一次新增回归的独立 ArtifactsPath 编译与 Editor 正式构建重叠，`GetTargetPath` 引发的 binding 物化仍需取得组件级 `generation.lock`，因等待超时被拒绝。已恢复串行执行，未删除锁、修改 timeout 或弱化生产校验；该尝试不计为成功。独立 SDK 输出不等于所有 binding owner 可以并行物化。

首轮完整 D 盘测试发现部分测试从执行 DLL 向上查找源码 checkout。这与 SDK 独立输出不兼容，并使一个只断言 InvalidOperationException 的用例可能在错误阶段通过。需要源码的四个测试项目改为显式导入测试层共享 SDK 输入与唯一读取器；移除各处猜路径函数，不修改生产 FindRepoRoot 契约、进程目录或断言条件。首次失败日志保留，修正后须重新编译及复验。

首次共享 C# 链接方案被有效项目图门禁以重复源码 owner 拒绝，见 `architecture-wiki-final-03.log`。已改为独立的 `Inno.Testing.Source` 测试支持程序集，四个消费者只保留项目引用；未增加校验例外。其唯一公开入口 `TestRepository.GetRoot(Assembly)` 不进入生产或脚本闭包；4 项公开边界回归通过，中立与 Windows 编译均 0 警告、0 错误。

第二轮执行中的 Solution owner fixture 使用 `Fixture.sln`，与当前规则只对所属 `InnoEngine.sln` 注入 owner 的契约不一致。已修正 fixture 文件名，并捕获子进程 stdout/stderr；生产规则和原有 owner 断言保持不变。相同两项目的独立 SDK 探针已通过同次共享、下次更新的检查，见 `solution-owner-repro-result.json`；修正后的测试仍须编译复验。

第二轮 Build.Tests 已完成：325 通过、2 失败、2 跳过。除上述 owner fixture 外，Wiki 测试未显式传递配置，误将 Release 套件的验证目标设置为默认 Debug。Wiki 与仓库架构测试现从自身公开 AssemblyConfiguration 元数据取得配置并传入验证入口；这些修正仍须重新编译及复验。本次独立最终 Wiki 校验显式使用 Release，不用缺失 Debug 输出解释当前 Release 产品是否通过。

Canvas 的原生 Metal 编译用例在非 macOS 系统原本直接返回。现在由测试发现阶段明确报告跳过，适用的托管用例继续执行；该修正等待 Canvas 套件复验。Canvas 与 Rendering2D 的插件导出示例同步补齐 `--tools-target`，不依赖没有为项目命令提供的默认值。

独立 Task 输出下直接调用目标模式 CheckBindings，七组件均报告所计算指纹的目标输出缺失，未得到内容差异结论，见 `check-bindings-final-01.json`。仓库源码比对应明确选择 HostSource；`verify-native` 第一阶段已据此修正，目标闭包仍由正式 Native 构建准备，不在只读检查中发布缺失缓存。该命令修正和七组件正向检查仍须复验。

单独重编测试时使用 BuildProjectReferences=false，使 Build.Tests 输出缺少 Inno.Native.UI.dll，Wiki 用例在生成逻辑引用前失败，见 `build-entry-fixes-final-01.trx`。该尝试不计为通过，已恢复完整 SourceRelease 项目图重建。该次 owner 用例通过，架构入口与四项测试输入边界也通过；Wiki 待完整依赖部署后重新执行。

完整项目图重新构建通过，0 警告、0 错误，见 `managed-windows-release-build-05.log`。完整依赖下重新执行 owner 与 Wiki 两项均通过；架构入口及四项测试输入边界也再次通过，见上表。汇总按原失败的完整用例名称逐项替换结果，不重复计入四项边界用例，也不改写原始 TRX。

七组件正向检查中，UI HostSource 的 compilation.json 仍记录头文件修改前的长度与哈希，目标产物已使用当前头文件。通过正式 GenerateBindings 和 CheckBindings 同次执行后复验通过；实际只改变 Native/Generated/compilation.json 的输入文件记录，managed binding 和所有 C++ 生成源码内容哈希均不变。前后清单、文件哈希与对照保留于 `ui-host-*-regeneration.json`、`ui-host-manifest-difference.json`，没有手改生成文件。

独立 SDK 输出下，项目 CLI 的 PrepareNativeAsync 仍按执行 DLL 的上级目录定位 checkout，实际 scripts 命令在 D 盘复现失败，见 `external-cli-source-before.log`。项目命令新增与其他 CLI 命令一致的可选 `--engine-root`；只在 CLI 内统一解析和校验，再把固定源码根传入 Native 准备。Canvas 工程从既有编译器项目位置取得该输入并传入，不要求使用者重复配置。公开领域 API、Toolchains.FindRepoRoot 契约及平台选择均未修改。新增源码预检与原开发入口回归在 Release 下 11/11 通过，相关 Debug/Release 构建及实际 Canvas 流程仍继续验收。

受控缓存清理在生成活跃时安全跳过。生成结束后正式 cleaner 退休约 19.9 GB 逻辑文件，磁盘实际仅增加约 2.0 GiB，见 `cache-clean-owned-05-space.json`；不以逻辑大小声称释放同等物理空间。SDK 输出和本轮临时目录使用 D 盘，保留当前产物、日志、全局包及 SDK 缓存。

Support Pack 首次实际准备完成原生依赖编译后，C 盘接近 51 GiB。通过所属控制台发送 Ctrl+C 后确认进程退出，未发现发布的 Pack；该次不计为通过，见 `windows-support-pack-final-01-cancellation.json`。经 SDK Clean 清理已验证范围内的旧 C 盘中立 Build.Tests Release 输出，保留 D 盘当前产物；再通过正式 cleaner 回收无引用缓存，实际空间恢复到约 54.25 GiB。未清理全局 SDK、NuGet 或 BGCS 工作区，随后重新发起正式准备。

第二次 Support Pack 准备及两种游戏发布均成功。CoreCLR smoke 的初始汇总脚本误用 Editor 的结束标记，三个游戏进程实际均退出 0；按 Player 的 `INNO-SMOKE frames=...` 协议重读相同日志后，三项帧数与 Draw 检查均通过，见 `flappybird-coreclr-smoke-final-02.json`。原始初次汇总保留，不把验收脚本标记错误描述为产品故障。

完整 Editor Debug 产品的原生准备接近 50 GiB 磁盘保留线，已通过所属控制台取消并确认相关编译进程退出；该产品未计为通过，见 `windows-editor-debug-final-01-cancellation.json`。本轮已通过的 SourceDebug 和相关入口 Debug 构建不代表该完整产品已验收。继续执行的条件是提供足够容量并保留 50 GiB；不删除全局缓存或安全保留目录，不添加跳过 Native 准备的生产路径。

最终 Wiki 检查的 191 个示例仍全部编译通过，但架构页出现两个外部示例名称的文档边界违规。已改为中立表述，保留实际项目名称于本验收记录；不修改校验规则。修正后重新执行完整门禁。

最终门禁已通过：233/233 项目、191 个示例、0 示例错误及未执行，见 `architecture-wiki-final-06.log`。Editor Debug 第二次尝试仍因空间取消；本次实际观测余量最低约 45.93 GiB，未守住 50 GiB 保留线，已明确记录而非宣称始终满足。系统分页文件当时约 21 GiB，缺少起始数据不能据此独立归因。后续观测空间回升到约 58.5 GiB，其中 SDK Clean 释放约 1.1 GiB；没有修改系统分页设置或终止用户进程。六个旧工程 net9.0 输出删除也被自动审批以 blocked by policy 拒绝，全部保留，未绕过。完整结论见[阶段验收报告](NET10_BGCS301_FINAL_ACCEPTANCE_2026_10_20.md)。

空的 `tests/Infrastructure` 目录清理被自动审批以 `blocked by policy` 拒绝，未给进一步理由，已停止该清理。该目录不含源码或跟踪文件，未通过其他方式绕过拒绝。

实施尚未完成，本记录不是最终完成声明。当前 Windows 环境无法访问 `/System/Library/Sounds/Glass.aiff`，未播放该提示音。
