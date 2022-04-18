using OpenTK.Mathematics;
using SharpCanvas;

namespace SharpCanvas.Scenes
{
    public class Planet : IHittable
    {
        public Vector3 Translation;

        public float AngleAroundAxis = 0;
        public float Angle = 0;

        public float Radius { get => _child.Radius; set => _child.Radius = value; }
        private Sphere _child;

        public Planet(Vector3 center, float radius, Material material)
        {
            Translation = center;
            _child = new Sphere(Vector3.Zero, radius, material);
        }

        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord record)
        {
            var rotateY = new RotateY(_child, AngleAroundAxis);
            var translate = new Translate(rotateY, Translation);
            rotateY = new RotateY(translate, Angle);

            return rotateY.Hit(ray, tMin, tMax, out record);
        }

        public bool HitBoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            var rotateY = new RotateY(_child, AngleAroundAxis);
            var translate = new Translate(rotateY, Translation);
            rotateY = new RotateY(translate, Angle);

            return rotateY.HitBoundingBox(time0, time1, out output);
        }
    }
}