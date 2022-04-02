using OpenTK.Mathematics;

namespace SharpCanvas
{
    public interface ITexture
    {
        Color4 Value(float u, float v, Vector3 p);
    }
}