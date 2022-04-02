using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class AxisAlignedBoundingBox
    {
        public Vector3 Maximum { get; }
        public Vector3 Minimum { get; }

        public AxisAlignedBoundingBox()
        { }

        public AxisAlignedBoundingBox(Vector3 minimum, Vector3 maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public bool Hit(Ray ray, float tMin, float tMax)
        {
            for (int a = 0; a < 3; a++)
            {
                var invD = 1.0f / ray.Direction[a];
                var t0 = (Minimum[a] - ray.Origin[a]) * invD;
                var t1 = (Maximum[a] - ray.Origin[a]) * invD;

                if (invD < 0.0f)
                {
                    var _t0 = t0;
                    var _t1 = t1;

                    t1 = _t0;
                    t0 = _t1;
                }

                tMin = t0 > tMin ? t0 : tMin;
                tMax = t1 < tMax ? t1 : tMax;

                if (tMax <= tMin)
                {
                    return false;
                }
            }

            return true;
        }
    }
}