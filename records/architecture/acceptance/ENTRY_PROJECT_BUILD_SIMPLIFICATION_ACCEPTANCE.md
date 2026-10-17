# 普通工程构建与 entry 启动简化验收

日期：2026-10-17。基线 HEAD：2f3fbc487520bc9eee28a35b588fd0f952a8cd8d。保留实施前未提交修改；按用户要求合并本地工程整理后提交至 kernel，提交身份以 Git author/committer 记录为准。没有推送或操作 Rider。

## 交付状态

工程与入口代码整改完成；源码编译、项目选择、部署和静态架构检查通过。Windows Rider 实机启动由用户验证。完整首次 Native/BGCS 准备与普通 Native 热命中未验收，不把使用既有支持产物的编译检查算作这些项目通过。

## 改动

- 两个 entry 使用普通 ProjectReference。Editor 在主线程直接调用 SDK 宿主对应的公开产品 Run；Build 直接调用 CLI。
- 宿主属性明确映射 win-x64 / osx-arm64，未知宿主在构建阶段拒绝。当前平台产品通过 entry 引用构建，其余产品不进入此依赖闭包。
- 删除 Bootstrap 项目、预构建 Editor receipt 启动门禁、动态 CLI 加载、入口 worker、独占这些路径的 ManagedToolRuntime API，以及两个 .run 文件。
- 默认 Debug/Release 正常选择源码、工具及测试项目；保留 SourceDebug/SourceRelease 的源码批量验证用途。删除旧 x64/x86 Solution 配置，保留项目 GUID。
- 平台产品继续拥有 Native 准备；完整 native 文件与部署索引通过 SDK 内容项目传递到 entry 输出目录。
- Build 默认执行 support，新增 support --product editor|player；editor 明确构建完整 entry。产品 clean 覆盖选定 entry 与平台产品的输出，沿用产品读取/独占 lease。
- 清理失去消费者的旧入口测试，仅同步直接依赖删除 API 的测试引用；未运行单元测试套件。
- 更新入口、平台、CLI、缓存、架构及工具文档。修改文件统一整理缩进、换行与职责分块；未修改 extern、BGCS 版本或生成绑定。

## 自动验证

| 检查 | 结果 |
| --- | --- |
| XML / 项目路径 / 直接引用 / 环检查 | 263 个 XML 文件、229 个项目，无错误。 |
| 默认 Solution Restore | 225 个恢复项目成功，包含 entry 选择的 Windows Editor；未要求 Browser workload。 |
| Solution 覆盖 | 229 个项目全部纳入。Debug/Release 选择 224 个独立源码构建根；五个平台产品由 entry 或显式发布驱动。SourceDebug/SourceRelease 选择 223 个，额外排除 Editor entry。 |
| 宿主求值 | WindowsX64 和模拟 SDK 属性的 MacOSArm64 各选择唯一对应 Editor 引用。macOS 仅工程求值。 |
| 不支持宿主 | linux-x64 SDK 属性触发明确构建错误，退出 1。 |
| Solution 依赖屏蔽 | 用平台产品未勾选的配置求值 AssignProjectConfiguration；当前宿主产品 BuildReference=true、ReferenceOutputAssembly=true。 |
| Editor Debug 编译 | 复用已有 NativeBindingSelection，0 警告、0 错误，125.28 秒。 |
| Editor 重复编译 | 相同验证条件，0 警告、0 错误，128.74 秒；不作为普通 Native 热命中或性能改善证据。 |
| Build Debug 编译 | 相同既有支持条件，0 警告、0 错误，46.05 秒。 |
| 架构工具编译 | 0 警告、0 错误，4.39 秒。 |
| Build 最终入口编译 | 不重编依赖，0 警告、0 错误，8.62 秒。 |
| 仓库架构验证 | InnoEngine.Build.dll verify . 通过；没有执行 Wiki 全量语义/示例编译或完整 SDK 项目图评估。 |
| Build 帮助与错误参数 | --help 退出 0；support --clean 正确拒绝，退出 1。 |
| Native 部署 | 15 个文件全部传递，SHA-256 与平台产品输出一致；Deployment.index、产品 DLL、entry runtimeconfig 存在。 |
| 删除与格式 | 当前源码和 Wiki 无被删除启动 API 引用；git diff --check 通过；新 deps.json 不引用 Bootstrap。 |

## 实际命令

从仓库根目录执行，日志位于 artifacts/acceptance/entry-project-build/20261017/。

```powershell
dotnet restore InnoEngine.sln --disable-parallel -p:Configuration=Debug
dotnet restore entry/InnoEngine.Editor/InnoEngine.Editor.csproj --disable-parallel
dotnet restore entry/InnoEngine.Build/InnoEngine.Build.csproj --disable-parallel

# Only this acceptance command reuses the pre-existing native/binding selection.
$selection = (Resolve-Path 'platforms/WindowsX64/editor/Inno.Editor.WindowsX64/obj/windows-x64/Debug/net9.0/NativeBindingSelection.props').Path
dotnet build entry/InnoEngine.Editor/InnoEngine.Editor.csproj --no-restore --configuration Debug -m:1 -nodeReuse:false -p:InnoPrepareProductNative=false "-p:InnoNativeBindingSelection=$selection"
dotnet build entry/InnoEngine.Build/InnoEngine.Build.csproj --no-restore --configuration Debug -m:1 -nodeReuse:false -p:InnoPrepareProductNative=false "-p:InnoNativeBindingSelection=$selection"

dotnet build tools/Inno.Tooling.Architecture/Inno.Tooling.Architecture.csproj --no-restore --configuration Debug
dotnet entry/InnoEngine.Build/bin/Debug/net9.0/InnoEngine.Build.dll --help
dotnet entry/InnoEngine.Build/bin/Debug/net9.0/InnoEngine.Build.dll verify .
```

