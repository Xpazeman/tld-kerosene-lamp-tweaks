using ModSettings;
using System;
using System.IO;
using System.Reflection;

namespace KeroseneLampTweaks
{
    public class KeroseneLampOptions
    {
        public static float placed_burn_multiplier = 1f;
        public static float held_burn_multiplier = 1f;

        public static float lamp_range = 1f;
        public static LampColor lamp_color = LampColor.Default;
        public static int lampColorR = 0;
        public static int lampColorG = 0;
        public static int lampColorB = 0;

        public static bool mute_lamps = false;

        /*public static bool auto_on = false;
        public static bool auto_off = false;
        public static int hour_on = 18;
        public static int minute_on = 30;
        public static int hour_off = 7;
        public static int minute_off = 15;*/
    }

    public enum LampColor
    {
        Default, Red, Yellow, Blue, Cyan, Green, Purple, White, Custom
    }

    internal class KeroseneLampSettings : ModSettingsBase
    {
        [Section("Base Settings")]

        [Name("Rate of burn for placed lamps")]
        [Description("At what rate the fuel of a lamp will be consumed when placed. 1 is default (4 hours), 0 makes lamps not consume fuel when placed, and 2 doubles the consumption.")]
        [Slider(0f, 2f, NumberFormat = "{0:F2}")]
        public float placed_burn_multiplier = 1f;

        [Name("Rate of burn for held lamps")]
        [Description("At what rate the fuel of a lamp will be consumed when held. 1 is default (4 hours), 0 makes lamps not consume fuel when equipped, and 2 doubles the consumption.")]
        [Slider(0f, 2f, NumberFormat = "{0:F2}")]
        public float held_burn_multiplier = 1f;

        [Name("Lamp Light Range Modifier")]
        [Description("How far the light from lamps is cast. e.g. 1 is default (20m outside, 25m inside), 0 makes lamps not cast light, and 2 doubles the distance.")]
        [Slider(0f, 2f, NumberFormat = "{0:F2}")]
        public float lamp_range = 1f;

        [Name("Lamp Light Color")]
        [Description("Color for the lamp light.")]
        [Choice("Default (Orange)", "Red", "Yellow", "Blue", "Cyan", "Green", "Purple", "White", "Custom")]
        public LampColor lamp_color = LampColor.Default;

        [Name("Lamp Color Red")]
        [Slider(0, 255)]
        public int lampColorR = 0;

        [Name("Lamp Color Green")]
        [Slider(0, 255)]
        public int lampColorG = 0;

        [Name("Lamp Color Blue")]
        [Slider(0, 255)]
        public int lampColorB = 0;


        [Name("Mute lamps audio")]
        [Description("This enables lamps to be silent when turned on and placed.")]
        public bool mute_lamps = false;

        /*[Section("Auto Lamps")]

        [Name("Make lamps automatic")]
        [Description("This enables lamps to turn on automatically between set times.")]
        public bool auto_on = false;

        [Name("Make lamps auto turn off")]
        [Description("This enables lamps to turn off automatically after off time.\nWARNING!: Setting this to 'Yes' will make placed lamps ALWAYS be turned off if outside of the 'on' hours, except in mines, caves, and basements.")]
        public bool auto_off = false;

        [Name("Turn on hour")]
        [Description("During which hour lamps should turn on.")]
        [Slider(0, 23)]
        public int hour_on = 18;

        [Name("Turn on minutes")]
        [Description("Minutes on the turn on hour when lamps should turn on.")]
        [Slider(0, 59)]
        public int minute_on = 30;

        [Name("Turn off hour")]
        [Description("During which hour lamps should turn off.")]
        [Slider(0, 23)]
        public int hour_off = 7;

        [Name("Turn off minutes")]
        [Description("Minutes on the turn off hour when lamps should turn off.")]
        [Slider(0, 59)]
        public int minute_off = 15;*/

