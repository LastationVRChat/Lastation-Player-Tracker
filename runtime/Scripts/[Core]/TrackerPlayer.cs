
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;


namespace Lastation.PlayerTrackerV2
{
    public class TrackerPlayer : UdonSharpBehaviour
    {
        public bool isUsed;
        [UdonSynced] public int playerId = -1;
        public TrackerMain controller;
        private bool isLocalTracker = false;
        public VRCPlayerApi thisOwner;
        public TrackerRoom currentRoom;
        [UdonSynced] public int currentRoomIndex = -1;
        private TrackerRoom oldRoom;
        public string playerName;
        
        void Start()
        {

        }
        
        public bool _TryGetTracker()
        {
            if (isUsed) return false;
            if (VRC.SDKBase.Utilities.IsValid(VRCPlayerApi.GetPlayerById(playerId)))
            {
                return false;
            }
            else
            {
                Networking.SetOwner(Networking.LocalPlayer,gameObject);
                thisOwner = Networking.LocalPlayer;
                playerId = thisOwner.playerId;
                controller.localTracker = this;
                isLocalTracker = true;
                currentRoomIndex = controller.GetRoomIndex(currentRoom);
                RequestSerialization();
                playerName = Networking.LocalPlayer.displayName;
                isUsed = true;
                controller.PingLoop();
            }
            return true;
        }

        public void UpdateRoom(TrackerRoom _room)
        {
            if (Networking.LocalPlayer != thisOwner) return;
            if (oldRoom == _room) return;
            
            if (oldRoom != null) oldRoom.playersInRoom--;
            oldRoom = currentRoom = _room;
            _room.playersInRoom++;
            controller.UpdatePlugins();
        }

        public void _ResetTracker()
        {
            if (!isUsed) return;
            isUsed = false;
            currentRoom = null;
            oldRoom.playersInRoom--;
            oldRoom = null;
            if (Networking.IsOwner(gameObject))
            {
                playerId = -1;
                currentRoomIndex = -1;
                RequestSerialization();
            }
            controller.UpdatePlugins();
        }

        public override void OnDeserialization()
        {
            thisOwner = VRCPlayerApi.GetPlayerById(playerId);
            if (!VRC.SDKBase.Utilities.IsValid(thisOwner))
            {
                _ResetTracker();
                return;
            }
            
            
            currentRoom = controller.trackedRooms[currentRoomIndex];
            if (oldRoom != currentRoom)
            {
                oldRoom.playersInRoom--;
                oldRoom = currentRoom;
                currentRoom.playersInRoom++;
                
                controller.UpdatePlugins();
            }
            playerName = thisOwner.displayName;
        }
    }
}