using System;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class RotateY : IHittable
    {
        public IHittable Hittable { get; set; }

        public float SinTheta { get; set; }
        public float CosTheta { get; set; }
        private bool _hasBox;
        private AxisAlignedBoundingBox _bbox;

        public RotateY(IHittable hittable, float angle)
        {
            Hittable = hittable;

            var radians = angle * MathF.PI / 180;
            SinTheta = MathF.Sin(radians);
            CosTheta = MathF.Cos(radians);
            _hasBox = Hittable.HitBoundingBox(0, 1, out _bbox);

            var min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            var max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        var x = i * _bbox.Maximum.X + (1 - i) * _bbox.Minimum.X;
                        var y = j * _bbox.Maximum.Y + (1 - j) * _bbox.Minimum.Y;
                        var z = k * _bbox.Maximum.Z + (1 - k) * _bbox.Minimum.Z;

                        var newx = CosTheta * x + SinTheta * z;
                        var newz = -SinTheta * x + CosTheta * z;

                        var tester = new Vector3(newx, y, newz);

                        for (int c = 0; c < 3; c++)
                        {
                            min[c] = MathF.Min(min[c], tester[c]);
                            max[c] = MathF.Max(max[c], tester[c]);
                        }
                    }
                }
            }

            _bbox = new AxisAlignedBoundingBox(min, max);
        }

        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord record)
        {
            var origin = ray.Origin;
            var direction = ray.Direction;

            origin[0] = CosTheta * ray.Origin[0] - SinTheta * ray.Origin[2];
            origin[2] = SinTheta * ray.Origin[0] + CosTheta * ray.Origin[2];

            direction[0] = CosTheta * ray.Direction[0] - SinTheta * ray.Direction[2];
            direction[2] = SinTheta * ray.Direction[0] + CosTheta * ray.Direction[2];

            var rotated_r = new Ray(origin, direction, ray.Time);

            if (!Hittable.Hit(rotated_r, tMin, tMax, out record))
                return false;

            var p = record.p;
            var normal = record.normal;

            p[0] = CosTheta * record.p[0] + SinTheta * record.p[2];
            p[2] = -SinTheta * record.p[0] + CosTheta * record.p[2];

            normal[0] = CosTheta * record.normal[0] + SinTheta * record.normal[2];
            normal[2] = -SinTheta * record.normal[0] + CosTheta * record.normal[2];

            record.p = p;
            record.SetFaceNormal(rotated_r, normal);

            return true;
        }

        public bool HitBoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            output = _bbox;
            return _hasBox;
        }
    }
}