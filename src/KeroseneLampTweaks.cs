using System.IO;
using System.Reflection;
using ModSettings;
using UnityEngine;

namespace KeroseneLampTweaks
{
    public class KeroseneLampOptions
    {
        public static float placed_burn_multiplier = 1f;
        public static float held_burn_multiplier = 1f;
    }

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

        internal class KeroseneLampSettings : ModSettingsBase
        {
            [Section("Kerosene Lamp Settings")]

            [Name("Rate of burn for placed lamps")]
            [Description("At what rate the fuel of a lamp will be consumed when placed. 1 is default, 0 makes lamps not consume fuel when placed, and 2 doubles the consumption.")]
            [Slider(0f, 2f)]
            public float placed_burn_multiplier = 1f;

            [Name("Rate of burn for held lamps")]
            [Description("At what rate the fuel of a lamp will be consumed when held. 1 is default, 0 makes lamps not consume fuel when equipped, and 2 doubles the consumption.")]
            [Slider(0f, 2f)]
            public float held_burn_multiplier = 1f;

            protected override void OnConfirm()
            {
                KeroseneLampOptions.placed_burn_multiplier = placed_burn_multiplier;

                string json_opts = FastJson.Serialize(this);

                File.WriteAllText(Path.Combine(mod_options_folder, options_file_name), json_opts);
            }
        }

        internal static class GearDecaySettingsLoad
        {
            private static KeroseneLampSettings custom_settings = new KeroseneLampSettings();

            public static void OnLoad()
            {
                if (File.Exists(Path.Combine(mod_options_folder, options_file_name)))
                {
                    string opts = File.ReadAllText(Path.Combine(mod_options_folder, options_file_name));
                    custom_settings = FastJson.Deserialize<KeroseneLampSettings>(opts);

                    KeroseneLampOptions.placed_burn_multiplier = custom_settings.placed_burn_multiplier;
                }

                custom_settings.AddToModSettings("Xpazeman Mini Mods");
            }
        }
    }
}
