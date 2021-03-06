
# AutoBotnet

<img src="media/logo.png" width="128" height="128" />

**AutoBotnet - The Game**

## Documentation

- Documentation can be found in [doc](doc/).
  - See `rest_api.md` for REST API documentation
  - See `player_api.md` for user scripting API documentation
  - See `dev/` for dev notes and documentation

## Design

Design information and documents can be found in [AutoBotnetDesign](https://github.com/CookieEaters/AutoBotnetDesign).

## Hacking

- Install latest .NET Core tooling/sdk
  - Current tooling (at time of writing): [v2.1.2](https://github.com/dotnet/core/blob/master/release-notes/download-archives/2.1.2-sdk-download.md)
- Clone the repository recursively (`git clone --recursive https://github.com/CookieEaters/AutoBotnet.git`)
- Open `src/server` in your editor (vscode recommended)
- Restore `dotnet` dependencies: `dotnet restore` in `src/server`
- Everything is preconfigured so you can get started right away, default keybindings are <kbd>F5</kbd> to debug, <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>B</kbd> to build.
- For script build, use `./script/build.sh` with development dependencies
already installed. This is used in our CI pipeline.

### Docker

`Speercs.Server` has full support for running in Docker. You will have to link in config files using Docker volumes.

Here is an example, running `speercs` in Docker using the development configuration file:

#### Building the Docker image
- Use `./script/build_docker.sh`, it will build the image and auto-tag it with the commit hash.

#### Exporting the Docker image to a server
- Use `./script/export_docker.sh`, it will write a 7z compressed Docker image to `./speercs-docker.tar.7z`
- Upload the image to the server and decompress, then import it with `docker load -i ./speercs-docker.tar`

#### Running
- `docker run --name AutoBotnetLocalDev -p 5000:80 -v $(pwd)/src/server/Speercs.Server/speercs.json:/app/speercs.json speercs`
    - This will forward speercs's port 80 to the local system's port 5000, and link the config file to `/app/speercs.json`.
    - If running on a server, you likely want to use `-p 127.0.0.1:5000:80` to bind only to localhost, for reverse proxying.
    - Use `docker create` to create the container without starting it immediately.
- `docker start AutoBotnetLocalDev` and `docker stop AutoBotnetLocalDev` to start/stop the container.

## Useful Commands
- Run server in development mode: `dotnet run ASPNETCORE_ENVIRONMENT=Development`

## License

Copyright © 2017-2018 ALTiCU, The CookieEaters. All Rights Reserved.

The source code of AutoBotnet is licensed under the **AGPLv3**, which can
be found in `LICENSE`.

Art and assets are licensed under the **CC BY-SA 4.0**.

Additional information about credits and licensing is available in `CREDITS.md`.
