using System;
using Harmony;
using UnityEngine;

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

    [HarmonyPatch(typeof(KeroseneLampItem), "Update")]
    public class KeroseneLampItem_Update
    {
        private const float INDOOR_DEF_RNG = 25f;
        private const float INDOORCORE_DEF_RNG = 0.12f;
        private const float OUTDOOR_DEF_RNG = 20f;

        public static void Postfix(ref KeroseneLampItem __instance)
        {
            var gi = Traverse.Create(__instance).Field("m_GearItem").GetValue<GearItem>();

            if (!gi.m_InPlayerInventory)
            {
                if (KeroseneLampOptions.mute_lamps)
                    __instance.StopLoopingAudio();

                /*var time = GameManager.GetTimeOfDayComponent();
                int hour = time.GetHour();
                int minute = time.GetMinutes();

                int curTime = (hour * 100) + minute;

                int onTime = (KeroseneLampOptions.hour_on * 100) + KeroseneLampOptions.minute_on;
                int offTime = (KeroseneLampOptions.hour_off * 100) + KeroseneLampOptions.minute_off;

                if (offTime < onTime)
                {
                    if ((curTime >= 0 && curTime < offTime) || (curTime <= 2400 && curTime > onTime))
                    {
                        if (KeroseneLampOptions.auto_on)
                            KeroseneLampTweaks.TurnOnLamp(ref __instance);
                    }
                    else
                    {
                        if (KeroseneLampOptions.auto_off)
                            KeroseneLampTweaks.TurnOffLamp(ref __instance);
                    }
                }
                else
                {
                    if ((curTime >= 0 && curTime < onTime) || (curTime <= 2400 && curTime > offTime))
                    {
                        if (KeroseneLampOptions.auto_off)
                            KeroseneLampTweaks.TurnOffLamp(ref __instance);
                    }
                    else
                    {
                        if (KeroseneLampOptions.auto_on)
                            KeroseneLampTweaks.TurnOnLamp(ref __instance);
                    }
                }*/
            }

            Light indoor = Traverse.Create(__instance).Field("m_LightIndoor").GetValue<Light>();
            Light indoorCore = Traverse.Create(__instance).Field("m_LightIndoorCore").GetValue<Light>();
            Light outdoor = Traverse.Create(__instance).Field("m_LightOutdoor").GetValue<Light>();

            indoor.range = INDOOR_DEF_RNG * KeroseneLampOptions.lamp_range;
            indoorCore.range = INDOORCORE_DEF_RNG * KeroseneLampOptions.lamp_range;
            outdoor.range = OUTDOOR_DEF_RNG * KeroseneLampOptions.lamp_range;
        }
        
    }

    [HarmonyPatch(typeof(FirstPersonLightSource), "TurnOnEffects")]
    internal class FirstPersonLightSource_Start
    {
        private const float INDOOR_DEF_RNG = 25f;
        private const float OUTDOOR_DEF_RNG = 20f;

        public static void Prefix(FirstPersonLightSource __instance)
        {

            if (__instance.gameObject.name.Contains("KerosceneLamp")){
                __instance.m_LightIndoor.range = INDOOR_DEF_RNG * KeroseneLampOptions.lamp_range;
                __instance.m_LightOutdoor.range = OUTDOOR_DEF_RNG * KeroseneLampOptions.lamp_range;

                KeroseneLampTweaks.ColorLamps(__instance.gameObject);
            }
        }
    }

    [HarmonyPatch(typeof(KeroseneLampIntensity), "Update")]
    internal class KeroseneLampIntensity_Update
    {
        public static void Prefix(KeroseneLampIntensity __instance)
        {
            Gradient gradient = new Gradient();
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;

            colorKey = new GradientColorKey[2];
            colorKey[0].color = KeroseneLampTweaks.GetNewColor();
            colorKey[0].time = 0.0f;
            colorKey[1].color = KeroseneLampTweaks.GetNewColor();
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
            var gi = Traverse.Create(__instance).Field("m_GearItem").GetValue<GearItem>();

            if (gi)
            {
                keroseneLampItem = gi.m_KeroseneLampItem;
                KeroseneLampTweaks.ColorLamps(keroseneLampItem.gameObject);
            }
        }
    }
}
