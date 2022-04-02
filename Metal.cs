using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class Metal : Material
    {
        public Metal(Color4 albedo, float fuzz)
        {
            Fuzz = fuzz;
            Albedo = albedo;
        }

        public float Fuzz { get; set; }
        public Color4 Albedo { get; set; }

        public override bool Scatter(Ray ray, HitRecord record, out Color4 attenuation, out Ray scattered)
        {
            Vector3 outlected = Helper.outlect(ray.Direction.Normalized(), record.normal);
            scattered = new Ray(record.p, outlected + Fuzz * Helper.RandomInUnitSphere(), ray.Time);
            attenuation = Albedo;
            return (Vector3.Dot(scattered.Direction, record.normal) > 0);
        }
    }
}