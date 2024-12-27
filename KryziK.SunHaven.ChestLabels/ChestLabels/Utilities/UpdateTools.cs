// Decompiled with JetBrains decompiler
// Type: KryziK.Utilities.UpdateTools
// Assembly: KryziK.SunHaven.ChestLabels, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DD7A1110-A8B4-49E6-86A7-8703E93D227C
// Assembly location: C:\Users\ABC\Downloads\Chest Labels-150-1-0-5-1698843986 (1)\KryziK.SunHaven.ChestLabels.dll

using System;
using System.Net;
using System.Text.RegularExpressions;

namespace KryziK.Utilities
{
  internal static class UpdateTools
  {
    private static Regex NexusVersionRegex { get; } = new Regex("<div\\s+class=\"titlestat\">Version<\\/div>\\s+?<div\\s+class=\"stat\">([^<]+)<\\/div>", RegexOptions.Compiled | RegexOptions.Singleline);

    public static string GetLatestVersion(string modId)
    {
      if (modId == null)
        return (string) null;
      try
      {
        using (WebClient webClient = new WebClient())
        {
          Match match = UpdateTools.NexusVersionRegex.Match(webClient.DownloadString("https://www.nexusmods.com/sunhaven/mods/" + modId));
          return match.Success ? match.Groups[1].Value : (string) null;
        }
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }
  }
}
