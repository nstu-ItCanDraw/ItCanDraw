using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections.ObjectModel;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

using LinearAlgebra;

namespace GUI
{
    /// <summary>
    /// Contains compiled and linked pipeline identificator and uniform variables locations.
    /// </summary>
    public class Pipeline : IDisposable
    {
        /// <summary>
        /// Current pipeline in use
        /// </summary>
        public static Pipeline Current { get; private set; } = null;
        /// <summary>
        /// Returns true, if this pipeline is current
        /// </summary>
        public bool IsCurrent { get => this == Current; }
        private int id;
        private Dictionary<string, int> locations = new Dictionary<string, int>();
        private bool disposed;
        public Pipeline(params Shader[] shaders)
        {
            id = GL.CreateProgram();
            List<ShaderType> shaderTypes = new List<ShaderType>();
            foreach (Shader shader in shaders)
            {
                GL.AttachShader(id, shader.Id);
                if (shaderTypes.Contains(shader.Type))
                    throw new ArgumentException("Pipeline can have only one of each stage, but found more than one of \"" + shader.Type.ToString() + "\" stage.");
                shaderTypes.Add(shader.Type);
            }

            if (!shaderTypes.Contains(ShaderType.VertexShader))
                throw new ArgumentException("Vertex shader stage is neccesary for pipeline.");
            if (!shaderTypes.Contains(ShaderType.FragmentShader))
                throw new ArgumentException("Fragment shader stage is neccesary for pipeline.");

            GL.LinkProgram(id);
            int result;
            GL.GetProgram(id, GetProgramParameterName.LinkStatus, out result);
            if (result == 0)
                throw new Exception("Program linking error: " + GL.GetProgramInfoLog(id));

            foreach (Shader shader in shaders)
            {
                GL.DetachShader(id, shader.Id);
                GL.DeleteShader(shader.Id);
            }

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
        ~Pipeline()
        {
            Dispose(disposing: false);
        }
        /// <summary>
        /// Sets this pipeline as current in OpenGL
        /// </summary>
        public void Use()
        {
            if (disposed)
                throw new ObjectDisposedException("Pipeline");
            Current = this;
        }
        private void checkCanSetUniform(string name)
        {
            if (disposed)
                throw new ObjectDisposedException("Pipeline");
            if (!IsCurrent)
                throw new Exception("Can't set uniform variables, this pipeline is not current.");
            if (!locations.ContainsKey(name))
                throw new ArgumentException("This pipeline don't have uniform variable with name \"" + name + "\". If you are sure that it exists in shader, make sure it is used, GLSL compiler removes unused variables.");
        }
        #region Uniform1
        /// <summary>
        /// Sets scalar uniform by it's name in shader
        /// </summary>
        public void Uniform1(string name, double value)
        {
            checkCanSetUniform(name);
            GL.Uniform1(locations[name], value);
        }
        /// <summary>
        /// Sets scalar uniform by it's name in shader
        /// </summary>
        public void Uniform1(string name, float value)
        {
            checkCanSetUniform(name);
            GL.Uniform1(locations[name], value);
        }
        /// <summary>
        /// Sets scalar uniform by it's name in shader
        /// </summary>
        public void Uniform1(string name, int value)
        {
            checkCanSetUniform(name);
            GL.Uniform1(locations[name], value);
        }
        /// <summary>
        /// Sets scalar uniform by it's name in shader
        /// </summary>
        public void Uniform1(string name, uint value)
        {
            checkCanSetUniform(name);
            GL.Uniform1(locations[name], value);
        }
        /// <summary>
        /// Sets array of scalar uniforms by it's name in shader
        /// </summary>
        public void Uniform1(string name, double[] values)
        {
            checkCanSetUniform(name);
            GL.Uniform1(locations[name], values.Length, values);
        }
        /// <summary>
        /// Sets array of scalar uniforms by it's name in shader
        /// </summary>
        public void Uniform1(string name, float[] values)
        {
            checkCanSetUniform(name);
            GL.Uniform1(locations[name], values.Length, values);
        }
        /// <summary>
        /// Sets array of scalar uniforms by it's name in shader
        /// </summary>
        public void Uniform1(string name, int[] values)
        {
            checkCanSetUniform(name);
            GL.Uniform1(locations[name], values.Length, values);
        }
        /// <summary>
        /// Sets array of scalar uniforms by it's name in shader
        /// </summary>
        public void Uniform1(string name, uint[] values)
        {
            checkCanSetUniform(name);
            GL.Uniform1(locations[name], values.Length, values);
        }
        #endregion
        #region Uniform2
        /// <summary>
        /// Set vector2 uniform by it's name in shader
        /// </summary>
        public void Uniform2(string name, Vector2 value)
        {
            checkCanSetUniform(name);
            GL.Uniform2(locations[name], value.x, value.y);
        }
        /// <summary>
        /// Set vector2 uniform by it's name in shader
        /// </summary>
        public void Uniform2(string name, Vector2f value)
        {
            checkCanSetUniform(name);
            GL.Uniform2(locations[name], value.x, value.y);
        }
        /// <summary>
        /// Set vector2 uniform by it's name in shader
        /// </summary>
        public void Uniform2(string name, double x, double y)
        {
            checkCanSetUniform(name);
            GL.Uniform2(locations[name], x, y);
        }
        /// <summary>
        /// Set vector2 uniform by it's name in shader
        /// </summary>
        public void Uniform2(string name, float x, float y)
        {
            checkCanSetUniform(name);
            GL.Uniform2(locations[name], x, y);
        }
        /// <summary>
        /// Set vector2 uniform by it's name in shader
        /// </summary>
        public void Uniform2(string name, int x, int y)
        {
            checkCanSetUniform(name);
            GL.Uniform2(locations[name], x, y);
        }
        /// <summary>
        /// Set vector2 uniform by it's name in shader
        /// </summary>
        public void Uniform2(string name, uint x, uint y)
        {
            checkCanSetUniform(name);
            GL.Uniform2(locations[name], x, y);
        }
        /// <summary>
        /// Set array of vector2 uniforms by it's name in shader
        /// </summary>
        public void Uniform2(string name, Vector2[] values)
        {
            checkCanSetUniform(name);
            double[] data = new double[values.Length * 2];
            for (int i = 0; i < values.Length; i++)
            {
                data[i * 2] = values[i].x;
                data[i * 2 + 1] = values[i].y;
            }
            GL.Uniform2(locations[name], values.Length, data);
        }
        /// <summary>
        /// Set array of vector2 uniforms by it's name in shader
        /// </summary>
        public void Uniform2(string name, Vector2f[] values)
        {
            checkCanSetUniform(name);
            float[] data = new float[values.Length * 2];
            for (int i = 0; i < values.Length; i++)
            {
                data[i * 2] = values[i].x;
                data[i * 2 + 1] = values[i].y;
            }
            GL.Uniform2(locations[name], values.Length, data);
        }
        /// <summary>
        /// Set array of vector2 uniforms by it's name in shader, each element in vector2 array represented by 2 continous elements in passed array.
        /// </summary>
        public void Uniform2(string name, double[] values)
        {
            checkCanSetUniform(name);
            if (values.Length % 2 != 0)
                throw new ArgumentException("Elements count for array of uniform2 must be a multiple of 2.");
            GL.Uniform2(locations[name], values.Length / 2, values);
        }
        /// <summary>
        /// Set array of vector2 uniforms by it's name in shader, each element in vector2 array represented by 2 continous elements in passed array.
        /// </summary>
        public void Uniform2(string name, float[] values)
        {
            checkCanSetUniform(name);
            if (values.Length % 2 != 0)
                throw new ArgumentException("Elements count for array of uniform2 must be a multiple of 2.");
            GL.Uniform2(locations[name], values.Length / 2, values);
        }
        /// <summary>
        /// Set array of vector2 uniforms by it's name in shader, each element in vector2 array represented by 2 continous elements in passed array.
        /// </summary>
        public void Uniform2(string name, int[] values)
        {
            checkCanSetUniform(name);
            if (values.Length % 2 != 0)
                throw new ArgumentException("Elements count for array of uniform2 must be a multiple of 2.");
            GL.Uniform2(locations[name], values.Length / 2, values);
        }
        /// <summary>
        /// Set array of vector2 uniforms by it's name in shader, each element in vector2 array represented by 2 continous elements in passed array.
        /// </summary>
        public void Uniform2(string name, uint[] values)
        {
            checkCanSetUniform(name);
            if (values.Length % 2 != 0)
                throw new ArgumentException("Elements count for array of uniform2 must be a multiple of 2.");
            GL.Uniform2(locations[name], values.Length / 2, values);
        }
        #endregion
        #region Uniform3
        /// <summary>
        /// Set vector3 uniform by it's name in shader
        /// </summary>
        public void Uniform3(string name, Vector3 value)
        {
            checkCanSetUniform(name);
            GL.Uniform3(locations[name], value.x, value.y, value.z);
        }
        /// <summary>
        /// Set vector3 uniform by it's name in shader
        /// </summary>
        public void Uniform3(string name, Vector3f value)
        {
            checkCanSetUniform(name);
            GL.Uniform3(locations[name], value.x, value.y, value.z);
        }
        /// <summary>
        /// Set vector3 uniform by it's name in shader
        /// </summary>
        public void Uniform3(string name, double x, double y, double z)
        {
            checkCanSetUniform(name);
            GL.Uniform3(locations[name], x, y, z);
        }
        /// <summary>
        /// Set vector3 uniform by it's name in shader
        /// </summary>
        public void Uniform3(string name, float x, float y, float z)
        {
            checkCanSetUniform(name);
            GL.Uniform3(locations[name], x, y, z);
        }
        /// <summary>
        /// Set vector3 uniform by it's name in shader
        /// </summary>
        public void Uniform3(string name, int x, int y, int z)
        {
            checkCanSetUniform(name);
            GL.Uniform3(locations[name], x, y, z);
        }
        /// <summary>
        /// Set vector3 uniform by it's name in shader
        /// </summary>
        public void Uniform3(string name, uint x, uint y, uint z)
        {
            checkCanSetUniform(name);
            GL.Uniform3(locations[name], x, y, z);
        }
        /// <summary>
        /// Set array of vector3 uniforms by it's name in shader
        /// </summary>
        public void Uniform3(string name, Vector3[] values)
        {
            checkCanSetUniform(name);
            double[] data = new double[values.Length * 3];
            for (int i = 0; i < values.Length; i++)
            {
                data[i * 3] = values[i].x;
                data[i * 3 + 1] = values[i].y;
                data[i * 3 + 2] = values[i].z;
            }
            GL.Uniform3(locations[name], values.Length, data);
        }
        /// <summary>
        /// Set array of vector3 uniforms by it's name in shader
        /// </summary>
        public void Uniform3(string name, Vector3f[] values)
        {
            checkCanSetUniform(name);
            float[] data = new float[values.Length * 3];
            for (int i = 0; i < values.Length; i++)
            {
                data[i * 3] = values[i].x;
                data[i * 3 + 1] = values[i].y;
                data[i * 3 + 2] = values[i].z;
            }
            GL.Uniform3(locations[name], values.Length, data);
        }
        /// <summary>
        /// Set array of vector3 uniforms by it's name in shader, each element in vector3 array represented by 3 continous elements in passed array.
        /// </summary>
        public void Uniform3(string name, double[] values)
        {
            checkCanSetUniform(name);
            if (values.Length % 3 != 0)
                throw new ArgumentException("Elements count for array of uniform3 must be a multiple of 3.");
            GL.Uniform3(locations[name], values.Length / 3, values);
        }
        /// <summary>
        /// Set array of vector3 uniforms by it's name in shader, each element in vector3 array represented by 3 continous elements in passed array.
        /// </summary>
        public void Uniform3(string name, float[] values)
        {
            checkCanSetUniform(name);
            if (values.Length % 3 != 0)
                throw new ArgumentException("Elements count for array of uniform3 must be a multiple of 3.");
            GL.Uniform3(locations[name], values.Length / 3, values);
        }
        /// <summary>
        /// Set array of vector3 uniforms by it's name in shader, each element in vector3 array represented by 3 continous elements in passed array.
        /// </summary>
        public void Uniform3(string name, int[] values)
        {
            checkCanSetUniform(name);
            if (values.Length % 3 != 0)
                throw new ArgumentException("Elements count for array of uniform3 must be a multiple of 3.");
            GL.Uniform3(locations[name], values.Length / 3, values);
        }
        /// <summary>
        /// Set array of vector3 uniforms by it's name in shader, each element in vector3 array represented by 3 continous elements in passed array.
        /// </summary>
        public void Uniform3(string name, uint[] values)
        {
            checkCanSetUniform(name);
            if (values.Length % 3 != 0)
                throw new ArgumentException("Elements count for array of uniform3 must be a multiple of 3.");
            GL.Uniform3(locations[name], values.Length / 3, values);
        }
        #endregion
        #region Uniform4
        /// <summary>
        /// Set vector4 uniform by it's name in shader
        /// </summary>
        public void Uniform4(string name, Vector4 value)
        {
            checkCanSetUniform(name);
            GL.Uniform4(locations[name], value.x, value.y, value.z, value.w);
        }
        /// <summary>
        /// Set vector4 uniform by it's name in shader
        /// </summary>
        public void Uniform4(string name, Vector4f value)
        {
            checkCanSetUniform(name);
            GL.Uniform4(locations[name], value.x, value.y, value.z, value.w);
        }
        /// <summary>
        /// Set vector4 uniform by it's name in shader
        /// </summary>
        public void Uniform4(string name, double x, double y, double z, double w)
        {
            checkCanSetUniform(name);
            GL.Uniform4(locations[name], x, y, z, w);
        }
        /// <summary>
        /// Set vector4 uniform by it's name in shader
        /// </summary>
        public void Uniform4(string name, float x, float y, float z, float w)
        {
            checkCanSetUniform(name);
            GL.Uniform4(locations[name], x, y, z, w);
        }
        /// <summary>
        /// Set vector4 uniform by it's name in shader
        /// </summary>
        public void Uniform4(string name, int x, int y, int z, int w)
        {
            checkCanSetUniform(name);
            GL.Uniform4(locations[name], x, y, z, w);
        }
        /// <summary>
        /// Set vector4 uniform by it's name in shader
        /// </summary>
        public void Uniform4(string name, uint x, uint y, uint z, uint w)
        {
            checkCanSetUniform(name);
            GL.Uniform4(locations[name], x, y, z, w);
        }
        /// <summary>
        /// Set array of vector4 uniforms by it's name in shader
        /// </summary>
        public void Uniform4(string name, Vector4[] values)
        {
            checkCanSetUniform(name);
            double[] data = new double[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                data[i * 4] = values[i].x;
                data[i * 4 + 1] = values[i].y;
                data[i * 4 + 2] = values[i].z;
                data[i * 4 + 3] = values[i].w;
            }
            GL.Uniform4(locations[name], values.Length, data);
        }
        /// <summary>
        /// Set array of vector4 uniforms by it's name in shader
        /// </summary>
        public void Uniform4(string name, Vector4f[] values)
        {
            checkCanSetUniform(name);
            float[] data = new float[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                data[i * 4] = values[i].x;
                data[i * 4 + 1] = values[i].y;
                data[i * 4 + 2] = values[i].z;
                data[i * 4 + 3] = values[i].w;
            }
            GL.Uniform4(locations[name], values.Length, data);
        }
        /// <summary>
        /// Set array of vector4 uniforms by it's name in shader, each element in vector4 array represented by 4 continous elements in passed array.
        /// </summary>
        public void Uniform4(string name, double[] values)
        {
            checkCanSetUniform(name);
            if (values.Length % 4 != 0)
                throw new ArgumentException("Elements count for array of uniform4 must be a multiple of 4.");
            GL.Uniform4(locations[name], values.Length / 4, values);
        }
        /// <summary>
        /// Set array of vector4 uniforms by it's name in shader, each element in vector4 array represented by 4 continous elements in passed array.
        /// </summary>
        public void Uniform4(string name, float[] values)
        {
            checkCanSetUniform(name);
            if (values.Length % 4 != 0)
                throw new ArgumentException("Elements count for array of uniform4 must be a multiple of 4.");
            GL.Uniform4(locations[name], values.Length / 4, values);
        }
        /// <summary>
        /// Set array of vector4 uniforms by it's name in shader, each element in vector4 array represented by 4 continous elements in passed array.
        /// </summary>
        public void Uniform4(string name, int[] values)
        {
            checkCanSetUniform(name);
            if (values.Length % 4 != 0)
                throw new ArgumentException("Elements count for array of uniform4 must be a multiple of 4.");
            GL.Uniform4(locations[name], values.Length / 4, values);
        }
        /// <summary>
        /// Set array of vector4 uniforms by it's name in shader, each element in vector4 array represented by 4 continous elements in passed array.
        /// </summary>
        public void Uniform4(string name, uint[] values)
        {
            checkCanSetUniform(name);
            if (values.Length % 4 != 0)
                throw new ArgumentException("Elements count for array of uniform4 must be a multiple of 4.");
            GL.Uniform4(locations[name], values.Length / 4, values);
        }
        #endregion
        #region UniformMatrix2x2
        /// <summary>
        /// Sets matrix2x2 uniform by it's name in shader
        /// </summary>
        public void UniformMatrix2x2(string name, Matrix2x2 value, bool rowMajor = true)
        {
            checkCanSetUniform(name);
            double[] data = new double[4] { value.v00, value.v01,
                                            value.v10, value.v11 };
            GL.UniformMatrix2(locations[name], 1, rowMajor, data);
        }
        /// <summary>
        /// Sets matrix2x2 uniform by it's name in shader
        /// </summary>
        public void UniformMatrix2x2(string name, Matrix2x2f value, bool rowMajor = true)
        {
            checkCanSetUniform(name);
            float[] data = new float[4] { value.v00, value.v01,
                                          value.v10, value.v11 };
            GL.UniformMatrix2(locations[name], 1, rowMajor, data);
        }
        /// <summary>
        /// Sets array of matrix2x2 uniforms by it's name in shader
        /// </summary>
        public void UniformMatrix2x2(string name, Matrix2x2[] values, bool rowMajor = true)
        {
            checkCanSetUniform(name);
            double[] data = new double[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                data[i] = values[i].v00;
                data[i + 1] = values[i].v01;
                data[i + 2] = values[i].v10;
                data[i + 3] = values[i].v11;
            }
            GL.UniformMatrix2(locations[name], values.Length, rowMajor, data);
        }
        /// <summary>
        /// Sets array of matrix2x2 uniforms by it's name in shader
        /// </summary>
        public void UniformMatrix2x2(string name, Matrix2x2f[] values, bool rowMajor = true)
        {
            checkCanSetUniform(name);
            float[] data = new float[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                data[i] = values[i].v00;
                data[i + 1] = values[i].v01;
                data[i + 2] = values[i].v10;
                data[i + 3] = values[i].v11;
            }
            GL.UniformMatrix2(locations[name], values.Length, rowMajor, data);
        }
        #endregion
        #region UniformMatrix3x3
        /// <summary>
        /// Sets matrix3x3 uniform by it's name in shader
        /// </summary>
        public void UniformMatrix3x3(string name, Matrix3x3 value, bool rowMajor = true)
        {
            checkCanSetUniform(name);
            double[] data = new double[9] { value.v00, value.v01, value.v02,
                                            value.v10, value.v11, value.v12,
                                            value.v20, value.v21, value.v22 };
            GL.UniformMatrix3(locations[name], 1, rowMajor, data);
        }
        /// <summary>
        /// Sets matrix3x3 uniform by it's name in shader
        /// </summary>
        public void UniformMatrix3x3(string name, Matrix3x3f value, bool rowMajor = true)
        {
            checkCanSetUniform(name);
            float[] data = new float[9] { value.v00, value.v01, value.v02,
                                          value.v10, value.v11, value.v12,
                                          value.v20, value.v21, value.v22 };
            GL.UniformMatrix3(locations[name], 1, rowMajor, data);
        }
        /// <summary>
        /// Sets array of matrix3x3 uniforms by it's name in shader
        /// </summary>
        public void UniformMatrix3x3(string name, Matrix3x3[] values, bool rowMajor = true)
        {
            checkCanSetUniform(name);
            double[] data = new double[values.Length * 9];
            for (int i = 0; i < values.Length; i++)
            {
                data[i] = values[i].v00;
                data[i + 1] = values[i].v01;
                data[i + 2] = values[i].v02;

                data[i + 3] = values[i].v10;
                data[i + 4] = values[i].v11;
                data[i + 5] = values[i].v12;

                data[i + 6] = values[i].v20;
                data[i + 7] = values[i].v21;
                data[i + 8] = values[i].v22;
            }
            GL.UniformMatrix3(locations[name], values.Length, rowMajor, data);
        }
        /// <summary>
        /// Sets array of matrix3x3 uniforms by it's name in shader
        /// </summary>
        public void UniformMatrix3x3(string name, Matrix3x3f[] values, bool rowMajor = true)
        {
            checkCanSetUniform(name);
            float[] data = new float[values.Length * 9];
            for (int i = 0; i < values.Length; i++)
            {
                data[i] = values[i].v00;
                data[i + 1] = values[i].v01;
                data[i + 2] = values[i].v02;

                data[i + 3] = values[i].v10;
                data[i + 4] = values[i].v11;
                data[i + 5] = values[i].v12;

                data[i + 6] = values[i].v20;
                data[i + 7] = values[i].v21;
                data[i + 8] = values[i].v22;
            }
            GL.UniformMatrix3(locations[name], values.Length, rowMajor, data);
        }
        #endregion
        #region UniformMatrix4x4
        /// <summary>
        /// Sets matrix4x4 uniform by it's name in shader
        /// </summary>
        public void UniformMatrix4x4(string name, Matrix4x4 value, bool rowMajor = true)
        {
            checkCanSetUniform(name);
            double[] data = new double[16] { value.v00, value.v01, value.v02, value.v03,
                                             value.v10, value.v11, value.v12, value.v13,
                                             value.v20, value.v21, value.v22, value.v23,
                                             value.v30, value.v31, value.v32, value.v33 };
            GL.UniformMatrix4(locations[name], 1, rowMajor, data);
        }
        /// <summary>
        /// Sets matrix4x4 uniform by it's name in shader
        /// </summary>
        public void UniformMatrix4x4(string name, Matrix4x4f value, bool rowMajor = true)
        {
            checkCanSetUniform(name);
            float[] data = new float[16] { value.v00, value.v01, value.v02, value.v03,
                                           value.v10, value.v11, value.v12, value.v13,
                                           value.v20, value.v21, value.v22, value.v23,
                                           value.v30, value.v31, value.v32, value.v33 };
            GL.UniformMatrix4(locations[name], 1, rowMajor, data);
        }
        /// <summary>
        /// Sets array of matrix4x4 uniforms by it's name in shader
        /// </summary>
        public void UniformMatrix4x4(string name, Matrix4x4[] values, bool rowMajor = true)
        {
            checkCanSetUniform(name);
            double[] data = new double[values.Length * 16];
            for (int i = 0; i < values.Length; i++)
            {
                data[i] = values[i].v00;
                data[i + 1] = values[i].v01;
                data[i + 2] = values[i].v02;
                data[i + 3] = values[i].v03;

                data[i + 4] = values[i].v10;
                data[i + 5] = values[i].v11;
                data[i + 6] = values[i].v12;
                data[i + 7] = values[i].v13;

                data[i + 8] = values[i].v20;
                data[i + 9] = values[i].v21;
                data[i + 10] = values[i].v22;
                data[i + 11] = values[i].v23;

                data[i + 12] = values[i].v30;
                data[i + 13] = values[i].v31;
                data[i + 14] = values[i].v32;
                data[i + 15] = values[i].v33;
            }
            GL.UniformMatrix4(locations[name], values.Length, rowMajor, data);
        }
        /// <summary>
        /// Sets array of matrix4x4 uniforms by it's name in shader
        /// </summary>
        public void UniformMatrix4x4(string name, Matrix4x4f[] values, bool rowMajor = true)
        {
            checkCanSetUniform(name);
            float[] data = new float[values.Length * 9];
            for (int i = 0; i < values.Length; i++)
            {
                data[i] = values[i].v00;
                data[i + 1] = values[i].v01;
                data[i + 2] = values[i].v02;
                data[i + 3] = values[i].v03;

                data[i + 4] = values[i].v10;
                data[i + 5] = values[i].v11;
                data[i + 6] = values[i].v12;
                data[i + 7] = values[i].v13;

                data[i + 8] = values[i].v20;
                data[i + 9] = values[i].v21;
                data[i + 10] = values[i].v22;
                data[i + 11] = values[i].v23;

                data[i + 12] = values[i].v30;
                data[i + 13] = values[i].v31;
                data[i + 14] = values[i].v32;
                data[i + 15] = values[i].v33;
            }
            GL.UniformMatrix4(locations[name], values.Length, rowMajor, data);
        }
        #endregion
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (IsCurrent)
                    GL.UseProgram(0);
                GL.DeleteProgram(id);

                disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
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
