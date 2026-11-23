$ErrorActionPreference = 'Stop'
$commit = 'a' * 40
$originalToken = $env:GH_TOKEN
$passed = 0
try {
    $env:GH_TOKEN = 'fixture-token'
    foreach ($case in @('success', 'already-moved', 'dispatch-race', 'failed', 'wrong-workflow', 'duplicate')) {
        $state = @{ posts = 0; polls = 0; request = '' }
        function Invoke-RestMethod {
            param($Uri, $Headers, $Method, $ContentType, $Body)
            if ($Uri.Contains('/commits/')) { return @{ sha = $(if ($case -eq 'already-moved') { 'b' * 40 } else { $commit }) } }
            if ($Method -ceq 'Post') {
                $state.posts++
                $dispatch = $Body | ConvertFrom-Json
                if ($dispatch.ref -cne 'master' -or $dispatch.inputs.full -cne 'true' -or $dispatch.inputs.source_run -cne '42') { throw 'Completion dispatched an incorrect policy or source.' }
                $state.request = $dispatch.inputs.request_id
                return
            }
            $state.polls++
            $run = @{ id = 43; display_title = "Engine validation $($state.request)"; head_sha = $commit; head_branch = 'master';
                path = '.github/workflows/engine-ci.yml'; status = 'completed'; conclusion = 'success'; html_url = 'https://example.invalid/run/43' }
            switch ($case) {
                'dispatch-race' { $run.head_sha = 'c' * 40 }
                'failed' { $run.conclusion = 'failure' }
                'wrong-workflow' { $run.path = '.github/workflows/other.yml' }
            }
            $runs = @($run)
            if ($case -eq 'duplicate') { $runs += $run }
            return @{ workflow_runs = $runs }
        }
        function Start-Sleep { param($Seconds) throw 'Unexpected wait in a completed-run contract.' }
        $accepted = $false
        try {
            $result = & "$PSScriptRoot/../../tools/ci/Complete-ValidationRelease.ps1" -SourceRunId 42 -Commit $commit -Repository fixture/engine -DefaultBranch master
            $accepted = $true
            if ($result -ne 43) { throw 'Completion returned another source run.' }
        } catch { if ($case -eq 'success') { throw } }
        if ($accepted -ne ($case -eq 'success')) { throw "Unexpected completion result: $case" }
        if ($state.posts -ne $(if ($case -eq 'already-moved') { 0 } else { 1 })) { throw 'Dispatch did not obey the exact candidate boundary.' }
        $passed++
    }
} finally { $env:GH_TOKEN = $originalToken }
Write-Output "Release completion contracts passed: $passed."
