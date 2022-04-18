//
// Written By: Salah Al-Deen Jojah - Yaman Alhalabi
//
// File Description:
// This file is where all the cool shit happens,
// it takes in the world as a hittable list,
// and camera. it seperates the window in peices
// and renders them in different threads. 
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SharpCanvas
{
    public class Renderer
    {
        public World World;
        public int Width { get; set; }
        public int Height { get; set; }
        public Color4 Background { get; set; } = Color4.Black;
        public int SegmentationSize { get; set; }
        public int SamplesPerPixel { get; set; }
        public int ImagesDone { get; private set; }
        public int MaxDepth { get; set; } = 50;
        public bool Aggregate { get; set; } = false;

        private List<(int x, int y, int x1, int y1)> _segmentations = new List<(int x, int y, int x1, int y1)>();

        private Canvas _canvas;
        private Vector3[] _pixels;

        private bool[] _activeSegments;

        private float _timeSpent = 0.0f;
        private int _segmentsDone = 0;
        private int _samplesDone = 0;

        public Renderer(Canvas canvas, World world, int width, int height, int segmentationSize, int samplesPerPixel)
        {
            _canvas = canvas;
            World = world;

            _pixels = new Vector3[width * height];

            Width = width;
            Height = height;

            SegmentationSize = segmentationSize;
            SamplesPerPixel = samplesPerPixel;

            _samplesDone = SamplesPerPixel;

            World.Start();
        }

        public void Render()
        {
            // if we are aggregating (imporving the picture over time)
            // we want to reset the _samplesDone every time we want to render
            // a new picture, we also need new pixels
            if (!Aggregate)
            {
                _samplesDone = SamplesPerPixel;
                _pixels = new Vector3[Width * Height];
            }

            var cols = Width / SegmentationSize;
            var rows = Height / SegmentationSize;

            var matrix = new (int x, int y, int x1, int y1)[cols, rows];

            // cut up the window on the x-axis
            for (int i = 0; i < Width; i += SegmentationSize)
            {
                // cut up the window on the y-axis
                for (int j = 0; j < Height; j += SegmentationSize)
                {
                    // add the segmentations to the stack
                    matrix[i / SegmentationSize, j / SegmentationSize] = ((i, j, i + SegmentationSize, j + SegmentationSize));
                }
            }

            _segmentations = Helper.SpiralizeArray(matrix, cols);

            // we send the segmentations we made to stand on a queue
            // waiting to be rendered (bread queue)
            QueueSegmentations();

            // after all the segments have been render
            // we update our stats
            ImagesDone += 1;
            _samplesDone += SamplesPerPixel;
        }

        public void QueueSegmentations()
        {
            // start counting time
            var watch = new Stopwatch();
            watch.Start();

            var length = _segmentations.Count;

            // we create an array that has all the states of the 
            // segments we want to render
            _activeSegments = new bool[length];

            // we go through each one
            for (int i = 0; i < length; i++)
            {
                var segment = _segmentations[i];

                // set the status of the segment to working
                _activeSegments[i] = true;

                ThreadPool.QueueUserWorkItem<((int x, int y, int x1, int y1) extents, int index)>((segment) =>
                {
                    // Draw working area
                    _canvas.DrawRectangle(new Vector2i(segment.extents.x, segment.extents.y), new Vector2i(SegmentationSize - 1, SegmentationSize - 1), Color4.Red);

                    // we render the segment
                    SampleSegment(segment.extents);

                    // set segment to done
                    _activeSegments[segment.index] = false;
                    _segmentsDone++;
                }, (segment, i), true);
            }

            // wait until the segments are finished
            while (IsRendering()) ;

            watch.Stop();

            if (!Aggregate)
                World.Update(watch.ElapsedMilliseconds / 1000f);

            _timeSpent += watch.ElapsedMilliseconds;

            Console.WriteLine($"Rendered {_samplesDone} samples after {(watch.ElapsedMilliseconds) / 1000f}s AVG: {(_timeSpent / (_samplesDone / SamplesPerPixel)) / 1000f}s TOT: {_timeSpent / 1000f}s APT: {(((_timeSpent / (_samplesDone / SamplesPerPixel)) / (double)(Width * Height)))}ms AST: {((((_timeSpent / (_samplesDone / SamplesPerPixel)) / (double)(Width * Height)))) / (double)SamplesPerPixel}ms.");

            _segmentsDone = 0;
        }

        private bool IsRendering()
        {
            // check if any of the segment statuses are true,
            // if any one is true the image is still rendering
            foreach (var state in _activeSegments)
            {
                if (state == true)
                    return true;
            }
            return false;
        }

        private void SampleSegment((int x, int y, int x1, int y1) segment)
        {
            // we go through all pixels
            for (int j = segment.y1 - 1; j >= segment.y; --j)
            {
                for (int i = segment.x; i < segment.x1; ++i)
                {
                    // these are going to have the values of 
                    // red green blue but multiplied by SamplesPerPixel
                    // so we are going to divide them later
                    var r = 0.0f;
                    var g = 0.0f;
                    var b = 0.0f;

                    // we repreat the process for the amount of samples
                    for (int s = 0; s < SamplesPerPixel; s++)
                    {
                        // get the percentage where we are in the screen
                        var u = (float)i / (Width - 1);
                        var v = (float)j / (Height - 1);

                        // send the ray through the camera lens
                        Ray ray = World.Camera.GetRay(u, v);

                        // send the ray out ot the world
                        var rayColor = GenerateRayColor(ray, Background, in World, MaxDepth);

                        r += rayColor.R;
                        g += rayColor.G;
                        b += rayColor.B;
                    }

                    // print the color to the image
                    PrintColor(i, j, new Vector3(r, g, b));
                }
            }
        }

        private void PrintColor(int j, int i, Vector3 color)
        {
            // convert 2D (x, y) to array index
            var index = _canvas.ConvertIndex(j, i);

            // check if the index is not correct
            if (index < 0 || index > Width * Height)
                return;

            // add the color to the samples
            var r = color.X + _pixels[index].X;
            var g = color.Y + _pixels[index].Y;
            var b = color.Z + _pixels[index].Z;

            if (float.IsNaN(r)) r = 0.0f;
            if (float.IsNaN(g)) g = 0.0f;
            if (float.IsNaN(b)) b = 0.0f;

            // do the color corrections
            var scale = 1.0f / _samplesDone;
            r = MathF.Sqrt(scale * r);
            g = MathF.Sqrt(scale * g);
            b = MathF.Sqrt(scale * b);

            // send the colors to the image!
            _pixels[index] += color;
            _canvas.Pixels[index] = new Color4(r, g, b, 1f);
        }

        private static Color4 SampleSky(Ray ray)
        {
            // if a ray is going to the sky we just return a color
            // based on it's height
            var unit_direction = ray.Direction.Normalized();
            var t = (0.5f * (unit_direction.Y + 1.0f));
            return new Color4((1f - t) + (t * 0.5f), (1f - t) + (t * 0.7f), (1f - t) + (t * 1f), 1f);
        }

        private Color4 GenerateRayColor(Ray ray, Color4 background, in World world, int depth)
        {
            // if the it has bounced more than the max return black
            if (depth <= 0)
            {
                return Color4.Black;
            }

            // send out a ray to the world from 0.0001 from here and to positive infinity
            // if the ray hits continue, otherwise draw sky
            if (!world.Hit(ray, 0.001f, float.PositiveInfinity, out var record))
                return background;

            Ray scattered = new Ray(Vector3.Zero, Vector3.Zero);
            Color4 attenuation = Color4.White;

            // Scatter the ray based on the hit material
            if (!record.Material.Scatter(ray, record, out attenuation, out scattered))
            {
                // if the material doesn't scatter the ray, return the light color
                return record.Material.Emitted(record.u, record.v, record.p);
            }

            // repreat the proccess (reflect) with - 1 depth
            var color = GenerateRayColor(scattered, background, in world, depth - 1);
            return new Color4(attenuation.R * color.R, attenuation.G * color.G, attenuation.B * color.B, 1f);
        }

        public void Save(string path)
        {
            // just save the image man, you don't need to understand this
            // it just works
            using (Image<Rgba32> image = new Image<Rgba32>(Width, Height))
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Span<Rgba32> row = image.GetPixelRowSpan(y);

                    for (int x = 0; x < row.Length; x++)
                    {
                        ref Rgba32 pixel = ref row[x];
                        pixel.R = (byte)(_canvas.Get(x, Height - y).R * 255);
                        pixel.G = (byte)(_canvas.Get(x, Height - y).G * 255);
                        pixel.B = (byte)(_canvas.Get(x, Height - y).B * 255);
                        pixel.A = 255;
                    }
                }
                image.SaveAsPngAsync($"{path}.png");
            }
        }
    }
}