using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Schneegans.Unattend;
using UnattendGeneratorApp.Models;
using UnattendGeneratorApp.UI;

namespace UnattendGeneratorApp
{
  public partial class MainForm
  {
    // Tab 5: System & Network
    private ComboBox _cboComputerNameMode = UiHelpers.CreateDropdown();
    private TextBox _txtCustomComputerName = new() { Width = 220 };
    private TextBox _txtComputerNameScript = new() { Multiline = true, Height = 80, ScrollBars = ScrollBars.Both, Font = new Font("Consolas", 8.5f), WordWrap = false };

    private ComboBox _cboWifiMode = UiHelpers.CreateDropdown();
    private TextBox _txtWifiName = new() { Width = 220 };
    private TextBox _txtWifiPassword = new() { Width = 220, UseSystemPasswordChar = true };
    private ComboBox _cboWifiAuth = UiHelpers.CreateDropdown();
    private CheckBox _chkWifiAuto = new() { Text = "Connect automatically" };
    private CheckBox _chkWifiNonBroadcast = new() { Text = "Connect even if network is not broadcasting (hidden SSID)" };
    private TextBox _txtWifiXml = new() { Multiline = true, Height = 80, ScrollBars = ScrollBars.Both, Font = new Font("Consolas", 8.5f), WordWrap = false };

    private CheckBox _chkEnableLongPaths = new() { Text = "Enable NTFS long paths (>260 chars)" };
    private CheckBox _chkEnableRemoteDesktop = new() { Text = "Enable Remote Desktop (RDP)" };
    private CheckBox _chkHardenAcl = new() { Text = "Harden system drive ACL" };
    private CheckBox _chkDeleteJunctions = new() { Text = "Delete legacy user profile junctions" };
    private CheckBox _chkAllowPsScripts = new() { Text = "Set PowerShell execution policy to RemoteSigned / Bypass" };
    private CheckBox _chkDisableLastAccess = new() { Text = "Disable NTFS Last Access timestamp" };
    private CheckBox _chkPreventAutoReboot = new() { Text = "Prevent automatic Windows Update reboot" };
    private CheckBox _chkDisableSac = new() { Text = "Disable Special Administration Console (SAC)" };
    private CheckBox _chkDisableUac = new() { Text = "Disable User Account Control (UAC)" };
    private CheckBox _chkDisableSmartScreen = new() { Text = "Disable Windows SmartScreen" };
    private CheckBox _chkDisableSystemRestore = new() { Text = "Disable System Restore" };
    private CheckBox _chkDisableFastStartup = new() { Text = "Disable Fast Startup (Hiberboot)" };
    private CheckBox _chkTurnOffSounds = new() { Text = "Turn off Windows system sounds" };
    private CheckBox _chkPreventDeviceEncryption = new() { Text = "Prevent automatic BitLocker / Device Encryption" };
    private CheckBox _chkDisableWindowsUpdate = new() { Text = "Disable Windows Update" };
    private CheckBox _chkDeleteWindowsOld = new() { Text = "Delete Windows.old folder" };
    private CheckBox _chkDisableCoreIsolation = new() { Text = "Disable Core Isolation / Memory Integrity (HVCI)" };
    private CheckBox _chkDisableAutoRestartSignOn = new() { Text = "Disable Automatic Restart Sign-On (ARSO)" };
    private CheckBox _chkDisableWpbt = new() { Text = "Disable Windows Platform Binary Table (WPBT)" };
    private CheckBox _chkPreventDeviceApps = new() { Text = "Prevent OEM/device apps from installing" };
    private CheckBox _chkKeepSensitiveFiles = new() { Text = "Keep sensitive script files after setup" };
    private CheckBox _chkHidePsWindows = new() { Text = "Hide PowerShell windows during setup" };

    private CheckBox _chkVBoxGuest = new() { Text = "VirtualBox Guest Additions" };
    private CheckBox _chkVMwareTools = new() { Text = "VMware Tools" };
    private CheckBox _chkVirtIoGuest = new() { Text = "QEMU / KVM VirtIO Guest Tools" };
    private CheckBox _chkParallelsTools = new() { Text = "Parallels Tools" };

    // Tab 6: UI & Personalization
    private CheckBox _chkClassicContextMenu = new() { Text = "Classic Windows 10 right-click context menu" };
    private CheckBox _chkLeftTaskbar = new() { Text = "Align taskbar to the left (Windows 11)" };
    private CheckBox _chkHideTaskView = new() { Text = "Hide Task View button" };
    private CheckBox _chkShowFileExtensions = new() { Text = "Show file extensions in Explorer" };
    private CheckBox _chkShowAllTrayIcons = new() { Text = "Show all system tray icons" };
    private ComboBox _cboHideFiles = UiHelpers.CreateDropdown();
    private ComboBox _cboTaskbarSearch = UiHelpers.CreateDropdown();
    private CheckBox _chkLaunchToThisPC = new() { Text = "Open File Explorer to 'This PC'" };
    private CheckBox _chkShowEndTask = new() { Text = "Enable 'End Task' in taskbar right-click menu" };
    private CheckBox _chkHideInfoTip = new() { Text = "Hide Explorer folder info tips" };
    private CheckBox _chkDisableAppSuggestions = new() { Text = "Disable Start/app suggestions & ads" };
    private CheckBox _chkDisableWidgets = new() { Text = "Disable Widgets (News and Interests)" };
    private CheckBox _chkDisableBing = new() { Text = "Disable Bing web results in Windows Search" };
    private CheckBox _chkDisablePointerPrecision = new() { Text = "Disable mouse 'Enhance pointer precision'" };
    private CheckBox _chkHideEdgeFre = new() { Text = "Hide Microsoft Edge first-run experience" };
    private CheckBox _chkDisableEdgeBoost = new() { Text = "Disable Edge Startup Boost" };
    private CheckBox _chkMakeEdgeUninstallable = new() { Text = "Make Microsoft Edge uninstallable" };
    private CheckBox _chkDeleteEdgeIcon = new() { Text = "Delete Edge desktop shortcut" };

    private ComboBox _cboWallpaperMode = UiHelpers.CreateDropdown();
    private Button _btnWallpaperColor = new() { Width = 100, Text = "#000000" };
    private TextBox _txtWallpaperScript = new() { Multiline = true, Height = 65, ScrollBars = ScrollBars.Both, Font = new Font("Consolas", 8.5f), WordWrap = false };

    private ComboBox _cboLockScreenMode = UiHelpers.CreateDropdown();
    private TextBox _txtLockScreenScript = new() { Multiline = true, Height = 65, ScrollBars = ScrollBars.Both, Font = new Font("Consolas", 8.5f), WordWrap = false };

    private ComboBox _cboColorMode = UiHelpers.CreateDropdown();
    private ComboBox _cboSystemTheme = UiHelpers.CreateDropdown();
    private ComboBox _cboAppsTheme = UiHelpers.CreateDropdown();
    private CheckBox _chkTransparency = new() { Text = "Enable transparency effects" };
    private CheckBox _chkAccentOnStart = new() { Text = "Show accent color on Start and Taskbar" };
    private CheckBox _chkAccentOnBorders = new() { Text = "Show accent color on title bars and window borders" };
    private Button _btnAccentColor = new() { Width = 100, Text = "#0078D4" };

    private ComboBox _cboEffectsMode = UiHelpers.CreateDropdown();
    private CheckedListBox _chkListEffects = new() { Height = 120, CheckOnClick = true };

    private ComboBox _cboStartPinsMode = UiHelpers.CreateDropdown();
    private TextBox _txtStartPinsJson = new() { Multiline = true, Height = 65, ScrollBars = ScrollBars.Both, Font = new Font("Consolas", 8.5f), WordWrap = false };
    private ComboBox _cboStartTilesMode = UiHelpers.CreateDropdown();
    private TextBox _txtStartTilesXml = new() { Multiline = true, Height = 65, ScrollBars = ScrollBars.Both, Font = new Font("Consolas", 8.5f), WordWrap = false };
    private ComboBox _cboTaskbarIconsMode = UiHelpers.CreateDropdown();
    private TextBox _txtTaskbarIconsXml = new() { Multiline = true, Height = 65, ScrollBars = ScrollBars.Both, Font = new Font("Consolas", 8.5f), WordWrap = false };

    private ComboBox _cboDesktopIconsMode = UiHelpers.CreateDropdown();
    private CheckedListBox _chkListDesktopIcons = new() { Height = 100, CheckOnClick = true };
    private ComboBox _cboStartFoldersMode = UiHelpers.CreateDropdown();
    private CheckedListBox _chkListStartFolders = new() { Height = 100, CheckOnClick = true };

