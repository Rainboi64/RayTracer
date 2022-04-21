using OpenTK.Mathematics;
using System;

namespace SharpCanvas
{
    public class Terrain : HittableList
    {
        float SizeX, SizeY;
        float DetailSize;
        public Terrain(float sizeX, float sizeY, float detailSize) : base()
        {
            SizeX = sizeX;
            SizeY = sizeY;
            DetailSize = detailSize;

            var perlinNoise = new NoiseTexture(0.25f);
            var lookFrom = new Vector3(SizeX + 30, 40, SizeY + 30);
            var lookAt = new Vector3(SizeX - 10, 0, SizeY - 10);


            for (float i = 0; i < SizeX; i += DetailSize)
            {
                for (float j = 0; j < SizeY; j += DetailSize)
                {
                    var horizontalNoise = perlinNoise.Value(i / SizeX, j / SizeY, new Vector3(i + lookAt.X, 0, j + lookAt.Z));
                    var verticalNoise = perlinNoise.Value(i / SizeX, j / SizeY, new Vector3(j + lookAt.Z, 0, i + lookAt.X));

                    var noise = horizontalNoise.R * verticalNoise.R;

                    var R = MathHelper.Lerp(0.1f, 0.0f, noise);
                    var G = MathHelper.Lerp(0.0f, 1.0f, noise);
                    var B = MathHelper.Lerp(0.1f, 0.0f, noise);

                    var surfaceMaterial = new Lambertian(new Color4(R, G, B, 1f));

                    Add(new Box(new Vector3(i, 0, j), new Vector3(i + DetailSize, noise, j + DetailSize), surfaceMaterial));
                }
            }
        }

        public new bool HitBoundingBox(float time0, float time1, out AxisAlignedBoundingBox output)
        {
            output = new AxisAlignedBoundingBox(new Vector3(0, 0, 0), new Vector3(SizeX * DetailSize, 1, SizeY * DetailSize));
            return true;
        }
    }
}