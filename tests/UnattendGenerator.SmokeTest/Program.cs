using System;
using System.IO;
using System.Text;
using Schneegans.Unattend;
using UnattendGeneratorApp.Models;

namespace UnattendGenerator.SmokeTest
{
  internal static class Program
  {
    private static int Main()
    {
      var gen = new Schneegans.Unattend.UnattendGenerator();
      Console.WriteLine($"Resources loaded: Editions={gen.WindowsEditions.Count}, Bloatwares={gen.Bloatwares.Count}, Locales={gen.UserLocales.Count}");

      // 1. Default profile
      var def = new AppProfile();
      var doc1 = gen.GenerateXml(def.ToConfiguration(gen));
      var bytes1 = Schneegans.Unattend.UnattendGenerator.Serialize(doc1);
      Console.WriteLine($"Default profile XML: {bytes1.Length} bytes");

      // 2. Customized profile + JSON round-trip
      var custom = new AppProfile
      {
        LanguageMode = "Unattended",
        ImageLanguageId = "de-DE",
        Locale1Id = "de-DE",
        Keyboard1Id = "00000407",
        GeoLocationId = "94",
        TimeZoneMode = "Explicit",
        TimeZoneId = "W. Europe Standard Time",
        PEMode = "Generate",
        EditionMode = "Unattended",
        WindowsEditionId = "pro",
        AccountMode = "Unattended",
        AutoLogonMode = "Own",
        ComputerNameMode = "Custom",
        CustomComputerName = "WORKSTATION01",
        WifiMode = "Parameterized",
        WifiName = "CorpNet",
        WifiPassword = "SuperSecretPassword123",
        ClassicContextMenu = true,
        ShowFileExtensions = true,
      };
      custom.SelectedBloatwareIds.Add("RemoveTeams");
      custom.SelectedBloatwareIds.Add("RemoveCopilot");
      custom.Scripts.Add(new ScriptProfileDto
      {
        Phase = ScriptPhase.FirstLogon,
        Type = ScriptType.Ps1,
        Content = "Write-Host 'Hello from Unattend';"
      });

      string tempPath = Path.Combine(Path.GetTempPath(), "unattend-smoke-profile.json");
      ProfileManager.Save(custom, tempPath);
      var loaded = ProfileManager.Load(tempPath);
      File.Delete(tempPath);

      var doc2 = gen.GenerateXml(loaded.ToConfiguration(gen));
      var bytes2 = Schneegans.Unattend.UnattendGenerator.Serialize(doc2);
      string xmlText = Encoding.ASCII.GetString(bytes2);
      if (!xmlText.Contains("WORKSTATION01") || !xmlText.Contains("CorpNet"))
      {
        Console.Error.WriteLine("Custom settings missing from generated XML!");
        return 1;
      }
      Console.WriteLine($"Custom profile round-trip XML: {bytes2.Length} bytes");
      return 0;
    }
  }
}
