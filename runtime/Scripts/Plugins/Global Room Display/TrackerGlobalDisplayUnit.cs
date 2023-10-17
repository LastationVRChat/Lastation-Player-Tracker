
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class TrackerGlobalDisplayUnit : UdonSharpBehaviour
{
    public Text nameText;
    public Text LocationText;
}
