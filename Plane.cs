using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class Plane : IHittable
    {
        public Vector3 Normal;
        public Material Material;
        public Vector3 Center;
        public Vector3 A, B, C, D;
        public float K = 1f;
        public Plane(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Material material)
        {
            Material = material;

            A = a;
            B = b;
            C = c;
            D = d;

            Center = (a + b + c + d) / 4f;

            var AB = b - a;
            var CD = d - c;

            Normal = Vector3.Cross(AB, CD);

            K = (d - a).Length;
        }

        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord record)
        {
            record = new HitRecord();
            float denom = Vector3.Dot(Normal, ray.Direction);
            if (Math.Abs(denom) > 0.0001f
            ) // your favorite epsilon
            {
                float t = Vector3.Dot(Center - ray.Origin, Normal) / denom;

                var intersection = (ray.At(t) * ray.Direction);
                // var x = ray.Origin.X + t * ray.Direction.X;
                // var y = ray.Origin.Y + t * ray.Direction.Y;

                var x = intersection.X;
                var y = intersection.Y;

                if (!(x < A.X || x > D.X || y < A.X || y > D.Y))
                    return false;

                record.u = (x - A.X) / (D.X - A.X);
                record.v = (y - A.Y) / (D.Y - A.Y);

                record.SetFaceNormal(ray, Normal);
                record.Material = Material;
                record.p = ray.At(t);

                if (t >= 0.0001f) return true; // you might want to allow an epsilon here too
            }
            return false;
        }

        public bool HitBoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            output = new AxisAlignedBoundingBox(new Vector3(A.X, A.Y, K - 0.0001f), new Vector3(D.X, D.Y, K + 0.0001f));
            return true;
        }
    }
}