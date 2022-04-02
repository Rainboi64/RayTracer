using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class PerlinNoise
    {
        private const int PointCount = 256;

        private Vector3[] _randomVectors;
        private int[] _permutationsX;
        private int[] _permutationsY;
        private int[] _permutationsZ;

        public PerlinNoise()
        {
            _randomVectors = new Vector3[PointCount];
            for (int i = 0; i < PointCount; ++i)
            {
                _randomVectors[i] = Helper.RandomVec3().Normalized();
            }

            _permutationsX = GeneratePerlinPermutations();
            _permutationsY = GeneratePerlinPermutations();
            _permutationsZ = GeneratePerlinPermutations();
        }

        public float Point(Vector3 p)
        {
            var u = p.X - MathF.Floor(p.X);
            var v = p.Y - MathF.Floor(p.Y);
            var w = p.Z - MathF.Floor(p.Z);

            var i = (int)(MathF.Floor(p.X));
            var j = (int)(MathF.Floor(p.Y));
            var k = (int)(MathF.Floor(p.Z));
            Vector3[,,] c = new Vector3[2, 2, 2];

            for (int di = 0; di < 2; di++)
                for (int dj = 0; dj < 2; dj++)
                    for (int dk = 0; dk < 2; dk++)
                        c[di, dj, dk] = _randomVectors[
                            _permutationsX[(i + di) & 255] ^
                            _permutationsY[(j + dj) & 255] ^
                            _permutationsZ[(k + dk) & 255]
                        ];

            return TriliniearInterp(c, u, v, w);
        }

        public float Turbulance(Vector3 p, int depth = 7)
        {
            var accum = 0.0f;
            var temp_p = p;
            var weight = 1.0f;
            for (int i = 0; i < depth; i++)
            {
                accum += weight * Point(temp_p);
                weight *= 0.5f;
                temp_p *= 2;
            }

            return MathF.Abs(accum);
        }

        private static int[] GeneratePerlinPermutations()
        {
            var p = new int[PointCount];

            for (int i = 0; i < PointCount; i++)
                p[i] = i;

            Permutate(p, PointCount);

            return p;
        }

        private static void Permutate(int[] p, int n)
        {
            for (int i = n - 1; i > 0; i--)
            {
                int target = Helper.Random.Next(0, i);
                int tmp = p[i];
                p[i] = p[target];
                p[target] = tmp;
            }
        }

        private static float TriliniearInterp(Vector3[,,] c, float u, float v, float w)
        {
            var uu = u * u * (3 - 2 * u);
            var vv = v * v * (3 - 2 * v);
            var ww = w * w * (3 - 2 * w);
            var accum = 0.0f;

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                    {
                        var weight_v = new Vector3(u - i, v - j, w - k);
                        accum += (i * uu + (1 - i) * (1 - uu))
                            * (j * vv + (1 - j) * (1 - vv))
                            * (k * ww + (1 - k) * (1 - ww))
                            * Vector3.Dot(c[i, j, k], weight_v);
                    }

            return accum;
        }
    }
}