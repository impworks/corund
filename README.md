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

* Scene manager
  * Resolution adaptation strategies (center, adjust, etc.)
  * Scene transitions
* Object types
  * Sprite
  * Object group (layer)
  * Particle system
  * Text
  * Polygon
* Sprites
  * Tiled sprite
  * Animated sprite
* Collision detection
  * Multiple boxes per object
  * Rotation and scale support
* Touch events
  * Multi touch
  * Swipe event
* Sound manager
* Rendering effects
  * Blending modes
  * Alpha blending
* Behaviours
  * Fade in / out
  * Custom property animation
  * Movement
    * Path movement
    * Spline movement
    * Gravity
    * Friction
    * Jitter
  * Misc
    * Blinking
    * Timebomb
    * Explosion
* Debug capabilities
  * Collision box visualization
  * FPS counter
  * SpriteBatch counter
  
## Later features

* Circle shape for collision detection
* UI elements:
  * Grid
  * Button
  * Scrollable list
* Shader effects
  * Blur
  * Glow
  * Color overlay
  * Shockwave
  * Warp
* Visual editor
  * Particle system configuration
  * Bounding box
  * Sprite sheet
