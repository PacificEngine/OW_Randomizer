# Using the Mod
Currently just randomizes the coordinates 

Coordinates are as follows
* 0 - Top-Left
* 1 - Top-Right
* 2 - Right
* 3 - Bottom-Right
* 4 - Bottom-Left
* 5 - Left

# Creating Code
Create a new file called `PacificEngine.OW_Randomizer.csproj.user`
```text/xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="Current" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <OuterWildsRootDirectory>$(OuterWildsDir)\Outer Wilds</OuterWildsRootDirectory>
    <OuterWildsModsDirectory>%AppData%\OuterWildsModManager\OWML\Mods</OuterWildsModsDirectory>
  </PropertyGroup>
</Project>
```