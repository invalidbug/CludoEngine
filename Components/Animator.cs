#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#endregion

namespace CludoEngine.Components {
    #region Sprite

    public delegate void AnimationBegan(object sender, AnimationBeganEventArgs args);

    public delegate void AnimationEnded(object sender, AnimationEndedEventArgs args);

    public delegate void AnimationIterated(object sender, AnimationIteratedArgs args);

    public delegate void AnimationLooped(object sender, AnimationLoopedEventArgs args);

    public class AnimationBeganEventArgs {
    }

    public class AnimationEndedEventArgs {
    }

    public class AnimationIteratedArgs {

        public AnimationIteratedArgs(int row, int column) {
            Row = row;
            Column = column;
        }

        /// <summary>
        /// Current animation's column
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Current animation's Row
        /// </summary>
        public int Row { get; set; }
    }

    public class AnimationLoopedEventArgs {
    }

    #endregion Sprite

    /// <summary>
    /// Class made to provide Animation to the GameObject.
    /// </summary>
    public class Animator : IComponent {
        #region Events

        public event AnimationBegan AnimationBeganEvent;

        public event AnimationEnded AnimationEndedEvent;

        public event AnimationIterated AnimationIteratedEvent;

        public event AnimationLooped AnimationLoopedEvent;

        #endregion Events

        #region Privates

        // Current X Frame
        public int CurrentXFrame;

        // Current Y Frame
        public int CurrentYFrame;

        // The X frame in which the animation checks if its at the end.
        public int EndXframe = 20;

        // The X frame in which the animation checks if its at the end.
        public int EndYframe = 20;

        // The X frame that'll be set to once the animation finishes and loop is true
        public int StartXframe;

        // The X frame that'll be set to once the animation finishes and loop is true
        public int StartYframe;

        // If Y increntments are valid.
        public bool YIncentments;

        #endregion Privates

        #region Properties

        /// <summary>
        /// Height of each frame, also known as a cell.
        /// </summary>
        public int CellHeight {
            get { return (int)CellSize.Y; }
        }

        /// <summary>
        /// Size for each Frame
        /// </summary>
        public Vector2 CellSize { get; set; }

        /// <summary>
        /// Width of each frame, also known as a cell.
        /// </summary>
        public int CellWidth {
            get { return (int)CellSize.X; }
        }

        /// <summary>
        /// Returns how many Columns there is.
        /// </summary>
        public int Columns {
            get { return EndXframe; }
        }

        /// <summary>
        /// If the animation loops.
        /// </summary>
        public bool DoesLoop { get; set; }

        public int Id { get; set; }

        /// <summary>
        /// Currently Animation
        /// </summary>
        public bool IsPlaying { get; set; }

