param(
    [Parameter(Mandatory)][object[]]$Hosts,
    [Parameter(Mandatory)][string]$Baseline,
    [Parameter(Mandatory)][string]$Candidate,
    [Parameter(Mandatory)][string]$DefaultBranch
)
$ErrorActionPreference = 'Stop'
if ($Baseline -cnotmatch '^[a-f0-9]{40}$' -or $Candidate -cnotmatch '^[a-f0-9]{40}$' -or !@($Hosts | Where-Object fast).Count) { return $Hosts }
& git merge-base --is-ancestor $Baseline $Candidate 2>$null
if ($LASTEXITCODE) { return $Hosts }
$changes = @(& git -c core.quotepath=false diff --no-renames --name-status $Baseline $Candidate)
if ($LASTEXITCODE -or !$changes.Count -or @($changes | Where-Object { $_ -cnotmatch '^M\t(tests/[^\t]+\.cs)$' }).Count) { return $Hosts }
$paths = @($changes | ForEach-Object { $_.Substring(2) })
$headers = @{Authorization="Bearer $env:GH_TOKEN";Accept='application/vnd.github+json'}
$base = "$env:GITHUB_API_URL/repos/$env:GITHUB_REPOSITORY"
try {
    $runs = Invoke-RestMethod -Headers $headers -Uri "$base/actions/workflows/engine-ci.yml/runs?head_sha=$Baseline&event=push&status=success&per_page=20"
    $run = $runs.workflow_runs | Where-Object {
        $_.head_sha -ceq $Baseline -and $_.head_branch -ceq $DefaultBranch -and $_.head_repository.id -eq $env:GITHUB_REPOSITORY_ID -and
        $_.event -ceq 'push' -and $_.conclusion -ceq 'success' -and $_.path -ceq '.github/workflows/engine-ci.yml'
    } | Select-Object -First 1
    if (!$run) { return $Hosts }
    $name = "validation-host-routing-attempt-$($run.run_attempt)"
    $list = Invoke-RestMethod -Headers $headers -Uri "$base/actions/runs/$($run.id)/artifacts?name=$name&per_page=2"
    $artifact = @($list.artifacts | Where-Object {
        !$_.expired -and $_.name -ceq $name -and $_.digest -cmatch '^sha256:[a-f0-9]{64}$' -and $_.size_in_bytes -le 4MB -and
        $_.workflow_run.head_sha -ceq $Baseline -and $_.workflow_run.id -eq $run.id -and $_.workflow_run.repository_id -eq $env:GITHUB_REPOSITORY_ID
    })
    if ($artifact.Count -ne 1) { return $Hosts }
    $file = [IO.Path]::GetTempFileName()
    try {
        Invoke-WebRequest -Headers $headers -Uri "$base/actions/artifacts/$($artifact[0].id)/zip" -OutFile $file
        if ('sha256:'+(Get-FileHash -LiteralPath $file).Hash.ToLowerInvariant() -cne $artifact[0].digest) { return $Hosts }
        $zip = [IO.Compression.ZipFile]::OpenRead($file)
        try {
            $entry = $zip.GetEntry('host-routing.json')
            if (!$entry -or $zip.Entries.Count -ne 1 -or $entry.Length -gt 4MB) { return $Hosts }
            $reader = [IO.StreamReader]::new($entry.Open())
            try { $proof = $reader.ReadToEnd() | ConvertFrom-Json -AsHashtable } finally { $reader.Dispose() }
        } finally { $zip.Dispose() }
    } finally { [IO.File]::Delete($file) }
    if ($proof.commit -cne $Baseline -or !$proof.hosts) { return $Hosts }
    return @($Hosts | Where-Object {
        $excluded = $proof.hosts[$_.runner]
        $_.fast -or !$excluded -or @($paths | Where-Object { $_ -cnotin $excluded }).Count -gt 0
    })
} catch {
    Write-Warning "Scheduling all registered hosts because the routing receipt could not be read ($($_.Exception.GetType().Name))."
    return $Hosts
}
