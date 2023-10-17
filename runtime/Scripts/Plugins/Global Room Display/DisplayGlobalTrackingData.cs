
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Lastation.PlayerTrackerV2
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DisplayGlobalTrackingData : UdonSharpBehaviour
    {
        public TrackerMain tracker;
        public GameObject[] templates;

        private Text[] nameTexts;
        private Text[] locationTexts;
        private void Start()
        {
            nameTexts = new Text[templates.Length];
            locationTexts = new Text[templates.Length];

            for (int i = 0; i < templates.Length; i++)
            {
                var displayUnit = templates[i].GetComponent<TrackerGlobalDisplayUnit>();
                nameTexts[i] = displayUnit.nameText;
                locationTexts[i] = displayUnit.LocationText;
            }
        }

        public void GeneratePlayerList()
        {
            for (int i = 0; i < templates.Length; i++)
            {
                if (tracker.trackers[i].isUsed)
                {
                    nameTexts[i].text = tracker.trackers[i].playerName;
                    locationTexts[i].text = tracker.trackers[i].currentRoom.roomName;
                    templates[i].SetActive(true);
                }
            }
        }
    }
}

