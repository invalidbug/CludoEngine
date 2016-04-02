using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CludoEngine {

    public abstract class ComponentSystem {

        public delegate void OnComponentAdded(object sender, OnComponentAddedEventArgs args);

        public delegate void OnComponentRemoved(object sender, OnComponentRemovedEventArgs args);

        public event OnComponentAdded OnComponentAddedEvent;

        public event OnComponentRemoved OnComponentRemovedEvent;

        public Dictionary<int, IComponent> Components { get; set; }

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

        public void DrawComponets(Microsoft.Xna.Framework.Graphics.SpriteBatch sb) {
            // Iterate through each component and Draw it.
            for (var i = 0; i < Components.Count; i++) {
                if (Components.ContainsKey(i) && Components[i] != null) {
                    Components.ElementAt(i).Value.Draw(sb);
                } else {
                    if (OnComponentRemovedEvent != null)
                        OnComponentRemovedEvent(this, new OnComponentRemovedEventArgs(Components.ElementAt(i).Value));
                    Components.Remove(Components.ElementAt(i).Key);
                }
            }
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

        public void UpdateComponents(GameTime gt) {
            // Iterate through each component and Update it.
            for (var i = 0; i < Components.Count; i++) {
                if (Components.ContainsKey(i) && Components[i] != null) {
                    Components.ElementAt(i).Value.Update(gt);
                } else {
                    if (OnComponentRemovedEvent != null)
                        OnComponentRemovedEvent(this, new OnComponentRemovedEventArgs(Components.ElementAt(i).Value));
                    Components.Remove(Components.ElementAt(i).Key);
                }
            }
        }
    }
}