using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Geometry;
using LinearAlgebra;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            fuuu();
            renderControl.Focusable = true;
            KeyDown += (sender, args) => { if (renderControl.IsFocused) renderControl.OnKeyDown(sender, args); };
            KeyUp += (sender, args) => { if (renderControl.IsFocused) renderControl.OnKeyUp(sender, args); };
        }

        void fuuu()
        {
            IGeometry obj1 = FigureFactory.CreateEllipse(1, 2, new Vector2(10, 10));
            obj1.Transform.RotationDegrees = 91;

            int y = 0;
        }
    }
}
