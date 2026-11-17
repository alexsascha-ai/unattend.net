using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Schneegans.Unattend;

namespace UnattendGeneratorApp.Models
{
  public sealed class AccountProfileDto
  {
    public string Name { get; set; } = "User";
    public string DisplayName { get; set; } = "";
    public string Password { get; set; } = "";
    public string Group { get; set; } = "Administrators";
  }

  public sealed class ScriptProfileDto
  {
    public ScriptPhase Phase { get; set; } = ScriptPhase.FirstLogon;
    public ScriptType Type { get; set; } = ScriptType.Ps1;
    public string Content { get; set; } = "";
  }

  public sealed class AppProfile
  {
    public int SchemaVersion { get; set; } = 1;

    // 1. Language & Region
    public bool ArchX86 { get; set; }
    public bool ArchAmd64 { get; set; } = true;
    public bool ArchArm64 { get; set; }

    public string LanguageMode { get; set; } = "Interactive"; // Interactive | Unattended
    public string ImageLanguageId { get; set; } = "en-US";
    public string Locale1Id { get; set; } = "en-US";
    public string Keyboard1Id { get; set; } = "00000409";
    public bool UseLocale2 { get; set; }
    public string Locale2Id { get; set; } = "en-US";
    public string Keyboard2Id { get; set; } = "00000409";
    public bool UseLocale3 { get; set; }
    public string Locale3Id { get; set; } = "en-US";
    public string Keyboard3Id { get; set; } = "00000409";
    public string GeoLocationId { get; set; } = "244";

    public string TimeZoneMode { get; set; } = "Implicit"; // Implicit | Explicit
    public string TimeZoneId { get; set; } = "UTC";

    // 2. Edition & Setup
    public string PEMode { get; set; } = "Default"; // Default | Generate | Script
    public string PEScript { get; set; } = "";
    public string EditionMode { get; set; } = "Interactive"; // Interactive | Firmware | Unattended | Custom
    public string WindowsEditionId { get; set; } = "pro";
    public string CustomEditionKey { get; set; } = "";
    public string ActivationKey { get; set; } = "";
    public bool BypassRequirementsCheck { get; set; } = true;
    public bool UseConfigurationSet { get; set; }
    public bool BypassNetworkCheck { get; set; }
    public ExpressSettingsMode ExpressSettings { get; set; } = ExpressSettingsMode.DisableAll;

    // 3. Disk & Partitions (for Generate PE mode)
    public string TargetDiskMode { get; set; } = "Generated"; // Interactive | Fixed | Generated | Script
    public int FixedDiskIndex { get; set; } = 0;
    public string TargetDiskScript { get; set; } = "";
    public int? GeneratedMinSizeGiB { get; set; } = 10;
    public int? GeneratedMaxSizeGiB { get; set; }
    public int? GeneratedDiskIndex { get; set; } = 0;
    public bool AssertNoPartitions { get; set; } = true;
    public bool AssertInterfaceType { get; set; }
    public bool AssertMediaType { get; set; }

    public string PartitionMode { get; set; } = "Unattended"; // Interactive | Unattended | Custom
    public PartitionLayout PartitionLayout { get; set; } = PartitionLayout.GPT;
    public RecoveryMode RecoveryMode { get; set; } = RecoveryMode.Partition;
    public int SystemPartitionSize { get; set; } = 100;
    public int RecoveryPartitionSize { get; set; } = 1000;
    public string CustomPartitionScript { get; set; } = "";

    public string InstallFromMode { get; set; } = "Edition"; // Interactive | Edition | Index | Name
    public int InstallFromIndex { get; set; } = 1;
    public string InstallFromName { get; set; } = "";

    public string PagingFileMode { get; set; } = "Automatic"; // Automatic | None | Custom
    public int PagingInitialMiB { get; set; } = 4096;
    public int PagingMaxMiB { get; set; } = 4096;

    public bool DisableDefender { get; set; }
    public bool Disable8Dot3Names { get; set; }
    public bool PauseBeforeFormatting { get; set; }
    public bool PauseBeforeReboot { get; set; }
    public bool CompactOs { get; set; }
    public bool SkipIntegrityCheck { get; set; }

