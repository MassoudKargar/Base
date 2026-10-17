[CmdletBinding()]
param(
    [string]$Configuration = 'Release',
    [string]$OutputDirectory,
    [switch]$ListOnly,
    [switch]$KeepBuildCache
)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$solution = Join-Path $root 'Base.sln'
if (!$OutputDirectory) { $OutputDirectory = Join-Path $root 'artifacts\branch-nuget' }
$output = if ([IO.Path]::IsPathRooted($OutputDirectory)) { [IO.Path]::GetFullPath($OutputDirectory) } else { [IO.Path]::GetFullPath((Join-Path $root $OutputDirectory)) }
$projects = @{}
function Add-PackageProject([string]$Path, [string]$Group) {
    $Path = [IO.Path]::GetFullPath($Path)
    if ($Path -match '(?i)(?:^|[\\/.])(samples?|tests?)(?:[\\/.]|$)') { return }
    [xml]$xml = Get-Content -LiteralPath $Path
    if ($xml.Project.Sdk -match 'Microsoft.NET.Sdk.Web|Microsoft.NET.Sdk.Worker') { return }
    if (@($xml.Project.PropertyGroup.IsPackable) -contains 'false' -or @($xml.Project.PropertyGroup.IsTestProject) -contains 'true') { return }
    if (@($xml.Project.PropertyGroup.OutputType) -match '^(Exe|WinExe)$') { return }
    $id = [string]($xml.Project.PropertyGroup.PackageId | Where-Object { $_ } | Select-Object -First 1)
    if (!$id) { $id = [IO.Path]::GetFileNameWithoutExtension($Path) }
    $version = [string]($xml.Project.PropertyGroup.Version | Where-Object { $_ } | Select-Object -First 1)
    if (!$version) { throw "Package version missing: $Path" }
    $frameworks = @($xml.Project.PropertyGroup.TargetFramework) + @($xml.Project.PropertyGroup.TargetFrameworks)
    $frameworks = @($frameworks | Where-Object { $_ } | ForEach-Object { $_ -split ';' })
    if (!$frameworks.Count -or @($frameworks | Where-Object { $_ -notmatch '^net10\.0(?:-|$)' }).Count) { return }
    $key = "$id/$version"
    if ($projects.ContainsKey($key)) {
        if ($projects[$key].Path -ne $Path) { throw "Duplicate package ID/version from different projects: $key" }
        return
    }
    $dependencyKeys = @()
    foreach ($reference in $xml.SelectNodes('//ProjectReference')) {
        $referencePath = [IO.Path]::GetFullPath((Join-Path (Split-Path $Path -Parent) $reference.Include))
        [xml]$referenceXml = Get-Content -LiteralPath $referencePath
        $referenceId = [string]($referenceXml.Project.PropertyGroup.PackageId | Where-Object { $_ } | Select-Object -First 1)
        if (!$referenceId) { $referenceId = [IO.Path]::GetFileNameWithoutExtension($referencePath) }
        $referenceVersion = [string]($referenceXml.Project.PropertyGroup.Version | Where-Object { $_ } | Select-Object -First 1)
        $dependencyKeys += "$referenceId/$referenceVersion"
    }
    $central = $null
    $directory = Split-Path $Path -Parent
    while ($directory) {
        $props = Join-Path $directory 'Directory.Packages.props'
        if (Test-Path $props) { [xml]$central = Get-Content $props; break }
        $directory = Split-Path $directory -Parent
    }
    foreach ($reference in $xml.SelectNodes('//PackageReference')) {
        $referenceVersion = $reference.GetAttribute('Version')
        if (!$referenceVersion -and $central) {
            $entry = $central.SelectNodes('//PackageVersion') | Where-Object Include -EQ $reference.Include | Select-Object -First 1
            if ($entry) { $referenceVersion = $entry.Version }
        }
        if ($referenceVersion) { $dependencyKeys += "$($reference.Include)/$referenceVersion" }
    }
    $projects[$key] = [pscustomobject]@{ Key=$key; Id=$id; Version=$version; Path=$Path; Group=$Group; Dependencies=$dependencyKeys }
}
foreach ($line in (Get-Content $solution)) {
    if ($line -match '^Project\("[^"]+"\) = "[^"]+", "([^"]+\.csproj)"') {
        Add-PackageProject (Join-Path $root $Matches[1]) 'Solution'
    }
}
# The old Extentsions tree is not a release source; use active Base.sln projects.
# Include migrated Utility libraries only; legacy Swagger and sample hosts stay excluded.
Get-ChildItem (Join-Path $root 'Utility') -Recurse -Filter *.csproj |
    Where-Object { $_.Name -notmatch 'Swagger' } |
    ForEach-Object { Add-PackageProject $_.FullName 'Utility' }
