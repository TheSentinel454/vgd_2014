Milestone 4
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

- Elementals
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

- (AI)
- RAIN
  - https://www.assetstore.unity3d.com/en/#!/content/6563
  
- (Scripts)
- AutoTransparent/ClearSight
  - http://answers.unity3d.com/questions/44815/make-object-transparent-when-between-camera-and-pl.html
- CameraFade
  - http://wiki.unity3d.com/index.php?title=FadeInOut


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

The Xbox controller controls are detailed in the help screen of our game, but if you are using a keyboard and mouse, use the following controls:

- Move with WASD keys
- '1' Changes to the Air Ninja (or back to Base Ninja if already in Air Ninja form)
- '2' Changes to the Fire Ninja (or back to Base Ninja if already in Fire Ninja form)
- '3' Changes to the Water Ninja (or back to Base Ninja if already in Water Ninja form)
- Left Click to Attack
- Moving the Mouse moves the camera
- Hold SHIFT while moving to run
- Press SPACE to jump

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

### Starting Scene

- Scenes/NinjaMorph

### Website

- http://unity.jonathantyates.com/milestone4
