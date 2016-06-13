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
        private int _width, _height;
        private bool _isBorderless;
        private bool _isFullscreen;

        internal IntPtr Handle;
        internal static GameWindow Primary;

        /// <summary>
        /// Gets or sets whether or not the window is in fullscreen. Be aware that 
        /// Shift does not support fullscreen modes that have differing buffer
        /// sizes and actual dimensions (aka it supports "fullscreen window" mode 
        /// but not mode switching. This is false by default;
        /// </summary>
        public bool IsFullscreen
        {
            get { return _isFullscreen; }
            set
            {
                _isFullscreen = value;
                SDL.SDL_SetWindowFullscreen(Handle, value ? (uint)WindowFlags.FullscreenDesktop : 0);
            }
        }

        /// <summary>
        /// Gets the client boundaries of the window (position, width, and height).
        /// </summary>
        public Rectangle ClientBounds
        {
            get
            {
                int x = 0, y = 0;
                SDL.SDL_GetWindowPosition(Handle, out x, out y);
                return new Rectangle(x, y, _width, _height);
            }
        }

        /// <summary>
        /// Gets or sets the window screen position.
        /// </summary>
        public Point Position
        {
            get
            {
                int x = 0, y = 0;

                if (!_isFullscreen)
                    SDL.SDL_GetWindowPosition(Handle, out x, out y);

                return new Point(x, y);
            }
            set { SDL.SDL_SetWindowPosition(Handle, value.X, value.Y); }
        }

        /// <summary>
        /// Gets or sets the border state. If true, the window is borderless. 
        /// If false, the window has a border. This is false by default.
        /// </summary>
        public bool IsBorderless
        {
            get { return _isBorderless; }
            set
            {
                SDL.SDL_SetWindowBordered(Handle, value ? (SDL.SDL_bool)1 : 0);
                _isBorderless = value;
            }
        }


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
            if (Primary == null)
                Primary = this;
            
            Handle = SDL.SDL_CreateWindow(title, PosCentered, PosCentered, width, height, (SDL.SDL_WindowFlags)(WindowFlags.OpenGL |
                WindowFlags.Hidden |
                WindowFlags.InputFocus |
                WindowFlags.MouseFocus));
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

        public const int PosUndefined = 0x1FFF0000;
        public const int PosCentered = 0x2FFF0000;
    }
}
