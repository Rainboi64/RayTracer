using System;
using System.Linq;

namespace SharpCanvas
{
    public class BVHNode : IHittable
    {
        IHittable Left;
        IHittable Right;
        AxisAlignedBoundingBox Box;

        public BVHNode(HittableList list, int start, int end, float time0, float time1)
        {
            var objects = list; // Create a modifiable array of the source scene objects

            int axis = Helper.Random.Next(0, 2);
            Func<IHittable, IHittable, bool> comparator = (axis == 0) ? BoxCompareX
                            : (axis == 1) ? BoxCompareY
                                        : BoxCompareZ;

            int object_span = end - start;

            if (object_span == 1)
            {
                Left = Right = objects[start];
            }
            else if (object_span == 2)
            {
                if (comparator(objects[start], objects[start + 1]))
                {
                    Left = objects[start];
                    Right = objects[start + 1];
                }
                else
                {
                    Left = objects[start + 1];
                    Right = objects[start];
                }
            }
            else
            {
                objects.OrderBy(x => comparator);

                var mid = start + object_span / 2;
                Left = new BVHNode(objects, start, mid, time0, time1);
                Right = new BVHNode(objects, mid, end, time0, time1);
            }

            AxisAlignedBoundingBox boxLeft, boxRight;

            var bl = !Left.HitBoundingBox(time0, time1, out boxLeft);
            var br = !Right.HitBoundingBox(time0, time1, out boxRight);

            if (bl || br)
                Console.WriteLine("No bounding box in bvh_node constructor.");

            Box = Helper.SurroundingBox(boxLeft, boxRight);
        }

        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord record)
        {
            record = new HitRecord();

            if (!Box.Hit(ray, tMin, tMax)) return false;

            bool hit_Left = Left.Hit(ray, tMin, tMax, out record);
            bool hit_Right = Right.Hit(ray, tMin, tMax, out record);

            return hit_Left || hit_Right;
        }

        public bool HitBoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            output = Box;
            return true;
        }


        bool BoxCompare(IHittable a, IHittable b, int axis)
        {
            AxisAlignedBoundingBox box_a;
            AxisAlignedBoundingBox box_b;

            var ab = a.HitBoundingBox(0, 0, out box_a);
            var bb = b.HitBoundingBox(0, 0, out box_b);

            if (!ab || !bb)
                Console.WriteLine("No bounding box in bvh_node constructor.");

            return box_a.Minimum[axis] < box_b.Minimum[axis];
        }


        bool BoxCompareX(IHittable a, IHittable b)
        {
            return BoxCompare(a, b, 0);
        }

        bool BoxCompareY(IHittable a, IHittable b)
        {
            return BoxCompare(a, b, 1);
        }

        bool BoxCompareZ(IHittable a, IHittable b)
        {
            return BoxCompare(a, b, 2);
        }
    }
}