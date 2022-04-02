using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class Emmisive : Material
    {
        public Emmisive(ITexture texture)
        {
            Emit = texture;
        }

        public Emmisive(Color4 color) : this(new SolidColor(color)) { }
        public ITexture Emit { get; set; }
        public override bool Scatter(Ray ray, HitRecord record, out Color4 attenuation, out Ray scattered)
        {
            attenuation = Color4.Black;
            scattered = new Ray();

            return false;
        }

        public override Color4 Emitted(float u, float v, Vector3 p)
        {
            return Emit.Value(u, v, p);
        }
    }
}