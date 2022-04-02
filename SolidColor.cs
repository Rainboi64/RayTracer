using OpenTK.Mathematics;

namespace SharpCanvas
{
    public class SolidColor : ITexture
    {
        public Color4 Color { get; set; }
        public SolidColor(Color4 color)
        {
            Color = color;
        }

        public Color4 Value(float u, float v, Vector3 p)
        {
            return Color;
        }
    }
}