    // 4. Accounts & Security
    public string AccountMode { get; set; } = "InteractiveMicrosoft"; // InteractiveMicrosoft | InteractiveLocal | Unattended
    [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
    public List<AccountProfileDto> Accounts { get; set; } = new()
    {
      new AccountProfileDto { Name = "Admin", DisplayName = "", Password = "", Group = "Administrators" }
    };
    public string AutoLogonMode { get; set; } = "None"; // None | Builtin | Own
    public string BuiltinAdminPassword { get; set; } = "";
    public bool ObscurePasswords { get; set; }

    public string PasswordExpirationMode { get; set; } = "Default"; // Default | Unlimited | Custom
    public int PasswordMaxAgeDays { get; set; } = 42;

    public string LockoutMode { get; set; } = "Default"; // Default | Disable | Custom
    public int LockoutThreshold { get; set; } = 5;
    public int LockoutDurationMinutes { get; set; } = 30;
    public int LockoutWindowMinutes { get; set; } = 30;

    public string ProcessAuditMode { get; set; } = "Disabled"; // Disabled | Enabled
    public bool ProcessAuditIncludeCommandLine { get; set; }

    public string AppLockerMode { get; set; } = "Skip"; // Skip | Configure
    public string AppLockerPolicyXml { get; set; } = "";

    // 5. System & Network
    public string ComputerNameMode { get; set; } = "Random"; // Random | Prompt | Custom | Script
    public string CustomComputerName { get; set; } = "PC-01";
    public string ComputerNameScript { get; set; } = "";

    public string WifiMode { get; set; } = "Interactive"; // Interactive | Skip | Parameterized | Xml
    public string WifiName { get; set; } = "";
    public string WifiPassword { get; set; } = "";
    public bool WifiConnectAutomatically { get; set; } = true;
    public WifiAuthentications WifiAuthentication { get; set; } = WifiAuthentications.WPA2PSK;
    public bool WifiNonBroadcast { get; set; }
    public string WifiProfileXml { get; set; } = "";

    public bool EnableLongPaths { get; set; }
    public bool EnableRemoteDesktop { get; set; }
    public bool HardenSystemDriveAcl { get; set; }
    public bool DeleteJunctions { get; set; }
    public bool AllowPowerShellScripts { get; set; }
    public bool DisableLastAccess { get; set; }
    public bool PreventAutomaticReboot { get; set; }
    public bool DisableSac { get; set; }
    public bool DisableUac { get; set; }
    public bool DisableSmartScreen { get; set; }
    public bool DisableSystemRestore { get; set; }
    public bool DisableFastStartup { get; set; }
    public bool TurnOffSystemSounds { get; set; }
    public bool PreventDeviceEncryption { get; set; }
    public bool DisableWindowsUpdate { get; set; }
    public bool DeleteWindowsOld { get; set; }
    public bool DisableCoreIsolation { get; set; }
    public bool DisableAutomaticRestartSignOn { get; set; }
    public bool DisableWpbt { get; set; }
    public bool PreventDeviceApps { get; set; }
    public bool KeepSensitiveFiles { get; set; }
    public bool HidePowerShellWindows { get; set; }

    public bool VBoxGuestAdditions { get; set; }
    public bool VMwareTools { get; set; }
    public bool VirtIoGuestTools { get; set; }
    public bool ParallelsTools { get; set; }

    // 6. UI & Personalization
    public bool ClassicContextMenu { get; set; }
    public bool LeftTaskbar { get; set; }
    public bool HideTaskViewButton { get; set; }
    public bool ShowFileExtensions { get; set; }
    public bool ShowAllTrayIcons { get; set; }
    public HideModes HideFiles { get; set; } = HideModes.Hidden;
    public bool HideEdgeFre { get; set; }
    public bool DisableEdgeStartupBoost { get; set; }
    public bool MakeEdgeUninstallable { get; set; }
    public bool DeleteEdgeDesktopIcon { get; set; }
    public bool LaunchToThisPC { get; set; }
    public bool DisableAppSuggestions { get; set; }
    public bool DisableWidgets { get; set; }
    public bool DisableBingResults { get; set; }
    public bool DisablePointerPrecision { get; set; }
    public bool ShowEndTask { get; set; }
    public bool HideInfoTip { get; set; }
    public TaskbarSearchMode TaskbarSearch { get; set; } = TaskbarSearchMode.Box;

    public string WallpaperMode { get; set; } = "Default"; // Default | Solid | Script
    public string WallpaperColorHex { get; set; } = "#000000";
    public string WallpaperScript { get; set; } = "";

    public string LockScreenMode { get; set; } = "Default"; // Default | Script
    public string LockScreenScript { get; set; } = "";

    public string ColorMode { get; set; } = "Default"; // Default | Custom
    public ColorTheme SystemTheme { get; set; } = ColorTheme.Dark;
    public ColorTheme AppsTheme { get; set; } = ColorTheme.Dark;
    public bool EnableTransparency { get; set; } = true;
    public bool AccentColorOnStart { get; set; }
    public bool AccentColorOnBorders { get; set; }
    public string AccentColorHex { get; set; } = "#0078D4";

    public string EffectsMode { get; set; } = "Default"; // Default | BestPerformance | BestAppearance | Custom
    public Dictionary<Effect, bool> CustomEffects { get; set; } = new();

    public string StartPinsMode { get; set; } = "Default"; // Default | Empty | Custom
    public string CustomStartPinsJson { get; set; } = "";

    public string StartTilesMode { get; set; } = "Default"; // Default | Empty | Custom
    public string CustomStartTilesXml { get; set; } = "";

    public string TaskbarIconsMode { get; set; } = "Default"; // Default | Empty | Custom
    public string CustomTaskbarIconsXml { get; set; } = "";

    public string DesktopIconsMode { get; set; } = "Default"; // Default | Custom
    public Dictionary<string, bool> CustomDesktopIcons { get; set; } = new();

    public string StartFoldersMode { get; set; } = "Default"; // Default | Custom
    public Dictionary<string, bool> CustomStartFolders { get; set; } = new();

    public bool UseNarrator { get; set; }
    public string LockKeysMode { get; set; } = "Skip"; // Skip | Configure
    public LockKeyInitial CapsLockInitial { get; set; } = LockKeyInitial.Off;
    public LockKeyBehavior CapsLockBehavior { get; set; } = LockKeyBehavior.Toggle;
    public LockKeyInitial NumLockInitial { get; set; } = LockKeyInitial.Off;
    public LockKeyBehavior NumLockBehavior { get; set; } = LockKeyBehavior.Toggle;
    public LockKeyInitial ScrollLockInitial { get; set; } = LockKeyInitial.Off;
    public LockKeyBehavior ScrollLockBehavior { get; set; } = LockKeyBehavior.Toggle;

    public string StickyKeysMode { get; set; } = "Default"; // Default | Disabled | Custom
    public List<StickyKeys> CustomStickyKeysFlags { get; set; } = new();

    // 7. Bloatware
    public List<string> SelectedBloatwareIds { get; set; } = new();

    // 8. Scripts
    public bool RestartExplorer { get; set; }
    public List<ScriptProfileDto> Scripts { get; set; } = new();

    public Configuration ToConfiguration(UnattendGenerator generator)
    {
      var archs = ImmutableHashSet.CreateBuilder<ProcessorArchitecture>();
      if (ArchX86) archs.Add(ProcessorArchitecture.x86);
      if (ArchAmd64) archs.Add(ProcessorArchitecture.amd64);
      if (ArchArm64) archs.Add(ProcessorArchitecture.arm64);
      if (archs.Count == 0)
      {
        throw new ConfigurationException("At least one processor architecture must be selected.");
      }

      ILanguageSettings languageSettings = LanguageMode == "Unattended"
        ? new UnattendedLanguageSettings(
            ImageLanguage: generator.Lookup<ImageLanguage>(ImageLanguageId),
            LocaleAndKeyboard: new LocaleAndKeyboard(
              generator.Lookup<UserLocale>(Locale1Id),
              generator.Lookup<KeyboardIdentifier>(Keyboard1Id)
            ),
            LocaleAndKeyboard2: UseLocale2
              ? new LocaleAndKeyboard(
                  generator.Lookup<UserLocale>(Locale2Id),
                  generator.Lookup<KeyboardIdentifier>(Keyboard2Id)
                )
              : null,
            LocaleAndKeyboard3: UseLocale3
              ? new LocaleAndKeyboard(
                  generator.Lookup<UserLocale>(Locale3Id),
                  generator.Lookup<KeyboardIdentifier>(Keyboard3Id)
                )
              : null,
            GeoLocation: generator.Lookup<GeoLocation>(GeoLocationId)
          )
        : new InteractiveLanguageSettings();

      ITimeZoneSettings timeZoneSettings = TimeZoneMode == "Explicit"
        ? generator.CreateExplicitTimeZoneSettings(TimeZoneId)
        : new ImplicitTimeZoneSettings();

      IEditionSettings editionSettings = EditionMode switch
      {
        "Firmware" => new FirmwareEditionSettings(),
        "Unattended" => new UnattendedEditionSettings(generator.Lookup<WindowsEdition>(WindowsEditionId)),
        "Custom" => new CustomEditionSettings(new ProductKey(CustomEditionKey)),
        _ => new InteractiveEditionSettings()
      };

      IPESettings peSettings;
      if (PEMode == "Generate")
      {
        ITargetDiskSettings targetDisk = TargetDiskMode switch
        {
          "Interactive" => new InteractiveTargetDiskSettings(),
          "Fixed" => new FixedTargetDiskSettings(FixedDiskIndex),
          "Script" => new ScriptTargetDiskSettings(TargetDiskScript),
          _ => new GeneratedTargetDiskSettings(
            minSizeGiB: GeneratedMinSizeGiB,
            maxSizeGiB: GeneratedMaxSizeGiB,
            index: GeneratedDiskIndex,
            assertNoPartitions: AssertNoPartitions,
            assertInterfaceType: AssertInterfaceType,
            assertMediaType: AssertMediaType
          )
        };

        IPartitionSettings partitionSettings = PartitionMode switch
        {
          "Interactive" => new InteractivePartitionSettings(),
          "Custom" => new CustomPartitionSettings(CustomPartitionScript),
          _ => new UnattendedPartitionSettings(
            TargetDiskSettings: targetDisk,
            PartitionLayout: PartitionLayout,
            RecoveryMode: RecoveryMode,
            SystemSize: SystemPartitionSize,
            RecoverySize: RecoveryPartitionSize
          )
        };

        IInstallFromSettings installFrom = InstallFromMode switch
        {
          "Edition" => new EditionInstallFromSettings(generator.Lookup<WindowsEdition>(WindowsEditionId)),
          "Index" => new IndexInstallFromSettings(InstallFromIndex),
          "Name" => new NameInstallFromSettings(InstallFromName),
          _ => new InteractiveInstallFromSettings()
        };

        IPagingFileSettings pagingFile = PagingFileMode switch
        {
          "None" => new NoPagingFileSettings(),
          "Custom" => new CustomPagingFileSettings(PagingInitialMiB, PagingMaxMiB),
          _ => new AutomaticPagingFileSettings()
        };

        peSettings = new GeneratePESettings(
          PartitionSettings: partitionSettings,
          InstallFromSettings: installFrom,
          PagingFileSettings: pagingFile,
          DisableDefender: DisableDefender,
          Disable8Dot3Names: Disable8Dot3Names,
          PauseBeforeFormatting: PauseBeforeFormatting,
          PauseBeforeReboot: PauseBeforeReboot,
          CompactOs: CompactOs,
          SkipIntegrityCheck: SkipIntegrityCheck
        );
      }
      else if (PEMode == "Script")
      {
        peSettings = new ScriptPESetttings(PEScript);
      }
      else
      {
        peSettings = new DefaultPESettings(
          EditionSettings: editionSettings,
          BypassRequirementsCheck: BypassRequirementsCheck,
          UseConfigurationSet: UseConfigurationSet
        );
      }

      IAccountSettings accountSettings;
      if (AccountMode == "Unattended")
      {
        IAutoLogonSettings autoLogon = AutoLogonMode switch
        {
          "Builtin" => new BuiltinAutoLogonSettings(BuiltinAdminPassword),
          "Own" => new OwnAutoLogonSettings(),
          _ => new NoneAutoLogonSettings()
        };

        var accounts = ImmutableList.CreateRange(
          Accounts.Select(a => new Account(a.Name, a.DisplayName, a.Password, a.Group))
        );
        accountSettings = new UnattendedAccountSettings(accounts, autoLogon, ObscurePasswords);
      }
      else if (AccountMode == "InteractiveLocal")
      {
        accountSettings = new InteractiveLocalAccountSettings();
      }
      else
      {
        accountSettings = new InteractiveMicrosoftAccountSettings();
      }

      IPasswordExpirationSettings passwordExpiration = PasswordExpirationMode switch
      {
        "Unlimited" => new UnlimitedPasswordExpirationSettings(),
        "Custom" => new CustomPasswordExpirationSettings(PasswordMaxAgeDays),
        _ => new DefaultPasswordExpirationSettings()
      };

      ILockoutSettings lockoutSettings = LockoutMode switch
      {
        "Disable" => new DisableLockoutSettings(),
        "Custom" => new CustomLockoutSettings(LockoutThreshold, LockoutDurationMinutes, LockoutWindowMinutes),
        _ => new DefaultLockoutSettings()
      };

      IProcessAuditSettings processAudit = ProcessAuditMode == "Enabled"
        ? new EnabledProcessAuditSettings(ProcessAuditIncludeCommandLine)
        : new DisabledProcessAuditSettings();

      IAppLockerSettings appLocker = AppLockerMode == "Configure"
        ? new ConfigureAppLockerSettings(AppLockerPolicyXml)
        : new SkipAppLockerSettings();

      IComputerNameSettings computerName = ComputerNameMode switch
      {
        "Prompt" => new PromptComputerNameSettings(),
        "Custom" => new CustomComputerNameSettings(CustomComputerName),
        "Script" => new ScriptComputerNameSettings(ComputerNameScript),
        _ => new RandomComputerNameSettings()
      };

      IWifiSettings wifi = WifiMode switch
      {
        "Skip" => new SkipWifiSettings(),
        "Parameterized" => new ParameterizedWifiSettings(
          Name: WifiName,
          Password: WifiPassword,
          ConnectAutomatically: WifiConnectAutomatically,
          Authentication: WifiAuthentication,
          NonBroadcast: WifiNonBroadcast
        ),
        "Xml" => new XmlWifiSettings(WifiProfileXml),
        _ => new InteractiveWifiSettings()
      };

      IWallpaperSettings wallpaper = WallpaperMode switch
      {
        "Solid" => new SolidWallpaperSettings(ColorTranslator.FromHtml(WallpaperColorHex)),
        "Script" => new ScriptWallpaperSettings(WallpaperScript),
        _ => new DefaultWallpaperSettings()
      };

      ILockScreenSettings lockScreen = LockScreenMode == "Script"
        ? new ScriptLockScreenSettings(LockScreenScript)
        : new DefaultLockScreenSettings();

      IColorSettings colorSettings = ColorMode == "Custom"
        ? new CustomColorSettings(
            SystemTheme: SystemTheme,
            AppsTheme: AppsTheme,
            EnableTransparency: EnableTransparency,
            AccentColorOnStart: AccentColorOnStart,
            AccentColorOnBorders: AccentColorOnBorders,
            AccentColor: ColorTranslator.FromHtml(AccentColorHex)
          )
        : new DefaultColorSettings();

      IEffects effects = EffectsMode switch
      {
        "BestPerformance" => new BestPerformanceEffects(),
        "BestAppearance" => new BestAppearanceEffects(),
        "Custom" => new CustomEffects(
          Enum.GetValues(typeof(Effect))
            .Cast<Effect>()
            .ToImmutableDictionary(
              e => e,
              e => CustomEffects.TryGetValue(e, out bool v) && v
            )
        ),
        _ => new DefaultEffects()
      };

      IStartPinsSettings startPins = StartPinsMode switch
      {
        "Empty" => new EmptyStartPinsSettings(),
        "Custom" => new CustomStartPinsSettings(CustomStartPinsJson),
        _ => new DefaultStartPinsSettings()
      };

      IStartTilesSettings startTiles = StartTilesMode switch
      {
        "Empty" => new EmptyStartTilesSettings(),
        "Custom" => new CustomStartTilesSettings(CustomStartTilesXml),
        _ => new DefaultStartTilesSettings()
      };

      ITaskbarIcons taskbarIcons = TaskbarIconsMode switch
      {
        "Empty" => new EmptyTaskbarIcons(),
        "Custom" => new CustomTaskbarIcons(CustomTaskbarIconsXml),
        _ => new DefaultTaskbarIcons()
      };

      IDesktopIconSettings desktopIcons = DesktopIconsMode == "Custom"
        ? new CustomDesktopIconSettings(
            generator.DesktopIcons.Values.ToDictionary(
              d => d,
              d => CustomDesktopIcons.TryGetValue(d.Id, out bool v) && v
            )
          )
        : new DefaultDesktopIconSettings();

      IStartFolderSettings startFolders = StartFoldersMode == "Custom"
        ? new CustomStartFolderSettings(
            generator.StartFolders.Values.ToDictionary(
              f => f,
              f => CustomStartFolders.TryGetValue(f.Id, out bool v) && v
            )
          )
        : new DefaultStartFolderSettings();

      ILockKeySettings lockKeys = LockKeysMode == "Configure"
        ? new ConfigureLockKeySettings(
            new LockKeySetting(CapsLockInitial, CapsLockBehavior),
            new LockKeySetting(NumLockInitial, NumLockBehavior),
            new LockKeySetting(ScrollLockInitial, ScrollLockBehavior)
          )
        : new SkipLockKeySettings();

      IStickyKeysSettings stickyKeys = StickyKeysMode switch
      {
        "Disabled" => new DisabledStickyKeysSettings(),
        "Custom" => new CustomStickyKeysSettings(new HashSet<StickyKeys>(CustomStickyKeysFlags)),
        _ => new DefaultStickyKeysSettings()
      };

      var bloatwares = ImmutableList.CreateRange(
        SelectedBloatwareIds
          .Where(id => generator.Bloatwares.ContainsKey(id))
          .Select(id => generator.Lookup<Bloatware>(id))
      );

      var scripts = Scripts.Select(s => new Script(s.Content, s.Phase, s.Type)).ToList();

      return Configuration.Default with
      {
        ProcessorArchitectures = archs.ToImmutable(),
        LanguageSettings = languageSettings,
        TimeZoneSettings = timeZoneSettings,
        PESettings = peSettings,
        ActivationKey = string.IsNullOrWhiteSpace(ActivationKey) ? null : new ProductKey(ActivationKey.Trim()),
        BypassNetworkCheck = BypassNetworkCheck,
        ExpressSettings = ExpressSettings,
        AccountSettings = accountSettings,
        PasswordExpirationSettings = passwordExpiration,
        LockoutSettings = lockoutSettings,
        ProcessAuditSettings = processAudit,
        AppLockerSettings = appLocker,
        ComputerNameSettings = computerName,
        WifiSettings = wifi,
        EnableLongPaths = EnableLongPaths,
        EnableRemoteDesktop = EnableRemoteDesktop,
        HardenSystemDriveAcl = HardenSystemDriveAcl,
        DeleteJunctions = DeleteJunctions,
        AllowPowerShellScripts = AllowPowerShellScripts,
        DisableLastAccess = DisableLastAccess,
        PreventAutomaticReboot = PreventAutomaticReboot,
        DisableSac = DisableSac,
        DisableUac = DisableUac,
        DisableSmartScreen = DisableSmartScreen,
        DisableSystemRestore = DisableSystemRestore,
        DisableFastStartup = DisableFastStartup,
        TurnOffSystemSounds = TurnOffSystemSounds,
        PreventDeviceEncryption = PreventDeviceEncryption,
        DisableWindowsUpdate = DisableWindowsUpdate,
        DeleteWindowsOld = DeleteWindowsOld,
        DisableCoreIsolation = DisableCoreIsolation,
        DisableAutomaticRestartSignOn = DisableAutomaticRestartSignOn,
        DisableWpbt = DisableWpbt,
        PreventDeviceApps = PreventDeviceApps,
        KeepSensitiveFiles = KeepSensitiveFiles,
        HidePowerShellWindows = HidePowerShellWindows,
        VBoxGuestAdditions = VBoxGuestAdditions,
        VMwareTools = VMwareTools,
        VirtIoGuestTools = VirtIoGuestTools,
        ParallelsTools = ParallelsTools,
        ClassicContextMenu = ClassicContextMenu,
        LeftTaskbar = LeftTaskbar,
        HideTaskViewButton = HideTaskViewButton,
        ShowFileExtensions = ShowFileExtensions,
        ShowAllTrayIcons = ShowAllTrayIcons,
        HideFiles = HideFiles,
        HideEdgeFre = HideEdgeFre,
        DisableEdgeStartupBoost = DisableEdgeStartupBoost,
        MakeEdgeUninstallable = MakeEdgeUninstallable,
        DeleteEdgeDesktopIcon = DeleteEdgeDesktopIcon,
        LaunchToThisPC = LaunchToThisPC,
        DisableAppSuggestions = DisableAppSuggestions,
        DisableWidgets = DisableWidgets,
        DisableBingResults = DisableBingResults,
        DisablePointerPrecision = DisablePointerPrecision,
        ShowEndTask = ShowEndTask,
        HideInfoTip = HideInfoTip,
        TaskbarSearch = TaskbarSearch,
        WallpaperSettings = wallpaper,
        LockScreenSettings = lockScreen,
        ColorSettings = colorSettings,
        Effects = effects,
        StartPinsSettings = startPins,
        StartTilesSettings = startTiles,
        TaskbarIcons = taskbarIcons,
        DesktopIcons = desktopIcons,
        StartFolderSettings = startFolders,
        UseNarrator = UseNarrator,
        LockKeySettings = lockKeys,
        StickyKeysSettings = stickyKeys,
        Bloatwares = bloatwares,
        ScriptSettings = new ScriptSettings(scripts, RestartExplorer)
      };
    }
  }

  public static class ProfileManager
  {
    public static string LastSettingsPath => Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
      "UnattendGeneratorApp",
      "last-settings.json"
    );

    public static void Save(AppProfile profile, string filePath)
    {
      string? dir = Path.GetDirectoryName(filePath);
      if (!string.IsNullOrEmpty(dir))
      {
        Directory.CreateDirectory(dir);
      }
      string json = JsonConvert.SerializeObject(profile, Formatting.Indented);
      File.WriteAllText(filePath, json);
    }

    public static AppProfile Load(string filePath)
    {
      string json = File.ReadAllText(filePath);
      return JsonConvert.DeserializeObject<AppProfile>(json) ?? new AppProfile();
    }

    public static AppProfile LoadLastOrDefault()
    {
      try
      {
        if (File.Exists(LastSettingsPath))
        {
          return Load(LastSettingsPath);
        }
      }
      catch
      {
        // Fall back to defaults if last-settings.json is corrupted
      }
      return new AppProfile();
    }

    public static void SaveLast(AppProfile profile)
    {
      try
      {
        Save(profile, LastSettingsPath);
      }
      catch
      {
        // Ignore persistence errors on exit
      }
    }
  }
}
