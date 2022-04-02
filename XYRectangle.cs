using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class XYRectangle : IHittable
    {
        public float X0, X1, Y0, Y1, K;
        public Material Material;
        public XYRectangle() { }
        public XYRectangle(float x0, float x1, float y0, float y1, float k, Material material)
        {
            X0 = x0;
            X1 = x1;
            Y0 = y0;
            Y1 = y1;

            K = k;

            Material = material;
        }

        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord record)
        {
            record = new HitRecord();

            var t = (K - ray.Origin.Z) / ray.Direction.Z;
            if (t < tMin || t > tMax)
                return false;
            var x = ray.Origin.X + t * ray.Direction.X;
            var y = ray.Origin.Y + t * ray.Direction.Y;
            if (x < X0 || x > X1 || y < Y0 || y > Y1)
                return false;
            record.u = (x - X0) / (X1 - X0);
            record.v = (y - Y0) / (Y1 - Y0);
            record.t = t;
            var outward_normal = new Vector3(0, 0, 1);
            record.SetFaceNormal(ray, outward_normal);
            record.Material = Material;
            record.p = ray.At(t);
            return true;
        }

        public bool HitBoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            // The bounding box must have non-zero width in each dimension, so pad the Z
            // dimension a small amount.
            output = new AxisAlignedBoundingBox(new Vector3(X0, Y0, K - 0.0001f), new Vector3(X1, Y1, K + 0.0001f));
            return true;
        }
    }
}