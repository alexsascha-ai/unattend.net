using System;
using System.Windows.Forms;

namespace UnattendGeneratorApp
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      try
      {
        Application.Run(new MainForm());
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString(), "UnattendGeneratorApp - Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
