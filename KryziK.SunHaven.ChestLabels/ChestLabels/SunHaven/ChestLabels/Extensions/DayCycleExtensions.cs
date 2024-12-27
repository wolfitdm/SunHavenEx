// Decompiled with JetBrains decompiler
// Type: KryziK.SunHaven.ChestLabels.Extensions.DayCycleExtensions
// Assembly: KryziK.SunHaven.ChestLabels, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DD7A1110-A8B4-49E6-86A7-8703E93D227C
// Assembly location: C:\Users\ABC\Downloads\Chest Labels-150-1-0-5-1698843986 (1)\KryziK.SunHaven.ChestLabels.dll

using HarmonyLib;
using System.Reflection;
using TMPro;
using UnityEngine;
using Wish;

namespace KryziK.SunHaven.ChestLabels.Extensions
{
  internal static class DayCycleExtensions
  {
    private static FieldInfo yearTextField { get; } = AccessTools.Field(typeof (DayCycle), "_yearTMP");

    public static TextMeshProUGUI GetYearUI(this DayCycle dayCycle)
    {
      return !((Object) dayCycle != (Object) null) ? (TextMeshProUGUI) null : DayCycleExtensions.yearTextField.GetValue((object) dayCycle) as TextMeshProUGUI;
    }
  }
}
