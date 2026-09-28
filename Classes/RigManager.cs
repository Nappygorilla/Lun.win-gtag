using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace StupidTemplate.Classes
{
    public class RigManager
    {
        public static VRRig GetVRRigFromPlayer(Player p)
        {
            if (p == null || GorillaGameManager.instance == null)
                return null;

            return GorillaGameManager.instance.FindPlayerVRRig(p);
        }

        public static VRRig GetRandomVRRig(bool includeSelf)
        {
            if (VRRigCache.ActiveRigs == null || VRRigCache.ActiveRigs.Count == 0)
                return null;

            if (includeSelf || VRRigCache.ActiveRigs.Count == 1)
                return VRRigCache.ActiveRigs[Random.Range(0, VRRigCache.ActiveRigs.Count)];

            int attempts = VRRigCache.ActiveRigs.Count;
            while (attempts-- > 0)
            {
                VRRig random = VRRigCache.ActiveRigs[Random.Range(0, VRRigCache.ActiveRigs.Count)];
                if (random != null && random != VRRig.LocalRig)
                    return random;
            }

            return null;
        }

        public static VRRig GetClosestVRRig()
        {
            if (VRRigCache.ActiveRigs == null || GorillaTagger.Instance?.bodyCollider == null)
                return null;

            float closestDistance = float.MaxValue;
            VRRig closestRig = null;
            Vector3 origin = GorillaTagger.Instance.bodyCollider.transform.position;

            foreach (VRRig vrrig in VRRigCache.ActiveRigs)
            {
                if (vrrig == null)
                    continue;

                float distance = Vector3.Distance(origin, vrrig.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestRig = vrrig;
                }
            }

            return closestRig;
        }

        public static PhotonView GetPhotonViewFromVRRig(VRRig p)
        {
            if (p == null)
                return null;

            try
            {
                return (PhotonView)Traverse.Create(p).Field("photonView").GetValue();
            }
            catch
            {
                return null;
            }
        }

        public static Player GetRandomPlayer(bool includeSelf)
        {
            Player[] players = includeSelf ? PhotonNetwork.PlayerList : PhotonNetwork.PlayerListOthers;
            if (players == null || players.Length == 0)
                return null;

            return players[Random.Range(0, players.Length)];
        }

        public static Player GetPlayerFromVRRig(VRRig p)
        {
            PhotonView view = GetPhotonViewFromVRRig(p);
            return view != null ? view.Owner : null;
        }

        public static Player GetPlayerFromID(string id)
        {
            Player found = null;
            foreach (Player target in PhotonNetwork.PlayerList)
            {
                if (target.UserId == id)
                {
                    found = target;
                    break;
                }
            }
            return found;
        }

        public static Color GetPlayerColor(VRRig Player)
        {
            if (Player == null)
                return Color.white;

            switch (Player.setMatIndex)
            {
                case 1:
                    return Color.red;
                case 2:
                case 11:
                    return new Color32(255, 128, 0, 255);
                case 3:
                case 7:
                    return Color.blue;
                case 12:
                    return Color.green;
                default:
                    return Player.playerColor;
            }
        }
    }
}