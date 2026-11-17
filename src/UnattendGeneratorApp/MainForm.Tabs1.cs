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
    // Tab 1: Language & Region
    private CheckBox _chkArchX86 = new(), _chkArchAmd64 = new(), _chkArchArm64 = new();
    private ComboBox _cboLanguageMode = UiHelpers.CreateDropdown();
    private ComboBox _cboImageLanguage = UiHelpers.CreateDropdown();
    private ComboBox _cboLocale1 = UiHelpers.CreateDropdown(), _cboKeyboard1 = UiHelpers.CreateDropdown();
    private CheckBox _chkLocale2 = new(), _chkLocale3 = new();
    private ComboBox _cboLocale2 = UiHelpers.CreateDropdown(), _cboKeyboard2 = UiHelpers.CreateDropdown();
    private ComboBox _cboLocale3 = UiHelpers.CreateDropdown(), _cboKeyboard3 = UiHelpers.CreateDropdown();
    private ComboBox _cboGeoLocation = UiHelpers.CreateDropdown();
    private ComboBox _cboTimeZoneMode = UiHelpers.CreateDropdown();
    private ComboBox _cboTimeZone = UiHelpers.CreateDropdown();

    // Tab 2: Edition & Setup
    private ComboBox _cboPEMode = UiHelpers.CreateDropdown();
    private TextBox _txtPEScript = new() { Multiline = true, Height = 90, ScrollBars = ScrollBars.Both, Font = new Font("Consolas", 8.5f), WordWrap = false };
    private ComboBox _cboEditionMode = UiHelpers.CreateDropdown();
    private ComboBox _cboWindowsEdition = UiHelpers.CreateDropdown();
    private TextBox _txtCustomEditionKey = new() { Width = 240 };
    private TextBox _txtActivationKey = new() { Width = 240 };
    private CheckBox _chkBypassRequirements = new() { Text = "Bypass Windows 11 requirements check (TPM, Secure Boot, RAM, CPU)" };
    private CheckBox _chkUseConfigSet = new() { Text = "Use Configuration Set" };
    private CheckBox _chkBypassNetwork = new() { Text = "Bypass network requirement during OOBE (BypassNRO)" };
    private ComboBox _cboExpressSettings = UiHelpers.CreateDropdown();

    // Tab 3: Disk & Partitions
    private ComboBox _cboTargetDiskMode = UiHelpers.CreateDropdown();
    private NumericUpDown _numFixedDiskIndex = new() { Minimum = 0, Maximum = 32, Width = 90 };
    private TextBox _txtTargetDiskScript = new() { Multiline = true, Height = 80, ScrollBars = ScrollBars.Both, Font = new Font("Consolas", 8.5f), WordWrap = false };
    private CheckBox _chkGenMinSize = new() { Text = "Min Size (GiB):" }, _chkGenMaxSize = new() { Text = "Max Size (GiB):" }, _chkGenIndex = new() { Text = "Disk Index:" };
    private NumericUpDown _numGenMinSize = new() { Minimum = 1, Maximum = 100000, Value = 10, Width = 90 };
    private NumericUpDown _numGenMaxSize = new() { Minimum = 1, Maximum = 100000, Value = 1000, Width = 90 };
    private NumericUpDown _numGenIndex = new() { Minimum = 0, Maximum = 32, Value = 0, Width = 90 };
    private CheckBox _chkAssertNoPartitions = new() { Text = "Only select disk with no partitions" };
    private CheckBox _chkAssertInterfaceType = new() { Text = "Prefer/assert interface type" };
    private CheckBox _chkAssertMediaType = new() { Text = "Prefer/assert media type" };

    private ComboBox _cboPartitionMode = UiHelpers.CreateDropdown();
    private ComboBox _cboPartitionLayout = UiHelpers.CreateDropdown();
    private ComboBox _cboRecoveryMode = UiHelpers.CreateDropdown();
    private NumericUpDown _numSystemSize = new() { Minimum = 50, Maximum = 10000, Value = 100, Width = 90 };
    private NumericUpDown _numRecoverySize = new() { Minimum = 100, Maximum = 50000, Value = 1000, Width = 90 };
    private TextBox _txtCustomPartitionScript = new() { Multiline = true, Height = 90, ScrollBars = ScrollBars.Both, Font = new Font("Consolas", 8.5f), WordWrap = false };

    private ComboBox _cboInstallFromMode = UiHelpers.CreateDropdown();
    private NumericUpDown _numInstallFromIndex = new() { Minimum = 1, Maximum = 32, Value = 1, Width = 90 };
    private TextBox _txtInstallFromName = new() { Width = 240 };

    private ComboBox _cboPagingFileMode = UiHelpers.CreateDropdown();
    private NumericUpDown _numPagingInitial = new() { Minimum = 16, Maximum = 1048576, Value = 4096, Width = 90 };
    private NumericUpDown _numPagingMax = new() { Minimum = 16, Maximum = 1048576, Value = 4096, Width = 90 };

    private CheckBox _chkDisableDefender = new() { Text = "Disable Windows Defender (PE / Offline)" };
    private CheckBox _chkDisable8Dot3 = new() { Text = "Disable 8.3 short filenames" };
    private CheckBox _chkPauseBeforeFormat = new() { Text = "Pause before formatting" };
    private CheckBox _chkPauseBeforeReboot = new() { Text = "Pause before rebooting" };
    private CheckBox _chkCompactOs = new() { Text = "Enable Compact OS" };
    private CheckBox _chkSkipIntegrityCheck = new() { Text = "Skip integrity check" };

    // Tab 4: Accounts & Security
    private ComboBox _cboAccountMode = UiHelpers.CreateDropdown();
    private DataGridView _gridAccounts = new();
    private ComboBox _cboAutoLogonMode = UiHelpers.CreateDropdown();
    private TextBox _txtBuiltinAdminPwd = new() { Width = 220, UseSystemPasswordChar = true };
    private CheckBox _chkObscurePasswords = new() { Text = "Obscure passwords in autounattend.xml (Base64)" };

    private ComboBox _cboPwdExpMode = UiHelpers.CreateDropdown();
    private NumericUpDown _numPwdMaxAge = new() { Minimum = 1, Maximum = 999, Value = 42, Width = 90 };

    private ComboBox _cboLockoutMode = UiHelpers.CreateDropdown();
    private NumericUpDown _numLockoutThreshold = new() { Minimum = 0, Maximum = 999, Value = 5, Width = 90 };
    private NumericUpDown _numLockoutDuration = new() { Minimum = 1, Maximum = 99999, Value = 30, Width = 90 };
    private NumericUpDown _numLockoutWindow = new() { Minimum = 1, Maximum = 99999, Value = 30, Width = 90 };

    private ComboBox _cboProcessAuditMode = UiHelpers.CreateDropdown();
    private CheckBox _chkProcessAuditCmdLine = new() { Text = "Include command line in process creation events" };

    private ComboBox _cboAppLockerMode = UiHelpers.CreateDropdown();
    private TextBox _txtAppLockerXml = new() { Multiline = true, Height = 90, ScrollBars = ScrollBars.Both, Font = new Font("Consolas", 8.5f), WordWrap = false };

    private TabPage BuildLanguageTab()
    {
      var page = new TabPage("Language && Region");
      var scroll = UiHelpers.CreateScrollPanel();

      // Processor Architecture
      var grpArch = UiHelpers.CreateSection("Processor Architecture");
      var flowArch = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
      _chkArchAmd64.Text = "amd64 (64-bit)";
      _chkArchX86.Text = "x86 (32-bit)";
      _chkArchArm64.Text = "arm64";
      _chkArchAmd64.AutoSize = _chkArchX86.AutoSize = _chkArchArm64.AutoSize = true;
      flowArch.Controls.AddRange(new Control[] { _chkArchAmd64, _chkArchX86, _chkArchArm64 });
      grpArch.Controls.Add(flowArch);

      // Language & Keyboard
      var grpLang = UiHelpers.CreateSection("Language, Locale & Keyboard");
      var tblLang = UiHelpers.CreateFormTable();
      _cboLanguageMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Interactive", "Interactive (ask during Windows Setup / OOBE)", "Interactive"),
        new ComboItem<string>("Unattended", "Unattended (configure automatically)", "Unattended")
      });

      foreach (var item in _generator.ImageLanguages.Values.OrderBy(x => x.DisplayName))
        _cboImageLanguage.Items.Add(new ComboItem<ImageLanguage>(item.Id, $"{item.DisplayName} ({item.Id})", item));

      foreach (var item in _generator.UserLocales.Values.OrderBy(x => x.DisplayName))
      {
        var ci = new ComboItem<UserLocale>(item.Id, $"{item.DisplayName} ({item.Id})", item);
        _cboLocale1.Items.Add(ci);
        _cboLocale2.Items.Add(ci);
        _cboLocale3.Items.Add(ci);
      }

      foreach (var item in _generator.KeyboardIdentifiers.Values.OrderBy(x => x.DisplayName))
      {
        var ci = new ComboItem<KeyboardIdentifier>(item.Id, $"{item.DisplayName} ({item.Id})", item);
        _cboKeyboard1.Items.Add(ci);
        _cboKeyboard2.Items.Add(ci);
        _cboKeyboard3.Items.Add(ci);
      }

      foreach (var item in _generator.GeoLocations.Values.OrderBy(x => x.DisplayName))
        _cboGeoLocation.Items.Add(new ComboItem<GeoLocation>(item.Id, $"{item.DisplayName} ({item.Id})", item));

      _chkLocale2.Text = "Add 2nd input language / keyboard";
      _chkLocale2.AutoSize = true;
      _chkLocale3.Text = "Add 3rd input language / keyboard";
      _chkLocale3.AutoSize = true;

      UiHelpers.AddRow(tblLang, "Language Mode:", _cboLanguageMode);
      UiHelpers.AddRow(tblLang, "Windows Image Language:", _cboImageLanguage);
      UiHelpers.AddRow(tblLang, "Primary User Locale:", _cboLocale1);
      UiHelpers.AddRow(tblLang, "Primary Keyboard:", _cboKeyboard1);
      UiHelpers.AddFullRow(tblLang, _chkLocale2);
      UiHelpers.AddRow(tblLang, "2nd User Locale:", _cboLocale2);
      UiHelpers.AddRow(tblLang, "2nd Keyboard:", _cboKeyboard2);
      UiHelpers.AddFullRow(tblLang, _chkLocale3);
      UiHelpers.AddRow(tblLang, "3rd User Locale:", _cboLocale3);
      UiHelpers.AddRow(tblLang, "3rd Keyboard:", _cboKeyboard3);
      UiHelpers.AddRow(tblLang, "Home / Geo Location:", _cboGeoLocation);
      grpLang.Controls.Add(tblLang);

      // Time Zone
      var grpTz = UiHelpers.CreateSection("Time Zone");
      var tblTz = UiHelpers.CreateFormTable();
      _cboTimeZoneMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Implicit", "Implicit (determine automatically / default)", "Implicit"),
        new ComboItem<string>("Explicit", "Explicit (set specific time zone)", "Explicit")
      });
      foreach (var item in _generator.TimeOffsets.Values.OrderBy(x => x.DisplayName))
        _cboTimeZone.Items.Add(new ComboItem<TimeOffset>(item.Id, item.DisplayName, item));

      UiHelpers.AddRow(tblTz, "Time Zone Mode:", _cboTimeZoneMode);
      UiHelpers.AddRow(tblTz, "Time Zone:", _cboTimeZone);
      grpTz.Controls.Add(tblTz);

      void UpdateLangEnabled()
      {
        bool unatt = UiHelpers.GetSelectedKey<string>(_cboLanguageMode) == "Unattended";
        _cboImageLanguage.Enabled = unatt;
        _cboLocale1.Enabled = unatt;
        _cboKeyboard1.Enabled = unatt;
        _chkLocale2.Enabled = unatt;
        _cboLocale2.Enabled = unatt && _chkLocale2.Checked;
        _cboKeyboard2.Enabled = unatt && _chkLocale2.Checked;
        _chkLocale3.Enabled = unatt;
        _cboLocale3.Enabled = unatt && _chkLocale3.Checked;
        _cboKeyboard3.Enabled = unatt && _chkLocale3.Checked;
        _cboGeoLocation.Enabled = unatt;
        _cboTimeZone.Enabled = UiHelpers.GetSelectedKey<string>(_cboTimeZoneMode) == "Explicit";
      }

      _cboLanguageMode.SelectedIndexChanged += (_, _) => UpdateLangEnabled();
      _chkLocale2.CheckedChanged += (_, _) => UpdateLangEnabled();
      _chkLocale3.CheckedChanged += (_, _) => UpdateLangEnabled();
      _cboTimeZoneMode.SelectedIndexChanged += (_, _) => UpdateLangEnabled();

      scroll.Controls.Add(grpTz);
      scroll.Controls.Add(grpLang);
      scroll.Controls.Add(grpArch);
      page.Controls.Add(scroll);
      return page;
    }

    private TabPage BuildEditionSetupTab()
    {
      var page = new TabPage("Edition && Setup");
      var scroll = UiHelpers.CreateScrollPanel();

      var grpPE = UiHelpers.CreateSection("Windows Setup (PE) Mode");
      var tblPE = UiHelpers.CreateFormTable();
      _cboPEMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Default", "Default (Standard Windows Setup)", "Default"),
        new ComboItem<string>("Generate", "Generate (Automated Diskpart / PE Script)", "Generate"),
        new ComboItem<string>("Script", "Custom PE Script (cmd)", "Script")
      });
      _chkBypassRequirements.AutoSize = true;
      _chkUseConfigSet.AutoSize = true;

      UiHelpers.AddRow(tblPE, "Windows PE Mode:", _cboPEMode);
      UiHelpers.AddFullRow(tblPE, _chkBypassRequirements);
      UiHelpers.AddFullRow(tblPE, _chkUseConfigSet);
      UiHelpers.AddRow(tblPE, "Custom PE Script:", _txtPEScript);
      grpPE.Controls.Add(tblPE);

      var grpEd = UiHelpers.CreateSection("Windows Edition & Product Key");
      var tblEd = UiHelpers.CreateFormTable();
      _cboEditionMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Interactive", "Interactive (ask during Setup)", "Interactive"),
        new ComboItem<string>("Firmware", "Use product key from UEFI/BIOS firmware", "Firmware"),
        new ComboItem<string>("Unattended", "Unattended (select Windows edition)", "Unattended"),
        new ComboItem<string>("Custom", "Custom product key (selects edition)", "Custom")
      });
      foreach (var ed in _generator.WindowsEditions.Values.OrderBy(x => x.DisplayName))
        _cboWindowsEdition.Items.Add(new ComboItem<WindowsEdition>(ed.Id, $"{ed.DisplayName} ({ed.ProductKey})", ed));

      UiHelpers.AddRow(tblEd, "Edition Mode:", _cboEditionMode);
      UiHelpers.AddRow(tblEd, "Windows Edition:", _cboWindowsEdition);
      UiHelpers.AddRow(tblEd, "Custom Edition Key:", _txtCustomEditionKey);
      UiHelpers.AddRow(tblEd, "Activation Key (optional):", _txtActivationKey);
      grpEd.Controls.Add(tblEd);

      var grpOobe = UiHelpers.CreateSection("OOBE & Privacy Express Settings");
      var tblOobe = UiHelpers.CreateFormTable();
      _chkBypassNetwork.AutoSize = true;
      _cboExpressSettings.Items.AddRange(new object[]
      {
        new ComboItem<ExpressSettingsMode>("DisableAll", "Disable All (Privacy-focused)", ExpressSettingsMode.DisableAll),
        new ComboItem<ExpressSettingsMode>("EnableAll", "Enable All", ExpressSettingsMode.EnableAll),
        new ComboItem<ExpressSettingsMode>("Interactive", "Interactive (Ask during OOBE)", ExpressSettingsMode.Interactive)
      });

      UiHelpers.AddFullRow(tblOobe, _chkBypassNetwork);
      UiHelpers.AddRow(tblOobe, "Express Settings:", _cboExpressSettings);
      grpOobe.Controls.Add(tblOobe);

      void UpdateSetupEnabled()
      {
        string pe = UiHelpers.GetSelectedKey<string>(_cboPEMode);
        _chkBypassRequirements.Enabled = pe == "Default";
        _chkUseConfigSet.Enabled = pe == "Default";
        _txtPEScript.Enabled = pe == "Script";

        string ed = UiHelpers.GetSelectedKey<string>(_cboEditionMode);
        _cboWindowsEdition.Enabled = pe == "Default" && ed == "Unattended";
        _txtCustomEditionKey.Enabled = pe == "Default" && ed == "Custom";
        _cboEditionMode.Enabled = pe == "Default";
      }

      _cboPEMode.SelectedIndexChanged += (_, _) => UpdateSetupEnabled();
      _cboEditionMode.SelectedIndexChanged += (_, _) => UpdateSetupEnabled();

      scroll.Controls.Add(grpOobe);
      scroll.Controls.Add(grpEd);
      scroll.Controls.Add(grpPE);
      page.Controls.Add(scroll);
      return page;
    }

    private TabPage BuildDiskTab()
    {
      var page = new TabPage("Disk && Partitions");
      var scroll = UiHelpers.CreateScrollPanel();

      var note = new Label
      {
        Text = "Note: These options apply when 'Windows PE Mode' (Edition & Setup tab) is set to 'Generate'.",
        Dock = DockStyle.Top,
        AutoSize = true,
        ForeColor = Color.DimGray,
        Padding = new Padding(0, 0, 0, 8)
      };

      var grpTarget = UiHelpers.CreateSection("Target Disk Selection");
      var tblTarget = UiHelpers.CreateFormTable();
      _cboTargetDiskMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Generated", "Generated (Auto-select disk by rules)", "Generated"),
        new ComboItem<string>("Fixed", "Fixed Disk Index", "Fixed"),
        new ComboItem<string>("Interactive", "Interactive (Ask during Setup)", "Interactive"),
        new ComboItem<string>("Script", "Custom VBScript", "Script")
      });

      var pnlGenRules = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, WrapContents = true };
      _chkGenMinSize.AutoSize = _chkGenMaxSize.AutoSize = _chkGenIndex.AutoSize = true;
      pnlGenRules.Controls.AddRange(new Control[] { _chkGenMinSize, _numGenMinSize, _chkGenMaxSize, _numGenMaxSize, _chkGenIndex, _numGenIndex });

      _chkAssertNoPartitions.AutoSize = _chkAssertInterfaceType.AutoSize = _chkAssertMediaType.AutoSize = true;
      var pnlAsserts = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.TopDown };
      pnlAsserts.Controls.AddRange(new Control[] { _chkAssertNoPartitions, _chkAssertInterfaceType, _chkAssertMediaType });

      UiHelpers.AddRow(tblTarget, "Target Disk Mode:", _cboTargetDiskMode);
      UiHelpers.AddRow(tblTarget, "Fixed Disk Index:", _numFixedDiskIndex);
      UiHelpers.AddRow(tblTarget, "Generated Rules:", pnlGenRules);
      UiHelpers.AddFullRow(tblTarget, pnlAsserts);
      UiHelpers.AddRow(tblTarget, "Target Disk VBScript:", _txtTargetDiskScript);
      grpTarget.Controls.Add(tblTarget);

      var grpPart = UiHelpers.CreateSection("Partitioning");
      var tblPart = UiHelpers.CreateFormTable();
      _cboPartitionMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Unattended", "Unattended (Auto-partition)", "Unattended"),
        new ComboItem<string>("Interactive", "Interactive", "Interactive"),
        new ComboItem<string>("Custom", "Custom Diskpart Script", "Custom")
      });
      _cboPartitionLayout.Items.AddRange(new object[]
      {
        new ComboItem<PartitionLayout>("GPT", "GPT (UEFI)", PartitionLayout.GPT),
        new ComboItem<PartitionLayout>("MBR", "MBR (Legacy BIOS)", PartitionLayout.MBR),
        new ComboItem<PartitionLayout>("Automatic", "Automatic (Detect)", PartitionLayout.Automatic)
      });
      _cboRecoveryMode.Items.AddRange(new object[]
      {
        new ComboItem<RecoveryMode>("Partition", "Create Recovery Partition", RecoveryMode.Partition),
        new ComboItem<RecoveryMode>("None", "No Recovery Partition", RecoveryMode.None)
      });

      UiHelpers.AddRow(tblPart, "Partition Mode:", _cboPartitionMode);
      UiHelpers.AddRow(tblPart, "Partition Layout:", _cboPartitionLayout);
      UiHelpers.AddRow(tblPart, "Recovery Mode:", _cboRecoveryMode);
      UiHelpers.AddRow(tblPart, "System/ESP Size (MiB):", _numSystemSize);
      UiHelpers.AddRow(tblPart, "Recovery Size (MiB):", _numRecoverySize);
      UiHelpers.AddRow(tblPart, "Custom Diskpart Script:", _txtCustomPartitionScript);
      grpPart.Controls.Add(tblPart);

      var grpInstall = UiHelpers.CreateSection("Image Selection, Paging File & PE Options");
      var tblInstall = UiHelpers.CreateFormTable();
      _cboInstallFromMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Edition", "Match Selected Windows Edition", "Edition"),
        new ComboItem<string>("Index", "By Image Index", "Index"),
        new ComboItem<string>("Name", "By Image Name", "Name"),
        new ComboItem<string>("Interactive", "Interactive", "Interactive")
      });
      _cboPagingFileMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Automatic", "Automatic (Windows managed)", "Automatic"),
        new ComboItem<string>("None", "No Paging File", "None"),
        new ComboItem<string>("Custom", "Custom Size (MiB)", "Custom")
      });

      var pnlPaging = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
      pnlPaging.Controls.AddRange(new Control[]
      {
        new Label { Text = "Initial:", AutoSize = true, Margin = new Padding(0, 6, 4, 0) },
        _numPagingInitial,
        new Label { Text = "Max:", AutoSize = true, Margin = new Padding(8, 6, 4, 0) },
        _numPagingMax
      });

      _chkDisableDefender.AutoSize = _chkDisable8Dot3.AutoSize = _chkPauseBeforeFormat.AutoSize =
        _chkPauseBeforeReboot.AutoSize = _chkCompactOs.AutoSize = _chkSkipIntegrityCheck.AutoSize = true;

      UiHelpers.AddRow(tblInstall, "Install From:", _cboInstallFromMode);
      UiHelpers.AddRow(tblInstall, "Image Index:", _numInstallFromIndex);
      UiHelpers.AddRow(tblInstall, "Image Name:", _txtInstallFromName);
      UiHelpers.AddRow(tblInstall, "Paging File:", _cboPagingFileMode);
      UiHelpers.AddRow(tblInstall, "Paging File Size (MiB):", pnlPaging);
      UiHelpers.AddFullRow(tblInstall, _chkDisableDefender);
      UiHelpers.AddFullRow(tblInstall, _chkDisable8Dot3);
      UiHelpers.AddFullRow(tblInstall, _chkCompactOs);
      UiHelpers.AddFullRow(tblInstall, _chkSkipIntegrityCheck);
      UiHelpers.AddFullRow(tblInstall, _chkPauseBeforeFormat);
      UiHelpers.AddFullRow(tblInstall, _chkPauseBeforeReboot);
      grpInstall.Controls.Add(tblInstall);

      void UpdateDiskEnabled()
      {
        string td = UiHelpers.GetSelectedKey<string>(_cboTargetDiskMode);
        _numFixedDiskIndex.Enabled = td == "Fixed";
        _txtTargetDiskScript.Enabled = td == "Script";
        bool gen = td == "Generated";
        _chkGenMinSize.Enabled = _chkGenMaxSize.Enabled = _chkGenIndex.Enabled = gen;
        _numGenMinSize.Enabled = gen && _chkGenMinSize.Checked;
        _numGenMaxSize.Enabled = gen && _chkGenMaxSize.Checked;
        _numGenIndex.Enabled = gen && _chkGenIndex.Checked;
        _chkAssertNoPartitions.Enabled = _chkAssertInterfaceType.Enabled = _chkAssertMediaType.Enabled = gen;

        string pm = UiHelpers.GetSelectedKey<string>(_cboPartitionMode);
        _cboPartitionLayout.Enabled = _cboRecoveryMode.Enabled = _numSystemSize.Enabled = _numRecoverySize.Enabled = pm == "Unattended";
        _txtCustomPartitionScript.Enabled = pm == "Custom";

        string ifm = UiHelpers.GetSelectedKey<string>(_cboInstallFromMode);
        _numInstallFromIndex.Enabled = ifm == "Index";
        _txtInstallFromName.Enabled = ifm == "Name";

        bool customPaging = UiHelpers.GetSelectedKey<string>(_cboPagingFileMode) == "Custom";
        _numPagingInitial.Enabled = _numPagingMax.Enabled = customPaging;
      }

      _cboTargetDiskMode.SelectedIndexChanged += (_, _) => UpdateDiskEnabled();
      _chkGenMinSize.CheckedChanged += (_, _) => UpdateDiskEnabled();
      _chkGenMaxSize.CheckedChanged += (_, _) => UpdateDiskEnabled();
      _chkGenIndex.CheckedChanged += (_, _) => UpdateDiskEnabled();
      _cboPartitionMode.SelectedIndexChanged += (_, _) => UpdateDiskEnabled();
      _cboInstallFromMode.SelectedIndexChanged += (_, _) => UpdateDiskEnabled();
      _cboPagingFileMode.SelectedIndexChanged += (_, _) => UpdateDiskEnabled();

      scroll.Controls.Add(grpInstall);
      scroll.Controls.Add(grpPart);
      scroll.Controls.Add(grpTarget);
      scroll.Controls.Add(note);
      page.Controls.Add(scroll);
      return page;
    }

    private TabPage BuildAccountsTab()
    {
      var page = new TabPage("Accounts && Security");
      var scroll = UiHelpers.CreateScrollPanel();

      var grpAcc = UiHelpers.CreateSection("User Accounts");
      var tblAcc = UiHelpers.CreateFormTable();
      _cboAccountMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("InteractiveMicrosoft", "Interactive (Microsoft Account in OOBE)", "InteractiveMicrosoft"),
        new ComboItem<string>("InteractiveLocal", "Interactive (Local Account in OOBE)", "InteractiveLocal"),
        new ComboItem<string>("Unattended", "Unattended (Create local accounts automatically)", "Unattended")
      });

      _gridAccounts.Height = 135;
      _gridAccounts.Dock = DockStyle.Top;
      _gridAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      _gridAccounts.AllowUserToResizeRows = false;
      _gridAccounts.RowHeadersVisible = false;
      _gridAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Username" });
      _gridAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "DisplayName", HeaderText = "Display Name" });
      _gridAccounts.Columns.Add(new DataGridViewTextBoxColumn { Name = "Password", HeaderText = "Password" });
      var colGroup = new DataGridViewComboBoxColumn { Name = "Group", HeaderText = "Group" };
      colGroup.Items.AddRange("Administrators", "Users");
      _gridAccounts.Columns.Add(colGroup);

      _cboAutoLogonMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("None", "None (show sign-in screen)", "None"),
        new ComboItem<string>("Own", "Automatically log on first account", "Own"),
        new ComboItem<string>("Builtin", "Automatically log on Built-in Administrator", "Builtin")
      });
      _chkObscurePasswords.AutoSize = true;

      UiHelpers.AddRow(tblAcc, "Account Mode:", _cboAccountMode);
      UiHelpers.AddFullRow(tblAcc, _gridAccounts);
      UiHelpers.AddRow(tblAcc, "Auto Logon:", _cboAutoLogonMode);
      UiHelpers.AddRow(tblAcc, "Built-in Admin Password:", _txtBuiltinAdminPwd);
      UiHelpers.AddFullRow(tblAcc, _chkObscurePasswords);
      grpAcc.Controls.Add(tblAcc);

      var grpSec = UiHelpers.CreateSection("Password Expiration & Account Lockout");
      var tblSec = UiHelpers.CreateFormTable();
      _cboPwdExpMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Default", "Windows Default (42 days)", "Default"),
        new ComboItem<string>("Unlimited", "Unlimited (Never expires)", "Unlimited"),
        new ComboItem<string>("Custom", "Custom Max Age (days)", "Custom")
      });
      _cboLockoutMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Default", "Windows Default", "Default"),
        new ComboItem<string>("Disable", "Disable Account Lockout", "Disable"),
        new ComboItem<string>("Custom", "Custom Lockout Policy", "Custom")
      });

      var pnlLockout = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, WrapContents = true };
      pnlLockout.Controls.AddRange(new Control[]
      {
        new Label { Text = "Threshold:", AutoSize = true, Margin = new Padding(0, 6, 4, 0) }, _numLockoutThreshold,
        new Label { Text = "Duration (min):", AutoSize = true, Margin = new Padding(8, 6, 4, 0) }, _numLockoutDuration,
        new Label { Text = "Window (min):", AutoSize = true, Margin = new Padding(8, 6, 4, 0) }, _numLockoutWindow
      });

      UiHelpers.AddRow(tblSec, "Password Expiration:", _cboPwdExpMode);
      UiHelpers.AddRow(tblSec, "Max Password Age (days):", _numPwdMaxAge);
      UiHelpers.AddRow(tblSec, "Account Lockout:", _cboLockoutMode);
      UiHelpers.AddRow(tblSec, "Lockout Parameters:", pnlLockout);
      grpSec.Controls.Add(tblSec);

      var grpAudit = UiHelpers.CreateSection("Process Auditing & AppLocker");
      var tblAudit = UiHelpers.CreateFormTable();
      _cboProcessAuditMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Disabled", "Disabled", "Disabled"),
        new ComboItem<string>("Enabled", "Enabled", "Enabled")
      });
      _chkProcessAuditCmdLine.AutoSize = true;
      _cboAppLockerMode.Items.AddRange(new object[]
      {
        new ComboItem<string>("Skip", "Skip", "Skip"),
        new ComboItem<string>("Configure", "Configure AppLocker Policy XML", "Configure")
      });

      UiHelpers.AddRow(tblAudit, "Process Auditing:", _cboProcessAuditMode);
      UiHelpers.AddFullRow(tblAudit, _chkProcessAuditCmdLine);
      UiHelpers.AddRow(tblAudit, "AppLocker:", _cboAppLockerMode);
      UiHelpers.AddRow(tblAudit, "AppLocker Policy XML:", _txtAppLockerXml);
      grpAudit.Controls.Add(tblAudit);

      void UpdateAccountsEnabled()
      {
        bool unatt = UiHelpers.GetSelectedKey<string>(_cboAccountMode) == "Unattended";
        _gridAccounts.Enabled = unatt;
        _cboAutoLogonMode.Enabled = unatt;
        _chkObscurePasswords.Enabled = unatt;
        _txtBuiltinAdminPwd.Enabled = unatt && UiHelpers.GetSelectedKey<string>(_cboAutoLogonMode) == "Builtin";
        _numPwdMaxAge.Enabled = UiHelpers.GetSelectedKey<string>(_cboPwdExpMode) == "Custom";
        bool customLockout = UiHelpers.GetSelectedKey<string>(_cboLockoutMode) == "Custom";
        _numLockoutThreshold.Enabled = _numLockoutDuration.Enabled = _numLockoutWindow.Enabled = customLockout;
        _chkProcessAuditCmdLine.Enabled = UiHelpers.GetSelectedKey<string>(_cboProcessAuditMode) == "Enabled";
        _txtAppLockerXml.Enabled = UiHelpers.GetSelectedKey<string>(_cboAppLockerMode) == "Configure";
      }

      _cboAccountMode.SelectedIndexChanged += (_, _) => UpdateAccountsEnabled();
      _cboAutoLogonMode.SelectedIndexChanged += (_, _) => UpdateAccountsEnabled();
      _cboPwdExpMode.SelectedIndexChanged += (_, _) => UpdateAccountsEnabled();
      _cboLockoutMode.SelectedIndexChanged += (_, _) => UpdateAccountsEnabled();
      _cboProcessAuditMode.SelectedIndexChanged += (_, _) => UpdateAccountsEnabled();
      _cboAppLockerMode.SelectedIndexChanged += (_, _) => UpdateAccountsEnabled();

      scroll.Controls.Add(grpAudit);
      scroll.Controls.Add(grpSec);
      scroll.Controls.Add(grpAcc);
      page.Controls.Add(scroll);
      return page;
    }
  }
}
