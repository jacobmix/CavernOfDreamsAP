using HarmonyLib;
using UnityEngine;
using CoDArchipelago.GlobalGameScene;

namespace CoDArchipelago.MiscPatches
{
    ///<summary>
    ///Removes the Jester Boots removal trigger in Sun Cavern, and instead
    ///removes Jester Boots upon portal entry.
    ///</summary>
    static class LoseBootsInSunCavernPortals
    {
        class RemoveSunCavernBootRemovalTrigger: InstantiateOnGameSceneLoad
        {
            public RemoveSunCavernBootRemovalTrigger()
            {
                GameObject.DestroyImmediate(GameScene.FindInScene("CAVE", "Sun Cavern (Main)/Objects/BootRemovalTrigger").gameObject);
            }
        }

        [HarmonyPatch(typeof(WarpTrigger), "Warp")]
        static class RemoveBootsOnSunCavernPortalWarp
        {
            public static void Postfix(WarpTrigger __instance)
            {
                // early-out to allow DropCarryablesOnWarp to do it instead
                if (DropCarryablesOnWarp.shouldDropCarryables) return;

                Player player = GlobalHub.Instance.player;
                if (!player.WearingHoverBoots()) return;

                if (!LocationSplitPatches.GratitudeTeleports.IsSunCavernPortal(__instance.transform)) return;

                GlobalHub.Instance.player.EquipHoverBoots(false, true);
            }
        }
    }
}
