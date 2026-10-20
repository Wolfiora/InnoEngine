# macOS ARM64 后续验收入口

状态：**未执行**。2026-10-21 用户确认目前无法使用 Mac。Windows 的构建、Noop 或隐藏窗口结果均不替代本页检查。

产品运行统一使用 `InnoEngine.Samples/FlappyBird`；独立插件编译及组件契约测试不代替该示例的运行结果。

[阶段报告](NET10_BGCS301_FINAL_ACCEPTANCE_2026_10_20.md) · [Browser SDK 阻塞](BGCS301_BROWSER_AGGREGATE_ABI.md) · [当前问题](../../../docs/architecture/CURRENT_ISSUES.md)

## 输入与顺序

使用原生 Apple Silicon Mac、ARM64 .NET SDK、Xcode 工具、CMake、make 和 PowerShell 7。实际工具解析遵循 [Mac 平台配置](../../../docs/platform/PRODUCT_STARTUP.md#from-zero)，不使用 Rosetta 或 Windows 原生产物。

四仓库必须包含本轮的全部源码及未跟踪的新文件。当前 Windows 工作区未提交，直接拉取现有远端分支不保证得到相同代码。传递源码时保留 `.imeta`、当前工程、锁文件、插件包与子模块身份；不复制 `bin/obj`、`Library` 或平台缓存。BGCS 从精确固定的 NuGet 3.0.1 还原，不复制 BindGen-CS 源码或引用其工程。

先记录环境和构建 CLI，再运行实际目标绑定。若 SDK 拒绝 ARM64 聚合 ABI，保存原始诊断并停止依赖该绑定的产品步骤；独立托管检查仍可执行。不改目标身份、不关闭校验、不修改生成源码。正式 3.0.1 二进制的公开能力查询也显示 ARM64 CoreCLR aggregate profile 为空，但该机器的实际生成结果仍须以日志确定。

以下代码块在仓库根目录的 `pwsh` 中按顺序执行。没有运行的步骤保持“未执行”；依赖失败的步骤标明“阻塞”，不能写为通过。

## 1. 环境与明确输出 owner

```powershell
$ErrorActionPreference = 'Stop'
$engine = (Resolve-Path .).Path
$workspace = Split-Path $engine -Parent
$canvas = Join-Path $workspace 'InnoEngine.Canvas'
$rendering = Join-Path $workspace 'InnoEngine.Rendering2D'
$sample = Join-Path $workspace 'InnoEngine.Samples/FlappyBird'
$sdk = (Get-Command dotnet -CommandType Application).Source
$architecture = [Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture.ToString()
if (-not $IsMacOS -or $architecture -ne 'Arm64') { throw 'Requires a native macOS ARM64 process.' }
$evidence = Join-Path $engine ('artifacts/acceptance/macos-bgcs301-' + (Get-Date -Format 'yyyyMMdd-HHmmss'))
$outputs = Join-Path $evidence 'outputs'
New-Item -ItemType Directory -Path $evidence | Out-Null
@{ sourceOwner = $engine; purpose = 'macOS ARM64 product acceptance'; sdk = $sdk } |
    ConvertTo-Json | Set-Content (Join-Path $evidence 'operation.json')
& $sdk --info | Set-Content (Join-Path $evidence 'dotnet-info.txt')
if ($LASTEXITCODE -ne 0) { throw 'SDK inspection failed.' }
& $sdk --version
if ($LASTEXITCODE -ne 0) { throw 'The committed SDK selection could not be resolved.' }
& xcodebuild -version 2>&1 | Set-Content (Join-Path $evidence 'xcode.txt')
& xcrun --sdk macosx --show-sdk-path
if ($LASTEXITCODE -ne 0) { throw 'The macOS SDK is unavailable.' }
& cmake --version
if ($LASTEXITCODE -ne 0) { throw 'CMake is unavailable.' }
foreach ($repository in @($engine, $canvas, $rendering, (Split-Path $sample -Parent))) {
    $name = Split-Path $repository -Leaf
    & git -C $repository rev-parse HEAD | Set-Content (Join-Path $evidence "$name-head.txt")
    if ($LASTEXITCODE -ne 0) { throw "Cannot identify $repository." }
    & git -C $repository status --short | Set-Content (Join-Path $evidence "$name-status.txt")
}
& git -C $engine submodule status --recursive | Set-Content (Join-Path $evidence 'submodules.txt')
Get-PSDrive -PSProvider FileSystem | Format-Table | Out-String |
    Set-Content (Join-Path $evidence 'disk-before.txt')
```

SDK 实际选择可以是 10.0.402 或声明允许的同 feature band 稳定补丁；报告必须记录实际版本。SDK、运行进程和目标均需为 ARM64。全过程保持至少 50 GiB 可用空间，容量不足时先停止，不能删除未知 owner 或全局缓存。只有 Command Line Tools 时，`xcodebuild -version` 的失败单独记录；`xcrun` 实际 SDK 和后续工具解析才决定 Native 能否运行。

## 2. CLI 与实际 BGCS 目标预检

```powershell
$entry = Join-Path $engine 'entry/InnoEngine.Build/InnoEngine.Build.csproj'
$cliOutputs = Join-Path $outputs 'cli'
& $sdk build $entry -c Release --disable-build-servers -m:1 -nr:false `
    -p:UseSharedCompilation=false "-p:ArtifactsPath=$cliOutputs" 2>&1 |
    Tee-Object (Join-Path $evidence 'cli-build.log')
if ($LASTEXITCODE -ne 0) { throw 'CLI compilation failed.' }
$cli = @(& $sdk msbuild $entry -nologo -p:Configuration=Release -p:DesignTimeBuild=true `
    "-p:ArtifactsPath=$cliOutputs" -getProperty:TargetPath)
if ($LASTEXITCODE -ne 0 -or $cli.Count -ne 1 -or -not (Test-Path $cli[0])) { throw 'CLI TargetPath is unavailable.' }
$cli = $cli[0].Trim()
foreach ($product in @('player', 'editor')) {
    & $sdk $cli bindings --engine-root $engine --target macos-arm64 --tools-target macos-arm64 `
        --product $product --configuration Release --dotnet $sdk 2>&1 |
        Tee-Object (Join-Path $evidence "bindings-$product.log")
    if ($LASTEXITCODE -ne 0) { throw "BGCS $product generation failed; stop dependent product checks and retain this log." }
}
```

Player 与 Editor 闭包分别由正式发行声明选择，不在验收脚本中复制组件名单。失败时同时保留缺失 SDK、ABI 或 source manifest 的具体错误。不要把后续未运行的链接/产品记成独立失败。

## 3. 源码、测试与 Wiki

BGCS/Native 受阻时，可以单独执行现有独立托管入口：

```powershell
& ./tests/Invoke-ManagedChecks.ps1 -Dotnet $sdk -Configuration SourceDebug `
    -ResultsDirectory (Join-Path $evidence 'managed-independent')
```

绑定预检成功后，构建实际 Mac 产品测试闭包并执行统一普通/隔离入口。不得跳过 Metal surface retirement、放宽超时或移除卸载断言：

```powershell
$productProperties = Join-Path $engine 'platforms/MacOSArm64/build/Inno.Build.MacOSArm64/EditorProduct.props'
$testOutputs = Join-Path $outputs 'tests'
& $sdk build InnoEngine.sln -c SourceDebug --disable-build-servers -m:1 -nr:false `
    -p:UseSharedCompilation=false "-p:ArtifactsPath=$testOutputs" `
    -p:InnoNativeTarget=macos-arm64 -p:InnoToolTarget=macos-arm64 `
    "-p:InnoProductBuildProperties=$productProperties" -p:InnoTestNativeProduct=editor `
    -p:InnoTestNativeTarget=macos-arm64 2>&1 | Tee-Object (Join-Path $evidence 'target-tests-build.log')
if ($LASTEXITCODE -ne 0) { throw 'Mac target test build failed.' }
& ./tests/Invoke-IsolatedTests.ps1 -Dotnet $sdk -Configuration SourceDebug `
    -Target macos-arm64 -ProductProperties $productProperties -ArtifactsPath $testOutputs `
    -ResultsDirectory (Join-Path $evidence 'tests')
```

随后用第 5 阶段正式生成的脚本引用执行 Wiki 编译门禁。`HintPath` 来自当前 IDE 工程，不能使用 Windows 的 Library：

```powershell
[xml]$ide = Get-Content (Join-Path $canvas 'Inno.EditorScripts.csproj') -Raw
$wikiReferences = @()
foreach ($reference in $ide.Project.ItemGroup.Reference) {
    if ($reference.Include -in @('Inno.ScriptApi.Runtime', 'Inno.ScriptApi.Editor')) {
        $switch = if ($reference.Include.EndsWith('.Runtime')) { '--wiki-runtime-reference' } else { '--wiki-editor-reference' }
        $wikiReferences += @($switch, [IO.Path]::GetFullPath([string]$reference.HintPath, $canvas))
    }
}
if ($wikiReferences.Count -ne 4) { throw 'Both current logical script references are required.' }
& $sdk $cli verify $engine --configuration Release --dotnet $sdk --wiki --wiki-target macos-arm64 `
    --project-graph (Join-Path $evidence 'project-graph.json') `
    --wiki-api (Join-Path $evidence 'wiki-api.json') `
    --wiki-examples (Join-Path $evidence 'wiki-examples.json') @wikiReferences 2>&1 |
    Tee-Object (Join-Path $evidence 'architecture-wiki.log')
if ($LASTEXITCODE -ne 0) { throw 'Mac architecture/Wiki verification failed.' }
```

## 4. Editor Debug / Release 与 Metal

```powershell
$editorProject = Join-Path $engine 'entry/InnoEngine.Editor/InnoEngine.Editor.csproj'
foreach ($configuration in @('Debug', 'Release')) {
    $editorOutputs = Join-Path $outputs "editor-$configuration"
    & $sdk build $editorProject -c $configuration --disable-build-servers -m:1 -nr:false `
        -p:UseSharedCompilation=false "-p:ArtifactsPath=$editorOutputs" 2>&1 |
        Tee-Object (Join-Path $evidence "editor-$configuration-build.log")
    if ($LASTEXITCODE -ne 0) { throw "$configuration Editor build failed." }
    $editor = @(& $sdk msbuild $editorProject -nologo "-p:Configuration=$configuration" `
        -p:DesignTimeBuild=true "-p:ArtifactsPath=$editorOutputs" -getProperty:TargetPath)
    if ($LASTEXITCODE -ne 0 -or $editor.Count -ne 1 -or -not (Test-Path $editor[0])) { throw 'Editor TargetPath is unavailable.' }
    foreach ($project in @($sample)) {
        $name = Split-Path $project -Leaf
        & $sdk $editor[0] $project --graphics-api metal --window-visible true --smoke-frames 600 2>&1 |
            Tee-Object (Join-Path $evidence "editor-$configuration-$name-metal.log")
        if ($LASTEXITCODE -ne 0) { throw "$configuration/$name Metal smoke failed." }
    }
}
```

每个结果要求真实 Metal 初始化、600 帧标记、`Dispose end.`、退出 0，无 `BGFX FATAL`、`Teardown failure`、导入失败或 Missing 类型诊断。保留 Project `Logs/EditorBoot.log` 与 Native 日志。需要 Native surface 的隔离用例失败时不得以短 smoke 通过覆盖它。

## 5. 插件、IDE 与安装态

```powershell
foreach ($project in @($canvas, $rendering, $sample)) {
    $name = Split-Path $project -Leaf
    & $sdk $cli scripts --engine-root $engine --tools-target macos-arm64 --dotnet $sdk `
        --project $project --output (Join-Path $outputs "scripts-$name") --ide 2>&1 |
        Tee-Object (Join-Path $evidence "scripts-$name.log")
    if ($LASTEXITCODE -ne 0) { throw "$name script compilation failed." }
    & $sdk build (Join-Path $project 'Inno.EditorScripts.csproj') -c Debug `
        --disable-build-servers -m:1 -nr:false -p:UseSharedCompilation=false -warnaserror 2>&1 |
        Tee-Object (Join-Path $evidence "ide-$name.log")
    if ($LASTEXITCODE -ne 0) { throw "$name IDE compilation failed." }
}
& $sdk test (Join-Path $canvas 'Tests/InnoEngine.Canvas.Tests/InnoEngine.Canvas.Tests.csproj') `
    -c Release --disable-build-servers -m:1 -nr:false -p:UseSharedCompilation=false `
    -p:CanvasToolsTarget=macos-arm64 `
    --logger trx --results-directory (Join-Path $evidence 'canvas-tests')
if ($LASTEXITCODE -ne 0) { throw 'Canvas contracts failed.' }
```