    private CheckBox _chkUseNarrator = new() { Text = "Start Narrator during setup" };
    private ComboBox _cboLockKeysMode = UiHelpers.CreateDropdown();
    private ComboBox _cboCapsInit = UiHelpers.CreateDropdown(), _cboCapsBehav = UiHelpers.CreateDropdown();
    private ComboBox _cboNumInit = UiHelpers.CreateDropdown(), _cboNumBehav = UiHelpers.CreateDropdown();
    private ComboBox _cboScrollInit = UiHelpers.CreateDropdown(), _cboScrollBehav = UiHelpers.CreateDropdown();
    private ComboBox _cboStickyKeysMode = UiHelpers.CreateDropdown();
    private CheckedListBox _chkListStickyKeys = new() { Height = 95, CheckOnClick = true };

    // Tab 7: Bloatware
    private TextBox _txtBloatFilter = new() { Width = 280 };
    private CheckedListBox _chkListBloatware = new() { CheckOnClick = true };
    private readonly HashSet<string> _selectedBloatIds = new(StringComparer.OrdinalIgnoreCase);
    private bool _suppressBloatCheckEvent;

    // Tab 8: Custom Scripts
    private CheckBox _chkRestartExplorer = new() { Text = "Restart Explorer after UserOnce scripts" };
    private ListBox _lstScripts = new() { Height = 120 };
    private ComboBox _cboScriptPhase = UiHelpers.CreateDropdown();
    private ComboBox _cboScriptType = UiHelpers.CreateDropdown();
    private TextBox _txtScriptContent = new() { Multiline = true, Height = 180, ScrollBars = ScrollBars.Both, Font = new Font("Consolas", 9f), WordWrap = false };
    private readonly List<ScriptProfileDto> _scripts = new();

    private TabPage BuildSystemNetworkTab()
    {
      var page = new TabPage("System && Network");
      var scroll = UiHelpers.CreateScrollPanel();

      var grpName = UiHelpers.CreateSection("Computer Name");
      var tblName = UiHelpers.CreateFormTable();
      _cboComputerNameMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Random", "Random (Windows default)", "Random"),
        new ComboItem<string>("Prompt", "Prompt user during setup", "Prompt"),
        new ComboItem<string>("Custom", "Custom static name", "Custom"),
        new ComboItem<string>("Script", "Custom PowerShell script", "Script")
      });
      UiHelpers.AddRow(tblName, "Computer Name Mode:", _cboComputerNameMode);
      UiHelpers.AddRow(tblName, "Custom Computer Name:", _txtCustomComputerName);
      UiHelpers.AddRow(tblName, "Computer Name Script:", _txtComputerNameScript);
      grpName.Controls.Add(tblName);

