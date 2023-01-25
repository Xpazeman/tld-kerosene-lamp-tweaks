using HarmonyLib;
using UnityEngine;
using Il2Cpp;
using Il2CppTLD.Gear;

namespace KeroseneLampTweaks
{
    [HarmonyPatch(typeof(KeroseneLampItem), "ReduceFuel", new Type[] { typeof(float) })]
    public class KeroseneLampItem_ReduceFuel
    {
        public static void Prefix(ref KeroseneLampItem __instance, ref float hoursBurned)
        {
            //var gi = Traverse.Create(__instance).Field("m_GearItem").GetValue<GearItem>();
            var gi = __instance.m_GearItem;

            if (!gi.m_InPlayerInventory)
            {
                hoursBurned *= Settings.options.placed_burn_multiplier;
            }
            else
            {
                hoursBurned *= Settings.options.held_burn_multiplier;
            }
        }
    }

    [HarmonyPatch(typeof(KeroseneLampItem), "Update")]
    public class KeroseneLampItem_Update
    {
        private const float INDOOR_DEF_RNG = 25f;
        private const float INDOORCORE_DEF_RNG = 0.12f;
        private const float OUTDOOR_DEF_RNG = 20f;

        public static void Postfix(ref KeroseneLampItem __instance)
        {
            //var gi = Traverse.Create(__instance).Field("m_GearItem").GetValue<GearItem>();
            var gi = __instance.m_GearItem;

            if (!gi.m_InPlayerInventory)
            {
                if (Settings.options.muteLamps)
                    __instance.StopLoopingAudio();
            }

            Light indoor = __instance.m_LightIndoor;
            Light indoorCore = __instance.m_LightIndoorCore;
            Light outdoor = __instance.m_LightOutdoor;

            indoor.range = INDOOR_DEF_RNG * Settings.options.lamp_range;
            indoorCore.range = INDOORCORE_DEF_RNG * Settings.options.lamp_range;
            outdoor.range = OUTDOOR_DEF_RNG * Settings.options.lamp_range;
        }
        
    }

    [HarmonyPatch(typeof(FirstPersonLightSource), "TurnOnEffects")]
    internal class FirstPersonLightSource_Start
    {
        private const float INDOOR_DEF_RNG = 25f;
        private const float OUTDOOR_DEF_RNG = 20f;

        public static void Prefix(FirstPersonLightSource __instance)
        {

            if (__instance.gameObject.name.Contains("KerosceneLamp") || __instance.gameObject.name.Contains("KeroseneLamp"))
            {
                __instance.m_LightIndoor.range = INDOOR_DEF_RNG * Settings.options.lamp_range;
                __instance.m_LightOutdoor.range = OUTDOOR_DEF_RNG * Settings.options.lamp_range;

                KeroseneLampTweaks.ColorLamps(__instance.gameObject);
            }
        }
    }

    [HarmonyPatch(typeof(KeroseneLampIntensity), "Update")]
    internal class KeroseneLampIntensity_Update
    {
        public static void Prefix(KeroseneLampIntensity __instance)
        {
            Color newColor;
            
            if (__instance.gameObject.name.Contains("Spelunkers") && Settings.options.spelunkerColor)
            {
                newColor = KeroseneLampTweaks.GetNewColor(Settings.options.spelunkersLampColor, true);
            }
            else
            {
                newColor = KeroseneLampTweaks.GetNewColor(Settings.options.lampColor);
            }

            Gradient gradient = new Gradient();
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;

            colorKey = new GradientColorKey[2];
            colorKey[0].color = newColor;
            colorKey[0].time = 0.0f;
            colorKey[1].color = newColor;
            colorKey[1].time = 1.0f;

            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);

            //Set Stuff
            __instance.m_GlassColor = gradient;
            __instance.m_FlameColor = gradient;

            if (__instance.m_LitGlass)
            {
                Material glassMat = __instance.m_LitGlass.GetComponent<MeshRenderer>().material;
                glassMat.SetColor("_Emission", __instance.m_GlassColor.Evaluate(0f));
            }


            KeroseneLampItem keroseneLampItem = null;
            var gi = __instance.m_GearItem;

            if (gi)
            {
                keroseneLampItem = gi.m_KeroseneLampItem;
                KeroseneLampTweaks.ColorLamps(keroseneLampItem.gameObject);
            }
        }
    }
}
