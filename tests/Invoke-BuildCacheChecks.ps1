param(
    [string]$Dotnet = 'dotnet',
    [Parameter(Mandatory)][string]$PlanPath,
    [Parameter(Mandatory)][string]$Context,
    [string]$ResultsDirectory = 'artifacts/acceptance/build-cache',
    [ValidateRange(10, 100)]
    [int]$Iterations = 10
)

$ErrorActionPreference = 'Stop'
$repository = Split-Path $PSScriptRoot -Parent
$Dotnet = (Get-Command $Dotnet -ErrorAction Stop).Source
$results = [IO.Path]::GetFullPath($ResultsDirectory, $repository)
New-Item -ItemType Directory -Force -Path $results | Out-Null
$entry = Join-Path $repository 'entry/InnoEngine.Editor/InnoEngine.Editor.csproj'
$plan = Get-Content -LiteralPath $PlanPath -Raw | ConvertFrom-Json
if ([IO.Path]::GetFullPath($plan.root) -ne [IO.Path]::GetFullPath($repository)) { throw 'The benchmark plan belongs to another checkout.' }
$selection = $plan.contexts.$Context
if ($null -eq $selection -or -not $selection.properties.InnoNativeTarget) { throw 'A declared native execution context is required.' }
$editor = @($selection.sdkGraph | Where-Object { [IO.Path]::GetFullPath($_.Path) -eq $entry })
if ($editor.Count -ne 1 -or -not (Test-Path -LiteralPath $editor[0].Properties.TargetPath)) { throw 'The plan must build its Editor union before hot sampling.' }
$buildArguments = @($selection.properties.PSObject.Properties | ForEach-Object { "-p:$($_.Name)=$($_.Value)" })
$samples = [Collections.Generic.List[object]]::new()

function Read-Selections {
    $state = @{}
    foreach ($folder in @('artifacts/native', 'artifacts/bindings')) {
        $root = Join-Path $repository $folder
        if (-not (Test-Path -LiteralPath $root)) { continue }
        foreach ($file in Get-ChildItem -LiteralPath $root -Recurse -File -Filter '*.current') {
            $state[$file.FullName] = @($file.LastWriteTimeUtc.Ticks, (Get-FileHash -LiteralPath $file.FullName).Hash)
        }
    }
    return $state
}

$before = Read-Selections
for ($iteration = 1; $iteration -le $Iterations; $iteration++) {
    $log = Join-Path $results "hot-$iteration.log"
    $timer = [Diagnostics.Stopwatch]::StartNew()
    & $Dotnet build $entry @buildArguments --disable-build-servers -m:1 -nr:false `
        "-bl:$(Join-Path $results "hot-$iteration.binlog")" 2>&1 | Tee-Object -FilePath $log
    if ($LASTEXITCODE -ne 0) { throw "Hot build $iteration failed; see $log." }
    $timer.Stop()
    $content = Get-Content -LiteralPath $log -Raw
    $preparations = [regex]::Matches($content, 'INNO-NATIVE-PREPARE[^\r\n]*processes=(\d+)[^\r\n]*bindingGenerations=(\d+)')
    if ($preparations.Count -eq 0) { throw "Build $iteration did not report its Native preparation counters." }
    foreach ($preparation in $preparations) {
        if ($preparation.Groups[1].Value -ne '0' -or $preparation.Groups[2].Value -ne '0') {
            throw "Build $iteration unexpectedly compiled Native inputs or regenerated bindings: $($preparation.Value)"
        }
    }
    $requests = [regex]::Matches($content, 'INNO-NATIVE-PREPARE request=([a-f0-9]{64})[^\r\n]*reusedPreparation=(\d+)')
    if ($requests.Count -ne $preparations.Count) { throw "Build $iteration lacks complete frozen request identities." }
    foreach ($request in $requests | Group-Object { $_.Groups[1].Value }) {
        if (@($request.Group | Where-Object { $_.Groups[2].Value -eq '0' }).Count -gt 1) {
            throw "Build $iteration prepared the same frozen native request more than once: $($request.Name)"
        }
    }
    $after = Read-Selections
    if ($before.Count -ne $after.Count) { throw "Build $iteration changed the stable selection count." }
    foreach ($path in $before.Keys) {
        if (-not $after.ContainsKey($path) -or ($before[$path] -join '|') -cne ($after[$path] -join '|')) {
            throw "Build $iteration rewrote a stable selection: $path"
        }
    }
    $samples.Add([pscustomobject]@{ iteration = $iteration; elapsedMilliseconds = $timer.Elapsed.TotalMilliseconds
        nativeProcesses = 0; bindingGenerations = 0; selectionCount = $after.Count })
    ConvertTo-Json -InputObject @($samples.ToArray()) -Depth 4 |
        Set-Content -LiteralPath (Join-Path $results 'hot-builds.json') -Encoding utf8
}
$ordered = @($samples.elapsedMilliseconds | Sort-Object)
$middle = [int][Math]::Floor($ordered.Count / 2)
$median = if ($ordered.Count % 2) { $ordered[$middle] } else { ($ordered[$middle - 1] + $ordered[$middle]) / 2 }
Write-Host "Validated $Iterations hot builds; median=$median ms; nativeProcesses=0; bindingGenerations=0."
