using System;
using HarmonyLib;
using UnityEngine;
using XRL.World;
using XRL.World.Parts;

namespace Fyrefly.HarmonyPatches{ 
    [HarmonyPatch(typeof(Brain), "HandleEvent", new Type[] {typeof(ObjectCreatedEvent)} )]
    public class BrainObjectCreatedPatch{

        public static void Postfix(Brain __instance, ObjectCreatedEvent E) {
            Debug.Log("Running Brain Patch to Require Brain!");
            if (__instance.ParentObject == null) {
                Debug.LogError("No parent object to require part on!");
                return;
            } 
            __instance.ParentObject.RequirePart<DynamicPersonality>();
            Debug.Log($"Brain Part added to {__instance.ParentObject.DebugName}");
        }

    }
}