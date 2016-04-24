#region

using System;
using System.Collections.Generic;
using System.Linq;
using CludoEngine.Graphics;
using CludoEngine.Pipeline;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

//using CludoEngine.GUI;

#endregion

namespace CludoEngine {
    public static class Extensions {
        public static Body ConvertToBody(this Texture2D me, Scene scene) {
            var polygonTexture = me;

            var data = new uint[polygonTexture.Width*polygonTexture.Height];
            polygonTexture.GetData(data);

            var verts = PolygonTools.CreatePolygon(data, polygonTexture.Width, true);

            //These 2 seem to work the best with tile maps
            verts = SimplifyTools.CollinearSimplify(verts);

            verts = SimplifyTools.DouglasPeuckerSimplify(verts, 0f);

            var list = BayazitDecomposer.ConvexPartition(verts);

            var vertScale = new Vector2(1f/ConvertUnits._displayUnitsToSimUnitsRatio);
            foreach (var vertices in list) {
                vertices.Scale(ref vertScale);
            }

            var body = BodyFactory.CreateCompoundPolygon(scene.World, list, 1);
            Debugging.Debug.WriteLine(
                "WARNING: In Texture2D.ConvertToBody, Body functions have not been implemented!");
            body.Friction = 0f;
            body.IsStatic = true;
            return body;
        }
    }

    public class Scene : IEngineFeature, IDisposable {
        // The Prefabs
        public static Dictionary<string, Type> TiledPrefabs;

        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        public ContentManager Content;

        public DebugViewXNA DebugView;

        public GameWindow GameWindow;

        public Texture2D Line;

        public List<TiledPrefab> LoadedTiledPrefabs;

        public Dictionary<string,CludoRenderTarget> RenderTargets;

        public Texture2D Vector;
        public int Ver;
        internal VirtualResolutionScaler VirtualResolutionScaler;

        public Scene(SpriteBatch sb, GraphicsDeviceManager graphicsDeviceManager, GraphicsDevice gd, GameWindow window,
            ContentManager content) {
            VirtualResolutionScaler = new VirtualResolutionScaler(graphicsDeviceManager.PreferredBackBufferWidth,
                graphicsDeviceManager.PreferredBackBufferHeight, gd);
            GraphicsDevice = gd;
            SpriteBatch = sb;
            _graphicsDeviceManager = graphicsDeviceManager;
            Content = content;
            GameWindow = window;
            GameWindow.ClientSizeChanged +=
                (sender, args) => {
                    VirtualResolutionScaler = new VirtualResolutionScaler(
                        graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight,
                        gd);
                };
            RenderTargets  = new Dictionary<string, CludoRenderTarget>();
            RenderTargets.Add("Game", new CludoRenderTarget(this));
            RenderTargets.Add("DontTransform", new CludoRenderTarget(this));
            RenderTargets.Add("Lights", new CludoRenderTarget(this));
            RenderTargets["DontTransform"].Layer = 0.55f;
            RenderTargets["Game"].Layer = 0.5f;
            RenderTargets["Lights"].Layer = 0.6f;
            RenderTargets["DontTransform"].Transform = false;
            var sortedDict = from entry in RenderTargets orderby entry.Value.Layer ascending select entry;
            RenderTargets = sortedDict.ToDictionary(pair => pair.Key, pair => pair.Value);
            Camera = new Camera(this, gd.Viewport);
            Input.Instance = new Input(this);
            GameObjects = new GameObjectManager(this);
            TiledPrefabs = new Dictionary<string, Type>();
            World = new World(Vector2.Zero);
            Settings.AllowSleep = true;
            Settings.VelocityIterations = 12;
            Settings.ContinuousPhysics = true;
            DebugView = new DebugViewXNA(World);
            DebugView.RemoveFlags(DebugViewFlags.Shape);
            DebugView.RemoveFlags(DebugViewFlags.Joint);
            DebugView.DefaultShapeColor = Color.LightPink;
            DebugView.SleepingShapeColor = Color.MediumPurple;
            DebugView.LoadContent(GraphicsDevice, content);
            DebugView.AppendFlags(DebugViewFlags.DebugPanel);
            DebugView.AppendFlags(DebugViewFlags.PerformanceGraph);
            Debug = true;
            LoadedTiledPrefabs = new List<TiledPrefab>();
            Pipeline = new CludoContentPipeline(gd);
            Vector = Pipeline.LoadContent<Texture2D>("Vector", content, true);
            Line = new Texture2D(GraphicsDevice, 1, 1);
            Line.SetData(new[] {Color.White});
            StartScene();
        }

        // Graphics device
        public static GraphicsDevice GraphicsDevice { get; set; }

        // Spritebatch
        public static SpriteBatch SpriteBatch { get; set; }

        public Camera Camera { get; set; }

        // The Color that the screen will always be cleared with.
        public Color ClearColor { get; set; }

        public bool Debug { get; set; }

        // The gameobjectmanager
        public GameObjectManager GameObjects { get; set; }

        public Vector2 Gravity {
            get { return World.Gravity; }
            set { World.Gravity = value; }
        }

        // The GUI manager
        //public CludoEngine.GUI.GUI gui { get; set; }

