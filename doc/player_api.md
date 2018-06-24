# Player API specifications

From the working TS library:

```typescript
// Types

declare class Point {
    x: number;
    y: number;
}

declare class RoomPosition {
    pos: Point;
    roomPos: Point;
}

declare enum Direction {
    None = 0,
    North = 1,
    East = 2,
    South = 3,
    West = 4
}

declare interface GameEntityRef {
    id: string;
    type: string;
    teamId: string;
    pos: RoomPosition;
}

declare interface BotCoreRef {
    type: string;
    qualities: any;
    drain: number;
    flags: number;
    size: number;
    call(name: string, ...args: any[]);
}

declare interface BotEntityRef extends GameEntityRef {
    cores: BotCoreRef[];
    model: string;

    coreCapacity: number;
    reactorPower: number;
    usedCoreSpace: number;
    coreDrain: number;
    move(direction: Direction);
}

// Modules

declare namespace Game {
    function push(data: any, type?: string): boolean;
    function getId(): string;
}

declare namespace Utils {
    function pointDistance(p1: Point, p2: Point): number;
    function distance(p1: RoomPosition, p2: RoomPosition): number;
    function findPath(start: RoomPosition, end: RoomPosition): RoomPosition[];
    function posDirection(source: RoomPosition, dest: RoomPosition): Direction;
}

declare namespace Army {
    function boot(): boolean;
    function getFactory(id?: string): GameEntityRef;
    function getBot(id?: string): BotEntityRef;
    function constructBot(template: string, factory: GameEntityRef): BotEntityRef;
    function deconstructBot(bot: BotEntityRef, factory: GameEntityRef): boolean;
    function installCore(template: string, bot: BotEntityRef): BotCoreRef;
    function getUnits(): any[];
    function getResources(): any;
}

declare namespace Map {
    function findTiles(name: string, roomPos: RoomPosition): RoomPosition[];
}
```
