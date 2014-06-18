Milestone 2
------------

**Luke Tornquist** (Air Garden)
*luke.tornquist@gatech.edu*

**Jonathan Yates** (Fire Garden)
*jyates32@gatech.edu*
*jyates32*

**Evan LaHurd** (Water Garden)
*elahurd3@gatech.edu*
*elahurd3*

---

### Air Garden Fulfilled Requirements

- Physics implemented 
  - The ninja is controlled by the NinjaController script, but it will apply force to rigid bodies when it collides with them
- Game feel 
  - Transparent white texture on air ninja
  - Air ninja blows fire when he runs by it
  - Air ninja puts out fire when he steps on it
  - Air ninja blows trees around when he is near them
- Garden implemented 
  - Air garden has sunny sky and various trees scattered around it
- Garden switching 
  - Can switch between gardens by pressing "1", "2", and "3"
- At least 5 geometry nodes controlled by physics
  - 9 barrels
  - 2 doors
- 2 joints 
  - Joints attached to the 2 doors
- Variable height terrain implemented
  - Bridges over pond
  - Raised terrain around pond
- At least two sounds when interacting with environment
  - sound when walking on grass terrain
  - sound when walking in water
  - sound when walking over bridge

#### Air Garden Instructions


**Controls:**
- Move with WASD keys
- Hold SHIFT while moving to run
- Press SPACE to jump
- Scroll to zoom in and out of the character

Pressing “1” loads the air scene. This scene is basically a large grass and palm tree covered terrain with a fenced off garden.
The first thing that you will notice about the air ninja is that it’s partially transparent, unlike the other types.
This is to give that airy feeling as though you could almost fly. So if I had to choose one word to describe the feel, it would be airy or smooth.
The air ninja is also affected less by gravity, so jumps feel more like gliding. This allows the ninja to jump higher and farther than the other types.
The air ninja has 3 standard walking audio sounds that correspond to the surface being traversed, either grass, wood, or water.
The sound also matches the speed by which the ninja is moving. Another important detail is the aura that surrounds the ninja.
It is to mimic a cyclone that is basically circling around the ninja.You can see how this affects the trees and torches by walking near them. 
They are essentially pushed away from the ninjas location. If you get close enough to the torches, the ninja will actually blow the torch out. 
The trees simply sway more when the ninja is close to them. The gate to the garden is controlled by 2 hinges that also have a spring component
so that the doors will always try to close to the natural resting position.  There are barrels within the garden that can be pushed around with ninja. 
These are not affected by the wind as they are too heavy.  There are also some plants that are protruding from the bridges that are setup to be traps.
There is a trigger that will sense the player and create the hinge that causes the trap door to fall.


---


### Fire Garden Fulfilled Requirements

- Physics implemented 
  - The ninja is controlled by the NinjaController script, but it will apply force to rigid bodies when it collides with them
- Game feel 
  - Red texture on fire ninja
  - Fire ninja catches on fire when he touches fire
  - Fire ninja can catch other things on fire
  - Fire can be put out in water
- Garden implemented 
  - Fire garden has dusky sky and is ontop of a large textured fire mountain
- Garden switching 
  - Can switch between gardens by pressing "1", "2", and "3"
- At least 5 geometry nodes controlled by physics
  - 5 barrels
  - 2 doors
- 2 joints 
  - Joints attached to the 2 doors
- Variable height terrain implemented
  - Fire garden is ontop of a mountain
- At least two sounds when interacting with environment
  - sound when walking on grass terrain
  - sound when walking in water

#### Fire Garden Instructions

**Controls:**
- Move with WASD keys
- Hold SHIFT while moving to run
- Press SPACE to jump
- Scroll to zoom in and out of the character

After pressing “2”, the fire scene will load. The game feel that the fire garden is going for is hot and smokey.
The first thing you will notice is that the fire ninja has on a red suit in contrast to the other two elemental ninjas respective colors.
To help portray the hot and smokey aspect of my game feel, you will notice the everlasting tree of fire behind you and the hot spring in front of you.
If you walk behind you using WASD and touch the everlasting tree of fire, you will notice your ninja catch fire. Being a fire ninja means that
he can catch himself along with other objects on fire.  Now run (using shift) over to the barrels and watch as he catches each of the
5 barrels on fire and knocks the barrels around. Notice as you walk along the ground the footstep sound effects. 
Now that you and each of the barrels are on fire, run and jump into the hot spring. You can jump by pressing spacebar. As you run into the hot spring,
notice two things: your ninja is no longer on fire (the water puts him and any other object that is on fire out) and the ninja now sounds
like he is walking in water.  Now that we have explored entirety of the top of fire mountain, let’s leave the area.  Walk over to the door 
located next to the hot spring and push against the door.  The door will hinge open and allow access into the next area.  As you walk along the 
variable terrain, make your way to the next gate and push through it like you just did.  Now you can continue to walk along the fire mountain path. 
If you want to view more of the scenery, you can right click and drag to move the camera around.  Likewise, if you want to zoom out of your character, 
you can do so by scrolling.


---


### Water Garden Fulfilled Requirements

- Physics implemented 
  - The ninja is controlled by the NinjaController script, but it will apply force to rigid bodies when it collides with them
- Game feel 
  - Bubbly particle aura around water ninja
  - Blue texture on water ninja 
  - Water ninja puts out fires when he runs through them
- Water garden implemented 
  - Overcast sky
  - 2 ponds with waterfalls pouring into them
  - Water sounds for the waterfalls and for walking in the ponds
- Garden switching 
  - Player can switch between gardens by pressing "1", "2", and "3"
- At least 5 geometry nodes controlled by physics 
  - 3 barrels
  - 2 doors
- 2 joints 
  - Joints attached to the 2 doors
- Variable height terrain implemented 
  - Raised terrain around the ponds
    Stairs leading up to the large plateau across the map from the player spawn
- At least two sounds when interacting with environment
  - sound when walking on grass terrain
  - sound when walking in water

#### Water Garden Instructions 

**Controls:**
- Move with WASD keys
- Hold SHIFT while moving to run
- Press SPACE to jump
- Scroll to zoom in and out of the character

Pressing the "3" key will load the water garden scene. Observe the water ninja. To capture the watery game feel of this garden, I added a blue texture and
bubbly aura to him. I also added an overcast sky to give the feeling that it's about to rain. Look up at the sky by holding Right-Click on your mouse and 
moving the mouse upwards. Move over towards the 2 ponds, and notice the footstep sounds while on the grassy terrain. Once you reach the ponds, walk around
in them, and notice the water footstep sounds while in the water. Observe the waterfalls and their sounds as well. Move over the fires at the pond entrances,
and notice how they are extinguished. If you run over them too fast, it will only put them out a little bit; if you stay in contact long enough, they are fully
extinguished. Move across the map from where you originally spawned, and run through the doors. Notice the hinge mechanism and how the doors only open so far.
Move/jump up the stairs (variable height terrain) to the plateau and you'll notice 3 barrels. Push them around (or off the cliff), and notice how they are
influenced by physics.

