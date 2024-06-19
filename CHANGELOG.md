## v1.0.0

### Initial Release!

After some community testing I'm happy to announce the initial release of SlipInfo!

The only known issues so far are that some fields may not be populated correctly when you are crew (not captain) on a ship. Some fields I am aware of:
- 'currentSalvage' from `/getShipInfo` is always 0
- `/getRunInfo` will contain null for all properties except `runId`