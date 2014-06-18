Milestone 2
------------------

### Air Garden Fulfilled Requirements

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
These are not affected by the wind as they are too heavy.  There are also some plants that are protruding from the bridges that are setup to be traps.  There is a trigger that will sense the player and create the hinge that causes the trap door to fall

### Fire Garden Fulfilled Requirements

- Physics implemented - character is controlled by NinjaController script, but it will apply force to rigid bodies when it collides with them.
- Game feel - red texture on fire ninja, fire ninja catches on fire when he touches fire and can catch other things on fire, fire can be put out in water
- Garden implemented - fire garden has dusky sky and is ontop of a large textured fire mountain, 
- Garden switching - can switch between gardens by pressing "1", "2", and "3"
- At least 5 geometry nodes controlled by physics - 5 barrels, 2 doors
- 2 joints - attached to the doors
- Variable height terrain implemented - fire garden is ontop of a mountain

#### Fire Garden Instructions

**Controls:**
- Move with WASD keys
- Hold SHIFT while moving to run
- Press SPACE to jump
- Scroll to zoom in and out of the character

- After pressing “2”, the fire scene should load. You should be ontop of a fire mountain.
- As you run around, notice the ground sound effects that match the ninja’s movement.
- Walk into the pond that is located in front of you and you will hear the sound effect change to the water foot steps. 
- Walk back out of the pond and touch the fire tree. You will now be on fire. Since you are playing as the fire ninja,
  the fire ninja can catch fire when he touches an object that is on fire.
- Now run over to the barrels that are located near the fire tree.
- Run into the barrels and watch as the barrels catch on fire and get pushed around by the ninja.
- If you want to put out your ninja, you can run into the water and the fire should go out.
- Likewise, if you push any of the barrels into the water they will also go out.  
- Now leave the fire tree area and walk out of the first gate. The gate will hinge open and spring shut as you walk through it.
- There is also a second gate in front of you that you can walk through again.
- Continue walking down fire mountain and enjoy the variable height terrain and scenery.
- If you want to view the scenery, you can use right click and drag to look around.



### Water Garden Fulfilled Requirements

*Evan LaHurd*
*elahurd3@gatech.edu*
*http://www.prism.gatech.edu/~elahurd3/*

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
- Two sounds when interacting with environment
  - sound when walking on grass terrain
  - sound when walking in water

#### Water Garden Instructions 

**Controls:**
- Move with WASD keys
- Hold SHIFT while moving to run
- Press SPACE to jump
- Scroll to zoom in and out of the character

- Pressing the "3" key will load the water garden scene
- Observe the water ninja and his blue texture and bubbly aura
- Look up at the overcast sky by holding Right-Click on your mouse and moving the mouse upwards
- Move towards the barrels (notice the footstep sounds when on the grass terrain)
- Push them around by running into them to see that they are influenced by physics
- Move over to the 2 ponds and walk around in them (notice the water footstep sounds when in the water)
- Observe the waterfalls and their sounds
- Move over the fires at the pond entrances and notice how they are extinguished (if you run over them too fast, it will only put them out a little bit; if you stay in contact long enough, they are fully extinguished). 
- Move across the map from where you originally spawned, and run through the doors (notice the hinge mechanism and how it only opens so far)
- Move/jump up the stairs to the plateau (variable height terrain)


