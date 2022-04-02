using System.Collections;
using System.Collections.Generic;

namespace SharpCanvas
{
    public class HittableList : IList<IHittable>
    {

        public bool BoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            output = new AxisAlignedBoundingBox();

            if (Count < 1)
                return false;

            AxisAlignedBoundingBox tempBox;
            bool firstBox = false;

            foreach (var item in _hittables)
            {
                if (!item.HitBoundingBox(time0, time1, out tempBox)) return false;
                output = firstBox ? tempBox : Helper.SurroundingBox(output, tempBox);
                firstBox = false;
            }

            return true;
        }

        public bool Hit(Ray ray, float tMin, float tMax, out HitRecord record)
        {
            record = new HitRecord();
            bool hitAnything = false;
            var closestSoFar = tMax;

            for (int i = 0; i < _hittables.Count; i++)
            {
                if (_hittables[i].Hit(ray, tMin, closestSoFar, out var tempRecord))
                {
                    hitAnything = true;
                    closestSoFar = tempRecord.t;
                    record = tempRecord;
                }
            }

            return hitAnything;
        }

        private List<IHittable> _hittables = new List<IHittable>();
        public IHittable this[int index] { get => _hittables[index]; set => _hittables[index] = value; }

        public int Count => _hittables.Count;

        public bool IsReadOnly => false;

        public void Add(IHittable item)
        {
            _hittables.Add(item);
        }

        public void Clear()
        {
            _hittables.Clear();
        }

        public bool Contains(IHittable item)
        {
            return _hittables.Contains(item);
        }

        public void CopyTo(IHittable[] array, int arrayIndex)
        {
            _hittables.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IHittable> GetEnumerator()
        {
            return _hittables.GetEnumerator();
        }

        public int IndexOf(IHittable item)
        {
            return _hittables.IndexOf(item);
        }

        public void Insert(int index, IHittable item)
        {
            _hittables.Insert(index, item);
        }

        public bool Remove(IHittable item)
        {
            return _hittables.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _hittables.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _hittables.GetEnumerator();
        }
    }
}