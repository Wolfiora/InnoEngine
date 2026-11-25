param(
    [Parameter(Mandatory)][string]$PlanPath,
    [Parameter(Mandatory)][string]$Acquisition,
    [Parameter(Mandatory)][string]$EntryAssembly,
    [Parameter(Mandatory)][string]$Downloads,
    [string]$Candidates,
    [array]$Promoted = @(),
    [string]$RebuildTargets,
    [string]$Dotnet = 'dotnet',
    [string]$Repository = $env:GITHUB_REPOSITORY,
    [string]$ApiUrl = 'https://api.github.com'
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$root = [IO.Path]::GetFullPath('../..',$PSScriptRoot)
$plan = Get-Content -LiteralPath $PlanPath -Raw | ConvertFrom-Json -AsHashtable -Depth 100
$request = Get-Content -LiteralPath $Acquisition -Raw | ConvertFrom-Json -AsHashtable
if (!$plan.backendStore -or $null -ne $plan.backendPackages -or $request.commit -cne $plan.commit) {
    throw 'Acquisition belongs to a different candidate or an already bound plan.'
}
$dotnetPath = (Get-Command $Dotnet -CommandType Application | Select-Object -First 1).Source
$entry = (Get-Item -LiteralPath $EntryAssembly).FullName
$store = [IO.Path]::GetFullPath($plan.backendStore,$root)
$downloadsRoot = [IO.Directory]::CreateDirectory([IO.Path]::GetFullPath($Downloads,$root)).FullName
$registry = @(Get-Content -LiteralPath (Join-Path $root '.github/native-targets.json') -Raw | ConvertFrom-Json)
$receipts = @(foreach ($package in $request.packages) {
    $target = $package.target
    $registered = @($registry | Where-Object target -CEQ $target)
    if ($registered.Count -ne 1) { throw "Unregistered backend platform: $target." }
    $candidate = if ($Candidates) { Join-Path $Candidates "backend-candidate-$target" }
    if ($candidate -and (Test-Path -LiteralPath (Join-Path $candidate 'consumption.json'))) {
        $archive = Join-Path $candidate $registered[0].asset
        $receipt = Get-Content -LiteralPath (Join-Path $candidate 'consumption.json') -Raw | ConvertFrom-Json
        $source = @($Promoted | Where-Object target -CEQ $target)
        $expectedCommit = if ($source.Count -eq 1) { $source[0].promotion.commit } else { $plan.commit }
        if ($source.Count -gt 1 -or $receipt.commit -cne $expectedCommit -or $receipt.target -cne $target) {
            throw "The produced $target candidate belongs to another commit or asset."
        }
        $digest = $receipt.sha256
    } elseif ($package.operation -ceq 'consume') {
        $digest = $package.sha256
        if ($digest -cnotmatch '^[a-f0-9]{64}$' -or [string]$package.assetId -cnotmatch '^[1-9][0-9]*$') { throw 'Invalid exact backend asset identity.' }
        $archive = & "$PSScriptRoot/Get-BackendPlatformArchive.ps1" -Sha256 $digest -AssetId $package.assetId -Directory $downloadsRoot -Repository $Repository -ApiUrl $ApiUrl
    } else { throw "Unexpected acquisition operation for $target." }
    if ($digest -cnotmatch '^[a-f0-9]{64}$') { throw 'Incomplete backend archive digest.' }
    $installed = Join-Path $store $target
    & $dotnetPath $entry validate import-backends --archive $archive --sha256 $digest --target $target --output $installed | Out-Host
    if ($LASTEXITCODE) { throw "Backend archive failed verification for $target." }
    Get-Content -LiteralPath (Join-Path $installed 'receipt.json') -Raw | ConvertFrom-Json
})
$receiptPath = Join-Path (Split-Path ([IO.Path]::GetFullPath($Acquisition)) -Parent) 'packages.json'
[IO.File]::WriteAllText($receiptPath, (ConvertTo-Json -InputObject $receipts -Depth 5 -Compress))
$testEvidence = Join-Path (Split-Path $receiptPath -Parent) 'backend-test-evidence.json'
$certificates = @($request.testEvidence)
if ($RebuildTargets) {
    $requested = @($RebuildTargets.Split(',') | ForEach-Object { $_.Trim() })
    if (@($request.packages | Where-Object { $_.target -cin $requested }).Count) { $certificates = @() }
}
[IO.File]::WriteAllText($testEvidence, (ConvertTo-Json -InputObject $certificates -Depth 20 -Compress))
& $dotnetPath $entry validate bind-backends --test-evidence $testEvidence --packages $receiptPath --plan $PlanPath --engine-root $root --dotnet $dotnetPath --output $PlanPath
if ($LASTEXITCODE) { throw 'The installed backend packages do not cover the frozen checks.' }
