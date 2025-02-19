## v1.0.5

Attempted to fix an issue where after some time the server would stop responding.

## v1.0.4

Updated to support the v1.90.0 Release of the game.

And also some internal changes to allow the ability to manage the compatible versions without making a new release every time.

## v1.0.3

Updated to support the 1.80.0 Hotfix 1 version of the game, released on August 18th, 2024.

## v1.0.2

Added game version detection. Will automatically disable ourselves if the game version changes.

## v1.0.1

Fixed an issue when attempting to make requests from another website using JS. Added the correct header for it.

In fun news, making great progress on some OBS overlays using this mod, more to share soon on those.

## v1.0.0

### Initial Release!

After some community testing I'm happy to announce the initial release of SlipInfo!

The only known issues so far are that some fields may not be populated correctly when you are crew (not captain) on a ship. Some fields I am aware of:
- 'currentSalvage' from `/getShipInfo` is always 0
- `/getRunInfo` will contain null for all properties except `runId`