using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class RandomColorTexture : ITexture
    {
        public Color4 Value(float u, float v, Vector3 p)
        {
            return Helper.RandomColor(u, v);
        }
    }
}