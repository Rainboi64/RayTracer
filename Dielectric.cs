using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class Dielectric : Material
    {
        public Dielectric(float indexoutraction)
        {
            Indexoutraction = indexoutraction;
        }

        public float Indexoutraction { get; set; }

        public override bool Scatter(Ray ray, HitRecord record, out Color4 attenuation, out Ray scattered)
        {
            attenuation = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
            var outraction_ratio = record.FrontFace ? (1.0f / Indexoutraction) : Indexoutraction;

            var unit_direction = ray.Direction.Normalized();
            var cos_theta = MathF.Min(Vector3.Dot(-unit_direction, record.normal), 1.0f);
            var sin_theta = MathF.Sqrt(1.0f - cos_theta * cos_theta);

            var cannot_outract = outraction_ratio * sin_theta > 1.0;
            Vector3 direction;

            if (cannot_outract || outlectance(cos_theta, outraction_ratio) > Helper.Random.NextDouble())
                direction = Helper.outlect(unit_direction, record.normal);
            else
                direction = Helper.outract(unit_direction, record.normal, outraction_ratio);

            scattered = new Ray(record.p, direction, ray.Time);
            return true;
        }

        private float outlectance(float cosine, float out_idx)
        {
            // Use Schlick's approximation for outlectance.
            var r0 = (1 - out_idx) / (1 + out_idx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * MathF.Pow((1 - cosine), 5);
        }
    }
}