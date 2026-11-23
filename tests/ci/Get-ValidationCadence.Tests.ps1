$ErrorActionPreference = 'Stop'
$directory = Join-Path ([IO.Path]::GetFullPath('../../artifacts/validation/event-routing-contracts', $PSScriptRoot)) ([Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $directory | Out-Null
$token = $env:GH_TOKEN
$output = $env:GITHUB_OUTPUT
$commit = 'a' * 40
try {
    $env:GH_TOKEN = 'fixture-read-token'
    $env:GITHUB_OUTPUT = Join-Path $directory 'outputs.txt'
    foreach ($case in @('matching', 'stale-head', 'foreign-repository', 'closed', 'foreign-branch', 'none', 'master', 'pull_request', 'merge_group')) {
        $event = if ($case -cin @('pull_request', 'merge_group')) { $case } else { 'push' }
        $branch = if ($case -ceq 'master') { 'master' } else { 'feature' }
        $eventPath = Join-Path $directory "$case.json"
        @{ ref = "refs/heads/$branch"; repository = @{ default_branch = 'master' } } | ConvertTo-Json -Depth 4 | Set-Content $eventPath
        $fixtureCalls = @{ count = 0 }
        $fixturePull = @{ state = 'open'; head = @{ sha = $commit; ref = 'feature'; repo = @{ full_name = 'fixture/engine' } } }
        switch ($case) {
            'stale-head' { $fixturePull.head.sha = 'b' * 40 }
            'foreign-repository' { $fixturePull.head.repo.full_name = 'foreign/engine' }
            'closed' { $fixturePull.state = 'closed' }
            'foreign-branch' { $fixturePull.head.ref = 'another-branch' }
        }
        $fixtureCase = $case
        function Invoke-RestMethod {
            param($Uri, $Headers)
            $fixtureCalls.count++
            if ($Uri -cne "https://example.invalid/repos/fixture/engine/commits/$commit/pulls?per_page=100") {
                throw 'Event routing queried a different candidate or service.'
            }
            if ($fixtureCase -cne 'none') { $fixturePull }
        }
        $selected = & "$PSScriptRoot/../../tools/ci/Get-ValidationCadence.ps1" -EventPath $eventPath -Event $event -Commit $commit `
            -Repository fixture/engine -ApiUrl https://example.invalid | ConvertFrom-Json
        if ($selected.run -ne ($case -cne 'matching')) { throw "Event delegation accepted a foreign or stale PR: $case" }
        $expected = if ($case -ceq 'master') { 'master' } elseif ($event -ceq 'push') { 'fast' } else { 'pull-request' }
        if ($selected.profile -cne $expected) { throw "The actual merge candidate lost its execution policy: $case" }
        $calls = if ($event -ceq 'push' -and $case -cne 'master') { 1 } else { 0 }
        if ($fixtureCalls.count -ne $calls) { throw 'A required candidate was incorrectly delegated to a branch lookup.' }
    }
} finally {
    Remove-Item Function:Invoke-RestMethod -ErrorAction SilentlyContinue
    $env:GH_TOKEN = $token
    $env:GITHUB_OUTPUT = $output
}
Write-Output "Passed 9 event routing contracts. Evidence: $directory"
