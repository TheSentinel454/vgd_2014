Milestone 4
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

**Character Animation: **
- Created a Mecanim Animation Controller that:
  - Uses speed and direction parameters as blending parameter
  - Blends from default state blend tree to 4 additional blend trees and
    each of the additional blend trees blend to at least 5 other animation
    clip motions
  - 3 additional animation layers that blend different masked animations
    together
  - Using Rain AI, created scripts that set the Mecanim parameters so that
    he can move around and steer
  - Integrated the animations into the game feel by giving them to our
    new and improved AI model who better looks like the player character

#### Instructions

**Controls:**
- Move with WASD keys
- Hold SHIFT while moving to run
- Press SPACE to jump
- Left click to attack

To see our animation blend trees in action, after you entered the air, fire
or water levels, there will be an Cyborg Troop roaming each of the levels.
He can be recognized by a glowing red light at his face. Walk up to the AI
and he will sense your presence and start running after you.  You will notice that as he gets close to you he blends from running to walking
to idle. Likewise, you will notice that he lifts his arm to aim his gun
which is done by manually blending the layer weights.  Walk towards him
as if you are trying to stand on top of him and you will notice he blends
to walking backwards by setting the speed parameter.  Now start running away from him and notice that his speed is blended towards walking and
as you keep running away notice he blends into running.


---

### Incomplete Requirements

- We had no incomplete requirements

### Known Issues

- Due to the fact that we use Mecanim animations, when moving close
  to the AI and he backs up, he can back through walls because he no
  longer pays attention to the nav mesh.

### Website

- http://unity.jonathantyates.com/milestone4
