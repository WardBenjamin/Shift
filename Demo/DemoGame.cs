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
        public override void Init()
        {
            //Texture t = new Texture("Content/Bitmap1.bmp");
        }

        public override void Draw(GameTime time)
        {
            Gl.Viewport(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
            Gl.ClearColor(1, 0, 0, 1);
            Gl.Clear(ClearBufferMask.ColorBufferBit);
        }

        public override void Update(GameTime time) { }
    }
}
