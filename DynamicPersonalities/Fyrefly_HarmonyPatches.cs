using System;
using HarmonyLib;
using UnityEngine;
using XRL.UI;
using XRL.World;
using XRL.World.Parts;

namespace Fyrefly.HarmonyPatches{ 

    /// <summary>
    /// Harmony Patch to attach the Personality to everything that gets a brain
    /// </summary>
    [HarmonyPatch(typeof(Brain), "InitFromFactions")]
    public class BrainObjectInitialisedPatch{

        public static void Postfix(Brain __instance) {
            //Debug.Log("Running Brain Patch to Require Brain!");
            if (__instance.ParentObject == null) {
                Debug.LogError("No parent object to require part on!");
                return;
            } 
            __instance.ParentObject.RequirePart<DynamicPersonality>();
            //Debug.Log($"Brain Part added to {__instance.ParentObject.DebugName}");
        }

    }

        [HarmonyPatch(typeof(TradeUI), "ShowTradeScreen")]
    public class CheckTradeWillingnessPatch{
        
        public static bool Prefix(TradeUI __instance, XRL.World.GameObject Trader) {
            Debug.Log("Running Trade Patch to check willingness!");
            if (Trader.HasPart("DynamicPersonality")) {
                DynamicPersonality personality = Trader.GetPart<DynamicPersonality>();
                if (personality.WillingToTrade) {
                    Debug.Log("Is willing to trade!");
                    return true;
                } else {
                    Debug.Log("Is NOT willing to trade!");
                    Popup.ShowFail(Trader.The + Trader.ShortDisplayName + " does not like you well enough to trade.");
                    return false;
                }

            }
            else {
                Debug.LogError($"CheckTradeWillingnessPatch run on gameobject {Trader.DebugName} which has no DynamicPersonality part");
                return true;
            }
        }

    }


}