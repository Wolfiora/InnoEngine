$ErrorActionPreference = 'Stop'
$directory = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/ci-retention-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
$cases = @(
    @{ name = 'failure-completed'; conclusion = 'failure'; export = $true; exportExit = 1; previous = $true; retained = $true; throws = $false },
    @{ name = 'cancelled-completed'; conclusion = 'cancelled'; export = $true; exportExit = 1; previous = $true; retained = $true; throws = $false },
    @{ name = 'failure-no-export'; conclusion = 'failure'; export = $false; exportExit = 1; previous = $true; retained = $false; throws = $false },
    @{ name = 'success-completed'; conclusion = 'success'; export = $true; exportExit = 0; previous = $false; retained = $true; throws = $false },
    @{ name = 'success-reconciliation-failed'; conclusion = 'success'; export = $true; exportExit = 1; previous = $false; retained = $false; throws = $true }
)
$originalOutput = $env:GITHUB_OUTPUT
$originalDirectory = (Get-Location).Path
try {
    foreach ($case in $cases) {
        $root = Join-Path $directory $case.name
        $scripts = Join-Path $root 'tests/ci'
        $output = Join-Path $root 'artifacts/validation/ci'
        New-Item -ItemType Directory -Path $scripts, $output -Force | Out-Null
        Copy-Item -LiteralPath "$PSScriptRoot/Invoke-ValidationCi.ps1" -Destination $scripts
        'fixture' | Set-Content (Join-Path $root 'source.txt')
        & git -C $root init -q
        if ($LASTEXITCODE -ne 0) { throw 'Cannot initialize the retention fixture.' }
        & git -C $root add source.txt
        & git -C $root -c user.name='Aaron Liao' -c user.email='hsuankailiao@gmail.com' commit -qm 'Retention contract fixture'
        if ($LASTEXITCODE -ne 0) { throw 'Cannot commit the retention fixture.' }
        $commit = (& git -C $root rev-parse HEAD).Trim()
        $assembly = Join-Path $root 'entry.dll'
        'entry fixture' | Set-Content $assembly
        @{ assembly = $assembly; sha256 = (Get-FileHash $assembly).Hash } | ConvertTo-Json | Set-Content (Join-Path $output 'entry.json')
        $case | ConvertTo-Json | Set-Content (Join-Path $root 'case.json')
        $stub = Join-Path $root 'dotnet-fixture.ps1'
        @'
param([Parameter(ValueFromRemainingArguments)][string[]]$Arguments)
$case = Get-Content (Join-Path $PSScriptRoot 'case.json') -Raw | ConvertFrom-Json
$position = [Array]::IndexOf($Arguments, '--cache-output')
if ($position -lt 0) { throw 'The retention entry did not request an explicit immutable export.' }
if ($case.export) { [IO.File]::WriteAllText($Arguments[$position + 1], 'completed immutable export') }
$global:LASTEXITCODE = [int]$case.exportExit
'@ | Set-Content $stub
        $transport = Join-Path $root 'artifacts/validation/native-cache.zip'
        if ($case.previous) { 'previous candidate' | Set-Content $transport }
        $env:GITHUB_OUTPUT = Join-Path $root 'outputs.txt'
        [IO.File]::WriteAllText($env:GITHUB_OUTPUT, '')
        $threw = $false
        try {
            & (Join-Path $scripts 'Invoke-ValidationCi.ps1') -Stage Retain -BaseRevision $commit -ExpectedCommit $commit `
                -Dotnet $stub -ValidationConclusion $case.conclusion
        } catch { $threw = $true; $_.Exception.Message | Set-Content (Join-Path $root 'failure.log') }
        if ($threw -ne $case.throws) { throw "Unexpected retention failure state: $($case.name)" }
        $published = [IO.File]::ReadAllText($env:GITHUB_OUTPUT)
        if (($published -match '(?m)^native=') -ne $case.retained) { throw "Wrong retention applicability: $($case.name)" }
        if ($case.retained) {
            $archive = ($published.Trim() -split '=', 2)[1]
            if ([IO.File]::ReadAllText($transport) -cne 'completed immutable export' -or
                (Get-FileHash $archive).Hash -cne (Get-FileHash $transport).Hash) {
                throw "Transport does not contain exactly the new completed export: $($case.name)"
            }
        } elseif ($case.previous -and [IO.File]::ReadAllText($transport).Trim() -cne 'previous candidate') {
            throw 'An incomplete export replaced the original diagnostic candidate.'
        }
    }
} finally {
    $env:GITHUB_OUTPUT = $originalOutput
    Set-Location -LiteralPath $originalDirectory
}
Write-Output "Passed $($cases.Count) CI retention protocol cases. These simulate the CLI protocol; real native export is validated separately. Evidence: $directory"
$global:LASTEXITCODE = 0
