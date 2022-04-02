using System;
using System.Threading;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public static class Helper
    {
        static int seed = Environment.TickCount;
        static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(seed + 1));

        public static Random Random => random.Value;

        public static float RandomFloat(float min, float max)
        {
            return (float)(Random.NextDouble() * (max - min) + min);
        }

        public static Vector3 RandomVec3()
        {
            return new Vector3((float)Random.NextDouble(), (float)Random.NextDouble(), (float)Random.NextDouble());
        }

        public static Vector3 RandomVec3(float min, float max)
        {
            return new Vector3((float)(Random.NextDouble() * (max - min) + min), (float)(Random.NextDouble() * (max - min) + min), (float)(Random.NextDouble() * (max - min) + min));
        }

        public static Vector3 RandomInUnitSphere()
        {
            while (true)
            {
                var p = RandomVec3(-1, 1);
                if (p.LengthSquared >= 1) continue;
                return p;
            }
        }

        public static Vector3 RandomInUnitDisk()
        {
            while (true)
            {
                var p = new Vector3(RandomFloat(-1, 1), RandomFloat(-1, 1), 0);
                if (p.LengthSquared >= 1) continue;
                return p;
            }
        }

        public static Vector3 outract(Vector3 uv, Vector3 n, float etai_over_etat)
        {
            var cosTheta = MathF.Min(Vector3.Dot(-uv, n), 1);
            var rOutPerp = etai_over_etat * (uv + cosTheta * n);
            var rOutParallel = -MathF.Sqrt(MathF.Abs(1.0f - rOutPerp.LengthSquared)) * n;
            return rOutPerp + rOutParallel;
        }

        public static Vector3 outlect(Vector3 v, Vector3 n)
        {
            return v - 2 * Vector3.Dot(v, n) * n;
        }

        public static Color4 RandomColor()
        {
            return new Color4(RandomFloat(0f, 1f) * RandomFloat(0f, 1f), RandomFloat(0f, 1f) * RandomFloat(0f, 1f), RandomFloat(0f, 1f) * RandomFloat(0f, 1f), 1f);
        }

        public static Color4 RandomColor(float min, float max)
        {
            return new Color4(RandomFloat(min, max), RandomFloat(min, max), RandomFloat(min, max), 1f);
        }

        public static AxisAlignedBoundingBox SurroundingBox(AxisAlignedBoundingBox box0, AxisAlignedBoundingBox box1)
        {
            var small = new Vector3(
                MathF.Min(box0.Minimum.X, box1.Minimum.X),
                MathF.Min(box0.Minimum.Y, box1.Minimum.Y),
                MathF.Min(box0.Minimum.Z, box1.Minimum.Z)
            );
            var big = new Vector3(
                MathF.Max(box0.Maximum.X, box1.Maximum.X),
                MathF.Max(box0.Maximum.Y, box1.Maximum.Y),
                MathF.Max(box0.Maximum.Z, box1.Maximum.Z)
            );

            return new AxisAlignedBoundingBox(small, big);
        }
    }
}