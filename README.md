# BugFixesAndQoL Plugin

This plugin introduces several bug fixes and quality of life improvements for the game `Etherium`. It patches issues related to the multiplayer NAT facilitator, movie handling, game state machines, and more, while also allowing configuration changes.

## Features

- Automatically deletes `AVProWindowsMedia-x64.dll` to prevent tutorial crashes.
- Provides patches to prevent the game from double-destroying movies, avoiding related crashes.
- Adds configuration options to modify NAT Facilitator IP and Port for multiplayer.
- Patches several in-game state machines to avoid crashes related to uninitialized states.
- Option to end the player's turn automatically after an invasion is completed.

## Latest Version
Currently the latest publicly available version is 1.3.0
The version currently being developed is 1.4.0

## Installation

1. Download and install [BepInEx](https://www.nexusmods.com/etherium/mods/1) for `Etherium`.
2. Place the plugin's `.dll` file in the `BepInEx/plugins` folder.

## Configuration

Once the plugin is installed, a configuration file will be generated in the `BepInEx/config` folder. You can adjust the following settings:

| Config Key             | Default Value | Description                                                                                  |
|------------------------|---------------|----------------------------------------------------------------------------------------------|
| `General.EndTurnOnInvade`  | `true`        | Automatically end the player's turn after completing an invasion.                             |
| `Multiplayer.NatFacilitatorIP` | `"127.0.0.1"` | The IP address for the NAT Facilitator server.                                                |
| `Multiplayer.NatFacilitatorPort` | `50005`       | The port for the NAT Facilitator server.                                                      |

## Patches

### Movie Handling

The plugin patches the `Scaleform.Movie.Destroy` and `Scaleform.Movie.Finalize` methods to prevent double-destroy issues that can cause crashes. 

### State Machines

Patches the `StateFSM_Deploy`, `HFSMState`, and `FiniteStateMachine` classes to prevent crashes when certain game states are uninitialized. 

### Score Screen

The `GUIScaleformScoreScreen` class is patched to avoid crashes related to uninitialized `InitParams` or improper state flags.

### Singleplayer Campaign

If the `EndTurnOnInvade` configuration is enabled, the player's turn will automatically end after a successful invasion during a campaign.

## How It Works

- The plugin uses the [Harmony](https://harmony.pardeike.net/) library to patch existing methods in the game's code.
- Upon launch, the plugin checks for and removes `AVProWindowsMedia-x64.dll`, which is known to cause crashes during the tutorial.
- It also modifies the NAT Facilitator server address and port according to the user-defined configuration.

## Compatibility

- **Game Version:** Etherium
- **Requires:** BepInEx 5.x

## Known Issues

- None at the moment.

## Logging

The plugin logs important information to the BepInEx log file, You can find the logs in the `BepInEx/LogOutput.log` file.
The game logs are located in the `Etherium_Data/output_log.txt` file.

## Credits

- **Developer:** LostGameDev
- **Frameworks Used:** BepInEx, Harmony

## License

This project is licensed under the GNU General Public License v3.0.
