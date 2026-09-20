> **Disclaimer:**
> Most of mods I work with are some old and outdated ones that their authors haven't updated so far to match 1.0. I am not a professional modder myself, but I am good with coding and gaming. If you experience any problems with the mods I published, you can find me in <a href="https://discord.com/channels/1522110224947871817/1522118606937133136">Hexium</a> discord server by typing DMT.

# Shipwright

A Valheim mod that lets you repair and destroy ships on the go without a crafting station.
[Fork/update of RustyMods' Shipwright](https://thunderstore.io/c/valheim/p/RustyMods/Shipwright/), maintained for Valheim 1.0.x.

**GitHub:** <img height="18" src="https://github.githubassets.com/favicons/favicon-dark.svg"></img> [DaiMinhTri/Shipwright](https://github.com/DaiMinhTri/Shipwright)

## Features

### Ship Repair
Left-click (Mouse0) to repair your ship using the Shipwright Hammer. No crafting station required — repair your vessel right on the water.

### Ship Destruction
Right-click (Mouse3) to deconstruct your ship. Enable the Can Deconstruct config to take apart ships you no longer need.

### Extra Longship Visuals
Optional cosmetic additions for the Viking longship: tent, trader lamp, storage, and shields. Enable each individually via configs.

> **Warning:** Turning on Extra Visuals without [Balrond's Shipyard](https://valheim.thunderstore.io/package/balrond/BalrondShipyard/) installed will crash the game.

### Server-Side Control
Configuration is locked by default and synced from the server using embedded ServerSync. Clients cannot change settings unless the server allows it.

## Configuration

A configuration file is generated at `BepInEx/config/DaiMinhTri.Shipwright.cfg` after the first launch.

| Section | Setting | Default | Description |
|---------|---------|---------|-------------|
| General | Lock Configuration | On | Lock config to server admins only |
| Settings | Default Material | Wood | Fallback material for unknown/modded ship types |
| Settings | Default Material Amount | 1 | Fallback material amount for unknown/modded ship types |
| Settings | Repair Amount | 0.1 | Health percentage per repair |
| Settings | Stamina Cost | 5 | Stamina needed per repair |
| Settings | Repair Duration | 1 | Duration in seconds, multiplied by tool quality |
| Settings | Repair Effects | On | Trigger effects on repair |
| Settings | Can Deconstruct | Off | Allow deconstructing with secondary attack |
| Settings | Deconstruct Duration | 10 | Duration in seconds, multiplied by tool quality |
| Settings | Use Durability | On | Tool uses durability per use |
| Karve | Material | Wood | Material required to repair Karve |
| Karve | Material Amount | 1 | Material amount needed to repair Karve |
| Longship | Material | FineWood | Material required to repair Longship |
| Longship | Material Amount | 2 | Material amount needed to repair Longship |
| Drakkar | Material | YggdrasilWood | Material required to repair Drakkar |
| Drakkar | Material Amount | 3 | Material amount needed to repair Drakkar |
| Longship Visuals | Extra Visuals | Off | Enable extra ship visuals (crashes without Balrond Shipyard) |
| Longship Visuals | Use Tent | Off | Enable tent on longship |
| Longship Visuals | Use Lamp | Off | Enable trader lamp on longship |
| Longship Visuals | Use Storage | Off | Enable storage on longship |
| Longship Visuals | Use Shields | Off | Enable shields on longship |

## Compatibility

### Ship Mods
- **Balrond's Shipyard** - Required if you enable Extra Visuals. The mod detects it automatically and disables extra visuals when installed.

## Installation

1. Install [BepInEx](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)
2. Download and extract `Shipwright.dll` into your `BepInEx/plugins/` folder
3. Launch the game to generate the config file

## Credits
- Original mod by **RustyMods**

## Buy Me a Coffee

If you enjoy this mod, consider buying me a coffee: [![Buy Me A Coffee](https://img.shields.io/badge/Buy%20Me%20A%20Coffee-daiminhtri-yellow)](https://buymeacoffee.com/daiminhtri)

## Changelog

### 1.1.2
- Added per-ship material requirements: Karve uses Wood, Longship uses Fine Wood, Drakkar uses Yggdrasil Wood
- Raft can no longer be repaired or deconstructed
- Added Default Material / Default Material Amount for unknown/modded ship types

### 1.1.1
- Updated ServerSync to latest version (compression, fragmentation, BufferingSocket)
- Removed duplicate VersionHandshake (conflicted with ConfigSync's VersionCheck)
- Fixed NullReferenceException in ShipCustomize when ship child objects are missing
- Fixed namespace from Settlers.Managers to Shipwright.Managers
- Fixed environment.props for gale profile

### 1.1.0
- Updated for Valheim 1.0.12
- Embedded ServerSync and asset bundle
- Fixed API breaking changes
- Fixed ConfigSync IsLocked null reference

### 1.0.2
- Bog witch update

### 1.0.1
- Check chainloader for Balrond Shipyard, if installed, disabling extra visuals

### 1.0.0
- Initial release
