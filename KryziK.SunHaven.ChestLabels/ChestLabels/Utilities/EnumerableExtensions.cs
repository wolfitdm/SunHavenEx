// Decompiled with JetBrains decompiler
// Type: KryziK.Utilities.EnumerableExtensions
// Assembly: KryziK.SunHaven.ChestLabels, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DD7A1110-A8B4-49E6-86A7-8703E93D227C
// Assembly location: C:\Users\ABC\Downloads\Chest Labels-150-1-0-5-1698843986 (1)\KryziK.SunHaven.ChestLabels.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace KryziK.Utilities
{
  internal static class EnumerableExtensions
  {
    public static string StringJoin<T>(this IEnumerable<T> source, string separator = ", ")
    {
      return string.Join<T>(separator, source);
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
      foreach (T obj in source)
        action(obj);
    }

    public static IEnumerable<T> DistinctBy<T, TKey>(
      this IEnumerable<T> source,
      Func<T, TKey> keySelector,
      Func<IEnumerable<T>, T> distinctor = null)
    {
      return source.GroupBy<T, TKey, T>(keySelector, (Func<TKey, IEnumerable<T>, T>) ((key, grp) => (distinctor ?? (Func<IEnumerable<T>, T>) (e => e.First<T>()))(grp)));
    }
  }
}
