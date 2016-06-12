using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDL2;

namespace Shift.Input
{
    class Keyboard
    {
        private static List<Keys> _keys = new List<Keys>();

        public static KeyboardState GetState()
        {
            var modifiers = (ModifierFlags)SDL.SDL_GetModState();
            return new KeyboardState(_keys, 
                modifiers.HasFlag(ModifierFlags.CapsLock), 
                modifiers.HasFlag(ModifierFlags.NumLock)
            );
        }

        [Flags]
        enum ModifierFlags : ushort
        {
            None = 0x0000,
            LeftShift = 0x0001,
            RightShift = 0x0002,
            LeftCtrl = 0x0040,
            RightCtrl = 0x0080,
            LeftAlt = 0x0100,
            RightAlt = 0x0200,
            LeftGui = 0x0400,
            RightGui = 0x0800,
            NumLock = 0x1000,
            CapsLock = 0x2000,
            AltGr = 0x4000,
            Ctrl = (LeftCtrl | RightCtrl),
            Shift = (LeftShift | RightShift),
            Alt = (LeftAlt | RightAlt),
            Gui = (LeftGui | RightGui)
        }
    }
}
