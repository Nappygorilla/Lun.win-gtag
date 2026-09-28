using UnityEngine;

namespace StupidTemplate
{
    public static class ThemeManager
    {
        public static readonly string[] ThemeNames = { "Lunar", "Monochrome", "Rainbow" };

        public static void NextTheme()
        {
            Settings.themeIndex = (Settings.themeIndex + 1) % ThemeNames.Length;
            ApplyTheme(Settings.themeIndex);
        }

        public static void ApplyTheme(int index)
        {
            index = Mathf.Clamp(index, 0, ThemeNames.Length - 1);
            Settings.themeIndex = index;

            switch (index)
            {
                case 0:
                    Settings.backgroundColor = new Classes.ExtGradient
                    {
                        colors = Classes.ExtGradient.GetSimpleGradient(new Color32(8, 8, 10, 255), new Color32(35, 35, 40, 255))
                    };
                    Settings.buttonColors = new[]
                    {
                        new Classes.ExtGradient { colors = Classes.ExtGradient.GetSolidGradient(new Color32(12, 12, 14, 255)) },
                        new Classes.ExtGradient { colors = Classes.ExtGradient.GetSolidGradient(new Color32(225, 225, 230, 255)) }
                    };
                    Settings.textColors = new[] { Color.white, Color.black };
                    break;

                case 1:
                    Settings.backgroundColor = new Classes.ExtGradient
                    {
                        colors = Classes.ExtGradient.GetSimpleGradient(Color.black, new Color32(70, 70, 70, 255))
                    };
                    Settings.buttonColors = new[]
                    {
                        new Classes.ExtGradient { colors = Classes.ExtGradient.GetSolidGradient(new Color32(20, 20, 20, 255)) },
                        new Classes.ExtGradient { colors = Classes.ExtGradient.GetSolidGradient(Color.white) }
                    };
                    Settings.textColors = new[] { Color.white, Color.black };
                    break;

                default:
                    Settings.backgroundColor = new Classes.ExtGradient { rainbow = true };
                    Settings.buttonColors = new[]
                    {
                        new Classes.ExtGradient { colors = Classes.ExtGradient.GetSolidGradient(Color.black) },
                        new Classes.ExtGradient { rainbow = true }
                    };
                    Settings.textColors = new[] { Color.white, Color.white };
                    break;
            }
        }
    }
}
