using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenGL;

namespace Shift.Graphics.GL
{
    public enum ParamType
    {
        Uniform,
        Attribute
    }

    public class ProgramParam
    {
        private Type type;
        private int location;
        private uint programid;
        private ParamType ptype;
        private string name;

        /// <summary>
        /// Specifies the C# equivalent of the GLSL data type.
        /// </summary>
        public Type Type { get { return type; } }

        /// <summary>
        /// Specifies the location of the parameter in the OpenGL program.
        /// </summary>
        public int Location { get { return location; } }

        /// <summary>
        /// Specifies the OpenGL program ID.
        /// </summary>
        public uint Program { get { return programid; } }

        /// <summary>
        /// Specifies the parameter type (either attribute or uniform).
        /// </summary>
        public ParamType ParamType { get { return ptype; } }

        /// <summary>
        /// Specifies the case-sensitive name of the parameter.
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// Creates a program parameter with a given type and name.
        /// The location must be found after the program is compiled
        /// by using the GetLocation(ShaderProgram Program) method.
        /// </summary>
        /// <param name="Type">Specifies the C# equivalent of the GLSL data type.</param>
        /// <param name="ParamType">Specifies the parameter type (either attribute or uniform).</param>
        /// <param name="Name">Specifies the case-sensitive name of the parameter.</param>
        public ProgramParam(Type Type, ParamType ParamType, string Name)
        {
            type = Type;
            ptype = ParamType;
            name = Name;
        }

        /// <summary>
        /// Creates a program parameter with a type, name, program and location.
        /// </summary>
        /// <param name="Type">Specifies the C# equivalent of the GLSL data type.</param>
        /// <param name="ParamType">Specifies the parameter type (either attribute or uniform).</param>
        /// <param name="Name">Specifies the case-sensitive name of the parameter.</param>
        /// <param name="Program">Specifies the OpenGL program ID.</param>
        /// <param name="Location">Specifies the location of the parameter.</param>
        public ProgramParam(Type Type, ParamType ParamType, string Name, uint Program, int Location)
            : this(Type, ParamType, Name)
        {
            programid = Program;
            location = Location;
        }

        /// <summary>
        /// Gets the location of the parameter in a compiled OpenGL program.
        /// </summary>
        /// <param name="Program">Specifies the shader program that contains this parameter.</param>
        public void GetLocation(ShaderProgram Program)
        {
            Program.Use();
            if (programid == 0)
            {
                programid = Program.ProgramID;
                location = (ptype == ParamType.Uniform ? Program.GetUniformLocation(name) : Program.GetAttributeLocation(name));
            }
        }

        public void SetValue(bool param)
        {
            if (Type != typeof(bool)) throw new Exception(string.Format("SetValue({0}) was given a bool.", Type));
            Gl.Uniform1(location, (param) ? 1 : 0);
        }

        public void SetValue(int param)
        {
            // TODO: Texture support
            if (Type != typeof(int) /*&& Type != typeof(Texture)*/) throw new Exception(string.Format("SetValue({0}) was given a int.", Type));
            Gl.Uniform1(location, param);
        }

        public void SetValue(float param)
        {
            if (Type != typeof(float)) throw new Exception(string.Format("SetValue({0}) was given a float.", Type));
            Gl.Uniform1(location, param);
        }

        public void SetValue(Vector2 param)
        {
            if (Type != typeof(Vector2)) throw new Exception(string.Format("SetValue({0}) was given a Vector2.", Type));
            Gl.Uniform2(location, param.X, param.Y);
        }

        public void SetValue(Vector3 param)
        {
            if (Type != typeof(Vector3)) throw new Exception(string.Format("SetValue({0}) was given a Vector3.", Type));
            Gl.Uniform3(location, param.X, param.Y, param.Z);
        }

        public void SetValue(Vector4 param)
        {
            if (Type != typeof(Vector4)) throw new Exception(string.Format("SetValue({0}) was given a Vector4.", Type));
            Gl.Uniform4(location, param.X, param.Y, param.Z, param.W);
        }

        public void SetValue(Matrix param)
        {
            if (Type != typeof(Matrix)) throw new Exception(string.Format("SetValue({0}) was given a Matrix.", Type));
            GLShortcut.UniformMatrix4(location, param);
        }

