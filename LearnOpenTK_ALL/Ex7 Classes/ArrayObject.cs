using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.OpenGL;

namespace LearnOpenTK_ALL.Example_7
{
    public enum AttribType
    {
        Float =  VertexAttribPointerType.Float
    }

    public enum ElementType
    {
        UnsignedInt = DrawElementsType.UnsignedInt
    }

    public class ArrayObject : IDisposable
    {
        private const int ErrorCode = -1;

        public int ArrayID { private set; get; } = 0;

        private bool _active = false;

        private List<int> _attribsList;
        private List<BufferObject> _buffersList;

        public ArrayObject()
        {
            _attribsList = new List<int>();
            _buffersList = new List<BufferObject>();
            ArrayID = GL.GenVertexArray();
        }

        public void Activate()
        {
            _active = true;
            GL.BindVertexArray(ArrayID);
        }

        public void Deactivate()
        {
            _active = false;
            GL.BindVertexArray(0);
        }

        public bool IsActive()
        {
            return _active;
        }

        public void AttachBuffer(BufferObject buffer)
        {
            if (IsActive() != true)
                Activate();

            buffer.Activate();
            _buffersList.Add(buffer);
        }

        public void AttribPointer(int index, int elementsPerVertex, AttribType type, int stride, int offset)
        {
            _attribsList.Add(index);
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, elementsPerVertex, (VertexAttribPointerType)type, false, stride, offset);
        }

        public void Draw(int start, int count)
        {
            Activate();
            GL.DrawArrays(PrimitiveType.Triangles, start, count);
        }

        public void DrawElements(int start, int count, ElementType type)
        {
            Activate();
            GL.DrawElements(PrimitiveType.Triangles, count, (DrawElementsType) type, start);
        }

        public void DisableAttribAll()
        {
            foreach(int attrib in _attribsList)
                GL.DisableVertexAttribArray(attrib);
        }

        public void Delete()
        {
            if (ArrayID == ErrorCode)
                return;

            Deactivate();
            GL.DeleteVertexArray(ArrayID);

            foreach (BufferObject buffer in _buffersList)
                buffer.Dispose();

            ArrayID = ErrorCode;
        }

        public void Dispose()
        {
            Delete();
            GC.SuppressFinalize(this);
        }
    }
}

