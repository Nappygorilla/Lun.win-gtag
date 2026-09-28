using BepInEx.Configuration;
using UnityEngine;

namespace StupidTemplate
{
    public static class Configuration
    {
        private static ConfigFile config;

        public static ConfigEntry<bool> RightHanded;
        public static ConfigEntry<bool> FpsCounter;
        public static ConfigEntry<bool> DisconnectButton;
        public static ConfigEntry<bool> Notifications;
        public static ConfigEntry<int> KeyboardButton;
        public static ConfigEntry<float> NotificationDuration;
        public static ConfigEntry<float> GradientSpeed;
        public static ConfigEntry<int> MenuScaleIndex;
        public static ConfigEntry<int> ThemeIndex;
        public static ConfigEntry<int> FlySpeedIndex;

        public static void Initialize(ConfigFile configFile)
        {
            config = configFile;

            RightHanded = config.Bind("Menu", "RightHanded", false, "Places the menu on the right hand when enabled.");
            FpsCounter = config.Bind("Menu", "FpsCounter", true, "Shows the FPS counter in the menu.");
            DisconnectButton = config.Bind("Menu", "DisconnectButton", true, "Shows the disconnect button.");
            Notifications = config.Bind("Menu", "Notifications", true, "Shows plugin notifications.");
            KeyboardButton = config.Bind("Menu", "KeyboardButton", (int)KeyCode.Q, "Keyboard key used to open the menu.");
            NotificationDuration = config.Bind("Menu", "NotificationDuration", 3f, "Seconds each notification remains before it expires.");
            GradientSpeed = config.Bind("Appearance", "GradientSpeed", 0.5f, "Menu gradient animation speed.");
            MenuScaleIndex = config.Bind("Appearance", "MenuScaleIndex", 1, "Menu size preset index.");
            ThemeIndex = config.Bind("Appearance", "ThemeIndex", 0, "Menu theme preset index.");
            FlySpeedIndex = config.Bind("Movement", "FlySpeedIndex", 2, "Fly speed preset index.");

            Settings.rightHanded = RightHanded.Value;
            Settings.fpsCounter = FpsCounter.Value;
            Settings.disconnectButton = DisconnectButton.Value;
            Settings.disableNotifications = !Notifications.Value;
            Settings.keyboardButton = Settings.GetValidMenuKey(KeyboardButton.Value);
            Settings.notificationDurationSeconds = Mathf.Clamp(NotificationDuration.Value, 0.5f, 15f);
            Settings.gradientSpeed = Mathf.Clamp(GradientSpeed.Value, 0.05f, 2f);
            Settings.menuScaleIndex = Mathf.Clamp(MenuScaleIndex.Value, 0, Settings.menuScaleNames.Length - 1);
            Settings.themeIndex = Mathf.Clamp(ThemeIndex.Value, 0, 2);
            Mods.Settings.Movement.flySpeedIndex = Mathf.Clamp(FlySpeedIndex.Value, 0, 5);
            Mods.Settings.Movement.ApplyFlySpeed();
            ThemeManager.ApplyTheme(Settings.themeIndex);
            Settings.ApplyMenuScale();
        }

        public static void Save()
        {
            if (config == null)
                return;

            RightHanded.Value = Settings.rightHanded;
            FpsCounter.Value = Settings.fpsCounter;
            DisconnectButton.Value = Settings.disconnectButton;
            Notifications.Value = !Settings.disableNotifications;
            KeyboardButton.Value = (int)Settings.keyboardButton;
            NotificationDuration.Value = Settings.notificationDurationSeconds;
            GradientSpeed.Value = Settings.gradientSpeed;
            MenuScaleIndex.Value = Settings.menuScaleIndex;
            ThemeIndex.Value = Settings.themeIndex;
            FlySpeedIndex.Value = Mods.Settings.Movement.flySpeedIndex;
            config.Save();
        }

        public static void ResetToDefaults()
        {
            Settings.rightHanded = false;
            Settings.fpsCounter = true;
            Settings.disconnectButton = true;
            Settings.disableNotifications = false;
            Settings.keyboardButton = KeyCode.Q;
            Settings.notificationDurationSeconds = 3f;
            Settings.gradientSpeed = 0.5f;
            Settings.menuScaleIndex = 1;
            Settings.themeIndex = 0;
            Mods.Settings.Movement.flySpeedIndex = 2;
            Mods.Settings.Movement.ApplyFlySpeed();
            ThemeManager.ApplyTheme(Settings.themeIndex);
            Settings.ApplyMenuScale();
            Save();
        }
    }
}
