# Local setup (not for commit)

This file documents local changes made to get the Avalonia app running. These changes are **not intended for upstream PRs** (e.g. .NET 10 upgrade for machines without .NET 9).

## Keep local changes out of commits

To prevent these files from being committed, mark them as "assume unchanged":

```bash
git update-index --assume-unchanged global.json
git update-index --assume-unchanged package.json
git update-index --assume-unchanged src/Angor/Client/global.json
git update-index --assume-unchanged src/Angor/Avalonia/AngorApp.Desktop/Program.cs
git update-index --assume-unchanged src/Angor/Avalonia/AngorApp.Desktop/AngorApp.Desktop.csproj
git update-index --assume-unchanged src/Angor/Avalonia/AngorApp/AngorApp.csproj
git update-index --assume-unchanged src/Angor/Avalonia/AngorApp.Model/AngorApp.Model.csproj
git update-index --assume-unchanged src/Angor/Avalonia/Angor.Sdk/Angor.Sdk.csproj
git update-index --assume-unchanged src/Angor/Avalonia/Angor.Data.Documents/Angor.Data.Documents.csproj
git update-index --assume-unchanged src/Angor/Avalonia/Angor.Data.Documents.LiteDb/Angor.Data.Documents.LiteDb.csproj
git update-index --assume-unchanged src/Angor/Avalonia/AngorApp.Tests/AngorApp.Tests.csproj
git update-index --assume-unchanged src/Angor/Avalonia/Angor.Sdk.Tests/Angor.Sdk.Tests.csproj
git update-index --assume-unchanged src/Angor/Avalonia/AngorApp.Android/AngorApp.Android.csproj
git update-index --assume-unchanged src/Angor/Avalonia/AngorApp.Browser/AngorApp.Browser.csproj
git update-index --assume-unchanged src/Angor/Avalonia/AngorApp.iOS/AngorApp.iOS.csproj
```

To undo (start tracking changes again):

```bash
git update-index --no-assume-unchanged <file>
# or for all:
git update-index --no-assume-unchanged global.json package.json src/Angor/Client/global.json
# ... etc
```
