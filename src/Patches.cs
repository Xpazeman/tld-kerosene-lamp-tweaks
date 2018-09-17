using System;
using Harmony;

namespace KeroseneLampTweaks
{
    [HarmonyPatch(typeof(KeroseneLampItem), "ReduceFuel", new Type[] { typeof(float) })]
    public class KeroseneLampItem_ReduceFuel
    {
        public static void Prefix(ref KeroseneLampItem __instance, ref float hoursBurned)
        {
            var gi = Traverse.Create(__instance).Field("m_GearItem").GetValue<GearItem>();

            if (!gi.m_InPlayerInventory)
            {
                hoursBurned *= KeroseneLampOptions.placed_burn_multiplier;
            }
            else
            {
                hoursBurned *= KeroseneLampOptions.held_burn_multiplier;
            }
                
        }
    }
}
