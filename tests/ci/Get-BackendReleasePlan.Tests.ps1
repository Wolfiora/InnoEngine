$ErrorActionPreference = 'Stop'
$script = Join-Path $PSScriptRoot '../../tools/ci/Get-BackendReleasePlan.ps1'
$assets = @(Get-Content (Join-Path $PSScriptRoot '../../.github/native-targets.json') -Raw | ConvertFrom-Json)
$revision = 'a' * 40
$date = [datetime]'2026-11-19T00:00:00Z'
function Digests([int]$Generation) {
    $result = @{}
    foreach ($asset in $assets) { $result[$asset.asset] = $Generation.ToString('x64') }
    return $result
}
function Release([int]$Generation) {
    $digests = Digests $Generation
    return [pscustomobject]@{ tag_name="backend-prebuilts-2026.11.19.$Generation"; id=$Generation; draft=$false; prerelease=$true
        target_commitish=$revision; published_at=$date.AddMinutes($Generation).ToString('o')
        assets=@($assets | ForEach-Object { [pscustomobject]@{name=$_.asset; state='uploaded'; digest="sha256:$($digests[$_.asset])"} }) }
}
$product = [pscustomobject]@{ tag_name='v1.2.3'; prerelease=$false; draft=$false }
$history = @($product)
for ($generation = 1; $generation -le 5; $generation++) {
    $plan = & $script -Releases $history -Digests (Digests $generation) -Revision $revision -UtcDate $date
    if ($plan.operation -cne 'create' -or $plan.tag -cne "backend-prebuilts-2026.11.19.$generation") { throw 'Unexpected platform release version.' }
    if ($generation -gt 3) {
        if ($plan.retire.id -ne ($generation - 3)) { throw 'Rotation selected the wrong release.' }
        $blocked = $false
        try { $null = & $script -Releases $history -Digests (Digests $generation) -Revision $revision -UtcDate $date -ActiveTags @($plan.retire.tag) }
        catch { $blocked = $_.Exception.Message.Contains('still has a reader') }
        if (!$blocked) { throw 'An active rollback release could be retired.' }
        $history = @($history | Where-Object tag_name -CNE $plan.retire.tag)
    } elseif ($null -ne $plan.retire) { throw 'A release was retired before the limit.' }
    $history += Release $generation
    $repeat = & $script -Releases $history -Digests (Digests $generation) -Revision $revision -UtcDate $date
    if ($repeat.operation -cne 'reuse' -or $repeat.tag -cne $plan.tag) { throw 'A retry created another prerelease.' }
}
$draft = Release 6
$draft.draft = $true
$draft.assets = @($draft.assets[0])
$resume = & $script -Releases ($history + $draft) -Digests (Digests 6) -Revision $revision -UtcDate $date
if ($resume.operation -cne 'resume' -or $resume.id -ne 6 -or $resume.retire.id -ne 3) { throw 'An interrupted matching draft could not resume.' }
$draft.assets[0].digest = 'sha256:' + ('0' * 64)
$rejected = $false
try { $null = & $script -Releases ($history + $draft) -Digests (Digests 6) -Revision $revision -UtcDate $date }
catch { $rejected = $_.Exception.Message.Contains('different producer or asset set') }
if (!$rejected) { throw 'An inconsistent draft was accepted.' }
