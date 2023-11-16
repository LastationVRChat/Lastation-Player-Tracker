
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class DoorLock : UdonSharpBehaviour
{
    public UdonBehaviour targetRoomScript;
    public string targetRoomScriptFunctionName;
    public bool displayedLockState;


    [Header("Interior Lock States")]
    [SerializeField] private GameObject[] lockDisplays;
    [SerializeField] private GameObject[] unlockDisplays;
    

    public void OnClick()
    {
        targetRoomScript.SendCustomEvent(targetRoomScriptFunctionName);
    }

    public void SetLockState(bool state)
    {
        if (state == displayedLockState)
        {
            return;
        }
        displayedLockState = state;
        if (state)
        {
            foreach (GameObject lockDisplay in lockDisplays)
            {
                lockDisplay.SetActive(true);
            }
            foreach (GameObject unlockDisplay in unlockDisplays)
            {
                unlockDisplay.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject lockDisplay in lockDisplays)
            {
                lockDisplay.SetActive(false);
            }
            foreach (GameObject unlockDisplay in unlockDisplays)
            {
                unlockDisplay.SetActive(true);
            }
        }
    } 
}
