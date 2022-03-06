using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    /// <summary>
    /// Represents local space in terms of position, rotation and scale
    /// </summary>
    public class Transform : INotifyPropertyChanged
    {
        private Vector2 localPosition;
        private double localRotation;
        private Vector2 localScale;
        private Transform parent;
        /// <summary>
        /// Position of local space in parent space
        /// </summary>
        public Vector2 LocalPosition 
        { 
            get 
            { 
                return localPosition; 
            } 
            set 
            { 
                localPosition = value; 
                recalculateMatrixes(); 
                OnPropertyChanged("LocalPosition");
                OnPropertyChanged("Position");
            } 
        }
        /// <summary>
        /// Rotation of local space in parent space in radians
        /// </summary>
        public double LocalRotation 
        { 
            get 
            { 
                return localRotation; 
            } 
            set 
            {
                localRotation = value; 
                recalculateMatrixes(); 
                OnPropertyChanged("LocalRotation");
                OnPropertyChanged("LocalRotationDegrees");
                OnPropertyChanged("Rotation");
                OnPropertyChanged("RotationDegrees");
            }
        }
        /// <summary>
        /// Rotation of local space in parent space in degrees
        /// </summary>
        public double LocalRotationDegrees 
        { 
            get 
            { 
                return localRotation / Math.PI * 180.0; 
            } 
            set 
            {
                localRotation = value / 180.0 * Math.PI; 
                recalculateMatrixes();
                OnPropertyChanged("LocalRotation");
                OnPropertyChanged("LocalRotationDegrees");
                OnPropertyChanged("Rotation");
                OnPropertyChanged("RotationDegrees");
            } 
        }
        /// <summary>
        /// Scale of local space in parent space
        /// </summary>
        public Vector2 LocalScale 
        { 
            get 
            { 
                return localScale; 
            } 
            set 
            { 
                localScale = value; 
                recalculateMatrixes();
                OnPropertyChanged("LocalScale");
            }
        }
        /// <summary>
        /// Position of local space in global space
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return parent == null ? localPosition : (parent.Model * new Vector3(localPosition, 1.0)).xy;
            }
            set
            {
                if (parent == null)
                    localPosition = value;
                else
                    localPosition = (parent.View * new Vector3(value, 1.0)).xy;

                recalculateMatrixes();
                OnPropertyChanged("LocalPosition");
                OnPropertyChanged("Position");
            }
        }
        /// <summary>
        /// Rotation of local space in global space in radians
        /// </summary>
        public double Rotation
        {
            get
            {
                return parent == null ? localRotation : parent.Rotation + localRotation;
            }
            set
            {
                localRotation = parent == null ? value : value - parent.Rotation;

                recalculateMatrixes();
                OnPropertyChanged("LocalRotation");
                OnPropertyChanged("LocalRotationDegrees");
                OnPropertyChanged("Rotation");
                OnPropertyChanged("RotationDegrees");
            }
        }
        /// <summary>
        /// Rotation of local space in global space in degrees
        /// </summary>
        public double RotationDegrees
        {
            get
            {
                return (parent == null ? localRotation : parent.Rotation + localRotation) / Math.PI * 180.0;
            }
            set
            {
                localRotation = parent == null ? (value / 180.0 * Math.PI) : (value / 180.0 * Math.PI) - parent.Rotation;

                recalculateMatrixes();
                OnPropertyChanged("LocalRotation");
                OnPropertyChanged("LocalRotationDegrees");
                OnPropertyChanged("Rotation");
                OnPropertyChanged("RotationDegrees");
            }
        }
        /// <summary>
        /// Transform of parent space
        /// </summary>
        public Transform Parent 
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
                recalculateMatrixes();
                OnPropertyChanged("Parent");
                OnPropertyChanged("Position");
                OnPropertyChanged("Rotation");
                OnPropertyChanged("RotationDegrees");
            }
        }
        /// <summary>
        /// Local-to-parent transformation matrix
        /// </summary>
        public Matrix3x3 LocalModel { get; private set; }
        /// <summary>
        /// Parent-to-local transformation matrix
        /// </summary>
        public Matrix3x3 LocalView { get; private set; }
        /// <summary>
        /// Local-to-global transformation matrix
        /// </summary>
        public Matrix3x3 Model
        {
            get
            {
                return parent == null ? LocalModel : parent.Model * LocalModel;
            }
        }
        /// <summary>
        /// Global-to-local transformation matrix
        /// </summary>
        public Matrix3x3 View
        {
            get
            {
                return parent == null ? LocalView : LocalView * parent.View;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private void recalculateMatrixes()
        {
            // Model * vec => Move^(-1) * Rotate^(-1) * Scale^(-1) * vec
            double sin = Math.Sin(localRotation);
            double cos = Math.Cos(localRotation);
            LocalModel = new Matrix3x3(cos * localScale.x, -sin * localScale.y, localPosition.x,
                                       sin * localScale.x, cos * localScale.y, localPosition.y,
                                       0.0, 0.0, 1.0);

            // View * vec => Scale * Rotate * Move * vec
            double invScaleX = 1.0 / localScale.x;
            double invScaleY = 1.0 / localScale.y;
            Matrix3x3 mat = new Matrix3x3(cos * invScaleX, sin * invScaleX, 0.0,
                                          -sin * invScaleY, cos * invScaleY, 0.0,
                                          0.0, 0.0, 1.0);

            mat.v02 = -mat.v00 * localPosition.x - mat.v01 * localPosition.y;
            mat.v12 = -mat.v10 * localPosition.x - mat.v11 * localPosition.y;

            LocalView = mat;

            OnPropertyChanged("LocalModel");
            OnPropertyChanged("LocalView");
            OnPropertyChanged("Model");
            OnPropertyChanged("View");
        }
        public Transform()
        {
            localPosition = Vector2.Zero;
            localScale = new Vector2(1, 1);
            localRotation = 0;
        }
        public Transform(Vector2 localPosition, Vector2 localScale, double localRotation)
        {
            this.localPosition = localPosition;
            this.localRotation = localRotation;
            this.localScale = localScale;
        }
        public Transform(Vector2 localPosition, double localRotation, Vector2 localScale)
        {
            this.localPosition = localPosition;
            this.localRotation = localRotation;
            this.localScale = localScale;
        }
    }
}
