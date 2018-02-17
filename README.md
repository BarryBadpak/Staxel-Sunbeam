<h1 align="center">Sunbeam</h1>
<p align="center">
  <a href="http://steamcommunity.com/sharedfiles/filedetails/?id=1293655054">
    <img src="https://img.shields.io/badge/Steam-release-blue.svg?style=flat" alt="v1.0.9.1" />
  </a>
  <a href="../../wiki">
    <img src="https://img.shields.io/badge/documentation-Wiki-4BC51D.svg?style=flat" alt="Documentation" />
  </a>
</p>

<p align="center">
  A shared mod library for Staxel that offers extra functionality to derived mods.
</p>

**Note:** The Sunbeam assembly should never be included with mods based on Sunbeam.

## Features
- Base mod class where extended classes gain access to all of the IModHookV2 methods besides `GameContextInitializeInit`.
Next to the IModHookV2 methods you gain access to the following custom events: `StartMenuUILoaded`, `IngameOverlayUILoaded`.
- Base mod class offers a AssetLoader through which assets from your `/content/mods/ModName/` folder can be loaded easily.
- Includes the [Harmony Library](https://github.com/pardeike/Harmony) by Andreas Pardeike

### Building
To build Sunbeam make sure that all assembly references get modified to their correct path, most of the assembly references
reference assembly's included with Staxel. The Harmony assembly however was built against the latest master commit as of 17-02-2018
due to it having a fix included for .net patching which is was not included in the latest release at that time.