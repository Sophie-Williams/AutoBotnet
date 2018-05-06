
# Entity storage

This game has a lot of entities asladjfsd all around the world.
We want a nice way to store and access these entities with a decent tradeoff between performance and features.

## Ideas

## "Spatial hash" entity storage

In the global EntityBag system, entities are stored according to the room they're in. They are also indexed by user, if they need to be found that way, by storing an ID list in the user. Finally, there's also a list of IDs for the global entity list. We need wrappers to make sure they don't go out of sync and break things.
