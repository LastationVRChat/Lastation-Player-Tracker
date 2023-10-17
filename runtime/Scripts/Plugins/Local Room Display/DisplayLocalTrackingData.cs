
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace Lastation.PlayerTrackerV2
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DisplayLocalTrackingData : UdonSharpBehaviour
    {
        public TrackerMain tracker;
        public TrackerRoom localRoomScript;
        public GameObject[] templates;

        private Text[] _templateNames;
        private void Start()
        {
            _templateNames = new Text[templates.Length];

            for (int i = 0; i < templates.Length; i++)
            {
                _templateNames[i] = templates[i].GetComponentInChildren<Text>();
            }
        }

        public void GeneratePlayerList()
        {
            string[] playernames = tracker.GetPlayerNames();

            //disable all the templates
            foreach (GameObject Template in templates)
            {
                Template.SetActive(false);
            }


            TrackerRoom[] playerRooms = tracker.playerRooms;

            //configure and enable the templates
            for (int i = 0; i < playernames.Length; i++)
            {
                if (playerRooms[i] != localRoomScript) continue; // The player is not in the same room as the current TrackerRoom.
                string playerName = playernames[i];
                if (string.IsNullOrEmpty(playerName)) continue;
                
                _templateNames[i].text = playerName;
                templates[i].SetActive(true);
            }
        }
    }
}

