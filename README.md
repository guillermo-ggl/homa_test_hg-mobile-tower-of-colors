
# Homa test - Tower of colors

This project serves as a case study for Homa.



## Optimizations

### Draw Calls
To optimize the draw calls, I began by analyzing the frame debugger. The majority of draw calls originated from the barrels, so because they use a simple common material, it was easy to batch them using GPU instancing, and the same was done for the water. Later in the development a Sprite Atlas was added so the UI can also be batched, and empty Image types were filled with a placeholder square sprite for further batching.

The draw calls were reduced from over 100 to less than 20. Further optimization could involve batching barrels of different colors using the same material while adjusting colors through material properties.

![image](https://github.com/guillermo-ggl/homa_test_hg-mobile-tower-of-colors/assets/148863785/eaa5f76c-605c-41cb-a990-23f6686cce58)

### Barrel Pooling
The barrel pooling system was implemented by repurposing the FXPool system code for code consistency. Merging the FXPool and barrel pooling systems is a feasible option. Barrels were unparented from the "Tower" parent to ensure they are not destroyed upon scene reload. If re-parenting is necessary in the future, it should be done with consideration, ensuring barrels are de-parented on scene reload or making the parent not destroyed on load. Some condition checks in the gameplay behavior were modified to accommodate the new "used/not used" state, addressing issues introduced by not destroying the object itself.



## New Mission System

Given the limited requirements and the goal of creating an easily extendable system for potential use in other games with minimal effort, several design choices were made, leading to a more complex solution than necessary for this project.

Scriptable Objects were the primary choice for implementation, providing a user-friendly way to design and add missions and rewards in the Unity Editor, without requiring a Unity Engineer. The three main objects in this system are MissionSystem.cs, Mission.cs, and Reward.cs, all implemented as Scriptable Objects.

![image](https://github.com/guillermo-ggl/homa_test_hg-mobile-tower-of-colors/assets/148863785/79fb327e-2b4f-4a6b-aedc-a9fbcb3df86e)

Mission and Reward are classes meant to be extended to fit the project's needs. In this case, Mission was extended with Mission_Additive and Mission_Conditioned classes to detect and count barrel explosions, completed levels, and combos. These classes can be further extended.

The system operates with a simple call to the Mission System when a mission event is triggered, such as a barrel elimination, with type and quantity as parameters. The game does not need to know which missions are active; the call is always made, and the Mission System discards irrelevant events. Information about current missions can be collected to create a UI, providing details about the goal, progress, description, and whether it is claimable or already claimed.

The UI was developed by reusing assets and integrating it with other UI elements and animations. The Missions UI is managed separately from the system through the MissionsUI.cs and MissionPanelUI.cs scripts, ensuring flexibility for customization in each project using the system.

![image](https://github.com/guillermo-ggl/homa_test_hg-mobile-tower-of-colors/assets/148863785/81dfe5fd-c466-4684-802c-e6e70f976b99)

Since no requirements were specified for selecting missions or rewards, missions are randomly selected based on difficulty, avoiding repetition of the last mission whenever possible. The assigned reward is also randomly selected based on mission difficulty.

Although there is no requirement to make missions and rewards unattainable based on conditions (f.e.: A reward that could be a skin to not be chosen if the skin is already unlocked), this can be easily added.

To ensure scalability and avoid unnecessary data storage, only data from active quests (three in this project) is saved. This decision centralizes data management in the MissionSystem rather than the Mission or Reward, eliminating the need for missions and rewards to know their slot indices.

As no localization requirements were provided, the mission description system is straightforward and hard-coded, expected to be adjusted based on the final localization system.

Enabling or disabling the system remotely also controls the visibility of the UI in the game hierarchy, and avoids reseting or reporting events to the system.
