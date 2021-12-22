![logo](logo.png)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/PacificEngine/OW_Randomizer?style=flat-square)
![GitHub Release Date](https://img.shields.io/github/release-date/PacificEngine/OW_Randomizer?label=last%20release&style=flat-square)
![GitHub all releases](https://img.shields.io/github/downloads/PacificEngine/OW_Randomizer/total?style=flat-square)
![GitHub release (latest by date)](https://img.shields.io/github/downloads/PacificEngine/OW_Randomizer/latest/total?style=flat-square)

# Randomizer Mod by Pacific Engine

## Installing the Mod
1) Download and install https://outerwildsmods.com/
1) From the Application, Install `PacificEngine's Common Resources` by `PacificEngine`
1) From the Application, Install `Randomizer` by `PacificEngine`

## Using the Mod
### Configurable in Settings
| Value | Description |
| :-: | --- |
Seed | Takes any number or characters and generates a level based upon the value. Empty will just use a random seed.
Eye Coordinates | Choose to randomizes the eye coordinates
Dark Bramble Portals | Choose to randomize the maze of dark bramble
Dark Bramble Vessel | Choose how many portals teleport to the vessel
Dark Bramble Exits | Choose how many portals teleport to the exit
Nomai Warp Platforms | Choose to randomize what warp pads lead where
Ash Twin Project Pads | Choose how many pad will teleport you to the Ash Twin Project
Are Reciever and Transmitter Mirrored | Transmitter and Recievers will always be link to each other
Can Pad's Point To Same Reciever | Will force pad to not have duplicate locations
Can Pad's Point to Same Type | Allows recievers to transmit to recievers and transmitters to transmit to transmitters
Planet Attributes | Randomizes planet orbits, orientation, and rotational speeds

### Different Types of Randomness
| Value | Description |
| :-: | --- |
Off | As the developers original intended
Seed | Use the seed provided
Profile | Makes the seed different between profiles
Death | Makes the seed different between deaths
Minute | Every minute, updates all objects with the random seeded value
On Use | When an object is used, update the object with the random seeded value
Minute + Use | A combination of Minute and On Use
Full On Use | When an object is used, regenerate all objects with the random seeded value
Seedless | Use a random seed
Seedless Minute | Use a new random seed every minute
Seedless Upon Use | When an object is used, update the object with a new random seed
Seedless Minute + Use | A combination of Seedless Minute and Seedless On Use
Seedless Full On Use | When an object is used, regenerate all objects with a new random seed

## Creating Code
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
