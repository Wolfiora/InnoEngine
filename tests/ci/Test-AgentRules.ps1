param(
    [string[]]$Paths = @(),
    [string[]]$Rules = @(),
    [switch]$ShowInstructions
)

$ErrorActionPreference = 'Stop'
$OutputEncoding = [Text.UTF8Encoding]::new($false)
[Console]::OutputEncoding = $OutputEncoding
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$ruleRoot = Join-Path $repository 'docs/architecture/rules'
$manifest = Get-Content -LiteralPath (Join-Path $ruleRoot 'coverage.json') -Raw | ConvertFrom-Json
$rootFile = Join-Path $repository 'AGENTS.md'
if ((Get-Item -LiteralPath $rootFile).Length -gt $manifest.rootBudgetBytes) {
    throw 'Root instructions exceed the 8 KiB discovery budget.'
}
$original = (& git -C $repository show "$($manifest.baseline):AGENTS.md") -join "`n"
if ($LASTEXITCODE -ne 0) { throw 'The rule preservation baseline is unavailable.' }
$ids = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$count = 0
foreach ($section in [regex]::Matches($original, '(?ms)^## (\d+)\. .*?(?=^## \d+\. |\z)')) {
    $number = [int]$section.Groups[1].Value
    if ($number -ne 8) {
        $destinations = @($manifest.rules | Where-Object { $_.id.StartsWith("$number.", [StringComparison]::Ordinal) } |
            Select-Object -ExpandProperty path -Unique)
        if ($destinations.Count -ne 1) { throw "The original section must have one authoritative owner: $number" }
        $sectionBody = (Get-Content -LiteralPath (Join-Path $repository $destinations[0]) -Raw).Replace("`r`n", "`n").Trim()
        if ($sectionBody -cne $section.Value.Trim()) {
            throw "Original section $number lost or changed nested requirements, examples, or explanatory text."
        }
    }
    $ordinal = 0
    foreach ($bullet in [regex]::Matches($section.Value, '(?m)^- .*')) {
        $ordinal++
        $id = "$number.$ordinal"
        $rows = @($manifest.rules | Where-Object id -CEQ $id)
        if ($rows.Count -ne 1) { throw "Missing or duplicate rule mapping: $id" }
        $row = $rows[0]
        $hash = [Convert]::ToHexStringLower([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($bullet.Value)))
        if ($row.originalHash -cne $hash -or -not $row.check -or -not $row.reviewer) {
            throw "Invalid rule provenance or review responsibility: $id"
        }
        $destination = [IO.Path]::GetFullPath($row.path, $repository)
        if (-not $destination.StartsWith($ruleRoot + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
            throw "Rule path escapes its owner: $id"
        }
        $body = Get-Content -LiteralPath $destination -Raw
        if ($row.disposition -eq 'preserved') {
            if (-not $body.Contains($bullet.Value, [StringComparison]::Ordinal)) { throw "Rule was lost: $id" }
        } elseif ($number -ne 8 -or $row.disposition -ne 'superseded-by-approved-plan') {
            throw "Unapproved rule replacement: $id"
        }
        [void]$ids.Add($id)
        $count++
    }
}
if ($count -ne $manifest.rules.Count) { throw 'Coverage contains unknown baseline rules.' }
$canonical = @($manifest.rules.path | Sort-Object -Unique)
foreach ($file in Get-ChildItem -LiteralPath $ruleRoot -Filter '*.md') {
    $relative = [IO.Path]::GetRelativePath($repository, $file.FullName).Replace('\', '/')
    if ($file.Name -ne 'README.md' -and $relative -cnotin $canonical) { throw "Unregistered canonical rule file: $relative" }
}

$selected = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
foreach ($rule in $Rules) { [void]$selected.Add($rule) }
foreach ($path in $Paths) {
    $path = $path.Replace('\', '/')
    $candidates = @()
    if ($path -match '\.cs$') { $candidates += 'naming','members','xml','errors','formatting' }
    if ($path -match '(\.csproj$|\.sln$)') { $candidates += 'solution','directories' }
    if ($path -match '(^tests/|/tests/)') { $candidates += 'tests','compilation' }
    if ($path -match '^docs/') { $candidates += 'wiki' }
    if ($path -match '(scripting|ScriptingApi)') { $candidates += 'scripting','identity-reload' }
    if ($path -match '(plugins|identity|scene|assets)') { $candidates += 'identity-reload' }
    if ($path -match 'plugins') { $candidates += 'plugins','no-legacy' }
    if ($path -match 'composition/editor') { $candidates += 'editor-projects' }
    if ($path -match '(editor/.+(presentation|panels)|ImGui)') { $candidates += 'editor-layout','editor-acceptance' }
    if ($path -match '(History|Workspace|State)') { $candidates += 'editor-state' }
    if ($path -match '(rendering|Rendering)') { $candidates += 'rendering','content-frame' }
    if ($path -match '^(build/|backends/|platforms/|\.github/)') { $candidates += 'build-host','platform-products','backend-build' }
    foreach ($rule in $candidates) { [void]$selected.Add($rule) }
}

$files = @($rootFile)
foreach ($rule in ($selected | Sort-Object)) {
    if ($rule -notmatch '^[a-z-]+$') { throw "Invalid rule name: $rule" }
    $file = Join-Path $ruleRoot "$rule.md"
    if (-not (Test-Path -LiteralPath $file -PathType Leaf)) { throw "Unknown rule: $rule" }
    $files += $file
}
foreach ($file in @($rootFile) + @(Get-ChildItem -LiteralPath $ruleRoot -Filter '*.md' | ForEach-Object FullName)) {
    foreach ($link in [regex]::Matches((Get-Content -LiteralPath $file -Raw), '\]\(([^)#]+)(?:#[^)]*)?\)')) {
        $target = $link.Groups[1].Value
        if ($target -match '^https?://') { continue }
        if (-not (Test-Path -LiteralPath ([IO.Path]::GetFullPath($target, (Split-Path $file))))) {
            throw "Broken instruction link in ${file}: $target"
        }
    }
}
$bytes = ($files | ForEach-Object { (Get-Item -LiteralPath $_).Length } | Measure-Object -Sum).Sum
Write-Output "Preserved mapping: $count rules. Root: $((Get-Item $rootFile).Length) bytes. Selected: $bytes bytes."
$files | ForEach-Object { Write-Output ([IO.Path]::GetRelativePath($repository, $_)) }
if ($ShowInstructions) {
    foreach ($file in $files) { Get-Content -LiteralPath $file -Raw | Write-Output }
}
if ($bytes -gt $manifest.taskBudgetBytes) {
    throw 'Selected instructions exceed 16 KiB. Split work by owner and read each applicable domain in full; do not truncate rules.'
}
