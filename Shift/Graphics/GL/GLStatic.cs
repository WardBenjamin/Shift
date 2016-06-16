using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shift.Graphics.GL
{
    class GLStatic
    {
        #region Preallocated Memory

        // Preallocate the float[] for matrix data. This is a huge optimization 
        // when large amounts of Gl.UniformMatrix4() calls occur.
        internal static float[] matrixFloat = new float[16];
        internal static uint[] int1 = new uint[1];
        internal static bool[] bool1 = new bool[1];

        #endregion

        internal static uint CurrentProgram = 0;
    }
}
