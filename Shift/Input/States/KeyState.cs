using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shift.Input
{
    /// <summary>
    /// Identifies the state of a keyboard key.
    /// </summary>
    public enum KeyState
    {
        /// <summary>
        /// Key is released.
        /// </summary>
        Up,

        /// <summary>
        /// Key is pressed.
        /// </summary>
        Down,
    }
}
