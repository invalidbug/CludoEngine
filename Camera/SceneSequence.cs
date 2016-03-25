#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Cludo_Engine
{
    public interface ISequenceStep
    {
        bool Done { get; set; }

        void Draw(SpriteBatch gt);

        void Update(GameTime gt);
    }

    public class SceneSequence
    {
        private List<ISequenceStep> _steps;

        public SceneSequence()
        {
            _steps = new List<ISequenceStep>();
        }

        public void AddStep(ISequenceStep step)
        {
            _steps.Add(step);
        }

        public void Draw(SpriteBatch sb)
        {
            for(int i = 0; i < _steps.Count;i++)
            {
                if (_steps[i].Done)
                {
                }
                else
                {
                    _steps[i].Draw(sb);
                    return;
                }
            }
        }

        public void Update(GameTime gt)
        {
            foreach (var i in _steps)
            {
                if (i.Done)
                {
                }
                else
                {
                    i.Update(gt);
                    return;
                }
            }
        }
    }
}