        public Vector2 LocalPosition { get; set; }
        public float LocalRotation { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Returns how many Rows the Texture has.
        /// </summary>
        public int Rows {
            get { return EndYframe; }
        }

        /// <summary>
        /// Texture size.
        /// </summary>
        public Vector2 SpriteSheetSize { get; set; }

        /// <summary>
        /// How long each frame lasts.
        /// </summary>
        public float TimePerFrame { get; set; }

        public string Type { get; set; }

        // How many Cells there are.
        private Vector2 Cells {
            get { return SpriteSheetSize / CellSize; }
        }

        // How much time there is
        private float TimeLeft { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Creates a new sprite
        /// </summary>
        /// <param name="spriteSize">The Size of the Texture</param>
        /// <param name="cellSize">The Size for each Animation frame/cell</param>
        /// <param name="timePerFrame">The time for each frame.</param>
        public Animator(Vector2 spriteSize, Vector2 cellSize, float timePerFrame) {
            CellSize = cellSize;
            SpriteSheetSize = spriteSize;
            IsPlaying = false;
            TimePerFrame = timePerFrame;
            TimeLeft = 0f;
            DoesLoop = false;
            Type = "Animator";
        }

        /// <summary>
        /// Creates a new sprite
        /// </summary>
        /// <param name="spriteWidth">The Width of the Texture</param>
        /// <param name="spriteHeight">The Height of the Texture</param>
        /// <param name="cellWidth">The Width for each Animation frame/cell</param>
        /// <param name="cellHeight">The Height for each Animation frame/cell</param>
        /// <param name="timePerFrame">The time for each frame.</param>
        public Animator(int spriteWidth, int spriteHeight, int cellWidth, int cellHeight, float timePerFrame) {
            CellSize = new Vector2(cellWidth, cellHeight);
            SpriteSheetSize = new Vector2(spriteWidth, spriteHeight);
            IsPlaying = false;
            TimePerFrame = timePerFrame;
            TimeLeft = 0f;
            DoesLoop = false;
            Type = "Animator";
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Creates a new sprite
        /// </summary>
        /// <param name="spriteWidth">The Width of the Texture</param>
        /// <param name="spriteHeight">The Height of the Texture</param>
        /// <param name="cellWidth">The Width for each Animation frame/cell</param>
        /// <param name="cellHeight">The Height for each Animation frame/cell</param>
        /// <param name="timePerFrame">The time for each frame.</param>
        public void Configure(int spriteWidth, int spriteHeight, int cellWidth, int cellHeight, float timePerFrame) {
            CellSize = new Vector2(cellWidth, cellHeight);
            SpriteSheetSize = new Vector2(spriteWidth, spriteHeight);
            IsPlaying = false;
            TimePerFrame = timePerFrame;
            TimeLeft = 0f;
            DoesLoop = false;
            Type = "Animator";
        }

        /// <summary>
        /// Plays an animation on the spritesheet.
        /// </summary>
        /// <param name="startRow">The Row on where the Animation should be played from. Starts at 0.</param>
        /// <param name="loops">If the Animation replays</param>
        public void Play(int startRow, bool loops) {
            CurrentYFrame = startRow;
            CurrentXFrame = 0;
            EndXframe = (int)SpriteSheetSize.X / CellWidth - 1;
            IsPlaying = true;
            DoesLoop = loops;
            YIncentments = false;
            if (AnimationBeganEvent != null) {
                AnimationBeganEvent(this, new AnimationBeganEventArgs());
            }
        }

        /// <summary>
        /// Plays an animation on the spritesheet, that can Ignore some columns.
        /// </summary>
        /// <param name="startRow">The Row on where the Animation should be played from. Starts at 0.</param>
        /// <param name="startColumn">The Column where the Animation should start.</param>
        /// <param name="endColumn">The Column where the Animation should stop at.</param>
        /// <param name="loops">If the Animation replays</param>
        public void Play(int startRow, int startColumn, int endColumn, bool loops) {
            CurrentYFrame = startRow;
            CurrentXFrame = startColumn;
            EndXframe = endColumn;
            StartXframe = startColumn;
            IsPlaying = true;
            DoesLoop = loops;
            YIncentments = false;
            if (AnimationBeganEvent != null) {
                AnimationBeganEvent(this, new AnimationBeganEventArgs());
            }
        }

        /// <summary>
        /// Plays an animation on the spritesheet, that iterates through rows and ignores specified Columns.
        /// </summary>
        /// <param name="startRow">The Row where the Animation should start. Starts at 0.</param>
        /// <param name="endRow">The Row where the Animation should end.</param>
        /// <param name="startColumn">The Column where the Animation should start.</param>
        /// <param name="endColumn">The Column where the Animation should stop at.</param>
        /// <param name="loops">If the Animation replays</param>
        public void Play(int startRow, int endRow, int startColumn, int endColumn, bool loops) {
            CurrentXFrame = startColumn;
            CurrentYFrame = startRow;
            EndXframe = endColumn;
            EndYframe = endRow;
            StartXframe = startColumn;
            StartYframe = startRow;
            YIncentments = true;
            IsPlaying = true;
            DoesLoop = loops;
            if (AnimationBeganEvent != null) {
                AnimationBeganEvent(this, new AnimationBeganEventArgs());
            }
        }

        /// <summary>
        /// Plays an animation on the spritesheet, that iterates through rows too instead of only Columns.
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="endRow"></param>
        /// <param name="loops"></param>
        public void Play(int startRow, int endRow, bool loops) {
            CurrentXFrame = 0;
            CurrentYFrame = startRow;
            StartXframe = 0;
            StartYframe = startRow;
            EndYframe = endRow;
            EndXframe = (int)SpriteSheetSize.X / CellWidth - 1;
            YIncentments = true;
            IsPlaying = true;
            DoesLoop = loops;
            if (AnimationBeganEvent != null) {
                AnimationBeganEvent(this, new AnimationBeganEventArgs());
            }
        }

        /// <summary>
        /// Sets the frame and stops Animation.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public void SetFrame(int row, int column) {
            CurrentXFrame = column;
            CurrentYFrame = row;
            IsPlaying = false;
            IteratedEvent();
        }

        public IComponent Clone(object[] args) {
            return new Animator((int)SpriteSheetSize.X, (int)SpriteSheetSize.Y, CellWidth, CellHeight, TimePerFrame);
        }

        /// <summary>
        /// Updates. This should be handled by the GameObject, not the user.
        /// </summary>
        /// <param name="gt"></param>
        public void Update(GameTime gt) {
            if (IsPlaying) {
                var e = (Single)gt.ElapsedGameTime.TotalSeconds;
                TimeLeft += e;
                if (TimeLeft >= TimePerFrame) {
                    TimeLeft = 0f;
                    if (YIncentments) {
                        if (CurrentXFrame == EndXframe) {
                            if (CurrentYFrame == EndYframe) {
                                if (DoesLoop) {
                                    CurrentXFrame = StartXframe;
                                    CurrentYFrame = StartYframe;
                                    IteratedEvent();
                                    if (AnimationLoopedEvent != null) {
                                        AnimationLoopedEvent(this, new AnimationLoopedEventArgs());
                                    }
                                } else {
                                    IsPlaying = false;
                                    if (AnimationEndedEvent != null) {
                                        AnimationEndedEvent(this, new AnimationEndedEventArgs());
                                    }
                                }
                            } else {
                                CurrentXFrame = StartXframe;
                                CurrentYFrame++;
                                IteratedEvent();
                            }
                        } else {
                            CurrentXFrame++;
                            IteratedEvent();
                        }
                    } else {
                        if (CurrentXFrame == EndXframe) {
                            if (DoesLoop) {
                                CurrentXFrame = StartXframe;
                                IteratedEvent();
                                if (AnimationLoopedEvent != null) {
                                    AnimationLoopedEvent(this, new AnimationLoopedEventArgs());
                                }
                            } else {
                                IsPlaying = false;
                                if (AnimationEndedEvent != null) {
                                    AnimationEndedEvent(this, new AnimationEndedEventArgs());
                                }
                            }
                        } else {
                            CurrentXFrame++;
                            IteratedEvent();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Easy event calling to reduce copied code.
        /// </summary>
        private void IteratedEvent() {
            if (AnimationIteratedEvent != null) {
                AnimationIteratedEvent(this, new AnimationIteratedArgs(CurrentYFrame, CurrentXFrame));
            }
        }

        #endregion Methods

        #region Functions

        public void Draw(SpriteBatch sb) {
        }

        /// <summary>
        /// Gets the viewing rectangle for the spritesheet
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRectangle() {
            return new Rectangle(CellWidth * CurrentXFrame, CellHeight * CurrentYFrame, CellWidth, CellHeight);
        }

        #endregion Functions
    }
}