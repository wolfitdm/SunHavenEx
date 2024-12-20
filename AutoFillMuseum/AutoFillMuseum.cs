using BepInEx;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using BepInEx.Configuration;
using Wish;
using PSS;
using Sirenix.Serialization;
using System.Security.Policy;
using System.Collections.Generic;
using UnityEngine;

namespace AutoFillMuseum
{
    public static class PluginInfo
    {
        // public const string PLUGIN_AUTHOR = "Rx4Byte";
        public const string PLUGIN_NAME = "Automatic Museums Filler";
        public const string PLUGIN_GUID = "com.Rx4Byte.AutomaticMuseumsFiller";
        public const string PLUGIN_VERSION = "1.2";
    }

    [Harmony]
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class AutoFillMuseum : BaseUnityPlugin
    {
        private static ConfigEntry<bool> ModEnabled { get; set; }
        private static ConfigEntry<bool> ShowNotifications { get; set; }

        private void Awake()
        {
            ModEnabled = Config.Bind("General", "Enabled", true, $"Enable {PluginInfo.PLUGIN_NAME}");
            ShowNotifications = Config.Bind("General", "Show Notifications", true, "Show notifications when items are added to the museum");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginInfo.PLUGIN_GUID);
        }

        private static void GetItemData(HungryMonster __instance, Inventory playerInventory, SlotItemData pItem, SlotItemData slotItemData, int amountToTransfer, bool showNotification, ItemData i)
        {
            __instance.sellingInventory.AddItem(i.GetItem(), amountToTransfer, slotItemData.slotNumber, false);
            playerInventory.RemoveItem(pItem.item, amountToTransfer);
            __instance.UpdateFullness();
            if (showNotification)
            {
                SingletonBehaviour<HelpTooltips>.Instance.SendNotification($"Added {i.name} to the museum!", $"Added {i.name} to the museum!" + (object)i.ID, (List<(Transform, Vector3, Direction)>)null, i.ID);
            }
        }

        private static void ShowItemFailed()
        {
            return;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(HungryMonster), nameof(HungryMonster.SetMeta))]
        private static void HungryMonster_SetMeta(HungryMonster __instance)
        {
            if (!ModEnabled.Value) return;
            if (__instance.bundleType != BundleType.MuseumBundle || Player.Instance == null || Player.Instance.Inventory == null) return;

            var playerInventory = Player.Instance.Inventory;

            if (__instance.sellingInventory == null) return;

            foreach (var slotItemData in __instance.sellingInventory.Items.Where(slotItemData => slotItemData.item != null && slotItemData.slot.numberOfItemToAccept != 0 && slotItemData.amount < slotItemData.slot.numberOfItemToAccept))
            {
                foreach (var pItem in playerInventory.Items)
                {
                    if (pItem.id != slotItemData.slot.itemToAccept.id) continue;
                    var amountToTransfer = Math.Min(pItem.amount, slotItemData.slot.numberOfItemToAccept - slotItemData.amount);
                    Action<ItemData> itemDataFunc = (i) => GetItemData(__instance, playerInventory, pItem, slotItemData, amountToTransfer, ShowNotifications.Value, i);
                    Action itemFailed = () => ShowItemFailed();
                    Database.GetData<ItemData>(slotItemData.slot.itemToAccept.id, itemDataFunc, itemFailed);
                }
            }
            MethodInfo mInfoMethod = null;
            foreach (var vPodium in FindObjectsOfType<MuseumBundleVisual>())
            {
                mInfoMethod = vPodium.GetType().GetMethods().FirstOrDefault(method => method.Name == "OnSaveInventory" && method.GetParameters().Count() == 0);
                break;
            }

            foreach (var vPodium in FindObjectsOfType<MuseumBundleVisual>())
            {
                mInfoMethod.Invoke(vPodium, null);
            }
        }
    }
}
