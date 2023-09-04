
![TrackDeezNuts](playertracker.png)
A sonar-like tracking system for players within a VRChat World.
This is much like a **Framework** in that more plug-ins and add-ons will be made and added over time.
### Why make a new tracker and not use an existing one?
Originally, We used an existing `Collider` and `Event` based tracking solution. However, this was prone to failure in many regards primarily because it was unreliable and would make many tracking mistakes when it came to certain events or edge cases, using colliders was a bit of a pain for Raycast guns at times. So we decided it was better to design this system that uses `Transforms` as probes that check the closest probe to each player and assign them to the room of that probe.

# Setting up the Prefab
If you are using the free version of DisBridge, you can skip to `Parts of a RoleContainer` below for setup, as you wont have a JSON to import and will need to do a manual setup.

To initially setup the prefab, it is recommended to import your roles from the DisBridge bot.
<br>All you need to do is to click the `Parse from Json` button in the bottom right of the editing tool.
<br><i>(If you dont have the Json file to import, you can get it by running `/guild-status` in your server.)</i>
<br>All of your roles should now be in the editing tool.

After importing your roles, you'll need to do some minor edits.
<br>To edit a role, all you need to do is click on the role, and it should expand to show all the options.
<br>You'll need to go though each of your now imported roles and check mark what roles are and aren't support and staff roles.
<br>The `Is Supporter` and `Is Staff` check marks are for generic checks. This is so if a plugin doesn't need to know what specific role you're in, and just needs to know if you're a staff member or a supporter.
<br>A role can be both a supporter and a staff member. If you're just using DisBridge to edit a list of names, you dont need to mark any roles as supporters or staff. But it's still recommended to do so.

Other notable options would be `Alternative Role Name`, `Role Icon` and `Manual Usernames`.
<br>You can read about what everything does in `Parts of a RoleContainer` below.

# Parts of the Framework:

## Variables

| Variable       | Description                                                                                                                                                                                                                                                                                                                                                |
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
