param(
    [Parameter(Mandatory)][string] $EngineRoot,
    [Parameter(Mandatory)][string] $Dotnet,
    [Parameter(Mandatory)][string] $BuildCli,
    [Parameter(Mandatory)][string] $Package,
    [Parameter(Mandatory)][string] $Workspace
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

function Invoke-Git([string] $Directory, [string[]] $Arguments) {
    $result = @(& git -C $Directory @Arguments 2>&1)
    if ($LASTEXITCODE -ne 0) { throw "Git operation failed ($($Arguments[0])): $($result -join [Environment]::NewLine)" }
    return $result
}
function Test-Ancestor([string] $Directory, [string] $Older, [string] $Newer) {
    & git -C $Directory merge-base --is-ancestor $Older $Newer
    if ($LASTEXITCODE -gt 1) { throw 'Cannot resolve source ancestry.' }
    return $LASTEXITCODE -eq 0
}

$engine = [IO.Path]::GetFullPath($EngineRoot)
$cli = [IO.Path]::GetFullPath($BuildCli)
$packagePath = [IO.Path]::GetFullPath($Package)
$workspacePath = [IO.Path]::GetFullPath($Workspace)
if (!(Test-Path -LiteralPath $Dotnet -PathType Leaf) -or !(Test-Path -LiteralPath $cli -PathType Leaf)) { throw 'The trusted managed API tooling is unavailable.' }
if (Test-Path -LiteralPath $workspacePath) { throw 'API publication requires a new, exclusively owned workspace.' }
$packet = Get-Content -LiteralPath $packagePath -Raw | ConvertFrom-Json
if ($packet.schema -cne 'inno-api-package/1' -or $packet.repository -cne 'https://github.com/Wolfiora/InnoEngine' -or
    $packet.revision -cnotmatch '^[a-f0-9]{40}$' -or $packet.sourceTree -cnotmatch '^[a-f0-9]{40}$') { throw 'Invalid API producer identity.' }
$origin = (Invoke-Git $engine @('config', '--get', 'remote.origin.url')) -join ''
if ($origin -cnotin @('https://github.com/Wolfiora/InnoEngine', 'https://github.com/Wolfiora/InnoEngine.git')) { throw 'API publication requires the canonical engine remote.' }
[void](Invoke-Git $engine @('fetch', '--no-tags', 'origin', 'master'))
$master = (Invoke-Git $engine @('rev-parse', 'FETCH_HEAD')) -join ''
if ($master -cne $packet.revision) { throw 'The API export is superseded by a newer master input; nothing was published.' }
$tree = (Invoke-Git $engine @('rev-parse', "$($packet.revision)^{tree}")) -join ''
if ($tree -cne $packet.sourceTree) { throw 'The API export tree differs from its verified master commit.' }
New-Item -ItemType Directory -Path $workspacePath | Out-Null

for ($attempt = 1; $attempt -le 3; $attempt++) {
    $checkout = Join-Path $workspacePath "attempt-$attempt"
    # This checkout is data. Never execute scripts, hooks or Actions from the Docs candidate with an App token.
    $clone = @(& git -c core.hooksPath= clone --no-checkout --single-branch --branch draft 'https://github.com/Wolfiora/InnoEngine.Docs.git' $checkout 2>&1)
    if ($LASTEXITCODE -ne 0) { throw "Cannot open the persistent Docs draft branch: $($clone -join [Environment]::NewLine)" }
    [void](Invoke-Git $checkout @('config', 'core.hooksPath', ''))
    [void](Invoke-Git $checkout @('config', 'user.name', 'InnoEngine API synchronization'))
    [void](Invoke-Git $checkout @('config', 'user.email', 'innoengine-api@users.noreply.github.com'))
    [void](Invoke-Git $checkout @('checkout', 'draft'))
    $expected = (Invoke-Git $checkout @('rev-parse', 'HEAD')) -join ''
    [void](Invoke-Git $checkout @('fetch', '--no-tags', 'origin', 'main:refs/remotes/origin/main'))
    if (!(Test-Ancestor $checkout 'origin/main' 'HEAD')) {
        # A human release merge remains in draft's ancestry; real conflicts fail without choosing a side.
        [void](Invoke-Git $checkout @('merge', '--no-edit', 'origin/main'))
    }
    $bindingPath = Join-Path $checkout 'reference/engine.lock.json'
    if (Test-Path -LiteralPath $bindingPath) {
        $binding = Get-Content -LiteralPath $bindingPath -Raw | ConvertFrom-Json
        if ($binding.repository -cne $packet.repository -or $binding.revision -cnotmatch '^[a-f0-9]{40}$') { throw 'Invalid retained engine binding.' }
        if (!(Test-Ancestor $engine $binding.revision $packet.revision)) { throw 'An older or unrelated export cannot replace the retained API revision.' }
    }
    & $Dotnet $cli docs api-import --docs-root $checkout --package $packagePath
    if ($LASTEXITCODE -ne 0) { throw 'Complete API import failed; draft and the deployed site are unchanged.' }
    & $Dotnet $cli docs api-check --docs-root $checkout > (Join-Path $workspacePath "editorial-$attempt.json")
    if ($LASTEXITCODE -ne 0) { throw 'The imported API content did not reconcile with its facts.' }
    $changes = @(Invoke-Git $checkout @('-c', 'core.quotepath=false', 'ls-files', '--modified', '--deleted', '--others', '--exclude-standard'))
    foreach ($path in $changes) {
        if ($path -cnotmatch '^(content/en/api/(engine|scripting)/.*\.md|reference/(api|projects)/.*\.json|reference/engine\.lock\.json)$' -or
            $path.Split('/') -contains '..') { throw "The API publisher attempted to change an independent file: $path" }
    }
    if ($changes.Count -ne 0) {
        [void](Invoke-Git $checkout @('add', '--', 'content/en/api', 'reference/api', 'reference/projects', 'reference/engine.lock.json'))
        [void](Invoke-Git $checkout @('commit', '-m', "docs(api): synchronize engine $($packet.revision.Substring(0, 12))"))
    }
    $candidate = (Invoke-Git $checkout @('rev-parse', 'HEAD')) -join ''
    if ($candidate -ceq $expected) {
        Write-Output 'API synchronization has no content changes; no commit or push was created.'
        exit 0
    }
    [void](Invoke-Git $engine @('fetch', '--no-tags', 'origin', 'master'))
    if (((Invoke-Git $engine @('rev-parse', 'FETCH_HEAD')) -join '') -cne $packet.revision) { throw 'A newer engine input superseded this export before publication.' }
    # An ordinary fast-forward push is the final compare-and-swap. Never force or push main.
    $push = @(& git -C $checkout push origin 'HEAD:refs/heads/draft' 2>&1)
    if ($LASTEXITCODE -eq 0) {
        Write-Output "Published complete API draft $candidate from engine $($packet.revision)."
        exit 0
    }
    [void](Invoke-Git $checkout @('fetch', '--no-tags', 'origin', 'draft'))
    $actual = (Invoke-Git $checkout @('rev-parse', 'FETCH_HEAD')) -join ''
    if ($actual -ceq $candidate) {
        Write-Output "Confirmed API draft $candidate after an interrupted push response."
        exit 0
    }
    if ($actual -ceq $expected) { throw "API push failed without a concurrent edit: $($push -join [Environment]::NewLine)" }
    Write-Output "Draft advanced during attempt $attempt. Recompute ownership from $actual; preserve the first attempt for diagnosis."
}
throw 'Draft changed during three publication attempts. No force push was attempted; rerun from the latest complete export.'
