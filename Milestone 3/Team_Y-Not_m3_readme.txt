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


### Fulfilled Requirements

- AI State Machine implemented 
  - Soldier can be in one of 3 states
  - Patrol state is when there is no player in Line of Sight, and follows a set path of waypoints
  - Attack state is when there is a player in Line of Sight, and the soldier will pursue the player and attack
  - If while in the Attack state the soldier catches up to the player, he will maintain a certain distance from the player and continue to fire since he has a ranged attack
  - Flee state is whenever the soldier takes damage, he will flee away from the player for 4 seconds
- Added position prediction for firing, so that the soldier will try to lead his shots in such a way to predict the position of the player

#### Instructions


**Controls:**
- Move with WASD keys
- Hold SHIFT while moving to run
- Press SPACE to jump



---

### Incomplete Requirements

- We did not implement the extra credit (A Star Search Path Planning)

### Known Issues

- There are no known issues

### Website

- http://unity.jonathantyates.com/milestone3