# SlipInfo
 Local API mod for Slipstream: Rogue Space

## Configuration Options

### port

The port the API will listen on. Default is 8001.

### prefix

The prefix for the API. Default is `/slipinfo`. (Ex `http://localhost:8001/slipinfo/getCrew`)

## API Endpoints

### GET /version

Gets the version information _for the mod_. Useful for testing without having to join a ship, works from the title screen.

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
    "maxHealth":11800.0,
    "minHealth":1888.0,
    "currentHealth":10180.333,
    "maxFuel":16,
    "currentFuel":6,
    "currentSalvage":0, //Note this may not be accurate if not the captain.
    "currentGems":72
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