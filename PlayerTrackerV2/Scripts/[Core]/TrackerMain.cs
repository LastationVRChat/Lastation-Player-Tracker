﻿
using System;
using VRC.Udon;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using Lastation.PlayerTrackerV2;
using VRC.Udon.Common.Interfaces;

#region Editor

#if !COMPILER_UDONSHARP && UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(TrackerMain))]
public class TrackerMainEditor : Editor
{
    private GUIContent customTitleContent;
    bool[] foldoutstates;
    private void OnEnable()
    {
        customTitleContent = new GUIContent("Lastation Player Tracker");
        //bool array for foldouts
        foldoutstates = new bool[((TrackerMain)target).trackedRooms.Length];
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        TrackerMain trackerMain = (TrackerMain)target;
        EditorGUILayout.LabelField(customTitleContent, EditorStyles.boldLabel);
        DrawDefaultInspector();
        EditorGUILayout.Space(10);
        EditorGUILayout.BeginVertical(GUI.skin.window);
        EditorGUILayout.LabelField("Room List", EditorStyles.boldLabel);


        if (foldoutstates.Length != trackerMain.trackedRooms.Length)
        {
            Array.Resize(ref foldoutstates, trackerMain.trackedRooms.Length);
        }

        //create a display for each room with name on top and transform array beneath
        for (int i = 0; i < trackerMain.trackedRooms.Length; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            //textfield with no label to display room name
            if (trackerMain.trackedRooms[i] != null)
            {
                trackerMain.trackedRooms[i].roomName = EditorGUILayout.TextField(trackerMain.trackedRooms[i].roomName);
                if (trackerMain.trackedRooms[i] != null && !string.IsNullOrEmpty(trackerMain.trackedRooms[i].roomName))
                {
                    trackerMain.trackedRooms[i].name = trackerMain.trackedRooms[i].roomName;
                }
            }
            else
            {
                EditorGUILayout.LabelField("Room " + (i + 1) + " is null");

            }

            trackerMain.trackedRooms[i] = (TrackerRoom)EditorGUILayout.ObjectField(trackerMain.trackedRooms[i], typeof(TrackerRoom), true);
            // tomfoolery to make sure things look nice
            EditorGUILayout.BeginHorizontal();
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical();

            //foldouts for probes in each room
            foldoutstates[i] = EditorGUILayout.Foldout(foldoutstates[i], "Probes [Player origin is at the feet, ensure these are placed low to the gound]");

            if (foldoutstates[i])
            {
                //null error check for the trackerroom being valid to generate the probe array
                if (trackerMain.trackedRooms[i] == null)
                {
                    EditorGUILayout.LabelField("No TrackerRoom to generate probes from");
                    EditorGUILayout.EndVertical();
                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    continue;
                }

                for (int j = 0; j < trackerMain.trackedRooms[i].probes.Length; j++)
                {
                    trackerMain.trackedRooms[i].probes[j] = EditorGUILayout.ObjectField("Probe " + j, trackerMain.trackedRooms[i].probes[j], typeof(Transform), true) as Transform;

                    // Add a button to remove the last probe in the array
                    if (j == trackerMain.trackedRooms[i].probes.Length - 1 && GUILayout.Button("Remove Probe"))
                    {
                        RemoveProbe(trackerMain.trackedRooms[i], j);
                    }
                }

                // Add a button to add a new probe
                if (GUILayout.Button("Add Probe"))
                {
                    AddProbe(trackerMain.trackedRooms[i]);
                }
            }

            //end of tomfoolery
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10);
        }

        EditorGUILayout.EndVertical();
        // Button to add a new room
        if (GUILayout.Button("Add Room"))
        {
            AddRoom(trackerMain);
        }

        if (GUILayout.Button("Remove Room"))
        {
            RemoveRoom(trackerMain);
        }

        serializedObject.ApplyModifiedProperties();
    }
    private void AddRoom(TrackerMain trackerMain)
    {
        Array.Resize(ref trackerMain.trackedRooms, trackerMain.trackedRooms.Length + 1);
        trackerMain.trackedRooms[trackerMain.trackedRooms.Length - 1] = new TrackerRoom();
    }

    private void RemoveRoom(TrackerMain trackerMain)
    {
        Array.Resize(ref trackerMain.trackedRooms, trackerMain.trackedRooms.Length - 1);
    }

    private void AddProbe(TrackerRoom room)
    {
        // Create a new array with one extra element
        Transform[] newProbes = new Transform[room.probes.Length + 1];
        Array.Copy(room.probes, newProbes, room.probes.Length);

        // Add the new probe to the array
        newProbes[newProbes.Length - 1] = null; // You can set a default value here if needed

        // Update the probes array with the new array
        room.probes = newProbes;

        // Mark the object as dirty so that the changes are saved
        EditorUtility.SetDirty(target);
    }

    private void RemoveProbe(TrackerRoom room, int index)
    {
        // Make sure the index is within the bounds of the array
        if (index >= 0 && index < room.probes.Length)
        {
            // Create a new array with one less element
            Transform[] newProbes = new Transform[room.probes.Length - 1];
            int newIndex = 0;

            // Copy the probes to the new array, skipping the one to be removed
            for (int i = 0; i < room.probes.Length; i++)
            {
                if (i != index)
                {
                    newProbes[newIndex] = room.probes[i];
                    newIndex++;
                }
            }

            // Update the probes array with the new array
            room.probes = newProbes;

            // Mark the object as dirty so that the changes are saved
            EditorUtility.SetDirty(target);
        }
    }

}

