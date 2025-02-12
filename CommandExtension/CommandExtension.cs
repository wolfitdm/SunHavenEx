using BepInEx;
using HarmonyLib;
using QFSW.QC;
using QFSW.QC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using System.IO;
using Wish;
using System.Runtime.Remoting.Messaging;
using PSS;
using System.Collections;
using System.Threading;
using System.Security.Policy;
using KeepAlive;
using BepInEx.Logging;
using BepInEx.Configuration;
using ZeroFormatter;
using TinyJson;

namespace CommandExtension
{
    public class PluginInfo
    {
        public const string PLUGIN_AUTHOR = "Rx4Byte";
        public const string PLUGIN_NAME = "Command Extension";
        public const string PLUGIN_GUID = "com.Rx4Byte.CommandExtension";
        public const string PLUGIN_VERSION = "1.5.3";
    }
    [CommandPrefix("!")]
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public partial class CommandExtension : BaseUnityPlugin
    {
        #region VAR's
        // debug var's
        public const bool debug = false;
        public const bool debugLog = debug;
        #region COMMAND's
        // DEAFULT COMMAND PARAMS
        public static class CommandParamDefaults
        {
            public const float timeMultiplier = 0.2F;
        }
        // COMMANDS
        public const string CmdPrefix = "!"; // just the prefix, no command
        public const string CmdHelp = CmdPrefix + "help";
        public const string CmdMineReset = CmdPrefix + "minereset";
        public const string CmdPause = CmdPrefix + "pause";
        public const string CmdCustomDaySpeed = CmdPrefix + "timespeed";
        public const string CmdMoney = CmdPrefix + "money";
        public const string CmdCoins = CmdPrefix + "coins";
        public const string CmdOrbs = CmdPrefix + "orbs";
        public const string CmdTickets = CmdPrefix + "tickets";
        public const string CmdSetDate = CmdPrefix + "time";
        public const string CmdWeather = CmdPrefix + "weather";
        public const string CmdDevKit = CmdPrefix + "devkit";
        public const string CmdJumper = CmdPrefix + "jumper";
        public const string CmdState = CmdPrefix + "state";
        public const string CmdPrintItemIds = CmdPrefix + "getitemids";
        public const string CmdSleep = CmdPrefix + "sleep";
        public const string CmdDasher = CmdPrefix + "dasher";
        public const string CmdManaFill = CmdPrefix + "manafill";
        public const string CmdManaInf = CmdPrefix + "manainf";
        public const string CmdHealthFill = CmdPrefix + "healthfill";
        public const string CmdNoHit = CmdPrefix + "nohit";
        public const string CmdMineOverfill = CmdPrefix + "mineoverfill";
        public const string CmdMineClear = CmdPrefix + "mineclear";
        public const string CmdNoClip = CmdPrefix + "noclip";
        public const string CmdPrintHoverItem = CmdPrefix + "printhoveritem";
        public const string CmdName = CmdPrefix + "name";
        public const string CmdFeedbackDisabled = CmdPrefix + "feedback";
        public const string CmdGive = CmdPrefix + "give";
        public const string CmdShowItems = CmdPrefix + "items";
        public const string CmdAutoFillMuseum = CmdPrefix + "autofillmuseum";
        public const string CmdCheatFillMuseum = CmdPrefix + "cheatfillmuseum";
        public const string CmdUI = CmdPrefix + "ui";
        public const string CmdTeleport = CmdPrefix + "tp";
        public const string CmdTeleportLocations = CmdPrefix + "tps";
        public const string CmdDespawnPet = CmdPrefix + "despawnpet";
        public const string CmdSpawnPet = CmdPrefix + "pet";
        public const string CmdPetList = CmdPrefix + "pets";
        public const string CmdAppendItemDescWithId = CmdPrefix + "showid";
        public const string CmdRelationship = CmdPrefix + "relationship";
        public const string CmdUnMarry = CmdPrefix + "divorce";
        public const string CmdMarryNpc = CmdPrefix + "marry";
        public const string CmdSetSeason = CmdPrefix + "season";
        public const string CmdFixYear = CmdPrefix + "yearfix";
        public const string CmdIncDecYear = CmdPrefix + "years";
        public const string CmdCheats = CmdPrefix + "cheats";
        public const string CmdQuestsList = CmdPrefix + "listquests";
        public const string CmdQuestLog = CmdPrefix + "questlog";
        public const string CmdQuestAdd = CmdPrefix + "addquest";
        public const string CmdQuestRemove = CmdPrefix + "removequest";
        public const string CmdSaveGame = CmdPrefix + "save";
        public const string CmdSleepSave = CmdPrefix + "sleepsave";
        public const string CmdBackupSaveGame = CmdPrefix + "backupsave";
        public const string CmdBackupSleepSave = CmdPrefix + "backupsleepsave";
        public const string CmdChristmas = CmdPrefix + "christmas";
        public const string CmdSetTp = CmdPrefix + "settp";
        public const string CmdGetTp = CmdPrefix + "gettp";
        public const string CmdListTp = CmdPrefix + "listtp";
        public const string CmdMainSp = CmdPrefix + "msp";
        public enum CommandState { None, Activated, Deactivated }
        // COMMAND CLASS
        public class Command
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public CommandState State { get; set; }
            public Command(string name, string description, CommandState state)
            {
                Name = name;
                Description = description;
                State = state;
            }
        }
   
        // COMMAND CREATION
        public static Command[] Commands = new Command[]
        {
            new Command(CmdHelp,                    "print commands to chat",                                                   CommandState.None),
            new Command(CmdMineReset,               "refill all mine shafts!",                                                  CommandState.None),
            new Command(CmdPause,                   "toggle time pause!",                                                       CommandState.Deactivated),
            new Command(CmdCustomDaySpeed,          "toggle or change dayspeed, paused if '!pause' is activ!",                  CommandState.Deactivated),
            new Command(CmdMoney,                   "give or remove coins",                                                     CommandState.None),
            new Command(CmdOrbs,                    "give or remove Orbs",                                                      CommandState.None),
            new Command(CmdTickets,                 "give or remove Tickets",                                                   CommandState.None),
            new Command(CmdSetDate,                 "set HOURE '6-23' e.g. 'set h 12'\nset DAY '1-28' e.g. 'set d 12'",         CommandState.None),
            new Command(CmdWeather,                 "'!weather [raining|heatwave|clear]'",                                      CommandState.None),
            new Command(CmdDevKit,                  "get dev items",                                                            CommandState.None),
            new Command(CmdJumper,                  "jump over object's (actually noclip while jump)",                          CommandState.Deactivated),
            new Command(CmdState,                   "print activ commands",                                                     CommandState.None),
            new Command(CmdPrintItemIds,            "print item ids [xp|money|all|bonus]",                                      CommandState.None),
            new Command(CmdSleep,                   "sleep to next the day",                                                    CommandState.None),
            new Command(CmdDasher,                  "infinite dashes",                                                          CommandState.Deactivated),
            new Command(CmdManaFill,                "mana refill",                                                              CommandState.None),
            new Command(CmdManaInf,                 "infinite mana",                                                            CommandState.Deactivated),
            new Command(CmdHealthFill,              "health refill",                                                            CommandState.None),
            new Command(CmdNoHit,                   "no hit (disable hitbox)",                                                  CommandState.Deactivated),
            new Command(CmdMineOverfill,            "fill mine completely with rocks & ores",                                   CommandState.None),
            new Command(CmdMineClear,               "clear mine completely from rocks & ores",                                  CommandState.None),
            new Command(CmdNoClip,                  "walk trough everything",                                                   CommandState.Deactivated),
            new Command(CmdPrintHoverItem,          "print item id to chat",                                                    CommandState.Deactivated),
            new Command(CmdName,                    "set name for command target ('!name Lynn') only '!name resets it' ",       CommandState.None),
            new Command(CmdFeedbackDisabled,        "toggle command feedback on/off",                                           CommandState.Deactivated),
            new Command(CmdGive,                    "give [ID] [AMOUNT]*",                                                      CommandState.None),
            new Command(CmdShowItems,               "print items with the given name",                                          CommandState.None),
            new Command(CmdAutoFillMuseum,          "toggle museum's auto fill upon entry",                                     CommandState.Deactivated),
            new Command(CmdCheatFillMuseum,         "toggle fill museum completely upon entry",                                 CommandState.Deactivated),
            new Command(CmdUI,                      "turn ui on/off",                                                           CommandState.None),
            new Command(CmdTeleport,                "teleport to some locations",                                               CommandState.None),
            new Command(CmdTeleportLocations,       "get teleport locations",                                                   CommandState.None),
            new Command(CmdDespawnPet,              "despawn current pet'",                                                     CommandState.None),
            new Command(CmdSpawnPet,                "spawn a specific pet 'pet [name]'",                                        CommandState.None),
            new Command(CmdPetList,                 "get the full list of pets '!pets'",                                        CommandState.None),
            new Command(CmdAppendItemDescWithId,    "toggle id shown to item description",                                      CommandState.Deactivated),
            new Command(CmdRelationship,            "'!relationship [name/all] [value] [add]*'",                                CommandState.None),
            new Command(CmdUnMarry,                 "unmarry an NPC '!divorce [name/all]'",                                     CommandState.None),
            new Command(CmdMarryNpc,                "marry an NPC '!marry [name/all]'",                                         CommandState.None),
            new Command(CmdSetSeason,               "change season",                                                            CommandState.None),
            new Command(CmdFixYear,                 "fix year (if needed)",                                                     CommandState.Activated),
            new Command(CmdIncDecYear,              "add or sub years '!years [value]' '-' to sub",                             CommandState.None),
            new Command(CmdCheats,                  "Toggle Cheats and hotkeys like F7,F8",                                     CommandState.Deactivated),
            new Command(CmdQuestsList,              "See the quest ids from the entire game",                                   CommandState.None),
            new Command(CmdQuestLog,                "See the quest ids from the quest log",                                     CommandState.None),
            new Command(CmdQuestAdd,                "Add a quest id",                                                           CommandState.None),
            new Command(CmdQuestRemove,             "Remove a quest id",                                                        CommandState.None),
            new Command(CmdSaveGame,                "Save the game",                                                            CommandState.None),
            new Command(CmdSleepSave,               "Sleep and save the game",                                                  CommandState.None),
            new Command(CmdBackupSaveGame,          "Save the game (backup)",                                                   CommandState.None),
            new Command(CmdBackupSleepSave,         "Sleep and save the game (backup)",                                         CommandState.None),
            new Command(CmdChristmas,               "Gives you a random item! Merry christmas!",                                CommandState.None),
            new Command(CmdSetTp,                   "Set a tp point on your location!",                                         CommandState.None),
            new Command(CmdGetTp,                   "Teleport to a setted tp point!",                                           CommandState.None),
            new Command(CmdListTp,                  "List your tp points!",                                                     CommandState.None),
            new Command(CmdMainSp,                  "Change or see your main spouse!",                                          CommandState.None),
        };
        #endregion

        // QUEST ID's

