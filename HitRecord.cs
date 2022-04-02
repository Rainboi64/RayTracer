using OpenTK.Mathematics;

namespace SharpCanvas
{
    public struct HitRecord
    {
        public Vector3 p;
        public Vector3 normal;
        public float t;
        public float u;
        public float v;
        public bool FrontFace;
        public Material Material;
        public void SetFaceNormal(Ray ray, Vector3 outwardNormal)
        {
            FrontFace = Vector3.Dot(ray.Direction, outwardNormal) < 0;
            normal = FrontFace ? outwardNormal : -outwardNormal;
        }
    }
}