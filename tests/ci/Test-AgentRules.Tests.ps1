param([string]$DocsRoot)
$ErrorActionPreference = 'Stop'
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$entry = Join-Path $PSScriptRoot '../../tools/ci/Test-AgentRules.ps1'

function Assert-Rejected([string]$message, [scriptblock]$action) {
    $failure = $null
    try { & $action | Out-Null } catch { $failure = $_.Exception.Message }
    if ($null -eq $failure -or -not $failure.Contains($message)) { throw "Expected a rejection containing '$message' but got: $failure" }
}
function Assert-Selection([string]$output, [string[]]$includes, [string[]]$excludes = @()) {
    foreach ($file in $includes) { if (-not $output.Contains($file + ' |')) { throw "Expected rule file $file in: $output" } }
    foreach ($file in $excludes) { if ($output.Contains($file + ' |')) { throw "Unexpected rule file $file in: $output" } }
}

# The repository rules load identically from any working directory and in both list forms.
$root = @(& $entry -Paths 'src/services/input/Example.cs' -Rules validation) -join "`n"
Push-Location (Join-Path $repository 'tools')
try { $nested = @(& $entry -Paths 'src/services/input/Example.cs' -Rules validation) -join "`n" }
finally { Pop-Location }
if ($root -cne $nested) { throw 'Root and nested invocation selected different rule files.' }
Assert-Selection $root @('.agents/rulebook/coding.md', '.agents/rulebook/validation.md', '.agents/rulebook/csharp-style/README.md')
$joined = @(& $entry -Paths 'Directory.Build.targets,Directory.Solution.targets') -join "`n"
if ($joined -cne (@(& $entry -Paths 'Directory.Build.targets', 'Directory.Solution.targets') -join "`n")) { throw 'Comma-joined and array selections differ.' }
Assert-Selection $joined @('.agents/rulebook/projects.md', '.agents/rulebook/domains/build-native/hosts.md',
    '.agents/rulebook/domains/build-native/platform-products.md', '.agents/rulebook/domains/build-native/backend-integration.md')
Assert-Selection (@(& $entry -Paths (Join-Path $repository 'AGENTS.md')) -join "`n") @('.agents/rulebook/validation.md')
Assert-Rejected 'Unknown rule: unknown-rule' { & $entry -Rules unknown-rule }
Assert-Rejected 'Unknown tag: unknown-tag' { & $entry -Tags unknown-tag }
Assert-Rejected 'Path is outside the repository' { & $entry -Paths '../outside.cs' }
if ($DocsRoot) { Assert-Selection (@(& $entry -DocsRoot $DocsRoot -Tags documentation) -join "`n") @('.agents/rulebook/documentation.md') }

