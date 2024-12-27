// Decompiled with JetBrains decompiler
// Type: KryziK.SunHaven.ChestLabels.Extensions.ChestExtensions
// Assembly: KryziK.SunHaven.ChestLabels, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DD7A1110-A8B4-49E6-86A7-8703E93D227C
// Assembly location: C:\Users\ABC\Downloads\Chest Labels-150-1-0-5-1698843986 (1)\KryziK.SunHaven.ChestLabels.dll

using HarmonyLib;
using System.Reflection;
using UnityEngine;
using Wish;

namespace KryziK.SunHaven.ChestLabels.Extensions
{
  internal static class ChestExtensions
  {
    public static FieldInfo DataField { get; } = AccessTools.Field(typeof (Chest), "data");

    public static ChestData GetData(this Chest chest)
    {
      return ((Object) chest != (Object) null ? ChestExtensions.DataField?.GetValue((object) chest) as ChestData : (ChestData) null) ?? new ChestData();
    }
  }
}