        public static string[] allQuestIds = new string[]
        {
            "AStrangerInTown",
            "APromiseMade1Quest",
            "APromiseMade2Quest",
            "DarkWaters1Quest",
            "DarkWaters2Quest",
            "DarkWaters3Quest",
            "DarkWaters4Quest",
            "DarkWaters5Quest",
            "DarkWaters6Quest",
            "DarkWaters7Quest",
            "DarkWaters8Quest",
            "DarkWaters9Quest",
            "DarkWaters10Quest",
            "DarkWaters11Quest",
            "DarkWaters12Quest",
            "DarkWaters13Quest",
            "DarkWaters14Quest",
            "DarkWaters15Quest",
            "DarkWaters16Quest",
            "DarkWaters17Quest",
            "DarkWaters18Quest",
            "DarkWaters19Quest",
            "DarkWaters20Quest",
            "DarkWaters21Quest",
            "DarkWaters22Quest",
            "DarkWaters23Quest",
            "DarkWaters24Quest",
            "TheCallOf1Quest",
            "TheCallOf2Quest",
            "TheWhirlpoolQuest",
            "AmateurDentistryQuest",
            "AmmoResupplyQuest",
            "AnchorsAWeighQuest",
            "ATadRustyQuest",
            "AwTrenchnutsQuest",
            "DefendingTheDeepsQuest",
            "FathomlessAppetiteQuest",
            "IWantToysQuest",
            "KabobJobQuest",
            "KeelhaulEmAllQuest",
            "KeysToSuccessQuest",
            "LendASandQuest",
            "ManaFuelQuest",
            "MonsterBaitQuest",
            "PilferingPinchersQuest",
            "SeasideSaladQuest",
            "SeeingStarsQuest",
            "SmoothieMoveQuest",
            "ThievesOfTheDeepQuest",
            "TreatsToEatQuest",
            "UnburiedTreasureQuest",
            "UnderwaterFashionQuest",
            "ValueOfADollarQuest",
            "WantedWormsQuest",
            "WoodYouHelpQuest",
            "WoopWoopQuest",
            "AngelsPolishQuest",
            "ATonicForLukeQuest",
            "BeeancasHoneyQuest",
            "CaspiansTrinketsQuest",
            "ClaysPestControlQuest",
            "GrensProofQuest",
            "GriffinsGottaEatQuest",
            "IrisCrystalsQuest",
            "JarrodsSightQuest",
            "LuciusJarsQuest",
            "LuciusNectarineSmoothieQuest",
            "MorgansLeavesQuest",
            "OpalsFishQuest",
            "OreForSylviaQuest",
            "PlatosSugarPlumsQuest",
            "QuincysFoodServiceQuest",
            "ReedsNeedsQuest",
            "RelTarsPeanutsQuest",
            "TornnsDyeQuest",
            "VaansHorticultureQuest",
            "WesleysEmergencyQuest",
            "WesleysWalkChoy",
            "WillowsCrystalsQuest",
            "AlbertsDietQuest",
            "AlbertsPasttimeQuest",
            "AllisonsClamsQuest",
            "AllisonsRiceBallQuest",
            "AllNighterQuest",
            "AmandasFavoriteFoodQuest",
            "AnnesSapphireQuest",
            "BarracksHelmetShortageQuest",
            "BarracksSwordShortageQuest",
            "BilliesCarbsQuest",
            "CalvinsBigBurgerQuest",
            "CalvinsLegsQuest",
            "CamilasLampQuest",
            "CatherinesRecipeQuest",
            "CharityCallQuest",
            "CheeseForLiam",
            "ChocolateMilkStandQuest",
            "ClaudesCravingQuest",
            "ClaudesTomatoesQuest",
            "ElizabethsProofQuest",
            "EmmasIronWillQuest",
            "EmmettsSkinCareQuest",
            "FireCrystalDeliveryQuest",
            "FlavorSavorQuest",
            "FoolsToolsQuest",
            "GiuseppesLoomQuest",
            "HeathersBirthstoneQuest",
            "HeathersShipmentQuest",
            "JudithsCenterpieceQuest",
            "JunsHoneyQuest",
            "KaisHardWood",
            "KaisHotSauce",
            "KaisSashimiQuest",
            "KarasRecordQuest",
            "KarasStrawberriesQuest",
            "KittysSecretStarfishQuest",
            "LestersLampQuest",
            "LestersTroutQuest",
            "LuciasSandDollarsQuest",
            "LynnsBlueberrySaladQuest",
            "MakeASeatQuest",
            "MarisTrickPeppersQuest",
            "MinjisBalloonFruitQuest",
            "MiyeonsDaisiesQuest",
            "MiyeonsRaspberriesQuest",
            "NathanielsEquipmentQuest",
            "OnionsForRonaldQuest",
            "PetersSealegsQuest",
            "PintosHeadacheQuest",
            "PintosPosturingQuest",
            "BassBase",
            "ChillOutMan",
            "FamilyFarm",
            "HayIsForHorses",
            "LockandSnowd",
            "PumpedForPumpkin",
            "SeeingIsBeLeaving",
            "TouchSand",
            "PodsPrankQuest",
            "PodsStrawBedQuest",
            "PracticeApplesQuest",
            "RaimisLullabyQuest",
            "CatherinesBrewQuest",
            "CopperForCamila",
            "EmmasDebtQuest",
            "LestersSnackQuest",
            "NoodlesForGiuseppe",
            "RozasFertilizerQuest",
            "ALittleHeartQuest",
            "AxingAFavorQuest",
            "BeamMeUpQuest",
            "CakeForCharlieQuest",
            "CiderAndFishinQuest",
            "CloverAndOutQuest",
            "CocoaSurpriseQuest",
            "FallSpiceQuest",
            "FruitDrillsQuest",
            "FunnyHoneyQuest",
            "FurnaceFireQuest",
            "GetThePaperQuest",
            "HomesForBeesQuest",
            "LeafMeAloneQuest",
            "LettuceGoQuest",
            "LightLunchQuest",
            "MeanAndGreenQuest",
            "MetalMantleQuest",
            "MoreFloorQuest",
            "NoManLikeSnowmanQuest",
            "PeasAndThankYouQuest",
            "PopOffQuest",
            "ResupplyMissionQuest",
            "SoupOnQuest",
            "SpotOfTeaQuest",
            "SpringCleaningQuest",
            "SpringRosesQuest",
            "StarOfTheShowQuest",
            "SummerBountyQuest",
            "SummerCharmsQuest",
            "SummerSnacksQuest",
            "SweetMemoriesQuest",
            "ThatsAWrapQuest",
            "ToastInNeedQuest",
            "TurnUpTheHeatQuest",
            "WallRepairQuest",
            "WarmAndFuzzyQuest",
            "WarmTheAnimalsQuest",
            "WindmillRepairQuest",
            "WinterDessertQuest",
            "ShangsHardstoneQuest",
            "ShangsMilkQuest",
            "SmoothieForEmmettQuest",
            "SolonsFavoriteQuest",
            "SophiesArmorQuest",
            "SophiesStrengthTestQuest",
            "SprucingSeasonQuest",
            "TonyasNewCouchQuest",
            "TopisBridgeQuest",
            "TopisTattoos",
            "VivisBananas",
            "VivisBars",
            "VivisRibs",
            "WornhardtsPaperweightsQuest",
            "WornhardtsSuppliesQuest",
            "ArnoldsShirtQuest",
            "CasperaNeedsSilkQuest",
            "CassiasCandleQuest",
            "ChristinesCratesQuest",
            "CordeliasBountyQuest",
            "CordeliasPlantQuest",
            "CushionForHoneyQuest",
            "DonovansBonesQuest",
            "DwaynesWeightsQuest",
            "ErisUmbrasCompassQuest",
            "FaeyonsScratcherQuest",
            "MirrorForFelixQuest",
            "PhoebesCravingQuest",
            "RibbonForTonyQuest",
            "SlobertsMedicationQuest",
            "SpectralKnightsShoeQuest",
            "TapeForTaliQuest",
            "TheDoctorsBrewQuest",
            "TonysShrimpQuest",
            "WyattsLunchQuest",
            "CombatTrainingQuest",
            "CornTributeQuest",
            "AlbertsKeepsakeQuest",
            "AmandasBookQuest",
            "BernardsBannersQuest",
            "CalvinIsBoredQuest",
            "CamilasCravingQuest",
            "CatherinesFertilizerQuest",
            "ClaudesRecordQuest",
            "EmmasSpillQuest",
            "HeatTheBarracksQuest",
            "HelpfulHalitosisQuest",
            "JustWhatTheDoctorOrderedQuest",
            "KittysDressQuest",
            "KittysGardenQuest",
            "KittyWantsFishQuest",
            "AHerosHarvest1AQuest",
            "AHerosHarvest1BQuest",
            "AHerosHarvest2Quest",
            "AHerosHarvest3Quest",
            "AHerosHarvest4Quest",
            "AHerosHarvest5Quest",
            "Daybreak1AQuest",
            "Daybreak1BQuest",
            "Daybreak2Quest",
            "FriendsToNelvari0Quest",
            "FriendsToNelvari1Quest",
            "FriendsToNelvari2Quest",
            "FriendsToNelvari3Quest",
            "FriendsToNelvari4Quest",
            "FriendsToNelvari5Quest",
            "FriendsToNelvari6Quest",
            "FriendsToNelvari7Quest",
            "PeaceWithWithergate1Quest",
            "PeaceWithWithergate2Quest",
            "PeaceWithWithergate3Quest",
            "PeaceWithWithergate4Quest",
            "PeaceWithWithergate5Quest",
            "PeaceWithWithergate6Quest",
            "PeaceWithWithergate7Quest",
            "PeaceWithWithergate8Quest",
            "ANeighborsAid1Quest",
            "ANeighborsAid2Quest",
            "AWheelyBigProblemQuest",
            "BladeBuddy1Quest",
            "BladeBuddy2Quest",
            "BladeBuddy3Quest",
            "Halloween1Quest",
            "Halloween2Quest",
            "Halloween3Quest",
            "HolidaySoup",
            "LanternFestival1Quest",
            "LanternFestival2Quest",
            "LanternFestival3Quest",
            "LanternFestival4Quest",
            "LanternFestival5Quest",
            "MusicFestival1Quest",
            "MusicFestival2Quest",
            "MusicFestival3Quest",
            "PickaxePartner1Quest",
            "PickaxePartner2Quest",
            "PickaxePartner3Quest",
            "PlayerBirthdayQuest",
            "SecretSantaAnne",
            "SecretSantaCatherine",
            "SecretSantaClaude",
            "SecretSantaDarius",
            "SecretSantaDonovan",
            "SecretSantaIris",
            "SecretSantaJun",
            "SecretSantaKai",
            "SecretSantaKarish",
            "SecretSantaKitty",
            "SecretSantaLiam",
            "SecretSantaLucia",
            "SecretSantaLucius",
            "SecretSantaLynn",
            "SecretSantaMiyeon",
            "SecretSantaNathaniel",
            "SecretSantaShang",
            "SecretSantaVaan",
            "SecretSantaVivi",
            "SecretSantaWesley",
            "SecretSantaWornhardt",
            "SecretSantaXyla",
            "SecretSantaZaria",
            "SummerBBQ1Quest",
            "SummerBBQ2Quest",
            "SummerBBQ3Quest",
            "WheelinNDealinQuest",
            "WheelRepairQuest",
            "WinterFestival1Quest",
            "WinterFestival2Quest",
            "WinterFestival3Quest",
            "AnneHangout1Quest",
            "AnneHangout2Quest",
            "CatherineHangout1Quest",
            "CatherineHangout2Quest",
            "ClaudeHangout1Quest",
            "ClaudeHangout2Quest",
            "DariusHangout1Quest",
            "DariusHangout2Quest",
            "DonovanHangout1Quest",
            "DonovanHangout2Quest",
            "IrisHangout1Quest",
            "IrisHangout2Quest",
            "JunHangout1Quest",
            "JunHangout2Quest",
            "KaiHangout1Quest",
            "KaiHangout2Quest",
            "KarishHangout1Quest",
            "KarishHangout2Quest",
            "KittyHangout1Quest",
            "KittyHangout2Quest",
            "LiamHangout1Quest",
            "LiamHangout2Quest",
            "LuciaHangout1Quest",
            "LuciaHangout2Quest",
            "LuciusHangout1Quest",
            "LuciusHangout2Quest",
            "LynnHangout1Quest",
            "LynnHangout2Quest",
            "MiyeonHangout1Quest",
            "MiyeonHangout2Quest",
            "NathanielHangout1Quest",
            "NathanielHangout2Quest",
            "ShangHangout1Quest",
            "ShangHangout2Quest",
            "VaanHangout1Quest",
            "VaanHangout2Quest",
            "ViviHangout1Quest",
            "ViviHangout2Quest",
            "WesleyHangout1Quest",
            "WesleyHangout2Quest",
            "WornhardtHangout1Quest",
            "WornhardtHangout2Quest",
            "XylaHangout1Quest",
            "XylaHangout2Quest",
            "ZariaHangout1Quest",
            "ZariaHangout2Quest",
            "HarvestingWheat1Quest",
            "HarvestingWheat2Quest",
            "Intro1Quest",
            "Intro2Quest",
            "Intro3Quest",
            "JudithsWeedsQuest",
            "JunBeautificationQuest",
            "JunsLessonQuest",
            "KaiHomeImprovement",
            "ANewHome1Quest",
            "ANewHome2Quest",
            "ANewHome3Quest",
            "ANewHome4Quest",
            "ANewHome5Quest",
            "ClearingTheRoad1Quest",
            "ClearingTheRoad2Quest",
            "ConfrontingDynus1Quest",
            "ConfrontingDynus2Quest",
            "ConfrontingDynus3Quest",
            "ConfrontingDynus4Quest",
            "ConfrontingDynus5Quest",
            "ConfrontingDynus6Quest",
            "DealingWithADragon1Quest",
            "DealingWithADragon2Quest",
            "DealingWithADragon3Quest",
            "DealingWithADragon4Quest",
            "DealingWithADragon5Quest",
            "DealingWithADragon6Quest",
            "DealingWithADragon7Quest",
            "DealingWithADragon8Quest",
            "JourneyToWithergate1Quest",
            "JourneyToWithergate2Quest",
            "JourneyToWithergate3Quest",
            "JourneyToWithergate4Quest",
            "JourneyToWithergate5Quest",
            "JourneyToWithergate6Quest",
            "NewJourneyToWithergate1Quest",
            "NewJourneyToWithergate2Quest",
            "NewJourneyToWithergate3Quest",
            "NewJourneyToWithergate4Quest",
            "NewJourneyToWithergate5Quest",
            "NewJourneyToWithergate6Quest",
            "NewJourneyToWithergate7Quest",
            "NewJourneyToWithergate8Quest",
            "NewJourneyToWithergate9Quest",
            "NewJourneyToWithergate10Quest",
            "NewJourneyToWithergate11Quest",
            "NewJourneyToWithergate12Quest",
            "NewJourneyToWithergate13Quest",
            "NewJourneyToWithergate14Quest",
            "NewJourneyToWithergate15Quest",
            "NewJourneyToWithergate16Quest",
            "NewJourneyToWithergate17Quest",
            "NewJourneyToWithergate18Quest",
            "NewJourneyToWithergate19Quest",
            "NewJourneyToWithergate20Quest",
            "NewJourneyToWithergate21Quest",
            "NivarasLessonCommonality1Quest",
            "NivarasLessonCommonality2Quest",
            "NivarasLessonCommonality3Quest",
            "NivarasLessonCommonality4Quest",
            "NivarasLessonCommonality5Quest",
            "NivarasLessonCommonality6Quest",
            "NivarasLessonCommonality7Quest",
            "NivarasLessonCommonality8Quest",
            "NivarasLessonGrowth1Quest",
            "NivarasLessonGrowth2Quest",
            "NivarasLessonGrowth3Quest",
            "NivarasLessonGrowth4Quest",
            "NivarasLessonPerspective1Quest",
            "NivarasLessonPerspective2Quest",
            "NivarasLessonPerspective3Quest",
            "NivarasLessonPerspective4Quest",
            "NivarasLessonPerspective5Quest",
            "NivarasLessonPerspective6Quest",
            "NivarasLessonPerspective7Quest",
            "PathToNelvari1Quest",
            "PathToNelvari2Quest",
            "PathToNelvari3Quest",
            "PathToNelvari4Quest",
            "PathToNelvari5Quest",
            "PathToNelvari6Quest",
            "PathToNelvari7Quest",
            "PathToNelvari8Quest",
            "PathToNelvari9Quest",
            "PathToNelvari10Quest",
            "PathToNelvari11Quest",
            "PathToNelvari12Quest",
            "PathToNelvari13Quest",
            "PathToNelvari14Quest",
            "PathToNelvari15Quest",
            "PathToNelvari16Quest",
            "PathToNelvari17Quest",
            "PathToNelvari18Quest",
            "PathToNelvari19Quest",
            "PathToNelvari20Quest",
            "PathToNelvari21Quest",
            "PathToNelvari22Quest",
            "PathToNelvari23Quest",
            "PathToNelvari24Quest",
            "QuarryOrderQuest",
            "SunDragonsApprentice1Quest",
            "SunDragonsApprentice2Quest",
            "SunDragonsApprentice3Quest",
            "SunDragonsApprentice4Quest",
            "TheKingsFavor1Quest",
            "TheKingsFavor2Quest",
            "TheKingsFavor3Quest",
            "TheKingsFavor4Quest",
            "TheKingsFavor5Quest",
            "TheKingsFavor6Quest",
            "TheKingsFavor7AQuest",
            "TheKingsFavor7BQuest",
            "TheKingsFavor8Quest",
            "TheKingsFavor9Quest",
            "TheKingsFavor10Quest",
            "TheKingsFavor11Quest",
            "TheKingsFavor12Quest",
            "TheMysteryOfNelvari1Quest",
            "TheMysteryOfNelvari2Quest",
            "TheMysteryOfNelvari3Quest",
            "TheMysteryOfNelvari4Quest",
            "TheSunDragonsProtection0Quest",
            "TheSunDragonsProtection1Quest",
            "TheSunDragonsProtection2Quest",
            "TheSunDragonsProtection3Quest",
            "TheSunDragonsProtection4Quest",
            "TheSunDragonsProtection5Quest",
            "TheSunDragonsProtection6Quest",
            "TheSunDragonsProtection7Quest",
            "TheSunDragonsProtection8Quest",
            "TheSunDragonsProtection9Quest",
            "TheWorldDragon1Quest",
            "TheWorldDragon2Quest",
            "TheWorldDragon3Quest",
            "TheWorldDragon4Quest",
            "TheWorldDragon5Quest",
            "TheWorldDragon6Quest",
            "TheWorldDragon7Quest",
            "TheWorldDragon8Quest",
            "UnwelcomeWelcoming1Quest",
            "UnwelcomeWelcoming2Quest",
            "UnwelcomeWelcoming3Quest",
            "UnwelcomeWelcoming4Quest",
            "WelcomeToSunHaven1Quest",
            "WelcomeToSunHaven2Quest",
            "WelcomeToSunHaven3Quest",
            "WelcomeToSunHaven4Quest",
            "WelcomeToSunHaven5Quest",
            "AnneMarriageQuest",
            "CatherineMarriageQuest",
            "ClaudeMarriageQuest",
            "DariusMarriageQuest",
            "DonovanMarriageQuest",
            "IrisMarriageQuest",
            "JunMarriageQuest",
            "KaiMarriageQuest",
            "KarishMarriageQuest",
            "KittyMarriageQuest",
            "LiamMarriageQuest",
            "LuciaMarriageQuest",
            "LuciusMarriageQuest",
            "LynnMarriageQuest",
            "MiyeonMarriageQuest",
            "NathanielMarriageQuest",
            "ShangMarriageQuest",
            "VaanMarriageQuest",
            "ViviMarriageQuest",
            "WesleyMarriageQuest",
            "WornhardtMarriageQuest",
            "XylaMarriageQuest",
            "ZariaMarriageQuest",
            "MiyeonsBeanstalkQuest",
            "MiyeonsGrowBeanstalkQuest",
            "MiyeonsWaterBeanstalkQuest",
            "MonkeyMadnessQuest",
            "AskingPodQuest",
            "DinnerOnRonaldQuest",
            "GetTheSpaghettiReadyQuest",
            "PetersLureQuest",
            "AMinorMinerQuest",
            "GrowingAFamily0Quest",
            "GrowingAFamily1Quest",
            "GrowingAFamily2Quest",
            "GrowingAFamily3Quest",
            "GrowingAFamily4Quest",
            "GrowingAFamily5Quest",
            "GrowingAFamily6Quest",
            "GrowingAFamily7Quest",
            "GrowingAFamily8Quest",
            "JustASmallFryQuest",
            "LikeFarmerLikeChildQuest",
            "PodsNecklaceQuest",
            "PostMarriageQuestAnne1",
            "PostMarriageQuestCatherine1",
            "PostMarriageQuestClaude1",
            "PostMarriageQuestDarius1",
            "PostMarriageQuestDonovan1",
            "PostMarriageQuestIris1",
            "PostMarriageQuestJun1",
            "PostMarriageQuestKai1",
            "PostMarriageQuestKarish1",
            "PostMarriageQuestKitty1",
            "PostMarriageQuestLiam1",
            "PostMarriageQuestLucia1",
            "PostMarriageQuestLucius1",
            "PostMarriageQuestLynn1",
            "PostMarriageQuestMiyeon1",
            "PostMarriageQuestNathaniel1",
            "PostMarriageQuestShang1",
            "PostMarriageQuestVaan1",
            "PostMarriageQuestVivi1",
            "PostMarriageQuestWesley1",
            "PostMarriageQuestWornhardt1",
            "PostMarriageQuestXyla1",
            "PostMarriageQuestZaria1",
            "PrepareTheEventSquareQuest",
            "AnglersBetQuest",
            "ExplorersTreasureQuest",
            "FarmersOrdersQuest",
            "SmithsSkillsQuest",
            "WarriorsRequestQuest",
            "RaimiIntroQuest",
            "BagOfKittenNipQuest",
            "CaskOfElvenGrapeJuiceQuest",
            "DiscoloredFishingLureQuest",
            "EasternBangleQuest",
            "EmmasLostToyQuest",
            "GrimeCoveredMagicCharmQuest",
            "InjuredFighterQuest",
            "LostLetterQuest",
            "MedicalGloveQuest",
            "ReadingGlassesQuest",
            "SunHavenLibraryBookQuest",
            "WaterloggedHelmetQuest",
            "RexCapQuest",
            "RiddleMeThisQuest",
            "RonaldAndMarisBetQuest",
            "RSHCafeQuest",
            "RSHClothingStoreQuest",
            "RSHDogHouseQuest",
            "RSHExtraLargeTableQuest",
            "RSHLargeTableQuest",
            "RSHMediumTableQuest",
            "RSHOwlStatueQuest",
            "RSHSalonQuest",
            "RSHSmallTableQuest",
            "ShangsArrivalQuest",
            "ShangsLeavingQuest",
            "SolonNeedsCoalQuest",
            "SolonNeedsMeatQuest",
            "AnneMarriageGiftQuest1",
            "AnneMarriageGiftQuest2",
            "AnneMarriageGiftQuest3",
            "AnneMarriageGiftQuest4",
            "CatherineMarriageGiftQuest1",
            "CatherineMarriageGiftQuest2",
            "CatherineMarriageGiftQuest3",
            "CatherineMarriageGiftQuest4",
            "ClaudeMarriageGiftQuest1",
            "ClaudeMarriageGiftQuest2",
            "ClaudeMarriageGiftQuest3",
            "ClaudeMarriageGiftQuest4",
            "DariusMarriageGiftQuest1",
            "DariusMarriageGiftQuest2",
            "DariusMarriageGiftQuest3",
            "DariusMarriageGiftQuest4",
            "DonovanMarriageGiftQuest1",
            "DonovanMarriageGiftQuest2",
            "DonovanMarriageGiftQuest3",
            "DonovanMarriageGiftQuest4",
            "IrisMarriageGiftQuest1",
            "IrisMarriageGiftQuest2",
            "IrisMarriageGiftQuest3",
            "IrisMarriageGiftQuest4",
            "JunMarriageGiftrQuest1",
            "JunMarriageGiftrQuest2",
            "JunMarriageGiftrQuest3",
            "JunMarriageGiftrQuest4",
            "KaiMarriageGiftQuest1",
            "KaiMarriageGiftQuest2",
            "KaiMarriageGiftQuest3",
            "KaiMarriageGiftQuest4",
            "KarishMarriageGiftQuest1",
            "KarishMarriageGiftQuest2",
            "KarishMarriageGiftQuest3",
            "KarishMarriageGiftQuest4",
            "KittyMarriageGiftQuest1",
            "KittyMarriageGiftQuest2",
            "KittyMarriageGiftQuest3",
            "KittyMarriageGiftQuest4",
            "LiamMarriageGiftQuest1",
            "LiamMarriageGiftQuest2",
            "LiamMarriageGiftQuest3",
            "LiamMarriageGiftQuest4",
            "LuciaMarriageGiftQuest1",
            "LuciaMarriageGiftQuest2",
            "LuciaMarriageGiftQuest3",
            "LuciaMarriageGiftQuest4",
            "LuciusMarriageGiftQuest1",
            "LuciusMarriageGiftQuest2",
            "LuciusMarriageGiftQuest3",
            "LuciusMarriageGiftQuest4",
            "LynnMarriageGiftQuest1",
            "LynnMarriageGiftQuest2",
            "LynnMarriageGiftQuest3",
            "LynnMarriageGiftQuest4",
            "MiyeonMarriageGiftQuest1",
            "MiyeonMarriageGiftQuest2",
            "MiyeonMarriageGiftQuest3",
            "MiyeonMarriageGiftQuest4",
            "NathanielMarriageGiftQuest1",
            "NathanielMarriageGiftQuest2",
            "NathanielMarriageGiftQuest3",
            "NathanielMarriageGiftQuest4",
            "ShangMarriageGiftQuest1",
            "ShangMarriageGiftQuest2",
            "ShangMarriageGiftQuest3",
            "ShangMarriageGiftQuest4",
            "TemplateMarriageGiftquest",
            "VaanMarriageGiftQuest1",
            "VaanMarriageGiftQuest2",
            "VaanMarriageGiftQuest3",
            "VaanMarriageGiftQuest4",
            "ViviMarriageGiftQuest1",
            "ViviMarriageGiftQuest2",
            "ViviMarriageGiftQuest3",
            "ViviMarriageGiftQuest4",
            "WesleyMarriageGiftQuest1",
            "WesleyMarriageGiftQuest2",
            "WesleyMarriageGiftQuest3",
            "WesleyMarriageGiftQuest4",
            "WornhardtMarriageQuest1",
            "WornhardtMarriageQuest2",
            "WornhardtMarriageQuest3",
            "WornhardtMarriageQuest4",
            "XylaMarriageGiftQuest1",
            "XylaMarriageGiftQuest2",
            "XylaMarriageGiftQuest3",
            "XylaMarriageGiftQuest4",
            "ZariaMarriageGiftQuest1",
            "ZariaMarriageGiftQuest2",
            "ZariaMarriageGiftQuest3",
            "ZariaMarriageGiftQuest4",
            "IrissExperimentQuest",
            "MailLynnsMailQuest",
            "MermaidsRelicQuest",
            "PetersBoostQuest",
            "RaimisSnifflesQuest",
            "SoldiersDietQuest",
            "WhenLifeGivesYouLemonsQuest",
            "TopisStudiesQuest",
        };

