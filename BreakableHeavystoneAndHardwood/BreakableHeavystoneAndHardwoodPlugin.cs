using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Rewired;
using System;
using UnityEngine;
using Wish;

namespace BreakableHeavystoneAndHardwood;


[BepInPlugin("vurawnica.sunhaven.breakableheavystoneandhardwood", "BreakableHeavystoneAndHardwood", "0.0.2")]
public class BreakableHeavystoneAndHardwoodPlugin : BaseUnityPlugin
{
    private Harmony m_harmony = new Harmony("vurawnica.sunhaven.breakableheavystoneandhardwood");
    public static ManualLogSource logger;

	private static ConfigEntry<float> m_required_power;
    private static ConfigEntry<float> base_damage;

    private static float requiredPower = 0;
    private static float baseDamage = 0;

    private void Awake()
    {
        // Plugin startup logic
        BreakableHeavystoneAndHardwoodPlugin.logger = this.Logger;
        logger.LogInfo((object) $"Plugin BreakableHeavystoneAndHardwood is loaded!");
        m_required_power = this.Config.Bind<float>("General", "Required Tool Level for Heavystone/Hardwood", 0, "3 is the vanilla value indicating adamant, 0 is the default for this mod");
        base_damage = this.Config.Bind<float>("General", "basic damage that you cause to Tree/Stone/Heavystone/Hardwood", 0, "0 is the vanilla default value");
        requiredPower = m_required_power.Value;
        baseDamage = base_damage.Value;
        if (requiredPower <= 0)
            requiredPower = 0;
        if (baseDamage <= 0)
            baseDamage = 0;
        Harmony.CreateAndPatchAll(typeof(BreakableHeavystoneAndHardwoodPlugin));
	}

    [HarmonyPatch(typeof(Rock), "Hit")]
    [HarmonyPrefix]
    public static bool HarmonyPatch_Rock_Hit_Prefix(float damage, float power, bool crit, bool hitFromLocalPlayer, bool homeIn, float rustyKeyDropMultiplier, bool brokeUsingPickaxe, ref float ____requiredPower, ref float ____currentHealth)
    {
        if (____requiredPower != 0)
        {
            ____requiredPower = requiredPower;
        }
        if (baseDamage > 0 && power >= ____requiredPower)
        {
            ____currentHealth -= baseDamage;
        }
        return true;
    }


    [HarmonyPatch(typeof(Wood), "Hit")]
    [HarmonyPrefix]
    public static bool HarmonyPatch_Wood_Hit_Prefix(float damage, float power, bool crit, bool hitFromLocalPlayer, bool homeIn, ref float ____requiredPower, ref float ____currentHealth)
    {
        if (____requiredPower != 0)
        {
            ____requiredPower = requiredPower;
        }
        if (baseDamage > 0 && power >= ____requiredPower)
        {
            ____currentHealth -= baseDamage;
        }
        return true;
    }

    [HarmonyPatch(typeof(Wish.Tree), "Hit")]
    [HarmonyPrefix]
    public static bool HarmonyPatch_Tree_Hit_Postfix(float damage, Vector3 position, bool crit, bool hitFromLocalPlayer, ref float ____currentHealth)
    {
        if (baseDamage > 0)
        {
            ____currentHealth -= baseDamage;
        }
        return true;
    }
}