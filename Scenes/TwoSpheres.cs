using System;
using OpenTK.Mathematics;

namespace SharpCanvas.Scenes
{
    public class TwoSpheres : World
    {
        float time = 0;
        public override void Start()
        {
            var lookFrom = new Vector3(26, 3, 6);
            var lookAt = new Vector3(0f, 2f, 0f);

            // The up vector for the camera
            var up = new Vector3(.1f, 1f, 0f);

            var distToFocus = 10f;

            // how much light does the camera allow
            var apreture = 1f / 440f;

            Camera = new Camera(lookFrom, lookAt, up, 20, 1, apreture, distToFocus, 0, 1f);

            var matte = new Lambertian(new ImageTexture("earthmap.jpg"));
            var matte2 = new Lambertian(new SolidColor(Color4.AntiqueWhite));
            var light = new Emmisive(new Color4(4f, 4f, 4f, 1f));

            Add("First", new Sphere(new Vector3(0, -1000, 0), 1000f, matte2));
            Add("Second", new Sphere(new Vector3(0, 2, 0), 2, matte));
            Add("Third", new XYRectangle(3, 5, 1, 3, -2, light));
        }

        public override void Update(float deltaTime)
        {
            time += deltaTime;
            Get<Sphere>("First").Radius = MathF.Sin(time);
            Get<Sphere>("Second").Radius = MathF.Cos(time) * 9f;
        }
    }
}