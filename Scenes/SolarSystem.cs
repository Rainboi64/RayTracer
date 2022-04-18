using OpenTK.Mathematics;

namespace SharpCanvas.Scenes
{
    public class SolarSystem : World
    {
        public override void Start()
        {
            var lookFrom = new Vector3(24000, 24000, 24000);
            var lookAt = new Vector3(0f, 0f, 0f);

            // The up vector for the camera
            var up = new Vector3(.1f, 1f, 0f);

            var distToFocus = 10f;

            // how much light does the camera allow
            var apreture = 1f / 440f;

            Camera = new Camera(lookFrom, lookAt, up, 20, 1, apreture, distToFocus, 0, 1f);

            var sun = new Emmisive(new ImageTexture("sunmap.jpg"), 1000f);
            Add("Sun", new Planet(Vector3.Zero, 1000f, sun));

            var earth = new Lambertian(new ImageTexture("earthmap.jpg"));
            Add("Earth", new Planet(new Vector3(0, 0, 1500f), 300f, earth));
            Get<Planet>("Sun").AngleAroundAxis += 1;

            Get<Planet>("Earth").AngleAroundAxis += 1 * 12;
            Get<Planet>("Earth").Angle += 1 * 300;
        }

        public override void Update(float deltaTime)
        {

        }
    }
}