# .NET 10 / C# 14 与 BGCS 接入阶段记录

日期：2026-10-20（Asia/Taipei）。环境：Windows x64，AMD Ryzen 9 6900HX，8 核 / 16 线程。

## 当前结论

四仓库继续使用 SDK 10.0.402 / C# 14。当前 BGCS 依赖仍精确固定为 3.0.0；3.0.1 的不可变发布和公共包消费尚未得到确认，因此没有切换到本地 SDK 源码或把 3.0.0 当作已修复包。

本阶段继续完成独立调用方、运行时并发、命令录制和测试工作。最终统一代码状态的完整托管选择为 1,135 通过、1 失败、0 跳过；38 个测试程序集的隔离发现成功，选中的隔离用例通过。剩余失败是仓库架构检查，诊断已收敛到 ImGuiWidget 公开签名中的 3 个未解析类型引用。当前不能宣布完整接入或最终验收通过。

本阶段没有修改 BindGen-CS 仓库，没有手工修改生成 Bindings 或 extern，没有提交或推送。Canvas、Rendering2D、Samples 的既有改动保留。

## 1. 已完成的独立改动

### BGFX 和 MiniAudio 调用方

- BGFX 按实际生成 API 使用 Raw 调用、真实指针、字节布尔值和枚举名称，移除对不存在的便利方法的调用。
- 名称使用明确的 UTF-8 调用缓冲区；短名称使用有界 stackalloc，长名称使用独立数组，原生调用期间保持地址有效。
- MiniAudio 使用真实结构指针，文件路径在调用期间保持 UTF-8 缓冲区有效；核对原生实现的异步路径复制行为。
- 原生初始化失败和 SDK 调用异常释放尚未转移的分配。
- BGFX、MiniAudio、SDL、Text、UI 的 Debug/Release 共 10 次最终托管构建均为 0 warning / 0 error；原生编译、链接和运行仍单独验收。

### 程序集分类并发

首轮托管测试发现 `PlayerContentSourceTests` 的内容校验被分类弱表的重复 key 异常打断。

根因是 `RegisterInnoAssemblyClassification` 使用 Remove/Add 两步更新，同一个静态程序集被独立 Host 并发发布时存在空窗。改为 ConditionalWeakTable.AddOrUpdate，继续只保存 domain/scope 值，不增加程序集强引用或另一套注册表。

新增测试只经过公开 ModuleHost、StaticModuleSource 和分类查询契约。修复前复现了相同的 ConditionalWeakTable.Add 重复 key；修复后 Runtime Release 62 项、Modules/退休 Release 32 项，以及 Runtime Debug 62 项通过。没有修改 Full GC / finalizers / Full GC、unload monitor、Faulted 或超时策略。

### 并行命令录制

- 移除逐命令的捕获委托，使用本次 Execute 独占的命令缓冲。
- handle 和 sampler 按值保存；uniform 和 transform 在调用时复制；引用参数保持原有对象语义。
- 字节缓冲与引用缓冲通过 BCL ArrayPool 借用，引用数组归还时清空。
- 正常结束、录制失败、BeginPass、回放、EndPass 和 EndGraph 失败均释放记录。
- 编译时冻结并行 pass 集合；没有并行 pass 时直接执行串行流程。
- 修复回放与 EndPass 同时失败时原始异常被覆盖的问题，按已有 AggregateException 契约保留全部失败；EndGraph 失败继续加入结果。
- 不改变公开 Rendering 签名、资源 generation、设备线程或 GPU 退休协议。
- Rendering Debug/Release 均为最新 61 项通过，包含单独退出失败及组合失败。

### 构建、归属与文档

- 架构检查使用当前 bindings.props 的组件、host config 和 target 声明，不再检查 SDK 已不使用的 outputPath 字段。
- 校验声明归属、唯一 import、路径、条件、未知属性及重复 target；相关架构命令回归 31 项通过。
- 修复两个 process 测试项目的 Solution 文件夹大小写，以及删除扩展工程后残留的 ImGui 项目文件夹层级。
- 删除无生产调用方的 LoadNativeDll / import resolver 注册通路。当前 Native 层解析部署路径，实际加载、函数表及释放属于 BGCS Runtime 或明确消费者。
- 同步 Native 路径调用方、测试与 Wiki；默认生成器构造函数提供完整英文 XML，不再要求一个没有实际用途的 dotnetHost 参数。
- 明确 31 个公开命令的参数语义和 Span 借用范围，录制实现继承公开文档并说明复制语义。
- 删除当前索引中的失效 BindingExtension 引用，更新当前问题和验收页面。

