
![TrackDeezNuts](playertracker.png)
A sonar-like tracking system for players within a VRChat World.
This is much like a **Framework** in that more plug-ins and add-ons will be made and added over time.
### Why make a new tracker and not use an existing one?
Originally, We used an existing `Collider` and `Event` based tracking solution. However, this was prone to failure in many regards primarily because it was unreliable and would make many tracking mistakes when it came to certain events or edge cases, using colliders was a bit of a pain for Raycast guns at times. So we decided it was better to design this system that uses `Transforms` as probes that check the closest probe to each player and assign them to the room of that probe.

# Setting up the Prefab
By default, you will have a Prefab folder with a prefab for one fully set-up room and two displays.
- Drag the `Location Tracker [Core]` Prefab into the scene.
- Within the GameObject called "Rooms" will be a GameObject Called "Room 1" Drag this to the location of your first room to set up.
- Select the `Location Tracker [Core]` GameObject from here you can Manage, Add, or Delete rooms.
- Give your first room a name in the "Room List" This will mirror the room name to the `TrackerRoom(Script)` and apply it to the "roomName" Variable as well as the GameObject of the room's script.
- From here you can use the foldout called "Probes" from here you can add or delete probes in a room.
- Next just simply adjust the positions of the probes throughout your room. (Keep them low to the floor for best results)
- Once you have that configured you can add rooms and probes via the `Location Tracker [Core]` Script and use the Prefabs in the folder located at `PlayertrackerV2/Prefabs/Components` to supply them.
- Set up any `Plugins` you want to use currently we only supply the two displays.
- Lastly, set up how you want to make the tracker refresh built-in is "PingLoop" which you can set in `Seconds` of how often you want it to run.
- Alternatively, you can set "PingLoop" to `0` which will disable it, and use a function call to script your own "Ping" scripts using the `Functions` below.



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
| `TrackerMain` public float pingLoop | Automatic Ping that will run after x amount of seconds. Set to 0 to disable.                                                                                                                                                                                                                                                                            |
| `TrackerMain` public UdonBehaviour[] plugins; | Plugin Array for future Plugins currently only used for the displays.                                                                                                                                                                                                                                                                                                                     |
| `TrackerMain` public TrackerRoom[] trackedRooms; | Stores the rooms to be tracked.                                                                                                                                                                                                                                                                                          |
| `TrackerMain` public TrackerRoom[] playerRooms; | Stores the room the player is in by array index. (See Below)                                                                                                                                                                                                                                                                                                               |
| `TrackerMain` private VRCPlayerApi[] players; | Stores the players index for the playerRooms array. ("Element 0" on this array stores X player and comparing it to "Element 0" on the `playerRooms` array will be the room that the player of "Element 0" is in)                                                                                                                                                                                                                                                                                                              |
| `TrackerMain` private int playerCount; | Stores the current player count. Used for generating the `players[]` array.                                                                                                                                                                                                       
                                    
| `TrackerMain` public UdonBehaviour[] plugins; | Plugin Array for future Plugins currently only used for the displays.   


## Functions

| Function       | Description                                                                                                                                                                                                                                                                                                                                                |
|----------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Role ID        | This is what is used to match your roles in Discord to the roles used in Unity.                                                                                                                                                                                                                                                                            |
| Role Name      | This is the name of the Role itself.                                                                                                                                                                                                                                                                                                                       |
| Role Alt Name  | This is the Alt Name of the role, generally this is a Singular name. Example being:<br>If the role was called `Developers` i would make the Alt name `Developer`.                                                                                                                                                                                          |
| Role Icon      | The Icon for plugins to use for the role.                                                                                                                                                                                                                                                                                                                  |
| Role Color     | The color for plugins to use for the role.                                                                                                                                                                                                                                                                                                                 |
| Manual Members | This manually adds people to the role, and will include them when creating the lists. These users will also be used if your key fails to load, or if no key is used.<br>Manual members will also be used to test permissions in Editor. Duplicate users found in Manual Members will not be added to the total user list when pulling users from your key. |
| Is Staff       | This marks your role as a Staff role and will mark the role to be added to the generic Staff list.                                                                                                                                                                                                                                                         |
| Is Supporter   | This marks your role as a Supporter role and will mark the role to be added to the generic Supporter list.                                                                                                                                                                                                                                                 |
| Is Booster     | This marks your role as your Discord Booster role. You can only have one of these marked, and if you have multiples the PluginManager will use the first one it finds.                                                                                                                                                                                     |
