// Decompiled with JetBrains decompiler
// Type: KryziK.SunHaven.ChestLabels.ChestLabelsPlugin
// Assembly: KryziK.SunHaven.ChestLabels, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DD7A1110-A8B4-49E6-86A7-8703E93D227C
// Assembly location: C:\Users\ABC\Downloads\Chest Labels-150-1-0-5-1698843986 (1)\KryziK.SunHaven.ChestLabels.dll

using BepInEx;
using BepInEx.Configuration;

namespace KryziK.SunHaven.ChestLabels
{
  [BepInPlugin("KryziK.SunHaven.ChestLabels", "Chest Labels", "1.0.6")]
  internal class ChestLabelsPlugin : KryBasePlugin
  {
    public static ChestLabelsPlugin Instance => (ChestLabelsPlugin) KryBasePlugin.Instance;

    public static ConfigEntry<ComponentVisibility> LabelVisibility { get; private set; }

    public static ConfigEntry<ComponentVisibility> IconVisibility { get; private set; }

    private ChestLabelsPlugin()
      : base("150")
    {
      ChestLabelsPlugin.LabelVisibility = ChestLabelsPlugin.Instance.Config.Bind<ComponentVisibility>("Visibility", "Labels", ComponentVisibility.Visible, "Visibility of chest labels.");
      ChestLabelsPlugin.IconVisibility = ChestLabelsPlugin.Instance.Config.Bind<ComponentVisibility>("Visibility", "Icons", ComponentVisibility.Visible, "Visibility of chest icons.");
    }
  }
}
