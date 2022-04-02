using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class Lambertian : Material
    {

        public Lambertian(Color4 albedo)
        {
            Albedo = new SolidColor(albedo);
        }
        public Lambertian(ITexture albedo)
        {
            Albedo = albedo;
        }

        public ITexture Albedo { get; set; }
        public override bool Scatter(Ray ray, HitRecord record, out Color4 attenuation, out Ray scattered)
        {
            var scatterDirection = record.normal + Helper.RandomInUnitSphere().Normalized();

            if (scatterDirection.X < 1e-4 && scatterDirection.Y < 1e-4 && scatterDirection.Z < 1e-4)
            {
                scatterDirection = record.normal;
            }

            scattered = new Ray(record.p, scatterDirection, ray.Time);
            attenuation = Albedo.Value(record.u, record.v, record.p);

            return true;
        }
    }
}