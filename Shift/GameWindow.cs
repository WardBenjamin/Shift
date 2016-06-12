using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDL2;
using Shift.Input;

namespace Shift
{
    public class GameWindow
    {
        internal IntPtr Handle;
        internal static GameWindow Primary;

        /// <summary>
        /// Gets the client boundaries of the window (position, width, and height).
        /// </summary>
        public Rectangle ClientBounds { get; private set; }

        /// <summary>
        /// MouseState associated with the window. Currently is not required because 
        /// only one window is supported, but could be useful in the future.
        /// </summary>
        internal MouseState MouseState { get; set; }

        /// <summary>
        /// Creates a new GameWindow with specified width, height, and title.
        /// </summary>
        /// <param name="width">Width of the created window</param>
        /// <param name="height">Height of the created window</param>
        /// <param name="title">Title of the created window</param>
        internal GameWindow(int width, int height, string title)
        {
            MouseState = MouseState.Default;
            Primary = this;
        }

        internal WindowFlags GetWindowFlags()
        {
            return (WindowFlags)SDL.SDL_GetWindowFlags(Handle);
        }

        /// <summary>
        /// Show the window.
        /// </summary>
        public void Show()
        {
            SDL.SDL_ShowWindow(Handle);
        }

        /// <summary>
        /// Hide the window.
        /// </summary>
        public void Hide()
        {
            SDL.SDL_HideWindow(Handle);
        }

        [Flags]
        internal enum WindowFlags
        {
            Fullscreen = 0x00000001,
            OpenGL = 0x00000002,
            Shown = 0x00000004,
            Hidden = 0x00000008,
            Boderless = 0x00000010,
            Resizable = 0x00000020,
            Minimized = 0x00000040,
            Maximized = 0x00000080,
            Grabbed = 0x00000100,
            InputFocus = 0x00000200,
            MouseFocus = 0x00000400,
            FullscreenDesktop = 0x00001001,
            Foreign = 0x00000800,
            AllowHighDPI = 0x00002000,
            MouseCapture = 0x00004000,
        }
    }
}
