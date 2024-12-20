using BepInEx;
using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;
using Wish;
using TMPro;
using System.Reflection;
using UnityEngine.Events;
using BepInEx.Logging;

public static class PluginInfo {

    public const string TITLE = "UnlimitedSkillPoints";
    public const string NAME = "unlimited_skill_points";
    public const string SHORT_DESCRIPTION = "Regretting that \"Pen Pal\" thing?  Or decided crossbow is better than sword?  How about a skill reset!  This mod adds a \"Reset\" text button to each skill tab to enable instant reset of skills for free and as often as you need.";

    public const string VERSION = "0.0.1";

    public const string AUTHOR = "devopsdinosaur_and_werrireloaded";
    public const string GAME_TITLE = "Sun Haven";
    public const string GAME = "sunhaven";
    public const string GUID = AUTHOR + "." + GAME + "." + NAME;
    public const string REPO = "sunhaven-mods";

    public static Dictionary<string, string> to_dict() {
        Dictionary<string, string> info = new Dictionary<string, string>();
        foreach (FieldInfo field in typeof(PluginInfo).GetFields((BindingFlags) 0xFFFFFFF)) {
            info[field.Name.ToLower()] = (string) field.GetValue(null);
        }
        return info;
    }
}

[BepInPlugin(PluginInfo.GUID, PluginInfo.TITLE, PluginInfo.VERSION)]
public class SelfPortraitPlugin: BaseUnityPlugin
{
    private Harmony m_harmony = new Harmony(PluginInfo.GUID);
	private static Dictionary<ProfessionType, GameObject> m_reset_buttons = new Dictionary<ProfessionType, GameObject>();
    public static ManualLogSource logger;
    private static Dictionary<string, int> professionStats = new Dictionary<string, int>();

