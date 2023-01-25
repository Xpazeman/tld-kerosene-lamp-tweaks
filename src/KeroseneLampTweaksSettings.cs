using ModSettings;
using System;
using System.IO;
using System.Reflection;

namespace KeroseneLampTweaks
{
    public enum LampColor
    {
        Default, Red, Yellow, Blue, Cyan, Green, Purple, White, Custom
    }

    internal class KeroseneLampSettings : JsonModSettings
    {
        [Section("Burn Rate")]

        [Name("Rate of burn for placed lamps")]
        [Description("At what rate the fuel of a lamp will be consumed when placed. 1 is default (4 hours), 0 makes lamps not consume fuel when placed, and 2 doubles the consumption.")]
        [Slider(0f, 2f, NumberFormat = "{0:F2}")]
        public float placed_burn_multiplier = 1f;

        [Name("Rate of burn for held lamps")]
        [Description("At what rate the fuel of a lamp will be consumed when held. 1 is default (4 hours), 0 makes lamps not consume fuel when equipped, and 2 doubles the consumption.")]
        [Slider(0f, 2f, NumberFormat = "{0:F2}")]
        public float held_burn_multiplier = 1f;

        [Section("Light Settings")]

        [Name("Lamp Light Range Modifier")]
        [Description("How far the light from lamps is cast. e.g. 1 is default (20m outside, 25m inside), 0 makes lamps not cast light, and 2 doubles the distance.")]
        [Slider(0f, 2f, NumberFormat = "{0:F2}")]
        public float lamp_range = 1f;

        [Name("Lamp Light Color")]
        [Description("Color for the lamp light.")]
        [Choice("Default (Orange)", "Red", "Yellow", "Blue", "Cyan", "Green", "Purple", "White", "Custom")]
        public LampColor lampColor = LampColor.Default;

        [Name("Lamp Color Red")]
        [Slider(0, 255)]
        public int lampColorR = 0;

        [Name("Lamp Color Green")]
        [Slider(0, 255)]
        public int lampColorG = 0;

        [Name("Lamp Color Blue")]
        [Slider(0, 255)]
        public int lampColorB = 0;

        [Name("Different Color for Spelunkers Lamp")]
        [Description("Turn this on to color spelunker's lamp a different color.")]
        public bool spelunkerColor = false;

        [Name("Spelunkers Lamp Light Color")]
        [Description("Color for the Spelunkers lamp light.")]
        [Choice("Default (Orange)", "Red", "Yellow", "Blue", "Cyan", "Green", "Purple", "White", "Custom")]
        public LampColor spelunkersLampColor = LampColor.Default;

        [Name("Spelunkers Lamp Color Red")]
        [Slider(0, 255)]
        public int spelunkersLampColorR = 0;

        [Name("Spelunkers Lamp Color Green")]
        [Slider(0, 255)]
        public int spelunkersLampColorG = 0;

        [Name("Spelunkers Lamp Color Blue")]
        [Slider(0, 255)]
        public int spelunkersLampColorB = 0;

        [Section("Misc")]

        [Name("Mute lamps audio")]
        [Description("This enables lamps to be silent when turned on and placed.")]
        public bool muteLamps = false;

        protected override void OnChange(FieldInfo field, object oldVal, object newVal)
        {
            RefreshFields();
        }

        internal void RefreshFields()
        {
            if (lampColor == LampColor.Custom)
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

            SetFieldVisible(nameof(spelunkersLampColor), spelunkerColor);

            if (spelunkersLampColor == LampColor.Custom)
            {
                SetFieldVisible(nameof(spelunkersLampColorR), true);
                SetFieldVisible(nameof(spelunkersLampColorG), true);
                SetFieldVisible(nameof(spelunkersLampColorB), true);
            }
            else
            {
                SetFieldVisible(nameof(spelunkersLampColorR), false);
                SetFieldVisible(nameof(spelunkersLampColorG), false);
                SetFieldVisible(nameof(spelunkersLampColorB), false);
            }
        }
    }

    internal static class Settings
    {
        public static KeroseneLampSettings options;

        public static void OnLoad()
        {
            options = new KeroseneLampSettings();
            options.RefreshFields();
            options.AddToModSettings("Kerosene Lamps Settings");
        }
    }
}
