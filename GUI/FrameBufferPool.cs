using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace GUI
{
    internal class FrameBufferPool
    {       
        private List<FrameBuffer> inUseFrameBuffers = new List<FrameBuffer>();
        private Stack<FrameBuffer> freeFrameBuffers = new Stack<FrameBuffer>();

        public int Width { get; private set; }
        public int Height { get; private set; }
        public TextureType TextureType { get; private set; }
        public TextureWrapMode WrapMode { get; private set; }
        public TextureMinFilter MinFilter { get; private set; }
        public TextureMagFilter MagFilter { get; private set; }

        public FrameBufferPool(int frameBufferAmmount, int width, int height, TextureType textureType = TextureType.RGBAColor, TextureWrapMode wrapMode = TextureWrapMode.Repeat,
            TextureMinFilter minFilter = TextureMinFilter.Nearest, TextureMagFilter magFilter = TextureMagFilter.Nearest)
        {
            Width = width;
            Height = height;
            TextureType = textureType;
            WrapMode = wrapMode;
            MinFilter = minFilter;
            MagFilter = magFilter;

            for (int i = 0; i < frameBufferAmmount; i++)
            { 
                inUseFrameBuffers.Add(new FrameBuffer(new Texture2D(Width, Height, TextureType, WrapMode, MinFilter, MagFilter)));
                freeFrameBuffers.Push(inUseFrameBuffers[i]);
            }
        }
        public FrameBuffer PoolGetFreeFrameBuffer()
        {
            if (freeFrameBuffers.Count == 0)
            {
                for (int i = 0; i < inUseFrameBuffers.Count; i++)
                {
                    inUseFrameBuffers.Add(new FrameBuffer(new Texture2D(Width, Height, TextureType, WrapMode, MinFilter, MagFilter)));
                    freeFrameBuffers.Push(inUseFrameBuffers[i]);
                }
            }

            return freeFrameBuffers.Pop();
        }
        public void PoolReleaseFrameBuffer(FrameBuffer curFrameBuffer)
        {
            bool exists = false;
            foreach (FrameBuffer fb in inUseFrameBuffers)
                if (fb == curFrameBuffer)
                {
                    freeFrameBuffers.Push(curFrameBuffer);
                    exists = true;
                    break;
                }
            if (!exists)
                throw new ArgumentException("This FrameBuffer does not exist in FrameBufferPool");
        }
    }
}
