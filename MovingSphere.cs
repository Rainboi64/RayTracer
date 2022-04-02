using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class MovingSphere : IHittable
    {
        public float Time0 { get; set; }
        public float Time1 { get; set; }
        public Vector3 Center0 { get; set; }
        public Vector3 Center1 { get; set; }
        public Material Material { get; set; }
        public float Radius { get; set; }

        public MovingSphere(Vector3 center0, Vector3 center1, float time0, float time1, float radius, Material material)
        {
            Center0 = center0;
            Center1 = center1;

            Time0 = time0;
            Time1 = time1;

            Material = material;

            Radius = radius;
        }

        public Vector3 Center(float time)
        {
            return Center0 + ((time - Time0) / (Time1 - Time0)) * (Center1 - Center0);
        }

        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord record)
        {
            record = new HitRecord();

            var oc = ray.Origin - Center(ray.Time);
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

            Vector3 outwardNormal = (record.p - Center(ray.Time)) / Radius;
            record.SetFaceNormal(ray, outwardNormal);
            record.Material = Material;
            return true;
        }

        public bool HitBoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            var box0 = new AxisAlignedBoundingBox(
                Center(time0) - new Vector3(Radius, Radius, Radius),
                Center(time0) + new Vector3(Radius, Radius, Radius)
            );

            var box1 = new AxisAlignedBoundingBox(
                Center(time1) - new Vector3(Radius, Radius, Radius),
                Center(time1) + new Vector3(Radius, Radius, Radius)
            );

            output = Helper.SurroundingBox(box0, box1);

            return true;
        }
    }
}