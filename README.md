
![TrackerImage](_playertracker1MB.png)
A sonar-like tracking system for players within a VRChat World.
This is much like a **Framework** in that more plug-ins and add-ons will be made and added over time.

- This script does most of its functions via the local user, meaning every time it "Pings" all player locations are captured and stored by the local user. This keeps something like an array of player locations from being sent over the network.

If you use our Prefab we do not ask you to credit us, simply add to our repo if you make any Plug-ins or have any code changes you would like to suggest our contact info is below or you may use the [discussions](https://github.com/LastationVRChat/Lastation-Player-Tracker/discussions)
<br>
<br>
**Contact**
<br> Our [Discord Server](https://discord.gg/lastation) (the best way to contact us)
<br> Email: Admin@Lastation.tech


### Why make a new tracker and not use an existing one?
Originally, We used an existing `Collider` and `Event` based tracking solution. However, this was prone to failure in many regards primarily because it was unreliable and would make many tracking mistakes when it came to certain events or edge cases, using colliders was a bit of a pain for Raycast guns at times. So we decided it was better to design this system that uses `Transforms` as probes that check the closest probe to each player and assign them to the room of that probe.

# Setting up the Prefab
By default, you will have a Prefab folder with a prefab for one fully set-up room and two displays.
1. Drag the `Location Tracker [Core]` Prefab into the scene.
2. Within the GameObject called "Rooms" will be a GameObject Called "Room 1" Drag this to the location of your first room to set up.
3. Select the `Location Tracker [Core]` GameObject from here you can Manage, Add, or Delete rooms.
4. Give your first room a name in the "Room List" This will mirror the room name to the `TrackerRoom(Script)` and apply it to the "roomName" Variable as well as the GameObject of the room's script.
5. From here you can use the foldout called "Probes" from here you can add or delete probes in a room.
6. Next just simply adjust the positions of the probes throughout your room. (Keep them low to the floor for best results)
7. Once you have that configured you can add rooms and probes via the `Location Tracker [Core]` Script and use the Prefabs in the folder located at `PlayertrackerV2/Prefabs/Components` to supply them.
8. Set up any `Plugins` you want to use currently we only supply the two displays.
9. Lastly, set up how you want to make the tracker refresh built-in is "PingLoop" which you can set in `Seconds` of how often you want it to run.
9. Alternatively, you can set "PingLoop" to `0` which will disable it, and use a function call to script your own "Ping" scripts using the `Functions` below.



# Parts of the Framework:

## Plugins

| Plugin       | Description                                                                                                                                                                                                                                                                                                                                                |
|----------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Global Room Display | This prefab is used to display all players and what room each one is in. (Great for testing the Probes)                                                                                                                                                                                                                                                                            |
| Local Room Display | This prefab is used to display the players that are inside of the attached `TrackerRoom(Script)`                                                                                                                                                                                                                                                                                                                        |
| More Plug-ins Soon!  | This is a project that will be added to as we at [Lastation](https://discord.gg/lastation) need them. However, we are going to provide documentation below so you can make your own, and should you want them added to `main` just contact us or create a [issue](https://github.com/LastationVRChat/Lastation-Player-Tracker/issues)                                                                                                                                                                                        |

## Variables

| Variable       | Description                                                                                                                                                                                                                                                                                                                                                |
|----------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `TrackerMain` public float pingLoop | Automatic Ping that will run after x amount of seconds. Set to 0 to disable. |
| `TrackerMain` public UdonBehaviour[] plugins; | Plugin Array for future Plugins currently only used for the displays. |
| `TrackerMain` public TrackerRoom[] trackedRooms; | Stores the rooms to be tracked. |
| `TrackerMain` public TrackerRoom[] playerRooms; | Stores the room the player is in by array index. (See Below) |
| `TrackerMain` private VRCPlayerApi[] players; | Stores the players index for the playerRooms array. ("Element 0" on this array stores X player and comparing it to "Element 0" on the `playerRooms` array will be the room that the player of "Element 0" is in.) |
| `TrackerMain` private int playerCount; | Stores the current player count. Used for generating the `players[]` array. |
| `TrackerMain` private TrackerRoom _nearestRoom; | Stores the nearest room during each forloop within the AssignRoom Function. (Do not touch, unless you know what you are doing.) |
| `TrackerRoom` public string roomName; | The name of this room, Managed by the `TrackerMain(Script)` editor scripting. |
| `TrackerRoom` public float distance; | Used to Store the player's distance to each room during each loop resets each time a new player is checked. Useful for local debugging, Hidden in inspector by default. |
| `TrackerRoom` public Transform[] probes; | Stores this rooms Probes, Managed by the `TrackerMain(Script)` editor scripting. |
| `TrackerRoom` public int playersInRoom; | Stores the current total of players within the room for use with Plugins. |

## Functions

| Function       | Description                                                                                                                                                                                                                                                                                                                                                |
|----------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `TrackerMain` SendNetworkPing() | Used to force all players to run the `Ping()` Function. |
| `TrackerMain` Ping() | Used to update all player locations and check what rooms they are in. (This function is local, Use the Function above if you want to force all players to check player locations. |
| `TrackerMain` string[] GetPlayerNames() | Returns a string array of player display names with the same index as the `VRCPlayerApi[] players` array for easy sorting. |
| `TrackerMain` TrackerRoom GetRoom(VRCPlayerApi player) | Feed this function a player to get the room they are in. (Useful for Plugins) |
| `TrackerRoom` int GetPlayerCount() | a Function within a `TrackerRoom` to get that rooms total players in the room. |
