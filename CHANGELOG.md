# Changelog


## 1.1.2
- Added per-ship material requirements: Karve uses Wood, Longship uses Fine Wood, Drakkar uses Yggdrasil Wood
- Raft can no longer be repaired or deconstructed
- Added Default Material / Default Material Amount for unknown/modded ship types
- Added Deconstruct Return Amount config (0-100% slider, default 100%)
- Deconstructed ship resources now go to player inventory
- New config file name
- Can Deconstruct default changed to On

## 1.1.1
- Updated ServerSync to latest version (compression, fragmentation, BufferingSocket)
- Removed duplicate VersionHandshake (conflicted with ConfigSync's VersionCheck)
- Fixed NullReferenceException in ShipCustomize when ship child objects are missing
- Fixed namespace from Settlers.Managers to Shipwright.Managers
- Fixed environment.props for gale profile

## 1.1.0
- Updated for Valheim 1.0.12
- Embedded ServerSync and asset bundle
- Fixed API breaking changes
- Fixed ConfigSync IsLocked null reference

## 1.0.2
- Bog witch update

## 1.0.1
- Check chainloader for Balrond Shipyard, if installed, disabling extra visuals

## 1.0.0
- Initial Release
