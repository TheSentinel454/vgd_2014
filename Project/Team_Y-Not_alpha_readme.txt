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


### Game Features

- Player
  - Element Changing Ninja
  - Fire Ninja
    - Runs faster and jumps higher
    - Lights wooden objects on fire

- UI
  - Introduction screen with help screen
  - Textured bars for Health and Energy
  - Enemy Health bars display over enemy

- AI

- Puzzle

- 3 Element specific rooms
  - Soldier can be in one of 3 states
  - Patrol state is when there is no player in Line of Sight, and follows a set path of waypoints
  - Attack state is when there is a player in Line of Sight, and the soldier will pursue the player and attack when within attack range
  - If while in the Attack state the soldier catches up to the player, he will maintain a certain distance from the player and continue to fire since he has a ranged attack
  - Flee state is whenever the soldier gets below 50 health, he will flee away whenever he sees the player
- Added position prediction for firing, so that the soldier will try to lead his shots in such a way to predict the position of the player

#### Instructions

**Controls:**
- Move with WASD keys
- '1' Changes to the Air Ninja
- '2' Changes to the Fire Ninja
- '3' Changes to the Water Ninja
- Left Click to Attack
- Moving the Mouse moves the camera
- Hold SHIFT while moving to run
- Press SPACE to jump

If you stay out of LoS (Line of Sight) of the soldier, then you can easily see the patrol state.  This is the default state for the soldier while there is no player in LoS.  You can see him follow a set of waypoints.  If you move within LoS of the solder, the soldier will purse the player.  The soldier will not fire until he is within a certain distance of the player.  You can also let the soldier get close to the player, and once he is within a certain cautionary distance, he will stop moving and just continue firing.  We mainly added this to make it more of a realistic behavior considering the soldier is firing a ranged weapon.  The flee state can be initiated by moving to the soldier and doing some damage to him.  Once he drops below a certain health threshold (50) he will transition into the flee state and run away whenever he sees the player.  Once the soldier cannot see the player, the soldier will attempt to go back to patrolling.  You will also notice that when the soldier is firing, he is predictively firing based on the player position and velocity. You should be able to see this relatively easily while running away from the soldier if you turn to the right or left.

---

### Incomplete Requirements

- None that we know of

### Known Issues

- https://github.com/TheSentinel454/vgd_2014/issues?milestone=3&state=open

### Website

- http://unity.jonathantyates.com/alpha