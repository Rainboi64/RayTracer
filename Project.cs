using System.Threading;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using SharpCanvas.Scenes;

namespace SharpCanvas
{
    public partial class Window : GameWindow
    {
        private void Begin()
        {
            var renderer = new Renderer(
                _renderer.Canvas,
                new SolarSystem(),
                Width,
                Height,
                64, // The Size of the blocks
                1 // Number of samples 
                );

            renderer.Aggregate = true;
            new Thread(() =>
            {
                while (true)
                {
                    renderer.Render();
                    // renderer.Save("output");
                }
            }).Start();
        }
    }
}