$fixture = Join-Path ([IO.Path]::GetFullPath([IO.Path]::GetTempPath())) ('InnoRuleTests-' + [guid]::NewGuid().ToString('N'))
try {
    $rules = Join-Path $fixture '.agents/rulebook'
    $docs = Join-Path $fixture 'docs-checkout'
    foreach ($directory in @((Join-Path $fixture 'tools/ci'), (Join-Path $rules 'domains'), (Join-Path $docs '.agents/rulebook'))) {
        [void][IO.Directory]::CreateDirectory($directory)
    }
    $fixtureEntry = Join-Path $fixture 'tools/ci/Test-AgentRules.ps1'
    $examples = Join-Path $fixture 'tools/ci/Test-AgentRules.examples.json'
    Copy-Item -LiteralPath $entry -Destination $fixtureEntry
    function Write-Fixture([string]$path, [string]$text) { [IO.File]::WriteAllText((Join-Path $fixture $path), $text) }
    Write-Fixture 'AGENTS.md' "# Entry`n"
    Write-Fixture '.agents/rulebook/README.md' "# Index without front matter`n"
    Write-Fixture '.agents/rulebook/coding.md' "---`npaths: [`"**/*.cs`", `"src/*.txt`"]`ntags: [coding, shared]`n---`n# Coding`n"
    Write-Fixture '.agents/rulebook/domains/editor.md' "---`npaths: [`"src/editor/**`"]`n---`n# Editor`n"
    Write-Fixture '.agents/rulebook/delivery.md' "---`ntags: [delivery, shared]`n---`n# Delivery`n"
    Write-Fixture 'docs-checkout/.agents/rulebook/README.md' "# Docs index`n"
    Write-Fixture 'docs-checkout/.agents/rulebook/writing.md' "---`npaths: [`"content/**`"]`ntags: [writing]`n---`n# Writing`n"
    Write-Fixture 'tools/ci/Test-AgentRules.examples.json' '[{ "path": "src/editor/State.cs", "includes": ["coding", "domains/editor"], "excludes": ["delivery"] }]'

    # Globs match the whole path, `*` stays inside one name and matching is case-sensitive.
    $valid = @(& $fixtureEntry) -join "`n"
    if (-not $valid.Contains('Rules are valid: 3 files.')) { throw "Unexpected fixture summary: $valid" }
    Assert-Selection (@(& $fixtureEntry -Paths 'Root.cs') -join "`n") @('.agents/rulebook/coding.md') @('.agents/rulebook/domains/editor.md')
    Assert-Selection (@(& $fixtureEntry -Paths 'src/editor/panels/Panel.cs') -join "`n") @('.agents/rulebook/coding.md', '.agents/rulebook/domains/editor.md')
    Assert-Selection (@(& $fixtureEntry -Paths 'src/note.txt') -join "`n") @('.agents/rulebook/coding.md')
    foreach ($unmatched in @('src/nested/note.txt', 'src/Editor/note.md', 'tools/editor/note.md', 'Root.CS')) {
        if ((@(& $fixtureEntry -Paths $unmatched) -join "`n").Contains(' | ')) { throw "Path must not select a rule: $unmatched" }
    }
    Assert-Selection (@(& $fixtureEntry -Tags shared) -join "`n") @('.agents/rulebook/coding.md', '.agents/rulebook/delivery.md') @('.agents/rulebook/domains/editor.md')
    Assert-Selection (@(& $fixtureEntry -Rules 'domains/editor') -join "`n") @('.agents/rulebook/domains/editor.md')

    # An explicit Docs checkout adds its own rule files; its paths never select engine rules.
    Assert-Rejected 'Unknown tag: writing' { & $fixtureEntry -Tags writing }
    $writing = Join-Path $docs '.agents/rulebook/writing.md'
    Assert-Selection (@(& $fixtureEntry -DocsRoot $docs -Tags writing) -join "`n") @($writing)
    Assert-Selection (@(& $fixtureEntry -DocsRoot $docs -Rules 'docs/writing') -join "`n") @($writing)
    Assert-Selection (@(& $fixtureEntry -DocsRoot $docs -Paths (Join-Path $docs 'content/Guide.cs')) -join "`n") @($writing) @('.agents/rulebook/coding.md')

    $cases = @(
        @{ message = 'needs front matter'; path = '.agents/rulebook/broken.md'; text = "# No front matter`n" },
        @{ message = 'needs front matter'; path = '.agents/rulebook/broken.md'; text = "---`ntags:`n  - block-list`n---`n# Rule`n" },
        @{ message = 'needs front matter'; path = '.agents/rulebook/broken.md'; text = "---`n---`n# Rule`n" },
        @{ message = 'Unknown or repeated front matter field'; path = '.agents/rulebook/broken.md'; text = "---`nrequires: [coding]`n---`n# Rule`n" },
        @{ message = 'Unknown or repeated front matter field'; path = '.agents/rulebook/broken.md'; text = "---`ntags: [one]`ntags: [two]`n---`n# Rule`n" },
        @{ message = 'Empty front matter value'; path = '.agents/rulebook/broken.md'; text = "---`ntags: []`n---`n# Rule`n" },
        @{ message = 'Invalid rule tag'; path = '.agents/rulebook/broken.md'; text = "---`ntags: [Not Valid]`n---`n# Rule`n" },
        @{ message = 'not a repository-relative glob'; path = '.agents/rulebook/broken.md'; text = "---`npaths: [`"/src/**`"]`n---`n# Rule`n" },
        @{ message = 'not a repository-relative glob'; path = '.agents/rulebook/broken.md'; text = "---`npaths: [`"src/../**`"]`n---`n# Rule`n" },
        @{ message = 'exceeds 16384 bytes'; path = '.agents/rulebook/broken.md'; text = "---`ntags: [large]`n---`n" + ('x' * 16384) },
        @{ message = 'Root instructions exceed 8192 bytes'; path = 'AGENTS.md'; text = 'x' * 8193 },
        @{ message = 'must select delivery'; path = 'tools/ci/Test-AgentRules.examples.json'; text = '[{ "path": "src/a.cs", "includes": ["delivery"] }]' },
        @{ message = 'must not select coding'; path = 'tools/ci/Test-AgentRules.examples.json'; text = '[{ "path": "src/a.cs", "excludes": ["coding"] }]' },
        @{ message = 'names an unknown rule: retired'; path = 'tools/ci/Test-AgentRules.examples.json'; text = '[{ "path": "src/a.cs", "excludes": ["retired"] }]' }
    )
    foreach ($case in $cases) {
        $target = Join-Path $fixture $case.path
        $original = if (Test-Path -LiteralPath $target) { [IO.File]::ReadAllText($target) } else { $null }
        try {
            [IO.File]::WriteAllText($target, $case.text)
            Assert-Rejected $case.message { & $fixtureEntry }
        } finally {
            if ($null -eq $original) { Remove-Item -LiteralPath $target } else { [IO.File]::WriteAllText($target, $original) }
        }
    }
    & $fixtureEntry | Out-Null
} finally {
    if (Test-Path -LiteralPath $fixture) { Remove-Item -LiteralPath $fixture -Recurse -Force }
}
Write-Output 'Agent rule loading, front matter, glob routing, size limits, invalid selections, routing examples and explicit Docs checkouts passed.'
