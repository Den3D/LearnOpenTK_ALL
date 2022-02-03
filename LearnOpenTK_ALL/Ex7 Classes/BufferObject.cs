using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace LearnOpenTK_ALL.Example_7
{
    public enum BufferType
    {
        ArrayBuffer = BufferTarget.ArrayBuffer,
        ElementBuffer = BufferTarget.ElementArrayBuffer
    }

    public enum BufferHint
    {
        StaticDraw = BufferUsageHint.StaticDraw
    }


    public sealed class BufferObject : IDisposable
    {
        private const int ErrorCode = -1;

        public int BufferID { private set; get; } = 0;
        private readonly BufferTarget _type;
        private bool _active = false;

        public BufferObject(BufferType type)
        {
            _type = (BufferTarget) type;
            BufferID = GL.GenBuffer();
        }

        public void SetData<T>(T[] data, BufferHint hint)where T : struct
        {
            if (data.Length == 0)
                throw new ArgumentException("Массив должен содержать хотябы один элемент", "data");
            
            Activate();
            GL.BufferData(_type, (IntPtr)(data.Length * Marshal.SizeOf(typeof(T))), data, (BufferUsageHint) hint);
        }

        public void Activate()
        {
            _active = true;
            GL.BindBuffer( _type, BufferID);
        }

        public void Deactivate()
        {
            _active = false;
            GL.BindBuffer(_type, 0);
        }

        public bool IsActive()
        {
            return _active;
        }

        public void Delete()
        {
            if (BufferID == ErrorCode)
                return;

            Deactivate();
            GL.DeleteBuffer(BufferID);

            BufferID = ErrorCode;
        }

        public void Dispose()
        {
            Delete();
            GC.SuppressFinalize(this);
        }
    }
}
