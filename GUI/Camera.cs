using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LinearAlgebra;

namespace GUI
{
    /// <summary>
    /// Contains information about camera object
    /// </summary>
    internal class Camera
    {
        private Vector2 position = Vector2.Zero;
        /// <summary>
        /// Camera position in global space
        /// </summary>
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
        /// <summary>
        /// Width of the render surface in pixels
        /// </summary>
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
        /// <summary>
        /// Height of the render surface in pixels
        /// </summary>
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
        /// <summary>
        /// Width of the camera in virtual space
        /// </summary>
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
        /// <summary>
        /// Height of the camera in virtual space
        /// </summary>
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
        /// <summary>
        /// Matrix for transforming vectors from world space to NDC (Normalized Device Coordinates)
        /// </summary>
        public Matrix3x3 View { get; private set; } = Matrix3x3.Identity;
        /// <summary>
        /// Matrix for transforming vectors from NDC (Normalized Device Coordinates) to world space
        /// </summary>
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
        /// <summary>
        /// Creates camera with specified virtual and screen size
        /// </summary>
        /// <param name="screenWidth">Width of the render surface</param>
        /// <param name="screenHeight">Height of the render surface</param>
        /// <param name="width">Camera width in virtual space</param>
        /// <param name="height">Camera height in virtual space</param>
        public Camera(int screenWidth, int screenHeight, double width, double height)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.width = width;
            this.height = height;
            recalculateMatrixes();
        }
        /// <summary>
        /// Transforms point from screen space (where top-left corner is origin, see WPF documentation for Mouse.GetPosition(IInputElement)) to virtual space
        /// </summary>
        public Vector2 ScreenToWorld(Vector2 point)
        {
            return new Vector2((point.x / screenWidth - 0.5) * width + position.x, -(point.y / screenHeight - 0.5) * height + position.y);
        }
        /// <summary>
        /// Transforms point from virtual space to screen space (where top-left corner is origin, see WPF documentation for Mouse.GetPosition(IInputElement))
        /// </summary>
        public Vector2 WorldToScreen(Vector2 point)
        {
            return new Vector2(((point.x - position.x) / width + 0.5) * screenWidth, (-(point.y - position.y) / height + 0.5) * screenHeight);
        }
    }
}
