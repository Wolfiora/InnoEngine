param(
    [Parameter(Mandatory)][string]$Platform,
    [Parameter(Mandatory)][string]$ExpectedCommit,
    [Parameter(Mandatory)][string]$WorkerDirectory,
    [int]$Attempt = 1,
    [long]$SourceRunId = 0
)

$ErrorActionPreference = 'Stop'
$repository = [IO.Path]::GetFullPath('../..', $PSScriptRoot)
$planRoot = Join-Path $repository 'artifacts/validation/ci-plan'
$plan = Get-Content -LiteralPath (Join-Path $planRoot 'plan.json') -Raw | ConvertFrom-Json
$shards = @($plan.shards)
if ($plan.commit -cne $ExpectedCommit -or $shards.Count -eq 0 -or
    @($shards.id | Sort-Object -Unique).Count -ne $shards.Count -or
    @($shards | Where-Object { $_.id -cnotin @('architecture', 'managed', 'native') }).Count -gt 0) {
    throw 'The runner requires a nonempty frozen shard matrix for this candidate.'
}
$workers = [IO.Path]::GetFullPath($WorkerDirectory)
if (-not [IO.Path]::IsPathFullyQualified($WorkerDirectory) -or (Test-Path -LiteralPath $workers)) {
    throw 'Each job requires an unused absolute worker directory.'
}
New-Item -ItemType Directory -Path $workers | Out-Null
$receipts = @{}
$failed = @()
$pending = @($shards)
$execute = {
    param($checkout, $planDirectory, $shard, $commit, $sourceRun, $prerequisites, $stepOutput)
    $ErrorActionPreference = 'Stop'
    $env:GITHUB_WORKSPACE = $checkout
    $env:GITHUB_OUTPUT = $stepOutput
    Set-Location -LiteralPath $checkout
    $frozen = Get-Content -LiteralPath (Join-Path $planDirectory 'plan.json') -Raw | ConvertFrom-Json
    if ($frozen.backendStore) {
        # Nested SDK invocations (including Support Pack publishing) inherit the same verified
        # platform inputs. These variables are scoped to this isolated worker process.
        $env:InnoBackendPackageDirectory = [IO.Path]::GetFullPath($frozen.backendStore, $checkout)
        $env:InnoBackendHostTarget = $frozen.backendHost
        $env:InnoNativeDependencyManifest = [IO.Path]::GetFullPath($frozen.nativeDependencies, $checkout)
    }
    ./tools/ci/Invoke-ValidationCi.ps1 -Stage Run -BaseRevision $commit -ExpectedCommit $commit `
        -Shard $shard -PlanDirectory $planDirectory -OutputDirectory "artifacts/validation/shards/$shard" `
        -PrerequisitesDirectory $prerequisites -SourceRunId $sourceRun
}
# Every worker uses this runner's frozen SDK and image. Concurrent writers use separate checkouts.
while ($pending.Count -gt 0) {
    $ready = @($pending | Where-Object {
        @($_.prerequisites | Where-Object { -not $receipts.ContainsKey($_) }).Count -eq 0
    } | Sort-Object @{ Expression = { $_.id -ceq 'native' }; Descending = $true }, id)
    if ($ready.Count -eq 0) { throw "Unsatisfied frozen shard prerequisites: $($pending.id -join ', '). Failed: $($failed -join ', ')." }
    $running = @()
    try {
        foreach ($shard in $ready) {
            $id = $shard.id
            $checkout = $repository
            $stepOutput = Join-Path $workers "$id.outputs"
            $prerequisites = Join-Path $workers "$id-prerequisites"
            New-Item -ItemType Directory -Path $prerequisites | Out-Null
            foreach ($dependency in $shard.prerequisites) {
                Copy-Item -LiteralPath $receipts[$dependency] -Destination (New-Item -ItemType Directory -Path (Join-Path $prerequisites $dependency))
            }
            if ($running.Count -gt 0) {
                $checkout = Join-Path $workers $id
                & git -C $repository worktree add --detach $checkout $ExpectedCommit | Out-Host
                if ($LASTEXITCODE -ne 0) { throw "Could not isolate concurrent shard: $id." }
                if ($plan.backendStore) {
                    # A fresh linked worktree shares immutable package bytes only. Native leases, MSBuild
                    # intermediate outputs and the selected product's installed generations stay worker-owned.
                    $store = [IO.Path]::GetFullPath($plan.backendStore, $repository)
                    $sourceBoundary = [IO.Path]::GetFullPath('artifacts', $repository) + [IO.Path]::DirectorySeparatorChar
                    if (!$store.StartsWith($sourceBoundary, [StringComparison]::Ordinal)) { throw 'Backend store escaped the repository artifact owner.' }
                    $destination = [IO.Path]::GetFullPath($plan.backendStore, $checkout)
                    $relativePaths = @('selection.json') + @(foreach ($package in $plan.backendPackages) {
                        $target = $package.target
                        if ($target -cnotmatch '^[a-z0-9-]+$') { throw 'Invalid acquired platform.' }
                        $inventory = Join-Path $store "$target/manifest.json"
                        if ((Get-FileHash -LiteralPath $inventory).Hash.ToLowerInvariant() -cne $package.manifestSha256) {
                            throw "The acquired inventory changed for $target."
                        }
                        "$target/receipt.json"
                        "$target/manifest.json"
                        foreach ($item in (Get-Content -LiteralPath $inventory -Raw | ConvertFrom-Json).files) { "$target/$($item.path)" }
                    })
                    foreach ($relative in $relativePaths) {
                        $source = [IO.Path]::GetFullPath($relative, $store)
                        if (!$source.StartsWith(($store + [IO.Path]::DirectorySeparatorChar), [StringComparison]::Ordinal)) {
                            throw 'Backend inventory escaped its owner.'
                        }
                        $path = Join-Path $destination $relative
                        [IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($path)) | Out-Null
                        if ([IO.Path]::GetPathRoot($source) -ceq [IO.Path]::GetPathRoot($path)) {
                            New-Item -Path $path -ItemType HardLink -Target $source | Out-Null
                        } else {
                            [IO.File]::Copy($source, $path, $false)
                        }
                    }
                }
                $inputs = Join-Path $workers "$id-inputs"
                $planOwner = Join-Path $inputs "validation-plan-$Platform-attempt-$Attempt"
                $entryOwner = Join-Path $inputs "validation-entry-$Platform-attempt-$Attempt"
                New-Item -ItemType Directory -Path $planOwner, $entryOwner -Force | Out-Null
                foreach ($name in @('plan.json','entry.json')) { Copy-Item -LiteralPath (Join-Path $planRoot $name) -Destination $planOwner }
                $entry = Join-Path $repository 'artifacts/validation/bootstrap/outputs/bin/ValidationEntry/debug'
                Get-ChildItem -LiteralPath $entry | Copy-Item -Destination $entryOwner -Recurse
                foreach ($dependency in $shard.prerequisites) {
                    $owner = New-Item -ItemType Directory -Path (Join-Path $inputs "validation-result-$Platform-$dependency-attempt-$Attempt")
                    Copy-Item -LiteralPath $receipts[$dependency] -Destination $owner
                }
                & (Join-Path $checkout 'tools/ci/Prepare-ValidationShard.ps1') -ReceivedDirectory $inputs -Platform $Platform -Shard $id -Attempt $Attempt
                $prerequisites = Join-Path $checkout 'artifacts/validation/prerequisites'
            }
            $output = Join-Path $checkout "artifacts/validation/shards/$id"
            "${id}_directory=$output" >> $env:GITHUB_OUTPUT
            $arguments = @($checkout, 'artifacts/validation/ci-plan', $id, $ExpectedCommit, $SourceRunId, $prerequisites, $stepOutput)
            $job = Start-Job -ScriptBlock $execute -ArgumentList $arguments
            $running += [pscustomobject]@{ id = $id; job = $job; output = $output; metadata = $stepOutput }
        }
        # Forward every active worker's output as it arrives, including failures from a shorter shard.
        Receive-Job -Job @($running.job) -Wait -ErrorAction Continue | Out-Host
        foreach ($worker in $running) {
            if ($worker.job.State -cne 'Completed') { $failed += $worker.id; continue }
            $receipt = Join-Path $worker.output 'results.json'
            if (-not (Test-Path -LiteralPath $receipt -PathType Leaf)) { throw "A successful shard omitted its receipt: $($worker.id)." }
            $receipts[$worker.id] = $receipt
            foreach ($line in Get-Content -LiteralPath $worker.metadata) {
                if ($line -cmatch '^(compiledOutputs|deliverables)=') { "$($worker.id)_$line" >> $env:GITHUB_OUTPUT }
            }
        }
    } finally {
        foreach ($worker in $running) {
            if ($worker.job.State -ceq 'Running') { Stop-Job $worker.job }
            Remove-Job $worker.job -Force
        }
    }
    $pending = @($pending | Where-Object { $_.id -cnotin $ready.id })
}
if ($failed.Count -gt 0) { throw "Required validation shards failed: $($failed -join ', ')." }

$joint = Join-Path $workers 'completed'
foreach ($shard in $shards) {
    $owner = New-Item -ItemType Directory -Path (Join-Path $joint $shard.id) -Force
    Copy-Item -LiteralPath $receipts[$shard.id] -Destination $owner
}
& "$PSScriptRoot/Invoke-ValidationCi.ps1" -Stage Join -Platform $Platform -BaseRevision $ExpectedCommit -ExpectedCommit $ExpectedCommit `
    -PlanDirectory 'artifacts/validation/ci-plan' -OutputDirectory 'artifacts/validation/host-certification' -PrerequisitesDirectory $joint
