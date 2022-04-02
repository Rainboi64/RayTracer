using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class Box : IHittable
    {
        public Vector3 Start { get; set; }
        public Vector3 End { get; set; }

        private HittableList _sides = new HittableList();

        public Box(Vector3 start, Vector3 end, Material material)
        {
            Start = start;
            End = end;

            _sides.Add(new XYRectangle(Start.X, End.X, Start.Y, End.Y, End.Z, material));
            _sides.Add(new XYRectangle(Start.X, End.X, Start.Y, End.Y, Start.Z, material));

            _sides.Add(new XZRectangle(Start.X, End.X, Start.Z, End.Z, End.Y, material));
            _sides.Add(new XZRectangle(Start.X, End.X, Start.Z, End.Z, Start.Y, material));

            _sides.Add(new YZRectangle(Start.Y, End.Y, Start.Z, End.Z, End.X, material));
            _sides.Add(new YZRectangle(Start.Y, End.Y, Start.Z, End.Z, Start.X, material));
        }

        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord record)
        {
            return _sides.Hit(ray, tMin, tMax, out record);
        }

        public bool HitBoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            output = new AxisAlignedBoundingBox(Start, End);
            return true;
        }
    }
}