
# Speercs (AutoBotnet) Changelog

## `0.0.3-dev` [TBD] (Unreleased)

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
