#region

using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

#endregion

namespace CludoEngine
{
    public struct Raycast
    {
        public Vector2 HitPoint;
        public Vector2 StartPoint;
        public Vector2 EndPoint;
        public float Fraction;
        public Vector2 Normal;
        public Body Body;
        public Fixture Fixture;
        public GameObject GameObject;

        public static List<Raycast> Casts;

        public Raycast(Vector2 startPoint, Vector2 hitPoint, Vector2 endPoint, float fraction, Vector2 normal,
            Fixture fixture)
            : this()
        {
            if (Casts == null)
                Casts = new List<Raycast>();
            HitPoint = hitPoint;
            EndPoint = endPoint;
            Fraction = fraction;
            StartPoint = startPoint;
            Normal = normal;
            if (fixture != null)
            {
                Fixture = fixture;
                Body = fixture.Body;
                GameObject = (GameObject) Body.UserData;
            }
            Casts.Add(this);
        }
    }

    public static class Utils
    {
        public static float ConvertPositive(float num)
        {
            if (num >= 0)
                return num;
            return +num;
        }

        public static float PositiveNumOrDefault(float num, float Default)
        {
            if (num >= 0)
                return num;
            return Default;
        }

        public static Vector2 Rotate(Vector2 position, Vector2 origin, float angleInDegrees)
        {
            var angleInRadians = angleInDegrees*(Math.PI/180);
            var cosTheta = Math.Cos(angleInRadians);
            var sinTheta = Math.Sin(angleInRadians);
            return new Vector2
            {
                X =
                    (int)
                        (cosTheta*(position.X - origin.X) -
                         sinTheta*(position.Y - origin.Y) + origin.X),
                Y =
                    (int)
                        (sinTheta*(position.X - origin.X) +
                         cosTheta*(position.Y - origin.Y) + origin.Y)
            };
        }

        public static Vector2 PositionOfFixture(Body obj, Fixture fixture)
        {
            switch (fixture.Shape.ShapeType)
            {
                case ShapeType.Circle:
                    var pos = ((CircleShape) fixture.Shape).Position + obj.Position;
                    return Rotate(ConvertUnits.ToDisplayUnits(pos), ConvertUnits.ToDisplayUnits(obj.Position),
                        obj.Rotation);

                case ShapeType.Polygon:
                    var shape = (PolygonShape) fixture.Shape;
                    var vecs = new List<Vector2>();
                    var total = Vector2.Zero;
                    foreach (var v in shape.Vertices)
                    {
                        vecs.Add(ConvertUnits.ToDisplayUnits(v));
                        total += vecs[vecs.Count - 1];
                    }
                    var position = total/vecs.Count;
                    return ConvertUnits.ToDisplayUnits(obj.Position) +
                           Rotate(position, new Vector2(0, 0), MathHelper.ToDegrees(obj.Rotation));
            }
            throw new ArgumentException("Couldn't grab ShapeType");
        }

        public static Vector2 PositionOfFixture(Body obj, int number)
        {
            return PositionOfFixture(obj, obj.FixtureList[number]);
        }

        public static Vector2 PositionOfFixture(GameObject obj, int number)
        {
            return PositionOfFixture(obj.Body, number);
        }

        public static Raycast CreateRayCast(Scene scene, Vector2 point0, Vector2 point1, GameObject ignore)
        {
            var ignoredFixtures = new List<Fixture>();
            foreach (var fixture in ignore.Body.FixtureList)
            {
                ignoredFixtures.Add(fixture);
            }
            return CreateRayCastFixtureIgnore(scene, point0, point1, ignoredFixtures);
        }

        public static Raycast CreateRayCast(Scene scene, Vector2 point0, Vector2 point1, List<GameObject> ignore = null)
        {
            var ignoredFixtures = new List<Fixture>();
            if (ignore != null)
            {
                foreach (var i in ignore)
                {
                    foreach (var fixture in i.Body.FixtureList)
                    {
                        ignoredFixtures.Add(fixture);
                    }
                }
            }
            else
            {
                return CreateRayCastFixtureIgnore(scene, point0, point1, ignoredFixtures);
            }
            return CreateRayCastFixtureIgnore(scene, point0, point1, ignoredFixtures);
        }

        public static Raycast CreateRayCastFixtureIgnore(Scene scene, Vector2 point0, Vector2 point1,
            List<Fixture> ignoreFixtures)
        {
            Fixture tfixture = null;
            var tpoint = Vector2.Zero;
            var tnormal = Vector2.Zero;
            ;
            var tfraction = 0f;
            Func<Fixture, Vector2, Vector2, float, float> getFirstCallback =
                delegate(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
                {
                    tfixture = fixture;
                    tpoint = point;
                    tnormal = normal;
                    tfraction = fraction;
                    if (ignoreFixtures != null)
                    {
                        if (ignoreFixtures.Contains(fixture))
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                    return 0;
                };
            scene.World.RayCast(getFirstCallback, ConvertUnits.ToSimUnits(point0), ConvertUnits.ToSimUnits(point1));
            return new Raycast(point0, ConvertUnits.ToDisplayUnits(tpoint), point1, tfraction, tnormal, tfixture);
        }

        /// <summary>
        /// Clamps a Rectangle inside another Rectangle.
        /// For example, if one Rectangle (B) is overlapping another Rectangle (A), 
        /// this function will return a non overlapping B copy.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Rectangle ClampRectangle(Rectangle a, Rectangle b)
        {
            var width = a.Width;
            var height = a.Height;
            var x = a.X;
            var y = a.Y;

            if (x + width > b.Width)
            {
                width = b.Width - x;
            }
            if (y + height > b.Height)
            {
                height = b.Height - y;
            }

            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;
            return new Rectangle(x, y, width, height);
        }
    }
}