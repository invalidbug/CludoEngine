# Cludo-Engine - A powerful 2d Monogame Game Engine!


Current Version: 0.7.3 Beta

[insert Travis CL thing here once I can figure out my error]

#What does the engine Aim to accomplish?
  A problem that has plagued game programming for quite some time, having to create indepth things from scratch. We have slowly worked towards making reusable code libraries however I feel its not quite there yet. The fact that for most engines you have to create a AI system from almost scratch in most cases is crazy. I feel once my Engine has reached all of its accomplishments, a Game like Undertale should take no longer than 200 hours to make(assuming you already have art and sound done).

#Features
- Scene System. Load, unload and set the current scene. You can have multiple scenes loaded at once but not running to enable really fast loading times!
- Component System. Easily add Pre-made or custom Components to your game objects, and remove or modify at run time!
- Farseer Physics. Super easy to use Farseer Physics implementation. At the moment I haven't fully implemented everything Farseer has to offer, but I plan to. If there's anything that is super needed, you can access the body of a gameobject by GameObjectInstance.body and SceneInstance.World. Current implementation enables multiple fixtures in one body, which includes Capsule and Rectangle at the moment along with converting a Texture to body. TextureInstance.ConvertToBody(SceneInstance), returns Body.
- Zoomable, Rotatable, Camera! Also includes CameraModes to enhance usability even more.
- SceneSequence, Program specific Events by inheriting the ISequenceStep and easily move through a Scene.
- Not fully fleshed out, but a Debug System.
- Optimized Rendering. We take the already optimized Monogame rendering and enhance it even more by adding RenderTargetLayers that contain IDrawables and asking if anything changed, if nothing changed for the entire layer, why re-render it? That's exactly what it aims to do, it doesn't re-render what doesn't need to be re-rendered!
- Almost complete Tiled integration. With our Tile Map class, you can load up Tiled maps, load up Pre-made prefabs and convert a layer to a Collision layer! For information on how to do this, please check the documentation.(Not yet done)
- A very unstable GUI system. I haven't dropped the GUI but it's literally two weeks old and hasn't yet been through heavy testing, how ever we can confirm it is working.

#Milestones
Over time, there is bound to be more. This is what I will be focusing on for the next bit of time.
- Finish commenting out the engine
- Create code examples for github page
- Finish FarseerPhysics implementation
- Complete Tiled implementation
- Complete GUI system with ComboBoxes, Textboxes, Progressbars, and more controls.
- A Platformer Character Controller. Because right now, it is insanely hard for someone who has never had to make a platformer character that uses true physics, to make a moveable character in my Engine(assuming its a platformer). This problem arises in Engines like unity, but it is fixed by making a Character Controller that fixes all of the physic problems with real-like physics.
- More fleshed out Scene Sequences, don't get confused with a Scene in this instance with a Level like scene. This scene is more for when a character talks, the camera zooms into the character, zooms to something else when hes done talking and so on. Right now, there isn't much code there to make it really easy for the coder, which is my intention. For the expirenced, its a piece of cake though.
- I'm not sure how to tackle the AI problem explained the the "What does the engine Aim to accomplish?", maybe I will create a bunch of AI classes that control said object to do said thing? This would make it very limited though, which is the opposite of what I want.
- Networking library, at the moment, my friend has created a Amazing library, that I would like to be supported, however I do not yet have permission and I'm not sure how it would work with Linux or other Platforms.
- And more!


#Credits
- Darin Boyer - Lead Developer and Project Manager. darinboyer.website & cludogames.com
- TiledSharp - A library used to help load Tiled Maps. https://github.com/marshallward/TiledSharp - Licensed under the Apache License. See https://github.com/marshallward/TiledSharp/blob/master/LICENSE for more information.
- Monogame - A library used to help Render anything on the screen. http://www.monogame.net/ - Licensed under the ﻿Microsoft Public License (Ms-PL). See https://github.com/mono/MonoGame/blob/develop/LICENSE.txt for more information.
- Farseer Physics - A library used to help implement Physics into the engine. https://farseerphysics.codeplex.com/ - Licensed under the Microsoft Permissive License (Ms-PL) v1.1. See https://farseerphysics.codeplex.com/license for more information.
- OpenTK - A library used by Monogame, to help Render things. http://www.opentk.com/ - Licesned under the The Open Toolkit library license. See http://www.opentk.com/project/license for more information.

#License
There is 2 licenses

License 1(This is the license used unless stated otherwise with our sales (darinboyer2000@gmail.com )
Creative Commons Attribution-NonCommercial 3.0 United States (CC BY-NC 3.0 US) https://creativecommons.org/licenses/by-nc/3.0/us/

License 2($20):
It will be the MIT license, but obvisouly will have to pay for said license.

Contact darinboyer2000@gmail.com for sales.
