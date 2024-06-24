## WinExport, a tool to export and restore the settings of Windows in a few clicks

| [Features](#features) | [Instructions for use](#instructions-for-use) | [Feedback](#feedback) | [Acknowledgements](#acknowledgements) | [Contact](#contact) |
| :--: | :--: | :--: | :--: | :--: |

![WinExport v0.1 GUI](https://github.com/Tux528/WinExport/assets/155831438/d7cbe3d0-078c-411b-aab5-0736bc0e7d17)

# Features

- Supports a total of 54 settings, such as appearance, behavioral, and hardware settings, and much more.
- Supports a wide range of system settings, including those of system applications like Windows Update or the File Explorer.
- Exports settings in just a few seconds to a single file.
- Accurately exports settings without including dynamic and conflicting data.
- Allows you to import and export a configuration file to select quickly specific settings.
- Automatically loads and saves the user's configuration at software startup and closing.
- Free, open-source, and portable.

# Detailed comparison with Appcopier and CloneApp
| **[WinExport](https://github.com/Tux528/WinExport) ([Tux 528](https://github.com/Tux528))** | **[Appcopier](https://github.com/builtbybel/Appcopier) ([Belim](https://github.com/Belim))** | **[CloneApp](https://github.com/builtbybel/CloneApp) ([Belim](https://github.com/Belim))** |
| :--- | :--: | :--: | :--: |
| Total number of supported settings | 54[^1] | 23[^2] | 249[^3] |
| Accurately exports settings without including dynamic and conflicting data? [^4] | ✔️ | ❌ | ❌ |
| Exports settings in a single file? | ✔️ | ❌ | ❌ |
| Supports a wide range of system settings? | ✔️ | ❌ | ❌ |
| Is designed to support the settings of Windows 10? | ✔️ | ❌ | ✔️ |
| Supports the settings of Windows 11? | ⚠️[^5] | ✔️ | ❌ |
| Supports the settings of third-party software? | ❌ | ✔️ | ✔️ |
| Is still working? | ✔️ | ✔️ | ⚠️[^6] |
| Is still maintained? | ✔️ | ✔️ | ❌ |
| Is open-source? | ✔️ | ✔️ | ❌ |
| Available in portable version? | ✔️ | ✔️ | ✔️ |
| Exports the pictures used by the Windows wallpapers and themes? | ❌ | ✔️ | ✔️ |
| Includes useful options (destination directory for exported settings, automatic saving of selected settings on closing...)? | ✔️ | ❌ | ✔️ |

Legend:

- ✔️: Yes
- ❌: No
- ⚠️: Partially

[^1]: Mostly system settings, but also the ones of system apps like Windows Update, Windows Defender, or the File Explorer.

[^2]: System and third-party software settings.

[^3]: Mostly third-party settings of software and some system settings.

[^4]: The "Dynamic and conflicting data" expression refers to technical items such as file paths, registry keys, dates, hashes, and unique identifiers used exclusively by your configuration. Unfortunately, these items are exported by Appcopier and CloneApp with the chosen settings, posing a significant risk to the targeted configuration. Indeed, a change of username or the absence of specific software can lead to conflicts and more or less annoying bugs on the system. Given the complexity of Windows, it is impossible to predict how these values will be handled by system components. In the best-case scenario, this may result in minor malfunctions and error messages, but in the worst-case scenario, you'll end up with inoperable and unstable components, and you'll be forced to repair or reinstall Windows. To prevent this stability problem, WinExport exports all registry keys selectively, including only necessary settings and excluding dynamic and conflicting data, guaranteeing a safer method in the long term.

[^5]: WinExport doesn't officially support the Windows 11 settings because this new version of Windows wasn't yet available when I started working on this project in September 2021. However, I plan to support them quickly and efficiently when I work on this project again. 😉

[^6]: CloneApp was updated in 2020 and doesn't support all recent versions of your software, so it's quite possible that it won't export certain settings correctly, especially if the version supported by CloneApp is very old. Don't hesitate to check the description of each option when you select your favorite software to make sure.

## Technical summary

### WinExport

Recommended:

- if you are searching for excellent compatibility with Windows 10;
- if you want to export accurate and hidden settings;
- if you want the settings of your choice to be restored without causing glitches, malfunctions, and conflicts to the system;
- if you are a picky and experimented user;
- if you are searching for a complete, modern, and efficient solution.

### Appcopier

Recommended:

- if you use Windows 11;
- if you want to export the main settings of Windows and your favorite software without spending a lot of time on it.

Not recommended:

- if you use Windows 10;
- if you want to export a lot of system settings;
- if you are searching for a stable and safe solution in the long term.

### CloneApp

Recommended:

- if you use an old version of Windows like Windows 7;
- if you want to export the settings of many software;
- if you have old versions of software to export;
- if you are a picky and experimented user.

Not recommended:

- if you use recent versions of Windows and your favorite software;
- if you want every setting of your favorite software to be correctly restored on the target configuration.

# Instructions for use

1. Download the latest version of the software from the Releases page (here, version 0.2 with an updated interface).
2. Extract the project archive and start the "WinExport.exe" file located in the "WinExport" folder.
3. Tick the settings of your choice in the CCleaner-style side menu.
4. Select a destination directory for the settings file by clicking on the "Browse..." button.
5. Click on the "Export Selected Settings" button and wait a little.
6. Double-click on the "Settings.reg" file to restore your settings with the Registry Editor. Feel free to copy, move, or send this file wherever you like to easily restore your settings on the targeted configuration. 😉

# Feedback

Feel free to send me all your suggestions and bug reports on this repository by [opening an issue](https://github.com/Tux528/WinExport/issues/new/choose) or sending me a message on Discord. 😁

I'll be glad to take your feedback into account to improve this project more and more so that each user can export and restore a unique configuration. 🤝

# Acknowledgements

Many thanks to Skunk1966 and TheDutchJewel for their continuous help in testing and improving this software. 👍

# Contact

Feel free to contact me on Discord or Nsane Forums if you'd like to share your feedback or just chat 😉:

- Discord: `tux_528`
- [Nsane Forums](https://nsaneforums.com/profile/105674-tux-528/): `Tux 528`
