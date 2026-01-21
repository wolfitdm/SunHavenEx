using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using I2.Loc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TinyJson;
using UnityEngine;
using UnityEngine.Events;
using Wish;
using static Wish.DLCShopMicroManager.ApiReturn.Asset;


namespace Polygamy;

[BepInPlugin(pluginGuid, pluginName, pluginVersion)]
public class PolygamyPlugin : BaseUnityPlugin
{
    private const string pluginGuid = "vurawnica.sunhaven.polygamy";
    private const string pluginName = "Polygamy";
    private const string pluginVersion = "0.0.4";
    private static bool ultraPolyGamySpoutsLoaded = false;
    private static string jsonUltraPolyGamySpoutsPath = Path.Combine(Paths.ConfigPath, "ultra_polygamy_spoutes.json");
    private static Dictionary<string, string> ultraPolyGamySpouts = new Dictionary<string, string>();
    private Harmony m_harmony = new Harmony(pluginGuid);
    public static ManualLogSource logger;
    private static String lastScene = null;
    private static Vector3 lastLocation;
    private static String lastTempScene = null;
    private static Vector3 lastTempLocation;
    private static string[] marriageCharacters = new string[] { "Anne", "Catherine", "Claude", "Darius", "Donovan", "Iris", "Jun", "Kai", "Karish", "Kitty", "Liam", "Lucia", "Lucius", "Lynn", "Miyeon", "Nathaniel", "Shang", "Vaan", "Vivi", "Wesley", "Wornhardt", "Xyla", "Zaria", "Elyssia", "Thorian"};

    public static void write_exception_to_console(Exception ex)
    {
        write_to_console("EXCEPTION ToString()", ex.ToString());
        write_to_console("EXCEPTION Message", ex.Message);
    }

    public static void write_to_console(string title, string message)
    {
        PolygamyPlugin.logger.LogInfo((object)$"[{pluginName} UpdatedByWerri {title}]: {message}");
    }

    private void Awake()
    {
        // Plugin startup logic
        PolygamyPlugin.logger = this.Logger;
        logger.LogInfo((object)$"Plugin {pluginName} is loaded!");
        HarmonyPatch_ScenePortalSpot_EnterHouse.Patch_ScenePortalSpot_EnterHouse(this.m_harmony);
        this.m_harmony.PatchAll();
    }

    private static void setScene()
    {
        lastScene = ScenePortalManager.ActiveSceneName;
        lastLocation = Player.Instance.transform.position;
    }

    private static void getScene()
    {
        if (lastScene == null)
        {
            return;
        }
        ScenePortalManager.Instance.ChangeScene(lastLocation, lastScene);
    }

    private static void loadCurrentScene()
    {
        setScene();
        lastTempLocation = new Vector2(361.236755f, 124.104744f);
        lastTempScene = "2playerfarm";
        UnityAction unityAction = getScene;
        ScenePortalManager.Instance.ChangeScene(lastTempLocation, lastTempScene, unityAction);
    }
    private static bool saveSpouts()
    {
        try
        {
            string json = ultraPolyGamySpouts.ToJson();
            System.IO.File.WriteAllText(jsonUltraPolyGamySpoutsPath, json);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return false;
        }
    }

