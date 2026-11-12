$ErrorActionPreference = 'Stop'
$oldEngine = $env:GH_TOKEN
$oldDocs = $env:INNO_DOCS_READ_TOKEN
$env:GH_TOKEN = 'engine-check-fixture'
$env:INNO_DOCS_READ_TOKEN = 'docs-read-fixture'
$state = @{ mode = ''; completions = @(); reads = 0; checkHeads = @() }
$head = 'a' * 40; $base = 'b' * 40; $merge = 'c' * 40; $tree = 'd' * 40; $docs = 'e' * 40
$call = {
    param([string]$Uri, [string]$Method, $Headers, [string]$ContentType, [string]$Body)
    $enginePrefix = 'https://api.github.com/repos/Wolfiora/InnoEngine'
    $docsPrefix = 'https://api.github.com/repos/Wolfiora/InnoEngine.Docs'
    if ($Uri.StartsWith($docsPrefix, [StringComparison]::Ordinal)) {
        if ($Headers.Authorization -cne 'Bearer docs-read-fixture' -or $Method) { throw 'Docs access used engine check credentials.' }
        if ($Uri -ceq $docsPrefix) { return @{ default_branch = 'main' } }
        if ($Uri -like '*/compare/*') { return @{ status = 'identical'; merge_base_commit = @{ sha = $docs } } }
        if ($Uri -like '*/contents/*') {
            $bytes = [Text.Encoding]::UTF8.GetBytes((@{ repository = 'https://github.com/Wolfiora/InnoEngine'; revision = $head; sourceTree = $tree } | ConvertTo-Json))
            return @{ type = 'file'; encoding = 'base64'; size = $bytes.Length; content = [Convert]::ToBase64String($bytes) }
        }
        if ($Uri -like '*documentation.yml/runs?*') { return @{ workflow_runs = @(@{ id = 12; head_sha = $docs; path = '.github/workflows/documentation.yml'; run_attempt = 1
            repository = @{ full_name = 'Wolfiora/InnoEngine.Docs' }; head_repository = @{ full_name = 'Wolfiora/InnoEngine.Docs' }
            status = 'completed'; conclusion = 'success'; event = 'push' }) } }
        if ($Uri -like '*/jobs?*') { return @{ jobs = @(@{ name = 'Documentation validation'; run_id = 12; status = 'completed'; conclusion = 'success' }) } }
        if ($Uri -like '*/artifacts?*') { return @{ artifacts = @(@{ id = 44; name = 'documentation-site-attempt-1'; expired = $false; size_in_bytes = 100
            digest = 'sha256:' + ('f' * 64); workflow_run = @{ id = 12; head_sha = $docs } }) } }
        throw "Unexpected Docs endpoint $Uri"
    }
    if ($Headers.Authorization -cne 'Bearer engine-check-fixture') { throw 'Engine checks used cross-repository credentials.' }
    $pull = @{ number = 83; state = 'open'; body = "Documentation-Candidate: Wolfiora/InnoEngine.Docs@$docs engine-tree=$tree"
        head = @{ sha = $head }; base = @{ sha = $base; repo = @{ full_name = 'Wolfiora/InnoEngine' } }; merge_commit_sha = $merge }
    if ($Uri.StartsWith("$enginePrefix/pulls?", [StringComparison]::Ordinal)) { return @($pull) }
    if ($Uri -ceq "$enginePrefix/pulls/83") {
        $state.reads++
        if ($state.reads -gt 1) {
            switch ($state.mode) {
                'head-changed' { $pull.head.sha = '1' * 40 }
                'base-changed' { $pull.base.sha = '2' * 40 }
                'merge-changed' { $pull.merge_commit_sha = '3' * 40 }
                'metadata-changed' { $pull.body += ' changed' }
                'closed' { $pull.state = 'closed' }
            }
        }
        if ($state.mode -ceq 'no-merge') { $pull.merge_commit_sha = $null }
        return $pull
    }
    if ($Uri -ceq "$enginePrefix/git/commits/$merge") { return @{ tree = @{ sha = $tree } } }
    if ($Uri -ceq "$enginePrefix/check-runs" -and $Method -ceq 'Post') {
        $value = $Body | ConvertFrom-Json
        if ($value.name -cne 'Documentation pairing' -or $value.status -cne 'in_progress') { throw 'A check was certified before reconciliation.' }
        $state.checkHeads += $value.head_sha
        return @{ id = 99 }
    }
    if ($Uri -ceq "$enginePrefix/check-runs/99" -and $Method -ceq 'Patch') {
        $state.completions += ($Body | ConvertFrom-Json)
        return @{ id = 99 }
    }
    throw "Unexpected engine endpoint $Uri"
}
Set-Item Function:Invoke-RestMethod $call.GetNewClosure()
$cases = 0
try {
    foreach ($mode in @('', 'head-changed', 'base-changed', 'merge-changed', 'metadata-changed', 'closed', 'no-merge', 'missing-access')) {
        $state.mode = $mode; $state.completions = @(); $state.checkHeads = @(); $state.reads = 0
        $env:INNO_DOCS_READ_TOKEN = if ($mode -ceq 'missing-access') { '' } else { 'docs-read-fixture' }
        $result = & "$PSScriptRoot/Update-DocumentationPairing.ps1" -DocumentationRevision $docs
        $expected = if ($mode) { 'failure' } else { 'success' }
        if ($state.completions.Count -ne 1 -or $state.completions[0].conclusion -cne $expected) { throw "Incorrect check result: $mode" }
        if ($state.checkHeads.Count -ne 1 -or $state.checkHeads[0] -cne $(if ($mode -ceq 'no-merge') { $head } else { $merge })) {
            throw 'Pairing status was not attached to the actual candidate.'
        }
        if ($mode -and $result.status -cne 'Blocked') { throw 'An invalid pair did not remain blocked.' }
        $cases++
    }
    $state.mode = ''; $state.completions = @(); $state.checkHeads = @(); $state.reads = 0
    $env:INNO_DOCS_READ_TOKEN = 'docs-read-fixture'
    $result = & "$PSScriptRoot/Update-DocumentationPairing.ps1" -MergeCommit $merge
    if ($result.status -cne 'Passed' -or $state.completions.Count -ne 1 -or $state.completions[0].conclusion -cne 'success' -or $state.checkHeads[0] -cne $merge) {
        throw 'The exact merge-group candidate did not receive its reconciled check.'
    }
    $cases++
} finally {
    Remove-Item Function:Invoke-RestMethod
    $env:GH_TOKEN = $oldEngine
    $env:INNO_DOCS_READ_TOKEN = $oldDocs
}
Write-Output "Passed $cases pairing status/race scenarios. No hosted check was created."
