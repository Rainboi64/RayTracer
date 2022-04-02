using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class Ray
    {
        public Ray() { }
        public Ray(Vector3 origin, Vector3 direction, float time = 0.0f)
        {
            Origin = origin;
            Direction = direction;
            Time = time;
        }

        public Vector3 Origin { get; set; }
        public Vector3 Direction { get; set; }
        public float Time { get; set; }

        public Vector3 At(float t)
        {
            return Origin + Direction * t;
        }
    }
}