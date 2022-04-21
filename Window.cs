//
// Written By: Salah Al-Deen Jojah - Yaman Alhalabi
//
// File Description:
// This file has the code necessary to open up
// a window and kick-off the rendering proccess 
//

using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace SharpCanvas
{
    public partial class Window : GameWindow
    {
        public float AspectRatio = 16f / 9f;
        public int Width, Height;
        private CanvasRenderer _renderer;

        public Window() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            // Hello Salah!

            // Program kick-off!

            Title = "Ray Tracer, Salah Al-Deen Jojah - Yaman Alhalabi";

            AspectRatio = 1f / 1f;
            // Width = 768;
            Width = 512;
            Height = (int)(Width / AspectRatio);

            Size = new Vector2i(Width, Height);

            // Intializes OpenGL debugger
            new Debug();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            // change our data when user resizes window
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnLoad()
        {
            // Create a canvas so we can draw stuff on
            _renderer = new CanvasRenderer();
            _renderer.Canvas = new Canvas();
            _renderer.Canvas.InitializeCanvas(Width, Height);
            _renderer.Create();

            // Begin the rendering stuff!
            // this calls the function in the Project.cs file
            // go to it to learn more about the rendering process
            Begin();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            // Set the clear color to pink, very easy to notice
            // when shit hits the fan. 
            GL.ClearColor(1f, 0f, 1f, 1f);

            // Actually sends stuff to the GPU
            _renderer.Render();
            SwapBuffers();
        }
    }
}