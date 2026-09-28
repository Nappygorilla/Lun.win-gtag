using System;
using StupidTemplate.Notifications;
using UnityEngine;

namespace StupidTemplate
{
    public static class Diagnostics
    {
        public static void ShowStatus()
        {
            string patchState = Patches.PatchHandler.IsPatched
                ? (Patches.PatchHandler.HasPatchErrors ? "PARTIAL" : "OK")
                : "NOT RUN";

            NotifiLib.SendNotification(
                $"<color=white>Lun.win status</color>\\n" +
                $"Game: {Application.version}\\n" +
                $"Plugin: {PluginInfo.Version}\\n" +
                $"Patches: {patchState} ({Patches.PatchHandler.PatchErrors} errors)" + Environment.NewLine);
        }

        public static void ShowPatchErrors()
        {
            if (!Patches.PatchHandler.HasPatchErrors)
            {
                NotifiLib.SendNotification("<color=green>Patch diagnostics:</color> no patch errors recorded.");
                return;
            }

            int shown = 0;
            foreach (string failure in Patches.PatchHandler.PatchFailures)
            {
                NotifiLib.SendNotification("<color=red>Patch:</color> " + failure);
                if (++shown >= 5)
                    break;
            }
        }

        public static void ResetSettings()
        {
            Configuration.ResetToDefaults();
            NotifiLib.SendNotification("<color=green>Settings reset to defaults.</color>");
        }

        public static void SaveSettings()
        {
            Configuration.Save();
            NotifiLib.SendNotification("<color=green>Settings saved.</color>");
        }

        public static void ClearNotifications()
        {
            NotifiLib.ClearAllNotifications();
        }
    }
}
