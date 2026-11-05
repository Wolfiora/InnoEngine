$ErrorActionPreference = 'Stop'
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$entry = Join-Path $PSScriptRoot 'Test-AgentRules.ps1'
$root = @(& $entry -Paths 'src/services/input/Example.cs' -Rules completion) -join "`n"
Push-Location (Join-Path $repository 'tools')
try { $nested = @(& $entry -Paths 'src/services/input/Example.cs' -Rules completion) -join "`n" }
finally { Pop-Location }
if ($root -cne $nested) { throw 'Root and nested invocation selected different instruction units.' }
if ($root -notmatch 'Preserved mapping: 241 rules') { throw 'Original rule coverage is incomplete.' }
foreach ($scenario in @(@{ Rules = 'unknown-unit' }, @{ Behaviors = 'unknown-behavior' }, @{ Stage = 999 })) {
    $rejected = $false
    try { & $entry @scenario | Out-Null } catch { $rejected = $true }
    if (-not $rejected) { throw 'Invalid rule selection was accepted.' }
}
$manifest = Get-Content -LiteralPath (Join-Path $repository '.agents/rulebook/coverage.json') -Raw | ConvertFrom-Json
foreach ($unit in $manifest.units) {
    if (-not $unit.sourceHash -or -not $unit.check -or -not $unit.reviewer) { throw "Incomplete rule responsibility: $($unit.id)" }
}
$all = @(& $entry -Rules $manifest.units.id) -join "`n"
foreach ($match in [regex]::Matches($all, 'Stage \d+: (\d+) bytes')) {
    if ([int]$match.Groups[1].Value -gt 16384) { throw 'A staged instruction set exceeds its complete-read budget.' }
}
$first = @(& $entry -Paths 'src/services/input/Example.cs' -Rules completion -Stage 1 -ShowInstructions) -join "`n"
if (-not $first.Contains('# InnoEngine agent instructions') -or -not $first.Contains('完整')) { throw 'Actual instruction bodies were not emitted.' }
$fixtureParent = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$fixture = Join-Path $fixtureParent ('InnoRuleTests-' + [guid]::NewGuid().ToString('N'))
try {
    & git clone --quiet --shared --no-checkout $repository $fixture
    if ($LASTEXITCODE -ne 0) { throw 'Cannot create the isolated rule provenance fixture.' }
    [void][IO.Directory]::CreateDirectory((Join-Path $fixture 'tests/ci'))
    [void][IO.Directory]::CreateDirectory((Join-Path $fixture '.agents'))
    Copy-Item -LiteralPath (Join-Path $repository '.agents/rulebook') -Destination (Join-Path $fixture '.agents/rulebook') -Recurse
    Copy-Item -LiteralPath (Join-Path $repository 'AGENTS.md') -Destination $fixture
    Copy-Item -LiteralPath $entry -Destination (Join-Path $fixture 'tests/ci/Test-AgentRules.ps1')
    $fixtureEntry = Join-Path $fixture 'tests/ci/Test-AgentRules.ps1'
    $registry = Join-Path $fixture '.agents/rulebook/coverage.json'
    $originalRegistry = [IO.File]::ReadAllText($registry)
    foreach ($scenario in @('cycle', 'missing-section', 'unregistered-section')) {
        [IO.File]::WriteAllText($registry, $originalRegistry)
        $copy = $originalRegistry | ConvertFrom-Json
        $unitPath = Join-Path $fixture $copy.units[0].path
        $originalUnit = [IO.File]::ReadAllText($unitPath)
        $expected = switch ($scenario) {
            'cycle' {
                $copy.units[0].requires = @($copy.units[0].id)
                [IO.File]::WriteAllText($registry, ($copy | ConvertTo-Json -Depth 50))
                'Cyclic rule dependency'
            }
            'missing-section' {
                [IO.File]::WriteAllText($unitPath, $originalUnit.Replace('<!-- rule-unit: ' + $copy.units[0].id + ' -->', '<!-- missing -->'))
                'Missing or repeated complete rule unit'
            }
            'unregistered-section' {
                [IO.File]::WriteAllText($unitPath, $originalUnit + "`n<!-- rule-unit: unregistered -->`n")
                'Unregistered rule unit'
            }
        }
        $failure = ''
        try { & $fixtureEntry | Out-Null } catch { $failure = $_.Exception.Message }
        [IO.File]::WriteAllText($unitPath, $originalUnit)
        if (-not $failure.Contains($expected)) { throw "Wrong rejection for $scenario : $failure" }
    }
} finally {
    $resolved = [IO.Path]::GetFullPath($fixture)
    if (-not $resolved.StartsWith($fixtureParent, [StringComparison]::Ordinal) -or (Split-Path $resolved -Leaf) -notmatch '^InnoRuleTests-[a-f0-9]{32}$') {
        throw 'Rule fixture cleanup escaped its unique owner.'
    }
    if (Test-Path -LiteralPath $resolved) { Remove-Item -LiteralPath $resolved -Recurse -Force }
}
Write-Output 'Agent rule coverage, actual loading, root/nested routing, staged budgets, invalid selections, missing units and cyclic dependencies passed.'
