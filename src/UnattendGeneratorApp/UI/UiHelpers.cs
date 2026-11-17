using System;
using System.Drawing;
using System.Windows.Forms;

namespace UnattendGeneratorApp.UI
{
  public sealed class ComboItem<T>
  {
    public string Key { get; }
    public string Display { get; }
    public T Value { get; }

    public ComboItem(string key, string display, T value)
    {
      Key = key;
      Display = display;
      Value = value;
    }

    public override string ToString() => Display;
  }

  public static class UiHelpers
  {
    public static Panel CreateScrollPanel()
    {
      return new Panel
      {
        Dock = DockStyle.Fill,
        AutoScroll = true,
        Padding = new Padding(10)
      };
    }

    public static GroupBox CreateSection(string title)
    {
      return new GroupBox
      {
        Text = title,
        Dock = DockStyle.Top,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        Padding = new Padding(10, 8, 10, 10),
        Margin = new Padding(0, 0, 0, 10)
      };
    }

    public static TableLayoutPanel CreateFormTable()
    {
      var table = new TableLayoutPanel
      {
        Dock = DockStyle.Top,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink,
        ColumnCount = 2,
        Margin = new Padding(0)
      };
      table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210));
      table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
      return table;
    }

    public static void AddRow(TableLayoutPanel table, string labelText, Control control)
    {
      int row = table.RowCount++;
      table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

      var label = new Label
      {
        Text = labelText,
        AutoSize = true,
        Anchor = AnchorStyles.Left,
        Margin = new Padding(0, 6, 8, 6)
      };

      control.Margin = new Padding(0, 3, 0, 3);
      if (control is ComboBox || control is TextBox)
      {
        control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      }
      else
      {
        control.Anchor = AnchorStyles.Left;
      }

      table.Controls.Add(label, 0, row);
      table.Controls.Add(control, 1, row);
    }

    public static void AddFullRow(TableLayoutPanel table, Control control)
    {
      int row = table.RowCount++;
      table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
      control.Margin = new Padding(0, 3, 0, 3);
      control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      table.Controls.Add(control, 0, row);
      table.SetColumnSpan(control, 2);
    }

    public static void SelectByKey<T>(ComboBox combo, string key)
    {
      for (int i = 0; i < combo.Items.Count; i++)
      {
        if (combo.Items[i] is ComboItem<T> item && string.Equals(item.Key, key, StringComparison.OrdinalIgnoreCase))
        {
          combo.SelectedIndex = i;
          return;
        }
      }
      if (combo.Items.Count > 0 && combo.SelectedIndex < 0)
      {
        combo.SelectedIndex = 0;
      }
    }

    public static string GetSelectedKey<T>(ComboBox combo, string fallback = "")
    {
      return combo.SelectedItem is ComboItem<T> item ? item.Key : fallback;
    }

    public static void SelectEnum<TEnum>(ComboBox combo, TEnum value) where TEnum : struct
    {
      for (int i = 0; i < combo.Items.Count; i++)
      {
        if (combo.Items[i] is ComboItem<TEnum> item && item.Value.Equals(value))
        {
          combo.SelectedIndex = i;
          return;
        }
      }
      if (combo.Items.Count > 0 && combo.SelectedIndex < 0)
      {
        combo.SelectedIndex = 0;
      }
    }

    public static TEnum GetSelectedEnum<TEnum>(ComboBox combo, TEnum fallback = default) where TEnum : struct
    {
      return combo.SelectedItem is ComboItem<TEnum> item ? item.Value : fallback;
    }

    public static ComboBox CreateDropdown()
    {
      return new ComboBox
      {
        DropDownStyle = ComboBoxStyle.DropDownList,
        Width = 340
      };
    }

    public static string ColorToHex(Color color)
    {
      return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    public static Color HexToColor(string hex, Color fallback)
    {
      try
      {
        return ColorTranslator.FromHtml(hex);
      }
      catch
      {
        return fallback;
      }
    }
  }
}
