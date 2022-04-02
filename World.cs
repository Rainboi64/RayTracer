using System.Collections.Generic;
using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class World : HittableList
    {
        public World()
        {
            var lookFrom = new Vector3(278, 278, -800);
            var lookAt = new Vector3(278, 278, 0);

            // The up vector for the camera
            var up = new Vector3(0f, 1f, 0f);

            var distToFocus = 10f;

            // how much light does the camera allow
            var apreture = 1f / 440f;

            Camera = new Camera(lookFrom, lookAt, up, 40, 1, apreture, distToFocus, 0, 1f);
        }

        public Camera Camera;
        public Dictionary<string, int> Objects = new Dictionary<string, int>();
        public void Add(string name, IHittable item)
        {
            Objects[name] = Count;
            Add(item);
        }
        public T Get<T>(string name) where T : IHittable
        {
            return (T)this[Objects[name]];
        }

        public virtual void Start() { }
        public virtual void Update(float deltaTime) { }
    }
}