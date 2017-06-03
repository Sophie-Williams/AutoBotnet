# Player API specifications

## Global `Game` object

This object contains properties/methods for accessing the state of the game.

### `bots` property

A dictionary of the user's own [`Bot`s](#bot-objects), where keys are each corresponding bot's identifier string. Could be iterated over like so:

```js
for (var id in Game.bots) {
    var bot = Game.bots[id];
    //...
}
```

## Global constants

 - Directional constants:
   - `NORTH` = `0`
   - `EAST` = `1`
   - `SOUTH` = `2`
   - `WEST` = `3`
 - Return codes:
   - `SUCCESS` = `0`
   - ...

## `Bot` objects

Represents a bot owned by some player.

### `pos` property

A [`RoomPosition`](#roomposition-objects) object, representing the bot's position.

## `RoomPosition` objects

Represents a location (tile and room) in the game world.

### `x` and `y` properties

The X and Y coordinates inside the room (0-63).

### `room` property

The [`Room`](#room-objects) that this position is in.

## `Room` objects

Represents a room in the game world.

...