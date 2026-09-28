using GorillaLocomotion;
using StupidTemplate.Classes;
using StupidTemplate.Notifications;
using UnityEngine;
using static StupidTemplate.Classes.RigManager;
using static StupidTemplate.Menu.Main;

namespace StupidTemplate.Mods
{
    public class Safety
    {
        public static VRRig reportRig; 
        public static void AntiReport(System.Action<VRRig, Vector3> onReport)
        {
            if (NetworkSystem.Instance == null || !NetworkSystem.Instance.InRoom || GorillaParent.instance == null)
                return;

            if (reportRig != null)
            {
            {
                onReport?.Invoke(reportRig, reportRig.transform.position);
                reportRig = null;
                return;
            }

            foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
            {
                if (line.linePlayer != NetworkSystem.Instance.LocalPlayer) continue;
                if (line == null || line.reportButton == null)
                    continue;

                Transform report = line.reportButton.gameObject != null ? line.reportButton.gameObject.transform : null;
                if (report == null)
                    continue;

                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig == null || vrrig.isLocal || vrrig.rightHandTransform == null || vrrig.leftHandTransform == null)
                        continue;

                    float rightDistance = Vector3.Distance(vrrig.rightHandTransform.position, report.position);
                    float leftDistance = Vector3.Distance(vrrig.leftHandTransform.position, report.position);
                    if (rightDistance < 0.35f || leftDistance < 0.35f)
                        onReport?.Invoke(vrrig, report.position);
                }
            }
        }

        public static float antiReportDelay;
        public static void AntiReportDisconnect()
        {
            AntiReport((vrrig, position) =>
            {
                NetworkSystem.Instance.ReturnToSinglePlayer();

                if (!(Time.time > antiReportDelay)) return;
                antiReportDelay = Time.time + 1f;
                var player = GetPlayerFromVRRig(vrrig);
                string playerName = player != null && !string.IsNullOrEmpty(player.NickName) ? player.NickName : "Unknown Player";
                NotifiLib.SendNotification("<color=grey>[</color><color=purple>ANTI-REPORT</color><color=grey>]</color> " + playerName + " attempted to report you, you have been disconnected.");
            });
        }
    }
}
