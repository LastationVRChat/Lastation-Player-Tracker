
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;
using VRC.Udon.Common.Interfaces;


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
        
        public bool _TryGetTracker(VRCPlayerApi _player)
        {
            Debug.LogError($"_TryGetTracker: {gameObject.name}");
            if (isUsed)
            {
                Debug.LogError($"isUsed = true, returning");
                return false;
            }
            if (VRC.SDKBase.Utilities.IsValid(VRCPlayerApi.GetPlayerById(playerId)))
            {
                Debug.LogError($"playerId {playerId} is valid");
                return false;
            }
            else
            {
                Debug.LogError($"Assigning tracker to {_player.displayName}");
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
            Debug.LogError($"run N_Setup on {gameObject.name}");
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
            Debug.LogError($"UpdateRoom {gameObject.name}");
            Debug.LogError($"Player ID is {playerId}");
            if (_room == null)
            {
                Debug.LogError($"Room is Null");
                return;
            }

            if (oldRoom == _room)
            {
                Debug.LogError($"No Room Change");
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
            Debug.LogError($"_ResetTracker {gameObject.name}");
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
            Debug.LogError($"OnDeserialization {gameObject.name}");
            if (Networking.GetOwner(gameObject).isLocal)
            {
                Debug.LogError($"Local player is owner, returning");
                playerId = Networking.LocalPlayer.playerId;
                RequestSerialization();
                return;
            }
            thisOwner = VRCPlayerApi.GetPlayerById(playerId);
            Debug.LogError($"playerId: {playerId}");
            if (!VRC.SDKBase.Utilities.IsValid(thisOwner))
            {
                Debug.LogError($"thisOwner is not valid");
                _ResetTracker();
                return;
            }
            Setup();
            Debug.LogError($"thisOwner: {thisOwner.displayName} | Owner: {Networking.GetOwner(gameObject).displayName}");
            Debug.LogError($"currentRoomIndex: {currentRoomIndex}");
            if (currentRoomIndex != -1)
            {
                UpdateRoom(controller.trackedRooms[currentRoomIndex]);
            }
            
            
        }
    }
}