    private static void initProfessionStats()
    {
        professionStats = new Dictionary<string, int>();

        professionStats.Add("Exploration1a", 3);
        professionStats.Add("Exploration1b", 3);
        professionStats.Add("Exploration1c", 3);
        professionStats.Add("Exploration1d", 2);


        professionStats.Add("Exploration2a", 3);
        professionStats.Add("Exploration2b", 3);
        professionStats.Add("Exploration2c", 3);
        professionStats.Add("Exploration2d", 3);


        professionStats.Add("Exploration3a", 3);
        professionStats.Add("Exploration3b", 3);
        professionStats.Add("Exploration3c", 3);
        professionStats.Add("Exploration3d", 3);


        professionStats.Add("Exploration4a", 3);
        professionStats.Add("Exploration4b", 3);
        professionStats.Add("Exploration4c", 3);
        professionStats.Add("Exploration4d", 3);


        professionStats.Add("Exploration5a", 3);
        professionStats.Add("Exploration5b", 3);
        professionStats.Add("Exploration5c", 3);
        professionStats.Add("Exploration5d", 3);


        professionStats.Add("Exploration6a", 3);
        professionStats.Add("Exploration6b", 3);
        professionStats.Add("Exploration6c", 3);
        professionStats.Add("Exploration6d", 2);


        professionStats.Add("Exploration7a", 3);
        professionStats.Add("Exploration7b", 3);
        professionStats.Add("Exploration7c", 2);
        professionStats.Add("Exploration7d", 3);


        professionStats.Add("Exploration8a", 3);
        professionStats.Add("Exploration8b", 3);
        professionStats.Add("Exploration8c", 3);
        professionStats.Add("Exploration8d", 3);


        professionStats.Add("Exploration9a", 3);
        professionStats.Add("Exploration9b", 3);
        professionStats.Add("Exploration9c", 3);
        professionStats.Add("Exploration9d", 3);


        professionStats.Add("Exploration10a", 3);
        professionStats.Add("Exploration10b", 3);
        professionStats.Add("Exploration10c", 3);
        professionStats.Add("Exploration10d", 3);


        professionStats.Add("Farming1a", 3);
        professionStats.Add("Farming1b", 3);
        professionStats.Add("Farming1c", 3);
        professionStats.Add("Farming1d", 3);


        professionStats.Add("Farming2a", 3);
        professionStats.Add("Farming2b", 3);
        professionStats.Add("Farming2c", 2);
        professionStats.Add("Farming2d", 3);


        professionStats.Add("Farming3a", 3);
        professionStats.Add("Farming3b", 3);
        professionStats.Add("Farming3c", 3);
        professionStats.Add("Farming3d", 2);


        professionStats.Add("Farming4a", 3);
        professionStats.Add("Farming4b", 3);
        professionStats.Add("Farming4c", 3);
        professionStats.Add("Farming4d", 3);


        professionStats.Add("Farming5a", 3);
        professionStats.Add("Farming5b", 1);
        professionStats.Add("Farming5c", 3);
        professionStats.Add("Farming5d", 3);


        professionStats.Add("Farming6a", 3);
        professionStats.Add("Farming6b", 3);
        professionStats.Add("Farming6c", 3);
        professionStats.Add("Farming6d", 3);


        professionStats.Add("Farming7a", 3);
        professionStats.Add("Farming7b", 3);
        professionStats.Add("Farming7c", 1);
        professionStats.Add("Farming7d", 3);


        professionStats.Add("Farming8a", 3);
        professionStats.Add("Farming8b", 3);
        professionStats.Add("Farming8c", 3);
        professionStats.Add("Farming8d", 3);


        professionStats.Add("Farming9a", 3);
        professionStats.Add("Farming9b", 1);
        professionStats.Add("Farming9c", 3);
        professionStats.Add("Farming9d", 3);


        professionStats.Add("Farming10a", 3);
        professionStats.Add("Farming10b", 3);
        professionStats.Add("Farming10c", 3);
        professionStats.Add("Farming10d", 1);


        professionStats.Add("Mining1a", 3);
        professionStats.Add("Mining1b", 3);
        professionStats.Add("Mining1c", 3);
        professionStats.Add("Mining1d", 3);


        professionStats.Add("Mining2a", 3);
        professionStats.Add("Mining2b", 3);
        professionStats.Add("Mining2c", 3);
        professionStats.Add("Mining2d", 3);


        professionStats.Add("Mining3a", 3);
        professionStats.Add("Mining3b", 3);
        professionStats.Add("Mining3c", 2);
        professionStats.Add("Mining3d", 3);


        professionStats.Add("Mining4a", 3);
        professionStats.Add("Mining4b", 1);
        professionStats.Add("Mining4c", 3);
        professionStats.Add("Mining4d", 3);


        professionStats.Add("Mining5a", 3);
        professionStats.Add("Mining5b", 2);
        professionStats.Add("Mining5c", 1);
        professionStats.Add("Mining5d", 3);


        professionStats.Add("Mining6a", 3);
        professionStats.Add("Mining6b", 3);
        professionStats.Add("Mining6c", 3);
        professionStats.Add("Mining6d", 3);


        professionStats.Add("Mining7a", 3);
        professionStats.Add("Mining7b", 3);
        professionStats.Add("Mining7c", 3);
        professionStats.Add("Mining7d", 3);


        professionStats.Add("Mining8a", 3);
        professionStats.Add("Mining8b", 1);
        professionStats.Add("Mining8c", 3);
        professionStats.Add("Mining8d", 3);


        professionStats.Add("Mining9a", 3);
        professionStats.Add("Mining9b", 3);
        professionStats.Add("Mining9c", 3);
        professionStats.Add("Mining9d", 3);


        professionStats.Add("Mining10a", 3);
        professionStats.Add("Mining10b", 3);
        professionStats.Add("Mining10c", 3);
        professionStats.Add("Mining10d", 3);


        professionStats.Add("Combat1a", 3);
        professionStats.Add("Combat1b", 3);
        professionStats.Add("Combat1c", 3);
        professionStats.Add("Combat1d", 3);


        professionStats.Add("Combat2a", 3);
        professionStats.Add("Combat2b", 3);
        professionStats.Add("Combat2c", 3);
        professionStats.Add("Combat2d", 3);


        professionStats.Add("Combat3a", 3);
        professionStats.Add("Combat3b", 3);
        professionStats.Add("Combat3c", 3);
        professionStats.Add("Combat3d", 2);


        professionStats.Add("Combat4a", 3);
        professionStats.Add("Combat4b", 3);
        professionStats.Add("Combat4c", 3);
        professionStats.Add("Combat4d", 3);


        professionStats.Add("Combat5a", 3);
        professionStats.Add("Combat5b", 3);
        professionStats.Add("Combat5c", 3);
        professionStats.Add("Combat5d", 2);


        professionStats.Add("Combat6a", 3);
        professionStats.Add("Combat6b", 3);
        professionStats.Add("Combat6c", 3);
        professionStats.Add("Combat6d", 3);


        professionStats.Add("Combat7a", 3);
        professionStats.Add("Combat7b", 3);
        professionStats.Add("Combat7c", 3);
        professionStats.Add("Combat7d", 3);


        professionStats.Add("Combat8a", 3);
        professionStats.Add("Combat8b", 3);
        professionStats.Add("Combat8c", 2);
        professionStats.Add("Combat8d", 3);


        professionStats.Add("Combat9a", 3);
        professionStats.Add("Combat9b", 3);
        professionStats.Add("Combat9c", 2);
        professionStats.Add("Combat9d", 3);


        professionStats.Add("Combat10a", 3);
        professionStats.Add("Combat10b", 2);
        professionStats.Add("Combat10c", 3);
        professionStats.Add("Combat10d", 3);


        professionStats.Add("Fishing1a", 3);
        professionStats.Add("Fishing1b", 3);
        professionStats.Add("Fishing1c", 3);


        professionStats.Add("Fishing2a", 3);
        professionStats.Add("Fishing2b", 3);
        professionStats.Add("Fishing2c", 3);


        professionStats.Add("Fishing3a", 3);
        professionStats.Add("Fishing3b", 3);
        professionStats.Add("Fishing3c", 3);


        professionStats.Add("Fishing4a", 3);
        professionStats.Add("Fishing4b", 3);
        professionStats.Add("Fishing4c", 3);


        professionStats.Add("Fishing5a", 3);
        professionStats.Add("Fishing5b", 3);
        professionStats.Add("Fishing5c", 3);


        professionStats.Add("Fishing6a", 3);
        professionStats.Add("Fishing6b", 3);
        professionStats.Add("Fishing6c", 3);


        professionStats.Add("Fishing7a", 3);
        professionStats.Add("Fishing7b", 3);
        professionStats.Add("Fishing7c", 3);


        professionStats.Add("Fishing8a", 3);
        professionStats.Add("Fishing8b", 1);
        professionStats.Add("Fishing8c", 3);


        professionStats.Add("Fishing9a", 3);
        professionStats.Add("Fishing9b", 3);
        professionStats.Add("Fishing9c", 1);


        professionStats.Add("Fishing10a", 3);
        professionStats.Add("Fishing10b", 3);
        professionStats.Add("Fishing10c", 3);
    }

