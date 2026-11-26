param(
    [string] $Dotnet,
    [string] $BuildCli
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$engineRoot = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$root = Join-Path $engineRoot ('artifacts/validation/api-publication/' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $root | Out-Null
if (!$Dotnet) { $Dotnet = (Get-Command dotnet -CommandType Application -ErrorAction Stop | Select-Object -First 1).Source }
if (!$BuildCli) {
    $BuildCli = & "$PSScriptRoot/../../tools/ci/Initialize-ValidationEntry.ps1" -Dotnet $Dotnet -LogPath (Join-Path $root 'bootstrap.log') `
        -WorkspaceDirectory (Join-Path $root 'artifacts/bootstrap')
}
if (!(Test-Path -LiteralPath $BuildCli -PathType Leaf)) { throw 'The actual managed API transport is required.' }
$oldConfig = $env:GIT_CONFIG_GLOBAL
$oldPrompt = $env:GIT_TERMINAL_PROMPT
$success = $false
function Invoke-FixtureGit([string] $Directory, [string[]] $Arguments) {
    $result = @(& git -C $Directory @Arguments 2>&1)
    if ($LASTEXITCODE) { throw "Fixture git failure: $($result -join [Environment]::NewLine)" }
    return $result
}
function Write-Text([string] $Path, [string] $Text) {
    [IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($Path)) | Out-Null
    [IO.File]::WriteAllText($Path, $Text, [Text.UTF8Encoding]::new($false))
}
function Hash([string] $Text) { return [Convert]::ToHexStringLower([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($Text))) }
function Initialize([string] $Directory, [string] $Branch) {
    [IO.Directory]::CreateDirectory($Directory) | Out-Null
    [void](Invoke-FixtureGit $Directory @('init', '-b', $Branch))
    [void](Invoke-FixtureGit $Directory @('config', 'user.name', 'API publication fixture'))
    [void](Invoke-FixtureGit $Directory @('config', 'user.email', 'fixture@example.invalid'))
}
try {
    $env:GIT_TERMINAL_PROMPT = '0'
    $env:GIT_CONFIG_GLOBAL = Join-Path $root 'gitconfig'
    $engine = Join-Path $root 'engine'
    $docs = Join-Path $root 'docs'
    Initialize $engine 'master'
    Write-Text (Join-Path $engine 'input.txt') 'One exact input.'
    [void](Invoke-FixtureGit $engine @('add', '.'))
    [void](Invoke-FixtureGit $engine @('commit', '-m', 'fixture(engine): establish input'))
    $revision = (Invoke-FixtureGit $engine @('rev-parse', 'HEAD')) -join ''
    $tree = (Invoke-FixtureGit $engine @('rev-parse', 'HEAD^{tree}')) -join ''
    $engineRemote = Join-Path $root 'engine.git'
    $docsRemote = Join-Path $root 'docs.git'
    [void](Invoke-FixtureGit $root @('clone', '--bare', $engine, $engineRemote))
    [void](Invoke-FixtureGit $engine @('remote', 'add', 'origin', 'https://github.com/Wolfiora/InnoEngine.git'))
    Write-Text $env:GIT_CONFIG_GLOBAL ("[url `"$($engineRemote.Replace('\','/'))`"]`n`t insteadOf = https://github.com/Wolfiora/InnoEngine.git`n[url `"$($docsRemote.Replace('\','/'))`"]`n`t insteadOf = https://github.com/Wolfiora/InnoEngine.Docs.git`n")
    Initialize $docs 'main'
    Write-Text (Join-Path $docs '.gitattributes') "* text=auto eol=lf`n"
    Write-Text (Join-Path $docs '.gitignore') "artifacts/`n"
    Write-Text (Join-Path $docs 'content/en/manual/task.md') 'A completely independent guide.'
    [void](Invoke-FixtureGit $docs @('add', '.'))
    [void](Invoke-FixtureGit $docs @('commit', '-m', 'fixture(docs): establish manual'))
    $mainBefore = (Invoke-FixtureGit $docs @('rev-parse', 'HEAD')) -join ''
    [void](Invoke-FixtureGit $docs @('checkout', '-b', 'draft'))
    [void](Invoke-FixtureGit $root @('clone', '--bare', $docs, $docsRemote))
    $signature = 'public class Probe.Value'
    $facts = @{ model='source-clr'; configuration='Debug'; target=$null; projects=@(@{
        name='Probe'; project='src/Probe.csproj'; symbols=@(@{
            project='Probe'; id='T:Probe.Value'; kind='NamedType'; signature=$signature; signatureHash=(Hash $signature);
            source='src/Value.cs'; line=1; documentation='<member><summary>A source value.</summary></member>';
            containingType=$null; baseType=$null; interfaces=@()
        })
    }) } | ConvertTo-Json -Depth 20 -Compress
    $inventory = ConvertTo-Json -InputObject @(@{name='Probe';path='src/Probe.csproj';isTest=$false;surfaces=@('source-clr')}) -Depth 10 -Compress
    $capture = @{path='reference/api/probe.json';sha256=(Hash $facts);model='source-clr';configuration='Debug';target=$null;sourceRevision=$revision;sourceTree=$tree}
    $binding = @{repository='https://github.com/Wolfiora/InnoEngine';revision=$revision;sourceTree=$tree;toolingRevision=$revision;
        projectManifest='reference/projects/catalog.json';projectManifestIdentity=(Hash $inventory);apiManifests=@($capture)}
    $inputRoot = Join-Path $root 'package-input'
    Write-Text (Join-Path $inputRoot 'reference/engine.lock.json') ($binding | ConvertTo-Json -Depth 20 -Compress)
    Write-Text (Join-Path $inputRoot 'reference/projects/catalog.json') $inventory
    Write-Text (Join-Path $inputRoot 'reference/api/probe.json') $facts
    $package = Join-Path $root 'api-package.json'
    & $Dotnet $BuildCli docs api-package --docs-root $inputRoot --package $package
    if ($LASTEXITCODE) { throw 'Fixture API packaging failed.' }
    $publisher = Join-Path $PSScriptRoot '../../.github/scripts/documentation_artifacts.py'
    & python -B $publisher publish --engine-root $engine --assembly $BuildCli --package $package --workspace (Join-Path $root 'first')
    if ($LASTEXITCODE) { throw 'First publication failed.' }
    $first = (Invoke-FixtureGit $docsRemote @('rev-parse', 'draft')) -join ''
    if (((Invoke-FixtureGit $docsRemote @('rev-parse', 'main')) -join '') -cne $mainBefore) { throw 'The publisher modified main.' }
    if (((Invoke-FixtureGit $docsRemote @('show', 'draft:content/en/manual/task.md')) -join '') -cne 'A completely independent guide.') { throw 'The publisher changed independent manual content.' }
    [void](Invoke-FixtureGit $docsRemote @('show', 'draft:reference/api/content.json'))
    # A release can advance main without changing the API payload. A repeated export
    # must not push a merge commit solely to synchronize branch ancestry.
    Write-Text (Join-Path $docs 'release-note.txt') 'Main-only release metadata.'
    [void](Invoke-FixtureGit $docs @('checkout', 'main'))
    [void](Invoke-FixtureGit $docs @('add', 'release-note.txt'))
    [void](Invoke-FixtureGit $docs @('commit', '-m', 'fixture(docs): advance main without API changes'))
    [void](Invoke-FixtureGit $docs @('push', $docsRemote, 'main'))
    & python -B $publisher publish --engine-root $engine --assembly $BuildCli --package $package --workspace (Join-Path $root 'repeat')
    if ($LASTEXITCODE -or ((Invoke-FixtureGit $docsRemote @('rev-parse', 'draft')) -join '') -cne $first) { throw 'Identical API export created another commit.' }
    # Intercept only the fixture process's first push. A real competing author commits before
    # the ordinary Git fast-forward push; production code receives no test hooks or bypasses.
    $author = Join-Path $root 'author'
    [void](Invoke-FixtureGit $root @('clone', '--branch', 'draft', $docsRemote, $author))
    [void](Invoke-FixtureGit $author @('config', 'user.name', 'Concurrent documentation author'))
    [void](Invoke-FixtureGit $author @('config', 'user.email', 'author@example.invalid'))
    $registry = Get-Content (Join-Path $author 'reference/api/content.json') -Raw | ConvertFrom-Json
    $fragment = $registry.entries[0].path
    $manual = [IO.File]::ReadAllText((Join-Path $author $fragment)).Replace('source: xml', 'source: FLwolfy') + "`nA concurrent author owns this explanation.`n"
    Write-Text (Join-Path $author $fragment) $manual
    [void](Invoke-FixtureGit $author @('add', '--', $fragment))
    [void](Invoke-FixtureGit $author @('commit', '-m', 'docs(api): take ownership during synchronization'))
    $facts = $facts.Replace('A source value.', 'An updated XML explanation.')
    $capture.sha256 = Hash $facts
    $binding.apiManifests = @($capture)
    Write-Text (Join-Path $inputRoot 'reference/engine.lock.json') ($binding | ConvertTo-Json -Depth 20 -Compress)
    Write-Text (Join-Path $inputRoot 'reference/api/probe.json') $facts
    & $Dotnet $BuildCli docs api-package --docs-root $inputRoot --package $package
    if ($LASTEXITCODE) { throw 'Updated fixture packaging failed.' }
    $hook = Join-Path $docsRemote 'hooks/pre-receive'
    $hookCode = @'
#!/bin/sh
set -eu
rm -- "$0"
unset GIT_DIR GIT_WORK_TREE GIT_INDEX_FILE GIT_QUARANTINE_PATH GIT_OBJECT_DIRECTORY GIT_ALTERNATE_OBJECT_DIRECTORIES
git -C "$INNO_FIXTURE_AUTHOR" push origin HEAD:refs/heads/draft
'@
    Write-Text $hook ($hookCode.Replace("`r`n", "`n") + "`n")
    if (!$IsWindows) { & chmod +x $hook }
    $env:INNO_FIXTURE_AUTHOR = $author
    try {
        & python -B $publisher publish --engine-root $engine --assembly $BuildCli --package $package --workspace (Join-Path $root 'concurrent') *> (Join-Path $root 'concurrent-result.log')
    } finally { Remove-Item Env:INNO_FIXTURE_AUTHOR }
    if ($LASTEXITCODE) { throw "Concurrent publication failed; see $root/concurrent-result.log." }
    $concurrentLog = [IO.File]::ReadAllText((Join-Path $root 'concurrent-result.log'))
    if (!$concurrentLog.Contains('Recompute ownership')) { throw 'The competing push did not exercise ownership recomputation.' }
    $after = Join-Path $root 'after-race'
    [void](Invoke-FixtureGit $root @('clone', '--branch', 'draft', $docsRemote, $after))
    $published = [IO.File]::ReadAllText((Join-Path $after $fragment))
    $authoring = '# Authoring metadata.'
    if (!$manual.Contains($authoring) -or !$published.Contains($authoring) -or
        $published.Substring($published.IndexOf($authoring)) -cne $manual.Substring($manual.IndexOf($authoring))) {
        throw 'Concurrent publication changed author-owned metadata or prose.'
    }
    if ($published -cnotmatch '(?m)^outdatedFor: [0-9a-f]{64}\r?$' -or $manual -cmatch '(?m)^outdatedFor:') {
        throw 'The competing XML change did not mark the previously current prose outdated.'
    }
    if (![IO.File]::ReadAllText((Join-Path $after 'reference/api/probe.json')).Contains('An updated XML explanation.')) { throw 'Source facts were not synchronized after the competing edit.' }
    $first = (Invoke-FixtureGit $docsRemote @('rev-parse', 'draft')) -join ''
    Write-Text (Join-Path $engine 'input.txt') 'A newer engine input.'
    [void](Invoke-FixtureGit $engine @('add', '.'))
    [void](Invoke-FixtureGit $engine @('commit', '-m', 'fixture(engine): advance master'))
    [void](Invoke-FixtureGit $engine @('push', 'origin', 'master'))
    & python -B $publisher publish --engine-root $engine --assembly $BuildCli --package $package --workspace (Join-Path $root 'stale') *> (Join-Path $root 'stale-result.log')
    if ($LASTEXITCODE -eq 0 -or ((Invoke-FixtureGit $docsRemote @('rev-parse', 'draft')) -join '') -cne $first) { throw 'A superseded API export was accepted.' }
    if ((Get-Content (Join-Path $root 'stale-result.log') -Raw) -notmatch 'superseded') { throw 'Superseded export did not explain its rejection.' }
    $success = $true
    Write-Output 'Passed: complete draft publication, untouched main/manual, repeat no-op, concurrent manual takeover, updated facts and stale export rejection.'
}
finally {
    $env:GIT_CONFIG_GLOBAL = $oldConfig
    $env:GIT_TERMINAL_PROMPT = $oldPrompt
    # First-failure workspaces remain available. Successful ephemeral clones belong exclusively to this fixture.
    if ($success) {
        $owned = [IO.Path]::GetFullPath($root)
        $parent = [IO.Path]::GetFullPath('artifacts/validation/api-publication', $engineRoot) + [IO.Path]::DirectorySeparatorChar
        if (!$owned.StartsWith($parent, [StringComparison]::OrdinalIgnoreCase)) { throw 'Unexpected fixture cleanup boundary.' }
        Remove-Item -LiteralPath $owned -Recurse -Force
    }
}
