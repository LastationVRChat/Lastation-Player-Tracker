
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Lastation.PlayerTrackerV2
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class BarrierPrivateRoom : UdonSharpBehaviour
    {
        [Header("Room Teleportation")]
        [SerializeField] private Transform interiorTeleportPoint;

        [Header("Location Tracking")]
        [SerializeField] private TrackerMain locationTracker;
        [SerializeField] private TrackerRoom trackerRoom;

        [Header("UI stuff")]
        [SerializeField] private GameObject lockDisplay;
        [SerializeField] private GameObject unlockDisplay;
        [SerializeField] private DoorLock doorLock;

        [SerializeField] private GameObject doorObject;

        [UdonSynced] private bool locked;
        private bool isInside;
        private VRCPlayerApi lplayer;


        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if(player.isLocal)
            {
                lplayer = player;
                locationTracker.Ping();
                SendCustomEventDelayedSeconds(nameof(CheckLocalPlayer), 5);
            }
        }

        public void CheckLocalPlayer()
        {
            if (locationTracker.GetRoom(lplayer) == trackerRoom)
            {
                isInside = true;
            }
            else
            {
                isInside = false;
            }
        }
 
        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (trackerRoom.playersInRoom == 0 && Networking.IsOwner(gameObject))
            {
                ToggleLock();
                doorLock.SetLockState(false);
            }
        }
        public void ToggleLock()
        {
            if (!locked)
            {
                locked = true;
                lockDisplay.SetActive(true);
                unlockDisplay.SetActive(false);
                doorObject.SetActive(true);
            }
            else
            {
                locked = false;
                lockDisplay.SetActive(false);
                unlockDisplay.SetActive(true);
                doorObject.SetActive(false);
            }
            //send the changes to the doorlock locally
            doorLock.SetLockState(locked);
            //send changes over the network
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            RequestSerialization();
        }

        public override void OnPlayerRespawn(VRCPlayerApi player)
        {
            if (isInside && player == Networking.LocalPlayer)
            {
                player.TeleportTo(interiorTeleportPoint.position, interiorTeleportPoint.rotation);
            }
        }

        public override void OnDeserialization()
        {
            if (locked)
            {
                lockDisplay.SetActive(true);
                unlockDisplay.SetActive(false);

                //check if the localplayer is inside this room using the ID given by the location tracker
                if (locationTracker.GetRoom(Networking.LocalPlayer) == trackerRoom)
                {
                    doorLock.SetLockState(locked);
                }

            }
            else
            {
                lockDisplay.SetActive(false);
                unlockDisplay.SetActive(true);

                if (locationTracker.GetRoom(Networking.LocalPlayer) == trackerRoom)
                {
                    doorLock.SetLockState(locked);
                }
            }
        }
    }

}

