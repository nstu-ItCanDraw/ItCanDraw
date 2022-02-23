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
    /// <summary>
    /// Contains compiled and linked pipeline identificator and uniform variables locations.
    /// </summary>
    public class Pipeline
    {
        /// <summary>
        /// Pipeline OpenGL identificator
        /// </summary>
        public int Id { get; private set; }
        private Dictionary<string, int> locations;
        /// <summary>
        /// Uniform variables locations, where key is variable's name and value is it's location
        /// </summary>
        public ReadOnlyDictionary<string, int> Locations { get => new ReadOnlyDictionary<string, int>(locations); }
        public Pipeline(params Shader[] shaders)
        {
            Id = GL.CreateProgram();
            List<ShaderType> shaderTypes = new List<ShaderType>();
            foreach (Shader shader in shaders)
            {
                GL.AttachShader(Id, shader.Id);
                if (shaderTypes.Contains(shader.Type))
                    throw new ArgumentException("Pipeline can have only one of each stage, but found more than one of \"" + shader.Type.ToString() + "\" stage.");
                shaderTypes.Add(shader.Type);
            }

            if (!shaderTypes.Contains(ShaderType.VertexShader))
                throw new ArgumentException("Vertex shader stage is neccesary for pipeline.");
            if (!shaderTypes.Contains(ShaderType.FragmentShader))
                throw new ArgumentException("Fragment shader stage is neccesary for pipeline.");

            GL.LinkProgram(Id);
            int result;
            GL.GetProgram(Id, GetProgramParameterName.LinkStatus, out result);
            if (result == 0)
                throw new Exception("Program linking error: " + GL.GetProgramInfoLog(Id));

            foreach (Shader shader in shaders)
            {
                GL.DetachShader(Id, shader.Id);
                GL.DeleteShader(shader.Id);
            }

            locations = new Dictionary<string, int>();
            int uniformsCount;
            GL.GetProgram(Id, GetProgramParameterName.ActiveUniforms, out uniformsCount);
            for (int i = 0; i < uniformsCount; i++)
            {
                string uniformName = GL.GetActiveUniform(Id, i, out _, out _);
                if (uniformName.EndsWith("[0]"))
                    uniformName = uniformName.Substring(0, uniformName.Length - 3);
                locations[uniformName] = GL.GetUniformLocation(Id, uniformName);
            }
        }
    }
    /// <summary>
    /// Contains compiled shader information
    /// </summary>
    public class Shader
    {
        /// <summary>
        /// Shader OpenGL identificator
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Shader type
        /// </summary>
        public ShaderType Type { get; private set; }
        /// <summary>
        /// Loads and compiles shader from given path based on file extension: vsh - vertex shader, fsh - fragment shader, gsh - geometry shader, csh - compute shader
        /// </summary>
        public Shader(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)) || !File.Exists(path))
                throw new FileNotFoundException("Shader file not found", path);

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
        /// <summary>
        /// Loads and compiles shader from given path based on given shader type
        /// </summary>
        public Shader(ShaderType type, string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)) || !File.Exists(path))
                throw new FileNotFoundException("Shader file not found", path);

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
    /// <summary>
    /// Loads and contains assets such as shader pipelines
    /// </summary>
    public static class AssetsManager
    {
        private static Dictionary<string, Pipeline> pipelines = new Dictionary<string, Pipeline>();
        /// <summary>
        /// Loaded shader pipelines, where key is shader pipeline name and value is pipeline itself
        /// </summary>
        public static ReadOnlyDictionary<string, Pipeline> Pipelines
        {
            get
            {
                return new ReadOnlyDictionary<string, Pipeline>(pipelines);
            }
        }
        /// <summary>
        /// Links given shaders into pipeline and saves it with given name to dictionary
        /// </summary>
        public static Pipeline LoadPipeline(string pipelineName, params Shader[] shaders)
        {
            if (pipelines.ContainsKey(pipelineName))
                throw new ArgumentException("Pipeline with this name is already loaded.");

            Pipeline pipeline = new Pipeline(shaders);
            pipelines[pipelineName] = pipeline;
            return pipeline;
        }
        /// <summary>
        /// Compiles and links shaders from given paths into pipeline and saves it with given name to dictionary
        /// </summary>
        public static Pipeline LoadPipeline(string pipelineName, params string[] shaderPaths)
        {
            if (pipelines.ContainsKey(pipelineName))
                throw new ArgumentException("Pipeline with this name is already loaded.");

            Pipeline pipeline = new Pipeline(shaderPaths.Select(path => new Shader(path)).ToArray());
            pipelines[pipelineName] = pipeline;
            return pipeline;
        }
    }
}