        // Pipeline
        public Pipeline.CludoContentPipeline Pipeline { get; set; }

        // The farseerphysics world
        public World World { get; set; }

        public void Dispose() {
            DebugView.Dispose();
            if (Vector != null) {
                Vector.Dispose();
            }
            foreach (var i in RenderTargets) {
                i.Value.Target.Dispose();
            }
            RenderTargets = null;
            foreach (var b in World.BodyList) {
                World.RemoveBody(b);
            }
            GC.SuppressFinalize(this);
        }

        public void ReorderDictionaries() {
            var sortedDict = from entry in RenderTargets orderby entry.Value.Layer ascending select entry;
            RenderTargets = sortedDict.ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        
        public virtual void Update(GameTime gt) {
            Input.Instance.Update(gt);
            World.Step((float) gt.ElapsedGameTime.TotalMilliseconds*0.001f);
            GameObjects.Update(gt);
            foreach (var prefab in LoadedTiledPrefabs) {
                prefab.Update(gt);
            }
            Camera.Update(gt);
        }

        public void AddTiledPrefab(string name, TiledPrefab a) {
            AddTiledPrefab(name, a.GetType());
        }

        public void AddTiledPrefab(string name, Type a) {
            TiledPrefabs.Add(name, a);
        }

        public void CreateTiledPrefab(string name, TmxObject tmxobject) {
            try {
                LoadedTiledPrefabs.Add((TiledPrefab) Activator.CreateInstance(TiledPrefabs[name], tmxobject, this));
            }
            catch (System.Reflection.TargetInvocationException e) {
                throw e.InnerException;
            }
        }

        public virtual void Draw(SpriteBatch sb) {
            foreach (var pair in RenderTargets) {
                pair.Value.Draw(sb);
            }
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(ClearColor);
            foreach (var pair in RenderTargets) {
                sb.Begin(SpriteSortMode.FrontToBack, pair.Value.BlendState, SamplerState.PointClamp);
                sb.Draw(pair.Value.Target,
                    new Rectangle(0, 0, pair.Value.Target.Width, pair.Value.Target.Height), null,
                    Color.White, 0f, Vector2.Zero, SpriteEffects.None, pair.Value.Layer);
                sb.End();
            }
            foreach (var prefab in LoadedTiledPrefabs) {
                prefab.Draw(sb);
            }
            if (Debug) {
                DrawDebug(sb);
            }
        }

        public void DrawDebug(SpriteBatch sb) {
            var view = Camera.GetFarseerViewMatrix();
            var projection = Matrix.CreateOrthographicOffCenter(0,
                ConvertUnits.ToSimUnits(GraphicsDevice.Viewport.Width),
                ConvertUnits.ToSimUnits(GraphicsDevice.Viewport.Height),
                0, 0, 1);
            DebugView.BeginCustomDraw(ref projection, ref view);
            foreach (var b in World.BodyList) {
                Transform f;
                b.GetTransform(out f);
                for (var i = 0; i <= b.FixtureList.Count() - 1; i++) {
                    var color = Color.Blue;
                    if (b.Awake) {
                        color = Color.Red;
                    }
                    if (b.FixtureList[i].Shape == null) {
                        continue;
                    }
                    if (b.UserData != null) {
                        if (((GameObject) b.UserData).IgnoreDebug) {
                            continue;
                        }
                    }
                    DebugView.DrawShape(b.FixtureList[i], f, color);
                }
            }
            DebugView.EndCustomDraw();
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null,
                Camera.GetViewMatrix());
            foreach (var obj in GameObjects.Objects) {
                sb.Draw(Vector, new Rectangle((int) obj.Value.Position.X - 32, (int) obj.Value.Position.Y - 32, 64, 64),
                    Color.White);
                var obj1 = obj;
                foreach (var pos in from i in obj.Value.Body.FixtureList
                    where i.UserData != null
                    where !((GameObject) i.UserData).IgnoreDebug
                    select Utils.PositionOfFixture(obj1.Value.Body, i)) {
                    sb.Draw(Vector, new Rectangle((int) pos.X - 16, (int) pos.Y - 16, 32, 32), Color.White);
                }
            }
            if (Raycast.Casts != null) {
                foreach (var cast in Raycast.Casts) {
                    if (cast.HitPoint != Vector2.Zero) {
                        DrawLine(SpriteBatch, cast.StartPoint, cast.HitPoint, Color.Red);
                        DrawLine(SpriteBatch, cast.HitPoint, cast.EndPoint, Color.Blue);
                    }
                    else {
                        DrawLine(SpriteBatch, cast.StartPoint, cast.EndPoint, Color.Blue);
                    }
                }
                Raycast.Casts.Clear();
            }
            sb.End();
        }

        private void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color drawColor) {
            var edge = end - start;
            var angle =
                (float) Math.Atan2(edge.Y, edge.X);

            sb.Draw(Line,
                new Rectangle(
                    (int) start.X,
                    (int) start.Y,
                    (int) edge.Length(),
                    1),
                null,
                drawColor,
                angle,
                new Vector2(0, 0),
                SpriteEffects.None,
                0);
        }


        public virtual void StartScene() {
        }
    }
}