param(
    [Parameter(Mandatory)][string] $Dotnet,
    [Parameter(Mandatory)][string] $Transport,
    [Parameter(Mandatory)][string] $InputDirectory,
    [Parameter(Mandatory)][string] $OutputDirectory,
    [Parameter(Mandatory)][string] $ExpectedCommit,
    [Parameter(Mandatory)][string[]] $Configurations
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$engine = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$inputRoot = [IO.Path]::GetFullPath($InputDirectory)
$outputRoot = [IO.Path]::GetFullPath($OutputDirectory)
if ($ExpectedCommit -cnotmatch '^[a-f0-9]{40}$' -or $Configurations.Count -eq 0 -or
    @($Configurations | Select-Object -Unique).Count -ne $Configurations.Count) { throw 'Exact source and distinct configurations are required.' }
$actual = (& git -C $engine rev-parse HEAD).Trim()
$tree = (& git -C $engine rev-parse 'HEAD^{tree}').Trim()
if ($LASTEXITCODE -ne 0 -or $actual -cne $ExpectedCommit) { throw 'The aggregation checkout differs from its source producers.' }
if (Test-Path -LiteralPath $outputRoot) { throw 'API aggregation needs an exclusively owned new staging directory.' }
[IO.Directory]::CreateDirectory($outputRoot) | Out-Null
$expectedInventory = Join-Path $outputRoot 'expected-projects.json'
& $Dotnet $Transport docs export-projects --engine-root $engine --output $expectedInventory
if ($LASTEXITCODE -ne 0) { throw 'Cannot establish the actual source inventory before API aggregation.' }
$expectedInventoryIdentity = (Get-FileHash -LiteralPath $expectedInventory -Algorithm SHA256).Hash.ToLowerInvariant()
function Read-Owned([string] $Directory, [string] $Name, [string] $Identity) {
    if ($Name -cnotmatch '^[a-zA-Z0-9-]+\.json$' -or $Identity -cnotmatch '^[a-f0-9]{64}$') { throw 'Invalid producer file identity.' }
    $path = Join-Path $Directory $Name
    if ((Get-Item -LiteralPath $path).Attributes -band [IO.FileAttributes]::ReparsePoint) { throw 'Producer files cannot be links.' }
    if ((Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash.ToLowerInvariant() -cne $Identity) { throw "Corrupt producer file: $Name" }
    return [IO.File]::ReadAllText($path)
}
$producers = [Collections.Generic.List[object]]::new()
$contexts = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$inventoryIdentity = $null
foreach ($receipt in Get-ChildItem -LiteralPath $inputRoot -Filter producer.json -Recurse -File) {
    $data = Get-Content -LiteralPath $receipt.FullName -Raw | ConvertFrom-Json
    if ($data.schema -cne 'inno-api-producer/1' -or $data.repository -cne 'https://github.com/Wolfiora/InnoEngine' -or
        $data.revision -cne $ExpectedCommit -or $data.sourceTree -cne $tree -or $data.toolingRevision -cne $ExpectedCommit -or
        $data.configuration -cnotin $Configurations -or $data.hostTarget -cnotmatch '^[a-z0-9-]+$' -or
        !$contexts.Add("$($data.configuration)/$($data.hostTarget)")) { throw 'Repeated, stale or foreign API producer context.' }
    $inventoryText = Read-Owned $receipt.DirectoryName 'projects.json' $data.projectManifestIdentity
    if ($data.projectManifestIdentity -cne $expectedInventoryIdentity) { throw 'API producer omitted or invented an actual source owner.' }
    if ($null -ne $inventoryIdentity -and $inventoryIdentity -cne $data.projectManifestIdentity) { throw 'API producers disagree about the actual owner inventory.' }
    $inventoryIdentity = $data.projectManifestIdentity
    $plan = (Read-Owned $receipt.DirectoryName 'plan.json' $data.planIdentity) | ConvertFrom-Json
    if ($plan.schema -cne 'inno-api-export-plan/1' -or $plan.hostTarget -cne $data.hostTarget -or
        $plan.configuration -cne $data.configuration -or $plan.sdkRuntime -cne $data.sdkRuntime) { throw 'API producer and evaluated plan disagree.' }
    $producers.Add(@{ data = $data; plan = $plan; directory = $receipt.DirectoryName })
}
if ($producers.Count -eq 0) { throw 'No completed API producers were discovered.' }
$inventory = $inventoryText | ConvertFrom-Json
$selected = [Collections.Generic.List[object]]::new()
foreach ($producer in $producers) {
    $data = $producer.data
    foreach ($capture in $data.captures) {
        $text = Read-Owned $producer.directory $capture.path $capture.sha256
        $facts = $text | ConvertFrom-Json
        if ($capture.configuration -cne $data.configuration -or $facts.configuration -cne $capture.configuration -or
            $facts.target -cne $capture.target -or $facts.model -cne $capture.model) { throw 'Capture facts differ from the producer context.' }
        # Prefer the real target producer. Foreign empty authored surfaces are accepted only
        # when no producer owns that target and every alternative reports identical empty facts.
        $nativeOwner = @($producers | Where-Object { $_.data.configuration -ceq $data.configuration -and $_.data.hostTarget -ceq $capture.target })
        if ($capture.target -cne $data.hostTarget -and $nativeOwner.Count -ne 0) { continue }
        if ($capture.target -cne $data.hostTarget -and ($capture.model -cne 'source-clr' -or
            @($facts.projects | Where-Object { @($_.symbols).Count -ne 0 }).Count -ne 0)) { throw 'A foreign producer cannot certify a nonempty API target.' }
        $duplicate = @($selected | Where-Object { $_.capture.configuration -ceq $capture.configuration -and
            $_.capture.target -ceq $capture.target -and $_.capture.model -ceq $capture.model })
        if ($duplicate.Count -ne 0) {
            if ($duplicate.Count -ne 1 -or $duplicate[0].capture.sha256 -cne $capture.sha256) { throw 'Producers disagree about one API context.' }
            continue
        }
        $selected.Add(@{ capture = $capture; facts = $facts; text = $text })
    }
}
foreach ($configuration in $Configurations) {
    $configured = @($producers | Where-Object { $_.data.configuration -ceq $configuration })
    if ($configured.Count -eq 0) { throw "Missing required API configuration: $configuration" }
    foreach ($producer in $configured) {
        foreach ($owner in $producer.plan.projects) {
            $target = if ($owner.target) { $owner.target } else { $producer.data.hostTarget }
            $coverage = @($selected | Where-Object { $_.capture.configuration -ceq $configuration -and $_.capture.target -ceq $target -and $_.capture.model -ceq 'source-clr' }
                | ForEach-Object { $_.facts.projects } | Where-Object project -CEQ $owner.path)
            if ($coverage.Count -ne 1) { throw "Required API owner/context is absent or repeated: $configuration/$target/$($owner.path)" }
        }
        foreach ($model in @('runtime-script', 'editor-script')) {
            if (@($selected | Where-Object { $_.capture.configuration -ceq $configuration -and
                $_.capture.target -ceq $producer.data.hostTarget -and $_.capture.model -ceq $model }).Count -ne 1) {
                throw "Missing actual $model projection for $configuration/$($producer.data.hostTarget)."
            }
        }
    }
}
[IO.Directory]::CreateDirectory((Join-Path $outputRoot 'reference/api')) | Out-Null
[IO.Directory]::CreateDirectory((Join-Path $outputRoot 'reference/projects')) | Out-Null
$manifests = @()
foreach ($item in $selected | Sort-Object { $_.capture.configuration }, { $_.capture.target }, { $_.capture.model }) {
    $capture = $item.capture
    $path = "reference/api/$($capture.configuration)-$($capture.target)-$($capture.model).json"
    [IO.File]::WriteAllText((Join-Path $outputRoot $path), $item.text, [Text.UTF8Encoding]::new($false))
    $manifests += @{ path = $path; sha256 = $capture.sha256; model = $capture.model; configuration = $capture.configuration;
        target = $capture.target; sourceRevision = $ExpectedCommit; sourceTree = $tree }
}
foreach ($project in $inventory) {
    $surfaces = @($selected | Where-Object { @($_.facts.projects | Where-Object name -CEQ $project.name).Count -ne 0 } | ForEach-Object { $_.capture.model } | Sort-Object -Unique)
    if ('runtime-script' -cin $surfaces -and 'editor-script' -cnotin $surfaces) { $surfaces += 'editor-script' }
    $project.surfaces = @($surfaces)
}
$projectPath = Join-Path $outputRoot 'reference/projects/catalog.json'
[IO.File]::WriteAllText($projectPath, (ConvertTo-Json -InputObject @($inventory) -Depth 30), [Text.UTF8Encoding]::new($false))
$binding = @{ repository = 'https://github.com/Wolfiora/InnoEngine'; revision = $ExpectedCommit; sourceTree = $tree;
    toolingRevision = $ExpectedCommit; projectManifest = 'reference/projects/catalog.json';
    projectManifestIdentity = (Get-FileHash -LiteralPath $projectPath -Algorithm SHA256).Hash.ToLowerInvariant(); apiManifests = $manifests }
[IO.File]::WriteAllText((Join-Path $outputRoot 'reference/engine.lock.json'), ($binding | ConvertTo-Json -Depth 30), [Text.UTF8Encoding]::new($false))
& $Dotnet $Transport docs api-package --docs-root $outputRoot --package (Join-Path $outputRoot 'api-package.json')
if ($LASTEXITCODE -ne 0) { throw 'Canonical API package validation failed; nothing can be published.' }
Write-Host "Reconciled $($producers.Count) producers and $($selected.Count) API contexts for $ExpectedCommit."