      var grpWifi = UiHelpers.CreateSection("Wi-Fi Configuration");
      var tblWifi = UiHelpers.CreateFormTable();
      _cboWifiMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Interactive", "Interactive (Ask during OOBE)", "Interactive"),
        new ComboItem<string>("Skip", "Skip Wi-Fi setup", "Skip"),
        new ComboItem<string>("Parameterized", "Configure SSID & Password", "Parameterized"),
        new ComboItem<string>("Xml", "Custom WLAN Profile XML", "Xml")
      });
      _cboWifiAuth.Items.AddRange(new object[]
      {
        new ComboItem<WifiAuthentications>("WPA2PSK", "WPA2-Personal (AES)", WifiAuthentications.WPA2PSK),
        new ComboItem<WifiAuthentications>("WPA3SAE", "WPA3-Personal (SAE)", WifiAuthentications.WPA3SAE),
        new ComboItem<WifiAuthentications>("Open", "Open (No password)", WifiAuthentications.Open)
      });
      _chkWifiAuto.AutoSize = _chkWifiNonBroadcast.AutoSize = true;

      UiHelpers.AddRow(tblWifi, "Wi-Fi Mode:", _cboWifiMode);
      UiHelpers.AddRow(tblWifi, "SSID (Network Name):", _txtWifiName);
      UiHelpers.AddRow(tblWifi, "Password:", _txtWifiPassword);
      UiHelpers.AddRow(tblWifi, "Authentication:", _cboWifiAuth);
      UiHelpers.AddFullRow(tblWifi, _chkWifiAuto);
      UiHelpers.AddFullRow(tblWifi, _chkWifiNonBroadcast);
      UiHelpers.AddRow(tblWifi, "WLAN Profile XML:", _txtWifiXml);
      grpWifi.Controls.Add(tblWifi);

      var grpTweaks = UiHelpers.CreateSection("System Tweaks & Security");
      var flowTweaks = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.TopDown };
      var sysChecks = new[]
      {
        _chkEnableLongPaths, _chkEnableRemoteDesktop, _chkHardenAcl, _chkDeleteJunctions,
        _chkAllowPsScripts, _chkDisableLastAccess, _chkPreventAutoReboot, _chkDisableSac,
        _chkDisableUac, _chkDisableSmartScreen, _chkDisableSystemRestore, _chkDisableFastStartup,
        _chkTurnOffSounds, _chkPreventDeviceEncryption, _chkDisableWindowsUpdate, _chkDeleteWindowsOld,
        _chkDisableCoreIsolation, _chkDisableAutoRestartSignOn, _chkDisableWpbt, _chkPreventDeviceApps,
        _chkKeepSensitiveFiles, _chkHidePsWindows
      };
      foreach (var c in sysChecks) { c.AutoSize = true; flowTweaks.Controls.Add(c); }
      grpTweaks.Controls.Add(flowTweaks);

      var grpVm = UiHelpers.CreateSection("Virtual Machine Guest Tools");
      var flowVm = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.TopDown };
      foreach (var c in new[] { _chkVBoxGuest, _chkVMwareTools, _chkVirtIoGuest, _chkParallelsTools })
      {
        c.AutoSize = true;
        flowVm.Controls.Add(c);
      }
      grpVm.Controls.Add(flowVm);

      void UpdateSysEnabled()
      {
        string cn = UiHelpers.GetSelectedKey<string>(_cboComputerNameMode);
        _txtCustomComputerName.Enabled = cn == "Custom";
        _txtComputerNameScript.Enabled = cn == "Script";

        string wm = UiHelpers.GetSelectedKey<string>(_cboWifiMode);
        bool param = wm == "Parameterized";
        _txtWifiName.Enabled = _txtWifiPassword.Enabled = _cboWifiAuth.Enabled = _chkWifiAuto.Enabled = _chkWifiNonBroadcast.Enabled = param;
        _txtWifiXml.Enabled = wm == "Xml";
      }
      _cboComputerNameMode.SelectedIndexChanged += (_, _) => UpdateSysEnabled();
      _cboWifiMode.SelectedIndexChanged += (_, _) => UpdateSysEnabled();

      scroll.Controls.Add(grpVm);
      scroll.Controls.Add(grpTweaks);
      scroll.Controls.Add(grpWifi);
      scroll.Controls.Add(grpName);
      page.Controls.Add(scroll);
      return page;
    }

    private TabPage BuildUiPersonalizationTab()
    {
      var page = new TabPage("UI && Personalization");
      var scroll = UiHelpers.CreateScrollPanel();

      var grpExp = UiHelpers.CreateSection("Explorer, Taskbar & Edge");
      var tblExp = UiHelpers.CreateFormTable();
      _cboHideFiles.Items.AddRange(new object[]
      {
        new ComboItem<HideModes>("Hidden", "Hide hidden and system files (Default)", HideModes.Hidden),
        new ComboItem<HideModes>("HiddenSystem", "Show hidden files, hide protected OS files", HideModes.HiddenSystem),
        new ComboItem<HideModes>("None", "Show all files (including hidden & system)", HideModes.None)
      });
      _cboTaskbarSearch.Items.AddRange(new object[]
      {
        new ComboItem<TaskbarSearchMode>("Box", "Search Box", TaskbarSearchMode.Box),
        new ComboItem<TaskbarSearchMode>("Icon", "Search Icon Only", TaskbarSearchMode.Icon),
        new ComboItem<TaskbarSearchMode>("Label", "Search Icon and Label", TaskbarSearchMode.Label),
        new ComboItem<TaskbarSearchMode>("Hide", "Hide Search", TaskbarSearchMode.Hide)
      });

      UiHelpers.AddRow(tblExp, "Hidden Files:", _cboHideFiles);
      UiHelpers.AddRow(tblExp, "Taskbar Search:", _cboTaskbarSearch);
      foreach (var c in new[]
      {
        _chkClassicContextMenu, _chkLeftTaskbar, _chkHideTaskView, _chkShowFileExtensions,
        _chkShowAllTrayIcons, _chkLaunchToThisPC, _chkShowEndTask, _chkHideInfoTip,
        _chkDisableAppSuggestions, _chkDisableWidgets, _chkDisableBing, _chkDisablePointerPrecision,
        _chkHideEdgeFre, _chkDisableEdgeBoost, _chkMakeEdgeUninstallable, _chkDeleteEdgeIcon
      })
      {
        c.AutoSize = true;
        UiHelpers.AddFullRow(tblExp, c);
      }
      grpExp.Controls.Add(tblExp);

      var grpLook = UiHelpers.CreateSection("Wallpaper, Lock Screen & Colors");
      var tblLook = UiHelpers.CreateFormTable();
      _cboWallpaperMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Default", "Windows Default", "Default"),
        new ComboItem<string>("Solid", "Solid Color", "Solid"),
        new ComboItem<string>("Script", "Custom Script", "Script")
      });
      _cboLockScreenMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Default", "Windows Default", "Default"),
        new ComboItem<string>("Script", "Custom Script", "Script")
      });
      _cboColorMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Default", "Windows Default", "Default"),
        new ComboItem<string>("Custom", "Custom Theme & Accent Color", "Custom")
      });
      _cboSystemTheme.Items.AddRange(new object[]
      {
        new ComboItem<ColorTheme>("Dark", "Dark", ColorTheme.Dark),
        new ComboItem<ColorTheme>("Light", "Light", ColorTheme.Light)
      });
      _cboAppsTheme.Items.AddRange(new object[]
      {
        new ComboItem<ColorTheme>("Dark", "Dark", ColorTheme.Dark),
        new ComboItem<ColorTheme>("Light", "Light", ColorTheme.Light)
      });
      _chkTransparency.AutoSize = _chkAccentOnStart.AutoSize = _chkAccentOnBorders.AutoSize = true;

      _btnWallpaperColor.Click += (_, _) => PickColor(_btnWallpaperColor);
      _btnAccentColor.Click += (_, _) => PickColor(_btnAccentColor);

      UiHelpers.AddRow(tblLook, "Wallpaper Mode:", _cboWallpaperMode);
      UiHelpers.AddRow(tblLook, "Wallpaper Color:", _btnWallpaperColor);
      UiHelpers.AddRow(tblLook, "Wallpaper Script:", _txtWallpaperScript);
      UiHelpers.AddRow(tblLook, "Lock Screen Mode:", _cboLockScreenMode);
      UiHelpers.AddRow(tblLook, "Lock Screen Script:", _txtLockScreenScript);
      UiHelpers.AddRow(tblLook, "Color Theme Mode:", _cboColorMode);
      UiHelpers.AddRow(tblLook, "System Theme:", _cboSystemTheme);
      UiHelpers.AddRow(tblLook, "Apps Theme:", _cboAppsTheme);
      UiHelpers.AddFullRow(tblLook, _chkTransparency);
      UiHelpers.AddFullRow(tblLook, _chkAccentOnStart);
      UiHelpers.AddFullRow(tblLook, _chkAccentOnBorders);
      UiHelpers.AddRow(tblLook, "Accent Color:", _btnAccentColor);
      grpLook.Controls.Add(tblLook);

      var grpStart = UiHelpers.CreateSection("Start Menu, Taskbar Icons, Desktop Icons & Visual Effects");
      var tblStart = UiHelpers.CreateFormTable();
      _cboEffectsMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Default", "Windows Default", "Default"),
        new ComboItem<string>("BestPerformance", "Adjust for Best Performance", "BestPerformance"),
        new ComboItem<string>("BestAppearance", "Adjust for Best Appearance", "BestAppearance"),
        new ComboItem<string>("Custom", "Custom Visual Effects", "Custom")
      });
      foreach (Effect eff in Enum.GetValues(typeof(Effect)))
        _chkListEffects.Items.Add(new ComboItem<Effect>(eff.ToString(), eff.ToString(), eff));

      _cboStartPinsMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Default", "Default", "Default"),
        new ComboItem<string>("Empty", "Empty (Remove all Start pins)", "Empty"),
        new ComboItem<string>("Custom", "Custom Start Pins JSON", "Custom")
      });
      _cboStartTilesMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Default", "Default", "Default"),
        new ComboItem<string>("Empty", "Empty", "Empty"),
        new ComboItem<string>("Custom", "Custom Start Tiles XML", "Custom")
      });
      _cboTaskbarIconsMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Default", "Default", "Default"),
        new ComboItem<string>("Empty", "Empty (Unpin all taskbar icons)", "Empty"),
        new ComboItem<string>("Custom", "Custom Taskbar Layout XML", "Custom")
      });
      _cboDesktopIconsMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Default", "Default", "Default"),
        new ComboItem<string>("Custom", "Custom Desktop Icons", "Custom")
      });
      foreach (var di in _generator.DesktopIcons.Values.OrderBy(x => x.DisplayName))
        _chkListDesktopIcons.Items.Add(new ComboItem<DesktopIcon>(di.Id, di.DisplayName, di));

      _cboStartFoldersMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Default", "Default", "Default"),
        new ComboItem<string>("Custom", "Custom Start Folders", "Custom")
      });
      foreach (var sf in _generator.StartFolders.Values.OrderBy(x => x.DisplayName))
        _chkListStartFolders.Items.Add(new ComboItem<StartFolder>(sf.Id, sf.DisplayName, sf));

      _chkListEffects.Dock = _chkListDesktopIcons.Dock = _chkListStartFolders.Dock = DockStyle.Top;
      UiHelpers.AddRow(tblStart, "Visual Effects:", _cboEffectsMode);
      UiHelpers.AddFullRow(tblStart, _chkListEffects);
      UiHelpers.AddRow(tblStart, "Start Pins (Win11):", _cboStartPinsMode);
      UiHelpers.AddRow(tblStart, "Start Pins JSON:", _txtStartPinsJson);
      UiHelpers.AddRow(tblStart, "Start Tiles (Win10):", _cboStartTilesMode);
      UiHelpers.AddRow(tblStart, "Start Tiles XML:", _txtStartTilesXml);
      UiHelpers.AddRow(tblStart, "Taskbar Icons:", _cboTaskbarIconsMode);
      UiHelpers.AddRow(tblStart, "Taskbar Layout XML:", _txtTaskbarIconsXml);
      UiHelpers.AddRow(tblStart, "Desktop Icons:", _cboDesktopIconsMode);
      UiHelpers.AddFullRow(tblStart, _chkListDesktopIcons);
      UiHelpers.AddRow(tblStart, "Start Folders:", _cboStartFoldersMode);
      UiHelpers.AddFullRow(tblStart, _chkListStartFolders);
      grpStart.Controls.Add(tblStart);

      var grpA11y = UiHelpers.CreateSection("Accessibility, Lock Keys & Sticky Keys");
      var tblA11y = UiHelpers.CreateFormTable();
      _chkUseNarrator.AutoSize = true;
      _cboLockKeysMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Skip", "Skip (Default)", "Skip"),
        new ComboItem<string>("Configure", "Configure Caps / Num / Scroll Lock", "Configure")
      });
      void FillLockCombos(ComboBox init, ComboBox behav)
      {
        init.Width = 100; behav.Width = 110;
        init.Items.AddRange(new object[] { new ComboItem<LockKeyInitial>("Off", "Off", LockKeyInitial.Off), new ComboItem<LockKeyInitial>("On", "On", LockKeyInitial.On) });
        behav.Items.AddRange(new object[] { new ComboItem<LockKeyBehavior>("Toggle", "Toggle", LockKeyBehavior.Toggle), new ComboItem<LockKeyBehavior>("Ignore", "Ignore", LockKeyBehavior.Ignore) });
      }
      FillLockCombos(_cboCapsInit, _cboCapsBehav);
      FillLockCombos(_cboNumInit, _cboNumBehav);
      FillLockCombos(_cboScrollInit, _cboScrollBehav);

      FlowLayoutPanel MakeKeyRow(ComboBox a, ComboBox b)
      {
        var f = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
        f.Controls.Add(a); f.Controls.Add(b);
        return f;
      }

      _cboStickyKeysMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Default", "Windows Default", "Default"),
        new ComboItem<string>("Disabled", "Disable Sticky Keys", "Disabled"),
        new ComboItem<string>("Custom", "Custom Sticky Keys Flags", "Custom")
      });
      foreach (StickyKeys sk in Enum.GetValues(typeof(StickyKeys)))
        _chkListStickyKeys.Items.Add(new ComboItem<StickyKeys>(sk.ToString(), sk.ToString(), sk));
      _chkListStickyKeys.Dock = DockStyle.Top;

      UiHelpers.AddFullRow(tblA11y, _chkUseNarrator);
      UiHelpers.AddRow(tblA11y, "Lock Keys:", _cboLockKeysMode);
      UiHelpers.AddRow(tblA11y, "Caps Lock:", MakeKeyRow(_cboCapsInit, _cboCapsBehav));
      UiHelpers.AddRow(tblA11y, "Num Lock:", MakeKeyRow(_cboNumInit, _cboNumBehav));
      UiHelpers.AddRow(tblA11y, "Scroll Lock:", MakeKeyRow(_cboScrollInit, _cboScrollBehav));
      UiHelpers.AddRow(tblA11y, "Sticky Keys:", _cboStickyKeysMode);
      UiHelpers.AddFullRow(tblA11y, _chkListStickyKeys);
      grpA11y.Controls.Add(tblA11y);

      void UpdateUiEnabled()
      {
        string wp = UiHelpers.GetSelectedKey<string>(_cboWallpaperMode);
        _btnWallpaperColor.Enabled = wp == "Solid";
        _txtWallpaperScript.Enabled = wp == "Script";
        _txtLockScreenScript.Enabled = UiHelpers.GetSelectedKey<string>(_cboLockScreenMode) == "Script";
        bool custColor = UiHelpers.GetSelectedKey<string>(_cboColorMode) == "Custom";
        _cboSystemTheme.Enabled = _cboAppsTheme.Enabled = _chkTransparency.Enabled = _chkAccentOnStart.Enabled = _chkAccentOnBorders.Enabled = _btnAccentColor.Enabled = custColor;

        _chkListEffects.Enabled = UiHelpers.GetSelectedKey<string>(_cboEffectsMode) == "Custom";
        _txtStartPinsJson.Enabled = UiHelpers.GetSelectedKey<string>(_cboStartPinsMode) == "Custom";
        _txtStartTilesXml.Enabled = UiHelpers.GetSelectedKey<string>(_cboStartTilesMode) == "Custom";
        _txtTaskbarIconsXml.Enabled = UiHelpers.GetSelectedKey<string>(_cboTaskbarIconsMode) == "Custom";
        _chkListDesktopIcons.Enabled = UiHelpers.GetSelectedKey<string>(_cboDesktopIconsMode) == "Custom";
        _chkListStartFolders.Enabled = UiHelpers.GetSelectedKey<string>(_cboStartFoldersMode) == "Custom";

        bool lk = UiHelpers.GetSelectedKey<string>(_cboLockKeysMode) == "Configure";
        _cboCapsInit.Enabled = _cboCapsBehav.Enabled = _cboNumInit.Enabled = _cboNumBehav.Enabled = _cboScrollInit.Enabled = _cboScrollBehav.Enabled = lk;
        _chkListStickyKeys.Enabled = UiHelpers.GetSelectedKey<string>(_cboStickyKeysMode) == "Custom";
      }

      _cboWallpaperMode.SelectedIndexChanged += (_, _) => UpdateUiEnabled();
      _cboLockScreenMode.SelectedIndexChanged += (_, _) => UpdateUiEnabled();
      _cboColorMode.SelectedIndexChanged += (_, _) => UpdateUiEnabled();
      _cboEffectsMode.SelectedIndexChanged += (_, _) => UpdateUiEnabled();
      _cboStartPinsMode.SelectedIndexChanged += (_, _) => UpdateUiEnabled();
      _cboStartTilesMode.SelectedIndexChanged += (_, _) => UpdateUiEnabled();
      _cboTaskbarIconsMode.SelectedIndexChanged += (_, _) => UpdateUiEnabled();
      _cboDesktopIconsMode.SelectedIndexChanged += (_, _) => UpdateUiEnabled();
      _cboStartFoldersMode.SelectedIndexChanged += (_, _) => UpdateUiEnabled();
      _cboLockKeysMode.SelectedIndexChanged += (_, _) => UpdateUiEnabled();
      _cboStickyKeysMode.SelectedIndexChanged += (_, _) => UpdateUiEnabled();

      scroll.Controls.Add(grpA11y);
      scroll.Controls.Add(grpStart);
      scroll.Controls.Add(grpLook);
      scroll.Controls.Add(grpExp);
      page.Controls.Add(scroll);
      return page;
    }

    private static void PickColor(Button btn)
    {
      using var dlg = new ColorDialog { Color = UiHelpers.HexToColor(btn.Text, Color.Black), FullOpen = true };
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        btn.Text = UiHelpers.ColorToHex(dlg.Color);
        btn.BackColor = dlg.Color;
        btn.ForeColor = dlg.Color.GetBrightness() < 0.5f ? Color.White : Color.Black;
      }
    }

    private TabPage BuildBloatwareTab()
    {
      var page = new TabPage("Bloatware");
      var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

      var topBar = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(0, 0, 0, 8) };
      var lblFilter = new Label { Text = "Filter:", AutoSize = true, Margin = new Padding(0, 6, 4, 0) };
      var btnAll = new Button { Text = "Select All Visible", AutoSize = true };
      var btnNone = new Button { Text = "Select None", AutoSize = true };

      topBar.Controls.AddRange(new Control[] { lblFilter, _txtBloatFilter, btnAll, btnNone });

      _chkListBloatware.Dock = DockStyle.Fill;
      _chkListBloatware.ItemCheck += (_, e) =>
      {
        if (_suppressBloatCheckEvent) return;
        if (_chkListBloatware.Items[e.Index] is ComboItem<Bloatware> item)
        {
          if (e.NewValue == CheckState.Checked) _selectedBloatIds.Add(item.Key);
          else _selectedBloatIds.Remove(item.Key);
        }
      };

      _txtBloatFilter.TextChanged += (_, _) => PopulateBloatwareList();
      btnAll.Click += (_, _) =>
      {
        for (int i = 0; i < _chkListBloatware.Items.Count; i++)
          _chkListBloatware.SetItemChecked(i, true);
      };
      btnNone.Click += (_, _) =>
      {
        _selectedBloatIds.Clear();
        for (int i = 0; i < _chkListBloatware.Items.Count; i++)
          _chkListBloatware.SetItemChecked(i, false);
      };

      panel.Controls.Add(_chkListBloatware);
      panel.Controls.Add(topBar);
      page.Controls.Add(panel);
      return page;
    }

    private void PopulateBloatwareList()
    {
      _suppressBloatCheckEvent = true;
      try
      {
        string filter = _txtBloatFilter.Text.Trim();
        _chkListBloatware.Items.Clear();
        foreach (var b in _generator.Bloatwares.Values.OrderBy(x => x.DisplayName))
        {
          if (string.IsNullOrEmpty(filter) ||
              b.DisplayName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 ||
              b.Id.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
          {
            int idx = _chkListBloatware.Items.Add(new ComboItem<Bloatware>(b.Id, $"{b.DisplayName} [{b.Id}]", b));
            _chkListBloatware.SetItemChecked(idx, _selectedBloatIds.Contains(b.Id));
          }
        }
      }
      finally
      {
        _suppressBloatCheckEvent = false;
      }
    }

    private TabPage BuildScriptsTab()
    {
      var page = new TabPage("Custom Scripts");
      var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

      _chkRestartExplorer.Dock = DockStyle.Top;
      _chkRestartExplorer.AutoSize = true;
      _chkRestartExplorer.Padding = new Padding(0, 0, 0, 8);

      var lblList = new Label { Text = "Configured Scripts:", Dock = DockStyle.Top, AutoSize = true };
      _lstScripts.Dock = DockStyle.Top;

      var btnBar = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(0, 6, 0, 6) };
      var btnAdd = new Button { Text = "Add Script", AutoSize = true };
      var btnUpdate = new Button { Text = "Update Selected", AutoSize = true };
      var btnRemove = new Button { Text = "Remove Selected", AutoSize = true };
      btnBar.Controls.AddRange(new Control[] { btnAdd, btnUpdate, btnRemove });

      var tblEdit = UiHelpers.CreateFormTable();
      foreach (ScriptPhase p in Enum.GetValues(typeof(ScriptPhase)))
        _cboScriptPhase.Items.Add(new ComboItem<ScriptPhase>(p.ToString(), p.ToString(), p));
      foreach (ScriptType t in Enum.GetValues(typeof(ScriptType)))
        _cboScriptType.Items.Add(new ComboItem<ScriptType>(t.ToString(), t.ToString(), t));

      UiHelpers.AddRow(tblEdit, "Execution Phase:", _cboScriptPhase);
      UiHelpers.AddRow(tblEdit, "Script Type:", _cboScriptType);
      UiHelpers.AddRow(tblEdit, "Script Content:", _txtScriptContent);

      void RefreshScriptList(int selectIdx = -1)
      {
        _lstScripts.Items.Clear();
        for (int i = 0; i < _scripts.Count; i++)
        {
          var s = _scripts[i];
          string firstLine = s.Content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "(empty)";
          if (firstLine.Length > 55) firstLine = firstLine.Substring(0, 55) + "...";
          _lstScripts.Items.Add($"{i + 1}. [{s.Phase} | {s.Type}] {firstLine}");
        }
        if (selectIdx >= 0 && selectIdx < _lstScripts.Items.Count)
          _lstScripts.SelectedIndex = selectIdx;
      }

      _lstScripts.SelectedIndexChanged += (_, _) =>
      {
        int idx = _lstScripts.SelectedIndex;
        if (idx >= 0 && idx < _scripts.Count)
        {
          UiHelpers.SelectEnum(_cboScriptPhase, _scripts[idx].Phase);
          UiHelpers.SelectEnum(_cboScriptType, _scripts[idx].Type);
          _txtScriptContent.Text = _scripts[idx].Content;
        }
      };

      btnAdd.Click += (_, _) =>
      {
        _scripts.Add(new ScriptProfileDto
        {
          Phase = UiHelpers.GetSelectedEnum(_cboScriptPhase, ScriptPhase.FirstLogon),
          Type = UiHelpers.GetSelectedEnum(_cboScriptType, ScriptType.Ps1),
          Content = _txtScriptContent.Text
        });
        RefreshScriptList(_scripts.Count - 1);
      };

      btnUpdate.Click += (_, _) =>
      {
        int idx = _lstScripts.SelectedIndex;
        if (idx >= 0 && idx < _scripts.Count)
        {
          _scripts[idx] = new ScriptProfileDto
          {
            Phase = UiHelpers.GetSelectedEnum(_cboScriptPhase, ScriptPhase.FirstLogon),
            Type = UiHelpers.GetSelectedEnum(_cboScriptType, ScriptType.Ps1),
            Content = _txtScriptContent.Text
          };
          RefreshScriptList(idx);
        }
      };

      btnRemove.Click += (_, _) =>
      {
        int idx = _lstScripts.SelectedIndex;
        if (idx >= 0 && idx < _scripts.Count)
        {
          _scripts.RemoveAt(idx);
          RefreshScriptList(Math.Min(idx, _scripts.Count - 1));
        }
      };

      panel.Controls.Add(tblEdit);
      panel.Controls.Add(btnBar);
      panel.Controls.Add(_lstScripts);
      panel.Controls.Add(lblList);
      panel.Controls.Add(_chkRestartExplorer);
      page.Controls.Add(panel);
      return page;
    }

    private void LoadProfileToUi(AppProfile p)
    {
      // Tab 1
      _chkArchX86.Checked = p.ArchX86;
      _chkArchAmd64.Checked = p.ArchAmd64;
      _chkArchArm64.Checked = p.ArchArm64;
      UiHelpers.SelectByKey<string>(_cboLanguageMode, p.LanguageMode);
      UiHelpers.SelectByKey<ImageLanguage>(_cboImageLanguage, p.ImageLanguageId);
      UiHelpers.SelectByKey<UserLocale>(_cboLocale1, p.Locale1Id);
      UiHelpers.SelectByKey<KeyboardIdentifier>(_cboKeyboard1, p.Keyboard1Id);
      _chkLocale2.Checked = p.UseLocale2;
      UiHelpers.SelectByKey<UserLocale>(_cboLocale2, p.Locale2Id);
      UiHelpers.SelectByKey<KeyboardIdentifier>(_cboKeyboard2, p.Keyboard2Id);
      _chkLocale3.Checked = p.UseLocale3;
      UiHelpers.SelectByKey<UserLocale>(_cboLocale3, p.Locale3Id);
      UiHelpers.SelectByKey<KeyboardIdentifier>(_cboKeyboard3, p.Keyboard3Id);
      UiHelpers.SelectByKey<GeoLocation>(_cboGeoLocation, p.GeoLocationId);
      UiHelpers.SelectByKey<string>(_cboTimeZoneMode, p.TimeZoneMode);
      UiHelpers.SelectByKey<TimeOffset>(_cboTimeZone, p.TimeZoneId);

      // Tab 2
      UiHelpers.SelectByKey<string>(_cboPEMode, p.PEMode);
      _txtPEScript.Text = p.PEScript;
      UiHelpers.SelectByKey<string>(_cboEditionMode, p.EditionMode);
      UiHelpers.SelectByKey<WindowsEdition>(_cboWindowsEdition, p.WindowsEditionId);
      _txtCustomEditionKey.Text = p.CustomEditionKey;
      _txtActivationKey.Text = p.ActivationKey;
      _chkBypassRequirements.Checked = p.BypassRequirementsCheck;
      _chkUseConfigSet.Checked = p.UseConfigurationSet;
      _chkBypassNetwork.Checked = p.BypassNetworkCheck;
      UiHelpers.SelectEnum(_cboExpressSettings, p.ExpressSettings);

      // Tab 3
      UiHelpers.SelectByKey<string>(_cboTargetDiskMode, p.TargetDiskMode);
      _numFixedDiskIndex.Value = Math.Max(_numFixedDiskIndex.Minimum, Math.Min(_numFixedDiskIndex.Maximum, p.FixedDiskIndex));
      _txtTargetDiskScript.Text = p.TargetDiskScript;
      _chkGenMinSize.Checked = p.GeneratedMinSizeGiB.HasValue;
      _numGenMinSize.Value = p.GeneratedMinSizeGiB ?? 10;
      _chkGenMaxSize.Checked = p.GeneratedMaxSizeGiB.HasValue;
      _numGenMaxSize.Value = p.GeneratedMaxSizeGiB ?? 1000;
      _chkGenIndex.Checked = p.GeneratedDiskIndex.HasValue;
      _numGenIndex.Value = p.GeneratedDiskIndex ?? 0;
      _chkAssertNoPartitions.Checked = p.AssertNoPartitions;
      _chkAssertInterfaceType.Checked = p.AssertInterfaceType;
      _chkAssertMediaType.Checked = p.AssertMediaType;

      UiHelpers.SelectByKey<string>(_cboPartitionMode, p.PartitionMode);
      UiHelpers.SelectEnum(_cboPartitionLayout, p.PartitionLayout);
      UiHelpers.SelectEnum(_cboRecoveryMode, p.RecoveryMode);
      _numSystemSize.Value = Math.Max(_numSystemSize.Minimum, Math.Min(_numSystemSize.Maximum, p.SystemPartitionSize));
      _numRecoverySize.Value = Math.Max(_numRecoverySize.Minimum, Math.Min(_numRecoverySize.Maximum, p.RecoveryPartitionSize));
      _txtCustomPartitionScript.Text = p.CustomPartitionScript;

      UiHelpers.SelectByKey<string>(_cboInstallFromMode, p.InstallFromMode);
      _numInstallFromIndex.Value = Math.Max(_numInstallFromIndex.Minimum, Math.Min(_numInstallFromIndex.Maximum, p.InstallFromIndex));
      _txtInstallFromName.Text = p.InstallFromName;

      UiHelpers.SelectByKey<string>(_cboPagingFileMode, p.PagingFileMode);
      _numPagingInitial.Value = Math.Max(_numPagingInitial.Minimum, Math.Min(_numPagingInitial.Maximum, p.PagingInitialMiB));
      _numPagingMax.Value = Math.Max(_numPagingMax.Minimum, Math.Min(_numPagingMax.Maximum, p.PagingMaxMiB));

      _chkDisableDefender.Checked = p.DisableDefender;
      _chkDisable8Dot3.Checked = p.Disable8Dot3Names;
      _chkPauseBeforeFormat.Checked = p.PauseBeforeFormatting;
      _chkPauseBeforeReboot.Checked = p.PauseBeforeReboot;
      _chkCompactOs.Checked = p.CompactOs;
      _chkSkipIntegrityCheck.Checked = p.SkipIntegrityCheck;

      // Tab 4
      UiHelpers.SelectByKey<string>(_cboAccountMode, p.AccountMode);
      _gridAccounts.Rows.Clear();
      foreach (var acc in p.Accounts)
        _gridAccounts.Rows.Add(acc.Name, acc.DisplayName, acc.Password, string.IsNullOrEmpty(acc.Group) ? "Administrators" : acc.Group);
      UiHelpers.SelectByKey<string>(_cboAutoLogonMode, p.AutoLogonMode);
      _txtBuiltinAdminPwd.Text = p.BuiltinAdminPassword;
      _chkObscurePasswords.Checked = p.ObscurePasswords;

      UiHelpers.SelectByKey<string>(_cboPwdExpMode, p.PasswordExpirationMode);
      _numPwdMaxAge.Value = Math.Max(_numPwdMaxAge.Minimum, Math.Min(_numPwdMaxAge.Maximum, p.PasswordMaxAgeDays));
      UiHelpers.SelectByKey<string>(_cboLockoutMode, p.LockoutMode);
      _numLockoutThreshold.Value = p.LockoutThreshold;
      _numLockoutDuration.Value = p.LockoutDurationMinutes;
      _numLockoutWindow.Value = p.LockoutWindowMinutes;
      UiHelpers.SelectByKey<string>(_cboProcessAuditMode, p.ProcessAuditMode);
      _chkProcessAuditCmdLine.Checked = p.ProcessAuditIncludeCommandLine;
      UiHelpers.SelectByKey<string>(_cboAppLockerMode, p.AppLockerMode);
      _txtAppLockerXml.Text = p.AppLockerPolicyXml;

      // Tab 5
      UiHelpers.SelectByKey<string>(_cboComputerNameMode, p.ComputerNameMode);
      _txtCustomComputerName.Text = p.CustomComputerName;
      _txtComputerNameScript.Text = p.ComputerNameScript;
      UiHelpers.SelectByKey<string>(_cboWifiMode, p.WifiMode);
      _txtWifiName.Text = p.WifiName;
      _txtWifiPassword.Text = p.WifiPassword;
      UiHelpers.SelectEnum(_cboWifiAuth, p.WifiAuthentication);
      _chkWifiAuto.Checked = p.WifiConnectAutomatically;
      _chkWifiNonBroadcast.Checked = p.WifiNonBroadcast;
      _txtWifiXml.Text = p.WifiProfileXml;

      _chkEnableLongPaths.Checked = p.EnableLongPaths;
      _chkEnableRemoteDesktop.Checked = p.EnableRemoteDesktop;
      _chkHardenAcl.Checked = p.HardenSystemDriveAcl;
      _chkDeleteJunctions.Checked = p.DeleteJunctions;
      _chkAllowPsScripts.Checked = p.AllowPowerShellScripts;
      _chkDisableLastAccess.Checked = p.DisableLastAccess;
      _chkPreventAutoReboot.Checked = p.PreventAutomaticReboot;
      _chkDisableSac.Checked = p.DisableSac;
      _chkDisableUac.Checked = p.DisableUac;
      _chkDisableSmartScreen.Checked = p.DisableSmartScreen;
      _chkDisableSystemRestore.Checked = p.DisableSystemRestore;
      _chkDisableFastStartup.Checked = p.DisableFastStartup;
      _chkTurnOffSounds.Checked = p.TurnOffSystemSounds;
      _chkPreventDeviceEncryption.Checked = p.PreventDeviceEncryption;
      _chkDisableWindowsUpdate.Checked = p.DisableWindowsUpdate;
      _chkDeleteWindowsOld.Checked = p.DeleteWindowsOld;
      _chkDisableCoreIsolation.Checked = p.DisableCoreIsolation;
      _chkDisableAutoRestartSignOn.Checked = p.DisableAutomaticRestartSignOn;
      _chkDisableWpbt.Checked = p.DisableWpbt;
      _chkPreventDeviceApps.Checked = p.PreventDeviceApps;
      _chkKeepSensitiveFiles.Checked = p.KeepSensitiveFiles;
      _chkHidePsWindows.Checked = p.HidePowerShellWindows;
      _chkVBoxGuest.Checked = p.VBoxGuestAdditions;
      _chkVMwareTools.Checked = p.VMwareTools;
      _chkVirtIoGuest.Checked = p.VirtIoGuestTools;
      _chkParallelsTools.Checked = p.ParallelsTools;

      // Tab 6
      _chkClassicContextMenu.Checked = p.ClassicContextMenu;
      _chkLeftTaskbar.Checked = p.LeftTaskbar;
      _chkHideTaskView.Checked = p.HideTaskViewButton;
      _chkShowFileExtensions.Checked = p.ShowFileExtensions;
      _chkShowAllTrayIcons.Checked = p.ShowAllTrayIcons;
      UiHelpers.SelectEnum(_cboHideFiles, p.HideFiles);
      UiHelpers.SelectEnum(_cboTaskbarSearch, p.TaskbarSearch);
      _chkLaunchToThisPC.Checked = p.LaunchToThisPC;
      _chkShowEndTask.Checked = p.ShowEndTask;
      _chkHideInfoTip.Checked = p.HideInfoTip;
      _chkDisableAppSuggestions.Checked = p.DisableAppSuggestions;
      _chkDisableWidgets.Checked = p.DisableWidgets;
      _chkDisableBing.Checked = p.DisableBingResults;
      _chkDisablePointerPrecision.Checked = p.DisablePointerPrecision;
      _chkHideEdgeFre.Checked = p.HideEdgeFre;
      _chkDisableEdgeBoost.Checked = p.DisableEdgeStartupBoost;
      _chkMakeEdgeUninstallable.Checked = p.MakeEdgeUninstallable;
      _chkDeleteEdgeIcon.Checked = p.DeleteEdgeDesktopIcon;

      UiHelpers.SelectByKey<string>(_cboWallpaperMode, p.WallpaperMode);
      SetColorButton(_btnWallpaperColor, p.WallpaperColorHex, Color.Black);
      _txtWallpaperScript.Text = p.WallpaperScript;
      UiHelpers.SelectByKey<string>(_cboLockScreenMode, p.LockScreenMode);
      _txtLockScreenScript.Text = p.LockScreenScript;
      UiHelpers.SelectByKey<string>(_cboColorMode, p.ColorMode);
      UiHelpers.SelectEnum(_cboSystemTheme, p.SystemTheme);
      UiHelpers.SelectEnum(_cboAppsTheme, p.AppsTheme);
      _chkTransparency.Checked = p.EnableTransparency;
      _chkAccentOnStart.Checked = p.AccentColorOnStart;
      _chkAccentOnBorders.Checked = p.AccentColorOnBorders;
      SetColorButton(_btnAccentColor, p.AccentColorHex, Color.FromArgb(0, 120, 212));

      UiHelpers.SelectByKey<string>(_cboEffectsMode, p.EffectsMode);
      for (int i = 0; i < _chkListEffects.Items.Count; i++)
        if (_chkListEffects.Items[i] is ComboItem<Effect> ei)
          _chkListEffects.SetItemChecked(i, p.CustomEffects.TryGetValue(ei.Value, out bool v) && v);

      UiHelpers.SelectByKey<string>(_cboStartPinsMode, p.StartPinsMode);
      _txtStartPinsJson.Text = p.CustomStartPinsJson;
      UiHelpers.SelectByKey<string>(_cboStartTilesMode, p.StartTilesMode);
      _txtStartTilesXml.Text = p.CustomStartTilesXml;
      UiHelpers.SelectByKey<string>(_cboTaskbarIconsMode, p.TaskbarIconsMode);
      _txtTaskbarIconsXml.Text = p.CustomTaskbarIconsXml;

      UiHelpers.SelectByKey<string>(_cboDesktopIconsMode, p.DesktopIconsMode);
      for (int i = 0; i < _chkListDesktopIcons.Items.Count; i++)
        if (_chkListDesktopIcons.Items[i] is ComboItem<DesktopIcon> di)
          _chkListDesktopIcons.SetItemChecked(i, p.CustomDesktopIcons.TryGetValue(di.Key, out bool v) && v);

      UiHelpers.SelectByKey<string>(_cboStartFoldersMode, p.StartFoldersMode);
      for (int i = 0; i < _chkListStartFolders.Items.Count; i++)
        if (_chkListStartFolders.Items[i] is ComboItem<StartFolder> sf)
          _chkListStartFolders.SetItemChecked(i, p.CustomStartFolders.TryGetValue(sf.Key, out bool v) && v);

      _chkUseNarrator.Checked = p.UseNarrator;
      UiHelpers.SelectByKey<string>(_cboLockKeysMode, p.LockKeysMode);
      UiHelpers.SelectEnum(_cboCapsInit, p.CapsLockInitial);
      UiHelpers.SelectEnum(_cboCapsBehav, p.CapsLockBehavior);
      UiHelpers.SelectEnum(_cboNumInit, p.NumLockInitial);
      UiHelpers.SelectEnum(_cboNumBehav, p.NumLockBehavior);
      UiHelpers.SelectEnum(_cboScrollInit, p.ScrollLockInitial);
      UiHelpers.SelectEnum(_cboScrollBehav, p.ScrollLockBehavior);
      UiHelpers.SelectByKey<string>(_cboStickyKeysMode, p.StickyKeysMode);
      for (int i = 0; i < _chkListStickyKeys.Items.Count; i++)
        if (_chkListStickyKeys.Items[i] is ComboItem<StickyKeys> sk)
          _chkListStickyKeys.SetItemChecked(i, p.CustomStickyKeysFlags.Contains(sk.Value));

      // Tab 7
      _selectedBloatIds.Clear();
      foreach (var id in p.SelectedBloatwareIds) _selectedBloatIds.Add(id);
      PopulateBloatwareList();

      // Tab 8
      _chkRestartExplorer.Checked = p.RestartExplorer;
      _scripts.Clear();
      _scripts.AddRange(p.Scripts.Select(s => new ScriptProfileDto { Phase = s.Phase, Type = s.Type, Content = s.Content }));
      _lstScripts.Items.Clear();
      for (int i = 0; i < _scripts.Count; i++)
      {
        var s = _scripts[i];
        string firstLine = s.Content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "(empty)";
        if (firstLine.Length > 55) firstLine = firstLine.Substring(0, 55) + "...";
        _lstScripts.Items.Add($"{i + 1}. [{s.Phase} | {s.Type}] {firstLine}");
      }
      UiHelpers.SelectEnum(_cboScriptPhase, ScriptPhase.FirstLogon);
      UiHelpers.SelectEnum(_cboScriptType, ScriptType.Ps1);
    }

    private static void SetColorButton(Button btn, string hex, Color fallback)
    {
      Color c = UiHelpers.HexToColor(hex, fallback);
      btn.Text = UiHelpers.ColorToHex(c);
      btn.BackColor = c;
      btn.ForeColor = c.GetBrightness() < 0.5f ? Color.White : Color.Black;
    }

    private AppProfile SaveUiToProfile()
    {
      var p = new AppProfile
      {
        // Tab 1
        ArchX86 = _chkArchX86.Checked,
        ArchAmd64 = _chkArchAmd64.Checked,
        ArchArm64 = _chkArchArm64.Checked,
        LanguageMode = UiHelpers.GetSelectedKey<string>(_cboLanguageMode, "Interactive"),
        ImageLanguageId = UiHelpers.GetSelectedKey<ImageLanguage>(_cboImageLanguage, "en-US"),
        Locale1Id = UiHelpers.GetSelectedKey<UserLocale>(_cboLocale1, "en-US"),
        Keyboard1Id = UiHelpers.GetSelectedKey<KeyboardIdentifier>(_cboKeyboard1, "00000409"),
        UseLocale2 = _chkLocale2.Checked,
        Locale2Id = UiHelpers.GetSelectedKey<UserLocale>(_cboLocale2, "en-US"),
        Keyboard2Id = UiHelpers.GetSelectedKey<KeyboardIdentifier>(_cboKeyboard2, "00000409"),
        UseLocale3 = _chkLocale3.Checked,
        Locale3Id = UiHelpers.GetSelectedKey<UserLocale>(_cboLocale3, "en-US"),
        Keyboard3Id = UiHelpers.GetSelectedKey<KeyboardIdentifier>(_cboKeyboard3, "00000409"),
        GeoLocationId = UiHelpers.GetSelectedKey<GeoLocation>(_cboGeoLocation, "244"),
        TimeZoneMode = UiHelpers.GetSelectedKey<string>(_cboTimeZoneMode, "Implicit"),
        TimeZoneId = UiHelpers.GetSelectedKey<TimeOffset>(_cboTimeZone, "UTC"),

        // Tab 2
        PEMode = UiHelpers.GetSelectedKey<string>(_cboPEMode, "Default"),
        PEScript = _txtPEScript.Text,
        EditionMode = UiHelpers.GetSelectedKey<string>(_cboEditionMode, "Interactive"),
        WindowsEditionId = UiHelpers.GetSelectedKey<WindowsEdition>(_cboWindowsEdition, "pro"),
        CustomEditionKey = _txtCustomEditionKey.Text,
        ActivationKey = _txtActivationKey.Text,
        BypassRequirementsCheck = _chkBypassRequirements.Checked,
        UseConfigurationSet = _chkUseConfigSet.Checked,
        BypassNetworkCheck = _chkBypassNetwork.Checked,
        ExpressSettings = UiHelpers.GetSelectedEnum(_cboExpressSettings, ExpressSettingsMode.DisableAll),

        // Tab 3
        TargetDiskMode = UiHelpers.GetSelectedKey<string>(_cboTargetDiskMode, "Generated"),
        FixedDiskIndex = (int)_numFixedDiskIndex.Value,
        TargetDiskScript = _txtTargetDiskScript.Text,
        GeneratedMinSizeGiB = _chkGenMinSize.Checked ? (int)_numGenMinSize.Value : null,
        GeneratedMaxSizeGiB = _chkGenMaxSize.Checked ? (int)_numGenMaxSize.Value : null,
        GeneratedDiskIndex = _chkGenIndex.Checked ? (int)_numGenIndex.Value : null,
        AssertNoPartitions = _chkAssertNoPartitions.Checked,
        AssertInterfaceType = _chkAssertInterfaceType.Checked,
        AssertMediaType = _chkAssertMediaType.Checked,
        PartitionMode = UiHelpers.GetSelectedKey<string>(_cboPartitionMode, "Unattended"),
        PartitionLayout = UiHelpers.GetSelectedEnum(_cboPartitionLayout, PartitionLayout.GPT),
        RecoveryMode = UiHelpers.GetSelectedEnum(_cboRecoveryMode, RecoveryMode.Partition),
        SystemPartitionSize = (int)_numSystemSize.Value,
        RecoveryPartitionSize = (int)_numRecoverySize.Value,
        CustomPartitionScript = _txtCustomPartitionScript.Text,
        InstallFromMode = UiHelpers.GetSelectedKey<string>(_cboInstallFromMode, "Edition"),
        InstallFromIndex = (int)_numInstallFromIndex.Value,
        InstallFromName = _txtInstallFromName.Text,
        PagingFileMode = UiHelpers.GetSelectedKey<string>(_cboPagingFileMode, "Automatic"),
        PagingInitialMiB = (int)_numPagingInitial.Value,
        PagingMaxMiB = (int)_numPagingMax.Value,
        DisableDefender = _chkDisableDefender.Checked,
        Disable8Dot3Names = _chkDisable8Dot3.Checked,
        PauseBeforeFormatting = _chkPauseBeforeFormat.Checked,
        PauseBeforeReboot = _chkPauseBeforeReboot.Checked,
        CompactOs = _chkCompactOs.Checked,
        SkipIntegrityCheck = _chkSkipIntegrityCheck.Checked,

        // Tab 4
        AccountMode = UiHelpers.GetSelectedKey<string>(_cboAccountMode, "InteractiveMicrosoft"),
        AutoLogonMode = UiHelpers.GetSelectedKey<string>(_cboAutoLogonMode, "None"),
        BuiltinAdminPassword = _txtBuiltinAdminPwd.Text,
        ObscurePasswords = _chkObscurePasswords.Checked,
        PasswordExpirationMode = UiHelpers.GetSelectedKey<string>(_cboPwdExpMode, "Default"),
        PasswordMaxAgeDays = (int)_numPwdMaxAge.Value,
        LockoutMode = UiHelpers.GetSelectedKey<string>(_cboLockoutMode, "Default"),
        LockoutThreshold = (int)_numLockoutThreshold.Value,
        LockoutDurationMinutes = (int)_numLockoutDuration.Value,
        LockoutWindowMinutes = (int)_numLockoutWindow.Value,
        ProcessAuditMode = UiHelpers.GetSelectedKey<string>(_cboProcessAuditMode, "Disabled"),
        ProcessAuditIncludeCommandLine = _chkProcessAuditCmdLine.Checked,
        AppLockerMode = UiHelpers.GetSelectedKey<string>(_cboAppLockerMode, "Skip"),
        AppLockerPolicyXml = _txtAppLockerXml.Text,

        // Tab 5
        ComputerNameMode = UiHelpers.GetSelectedKey<string>(_cboComputerNameMode, "Random"),
        CustomComputerName = _txtCustomComputerName.Text,
        ComputerNameScript = _txtComputerNameScript.Text,
        WifiMode = UiHelpers.GetSelectedKey<string>(_cboWifiMode, "Interactive"),
        WifiName = _txtWifiName.Text,
        WifiPassword = _txtWifiPassword.Text,
        WifiAuthentication = UiHelpers.GetSelectedEnum(_cboWifiAuth, WifiAuthentications.WPA2PSK),
        WifiConnectAutomatically = _chkWifiAuto.Checked,
        WifiNonBroadcast = _chkWifiNonBroadcast.Checked,
        WifiProfileXml = _txtWifiXml.Text,
        EnableLongPaths = _chkEnableLongPaths.Checked,
        EnableRemoteDesktop = _chkEnableRemoteDesktop.Checked,
        HardenSystemDriveAcl = _chkHardenAcl.Checked,
        DeleteJunctions = _chkDeleteJunctions.Checked,
        AllowPowerShellScripts = _chkAllowPsScripts.Checked,
        DisableLastAccess = _chkDisableLastAccess.Checked,
        PreventAutomaticReboot = _chkPreventAutoReboot.Checked,
        DisableSac = _chkDisableSac.Checked,
        DisableUac = _chkDisableUac.Checked,
        DisableSmartScreen = _chkDisableSmartScreen.Checked,
        DisableSystemRestore = _chkDisableSystemRestore.Checked,
        DisableFastStartup = _chkDisableFastStartup.Checked,
        TurnOffSystemSounds = _chkTurnOffSounds.Checked,
        PreventDeviceEncryption = _chkPreventDeviceEncryption.Checked,
        DisableWindowsUpdate = _chkDisableWindowsUpdate.Checked,
        DeleteWindowsOld = _chkDeleteWindowsOld.Checked,
        DisableCoreIsolation = _chkDisableCoreIsolation.Checked,
        DisableAutomaticRestartSignOn = _chkDisableAutoRestartSignOn.Checked,
        DisableWpbt = _chkDisableWpbt.Checked,
        PreventDeviceApps = _chkPreventDeviceApps.Checked,
        KeepSensitiveFiles = _chkKeepSensitiveFiles.Checked,
        HidePowerShellWindows = _chkHidePsWindows.Checked,
        VBoxGuestAdditions = _chkVBoxGuest.Checked,
        VMwareTools = _chkVMwareTools.Checked,
        VirtIoGuestTools = _chkVirtIoGuest.Checked,
        ParallelsTools = _chkParallelsTools.Checked,

        // Tab 6
        ClassicContextMenu = _chkClassicContextMenu.Checked,
        LeftTaskbar = _chkLeftTaskbar.Checked,
        HideTaskViewButton = _chkHideTaskView.Checked,
        ShowFileExtensions = _chkShowFileExtensions.Checked,
        ShowAllTrayIcons = _chkShowAllTrayIcons.Checked,
        HideFiles = UiHelpers.GetSelectedEnum(_cboHideFiles, HideModes.Hidden),
        TaskbarSearch = UiHelpers.GetSelectedEnum(_cboTaskbarSearch, TaskbarSearchMode.Box),
        LaunchToThisPC = _chkLaunchToThisPC.Checked,
        ShowEndTask = _chkShowEndTask.Checked,
        HideInfoTip = _chkHideInfoTip.Checked,
        DisableAppSuggestions = _chkDisableAppSuggestions.Checked,
        DisableWidgets = _chkDisableWidgets.Checked,
        DisableBingResults = _chkDisableBing.Checked,
        DisablePointerPrecision = _chkDisablePointerPrecision.Checked,
        HideEdgeFre = _chkHideEdgeFre.Checked,
        DisableEdgeStartupBoost = _chkDisableEdgeBoost.Checked,
        MakeEdgeUninstallable = _chkMakeEdgeUninstallable.Checked,
        DeleteEdgeDesktopIcon = _chkDeleteEdgeIcon.Checked,
        WallpaperMode = UiHelpers.GetSelectedKey<string>(_cboWallpaperMode, "Default"),
        WallpaperColorHex = _btnWallpaperColor.Text,
        WallpaperScript = _txtWallpaperScript.Text,
        LockScreenMode = UiHelpers.GetSelectedKey<string>(_cboLockScreenMode, "Default"),
        LockScreenScript = _txtLockScreenScript.Text,
        ColorMode = UiHelpers.GetSelectedKey<string>(_cboColorMode, "Default"),
        SystemTheme = UiHelpers.GetSelectedEnum(_cboSystemTheme, ColorTheme.Dark),
        AppsTheme = UiHelpers.GetSelectedEnum(_cboAppsTheme, ColorTheme.Dark),
        EnableTransparency = _chkTransparency.Checked,
        AccentColorOnStart = _chkAccentOnStart.Checked,
        AccentColorOnBorders = _chkAccentOnBorders.Checked,
        AccentColorHex = _btnAccentColor.Text,
        EffectsMode = UiHelpers.GetSelectedKey<string>(_cboEffectsMode, "Default"),
        StartPinsMode = UiHelpers.GetSelectedKey<string>(_cboStartPinsMode, "Default"),
        CustomStartPinsJson = _txtStartPinsJson.Text,
        StartTilesMode = UiHelpers.GetSelectedKey<string>(_cboStartTilesMode, "Default"),
        CustomStartTilesXml = _txtStartTilesXml.Text,
        TaskbarIconsMode = UiHelpers.GetSelectedKey<string>(_cboTaskbarIconsMode, "Default"),
        CustomTaskbarIconsXml = _txtTaskbarIconsXml.Text,
        DesktopIconsMode = UiHelpers.GetSelectedKey<string>(_cboDesktopIconsMode, "Default"),
        StartFoldersMode = UiHelpers.GetSelectedKey<string>(_cboStartFoldersMode, "Default"),
        UseNarrator = _chkUseNarrator.Checked,
        LockKeysMode = UiHelpers.GetSelectedKey<string>(_cboLockKeysMode, "Skip"),
        CapsLockInitial = UiHelpers.GetSelectedEnum(_cboCapsInit, LockKeyInitial.Off),
        CapsLockBehavior = UiHelpers.GetSelectedEnum(_cboCapsBehav, LockKeyBehavior.Toggle),
        NumLockInitial = UiHelpers.GetSelectedEnum(_cboNumInit, LockKeyInitial.Off),
        NumLockBehavior = UiHelpers.GetSelectedEnum(_cboNumBehav, LockKeyBehavior.Toggle),
        ScrollLockInitial = UiHelpers.GetSelectedEnum(_cboScrollInit, LockKeyInitial.Off),
        ScrollLockBehavior = UiHelpers.GetSelectedEnum(_cboScrollBehav, LockKeyBehavior.Toggle),
        StickyKeysMode = UiHelpers.GetSelectedKey<string>(_cboStickyKeysMode, "Default"),

        // Tab 7 & 8
        SelectedBloatwareIds = _selectedBloatIds.OrderBy(x => x).ToList(),
        RestartExplorer = _chkRestartExplorer.Checked,
        Scripts = _scripts.Select(s => new ScriptProfileDto { Phase = s.Phase, Type = s.Type, Content = s.Content }).ToList()
      };

      p.Accounts.Clear();
      foreach (DataGridViewRow row in _gridAccounts.Rows)
      {
        if (row.IsNewRow) continue;
        string name = Convert.ToString(row.Cells["Name"].Value) ?? "";
        if (string.IsNullOrWhiteSpace(name)) continue;
        p.Accounts.Add(new AccountProfileDto
        {
          Name = name,
          DisplayName = Convert.ToString(row.Cells["DisplayName"].Value) ?? "",
          Password = Convert.ToString(row.Cells["Password"].Value) ?? "",
          Group = Convert.ToString(row.Cells["Group"].Value) ?? "Administrators"
        });
      }

      for (int i = 0; i < _chkListEffects.Items.Count; i++)
        if (_chkListEffects.Items[i] is ComboItem<Effect> ei)
          p.CustomEffects[ei.Value] = _chkListEffects.GetItemChecked(i);

      for (int i = 0; i < _chkListDesktopIcons.Items.Count; i++)
        if (_chkListDesktopIcons.Items[i] is ComboItem<DesktopIcon> di)
          p.CustomDesktopIcons[di.Key] = _chkListDesktopIcons.GetItemChecked(i);

      for (int i = 0; i < _chkListStartFolders.Items.Count; i++)
        if (_chkListStartFolders.Items[i] is ComboItem<StartFolder> sf)
          p.CustomStartFolders[sf.Key] = _chkListStartFolders.GetItemChecked(i);

      for (int i = 0; i < _chkListStickyKeys.Items.Count; i++)
        if (_chkListStickyKeys.Items[i] is ComboItem<StickyKeys> sk && _chkListStickyKeys.GetItemChecked(i))
          p.CustomStickyKeysFlags.Add(sk.Value);

      return p;
    }
  }
}
