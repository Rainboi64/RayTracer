using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class Emmisive : Material
    {
        public Emmisive(ITexture texture, float emmisiveness)
        {
            Emit = texture;
        }

        public Emmisive(Color4 color, float emmisiveness) : this(new SolidColor(color), emmisiveness) { }
        public Emmisive(Color4 color) : this(new SolidColor(color), 1f) { }

        public float Emmisiveness { get; set; } = 1f;
        public ITexture Emit { get; set; }

        public override bool Scatter(Ray ray, HitRecord record, out Color4 attenuation, out Ray scattered)
        {
            attenuation = Color4.Black;
            scattered = new Ray();

            return false;
        }

        public override Color4 Emitted(float u, float v, Vector3 p)
        {
            var value = Emit.Value(u, v, p);
            
            return new Color4(value.R * Emmisiveness, value.G *Emmisiveness, value.B * Emmisiveness, Emmisiveness);
        }
    }
}