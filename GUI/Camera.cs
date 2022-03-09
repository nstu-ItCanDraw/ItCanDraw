using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LinearAlgebra;

namespace GUI
{
    internal class Camera
    {
        private Vector2 position = Vector2.Zero;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                recalculateMatrixes();
            }
        }
        private int screenWidth;
        public int ScreenWidth
        {
            get
            {
                return screenWidth;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("ScreenWidth", "Screen size must be positive.");
                screenWidth = value;
                recalculateMatrixes();
            }
        }
        private int screenHeight;
        public int ScreenHeight
        {
            get
            {
                return screenHeight;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("ScreenHeight", "Screen size must be positive.");
                screenHeight = value;
                recalculateMatrixes();
            }
        }
        private double width;
        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Width", "Camera size must be positive.");
                width = value;
                recalculateMatrixes();
            }
        }
        private double height;
        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Height", "Camera size must be positive.");
                height = value;
                recalculateMatrixes();
            }
        }
        public Matrix3x3 View { get; private set; } = Matrix3x3.Identity;
        public Matrix3x3 Model { get; private set; } = Matrix3x3.Identity;
        private void recalculateMatrixes()
        {
            double halfWidth = width / 2.0;
            double halfHeight = height / 2.0;
            double invHalfWidth = 1.0 / halfWidth;
            double invHalfHeight = 1.0 / halfHeight;
            View = new Matrix3x3(invHalfWidth, 0.0, -position.x * invHalfWidth,
                                 0.0, invHalfHeight, -position.y * invHalfHeight,
                                 0.0, 0.0, 1.0);
            Model = new Matrix3x3(halfWidth, 0.0, position.x,
                                  0.0, halfHeight, position.y,
                                  0.0, 0.0, 1.0);
        }
        public Camera(int screenWidth, int screenHeight, double width, double height)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.width = width;
            this.height = height;
            recalculateMatrixes();
        }
        public Vector2 ScreenToWorld(Vector2 point)
        {
            return new Vector2((point.x / screenWidth - 0.5) * width + position.x, -(point.y / screenHeight - 0.5) * height + position.y);
        }
        public Vector2 WorldToScreen(Vector2 point)
        {
            return new Vector2(((point.x - position.x) / width + 0.5) * screenWidth, (-(point.y - position.y) / height + 0.5) * screenHeight);
        }
    }
}
