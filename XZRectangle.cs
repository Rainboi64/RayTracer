using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class XZRectangle : IHittable
    {
        public float X0, X1, Z0, Z1, K;
        public Material Material;
        public XZRectangle() { }
        public XZRectangle(float x0, float x1, float z0, float z1, float k, Material material)
        {
            X0 = x0;
            X1 = x1;
            Z0 = z0;
            Z1 = z1;

            K = k;

            Material = material;
        }

        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord record)
        {
            record = new HitRecord();

            var t = (K - ray.Origin.Y) / ray.Direction.Y;
            if (t < tMin || t > tMax)
                return false;
            var x = ray.Origin.X + t * ray.Direction.X;
            var z = ray.Origin.Z + t * ray.Direction.Z;
            if (x < X0 || x > X1 || z < Z0 || z > Z1)
                return false;
            record.u = (x - X0) / (X1 - X0);
            record.v = (z - Z0) / (Z1 - Z0);
            record.t = t;
            var outward_normal = new Vector3(0, 1, 0);
            record.SetFaceNormal(ray, outward_normal);
            record.Material = Material;
            record.p = ray.At(t);
            return true;
        }

        public bool HitBoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            // The bounding box must have non-zero width in each dimension, so pad the Z
            // dimension a small amount.
            output = new AxisAlignedBoundingBox(new Vector3(X0, K - 0.0001f, Z0), new Vector3(X1, K + 0.0001f, Z1));
            return true;
        }
    }
}