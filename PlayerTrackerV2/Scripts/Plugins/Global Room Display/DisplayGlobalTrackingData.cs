
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Lastation.PlayerTrackerV2
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DisplayGlobalTrackingData : UdonSharpBehaviour
    {
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

            for (int i = 0; i < playernames.Length; i++)
            {

                GameObject newThing = Instantiate(Template);

                TrackerGlobalDisplayUnit displayUnit = newThing.GetComponent<TrackerGlobalDisplayUnit>();
                if (displayUnit)
                {
                    displayUnit.nameText.text = playernames[i];
                    displayUnit.LocationText.text = tracker.playerRooms[i].roomName;
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

