using System;

using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LearnOpenTK_ALL.Example_3
{
    public class ExampleWindow : GameWindow
    {
        private float frameTime = 0.0f;
        private int fps = 0;

        public ExampleWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            NameExampleWindow = "Primitives";
            Title = NameExampleWindow;

            Console.WriteLine(GL.GetString(StringName.Version));
            Console.WriteLine(GL.GetString(StringName.Vendor));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.ShadingLanguageVersion));

            VSync = VSyncMode.On;
            CursorVisible = true;
        }

        public string NameExampleWindow { private set; get; }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(173 / 255.0f, 216 / 255.0f, 230 / 255.0f, 255 / 255.0f);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            // GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
            // GL.PolygonMode(MaterialFace.Back, PolygonMode.Point);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            frameTime += (float)args.Time;
            fps++;
            if (frameTime >= 1.0f)
            {
                Title = $"OpenTK {NameExampleWindow} : FPS - {fps}";
                frameTime = 0.0f;
                fps = 0;
            }

            var key = KeyboardState;

            if (key.IsKeyDown(Keys.Escape))
            {
                Console.WriteLine(Keys.Escape.ToString());
                Close();
            }

            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.LineWidth(5.0f);
            GL.PointSize(8.0f);

            GL.Rotate(0.3f, 0.0f, 1.0f, 0.0f);
            GL.Color3(1.0f, 0.0f, 0.0f);

            GL.Begin(PrimitiveType.LineStrip);
                GL.Vertex2(-0.8f, 0.5f);
                GL.Vertex2(-0.8f, -0.5f);
                GL.Vertex2(-0.5f, 0.5f);
                GL.Vertex2(-0.5f, -0.5f);
                GL.Vertex2(-0.3f, 0.5f);
                GL.Vertex2(-0.3f, -0.5f);
                GL.Vertex2(0.3f, 0.5f);
                GL.Vertex2(0.3f, -0.5f);
                GL.Vertex2(0.5f, 0.5f);
                GL.Vertex2(0.5f, -0.5f);
                GL.Vertex2(0.7f, 0.5f);
                GL.Vertex2(0.7f, -0.5f);
                GL.Vertex2(0.9f, 0.5f);
                GL.Vertex2(0.9f, -0.5f);
            GL.End();

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
        }

        private void DrawLine()
        {
            GL.LineWidth(10.0f);

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex2(0.5f, 0.5f);

            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex2(0.5f, -0.5f);

            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex2(-0.5f, -0.5f);

            GL.Color3(1.0f, 0.0f, 0.6f);
            GL.Vertex2(-0.5f, 0.5f);
            GL.End();
        }
    }
}
