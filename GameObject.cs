#region

using System.Collections.Generic;
using System.Linq;
using Cludo_Engine.Components;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

#endregion

namespace Cludo_Engine {
    public static class CludoRenderTargetLayers {
        public static float Game() {
            return 0.5f;
        }

        public static float Gui() {
            return 0.9f;
        }

        public static float Lights() {
            return 0.6f;
        }
    }

    /// <summary>
    /// The base GameObject class. This class will not handle collision drawing sprites or animation or even scripting.
    /// All of those features will be seperated in specific Component's, and the Gameobject will just be the container
    /// for them. It has a Dictionary where the key is an int and the value is an Component. The int is the ID of the
    /// component, and Name just has to do with naming the components, but you can have multiple instances of the same name.
    /// The Type property in the component is of string and thats for when we create our own components, when we want to
    /// iterate through a specific type of Component and thats all, just for ease of use. When an object is added the ID is
    /// set to the count of the components +1.
    /// </summary>
    public class GameObject : IUpdateable, IDrawable {
        public GameObject(string name, Scene scene, Vector2 position) {
            OnComponentAddedEvent += GameObject_OnComponentAddedEvent;
            Rendertarget = "Game";
            RendertargetLayer = CludoRenderTargetLayers.Game();
            Components = new Dictionary<int, IComponent>();
            Name = name;
            Scene = scene;
            Tags = new List<string>();
            AddToTarget();
            Body = BodyFactory.CreateBody(scene.World, ConvertUnits.ToSimUnits(position), 0f, this);
            Body.UserData = this;
            Body.IsStatic = false;
            Position = position;
            _lastPosition = Vector2.Zero;
            Body.OnCollision += Body_OnCollision;
            _lastRotation = 2.232f;
        }

        public bool IgnoreDebug { get; set; }

        public void Update(GameTime gt) {
            if (Body.UserData == null)
                Body.UserData = this;
            // Iterate through each component and Update it.
            for (var i = 0; i < Components.Count; i++) {
                if (Components.ContainsKey(i) && Components[i] != null) {
                    Components.ElementAt(i).Value.Update(gt);
                } else {
                    Debugging.Debug.DoWarning(
                        "Gameobject ID: " + Id + " contains component " + Components.ElementAt(i).Key +
                        " that is null, removing from gameobject.", 2);
                    if (OnComponentRemovedEvent != null)
                        OnComponentRemovedEvent(this, new OnComponentRemovedEventArgs(Components.ElementAt(i).Value));
                    Components.Remove(Components.ElementAt(i).Key);
                }
            }
        }

        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB,
            FarseerPhysics.Dynamics.Contacts.Contact contact) {
            if (OnCollisionEvent != null) {
                OnCollisionEvent(this,
                    new OnCollisionEventArgs((GameObject)fixtureA.Body.UserData, (GameObject)fixtureB.Body.UserData));
            }
            return true;
        }

        private void GameObject_OnComponentAddedEvent(object sender, OnComponentAddedEventArgs args) {
            switch (args.Added.Type) {
                case "CircleCollider":
                var circle = (CircleCollider)args.Added;
                FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(circle.Radius), circle.Density, this.Body,new Vector2(ConvertUnits.ToSimUnits(circle.LocalX), ConvertUnits.ToSimUnits(circle.LocalY)), Body);
                Components.Remove(Components.Count - 1);
                Body.FixtureList[Body.FixtureList.Count - 1].UserData = this;
                Body.OnCollision += Body_OnCollision;
                Body.UserData = this;
                break;

                case "RectangleCollider":
                var rect = (RectangleCollider)args.Added;
                FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(rect.Width),
                    ConvertUnits.ToSimUnits(rect.Height), rect.Density,
                    new Vector2(ConvertUnits.ToSimUnits(rect.LocalX), ConvertUnits.ToSimUnits(rect.LocalY)), Body);
                Components.Remove(Components.Count - 1);
                Body.FixtureList[Body.FixtureList.Count - 1].UserData = this;
                Body.OnCollision += Body_OnCollision;
                Body.UserData = this;
                break;

