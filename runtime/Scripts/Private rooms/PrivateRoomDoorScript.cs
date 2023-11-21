
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Lastation.PlayerTrackerV2
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class PrivateRoomDoorScript : UdonSharpBehaviour
    {
        [Header("Room Teleportation")]
        [SerializeField] private GameObject room;
        [SerializeField] private Transform interiorTeleportPoint;
        [SerializeField] private Transform exteriorTeleportPoint;

        [Header("Location Tracking")]
        [SerializeField] private TrackerMain locationTracker;
        [SerializeField] private TrackerRoom trackerRoom;

        [Header("UI stuff")]
        [SerializeField] private GameObject[] lockDisplays;
        [SerializeField] private GameObject[] unlockDisplays;
        [UdonSynced] private bool locked;
        private bool isInside;
        [SerializeField] private DoorLock doorLock;
        

        public void EnterRoom()
        {
            if (locked)
            {
                return;
            }
            else
            {
                //teleport the local player
                Networking.LocalPlayer.TeleportTo(interiorTeleportPoint.position, interiorTeleportPoint.rotation);
                locationTracker.Ping();
                doorLock.SetLockState(locked);
                isInside = true;
            }
        }
        public void exitRoom()
        {
            if (locked)
            {
                return;
            }
            else
            {
                Networking.LocalPlayer.TeleportTo(exteriorTeleportPoint.position, exteriorTeleportPoint.rotation);
                locationTracker.Ping();
                isInside = false;
            }
        }
        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (trackerRoom.playersInRoom == 0 && Networking.IsOwner(gameObject))
            {
                if (locked)
                {
                    ToggleLock();
                }
            }
        }
        public void ToggleLock()
        {
            if (!locked)
            {
                locked = true;
                //set the lock displays to the locked state
                foreach (GameObject display in lockDisplays)
                {
                    display.SetActive(true);
                }
                foreach (GameObject display in unlockDisplays)
                {
                    display.SetActive(false);
                }
            }
            else
            {

                locked = false;
                //set the lock displays to the unlocked state
                foreach (GameObject display in lockDisplays)
                {
                    display.SetActive(false);
                }
                foreach (GameObject display in unlockDisplays)
                {
                    display.SetActive(true);
                }
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
                foreach (GameObject display in lockDisplays)
                {
                    display.SetActive(true);
                }
                foreach (GameObject display in unlockDisplays)
                {
                    display.SetActive(false);
                }
                //check if the localplayer is inside this room using the ID given by the location tracker
                if (locationTracker.GetRoom(Networking.LocalPlayer) == trackerRoom)
                {
                    //set the lock state on the door lock
                    doorLock.SetLockState(locked);
                }

            }
            else
            {
                //set the lock displays to the unlocked state
                foreach (GameObject display in lockDisplays)
                {
                    display.SetActive(false);
                }
                foreach (GameObject display in unlockDisplays)
                {
                    display.SetActive(true);
                }
                if (locationTracker.GetRoom(Networking.LocalPlayer) == trackerRoom)
                {
                    //set the lock state on the door lock
                    doorLock.SetLockState(locked);
                }
            }
        }
    }

}