    private static bool loadSpouts()
    {
        if (!File.Exists(jsonUltraPolyGamySpoutsPath))
        {
            saveSpouts();
            return true;
        }

        Dictionary<string, string> oldUltraPolyGamySpouts = ultraPolyGamySpouts.ToDictionary(entry => entry.Key,
                                                                       entry => entry.Value);
        Dictionary<string, string> newUltraPolyGamySpouts = new Dictionary<string, string>();
        try
        {
            string fileJson = System.IO.File.ReadAllText(jsonUltraPolyGamySpoutsPath);
            newUltraPolyGamySpouts = fileJson.FromJson<Dictionary<string, string>>();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        try
        {
            foreach (var k in newUltraPolyGamySpouts)
            {
                if (oldUltraPolyGamySpouts.ContainsKey(k.Key))
                {
                    oldUltraPolyGamySpouts.Remove(k.Key);
                }
                oldUltraPolyGamySpouts.Add(k.Key, k.Value);
            }
            foreach (var k in oldUltraPolyGamySpouts)
            {
                if (ultraPolyGamySpouts.ContainsKey(k.Key))
                {
                    ultraPolyGamySpouts.Remove(k.Key);
                }
                ultraPolyGamySpouts.Add(k.Key, k.Value);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        return true;
    }

    [HarmonyPatch(typeof(NPCAI), "HandleLoveLetter")]
    class HarmonyPatch_NPCAI_HandleLoveLetter

    {
        private static ConfigFile polyGamyModMarryEveryoneAgainConfigLL = new ConfigFile(Path.Combine(Paths.ConfigPath, "vurawnica.updatedby.werri.sunhaven.polygamy.cfg"), true);
        private static ConfigEntry<bool> polyGamyModMarryEveryoneAgainLL = polyGamyModMarryEveryoneAgainConfigLL.Bind("General.Toggles", "polyGamyModMarryEveryoneAgain", true, "Whether or not to marriage everyone again?");

        private static bool NPCAI_CycleUnlocked(NPCAI npc)
        {
            string progressID;
            switch (npc.OriginalName)
            {
                case "Catherine":
                    progressID = "Catherine Cycle 9";
                    break;
                case "Claude":
                    progressID = "Claude Cycle 11";
                    break;
                case "Darius":
                    progressID = "Darius Cycle 11";
                    break;
                case "Donovan":
                    progressID = "Donovan Cycle 8";
                    break;
                case "Iris":
                    progressID = "Iris Cycle 11";
                    break;
                case "Jun":
                    progressID = "Jun Cycle 7";
                    break;
                case "Kai":
                    progressID = "Kai Cycle 5";
                    break;
                case "Karish":
                    progressID = "Karish Cycle 5";
                    break;
                case "Kitty":
                    progressID = "Kitty Cycle 8";
                    break;
                case "Liam":
                    progressID = "Liam Cycle 10";
                    break;
                case "Lucia":
                    progressID = "Lucia Cycle 9";
                    break;
                case "Lucius":
                    progressID = "Lucius Cycle 5";
                    break;
                case "Lynn":
                    progressID = "Lynn Cycle 9";
                    break;
                case "Miyeon":
                    progressID = "Miyeon Cycle 5";
                    break;
                case "Nathaniel":
                    progressID = "Nathaniel Cycle 9";
                    break;
                case "Shang":
                    progressID = "Shang Cycle 5";
                    break;
                case "Vaan":
                    progressID = "Vaan Cycle 8";
                    break;
                case "Vivi":
                    progressID = "Vivi Cycle 5";
                    break;
                case "Wesley":
                    progressID = "Wesley Cycle 5";
                    break;
                case "Wornhardt":
                    progressID = "Wornhardt Cycle 10";
                    break;
                case "Xyla":
                    progressID = "Xyla Cycle 11";
                    break;
                case "Zaria":
                    progressID = "Zaria Cycle 5";
                    break;
                case "Elyssia":
                    progressID = "Elyssia Cycle 5";
                    break;
                case "Thorian":
                    progressID = "Thorian Cycle 5";
                    break;
                default:
                    progressID = "Anne Cycle 10";
                    break;
            }
            return SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter(progressID);
        }

        private static bool NPCAI_UnlockedEx(NPCAI npcai)
        {
            MethodInfo instanceMethod = typeof(NPCAI).GetMethod(
                "CycleUnlocked",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            if (instanceMethod == null)
                return false;

            object result1 = instanceMethod.Invoke(npcai, null);
            if (result1 == null) return false;
            if (result1 is bool) return (bool) result1;
            return false;
        }
        private static bool Prefix(string __result, out bool response, NPCAI __instance, ref string ____npcName)
        {
            response = false;
            bool flag = false;
            string str = "";
            if (__instance != null && ____npcName == __instance.OriginalName)
            {
                PolygamyPlugin.logger.LogInfo("[PolyGamy Mod v0.0.4 UpdatedByWerri]: You can use __instance.OriginalName!");
            }
            if (__instance.OriginalName.Equals("Wesley") && !SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("UnlockedWesley"))
            {
                string loveLetterReject;
                __result = loveLetterReject = ScriptLocalization.RNPC_Welsey_LoveLetter_Reject;
            }
            if (__instance.IsMarriedToPlayer() || __instance.IsDatingPlayer())
            {
                flag = true;
                str = ScriptLocalization.RNPC_Generic_LoveLetter_Reject_01;
            }
            else
            {
                float num;
                if (!SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships.TryGetValue(__instance.OriginalName, out num) || (double)num < 25.0 || !NPCAI_CycleUnlocked(__instance))
                {
                    flag = true;
                    str = ScriptLocalization.RNPC_Generic_LoveLetter_Reject_00;
                }
            }
            if (flag)
            {
                Player.Instance.Inventory.AddItem(502);
                __result = str + "[]" + ScriptLocalization.RNPC_Generic_LoveLetter_Reject_02;
            }
            response = true;
            __instance.DatePlayer();
            __result = ScriptLocalization.RNPC_Generic_LoveLetter_Accept;

            return response;
        }
    }

    [HarmonyPatch(typeof(NPCAI), "HandleWeddingRing")]
    class HarmonyPatch_NPCAI_HandleWeddingRing
    {
        private static ConfigFile polyGamyModMarryEveryoneAgainConfig = new ConfigFile(Path.Combine(Paths.ConfigPath, "vurawnica.updatedby.werri.sunhaven.polygamy.cfg"), true);
		private static ConfigEntry<bool> polyGamyModMarryEveryoneAgain = polyGamyModMarryEveryoneAgainConfig.Bind("General.Toggles", "polyGamyModMarryEveryoneAgain", true, "Whether or not to marriage everyone again?");
        private static bool Prefix(string __result, out bool response, NPCAI __instance, ref string ____npcName)
        {
            response = false;
            bool flag = false;
            string str = "";
            float value;
            if (__instance != null && ____npcName == __instance.OriginalName)
            {
                PolygamyPlugin.logger.LogInfo("[PolyGamy Mod v0.0.4 UpdatedByWerri]: You can use __instance.OriginalName!");
            }
            if (__instance.IsMarriedToPlayer())
            {
                flag = true;
                switch (__instance.OriginalName)
                {
                    case "Anne":
                        str = ScriptLocalization.RNPC_Anne_DeclineProposal_01;
                        break;
                    case "Catherine":
                        str = "";
                        break;
                    case "Claude":
                        str = "";
                        break;
                    case "Darius":
                        str = "";
                        break;
                    case "Donovan":
                        str = "";
                        break;
                    case "Iris":
                        str = "";
                        break;
                    case "Jun":
                        str = "";
                        break;
                    case "Kai":
                        str = ScriptLocalization.RNPC_Kai_DeclineProposal_01;
                        break;
                    case "Karish":
                        str = ScriptLocalization.RNPC_Karish_DeclineProposal_01;
                        break;
                    case "Kitty":
                        str = "";
                        break;
                    case "Liam":
                        str = "";
                        break;
                    case "Lucia":
                        str = "";
                        break;
                    case "Lucius":
                        str = ScriptLocalization.RNPC_Lucius_DeclineProposal_01;
                        break;
                    case "Lynn":
                        str = ScriptLocalization.RNPC_Lynn_DeclineProposal_01;
                        break;
                    case "Miyeon":
                        str = ScriptLocalization.RNPC_Miyeon_DeclineProposal_01;
                        break;
                    case "Nathaniel":
                        str = "";
                        break;
                    case "Shang":
                        str = ScriptLocalization.RNPC_Shang_DeclineProposal_01;
                        break;
                    case "Vaan":
                        str = "";
                        break;
                    case "Vivi":
                        str = ScriptLocalization.RNPC_Vivi_DeclineProposal_01;
                        break;
                    case "Wesley":
                        str = ScriptLocalization.RNPC_Wesley_DeclineProposal_01;
                        break;
                    case "Wornhardt":
                        str = "";
                        break;
                    case "Xyla":
                        str = "";
                        break;
                    case "Zaria":
                        str = ScriptLocalization.RNPC_Zaria_DeclineProposal_01;
                        break;
                    case "Elyssia":
                        str = ScriptLocalization.RNPC_Elyssia_DeclineProposal_01;
                        break;
                    case "Thorian":
                        str = ScriptLocalization.RNPC_Thorian_DeclineProposal_01;
                        break;
                    default:
                        str = "";
                        break;
                }
            }
            else if (SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("Married"))
            {
                flag = true;
                switch (__instance.OriginalName)
                {
                    case "Anne":
                        str = ScriptLocalization.RNPC_Anne_DeclineProposal_02;
                        break;
                    case "Catherine":
                        str = "";
                        break;
                    case "Claude":
                        str = "";
                        break;
                    case "Darius":
                        str = "";
                        break;
                    case "Donovan":
                        str = "";
                        break;
                    case "Iris":
                        str = "";
                        break;
                    case "Jun":
                        str = "";
                        break;
                    case "Kai":
                        str = ScriptLocalization.RNPC_Kai_DeclineProposal_02;
                        break;
                    case "Karish":
                        str = ScriptLocalization.RNPC_Karish_DeclineProposal_02;
                        break;
                    case "Kitty":
                        str = "";
                        break;
                    case "Liam":
                        str = "";
                        break;
                    case "Lucia":
                        str = "";
                        break;
                    case "Lucius":
                        str = ScriptLocalization.RNPC_Lucius_DeclineProposal_02;
                        break;
                    case "Lynn":
                        str = ScriptLocalization.RNPC_Lynn_DeclineProposal_02;
                        break;
                    case "Miyeon":
                        str = ScriptLocalization.RNPC_Miyeon_DeclineProposal_02;
                        break;
                    case "Nathaniel":
                        str = "";
                        break;
                    case "Shang":
                        str = ScriptLocalization.RNPC_Shang_DeclineProposal_02;
                        break;
                    case "Vaan":
                        str = "";
                        break;
                    case "Vivi":
                        str = ScriptLocalization.RNPC_Vivi_DeclineProposal_02;
                        break;
                    case "Wesley":
                        str = ScriptLocalization.RNPC_Wesley_DeclineProposal_02;
                        break;
                    case "Wornhardt":
                        str = "";
                        break;
                    case "Xyla":
                        str = "";
                        break;
                    case "Zaria":
                        str = ScriptLocalization.RNPC_Zaria_DeclineProposal_02;
                        break;
                    case "Elyssia":
                        str = ScriptLocalization.RNPC_Elyssia_DeclineProposal_02;
                        break;
                    case "Thorian":
                        str = ScriptLocalization.RNPC_Thorian_DeclineProposal_02;
                        break;
                    default:
                        str = "";
                        break;
                }
            }
            else if (!__instance.IsDatingPlayer() || !SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter(____npcName + " Cycle 14") || !SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships.TryGetValue(____npcName, out value) || (double)value < 75.0)
            {
                    flag = true;
                    switch (__instance.OriginalName)
                    {
                        case "Anne":
                            str = ScriptLocalization.RNPC_Anne_DeclineProposal_00;
                            break;
                        case "Catherine":
                            str = "";
                            break;
                        case "Claude":
                            str = "";
                            break;
                        case "Darius":
                            str = "";
                            break;
                        case "Donovan":
                            str = "";
                            break;
                        case "Iris":
                            str = "";
                            break;
                        case "Jun":
                            str = "";
                            break;
                        case "Kai":
                            str = ScriptLocalization.RNPC_Kai_DeclineProposal_00;
                            break;
                        case "Karish":
                            str = ScriptLocalization.RNPC_Karish_DeclineProposal_00;
                            break;
                        case "Kitty":
                            str = "";
                            break;
                        case "Liam":
                            str = "";
                            break;
                        case "Lucia":
                            str = "";
                            break;
                        case "Lucius":
                            str = ScriptLocalization.RNPC_Lucius_DeclineProposal_00;
                            break;
                        case "Lynn":
                            str = ScriptLocalization.RNPC_Lynn_DeclineProposal_00;
                            break;
                        case "Miyeon":
                            str = ScriptLocalization.RNPC_Miyeon_DeclineProposal_00;
                            break;
                        case "Nathaniel":
                            str = "";
                            break;
                        case "Shang":
                            str = ScriptLocalization.RNPC_Shang_DeclineProposal_00;
                            break;
                        case "Vaan":
                            str = "";
                            break;
                        case "Vivi":
                            str = ScriptLocalization.RNPC_Vivi_DeclineProposal_00;
                            break;
                        case "Wesley":
                            str = ScriptLocalization.RNPC_Wesley_DeclineProposal_00;
                            break;
                        case "Wornhardt":
                            str = "";
                            break;
                        case "Xyla":
                            str = "";
                            break;
                        case "Zaria":
                            str = ScriptLocalization.RNPC_Zaria_DeclineProposal_00;
                            break;
                        case "Elyssia":
                            str = ScriptLocalization.RNPC_Elyssia_DeclineProposal_00;
                            break;
                        case "Thorian":
                            str = ScriptLocalization.RNPC_Thorian_DeclineProposal_00;
                            break;
                        default:
                            str = "";
                            break;
                    }
            }
            // PolyGamy Mod Code Start
            foreach (string quest in Player.Instance.QuestList.questLog.Keys.ToList<string>())
            {
                if (quest.Contains("MarriageQuest"))
                {
                    Player.Instance.QuestList.AbandonQuest(quest);
                }
            }
            // PolyGamy Mod Code End
            if (flag)
            {
                Player.Instance.Inventory.AddItem(6107);
                __result = str + "[]" + ScriptLocalization.RNPC_Generic_DeclineProposal;
                PolygamyPlugin.logger.LogInfo("[PolyGamy Mod v0.0.4 UpdatedByWerri]: Normally return is here, but now we want to override this!");
            }
            // PolyGamy Mod Code Start
            //if (SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("Married"))
            //{
            //    Player.Instance.Inventory.RemoveItem(6107, 1);
            //}
            // PolyGamy Mod Code End
            response = true;
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            switch (__instance.OriginalName)
            {
                case "Anne":
                    Player.Instance.QuestList.StartQuest("AnneMarriageQuest");
                    __result = ScriptLocalization.RNPC_Anne_AcceptProposal;
                    break;
                case "Catherine":
                    Player.Instance.QuestList.StartQuest("CatherineMarriageQuest");
                    __result = ScriptLocalization.RNPC_Catherine_AcceptProposal;
                    break;
                case "Claude":
                    Player.Instance.QuestList.StartQuest("ClaudeMarriageQuest");
                    __result = ScriptLocalization.RNPC_Claude_AcceptProposal;
                    break;
                case "Darius":
                    Player.Instance.QuestList.StartQuest("DariusMarriageQuest");
                    __result = ScriptLocalization.RNPC_Darius_AcceptProposal;
                    break;
                case "Donovan":
                    Player.Instance.QuestList.StartQuest("DonovanMarriageQuest");
                    __result = ScriptLocalization.RNPC_Donovan_AcceptProposal;
                    break;
                case "Iris":
                    Player.Instance.QuestList.StartQuest("IrisMarriageQuest");
                    __result = ScriptLocalization.RNPC_Iris_AcceptProposal;
                    break;
                case "Jun":
                    Player.Instance.QuestList.StartQuest("JunMarriageQuest");
                    __result = ScriptLocalization.RNPC_Jun_AcceptProposal;
                    break;
                case "Kai":
                    Player.Instance.QuestList.StartQuest("KaiMarriageQuest");
                    __result = ScriptLocalization.RNPC_Kai_AcceptProposal;
                    break;
                case "Karish":
                    Player.Instance.QuestList.StartQuest("KarishMarriageQuest");
                    __result = ScriptLocalization.RNPC_Karish_AcceptProposal;
                    break;
                case "Kitty":
                    Player.Instance.QuestList.StartQuest("KittyMarriageQuest");
                    __result = ScriptLocalization.RNPC_Kitty_AcceptProposal;
                    break;
                case "Liam":
                    Player.Instance.QuestList.StartQuest("LiamMarriageQuest");
                    __result = ScriptLocalization.RNPC_Liam_AcceptProposal;
                    break;
                case "Lucia":
                    Player.Instance.QuestList.StartQuest("LuciaMarriageQuest");
                    __result = ScriptLocalization.RNPC_Lucia_AcceptProposal;
                    break;
                case "Lucius":
                    Player.Instance.QuestList.StartQuest("LuciusMarriageQuest");
                    __result = ScriptLocalization.RNPC_Lucius_AcceptProposal;
                    break;
                case "Lynn":
                    Player.Instance.QuestList.StartQuest("LynnMarriageQuest");
                    __result = ScriptLocalization.RNPC_Lynn_AcceptProposal;
                    break;
                case "Miyeon":
                    Player.Instance.QuestList.StartQuest("MiyeonMarriageQuest");
                    __result = ScriptLocalization.RNPC_Miyeon_AcceptProposal;
                    break;
                case "Nathaniel":
                    Player.Instance.QuestList.StartQuest("NathanielMarriageQuest");
                    __result = ScriptLocalization.RNPC_Nathaniel_AcceptProposal;
                    break;
                case "Shang":
                    Player.Instance.QuestList.StartQuest("ShangMarriageQuest");
                    __result = ScriptLocalization.RNPC_Shang_AcceptProposal;
                    break;
                case "Vaan":
                    Player.Instance.QuestList.StartQuest("VaanMarriageQuest");
                    __result = ScriptLocalization.RNPC_Vaan_AcceptProposal;
                    break;
                case "Vivi":
                    Player.Instance.QuestList.StartQuest("ViviMarriageQuest");
                    __result = ScriptLocalization.RNPC_Vivi_AcceptProposal;
                    break;
                case "Wesley":
                    Player.Instance.QuestList.StartQuest("WesleyMarriageQuest");
                    __result = ScriptLocalization.RNPC_Wesley_AcceptProposal;
                    break;
                case "Wornhardt":
                    Player.Instance.QuestList.StartQuest("WornhardtMarriageQuest");
                    __result = ScriptLocalization.RNPC_Wornhardt_AcceptProposal;
                    break;
                case "Xyla":
                    Player.Instance.QuestList.StartQuest("XylaMarriageQuest");
                    __result = ScriptLocalization.RNPC_Xyla_AcceptProposal;
                    break;
                case "Zaria":
                    Player.Instance.QuestList.StartQuest("ZariaMarriageQuest");
                    __result = ScriptLocalization.RNPC_Zaria_AcceptProposal;
                    break;
                case "Elyssia":
                    Player.Instance.QuestList.StartQuest("ElyssiaMarriageQuest");
                    __result = ScriptLocalization.RNPC_Elyssia_AcceptProposal;
                    break;
                case "Thorian":
                    Player.Instance.QuestList.StartQuest("ThorianMarriageQuest");
                    __result = ScriptLocalization.RNPC_Thorian_AcceptProposal;
                    break;
                default:
                    Player.Instance.QuestList.StartQuest(__instance.OriginalName+"MarriageQuest");
                    __result = ScriptLocalization.RNPC_Generic_AcceptProposal;
                    break;
            }
            PolygamyPlugin.logger.LogInfo("[PolyGamy Mod v0.0.4 UpdatedByWerri]: Override Return, Don't decline marriage!!");
            // PolyGamy Mod Code Start
            return false;
            // PolyGamy Mod Code End
        }

        private static string Postfix(string __result, out bool response, NPCAI __instance, ref string ____npcName)
	    {
            if (__instance != null && ____npcName == __instance.OriginalName)
            {
                PolygamyPlugin.logger.LogInfo("[PolyGamy Mod v0.0.4 UpdatedByWerri]: You can use __instance.OriginalName!");
            }
            response = false;
			string resultParam = __result;
			bool responseParam = response;
			NPCAI instanceParam = __instance;
            string npcNameParam = ____npcName;
            string ret = "";
            bool _configMarryEveryoneAgain = polyGamyModMarryEveryoneAgain.Value;
			if (_configMarryEveryoneAgain) {
				PolygamyPlugin.logger.LogInfo("[PolygamyMod v0.0.4] call postFixMarryOneAgain function!");
				ret = _Postfix_MarryEveryoneAgain(resultParam, out responseParam, instanceParam, ref npcNameParam);
			} else {
				PolygamyPlugin.logger.LogInfo("[PolygamyMod v0.0.4] call postFixOriginal function!");
				ret = _Postfix_Original(resultParam, out responseParam, instanceParam, ref npcNameParam);
			}
            __result = resultParam;
            response = responseParam;
			__instance = instanceParam;
			____npcName = npcNameParam;
			return ret;
		}
   
        private static string _Postfix_Original(string __result, out bool response, NPCAI __instance, ref string ____npcName)
        {
            response = false;
            bool flag = false;
            string text = "";
            float value;
            if (SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("MarriedTo" + __instance.OriginalName))
            {
                flag = true;
                switch (__instance.OriginalName)
                {
                    case "Lynn":
                        text = "Hehe, you want to get married again?? That's cute sweetie, but I don't think it works that way.";
                        break;
                    case "Anne":
                        text = "I don't know if this is a little joke or not, but the gesture is really cute. And of course, I'd be happy to take a second ring!";
                        break;
                    case "Donovan":
                        text = "";
                        break;
                    case "Vaan":
                        text = "";
                        break;
                    case "Xyla":
                        text = "";
                        break;
                    case "Nathaniel":
                        text = "";
                        break;
                    case "Jun":
                        text = "";
                        break;
                    case "Liam":
                        text = "";
                        break;
                    case "Catherine":
                        text = "";
                        break;
                    case "Claude":
                        text = "";
                        break;
                    case "Kitty":
                        text = "";
                        break;
                    case "Wornhardt":
                        text = "";
                        break;
                    case "Darius":
                        text = "";
                        break;
                    case "Lucia":
                        text = "";
                        break;
                    case "Iris":
                        text = "";
                        break;
                    case "Miyeon":
                        text = "";
                        break;
                    case "Kai":
                        text = "";
                        break;
                    case "Shang":
                        text = "";
                        break;
                    case "Wesley":
                        text = "";
                        break;
                    case "Vivi":
                        text = "";
                        break;
                    case "Lucius":
                        text = "";
                        break;
                    case "Elyssia":
                        text = "";
                        break;
                    case "Thorian":
                        text = "";
                        break;
                    default:
                        text = "";
                        break;
                }
            }
            else if
                (
                !SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("Dating" + __instance.OriginalName) ||
                !SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter(__instance.OriginalName + " Cycle 14") ||
                !SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships.TryGetValue(__instance.OriginalName, out value) ||
                value < 75f
                )
            {
                flag = true;
                switch (__instance.OriginalName)
                {
                    case "Lynn":
                        text = "Oh - wow! I'm so sorry XX, but I don't think I'm quite ready for that. We should get to know each other better first, right?";
                        break;
                    case "Anne":
                        text = "Who do I look like to you? I'm a merchant, not a farm wife. I love the ring, but it's going to take more effort on your part before I say yes.";
                        break;
                    case "Donovan":
                        text = "";
                        break;
                    case "Vaan":
                        text = "";
                        break;
                    case "Xyla":
                        text = "";
                        break;
                    case "Nathaniel":
                        text = "";
                        break;
                    case "Jun":
                        text = "";
                        break;
                    case "Liam":
                        text = "";
                        break;
                    case "Catherine":
                        text = "";
                        break;
                    case "Claude":
                        text = "";
                        break;
                    case "Kitty":
                        text = "";
                        break;
                    case "Wornhardt":
                        text = "";
                        break;
                    case "Darius":
                        text = "";
                        break;
                    case "Lucia":
                        text = "";
                        break;
                    case "Iris":
                        text = "";
                        break;
                    case "Miyeon":
                        text = "";
                        break;
                    case "Kai":
                        text = "";
                        break;
                    case "Shang":
                        text = "";
                        break;
                    case "Wesley":
                        text = "";
                        break;
                    case "Vivi":
                        text = "";
                        break;
                    case "Lucius":
                        text = "";
                        break;
                    case "Elyssia":
                        text = "";
                        break;
                    case "Thorian":
                        text = "";
                        break;
                    default:
                        text = "";
                        break;
                }
            }
            if (flag)
            {
                return text + "[]<i>(You must be dating and not already married to this character, achieved 15 full hearts, and progressed far enough in the dialogue to marry them)</i>";
            }
            if (SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("Married"))
            {
                Player.Instance.Inventory.RemoveItem(6107, 1);
            }
            response = true;
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", value: true);
            switch (____npcName)
            {
                case "Lynn":
                    Player.Instance.QuestList.StartQuest("LynnMarriageQuest");
                    return "Oh! Heh, I really shouldn't be surprised. Actually, what's really surprising is... I don't think it's a bad idea. Sure, let's do it, XX![]We should do it at 4pm tomorrow at the event center! I'll take care of everything else, you just show up!";
                case "Anne":
                    Player.Instance.QuestList.StartQuest("AnneMarriageQuest");
                    return "Heh, it's about time my investment paid off. What do you mean \"what investment?\" I'm talking about <i>you</i>, XX![]Of course I'll marry you! I'll pay for someone to arrange the ceremony, you just meet me at the event square tomorrow at 4:00 pm.";
                case "Donovan":
                    Player.Instance.QuestList.StartQuest("DonovanMarriageQuest");
                    return "Ah, I figured this was coming... Well I won't hold you in suspense.[]Let's do it, XX - let's get married!![]I'll have someone set up a nice little ceremony in your Human town. Just show up at 4:00 pm and I'll take care of the rest!";
                case "Vaan":
                    Player.Instance.QuestList.StartQuest("VaanMarriageQuest");
                    return "Finally! Yes of course we should be married, XX! It makes all the sense in the world.[]We should do it in Sun Haven. I'll see about contacting your Archmage to set up the ceremony. You just make sure you're on time! Let's call it 4:00 pm. I'll see you tomorrow!";
                case "Xyla":
                    Player.Instance.QuestList.StartQuest("XylaMarriageQuest");
                    return "Oh! Heh, I really shouldn't be surprised. Actually, what's really surprising is... I don't think it's a bad idea. Sure, let's do it, XX![]I'll even set it up for you in your beloved Human town. Let's kick it off at 4:00 pm. Don't be late, sewer rat!";
                case "Nathaniel":
                    Player.Instance.QuestList.StartQuest("NathanielMarriageQuest");
                    return "Ah! Heh, it's about time![]Yes XX, I will absolutely marry you! Let me handle the ceremony, you just get yourself to the event square tomorrow at 4:00 pm. I can't wait!";
                case "Jun":
                    Player.Instance.QuestList.StartQuest("JunMarriageQuest");
                    return "XX! I've pictured this moment so many times... Yes, I will marry you![]Let's have the ceremony tomorrow at 4:00 pm. I'll get the event center all set up, you just need to be there. I'll see you then!";
                case "Liam":
                    Player.Instance.QuestList.StartQuest("LiamMarriageQuest");
                    return "Ah, wow, this is a lot.[]You know... I think I'm ready for this! Yes XX, let's get married!! I'll see about setting up a ceremony. You just meet me tomorrow at the event square. Let's say 4:00 pm!";
                case "Catherine":
                    Player.Instance.QuestList.StartQuest("CatherineMarriageQuest");
                    return "I've been dreaming of the moment! I thought I'd be prepared for it, but now I feel like I'm floating.[]Of course I'll marry you, XX!! I'll prepare the ceremony, you just meet me at the event square tomorrow at 4:00 pm.";
                case "Claude":
                    Player.Instance.QuestList.StartQuest("ClaudeMarriageQuest");
                    return "Marriage? With me? I never thought... Well, I guess it doesn't matter what I thought now.[]Yes, of course I will marry you, XX. I'll pay for somebody to set up the ceremony in the event square. Be there tomorrow at 4:00 pm. I really can't wait!";
                case "Kitty":
                    Player.Instance.QuestList.StartQuest("KittyMarriageQuest");
                    return "OH - oh my goodness gracious! XX, I will marry you! Kitty will marry XX, nya nya![]Don't worry, Kitty will get the ceremony set up. You just show up to the event square at 4:00 pm tomorrow! I can't wait!";
                case "Wornhardt":
                    Player.Instance.QuestList.StartQuest("WornhardtMarriageQuest");
                    return "... Really, do you mean this?[]XX, marrying you would make me the happiest man in town! Yes, let's do it! I'll handle the preparations, you just get yourself to the event square by 4:00 pm tomorrow.";
                case "Darius":
                    Player.Instance.QuestList.StartQuest("DariusMarriageQuest");
                    return "It's about time you asked, XX. I was growing impatient, but now you can take your proper place by the side of Withergate's future king.[]That is to say, I accept your proposal! I'll have some lackeys set up a ceremony tomorrow in your Human town. Be there at 4:00 pm, and don't keep me waiting.";
                case "Lucia":
                    Player.Instance.QuestList.StartQuest("LuciaMarriageQuest");
                    return "Oh my goodness!! XX!! Yes, yes <i>of course</i> I'll marry you![]I'll prepare the ceremony for us tomorrow. Try to be at the event square by 4:00 pm! I'm so excited, XX!";
                case "Iris":
                    Player.Instance.QuestList.StartQuest("IrisMarriageQuest");
                    return "You're proposing?? Oh - I'm sorry, I was just so unprepared for this.[]...The answer is yes, obviously! I know you'll want it in Sun Haven, so I'll talk to your Archmage to set it up. Just be there tomorrow at 4:00 pm. Ah, this is so exciting!";
                case "Miyeon":
                    Player.Instance.QuestList.StartQuest("MiyeonMarriageQuest");
                    return "Oh! Heh, I really shouldn't be surprised. Actually, what's really surprising is... I don't think it's a bad idea. Sure, let's do it, XX![]We should do it at 4pm tomorrow at the event center! I'll take care of everything else, you just show up!";
                case "Kai":
                    Player.Instance.QuestList.StartQuest("KaiMarriageQuest");
                    return "Oh! Heh, I really shouldn't be surprised. Actually, what's really surprising is... I don't think it's a bad idea. Sure, let's do it, XX![]We should do it at 4pm tomorrow at the event center! I'll take care of everything else, you just show up!";
                case "Shang":
                    Player.Instance.QuestList.StartQuest("ShangMarriageQuest");
                    return "Oh! Heh, I really shouldn't be surprised. Actually, what's really surprising is... I don't think it's a bad idea. Sure, let's do it, XX![]We should do it at 4pm tomorrow at the event center! I'll take care of everything else, you just show up!";
                case "Wesley":
                    Player.Instance.QuestList.StartQuest("WesleyMarriageQuest");
                    return "Oh! Heh, I really shouldn't be surprised. Actually, what's really surprising is... I don't think it's a bad idea. Sure, let's do it, XX![]We should do it at 4pm tomorrow at the event center! I'll take care of everything else, you just show up!";
                case "Vivi":
                    Player.Instance.QuestList.StartQuest("ViviMarriageQuest");
                    return "Oh! Heh, I really shouldn't be surprised. Actually, what's really surprising is... I don't think it's a bad idea. Sure, let's do it, XX![]We should do it at 4pm tomorrow at the event center! I'll take care of everything else, you just show up!";
                case "Lucius":
                    Player.Instance.QuestList.StartQuest("LuciusMarriageQuest");
                    return "Oh! Heh, I really shouldn't be surprised. Actually, what's really surprising is... I don't think it's a bad idea. Sure, let's do it, XX![]We should do it at 4pm tomorrow at the event center! I'll take care of everything else, you just show up!";
                default:
                    Player.Instance.QuestList.StartQuest(____npcName + "MarriageQuest");
                    return "Oh! Heh, I really shouldn't be surprised. Actually, what's really surprising is... I don't think it's a bad idea. Sure, let's do it, XX![]We should do it at 4pm tomorrow at the event center! I'll take care of everything else, you just show up!";
            }
        }
    }
	
	private static string _Postfix_MarryEveryoneAgain(
      string __result,
      out bool response,
      NPCAI __instance,
      ref string ____npcName)
    {
      response = false;
      bool flag = false;
      string str = "";
      if (SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("MarriedTo" + __instance.OriginalName))
      {
        flag = true;
        switch (__instance.OriginalName)
        {
          case "Anne":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("AnneMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "I don't know if this is a little joke or not, but the gesture is really cute. And of course, I'd be happy to take a second ring! [] I'll pay for someone to arrange the ceremony, you just meet me at the event square tomorrow at 4:00 pm.";
            break;
          case "Catherine":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("CatherineMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Claude":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("ClaudeMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Darius":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("DariusMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Donovan":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("DonovanMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Iris":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("IrisMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Jun":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("JunMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Kai":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("KaiMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Karish":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("KarishMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Kitty":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("KittyMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Liam":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("LiamMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Lucia":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("LuciaMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Lucius":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("LuciusMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Lynn":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("LynnMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "Hehe, you want to get married again?? That's cute sweetie. Sure, let's do it, XX![]We should do it at 4pm tomorrow at the event center! I'll take care of everything else, you just show up!";
            break;
          case "Miyeon":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("MiyeonMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Nathaniel":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("NathanielMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Shang":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("ShangMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Vaan":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("VaanMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Vivi":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("ViviMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Wesley":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("WesleyMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Wornhardt":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("WornhardtMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Xyla":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("XylaMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          case "Zaria":
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest("ZariaMarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
            break;
          default:
            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
            Player.Instance.QuestList.StartQuest(__instance.OriginalName + "MarriageQuest", false);
            Player.Instance.Inventory.RemoveItem(6107, 1, 0);
            response = true;
            str = "A vow renewal? That's a great idea! I'll take care of everything, you just show up, my dear!";
           break;
        }
        if (flag)
          return str + "[]<i>(There might be an error message here, just ignore that. You should have started the Marriage Quest again)</i>";
      }
      else
      {
        float num;
        if (!SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("Dating" + __instance.OriginalName) || !SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter(__instance.OriginalName + " Cycle 14") || !SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships.TryGetValue(__instance.OriginalName, out num) || (double) num < 75.0)
        {
          flag = true;
          switch (____npcName)
          {
            case "Anne":
              str = "Who do I look like to you? I'm a merchant, not a farm wife. I love the ring, but it's going to take more effort on your part before I say yes.";
              break;
            case "Catherine":
              str = "";
              break;
            case "Claude":
              str = "";
              break;
            case "Darius":
              str = "";
              break;
            case "Donovan":
              str = "";
              break;
            case "Iris":
              str = "";
              break;
            case "Jun":
              str = "";
              break;
            case "Kai":
              str = "";
              break;
            case "Kitty":
              str = "";
              break;
            case "Liam":
              str = "";
              break;
            case "Lucia":
              str = "";
              break;
            case "Lucius":
              str = "";
              break;
            case "Lynn":
              str = "Oh - wow! I'm so sorry XX, but I don't think I'm quite ready for that. We should get to know each other better first, right?";
              break;
            case "Miyeon":
              str = "";
              break;
            case "Nathaniel":
              str = "";
              break;
            case "Shang":
              str = "";
              break;
            case "Vaan":
              str = "";
              break;
            case "Vivi":
              str = "";
              break;
            case "Wesley":
              str = "";
              break;
            case "Wornhardt":
              str = "";
              break;
            case "Xyla":
              str = "";
              break;
            default:
              str = "";
              break;
          }
        }
      }
      if (flag)
        return str + "[]<i>(You must be dating this character, achieved 15 full hearts, and progressed far enough in the dialogue to marry them)</i>";
      if (SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("Married"))
        Player.Instance.Inventory.RemoveItem(6107, 1, 0);
      response = true;
      SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("EngagedToRNPC", true);
      switch (____npcName)
      {
        case "Anne":
          Player.Instance.QuestList.StartQuest("AnneMarriageQuest", false);
          return "Heh, it's about time my investment paid off. What do you mean \"what investment?\" I'm talking about <i>you</i>, XX![]Of course I'll marry you! I'll pay for someone to arrange the ceremony, you just meet me at the event square tomorrow at 4:00 pm.";
        case "Catherine":
          Player.Instance.QuestList.StartQuest("CatherineMarriageQuest", false);
          return "I've been dreaming of the moment! I thought I'd be prepared for it, but now I feel like I'm floating.[]Of course I'll marry you, XX!! I'll prepare the ceremony, you just meet me at the event square tomorrow at 4:00 pm.";
        case "Claude":
          Player.Instance.QuestList.StartQuest("ClaudeMarriageQuest", false);
          return "Marriage? With me? I never thought... Well, I guess it doesn't matter what I thought now.[]Yes, of course I will marry you, XX. I'll pay for somebody to set up the ceremony in the event square. Be there tomorrow at 4:00 pm. I really can't wait!";
        case "Darius":
          Player.Instance.QuestList.StartQuest("DariusMarriageQuest", false);
          return "It's about time you asked, XX. I was growing impatient, but now you can take your proper place by the side of Withergate's future king.[]That is to say, I accept your proposal! I'll have some lackeys set up a ceremony tomorrow in your Human town. Be there at 4:00 pm, and don't keep me waiting.";
        case "Donovan":
          Player.Instance.QuestList.StartQuest("DonovanMarriageQuest", false);
          return "Ah, I figured this was coming... Well I won't hold you in suspense.[]Let's do it, XX - let's get married!![]I'll have someone set up a nice little ceremony in your Human town. Just show up at 4:00 pm and I'll take care of the rest!";
        case "Iris":
          Player.Instance.QuestList.StartQuest("IrisMarriageQuest", false);
          return "You're proposing?? Oh - I'm sorry, I was just so unprepared for this.[]...The answer is yes, obviously! I know you'll want it in Sun Haven, so I'll talk to your Archmage to set it up. Just be there tomorrow at 4:00 pm. Ah, this is so exciting!";
        case "Jun":
          Player.Instance.QuestList.StartQuest("JunMarriageQuest", false);
          return "XX! I've pictured this moment so many times... Yes, I will marry you![]Let's have the ceremony tomorrow at 4:00 pm. I'll get the event center all set up, you just need to be there. I'll see you then!";
        case "Kai":
          Player.Instance.QuestList.StartQuest("KaiMarriageQuest", false);
          return "Then let us make a lifetime of memories together, XX. Ones that I can look back on and be happy with.";
        case "Karish":
          Player.Instance.QuestList.StartQuest("KarishMarriageQuest", false);
          return "Marry? Me, and you?! Well, yes! The answer is yes, XX! Of course I'll marry you! Let's do it! You asked me before I could ask you, so it's only fair we do it in your home at Sun Haven!! Don't worry, I'll take care of everything! [] Be prepared by 4:00 pm tomorrow, okay?! You can't be late!";
        case "Kitty":
          Player.Instance.QuestList.StartQuest("KittyMarriageQuest", false);
          return "OH - oh my goodness gracious! XX, I will marry you! Kitty will marry XX, nya nya![]Don't worry, Kitty will get the ceremony set up. You just show up to the event square at 4:00 pm tomorrow! I can't wait!";
        case "Liam":
          Player.Instance.QuestList.StartQuest("LiamMarriageQuest", false);
          return "Ah, wow, this is a lot.[]You know... I think I'm ready for this! Yes XX, let's get married!! I'll see about setting up a ceremony. You just meet me tomorrow at the event square. Let's say 4:00 pm!";
        case "Lucia":
          Player.Instance.QuestList.StartQuest("LuciaMarriageQuest", false);
          return "Oh my goodness!! XX!! Yes, yes <i>of course</i> I'll marry you![]I'll prepare the ceremony for us tomorrow. Try to be at the event square by 4:00 pm! I'm so excited, XX!";
        case "Lucius":
          Player.Instance.QuestList.StartQuest("LuciusMarriageQuest", false);
          return "M-marriage?! Me? I suppose that WOULD make me even happier![] But could you ever imagine me getting married? I never did![] But maybe... there's a first time for everything? Oh, now I'm absolutely shaking with excitement!";
        case "Lynn":
          Player.Instance.QuestList.StartQuest("LynnMarriageQuest", false);
          return "Oh! Heh, I really shouldn't be surprised. Actually, what's really surprising is... I don't think it's a bad idea. Sure, let's do it, XX![]We should do it at 4pm tomorrow at the event center! I'll take care of everything else, you just show up!";
        case "Miyeon":
          Player.Instance.QuestList.StartQuest("MiyeonMarriageQuest", false);
          return "What do you mean, XX? Are you talking about marriage? []...I could be ready for that with you. If you're ready, I mean. Are you ready?[] Ohh, no, don't tell me! I'm going to blush! I-is this really happening?!";
        case "Nathaniel":
          Player.Instance.QuestList.StartQuest("NathanielMarriageQuest", false);
          return "Ah! Heh, it's about time![]Yes XX, I will absolutely marry you! Let me handle the ceremony, you just get yourself to the event square tomorrow at 4:00 pm. I can't wait!";
        case "Shang":
          Player.Instance.QuestList.StartQuest("ShangMarriageQuest", false);
          return "Why shouldn't I be? I feel powerful and alive. I have you, XX. I feel whole. You are the whole, XX.";
        case "Vaan":
          Player.Instance.QuestList.StartQuest("VaanMarriageQuest", false);
          return "Finally! Yes of course we should be married, XX! It makes all the sense in the world.[]We should do it in Sun Haven. I'll see about contacting your Archmage to set up the ceremony. You just make sure you're on time! Let's call it 4:00 pm. I'll see you tomorrow!";
        case "Vivi":
          Player.Instance.QuestList.StartQuest("ViviMarriageQuest", false);
          return "Oh YEAH? Well I'M happy that you're so impressive! You'll never have more passion than a Wildborn like me!!";
        case "Wesley":
          Player.Instance.QuestList.StartQuest("WesleyMarriageQuest", false);
          return "Marriage? You? Me? I... well, yes. Of Course I'll marry you. What? Why do you look so surprised?!?![] Do you think I'm joking?! I'll even make all the arrangements! In Sun Haven, no less! Be ready tomorrow, at 4:00 pm. Make sure to not be late!";
        case "Wornhardt":
          Player.Instance.QuestList.StartQuest("WornhardtMarriageQuest", false);
          return "... Really, do you mean this?[]XX, marrying you would make me the happiest man in town! Yes, let's do it! I'll handle the preparations, you just get yourself to the event square by 4:00 pm tomorrow.";
        case "Xyla":
          Player.Instance.QuestList.StartQuest("XylaMarriageQuest", false);
          return "Oh! Heh, I really shouldn't be surprised. Actually, what's really surprising is... I don't think it's a bad idea. Sure, let's do it, XX![]I'll even set it up for you in your beloved Human town. Let's kick it off at 4:00 pm. Don't be late, sewer rat!";
        case "Zaria":
          Player.Instance.QuestList.StartQuest("ZariaMarriageQuest", false);
          return "This isn't happening. Literally, I'm going crazy. No? It's all real? Great. I don't know if saying \"yes\" is what I <i>should</i> do... But forget being worried. I'm done with it. Yes, XX. Let's do it [] I'll <i>walk</i> into your Sun Haven and take care of all the details myself. Be ready tomorrow at 4 om! Got that?";
        default:
          Player.Instance.QuestList.StartQuest(____npcName + "MarriageQuest", false);
          return "Oh! Heh, I really shouldn't be surprised. Actually, what's really surprising is... I don't think it's a bad idea. Sure, let's do it, XX![]We should do it at 4pm tomorrow at the event center! I'll take care of everything else, you just show up!";
      }
    }

    class HarmonyPatch_ScenePortalSpot_EnterHouse
    {

        private static MethodInfo reentrantMethod;
        private static MethodInfo reentrantMethodSetSpouse;
        public static List<int> actions = new List<int>();
        public static UnityAction onChangeScene = null;
        public static PlayerHouse playerHouse = null;
        public static ScenePortalSpot portalSpot = null;
        public static DecorationPositionData oldMeta = null;
        private static MethodInfo onChangeSceneMethod = null;
        private static void EnterHousePostfix(UnityAction onSceneLoad, ScenePortalSpot __instance)
        {
            Debug.Log("EnterHouse aber dally");
        }

        private static void SetSpousePostfix(PlayerHouse __instance)
        {
            return;
        }

        private static void SetMetaPostfix(DecorationPositionData decorationData, PlayerHouse __instance)
        {
            if (onChangeSceneMethod == null)
            {
                onChangeSceneMethod = __instance.GetType().GetMethod("OnChangeScene", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            if (onChangeSceneMethod != null) {
                onChangeSceneMethod.Invoke(__instance,  null);
            }
            decorationData.meta = __instance.meta;
            playerHouse = __instance;
        }

        public static void EnterHousePostFix(UnityAction onSceneLoad)
        {
            Debug.Log("Hallo was geht");
        }
        public static void Interact(int interactType, ScenePortalSpot __instance)
        {
            if (interactType != 0)
                return;
            __instance.InteractWithPortal();
        }

        public static void OnChangeScenePrefix(ScenePortalSpot __instance)
        {
            UnityAction onChangeScene = __instance.onChangeScene;
            if (onChangeScene == null && HarmonyPatch_ScenePortalSpot_EnterHouse.onChangeScene != null)
            {
                onChangeScene = HarmonyPatch_ScenePortalSpot_EnterHouse.onChangeScene;
            }
            if (onChangeScene == null)
            {
                return;
            }
            onChangeScene();
            string name = onChangeScene.Method.ReflectedType.Name;
            if (name == "PlayerHouse")
            {
                HarmonyPatch_ScenePortalSpot_EnterHouse.onChangeScene = onChangeScene;
            }
        }

       public static void BuildPostfix(PlayerHouse __instance)
        {
            Debug.Log("Hallo was geh             dddddt");
            Debug.Log("Hallo was geh             dddddt");
            Debug.Log("Hallo was geh             dddddt");
            Debug.Log("Hallo was geh             dddddt");
            Debug.Log("Hallo was geh             dddddt");
            Debug.Log("Hallo was geh             dddddt");
            Debug.Log("Hallo was geh             dddddt");
            Debug.Log("Hallo was geh             dddddt");
            Debug.Log("Hallo was geh             dddddt");
            Debug.Log("Hallo was geh             dddddt");
            Debug.Log("Hallo was geh             dddddt");
            Debug.Log("Hallo was geh             dddddt");
          /*  if (portalSpot != null)
            {
                return;
            }
            Debug.Log("Hallo was geh             dddddt");
            FieldInfo _portal = typeof(PlayerHouse).GetField("_portal", BindingFlags.NonPublic | BindingFlags.Instance);
            if (_portal == null)
            {
                Debug.Log("Fuck of postfix");
                return;
            }
            object obj = _portal.GetValue(__instance);
            if (obj.GetType() != typeof(ScenePortalSpot))
            {
                Debug.Log("Fuck of postfix 2");
                return;
            }
            ScenePortalSpot portal = (ScenePortalSpot)obj;
            if (portal == null || portal.onChangeScene == null)
            {
                Debug.Log("Fuck of postfix 3");
                return;
            }
            portalSpot = portal;
            onChangeScene = portalSpot.onChangeScene;*/
        }


        private static Boolean patched = false;

        public static void Patch_ScenePortalSpot_EnterHouse(Harmony harmony)
        {
            if (patched) return;
            var original = AccessTools.Method(typeof(ScenePortalSpot), "EnterHouse");
            var originalOnChangeScene = AccessTools.Method(typeof(PlayerHouse), "SetSpouse");
            var baseSetMeta = AccessTools.Method(typeof(PlayerHouse), "SetMeta");
            var interactScenePortalSpot = AccessTools.Method(typeof(ScenePortalSpot), "Interact");
            //var prefix = AccessTools.Method(typeof(HarmonyPatch_ScenePortalSpot_EnterHouse), "EnterHousePostfix");
            //var prefixOnChangeScene = AccessTools.Method(typeof(HarmonyPatch_ScenePortalSpot_EnterHouse), "SetSpousePostfix");
            var prefixSetMeta = AccessTools.Method(typeof(HarmonyPatch_ScenePortalSpot_EnterHouse), "SetMetaPostfix");
            //var enterHousePostfix = AccessTools.Method(typeof(HarmonyPatch_ScenePortalSpot_EnterHouse), "EnterHousePostfix");
            //harmony.Patch(original, null, new HarmonyMethod(enterHousePostfix));
           // harmony.Patch(originalOnChangeScene, null, new HarmonyMethod(prefixOnChangeScene));
           // harmony.Patch(baseSetMeta, null, new HarmonyMethod(prefixSetMeta));
            var build = AccessTools.Method(typeof(PlayerHouse), "Build");
            var postfixBuild = AccessTools.Method(typeof(HarmonyPatch_ScenePortalSpot_EnterHouse), "BuildPostfix");
            var prefixInteract = AccessTools.Method(typeof(HarmonyPatch_ScenePortalSpot_EnterHouse), "Interact");
            var scene = AccessTools.Method(typeof(ScenePortalSpot), "OnChangescene");
            var scenePrefix = AccessTools.Method(typeof(HarmonyPatch_ScenePortalSpot_EnterHouse), "OnChangeScenePrefix");
            //harmony.Patch(build, null, new HarmonyMethod(postfixBuild));
            // harmony.Patch(interactScenePortalSpot, new HarmonyMethod(prefixInteract));
            harmony.Patch(baseSetMeta,  null, new HarmonyMethod(prefixSetMeta));
            harmony.Patch(scene, new HarmonyMethod(scenePrefix));
            patched = true;
        }
    }

    [HarmonyPatch(typeof(NPCAI), "HandleMemoryLossPotion")]
    class HarmonyPatch_NPCAI_HandleMemoryLossPotion
    {
        private static bool Prefix(ref string __result, out bool response, NPCAI __instance, ref string ____npcName)
        {
            if (__instance != null && ____npcName == __instance.OriginalName)
            {
                PolygamyPlugin.logger.LogInfo("[PolyGamy Mod v0.0.4 UpdatedByWerri]: You can use __instance.OriginalName!");
            }

            response = true;

            try
            {
                List<string> spouses = new List<string>();
                int count = 0;
                foreach (var npcai in SingletonBehaviour<NPCManager>.Instance._npcs.Values.Where(npcai => SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("MarriedTo" + npcai.OriginalName)))
                {
                    //if (!marriageCharacters.Contains(npcai.OriginalName))
                    //{
                    //   continue;
                    //}
                    if (!npcai.Romanceable)
                        continue;
                    spouses.Add(npcai.OriginalName);
                    count += 1;
                }
                if (count <= 1)
                {
                    return true;
                }

                bool flag = __instance.IsMarriedToPlayer();
                __instance.PlatonicPlayer();
                string new_main;

                if (GameSave.CurrentCharacter.Relationships.ContainsKey(____npcName))
                {
                    GameSave.CurrentCharacter.Relationships[____npcName] = Mathf.Max(0f, GameSave.CurrentCharacter.Relationships[____npcName]);
                }
                string progressStringCharacter = SingletonBehaviour<GameSave>.Instance.GetProgressStringCharacter("MarriedWith");

                if (progressStringCharacter.Equals(____npcName))
                {
                    if (spouses[0] == ____npcName)
                    {
                        new_main = spouses[1];
                    }
                    else
                    {
                        new_main = spouses[0];
                    }
                    SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", new_main);
                    SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("MarriedTo" + progressStringCharacter, false);
                    SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld(____npcName + "MarriedWalkPath", false, true);
                    NPCAI realNPC = SingletonBehaviour<NPCManager>.Instance.GetRealNPC(____npcName);
                    realNPC.GeneratePath();
                    SingletonBehaviour<NPCManager>.Instance.StartNPCPath(realNPC);
                }
                __instance.GenerateCycle(false);

                if (flag)
                {
                    switch (____npcName)
                    {
                        case "Wornhardt":
                            __result = ScriptLocalization.RNPC_Wornhardt_MLP_Married;
                            return false;
                        case "Zaria":
                            __result = ScriptLocalization.RNPC_Zaria_MLP_Married;
                            return false;
                        case "Donovan":
                            __result = ScriptLocalization.RNPC_Donovan_MLP_Married;
                            return false;
                        case "Karish":
                            __result = ScriptLocalization.RNPC_Karish_MLP_Married;
                            return false;
                        case "Claude":
                            __result = ScriptLocalization.RNPC_Claude_MLP_Married;
                            return false;
                        case "Jun":
                            __result = ScriptLocalization.RNPC_Jun_MLP_Married;
                            return false;
                        case "Vivi":
                            __result = ScriptLocalization.RNPC_Vivi_MLP_Married;
                            return false;
                        case "Lynn":
                            __result = ScriptLocalization.RNPC_Lynn_MLP_Married;
                            return false;
                        case "Liam":
                            __result = ScriptLocalization.RNPC_Liam_MLP_Married;
                            return false;
                        case "Anne":
                            __result = ScriptLocalization.RNPC_Anne_MLP_Married;
                            return false;
                        case "Miyeon":
                            __result = ScriptLocalization.RNPC_Miyeon_MLP_Married;
                            return false;
                        case "Kitty":
                            __result = ScriptLocalization.RNPC_Kitty_MLP_Married;
                            return false;
                        case "Lucius":
                            __result = ScriptLocalization.RNPC_Lucius_MLP_Married;
                            return false;
                        case "Shang":
                            __result = ScriptLocalization.RNPC_Shang_MLP_Married;
                            return false;
                        case "Wesley":
                            __result = ScriptLocalization.RNPC_Wesley_MLP_Married;
                            return false;
                        case "Darius":
                            __result = ScriptLocalization.RNPC_Darius_MLP_Married;
                            return false;
                        case "Xyla":
                            __result = ScriptLocalization.RNPC_Xyla_MLP_Married;
                            return false;
                        case "Catherine":
                            __result = ScriptLocalization.RNPC_Catherine_MLP_Married;
                            return false;
                        case "Lucia":
                            __result = ScriptLocalization.RNPC_Lucia_MLP_Married;
                            return false;
                        case "Iris":
                            __result = ScriptLocalization.RNPC_Iris_MLP_Married;
                            return false;
                        case "Kai":
                            __result = ScriptLocalization.RNPC_Kai_MLP_Married;
                            return false;
                        case "Vaan":
                            __result = ScriptLocalization.RNPC_Vaan_MLP_Married;
                            return false;
                        case "Nathaniel":
                            __result = ScriptLocalization.RNPC_Nathaniel_MLP_Married;
                            return false;
                        default:
                            __result = ScriptLocalization.RNPC_Anne_MLP_Married;
                            return false;
                    }
                }
                switch (____npcName)
                {
                    case "Wornhardt":
                        __result = ScriptLocalization.RNPC_Wornhardt_MLP;
                        return false;
                    case "Zaria":
                        __result = ScriptLocalization.RNPC_Zaria_MLP;
                        return false;
                    case "Donovan":
                        __result = ScriptLocalization.RNPC_Donovan_MLP;
                        return false;
                    case "Karish":
                        __result = ScriptLocalization.RNPC_Karish_MLP;
                        return false;
                    case "Claude":
                        __result = ScriptLocalization.RNPC_Claude_MLP;
                        return false;
                    case "Jun":
                        __result = ScriptLocalization.RNPC_Jun_MLP;
                        return false;
                    case "Vivi":
                        __result = ScriptLocalization.RNPC_Vivi_MLP;
                        return false;
                    case "Lynn":
                        __result = ScriptLocalization.RNPC_Lynn_MLP;
                        return false;
                    case "Liam":
                        __result = ScriptLocalization.RNPC_Liam_MLP;
                        return false;
                    case "Anne":
                        __result = ScriptLocalization.RNPC_Anne_MLP;
                        return false;
                    case "Miyeon":
                        __result = ScriptLocalization.RNPC_Miyeon_MLP;
                        return false;
                    case "Kitty":
                        __result = ScriptLocalization.RNPC_Kitty_MLP;
                        return false;
                    case "Lucius":
                        __result = ScriptLocalization.RNPC_Lucius_MLP;
                        return false;
                    case "Shang":
                        __result = ScriptLocalization.RNPC_Shang_MLP;
                        return false;
                    case "Wesley":
                        __result = ScriptLocalization.RNPC_Wesley_MLP;
                        return false;
                    case "Darius":
                        __result = ScriptLocalization.RNPC_Darius_MLP;
                        return false;
                    case "Xyla":
                        __result = ScriptLocalization.RNPC_Xyla_MLP;
                        return false;
                    case "Catherine":
                        __result = ScriptLocalization.RNPC_Catherine_MLP;
                        return false;
                    case "Lucia":
                        __result = ScriptLocalization.RNPC_Lucia_MLP;
                        return false;
                    case "Iris":
                        __result = ScriptLocalization.RNPC_Iris_MLP;
                        return false;
                    case "Kai":
                        __result = ScriptLocalization.RNPC_Kai_MLP;
                        return false;
                    case "Vaan":
                        __result = ScriptLocalization.RNPC_Vaan_MLP;
                        return false;
                    case "Nathaniel":
                        __result = ScriptLocalization.RNPC_Nathaniel_MLP;
                        return false;
                    default:
                        __result = ScriptLocalization.RNPC_Anne_MLP;
                        return false;
                }
            }
            catch (Exception e)
            {
                write_exception_to_console(e);
                return false;
            }
        }
    }

    [HarmonyPatch(typeof(NPCAI), "MarryPlayer")]
    class HarmonyPatch_Player_MarryPlayer
    {
        public static void Prefix(NPCAI __instance, ref string ____npcName)
        {
            loadSpouts();
            List<string> romancables = new List<string>();
            Debug.Log((object)"PolygamyMod v1 111 - 1");
            if (__instance != null && ____npcName == __instance.OriginalName)
            {
                Debug.Log((object)"PolygamyMod v1 111 - 2");
                PolygamyPlugin.logger.LogInfo("[PolyGamy Mod v0.0.4 UpdatedByWerri]: You can use __instance.OriginalName!");
                Debug.Log((object)"PolygamyMod v1 111 - 3");
            }
            Debug.Log((object)"PolygamyMod v1 111 - 4");
            try
            {
                List<string> spouses = new List<string>();
                Debug.Log((object)"PolygamyMod v1 111 - 5");
                int count = 0;
                string new_main;

                foreach (var npcai in SingletonBehaviour<NPCManager>.Instance._npcs.Values.Where(npcai => SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("MarriedTo" + npcai.OriginalName)))
                {
                    //if (!marriageCharacters.Contains(npcai.OriginalName))
                    //{
                    //    continue;
                    //}
                    if (!npcai.Romanceable)
                        continue;
                    romancables.Add(npcai.OriginalName);
                    Debug.Log((object)"PolygamyMod v1 111 - 6");
                    spouses.Add(npcai.OriginalName);
                    Debug.Log((object)"PolygamyMod v1 111 - 7");
                    count += 1;
                    if (!ultraPolyGamySpouts.ContainsKey("MarriedTo" + __instance.OriginalName))
                    {
                        ultraPolyGamySpouts.Add("MarriedTo" + __instance.OriginalName, __instance.OriginalName);
                    }
                    if (!ultraPolyGamySpouts.ContainsKey("Married" + __instance.OriginalName))
                    {
                        ultraPolyGamySpouts.Add("Married" + __instance.OriginalName, __instance.OriginalName);
                    }
                    if (!ultraPolyGamySpouts.ContainsKey("Dating" + __instance.OriginalName))
                    {
                        ultraPolyGamySpouts.Add("Dating" + __instance.OriginalName, __instance.OriginalName);
                    }
                    if (!ultraPolyGamySpouts.ContainsKey("MarriedWith" + __instance.OriginalName))
                    {
                        ultraPolyGamySpouts.Add("MarriedWith" + __instance.OriginalName, __instance.OriginalName);
                    }
                    if (!ultraPolyGamySpouts.ContainsKey("current_spouse"))
                    {
                        ultraPolyGamySpouts.Add("current_spouse", __instance.OriginalName);
                    }
                }
                if (count <= 1)
                {
                    Debug.Log((object)"PolygamyMod v1 111 - 8");
                    return;
                }

                if (spouses[0] == __instance.OriginalName)
                {
                    Debug.Log((object)"PolygamyMod v1 111 - 9");
                    new_main = spouses[1];
                    if (!ultraPolyGamySpouts.ContainsKey("new_main"))
                    {
                        ultraPolyGamySpouts.Add("new_main", new_main);
                    } else
                    {
                        ultraPolyGamySpouts.Remove("new_main");
                        ultraPolyGamySpouts.Add("new_main", new_main);
                    }
                }
                else
                {
                    Debug.Log((object)"PolygamyMod v1 111 - 10");
                    new_main = spouses[0];
                    if (!ultraPolyGamySpouts.ContainsKey("new_main"))
                    {
                        ultraPolyGamySpouts.Add("new_main", new_main);
                    } else
                    {
                        ultraPolyGamySpouts.Remove("new_main");
                        ultraPolyGamySpouts.Add("new_main", new_main);
                    }
                }
                string current_spouse = SingletonBehaviour<GameSave>.Instance.GetProgressStringCharacter("MarriedWith");
                NPCAI realNPC = null;
                bool haveCurrentSpouse = false;

                if (current_spouse.IsNullOrWhiteSpace())
                {
                    if (!romancables.Contains(current_spouse))
                    {
                        current_spouse = romancables[0];
                        SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", current_spouse);
                        haveCurrentSpouse = true;
                    } else if (ultraPolyGamySpouts.ContainsKey("current_spouse"))
                    {
                        if (ultraPolyGamySpouts.TryGetValue("current_spouse", out current_spouse))
                        {
                            haveCurrentSpouse = true;
                        }
                    }
                } else
                {
                    if (!romancables.Contains(current_spouse))
                    {
                        current_spouse = romancables[0];
                        SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", current_spouse);
                    }

                    haveCurrentSpouse = true;
                }
                if (haveCurrentSpouse) {
                    Debug.Log((object)"PolygamyMod v1 111 - 11");
                    SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld(current_spouse + "MarriedWalkPath", false, true);
                    Debug.Log((object)"PolygamyMod v1 111 - 12");
                    realNPC = SingletonBehaviour<NPCManager>.Instance.GetRealNPC(current_spouse);
                    realNPC.GeneratePath();
                    SingletonBehaviour<NPCManager>.Instance.StartNPCPath(realNPC);
                }
                /*SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("MarriedTo" + this.OriginalName, true);
                SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("Married", true);
                SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("Dating" + this.OriginalName, false);
                SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", this.OriginalName);
                if (SingletonBehaviour<GameSave>.Instance.GetProgressBoolWorld("Tier3House"))
                {
                 SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld(this.OriginalName + "MarriedWalkPath", true);
                 NPCAI realNpc = SingletonBehaviour<NPCManager>.Instance.GetRealNPC(this.OriginalName);
                  realNpc.GeneratePath();
                 SingletonBehaviour<NPCManager>.Instance.StartNPCPath(realNpc);
                }
                Wish.Utilities.UnlockAcheivement(100);
                this.GenerateCycle();*/
                Debug.Log((object)"PolygamyMod v1 111 - 13");
                SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("MarriedTo" + __instance.OriginalName, true);
                SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("Married", true);
                SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("Dating" + __instance.OriginalName, false);
                SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", __instance.OriginalName);
                if (!ultraPolyGamySpouts.ContainsKey("MarriedTo" + __instance.OriginalName))
                {
                    ultraPolyGamySpouts.Add("MarriedTo" + __instance.OriginalName, __instance.OriginalName);
                }
                if (!ultraPolyGamySpouts.ContainsKey("Married" + __instance.OriginalName))
                {
                    ultraPolyGamySpouts.Add("Married" + __instance.OriginalName, __instance.OriginalName);
                }
                if (!ultraPolyGamySpouts.ContainsKey("Dating" + __instance.OriginalName))
                {
                    ultraPolyGamySpouts.Add("Dating" + __instance.OriginalName, __instance.OriginalName);
                }
                if (!ultraPolyGamySpouts.ContainsKey("MarriedWith" + __instance.OriginalName))
                {
                    ultraPolyGamySpouts.Add("MarriedWith" + __instance.OriginalName, __instance.OriginalName);
                }
                if (!ultraPolyGamySpouts.ContainsKey("current_spouse"))
                {
                    ultraPolyGamySpouts.Add("current_spouse", __instance.OriginalName);
                }
                saveSpouts();
                if (SingletonBehaviour<GameSave>.Instance.GetProgressBoolWorld("Tier3House"))
                {
                    Debug.Log((object)"PolygamyMod v1 111 - 14");
                    SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld(__instance.OriginalName + "MarriedWalkPath", true, true);
                    if (!ultraPolyGamySpouts.ContainsKey("MarriedWalkPath" + __instance.OriginalName))
                    {
                        ultraPolyGamySpouts.Add("MarriedWalkPath" + __instance.OriginalName, __instance.OriginalName);
                        saveSpouts();
                    }
                    realNPC = SingletonBehaviour<NPCManager>.Instance.GetRealNPC(__instance.OriginalName);
                    realNPC.GeneratePath();
                    SingletonBehaviour<NPCManager>.Instance.StartNPCPath(realNPC);
                    Debug.Log((object)"PolygamyMod v1 111 - 15");
                }
                Utilities.UnlockAcheivement(100);
                Debug.Log((object)"PolygamyMod v1 111 - 16");
                __instance.GenerateCycle(false);
                Debug.Log((object)"PolygamyMod v1 111 - 17");
                return;
            }
            catch (Exception e)
            {
                Debug.Log((object)"PolygamyMod v1 111 - 18");
                write_exception_to_console(e);
                return;
            }
        }
    }

    [HarmonyPatch(typeof(Player), "RequestSleep")]
    class HarmonyPatch_Player_RequestSleep
    {
        public static bool Prefix(Bed bed, bool isMarriageBed = false, MarriageOvernightCutscene marriageOvernightCutscene = null, bool isCutsceneComplete = false)
        {
            List<string> romancables = new List<string>();
            if (bed != null)
            {
                PolygamyPlugin.logger.LogInfo("[PolyGamy Mod v0.0.4 UpdatedByWerri] RequestSleep: all ok!");
            }
            try
            {
                string marriedWith = SingletonBehaviour<GameSave>.Instance.GetProgressStringCharacter("MarriedWith");
                if (marriedWith.IsNullOrWhiteSpace())
                {
                    marriedWith = "";
                } else
                {
                    marriedWith = " (" + marriedWith + ")";
                }
                string mainSpouseText = "Change main spouse" + marriedWith;
                if (isMarriageBed && !isCutsceneComplete)
                {
                    string sleep = "Early";
                    if (SingletonBehaviour<DayCycle>.Instance.Time.Hour >= 18 || SingletonBehaviour<DayCycle>.Instance.Time.Hour < 6)
                    {
                        sleep = "Sleep";
                    }

                    LocalizedDialogueTree localizedDialogueTree0 = new LocalizedDialogueTree
                    {
                        npc = null
                    };


                    string name0 = "Bed";
                    DialogueNode dialogueNode0 = new DialogueNode();
                    dialogueNode0.dialogueText = new List<string>
                    {
                        "What do you want to do?"
                    };

                    Dictionary<int, Response> dictionary0 = new Dictionary<int, Response>();

                    Response response01 = new Response();
                    response01.responseText = (() => "Sleep");
                    response01.action = delegate ()
                    {
                        localizedDialogueTree0.Talk(sleep, true, null);
                    };
                    dictionary0.Add(0, response01);

                    Response response02 = new Response();
                    response02.responseText = (() => mainSpouseText);
                    response02.action = delegate ()
                    {
                        localizedDialogueTree0.Talk("ChangeMainSpouse1", true, null);
                    };
                    dictionary0.Add(1, response02);
                    dialogueNode0.responses = dictionary0;

                    Response response03 = new Response();
                    response03.responseText = (() => "Nothing");
                    response03.action = delegate ()
                    {
                        DialogueController.Instance.CancelDialogue();
                    };
                    dictionary0.Add(2, response03);
                    dialogueNode0.responses = dictionary0;
                    localizedDialogueTree0.AddNode(name0, dialogueNode0);


                    string name1 = "Sleep";
                    DialogueNode dialogueNode1 = new DialogueNode();
                    dialogueNode1.dialogueText = new List<string>
                    {
                        ScriptLocalization.SleepRequestSpouse
                    };
                    Dictionary<int, Response> dictionary1 = new Dictionary<int, Response>();

                    Response response04 = new Response();
                    response04.responseText = (() => ScriptLocalization.Yes);
                    response04.action = delegate ()
                    {
                        marriageOvernightCutscene.Begin();
                        DialogueController.Instance.CancelDialogue();
                    };
                    dictionary1.Add(0, response04);

                    Response response05 = new Response();
                    response05.responseText = (() => ScriptLocalization.No);
                    response05.action = delegate ()
                    {
                        DialogueController.Instance.CancelDialogue();
                    };
                    dictionary1.Add(1, response05);

                    dialogueNode1.responses = dictionary1;
                    localizedDialogueTree0.AddNode(name1, dialogueNode1);


                    string name2 = "Early";
                    DialogueNode dialogueNode2 = new DialogueNode();
                    dialogueNode2.dialogueText = new List<string>
                    {
                        ScriptLocalization.TooEarlyToSleep
                    };
                    localizedDialogueTree0.AddNode(name2, dialogueNode2);


                    string name3 = "ChangeMainSpouse";

                    List<string> spouses = new List<string>();
                    Dictionary<int, Response> dictionary2 = new Dictionary<int, Response>();
                    int i = 0;
                    int page = 1;
                    string page_name;

                    foreach (var npcai in SingletonBehaviour<NPCManager>.Instance._npcs.Values.Where(npcai => SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("MarriedTo" + npcai.OriginalName)))
                    {
                        loadSpouts();
                        if (!npcai.Romanceable)
                        {
                            continue;
                        }
                        romancables.Add(npcai.OriginalName);
                        spouses.Add(npcai.OriginalName);
                        string npcName = npcai.OriginalName;
                        if (!ultraPolyGamySpouts.ContainsKey("MarriedTo" + npcName))
                        {
                            ultraPolyGamySpouts.Add("MarriedTo" + npcName, npcName);
                        }
                        if (!ultraPolyGamySpouts.ContainsKey("Married" + npcName))
                        {
                            ultraPolyGamySpouts.Add("Married" + npcName, npcName);
                        }
                        if (!ultraPolyGamySpouts.ContainsKey("Dating" + npcName))
                        {
                            ultraPolyGamySpouts.Add("Dating" + npcName, npcName);
                        }
                        if (!ultraPolyGamySpouts.ContainsKey("MarriedWith" + npcName))
                        {
                            ultraPolyGamySpouts.Add("MarriedWith" + npcName, npcName);
                        }
                        if (!ultraPolyGamySpouts.ContainsKey("current_spouse"))
                        {
                            ultraPolyGamySpouts.Add("current_spouse", npcName);
                        }
                        saveSpouts();
                    }
                    foreach (var name in spouses)
                    {
                        if (i == 3)
                        {
                            DialogueNode dialogueNode3 = new DialogueNode();
                            dialogueNode3.dialogueText = new List<string>
                            {
                                "Who will be the main spouse?",
                            };
                            page_name = name3 + page;
                            page += 1;
                            string next_page_name = name3 + page;

                            Response next_page = new Response();
                            next_page.responseText = (() => "Next");
                            next_page.action = delegate ()
                            {
                                localizedDialogueTree0.Talk(next_page_name, true, null);
                            };
                            dictionary2.Add(i, next_page);
                            dialogueNode3.responses = dictionary2;
                            localizedDialogueTree0.AddNode(page_name, dialogueNode3);


                            i = 0;
                            dictionary2 = new Dictionary<int, Response>();
                        }
                        bool haveCurrentSpouse = false;
                        Response response = new Response();
                        response.responseText = (() => name);
                        response.action = delegate ()
                        {
                            string current_spouse = SingletonBehaviour<GameSave>.Instance.GetProgressStringCharacter("MarriedWith");

                            if (current_spouse.IsNullOrWhiteSpace())
                            {
                                if (!romancables.Contains(current_spouse))
                                {
                                    current_spouse = romancables[0];
                                    SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", current_spouse);
                                    haveCurrentSpouse = true;
                                } else if (ultraPolyGamySpouts.ContainsKey("current_spouse"))
                                {
                                    if (ultraPolyGamySpouts.TryGetValue("current_spouse", out current_spouse))
                                    {
                                        haveCurrentSpouse = true;
                                    }
                                }
                            }
                            else
                            {
                                if (!romancables.Contains(current_spouse))
                                {
                                    current_spouse = romancables[0];
                                    SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", current_spouse);
                                }
                                haveCurrentSpouse = true;
                            }

                            SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld(current_spouse + "MarriedWalkPath", false, true);
                            if (!ultraPolyGamySpouts.ContainsKey("current_spouse"))
                            {
                                ultraPolyGamySpouts.Add("current_spouse", current_spouse);
                            }
                            else
                            {
                                ultraPolyGamySpouts.Remove("current_spouse");
                                ultraPolyGamySpouts.Add("current_spouse", current_spouse);
                            }
                            NPCAI realNPC = SingletonBehaviour<NPCManager>.Instance.GetRealNPC(current_spouse);
                            realNPC.GeneratePath();
                            SingletonBehaviour<NPCManager>.Instance.StartNPCPath(realNPC);

                            SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", name);
                            localizedDialogueTree0.Talk("Reenter", true, null);
                            //loadCurrentScene();
                            /*  HarmonyPatch_ScenePortalSpot_EnterHouse.actions.Add(1);
                              SceneFadeType sceneFadeType = SceneFadeType.Circle;
                              if (SingletonBehaviour<GameSave>.Instance.CurrentWorld.HousePortalSpots.TryGetValue(ScenePortalSpot.playerHouseNumber, out serializedVector2))
                              {
                                  SingletonBehaviour<ScenePortalManager>.Instance.ChangeScene(new Vector2(355.9167f, 124.1562f), "2playerfarm", SceneFadeType.Circle);
                            */
                            if (HarmonyPatch_ScenePortalSpot_EnterHouse.oldMeta != null)
                            {
                                HarmonyPatch_ScenePortalSpot_EnterHouse.playerHouse.SetMeta(HarmonyPatch_ScenePortalSpot_EnterHouse.oldMeta);
                            }
                            loadCurrentScene();
                        };
                        dictionary2.Add(i, response);
                        i += 1;
                    }

                    DialogueNode dialogueNode4 = new DialogueNode();
                    dialogueNode4.dialogueText = new List<string>
                    {
                        "Who will be the main spouse?",
                    };
                    page_name = name3 + page;

                    Response nevermind = new Response();
                    nevermind.responseText = (() => "Nevermind");
                    nevermind.action = delegate ()
                    {
                        DialogueController.Instance.CancelDialogue();
                    };
                    dictionary2.Add(i, nevermind);
                    dialogueNode4.responses = dictionary2;
                    localizedDialogueTree0.AddNode(page_name, dialogueNode4);

                    string name4 = "Reenter";
                    DialogueNode dialogueNode5 = new DialogueNode();
                    dialogueNode5.dialogueText = new List<string>
                    {
                        "Re-enter the house to apply changes."
                    };
                    localizedDialogueTree0.AddNode(name4, dialogueNode5);

                    localizedDialogueTree0.Talk("Bed", true, null);
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                write_exception_to_console(ex);
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(LocalizedDialogueTree), "BernardHandleDivorce")]
    class HarmonyPatch_Player_BernardHandleDivorce
    {
        private static LocalizedDialogueTree HandleConfirm(LocalizedDialogueTree localizedDialogueTree0, string name, string new_main)
        {
            try
            {
                if (localizedDialogueTree0 != null)
                {
                    PolygamyPlugin.logger.LogInfo("[PolyGamy Mod v0.0.4 UpdatedByWerri Bernard]: HandleConfirm confirmDivorce!");
                }

                string name1 = "ConfirmDivorce";
                DialogueNode dialogueNode2 = new DialogueNode();
                dialogueNode2.dialogueText = new List<string>
            {
                $"Are you sure you want to divorce {name}?"
            };
                Dictionary<int, Response> dictionary1 = new Dictionary<int, Response>();

                Response response01 = new Response();
                response01.responseText = (() => ScriptLocalization.Yes);
                response01.action = delegate ()
                {
                    Utilities.UnlockAcheivement(131);
                    if (SingletonBehaviour<GameSave>.Instance.GetProgressStringCharacter("MarriedWith") == name)
                    {
                        SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", new_main);
                    }
                    SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("MarriedTo" + name, false);
                    SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld(name + "MarriedWalkPath", false);
                    GameSave.CurrentCharacter.Relationships[name] = 40f;

                    NPCAI realNPC = SingletonBehaviour<NPCManager>.Instance.GetRealNPC(name);
                    realNPC.GenerateCycle(false);
                    realNPC.GeneratePath();
                    SingletonBehaviour<NPCManager>.Instance.StartNPCPath(realNPC);
                };
                dictionary1.Add(0, response01);

                Response response05 = new Response();
                response05.responseText = (() => ScriptLocalization.No);
                response05.action = delegate ()
                {
                    DialogueController.Instance.CancelDialogue();
                };
                dictionary1.Add(1, response05);

                dialogueNode2.responses = dictionary1;
                localizedDialogueTree0.AddNode(name1, dialogueNode2);
                return localizedDialogueTree0;
            }
            catch (Exception ex)
            {
                write_exception_to_console(ex);
                return localizedDialogueTree0;
            }
        }

        private static bool Prefix()
        {
            List<string> romancables = new List<string>();
            try
            {
                if (true)
                {
                    PolygamyPlugin.logger.LogInfo("[PolyGamy Mod v0.0.4 UpdatedByWerri Bernard]: Divorce Prefix multiple spouses!");
                }

                LocalizedDialogueTree localizedDialogueTree0 = new LocalizedDialogueTree
                {
                    npc = null
                };

                string name0 = "Divorce";

                List<string> spouses = new List<string>();
                Dictionary<int, Response> dictionary0 = new Dictionary<int, Response>();
                int i = 0;
                int page = 1;
                string page_name;
                int count = 0;
                string new_main;

                foreach (var npcai in SingletonBehaviour<NPCManager>.Instance._npcs.Values.Where(npcai => SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("MarriedTo" + npcai.OriginalName)))
                {
                    if (!npcai.Romanceable)
                    {
                        continue;
                    }
                    romancables.Add(npcai.OriginalName);
                    spouses.Add(npcai.OriginalName);
                    count += 1;
                }
                if (count <= 1)
                {
                    return true;
                }
                foreach (var name in spouses)
                {
                    if (i == 3)
                    {
                        DialogueNode dialogueNode0 = new DialogueNode();
                        dialogueNode0.dialogueText = new List<string>
                    {
                        "Who do you want to divorce?"
                    };
                        page_name = name0 + page;
                        page += 1;
                        string next_page_name = name0 + page;

                        Response next_page = new Response();
                        next_page.responseText = (() => "Next");
                        next_page.action = delegate ()
                        {
                            localizedDialogueTree0.Talk(next_page_name, true, null);
                        };
                        dictionary0.Add(i, next_page);
                        dialogueNode0.responses = dictionary0;
                        localizedDialogueTree0.AddNode(page_name, dialogueNode0);


                        i = 0;
                        dictionary0 = new Dictionary<int, Response>();
                    }

                    Response response = new Response();
                    response.responseText = (() => name);
                    response.action = delegate ()
                    {
                        if (spouses[0] == name)
                        {
                            new_main = spouses[1];
                        }
                        else
                        {
                            new_main = spouses[0];
                        }

                        localizedDialogueTree0 = HandleConfirm(localizedDialogueTree0, name, new_main);
                        localizedDialogueTree0.Talk("ConfirmDivorce", true, null);
                    };
                    dictionary0.Add(i, response);
                    i += 1;

                }

                DialogueNode dialogueNode1 = new DialogueNode();
                dialogueNode1.dialogueText = new List<string>
            {
                "Who do you want to divorce?"
            };
                page_name = name0 + page;

                Response nevermind = new Response();
                nevermind.responseText = (() => "Nevermind");
                nevermind.action = delegate ()
                {
                    DialogueController.Instance.CancelDialogue();
                };
                dictionary0.Add(i, nevermind);
                dialogueNode1.responses = dictionary0;
                localizedDialogueTree0.AddNode(page_name, dialogueNode1);

                localizedDialogueTree0.Talk("Divorce1", true, null);

                return false;
            }
            catch (Exception ex)
            {
                write_exception_to_console(ex);
                return false;
            }
        }
    }
}