        protected override void OnConfirm()
        {
            KeroseneLampOptions.placed_burn_multiplier = (float)Math.Round(placed_burn_multiplier, 2);
            KeroseneLampOptions.held_burn_multiplier = (float)Math.Round(held_burn_multiplier, 2);

            KeroseneLampOptions.lamp_range = (float)Math.Round(lamp_range, 2);

            KeroseneLampOptions.lamp_color = lamp_color;

            KeroseneLampOptions.lampColorR = lampColorR;
            KeroseneLampOptions.lampColorG = lampColorG;
            KeroseneLampOptions.lampColorB = lampColorB;

            KeroseneLampOptions.mute_lamps = mute_lamps;

            /*KeroseneLampOptions.auto_on = auto_on;
            KeroseneLampOptions.auto_off = auto_off;
            

            KeroseneLampOptions.hour_on = hour_on;
            KeroseneLampOptions.minute_on = minute_on;
            KeroseneLampOptions.hour_off = hour_off;
            KeroseneLampOptions.minute_off = minute_off;*/

            string json_opts = FastJson.Serialize(this);

            File.WriteAllText(Path.Combine(KeroseneLampTweaks.mod_options_folder, KeroseneLampTweaks.options_file_name), json_opts);
        }

        protected override void OnChange(FieldInfo field, object oldVal, object newVal)
        {
            if (field.Name == nameof(lamp_color))
            {
                ChangeColorPreset((LampColor)newVal);
            }
            else if (field.Name == nameof(lampColorR) || field.Name == nameof(lampColorG) || field.Name == nameof(lampColorB))
            {
                lamp_color = LampColor.Custom;
            }

            RefreshFields();
        }

        internal void RefreshFields()
        {
            if (lamp_color == LampColor.Custom)
            {
                SetFieldVisible(nameof(lampColorR), true);
                SetFieldVisible(nameof(lampColorG), true);
                SetFieldVisible(nameof(lampColorB), true);
            }
            else
            {
                SetFieldVisible(nameof(lampColorR), false);
                SetFieldVisible(nameof(lampColorG), false);
                SetFieldVisible(nameof(lampColorB), false);
            }
            /*SetFieldVisible(nameof(auto_off), auto_on);

            SetFieldVisible(nameof(hour_on), auto_on);
            SetFieldVisible(nameof(minute_on), auto_on);

            SetFieldVisible(nameof(hour_off), auto_on);
            SetFieldVisible(nameof(minute_off), auto_on);*/
        }

        internal void ChangeColorPreset(LampColor newPreset)
        {

        }
    }

    internal static class GearDecaySettingsLoad
    {
        private static KeroseneLampSettings custom_settings = new KeroseneLampSettings();

        public static void OnLoad()
        {
            if (File.Exists(Path.Combine(KeroseneLampTweaks.mod_options_folder, KeroseneLampTweaks.options_file_name)))
            {
                string opts = File.ReadAllText(Path.Combine(KeroseneLampTweaks.mod_options_folder, KeroseneLampTweaks.options_file_name));
                custom_settings = FastJson.Deserialize<KeroseneLampSettings>(opts);

                KeroseneLampOptions.placed_burn_multiplier = custom_settings.placed_burn_multiplier;
                KeroseneLampOptions.held_burn_multiplier = custom_settings.held_burn_multiplier;

                KeroseneLampOptions.lamp_range = custom_settings.lamp_range;

                KeroseneLampOptions.lamp_color = custom_settings.lamp_color;

                KeroseneLampOptions.lampColorR = custom_settings.lampColorR;
                KeroseneLampOptions.lampColorG = custom_settings.lampColorG;
                KeroseneLampOptions.lampColorB = custom_settings.lampColorB;

                /*KeroseneLampOptions.auto_on = custom_settings.auto_on;
                KeroseneLampOptions.auto_off = custom_settings.auto_off;
                KeroseneLampOptions.mute_lamps = custom_settings.mute_lamps;

                KeroseneLampOptions.hour_on = custom_settings.hour_on;
                KeroseneLampOptions.minute_on = custom_settings.minute_on;

                KeroseneLampOptions.hour_off = custom_settings.hour_off;
                KeroseneLampOptions.minute_off = custom_settings.minute_off;*/

                custom_settings.RefreshFields();
            }

            custom_settings.AddToModSettings("Kerosene Lamp Tweaks");
        }
    }
}
