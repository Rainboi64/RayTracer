using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class Sphere : IHittable
    {
        public Sphere(Vector3 center, float radius, Material material)
        {
            Material = material;
            Center = center;
            Radius = radius;
        }
        public Material Material { get; set; }
        public Vector3 Center { get; set; }
        public float Radius { get; set; }
        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord record)
        {
            record = new HitRecord();

            var oc = ray.Origin - Center;
            var a = ray.Direction.LengthSquared;
            var half_b = Vector3.Dot(oc, ray.Direction);
            var c = oc.LengthSquared - Radius * Radius;

            var discriminant = half_b * half_b - a * c;
            if (discriminant < 0) return false;
            var sqrtd = MathF.Sqrt(discriminant);

            // Find the nearest root that lies in the acceptable range.
            var root = (-half_b - sqrtd) / a;
            if (root < tMin || tMax < root)
            {
                root = (-half_b + sqrtd) / a;
                if (root < tMin || tMax < root)
                    return false;
            }

            record.t = root;
            record.p = ray.At(record.t);
            record.normal = (record.p - Center) / Radius;

            Vector3 outwardNormal = (record.p - Center) / Radius;
            record.SetFaceNormal(ray, outwardNormal);
            GetSphereUV(outwardNormal, out record.u, out record.v);

            record.Material = Material;
            return true;
        }

        public bool HitBoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            output = new AxisAlignedBoundingBox(
                Center - new Vector3(Radius, Radius, Radius),
                Center + new Vector3(Radius, Radius, Radius)
            );
            return true;
        }

        private static void GetSphereUV(Vector3 p, out float u, out float v)
        {
            var theta = MathF.Acos(Math.Clamp(-p.Y, -1f, 1f));
            var phi = MathF.Atan2(-p.Z, p.X) + MathF.PI;

            u = phi / (2 * MathF.PI);
            v = theta / MathF.PI;
        }
    }
}