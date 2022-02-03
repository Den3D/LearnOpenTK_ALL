using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LearnOpenTK_ALL.Example_7
{
    public class ExampleWindow : GameWindow
    {
        private float frameTime = 0.0f;
        private int fps = 0;

        uint[] indexes = new uint[] {
                0, 1, 2,
                0, 2, 3,
                3, 2, 4,
                3, 4, 5,
                5, 4, 6,
                5, 6, 7,

                1, 8, 9,
                1, 9, 2,
                2, 9, 10,
                2, 10, 4,
                4, 10, 11,
                4, 11, 6
        };

        float[] vert_colors = new float[]
        {
                // vertices           // colosrs 
                -0.8f,  0.6f, 0.0f,   1.0f, 0.0f, 0.0f, 1.0f,
                -0.8f,  0.0f, 0.0f,   0.0f, 1.0f, 0.0f, 1.0f,
                -0.2f,  0.0f, 0.0f,   0.0f, 0.0f, 1.0f, 1.0f,
                -0.2f,  0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.2f,  0.0f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.2f,  0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.8f,  0.0f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.8f,  0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,

                -0.8f,  -0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                -0.2f,  -0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.2f,  -0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f,
                 0.8f,  -0.6f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f
        };

        private ShaderProgram shaderProgram;
        private ArrayObject vao;
        private BufferObject vboVC;
        private BufferObject ebo;

        public ExampleWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            NameExampleWindow = "Index Buffer Object";
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

            shaderProgram = new ShaderProgram(@"Assets\shaders\shader_base.vert", @"Assets\shaders\shader_base.frag");
            CreateVAO();
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

        private void CreateVAO()
        {
            vboVC = new BufferObject(BufferType.ArrayBuffer);
            vboVC.SetData(vert_colors, BufferHint.StaticDraw);

            ebo = new BufferObject(BufferType.ElementBuffer);
            ebo.SetData(indexes, BufferHint.StaticDraw);

            int VertexArray = shaderProgram.GetAttribProgram("aPosition");
            int ColorArray = shaderProgram.GetAttribProgram("aColor");

            vao = new ArrayObject();
            vao.Activate();

            vao.AttachBuffer(ebo);
            vao.AttachBuffer(vboVC);

            vao.AttribPointer(VertexArray, 3, AttribType.Float, 7 * sizeof(float), 0);
            vao.AttribPointer(ColorArray, 4, AttribType.Float, 7 * sizeof(float), 3 * sizeof(float));

            vao.Deactivate();
            vao.DisableAttribAll();
        }

        private void Draw()
        {
            shaderProgram.ActiveProgram();
            vao.Activate();
            vao.DrawElements(0, indexes.Length, ElementType.UnsignedInt);
            shaderProgram.DeactiveProgram();
        }

        //-----------------------------------------------------------------------

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Draw();

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUnload()
        {
            vao.Dispose();
            shaderProgram.DeleteProgram();
            base.OnUnload();
        }
    }
}
