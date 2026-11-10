$ErrorActionPreference = 'Stop'
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$entry = Join-Path $PSScriptRoot 'Test-AgentRules.ps1'
$root = @(& $entry -Paths 'src/services/input/Example.cs' -Rules completion) -join "`n"
Push-Location (Join-Path $repository 'tools')
try { $nested = @(& $entry -Paths 'src/services/input/Example.cs' -Rules completion) -join "`n" }
finally { Pop-Location }
if ($root -cne $nested) { throw 'Root and nested invocation selected different instruction units.' }
if ($root -notmatch 'Preserved mapping: 241 rules') { throw 'Original rule coverage is incomplete.' }
foreach ($path in @('Directory.Build.targets', 'Directory.Solution.targets')) {
    $selection = @(& $entry -Paths $path) -join "`n"
    foreach ($required in @('build-host', 'platform-products', 'backend-build', 'solution')) {
        if (-not $selection.Contains($required)) { throw "Shared build input $path did not route to $required." }
    }
}
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
if (-not $first.Contains('# InnoEngine agent instructions') -or -not $first.Contains('Define clear assembly/domain boundaries')) { throw 'Actual instruction bodies were not emitted.' }
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
    foreach ($review in @($manifest.units | Where-Object translation | ForEach-Object { ($_.translation.review -split '#')[0] } | Sort-Object -Unique)) {
        $reviewTarget = Join-Path $fixture $review
        [void][IO.Directory]::CreateDirectory((Split-Path $reviewTarget))
        Copy-Item -LiteralPath (Join-Path $repository $review) -Destination $reviewTarget
    }
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
    [IO.File]::WriteAllText($registry, $originalRegistry)
    $translationUnit = ($originalRegistry | ConvertFrom-Json).units | Where-Object id -CEQ 'delivery'
    $translationPath = Join-Path $fixture $translationUnit.path
    $originalTranslation = [IO.File]::ReadAllText($translationPath)
    foreach ($scenario in @('changed-translation', 'missing-clause', 'changed-source-map', 'missing-review', 'escaped-review', 'lost-original-rule')) {
        $copy = $originalRegistry | ConvertFrom-Json
        $translated = $copy.units | Where-Object id -CEQ 'delivery'
        [IO.File]::WriteAllText($translationPath, $originalTranslation)
        $expected = switch ($scenario) {
            'changed-translation' {
                [IO.File]::WriteAllText($translationPath, $originalTranslation.Replace('must be entirely in English', 'may be in any language'))
                'Changed or unmapped translation clause'
            }
            'missing-clause' {
                $translated.translation.lines = @($translated.translation.lines | Select-Object -Skip 1)
                'Incomplete translation coverage'
            }
            'changed-source-map' {
                $translated.translation.lines[0].sourceHash = 'invalid'
                'Changed or unmapped translation clause'
            }
            'missing-review' {
                $translated.translation.review = '.agents/records/missing.md'
                'Missing translation review'
            }
            'escaped-review' {
                $translated.translation.review = '.agents/records/../../AGENTS.md'
                'Missing translation review'
            }
            'lost-original-rule' {
                ($copy.rules | Where-Object id -CEQ '31.1').translatedHash = 'invalid'
                'Rule translation was lost'
            }
        }
        [IO.File]::WriteAllText($registry, ($copy | ConvertTo-Json -Depth 50))
        $failure = ''
        try { & $fixtureEntry | Out-Null } catch { $failure = $_.Exception.Message }
        if (-not $failure.Contains($expected)) { throw "Wrong rejection for $scenario : $failure" }
    }
    [IO.File]::WriteAllText($translationPath, $originalTranslation)
    [IO.File]::WriteAllText($registry, $originalRegistry)
    $nestedRulePath = Join-Path $fixture '.agents/rulebook/coding.md'
    $nestedRuleText = [IO.File]::ReadAllText($nestedRulePath)
    [IO.File]::WriteAllText($nestedRulePath, $nestedRuleText.Replace('  - Types and members', '- Types and members'))
    $failure = ''
    try { & $fixtureEntry | Out-Null } catch { $failure = $_.Exception.Message }
    [IO.File]::WriteAllText($nestedRulePath, $nestedRuleText)
    if (-not $failure.Contains('Changed or unmapped translation clause')) { throw 'Nested rule indentation lost its provenance protection.' }
    $additionUnit = ($originalRegistry | ConvertFrom-Json).units | Where-Object id -CEQ 'csharp-standard-5'
    $additionPath = Join-Path $fixture $additionUnit.path
    $originalAddition = [IO.File]::ReadAllText($additionPath)
    $loaded = @(& $entry -Rules $additionUnit.id -ShowInstructions) -join "`n"
    if (-not $loaded.Contains('Handwritten C# must not use `global::`')) { throw 'Approved addition was not loaded.' }
    foreach ($scenario in @('changed-addition', 'missing-addition', 'missing-approval', 'changed-baseline')) {
        [IO.File]::WriteAllText($registry, $originalRegistry)
        [IO.File]::WriteAllText($additionPath, $originalAddition)
        $expected = 'Invalid approved rule addition'
        switch ($scenario) {
            'changed-addition' {
                [IO.File]::WriteAllText($additionPath, $originalAddition.Replace('Handwritten C# must not use', 'Handwritten C# may use'))
                $expected = 'Changed or unmapped translation clause'
            }
            'missing-addition' {
                [IO.File]::WriteAllText($additionPath, $originalAddition.Replace('<!-- approved-addition -->', ''))
            }
            'missing-approval' {
                $copy = $originalRegistry | ConvertFrom-Json
                ($copy.units | Where-Object id -CEQ $additionUnit.id).approvedAddition.approval = ''
                [IO.File]::WriteAllText($registry, ($copy | ConvertTo-Json -Depth 50))
            }
            'changed-baseline' {
                [IO.File]::WriteAllText($additionPath, $originalAddition.Replace('- Explicitly declare ordinary using directives in every file.', ''))
                $expected = 'Incomplete translation coverage'
            }
        }
        $failure = ''
        try { & $fixtureEntry | Out-Null } catch { $failure = $_.Exception.Message }
        if (-not $failure.Contains($expected)) { throw "Wrong rejection for $scenario : $failure" }
    }
} finally {
    $resolved = [IO.Path]::GetFullPath($fixture)
    if (-not $resolved.StartsWith($fixtureParent, [StringComparison]::Ordinal) -or (Split-Path $resolved -Leaf) -notmatch '^InnoRuleTests-[a-f0-9]{32}$') {
        throw 'Rule fixture cleanup escaped its unique owner.'
    }
    if (Test-Path -LiteralPath $resolved) { Remove-Item -LiteralPath $resolved -Recurse -Force }
}
Write-Output 'Agent rule coverage, actual loading, root/nested routing, staged budgets, invalid selections, missing units, cyclic dependencies, approved additions and English translation provenance passed.'
