// Decompiled with JetBrains decompiler
// Type: KryziK.SunHaven.ChestLabels.ChestPatch
// Assembly: KryziK.SunHaven.ChestLabels, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DD7A1110-A8B4-49E6-86A7-8703E93D227C
// Assembly location: C:\Users\ABC\Downloads\Chest Labels-150-1-0-5-1698843986 (1)\KryziK.SunHaven.ChestLabels.dll

using HarmonyLib;
using KryziK.Utilities;
using System.Collections.Generic;
using Wish;

namespace KryziK.SunHaven.ChestLabels
{
  [HarmonyPatch(typeof (Chest))]
  internal static class ChestPatch
  {
    [HarmonyPostfix]
    [HarmonyPatch("SetMeta")]
    [HarmonyPatch("ReceiveNewMeta")]
    [HarmonyPatch("SaveMeta")]
    private static void SetMeta_Postfix(Chest __instance)
    {
      if (__instance.IsNotActuallyA<Chest>())
        return;
      KryChestLabel component;
      if (!__instance.TryGetComponent<KryChestLabel>(out component))
        component = __instance.gameObject.AddComponent<KryChestLabel>().Init();
      component.DoUpdate();
    }

    [HarmonyPostfix]
    [HarmonyPatch("InteractionPoint", MethodType.Getter)]
    private static void Chest_InteractionPoint_Postfix(Chest __instance, InteractionInfo __result)
    {
      if (__instance.IsNotActuallyA<Chest>())
        return;
      string text = __instance.GetComponent<KryChestLabel>().GetText();
      __result.interactionText = new List<string>()
      {
        !string.IsNullOrWhiteSpace(text) ? text : "Chest"
      };
    }
  }
}
