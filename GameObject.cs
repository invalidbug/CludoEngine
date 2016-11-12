#region

using CludoEngine.Components;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#endregion

namespace CludoEngine {

    /// <summary>
    /// The base GameObject class. This class will not handle collision drawing sprites or animation or even scripting.
    /// All of those features will be seperated in specific Component's, and the Gameobject will just be the container
    /// for them. It has a Dictionary where the key is an int and the value is an Component. The int is the ID of the
    /// component, and Name just has to do with naming the components, but you can have multiple instances of the same name.
    /// The Type property in the component is of string and thats for when we create our own components, when we want to
    /// iterate through a specific type of Component and thats all, just for ease of use. When an object is added the ID is
    /// set to the count of the components +1.
    /// </summary>
    public class GameObject : ComponentSystem, IUpdateable {

        public GameObject(string name, Scene scene, Vector2 position) {
            OnComponentAddedEvent += GameObject_OnComponentAddedEvent;
            Components = new Dictionary<int, IComponent>();
            Name = name;
            Scene = scene;
            Tags = new List<string>();

            Body = BodyFactory.CreateBody(scene.World, ConvertUnits.ToSimUnits(position), 0f, this);
            Body.UserData = this;
            Body.IsStatic = false;
            Position = position;
            Body.OnCollision += Body_OnCollision;
        }

        public bool IgnoreDebug { get; set; }

        public virtual void Update(GameTime gt) {
            if (Body.UserData == null) {
                Body.UserData = this;
            }
            UpdateComponents(gt);
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
                FixtureFactory.AttachCircle(ConvertUnits.ToSimUnits(circle.Radius), circle.Density, Body,
                    new Vector2(ConvertUnits.ToSimUnits(circle.LocalX), ConvertUnits.ToSimUnits(circle.LocalY)),
                    Body);
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

        #region GameObjectSpecific

        public Body Body { get; set; }
        public int Id { get { return Body.BodyId; } }
        public string Name { get; set; }
        public Scene Scene { get; set; }
        public List<string> Tags { get; set; }

        #endregion GameObjectSpecific

        #region DrawingImplementation

        /// <summary>
        /// Draws items. This function alone relies on the NormalDrawingSystem, if you want to change that you need to override this function.
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb) {
            DrawComponets(sb);
        }

        #endregion DrawingImplementation
    }
}