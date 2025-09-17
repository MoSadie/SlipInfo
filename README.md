# SlipInfo
 Local API mod for [Slipstream: Rogue Space](https://playslipstream.com). Hosts a local HTTP server you can query for information about the current game state.

 ## Requirements

- [Slipstream: Rogue Space (on Steam)](https://playslipstream.com)
- [r2modman](https://thunderstore.io/c/slipstream-rogue-space/p/ebkr/r2modman/)

## (Quick) Setup Video

(video soon!)

## Installation

1) Launch Slipstream at least once.
2) Download and setup r2modman from [here](https://thunderstore.io/c/slipstream-rogue-space/p/ebkr/r2modman/) (Click "Manual Download" and run the setup exe)
3) Select "Slipstream: Rogue Space" from the list of games in r2modman and create a profile.
4) In the "Online" tab look for SlipInfo and click it. Then click "Download"
5) Launch Slipstream using the "Start modded" button to generate the config file.
5) (Optional) Modify the config file using the "Config editor" tab. (see the config file for more details)

## Configuration Options

### port

The port the API will listen on. Default is 8001.

### prefix

The prefix for the API. Default is `/slipinfo`. (Ex `http://localhost:8001/slipinfo/getCrew`)

**If you run into any issues getting this set up, please reach out! Best way is via Discord or GitHub Issues!**

## API Endpoints

### GET /version

Gets the version information _for the mod_. Useful for testing without having to join a ship or sign in, since this works from the title screen.

<details>
<summary>Example Response</summary>

```json
{ "version": "1.0.0.0" }
```

</details>

### GET /getCrew

Returns a list of all crew members on the ship.

<details>
<summary>Example Response</summary>

```json
{
    "crewList":[
        {
            "name":"MoSadie",
            "archetype":"hamster",
            "skin":"HamsterWildWest",
            "level":15,
            "xp":87661,
            "currentHealth":40.0,
            "maxHealth":40.0,
            "currentShields":0.0,
            "maxShields":40.0,
            "isCaptain":true,
            "isLocalPlayer":true
        },
        ...
    ]
}
```

</details>

### GET /getCrewByUsername

Returns a crew member by their username.


#### Query Parameters

- `username` (string): The username of the crew member.

<details>
<summary>Example Response</summary>

```json
{
    "crewmate":{
        "name":"MoSadie",
        "archetype":"hamster",
        "skin":"HamsterWildWest",
        "level":15,
        "xp":87661,
        "currentHealth":40.0,
        "maxHealth":40.0,
        "currentShields":0.0,
        "maxShields":40.0,
        "isCaptain":false,
        "isLocalPlayer":true
    }
}
```

</details>

### GET /getSelf

Returns information about the player character. Same information as /getCrewByUsername, but automatically selects the player character.

<details>
<summary>Example Response</summary>

```json
{
    "crewmate":{
        "name":"MoSadie",
        "archetype":"turtle",
        "skin":"TurtleWildWest",
        "level":3,
        "xp":1678,
        "currentHealth":50.0,
        "maxHealth":50.0,
        "currentShields":0.0,
        "maxShields":80.0,
        "isCaptain":false,
        "isLocalPlayer":true
    }
}
```
</details>

### GET /getShipInfo

Returns information about the ship including type, name, ship health, and fuel tanks.

<details>
<summary>Example Response</summary>

```json
{
  "maxHealth": 5400.0,
  "minHealth": 0.0,
  "currentHealth": 5400.0,
  "maxFuel": 10,
  "currentFuel": 0, // Note this may not be accurate if you are not the captain or first mate.
  "currentSalvage": 0, // Note this may not be accurate if you are not the captain or first mate.
  "currentGems": 0,
  "shipTech": [
    {
      "Name": "Scan Booster",
      "ShortDescription": "Improves scan success rate",
      "LongDescription": "Improves scan success rate when exploring.",
      "Level": 1,
      "MaxLevel": 5,
      "IsActive": true,
      "Color": "#40c5c7",
      "Unit": "VALUE",
      "Levels": [
        {
          "Level": 1,
          "Value": 30.0,
          "Cost": 0
        },
        {
          "Level": 2,
          "Value": 50.0,
          "Cost": 5
        },
        {
          "Level": 3,
          "Value": 70.0,
          "Cost": 15
        },
        {
          "Level": 4,
          "Value": 90.0,
          "Cost": 25
        },
        {
          "Level": 5,
          "Value": 99.0,
          "Cost": 50
        }
      ]
    },
    {
      "Name": "Fuel Compressor",
      "ShortDescription": "Increases max fuel cell capacity",
      "LongDescription": "Increases max fuel cell capacity using alien compression technology.",
      "Level": 1,
      "MaxLevel": 5,
      "IsActive": true,
      "Color": "#ca2dca",
      "UnitType": 0,
      "Levels": [
        {
          "Level": 1,
          "Value": 10.0,
          "Cost": 0
        },
        {
          "Level": 2,
          "Value": 12.0,
          "Cost": 10
        },
        {
          "Level": 3,
          "Value": 14.0,
          "Cost": 25
        },
        {
          "Level": 4,
          "Value": 16.0,
          "Cost": 50
        },
        {
          "Level": 5,
          "Value": 20.0,
          "Cost": 75
        }
      ]
    }
  ]
}
```
</details>

### GET /getEnemyShipInfo

Returns information about the enemy ship, if there is one.

<details>
<summary>Example Response</summary>

```json
{
    "enemyShip":{
        "maxHealth":10868.0,
        "minHealth":0.0,
        "currentHealth":3053.50952,
        "name":"FUEL SPEEDER",
        "invaders":"None",
        "intel":"Automated fuel supply ship. Lightly armed but built for extreme speed.",
        "threatLevel":5,
        "cargoLevel":5,
        "speedLevel":10
    }
}
```

</details>

### GET /getRunInfo

Returns information about the current run including the current sector and region.

<details>
<summary>Example Response</summary>

Crew:

```json
{
    "region":null,
    "regionDescription":null,
    "sector":null,
    "runId":0
}
```

Captain:

```json
{
    "region":"PLUTO",
    "regionDescription":"Bleak, barren, and very cold. Two sectors, low threat; a good place to train new crew.",
    "sector":"PLUTO OUTSKIRTS",
    "runId":0
}
```

Between Runs:

```json
{
    "region":"Space",
    "regionDescription":"The vast expanse of space. Perfect place to plan the next adventure!",
    "sector":"The Void",
    "runId":-1
}
```