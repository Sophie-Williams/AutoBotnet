
# AutoBotnet

<img src="media/logo.png" width="128" height="128" />

**AutoBotnet - The Game**

## Documentation

Documentation can be found in [doc](doc/)

## Design

Design information can be found in [AutoBotnetDesign](https://github.com/CookieEaters/AutoBotnetDesign).

## Hacking

- Install latest .NET Core tooling/sdk
  - As of the time of writing: [this version](https://github.com/dotnet/core/blob/master/release-notes/download-archives/1.1.1-download.md)
- Clone the repository recursively (`git clone --recursive https://github.com/CookieEaters/AutoBotnet.git`)
- Open `src/server` in your editor (vscode recommended)
- Restore `dotnet` dependencies: `dotnet restore` in `src/server`
- Restore nodejs dependencies for WebUI: `yarn` or `npm i` in `src/server/Speercs.Server/ClientApp`.
- Everything is preconfigured so you can get started right away, default keybindings are <kbd>F5</kbd> to debug, <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>B</kbd> to build.
- For script build, use `./script/build.sh` with development dependencies
already installed. This is used in our CI pipeline.

# License

Copyright © 2017 The CookieEaters. All Rights Reserved.

The source code of AutoBotnet is licensed under the **AGPLv3**, which can
be found in `LICENSE`.

Art and assets are licensed under the **CC BY-SA 4.0**.

Additional information is available in `CREDITS.md`.
