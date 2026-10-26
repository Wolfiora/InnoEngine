param(
    [string]$Dotnet = 'dotnet',
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug',
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

$warmup = Join-Path $results 'warmup.log'
& $Dotnet build $entry --configuration $Configuration --disable-build-servers -m:1 -nr:false 2>&1 |
    Tee-Object -FilePath $warmup
if ($LASTEXITCODE -ne 0) { throw "The selected configuration could not be prepared; see $warmup." }
$before = Read-Selections
for ($iteration = 1; $iteration -le $Iterations; $iteration++) {
    $log = Join-Path $results "hot-$iteration.log"
    $timer = [Diagnostics.Stopwatch]::StartNew()
    & $Dotnet build $entry --configuration $Configuration --disable-build-servers -m:1 -nr:false `
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
