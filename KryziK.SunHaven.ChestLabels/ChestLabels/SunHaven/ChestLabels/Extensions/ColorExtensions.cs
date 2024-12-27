// Decompiled with JetBrains decompiler
// Type: KryziK.SunHaven.ChestLabels.Extensions.ColorExtensions
// Assembly: KryziK.SunHaven.ChestLabels, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DD7A1110-A8B4-49E6-86A7-8703E93D227C
// Assembly location: C:\Users\ABC\Downloads\Chest Labels-150-1-0-5-1698843986 (1)\KryziK.SunHaven.ChestLabels.dll

using UnityEngine;

namespace KryziK.SunHaven.ChestLabels.Extensions
{
  internal static class ColorExtensions
  {
    public static Color32 ToColor(this int hexVal)
    {
      return new Color32((byte) (hexVal >> 16 & (int) byte.MaxValue), (byte) (hexVal >> 8 & (int) byte.MaxValue), (byte) (hexVal & (int) byte.MaxValue), byte.MaxValue);
    }
  }
}
