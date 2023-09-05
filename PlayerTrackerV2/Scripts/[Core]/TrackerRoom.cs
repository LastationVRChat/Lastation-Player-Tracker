
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Lastation.PlayerTrackerV2
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TrackerRoom : UdonSharpBehaviour
    {
        public string roomName;
        [HideInInspector] public float distance;
        public Transform[] probes;
        public int playersInRoom;


        private float tmpDistance;
        public void RoomProbeCheck(VRCPlayerApi _player)
        {
            distance = float.PositiveInfinity;
            for (int i = 0; i < probes.Length; i++)
            {
                if (probes[i] == null) continue;
                tmpDistance = Vector3.Distance(_player.GetPosition(), probes[i].position);
                distance = (tmpDistance < distance) ? tmpDistance : distance;

                /*
                same as above but as a if statement
                if (tmpDistance < distance)
                {
                distance = tmpDistance;
                }
                else
                {
                distance = distance;
                }
                */
            }
            return;
        }
        public int GetPlayerCount()
        {
            return playersInRoom;
        }
    }

}
