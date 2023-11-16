
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class DoorTriggerLastation : UdonSharpBehaviour
{

    [SerializeField] private UdonBehaviour TargetScript;
    [SerializeField] private string TargetFunction;
    public override void Interact()
    {
        TargetScript.SendCustomEvent(TargetFunction);
    }
    public void SetTarget(UdonBehaviour targetScript, string targetFunction)
    {
        TargetScript = targetScript;
        TargetFunction = targetFunction; 
    }
}
