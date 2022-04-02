using OpenTK.Mathematics;

namespace SharpCanvas.Scenes
{
    public class BouncingMarbles : World
    {
        float time;
        public override void Start()
        {
            var lookFrom = new Vector3(13f, 2f, 3f);
            var lookAt = new Vector3(0f, 0f, 0f);

            // The up vector for the camera
            var up = new Vector3(.1f, 1f, 0f);

            var distToFocus = 10f;

            // how much light does the camera allow
            var apreture = 1f / 440f;

            Camera = new Camera(lookFrom, lookAt, up, 40, 1, apreture, distToFocus, 0, 1f);

            var groundMaterial = new Lambertian(new NoiseTexture(4));
            //var groundMaterial = new Lambertian(new CheckerTexture(Color4.BurlyWood, Color4.White));
            Add("Ground", new Sphere(new Vector3(0f, -1000f, 0), 1000, groundMaterial));

            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    var chooseMat = Helper.RandomFloat(0, 1);
                    var center = new Vector3(a + 0.9f * Helper.RandomFloat(0, 1), 0.2f, b + 0.9f * Helper.RandomFloat(0, 1));

                    if ((center - new Vector3(4f, 0.2f, 0)).Length > 0.9f)
                    {
                        Material sphereMaterial;

                        if (chooseMat > 0.5)
                        {
                            var albedo = Helper.RandomColor();

                            sphereMaterial = new Lambertian(albedo);

                            var center2 = center + new Vector3(0f, Helper.RandomFloat(0, .5f), 0f);
                            Add(new MovingSphere(center, center2, 0.0f, 1.0f, 0.2f, sphereMaterial));
                        }
                        else if (chooseMat < 0.95)
                        {
                            var albedo = Helper.RandomColor(0.5f, 1.0f);
                            var fuzz = Helper.RandomFloat(0.0f, 0.5f);

                            sphereMaterial = new Metal(albedo, fuzz);
                            Add(new Sphere(center, 0.2f, sphereMaterial));
                        }
                        else
                        {
                            sphereMaterial = new Dielectric(1.5f);
                            Add(new Sphere(center, 0.2f, sphereMaterial));
                        }
                    }
                }
            }

            var material1 = new Dielectric(0f);
            Add("Glass", new Sphere(new Vector3(0f, 1f, 0f), 1f, material1));

            var material2 = new Lambertian(new CheckerTexture(new RandomColorTexture(), Color4.Black));
            Add("Matte", new Sphere(new Vector3(-4f, 1f, 0f), 1.0f, material2));

            var material3 = new Metal(new Color4(0.7f, 0.6f, 0.5f, 1f), 0.5f);
            //var material3 = new Lambertian(new NoiseTexture());
            Add("Metal", new Sphere(new Vector3(4f, 1f, 0f), 1.0f, material3));
        }

        public override void Update(float deltaTime)
        {
            time = time + deltaTime;
            Get<Sphere>("Matte").Material = new Lambertian(new CheckerTexture(new Color4(deltaTime, deltaTime, deltaTime, 1f), Color4.Black));
        }
    }
}