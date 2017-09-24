# Corund

Corund is a 2D game engine for mobile games, based on Monogame (open implementation of XNA). Inherit from a variety of existing objects and behaviours to easily compose a game or a prototype.

## Core concepts

### Scene graph

Each scene contains a list of objects, and each object may contain children of its own, which makes the scene a tree-like structure. Moving, rotating and scaling parents affects its children.

### Behaviours

Each object can be augmented with multiple behaviours, which modify the object's properties at runtime. Behaviours can be used to do the following and much more:

* Move the object
* Fade the object in and out
* Animate any property of the object (position, angle, scale, etc)
* Detect and handle touch events
* Create parallax effects

### Interpolation

You can create smooth transitions for any property to make the animation look more vivid. A lot of common predefined patterns are included, or you can also create your own.

## Expected set of features (in v1.0):

- [x] Scene manager
  - [x] Resolution adaptation strategies (center, adjust, etc.)
  - [x] Scene transitions
- [ ] Object types
  - [x] Sprite
  - [x] Object group (layer)
  - [x] Particle system
  - [x] Text
  - [ ] Polygon
- [x] Sprites
  - [x] Tiled sprite
  - [x] Animated sprite
- [x] Collision detection
  - [x] Multiple boxes per object
  - [x] Rotation and scale support
- [x] Touch events
  - [x] Multi touch
  - [x] Swipe event
- [x] Sound manager
- [x] Rendering effects
  - [x] Blending modes
  - [x] Built-in shader effects
- [ ] Behaviours
  - [x] Fade in / out
  - [x] Custom property animation
  - [ ] Movement
    - [x] Path movement
    - [x] Spline movement
    - [ ] Friction
    - [x] Jitter
  - [ ] Misc
    - [x] Blinking
    - [ ] Explosion
- [ ] UI elements:
  - [ ] Grid
  - [ ] Button
  - [ ] Scrollable list
  - [ ] Fancy text
- [ ] Debug capabilities
  - [x] Collision box visualization
  - [x] FPS counter
  - [ ] SpriteBatch counter
  
## Later features

* Circle shape for collision detection
* More shaders
  * Glow
  * Shockwave
  * Distortion
* Visual editor
  * Particle system configuration
  * Bounding box
  * Sprite sheet
