
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class DisplayLocalTrackingData : UdonSharpBehaviour
{
     public TrackerRoom localRoomScript;
     public GameObject Template;
     public Transform contentParent;
     public TrackerMain tracker;
     
     
     public void GeneratePlayerList()
     {
          string[] playernames = tracker.GetPlayerNames();

          //kill all children
          for (int i = 0; i < contentParent.childCount; i++)
          {
                Destroy(contentParent.GetChild(i).gameObject);
          }

          //create new children
          for (int i = 0; i < playernames.Length; i++)
          {
            if (tracker.playerRooms[i] == localRoomScript && playernames[i] != ""&& playernames[i] != null) //if the player is in the same room as the current TrackerRoom
            {
                 GameObject newThing = Instantiate(Template);
    
                 Text name = newThing.transform.GetChild(0).GetComponent<Text>();
                 if (name)
                 {
                      name.text = playernames[i];
                 }
    
                 newThing.transform.position = Template.transform.position;
                 newThing.transform.rotation = Template.transform.rotation;
                 newThing.transform.SetParent(contentParent);
                 newThing.transform.localScale = Template.transform.localScale;
                 newThing.SetActive(true);
            }
          }
     }
}
