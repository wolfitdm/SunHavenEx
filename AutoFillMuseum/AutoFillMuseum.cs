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
using I2.Loc;

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
            //Debug.Log((object)"Enable Autofill Museum all ok here");
            ModEnabled = Config.Bind("General", "Enabled", true, $"Enable {PluginInfo.PLUGIN_NAME}");
            //Debug.Log((object)"Enable Autofill Museum all ok here 1");
            ShowNotifications = Config.Bind("General", "Show Notifications", true, "Show notifications when items are added to the museum");
            //Debug.Log((object)"Enable Autofill Museum all ok here 2");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginInfo.PLUGIN_GUID);
            //Debug.Log((object)"Enable Autofill Museum all ok here 3");
        }

        private static void GetItemData(HungryMonster __instance, Inventory playerInventory, SlotItemData pItem, SlotItemData slotItemData, int amountToTransfer, bool showNotification, ItemData i)
        {
            //Debug.Log((object)"Enable Autofill Museum all ok here 4");
            __instance.sellingInventory.AddItem(i.GetItem(), amountToTransfer, slotItemData.slotNumber, false);
            //Debug.Log((object)"Enable Autofill Museum all ok here 5");
            playerInventory.RemoveItem(pItem.item, amountToTransfer);
            //Debug.Log((object)"Enable Autofill Museum all ok here 6");
            __instance.UpdateFullness();
            //Debug.Log((object)"Enable Autofill Museum all ok here 7");
            if (showNotification)
            {
                //Debug.Log((object)"Enable Autofill Museum all ok here 8");
                SingletonBehaviour<NotificationStack>.Instance.SendNotification($"Added {i.name} to the museum!", unique: true);
                //SingletonBehaviour<HelpTooltips>.Instance.SendNotification($"Added {i.name} to the museum!", $"Added {i.name} to the museum!" + (object)i.ID, (List<(Transform, Vector3, Direction)>)null, i.ID);
            }
            //Debug.Log((object)"Enable Autofill Museum all ok here 9");
        }

        private static void ShowItemFailed()
        {
            //Debug.Log((object)"Enable Autofill Museum all ok here 10");
            return;
        }

        /*
         *    ItemIcon componentInChildren = slotItemData.slot.GetComponentInChildren<ItemIcon>();
                    if (!(bool)(UnityEngine.Object)componentInChildren)
                        return false;
                    ItemData itemData = componentInChildren.itemData;
                    int num = slotItemData.slot.onlyAcceptSpecificItem ? slotItemData.slot.numberOfItemToAccept : itemData.stackSize;
                    var amountToTransfer = Math.Min(pItem.amount, slotItemData.slot.numberOfItemToAccept - slotItemData.amount);
        */
        [HarmonyPostfix]
        [HarmonyPatch(typeof(HungryMonster), nameof(HungryMonster.SetMeta))]
        private static void HungryMonster_SetMeta(HungryMonster __instance)
        {
            //Debug.Log((object)"Enable Autofill Museum all ok here 11");
            if (!ModEnabled.Value) return;
            //Debug.Log((object)"Enable Autofill Museum all ok here 12");
            if (__instance.bundleType != BundleType.MuseumBundle || Player.Instance == null || Player.Instance.Inventory == null) return;
            //Debug.Log((object)"Enable Autofill Museum all ok here 13");
            var playerInventory = Player.Instance.Inventory;
            //Debug.Log((object)"Enable Autofill Museum all ok here 14");

            if (__instance.sellingInventory == null) return;
            //Debug.Log((object)"Enable Autofill Museum all ok here 15");
            bool anythingIsNull = false;

            foreach (var slotItemData in __instance.sellingInventory.Items.Where(slotItemData => slotItemData.item != null && slotItemData.slot.numberOfItemToAccept != 0 && slotItemData.amount < slotItemData.slot.numberOfItemToAccept))
            {
                if (slotItemData == null)
                {
                    //Debug.Log((object)"Enable Autofill Museum all ok here 16");
                    anythingIsNull = true;
                }
                foreach (var pItem in playerInventory.Items)
                {
                    if (pItem == null)
                    {
                        //Debug.Log((object)"Enable Autofill Museum all ok here 17");
                        anythingIsNull = true;
                    }
                    if (slotItemData != null && slotItemData.slot == null)
                    {
                        //Debug.Log((object)"Enable Autofill Museum all ok here 18");
                        anythingIsNull = true;
                    }
                    if (slotItemData != null && slotItemData.slot != null && slotItemData.slot.serializedItemToAccept == null)
                    {
                        //Debug.Log((object)"Enable Autofill Museum all ok here 19.5");
                        anythingIsNull = true;
                    }
                    if (slotItemData != null && slotItemData.slot != null && slotItemData.slot.itemToAccept == null)
                    {
                        //Debug.Log((object)"Enable Autofill Museum all ok here 19");
                    }
                    
                    //Debug.Log((object)"Enable Autofill Museum all ok here 20");
                    if (anythingIsNull) { continue; };
                    if (pItem.id != slotItemData.slot.serializedItemToAccept.id) continue;
                    //Debug.Log((object)"Enable Autofill Museum all ok here 21");
                    //Debug.Log((object)"Enable Autofill Museum all ok here 22");
                    var amountToTransfer = Math.Min(pItem.amount, slotItemData.slot.numberOfItemToAccept - slotItemData.amount);
                    Action<ItemData> itemDataFunc = (i) => GetItemData(__instance, playerInventory, pItem, slotItemData, amountToTransfer, ShowNotifications.Value, i);
                    //Debug.Log((object)"Enable Autofill Museum all ok here 23");
                    Action itemFailed = () => ShowItemFailed();
                    //Debug.Log((object)"Enable Autofill Museum all ok here 24");
                    Database.GetData<ItemData>(slotItemData.slot.serializedItemToAccept.id, itemDataFunc, itemFailed);
                    //Debug.Log((object)"Enable Autofill Museum all ok here 25");
                }
                //Debug.Log((object)"Enable Autofill Museum all ok here 25.5");
            }
            MethodInfo mInfoMethod = null;
            foreach (var vPodium in FindObjectsOfType<MuseumBundleVisual>())
            {
                //Debug.Log((object)"Enable Autofill Museum all ok here 26");
                mInfoMethod = vPodium.GetType().GetMethods().FirstOrDefault(method => method.Name == "OnSaveInventory" && method.GetParameters().Count() == 0);
                //Debug.Log((object)"Enable Autofill Museum all ok here 27");
                break;
            }
            if (mInfoMethod == null)
            {
                //Debug.Log((object)"Enable Autofill Museum all ok here 28");
                return;
            }
            foreach (var vPodium in FindObjectsOfType<MuseumBundleVisual>())
            {
                if (vPodium == null)
                {
                    //Debug.Log((object)"Enable Autofill Museum all ok here 29");
                    continue;
                }
                //Debug.Log((object)"Enable Autofill Museum all ok here 30");
                mInfoMethod.Invoke(vPodium, null);
                //Debug.Log((object)"Enable Autofill Museum all ok here 31");
            }
        }
    }
}