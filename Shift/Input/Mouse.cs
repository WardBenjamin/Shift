using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shift.Input
{
    class Mouse
    {
        public static MouseState GetState(GameWindow window)
        {
            int x, y;

            var winFlags = Sdl.Window.GetWindowFlags(window.Handle);
            var state = (Sdl.Patch > 4) ? // SDL 2.0.4 has a bug with Global Mouse
                    Sdl.Mouse.GetGlobalState(out x, out y) :
                    Sdl.Mouse.GetState(out x, out y);
            var clientBounds = window.ClientBounds;

            if (winFlags.HasFlag(Sdl.Window.State.MouseFocus))
            {
                window.MouseState.LeftButton = (state.HasFlag(Sdl.Mouse.Button.Left)) ? ButtonState.Pressed : ButtonState.Released;
                window.MouseState.MiddleButton = (state.HasFlag(Sdl.Mouse.Button.Middle)) ? ButtonState.Pressed : ButtonState.Released;
                window.MouseState.RightButton = (state.HasFlag(Sdl.Mouse.Button.Right)) ? ButtonState.Pressed : ButtonState.Released;
                window.MouseState.XButton1 = (state.HasFlag(Sdl.Mouse.Button.X1Mask)) ? ButtonState.Pressed : ButtonState.Released;
                window.MouseState.XButton2 = (state.HasFlag(Sdl.Mouse.Button.X2Mask)) ? ButtonState.Pressed : ButtonState.Released;

                window.MouseState.ScrollWheelValue = ScrollY;
            }

            window.MouseState.X = x - ((Sdl.Patch > 4) ? clientBounds.X : 0);
            window.MouseState.Y = y - ((Sdl.Patch > 4) ? clientBounds.Y : 0);

            return window.MouseState;
        }
    }

    class MouseState
    {

    }
}
