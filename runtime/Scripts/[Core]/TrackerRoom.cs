
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Lastation.PlayerTrackerV2
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TrackerRoom : UdonSharpBehaviour
    {
        public string roomName;
        public Transform[] probes;
        public int playersInRoom;

        public float RoomProbeCheck(VRCPlayerApi _player)
        {
            float smallestDistance = float.PositiveInfinity;
            for (int i = 0; i < probes.Length; i++)
            {
                if (!probes[i]) continue;

                float distance = Vector3.Distance(_player.GetPosition(), probes[i].position);
                
                if (distance < smallestDistance)
                    smallestDistance = distance;
            }
            return smallestDistance;
        }
    }

}
