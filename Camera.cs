using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class Camera
    {
        Random random = new Random();
        float ViewportHeight;
        float ViewportWidth;

        Vector3 w, u, v;
        float lensRadius;

        public float time0, time1;

        Vector3 Origin;
        Vector3 Horizontal;
        Vector3 Vertical;
        Vector3 LowerLeftCorner;

        public Camera(Vector3 lookFrom, Vector3 lookAt, Vector3 vUp, float vFOV, float aspectRatio, float apreture, float focus_distance, float time0, float time1)
        {
            // deg to rad
            var theta = vFOV * 0.0174532925f;
            var h = MathF.Tan(theta / 2f);

            ViewportHeight = 2.0f * h;
            ViewportWidth = aspectRatio * ViewportHeight;

            w = (lookFrom - lookAt).Normalized();
            u = Vector3.Cross(vUp, w);
            v = Vector3.Cross(w, u);

            Origin = lookFrom;

            Horizontal = focus_distance * ViewportWidth * u;
            Vertical = focus_distance * ViewportHeight * v;
            LowerLeftCorner = Origin - Horizontal / 2f - Vertical / 2f - focus_distance * w;
        
            lensRadius = apreture / 2;

            this.time0 = time0;
            this.time1 = time1;
        }

        public Ray GetRay(float s, float t)
        {
            var rd = lensRadius * Helper.RandomInUnitDisk();
            var offset = u * rd.X + v * rd.Y; 

            return new Ray(Origin + offset,
            LowerLeftCorner + s * Horizontal + t * Vertical - Origin - offset, Helper.RandomFloat(time0, time1));
        }
    }
}