$pending = @($projects.Keys | Sort-Object)
$ordered = @()
while ($pending.Count) {
    $ready = @($pending | Where-Object { $key=$_; !@($projects[$key].Dependencies | Where-Object { $pending -contains $_ }).Count })
    if (!$ready.Count) { throw 'Package/project dependency cycle detected.' }
    foreach ($key in $ready) { $ordered += $projects[$key] }
    $pending = @($pending | Where-Object { $ready -notcontains $_ })
}
if ($ListOnly) {
    $ordered | Select-Object Id, Version, Group, Path | Format-Table -AutoSize
    Write-Host "Selected $($ordered.Count) NET 10 library packages; legacy 8/9, samples, tests and executable endpoints are excluded."
    return
}
New-Item -ItemType Directory -Force $output | Out-Null
$logs = Join-Path $output 'logs'
New-Item -ItemType Directory -Force $logs | Out-Null
# Seed an external-only source so old Base packages cannot contaminate template validation.
$external = Join-Path $output 'external-cache'
New-Item -ItemType Directory -Force $external | Out-Null
$cache = if ($env:NUGET_PACKAGES) { $env:NUGET_PACKAGES } else { Join-Path $env:USERPROFILE '.nuget\packages' }
if (Test-Path $cache) {
    Get-ChildItem $cache -Directory | Where-Object Name -NotLike 'base.*' | ForEach-Object {
        Get-ChildItem $_.FullName -Recurse -Filter *.nupkg | ForEach-Object {
            $destination = Join-Path $external $_.Name
            if (!(Test-Path $destination)) { Copy-Item -LiteralPath $_.FullName -Destination $destination }
        }
    }
}

$buildConfig = Join-Path $output 'Build.NuGet.Config'
$restorePackages = Join-Path $output ('restore-' + [guid]::NewGuid().ToString('N'))
$externalFeedXml = [Security.SecurityElement]::Escape((Join-Path $output 'external-cache'))
$outputXml = [Security.SecurityElement]::Escape($output)
@"
<?xml version="1.0" encoding="utf-8"?>
<configuration><packageSources><clear />
<add key="current-output" value="$outputXml" />
<add key="external-cache" value="$externalFeedXml" />
<add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
</packageSources></configuration>
"@ | Set-Content $buildConfig -Encoding utf8
$manifest = @()
foreach ($project in $ordered) {
    Write-Host "[$($manifest.Count+1)/$($ordered.Count)] $($project.Id) $($project.Version) [$($project.Group)]"
    $log = Join-Path $logs ($project.Id + '.' + $project.Version + '.log')
    & dotnet restore $project.Path --configfile $buildConfig --packages $restorePackages --disable-parallel -m:1 -p:BuildInParallel=false *> $log
    if ($LASTEXITCODE) { Get-Content $log -Tail 25; throw "Restore failed: $($project.Key). See $log" }
    & dotnet pack $project.Path -c $Configuration -o $output --no-restore -m:1 -p:UseSharedCompilation=false -p:BuildInParallel=false -p:GeneratePackageOnBuild=false *>> $log
    if ($LASTEXITCODE) { Get-Content $log -Tail 35; throw "Pack failed: $($project.Key). See $log" }
    $file = Join-Path $output ($project.Id + '.' + $project.Version + '.nupkg')
    if (!(Test-Path $file)) { throw "Package missing: $file" }
    $manifest += [pscustomobject]@{ Order=$manifest.Count+1; PackageId=$project.Id; Version=$project.Version; SourceGroup=$project.Group; File=$file; SHA256=(Get-FileHash $file -Algorithm SHA256).Hash; Project=$project.Path }
}
$manifest | Export-Csv (Join-Path $output 'publish-manifest.csv') -NoTypeInformation -Encoding utf8
$outputXml = [Security.SecurityElement]::Escape($output)
$externalXml = [Security.SecurityElement]::Escape($external)
$externalSource = if ($KeepBuildCache) { '<add key="external-cache" value="' + $externalXml + '" />' } else { '' }
@"
<?xml version="1.0" encoding="utf-8"?>
<configuration><packageSources><clear />
<add key="current-branch" value="$outputXml" />
$externalSource
<add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
</packageSources></configuration>
"@ | Set-Content (Join-Path $output 'NuGet.Config') -Encoding utf8
# Keep release output small; retain diagnostics/cache only on failure or explicit request.
if (!$KeepBuildCache) {
    $outputBoundary = $output.TrimEnd([IO.Path]::DirectorySeparatorChar) + [IO.Path]::DirectorySeparatorChar
    foreach ($temporaryPath in @($restorePackages, $external, $logs, $buildConfig)) {
        $absoluteTemporaryPath = [IO.Path]::GetFullPath($temporaryPath)
        if (!$absoluteTemporaryPath.StartsWith($outputBoundary, [StringComparison]::OrdinalIgnoreCase)) {
            throw "Refusing cleanup outside package output: $absoluteTemporaryPath"
        }
        if (Test-Path -LiteralPath $absoluteTemporaryPath) {
            Remove-Item -LiteralPath $absoluteTemporaryPath -Recurse -Force
        }
    }
}
Write-Host "Packed $($manifest.Count) NET 10 library packages; legacy 8/9, samples and executable endpoints excluded. See publish-manifest.csv."