## 2. 本阶段文件结构

```text
InnoEngine/
├─ backends/
│  ├─ Bgfx/
│  │  ├─ runtime/Inno.Adapter.Rendering.Bgfx/   # Raw API、指针、枚举与字符串调用
│  │  └─ tests/Inno.Native.Bgfx.Tests/         # 当前函数表初始化用例
│  ├─ MiniAudio/
│  │  ├─ runtime/Inno.Adapter.Audio.MiniAudio/
│  │  │  └─ MiniAudioDevice.cs
│  │  └─ tests/Inno.Native.MiniAudio.Tests/
│  ├─ Interop/native/Inno.Native.LibraryLoading/
│  │  └─ NativeDllLoader.cs                    # 仅解析明确部署的库与文件
│  └─ Sdl3/tests/Inno.Native.Sdl3.Tests/
├─ build/bindings/Inno.Build.Bindings/
│  └─ NativeBindingGenerator.cs                # 按 operation 取得工具与输入
├─ src/
│  ├─ foundation/extensibility/Inno.Extensibility.Modules/Metadata/
│  │  └─ AssemblyExtensions.cs                # 原子弱表更新
│  └─ services/rendering/Inno.Rendering/
│     ├─ Device/
│     │  ├─ RenderCommands.cs                  # 公开命令与借用契约
│     │  ├─ RecordedRenderCommandEncoder.cs    # 记录命令
│     │  ├─ RecordedRenderCommandEncoder.Replay.cs
│     │  └─ RecordedRenderCommandBuffer.cs     # 缓冲 owner 与只读 reader
│     └─ Graph/CompiledRenderGraph.cs          # 调度、回放及退出清理
├─ tools/Inno.Tooling.Architecture/
│  ├─ ArchitectureRules.cs
│  └─ NativeBindingOwnershipValidator.cs
├─ tests/
│  ├─ runtime/Inno.Runtime.Tests/AssemblyClassificationTests.cs
│  ├─ rendering/Inno.Rendering.Tests/ParallelCommandRecordingTests.cs
│  └─ tooling/Inno.Tooling.Architecture.Tests/ArchitectureCommandTests.cs
└─ docs/                                      # 对应项目页与当前验收
```

新增类型只承担本领域的一项职责；没有新增领域公开 API、全局服务、平台选择表、测试后门或跨层版本服务。Native 路径开发入口已同步清理，不能将其视为保持了全部原生工具 API 不变。

## 3. 性能采样

同一 SDK、Release 配置和公开 Noop backend 输入，改良前后各运行 3 个独立进程。每个场景预热 48 帧，再采样 32 帧，并核对实际 Draw 回放数量。

表内为 3 次采样中位数，KB/MB 使用十进制。耗时来自 Stopwatch，是该工作流的经过时间，不是 GPU 时间或独立进程 CPU profiler 数据。

| 场景 | 改良前分配 / 帧 | 改良后分配 / 帧 | 改良前耗时 / 帧 | 改良后耗时 / 帧 |
| --- | ---: | ---: | ---: | ---: |
| 并行 1,000 Draw | 107,777.25 B | 3,139.75 B | 0.072646875 ms | 0.033046875 ms |
| 并行 10,000 Draw | 1,145,632.75 B | 3,139.75 B | 1.361090625 ms | 0.24379375 ms |

收益归因于命令表示、缓冲所有权、取消空录制调度及 JIT 热路径的具体实现改良；没有将其归因于 .NET 10 运行时升级本身。低命令量的微秒级计时存在调度波动，不能据此推断真实产品帧率。

初版曾出现 1,000 Draw 耗时回退，已经继续修改并重新采样，未将该初版作为验收结果。表中使用包含退出错误聚合与 XML 整理的最终性能复验结果。

