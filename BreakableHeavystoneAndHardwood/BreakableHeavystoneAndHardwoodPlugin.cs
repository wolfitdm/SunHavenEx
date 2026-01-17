using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using Wish;
using UnityEngine;

using System;

namespace BreakableHeavystoneAndHardwood;


[BepInPlugin("vurawnica.sunhaven.breakableheavystoneandhardwood", "BreakableHeavystoneAndHardwood", "0.0.2")]
public class BreakableHeavystoneAndHardwoodPlugin : BaseUnityPlugin
{
    private Harmony m_harmony = new Harmony("vurawnica.sunhaven.breakableheavystoneandhardwood");
    public static ManualLogSource logger;

	private static ConfigEntry<float> m_required_power;

    private void Awake()
    {
        // Plugin startup logic
        BreakableHeavystoneAndHardwoodPlugin.logger = this.Logger;
        logger.LogInfo((object) $"Plugin BreakableHeavystoneAndHardwood is loaded!");
        m_required_power = this.Config.Bind<float>("General", "Required Tool Level for Heavystone/Hardwood", 0, "3 is the vanilla value indicating adamant, 0 is the default for this mod");
		this.m_harmony.PatchAll();
	}

    [HarmonyPatch(typeof(Rock), "Hit")]
    [HarmonyPrefix]
    public static bool HarmonyPatch_Rock_Hit_Prefix(float damage, float power, bool crit, bool hitFromLocalPlayer, bool homeIn, float rustyKeyDropMultiplier, bool brokeUsingPickaxe, ref float ___requiredPower)
    {
        if (___requiredPower != 0)
        {
            ___requiredPower = m_required_power.Value;
        }
        return true;
    }

    [HarmonyPatch(typeof(Wood), "Hit")]
    [HarmonyPrefix]
    public static bool HarmonyPatch_Wood_Hit_Prefix(float damage, float power, bool crit, bool hitFromLocalPlayer, bool homeIn, ref float ___requiredPower)
    {
        if (___requiredPower != 0)
        {
            ___requiredPower = m_required_power.Value;
        }
        return true;
    }
}