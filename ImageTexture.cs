using System;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SharpCanvas
{
    public class ImageTexture : ITexture
    {
        public int Width { get; }
        public int Height { get; }
        public Color4[,] Pixels;

        public ImageTexture() { }
        public ImageTexture(string path)
        {
            var image = Image.Load<RgbaVector>(path);
            Width = image.Width;
            Height = image.Height;

            Pixels = new Color4[Width, Height];

            for (int y = 0; y < image.Height; y++)
            {
                Span<RgbaVector> row = image.GetPixelRowSpan(y);

                for (int x = 0; x < row.Length; x++)
                {
                    ref RgbaVector pixel = ref row[x];
                    Pixels[x, y] = Unsafe.As<RgbaVector, Color4>(ref pixel);
                }
            }
        }

        public Color4 Value(float u, float v, Vector3 p)
        {
            // Clamp input texture coordinates to [0,1] x [1,0]
            u = Math.Clamp(u, 0.0f, 1.0f);
            v = 1.0f - Math.Clamp(v, 0.0f, 1.0f);  // Flip V to image coordinates

            var i = (int)(u * Width);
            var j = (int)(v * Height);

            if (i >= Width) i = Width - 1;
            if (j >= Height) j = Height - 1;

            return Pixels[i, j];
        }

    }
}