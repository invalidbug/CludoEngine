#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.GUI {
    public enum ControlState {
        NotSelected = 0,
        Hover = 1,
        Selected = 2
    }

    public class Gui {
        private IControl _selected;
        public Dictionary<string, IControl> Controls;

        public Gui() {
            Controls = new Dictionary<string, IControl>();
        }

        public void AddControl(string name, IControl control) {
            Controls.Add(name, control);
        }

        public void Update(GameTime gt) {
            _selected = UpdateControls(gt, Controls, _selected);
        }

        public void Draw(SpriteBatch sb) {
            DrawControls(sb, Controls, _selected);
        }

        public static void DrawControls(SpriteBatch sb, Dictionary<string, IControl> controls, IControl selected) {
            foreach (var control in controls.Values) {
                if (control != selected) {
                    control.Draw(sb);
                }
            }
            if (selected != null) {
                selected.Draw(sb);
            }
        }

        public static IControl UpdateControls(GameTime gt, Dictionary<string, IControl> controls, IControl selected) {
            if (selected != null) {
                selected = CheckForNewSelectedControl(controls, selected);
            }
            else {
                selected = CheckForNewSelectedControl(controls, selected);
            }
            if (selected != null) {
                selected.Update(gt);
            }
            return selected;
        }

        private static IControl CheckForNewSelectedControl(Dictionary<string, IControl> controls, IControl selected) {
            if (Input.IsLeftMouseButtonDown() && Input.WasLeftMouseButtonUp()) {
                foreach (var i in controls.Values) {
                    if (i.TestMouse()) {
                        if (selected != null) {
                            selected.State = ControlState.NotSelected;
                        }
                        selected = i;
                        break;
                    }
                }
            }
            else {
                foreach (var i in controls.Values) {
                    var foundHover = false;
                    if (i != selected) {
                        if (i.TestMouse()) {
                            if (foundHover == false) {
                                i.State = ControlState.Hover;
                            }
                            foundHover = true;
                        }
                        else {
                            i.State = ControlState.NotSelected;
                        }
                    }
                }
            }
            return selected;
        }
    }
}