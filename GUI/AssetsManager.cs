using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections.ObjectModel;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

using LinearAlgebra;

namespace Logic
{
    public class Pipeline
    {
        public int id;
        public Dictionary<string, int> locations;
        public Pipeline(int id)
        {
            this.id = id;
            locations = new Dictionary<string, int>();
            int uniformsCount;
            GL.GetProgram(id, GetProgramParameterName.ActiveUniforms, out uniformsCount);
            for (int i = 0; i < uniformsCount; i++)
            {
                string uniformName = GL.GetActiveUniform(id, i, out _, out _);
                if (uniformName.EndsWith("[0]"))
                    uniformName = uniformName.Substring(0, uniformName.Length - 3);
                locations[uniformName] = GL.GetUniformLocation(id, uniformName);
            }
        }
    }
    public class Shader
    {
        public int Id { get; private set; }
        public ShaderType Type { get; private set; }
        public Shader(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)) || !File.Exists(path))
                throw new FileNotFoundException("Vertex shader file not found", path);

            switch (Path.GetExtension(path))
            {
                case ".vsh":
                    Type = ShaderType.VertexShader;
                    break;
                case ".fsh":
                    Type = ShaderType.FragmentShader;
                    break;
                case ".gsh":
                    Type = ShaderType.GeometryShader;
                    break;
                case ".csh":
                    Type = ShaderType.ComputeShader;
                    break;
                default:
                    throw new ArgumentException("Unable to get shader type from file extension, change the file extension or define shader type explicitly by using other constructor overload.");
            }
            Id = GL.CreateShader(Type);
            GL.ShaderSource(Id, File.ReadAllText(path));
            GL.CompileShader(Id);
            int result;
            GL.GetShader(Id, ShaderParameter.CompileStatus, out result);
            if (result == 0)
                throw new Exception("Shader compilation error, shader type: " + Type.ToString() + ", error: " + GL.GetShaderInfoLog(Id));
        }
        public Shader(ShaderType type, string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)) || !File.Exists(path))
                throw new FileNotFoundException("Vertex shader file not found", path);

            Type = type;
            Id = GL.CreateShader(type);
            GL.ShaderSource(Id, File.ReadAllText(path));
            GL.CompileShader(Id);
            int result;
            GL.GetShader(Id, ShaderParameter.CompileStatus, out result);
            if (result == 0)
                throw new Exception("Shader compilation error, shader type: " + Type.ToString() + ", error: " + GL.GetShaderInfoLog(Id));
        }
    }
    public static class AssetsManager
    {
        private static Dictionary<string, Pipeline> pipelines = new Dictionary<string, Pipeline>();
        public static ReadOnlyDictionary<string, Pipeline> Pipelines
        {
            get
            {
                return new ReadOnlyDictionary<string, Pipeline>(pipelines);
            }
        }
        public static Pipeline LoadPipeline(string pipelineName, params Shader[] shaders)
        {
            int program = GL.CreateProgram();
            foreach (Shader shader in shaders)
                GL.AttachShader(program, shader.Id);
            GL.LinkProgram(program);
            int result;
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out result);
            if (result == 0)
                throw new Exception("Program linking error: " + GL.GetProgramInfoLog(program));

            foreach (Shader shader in shaders)
            {
                GL.DetachShader(program, shader.Id);
                GL.DeleteShader(shader.Id);
            }

            Pipeline pipeline = new Pipeline(program);
            pipelines[pipelineName] = pipeline;
            return pipeline;
        }
    }
}