安装态使用 Samples 当前 `Plugins/*.iplugin`，不修改包内容。源 Assets 改变时先通过正式 `plugin` 导出并同步安装包，再重新执行项目编译，记录包内容哈希。

## 6. Support Pack、CoreCLR 与 NativeAOT

```powershell
$packs = Join-Path $outputs 'support-packs'
& $sdk $cli support-pack --engine-root $engine --target macos-arm64 --tools-target macos-arm64 `
    --output $packs --dotnet $sdk 2>&1 | Tee-Object (Join-Path $evidence 'support-pack.log')
if ($LASTEXITCODE -ne 0) { throw 'Mac Support Pack preparation failed.' }
foreach ($deployment in @('coreclr', 'nativeaot')) {
    $gameOutput = Join-Path $outputs "game-$deployment"
    & $sdk $cli game --engine-root $engine --tools-target macos-arm64 --dotnet $sdk `
        --project $sample --support-packs $packs --output $gameOutput --target macos-arm64 `
        --deployment $deployment --startup-scene FlappyBird/FlappyBird.iscene 2>&1 |
        Tee-Object (Join-Path $evidence "game-$deployment-publish.log")
    if ($LASTEXITCODE -ne 0) { throw "$deployment publication failed." }
    $applications = @(Get-ChildItem $gameOutput -Directory -Filter '*.app' -Recurse)
    if ($applications.Count -ne 1) { throw 'Expected exactly one published application.' }
    $application = $applications[0].FullName
    $executableName = & /usr/libexec/PlistBuddy -c 'Print :CFBundleExecutable' (Join-Path $application 'Contents/Info.plist')
    if ($LASTEXITCODE -ne 0) { throw 'Cannot read the published executable identity.' }
    $player = Join-Path $application "Contents/MacOS/$executableName"
    & $player --graphics-api metal --window-visible true --smoke-frames 600 2>&1 |
        Tee-Object (Join-Path $evidence "game-$deployment-metal.log")
    if ($LASTEXITCODE -ne 0) { throw "$deployment Metal Player failed." }
}
```

Player 必须输出 `INNO-SMOKE frames=600`、非零绘制并正常退出；Editor 与 Player 使用各自实际协议。最终 App 在不运行源码构建工具的环境中复核启动、输入、存储与退出。

## 7. 设备操作与归档

| 检查 | 验收内容 |
| --- | --- |
| Metal surface 生命周期 | 反复打开、拖出和关闭附加 viewport，最小化、恢复；主线程与单线程退休测试均结束，不放宽超时。 |
| DPI / 焦点 / 输入 | Retina 和外接屏往返、实时缩放、键鼠捕获、失焦释放、Game View 与 Editor 焦点切换。 |
| Canvas / 渲染 | 文本、纹理、透明排序、点击和键盘；2D 图层、材质与场景渲染，不以 NOT_READY 帧作为最终画面。 |
| 创作与恢复 | Play/Stop、Scene/Prefab 保存、Undo/Redo、脚本成功/失败 reload、Missing 恢复、旧 ALC 退休。 |
| 音频 / 存储 / 长运行 | 实际播放、暂停、设备变化、存储写回；记录运行时长、帧时间、GC、资源与多轮 reload 后释放。 |

归档本页所列日志、TRX、普通/隔离 execution-summary、架构/Wiki JSON、设备型号、OS/SDK/工具版本、源 revision/dirty 内容以及逐项“通过/失败/阻塞/未执行”。所有适用断言通过后才关闭 Mac 项；没有设备不能关闭。结束时按项目规范播放 `/System/Library/Sounds/Glass.aiff`，失败则说明。
