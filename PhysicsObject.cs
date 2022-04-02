using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class PhysicsObject : IHittable
    {
        public float Mass { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 Acceleration { get; set; }
        public Vector3 Force { get; set; }

        public PhysicsObject(IHittable mesh)
        {
            Mesh = mesh;
        }

        public IHittable Mesh;
        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord record)
        {
            return Mesh.Hit(ray, tMin, tMax, out record);
        }

        public bool HitBoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            return Mesh.HitBoundingBox(time0, time1, out output);
        }
    }
}