using System.IO;
using System.Reflection;
using UnityEngine;
using Harmony;

namespace KeroseneLampTweaks
{
    class KeroseneLampTweaks
    {
        public static string mods_folder;
        public static string mod_options_folder;
        public static string options_folder_name = "xpazeman-minimods";
        public static string options_file_name = "config-lamps.json";

        public static void OnLoad()
        {
            Debug.Log("[kerosene-lamp-tweaks] Version " + Assembly.GetExecutingAssembly().GetName().Version);

            mods_folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            mod_options_folder = Path.Combine(mods_folder, options_folder_name);
        }

        /*public static void TurnOnLamp(ref KeroseneLampItem lamp)
        {
            if (!lamp.IsOn())
            {
                lamp.TurnOn(true);
                lamp.HideEffects(false);
                lamp.m_TurnOnEffectsTimer = Time.time + lamp.m_TurnOnEffectsDelay + 1f;
            }
            else
            {
                if (KeroseneLampOptions.mute_lamps)
                    lamp.StopLoopingAudio();
            }
        }

        public static void TurnOffLamp(ref KeroseneLampItem lamp)
        {
            if (!GameManager.m_ActiveScene.Contains("Cave") && !GameManager.m_ActiveScene.Contains("Basement") && !GameManager.m_ActiveScene.Contains("Mine"))
            {
                if (lamp.IsOn())
                {
                    lamp.TurnOff(false);
                }
            }
        }*/

        public static void ColorLamps(GameObject lamp)
        {
            

            if (KeroseneLampOptions.lamp_color != LampColor.Default)
            {
                Color newColor = GetNewColor();                

                foreach (Light light in lamp.GetComponentsInChildren<Light>())
                {
                    light.color = newColor;
                }

                foreach (Light light in lamp.GetComponents<Light>())
                {
                    light.color = newColor;
                }

                /*indoor.color = new Color32(255, 105, 92, 255);
                indoorCore.color = new Color32(255, 105, 92, 255);
                outdoor.color = new Color32(255, 105, 92, 255);*/
            }
        }

        public static Color GetNewColor()
        {
            Color newColor = new Color(0.993f, 0.670f, 0.369f, 1.000f);

            switch (KeroseneLampOptions.lamp_color)
            {
                case LampColor.Red:
                    newColor = new Color32(255, 105, 92, 255);
                    break;

                case LampColor.Yellow:
                    newColor = new Color32(255, 228, 92, 255);
                    break;

                case LampColor.Blue:
                    newColor = new Color32(92, 105, 255, 255);
                    break;

                case LampColor.Cyan:
                    newColor = new Color32(92, 225, 255, 255);
                    break;

                case LampColor.Green:
                    newColor = new Color32(91, 216, 95, 255);
                    break;

                case LampColor.Purple:
                    newColor = new Color32(208, 91, 216, 255);
                    break;

                case LampColor.White:
                    newColor = new Color32(255, 255, 255, 255);
                    break;

                case LampColor.Custom:
                    newColor = new Color32((byte)KeroseneLampOptions.lampColorR, (byte)KeroseneLampOptions.lampColorG, (byte)KeroseneLampOptions.lampColorB, 255);
                    break;
            }

            return newColor;
        }
    }
}
