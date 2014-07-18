Milestone 3
------------

**Luke Tornquist**  -  *luke.tornquist@gatech.edu* - *ltornquist3*

**Jonathan Yates**  -  *jyates32@gatech.edu*  -  *jyates32*

**Evan LaHurd**  -  *elahurd3@gatech.edu*  -  *elahurd3*

---

### External Resources

- (Models)
- Cyborg Model
  - https://www.assetstore.unity3d.com/en/#!/content/10705
- Torch
  - https://www.assetstore.unity3d.com/en/#!/content/7275
- Crates
  - https://www.assetstore.unity3d.com/en/#!/content/8836

- Fire, Flame, Dust, Bubble, Waterfall, Water Fountain, and Sparkle Particle Effects
  - Assets -> Import Package -> Particles

-Elementals
  - https://www.assetstore.unity3d.com/en/#!/content/11158

- Water (Ponds)
  - Assets -> Import Package -> Water (Basic)

- Rustic Door
  - https://www.assetstore.unity3d.com/en/#!/content/861

- (Audio)
- Footstep Audio
  - http://www.freesound.org/
- Water Footsteps
  - http://www.freesfx.co.uk/sfx/footsteps?p=3 (Footsteps In Water 1)
- Campfire Sound Effect
  - http://www.soundjay.com/nature/campfire-1.wav  (Fire Tree Sound Effect)
- Puzzle Solve Sound
  - http://noproblo.dayjo.org/ZeldaSounds/OOT/OOT_Secret.wav


### Fulfilled Requirements

- AI State Machine implemented 
  - Soldier can be in one of 3 states
  - Patrol state is when there is no player in Line of Sight, and follows a set path of waypoints
  - Attack state is when there is a player in Line of Sight, and the soldier will pursue the player and attack when within attack range
  - If while in the Attack state the soldier catches up to the player, he will maintain a certain distance from the player and continue to fire since he has a ranged attack
  - Flee state is whenever the soldier gets below 50 health, he will flee away whenever he sees the player
- Added position prediction for firing, so that the soldier will try to lead his shots in such a way to predict the position of the player

#### Instructions

The Xbox controller controls are detailed in the help screen of our game, but if you are using a keyboard and mouse, use the following controls:

To move the ninja around, use the WASD keys. Move the camera using the mouse. Hold SHIFT to sprint, and press SPACE to jump. Press "1" to change to Air form,
press "2" to change to Fire form, and press "3" to change to "Water" form. Pressing "1", "2", or "3" again while in an elemental form changes you back to the base
form. To charge your elements, you need to be standing in the particular element that you want to charge while in that elemental form or in the base state. For 
example, to charge fire, you should be standing in fire while in the fire elemental form or while you're in the base form. To attack, click the left mouse button.

The objective of the game is to solve the puzzles in all 3 rooms which are all themed based on the 3 elements that the ninja can change into. In each room 
there is an enemy that you can kill by attacking it.
 
The first room is the air room. To beat this puzzle you need to jump up the pillars and reach the exit. To jump across the pillars, you need to be in the
air form, so change to that form. If you run out of air energy, you can charge it in the misty substance that is on the ground where you spawned. 
While jumping, you should sprint and jump only once. Pressing jump repeatedly usually results in a double jump which makes you jump off the pillar. 
Jump up the first 3 pillars, and you will hit a fork. Jump straight to reach the pillar with the red button, and walk into it to press it. 
You will hear an obvious "puzzle complete" sound once vyou do so. Head back to the third pillar, and take the right path (if you're facing the button). 
Now the pillar that was previously too far from the green waypoint will be close enough for you to make the jump. Jump across the rest of the pillars 
to the exit with the green waypoint. We were trying to give a light, floating game feel for the air room, so we made the air ninja have a higher jump and lower
gravity in order to be able to complete the pillar jumps. We added calm, mysterious ambient music to add to the game feel as well. In addition, the lighting in 
this room is white, which matches the air ninja texture and effects.

Now you will be in the fire room. You will need to light all 4 torches in the room in a specific order. Light the torch on the lowest stack of crates first, 
and then light the rest in ascending order. You can charge your fire energy on any lit torch or in the torch in the center of the room. Once you light the highest
torch by the door, the crates below it will fall down, revealing the exit. Cross through the exit over the green waypoint to continue. We tried to give the fire
room a fast-paced feel, so we created a sense of urgency by adding fast adventure music to the room. The room is also tinted orange to go along with the fire theme.

Now you will be in the water room. You must fill the buckets up before the timer runs out. Once you fill the first bucket up all the way, 
you will have 30 seconds to jump in all of the remaining buckets and fill them up. Make sure to stay in the buckets until they stop filling. Once you complete
this puzzle, the game is over. For the water room, we wanted a calm feel, so we added calm ambient music. We also gave the room blue lighting to match the water 
theme.


---

### Incomplete Requirements

- We did not implement the extra credit (A Star Search Path Planning)

### Known Issues

- There are no known issues

### Website

- http://unity.jonathantyates.com/milestone3