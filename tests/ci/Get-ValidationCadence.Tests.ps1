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

    Push-Location $directory
    try {
        git init --quiet
        git config user.name 'CI fixture'
        git config user.email 'ci@example.invalid'
        git commit --allow-empty --quiet -m 'Base'
        $base = git rev-parse HEAD
        New-Item -ItemType Directory benchmark | Out-Null
        'local' | Set-Content benchmark/sample.cs
        git add -- benchmark/
        git commit --quiet -m 'Benchmark only'
        $local = git rev-parse HEAD
        'engine' | Set-Content engine.cs
        git add -- engine.cs
        git commit --quiet -m 'Engine change'
        $mixed = git rev-parse HEAD
        foreach ($event in @('pull_request', 'merge_group')) {
            $eventPath = Join-Path $directory "$event.json"
            @{ pull_request = @{ base = @{ sha = $base } }; merge_group = @{ base_sha = $base } } |
                ConvertTo-Json -Depth 4 | Set-Content $eventPath
            foreach ($candidate in @($base, $local, $mixed)) {
                $selected = & "$PSScriptRoot/../../tools/ci/Get-ValidationCadence.ps1" -EventPath $eventPath -Event $event -Commit $candidate -Repository fixture/engine | ConvertFrom-Json
                if ($selected.run -ne ($candidate -ne $local)) { throw "Incorrect benchmark-only routing for $event at $candidate." }
            }
        }
    } finally {
        Pop-Location
    }
} finally {
    Remove-Item Function:Invoke-RestMethod -ErrorAction SilentlyContinue
    $env:GH_TOKEN = $token
    $env:GITHUB_OUTPUT = $output
}
Write-Output "Passed 15 event routing contracts. Evidence: $directory"
