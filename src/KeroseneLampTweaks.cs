using System.IO;
using System.Reflection;
using UnityEngine;
using MelonLoader;

namespace KeroseneLampTweaks
{
    class KeroseneLampTweaks : MelonMod
    {
        
        public override void OnApplicationStart()
        {
            Debug.Log("[kerosene-lamp-tweaks] Version " + Assembly.GetExecutingAssembly().GetName().Version);

            Settings.OnLoad();
        }

        public static void ColorLamps(GameObject lamp)
        {
            
            if (Settings.options.lamp_color != LampColor.Default)
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
            }
        }

        public static Color GetNewColor()
        {
            Color newColor = new Color(0.993f, 0.670f, 0.369f, 1.000f);

            switch (Settings.options.lamp_color)
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
                    newColor = new Color32((byte)Settings.options.lampColorR, (byte)Settings.options.lampColorG, (byte)Settings.options.lampColorB, 255);
                    break;
            }

            return newColor;
        }
    }
}
