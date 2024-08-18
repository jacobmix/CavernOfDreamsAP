using HarmonyLib;

namespace CoDArchipelago.MiscPatches
{
    static class DropCarryablesOnWarp
    {
        [HarmonyPatch(typeof(WarpTrigger), "Warp")]
        static class RemoveCarryablesOnWarp
        {
            public static void Postfix(WarpTrigger __instance)
            {
                if (!APClient.Client.SlotData.dropCarryables) return;

                Player player = GlobalHub.Instance.player;
                if (!player.IsCarrying() && !player.WearingHoverBoots()) return;

                if (player.WearingHoverBoots()) {
                    GlobalHub.Instance.player.EquipHoverBoots(false, true);
                } else {
                    GlobalHub.Instance.player.ReleaseCarryable(new());
                }
            }
        }
    }
}
