# SlipInfo
 Local API mod for Slipstream: Rogue Space

## Configuration Options

### port

The port the API will listen on. Default is 8001.

## API Endpoints

### GET /getCrew

Returns a list of all crew members on the ship.

### GET /getCrewByUsername

Returns a crew member by their username.

#### Query Parameters

- `username` (string): The username of the crew member.

### GET /getShipInfo

Returns information about the ship including type, name, ship health, and hull damage.

### GET /getRunInfo

Returns information about the current run including the current sector and region.

### GET /getNodeInfo

Returns information about the current node including the node type, name, and description. (Includes a field for node-specific infromation, such as fights, shops, etc.)