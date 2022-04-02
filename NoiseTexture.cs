using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class NoiseTexture : ITexture
    {
        private float _scale = 1f;
        private PerlinNoise _noise = new PerlinNoise();

        public NoiseTexture() { }
        public NoiseTexture(float scale)
        {
            _scale = scale;
        }

        public Color4 Value(float u, float v, Vector3 p)
        {
            var value = 0.5f * (1f + MathF.Sin(_scale * p.Z + 10f * _noise.Turbulance(p)));
            //var value = 0.5f * (1.0f + noise.Point(_scale * p));
            return new Color4(value, value, value, 1f);
        }
    }
}