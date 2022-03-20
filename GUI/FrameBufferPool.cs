using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL4;

namespace GUI
{
    internal class FrameBufferPool : IDisposable
    {       
        private List<FrameBuffer> frameBuffers = new List<FrameBuffer>();
        private Stack<FrameBuffer> freeFrameBuffers = new Stack<FrameBuffer>();
        private bool disposed;

        public int PoolCapacity 
        { 
            get
            {
                return frameBuffers.Count();
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public TextureType TextureType { get; private set; }
        public TextureWrapMode WrapMode { get; private set; }
        public TextureMinFilter MinFilter { get; private set; }
        public TextureMagFilter MagFilter { get; private set; }

        public FrameBufferPool(int capacity, int width, int height, TextureType textureType = TextureType.RGBAColor, TextureWrapMode wrapMode = TextureWrapMode.Repeat,
            TextureMinFilter minFilter = TextureMinFilter.Nearest, TextureMagFilter magFilter = TextureMagFilter.Nearest)
        {
            Width = width;
            Height = height;
            TextureType = textureType;
            WrapMode = wrapMode;
            MinFilter = minFilter;
            MagFilter = magFilter;

            for (int i = 0; i < capacity; i++)
            { 
                frameBuffers.Add(new FrameBuffer(new Texture2D(Width, Height, TextureType, WrapMode, MinFilter, MagFilter)));
                freeFrameBuffers.Push(frameBuffers[i]);
            }
        }
        public FrameBuffer Get()
        {
            if (disposed)
                throw new ObjectDisposedException("FrameBufferPool");

            if (freeFrameBuffers.Count == 0)
            {
                int capacity = PoolCapacity;
                for (int i = 0; i < capacity; i++)
                {
                    frameBuffers.Add(new FrameBuffer(new Texture2D(Width, Height, TextureType, WrapMode, MinFilter, MagFilter)));
                    freeFrameBuffers.Push(frameBuffers[^1]);
                }
            }

            return freeFrameBuffers.Pop();
        }
        public void Release(FrameBuffer curFrameBuffer)
        {
            if (disposed)
                throw new ObjectDisposedException("FrameBufferPool");

            foreach (FrameBuffer fb in frameBuffers)
                if (fb == curFrameBuffer)
                {
                    freeFrameBuffers.Push(curFrameBuffer);
                    return;
                }
            throw new ArgumentException("This FrameBuffer does not exist in this FrameBufferPool");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                foreach (FrameBuffer fb in frameBuffers)
                {
                    fb.Dispose();
                    fb.ColorTexture.Dispose();
                }

                disposed = true;
            }
        }

        ~FrameBufferPool()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
