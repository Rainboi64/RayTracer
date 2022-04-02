using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace SharpCanvas
{
    public class Shader
    {
        private readonly Dictionary<string, int> _nameLocationCache;

        private int _handle;

        public Shader()
        {
            _nameLocationCache = new Dictionary<string, int>();
        }

        public Shader(string vertexPath, string fragmentPath)
        {
            _nameLocationCache = new Dictionary<string, int>();

            string vertexShaderSource;

            // Read shaders from file.

            using (var reader = new StreamReader(vertexPath, Encoding.UTF8))
            {
                vertexShaderSource = reader.ReadToEnd();
            }

            string fragmentShaderSource;

            using (var reader = new StreamReader(fragmentPath, Encoding.UTF8))
            {
                fragmentShaderSource = reader.ReadToEnd();
            }

            LoadSource(vertexShaderSource, fragmentShaderSource);
        }

        public void Enable()
        {
            GL.UseProgram(_handle);
        }

        public void Disable()
        {
            GL.UseProgram(0);
        }
        public int GetAttributeLocation(string name)
        {
            return GL.GetAttribLocation(_handle, name);
        }
        // last implementation was cursed. I seroiusly don't know what the fuccc
        public int GetUniformLocation(string Name)
        {
            var value = GL.GetUniformLocation(_handle, Name);
            return value;
        }

        public void LoadSource(string vertexShaderSource, string pixelShaderSource)
        {
            //Create shaders

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, pixelShaderSource);

            GL.CompileShader(vertexShader);

            //error checking
            var shaderInfoLogVertex = GL.GetShaderInfoLog(vertexShader);
            if (shaderInfoLogVertex != string.Empty)
                Console.WriteLine($"SHADER COMPILATION ERROR:{shaderInfoLogVertex}");

            GL.CompileShader(fragmentShader);
            //error checking
            var shaderInfoLogFrag = GL.GetShaderInfoLog(fragmentShader);
            if (shaderInfoLogFrag != string.Empty)
                Console.WriteLine($"SHADER COMPILATION ERROR:{shaderInfoLogFrag}");

            _handle = GL.CreateProgram();

            GL.AttachShader(_handle, vertexShader);
            GL.AttachShader(_handle, fragmentShader);

            GL.LinkProgram(_handle);

            GL.DetachShader(_handle, vertexShader);
            GL.DetachShader(_handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
        }

        public void Uniform1(string Name, int a)
        {
            GL.Uniform1(GetUniformLocation(Name), a);
        }

        public void Uniform1(string Name, float a)
        {
            GL.Uniform1(GetUniformLocation(Name), a);
        }

        public void Uniform1(string Name, double a)
        {
            GL.Uniform1(GetUniformLocation(Name), a);
        }

        public void Uniform2(string name, double a, double b)
        {
            GL.Uniform2(GetUniformLocation(name), a, b);
        }

        public void Uniform2(string name, int a, int b)
        {
            GL.Uniform2(GetUniformLocation(name), a, b);
        }

        public void Uniform2(string name, float a, float b)
        {
            GL.Uniform2(GetUniformLocation(name), a, b);
        }

        public void Uniform2(string name, Vector2 a)
        {
            GL.Uniform2(GetUniformLocation(name), a);
        }

        public void Uniform3(string name, int a, int b, int c)
        {
            GL.Uniform3(GetUniformLocation(name), a, b, c);
        }

        public void Uniform3(string name, double a, double b, double c)
        {
            GL.Uniform3(GetUniformLocation(name), a, b, c);
        }

        public void Uniform3(string name, float a, float b, float c)
        {
            GL.Uniform3(GetUniformLocation(name), a, b, c);
        }

        public void Uniform3(string name, Vector3 a)
        {
            GL.Uniform3(GetUniformLocation(name), a);
        }

        public void Uniform4(string name, int a, int b, int c, int d)
        {
            GL.Uniform4(GetUniformLocation(name), a, b, c, d);
        }

        public void Uniform4(string name, double a, double b, double c, double d)
        {
            GL.Uniform4(GetUniformLocation(name), a, b, c, d);
        }

        public void Uniform4(string name, float a, float b, float c, float d)
        {
            GL.Uniform4(GetUniformLocation(name), a, b, c, d);
        }

        public void Uniform4(string name, Vector4 a)
        {
            GL.Uniform4(GetUniformLocation(name), a);
        }

        public void UniformMat3(string Name, Matrix3 a, bool Transpose = true)
        {
            GL.UniformMatrix3(GetUniformLocation(Name), Transpose, ref a);
        }

        public void UniformMat4(string Name, Matrix4 a, bool Transpose = true)
        {
            GL.UniformMatrix4(GetUniformLocation(Name), Transpose, ref a);
        }

        // Dispose Code
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            GL.DeleteProgram(_handle);
            _disposedValue = true;
        }

        public void Dispose()
        {
            GL.DeleteProgram(_handle);

            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}