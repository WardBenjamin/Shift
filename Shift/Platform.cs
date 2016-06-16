using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SDL2;
using Shift.Input;
using Shift.Input.Util;
using EventType = SDL2.SDL.SDL_EventType;

namespace Shift
{
    class Platform
    {
        public enum OS
        {
            Windows,
            Linux,
            Mac
        }
        private static Game _game;
        private static OS _currentOperatingSystem;
        private static List<Keys> _keys;

        public static bool IsActive
        {
            get; private set;
        }

        public static OS CurrentOS
        {
            get
            {
                return _currentOperatingSystem;
            }
        }

        static Platform()
        {
            _keys = new List<Keys>();
            Keyboard.SetKeys(_keys);

            _currentOperatingSystem = GetCurrentOS();
        }


        public static void LoadDLLs()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string resourceName = "AssemblyLoadingAndReflection." +
                   new AssemblyName(args.Name).Name + ".dll";
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
        }

        public static void Init(Game game)
        {
            _game = game;
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);/* | SDL.SDL_INIT_JOYSTICK 
                | SDL.SDL_INIT_GAMECONTROLLER | SDL.SDL_INIT_HAPTIC);**/

            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 1);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, (int)SDL.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_FLAGS, (int)SDL.SDL_GLcontext.SDL_GL_CONTEXT_FORWARD_COMPATIBLE_FLAG);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_DOUBLEBUFFER, 1);

            SDL.SDL_DisableScreenSaver();
        }

        public static void ProcessEvents()
        {
            SDL.SDL_Event ev;

            while (SDL.SDL_PollEvent(out ev) == 1)
            {
                if (ev.type == EventType.SDL_QUIT)
                {
                    _game.Running = false;
                    break;
                }
                /*else if (ev.type == EventType.JoyDeviceAdded)
                    Joystick.AddDevice(ev.JoystickDevice.Which);
                else if (ev.type == EventType.ControllerDeviceRemoved)
                    GamePad.RemoveDevice(ev.ControllerDevice.Which);
                else if (ev.type == EventType.JoyDeviceRemoved)
                    Joystick.RemoveDevice(ev.JoystickDevice.Which);*/
                else if (ev.type == EventType.SDL_MOUSEWHEEL)
                {
                    Mouse.ScrollY += ev.wheel.y * 120;
                }
                else if (ev.type == EventType.SDL_KEYDOWN)
                {
                    var key = KeyboardUtil.KeyFromSDLCode((int)ev.key.keysym.sym);
                    if (!_keys.Contains(key))
                        _keys.Add(key);
                    char character = (char)ev.key.keysym.sym;
                    // TODO: Input events
                    /*if (char.IsControl(character))
                        CallTextInput(character, key);*/
                }
                else if (ev.type == EventType.SDL_KEYUP)
                {
                    var key = KeyboardUtil.KeyFromSDLCode((int)ev.key.keysym.sym);
                    _keys.Remove(key);
                }
                else if (ev.type == EventType.SDL_TEXTINPUT)
                {
                    string text;
                    unsafe
                    {
                        text = new string((char*)ev.text.text);
                    }
                    if (text.Length == 0)
                        continue;
                    foreach (var c in text)
                    {
                        var key = KeyboardUtil.KeyFromSDLCode((int)c);
                        // TODO: Text input again
                        /*_view.CallTextInput(c, key);*/
                    }
                }
                else if (ev.type == EventType.SDL_WINDOWEVENT)
                {
                    //if (ev.window.windowEvent == Sdl.Window.EventId.Resized || ev.Window.EventID == Sdl.Window.EventId.SizeChanged)
                    //_view.ClientResize(ev.Window.Data1, ev.Window.Data2);
                    if (ev.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED)
                        IsActive = true;
                    else if (ev.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST)
                        IsActive = false;
                }
            }
        }

        private static OS GetCurrentOS()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    // Well, there are chances MacOSX is reported as Unix instead of MacOSX.
                    // Instead of platform check, we'll do a feature checks (Mac specific root folders)
                    if (Directory.Exists("/Applications")
                        & Directory.Exists("/System")
                        & Directory.Exists("/Users")
                        & Directory.Exists("/Volumes"))
                        return OS.Mac;
                    else
                        return OS.Linux;

                case PlatformID.MacOSX:
                    return OS.Mac;

                default:
                    return OS.Windows;
            }
        }
    }
}
