using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shift;
using OpenGL;

namespace Demo
{
    class DemoGame : Game
    {
        public override void Draw(GameTime time)
        {
            Gl.Viewport(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
            Gl.ClearColor(1, 0, 0, 1);
            Gl.Clear(ClearBufferMask.ColorBufferBit);
        }

        public override void Update(GameTime time) { }
    }
}