SDK：9.0.318，Windows X64。InnoPrepareProductNative=false 只用于上述明确限定的编译验收，没有写入生产配置；日常构建继续准备和校验匹配的支持产物。

## 中间结果与限制

1. 整理源码期间的首轮构建被既有 Task 输入稳定性校验拒绝，未发布变化中的工具闭包。原始日志保留。
2. 固定源码后尝试普通 Editor Build，发现需刷新 BGCS 绑定。按用户暂缓第三方与绑定专项验收的要求，停止所属构建进程树；没有升级包或修改生成绑定。改为显式复用已存在的完整选择进行 C# 编译和部署检查。
3. 尚未验证干净首次 Native 准备、普通 Native 热构建零编译、Release、macOS 实机、完整产品发布或 GUI。Rider 首次正常运行仍可能因输入变化而准备支持产物，不能承诺瞬间完成。
4. 独立引擎发行包不属于本轮。没有大规模删除缓存、用户数据或 SDK；保留已有验收证据。
5. 当前 Windows 不存在 /System/Library/Sounds/Glass.aiff，提示音未播放。

## 用户的 Rider 检查

1. 打开 InnoEngine.sln，使用 Debug 或 Release。
2. 选择 entry 下的 InnoEngine.Editor，正常 Run/Debug。
3. 首次等待对应支持准备；随后应进入项目目录选择或给定项目。
4. 修改一处普通 C# 源码，再次运行，确认执行新编译结果。
5. InnoEngine.Build 可单独运行；无参数准备宿主支持，editor 显式构建完整 Editor。

不需要 .run 文件或先执行 Build。个人 IDE 中历史保存的运行配置由用户自行选择整理。

## Solution 与项目文件收口

- 重新清点 229 个源码项目和 263 个手写 MSBuild XML 文件。229 个项目全部进入 Solution，包含绑定扩展和 fixture；141 个 Solution Folder 都有有效内容，无游离项目、空分组、悬空父分组、重复 GUID、失效路径或项目引用环。
- Solution 声明按 entry、src、backends、platforms、build、tools、tests 展开，再按所属职责展开子节点。项目配置与嵌套映射遵循同一顺序；保留现有项目 GUID、父分组与四种配置映射。
- 两个系统服务项目删除空 ItemGroup。项目和共同 props/targets 补齐职责块空行、两空格缩进、最终换行，多行属性统一逐行排列。纯排版前后比较解析后的 XML，属性值、Import/Property/Target 顺序及引用 metadata 均未改变。
- CLI 引用按共同构建、共享 backend、平台规则、创作与运行契约、验证工具分组，所有引用及可见性保持原值。
- 无直接入站 ProjectReference 的项目逐项核对真实 owner。入口由用户启动；Player 是发布输入；Panel/Feature 通过 EditorProduct.props 声明；ImGui 扩展由 bindings.props 声明；probe 与代际 fixture 由所属测试 MSBuild 目标调用。不因静态入站计数为零而误删。
- 三个隔离代际 fixture 有意输出同一 Inno.Extensibility.Modules.TestModule 程序集名，用于替换代际测试；不属于生产程序集重名。它们各自拥有独立项目与输出目录。
- 同步 CI：完整宿主产品构建明确调用 `editor --configuration Debug/Release`，避免默认 support 只准备 Native 后，smoke 步骤尝试运行尚未编译的 Editor entry。
- 删除当前 Wiki 和 AGENTS 中遗留的启动 receipt 说明，补齐 WindowsX64 / MacOSArm64 系统前提、递归 Clone、SDK 架构与 Rider 启动步骤。

原始检查位于同一 acceptance run 的 solution-cleanup-audit.json、project-formatting.json、cleanup-solution-configuration.log 与 cleanup-whitespace.log。此整理没有执行 GUI、BGCS 重新生成、第三方专项或远端 CI，不把静态检查算作这些验收通过。

| 整理后补充检查 | 结果 |
| --- | --- |
| Solution 配置验证 | ValidateSolutionConfiguration 退出 0。Debug/Release 各 224 个独立构建根，SourceDebug/SourceRelease 各 223 个；宿主产品仍通过 entry 引用构建。 |
| 宿主项目求值 | Windows 与模拟 osx-arm64 各选择唯一对应 Editor；macOS 仅配置求值。 |
| CLI 编译 | 复用已编译依赖，BuildProjectReferences=false；0 警告、0 错误，13.22 秒。 |
| Build entry 编译 | 复用已编译依赖，BuildProjectReferences=false；0 警告、0 错误，16.77 秒。 |
| CLI 帮助与仓库架构检查 | --help 退出 0，verify . 通过。未执行全量 Wiki 语义或测试套件。 |
| 改动 Markdown 链接 | 18 个已跟踪改动页的相对文件链接有效；未把此检查当作全部锚点/API/示例验收。 |
| 格式与禁止改动范围 | git diff --check 通过；没有修改 extern、Generated 绑定或 BGCS 版本入口。 |
