param(
    [string]$Dotnet = 'dotnet',
    [Parameter(Mandatory)][string]$PlanPath,
    [Parameter(Mandatory)][string]$Context,
    [string]$ResultsDirectory = 'artifacts/acceptance/build-cache'
)

$ErrorActionPreference = 'Stop'
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$Dotnet = (Get-Command $Dotnet -ErrorAction Stop).Source
$results = [IO.Path]::GetFullPath($ResultsDirectory, $repository)
New-Item -ItemType Directory -Force -Path $results | Out-Null
$entry = Join-Path $repository 'entry/InnoEngine.Editor/InnoEngine.Editor.csproj'
$plan = Get-Content -LiteralPath $PlanPath -Raw | ConvertFrom-Json
if ([IO.Path]::GetFullPath($plan.root) -ne [IO.Path]::GetFullPath($repository)) { throw 'The validation plan belongs to another checkout.' }
$selection = $plan.contexts.$Context
if ($null -eq $selection -or -not $selection.properties.InnoNativeTarget) { throw 'A declared native execution context is required.' }
$editor = @($selection.sdkGraph | Where-Object { [IO.Path]::GetFullPath($_.Path) -eq $entry })
if ($editor.Count -ne 1 -or -not (Test-Path -LiteralPath $editor[0].Properties.TargetPath)) { throw 'The plan must build its Editor union before checking reuse.' }
$buildArguments = @($selection.properties.PSObject.Properties | ForEach-Object { "-p:$($_.Name)=$($_.Value)" })

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
$log = Join-Path $results "hot-build.log"
& $Dotnet build $entry @buildArguments --disable-build-servers -m:1 -nr:false `
    "-bl:$(Join-Path $results "hot-build.binlog")" 2>&1 | Tee-Object -FilePath $log
if ($LASTEXITCODE -ne 0) { throw "Hot build failed; see $log." }
$content = Get-Content -LiteralPath $log -Raw
$consumeOnly = -not [string]::IsNullOrWhiteSpace($selection.properties.InnoNativeDependencyManifest)
$operation = if ($consumeOnly) { 'CONSUME' } else { 'PREPARE' }
if ($consumeOnly -and $content -match 'INNO-NATIVE-PREPARE') { throw 'The consume-only build invoked a source producer.' }
$preparations = [regex]::Matches($content, "INNO-NATIVE-$operation[^\r\n]*processes=(\d+)[^\r\n]*bindingGenerations=(\d+)")
if ($preparations.Count -eq 0) { throw "The build did not report its Native preparation counters." }
foreach ($preparation in $preparations) {
    if ($preparation.Groups[1].Value -ne '0' -or $preparation.Groups[2].Value -ne '0') {
        throw "The build unexpectedly compiled Native inputs or regenerated bindings: $($preparation.Value)"
    }
}
$requests = [regex]::Matches($content, 'INNO-NATIVE-PREPARE request=([a-f0-9]{64})[^\r\n]*reusedPreparation=(\d+)')
if (-not $consumeOnly -and $requests.Count -ne $preparations.Count) { throw "The build lacks complete frozen request identities." }
foreach ($request in $requests | Group-Object { $_.Groups[1].Value }) {
    if (@($request.Group | Where-Object { $_.Groups[2].Value -eq '0' }).Count -gt 1) {
        throw "The build prepared the same frozen native request more than once: $($request.Name)"
    }
}
$after = Read-Selections
if ($before.Count -ne $after.Count) { throw "The build changed the stable selection count." }
foreach ($path in $before.Keys) {
    if (-not $after.ContainsKey($path) -or ($before[$path] -join '|') -cne ($after[$path] -join '|')) {
        throw "The build rewrote a stable selection: $path"
    }
}
Write-Host "Verified unchanged build: stable selections, no native processes and no binding generation."