        public void SetValue(float[] param)
        {
            if (Type != typeof(Matrix)) throw new Exception(string.Format("SetValue({0}) was given a Matrix.", Type));
            if (param.Length != 16) throw new Exception(string.Format("Expected a float[] of 16 for a Matrix, but instead got {0}.", param.Length));
            Gl.UniformMatrix4(location, 1, false, param);
        }
    }

    public class Shader : IDisposable
    {
        /// <summary>
        /// Specifies the OpenGL ShaderID.
        /// </summary>
        public uint ShaderID { get; private set; }

        /// <summary>
        /// Specifies the type of shader.
        /// </summary>
        public ShaderType ShaderType { get; private set; }

        /// <summary>
        /// Contains all of the attributes and uniforms parsed from this shader source.
        /// </summary>
        public ProgramParam[] ShaderParams { get; private set; }

        /// <summary>
        /// Returns Gl.GetShaderInfoLog(ShaderID), which contains any compilation errors.
        /// </summary>
        public string ShaderLog
        {
            get
            {
                return GLShortcut.GetShaderInfoLog(ShaderID);
            }
        }

        /// <summary>
        /// Compiles a shader, which can be either vertex, fragment or geometry.
        /// </summary>
        /// <param name="source">Specifies the source code of the shader object.</param>
        /// <param name="type">Specifies the type of shader to create (either vertex, fragment or geometry).</param>
        public Shader(ShaderType type, string source)
        {
            this.ShaderType = type;
            this.ShaderID = Gl.CreateShader((int)type);

            GLShortcut.ShaderSource(ShaderID, source);
            Gl.CompileShader(ShaderID);

            GetParams(source);
        }

        ~Shader()
        {
            if (ShaderID != 0) System.Diagnostics.Debug.Fail("Shader was not disposed of properly.");
        }

        private Type GlslTypeFromString(string type)
        {
            switch (type.ToLower())
            {
                case "float": return typeof(float);
                case "bool": return typeof(bool);
                case "int": return typeof(int);
                case "uint": return typeof(uint);
                case "vec2": return typeof(Vector2);
                case "vec3": return typeof(Vector3);
                case "vec4": return typeof(Vector4);
                case "mat4": return typeof(Matrix);
                case "sampler2d":
                case "sampler2dshadow":
                case "sampler1d":
                case "sampler1dshadow":
                case "sampler3d":
                case "sampler2darray":
                case "sampler2darrayshadow": /*return typeof(Texture);*/ // TODO: Texture support
                default: throw new Exception(string.Format("Unsupported GLSL type {0}", type));
            }
        }

