
# Speercs (AutoBotnet) Changelog

## `0.0.5-dev` (In Development)

- performance tweaks and optimizations
    - optimized room pack format to be gzipped binary; this new format is also used in database serialization
    - types from plugins are cached for quick indexed lookup in `ItemRegistry`
- map interaction support
    - drilling support
        - drill using the `CoreDrill`, by calling the `drill` action
        - tiles now have an associated `durability` that represents the amount of drill power (drill core tier) required
        - drilling transfers resources from tiles to an empire
    - `TileDeltaEvent` for when tiles change
- multiple entities can no longer inhabit the same space
    - enforced when creating new bots from factory and in `MobileEntity::move`

## `0.0.4-dev` "Clink" (June 23, 2018)

- major overhaul of client script api
    - new client api modules
        - `MapModule`, `ArmyModule`, `GameApiModule`, `UtilsModule`, `ConstantsModule` replace old globals
    - automatic spawn placement and room generation
    - bot "core" system based on design
        - modular data-driven bot cores
        - cores define stats of bots
        - cores provide custom functions (ex. "drill")
    - realtime and queued push notifications
    - bot reference sandboxing through `ProxyRef` objects: (`GameEntityRef`) that safely forward read-only data fields
- extensibility expansions
    - nearly every "content" part of the game can be added to without need for reflection or hooking
- global real time evented system
    - for connecting to client frames
    - events are automatically restricted to bots sensor range
- improved metrics data control and allow full data export

## `0.0.3-dev` (Unreleased)

- internal engine optimizations
    - store as much data as possible in persistentdata instead of using additional containers
- improved script sandboxing
    - configurable sandbox limits (recursion, code size, timeout time, memory usage)

## `0.0.2-dev` "Illeg" (February 5, 2018)

- Upgrade to ASP.NET Core 2.0
- Significant improvements and fixes in the script execution engine, and support for security measures such as timeouts and recursion limits
- REST API improvements
- Security fix: password change triggers re-generation of API key
- Internal engine improvements
- Completely removed the WebUI in favor of future native desktop-based and terminal-based clients (Will be Windows, macOS, and GNU/Linux)
- Updated documentation, includes metadata endpoints, code deployment, and complete and detailed documentation of authentication endpoints

## `0.0.1-dev` "Pre-Alpha 1" (August 9, 2017)

- Core engine is mostly working
- Working webui and REST services for internal game systems.
- Can be used to execute code, but no user-friendly frontend, only REST calls for now.
