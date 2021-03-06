using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace SharpCanvas
{
    public class Debug
    {
        /// <summary>
        /// Enables The call-back of GL errors to be displayed in Console.
        /// </summary>
        public Debug()
        {
            GL.DebugMessageCallback(_debugProcCallback, IntPtr.Zero);
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
        }

        private static readonly DebugProc _debugProcCallback = DebugCallback;
        private static void DebugCallback(DebugSource source,
            DebugType type,
            int id,
            DebugSeverity severity,
            int length,
            IntPtr message,
            IntPtr userParam)
        {
            var messageString = Marshal.PtrToStringAnsi(message, length);

            if (type == DebugType.DebugTypeError)
                Console.WriteLine($"OpenGL Debug System: {severity} {type} | {messageString}");
            else
                Console.WriteLine($"OpenGL Debug System: {severity} {type} | {messageString}");
        }
    }
}