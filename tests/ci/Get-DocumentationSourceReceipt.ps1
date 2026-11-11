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
        if ($project.status -cne 'Passed' -or @($project.differences).Count -ne 0 -or [string]::IsNullOrWhiteSpace($project.name)) {
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
    [pscustomobject]@{
        check = $check.id
        reportIdentity = (Get-FileHash -LiteralPath $file -Algorithm SHA256).Hash.ToLowerInvariant()
        catalogIdentity = (Get-FileHash -LiteralPath $catalogPath -Algorithm SHA256).Hash.ToLowerInvariant()
        configuration = $report.configuration
        target = $report.target
        references = @($report.references)
        projects = @($report.projects | ForEach-Object { $_.name })
    }
}
