param(
    [string[]]$Paths = @(),
    [string[]]$Rules = @(),
    [string[]]$Behaviors = @(),
    [switch]$ShowInstructions,
    [int]$Stage = 0
)
$ErrorActionPreference = 'Stop'
$OutputEncoding = [Text.UTF8Encoding]::new($false)
[Console]::OutputEncoding = $OutputEncoding
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$ruleRoot = Join-Path $repository '.agents/rulebook'
$manifest = Get-Content -LiteralPath (Join-Path $ruleRoot 'coverage.json') -Raw | ConvertFrom-Json
$rootText = Get-Content -LiteralPath (Join-Path $repository 'AGENTS.md') -Raw
$rootBytes = [Text.Encoding]::UTF8.GetByteCount($rootText)
if ($rootBytes -gt $manifest.rootBudgetBytes) { throw 'Root instructions exceed 8 KiB.' }
$sources = @{}
function Read-Source([string]$identity) {
    if (-not $sources.ContainsKey($identity)) {
        $sources[$identity] = (& git -C $repository show $identity) -join "`n"
        if ($LASTEXITCODE -ne 0) { throw "Unavailable rule source: $identity" }
    }
    return $sources[$identity]
}
function Get-Hash([string]$value) {
    return [Convert]::ToHexStringLower([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($value)))
}
function Normalize-Links([string]$value) {
    # Relocated link destinations do not change their labels or surrounding requirements.
    return [regex]::Replace($value.Replace("`r`n", "`n").Trim(), '(\]\()[^)]+(\))', '$1<relocated>$2')
}
function Get-RuleLines([string]$value) {
    # Every nonblank line participates, including headings, examples and nested requirements.
    return @($value.Replace("`r`n", "`n").Trim() -split "`n" | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
}
function Normalize-RuleLine([string]$value) {
    # Indentation expresses nested Markdown obligations and code examples; preserve it.
    return [regex]::Replace($value, '(\]\()[^)]+(\))', '$1<relocated>$2')
}
function Test-Translation($unit, [string]$source, [string]$target) {
    $translation = $unit.translation
    if ($translation.language -cne 'en' -or [string]::IsNullOrWhiteSpace($translation.approval) -or
        [string]::IsNullOrWhiteSpace($translation.review)) { throw "Incomplete translation review: $($unit.id)" }
    $reviewRoot = [IO.Path]::GetFullPath('.agents/records/', $repository)
    $reviewPath = [IO.Path]::GetFullPath(($translation.review -split '#')[0], $repository)
    if (-not $reviewPath.StartsWith($reviewRoot, [StringComparison]::Ordinal) -or
        -not (Test-Path -LiteralPath $reviewPath -PathType Leaf)) { throw "Missing translation review: $($unit.id)" }
    $sourceLines = @(Get-RuleLines $source)
    $targetLines = @(Get-RuleLines $target)
    if ($translation.lines.Count -ne $sourceLines.Count -or $targetLines.Count -ne $sourceLines.Count) {
        throw "Incomplete translation coverage: $($unit.id)"
    }
    for ($index = 0; $index -lt $sourceLines.Count; $index++) {
        $line = $translation.lines[$index]
        if ($line.ordinal -ne $index + 1 -or $line.sourceHash -cne (Get-Hash (Normalize-RuleLine $sourceLines[$index])) -or
            $line.targetHash -cne (Get-Hash (Normalize-RuleLine $targetLines[$index]))) {
            throw "Changed or unmapped translation clause: $($unit.id) line $($index + 1)"
        }
    }
}
$units = @{}; $bodies = @{}
foreach ($unit in $manifest.units) {
    if ($units.ContainsKey($unit.id)) { throw "Duplicate rule unit: $($unit.id)" }
    $destination = [IO.Path]::GetFullPath($unit.path, $repository)
    if (-not $destination.StartsWith($ruleRoot + [IO.Path]::DirectorySeparatorChar, [StringComparison]::Ordinal)) { throw "Rule path escapes owner: $($unit.id)" }
    $text = Get-Content -LiteralPath $destination -Raw
    $pattern = '(?s)<!-- rule-unit: ' + [regex]::Escape($unit.id) + ' -->\s*<a id="[^"\r\n]+"></a>\s*(.*?)\s*<!-- /rule-unit: ' + [regex]::Escape($unit.id) + ' -->'
    $matches = [regex]::Matches($text, $pattern)
    if ($matches.Count -ne 1) { throw "Missing or repeated complete rule unit: $($unit.id)" }
    $body = $matches[0].Groups[1].Value
    $source = Read-Source $unit.source
    if ($unit.PSObject.Properties.Name -contains 'sourceSection') { $source = [regex]::Split($source, '(?m)(?=^## )')[$unit.sourceSection] }
    if ((Get-Hash $source.Trim()) -cne $unit.sourceHash) { throw "Changed source identity: $($unit.id)" }
    foreach ($replacement in $manifest.pathReplacements) { $source = $source.Replace($replacement.old, $replacement.current) }
    $preservedBody = $body
    if ($unit.PSObject.Properties.Name -contains 'approvedAddition') {
        $parts = [regex]::Split($body, '(?m)^<!-- approved-addition -->\r?$')
        if ($parts.Count -ne 2 -or [string]::IsNullOrWhiteSpace($parts[1]) -or
            [string]::IsNullOrWhiteSpace($unit.approvedAddition.approval)) {
            throw "Invalid approved rule addition: $($unit.id)"
        }
        if ($unit.approvedAddition.PSObject.Properties.Name -contains 'translation') {
            $additionOriginal = Read-Source $unit.approvedAddition.translation.source
            $additionOriginal = [regex]::Match($additionOriginal, $pattern).Groups[1].Value
            $additionParts = [regex]::Split($additionOriginal, '(?m)^<!-- approved-addition -->\r?$')
            if ($additionParts.Count -ne 2 -or (Get-Hash $additionParts[1].Replace("`r`n", "`n").Trim()) -cne $unit.approvedAddition.hash) {
                throw "Invalid approved addition source: $($unit.id)"
            }
            Test-Translation @{ id = $unit.id + '/addition'; translation = $unit.approvedAddition.translation } $additionParts[1] $parts[1]
        } elseif ((Get-Hash $parts[1].Replace("`r`n", "`n").Trim()) -cne $unit.approvedAddition.hash) {
            throw "Invalid approved rule addition: $($unit.id)"
        }
        $preservedBody = $parts[0]
    }
    if ($unit.PSObject.Properties.Name -contains 'translation') {
        Test-Translation $unit $source $preservedBody
    } elseif ((Normalize-Links $preservedBody) -cne (Normalize-Links $source)) { throw "Lost nested requirements, examples or prose: $($unit.id)" }
    $units[$unit.id] = $unit; $bodies[$unit.id] = $body
}
$original = Read-Source "$($manifest.baseline):AGENTS.md"
foreach ($file in Get-ChildItem -LiteralPath $ruleRoot -Filter '*.md' -Recurse) {
    if ($file.Name -cne 'README.md' -and -not @($manifest.units | Where-Object { [IO.Path]::GetFullPath($_.path, $repository) -ceq $file.FullName }).Count) {
        throw "Unregistered rule file: $($file.FullName)"
    }
    foreach ($marker in [regex]::Matches((Get-Content -LiteralPath $file.FullName -Raw), '<!-- rule-unit: ([^ ]+) -->')) {
        $id = $marker.Groups[1].Value
        if (-not $units.ContainsKey($id) -or [IO.Path]::GetFullPath($units[$id].path, $repository) -cne $file.FullName) {
            throw "Unregistered rule unit or authority: $id in $($file.FullName)"
        }
    }
}
$count = 0
foreach ($section in [regex]::Matches($original, '(?ms)^## (\d+)\. .*?(?=^## \d+\. |\z)')) {
    $number = [int]$section.Groups[1].Value; $ordinal = 0
    foreach ($bullet in [regex]::Matches($section.Value, '(?m)^- .*')) {
        $ordinal++; $id = "$number.$ordinal"
        $rows = @($manifest.rules | Where-Object id -CEQ $id)
        if ($rows.Count -ne 1) { throw "Missing or duplicate rule mapping: $id" }
        $row = $rows[0]
        if ($row.originalHash -cne (Get-Hash $bullet.Value) -or -not $row.check -or -not $row.reviewer) { throw "Invalid provenance or responsibility: $id" }
        if (-not $units.ContainsKey($row.unit) -or $units[$row.unit].path -cne $row.path) { throw "Rule has no unique authority: $id" }
        $expected = $bullet.Value
        foreach ($replacement in $manifest.pathReplacements) { $expected = $expected.Replace($replacement.old, $replacement.current) }
        if ($row.disposition -eq 'preserved') {
            if ($units[$row.unit].PSObject.Properties.Name -contains 'translation') {
                $mapping = @($units[$row.unit].translation.lines | Where-Object sourceHash -CEQ (Get-Hash (Normalize-RuleLine $expected)))
                if ($mapping.Count -ne 1 -or $row.translatedHash -cne $mapping[0].targetHash) { throw "Rule translation was lost: $id" }
            } elseif (-not (Normalize-Links $bodies[$row.unit]).Contains((Normalize-Links $expected), [StringComparison]::Ordinal)) { throw "Rule was lost: $id" }
        } elseif ($number -ne 8 -or $row.disposition -ne 'superseded-by-approved-plan') { throw "Unapproved replacement: $id" }
        $count++
    }
}
if ($count -ne $manifest.rules.Count) { throw 'Unknown baseline rule mappings.' }
$selected = @{}
$active = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$checked = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
function Check-Dependencies([string]$id) {
    if (-not $units.ContainsKey($id)) { throw "Unknown rule: $id" }
    if ($checked.Contains($id)) { return }
    if (-not $active.Add($id)) { throw "Cyclic rule dependency: $id" }
    foreach ($dependency in $units[$id].requires) { Check-Dependencies $dependency }
    [void]$active.Remove($id); [void]$checked.Add($id)
}
foreach ($id in $units.Keys) { Check-Dependencies $id }
foreach ($route in $manifest.routes) {
    [void][regex]::new($route.pattern)
    foreach ($id in $route.units) {
        if (-not $units.ContainsKey($id)) { throw "Unknown routed rule: $id" }
    }
}
function Select-Unit([string]$id, [string]$reason) {
    if (-not $units.ContainsKey($id)) { throw "Unknown rule: $id" }
    if ($selected.ContainsKey($id)) { $selected[$id].Add($reason); return }
    $selected[$id] = [Collections.Generic.List[string]]::new(); $selected[$id].Add($reason)
    foreach ($dependency in $units[$id].requires) { Select-Unit $dependency "required by $id" }
}
foreach ($id in $Rules) { Select-Unit $id 'explicit rule' }
foreach ($tag in $Behaviors) {
    $matches = @($manifest.units | Where-Object { $tag -cin $_.tags })
    if ($matches.Count -eq 0) { throw "Unknown behavior: $tag" }
    foreach ($unit in $matches) { Select-Unit $unit.id "behavior: $tag" }
}
foreach ($path in $Paths) {
    $normalized = $path.Replace('\', '/')
    if ([IO.Path]::IsPathRooted($path)) { $normalized = [IO.Path]::GetRelativePath($repository, $path).Replace('\', '/') }
    foreach ($route in $manifest.routes) {
        if ($normalized -match $route.pattern) { foreach ($id in $route.units) { Select-Unit $id "path: $normalized" } }
    }
}
foreach ($file in @((Join-Path $repository 'AGENTS.md')) + @(Get-ChildItem -LiteralPath $ruleRoot -Filter '*.md' -Recurse | ForEach-Object FullName)) {
    foreach ($link in [regex]::Matches((Get-Content -LiteralPath $file -Raw), '\]\(([^)#]+)(?:#[^)]*)?\)')) {
        $target = $link.Groups[1].Value
        if ($target -match '^[a-z]+:') { continue }
        if (-not (Test-Path -LiteralPath ([IO.Path]::GetFullPath($target, (Split-Path $file))))) { throw "Broken instruction link: $file -> $target" }
    }
}
$stages = [Collections.Generic.List[object]]::new()
$current = [Collections.Generic.List[string]]::new(); $bytes = $rootBytes
foreach ($id in ($selected.Keys | Sort-Object)) {
    $size = [Text.Encoding]::UTF8.GetByteCount($bodies[$id])
    if ($size + $rootBytes -gt $manifest.taskBudgetBytes) { throw "Unit $id needs smaller complete subchapters; truncation is forbidden." }
    if ($bytes + $size -gt $manifest.taskBudgetBytes) {
        $stages.Add(@{ ids = $current.ToArray(); bytes = $bytes }); $current.Clear(); $bytes = $rootBytes
    }
    $current.Add($id); $bytes += $size
}
$stages.Add(@{ ids = $current.ToArray(); bytes = $bytes })
if ($Stage -lt 0 -or $Stage -gt $stages.Count) { throw 'Invalid instruction stage.' }
Write-Output "Preserved mapping: $count rules; $($units.Count) complete units. Root: $rootBytes bytes. Stages: $($stages.Count)."
for ($index = 0; $index -lt $stages.Count; $index++) {
    $entry = $stages[$index]
    Write-Output "Stage $($index + 1): $($entry.bytes) bytes; units: $($entry.ids -join ', ')"
    foreach ($id in $entry.ids) { Write-Output "$id | $($units[$id].path)#$id | source: $($units[$id].source) | $($selected[$id] -join '; ')" }
    if ($ShowInstructions -and ($Stage -eq 0 -or $Stage -eq $index + 1)) {
        Write-Output $rootText
        foreach ($id in $entry.ids) { Write-Output $bodies[$id] }
    }
}
