using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Schneegans.Unattend;
using UnattendGeneratorApp.Models;
using UnattendGeneratorApp.UI;

namespace UnattendGeneratorApp
{
  public partial class MainForm : Form
  {
    private readonly UnattendGenerator _generator = new();
    private readonly TabControl _tabControl = new();
    private readonly TextBox _xmlPreview = new();
    private readonly Label _statusLabel = new();
    private readonly Label _validationLabel = new();
    private byte[]? _lastXmlBytes;
    private string? _currentProfilePath;

    public MainForm()
    {
      Text = "Windows Unattend Generator (.NET 4.8)";
      Size = new Size(1340, 860);
      MinimumSize = new Size(1050, 680);
      StartPosition = FormStartPosition.CenterScreen;
      Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

      InitializeLayout();

      var initialProfile = ProfileManager.LoadLastOrDefault();
      LoadProfileToUi(initialProfile);
      GeneratePreview();
    }

    private void InitializeLayout()
    {
      var toolStrip = new ToolStrip
      {
        GripStyle = ToolStripGripStyle.Hidden,
        Padding = new Padding(6, 4, 6, 4),
        RenderMode = ToolStripRenderMode.System
      };

      var btnLoadProfile = new ToolStripButton("Load Profile...") { DisplayStyle = ToolStripItemDisplayStyle.Text };
      btnLoadProfile.Click += (_, _) => LoadProfileFromFile();

      var btnSaveProfile = new ToolStripButton("Save Profile...") { DisplayStyle = ToolStripItemDisplayStyle.Text };
      btnSaveProfile.Click += (_, _) => SaveProfileToFile();

      var btnReset = new ToolStripButton("Reset Defaults") { DisplayStyle = ToolStripItemDisplayStyle.Text };
      btnReset.Click += (_, _) =>
      {
        if (MessageBox.Show(this, "Reset all settings to defaults?", "Reset Defaults", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          _currentProfilePath = null;
          LoadProfileToUi(new AppProfile());
          GeneratePreview();
          UpdateStatus("Reset to default settings.");
        }
      };

      var btnGenerate = new ToolStripButton("Generate Preview") { DisplayStyle = ToolStripItemDisplayStyle.Text, Font = new Font(Font, FontStyle.Bold) };
      btnGenerate.Click += (_, _) => GeneratePreview();

      var btnSaveXml = new ToolStripButton("Save autounattend.xml...") { DisplayStyle = ToolStripItemDisplayStyle.Text };
      btnSaveXml.Click += (_, _) => SaveXmlToFile();

      var btnCopyXml = new ToolStripButton("Copy XML") { DisplayStyle = ToolStripItemDisplayStyle.Text };
      btnCopyXml.Click += (_, _) => CopyXmlToClipboard();

      toolStrip.Items.Add(btnLoadProfile);
      toolStrip.Items.Add(btnSaveProfile);
      toolStrip.Items.Add(btnReset);
      toolStrip.Items.Add(new ToolStripSeparator());
      toolStrip.Items.Add(btnGenerate);
      toolStrip.Items.Add(btnSaveXml);
      toolStrip.Items.Add(btnCopyXml);

      var statusStrip = new StatusStrip();
      _statusLabel.Text = "Ready";
      var statusHost = new ToolStripStatusLabel { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
      statusHost.Text = "Ready";
      statusStrip.Items.Add(statusHost);
      // Keep a reference via Tag or direct update helper
      _statusLabel.Tag = statusHost;

      var split = new SplitContainer
      {
        Dock = DockStyle.Fill,
        Orientation = Orientation.Vertical
      };
      Load += (_, _) =>
      {
        split.Panel1MinSize = 420;
        split.Panel2MinSize = 300;
        if (split.Width > 720)
        {
          split.SplitterDistance = (int)(split.Width * 0.56);
        }
      };

      _tabControl.Dock = DockStyle.Fill;
      _tabControl.Padding = new Point(10, 5);
      _tabControl.TabPages.Add(BuildLanguageTab());
      _tabControl.TabPages.Add(BuildEditionSetupTab());
      _tabControl.TabPages.Add(BuildDiskTab());
      _tabControl.TabPages.Add(BuildAccountsTab());
      _tabControl.TabPages.Add(BuildSystemNetworkTab());
      _tabControl.TabPages.Add(BuildUiPersonalizationTab());
      _tabControl.TabPages.Add(BuildBloatwareTab());
      _tabControl.TabPages.Add(BuildScriptsTab());
      split.Panel1.Controls.Add(_tabControl);

      var rightPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(6) };

      var previewHeader = new Label
      {
        Text = "autounattend.xml Preview",
        Dock = DockStyle.Top,
        Font = new Font(Font, FontStyle.Bold),
        Height = 24
      };

      _validationLabel.Dock = DockStyle.Top;
      _validationLabel.AutoSize = false;
      _validationLabel.Height = 36;
      _validationLabel.ForeColor = Color.DarkGreen;
      _validationLabel.Text = "Valid XML";

      _xmlPreview.Dock = DockStyle.Fill;
      _xmlPreview.Multiline = true;
      _xmlPreview.ReadOnly = true;
      _xmlPreview.ScrollBars = ScrollBars.Both;
      _xmlPreview.WordWrap = false;
      _xmlPreview.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
      _xmlPreview.BackColor = Color.FromArgb(250, 250, 250);

      rightPanel.Controls.Add(_xmlPreview);
      rightPanel.Controls.Add(_validationLabel);
      rightPanel.Controls.Add(previewHeader);
      split.Panel2.Controls.Add(rightPanel);

      Controls.Add(split);
      Controls.Add(toolStrip);
      Controls.Add(statusStrip);
    }

    private void UpdateStatus(string text)
    {
      if (_statusLabel.Tag is ToolStripStatusLabel item)
      {
        item.Text = text;
      }
    }

    private bool GeneratePreview()
    {
      try
      {
        var profile = SaveUiToProfile();
        Configuration config = profile.ToConfiguration(_generator);
        XmlDocument doc = _generator.GenerateXml(config);
        _lastXmlBytes = UnattendGenerator.Serialize(doc);
        _xmlPreview.Text = Encoding.ASCII.GetString(_lastXmlBytes);
        _validationLabel.ForeColor = Color.DarkGreen;
        _validationLabel.Text = $"Generated and schema-validated ({_lastXmlBytes.Length:N0} bytes).";
        ProfileManager.SaveLast(profile);
        UpdateStatus(_currentProfilePath != null
          ? $"Preview updated | Profile: {Path.GetFileName(_currentProfilePath)}"
          : "Preview updated");
        return true;
      }
      catch (Exception ex)
      {
        _lastXmlBytes = null;
        _validationLabel.ForeColor = Color.Firebrick;
        _validationLabel.Text = "Configuration error: " + ex.Message;
        UpdateStatus("Configuration error");
        return false;
      }
    }

    private void SaveXmlToFile()
    {
      if (!GeneratePreview() || _lastXmlBytes == null)
      {
        MessageBox.Show(this, _validationLabel.Text, "Cannot Generate XML", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      using var dlg = new SaveFileDialog
      {
        Title = "Save autounattend.xml",
        Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
        FileName = "autounattend.xml",
        DefaultExt = "xml"
      };
      if (dlg.ShowDialog(this) == DialogResult.OK)
      {
        File.WriteAllBytes(dlg.FileName, _lastXmlBytes);
        UpdateStatus($"Saved XML to {dlg.FileName}");
      }
    }

    private void CopyXmlToClipboard()
    {
      if (!GeneratePreview() || string.IsNullOrEmpty(_xmlPreview.Text))
      {
        MessageBox.Show(this, _validationLabel.Text, "Cannot Generate XML", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }
      Clipboard.SetText(_xmlPreview.Text);
      UpdateStatus("Copied autounattend.xml to clipboard.");
    }

    private void SaveProfileToFile()
    {
      using var dlg = new SaveFileDialog
      {
        Title = "Save Settings Profile",
        Filter = "JSON Profile (*.json)|*.json|All Files (*.*)|*.*",
        FileName = _currentProfilePath != null ? Path.GetFileName(_currentProfilePath) : "unattend-profile.json",
        DefaultExt = "json"
      };
      if (dlg.ShowDialog(this) == DialogResult.OK)
      {
        var profile = SaveUiToProfile();
        ProfileManager.Save(profile, dlg.FileName);
        _currentProfilePath = dlg.FileName;
        UpdateStatus($"Saved profile: {dlg.FileName}");
      }
    }

    private void LoadProfileFromFile()
    {
      using var dlg = new OpenFileDialog
      {
        Title = "Load Settings Profile",
        Filter = "JSON Profile (*.json)|*.json|All Files (*.*)|*.*"
      };
      if (dlg.ShowDialog(this) == DialogResult.OK)
      {
        try
        {
          var profile = ProfileManager.Load(dlg.FileName);
          _currentProfilePath = dlg.FileName;
          LoadProfileToUi(profile);
          GeneratePreview();
          UpdateStatus($"Loaded profile: {dlg.FileName}");
        }
        catch (Exception ex)
        {
          MessageBox.Show(this, $"Failed to load profile:\n{ex.Message}", "Load Profile Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
      try
      {
        var profile = SaveUiToProfile();
        ProfileManager.SaveLast(profile);
      }
      catch
      {
        // Ignore save errors on exit
      }
      base.OnFormClosing(e);
    }
  }
}