    private void Awake() {
        logger = this.Logger;
        try {
            this.m_harmony.PatchAll();
            logger.LogInfo($"{PluginInfo.GUID} v{PluginInfo.VERSION} loaded.");
        } catch (Exception e) {
            logger.LogError("** Awake FATAL - " + e);
        }
        initProfessionStats();
    }

    public static void reset_profession(Skills skills, ProfessionType profession_type) {
		try {
			Profession profession = GameSave.Instance.CurrentSave.characterData.Professions[profession_type];
			string profession_string = profession_type.ToString();
			string column_string;
			string key;

			for (int column = 1; column <= 10; column++) {
				column_string = column.ToString();
				foreach (char letter in "abcd") {
					if (profession_type == ProfessionType.Fishing && letter == 'd') {
						continue;
					}
					key = profession_string + column_string + letter;
                    logger.LogInfo("** reset_profession INFO - " + key);
                    int statsValue = 4;
                    if (professionStats.ContainsKey(key))
                    {
                        if (!professionStats.TryGetValue(key, out statsValue))
                            statsValue = 0;
                    }
                    profession.nodes[key.GetStableHashCode()] = statsValue;
				}
            }
			Skills.skillPointsUsed[profession_type] = 0;
			skills.EnablePanelWithAvailableSkillPoint();
		} catch (Exception e) {
			logger.LogError("** reset_profession ERROR - " + e);
		}
	}

	[HarmonyPatch(typeof(Skills), "SetupProfession")]
	class HarmonyPatch_Skills_SetupProfession {

		private static void Postfix(Skills __instance, ProfessionType profession, SkillTree panel) {
			try {
				TextMeshProUGUI _skillPointsTMP = (TextMeshProUGUI) panel.
					GetType().
					GetTypeInfo().
					GetField("_skillPointsTMP", BindingFlags.Instance | BindingFlags.NonPublic).
					GetValue(panel);
				GameObject reset_button = GameObject.Instantiate<GameObject>(_skillPointsTMP.gameObject, _skillPointsTMP.transform.parent);
				TextMeshProUGUI label = reset_button.GetComponent<TextMeshProUGUI>();
				reset_button.transform.position = _skillPointsTMP.transform.position + ((Vector3.down * _skillPointsTMP.GetComponent<RectTransform>().rect.height) * 1.5f);
				label.fontSize = 12;
				label.text = "[Unlimited]";
				reset_button.AddComponent<UnityEngine.UI.Button>().onClick.AddListener((UnityAction) delegate {
					reset_profession(__instance, profession);
				});
				m_reset_buttons[profession] = reset_button;
            } catch (Exception e) {
                logger.LogError("** HarmonyPatch_Skills_SetupProfession.Postfix ERROR - " + e);
            }
        }
	}
}