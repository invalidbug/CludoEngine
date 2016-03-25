# Cludo-Engine

A powerful 2d Monogame Game Engine!
---
Current Version: 0.7.3 Beta

[insert Travis CL thing here once I can figure out my error]

#Features
- Scene System. Create scenes load them, set them as the current scene and unload them!
- Component System. Easily add Pre-made or self made Components to your gameobjects, and remove or modify at runtime!
- Farseer Physics. Super easy to use Farseer Physics implementation. At the moment I haven't fully implemented every Farseer has to offer, but plan to. If theres anything that is super needed you can access the body of a gameobject by gameobjectinstance.body and SceneInstance.World. Current implementation enables multiple fixtures in one body, which includes Capsule and Rectangle at the moment along with converting a Texture to body. TextureInstance.ConvertToBody(SceneInstance), returns Body.
- Zoomable, Rotatable, Camera! Also includes Camera Modes to enchance usability even more.
- SceneSequence, Program specific Events by inheriting the ISequenceStep and easily move through a scene.
- Not fully fleshed out, but a Debug System.
- Optimized Rendering. We take the already optimized Monogame rendering and enchance it even more by adding RenderTargetLayers that contain IDrawables and asking if anything changed, if nothing changed for the entire layer, why rerender it? That's exactly what it aims to do, it doesn't rerender what doesn't need to be rerendered!
- Almost complete Tiled intregration. With our tilemap class, you can 
