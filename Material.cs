using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class Material
    {
        public virtual Color4 Emitted(float u, float v, Vector3 p) => Color4.Black;
        public virtual bool Scatter(Ray ray, HitRecord record, out Color4 attenuation, out Ray scattered) => throw new System.NotImplementedException();
    }
}