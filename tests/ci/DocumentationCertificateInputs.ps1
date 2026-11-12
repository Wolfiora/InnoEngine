# Certificate-local tables preserve exact input membership without expanding a build closure per page.
function New-DocumentationInputStore {
    @{ files = [Collections.Generic.List[object]]::new(); sets = [Collections.Generic.List[object]]::new()
       groups = [Collections.Generic.List[object]]::new()
       fileIds = [Collections.Generic.Dictionary[string,int]]::new([StringComparer]::Ordinal)
       setIds = [Collections.Generic.Dictionary[string,int]]::new([StringComparer]::Ordinal) }
}

function Add-DocumentationInputProof($Store, $Proof) {
    $sources = & "$PSScriptRoot/Get-DocumentationSourceInputs.ps1" -Inputs $Proof.sources
    $indices = [Collections.Generic.List[int]]::new()
    foreach ($path in @($sources.Keys | Sort-Object -CaseSensitive)) {
        $key = $path + "`0" + $sources[$path]
        if (-not $Store.fileIds.ContainsKey($key)) {
            $Store.fileIds.Add($key, $Store.files.Count)
            $Store.files.Add(@{ path = $path; identity = $sources[$path] })
        }
        $indices.Add($Store.fileIds[$key])
    }
    $key = $indices -join ','
    if (-not $Store.setIds.ContainsKey($key)) {
        $Store.setIds.Add($key, $Store.sets.Count)
        $Store.sets.Add($indices.ToArray())
    }
    $entry = [ordered]@{}
    $properties = if ($Proof -is [Collections.IDictionary]) { $Proof.GetEnumerator() } else { $Proof.PSObject.Properties }
    foreach ($property in $properties) { if ($property.Name -cne 'sources') { $entry[$property.Name] = $property.Value } }
    $entry['sourceSet'] = $Store.setIds[$key]
    $entry
}

function Get-DocumentationInputProof($Certificate, $Proof) {
    if ($Proof.sourceSet -isnot [long] -and $Proof.sourceSet -isnot [int]) { throw 'Certificate input proof lacks an integer source-set reference.' }
    if ($Proof.sourceSet -lt 0 -or $Proof.sourceSet -ge @($Certificate.sourceSets).Count) { throw 'Certificate input proof references a missing source set.' }
    $sources = [ordered]@{}
    foreach ($index in @($Certificate.sourceSets[$Proof.sourceSet])) {
        if (($index -isnot [long] -and $index -isnot [int]) -or $index -lt 0 -or $index -ge @($Certificate.sourceFiles).Count) {
            throw 'Certificate source set references a missing file.'
        }
        $file = $Certificate.sourceFiles[$index]
        if ($sources.Contains($file.path)) { throw 'Certificate source set repeats a path.' }
        $sources.Add($file.path, $file.identity)
    }
    $null = & "$PSScriptRoot/Get-DocumentationSourceInputs.ps1" -Inputs $sources
    $entry = [ordered]@{}
    foreach ($property in $Proof.PSObject.Properties) { if ($property.Name -cne 'sourceSet') { $entry[$property.Name] = $property.Value } }
    $entry['sources'] = $sources
    [pscustomobject]$entry
}

function Get-DocumentationInputGroup($Certificate, $Context) {
    if (($Context.inputGroup -isnot [long] -and $Context.inputGroup -isnot [int]) -or $Context.inputGroup -lt 0 `
        -or $Context.inputGroup -ge @($Certificate.inputGroups).Count) { throw 'Certificate context references a missing input group.' }
    $group = $Certificate.inputGroups[$Context.inputGroup]
    if ($group.platform -cne $Context.platform -or $group.configuration -cne $Context.configuration `
        -or $group.target -cne $Context.target -or $group.reportIdentity -cne $Context.reportIdentity -or @($group.inputs).Count -eq 0) {
        throw 'Certificate input group belongs to another execution context.'
    }
    foreach ($proof in $group.inputs) { Get-DocumentationInputProof $Certificate $proof }
}
