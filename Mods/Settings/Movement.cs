using StupidTemplate.Classes;
using static StupidTemplate.Menu.Main;

namespace StupidTemplate.Mods.Settings
{
    public class Movement
    {
        public static int flySpeedIndex = 2;
        public static float flySpeed = 15f;

        private static readonly string[] speedNames = { "Very Slow", "Slow", "Normal", "Fast", "Very Fast", "Extreme" };
        private static readonly float[] speedValues = { 5f, 10f, 15f, 20f, 30f, 50f };

        public static void ApplyFlySpeed()
        {
            flySpeedIndex = System.Math.Max(0, System.Math.Min(flySpeedIndex, speedValues.Length - 1));
            flySpeed = speedValues[flySpeedIndex];
        }

        public static void ChangeFlySpeed()
        {

            flySpeedIndex++;
            flySpeedIndex %= speedNames.Length;
            ApplyFlySpeed();

            ButtonInfo button = GetIndex("Change Fly Speed");
            if (button != null)
                button.overlapText = $"Change Fly Speed [{speedNames[flySpeedIndex]}]";
        }
    }
}
