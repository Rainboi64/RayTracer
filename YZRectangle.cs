using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class YZRectangle : IHittable
    {
        public float Y0, Y1, Z0, Z1, K;
        public Material Material;
        public YZRectangle() { }
        public YZRectangle(float y0, float y1, float z0, float z1, float k, Material material)
        {
            Y0 = y0;
            Y1 = y1;
            Z0 = z0;
            Z1 = z1;

            K = k;

            Material = material;
        }

        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord record)
        {
            record = new HitRecord();

            var t = (K - ray.Origin.X) / ray.Direction.X;
            if (t < tMin || t > tMax)
                return false;
            var y = ray.Origin.Y + t * ray.Direction.Y;
            var z = ray.Origin.Z + t * ray.Direction.Z;
            if (y < Y0 || y > Y1 || z < Z0 || z > Z1)
                return false;
            record.u = (y - Y0) / (Y1 - Y0);
            record.v = (z - Z0) / (Z1 - Z0);
            record.t = t;
            var outward_normal = new Vector3(1, 0, 0);
            record.SetFaceNormal(ray, outward_normal);
            record.Material = Material;
            record.p = ray.At(t);
            return true;
        }

        public bool HitBoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            // The bounding box must have non-zero width in each dimension, so pad the Z
            // dimension a small amount.
            output = new AxisAlignedBoundingBox(new Vector3(K - 0.0001f, Y0, Z0), new Vector3(K + 0.0001f, Y1, Z1));
            return true;
        }
    }
}