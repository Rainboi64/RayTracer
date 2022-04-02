using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class CheckerTexture : ITexture
    {
        public CheckerTexture(ITexture even, Color4 odd)
        {
            Even = even;
            Odd = new SolidColor(odd);
        }
        public CheckerTexture(Color4 even, Color4 odd)
        {
            Even = new SolidColor(even);
            Odd = new SolidColor(odd);
        }

        public ITexture Even { get; }
        public ITexture Odd { get; }

        public Color4 Value(float u, float v, Vector3 p)
        {
            var sines = MathF.Sin(10f * p.X) * MathF.Sin(10 * p.Y) * MathF.Sin(10 * p.Z);
            if(sines < 0f)
            {
                return Odd.Value(u, v, p);
            }
            else
            {
                return Even.Value(u, v, p);
            }
        }
    }
}