## 4. 命令与证据

本机使用 `C:/Users/23842/.dotnet/dotnet.exe`；执行前在子 shell 中设置 DOTNET_ROOT、DOTNET_HOST_PATH 与 PATH，不修改父 IDE 的环境。

统一托管入口：

```powershell
./tests/Invoke-IsolatedTests.ps1 `
  -Configuration SourceRelease `
  -Dotnet 'C:/Users/23842/.dotnet/dotnet.exe' `
  -Solution 'artifacts/acceptance/dotnet10-resume-2026-10-20/Managed.slnf' `
  -Filter 'ExternalDependency!=BGCS' `
  -ResultsDirectory 'artifacts/acceptance/dotnet10-resume-2026-10-20/managed-final-state'
```

该选择保留普通与进程隔离执行，未按 Binding 名称排除测试。它不是完整产品或所有测试工程的验收。

原始证据根：`artifacts/acceptance/dotnet10-resume-2026-10-20/`。

- `*.status-before.txt`：四仓库恢复工作前的状态。
- `bgfx-build-02.log`、`miniaudio-build-01.log`：调用方编译。
- `native-*-build.log`、`native-managed-guards.log`：原生测试工程编译及 10 项托管检查。
- `classification-before.log`：有效复现中的重复 key 堆栈；目录另保留了编写 fixture 时的早期尝试，不将所有 TRX 合并为验收统计。
- `runtime-after`、`modules-after`、`runtime-debug`：分类修复与退休回归。
- `rendering-cleanup`、`rendering-debug-final`：命令、引用释放和组合失败的 Release/Debug 最终局部回归。
- `parallel-before-*.json`、`parallel-final-state-*.json`、`parallel-comparison-final-state.json`：同 SDK 最终采样及中位数。
- `*-debug-final.log`、`*-release-final.log`：五适配器两配置的 10 次最终托管构建。
- `native-guards-final`、`architecture-contracts-final`：10 项部署参数/flag 宽度与 31 项架构命令最终局部回归。
- `managed-final-state/totals.json`、同目录 `execution-summary.json` 与 TRX：最终完整托管选择的通过、失败与隔离执行结果。
- `architecture-final.log`：剩余 3 条 ImGuiWidget 签名诊断。
- `binding-host-final.log`：生成库最近一次构建通过。
- `build-entry-01.log`、`imgui-sdl-build-01.log`：尚未完成的 ImGui 调用方编译诊断。

## 5. 后续顺序与验收条件

1. 取得 BGCS 3.0.1 不可变发布、公共 NuGet 下载与真实消费的明确结果，再更新依赖和作用域锁文件。
2. 使用发布包检查模板类型与实际生成 API，完成 ImGui/ImGuizmo、Editor 及剩余调用方。不恢复指针包装或旧方法别名。
3. 在 Inno.Build.Bindings 内消费 SDK 的类型化原生编译清单，转换为中立构建描述；Toolchains 不引用 BGCS 类型。
4. 将源码、头文件、include 根、静态 adapter、布局断言和导出要求接入现有冻结复制树及 CMake。不得猜固定生成文件名或重写生成文本。
5. 验证正式 MSBuild Task 的 worker 资产闭包，更新实际 BGCS fixture 和相关 CI；保留中立协议测试、失败/取消清理及生产校验。
6. 使用正式流程重新生成七组件、IDE 投影和必要插件产物，验证 Samples 安装态编译。
7. 在同一最终代码状态下通过全部适用托管构建、普通/隔离测试、架构、XML、Wiki 和原生 ABI/exports 检查。
8. 执行可用 Windows 产品与 Browser 发布/运行，逐项报告 macOS、真实 GPU、音频及其他设备环境的未执行项。

完整完成标准仍为全部适用检查通过。当前唯一托管套件失败包含的是引擎 ImGui 调用方问题，不能仅标作 SDK 缺陷，也不能记为通过。冻结 include、类型化清单和正式包消费属于明确的外部依赖边界。

阶段结束时共享磁盘约 53.7 GiB 空闲，未清理 BGCS 仓库产物。当前 Windows 环境无法访问 `/System/Library/Sounds/Glass.aiff`，提示音未播放。