                case "CapsuleCollider":
                var capsule = (CapsuleCollider)args.Added;
                FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(capsule.Width),
                    ConvertUnits.ToSimUnits(capsule.Height), capsule.Density,
                    new Vector2(ConvertUnits.ToSimUnits(capsule.LocalX), ConvertUnits.ToSimUnits(capsule.LocalY)),
                    Body);
                FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(capsule.Width / 2), capsule.Density, Body,
                    new Vector2(0, ConvertUnits.ToSimUnits(capsule.Height / 2)));
                FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(capsule.Width / 2), capsule.Density, Body,
                    new Vector2(0, ConvertUnits.ToSimUnits(-(capsule.Height / 2))));
                Body.OnCollision += Body_OnCollision;
                Body.UserData = this;
                break;

            }
        }

        #region Physical Properties

        public delegate void OnCollision(object sender, OnCollisionEventArgs args);

        public event OnCollision OnCollisionEvent;

        public float Mass {
            get { return Body.Mass; }
            set { Body.Mass = value; }
        }

        public Vector2 Position {
            get { return ConvertUnits.ToDisplayUnits(Body.Position); }
            set { Body.Position = ConvertUnits.ToSimUnits(value); }
        }

        public float Rotation {
            get { return Body.Rotation; }
            set { Body.Rotation = value; }
        }

        public bool Static {
            get { return Body.IsStatic; }
            set { Body.IsStatic = value; }
        }

        public bool StaticRotation {
            get { return Body.FixedRotation; }
            set { Body.FixedRotation = value; }
        }

        public Vector2 Velocity {
            get { return ConvertUnits.ToDisplayUnits(Body.LinearVelocity); }
            set { Body.LinearVelocity = ConvertUnits.ToSimUnits(value); }
        }

        #endregion Physical Properties

        #region Events

        public delegate void OnComponentAdded(object sender, OnComponentAddedEventArgs args);

        public delegate void OnComponentRemoved(object sender, OnComponentRemovedEventArgs args);

        public event OnComponentAdded OnComponentAddedEvent;

        public event OnComponentRemoved OnComponentRemovedEvent;

        #endregion Events

        #region GameObjectSpecific

        public Body Body { get; set; }
        public Dictionary<int, IComponent> Components { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Scene Scene { get; set; }
        public List<string> Tags { get; set; }

        #endregion GameObjectSpecific

        #region RenderTargetInformation

        public string RenderTarget {
            get { return Rendertarget; }
            set {
                RemoveFromTarget(RendertargetLayer, Rendertarget);
                Rendertarget = value;
                AddToTarget();
            }
        }

        public float RenderTargetLayer {
            get { return RendertargetLayer; }
            set {
                RemoveFromTarget(RendertargetLayer, Rendertarget);
                RendertargetLayer = value;
                AddToTarget();
            }
        }

        internal Graphics.CludoRenderTarget Target { get; set; }
        private string Rendertarget { get; set; }
        private float RendertargetLayer { get; set; }

        #endregion RenderTargetInformation

        #region IDrawableImplementation

        private Vector2 _lastPosition;
        private float _lastRotation;

        public void AddToTarget() {
            Scene.RenderTargets[RenderTarget].AddDrawable(RenderTargetLayer, this);
            Target = Scene.RenderTargets[RenderTarget];
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb) {
            // Iterate through each component and Draw it.
            for (var i = 0; i < Components.Count; i++) {
                if (Components.ContainsKey(i) && Components[i] != null) {
                    Components.ElementAt(i).Value.Draw(sb);
                } else {
                    Debugging.Debug.DoWarning(
                        "Gameobject ID: " + Id + " contains component " + Components.ElementAt(i).Key +
                        " that is null, removing from gameobject.", 2);
                    if (OnComponentRemovedEvent != null)
                        OnComponentRemovedEvent(this, new OnComponentRemovedEventArgs(Components.ElementAt(i).Value));
                    Components.Remove(Components.ElementAt(i).Key);
                }
            }
        }

        public void RemoveFromTarget(float layer, string target) {
            Scene.RenderTargets[target].RemoveDrawable(layer, this);
        }

        public bool TestIfDrawNeeded() {
            if (_lastRotation != Rotation || _lastPosition != Position) {
                _lastRotation = Rotation;
                _lastPosition = Position;
                return true;
            }
            return false;
        }

        #endregion IDrawableImplementation

        #region AddingGettingAndRemovingComponents

        /// <summary>
        /// Adds a component. Returns Component's ID.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="component"></param>
        public int AddComponent(string name, IComponent component) {
            component.Name = name;
            component.Id = Components.Count;
            Components.Add(component.Id, component);
            if (OnComponentAddedEvent != null)
                OnComponentAddedEvent(this, new OnComponentAddedEventArgs(component));
            return component.Id;
        }

        /// <summary>
        /// Gets Components by ID.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IComponent GetComponent(int id) {
            return Components[id];
        }

        /// <summary>
        /// Gets Components by name, returns IEnumerable of Component type. This can be iterated through like a list or an array.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<IComponent> GetComponents(string name) {
            return
                from entry in Components
                where entry.Value.Name == name
                select entry.Value;
        }

        /// <summary>
        /// Gets Components by Type, returns IEnumerable of Component type. This can be iterated through like a list or an array.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<IComponent> GetComponentsByType(string type) {
            return
                from entry in Components
                where entry.Value.Type == type
                select entry.Value;
        }

        /// <summary>
        /// Removes a component by ID.
        /// </summary>
        /// <param name="id"></param>
        public void RemoveComponent(int id) {
            if (OnComponentRemovedEvent != null)
                OnComponentRemovedEvent(this, new OnComponentRemovedEventArgs(Components[id]));
            Components.Remove(id);
        }

        /// <summary>
        /// Removes Components by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void RemoveComponents(string name) {
            var q =
                from entry in Components
                where entry.Value.Name == name
                select entry.Key;
            foreach (var c in q) {
                if (OnComponentRemovedEvent != null)
                    OnComponentRemovedEvent(this, new OnComponentRemovedEventArgs(Components[c]));
                Components.Remove(c);
            }
        }

        /// <summary>
        /// Removes Components by Type.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void RemoveComponentsByType(string type) {
            var q =
                from entry in Components
                where entry.Value.Type == type
                select entry.Key;
            foreach (var c in q) {
                if (OnComponentRemovedEvent != null)
                    OnComponentRemovedEvent(this, new OnComponentRemovedEventArgs(Components[c]));
                Components.Remove(c);
            }
        }

        #endregion AddingGettingAndRemovingComponents
    }
}