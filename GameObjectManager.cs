#region

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace CludoEngine {

    public class GameObjectManager : IUpdateable {

        public delegate void OnGameObjectAdded(object sender, OnGameObjectAddedEventArgs args);

        public delegate void OnGameObjectRemoved(object sender, OnGameObjectRemovedEventArgs args);

        private Scene _scene;
        public Dictionary<int, GameObject> Objects;

        public GameObjectManager(Scene scene) {
            Objects = new Dictionary<int, GameObject>();
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
            gameObject.Name = name;
            Objects.Add(gameObject.Id,gameObject);
                if (OnGameObjectAddedEvent != null) {
                    OnGameObjectAddedEvent(this, new OnGameObjectAddedEventArgs(gameObject));
                }
            // old system.
            //gameObject.Id = Objects.Count;
            //var name2 = "";
            //if (Objects.ContainsKey(name)) {
            //    name2 = name + gameObject.Id;
            //    Objects.Add(name2, gameObject);
            //    if (OnGameObjectAddedEvent != null) {
            //        OnGameObjectAddedEvent(this, new OnGameObjectAddedEventArgs(gameObject));
            //    }
            //} else {
            //    name2 = name;
            //    Objects.Add(name2, gameObject);
            //    if (OnGameObjectAddedEvent != null) {
            //        OnGameObjectAddedEvent(this, new OnGameObjectAddedEventArgs(gameObject));
            //    }
            //}
            //gameObject.Name = name2;
        }

        public IEnumerable<GameObject> ContainsTag(string tag) {
            return HasTag(tag);
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
                where ((GameObject)entry.Value).Name.EndsWith(i) 
                select entry.Value;
        }

        public IEnumerable<GameObject> NameStartsWith(string i) {
            return
                from entry in Objects
                where ((GameObject)entry.Value).Name.StartsWith(i)
                select entry.Value;
        }

        public bool Exists(string name) {
            return GetGameObject(name) != null;
        }

        public GameObject GetGameObject(string name) {
            foreach (GameObject i in Objects.Values) {
                if (i.Name == name) {
                    return i;
                }
            }
            return null;
        }

        public void RemoveContainsTag(string tag) {
            RemoveHasTag(tag);
        }

        public void RemoveHasTag(string tag) {
            var i = HasTag(tag);
            foreach (var go in i) {
                RemoveObject(go.Id);
            }
        }

        public void RemoveNameEndsWith(string i) {
            var co = NameEndsWith(i);
            foreach (var l in co) {
                RemoveObject(l.Id);
            }
        }

        public void RemoveNameStartsWith(string i) {
            var co = NameStartsWith(i);
            foreach (var l in co) {
                RemoveObject(l.Id);
            }
        }

        public void RemoveObject(int i) {
            if (OnGameObjectRemovedEvent != null) {
                OnGameObjectRemovedEvent(this, new OnGameObjectRemovedEventArgs(Objects[i]));
            }
            _scene.World.RemoveBody(Objects[i].Body);
            Objects.Remove(i);
        }

        #endregion Getting and adding objects;
    }
}