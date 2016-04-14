#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace CludoEngine {
    public class GameObjectManager : IUpdateable {
        public delegate void OnGameObjectAdded(object sender, OnGameObjectAddedEventArgs args);

        public delegate void OnGameObjectRemoved(object sender, OnGameObjectRemovedEventArgs args);

        private Scene _scene;
        public Dictionary<string, GameObject> Objects;

        public GameObjectManager(Scene scene) {
            Objects = new Dictionary<string, GameObject>();
            _scene = scene;
        }

        public void Update(Microsoft.Xna.Framework.GameTime gt) {
            // Iterate through each component and Update it.
            for (var i = 0; i < Objects.Count; i++) {
                if (Objects.Count >= i) {
                    Objects.ElementAt(i).Value.Update(gt);
                }
            }
        }

        public event OnGameObjectAdded OnGameObjectAddedEvent;

        public event OnGameObjectRemoved OnGameObjectRemovedEvent;

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb) {
            // Iterate through each component and Draw it.
            for (var i = 0; i < Objects.Count; i++) {
                if (Objects.Count >= i) {
                    Objects.ElementAt(i).Value.Draw(sb);
                }
            }
        }

        #region Getting and adding objects;

        public void AddGameObject(string name, GameObject gameObject) {
            gameObject.Id = Objects.Count;
            var name2 = "";
            if (Objects.ContainsKey(name)) {
                name2 = name + gameObject.Id;
                Objects.Add(name2, gameObject);
                if (OnGameObjectAddedEvent != null) {
                    OnGameObjectAddedEvent(this, new OnGameObjectAddedEventArgs(gameObject));
                }
            }
            else {
                name2 = name;
                Objects.Add(name2, gameObject);
                if (OnGameObjectAddedEvent != null) {
                    OnGameObjectAddedEvent(this, new OnGameObjectAddedEventArgs(gameObject));
                }
            }
            gameObject.Name = name2;
        }

        public IEnumerable<GameObject> ContainsTag(string tag) {
            return HasTag(tag);
        }

        public GameObject GetGameObject(string name) {
            return Objects[name];
        }

        public IEnumerable<GameObject> HasTag(string tag) {
            return
                from entry in Objects
                where entry.Value.Tags.Contains(tag)
                select entry.Value;
        }

        public IEnumerable<GameObject> NameEndsWith(string i) {
            return
                from entry in Objects
                where entry.Key.EndsWith(i)
                select entry.Value;
        }

        public IEnumerable<GameObject> NameStartsWith(string i) {
            return
                from entry in Objects
                where entry.Key.StartsWith(i)
                select entry.Value;
        }

        public void RemoveContainsTag(string tag) {
            RemoveHasTag(tag);
        }

        public void RemoveHasTag(string tag) {
            var i = HasTag(tag);
            foreach (var go in i) {
                RemoveObject(go.Name);
            }
        }

        public void RemoveNameEndsWith(string i) {
            var co = NameEndsWith(i);
            foreach (var l in co) {
                RemoveObject(l.Name);
            }
        }

        public void RemoveNameStartsWith(string i) {
            var co = NameStartsWith(i);
            foreach (var l in co) {
                RemoveObject(l.Name);
            }
        }

        public void RemoveObject(string name) {
            if (OnGameObjectRemovedEvent != null) {
                OnGameObjectRemovedEvent(this, new OnGameObjectRemovedEventArgs(Objects[name]));
            }
            Objects.Remove(name);
        }

        #endregion Getting and adding objects;
    }
}