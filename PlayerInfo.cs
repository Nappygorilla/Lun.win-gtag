using System;
using Photon.Pun;
using Photon.Realtime;
using StupidTemplate.Classes;
using StupidTemplate.Notifications;
using UnityEngine;
using static StupidTemplate.Menu.Buttons;

namespace StupidTemplate
{
    public static class PlayerInfo
    {
        private static readonly Player[] cachedPlayers = new Player[6];

        public static void RefreshData()
        {
            Array.Clear(cachedPlayers, 0, cachedPlayers.Length);

            Player[] players = PhotonNetwork.PlayerListOthers;
            if (players == null)
                return;

            int count = Mathf.Min(players.Length, cachedPlayers.Length);
            for (int i = 0; i < count; i++)
                cachedPlayers[i] = players[i];

            for (int i = 0; i < cachedPlayers.Length; i++)
            {
                ButtonInfo button = GetIndex("Player " + (i + 1));
                if (button != null)
                {
                    Player player = cachedPlayers[i];
                    button.overlapText = player == null
                        ? "Player " + (i + 1) + " [Empty]"
                        : "Player " + (i + 1) + ": " + SafeName(player.NickName);
                }
            }
        }

        public static void Refresh()
        {
            RefreshData();
            NotifiLib.SendNotification("<color=green>Player list refreshed.</color>");
        }

        public static void ShowPlayer(int index)
        {
            if (index < 0 || index >= cachedPlayers.Length)
                return;

            Player player = cachedPlayers[index];
            if (player == null)
            {
                NotifiLib.SendNotification("<color=grey>Player slot is empty.</color>");
                return;
            }

            string name = SafeName(player.NickName);
            string distance = "unknown";
            VRRig rig = RigManager.GetVRRigFromPlayer(player);
            if (rig != null && GorillaTagger.Instance?.bodyCollider != null)
            {
                float meters = Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, rig.transform.position);
                distance = meters.ToString("0.0") + "m";
            }

            NotifiLib.SendNotification(
                $"<color=white>{name}</color>\\nActor: {player.ActorNumber}\\nDistance: {distance}" + Environment.NewLine);
        }

        public static void ShowRoomSummary()
        {
            Player[] players = PhotonNetwork.PlayerList;
            int count = players == null ? 0 : players.Length;
            string roomName = PhotonNetwork.CurrentRoom != null ? PhotonNetwork.CurrentRoom.Name : "Offline";
            NotifiLib.SendNotification($"<color=white>Room:</color> {roomName}\\nPlayers: {count}" + Environment.NewLine);
        }

        private static string SafeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Unknown";

            return name.Replace("<", "").Replace(">", "");
        }
    }
}
