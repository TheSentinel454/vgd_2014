Milestone 3
------------

**Luke Tornquist**  -  *luke.tornquist@gatech.edu* - *ltornquist3*

**Jonathan Yates**  -  *jyates32@gatech.edu*  -  *jyates32*

**Evan LaHurd**  -  *elahurd3@gatech.edu*  -  *elahurd3*

---

### External Resources

- (Models)
- Ninja Model
  - https://www.assetstore.unity3d.com/en/#!/content/12679
- Cyborg Model
  - https://www.assetstore.unity3d.com/en/#!/content/10705
- Wood Barrel
  - https://www.assetstore.unity3d.com/en/#!/content/4427
- Torch
  - https://www.assetstore.unity3d.com/en/#!/content/7275
- Crates
  - https://www.assetstore.unity3d.com/en/#!/content/8836

- Fire, Flame, Dust, Bubble, Waterfall, Water Fountain, and Sparkle Particle Effects
  - Assets -> Import Package -> Particles

- Water (Ponds)
  - Assets -> Import Package -> Water (Basic)

- Rustic Door
  - https://www.assetstore.unity3d.com/en/#!/content/861
- Terrain
  - https://www.assetstore.unity3d.com/en/#!/content/6

- (Audio)
- Grass Footsteps
  - http://www.freesfx.co.uk/sfx/footsteps (Footsteps Forest 01)
- Water Footsteps
  - http://www.freesfx.co.uk/sfx/footsteps?p=3 (Footsteps In Water 1)
- Wood Footsteps
  - http://www.freesfx.co.uk/sfx/footsteps?p=2 (Footsteps on hollow wooden surface)
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


**Controls:**
- Move with WASD keys
- Hold SHIFT while moving to run
- Press SPACE to jump

If you stay out of LoS (Line of Sight) of the soldier, then you can easily see the patrol state.  This is the default state for the soldier while there is no player in LoS.  You can see him follow a set of waypoints.  If you move within LoS of the solder, the soldier will purse the player.  The soldier will not fire until he is within a certain distance of the player.  You can also let the soldier get close to the player, and once he is within a certain cautionary distance, he will stop moving and just continue firing.  We mainly added this to make it more of a realistic behavior considering the soldier is firing a ranged weapon.  The flee state can be initiated by moving to the soldier and doing some damage to him.  Once he drops below a certain health threshold (50) he will transition into the flee state and run away whenever he sees the player.  Once the soldier cannot see the player, the soldier will attempt to go back to patrolling.  You will also notice that when the soldier is firing, he is predictively firing based on the player position and velocity. You should be able to see this relatively easily while running away from the soldier if you turn to the right or left.

---

### Incomplete Requirements

- We did not implement the extra credit (A Star Search Path Planning)

### Known Issues

- There are no known issues

### Website

- http://unity.jonathantyates.com/milestone3