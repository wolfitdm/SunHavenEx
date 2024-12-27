// Decompiled with JetBrains decompiler
// Type: KryziK.KryBasePlugin
// Assembly: KryziK.SunHaven.ChestLabels, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DD7A1110-A8B4-49E6-86A7-8703E93D227C
// Assembly location: C:\Users\ABC\Downloads\Chest Labels-150-1-0-5-1698843986 (1)\KryziK.SunHaven.ChestLabels.dll

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using KryziK.Utilities;
using System;
using System.Reflection;

namespace KryziK
{
  internal abstract class KryBasePlugin : BaseUnityPlugin
  {
    public static KryBasePlugin Instance { get; private set; }

    public new ConfigFile Config { get; }

    public new ManualLogSource Logger { get; }

    public ConfigEntry<bool> PluginEnabled { get; }

    public BepInPlugin PluginInfo { get; }

    private string ExternalId { get; }

    protected KryBasePlugin(string externalId = null)
    {
      if ((UnityEngine.Object) KryBasePlugin.Instance != (UnityEngine.Object) null)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this);
        throw new Exception("What are you doing?");
      }
      ConfigFile config = base.Config;
      string str = externalId;
      ManualLogSource logger = base.Logger;
      this.Config = config;
      this.ExternalId = str;
      KryBasePlugin.Instance = this;
      this.Logger = logger;
      this.PluginEnabled = this.Config.Bind<bool>("General", "Enabled", true, "Enables or disables the plugin.");
      this.PluginInfo = this.GetType().GetCustomAttribute<BepInPlugin>();
    }

    protected virtual void Awake()
    {
      this.Logger.LogInfo((object) string.Format("{0} v{1} is loaded! Enabled: {2}", (object) this.PluginInfo.GUID, (object) this.PluginInfo.Version, (object) this.PluginEnabled.Value));
      if (this.PluginEnabled.Value)
        new Harmony(this.PluginInfo.GUID).PatchAll(Assembly.GetExecutingAssembly());
      string latestVersion = UpdateTools.GetLatestVersion(this.ExternalId);
      if (latestVersion == null || !(latestVersion != this.PluginInfo.Version.ToString()))
        return;
      this.Logger.LogWarning((object) string.Format("There is a new version of {0} available! Current: {1}, Latest: {2}", (object) this.PluginInfo.Name, (object) this.PluginInfo.Version, (object) latestVersion));
    }
  }
}
