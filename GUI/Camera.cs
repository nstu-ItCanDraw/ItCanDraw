using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using LinearAlgebra;

namespace GUI
{
    /// <summary>
    /// Contains information about camera object
    /// </summary>
    internal class Camera : INotifyPropertyChanged
    {
        private Vector2 position = Vector2.Zero;
        private readonly double MinPosition = -1e6;
        private readonly double MaxPosition = 1e6;
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
                OnPropertyChanged("Position");
            }
        }
        private int screenWidth = 1;
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
                OnPropertyChanged("ScreenWidth");
                OnPropertyChanged("Aspect");
                OnPropertyChanged("Width");
            }
        }
        private int screenHeight = 1;
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
                OnPropertyChanged("ScreenHeight");
                OnPropertyChanged("Aspect");
                OnPropertyChanged("Width");
            }
        }
        /// <summary>
        /// Width to Height (ScreenWidth to ScreenHeight) ratio
        /// </summary>
        public double Aspect
        {
            get
            {
                return screenWidth / (double)screenHeight;
            }
        }
        /// <summary>
        /// Width of the camera in virtual space, changing it will change Height to preserve aspect
        /// </summary>
        public double Width
        {
            get
            {
                return height * Aspect;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Width", "Camera size must be positive.");
                Height = value / Aspect;
            }
        }
        private double height = 1.0;

        /// <summary>
        /// Height of the camera in virtual space, changing it will change Width to preserve aspect
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
                OnPropertyChanged("Height");
                OnPropertyChanged("Width");
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
            double halfWidth = Width / 2.0;
            double halfHeight = height / 2.0;
            double invHalfWidth = 1.0 / halfWidth;
            double invHalfHeight = 1.0 / halfHeight;
            View = new Matrix3x3(invHalfWidth, 0.0, -position.x * invHalfWidth,
                                 0.0, invHalfHeight, -position.y * invHalfHeight,
                                 0.0, 0.0, 1.0);
            Model = new Matrix3x3(halfWidth, 0.0, position.x,
                                  0.0, halfHeight, position.y,
                                  0.0, 0.0, 1.0);
            OnPropertyChanged("View");
            OnPropertyChanged("Model");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        /// <summary>
        /// Creates camera with specified screen size and virtual height (width is calculated from aspect ratio of screen size)
        /// </summary>
        /// <param name="screenWidth">Width of the render surface</param>
        /// <param name="screenHeight">Height of the render surface</param>
        /// <param name="height">Camera height in virtual space</param>
        public Camera(int screenWidth, int screenHeight, double height)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            Height = height;
        }

        /// <summary>
        /// Transforms point from screen space (where top-left corner is origin, see WPF documentation for Mouse.GetPosition(IInputElement)) to virtual space
        /// </summary>
        /// <param name="isPoint">True if given vector is point (so offset is applied) and false if vector is a direction (so offset is ignored)</param>
        public Vector2 ScreenToWorld(Vector2 vec, bool isPoint = true)
        {
            if (isPoint)
                return new Vector2((vec.x / screenWidth - 0.5) * Width + position.x, -(vec.y / screenHeight - 0.5) * height + position.y);
            else
                return new Vector2(vec.x / screenWidth * Width, -vec.y / screenHeight * height);
        }
        /// <summary>
        /// Transforms point from virtual space to screen space (where top-left corner is origin, see WPF documentation for Mouse.GetPosition(IInputElement))
        /// </summary>
        /// <param name="isPoint">True if given vector is point (so offset is applied) and false if vector is a direction (so offset is ignored)</param>
        public Vector2 WorldToScreen(Vector2 vec, bool isPoint = true)
        {
            if (isPoint)
                return new Vector2(((vec.x - position.x) / Width + 0.5) * screenWidth, (-(vec.y - position.y) / height + 0.5) * screenHeight);
            else
                return new Vector2(vec.x / Width * screenWidth, -vec.y / height * screenHeight);
        }
        /// <summary>
        /// Zooms camera for given delta, keeping given virtual point at the same place, delta > 1 means zoom in and delta < 1 means zoom out, so, for example, value of 2 means zoom in twice and value of 0.5 means zoom out twice
        /// </summary>
        /// <param name="point">Point that will be the same in virtual space, pass camera's position too zoom into or out of camera center</param>
        /// <param name="delta">Delta to zoom for, must be positive, value > 1 means zoom in and value < 1 means zoom out</param>
        public void Zoom(Vector2 point, double delta)
        {
            if (delta <= 0)
                throw new ArgumentOutOfRangeException("delta", "Zoom delta must be positive.");

            double newHeight = height / delta;

            if (newHeight < 1e-6 || 1e6 < newHeight)
                return;

            position = point + (position - point) / delta;

            Position = new Vector2(Math.Min(MaxPosition, Math.Max(MinPosition, position.x)), Math.Min(MaxPosition, Math.Max(MinPosition, position.y)));

            Height = newHeight;
            recalculateMatrixes();
        }
    }
}
