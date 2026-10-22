# Native binding generation

[Native 索引](README.md) · [构建入口](../build/Inno.Build.Cli.md) · [Wiki 首页](../README.md)

每个 Native 组件通过自己的 `Bindings/bindings.props` 声明唯一组件身份和公共配置。组件拥有命名、类型及生命周期语义；平台 Build integration 拥有 ABI 与托管 runtime Profile，平台 provider 冻结 SDK 输入。包族由 `build/msbuild/Inno.Packages.props` 精确固定为 3.0.1。

## 生成与检查

在仓库根目录执行：

```shell
dotnet build backends/Sdl3/native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj -t:GenerateBindings -c Release -p:InnoBindingTargetId=windows-x64
dotnet build backends/Sdl3/native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj -t:CheckBindings -c Release -p:InnoBindingTargetId=windows-x64
```

`GenerateBindings` 调用正式 SDK 流程；`CheckBindings` 在独立候选中重新生成并比较完整 managed/native 输出，不发布候选。普通宿主编译在所选 binding 缺失时准备生成物；目标产品使用本次 Native 构建选定的身份。

Bgfx、ImGui、ImGuizmo、MiniAudio、Sdl3、Text、UI 均使用组件自己的 唯一 `Bindings/bindgen.json`。Text 与 UI 的手写语义 facade 分别为 `Native/include/TextRuntime.hpp` 和 `Native/include/RmlUiRuntime.hpp`，实现属于同一 Native owner。SDK 从 facade 生成 C ABI 和 managed binding。

源码、配置和第三方依赖分别归组件与 extern 所有。所有生成物通过正式流程获得，不手改 `Generated/Bindings.cs`、生成原生代码或第三方源码，不另写平行 import。

## 输出与读取保护

HostSource 输出为组件的 `Generated/Bindings.cs` 和 `Native/Generated`。TargetArtifacts 输出为 `artifacts/bindings/<component>/<target>/<fingerprint>/Generated` 和 `Native`。

Native 目录只复制 SDK 清单声明的原生源码、头文件和 generated root 输入，保持它们的相对布局，并包含引擎中立的 `compilation.json`。SDK 会话报告用于诊断，不进入原生编译输出或 CheckBindings 比较。managed 与 native 候选一起验证、发布；失败和取消不返回部分闭包。配置、实际头文件、工具、包内容及生成实现参与身份。

调用方持有 `NativeBindingGenerationResult`，直到编译、链接或复制结束。目标结果持有 artifact 读取 lease；宿主可变输出持有 generation owner。修复和回收必须等待 reader 释放，不能通过保留路径字符串延长产物生命周期。

MSBuild 在读取保护期间将唯一选中 `Bindings.cs` 复制到本次 SDK 中间目录，实际 Compile item 消费该副本。相同内容保持时间戳，构建期间仍保留完整性与身份校验。

## 原生编译要求

`Inno.Build.Bindings` 是 SDK 消费边界。它读取经过 SDK 发布锁和哈希验证的 `NativeSourceManifest`，结合本次会话的物理 root bindings，转换为 `NativeBindingCompilation`。中立 Toolchains 不引用 BGCS 类型；平台 Build integration 仅使用官方公开配置创建 Profile，不解析或复制 SDK 实现。平台基础 build 不引用 BGCS。

中立描述保存根身份、文件长度与 SHA-256、翻译单元标记、有序 include、宏、语言要求及导出要求。逻辑根按每个组件独立解释。SDK 的生成源码和 ShimSource 输入进入编译列表；入口头和依赖不作为翻译单元。

冷 CMake producer 将用户源码与所选生成物复制到同一冻结树，保持重叠根和相对布局；外部 SDK 保持所选工具链的位置。使用前核对映射输入的长度和 SHA-256。生成 include 不绑定原工作区绝对路径。

共同 executor 从中立描述生成本次 producer 专属的 CMake 输入，组件不猜测固定生成文件名。独立组件在目标创建后应用要求；Browser 聚合按组件应用同一机制。编译器 builtin/SDK 搜索顺序由所属工具链负责，不能把解析器目录机械重复加入 -isystem。

清单宏只应用到清单中的翻译单元，上游源码继续使用组件自己的 CMake 配置。解析器的目标宏事实单独记录，不能覆盖实际消费编译器的 builtin。读取当前中立描述时，缺少根语义或出现未知字段均明确失败。

初始、批次锁后、生成后、Native 锁后及编译后继续执行各自 fresh 校验。冻结输入使用复制，不能以指向可变源码的链接代替，也不能后处理生成文本来修补路径。

## 工具与运行边界

Compiler、sysroot、系统 include、Emscripten、Node 与 Python 由相应平台 provider 明确选择，并从 operation 冻结的 Native SDK 输入传给 SDK 的类型化字段；不修改父进程，也不实现独立的配置变量替换语法。BGCS 生成源码；编译、链接、导出验证、打包和部署由引擎完成。

生成工具通过实际宿主 NuGet Restore 图取得 worker 资产。Player 只引用 BGCS.Runtime，不携带生成器。绑定组件统一禁用默认 runtime marshalling，原生 ABI 宽度、参数、返回值和 callback 都需要实际验收。

Native CMake 中间产物归所属 toolchain 的 obj/native，运行时只读取明确部署的文件。完整验证使用 [Build CLI](../build/Inno.Build.Cli.md)，结果见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)；单个目标通过不代表其他平台已通过。
