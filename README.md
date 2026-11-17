# unattend.net

A **.NET Framework 4.8** Windows Forms desktop application for generating Windows 10 and Windows 11 `autounattend.xml` answer files, powered by [Christoph Schneegans' UnattendGenerator](https://github.com/cschneegans/unattend-generator/) library.

## Features

- **Logical Tabbed Interface** — All configuration options organized into 8 clean tabs:
  1. **Language & Region** — Processor architectures (`amd64`, `x86`, `arm64`), Windows display language, up to 3 locales & keyboard layouts, home/geo location, and time zone.
  2. **Edition & Setup** — Windows PE mode (`Default`, `Generate`, `Custom Script`), Windows edition / product key, optional activation key, Windows 11 hardware requirement bypass, BypassNRO, and OOBE express privacy settings.
  3. **Disk & Partitions** — Target disk selection (`Generated` rules, `Fixed` index, `Interactive`, or `VBScript`), GPT/MBR/Automatic partitioning, recovery partition sizing, image selection, paging file, and PE options (Defender, 8.3 names, Compact OS).
  4. **Accounts & Security** — Interactive or unattended local accounts, AutoLogon, password obscuring, password expiration, account lockout policy, process auditing, and AppLocker XML policy.
  5. **System & Network** — Computer name, Wi-Fi profile configuration (SSID/password or WLAN XML), system/security tweaks, and VM guest additions (VirtualBox, VMware, VirtIO, Parallels).
  6. **UI & Personalization** — Explorer/taskbar/Edge tweaks, wallpaper, lock screen, system/app theme & accent color, visual effects, Start pins/tiles, taskbar/desktop icons, Start folders, Narrator, lock keys, and sticky keys.
  7. **Bloatware** — Filterable checklist of 60 pre-installed Windows apps, capabilities, and optional features to remove.
  8. **Custom Scripts** — Add, edit, and remove custom `.cmd`, `.ps1`, `.reg`, and `.vbs` scripts across `System`, `FirstLogon`, `UserOnce`, and `DefaultUser` phases.
- **Live Schema-Validated XML Preview** — Generates and validates `autounattend.xml` against Microsoft's `autounattend.xsd` with ASCII-safe serialization.
- **Profile Management & Last Settings Memory**:
  - Automatically remembers your last settings in `%AppData%\UnattendGeneratorApp\last-settings.json` and restores them on startup.
  - **Save Profile...** and **Load Profile...** export/import portable `.json` configuration profiles.

## Requirements

- Windows 10 / Windows 11 (or Windows 7 SP1+ with .NET Framework 4.8)
- [.NET Framework 4.8 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48) (pre-installed on Windows 10 1903+ and Windows 11)

## Building from Source

Requires the .NET SDK (with .NET Framework 4.8 targeting pack) or Visual Studio 2022+:

```powershell
dotnet build UnattendGenerator.sln -c Release
```

The compiled application will be in:
`src\UnattendGeneratorApp\bin\Release\net48\UnattendGeneratorApp.exe`

To run the smoke test:

```powershell
dotnet run --project tests\UnattendGenerator.SmokeTest\UnattendGenerator.SmokeTest.csproj
```

## Usage

1. Launch `UnattendGeneratorApp.exe`.
2. Configure your desired options across the tabs.
3. Click **Generate Preview** to validate and inspect the generated `autounattend.xml`.
4. Click **Save autounattend.xml...** and place the file in the root directory of your Windows installation USB drive or ISO.

## Credits

- Core XML generation engine & resources: [cschneegans/unattend-generator](https://github.com/cschneegans/unattend-generator/) (MIT License)
