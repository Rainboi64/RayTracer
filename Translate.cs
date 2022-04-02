using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class Translate : IHittable
    {
        public Vector3 Offset { get; set; }
        public IHittable Hittable { get; set; }

        public Translate(IHittable hittable, Vector3 offset)
        {
            Offset = offset;
            Hittable = hittable;
        }

        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord record)
        {
            var movedRay = new Ray(ray.Origin - Offset, ray.Direction, ray.Time);
            if (!Hittable.Hit(movedRay, tMin, tMax, out record))
                return false;

            record.p += Offset;
            record.SetFaceNormal(movedRay, record.normal);

            return true;
        }

        public bool HitBoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            if (!Hittable.HitBoundingBox(time0, time1, out output))
                return false;

            output = new AxisAlignedBoundingBox(
                output.Maximum + Offset,
                output.Minimum + Offset);

            return true;
        }
    }
}