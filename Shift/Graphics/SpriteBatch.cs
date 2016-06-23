using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using OpenGL;
using Shift.Graphics.GL;

namespace Shift.Graphics
{
    /// <summary>
    /// Helper class for drawing sprites in one or more optimized batches.
    /// </summary>
    public class SpriteBatch
    {
        private static ShaderProgram shader;

        static SpriteBatch()
        {
            shader = new ShaderProgram(vertexShaderSource, fragmentShaderSource);
        }

        // Matrix caches
        private Matrix Projection; // Orthographic projection matrix
        private Matrix View; // "Camera" matrix
        private Matrix Model; // Model matrix

        private uint VAO;
        private uint VBO, EBO; // Vertex buffer, Element index buffer

        private int VertexStride = Marshal.SizeOf(new Vertex());

        private int batchCapacity;
        private Dictionary<uint, Batch> batches;

        private bool _beginCalled;

        public SpriteBatch(int capacity = 1000)
        {
            batchCapacity = capacity;

            // Generate VAO and VBOs
            VAO = Gl.GenVertexArray();
            VBO = Gl.GenBuffer();
            EBO = Gl.GenBuffer();

            _beginCalled = false;
        }

        /// <summary>
        /// Begins a new sprite batch . 
        /// </summary>
        public void Begin()
        {
            if (_beginCalled)
                throw new InvalidOperationException("Begin cannot be called again until after End!");
            _beginCalled = true;
            batches = new Dictionary<uint, Batch>();
        }

        public void Draw(Texture texture, Vector2 position)
        {
            if (!_beginCalled)
                throw new InvalidOperationException("Begin must be called before Draw!");
            Vertex tl, tr, bl, br;
            tl = new Vertex(position, 1, Color.White, Vector2.Zero);
            tr = new Vertex(new Vector2(position.X + texture.Size.X, position.Y), 1, Color.White, Vector2.UnitX);
            bl = new Vertex(new Vector2(position.X, position.Y + texture.Size.Y), 1, Color.White, Vector2.UnitY);
            br = new Vertex(new Vector2(position.X + texture.Size.X, position.Y + texture.Size.Y), 1, Color.White, Vector2.One);
            Draw(texture, tl, tr, bl, br);
        }

        private void Draw(Texture texture, Vertex tl, Vertex tr, Vertex bl, Vertex br)
        {
            if (!batches.ContainsKey(texture.TextureID))
                batches.Add(texture.TextureID, new Batch(texture, batchCapacity));

            batches[texture.TextureID].AddSprite(tl, tr, bl, br);
        }

        /// <summary>
        /// Ends batching and flushes batches to GPU.
        /// </summary>
        public void End()
        {
            if (!_beginCalled)
                throw new InvalidOperationException("Begin must be called before End!");
            foreach (var batch in batches.Values)
                DrawBatch(batch);
            _beginCalled = false;
        }

        private void DrawBatch(Batch batch)
        {
            Gl.BindVertexArray(VAO);

            Gl.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);
            Gl.BufferData(BufferTargetARB.ArrayBuffer, batch.VertexCount * (uint)VertexStride, batch.Vertices, BufferUsageARB.StreamDraw);

            Gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, EBO);
            Gl.BufferData(BufferTargetARB.ElementArrayBuffer, batch.VertexCount * sizeof(int), batch.Indices, BufferUsageARB.StreamDraw);

            // Vertex position
            Gl.EnableVertexAttribArray(0);
            Gl.VertexAttribPointer(0, 3, (int)VertexPointerType.Float, false, VertexStride, 0);
            // Color
            Gl.EnableVertexAttribArray(1);
            Gl.VertexAttribPointer(1, 4, Gl.UNSIGNED_BYTE, true, VertexStride, Vector3.SizeInBytes);
            // UV
            Gl.EnableVertexAttribArray(2);
            Gl.VertexAttribPointer(2, 2, (int)VertexPointerType.Float, false, VertexStride, Vector3.SizeInBytes + Color.SizeInBytes);

            shader.Use();

            Gl.BindTexture(batch.Texture.TextureTarget, batch.Texture.TextureID);
            Gl.DrawArrays(PrimitiveType.Triangles, 0, (int)(batch.VertexCount / 3));

            Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            Gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
            Gl.DisableVertexAttribArray(0);
            Gl.DisableVertexAttribArray(1);
            Gl.DisableVertexAttribArray(2);
            Gl.BindVertexArray(0);
        }


        private struct Vertex
        {
            public Vector3 Position;
            public Color Color;
            public Vector2 UV;

            public Vertex(Vector2 position, Color color, Vector2 uv)
            {
                Position = new Vector3(position, 0);
                Color = color;
                UV = uv;
            }

            public Vertex(Vector2 position, int order, Color color, Vector2 uv)
            {
                Position = new Vector3(position, order);
                Color = color;
                UV = uv;
            }

            public Vertex(Vector3 position, Color color, Vector2 uv)
            {
                Position = position;
                Color = color;
                UV = uv;
            }
        }

        private class Batch
        {
            public Vertex[] Vertices;
            public int[] Indices;

            /// <summary>
            /// Number of vertices that can be used in this batch
            /// </summary>
            public int Capacity;

            /// <summary>
            /// Number of vertices currently present in this batch
            /// </summary>
            public uint VertexCount;

            /// <summary>
            /// Texture used by all sprites in this batch
            /// </summary>
            public Texture Texture;

            public Batch(Texture texture, int capacity)
            {
                Texture = texture;
                Capacity = capacity;
                VertexCount = 0;

                Vertices = new Vertex[Capacity];
                Indices = new int[Capacity];

                for (int i = 0; i < (int)(Capacity / 6.0); i++)
                {
                    Indices[i * 6 + 0] = 0 + (i * 4);
                    Indices[i * 6 + 1] = 1 + (i * 4);
                    Indices[i * 6 + 2] = 2 + (i * 4);

                    Indices[i * 6 + 3] = 0 + (i * 4);
                    Indices[i * 6 + 4] = 2 + (i * 4);
                    Indices[i * 6 + 5] = 3 + (i * 4);
                }
            }

            public bool AddSprite(Vertex tl, Vertex tr, Vertex bl, Vertex br)
            {
                if (VertexCount + 6 > Capacity)
                    return false;

                /// Vertex layout:
                /// 0--1
                /// |\ |
                /// | \|
                /// 3--2
                /// 0 -> 1 -> 2 = Triangle #1
                /// 0 -> 2 -> 3 = Triangle #2

                // Triangle 1
                Vertices[VertexCount + 0] = tl;
                Vertices[VertexCount + 1] = tr;
                Vertices[VertexCount + 2] = br;

                // Triangle 2
                Vertices[VertexCount + 3] = tl;
                Vertices[VertexCount + 4] = br;
                Vertices[VertexCount + 5] = bl;

                VertexCount += 6;

                return true;
            }
        }

        // Note: Packed color is ARGB
        private static string vertexShaderSource =
            @"#version 140

            in vec3 vpos;
            in vec4 vcolor;
            in vec2 uv;

            out vec4 color;
            out vec2 texUV;

            void main() {
                texUV = uv;
                color = vcolor
                gl_Position = vec4(vpos, 0);
            }";

        private static string fragmentShaderSource =
            @"#version 140

            in vec4 color;
            in vec2 texUV;

            uniform sampler2D tex;

            out vec4 gl_FragColor;

            void main() {
                gl_FragColor = texture(tex, texUV) * color;
            }";
    }
}