        /// <summary>
        /// Parses the shader source code and finds all attributes and uniforms
        /// to cache their location for speedy lookup.
        /// </summary>
        /// <param name="source">Specifies the source code of the shader.</param>
        private void GetParams(string source)
        {
            List<ProgramParam> shaderParams = new List<ProgramParam>();
            var tokens = GLSLLexer.GetTokensFromMemory(source);

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].TokenType == GLSLLexer.TokenType.Keyword && (tokens[i].Text == "uniform" || tokens[i].Text == "attribute" || tokens[i].Text == "in"))
                {
                    // get the parameter type (either uniform or attribute/in)
                    ParamType paramType = (tokens[i].Text == "uniform" ? ParamType.Uniform : ParamType.Attribute);
                    i++;    // move past the uniform/attribute/in keyword

                    // get the glsl type of the parameter
                    if (i == tokens.Count) break;
                    Type type = GlslTypeFromString(tokens[i].Text);
                    i++;    // move past the type

                    // now continue reading parameters until we hit EOF, semi-colon or the glsl programmer assigns a default value
                    while (i < tokens.Count && tokens[i].Text != ";")
                    {
                        if (tokens[i].TokenType == GLSLLexer.TokenType.Word) shaderParams.Add(new ProgramParam(type, paramType, tokens[i].Text));
                        else if (tokens[i].Text == "=") break;  // they've assigned a default value, so continue on
                        i++;
                    }
                }
            }

            this.ShaderParams = shaderParams.ToArray();
        }

        public void Dispose()
        {
            if (ShaderID != 0)
            {
                Gl.DeleteShader(ShaderID);
                this.ShaderID = 0;
            }
        }
    }

    public class ShaderProgram : IDisposable
    {
        /// <summary>
        /// Specifies the OpenGL shader program ID.
        /// </summary>
        public uint ProgramID { get; private set; }

        /// <summary>
        /// Specifies the vertex shader used in this program.
        /// </summary>
        public Shader VertexShader { get; private set; }

        /// <summary>
        /// Specifies the fragment shader used in this program.
        /// </summary>
        public Shader FragmentShader { get; private set; }

        /// <summary>
        /// Specifies whether this program will dispose of the child 
        /// vertex/fragment programs when the IDisposable method is called.
        /// </summary>
        public bool DisposeChildren { get; set; }

        private Dictionary<string, ProgramParam> shaderParams;

        /// <summary>
        /// Queries the shader parameter hashtable to find a matching attribute/uniform.
        /// </summary>
        /// <param name="name">Specifies the case-sensitive name of the shader attribute/uniform.</param>
        /// <returns>The requested attribute/uniform, or null on a failure.</returns>
        public ProgramParam this[string name]
        {
            get { return shaderParams.ContainsKey(name) ? shaderParams[name] : null; }
        }

        /// <summary>
        /// Returns Gl.GetProgramInfoLog(ShaderID), which contains any linking errors.
        /// </summary>
        public string ProgramLog
        {
            get { return GLShortcut.GetProgramInfoLog(ProgramID); }
        }

        /// <summary>
        /// Links a vertex and fragment shader together to create a shader program.
        /// </summary>
        /// <param name="vertexShader">Specifies the vertex shader.</param>
        /// <param name="fragmentShader">Specifies the fragment shader.</param>
        public ShaderProgram(Shader vertexShader, Shader fragmentShader)
        {
            this.VertexShader = vertexShader;
            this.FragmentShader = fragmentShader;
            this.ProgramID = Gl.CreateProgram();
            this.DisposeChildren = false;

            Gl.AttachShader(ProgramID, vertexShader.ShaderID);
            Gl.AttachShader(ProgramID, fragmentShader.ShaderID);
            Gl.LinkProgram(ProgramID);

            GetParams();
        }

        /// <summary>
        /// Creates two shaders and then links them together to create a shader program.
        /// </summary>
        /// <param name="vertexShaderSource">Specifies the source code of the vertex shader.</param>
        /// <param name="fragmentShaderSource">Specifies the source code of the fragment shader.</param>
        public ShaderProgram(string vertexShaderSource, string fragmentShaderSource)
            : this(new Shader(ShaderType.VertexShader, vertexShaderSource), new Shader(ShaderType.FragmentShader, fragmentShaderSource))
        {
            DisposeChildren = true;
        }

        ~ShaderProgram()
        {
            if (ProgramID != 0) System.Diagnostics.Debug.Fail("ShaderProgram was not disposed of properly.");
        }

        /// <summary>
        /// Parses all of the parameters (attributes/uniforms) from the two attached shaders
        /// and then loads their location by passing this shader program into the parameter object.
        /// </summary>
        private void GetParams()
        {
            shaderParams = new Dictionary<string, ProgramParam>();
            foreach (ProgramParam pParam in VertexShader.ShaderParams)
            {
                if (!shaderParams.ContainsKey(pParam.Name))
                {
                    shaderParams.Add(pParam.Name, pParam);
                    pParam.GetLocation(this);
                }
            }
            foreach (ProgramParam pParam in FragmentShader.ShaderParams)
            {
                if (!shaderParams.ContainsKey(pParam.Name))
                {
                    shaderParams.Add(pParam.Name, pParam);
                    pParam.GetLocation(this);
                }
            }
        }

        public void Use()
        {
            if (GLShortcut.CurrentProgram != ProgramID) Gl.UseProgram(this.ProgramID);
        }

        public int GetUniformLocation(string Name)
        {
            Use();
            return Gl.GetUniformLocation(ProgramID, Name);
        }

        public int GetAttributeLocation(string Name)
        {
            Use();
            return Gl.GetAttribLocation(ProgramID, Name);
        }

        public void Dispose()
        {
            if (ProgramID != 0)
            {
                // Make sure this program isn't being used
                if (GLShortcut.CurrentProgram == ProgramID) Gl.UseProgram(0);

                Gl.DetachShader(ProgramID, VertexShader.ShaderID);
                Gl.DetachShader(ProgramID, FragmentShader.ShaderID);
                Gl.DeleteProgram(ProgramID);

                if (DisposeChildren)
                {
                    VertexShader.Dispose();
                    FragmentShader.Dispose();
                }

                this.ProgramID = 0;
            }
        }
    }

}
