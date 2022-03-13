using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace GUI
{
    internal class FrameBufferPool
    {       
        private int _maxCapacity;
        private int _frameBufferAmmount;
        private List<FrameBuffer> inUseFrameBuffers = new List<FrameBuffer>();
        private Stack<FrameBuffer> freeFrameBuffers = new Stack<FrameBuffer>();
        private int FrameBufferAmmount
        {
            get
            {
                return inUseFrameBuffers.Count;
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public TextureType TextureType { get; private set; }
        public TextureWrapMode WrapMode { get; private set; }
        public TextureMinFilter MinFilter { get; private set; }
        public TextureMagFilter MagFilter { get; private set; }

        public FrameBufferPool(int frameBufferAmmount, int maxCapacity, int width, int height, TextureType textureType = TextureType.RGBAColor, TextureWrapMode wrapMode = TextureWrapMode.Repeat,
            TextureMinFilter minFilter = TextureMinFilter.Nearest, TextureMagFilter magFilter = TextureMagFilter.Nearest)
        {
            _frameBufferAmmount = frameBufferAmmount;
            _maxCapacity = maxCapacity;

            Width = width;
            Height = height;
            TextureType = textureType;
            WrapMode = wrapMode;
            MinFilter = minFilter;
            MagFilter = magFilter;

            for (int i = 0; i < _frameBufferAmmount; i++)
                inUseFrameBuffers.Add(new FrameBuffer(new Texture2D(Width, Height, TextureType, WrapMode, MinFilter, MagFilter)));

            for (int i = _frameBufferAmmount; i < _maxCapacity; i++)
                freeFrameBuffers.Push(new FrameBuffer(new Texture2D(Width, Height, TextureType, WrapMode, MinFilter, MagFilter)));
        }
        public FrameBuffer PoolGetFreeFrameBuffer()
        {
            if (FrameBufferAmmount > _maxCapacity)
            {
                _maxCapacity *= 2;
                for (int i = _frameBufferAmmount; i < _maxCapacity; i++)
                    freeFrameBuffers.Push(new FrameBuffer(new Texture2D(Width, Height, TextureType, WrapMode, MinFilter, MagFilter)));
            }

            inUseFrameBuffers.Add(freeFrameBuffers.Pop());
            return inUseFrameBuffers[^1];
        }
        public void PoolReleaseFrameBuffer(FrameBuffer curFrameBuffer)
        {
            bool exists = false;
            foreach (FrameBuffer fb in inUseFrameBuffers)
                if (fb == curFrameBuffer)
                    exists = true;
            if (!exists)
                throw new ArgumentException("This FrameBuffer does not exist in FrameBufferPool");

            inUseFrameBuffers.Remove(curFrameBuffer);
            freeFrameBuffers.Push(curFrameBuffer);
        }
    }
}