#endif

#endregion Editor


namespace Lastation.PlayerTrackerV2
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class TrackerMain : UdonSharpBehaviour
    {
        #region Serialized Fields

        [Tooltip("Automatic Ping that will run after x amount of seconds. Set to 0 to disable")]
        [SerializeField] private float pingLoop = 60;
        [Tooltip("Plugin Array for future Plugins currently only used for the displays")]
        [SerializeField] private UdonBehaviour[] plugins;

        #endregion Serialized Fields

        [HideInInspector] public TrackerRoom[] trackedRooms; //stores the rooms to be tracked
        [HideInInspector] public TrackerRoom[] playerRooms; //stores the room the player is in by array index
        [HideInInspector] public string[] playerNames; //stores the player names by array index

        #region Private Fields

        private VRCPlayerApi[] players; //stores the players for the playerRooms array
        private int playerCount; //stores the current player count
        private int _currentPlayerToPing;
        private bool _pinging = false;

        #endregion Private Fields


        private void Start()
        {
            SendCustomEventDelayedFrames(nameof(PingLoop), 10);
        }

        #region VRC Overrides

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            _PlayerPingLoop();
        }
        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            _PlayerPingLoop();
        }

        #endregion VRC Overrides

        

        #region API

        public void SendNetworkPing()
        {
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(Ping));
        }

        /// <summary>
        /// Call this to start the process that finds what room everyone is in.
        /// </summary>
        public void Ping()
        {
            if (_pinging) return;

            // Ask the VRCPlayerAPI for the number of players
            playerCount = VRCPlayerApi.GetPlayerCount();

            // Create an array of VRCPlayerApi objects of the required size
            players = new VRCPlayerApi[playerCount];
            playerRooms = new TrackerRoom[playerCount];
            playerNames = new string[playerCount];

            // Get the players and assign them to the array
            VRCPlayerApi.GetPlayers(players);

            for (int i = 0; i < playerCount; i++)
            {
                playerNames[i] = players[i].displayName;
            }

            // TODO: CHECK THIS IN EDITOR
            // Sanity check that there are rooms
            if (trackedRooms == null) { Debug.LogError("No rooms found, please add rooms to the tracker"); return; }

            // Reset all the counters
            for (int i = 0; i < trackedRooms.Length; i++)
            {
                trackedRooms[i].playersInRoom = 0;
            }

            _currentPlayerToPing = 0;
            _pinging = true;
            _PlayerPingLoop();
        }

        public void _PlayerPingLoop()
        {
            if (_currentPlayerToPing == playerCount) { PingCompleted(); return; }

            AssignRoom(players[_currentPlayerToPing]);
            _currentPlayerToPing++;
            SendCustomEventDelayedFrames(nameof(_PlayerPingLoop), 1);
        }

        public TrackerRoom GetRoom(VRCPlayerApi player)
        {
            int playerRoomsIndex = Array.IndexOf(players, player); //get the 'INT' index of the player in the players array.

            if (playerRoomsIndex == -1) { Debug.LogError("[GetRoom] - Player Not Found"); return null; }

            return playerRooms[playerRoomsIndex];
        }

        /// <summary>
        /// Used by the room displays to get the player names.
        /// </summary>
        /// <returns>PlayerNames</returns>
        public string[] GetPlayerNames() => playerNames;

        // get room names from the 

        #endregion API

        #region Internal

        private void PingCompleted()
        {
            _pinging = false;
            for (int i = 0; i < plugins.Length; i++)
            {
                plugins[i].SendCustomEvent("GeneratePlayerList");
            }
        }

        private void AssignRoom(VRCPlayerApi _player)
        {
            int playerIndex = Array.IndexOf(players, _player); // Get the index of the player in the players array.
            if (playerIndex == -1) { Debug.LogError("[AssignRoom] - Player Is Not In Players Array"); return; }

            float smallestDistanceFound = float.PositiveInfinity;
            int closestRoomIndex = -1;

            // Find the closest room.
            for (int i = 0; i < trackedRooms.Length; i++)
            {
                float distanceToRoom = trackedRooms[i].RoomProbeCheck(_player);

                if (distanceToRoom > smallestDistanceFound) continue;

                smallestDistanceFound = distanceToRoom;
                closestRoomIndex = i;
            }

            TrackerRoom closestRoom = trackedRooms[closestRoomIndex];
            closestRoom.playersInRoom++;
            playerRooms[playerIndex] = closestRoom; // Update the room the player is in.
        }

        #endregion Internal

        /// <summary>
        /// This is internal but has to be public due to SendCustomEventDelayed. Don't call this.
        /// </summary>
        public void PingLoop()
        {
            Ping();
            if (pingLoop <= 0) return;
            SendCustomEventDelayedSeconds(nameof(PingLoop), pingLoop);
        }
    }
}

