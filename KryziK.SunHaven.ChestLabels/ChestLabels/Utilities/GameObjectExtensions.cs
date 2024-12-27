// Decompiled with JetBrains decompiler
// Type: KryziK.Utilities.GameObjectExtensions
// Assembly: KryziK.SunHaven.ChestLabels, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DD7A1110-A8B4-49E6-86A7-8703E93D227C
// Assembly location: C:\Users\ABC\Downloads\Chest Labels-150-1-0-5-1698843986 (1)\KryziK.SunHaven.ChestLabels.dll

namespace KryziK.Utilities
{
  internal static class GameObjectExtensions
  {
    public static bool IsActuallyA<T>(this object obj) => obj?.GetType() == typeof (T);

    public static bool IsActuallyA<T>(this object obj, out T actual)
    {
      bool flag = obj.IsActuallyA<T>();
      actual = flag ? (T) obj : default (T);
      return flag;
    }

    public static bool IsActuallyAn<T>(this object obj) => obj.IsActuallyA<T>();

    public static bool IsActuallyAn<T>(this object obj, out T actual)
    {
      return obj.IsActuallyA<T>(out actual);
    }

    public static bool IsNotActuallyA<T>(this object obj) => !obj.IsActuallyA<T>();

    public static bool IsNotActuallyA<T>(this object obj, out T actual)
    {
      return !obj.IsActuallyA<T>(out actual);
    }

    public static bool IsNotActuallyAn<T>(this object obj) => !obj.IsActuallyA<T>();

    public static bool IsNotActuallyAn<T>(this object obj, out T actual)
    {
      return !obj.IsActuallyA<T>(out actual);
    }
  }
}
