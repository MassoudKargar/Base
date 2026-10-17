[CmdletBinding()]
param(
    [string]$OutputDirectory = (Join-Path $PSScriptRoot '..\artifacts\nuget'),
    [string]$Configuration = 'Release',
    [string]$StartAt,
    [string]$NuGetSource = 'https://api.nuget.org/v3/index.json'
)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$output = [IO.Path]::GetFullPath($OutputDirectory)
New-Item -ItemType Directory -Force -Path $output | Out-Null
$logs = Join-Path $output 'logs'
New-Item -ItemType Directory -Force -Path $logs | Out-Null
# Isolate internal packages from old global-cache copies of the same version.
$cache = Join-Path $output 'packages'
$externalFeed = Join-Path $output 'external-cache'
New-Item -ItemType Directory -Force -Path $externalFeed | Out-Null
$globalCache = if ($env:NUGET_PACKAGES) { $env:NUGET_PACKAGES } else { Join-Path $env:USERPROFILE '.nuget\packages' }
if (Test-Path $globalCache) {
    Get-ChildItem $globalCache -Directory | Where-Object Name -NotLike 'Base.*' | ForEach-Object {
        Get-ChildItem $_.FullName -Recurse -Filter *.nupkg | ForEach-Object {
            $destination = Join-Path $externalFeed $_.Name
            if (!(Test-Path $destination)) { Copy-Item -LiteralPath $_.FullName -Destination $destination }
        }
    }
}
$config = Join-Path $output 'NuGet.Config'
$escape = { param($value) [Security.SecurityElement]::Escape($value) }
@"
<?xml version="1.0" encoding="utf-8"?>
<configuration><packageSources><clear />
<add key="current-base" value="$(& $escape $output)" />
<add key="cached-external" value="$(& $escape $externalFeed)" />
<add key="nuget.org" value="$(& $escape $NuGetSource)" />
</packageSources></configuration>
"@ | Set-Content -LiteralPath $config -Encoding utf8
$projects = @{}
foreach ($folder in @('src', 'Extentsions', 'Utility')) {
    Get-ChildItem (Join-Path $root $folder) -Recurse -Filter *.csproj | Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' -and $_.BaseName -notmatch 'Sample|Tests' } | ForEach-Object {
        [xml]$xml = Get-Content -LiteralPath $_.FullName
        $id = [string]($xml.Project.PropertyGroup.PackageId | Where-Object { $_ } | Select-Object -First 1)
        if (!$id) { $id = $_.BaseName }
        $version = [string]($xml.Project.PropertyGroup.Version | Where-Object { $_ } | Select-Object -First 1)
        if (!$version) { throw "Missing explicit package version: $($_.FullName)" }
        $dependencies = @($xml.SelectNodes('//PackageReference') | ForEach-Object { $_.Include })
        $dependencies += @($xml.SelectNodes('//ProjectReference') | ForEach-Object { [IO.Path]::GetFileNameWithoutExtension($_.Include.Replace('\', '/')) })
        $projects[$id] = [pscustomobject]@{ Id=$id; Version=$version; Path=$_.FullName; Dependencies=$dependencies }
    }
}
$pending = @($projects.Keys | Sort-Object)
$ordered = @()
while ($pending.Count) {
    $ready = @($pending | Where-Object { $id=$_; !@($projects[$id].Dependencies | Where-Object { $pending -contains $_ }).Count })
    if (!$ready.Count) { throw "Internal package dependency cycle: $($pending -join ', ')" }
    foreach ($id in $ready) { $ordered += $projects[$id] }
    $pending = @($pending | Where-Object { $ready -notcontains $_ })
}
# Re-extract our packages on every run; unchanged version numbers must not reuse stale binaries.
if (Test-Path $cache) {
    $cachePrefix = [IO.Path]::GetFullPath($cache).TrimEnd('\') + '\'
    Get-ChildItem -LiteralPath $cache -Directory -Filter 'base.*' | ForEach-Object {
        $target = [IO.Path]::GetFullPath($_.FullName)
        if (!$target.StartsWith($cachePrefix, [StringComparison]::OrdinalIgnoreCase)) { throw "Unsafe cache path: $target" }
        Remove-Item -LiteralPath $target -Recurse -Force
    }
}
$manifest = @()
$started = !$StartAt
if ($StartAt -and $ordered.Id -notcontains $StartAt) { throw "Unknown starting package: $StartAt" }
foreach ($project in $ordered) {
    Write-Host "[$($manifest.Count + 1)/$($ordered.Count)] $($project.Id) $($project.Version)"
    if ($project.Id -eq $StartAt) { $started = $true }
    if (!$started) {
        $file = Join-Path $output ($project.Id + '.' + $project.Version + '.nupkg')
        if (!(Test-Path $file)) { throw "Cannot resume; package missing: $file" }
        $manifest += [pscustomobject]@{ Order=$manifest.Count+1; PackageId=$project.Id; Version=$project.Version; File=$file; SHA256=(Get-FileHash $file -Algorithm SHA256).Hash; Project=$project.Path }
        continue
    }
    $log = Join-Path $logs ($project.Id + '.log')
    & dotnet restore $project.Path --packages $cache --configfile $config --disable-parallel -m:1 -p:BuildInParallel=false -v:minimal *> $log
    if ($LASTEXITCODE) { Get-Content $log -Tail 35; throw "Restore failed: $($project.Id). See $log" }
    & dotnet pack $project.Path -c $Configuration --no-restore -o $output -m:1 -p:BuildInParallel=false -p:UseSharedCompilation=false -p:GeneratePackageOnBuild=false -p:RestorePackagesPath=$cache --nologo -v:minimal *>> $log
    if ($LASTEXITCODE) { Get-Content $log -Tail 45; throw "Pack failed: $($project.Id). See $log" }
    $file = Join-Path $output ($project.Id + '.' + $project.Version + '.nupkg')
    if (!(Test-Path $file)) { throw "Expected package missing: $file" }
    $manifest += [pscustomobject]@{ Order=$manifest.Count+1; PackageId=$project.Id; Version=$project.Version; File=$file; SHA256=(Get-FileHash $file -Algorithm SHA256).Hash; Project=$project.Path }
    $manifest | Export-Csv (Join-Path $output 'publish-manifest.csv') -NoTypeInformation -Encoding utf8
}
Write-Host "Built $($manifest.Count) packages. Publish only files in publish-manifest.csv, in its listed order."
