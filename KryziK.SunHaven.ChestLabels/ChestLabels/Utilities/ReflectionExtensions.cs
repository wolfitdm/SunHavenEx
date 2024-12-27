// Decompiled with JetBrains decompiler
// Type: KryziK.Utilities.ReflectionExtensions
// Assembly: KryziK.SunHaven.ChestLabels, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DD7A1110-A8B4-49E6-86A7-8703E93D227C
// Assembly location: C:\Users\ABC\Downloads\Chest Labels-150-1-0-5-1698843986 (1)\KryziK.SunHaven.ChestLabels.dll

using HarmonyLib;

namespace KryziK.Utilities
{
  internal static class ReflectionExtensions
  {
    public static void SmartInvoke(this object obj, string methodName, params object[] args)
    {
      AccessTools.Method(obj.GetType(), methodName).Invoke(obj, args);
    }

    public static T SmartInvoke<T>(this object obj, string methodName, params object[] args)
    {
      return (T) AccessTools.Method(obj.GetType(), methodName).Invoke(obj, args);
    }
  }
}