        // MARRIAGE CHARACTERS
        private static string[] marriageCharacters = new string[] { "Anne", "Catherine", "Claude", "Darius", "Donovan", "Iris", "Jun", "Kai", "Karish", "Kitty", "Liam", "Lucia", "Lucius", "Lynn", "Miyeon", "Nathaniel", "Shang", "Vaan", "Vivi", "Wesley", "Wornhardt", "Xyla", "Zaria" };

        // ITEM ID's
        private static Dictionary<string, int> getAllIds()
        {
            try
            {
               return (Dictionary<string, int>)typeof(Database).GetField("ids", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Database.Instance);
            } catch
            {
                return new Dictionary<string, int>();
            }
        }

        private static HashSet<int> getValidIds()
        {
            try
            {
                return (HashSet<int>)typeof(Database).GetField("validIDs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Database.Instance);
            }
            catch
            {
                return new HashSet<int>();
            }
        }

        private static List<int> getAllIdsList(Dictionary<string,int> dict)
        {
            List<int> allItems = new List<int>();
            if (dict != null && dict.Count > 0)
            {
                allItems.AddRange(dict.Values);
            }
            return allItems;
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        private static int CurrentTimeSeconds()
        {
            return (int)((long)(CurrentTimeMillis() / 1000));
        }
        private static System.Random getRandom()
        {
            try
            {
                System.Random rand1 = new System.Random(CurrentTimeSeconds());
                return rand1;
            } catch(Exception ex) {
                return null;
            }
        }
   
        private static bool testTpPoint(string scene)
        {
            if (scene == "withergatefarm")
            {
                return false;
            }
            else if (scene == "throneroom")
            {
                return false;
            }
            else if (scene == "nelvari")
            {
                return false;
            }
            else if (scene == "wishingwell")
            {
                return false;
            }
            else if (scene.Contains("altar"))
            {
                return false;
            }
            else if (scene.Contains("hospital"))
            {
                return false;
            }
            else if (scene.Contains("sunhaven"))
            {
                return false;
            }
            else if (scene.Contains("nelvarifarm"))
            {
                return false;
            }
            else if (scene.Contains("nelvarimine"))
            {
                return false;
            }
            else if (scene.Contains("nelvarihome"))
            {
                return false;
            }
            else if (scene.Contains("castle"))
            {
                return false;
            }
            else if (scene.Contains("withergatehome"))
            {
                return false;
            }
            else if (scene.Contains("grandtree"))
            {
                return false;
            }
            else if (scene.Contains("taxi"))
            {
                return false;
            }
            else if (scene == "dynus")
            {
                return false;
            }
            else if (scene == "sewer")
            {
                return false;
            }
            else if (scene == "nivara")
            {
                return false;
            }
            else if (scene.Contains("barrack"))
            {
                return false;
            }
            else if (scene.Contains("elios"))
            {
                return false;
            }
            else if (scene.Contains("dungeon"))
            {
                return false;
            }
            else if (scene.Contains("store"))
            {
                return false;
            }
            else if (scene.Contains("beach"))
            {
                return false;
            }
            else if (scene == "home" || scene.Contains("sunhavenhome") || scene == "farm")
            {
                return false;
            }
            else if (scene == "back")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static bool setTpPoint(string name)
        {
            if (!tpPointsLoaded)
            {
                loadTpPoints();
                tpPointsLoaded = true;
            }

            try
            {
                name = name.ToLower();
                if (!testTpPoint(name))
                {
                    return false;
                }


                if (tpPoints.ContainsKey(name))
                {
                    return false;
                }

                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                StringVector2 lastLoc = new StringVector2(lastLocation);
                string lls = lastLoc.ToStringEx();
                lls = lastScene + ";" + lls;
                tpPoints.Add(name, lls);
                bool set = saveTpPoints();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }
        private static bool getTpPoint(string name)
        {
            if (!tpPointsLoaded)
            {
                loadTpPoints();
                tpPointsLoaded = true;
            }

            try
            {
                name = name.ToLower();
                if (!testTpPoint(name))
                {
                    return false;
                }

                if (!tpPoints.ContainsKey(name))
                {
                    return false;
                }
                if (!Regex.IsMatch(name, @"^[a-z]+$"))
                {
                    return false;
                }
                string ls = "";
                bool g = tpPoints.TryGetValue(name, out ls);
                if (!g)
                {
                    return false;
                }
                string[] gets = ls.Split(';');
                if (gets.Length == 2)
                {
                    string sceneName = gets[0];
                    StringVector2 xy = new StringVector2();
                    Vector2 position = xy.FromStringExVector2(gets[1]);
                    ScenePortalManager.Instance.ChangeScene(position, sceneName);
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }

        private static bool saveTpPoints()
        {
            try
            {
                string json = tpPoints.ToJson();
                System.IO.File.WriteAllText(jsonTpPointsPath, json);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }

        private static bool loadTpPoints()
        {
            if (!File.Exists(jsonTpPointsPath))
            {
                saveTpPoints();
                return true;
            }

            Dictionary<string, string> oldTpPoints = tpPoints.ToDictionary(entry => entry.Key,
                                                                           entry => entry.Value);
            Dictionary<string,string> newTpPoints = new Dictionary<string,string>();
            try
            {
                string fileJson = System.IO.File.ReadAllText(jsonTpPointsPath);
                newTpPoints = fileJson.FromJson<Dictionary<string, string>>();
            } catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            try
            {
                foreach (var k in newTpPoints)
                {
                    if (oldTpPoints.ContainsKey(k.Key))
                    {
                        oldTpPoints.Remove(k.Key);
                    }
                    oldTpPoints.Add(k.Key, k.Value);
                }
                foreach (var k in oldTpPoints)
                {
                    if (tpPoints.ContainsKey(k.Key))
                    {
                        tpPoints.Remove(k.Key);
                    }
                    tpPoints.Add(k.Key, k.Value);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            return true;
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

        private static Dictionary<string, int> allIds = null;
        private static HashSet<int> validIDs = null;
        private static List<int> allIdsList = null;
        private static bool initAllIds = false;
        private static bool initValidIDs = false;
        private static bool initAllIdsList = false;
        private static bool reinitAllIdsB = false;
        private static bool tpPointsLoaded = false;
        private static string jsonTpPointsPath = Path.Combine(Paths.ConfigPath, "commandextension_tp_points.json");
        private static Dictionary<string, string> tpPoints = new Dictionary<string, string>();
        private static bool ultraPolyGamySpoutsLoaded = false;
        private static string jsonUltraPolyGamySpoutsPath = Path.Combine(Paths.ConfigPath, "ultra_polygamy_spoutes.json");
        private static Dictionary<string, string> ultraPolyGamySpouts = new Dictionary<string, string>();
        private static System.Random random = getRandom();

        private static void reinitAllIds()
        {
            if (reinitAllIdsB)
            {
                return;
            }

            if (!initAllIds && (allIds == null || allIds.Count == 0))
            {
                allIds = getAllIds();
                initAllIds = allIds.Count > 0;
            }
            if (!initValidIDs && (validIDs == null || validIDs.Count == 0))
            {
                validIDs = getValidIds();
                initValidIDs = validIDs.Count > 0;
            }
            if (!initAllIdsList && (allIdsList == null || allIdsList.Count == 0))
            {
                allIdsList = getAllIdsList(allIds);
                initAllIdsList = allIdsList.Count > 0;
            }
            reinitAllIdsB = initAllIds && initValidIDs && initAllIdsList;
           
        }
        private static int getRangeRandomItemPos()
        {
            if (allIdsList.Count > 0)
            {
                return random.Next(0, allIdsList.Count);
            }
            else
            {
                return 0;
            }
        }
        private static int getRangeRandomItemCount(int pos)
        {
            if (pos + 5 < allIdsList.Count)
            {
                return random.Next(1, 5);
            } else
            {
                return 1;
            }
        }
        private static int getRangeRandomItemAmount()
        {
            return random.Next(1, 10);
        }
        private static Dictionary<string, int> moneyIds = new Dictionary<string, int>() { { "coins", 60000 }, { "orbs", 18010 }, { "tickets", 18011 } };
        private static Dictionary<string, int> xpIds = new Dictionary<string, int>() { { "combatexp", 60003 }, { "farmingexp", 60004 }, { "miningexp", 60006 }, { "explorationexp", 60005 }, { "fishingexp", 60008 } };
        private static Dictionary<string, int> bonusIds = new Dictionary<string, int>() { { "health", 60009 }, { "mana", 60007 } };
        private static Dictionary<string, Dictionary<string, int>> categorizedItems = new Dictionary<string, Dictionary<string, int>>()
            { { "Furniture Items", new Dictionary<string, int>() },  { "Craftable Items", new Dictionary<string, int>() },
            { "Useable Items", new Dictionary<string, int>() }, { "Monster Items", new Dictionary<string, int>() },
            { "Equipable Items", new Dictionary<string, int>() }, { "Quest Items", new Dictionary<string, int>() }, { "Other Items", new Dictionary<string, int>() } };
        private static Dictionary<string, Pet> petList = null;
        private static List<string> tpLocations = new List<string>()
            { "throneroom", "nelvari", "wishingwell", "altar", "hospital", "sunhaven", "sunhavenfarm/farm/home", "nelvarifarm", "nelvarimine", "nelvarihome",
                "withergatefarm", "castle", "withergatehome", "grandtree", "taxi", "dynus", "sewer", "nivara", "barracks", "elios", "dungeon", "store", "beach" };
        // COMMAND STATE VAR'S FOR FASTER ACCESS (inside patches)
        private static bool jumpOver = Commands[Array.FindIndex(Commands, command => command.Name == CmdJumper)].State == CommandState.Activated;
        private static bool cheatsOff = Commands[Array.FindIndex(Commands, command => command.Name == CmdCheats)].State == CommandState.Activated;
        private static bool noclip = Commands[Array.FindIndex(Commands, command => command.Name == CmdNoClip)].State == CommandState.Activated;
        private static bool printOnHover = Commands[Array.FindIndex(Commands, command => command.Name == CmdPrintHoverItem)].State == CommandState.Activated;
        private static bool infMana = Commands[Array.FindIndex(Commands, command => command.Name == CmdManaInf)].State == CommandState.Activated;
        private static bool infAirSkips = Commands[Array.FindIndex(Commands, command => command.Name == CmdDasher)].State == CommandState.Activated;
        private static bool appendItemDescWithId = Commands[Array.FindIndex(Commands, command => command.Name == CmdAppendItemDescWithId)].State == CommandState.Activated;
        private static bool yearFix = Commands[Array.FindIndex(Commands, command => command.Name == CmdFixYear)].State == CommandState.Activated;
        // ...
        private static float timeMultiplier = CommandParamDefaults.timeMultiplier;
        private static string playerNameForCommandsFirst;
        private static string playerNameForCommands;
        private static string lastPetName = "";
        private static string gap = "  -  ";
        private static int ranOnceOnPlayerSpawn = 0;
        private static string lastScene;
        private static Vector2 lastLocation;
        private static Color Red = new Color(255, 0, 0);
        private static Color Green = new Color(0, 255, 0);
        private static Color Yellow = new Color(240, 240, 0);
        public static ManualLogSource logger;
        private static ConfigEntry<int> RetentionDays;
        private static ConfigEntry<bool> DontDeleteBackups;
        public static string lastBackupSavePath = "";
        public static string lastSavePath = "";
        #endregion

        #region Awake() | Update() | OnGui()   -   BASE UNITY OBJECT METHODES
        private void Awake()
        {
            CommandExtension.logger = this.Logger;
            CommandExtension.RetentionDays = this.Config.Bind<int>("General.SaveBackups", "Days kept", 30, "Number of in-game days for which backups should be kept");
            CommandExtension.DontDeleteBackups = this.Config.Bind<bool>("General.SaveBackups", "Do not delete backups", true,  "Do not delete backups");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
            Array.Sort(Commands, (x, y) => x.Name.CompareTo(y.Name));
        }
        private void Update()
        {
        }
        private void OnGUI()
        {
        }
        #endregion

        #region Core
        // get the player for singleplayer/multiplayer
        #region Get Wish.Player for command execution
        public static Player GetPlayerForCommand()
        {
            Player[] players = FindObjectsOfType<Player>();
            foreach (Player player in players)
            {
                if (players.Length == 1 || player.GetComponent<NetworkGamePlayer>().playerName == playerNameForCommands)
                    return player;
            }
            return null;
        }
        #endregion

        // no chat bubble for commands
        #region CheckIfCommand DisplayChatBubble
        private static bool CheckIfCommandDisplayChatBubble(string mayCommand)
        {
            mayCommand = mayCommand.ToLower();
            if (mayCommand[0] != '!' || GetPlayerForCommand() == null && !mayCommand.Contains(CmdName))
                return false;
            foreach (var command in Commands)
            {
                if (mayCommand.Contains(command.Name))
                    return true;
            }
            return false;
        }
        #endregion

        // check and execute methode
        #region CheckIfCommand SendChatMessage
        public static bool CheckIfCommandSendChatMessage(string mayCommand)
        {
            String originalMayCommand = mayCommand;
            mayCommand = mayCommand.ToLower();
            if ((mayCommand[0] != '!' || GetPlayerForCommand() == null) && !mayCommand.Contains(CmdName))
                return false;

            string[] mayCommandParam = mayCommand.Split(' ');
            string[] originalMayCommandParam = originalMayCommand.Split(' ');

            if (mayCommandParam.Length > 0 && originalMayCommandParam.Length > 0)
            {
                originalMayCommandParam[0] = mayCommandParam[0];
            }

            switch (mayCommandParam[0])
            {
                case CmdHelp:
                    return CommandFunction_Help();

                case CmdState:
                    return CommandFunction_Help(true);

                case CmdFeedbackDisabled:
                    return CommandFunction_ToggleFeedback();

                case CmdMineReset:
                    return CommandFunction_ResetMines();

                case CmdPause:
                    return CommandFunction_Pause();

                case CmdCustomDaySpeed:
                    return CommandFunction_CustomDaySpeed(mayCommand);

                case CmdMoney:
                    return CommandFunction_AddMoney(mayCommand);

                case CmdCoins:
                    return CommandFunction_AddMoney(mayCommand);

                case CmdOrbs:
                    return CommandFunction_AddOrbs(mayCommand);

                case CmdTickets:
                    return CommandFunction_AddTickets(mayCommand);

                case CmdSetDate:
                    return CommandFunction_ChangeDate(mayCommand);

                case CmdWeather:
                    return CommandFunction_ChangeWeather(mayCommand);

                case CmdDevKit:
                    return CommandFunction_GiveDevItems();

                case CmdJumper:
                    return CommandFunction_Jumper();

                case CmdCheats:
                    return CommandFunction_ToggleCheats();

                case CmdSleep:
                    return CommandFunction_Sleep();

                case CmdDasher:
                    return CommandFunction_InfiniteAirSkips();

                case CmdManaFill:
                    return CommandFunction_ManaFill();

                case CmdManaInf:
                    return CommandFunction_InfiniteMana();

                case CmdHealthFill:
                    return CommandFunction_HealthFill();

                case CmdNoHit:
                    return CommandFunction_NoHit();

                case CmdMineOverfill:
                    return CommandFunction_OverfillMines();

                case CmdMineClear:
                    return CommandFunction_ClearMines();

                case CmdNoClip:
                    return CommandFunction_NoClip();

                case CmdPrintHoverItem:
                    return CommandFunction_PrintItemIdOnHover();

                case CmdName:
                    return CommandFunction_SetName(mayCommand);

                case CmdGive:
                    return CommandFunction_GiveItemByIdOrName(mayCommandParam);

                case CmdShowItems:
                    return CommandFunction_ShowItemByName(mayCommandParam);

                case CmdPrintItemIds:
                    return CommandFunction_PrintItemIds(mayCommandParam);

                case CmdAutoFillMuseum:
                    return CommandFunction_AutoFillMuseum();

                case CmdCheatFillMuseum:
                    return CommandFunction_CheatFillMuseum();

                case CmdUI:
                    return CommandFunction_ToggleUI(mayCommand);

                case CmdTeleport:
                    return CommandFunction_TeleportToScene(mayCommandParam);

                case CmdTeleportLocations:
                    return CommandFunction_TeleportLocations();

                case CmdDespawnPet:
                    return CommandFunction_DespawnPet();

                case CmdSpawnPet:
                    return CommandFunction_SpawnPet(mayCommandParam);

                case CmdPetList:
                    return CommandFunction_GetPetList();

                case CmdAppendItemDescWithId:
                    return CommandFunction_ShowID();

                case CmdRelationship:
                    return CommandFunction_Relationship(originalMayCommandParam);

                case CmdUnMarry:
                    return CommandFunction_UnMarry(originalMayCommandParam);

                case CmdMarryNpc:
                    return CommandFunction_MarryNPC(originalMayCommandParam);

                case CmdSetSeason:
                    return CommandFunction_SetSeason(mayCommandParam);

                case CmdFixYear:
                    return CommandFunction_ToggleYearFix();

                case CmdIncDecYear:
                    return CommandFunction_IncDecYear(mayCommand);

                case CmdQuestsList:
                    return CommandFunction_QuestsList();

                case CmdQuestLog:
                    return CommandFunction_QuestsLog();

                case CmdQuestAdd:
                    return CommandFunction_QuestAdd(originalMayCommandParam);

                case CmdQuestRemove:
                    return CommandFunction_QuestRemove(originalMayCommandParam);

                case CmdSaveGame:
                    return CommandFunction_SaveGame();

                case CmdSleepSave:
                    return CommandFunction_SleepSave();

                case CmdBackupSaveGame:
                    return CommandFunction_BackupSaveGame();

                case CmdBackupSleepSave:
                    return CommandFunction_BackupSleepSave();

                case CmdChristmas:
                    return CommandFunction_Christmas();

                case CmdSetTp:
                    return CommandFunction_SetTp(mayCommandParam);

                case CmdGetTp:
                    return CommandFunction_GetTp(mayCommandParam);

                case CmdListTp:
                    return CommandFunction_ListTp();

                case CmdMainSp:
                    return CommandFunction_MainSp(originalMayCommandParam);

                // no valid command found
                default:
                    return false;
            }
        }
        #endregion

        private static void sortItemAll(KeyValuePair<string, int> item, ItemData itemData)
        {
            ItemCategory itemCategory = itemData.category;
            switch (itemCategory)
            {
                case ItemCategory.Furniture:
                    categorizedItems["Furniture Items"].Add(item.Key, item.Value);
                    break;

                case ItemCategory.Equip:
                    categorizedItems["Equipable Items"].Add(item.Key, item.Value);
                    break;

                case ItemCategory.Quest:
                    categorizedItems["Quest Items"].Add(item.Key, item.Value);
                    break;

                case ItemCategory.Craftable:
                    categorizedItems["Craftable Items"].Add(item.Key, item.Value);
                    break;

                case ItemCategory.Monster:
                    categorizedItems["Monster Items"].Add(item.Key, item.Value);
                    break;

                case ItemCategory.Use:
                    categorizedItems["Useable Items"].Add(item.Key, item.Value);
                    break;

                default:
                    categorizedItems["Other Items"].Add(item.Key, item.Value);
                    break;

            }
        }

        private static void sortItemFailed()
        {

        }

        // Categorize all items
        #region Categorize 'ItemDatabaseWrapper.ItemDatabase.ids' into 'categorizedItems'
        private static bool CategorizeItemList()
        {
            if (allIds == null || allIds.Count < 1)
                   return false;
            Action<ItemData> itemDataFunc = null;
            Action itemFailed = () => sortItemFailed();
            foreach (var item in allIds)
            {
                itemDataFunc = (i) => sortItemAll(item, i);
                Database.GetData<ItemData>(item.Value, itemDataFunc, itemFailed);
            }
            return true;
        }
        #endregion

        // command methodes
        #region CommandFunction - Methode's
        // PRINT FUNCTION **
        private static void CommandFunction_PrintToChat(string text)
        {
            if (Commands.First(command => command.Name == CmdFeedbackDisabled).State == CommandState.Deactivated)
                QuantumConsole.Instance.LogPlayerText(text);
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        // HELP *
        private static bool CommandFunction_Help(bool status = false)
        {
            if (!status)
            {
                CommandFunction_PrintToChat("[HELP]".ColorText(Color.black));
                foreach (Command command in Commands)
                {
                    if (command.State == CommandState.Activated)
                        CommandFunction_PrintToChat($"{command.Name.ColorText(Green)}{gap.ColorText(Color.black)}{command.Description.ColorText(Yellow)}");
                    if (command.State == CommandState.Deactivated)
                        CommandFunction_PrintToChat($"{command.Name.ColorText(Red)}{gap.ColorText(Color.black)}{command.Description.ColorText(Yellow)}");
                    if (command.State == CommandState.None)
                        CommandFunction_PrintToChat($"{command.Name.ColorText(Yellow)}{gap.ColorText(Color.black)}{command.Description.ColorText(Yellow)}");
                }
            }
            else
            {
                CommandFunction_PrintToChat("[STATE]".ColorText(Color.black));
                foreach (Command command in Commands)
                {
                    if (command.State == CommandState.Activated)
                        CommandFunction_PrintToChat($"{command.Name.ColorText(Yellow)}{gap.ColorText(Color.black)}{(command.State.ToString().ColorText(Green))}");
                }
            }
            return true;
        }

        private static void printItemIds(char s)
        {
            void sortItemFurniture(KeyValuePair<string, int> item, ItemData itemData)
            {
                ItemCategory itemCategory = itemData.category;
                switch (itemCategory)
                {
                    case ItemCategory.Furniture:
                        CommandFunction_PrintToChat($"{item.Key} : {item.Value}");
                        break;
                }
            };

            void sortItemQuest(KeyValuePair<string,int> item, ItemData itemData)
            {
                ItemCategory itemCategory = itemData.category;
                switch (itemCategory)
                {
                    case ItemCategory.Quest:
                        CommandFunction_PrintToChat($"{item.Key} : {item.Value}");
                        break;

                }
            };

            void sortItemFailed()
            {

            };

            if (allIds == null || allIds.Count < 1)
               return;
            
            Action<ItemData> itemDataFunc = null;
            Action itemFailed = () => sortItemFailed();
            switch (s)
            {
                case 'f':
                    foreach (var item in allIds)
                    {
                        itemDataFunc = (i) => sortItemFurniture(item, i);
                        Database.GetData<ItemData>(item.Value, itemDataFunc, itemFailed);
                    }
                break;
                case 'q':
                    foreach (var item in allIds)
                    {
                        itemDataFunc = (i) => sortItemQuest(item, i);
                        Database.GetData<ItemData>(item.Value, itemDataFunc, itemFailed);
                    }
                break;
            }
        }
        // PRINT SPECIAL ITEMS
        private static bool CommandFunction_PrintItemIds(string[] mayCommandParam)
        {
            reinitAllIds();
            switch ((mayCommandParam.Length >= 2) ? mayCommandParam[1][0] : '-')
            {
                case 'x':
                    CommandFunction_PrintToChat("[XP-ITEM-IDs]".ColorText(Color.black));
                    foreach (KeyValuePair<string, int> id in xpIds)
                        CommandFunction_PrintToChat($"{id.Key} : {id.Value}");
                    break;
                case 'm':
                    CommandFunction_PrintToChat("[MONEY-ITEM-IDs]".ColorText(Color.black));
                    foreach (KeyValuePair<string, int> id in moneyIds)
                        CommandFunction_PrintToChat($"{id.Key} : {id.Value}");
                    break;
                case 'a':
                    if (CategorizeItemList())
                    {
                        string file = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/")) + "\\itemIDs(CommandExtension).txt";
                        if (!File.Exists(file))
                        {
                            using (File.Create(file)) { }
                        }
                        bool isEmpty = true;
                        File.WriteAllText(file, "");
                        string overviewLine = new string('#', 80);
                        foreach (var category in categorizedItems)
                        {
                            if (category.Value.Count >= 1)
                            {
                                if (isEmpty)
                                {
                                    File.AppendAllText(file, $"[{category.Key}]\n");
                                    isEmpty = false;
                                }
                                else
                                    File.AppendAllText(file, $"\n\n\n{overviewLine}\n[{category.Key}]\n");
                                foreach (var item in category.Value.OrderBy(i => i.Key))
                                {
                                    File.AppendAllText(file, $"{item.Key} : {item.Value}\n");
                                }
                                File.AppendAllText(file, "");
                            }
                        }
                        CommandFunction_PrintToChat("ID list created inside your Sun Haven folder:".ColorText(Color.green));
                        CommandFunction_PrintToChat(file.ColorText(Color.white));
                    }
                    else
                        CommandFunction_PrintToChat("ERROR: ".ColorText(Red) + "ItemDatabaseWrapper.ItemDatabase.ids".ColorText(Color.white) + " is empty!".ColorText(Red));
                    break;
                case 'b':
                    CommandFunction_PrintToChat("[BONUS-ITEM-IDs]".ColorText(Color.black));
                    foreach (KeyValuePair<string, int> id in bonusIds)
                        CommandFunction_PrintToChat($"{id.Key} : {id.Value}");
                    break;
                case 'f':
                    CommandFunction_PrintToChat("[FURNITURE-ITEM-IDs]".ColorText(Color.black));
                    printItemIds('f');
                    break;
                case 'q':
                    CommandFunction_PrintToChat("[QUEST-ITEM-IDs]".ColorText(Color.black));
                    printItemIds('q');
                    break;
                default:
                    CommandFunction_PrintToChat(CmdPrintItemIds + " [xp|money|all|bonus|furniture|quest]".ColorText(Red));
                    return true;
            }
            return true;
        }
        // RESET MINES
        private static bool CommandFunction_ResetMines()
        {
            if (FindObjectOfType<MineGenerator2>() != null)
            {
                typeof(MineGenerator2).GetMethod("ResetMines", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(MineGenerator2.Instance, new object[] { (ushort)0 });
                CommandFunction_PrintToChat($"Mine {"Reseted".ColorText(Green)}!".ColorText(Yellow));
            }
            else
                CommandFunction_PrintToChat(("Must be inside a Mine!").ColorText(Red));
            return true;
        }
        // CLEAR MINES
        private static bool CommandFunction_ClearMines()
        {
            if (FindObjectOfType<MineGenerator2>() != null)
            {
                typeof(MineGenerator2).GetMethod("ClearMine", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(MineGenerator2.Instance, null);
                CommandFunction_PrintToChat($"Mine {"Cleared".ColorText(Green)}!".ColorText(Yellow));
            }
            else
                CommandFunction_PrintToChat(("Must be inside a Mine!").ColorText(Red));
            return true;
        }
        // OVERFILL MINES
        private static bool CommandFunction_OverfillMines()
        {
            if (FindObjectOfType<MineGenerator2>() != null)
            {
                for (int i = 0; i < 30; i++)
                {
                    typeof(MineGenerator2).GetMethod("GenerateRocks", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(MineGenerator2.Instance, null);
                }
                CommandFunction_PrintToChat($"Mine {"Overfilled".ColorText(Green)}!".ColorText(Yellow));
            }
            else
                CommandFunction_PrintToChat(("Must be inside a Mine!").ColorText(Red));
            return true;
        }
        // ADD MONEY/COINS
        private static bool CommandFunction_AddMoney(string mayCommand)
        {
            if (!int.TryParse(Regex.Match(mayCommand, @"\d+").Value, out int moneyAmount))
            {
                CommandFunction_PrintToChat("Something wen't wrong..".ColorText(Red));
                CommandFunction_PrintToChat("Try '!money 500' or '!coins 500'".ColorText(Red));
                return true;
            }
            if (mayCommand.Contains("-"))
            {
                GetPlayerForCommand().AddMoney(-moneyAmount, true, true, true);
                CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} paid {moneyAmount.ToString().ColorText(Color.white)} Coins!".ColorText(Yellow));
            }
            else
            {
                GetPlayerForCommand().AddMoney(moneyAmount, true, true, true);
                CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} got {moneyAmount.ToString().ColorText(Color.white)} Coins!".ColorText(Yellow));
            }
            return true;
        }
        // ADD MONEY
        private static bool CommandFunction_AddOrbs(string mayCommand)
        {
            if (!int.TryParse(Regex.Match(mayCommand, @"\d+").Value, out int moneyAmount))
            {
                CommandFunction_PrintToChat("Something wen't wrong..".ColorText(Red));
                CommandFunction_PrintToChat("Try '!orbs 500'".ColorText(Red));
                return true;
            }
            if (mayCommand.Contains("-"))
            {
                GetPlayerForCommand().AddOrbs(-moneyAmount);
                CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} paid {moneyAmount.ToString().ColorText(Color.white)} Orbs!".ColorText(Yellow));
            }
            else
            {
                GetPlayerForCommand().AddOrbs(moneyAmount);
                CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} got {moneyAmount.ToString().ColorText(Color.white)} Orbs!".ColorText(Yellow));
            }
            return true;
        }
        // ADD MONEY
        private static bool CommandFunction_AddTickets(string mayCommand)
        {
            if (!int.TryParse(Regex.Match(mayCommand, @"\d+").Value, out int moneyAmount))
            {
                CommandFunction_PrintToChat("Something wen't wrong..".ColorText(Red));
                CommandFunction_PrintToChat("Try '!tickets 500'".ColorText(Red));
                return true;
            }
            if (mayCommand.Contains("-"))
            {
                GetPlayerForCommand().AddTickets(-moneyAmount);
                CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} paid {moneyAmount.ToString().ColorText(Color.white)} Tickets!".ColorText(Yellow));
            }
            else
            {
                GetPlayerForCommand().AddTickets(moneyAmount);
                CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} got {moneyAmount.ToString().ColorText(Color.white)} Tickets!".ColorText(Yellow));
            }
            return true;
        }
        // SET NAME
        private static bool CommandFunction_SetName(string mayCommand)
        {
            string[] mayCommandParam = mayCommand.Split(' ');
            if (mayCommand.Length <= CmdName.Length + 1)
            {
                playerNameForCommands = playerNameForCommandsFirst;
                CommandFunction_PrintToChat($"Command target name {"reseted".ColorText(Green)} to {playerNameForCommandsFirst.ColorText(Color.magenta)}!".ColorText(Yellow));
            }
            else if (mayCommandParam.Length >= 2)
            {
                playerNameForCommands = mayCommandParam[1];
                CommandFunction_PrintToChat($"Command target name changed to {mayCommandParam[1].ColorText(Color.magenta)}!".ColorText(Color.yellow));
            }
            return true;
        }
        // CHANGE DATE (only hour and day!)
        public static bool CommandFunction_ChangeDate(string mayCommand)
        {
            string[] mayCommandParam = mayCommand.ToLower().Split(' ');
            if (mayCommandParam.Length == 3)
            {
                var Date = DayCycle.Instance;
                if (int.TryParse(mayCommandParam[2], out int dateValue))
                {
                    switch (mayCommandParam[1][0])
                    {
                        case 'd':
                            if (dateValue <= 0 || dateValue > 28)
                            { CommandFunction_PrintToChat($"day {"1-28".ColorText(Color.white)} are allowed".ColorText(Red)); return true; }
                            Date.Time = new DateTime(Date.Time.Year, Date.Time.Month, dateValue, Date.Time.Hour + 1, Date.Time.Minute, Date.Time.Second, Date.Time.Millisecond);
                            typeof(DayCycle).GetMethod("SetInitialTime", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(DayCycle.Instance, null);
                            CommandFunction_PrintToChat($"{"Day".ColorText(Green)} set to {dateValue.ToString().ColorText(Color.white)}!".ColorText(Yellow));
                            break;
                        case 'h':
                            if (dateValue < 6 || dateValue > 23) // 6-23 
                            { CommandFunction_PrintToChat($"hour {"6-23".ColorText(Color.white)} are allowed".ColorText(Red)); return true; }
                            Date.Time = new DateTime(Date.Time.Year, Date.Time.Month, Date.Time.Day, dateValue + 1, Date.Time.Minute, Date.Time.Second, Date.Time.Millisecond);
                            CommandFunction_PrintToChat($"{"Hour".ColorText(Green)} set to {dateValue.ToString().ColorText(Color.white)}!".ColorText(Yellow));
                            break;
                    }
                }
            }
            return true;
        }
        // CHANGE WEATHER
        public static bool CommandFunction_ChangeWeather(string mayCommand)
        {
            string[] mayCommandParam = mayCommand.ToLower().Split(' ');
            if (mayCommandParam.Length >= 2)
            {
                var Date = DayCycle.Instance;
                switch (mayCommandParam[1][0])
                {
                    case 'r': // rain toggle
                        Date.SetToRaining(Date.Raining ? false : true);
                        CommandFunction_PrintToChat($"{"Raining".ColorText(Green)} turned {(!Date.Raining ? "Off".ColorText(Red) : "On".ColorText(Green))}!".ColorText(Yellow));
                        break;
                    case 'h': // heatwave toggle
                        Date.SetToHeatWave(Date.Heatwave ? false : true);
                        CommandFunction_PrintToChat($"{"Heatwave".ColorText(Green)} turned {(!Date.Heatwave ? "Off".ColorText(Red) : "On".ColorText(Green))}!".ColorText(Yellow));
                        break;
                    case 'c': // clear both
                        Date.SetToHeatWave(false);
                        Date.SetToRaining(false);
                        CommandFunction_PrintToChat($"{"Sunny".ColorText(Green)} weather turned {"On".ColorText(Green)}!".ColorText(Yellow));
                        break;
                }
            }
            return true;
        }

        private static void giveItemMessage(int itemAmount, ItemData itemData)
        {
            int itemId = itemData.id;
            string name = itemData.name.ToLower();
            string nameColored = $"{name} ( ID: {itemId.ToString()} )";
            CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} got {itemAmount.ToString().ColorText(Color.white)} * {nameColored.ColorText(Color.white)}!".ColorText(Yellow));
        }

        private static void giveItemMessageFailed()
        {

        }
        // GIVE ITEM BY ID/NAME 
        private static bool CommandFunction_GiveItemByIdOrName(string[] mayCommandParam)
        {
            if (mayCommandParam.Length >= 2)
            {
                int itemId = 0;
                int stringItemId = 0;
                itemId = int.TryParse(mayCommandParam[1], out itemId) ? itemId : 0;
				string stringItem = itemId == 0 ? mayCommandParam[1].ToLower() : "";
				stringItemId = itemId == 0 ? (Database.GetID(stringItem)) : 0;
				int itemAmount = ((mayCommandParam.Length >= 3 && int.TryParse(mayCommandParam[2], out itemAmount)) ? itemAmount : 1);
                Action<ItemData> itemDataFunc = (i) => giveItemMessage(itemAmount, i);
                Action itemFailed = () => giveItemMessageFailed();

                if (itemId > 0)
                {
                    if (Database.ValidID(itemId))
                    {
                       GetPlayerForCommand().Inventory.AddItem(itemId, itemAmount, 0, true, true);
                       Database.GetData<ItemData>(itemId, itemDataFunc, itemFailed);
                    }
					else
					{
						CommandFunction_PrintToChat($"no item with id: {itemId.ToString().ColorText(Color.white)} found!".ColorText(Red));
					}
                }
                else if (stringItemId > 0) 
                {
					if (Database.ValidID(stringItemId))
                    {
						GetPlayerForCommand().Inventory.AddItem(stringItemId, itemAmount, 0, true, true);
                        Database.GetData<ItemData>(stringItemId, itemDataFunc, itemFailed);
                    }
                    else
					{
                        CommandFunction_PrintToChat($"invalid itemId: {stringItemId.ToString().ColorText(Color.white)}!".ColorText(Red));
						CommandFunction_PrintToChat($"no item name contains {stringItem.ColorText(Color.white)}!".ColorText(Red));
					}
                    return true;
                }
				else
				{
					CommandFunction_PrintToChat($"invalid itemId: {stringItemId.ToString().ColorText(Color.white)}!".ColorText(Red));
				}
            }
            else
			{
                CommandFunction_PrintToChat($"wrong use of !give".ColorText(Red));
			}
            return true;
        }
        // GIVE ITEM BY ID/NAME 
        private static bool CommandFunction_ShowItemByName(string[] mayCommandParam)
        {
            reinitAllIds();
            if (mayCommandParam.Length >= 2)
            {
                List<string> items = new List<string>();
                foreach (KeyValuePair<string, int> id in allIds)
                {
                    if (id.Key.ToLower().Contains(mayCommandParam[1]))
                        items.Add(id.Key.ColorText(Color.white) + " : ".ColorText(Color.black) + id.Value.ToString().ColorText(Color.white));
                }
                if (items.Count >= 1)
                {
                    CommandFunction_PrintToChat("[FOUND ITEMS]".ColorText(Color.black));
                    foreach (string �tem in items)
                        CommandFunction_PrintToChat(�tem);
                }
            }
            else 
			{
				CommandFunction_PrintToChat($"wrong use of !showitembyname".ColorText(Red));
			}	
            return true;
        }
        // GIVE DEV ITEMS
        private static bool CommandFunction_GiveDevItems()
        {
            foreach (int item in new int[] { 30003, 30004, 30005, 30006, 30007, 30008 })
                GetPlayerForCommand().Inventory.AddItem(item, 1, 0, true);
            CommandFunction_PrintToChat($"{playerNameForCommands.ColorText(Color.magenta)} got a {"DevKit".ColorText(Color.white)}".ColorText(Yellow));
            return true;
        }
        // INFINITE AIR-SKIPS
        private static bool CommandFunction_InfiniteAirSkips()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdDasher);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = infAirSkips = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // SHOW ID
        private static bool CommandFunction_ShowID()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdAppendItemDescWithId);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = appendItemDescWithId = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // FILL MANA
        private static bool CommandFunction_ManaFill()
        {
            var player = GetPlayerForCommand();
            player.AddMana(player.MaxMana, 1f);
            CommandFunction_PrintToChat(playerNameForCommands.ColorText(Color.magenta) + "'s Mana Refilled".ColorText(Yellow));
            return true;
        }
        // INFINITE MANA
        private static bool CommandFunction_InfiniteMana()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdManaInf);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = infMana = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // FILL HEALTH
        private static bool CommandFunction_HealthFill()
        {
            var player = GetPlayerForCommand();
            player.Heal(player.MaxMana, true, 1f);
            CommandFunction_PrintToChat(playerNameForCommands.ColorText(Color.magenta) + "'s Health Refilled".ColorText(Yellow));
            return true;
        }
        // SLEEP
        private static bool CommandFunction_Sleep()
        {
            GetPlayerForCommand().SkipSleep();
            CommandFunction_PrintToChat($"{"Slept".ColorText(Green)} once! Another Day is a Good Day!".ColorText(Yellow));
            return true;
        }

        // SLEEP SAVE
        private static bool CommandFunction_SleepSave()
        {
            GetPlayerForCommand().SkipSleep();
            CommandFunction_PrintToChat($"{"Slept".ColorText(Green)} once! Another Day is a Good Day!".ColorText(Yellow));
            SingletonBehaviour<GameSave>.Instance.SaveGame(false);
            SingletonBehaviour<GameSave>.Instance.WriteCharacterToFile(false, false);
            CommandFunction_PrintToChat($"Game saved to: {CommandExtension.lastSavePath.ColorText(Color.white)}!".ColorText(Green));
            return true;
        }

        // BACKUP SLEEP SAVE
        private static bool CommandFunction_BackupSleepSave()
        {
            GetPlayerForCommand().SkipSleep();
            CommandFunction_PrintToChat($"{"Slept".ColorText(Green)} once! Another Day is a Good Day!".ColorText(Yellow));
            SingletonBehaviour<GameSave>.Instance.SaveGame(false);
            SingletonBehaviour<GameSave>.Instance.WriteCharacterToFile(true, false);
            CommandFunction_PrintToChat($"Game backup saved to: {CommandExtension.lastBackupSavePath.ColorText(Color.white)}!".ColorText(Green));
            return true;
        }

        // PRINT ID ON HOVER
        private static bool CommandFunction_PrintItemIdOnHover()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdPrintHoverItem);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            printOnHover = flag;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // TOGGLE COMMAND FEEDBACK
        private static bool CommandFunction_ToggleFeedback()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdFeedbackDisabled);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // PAUSE
        private static bool CommandFunction_Pause()
        {
            // get Command values
            int i = Array.FindIndex(Commands, command => command.Name == CmdPause);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // SLOW TIME
        private static bool CommandFunction_CustomDaySpeed(string mayCommand)
        {
            string[] mayCommandParam = mayCommand.Split(' ');
            int i = Array.FindIndex(Commands, command => command.Name == CmdCustomDaySpeed);
            if (mayCommandParam.Length >= 2)
            {
                if (int.TryParse(mayCommandParam[1], out int value))
                {
                    Commands[i].State = CommandState.Activated;
                    timeMultiplier = (float)System.Math.Round(20f / value, 4);
                    CommandFunction_PrintToChat($"Custom Dayspeed {"Activated".ColorText(Green)} and {"changed".ColorText(Green)}! multiplier: {timeMultiplier.ToString().ColorText(Color.white)}".ColorText(Yellow));
                    return true;
                }
                else if (mayCommandParam[1].Contains("r") || mayCommandParam[1].Contains("d")) // r = reset | d = default
                {
                    timeMultiplier = CommandParamDefaults.timeMultiplier;
                    Commands[i].State = CommandState.Activated;
                    CommandFunction_PrintToChat($"Custom Dayspeed {"Activated".ColorText(Green)} and {"reseted".ColorText(Green)}! multiplier: {timeMultiplier.ToString().ColorText(Color.white)}".ColorText(Yellow));
                    return true;
                }
            }
            else
                Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // UI ON/OFF
        private static bool CommandFunction_ToggleUI(string mayCommand)
        {
            string[] mayCommandParam = mayCommand.Split(' ');
            string error = $"try '{"!ui [on/off]".ColorText(Color.white)}'".ColorText(Red);
            if (mayCommandParam.Length >= 2)
            {
                bool flag = true;
                if (mayCommandParam[1].Contains("on"))
                    flag = true;
                else if (mayCommandParam[1].Contains("of"))
                    flag = false;
                else
                    CommandFunction_PrintToChat(error);
                #region gameObject.SetActive (from QuantumConsoleManager commands)
                GameObject gameObject = Utilities.FindObject(GameObject.Find("Player"), "ActionBar");
                if (gameObject != null)
                {
                    gameObject.SetActive(flag);
                }
                GameObject gameObject2 = Utilities.FindObject(GameObject.Find("Player(Clone)"), "ActionBar");
                if (gameObject2 != null)
                {
                    gameObject2.SetActive(flag);
                }
                GameObject gameObject3 = Utilities.FindObject(GameObject.Find("Player"), "ExpBars");
                if (gameObject3 != null)
                {
                    gameObject3.SetActive(flag);
                }
                GameObject gameObject4 = Utilities.FindObject(GameObject.Find("Player(Clone)"), "ExpBars");
                if (gameObject4 != null)
                {
                    gameObject4.SetActive(flag);
                }
                GameObject gameObject5 = Utilities.FindObject(GameObject.Find("Player"), "QuestTracking");
                if (gameObject5 != null)
                {
                    gameObject5.SetActive(flag);
                }
                GameObject gameObject6 = Utilities.FindObject(GameObject.Find("Player(Clone)"), "QuestTracking");
                if (gameObject6 != null)
                {
                    gameObject6.SetActive(flag);
                }
                GameObject gameObject7 = Utilities.FindObject(GameObject.Find("Player"), "QuestTracker");
                if (gameObject7 != null)
                {
                    gameObject7.SetActive(flag);
                }
                GameObject gameObject8 = Utilities.FindObject(GameObject.Find("Player(Clone)"), "QuestTracker");
                if (gameObject8 != null)
                {
                    gameObject8.SetActive(flag);
                }
                GameObject gameObject9 = Utilities.FindObject(GameObject.Find("Player"), "HelpNotifications");
                if (gameObject9 != null)
                {
                    gameObject9.SetActive(flag);
                }
                GameObject gameObject10 = Utilities.FindObject(GameObject.Find("Player(Clone)"), "HelpNotifications");
                if (gameObject10 != null)
                {
                    gameObject10.SetActive(flag);
                }
                GameObject gameObject11 = Utilities.FindObject(GameObject.Find("Player"), "NotificationStack");
                if (gameObject11 != null)
                {
                    gameObject11.SetActive(flag);
                }
                GameObject gameObject12 = Utilities.FindObject(GameObject.Find("Player(Clone)"), "NotificationStack");
                if (gameObject12 != null)
                {
                    gameObject12.SetActive(flag);
                }
                GameObject gameObject13 = Utilities.FindObject(GameObject.Find("Manager"), "UI");
                if (gameObject13 != null)
                {
                    gameObject13.SetActive(flag);
                }
                GameObject gameObject14 = GameObject.Find("QuestTrackerVisibilityToggle");
                if (gameObject14 != null)
                {
                    gameObject14.SetActive(flag);
                }
                #endregion
                CommandFunction_PrintToChat("UI now ".ColorText(Yellow) + (flag ? "VISIBLE".ColorText(Green) : "HIDDEN".ColorText(Green)));
            }
            else
                CommandFunction_PrintToChat(error);
            return true;
        }
        // JUMPER
        private static bool CommandFunction_Jumper()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdJumper);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            jumpOver = flag;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // NOCLIP
        private static bool CommandFunction_NoClip()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdNoClip);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            GetPlayerForCommand().rigidbody.bodyType = Commands[i].State == CommandState.Activated ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
            bool flag = Commands[i].State == CommandState.Activated;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // NO HIT
        private static bool CommandFunction_NoHit()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdNoHit);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            GetPlayerForCommand().Invincible = flag;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // AUTO-FILL MUSEUM
        private static bool CommandFunction_AutoFillMuseum()
        {
            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here");
            int i = Array.FindIndex(Commands, command => command.Name == CmdAutoFillMuseum);
            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here 2");
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here 3");
            bool flag = Commands[i].State == CommandState.Activated;
            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here 4");
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here 5");
            return true;
        }
        // CHEAT-FILL MUSEUM
        private static bool CommandFunction_CheatFillMuseum()
        {
            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here 6");
            int i = Array.FindIndex(Commands, command => command.Name == CmdCheatFillMuseum);
            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here 7");
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here 8");
            bool flag = Commands[i].State == CommandState.Activated;
            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here 9");
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here 10");
            return true;
        }
        // TELEPORT
        private static bool CommandFunction_TeleportToScene(string[] mayCmdParam)
        {
            if (mayCmdParam.Length <= 1)
                return true;
            string scene = mayCmdParam[1];
            if (scene == "withergatefarm")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(138f, 89.16582f), "WithergateRooftopFarm");
            }
            else if (scene == "throneroom")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(21.5f, 8.681581f), "Throneroom");
            }
            else if (scene == "nelvari")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(320.3333f, 98.76098f), "Nelvari6");
            }
            else if (scene == "wishingwell")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(55.83683f, 61.48384f), "WishingWell");
            }
            else if (scene.Contains("altar"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(199.3957f, 122.6284f), "DynusAltar");
            }
            else if (scene.Contains("hospital"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(80.83334f, 65.58415f), "Hospital");
            }
            else if (scene.Contains("sunhaven"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(268.125f, 299.9311f), "Town10");
            }
            else if (scene.Contains("nelvarifarm"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(139.6753f, 100.4739f), "NelvariFarm");
            }
            else if (scene.Contains("nelvarimine"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(144.7133f, 152.1591f), "NelvariMinesEntrance");
            }
            else if (scene.Contains("nelvarihome"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(51.5f, 54.97755f), "NelvariPlayerHouse");
            }
            else if (scene.Contains("castle"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(133.7634f, 229.2485f), "Withergatecastleentrance");
            }
            else if (scene.Contains("withergatehome"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(63.5f, 54.624f), "WithergatePlayerApartment");
            }
            else if (scene.Contains("grandtree"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(314.4297f, 235.2298f), "GrandTreeEntrance1");
            }
            else if (scene.Contains("taxi"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(101.707f, 123.4622f), "WildernessTaxi");
            }
            else if (scene == "dynus")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(94.5f, 121.09f), "Dynus");
            }
            else if (scene == "sewer")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(134.5833f, 129.813f), "Sewer");
            }
            else if (scene == "nivara")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(99f, 266.6305f), "Nivara");
            }
            else if (scene.Contains("barrack"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(71.58334f, 54.56507f), "Barracks");
            }
            else if (scene.Contains("elios"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(113.9856f, 104.2902f), "DragonsMeet");
            }
            else if (scene.Contains("dungeon"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(136.48f, 193.92f), "CombatDungeonEntrance");
            }
            else if (scene.Contains("store"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(77.5f, 58.55f), "GeneralStore");
            }
            else if (scene.Contains("beach"))
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(96.491529f, 87.46871f), "BeachRevamp");
            }
            else if (scene == "home" || scene.Contains("sunhavenhome") || scene == "farm")
            {
                lastScene = ScenePortalManager.ActiveSceneName; lastLocation = Player.Instance.transform.position;
                ScenePortalManager.Instance.ChangeScene(new Vector2(316.4159f, 152.5824f), "2Playerfarm");
            }
            else if (scene == "back")
            {
                ScenePortalManager.Instance.ChangeScene(lastLocation, lastScene);
            }
            else if (!getTpPoint(scene)) {
                CommandFunction_PrintToChat("invalid scene name".ColorText(Color.red));
            }

            return true;
        }
        // GET TELEPORT LOCATIONS
        private static bool CommandFunction_TeleportLocations()
        {
            foreach (string tpLocation in tpLocations)
                CommandFunction_PrintToChat(tpLocation.ColorText(Color.white));
            foreach (string tpLocation in tpPoints.Keys)
                CommandFunction_PrintToChat(tpLocation.ColorText(Color.white));
            return true;
        }
        // DESPAWN PET
        private static bool CommandFunction_DespawnPet()
        {
            if (lastPetName == "")
                CommandFunction_PrintToChat("No pet spawned by command".ColorText(Red));
            else
            {
                PetManager.Instance.DespawnPet(Player.Instance);
                CommandFunction_PrintToChat($"Pet ({lastPetName.ColorText(Color.white)}) removed!".ColorText(Green));
                lastPetName = "";
            }
            return true;
        }
        // SPAWN PET
        private static bool CommandFunction_SpawnPet(string[] mayCmdParam)
        {
            if (petList == null)
                petList = (Dictionary<string, Pet>)typeof(PetManager).GetField("_petDictionary", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(PetManager.Instance);
            if (petList != null)
            {
                if (mayCmdParam.Length >= 2)
                {
                    string petCmd = mayCmdParam[1];
                    List<string> despawnCmds = new List<string> { "despawn", "remove" };
                    foreach (string cmd in despawnCmds)
                    {
                        if (petCmd.Contains(cmd))
                        {
                            if (lastPetName != "")
                            {
                                PetManager.Instance.DespawnPet(Player.Instance);
                                CommandFunction_PrintToChat($"Pet ({lastPetName.ColorText(Color.white)}) removed!".ColorText(Green));
                                lastPetName = "";
                            }
                            else
                                CommandFunction_PrintToChat("No pet spawned by command".ColorText(Red));
                            return true;
                        }
                    }
                    if (petList.ContainsKey(petCmd))
                    {
                        PetManager.Instance.SpawnPet(petCmd, Player.Instance, null);
                        lastPetName = petCmd;
                    }
                    else
                        CommandFunction_PrintToChat($"wrong pet name, get pets using '{"!pets".ColorText(Color.white)}'".ColorText(Red));
                }
                else
                    CommandFunction_PrintToChat($"try '{"!pet [pet name]".ColorText(Color.white)}'!".ColorText(Red));
            }
            else
                CommandFunction_PrintToChat("ISSUE: no 'petList', u can report this bug".ColorText(Red));
            return true;
        }
        // GET PET LIST
        private static bool CommandFunction_GetPetList()
        {
            if (petList == null)
            {
                petList = (Dictionary<string, Pet>)typeof(PetManager).GetField("_petDictionary", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(PetManager.Instance);
                if (petList == null)
                    CommandFunction_PrintToChat("ISSUE: no 'petList', u can report this bug".ColorText(Red));
                return true;
            }
            CommandFunction_PrintToChat("[PET-LIST]".ColorText(Color.black));
            foreach (string pet in petList.Keys)
                CommandFunction_PrintToChat(pet);
            return true;
        }
        // UN-MARRY NPC
        private static bool CommandFunction_UnMarry(string[] mayCmdParam)
        {
            if (mayCmdParam.Length >= 2)
            {
                loadSpouts();
                string name = mayCmdParam[1];
                bool all = name.ToLower() == "all";
                NPCAI[] npcs = FindObjectsOfType<NPCAI>();
                List<string> spouses = new List<string>();
                int count = 0;
                foreach (var npcai in SingletonBehaviour<NPCManager>.Instance._npcs.Values.Where(npcai => SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("MarriedTo" + npcai.OriginalName)))
                {
                    spouses.Add(npcai.OriginalName);
                    count += 1;
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
                string progressStringCharacter = null;
                foreach (NPCAI npcai in npcs)
                {
                    string npcName = npcai.OriginalName;
                    if (!SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships.ContainsKey(npcName))
                    {
                        SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships.Add(npcName, 0);
                    }
                    if (all || npcName.ToLower() == name.ToLower())
                    {
                        if (marriageCharacters.Contains(npcName))
                        {
                            npcai.MarryPlayer();
                        }
                        loadSpouts();
                        bool progressBoolCharacterMarriedTo = SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("MarriedTo" + npcName);
                        bool progressBoolCharacterMarried = SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("Married");
                        bool progressBoolCharacterDating = SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("Dating" + npcName);
                        bool progressBoolCharacterMarriedWith = SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("MarriedWith");
                        bool progressBoolWorldTier3House = SingletonBehaviour<GameSave>.Instance.GetProgressBoolWorld("Tier3House");
                        bool progressMarriedWalkPath = progressBoolWorldTier3House ? SingletonBehaviour<GameSave>.Instance.GetProgressBoolWorld(npcName + "MarriedWalkPath") : false;
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
                        spouses.Remove(npcai.OriginalName);
                        if (ultraPolyGamySpouts.ContainsKey("MarriedTo" + npcName))
                        {
                            ultraPolyGamySpouts.Remove("MarriedTo" + npcName);
                        }
                        if (ultraPolyGamySpouts.ContainsKey("Married" + npcName))
                        {
                            ultraPolyGamySpouts.Remove("Married" + npcName);
                        }
                        if (ultraPolyGamySpouts.ContainsKey("Dating" + npcName))
                        {
                            ultraPolyGamySpouts.Remove("Dating" + npcName);
                        }
                        if (ultraPolyGamySpouts.ContainsKey("MarriedWith" + npcName))
                        {
                            ultraPolyGamySpouts.Remove("MarriedWith" + npcName);
                        }
                        progressStringCharacter = SingletonBehaviour<GameSave>.Instance.GetProgressStringCharacter("MarriedWith");
                        if (!progressStringCharacter.IsNullOrWhiteSpace())
                        {
                            SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("MarriedTo" + progressStringCharacter, false);
                            GameSave.CurrentCharacter.Relationships[progressStringCharacter] = 40f;
                            NPCAI realNPC = SingletonBehaviour<NPCManager>.Instance.GetRealNPC(progressStringCharacter);
                            realNPC.GenerateCycle(false);
                            realNPC.GeneratePath();
                            SingletonBehaviour<NPCManager>.Instance.StartNPCPath(realNPC);
                            SingletonBehaviour<GameSave>.Instance.SetProgressBoolWorld(name + "MarriedWalkPath", false);
                            if (!all)
                            {
                                CommandFunction_PrintToChat($"You divorced {npcName.ColorText(Color.white)}!".ColorText(Green));
                            }
                        }
                    }
                }
                if (spouses.Count == 0)
                {
                    SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", "");
                    SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("Married", false);
                    if (ultraPolyGamySpouts.ContainsKey("current_spouse"))
                    {
                        ultraPolyGamySpouts.Remove("current_spouse");
                    }
                    if (ultraPolyGamySpouts.ContainsKey("new_main"))
                    {
                        ultraPolyGamySpouts.Remove("new_main");
                    }
                } else
                {
                    string marriedWIth = SingletonBehaviour<GameSave>.Instance.GetProgressStringCharacter("MarriedWith");
                    string spouse = spouses[0];
                    if (!marriedWIth.IsNullOrWhiteSpace())
                    {
                        if(spouses.Contains(marriedWIth))
                        {
                            spouse = marriedWIth;
                        }
                    }
                    SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", spouse);
                    SingletonBehaviour<GameSave>.Instance.SetProgressBoolCharacter("Married", true);
                    if (ultraPolyGamySpouts.ContainsKey("current_spouse"))
                    {
                        ultraPolyGamySpouts.Remove("current_spouse");
                    }
                    if (ultraPolyGamySpouts.ContainsKey("new_main"))
                    {
                        ultraPolyGamySpouts.Remove("new_main");
                    }
                    ultraPolyGamySpouts.Add("current_spouse", spouse);
                    ultraPolyGamySpouts.Add("new_main", spouse);
                }
                saveSpouts();
                if (!all)
                {
                    return true;
                }
                CommandFunction_PrintToChat(all ? "You divorced all NPCs!".ColorText(Green) : $"no npc with the name {name.ColorText(Color.white)} found!".ColorText(Red));
            }
            else
                CommandFunction_PrintToChat("a name or parameter 'all' needed");
            return true;
        }
        // SET RELATIONSHIP
        private static bool CommandFunction_Relationship(string[] mayCmdParam)
        {
            if (mayCmdParam.Length >= 3)
            {
                float value;
                string name = mayCmdParam[1];
                bool all = name.ToLower() == "all";
                bool add = mayCmdParam.Length >= 4 && mayCmdParam[3] == "add";
                if (float.TryParse(mayCmdParam[2], out value))
                {
                    value = Math.Max(0, Math.Min(100, value));
                    NPCAI[] npcs = FindObjectsOfType<NPCAI>();
                    foreach (NPCAI npc in npcs)
                    {
                        string npcName = npc.OriginalName;
                        if (!SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships.ContainsKey(npcName))
                        {
                            SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships.Add(npcName, 0);
                        }
                        if (all || npcName.ToLower() == name.ToLower())
                        {
                            if (add)
                            {
                                npc.AddRelationship(value);
                                if (!all)
                                {
                                    CommandFunction_PrintToChat($"Relationship with {npcName.ColorText(Color.white)} is now {SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships[npcName].ToString().ColorText(Color.white)}!".ColorText(Green));
                                    return true;
                                }
                            }
                            else
                            {
                                SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships[npcName] = value;
                                if (!all)
                                {
                                    CommandFunction_PrintToChat($"Relationship with {npcName.ColorText(Color.white)} set to {value.ToString().ColorText(Color.white)}!".ColorText(Green));
                                    return true;
                                }
                            }
                        }
                    }
                    if (all)
                        CommandFunction_PrintToChat(add ? "Relationships increased!".ColorText(Green) : "Relationships set!".ColorText(Green));
                    else
                        CommandFunction_PrintToChat($"No NPC with the name {name.ColorText(Color.white)} found!".ColorText(Red));
                }
                else
                    CommandFunction_PrintToChat($"NO VALID VALUE, try '{$"!relationship {name.ColorText(Color.white)} 10".ColorText(Color.white)}'".ColorText(Red));
            }
            return true;
        }
        private static Regex npcNameRegex = new Regex(@"[a-zA-Z\s\.]+");
        private static String _GetNpcName(NPCAI npcai)
        {

            var matches = npcNameRegex.Matches(npcai.OriginalName);
            foreach (Match match in matches)
            {
                return (String)match.Value;
            }
            return null;
        }

        // MARRY NPC
        private static bool CommandFunction_MarryNPC(string[] mayCmdParam)
        {
            if (mayCmdParam.Length >= 2)
            {
                string name = mayCmdParam[1];
                bool all = name.ToLower() == "all";
                NPCAI[] npcs = FindObjectsOfType<NPCAI>();
                foreach (NPCAI npcai in npcs)
                {
                    string npcName = npcai.OriginalName;
                    if (!SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships.ContainsKey(npcName))
                    {
                        SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships.Add(npcName, 0);
                    }
                    if (all || npcName.ToLower() == name.ToLower())
                    {
                        if (SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships[npcName] < 100f)
                        {
                            SingletonBehaviour<GameSave>.Instance.CurrentSave.characterData.Relationships[npcName] = 100f;
                        }
                        if (marriageCharacters.Contains(npcai.OriginalName))
                        {
                            npcai.MarryPlayer();
                        }
                        if (!all)
                        {
                            CommandFunction_PrintToChat($"You married {npcName.ColorText(Color.white)}!".ColorText(Green));
                            return true;
                        }
                    }
                }
                CommandFunction_PrintToChat(all ? "You have married all NPCs!".ColorText(Green) : $"no npc with the name {name.ColorText(Color.white)} found!".ColorText(Red));
            }
            else
                CommandFunction_PrintToChat("a name or parameter 'all' needed");
            return true;
        }
        // SET SEASON
        private static bool CommandFunction_SetSeason(string[] mayCmdParam)
        {
            if (mayCmdParam.Length < 2)
            { CommandFunction_PrintToChat("specify the season!".ColorText(Red)); return true; }
            if (!Enum.TryParse(mayCmdParam[1], true, out Season season2))
            { CommandFunction_PrintToChat("no valid season!".ColorText(Red)); return true; }
            var Date = DayCycle.Instance;
            int targetYear = Date.Time.Year + ((int)season2 - (int)Date.Season + 4) % 4;
            DayCycle.Instance.Time = new DateTime(targetYear, Date.Time.Month, 1, Date.Time.Hour, Date.Time.Minute, 0, DateTimeKind.Utc).ToUniversalTime();
            typeof(DayCycle).GetMethod("SetInitialTime", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(DayCycle.Instance, null);
            CommandFunction_PrintToChat("It's now ".ColorText(Yellow) + season2.ToString().ColorText(Green));
            //DayCycle.Instance.SetSeason(season2);
            return true;
        }
        // YEAR FIX
        private static bool CommandFunction_ToggleYearFix()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdFixYear);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            yearFix = flag;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }
        // CHANGE YEAR
        private static bool CommandFunction_IncDecYear(string mayCommand)
        {
            if (!int.TryParse(Regex.Match(mayCommand, @"\d+").Value, out int value))
            {
                CommandFunction_PrintToChat("Something wen't wrong..".ColorText(Red));
                CommandFunction_PrintToChat("Try '!years 1' or '!years -1'".ColorText(Red));
                return true;
            }
            var Date = DayCycle.Instance;
            int newYear;
            if (mayCommand.Contains("-"))
            {
                if (Date.Time.Year - (value * 4) >= 1)
                    newYear = Date.Time.Year - (value * 4);
                else
                {
                    CommandFunction_PrintToChat("must be in year 1 or above");
                    return true;
                }
            }
            else
                newYear = Date.Time.Year + (value * 4);

            DayCycle.Instance.Time = new DateTime(newYear, Date.Time.Month, Date.Time.Day, Date.Time.Hour, Date.Time.Minute, 0, DateTimeKind.Utc).ToUniversalTime();
            typeof(DayCycle).GetMethod("SetInitialTime", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(DayCycle.Instance, null);
            CommandFunction_PrintToChat($"It's now Year {(Date.Time.Year / 4).ToString().ColorText(Green)}!".ColorText(Yellow));
            return true;
        }
        // TOGGLE CHEATS
        private static bool CommandFunction_ToggleCheats()
        {
            int i = Array.FindIndex(Commands, command => command.Name == CmdCheats);
            Commands[i].State = Commands[i].State == CommandState.Activated ? CommandState.Deactivated : CommandState.Activated;
            bool flag = Commands[i].State == CommandState.Activated;
            Settings.EnableCheats = flag;
            CommandFunction_PrintToChat($"{Commands[i].Name} {Commands[i].State.ToString().ColorText(flag ? Green : Red)}".ColorText(Yellow));
            return true;
        }

        // QUESTS LIST
        private static bool CommandFunction_QuestsList()
        {
            int questIds = 0;
            CommandFunction_PrintToChat($"!---------------All quest-ids---------------!".ColorText(Green));
            foreach (string quest in allQuestIds)
            {
                CommandFunction_PrintToChat($"Entry {questIds.ToString()}, QuestID: {quest.ColorText(Color.white)}!".ColorText(Green));
                questIds++;
            }
            return true;
        }

        // QUESTS LOG
        private static bool CommandFunction_QuestsLog()
        {
            Player.Instance.QuestList.ReloadQuestList();
            List<String> questlog = Player.Instance.QuestList.questLog.Keys.ToList<string>();
            int questIds = 0;
            CommandFunction_PrintToChat($"!---------------Quest-log-ids---------------!".ColorText(Green));
            foreach (string quest in questlog)
            {
                Player.Instance.QuestList.questLog.TryGetValue(quest, out QuestBundle value);
                string questName = value.quest.questAsset.questName;
                string keyQuestName = value.quest.questAsset.keyQuestName;
                string questDesc = value.quest.questAsset.questDescription;
                string keyQuestDesc = value.quest.questAsset.questDescription;
                CommandFunction_PrintToChat($"Entry {questIds.ToString()}, QuestID: {quest.ColorText(Color.white)}!".ColorText(Green));
                CommandFunction_PrintToChat($"Entry {questIds.ToString()}, QuestName: {questName.ColorText(Color.white)}!".ColorText(Green));
                CommandFunction_PrintToChat($"Entry {questIds.ToString()}, KeyQuestName: {keyQuestName.ColorText(Color.white)}!".ColorText(Green));
                CommandFunction_PrintToChat($"Entry {questIds.ToString()}, QuestDescription: {questDesc.ColorText(Color.white)}!".ColorText(Green));
                CommandFunction_PrintToChat($"Entry {questIds.ToString()}, KeyQuestDescription: {keyQuestDesc.ColorText(Color.white)}!".ColorText(Green));
                questIds++;
            }
            return true;
        }

        private static bool playerHaveQuest(string quest)
        {
            Player.Instance.QuestList.ReloadQuestList();
            return Player.Instance.QuestList.questLog.ContainsKey(quest);
        }

        private static bool playerIsValidQuest(string quest)
        {
            return allQuestIds.Contains(quest);
        }

        // QUEST ADD
        private static bool CommandFunction_QuestAdd(string[] mayCmdParam)
        {
            if (mayCmdParam.Length >= 2)
            {
                string quest = mayCmdParam[1];
                if (!playerIsValidQuest(quest))
                {
                    CommandFunction_PrintToChat($"Quest: {quest.ColorText(Color.white)} is not be valid!".ColorText(Green));
                    return true;
                }
                if (!playerHaveQuest(quest))
                {
                    Player.Instance.QuestList.StartQuest(quest);
                    bool added = playerHaveQuest(quest);
                    if (added)
                    {
                        CommandFunction_PrintToChat($"Quest: {quest.ColorText(Color.white)} added!".ColorText(Green));
                    }
                    else
                    {
                        CommandFunction_PrintToChat($"Quest: {quest.ColorText(Color.white)} can not be added!".ColorText(Green));
                    }
                }
                else
                {
                    CommandFunction_PrintToChat($"You have already the Quest: {quest.ColorText(Color.white)}!".ColorText(Green));
                }
            }
            else
            {
                CommandFunction_PrintToChat($"wrong use of the command !addquest".ColorText(Red));
            }
            return true;
        }

        // QUEST REMOVE
        private static bool CommandFunction_QuestRemove(string[] mayCmdParam)
        {
            if (mayCmdParam.Length >= 2)
            {
                string quest = mayCmdParam[1];
                if (!playerIsValidQuest(quest))
                {
                    CommandFunction_PrintToChat($"Quest: {quest.ColorText(Color.white)} is not be valid!".ColorText(Green));
                    return true;
                }
                if (playerHaveQuest(quest))
                {
                    Player.Instance.QuestList.AbandonQuest(quest);
                    bool removed = !playerHaveQuest(quest);
                    if (removed)
                    {
                        CommandFunction_PrintToChat($"Quest: {quest.ColorText(Color.white)} removed!".ColorText(Green));
                    } else
                    {
                        CommandFunction_PrintToChat($"Quest: {quest.ColorText(Color.white)} can not be removed!".ColorText(Green));
                    }
                }
                else
                {
                    CommandFunction_PrintToChat($"You don't have the Quest: {quest.ColorText(Color.white)}!".ColorText(Green));
                }
            } 
            else
            {
                CommandFunction_PrintToChat($"wrong use of the command !removequest".ColorText(Red));
            }
            return true;
        }

        // BACKUP SAVE GAME
        private static bool CommandFunction_BackupSaveGame()
        {
            SingletonBehaviour<GameSave>.Instance.SaveGame(false);
            SingletonBehaviour<GameSave>.Instance.WriteCharacterToFile(true, false);
            CommandFunction_PrintToChat($"Game backup saved to: {CommandExtension.lastBackupSavePath.ColorText(Color.white)}!".ColorText(Green));
            return true;
        }

        // SAVE GAME
        private static bool CommandFunction_SaveGame()
        {
            SingletonBehaviour<GameSave>.Instance.SaveGame(false);
            SingletonBehaviour<GameSave>.Instance.WriteCharacterToFile(false, false);
            CommandFunction_PrintToChat($"Game saved to: {CommandExtension.lastSavePath.ColorText(Color.white)}!".ColorText(Green));
            return true;
        }

        // TELEPORT
        private static bool CommandFunction_SetTp(string[] mayCmdParam)
        {
            if (mayCmdParam.Length <= 1)
                return true;
            string scene = mayCmdParam[1].ToLower();
            bool settp = setTpPoint(scene);
            if (settp)
            {
                CommandFunction_PrintToChat($"set tp point to {scene}!".ColorText(Color.green));
            }
            else
            {
                CommandFunction_PrintToChat("invalid scene name".ColorText(Color.red));
            }

            return true;
        }

        private static bool CommandFunction_GetTp(string[] mayCmdParam)
        {
            if (mayCmdParam.Length <= 1)
                return true;
            string scene = mayCmdParam[1].ToLower();
            bool gettp = getTpPoint(scene);
            if (gettp)
            {
                CommandFunction_PrintToChat($"Teleport to tp point {scene}!".ColorText(Color.green));
            }
            else
            {
                CommandFunction_PrintToChat("invalid scene name".ColorText(Color.red));
            }
            return true;
        }
        // GET TP LOCATIONS
        private static bool CommandFunction_ListTp()
        {
            if (!tpPointsLoaded)
            {
                loadTpPoints();
                tpPointsLoaded = true;
            }

            try
            {
                foreach (string tpLocation in tpPoints.Keys)
                    CommandFunction_PrintToChat(tpLocation.ColorText(Color.white));
            }
            catch (Exception e) { 
                Debug.LogException(e);
            }
            return true;
        }

        // CHRISTMAS SPECIAL UPDATE
        private static bool CommandFunction_Christmas()
        {
            reinitAllIds();
            try
            {
                int amount = getRangeRandomItemAmount();
                int pos = getRangeRandomItemPos();
                int rangeLength = getRangeRandomItemCount(pos);
                Debug.Log("allIdsListSize: " + allIdsList.Count.ToString());
                List<int> c = allIdsList.GetRange(pos, rangeLength);
                Action<ItemData> itemDataFunc = (i) => giveItemMessage(1, i);
                Action itemFailed = () => giveItemMessageFailed();

                foreach (int itemId in c)
                {
                    if (itemId > 0)
                    {
                        if (Database.ValidID(itemId))
                        {
                            for (int i = 0; i < amount; i++)
                            {
                                GetPlayerForCommand().Inventory.AddItem(itemId, 1, 0, true, true);
                                Database.GetData<ItemData>(itemId, itemDataFunc, itemFailed);
                            }
                        }
                        else
                        {
                            CommandFunction_PrintToChat($"no item with id: {itemId.ToString().ColorText(Color.white)} found!".ColorText(Red));
                        }
                    }
                }
                CommandFunction_PrintToChat($"Merry christmas!".ColorText(Green));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return true;
        }

        // MAIN SPOUSE
        private static bool CommandFunction_MainSp(string[] mayCmdParam)
        {
            string npcName = "";
            bool married = SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("Married");
            if (!married)
            {
                CommandFunction_PrintToChat("You are not married!".ColorText(Color.red));
                return true;
            }
            if (mayCmdParam.Length <= 1)
            {
                npcName = SingletonBehaviour<GameSave>.Instance.GetProgressStringCharacter("MarriedWith");
                CommandFunction_PrintToChat("Your main spouse is: " + npcName.ColorText(Color.green));
            } else
            {
                bool marriedTo = SingletonBehaviour<GameSave>.Instance.GetProgressBoolCharacter("MarriedTo" + mayCmdParam[1]);
                if (!marriedTo)
                {
                    CommandFunction_PrintToChat("You are not married to " + mayCmdParam[1].ColorText(Color.red));
                    return true;
                }
                npcName = mayCmdParam[1];
                SingletonBehaviour<GameSave>.Instance.SetProgressStringCharacter("MarriedWith", npcName);
                CommandFunction_PrintToChat("Your main spouse is: " + npcName.ColorText(Color.green));
            }
            return true;
        }

        #endregion

        // duplicated "command methodes" (no functions) to use the in-game COMMAND feature
        #region fake methode to show commands while typing
        [Command("help", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm0(string INFO_showAllCommandsWithCurrentState)
        {
        }
        [Command("state", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm1(string INFO_showActivatedCommands)
        {
        }
        [Command("getitemids", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm2(string xp_money_bonus_furniture_quest_all)
        {
        }
        [Command("minereset", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm3()
        {
        }
        [Command("mineclear", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm4()
        {
        }
        [Command("mineoverfill", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm5()
        {
        }
        [Command("coins", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm6(string amountToAddOrSub)
        {
        }
        [Command("orbs", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm7(string amountToAddOrSub)
        {
        }
        [Command("tickets", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm8(string amountToAddOrSub)
        {
        }
        [Command("name", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm9(string playerNameForCommand)
        {
        }
        [Command("time", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        public static void fm10(string DayOrHoure_and_Value)
        {
        }
        [Command("weather", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        public static void fm11(string raining_heatwave_clear)
        {
        }
        [Command("give", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm12(string itemName)
        {
        }
        [Command("items", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm13(string itemName_ToShowItemsWithGivenName)
        {
        }
        [Command("devkit", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm14(string INFO_getDevItems)
        {
        }
        [Command("dasher", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm15(string INFO_infiniteAirJumps)
        {
        }
        [Command("showid", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm16(string INFO_ShowsItemIdsInDescription)
        {
        }
        [Command("manafill", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm17(string INFO_refillMana)
        {
        }
        [Command("manainf", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm18(string INFO_infiniteMana)
        {
        }
        [Command("healthfill", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm19(string INFO_refillHealth)
        {
        }
        [Command("sleep", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm20(string INFO_sleepOnce)
        {
        }
        [Command("printhoveritem", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm21(string INFO_sendItemIdAndNameToChat)
        {
        }
        [Command("feedback", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm22(string INFO_toggleCommandFeedback)
        {
        }
        [Command("pause", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm23(string INFO_pauseTime)
        {
        }
        [Command("timespeed", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm24(string toggleOrSetCustomTimeSpeed)
        {
        }
        [Command("ui", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm25(string OnOrOff_ToToggleHUD)
        {
        }
        [Command("jumper", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm26(string INFO_toggleToJumpOverObjects)
        {
        }
        [Command("noclip", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm27(string INFO_toggleForNoclip)
        {
        }
        [Command("nohit", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm28(string INFO_toggleForGodMode)
        {
        }
        [Command("autofillmuseum", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm29(string INFO_autofillMusuemOnEnterMuseum)
        {
        }
        [Command("cheatfillmuseum", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm30(string INFO_cheatfillMusuemOnEnterMuseum)
        {
        }
        [Command("tp", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm31(string teleportLocation)
        {
        }
        [Command("tps", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm32(string INFO_showTeleportLocations)
        {
        }
        [Command("despawnpet", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm33(string INFO_despawnPet)
        {
        }
        [Command("pet", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm34(string petNameToSpawn)
        {
        }
        [Command("pets", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm35(string INFO_showPetNames)
        {
        }
        [Command("divorce", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm36(string NPCNameToUnmarry)
        {
        }
        [Command("relationship", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm37(string NPCName_and_value_ToChangeRelationship)
        {
        }
        [Command("marry", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm38(string NPCNameToMarry)
        {
        }
        [Command("season", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm39(string seasonToSet)
        {
        }
        [Command("yearfix", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm40(string INFO_toggleToShowTheCorrectYear)
        {
        }
        [Command("years", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm41(string yearsToAddOrSub)
        {
        }
        [Command("cheats", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm42(string INFO_toggleGameCheats)
        {
        }
        [Command("listquests", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm43(string INFO_listQuestIDs)
        {
        }
        [Command("questlog", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm44(string INFO_listQuestLogIDs)
        {
        }
        [Command("addquest", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm45(string questID)
        {
        }
        [Command("removequest", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm46(string questID)
        {
        }
        [Command("save", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm47(string INFO_saveTheGame)
        {
        }
        [Command("sleepsave", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm48(string INFO_sleepAndSaveTheGame)
        {
        }
        [Command("backupsave", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm49(string INFO_saveTheGameBackup)
        {
        }
        [Command("backupsleepsave", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm50(string INFO_sleepAndSaveTheGameBackup)
        {
        }
        [Command("settp", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm51(string tpPointName)
        {
        }
        [Command("gettp", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm52(string tpPointName)
        {
        }
        [Command("christmas", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm53(string INFO_MerryChristmas)
        {
        }

        [Command("msp", QFSW.QC.Platform.AllPlatforms, MonoTargetType.Single)]
        private static void fm54(string OPTIONAL_spouse_to_set)
        {
        }
        #endregion
        #endregion

        #region Patches
        // Year fix
        #region Patch_DayCycle.Year
        [HarmonyPatch(typeof(DayCycle))]
        [HarmonyPatch("Year", MethodType.Getter)]
        public static class Patch_DayCycleYear
        {
            public static bool Prefix(ref int __result)
            {
                __result = (DayCycle.Day - 1) / 112 + 1;
                return !yearFix;
            }
        }
        #endregion
        // auto fill museum
        private static void AddItemToHungryMonster(HungryMonster monster, int amount, SlotItemData slotItemData, bool specialItem, bool superSecretCheck, ItemData i)
        {
            monster.sellingInventory.AddItem(i.GetItem(), amount, slotItemData.slotNumber, specialItem, superSecretCheck);
        }

        private static void AddItemToHungryMonsterTransfered(HungryMonster monster, int amount, SlotItemData slotItemData, bool specialItem, bool superSecretCheck, ItemData i)
        {
            monster.sellingInventory.AddItem(i.GetItem(), amount, slotItemData.slotNumber, specialItem, superSecretCheck);
            CommandFunction_PrintToChat($"transferred: {amount.ToString().ColorText(Color.white)} * {i.name.ColorText(Color.white)}");
        }

        private static void ShowItemFailed()
        {
            return;
        }

        #region Patch_HungryMonster.SetMeta
        [HarmonyPatch(typeof(HungryMonster))]
        [HarmonyPatch("SetMeta")]
        class Patch_HungryMonsterSetMeta
        {
            static void Postfix(HungryMonster __instance, DecorationPositionData decorationData)
            {
                //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x1");
                Action<ItemData> itemDataFunc = null;
                //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x2");
                Action itemFailed = () => ShowItemFailed();
                //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x3");
                if (__instance.bundleType == BundleType.MuseumBundle)
                {
                    //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x4");
                    if (Commands[Array.FindIndex(Commands, command => command.Name == CmdCheatFillMuseum)].State == CommandState.Activated
                    ||
                    Commands[Array.FindIndex(Commands, command => command.Name == CmdAutoFillMuseum)].State == CommandState.Activated)
                    {
                        //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x5");
                        Player player = GetPlayerForCommand();
                        if (player == null)
                        {
                            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x6");
                            return;
                        }
                        HungryMonster monster = __instance;
                        //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x7");
                        if (monster.sellingInventory != null) // && monster.sellingInventory.Items != null && monster.sellingInventory.Items.Count >= 1
                        {
                            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x8");
                            if (Commands.Any(command => command.Name == CmdCheatFillMuseum && command.State == CommandState.Activated))
                            {
                                //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x9");
                                foreach (SlotItemData slotItemData in monster.sellingInventory.Items)
                                {
                                    //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x10");
                                    if (slotItemData.slot == null || slotItemData.item == null || slotItemData.slot.numberOfItemToAccept == 0 || slotItemData.amount == slotItemData.slot.numberOfItemToAccept || (slotItemData.slot.serializedItemToAccept == null && slotItemData.slot.itemToAccept == null) )
                                    {
                                        //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x11");
                                        continue;
                                    }
                                    int idToAccept = slotItemData.slot.itemToAccept != null ? slotItemData.slot.itemToAccept.id : 0;
                                    idToAccept = idToAccept == 0 && slotItemData.slot.serializedItemToAccept != null ? slotItemData.slot.serializedItemToAccept.id : 0;
                                    if (!monster.name.ToLower().Contains("money"))
                                    {
                                        //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x12");
                                        itemDataFunc = (i) => AddItemToHungryMonster(monster, slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData, false, true, i);
                                        //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x13");
                                        Database.GetData<ItemData>(slotItemData.slot.serializedItemToAccept.id, itemDataFunc, itemFailed);
                                        //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x14");
                                    }
                                    else if (monster.name.ToLower().Contains("money"))
                                    {
                                        Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x15");
                                        if (idToAccept >= 60000 && idToAccept <= 60002)
                                        {
                                            itemDataFunc = (i) => AddItemToHungryMonster(monster, slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData, false, false, i);
                                            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x16");
                                            Database.GetData<ItemData>(idToAccept, itemDataFunc, itemFailed);
                                            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x17");
                                        }//if (slotItemData.slot.itemToAccept.id == 60000)
                                        //    monster.sellingInventory.AddItem(slotItemData.slot.itemToAccept.id, slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData.slotNumber, false, false);
                                        //else if (slotItemData.slot.itemToAccept.id == 60001)
                                        //    monster.sellingInventory.AddItem(slotItemData.slot.itemToAccept.id, slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData.slotNumber, false, false);
                                        //else if (slotItemData.slot.itemToAccept.id == 60002)
                                        //    monster.sellingInventory.AddItem(slotItemData.slot.itemToAccept.id, slotItemData.slot.numberOfItemToAccept - slotItemData.amount, slotItemData.slotNumber, false, false);
                                    }
                                    //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x18");
                                    monster.UpdateFullness();
                                }
                            }
                            else
                            {
                                //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x19");
                                foreach (SlotItemData slotItemData in monster.sellingInventory.Items)
                                {
                                    //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x20");
                                    if (!monster.name.ToLower().Contains("money") && slotItemData.item != null && player.Inventory != null)
                                    {
                                        //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x21");
                                        if (slotItemData.slot == null || slotItemData.item == null || slotItemData.slot.numberOfItemToAccept == 0 || slotItemData.amount == slotItemData.slot.numberOfItemToAccept || (slotItemData.slot.serializedItemToAccept == null && slotItemData.slot.itemToAccept == null))
                                        {
                                            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x22");
                                            continue;
                                        }
                                        int idToAccept = slotItemData.slot.itemToAccept != null ? slotItemData.slot.itemToAccept.id : 0;
                                        idToAccept = idToAccept == 0 && slotItemData.slot.serializedItemToAccept != null ? slotItemData.slot.serializedItemToAccept.id : 0;
                                        Inventory pInventory = player.Inventory;
                                        //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x23");
                                        foreach (var pItem in pInventory.Items)
                                        {
                                            //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x24");
                                            if (pItem.id == idToAccept)
                                            {
                                                //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x25");
                                                int amount = Math.Min(pItem.amount, slotItemData.slot.numberOfItemToAccept - slotItemData.amount);
                                                //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x26");
                                                itemDataFunc = (i) => AddItemToHungryMonsterTransfered(monster, amount, slotItemData, false, true, i);
                                                //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x27");
                                                Database.GetData<ItemData>(idToAccept, itemDataFunc, itemFailed);
                                                //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x28");
                                                player.Inventory.RemoveItem(pItem.item, amount);
                                                //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x29");
                                                monster.UpdateFullness();
                                               // Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x30");
                                            }
                                        }
                                        //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x31");
                                    }
                                }
                            }

                        }
                        //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x32");
                        Array.ForEach(FindObjectsOfType<MuseumBundleVisual>(), vPodium => typeof(MuseumBundleVisual).GetMethod("OnSaveInventory", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(vPodium, null));
                        //Debug.Log((object)"CommandExteion Enable Autofill Museum all ok here x33");
                    }
                }


            }
        }
        #endregion

        // get name for singleplayer
        #region Patch_GameSave.LoadCharacter
        [HarmonyPatch(typeof(GameSave), nameof(GameSave.LoadCharacter))]
        class Patch_GameSaveLoadCharacter
        {
            static void Postfix(int characterNumber, GameSave __instance) => playerNameForCommands = playerNameForCommandsFirst = __instance.CurrentSave.characterData.characterName;
        }
        #endregion

        // infinite airSkips
        #region Patch_Player.AirSkipsUsed
        [HarmonyPatch(typeof(Player), nameof(Player.AirSkipsUsed), MethodType.Setter)]
        class Patch_PlayerAirSkipsUsed
        {
            static bool Prefix(int value)
            {
                return !infAirSkips;
            }
        }
        #endregion

        // skip chat bubble for commands
        #region Patch_Player.DisplayChatBubble
        [HarmonyPatch(typeof(Player), nameof(Player.DisplayChatBubble))]
        class Patch_PlayerDisplayChatBubble
        {
            static bool Prefix(ref string text)
            {
                if (CheckIfCommandDisplayChatBubble(text))
                    return false;
                return true;
            }
        }
        #endregion

        // get chat message for command check
        #region Patch_Player.SendChatMessage
        [HarmonyPatch(typeof(Player), nameof(Player.SendChatMessage), new[] { typeof(string), typeof(string) })]
        class Patch_PlayerSendChatMessage
        {
            static bool Prefix(string characterName, string message)
            {
                if (characterName != playerNameForCommands && characterName != playerNameForCommandsFirst)
                    return true;
                if (CheckIfCommandSendChatMessage(message))
                    return false;  // SEND COMMAND 
                return true;  // SEND CHAT
            }
        }
        #endregion

        // send welcome message and get all items
        #region Patch_Player.Initialize
        [HarmonyPatch(typeof(Player), nameof(Player.Initialize))]
        class Patch_PlayerInitialize
        {
            static void Postfix(Player __instance)
            {
                if (Commands[Array.FindIndex(Commands, command => command.Name == CmdFeedbackDisabled)].State == CommandState.Deactivated)
                {
                    // show welcome message
                    if (ranOnceOnPlayerSpawn < 2)
                        ranOnceOnPlayerSpawn++;
                    else if (ranOnceOnPlayerSpawn == 2)
                    {
                        CommandFunction_PrintToChat("> Command Extension Active! type '!help' for command list".ColorText(Color.magenta) + "\n -----------------------------------------------------------------".ColorText(Color.black));
                        ranOnceOnPlayerSpawn++;
                        // enable test helper
                        if (debug)
                        {
                            CommandFunction_PrintToChat("debug: enable cheat commands".ColorText(Color.magenta));
                            CommandFunction_Jumper();
                            CommandFunction_InfiniteMana();
                            CommandFunction_InfiniteAirSkips();
                            CommandFunction_Pause();
                        }
                    }

                    // Enable in-game command feature 
                    if (Player.Instance != null && QuantumConsole.Instance)
                    {
                        QuantumConsole.Instance.GenerateCommands = Settings.EnableCheats = true;
                        QuantumConsole.Instance.Initialize();
                        Settings.EnableCheats = false;
                    }
                }
            }
        }
        #endregion

        // jump over everything
        #region Patch_Player.Update
        [HarmonyPatch(typeof(Player), "Update")]
        class Patch_PlayerUpdate
        {
            static void Postfix(ref Player __instance)
            {
                if (jumpOver && !noclip) //ignore the wrong turned boolean!
                {
                    if (__instance.Grounded)
                        __instance.rigidbody.bodyType = RigidbodyType2D.Dynamic;
                    else
                        __instance.rigidbody.bodyType = RigidbodyType2D.Kinematic;
                }
            }
        }
        #endregion

        // no mana use
        #region Patch_Player.UseMana
        [HarmonyPatch(typeof(Player), nameof(Player.UseMana), new[] { typeof(float) })]
        class Patch_PlayerUseMana
        {
            static bool Prefix(float mana)
            {
                return !infMana;
            }
        }
        #endregion

        // pause and custom time multiplier
        #region Patch_DayCycle.DaySpeedMultiplier
        [HarmonyPatch(typeof(Settings))]
        [HarmonyPatch("DaySpeedMultiplier", MethodType.Getter)]
        class Patch_DayCycleDaySpeedMultiplier
        {
            static bool Prefix(ref float __result)
            {
                if (Commands[Array.FindIndex(Commands, command => command.Name == CmdPause)].State == CommandState.Activated)
                    __result = 0f;
                else if (Commands[Array.FindIndex(Commands, command => command.Name == CmdCustomDaySpeed)].State == CommandState.Activated)
                    __result = timeMultiplier;
                else
                    return true;  // vanilla mulitplier
                return false;  // custom mulitplier
            }
        }
        #endregion

        // print itemId on hover
        #region Patch_Item.GetToolTip
        [HarmonyPatch]
        class Patch_ItemGetToolTip
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                Type[] itemTypes = new Type[] { typeof(NormalItem), typeof(ArmorItem), typeof(FoodItem), typeof(FishItem), typeof(CropItem), typeof(WateringCanItem), typeof(AnimalItem), typeof(PetItem), typeof(ToolItem) };
                foreach (Type itemType in itemTypes)
                    yield return AccessTools.Method(itemType, "GetToolTip", new[] { typeof(Tooltip), typeof(int), typeof(bool) });
            }

            static void PrintOnHover(int id, bool printOnHover, bool appendItemDescWithId, ItemData itemData)
            {
                if (printOnHover)
                    CommandFunction_PrintToChat($"{id} : {itemData.name}");
                string text = "ID: ".ColorText(Color.magenta) + id.ToString().ColorText(Color.magenta) + "\"\n\"";
                if (appendItemDescWithId)
                {
                   if (!itemData.description.Contains(text))
                      itemData.description = text + itemData.description;
                }
                else if (itemData.description.Contains(text))
                  itemData.description = itemData.description.Replace(text, "");
            }

            static void PrintOnFailed()
            {
                return;
            }

            static void Prefix(Item __instance)
            {
                int id = __instance.ID();
                Action<ItemData> itemDataFunc = (itemData) => PrintOnHover(id, printOnHover, appendItemDescWithId, itemData);
                Action itemFailed = () => PrintOnFailed();
                Database.GetData<ItemData>(id, itemDataFunc, itemFailed);
            }
        }
        #endregion

        // append itemId to itemDescription
        #region Patch_ItemData.FormattedDescription
        [HarmonyPatch(typeof(ItemData))]
        [HarmonyPatch("FormattedDescription", MethodType.Getter)]
        class Patch_ItemDataFormattedDescription
        {
            static void Postfix(ref string __result, ItemData __instance)
            {
                if (debug)
                    __result = __instance.id.ToString().ColorText(Color.magenta) + "\"\n\"";
            }
        }
        #endregion

        private static string getSavePath(
                bool backup,
                GameSave __instance,
                string ___characterFolder)
        {
                byte characterIndex = __instance.CurrentSave.characterData.characterIndex;
                byte zero = (byte)0;
                string path = "";
                for (byte i = characterIndex; i < 255; i++)
                {
                    string str_ = characterIndex == zero ? "" : characterIndex.ToString();
                    if (backup)
                        path = Application.persistentDataPath + "/" + ___characterFolder + "/Backups/" + GameSave.SanitizeFileName(__instance.CurrentSave.characterData.characterName) + str_ + ".save";
                    else
                        path = Application.persistentDataPath + "/" + ___characterFolder + "/" + GameSave.SanitizeFileName(__instance.CurrentSave.characterData.characterName) + str_ + ".save";
                    if (File.Exists(path))
                    {
                        continue;
                    }
                }
                return path;
        }
        // WriteCharacterToFile
        #region Patch_GameSave.WriteCharacterToFile
        [HarmonyPatch(typeof(GameSave), nameof(GameSave.WriteCharacterToFile))]
        class Patch_GameSaveWriteCharacterToFile
        {
            public static void Postfix(
                bool backup,
                bool newCharacter,
                GameSave __instance,
                string ___characterFolder)
            {
                bool noBackup = false;
                CommandExtension.logger.LogDebug((object)("Execute WriteCharacterToFile!"));
                Debug.Log((object)"Execute WriteCharacterToFile");
                try
                {
                    Debug.Log((object)"TestCommandExtension: noBackup = true");
                    Debug.Log((object)"Test5a");
                    if (!Directory.Exists(Application.persistentDataPath + "/" + ___characterFolder + "/"))
                        Directory.CreateDirectory(Application.persistentDataPath + "/" + ___characterFolder + "/");
                    if (!Directory.Exists(Application.persistentDataPath + "/" + ___characterFolder + "/Backups/"))
                        Directory.CreateDirectory(Application.persistentDataPath + "/" + ___characterFolder + "/Backups/");
                    object obj = typeof(GameSave).GetField("fileExtension", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue((object)null);
                    if (!backup)
                    {
                        noBackup = true;
                    }
                    if (obj == null)
                    {
                        noBackup = true;
                    }
                    if (newCharacter)
                    {
                        noBackup = true;
                    }
                    GameSaveData gameSaveData = (GameSaveData)__instance.GetType().GetMethod("CopySaveData", BindingFlags.Instance | BindingFlags.NonPublic).Invoke((object)__instance, new object[1]
                    {
                        (object) __instance.CurrentSave
                    });
                    Debug.Log((object)"TestCommandExtension: all ok until here!");
                    if (obj == null)
                    {
                        Debug.Log((object)"TestCommandExtension: can not find fileExtension");
                    }
                    byte characterIndex = __instance.CurrentSave.characterData.characterIndex;
                    byte zero = (byte)0;
                    if (noBackup)
                    {
                        Debug.Log((object)"Test5b");
                        string path = getSavePath(backup, __instance, ___characterFolder);
                        Debug.Log((object)"Test5c");
                        if (newCharacter)
                        {
                            int num;
                            object[] objArray;
                            for (num = 0; File.Exists(path) && num < (int)byte.MaxValue; path = string.Concat(objArray))
                            {
                                ++num;
                                objArray = new object[7]
                                {
                                    (object) Application.persistentDataPath,
                                    (object) "/",
                                    (object) ___characterFolder,
                                    (object) "/",
                                    (object) __instance.CurrentSave.characterData.characterName,
                                    (object) num,
                                    (object) ".save"
                                };
                            }
                            gameSaveData.characterData.characterIndex = (byte)num;
                            __instance.CurrentSave.characterData.characterIndex = (byte)num;
                        }
                        Debug.Log((object)"Test5d");
                        byte[] bytes1 = ZeroFormatterSerializer.Serialize<GameSaveData>(gameSaveData);
                        Debug.Log((object)"Test5e");
                        byte[] bytes2 = GameSave.CompressBytes(bytes1);
                        Debug.Log((object)"Test5f");
                        Debug.Log((object)("Write to file " + (object)gameSaveData.characterData.characterIndex));
                        File.WriteAllBytes(path, bytes2);
                        Debug.Log((object)"Test5g");
                        CommandExtension.logger.LogDebug((object)("Writing save to: " + path));
                        CommandExtension.lastSavePath = path;
                        return;
                    } 
                    int num1 = DayCycle.DayFromTime(gameSaveData.worldData.time);
                    if (gameSaveData.worldData.time.Hour < 6)
                        --num1;
                    int num9 = gameSaveData.worldData.time.Minute; 
                    int num10 = gameSaveData.worldData.time.Second;
                    int num12 = gameSaveData.worldData.time.Year;
                    int num13 = gameSaveData.worldData.time.Month;
                    int num14 = gameSaveData.worldData.time.Hour;
                    string str = gameSaveData.characterData.characterIndex == (byte)0 ? "" : gameSaveData.characterData.characterIndex.ToString();
                    string path1 = Application.persistentDataPath + "/" + ___characterFolder + "/Backups/" + Regex.Replace(gameSaveData.characterData.characterName, "<|>|=|#", "") + str;
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(path1);
                    }
                    string path2 = path1 + "_year_" + num12.ToString() + "_month_" + num13.ToString()  + "_day_" + num1.ToString() + "_hour_" + num14.ToString() + "_min_" + num9.ToString() + "_sec_" + num10.ToString() + "." + obj?.ToString();
                    Debug.Log((object)"Path2");
                    Debug.Log((object)path2);
                    CommandExtension.logger.LogDebug((object)(path2));
                    if (File.Exists(path2))
                    {
                        CommandExtension.lastBackupSavePath = path2;
                        return;
                    }
                    CommandExtension.logger.LogDebug((object)("Writing backup to: " + path2));
                    byte[] bytes = GameSave.CompressBytes(ZeroFormatterSerializer.Serialize<GameSaveData>(gameSaveData));
                    File.WriteAllBytes(path2, bytes);
                    CommandExtension.lastBackupSavePath = path2;
                    foreach (string file in Directory.GetFiles(path1, "day*." + obj?.ToString(), SearchOption.TopDirectoryOnly))
                    {
                        if (CommandExtension.DontDeleteBackups.Value)
                        {
                            CommandExtension.logger.LogDebug((object)("Don't delete files!"));
                            break;
                        }
                        Match match = Regex.Match(file, "day(\\d+)\\.");
                        if (match.Success)
                        {
                            int num2 = int.Parse(match.Groups[1].Value);
                            if (num2 != 0)
                            {
                                int num3 = num1 - num2;
                                CommandExtension.logger.LogDebug((object)string.Format("Found backup from day {0} with path {1}, it's {2} days old", (object)num2, (object)file, (object)num3));
                                if (num3 > CommandExtension.RetentionDays.Value)
                                {
                                    CommandExtension.logger.LogDebug((object)("Deleting " + file + " due to being too old"));
                                    File.Delete(file);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    CommandExtension.logger.LogError((object)string.Format("Plugin {0} error in WriteCharacterToFile: {1}", (object)"CommandExtension", (object)ex));
                }
            }
        }
        #endregion
        #endregion
    }
}