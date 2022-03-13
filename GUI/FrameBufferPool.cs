using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL4;

namespace GUI
{
    internal class FrameBufferPool
    {       
        private List<FrameBuffer> frameBuffers = new List<FrameBuffer>();
        private Stack<FrameBuffer> freeFrameBuffers = new Stack<FrameBuffer>();
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
            foreach (FrameBuffer fb in frameBuffers)
                if (fb == curFrameBuffer)
                {
                    freeFrameBuffers.Push(curFrameBuffer);
                    break;
                }
           
            throw new ArgumentException("This FrameBuffer does not exist in this FrameBufferPool");
        }
    }
}
