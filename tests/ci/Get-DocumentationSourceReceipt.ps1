param(
    [Parameter(Mandatory)]$Plan,
    [Parameter(Mandatory)]$Evidence,
    [Parameter(Mandatory)]$Pairing,
    [Parameter(Mandatory)][string]$Directory
)

$ErrorActionPreference = 'Stop'
$selected = @($Plan.checks | Where-Object { $_.kind -ceq 'documentation-api' -and $null -eq $_.exclusion })
if ($selected.Count -eq 0) { return }
$directory = [IO.Path]::GetFullPath($Directory)
$catalogPath = Join-Path $directory 'documentation-catalog.json'
if ((Get-Item -LiteralPath $catalogPath).LinkType) { throw 'Source certification cannot follow a linked catalog.' }
$catalog = Get-Content -LiteralPath $catalogPath -Raw | ConvertFrom-Json
if ($catalog.snapshot -cne $Pairing.documentationSnapshot -or $catalog.sourceTree -cne $Pairing.engineTree `
    -or $catalog.contentIdentity -cne $Pairing.contentIdentity -or $catalog.repository -cne 'https://github.com/Wolfiora/InnoEngine') {
    throw 'Source certification catalog differs from the reconciled documentation pair.'
}
foreach ($check in $selected) {
    if ($check.id -cnotmatch '^documentation-api:[a-zA-Z0-9_-]+:[a-zA-Z0-9_-]+$') { throw 'Invalid source comparison report identity.' }
    $result = @($Evidence.results | Where-Object { $_.id -ceq $check.id -and $_.status -cin @('Passed', 'Reused') })
    if ($result.Count -ne 1) { throw "Source comparison did not pass: $($check.id)" }
    $file = Join-Path $directory ($check.id.Replace(':', '-') + '.json')
    if ((Get-Item -LiteralPath $file).LinkType) { throw 'Source certification cannot follow a linked comparison report.' }
    $report = Get-Content -LiteralPath $file -Raw | ConvertFrom-Json
    if ($report.status -cne 'Passed' -or $report.scope -cne 'SelectedSourceReferenceComparison' `
        -or $report.documentationSnapshot -cne $catalog.snapshot -or $report.documentationSourceTree -cne $catalog.sourceTree `
        -or $report.configuration -cne $check.documentation.configuration -or $report.target -cne $check.documentation.target) {
        throw "Source comparison context differs from its planned obligation: $($check.id)"
    }
    $actual = @($report.projects | ForEach-Object { $_.project } | Sort-Object -CaseSensitive)
    $expected = @($check.projects | Sort-Object -CaseSensitive)
    if ($actual.Count -eq 0 -or ($actual -join "`n") -cne ($expected -join "`n") `
        -or @($actual | Select-Object -Unique).Count -ne $actual.Count) { throw 'Source comparison owner coverage is incomplete or duplicated.' }
    foreach ($project in @($report.projects)) {
        if ($project.status -cne 'Passed' -or @($project.differences).Count -ne 0 -or [string]::IsNullOrWhiteSpace($project.name) `
            -or $project.referenceIdentity -cnotmatch '^[a-f0-9]{64}$') {
            throw "Source owner did not pass: $($project.project)"
        }
    }
    $referencePaths = @($report.references | ForEach-Object { $_.path } | Sort-Object -CaseSensitive)
    if (($referencePaths -join "`n") -cne (@($check.documentation.references | Sort-Object -CaseSensitive) -join "`n") `
        -or $referencePaths.Count -eq 0 -or @($referencePaths | Select-Object -Unique).Count -ne $referencePaths.Count) {
        throw 'Source comparison reference selection differs from the plan.'
    }
    foreach ($reference in @($report.references)) {
        $bound = @($catalog.references | Where-Object { $_.path -ceq $reference.path -and $_.identity -ceq $reference.identity `
            -and $_.model -ceq 'source-clr' -and $_.configuration -ceq $report.configuration -and $_.target -ceq $report.target })
        if ($bound.Count -ne 1) { throw "Source comparison reference differs from the immutable catalog: $($reference.path)" }
    }
    $compiled = @($check.inputs | Where-Object { $_ -like '@*' -and $_ -cnotin @('@tooling', '@native-sources') }).Count -gt 0
    $proofs = @($result[0].documentationInputs | Where-Object { $null -ne $_ })
    if ($proofs.Count -ne $actual.Count) {
        throw 'Source input provenance does not cover its actual compiled owners.'
    }
    $seenInputs = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    $inputProofs = foreach ($proof in $proofs) {
        $owner = @($report.projects | Where-Object project -CEQ $proof.project)
        if ($owner.Count -ne 1 -or -not $seenInputs.Add($proof.project) `
            -or $proof.sourceIdentity -cnotmatch '^[a-f0-9]{64}$' -or $Plan.environment -cnotmatch '^[a-f0-9]{64}$' `
            -or $proof.environment -cne $Plan.environment -or $proof.configuration -cne $report.configuration -or $proof.target -cne $report.target `
            -or @($proof.owners).Count -eq 0 -or $proof.project -cnotin $proof.owners `
            -or @($proof.owners | Sort-Object -Unique).Count -ne @($proof.owners).Count -or $null -eq $proof.packages) {
            throw 'Source input provenance has missing, duplicate or mismatched owners, environment or context.'
        }
        foreach ($path in $proof.owners) {
            if ([string]::IsNullOrWhiteSpace($path) -or [IO.Path]::IsPathRooted($path) -or $path.Contains('\') -or $path.Contains(':') `
                -or $path -match '(^|/)\.{1,2}(/|$)' -or -not $path.EndsWith('.csproj')) {
                throw 'Source input owner escapes its canonical repository boundary.'
            }
        }
        $packages = $proof.packages | ConvertTo-Json -Depth 4 | ConvertFrom-Json -AsHashtable
        if ($packages -isnot [Collections.IDictionary]) { throw 'Source package provenance must be an explicit content map.' }
        if (-not $compiled -and $packages.Count -ne 0) { throw 'Source-only inspection cannot claim compiled package inputs.' }
        foreach ($package in $packages.GetEnumerator()) {
            if ($package.Key -cnotmatch '^[a-z0-9_.-]+/[a-z0-9_.+-]+$' -or $package.Key -match '(^|/)\.{1,2}(/|$)' `
                -or $package.Value -cnotmatch '^[a-f0-9]{64}$') { throw 'Source package provenance has an invalid path or content identity.' }
        }
        $sources = & "$PSScriptRoot/Get-DocumentationSourceInputs.ps1" -Inputs $proof.sources
        [pscustomobject]@{ project = $owner[0].name; path = $proof.project; owners = @($proof.owners)
            sourceIdentity = $proof.sourceIdentity; environment = $proof.environment; configuration = $proof.configuration; target = $proof.target
            referenceIdentity = $owner[0].referenceIdentity; packages = $packages; sources = $sources }
    }
    [pscustomobject]@{
        check = $check.id
        reportIdentity = (Get-FileHash -LiteralPath $file -Algorithm SHA256).Hash.ToLowerInvariant()
        catalogIdentity = (Get-FileHash -LiteralPath $catalogPath -Algorithm SHA256).Hash.ToLowerInvariant()
        configuration = $report.configuration
        target = $report.target
        references = @($report.references)
        projects = @($report.projects | ForEach-Object { $_.name })
        inputMode = $(if ($compiled) { 'Compiled' } else { 'SourceOnly' })
        inputs = @($inputProofs)
    }
}
