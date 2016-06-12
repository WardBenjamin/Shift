using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDL2;

namespace Shift.Input
{
    class Mouse
    {
        internal static int ScrollY;

        public static MouseState GetState()
        {
            return GetState(GameWindow.Primary);
        }

        private static MouseState GetState(GameWindow window)
        {
            int x, y;

            var winFlags = window.GetWindowFlags();
            // NOTE: SDL 2.0.4 has a bug with Global Mouse. When fixed, replace this with GlobalMouseState
            var state = (ButtonFlags)SDL.SDL_GetMouseState(out x, out y);
            var clientBounds = window.ClientBounds;
            var windowState = window.MouseState;


            if (winFlags.HasFlag(GameWindow.WindowFlags.MouseFocus))
            {
                windowState.LeftButton = (state.HasFlag(ButtonFlags.Left)) ? ButtonState.Pressed : ButtonState.Released;
                windowState.MiddleButton = (state.HasFlag(ButtonFlags.Middle)) ? ButtonState.Pressed : ButtonState.Released;
                windowState.RightButton = (state.HasFlag(ButtonFlags.Right)) ? ButtonState.Pressed : ButtonState.Released;
                windowState.XButton1 = (state.HasFlag(ButtonFlags.X1Mask)) ? ButtonState.Pressed : ButtonState.Released;
                windowState.XButton2 = (state.HasFlag(ButtonFlags.X2Mask)) ? ButtonState.Pressed : ButtonState.Released;

                windowState.ScrollWheelValue = ScrollY;
            }

            // NOTE: Subtract clientBounds.X and Y instead of zero when Global Mouse bug is fixed.
            windowState.X = x - 0;
            windowState.Y = y - 0;

            window.MouseState = windowState;

            return window.MouseState;
        }

        public static void SetPosition(int x, int y)
        {
            SetPosition(GameWindow.Primary, x, y);
        }

        private static void SetPosition(GameWindow window, int x, int y)
        {
            MouseState windowState = window.MouseState;
            windowState.X = x;
            windowState.Y = y;

            window.MouseState = windowState;
            SDL.SDL_WarpMouseInWindow(window.Handle, x, y);
        }

        public static void SetCursor()//MouseCursor cursor)
        {
            //Sdl.Mouse.SetCursor(cursor.Handle);
        }

        [Flags]
        enum ButtonFlags
        {
            Left = 1 << 0,
            Middle = 1 << 1,
            Right = 1 << 2,
            X1Mask = 1 << 3,
            X2Mask = 1 << 4
        }
    }
}
