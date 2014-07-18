Alpha
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


### Game Features

- Player
  - Element Changing Ninja
  - XBox360 Controller Integration
  - Fire Ninja
    - Runs faster and jumps higher
    - Lights wooden objects on fire
  - Water Ninja
    - Runs faster and jumps higher
    - Can fill objects with water
    - Can put out fire (some, not all)
  - Air Ninja
    - Runs faster, and jumps higher than all others
    - Affected less by gravity (falls slower after jumping)
    - Can blow out fire (some, not all)
    - Can traverse larger gaps
  - Each element has unique particle effect to match theme
  - Elements can be charged by standing in the corresponding element (e.g. Charge fire by standing in fire)
  - Zen (Health) can be charged if you charge an element that is already at maximum

- UI
  - Introduction screen with help screen
  - Textured bars for Health and Energy
  - Enemy Health bars display over enemy
  - Fade In/Out on Level transitions

- AI
  - Soldier can be in one of 3 states
  - Patrol state is when there is no player in Line of Sight, and follows a set path of waypoints
  - Attack state is when there is a player in Line of Sight, and the soldier will pursue the player and attack when within attack range
  - If while in the Attack state the soldier catches up to the player, he will maintain a certain distance from the player and continue to fire since he has a ranged attack
  - If the player gets closer than the maintain distance, the soldier will back away from the player
  - Position prediction for firing
  - Mecanim Animation
    - 5 Blend trees
    - 30+ Animations
  - Soldier deals and takes damage
  - When killed the soldier will decay and get destroyed after a short time

- Puzzle
  - Air Puzzle
    - Platforms that can only be traversed using Air's low gravity
    - Button that must be pressed to shift a platform in order to complete the puzzle
  - Fire Puzzle
    - Torches that can be lit on fire
    - Torches must be lit in order
    - If order is incorrect, the puzzle is reset
    - Order is in order from smallest to largest pile
  - Water Puzzle
    - Buckets can be filled with water
    - Once one bucket is filled, a 30 second timer is started
    - All buckets must be filled within 30 seconds or the puzzle will reset

#### Instructions

**Controls:**

The Xbox controller controls are detailed in the help screen of our game, but if you are using a keyboard and mouse, use the following controls:

- Move with WASD keys
- '1' Changes to the Air Ninja (or back to Base Ninja if already in Air Ninja form)
- '2' Changes to the Fire Ninja (or back to Base Ninja if already in Fire Ninja form)
- '3' Changes to the Water Ninja (or back to Base Ninja if already in Water Ninja form)
- Left Click to Attack
- Moving the Mouse moves the camera
- Hold SHIFT while moving to run
- Press SPACE to jump

To charge your elements, you need to be standing in the particular element that you want to charge while in that elemental form or in the base state. For 
example, to charge fire, you should be standing in fire while in the fire elemental form or while you're in the base form. To attack, click the left mouse button.

The objective of the game is to solve the puzzles in all 3 rooms which are all themed based on the 3 elements that the ninja can change into. In each room 
there is an enemy that you can kill by attacking it.
 
The first room is the air room. To beat this puzzle you need to jump up the pillars and reach the exit. To jump across the pillars, you need to be in the
air form, so change to that form. If you run out of air energy, you can charge it in the misty substance that is on the ground where you spawned. 
While jumping, you should sprint and jump only once. Pressing jump repeatedly usually results in a double jump which makes you jump off the pillar. 
Jump up the first 3 pillars, and you will hit a fork. Jump straight to reach the pillar with the red button, and walk into it to press it. 
You will hear an obvious "puzzle complete" sound once you do so. Head back to the third pillar, and take the right path (if you're facing the button). 
Now the pillar that was previously too far from the green way point will be close enough for you to make the jump. Jump across the rest of the pillars 
to the exit with the green way point. We were trying to give a light, floating game feel for the air room, so we made the air ninja have a higher jump and lower
gravity in order to be able to complete the pillar jumps. We added calm, mysterious ambient music to add to the game feel as well. In addition, the lighting in 
this room is white, which matches the air ninja texture and effects.

Now you will be in the fire room. You will need to light all 4 torches in the room in a specific order. Light the torch on the lowest stack of crates first, 
and then light the rest in ascending order. You can charge your fire energy on any lit torch or in the torch in the center of the room. Once you light the highest
torch by the door, the crates below it will fall down, revealing the exit. Cross through the exit over the green way point to continue. We tried to give the fire
room a fast-paced feel, so we created a sense of urgency by adding fast adventure music to the room. The room is also tinted orange to go along with the fire theme.

Now you will be in the water room. You must fill the buckets up before the timer runs out. Once you fill the first bucket up all the way, 
you will have 30 seconds to jump in all of the remaining buckets and fill them up. Make sure to stay in the buckets until they stop filling. Once you complete
this puzzle, the game is over. For the water room, we wanted a calm feel, so we added calm ambient music. We also gave the room blue lighting to match the water 
theme.

---

### Incomplete Requirements

- None that we know of

### Known Issues

- https://github.com/TheSentinel454/vgd_2014/issues?milestone=3&state=open

### Website

- http://unity.jonathantyates.com/alpha