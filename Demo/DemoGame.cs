using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shift;
using OpenGL;
using Shift.Graphics;

namespace Demo
{
    class DemoGame : Game
    {
        Texture t;
        SpriteBatch spriteBatch;
        public override void Init()
        {
            t = new Texture("Content/Untitled.png");
            spriteBatch = new SpriteBatch();
            Gl.Viewport(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
            Gl.ClearColor(1, 0, 0, 1);
        }

        public override void Draw(GameTime time)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit);

            spriteBatch.Begin();
            spriteBatch.Draw(t, Vector2.Zero);
            spriteBatch.End();
        }

        public override void Update(GameTime time) { }
    }
}
