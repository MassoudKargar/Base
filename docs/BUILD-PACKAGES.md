# Package build commands

Run `build-package.bat` from any directory. It prefers PowerShell 7, falls back to Windows PowerShell, and returns a nonzero exit code on failure.

Default output: `artifacts/branch-nuget` relative to the Base repository.

- Includes net10.0 libraries from the active `Base.sln`, including its current extension/BuildingBlocks projects, plus the four migrated Utility libraries (Auth, OpenTelemetry, Serilog and SoftwarePartsDetector). Swagger Utility is excluded.
- The old `Extentsions` tree is not scanned or packaged. Its replacements are maintained in the active Base projects.
- Excludes Sample/Samples and Test/Tests projects, Web/Worker SDK executable projects, explicit non-packable projects, and other executable outputs.
- Preserves each selected project's PackageId and Version. Remaining legacy net8.0/net9.0 libraries in Extentsions/Utility remain in source but are excluded from packaging and generated release output.
- IDs with different versions coexist; exact duplicate IDs/versions from different projects fail explicitly.
- Packages in dependency order. Creates and seeds the external-only cache before Restore. Uses a fresh restore cache per run and a build-scoped NuGet config to avoid broken machine-local feeds. Logs are under the output's logs folder. Existing output folders are not deleted.
- A successful run writes publish-manifest.csv with package identity, version, source group, path and SHA256. Use this manifest instead of publishing every historical file by wildcard.

```powershell
.\build-package.bat
.\build-package.bat -ListOnly
.\build-package.bat -Configuration Release -OutputDirectory "artifacts\my-packages"
```

The template repository has a separate batch file:

```powershell
# Only pack the template; does not require published Base packages.
.\build-package.bat
# Also install/generate/restore/build using the local Base feed.
.\build-package.bat -Verify -BasePackageDirectory C:\Base\artifacts\branch-nuget
```

Neither batch file pushes packages to NuGet.org. Existing vulnerability warnings do not mean a clean security audit; see BASEDOTNET10-AUDIT.md.


After a successful run, temporary restore/external caches, build-only configuration and logs are removed. Use -KeepBuildCache to retain them for diagnostics. Failed builds retain their logs and caches. The release NuGet.Config uses the release folder and nuget.org; it does not require the temporary cache.
