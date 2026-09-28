using StupidTemplate.Classes;
using UnityEngine;

namespace StupidTemplate
{
    public class Settings
    {
        /*
         * These are the settings for the menu.
         * 
         * To change the colors, you need to modify the ExtGradient variables.
         * Here are some examples on how to use ExtGradient:
         * 
         * Solid Color:
         *  new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) }
         *  
         * Simple Gradient:
         *  new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, Color.white) }
         * 
         * Rainbow Color:
         *   new ExtGradient { rainbow = true }
         *   
         * Epileptic Color (random color every frame):
         *   new ExtGradient { epileptic = true }
         *   
         * Self Color:
         *   new ExtGradient { copyRigColor = true }
         *   
         * To change the font, you may use the following code:
         *   Font.CreateDynamicFontFromOSFont("Comic Sans MS", 24)
         */

        public static ExtGradient backgroundColor = new ExtGradient { rainbow = true};
        public static ExtGradient[] buttonColors = new ExtGradient[]
        {
            new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) }, // Disabled
            new ExtGradient { rainbow = true } // Enabled
        };
        public static Color[] textColors = new Color[]
        {
            Color.white, // Disabled
            Color.white // Enabled
        };

        public static Font currentFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

        public static bool fpsCounter = true;
        public static bool disconnectButton = true;
        public static bool rightHanded;
        public static bool disableNotifications;

        public static KeyCode keyboardButton = KeyCode.Q;
        public static float notificationDurationSeconds = 3f;

        public static readonly KeyCode[] menuKeys = { KeyCode.Q, KeyCode.E, KeyCode.Tab, KeyCode.F2 };
        public static readonly string[] menuKeyNames = { "Q", "E", "TAB", "F2" };

        public static Vector3 menuSize = new Vector3(0.1f, 1f, 1f); // Depth, width, height
        public static int buttonsPerPage = 8;

        public static float gradientSpeed = 0.5f; // Speed of colors

        public static readonly string[] menuScaleNames = { "Small", "Normal", "Large", "Huge" };
        public static int menuScaleIndex = 1;
        public static int themeIndex = 0;


        public static KeyCode GetValidMenuKey(int value)
        {
            for (int i = 0; i < menuKeys.Length; i++)
            {
                if ((int)menuKeys[i] == value)
                    return menuKeys[i];
            }

            return KeyCode.Q;
        }

        public static void NextKeyboardButton()
        {
            int index = 0;
            for (int i = 0; i < menuKeys.Length; i++)
            {
                if (menuKeys[i] == keyboardButton)
                {
                    index = i;
                    break;
                }
            }

            index = (index + 1) % menuKeys.Length;
            keyboardButton = menuKeys[index];
            Classes.ButtonInfo button = Menu.Main.GetIndex("Menu Key");
            if (button != null)
                button.overlapText = "Menu Key [" + menuKeyNames[index] + "]";
        }

        public static void NextMenuScale()
        {
            menuScaleIndex = (menuScaleIndex + 1) % menuScaleNames.Length;
            ApplyMenuScale();
            Classes.ButtonInfo button = Menu.Main.GetIndex("Menu Scale");
            if (button != null)
                button.overlapText = "Menu Scale [" + menuScaleNames[menuScaleIndex] + "]";
        }

        public static void NextGradientSpeed()
        {
            float[] speeds = { 0.25f, 0.5f, 0.75f, 1f };
            int index = 0;
            float closest = float.MaxValue;
            for (int i = 0; i < speeds.Length; i++)
            {
                float distance = Mathf.Abs(gradientSpeed - speeds[i]);
                if (distance < closest)
                {
                    closest = distance;
                    index = i;
                }
            }

            index = (index + 1) % speeds.Length;
            gradientSpeed = speeds[index];
            Classes.ButtonInfo button = Menu.Main.GetIndex("Gradient Speed");
            if (button != null)
                button.overlapText = "Gradient Speed [" + gradientSpeed.ToString("0.##") + "]";
        }

        public static void ApplyMenuScale()
        {
            float[] scales = { 0.8f, 1f, 1.2f, 1.4f };
            menuScaleIndex = Mathf.Clamp(menuScaleIndex, 0, scales.Length - 1);
            float scale = scales[menuScaleIndex];
            menuSize = new Vector3(0.1f, 1f, 1f) * scale;
        }
    }
}
