$ErrorActionPreference = 'Stop'
# These are the certificate producer's executable dependencies, not a project or test selector.
# Keep this boundary separate from unrelated release, cache and notification transports.
$dependencies = @('Get-DocumentationCertificatePolicy.ps1', 'New-DocumentationSourceCertificate.ps1',
    'Get-CompatibleDocumentationEvidence.ps1', 'Get-DocumentationSourceInputs.ps1',
    'Get-DocumentationSourceReceipt.ps1', 'Get-DocumentationExampleReceipt.ps1', 'Test-ValidationReport.ps1')
$inputs = @($dependencies | Sort-Object -CaseSensitive | ForEach-Object {
    $_ + "`0" + (Get-FileHash -LiteralPath (Join-Path $PSScriptRoot $_) -Algorithm SHA256).Hash.ToLowerInvariant()
})
[Convert]::ToHexStringLower([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($inputs -join "`n")))
