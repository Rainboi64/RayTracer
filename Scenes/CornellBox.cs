using System;
using OpenTK.Mathematics;

namespace SharpCanvas.Scenes
{
    public class CornellBox : World
    {
        public override void Start()
        {
            var red = new Lambertian(new Color4(.65f, .05f, .05f, 1f));
            var white = new Lambertian(new Color4(.73f, .73f, .73f, 1f));
            var green = new Lambertian(new Color4(.12f, .45f, .15f, 1f));
            var light = new Emmisive(new Color4(15, 15, 15, 1f));
            var mirror = new Metal(Color4.White, 0f);

            Add(new YZRectangle(0, 555, 0, 555, 555, green));
            Add(new YZRectangle(0, 555, 0, 555, 0, red));
            Add(new XZRectangle(213, 343, 227, 332, 554, light));
            Add(new XZRectangle(0, 555, 0, 555, 0, white));
            Add(new XZRectangle(0, 555, 0, 555, 555, white));
            Add(new XYRectangle(0, 555, 0, 555, 555, white));

            IHittable box1 = new Box(new Vector3(0, 0, 0), new Vector3(165, 330, 165), mirror);
            box1 = new RotateY(box1, 25);
            box1 = new Translate(box1, new Vector3(256, 0, 295));

            IHittable box2 = new Box(new Vector3(0, 0, 0), new Vector3(165, 165, 165), white);
            box2 = new RotateY(box2, -18);
            box2 = new Translate(box2, new Vector3(130, 0, 65));

            Add(box1);
            Add(box2);
        }

    }
}