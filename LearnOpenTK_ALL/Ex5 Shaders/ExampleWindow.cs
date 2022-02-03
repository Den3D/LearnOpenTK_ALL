using System;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace LearnOpenTK_ALL.Example_5
{
    public class ExampleWindow : GameWindow
    {
        private float frameTime = 0.0f;
        private int fps = 0;

        float[] vertices = new float[] {
                    -0.5f, -0.5f, 0.0f,
                     0.5f, -0.5f, 0.0f,
                    -0.5f,  0.5f, 0.0f,
                     0.5f,  0.5f, 0.0f
            };

        float[] colosrs = new float[] {
                    1.0f, 0.0f, 0.0f, 1.0f,
                    0.0f, 1.0f, 0.0f, 1.0f,
                    0.0f, 0.0f, 1.0f, 1.0f,
                    0.8f, 0.6f, 0.2f, 1.0f
            };

        float[] vert_colors = new float[]
        {
                // vertices           // colosrs 
                -0.5f, -0.5f, 0.0f,   1.0f, 0.0f, 0.0f, 1.0f,
                 0.5f, -0.5f, 0.0f,   0.0f, 1.0f, 0.0f, 1.0f,
                -0.5f,  0.5f, 0.0f,   0.0f, 0.0f, 1.0f, 1.0f,
                 0.5f,  0.5f, 0.0f,   0.8f, 0.6f, 0.2f, 1.0f
        };

        private int vboVertex = 0;
        private int vboColor = 0;
        private int vaoId = 0;

        private ShaderProgram shaderProgram;

        public ExampleWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            NameExampleWindow = "Shaders";
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
            vaoId = CreateVAOShaders();
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

        private int CreateVBO(float[] data)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            return vbo;
        }

        private int CreateVAOShaders()
        {
            int vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            int vboV = CreateVBO(vertices);
            int vboC = CreateVBO(colosrs);
            int vboVC = CreateVBO(vert_colors);

            int VertexArray = shaderProgram.GetAttribProgram("aPosition");
            int ColorArray = shaderProgram.GetAttribProgram("aColor");

            GL.EnableVertexAttribArray(VertexArray);
            GL.EnableVertexAttribArray(ColorArray);

            // GL.BindBuffer(BufferTarget.ArrayBuffer, vboV);
            // GL.VertexAttribPointer(VertexArray, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            // GL.BindBuffer(BufferTarget.ArrayBuffer, vboC);
            // GL.VertexAttribPointer(ColorArray, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboVC);
            GL.VertexAttribPointer(VertexArray, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
            GL.VertexAttribPointer(ColorArray, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.DisableVertexAttribArray(VertexArray);
            GL.DisableVertexAttribArray(ColorArray);
            return vao;
        }

        private void DrawVAOShaders()
        {
            shaderProgram.ActiveProgram();
            // shaderProgram.SetUniform4("color", new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
            GL.BindVertexArray(vaoId);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            shaderProgram.DeactiveProgram();
        }

        private void DeleteVAOShaders()
        {
            GL.BindVertexArray(0);
            GL.DeleteVertexArray(vaoId);
            GL.DeleteBuffer(vboVertex);
            GL.DeleteBuffer(vboColor);
        }
        //-----------------------------------------------------------------------

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            DrawVAOShaders();

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUnload()
        {
            DeleteVAOShaders();
            shaderProgram.DeleteProgram();
            base.OnUnload();
        }
    }
}
