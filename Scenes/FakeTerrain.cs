using System;
using OpenTK.Mathematics;

namespace SharpCanvas.Scenes
{
    public class FakeTerrain : World
    {
        float SizeX = 20, SizeY = 20;
        float DetailSize = 0.1f;

        public override void Start()
        {
            var lookFrom = new Vector3(SizeX + 40, 50, SizeY + 40);
            var lookAt = new Vector3(SizeX - 10, 0, SizeY - 10);

            // The up vector for the camera
            var up = new Vector3(0, 1f, 0f);

            var distToFocus = 10f;

            // how much light does the camera allow
            var apreture = 1f / 440f;

            Camera = new Camera(lookFrom, lookAt, up, 20, 1, apreture, distToFocus, 0, 1f);

            //Add("Light", new Sphere(new Vector3(SizeX + 40, 100, SizeY + 40), 30, new Emmisive(Color4.White, 20f)));
            Add("Terrain", new Terrain(SizeX, SizeY, DetailSize));
        }

        public override void Update(float deltaTime)
        {
            //     time += deltaTime;
            //     Get<Sphere>("First").Radius = MathF.Sin(time);
            //     Get<Sphere>("Second").Radius = MathF.Cos(time) * 9f;
        }
    }
}