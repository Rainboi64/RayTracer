using System;
using System.IO;
using OpenTK.Mathematics;
using StbImageSharp;

namespace SharpCanvas
{
    public class ImageTexture : ITexture
    {
        public int Width { get; }
        public int Height { get; }
        public Color4[] Pixels;

        public ImageTexture() { }
        public ImageTexture(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlue);
                Width = image.Width;
                Height = image.Height;

                Pixels = new Color4[Width * Height];

                for (int i = 0; i < Width * Height; ++i)
                {
                    float r = image.Data[i * 3 + 0] / 255f;
                    float g = image.Data[i * 3 + 1] / 255f;
                    float b = image.Data[i * 3 + 2] / 255f;

                    Pixels[i] = new Color4(r, g, b, 1f);
                }
            }
        }

        public Color4 Value(float u, float v, Vector3 p)
        {
            // Clamp input texture coordinates to [0,1] x [1,0]
            u = Math.Clamp(u, 0.0f, 1.0f);
            v = 1f - Math.Clamp(v, 0.0f, 1.0f);  // Flip V to image coordinates

            var i = (int)((float)Width * u);
            var j = (int)((float)Height * v);

            if (i >= Width) i = Width - 1;
            if (j >= Height) j = Height - 1;

            return Pixels[j * Height + i];
        }

    }
}