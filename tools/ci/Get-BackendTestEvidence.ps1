param(
    [Parameter(Mandatory)][string]$ApiBase,
    [Parameter(Mandatory)][hashtable]$Headers,
    [Parameter(Mandatory)][string]$HostTarget,
    [Parameter(Mandatory)][string]$EnvironmentIdentity,
    [Parameter(Mandatory)][string]$DefaultBranch,
    [Parameter(Mandatory)][long]$RepositoryId
)
$ErrorActionPreference = 'Stop'
if ($HostTarget -cnotmatch '^[a-z0-9-]+$' -or $EnvironmentIdentity -cnotmatch '^[a-f0-9]{64}$' -or $RepositoryId -le 0) { throw 'Invalid backend evidence scope.' }
$name = "backend-tests-$HostTarget-$EnvironmentIdentity"
$list = Invoke-RestMethod -Headers $Headers -Uri "$ApiBase/actions/artifacts?name=$name&per_page=12"
foreach ($item in @($list.artifacts | Where-Object {
    !$_.expired -and $_.name -ceq $name -and $_.digest -cmatch '^sha256:[a-f0-9]{64}$' -and $_.size_in_bytes -le 4MB -and
    $_.workflow_run.head_branch -ceq $DefaultBranch -and $_.workflow_run.repository_id -eq $RepositoryId -and $_.workflow_run.head_repository_id -eq $RepositoryId
} | Select-Object -First 3)) {
    $run = Invoke-RestMethod -Headers $Headers -Uri "$ApiBase/actions/runs/$($item.workflow_run.id)"
    if ($run.conclusion -cne 'success' -or $run.status -cne 'completed' -or $run.head_sha -cne $item.workflow_run.head_sha -or
        $run.head_branch -cne $DefaultBranch -or $run.head_repository.id -ne $RepositoryId -or
        $run.event -cnotin @('push','workflow_dispatch') -or $run.path -cne '.github/workflows/engine-ci.yml') { continue }
    $file = [IO.Path]::GetTempFileName()
    try {
        Invoke-WebRequest -Headers $Headers -Uri "$ApiBase/actions/artifacts/$($item.id)/zip" -OutFile $file
        if ('sha256:'+(Get-FileHash -LiteralPath $file).Hash.ToLowerInvariant() -cne $item.digest) { throw 'Backend test evidence changed during transfer.' }
        $zip = [IO.Compression.ZipFile]::OpenRead($file)
        try {
            $entry = $zip.GetEntry('backend-tests.json')
            if ($zip.Entries.Count -ne 1 -or !$entry -or $entry.Length -gt 4MB) { throw 'Invalid backend test evidence payload.' }
            $reader = [IO.StreamReader]::new($entry.Open())
            try { $proof = $reader.ReadToEnd() | ConvertFrom-Json -AsHashtable -Depth 40 } finally { $reader.Dispose() }
        } finally { $zip.Dispose() }
    } finally { [IO.File]::Delete($file) }
    if ($proof.version -ne 2 -or $proof.commit -cne $run.head_sha -or $proof.attempt -ne $run.run_attempt -or
        $proof.environment -cne $EnvironmentIdentity -or $proof.hostTarget -cne $HostTarget) { continue }
    $proof.tests
}
