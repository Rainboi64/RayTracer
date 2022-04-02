//
// CanvasRenderer.cs
//
// Copyright (C) 2021 Yaman Alhalabi
//

using System;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace SharpCanvas
{
    public class CanvasRenderer
    {
        public Canvas Canvas;

        private const string _vertexShader = @"
#version 420 core

layout(location = 0) in vec3 aPosition;

layout(location = 1) in vec2 aTexCoord;

out vec3 vert;
out vec2 texCoord;

void main(void)
{
    texCoord = aTexCoord;
    vert = aPosition;
    gl_Position = vec4(aPosition, 1.0);
}
";
        private const string _fragmentShader = @"
#version 420 core

out vec4 outputColor;

in vec3 vert;
in vec2 texCoord;

uniform sampler2D texture0;

void main()
{
    outputColor = texture(texture0, texCoord) ;
}
";

        private static readonly float[] _vertices =
        {
            //Position        Texture coordinates
             1f,  1f, 0f,     1f, 1f, // top right
             1f, -1f, 0f,     1f, 0f, // bottom right
            -1f, -1f, 0f,     0f, 0f, // bottom left
            -1f,  1f, 0f,     0f, 1f  // top left
        };

        private static readonly int[] _indices = {
            0, 1, 3, // first triangle
            1, 2, 3  // second triangle
        };

        private int _vbo = 0;
        private int _ebo = 0;
        private int _texture = 0;
        private Shader _shader;
        private int _vao;

        public void Create()
        {
            _shader = new Shader();
            _shader.LoadSource(_vertexShader, _fragmentShader);

            _vao = GL.GenVertexArray();

            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            _texture = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, _texture);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * _vertices.Length, _vertices, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * _indices.Length, _indices, BufferUsageHint.DynamicDraw);
        }

        private void CreateTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, _texture);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                Canvas.Width,
                Canvas.Height,
                0,
                PixelFormat.Rgba,
                PixelType.Float,
                Canvas.Pixels);

            GL.ActiveTexture(TextureUnit.Texture0);
        }

        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Enable();
            _shader.Uniform1("texture0", 0);
            CreateTexture();

            GL.BindVertexArray(_vao);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(
            0,
            3,
            VertexAttribPointerType.Float,
            false,
            4 * 3 + 4 * 2,
            0
            );

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(
            1,
            2,
            VertexAttribPointerType.Float,
            false,
            4 * 3 + 4 * 2,
            4 * 3
            );
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, _indices);
        }

    }
}
