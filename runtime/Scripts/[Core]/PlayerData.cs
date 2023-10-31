
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;
using VRC.Udon.Common.Interfaces;


namespace Lastation.PlayerTrackerV2
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class PlayerData : UdonSharpBehaviour
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
        
        public bool _TryGetTracker(VRCPlayerApi _player)
        {
            if (isUsed)
            {
                return false;
            }
            if (VRC.SDKBase.Utilities.IsValid(VRCPlayerApi.GetPlayerById(playerId)))
            {
                return false;
            }
            else
            {
                Networking.SetOwner(_player,gameObject);
                if (Networking.GetOwner(gameObject).isLocal)
                {
                    N_Setup();
                }
                else
                {
                    SendCustomNetworkEvent(NetworkEventTarget.Owner,nameof(N_Setup));
                }
            }
            return true;
        }

        public void N_Setup()
        {
            thisOwner = Networking.LocalPlayer;
            playerId = thisOwner.playerId;
            currentRoomIndex = controller.GetRoomIndex(currentRoom);
            controller.localTracker = this;
            isLocalTracker = true;
            Setup();
            controller.PingLoop();
            RequestSerialization();
        }

        public void UpdateRoom(TrackerRoom _room)
        {
            if (_room == null)
            {
                Debug.LogError($"Room is Null");
                return;
            }

            if (oldRoom == _room)
            {
                return;
            }
            if (oldRoom != null){ oldRoom.playersInRoom--;}
            oldRoom = currentRoom = _room;
            _room.playersInRoom++;
            
            if (Networking.LocalPlayer.IsOwner(gameObject))
            {
                currentRoomIndex = controller.GetRoomIndex(_room);
                RequestSerialization();
            }
            controller.UpdatePlugins();
            
        }

        public void _ResetTracker()
        {
            if (!isUsed) return;
            isUsed = false;
            currentRoom = null;
            oldRoom.playersInRoom--;
            oldRoom = null;
            if (Networking.LocalPlayer.IsOwner(gameObject))
            {
                playerId = -1;
                currentRoomIndex = -1;
                RequestSerialization();
            }
            controller.UpdatePlugins();
        }

        private void Setup()
        {
            if (isUsed) return;
            playerName = thisOwner.displayName;
            isUsed = true;
        }
        
        public override void OnDeserialization()
        {
            if (Networking.GetOwner(gameObject).isLocal)
            {
                playerId = Networking.LocalPlayer.playerId;
                RequestSerialization();
                return;
            }
            thisOwner = VRCPlayerApi.GetPlayerById(playerId);
            if (!VRC.SDKBase.Utilities.IsValid(thisOwner))
            {
                _ResetTracker();
                return;
            }
            Setup();
            if (currentRoomIndex != -1)
            {
                UpdateRoom(controller.trackedRooms[currentRoomIndex]);
            }
            
            
        }
    }
}