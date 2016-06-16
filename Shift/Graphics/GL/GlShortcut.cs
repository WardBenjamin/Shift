using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenGL;

namespace Shift.Graphics.GL
{
    class GLShortcut
    {
        public static uint CurrentProgram
        {
            get
            {
                int programID = 0;
                Gl.Get(Gl.CURRENT_PROGRAM, out programID);
                if (programID < 0)
                    throw new MemberAccessException("Returned current program ID is less than zero");
                return (uint)programID;
            }
        }
        public static string GetShaderInfoLog(uint shaderID)
        {
            int length = 0;
            Gl.GetShader(shaderID, Gl.INFO_LOG_LENGTH, out length);
            if (length == 0) return String.Empty;
            StringBuilder sb = new StringBuilder(length);
            Gl.GetShaderInfoLog(shaderID, sb.Capacity, out length, sb);
            return sb.ToString();
        }

        public static string GetProgramInfoLog(uint programID)
        {
            int length = 0;
            Gl.GetProgram(programID, Gl.INFO_LOG_LENGTH, out length);
            if (length == 0) return String.Empty;
            StringBuilder sb = new StringBuilder(length);
            Gl.GetProgramInfoLog(programID, sb.Capacity, out length, sb);
            return sb.ToString();
        }

        public static void UniformMatrix4(int location, Matrix param)
        {
            // use the statically allocated float[] for setting the uniform
            GLStatic.matrixFloat[0] = param[0, 0]; GLStatic.matrixFloat[1] = param[0, 1]; GLStatic.matrixFloat[2] = param[0, 2]; GLStatic.matrixFloat[3] = param[0, 3];
            GLStatic.matrixFloat[4] = param[1, 0]; GLStatic.matrixFloat[5] = param[1, 1]; GLStatic.matrixFloat[6] = param[1, 2]; GLStatic.matrixFloat[7] = param[1, 3];
            GLStatic.matrixFloat[8] = param[2, 0]; GLStatic.matrixFloat[9] = param[2, 1]; GLStatic.matrixFloat[10] = param[2, 2]; GLStatic.matrixFloat[11] = param[2, 3];
            GLStatic.matrixFloat[12] = param[3, 0]; GLStatic.matrixFloat[13] = param[3, 1]; GLStatic.matrixFloat[14] = param[3, 2]; GLStatic.matrixFloat[15] = param[3, 3];

            Gl.UniformMatrix4(location, 1, false, GLStatic.matrixFloat);
        }

        public static void ShaderSource(uint shaderID, string source)
        {
            Gl.ShaderSource(shaderID, new string[] { source });
        }
    }
}
